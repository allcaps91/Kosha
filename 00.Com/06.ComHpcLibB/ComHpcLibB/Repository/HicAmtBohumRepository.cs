namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicAmtBohumRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicAmtBohumRepository()
        {
        }

        public IList<HIC_AMT_BOHUM> FindAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SDATE, ROWID AS RID         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_AMT_BOHUM   ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql(" ORDER BY SDATE DESC               ");

            return ExecuteReader<HIC_AMT_BOHUM>(parameter);
        }

        public IList<HIC_AMT_BOHUM> GetListBySDate(string strDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SDATE,ONE_AMT01,ONE_AMT02,ONE_AMT03,ONE_AMT04,ONE_AMT05,ONE_AMT06                       ");
            parameter.AppendSql("      ,ONE_AMT07, ONE_AMT08, ONE_AMT09, ONE_AMT10, ONE_AMT11, ONE_AMT12, ONE_AMT13, ONE_AMT14  ");
            parameter.AppendSql("      ,ONE_AMT15, ONE_AMT16, ONE_AMT17, ONE_AMT18, ONE_AMT19, ONE_AMT20, ONE_AMT21, ONE_AMT22  ");
            parameter.AppendSql("      ,ONE_AMT23, ONE_AMT24, ONE_AMT25, ONE_DENT1, ONE_DENT2, TWO_AMT01, TWO_AMT02, TWO_AMT03  ");
            parameter.AppendSql("      ,TWO_AMT04, TWO_AMT05, TWO_AMT06, TWO_AMT07, TWO_AMT08, TWO_AMT09, TWO_AMT10, TWO_AMT11  ");
            parameter.AppendSql("      ,TWO_AMT12, TWO_AMT13, TWO_AMT14, TWO_AMT15                                              ");
            parameter.AppendSql("      ,ROWID AS RID                                                                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_AMT_BOHUM                                                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                   ");
            parameter.AppendSql("   AND SDATE =:SDATE                                                                           ");
            parameter.AppendSql(" ORDER BY SDATE DESC                                                                           ");

            parameter.Add("SDATE", strDate);

            return ExecuteReader<HIC_AMT_BOHUM>(parameter);
        }

        public List<HIC_AMT_BOHUM> GetItembySDate(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SDATE,ONE_AMT01,ONE_AMT02,ONE_AMT03,ONE_AMT04,ONE_AMT05,ONE_AMT06                       ");
            parameter.AppendSql("      ,ONE_AMT07, ONE_AMT08, ONE_AMT09, ONE_AMT10, ONE_AMT11, ONE_AMT12, ONE_AMT13, ONE_AMT14  ");
            parameter.AppendSql("      ,ONE_AMT15, ONE_AMT16, ONE_AMT17, ONE_AMT18, ONE_AMT19, ONE_AMT20, ONE_AMT21, ONE_AMT22  ");
            parameter.AppendSql("      ,ONE_AMT23, ONE_AMT24, ONE_AMT25, ONE_DENT1, ONE_DENT2, TWO_AMT01, TWO_AMT02, TWO_AMT03  ");
            parameter.AppendSql("      ,TWO_AMT04, TWO_AMT05, TWO_AMT06, TWO_AMT07, TWO_AMT08, TWO_AMT09, TWO_AMT10, TWO_AMT11  ");
            parameter.AppendSql("      ,TWO_AMT12, TWO_AMT13, TWO_AMT14, TWO_AMT15                                              ");
            parameter.AppendSql("      ,ROWID AS RID                                                                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_AMT_BOHUM                                                               ");
            parameter.AppendSql(" WHERE SDATE >= :FDATE                                                                         ");
            parameter.AppendSql("   AND SDATE <= :TDATE                                                                         ");
            parameter.AppendSql(" ORDER BY SDate DESC                                                                           ");

            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);

            return ExecuteReader<HIC_AMT_BOHUM>(parameter);
        }

        public int DeleteByRowid(string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETTE KOSMOS_PMPA.HIC_AMT_BOHUM    ");
            parameter.AppendSql("  WHERE ROWID = :RID              ");

            #region Query 변수대입
            parameter.Add("RID", argRowid);
            #endregion
            return ExecuteNonQuery(parameter);
        }
    }
}
