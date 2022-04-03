namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using HC.Core.Service;
    using HC.OSHA.Dto;
    using HC_Core.Service;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard4_2Repository : BaseRepository
    {

        public List<HC_OSHA_CARD4_2> FindByEstimateId(long estimateId, string year)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER ");
            parameter.AppendSql("  FROM HIC_OSHA_CARD4_2 A ");
            parameter.AppendSql("       INNER JOIN HIC_USERS B ");
            parameter.AppendSql("             ON  A.CREATEDUSER = B.USERID ");
            parameter.AppendSql("             AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("       INNER JOIN HIC_USERS C ");
            parameter.AppendSql("             ON  A.MODIFIEDUSER = C.USERID ");
            parameter.AppendSql("             AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" WHERE A.ESTIMATE_ID = :ESTIMATE_ID ");
            parameter.AppendSql("   AND A.YEAR = :YEAR ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("ORDER BY RANK,TASK ");

            parameter.Add("ESTIMATE_ID", estimateId);
            parameter.Add("YEAR", year);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD4_2>(parameter);

        }

        public HC_OSHA_CARD4_2 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER ");
            parameter.AppendSql("  FROM HIC_OSHA_CARD4_2 A ");
            parameter.AppendSql("       INNER JOIN HIC_USERS B ");
            parameter.AppendSql("             ON  A.CREATEDUSER = B.USERID ");
            parameter.AppendSql("             AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("       INNER JOIN HIC_USERS C ");
            parameter.AppendSql("             ON  A.MODIFIEDUSER = C.USERID ");
            parameter.AppendSql("             AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_CARD4_2>(parameter);

        }
       

        public HC_OSHA_CARD4_2 Insert(HC_OSHA_CARD4_2 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD4_2      ");
            parameter.AppendSql("(                                 ");
            parameter.AppendSql("  SWLICENSE,                      ");
            parameter.AppendSql("  ID,                             ");
            parameter.AppendSql("  ESTIMATE_ID,                    ");
            parameter.AppendSql("  SITE_ID,                        ");
            parameter.AppendSql("  YEAR,                           ");
            parameter.AppendSql("  RANK,                           ");
            parameter.AppendSql("  TASK,                           ");
            parameter.AppendSql("  TASKUNIT,                       ");
            parameter.AppendSql("  MSDS,                           ");
            parameter.AppendSql("  WORKERCOUNT,                    ");
            parameter.AppendSql("  REMARK,                         ");
            parameter.AppendSql("  WORKDESC,                       ");
            parameter.AppendSql("  MODIFIED,                       ");
            parameter.AppendSql("  MODIFIEDUSER,                   ");
            parameter.AppendSql("  CREATED,                        ");
            parameter.AppendSql("  CREATEDUSER                     ");
            parameter.AppendSql(")                                 ");
            parameter.AppendSql("VALUES                            ");
            parameter.AppendSql("(                                 ");
            parameter.AppendSql("  :SWLICENSE,                     ");
            parameter.AppendSql("  :ID,                            ");
            parameter.AppendSql("  :ESTIMATE_ID,                   ");
            parameter.AppendSql("  :SITE_ID,                       ");
            parameter.AppendSql("  :YEAR,                          ");
            parameter.AppendSql("  :RANK,                          ");
            parameter.AppendSql("  :TASK,                          ");
            parameter.AppendSql("  :TASKUNIT,                      ");
            parameter.AppendSql("  :MSDS,                          ");
            parameter.AppendSql("  :WORKERCOUNT,                   ");
            parameter.AppendSql("  :REMARK,                        ");
            parameter.AppendSql("  :WORKDESC,                      ");
            parameter.AppendSql("  SYSTIMESTAMP,                   ");
            parameter.AppendSql("  :MODIFIEDUSER,                  ");
            parameter.AppendSql("  SYSTIMESTAMP,                   ");
            parameter.AppendSql("  :CREATEDUSER                    ");
            parameter.AppendSql(")                                 ");
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            parameter.Add("ID", dto.ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("RANK", dto.RANK);
            parameter.Add("TASK", dto.TASK);
            parameter.Add("TASKUNIT", dto.TASKUNIT);
            parameter.Add("MSDS", dto.MSDS);
            parameter.Add("WORKERCOUNT", dto.WORKERCOUNT);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("WORKDESC", dto.WORKDESC);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Insert("HIC_OSHA_CARD4_2", dto.ID);

            return FindOne(dto.ID);
        }

        public HC_OSHA_CARD4_2 Update(HC_OSHA_CARD4_2 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD4_2              ");
            parameter.AppendSql("SET                                  ");
            parameter.AppendSql("  TASK = :TASK,                      ");
            parameter.AppendSql("  RANK = :RANK,                      ");
            parameter.AppendSql("  YEAR = :YEAR,                      ");
            parameter.AppendSql("  TASKUNIT = :TASKUNIT,              ");
            parameter.AppendSql("  MSDS = :MSDS,                      ");
            parameter.AppendSql("  WORKERCOUNT = :WORKERCOUNT,        ");
            parameter.AppendSql("  REMARK = :REMARK,                  ");
            parameter.AppendSql("  WORKDESC = :WORKDESC,              ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,           ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER       ");
            parameter.AppendSql("WHERE ID = :ID                       ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE         ");

            parameter.Add("ID", dto.ID);
            parameter.Add("RANK", dto.RANK);
            parameter.Add("TASK", dto.TASK);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("TASKUNIT", dto.TASKUNIT);
            parameter.Add("MSDS", dto.MSDS);
            parameter.Add("WORKERCOUNT", dto.WORKERCOUNT);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("WORKDESC", dto.WORKDESC);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Update("HIC_OSHA_CARD4_2", dto.ID);
            return FindOne(dto.ID);

        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD4_2        ");
            parameter.AppendSql("WHERE ID = :ID                      ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE        ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_CARD4_2", id);

        }
    }
}
