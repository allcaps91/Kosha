namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class AccTaxRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public AccTaxRepository()
        {
        }

        public List<ACC_TAX> GetTaxDate()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT         TO_CHAR(TAXDATE,'YYYY-MM-DD') TAXDATE FROM ADMIN.ACC_TAX  ");

            parameter.AppendSql("WHERE          GUBUN = '4'                                                     ");

            return ExecuteReader<ACC_TAX>(parameter);
        }
    }
}
