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
    public class HicGroupexamRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicGroupexamRepository()
        {
        }

        public void DeleteAll(string fstrCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_GROUPEXAM       ");
            parameter.AppendSql(" WHERE GROUPCODE =:RID                 ");

            parameter.Add("RID", fstrCode);

            ExecuteNonQuery(parameter);
        }

        public void Insert(HIC_GROUPEXAM code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT KOSMOS_PMPA.HIC_GROUPEXAM (                         ");
            parameter.AppendSql("   GROUPCODE,EXCODE,SUGAGBN,SEQNO,ENTSABUN,ENTTIME )       ");
            parameter.AppendSql(" VALUES (                                                  ");
            parameter.AppendSql("  :GROUPCODE,:EXCODE,:SUGAGBN,:SEQNO,:ENTSABUN,SYSDATE )   ");

            parameter.Add("CODE",    code.GROUPCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("EXCODE",  code.EXCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("SUGAGBN", code.SUGAGBN, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("SEQNO",   code.SEQNO);
            parameter.Add("ENTSABUN", clsType.User.IdNumber);

            ExecuteNonQuery(parameter);
        }

        public void Update(HIC_GROUPEXAM code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_GROUPEXAM        ");
            parameter.AppendSql("   SET EXCODE   =:EXCODE                ");
            parameter.AppendSql("      ,SUGAGBN  =:SUGAGBN               ");
            parameter.AppendSql("      ,SEQNO    =:SEQNO                 ");
            parameter.AppendSql("      ,ENTSABUN =:ENTSABUN              ");
            parameter.AppendSql("      ,ENTTIME  =SYSDATE                ");
            parameter.AppendSql(" WHERE ROWID    =:RID                  ");

            parameter.Add("EXCODE",     code.EXCODE);
            parameter.Add("SUGAGBN",    code.SUGAGBN);
            parameter.Add("SEQNO",      code.SEQNO);
            parameter.Add("ENTSABUN",   clsType.User.IdNumber);
            parameter.Add("RID",        code.RID);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_GROUPEXAM> GetExCodeByGrCDExCodeIN(string cODE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GROUPCODE, EXCODE                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_GROUPEXAM                           ");
            parameter.AppendSql(" WHERE GROUPCODE = :GROUPCODE                              ");
            parameter.AppendSql("   AND ExCode IN ('A136','A142','A154','A211','A213','TX11','TX13','TX14','TX16','TX53','TX54')  ");

            parameter.Add("GROUPCODE", cODE, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_GROUPEXAM>(parameter);
        }

        public List<string> GetExcodeByGrpCode(object argHcGroupCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT EXCODE                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_GROUPEXAM    ");
            parameter.AppendSql(" WHERE 1 = 1                        ");
            parameter.AppendSql("   AND GROUPCODE =:GROUPCODE        ");

            parameter.Add("GROUPCODE", argHcGroupCode);

            return ExecuteReader<string>(parameter);
        }


        public void Delete(HIC_GROUPEXAM code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_GROUPEXAM        ");
            parameter.AppendSql(" WHERE ROWID    =:RID                  ");

            parameter.Add("RID",        code.RID);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_GROUPEXAM> GetExcodeByGroupCode(List<string> argExcode, List<string> argGroupcode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT EXCODE                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_GROUPEXAM           ");
            parameter.AppendSql(" WHERE 1 = 1                               ");
            parameter.AppendSql("   AND GROUPCODE IN (:GROUPCODE)            ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)                  ");
            parameter.AppendSql("   GROUP BY EXCODE                          ");

            parameter.AddInStatement("EXCODE", argExcode, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("GROUPCODE", argGroupcode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_GROUPEXAM>(parameter);
        }
    }
}
