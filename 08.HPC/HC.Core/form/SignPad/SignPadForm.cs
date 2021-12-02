using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;
using ComHpcLibB;
using System;
using System.Windows.Forms;
using HC.Core.Dto;
using HC.Core.Service;


namespace HC_Core
{
    public partial class SignPadForm : Form
    {
        public String Base64Image { get; set; }
        private ChromiumWebBrowser browser = null;
        private readonly string JavascriptBoundName = "cefsharpBoundAsync";
        private SignPadBound bound = null;
        public SignPadForm(bool isUserSign)
        {
            InitializeComponent();

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
                        defaultPage: "signpad.html"
                    )
                });

                Cef.Initialize(settings);
                CefSharpSettings.LegacyJavascriptBindingEnabled = true;
                CefSharpSettings.SubprocessExitIfParentProcessClosed = true;
            }
            bound = new SignPadBound(this, isUserSign);
            bound.IsUserSign = isUserSign;
            browser = new ChromiumWebBrowser("localfolder://cefsharp/signpad.html");

            // browser.RegisterAsyncJsObject(JavascriptBoundName, bound);
            browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            browser.JavascriptObjectRepository.Register(JavascriptBoundName, bound, isAsync: true, options: BindingOptions.DefaultBinder);
            browser.MenuHandler = new CefSharpNoContextMenu();

            PanWeb.Controls.Add(browser);
            browser.BringToFront();
            browser.Show();
        }
        public void Save(string base64Image)
        {
            this.Base64Image = base64Image;
            this.DialogResult = DialogResult.OK;
           
        }
        public void ShowSaveButton()
        {
            if (browser.IsBrowserInitialized)
            {
                if (browser.CanExecuteJavascriptInMainFrame)
                {
                    browser.ExecuteScriptAsync("showSaveButton()");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }
    }
}
