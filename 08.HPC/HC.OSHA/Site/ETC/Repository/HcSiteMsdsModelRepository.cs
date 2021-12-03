namespace HC.OSHA.Site.ETC.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.OSHA.Site.ETC.Model;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcSiteMsdsModelRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public List<HC_SITE_MSDS_MODEL> FindAll(long estimateId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.PROCESS, A.PRODUCTNAME, A.USAGE, A.MONTHLYAMOUNT, A.REVISIONDATE, A.GHSPICTURE, A.MANUFACTURER,  C.NAME, C.CASNO,  B.QTY,  C.EXPOSURE_MATERIAL, C.WEM_MATERIAL, C.SPECIALHEALTH_MATERIAL, ");
            parameter.AppendSql("C.MANAGETARGET_MATERIAL, C.SPECIALMANAGE_MATERIAL, C.STANDARD_MATERIAL, C.PERMISSION_MATERIAL, C.PSM_MATERIAL ");
            parameter.AppendSql("FROM HC_SITE_PRODUCT A                 ");
            parameter.AppendSql("INNER JOIN HC_SITE_PRODUCT_MSDS B      ");
            parameter.AppendSql("ON A.ID = B.SITE_PRODUCT_ID            ");
            parameter.AppendSql("INNER JOIN HC_MSDS C                   ");
            parameter.AppendSql("ON C.ID = B.MSDS_ID                    ");
            parameter.AppendSql("WHERE A.ESTIMATE_ID = :ESTIMATE_ID     ");
            parameter.AppendSql("ORDER BY A.ID, B.MSDS_ID                     ");
            parameter.Add("ESTIMATE_ID", estimateId);

            return ExecuteReader<HC_SITE_MSDS_MODEL>(parameter);
        }
    }
}
