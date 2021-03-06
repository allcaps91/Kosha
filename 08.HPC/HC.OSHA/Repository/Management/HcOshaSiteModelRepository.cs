namespace HC.OSHA.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase;
    using ComBase.Mvc;
    using ComBase.Mvc.Utils;
    using HC.Core.Dto;
    using HC.Core.Repository;
    using HC.Core.Service;
    using HC.OSHA.Model;

    public class HcOshaSiteModelRepository : BaseRepository
    {
        HcUsersRepository hcUsersRepository;

        public HcOshaSiteModelRepository()
        {
            hcUsersRepository = new HcUsersRepository();
        }
        public List<HC_OSHA_SITE_MODEL> FindChild(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DISTINCT B.*,  A.ISACTIVE, A.PARENTSITE_ID as PARENTSITE_ID, A.HASCHILD ");
            parameter.AppendSql(" FROM HIC_OSHA_SITE A ");
            parameter.AppendSql("       INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("              ON A.ID = B.ID ");
            parameter.AppendSql("              AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("WHERE A.PARENTSITE_ID = :ID ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  AND B.DELDATE IS NULL ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<HC_OSHA_SITE_MODEL>(parameter);
        }
        public HC_OSHA_SITE_MODEL FindById(long id, string userId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT B.*, A.ISACTIVE,  A.PARENTSITE_ID, C.NAME AS PARENTSITE_NAME ");
            parameter.AppendSql("  FROM HIC_OSHA_SITE A ");
            parameter.AppendSql("       INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("             ON A.ID = B.ID ");
            parameter.AppendSql("             AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("        LEFT OUTER JOIN HC_SITE_VIEW C ");
            parameter.AppendSql("             ON A.PARENTSITE_ID = C.ID ");
            parameter.AppendSql("             AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("WHERE A.ID = :ID ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  AND B.DELDATE IS NULL ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_SITE_MODEL>(parameter);
        }
        public List<HC_OSHA_SITE_MODEL> FIndByUserId(string userId, Role role)
        {

            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.* FROM ( ");
            parameter.AppendSql("       SELECT DISTINCT B.*,A.ISACTIVE,A.ID AS SITE_ID,A.PARENTSITE_ID as PARENTSITE_ID,A.HASCHILD, ");
            parameter.AppendSql("               DECODE(C.VISITWEEK, '첫째주', 1, ");
            parameter.AppendSql("                                   '둘째주', 2, ");
            parameter.AppendSql("                                   '셋째주', 3, ");
            parameter.AppendSql("                                   '넷째주', 4, 5) as VISITWEEK, ");
            parameter.AppendSql("               DECODE(C.VISITDAY, '월', 1, ");
            parameter.AppendSql("                                  '화', 2,  ");
            parameter.AppendSql("                                  '수', 3,  ");
            parameter.AppendSql("                                  '목', 4,  ");
            parameter.AppendSql("                                  '금', 5,  ");
            parameter.AppendSql("                                  '토', 6, 7)  as VISITDAY, ");
            parameter.AppendSql("               C.MANAGEENGINEERCOUNT,C.MANAGEDOCTORCOUNT,C.MANAGENURSECOUNT,");
            parameter.AppendSql("               C.MANAGEENGINEERSTARTDATE,C.MANAGEDOCTORSTARTDATE,C.MANAGENURSESTARTDATE,");
            parameter.AppendSql("               C.WORKERTOTALCOUNT ");
            parameter.AppendSql("          FROM HIC_OSHA_SITE A ");
            parameter.AppendSql("               INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("                     ON A.ID = B.ID ");
            parameter.AppendSql("                     AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("               INNER JOIN HIC_OSHA_CONTRACT C ");
            parameter.AppendSql("                     ON C.OSHA_SITE_ID = A.ID ");
            parameter.AppendSql("                     AND C.ISDELETED = 'N' ");
            parameter.AppendSql("                     AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("         WHERE 1=1 ");
            parameter.AppendSql("           AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("           AND B.DELDATE IS NULL ");
            if (role == Role.DOCTOR)
            {
                parameter.AppendSql("AND C.MANAGEDOCTOR = :USERID ");
            }
            else if (role == Role.NURSE)
            {
                parameter.AppendSql("AND C.MANAGENURSE= :USERID ");
            }
            else if (role == Role.ENGINEER)
            {
                parameter.AppendSql("AND C.MANAGEENGINEER= :USERID ");
            }

            parameter.AppendSql("AND A.ISACTIVE = 'Y'                                   ");
            parameter.AppendSql("AND (C.TERMINATEDATE IS NULL OR TO_DATE(C.TERMINATEDATE, 'YYYY-MM-DD') >= SYSDATE)");

            parameter.AppendSql("  ) A                                                                                                                           ");

            parameter.AppendSql("ORDER BY A.NAME                                        ");

            parameter.Add("USERID", userId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_SITE_MODEL>(parameter);
        }
        public List<HC_OSHA_SITE_MODEL> FindById(string id, string userId, Role role, bool isOSha, bool isSchedule)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DISTINCT A.* , B.ISACTIVE, B.PARENTSITE_ID as PARENTSITE_ID, B.HASCHILD ");
            parameter.AppendSql("  FROM HC_SITE_VIEW A ");
            parameter.AppendSql("       INNER JOIN HIC_OSHA_SITE B ");
            parameter.AppendSql("             ON A.ID = B.ID ");
            parameter.AppendSql("             AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("             AND B.ISACTIVE='Y' ");
            parameter.AppendSql("WHERE A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  AND A.DELDATE IS NULL ");

            // 관계사 사용자는 무조건 자기 사업장만 표시
            if (clsType.User.LtdUser != "")
            {
                parameter.AppendSql(" AND A.ID='" + clsType.User.LtdUser + "' ");
            }
            else if (id != "")
            {
                parameter.AppendSql("  AND A.ID = :ID ");
            }
            else
            {
                if (userId.NotEmpty() && isSchedule)
                {
                    parameter.AppendSql(" AND A.ID IN (SELECT SITE_ID FROM HIC_OSHA_SCHEDULE ");
                    parameter.AppendSql("        WHERE VISITUSERID = :USERID ");
                    parameter.AppendSql("          AND VISITRESERVEDATE = TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ");
                    parameter.AppendSql("          AND ISDELETED = 'N' ");
                    parameter.AppendSql("          AND SWLICENSE = :SWLICENSE ) ");
                }
                else if (userId.NotEmpty() && (role == Role.DOCTOR || role == Role.NURSE || role == Role.ENGINEER))
                {
                    parameter.AppendSql(" AND A.ID IN (SELECT OSHA_SITE_ID FROM HIC_OSHA_CONTRACT ");
                    parameter.AppendSql("        WHERE CONTRACTSTARTDATE <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                    parameter.AppendSql("          AND CONTRACTENDDATE >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                    parameter.AppendSql("          AND ISDELETED = 'N'  ");
                    parameter.AppendSql("          AND (TERMINATEDATE IS NULL OR ");
                    parameter.AppendSql("                TERMINATEDATE >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "') ");
                    parameter.AppendSql("          AND SWLICENSE = :SWLICENSE ");
                    if (role == Role.DOCTOR) parameter.AppendSql(" AND MANAGEDOCTOR = :USERID) ");
                    if (role == Role.NURSE) parameter.AppendSql(" AND MANAGENURSE = :USERID) ");
                    if (role == Role.ENGINEER) parameter.AppendSql(" AND MANAGEENGINEER = :USERID) ");
                }
            }
            parameter.AppendSql(" ORDER BY A.NAME ");
            if (id != "") parameter.Add("ID", id);
            if (userId.NotEmpty()) parameter.Add("USERID", userId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_SITE_MODEL>(parameter);
        }

        // 관계사 사용자는 무조건 자기 사업장만 표시
        public List<HC_OSHA_SITE_MODEL> FindByLtduser()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DISTINCT B.*,  A.ISACTIVE, A.PARENTSITE_ID as PARENTSITE_ID, A.HASCHILD ");
            parameter.AppendSql("  FROM HIC_OSHA_SITE A ");
            parameter.AppendSql("       INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("             ON A.ID = B.ID ");
            parameter.AppendSql("             AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("             AND B.ID='" + clsType.User.LtdUser + "' ");
            parameter.AppendSql("WHERE A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" ORDER BY B.NAME ");
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_SITE_MODEL>(parameter);
        }

        public List<HC_OSHA_SITE_MODEL> FindByName(string name, string userId, Role role, bool isOSha, bool isSChedule)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DISTINCT A.* , B.ISACTIVE, B.PARENTSITE_ID as PARENTSITE_ID, B.HASCHILD ");
            parameter.AppendSql("  FROM HC_SITE_VIEW A ");
            parameter.AppendSql("       INNER JOIN HIC_OSHA_SITE B ");
            parameter.AppendSql("             ON A.ID = B.ID ");
            parameter.AppendSql("             AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("             AND B.ISACTIVE='Y' ");
            parameter.AppendSql("WHERE A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  AND A.DELDATE IS NULL ");

            // 관계사 사용자는 무조건 자기 사업장만 표시
            if (clsType.User.LtdUser != "")
            {
                parameter.AppendSql(" AND A.ID='" + clsType.User.LtdUser + "' ");
            }
            else
            {
                if (userId.NotEmpty() && isSChedule)
                {
                    parameter.AppendSql(" AND A.ID IN (SELECT SITE_ID FROM HIC_OSHA_SCHEDULE ");
                    parameter.AppendSql("        WHERE VISITUSERID = :USERID ");
                    parameter.AppendSql("          AND VISITRESERVEDATE = TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ");
                    parameter.AppendSql("          AND ISDELETED = 'N' ");
                    parameter.AppendSql("          AND SWLICENSE = :SWLICENSE ) ");
                }
                else if (userId.NotEmpty() && (role == Role.DOCTOR || role == Role.NURSE || role == Role.ENGINEER))
                {
                    parameter.AppendSql(" AND A.ID IN (SELECT OSHA_SITE_ID FROM HIC_OSHA_CONTRACT ");
                    parameter.AppendSql("        WHERE CONTRACTSTARTDATE <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                    parameter.AppendSql("          AND CONTRACTENDDATE >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                    parameter.AppendSql("          AND ISDELETED = 'N'  ");
                    parameter.AppendSql("          AND (TERMINATEDATE IS NULL OR ");
                    parameter.AppendSql("                TERMINATEDATE >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "') ");
                    parameter.AppendSql("          AND SWLICENSE = :SWLICENSE ");
                    if (role == Role.DOCTOR) parameter.AppendSql(" AND MANAGEDOCTOR = :USERID) ");
                    if (role == Role.NURSE) parameter.AppendSql(" AND MANAGENURSE = :USERID) ");
                    if (role == Role.ENGINEER) parameter.AppendSql(" AND MANAGEENGINEER = :USERID) ");
                }
                if (name != "") parameter.AppendSql(" AND A.NAME LIKE :NAME ");
            }
            parameter.AppendSql(" ORDER BY A.NAME ");
            if (name != "") parameter.AddLikeStatement("NAME", name);
            if (userId.NotEmpty())
            {
                parameter.Add("USERID", userId);
            }
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_SITE_MODEL>(parameter);
        }
    }
}
