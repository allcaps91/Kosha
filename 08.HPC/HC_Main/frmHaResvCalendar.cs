using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;
using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using ComEmrBase;
using HC.Core.Common.Browser;
using HC_Main.Model;
using HC_Main.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_Main
{
    public partial class frmHaResvCalendar : Form
    {
        private ChromiumWebBrowser browser = null;
        private readonly string JavascriptBoundName = "cefsharpBoundAsync";
        private JavascriptBoundCefSharp bound = null;

        private SchduleModelService schduleModelService;

        public frmHaResvCalendar()
        {
            InitializeComponent();
            SetControl();
        }

        private void SetControl()
        {
            if (!CefSharp.Cef.IsInitialized)
            {
                //localstorage 재사용가능해짐..
                CefSettings settings = new CefSettings()
                {
                    CachePath = "cache",


                };
                settings.CefCommandLineArgs.Add("disable-web-security", "true"); //google 달력 cors 문제해결

                settings.RegisterScheme(new CefCustomScheme
                {
                    IsCorsEnabled = false,
                    IsSecure = false,
                    SchemeName = "localfolder",
                    DomainName = "cefsharp",
                    SchemeHandlerFactory = new FolderSchemeHandlerFactory(
                    rootFolder: @"./Resources/html",
                    hostName: "cefsharp",
                    defaultPage: "index.html"

                )
                });

                Cef.Initialize(settings);
                CefSharpSettings.LegacyJavascriptBindingEnabled = true;

            }

            //string html = File.ReadAllText(@"index.html");
            bound = new JavascriptBoundCefSharp(this);
            browser = new ChromiumWebBrowser("localfolder://cefsharp");

            browser.RegisterAsyncJsObject(JavascriptBoundName, bound);
            browser.MenuHandler = new CefSharpNoContextMenu();

            panCal.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
        }

        public String GetJsonCalendarEvents(string date)
        {
            CalendarSearchModel model = panSearch.GetData<CalendarSearchModel>();
            model.StartDate = DateUtil.stringToDateTime(date, DateTimeType.YYYY_MM_DD).AddMonths(-2).ToString("yyyy-MM-dd");
            model.EndDate = DateUtil.stringToDateTime(date, DateTimeType.YYYY_MM_DD).AddMonths(2).ToString("yyyy-MM-dd");
            List<CalendarEvent> list = schduleModelService.SearchSchedule(model);
            Log.Debug("3" + list.Count);
            return JsonConvert.SerializeObject(list);
        }


        public ChromiumWebBrowser GetBrowser()
        {
            return this.browser;
        }
    }
}
