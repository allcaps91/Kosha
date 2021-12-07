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
    public class HcOshaCard4_3Repository : BaseRepository
    {
        public HC_OSHA_CARD4_3 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_CARD4_3       ");
            parameter.AppendSql("WHERE ID = :ID                       ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE         ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_CARD4_3>(parameter);

        }
        public List<HC_OSHA_CARD4_3> FindAll(long estimateId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD4_3 A ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                 ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID            ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                 ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID           ");
            parameter.AppendSql("WHERE A.Estimate_Id = :Estimate_Id     ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE1        ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE2        ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE3        ");
            parameter.AppendSql("ORDER BY EDUDATE DESC                  ");

            parameter.Add("Estimate_Id", estimateId);
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE3", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD4_3>(parameter);
        }

        public HC_OSHA_CARD4_3 Insert(HC_OSHA_CARD4_3 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD4_3                    ");
            parameter.AppendSql("(                                               ");
            parameter.AppendSql("  ID,                                           ");
            parameter.AppendSql("  ESTIMATE_ID,                                  ");
            parameter.AppendSql("  NAME,                                         ");
            parameter.AppendSql("  DECLAREDATE,                                  ");
            parameter.AppendSql("  GRADE,                                        ");
            parameter.AppendSql("  TASKNAME,                                     ");
            parameter.AppendSql("  EDUNAME,                                      ");
            parameter.AppendSql("  EDUDATE,                                      ");
            parameter.AppendSql("  MODIFIED,                                     ");
            parameter.AppendSql("  MODIFIEDUSER,                                 ");
            parameter.AppendSql("  CREATED,                                      ");
            parameter.AppendSql("  CREATEDUSER,                                  ");
            parameter.AppendSql("  SWLICENSE                                     ");

            parameter.AppendSql(")                                               ");
            parameter.AppendSql("VALUES                                          ");
            parameter.AppendSql("(                                               ");
            parameter.AppendSql("  :ID,                                          ");
            parameter.AppendSql("  :ESTIMATE_ID,                                 ");
            parameter.AppendSql("  :NAME,                                        ");
            parameter.AppendSql("  :DECLAREDATE,                                 ");
            parameter.AppendSql("  :GRADE,                                       ");
            parameter.AppendSql("  :TASKNAME,                                    ");
            parameter.AppendSql("  :EDUNAME,                                     ");
            parameter.AppendSql("  :EDUDATE,                                     ");
            parameter.AppendSql("  SYSTIMESTAMP,                                 ");
            parameter.AppendSql("  :MODIFIEDUSER,                                ");
            parameter.AppendSql("  SYSTIMESTAMP,                                 ");
            parameter.AppendSql("  :CREATEDUSER                                  ");
            parameter.AppendSql("  :SWLICENSE                                    ");
            parameter.AppendSql(")                                               ");
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("ID", dto.ID);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("DECLAREDATE", dto.DECLAREDATE);
            parameter.Add("GRADE", dto.GRADE);
            parameter.Add("TASKNAME", dto.TASKNAME);
            parameter.Add("EDUNAME", dto.EDUNAME);
            parameter.Add("EDUDATE", dto.EDUDATE);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            DataSyncService.Instance.Insert("HIC_OSHA_CARD4_3", dto.ID);
            ExecuteNonQuery(parameter);
            return FindOne(dto.ID);
        }

        public void Update(HC_OSHA_CARD4_3 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD4_3                   ");
            parameter.AppendSql("SET                                       ");
            parameter.AppendSql("  NAME = :NAME,                           ");
            parameter.AppendSql("  DECLAREDATE = :DECLAREDATE,             ");
            parameter.AppendSql("  GRADE = :GRADE,                         ");
            parameter.AppendSql("  TASKNAME = :TASKNAME,                   ");
            parameter.AppendSql("  EDUNAME = :EDUNAME,                     ");
            parameter.AppendSql("  EDUDATE = :EDUDATE,                     ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER            ");
            parameter.AppendSql("WHERE ID = :ID                            ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("DECLAREDATE", dto.DECLAREDATE);
            parameter.Add("GRADE", dto.GRADE);
            parameter.Add("TASKNAME", dto.TASKNAME);
            parameter.Add("EDUNAME", dto.EDUNAME);
            parameter.Add("EDUDATE", dto.EDUDATE);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD4_3", dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD4_3                ");
            parameter.AppendSql("WHERE ID = :ID                              ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_CARD4_3", id);
        }
    }
}

