using CefSharp;
using ComBase;
using HC.OSHA.Model;
using HC_OSHA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HC.OSHA.Service
{
    /// <summary>
    /// debug\bin\resources\index.html을 브라우저상에서 테스트
    /// </summary>
    public class JavascriptBoundCefSharp
    {
        CalendarForm calendarForm;
        private SchduleModelService schduleModelService;
        private HcOshaScheduleService hcOshaScheduleService;
        public string CurrentCalendarDate { get; set; }
        public JavascriptBoundCefSharp(CalendarForm form)
        {
            schduleModelService = new SchduleModelService();
            hcOshaScheduleService = new HcOshaScheduleService();

            this.calendarForm = form;
        }

        /// <summary>
        /// 새로운 이벤트
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="type"></param>
        public void NewEvent(string startDate, string viewType)
        {
            Thread thread = new Thread(new ThreadStart(delegate ()
            {
                calendarForm.Invoke(new Action(delegate ()
                {
                    ScheduleRegisterForm form = new ScheduleRegisterForm();
                    form.StartDate = startDate;
                    form.ViewType = viewType;
                    form.ShowDialog();
                    ReadyCalendar(startDate);//또는 저장시마다 이벤트 받아서 한개씩 추가하는방법(addevent)
                    //string events = JsonConvert.SerializeObject(schduleModelService.Today());
                    //calendarForm.GetBrowser().ExecuteScriptAsync("AddEventJson('" + events + "');");

                }));
            }));
            thread.Start();
        }
        /// <summary>
        /// 이벤트 수정
        /// </summary>
        /// <param name="eventId"></param>
        public void UpdateEvent(string eventId, string startDate, string viewType)
        {
            Thread thread = new Thread(new ThreadStart(delegate () // thread 생성
            {
                calendarForm.Invoke(new Action(delegate ()
                {
                    ScheduleRegisterForm form = new ScheduleRegisterForm();
                    form.StartDate = startDate;
                    form.EventId = eventId;
                    form.ViewType = viewType;
                    form.ShowDialog();
                    ReadyCalendar(startDate);
                }));
            }));
            thread.Start();
        }

        //캘린더 로드시 호출
        public void ReadyCalendar(string date)
        {
            CurrentCalendarDate = date;
            Thread thread = new Thread(new ThreadStart(delegate ()
            {
                calendarForm.Invoke(new Action(delegate ()
                {
                       string json = calendarForm.GetJsonCalendarEvents(date);
                       Log.Debug("4");

                    //CalendarSearchModel model = new CalendarSearchModel();
                    //model.StartDate = "2019-09-01";
                    //model.CalendarSearchType = CalendarSearchType.ALL;
                    //model.EndDate = "2019-09-30";
                    //string json = JsonConvert.SerializeObject(schduleModelService.SearchSchedule(model));
                    calendarForm.GetBrowser().ExecuteScriptAsync("AddEventJson('" + json + "');");

                }));
            }));
            thread.Start();

        }

        /// <summary>
        /// viewType 타입에 따라 (날짜정보가 다름)
        /// </summary>
        /// <param name="nextDate"></param>
        public void Next(string nextDate, string viewType)
        {
            CurrentCalendarDate = nextDate;
            ReadyCalendar(nextDate);

        }
        /// <summary>
        /// 드래그앤드랍 날짜만 이동.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="startDate"></param>
        /// <param name="viewType"></param>
        public void DragDropEvent(string eventId, string startDate, string viewType)
        {
            //날짜변경후 
            hcOshaScheduleService.UpdateStartDate(long.Parse(eventId), startDate);
        }
    }
}
