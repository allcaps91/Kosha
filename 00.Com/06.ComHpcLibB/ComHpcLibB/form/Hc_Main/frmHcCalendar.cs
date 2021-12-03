using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;
using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using ComEmrBase;
using ComHpcLibB.Service;
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

namespace ComHpcLibB
{
    public partial class frmHcCalendar :Form
    {
        private ChromiumWebBrowser browser = null;
        private readonly string JavascriptBoundName = "cefsharpBoundAsync";
        private HcResvJavascriptBoundCefSharp bound = null;
        HicChulresvService hicChulresvService = null;

        public frmHcCalendar()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        private void SetEvent()
        {
            this.Load           += new EventHandler(eFormLoad);
            this.btnDev.Click   += new EventHandler(eBtnClick);
            this.btnExit.Click  += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnDev)
            {
                browser.ShowDevTools();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            
        }

        private void SetControl()
        {
            hicChulresvService = new HicChulresvService();

            if (!Cef.IsInitialized)
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
                        defaultPage: "htmHaCal.html"
                    )
                });

                Cef.Initialize(settings);
                CefSharpSettings.LegacyJavascriptBindingEnabled = true;

            }

            //string html = File.ReadAllText(@"index.html");
            bound = new HcResvJavascriptBoundCefSharp(this);
            browser = new ChromiumWebBrowser("localfolder://cefsharp");

            browser.RegisterAsyncJsObject(JavascriptBoundName, bound);
            browser.MenuHandler = new CefSharpNoContextMenu();

            panCal.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
        }

        public ChromiumWebBrowser GetBrowser()
        {
            return this.browser;
        }

        public String GetJsonCalendarEvents(string date)
        {
            CalendarSearchModel CSM = new CalendarSearchModel
            {
                ViewGubun = rdoView1.Checked == true ? "1" : "2",
                StartDate = DateUtil.stringToDateTime(date, DateTimeType.YYYY_MM_DD).AddMonths(-2).ToString("yyyy-MM-dd"),
                EndDate = DateUtil.stringToDateTime(date, DateTimeType.YYYY_MM_DD).AddMonths(2).ToString("yyyy-MM-dd")
            };

            List<CalendarEvent> list = hicChulresvService.ScheduleListUp(CSM);
            Log.Debug("3" + list.Count);
            return JsonConvert.SerializeObject(list);
        }

    }
}
