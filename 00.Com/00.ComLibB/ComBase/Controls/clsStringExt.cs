using System;
using System.Text;

namespace ComBase.Controls
{
    public static class clsStringExt
    {
        /// <summary>
        /// 문자열 객체의 값을 문자열 값의 배열로 검사합니다.
        /// </summary>
        /// <param name = "stringValues"> 비교할 문자열 값의 배열 </param>
        /// <returns> 문자열 값이 일치하면 true를 반환합니다. </returns>
        public static bool In(this string value, params string[] stringValues)
        {
            foreach (string otherValue in stringValues)
            {
                if (string.Compare(value, otherValue) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 지정된 길이의 오른쪽에서 문자를 반환합니다.
        /// </ summary>
        /// <param name = "value"> 문자열 값 </ param>
        /// <param name = "length"> 반환 할 문자의 최대 수 </ param>
        /// <returns> 오른쪽에서 문자열을 반환합니다. </ returns>
        public static string Right(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(value.Length - length) : value;
        }

        /// <summary>
        /// 지정된 길이의 왼쪽부터 문자를 반환합니다.
        /// </summary>
        /// <param name = "value"> 문자열 값 </param>
        /// <param name = "length"> 반환 할 문자의 최대 수 </param>
        /// <returns> 왼쪽에서 문자열을 반환합니다. </returns>
        public static string Left(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(0, length) : value;
        }

        /// <summary>
        /// 지정된 System.String의 형식 항목을 해당하는 텍스트로 바꿉니다.
        /// 지정된 System.Object 인스턴스의 값을 반환합니다.
        /// </summary>
        /// <param name = "value"> 복합 형식 문자열 </param>
        /// <param name = "arg0"> 포맷 할 System.Object </param>
        /// <returns> 첫 번째 형식 항목이
        /// System.String arg0와 동일합니다. </returns>
        public static string Format(this string value, object arg0)
        {
            return string.Format(value, arg0);
        }

        /// <summary>
        /// 지정된 System.String의 형식 항목을 해당하는 텍스트로 바꿉니다.
        /// 지정된 System.Object 인스턴스의 값을 반환합니다.
        /// </summary>
        /// <param name = "value"> 복합 형식 문자열 </param>
        /// <param name = "args"> 포맷 할 객체가 0 개 이상있는 System.Object 배열입니다. </param>
        /// <returns> 형식 항목이 System.String으로 대체 된 형식의 복사본입니다.
        /// args에있는 System.Object의 해당 인스턴스와 동일합니다. </returns>
        public static string Format(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        /// <summary>
        /// 문자열을 지정된 길이로 자르고 잘린 것을 a로 바꾸십시오 ...
        /// </summary>
        /// <param name = "text"> 잘리는 문자열 </param>
        /// <param name = "maxLength"> 자르기 전에 유지해야 할 문자의 총 길이 </param>
        /// <returns> 잘린 문자열 </ returns>
        public static string Truncate(this string text, int maxLength)
        {
            // 잘린 문자열을 a로 대체합니다.
            const string suffix = "...";
            string truncatedString = text;

            if (maxLength <= 0) return truncatedString;
            int strLength = maxLength - suffix.Length;

            if (strLength <= 0) return truncatedString;

            if (text == null || text.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();
            truncatedString += suffix;
            return truncatedString;
        }

        public static string Concat2(this string text, params string[] list)
        {
            return string.Concat(text, list);
        }

        /// <summary>
        ///  문자열을 특정 Byte로 변환하고 남는 부분은 공백문자 체웁니다.
        ///  공백 제거가 필요하시면 Trim() 이용해 주세요.
        /// </summary>
        /// <param name="str">자를 문자</param>
        /// <param name="Byte">자를 바이트 수</param>
        /// <returns></returns>
        public static string SubStrByte(this string text, int Byte)
        {
            // ANSI 멀티바이트 문자열로 변환 하여 길이를 구한다.
            int inCnt = Encoding.Default.GetByteCount(text);
            if (inCnt > Byte)
            {
                int i = 0;
                for (i = text.Length - 1; inCnt > Byte; i--)
                {
                    //ANSI 문자는 1, 이외의 문자는 2자리로 계산한다
                    if (text[i] > 0x7f)
                    {
                        inCnt -= 2;
                    }
                    else
                    {
                        inCnt -= 1;
                    }
                }

                // i는 마지막 문자 인덱스이고 substring 의 두번째 파라미터는 길이이기 때문에 + 1 한다.
                text = text.Substring(0, i + 1);
                // ANSI 멀티바이트 문자열로 변환 하여 길이를 구한다.
                inCnt = Encoding.Default.GetByteCount(text);
            }
            //PadRight(len) 이 맞겠지만 유니코드 처리가 되기 때문에 멀티바이트 문자도 1로 
            //처리되어 길이가 일정하지 않기 때문에 아래와 같이 계산하여 Padding한다
            text = text.PadRight(text.Length + Byte - inCnt);
            return text;
        }
    }
}
