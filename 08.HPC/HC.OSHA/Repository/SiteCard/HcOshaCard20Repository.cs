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
    public class HcOshaCard20Repository : BaseRepository
    {

        public HC_OSHA_CARD20 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD20 A   ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                           ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                          ");
            parameter.AppendSql("WHERE A.ID = :ID");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE1        ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE2        ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE3        ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE3", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_CARD20>(parameter);
        }
        public List<HC_OSHA_CARD20> FindAll(long estimateId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD20 A   ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                             ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                        ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                             ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                       ");
            parameter.AppendSql("WHERE A.ESTIMATE_ID = :ESTIMATE_ID     ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE1        ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE2        ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE3        ");
            parameter.AppendSql("ORDER BY YEAR DESC                     ");
            parameter.Add("ESTIMATE_ID", estimateId);
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE3", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD20>(parameter);

        }
        public List<HC_OSHA_CARD20> FindAllByYear(string year, long site_Id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD20 A   ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                             ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                        ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                             ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                       ");
            parameter.AppendSql("WHERE A.YEAR = :YEAR                   ");
            parameter.AppendSql("  AND A.SITE_ID = :SITE_ID             ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE1        ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE2        ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE3        ");
            parameter.AppendSql("ORDER BY ID                            ");
            parameter.Add("YEAR", year);
            parameter.Add("SITE_ID", site_Id);
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE3", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD20>(parameter);

        }
        public HC_OSHA_CARD20 Insert(HC_OSHA_CARD20 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHACARD20                                                  ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                                  ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  YEAR,                                                                     ");
            parameter.AppendSql("  QUARTER,                                                                  ");
            parameter.AppendSql("  STATISFACTION,                                                            ");
            parameter.AppendSql("  NAME,                                                                     ");
            parameter.AppendSql("  MODIFIED,                                                                 ");
            parameter.AppendSql("  MODIFIEDUSER,                                                             ");
            parameter.AppendSql("  CREATED,                                                                  ");
            parameter.AppendSql("  CREATEDUSER,                                                              ");
            parameter.AppendSql("  SWLICENSE                                                                 ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :ID,                                                                      ");
            parameter.AppendSql("  :SITE_ID,                                                                 ");
            parameter.AppendSql("  :ESTIMATE_ID,                                                             ");
            parameter.AppendSql("  :YEAR,                                                                    ");
            parameter.AppendSql("  :QUARTER,                                                                 ");
            parameter.AppendSql("  :STATISFACTION,                                                           ");
            parameter.AppendSql("  :NAME,                                                                    ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                            ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :CREATEDUSER,                                                             ");
            parameter.AppendSql("  :SWLICENSE                                                                ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("QUARTER", dto.QUARTER);
            parameter.Add("STATISFACTION", dto.STATISFACTION);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_CARD20", dto.ID);
            return FindOne(dto.ID);
        }

        public HC_OSHA_CARD20 Update(HC_OSHA_CARD20 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD20                                                      ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  QUARTER = :QUARTER,                                                       ");
            parameter.AppendSql("  YEAR = :YEAR,                                                             ");
            parameter.AppendSql("  STATISFACTION = :STATISFACTION,                                           ");
            parameter.AppendSql("  NAME = :NAME,                                                             ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                  ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                              ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("QUARTER", dto.QUARTER);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("STATISFACTION", dto.STATISFACTION);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD20", dto.ID);
            return FindOne(dto.ID);

        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD20                        ");
            parameter.AppendSql("WHERE ID = :ID                                     ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_CARD20", id);
        }
    }
}
