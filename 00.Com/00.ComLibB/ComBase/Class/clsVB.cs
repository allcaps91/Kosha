using Microsoft.VisualBasic;
using System;

namespace ComBase
{
    /// <summary>
    /// VB 함수
    /// </summary>
    public class VB
    {
        /// <summary>
        /// Strings.Trim
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Trim(string str)
        {
            string rtnVal = str;
            try
            {
                rtnVal = str.Trim();
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.LTrim
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string LTrim(string str)
        {
            string rtnVal = str;
            try
            {
                rtnVal = Strings.LTrim(str);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.RTrim
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RTrim(string str)
        {
            string rtnVal = str;
            try
            {
                rtnVal = Strings.RTrim(str);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.Mid
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Start"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string Mid(string str, int Start, int Length)
        {
            string rtnVal = str;

            try
            {
                if(Start == 0)
                {
                    Start = 1;
                }

                rtnVal = Strings.Mid(rtnVal, Start, Length);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.Left
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string Left(string str, int Length)
        {
            string rtnVal = str;

            try
            {
                rtnVal = Strings.Left(rtnVal, Length);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.Right
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string Right(string str, int Length)
        {
            string rtnVal = str;

            try
            {
                rtnVal = Strings.Right(rtnVal, Length);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.Len
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static int Len(string Expression)
        {
            int rtnVal = 0;

            try
            {
                rtnVal = Strings.Len(Expression);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }


        /// <summary>
        /// Strings.InStr
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="String1"></param>
        /// <param name="String2"></param>
        /// <returns></returns>
        public static int InStr(int Start, string String1, string String2)
        {
            int rtnVal = 0;

            try
            {
                rtnVal = Strings.InStr(Start, String1, String2, CompareMethod.Text);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.InStr
        /// </summary>
        /// <param name="String1"></param>
        /// <param name="String2"></param>
        /// <returns></returns>
        public static int InStr(string String1, string String2)
        {
            int rtnVal = 0;

            try
            {
                rtnVal = Strings.InStr(String1, String2, CompareMethod.Text);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.Space
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public static string Space(int Number)
        {
            string rtnVal = "";

            try
            {
                rtnVal = Strings.Space(Number);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.
        /// </summary>
        /// <param name="CharCode"></param>
        /// <returns></returns>
        public static char ChrW(int CharCode)
        {
            char rtnVal = (char)0;

            try
            {
                rtnVal = Strings.ChrW(CharCode);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.
        /// </summary>
        /// <param name="CharCode"></param>
        /// <returns></returns>
        public static char Chr(int CharCode)
        {
            char rtnVal = (char)0;

            try
            {
                rtnVal = Strings.Chr(CharCode);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.
        /// C# .ToLower 사용하세요
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string LCase(string Value)
        {
            string rtnVal = Value;

            try
            {
                rtnVal = Strings.UCase(Value);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.
        /// C# .ToUpper 사용하세요
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string UCase(string Value)
        {
            string rtnVal = Value;

            try
            {
                rtnVal = Strings.UCase(Value);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="Delimiter"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        public static string[] Split(string Expression, string Delimiter = " ", int Limit = -1)
        {
            string[] rtnVal = null;

            try
            {
                rtnVal = Strings.Split(Expression, Delimiter, Limit, CompareMethod.Text);
                return rtnVal;

            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="Find"></param>
        /// <param name="Replacement"></param>
        /// <param name="Start"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static string Replace(string Expression, string Find, string Replacement, int Start = 1, int Count = -1)
        {
            string rtnVal = "";

            try
            {
                rtnVal = Expression;
                rtnVal = Strings.Replace(Expression, Find, Replacement, Start, Count, CompareMethod.Text);
                return rtnVal;

            }
            catch
            {
                rtnVal = "";
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="Style"></param>
        /// <returns></returns>
        public static string Format(object Expression, string Style = "")
        {
            string rtnVal = "";
            try
            {
                rtnVal = Expression.ToString();

                rtnVal = Strings.Format(Expression, Style);
                return rtnVal;

            }
            catch
            {
                rtnVal = "";
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.
        /// </summary>
        /// <param name="String"></param>
        /// <returns></returns>
        public static int Asc(string String)
        {
            int rtnVal = 0;

            try
            {
                rtnVal = Strings.Asc(String);
                return rtnVal;

            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// Strings.
        /// </summary>
        /// <param name="String"></param>
        /// <returns></returns>
        public static int AscW(string String)
        {
            int rtnVal = 0;
            rtnVal = Strings.AscW(String);
            return rtnVal;
        }

        /// <summary>
        /// String
        /// </summary>
        /// <param name="intLen"></param>
        /// <param name="String"></param>
        /// <returns></returns>
        public static string String(int intLen, string String)
        {
            string rtnVal = "";
            for(int i = 1; i <= intLen; i++)
            {
                rtnVal = rtnVal + String;
            }
            return rtnVal;
        }

        /// <summary>
        /// DateAndTime.
        /// </summary>
        /// <returns></returns>
        public static DateTime Now()
        {
            DateTime rtnVal = DateAndTime.Now;
            return rtnVal;
        }

        /// <summary>
        /// DateAndTime.
        /// </summary>
        /// <returns></returns>
        public static DateTime Today()
        {
            DateTime rtnVal = DateAndTime.Today;
            return rtnVal;
        }

        /// <summary>
        /// DateAndTime.
        /// </summary>
        /// <param name="Interval"></param>
        /// <param name="Number"></param>
        /// <param name="DateValue"></param>
        /// <returns></returns>
        public static DateTime DateAddVB(DateInterval Interval, double Number, DateTime DateValue)
        {
            DateTime rtnVal = Convert.ToDateTime(DateAndTime.DateAdd(Interval, Number, DateValue));
            return rtnVal;
        }

        /// <summary>
        /// DateAndTime.
        /// </summary>
        /// <param name="Interval"></param>
        /// <param name="Number"></param>
        /// <param name="DateValue"></param>
        /// <returns></returns>
        public static DateTime DateAdd(string Interval, double Number, object DateValue)
        {
            DateTime rtnVal;
            rtnVal = Convert.ToDateTime(DateAndTime.DateAdd(Interval, Number, DateValue));
            return rtnVal;
        }

        /// <summary>
        /// DateAndTime.
        /// </summary>
        /// <param name="Interval"></param>
        /// <param name="Date1"></param>
        /// <param name="Date2"></param>
        /// <returns></returns>
        public static long DateDiffVB(DateInterval Interval, DateTime Date1, DateTime Date2)
        {
            long rtnVal = DateAndTime.DateDiff(Interval, Date1, Date2, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
            return rtnVal;
        }

        /// <summary>
        /// DateAndTime.
        /// </summary>
        /// <param name="Interval"></param>
        /// <param name="Date1"></param>
        /// <param name="Date2"></param>
        /// <returns></returns>
        public static long DateDiff(string Interval, object Date1, object Date2)
        {
            //s Second
            //n Minute
            //h Hour
            //d Day
            //m Month
            //yyyy Year
            //y Day of year
            //q Quarter
            //ww Week of year
            //w Weekday
            long rtnVal = DateAndTime.DateDiff(Interval, Date1, Date2, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
            return rtnVal;
        }

        /// <summary>
        /// DateAndTime.
        /// </summary>
        /// <param name="Interval"></param>
        /// <param name="Date1"></param>
        /// <returns></returns>
        public static long DatePart(string Interval, object Date1)
        {
            long rtnVal = DateAndTime.DatePart(Interval, Date1, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1);
            return rtnVal;
        }

        /// <summary>
        /// DateAndTime.
        /// </summary>
        /// <param name="DateValue"></param>
        /// <returns></returns>
        public static int Day(DateTime DateValue)
        {
            int rtnVal = DateAndTime.Day(DateValue);
            return rtnVal;
        }

        /// <summary>
        /// DateAndTime.
        /// </summary>
        /// <param name="TimeValue"></param>
        /// <returns></returns>
        public static int Hour(DateTime TimeValue)
        {
            int rtnVal = DateAndTime.Hour(TimeValue);
            return rtnVal;
        }

        /// <summary>
        /// DateAndTime.
        /// </summary>
        /// <param name="TimeValue"></param>
        /// <returns></returns>
        public static int Minute(DateTime TimeValue)
        {
            int rtnVal = DateAndTime.Minute(TimeValue);
            return rtnVal;
        }

        /// <summary>
        /// DateAndTime.
        /// </summary>
        /// <param name="DateValue"></param>
        /// <returns></returns>
        public static int Month(DateTime DateValue)
        {
            int rtnVal = DateAndTime.Month(DateValue);
            return rtnVal;
        }

        /// <summary>
        /// DateAndTime.
        /// </summary>
        /// <param name="DateValue"></param>
        /// <returns></returns>
        public static int Year(DateTime DateValue)
        {
            int rtnVal = DateAndTime.Year(DateValue);
            return rtnVal;
        }

        /// <summary>
        /// DateAndTime.
        /// </summary>
        /// <param name="DateValue"></param>
        /// <returns></returns>
        public static int Weekday(DateTime DateValue)
        {
            int rtnVal = DateAndTime.Weekday(DateValue, FirstDayOfWeek.Sunday);
            return rtnVal;
        }

        /// <summary>
        /// Conversion.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double Val(string str)
        {
            double rtnVal = 0;

            try
            {
                rtnVal = Conversion.Val(str);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        public static int Int(int Number)
        {
            int rtnVal = 0;

            try
            {
                rtnVal = Conversion.Int(Number);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        public static int Fix(int Number)
        {
            int rtnVal = 0;

            try
            {
                rtnVal = Conversion.Fix(Number);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        public static double FixDbl(double Number)
        {
            double rtnVal = 0;

            try
            {
                rtnVal = Conversion.Fix(Number);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        public static string Hex(int Number)
        {
            string rtnVal = "";

            try
            {
                rtnVal = Conversion.Hex(Number);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        public static string Command()
        {
            string rtnVal = "";

            try
            {
                rtnVal = Interaction.Command();
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        public static int Shell(string PathName, string strStyle = "NormalFocus", bool Wait = false, int Timeout = -1)
        {
            int rtnVal = 0;
            AppWinStyle Style = AppWinStyle.NormalNoFocus;
            switch(strStyle)
            {
                case "Hide":
                    Style = AppWinStyle.Hide;
                    break;
                case "MaximizedFocus":
                    Style = AppWinStyle.MaximizedFocus;
                    break;
                case "MinimizedFocus":
                    Style = AppWinStyle.MinimizedFocus;
                    break;
                case "MinimizedNoFocus":
                    Style = AppWinStyle.MinimizedNoFocus;
                    break;
                case "NormalFocus":
                    Style = AppWinStyle.NormalFocus;
                    break;
                case "NormalNoFocus":
                    Style = AppWinStyle.NormalNoFocus;
                    break;
            }
            try
            {
                rtnVal = Interaction.Shell(PathName, Style, Wait, Timeout);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// (a==b ? true:false) 사용하세요.
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="TruePart"></param>
        /// <param name="FalsePart"></param>
        /// <returns></returns>
        public static object IIf(bool Expression, object TruePart, object FalsePart)
        {
            object rtnVal = null;
            try
            {
                rtnVal = Interaction.IIf(Expression, TruePart, FalsePart);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        public static string GetSetting(string AppName, string Section, string Key, string Default = "")
        {
            string rtnVal = "";
            try
            {
                rtnVal = Interaction.GetSetting(AppName, Section, Key, Default);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }
        public static void SaveSetting(string AppName, string Section, string Key, string Setting)
        {
            try
            {
                Interaction.SaveSetting(AppName, Section, Key, Setting);
            }
            catch
            {

            }
        }

        public static string InputBox(string Prompt, string Title = "", string Default = "", int XPos = -1, int YPos = -1)
        {
            string rtnVal = "";
            try
            {
                rtnVal = Interaction.InputBox(Prompt, Title, Default, XPos, YPos);

                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        public static object GetObject(string PathName = "", string Class = "")
        {
            object rtnVal = null;

            try
            {
                rtnVal = Interaction.GetObject(PathName, Class);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        public static bool IsNumeric(object Expression)
        {
            bool rtnVal = false;

            try
            {
                rtnVal = Information.IsNumeric(Expression);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }


        public static bool IsHangul(string strText)
        {
            int ncount = 0;
            int iq = 0;

            char[] chrarr = strText.ToCharArray();


            for (iq = 0; iq < chrarr.Length; iq++)
            {
                int iASCii = Convert.ToInt32(chrarr[iq]);
                if (iASCii < 0 || iASCii > 127)
                {
                    ncount = ncount + 1;
                    break;
                }
            }

            if (ncount > 0)
                return true;      //한글
            else
                return false;     //영문

        }


        public static int UBound(Array Array, int Rank = 1)
        {
            int rtnVal = 0;

            try
            {
                rtnVal = Information.UBound(Array);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        public static int LBound(Array Array, int Rank = 1)
        {
            int rtnVal = 0;

            try
            {
                rtnVal = Information.LBound(Array);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        public static bool IsDate(object Expression)
        {
            bool rtnVal = false;

            try
            {
                rtnVal = Information.IsDate(Expression);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        public static bool IsNull(object Expression)
        {
            bool rtnVal = false;

            try
            {
                rtnVal = Information.IsNothing(Expression);
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// vbstring.P
        /// </summary>
        /// <param name="strVar"></param>
        /// <param name="strDel"></param>
        /// <param name="intFromCnt"></param>
        /// <returns></returns>
        public static string Pstr(string strVar, string strDel, int intFromCnt, int intArg1 = 0, int intArg2 = 0)
        {
            //' (c)CopyRight 1997.12.16  made by hyuntae Jo   use TypeName function
            string rtnVal = "";

            double dblSum = VB.Val(IIf(intArg1 > 0, 1, 0).ToString()) + VB.Val(IIf(intArg2 > 0, 1, 0).ToString());
            
            switch (dblSum.ToString())
            {
                case "2":
                    rtnVal = MultiPieceSet(strVar, strDel, intFromCnt, intArg1, intArg2.ToString());
                    break;
                case "1":
                    rtnVal = MultiPiece(strVar, strDel, intFromCnt, intArg1);
                    break;
                case "0":
                    rtnVal = SinglePiece(strVar, strDel, intFromCnt);
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// vbstring.PP
        /// </summary>
        /// <param name="strVar"></param>
        /// <param name="strDel"></param>
        /// <param name="intFromCnt"></param>
        /// <returns></returns>
        public static string PP(string strVar, string strDel, int intFromCnt, int intArg1 = 0, int intArg2 = 0)
        {
            //' (c)CopyRight 1997.12.16  made by hyuntae Jo   use TypeName function
            string rtnVal = "";

            double dblSum = VB.Val(IIf(intArg1 > 0, 1, -1).ToString()) + VB.Val(IIf(intArg2 > 0, 1, -1).ToString());

            switch (dblSum.ToString())
            {
                case "-2":
                    rtnVal = SinglePiece(strVar, strDel, intFromCnt);                    
                    break;
                case "-1":
                    rtnVal = MultiPiece(strVar, strDel, intFromCnt, intArg1);
                    break;
                case "0":
                    rtnVal = MultiPieceSet(strVar, strDel, intFromCnt, intArg1, intArg2.ToString());
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// vbstring.P
        /// </summary>
        /// <param name="strVar"></param>
        /// <param name="strDel"></param>
        /// <param name="intCnt"></param>
        /// <returns></returns>
        public static string SinglePiece(string strVar, string strDel, int intCnt)
        {
            string rtnVal = "";

            int intPrt = 0;
            int intSrt = 0;
            int intNxt = 0;

            if(intCnt <= 0)
            {
                return rtnVal;
            }

            intNxt = (Len(strDel) * -1) + 1;

            for(intPrt = 1; intPrt <= intCnt; intPrt++)
            {
                intSrt = intNxt + Len(strDel);
                intNxt = InStr(intSrt, strVar, strDel);

                if(intNxt == 0)
                {
                    intNxt = Len(strVar) + Len(strDel);
                    break;
                }
            }

            if(intPrt >= intCnt)
            {
                rtnVal = Mid(strVar, intSrt, (intNxt - intSrt));
            }

            return rtnVal;
        }

        /// <summary>
        /// vbstring.P
        /// </summary>
        /// <param name="strVar"></param>
        /// <param name="strDel"></param>
        /// <param name="intFromCnt"></param>
        /// <param name="intToCnt"></param>
        /// <returns></returns>
        public static string MultiPiece(string strVar, string strDel, int intFromCnt, int intToCnt)
        {
            //' (c)CopyRight 1997.07.15  made by hyuntae Jo   multi-delimiter support
            string rtnVal = "";

            int intPrt = 0;
            int intSrt = 0;
            int intNxt = 0;
            int intFromBuf = 0;

            if(intFromCnt > intToCnt)
            {
                return rtnVal;
            }

            if(intFromCnt < 1)
            {
                intFromCnt = 1;
            }

            intNxt = (Len(strDel) * -1) + 1;

            for(intPrt = 1; intPrt <= intToCnt; intPrt++)
            {
                intSrt = intNxt + Len(strDel);
                intNxt = InStr(intSrt, strVar, strDel);

                if(intPrt == intFromCnt)
                {
                    intFromBuf = intSrt;
                }

                if(intNxt == 0)
                {
                    intNxt = Len(strVar) + Len(strDel);
                    break;
                }
            }

            if(intFromBuf == 0)
            {
                return rtnVal;
            }

            rtnVal = Mid(strVar, intFromBuf, (intNxt - intFromBuf));
            return rtnVal;
        }

        /// <summary>
        /// vbstring.SinglePieceSet
        /// </summary>
        /// <param name="strVar"></param>
        /// <param name="strDel"></param>
        /// <param name="intCnt"></param>
        /// <param name="strXCH"></param>
        /// <returns></returns>
        public static string SinglePieceSet(string strVar, string strDel, int intCnt, string strXCH)
        {
            //' (c)CopyRight 1997.07.15  made by hyuntae Jo   multi-delimiter support
            string rtnVal = "";

            int intPrt = 0;
            int intSrt = 0;
            int intNxt = 0;

            if(intCnt == 0)
            {
                return rtnVal;
            }

            intNxt = (Len(strDel) * -1) + 1;

            for(intPrt = 1; intPrt <= intCnt; intPrt++)
            {
                intSrt = intNxt + Len(strDel);
                intNxt = InStr(intSrt, strVar, strDel);

                if(intNxt == 0)
                {
                    intNxt = Len(strVar) + Len(strDel);
                    break;
                }
            }

            if(intPrt >= intCnt)
            {
                rtnVal = Left(strVar, intSrt - 1) + strXCH + Mid(strVar, intNxt, Len(strVar) - intNxt + Len(strDel));
            }
            else
            {
                for(intSrt = 1; intSrt <= (intCnt - intPrt); intSrt++)
                {
                    strVar = strVar + strDel;
                }

                rtnVal = strVar + strXCH;
            }

            return rtnVal;
        }

        /// <summary>
        /// vbstring.MultiPieceSet
        /// </summary>
        /// <param name="strVar"></param>
        /// <param name="strDel"></param>
        /// <param name="intFromCnt"></param>
        /// <param name="intToCnt"></param>
        /// <param name="strXCH"></param>
        /// <returns></returns>
        public static string MultiPieceSet(string strVar, string strDel, int intFromCnt, int intToCnt, string strXCH)
        {
            //' (c)CopyRight 1997.07.15  made by hyuntae Jo   multi-delimiter support
            string rtnVal = "";

            int intPrt = 0;
            int intSrt = 0;
            int intNxt = 0;
            int intFromBuf = 0;

            if(intFromCnt > intToCnt)
            {
                return rtnVal;
            }

            if(intFromCnt < 1)
            {
                intFromCnt = 1;
            }

            if(strDel == "")
            {
                rtnVal = Left(strVar, intFromCnt - 1) + strXCH + Mid(strVar, intToCnt + 1, Len(strVar));
                return rtnVal;
            }

            intNxt = (Len(strDel) * -1) + 1;

            for(intPrt = 1; intPrt <= intToCnt; intPrt++)
            {
                intSrt = intNxt + Len(strDel);
                intNxt = InStr(intSrt, strVar, strDel);

                if(intPrt == intFromCnt)
                {
                    intFromBuf = intSrt;
                }

                if(intNxt == 0)
                {
                    intNxt = Len(strVar) + Len(strDel);
                    break;
                }
            }

            if(intFromBuf > 0)
            {
                rtnVal = Left(strVar, intFromBuf - 1) + strXCH + Mid(strVar, intNxt, Len(strVar) - intNxt + Len(strDel));
            }
            else
            {
                for(intSrt = 1; intSrt <= intFromCnt - intPrt; intSrt++)
                {
                    strVar = strVar + strDel;
                }

                rtnVal = strVar + strXCH;
            }

            return rtnVal;
        }

        /// <summary>
        /// vbstring.HExtract
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="strDel"></param>
        /// <param name="intGetCnt"></param>
        /// <returns></returns>
        public static string HExtract(string strVar, string strDel, int intGetCnt)
        {
            //' (c)CopyRight 1998.11.05  made by hyuntae Jo  한글 단어보호
            string rtnVal = "";

            string strBUF = "";
            string strTMP = "";
            int intnum = 0;
            int intCnt = 0;

            if(strVar == "" || intGetCnt < 2)
            {
                return rtnVal;
            }

            do
            {
                intCnt = intCnt + 1;
                strTMP = Mid(strVar, intCnt, 1);
                intnum = intnum + 1;

                if(Asc(strTMP) < 0)
                {
                    intnum = intnum + 1;
                }

                if(intnum < intGetCnt)
                {
                    strBUF = strBUF + strTMP;
                }
                else if(intnum == intGetCnt)
                {
                    intnum = 0;
                    strBUF = strBUF + strTMP + strDel;
                }
                else if(intnum > intGetCnt)
                {
                    intnum = 2;
                    strBUF = strBUF + strDel + strTMP;
                }

            }
            while(intCnt >= Len(strVar));

            if(Right(strBUF, 1) == strDel)
            {
                strBUF = Left(strBUF, Len(strBUF) - 1);
            }

            return rtnVal;
        }

        /// <summary>
        /// vbstring.TR
        /// </summary>
        /// <param name="strVar"></param>
        /// <param name="strDel"></param>
        /// <param name="strXCH"></param>
        /// <returns></returns>
        public static string TR(string strVar, string strDel, string strXCH)
        {
            //' (c)CopyRight 1997.05.03  made by hyuntae Jo   multi-delimiter support
            string rtnVal = "";

            int intSrt = 0;
            int intNxt = 0;
            int intTMP = 0;
            string strBUF = "";

            if(strDel == "")
            {
                rtnVal = strVar;
                return rtnVal;
            }

            intNxt = (Len(strDel) * -1) + 1;
            if (intNxt == 0)
            {
                rtnVal = strVar;
                return rtnVal;
            }
            while (intNxt != 0)
            {
                intSrt = intNxt + Len(strDel);
                intTMP = intNxt + Len(strDel) - 1;
                intNxt = InStr(intSrt, strVar, strDel);

                if (intNxt != 0)
                {
                    strBUF = strBUF + Mid(strVar, intSrt, (intNxt - intSrt)) + strXCH;
                }
                else
                {
                    strBUF = strBUF + Right(strVar, Len(strVar) - intTMP);
                }
            };

            rtnVal = strBUF;

            return rtnVal;
        }

        /// <summary>
        /// VBStrUtil.bas : L
        /// </summary>
        /// <param name="var"></param>
        /// <param name="Del"></param>
        /// <returns></returns>
        public static long L(string var, string Del)
        {
            //(c)CopyRight 1997.05.03  made by hyuntae Jo   multi - delimiter support

            long Srt = 0;
            long Nxt = 0;
            long Cnt = 0;
            long rtnVal = 0;

            if (Del == "")
            {
                rtnVal = 0;
                return rtnVal;
            }

            Nxt = ((Del.Length) * -1) + 1;

            while (true)
            {
                Srt = Nxt + Del.Length;
                Nxt = Strings.InStr(Convert.ToInt32(Srt), var, Del);
                Cnt += 1;
                if (Nxt == 0)
                {
                    break;
                }
            }


            rtnVal = Cnt;
            return rtnVal;
        }

        //---( 문자열에서 특정문자 ~ 특정문자 사이의 문자열을 가져옴 )--------
        //
        //    비교문자는 대소문자를 구분 안함
        //
        // ⓐ Cut("제목:일정통보<end>","제목:","<end>") --> "일정통보"
        // ⓑ Cut("제목:일정통보<end>","","<end>") -------> "제목:일정통보"
        // ⓒ Cut("제목:일정통보<end>","제목:","") -------> "일정통보<end>"

        /// <summary>
        /// vbstring.STRCUT  '---( 문자열에서 특정문자 ~ 특정문자 사이의 문자열을 가져옴 )--------
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="strMode"></param>
        /// <param name="Date2"></param>
        /// <returns></returns>
        public static string STRCUT(string ArgString, string ArgStart, string ArgEnd)
        {
            string rtnVal = "";

            int i = 0;
            int nStartPos = 0;
            int nEndPos = 0;

            string strString = "";
            string strStart = "";
            string strEnd = "";

            //문자열이 공란이면 공란을 Return
            if(ArgString == "")
            {
                return rtnVal;
            }

            //짜를 시작문자,종료문자가 모두 NULL이면 전체문장을 Return
            if(ArgStart == "" && ArgEnd == "")
            {
                rtnVal = ArgString;
                return rtnVal;
            }

            //비교대상,비교할 단어를 소문자로 변경함
            strString = LCase(ArgString);
            strStart = LCase(ArgStart);
            strEnd = LCase(ArgEnd);

            //-----------------------------------
            // 문자열중 짜를 시작위치를 찾음
            //-----------------------------------

            if(ArgStart == "")
            {
                nStartPos = 1;
            }
            else
            {
                nStartPos = InStr(strString, strStart);

                if(nStartPos > 0)
                {
                    nStartPos = nStartPos + Len(strStart);
                }
                else
                {
                    //문장에 시작문장이 없으면 NULL을 Return
                    return rtnVal;
                }
            }

            //-----------------------------------
            // 문자열중 짜를 종료위치를 찾음
            //-----------------------------------
            if(ArgEnd == "")
            {
                nEndPos = Len(ArgString);
            }
            else
            {
                nEndPos = InStr(nStartPos, strString, strEnd);

                if(nEndPos > 0)
                {
                    nEndPos = nEndPos - 1;
                }
                else
                {
                    //문장에 종료문장이 없으면 NULL을 Return
                    return rtnVal;
                }
            }

            if(nEndPos >= nStartPos)
            {
                rtnVal = Mid(ArgString, nStartPos, (nEndPos - nStartPos + 1));
            }

            return rtnVal;
        }

        /// <summary>
        /// vbstring.Extract_String '2016-03-04 문자열에서 한글,영어(숫자), 특수문자 추출해내기 K.M.C
        /// </summary>
        /// <param name="strVar"></param>
        /// <param name="strMode"></param>
        /// <returns></returns>
        public static string ExtractString(string strVar, string strMode)
        {
            //ArgMode = "1"  -> 한글추출
            //ArgMode = "2"  -> 영어숫자추출
            //ArgMode = "3"  -> 특수기호추출

            string rtnVal = "";

            string strENG = "";
            string strKOR = "";
            string strETC = "";

            int i = 0;

            for(i = 1; i <= Len(strVar); i++)
            {
                if(Asc(Mid(strVar, i, 1)) >= 0 && Asc(Mid(strVar, i, 1)) <= 255)
                {
                    strENG = strENG + Mid(strVar, i, 1);
                }
                else if(Asc(Mid(strVar, i, 1)) >= -20319 && Asc(Mid(strVar, i, 1)) <= -14082)
                {
                    strKOR = strKOR + Mid(strVar, i, 1);
                }
                else
                {
                    strETC = strETC + Mid(strVar, i, 1);
                }

                if(Mid(strVar, i, 1) == "뉀")
                {
                    strKOR = strKOR + "뉀";
                }
            }

            switch(strMode)
            {
                case "1":
                    rtnVal = strKOR;
                    break;
                case "2":
                    rtnVal = strENG;
                    break;
                case "3":
                    rtnVal = strETC;
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// vbstring.l
        /// </summary>
        /// <param name="strVar"></param>
        /// <param name="strDel"></param>
        /// <returns></returns>
        public static int I(string strVar, string strDel)
        {
            int intSrt = 0;
            int intNxt = 0;
            int intCnt = 0;

            int rtnVal = 0;

            if (strDel == "")
            {
                return rtnVal;
            }

            intNxt = (VB.Len(strDel) * -1) + 1;

            do
            {
                intSrt = intNxt + VB.Len(strDel);
                intNxt = InStr(intSrt, strVar, strDel);

                intCnt = intCnt + 1;
            }
            while(intNxt != 0);

            rtnVal = intCnt;

            return rtnVal;
        }
    }
}
