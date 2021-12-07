namespace HC.OSHA.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using HC.Core.Service;
    using HC.OSHA.Dto;
    using HC_Core.Service;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard5Repository : BaseRepository
    {
        public HC_OSHA_CARD5 FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_CARD5          ");
            parameter.AppendSql("WHERE ID = :ID                        ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_CARD5>(parameter);

        }
        public List<HC_OSHA_CARD5> FindYear(long siteId, string year)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUBSTR(REGISTERDATE, 1, 7) AS REGISTERDATE");
            parameter.AppendSql("     , MAX(JOINCOUNT) AS JOINCOUNT               ");
            parameter.AppendSql("     , MAX(QUITCOUNT) AS QUITCOUNT               ");
            parameter.AppendSql("  FROM HIC_OSHA_CARD5                            ");
            parameter.AppendSql(" WHERE SWLICENSE = :SWLICENSE1                   ");
            parameter.AppendSql("   AND ESTIMATE_ID IN (                          ");
            parameter.AppendSql("        SELECT ID                                ");
            parameter.AppendSql("          FROM HIC_OSHA_ESTIMATE                 ");
            parameter.AppendSql("         WHERE SWLICENSE = :SWLICENSE2           ");
            parameter.AppendSql("           AND OSHA_SITE_ID = :SITE_ID           ");
            parameter.AppendSql("   )                                             ");
            parameter.AppendSql("   AND REGISTERDATE BETWEEN :START_DATE          ");
            parameter.AppendSql("                        AND :END_DATE            ");
            parameter.AppendSql("GROUP BY SUBSTR(REGISTERDATE, 1, 7)              ");
            parameter.AppendSql("ORDER BY REGISTERDATE                            ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("START_DATE", year +"-01-01");
            parameter.Add("END_DATE", year + "-12-31");
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD5>(parameter);
        }

        public List<HC_OSHA_CARD5> FindAll(long estimateId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS CREATEDUSER, C.NAME AS MODIFIEDUSER FROM HIC_OSHA_CARD5 A ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                   ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID              ");
            parameter.AppendSql("INNER JOIN HIC_USERS C                   ");
            parameter.AppendSql("ON A.MODIFIEDUSER = C.USERID             ");
            parameter.AppendSql("WHERE A.ESTIMATE_ID = :ESTIMATE_ID       ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE1          ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE2          ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE3          ");
            parameter.AppendSql("ORDER BY REGISTERDATE DESC               ");

            parameter.Add("ESTIMATE_ID", estimateId);
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE3", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD5>(parameter);

        }

        internal List<HC_OSHA_CARD5> Find(HC_OSHA_CARD5 item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT *                                 ");
            parameter.AppendSql("  FROM HIC_OSHA_CARD5                    ");
            parameter.AppendSql(" WHERE SITE_ID         = :SITE_ID        ");
            parameter.AppendSql("   AND ESTIMATE_ID     = :ESTIMATE_ID    ");
            parameter.AppendSql("   AND REGISTERDATE    = :REGISTERDATE   ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("SITE_ID", item.SITE_ID);
            parameter.Add("ESTIMATE_ID", item.ESTIMATE_ID);
            parameter.Add("REGISTERDATE", item.REGISTERDATE);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_CARD5>(parameter);
        }

        public HC_OSHA_CARD5 Insert(HC_OSHA_CARD5 dto)
        {
            dto.ID = GetSequenceNextVal("HC_OSHA_CARD_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_CARD5                    ");
            parameter.AppendSql("(                                             ");
            parameter.AppendSql("  ID,                                         ");
            parameter.AppendSql("  ESTIMATE_ID,                                ");
            parameter.AppendSql("  SITE_ID,                                    ");
            parameter.AppendSql("  REGISTERDATE,                               ");
            parameter.AppendSql("  JOINCOUNT,                                  ");
            parameter.AppendSql("  QUITCOUNT,                                  ");
            parameter.AppendSql("  MODIFIED,                                   ");
            parameter.AppendSql("  MODIFIEDUSER,                               ");
            parameter.AppendSql("  CREATED,                                    ");
            parameter.AppendSql("  CREATEDUSER,                                ");
            parameter.AppendSql("  SWLICENSE                                   ");
            parameter.AppendSql(")                                             ");
            parameter.AppendSql("VALUES                                        ");
            parameter.AppendSql("(                                             ");
            parameter.AppendSql("  :ID,                                        ");
            parameter.AppendSql("  :ESTIMATE_ID,                               ");
            parameter.AppendSql("  :SITE_ID,                                   ");
            parameter.AppendSql("  :REGISTERDATE,                              ");
            parameter.AppendSql("  :JOINCOUNT,                                 ");
            parameter.AppendSql("  :QUITCOUNT,                                 ");
            parameter.AppendSql("  SYSTIMESTAMP,                               ");
            parameter.AppendSql("  :MODIFIEDUSER,                              ");
            parameter.AppendSql("  SYSTIMESTAMP,                               ");
            parameter.AppendSql("  :CREATEDUSER,                               ");
            parameter.AppendSql("  :SWLICENSE                                  ");
            parameter.AppendSql(")                                             ");
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("ID", dto.ID);

            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("REGISTERDATE", dto.REGISTERDATE);
            parameter.Add("JOINCOUNT", dto.JOINCOUNT);
            parameter.Add("QUITCOUNT", dto.QUITCOUNT);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Insert("HIC_OSHA_CARD5", dto.ID);
            return FindOne(dto.ID);
        }

        public void Update(HC_OSHA_CARD5 dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CARD5                     ");
            parameter.AppendSql("SET                                       ");
            parameter.AppendSql("  REGISTERDATE = :REGISTERDATE,           ");
            parameter.AppendSql("  JOINCOUNT = :JOINCOUNT,                 ");
            parameter.AppendSql("  QUITCOUNT = :QUITCOUNT,                 ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER            ");
            parameter.AppendSql("WHERE ID = :ID                            ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("REGISTERDATE", dto.REGISTERDATE);
            parameter.Add("JOINCOUNT", dto.JOINCOUNT);
            parameter.Add("QUITCOUNT", dto.QUITCOUNT);
            parameter.Add("MODIFIEDUSER",CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_CARD5", dto.ID);

        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CARD5       ");
            parameter.AppendSql("WHERE ID = :ID                   ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE    ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_CARD5", id);
        }
    }
}
