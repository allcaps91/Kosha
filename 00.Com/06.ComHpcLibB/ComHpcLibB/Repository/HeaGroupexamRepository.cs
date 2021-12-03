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
    public class HeaGroupexamRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaGroupexamRepository()
        {
        }

        public void DeleteAll(string fstrCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HEA_GROUPEXAM       ");
            parameter.AppendSql(" WHERE GROUPCODE =:RID                 ");

            parameter.Add("RID", fstrCode);

            ExecuteNonQuery(parameter);
        }

        public void Insert(HEA_GROUPEXAM code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT KOSMOS_PMPA.HEA_GROUPEXAM (                 ");
            parameter.AppendSql("   GROUPCODE,EXCODE,SEQNO,ENTSABUN,ENTDATE )       ");
            parameter.AppendSql(" VALUES (                                          ");
            parameter.AppendSql("  :GROUPCODE,:EXCODE,:SEQNO,:ENTSABUN,SYSDATE )    ");

            parameter.Add("GROUPCODE",  code.GROUPCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EXCODE",     code.EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SEQNO",      code.SEQNO);
            parameter.Add("ENTSABUN", clsType.User.IdNumber);

            ExecuteNonQuery(parameter);
        }

        public void Update(HEA_GROUPEXAM code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_GROUPEXAM        ");
            parameter.AppendSql("   SET EXCODE   =:EXCODE                ");
            parameter.AppendSql("      ,SEQNO    =:SEQNO                 ");
            parameter.AppendSql("      ,ENTSABUN =:ENTSABUN              ");
            parameter.AppendSql("      ,ENTDATE  =SYSDATE                ");
            parameter.AppendSql(" WHERE ROWID    = :RID                  ");

            parameter.Add("EXCODE", code.EXCODE);
            parameter.Add("SEQNO",  code.SEQNO);
            parameter.Add("ENTSABUN", clsType.User.IdNumber);
            parameter.Add("RID", code.RID);

            ExecuteNonQuery(parameter);
        }

        public string ExistByGrpCodeExCode(string strGrpCode, string strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID AS RID                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPEXAM     ");
            parameter.AppendSql(" WHERE 1 = 1                         ");
            parameter.AppendSql("   AND GROUPCODE =:GROUPCODE         ");
            parameter.AppendSql("   AND EXCODE =:EXCODE               ");

            parameter.Add("GROUPCODE", strGrpCode);
            parameter.Add("EXCODE", strExCode);

            return ExecuteScalar<string>(parameter);
        }

        public void Delete(HEA_GROUPEXAM code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HEA_GROUPEXAM        ");
            parameter.AppendSql(" WHERE ROWID    = :RID                  ");

            parameter.Add("RID", code.RID);

            ExecuteNonQuery(parameter);
        }
    }
}
