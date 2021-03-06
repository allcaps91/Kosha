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
            parameter.AppendSql("SELECT * FROM ( ");
            parameter.AppendSql("        SELECT A.ID, A.ESTIMATE_ID,AA.NAME AS SITENAME,AA.ID AS SITEID,A.SENDMAIL,");
            parameter.AppendSql("               A.VISITRESERVEDATE, A.VISITUSERID, A.VISITPLACE, B.name AS VISITUSERNAME,");
            parameter.AppendSql("               B.ROLE AS VISITUSERROLE, A.VISITMANAGERID AS VISITUSERID2,");
            parameter.AppendSql("               C.NAME AS ViSITUSERNAME2, C.ROLE AS VISITUSERROLE2 ");
            parameter.AppendSql("          FROM ADMIN.HIC_OSHA_SCHEDULE A ");
            parameter.AppendSql("               INNER JOIN HC_SITE_VIEW AA ");
            parameter.AppendSql("                     ON A.SITE_ID = AA.ID ");
            parameter.AppendSql("                     AND AA.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("               LEFT  OUTER JOIN HIC_USERS B ");
            parameter.AppendSql("                     ON A.VISITUSERID = b.USERID ");
            parameter.AppendSql("                     AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("               LEFT  OUTER JOIN HIC_USERS C ");
            parameter.AppendSql("                     ON A.VISITMANAGERID = C.USERID ");
            parameter.AppendSql("                     AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("         WHERE A.ISDELETED = 'N' ");
            parameter.AppendSql("           AND TO_CHAR(A.VISITRESERVEDATE,'YYYY-MM') = :MONTH ");
            parameter.AppendSql("           AND A.SWLICENSE = :SWLICENSE ");
            if (siteId > 0)
            {
                parameter.AppendSql(" AND AA.ID = :SITEID ");
                parameter.Add("SITEID", siteId);
            }
            parameter.AppendSql("        ORDER BY AA.NAME, AA.ID, A.VISITRESERVEDATE ");
            parameter.AppendSql("  ) A LEFT OUTER JOIN ( ");
            parameter.AppendSql("        SELECT TO_CHAR(A.SEND_DATE, 'YYYY-MM-DD') AS SENDDATE ");
            parameter.AppendSql("             , A.SEND_USER ");
            parameter.AppendSql("             , A.SITE_ID ");
            parameter.AppendSql("             , B.NAME AS SENDNAME ");
            parameter.AppendSql("          FROM ( ");
            parameter.AppendSql("                SELECT ROW_NUMBER() OVER(PARTITION BY WRTNO ORDER BY SEND_DATE DESC) AS NUM ");
            parameter.AppendSql("                     , SEND_DATE ");
            parameter.AppendSql("                     , SEND_USER ");
            parameter.AppendSql("                     , SITE_ID ");
            parameter.AppendSql("                  FROM HIC_OSHA_MAIL_SEND ");
            parameter.AppendSql("                 WHERE SEND_TYPE = 'VISIT' ");
            parameter.AppendSql("                   AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("          ) A LEFT OUTER JOIN HIC_USERS B ");
            parameter.AppendSql("                           ON A.SEND_USER = TRIM(B.USERID) ");
            parameter.AppendSql("         WHERE NUM = 1 ");
            parameter.AppendSql("           AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  ) B ON A.SITEID = B.SITE_ID ");
            parameter.Add("MONTH", month);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<VisitDocumentModel>(parameter);
        }

    }
}
