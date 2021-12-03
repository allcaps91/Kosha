namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaDayRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaDayRepository()
        {
        }

        public string GetRemarkByDate(string argCurMonth)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT REMARK                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_DAY                      ");
            parameter.AppendSql(" WHERE HOO_DATE = TO_DATE(:HDATE, 'YYYY-MM-DD') ");

            parameter.Add("HDATE", argCurMonth);

            return ExecuteScalar<string>(parameter);
        }
    }
}
