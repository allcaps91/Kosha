namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.Core.Service;
    using HC.OSHA.Dto;
    using HC_Core.Service;


    /// <summary>
    /// ????
    /// </summary>
    public class HcOshaVisitRepository : BaseRepository
    {

        public List<HC_OSHA_VISIT> FindLastVisiDate( long siteId, string lastDate, string userId, Role role)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT                       ");
            parameter.AppendSql("WHERE                                                ");
            parameter.AppendSql("ISDELETED ='N'  AND ISPRECHARGE = 'N'                  ");
            parameter.AppendSql("AND VISITDATETIME >= :lastDate                            ");
            parameter.AppendSql("AND SITE_ID = :SITEID                                ");
            if (role == Role.DOCTOR)
            {
                parameter.AppendSql("AND VISITDOCTOR = :VISITUSER                                ");
            }
            else
            {
                parameter.AppendSql("AND VISITUSER = :VISITUSER                                ");
            }
                
            parameter.AppendSql("ORDER BY VISITDATETIME                                        ");
           
            parameter.Add("lastDate", lastDate);
            parameter.Add("SITEID", siteId);
            parameter.Add("VISITUSER", userId);

            return ExecuteReader<HC_OSHA_VISIT>(parameter);
        }

        public HC_OSHA_VISIT FindById(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT                                                   ");
            parameter.AppendSql("WHERE ID = :ID  AND ISDELETED = 'N'                                           ");
            parameter.Add("ID", id);

            return ExecuteReaderSingle<HC_OSHA_VISIT>(parameter);

        }
        public HC_OSHA_VISIT FindByScheduleId(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT                                                   ");
            parameter.AppendSql("WHERE SCHEDULE_ID = :ID  AND ISDELETED = 'N'                                           ");
            parameter.Add("ID", id);

            return ExecuteReaderSingle<HC_OSHA_VISIT>(parameter);

        }
        public List<HC_OSHA_VISIT> FindAllByEstimate(long estimateId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT                                                   ");
            parameter.AppendSql("WHERE ESTIMATE_ID = :ESTIMATE_ID  AND ISDELETED = 'N'                                           ");
            parameter.Add("ESTIMATE_ID", estimateId);

            return ExecuteReader<HC_OSHA_VISIT>(parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_VISIT Insert(HC_OSHA_VISIT dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_VISIT_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_VISIT                                                   ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SCHEDULE_ID,                                                              ");
            parameter.AppendSql("  SITE_ID,                                                              ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  VISITDATETIME,                                                            ");
            parameter.AppendSql("  VISITTYPE,                                                                ");
            parameter.AppendSql("  STARTTIME,                                                                ");
            parameter.AppendSql("  ENDTIME,                                                                  ");
            parameter.AppendSql("  TAKEHOUR,                                                                 ");
            parameter.AppendSql("  TAKEMINUTE,                                                               ");
            parameter.AppendSql("  VISITUSERNAME,                                                            ");
            parameter.AppendSql("  VISITUSER,                                                                ");
            parameter.AppendSql("  VISITDOCTORNAME,                                                          ");
            parameter.AppendSql("  VISITDOCTOR,                                                              ");
            parameter.AppendSql("  ISFEE,                                                                    ");
            parameter.AppendSql("  ISPRECHARGE,                                                                    ");
            parameter.AppendSql("  ISKUKGO,                                                                  ");
            parameter.AppendSql("  REMARK,                                                                  ");
            parameter.AppendSql("  ISDELETED,                                                                ");
            parameter.AppendSql("  MODIFIED,                                                                 ");
            parameter.AppendSql("  MODIFIEDUSER,                                                             ");
            parameter.AppendSql("  CREATED,                                                                  ");
            parameter.AppendSql("  CREATEDUSER                                                              ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :ID,                                                                      ");
            parameter.AppendSql("  :SCHEDULE_ID,                                                             ");
            parameter.AppendSql("  :SITE_ID,                                                             ");
            parameter.AppendSql("  :ESTIMATE_ID,                                                             ");
            parameter.AppendSql("  :VISITDATETIME,                                                           ");
            parameter.AppendSql("  :VISITTYPE,                                                               ");
            parameter.AppendSql("  :STARTTIME,                                                               ");
            parameter.AppendSql("  :ENDTIME,                                                                 ");
            parameter.AppendSql("  :TAKEHOUR,                                                                ");
            parameter.AppendSql("  :TAKEMINUTE,                                                              ");
            parameter.AppendSql("  :VISITUSERNAME,                                                           ");
            parameter.AppendSql("  :VISITUSER,                                                               ");
            parameter.AppendSql("  :VISITDOCTORNAME,                                                         ");
            parameter.AppendSql("  :VISITDOCTOR,                                                             ");
            parameter.AppendSql("  :ISFEE,                                                                   ");
            parameter.AppendSql("  :ISPRECHARGE,                                                                   ");
            parameter.AppendSql("  :ISKUKGO,                                                                 ");
            parameter.AppendSql("  :REMARK,                                                                  ");
            parameter.AppendSql("  'N',                                                                      ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                            ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :CREATEDUSER                                                              ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SCHEDULE_ID", dto.SCHEDULE_ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("VISITDATETIME", dto.VISITDATETIME);
            parameter.Add("VISITTYPE", dto.VISITTYPE);
            parameter.Add("STARTTIME", dto.STARTTIME);
            parameter.Add("ENDTIME", dto.ENDTIME);
            parameter.Add("TAKEHOUR", dto.TAKEHOUR);
            parameter.Add("TAKEMINUTE", dto.TAKEMINUTE);
            parameter.Add("VISITUSERNAME", dto.VISITUSERNAME);
            parameter.Add("VISITUSER", dto.VISITUSER);
            parameter.Add("VISITDOCTORNAME", dto.VISITDOCTORNAME);
            parameter.Add("VISITDOCTOR", dto.VISITDOCTOR);
            parameter.Add("ISFEE", dto.ISFEE);
            parameter.Add("ISPRECHARGE", dto.ISPRECHARGE);
            
            parameter.Add("ISKUKGO", dto.ISKUKGO);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Insert("HIC_OSHA_VISIT", dto.ID);
            return FindById(dto.ID);
        }

        public HC_OSHA_VISIT Update(HC_OSHA_VISIT dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_VISIT                                                        ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  VISITDATETIME = :VISITDATETIME,                                           ");
            parameter.AppendSql("  VISITTYPE = :VISITTYPE,                                                   ");
            parameter.AppendSql("  STARTTIME = :STARTTIME,                                                   ");
            parameter.AppendSql("  ENDTIME = :ENDTIME,                                                       ");
            parameter.AppendSql("  TAKEHOUR = :TAKEHOUR,                                                     ");
            parameter.AppendSql("  TAKEMINUTE = :TAKEMINUTE,                                                 ");
            parameter.AppendSql("  VISITUSERNAME = :VISITUSERNAME,                                           ");
            parameter.AppendSql("  VISITUSER = :VISITUSER,                                                   ");
            parameter.AppendSql("  VISITDOCTORNAME = :VISITDOCTORNAME,                                       ");
            parameter.AppendSql("  VISITDOCTOR = :VISITDOCTOR,                                               ");
            parameter.AppendSql("  ISFEE = :ISFEE,                                                           ");
            parameter.AppendSql("  ISPRECHARGE = :ISPRECHARGE,                                                           ");
            parameter.AppendSql("  ISKUKGO = :ISKUKGO,                                                       ");
            parameter.AppendSql("  REMARK = :REMARK,                                                         ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                  ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                              ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            parameter.Add("VISITDATETIME", dto.VISITDATETIME);
            parameter.Add("VISITTYPE", dto.VISITTYPE);
            parameter.Add("STARTTIME", dto.STARTTIME);
            parameter.Add("ENDTIME", dto.ENDTIME);
            parameter.Add("TAKEHOUR", dto.TAKEHOUR);
            parameter.Add("TAKEMINUTE", dto.TAKEMINUTE);
            parameter.Add("VISITUSERNAME", dto.VISITUSERNAME);
            parameter.Add("VISITUSER", dto.VISITUSER);
            parameter.Add("VISITDOCTORNAME", dto.VISITDOCTORNAME);
            parameter.Add("VISITDOCTOR", dto.VISITDOCTOR);
            parameter.Add("ISFEE", dto.ISFEE);
            parameter.Add("ISPRECHARGE", dto.ISPRECHARGE);
            parameter.Add("ISKUKGO", dto.ISKUKGO);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Update("HIC_OSHA_VISIT", dto.ID);

            return FindById(dto.ID);
        }

        public void Delete(HC_OSHA_VISIT dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_VISIT                                                        ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  ISDELETED = :ISDELETED,                                                                 ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                               ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");

            parameter.Add("ISDELETED", dto.ISDELETED);
            parameter.Add("MODIFIEDUSER", dto.MODIFIEDUSER);
            parameter.Add("ID", dto.ID);


            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_VISIT", dto.ID);

        }
    }
}
