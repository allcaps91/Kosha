using System;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using System.IO;
using ComDbB; //DB연결

namespace ComBase
{
    public class clsVbfunc
    {
        [DllImport("imm32.dll")]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);
        [DllImport("Imm32.dll")]
        public static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);
        [DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
        private static extern int ImmGetCompositionStringW(IntPtr hIMC, int dwIndex, byte[] lpBuf, int dwBufLen);

        /// <summary>
        /// 입력상태를 한글로 전환한다.
        /// </summary>
        /// <param name="handle"></param>
        public static void cvtToHan(IntPtr handle)
        {
            int readType = 10;

            IntPtr hIMC = ImmGetContext(handle);

            try
            {
                int strLen = ImmGetCompositionStringW(hIMC, readType, null, 0);

                byte[] buffer = new byte[strLen];

                ImmGetCompositionStringW(hIMC, readType, buffer, strLen);
            }
            finally
            {
                ImmReleaseContext(handle, hIMC);
            }
        }

        /// <summary>
        /// 입력상태를 영문으로 전환한다.
        /// </summary>
        /// <param name="handle"></param>
        public static void cvtToEng(IntPtr handle)
        {
            int readType = 0;

            IntPtr hIMC = ImmGetContext(handle);

            try
            {
                int strLen = ImmGetCompositionStringW(hIMC, readType, null, 0);

                byte[] buffer = new byte[strLen];

                ImmGetCompositionStringW(hIMC, readType, buffer, strLen);
            }
            finally
            {
                ImmReleaseContext(handle, hIMC);
            }
        }

        /// <summary>
        /// Form 내의 모든 TextBox의 내용을 삭제한다.
        /// </summary>
        /// <param name="Frm"></param>
        public static void TextBoxClear(Form Frm)
        {
            Control[] conAll = ComFunc.GetAllControls(Frm);

            if (conAll == null)
                return;

            foreach (Control conVal in conAll)
            {
                if (conVal is TextBox)
                {
                    ((TextBox)conVal).Text = "";
                }
            }
        }

        public static void Msg(String strGb, String strToD)
        {
            string strHead = "";
            string strFoot = "";
            string strMsg = "";

            string strt = "";
            string strD = "";

            switch (strToD)
            {
                case "1":
                    strD = "이양재 과장";
                    strt = "3 4 9";
                    break;
                case "2":
                    strD = "전종윤 주임";
                    strt = "6 8 7";
                    break;
                case "3":
                    strD = "윤조연 주임";
                    strt = "6 8 8";
                    break;
                case "4":
                    strD = "김현욱 주임";
                    strt = "3 5 1";
                    break;
                case "5":
                    strD = "김민철 주임";
                    strt = "3 5 2";
                    break;
                case "6":
                    strD = "김성준     ";
                    strt = "3 5 0";
                    break;
            }

            strHead = "주       의" + ComNum.VBLF + ComNum.VBLF;
            strHead = strHead + "▶=====================================================================◀" + ComNum.VBLF + ComNum.VBLF;

            strFoot = "▶=====================================================================◀" + ComNum.VBLF + ComNum.VBLF;
            strFoot = strFoot + "          담 당 자: " + strD + ComNum.VBLF + ComNum.VBLF;
            strFoot = strFoot + "          원내전화: " + strt + ComNum.VBLF + ComNum.VBLF;
            strFoot = strFoot + "▶=====================================================================◀" + ComNum.VBLF + ComNum.VBLF;

            if (strGb == "1")
            {
                strMsg = "   1. 이 프로그램은 UPGRADE 버전입니다. " + ComNum.VBLF +
                         "   2. 작업 중 오류가 발생 할 수 있습니다. " + ComNum.VBLF +
                         "   3. 오류 발생시 현재 오류 화면을 그대로 둡니다. " + ComNum.VBLF +
                         "   4. 전산정보팀 담당자에게 연락합니다. " + ComNum.VBLF;
            }

            ComFunc.MsgBox(strHead + strMsg + strFoot, "주의");

        }

        /// <summary>
        /// yyyy-MM-dd 형식으로 사용 해야함.
        /// </summary>
        /// <param name="strToFdate"> yyyy-MM-dd 형식으로 </param>
        /// <param name="strToTdate"> yyyy-MM-dd 형식으로 </param>
        /// <returns></returns>
        public static string DateGyesanYYMM(string strToFdate, string strToTdate)
        {
            string strVal = "";
            string strFDate = "";
            string strTDate = "";
            string strFYear = "";
            string strTYear = "";
            string strFMonth = "";
            string strTMonth = "";

            int intResYear = 0;
            int intResMonth = 0;

            if (strToFdate.Length != 10 || VB.IsDate(strToFdate) == false
                || (strToTdate.Length != 10) || VB.IsDate(strToTdate) == false)
            {
                return strVal;
            }

            if (Convert.ToDateTime(strToFdate) > Convert.ToDateTime(strToTdate))
            {
                return strVal;
            }

            strFDate = strToFdate.Replace("-", "");
            strTDate = strToTdate.Replace("-", "");

            strFYear = VB.Left(strToFdate, 4);
            strTYear = VB.Left(strToTdate, 4);

            strFMonth = VB.Mid(strFDate, 5, 2);
            strTMonth = VB.Mid(strTDate, 5, 2);

            intResYear = Convert.ToInt32(VB.Val(strTYear) - VB.Val(strFYear));
            intResMonth = Convert.ToInt32(VB.Val(strTMonth) - VB.Val(strFMonth));

            if (VB.Right(strToFdate, 2) == "01"
                && strToTdate == Convert.ToDateTime(VB.Left(strToTdate, 8) + "01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"))
            {
                intResMonth = intResMonth + 1;
            }

            if (intResMonth >= 12)
            {
                intResYear = intResYear + 1;
                intResMonth = intResMonth - 12;
            }

            if (intResMonth < 0)
            {
                intResYear = intResYear - 1;
                intResMonth = intResMonth + 12;
            }

            strVal = intResYear.ToString("0") + "-" + intResMonth.ToString("00");

            return strVal;
        }

        /// <summary>
        /// 일수 간격에 휴일 수 뺀 일수
        /// </summary>
        /// <param name="strFdate"></param>
        /// <param name="strTdate"></param>
        /// <returns></returns>
        public static string DateNarSuWD(PsmhDb pDbCon, string strFdate, string strTdate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strVal = "";
            int intDay = 0;
            DataTable dt = null;

            if (strFdate.Length != 10 || VB.IsDate(strFdate) == false
                || (strTdate.Length != 10) || VB.IsDate(strTdate) == false)
            {
                return strVal;
            }

            if (Convert.ToDateTime(strFdate) > Convert.ToDateTime(strTdate))
            {
                return strVal;
            }

            intDay = Convert.ToInt32(VB.DateDiff("d", Convert.ToDateTime(strFdate), Convert.ToDateTime(strTdate)));

            try
            {
                //'휴일일수는 제외함
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(JobDate,'DD') ILJA FROM BAS_JOB";
                SQL = SQL + ComNum.VBLF + " WHERE JobDate >= TO_DATE('" + strFdate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND JobDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND HolyDay = '*' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                intDay = intDay - dt.Rows.Count;
                strVal = Convert.ToString(intDay);

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                strVal = "";
            }

            return strVal;
        }

        /// <summary>
        /// 초를 받아서 일,시,분,초 값으로 리턴
        /// </summary>
        /// <param name="argSS"></param>
        /// <returns></returns>
        public static string DATE_ILSU_SS(double argSS)
        {
            double nMin = 0;
            double nHour = 0;
            double nSS = 0;
            double nDay = 0;

            nSS = argSS;

            nMin = VB.Fix((int)argSS / 60);

            nDay = VB.Fix((int)nMin / 1440);

            nMin = nMin % 1440;

            nHour = VB.Fix((int)nMin / 60);

            nMin = nMin % 60;

            nSS = nSS % 60;

            if (nDay == 0 && nHour == 0)
            {
                return nMin.ToString("00") + "분" + nSS.ToString("00") + "초";
            }
            else if (nDay == 0)
            {
                return nHour.ToString("00") + "시간 " + nMin.ToString("00") + "분 " + nSS.ToString("00") + "초";
            }
            else
            {
                return nDay + "일 " + nHour.ToString("00") + "시간 " + nMin.ToString("00") + "분" + nSS.ToString("00") + "초";
            }
        }

        /// <summary>
        /// tick값을 받아서 일,시,분으로 리턴
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static string DATE_ILSU_TICK(long ticks)
        {
            double nMin = 0;
            double nHour = 0;
            double nDay = 0;

            TimeSpan t = new TimeSpan(ticks);

            nMin = t.Minutes;

            nDay = t.Days;

            nHour = t.Hours;

            if (nDay == 0)
            {
                return nHour.ToString("#0") + "시간 " + nMin.ToString("#0") + "분";
            }
            else
            {
                return nDay + "일 " + nHour.ToString("#0") + "시간 " + nMin.ToString("#0") + "분";
            }
        }

        /// <summary>
        /// 분을 받아서 일,시,분으로 리턴
        /// </summary>
        /// <param name="argMin"></param>
        /// <returns></returns>
        public static string DATE_ILSU_MIN(double argMin)
        {
            double nMin = 0;
            double nHour = 0;
            double nDay = 0;

            nMin = argMin;

            nDay = VB.Fix((int)nMin / 1440);

            nMin = nMin % 1440;

            nHour = VB.Fix((int)nMin / 60);

            nMin = nMin % 60;

            if (nDay == 0)
            {
                return nHour.ToString("#0") + "시간 " + nMin.ToString("#0") + "분";
            }
            else
            {
                return nDay + "일 " + nHour.ToString("#0") + "시간 " + nMin.ToString("#0") + "분";
            }
        }

        /// <summary>
        /// 분을 받아서 일, 시:분 으로 반환
        /// </summary>
        /// <param name="argMin"></param>
        /// <returns></returns>
        public static string DATE_ILSU_MIN2(double argMin)
        {
            double nMin = 0;
            double nHour = 0;
            double nDay = 0;

            nMin = argMin;

            nDay = VB.Fix((int)nMin / 1440);

            nHour = VB.Fix((int)nMin / 60);

            nMin = nMin % 60;

            return nDay + "일 " + nHour.ToString("00") + ":" + nMin.ToString("00") + "";
        }

        /// <summary>
        ///  날짜 간격을 HH:mm 으로 리턴함
        /// </summary>
        /// <param name="strFdate">yyyy-MM-dd HH:mm:ss 형식으로</param>
        /// <param name="strTdate">yyyy-MM-dd HH:mm:ss 형식으로</param>
        /// <returns></returns>
        public static string DateTimeHour(string strFdate, string strTdate)
        {
            int intmm = 0;
            string strVal = "";
            string strHH = "";
            string strmm = "";

            if (VB.IsDate(strFdate) == false || VB.IsDate(strTdate) == false)
            {
                return strVal;
            }

            if (Convert.ToDateTime(strFdate) > Convert.ToDateTime(strTdate))
            {
                return strVal;
            }

            intmm = Convert.ToInt32(VB.DateDiff("n", Convert.ToDateTime(strFdate), Convert.ToDateTime(strTdate)));

            strHH = (intmm / 60).ToString("00");

            strmm = (intmm % 60).ToString("00");

            strVal = strHH + ":" + strmm;

            return strVal;
        }

        /// <summary>
        /// yyyyMM 날짜 형식의 날짜에 월을 가감 해준다.
        /// </summary>
        /// <param name="strDateYYMM">yyyyMM 형식의 날짜</param>
        /// <param name="intAdd">더하서나 뺄 개월 수</param>
        /// <returns></returns>
        public static string DateYYMMAdd(string strDateYYMM, int intAdd)
        {
            string strVal = "";
            string strDate = ComFunc.FormatStrToDate(strDateYYMM + "01", "D");

            if (VB.IsDate(strDate) == false)
                return strVal;

            strVal = Convert.ToDateTime(strDate).AddMonths(intAdd).ToString("yyyyMM");

            return strVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strDate"> yyyy-MM-dd 형식으로 넣어야함.</param>
        /// <returns></returns>
        public static string SangHanMaGam(string strDate)
        {
            string strEndDate = "";
            string strDD = "";
            string strMM = "";
            string strEndMM = "";
            string strEndYear = "";

            if (VB.IsDate(strDate) == false)
                return strEndDate;

            if (Convert.ToDateTime(strDate) <= Convert.ToDateTime("2004-06-30"))
            {
                strDate = "2004-07-01";
            }
            //상한제 마감일자 2009년부터 1년기간임
            //                2008년은 6개월기간임

            if (Convert.ToDateTime(strDate) <= Convert.ToDateTime("2009-01-01"))
            {
                strMM = VB.Mid(strDate, 6, 2);
                strDD = VB.Right(strDate, 2);
                strEndYear = VB.Left(strDate, 4);
                strEndDate = strEndYear + "-12-31";
            }
            else if (Convert.ToDateTime(strDate) <= Convert.ToDateTime("2008-12-31"))
            {
                strMM = VB.Mid(strDate, 6, 2);
                strDD = VB.Right(strDate, 2);
                strEndYear = VB.Left(strDate, 4);
                strEndMM = (VB.Val(strMM) + 6).ToString("00");
                if (VB.Val(strEndMM) >= 13)
                {
                    strEndMM = (VB.Val(strEndMM) - 12).ToString("00");
                    strEndYear = Convert.ToString(VB.Val(strEndYear) + 1);
                }

                if (strDD == "01")
                {
                    strEndDate = Convert.ToDateTime(strEndYear + "-" + strEndMM + "-" + strDD).AddDays(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    strEndDate = strEndYear + "-" + strEndMM + "-" + strDD;

                    if ((Convert.ToDateTime("2017" + "-" + strEndMM + "-" + strDD) >= Convert.ToDateTime("2017-02-29")
                        && Convert.ToDateTime("2017" + "-" + strEndMM + "-" + strDD) <= Convert.ToDateTime("2017-02-31"))
                        || strEndMM + strDD == "0431"
                        || strEndMM + strDD == "0631"
                        || strEndMM + strDD == "0931"
                        || strEndMM + strDD == "1131")
                    {
                        strEndDate = LastDay(Convert.ToInt32(strEndYear), Convert.ToInt32(strEndMM));
                    }
                    else
                    {
                        strEndDate = Convert.ToDateTime(strEndDate).AddDays(-1).ToString("yyyy-MM-dd");
                    }
                }

            }
            return strEndDate;
        }

        /// <summary>
        /// 
        /// </summary> 해당 년도 월의 마지말 날자를 yyyy-MM-dd 형식으로 리턴한다.
        /// <param name="intYYYYDate">년도 4자리</param>
        /// <param name="intMMDate">월</param>
        /// <returns></returns>
        public static string LastDay(int intYYYYDate, int intMMDate)
        {
            if (VB.IsDate(intYYYYDate.ToString() + "-" + intMMDate.ToString("00") + "-" + "01") == false)
                return "";

            return intYYYYDate.ToString("0000") + "-" + intMMDate.ToString("00") + "-" + DateTime.DaysInMonth(intYYYYDate, intMMDate).ToString("00");
        }



        /// <summary>
        /// 현재시점의 재원자
        /// clsPublic.GstrIpdCnt360, clsPublic.GstrIpdCnt390 에 값이 들어감
        /// </summary>
        /// <param name="strFlag"> 'ARGFLAG =1 '재원자에 당일 퇴원자중 계산완료된자만 제외</param>
        public static string GetJeWon(PsmhDb pDbCon, string strFlag)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strVal = "";
            DataTable dt = null;

            clsPublic.GstrIpdCnt360 = ""; //'340 인원조정 2013-07-03
            clsPublic.GstrIpdCnt390 = ""; //'370 385 390

            try
            {
                SQL = "";
                SQL = " SELECT COUNT(*) CNT FROM ADMIN.IPD_NEW_MASTER ";

                if (strFlag == "1")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE JDATE =TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ROOMCODE <> '359' ";    //'재활낮병동
                    SQL = SQL + ComNum.VBLF + "   AND SUBSTR(PANO,1,7) <> '8100000'  "; //'지병환자 제외
                    SQL = SQL + ComNum.VBLF + "   AND ( (JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND OUTDATE IS NULL) OR OUTDATE >= TRUNC(SYSDATE) ) ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE GbSTS NOT IN ( '1','7','9' ) ";
                    SQL = SQL + ComNum.VBLF + "   AND ((JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND OUTDATE IS NULL) OR OUTDATE >= TRUNC(SYSDATE) ) ";
                    SQL = SQL + ComNum.VBLF + "   AND RoomCode<>'359' "; //'재활낮병동
                    SQL = SQL + ComNum.VBLF + "   AND  ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + "   AND SUBSTR(Pano,1,7) <> '8100000'  "; //'지병환자 제외
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) >= 390)  //'2013-07-11 원무팀장
                    {
                        clsPublic.GstrIpdCnt390 = "OK";
                    }

                    strVal = VB.Space(15) + "재원:" + dt.Rows[0]["CNT"].ToString().Trim() + " 명";

                    if (VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) < 360)  //'2013-07-11 원무팀장
                    {
                        clsPublic.GstrIpdCnt360 = "OK";
                    }
                }

                dt.Dispose();
                dt = null;


                //'입원 예약자 READ
                SQL = " SELECT COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM  ADMIN.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE OUTDATE = TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND Amset4 <> '3' "; //'정상애기
                SQL = SQL + ComNum.VBLF + "   AND RoomCode<>'359' "; //'재활낮병동
                SQL = SQL + ComNum.VBLF + "   AND Pano <  '90000000' "; //'지병환자 제외
                SQL = SQL + ComNum.VBLF + "   AND SUBSTR(Pano,1,7) <> '8100000' "; //'전산실연습

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = strVal + " 퇴원중:" + dt.Rows[0]["CNT"].ToString().Trim() + " 명";
                }

                dt.Dispose();
                dt = null;

                //'잔여병상조회 (중환자실, 인큐베이터제외)
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  SUM(TCNT - ICNT) CNT FROM ( ";
                SQL = SQL + ComNum.VBLF + "   SELECT  'T' GBN,  SUM(TBED) TCNT, 0 ICNT FROM ADMIN.BAS_ROOM ";
                SQL = SQL + ComNum.VBLF + "   WHERE WARDCODE NOT IN ('NR','IQ','32','33','35') ";
                SQL = SQL + ComNum.VBLF + "   UNION ";
                SQL = SQL + ComNum.VBLF + "   SELECT   'I', 0 TCNT ,  COUNT(*) ICNT  ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.IPD_NEW_MASTER  ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO <  '90000000'  ";
                SQL = SQL + ComNum.VBLF + "  AND SUBSTR(Pano,1,7) <> '8100000'  ";
                SQL = SQL + ComNum.VBLF + "  AND JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND OUTDATE IS NULL  ";
                SQL = SQL + ComNum.VBLF + "  AND GBSTS = '0'  ";
                SQL = SQL + ComNum.VBLF + "  AND ROOMCODE NOT IN ('564','565')   ";
                SQL = SQL + ComNum.VBLF + "  AND WARDCODE NOT IN ('IQ','NR','32','33','35') ";
                SQL = SQL + ComNum.VBLF + "      )  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = "잔여병상: " + dt.Rows[0]["CNT"].ToString().Trim() + " Bed";

                    if (VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) >= 50)
                    {
                        clsPublic.GstrIpdCnt390 = "OK";
                    }
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }

            return strVal;
        }




        /// <summary>
        ///  yyyy-MM-dd 형식의 날짜 문자를 넣어주면 해당 요일을 반환한다. ex)"일요일"
        /// </summary>
        /// <param name="strDate">yyyy-MM-dd 형식</param>
        /// <returns></returns>
        public static string GetYoIl(string strDate)
        {
            string strVal = "";

            if (VB.IsDate(strDate) == false)
                return strVal;

            switch (Convert.ToInt32(Convert.ToDateTime(strDate).DayOfWeek))
            {
                case 0:
                    strVal = "일요일";
                    break;
                case 1:
                    strVal = "월요일";
                    break;
                case 2:
                    strVal = "화요일";
                    break;
                case 3:
                    strVal = "수요일";
                    break;
                case 4:
                    strVal = "목요일";
                    break;
                case 5:
                    strVal = "금요일";
                    break;
                case 6:
                    strVal = "토요일";
                    break;
                default:
                    strVal = "";
                    break;
            }

            return strVal;
        }

        public static string GetNextActDate(PsmhDb pDbCon)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(JOBDATE,'YYYY-MM-DD') JOBDATE ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_JOB ";
                SQL = SQL + ComNum.VBLF + " WHERE JOBDATE > TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "   AND HOLYDAY <> '*' ";
                SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["JOBDATE"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetPassName(PsmhDb pDbCon, string strSaBun)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Val(strSaBun) == 0)
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT USERNAME FROM ADMIN.BAS_USER ";
                SQL = SQL + ComNum.VBLF + " WHERE IDNUMBER =" + VB.Val(strSaBun) + " ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["USERNAME"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetBASDoctorName(PsmhDb pDbCon, string strDrCode)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strDrCode.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DRNAME FROM ADMIN.BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DRCODE ='" + strDrCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["DRNAME"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetOCSDoctorName(PsmhDb pDbCon, string strDrSabun)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strDrSabun.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DRNAME FROM ADMIN.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN ='" + strDrSabun + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["DRNAME"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetOCSDoctorDRBUNHO(PsmhDb pDbCon, string strSabun)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Val(strSabun) == 0)
                return strVal;

            try
            {
                SQL = "";
                SQL = "SELECT DRBUNHO FROM ADMIN.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE SABUN='" + strSabun + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["DRBUNHO"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        /// <summary>
        /// 사번을 의사 성명을 반환해준다.
        /// </summary>
        /// <param name="strDrName"></param>
        /// <returns></returns>
        public static string GetOCSDoctorCode(PsmhDb pDbCon, string strDrName)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strDrName.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = "SELECT DRCODE FROM ADMIN.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE DrName='" + strDrName + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["DRCODE"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetOCSDrNameSabun(PsmhDb pDbCon, string strSabun)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Val(strSabun) == 0)
                return strVal;

            try
            {
                SQL = "";
                SQL = "SELECT DRNAME FROM ADMIN.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE SABUN ='" + strSabun + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["DRNAME"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetOCSDrCodeDrName(PsmhDb pDbCon, string strCode)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strCode.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = "SELECT DRNAME FROM ADMIN.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE DrCode='" + strCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["DRNAME"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetOCSDrCodeSabun(PsmhDb pDbCon, string strDrCode)
        {
            string strVal = "";
            DataTable dtVB = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strDrCode.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = "SELECT SABUN FROM ADMIN.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE DRCODE='" + strDrCode.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dtVB, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dtVB.Rows.Count > 0)
                {
                    strVal = dtVB.Rows[0]["SABUN"].ToString().Trim();
                }

                dtVB.Dispose();
                dtVB = null;
            }
            catch (Exception ex)
            {
                if (dtVB != null)
                {
                    dtVB.Dispose();
                    dtVB = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }


        public static string GetOCSDrCodeNaSabun(PsmhDb pDbCon, string strDrName, string strDepcode = "")
        {
            string strVal = "";
            DataTable dtVB = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strDrName.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = "SELECT SABUN FROM ADMIN.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE DRNAME='" + strDrName.Trim() + "' ";

                if (strDepcode != "")
                {
                    SQL = SQL + ComNum.VBLF + "AND DEPTCODE='" + strDepcode.Trim() + "' ";
                }


                SqlErr = clsDB.GetDataTableEx(ref dtVB, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dtVB.Rows.Count > 0)
                {
                    strVal = dtVB.Rows[0]["SABUN"].ToString().Trim();
                }

                dtVB.Dispose();
                dtVB = null;
            }
            catch (Exception ex)
            {
                if (dtVB != null)
                {
                    dtVB.Dispose();
                    dtVB = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }


        public static string GetOCSDrDeptcodeSabun(PsmhDb pDbCon, string strDrSabun)
        {
            string strVal = "";
            DataTable dtVB = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            

            try
            {
                SQL = "";
                SQL = "SELECT DEPTCODE FROM ADMIN.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE SABUN='" + strDrSabun.Trim() + "' ";

          
                SqlErr = clsDB.GetDataTableEx(ref dtVB, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dtVB.Rows.Count > 0)
                {
                    strVal = dtVB.Rows[0]["DEPTCODE"].ToString().Trim();
                }

                dtVB.Dispose();
                dtVB = null;
            }
            catch (Exception ex)
            {
                if (dtVB != null)
                {
                    dtVB.Dispose();
                    dtVB = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetOCSDrBunhoDrCode(PsmhDb pDbCon, string strDrCode)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strDrCode.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DRBUNHO FROM ADMIN.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DrCode='" + strDrCode.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["DRBUNHO"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetBASClinicDeptNameK(PsmhDb pDbCon, string strDeptCode)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strDeptCode.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DEPTNAMEK FROM ADMIN.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE = '" + strDeptCode.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["DEPTNAMEK"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strJuMin"></param>
        /// <param name="strGBN"> 
        ///   1: 신종플루예방접종
        ///      구분
        ///      예약일 
        ///   2:yy/MM/dd 
        ///   나머지 yyyy-MM-dd 형식으로 반환</param>
        /// <returns></returns>
        public static string GetFlueReserved(PsmhDb pDbCon, string strJuMin, string strGBN)
        {
            //'암호화 적용 됨

            //'ARGGBN 구분에 따라 RETURN 값이 다름
            //'1: 메세지 표시
            //'2: 예약날짜축약
            //'기타: 예약날짜

            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strJuminHash = "";
            string strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D");
            DataTable dt = null;

            if (strJuMin.Trim() == "" || strGBN.Trim() == "")
                return strVal;

            try
            {
                strJuminHash = clsAES.AES(strJuMin.Replace("-", ""));

                //'플루예방접종 예약내역 조회
                SQL = " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') RDATE1, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(RDATE,'YY/MM/DD') RDATE2, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(RDATE2,'YYYY-MM-DD') RDATE_TO, GBN ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.VACCINE_RESERVED  ";
                SQL = SQL + ComNum.VBLF + "    WHERE ( JUMIN ='" + strJuMin.Replace("-", "") + "'  or JUMIN3 = '" + strJuminHash + "')  "; //'주민번호 - 입력 해도 처리;
                SQL = SQL + ComNum.VBLF + "    AND ( VDATE IS NULL OR VDATE =TRUNC(SYSDATE) )";
                SQL = SQL + ComNum.VBLF + "    AND CANDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "    AND ( RDATE >=TRUNC(SYSDATE - 14)  or  RDATE2 >=TRUNC(SYSDATE - 14) )";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (strGBN == "1")
                    {
                        if (dt.Rows[0]["RDATE_TO"].ToString().Trim() != "")
                        {
                            if (Convert.ToDateTime(dt.Rows[0]["RDATE_TO"].ToString().Trim()) > Convert.ToDateTime(strDate))
                            {
                                strVal = "신종플루예방접종" + ComNum.VBLF +
                                        "구  분  :" + dt.Rows[0]["GBN"].ToString().Trim() + ComNum.VBLF +
                                        "예약일  :" + dt.Rows[0]["RDATE1"].ToString().Trim() + (dt.Rows[0]["RDATE_TO"].ToString().Trim() != "" ? "~" + dt.Rows[0]["RDATE_TO"].ToString().Trim() : "");
                            }
                            else
                            {
                                strVal = "신종플루예방접종 (일자경과) " + ComNum.VBLF +
                                        "구  분  :" + dt.Rows[0]["GBN"].ToString().Trim() + ComNum.VBLF +
                                        "예약일  :" + dt.Rows[0]["RDATE1"].ToString().Trim() + (dt.Rows[0]["RDATE_TO"].ToString().Trim() != "" ? "~" + dt.Rows[0]["RDATE_TO"].ToString().Trim() : "");
                            }

                        }
                        else
                        {
                            if (Convert.ToDateTime(dt.Rows[0]["RDATE1"].ToString().Trim()) > Convert.ToDateTime(strDate))
                            {
                                strVal = "신종플루예방접종" + ComNum.VBLF +
                                       "구  분  :" + dt.Rows[0]["GBN"].ToString().Trim() + ComNum.VBLF +
                                       "예약일  :" + dt.Rows[0]["RDATE1"].ToString().Trim() + (dt.Rows[0]["RDATE_TO"].ToString().Trim() != "" ? "~" + dt.Rows[0]["RDATE_TO"].ToString().Trim() : "");
                            }
                            else
                            {
                                strVal = "신종플루예방접종 (일자경과) " + ComNum.VBLF +
                                        "구  분  :" + dt.Rows[0]["GBN"].ToString().Trim() + ComNum.VBLF +
                                        "예약일  :" + dt.Rows[0]["RDATE1"].ToString().Trim() + (dt.Rows[0]["RDATE_TO"].ToString().Trim() != "" ? "~" + dt.Rows[0]["RDATE_TO"].ToString().Trim() : "");
                            }
                        }

                    }
                    else if (strGBN == "2")
                    {
                        strVal = dt.Rows[0]["RDATE2"].ToString().Trim();
                    }
                    else
                    {
                        strVal = dt.Rows[0]["RDATE1"].ToString().Trim();
                    }
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetInSaName(PsmhDb pDbCon, string strSabun)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Val(strSabun) == 0)
                return strVal;

            string strSabun1 = VB.Val(strSabun).ToString("#00000");

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT KORNAME , BUSE FROM ADMIN.INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + strSabun1 + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ( TOIDAY IS NULL OR TOIDAY < TRUNC(SYSDATE) )";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["KORNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                if (strVal == "")
                {
                    switch (strSabun)
                    {
                        case "4444":
                            strVal = "영양팀";
                            break;
                        case "111":
                            strVal = "일반검진";
                            break;
                        case "222":
                            strVal = "종합검진";
                            break;
                        case "333":
                            strVal = "기록실";
                            break;
                        case "555":
                            strVal = "예약자부도";
                            break;
                        case "2222":
                            strVal = "HD수납";
                            break;
                        case "123":
                            strVal = "전화공용";
                            break;
                        case "500":
                            strVal = "외래상담용";
                            break;
                        case "4349":
                            strVal = "전산정보팀";
                            break;
                        case "04349":
                            strVal = "전산정보팀";
                            break;
                        case "6666":
                            strVal = "진료의뢰";
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetPatientName(PsmhDb pDbCon, string strPtNo)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Val(strPtNo) == 0)
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SNAME FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO='" + strPtNo + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["SNAME"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetPatientName_New(PsmhDb pDbCon, string strPtNo)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            
            // Full NAME 사용에 따른 추가 

            if (VB.Val(strPtNo) == 0)
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT nvl(sname2,sname) SNAME FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO='" + strPtNo + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["SNAME"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        /// <summary>
        /// strGNB 1 성명 이니셜만 표시 예) KIM K. D.
        ///        2 첫글짜만대문자     예) Kim Kil Dong
        ///        3 성명 이니셜만 표시 예) KKD
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strGNB"></param>
        /// <returns></returns>
        public static string GetPatientEName(PsmhDb pDbCon, string strPtNo, string strGNB)
        {
            string strVal = "";
            string strEName = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Val(strPtNo) == 0)
                return strVal;

            if (strGNB.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SNAME FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO='" + strPtNo + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["SNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strVal != "")
                {
                    for (int i = 1; i <= strVal.Length; i++)
                    {
                        SQL = "";
                        SQL = "SELECT ENGNAME FROM ADMIN.BAS_Z300FONT ";
                        SQL = SQL + ComNum.VBLF + " WHERE Z300CODE = '" + VB.Mid(strVal, i, 1) + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return strVal;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            if (strGNB == "1")
                            {
                                if (i == 1)
                                {
                                    strEName = strEName + " " + dt.Rows[0]["ENGNAME"].ToString().Trim();
                                }
                                else
                                {
                                    strEName = strEName + " " + VB.Left(dt.Rows[0]["ENGNAME"].ToString().Trim(), 1) + ".";
                                }
                            }
                            else if (strGNB == "2")
                            {
                                strEName = strEName + " " + VB.Left(dt.Rows[0]["ENGNAME"].ToString().Trim(), 1).ToUpper() + VB.Mid(dt.Rows[0]["ENGNAME"].ToString().Trim(), 2, dt.Rows[0]["ENGNAME"].ToString().Trim().Length).ToLower();
                            }
                            else if (strGNB == "3")
                            {
                                if (i == 1)
                                {
                                    strEName = strEName + VB.Left(dt.Rows[0]["ENGNAME"].ToString().Trim(), 1);
                                }
                                else
                                {
                                    strEName = strEName + VB.Left(dt.Rows[0]["ENGNAME"].ToString().Trim(), 1);
                                }
                            }
                            else
                            {
                                strEName = strEName + " " + dt.Rows[0]["ENGNAME"].ToString().Trim();
                            }
                        }
                        dt.Dispose();
                        dt = null;

                    }

                    strVal = strEName;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string READ_AGE_GESAN(PsmhDb pDbCon, string strPtNo)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Val(strPtNo) == 0)
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT JUMIN1 || JUMIN2 AS JUMIN FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO='" + strPtNo + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = ComFunc.AgeCalc(pDbCon, dt.Rows[0]["JUMIN"].ToString().Trim()).ToString();
                }

                dt.Dispose();
                dt = null;

                return strVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                return strVal;
            }
        }


        /// <summary>
        /// VbFunction READ_AGE_GESAN
        /// 2018-04-17 박웅규
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtNo"></param>
        /// <returns></returns>
        public static string READ_AGE_GESAN_Ex(PsmhDb pDbCon, string strPtNo)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Val(strPtNo) == 0)
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT JUMIN1 || JUMIN2 AS JUMIN FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO='" + strPtNo + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = ComFunc.AgeCalc(pDbCon, dt.Rows[0]["JUMIN"].ToString().Trim()).ToString();
                }

                dt.Dispose();
                dt = null;

                return strVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                return strVal;
            }
        }

        /// <summary>
        /// 1세 미만의 경우 개월수 표시, 1세 이상일 경우 기존 나이 계산 로직 사용
        /// 2018-12-24 박웅규
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtNo"></param>
        /// <returns></returns>
        public static string READ_AGE_GESAN_Ex1(PsmhDb pDbCon, string strPtNo)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strJumin = "";
            string strMonth = "";
            string strIlsu = "";

            if (VB.Val(strPtNo) == 0)
                return strVal;

            try
            {
                SQL = " SELECT  TO_DATE(TODAY,'YYYY-MM-DD') - BIRTH + 1 ILSU, ";
                //SQL = SQL + ComNum.VBLF + "     MONTHS_BETWEEN(TO_DATE(TODAY,'YYYY-MM-DD'), BIRTH) MONTH, ";
                //SQL = SQL + ComNum.VBLF + "     ROUND(MONTHS_BETWEEN(TO_DATE(TODAY,'YYYY-MM-DD'), BIRTH)) AS MONTH, ";
                SQL = SQL + ComNum.VBLF + "     FLOOR(MONTHS_BETWEEN(TO_DATE(TODAY,'YYYY-MM-DD'), BIRTH)) AS MONTH, "; //2020-07-20 의뢰서 간호부 기준으로 개월수 표시(반올림이 아니라 버림 처리)
                SQL = SQL + ComNum.VBLF + "     JUMIN1 ||JUMIN2 JUMIN";
                SQL = SQL + ComNum.VBLF + " FROM (";
                SQL = SQL + ComNum.VBLF + "     SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD') TODAY, ";
                SQL = SQL + ComNum.VBLF + "         CASE TRIM(SUBSTR(JUMIN2, 1, 1))";
                SQL = SQL + ComNum.VBLF + "             WHEN '1' THEN '19'";
                SQL = SQL + ComNum.VBLF + "             WHEN '2' THEN '19'";
                SQL = SQL + ComNum.VBLF + "             WHEN '5' THEN '19'";
                SQL = SQL + ComNum.VBLF + "             WHEN '6' THEN '19'";
                SQL = SQL + ComNum.VBLF + "             WHEN '3' THEN '20'";
                SQL = SQL + ComNum.VBLF + "             WHEN '4' THEN '20'";
                SQL = SQL + ComNum.VBLF + "             WHEN '7' THEN '20'";
                SQL = SQL + ComNum.VBLF + "             WHEN '8' THEN '20'";
                SQL = SQL + ComNum.VBLF + "             WHEN '9' THEN '18'";
                SQL = SQL + ComNum.VBLF + "             WHEN '0' THEN '18'";
                SQL = SQL + ComNum.VBLF + "         END NYEAR, ";
                SQL = SQL + ComNum.VBLF + "         JUMIN1, JUMIN2, BIRTH ";
                SQL = SQL + ComNum.VBLF + "     From ADMIN.BAS_PATIENT";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPtNo + "')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["MONTH"].ToString().Trim() != "")
                    {
                        strMonth = dt.Rows[0]["MONTH"].ToString().Trim();
                        strJumin = dt.Rows[0]["JUMIN"].ToString().Trim();
                        strIlsu = dt.Rows[0]["ILSU"].ToString().Trim();
                        dt.Dispose();
                        dt = null;

                        if (VB.Val(strIlsu) <= 30)
                        {
                            strVal = strIlsu + "d";
                        }
                        else if (VB.Val(strMonth) < 12 && VB.Val(strIlsu) >= 31)
                        {
                            strVal = strMonth + "m";
                        }
                        else
                        {
                            string strGDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D");
                            strVal = (AGE_YEAR_GESAN2(strJumin, strGDate)).ToString();
                        }
                    }
                    else
                    {
                        strJumin = dt.Rows[0]["JUMIN"].ToString().Trim();
                        dt.Dispose();
                        dt = null;
                        string strGDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D");
                        strVal = (AGE_YEAR_GESAN2(strJumin, strGDate)).ToString();
                    }
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                }

                return strVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                return strVal;
            }
        }

        public static int AGE_YEAR_GESAN2(string strJumin, string strGDate)
        {
            int intYear1 = 0;   //기준년도
            string strMMDD1 = "";   //기준월일
            int intYear2 = 0;   //출생년도
            string strMMDD2 = "";   //출생월일
            int intAge = 0;
            string strSysDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            int rtnVal = 0;

            //기준일자는 반드시 'YYYY - MM - DD' Type이어야 하며,
            //아니면 현재일자를 기준일자로 처리함
            if (VB.Len(strGDate) == 10)
            {
                intYear1 = (int)VB.Val(VB.Left(strGDate, 4));
                strMMDD1 = VB.Mid(strGDate, 6, 2) + VB.Right(strGDate, 2);
            }
            else
            {
                intYear1 = (int)VB.Val(VB.Left(strSysDate, 4));
                strMMDD1 = VB.Mid(strSysDate, 6, 2) + VB.Right(strSysDate, 2);
            }

            //성별구분으로 세기를 판단함
            intYear2 = (int)VB.Val(VB.Left(strJumin, 2));

            switch (VB.Mid(strJumin + VB.Space(7), 7, 1))
            {
                case "0":
                case "9":
                    intYear2 = intYear2 + 1800;
                    break;
                case "1":
                case "2":
                    intYear2 = intYear2 + 1900;
                    break;
                case "3":
                case "4":
                    intYear2 = intYear2 + 2000;
                    break;
                case "5":
                case "6":
                    intYear2 = intYear2 + 1900;
                    break;
                case "7":
                case "8":
                    //외국인경우 2000년도나 1900년도나 같기때문에 100 이상인 외국인은 없다고 가정함
                    //7:외국인(남) 8:외국인(여)
                    if (VB.Val(VB.Left(strJumin, 2)) >= 0 && VB.Val(VB.Left(strJumin, 2)) <= VB.Val(VB.Mid(strSysDate, 3, 2)))
                    {
                        intYear2 = intYear2 + 2000;
                    }
                    else
                    {
                        intYear2 = intYear2 + 1900;
                    }
                    break;
                default:
                    intYear2 = intYear2 + 1900;
                    break;
            }

            //나이를 계산함(기준년도 - 출생년도)
            rtnVal = intYear1 - intYear2;

            //월일을 비교하여 출생월일이 기준월일보다 크면 나이에서 (-1)처리
            strMMDD2 = VB.Mid(strJumin + VB.Space(4), 3, 4);

            if (VB.Val(strMMDD2) > VB.Val(strMMDD1))
            {
                rtnVal = rtnVal - 1;
            }

            //나이가 110살보다 크거나 0보다 적으면 10살로 처리함
            //2017-12-29 120살로 변경함... ㅡ.ㅡ;;;
            if (rtnVal > 120 || rtnVal < 0)
            {
                rtnVal = 10;
            }

            if (rtnVal == 0)
            {
                rtnVal = (int)(((VB.Val(VB.Left(strMMDD1, 2)) - VB.Val(VB.Left(strMMDD2, 2))) + ((intYear1 - intYear2) * 12)) / 10);
            }

            return rtnVal;
        }

        public static string READ_SEX(PsmhDb pDbCon, string strPtNo)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Val(strPtNo) == 0)
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SEX FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO='" + strPtNo + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["SEX"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return strVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                return strVal;
            }
        }

        public static string GetPatientHPhone(PsmhDb pDbCon, string strPtNo)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Val(strPtNo) == 0)
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT HPHONE FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO='" + strPtNo + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["HPHONE"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetBiName(string strBi)
        {
            string strVal = "";

            switch (strBi.Trim())
            {
                case "11":
                case "12":
                case "13":
                    strVal = "건강보험";
                    break;
                case "21":
                    strVal = "의료급여1종";
                    break;
                case "22":
                    strVal = "의료급여2종";
                    break;
                case "23":
                    strVal = "의료급여3종";
                    break;
                case "24":
                    strVal = "행려환자";
                    break;
                case "31":
                    strVal = "산재";
                    break;
                case "32":
                    strVal = "공상";
                    break;
                case "33":
                    strVal = "산재공상";
                    break;
                case "41":
                    strVal = "공단100%";
                    break;
                case "42":
                    strVal = "직장100%";
                    break;
                case "43":
                    strVal = "지역100%";
                    break;
                case "44":
                    strVal = "가족계획";
                    break;
                case "51":
                    strVal = "일반";
                    break;
                case "52":
                    strVal = "TA보험";
                    break;
                case "53":
                    strVal = "계약처";
                    break;
                case "54":
                    strVal = "미확인";
                    break;
                case "55":
                    strVal = "TA일반";
                    break;
                default:
                    strVal = "";
                    break;
            }

            return strVal;
        }

        public static string GetBASBuSe(PsmhDb pDbCon, string strBuCode)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strBuCode.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT NAME, SNAME FROM ADMIN.BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + " WHERE BUCODE = '" + strBuCode.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["SNAME"].ToString().Trim() != "")
                    {
                        strVal = dt.Rows[0]["SNAME"].ToString().Trim();
                    }
                    else
                    {
                        strVal = dt.Rows[0]["NAME"].ToString().Trim();
                    }
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetGamF(PsmhDb pDbCon, string strPtNo, string strRtnGbn)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strJuminHash = "";
            string strSYSDATE = ComQuery.CurrentDateTime(pDbCon, "D");

            if (VB.Val(strPtNo) == 0)
                return strVal;


            clsAES.Read_Jumin_AES(pDbCon, strPtNo); //'VB60_NEW\BASEFILE\VBAES.BAS 에 있음

            strJuminHash = clsAES.AES(clsAES.GstrAesJumin1 + clsAES.GstrAesJumin2);

            try
            {

                //'직원 감액 조회
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.GamSaBun, B.NAME, A.GAMMESSAGE  ";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.BAS_GAMF A , ADMIN.BAS_BUSE B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.GAMSOSOK = B.BUCODE(+) ";
                SQL = SQL + ComNum.VBLF + "    AND A.GAMJUMIN3 = '" + strJuminHash + "'  ";  //'2014-05-27
                SQL = SQL + ComNum.VBLF + "    AND (A.GAMEND >= TO_DATE('" + strSYSDATE + "','YYYY-MM-DD') OR A.GAMEND IS NULL) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (strRtnGbn == "SABUN")
                    {
                        strVal = dt.Rows[0]["GAMMESSAGE"].ToString().Trim() + " ( " + dt.Rows[0]["GamSaBun"].ToString().Trim() + " ) ";
                    }
                    else
                    {
                        strVal = "▶" + dt.Rows[0]["NAME"].ToString().Trim() + " " + dt.Rows[0]["GAMMESSAGE"].ToString().Trim() + "◀ ";
                    }

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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetJikupName(PsmhDb pDbCon, string strCode)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strCode.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT NAME FROM ADMIN.ETC_CSINFO_CODE ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = '4' "; //'직업
                SQL = SQL + ComNum.VBLF + "   AND CODE = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["NAME"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetJiCodeName(PsmhDb pDbCon, string strCode)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strCode.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT JINAME FROM ADMIN.BAS_AREA ";
                SQL = SQL + ComNum.VBLF + " WHERE JICODE='" + strCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["JINAME"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetBASMail(PsmhDb pDbCon, string strCode)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strCode.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT MAILJUSO FROM ADMIN.BAS_MAILNEW ";
                SQL = SQL + ComNum.VBLF + " WHERE MAILCODE='" + strCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["MAILJUSO"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string JobStop(PsmhDb pDbCon)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT REMARK FROM ADMIN.ETC_JOB_STOP";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE <= SYSDATE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["REMARK"].ToString().Trim();
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
                clsDB.SaveSqlErrLog("함수명 : " + "JobStop" + ComNum.VBLF + ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show("함수명 : " + "JobStop" + ComNum.VBLF + ex.Message);
            }

            return strVal;
        }

        public static string QuotationChange(string strQuotation)
        {
            strQuotation = strQuotation.Replace("'", "`");

            strQuotation = strQuotation.Replace("·", ".");

            return strQuotation;
        }

        /// <summary>
        /// 콤보박스에 년도를 세팅한다.
        /// </summary>
        /// <param name="cboVal"></param>
        /// <param name="intCboLen"></param>
        /// <param name="strGubun">
        /// 1 : yyyy
        /// 2 : yyyy년도
        /// </param>
        public static void SetCboDateYY(PsmhDb pDbCon, ComboBox cboVal, int intCboLen, string strGubun)
        {
            DateTime dateVal;
            int i = 0;
            int intYY = 0;

            cboVal.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVal.Items.Clear();
            dateVal = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D"));

            intYY = dateVal.Year;

            for (i = 0; i < intCboLen; i++)
            {
                if (strGubun == "1")
                {
                    cboVal.Items.Add((intYY - i).ToString("0000"));
                }
                else if (strGubun == "2")
                {
                    cboVal.Items.Add((intYY - i).ToString("0000") + "년도");
                }
            }
            cboVal.SelectedIndex = 0;
        }

        /// <summary>
        /// 행당 콤보박스에 년월을 세팅 해준다.
        /// </summary>
        /// <param name="cboVal">해당 콤보박스</param>
        /// <param name="intCboLen">콤보박스에 세팅할 아이템 갯수 갯수만큼 월수가 줄어 든다.</param>
        /// <param name="strSdate"> 최하 년월일. 없으면 "" 으로 입력
        /// </param>
        /// <param name="strGubun">
        /// 0 :  yyyy-MM
        /// 1 :  yyyy년 MM월분
        /// 2 :  yyyy년 MM월
        /// </param>
        public static void SetCboDate(PsmhDb pDbCon, ComboBox cboVal, int intCboLen, string strSdate, string strGubun)
        {
            DateTime dateVal;
            int i = 0;
            int intYY = 0;
            int intMM = 0;

            cboVal.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVal.Items.Clear();
            dateVal = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D"));

            intYY = dateVal.Year;
            intMM = dateVal.Month;

            for (i = 0; i < intCboLen; i++)
            {
                if (strGubun == "0")
                {
                    cboVal.Items.Add(intYY.ToString("0000") + "-" + intMM.ToString("00"));
                }
                else if (strGubun == "1")
                {
                    cboVal.Items.Add(intYY.ToString("0000") + "년 " + intMM.ToString("00") + "월분");
                }
                else if (strGubun == "2")
                {
                    cboVal.Items.Add(intYY.ToString("0000") + "년 " + intMM.ToString("00") + "월");
                }

                intMM = intMM - 1;
                if (intMM == 0)
                {
                    intMM = 12;
                    intYY = intYY - 1;
                }

                if (strSdate != "")
                {
                    if (VB.Val(strSdate) > VB.Val(Convert.ToString(intYY) + Convert.ToString(intMM)))
                    {
                        return;
                    }
                }
            }
            cboVal.SelectedIndex = 0;
        }

        /// <summary>
        /// 해당 콤보박스에 년월을 세팅 해준다.
        /// <param name="cboVal"></param>
        /// <param name="strSdate">콤보박스에 세팅할 아이템 갯수 갯수만큼 월수가 줄어 든다</param>
        /// <param name="intCboLen">최하 년월일. 없으면 "" 으로 입력</param>
        /// <param name="strGubun">
        /// 0 :  yyyy-MM
        /// 1 :  yyyy년 MM월분
        /// 2 :  yyyy년 MM월
        /// </param>
        /// <param name="startM">현재월 기준으로 처음시작 월 설정 예 5, -5  현재기준 5개월후가 시작월, 현재기준 -5개월이 시작월 </param>
        /// 2017.07.03 윤조연
        /// </summary>
        public static void SetCboDate(PsmhDb pDbCon, ComboBox cboVal, int intCboLen, string strSdate, string strGubun, int startM)
        {
            DateTime dateVal;
            int i = 0;
            int intYY = 0;
            int intMM = 0;

            cboVal.DropDownStyle = ComboBoxStyle.DropDownList;
            cboVal.Items.Clear();

            dateVal = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D"));

            if (startM != 0)
            {
                dateVal = Convert.ToDateTime(dateVal.AddMonths(startM));
            }

            intYY = dateVal.Year;
            intMM = dateVal.Month;


            for (i = 0; i < intCboLen; i++)
            {
                if (strGubun == "0")
                {
                    cboVal.Items.Add(intYY.ToString("0000") + "-" + intMM.ToString("00"));
                }
                else if (strGubun == "1")
                {
                    cboVal.Items.Add(intYY.ToString("0000") + "년 " + intMM.ToString("00") + "월분");
                }
                else if (strGubun == "2")
                {
                    cboVal.Items.Add(intYY.ToString("0000") + "년 " + intMM.ToString("00") + "월");
                }

                intMM = intMM - 1;
                if (intMM == 0)
                {
                    intMM = 12;
                    intYY = intYY - 1;
                }

                if (strSdate != "")
                {
                    if (VB.Val(strSdate) > VB.Val(Convert.ToString(intYY) + Convert.ToString(intMM)))
                    {
                        return;
                    }
                }
            }

            cboVal.SelectedIndex = 0;
        }

        /// <summary>
        /// 스트링이 시분 (00:00) 형태 인지 확인한다.
        /// </summary>
        /// <param name="strTime"></param>
        /// <returns></returns>
        public static bool TimeFormatCheck(string strTime)
        {
            bool bolVal = false;

            //'공란은 정상으로 처리
            if (strTime == "")
                return true;

            if (strTime.Length != 5)
                return bolVal;

            if (VB.Mid(strTime, 3, 1) != ":")
                return bolVal;

            if (VB.Val(VB.Split(strTime, ":")[0]) > 23)
                return bolVal;

            if (VB.Val(VB.Split(strTime, ":")[1]) > 59)
                return bolVal;

            bolVal = true;

            return bolVal;
        }

        /// <summary>
        /// ' Usage : strGunsok = Gunsok_YearMonthDay_Gesan("2002-01-01","2003-05-20")
        /// ' 결과  : 근속년수 3자리,근속월수 2자리, 근속일수 2자리 (ex.0120510:12년5개월10일)
        /// </summary>
        /// <param name="strFrDate">yyyy-MM-dd 형식으로</param>
        /// <param name="strEndDate">yyyy-MM-dd 형식으로</param>
        /// <returns></returns>
        public static string GunsokYearMonthDayGesan(string strFrDate, string strEndDate)
        {
            string strVal = "0000000";
            int intYEAR = 0;
            int intMonth = 0;
            int intDay = 0;
            DateTime dateFr; //입사일자
            DateTime dateEnd; //퇴직일자
            DateTime dateLast; //월의 마지막 일자

            if (VB.IsDate(strFrDate) == false || VB.IsDate(strEndDate) == false)
                return strVal;

            dateFr = Convert.ToDateTime(strFrDate);
            dateEnd = Convert.ToDateTime(strEndDate);

            //'퇴사일자가 입사일보다 적으면 0년 0개월 0일을 Return
            if (dateEnd < dateFr)
                return strVal;

            //'근속월수=(퇴사년도 - 입사년도) * 12개월
            intYEAR = (dateEnd.Year - dateFr.Year) * 12;

            //'근속월수=근속월수 + (퇴사월 - 입사월)
            intMonth = intYEAR + (dateEnd.Month - dateFr.Month);

            dateLast = Convert.ToDateTime(LastDay(dateEnd.Year, dateEnd.Month)); //퇴직일의 마지막 일수

            //'월초입사(해당월의 1일에 입사) -> ex: 2003.1.1~2003.2.10
            if (dateFr.Day == 1)
            {
                //'월말퇴사이면
                if (dateEnd == dateLast)
                {
                    intMonth = intMonth + 1;
                    intDay = 0;
                }
                else
                {
                    intDay = dateEnd.Day;
                }
            }
            //'월말일자로 퇴사한 경우 -> ex: 2003.1.3~2003.2.28
            else if (dateEnd == dateLast)
            {
                dateLast = Convert.ToDateTime(LastDay(dateFr.Year, dateFr.Month)); //'입사월의 마지막 일자
                intDay = dateLast.Day - dateFr.Day + 1;  //'근무일수는 입사월의 근무일수
            }
            else
            {
                dateLast = Convert.ToDateTime(LastDay(dateFr.Year, dateFr.Month)); //'입사월의 마지막 일자
                //'입사월의 근무일  + 퇴사월의 근무일
                intDay = (dateLast.Day - dateFr.Day + 1) + dateEnd.Day;
                intMonth = intMonth - 1;
            }

            intYEAR = VB.Fix(intMonth / 12);
            intMonth = intMonth - (intYEAR * 12);

            strVal = intYEAR.ToString("000") + intMonth.ToString("00") + intDay.ToString("00");

            return strVal;
        }

        /// <summary>
        /// '거래처명칭
        /// '약국에사 사용하는 READ_AIS_LTD는 모두 변경바랍니다.
        /// </summary>
        /// <returns></returns>
        public static string GetAISLTD(PsmhDb pDbCon, string strCode)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strCode.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = "SELECT NAME FROM ADMIN.AIS_LTD ";
                SQL = SQL + ComNum.VBLF + "WHERE LTDCODE='" + strCode.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["NAME"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetJUSOJiCode(PsmhDb pDbCon, string strJiCode)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strJiCode.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = "SELECT JINAME FROM BAS_AREA WHERE JICODE = '" + strJiCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["JINAME"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetJuSo(PsmhDb pDbCon, string strZipCode1, string strZipCode2) // 주소
        {
            //' 공용함수는 수정 불가임.
            //' 추가는 가능함.
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strZipCode1.Trim() == "" || strZipCode2.Trim() == "")
                return strVal;

            try
            {
                SQL = "SELECT MAILJUSO";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_MAILNEW      ";
                SQL = SQL + ComNum.VBLF + "WHERE MAILCODE = '" + strZipCode1.Trim() + strZipCode2.Trim() + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["MAILJUSO"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetRoadJuSo(PsmhDb pDbCon, string strBuildNo) // 주소
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strBuildNo == null) return strVal;
            if (strBuildNo.Trim() == "") return strVal;

            try
            {
                SQL = "";
                SQL = " SELECT ZIPNAME1 || ' ' || ZIPNAME2 || ' ' || ZIPNAME3 AS HEADJUSO,";
                SQL = SQL + ComNum.VBLF + "  ROADNAME, BUILDNAME, BUN1, BUN2 ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_ZIPS_ROAD ";
                SQL = SQL + ComNum.VBLF + " WHERE BUILDNO = '" + strBuildNo + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["HEADJUSO"].ToString().Trim();
                    strVal = strVal + dt.Rows[0]["ROADNAME"].ToString().Trim();
                    strVal = strVal + dt.Rows[0]["BUN1"].ToString().Trim();

                    if (VB.Val(dt.Rows[0]["BUN2"].ToString().Trim()) > 0)
                    {
                        strVal = strVal + "-" + dt.Rows[0]["BUN2"].ToString().Trim();
                    }

                    if (dt.Rows[0]["BUILDNAME"].ToString().Trim() != "")
                    {
                        strVal = strVal + "" + dt.Rows[0]["BUILDNAME"].ToString().Trim();
                    }
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetIpAddressOeacle(PsmhDb pDbCon)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT SYS_CONTEXT ('USERENV', 'IP_ADDRESS')  IP  FROM DUAL";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["IP"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetBiName2(string strData)
        {
            string strVal = "";

            switch (strData.Trim())
            {
                case "11":
                    strVal = "공무원";
                    break;
                case "12":
                    strVal = "직장";
                    break;
                case "13":
                    strVal = "지역";
                    break;
                case "21":
                    strVal = "급여1종";
                    break;
                case "22":
                    strVal = "급여2종";
                    break;
                case "23":
                    strVal = "급여3종";
                    break;
                case "24":
                    strVal = "행려환자";
                    break;
                case "31":
                    strVal = "산재";
                    break;
                case "32":
                    strVal = "공상";
                    break;
                case "33":
                    strVal = "산재공상";
                    break;
                case "41":
                    strVal = "공단100";
                    break;
                case "42":
                    strVal = "직장100";
                    break;
                case "43":
                    strVal = "지역100";
                    break;
                case "44":
                    strVal = "가족계획";
                    break;
                case "51":
                    strVal = "일반";
                    break;
                case "52":
                    strVal = "교통";
                    break;
                case "53":
                    strVal = "계약처";
                    break;
                case "54":
                    strVal = "미확인";
                    break;
                case "55":
                    strVal = "TA일반";
                    break;
            }

            return strVal;

        }


        public static string GetJobsStopTime(PsmhDb pDbCon)
        {
            //' 전산정보팀에서 DB점검시 일시적으로 OCS, 원무행정 업무 중지시 사용
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT REMARK FROM ADMIN.ETC_JOBSTOP_TIME ";
                SQL = SQL + ComNum.VBLF + " WHERE JOBSTOP_FROM <= SYSDATE ";
                SQL = SQL + ComNum.VBLF + "   AND JOBSTOP_TO >= SYSDATE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["REMARK"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }


        public static string SexSearch(string strJuMin2)
        {
            string strVal = "";

            switch (VB.Left(strJuMin2, 1))
            {
                case "1":
                case "3":
                case "5":
                case "7":
                case "9":
                    strVal = "M";
                    break;
                default:
                    strVal = "F";
                    break;
            }
            return strVal;
        }



        /// <summary>
        /// READ_BCODE_CODE, READ_BCODE_CODE1
        /// </summary>
        /// <param name="strGubun"></param>
        /// <param name="strDate"> yyyy-MM-dd "" 값이면 데이터 조건문 적용안함</param>
        /// <returns></returns>
        public static string GetBCodeCODE(PsmhDb pDbCon, string strGubun, string strDate, [Optional] string strName)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strGubun.Trim() == "")
                return strVal;

            if (strDate != "")
            {
                if (VB.IsDate(strDate) == false)
                    return strVal;
            }

            try
            {
                SQL = "";
                SQL = "SELECT CODE FROM ADMIN.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN='" + strGubun + "' ";

                if (strDate != "")
                {
                    SQL = SQL + ComNum.VBLF + "  AND JDATE <= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY JDATE DESC ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["CODE"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }



        /// <summary>
        /// 모든 업무에 사용하는 코드명칭 기본코드 명칭 읽기 
        ///  [strFlag] 1 : strData = Code -> Name
        ///            2 : strData = Name -> Code
        /// </summary>
        /// <param name="strFlag">1, 2 만 입력</param>
        /// <param name="strGubun"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static string GetBCODENameCode(PsmhDb pDbCon, string strFlag, string strGubun, string strData)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strData.Trim() == "" || strFlag.Trim() == "" && strFlag.Trim() != "1" && strFlag.Trim() != "2")
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT " + (strFlag.Trim() == "1" ? "NAME" : "CODE") + " FROM ADMIN.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN='" + strGubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND " + (strFlag.Trim() == "1" ? "CODE" : "NAME") + " = '" + strData.Trim() + "'";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0][(strFlag.Trim() == "1" ? "NAME" : "CODE")].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        /// <summary>
        /// 수술실 전용 코드 SET
        /// 자료사전을 이용하여 ComboBox SET
        /// <param name="ArgCombobox"></param>
        /// <param name="argGubun"></param>
        /// <param name="strTYPE"> 1 = (코드) + "." + (명칭)형식 2: (코드) 3.(명칭) </param>
        /// <param name="argAll"></param>
        public static void SetOprCodeCombo(PsmhDb pDbCon, ComboBox ArgCombobox, string argGubun, string strTYPE, string argAll)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;

            if (ArgCombobox == null || argGubun.Trim() == "" || strTYPE.Trim() == "")
                return;

            try
            {
                ArgCombobox.Items.Clear();

                if (argAll == "ALL")
                {
                    ArgCombobox.Items.Add("*.전체");
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SORT,CODE,NAME FROM ADMIN.OPR_CODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN='" + argGubun + "' ";

                if (strTYPE == "4")
                {
                    SQL = SQL + ComNum.VBLF + "  AND CODE NOT IN '07'";
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY SORT,CODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (strTYPE == "1")
                        {
                            ArgCombobox.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                        }
                        else if (strTYPE == "2")
                        {
                            ArgCombobox.Items.Add(dt.Rows[i]["CODE"].ToString().Trim());
                        }
                        else if (strTYPE == "3")
                        {
                            ArgCombobox.Items.Add(dt.Rows[i]["NAME"].ToString().Trim());
                        }
                    }
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
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 해당 날짜가 휴일인지 Check 함
        /// True: 휴일  False: 휴일이 아님
        /// </summary>
        /// <param name="strDate"> yyyy-MM-dd 형식으로</param>
        /// <returns></returns>
        public static bool ChkDateHuIl(PsmhDb pDbCon, string strDate)
        {
            bool bolVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (VB.IsDate(strDate) == false)
                return bolVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT HOLYDAY,TEMPHOLYDAY FROM ADMIN.BAS_JOB ";
                SQL = SQL + ComNum.VBLF + " WHERE JOBDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["HolyDay"].ToString().Trim() == "*")
                    {
                        //'공휴일 가산에 쓰는 변수임 삭제하지 마세요~ KMC 2014-11-24
                        clsPublic.GstrHoliday = "*";
                        bolVal = true;
                    }

                    if (dt.Rows[0]["TempHolyDay"].ToString().Trim() == "*")
                    {
                        clsPublic.GstrTempHoliday = "*";

                    }
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
                MessageBox.Show(ex.Message);
            }

            return bolVal;
        }


        /// <summary>
        ///  병동 ComboBox SET
        /// </summary>
        /// <param name="argCombobox"></param>
        /// <param name="strIcuGbn">중환자실표시(1.MICU,SICU로 분리 2.IU로 표시)</param>
        /// <param name="bolClear">true=Combobox를 Clear후 다른 자료를 Additem, false=Clear 안함</param>
        /// <param name="intType">1=(코드) + "." + (명칭)형식 2: (코드) 3.(명칭)</param>
        public static void SetWardCodeCombo(PsmhDb pDbCon, ComboBox argCombobox, string strIcuGbn, bool bolClear, int intType)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strWard = "";
            int i = 0;

            if (bolClear == true)
                argCombobox.Items.Clear();

            //'중환자실을 SICU와 MICU분리
            if (strIcuGbn == "1")
            {
                if (intType == 1)
                {
                    argCombobox.Items.Add("SICU.SICU");
                    argCombobox.Items.Add("MICU.MICU");
                }
                else
                {
                    argCombobox.Items.Add("SICU");
                    argCombobox.Items.Add("MICU");
                }
            }

            try
            {
                //'병동마스타에서 코드를 Select
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT WARDCODE,WARDNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_WARD ";

                if (strIcuGbn == "1")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('IU','NP','2W','IQ') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('NP','2W') ";
                }

                SQL = SQL + ComNum.VBLF + " AND USED = 'Y' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WARDCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (intType)
                        {
                            case 1:
                                if (strIcuGbn == "1")
                                {
                                    strWard = VB.Left(dt.Rows[i]["WARDCODE"].ToString().Trim() + VB.Space(4), 4) + ".";
                                }
                                else
                                {
                                    strWard = dt.Rows[i]["WARDCODE"].ToString().Trim() + ".";
                                }

                                strWard = strWard + dt.Rows[i]["WARDNAME"].ToString().Trim();
                                argCombobox.Items.Add(strWard);
                                break;

                            case 2:
                                argCombobox.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                                break;

                            case 3:
                                argCombobox.Items.Add(dt.Rows[i]["WARDNAME"].ToString().Trim());
                                break;
                        }
                    }
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
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        ///   작업자 사번으로 해당 업무를 할 수 있는지 점검하는 함수
        ///   Ex. if (Program_execute_Sabun("IPD_퇴원심사완료") == false) ....
        /// </summary>
        /// <param name="strJobName"></param>
        /// <returns></returns>
        public static bool ProgramExecuteSabun(PsmhDb pDbCon, string strJobName)
        {
            bool bolVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strJobName == "")
                return bolVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT * FROM ADMIN.BAS_JOB_SABUN ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN='" + strJobName + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '" + clsPublic.GnJobSabun.ToString("00000") + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    bolVal = true;
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
                MessageBox.Show(ex.Message);
            }

            return bolVal;
        }

        /// <summary>
        /// 자료사전을 이용하여 ComboBox SET
        /// </summary>
        /// <param name="argCombobox"></param>
        /// <param name="strGuBun"></param>
        /// <param name="intType">1=(코드) + "." + (명칭)형식 2: (코드) 3.(명칭)</param>
        /// <param name="bolClear">true=Combobox를 Clear후 다른 자료를 Additem, false=Clear 안함</param>
        /// <param name="strNULL">N = " " 제외</param>
        public static void SetBCodeCombo(PsmhDb pDbCon, ComboBox argCombobox, string strGuBun, int intType, bool bolClear, string strNULL)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;

            if (bolClear == true)
                argCombobox.Items.Clear();

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SORT,CODE,NAME FROM ADMIN.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN='" + strGuBun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE > TRUNC(SYSDATE)) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SORT,CODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (strNULL != "N")
                        argCombobox.Items.Add(" ");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (intType)
                        {
                            case 1:
                                argCombobox.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                                break;

                            case 2:
                                argCombobox.Items.Add(dt.Rows[i]["CODE"].ToString().Trim());
                                break;

                            case 3:
                                argCombobox.Items.Add(dt.Rows[i]["NAME"].ToString().Trim());
                                break;
                        }
                    }
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
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="argCombo"></param>
        /// <param name="intType">1: 코드 + "." + 명칭 2: 코드 3: 명칭</param>
        public static void XrayReadDoctor(PsmhDb pDbCon, ComboBox argCombo, int intType)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;

            if (intType != 2 && intType != 3)
                intType = 1;

            argCombo.Items.Clear();

            try
            {
                SQL = "";
                SQL = "SELECT IDNUMBER,NAME FROM BAS_PASS ";
                SQL = SQL + ComNum.VBLF + "WHERE PROGRAMID='XUREAD' ";
                SQL = SQL + ComNum.VBLF + "  AND IDNUMBER > 0 ";
                SQL = SQL + ComNum.VBLF + "ORDER BY IDNUMBER ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                argCombo.Items.Add(" ");

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        argCombo.Items.Add(dt.Rows[i]["IDNUMBER"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());

                        switch (intType)
                        {

                            case 1:
                                argCombo.Items.Add(dt.Rows[i]["IDNUMBER"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                                break;

                            case 2:
                                argCombo.Items.Add(dt.Rows[i]["IDNUMBER"].ToString().Trim());
                                break;

                            case 3:
                                argCombo.Items.Add(dt.Rows[i]["NAME"].ToString().Trim());
                                break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                argCombo.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// READ_PT_ETC_SUGA_CHK
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strSuGaCode"></param>
        /// <returns></returns>
        public static bool GetPtEtcSuGaChk(PsmhDb pDbCon, string strSuGaCode)
        {
            bool bolVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='OCS_외래물리치료수가2' ";
                SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE ='') ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(Code) ='" + strSuGaCode.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return bolVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return bolVal;
                }

                bolVal = true;

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
                MessageBox.Show(ex.Message);
            }

            return bolVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argCombo"></param>
        /// <param name="strAll">1: **.전체</param>
        /// <param name="intType">1: 코드 + "." + 명칭 2: 코드 3: 명칭</param>
        public static void SetComboDept(PsmhDb pDbCon, ComboBox argCombo, string strAll = "", int intType = 0)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;

            if (intType != 2 && intType != 3)
                intType = 1;

            if (strAll == "")
                strAll = "1";

            argCombo.Items.Clear();

            try
            {
                SQL = "";
                SQL = "SELECT DEPTCODE, DEPTNAMEK FROM ADMIN.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PRINTRANKING  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    if (strAll == "1")
                    {
                        switch (intType)
                        {

                            case 1:
                                argCombo.Items.Add("**.전체");
                                break;

                            case 2:
                                argCombo.Items.Add("**");
                                break;

                            case 3:
                                argCombo.Items.Add("전체");
                                break;
                        }

                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {

                        switch (intType)
                        {

                            case 1:
                                argCombo.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                                break;

                            case 2:
                                argCombo.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                                break;

                            case 3:
                                argCombo.Items.Add(dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                                break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                argCombo.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argCombo"></param>
        /// <param name="strDept"></param>
        /// <param name="strAll">1: **.전체 , 2: 내과(MD)일경우 세부내과 모두 표시</param>
        /// <param name="intType">1: 코드 + "." + 명칭 2: 코드 3: 명칭</param>
        /// <param name="strMD">MD: 내과 (MD) 일경우 세부내과로 모든의사표시</param>
        public static void SetDrCodeCombo(PsmhDb pDbCon, ComboBox argCombo, string strDept, string strAll, int intType, string strMD)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;

            if (intType != 2 && intType != 3)
                intType = 1;

            if (strAll == "")
                strAll = "1";

            argCombo.Items.Clear();

            try
            {
                SQL = "";
                SQL = "SELECT DRCODE, DRNAME ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_DOCTOR";
                SQL = SQL + ComNum.VBLF + "WHERE TOUR <> 'Y'";

                if (strDept.IndexOf("전체") > -1 || strDept.IndexOf("**") > -1)
                {

                }
                else
                {
                    if (strMD == "MD")
                    {
                        if (strDept == "MD")
                        {
                            SQL = SQL + ComNum.VBLF + "     AND ( DRDEPT1 = '" + strDept + "' OR DRDEPT1 LIKE 'M%' ) ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "     AND DRDEPT1 = '" + strDept + "' ";
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "       AND DRDEPT1 = '" + strDept + "' ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY PRINTRANKING";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (strAll == "1")
                        argCombo.Items.Add("****.전체");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (intType)
                        {
                            case 1:
                                argCombo.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim());
                                break;

                            case 2:
                                argCombo.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim());
                                break;

                            case 3:
                                argCombo.Items.Add(dt.Rows[i]["DRNAME"].ToString().Trim());
                                break;
                        }
                    }
                }

                argCombo.SelectedIndex = 0;

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
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// BAS_AREA 지역코드 세팅
        /// </summary>
        /// <param name="argCombo"></param>
        /// <param name="strAll">1: **.전체</param>
        /// <param name="intType">1: 코드 + "." + 명칭 2: 코드 3: 명칭</param>
        /// <history> 2017.08.21 KMC</history>
        public static void SetComboJiCode(PsmhDb pDbCon, ComboBox argCombo, string strAll = "", int intType = 0)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;

            if (intType != 2 && intType != 3)
                intType = 1;

            if (strAll == "")
                strAll = "1";

            argCombo.Items.Clear();

            try
            {
                SQL = "";
                SQL = "SELECT JICODE, JINAME FROM " + ComNum.DB_PMPA + "BAS_AREA ";
                SQL = SQL + ComNum.VBLF + "ORDER BY JICODE  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    if (strAll == "1")
                    {
                        switch (intType)
                        {

                            case 1:
                                argCombo.Items.Add("**.전체");
                                break;

                            case 2:
                                argCombo.Items.Add("**");
                                break;

                            case 3:
                                argCombo.Items.Add("전체");
                                break;
                        }

                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {

                        switch (intType)
                        {

                            case 1:
                                argCombo.Items.Add(dt.Rows[i]["JICODE"].ToString().Trim() + "." + dt.Rows[i]["JINAME"].ToString().Trim());
                                break;

                            case 2:
                                argCombo.Items.Add(dt.Rows[i]["JICODE"].ToString().Trim());
                                break;

                            case 3:
                                argCombo.Items.Add(dt.Rows[i]["JINAME"].ToString().Trim());
                                break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                argCombo.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 'EMR 스캔여부를 점검(True:스캔됨 False:스캔안됨)
        /// </summary>
        /// <param name="strPano"></param>
        /// <param name="strIO"></param>
        /// <param name="strDate">yyyy-MM-dd 형식으로 입력</param>
        /// <param name="strDept"></param>
        /// <param name="strGb"></param>
        /// <param name="strTdate"></param>
        /// <returns></returns>
        public static bool ChkEmrScan(PsmhDb pDbCon, string strPano, string strIO, string strDate, string strDept, string strGb = "", string strTdate = "")
        {
            //'argTdate 는 특정기간안에 입원내역이 있는지 점검
            bool bolVal = false;
            string strNextDate = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (VB.IsDate(strDate) == false)
                return bolVal;

            strNextDate = Convert.ToDateTime(strDate).AddDays(1).ToString("yyyy-MM-dd");

            try
            {
                SQL = " SELECT PATID FROM ADMIN.EMR_TREATT ";
                SQL = SQL + ComNum.VBLF + "WHERE PATID = '" + strPano.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND CHECKED ='1'";

                if (strGb != "S")//'"S"는 스캔된것이 있으면 OK
                {
                    SQL = SQL + "  AND Class = '" + strIO.Trim() + "' ";

                    if (strDept == "ER")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND InDate >= '" + strDate.Replace("-", "") + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND InDate <= '" + strNextDate.Replace("-", "") + "' ";
                    }
                    else if (strDept == "I")
                    {
                        if (strGb == "Y")
                        {
                            SQL = SQL + ComNum.VBLF + "  AND OutDate >= '" + strDate.Replace("-", "") + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND OutDate <= '" + strTdate.Replace("-", "") + "' ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  AND InDate <= '" + strDate.Replace("-", "") + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND OutDate >= '" + strDate.Replace("-", "") + "' ";
                        }
                    }
                    else if (strDept == "O")
                    {
                        if (strDept == "MD")
                        {
                            SQL = SQL + ComNum.VBLF + "  AND ClinCode in ('MD','MR','RA')";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  AND ClinCode = '" + strDept.Trim() + "' ";
                        }
                    }
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return bolVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return bolVal;
                }

                bolVal = true;

                dt.Dispose();
                dt = null;

                if (strDept == "ER" && bolVal == false)
                {
                    SQL = "";
                    SQL = "SELECT A.PATID ";
                    SQL = SQL + ComNum.VBLF + "FROM ADMIN.EMR_TREATT A,ADMIN.EMR_CHARTPAGET B ";
                    SQL = SQL + ComNum.VBLF + "WHERE A.PATID = '" + strPano.Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND A.CLASS='I' ";
                    SQL = SQL + ComNum.VBLF + "     AND A.CHECKED ='1'";
                    SQL = SQL + ComNum.VBLF + "     AND A.INDATE<='" + strNextDate.Replace("-", "") + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND A.OUTDATE>='" + strDate.Replace("-", "") + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND A.TREATNO=B.TREATNO(+) ";
                    SQL = SQL + ComNum.VBLF + "     AND B.FORMCODE='103' "; //'ER기록지;

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return bolVal;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return bolVal;
                    }

                    bolVal = true;

                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
            return bolVal;
        }


        /// <summary>
        /// HH:mm => 분으로만 계산
        /// </summary>
        /// <param name="strDate"> HH:mm 형식으로 넣기 </param>
        /// <returns></returns>
        public static int TimeToMi(string strDate)
        {
            int intVal = 0;

            if (VB.Mid(strDate, 3, 1) != ":")
                return 0;

            if (VB.IsNumeric(strDate.Replace(":", "")) == false)
                return 0;

            intVal = Convert.ToInt32((VB.Val(VB.Split(strDate, ":")[0]) * 60) + VB.Val(VB.Split(strDate, ":")[1]));

            return intVal;
        }

        /// <summary>
        /// 분 => HH:mm
        /// </summary>
        /// <param name="strDate">  </param>
        /// <returns></returns>
        public static string MiToTime(int intDate)
        {
            string strVal = "";

            strVal = (intDate / 60).ToString("00") + ":" + (intDate % 60).ToString("00");

            return strVal;
        }

        /// <summary>
        /// Usage: nTime = TIME_GESAN("11:21-12:23")
        /// </summary>
        /// <param name="strDate"> "HH:mm-HH:mm" </param>
        /// <returns></returns>
        public static int TimeGeSan(string strDate)
        {
            int intVal = 0;
            int intTT1 = 0;
            int intMM1 = 0;
            int intTT2 = 0;
            int intMM2 = 0;

            if (strDate.Length != 11)
                return intVal;
            if (VB.Mid(strDate, 3, 1) != ":")
                return intVal;
            if (VB.Mid(strDate, 6, 1) != "-")
                return intVal;
            if (VB.Mid(strDate, 9, 1) != ":")
                return intVal;

            intTT1 = Convert.ToInt32(VB.Val(VB.Left(strDate, 2)));
            intMM1 = Convert.ToInt32(VB.Val(VB.Mid(strDate, 4, 2)));
            if (intTT1 < 0 || intTT1 > 23)
                return intVal;
            if (intMM1 < 0 || intMM1 > 59)
                return intVal;

            intTT2 = Convert.ToInt32(VB.Val(VB.Mid(strDate, 7, 2)));
            intMM2 = Convert.ToInt32(VB.Val(VB.Right(strDate, 2)));
            if (intTT2 < 0 || intTT2 > 23)
                return intVal;
            if (intMM2 < 0 || intMM2 > 59)
                return intVal;

            if (intTT2 >= intTT1)
            {
                intVal = (intTT2 * 60 + intMM2) - (intTT1 * 60 + intMM1);
            }
            else
            {
                intVal = ((intTT2 + 24) * 60 + intMM2) - (intTT1 * 60 + intMM1);
            }

            return intVal;
        }

        /// <summary>
        /// '폴더가 존재하는가를 검사
        /// </summary>
        /// <param name="strFolderName">경로 포함 해서 입력</param>
        /// <returns></returns>
        public static bool FolderExists(string strFolderName)
        {
            bool bolVal = false;
            DirectoryInfo di = new DirectoryInfo(strFolderName);

            bolVal = di.Exists;

            return bolVal;
        }

        /// <summary>
        /// '폴더를 강제로 만듬
        /// </summary>
        /// <param name="strPath"></param>
        public static void MakeFolder(string strPath)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(strPath);
                di.Create();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string GetFile(string strPath)
        {
            string strVal = "";

            try
            {
                FileInfo di = new FileInfo(strPath);

                if (di.Exists == false)
                    return strVal;

                strVal = Convert.ToString(di.OpenText());
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }

            return strVal;
        }

        /// <summary>
        /// '문자열에 보관된 내용을 지정한 파일에 보관함
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="strString"></param>
        public static void WriteFile(string strPath, string strString)
        {
            try
            {
                File.WriteAllText(strPath, strString, Encoding.Default);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 작성일 : 2010-11-09
        /// 작성자 : 김현욱
        /// 용  도 : 전자인증이 된 사번인지 확인
        ///          전자인증이 되지 안았을 경우 false를 반환함
        /// </summary>
        /// <param name="strSabun"></param>
        /// <returns></returns>
        public static bool GetCertiOk(PsmhDb pDbCon, string strSabun)
        {
            bool bolVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (clsPublic.GnJobSabun == 23515 || clsPublic.GstrJobGrade == "EDPS")
                return true;

            try
            {
                SQL = "   SELECT B.SABUN, B.CERTIOK";
                SQL = SQL + ComNum.VBLF + "       FROM ADMIN.INSA_MST A, ADMIN.INSA_MSTS B";
                SQL = SQL + ComNum.VBLF + "       WHERE A.SABUN = B.SABUN";
                SQL = SQL + ComNum.VBLF + "         AND A.TOIDAY IS NULL";
                SQL = SQL + ComNum.VBLF + "         AND B.CERTIOK = '1'";
                SQL = SQL + ComNum.VBLF + "         AND A.SABUN = '" + ComFunc.LPAD(strSabun, 5, "0") + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return bolVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return bolVal;
                }

                bolVal = true;

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
                MessageBox.Show(ex.Message);
            }
            return bolVal;
        }

        public static string GetErCarName(PsmhDb pDbCon, string strCode)
        {
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = "   SELECT NAME FROM ADMIN.ETC_ER_CAR ";
                SQL = SQL + ComNum.VBLF + "    WHERE CODE = '" + strCode + "' ";
                SQL = SQL + ComNum.VBLF + "      AND DELDATE IS NULL";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

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

                strVal = dt.Rows[0]["NAME"].ToString().Trim();

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
                MessageBox.Show(ex.Message);
            }
            return strVal;
        }

        public static string GetSaBunBuSeName(PsmhDb pDbCon, string strSaBun)
        {
            string strVal = "";
            string strBuse = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (strSaBun == "")
                return strVal;

            strSaBun = ComFunc.LPAD(strSaBun, 5, "0");

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT BUSE FROM ADMIN.INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN='" + strSaBun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ( TOIDAY IS NULL OR TOIDAY < TRUNC(SYSDATE) )";

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

                strBuse = dt.Rows[0]["BUSE"].ToString().Trim();

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT NAME,SNAME FROM ADMIN.BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + " WHERE BUCODE='" + strBuse + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

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

                if (dt.Rows[0]["SNAME"].ToString().Trim() != "")
                {
                    strVal = dt.Rows[0]["SNAME"].ToString().Trim();
                }
                else
                {
                    strVal = dt.Rows[0]["NAME"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
            return strVal;
        }

        /// <summary>
        /// ========================================================
        /// 작성일 : 2011-05-19
        /// 작성자 : 김현욱
        /// 용  도 : 지정일(argBDATE)에 대해서 선택진료 여부 확인
        ///          선택진료일 경우 "True" 값 반환
        /// ========================================================
        /// </summary>
        /// <param name="strPTNO"></param>
        /// <param name="strBDate"></param>
        /// <param name="strDeptCode"></param>
        /// <param name="strIO"></param>
        /// <param name="strGBSTS"></param>
        /// <returns></returns>
        public static bool GetSpecialService(PsmhDb pDbCon, string strPTNO, string strBDate, string strDeptCode, string strIO, string strGBSTS)
        {
            bool bolVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (strPTNO == "" || strBDate == "" || strDeptCode == "" || strIO == "")
                return bolVal;

            try
            {
                switch (strIO)
                {
                    case "I":
                        SQL = "";
                        SQL = " SELECT /*+ INDEX(IPD_NEW_MASTER INDEX_IPDNEWMST6) */ GBSPC SPECIAL";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.IPD_NEW_MASTER ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPTNO + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('" + strBDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                        SQL = SQL + ComNum.VBLF + "   AND INDATE <= TO_DATE('" + strBDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "'";

                        if (strGBSTS != "")
                        {
                            SQL = SQL + ComNum.VBLF + "   AND GBSTS = '9'  ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "   AND GBSTS <> '9' ";
                        }
                        break;

                    case "O":
                        SQL = "SELECT /*+ INDEX(OPD_MASTER INDEX_OM5) */ GBSPC SPECIAL";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.OPD_MASTER";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPTNO + "'";
                        SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "'";
                        break;

                    default:
                        return bolVal;
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return bolVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return bolVal;
                }

                switch (dt.Rows[0]["SPECIAL"].ToString().Trim())
                {
                    case "1":
                        bolVal = true;
                        break;

                    default:
                        bolVal = false;
                        break;
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
                MessageBox.Show(ex.Message);
            }
            return bolVal;
        }

        /// <summary>
        /// 진료비영수증 관련 인쇄 허용 - 기본5년
        /// </summary>
        /// <param name="strCODE"> 앞자리에 0 부분 없어야 함. </param>
        /// <returns></returns>
        public static bool JinAmtPrintChk(PsmhDb pDbCon, string strCode)
        {
            bool bolVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = "SELECT CODE ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN ='외부진료영수증인쇄제한' ";
                SQL = SQL + ComNum.VBLF + "AND TRIM(CODE) ='" + strCode + "'  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return bolVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return bolVal;
                }

                bolVal = true;

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
                MessageBox.Show(ex.Message);
            }
            return bolVal;
        }

        /// <summary>
        /// 'kyo 2017-01-26
        /// READ_V코드비필요내시경_CHK
        /// </summary>
        /// <seealso cref="READ_V코드비필요내시경_CHK"/>>
        /// <param name="strSabun"></param>
        /// <returns></returns>
        public static bool GetVCodeBiPilYoNaeSiGyeongChk(PsmhDb pDbCon, string strSuCode)
        {
            bool bolVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = "SELECT CODE      ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = 'BAS_V코드비필요내시경'        ";
                SQL = SQL + ComNum.VBLF + "AND TRIM(CODE) = '" + strSuCode + "'  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return bolVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return bolVal;
                }

                bolVal = true;

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
                MessageBox.Show(ex.Message);
            }
            return bolVal;
        }

        /// <summary>
        /// READ_V코드필요내시경_CHK
        /// 'kyo 2017-01-26
        /// </summary>
        /// <seealso cref="READ_V코드필요내시경_CHK"/>
        /// <param name="strSuCode"></param>
        /// <returns></returns>
        public static bool GetVCodePilYoNaeSiGyeongChk(PsmhDb pDbCon, string strSuCode)
        {
            bool bolVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = "SELECT CODE      ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_BCODE       ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = 'BAS_V코드필요내시경'         ";
                SQL = SQL + ComNum.VBLF + "AND TRIM(CODE) = '" + strSuCode + "'  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return bolVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return bolVal;
                }

                bolVal = true;

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
                MessageBox.Show(ex.Message);
            }
            return bolVal;

        }

        public static string GetNewDrug(PsmhDb pDbCon)
        {
            int i = 0;
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "  SELECT GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM (";
                SQL = SQL + ComNum.VBLF + "  SELECT TO_CHAR(WRITEDATE, 'YYYY-MM-DD') CDATE, '신약사용안내' GUBUN";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.DRUG_INFO01";
                SQL = SQL + ComNum.VBLF + "  WHERE (CDATE >= TRUNC(SYSDATE) OR CDATE IS NULL)";
                SQL = SQL + ComNum.VBLF + "    UNION ALL";
                SQL = SQL + ComNum.VBLF + "  SELECT TO_CHAR(WRITEDATE, 'YYYY-MM-DD') CDATE, '원내사용중단약품' GUBUN";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.DRUG_INFO02";
                SQL = SQL + ComNum.VBLF + "  WHERE (CDATE >= TRUNC(SYSDATE) OR CDATE IS NULL)";
                SQL = SQL + ComNum.VBLF + "    UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(WRITEDATE, 'YYYY-MM-DD') CDATE, '약품코드변경안내' GUBUN";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.DRUG_INFO03";
                SQL = SQL + ComNum.VBLF + "  WHERE (CDATE >= TRUNC(SYSDATE) OR CDATE IS NULL))";
                SQL = SQL + ComNum.VBLF + " GROUP BY GUBUN";
                SQL = SQL + ComNum.VBLF + " ORDER BY GUBUN";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strVal = strVal + dt.Rows[i]["GUBUN"].ToString().Trim() + ",";
                }

                strVal = VB.Mid(strVal, 1, strVal.Length - 2);

                dt.Dispose();
                dt = null;

                if (strVal != "")
                {
                    strVal = "[신약/사용중단] 약품에 대한 공지가 등록되어 있습니다.";
                }
            }
            catch (Exception ex)
            {
                strVal = "";

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        /// <summary>
        /// 당일 퇴원예고자
        /// argGBN = '1' 옵션은 argBDATE 날짜 이후 퇴원예고일자가 등록되어있는지 여부를 넘겨주는 것
        /// </summary>
        /// <param name="strPTNO"></param>
        /// <param name="strBDate">yyyy-MM-dd 형식으로 입력</param>
        /// <param name="strGBN">argGBN = '1' 옵션은 argBDATE 날짜 이후 퇴원예고일자가 등록되어있는지 여부를 넘겨주는 것</param>
        /// <returns></returns>
        public static string GetPreToi(PsmhDb pDbCon, string strPTNO, string strBDate, string strGBN = "")
        {
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (VB.IsDate(strBDate) == false)
                return strVal;

            strVal = "0";

            try
            {
                SQL = "";
                SQL = "SELECT OUTDATE ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPTNO + "' ";

                if (strGBN == "1")
                {
                    SQL = SQL + ComNum.VBLF + "    AND ROUTDATE >= TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND ROUTDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

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

                strVal = "1";

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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetQueryReadWard()
        {
            string strVal = "";

            strVal = " SELECT WARDCODE, WARDNAME FROM BAS_WARD ";
            strVal = strVal + ComNum.VBLF + " WHERE WARDCODE NOT IN ('IQ','2W', '3B','3C','ND','NR','32','DR','DS') ";
            strVal = strVal + ComNum.VBLF + " AND USED = 'Y' ";
            strVal = strVal + ComNum.VBLF + " ORDER BY WARDCODE, WARDNAME ";

            return strVal;
        }

        /// <summary>
        /// 격리 병실 종류 리턴
        /// strOpt에 값이 있으면 앞에 두글자만 나오고 값이 없으면 전체글자 나옴
        /// </summary>
        /// <param name="strRoom"></param>
        /// <param name="strOpt">1 : 병동환자관리 2: 원무팀</param>
        /// <returns></returns>
        public static string GetRoomSpecial(PsmhDb pDbCon, string strRoom, string strOpt)
        {
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            strVal = "";

            try
            {
                SQL = "";
                SQL = " SELECT  NAME ";
                SQL = SQL + ComNum.VBLF + "     FROM ADMIN.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = 'BAS_격리실종류'";
                SQL = SQL + ComNum.VBLF + "      AND CODE = '" + strRoom + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (strOpt == "")
                    {
                        strVal = dt.Rows[0]["NAME"].ToString().Trim();
                    }
                    else
                    {
                        strVal = VB.Left(dt.Rows[0]["NAME"].ToString().Trim(), 2);

                        switch (strOpt)
                        {
                            case "1": //'병동환자관리
                                strVal = "★" + strVal;
                                break;
                            case "2": //원무팀
                                strVal = "★" + strVal + ComNum.VBLF;
                                break;
                        }
                    }

                }

                dt.Dispose();
                dt = null;

                if (strOpt != "1")
                {
                    return strVal;
                }

                SQL = "";
                SQL = " SELECT  NAME ";
                SQL = SQL + ComNum.VBLF + "     FROM ADMIN.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = 'BAS_특실표시_간호'";
                SQL = SQL + ComNum.VBLF + "      AND CODE = '" + strRoom + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

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

                strVal = strVal + ComNum.VBLF + dt.Rows[0]["NAME"].ToString().Trim();

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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        /// <summary>
        /// '우편번호 세자리씩 사용할 경우 arg2 사용
        /// </summary>
        /// <param name="strZipCode1"></param>
        /// <param name="strZipCode2">우편번호 세자리씩 사용할 경우 arg2 사용</param>
        /// <returns></returns>
        public static string GetZipsName(PsmhDb pDbCon, string strZipCode1, string strZipCode2)
        {
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = " SELECT SI_DO, SI_GUN_GU, EUP_MYEON, RI, GIL, DONG";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_ZIPSNEW_2";
                SQL = SQL + ComNum.VBLF + " WHERE ZIPCODE = '" + strZipCode1 + strZipCode2 + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

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

                if (dt.Rows[0]["SI_DO"].ToString().Trim() != "")
                    strVal = dt.Rows[0]["SI_DO"].ToString().Trim();

                if (dt.Rows[0]["SI_GUN_GU"].ToString().Trim() != "")
                    strVal = strVal + " " + dt.Rows[0]["SI_GUN_GU"].ToString().Trim();

                if (dt.Rows[0]["EUP_MYEON"].ToString().Trim() != "")
                    strVal = strVal + " " + dt.Rows[0]["EUP_MYEON"].ToString().Trim();

                if (dt.Rows[0]["RI"].ToString().Trim() != "")
                    strVal = strVal + " " + dt.Rows[0]["RI"].ToString().Trim();

                if (dt.Rows[0]["GIL"].ToString().Trim() != "")
                    strVal = strVal + " " + dt.Rows[0]["GIL"].ToString().Trim();

                if (dt.Rows[0]["DONG"].ToString().Trim() != "")
                    strVal = strVal + " " + dt.Rows[0]["DONG"].ToString().Trim();

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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        /// <summary>
        /// 2014-02-03 주임 김현욱 추가
        /// 간호부 관리자 보다 한단계 높은 권한을 주는 부분입니다.
        /// 일반적으로 프로그램 변경사항, 전산실 연습 등을 미리 볼수 있는 권한입니다.(예전 고경자 과장 권한 기준)
        /// 모든 수간호사가 다 봐야한다면 NUR_MANAGER_CHECK를 이용하시고
        /// 좀 더 높은 권한(간호부 과장급)의 경우에 사용하시면 되겠습니다.
        /// </summary>
        /// <param name="strSabun"></param>
        /// <returns></returns>
        public static bool NurseSystemManagerChk(PsmhDb pDbCon, string strSabun)
        {
            bool bolVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = " SELECT CODE";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_시스템관리자'";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '" + strSabun + "' ";
                SQL = SQL + ComNum.VBLF + "      AND DELDATE IS NULL";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return bolVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return bolVal;
                }

                if (VB.Val(dt.Rows[0]["CODE"].ToString().Trim()) > 0)
                {
                    bolVal = true;
                }
                else
                {
                    bolVal = false;
                }

                dt.Dispose();
                dt = null;

                if (bolVal == false)
                {

                    SQL = " SELECT CODE";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_시스템관리자'";
                    SQL = SQL + ComNum.VBLF + "   AND CODE = '" + clsCompuInfo.gstrCOMIP + "' ";
                    SQL = SQL + ComNum.VBLF + "      AND DELDATE IS NULL";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return bolVal;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return bolVal;
                    }

                    if (VB.Val(dt.Rows[0]["CODE"].ToString().Trim()) > 0)
                    {
                        bolVal = true;
                    }
                    else
                    {
                        bolVal = false;
                    }

                    dt.Dispose();
                    dt = null;

                }

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return bolVal;
        }

        public static string MaskName(string strArg, string strArg2 = "")
        {
            int i = 0;
            string strVal = "";

            if (strArg2 != "")
            {
                for (i = 1; i <= strArg.Length; i++)
                {
                    if (i == strArg.Length)
                    {
                        strVal = strVal + "☆";
                    }
                    else
                    {
                        strVal = strVal + VB.Mid(strArg, i, 1);
                    }
                }
                return strVal;
            }

            strArg = strArg.Trim();
            for (i = 1; i <= strArg.Length; i++)
            {
                if (i == 1 || i == strArg.Length)
                {

                }
                else
                {
                    strVal = strVal + "☆";
                }
            }

            strVal = VB.Left(strArg, 1) + strVal + VB.Right(strArg, 1);

            return strVal;
        }

        /// <summary>
        /// 'strGBN = '1'일 경우 입원일시가 넘어감.
        /// </summary>
        /// <param name="strPTNO"></param>
        /// <param name="strInDate"></param>
        /// <param name="strWard"></param>
        /// <param name="strGBN">'strGBN = '1'일 경우 입원일시가 넘어감.</param>
        /// <returns></returns>
        public static string GetIpwonTime(PsmhDb pDbCon, string strPTNO, string strInDate, string strWard = "", string strGBN = "")
        {
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = " SELECT extractValue(chartxml, '//dt1') indate, extractValue(chartxml, '//it4') time";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.EMRXML ";
                SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + strPTNO + "'";
                SQL = SQL + ComNum.VBLF + "     AND FORMNO = '2311'";
                SQL = SQL + ComNum.VBLF + "     AND MEDFRDATE = '" + strInDate.Replace("-", "") + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (strGBN == "1")
                    {
                        strVal = dt.Rows[0]["INDATE"].ToString().Trim() + " " + dt.Rows[0]["TIME"].ToString().Trim();
                    }
                    else
                    {
                        strVal = dt.Rows[0]["TIME"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    return strVal;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT DECODE(FORMNO, '2285', extractValue(chartxml, '//dt1'), extractValue(chartxml, '//dt2')) indate, ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//it3') time";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXML ";
                SQL = SQL + ComNum.VBLF + "    WHERE PTNO = '" + strPTNO + "'";
                SQL = SQL + ComNum.VBLF + "    AND FORMNO IN ('2285','2356')";
                SQL = SQL + ComNum.VBLF + "    AND MEDFRDATE = '" + strInDate.Replace("-", "") + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (strGBN == "1")
                    {
                        strVal = dt.Rows[0]["INDATE"].ToString().Trim() + " " + dt.Rows[0]["TIME"].ToString().Trim();
                    }
                    else
                    {
                        strVal = dt.Rows[0]["TIME"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    return strVal;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT DECODE(FORMNO, '2295', extractValue(chartxml, '//dt3'), extractValue(chartxml, '//dt2')) indate, ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//it4') time";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXML ";
                SQL = SQL + ComNum.VBLF + "    WHERE PTNO = '" + strPTNO + "'";
                SQL = SQL + ComNum.VBLF + "    AND FORMNO IN ('2294','2295','2305')";
                SQL = SQL + ComNum.VBLF + "    AND MEDFRDATE = '" + strInDate.Replace("-", "") + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (strGBN == "1")
                    {
                        strVal = dt.Rows[0]["INDATE"].ToString().Trim() + " " + dt.Rows[0]["TIME"].ToString().Trim();
                    }
                    else
                    {
                        strVal = dt.Rows[0]["TIME"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    return strVal;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT DECODE(FORMNO, '1556', extractValue(chartxml, '//it8'), ";
                SQL = SQL + ComNum.VBLF + " DECODE(FORMNO, '1558', extractValue(chartxml, '//it99'), extractValue(chartxml, '//it3'))) time";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXML";
                SQL = SQL + ComNum.VBLF + "    WHERE PTNO = '" + strPTNO + "'";
                SQL = SQL + ComNum.VBLF + "    AND FORMNO IN ('1545','1553','1554','1556','1558','1561')";
                SQL = SQL + ComNum.VBLF + "    AND MEDFRDATE = '" + strInDate.Replace("-", "") + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["TIME"].ToString().Trim();
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        /// <summary>
        /// Read_환자장애구분
        /// </summary>
        /// <seealso cref="Read_환자장애구분"/>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public static string HwanJaJangAeGuBun(PsmhDb pDbCon, string strCode)
        {
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = " SELECT NAME FROM ADMIN.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN ='환자장애구분' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE ='" + strCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

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

                strVal = dt.Rows[0]["NAME"].ToString().Trim();

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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetBasPatientObst(PsmhDb pDbCon, string strPtNo)
        {
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = " SELECT OBST FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPtNo + "' ";

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

                strVal = dt.Rows[0]["OBST"].ToString().Trim();

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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strDashX">Y : 0000000000000 Y가 아니면 000000-0000000 </param>
        /// <returns></returns>
        public static string GetBasPatientJumin2(PsmhDb pDbCon, string strPtNo, string strDashX)
        {
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = " SELECT JUMIN1,JUMIN2,JUMIN3 FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPtNo + "' ";

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

                if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                {
                    if (strDashX == "Y")
                    {
                        strVal = dt.Rows[0]["JUMIN1"].ToString().Trim() + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    }
                    else
                    {
                        strVal = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    }
                }
                else
                {
                    if (strDashX == "Y")
                    {
                        strVal = dt.Rows[0]["JUMIN1"].ToString().Trim() + dt.Rows[0]["JUMIN2"].ToString().Trim();
                    }
                    else
                    {
                        strVal = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + dt.Rows[0]["JUMIN2"].ToString().Trim();
                    }
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        public static string GetBasPatientJuminDeAES(PsmhDb pDbCon, string strPtNo)
        {
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = " SELECT JUMIN1,JUMIN2,JUMIN3 FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPtNo + "' ";

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

                strVal = dt.Rows[0]["JUMIN1"].ToString().Trim() + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());

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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        /// <summary>
        /// 2016-08-08 계장 김현욱
        /// 검사결과 조회 제한
        /// 병원장님 지시사항입니다.
        /// 1) 재원자 및 당일 외래 진료자만 조회 가능
        /// 2) 병록번호 수정 불가하게 막음, 파라메터로 전달 받은 내용은 조회 가능
        /// 3) 파라메터로 전달 받아도 조회 조건에 맞지 않으면 조회 안됨
        /// 4) 의사 사번 제외    
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strJobSaBun"></param>
        /// <param name="conText"></param>
        /// <returns></returns>
        public static bool EtcViewCert(PsmhDb pDbCon, string strPtNo, string strJobSaBun, TextBox conText)
        {
            bool bolVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D");
            DataTable dt = null;

            try
            {

                conText.Enabled = false;

                SQL = " SELECT NAME";
                SQL = SQL + ComNum.VBLF + " From ADMIN.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'OCS_기타검사결과_조회제한'";
                SQL = SQL + ComNum.VBLF + " AND CODE = '시행'";
                SQL = SQL + ComNum.VBLF + " AND NAME = 'N'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;

                    bolVal = true;

                    return bolVal;
                }

                dt.Dispose();
                dt = null;

                conText.Enabled = true;

                //'1) 의사일 경우 OK
                SQL = " SELECT DRCODE";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.OCS_DOCTOR";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + ComFunc.LPAD(strJobSaBun, 5, "0") + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;

                    bolVal = true;

                    return bolVal;
                }

                dt.Dispose();
                dt = null;

                //'2) 의료정보팀에서 세팅한 경우 OK
                SQL = " SELECT USERID ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMR_USERT ";
                SQL = SQL + ComNum.VBLF + " WHERE USERID = '" + strJobSaBun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND H_VIEW = '*'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;

                    bolVal = true;

                    return bolVal;
                }

                dt.Dispose();
                dt = null;

                //'3) 당일 외래 접수 내역이 있을 경우 OK
                SQL = " SELECT PANO ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.OPD_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;

                    bolVal = true;

                    return bolVal;
                }

                dt.Dispose();
                dt = null;

                //'4) 현재 재원자이거나 당일 퇴원자일 경우 OK
                SQL = " SELECT PANO ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND (JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     OR OUTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "       )";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;

                    bolVal = true;

                    return bolVal;
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
                MessageBox.Show(ex.Message);
            }

            return bolVal;
        }

        public static string GetSuBulMatchCode(PsmhDb pDbCon, string strWard, string strGubun)
        {
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (strWard.Trim() == "")
                return strVal;

            switch (strGubun)
            {
                case "DRUG":
                case "GUME":
                    break;
                default:
                    break;
            }

            try
            {
                SQL = "SELECT NAME ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "WHERE CODE = '" + strWard + "' ";

                if (strGubun == "DRUG")
                {
                    SQL = SQL + ComNum.VBLF + "     AND GUBUN = 'OCS_불출부서_매칭_약제' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND GUBUN = 'OCS_불출부서_매칭_구매' ";
                }
                SQL = SQL + ComNum.VBLF + "     AND DELDATE IS NULL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count == 0)
                {
                    strVal = dt.Rows[0]["NAME"].ToString().Trim();
                }
                else if (dt.Rows.Count > 0)
                {
                    strVal = strWard;
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
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        /// <summary>
        /// 상병명 반환
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public static string READ_ILLName(PsmhDb pDbCon, string strCode, string strGubun = "K")
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT IllNameK, ILLNAMEE FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                SQL = SQL + ComNum.VBLF + " WHERE IllCode = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    if (strGubun == "K")
                    {
                        rtnVal = dt.Rows[0]["IllNameK"].ToString().Trim();
                    }
                    else if (strGubun == "E")
                    {
                        rtnVal = dt.Rows[0]["IllNameE"].ToString().Trim();
                    }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public static string READ_ConvGbn_Name(string strCode)
        {
            string rtnVal = "";

            switch (strCode)
            {
                case "1":
                    rtnVal = "진찰료 변환정보";
                    break;
                case "2":
                    rtnVal = "감액계정 변환정보";
                    break;
                default:
                    rtnVal = "** 오류 **";
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 수가명 반환
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public static string READ_SugaName(PsmhDb pDbCon, string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT SuNameK FROM " + ComNum.DB_PMPA + "BAS_SUN";
                SQL = SQL + ComNum.VBLF + " WHERE SuNext = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SuNameK"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public static string READ_INSA_BUSE(PsmhDb pDbCon, string strSABUN)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = " SELECT B.NAME ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.INSA_MST A, ADMIN.BAS_BUSE B";
                SQL = SQL + ComNum.VBLF + " WHERE A.SABUN = '" + strSABUN + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.BUSE = B.BUCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public static string readIPDNO(PsmhDb pDbCon, string strInDate, string strPtNo)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT IPDNO FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND INDATE >= TO_DATE('" + strInDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "         AND INDATE <= TO_DATE('" + strInDate + " 23:59','YYYY-MM-DD HH24:MI') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["IPDNO"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return rtnVal;
            }
        }

        /// <summary>
        /// 미시행여부 체크
        /// </summary>
        /// <param name="strPano"></param>
        /// <param name="strDeptCode"></param>
        /// <returns></returns>
        public static string CHECK_EXECUTE_new(PsmhDb pDbCon, string strPano, string strDeptCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "N";
            string GstrEmrViewDoct_NEW = "";

            string strFDate = "";
            string strTDate = "";

            strFDate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "D"), "D", "-")).AddDays(-100).ToString("yyyy-MM-dd");
            strTDate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "D"), "D", "-")).ToString("yyyy-MM-dd");

            if (Convert.ToDateTime(strFDate) <= Convert.ToDateTime("2014-01-01"))
            {
                strFDate = "2014-01-01";
            }

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.ENTERDATE, A.PANO, A.SNAME,  A.DEPTCODE, A.DRCODE, A.XCODE, B.XNAME   ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "XRAY_DETAIL A, " + ComNum.DB_PMPA + "XRAY_CODE B ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND ((A.BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') AND A.BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + "                 OR (A.RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') AND A.RDATE < TO_DATE('" + Convert.ToDateTime(strTDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')))";
                SQL = SQL + ComNum.VBLF + "         AND (GbSTS IS NULL OR GbSTS NOT IN ('7','D') ) ";
                SQL = SQL + ComNum.VBLF + "         AND A.XCODE = B.XCODE(+)  ";

                //내과코드는 별도처리
                if (strDeptCode == "MG" || strDeptCode == "MC" || strDeptCode == "MP" || strDeptCode == "ME" || strDeptCode == "MN" || strDeptCode == "MR" || strDeptCode == "MI")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE IN ('" + strDeptCode + "','MD')  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE ='" + strDeptCode + "' ";
                }

                SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD ='O'";

                if (GstrEmrViewDoct_NEW != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.DrCode IN (" + GstrEmrViewDoct_NEW + ") ";
                }

                SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE NOT IN ('DT') ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.BDATE ENTDATE,  PANO, A.SNAME,  A.DEPTCODE, A.DRCODE , A.MASTERCODE XCODE ,  C.EXAMNAME XNAME   ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_ORDER A , " + ComNum.DB_MED + "EXAM_MASTER C  ";
                SQL = SQL + ComNum.VBLF + "     WHERE  (( A.BDATE >= SYSDATE -60 AND A.BDATE <= SYSDATE AND RDATE IS NULL ) ";
                SQL = SQL + ComNum.VBLF + "             OR (  A.RDATE >= SYSDATE -60 AND A.RDATE <SYSDATE+1 AND RDATE IS NULL ) )";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.SPECNO IS NULL ";
                SQL = SQL + ComNum.VBLF + "         AND (A.CANCEL IS NULL OR A.CANCEL = '' OR A.CANCEL = ' ') ";

                //내과코드는 별도처리
                if (strDeptCode == "MG" || strDeptCode == "MC" || strDeptCode == "MP" || strDeptCode == "ME" || strDeptCode == "MN" || strDeptCode == "MR" || strDeptCode == "MI")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE IN ('" + strDeptCode + "','MD' )  ";

                    if (GstrEmrViewDoct_NEW != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND ( A.DrCode IN (" + GstrEmrViewDoct_NEW + ") OR a.DrCode IN ( '1102','1104','1107','1108','1114','1122','1123','1125','1126','1127') )  ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         AND  a.DrCode IN ( '1102','1104','1107','1108','1114','1122','1123','1125','1126','1127')   ";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE ='" + strDeptCode + "' ";

                    if (GstrEmrViewDoct_NEW != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.DrCode IN (" + GstrEmrViewDoct_NEW + ") ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD ='O'";
                SQL = SQL + ComNum.VBLF + "         AND A.MASTERCODE = C.MASTERCODE  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "Y";
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return rtnVal;
            }
        }

        /// <summary>
        /// PatWarning.READ_낙상고위험체크_OPD
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strBDate"></param>
        /// <param name="strAge"></param>
        /// <returns></returns>
        public static string READ_낙상고위험체크_OPD(PsmhDb pDbCon, string strPtNo, string strBDate, string strAge)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT ROWID FROM " + ComNum.DB_PMPA + "NUR_FALL_SCALE_OPD";
                SQL = SQL + ComNum.VBLF + "     WHERE ACTDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPtNo + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;

                    rtnVal = "OK";
                    return rtnVal;
                }

                SQL = "";
                SQL = "SELECT PANO FROM " + ComNum.DB_PMPA + "NUR_FALL_REPORT ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND ROWNUM = 1";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;

                    rtnVal = "OK";
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;

                //2015-10-15 병동 대상이면 고위험 체크
                if (READ_WARNING_FALL(pDbCon, strPtNo, strBDate, 0, strAge, "외래") == "낙상위험")
                {
                    rtnVal = "OK";
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

        public static string Read_Drug_Jep_Name(PsmhDb pDbCon, string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";
            string strTemp = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     JEPNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_JEP ";
                SQL = SQL + ComNum.VBLF + "     WHERE JEPCODE IN (";
                SQL = SQL + ComNum.VBLF + "                     SELECT JEPCODE FROM " + ComNum.DB_ERP + "DRUG_CONVRATE";
                SQL = SQL + ComNum.VBLF + "                         WHERE SUNEXT = '" + strCode + "')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strTemp = dt.Rows[0]["JEPNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //한글만 표현 특수문자 및 영어는 삭제처리
                for (int i = 1; i <= strTemp.Length; i++)
                {
                    if (VB.Asc(VB.Mid(strTemp, i, 1)) >= 33 && VB.Asc(VB.Mid(strTemp, i, 1)) <= 126)
                    {
                    }
                    else
                    {
                        rtnVal = rtnVal + VB.Mid(strTemp, i, 1);
                    }
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
        /// 병실로 병동찾기
        /// </summary>
        /// <param name="strRoomCode">병실코드</param>
        /// <returns></returns>
        public static string READ_WARD_FROM_ROOM(PsmhDb pDbCon, string strRoomCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     WARDCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ROOM ";
                SQL = SQL + ComNum.VBLF + "     WHERE ROOMCODE = '" + strRoomCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["WARDCODE"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

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
        /// Description : 날짜포맷
        /// author : 박병규
        /// Create Date : 2017-10-18
        /// <param name="str"></param>
        /// <seealso cref="vbfunc.bas : Date_Format"/>
        /// </summary>
        public string Date_Format(string ArgDate)
        {
            string rtnVal = string.Empty;


            if (VB.IsNumeric(ArgDate))
            {
                switch (ArgDate.Length)
                {
                    case 4:
                        rtnVal = ArgDate.Substring(0, 2);
                        rtnVal += "-" + ArgDate.Substring(2);
                        break;
                    case 6:
                        rtnVal = ArgDate.Substring(0, 2);
                        rtnVal += "-" + ArgDate.Substring(2, 2);
                        rtnVal += "-" + ArgDate.Substring(4);
                        break;
                    case 8:
                        rtnVal = ArgDate.Substring(0, 4);
                        rtnVal += "-" + ArgDate.Substring(4, 2);
                        rtnVal += "-" + ArgDate.Substring(6);
                        break;
                }
            }
            else
            {
                rtnVal = ArgDate;
            }

            if (VB.IsDate(rtnVal))
                rtnVal = string.Format("{0:yyyy-MM-dd}", rtnVal);
            else
                rtnVal = "";

            return rtnVal;
        }

        /// <summary>
        /// 지정일(argBDATE)에 대해서 선택진료 여부 확인 선택진료일 경우 "True" 값 반환
        /// <seealso cref="vbfunc.bas : READ_SPECIAL_SERVICE"/>
        /// </summary>
        /// <param name="argPTNO"></param>
        /// <param name="ArgBDate"></param>
        /// <param name="ArgDeptCode"></param>
        /// <param name="ArgIO"></param>
        /// <param name="ArgGBSTS"></param>
        /// <returns></returns>
        public static bool READ_SPECIAL_SERVICE(PsmhDb pDbCon, string argPTNO, string ArgBDate, string ArgDeptCode, string ArgIO, string ArgGBSTS = "")
        {
            bool rtnVal = false;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (argPTNO == "" || ArgBDate == "" || ArgDeptCode == "" || ArgIO == "")
                return false;

            if (ArgIO == "I")
            {
                SQL = " SELECT /*+ INDEX(IPD_NEW_MASTER INDEX_IPDNEWMST6) */ GBSPC SPECIAL";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('" + ArgBDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE <= TO_DATE('" + ArgBDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + ArgDeptCode + "'";
                if (ArgGBSTS != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND GBSTS = '9'  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND GBSTS <> '9' ";
                }
            }
            else if (ArgIO == "O")
            {
                SQL = "SELECT /*+ INDEX(OPD_MASTER INDEX_OM5) */ GBSPC SPECIAL";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.OPD_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "'";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + ArgDeptCode + "'";
            }
            else
            {
                return false;
            }

            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

            if (dt.Rows[0]["SPECIAL"].ToString().Trim() == "1")
            {
                rtnVal = true;
            }
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtno"></param>
        /// <param name="ArgGbn"></param>
        /// <returns></returns>
        /// <seealso cref="vbfunc.bas : READ_Patient_EName"/>
        public string Read_Patient_Ename(PsmhDb pDbCon, string ArgPtno, string ArgGbn = "")
        {
            //gbn : 1. 성명 이니셜만 표시 예) KIM K.D.
            //      2. 첫글짜만대문자     예) KIM Kil Dong
            //      3. 성명 이니셜만 표시 예) KKD

            DataTable DtFunc = null;
            DataTable DtSub = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";
            string strEname = "";

            int nNameLen = 0;

            if (VB.Val(ArgPtno) == 0)
            { return rtnVal; }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SName ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
            SQL += ComNum.VBLF + "  WHERE Pano  = '" + ArgPtno + "' ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                rtnVal = DtFunc.Rows[0]["SNAME"].ToString().Trim();
                nNameLen = rtnVal.Length;       //2019-02-25 이름 카운트 읽고
            }

            else
                rtnVal = "";

            DtFunc.Dispose();
            DtFunc = null;

            if (rtnVal != "")
            {
                //for (int i = 0; i < 5; i++)  
                for (int i = 0; i < nNameLen; i++) // 카운트 만큼만 반복
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ENGNAME ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_Z300FONT ";
                    //SQL += ComNum.VBLF + "  WHERE Z300CODE  = '" + rtnVal.Substring(0, 1) + "' ";
                    SQL += ComNum.VBLF + "  WHERE Z300CODE  = '" + rtnVal.Substring(i, 1) + "' ";  //2019-02-25 환자 영문 이니설 같은 글자만 출력 오류 보완
                    SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtSub.Dispose();
                        DtSub = null;
                        return rtnVal;
                    }

                    if (DtSub.Rows.Count > 0)
                    {
                        if (DtSub.Rows[0]["ENGNAME"].ToString().Trim() != "")
                        {
                            if (ArgGbn == "1")
                            {
                                if (i == 0)
                                    strEname += " " + DtSub.Rows[0]["ENGNAME"].ToString().Trim();
                                else
                                    strEname += " " + DtSub.Rows[0]["ENGNAME"].ToString().Trim().Substring(0, 1) + ".";
                            }
                            else if (ArgGbn == "2")
                                strEname += " " + DtSub.Rows[0]["ENGNAME"].ToString().Trim().Substring(0, 1).ToUpper() + DtSub.Rows[0]["ENGNAME"].ToString().Trim().Substring(1).ToLower();
                            else if (ArgGbn == "3")
                            {
                                if (i == 0)
                                    strEname += "" + DtSub.Rows[0]["ENGNAME"].ToString().Trim().Substring(0, 1);
                                else
                                    strEname += "" + DtSub.Rows[0]["ENGNAME"].ToString().Trim().Substring(0, 1);
                            }
                            else
                                strEname += " " + DtSub.Rows[0]["ENGNAME"].ToString().Trim();
                        }
                    }

                    DtSub.Dispose();
                    DtSub = null;
                }

                rtnVal = strEname;
            }

            return rtnVal;
        }

        /// <summary>
        /// 병동 ComboBox SET
        /// Author : 박병규
        /// Create Date : 2018.03.05
        /// </summary>
        /// <param name="o"></param>
        /// <param name="pDbCon"></param>
        /// <param name="ArgICUGbn">중환자실표시(1.MICU,SICU로 분리 2.IU로 표시)</param>
        /// <param name="ArgClear">True=Combobox를 Clear후 다른 자료를 Additem, False=Clear 안함</param>
        /// <param name="ArgType">1=(코드) + "." + (명칭)형식 2: (코드) 3.(명칭)</param>
        /// <seealso cref="vbfunc.bas : Combo_WardCode_SET"/> 
        public void Combo_Wardcode_Set(PsmhDb pDbCon, ComboBox o, string ArgICUGbn, bool ArgClear, int ArgType)
        {
            DataTable DtFunc = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string strWard = "";

            if (ArgClear)
            { o.Items.Clear(); }

            if (ArgICUGbn == "1")
            {
                if (ArgType == 1)
                {
                    o.Items.Add("SICU.SICU");
                    o.Items.Add("MICU.MICU");
                }
                else
                {
                    o.Items.Add("SICU");
                    o.Items.Add("MICU");
                }
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT WardCode, WardName ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_WARD ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";

            if (ArgICUGbn == "1")
                SQL += ComNum.VBLF + "AND WARDCODE NOT IN ('IU','NP','2W','IQ') ";
            else
                SQL += ComNum.VBLF + "AND WardCode NOT IN ('NP','2W') ";

            SQL += ComNum.VBLF + "    AND USED  = 'Y' ";
            SQL += ComNum.VBLF + "  ORDER BY WardCode ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return;
            }

            if (DtFunc.Rows.Count > 0)
            {

                for (int i = 0; i < DtFunc.Rows.Count; i++)
                {
                    if (ArgType == 1)
                    {
                        if (ArgICUGbn == "1")
                            strWard = (DtFunc.Rows[i]["WARDCODE"].ToString() + "    ").Substring(0, 4) + ".";
                        else
                            strWard = DtFunc.Rows[i]["WARDCODE"].ToString().Trim() + ".";

                        strWard += DtFunc.Rows[i]["WARDNAME"].ToString().Trim();
                        o.Items.Add(strWard);
                    }
                    else if (ArgType == 2)
                        o.Items.Add(DtFunc.Rows[i]["WARDCODE"].ToString().Trim());

                    else if (ArgType == 3)
                        o.Items.Add(DtFunc.Rows[i]["WARDNAME"].ToString().Trim());
                }
            }

            DtFunc.Dispose();
            DtFunc = null;

        }

        /// <summary>
        /// 병동 ComboBox SET
        /// Author : 박병규
        /// Create Date : 2018.03.05
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ArgJumin"></param>
        /// <param name="ArgInDate"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="vbfunc.bas : AGE_PD_GESAN"/> 
        public string PD_AGE_GESAN(string ArgJumin, string ArgInDate, string ArgDate)
        {
            string rtnVal = "";
            int nAge_1 = ComFunc.AgeCalcEx(ArgJumin, ArgInDate);
            int nAge_2 = ComFunc.AgeCalcEx(ArgJumin, ArgDate);

            if (nAge_1 < 6)
            {
                if (nAge_2 >= 6)
                {
                    if (nAge_1 < nAge_2)
                        rtnVal = "입원시: 5세이하  ◆ 현재는: 6세이상 ◆. 꼭 퇴원 분리 작업을 해야함.";
                    else
                        rtnVal = "";
                }
            }

            return rtnVal;
        }
        /// <summary>
        /// YYYYMM 으로 개월 단위 계산
        /// Author : 이상훈
        /// Create Date : 2018.03.28
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="nMonth"></param>
        /// <returns></returns>
        public string MONTH_ADD_NEW(string strDate, int nMonth)
        {
            string rtnVal = "";

            int nYY;
            int nMM;

            if (strDate.Length != 6)
            {
                rtnVal = "";
            }

            nYY = int.Parse(VB.Left(strDate, 4));
            nMM = int.Parse(VB.Mid(strDate, 5, 2));

            if (nMonth > 0)
            {
                for (int i = 0; i < nMonth; i++)
                {
                    nMM += 1;
                    if (nMM > 12)
                    {
                        nYY += 1;
                        nMM = 1;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Math.Abs(nMonth); i++)
                {
                    nMonth -= 1;
                    if (nMM < 1)
                    {
                        nYY = -1;
                        nMM = 12;
                    }
                }
            }
            rtnVal = string.Format("{0:0000}", nYY) + string.Format("{0:00}", nMM);
            return rtnVal;
        }

        public static string READ_Gamek_infoSabun(string strJumin)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                //SQL = "";
                //SQL = "SELECT";
                //SQL = SQL + ComNum.VBLF + "     SABUN ,TOIDAY  ";
                //SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "insa_mst ";
                //SQL = SQL + ComNum.VBLF + "     WHERE Jumin3 = '" + clsAES.AES(strJumin) + "'  ";   //2013-02-20
                //SQL = SQL + ComNum.VBLF + "ORDER BY TOIDAY desc ";

                SQL = " SELECT SABUN ,SYSDATE TOIDAY ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.INSA_MSTB a, ADMIN.BAS_GAMF b ";
                SQL = SQL + ComNum.VBLF + "  WHERE a.JUMIN3 = b.GAMJUMIN3 ";
                SQL = SQL + ComNum.VBLF + "   AND a.JUMIN3 ='" + clsAES.AES(strJumin) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND rownum=1 union all ";
                SQL = SQL + ComNum.VBLF + " SELECT SABUN ,TOIDAY  ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.insa_mst ";
                SQL = SQL + ComNum.VBLF + " WHERE Jumin3  = '" + clsAES.AES(strJumin) + "'  ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY TOIDAY desc  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SABUN"].ToString().Trim();
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

        public static string READ_HIC_AGE_GESAN2(string argJumin)
        {
            string rtnVal = "999";
            string strGbn = "";     //구분
            string strJumin = "";   //주민번호
            string strBirth = "";   //생년월일
            int nYEAR = 0;
            int nJYEAR = 0;

            strJumin = argJumin;
            strGbn = VB.Mid(argJumin, 7, 1);

            ComFunc.ReadSysDate(clsDB.DbCon);
            nJYEAR = (int)VB.Val(VB.Left(clsPublic.GstrSysDate, 4));

            //'생년월일을 YYYY-MM-DD Type으로 변경
            if (strGbn == "1" || strGbn == "2" || strGbn == "5" || strGbn == "6")        //'한국인 남1 녀2 , 외국인 남5 녀6
            {
                strBirth = "19" + VB.Left(argJumin, 2) + "-" + VB.Mid(argJumin, 3, 2) + "-" + VB.Mid(argJumin, 5, 2);
            }
            else if (strGbn == "3" || strGbn == "4" || strGbn == "7" || strGbn == "8")    //'한국인 남3 녀4 , 외국인 남7 녀8
            {
                strBirth = "20" + VB.Left(argJumin, 2) + "-" + VB.Mid(argJumin, 3, 2) + "-" + VB.Mid(argJumin, 5, 2);
            }
            else if (strGbn == "0" || strGbn == "9")
            {
                strBirth = "18" + VB.Left(argJumin, 2) + "-" + VB.Mid(argJumin, 3, 2) + "-" + VB.Mid(argJumin, 5, 2);
            }
            else
            {
                rtnVal = "999";
                return rtnVal;
            }

            nYEAR = (int)VB.Val(VB.Left(strBirth, 4));
            rtnVal = (nJYEAR - nYEAR).ToString();
            return rtnVal;
        }

        //병동명칭 Read
        public static string Read_WardName(PsmhDb pDbCon, string argCode)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            try
            {
                SQL = "";
                SQL = " SELECT WARDName FROM BAS_WARD ";
                SQL = SQL + ComNum.VBLF + "WHERE WARDCODE='" + argCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                rtnVar = dt.Rows[0]["WARDName"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return rtnVar;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVar;
            }
        }


        public static string BAS_BUSE_TREE_UP(PsmhDb pDbCon, string ArgBucode)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;

            string strBuCode = VB.Val(ArgBucode).ToString("000000");
            string strTemp = "";
            double nCnt = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //'LEVEL 읽기
                SQL = " SELECT MAX(TREE_DEPTH) DEPT_LEVEL FROM ADMIN.BAS_BUSE ";
                SQL += ComNum.VBLF + " WHERE BUCODE = '" + ArgBucode + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return strBuCode;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return strBuCode;
                }

                nCnt = VB.Val(dt.Rows[0]["DEPT_LEVEL"].ToString().Trim());

                dt.Dispose();
                dt = null;

                if (nCnt > 1)
                {
                    strBuCode = ArgBucode;
                    for (int i = 1; i < nCnt; i++)
                    {
                        SQL = " SELECT INSA, DEPT_ID_UP FROM ADMIN.BAS_BUSE ";
                        SQL += ComNum.VBLF + " WHERE DEPT_ID = " + strBuCode;

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return strBuCode;
                        }

                        if (dt.Rows.Count == 0)
                        {
                            break;
                        }

                        SQL = " SELECT INSA FROM ADMIN.BAS_BUSE WHERE BUCODE = '" + VB.Val(dt.Rows[0]["DEPT_ID_UP"].ToString().Trim()).ToString("000000") + "'";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return strBuCode;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            strTemp = dt2.Rows[0]["INSA"].ToString().Trim();
                        }

                        if (strTemp != "*")
                        {
                            dt2.Dispose();
                            dt2 = null;
                            break;
                        }

                        strBuCode = VB.Val(dt.Rows[0]["DEPT_ID_UP"].ToString().Trim()).ToString("000000");
                        //break;

                        dt.Dispose();
                        dt = null;
                        dt2.Dispose();
                        dt2 = null;
                    }
                }

                Cursor.Current = Cursors.Default;
                return strBuCode;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strBuCode;
            }
        }

        public static string BAS_BUSE_TREE(PsmhDb pDbCon, string argBucode)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nCnt = 0;
            int j = 0;
            string strBuCode = "";
            string[] strBucodetemp = new string[21];

            strBuCode = "'" + argBucode.PadLeft(6, '0') + "'";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //LEVEL 읽기
                SQL = "";
                SQL = "SELECT MAX(DEPT_LEVEL) DEPT_LEVEL";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BUSE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strBuCode;
                }

                nCnt = 0;
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return strBuCode;
                }

                nCnt = Convert.ToInt32(VB.Val(dt.Rows[0]["DEPT_LEVEL"].ToString().Trim()));

                dt.Dispose();
                dt = null;

                for (i = 0; i < strBucodetemp.Length; i++)
                {
                    strBucodetemp[i] = "";
                }

                //루프로 하의 부서 코드를 읽음
                strBucodetemp[1] = "'" + argBucode.PadLeft(6, '0') + "'";

                for (i = 1; i < nCnt; i++)
                {
                    SQL = "";
                    SQL = "SELECT DEPT_ID, DEPT_ID_UP";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
                    SQL = SQL + ComNum.VBLF + " WHERE DEPT_ID_UP IN ( " + strBucodetemp[i] + " ) ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return strBuCode;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        break;
                    }

                    for (j = 0; j < dt.Rows.Count; j++)
                    {
                        if (j == 0)
                        {
                            strBucodetemp[i + 1] = "'" + dt.Rows[j]["DEPT_ID"].ToString().Trim().PadLeft(6, '0') + "'";
                        }
                        else
                        {
                            strBucodetemp[i + 1] = strBucodetemp[i + 1] + ",'" + dt.Rows[j]["DEPT_ID"].ToString().Trim().PadLeft(6, '0') + "'";
                        }

                        strBuCode = strBuCode + ",'" + dt.Rows[j]["DEPT_ID"].ToString().Trim().PadLeft(6, '0') + "'";
                    }

                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;

                return strBuCode;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strBuCode;
            }
        }

        public static string READ_BAS_BUSE(PsmhDb pDbCon, string strBuseCode)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string rtnVal = "";

            try
            {
                SQL = "SELECT Name,SName FROM ADMIN.BAS_BUSE ";
                SQL += ComNum.VBLF + " WHERE BuCode='" + strBuseCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
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

                rtnVal = dt.Rows[0]["Name"].ToString().Trim().Length > 0 ? dt.Rows[0]["Name"].ToString().Trim() : dt.Rows[0]["SName"].ToString().Trim();

                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }
    }
}
