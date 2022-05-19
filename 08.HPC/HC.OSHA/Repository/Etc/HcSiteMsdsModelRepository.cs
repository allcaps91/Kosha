namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using HC.OSHA.Model;
    
    /// <summary>
    /// 
    /// </summary>
    public class HcSiteMsdsModelRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public List<HC_SITE_MSDS_MODEL> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID, A.PROCESS, A.PRODUCTNAME, A.USAGE, A.MONTHLYAMOUNT, A.UNIT, ");
            parameter.AppendSql("       A.REVISIONDATE, A.GHSPICTURE, C.GHS_PICTURE AS BASE_GHSPICTURE,A.MANUFACTURER,");
            parameter.AppendSql("       C.NAME, B.CASNO,  B.QTY,  C.EXPOSURE_MATERIAL, C.WEM_MATERIAL, C.SPECIALHEALTH_MATERIAL, ");
            parameter.AppendSql("       C.MANAGETARGET_MATERIAL, C.SPECIALMANAGE_MATERIAL, C.STANDARD_MATERIAL, C.PERMISSION_MATERIAL, C.PSM_MATERIAL ");
            parameter.AppendSql("FROM HIC_SITE_PRODUCT A ");
            parameter.AppendSql("     INNER JOIN HIC_SITE_PRODUCT_MSDS B ");
            parameter.AppendSql("           ON A.ID = B.SITE_PRODUCT_ID ");
            parameter.AppendSql("           AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("     INNER JOIN HIC_MSDS C ");
            parameter.AppendSql("           ON C.CASNO = B.CASNO ");
            parameter.AppendSql("WHERE 1=1      ");
            parameter.AppendSql("  AND A.SITE_ID = :SITE_ID ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("ORDER BY A.ID, B.CASNO ");
            parameter.Add("SITE_ID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<HC_SITE_MSDS_MODEL>(parameter);
        }
    }
}
