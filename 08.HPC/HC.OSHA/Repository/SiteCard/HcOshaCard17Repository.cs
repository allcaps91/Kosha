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
    public class HcOshaCard17Repository : BaseRepository
    {

        public HC_OSHA_CARD17 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD17 A     ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                               ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                          ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                               ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                         ");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE        ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_CARD17>(parameter);
        }
        public List<HC_OSHA_CARD17> FindAll(long siteId, string startDate, string endDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER, D.CODENAME AS EDUTYPE,");
            parameter.AppendSql("       E.CODENAME AS EDUUSAGE ");
            parameter.AppendSql("  FROM HIC_OSHA_CARD17 A ");
            parameter.AppendSql("       INNER JOIN HIC_USERS B ");
            parameter.AppendSql("             ON A.CREATEDUSER = B.USERID ");
            parameter.AppendSql("             AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("       INNER JOIN HIC_USERS C ");
            parameter.AppendSql("             ON A.MODIFIEDUSER = C.USERID ");
            parameter.AppendSql("             AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("       INNER JOIN HIC_CODES D ");
            parameter.AppendSql("             ON D.CODE = A.EDUTYPE ");
            parameter.AppendSql("             AND D.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("             AND D.GROUPCODE = 'SITE_CARD_EDUTYPE' ");
            parameter.AppendSql("       INNER JOIN HIC_CODES E ");
            parameter.AppendSql("             ON E.CODE = A.EDUUSAGE ");
            parameter.AppendSql("             AND E.GROUPCODE = 'SITE_CARD_EDUUSAGE' ");
            parameter.AppendSql("             AND E.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("WHERE A.SITE_ID = :SITE_ID ");
            parameter.AppendSql("  AND A.EDUDATE BETWEEN :STARTDATE AND :ENDDATE ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("ORDER BY A.EDUDATE ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("STARTDATE", startDate);
            parameter.Add("ENDDATE", endDate);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD17>(parameter);

        }
        public List<HC_OSHA_CARD17> FindAll(long siteId, string year)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER, D.CODENAME AS EDUTYPE,");
            parameter.AppendSql("       E.CODENAME AS EDUUSAGE ");
            parameter.AppendSql("  FROM HIC_OSHA_CARD17 A ");
            parameter.AppendSql("       INNER JOIN HIC_USERS B ");
            parameter.AppendSql("             ON A.CREATEDUSER = B.USERID ");
            parameter.AppendSql("             AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("       INNER JOIN HIC_USERS C ");
            parameter.AppendSql("             ON A.MODIFIEDUSER = C.USERID ");
            parameter.AppendSql("             AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("       INNER JOIN HIC_CODES D ");
            parameter.AppendSql("             ON D.CODE = A.EDUTYPE ");
            parameter.AppendSql("             AND D.GROUPCODE = 'SITE_CARD_EDUTYPE' ");
            parameter.AppendSql("             AND D.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("       INNER JOIN HIC_CODES E ");
            parameter.AppendSql("             ON E.CODE = A.EDUUSAGE ");
            parameter.AppendSql("             AND E.GROUPCODE = 'SITE_CARD_EDUUSAGE' ");
            parameter.AppendSql("             AND E.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("WHERE A.SITE_ID = :SITE_ID ");
            parameter.AppendSql("  AND A.YEAR = :YEAR ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("ORDER BY A.EDUDATE ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("YEAR", year);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD17>(parameter);

        }
        public HC_OSHA_CARD17 Insert(HC_OSHA_CARD17 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD17                                                 ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                                  ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  YEAR,                                                                     ");
            parameter.AppendSql("  EDUDATE,                                                                  ");
            parameter.AppendSql("  EDUTYPE,                                                                  ");
            parameter.AppendSql("  EDUPLACE,                                                                 ");
            parameter.AppendSql("  EDUUSAGE,                                                                 ");
            parameter.AppendSql("  EDUNAME,                                                                  ");
            parameter.AppendSql("  TARGETCOUNT,                                                              ");
            parameter.AppendSql("  ACTCOUNT,                                                                 ");
            parameter.AppendSql("  CONTENT,                                                                  ");
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
            parameter.AppendSql("  :EDUDATE,                                                                 ");
            parameter.AppendSql("  :EDUTYPE,                                                                 ");
            parameter.AppendSql("  :EDUPLACE,                                                                ");
            parameter.AppendSql("  :EDUUSAGE,                                                                ");
            parameter.AppendSql("  :EDUNAME,                                                                 ");
            parameter.AppendSql("  :TARGETCOUNT,                                                             ");
            parameter.AppendSql("  :ACTCOUNT,                                                                ");
            parameter.AppendSql("  :CONTENT,                                                                 ");
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
            parameter.Add("EDUDATE", dto.EDUDATE);
            parameter.Add("EDUTYPE", dto.EDUTYPE);
            parameter.Add("EDUPLACE", dto.EDUPLACE);
            parameter.Add("EDUUSAGE", dto.EDUUSAGE);
            parameter.Add("EDUNAME", dto.EDUNAME);
            parameter.Add("TARGETCOUNT", dto.TARGETCOUNT);
            parameter.Add("ACTCOUNT", dto.ACTCOUNT);
            parameter.Add("CONTENT", dto.CONTENT);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_CARD17", dto.ID);
            return FindOne(dto.ID);
        }

        public HC_OSHA_CARD17 Update(HC_OSHA_CARD17 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD17                                                      ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  EDUDATE = :EDUDATE,                                                       ");
            parameter.AppendSql("  YEAR = :YEAR,                                                             ");
            parameter.AppendSql("  EDUTYPE = :EDUTYPE,                                                       ");
            parameter.AppendSql("  EDUPLACE = :EDUPLACE,                                                     ");
            parameter.AppendSql("  EDUUSAGE = :EDUUSAGE,                                                     ");
            parameter.AppendSql("  EDUNAME = :EDUNAME,                                                       ");
            parameter.AppendSql("  TARGETCOUNT = :TARGETCOUNT,                                               ");
            parameter.AppendSql("  ACTCOUNT = :ACTCOUNT,                                                     ");
            parameter.AppendSql("  CONTENT = :CONTENT,                                                       ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                  ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                              ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("EDUDATE", dto.EDUDATE);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("EDUTYPE", dto.EDUTYPE);
            parameter.Add("EDUPLACE", dto.EDUPLACE);
            parameter.Add("EDUUSAGE", dto.EDUUSAGE);
            parameter.Add("EDUNAME", dto.EDUNAME);
            parameter.Add("TARGETCOUNT", dto.TARGETCOUNT);
            parameter.Add("ACTCOUNT", dto.ACTCOUNT);
            parameter.Add("CONTENT", dto.CONTENT);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD17", dto.ID);
            return FindOne(dto.ID);
        }

        public void Delete(long id )
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD17                  ");
            parameter.AppendSql("WHERE ID = :ID                               ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_CARD17", id);
        }
    }
}
