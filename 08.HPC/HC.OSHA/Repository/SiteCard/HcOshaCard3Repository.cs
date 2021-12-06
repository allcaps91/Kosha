namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using HC.Core.Service;
    using HC.OSHA.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard3Repository : BaseRepository
    {
        public HC_OSHA_CARD3 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_CARD3                                         ");
            parameter.AppendSql(" WHERE ID  = :ID                                                     ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE                                        ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_CARD3>(parameter);

        }
        public List<HC_OSHA_CARD3> FindAll(long estimateId, string year)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD3 A  ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                          ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                          ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                          ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                          ");
            parameter.AppendSql("WHERE A.ESTIMATE_ID = :ESTIMATE_ID                    ");
            parameter.AppendSql("  AND A.YEAR = :YEAR                              ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE                    ");
            parameter.AppendSql("  AND A.SWLICENSE = B.SWLICENSE                    ");
            parameter.AppendSql("  AND A.SWLICENSE = C.SWLICENSE                    ");
            parameter.AppendSql("  ORDER BY A.TASKTYPE                              ");

            parameter.Add("ESTIMATE_ID", estimateId);
            parameter.Add("YEAR", year);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD3>(parameter);

        }
        public HC_OSHA_CARD3 Insert(HC_OSHA_CARD3 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD3                                                   ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  YEAR,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                              ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  TASKTYPE,                                                                  ");
            parameter.AppendSql("  NAME,                                                                     ");
            parameter.AppendSql("  DELACEDATE,                                                               ");
            parameter.AppendSql("  CERT,                                                                     ");
            parameter.AppendSql("  EDUCATION,                                                                ");
            parameter.AppendSql("  CARRER,                                                                   ");
            parameter.AppendSql("  EDUATIONNAME,                                                             ");
            parameter.AppendSql("  EDUCATIONSTARTDATE,                                                       ");
            parameter.AppendSql("  EDUCATIONENDDATE,                                                         ");
            parameter.AppendSql("  MODIFIED,                                                                 ");
            parameter.AppendSql("  MODIFIEDUSER,                                                             ");
            parameter.AppendSql("  CREATED,                                                                  ");
            parameter.AppendSql("  CREATEDUSER                                                               ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :ID,                                                                      ");
            parameter.AppendSql("  :YEAR,                                                                     ");
            parameter.AppendSql("  :SITE_ID,                                                              ");
            parameter.AppendSql("  :ESTIMATE_ID,                                                             ");
            parameter.AppendSql("  :TASKTYPE,                                                                 ");
            parameter.AppendSql("  :NAME,                                                                    ");
            parameter.AppendSql("  :DELACEDATE,                                                              ");
            parameter.AppendSql("  :CERT,                                                                    ");
            parameter.AppendSql("  :EDUCATION,                                                               ");
            parameter.AppendSql("  :CARRER,                                                                  ");
            parameter.AppendSql("  :EDUATIONNAME,                                                            ");
            parameter.AppendSql("  :EDUCATIONSTARTDATE,                                                      ");
            parameter.AppendSql("  :EDUCATIONENDDATE,                                                        ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                            ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                 ");
            parameter.AppendSql("  :CREATEDUSER                                                            ");
            parameter.AppendSql(")                                                                           "); 
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("ID", dto.ID);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("TASKTYPE", dto.TASKTYPE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("DELACEDATE", dto.DELACEDATE);
            parameter.Add("CERT", dto.CERT);
            parameter.Add("EDUCATION", dto.EDUCATION);
            parameter.Add("CARRER", dto.CARRER);
            parameter.Add("EDUATIONNAME", dto.EDUATIONNAME);
            parameter.Add("EDUCATIONSTARTDATE", dto.EDUCATIONSTARTDATE);
            parameter.Add("EDUCATIONENDDATE", dto.EDUCATIONENDDATE);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);
            //DataSyncService.Instance.Insert("HIC_OSHA_CARD3", dto.ID);
            return FindOne(dto.ID);
        }

        public void Update(HC_OSHA_CARD3 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD3                                                       ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  TASKTYPE = :TASKTYPE,                                                     ");
            parameter.AppendSql("  NAME = :NAME,                                                             ");
            parameter.AppendSql("  YEAR = :YEAR,                                                             ");
            parameter.AppendSql("  DELACEDATE = :DELACEDATE,                                                 ");
            parameter.AppendSql("  CERT = :CERT,                                                             ");
            parameter.AppendSql("  EDUCATION = :EDUCATION,                                                   ");
            parameter.AppendSql("  CARRER = :CARRER,                                                         ");
            parameter.AppendSql("  EDUATIONNAME = :EDUATIONNAME,                                             ");
            parameter.AppendSql("  EDUCATIONSTARTDATE = :EDUCATIONSTARTDATE,                                 ");
            parameter.AppendSql("  EDUCATIONENDDATE = :EDUCATIONENDDATE,                                     ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                  ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                              ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE                                                ");

            parameter.Add("ID", dto.ID);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            parameter.Add("TASKTYPE", dto.TASKTYPE);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("DELACEDATE", dto.DELACEDATE);
            parameter.Add("CERT", dto.CERT);
            parameter.Add("EDUCATION", dto.EDUCATION);
            parameter.Add("CARRER", dto.CARRER);
            parameter.Add("EDUATIONNAME", dto.EDUATIONNAME);
            parameter.Add("EDUCATIONSTARTDATE", dto.EDUCATIONSTARTDATE);
            parameter.Add("EDUCATIONENDDATE", dto.EDUCATIONENDDATE);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

            //DataSyncService.Instance.Update("HIC_OSHA_CARD3", dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD3                                                   ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", id);
            ExecuteNonQuery(parameter);


            //DataSyncService.Instance.Delete("HIC_OSHA_CARD3", id);
        }
    }
}
