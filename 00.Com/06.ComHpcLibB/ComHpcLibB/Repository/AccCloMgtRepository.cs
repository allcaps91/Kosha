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
    public class AccCloMgtRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public AccCloMgtRepository()
        {
        }

        public List<ACC_CLO_MGT> GetMagamDay(string strCLOYMD)
        {
             MParameter parameter = CreateParameter();

             parameter.AppendSql("SELECT     MM_CLO_YN FROM KOSMOS_ERP.ACC_CLO_MGT   ");

             parameter.AppendSql("WHERE      CLO_BSNS_GB = '0003'                    ");
            parameter.AppendSql("   AND     CLO_YMD = :COLYMD                       ");

            parameter.Add("COLYMD", strCLOYMD, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2);

             return ExecuteReader<ACC_CLO_MGT>(parameter);
        }
    }
}
