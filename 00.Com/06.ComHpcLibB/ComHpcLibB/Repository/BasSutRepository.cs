namespace ComHpcLibB.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class BasSutRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public BasSutRepository()
        {
        }

        public BAS_SUT GetItembySuCode(string strSuCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT BAMT  ");
            parameter.AppendSql("  FROM ADMIN.BAS_SUT                     ");
            parameter.AppendSql(" WHERE SUCODE = :SUCODE                        ");

            parameter.Add("SUCODE", strSuCode);

            return ExecuteReaderSingle<BAS_SUT>(parameter);
        }
    }
}
