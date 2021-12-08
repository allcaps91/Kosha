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
    public class HcOshaCard95Repository : BaseRepository
    {

        public HC_OSHA_CARD9_5 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD9_5 A    ");
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

            return ExecuteReaderSingle<HC_OSHA_CARD9_5>(parameter);
        }
        public List<HC_OSHA_CARD9_5> FindAll(long siteId, string year)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER, D.CODENAME AS ISAPPROVE_TEXT FROM HIC_OSHA_CARD9_5 A  ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                      ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                 ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                      ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                ");
            parameter.AppendSql("INNER JOIN HIC_CODES D                      ");
            parameter.AppendSql("ON A.ISAPPROVE = D.CODE                     ");
            parameter.AppendSql("AND D.GROUPCODE = 'SITE_CARD_APPROVE'       ");
            parameter.AppendSql("WHERE A.SITE_ID = :SITE_ID                  ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE             ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE             ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE             ");
            parameter.AppendSql("ORDER BY REQUESTDATE DESC                   ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<HC_OSHA_CARD9_5>(parameter);

        }
        public HC_OSHA_CARD9_5 Insert(HC_OSHA_CARD9_5 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD9_5      ");
            parameter.AppendSql("(                                 ");
            parameter.AppendSql("  ID,                             ");
            parameter.AppendSql("  SITE_ID,                        ");
            parameter.AppendSql("  ESTIMATE_ID,                    ");
            parameter.AppendSql("  REQUESTDATE,                    ");
            parameter.AppendSql("  APPROVEDATE,                    ");
            parameter.AppendSql("  ISAPPROVE,                      ");
            parameter.AppendSql("  STARTDATE,                      ");
            parameter.AppendSql("  ENDDATE,                        ");
            parameter.AppendSql("  REMARK,                         ");
            parameter.AppendSql("  MODIFIED,                       ");
            parameter.AppendSql("  MODIFIEDUSER,                   ");
            parameter.AppendSql("  CREATED,                        ");
            parameter.AppendSql("  CREATEDUSER,                    ");
            parameter.AppendSql("  SWLICENSE                       ");
            parameter.AppendSql(")                                 ");
            parameter.AppendSql("VALUES                            ");
            parameter.AppendSql("(                                 ");
            parameter.AppendSql("  :ID,                            ");
            parameter.AppendSql("  :SITE_ID,                       ");
            parameter.AppendSql("  :ESTIMATE_ID,                   ");
            parameter.AppendSql("  :REQUESTDATE,                   ");
            parameter.AppendSql("  :APPROVEDATE,                   ");
            parameter.AppendSql("  :ISAPPROVE,                     ");
            parameter.AppendSql("  :STARTDATE,                     ");
            parameter.AppendSql("  :ENDDATE,                       ");
            parameter.AppendSql("  :REMARK,                        ");
            parameter.AppendSql("  SYSTIMESTAMP,                   ");
            parameter.AppendSql("  :MODIFIEDUSER,                  ");
            parameter.AppendSql("  SYSTIMESTAMP,                   ");
            parameter.AppendSql("  :CREATEDUSER,                   ");
            parameter.AppendSql("  :SWLICENSE                      ");
            parameter.AppendSql(")                                 ");

            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("REQUESTDATE", dto.REQUESTDATE);
            parameter.Add("APPROVEDATE", dto.APPROVEDATE);
            parameter.Add("ISAPPROVE", dto.ISAPPROVE);
            parameter.Add("STARTDATE", dto.STARTDATE);
            parameter.Add("ENDDATE", dto.ENDDATE);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_CARD9_5", dto.ID);

            return FindOne(dto.ID);
        }

        public HC_OSHA_CARD9_5 Update(HC_OSHA_CARD9_5 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD9_5                         ");
            parameter.AppendSql("SET                                             ");
            parameter.AppendSql("  REQUESTDATE = :REQUESTDATE,                   ");
            parameter.AppendSql("  APPROVEDATE = :APPROVEDATE,                   ");
            parameter.AppendSql("  ISAPPROVE = :ISAPPROVE,                       ");
            parameter.AppendSql("  STARTDATE = :STARTDATE,                       ");
            parameter.AppendSql("  ENDDATE = :ENDDATE,                           ");
            parameter.AppendSql("  REMARK = :REMARK,                             ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                      ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                  ");
            parameter.AppendSql("WHERE ID = :ID                                  ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("REQUESTDATE", dto.REQUESTDATE);
            parameter.Add("APPROVEDATE", dto.APPROVEDATE);
            parameter.Add("ISAPPROVE", dto.ISAPPROVE);
            parameter.Add("STARTDATE", dto.STARTDATE);
            parameter.Add("ENDDATE", dto.ENDDATE);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD9_5", dto.ID);
            return FindOne(dto.ID);
        }


        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD9_5              ");
            parameter.AppendSql("WHERE ID = :ID                            ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_CARD9_5", id);
        }
    }
}