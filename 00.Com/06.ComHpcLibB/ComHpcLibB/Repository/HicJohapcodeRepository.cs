namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicJohapcodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJohapcodeRepository()
        {
        }
        
        public string Read_Johap_Name(string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Name                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JOHAPCODE   ");
            parameter.AppendSql(" WHERE CODE = :CODE                ");

            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }
    }
}
