namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicChukmstRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicChukmstRepository()
        {
        }

        public List<HIC_CHUKMST> GetSDatebySDateLtdCode(string strFrDate, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE           ");
            parameter.AppendSql("  FROM ADMIN.HIC_CHUKMST                     ");
            parameter.AppendSql("  WHERE SDate >= TO_DATE(:SDATE, 'YYYY-MM-DD')     ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                          ");
            parameter.AppendSql(" ORDER BY SDate DESC                               ");

            parameter.Add("SDATE", strFrDate);
            parameter.Add("LTDCODE", nLtdCode);

            return ExecuteReader<HIC_CHUKMST>(parameter);
        }
    }
}
