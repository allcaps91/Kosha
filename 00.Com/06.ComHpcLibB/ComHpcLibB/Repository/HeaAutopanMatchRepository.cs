namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaAutopanMatchRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaAutopanMatchRepository()
        {
        }

        public int Insert(string strWrtNo, string strMCode, string strExcode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.HEA_AUTOPAN_MATCH (   ");
            parameter.AppendSql("        WRTNO, MCODE, EXCODE)                  ");
            parameter.AppendSql(" VALUES (                                      ");
            parameter.AppendSql("        :WRTNO, :MCODE, :EXCODE)               ");

            parameter.Add("WRTNO", strWrtNo);
            parameter.Add("MCODE", strMCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EXCODE", strExcode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int Update(string strExCode, string strRowId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HEA_AUTOPAN_MATCH SET  ");
            parameter.AppendSql("        EXCODE = :EXCODE                   ");
            parameter.AppendSql("  WHERE ROWID  = :RID                      ");

            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RID", strRowId);

            return ExecuteNonQuery(parameter);
        }

        public string GetRowIdbyWrtNo(string strWrtNo, string strMCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_AUTOPAN_MATCH       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");
            parameter.AppendSql("   AND MCODE = :MCODE                      ");

            parameter.Add("WRTNO", strWrtNo);
            parameter.Add("MCODE", strMCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }
    }
}
