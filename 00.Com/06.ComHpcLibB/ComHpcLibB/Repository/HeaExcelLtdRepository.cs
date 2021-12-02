namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HeaExcelLtdRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaExcelLtdRepository()
        {
        }

        public List<HEA_EXCEL_LTD> GetLtdNamebyYear(string strYear, string strChk, string strName = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.LTDCODE, b.NAME, COUNT(*) CNT ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXCEL a         ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_LTD   b         ");
            parameter.AppendSql(" WHERE a.YEAR = :YEAR                  ");
            if (strChk == "1")
            {
                parameter.AppendSql("   AND a.RDate IS NULL             ");
            }
            if (strName != "")
            {
                parameter.AppendSql("   AND b.Name LIKE :NAME           ");
            }
            parameter.AppendSql("   AND a.LTDCODE = b.CODE(+)           ");
            parameter.AppendSql(" GROUP BY b.NAME, a.LTDCODE            ");
            parameter.AppendSql(" ORDER BY b.NAME, a.LTDCODE            ");

            parameter.Add("YEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (strName != "")
            {
                parameter.AddLikeStatement("NAME", strName);
            }

            return ExecuteReader<HEA_EXCEL_LTD>(parameter);
        }

        public List<HEA_EXCEL_LTD> GetItemAllbyYear(string text, string strChk)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.LTDCODE CODE,b.Name NAME, COUNT(*) CNT    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXCEL a                     ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_LTD   b                     ");
            parameter.AppendSql(" WHERE a.YEAR = :YEAR                              ");
            if (strChk == "OK")
            {
                parameter.AppendSql("   AND a.RDate IS NULL                         ");
            }
            parameter.AppendSql("   AND a.LtdCode = b.Code(+)                       ");
            parameter.AppendSql(" GROUP BY b.Name, a.LtdCode                        ");
            parameter.AppendSql(" ORDER BY b.Name, a.LtdCode                        ");

            parameter.Add("YEAR", text, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_EXCEL_LTD>(parameter);
        }
    }
}
