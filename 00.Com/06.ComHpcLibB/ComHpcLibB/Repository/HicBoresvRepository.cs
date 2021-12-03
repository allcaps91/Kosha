namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicBoresvRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicBoresvRepository()
        {
        }

        public List<HIC_BORESV> GetHealth(string dtpFDate, string strNextDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT LTDCODE,TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,Man ");

            parameter.AppendSql("   FROM   KOSMOS_PMPA.HIC_BORESV");

            parameter.AppendSql("   WHERE RDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND RDATE <= TO_DATE(:STRNEXTDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND LTDCODE IS NOT NULL  ");
            parameter.AppendSql("   ORDER BY RDATE  ");


            parameter.Add("FDATE", dtpFDate);
            parameter.Add("STRNEXTDATE", strNextDate);

            return ExecuteReader<HIC_BORESV>(parameter);
        }
    }
}
