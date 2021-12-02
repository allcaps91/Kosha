using ComBase.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Validation
{

    /// <summary>
    /// 속성타입이 문자열일때 사욥합니다 문자열의 최소 최대 길이 검사
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MTSStringLengthAttribute : Attribute, IMTSValidation
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public string Message { get; set; }
        public int Order { get; set; }

        public MTSStringLengthAttribute()
        {
            Message = Min + "보다 크고 " + Max + " 보다 작아야합니다";
        }

        public bool DoValidation(object value)
        {
            if (value.IsNullOrEmpty())
            {
                return false;
            }
            else
            {
                int size = value.ToString().Length;
                if (size < Min || size > Max)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
