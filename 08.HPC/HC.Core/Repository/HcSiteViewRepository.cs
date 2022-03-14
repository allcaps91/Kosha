namespace HC.Core.Repository
{
    using ComBase; 
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;
    using HC.Core.Service;
    using System.Collections.Generic;


    /// <summary>
    /// 
    /// </summary>
    public class HcSiteViewRepository : BaseRepository
    {
        public HC_SITE_VIEW FindById(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME AS BIZUPJONGNAME ");
            parameter.AppendSql("  FROM HC_SITE_VIEW A ");
            parameter.AppendSql("       LEFT OUTER JOIN HIC_CODE B ");
            parameter.AppendSql("            ON TRIM(CODE) = A.BIZUPJONG ");
            parameter.AppendSql("            AND GUBUN = '01' ");
            parameter.AppendSql("            AND SWLICENSE=:SWLICENSE ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE=:SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_SITE_VIEW>(parameter);
        }


        public List<HC_SITE_VIEW> FindAllByName(string name, string userId, Role role)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT B.* FROM HIC_OSHA_SITE A ");
            parameter.AppendSql("      INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("            ON A.ID = B.ID ");
            parameter.AppendSql("            AND B.SWLICENSE=:SWLICENSE ");
            if (userId!="all" & userId != "")
            {
                parameter.AppendSql(" INNER JOIN HIC_OSHA_CONTRACT C ");
                parameter.AppendSql("       ON C.OSHA_SITE_ID = A.ID ");
                parameter.AppendSql("       AND C.SWLICENSE=:SWLICENSE ");
            }
            parameter.AppendSql(" WHERE A.SWLICENSE=:SWLICENSE ");
            if (name.NotEmpty())
            {
                parameter.AppendSql(" AND B.NAME LIKE :NAME ");
            }

            if (userId != "all" & userId != "")
            {
                parameter.AppendSql(" AND A.ISACTIVE = 'Y' ");
                parameter.AppendSql(" AND C.ISDELETED = 'N' ");
                if (userId.NotEmpty() && role == Role.DOCTOR)
                {
                    parameter.AppendSql("AND C.MANAGEDOCTOR = :USERID ");
                }
                else if (userId.NotEmpty() && role == Role.NURSE)
                {
                    parameter.AppendSql("AND C.MANAGENURSE= :USERID ");
                }
                else if (userId.NotEmpty() && role == Role.ENGINEER)
                {
                    parameter.AppendSql("AND C.MANAGEENGINEER= :USERID ");
                }
            }
            parameter.AppendSql("   AND B.DelDate IS NULL ");
            parameter.AppendSql(" ORDER BY B.NAME ");

            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            if (userId != "all" & userId != "")
            {
                parameter.Add("USERID", userId);
            }
            if (name.NotEmpty())
            {
                parameter.AddLikeStatement("NAME", name);
            }
            return ExecuteReader<HC_SITE_VIEW>(parameter);
        }

        public List<HC_SITE_VIEW> FindAllById(string id, string userId, Role role)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT DISTINCT B.* FROM HIC_OSHA_SITE A ");
            parameter.AppendSql("        INNER JOIN HC_SITE_VIEW  B ");
            parameter.AppendSql("              ON A.ID = B.ID ");
            parameter.AppendSql("              AND B.SWLICENSE=:SWLICENSE ");
            if (userId.NotEmpty() && userId != "all")
            {
                parameter.AppendSql(" INNER JOIN HIC_OSHA_CONTRACT C ");
                parameter.AppendSql("       ON C.OSHA_SITE_ID = A.ID ");
                parameter.AppendSql("       AND C.SWLICENSE=:SWLICENSE ");
            }
            parameter.AppendSql(" WHERE 1 =1      ");
            parameter.AppendSql("   AND A.SWLICENSE=:SWLICENSE ");
            if (userId != "all")
            {
                if (id.NotEmpty())
                {
                    parameter.AppendSql(" AND A.ID  LIKE :ID       ");
                }
                 
                if (userId.NotEmpty())
                {
                    parameter.AppendSql("AND A.ISACTIVE = 'Y' ");
                }

                if (userId.NotEmpty() && role == Role.DOCTOR)
                {
                    parameter.AppendSql("AND C.MANAGEDOCTOR = :USERID ");
                }
                else if (userId.NotEmpty() && role == Role.NURSE)
                {
                    parameter.AppendSql("AND C.MANAGENURSE= :USERID ");
                }
                else if (userId.NotEmpty() && role == Role.ENGINEER)
                {
                    parameter.AppendSql("AND C.MANAGEENGINEER= :USERID ");
                }
            }
            else
            {
                if (id.NotEmpty())
                {
                    parameter.AppendSql(" AND A.ID LIKE :ID       ");
                }
                 
            }
            parameter.AppendSql("   AND B.DelDate IS NULL ");
            parameter.AppendSql(" ORDER BY B.NAME ");

            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            if (userId != "all" )
            {
                parameter.Add("USERID", userId);
                if (id.NotEmpty())
                {
                    parameter.AddLikeStatement("ID", id);
                }
            }
            else
            {
                if (id.NotEmpty())
                {
                    parameter.AddLikeStatement("ID", id);
                }
            }

            return ExecuteReader<HC_SITE_VIEW>(parameter);
        }
    }
}
