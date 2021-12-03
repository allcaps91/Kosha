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
    public class HaResvJavascriptBoundCefSharp
    {
        frmHaResvCalendar form;

        public HaResvJavascriptBoundCefSharp(frmHaResvCalendar form)
        {
         
            this.form = form;
        }


        public void Ready(string date)
        {
            Thread thread = new Thread(new ThreadStart(delegate ()
            {
                form.Invoke(new Action(delegate ()
                {
                    //string json = form.GetJsonCalendarEvents(date);
                    //json = json.Replace("\\r\\n", "");
                    //Log.Debug("4");

                    //form.GetBrowser().ExecuteScriptAsync("AddEventJson('" + json + "');");

                }));
            }));
            thread.Start();
        }

        public void NewEvent(string startDate, string viewType)
        {
            
        }

        public void UpdateEvent(string eventId, string startDate, string viewType)
        {
          
        }
    }
}
