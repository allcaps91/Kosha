using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Utils;
using ComHpcLibB;
using ComHpcLibB.Model;
using FarPoint.Win.Spread;
using HC.Core.Dto;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Model;
using HC.OSHA.Repository;
using HC.OSHA.Repository.StatusReport;
using HC.OSHA.Service;
using HC.OSHA.Service.StatusReport;
using HC_OSHA.StatusReport;
using HC_Core;
using HC_OSHA.form.StatusReport;
using HC_OSHA.Model.StatussReport;
using HC_OSHA.Repository.StatusReport;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_OSHA.StatusReport
{
    public partial class StatusReportByDoctor : CommonForm, ISelectSite
    {
        private StatusReportDoctorService statusReportDoctorService;
        private HcSiteViewService hcSiteViewService;
        private MacrowordService macrowordService;
        private WorkerHealthCheckForm workerHealthCheckForm;
        private DoctorOpinionForm doctorOpinionForm;
        private HcOshaContractRepository hcOshaContractRepository;
        private HcSiteWorkerService hcSiteWorkerService;
        private StatusReportViewer statusReportViewer;
        private AutoCompleteMacro autoCompleteMacro;
        private StatusReportMemoRepository statusReportMemoRepository;

        public StatusReportByDoctor()
        {
            InitializeComponent();
            statusReportDoctorService = new StatusReportDoctorService();
            hcSiteViewService = new HcSiteViewService();
            workerHealthCheckForm = new WorkerHealthCheckForm();
            doctorOpinionForm = new DoctorOpinionForm();
            macrowordService = new MacrowordService();
            hcOshaContractRepository = new HcOshaContractRepository();
            hcSiteWorkerService = new HcSiteWorkerService();
            statusReportMemoRepository = new StatusReportMemoRepository();
        }

        private void StatusReportByDoctor_Load(object sender, EventArgs e)
        {
            autoCompleteMacro = new AutoCompleteMacro(this.Name);
            autoCompleteMacro.Add(TxtIsEduTitle);
            autoCompleteMacro.Add(textBox1);
            autoCompleteMacro.Add(textBox2);
            autoCompleteMacro.Add(textBox5);
            autoCompleteMacro.Add(textBox4);
            autoCompleteMacro.Add(textBox10);
            autoCompleteMacro.Add(textBox3);
            autoCompleteMacro.Add(textBox11);
            autoCompleteMacro.Add(textBox12);
            autoCompleteMacro.Add(textBox6);
            autoCompleteMacro.Add(textBox7);
            autoCompleteMacro.Add(textBox8);
            this.Controls.Add(autoCompleteMacro);

            workerHealthCheckForm.Dock = DockStyle.Fill;
            workerHealthCheckForm.FormBorderStyle = FormBorderStyle.None;
            workerHealthCheckForm.TopLevel = false;
            workerHealthCheckForm.IsDoctor = true;
            workerHealthCheckForm.Show();
            tabPage2.Controls.Add(workerHealthCheckForm);

            doctorOpinionForm.Dock = DockStyle.Fill;
            doctorOpinionForm.FormBorderStyle = FormBorderStyle.None;
            doctorOpinionForm.TopLevel = false;
            doctorOpinionForm.Show();
            tabPage3.Controls.Add(doctorOpinionForm);

            DtpVisitDate.SetOptions(new DateTimePickerOption { DataField = nameof(StatusReportDoctorDto.VISITDATE), DataBaseFormat = DateTimeType.YYYYMMDD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpVisitReserveDate.SetOptions(new DateTimePickerOption { DataField = nameof(StatusReportDoctorDto.VISITRESERVEDATE), DataBaseFormat = DateTimeType.YYYYMMDD, DisplayFormat = DateTimeType.YYYY_MM_DD });

            dateTimePicker1.SetOptions(new DateTimePickerOption { DataField = nameof(DoctorPerformContentJson.sDate), DataBaseFormat = DateTimeType.YYYYMMDD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            radioButton1.SetOptions(new RadioButtonOption { DataField = nameof(DoctorPerformContentJson.bogunRadio), CheckValue = "Y", UnCheckValue = "N" });
            radioButton2.SetOptions(new RadioButtonOption { DataField = nameof(DoctorPerformContentJson.bogunRadio), CheckValue = "N", UnCheckValue = "Y" });
            ChkIsEduKind1.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            ChkIsEduKind2.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            ChkIsEduKind3.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            ChkIsEduKind4.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            ChkIsEduKind5.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            ChkIsEduKind6.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            ChkIsEduKind7.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox1.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox2.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox3.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox4.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox5.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox6.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox7.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox8.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox9.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });

            checkBox10.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox11.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox12.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox13.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox15.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox16.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox17.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox18.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });

            checkBox19.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox20.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox21.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox22.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox23.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox24.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox25.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });
            checkBox26.SetOptions(new CheckBoxOption { CheckValue = "Y", UnCheckValue = "N" });

            SSReportList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSReportList.AddColumnText("ID", nameof(VisitDateModel.ID), 59, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSReportList.AddColumnText("방문일자", nameof(VisitDateModel.VisitDate), 78, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSReportList.AddColumnText("작성자", nameof(VisitDateModel.Name), 55, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

            siteStatusControl.Set_btnLocation();
            WindowState = FormWindowState.Maximized;
        }

        public void SetSite()
        {
            ContentTitle.TitleText = "보건관리상태보고서 의사 - " + base.SelectedSite.NAME;

            Clear();

            siteStatusControl.Initialize(this, DtpVisitDate.GetValue());

            siteStatusControl.SetTxtOshaData(textBox12);
            siteStatusControl.SetSitName(base.SelectedSite.NAME);
            workerHealthCheckForm.Init();

            if (TabReport.SelectedIndex == 2)
            {
                workerHealthCheckForm.Select(base.SelectedSite);
            }

            SearchReport();

            //보건담당자명 자동 표시
            if (TxtSITEMANAGERNAME.Text.Trim() == "")
            {
                TxtSITEMANAGERGRADE.Text = "보건담당자";
                TxtSITEMANAGERNAME.Text = GetLtdBogen(base.SelectedSite.ID);
            }
            //의사 이름 자동 표시
            if (TxtDoctorNAME.Text.Trim() == "") TxtDoctorNAME.Text = clsType.User.JobName;

        }

        //회사 보건관리자 이름 찾기
        private string GetLtdBogen(long Site_ID)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strName = "";
            string strNow = DateTime.Now.ToString("yyyy-MM-dd");

            SQL = "";
            SQL = "SELECT NAME FROM HIC_OSHA_CONTRACT_MANAGER ";
            SQL = SQL + ComNum.VBLF + "WHERE ESTIMATE_ID IN (SELECT ESTIMATE_ID FROM HIC_OSHA_CONTRACT ";
            SQL = SQL + ComNum.VBLF + "      WHERE OSHA_SITE_ID=" + Site_ID + " ";
            SQL = SQL + ComNum.VBLF + "        AND CONTRACTSTARTDATE<='" + strNow + "' ";
            SQL = SQL + ComNum.VBLF + "        AND CONTRACTENDDATE>='" + strNow + "' ";
            SQL = SQL + ComNum.VBLF + "        AND ISDELETED='N') ";
            SQL = SQL + ComNum.VBLF + "  AND WORKER_ROLE='HEALTH_ROLE' ";
            SQL = SQL + ComNum.VBLF + "  AND ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            strName = "";
            if (dt.Rows.Count > 0) strName = dt.Rows[0]["NAME"].ToString().Trim();
            dt.Dispose();
            dt = null;

            return strName;
        }

        private void SearchReport()
        {
            List<VisitDateModel> list = statusReportDoctorService.StatusReportDoctorRepository.FindAll(base.SelectedSite.ID);
            SSReportList.SetDataSource(list);
            if (SSReportList.RowCount() > 0)
            {
                SSReportList_CellDoubleClick(SSReportList, new CellClickEventArgs(null, 0, 0, 0, 0, MouseButtons.Left, false, false, false));
            }
        }

        public void SetMemo(string strMemo)
        {
            TxtMemo.Text = strMemo;
        }

        public void Clear()
        {
            DtpVisitDate.ValueChanged -= DtpVisitDate_ValueChanged;
            PanStatausReportDoctor.SetData(new StatusReportDoctorDto());
            siteStatusControl.Initialize(this, DtpVisitDate.GetValue());
            GrpPerformContent.Initialize();
            dateTimePicker1.SetValue(null);
            //TxtOPINION.Text = "";

            TxtDoctorNAME.Text = clsType.User.UserName;// CommonService.Instance.Session.UserName;

            workerHealthCheckForm.Init();
            workerHealthCheckForm.StatusReportDoctorDto = null;
            workerHealthCheckForm.StatusReportNurseDto = null;

            if (SelectedSite != null)
            {
                ContentTitle.TitleText = "보건관리상태보고서 의사 - " + base.SelectedSite.NAME;

                HC_SITE_WORKER worker = hcSiteWorkerService.FindHealthRole(SelectedSite.ID);
                if (worker != null)
                {
                    TxtSITEMANAGERGRADE.Text = "보건담당자";
                    TxtSITEMANAGERNAME.Text = worker.NAME;
                }
                siteStatusControl.SetSitName(SelectedSite.NAME);
            }

            DtpVisitDate.ValueChanged += DtpVisitDate_ValueChanged;
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            StatusReportDoctorDto dto = PanStatausReportDoctor.GetData<StatusReportDoctorDto>();
            if (dto.ID > 0)
            {
                dto = this.statusReportDoctorService.StatusReportDoctorRepository.FindOne(dto.ID);
                if (statusReportViewer != null)
                {
                    if (!statusReportViewer.IsDisposed)
                    {
                        statusReportViewer.Dispose();
                    }
                }
                statusReportViewer = new StatusReportViewer("statusReportByDoctor.html", base.SelectedSite.ID);
                statusReportViewer.PrintStatusReportDoctorDto(dto, ContentTitle.TitleText);
                statusReportViewer.ShowDialog();
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            DB_Save();
        }

        private void DB_Save()
        {
            if (base.SelectedSite == null)
            {
                MessageUtil.Alert("사업장을 선택하세요");
            }
            else
            {
                if (base.SelectedEstimate == null)
                {
                    MessageUtil.Alert("계약 내용이 없습니다");
                    return;
                }
                StatusReportDoctorDto dto = PanStatausReportDoctor.GetData<StatusReportDoctorDto>();

                if (PanStatausReportDoctor.Validate<StatusReportDoctorDto>())
                {
                    dto.SITE_ID = base.SelectedSite.ID;
                    //dto.ESTIMATE_ID = hcOshaContractRepository.FindByDate(dto.VISITDATE);
                    string visitdate = dto.VISITDATE.Substring(0, 4) + "-" + dto.VISITDATE.Substring(4, 2) + "-" + dto.VISITDATE.Substring(6, 2);
                    HC_OSHA_CONTRACT contract = hcOshaContractRepository.FindByDate(dto.SITE_ID, visitdate);
                    if (contract == null)
                    {
                        MessageUtil.Alert("방문일자가 계약일자에 해당되지 않습니다.");
                        return;
                    }
                    dto.ESTIMATE_ID = contract.ESTIMATE_ID;

                    SiteStatusDto siteStatusDto = siteStatusControl.GetData();

                    dto.SiteStatusDto = siteStatusDto;
                    // dto.OPINION = TxtOPINION.Text;
                    DoctorPerformContentJson performContentJson = panel2.GetData<DoctorPerformContentJson>();
                    String xx = performContentJson.bogunRadio;


                    //업무수행내용
                    string json = JsonConvert.SerializeObject(performContentJson);
                    dto.PERFORMCONTENT = json;

                    if (dto.ID > 0)
                    {
                        if (clsType.User.IdNumber != "170301" && clsType.User.IdNumber != "1")
                        {
                            if (statusReportDoctorService.IsGranted(dto.ID) == false)
                            {
                                MessageUtil.Alert("수정 권한이 없습니다");
                                return;
                            }
                        }
                    }

                    StatusReportDoctorDto saved = this.statusReportDoctorService.Save(dto);

                    try
                    {
                        HIC_OSHA_MEMO memo = this.statusReportMemoRepository.FindOne(dto.SITE_ID);
                        if (memo == null)
                        {
                            memo = new HIC_OSHA_MEMO();
                            memo.SITEID = dto.SITE_ID;
                            memo.MEMO = TxtMemo.Text;
                            this.statusReportMemoRepository.Insert(memo);
                        }
                        else
                        {
                            memo.MEMO = TxtMemo.Text;
                            //if (!memo.MEMO.IsNullOrEmpty())
                            {
                                this.statusReportMemoRepository.Update(memo);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }


                    SearchReport();

                    MessageUtil.Info("저장하였습니다");
                    for (int i = 0; i < SSReportList.ActiveSheet.RowCount; i++)
                    {
                        long id = SSReportList.ActiveSheet.Cells[i, 0].Value.To<long>(0);
                        if (id == saved.ID)
                        {
                            SSReportList_CellDoubleClick(SSReportList, new CellClickEventArgs(null, i, 0, 0, 0, MouseButtons.Left, false, false, false));
                        }

                    }
                }
            }
        }

        private void CboVisitYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SetCboVisitDate();
            //if (CboVisitDate.Items.Count > 0)
            //{
            //    CboVisitDate.SelectedIndex = 0;
            //}
        }

        private void CboVisitDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            //          long id = Convert.ToInt64(CboVisitDate.GetValue().ToString());

            //          //DtpVisitDate.SetValue("20190101");
            ////          Clear();

            //          StatusReportDoctorDto dto = this.statusReportDoctorService.StatusReportDoctorRepository.FindOne(id);
            //          if (dto != null)
            //          {
            //              SetData(dto);
            //          }
        }
        private void SetData(StatusReportDoctorDto dto)
        {
            PanStatausReportDoctor.SetData(dto);
            //  TxtOPINION.Text = dto.OPINION;
            if (dto.SiteStatusDto != null)
            {
                siteStatusControl.SetData(dto.SiteStatusDto);
            }

            if (dto.PERFORMCONTENT.NotEmpty())
            {
                DoctorPerformContentJson jsonDto = JsonConvert.DeserializeObject<DoctorPerformContentJson>(dto.PERFORMCONTENT);
                String xxx = jsonDto.noiseC1;

                panel2.SetData(jsonDto);
            }

            string month = dto.VISITDATE.Substring(4, 2);
            if (month.StartsWith("0"))
            {
                month = month.Substring(1, 1);
            }

            ContentTitle.TitleText = "보건관리상태보고서 의사 - " + base.SelectedSite.NAME + " " + month + "월 상태보고서";

            //GrpPerformContent.Initialize();

            workerHealthCheckForm.StatusReportDoctorDto = dto;
            doctorOpinionForm.SetDto(dto, base.SelectedSite, base.SelectedEstimate);

            HIC_OSHA_MEMO memo = statusReportMemoRepository.FindOne(dto.SITE_ID);
            if (memo != null)
            {
                TxtMemo.Text = memo.MEMO;
                doctorOpinionForm.SetMemo(memo.MEMO);
            }
            else
            {
                TxtMemo.Text = "";
                doctorOpinionForm.SetMemo("");
            }
        }

        private void DtpVisitDate_ValueChanged(object sender, EventArgs e)
        {
            if (base.SelectedSite != null)
            {
                string dtpVisitDate = DtpVisitDate.GetValue();
                string visitdate = dtpVisitDate.Substring(0, 4) + "-" + dtpVisitDate.Substring(4, 2) + "-" + dtpVisitDate.Substring(6, 2);

                HcEstimateModelRepository repo = new HcEstimateModelRepository();
                HC_OSHA_CONTRACT contract = hcOshaContractRepository.FindByDate(base.SelectedSite.ID, visitdate);
                if (contract == null)
                {
                    MessageUtil.Alert("방문일자가 계약일자에 해당되지 않습니다.");
                    this.SelectedEstimate = null;
                    return;
                }
                this.SelectedEstimate = repo.FindByEstimateId(contract.ESTIMATE_ID);

                //    HcEstimateModelRepository repo = new HcEstimateModelRepository();

                //    HC_OSHA_CONTRACT con = hcOshaContractRepository.FindByMaxContract(base.SelectedSite.ID);
                //    if (con != null)
                //    {
                //        this.SelectedEstimate = repo.FindByEstimateId(con.ESTIMATE_ID);
                //    }
                //    else
                //    {

                //    }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Select(ISiteModel siteModel)
        {
            if (siteModel.ID > 0)
            {
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            StatusReportDoctorDto dto = PanStatausReportDoctor.GetData<StatusReportDoctorDto>();
            if (dto.ID > 0)
            {
                bool isDeleted = false;
                HealthCheckService healthCheckService = new HealthCheckService();
                List<HealthCheckDto> list = healthCheckService.healthCheckRepository.FindAll(dto.ID);
                if (list.Count > 0)
                {
                    if (MessageUtil.Confirm(dto.ID + "번 상태보고서를 삭제 하시겠습니까? 근로자 상담이 작성되어 있습니다.(상담내용을 확인하세요) ") == DialogResult.Yes)
                    {
                        isDeleted = true;
                    }
                }
                else
                {
                    if (MessageUtil.Confirm(dto.ID + "번 상태보고서를 삭제 하시겠습니까?") == DialogResult.Yes)
                    {
                        isDeleted = true;
                    }
                }

                if (isDeleted)
                {
                    statusReportDoctorService.StatusReportDoctorRepository.Delete(dto.ID);

                    Clear();

                    SearchReport();
                }

            }
        }

        private void BtnWorker_Click(object sender, EventArgs e)
        {
            SiteWorkerPopupForm form = new SiteWorkerPopupForm();
            form.SelectedSite = base.SelectedSite;
            form.ShowDialog();

            foreach (HC_SITE_WORKER worker in form.GetWorker())
            {
                TxtSITEMANAGERGRADE.Text = codeService.FindActiveCodeByGroupAndCode("WORKER_ROLE", worker.WORKER_ROLE, "OSHA").CodeName;//보건관리자
                TxtSITEMANAGERNAME.Text = worker.NAME;
            }
        }

        private void TabReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabReport.SelectedIndex == 2)
            {
                workerHealthCheckForm.SelectedSite = base.SelectedSite;
                workerHealthCheckForm.SetDept();
                workerHealthCheckForm.SetPanjeong();
            }
        }

        private void BtnLast_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null)
            {
                return;
            }
            StatusReportDoctorDto dto = statusReportDoctorService.StatusReportDoctorRepository.FindLast(base.SelectedSite.ID);
            if (dto != null)
            {
                dto.ID = 0;
                dto.VISITDATE = DateTime.Now.ToString("yyyyMMdd");
                //CboVisitYear.Text = "";
                //CboVisitDate.Text = "";
                SetData(dto);
            }
        }

        private void BtnGetCount_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strVisitDate = DtpVisitDate.GetValue();
            long nNurseId = 0;

            StatusReportDoctorDto dto = PanStatausReportDoctor.GetData<StatusReportDoctorDto>();
            if (dto.ID == 0) return;

            //상담건수, 혈압, 혈당 등 검사 건수
            SQL = "SELECT COUNT(*) as TotCount,sum(decode(bpl,null, 0, 1)) as BpCount,sum(decode(bst, null, 0, 1)) AS BstCount, ";
            SQL = SQL + ComNum.VBLF + "   sum(decode(dan, null, 0, 1)) AS DanCount,sum(decode(BMI, null, 0, 1)) as BMICount, ";
            SQL = SQL + ComNum.VBLF + "   sum(decode(EXAM, null, 0, 1)) AS ExamCount ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_HEALTHCHECK ";
            SQL = SQL + ComNum.VBLF + "WHERE SITE_ID=" + base.SelectedSite.ID + " ";
            SQL = SQL + ComNum.VBLF + " AND REPORT_ID = " + dto.ID + " ";
            //if (nNurseId > 0) SQL = SQL + ComNum.VBLF + "  AND REPORT_ID IN (" + dto.ID + "," + nNurseId + ") ";
            SQL = SQL + ComNum.VBLF + "  AND ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense='" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["TotCount"].ToString().Trim() != "") numericUpDown1.SetValue(long.Parse(dt.Rows[0]["TotCount"].ToString()));
                if (dt.Rows[0]["BpCount"].ToString().Trim() != "") numericUpDown22.SetValue(long.Parse(dt.Rows[0]["BpCount"].ToString() + ""));
                if (dt.Rows[0]["BstCount"].ToString().Trim() != "") numericUpDown23.SetValue(long.Parse(dt.Rows[0]["BstCount"].ToString() + ""));
                if (dt.Rows[0]["DanCount"].ToString().Trim() != "") numericUpDown24.SetValue(long.Parse(dt.Rows[0]["DanCount"].ToString() + ""));
                if (dt.Rows[0]["BMICount"].ToString().Trim() != "") numericUpDown25.SetValue(long.Parse(dt.Rows[0]["BMICount"].ToString() + ""));
                if (dt.Rows[0]["ExamCount"].ToString().Trim() != "") numericUpDown30.SetValue(long.Parse(dt.Rows[0]["ExamCount"].ToString() + ""));
            }
            dt.Dispose();
            dt = null;

            //간이검사 건수
            SQL = "SELECT COUNT(*) as TotCount ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_HEALTHCHECK ";
            SQL = SQL + ComNum.VBLF + "WHERE SITE_ID=" + base.SelectedSite.ID + " ";
            SQL = SQL + ComNum.VBLF + "  AND REPORT_ID = " + dto.ID + " ";
            //if (nNurseId > 0) SQL = SQL + ComNum.VBLF + "  AND REPORT_ID IN (" + dto.ID + "," + nNurseId + ") ";
            SQL = SQL + ComNum.VBLF + "  AND ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND (bpl IS NOT NULL OR bst IS NOT NULL OR dan IS NOT NULL OR BMI IS NOT NULL) ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense='" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["TotCount"].ToString().Trim() != "") numericUpDown21.SetValue(long.Parse(dt.Rows[0]["TotCount"].ToString()));
            }
            dt.Dispose();
            dt = null;

            DB_Save(); //자동저장
        }

        private void oshaSiteList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            base.SelectedSite = oshaSiteList1.GetSite;
            DtpVisitDate_ValueChanged(DtpVisitDate, null);
            SetSite();
        }

        private void SSReportList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            VisitDateModel model = SSReportList.GetRowData(e.Row) as VisitDateModel;
            StatusReportDoctorDto dto = this.statusReportDoctorService.StatusReportDoctorRepository.FindOne(model.ID);
            if (dto != null)
            {
                SetData(dto);
            }
            else
            {
                SetData(new StatusReportDoctorDto());
            }
        }

        private void BtnOpinion_Click(object sender, EventArgs e)
        {
            StatusReportDoctorDto dto = PanStatausReportDoctor.GetData<StatusReportDoctorDto>();
            if (dto.ID > 0)
            {
                DoctorOpinionForm form = new DoctorOpinionForm();
                form.SetDto(dto, base.SelectedSite, base.SelectedEstimate);
                //form.MemoChanged += new EventHandler(memoChanged);
                form.ShowDialog();

                StatusReportDoctorDto saved = this.statusReportDoctorService.StatusReportDoctorRepository.FindOne(dto.ID);
                SetData(saved);
            }
        }

        private void memoChanged(string memo)
        {
            TxtMemo.Text += memo;
        }

        private void memoChanged()
        {
            TxtMemo.Text += "변경됨";
        }

        private void BtnPopup1_Click(object sender, EventArgs e)
        {
            frmHcPanGenMedExamResult_New form = new frmHcPanGenMedExamResult_New();

            form.Show();

            if (base.SelectedSite != null)
            {
                form.SetSite(SelectedSite.ID + "." + SelectedSite.NAME);
            }
        }

        private void BtnInfo_Click(object sender, EventArgs e)
        {
            InformationForm informationForm = new InformationForm(true);
            informationForm.SelectedSite = base.SelectedSite;
            informationForm.OnSelected += (item) =>
            {
                if (item != null)
                {
                    textBox12.Text = item.REMARK;
                }
            };

            informationForm.Width = 900;
            informationForm.Show(this);
        }

        private void oshaSiteList1_Load(object sender, EventArgs e)
        {

        }

        //간호사가 입력한 사업장현황을 가져오기
        private void BtnSaup_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strVisitDate = DtpVisitDate.GetValue();
            long nNurseId = 0;

            StatusReportDoctorDto dto = PanStatausReportDoctor.GetData<StatusReportDoctorDto>();
            if (dto.ID == 0) return;

            //의사,간호사 REPORT_ID를 찾음
            SQL = "SELECT ID FROM HIC_OSHA_REPORT_NURSE ";
            SQL = SQL + ComNum.VBLF + "WHERE SITE_ID=" + base.SelectedSite.ID + " ";
            SQL = SQL + ComNum.VBLF + "  AND VISITDATE='" + strVisitDate + "' ";
            SQL = SQL + ComNum.VBLF + "  AND ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense='" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY ID DESC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0) nNurseId = long.Parse(dt.Rows[0]["ID"].ToString());
            dt.Dispose();
            dt = null;
            if (nNurseId == 0) return;

            //간호사가 입력한 사업장현황을 읽어 표시함
            SiteStatusDto siteStatusDto = statusReportDoctorService.StatusReportDoctorRepository.FindOneNurse(nNurseId);

            if (siteStatusDto != null)
            {
                siteStatusControl.SetData(siteStatusDto);
                DB_Save(); //자동저장
            }
        }

        private void btnBogen_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = "";
            DataTable dt = new DataTable();

            dateTimePicker1.SetValue(null);
            if (base.SelectedSite.ID == 0) return;

            string strYear = DateUtil.DateTimeToStrig(DtpVisitDate.Value, DateTimeType.YYYY);

            SQL = "SELECT * FROM HIC_OSHA_CARD7_1 ";
            SQL += ComNum.VBLF + "WHERE SITE_ID = " + base.SelectedSite.ID + " ";
            SQL += ComNum.VBLF + "  AND SWLICENSE = '" + clsType.HosInfo.SwLicense + "' ";
            SQL += ComNum.VBLF + "  AND YEAR = '" + strYear + "' ";
            SQL += ComNum.VBLF + "ORDER BY MEETDATE DESC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                dateTimePicker1.Value = DateUtil.stringToDateTime(dt.Rows[0]["MEETDATE"].ToString(), DateTimeType.YYYY_MM_DD);
            }
            dt.Dispose();
            dt = null;
        }

        private void btnDB재접속_Click(object sender, EventArgs e)
        {
            clsDB.DisDBConnect(clsDB.DbCon);
            clsDB.DbCon = clsDB.DBConnect_Cloud();
            if (clsDB.DbCon == null)
            {
                ComFunc.MsgBox("DB 재접속 실패", "알림");
            }
            else
            {
                ComFunc.MsgBox("DB 재접속 완료", "알림");
            }
        }

        private void btnMemoRead_Click(object sender, EventArgs e)
        {
            HIC_OSHA_MEMO memo = statusReportMemoRepository.FindOne(base.SelectedSite.ID);
            if (memo != null)
            {
                TxtMemo.Text = memo.MEMO;
                doctorOpinionForm.SetMemo(memo.MEMO);
            }
            else
            {
                TxtMemo.Text = "";
                doctorOpinionForm.SetMemo("");
            }

        }
    }
}

