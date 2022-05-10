using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Utils;
using ComHpcLibB;
using HC.Core.Dto;
using HC.Core.Service;
using System;
using System.Collections.Generic;

namespace HC_Core.Macroword
{
    public partial class MacrowordCkeditorForm : CommonForm
    {
        private MacrowordService macrowordService;
        public string FormName { get; set; }
        public string ControlId { get; set; }

        private ChromiumWebBrowser browser = null;
        private readonly string JavascriptBoundName = "cefsharpBoundAsync";
        private MacrowordCkEditorBound bound = null;
        private ChromiumWebBrowser targetBrowser;

        public delegate void MacroHandler(MacrowordDto dto);
        public event MacroHandler macroHandler;
        private List<MacrowordDto> card19List;
        public MacrowordCkeditorForm(string formName, string controlId, ChromiumWebBrowser targetBrowser)
        {
            InitializeComponent();
            macrowordService = new MacrowordService();

            this.FormName = formName;
            this.ControlId = controlId;
            this.targetBrowser = targetBrowser;

            this.targetBrowser = targetBrowser;
            this.card19List = new List<MacrowordDto>(); ;
            if (!CefSharp.Cef.IsInitialized)
            {
                //localstorage 재사용가능해짐..
                CefSettings settings = new CefSettings()
                {
                    //CachePath = "cache",
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
            bound = new MacrowordCkEditorBound();
            browser = new ChromiumWebBrowser("localfolder://cefsharp/macroCkeditor.html");

            bound.MacrowordCkeditorForm = this;
            //browser.RegisterAsyncJsObject(JavascriptBoundName, bound);
            browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            browser.JavascriptObjectRepository.Register(JavascriptBoundName, bound, isAsync: true, options: BindingOptions.DefaultBinder);

            browser.MenuHandler = new CefSharpNoContextMenu();

            panWeb.Controls.Add(browser);
            browser.BringToFront();
            browser.Show();
        }
        private void MacrowordCkeditorForm_Load(object sender, EventArgs e)
        {
            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSList.AddColumnText("상용구", nameof(MacrowordDto.TITLE), 227, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("표시순서", nameof(MacrowordDto.DISPSEQ), 37, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //   SSList.AddColumnButton("", 50, new SpreadCellTypeOption { IsSort = false, ButtonText = "수정" }).ButtonClick += MacrowordForm_ButtonClick;
            SearchMacroword();
        }
        public void SearchMacroword()
        {
            List<MacrowordDto> list = macrowordService.FindAll(this.FormName, this.ControlId, txtView.Text.Trim());
            SSList.SetDataSource(list);
        }
     
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (TxtTitle.Text.IsNullOrEmpty())
            {
                MessageUtil.Info("상용구 제목이 없습니다");
            }
            else
            {
                browser.ExecuteScriptAsync("saveMacro()");

            }
            
        }

        public void SaveMacro(string html)
        {
            MacrowordDto dto = new MacrowordDto();
            if (txtMacroId.Text.IsNullOrEmpty())
            {

                dto.ID = 0;
            }
            else
            {
                dto.ID = long.Parse(txtMacroId.Text);
            }

            dto.FORMNAME = this.FormName;
            dto.CONTROL = this.ControlId;
            dto.TITLE = TxtTitle.Text;
            dto.SUBTITLE = TxtSUBTITLE.Text;
            dto.CONTENT = html;
            dto.DISPSEQ = Double.Parse(numDispSeq.GetValue().ToString());
            MacrowordDto saved = macrowordService.Save(dto);

          

            SearchMacroword();
            
            txtMacroId.BeginInvoke(new MacroHandler(UpdateMacroId), saved);
        }
        public void UpdateMacroId(MacrowordDto saved)
        {
            txtMacroId.Text = saved.ID.ToString();

        }
        public void Clear()
        {
            TxtTitle.Text = "";
            txtMacroId.Text = "";
            TxtSUBTITLE.Text = "";
            if (browser.IsBrowserInitialized)
            {
                if (browser.CanExecuteJavascriptInMainFrame)
                {
                    browser.ExecuteScriptAsync("clear()");
                }
            }
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            
            if (txtMacroId.Text.Length <= 0)
            {
                MessageUtil.Alert(" 상용구를 선택하세요 ");
            }
            else
            {
                macrowordService.MacrowordRepository.Delete(long.Parse(txtMacroId.Text));
                Clear();
                SearchMacroword();

            }
        }

        private void SSList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            MacrowordDto dto = SSList.GetRowData(e.Row) as MacrowordDto;
            txtMacroId.Text = dto.ID.ToString();
            TxtTitle.Text = dto.TITLE;
            TxtSUBTITLE.Text = dto.SUBTITLE;
            numDispSeq.SetValue(dto.DISPSEQ);


            if (browser.IsBrowserInitialized)
            {
                if (browser.CanExecuteJavascriptInMainFrame)
                {
                    browser.ExecuteScriptAsync("load('" + dto.CONTENT + "')");
                }
            }
           
                
        }

        private void SSList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            MacrowordDto dto = SSList.GetRowData(e.Row) as MacrowordDto;
            if (targetBrowser != null)
            {
                if (targetBrowser.CanExecuteJavascriptInMainFrame)
                {
                    //종합의견 브라우저 macroLoad 콜
                    targetBrowser.ExecuteScriptAsync("macroLoad('" + dto.TITLE + "','" + dto.SUBTITLE + "','" + dto.CONTENT +"')");
                    //this.Close();
                    if (card19List != null)
                    {
                        card19List.Add(dto);
                    }
              
                }
                
            }
            
        }

        public List<MacrowordDto> GetCard19List()
        {
            return this.card19List;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            SearchMacroword();
        }
    }
}