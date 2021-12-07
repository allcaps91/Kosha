namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using HC.Core.Service;
    using HC.OSHA.Dto;
    using HC_Core.Service;
    
    /// <summary>
    /// 
    /// </summary>
    public class HcSiteProductRepository : BaseRepository
    {
        public HC_SITE_PRODUCT FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_SITE_PRODUCT    ");
            parameter.AppendSql("WHERE ID = :ID                    ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_SITE_PRODUCT>(parameter);
        }
        public List<HC_SITE_PRODUCT> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_SITE_PRODUCT   ");
            parameter.AppendSql("WHERE SITE_ID = :SITE_ID         ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE     ");
            parameter.AppendSql("ORDER BY ID                      ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_SITE_PRODUCT>(parameter);
        }
        public HC_SITE_PRODUCT Insert(HC_SITE_PRODUCT dto)
        {
            dto.ID = GetSequenceNextVal("HC_SITE_PRODUCT_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_SITE_PRODUCT  ");
            parameter.AppendSql("(                             ");
            parameter.AppendSql("  ID,                         ");
            parameter.AppendSql("  SITE_ID,                    ");
            parameter.AppendSql("  PRODUCTNAME,                ");
            parameter.AppendSql("  PROCESS,                    ");
            parameter.AppendSql("  USAGE,                      ");
            parameter.AppendSql("  MANUFACTURER,               ");
            parameter.AppendSql("  MONTHLYAMOUNT,              ");
            parameter.AppendSql("  UNIT,                       ");
            parameter.AppendSql("  REVISIONDATE,               ");
            parameter.AppendSql("  GHSPICTURE,                 ");
            parameter.AppendSql("  MODIFIED,                   ");
            parameter.AppendSql("  MODIFIEDUSER,               ");
            parameter.AppendSql("  CREATED,                    ");
            parameter.AppendSql("  CREATEDUSER,                ");
            parameter.AppendSql("  SWLICENSE                   ");
            parameter.AppendSql(")                                                                            ");
            parameter.AppendSql("VALUES                        ");
            parameter.AppendSql("(                             ");
            parameter.AppendSql("  :ID,                        ");
            parameter.AppendSql("  :SITE_ID,                   ");
            parameter.AppendSql("  :PRODUCTNAME,               ");
            parameter.AppendSql("  :PROCESS,                   ");
            parameter.AppendSql("  :USAGE,                     ");
            parameter.AppendSql("  :MANUFACTURER,              ");
            parameter.AppendSql("  :MONTHLYAMOUNT,             ");
            parameter.AppendSql("  :UNIT,                      ");
            parameter.AppendSql("  :REVISIONDATE,              ");
            parameter.AppendSql("  :GHSPICTURE,                ");
            parameter.AppendSql("  SYSTIMESTAMP,               ");
            parameter.AppendSql("  :MODIFIEDUSER,              ");
            parameter.AppendSql("  SYSTIMESTAMP,               ");
            parameter.AppendSql("  :CREATEDUSER,               ");
            parameter.AppendSql("  :SWLICENSE                  ");
            parameter.AppendSql(")                             ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("PRODUCTNAME", dto.PRODUCTNAME);
            parameter.Add("PROCESS", dto.PROCESS);
            parameter.Add("USAGE", dto.USAGE);
            parameter.Add("MANUFACTURER", dto.MANUFACTURER);
            parameter.Add("MONTHLYAMOUNT", dto.MONTHLYAMOUNT);
            parameter.Add("UNIT", dto.UNIT);
            parameter.Add("REVISIONDATE", dto.REVISIONDATE);
            parameter.Add("GHSPICTURE", dto.GHSPICTURE);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Insert("HIC_SITE_PRODUCT", dto.ID);
            return FindOne(dto.ID);

        }

        public void Update(HC_SITE_PRODUCT dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_SITE_PRODUCT               ");
            parameter.AppendSql("SET                                   ");         
            parameter.AppendSql("  SITE_ID = :SITE_ID,                 ");
            parameter.AppendSql("  PRODUCTNAME = :PRODUCTNAME,         ");
            parameter.AppendSql("  PROCESS = :PROCESS,                 ");
            parameter.AppendSql("  USAGE = :USAGE,                     ");
            parameter.AppendSql("  MANUFACTURER = :MANUFACTURER,       ");
            parameter.AppendSql("  MONTHLYAMOUNT = :MONTHLYAMOUNT,     ");
            parameter.AppendSql("  UNIT = :UNIT,                       ");
            parameter.AppendSql("  REVISIONDATE = :REVISIONDATE,       ");
            parameter.AppendSql("  GHSPICTURE = :GHSPICTURE,           ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,            ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER        ");
            parameter.AppendSql("WHERE ID = :ID                        ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("PRODUCTNAME", dto.PRODUCTNAME);
            parameter.Add("PROCESS", dto.PROCESS);
            parameter.Add("USAGE", dto.USAGE);
            parameter.Add("MANUFACTURER", dto.MANUFACTURER);
            parameter.Add("MONTHLYAMOUNT", dto.MONTHLYAMOUNT);
            parameter.Add("UNIT", dto.UNIT);
            parameter.Add("REVISIONDATE", dto.REVISIONDATE);
            parameter.Add("GHSPICTURE", dto.GHSPICTURE);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Update("HIC_SITE_PRODUCT", dto.ID);

        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_SITE_PRODUCT  ");
            parameter.AppendSql("WHERE ID = :ID                ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE  ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_SITE_PRODUCT", id);
        }
    }
}
