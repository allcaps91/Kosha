using ComBase;
using ComBase.Mvc;
using HC_OSHA.Model.Visit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.Repository.Visit
{
    public class DailyReportRepository : BaseRepository
    {

        public List<DailyReportSiteModel> FindVisitSite(string date)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.NAME, A.ADDRESS, A.TEL,B.SITE_ID,");
            parameter.AppendSql("       B.VISITUSER, B.VISITUSERNAME, D.ROLE, B.VISITDOCTOR, B.VISITDOCTORNAME,B.REMARK ");
            parameter.AppendSql("  FROM HC_SITE_VIEW A ");
            parameter.AppendSql("       INNER JOIN HIC_OSHA_VISIT B ");
            parameter.AppendSql("             ON A.ID = B.SITE_ID ");
            parameter.AppendSql("             AND B.ISPRECHARGE = 'N' ");
            parameter.AppendSql("             AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("       INNER JOIN HIC_USERS D ");
            parameter.AppendSql("             ON B.VISITUSER = D.USERID ");
            parameter.AppendSql("             AND D.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" WHERE B.ISDELETED = 'N' ");
            parameter.AppendSql("   AND TO_CHAR(B.VISITDATETIME, 'YYYY-MM-DD') = :dateTime ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" ORDER BY A.NAME ");

            parameter.Add("dateTime", date);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<DailyReportSiteModel>(parameter);

        }
        public List<DailyReportVisitModel> FindVisitList(string date)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM                                                                                                                                                                                           ");
            parameter.AppendSql("(                                                                                                                                                                                                       ");
            parameter.AppendSql("SELECT A.id AS visitId, B.NAME, A.STARTTIME ||'~'|| A.ENDTIME AS VISITTIME,A.STARTTIME, ");
            parameter.AppendSql("       A.VISITDOCTOR AS VisitUserId,  A.VISITDOCTORNAME AS VisitUserName, 'DOCTOR' AS ROLE,");
            parameter.AppendSql("       1 AS SEQ, A.REMARK ");
            parameter.AppendSql("  FROM HIC_OSHA_VISIT A  ");
            parameter.AppendSql("       INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("             ON A.SITE_ID = B.ID "); 
            parameter.AppendSql("             AND A.ISPRECHARGE = 'N' ");
            parameter.AppendSql("       INNER JOIN HIC_USERS C ");
            parameter.AppendSql("             ON A.VISITUSER = C.USERID ");
            parameter.AppendSql(" WHERE TO_CHAR(A.VISITDATETIME, 'YYYY-MM-DD') = :dateTime ");
            parameter.AppendSql("   AND A.VISITDOCTOR IS NOT NULL ");
            parameter.AppendSql("   AND A.ISDELETED ='N' ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" UNION ALL ");
            parameter.AppendSql("SELECT A.id,B.NAME,  A.STARTTIME ||'~'|| A.ENDTIME,  A.STARTTIME, A.VISITUSER, A.VISITUSERNAME,");
            parameter.AppendSql("       C.ROLE, 2 AS SEQ, A.REMARK ");
            parameter.AppendSql("  FROM HIC_OSHA_VISIT A ");
            parameter.AppendSql("       INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("             ON A.SITE_ID = B.ID ");
            parameter.AppendSql("             AND A.ISPRECHARGE = 'N' ");
            parameter.AppendSql("       INNER JOIN HIC_USERS C ");
            parameter.AppendSql("             ON A.VISITUSER = C.USERID ");
            parameter.AppendSql(" WHERE TO_CHAR(A.VISITDATETIME, 'YYYY-MM-DD') =  :dateTime ");
            parameter.AppendSql("   AND C.ROLE='NURSE' ");
            parameter.AppendSql("   AND A.ISDELETED ='N' ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" UNION ALL                                                                                                                                                                                               ");
            parameter.AppendSql("SELECT A.id,B.NAME,  A.STARTTIME ||'~'|| A.ENDTIME,  A.STARTTIME, A.VISITUSER, A.VISITUSERNAME,");
            parameter.AppendSql("       C.ROLE, 3 AS SEQ , A.REMARK ");
            parameter.AppendSql("  FROM HIC_OSHA_VISIT A ");
            parameter.AppendSql("       INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("             ON A.SITE_ID = B.ID ");
            parameter.AppendSql("       INNER JOIN HIC_USERS C ");
            parameter.AppendSql("             ON A.VISITUSER = C.USERID ");
            parameter.AppendSql(" WHERE TO_CHAR(A.VISITDATETIME, 'YYYY-MM-DD') = :dateTime ");
            parameter.AppendSql("   AND C.ROLE='ENGINEER' ");
            parameter.AppendSql("   AND A.ISDELETED ='N' ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND C.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(") AA ");
            parameter.AppendSql("ORDER BY AA.SEQ,AA.VISITUSERNAME,AA.STARTTIME ");

            parameter.Add("dateTime", date);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<DailyReportVisitModel>(parameter);

        }
    }
}
