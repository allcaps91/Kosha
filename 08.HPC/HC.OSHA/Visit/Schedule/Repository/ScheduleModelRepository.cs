using ComBase.Controls;
using ComBase.Mvc;
using HC.OSHA.Visit.Schedule.Dto;
using HC.OSHA.Visit.Schedule.Model;
using System.Collections.Generic;
using HC.Core.Common.Service;

namespace HC.OSHA.Visit.Schedule.Repository
{
    public class ScheduleModelRepository : BaseRepository
    {
        public List<HC_OSHA_SCHEDULE> FindByToday()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_OSHA_SCHEDULE                               ");


            return ExecuteReader<HC_OSHA_SCHEDULE>(parameter);
        }
        /// <summary>
        /// 비망문 사업장 목록
        /// </summary>
        /// <param name="visitUserId"></param>
        /// <param name="visitStartDate"></param>
        /// <param name="visitEndDate"></param>
        /// <returns></returns>
        public List<UnvisitSiteModel> FindUnvisitSiteList(string visitUserId, string visitStartDate, string visitEndDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID AS SCHEDULE_ID, C.NAME, A.SITE_ID, A.VISITRESERVEDATE, A.VISITUSERNAME					");
            parameter.AppendSql("	FROM HC_OSHA_SCHEDULE A                                                                         ");
            parameter.AppendSql("	LEFT OUTER JOIN HC_OSHA_VISIT B                                                                 ");
            parameter.AppendSql("	ON A.ID = B.SCHEDULE_ID                                                                         ");
            parameter.AppendSql("	INNER JOIN HC_SITE_VIEW C                                                                       ");
            parameter.AppendSql("	ON A.SITE_ID = C.ID	                                                                            ");
            parameter.AppendSql("WHERE                                                                                               ");
            parameter.AppendSql("	A.VISITRESERVEDATE BETWEEN TO_DATE(:visitStartDate) AND TO_DATE(:visitEndDate)                      ");
            parameter.AppendSql("	AND B.ID IS NULL                         ");
            parameter.AppendSql("	AND A.ISDELETED ='N'                     ");
            if (visitUserId.NotEmpty())
            {
                parameter.AppendSql("	AND	A.VISITUSERID = :visitUserId                                                             ");
            }
            
            parameter.AppendSql("ORDER BY A.VISITRESERVEDATE, C.NAME                                                                 ");

            parameter.Add("visitStartDate", visitStartDate);
            parameter.Add("visitEndDate", visitEndDate);
            if (visitUserId.NotEmpty())
            {
                parameter.Add("visitUserId", visitUserId);
            }
             
            return ExecuteReader<UnvisitSiteModel>(parameter);
        }

        /// <summary>
        /// 방문 사업장 목록
        /// </summary>
        /// <param name="visitUserId"></param>
        /// <param name="visitStartDate"></param>
        /// <param name="visitEndDate"></param>
        /// <returns></returns>
        public List<VisitSiteModel> FindVisitSiteList(string visitUserId, string visitStartDate, string visitEndDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.ID AS SCHEDULE_ID, B.ID AS VISIT_ID, C.NAME, A.SITE_ID, B.VISITDATETIME, B.VISITUSERNAME                        ");
            parameter.AppendSql("	FROM HC_OSHA_SCHEDULE A                                                                                              ");
            parameter.AppendSql("	INNER JOIN HC_OSHA_VISIT B                                                                                           ");
            parameter.AppendSql("	ON A.ID = B.SCHEDULE_ID	AND A.ISDELETED ='N'                                                                                             ");
            parameter.AppendSql("	INNER JOIN HC_SITE_VIEW C                                                                                            ");
            parameter.AppendSql("	ON A.SITE_ID = C.ID	                                                                                                 ");
            parameter.AppendSql("WHERE                                                                                                                    ");
            parameter.AppendSql("	B.VISITDATETIME BETWEEN TO_DATE(:visitStartDate) AND TO_DATE(:visitEndDate)                                              ");
            if (visitUserId.NotEmpty())
            {
                parameter.AppendSql("	AND	B.VISITUSER= :visitUserId                                                             ");
            }

            parameter.AppendSql("ORDER BY B.VISITDATETIME, C.NAME                                                                 ");

            parameter.Add("visitStartDate", visitStartDate);
            parameter.Add("visitEndDate", visitEndDate);
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



            parameter.AppendSql("SELECT A.ID AS EVENT_ID, B.ID AS VISIT_ID, C.NAME, A.EVENTSTARTDATETIME, A.VISITUSERNAME, B.ISFEE        ");
            parameter.AppendSql("	FROM HC_OSHA_SCHEDULE A                                                                                              ");
            parameter.AppendSql("	LEFT OUTER JOIN HC_OSHA_VISIT B                                                                                      ");
            parameter.AppendSql("	ON A.ID = B.SCHEDULE_ID                                                                                              ");
            parameter.AppendSql("	INNER JOIN HC_SITE_VIEW C                                                                                            ");
            parameter.AppendSql("	ON A.SITE_ID = C.ID                                                                                                  ");
            parameter.AppendSql("WHERE                                                                                                  ");
            parameter.AppendSql("   A.EVENTSTARTDATETIME BETWEEN TO_DATE(:startDate) AND TO_DATE(:endDate)                                              ");



            if (model.CalendarSearchType == CalendarSearchType.UNVISIT)
            {
                parameter.AppendSql("AND B.ID IS NULL                                                                              ");
                parameter.AppendSql("	AND A.ISDELETED ='N'                                                                                                 ");

            }
            else if (model.CalendarSearchType == CalendarSearchType.VISIT)
            {
                parameter.AppendSql("AND B.ID IS NOT NULL                                                                              ");
                parameter.AppendSql("AND A.ISDELETED ='N'                                                                                                 ");
                parameter.AppendSql("AND B.ISDELETED='N'                                                                                                  ");
            }

            if (model.VisitUser.NotEmpty())
            {
                parameter.AppendSql("	AND	A.VISITUSERID = :visitUserId                                                             ");
            }


            parameter.AppendSql("ORDER BY A.EVENTSTARTDATETIME                                                                            ");

            parameter.Add("startDate", model.StartDate);
            parameter.Add("endDate", model.EndDate);

            if (model.VisitUser.NotEmpty())
            {
                parameter.Add("visitUserId", model.VisitUser);
            }


            return ExecuteReader<CalendarEventModel>(parameter);
        }
    }
}
