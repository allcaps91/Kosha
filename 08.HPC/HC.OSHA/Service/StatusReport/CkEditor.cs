using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;
using ComBase.Mvc.Utils;
using ComHpcLibB;
using HC.Core.Dto;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Repository;
using HC.OSHA.Repository.StatusReport;
using HC.OSHA.Service.StatusReport;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HC.OSHA.Service
{
  
    /// <summary>
    /// CEF가 느려지는 현상 Cef Browser 프로세스 비정상적으로 많아짐을 확인해야함.
    /// </summary>
    public class CkEditor
    {
        private ChromiumWebBrowser browser = null;
        private readonly string JavascriptBoundName = "cefsharpBoundAsync";
        private CkEditorBoundCefSharp bound = null;
        private StatusReportEngineerDto statusReportEngineerDto;
        private StatusReportNurseDto statusReportNurseDto;
        
        public CkEditor(string fileName)
        {
            //    string page = string.Format(@"{0}\Resources\html\index.html", Application.StartupPath);
            //String page = @"C:\Users\SDkCarlos\Desktop\artyom-HOMEPAGE\index.html";
            try
            {
                if (!CefSharp.Cef.IsInitialized)
                {
                    //localstorage 재사용가능해짐..
                    CefSettings settings = new CefSettings()
                    {
                       // CachePath = "cache",
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
                            defaultPage: "ckeditor.html"
                        )
                    });

                    Cef.Initialize(settings);
                    CefSharpSettings.LegacyJavascriptBindingEnabled = true;
                    CefSharpSettings.SubprocessExitIfParentProcessClosed = true;
                }
                bound = new CkEditorBoundCefSharp();
                browser = new ChromiumWebBrowser("localfolder://cefsharp/" + fileName);

                bound.CkEditor = this;
                //browser.RegisterAsyncJsObject(JavascriptBoundName, bound);
                browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
                browser.JavascriptObjectRepository.Register(JavascriptBoundName, bound, isAsync: true, options: BindingOptions.DefaultBinder);

                browser.MenuHandler = new CefSharpNoContextMenu();
                browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            bool init = browser.IsBrowserInitialized;
            if (statusReportEngineerDto != null)
            {
                if (browser.CanExecuteJavascriptInMainFrame)
                {
                    browser.ExecuteScriptAsync("load('" + this.statusReportEngineerDto.OPINION + "')");
                }
            }
        }

       
        public void Clear()
        {
            if (browser.IsBrowserInitialized)
            {
                if (browser.CanExecuteJavascriptInMainFrame)
                {
                    browser.ExecuteScriptAsync("clear()");
                }
                
            }
            
            this.statusReportEngineerDto = null;
        }
        public ChromiumWebBrowser GetBrowser()
        {
            return this.browser;
        }

        //현실태 및 개선의견
        public void SetStatusReportEngineerDto(StatusReportEngineerDto dto)
        {
            this.statusReportEngineerDto = dto;
            LoadOpinionByEngineer();
        }
        public void SetStatusReportNurseDto(StatusReportNurseDto dto)
        {
            this.statusReportNurseDto = dto;
            LoadOpinionByNurse();
        }

        
        public void LoadOpinionByNurse()
        {
            if (this.browser.IsBrowserInitialized)
            {
                if (statusReportNurseDto != null)
                {
                    if (this.browser.CanExecuteJavascriptInMainFrame && statusReportNurseDto.OPINION != null)
                    {
                        browser.ExecuteScriptAsync("load('" + statusReportNurseDto.OPINION + "')");
                    }

                }
            }
        }
        public void LoadOpinionByEngineer()
        {
            if (this.browser.IsBrowserInitialized)
            {
                if (statusReportEngineerDto != null)
                {
                    if (this.browser.CanExecuteJavascriptInMainFrame && statusReportEngineerDto.OPINION != null)
                    {
                        browser.ExecuteScriptAsync("load('" + statusReportEngineerDto.OPINION + "')");
                    }

                }
            }
        }
        /// <summary>
        /// 자바스크립트에서 관리카드 업무수행일지 호출(상용구 제목만)
        /// </summary>
        /// <param name="html"></param>
        public void SaveCard19(string html)
        {
            if (statusReportEngineerDto == null)
            {
                MessageUtil.Info("상태보고서를 먼저 저장 하여야 합니다.");
            }
            else
            {
                if (statusReportEngineerDto.ID > 0)
                {
                    //  var json = JObject.Parse(bound.Opinion);
                    //  string html = json.GetValue("html");
                    //  object JsonConvert.DeserializeObject(bound.Opinion);
                    //  StatusReportEngineerDto dto = PanStatausReport.GetData<StatusReportEngineerDto>();
                    HcOshaCard19Service hcOshaCard19Service = new HcOshaCard19Service();
                    
                    string[] array = html.Split('$');
                    foreach(string title in array)
                    {
                        if (title != "")
                        {
                            HC_OSHA_CARD19 dto = new HC_OSHA_CARD19();
                            dto.REGDATE = DateUtil.stringToDateTime(statusReportEngineerDto.VISITDATE, ComBase.Controls.DateTimeType.YYYYMMDD);
                            dto.CONTENT = title;
                            dto.SITE_ID = statusReportEngineerDto.SITE_ID;
                            dto.ESTIMATE_ID = statusReportEngineerDto.ESTIMATE_ID;
                            hcOshaCard19Service.Save(dto);
                        }
                        
                    }

                    
                }
            }
        }
     
        /// <summary>
        /// 자바스크립트에서 저장 호출
        /// </summary>
        /// <param name="html"></param>
        public void SaveEngineerOpinion(string html)
        {
            if(statusReportEngineerDto == null)
            {
                MessageUtil.Info("상태보고서를 먼저 저장 하여야 합니다.");
            }
            else{
                if (statusReportEngineerDto.ID > 0)
                {
                    //  var json = JObject.Parse(bound.Opinion);
                    //  string html = json.GetValue("html");
                    //  object JsonConvert.DeserializeObject(bound.Opinion);
                    //  StatusReportEngineerDto dto = PanStatausReport.GetData<StatusReportEngineerDto>();
                    StatusReportEngineerRepository repo = new StatusReportEngineerRepository();
                    repo.UpdateOptioin(statusReportEngineerDto.ID, html);

                }
            }
        }
        public void SaveNurseOpinion(String html)
        {
            if (statusReportNurseDto == null)
            {
                MessageUtil.Info("상태보고서를 먼저 저장 하여야 합니다.");
            }
            else
            {
                if (statusReportNurseDto.ID > 0)
                {
                    StatusReportNurseRepository repo = new StatusReportNurseRepository();
                    repo.UpdateOptioin(statusReportNurseDto.ID, html);
                    
                    //MessageUtil.Info("상태보고서 종합의견을 저장하였습니다 "); 

                }
            }
        }
    }
}
