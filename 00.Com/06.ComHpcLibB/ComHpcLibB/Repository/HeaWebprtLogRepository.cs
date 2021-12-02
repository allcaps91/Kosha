namespace ComHpcLibB.Repository
{

    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HeaWebprtLogRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HeaWebprtLogRepository()
        {
        }


        public int Insert(HEA_WEBPRT_LOG item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_WEBPRT_LOG                                                         ");
            parameter.AppendSql("       (REQDATE, WRTNO, SNAME, GJJONG, ENTSABUN)          ");
            parameter.AppendSql("VALUES                                                                                         ");
            parameter.AppendSql("       (:REQDATE, :WRTNO, :SNAME, :GJJONG, :ENTSABUN)   ");

            parameter.Add("REQDATE", item.REQDATE);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("GJJONG", item.GJJONG);
            parameter.Add("ENTSABUN", item.ENTSABUN);

            return ExecuteNonQuery(parameter);
        }

        public HEA_WEBPRT_LOG GetItemByWrtnoGjjong(long argWrtno, string argGjjong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, GJJONG                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_WEBPRT_LOG                      ");
            parameter.AppendSql(" WHERE 1=1                                             ");                        
            parameter.AppendSql(" AND WRTNO = :WRTNO                                    ");
            parameter.AppendSql(" AND GJJONG = :GJJONG                                  ");
            parameter.Add("WRTNO", argWrtno);
            parameter.Add("GJJONG", argGjjong);

            return ExecuteReaderSingle<HEA_WEBPRT_LOG>(parameter);
        }
    }
}
