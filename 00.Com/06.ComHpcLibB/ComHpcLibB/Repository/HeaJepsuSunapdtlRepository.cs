namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;

    public class HeaJepsuSunapdtlRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuSunapdtlRepository()
        {

        }

        public List<HEA_JEPSU_SUNAPDTL> GetGamInfoByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.BURATE,a.GAMCODE,a.GAMCODE2,b.CODE,b.AMT,b.GBSELF ");
            parameter.AppendSql("      ,b.GBHALIN,a.GAMAMT,a.LTDSAMT, b.BONINAMT, B.LTDAMT  ");
            parameter.AppendSql("  FROM ADMIN.HEA_JEPSU a                             ");
            parameter.AppendSql("     , ADMIN.HEA_SUNAPDTL b                          ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                    ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                   ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO                                   ");
            
            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HEA_JEPSU_SUNAPDTL>(parameter);
        }
    }
}