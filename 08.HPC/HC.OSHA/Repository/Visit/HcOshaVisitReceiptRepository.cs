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
    public class HcOshaVisitReceiptRepository : BaseRepository
    {
        public HC_OSHA_VISIT_RECEIPT FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT_RECEIPT ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_VISIT_RECEIPT>(parameter);
        }
        public List<HC_OSHA_VISIT_RECEIPT> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT_RECEIPT ");
            parameter.AppendSql(" WHERE SITE_ID = :siteId ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" ORDER BY REGDATE DESC ");

            parameter.Add("siteId", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_VISIT_RECEIPT>(parameter);
        }

        public HC_OSHA_VISIT_RECEIPT Insert(HC_OSHA_VISIT_RECEIPT dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_SHARED_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_VISIT_RECEIPT ");
            parameter.AppendSql(" (ID,SITE_ID,REGDATE,REGUSERID,REGUSERNAME,CANCELDATE,");
            parameter.AppendSql("  TAKEOVER,REMARK,SWLICENSE) ");
            parameter.AppendSql(" VALUES ");
            parameter.AppendSql(" (:ID,:SITE_ID,:REGDATE,:REGUSERID,:REGUSERNAME,:CANCELDATE,");
            parameter.AppendSql("  :TAKEOVER,:REMARK,:SWLICENSE) ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("REGDATE", dto.REGDATE);
            parameter.Add("REGUSERID", dto.REGUSERID);
            parameter.Add("REGUSERNAME", dto.REGUSERNAME);
            parameter.Add("CANCELDATE", dto.CANCELDATE);
            parameter.Add("TAKEOVER", dto.TAKEOVER);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_VISIT_RECEIPT", dto.ID);

            return FindOne(dto.ID);
        }

        public void Update(HC_OSHA_VISIT_RECEIPT dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_VISIT_RECEIPT SET ");
            parameter.AppendSql("  REGDATE = :REGDATE, ");
            parameter.AppendSql("  REGUSERID = :REGUSERID, ");
            parameter.AppendSql("  REGUSERNAME = :REGUSERNAME, ");
            parameter.AppendSql("  CANCELDATE = :CANCELDATE, ");
            parameter.AppendSql("  TAKEOVER = :TAKEOVER, ");
            parameter.AppendSql("  REMARK = :REMARK ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", dto.ID);
            parameter.Add("REGDATE", dto.REGDATE);
            parameter.Add("REGUSERID", dto.REGUSERID);
            parameter.Add("REGUSERNAME", dto.REGUSERNAME);
            parameter.Add("CANCELDATE", dto.CANCELDATE);
            parameter.Add("TAKEOVER", dto.TAKEOVER);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_VISIT_RECEIPT", dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_VISIT_RECEIPT ");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_VISIT_RECEIPT", id);
        }
    }
}
