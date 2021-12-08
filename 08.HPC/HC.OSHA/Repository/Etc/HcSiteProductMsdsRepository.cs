namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using HC_Core.Service;
    using HC.OSHA.Dto;
    using HC.OSHA.Model;
    using HC.Core.Service;
    
    /// <summary>
    /// 荤诀厘 力前 MSDS包府
    /// </summary>
    public class HcSiteProductMsdsRepository : BaseRepository
    {
     
        public List<HC_SITE_PRODUCT_MSDS_MODEL> FindAll(long site_product_id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.Id, A.SITE_PRODUCT_ID, A.MSDS_ID, A.QTY,  B.NAME, B.CASNO, B.EXPOSURE_MATERIAL, B.WEM_MATERIAL, B.SPECIALHEALTH_MATERIAL, B.MANAGETARGET_MATERIAL,   ");
            parameter.AppendSql("B.SPECIALMANAGE_MATERIAL, B.STANDARD_MATERIAL, B.PERMISSION_MATERIAL, B.PSM_MATERIAL, B.GHS_PICTURE, C.NAME AS MODIFIEDUSER  ");
            parameter.AppendSql("FROM HIC_SITE_PRODUCT_MSDS A            ");
            parameter.AppendSql("INNER JOIN HIC_MSDS B                   ");
            parameter.AppendSql("ON A.MSDS_ID = B.ID                    ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                  ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID           ");
            parameter.AppendSql("WHERE A.SITE_PRODUCT_ID = :SITE_PRODUCT_ID  ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE ");
            parameter.Add("SITE_PRODUCT_ID", site_product_id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_SITE_PRODUCT_MSDS_MODEL>(parameter);

        }
        public HC_SITE_PRODUCT_MSDS FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_SITE_PRODUCT_MSDS WHERE ID = :ID");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReaderSingle<HC_SITE_PRODUCT_MSDS>(parameter);
        }
        public HC_SITE_PRODUCT_MSDS Insert(HC_SITE_PRODUCT_MSDS dto)
        {
            MParameter parameter = CreateParameter();

            dto.ID = GetSequenceNextVal("HC_SITE_PRODUCT_MSDS_ID_SEQ");
            parameter.AppendSql("INSERT INTO HIC_SITE_PRODUCT_MSDS   ");
            parameter.AppendSql("(                         ");
            parameter.AppendSql("  ID,                     ");
            parameter.AppendSql("  SITE_PRODUCT_ID,        ");
            parameter.AppendSql("  MSDS_ID,                ");
            parameter.AppendSql("  QTY,                    ");
            parameter.AppendSql("  MODIFIED,               ");
            parameter.AppendSql("  MODIFIEDUSER,           ");
            parameter.AppendSql("  CREATED,                ");
            parameter.AppendSql("  CREATEDUSER,            ");
            parameter.AppendSql("  SWLICENSE               ");
            parameter.AppendSql(")                         ");
            parameter.AppendSql("VALUES                    ");
            parameter.AppendSql("(                         ");
            parameter.AppendSql("  :ID,                    ");
            parameter.AppendSql("  :SITE_PRODUCT_ID,       ");
            parameter.AppendSql("  :MSDS_ID,               ");
            parameter.AppendSql("  :QTY,                   ");
            parameter.AppendSql("  SYSTIMESTAMP,           ");
            parameter.AppendSql("  :MODIFIEDUSER,          ");
            parameter.AppendSql("  SYSTIMESTAMP,           ");
            parameter.AppendSql("  :CREATEDUSER,           ");
            parameter.AppendSql("  :SWLICENSE              ");
            parameter.AppendSql(")                         ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_PRODUCT_ID", dto.SITE_PRODUCT_ID);
            parameter.Add("MSDS_ID", dto.MSDS_ID);
            parameter.Add("QTY", dto.QTY);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_SITE_PRODUCT_MSDS", dto.ID);
            return FindOne(dto.ID);
        }

        public void Update(HC_SITE_PRODUCT_MSDS dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_SITE_PRODUCT_MSDS        ");
            parameter.AppendSql("SET                                 ");
            parameter.AppendSql("  QTY = :QTY,                       ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,          ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER      ");
            parameter.AppendSql("WHERE ID = :ID                      ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE        ");

            parameter.Add("ID", dto.ID);
            parameter.Add("QTY", dto.QTY);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_SITE_PRODUCT_MSDS", dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_SITE_PRODUCT_MSDS   ");
            parameter.AppendSql("WHERE ID = :ID                      ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE        ");
            parameter.Add("ID", id);
            ExecuteNonQuery(parameter);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            DataSyncService.Instance.Delete("HIC_SITE_PRODUCT_MSDS", id);
        }
    }
}
