namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class BasJobRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public BasJobRepository()
        {
        }


        public List<BAS_JOB> READ_Holyday(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(JOBDATE,'DD') ILJA                  ");
            parameter.AppendSql("  FROM ADMIN.BAS_JOB                         ");
            parameter.AppendSql(" WHERE  1=1                                        ");
            parameter.AppendSql("   AND JOBDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND JOBDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND HOLYDAY = '*'                               ");

            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);

            return ExecuteReader<BAS_JOB>(parameter);
        }

        public string GetHolyDay(string strDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT HolyDay                                     ");
            parameter.AppendSql("  FROM ADMIN.BAS_JOB                         ");
            parameter.AppendSql(" WHERE JOBDATE = TO_DATE(:JOBDATE, 'YYYY-MM-DD')   ");

            parameter.Add("JOBDATE", strDate);

            return ExecuteScalar<string>(parameter);
        }
    }
}
