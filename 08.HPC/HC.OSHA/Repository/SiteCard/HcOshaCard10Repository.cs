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
    public class HcOshaCard10Repository : BaseRepository
    {
        public HC_OSHA_CARD10 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_CARD10                                               ");
            parameter.AppendSql("WHERE ID = :ID                                                             ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE                                                ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicInfo);

            return ExecuteReaderSingle<HC_OSHA_CARD10>(parameter);

        }
        public List<HC_OSHA_CARD10> FindAll(long siteId, string year)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD10 A                          ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                          ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                          ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                          ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                          ");
            parameter.AppendSql("WHERE A.SITE_ID = :SITE_ID                                   ");
            parameter.AppendSql("  AND A.YEAR = :YEAR                              ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE                   ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE                   ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE                   ");
            //    parameter.AppendSql("ORDER BY PUBLISHDATE                                 ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("YEAR", year);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicInfo);

            return ExecuteReader<HC_OSHA_CARD10>(parameter);

        }
        public HC_OSHA_CARD10 Insert(HC_OSHA_CARD10 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD10                                                  ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  SWLICENSE,                                                                ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                                  ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  YEAR,                                                              ");
            parameter.AppendSql("  PUBLISHDATE,                                                              ");
            parameter.AppendSql("  GOAL,                                                                     ");
            parameter.AppendSql("  COMPLETE,                                                                 ");
            parameter.AppendSql("  STATUS,                                                                   ");
            parameter.AppendSql("  MODIFIED,                                                                 ");
            parameter.AppendSql("  MODIFIEDUSER,                                                             ");
            parameter.AppendSql("  CREATED,                                                                  ");
            parameter.AppendSql("  CREATEDUSER                                                               ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :SWLICENSE,                                                               ");
            parameter.AppendSql("  :ID,                                                                      ");
            parameter.AppendSql("  :SITE_ID,                                                                 ");
            parameter.AppendSql("  :ESTIMATE_ID,                                                             ");
            parameter.AppendSql("  :YEAR,                                                             ");
            parameter.AppendSql("  :PUBLISHDATE,                                                             ");
            parameter.AppendSql("  :GOAL,                                                                    ");
            parameter.AppendSql("  :COMPLETE,                                                                ");
            parameter.AppendSql("  :STATUS,                                                                  ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                            ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                 ");
            parameter.AppendSql("  :CREATEDUSER                                                            ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("ID", dto.ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("PUBLISHDATE", dto.PUBLISHDATE);
            parameter.Add("GOAL", dto.GOAL);
            parameter.Add("COMPLETE", dto.COMPLETE);
            parameter.Add("STATUS", dto.STATUS);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicInfo);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_CARD10", dto.ID);
            return FindOne(dto.ID);
        }

        public void Update(HC_OSHA_CARD10 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD10                                                       ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  PUBLISHDATE = :PUBLISHDATE,                                               ");
            parameter.AppendSql("  GOAL = :GOAL,                                                             ");
            parameter.AppendSql("  YEAR = :YEAR,                                                             ");
            parameter.AppendSql("  COMPLETE = :COMPLETE,                                                     ");
            parameter.AppendSql("  STATUS = :STATUS,                                                         ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                     ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                             ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE                                                ");

            parameter.Add("ID", dto.ID);
            parameter.Add("PUBLISHDATE", dto.PUBLISHDATE);
            parameter.Add("GOAL", dto.GOAL);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("COMPLETE", dto.COMPLETE);
            parameter.Add("STATUS", dto.STATUS);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicInfo);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD10", dto.ID);


        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD10                                                   ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE                                                ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicInfo);
            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_CARD10", id);
        }
    }
}
