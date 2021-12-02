namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaResvLtdHisRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaResvLtdHisRepository()
        {
        }

        public int InsertData(HEA_RESV_LTD_HIS item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_RESV_LTD_HIS (                             ");
            parameter.AppendSql("       JOBTIME,JOBSABUN,GBJOB,LTDCODE,ENTSABUN,ENTTIME,SDATE           ");
            parameter.AppendSql(") VALUES (                                                             ");
            parameter.AppendSql("      SYSDATE, :JOBSABUN, :GBJOB, :LTDCODE, :ENTSABUN, SYSDATE, :SDATE ");
            parameter.AppendSql(")                                                                      ");

            #region Query 변수대입
            parameter.Add("JOBSABUN", item.JOBSABUN);
            parameter.Add("GBJOB", item.GBJOB);
            parameter.Add("LTDCODE", item.LTDCODE);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("SDATE", item.SDATE);

            #endregion

            return ExecuteNonQuery(parameter);
        }
    }
}
