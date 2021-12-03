namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicAmtCancerRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicAmtCancerRepository()
        {
        }

        public IList<HIC_AMT_CANCER> FindAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SDATE, ROWID AS RID         ");
            parameter.AppendSql("  FROM ADMIN.HIC_AMT_CANCER   ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql(" ORDER BY SDATE DESC               ");

            return ExecuteReader<HIC_AMT_CANCER>(parameter);
        }

        public IList<HIC_AMT_CANCER> GetListBySDate(string strDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SDATE,AMT01,AMT02, AMT03, AMT04, AMT05, AMT06, AMT07, AMT08, AMT09  ");
            parameter.AppendSql("      ,AMT10,AMT11,AMT12, AMT13, AMT14, AMT15, AMT16, AMT17, AMT18, AMT19  ");
            parameter.AppendSql("      ,AMT20,AMT21,AMT22, AMT23, AMT24, AMT25 ,AMT26, AMT27, AMT28, AMT29  ");
            parameter.AppendSql("      ,ROWID AS RID                                                        ");
            parameter.AppendSql("  FROM ADMIN.HIC_AMT_CANCER                                          ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql(" ORDER BY SDATE DESC                                                       ");

            return ExecuteReader<HIC_AMT_CANCER>(parameter);
        }

        public List<HIC_AMT_CANCER> GetItembySDate(string strFDate, string strSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SDATE,AMT01,AMT02, AMT03, AMT04, AMT05, AMT06, AMT07, AMT08, AMT09  ");
            parameter.AppendSql("      ,AMT10,AMT11,AMT12, AMT13, AMT14, AMT15, AMT16, AMT17, AMT18, AMT19  ");
            parameter.AppendSql("      ,AMT20,AMT21,AMT22, AMT23, AMT24, AMT25 ,AMT26, AMT27, AMT28, AMT29  ");
            parameter.AppendSql("      ,ROWID AS RID                                                        ");
            parameter.AppendSql("  FROM ADMIN.HIC_AMT_CANCER                                          ");
            parameter.AppendSql(" WHERE SDATE >= :FDATE                                                     ");
            parameter.AppendSql("   AND SDATE <= :SDATE                                                     ");
            parameter.AppendSql(" ORDER BY SDATE DESC                                                       ");

            parameter.Add("FDATE", strFDate);
            parameter.Add("SDATE", strSDate);

            return ExecuteReader<HIC_AMT_CANCER>(parameter);
        }

        public int DeleteByRowid(string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETTE ADMIN.HIC_AMT_CANCER    ");
            parameter.AppendSql("  WHERE ROWID = :RID              ");

            #region Query 변수대입
            parameter.Add("RID", argRowid);
            #endregion
            return ExecuteNonQuery(parameter);
        }
    }
}
