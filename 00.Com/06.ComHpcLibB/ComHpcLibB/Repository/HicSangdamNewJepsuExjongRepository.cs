namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicSangdamNewJepsuExjongRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSangdamNewJepsuExjongRepository()
        {
        }

        public HIC_SANGDAM_NEW_JEPSU_EXJONG GetCntCnt2(long nLicense)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(DECODE(a.SANGDAMDRNO, :LICENSE, 1)) CNT       ");
            parameter.AppendSql("     , COUNT(DECODE(a.SANGDAMDRNO,'', 1)) CNT2             ");
            parameter.AppendSql("  FROM ADMIN.HIC_SANGDAM_NEW a                       ");
            parameter.AppendSql("     , ADMIN.HIC_JEPSU       b                       ");
            parameter.AppendSql("     , ADMIN.HIC_EXJONG      c                       ");
            parameter.AppendSql(" WHERE b.WRTNO = a.WRTNO(+)                                ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                   ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                   ");
            parameter.AppendSql("   AND a.GjJong = c.Code(+)                                ");
            parameter.AppendSql("   AND c.GbSangDam = 'Y'                                   ");
            parameter.AppendSql("   AND b.JEPDATE = TRUNC(SYSDATE)                          ");

            parameter.Add("LICENSE", nLicense);

            return ExecuteReaderSingle<HIC_SANGDAM_NEW_JEPSU_EXJONG>(parameter);
        }
    }
}
