using System;
using System.ComponentModel;
using System.Reflection;

namespace ComBase.Controls
{
    public static class clsEnumExt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum value)
        {
            Type type = value.GetType();
            MemberInfo[] memInfo = type.GetMember(value.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return value.ToString();
        }

        /// <summary>
        /// 문자열을 enum 개체로 변환합니다.
        /// </summary>
        /// <typeparam name = "T"> 열거 형 유형 </typeparam>
        /// <param name = "value"> 변환 할 문자열 값 </param>
        /// <returns> enum 개체를 반환합니다. </returns>
        public static T ToEnum<T>(this string value) where T : struct
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
