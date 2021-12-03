namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class ViewSugaCodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public ViewSugaCodeRepository()
        {
        }

        public long GetBAmtByLikeSucode(string argSucode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT BAMT FROM ADMIN.VIEW_SUGA_CODE            ");
            parameter.AppendSql(" WHERE SUCODE LIKE :SUCODE                             ");
            parameter.AppendSql(" ORDER BY SUCODE                                       ");

            parameter.AddLikeStatement("SUCODE", argSucode);
           
            return ExecuteScalar<long>(parameter);

        }
    }
}
