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
    public class HcOshaCard93Repository : BaseRepository
    {

        public HC_OSHA_CARD9_3 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD9_3 A    ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                 ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID            ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                 ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID           ");
            parameter.AppendSql("WHERE A.ID = :ID                       ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE        ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_CARD9_3>(parameter);
        }
        public List<HC_OSHA_CARD9_3> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD9_3 A   ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                 ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID            ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                 ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID           ");
            parameter.AppendSql("WHERE A.SITE_ID = :SITE_ID             ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("ORDER BY SITESTARTDATE DESC            ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD9_3>(parameter);
        }
        public HC_OSHA_CARD9_3 Insert(HC_OSHA_CARD9_3 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD9_3     ");
            parameter.AppendSql("(                                ");
            parameter.AppendSql("  ID,                            ");
            parameter.AppendSql("  SITE_ID,                       ");
            parameter.AppendSql("  ESTIMATE_ID,                   ");
            parameter.AppendSql("  SITESTARTDATE,                 ");
            parameter.AppendSql("  SITEENDDATE,                   ");
            parameter.AppendSql("  SITEGRADE,                     ");
            parameter.AppendSql("  SITENAME,                      ");
            parameter.AppendSql("  TESTSTARTDATE,                 ");
            parameter.AppendSql("  TESTENDDATE,                   ");
            parameter.AppendSql("  TESTGRADE,                     ");
            parameter.AppendSql("  TESTNAME,                      ");
            parameter.AppendSql("  MODIFIED,                      ");
            parameter.AppendSql("  MODIFIEDUSER,                  ");
            parameter.AppendSql("  CREATED,                       ");
            parameter.AppendSql("  CREATEDUSER,                   ");
            parameter.AppendSql("  SWLICENSE                      ");
            parameter.AppendSql(")                                ");
            parameter.AppendSql("VALUES                           ");
            parameter.AppendSql("(                                ");
            parameter.AppendSql("  :ID,                           ");
            parameter.AppendSql("  :SITE_ID,                      ");
            parameter.AppendSql("  :ESTIMATE_ID,                  ");
            parameter.AppendSql("  :SITESTARTDATE,                ");
            parameter.AppendSql("  :SITEENDDATE,                  ");
            parameter.AppendSql("  :SITEGRADE,                    ");
            parameter.AppendSql("  :SITENAME,                     ");
            parameter.AppendSql("  :TESTSTARTDATE,                ");
            parameter.AppendSql("  :TESTENDDATE,                  ");
            parameter.AppendSql("  :TESTGRADE,                    ");
            parameter.AppendSql("  :TESTNAME,                     ");
            parameter.AppendSql("  SYSTIMESTAMP,                  ");
            parameter.AppendSql("  :MODIFIEDUSER,                 ");
            parameter.AppendSql("  SYSTIMESTAMP,                  ");
            parameter.AppendSql("  :CREATEDUSER,                  ");
            parameter.AppendSql("  :SWLICENSE                     ");
            parameter.AppendSql(")                                ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("SITESTARTDATE", dto.SITESTARTDATE);
            parameter.Add("SITEENDDATE", dto.SITEENDDATE);
            parameter.Add("SITEGRADE", dto.SITEGRADE);
            parameter.Add("SITENAME", dto.SITENAME);
            parameter.Add("TESTSTARTDATE", dto.TESTSTARTDATE);
            parameter.Add("TESTENDDATE", dto.TESTENDDATE);
            parameter.Add("TESTGRADE", dto.TESTGRADE);
            parameter.Add("TESTNAME", dto.TESTNAME);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_CARD9_3", dto.ID);

            return FindOne(dto.ID);
        }

        public HC_OSHA_CARD9_3 Update(HC_OSHA_CARD9_3 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD9_3                    ");
            parameter.AppendSql("SET                                        ");
            parameter.AppendSql("  SITESTARTDATE = :SITESTARTDATE,          ");
            parameter.AppendSql("  SITEENDDATE = :SITEENDDATE,              ");
            parameter.AppendSql("  SITEGRADE = :SITEGRADE,                  ");
            parameter.AppendSql("  SITENAME = :SITENAME,                    ");
            parameter.AppendSql("  TESTSTARTDATE = :TESTSTARTDATE,          ");
            parameter.AppendSql("  TESTENDDATE = :TESTENDDATE,              ");
            parameter.AppendSql("  TESTGRADE = :TESTGRADE,                  ");
            parameter.AppendSql("  TESTNAME = :TESTNAME,                    ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                 ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER             ");
            parameter.AppendSql("WHERE ID = :ID                             ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITESTARTDATE", dto.SITESTARTDATE);
            parameter.Add("SITEENDDATE", dto.SITEENDDATE);
            parameter.Add("SITEGRADE", dto.SITEGRADE);
            parameter.Add("SITENAME", dto.SITENAME);
            parameter.Add("TESTSTARTDATE", dto.TESTSTARTDATE);
            parameter.Add("TESTENDDATE", dto.TESTENDDATE);
            parameter.Add("TESTGRADE", dto.TESTGRADE);
            parameter.Add("TESTNAME", dto.TESTNAME);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD9_3", dto.ID);

            return FindOne(dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD9_3           ");
            parameter.AppendSql("WHERE ID = :ID                         ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_CARD9_3", id);
        }
    }
}