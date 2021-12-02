using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComBase.Controls;
namespace ComBase.Mvc.Utils
{
    /// <summary>
    /// 문자열 관련 유틸 클래스
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// 숫자여부 확인
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string value)
        {
            foreach (char cData in value)
            {
                if (!Char.IsNumber(cData))
                {
                    return false;
                }
            }
            return true;
        }
        public static int ToInt(this string value)
        {
            if (value.IsNullOrEmpty())
            {
                return 0;
            }
            return int.Parse(value);
        }

        public static bool IsDate(this string value)
        {
            DateTime dt;

            if (!DateTime.TryParseExact(value, nameof(DateTimeType.YYYY_MM_DD), null, DateTimeStyles.AssumeLocal, out dt))
            {
                return false;
            }
            return true;
        }
    }
}
