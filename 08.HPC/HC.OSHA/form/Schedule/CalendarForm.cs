using CefSharp.WinForms;
using CefSharp;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Exceptions;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.SchemeHandler;
using System.IO;
using Newtonsoft.Json;
using HC_OSHA;
using HC_Core;
using HC.OSHA.Service;
using System;
using HC.OSHA.Model;
using HC.Core.Service;
using ComBase;
using ComHpcLibB;

namespace HC_OSHA
{
    /// <summary>
    /// 앱종료시 cef shutdown 해야함
    /// </summary>
    public partial class CalendarForm : CommonForm
    {
        private ChromiumWebBrowser browser = null;
        private readonly string JavascriptBoundName = "cefsharpBoundAsync";
        private JavascriptBoundCefSharp bound = null;

        private HcOshaScheduleService hcOshaScheduleService;
        private SchduleModelService schduleModelService;
        public CalendarForm()
        {
            InitializeComponent();
            hcOshaScheduleService = new HcOshaScheduleService();
            schduleModelService = new SchduleModelService();

           // string page = string.Format(@"{0}\Resources\html\index.html", Application.StartupPath);
            //String page = @"C:\Users\SDkCarlos\Desktop\artyom-HOMEPAGE\index.html";
            if (!CefSharp.Cef.IsInitialized)
            {
                //localstorage 재사용가능해짐..
                CefSettings settings = new CefSettings()
                {
                    ///CachePath = "cache",
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
            else
            {
                
            }
            
            //string html = File.ReadAllText(@"index.html");
            bound = new JavascriptBoundCefSharp(this);
            browser = new ChromiumWebBrowser("localfolder://cefsharp/index.html");

            //browser.RegisterAsyncJsObject(JavascriptBoundName, bound);
            browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            browser.JavascriptObjectRepository.Register(JavascriptBoundName, bound, isAsync: true, options: BindingOptions.DefaultBinder);
            browser.MenuHandler = new CefSharpNoContextMenu();

   

            panCalendar.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;

            //browser.IsBrowserInitializedChanged += Brower_IsBrowserInitializedChanged;
            //browser.LoadingStateChanged += ChromiumWebBrowser_LoadingStateChanged;

        }

        public ChromiumWebBrowser GetBrowser()
        {
            return this.browser;
        }

        private void CalendarForm_Load(object sender, EventArgs e)
        {
            RdoCalendarSearchType_All.SetOptions(new RadioButtonOption { DataField = nameof(HC.OSHA.Model.CalendarSearchModel.CalendarSearchType), CheckValue = "ALL" });
            RdoCalendarSearchType_Visit.SetOptions(new RadioButtonOption { DataField = nameof(HC.OSHA.Model.CalendarSearchModel.CalendarSearchType), CheckValue = "VISIT" });
            RdoCalendarSearchType_Unvisit.SetOptions(new RadioButtonOption { DataField = nameof(HC.OSHA.Model.CalendarSearchModel.CalendarSearchType), CheckValue = "UNVISIT" });
            CboManager.SetOptions(new ComboBoxOption { DataField = nameof(HC.OSHA.Model.CalendarSearchModel.VisitUser) });
            CboManager.SetItems(CommonService.Instance.UserService.GetOsha(), "Name", "UserId", "전체", "", AddComboBoxPosition.Top);

            panSearch.SetData(new HC.OSHA.Model.CalendarSearchModel());
            CboManager.SetValue(CommonService.Instance.Session.UserId);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {

            bound.ReadyCalendar(bound.CurrentCalendarDate);
        }

        public String GetJsonCalendarEvents(string date)
        {
            HC.OSHA.Model.CalendarSearchModel model = panSearch.GetData<HC.OSHA.Model.CalendarSearchModel>();
            model.StartDate = DateUtil.stringToDateTime(date, DateTimeType.YYYY_MM_DD).AddMonths(-2).ToString("yyyy-MM-dd");
            model.EndDate = DateUtil.stringToDateTime(date, DateTimeType.YYYY_MM_DD).AddMonths(2).ToString("yyyy-MM-dd");
            List<CalendarEvent>  list = schduleModelService.SearchSchedule(model);
            Log.Debug("3" + list.Count);
            return JsonConvert.SerializeObject(list);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }

    
    

        private void RdoCalendarSearchType_All_Click(object sender, EventArgs e)
        {
            bound.ReadyCalendar(bound.CurrentCalendarDate);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            browser.Print();

        }
    }
}
