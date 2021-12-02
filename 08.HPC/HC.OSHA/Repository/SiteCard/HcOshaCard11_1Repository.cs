namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.Core.Service;
    using HC.OSHA.Dto;
    using HC_Core.Service;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard11_1Repository : BaseRepository
    {

        public HC_OSHA_CARD11_1 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD11_1 A                          ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                               ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                         ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                               ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                         ");
            parameter.AppendSql("WHERE ID = :ID");

            parameter.Add("ID", id);

            return ExecuteReaderSingle<HC_OSHA_CARD11_1>(parameter);
        }
        public List<HC_OSHA_CARD11_1> FindAll(long siteId, string YEAR)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD11_1 A                          ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                               ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                         ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                               ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                         ");
            parameter.AppendSql("WHERE A.SITE_ID = :SITE_ID AND A.YEAR = :YEAR   ORDER BY PUBLISHDATE DESC            ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("YEAR", YEAR);

            return ExecuteReader<HC_OSHA_CARD11_1>(parameter);

        }
        public HC_OSHA_CARD11_1 Insert(HC_OSHA_CARD11_1 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD11_1                                                ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                                  ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  YEAR,                                                              ");
            parameter.AppendSql("  PUBLISHDATE,                                                              ");
            parameter.AppendSql("  ACTDATE,                                                                  ");
            parameter.AppendSql("  STATUS,                                                                   ");
            parameter.AppendSql("  CONTENT,                                                                  ");
            parameter.AppendSql("  MODIFIED,                                                                 ");
            parameter.AppendSql("  MODIFIEDUSER,                                                             ");
            parameter.AppendSql("  CREATED,                                                                  ");
            parameter.AppendSql("  CREATEDUSER                                                              ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :ID,                                                                      ");
            parameter.AppendSql("  :SITE_ID,                                                                 ");
            parameter.AppendSql("  :ESTIMATE_ID,                                                             ");
            parameter.AppendSql("  :YEAR,                                                             ");
            parameter.AppendSql("  :PUBLISHDATE,                                                             ");
            parameter.AppendSql("  :ACTDATE,                                                                 ");
            parameter.AppendSql("  :STATUS,                                                                  ");
            parameter.AppendSql("  :CONTENT,                                                                 ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                            ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                 ");
            parameter.AppendSql("  :CREATEDUSER                                                            ");
            parameter.AppendSql(")                                                                              "); 

            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("PUBLISHDATE", dto.PUBLISHDATE);
            parameter.Add("ACTDATE", dto.ACTDATE);
            parameter.Add("STATUS", dto.STATUS);
            parameter.Add("CONTENT", dto.CONTENT);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_CARD11_1", dto.ID);
            return FindOne(dto.ID);
        }

        public HC_OSHA_CARD11_1 Update(HC_OSHA_CARD11_1 dto)
        {

            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD11_1                                                     ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  PUBLISHDATE = :PUBLISHDATE,                                               ");
            parameter.AppendSql("  ACTDATE = :ACTDATE,                                                       ");
            parameter.AppendSql("  YEAR = :YEAR,                                                       ");
            parameter.AppendSql("  STATUS = :STATUS,                                                         ");
            parameter.AppendSql("  CONTENT = :CONTENT,                                                       ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                     ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                             ");

            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            parameter.Add("PUBLISHDATE", dto.PUBLISHDATE);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("ACTDATE", dto.ACTDATE);
            parameter.Add("STATUS", dto.STATUS);
            parameter.Add("CONTENT", dto.CONTENT);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Update("HIC_OSHA_CARD11_1", dto.ID);
            return FindOne(dto.ID);
            

        
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD11_1                                                   ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", id);
            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_CARD11_1", id);
        }
    }
}
