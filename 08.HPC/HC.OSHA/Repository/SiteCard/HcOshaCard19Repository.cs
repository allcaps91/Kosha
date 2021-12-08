namespace HC.OSHA.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase;
    using ComBase.Mvc;
    using HC.Core.Service;
    using HC.OSHA.Dto;
    using HC_Core.Service;

    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard19Repository : BaseRepository
    {

        public HC_OSHA_CARD19 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD19 A  ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                            ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                       ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                            ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                      ");
            parameter.AppendSql("WHERE A.ID = :ID ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE        ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_CARD19>(parameter);
        }
        
        public List<HC_OSHA_CARD19> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD19 A    ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                                              ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                                         ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                                                              ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID                                                        ");
            parameter.AppendSql("WHERE A.SITE_ID = :SITE_ID             ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE        ");
            parameter.AppendSql("ORDER BY REGDATE                       ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD19>(parameter);

        }

        internal List<HC_OSHA_CARD19> FindAll(long siteId, string contractStartDate, string contractendDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*                                              ");
            parameter.AppendSql("     , B.NAME AS CREATEDUSER                            ");
            parameter.AppendSql("     , C.NAME AS MODIFIEDUSER                           ");
            parameter.AppendSql("  FROM HIC_OSHA_CARD19 A                                ");
            parameter.AppendSql("  INNER JOIN HIC_USERS B                                ");
            parameter.AppendSql("          ON A.CREATEDUSER = B.USERID                   ");
            parameter.AppendSql("  INNER JOIN HIC_USERS C                                ");
            parameter.AppendSql("          ON A.MODIFIEDUSER = C.USERID                  ");
            parameter.AppendSql(" WHERE A.SITE_ID  = :SITE_ID                            ");
            parameter.AppendSql("   AND A.REGDATE >= TO_DATE(:START_DATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND A.REGDATE <= TO_DATE(:END_DATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE                        ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE                        ");
            parameter.AppendSql("   AND C.SWLICENSE = :SWLICENSE                        ");
            parameter.AppendSql("ORDER BY REGDATE                                        ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("START_DATE", string.Concat(contractStartDate, "-01-01"));
            //parameter.Add("END_DATE", contractendDate);
            parameter.Add("END_DATE", string.Concat(contractendDate, "-12-31"));
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD19>(parameter);

        }

        public HC_OSHA_CARD19 Insert(HC_OSHA_CARD19 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD19                    ");
            parameter.AppendSql("(                                              ");
            parameter.AppendSql("  ID,                                          ");
            parameter.AppendSql("  SITE_ID,                                     ");
            parameter.AppendSql("  ESTIMATE_ID,                                 ");
            parameter.AppendSql("  REGDATE,                                     ");
            parameter.AppendSql("  CERT,                                        ");
            parameter.AppendSql("  NAME,                                        ");
            parameter.AppendSql("  CONTENT,                                     ");
            parameter.AppendSql("  OPINION,                                     ");
            parameter.AppendSql("  STATUS,                                      ");
            parameter.AppendSql("  SITESIGN,                                    ");
            parameter.AppendSql("  MODIFIED,                                    ");
            parameter.AppendSql("  MODIFIEDUSER,                                ");
            parameter.AppendSql("  CREATED,                                     ");
            parameter.AppendSql("  CREATEDUSER,                                 ");
            parameter.AppendSql("  SWLICENSE                                    ");
            parameter.AppendSql(")                                              ");
            parameter.AppendSql("VALUES                                         ");
            parameter.AppendSql("(                                              ");
            parameter.AppendSql("  :ID,                                         ");
            parameter.AppendSql("  :SITE_ID,                                    ");
            parameter.AppendSql("  :ESTIMATE_ID,                                ");
            parameter.AppendSql("  :REGDATE,                                    ");
            parameter.AppendSql("  :CERT,                                       ");
            parameter.AppendSql("  :NAME,                                       ");
            parameter.AppendSql("  :CONTENT,                                    ");
            parameter.AppendSql("  :OPINION,                                    ");
            parameter.AppendSql("  :STATUS,                                     ");
            parameter.AppendSql("  :SITESIGN,                                   ");
            parameter.AppendSql("  SYSTIMESTAMP,                                ");
            parameter.AppendSql("  :MODIFIEDUSER,                               ");
            parameter.AppendSql("  SYSTIMESTAMP,                                ");
            parameter.AppendSql("  :CREATEDUSER,                                ");
            parameter.AppendSql("  :SWLICENSE                                   ");
            parameter.AppendSql(")                                              ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("REGDATE", dto.REGDATE);
            parameter.Add("CERT", dto.CERT);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("CONTENT", dto.CONTENT);
            parameter.Add("OPINION", dto.OPINION);
            parameter.Add("STATUS", dto.STATUS);
            parameter.Add("SITESIGN", dto.SITESIGN);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_CARD19", dto.ID);
            return FindOne(dto.ID);
        }

        public HC_OSHA_CARD19 Update(HC_OSHA_CARD19 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD19                                                  ");
            parameter.AppendSql("SET                                                                     ");
            parameter.AppendSql("  ID = :ID,                                                             ");
            parameter.AppendSql("  REGDATE = :REGDATE,                                                   ");
            parameter.AppendSql("  CERT = :CERT,                                                         ");
            parameter.AppendSql("  NAME = :NAME,                                                         ");
            parameter.AppendSql("  CONTENT = :CONTENT,                                                   ");
            parameter.AppendSql("  OPINION = :OPINION,                                                   ");
            parameter.AppendSql("  STATUS = :STATUS,                                                     ");
            parameter.AppendSql("  SITESIGN = :SITESIGN,                                                 ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                              ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                          ");
            parameter.AppendSql("WHERE ID = :ID                                                          ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("REGDATE", dto.REGDATE);
            parameter.Add("CERT", dto.CERT);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("CONTENT", dto.CONTENT);
            parameter.Add("OPINION", dto.OPINION);
            parameter.Add("STATUS", dto.STATUS);
            parameter.Add("SITESIGN", dto.SITESIGN);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD19", dto.ID);
            return FindOne(dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD19              ");
            parameter.AppendSql("WHERE ID = :ID                           ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_CARD19", id);
        }
    }
}
