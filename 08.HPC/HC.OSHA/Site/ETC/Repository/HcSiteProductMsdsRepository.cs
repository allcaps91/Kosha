namespace HC.OSHA.Site.ETC.Repository
{
    using ComBase.Mvc;
    using HC.OSHA.Site.ETC.Dto;
    using HC.OSHA.Site.ETC.Model;
    using HC.Core.Common.Service;
    using System.Collections.Generic;


    /// <summary>
    /// 
    /// </summary>
    public class HcSiteProductMsdsRepository : BaseRepository
    {
     
        public List<HC_SITE_PRODUCT_MSDS_MODEL> FindAll(long site_product_id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.SITE_PRODUCT_ID, A.MSDS_ID, A.QTY,  B.NAME, B.CASNO, B.EXPOSURE_MATERIAL, B.WEM_MATERIAL, B.SPECIALHEALTH_MATERIAL, B.MANAGETARGET_MATERIAL,   ");
            parameter.AppendSql("B.SPECIALMANAGE_MATERIAL, B.STANDARD_MATERIAL, B.PERMISSION_MATERIAL, B.PSM_MATERIAL, B.GHS_PICTURE, C.NAME AS MODIFIEDUSER FROM HC_SITE_PRODUCT_MSDS A ");
            parameter.AppendSql("INNER JOIN HC_MSDS B                   ");
            parameter.AppendSql("ON A.MSDS_ID = B.ID                    ");
            parameter.AppendSql("INNER JOIN HC_USERS C                  ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID           ");
            parameter.AppendSql("WHERE A.SITE_PRODUCT_ID = :SITE_PRODUCT_ID            ");
            parameter.Add("SITE_PRODUCT_ID", site_product_id);

            return ExecuteReader<HC_SITE_PRODUCT_MSDS_MODEL>(parameter);

        }
        public void Insert(HC_SITE_PRODUCT_MSDS dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HC_SITE_PRODUCT_MSDS                                            ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  SITE_PRODUCT_ID,                                                          ");
            parameter.AppendSql("  MSDS_ID,                                                                  ");
            parameter.AppendSql("  QTY,                                                                      ");
            parameter.AppendSql("  MODIFIED,                                                                 ");
            parameter.AppendSql("  MODIFIEDUSER,                                                              ");
            parameter.AppendSql("  CREATED,                                                                  ");
            parameter.AppendSql("  CREATEDUSER                                                              ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :SITE_PRODUCT_ID,                                                         ");
            parameter.AppendSql("  :MSDS_ID,                                                                 ");
            parameter.AppendSql("  :QTY,                                                                     ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                             ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                 ");
            parameter.AppendSql("  :CREATEDUSER                                                             ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("SITE_PRODUCT_ID", dto.SITE_PRODUCT_ID);
            parameter.Add("MSDS_ID", dto.MSDS_ID);
            parameter.Add("QTY", dto.QTY);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

        }

        public void Update(HC_SITE_PRODUCT_MSDS dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_SITE_PRODUCT_MSDS                                                 ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  QTY = :QTY,                                                               ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                     ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                               ");
            parameter.AppendSql("WHERE SITE_PRODUCT_ID = :SITE_PRODUCT_ID                                                               ");
            parameter.AppendSql("AND MSDS_ID = :MSDS_ID                                                      ");
            parameter.Add("SITE_PRODUCT_ID", dto.SITE_PRODUCT_ID);
            parameter.Add("MSDS_ID", dto.MSDS_ID);
            parameter.Add("QTY", dto.QTY);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            ExecuteNonQuery(parameter);

        }

        public void Delete(long site_product_id, long msds_id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HC_SITE_PRODUCT_MSDS                                            ");
            parameter.AppendSql("WHERE SITE_PRODUCT_ID = :SITE_PRODUCT_ID                                    ");
            parameter.AppendSql("AND MSDS_ID = :MSDS_ID                                                      ");
            parameter.Add("SITE_PRODUCT_ID", site_product_id);
            parameter.Add("MSDS_ID", msds_id);
            ExecuteNonQuery(parameter);

        }
    }
}
