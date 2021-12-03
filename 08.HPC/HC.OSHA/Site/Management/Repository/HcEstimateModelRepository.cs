namespace HC.OSHA.Site.Management.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.OSHA.Site.Management.Model;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcEstimateModelRepository : BaseRepository
    {
        /// <summary>
        /// 견적 계약 조인 모델 가져오기
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<HC_ESTIMATE_MODEL> FindBySiteId(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID, A.ESTIMATEDATE, B.ISCONTRACT, B.CONTRACTDATE, B.CONTRACTSTARTDATE, B.CONTRACTENDDATE FROM HC_OSHA_ESTIMATE A ");
            parameter.AppendSql("LEFT OUTER JOIN HC_OSHA_CONTRACT B                             ");
            parameter.AppendSql("ON A.ID = B.ESTIMATE_ID                                        ");
            parameter.AppendSql("WHERE A.OSHA_SITE_ID = :ID                                     ");
            parameter.AppendSql("AND A.ISDELETED = 'N'                                          ");
            parameter.AppendSql("AND B.ISDELETED = 'N'                                          ");
            parameter.AppendSql("ORDER BY A.ESTIMATEDATE DESC                                   ");

            parameter.Add("ID", id);

            return ExecuteReader<HC_ESTIMATE_MODEL>(parameter);
        }
    }
}
