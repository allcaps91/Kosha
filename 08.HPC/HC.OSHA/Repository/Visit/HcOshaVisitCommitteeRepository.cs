namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.OSHA.Dto;
    using HC_Core.Service;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaVisitCommitteeRepository : BaseRepository
    {
        public HC_OSHA_VISIT_COMMITTEE FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT_COMMITTEE                                               ");
            parameter.AppendSql("WHERE ID = :ID                                         ");

            parameter.Add("ID", id);

            return ExecuteReaderSingle<HC_OSHA_VISIT_COMMITTEE>(parameter);

        }
        public List<HC_OSHA_VISIT_COMMITTEE> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT_COMMITTEE                                               ");
            parameter.AppendSql("WHERE SITE_ID = :siteId   ORDER BY REGDATE DESC                                           ");

            parameter.Add("siteId", siteId);

            return ExecuteReader<HC_OSHA_VISIT_COMMITTEE>(parameter);

        }
        public HC_OSHA_VISIT_COMMITTEE Insert(HC_OSHA_VISIT_COMMITTEE dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_SHARED_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_VISIT_COMMITTEE                                         ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                                  ");
            parameter.AppendSql("  REGDATE,                                                                  ");
            parameter.AppendSql("  METTINGTYPE,                                                              ");
            parameter.AppendSql("  MEETINGKIND,                                                              ");
            parameter.AppendSql("  MEETINGUSER                                                              ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :ID,                                                                      ");
            parameter.AppendSql("  :SITE_ID,                                                                 ");
            parameter.AppendSql("  :REGDATE,                                                                 ");
            parameter.AppendSql("  :METTINGTYPE,                                                             ");
            parameter.AppendSql("  :MEETINGKIND,                                                             ");
            parameter.AppendSql("  :MEETINGUSER                                                             ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("REGDATE", dto.REGDATE);
            parameter.Add("METTINGTYPE", dto.METTINGTYPE);
            parameter.Add("MEETINGKIND", dto.MEETINGKIND);
            parameter.Add("MEETINGUSER", dto.MEETINGUSER);


            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Insert("HIC_OSHA_VISIT_COMMITTEE", dto.ID);
            return FindOne(dto.ID);
        }

        public void Update(HC_OSHA_VISIT_COMMITTEE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_VISIT_COMMITTEE                                              ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  ID = :ID,                                                                 ");
            parameter.AppendSql("  SITE_ID = :SITE_ID,                                                       ");
            parameter.AppendSql("  REGDATE = :REGDATE,                                                       ");
            parameter.AppendSql("  METTINGTYPE = :METTINGTYPE,                                               ");
            parameter.AppendSql("  MEETINGKIND = :MEETINGKIND,                                               ");
            parameter.AppendSql("  MEETINGUSER = :MEETINGUSER                                               ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("REGDATE", dto.REGDATE);
            parameter.Add("METTINGTYPE", dto.METTINGTYPE);
            parameter.Add("MEETINGKIND", dto.MEETINGKIND);
            parameter.Add("MEETINGUSER", dto.MEETINGUSER);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_VISIT_COMMITTEE", dto.ID);

        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_VISIT_COMMITTEE                                               ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", id);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_VISIT_COMMITTEE", id);

        }
    }
}
