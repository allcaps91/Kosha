namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicMisuGiroRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicMisuGiroRepository()
        {
        }

        public List<HIC_MISU_GIRO> getGiro(long nGiroNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT                  MISUNO                  ");

            parameter.AppendSql("   FROM                    ADMIN.MISU_GIRO   ");

            parameter.AppendSql("   WHERE                   WRTNO = :WRTNO          ");
            parameter.AppendSql("       AND                 DELDATE IS NULL         ");
            parameter.AppendSql("       AND                 GBBUSE='1'              ");

            parameter.Add("WRTNO", nGiroNo);
            
            return ExecuteReader<HIC_MISU_GIRO>(parameter);
        }
    }
}
