using ComBase.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Validation
{
    /// <summary>
    /// 속성타입이 int, long일 경우 사용합니다
    /// 십진수의 최소값보아\다 커야합니다
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MTSDecimalMinAttribute : Attribute, IMTSValidation
    {
        public int Min { get; set; }
        public string Message { get; set; }
        public int Order { get; set; }

        public MTSDecimalMinAttribute()
        {
            Message = Min + "보다 커야 합니다";
        }


        public bool DoValidation(object value)
        {
            if (value.IsNullOrEmpty())
            {
                return false;
            }
            else
            {
                int number = int.Parse(value.ToString());


                if (number <= Min)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
