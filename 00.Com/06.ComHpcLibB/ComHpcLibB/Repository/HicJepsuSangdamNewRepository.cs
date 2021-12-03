namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuSangdamNewRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuSangdamNewRepository()
        {
        }

        public List<HIC_JEPSU_SANGDAM_NEW> GetItembyPaNoJepDate(long argPaNo, string argJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,a.GJJONG,a.UCODES,a.GjChasu,b.GBSTS                         ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU       a                                       ");
            parameter.AppendSql("     , ADMIN.HIC_SANGDAM_NEW b                                       ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                                      ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO                                                   ");
            parameter.AppendSql("   AND a.JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                   ");
            parameter.AppendSql(" ORDER BY GBSTS DESC                                                       ");

            parameter.Add("PANO", argPaNo);
            parameter.Add("JEPDATE", argJepDate);

            return ExecuteReader<HIC_JEPSU_SANGDAM_NEW>(parameter);
        }
    }
}
