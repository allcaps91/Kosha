namespace HC.OSHA.Visit.Management.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.OSHA.Visit.Management.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcOshaVisitEduRepository : BaseRepository
    {
        public List<HC_OSHA_VISIT_EDU> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_OSHA_VISIT_EDU                                               ");
            parameter.AppendSql("WHERE SITE_ID = :siteId   ORDER BY EDUDATE DESC                                           ");

            parameter.Add("siteId", siteId);

            return ExecuteReader<HC_OSHA_VISIT_EDU>(parameter);

        }
        public void Insert(HC_OSHA_VISIT_EDU dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HC_OSHA_VISIT_EDU                                               ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                                  ");
            parameter.AppendSql("  EDUDATE,                                                                  ");
            parameter.AppendSql("  EDUTYPE,                                                                  ");
            parameter.AppendSql("  TARGET,                                                                   ");
            parameter.AppendSql("  TITLE,                                                                    ");
            parameter.AppendSql("  LOCATION,                                                                 ");
            parameter.AppendSql("  EDUUSERID,                                                                ");
            parameter.AppendSql("  EDUUSERNAME                                                              ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  HC_OSHA_SHARED_ID_SEQ.NEXTVAL,                                                                      ");
            parameter.AppendSql("  :SITE_ID,                                                                 ");
            parameter.AppendSql("  :EDUDATE,                                                                 ");
            parameter.AppendSql("  :EDUTYPE,                                                                 ");
            parameter.AppendSql("  :TARGET,                                                                  ");
            parameter.AppendSql("  :TITLE,                                                                   ");
            parameter.AppendSql("  :LOCATION,                                                                ");
            parameter.AppendSql("  :EDUUSERID,                                                               ");
            parameter.AppendSql("  :EDUUSERNAME                                                             ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("EDUDATE", dto.EDUDATE);
            parameter.Add("EDUTYPE", dto.EDUTYPE);
            parameter.Add("TARGET", dto.TARGET);
            parameter.Add("TITLE", dto.TITLE);
            parameter.Add("LOCATION", dto.LOCATION);
            parameter.Add("EDUUSERID", dto.EDUUSERID);
            parameter.Add("EDUUSERNAME", dto.EDUUSERNAME);
            ExecuteNonQuery(parameter);

        }

        public void Update(HC_OSHA_VISIT_EDU dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_OSHA_VISIT_EDU                                                    ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  SITE_ID = :SITE_ID,                                                       ");
            parameter.AppendSql("  EDUDATE = :EDUDATE,                                                       ");
            parameter.AppendSql("  EDUTYPE = :EDUTYPE,                                                       ");
            parameter.AppendSql("  TARGET = :TARGET,                                                         ");
            parameter.AppendSql("  TITLE = :TITLE,                                                           ");
            parameter.AppendSql("  LOCATION = :LOCATION,                                                     ");
            parameter.AppendSql("  EDUUSERID = :EDUUSERID,                                                   ");
            parameter.AppendSql("  EDUUSERNAME = :EDUUSERNAME                                                ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("EDUDATE", dto.EDUDATE);
            parameter.Add("EDUTYPE", dto.EDUTYPE);
            parameter.Add("TARGET", dto.TARGET);
            parameter.Add("TITLE", dto.TITLE);
            parameter.Add("LOCATION", dto.LOCATION);
            parameter.Add("EDUUSERID", dto.EDUUSERID);
            parameter.Add("EDUUSERNAME", dto.EDUUSERNAME);
            ExecuteNonQuery(parameter);

        }

        public void Delete(HC_OSHA_VISIT_EDU dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HC_OSHA_VISIT_EDU                                               ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            ExecuteNonQuery(parameter);

        }
    }
}
