using ComBase;
using ComBase.Mvc;
using ComBase.Controls;
using HC_OSHA.Model.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.Repository.Schedule
{
    public class VIsitDocumentRepository : BaseRepository
    {
        public List<VisitDocumentModel> FindScheduleByMonth(string month, long siteId = 0)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT *                                                                                   ");
            parameter.AppendSql("  FROM                                                                                     ");
            parameter.AppendSql("  (                                                                                        ");
            parameter.AppendSql("        SELECT A.ID                                                                        ");
            parameter.AppendSql("             , AA.NAME AS SITENAME                                                         ");
            parameter.AppendSql("             , AA.ID AS SITEID                                                             ");
            parameter.AppendSql("             , A.VISITRESERVEDATE                                                          ");
            parameter.AppendSql("             , A.VISITUSERID                                                               ");
            parameter.AppendSql("             , B.name AS VISITUSERNAME                                                     ");
            parameter.AppendSql("             , B.ROLE AS VISITUSERROLE                                                     ");
            parameter.AppendSql("             , A.VISITMANAGERID AS VISITUSERID2                                            ");
            parameter.AppendSql("             , C.NAME AS ViSITUSERNAME2                                                    ");
            parameter.AppendSql("             , C.ROLE AS VISITUSERROLE2                                                    ");
            parameter.AppendSql("          FROM ADMIN.HIC_OSHA_SCHEDULE A                                                   ");
            parameter.AppendSql("          INNER JOIN HC_SITE_VIEW AA                                                       ");
            parameter.AppendSql("                  ON A.SITE_ID = AA.ID                                                     ");
            parameter.AppendSql("          LEFT OUTER JOIN HIC_USERS B                                                      ");
            parameter.AppendSql("                       ON A.VISITUSERID = b.USERID                                         ");
            parameter.AppendSql("          LEFT OUTER JOIN HIC_USERS C                                                      ");
            parameter.AppendSql("                       ON A.VISITMANAGERID = C.USERID                                      ");
            parameter.AppendSql("         WHERE A.ISDELETED = 'N'                                                           ");
            parameter.AppendSql("           AND TO_CHAR(A.VISITRESERVEDATE,'YYYY-MM') = :MONTH                              ");
            parameter.AppendSql("           AND A.SWLICENSE = :SWLICENSE1                                                   ");
            parameter.AppendSql("           AND AA.SWLICENSE = :SWLICENSE2                                                  ");
            parameter.AppendSql("           AND B.SWLICENSE = :SWLICENSE3                                                   ");
            parameter.AppendSql("           AND C.SWLICENSE = :SWLICENSE4                                                   ");
            if (siteId > 0)
            {
                parameter.AppendSql(" AND AA.ID = :SITEID ");
                parameter.Add("SITEID", siteId);
            }

            parameter.AppendSql("        ORDER BY AA.NAME, AA.ID, A.VISITRESERVEDATE                                        ");
            parameter.AppendSql("  ) A LEFT OUTER JOIN (                                                                    ");
            parameter.AppendSql("        SELECT TO_CHAR(A.SEND_DATE, 'YYYY-MM-DD') AS SENDDATE                              ");
            parameter.AppendSql("             , A.SEND_USER                                                                 ");
            parameter.AppendSql("             , A.SITE_ID                                                                   ");
            parameter.AppendSql("             , B.NAME AS SENDNAME                                                          ");
            parameter.AppendSql("          FROM                                                                             ");
            parameter.AppendSql("          (                                                                                ");
            parameter.AppendSql("                SELECT ROW_NUMBER() OVER(PARTITION BY WRTNO ORDER BY SEND_DATE DESC) AS NUM");
            parameter.AppendSql("                     , SEND_DATE                                                           ");
            parameter.AppendSql("                     , SEND_USER                                                           ");
            parameter.AppendSql("                     , SITE_ID                                                             ");
            parameter.AppendSql("                  FROM HIC_OSHA_MAIL_SEND                                                  ");
            parameter.AppendSql("                 WHERE SEND_TYPE = 'VISIT'                                                 ");
            parameter.AppendSql("                   AND SWLICENSE = :SWLICENSE5                                             ");
            parameter.AppendSql("          ) A LEFT OUTER JOIN HIC_USERS B                                                  ");
            parameter.AppendSql("                           ON A.SEND_USER = TRIM(B.USERID)                                 ");
            parameter.AppendSql("         WHERE NUM = 1                                                                     ");
            parameter.AppendSql("           AND B.SWLICENSE = :SWLICENSE6                                                   ");
            parameter.AppendSql("  ) B ON A.SITEID = B.SITE_ID                                                              ");

            parameter.Add("MONTH", month);
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE3", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE4", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE5", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE6", clsType.HosInfo.SwLicense);

            return ExecuteReader<VisitDocumentModel>(parameter);
        }

    }
}
