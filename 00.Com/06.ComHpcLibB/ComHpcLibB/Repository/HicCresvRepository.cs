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
    public class HicCresvRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicCresvRepository()
        {
        }

        public List<HIC_CRESV> GetEnvironment(string dtpFDate, string strNextDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT GUBUN,LTDCODE,TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,Man ");

            parameter.AppendSql("   FROM   KOSMOS_PMPA.HIC_CRESV");

            parameter.AppendSql("   WHERE RDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND RDATE <= TO_DATE(:STRNEXTDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND LTDCODE IS NOT NULL  ");
            parameter.AppendSql("   ORDER BY RDATE  ");


            parameter.Add("FDATE", dtpFDate);
            parameter.Add("STRNEXTDATE", strNextDate);

            return ExecuteReader<HIC_CRESV>(parameter);
        }
    }
}
