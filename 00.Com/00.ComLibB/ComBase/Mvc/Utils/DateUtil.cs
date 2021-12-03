using ComBase.Mvc.Enums;
using ComBase.Controls;
using ComBase.Mvc.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Utils
{
    /// <summary>
    ///  DateTime 관련 Extension 메소드 
    /// </summary>
    public static class DateUtil
    {

        /// <summary>
        /// 문자열을 DateTim으로 변환
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="DateTimeType"></param>
        /// <returns></returns>
        public static DateTime stringToDateTime(string dateTime, DateTimeType DateTimeType)
        {
            return GetStringToDateTIme(dateTime, DateTimeType);
        }

        private static DateTime GetStringToDateTIme(string dateTime, DateTimeType DateTimeType)
        {
            try
            {
                DateTime _dateTime = default(DateTime);

                switch (DateTimeType)
                {
                    case DateTimeType.YYYYMMDD: _dateTime = DateTime.ParseExact(dateTime, "yyyyMMdd", null); break;
                    case DateTimeType.YYYYMM: _dateTime = DateTime.ParseExact(dateTime, "yyyyMM", null); break;
                    case DateTimeType.YYYY: _dateTime = DateTime.ParseExact(dateTime, "yyyy", null); break;
                    case DateTimeType.YYYYMMDDHHMM: _dateTime = DateTime.ParseExact(dateTime, "yyyyMMddHHmm", null); break;
                    case DateTimeType.YYYYMMDDHHMMSS: _dateTime = DateTime.ParseExact(dateTime, "yyyyMMddHHmmss", null); break;
                    case DateTimeType.YYYY_MM_DD: _dateTime = DateTime.ParseExact(dateTime, "yyyy-MM-dd", null); break;
                    case DateTimeType.YYYY_MM: _dateTime = DateTime.ParseExact(dateTime, "yyyy-MM", null); break;
                    case DateTimeType.YYYY_MM_DD_HH_MM: _dateTime = DateTime.ParseExact(dateTime, "yyyy-MM-dd HH:mm", null); break;
                    case DateTimeType.YYYY_MM_DD_HH_MM_SS: _dateTime = DateTime.ParseExact(dateTime, "yyyy-MM-dd HH:mm:ss", null); break;
                    case DateTimeType.HH_MM_SS: _dateTime = _dateTime = DateTime.ParseExact(dateTime, "HH:mm:ss", null); break;
                    case DateTimeType.HH_MM: _dateTime = DateTime.ParseExact(dateTime, "HH:mm", null); break;
                }

                return _dateTime;
            }
            catch (FormatException ex)
            {
                throw new MTSException(dateTime + " 문자열이 " + DateTimeType.ToString() + " 유효하지 않습니다.", ex);
            }

        }
        public static string TodayAsYYYY_MM_DD()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// DateTime을 문자열로 변환
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="DateTimeType"></param>
        /// <returns></returns>
        public static string DateTimeToStrig(DateTime dateTime, DateTimeType DateTimeType)
        {
            return GetDateToString(dateTime, DateTimeType);
        }

        /// <summary>
        /// DateTime을 문자열로 변환
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="DateTimeType"></param>
        /// <returns></returns>
        public static string DateTimeToStrig(DateTime? dateTime, DateTimeType DateTimeType)
        {

            DateTime date = Convert.ToDateTime(dateTime);
            return GetDateToString(date, DateTimeType);

        }
        private static string GetDateToString(DateTime dateTime, DateTimeType DateTimeType)
        {
            string text = string.Empty;
            switch (DateTimeType)
            {
                case DateTimeType.YYYYMMDD: text = dateTime.ToString("yyyyMMdd"); break;
                case DateTimeType.YYYYMM: text = dateTime.ToString("yyyyMM"); break;
                case DateTimeType.YYYY: text = dateTime.ToString("yyyy"); break;
                case DateTimeType.YYYYMMDDHHMM: text = dateTime.ToString("yyyyMMddHHmm"); break;
                case DateTimeType.YYYYMMDDHHMMSS: text = dateTime.ToString("yyyyMMddHHmmss"); break;
                case DateTimeType.YYYY_MM_DD: text = dateTime.ToString("yyyy-MM-dd"); break;
                case DateTimeType.YYYY_MM: text = dateTime.ToString("yyyy-MM"); break;
                case DateTimeType.YYYY_MM_DD_HH_MM: text = dateTime.ToString("yyyy-MM-dd HH:mm"); break;
                case DateTimeType.YYYY_MM_DD_HH_MM_SS: text = dateTime.ToString("yyyy-MM-dd HH:mm:ss"); break;
                case DateTimeType.HH_MM_SS: text = dateTime.ToString("HH:mm:ss"); break;
                case DateTimeType.HH_MM: text = dateTime.ToString("HH:mm"); break;
                case DateTimeType.HHMM: text = dateTime.ToString("HHmm"); break;
            }
            return text;
        }
        /// <summary>
        /// day만큼 일수를 빼고 yyyy-MM-dd 형식의 문자열을 리턴합니다.
        /// </summary>
        /// <param name="dateTIme"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string MinusDayToShort(this DateTime dateTIme, int day)
        {

            return dateTIme.AddDays(-day).ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// DateTime을 string으로 변환(yyyy-MM-dd) 하여 리턴합니다
        /// </summary>
        /// <param name="dateTIme"></param>
        /// <returns></returns>
        public static string ToShort(this DateTime dateTIme)
        {
            return dateTIme.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 주어진 날짜형식(format)으로 변환합니다
        /// </summary>
        /// <param name="dateTIme"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToShort(this DateTime dateTIme, string format)
        {
            return dateTIme.ToString(format);
        }
        public static string ToKorean(this DateTime dateTime, DateTimeType DateTimeType)
        {
            DateTime date = dateTime;
            return date.Year + " 년" + date.Month + " 월" + date.Day + " 일";
        }
        public static string ToKorean(this string dateTime, DateTimeType DateTimeType)
        {
            DateTime date = stringToDateTime(dateTime, DateTimeType);
            return date.Year + " 년" + date.Month + " 월" + date.Day + " 일";
        }
        public static string ToKorean()
        {
            DateTime date = DateTime.Now;
            return date.Year + " 년" + date.Month + " 월" + date.Day + " 일";
        }
        /// <summary>
        ///  DateTime을 string으로 변환(yyyy-MM-dd hh:mm:ss) 리턴합니다
        /// </summary>
        /// <param name="dateTIme"></param>
        /// <returns></returns>
        public static string ToLong(this DateTime dateTIme)
        {
            return dateTIme.ToString("yyyy-MM-dd hh:mm:ss");
        }

        public static string ToDayOfWeek(this string dateTime, DateTimeType dateTimeType)
        {
            DateTime date = stringToDateTime(dateTime, dateTimeType);
            switch (Convert.ToInt32(date.DayOfWeek))
            {
                case 0:
                    return "일요일";
                case 1:
                    return "월요일";
                case 2:
                    return "화요일";
                case 3:
                    return  "수요일";
                case 4:
                    return  "목요일";
                case 5:
                    return  "금요일";
                case 6:
                    return  "토요일";
                default:
                    return  "";
            }
        }
        public static string ToDayOfWeek(this DateTime dateTime)
        {
            switch (Convert.ToInt32(dateTime.DayOfWeek))
            {
                case 0:
                    return "일요일";
                case 1:
                    return "월요일";
                case 2:
                    return "화요일";
                case 3:
                    return "수요일";
                case 4:
                    return "목요일";
                case 5:
                    return "금요일";
                case 6:
                    return "토요일";
                default:
                    return "";
            }
        }
        /// <summary>
        /// 날짜를 소셜형태의 날짜형식으로 리턴합니다. ex) 방금전, 1시간전
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static String GetPrettyDate(this DateTime date)
        {
            TimeSpan s = DateTime.Now.Subtract(date);

            double dayDiff = (int)s.TotalDays;

            double secDiff = (int)s.TotalSeconds;

            if (dayDiff < 0)
            {
                return null;
            }

            if (dayDiff == 0)
            {
                if (secDiff < 60)
                {
                    return "방금전";
                }
                if (secDiff < 120)
                {
                    return "1분 전";
                }
                if (secDiff < 3600)
                {
                    return string.Format("{0}분 전", Math.Floor((double)secDiff / 60));
                }
                if (secDiff < 7200)
                {
                    return "1시간 전";
                }
                if (secDiff < 86400)
                {
                    return string.Format("{0}시간 전",
                        Math.Floor((double)secDiff / 3600));
                }
            }
            if (dayDiff == 1)
            {
                return "어제";
            }
            if (dayDiff < 7)
            {
                return string.Format("{0}일 전",
                    dayDiff);
            }
            if (dayDiff < 91)
            {
                return string.Format("{0}주 전",
                    Math.Ceiling((double)dayDiff / 7));
            }
            return date.ToShortDateString();
        }
    }
}