using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Validation
{
    public interface IMTSValidation
    {
        /// <summary>
        /// 유효성 검사 메세지
        /// </summary>
        string Message { get; set; }
        /// <summary>
        /// 유효성 검사 순서
        /// </summary>
        int Order { get; set; }

        /// <summary>
        /// 유효성 검사 실행
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool DoValidation(object value);

    }
}
