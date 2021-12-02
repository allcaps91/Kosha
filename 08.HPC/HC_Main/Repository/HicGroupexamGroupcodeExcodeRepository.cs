namespace HC_Main.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using HC_Main.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicGroupexamGroupcodeExcodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicGroupexamGroupcodeExcodeRepository()
        {
        }

        public List<HIC_GROUPEXAM_GROUPCODE_EXCODE> GetHicListByCode(string argCODE, List<string> lstExList)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT a.GROUPCODE, b.HANG, b.NAME AS GROUPNAME, a.EXCODE, c.HNAME        ");
            parameter.AppendSql("       ,c.AMT1, c.AMT2, c.AMT3, c.AMT4, c.AMT5                             ");
            parameter.AppendSql("       ,c.OLDAMT1, c.OLDAMT2, c.OLDAMT3, c.OLDAMT4, c.OLDAMT5              ");
            parameter.AppendSql("       ,TO_CHAR(c.SUDATE,'YYYY-MM-DD') AS SUDATE, a.SUGAGBN, b.GBSUGA      ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_GROUPEXAM a                               ");
            parameter.AppendSql("      ," + ComNum.DB_PMPA + "HIC_GROUPCODE b                               ");
            parameter.AppendSql("      ," + ComNum.DB_PMPA + "HIC_EXCODE c                                  ");
            parameter.AppendSql(" WHERE a.GROUPCODE =:GROUPCODE                                             ");
            if (!lstExList.IsNullOrEmpty() && lstExList.Count > 0)
            {
                parameter.AppendSql("    AND a.EXCODE NOT IN (:EXCODE)                                      ");
            }
            parameter.AppendSql("  AND a.GROUPCODE = b.CODE(+)                                              ");
            parameter.AppendSql("  AND a.EXCODE = c.CODE(+)                                                 ");
            parameter.AppendSql("ORDER BY a.GROUPCODE, a.EXCODE                                             ");

            parameter.Add("GROUPCODE", argCODE, Oracle.DataAccess.Client.OracleDbType.Char);

            if (!lstExList.IsNullOrEmpty() && lstExList.Count > 0)
            { 
                parameter.AddInStatement("EXCODE", lstExList);
            }


            return ExecuteReader<HIC_GROUPEXAM_GROUPCODE_EXCODE>(parameter);
        }

        public List<HIC_GROUPEXAM_GROUPCODE_EXCODE> GetListByCodeIn(string argCODE, List<string> lstExList)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT a.GROUPCODE, b.HANG, b.NAME AS GROUPNAME, a.EXCODE, c.HNAME        ");
            parameter.AppendSql("       ,c.AMT1, c.AMT2, c.AMT3, c.AMT4, c.AMT5                             ");
            parameter.AppendSql("       ,c.OLDAMT1, c.OLDAMT2, c.OLDAMT3, c.OLDAMT4, c.OLDAMT5              ");
            parameter.AppendSql("       ,TO_CHAR(c.SUDATE,'YYYY-MM-DD') AS SUDATE, a.SUGAGBN, b.GBSUGA      ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_GROUPEXAM a                               ");
            parameter.AppendSql("      ," + ComNum.DB_PMPA + "HIC_GROUPCODE b                               ");
            parameter.AppendSql("      ," + ComNum.DB_PMPA + "HIC_EXCODE c                                  ");
            parameter.AppendSql(" WHERE a.GROUPCODE =:GROUPCODE                                             ");
            if (!lstExList.IsNullOrEmpty() && lstExList.Count > 0)
            {
                parameter.AppendSql("    AND a.EXCODE IN (:EXCODE)                                          ");
            }
            parameter.AppendSql("  AND a.GROUPCODE = b.CODE(+)                                              ");
            parameter.AppendSql("  AND a.EXCODE = c.CODE(+)                                                 ");
            parameter.AppendSql("ORDER BY a.GROUPCODE, a.EXCODE                                             ");

            parameter.Add("GROUPCODE", argCODE, Oracle.DataAccess.Client.OracleDbType.Char);

            if (!lstExList.IsNullOrEmpty() && lstExList.Count > 0)
            {
                parameter.AddInStatement("EXCODE", lstExList);
            }


            return ExecuteReader<HIC_GROUPEXAM_GROUPCODE_EXCODE>(parameter);
        }

        public List<HIC_GROUPEXAM_GROUPCODE_EXCODE> GetListByCodesIN(List<string> lstExList)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT a.GROUPCODE, b.HANG, b.NAME AS GROUPNAME, a.EXCODE, c.HNAME        ");
            parameter.AppendSql("       ,c.AMT1, c.AMT2, c.AMT3, c.AMT4, c.AMT5                             ");
            parameter.AppendSql("       ,c.OLDAMT1, c.OLDAMT2, c.OLDAMT3, c.OLDAMT4, c.OLDAMT5              ");
            parameter.AppendSql("       ,TO_CHAR(c.SUDATE,'YYYY-MM-DD') AS SUDATE, a.SUGAGBN, b.GBSUGA      ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_GROUPEXAM a                               ");
            parameter.AppendSql("      ," + ComNum.DB_PMPA + "HIC_GROUPCODE b                               ");
            parameter.AppendSql("      ," + ComNum.DB_PMPA + "HIC_EXCODE c                                  ");
            parameter.AppendSql(" WHERE a.GROUPCODE IN (:GROUPCODE)                                         ");
            parameter.AppendSql("  AND a.GROUPCODE = b.CODE(+)                                              ");
            parameter.AppendSql("  AND a.EXCODE = c.CODE(+)                                                 ");
            parameter.AppendSql("ORDER BY a.GROUPCODE, a.EXCODE                                             ");

            parameter.AddInStatement("GROUPCODE", lstExList, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_GROUPEXAM_GROUPCODE_EXCODE>(parameter);
        }

        public List<HIC_GROUPEXAM_GROUPCODE_EXCODE> GetHeaListByCode(string argCODE, List<string> lstExList)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT a.GROUPCODE, '' AS GROUPNAME, a.EXCODE, c.HNAME                ");
            parameter.AppendSql("       ,c.AMT1, c.AMT2, c.AMT3, c.AMT4, c.AMT5                             ");
            parameter.AppendSql("       ,c.OLDAMT1, c.OLDAMT2, c.OLDAMT3, c.OLDAMT4, c.OLDAMT5              ");
            parameter.AppendSql("       ,TO_CHAR(c.SUDATE,'YYYY-MM-DD') SUDATE, a.SUGAGBN                   ");
            parameter.AppendSql("      , KOSMOS_PMPA.FC_HEA_CODE_NM('13', a.EXCODE) AS ETCEXAM              ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HEA_GROUPEXAM a                               ");
            parameter.AppendSql("      ," + ComNum.DB_PMPA + "HIC_EXCODE c                                  ");
            parameter.AppendSql(" WHERE a.GROUPCODE =:GROUPCODE                                             ");
            if (!lstExList.IsNullOrEmpty() && lstExList.Count > 0)
            {
                parameter.AppendSql("    AND a.EXCODE NOT IN (:EXCODE)                                      ");
            }
            parameter.AppendSql("   AND a.EXCODE = c.CODE(+)                                                ");
            parameter.AppendSql("ORDER BY a.GROUPCODE, a.EXCODE                                             ");

            parameter.Add("GROUPCODE", argCODE, Oracle.DataAccess.Client.OracleDbType.Char);

            if (!lstExList.IsNullOrEmpty() && lstExList.Count > 0)
            {
                parameter.AddInStatement("EXCODE", lstExList);
            }

            return ExecuteReader<HIC_GROUPEXAM_GROUPCODE_EXCODE>(parameter);
        }
    }
}
