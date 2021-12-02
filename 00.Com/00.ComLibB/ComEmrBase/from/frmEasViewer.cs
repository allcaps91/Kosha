using CefSharp;
using CefSharp.WinForms;
using ComBase;
using ComBase.Controls;
using PdfiumViewer;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmEasViewer : Form, EmrChartForm
    {
        public delegate void ExitDelegate(EmrForm emrForm, EmrPatient emrPatient, string formDataId);
        public event ExitDelegate exitDelegate;

        public bool IsAutoCloseBySave { get; set; }

        public ChromiumWebBrowser Browser { get; set; }
        private EasManager manager = null;
        private EasViewerJavascriptBound bound = null;
        public BrowserMode BrowserMode { get; set; }
        private string currentUrl = string.Empty;
        private int printedCount = 0;

        private EmrPatient emrPatient = null;
        private EmrForm emrForm = null;
        private frmEmrNewHisView frmEmrNewHisView = null;
        //     private bool isOcr = false;
        private string ocrNo = string.Empty;
        private long NowformDataId = 0;

        public frmEasTabletViewer TabletViewer { get; set; }

        public delegate void OnPrintCompleted(int printedPageCount);
        //   private OnPrintCompleted onPrintCompleted = null;
        ManualResetEvent manualEvent = new ManualResetEvent(false);

        public frmEasViewer(EasManager manager)
        {
            InitializeComponent();

            this.manager = manager;

            Initialize(manager);
        }

        private void Initialize(EasManager manager)
        {
            btnDevTool.Visible = false;
            if (clsType.User.Sabun == "800594"|| clsType.User.Sabun == "40024" || clsType.User.Sabun == "36540")
            {
                btnDevTool.Visible = true;
            }
            BrowserMode = BrowserMode.Login;

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

            bound = new EasViewerJavascriptBound(this);
            Browser = new ChromiumWebBrowser(manager.LOGIN_URL);
            //Browser.RegisterAsyncJsObject(manager.JavascriptBoundName, bound);
            Browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            Browser.JavascriptObjectRepository.Register(manager.JavascriptBoundName, bound, isAsync: true, options: BindingOptions.DefaultBinder);

            Browser.MenuHandler = new CefSharpNoContextMenu();
            Browser.Margin = new Padding(0);

            panel1.AllowDrop = true;
            this.KeyPreview = true;
            this.AllowDrop = true;
            panel1.Controls.Add(Browser);
            Browser.Dock = DockStyle.Fill;

            Browser.LoadingStateChanged += ChromiumWebBrowser_LoadingStateChanged;
        }

        public EasViewerJavascriptBound GetBound()
        {
            return this.bound;
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
        public void LoadUrl()
        {
            Browser.Load(currentUrl);

        }

        public void Write(EmrForm emrForm, EmrPatient acpEmr, EasParam easParam = null)
        {
            bound.FormDataId = "";

            this.emrForm = emrForm;
            this.emrPatient = acpEmr;

            currentUrl = manager.WRITE_URL.Replace("$formNo", emrForm.FmFORMNO.ToString());
            currentUrl = currentUrl.Replace("$ptNo", acpEmr.ptNo);
            currentUrl = currentUrl.Replace("$medDrCd", acpEmr.medDrCd);
            currentUrl = currentUrl.Replace("$medDeptCd", acpEmr.medDeptCd);
            currentUrl = currentUrl.Replace("$medFrDate", acpEmr.medFrDate);
            currentUrl = currentUrl.Replace("$medFrTime", acpEmr.medFrTime);
            currentUrl = currentUrl.Replace("$inOutCls", acpEmr.inOutCls);
            currentUrl = currentUrl.Replace("$updateNo", emrForm.FmUPDATENO.ToString());
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

            //currentUrl = "http://localhost:8080/eas/write/2342?formDataId=0&ptNo=05045622&medDrCd=2119&medDeptCd=GS&medFrDate=20180719&medFrTime=123400&inOutCls=I&isWinform=1&updateNo=2";

            Browser.Load(currentUrl);

            WriteMode();

            //private string WRITE_URL = "/write/$formNo?formDataId=0&ptNo=$ptNo&medDrCd=$medDrCd&medDeptCd=$medDeptCd&medFrDate=$medFrDate&medFrTime=$medFrTime&inOutCls=$inOutCls&updateNo=$updateNo";
        }

        public void EmptyPrint()
        {
            currentUrl = manager.PRINT_URL.Replace("$formNo", emrForm.FmFORMNO.ToString());
            currentUrl = currentUrl.Replace("{}", string.Empty);
            currentUrl = currentUrl + emrForm.FmUPDATENO;
            currentUrl = currentUrl + "&userId=" + clsType.User.IdNumber;
            Browser.Load(currentUrl);
            bound.SetPrintPageLoadCompleted();
        }
        public void Print(EmrForm emrForm, EmrPatient acpEmr, string formDataId)
        {
            this.emrForm = emrForm;
            this.emrPatient = acpEmr;

            //currentUrl = manager.UPDATE_URL.Replace("$formNo", emrForm.FmFORMNO.ToString());
            currentUrl = manager.PRINT_URL.Replace("$formNo", emrForm.FmFORMNO.ToString());
           
            currentUrl = currentUrl.Replace("{}", formDataId);
            currentUrl = currentUrl + emrForm.FmUPDATENO + "&userId=" + clsType.User.IdNumber;

            Browser.Load(currentUrl);
            //manualEvent.WaitOne();//프린트 대기
            
            //bound.SetPrintPageLoadCompleted();


        }

        public void WaitPrint()
        {
            manualEvent.WaitOne(1000 * 60);//프린트 대기
        }
        public void Update(EmrForm emrForm, EmrPatient acpEmr, long formDataId, EasParam easParam)
        {
            this.emrForm = emrForm;
            this.emrPatient = acpEmr;

            currentUrl = manager.UPDATE_URL.Replace("$formNo", emrForm.FmFORMNO.ToString());
            currentUrl = currentUrl.Replace("$formDataId", formDataId.ToString());
            //currentUrl = currentUrl.Replace("$medDrCd", acpEmr.medDrCd);
            //currentUrl = currentUrl.Replace("$medDeptCd", acpEmr.medDeptCd);
            //currentUrl = currentUrl.Replace("$medFrDate", acpEmr.medFrDate);
            //currentUrl = currentUrl.Replace("$medFrTime", acpEmr.medFrTime);
            //currentUrl = currentUrl.Replace("$inOutCls", acpEmr.inOutCls);
            currentUrl = currentUrl.Replace("$updateNo", emrForm.FmUPDATENO.ToString());
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
            //       currentUrl = "http://localhost:8080/eas/write/2342?formDataId=0&ptNo=05045622&medDrCd=2119&medDeptCd=GS&medFrDate=20180719&medFrTime=123400&inOutCls=I&isWinform=1&updateNo=2";
            Browser.Load(currentUrl);

            WriteMode();
        }

        public void HistoryView(EmrForm emrForm, EmrPatient acpEmr, long formNo, string easFormDataId, string easFormDataHistoryId)
        {

            this.emrForm = emrForm;
            this.emrPatient = acpEmr;

            currentUrl = manager.HISTORY2_URL.Replace("$formNo", formNo.ToString());
            currentUrl = currentUrl.Replace("{0}", easFormDataId);
            currentUrl = currentUrl.Replace("{1}", easFormDataHistoryId);
            currentUrl = currentUrl + emrForm.FmUPDATENO + "&userId=" + clsType.User.IdNumber;
            Browser.Load(currentUrl);

            ViewMode();
        }

        public void HistoryView(EmrForm emrForm, EmrPatient acpEmr, long formNo, string easFormDataHistoryId)
        {

            this.emrForm = emrForm;
            this.emrPatient = acpEmr;

            currentUrl = manager.HISTORY_URL.Replace("$formNo", formNo.ToString());
            currentUrl = currentUrl.Replace("{}", easFormDataHistoryId);
            currentUrl = currentUrl + emrForm.FmUPDATENO;
            Browser.Load(currentUrl);

            ViewMode();
        }

        public void View(EmrForm emrForm, EmrPatient acpEmr, long formNo, long formDataId)
        {
          

            if (formDataId == 0)
            {
                throw new Exception("formDataId가 없습니다");
            }
            this.emrForm = emrForm;
            this.emrPatient = acpEmr;
            this.NowformDataId = formDataId;

            currentUrl = manager.VIEW_URL.Replace("$formNo", formNo.ToString());
            currentUrl = currentUrl.Replace("{}", formDataId.ToString());
            currentUrl = currentUrl + emrForm.FmUPDATENO + "&userId=" + clsType.User.IdNumber; 
            Browser.Load(currentUrl);

            ViewMode();
        }
        public void ShowDev()
        {
            Browser.ShowDevTools();
        }
        private void ViewMode()
        {
            BrowserMode = BrowserMode.View;

            //    btnEmptyPrint.Visible = false;

            btnSave.Visible = false;
            btnDelete.Visible = false;
        }
        private void WriteMode()
        {
            BrowserMode = BrowserMode.Write;
            btnEmptyPrint.Visible = true;
            btnSave.Visible = true;
            btnDelete.Visible = true;
            this.bound.viewer = this;
        }

        /// <summary>
        /// print.js 에서 호출 
        /// </summary>
        public void Print()
        {
            int pageNumber = 0;
            string pdfFileName = this.manager.GetPdfFileName(emrForm.FmFORMNO, emrForm.FmUPDATENO);

            CefSharp.PdfPrintSettings setting = new CefSharp.PdfPrintSettings();
            //       setting.BackgroundsEnabled = true;
            //setting.ScaleFactor = 70;
            setting.ScaleFactor = 120;
          

            //Task<bool> task = Browser.PrintToPdfAsync(pdfFileName, setting);
            Task<bool> task = Browser.PrintToPdfAsync(pdfFileName, setting);
            try
            {
                task.Wait();
                bool isOcr = false;
                if (task.IsCompleted && task.Result)
                {
                    Thread.Sleep(200);
                    PdfDocument pdfDoc = PdfDocument.Load(pdfFileName);
                    printedCount = pdfDoc.PageCount;

                    if (emrForm.FmDOCPRINTHEAD == 0) // 타이틀만
                    {

                    }
                    else if (emrForm.FmDOCPRINTHEAD == 1 || emrForm.FmDOCPRINTHEAD == 3) //환자정보(1)+OCR, OCR만(3)
                    {
                        isOcr = true;

                        ocrNo = manager.GetOcrNo(emrForm, emrPatient, printedCount);
                    }


                    PrintDocument doc = pdfDoc.CreatePrintDocument();


                    if (!string.IsNullOrEmpty(this.manager.PrinterName))
                    {
                        doc.PrinterSettings.PrinterName = this.manager.PrinterName;
                    }
                    PageSettings pg = new PageSettings();
                    pg.Margins = new Margins(0, 0, 0, 0);

                    doc.PrintPage += (s, e) =>
                    {

                        pageNumber += 1;
                        Rectangle printAbleRect = Rectangle.Round(e.PageSettings.PrintableArea);

                        if (emrPatient != null)
                        {
                            Font titleFont = new Font("Arial", 16f, FontStyle.Bold);
                            Font font = new Font("굴림", 9f, FontStyle.Bold);
                            StringFormat CenterFormat = new StringFormat();
                            CenterFormat.Alignment = StringAlignment.Center;
                            CenterFormat.LineAlignment = StringAlignment.Center;
                            Rectangle titleHeader = new Rectangle();
                            titleHeader.X = 0 + printAbleRect.X;
                            titleHeader.Y = 15 + printAbleRect.Y;
                            titleHeader.Width = printAbleRect.Width;
                            titleHeader.Height = 115;
                            float startX = 50 + printAbleRect.X;
                            float plusY = 20 + printAbleRect.Y;

                            if (isOcr)
                            {
                                Rectangle ocrRect = new Rectangle(0, 5, printAbleRect.Width, 50);
                                e.Graphics.DrawString(ocrNo, titleFont, Brushes.Black, ocrRect, CenterFormat);
                            }


                            e.Graphics.DrawString(emrForm.FmFORMNAME, titleFont, Brushes.Black, titleHeader, CenterFormat);



                          if (emrForm.FmDOCPRINTHEAD == 1 || emrForm.FmDOCPRINTHEAD == 4) //환자정보
                            {

                                e.Graphics.DrawString("등록번호 : " + emrPatient.ptNo, font, Brushes.Black, startX, 70 + plusY);
                                e.Graphics.DrawString("성     명 : " + emrPatient.ptName + " (" + emrPatient.age + "/" + emrPatient.sex + " )", font, Brushes.Black, startX, 90 + plusY);
                                e.Graphics.DrawString("주민번호 : " + emrPatient.ssno1 + "-" + emrPatient.ssno2.Substring(0, 1), font, Brushes.Black, startX, 110 + plusY);
                                e.Graphics.DrawString("진료과/주치의(입원일자) : " + emrPatient.medDeptKorName + " / " + emrPatient.medDrName + " (" + emrPatient.medFrDate + ")", font, Brushes.Black, startX, 130 + plusY);
                            }

                            if (isOcr)
                            {
                                e.Graphics.DrawString("출력자(출력일자) : " + clsType.User.UserName + " (" + DateTime.Now.ToString("yyyy-MM-dd") + ")", font, Brushes.Black, startX, 150 + plusY);
                            }
                            else
                            {
                                if (emrPatient.writeName.NotEmpty())
                                {
                                    e.Graphics.DrawString("작성자(작성일자) : " + emrPatient.writeName + " (" + emrPatient.writeDate + ")", font, Brushes.Black, startX, 150 + plusY);
                                }
                                if (emrForm.FmDOCPRINTHEAD != 2)
                                {
                                    e.Graphics.DrawString("출력자(출력일자) : " + clsType.User.UserName + " (" + DateTime.Now.ToString("yyyy-MM-dd") + ")", font, Brushes.Black, startX + 440, 150 + plusY);
                                }

                            }


                            //   string image64 = "data:image/png;base64,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gAQIECBAgACBMQKC4hh3vRIgQIAAAQIEphcQFKcvkQESIECAAAECBMYICIpj3PVKgAABAgQIEJheQFCcvkQGSIAAAQIECBAYIyAojnHXKwECBAgQIEBgegFBcfoSGSABAgQIECBAYIyAoDjGXa8ECBAgQIAAgekFBMXpS2SABAgQIECAAIExAoLiGHe9EiBAgAABAgSmFxAUpy+RARIgQIAAAQIExggIimPc9UqAAAECBAgQmF5AUJy+RAZIgAABAgQIEBgjICiOcdcrAQIECBAgQGB6AUFx+hIZIAECBAgQIEBgjICgOMZdrwQIECBAgACB6QUExelLZIAECBAgQIAAgTECguIYd70SIECAAAECBKYXEBSnL5EBEiBAgAABAgTGCAiKY9z1SoAAAQIECBCYXkBQnL5EBkiAAAECBAgQGCMgKI5x1ysBAgQIECBAYHoBQXH6EhkgAQIECBAgQGCMgKA4xl2vBAgQIECAAIHpBQTF6UtkgAQIECBAgACBMQKC4hh3vRIgQIAAAQIEphcQFKcvkQESIECAAAECBMYICIpj3PVKgAABAgQIEJheQFCcvkQGSIAAAQIECBAYIyAojnHXKwECBAgQIEBgegFBcfoSGSABAgQIECBAYIyAoDjGXa8ECBAgQIAAgekFBMXpS2SABAgQIECAAIExAv8LQihX2Gl3IuEAAAAASUVORK5CYII=";

                            //       e.Graphics.DrawImage(ConvertStringtoImage(image64), titleHeader);
                            Pen cPen = new Pen(Color.Black);
                            cPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                            cPen.Width = 2;

                            float endLineX = printAbleRect.Width - (startX - 10);
                            e.Graphics.DrawLine(cPen, startX - 10, 170 + plusY, endLineX, 170 + plusY);

                            int pageheight = e.PageBounds.Height;
                            int wwww = e.MarginBounds.X;
                            //footer line
                            //e.Graphics.DrawLine(cPen, startX - 10, 1110 + plusY, 800, 1110 + plusY);
                            e.Graphics.DrawLine(cPen, startX - 10, printAbleRect.Height - 50, endLineX, printAbleRect.Height - 50);


                            Rectangle pageRect = new Rectangle();
                            pageRect.X = 0;
                            pageRect.Y = printAbleRect.Height - 30;
                            pageRect.Width = printAbleRect.Width;
                            pageRect.Height = 15;
                            e.Graphics.DrawString(pageNumber.ToString(), font, Brushes.Black, pageRect, CenterFormat);

                            //   e.Graphics.DrawLine(cPen, 0, 10, 10, 10);



                        }

                    };

                    doc.Print();
                    doc.Dispose();
                    pdfDoc.Dispose();

                    manualEvent.Set();
                    this.manager.ClearPdfFoler();

                }
                else
                {

                    MessageBox.Show("인쇄 실패");
                    manualEvent.Set();
                }

                isOcr = false;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                manualEvent.Set();
                MessageBox.Show(ex.Message);
            }

        }



        /// <summary>
        /// 저장된 차트 서식 인쇄
        /// </summary>
        /// <param name="formNo"></param>
        /// <param name="updateNo"></param>
        /// <param name="formDataId"></param>
        //public void Print(OnPrintCompleted onPrintCompleted, long formNo, int updateNo, long formDataId, EmrForm emrForm = null, EmrPatient emrPatient = null)
        //{
        //    this.emrPatient = emrPatient;
        //    this.emrForm = emrForm;
        //    this.onPrintCompleted = new OnPrintCompleted(onPrintCompleted);

        //    currentUrl = manager.PRINT_URL.Replace("$formNo", formNo.ToString());
        //    currentUrl = currentUrl.Replace("{}", formDataId.ToString());
        //    currentUrl = currentUrl + updateNo;

        //    if (bound.IsSigned)
        //    {

        //    }
        //    browser.Load(currentUrl);
        //    manualEvent.WaitOne();
        //}

        private void btnDevTool_Click(object sender, EventArgs e)
        {

            Browser.ShowDevTools();
        }

        private void btnEmptyPrint_Click(object sender, EventArgs e)
        {
            //btnDevTool.Visible = true;
            EmptyPrint();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {

        }

        public void ClearFormMsg()
        {
            throw new NotImplementedException();
        }

        public bool DelDataMsg()
        {
            throw new NotImplementedException();
        }

        public int PrintFormMsg(string strPRINTFLAG)
        {
            return this.printedCount;
        }

        public double SaveDataMsg(string strFlag)
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

        public void SetUserFormMsg(double dblMACRONO)
        {
            throw new NotImplementedException();
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            throw new NotImplementedException();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
           
        }
        public async void Save()
        {
           // IsAutoCloseBySave = true;
            // String xx = Browser.EvaluateScriptAsync("saveFormData(saveHandler);").Result.Message;
            bool isClose = false;
           await Browser.EvaluateScriptAsync("saveFormData(saveHandler);").ContinueWith(x =>
            {
                var response = x.Result.Result;

                if (IsAutoCloseBySave)
                {
                    if (response==null)
                    {
                        var t = Task.Delay(1000);
                        t.Wait();
                        isClose = true;
                       
                    }

                }
            });
            if (isClose)
            {
                this.Close();
            }

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            Browser.ExecuteScriptAsync("deleteChart();");
        }

        private void BtnTabletMonitor_Click(object sender, EventArgs e)
        {
            ShowTabletViewer(null);
        }
        public void LocationToRight()
        {
            Screen screen = null;
            Screen[] screens = Screen.AllScreens;
            if (screens.Length > 1)
            {
                foreach (Screen scr in screens)
                {
                    if (scr.WorkingArea.Width != 720)
                    {
                        screen = scr;
                    }
                }
            }

            if (screen != null)
            {
                this.BringToFront();
                this.Location = new Point(950, 50);
            }
        }
        public void ShowTabletViewer(EasParam easParam, long formDataId = 0)
        {
            var t = Task.Delay(1000);
            t.Wait();
            bool isHasTableMonitor = false;
            Screen tableMonitorScreen = null;
            Screen[] screens = Screen.AllScreens;
            if (screens.Length > 1)
            {
                foreach (Screen scr in screens)
                {
                    if (scr.WorkingArea.Width == 720)
                    {
                        isHasTableMonitor = true;
                        tableMonitorScreen = scr;

                    }
                }
            }
            if (isHasTableMonitor)
            {
                TabletViewer = manager.GetEasTablerViewer();
                TabletViewer.frmEasViewer = this;
                this.bound.EasTabletViewerBound = TabletViewer.easTabletViewerBound;
                if (formDataId > 0)
                {
                    TabletViewer.Update(this.emrForm, this.emrPatient, formDataId, easParam);
                }
                else
                {
                    TabletViewer.Write(this.emrForm, this.emrPatient, easParam);
                }

                TabletViewer.Show();
                TabletViewer.BringToFront();
                TabletViewer.Location = new Point(tableMonitorScreen.Bounds.Left, 0);
                TabletViewer.WindowState = FormWindowState.Maximized;
                //브라우저 동기화
                this.bound.EasTabletViewerBound = TabletViewer.easTabletViewerBound;
                this.bound.EasTabletViewerBound.EasViewerBound = this.bound;
            }
            else
            {
                //개발테스트중
                btnDevTool.Visible = true;
                Log.Debug("아큐 없음");
                TabletViewer = manager.GetEasTablerViewer();
                TabletViewer.frmEasViewer = this;

                if (formDataId > 0)
                {
                    TabletViewer.Update
                        (this.emrForm, this.emrPatient, formDataId, easParam);
                }
                else
                {
                    TabletViewer.Write(this.emrForm, this.emrPatient, easParam);
                }
                TabletViewer.Show();
                TabletViewer.BringToFront();
                TabletViewer.WindowState = FormWindowState.Maximized;
                this.bound.EasTabletViewerBound = TabletViewer.easTabletViewerBound;
                this.bound.EasTabletViewerBound.EasViewerBound = this.bound;
            }


        }
        private void BtnSignByPatient_Click(object sender, EventArgs e)
        {
            bound.EasTabletViewerBound.OpenSignByPatient();

        }

        private void panTitle_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtnHis_Click(object sender, EventArgs e)
        {
            if (frmEmrNewHisView != null)
            {
                if (!frmEmrNewHisView.IsDisposed)
                {
                    frmEmrNewHisView.BringToFront();
                    frmEmrNewHisView.Show();
                    return;
                }

            }

            frmEmrNewHisView = new frmEmrNewHisView(this.emrPatient, this.emrForm, NowformDataId.ToString());
            frmEmrNewHisView.Show();
        }

        private void frmEasViewer_Load(object sender, EventArgs e)
        {
            Screen[] screens = Screen.AllScreens;

            foreach (Screen scr in screens)
            {

                if (scr.WorkingArea.Width == 912 || scr.WorkingArea.Width == 1386)
                {
                    BtnMaximize.Visible = true;
                }
            }

            #region 기록실, 관리자 권한
            if (clsType.User.BuseCode.Equals("044201") && clsType.User.AuAMANAGE.Equals("1"))
            {
                BtnHis.Visible = true;
            }
            #endregion


            //BtnMaximize
        }

        private void BtnMaximize_Click(object sender, EventArgs e)
        {
            //ShowTabletViewer();
            if (this.manager.isLeft)
            {
                BtnMaximize.Text = "<<";
                this.manager.RightMoveSplitter();
            }
            else
            {
                BtnMaximize.Text = ">>";
                this.manager.LeftMoveSplitter();
            }

        }

        private void frmEasViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TabletViewer != null)
            {
                if (!TabletViewer.IsDisposed)
                {
                    TabletViewer.Close();
                }
            }

            if (exitDelegate != null)
            {
                this.exitDelegate(this.emrForm, this.emrPatient, bound.FormDataId);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Print(this.emrForm, this.emrPatient, this.NowformDataId.ToString());
           // ShowDev();
        }
    }
}
