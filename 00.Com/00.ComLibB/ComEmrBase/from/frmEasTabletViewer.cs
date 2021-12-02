using CefSharp;
using CefSharp.WinForms;
using ComBase;
using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// </summary>
    public partial class frmEasTabletViewer : Form
    {
        public ChromiumWebBrowser Browser { get; set; } 
        private EasManager manager = null;
        public  EasTabaletViewerJavascriptBound easTabletViewerBound  { get; set; }
        public EasViewerJavascriptBound easViewerBound = null;
        public frmEasViewer frmEasViewer { get; set; }

        private string currentUrl = string.Empty;

      
        public frmEasTabletViewer(EasManager manager, EasViewerJavascriptBound easViewerBound)
        {
            InitializeComponent();

            this.manager = manager;
            this.easViewerBound = easViewerBound;
            
            Initialize(manager);
        }
        private void Initialize(EasManager manager)
        {
            if (clsType.User.Sabun == "800594" || clsType.User.Sabun == "36540")
            {
                BtnDev.Visible = true;
            }
            //if (!CefSharp.Cef.IsInitialized)
            //{
            //    //localstorage 재사용가능해짐..
            //    CefSettings settings = new CefSettings()
            //    {
            //        CachePath = "cache",

            //    };
            //    CefSharp.Cef.Initialize(settings);

            //}

            CefSharpSettings.LegacyJavascriptBindingEnabled = true;

            easTabletViewerBound = new EasTabaletViewerJavascriptBound(this);
            
            easViewerBound.EasTabletViewerBound = easTabletViewerBound;

            easTabletViewerBound.EasViewerBound = easViewerBound;

            Browser = new ChromiumWebBrowser(manager.LOGIN_URL);
            //Browser.RegisterAsyncJsObject(manager.JavascriptBoundName, easTabletViewerBound);
            Browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            Browser.JavascriptObjectRepository.Register(manager.JavascriptBoundName, easTabletViewerBound, isAsync: true, options: BindingOptions.DefaultBinder);


            Browser.MenuHandler = new CefSharpNoContextMenu();
            Browser.LoadingStateChanged += ChromiumWebBrowser_LoadingStateChanged;
            panel1.Controls.Add(Browser);
            Browser.Dock = DockStyle.Fill;

        }
        private void ChromiumWebBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {

            string url = e.Browser.FocusedFrame.Url;
            if (url == "chrome-error://chromewebdata/")
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    MessageBox.Show("chrome-error://chromewebdata/");

                }));

            }
           
        }
        public void Write(EmrForm emrForm, EmrPatient acpEmr, EasParam easParam = null)
        {
          
            currentUrl = manager.WRITE_URL.Replace("$formNo", emrForm.FmFORMNO.ToString());
            currentUrl = currentUrl.Replace("$ptNo", acpEmr.ptNo);
            currentUrl = currentUrl.Replace("$medDrCd", acpEmr.medDrCd);
            currentUrl = currentUrl.Replace("$medDeptCd", acpEmr.medDeptCd);
            currentUrl = currentUrl.Replace("$medFrDate", acpEmr.medFrDate);
            currentUrl = currentUrl.Replace("$medFrTime", acpEmr.medFrTime);
            currentUrl = currentUrl.Replace("$inOutCls", acpEmr.inOutCls);
            currentUrl = currentUrl.Replace("$updateNo", emrForm.FmUPDATENO.ToString());

            currentUrl = currentUrl + "&isTabletMonitor=1";
            currentUrl = currentUrl + "&userId=" + clsType.User.IdNumber;
            if (easParam != null)
            {
                currentUrl = currentUrl + "&hp=" + easParam.Hp;
                currentUrl = currentUrl + "&tel=" + easParam.Tel;
                currentUrl = currentUrl + "&jumin=" + easParam.Jumin;
                currentUrl = currentUrl + "&address=" + easParam.Address;
                currentUrl = currentUrl + "&bohum=" + easParam.Bohum;
                currentUrl = currentUrl + "&bohumjong=" + easParam.BohumJong;
                currentUrl = currentUrl + "&fromadmission=" + easParam.FromAdmission;
                currentUrl = currentUrl + "&ward=" + easParam.Ward;
                currentUrl = currentUrl + "&room=" + easParam.Room;
                currentUrl = currentUrl + "&deptname=" + easParam.DeptName;
                currentUrl = currentUrl + "&doctorname=" + easParam.DoctorName;
                currentUrl = currentUrl + "&birthday=" + easParam.BirthDay;
                currentUrl = currentUrl + "&roomtype=" + easParam.RoomType;
                currentUrl = currentUrl + "&roomoveramt=" + easParam.RoomOverAmt;
                currentUrl = currentUrl + "&bohumsa=" + easParam.Bohumsa;
                currentUrl = currentUrl + "&controlinit=" + easParam.ControlInit;
            }

            //     currentUrl = "http://localhost:8080/eas/write/2342?formDataId=0&ptNo=05045622&medDrCd=2119&medDeptCd=GS&medFrDate=20180719&medFrTime=123400&inOutCls=I&isWinform=1&updateNo=2&isTabletMonitor=1";
            Browser.Load(currentUrl);


            //private string WRITE_URL = "/write/$formNo?formDataId=0&ptNo=$ptNo&medDrCd=$medDrCd&medDeptCd=$medDeptCd&medFrDate=$medFrDate&medFrTime=$medFrTime&inOutCls=$inOutCls&updateNo=$updateNo";
        }
        public void Update(EmrForm emrForm, EmrPatient acpEmr, long formDataId, EasParam easParam = null)
        {

            currentUrl = manager.UPDATE_URL.Replace("$formNo", emrForm.FmFORMNO.ToString());
            currentUrl = currentUrl.Replace("$formDataId", formDataId.ToString());
            //currentUrl = currentUrl.Replace("$medDrCd", acpEmr.medDrCd);
            //currentUrl = currentUrl.Replace("$medDeptCd", acpEmr.medDeptCd);
            //currentUrl = currentUrl.Replace("$medFrDate", acpEmr.medFrDate);
            //currentUrl = currentUrl.Replace("$medFrTime", acpEmr.medFrTime);
            //currentUrl = currentUrl.Replace("$inOutCls", acpEmr.inOutCls);
            currentUrl = currentUrl.Replace("$updateNo", emrForm.FmUPDATENO.ToString());
            currentUrl = currentUrl + "&userId=" + clsType.User.IdNumber;

            currentUrl += "&isTabletMonitor=1";
            if (easParam != null)
            {
                currentUrl = currentUrl + "&hp=" + easParam.Hp;
                currentUrl = currentUrl + "&tel=" + easParam.Tel;
                currentUrl = currentUrl + "&jumin=" + easParam.Jumin;
                currentUrl = currentUrl + "&address=" + easParam.Address;
                currentUrl = currentUrl + "&bohum=" + easParam.Bohum;
                currentUrl = currentUrl + "&bohumjong=" + easParam.BohumJong;
                currentUrl = currentUrl + "&fromadmission=" + easParam.FromAdmission;
                currentUrl = currentUrl + "&ward=" + easParam.Ward;
                currentUrl = currentUrl + "&room=" + easParam.Room;
                currentUrl = currentUrl + "&deptname=" + easParam.DeptName;
                currentUrl = currentUrl + "&doctorname=" + easParam.DoctorName;
                currentUrl = currentUrl + "&birthday=" + easParam.BirthDay;
                currentUrl = currentUrl + "&roomtype=" + easParam.RoomType;
                currentUrl = currentUrl + "&roomoveramt=" + easParam.RoomOverAmt;
                currentUrl = currentUrl + "&bohumsa=" + easParam.Bohumsa;
                currentUrl = currentUrl + "&controlinit=" + easParam.ControlInit;
            }

            //     currentUrl = "http://localhost:8080/eas/write/2342?formDataId=0&ptNo=05045622&medDrCd=2119&medDeptCd=GS&medFrDate=20180719&medFrTime=123400&inOutCls=I&isWinform=1&updateNo=2&isTabletMonitor=1";
            Browser.Load(currentUrl);


            //private string WRITE_URL = "/write/$formNo?formDataId=0&ptNo=$ptNo&medDrCd=$medDrCd&medDeptCd=$medDeptCd&medFrDate=$medFrDate&medFrTime=$medFrTime&inOutCls=$inOutCls&updateNo=$updateNo";
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Browser.ShowDevTools();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Browser.Reload();
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            frmEasViewer.Save();
        }

        private void btnEmptyPrint_Click(object sender, EventArgs e)
        {
            frmEasViewer.EmptyPrint();
        }
    }
}
