namespace HC.OSHA.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using HC.Core.Service;
    using HC.OSHA.Dto;
    using HC_Core.Service;

    /// <summary>
    /// 
    /// </summary>
    public class HcOshaScheduleRepository : BaseRepository
    {
        public List<HC_OSHA_SCHEDULE> FindAll(long siteId, string startDate, string endDate, string visitUserid, string visitUserid2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, A.VISITSTARTTIME || '~'|| A.VISITENDTIME AS VISITPERIOD ,");
            parameter.AppendSql("       B.NAME AS SITE_NAME, C.Id AS VISIT_ID ");
            parameter.AppendSql("  FROM HIC_OSHA_SCHEDULE A ");
            parameter.AppendSql("       INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("             ON A.SITE_ID = B.ID ");
            parameter.AppendSql("             AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("       LEFT  OUTER JOIN HIC_OSHA_VISIT C ");
            parameter.AppendSql("             ON A.ID = C.SCHEDULE_ID ");
            parameter.AppendSql("             AND C.ISDELETED ='N' ");
            parameter.AppendSql("             AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" WHERE A.ISDELETED ='N' ");
            parameter.AppendSql("   AND A.VISITRESERVEDATE >= TO_DATE(:startDate,'YYYY-MM-DD') ");
            parameter.AppendSql("   AND A.VISITRESERVEDATE <= TO_DATE(:endDate,'YYYY-MM-DD')");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            if (siteId > 0)
            {
                parameter.AppendSql("AND A.SITE_ID = :SITEID ");
            }
            if (!visitUserid.IsNullOrEmpty())
            {
                parameter.AppendSql("AND A.VISITUSERID = :visitUserid ");
            }
            if (!visitUserid2.IsNullOrEmpty())
            {
                parameter.AppendSql("AND A.VISITMANAGERID = :visitUserid2 ");
            }
            parameter.AppendSql("ORDER BY A.visitreservedate,  A.VISITSTARTTIME ");

            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);

            if (siteId > 0)
            {
                parameter.Add("SITEID", siteId);
            }

            if (!visitUserid.IsNullOrEmpty())
            {
                parameter.Add("visitUserid", visitUserid);
            }
            if (!visitUserid2.IsNullOrEmpty())
            {
                parameter.Add("visitUserid2", visitUserid2);
            }
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_SCHEDULE>(parameter);
        }

        public List<HC_OSHA_SCHEDULE> FindAll( string startDate, string endDate, long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, A.VISITSTARTTIME || '~'|| A.VISITENDTIME AS VISITPERIOD,");
            parameter.AppendSql("       B.NAME AS SITE_NAME ");
            parameter.AppendSql("  FROM HIC_OSHA_SCHEDULE A ");
            parameter.AppendSql("       INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("             ON A.SITE_ID = B.ID ");
            parameter.AppendSql("             AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" WHERE A.ISDELETED ='N' ");
            parameter.AppendSql("   AND A.VISITRESERVEDATE >= :startDate ");
            parameter.AppendSql("   AND A.VISITRESERVEDATE <= :endDate ");
            parameter.AppendSql("   AND A.SITE_ID = :SITEID ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("ORDER BY A.visitreservedate,  A.VISITSTARTTIME ");
            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            if (siteId > 0)
            {
                parameter.Add("SITEID", siteId);
            }

            return ExecuteReader<HC_OSHA_SCHEDULE>(parameter);
        }
        public HC_OSHA_SCHEDULE FindById(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS SITE_NAME ");
            parameter.AppendSql("  FROM HIC_OSHA_SCHEDULE A ");
            parameter.AppendSql("       INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("             ON A.SITE_ID = B.ID ");
            parameter.AppendSql("             AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" WHERE A.ID = :ID ");
            parameter.AppendSql("   AND A.ISDELETED ='N' ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            
            return ExecuteReaderSingle<HC_OSHA_SCHEDULE>(parameter);
        }
   
        public HC_OSHA_SCHEDULE Insert(HC_OSHA_SCHEDULE dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_SCHEDULE_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_SCHEDULE    ");
            parameter.AppendSql("(                                ");
            parameter.AppendSql("  ID,                            ");
            parameter.AppendSql("  ESTIMATE_ID,                   ");
            parameter.AppendSql("  SITE_ID,                       ");
            parameter.AppendSql("  EVENTSTARTDATETIME,            ");
            parameter.AppendSql("  EVENTENDDATETIME,              ");
            parameter.AppendSql("  VISITRESERVEDATE,              ");
            parameter.AppendSql("  VISITSTARTTIME,                ");
            parameter.AppendSql("  VISITUSERNAME,                 ");
            parameter.AppendSql("  VISITUSERID,                   ");
            parameter.AppendSql("  VISITMANAGERNAME,              ");
            parameter.AppendSql("  VISITMANAGERID,                ");
            parameter.AppendSql("  REMARK,                        ");
            parameter.AppendSql("  INDOCPRINTDATETIME,            ");
            parameter.AppendSql("  OUTDOCPRINTDATETIME,           ");
            parameter.AppendSql("  GBCHANGE,                      ");
            parameter.AppendSql("  WORKERCOUNT,                   ");
            parameter.AppendSql("  ISDELETED,                     ");
            parameter.AppendSql("  MODIFIED,                      ");
            parameter.AppendSql("  MODIFIEDUSER,                  ");
            parameter.AppendSql("  CREATED,                       ");
            parameter.AppendSql("  CREATEDUSER,                   ");
            parameter.AppendSql("  SWLICENSE                      ");
            parameter.AppendSql(")                                ");
            parameter.AppendSql("VALUES                           ");
            parameter.AppendSql("(                                ");
            parameter.AppendSql("  :ID,                           ");
            parameter.AppendSql("  :ESTIMATE_ID,                  ");
            parameter.AppendSql("  :SITE_ID,                      ");
            parameter.AppendSql("  :EVENTSTARTDATETIME,           ");
            parameter.AppendSql("  :EVENTENDDATETIME,             ");
            parameter.AppendSql("  :VISITRESERVEDATE,             ");
            parameter.AppendSql("  :VISITSTARTTIME,               ");
            parameter.AppendSql("  :VISITUSERNAME,                ");
            parameter.AppendSql("  :VISITUSERID,                  ");
            parameter.AppendSql("  :VISITMANAGERNAME,             ");
            parameter.AppendSql("  :VISITMANAGERID,               ");
            parameter.AppendSql("  :REMARK,                       ");
            parameter.AppendSql("  :INDOCPRINTDATETIME,           ");
            parameter.AppendSql("  :OUTDOCPRINTDATETIME,          ");
            parameter.AppendSql("  :GBCHANGE,                     ");
            parameter.AppendSql("  :WORKERCOUNT,                  ");
            parameter.AppendSql("  'N',                           ");
            parameter.AppendSql("  SYSTIMESTAMP,                  ");
            parameter.AppendSql("  :MODIFIEDUSER,                 ");
            parameter.AppendSql("  SYSTIMESTAMP,                  ");
            parameter.AppendSql("  :CREATEDUSER,                  ");
            parameter.AppendSql("  :SWLICENSE                     ");
            parameter.AppendSql(")                                ");
            parameter.Add("ID", dto.ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("EVENTSTARTDATETIME", dto.EVENTSTARTDATETIME);
            parameter.Add("EVENTENDDATETIME", dto.EVENTENDDATETIME);
            parameter.Add("VISITRESERVEDATE", dto.VISITRESERVEDATE);
            parameter.Add("VISITSTARTTIME", dto.VISITSTARTTIME);
            parameter.Add("VISITUSERNAME", dto.VISITUSERNAME);
            parameter.Add("VISITUSERID", dto.VISITUSERID);
            parameter.Add("VISITMANAGERNAME", dto.VISITMANAGERNAME);
            parameter.Add("VISITMANAGERID", dto.VISITMANAGERID);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("GBCHANGE", dto.GBCHANGE);
            parameter.Add("WORKERCOUNT", dto.WORKERCOUNT);
            parameter.Add("INDOCPRINTDATETIME", dto.INDOCPRINTDATETIME);
            parameter.Add("OUTDOCPRINTDATETIME", dto.OUTDOCPRINTDATETIME);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_SCHEDULE", dto.ID);
            return FindById(dto.ID);

        }

        public HC_OSHA_SCHEDULE Update(HC_OSHA_SCHEDULE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_SCHEDULE ");
            parameter.AppendSql("SET ");
            parameter.AppendSql("  EVENTSTARTDATETIME = :EVENTSTARTDATETIME, ");
            parameter.AppendSql("  EVENTENDDATETIME = :EVENTENDDATETIME, ");
            parameter.AppendSql("  VISITRESERVEDATE = :VISITRESERVEDATE,");
            parameter.AppendSql("  VISITSTARTTIME = :VISITSTARTTIME,");
            parameter.AppendSql("  VISITUSERNAME = :VISITUSERNAME,");
            parameter.AppendSql("  VISITUSERID = :VISITUSERID,");
            parameter.AppendSql("  VISITMANAGERNAME = :VISITMANAGERNAME,");
            parameter.AppendSql("  VISITMANAGERID = :VISITMANAGERID,");
            parameter.AppendSql("  REMARK = :REMARK,");
            parameter.AppendSql("  INDOCPRINTDATETIME = :INDOCPRINTDATETIME,");
            parameter.AppendSql("  OUTDOCPRINTDATETIME = :OUTDOCPRINTDATETIME,");
            parameter.AppendSql("  GBCHANGE = :GBCHANGE,");
            parameter.AppendSql("  WORKERCOUNT = :WORKERCOUNT,");
            parameter.AppendSql("  ISDELETED = :ISDELETED,");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER ");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("EVENTSTARTDATETIME", dto.EVENTSTARTDATETIME);
            parameter.Add("EVENTENDDATETIME", dto.EVENTENDDATETIME);
            parameter.Add("VISITRESERVEDATE", dto.VISITRESERVEDATE);
            parameter.Add("VISITSTARTTIME", dto.VISITSTARTTIME);
            parameter.Add("VISITUSERNAME", dto.VISITUSERNAME);
            parameter.Add("VISITUSERID", dto.VISITUSERID);
            parameter.Add("VISITMANAGERNAME", dto.VISITMANAGERNAME);
            parameter.Add("VISITMANAGERID", dto.VISITMANAGERID);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("INDOCPRINTDATETIME", dto.INDOCPRINTDATETIME);
            parameter.Add("OUTDOCPRINTDATETIME", dto.OUTDOCPRINTDATETIME);
            parameter.Add("GBCHANGE", dto.GBCHANGE);
            parameter.Add("WORKERCOUNT", dto.WORKERCOUNT);
            parameter.Add("ISDELETED", dto.ISDELETED);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            return FindById(dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_SCHEDULE ");
            parameter.AppendSql("   SET ISDELETED = 'Y' ");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
        }

    }
}
