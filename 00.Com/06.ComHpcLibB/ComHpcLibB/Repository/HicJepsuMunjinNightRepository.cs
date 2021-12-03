namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuMunjinNightRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuMunjinNightRepository()
        {
        }

        public List<HIC_JEPSU_MUNJIN_NIGHT> GetItembyPaNoGjYear(long nPano, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE,A.WRTNO,A.GJYEAR,A.GJCHASU  ");
            parameter.AppendSql("     , b.ITEM1_DATA, b.ITEM1_JEMSU, b.ITEM1_PANJENG                        ");
            parameter.AppendSql("     , b.ITEM2_DATA, b.ITEM2_JEMSU, b.ITEM2_PANJENG                        ");
            parameter.AppendSql("     , b.ITEM3_DATA, b.ITEM3_JEMSU, b.ITEM3_PANJENG                        ");
            parameter.AppendSql("     , b.ITEM4_DATA, b.ITEM4_JEMSU, b.ITEM4_PANJENG                        ");
            parameter.AppendSql("     , b.ITEM5_DATA, b.ITEM5_JEMSU, b.ITEM5_PANJENG                        ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a                                             ");
            parameter.AppendSql("     , ADMIN.HIC_MUNJIN_NIGHT b                                      ");
            parameter.AppendSql(" WHERE a.PANO   = :PANO                                                    ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                                  ");
            parameter.AppendSql("   AND a.WRTNO  = b.WRTNO(+)                                               ");
            parameter.AppendSql("   AND b.ENTDATE IS NOT NULL                                               ");
            parameter.AppendSql(" ORDER BY ENTDATE DESC                                                     ");

            parameter.Add("PANO", nPano);
            parameter.Add("GJYEAR", strGjYear);

            return ExecuteReader<HIC_JEPSU_MUNJIN_NIGHT>(parameter);
        }
    }
}
