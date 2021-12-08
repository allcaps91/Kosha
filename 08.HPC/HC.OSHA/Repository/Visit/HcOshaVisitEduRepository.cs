namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using HC.OSHA.Dto;
    using HC_Core.Service;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaVisitEduRepository : BaseRepository
    {
        public HC_OSHA_VISIT_EDU FindOne(long id)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT_EDU ");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_VISIT_EDU>(parameter);

        }
        public List<HC_OSHA_VISIT_EDU> FindAll(string startDate, string endDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.EDUDATE, B.NAME, C.CODENAME AS EDUTYPE,  A.TITLE, A.TARGET,");
            parameter.AppendSql("       A.LOCATION, D.NAME AS EDUUSERNAME ");
            parameter.AppendSql("  FROM HIC_OSHA_VISIT_EDU A ");
            parameter.AppendSql("       INNER JOIN HIC_SITE_VIEW B ");
            parameter.AppendSql("             ON A.SITE_ID = B.ID ");
            parameter.AppendSql("       INNER JOIN HIC_CODES C ");
            parameter.AppendSql("             ON A.EDUTYPE = C.CODE ");
            parameter.AppendSql("             AND C.GROUPCODE = 'VISIT_EDU_TYPE' ");
            parameter.AppendSql("       INNER JOIN HIC_USERS D ");
            parameter.AppendSql("             ON A.EDUUSERID = D.USERID ");
            parameter.AppendSql(" WHERE A.EDUDATE BETWEEN :startDate AND  :endDate ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND D.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" ORDER BY A.EDUDATE DESC ");

            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_VISIT_EDU>(parameter);
        }

        public List<HC_OSHA_VISIT_EDU> FindAll(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_VISIT_EDU ");
            parameter.AppendSql(" WHERE SITE_ID = :siteId ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" ORDER BY EDUDATE DESC ");

            parameter.Add("siteId", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_VISIT_EDU>(parameter);

        }
        public HC_OSHA_VISIT_EDU Insert(HC_OSHA_VISIT_EDU dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_VISIT_EDU (ID,SITE_ID,EDUDATE,EDUTYPE,TARGET,");
            parameter.AppendSql("       TITLE,LOCATION,EDUUSERID,EDUUSERNAME,SWLICENSE) ");
            parameter.AppendSql("VALUES (:ID,:SITE_ID,:EDUDATE,:EDUTYPE,:TARGET,:TITLE,:LOCATION,");
            parameter.AppendSql("       :EDUUSERID,:EDUUSERNAME,:SWLICENSE) ");

            dto.ID = GetSequenceNextVal("HC_OSHA_SHARED_ID_SEQ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("EDUDATE", dto.EDUDATE);
            parameter.Add("EDUTYPE", dto.EDUTYPE);
            parameter.Add("TARGET", dto.TARGET);
            parameter.Add("TITLE", dto.TITLE);
            parameter.Add("LOCATION", dto.LOCATION);
            parameter.Add("EDUUSERID", dto.EDUUSERID);
            parameter.Add("EDUUSERNAME", dto.EDUUSERNAME);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            DataSyncService.Instance.Insert("HIC_OSHA_VISIT_EDU", dto.ID);

            ExecuteNonQuery(parameter);

            return FindOne(dto.ID);
        }

        public void Update(HC_OSHA_VISIT_EDU dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_VISIT_EDU ");
            parameter.AppendSql("   SET SITE_ID = :SITE_ID, ");
            parameter.AppendSql("       EDUDATE = :EDUDATE, ");
            parameter.AppendSql("       EDUTYPE = :EDUTYPE, ");
            parameter.AppendSql("       TARGET = :TARGET, ");
            parameter.AppendSql("       TITLE = :TITLE, ");
            parameter.AppendSql("       LOCATION = :LOCATION, ");
            parameter.AppendSql("       EDUUSERID = :EDUUSERID, ");
            parameter.AppendSql("       EDUUSERNAME = :EDUUSERNAME ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("EDUDATE", dto.EDUDATE);
            parameter.Add("EDUTYPE", dto.EDUTYPE);
            parameter.Add("TARGET", dto.TARGET);
            parameter.Add("TITLE", dto.TITLE);
            parameter.Add("LOCATION", dto.LOCATION);
            parameter.Add("EDUUSERID", dto.EDUUSERID);
            parameter.Add("EDUUSERNAME", dto.EDUUSERNAME);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            DataSyncService.Instance.Update("HIC_OSHA_VISIT_EDU", dto.ID);
            ExecuteNonQuery(parameter);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_VISIT_EDU ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_VISIT_EDU", id);
        }
    }
}
