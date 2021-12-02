namespace HC.OSHA.Visit.Schedule.Repository
{
    using ComBase.Mvc;
    using HC.OSHA.Visit.Schedule.Dto;
    using HC.Core.Common.Service;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaScheduleRepository : BaseRepository
    {
        
        public HC_OSHA_SCHEDULE FindById(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS SITE_NAME FROM HC_OSHA_SCHEDULE A           ");
            parameter.AppendSql("INNER JOIN HC_SITE_VIEW B                                                ");
            parameter.AppendSql("ON A.SITE_ID = B.ID                                              ");
            parameter.AppendSql("WHERE A.ID = :ID                                               ");
            parameter.AppendSql("AND A.ISDELETED ='N'                                              ");
            parameter.Add("ID", id);
            return ExecuteReaderSingle<HC_OSHA_SCHEDULE>(parameter);
        }

        public HC_OSHA_SCHEDULE Insert(HC_OSHA_SCHEDULE dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_SCHEDULE_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HC_OSHA_SCHEDULE                                                ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  ESTIMATE_ID,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                                  ");
            parameter.AppendSql("  EVENTSTARTDATETIME,                                                       ");
            parameter.AppendSql("  EVENTENDDATETIME,                                                         ");
            parameter.AppendSql("  VISITRESERVEDATE,                                                         ");
            parameter.AppendSql("  VISITSTARTTIME,                                                           ");
            parameter.AppendSql("  VISITENDTIME,                                                             ");
            parameter.AppendSql("  DEPARTUREDATETIME,                                                        ");
            parameter.AppendSql("  ARRIVALTIME,                                                              ");
            parameter.AppendSql("  VISITUSERNAME,                                                            ");
            parameter.AppendSql("  VISITUSERID,                                                              ");
            parameter.AppendSql("  VISITMANAGERNAME,                                                         ");
            parameter.AppendSql("  VISITMANAGERID,                                                           ");
            parameter.AppendSql("  REMARK,                                                                   ");
            parameter.AppendSql("  INDOCPRINTDATETIME,                                                       ");
            parameter.AppendSql("  OUTDOCPRINTDATETIME,                                                      ");
            parameter.AppendSql("  SENDMAILDATETIME,                                                         ");
            parameter.AppendSql("  ISDELETED,                                                                ");
            parameter.AppendSql("  MODIFIED,                                                                 ");
            parameter.AppendSql("  MODIFIEDUSER,                                                             ");
            parameter.AppendSql("  CREATED,                                                                  ");
            parameter.AppendSql("  CREATEDUSER                                                              ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :ID,                                                                       ");
            parameter.AppendSql("  :ESTIMATE_ID,                                                                       ");
            parameter.AppendSql("  :SITE_ID,                                                                 ");
            parameter.AppendSql("  :EVENTSTARTDATETIME,                                                      ");
            parameter.AppendSql("  :EVENTENDDATETIME,                                                        ");
            parameter.AppendSql("  :VISITRESERVEDATE,                                                        ");
            parameter.AppendSql("  :VISITSTARTTIME,                                                          ");
            parameter.AppendSql("  :VISITENDTIME,                                                            ");
            parameter.AppendSql("  :DEPARTUREDATETIME,                                                       ");
            parameter.AppendSql("  :ARRIVALTIME,                                                             ");
            parameter.AppendSql("  :VISITUSERNAME,                                                           ");
            parameter.AppendSql("  :VISITUSERID,                                                             ");
            parameter.AppendSql("  :VISITMANAGERNAME,                                                        ");
            parameter.AppendSql("  :VISITMANAGERID,                                                          ");
            parameter.AppendSql("  :REMARK,                                                                  ");
            parameter.AppendSql("  :INDOCPRINTDATETIME,                                                      ");
            parameter.AppendSql("  :OUTDOCPRINTDATETIME,                                                     ");
            parameter.AppendSql("  :SENDMAILDATETIME,                                                        ");
            parameter.AppendSql("  'N',                                                                      ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                            ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :CREATEDUSER                                                              ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("ID", dto.ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("EVENTSTARTDATETIME", dto.EVENTSTARTDATETIME);
            parameter.Add("EVENTENDDATETIME", dto.EVENTENDDATETIME);
            parameter.Add("VISITRESERVEDATE", dto.VISITRESERVEDATE);
            parameter.Add("VISITSTARTTIME", dto.VISITSTARTTIME);
            parameter.Add("VISITENDTIME", dto.VISITENDTIME);
            parameter.Add("DEPARTUREDATETIME", dto.DEPARTUREDATETIME);
            parameter.Add("ARRIVALTIME", dto.ARRIVALTIME);
            parameter.Add("VISITUSERNAME", dto.VISITUSERNAME);
            parameter.Add("VISITUSERID", dto.VISITUSERID);
            parameter.Add("VISITMANAGERNAME", dto.VISITMANAGERNAME);
            parameter.Add("VISITMANAGERID", dto.VISITMANAGERID);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("INDOCPRINTDATETIME", dto.INDOCPRINTDATETIME);
            parameter.Add("OUTDOCPRINTDATETIME", dto.OUTDOCPRINTDATETIME);
            parameter.Add("SENDMAILDATETIME", dto.SENDMAILDATETIME);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
          
            ExecuteNonQuery(parameter);

            return FindById(dto.ID);

        }

        public HC_OSHA_SCHEDULE Update(HC_OSHA_SCHEDULE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_OSHA_SCHEDULE                                                     ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  EVENTSTARTDATETIME = :EVENTSTARTDATETIME,                                 ");
            parameter.AppendSql("  EVENTENDDATETIME = :EVENTENDDATETIME,                                     ");
            parameter.AppendSql("  VISITRESERVEDATE = :VISITRESERVEDATE,                                     ");
            parameter.AppendSql("  VISITSTARTTIME = :VISITSTARTTIME,                                         ");
            parameter.AppendSql("  VISITENDTIME = :VISITENDTIME,                                             ");
            parameter.AppendSql("  DEPARTUREDATETIME = :DEPARTUREDATETIME,                                   ");
            parameter.AppendSql("  ARRIVALTIME = :ARRIVALTIME,                                               ");
            parameter.AppendSql("  VISITUSERNAME = :VISITUSERNAME,                                           ");
            parameter.AppendSql("  VISITUSERID = :VISITUSERID,                                               ");
            parameter.AppendSql("  VISITMANAGERNAME = :VISITMANAGERNAME,                                     ");
            parameter.AppendSql("  VISITMANAGERID = :VISITMANAGERID,                                         ");
            parameter.AppendSql("  REMARK = :REMARK,                                                         ");
            parameter.AppendSql("  INDOCPRINTDATETIME = :INDOCPRINTDATETIME,                                 ");
            parameter.AppendSql("  OUTDOCPRINTDATETIME = :OUTDOCPRINTDATETIME,                               ");
            parameter.AppendSql("  SENDMAILDATETIME = :SENDMAILDATETIME,                                     ");
            parameter.AppendSql("  ISDELETED = :ISDELETED,                                                   ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                     ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                             ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            parameter.Add("EVENTSTARTDATETIME", dto.EVENTSTARTDATETIME);
            parameter.Add("EVENTENDDATETIME", dto.EVENTENDDATETIME);
            parameter.Add("VISITRESERVEDATE", dto.VISITRESERVEDATE);
            parameter.Add("VISITSTARTTIME", dto.VISITSTARTTIME);
            parameter.Add("VISITENDTIME", dto.VISITENDTIME);
            parameter.Add("DEPARTUREDATETIME", dto.DEPARTUREDATETIME);
            parameter.Add("ARRIVALTIME", dto.ARRIVALTIME);
            parameter.Add("VISITUSERNAME", dto.VISITUSERNAME);
            parameter.Add("VISITUSERID", dto.VISITUSERID);
            parameter.Add("VISITMANAGERNAME", dto.VISITMANAGERNAME);
            parameter.Add("VISITMANAGERID", dto.VISITMANAGERID);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("INDOCPRINTDATETIME", dto.INDOCPRINTDATETIME);
            parameter.Add("OUTDOCPRINTDATETIME", dto.OUTDOCPRINTDATETIME);
            parameter.Add("SENDMAILDATETIME", dto.SENDMAILDATETIME);
            parameter.Add("ISDELETED", dto.ISDELETED);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);
            return FindById(dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_OSHA_SCHEDULE                                                     ");
            parameter.AppendSql("   SET ISDELETED = 'Y'                                                      ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");

            parameter.Add("ID", id);

            ExecuteNonQuery(parameter);
        }
    }
}
