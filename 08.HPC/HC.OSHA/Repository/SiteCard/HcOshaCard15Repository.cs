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
    public class HcOshaCard15Repository : BaseRepository
    {
        public HC_OSHA_CARD15 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD15 A     ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                               ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                          ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                               ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                         ");
            parameter.AppendSql("WHERE ID = :ID");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE        ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_CARD15>(parameter);
        }
        public List<HC_OSHA_CARD15> FindAll(long siteId, string year)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD15 A                          ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                               ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                         ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                               ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                         ");
            parameter.AppendSql("WHERE SITE_ID = :SITE_ID            ");
            parameter.AppendSql("  AND  YEAR = :YEAR                 ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE     ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE     ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE     ");
            parameter.AppendSql("ORDER BY A.MODIFIED DESC            ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("YEAR", year);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD15>(parameter);

        }
        public HC_OSHA_CARD15 Insert(HC_OSHA_CARD15 dto )
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD15                                                  ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  ID,                                                                       ");
            parameter.AppendSql("  SITE_ID,                                                                  ");
            parameter.AppendSql("  ESTIMATE_ID,                                                              ");
            parameter.AppendSql("  YEAR,                                                                    ");
            parameter.AppendSql("  TASKNAME,                                                                 ");
            parameter.AppendSql("  TASKTYPE,                                                                 ");
            parameter.AppendSql("  NAME,                                                                     ");
            parameter.AppendSql("  QTY,                                                                      ");
            parameter.AppendSql("  STATUS,                                                                   ");
            parameter.AppendSql("  REMARK,                                                                   ");
            parameter.AppendSql("  MANAGESTATUS,                                                             ");
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
            parameter.AppendSql("  :TASKNAME,                                                                ");
            parameter.AppendSql("  :TASKTYPE,                                                                ");
            parameter.AppendSql("  :NAME,                                                                    ");
            parameter.AppendSql("  :QTY,                                                                     ");
            parameter.AppendSql("  :STATUS,                                                                  ");
            parameter.AppendSql("  :REMARK,                                                                  ");
            parameter.AppendSql("  :MANAGESTATUS,                                                            ");
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
            parameter.Add("TASKNAME", dto.TASKNAME);
            parameter.Add("TASKTYPE", dto.TASKTYPE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("QTY", dto.QTY);
            parameter.Add("STATUS", dto.STATUS);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("MANAGESTATUS", dto.MANAGESTATUS);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_CARD15", dto.ID);
            return FindOne(dto.ID);
        }
    

        public HC_OSHA_CARD15 Update(HC_OSHA_CARD15 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD15                                                      ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  ID = :ID,                                                                 ");
            parameter.AppendSql("  TASKNAME = :TASKNAME,                                                     ");
            parameter.AppendSql("  TASKTYPE = :TASKTYPE,                                                     ");
            parameter.AppendSql("  NAME = :NAME,                                                             ");
            parameter.AppendSql("  QTY = :QTY,                                                               ");
            parameter.AppendSql("  STATUS = :STATUS,                                                         ");
            parameter.AppendSql("  REMARK = :REMARK,                                                         ");
            parameter.AppendSql("  MANAGESTATUS = :MANAGESTATUS,                                             ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                  ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                              ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("TASKNAME", dto.TASKNAME);
            parameter.Add("TASKTYPE", dto.TASKTYPE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("QTY", dto.QTY);
            parameter.Add("STATUS", dto.STATUS);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("MANAGESTATUS", dto.MANAGESTATUS);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD15", dto.ID);
            return FindOne(dto.ID);
        }


        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD15                  ");
            parameter.AppendSql("WHERE ID = :ID                               ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_CARD15", id);
        }
     }
}
