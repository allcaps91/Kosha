using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComBase.Controls;
namespace ComBase.Mvc.Utils
{
    public static class NumberUtil
    {
        /// <summary>
        /// 숫자에 콤마 넣기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Comma(this object value)
        {
            if (value.Empty())
            {
                return "";
            }
            else
            {
                return String.Format("{0:#,###}", value);
            }
            
        }
    }
}
