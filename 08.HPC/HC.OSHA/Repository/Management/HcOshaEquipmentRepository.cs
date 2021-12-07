namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using HC.Core.Service;
    using HC.OSHA.Dto;
    using HC.OSHA.Model;
    using HC_Core.Service;

    /// <summary>
    /// 
    /// </summary>
    public class HcOshaEquipmentRepository : BaseRepository
    {
        /// <summary>
        /// 견적 계약 조인 모델 가져오기
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HC_OSHA_EQUIPMENT FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_EQUIPMENT A ");
            parameter.AppendSql("WHERE ID = :ID                    ");
            parameter.AppendSql("  AND ISDELETED ='N' ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_EQUIPMENT>(parameter);
        }
        public List<HC_OSHA_EQUIPMENT> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * ");
            parameter.AppendSql("  FROM HIC_OSHA_EQUIPMENT A");
            parameter.AppendSql("  WHERE SITE_ID = :ID ");
            parameter.AppendSql("  AND ISDELETED ='N' ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  ORDER BY A.ID DESC ");

            parameter.Add("ID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_EQUIPMENT>(parameter);
        }

        public HC_OSHA_EQUIPMENT Insert(HC_OSHA_EQUIPMENT dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_SHARED_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_EQUIPMENT");
            parameter.AppendSql("(");
            parameter.AppendSql("    ID");
            parameter.AppendSql("  , SITE_ID");
            parameter.AppendSql("  , REGDATE");
            parameter.AppendSql("  , NAME");
            parameter.AppendSql("  , MODELNAME");
            parameter.AppendSql("  , SERIALNUMBER");
            parameter.AppendSql("  , REMARK");
            parameter.AppendSql("  , ISDELETED");
            parameter.AppendSql("  , MODIFIED");
            parameter.AppendSql("  , MODIFIEDUSER");
            parameter.AppendSql("  , CREATED");
            parameter.AppendSql("  , CREATEDUSER");
            parameter.AppendSql("  , SWLICENSE");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :ID");
            parameter.AppendSql("  , :SITE_ID");
            parameter.AppendSql("  , :REGDATE");
            parameter.AppendSql("  , :NAME");
            parameter.AppendSql("  , :MODELNAME");
            parameter.AppendSql("  , :SERIALNUMBER");
            parameter.AppendSql("  , :REMARK");
            parameter.AppendSql("  , 'N'");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :MODIFIEDUSER");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :CREATEDUSER");
            parameter.AppendSql("  , :SWLICENSE");
            parameter.AppendSql(") ");

            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("REGDATE", dto.REGDATE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("MODELNAME", dto.MODELNAME);
            parameter.Add("SERIALNUMBER", dto.SERIALNUMBER);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_EQUIPMENT", dto.ID);

            return FindOne(dto.ID);
        }

        public void Update(HC_OSHA_EQUIPMENT dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_EQUIPMENT                                                        ");
            parameter.AppendSql("SET                                                                             ");
            parameter.AppendSql("  REGDATE = :REGDATE,                                                            ");
            parameter.AppendSql("  NAME = :NAME,                                                                 ");
            parameter.AppendSql("  MODELNAME = :MODELNAME,                                                       ");
            parameter.AppendSql("  SERIALNUMBER = :SERIALNUMBER,                                                 ");
            parameter.AppendSql("  REMARK = :REMARK,                                                 ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                      ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                                  ");
            parameter.AppendSql("WHERE ID = :ID                                                                  ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", dto.ID);
            parameter.Add("REGDATE", dto.REGDATE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("MODELNAME", dto.MODELNAME);
            parameter.Add("SERIALNUMBER", dto.SERIALNUMBER);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_EQUIPMENT", dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_EQUIPMENT");
            parameter.AppendSql("    SET ");
            parameter.AppendSql("       ISDELETED = 'Y' ");
            parameter.AppendSql("      , MODIFIED =  SYSTIMESTAMP");
            parameter.AppendSql("      , MODIFIEDUSER = :MODIFIEDUSER");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_EQUIPMENT", id);
        }
    }
}
