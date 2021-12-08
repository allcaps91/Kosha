using System.Collections.Generic;
using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Utils;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;

namespace HC.OSHA.Repository
{
    public class ScheduleModelRepository : BaseRepository
    {
        public List<HC_OSHA_SCHEDULE> FindByToday()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_SCHEDULE   ");
            parameter.AppendSql(" WHERE SWLICENSE = :SWLICENSE ");
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_OSHA_SCHEDULE>(parameter);
        }
        /// <summary>
        /// 미방문 사업장 목록
        /// </summary>
        /// <param name="visitUserId"></param>
        /// <param name="visitStartDate"></param>
        /// <param name="visitEndDate"></param>
        /// <returns></returns>
        public List<UnvisitSiteModel> FindUnvisitSiteList(string visitUserId, string visitStartDate, string visitEndDate, string siteIdOrname)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID AS SCHEDULE_ID, C.NAME, A.SITE_ID, A.VISITRESERVEDATE, A.VISITUSERNAME	, 0 as VISIT_ID	, A.DEPARTUREDATETIME , 'N' AS ISPRECHARGE			");
            parameter.AppendSql("	FROM HIC_OSHA_SCHEDULE A        ");                   
            parameter.AppendSql("	INNER JOIN HC_SITE_VIEW C       ");
            parameter.AppendSql("	ON A.SITE_ID = C.ID	            ");
            parameter.AppendSql("WHERE                              ");
            parameter.AppendSql("	A.VISITRESERVEDATE BETWEEN TO_DATE(:visitStartDate , 'YYYY-MM-DD') AND TO_DATE(:visitEndDate, 'YYYY-MM-DD')  ");
            parameter.AppendSql("	AND A.ISDELETED ='N'            ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE1   ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE2   ");
            if (siteIdOrname.NotEmpty())
            {
                if (siteIdOrname.IsNumeric())
                {
                    parameter.AppendSql("        AND A.SITE_ID  LIKE :siteIdOrname    ");
                }
                else
                {
                    parameter.AppendSql("        AND C.NAME  LIKE :siteIdOrname    ");
                }
            }
            if (visitUserId.NotEmpty())
            {
                parameter.AppendSql("	AND	A.VISITUSERID = :visitUserId                                                             ");
            }
            parameter.AppendSql("	 AND A.id not in                        ");
            parameter.AppendSql("	 (                                      ");
            parameter.AppendSql("	select a.id from HIC_OSHA_SCHEDULE a    ");
            parameter.AppendSql("	inner join HIC_OSHA_VISIT b             ");
            parameter.AppendSql("	on a.id = b.schedule_id                 ");
            parameter.AppendSql("	where a.isdeleted = 'N'                 ");
            parameter.AppendSql("	and b.isdeleted = 'N'                   ");
            parameter.AppendSql("   AND a.SWLICENSE = :SWLICENSE3           ");
            parameter.AppendSql("   AND b.SWLICENSE = :SWLICENSE4           ");
            parameter.AppendSql("	)                                       ");
            parameter.AppendSql(" UNION ALL                                 ");
            parameter.AppendSql("	SELECT A.ID AS SCHEDULE_ID,  C.NAME, A.SITE_ID, A.VISITRESERVEDATE, B.VISITUSERNAME  , B.ID AS VISIT_ID, A.DEPARTUREDATETIME , B.ISPRECHARGE         ");
            parameter.AppendSql("	FROM HIC_OSHA_SCHEDULE A          ");
            parameter.AppendSql("	INNER JOIN HIC_OSHA_VISIT B          ");
            parameter.AppendSql("	ON A.ID = B.SCHEDULE_ID    AND A.ISDELETED = 'N'          ");
            parameter.AppendSql("	INNER JOIN HC_SITE_VIEW C          ");
            parameter.AppendSql("	ON A.SITE_ID = C.ID          ");
            parameter.AppendSql("	WHERE                         ");
            parameter.AppendSql("	A.VISITRESERVEDATE BETWEEN TO_DATE(:visitStartDate , 'YYYY-MM-DD') AND TO_DATE(:visitEndDate, 'YYYY-MM-DD')                      ");
            if (siteIdOrname.NotEmpty())
            {
                if (siteIdOrname.IsNumeric())
                {
                    parameter.AppendSql("        AND A.SITE_ID  LIKE :siteIdOrname    ");
                }
                else
                {
                    parameter.AppendSql("        AND C.NAME  LIKE :siteIdOrname    ");
                }
            }
            if (visitUserId.NotEmpty())
            {
                parameter.AppendSql("	AND	A.VISITUSERID = :visitUserId                                                             ");
            }
            parameter.AppendSql("	AND B.ISPRECHARGE = 'Y'          ");
            parameter.AppendSql("	AND B.ISDELETED = 'N'          ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE5   ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE6   ");
            parameter.AppendSql("   AND C.SWLICENSE = :SWLICENSE7   ");

            parameter.AppendSql("ORDER BY VISITRESERVEDATE, DEPARTUREDATETIME                                                                  ");

            parameter.Add("visitStartDate", visitStartDate );
            parameter.Add("visitEndDate", visitEndDate);
            if (visitUserId.NotEmpty())
            {
                parameter.Add("visitUserId", visitUserId);
            }
            if (siteIdOrname.NotEmpty())
            {
                 parameter.AddLikeStatement ("siteIdOrname", siteIdOrname); 
            }
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE3", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE4", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE5", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE6", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE7", clsType.HosInfo.SwLicense);

            return ExecuteReader<UnvisitSiteModel>(parameter);
        }

        /// <summary>
        /// 방문 사업장 목록
        /// </summary>
        /// <param name="visitUserId"></param>
        /// <param name="visitStartDate"></param>
        /// <param name="visitEndDate"></param>
        /// <returns></returns>
        public List<VisitSiteModel> FindVisitSiteList(string visitUserId, string visitStartDate, string visitEndDate, string siteIdOrname)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.ID AS SCHEDULE_ID, B.ID AS VISIT_ID, C.NAME, A.SITE_ID, B.VISITDATETIME, B.VISITUSERNAME  , B.ISFEE                       ");
            parameter.AppendSql("	FROM HIC_OSHA_SCHEDULE A                          ");
            parameter.AppendSql("	INNER JOIN HIC_OSHA_VISIT B                       ");
            parameter.AppendSql("	ON A.ID = B.SCHEDULE_ID	AND A.ISDELETED ='N'      ");
            parameter.AppendSql("	INNER JOIN HC_SITE_VIEW C                         ");
            parameter.AppendSql("	ON A.SITE_ID = C.ID	                              ");
            parameter.AppendSql("WHERE                                                                                                                    ");
            parameter.AppendSql("	B.VISITDATETIME BETWEEN TO_DATE(:visitStartDate, 'YYYY-MM-DD') AND TO_DATE(:visitEndDate, 'YYYY-MM-DD')               ");
            parameter.AppendSql("	AND B.ISPRECHARGE ='N'       ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE1 ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE2 ");
            parameter.AppendSql("   AND C.SWLICENSE = :SWLICENSE3 ");
            if (siteIdOrname.NotEmpty())
            {
                if (siteIdOrname.IsNumeric())
                {
                    parameter.AppendSql("        AND A.SITE_ID  LIKE :siteIdOrname    ");
                }
                else
                {
                    parameter.AppendSql("        AND C.NAME  LIKE :siteIdOrname    ");
                }
            }
            if (visitUserId.NotEmpty())
            {
                parameter.AppendSql("	AND	B.VISITUSER= :visitUserId                                                             ");
            }
            parameter.AppendSql("	AND B.ISDELETED = 'N'                                                                          "); 
            parameter.AppendSql("ORDER BY B.VISITDATETIME DESC, C.NAME                                                                 ");

            parameter.Add("visitStartDate", visitStartDate);
            parameter.Add("visitEndDate", visitEndDate);
            if (visitUserId.NotEmpty())
            {
                parameter.Add("visitUserId", visitUserId);
            }
            if (siteIdOrname.NotEmpty())
            {
                parameter.AddLikeStatement("siteIdOrname", siteIdOrname);
            }
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE3", clsType.HosInfo.SwLicense);

            return ExecuteReader<VisitSiteModel>(parameter);
        }
        /// <summary>
        /// 선청구 방문 사업장
        /// </summary>
        /// <param name="visitUserId"></param>
        /// <param name="visitStartDate"></param>
        /// <param name="visitEndDate"></param>
        /// <returns></returns>
        public List<VisitSiteModel> FindPrehargeVisitSiteList(string visitUserId, string visitStartDate, string visitEndDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.ID AS SCHEDULE_ID, B.ID AS VISIT_ID, C.NAME, A.SITE_ID, B.VISITDATETIME, B.VISITUSERNAME                        ");
            parameter.AppendSql("	FROM HIC_OSHA_SCHEDULE A                                                                                              ");
            parameter.AppendSql("	INNER JOIN HIC_OSHA_VISIT B                                                                                           ");
            parameter.AppendSql("	ON A.ID = B.SCHEDULE_ID	AND A.ISDELETED ='N'                                                                          ");
            parameter.AppendSql("	INNER JOIN HC_SITE_VIEW C                                                                                            ");
            parameter.AppendSql("	ON A.SITE_ID = C.ID	                                                                                                 ");
            parameter.AppendSql("WHERE                                                                                                                    ");
            parameter.AppendSql("	B.VISITDATETIME BETWEEN TO_DATE(:visitStartDate, 'YYYY-MM-DD') AND TO_DATE(:visitEndDate, 'YYYY-MM-DD')               ");
            parameter.AppendSql("	AND B.ISPRECHARGE ='Y'       ");
            parameter.AppendSql("	AND B.ISDELETED ='N'       ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE1 ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE2 ");
            parameter.AppendSql("   AND C.SWLICENSE = :SWLICENSE3 ");
            if (visitUserId.NotEmpty())
            {
                parameter.AppendSql("	AND	B.VISITUSER= :visitUserId ");
            }
            parameter.AppendSql("ORDER BY B.VISITDATETIME, C.NAME     ");

            parameter.Add("visitStartDate", visitStartDate);
            parameter.Add("visitEndDate", visitEndDate);
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE3", clsType.HosInfo.SwLicense);
            if (visitUserId.NotEmpty())
            {
                parameter.Add("visitUserId", visitUserId);
            }

            return ExecuteReader<VisitSiteModel>(parameter);
        }
        /// <summary>
        /// 달력 검색
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<CalendarEventModel> FindCalendarList(CalendarSearchModel model)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID AS EVENT_ID, B.ID AS VISIT_ID, C.NAME, A.EVENTSTARTDATETIME, A.VISITUSERNAME, A.VISITMANAGERNAME, A.VISITSTARTTIME, B.ISFEE ");
            parameter.AppendSql("	FROM HIC_OSHA_SCHEDULE A                 ");
            parameter.AppendSql("	LEFT OUTER JOIN HIC_OSHA_VISIT B         ");
            parameter.AppendSql("	ON A.ID = B.SCHEDULE_ID                  ");
            parameter.AppendSql("	AND B.ISPRECHARGE ='N'                   ");
            parameter.AppendSql("	INNER JOIN HC_SITE_VIEW C                ");
            parameter.AppendSql("	ON A.SITE_ID = C.ID                      ");
            parameter.AppendSql("WHERE                                       ");
            parameter.AppendSql("   A.EVENTSTARTDATETIME BETWEEN TO_DATE(:startDate, 'YYYY-MM-DD') AND TO_DATE(:endDate, 'YYYY-MM-DD')  ");

            if (model.CalendarSearchType == CalendarSearchType.UNVISIT)
            {
                parameter.AppendSql("AND B.ID IS NULL                        ");
                parameter.AppendSql("	AND A.ISDELETED ='N'                 ");

            }
            else if (model.CalendarSearchType == CalendarSearchType.VISIT)
            {
                parameter.AppendSql("AND B.ID IS NOT NULL      ");
                parameter.AppendSql("AND A.ISDELETED ='N'      ");
                parameter.AppendSql("AND B.ISDELETED='N'       ");
            }

            if (model.VisitUser.NotEmpty()) { parameter.AppendSql("	AND	A.VISITUSERID = :visitUserId "); }
            parameter.AppendSql("  AND A.SWLICENSE = :SWLICENSE1 ");
            parameter.AppendSql("  AND B.SWLICENSE = :SWLICENSE2 ");
            parameter.AppendSql("  AND C.SWLICENSE = :SWLICENSE3 ");
            parameter.AppendSql("ORDER BY A.EVENTSTARTDATETIME ");

            parameter.Add("startDate", model.StartDate);
            parameter.Add("endDate", model.EndDate);
            if (model.VisitUser.NotEmpty()) { parameter.Add("visitUserId", model.VisitUser); }
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE3", clsType.HosInfo.SwLicense);

            return ExecuteReader<CalendarEventModel>(parameter);
        }
    }
}
