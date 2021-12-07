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
    public class HcOshaCard13Repository : BaseRepository
    {

        public HC_OSHA_CARD13 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD13 A     ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                               ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                          ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                               ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                         ");
            parameter.AppendSql("WHERE ID = :ID");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE1        ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE2        ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE3        ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE3", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_CARD13>(parameter);
        }
        public List<HC_OSHA_CARD13> FindAll(long siteId, string year)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD13 A     ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                               ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                          ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                               ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                         ");
            parameter.AppendSql("WHERE A.SITE_ID = :SITE_ID             ");
            parameter.AppendSql("  AND A.YEAR = :YEAR                   ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE1        ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE2        ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE3        ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("YEAR", year);
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE3", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD13>(parameter);

        }
        public HC_OSHA_CARD13 Insert(HC_OSHA_CARD13 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD13                                                  ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                                  ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  YEAR,                                                                     ");
            parameter.AppendSql("  NAME,                                                                     ");
            parameter.AppendSql("  TARGET,                                                                   ");
            parameter.AppendSql("  WORKERCOUNT,                                                              ");
            parameter.AppendSql("  TARGETCOUNT,                                                              ");
            parameter.AppendSql("  PASSNUMBER,                                                               ");
            parameter.AppendSql("  REMARK,                                                                   ");
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
            parameter.AppendSql("  :NAME,                                                                    ");
            parameter.AppendSql("  :TARGET,                                                                  ");
            parameter.AppendSql("  :WORKERCOUNT,                                                             ");
            parameter.AppendSql("  :TARGETCOUNT,                                                             ");
            parameter.AppendSql("  :PASSNUMBER,                                                              ");
            parameter.AppendSql("  :REMARK,                                                                  ");
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
            parameter.Add("NAME", dto.NAME);
            parameter.Add("TARGET", dto.TARGET);
            parameter.Add("WORKERCOUNT", dto.WORKERCOUNT);
            parameter.Add("TARGETCOUNT", dto.TARGETCOUNT);
            parameter.Add("PASSNUMBER", dto.PASSNUMBER);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_CARD13", dto.ID);
            return FindOne(dto.ID);
        }

        public HC_OSHA_CARD13 Update(HC_OSHA_CARD13 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD3                                                       ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  ID = :ID,                                                                 ");
            parameter.AppendSql("  NAME = :NAME,                                                             ");
            parameter.AppendSql("  YEAR = :YEAR,                                                             ");
            parameter.AppendSql("  TARGET = :TARGET,                                                         ");
            parameter.AppendSql("  WORKERCOUNT = :WORKERCOUNT,                                               ");
            parameter.AppendSql("  TARGETCOUNT = :TARGETCOUNT,                                               ");
            parameter.AppendSql("  PASSNUMBER = :PASSNUMBER,                                                 ");
            parameter.AppendSql("  REMARK = :REMARK,                                                         ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                  ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                              ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("TARGET", dto.TARGET);
            parameter.Add("WORKERCOUNT", dto.WORKERCOUNT);
            parameter.Add("TARGETCOUNT", dto.TARGETCOUNT);
            parameter.Add("PASSNUMBER", dto.PASSNUMBER);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD13", dto.ID);
            return FindOne(dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD13                 ");
            parameter.AppendSql("WHERE ID = :ID                              ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE                ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_CARD13", id);
        }
    }
}
