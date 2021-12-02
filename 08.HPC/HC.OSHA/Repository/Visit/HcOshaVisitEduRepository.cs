namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.OSHA.Dto;
    using HC_Core.Service;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaVisitEduRepository : BaseRepository
    {
        public HC_OSHA_VISIT_EDU FindOne(long id)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT_EDU");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.Add("ID", id);
            return ExecuteReaderSingle<HC_OSHA_VISIT_EDU>(parameter);

        }
        public List<HC_OSHA_VISIT_EDU> FindAll(string startDate, string endDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.EDUDATE, B.NAME, C.CODENAME AS EDUTYPE,  A.TITLE, A.TARGET, A.LOCATION, D.NAME AS EDUUSERNAME        ");
            parameter.AppendSql("FROM HIC_OSHA_VISIT_EDU A                                                                                      ");
            parameter.AppendSql("INNER JOIN HIC_SITE_VIEW B                                                                                     ");
            parameter.AppendSql("ON A.SITE_ID = B.ID                                                                                           ");
            parameter.AppendSql("INNER JOIN HIC_CODES C                                                                                          ");
            parameter.AppendSql("ON A.EDUTYPE = C.CODE                                                                                         ");
            parameter.AppendSql("AND C.GROUPCODE = 'VISIT_EDU_TYPE'                                                                            ");
            parameter.AppendSql("INNER JOIN HIC_USERS D                                                                                         ");
            parameter.AppendSql("ON A.EDUUSERID = D.USERID                                                                                     ");
            parameter.AppendSql("WHERE  A.EDUDATE BETWEEN :startDate AND  :endDate                                                       ");
            parameter.AppendSql("ORDER BY A.EDUDATE DESC                                                                        ");

            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);

         
            return ExecuteReader<HC_OSHA_VISIT_EDU>(parameter);

        }

        public List<HC_OSHA_VISIT_EDU> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT_EDU A                                               ");
            parameter.AppendSql("WHERE SITE_ID = :siteId   ORDER BY EDUDATE DESC                                           ");

            parameter.Add("siteId", siteId);

            return ExecuteReader<HC_OSHA_VISIT_EDU>(parameter);

        }
        public HC_OSHA_VISIT_EDU Insert(HC_OSHA_VISIT_EDU dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_VISIT_EDU                                               ");
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
            parameter.AppendSql("  :ID,                                                                      ");
            parameter.AppendSql("  :SITE_ID,                                                                 ");
            parameter.AppendSql("  :EDUDATE,                                                                 ");
            parameter.AppendSql("  :EDUTYPE,                                                                 ");
            parameter.AppendSql("  :TARGET,                                                                  ");
            parameter.AppendSql("  :TITLE,                                                                   ");
            parameter.AppendSql("  :LOCATION,                                                                ");
            parameter.AppendSql("  :EDUUSERID,                                                               ");
            parameter.AppendSql("  :EDUUSERNAME                                                             ");
            parameter.AppendSql(")                                                                           ");

            dto.ID = GetSequenceNextVal("HC_OSHA_SHARED_ID_SEQ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("EDUDATE", dto.EDUDATE);
            parameter.Add("EDUTYPE", dto.EDUTYPE);
            parameter.Add("TARGET", dto.TARGET);
            parameter.Add("TITLE", dto.TITLE);
            parameter.Add("LOCATION", dto.LOCATION);
            parameter.Add("EDUUSERID", dto.EDUUSERID);
            parameter.Add("EDUUSERNAME", dto.EDUUSERNAME);

            DataSyncService.Instance.Insert("HIC_OSHA_VISIT_EDU", dto.ID);

            ExecuteNonQuery(parameter);

            return FindOne(dto.ID);
        }

        public void Update(HC_OSHA_VISIT_EDU dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_VISIT_EDU                                                    ");
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

            DataSyncService.Instance.Update("HIC_OSHA_VISIT_EDU", dto.ID);
            ExecuteNonQuery(parameter);

        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_VISIT_EDU                                               ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", id);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_VISIT_EDU", id);


        }
    }
}
