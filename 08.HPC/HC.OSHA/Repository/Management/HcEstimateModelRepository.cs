namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using HC.OSHA.Model;
    
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
            parameter.AppendSql("SELECT A.ID, A.ESTIMATEDATE, B.ISCONTRACT, B.CONTRACTDATE, B.CONTRACTSTARTDATE, B.CONTRACTENDDATE FROM HIC_OSHA_ESTIMATE A ");
            parameter.AppendSql("LEFT OUTER JOIN HIC_OSHA_CONTRACT B                             ");
            parameter.AppendSql("ON A.ID = B.ESTIMATE_ID                                        ");
            parameter.AppendSql("AND B.ISDELETED = 'N'                                          ");
            parameter.AppendSql("WHERE A.OSHA_SITE_ID = :ID                                     ");
            parameter.AppendSql("AND A.ISDELETED = 'N'                                          ");
            parameter.AppendSql("AND A.SWLICENSE = :SWLICENSE1                                  ");
            parameter.AppendSql("AND B.SWLICENSE = :SWLICENSE2                                  ");
            parameter.AppendSql("ORDER BY A.ESTIMATEDATE DESC                                   ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_ESTIMATE_MODEL>(parameter);
        }
        /// <summary>
        /// 견적 계약 조인 모델 가져오기(계약된것만)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<HC_ESTIMATE_MODEL> FindBySiteIdAndContract(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID, A.ESTIMATEDATE, B.ISCONTRACT, B.CONTRACTDATE, B.CONTRACTSTARTDATE, B.CONTRACTENDDATE FROM HIC_OSHA_ESTIMATE A ");
            parameter.AppendSql("INNER JOIN HIC_OSHA_CONTRACT B                             ");
            parameter.AppendSql("ON A.ID = B.ESTIMATE_ID                                        ");
            parameter.AppendSql("AND B.ISDELETED = 'N'                                          ");
            parameter.AppendSql("WHERE A.OSHA_SITE_ID = :ID                                     ");
            parameter.AppendSql("AND A.ISDELETED = 'N'                                          ");
            parameter.AppendSql("AND A.SWLICENSE = :SWLICENSE1                                  ");
            parameter.AppendSql("AND B.SWLICENSE = :SWLICENSE2                                  ");
            //parameter.AppendSql("AND B.ISCONTRACT = 'Y'                                          ");
            parameter.AppendSql("ORDER BY A.ESTIMATEDATE DESC                                   ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_ESTIMATE_MODEL>(parameter);
        }
        public HC_ESTIMATE_MODEL FindByEstimateId(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID, A.ESTIMATEDATE, B.ISCONTRACT, B.CONTRACTDATE, B.CONTRACTSTARTDATE, B.CONTRACTENDDATE FROM HIC_OSHA_ESTIMATE A ");
            parameter.AppendSql("LEFT OUTER JOIN HIC_OSHA_CONTRACT B                             ");
            parameter.AppendSql("ON A.ID = B.ESTIMATE_ID                                        ");
            parameter.AppendSql("AND B.ISDELETED = 'N'                                          ");
            parameter.AppendSql("WHERE A.ID = :ID                                               ");
            parameter.AppendSql("AND A.ISDELETED = 'N'                                          ");
            parameter.AppendSql("AND A.SWLICENSE = :SWLICENSE1                                  ");
            parameter.AppendSql("AND B.SWLICENSE = :SWLICENSE2                                  ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_ESTIMATE_MODEL>(parameter);
        }
    }
}
