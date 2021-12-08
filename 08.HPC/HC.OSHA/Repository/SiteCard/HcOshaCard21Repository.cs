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
    public class HcOshaCard21Repository : BaseRepository
    {

        public HC_OSHA_CARD21 FindByEstimateId(long id, string year)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD21 A   ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                              ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                         ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                              ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                        ");
            parameter.AppendSql("WHERE A.ESTIMATE_ID = :ESTIMATE_ID     ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND A.YEAR = :YEAR                   ");

            parameter.Add("ESTIMATE_ID", id);
            parameter.Add("YEAR", year);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_CARD21>(parameter);
        }

        public HC_OSHA_CARD21 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD21 A    ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                              ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                         ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                              ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                        ");
            parameter.AppendSql("WHERE A.ID = :ID                       ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE        ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_CARD21>(parameter);
        }
        
        public HC_OSHA_CARD21 Insert(HC_OSHA_CARD21 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD21                                                 ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                                  ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  YEAR,                                                                     ");
            parameter.AppendSql("  ISHOSPITAL,                                                               ");
            parameter.AppendSql("  HOSPITALNAME,                                                             ");
            parameter.AppendSql("  ADDRESS,                                                                  ");
            parameter.AppendSql("  TEL,                                                                      ");
            parameter.AppendSql("  ISMANAGER,                                                                ");
            parameter.AppendSql("  MANAGERNAME,                                                              ");
            parameter.AppendSql("  ISAIDKIT,                                                                 ");
            parameter.AppendSql("  AIDKITTYPE,                                                               ");
            parameter.AppendSql("  REMARK,                                                                   ");
            parameter.AppendSql("  CLEAN1CHECKBOX,                                                           ");
            parameter.AppendSql("  CLEAN1ETC,                                                                ");
            parameter.AppendSql("  CLEAN2CHECKBOX,                                                           ");
            parameter.AppendSql("  CLEAN2ETC,                                                                ");
            parameter.AppendSql("  CLEAN3CHECKBOX,                                                           ");
            parameter.AppendSql("  CLEAN3ETC,                                                                ");
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
            parameter.AppendSql("  :ISHOSPITAL,                                                              ");
            parameter.AppendSql("  :HOSPITALNAME,                                                            ");
            parameter.AppendSql("  :ADDRESS,                                                                 ");
            parameter.AppendSql("  :TEL,                                                                     ");
            parameter.AppendSql("  :ISMANAGER,                                                               ");
            parameter.AppendSql("  :MANAGERNAME,                                                             ");
            parameter.AppendSql("  :ISAIDKIT,                                                                ");
            parameter.AppendSql("  :AIDKITTYPE,                                                              ");
            parameter.AppendSql("  :REMARK,                                                                  ");
            parameter.AppendSql("  :CLEAN1CHECKBOX,                                                          ");
            parameter.AppendSql("  :CLEAN1ETC,                                                               ");
            parameter.AppendSql("  :CLEAN2CHECKBOX,                                                          ");
            parameter.AppendSql("  :CLEAN2ETC,                                                               ");
            parameter.AppendSql("  :CLEAN3CHECKBOX,                                                          ");
            parameter.AppendSql("  :CLEAN3ETC,                                                               ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                            ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :CREATEDUSER,                                                             ");
            parameter.AppendSql("  :SWLICENSE                                                                ");
            parameter.AppendSql(")                                              ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("ISHOSPITAL", dto.ISHOSPITAL);
            parameter.Add("HOSPITALNAME", dto.HOSPITALNAME);
            parameter.Add("ADDRESS", dto.ADDRESS);
            parameter.Add("TEL", dto.TEL);
            parameter.Add("ISMANAGER", dto.ISMANAGER);
            parameter.Add("MANAGERNAME", dto.MANAGERNAME);
            parameter.Add("ISAIDKIT", dto.ISAIDKIT);
            parameter.Add("AIDKITTYPE", dto.AIDKITTYPE);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("CLEAN1CHECKBOX", dto.CLEAN1CHECKBOX);
            parameter.Add("CLEAN1ETC", dto.CLEAN1ETC);
            parameter.Add("CLEAN2CHECKBOX", dto.CLEAN2CHECKBOX);
            parameter.Add("CLEAN2ETC", dto.CLEAN2ETC);
            parameter.Add("CLEAN3CHECKBOX", dto.CLEAN3CHECKBOX);
            parameter.Add("CLEAN3ETC", dto.CLEAN3ETC);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Insert("HIC_OSHA_CARD21", dto.ID);

            return FindOne(dto.ID);
        }

        public HC_OSHA_CARD21 Update(HC_OSHA_CARD21 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD21                                                      ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  ISHOSPITAL = :ISHOSPITAL,                                                 ");
            parameter.AppendSql("  HOSPITALNAME = :HOSPITALNAME,                                             ");
            parameter.AppendSql("  ADDRESS = :ADDRESS,                                                       ");
            parameter.AppendSql("  TEL = :TEL,                                                               ");
            parameter.AppendSql("  YEAR = :YEAR,                                                             ");
            parameter.AppendSql("  ISMANAGER = :ISMANAGER,                                                   ");
            parameter.AppendSql("  MANAGERNAME = :MANAGERNAME,                                               ");
            parameter.AppendSql("  ISAIDKIT = :ISAIDKIT,                                                     ");
            parameter.AppendSql("  AIDKITTYPE = :AIDKITTYPE,                                                 ");
            parameter.AppendSql("  REMARK = :REMARK,                                                         ");
            parameter.AppendSql("  CLEAN1CHECKBOX = :CLEAN1CHECKBOX,                                         ");
            parameter.AppendSql("  CLEAN1ETC = :CLEAN1ETC,                                                   ");
            parameter.AppendSql("  CLEAN2CHECKBOX = :CLEAN2CHECKBOX,                                         ");
            parameter.AppendSql("  CLEAN2ETC = :CLEAN2ETC,                                                   ");
            parameter.AppendSql("  CLEAN3CHECKBOX = :CLEAN3CHECKBOX,                                         ");
            parameter.AppendSql("  CLEAN3ETC = :CLEAN3ETC,                                                   ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                  ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                              ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("ISHOSPITAL", dto.ISHOSPITAL);
            parameter.Add("HOSPITALNAME", dto.HOSPITALNAME);
            parameter.Add("ADDRESS", dto.ADDRESS);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("TEL", dto.TEL);
            parameter.Add("ISMANAGER", dto.ISMANAGER);
            parameter.Add("MANAGERNAME", dto.MANAGERNAME);
            parameter.Add("ISAIDKIT", dto.ISAIDKIT);
            parameter.Add("AIDKITTYPE", dto.AIDKITTYPE);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("CLEAN1CHECKBOX", dto.CLEAN1CHECKBOX);
            parameter.Add("CLEAN1ETC", dto.CLEAN1ETC);
            parameter.Add("CLEAN2CHECKBOX", dto.CLEAN2CHECKBOX);
            parameter.Add("CLEAN2ETC", dto.CLEAN2ETC);
            parameter.Add("CLEAN3CHECKBOX", dto.CLEAN3CHECKBOX);
            parameter.Add("CLEAN3ETC", dto.CLEAN3ETC);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD21", dto.ID);
            return FindOne(dto.ID);

        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD21       ");
            parameter.AppendSql("WHERE ID = :ID                    ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE      ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_CARD21", id);
        }
    }
}
