namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
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
            parameter.AppendSql("SELECT * FROM HC_OSHA_VISIT_RECEIPT                                               ");
            parameter.AppendSql("WHERE ID = :ID                                         ");

            parameter.Add("ID", id);

            return ExecuteReaderSingle<HC_OSHA_VISIT_RECEIPT>(parameter);

        }
        public List<HC_OSHA_VISIT_RECEIPT> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT_RECEIPT                                               ");
            parameter.AppendSql("WHERE SITE_ID = :siteId   ORDER BY REGDATE DESC                                           ");

            parameter.Add("siteId", siteId);

            return ExecuteReader<HC_OSHA_VISIT_RECEIPT>(parameter);

        }
        public HC_OSHA_VISIT_RECEIPT Insert(HC_OSHA_VISIT_RECEIPT dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_SHARED_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_VISIT_RECEIPT                                           ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                                  ");
            parameter.AppendSql("  REGDATE,                                                                  ");
            parameter.AppendSql("  REGUSERID,                                                                ");
            parameter.AppendSql("  REGUSERNAME,                                                                ");
            parameter.AppendSql("  CANCELDATE,                                                          ");
            parameter.AppendSql("  TAKEOVER,                                                                 ");
            parameter.AppendSql("  REMARK                                                                   ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :ID,                                                                      ");
            parameter.AppendSql("  :SITE_ID,                                                                 ");
            parameter.AppendSql("  :REGDATE,                                                                 ");
            parameter.AppendSql("  :REGUSERNAME,                                                                ");
            parameter.AppendSql("  :REGUSERID,                                                               ");
            parameter.AppendSql("  :CANCELDATE,                                                         ");
            parameter.AppendSql("  :TAKEOVER,                                                                ");
            parameter.AppendSql("  :REMARK                                                                  ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("REGDATE", dto.REGDATE);
            parameter.Add("REGUSERID", dto.REGUSERID);
            parameter.Add("REGUSERNAME", dto.REGUSERNAME);
            parameter.Add("CANCELDATE", dto.CANCELDATE);
            parameter.Add("TAKEOVER", dto.TAKEOVER);
            parameter.Add("REMARK", dto.REMARK);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_VISIT_RECEIPT", dto.ID);

            return FindOne(dto.ID);



        }

        public void Update(HC_OSHA_VISIT_RECEIPT dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_VISIT_RECEIPT                                                ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  REGDATE = :REGDATE,                                                       ");
            parameter.AppendSql("  REGUSERID = :REGUSERID,                                                   ");
            parameter.AppendSql("  REGUSERNAME = :REGUSERNAME,                                                   ");
            parameter.AppendSql("  CANCELDATE = :CANCELDATE,                                       ");
            parameter.AppendSql("  TAKEOVER = :TAKEOVER,                                                     ");
            parameter.AppendSql("  REMARK = :REMARK                                                         ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");

            parameter.Add("ID", dto.ID);
            parameter.Add("REGDATE", dto.REGDATE);
            parameter.Add("REGUSERID", dto.REGUSERID);
            parameter.Add("REGUSERNAME", dto.REGUSERNAME);
            parameter.Add("CANCELDATE", dto.CANCELDATE);
            parameter.Add("TAKEOVER", dto.TAKEOVER);
            parameter.Add("REMARK", dto.REMARK);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_VISIT_RECEIPT", dto.ID);


        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_VISIT_RECEIPT                                           ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", id);
            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_VISIT_RECEIPT", id);

        }
    }
}
