namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using HC.OSHA.Dto;
    using HC_Core.Service;

    /// <summary>
    /// 
    /// </summary>
    public class HcOshaVisitInformationRepository : BaseRepository
    {
        public HC_OSHA_VISIT_INFORMATION FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT_INFORMATION ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_VISIT_INFORMATION>(parameter);

        }
        public List<HC_OSHA_VISIT_INFORMATION> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT_INFORMATION ");
            parameter.AppendSql(" WHERE SITE_ID = :siteId ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" ORDER BY REGDATE DESC ");

            parameter.Add("siteId", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_VISIT_INFORMATION>(parameter);

        }
        public HC_OSHA_VISIT_INFORMATION Insert(HC_OSHA_VISIT_INFORMATION dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_SHARED_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_VISIT_INFORMATION ");
            parameter.AppendSql("  (ID,SITE_ID,REGDATE,REGUSERID,REGUSERNAME, ");
            parameter.AppendSql("   INFORMATIONTYPE,REMARK,SWLICENSE) ");
            parameter.AppendSql(" VALUES ");
            parameter.AppendSql(" (:ID,:SITE_ID,:REGDATE,:REGUSERID,:REGUSERNAME, ");
            parameter.AppendSql("  :INFORMATIONTYPE,:REMARK,:SWLICENSE) ");

            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("REGDATE", dto.REGDATE);
            parameter.Add("REGUSERID", dto.REGUSERID);
            parameter.Add("REGUSERNAME", dto.REGUSERNAME);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            parameter.Add("INFORMATIONTYPE", dto.INFORMATIONTYPE);
            parameter.Add("REMARK", dto.REMARK);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_VISIT_INFORMATION", dto.ID);

            return FindOne(dto.ID);
        }

        public void Update(HC_OSHA_VISIT_INFORMATION dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_VISIT_INFORMATION SET ");
            parameter.AppendSql("  SITE_ID = :SITE_ID, ");
            parameter.AppendSql("  REGDATE = :REGDATE, ");
            parameter.AppendSql("  REGUSERID = :REGUSERID, ");
            parameter.AppendSql("  REGUSERNAME = :REGUSERNAME, ");
            parameter.AppendSql("  INFORMATIONTYPE = :INFORMATIONTYPE, ");
            parameter.AppendSql("  REMARK = :REMARK ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("REGDATE", dto.REGDATE);
            parameter.Add("REGUSERID", dto.REGUSERID);
            parameter.Add("REGUSERNAME", dto.REGUSERNAME);
            parameter.Add("INFORMATIONTYPE", dto.INFORMATIONTYPE);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_VISIT_INFORMATION", dto.ID);

        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_VISIT_INFORMATION ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_VISIT_INFORMATION", id);
        }
    }
}
