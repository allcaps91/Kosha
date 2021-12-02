using ComBase.Controls;
using ComBase.Mvc.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Validation
{

    /// <summary>
    /// 속성타입이 문자 또는 객체에 사용합니다
    /// Null or "" 인지를 검사합니다
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MTSNotEmptyAttribute : Attribute, IMTSValidation
    {
        public string Message { get; set; }
        public int Order { get; set; }
        public MTSNotEmptyAttribute()
        {
            Message = "비워 둘 수 없습니다";
        }
        public bool DoValidation(object value)
        {

            if (value.IsNullOrEmpty())
            {
                return false;
            }
            else if(value is int || value is long)
            {
                throw new MTSException("속성타입이 int이거나 long일경우 사용할 수 없습니다");
            }
            return true;
        }
    }
}
