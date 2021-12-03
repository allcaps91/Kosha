using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Exceptions
{
    /// <summary>
    /// 오류코드
    /// 사용자에게 정확한 오류 정보제공을 목적으로 함 
    /// </summary>
    public enum ErrorCode
    {
        UnknowError,
        UserOperationError,
        /// <summary>
        /// 네트워크 오류
        /// </summary>
        NetworkError,
        DatabaseError,
        DatabasePKError
    }


    /// <summary>
    /// 모든 사용자 예외 클래스는 본 클래스를 상속받아야 한다
    /// </summary>
    ///<created>Mentorsoft : donghoonkim 2018-12-27,10:06</created>
    public abstract class MentorsoftException : System.Exception
    {
        /// <summary>
        /// 실행되어진 쿼리 문자열입니다
        /// </summary>
        public string Sql { get; set; }

        public ErrorCode ErrorCode { get; set; }

        public MentorsoftException() : base() { }
        public MentorsoftException(ErrorCode errorCode, Exception innerException) : base(innerException.Message, innerException)
        {
            /// Log.Error("innerException: {}", innerException.ToString());
        }
        public MentorsoftException(string message) : base(message) { }
        public MentorsoftException(string message, Exception innerException) : base(message, innerException)
        {
            /// Log.Error("innerException: {}",innerException.ToString());
        }
    }
}
