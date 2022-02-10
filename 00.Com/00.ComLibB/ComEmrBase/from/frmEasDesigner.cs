using CefSharp;
using CefSharp.WinForms;
using ComBase;
using PdfiumViewer;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// http://localhost:8080/eas/winform URL로 브라우저 객체를 생성한다
    /// 위 URL 자바스크립트에서 자동 로그인을 시도하여 인증문제를 해소함..
    /// </summary>
    public partial class frmEasDesigner : Form, EmrChartForm
    {
        private ChromiumWebBrowser browser = null;

        private EasManager manager = null;

        private int formNo = 0;
        private int updateNo = 0;

        EmrPatient emrPatient = null;
        EmrForm emrForm = null;

        private JavascriptBoundCefSharp bound = null;
        private string currentUrl = string.Empty;
        public BrowserMode BrowserMode { get; set; }

        public delegate void OnPrintCompleted(int printedPageCount);

        private OnPrintCompleted onPrintCompleted = null;

        ManualResetEvent manualEvent = new ManualResetEvent(false);

        int printedCount = 0;
        public frmEasDesigner(EasManager manager)
        {
            InitializeComponent();
            this.manager = manager;
            Initialize(manager);
        }

        private void Initialize(EasManager manager)
        {
            BrowserMode = BrowserMode.Login;

            //if (!CefSharp.Cef.IsInitialized)
            //{
            //    //localstorage 재사용가능해짐..
            //    CefSettings settings = new CefSettings()
            //    {
            //        CachePath = "cache",

            //    };
            //    bool isInit = CefSharp.Cef.Initialize(settings);

            //}
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;

            bound = new JavascriptBoundCefSharp(this);
            browser = new ChromiumWebBrowser(manager.LOGIN_URL);
            //browser.RegisterAsyncJsObject(manager.JavascriptBoundName, bound);
            browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            browser.JavascriptObjectRepository.Register(manager.JavascriptBoundName, bound, isAsync: true, options: BindingOptions.DefaultBinder);

            browser.MenuHandler = new CefSharpNoContextMenu();

            panel1.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;

            //   browser.IsBrowserInitializedChanged += Brower_IsBrowserInitializedChanged;
            browser.LoadingStateChanged += ChromiumWebBrowser_LoadingStateChanged;

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
            if (e.IsLoading == false)
            {/*
                this.Invoke(new MethodInvoker(delegate ()
                {
                    if (this.BrowserMode == BrowserMode.Preview){
                        BtnPrint.Show();
                    }
                    else
                    {
                        BtnPrint.Hide();
                    }
                   
                }));
                */
                //     BtnPrint.Enabled = true;

                //if (url.Equals(LOGIN_URL))
                //{
                //    brower.ExecuteScriptAsync("cefSharp_Connect('');"); //웹에서 관리화면으로 분기함.
                //    brower.ExecuteScriptAsync("document.getElementById('username').value='"+ UserId + "'");
                //    brower.ExecuteScriptAsync("document.getElementById('password').value='"+ Password +"'");
                //    brower.ExecuteScriptAsync("document.getElementById('btnLogin').click();");
                //}
                //else if (url.Equals(FORM_MANAGER_URL))
                //{
                //    brower.ExecuteScriptAsync("cefSharp_Connect('');");

                //    //   brower.ExecuteScriptAsync("cefSharp_managerFormReady();");

                //}

            }
        }
        public void LoadUrl()
        {
            browser.Load(currentUrl);

        }



        /// <summary>
        /// 서식 미리보기
        /// </summary>
        /// <param name="formNo"></param>
        /// <param name="updateNo"></param>
        public void Preview(int formNo, int updateNo)
        {
            this.formNo = formNo;
            this.updateNo = updateNo;
            currentUrl = manager.PREVIEW_URL.Replace("$formNo", formNo.ToString());
            currentUrl = currentUrl.Replace("{}", "0");
            currentUrl = currentUrl + updateNo;

            //if (bound.IsSigned)
            //{
            //    browser.Load(currentUrl);          
            //}
            browser.Load(currentUrl);
            BrowserMode = BrowserMode.Preview;

        }
        /// <summary>
        /// 저장된 차트 서식 인쇄
        /// </summary>
        /// <param name="formNo"></param>
        /// <param name="updateNo"></param>
        /// <param name="formDataId"></param>
        public void SavedChartPrint(OnPrintCompleted onPrintCompleted, long formNo, int updateNo, long formDataId, EmrForm emrForm = null, EmrPatient emrPatient = null)
        {
            this.emrPatient = emrPatient;
            this.emrForm = emrForm;
            this.onPrintCompleted = new OnPrintCompleted(onPrintCompleted);
            //onPrintCompleted.wait
            currentUrl = manager.PRINT_URL.Replace("$formNo", formNo.ToString());
            currentUrl = currentUrl.Replace("{}", formDataId.ToString());
            currentUrl = currentUrl + "&userId=" + clsType.User.IdNumber;
            currentUrl = currentUrl + updateNo;

            if (bound.IsSigned)
            {

            }
            browser.Load(currentUrl);
            manualEvent.WaitOne();
        }

        /// <summary>
        /// 서식 수정 정보를 설정후, 웹에서 로드가 완료되면 JavascriptBoundCefSharp.ReadyFormMain 을 호출한다.
        /// </summary>
        /// <param name="formNo"></param>
        /// <param name="updateNo"></param>
        public void EditUrl(int formNo, int updateNo)
        {
            this.formNo = formNo;
            this.updateNo = updateNo;
            currentUrl = manager.FORM_MANAGER_URL;
            //browser.ExecuteScriptAsync("editForm(" + formNo + ", " + updateNo + ");");
            if (bound.IsSigned)
            {
                browser.Load(currentUrl);
            }
        }

        /// <summary>
        /// 디자이너 폼이 생성되어 있을경우에만 사용가능
        /// </summary>
        /// <param name="formNo"></param>
        /// <param name="updateNo"></param>
        public void Edit(int formNo, int updateNo)
        {

            this.formNo = formNo;
            this.updateNo = updateNo;
            Edit();
        }
        public void Edit()
        {
            BrowserMode = BrowserMode.Edit;
            if (this.formNo > 0)
            {
                browser.ExecuteScriptAsync("editForm(" + formNo + ", " + updateNo + ");");
            }
            else
            {
                MessageBox.Show("서식을  선택하세요");
            }
        }




        private void btnDevTool_Click(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }

        private void frmEasDesigner_FormClosing(object sender, FormClosingEventArgs e)
        {

            //Cef.Shutdown(); // CefSharp.BrowserSubprocess 가 종료되어 버림 어플리케이션 종료시 호출할것
        }


        private CefSharp.PdfPrintSettings GetPrintSetting()
        {
            CefSharp.PdfPrintSettings printSettings = new CefSharp.PdfPrintSettings();
            printSettings.Landscape = false;
            printSettings.BackgroundsEnabled = true;
            printSettings.HeaderFooterEnabled = false;
            printSettings.MarginType = CefPdfPrintMarginType.Custom;
            printSettings.MarginTop = 130;
            printSettings.MarginLeft = 30;
            printSettings.MarginRight = 30;
            printSettings.MarginBottom = 30;

            return printSettings;
        }
        private CefSharp.PdfPrintSettings GetPrintSettingByPatientEmpty()
        {
            CefSharp.PdfPrintSettings printSettings = new CefSharp.PdfPrintSettings();
            printSettings.Landscape = false;
            printSettings.BackgroundsEnabled = true;
            printSettings.HeaderFooterEnabled = false;
            /* c#에서 처리하지않고 웹에서 print 미디어 쿼리로 여백을 지정하도록 한다.
            printSettings.MarginType = CefPdfPrintMarginType.Custom;
            printSettings.MarginTop = 100;
            printSettings.MarginLeft = 97;
            printSettings.MarginRight = 97;
            */
            printSettings.MarginType = CefPdfPrintMarginType.Default;

            return printSettings;
        }

        /// <summary>
        /// 사용 안함
        /// </summary>
        /// <returns></returns>
        public int Print()
        {

            //프린터 형식으로..

            // EmrPatient emrPatient
            // EmrForm emrForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, formNo.ToString(), updateNo.ToString());
            // clsEmrChart.SetEmrPatInfoEas(clsDB.DbCon, strEmrNo);

            Log.Debug("동의서 인쇄 시작 formNo:{}, updateNo:{}", formNo, updateNo);
            String folderName = "C:\\HealthSoft\\pdfprint";
            DirectoryInfo di = new DirectoryInfo(folderName);
            if (!di.Exists)
            {
                di.Create();
            }
            string pdfFileName = @"C:\HealthSoft\pdfprint\" + formNo + "_" + "version_" + "_" + VB.Format(DateTime.Now, "yyyyMMHHhhmmss.fff") + ".pdf";
            //CefSharp.PdfPrintSettings printSettings = null;

            //printSettings = GetPrintSettingByPatientEmpty();

            if (emrPatient == null)
            {
                //printSettings = GetPrintSetting();
                emrPatient = new EmrPatient()
                {
                    ptNo = "00000000",
                    ptName = "홍길동",
                    age = "00",
                    sex = "남",
                    ssno1 = "000000",
                    ssno2 = "000000"


                };
            }
            else
            {

            }

            Task<bool> task = browser.PrintToPdfAsync(pdfFileName);

            try
            {

                task.Wait();

                if (task.IsCompleted && task.Result)
                {
                    Thread.Sleep(200);
                    PdfDocument pdfDoc = PdfDocument.Load(pdfFileName);
                    printedCount = pdfDoc.PageCount;


                    PrintDocument doc = pdfDoc.CreatePrintDocument();

                    //PageSettings ss = new PageSettings();
                    //ss.Margins = new Margins(0, 0, 0, 0);
                    //doc.DefaultPageSettings = ss;

                    if (!string.IsNullOrEmpty(this.manager.PrinterName))
                    {
                        doc.PrinterSettings.PrinterName = this.manager.PrinterName;
                    }

                    doc.PrintPage += (s, e) =>
                    {
                        if (emrPatient != null)
                        {
                            Font titleFont = new Font("굴림", 16f, FontStyle.Bold);
                            Font font = new Font("굴림", 9f, FontStyle.Bold);
                            StringFormat CenterFormat = new StringFormat();
                            CenterFormat.Alignment = StringAlignment.Center;
                            CenterFormat.LineAlignment = StringAlignment.Center;
                            Rectangle titleHeader = new Rectangle();
                            titleHeader.X = 0;
                            titleHeader.Y = 20;
                            titleHeader.Width = 827;
                            titleHeader.Height = 115;
                            float startX = 60;
                            float plusY = 20;
                            e.Graphics.DrawString("시술/검사전 기록지(Receive)", titleFont, Brushes.Black, titleHeader, CenterFormat);
                            e.Graphics.DrawString("등록번호 : " + emrPatient.ptNo, font, Brushes.Black, startX, 70 + plusY);
                            e.Graphics.DrawString("성     명 : " + emrPatient.ptName + " (" + emrPatient.age + "/" + emrPatient.sex + " )", font, Brushes.Black, startX, 90 + plusY);
                            e.Graphics.DrawString("주민번호 : " + emrPatient.ssno1 + "-" + emrPatient.ssno2.Substring(0, 1), font, Brushes.Black, startX, 110 + plusY);
                            e.Graphics.DrawString("진료과/주치의(입원일자) : " + emrPatient.medDeptKorName + " / (" + emrPatient.medFrDate + ")", font, Brushes.Black, startX, 130 + plusY);
                            e.Graphics.DrawString("작성자(작성일자) : 홍길동 (2019-00-00 00:00)", font, Brushes.Black, startX, 150 + plusY);
                            e.Graphics.DrawString("출력자(출력일자) : " + clsType.User.UserName + " (" + new DateTime().ToString("yyyy-MM-dd") + ")", font, Brushes.Black, 550, 150 + plusY);

                            //   string image64 = "data:image/png;base64,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gAQIECBAgACBMQKC4hh3vRIgQIAAAQIEphcQFKcvkQESIECAAAECBMYICIpj3PVKgAABAgQIEJheQFCcvkQGSIAAAQIECBAYIyAojnHXKwECBAgQIEBgegFBcfoSGSABAgQIECBAYIyAoDjGXa8ECBAgQIAAgekFBMXpS2SABAgQIECAAIExAoLiGHe9EiBAgAABAgSmFxAUpy+RARIgQIAAAQIExggIimPc9UqAAAECBAgQmF5AUJy+RAZIgAABAgQIEBgjICiOcdcrAQIECBAgQGB6AUFx+hIZIAECBAgQIEBgjICgOMZdrwQIECBAgACB6QUExelLZIAECBAgQIAAgTECguIYd70SIECAAAECBKYXEBSnL5EBEiBAgAABAgTGCAiKY9z1SoAAAQIECBCYXkBQnL5EBkiAAAECBAgQGCMgKI5x1ysBAgQIECBAYHoBQXH6EhkgAQIECBAgQGCMgKA4xl2vBAgQIECAAIHpBQTF6UtkgAQIECBAgACBMQKC4hh3vRIgQIAAAQIEphcQFKcvkQESIECAAAECBMYICIpj3PVKgAABAgQIEJheQFCcvkQGSIAAAQIECBAYIyAojnHXKwECBAgQIEBgegFBcfoSGSABAgQIECBAYIyAoDjGXa8ECBAgQIAAgekFBMXpS2SABAgQIECAAIExAv8LQihX2Gl3IuEAAAAASUVORK5CYII=";

                            //       e.Graphics.DrawImage(ConvertStringtoImage(image64), titleHeader);
                            Pen cPen = new Pen(Color.Black);
                            cPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                            cPen.Width = 2;
                            e.Graphics.DrawLine(cPen, startX - 10, 170 + plusY, 800, 170 + plusY);

                            //footer line
                            e.Graphics.DrawLine(cPen, startX - 10, 1100 + plusY, 800, 1100 + plusY);
                        }

                    };
                    //  doc.pri
                    doc.Print();
                    pdfDoc.Dispose();
                    if (onPrintCompleted != null)
                    {
                        onPrintCompleted(printedCount);

                    }

                    this.manager.ClearPdfFoler();

                    manualEvent.Set();

                    //        FileInfo file = new FileInfo(pdfFileName);
                    //        file.Delete();

                }
                else
                {
                    MessageBox.Show("인쇄 실패");
                    manualEvent.Set();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                manualEvent.Set();
                MessageBox.Show(ex.Message);
            }

            return printedCount;
        }
        private Image ConvertStringtoImage(string commands)
        {

            byte[] photoarray = Convert.FromBase64String(commands);
            MemoryStream ms = new MemoryStream(photoarray, 0, photoarray.Length);
            ms.Write(photoarray, 0, photoarray.Length);
            Image image = System.Drawing.Image.FromStream(ms);
            return image;

        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Preview(this.formNo, this.updateNo);
            //string pdfFileName = @"C:\HealthSoft\pdfprint\" + formNo +"_"+ VB.Format(DateTime.Now, "yyyyMMHHhhmmss.fff") + ".pdf";
            //Print(formNo, updateNo , 0);

        }

        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("aaaaa 하하하", new Font("맑은 고딕", 10f, FontStyle.Bold), Brushes.Black, 7, 5, new StringFormat());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //print(formNo, updateNo, new EmrPatient());
            Print();

        }

        public double SaveDataMsg(string strFlag)
        {
            throw new NotImplementedException();
        }

        public bool DelDataMsg()
        {
            throw new NotImplementedException();
        }

        public void ClearFormMsg()
        {
            throw new NotImplementedException();
        }

        public void SetUserFormMsg(double dblMACRONO)
        {
            throw new NotImplementedException();
        }

        public bool SaveUserFormMsg(double dblMACRONO)
        {
            throw new NotImplementedException();
        }

        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 프린트된 페이지 수
        /// </summary>
        /// <param name="strPRINTFLAG"></param>
        /// <returns></returns>
        public int PrintFormMsg(string strPRINTFLAG)
        {
            return this.printedCount;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            throw new NotImplementedException();
        }

        private void frmEasDesigner_Load(object sender, EventArgs e)
        {

        }
    }

    public enum BrowserMode
    {
        Login, Edit, Preview, View, Write,
    }
}
