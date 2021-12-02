using ComBase.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComBase.Mvc.Validation
{
    /// <summary>
    /// 속성타입이 string일때 사용합니다
    /// 이메일 형식 검사
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MTSEmailAttribute : Attribute, IMTSValidation
    {
        public string Message { get; set; }
        public int Order { get; set; }
        public MTSEmailAttribute()
        {
            Message = "Email 형식이 아닙니다";
        }
        public bool DoValidation(object value)
        {
            if (value.IsNullOrEmpty())
            {
                return false;
            }
           return  Regex.IsMatch(value.ToString(), @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
           
        }
    }
}
