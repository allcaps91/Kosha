namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class BasMailnewRepository : BaseRepository
    {        
        /// <summary>
        /// 
        /// </summary>
        public BasMailnewRepository()
        {
        }

        public string Read_MailName(string MAILCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAILJUSO                    ");
            parameter.AppendSql("  FROM ADMIN.BAS_MAILNEW     ");
            parameter.AppendSql(" WHERE MAILCODE = :MAILCODE        ");

            parameter.Add("MAILCODE", MAILCODE);

            return ExecuteScalar<string>(parameter);
        }

        public string GetMailJiyekbyMailCode(string strZipCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAILJIYEK                   ");
            parameter.AppendSql("  FROM ADMIN.BAS_MAILNEW     ");
            parameter.AppendSql(" WHERE MAILCODE = :MAILCODE        ");

            parameter.Add("MAILCODE", strZipCode);

            return ExecuteScalar<string>(parameter);
        }
    }
}
