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
    public class HcOshaCard94Repository : BaseRepository
    {

        public HC_OSHA_CARD9_4 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD9_4 A                          ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                               ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                         ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                               ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                         ");
            parameter.AppendSql("WHERE ID = :ID");

            parameter.Add("ID", id);

            return ExecuteReaderSingle<HC_OSHA_CARD9_4>(parameter);
        }
        public List<HC_OSHA_CARD9_4> FindAll(long siteId, string yyyy_mm_dd)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD9_4 A                          ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                               ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                         ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                               ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                         ");
            parameter.AppendSql("WHERE SITE_ID = :SITE_ID AND EndDate <= :EndDate   ORDER BY STARTDATE             ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("EndDate", yyyy_mm_dd);

            return ExecuteReader<HC_OSHA_CARD9_4>(parameter);

        }
        public HC_OSHA_CARD9_4 Insert(HC_OSHA_CARD9_4 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD9_4                                                 ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                                  ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  STARTDATE,                                                                ");
            parameter.AppendSql("  ENDDATE,                                                                  ");
            parameter.AppendSql("  NAME,                                                                     ");
            parameter.AppendSql("  CONTENT,                                                                  ");
            parameter.AppendSql("  REMARK,                                                                   ");
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
            parameter.AppendSql("  :STARTDATE,                                                               ");
            parameter.AppendSql("  :ENDDATE,                                                                 ");
            parameter.AppendSql("  :NAME,                                                                    ");
            parameter.AppendSql("  :CONTENT,                                                                 ");
            parameter.AppendSql("  :REMARK,                                                                  ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                            ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                 ");
            parameter.AppendSql("  :CREATEDUSER                                                            ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("STARTDATE", dto.STARTDATE);
            parameter.Add("ENDDATE", dto.ENDDATE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("CONTENT", dto.CONTENT);
            parameter.Add("REMARK", dto.REMARK);

            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_CARD9_4", dto.ID);

            return FindOne(dto.ID);
        }

        public HC_OSHA_CARD9_4 Update(HC_OSHA_CARD9_4 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD9_4                                                      ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  STARTDATE = :STARTDATE,                                                   ");
            parameter.AppendSql("  ENDDATE = :ENDDATE,                                                       ");
            parameter.AppendSql("  NAME = :NAME,                                                             ");
            parameter.AppendSql("  CONTENT = :CONTENT,                                                       ");
            parameter.AppendSql("  REMARK = :REMARK,                                                         ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                     ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                             ");

            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            parameter.Add("STARTDATE", dto.STARTDATE);
            parameter.Add("ENDDATE", dto.ENDDATE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("CONTENT", dto.CONTENT);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD9_4", dto.ID);
            return FindOne(dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD9_4                                                   ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", id);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_CARD9_4", id);
        }
    }
}