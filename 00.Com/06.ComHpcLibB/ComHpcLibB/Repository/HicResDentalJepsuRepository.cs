namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicResDentalJepsuRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicResDentalJepsuRepository()
        {
        }

        public HIC_RES_DENTAL_JEPSU GetCountbySysDate(long nLicense)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(DECODE(a.PanJengDRNO, :LICENSE, 1)) CNT                   ");
            parameter.AppendSql("     , COUNT(DECODE(a.PanJengDRNO,'', 1)) CNT2                         ");
            parameter.AppendSql("  FROM ADMIN.HIC_RES_DENTAL a, ADMIN.HIC_JEPSU b           ");
            parameter.AppendSql(" WHERE b.WRTNO = a.WRTNO                                               ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                               ");
            parameter.AppendSql("   AND b.GjJong NOT IN ('31','35','56')                                "); //암검진 제외
            parameter.AppendSql("   AND b.JEPDATE = TRUNC(SYSDATE)                                      "); //판정완료

            parameter.Add("LICENSE", nLicense);

            return ExecuteReaderSingle<HIC_RES_DENTAL_JEPSU>(parameter);
        }
    }
}
