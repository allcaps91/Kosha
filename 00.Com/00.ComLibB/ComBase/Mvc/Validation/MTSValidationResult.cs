using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Validation
{
    public class MTSValidationResult
    {
        /// <summary>
        /// 유효성 검사 메세지
        /// </summary>
        public string Message { get;}
        /// <summary>
        /// 유효성 검사 프로퍼티 이름
        /// </summary>
        public string PropertyName { get; }
        /// <summary>
        /// 유효성 검사 프로퍼티 값
        /// </summary>
        public object Value { get; }
        /// <summary>
        /// 유효성 검사 
        /// </summary>
        public IMTSValidation validation { get; }

        public MTSValidationResult(IMTSValidation validation, string propertyName, object value)
        {
            this.validation = validation;
            this.Message = validation.Message;
            this.PropertyName = propertyName;
            this.Value = value;
        }
    }
}
