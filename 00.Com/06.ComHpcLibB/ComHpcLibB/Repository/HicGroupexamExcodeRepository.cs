namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicGroupexamExcodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicGroupexamExcodeRepository()
        {
        }

        public List<HIC_GROUPEXAM_EXCODE> GetEndoGubunbyGroupCode(string strCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.ENDOSCOPE, b.ENDOGUBUN1, b.ENDOGUBUN2, b.ENDOGUBUN3, b.ENDOGUBUN4, b.ENDOGUBUN5       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_GROUPEXAM a                                                             ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE    b                                                             ");
            parameter.AppendSql(" WHERE a.GROUPCODE = :CODE                                                                     ");
            parameter.AppendSql("   AND (b.ENDOGUBUN2 = 'Y' OR b.ENDOGUBUN3 = 'Y' OR b.ENDOGUBUN4 = 'Y' OR b.ENDOGUBUN5 = 'Y')  ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                                    ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                                                       ");

            parameter.Add("CODE", strCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_GROUPEXAM_EXCODE>(parameter);
        }

        public List<HIC_GROUPEXAM_EXCODE> GetItembyGroupCode(string strCode, List<string> strExamList)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.GroupCode,b.Hang,b.Name GroupName,a.ExCode,c.HName,c.Amt1,c.Amt2                      ");
            parameter.AppendSql("     , c.Amt3,c.Amt4,c.Amt5,TO_CHAR(c.SuDate,'YYYY-MM-DD') SuDate                              ");
            parameter.AppendSql("     , c.OldAmt1,c.OldAmt2,c.OldAmt3,c.OldAmt4,c.OldAmt5                                       ");
            parameter.AppendSql("     , a.SugaGbn,b.GbSuga                                                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_GROUPEXAM a, KOSMOS_PMPA.HIC_GROUPCODE b, KOSMOS_PMPA.HIC_EXCODE c      ");
            parameter.AppendSql(" WHERE a.GROUPCODE = :GROUPCODE                                                                ");
            if (!strExamList.IsNullOrEmpty() && strExamList.Count > 0)
            {
                parameter.AppendSql("   AND a.EXCODE NOT IN (:EXCODE)                                                           ");
            }
            parameter.AppendSql("   AND a.GroupCode = b.Code(+)                                                                 ");
            parameter.AppendSql("   AND a.ExCode=c.Code(+)                                                                      ");
            parameter.AppendSql(" ORDER BY a.GroupCode,a.ExCode                                                                 ");

            parameter.Add("GROUPCODE", strCode, Oracle.DataAccess.Client.OracleDbType.Char);
            if (!strExamList.IsNullOrEmpty() && strExamList.Count > 0)
            {
                parameter.AddInStatement("EXCODE", strExamList);
            }

            return ExecuteReader<HIC_GROUPEXAM_EXCODE>(parameter);
        }

        public List<HIC_GROUPEXAM_EXCODE> GetItemsByCode(string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.SEQNO,a.EXCODE,b.HNAME,a.SUGAGBN,a.ROWID AS RID,SUDATE        ");
            //parameter.AppendSql("      ,b.GAMT1,b.JAMT1,b.SAMT1,b.IAMT1,b.GAMT2,b.JAMT2,b.SAMT2,b.IAMT2 ");
            parameter.AppendSql("      ,b.AMT1 AS GAMT1 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_GROUPEXAM a                                     ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_EXCODE b                                        ");
            parameter.AppendSql(" WHERE A.GROUPCODE =:GROUPCODE                                         ");
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)                                            ");
            parameter.AppendSql(" ORDER BY A.SEQNO,A.EXCODE                                             ");

            parameter.Add("GROUPCODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_GROUPEXAM_EXCODE>(parameter);
        }

    }
}
