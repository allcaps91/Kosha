using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;
using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using ComHpcLibB;
using HC.Core.Dto;
using HC.Core.Repository;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Repository;
using HC.OSHA.Repository.StatusReport;
using HC_Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.IO;

namespace HC_OSHA.StatusReport
{
    public partial class StatusReportViewer : Form
    {
        private ChromiumWebBrowser browser = null;
        private readonly string JavascriptBoundName = "cefsharpBoundAsync";
        private StatusReportViewerBound bound = null;

        private StatusReportEngineerDto statusReportEngineerDto;
        private StatusReportNurseDto statusReportNurseDto;
        private StatusReportDoctorDto statusReportDoctorDto;
        private HcUserSignRepository hcUserSignRepository;
        private HIC_USERSIGN userSign = null;
        private HcUsersRepository hcUsersRepository;
        private HicMailRepository hicMailRepository;

        private long siteId;
        private string title;

        private Role USER_ROLE = Role.ENGINEER;
        private MailType MAIL_TYPE = MailType.STATUS_COMPANY_DOCTOR;
        public StatusReportViewer(string htmlFile, long siteId)
        {
            InitializeComponent();
            this.siteId = siteId;

            hcUsersRepository = new HcUsersRepository();
            hicMailRepository = new HicMailRepository();
            Initialize(htmlFile);

            //  TODO : 메일 발송정보
            HC_USER user = hcUsersRepository.FindOne(clsType.User.Sabun);
            Role USER_ROLE = Role.ENGINEER;

            if (user != null)
            {
                USER_ROLE = user.Role.ToEnum<Role>();
            }

            if (USER_ROLE == Role.NURSE)
            {
                MAIL_TYPE = MailType.STATUS_COMPANY_NURS;
            }
            else if (USER_ROLE == Role.ENGINEER)
            {
                MAIL_TYPE = MailType.STATUS_COMPANY_ENGINEER;
            }

            GetMailDate();
        }

        public void GetMailDate()
        {
            LblMailSendDate.Text = string.Empty;
            List<HIC_OSHA_MAIL_SEND> mail = hicMailRepository.FindBySiteIdAndType(this.siteId, MAIL_TYPE.ToString());

            if (mail.Count > 0)
            {
                LblMailSendDate.Text = string.Concat(mail[0].SEND_DATE.ToString("yyyy-MM-dd"), " ", mail[0].USERNAME, "(", mail[0].SEND_USER, ")");
            }
        }

        public void Initialize(string htmlFile)
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
                        defaultPage: htmlFile
                    )
                });
               
                CefSharp.Cef.Initialize(settings);
                CefSharpSettings.LegacyJavascriptBindingEnabled = true;

            }
            else
            {
                
            }
            
            bound = new StatusReportViewerBound(this);
            browser = new ChromiumWebBrowser("localfolder://cefsharp/"+ htmlFile);

            //browser.RegisterAsyncJsObject(JavascriptBoundName, bound);
            browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            browser.JavascriptObjectRepository.Register(JavascriptBoundName, bound, isAsync: true, options: BindingOptions.DefaultBinder);
            browser.MenuHandler = new CefSharpNoContextMenu();
            panel1.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;

            hcUserSignRepository = new HcUserSignRepository();
           
        }

        public void PrintStatusReportEngineerDto(StatusReportEngineerDto dto, string title)
        {
            this.statusReportEngineerDto = dto;
            userSign = hcUserSignRepository.FindOne(dto.CREATEDUSER);
            this.title = title;
        }
        public void PrintStatusReportNurseDto(StatusReportNurseDto dto, string title)
        {
            this.statusReportNurseDto = dto;
            if (dto.MODIFIEDUSER.NotEmpty())
            {
                userSign = hcUserSignRepository.FindOne(dto.MODIFIEDUSER);
            }
            else
            {
                userSign = hcUserSignRepository.FindOne(dto.CREATEDUSER);
            }
                
            this.title = title;
        }
        public void PrintStatusReportDoctorDto(StatusReportDoctorDto dto, string title)
        {
            this.statusReportDoctorDto = dto;
            userSign = hcUserSignRepository.FindOne(dto.CREATEDUSER);
            this.title = title;
        }
        public void LoadStatusReportEngineerDto()
        {
            //브라우저에서 로드 완료 되면  winform 으로 호출되도록
            if (statusReportEngineerDto != null)
            {
                string jsonString = JsonConvert.SerializeObject(statusReportEngineerDto);
                jsonString = jsonString.Replace("null", "\"\"");
                browser.ExecuteScriptAsync("SetSite('" + jsonString + "')");
                browser.ExecuteScriptAsync("SetENVCHECKJSON1('" + statusReportEngineerDto.ENVCHECKJSON1 + "')");

                statusReportEngineerDto.ENVCHECKJSON2 = statusReportEngineerDto.ENVCHECKJSON2.Replace("null", "\"\"");
                browser.ExecuteScriptAsync("SetENVCHECKJSON2('" + statusReportEngineerDto.ENVCHECKJSON2 + "')");

                statusReportEngineerDto.ENVCHECKJSON3 = statusReportEngineerDto.ENVCHECKJSON3.Replace("null", "\"\"");
                browser.ExecuteScriptAsync("SetENVCHECKJSON3('" + statusReportEngineerDto.ENVCHECKJSON3 + "')");
                browser.ExecuteScriptAsync("SetEDUTYPEJSON('" + statusReportEngineerDto.EDUTYPEJSON + "')");
                browser.ExecuteScriptAsync("SetEDUMETHODJSON('" + statusReportEngineerDto.EDUMETHODJSON + "')");
                if (statusReportEngineerDto.OPINION.NotEmpty())
                {
                    statusReportEngineerDto.OPINION = statusReportEngineerDto.OPINION.Replace("'", "\\'");
                    browser.ExecuteScriptAsync("SetOpinion('" + statusReportEngineerDto.OPINION + "')");
                }


                if (userSign != null && !userSign.IMAGEBASE64.IsNullOrEmpty())
                {
                    
                    browser.ExecuteScriptAsync("setUserSign('" + userSign.IMAGEBASE64 + "')");
                }
                if (!statusReportEngineerDto.SITEMANAGERSIGN.IsNullOrEmpty())
                {
                    //사업장 서명
                    browser.ExecuteScriptAsync("setSiteSign('" + statusReportEngineerDto.SITEMANAGERSIGN + "')");
                }
            }
        }
        public void LoadStatusReportNurseDto()
        {
            //브라우저에서 로드 완료 되면  winform 으로 호출되도록
            if (statusReportNurseDto != null)
            {
                string jsonString = JsonConvert.SerializeObject(statusReportNurseDto);
                jsonString = jsonString.Replace("null", "\"\"");
                string siteStatusDtoJson = JsonConvert.SerializeObject(statusReportNurseDto.SiteStatusDto);
                //siteStatusDtoJson = siteStatusDtoJson.Replace("\\n", "");
                siteStatusDtoJson = siteStatusDtoJson.Replace("\\r\\n", " ");

                siteStatusDtoJson = siteStatusDtoJson.Replace("null", "\"\"");
                browser.ExecuteScriptAsync("SetSite('" + jsonString + "')");
                statusReportNurseDto.PERFORMCONTENT = statusReportNurseDto.PERFORMCONTENT.Replace("null", "\"\"");
                browser.ExecuteScriptAsync("SetPERFORMCONTENT('" + statusReportNurseDto.PERFORMCONTENT + "')");
                browser.ExecuteScriptAsync("SetsiteStatusDtoJson('" + siteStatusDtoJson+ "')");
                if (statusReportNurseDto.OPINION.NotEmpty())
                {
                    statusReportNurseDto.OPINION = statusReportNurseDto.OPINION.Replace("'", "\\'");
                    browser.ExecuteScriptAsync("SetOpinion('" + statusReportNurseDto.OPINION + "')");
                }
  
                if (userSign != null && !userSign.IMAGEBASE64.IsNullOrEmpty())
                {
                    browser.ExecuteScriptAsync("setUserSign('" + userSign.IMAGEBASE64 + "')");
                }
                if (!statusReportNurseDto.SITEMANAGERSIGN.IsNullOrEmpty())
                {
                    //사업장 서명
                    browser.ExecuteScriptAsync("setSiteSign('" + statusReportNurseDto.SITEMANAGERSIGN + "')");
                }
            }
        }
        public void LoadStatusReportDoctorDto()
        {
            //브라우저에서 로드 완료 되면  winform 으로 호출되도록
            if (statusReportDoctorDto != null)
            {
                string jsonString = JsonConvert.SerializeObject(statusReportDoctorDto);
                jsonString = jsonString.Replace("null", "\"\"");
                string siteStatusDtoJson = JsonConvert.SerializeObject(statusReportDoctorDto.SiteStatusDto);
                siteStatusDtoJson = siteStatusDtoJson.Replace("\\r\\n", " ");
                siteStatusDtoJson = siteStatusDtoJson.Replace("null", "\"\"");
                browser.ExecuteScriptAsync("SetSite('" + jsonString + "')");
                statusReportDoctorDto.PERFORMCONTENT = statusReportDoctorDto.PERFORMCONTENT.Replace("null", "\"\"");
                browser.ExecuteScriptAsync("SetPERFORMCONTENT('" + statusReportDoctorDto.PERFORMCONTENT + "')");
                browser.ExecuteScriptAsync("SetsiteStatusDtoJson('" + siteStatusDtoJson + "')");
                if (statusReportDoctorDto.OPINION != null)
                {
                    //string json = "{\"opinion\":\" " +statusReportDoctorDto.OPINION+ "\"}" ;
                    //string tmp = JsonConvert.SerializeObject(json);

                    statusReportDoctorDto.OPINION = statusReportDoctorDto.OPINION.Replace("'", "\\'");
                    browser.ExecuteScriptAsync("SetOpinion('" + JsonConvert.SerializeObject(statusReportDoctorDto.OPINION) + "')");
                }
               
                if (userSign != null && !userSign.IMAGEBASE64.IsNullOrEmpty())
                {
                    //의사 이미지 서명
                    browser.ExecuteScriptAsync("setUserSign('" + userSign.IMAGEBASE64 + "')");
                }
                if (!statusReportDoctorDto.SITEMANAGERSIGN.IsNullOrEmpty())
                {
                    //사업장 서명
                    browser.ExecuteScriptAsync("setSiteSign('" + statusReportDoctorDto.SITEMANAGERSIGN + "')");
                }
            }
        }
        private void btnDevTool_Click(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            browser.Print();
            
        }

        private void BtnPdf_Click(object sender, EventArgs e)
        {
            string pdfFileName = "";
            if (statusReportEngineerDto != null)
            {
                pdfFileName = this.title + "_" + statusReportEngineerDto.VISITDATE;
            }
            else if (statusReportNurseDto != null)
            {
                pdfFileName = this.title + "_" + statusReportNurseDto.VISITDATE;
            }
            else if (statusReportDoctorDto != null)
            {
                pdfFileName = this.title + "_" + statusReportDoctorDto.VISITDATE;
            }
                
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PDF |*.pdf";
            saveFileDialog1.Title = "PDF로 저장하기";
            saveFileDialog1.FileName = pdfFileName+".pdf";
            saveFileDialog1.ShowDialog();
          
            if (saveFileDialog1.FileName != "")
            {
                browser.PrintToPdfAsync(saveFileDialog1.FileName);
            }
            //   browser.PrintToPdfAsync
        }

        private void BtnSendMail_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                HcCodeService codeService = new HcCodeService();
                HC_CODE pdfPath = codeService.FindActiveCodeByGroupAndCode("PDF_PATH", "OSHA_ESTIMATE", "OSHA");
              
                string pdfFileName = pdfPath.CodeName + "\\" + title +"_"+ DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";

                browser.PrintToPdfAsync(pdfFileName);
                Thread.Sleep(2000);
                //pdf파일 생성여부 확인 및 파일크기 점검
                for (int i = 0; i < 15; i++)
                {
                    Thread.Sleep(1000);
                    if (File.Exists(pdfFileName))
                    {
                        FileInfo info = new FileInfo(pdfFileName);
                        if (info.Length > 25000000)
                        {
                            MessageUtil.Alert("첨부파일 25MB 초과로 전송이 불가능합니다.");
                            return;
                        }
                        break;
                    }
                }

                HC_CODE sendMailAddress = codeService.FindActiveCodeByGroupAndCode("OSHA_MANAGER", "mail", "OSHA");
                EstimateMailForm form = new EstimateMailForm();
                form.GetMailForm().SenderMailAddress = sendMailAddress.CodeName;
                form.GetMailForm().AttachmentsList.Add(pdfFileName);
                form.set_SiteId(this.siteId);
                //보낼 메일을 찾기
                long estimateID = 0;
                if (statusReportEngineerDto != null)
                {
                    estimateID = statusReportEngineerDto.ESTIMATE_ID;
                }
                else if (statusReportNurseDto != null)
                {
                    estimateID = statusReportNurseDto.ESTIMATE_ID;
                }
                else if (statusReportDoctorDto != null)
                {
                    estimateID = statusReportDoctorDto.ESTIMATE_ID;
                }

                string strMailList = GetEmail(this.siteId);
                if (strMailList != "")
                {
                    for (int i=1;i<=VB.L(strMailList,",");i++)
                    {
                        form.GetMailForm().ReciverMailSddress.Add(VB.Pstr(strMailList, ",", i).Trim());
                    }
                }
                form.GetMailForm().RefreshReceiver();
                form.GetMailForm().Subject = title;
                form.GetMailForm().Body = title;

                //  form.GetMailForm().AttachmentsList.Add(pdfFileName);
                DialogResult result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    HIC_OSHA_MAIL_SEND mail = new HIC_OSHA_MAIL_SEND
                    {
                        SITE_ID = this.siteId,
                        SEND_TYPE = MAIL_TYPE.ToString(),
                        SEND_USER = clsType.User.Sabun
                    };

                    int res = hicMailRepository.Insert(mail);

                    GetMailDate();
                }
            }
            catch (Exception ex)
            {
                MessageUtil.Alert(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private string GetEmail(long Site_ID)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strEMail = "";
            string strList = "";
            string email = string.Empty;
            string strNow = DateTime.Now.ToString("yyyy-MM-dd");

            SQL = "";
            SQL = "SELECT NAME,EMAIL FROM HIC_OSHA_CONTRACT_MANAGER ";
            SQL = SQL + ComNum.VBLF + "WHERE ESTIMATE_ID IN (SELECT ESTIMATE_ID FROM HIC_OSHA_CONTRACT ";
            SQL = SQL + ComNum.VBLF + "      WHERE OSHA_SITE_ID=" + Site_ID + " ";
            SQL = SQL + ComNum.VBLF + "        AND CONTRACTSTARTDATE<='" + strNow + "' ";
            SQL = SQL + ComNum.VBLF + "        AND CONTRACTENDDATE>='" + strNow + "' ";
            SQL = SQL + ComNum.VBLF + "        AND ISDELETED='N') ";
            SQL = SQL + ComNum.VBLF + "  AND WORKER_ROLE='HEALTH_ROLE' ";
            SQL = SQL + ComNum.VBLF + "  AND ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND EMAIL IS NOT NULL ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strEMail = dt.Rows[i]["EMAIL"].ToString().Trim();
                    if (strEMail != "")
                    {
                        if (strList == "")
                        {
                            strList = strEMail;
                        }
                        else
                        {
                            strList += "," + strEMail;
                        }
                    }
                }
            }
            dt.Dispose();
            dt = null;

            return strList;
        }

        private void BtnSign_Click(object sender, EventArgs e)
        {
            SignPadForm form = new SignPadForm(false);
            form.Location = new Point(0, 0);

            if (form.ShowDialog() == DialogResult.OK)
            {
                string image = form.Base64Image;

                browser.ExecuteScriptAsync("setSiteSign('" + image + "')");

                if (this.statusReportEngineerDto != null)
                {
                    StatusReportEngineerRepository repo = new StatusReportEngineerRepository();
                    repo.UpdateSign(statusReportEngineerDto.ID, image);
                }
                else if (this.statusReportNurseDto != null)
                {
                    StatusReportNurseRepository repo = new StatusReportNurseRepository();
                    repo.UpdateSign(statusReportNurseDto.ID, image);
                }
                else if (this.statusReportDoctorDto != null)
                {
                    StatusReportDoctorRepository repo = new StatusReportDoctorRepository();
                    repo.UpdateSign(statusReportDoctorDto.ID, image);
                }
            }
        }

        private void StatusReportViewer_Load(object sender, EventArgs e)
        {

        }

        private void ChkOSHA_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkOSHA.Checked)
            {
                browser.ExecuteScriptAsync("setOSHA();");
            }
            else
            {
                browser.ExecuteScriptAsync("unSetOSHA();");
            }
        }

        private void Rdo3_CheckedChanged(object sender, EventArgs e)
        {
            if (Rdo3.Checked)
            {
                browser.ExecuteScriptAsync("blank4Hide();");  
            }
            else
            {
                browser.ExecuteScriptAsync("blank4Show();");
            }
        }
    }
}
