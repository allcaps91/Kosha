using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Exceptions
{
    /// <summary>
    /// 데이타베이스 작업과 관련된 예외를 처리합니다.
    /// </summary>
    /// <modified>Mentorsoft : donghoonkim 2018-12-27,10:06</modified>
    public class MTSDatabaseException : MentorsoftException
    {
        public MTSDatabaseException(string message) : base(message) { }
        public MTSDatabaseException(ErrorCode errorCode, System.Exception innerException) : base(errorCode, innerException) { }

        public MTSDatabaseException(string message, System.Exception innerException, string sql) : base(message, innerException)
        {
            this.Sql = sql;
        }

    }
}
