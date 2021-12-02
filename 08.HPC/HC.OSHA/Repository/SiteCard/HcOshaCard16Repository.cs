namespace HC.OSHA.Repository
{
    using ComBase.Mvc;
    using HC.Core.Service;
    using HC.OSHA.Dto;
    using HC_Core.Service;
    using System.Collections.Generic;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard16Repository : BaseRepository
    {
        public HC_OSHA_CARD16 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD16 A                          ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                               ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                         ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                               ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                         ");
            parameter.AppendSql("WHERE ID = :ID");

            parameter.Add("ID", id);

            return ExecuteReaderSingle<HC_OSHA_CARD16>(parameter);
        }
        public List<HC_OSHA_CARD16> FindAll(long siteId, string year)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD16 A                          ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                               ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                         ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                               ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                         ");
            parameter.AppendSql("WHERE SITE_ID = :SITE_ID             ");
            parameter.AppendSql("AND A.YEAR = :YEAR            ");
            parameter.AppendSql("ORDER BY  A.ID DESC            ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("YEAR", year);

            return ExecuteReader<HC_OSHA_CARD16>(parameter);

        }
        public HC_OSHA_CARD16 Insert(HC_OSHA_CARD16 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD16                                                  ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                                  ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  YEAR,                                                                 ");
            parameter.AppendSql("  TASKNAME,                                                                 ");
            parameter.AppendSql("  TASKTYPE,                                                                 ");
            parameter.AppendSql("  NAME,                                                                     ");
            parameter.AppendSql("  USAGE,                                                                    ");
            parameter.AppendSql("  QTY,                                                                      ");
            parameter.AppendSql("  EXPOSURE,                                                                 ");
            parameter.AppendSql("  COSENESS,                                                                 ");
            parameter.AppendSql("  PROTECTION,                                                               ");
            parameter.AppendSql("  ISMSDSPUBLISH,                                                            ");
            parameter.AppendSql("  ISALET,                                                                   ");
            parameter.AppendSql("  ISMSDSEDUCATION,                                                          ");
            parameter.AppendSql("  MODIFIED,                                                                 ");
            parameter.AppendSql("  MODIFIEDUSER,                                                             ");
            parameter.AppendSql("  CREATED,                                                                  ");
            parameter.AppendSql("  CREATEDUSER                                                              ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :ID,                                                                      ");
            parameter.AppendSql("  :SITE_ID,                                                                 ");
            parameter.AppendSql("  :ESTIMATE_ID,                                                             ");
            parameter.AppendSql("  :YEAR,                                                             ");
            parameter.AppendSql("  :TASKNAME,                                                                ");
            parameter.AppendSql("  :TASKTYPE,                                                                ");
            parameter.AppendSql("  :NAME,                                                                    ");
            parameter.AppendSql("  :USAGE,                                                                   ");
            parameter.AppendSql("  :QTY,                                                                     ");
            parameter.AppendSql("  :EXPOSURE,                                                                ");
            parameter.AppendSql("  :COSENESS,                                                                ");
            parameter.AppendSql("  :PROTECTION,                                                              ");
            parameter.AppendSql("  :ISMSDSPUBLISH,                                                           ");
            parameter.AppendSql("  :ISALET,                                                                  ");
            parameter.AppendSql("  :ISMSDSEDUCATION,                                                         ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                            ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                 ");
            parameter.AppendSql("  :CREATEDUSER                                                            ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("TASKNAME", dto.TASKNAME);
            parameter.Add("TASKTYPE", dto.TASKTYPE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("USAGE", dto.USAGE);
            parameter.Add("QTY", dto.QTY);
            parameter.Add("EXPOSURE", dto.EXPOSURE);
            parameter.Add("COSENESS", dto.COSENESS);
            parameter.Add("PROTECTION", dto.PROTECTION);
            parameter.Add("ISMSDSPUBLISH", dto.ISMSDSPUBLISH);
            parameter.Add("ISALET", dto.ISALET);
            parameter.Add("ISMSDSEDUCATION", dto.ISMSDSEDUCATION);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_CARD16", dto.ID);
            return FindOne(dto.ID);
        }

        public HC_OSHA_CARD16 Update(HC_OSHA_CARD16 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD16                                                       ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  TASKNAME = :TASKNAME,                                                     ");
            parameter.AppendSql("  TASKTYPE = :TASKTYPE,                                                     ");
            parameter.AppendSql("  NAME = :NAME,                                                             ");
            parameter.AppendSql("  YEAR = :YEAR,                                                             ");
            parameter.AppendSql("  USAGE = :USAGE,                                                           ");
            parameter.AppendSql("  QTY = :QTY,                                                               ");
            parameter.AppendSql("  EXPOSURE = :EXPOSURE,                                                     ");
            parameter.AppendSql("  COSENESS = :COSENESS,                                                     ");
            parameter.AppendSql("  PROTECTION = :PROTECTION,                                                 ");
            parameter.AppendSql("  ISMSDSPUBLISH = :ISMSDSPUBLISH,                                           ");
            parameter.AppendSql("  ISALET = :ISALET,                                                         ");
            parameter.AppendSql("  ISMSDSEDUCATION = :ISMSDSEDUCATION,                                       ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                     ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                             ");

            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            parameter.Add("TASKNAME", dto.TASKNAME);
            parameter.Add("TASKTYPE", dto.TASKTYPE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("USAGE", dto.USAGE);
            parameter.Add("QTY", dto.QTY);
            parameter.Add("EXPOSURE", dto.EXPOSURE);
            parameter.Add("COSENESS", dto.COSENESS);
            parameter.Add("PROTECTION", dto.PROTECTION);
            parameter.Add("ISMSDSPUBLISH", dto.ISMSDSPUBLISH);
            parameter.Add("ISALET", dto.ISALET);
            parameter.Add("ISMSDSEDUCATION", dto.ISMSDSEDUCATION);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD16", dto.ID);
            return FindOne(dto.ID);
        }

        public void Delete(HC_OSHA_CARD16 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD16                                                   ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_CARD16", dto.ID);
        }
    }
}
