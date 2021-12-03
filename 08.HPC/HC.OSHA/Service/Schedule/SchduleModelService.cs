using ComBase.Mvc.Utils;
using ComBase.Controls;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC.OSHA.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComBase;
using ComHpcLibB;

namespace HC.OSHA.Service
{
    public class SchduleModelService
    {
        public ScheduleModelRepository scheduleModelRepository { get; }
        public SchduleModelService()
        {
            scheduleModelRepository = new ScheduleModelRepository();
        }

      
        public List<CalendarEvent> SearchSchedule(HC.OSHA.Model.CalendarSearchModel searchModel)
        {
            List<CalendarEvent> list =  new List<CalendarEvent>();
            try
            {
                foreach (HC.OSHA.Model.CalendarEventModel model in scheduleModelRepository.FindCalendarList(searchModel))
                {
                    Log.Debug("1");
                    string title = model.NAME;
                    if (model.VISITUSERNAME.NotEmpty())
                    {
                        title =   model.VISITSTARTTIME +" "+ model.NAME + "[" + model.VISITUSERNAME + "]" + "[" + model.VISITMANAGERNAME + "]";
                    }
                    string color = "#759FD2";
                    if (model.VISIT_ID == 0)
                    {
                        color = "#D9D9D9";
                    }
                    else if (model.ISFEE == "Y")
                    {
                        color = "#98E0AD";
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
                Log.Debug("2");
            }catch(Exception e)
            {
                Log.Debug(e.Message);
            }
         
            return list;
        }
    }
}
