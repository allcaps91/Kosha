using ComBase.Mvc.Utils;
using ComBase.Controls;
using HC.Core.Common.Browser;
using HC.OSHA.Visit.Schedule.Dto;
using HC.OSHA.Visit.Schedule.Model;
using HC.OSHA.Visit.Schedule.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Visit.Schedule.Service
{
    public class SchduleModelService
    {
        public ScheduleModelRepository scheduleModelRepository { get; }
        public SchduleModelService()
        {
            scheduleModelRepository = new ScheduleModelRepository();
        }

      
        public List<CalendarEvent> SearchSchedule(CalendarSearchModel searchModel)
        {
            List<CalendarEvent> list =  new List<CalendarEvent>();
            foreach (CalendarEventModel model in scheduleModelRepository.FindCalendarList(searchModel))
            {
                string title = model.NAME;
                if (model.VISITUSERNAME.NotEmpty())
                {
                    title = model.NAME + "[" + model.VISITUSERNAME + "]";
                }
                string color = "blue";
                if(model.VISIT_ID == 0)
                {
                    color = "gray";
                }
                else if(model.ISFEE == "Y")
                {
                    color = "green";
                }

                CalendarEvent e = new CalendarEvent()
                {
                    id = model.EVENT_ID.ToString(),
                    title = title,
                    start = DateUtil.DateTimeToStrig(model.EVENTSTARTDATETIME, ComBase.Controls.DateTimeType.YYYY_MM_DD_HH_MM),
                    end = DateUtil.DateTimeToStrig(model.EVENTENDDATETIME, ComBase.Controls.DateTimeType.YYYY_MM_DD_HH_MM),
                    color = color
                };
                list.Add(e);
            }
            return list;
        }
    }
}
