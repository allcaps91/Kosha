using ComBase.Mvc.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Exceptions
{
    /// <summary>
    /// DTO 유효성검사 예외
    /// ValidationResult 프로퍼티에 유효성 검사 결과 정보가 있쓰니다
    /// </summary>
    public class MTSValidationException : MentorsoftException
    { 
        /// <summary>
        /// 유효성 검사결과
        /// </summary>
        public List<MTSValidationResult> ValidationResult { get; }
        public MTSValidationException(string message) : base(message) { }
        public MTSValidationException(string message, Exception innerException) : base(message, innerException) { }
        public MTSValidationException(List<MTSValidationResult> list)
        {
            this.ValidationResult = list;
        }
    }
}
