using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Utils;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Model;
using HC_Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Repository.StatusReport
{
    public class HealthCheckRepository : BaseRepository
    {

        //public List<HealthCheckDto> FindAll(long siteId)
        //{
        //    MParameter parameter = CreateParameter();
        //    parameter.AppendSql(" SELECT * FROM HC_OSHA_HEALTHCHECK ");
        //    parameter.AppendSql(" WHERE SITE_ID = :ID");
        //    parameter.AppendSql(" AND ISDELETED = 'N' ");

        //    parameter.Add("ID", siteId);
        //    return ExecuteReader<HealthCheckDto>(parameter);
        //}

        public List<HealthCheckWorkerModel> FindWorker(long siteId, string name, string dept, string panjeong, string isManageOsha, long reportId, bool isEnd)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*                                                                                                      ");
            parameter.AppendSql("     , B.END_DATE                                                                                               ");
            parameter.AppendSql("  FROM                                                                                                          ");
            parameter.AppendSql("  (                                                                                                             ");
            parameter.AppendSql("       SELECT WORKER_ID                                                                                         ");
            parameter.AppendSql("            , ID                                                                                                ");
            parameter.AppendSql("            , PTNO                                                                                              ");
            parameter.AppendSql("            , SITEID                                                                                            ");
            parameter.AppendSql("            , NAME                                                                                              ");
            parameter.AppendSql("            , WORKER_ROLE                                                                                       ");
            parameter.AppendSql("            , DEPT                                                                                              ");
            parameter.AppendSql("            , TEL                                                                                               ");
            parameter.AppendSql("            , HP                                                                                                ");
            parameter.AppendSql("            , EMAIL                                                                                             ");
            parameter.AppendSql("            , JUMIN                                                                                             ");
            parameter.AppendSql("            , PANO                                                                                              ");
            parameter.AppendSql("            , YEAR                                                                                              ");
            parameter.AppendSql("            , PANJEONG                                                                                          ");
            parameter.AppendSql("            , ISSPECIAL                                                                                         ");
            parameter.AppendSql("            , ISMANAGEOSHA2 AS ISMANAGEOSHA                                                                     ");
            parameter.AppendSql("            , OPINION AS PANNAME                                                                                ");
            parameter.AppendSql("            , DENSE_RANK() OVER (PARTITION BY WORKER_ID ORDER BY YEAR DESC) AS RANK                             ");
            parameter.AppendSql("         FROM                                                                                                   ");
            parameter.AppendSql("         (                                                                                                      ");
            parameter.AppendSql("               SELECT A.ID AS WORKER_ID                                                                         ");
            parameter.AppendSql("                    , A.*                                                                                       ");
            parameter.AppendSql("                    , B.YEAR                                                                                    ");
            parameter.AppendSql("                    , B.PANJEONG                                                                                ");
            parameter.AppendSql("                    , B.ISSPECIAL                                                                               ");
            parameter.AppendSql("                    , A.ISMANAGEOSHA AS ISMANAGEOSHA2                                                           ");
            parameter.AppendSql("                    , B.OPINION                                                                                 ");
            parameter.AppendSql("                 FROM HC_SITE_WORKER_VIEW A                                                                     ");
            parameter.AppendSql("                 LEFT OUTER JOIN                                                                                ");
            parameter.AppendSql("                 (                                                                                              ");
            parameter.AppendSql("                       SELECT MAX(YEAR) AS YEAR                                                                 ");
            parameter.AppendSql("                            , WORKER_ID                                                                         ");
            parameter.AppendSql("                            , PANJEONG                                                                          ");
            parameter.AppendSql("                            , ISSPECIAL                                                                         ");
            parameter.AppendSql("                            , OPINION                                                                           ");
            parameter.AppendSql("                         FROM HIC_OSHA_PANJEONG                                                                 ");
            parameter.AppendSql("                        WHERE SITE_ID = :SITE_ID                                                                ");
            parameter.AppendSql("                       GROUP BY WORKER_ID, PANJEONG, ISSPECIAL, OPINION                                         ");
            parameter.AppendSql("                 ) B ON A.ID = B.WORKER_ID                                                                      ");
            if (reportId > 0)
            {
                parameter.AppendSql("                 INNER JOIN HIC_OSHA_HEALTHCHECK C                                                              ");
                parameter.AppendSql("                         ON A.ID = C.WORKER_ID                                                                  ");
                parameter.AppendSql("                        AND C.ISDELETED = 'N'                                                                   ");
                parameter.AppendSql("                        AND C.REPORT_ID = :reportId                                                             ");
            }
            
            parameter.AppendSql("                WHERE A.SITEID = :SITE_ID                                                                       ");
            parameter.AppendSql("               UNION                                                                                            ");
            parameter.AppendSql("               SELECT A.ID AS WORKER_ID                                                                         ");
            parameter.AppendSql("                    , A.*                                                                                       ");
            parameter.AppendSql("                    , B.YEAR                                                                                    ");
            parameter.AppendSql("                    , B.PANJEONG                                                                                ");
            parameter.AppendSql("                    , B.ISSPECIAL                                                                               ");
            parameter.AppendSql("                    , A.ISMANAGEOSHA AS ISMANAGEOSHA2                                                           ");
            parameter.AppendSql("                    , B.OPINION                                                                                 ");
            parameter.AppendSql("                 FROM HC_SITE_WORKER_VIEW A                                                                     ");
            parameter.AppendSql("                 LEFT OUTER JOIN                                                                                ");
            parameter.AppendSql("                 (                                                                                              ");
            parameter.AppendSql("                       SELECT MAX(YEAR) AS YEAR                                                                 ");
            parameter.AppendSql("                            , WORKER_ID                                                                         ");
            parameter.AppendSql("                            , PANJEONG                                                                          ");
            parameter.AppendSql("                            , ISSPECIAL                                                                         ");
            parameter.AppendSql("                            , OPINION                                                                           ");
            parameter.AppendSql("                         FROM HIC_OSHA_PANJEONG                                                                 ");
            parameter.AppendSql("                        WHERE SITE_ID IN (SELECT CHILD_ID FROM HIC_OSHA_RELATION WHERE PARENT_ID= :SITE_ID     )");
            parameter.AppendSql("                       GROUP BY WORKER_ID, PANJEONG, ISSPECIAL, OPINION                                         ");
            parameter.AppendSql("                 ) B ON A.ID = B.WORKER_ID                                                                      ");
            if (reportId > 0)
            {
                parameter.AppendSql("                 INNER JOIN HIC_OSHA_HEALTHCHECK C                                                              ");
                parameter.AppendSql("                         ON A.ID = C.WORKER_ID                                                                  ");
                parameter.AppendSql("                        AND C.ISDELETED = 'N'                                                                   ");
                parameter.AppendSql("                        AND C.REPORT_ID = :reportId                                                             ");
            }
            parameter.AppendSql("               WHERE A.SITEID IN (SELECT CHILD_ID FROM HIC_OSHA_RELATION WHERE PARENT_ID = :SITE_ID     )      ");
            parameter.AppendSql("         )                                                                                                      ");
            parameter.AppendSql("        WHERE 1=1                                                                                               ");

            if (isManageOsha == "Y")
            {
                parameter.AppendSql("   AND ISMANAGEOSHA = :ISMANAGEOSHA                               ");
            }

            if (!name.IsNullOrEmpty())
            {
                if (name.IsNumeric())
                {
                    parameter.AppendSql("   AND PTNO LIKE :NAME ");
                }
                else
                {
                    parameter.AppendSql("   AND NAME LIKE :NAME ");
                }

            }
            if (!dept.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND DEPT = :DEPT ");
            }
            if (!panjeong.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND panjeong = :panjeong ");
            }

            parameter.AppendSql("  ) A LEFT OUTER JOIN HIC_OSHA_WORKER_END B                                    ");
            parameter.AppendSql("                   ON A.SITEID    = B.SITE_ID                                  ");
            parameter.AppendSql("                  AND A.WORKER_ID = B.WORKER_ID                                ");
            parameter.AppendSql(" WHERE 1 = 1                                                                   ");
            parameter.AppendSql("   AND A.RANK < 3                                                              ");
            if (!isEnd)
            {
                parameter.AppendSql("    AND (B.END_DATE IS NULL OR B.END_DATE >= SYSDATE)                      ");
            }

            parameter.AppendSql(" ORDER BY A.NAME, A.PANO, A.YEAR DESC                                           ");

            parameter.Add("SITE_ID", siteId);


            if (isManageOsha == "Y")
            {
                parameter.Add("ISMANAGEOSHA", isManageOsha);
            }




            if (!name.IsNullOrEmpty())
            {
                if (name.IsNumeric())
                {
                    parameter.AddLikeStatement("NAME", name);
                }
                else
                {
                    parameter.AddLikeStatement("NAME", name);
                }

            }
            if (!dept.IsNullOrEmpty())
            {
                parameter.Add("DEPT", dept);
            }
            if (!panjeong.IsNullOrEmpty())
            {
                parameter.Add("panjeong", panjeong);
            }

            if (reportId > 0)
            {
                parameter.Add("reportId", reportId);
            }

            return ExecuteReader<HealthCheckWorkerModel>(parameter);
        }

        internal List<HealthCheckWorkerModel> FindNewWorker(long siteId, string name, string dept, string panjeong, string isManageOsha, long reportId, bool isEnd, DateTime dtm)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CHILD_ID FROM HIC_OSHA_RELATION WHERE PARENT_ID = :SITE_ID");
            parameter.Add("SITE_ID", siteId);

            List<Dictionary<string, object>> childList = ExecuteReader(parameter);

            List<long> siteList = new List<long>();
            siteList.Add(siteId);
            if (childList != null && childList.Count > 0)
            {
                foreach(var id in childList)
                {
                    siteList.Add(id["CHILD_ID"].To<long>(0));
                }
            }

            parameter = CreateParameter();
            parameter.AppendSql("WITH BOHUM1 AS                                                                                                         ");
            parameter.AppendSql("(                                                                                                                      ");
            parameter.AppendSql("    SELECT PANJENGR1                                                                                                   ");
            parameter.AppendSql("         , PANJENGR2                                                                                                   ");
            parameter.AppendSql("         , PANJENGR3                                                                                                   ");
            parameter.AppendSql("         , PANJENGR4                                                                                                   ");
            parameter.AppendSql("         , PANJENGR5                                                                                                   ");
            parameter.AppendSql("         , PANJENGR6                                                                                                   ");
            parameter.AppendSql("         , PANJENGR7                                                                                                   ");
            parameter.AppendSql("         , PANJENGR8                                                                                                   ");
            parameter.AppendSql("         , PANJENGR9                                                                                                   ");
            parameter.AppendSql("         , PANJENGR10                                                                                                  ");
            parameter.AppendSql("         , PANJENGR11                                                                                                  ");
            parameter.AppendSql("         , PANJENGR12                                                                                                  ");
            parameter.AppendSql("         , PANJENGD11                                                                                                  ");
            parameter.AppendSql("         , PANJENGD12                                                                                                  ");
            parameter.AppendSql("         , PANJENGD13                                                                                                  ");
            parameter.AppendSql("         , PANJENGD21                                                                                                  ");
            parameter.AppendSql("         , PANJENGD22                                                                                                  ");
            parameter.AppendSql("         , PANJENGD23                                                                                                  ");
            parameter.AppendSql("         , PANJENGU1                                                                                                   ");
            parameter.AppendSql("         , PANJENGU2                                                                                                   ");
            parameter.AppendSql("         , PANJENGU3                                                                                                   ");
            parameter.AppendSql("         , PANJENGU4                                                                                                   ");
            parameter.AppendSql("         , ROWID                                                                                                       ");
            parameter.AppendSql("         , WRTNO                                                                                                       ");
            parameter.AppendSql("         , PANJENG                                                                                                     ");
            parameter.AppendSql("         , SOGEN                                                                                               ");
            parameter.AppendSql("         , PANJENGDRNO AS PANDRNO                                                                                      ");
            parameter.AppendSql("      FROM ADMIN.HIC_RES_BOHUM1                                                                                   ");
            parameter.AppendSql("),                                                                                                                        ");
            parameter.AppendSql("BOHUM2 AS(                                                                                                                ");
            parameter.AppendSql("    SELECT ROWID                                                                                                          ");
            parameter.AppendSql("         , WRTNO                                                                                                          ");
            parameter.AppendSql("      FROM ADMIN.HIC_RES_BOHUM2                                                                                     ");
            parameter.AppendSql(")                                                                                                                         ");
            parameter.AppendSql("SELECT DISTINCT                                                                                                                   ");
            parameter.AppendSql("       A.PANO                                                                                                             ");
            parameter.AppendSql("     , A.SNAME                                                                                                            ");
            parameter.AppendSql("     , A.GJYEAR AS YEAR                                                                                                   ");
            parameter.AppendSql("     , A.RNUM                                                                                                             ");
            parameter.AppendSql("     , A.GJCHASU                                                                                                          ");
            parameter.AppendSql("     , A.GJJONG--  41 = 생애, 나머지 = 일반                                                                                  ");
            parameter.AppendSql("     , B.SOGEN                                                                                                  ");
            parameter.AppendSql("     , B.ROWID AS BROWID                                                                                                  ");
            parameter.AppendSql("     , C.ROWID AS CROWID                                                                                                  ");
            parameter.AppendSql("     , A.JEPDATE                                                                                                          ");
            parameter.AppendSql("     , A.IPSADATE                                                                                                         ");
            parameter.AppendSql("     , A.UCODES                                                                                                           ");
            parameter.AppendSql("     , A.AGE                                                                                                              ");
            parameter.AppendSql("     , AA.GONGJENG                                                                                                        ");
            parameter.AppendSql("     , AA.BUSENAME                                                                                                        ");
            parameter.AppendSql("     , A.JUMIN                                                                                                            ");
            parameter.AppendSql("     , A.WORKER_ID                                                                                                        ");
            parameter.AppendSql("     , A.PTNO                                                                                                             ");
            parameter.AppendSql("     , A.SITEID                                                                                                           ");
            parameter.AppendSql("     , A.NAME                                                                                                             ");
            parameter.AppendSql("     , A.DEPT                                                                                                             ");
            parameter.AppendSql("     , A.ISMANAGEOSHA                                                                                                     ");
            parameter.AppendSql("     , A.WRTNO                                                                                                            ");
            parameter.AppendSql("     , A.GJBANGI                                                                                                          ");
            parameter.AppendSql("     , A.SEX                                                                                                              ");
            parameter.AppendSql("     , A.LTDCODE                                                                                                          ");
            parameter.AppendSql("     , B.PANJENG                                                                                                          ");
            parameter.AppendSql("     , B.PANJENGR1                                                                                                        ");
            parameter.AppendSql("     , B.PANJENGR2                                                                                                        ");
            parameter.AppendSql("     , B.PANJENGR3                                                                                                        ");
            parameter.AppendSql("     , B.PANJENGR4                                                                                                        ");
            parameter.AppendSql("     , B.PANJENGR5                                                                                                        ");
            parameter.AppendSql("     , B.PANJENGR6                                                                                                        ");
            parameter.AppendSql("     , B.PANJENGR7                                                                                                        ");
            parameter.AppendSql("     , B.PANJENGR8                                                                                                        ");
            parameter.AppendSql("     , B.PANJENGR9                                                                                                        ");
            parameter.AppendSql("     , B.PANJENGR10                                                                                                       ");
            parameter.AppendSql("     , B.PANJENGR11                                                                                                       ");
            parameter.AppendSql("     , B.PANJENGR12                                                                                                       ");
            parameter.AppendSql("     , B.PANJENGD11                                                                                                       ");
            parameter.AppendSql("     , B.PANJENGD12                                                                                                       ");
            parameter.AppendSql("     , B.PANJENGD13                                                                                                       ");
            parameter.AppendSql("     , B.PANJENGD21                                                                                                       ");
            parameter.AppendSql("     , B.PANJENGD22                                                                                                       ");
            parameter.AppendSql("     , B.PANJENGD23                                                                                                       ");
            parameter.AppendSql("     , B.PANJENGU1                                                                                                        ");
            parameter.AppendSql("     , B.PANJENGU2                                                                                                        ");
            parameter.AppendSql("     , B.PANJENGU3                                                                                                        ");
            parameter.AppendSql("     , B.PANJENGU4                                                                                                        ");
            parameter.AppendSql("     , C.WRTNO AS CWRTNO                                                                                                  ");
            parameter.AppendSql("     , D.END_DATE                                                                                                         ");
            parameter.AppendSql("     , E.REMARK                                                                                                           ");
            parameter.AppendSql("  FROM                                                                                                                    ");
            parameter.AppendSql("  (                                                                                                                       ");
            parameter.AppendSql("        SELECT A.ID AS WORKER_ID                                                                                          ");
            parameter.AppendSql("             , A.PTNO                                                                                                     ");
            parameter.AppendSql("             , A.SITEID                                                                                                   ");
            parameter.AppendSql("             , A.NAME                                                                                                     ");
            parameter.AppendSql("             , A.DEPT                                                                                                     ");
            parameter.AppendSql("             , A.ISMANAGEOSHA                                                                                             ");
            parameter.AppendSql("             , A.JUMIN                                                                                                    ");
            parameter.AppendSql("             , B.WRTNO                                                                                                    ");
            parameter.AppendSql("             , B.PANO                                                                                                     ");
            parameter.AppendSql("             , B.SNAME                                                                                                    ");
            parameter.AppendSql("             , B.JEPDATE                                                                                                  ");
            parameter.AppendSql("             , B.GJJONG                                                                                                   ");
            parameter.AppendSql("             , B.GJCHASU                                                                                                  ");
            parameter.AppendSql("             , B.UCODES                                                                                                   ");
            parameter.AppendSql("             , B.GJYEAR                                                                                                   ");
            parameter.AppendSql("             , B.GJBANGI                                                                                                  ");
            parameter.AppendSql("             , B.SEX                                                                                                      ");
            parameter.AppendSql("             , B.IPSADATE                                                                                                 ");
            parameter.AppendSql("             , B.AGE                                                                                                      ");
            parameter.AppendSql("             , B.LTDCODE                                                                                                  ");
            parameter.AppendSql("             , B.RNUM                                                                                                     ");
            parameter.AppendSql("          FROM HC_SITE_WORKER_VIEW A                                                                                      ");

            if(reportId > 0)
            {
                parameter.AppendSql("          INNER JOIN HIC_OSHA_HEALTHCHECK C                                                                           ");
                parameter.AppendSql("                  ON A.ID = C.WORKER_ID                                                                               ");
                parameter.AppendSql("                 AND C.ISDELETED = 'N'                                                                                ");
                parameter.AppendSql("                 AND C.REPORT_ID = :reportId                                                                          ");

                parameter.Add("reportId", reportId);
            }

            parameter.AppendSql("          LEFT OUTER JOIN                                                                                                 ");
            parameter.AppendSql("          (                                                                                                               ");
            parameter.AppendSql("                SELECT *                                                                                                  ");
            parameter.AppendSql("                  FROM                                                                                                    ");
            parameter.AppendSql("                  (                                                                                                       ");
            parameter.AppendSql("                        SELECT A.*                                                                                        ");
            //parameter.AppendSql("                             , ROW_NUMBER() OVER(PARTITION BY PANO, A.GJYEAR ORDER BY SNAME, PANO, A.GJYEAR DESC) AS RNUM ");
            parameter.AppendSql("                             , DENSE_RANK() OVER (PARTITION BY PANO ORDER BY  A.GJYEAR DESC) AS RNUM ");
            parameter.AppendSql("                          FROM                                                                                            ");
            parameter.AppendSql("                          (                                                                                               ");
            parameter.AppendSql("                                SELECT B.WRTNO                                                                            ");
            parameter.AppendSql("                                     , B.PANO                                                                             ");
            parameter.AppendSql("                                     , B.SNAME                                                                            ");
            parameter.AppendSql("                                     , TO_CHAR(B.JEPDATE, 'YYYY-MM-DD') AS JEPDATE                                        ");
            parameter.AppendSql("                                     , B.GJJONG                                                                           ");
            parameter.AppendSql("                                     , B.GJCHASU                                                                          ");
            parameter.AppendSql("                                     , B.UCODES                                                                           ");
            parameter.AppendSql("                                     , B.GJYEAR                                                                           ");
            parameter.AppendSql("                                     , B.GJBANGI                                                                          ");
            parameter.AppendSql("                                     , B.SEX                                                                              ");
            parameter.AppendSql("                                     , TO_CHAR(B.IPSADATE, 'YYYY-MM-DD') AS IPSADATE                                      ");
            parameter.AppendSql("                                     , B.AGE                                                                              ");
            parameter.AppendSql("                                     , TRIM(B.LTDCODE) AS LTDCODE                                                         ");
            parameter.AppendSql("                                  FROM ADMIN.HIC_JEPSU B                                                            ");
            parameter.AppendSql("                                 WHERE 1 = 1                                                                              ");
            parameter.AppendSql("                                   AND B.JEPDATE >= TO_DATE(:SDATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("                                   AND B.JEPDATE <= TO_DATE(:EDATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("                                   AND B.DELDATE IS NULL                                                                  ");
            parameter.AppendSql("                                   AND B.LTDCODE IN (:SITE_ID)                                                            ");
            parameter.AppendSql("                                   AND B.GJJONG  IN ('11', '12', '14', '23', '41')                                        ");
            parameter.AppendSql("                                   AND B.PANJENGDRNO IS NOT NULL                                                          ");
            parameter.AppendSql("                                UNION ALL                                                                                 ");
            parameter.AppendSql("                                SELECT WRTNO                                                                              ");
            parameter.AppendSql("                                     , PANO                                                                               ");
            parameter.AppendSql("                                     , SNAME                                                                              ");
            parameter.AppendSql("                                     , TO_CHAR(JEPDATE, 'YYYY-MM-DD') AS JEPDATE                                          ");
            parameter.AppendSql("                                     , GJJONG                                                                             ");
            parameter.AppendSql("                                     , GJCHASU                                                                            ");
            parameter.AppendSql("                                     , UCODES                                                                             ");
            parameter.AppendSql("                                     , GJYEAR                                                                             ");
            parameter.AppendSql("                                     , GJBANGI                                                                            ");
            parameter.AppendSql("                                     , SEX                                                                                ");
            parameter.AppendSql("                                     , TO_CHAR(B.IPSADATE, 'YYYY-MM-DD') AS IPSADATE                                      ");
            parameter.AppendSql("                                     , B.AGE                                                                              ");
            parameter.AppendSql("                                     , TRIM(B.LTDCODE) AS LTDCODE                                                         ");
            parameter.AppendSql("                                  FROM ADMIN.HIC_JEPSU B                                                            ");
            parameter.AppendSql("                                 WHERE 1 = 1                                                                              ");
            parameter.AppendSql("                                   AND B.JEPDATE >= TO_DATE(:SDATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("                                   AND B.JEPDATE <= TO_DATE(:EDATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("                                   AND B.LTDCODE IN (:SITE_ID)                                                            ");
            parameter.AppendSql("                                   AND B.GJJONG  IN ('16', '17', '18', '19', '44', '45')                                  ");
            parameter.AppendSql("                                   AND B.GJCHASU = '2'                                                                    ");
            parameter.AppendSql("                                   AND(UCodes IS NULL OR UCodes = 'ZZZ')                                                  ");
            parameter.AppendSql("                                   AND DELDATE IS NULL                                                                    ");
            parameter.AppendSql("                          ) A                                                                                             ");
            parameter.AppendSql("                  ) A                                                                                                     ");
            parameter.AppendSql("                 WHERE RNUM < 3                                                                                           ");
            parameter.AppendSql("          ) B ON A.PANO = B.PANO                                                                                          ");
            parameter.AppendSql("             AND A.SITEID = B.LTDCODE                                                                                     ");
            parameter.AppendSql("        WHERE A.SITEID IN (:SITE_ID)                                                                                      ");
            parameter.AppendSql("  ) A LEFT OUTER JOIN ADMIN.HIC_PATIENT AA                                                                          ");
            parameter.AppendSql("                   ON A.PANO = AA.PANO                                                                                    ");
            parameter.AppendSql("      LEFT OUTER JOIN BOHUM1 B                                                                                            ");
            parameter.AppendSql("                   ON A.WRTNO = B.WRTNO                                                                                   ");
            parameter.AppendSql("      LEFT OUTER JOIN BOHUM2 C                                                                                            ");
            parameter.AppendSql("                   ON A.WRTNO = C.WRTNO                                                                                   ");
            parameter.AppendSql("      LEFT OUTER JOIN HIC_OSHA_WORKER_END D                                                                               ");
            parameter.AppendSql("                   ON A.SITEID = D.SITE_ID                                                                                ");
            parameter.AppendSql("                  AND A.WORKER_ID = D.WORKER_ID                                                                           ");
            parameter.AppendSql("      LEFT OUTER JOIN(                                                                                                    ");
            parameter.AppendSql("            SELECT *                                                                                                      ");
            parameter.AppendSql("              FROM                                                                                                        ");
            parameter.AppendSql("              (                                                                                                           ");
            parameter.AppendSql("                    SELECT SITE_ID                                                                                        ");
            parameter.AppendSql("                         , WORKER_ID                                                                                      ");
            parameter.AppendSql("                         , REMARK                                                                                         ");
            parameter.AppendSql("                         , ID                                                                                             ");
            parameter.AppendSql("                         , ROW_NUMBER() OVER(PARTITION BY SITE_ID, WORKER_ID ORDER BY SITE_ID, WORKER_ID, ID DESC) AS RNUM");
            parameter.AppendSql("                       FROM HIC_OSHA_PATIENT_REMARK                                                                       ");
            parameter.AppendSql("              ) A                                                                                                         ");
            parameter.AppendSql("            WHERE RNUM = 1                                                                                                ");
            parameter.AppendSql("      ) E ON A.SITEID = E.SITE_ID                                                                                         ");
            parameter.AppendSql("         AND A.WORKER_ID = E.WORKER_ID                                                                                    ");
            parameter.AppendSql("  WHERE 1 = 1                                                                                                             ");

            if(!isEnd)
            {
                parameter.AppendSql("    AND (D.END_DATE IS NULL OR D.END_DATE >= SYSDATE)                                                                  ");
            }
            if (isManageOsha == "Y")
            {
                parameter.AppendSql("AND A.ISMANAGEOSHA = :ISMANAGEOSHA                                                                                       ");
                parameter.Add("ISMANAGEOSHA", isManageOsha);
            }
            if (!name.IsNullOrEmpty())
            {
                if (name.IsNumeric())
                {
                    parameter.AppendSql("AND A.PTNO LIKE :NAME ");
                }
                else
                {
                    parameter.AppendSql("AND A.NAME LIKE :NAME ");
                }
                parameter.AddLikeStatement("NAME", name);
            }
            if (!dept.IsNullOrEmpty())
            {
                parameter.AppendSql("AND A.DEPT = :DEPT ");
                parameter.Add("DEPT", dept);
            }
            //if (!panjeong.IsNullOrEmpty())
            //{
            //    parameter.AppendSql("AND B.PANJENG = :panjeong ");
            //    parameter.Add("panjeong", panjeong);
            //}
            parameter.AppendSql("ORDER BY A.NAME, A.PANO, A.GJYEAR DESC                                                                                    ");

            parameter.AddInStatement("SITE_ID", siteList.ToArray());
            parameter.Add("SDATE", string.Concat(dtm.AddYears(-5).ToString("yyyy"), "-01-01"));
            parameter.Add("EDATE", string.Concat(dtm.ToString("yyyy"), "-12-31"));

            return ExecuteReader<HealthCheckWorkerModel>(parameter);
        }

        public List<HealthCheckDto> FindAll(string worker_id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT A.*, B.NAME AS MODIFIEDUSER FROM HIC_OSHA_HEALTHCHECK A ");
            parameter.AppendSql(" INNER JOIN HIC_USERS B ");
            parameter.AppendSql(" ON A.MODIFIEDUSER = B.USERID ");
            parameter.AppendSql(" WHERE A.WORKER_ID = :ID");
            parameter.AppendSql(" AND A.ISDELETED = 'N' ");
            parameter.AppendSql(" ORDER BY A.CHARTDATE DESC, A.CHARTTIME DESC ");

            parameter.Add("ID", worker_id);

            return ExecuteReader<HealthCheckDto>(parameter);
        }
        public List<HealthCheckDto> FindAll(long siteId, long reprotid, string startDate, string endDate, bool isDelete)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT A.*                        ");
            parameter.AppendSql("   FROM HIC_OSHA_HEALTHCHECK A     ");
            parameter.AppendSql("  WHERE 1 = 1                      ");
            if(!isDelete)
            {
                parameter.AppendSql("    AND A.ISDELETED = 'N'          ");
            }
            parameter.AppendSql("    AND A.REPORT_ID = :REPORTID    ");
            parameter.AppendSql(" ORDER BY A.CHARTDATE, A.CHARTTIME ");

            parameter.Add("REPORTID", reprotid);

            return ExecuteReader<HealthCheckDto>(parameter);
        }
        public List<HealthCheckDto> FindAll(long reprotid)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT A.* FROM HIC_OSHA_HEALTHCHECK A ");
            parameter.AppendSql(" WHERE ");
            parameter.AppendSql(" A.ISDELETED = 'N' ");
            parameter.AppendSql(" AND A.REPORT_ID = :REPORTID ");
            parameter.AppendSql(" ORDER BY A.CHARTDATE, A.CHARTTIME");

            parameter.Add("REPORTID", reprotid);

            return ExecuteReader<HealthCheckDto>(parameter);
        }

        public HealthCheckDto FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT  *  ");
            parameter.AppendSql("  FROM HIC_OSHA_HEALTHCHECK A");
            parameter.AppendSql(" WHERE A.ID = :ID");

            parameter.Add("ID", id);
            HealthCheckDto dto = ExecuteReaderSingle<HealthCheckDto>(parameter);
            return dto;
        }


      
        public HealthCheckDto Update(HealthCheckDto dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_HEALTHCHECK");
            parameter.AppendSql("    SET ");         
            parameter.AppendSql("       name = :name");
            parameter.AppendSql("      , dept = :dept");
            parameter.AppendSql("      , gender = :gender");
            parameter.AppendSql("      , age = :age");
            parameter.AppendSql("      , content = :content");
            parameter.AppendSql("      , suggestion = :suggestion");
            parameter.AppendSql("      , bpl = :bpl");
            parameter.AppendSql("      , bpr = :bpr");
            parameter.AppendSql("      , bst = :bst");
            parameter.AppendSql("      , dan = :dan");
            parameter.AppendSql("      , WEIGHT = :WEIGHT");
            parameter.AppendSql("      , ALCHOL = :ALCHOL");
            parameter.AppendSql("      , SMOKE = :SMOKE");
            parameter.AppendSql("      , BMI = :BMI");
            parameter.AppendSql("      , EXAM = :EXAM");
            parameter.AppendSql("      , CHARTDATE = :CHARTDATE");
            parameter.AppendSql("      , CHARTTIME = :CHARTTIME");
            parameter.AppendSql("      , MODIFIED = systimestamp");
            parameter.AppendSql("      , MODIFIEDUSER = :MODIFIEDUSER");

            parameter.AppendSql("WHERE ID = :ID                     ");

            parameter.Add("id", dto.id);      
            parameter.Add("name", dto.name);
            parameter.Add("dept", dto.dept);
            parameter.Add("gender", dto.gender);
            parameter.Add("age", dto.age);
            parameter.Add("content", dto.content);
            parameter.Add("suggestion", dto.suggestion);
            parameter.Add("bpl", dto.bpl);
            parameter.Add("bpr", dto.bpr);
            parameter.Add("bst", dto.bst);
            parameter.Add("dan", dto.dan);
            parameter.Add("WEIGHT", dto.WEIGHT);
            parameter.Add("ALCHOL", dto.ALCHOL);
            parameter.Add("SMOKE", dto.SMOKE);
            parameter.Add("BMI", dto.BMI);
            parameter.Add("EXAM", dto.EXAM);
            parameter.Add("CHARTDATE", dto.CHARTDATE);
            parameter.Add("CHARTTIME", dto.CHARTTIME);
          
            if (dto.MODIFIEDUSER.IsNullOrEmpty())
            {
                parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            }
            else
            {
                parameter.Add("MODIFIEDUSER", dto.CREATEDUSER);
            }
    //        parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_HEALTHCHECK", dto.id);
            return FindOne(dto.id);

        }
        public HealthCheckDto Insert(HealthCheckDto dto)
        {
            dto.id = GetSequenceNextVal("HC_OSHA_HEALTHCHECK_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_HEALTHCHECK");
            parameter.AppendSql("(");
            parameter.AppendSql("    id");
            parameter.AppendSql("  , worker_id");
            parameter.AppendSql("  , site_id");
            parameter.AppendSql("  , name");
            parameter.AppendSql("  , dept");
            parameter.AppendSql("  , gender");
            parameter.AppendSql("  , age");
            parameter.AppendSql("  , content");
            parameter.AppendSql("  , suggestion");
            parameter.AppendSql("  , bpl");
            parameter.AppendSql("  , bpr");
            parameter.AppendSql("  , bst");
            parameter.AppendSql("  , dan");
            parameter.AppendSql("  , WEIGHT");
            parameter.AppendSql("  , ALCHOL");
            parameter.AppendSql("  , SMOKE");
            parameter.AppendSql("  , BMI");
            parameter.AppendSql("  , EXAM");
            parameter.AppendSql("  , IsDeleted");
            parameter.AppendSql("  , REPORT_ID");
            parameter.AppendSql("  , BREPORT_ID");
            parameter.AppendSql("  , ISDOCTOR");
            parameter.AppendSql("  , CHARTDATE");
            parameter.AppendSql("  , CHARTTIME");            
            parameter.AppendSql("  , MODIFIED");
            parameter.AppendSql("  , MODIFIEDUSER");
            parameter.AppendSql("  , CREATED");
            parameter.AppendSql("  , CREATEDUSER");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :id");
            parameter.AppendSql("  , :worker_id");
            parameter.AppendSql("  , :site_id");
            parameter.AppendSql("  , :name");
            parameter.AppendSql("  , :dept");
            parameter.AppendSql("  , :gender");
            parameter.AppendSql("  , :age");
            parameter.AppendSql("  , :content");
            parameter.AppendSql("  , :suggestion");
            parameter.AppendSql("  , :bpl");
            parameter.AppendSql("  , :bpr");
            parameter.AppendSql("  , :bst");
            parameter.AppendSql("  , :dan");
            parameter.AppendSql("  , :WEIGHT");
            parameter.AppendSql("  , :ALCHOL");
            parameter.AppendSql("  , :SMOKE");
            parameter.AppendSql("  , :BMI");
            parameter.AppendSql("  , :EXAM");
            parameter.AppendSql("  , :IsDeleted");
            parameter.AppendSql("  , :REPORT_ID");
            parameter.AppendSql("  , :BREPORT_ID");
            parameter.AppendSql("  , :ISDOCTOR");
            parameter.AppendSql("  , :CHARTDATE");
            parameter.AppendSql("  , :CHARTTIME");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :MODIFIEDUSER");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :CREATEDUSER");
            parameter.AppendSql(") ");

            parameter.Add("id", dto.id);
            parameter.Add("worker_id", dto.worker_id);
            parameter.Add("site_id", dto.site_id);
            parameter.Add("name", dto.name);
            parameter.Add("dept", dto.dept);
            parameter.Add("gender", dto.gender);
            parameter.Add("age", dto.age);
            parameter.Add("content", dto.content);
            parameter.Add("suggestion", dto.suggestion);
            parameter.Add("bpl", dto.bpl);
            parameter.Add("bpr", dto.bpr);
            parameter.Add("bst", dto.bst);
            parameter.Add("dan", dto.dan);
            parameter.Add("WEIGHT", dto.WEIGHT);
            parameter.Add("ALCHOL", dto.ALCHOL);
            parameter.Add("SMOKE", dto.SMOKE);
            parameter.Add("BMI", dto.BMI);
            parameter.Add("EXAM", dto.EXAM);
            parameter.Add("IsDeleted", 'N');
            parameter.Add("REPORT_ID", dto.REPORT_ID);
            parameter.Add("BREPORT_ID", dto.REPORT_ID);
            parameter.Add("ISDOCTOR", dto.ISDOCTOR);
            parameter.Add("CHARTDATE", dto.CHARTDATE);
            parameter.Add("CHARTTIME", dto.CHARTTIME);

            if (dto.CREATEDUSER.IsNullOrEmpty())
            {
                parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
                parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            }
            else
            {
                parameter.Add("MODIFIEDUSER", dto.CREATEDUSER);
                parameter.Add("CREATEDUSER", dto.CREATEDUSER);
            }


            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_HEALTHCHECK", dto.id);
            return FindOne(dto.id);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_HEALTHCHECK    ");
            parameter.AppendSql("SET ISDELETED = 'Y'  ");
            parameter.AppendSql("WHERE ID = :ID  ");
            parameter.Add("ID", id);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_HEALTHCHECK", id);
        }

        internal void PrintDeleteUpdate(long id, string isDeleted)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_HEALTHCHECK    ");
            parameter.AppendSql("   SET ISDELETED = :ISDELETED  ");
            parameter.AppendSql(" WHERE ID = :ID                ");

            parameter.Add("ID", id);
            parameter.Add("ISDELETED", isDeleted);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_HEALTHCHECK", id);
        }
    }
}

