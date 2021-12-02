namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HeaGroupexamExcodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaGroupexamExcodeRepository()
        {
        }

        public List<HEA_GROUPEXAM_EXCODE> GetItemsByCode(string strCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.SEQNO,a.EXCODE,b.HNAME,a.SUGAGBN,a.ROWID AS RID,SUDATE        ");
            //parameter.AppendSql("      ,b.GAMT1,b.JAMT1,b.SAMT1,b.IAMT1,b.GAMT2,b.JAMT2,b.SAMT2,b.IAMT2 ");
            parameter.AppendSql("      ,b.AMT1 AS GAMT1 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPEXAM a                                     ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_EXCODE b                                        ");
            parameter.AppendSql(" WHERE A.GROUPCODE =:GROUPCODE                                         ");
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)                                            ");
            parameter.AppendSql(" ORDER BY A.SEQNO,A.EXCODE                                             ");

            parameter.Add("GROUPCODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_GROUPEXAM_EXCODE>(parameter);
        }

        public List<HEA_GROUPEXAM_EXCODE> GetGbEndoCodeByCodeIN(List<string> lstgrpCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.EXCODE, b.ENDOGUBUN1, b.ENDOGUBUN2, b.ENDOGUBUN3, b.ENDOGUBUN4, b.ENDOGUBUN5    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPEXAM a                                             ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_EXCODE b                                                ");
            parameter.AppendSql(" WHERE a.GROUPCODE IN (:GROUPCODE)                                             ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                    ");
            parameter.AppendSql(" GROUP BY a.EXCODE, b.ENDOGUBUN1, b.ENDOGUBUN2, b.ENDOGUBUN3, b.ENDOGUBUN4, b.ENDOGUBUN5 ");
            parameter.AppendSql(" ORDER BY a.EXCODE ");

            parameter.AddInStatement("GROUPCODE", lstgrpCode);

            return ExecuteReader<HEA_GROUPEXAM_EXCODE>(parameter);
        }

        public string GetRowidByGroupCode(string argGrpCD)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.ROWID AS RID                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPEXAM a         ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_EXCODE b            ");
            parameter.AppendSql(" WHERE a.GROUPCODE =:GROUPCODE             ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE                   ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                   ");
            parameter.AppendSql("   AND b.EXCODE IS NOT NULL                ");

            parameter.Add("GROUPCODE", argGrpCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HEA_GROUPEXAM_EXCODE> GetItemsByCodeIN(List<string> lstCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.EXCODE, b.HNAME               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPEXAM a     ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_EXCODE b        ");
            parameter.AppendSql(" WHERE a.GROUPCODE IN (:GROUPCODE)     ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)            ");
            parameter.AppendSql(" GROUP BY a.EXCODE, b.HNAME            ");
            parameter.AppendSql(" ORDER BY a.EXCODE                     ");

            parameter.AddInStatement("GROUPCODE", lstCode);

            return ExecuteReader<HEA_GROUPEXAM_EXCODE>(parameter);
        }
    }
}
