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
    public class HcOshaCard92Repository : BaseRepository
    {

        public HC_OSHA_CARD9_2 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD9_2 A   ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                    ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID               ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                    ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID              ");
            parameter.AppendSql("WHERE A.ID = :ID                          ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE           ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE           ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE           ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReaderSingle<HC_OSHA_CARD9_2>(parameter);
        }
        public List<HC_OSHA_CARD9_2> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD9_2 A  ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                     ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                     ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID               ");
            parameter.AppendSql("WHERE A.SITE_ID = :SITE_ID                 ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE            ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE            ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE            ");
            parameter.AppendSql("ORDER BY STARTDATE  DESC ");
            parameter.Add("SITE_ID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD9_2>(parameter);

        }
        public HC_OSHA_CARD9_2 Insert(HC_OSHA_CARD9_2 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD9_2          ");
            parameter.AppendSql("(                                     ");
            parameter.AppendSql("  ID,                                 ");
            parameter.AppendSql("  SITE_ID,                            ");
            parameter.AppendSql("  ESTIMATE_ID,                        ");
            parameter.AppendSql("  WORKNAME,                           ");
            parameter.AppendSql("  STARTDATE,                          ");
            parameter.AppendSql("  ENDDATE,                            ");
            parameter.AppendSql("  CONTENT,                            ");
            parameter.AppendSql("  GRADE1,                             ");
            parameter.AppendSql("  NAME1,                              ");
            parameter.AppendSql("  GRADE2,                             ");
            parameter.AppendSql("  NAME2,                              ");
            parameter.AppendSql("  MODIFIED,                           ");
            parameter.AppendSql("  MODIFIEDUSER,                       ");
            parameter.AppendSql("  CREATED,                            ");
            parameter.AppendSql("  CREATEDUSER,                        ");
            parameter.AppendSql("  SWLICENSE                           ");
            parameter.AppendSql(")                                     ");
            parameter.AppendSql("VALUES                                ");
            parameter.AppendSql("(                                     ");
            parameter.AppendSql("  :ID,                                ");
            parameter.AppendSql("  :SITE_ID,                           ");
            parameter.AppendSql("  :ESTIMATE_ID,                       ");
            parameter.AppendSql("  :WORKNAME,                          ");
            parameter.AppendSql("  :STARTDATE,                         ");
            parameter.AppendSql("  :ENDDATE,                           ");
            parameter.AppendSql("  :CONTENT,                           ");
            parameter.AppendSql("  :GRADE1,                            ");
            parameter.AppendSql("  :NAME1,                             ");
            parameter.AppendSql("  :GRADE2,                            ");
            parameter.AppendSql("  :NAME2,                             ");
            parameter.AppendSql("  SYSTIMESTAMP,                       ");
            parameter.AppendSql("  :MODIFIEDUSER,                      ");
            parameter.AppendSql("  SYSTIMESTAMP,                       ");
            parameter.AppendSql("  :CREATEDUSER,                       ");
            parameter.AppendSql("  :SWLICENSE                          ");
            parameter.AppendSql(")                                     ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("WORKNAME", dto.WORKNAME);
            parameter.Add("STARTDATE", dto.STARTDATE);
            parameter.Add("ENDDATE", dto.ENDDATE);
            parameter.Add("CONTENT", dto.CONTENT);
            parameter.Add("GRADE1", dto.GRADE1);
            parameter.Add("NAME1", dto.NAME1);
            parameter.Add("GRADE2", dto.GRADE2);
            parameter.Add("NAME2", dto.NAME2);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_CARD9_2", dto.ID);

            return FindOne(dto.ID);
        }

        public HC_OSHA_CARD9_2 Update(HC_OSHA_CARD9_2 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD9_2                     ");
            parameter.AppendSql("SET                                         ");
            parameter.AppendSql("  WORKNAME = :WORKNAME,                     ");
            parameter.AppendSql("  STARTDATE = :STARTDATE,                   ");
            parameter.AppendSql("  ENDDATE = :ENDDATE,                       ");
            parameter.AppendSql("  CONTENT = :CONTENT,                       ");
            parameter.AppendSql("  GRADE1 = :GRADE1,                         ");
            parameter.AppendSql("  NAME1 = :NAME1,                           ");
            parameter.AppendSql("  GRADE2 = :GRADE2,                         ");
            parameter.AppendSql("  NAME2 = :NAME2,                           ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                  ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER              ");
            parameter.AppendSql("WHERE ID = :ID                              ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("WORKNAME", dto.WORKNAME);
            parameter.Add("STARTDATE", dto.STARTDATE);
            parameter.Add("ENDDATE", dto.ENDDATE);
            parameter.Add("CONTENT", dto.CONTENT);
            parameter.Add("GRADE1", dto.GRADE1);
            parameter.Add("NAME1", dto.NAME1);
            parameter.Add("GRADE2", dto.GRADE2);
            parameter.Add("NAME2", dto.NAME2);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD9_2", dto.ID);

            return FindOne(dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD9_2          ");
            parameter.AppendSql("WHERE ID = :ID                        ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE          ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_CARD9_2", id);
        }
    }
}