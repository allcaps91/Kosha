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
    public class HcMsdsProductRepository : BaseRepository
    {
        public HC_MSDS_PRODUCT FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_MSDS_PRODUCT ");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_MSDS_PRODUCT>(parameter);
        }
        public HC_MSDS_PRODUCT FindByName(string viewName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_MSDS_PRODUCT ");
            parameter.AppendSql("WHERE PRODUCTNAME = :VIEWNAME ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("VIEWNAME", viewName);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_MSDS_PRODUCT>(parameter);
        }

        public List<HC_MSDS_PRODUCT> FindAll(string strName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_MSDS_PRODUCT ");
            parameter.AppendSql(" WHERE SWLICENSE = :SWLICENSE ");
            if (strName!="") parameter.AppendSql(" AND PRODUCTNAME LIKE :PRODUCTNAME ");
            parameter.AppendSql("ORDER BY PRODUCTNAME ");

            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            if (strName != "") parameter.AddLikeStatement("PRODUCTNAME", strName);

            return ExecuteReader<HC_MSDS_PRODUCT>(parameter);
        }
        public HC_MSDS_PRODUCT Insert(HC_MSDS_PRODUCT dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_MSDS_PRODUCT_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_MSDS_PRODUCT  ");
            parameter.AppendSql("(                             ");
            parameter.AppendSql("  ID,                         ");
            parameter.AppendSql("  PRODUCTNAME,                ");
            parameter.AppendSql("  USAGE,                      ");
            parameter.AppendSql("  MANUFACTURER,               ");
            parameter.AppendSql("  MSDSCODE,                   ");
            parameter.AppendSql("  MODIFIED,                   ");
            parameter.AppendSql("  MODIFIEDUSER,               ");
            parameter.AppendSql("  CREATED,                    ");
            parameter.AppendSql("  CREATEDUSER,                ");
            parameter.AppendSql("  SWLICENSE                   ");
            parameter.AppendSql(")                                                                            ");
            parameter.AppendSql("VALUES                        ");
            parameter.AppendSql("(                             ");
            parameter.AppendSql("  :ID,                        ");
            parameter.AppendSql("  :PRODUCTNAME,               ");
            parameter.AppendSql("  :USAGE,                     ");
            parameter.AppendSql("  :MANUFACTURER,              ");
            parameter.AppendSql("  :MSDSCODE,                  ");
            parameter.AppendSql("  SYSTIMESTAMP,               ");
            parameter.AppendSql("  :MODIFIEDUSER,              ");
            parameter.AppendSql("  SYSTIMESTAMP,               ");
            parameter.AppendSql("  :CREATEDUSER,               ");
            parameter.AppendSql("  :SWLICENSE                  ");
            parameter.AppendSql(")                             ");
            parameter.Add("ID", dto.ID);
            parameter.Add("PRODUCTNAME", dto.PRODUCTNAME);
            parameter.Add("USAGE", dto.USAGE);
            parameter.Add("MANUFACTURER", dto.MANUFACTURER);
            parameter.Add("MSDSCODE", dto.MSDSCODE);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Insert("HIC_OSHA_MSDS_PRODUCT", dto.ID);
            return FindOne(dto.ID);

        }

        public void Update(HC_MSDS_PRODUCT dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_MSDS_PRODUCT          ");
            parameter.AppendSql("SET                                   ");
            parameter.AppendSql("  PRODUCTNAME = :PRODUCTNAME,         ");
            parameter.AppendSql("  USAGE = :USAGE,                     ");
            parameter.AppendSql("  MANUFACTURER = :MANUFACTURER,       ");
            parameter.AppendSql("  MSDSCODE = :MSDSCODE,               ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,            ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER        ");
            parameter.AppendSql("WHERE ID = :ID                        ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("PRODUCTNAME", dto.PRODUCTNAME);
            parameter.Add("USAGE", dto.USAGE);
            parameter.Add("MSDSCODE", dto.MSDSCODE);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Update("HIC_OSHA_MSDS_PRODUCT", dto.ID);
        }

        public void Update_Msdslist(long id,string msdslist)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_MSDS_PRODUCT          ");
            parameter.AppendSql("SET                                   ");
            parameter.AppendSql("  MSDSLIST = :MSDSLIST,               ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,            ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER        ");
            parameter.AppendSql("WHERE ID = :ID                        ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("MSDSLIST", msdslist);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Update("HIC_OSHA_MSDS_PRODUCT", id);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_MSDS_PRODUCT ");
            parameter.AppendSql("WHERE ID = :ID                ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE  ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_MSDS_PRODUCT", id);
        }
    }
}
