using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase.Controls;
using HC.OSHA.Dto;
using HC_Core;
using HC.OSHA.Service;
using ComHpcLibB.Model;
using ComBase.Mvc.Utils;
using HC.OSHA.Repository;
using ComBase;

namespace HC_OSHA.StatusReport
{
    public partial class SiteStatusControl : UserControl
    {
        AutoCompleteMacro autoCompleteMacro;
        public string VisitDate { get; set; }
        private CommonForm commonForm;
        private bool IsDoctorStatusReport;
        private HicOshaGeneralResultRepository hicOshaGeneralResultRepository;
        private HicOshaSpecialResultRepository hicOshaSpecialResultRepository;

        //정보자료보급
        private TextBox textbox;
        [Category("A-MTS-Framework-Properties")]
        [Description("의사용")]
        public bool IsDoctor
        {
            get { return IsDoctorStatusReport; }
            set
            {
                IsDoctorStatusReport = value;
                if (value)
                {
                    label27.Visible = false;
                    RdoWEMExporsure1.Visible = false;
                    RdoWEMExporsure2.Visible = false;
                    TxtWEMExporsureRemark.Visible = false;
                }
                else
                {
                    label27.Visible = true;
                    RdoWEMExporsure1.Visible = true;
                    RdoWEMExporsure2.Visible = true;
                    TxtWEMExporsureRemark.Visible = true;
                }

            }
        }
        public SiteStatusControl()
        {
            InitializeComponent();
            hicOshaGeneralResultRepository = new HicOshaGeneralResultRepository();
            hicOshaSpecialResultRepository = new HicOshaSpecialResultRepository();
        }

        private void SiteStatusControl_Load(object sender, EventArgs e)
        {
            NumCurrentWorkerCount.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.CURRENTWORKERCOUNT), });
            NumNewWorkerCount.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.NEWWORKERCOUNT), });
            NumRetireWorkerCount.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.RETIREWORKERCOUNT), });
            NumChangeWorkerCount.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.CHANGEWORKERCOUNT), });

            NumAccidentWorkerCount.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.ACCIDENTWORKERCOUNT), });
            NumDeadWorkerCount.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.DEADWORKERCOUNT), });
            NumInjuryWorkerCount.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.INJURYWORKERCOUNT), });
            NumBizDiseaseWorkerCount.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.BIZDISEASEWORKERCOUNT), });

            NumGeneralTotalCount.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.GENERALTOTALCOUNT), Min = 0 });
            NumSpecialTotalCount.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.SPECIALTOTALCOUNT), Min = 0 });

            DtpGeneralHealthCheckDate.SetOptions(new DateTimePickerOption { DataField = nameof(SiteStatusDto.GENERALHEALTHCHECKDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            NumGeneralD2Count.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.GENERALD2COUNT), Min = 0 });
            NumGeneralC2Count.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.GENERALC2COUNT), Min = 0 });

            DtpSpecialHealthCheckDate.SetOptions(new DateTimePickerOption { DataField = nameof(SiteStatusDto.SPECIALHEALTHCHECKDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            NumSpecialD1Count.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.SPECIALD1COUNT), Min =0  });
            NumSpecialC1Count.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.SPECIALC1COUNT), Min = 0 });
            NumSpecialD2Count.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.SPECIALD2COUNT), Min = 0 });
            NumSpecialC2Count.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.SPECIALC2COUNT), Min = 0 });
            NumSpecialDNCount.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.SPECIALDNCOUNT), Min = 0 });
            NumSpecialCNCount.SetOptions(new NumericUpDownOption { DataField = nameof(SiteStatusDto.SPECIALCNCOUNT), Min = 0 });

            DtpWEMDate.SetOptions(new DateTimePickerOption { DataField = nameof(SiteStatusDto.WEMDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            RdoWEMExporsure1.SetOptions(new CheckBoxOption { DataField = nameof(SiteStatusDto.WEMEXPORSURE), CheckValue = "Y", UnCheckValue = "N" });
            RdoWEMExporsure2.SetOptions(new CheckBoxOption { DataField = nameof(SiteStatusDto.WEMEXPORSURE2), CheckValue = "Y", UnCheckValue = "N" });
            TxtWEMExporsureRemark.SetOptions(new TextBoxOption { DataField = nameof(SiteStatusDto.WEMEXPORSUREREMARK) });

            TxtWEMHarmfulFactors.SetOptions(new TextBoxOption { DataField = nameof(SiteStatusDto.WEMHARMFULFACTORS) });
            TxtSiteName.SetOptions(new TextBoxOption { DataField = nameof(SiteStatusDto.SITENAME) });
            TxtExtData.SetOptions(new TextBoxOption { DataField = nameof(SiteStatusDto.EXTDATA) });

            TxtDeptName.SetOptions(new TextBoxOption { DataField = nameof(SiteStatusDto.DEPTNAME) });
            //autoCompleteMacro = new AutoCompleteMacro(this.Name);
            //autoCompleteMacro.Add(TxtWEMHarmfulFactors);
            autoCompleteMacro1.Add(TxtWEMHarmfulFactors);
            autoCompleteMacro1.Hide();
        }
        public void SetAcciendt(int d, int a, int d2)
        {
            NumDeadWorkerCount.SetValue(d);
            NumInjuryWorkerCount.SetValue(a);
            NumBizDiseaseWorkerCount.SetValue(d2);

            NumAccidentWorkerCount.SetValue(d + a + d2);
        }
        public void SetGeneralCount(DESEASE_COUNT_MODEL model)
        {
            NumGeneralC2Count.Value = model.C2;
            NumGeneralD2Count.Value = model.D2;
            DtpGeneralHealthCheckDate.SetValue(model.JEPDATE);
            NumGeneralTotalCount.Value = model.GeneralTotalCount;
        }

        public void SetSpecialCount(DESEASE_COUNT_MODEL model)
        {
            NumSpecialC1Count.Value =  model.C1;
            NumSpecialD1Count.Value = model.D1;

            NumSpecialC2Count.Value = model.C2;
            NumSpecialD2Count.Value = model.D2;

            NumSpecialDNCount.Value = model.DN;
            NumSpecialCNCount.Value = model.CN;

            DtpSpecialHealthCheckDate.SetValue(model.JEPDATE);
            NumSpecialTotalCount.Value = model.SpecialTotalCount;
        }
        public void SetSitName(string name)
        {
            TxtSiteName.Text = name;
        }
        public void SetTxtOshaData(TextBox textbox)
        {
            this.textbox = textbox;
        }
        public void SetInformation(string remark)
        {
            if (this.textbox != null)
            {
                this.textbox.Text = remark;
             //   MessageUtil.Info(" 적용하였습니다 ");
            }

            
        }
        public void SetInoutEmployee(int newWorkerCount, int retireWorkerCount)
        {
            NumNewWorkerCount.SetValue(newWorkerCount);
            NumRetireWorkerCount.SetValue(retireWorkerCount); ;
        }
        public void Initialize(CommonForm commonForm, string visitDate)
        {
            this.commonForm = commonForm;
            //       panSiteSatus.SetData(new SiteStatusDto());
            try
            {
                panSiteSatus.Initialize();
                VisitDate = visitDate;

                string startYear = DateTime.Now.AddYears(-5).ToString("yyyy");
                string endYear = DateTime.Now.ToString("yyyy");
                List<HIC_OSHA_GENEAL_RESULT> list = hicOshaGeneralResultRepository.FindAllNew(getCommonForm().SelectedSite.ID, startYear, endYear);
                if (list.Count > 0)
                {
                    DESEASE_COUNT_MODEL model = new DESEASE_COUNT_MODEL();
                    model.D2 = list[0].D2COUNT;
                    model.C2 = list[0].C2COUNT;
                    model.GeneralTotalCount = list[0].TOTALCOUNT;
                    model.JEPDATE = hicOshaGeneralResultRepository.FindByMinJepDate(getCommonForm().SelectedSite.ID, list[0].YEAR);
                    SetGeneralCount(model);
                }

                List<HIC_OSHA_SPECIAL_RESULT> list2 = hicOshaSpecialResultRepository.FindAllNew(getCommonForm().SelectedSite.ID, startYear, endYear);
                if (list2.Count > 0)
                {
                    DESEASE_COUNT_MODEL model = new DESEASE_COUNT_MODEL();
                    model.D2 = list2[0].D2COUNT;
                    model.C2 = list2[0].C2COUNT;
                    model.D1 = list2[0].D1COUNT;
                    model.C1 = list2[0].C1COUNT;
                    model.CN = list2[0].CNCOUNT;
                    model.DN = list2[0].DNCOUNT;
                    model.JEPDATE = hicOshaGeneralResultRepository.FindBySpecialMinJepDate(getCommonForm().SelectedSite.ID, list2[0].YEAR);
                    model.SpecialTotalCount = list2[0].TOTALCOUNT;
                    SetSpecialCount(model);
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex);
            }
          
        }

        public SiteStatusDto GetData()
        {
            return panSiteSatus.GetData<SiteStatusDto>();
        }
     
        public void SetData(SiteStatusDto dto)
        {
            panSiteSatus.SetData(dto);
        }
        private void BtnDataLink_Click(object sender, EventArgs e)
        {
            if (commonForm == null)
            {
                return;
            }
            if(getCommonForm().SelectedEstimate == null)
            {
                return;
            }
            long estimateId =  getCommonForm().SelectedEstimate.ID;
            if (this.commonForm != null)
            {
                OshaPriceService oshaPriceService = new OshaPriceService();
                OSHA_PRICE price = oshaPriceService.OshaPriceRepository.FindMaxIdByEstimate(estimateId);
                if (price != null)
                {
                    NumCurrentWorkerCount.SetValue( price.WORKERTOTALCOUNT);
                }

                StatisReportDataLinkForm frm = new StatisReportDataLinkForm();
                frm.SetStatisReportDataLinkForm(this);
                frm.Show();
            }
        }

        public CommonForm getCommonForm()
        {
            return this.commonForm; ;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            NumGeneralC2Count.SetValue(0);
            NumGeneralD2Count.SetValue(0);

            NumSpecialC1Count.SetValue(0);
            NumSpecialD1Count.SetValue(0);
            
            NumSpecialC2Count.SetValue(0);
            NumSpecialD2Count.SetValue(0);
            NumSpecialDNCount.SetValue(0);
            NumSpecialCNCount.SetValue(0);
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            long nSITE_ID = getCommonForm().SelectedSite.ID;
            string strVisitdate = VisitDate;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            bool bOK = false;

            int i = 0;
            int nRow = 0;
            string strDate1 = "";
            string strDate2 = "";
            string strDate3 = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT VISITRESERVEDATE,GENERALHEALTHCHECKDATE,SPECIALHEALTHCHECKDATE,WEMDATE ";
                SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_REPORT_NURSE ";
                SQL = SQL + ComNum.VBLF + " WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SITE_ID = " + nSITE_ID + " ";
                SQL = SQL + ComNum.VBLF + "   AND VISITDATE>='" + strVisitdate.Substring(0, 4) + "0101' ";
                SQL = SQL + ComNum.VBLF + "   AND VISITDATE<='" + strVisitdate.Substring(0, 4) + "1231' ";
                SQL = SQL + ComNum.VBLF + "   AND ISDELETED='N' ";
                SQL = SQL + ComNum.VBLF + "   AND (GENERALHEALTHCHECKDATE IS NOT NULL OR ";
                SQL = SQL + ComNum.VBLF + "        SPECIALHEALTHCHECKDATE IS NOT NULL OR ";
                SQL = SQL + ComNum.VBLF + "        WEMDATE IS NOT NULL) ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT VISITRESERVEDATE,GENERALHEALTHCHECKDATE,SPECIALHEALTHCHECKDATE,WEMDATE ";
                SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_REPORT_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SITE_ID = " + nSITE_ID + " ";
                SQL = SQL + ComNum.VBLF + "   AND VISITDATE>='" + strVisitdate.Substring(0, 4) + "0101' ";
                SQL = SQL + ComNum.VBLF + "   AND VISITDATE<='" + strVisitdate.Substring(0, 4) + "1231' ";
                SQL = SQL + ComNum.VBLF + "   AND ISDELETED='N' ";
                SQL = SQL + ComNum.VBLF + "   AND (GENERALHEALTHCHECKDATE IS NOT NULL OR ";
                SQL = SQL + ComNum.VBLF + "        SPECIALHEALTHCHECKDATE IS NOT NULL OR ";
                SQL = SQL + ComNum.VBLF + "        WEMDATE IS NOT NULL) ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT VISITRESERVEDATE,'' GENERALHEALTHCHECKDATE,'' SPECIALHEALTHCHECKDATE,WEMDATE ";
                SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_REPORT_ENGINEER ";
                SQL = SQL + ComNum.VBLF + " WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SITE_ID = " + nSITE_ID + " ";
                SQL = SQL + ComNum.VBLF + "   AND VISITDATE>='" + strVisitdate.Substring(0, 4) + "0101' ";
                SQL = SQL + ComNum.VBLF + "   AND VISITDATE<='" + strVisitdate.Substring(0, 4) + "1231' ";
                SQL = SQL + ComNum.VBLF + "   AND ISDELETED='N' ";
                SQL = SQL + ComNum.VBLF + "   AND  WEMDATE IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY VISITRESERVEDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                strDate1 = "";
                strDate2 = "";
                strDate3 = "";
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        //일반검진(예정)일
                        if (strDate1 == "")
                        {
                            if (dt.Rows[i]["GENERALHEALTHCHECKDATE"].ToString().Substring(0, 4) == strVisitdate.Substring(0, 4))
                            {
                                strDate1 = dt.Rows[i]["GENERALHEALTHCHECKDATE"].ToString().Trim();
                            }
                        }

                        //특수검진(예정)일
                        if (strDate2 == "")
                        {
                            if (dt.Rows[i]["SPECIALHEALTHCHECKDATE"].ToString().Substring(0, 4) == strVisitdate.Substring(0, 4))
                            {
                                strDate2 = dt.Rows[i]["SPECIALHEALTHCHECKDATE"].ToString().Trim();
                            }
                        }

                        //작업환경측정(예정)일
                        if (strDate3 == "")
                        {
                            if (dt.Rows[i]["WEMDATE"].ToString().Substring(0, 4) == strVisitdate.Substring(0, 4))
                            {
                                strDate3 = dt.Rows[i]["WEMDATE"].ToString().Trim();
                            }
                        }

                        if (strDate1 != "" && strDate2 != "" && strDate3 != "") break;
                    }
                }
                Cursor.Current = Cursors.Default;

                DtpGeneralHealthCheckDate.SetValue(null);
                DtpSpecialHealthCheckDate.SetValue(null);
                DtpWEMDate.SetValue(null);
                if (strDate1 != "") DtpGeneralHealthCheckDate.SetValue(strDate1);
                if (strDate2 != "") DtpSpecialHealthCheckDate.SetValue(strDate2);
                if (strDate3 != "") DtpWEMDate.SetValue(strDate3);

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
