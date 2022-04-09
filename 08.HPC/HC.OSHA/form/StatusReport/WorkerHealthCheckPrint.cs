using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using HC.Core.Common.Util;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Repository;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Repository;
using HC.OSHA.Repository.StatusReport;
using HC.OSHA.Service.StatusReport;
using HC_Core;
using HC_Core.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Threading;

namespace HC_OSHA.StatusReport
{
    public partial class WorkerHealthCheckPrint : CommonForm
    {
        private HealthCheckService healthCheckService;
        private HcUsersRepository hcUsersRepository;
        private HicMailRepository hicMailRepository;

        private StatusReportNurseDto statusReportNurseDto;
        private StatusReportDoctorDto statusReportDoctorDto;
        private StatusReportNurseRepository statusReportNurseRepository;
        private StatusReportDoctorRepository statusReportDoctorRepository;

        private Role USER_ROLE = Role.ENGINEER;
        private MailType MAIL_TYPE = MailType.STATUS_WORKER_DOCTOR;

        public WorkerHealthCheckPrint()
        {
            InitializeComponent();
            healthCheckService = new HealthCheckService();
            hcUsersRepository = new HcUsersRepository();
            hicMailRepository = new HicMailRepository();

            statusReportDoctorRepository = new StatusReportDoctorRepository();
            statusReportNurseRepository = new StatusReportNurseRepository();


            SSView.Columns()[9].Visible = false;
            SSView.Columns()[10].Visible = false;

            //  TODO : 메일 발송정보
            HC_USER user = hcUsersRepository.FindOne(clsType.User.Sabun);
            Role USER_ROLE = Role.ENGINEER;

            if (user != null)
            {
                USER_ROLE = user.Role.ToEnum<Role>();
            }

            if (USER_ROLE == Role.NURSE)
            {
                MAIL_TYPE = MailType.STATUS_WORKER_NURS;
            }
            else if (USER_ROLE == Role.ENGINEER)
            {
                MAIL_TYPE = MailType.STATUS_WORKER_ENGINEER;
            }

        }
        public void GetMailDate()
        {
            LblMailSendDate.Text = string.Empty;
            List<HIC_OSHA_MAIL_SEND> mail = hicMailRepository.FindBySiteIdAndType(this.SelectedSite.ID, MAIL_TYPE.ToString());

            if (mail.Count > 0)
            {
                LblMailSendDate.Text = string.Concat(mail[0].SEND_DATE.ToString("yyyy-MM-dd"), " ", mail[0].USERNAME, "(", mail[0].SEND_USER, ")");
            }
        }

        public void SetStatusReportNurseDto(StatusReportNurseDto statusReportNurseDto, ISiteModel SelectedSite)
        {
            this.statusReportNurseDto = statusReportNurseDto;
            this.SelectedSite = SelectedSite;

            GetMailDate();
        }
        public void SetStatusReportDoctorDto(StatusReportDoctorDto statusReportDoctorDto, ISiteModel SelectedSite)
        {
            this.statusReportDoctorDto = statusReportDoctorDto;
            this.SelectedSite = SelectedSite;

            GetMailDate();
        }

        private void Search()
        {
            string strStartDate = "";
            string strEndDate = "";

            if (SelectedSite == null)
            {
                MessageUtil.Alert("사업장 정보가 없습니다");
                return;
            }
            long reprotid = 0;
            strStartDate = "";
            strEndDate = "";
            if (statusReportNurseDto != null)
            {
                reprotid = statusReportNurseDto.ID;
                strStartDate = VB.Left(statusReportNurseDto.VISITDATE,6) + "01";
                strEndDate = VB.Left(statusReportNurseDto.VISITDATE,6) + "31";
            }
            else if (statusReportDoctorDto != null)
            {
                reprotid = statusReportDoctorDto.ID;
                strStartDate = VB.Left(statusReportDoctorDto.VISITDATE,6) + "01";
                strEndDate = VB.Left(statusReportDoctorDto.VISITDATE,6) + "31";
            }
            List<HealthCheckDto> list = healthCheckService.healthCheckRepository.FindAll(0, reprotid, "", "", ChkDel.Checked);

            LblCount.Text = "총 : " + list.Count + " 건";

            SSCard.ActiveSheet.RowCount = 2;
            SSView.ActiveSheet.RowCount = 2;
            int row = 2;

            for (int i = 0; i < list.Count; i++)
            {
                SSCard.ActiveSheet.RowCount++;
                SSView.ActiveSheet.RowCount++;
                SSCard.ActiveSheet.Cells[row, 0].Value = list[i].CHARTDATE.Substring(0, 4) + "-" + list[i].CHARTDATE.Substring(4, 2) + "-" + list[i].CHARTDATE.Substring(6, 2);
                SSCard.ActiveSheet.Cells[row, 0].ColumnSpan = 1;

                SSCard.ActiveSheet.Cells[row, 1].Value = list[i].name;

                SSCard.ActiveSheet.Cells[row, 2].Value = list[i].dept;
                if (list[i].sabun.IsNullOrEmpty()==false) SSCard.ActiveSheet.Cells[row, 2].Value += "(" + list[i].sabun + ")";

                SSCard.ActiveSheet.Cells[row, 3].Value = list[i].gender;
                if (list[i].age>0) SSCard.ActiveSheet.Cells[row, 3].Value += "(" + list[i].age + ")";
                SSCard.ActiveSheet.Cells[row, 4].ColumnSpan = 2;
                SSCard.ActiveSheet.Cells[row, 6].ColumnSpan = 2;

                string exam = string.Empty;
                if (!list[i].bpl.IsNullOrEmpty())
                {
                    exam += "혈압: " + list[i].bpl + "/" + list[i].bpr + " ";
                }
                if (!list[i].bst.IsNullOrEmpty())
                {
                    exam += "혈당: " + list[i].bst + " ";
                }
                if (!list[i].dan.IsNullOrEmpty())
                {
                    exam += "단백뇨: " + list[i].dan + " ";
                }
                if (!list[i].WEIGHT.IsNullOrEmpty())
                {
                    exam += "체중: " + list[i].WEIGHT + " ";
                }
                if (!list[i].BMI.IsNullOrEmpty())
                {
                    exam += "체지방: " + list[i].BMI + " ";
                }
                if (!list[i].ALCHOL.IsNullOrEmpty())
                {
                    exam += "음주량: " + list[i].ALCHOL + " ";
                }
                if (!list[i].SMOKE.IsNullOrEmpty())
                {
                    exam += "흡연량: " + list[i].SMOKE + " ";
                }

                if (exam.Empty() == true)
                {
                    SSCard.ActiveSheet.Cells[row, 4].Text = list[i].content;
                }
                else
                {
                    SSCard.ActiveSheet.Cells[row, 4].Text = exam + "\r\n\r\n" + list[i].content;
                }
                SSCard.ActiveSheet.Cells[row, 6].Text = list[i].suggestion;

                SSView.ActiveSheet.Cells[row, 0].Text = SSCard.ActiveSheet.Cells[row, 0].Text;
                SSView.ActiveSheet.Cells[row, 1].Text = SSCard.ActiveSheet.Cells[row, 1].Text;
                SSView.ActiveSheet.Cells[row, 2].Text = SSCard.ActiveSheet.Cells[row, 2].Text;
                SSView.ActiveSheet.Cells[row, 3].Text = SSCard.ActiveSheet.Cells[row, 3].Text;
                SSView.ActiveSheet.Cells[row, 4].Text = SSCard.ActiveSheet.Cells[row, 4].Text;
                SSView.ActiveSheet.Cells[row, 6].Text = SSCard.ActiveSheet.Cells[row, 6].Text;
                SSView.ActiveSheet.Cells[row, 4].HorizontalAlignment = CellHorizontalAlignment.Left;
                SSView.ActiveSheet.Cells[row, 6].HorizontalAlignment = CellHorizontalAlignment.Left;
                SSView.ActiveSheet.Cells[row, 9].Text = list[i].IsDeleted;
                SSView.ActiveSheet.Cells[row, 10].Text = list[i].id.ToString();

                SSView.ActiveSheet.Cells[row, 4].ColumnSpan = 2;
                SSView.ActiveSheet.Cells[row, 6].ColumnSpan = 2;
                ButtonCellType buttonCellType = new ButtonCellType();
                buttonCellType.Text = "Y".Equals(list[i].IsDeleted) ? "복구" : "삭제";
                buttonCellType.TextColor = "Y".Equals(list[i].IsDeleted) ? Color.Blue : Color.Red;
                SSView.ActiveSheet.Cells[row, 8].CellType = buttonCellType;

                SSCard.ActiveSheet.Rows[row].Height = SSCard.ActiveSheet.Rows[row].GetPreferredHeight() + 30;
                SSView.ActiveSheet.Rows[row].Height = SSView.ActiveSheet.Rows[row].GetPreferredHeight() + 30;
                row = row + 1;
            }

            //서명
            row = SSCard.AddRows();
            SSCard.ActiveSheet.Cells[row, 0].ColumnSpan = 4;
            SSCard.ActiveSheet.Cells[row, 4].Row.Height = 50;
            SSCard.ActiveSheet.Cells[row, 0].HorizontalAlignment = CellHorizontalAlignment.Left;
            SSCard.ActiveSheet.Cells[row, 4].Value = "담당자:  홍길동";

            SSView.AddRows();
            SSView.ActiveSheet.Cells[row, 0].ColumnSpan = 4;
            SSView.ActiveSheet.Cells[row, 4].Row.Height = 50;
            SSView.ActiveSheet.Cells[row, 0].HorizontalAlignment = CellHorizontalAlignment.Left;
            SSView.ActiveSheet.Cells[row, 4].Value = "담당자:  홍길동";

            //    SSCard.ActiveSheet.Cells[row, 5].Value = "근로자         (서명)";

            if (this.statusReportDoctorDto != null)
            {
                statusReportDoctorDto = this.statusReportDoctorRepository.FindOne(statusReportDoctorDto.ID);
                if (statusReportDoctorDto != null)
                {
                    SetSiteSign(statusReportDoctorDto.SANGDAMSIGN);
                    SSCard.ActiveSheet.Cells[row, 4].Value = "담당자:  " + statusReportDoctorDto.SITEMANAGERNAME;
                    SSView.ActiveSheet.Cells[row, 4].Value = "담당자:  " + statusReportDoctorDto.SITEMANAGERNAME;

                    HcUserSignRepository hcUserSignRepository = new HcUserSignRepository();
                    HIC_USERSIGN userSign = hcUserSignRepository.FindOne(statusReportDoctorDto.CREATEDUSER);
                    if (userSign != null)
                    {
                        if (userSign.IMAGEBASE64 != null)
                        {
                            SetUserSign(userSign.IMAGEBASE64);
                        }
                    }

                    SSCard.ActiveSheet.Cells[row, 6].Value = "상담자:  " + statusReportDoctorDto.DOCTORNAME;
                    SSView.ActiveSheet.Cells[row, 6].Value = "상담자:  " + statusReportDoctorDto.DOCTORNAME;
                }
            }
            else if (this.statusReportNurseDto != null)
            {
                statusReportNurseDto = this.statusReportNurseRepository.FindOne(statusReportNurseDto.ID);
                if (statusReportNurseDto != null)
                {
                    SetSiteSign(statusReportNurseDto.SANGDAMSIGN);
                    SSCard.ActiveSheet.Cells[row, 4].Value = "담당자:  " + statusReportNurseDto.SITEMANAGERNAME;
                    SSView.ActiveSheet.Cells[row, 4].Value = "담당자:  " + statusReportNurseDto.SITEMANAGERNAME;

                    HcUserSignRepository hcUserSignRepository = new HcUserSignRepository();
                    HIC_USERSIGN userSign = null;
                    if(statusReportNurseDto.MODIFIEDUSER.NotEmpty())
                    {
                        userSign = hcUserSignRepository.FindOne(statusReportNurseDto.MODIFIEDUSER);
                    }
                    else
                    {
                        userSign = hcUserSignRepository.FindOne(statusReportNurseDto.CREATEDUSER);
                    }

                    if (userSign != null)
                    {
                        if (userSign.IMAGEBASE64 != null)
                        {
                            SetUserSign(userSign.IMAGEBASE64);
                        }
                        SSCard.ActiveSheet.Cells[row, 6].Value = "상담자:  " + statusReportNurseDto.NURSENAME;
                        SSView.ActiveSheet.Cells[row, 6].Value = "상담자:  " + statusReportNurseDto.NURSENAME;
                    }
                }
            }

            for(int i = SSCard.RowCount() - 1; i>= 0; i--)
            {
                if(SSView.Cell(i, 9).Text.Equals("Y"))
                {
                    SSCard.ActiveSheet.RemoveRows(i, 1);
                }
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            PrintSignModel model = new PrintSignModel();
            if (this.statusReportDoctorDto != null)
            {
                statusReportDoctorDto = this.statusReportDoctorRepository.FindOne(statusReportDoctorDto.ID);
                if (statusReportDoctorDto != null)
                {
                    model.SiteManagerSign = SetSiteSign(statusReportDoctorDto.SANGDAMSIGN);
                    model.SiteManagerName = "담당자: " + statusReportDoctorDto.SITEMANAGERNAME;

                    HcUserSignRepository hcUserSignRepository = new HcUserSignRepository();
                    HIC_USERSIGN userSign = hcUserSignRepository.FindOne(statusReportDoctorDto.CREATEDUSER);
                    if (userSign.IMAGEBASE64 != null)
                    {
                        model.OshaSign = SetUserSign(userSign.IMAGEBASE64);
                        model.OshaName = "상담자:  " + statusReportDoctorDto.DOCTORNAME;
                    }

                }
            }
            else if (this.statusReportNurseDto != null)
            {

                statusReportNurseDto = this.statusReportNurseRepository.FindOne(statusReportNurseDto.ID);
                if (statusReportNurseDto != null)
                {
                    model.SiteManagerSign = SetSiteSign(statusReportNurseDto.SANGDAMSIGN);
                    model.SiteManagerName = "담당자: " + statusReportNurseDto.SITEMANAGERNAME;

                    HcUserSignRepository hcUserSignRepository = new HcUserSignRepository();
                    HIC_USERSIGN userSign = null;
                    if (statusReportNurseDto.MODIFIEDUSER.NotEmpty())
                    {
                        userSign = hcUserSignRepository.FindOne(statusReportNurseDto.MODIFIEDUSER);
                    }
                    else
                    {
                        userSign = hcUserSignRepository.FindOne(statusReportNurseDto.CREATEDUSER);
                    }

                    if (userSign != null)
                    {
                        if (userSign.IMAGEBASE64 != null)
                        {
                            model.OshaSign = SetUserSign(userSign.IMAGEBASE64);
                            model.OshaName = "상담자:  " + statusReportNurseDto.NURSENAME;
                        }
                    }

                }

            }


            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.STANDARD_APPROVAL_SIGN);
            if (this.SelectedSite != null)
            {
                print.SiteName = SelectedSite.NAME;
            }
            print.PrintSignModel = model;
            print.Execute();
        }

        public Image Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes))
            {
                Image image = Image.FromStream(ms);
                return image;
            }
        }
        private void BtnSign_Click(object sender, EventArgs e)
        {
            SignPadForm form = new SignPadForm(false);
            form.Location = new Point(0, 0);

            if (form.ShowDialog() == DialogResult.OK)
            {
                //image = "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";

                SetSiteSign(form.Base64Image);

                if (this.statusReportDoctorDto != null)
                {
                    StatusReportDoctorRepository repo = new StatusReportDoctorRepository();
                    repo.UpdateSangdamSign(statusReportDoctorDto.ID, form.Base64Image);
                }
                else if (this.statusReportNurseDto != null)
                {
                    StatusReportNurseRepository repo = new StatusReportNurseRepository();
                    repo.UpdateSangdamSign(statusReportNurseDto.ID, form.Base64Image);
                }
            }
        }

        private Image SetSiteSign(string base64)
        {
            Image signImage = null;
            Image signImage2 = null;
            if (!base64.IsNullOrEmpty())
            {
                string[] image = base64.Split(',');
                signImage = Base64ToImage(image[1]);
                signImage2 = (Image)signImage.Clone();

                //사업장
                ImageCellType imageCellType = new ImageCellType();
                imageCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;
                SSCard.ActiveSheet.Cells[SSCard.ActiveSheet.RowCount - 1, 5].CellType = imageCellType;
                
                ImageCellType imageCellType2 = new ImageCellType();
                imageCellType2.Style = FarPoint.Win.RenderStyle.StretchAndScale;
                SSView.ActiveSheet.Cells[SSView.ActiveSheet.RowCount - 1, 5].CellType = imageCellType2;

                if (signImage != null)
                {
                    SSCard.ActiveSheet.Cells[SSCard.ActiveSheet.RowCount - 1, 5].Value = signImage;
                    SSView.ActiveSheet.Cells[SSView.ActiveSheet.RowCount - 1, 5].Value = signImage2;
                }
                
               
            }
            return signImage;
        }
        private Image SetUserSign(string base64)
        {
            Image signImage = null;
            Image signImage2 = null;
            if (!base64.IsNullOrEmpty())
            {
                string[] image = base64.Split(',');
                signImage = Base64ToImage(image[1]);
                signImage2 = (Image)signImage.Clone();

                //상담자 
                ImageCellType imageCellType = new ImageCellType();
                imageCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;
                SSCard.ActiveSheet.Cells[SSCard.ActiveSheet.RowCount - 1, 7].CellType = imageCellType;

                ImageCellType imageCellType2 = new ImageCellType();
                imageCellType2.Style = FarPoint.Win.RenderStyle.StretchAndScale;
                SSView.ActiveSheet.Cells[SSView.ActiveSheet.RowCount - 1, 7].CellType = imageCellType2;

                if (signImage != null)
                {
                    SSCard.ActiveSheet.Cells[SSCard.ActiveSheet.RowCount - 1, 7].Value = signImage;
                    SSView.ActiveSheet.Cells[SSView.ActiveSheet.RowCount - 1, 7].Value = signImage2;
                }
                
            }
            return signImage;
        }
        private void WorkerHealthCheckPrint_Load(object sender, EventArgs e)
        {
            Search();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedSite == null)
                {
                    MessageUtil.Alert("사업장 정보가 없습니다");
                    return;
                }
           

                PrintSignModel model = new PrintSignModel();
                if (this.statusReportDoctorDto != null)
                {
                    statusReportDoctorDto = this.statusReportDoctorRepository.FindOne(statusReportDoctorDto.ID);
                    if (statusReportDoctorDto != null)
                    {
                        model.SiteManagerSign = SetSiteSign(statusReportDoctorDto.SANGDAMSIGN);
                        model.SiteManagerName = "담당자: " + statusReportDoctorDto.SITEMANAGERNAME;

                        HcUserSignRepository hcUserSignRepository = new HcUserSignRepository();
                        HIC_USERSIGN userSign = hcUserSignRepository.FindOne(statusReportDoctorDto.CREATEDUSER);
                        if (userSign.IMAGEBASE64 != null)
                        {
                            model.OshaSign = SetUserSign(userSign.IMAGEBASE64);
                            model.OshaName = "상담자:  " + statusReportDoctorDto.DOCTORNAME;
                        }

                    }
                }
                else if (this.statusReportNurseDto != null)
                {
                    statusReportNurseDto = this.statusReportNurseRepository.FindOne(statusReportNurseDto.ID);
                    if (statusReportNurseDto != null)
                    {
                        model.SiteManagerSign = SetSiteSign(statusReportNurseDto.SANGDAMSIGN);
                        model.SiteManagerName = "담당자: " + statusReportNurseDto.SITEMANAGERNAME;

                        HcUserSignRepository hcUserSignRepository = new HcUserSignRepository();
                        HIC_USERSIGN userSign = null;
                        if(statusReportNurseDto.MODIFIEDUSER.NotEmpty())
                        {
                            userSign = hcUserSignRepository.FindOne(statusReportNurseDto.MODIFIEDUSER);
                        }
                        else
                        {
                            userSign = hcUserSignRepository.FindOne(statusReportNurseDto.CREATEDUSER);
                        }
                        
                        if (userSign != null)
                        {
                            if (userSign.IMAGEBASE64 != null)
                            {
                                model.OshaSign = SetUserSign(userSign.IMAGEBASE64);
                                model.OshaName = "상담자:  " + statusReportNurseDto.NURSENAME;
                            }
                        }

                    }

                }
               
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "PDF |*.pdf";
                saveFileDialog1.Title = "PDF로 저장하기";
                saveFileDialog1.ShowDialog();
                if (saveFileDialog1.FileName != "")
                {
                    SpreadPrint sp = new SpreadPrint(SSCard, PrintStyle.STANDARD_APPROVAL_SIGN, true); ;
                    if (this.SelectedSite != null)
                    {
                        sp.SiteName = SelectedSite.NAME;
                    }
                    sp.PrintSignModel = model;
                    sp.ExportPDF(saveFileDialog1.FileName);

                    MessageUtil.Info("PDF를 저장하였습니다");
                }
                
            }
            catch(Exception ex)
            {
                MessageUtil.Alert(ex.Message);
            }
         
        
        }

        private void BtnSendMail_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedSite == null)
                {
                    MessageUtil.Alert("사업장 정보가 없습니다");
                    return;
                }
             
                long siteId = 0;
                long estimateID = 0;
                string strGbn = "";
              
                if (this.statusReportDoctorDto != null)
                {
                    siteId = statusReportDoctorDto.SITE_ID;
                    estimateID = statusReportDoctorDto.ESTIMATE_ID;
                    strGbn = "의사";
                }
                else if (this.statusReportNurseDto != null)
                {
                    siteId = statusReportNurseDto.SITE_ID;
                    estimateID = statusReportNurseDto.ESTIMATE_ID;
                    strGbn = "간호사";
                }

                Cursor.Current = Cursors.WaitCursor;
                HcCodeService codeService = new HcCodeService();
                HC_CODE pdfPath = codeService.FindActiveCodeByGroupAndCode("PDF_PATH", "OSHA_ESTIMATE", "OSHA");
                string title = "근로자 건강상담_" + strGbn + "_" + SelectedSite.NAME;
                string pdfFileName = pdfPath.CodeName + "\\" + title + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.STANDARD_APPROVAL, false);
                if (this.SelectedSite != null)
                {
                    print.SiteName = SelectedSite.NAME;
                }
                print.ExportPDFNoWait(pdfFileName, SSCard.ActiveSheet);
                Thread.Sleep(2000);

                HC_CODE sendMailAddress = codeService.FindActiveCodeByGroupAndCode("OSHA_MANAGER", "mail", "OSHA");
                EstimateMailForm form = new EstimateMailForm();
                form.set_SiteId(siteId);
                form.GetMailForm().SenderMailAddress = sendMailAddress.CodeName;
                form.GetMailForm().AttachmentsList.Add(pdfFileName);

                string strMailList = GetEmail(estimateID);
                if (strMailList != "")
                { 
                    for (int i = 1; i <= VB.L(strMailList, ","); i++)
                    {
                        form.GetMailForm().ReciverMailSddress.Add(VB.Pstr(strMailList, ",", i).Trim());
                    }
                    form.GetMailForm().RefreshReceiver();
                }
                form.GetMailForm().Subject = title;
                form.GetMailForm().Body = title;

                DialogResult result = form.ShowDialog();
                
                if(result == DialogResult.OK)
                {
                    HIC_OSHA_MAIL_SEND mail = new HIC_OSHA_MAIL_SEND
                    {
                        SITE_ID = this.SelectedSite.ID,
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

        private string GetEmail(long ESTIMATE_ID)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strEMail = "";
            string strList = "";
            string email = string.Empty;

            SQL = "";
            SQL = "SELECT NAME,EMAIL FROM HIC_OSHA_CONTRACT_MANAGER ";
            SQL = SQL + ComNum.VBLF + "WHERE ESTIMATE_ID=" + ESTIMATE_ID + " ";
            SQL = SQL + ComNum.VBLF + "  AND (EMAILSEND='Y' OR EMAILSEND='y') ";
            SQL = SQL + ComNum.VBLF + "  AND WORKER_ROLE='HEALTH_ROLE' ";
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

        private void SSView_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            //  col 9 IsDeleted, 10 : Id
            long id = SSView.Cell(e.Row, 10).Text.To<long>(0);
            string IsDeleted = SSView.Cell(e.Row, 9).Text;

            if(IsDeleted.Equals("N"))
            {
                IsDeleted = "Y";
            }
            else
            {
                IsDeleted = "N";
            }

            healthCheckService.healthCheckRepository.PrintDeleteUpdate(id, IsDeleted);

            Search();
        }

        private void ChkDel_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
