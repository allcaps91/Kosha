namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcPanjengScodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcPanjengScodeRepository()
        {
        }

        public List<HIC_SPC_PANJENG_SCODE> GetItembyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.DBun,a.SogenCode,COUNT(*) CNT                             ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_PANJENG a, ADMIN.HIC_SPC_SCODE b  ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                            ");
            parameter.AppendSql("   AND a.Panjeng IN ('3','4','5','6','9','A')                      ");
            parameter.AppendSql("   AND a.Deldate is null                                           ");
            parameter.AppendSql("   AND Rtrim(a.SogenCode)=RTrim(b.Code(+))                         ");
            parameter.AppendSql(" GROUP BY b.DBun,a.SogenCode                                       ");
            parameter.AppendSql(" ORDER BY b.DBun,a.SogenCode                                       ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_SPC_PANJENG_SCODE>(parameter);
        }
    }
}
