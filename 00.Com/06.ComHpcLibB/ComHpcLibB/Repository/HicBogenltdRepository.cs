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
    public class HicBogenltdRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicBogenltdRepository()
        {
        }

        public List<HIC_BOGENLTD> GetDaeHang(long LTDCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT         LTDCODE, INWON, PRICE, BILL    ");
            parameter.AppendSql("  FROM         ADMIN.HIC_BOGENLTD       ");
            parameter.AppendSql("  WHERE        LTDCODE = :LTDCODE             ");

            parameter.Add("LTDCODE", LTDCODE);

            return ExecuteReader<HIC_BOGENLTD>(parameter);
        }


        //public List<HEA_EXCEL_LTD> GetItemAllbyYear(string text, string strChk)
        //{
        //    MParameter parameter = CreateParameter();
        //    parameter.AppendSql("SELECT a.LTDCODE CODE,b.Name NAME, COUNT(*) CNT    ");
        //    parameter.AppendSql("  FROM ADMIN.HEA_EXCEL a                     ");
        //    parameter.AppendSql("     , ADMIN.HIC_LTD   b                     ");
        //    parameter.AppendSql(" WHERE a.YEAR = :YEAR                              ");
        //    if (strChk == "OK")
        //    {
        //        parameter.AppendSql("   AND a.RDate IS NULL                         ");
        //    }
        //    parameter.AppendSql("   AND a.LtdCode = b.Code(+)                       ");
        //    parameter.AppendSql(" GROUP BY b.Name, a.LtdCode                        ");
        //    parameter.AppendSql(" ORDER BY b.Name, a.LtdCode                        ");

        //    parameter.Add("YEAR", text, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

        //    return ExecuteReader<HEA_EXCEL_LTD>(parameter);
        //}
    }
}
