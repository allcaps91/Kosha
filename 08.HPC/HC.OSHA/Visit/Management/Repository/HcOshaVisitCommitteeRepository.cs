namespace HC.OSHA.Visit.Management.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.OSHA.Visit.Management.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcOshaVisitCommitteeRepository : BaseRepository
    {

        public List<HC_OSHA_VISIT_COMMITTEE> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_OSHA_VISIT_COMMITTEE                                               ");
            parameter.AppendSql("WHERE SITE_ID = :siteId   ORDER BY REGDATE DESC                                           ");

            parameter.Add("siteId", siteId);

            return ExecuteReader<HC_OSHA_VISIT_COMMITTEE>(parameter);

        }
        public void Insert(HC_OSHA_VISIT_COMMITTEE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HC_OSHA_VISIT_COMMITTEE                                         ");
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
            parameter.AppendSql("  HC_OSHA_SHARED_ID_SEQ.NEXTVAL,                                                                      ");
            parameter.AppendSql("  :SITE_ID,                                                                 ");
            parameter.AppendSql("  :REGDATE,                                                                 ");
            parameter.AppendSql("  :METTINGTYPE,                                                             ");
            parameter.AppendSql("  :MEETINGKIND,                                                             ");
            parameter.AppendSql("  :MEETINGUSER                                                             ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("REGDATE", dto.REGDATE);
            parameter.Add("METTINGTYPE", dto.METTINGTYPE);
            parameter.Add("MEETINGKIND", dto.MEETINGKIND);
            parameter.Add("MEETINGUSER", dto.MEETINGUSER);


            ExecuteNonQuery(parameter);

        }

        public void Update(HC_OSHA_VISIT_COMMITTEE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_OSHA_VISIT_COMMITTEE                                              ");
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

        }

        public void Delete(HC_OSHA_VISIT_COMMITTEE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HC_OSHA_VISIT_COMMITTEE                                               ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            ExecuteNonQuery(parameter);

        }
    }
}
