using CefSharp;
using ComBase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComHpcLibB.Service
{
    public class HcResvJavascriptBoundCefSharp
    {
        frmHcCalendar form;

        public HcResvJavascriptBoundCefSharp(frmHcCalendar form)
        {
            this.form = form;
        }


        public void Ready(string date)
        {
            //List<CalendarEvent> list = new List<CalendarEvent>();

            //CalendarEvent e = new CalendarEvent
            //{
            //    title = "aaa",
            //    start = "2020-05-22",
            //    end = "2020-05-22"
            //};

            //list.Add(e);

            //CalendarEvent e2 = new CalendarEvent
            //{
            //    title = "aaa2",
            //    start = "2020-05-23",
            //    end = "2020-05-24"
            //};

            //list.Add(e2);

            Thread thread = new Thread(new ThreadStart(delegate ()
            {
                form.Invoke(new Action(delegate ()
                {
                    string json = form.GetJsonCalendarEvents(date);
                    json = json.Replace("\\r\\n", "");
                    Log.Debug("4");

                    //CalendarSearchModel model = new CalendarSearchModel();
                    //model.StartDate = "2019-09-01";
                    //model.CalendarSearchType = CalendarSearchType.ALL;
                    //model.EndDate = "2019-09-30";
                    //string json = JsonConvert.SerializeObject(schduleModelService.SearchSchedule(model));
                    form.GetBrowser().ExecuteScriptAsync("AddEventJson('" + json + "');");

                }));
            }));
            thread.Start();


        }

        public void NewEvent(string startDate, string viewType)
        {
            
        }


        public void UpdateEvent(string eventId, string startDate, string viewType)
        {
            Thread thread = new Thread(new ThreadStart(delegate () // thread 생성
            {
                form.Invoke(new Action(delegate ()
                {
                    frmHaGamCode form = new frmHaGamCode();
                    form.ShowDialog();
                    //Ready(startDate);
                }));
            }));
            thread.Start();
        }
    }
}
