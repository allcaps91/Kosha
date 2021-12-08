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
            parameter.AppendSql("SELECT DISTINCT B.*,  A.ISACTIVE, A.PARENTSITE_ID as PARENTSITE_ID, A.HASCHILD FROM HIC_OSHA_SITE A ");
            parameter.AppendSql("INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("ON A.ID = B.ID       ");
            parameter.AppendSql("WHERE A.PARENTSITE_ID = :ID      ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<HC_OSHA_SITE_MODEL>(parameter);
        }
        public HC_OSHA_SITE_MODEL FindById(long id, string userId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT B.*, A.ISACTIVE,  A.PARENTSITE_ID, C.NAME AS PARENTSITE_NAME FROM HIC_OSHA_SITE A ");
            parameter.AppendSql("INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("ON A.ID = B.ID       ");
            parameter.AppendSql("LEFT OUTER JOIN HC_SITE_VIEW C ");
            parameter.AppendSql("ON A.PARENTSITE_ID = C.ID       ");
            parameter.AppendSql("WHERE A.ID = :ID     ");
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_OSHA_SITE_MODEL>(parameter);

            //MParameter parameter = CreateParameter();
            //parameter.AppendSql("SELECT * FROM HC_SITE_VIEW ");
            //parameter.AppendSql("WHERE ID LIKE :ID     ");
            //parameter.AddLikeStatement("ID", id);
            //return ExecuteReaderSingle<HC_OSHA_SITE_MODEL>(parameter);
        }
        public List<HC_OSHA_SITE_MODEL> FIndByUserId(string userId, Role role)
        {

            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*                                                              ");
            //parameter.AppendSql("     , (                                                                ");
            //parameter.AppendSql("            SELECT WORKERTOTALCOUNT                                     ");
            //parameter.AppendSql("              FROM HIC_OSHA_PRICE                                       ");
            //parameter.AppendSql("             WHERE ESTIMATE_ID = (                                      ");
            //parameter.AppendSql("                    SELECT ESTIMATE_ID                                  ");
            //parameter.AppendSql("                      FROM HIC_OSHA_CONTRACT                            ");
            //parameter.AppendSql("                     WHERE OSHA_SITE_ID = A.ID                          ");
            //parameter.AppendSql("                       AND ISDELETED != 'Y'                             ");
            //parameter.AppendSql("                       AND ESTIMATE_ID = (SELECT MAX(ESTIMATE_ID)       ");
            //parameter.AppendSql("                                            FROM HIC_OSHA_CONTRACT      ");
            //parameter.AppendSql("                                           WHERE OSHA_SITE_ID = A.ID    ");
            //parameter.AppendSql("                                             AND ISDELETED != 'Y')      ");
            //parameter.AppendSql("               )                                                        ");
            //parameter.AppendSql("               AND ID = (                                               ");
            //parameter.AppendSql("                    SELECT MAX(ID)                                      ");
            //parameter.AppendSql("                      FROM HIC_OSHA_PRICE                               ");
            //parameter.AppendSql("                     WHERE ESTIMATE_ID = (                              ");
            //parameter.AppendSql("                        SELECT ESTIMATE_ID                              ");
            //parameter.AppendSql("                          FROM HIC_OSHA_CONTRACT                        ");
            //parameter.AppendSql("                         WHERE OSHA_SITE_ID = A.ID                      ");
            //parameter.AppendSql("                           AND ISDELETED != 'Y'                         ");
            //parameter.AppendSql("                           AND ESTIMATE_ID = (SELECT MAX(ESTIMATE_ID)   ");
            //parameter.AppendSql("                                                FROM HIC_OSHA_CONTRACT  ");
            //parameter.AppendSql("                                               WHERE OSHA_SITE_ID = A.ID");
            //parameter.AppendSql("                                                 AND ISDELETED != 'Y')  ");
            //parameter.AppendSql("                                     )                                  ");
            //parameter.AppendSql("               )                                                        ");
            //parameter.AppendSql("      ) AS WORKERTOTALCOUNT                                             ");
            parameter.AppendSql("  FROM                                                                  ");
            parameter.AppendSql("  (                                                                     ");
            parameter.AppendSql("        SELECT DISTINCT                                                 ");
            parameter.AppendSql("               B.*                                                      ");
            parameter.AppendSql("             , A.ISACTIVE                                               ");
            parameter.AppendSql("             , A.ID AS SITE_ID                                          ");
            parameter.AppendSql("             , A.PARENTSITE_ID as PARENTSITE_ID                         ");
            parameter.AppendSql("             , A.HASCHILD                                               ");
            parameter.AppendSql("             , DECODE(C.VISITWEEK, '첫째주', 1                             ");
            parameter.AppendSql("                                 , '둘째주', 2                             ");
            parameter.AppendSql("                                 , '셋째주', 3                             ");
            parameter.AppendSql("                                 , '넷째주', 4, 5) as VISITWEEK            ");
            parameter.AppendSql("             , DECODE(C.VISITDAY, '월', 1                                ");
            parameter.AppendSql("                                , '화', 2                                ");
            parameter.AppendSql("                                , '수', 3                                ");
            parameter.AppendSql("                                , '목', 4                                ");
            parameter.AppendSql("                                , '금', 5                                ");
            parameter.AppendSql("                                , '토', 6, 7)  as VISITDAY               ");
            parameter.AppendSql("             , C.MANAGEENGINEERCOUNT                                    ");
            parameter.AppendSql("             , C.MANAGEDOCTORCOUNT                                      ");
            parameter.AppendSql("             , C.MANAGENURSECOUNT                                       ");
            parameter.AppendSql("             , C.MANAGEENGINEERSTARTDATE                                ");
            parameter.AppendSql("             , C.MANAGEDOCTORSTARTDATE                                  ");
            parameter.AppendSql("             , C.MANAGENURSESTARTDATE                                   ");
            parameter.AppendSql("             , C.WORKERTOTALCOUNT                                               ");
            parameter.AppendSql("          FROM HIC_OSHA_SITE A                                          ");
            parameter.AppendSql("          INNER JOIN HC_SITE_VIEW B                                     ");
            parameter.AppendSql("                  ON A.ID = B.ID                                        ");
            parameter.AppendSql("          INNER JOIN HIC_OSHA_CONTRACT C                                ");
            parameter.AppendSql("                  ON C.OSHA_SITE_ID = A.ID                              ");
            parameter.AppendSql("                 AND C.ISDELETED = 'N'                                 ");
            parameter.AppendSql("         WHERE 1=1                                                      ");
            parameter.AppendSql("           AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("           AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("           AND C.SWLICENSE = :SWLICENSE ");

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
            parameter.AppendSql("SELECT DISTINCT B.*,  A.ISACTIVE, A.PARENTSITE_ID as PARENTSITE_ID, A.HASCHILD FROM HIC_OSHA_SITE A ");
            if (isOSha)
            {
                parameter.AppendSql("INNER JOIN HC_SITE_VIEW B ");
            }
            else
            {
                parameter.AppendSql("RIGHT OUTER JOIN HC_SITE_VIEW B ");
            }

            parameter.AppendSql("ON A.ID = B.ID       ");
            if (userId.NotEmpty())
            {
                parameter.AppendSql("AND A.ISACTIVE = 'Y' ");
            }
            if (userId.NotEmpty() )
            {
                parameter.AppendSql(" INNER JOIN HIC_OSHA_CONTRACT C ");
                parameter.AppendSql(" ON C.OSHA_SITE_ID = A.ID ");
            }
            if (userId.NotEmpty() && isSchedule)
            {
                parameter.AppendSql(" INNER JOIN        ");
                parameter.AppendSql(" (                         ");
                parameter.AppendSql(" SELECT SITE_ID FROM HIC_OSHA_SCHEDULE  ");
                parameter.AppendSql(" WHERE VISITUSERID = :USERID   ");
                parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
                parameter.AppendSql(" AND VISITRESERVEDATE = TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')");
                parameter.AppendSql(" AND ISDELETED = 'N'  ");
                parameter.AppendSql(" ) AA  ");
                parameter.AppendSql(" ON A.ID = AA.SITE_ID  ");
            }
            parameter.AppendSql("WHERE B.ID LIKE :ID     ");
                
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
            parameter.AppendSql("AND A.ISACTIVE ='Y' ");
            parameter.AppendSql("AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("AND C.SWLICENSE = :SWLICENSE ");

            parameter.AppendSql("ORDER BY B.NAME   ");
            parameter.AddLikeStatement("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            if (userId.NotEmpty())
            {
                parameter.Add("USERID", userId);
            }
            return ExecuteReader<HC_OSHA_SITE_MODEL>(parameter);


            //parameter.AppendSql("SELECT* FROM ( ");
            //parameter.AppendSql("SELECT TO_CHAR(A.MODIFIED, 'yyyy-MM-dd') AS LASTMODIFIED, B.* , A.ISACTIVE, A.PARENTSITE_ID as PARENTSITE_ID FROM HIC_OSHA_SITE A  ");
            //parameter.AppendSql("INNER JOIN HC_SITE_VIEW B          ");
            //parameter.AppendSql("ON A.ID = B.ID  ");
            //parameter.AppendSql("WHERE A.ID LIKE :ID                 ");
            //if (id.Empty())
            //{
            //    parameter.AppendSql("AND MODIFIED > SYSTIMESTAMP - 1000 ");
            //}

            //parameter.AppendSql("UNION ");
            //parameter.AppendSql("SELECT TO_CHAR(A.MODIFIED, 'yyyy-MM-dd') AS LASTMODIFIED, B.* , A.ISACTIVE, NULL PARENTSITE_ID FROM HIC_OSHA_SITE A  ");
            //parameter.AppendSql("INNER JOIN HC_SITE_VIEW B  ");
            //parameter.AppendSql("ON A.PARENTSITE_ID = B.ID  ");
            //// parameter.AppendSql("WHERE A.ID LIKE :ID  ");
            //if (id.Empty())
            //{
            //    parameter.AppendSql("WHERE MODIFIED > SYSTIMESTAMP - 1000  ");
            //}
            ////parameter.AppendSql("ORDER BY LASTMODIFIED DESC  ");
            //parameter.AppendSql(") ORDER BY NAME   ");

            //parameter.AddLikeStatement("ID", id);
            //return ExecuteReader<HC_OSHA_SITE_MODEL>(parameter);
        }


        public List<HC_OSHA_SITE_MODEL> FindByName(string name, string userId, Role role, bool isOSha, bool isSChedule)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DISTINCT B.* , A.ISACTIVE, A.PARENTSITE_ID as PARENTSITE_ID, A.HASCHILD FROM HIC_OSHA_SITE A ");
            if (isOSha)
            {
                parameter.AppendSql("INNER JOIN HC_SITE_VIEW B ");
            }
            else
            {
                parameter.AppendSql("RIGHT OUTER JOIN HC_SITE_VIEW B ");
            }

            parameter.AppendSql("ON A.ID = B.ID       ");
            if (userId.NotEmpty())
            {
                parameter.AppendSql("AND A.ISACTIVE = 'Y' ");
            }
            if (userId.NotEmpty())
            {
                parameter.AppendSql(" INNER JOIN HIC_OSHA_CONTRACT C ");
                parameter.AppendSql(" ON C.OSHA_SITE_ID = A.ID ");
            }

            if (userId.NotEmpty() && isSChedule)
            {
                parameter.AppendSql(" INNER JOIN        ");
                parameter.AppendSql(" (                         ");
                parameter.AppendSql(" SELECT SITE_ID FROM HIC_OSHA_SCHEDULE  ");
                parameter.AppendSql(" WHERE VISITUSERID = :USERID   ");
                parameter.AppendSql(" AND VISITRESERVEDATE = TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')");
                parameter.AppendSql(" AND ISDELETED = 'N'  ");
                parameter.AppendSql(" ) AA  ");
                parameter.AppendSql(" ON A.ID = AA.SITE_ID  ");
            }

            parameter.AppendSql("WHERE B.NAME LIKE :NAME     ");

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
            parameter.AppendSql("AND A.ISACTIVE ='Y' ");
            parameter.AppendSql("AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("AND C.SWLICENSE = :SWLICENSE ");

            parameter.AppendSql("ORDER BY NAME   ");
            parameter.AddLikeStatement("NAME", name);
            if (userId.NotEmpty())
            {
                parameter.Add("USERID", userId);
            }
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_SITE_MODEL>(parameter);

            //parameter.AppendSql("SELECT* FROM ( ");
            //parameter.AppendSql("SELECT TO_CHAR(A.MODIFIED, 'yyyy-MM-dd') AS LASTMODIFIED, B.* , A.ISACTIVE, A.PARENTSITE_ID as PARENTSITE_ID FROM HIC_OSHA_SITE A  ");
            //parameter.AppendSql("INNER JOIN HC_SITE_VIEW B          ");
            //parameter.AppendSql("ON A.ID = B.ID  ");
            //parameter.AppendSql("WHERE B.NAME LIKE :NAME     ");
            //if (name.Empty())
            //{
            //    parameter.AppendSql("AND MODIFIED > SYSTIMESTAMP - 1000  ");
            //}

            //parameter.AppendSql("UNION ");
            //parameter.AppendSql("SELECT TO_CHAR(A.MODIFIED, 'yyyy-MM-dd') AS LASTMODIFIED, B.* , A.ISACTIVE, NULL PARENTSITE_ID FROM HIC_OSHA_SITE A  ");
            //parameter.AppendSql("INNER JOIN HC_SITE_VIEW B  ");
            //parameter.AppendSql("ON A.PARENTSITE_ID = B.ID  ");
            ////  parameter.AppendSql("WHERE B.NAME LIKE :NAME     ");
            //if (name.Empty())
            //{
            //    parameter.AppendSql("WHERE MODIFIED > SYSTIMESTAMP - 1000  ");
            //}
            ////parameter.AppendSql("ORDER BY LASTMODIFIED DESC ");
            //parameter.AppendSql(") ORDER BY NAME   ");

            //parameter.AddLikeStatement("NAME", name);
            //return ExecuteReader<HC_OSHA_SITE_MODEL>(parameter);
        }
    }
}
