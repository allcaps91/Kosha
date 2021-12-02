using ComBase.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Validation
{
    /// <summary>
    /// 속성타입이 숫자형일경우 사용합니다
    /// 0일 경우 유효성 검사 실패합니다
    /// </summary>
    public class MTSZeroAttribute : Attribute, IMTSValidation
    {
        public string Message { get; set; }
        public int Order { get; set; }

        public MTSZeroAttribute()
        {
            Message = "값이 0이 아니어야 합니다 ";
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

                if (number == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
