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
    public class HcOshaCard4_1Repository : BaseRepository
    {

        public HC_OSHA_CARD4_1 FindByEstimate(long estimateId, string year)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER ");
            parameter.AppendSql("  FROM HIC_OSHA_CARD4_1 A ");
            parameter.AppendSql("       INNER JOIN HIC_USERS B ");
            parameter.AppendSql("             ON A.CREATEDUSER = B.USERID ");
            parameter.AppendSql("       INNER JOIN HIC_USERS C ");
            parameter.AppendSql("             ON A.MODIFIEDUSER = C.USERID ");
            parameter.AppendSql(" WHERE A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND A.ID = ( ");
            parameter.AppendSql("       SELECT max(id) FROM HIC_OSHA_CARD4_1 ");
            parameter.AppendSql("        WHERE ESTIMATE_ID = :estimateId ");
            parameter.AppendSql("          AND YEAR = :YEAR ");
            parameter.AppendSql("          AND SWLICENSE = :SWLICENSE ) ");

            parameter.Add("estimateId", estimateId);
            parameter.Add("YEAR", year);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_CARD4_1>(parameter);

        }

        public HC_OSHA_CARD4_1 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER ");
            parameter.AppendSql("  FROM HIC_OSHA_CARD4_1 A    ");
            parameter.AppendSql("       INNER JOIN HIC_USERS B ");
            parameter.AppendSql("             ON A.CREATEDUSER = B.USERID ");
            parameter.AppendSql("       INNER JOIN HIC_USERS C ");
            parameter.AppendSql("             ON A.MODIFIEDUSER = C.USERID ");
            parameter.AppendSql(" WHERE A.ID = :ID ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND C.SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_CARD4_1>(parameter);

        }
        public HC_OSHA_CARD4_1 Insert(HC_OSHA_CARD4_1 dto)
        {
            dto.ID =  GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD4_1                                                ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  SWLICENSE,                                                                ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  TASKDIAGRAM,                                                              ");
            parameter.AppendSql("  YEAR,                                                                     ");
            parameter.AppendSql("  MODIFIED,                                                                 ");
            parameter.AppendSql("  MODIFIEDUSER,                                                             ");
            parameter.AppendSql("  CREATED,                                                                  ");
            parameter.AppendSql("  CREATEDUSER                                                               ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :SWLICENSE,                                                               ");
            parameter.AppendSql("  :ID,                                                                      ");
            parameter.AppendSql("  :ESTIMATE_ID,                                                             ");
            parameter.AppendSql("  :TASKDIAGRAM,                                                             ");
            parameter.AppendSql("  :YEAR,                                                                    ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                            ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                             ");
            parameter.AppendSql("  :CREATEDUSER                                                              ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("TASKDIAGRAM", dto.TASKDIAGRAM);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_CARD4_1", dto.ID);
            return FindOne(dto.ID);
        }

        public HC_OSHA_CARD4_1 Update(HC_OSHA_CARD4_1 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD4_1                                                 ");
            parameter.AppendSql("SET                                                                     ");
            parameter.AppendSql("  TASKDIAGRAM = :TASKDIAGRAM,                                           ");
            parameter.AppendSql("  YEAR = :YEAR,                                                         ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                              ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                          ");
            parameter.AppendSql("WHERE ID = :ID                                                          ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE                                            ");
            parameter.Add("ID", dto.ID);
            parameter.Add("TASKDIAGRAM", dto.TASKDIAGRAM);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD4_1", dto.ID);
            return FindOne(dto.ID);

        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD4_1              ");
            parameter.AppendSql("WHERE ID = :ID                            ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE              ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_CARD4_1", id);
        }
    }
}
