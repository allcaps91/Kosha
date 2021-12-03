using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using ComHpcLibB.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComHpcLibB.Service
{
    public class HeaResvCalService
    {
        private HeaResvCalRepository heaResvCalRepository;

        /// <summary>
        /// 
        /// </summary>
        public HeaResvCalService()
        {
            this.heaResvCalRepository = new HeaResvCalRepository();
        }

        public List<CalendarEvent> ScheduleListUp(CalendarSearchModel searchModel)
        {
            List<CalendarEvent> list =  new List<CalendarEvent>();
            try
            {
                foreach (CalendarEventModel model in heaResvCalRepository.SearchSchedule(searchModel))
                {
                    Log.Debug("1");

                    string title = string.Empty;

                    if (model.LTDCODE > 0)
                    {
                        title = "회사:" + model.LTDNAME + ComNum.VBLF;
                        title += "▶출발시간:" + model.STARTTIME.ToString() + ComNum.VBLF;
                        title += "검진시간:" + model.RTIME + ComNum.VBLF;
                        title += "인원:" + model.INWON.To<string>() + ComNum.VBLF;
                        title += "검진종류:" + model.SPECIAL + ComNum.VBLF;
                        title += "참고사항:" + model.REMARK;
                    }
                    else
                    {
                        title += ComNum.VBLF + "참고사항:" + model.REMARK;
                    }

                    string color = "#81BEF7";

                    //string color = "#759FD2";
                    //if (model.VISIT_ID == 0)
                    //{
                    //    color = "#D9D9D9";
                    //}
                    //else if (model.ISFEE == "Y")
                    //{
                    //    color = "#98E0AD";
                    //}

                    CalendarEvent e = new CalendarEvent()
                    {
                        //id = model.EVENT_ID.ToString(),
                        title = title,
                        start = DateUtil.DateTimeToStrig(model.RDATE, ComBase.Controls.DateTimeType.YYYY_MM_DD_HH_MM),
                        end = DateUtil.DateTimeToStrig(model.RDATE, ComBase.Controls.DateTimeType.YYYY_MM_DD_HH_MM),
                        color = color
                    };

                    list.Add(e);
                }
                Log.Debug("2");
            }
            catch (Exception e)
            {
                Log.Debug(e.Message);
            }

            return list;
        }

    }
}
