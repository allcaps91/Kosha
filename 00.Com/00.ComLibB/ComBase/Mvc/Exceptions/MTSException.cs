using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Exceptions
{
    /// <summary>
    /// 기본 예외처리
    /// </summary>
    public class MTSException : MentorsoftException
    {
        public MTSException(string message) : base(message) { }
        public MTSException(string message, Exception innerException) : base(message, innerException) { }
    }
}
