namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class OcsSubcodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public OcsSubcodeRepository()
        {
        }

        public List<OCS_SUBCODE> FindSubCode(string strOrderCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SubName,ItemCd,SuCode               ");
            parameter.AppendSql("  FROM " + ComNum.DB_MED + "OCS_SUBCODE    ");
            parameter.AppendSql(" WHERE 1 = 1                               ");
            parameter.AppendSql("   AND OrderCode = :OrderCode              ");
            parameter.AppendSql(" ORDER BY SeqNo,SubName                    ");

            parameter.Add("OrderCode", strOrderCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<OCS_SUBCODE>(parameter);
        }
    }
}
