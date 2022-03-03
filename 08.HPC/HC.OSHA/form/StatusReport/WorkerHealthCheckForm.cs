using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using ComDbB;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Model;
using HC.OSHA.Repository.StatusReport;
using HC.OSHA.Service;
using HC.OSHA.Service.StatusReport;
using HC_Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static HC.Core.Service.LogService;

namespace HC_OSHA.StatusReport
{
    /// <summary>
    /// 근로자 건강상담
    /// </summary>
    public partial class WorkerHealthCheckForm : CommonForm, ISelectSite
    {
        public bool IsDoctor { get; set; }

        public StatusReportNurseDto StatusReportNurseDto { get; set; }
        public StatusReportDoctorDto StatusReportDoctorDto { get; set; }
        private HcSiteWorkerService hcSiteWorkerService;
        private HealthCheckService healthCheckService;

        private HealthCareService healthCareService;

        private WorkerHealthCheckMacroService workerHealthCheckMacroService;
        private HealthCareReceiptService healthCareReceiptService;
        private HealthCheckWorkerModel SELECTED_WORKED;
        private clsHcMain clsHcMain;
        private HealthCheckMemoRepository healthCheckMemoRepository;
        private WorkerEndRepository workerEndRepository;
        public WorkerHealthCheckForm()
        {
            InitializeComponent();
            hcSiteWorkerService = new HcSiteWorkerService();
            healthCheckService = new HealthCheckService();
            workerHealthCheckMacroService = new WorkerHealthCheckMacroService();
            healthCareService = new HealthCareService();
            healthCheckMemoRepository = new HealthCheckMemoRepository();
            healthCareReceiptService = new HealthCareReceiptService();
            clsHcMain = new clsHcMain();

            workerEndRepository = new WorkerEndRepository();
        }

        private void WorkerHealthCheckForm_Load(object sender, EventArgs e)
        {
            txtSearchName.SetExecuteButton(btnSearch);
            panHealthCheck.SetEnterKey();

            rdoMale.SetOptions(new RadioButtonOption { DataField = nameof(HealthCheckDto.gender), CheckValue = "남", UnCheckValue = "여" });
            rdoFemale.SetOptions(new RadioButtonOption { DataField = nameof(HealthCheckDto.gender), CheckValue = "여", UnCheckValue = "남" });

            DtpWorkerEndDate.SetOptions(new DateTimePickerOption { DataField = "END_DATE", DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });

            SSWorkerList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSWorkerList.AddColumnText("등록번호", nameof(HealthCheckWorkerModel.Worker_ID), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, mergePolicy = MergePolicy.Always });
            SSWorkerList.AddColumnText("이름", nameof(HealthCheckWorkerModel.Name), 70, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true});
            SSWorkerList.AddColumnText("성별(연령)", nameof(HealthCheckWorkerModel.AgeAndGender), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, mergePolicy = MergePolicy.None });
            SSWorkerList.AddColumnText("부서", nameof(HealthCheckWorkerModel.Dept), 68, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, WordWrap=true, IsMulti=true });
            SSWorkerList.AddColumnText("건강구분", nameof(HealthCheckWorkerModel.Panjeong), 49, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, WordWrap = true });
            SSWorkerList.AddColumnText("검진년도", nameof(HealthCheckWorkerModel.Year), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("검진소견", nameof(HealthCheckWorkerModel.PanName), 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSWorkerList.AddColumnText("검진", nameof(HealthCheckWorkerModel.IsSpecial), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("퇴사일자", nameof(HealthCheckWorkerModel.END_DATE), 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });

            SSMemo.Initialize(new SpreadOption() { IsRowSelectColor = false, RowHeightAuto = true });
            SSMemo.AddColumnText("메모", nameof(HIC_OSHA_PATIENT_MEMO.MEMO), 347, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true });
            SSMemo.AddColumnText("작성일", nameof(HIC_OSHA_PATIENT_MEMO.WriteDate), 117, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true });
            SSMemo.AddColumnText("작성자", nameof(HIC_OSHA_PATIENT_MEMO.WriteName), 70, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true });
            SSMemo.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += WorkerHealthCheckForm_ButtonClick;

            SSHistory.Initialize(new SpreadOption() { IsRowSelectColor = false, RowHeightAuto = true, ColumnHeaderHeight = 20  });
            SSHistory.AddColumnText("상담일자", nameof(HealthCheckDto.WriteDateString), 117, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSHistory.AddColumnText("상담(지도) 내용", nameof(HealthCheckDto.content), 300, IsReadOnly.N , new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.General });
            SSHistory.AddColumnText("상담후 건의사항", nameof(HealthCheckDto.suggestion), 300, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.General });
            SSHistory.AddColumnText("혈압", nameof(HealthCheckDto.bp), 60, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false,  IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Center });
            SSHistory.AddColumnText("혈당", nameof(HealthCheckDto.bst), 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Center });
            SSHistory.AddColumnText("단백뇨", nameof(HealthCheckDto.dan), 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Center });
            SSHistory.AddColumnText("체중", nameof(HealthCheckDto.WEIGHT), 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Center });
            SSHistory.AddColumnText("체지방", nameof(HealthCheckDto.BMI), 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Center });
            SSHistory.AddColumnText("음주량", nameof(HealthCheckDto.ALCHOL), 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Center });
            SSHistory.AddColumnText("흡연량", nameof(HealthCheckDto.SMOKE), 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Center });
            SSHistory.AddColumnText("외래진료검사의뢰", nameof(HealthCheckDto.EXAM), 130, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Left });
            SSHistory.AddColumnText("상태보고서번호", nameof(HealthCheckDto.REPORT_ID), 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Left });
            SSHistory.AddColumnText("작성자", nameof(HealthCheckDto.MODIFIEDUSER), 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Center });

            SSHistory.SetDataSource(new List<HealthCheckDto>());

            ssMacroContentList.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight = 20 });
            ssMacroContentList.AddColumnText("상용구", nameof(WorkerHealthCheckMacrowordDto.TITLE), 354, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true });

            ssMacroSugesstionList.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight = 20 });
            ssMacroSugesstionList.AddColumnText("상용구", nameof(WorkerHealthCheckMacrowordDto.TITLE), 354, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true });

            SearchMacro();
            SearchMacro2();
            //ssMacroList.AddColumnText("상담(지도) 내용", nameof(WorkerHealthCheckMacrowordDto.CONTENT), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true });
            //ssMacroList.AddColumnText("상담후 건의사항", nameof(WorkerHealthCheckMacrowordDto.SUGESSTION), 150, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true });

            // Search();
            SSCard.ShowRow(0, 0, FarPoint.Win.Spread.VerticalPosition.Top);
        }

        // 자회사 정보를 읽음
        private string READ_Relation_LTD(long SiteID)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strLTD = "";

            strLTD = SiteID.ToString();

            try
            {
                SQL = "";
                SQL = "SELECT CHILD_ID FROM HIC_OSHA_RELATION ";
                SQL = SQL + ComNum.VBLF + " WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  PARENT_ID = " + strLTD + " ";
                SQL = SQL + ComNum.VBLF + "ORDER BY CHILD_ID ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strLTD = strLTD + "," + dt.Rows[i]["CHILD_ID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                return strLTD;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBox(ex.Message);
                return strLTD;
            }
        }

        // 마지막 연도의 판정 정보를 읽음
        private string Get_Last_Panjeng(string strID, string strJong)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strResult = "";

            try
            {
                SQL = "";
                SQL = "SELECT YEAR,GONGJENG,SEX,AGE,GGUBUN,SOGEN FROM HIC_LTD_RESULT3 ";
                SQL = SQL + ComNum.VBLF + " WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  ID = '" + strID + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  JONG = '" + strJong + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY YEAR DESC ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    strResult = strID + "{}" + dt.Rows[0]["YEAR"].ToString().Trim() + "{}"; //(1),(2)
                    strResult = strResult + dt.Rows[0]["GONGJENG"].ToString().Trim() + "{}"; //(3)
                    strResult = strResult + dt.Rows[0]["SEX"].ToString().Trim() + "{}"; //(4)
                    strResult = strResult + dt.Rows[0]["AGE"].ToString().Trim() + "{}"; //(5)
                    strResult = strResult + dt.Rows[0]["GGUBUN"].ToString().Trim() + "{}"; //(6)
                    strResult = strResult + dt.Rows[0]["SOGEN"].ToString().Trim() + "{}"; //(7)
                }

                dt.Dispose();
                dt = null;
                return strResult;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBox(ex.Message);
                return "";
            }
        }

        private void SearchMacro()
        {

            string macroType = "0";
            if (RdoMacroType0.Checked)
            {
                macroType = "0";
            }
            else if (RdoMacroType1.Checked)
            {
                macroType = "1";
            }
            else
            {
                macroType = "2";
            }
            List<WorkerHealthCheckMacrowordDto> list = workerHealthCheckMacroService.healthChecMacroRepository.FindAll(CommonService.Instance.Session.UserId, "1", macroType);
            ssMacroContentList.SetDataSource(list);


        }
        private void SearchMacro2()
        {

            string macroType2 = "0";
            if (RdoMacro2Type0.Checked)
            {
                macroType2 = "0";
            }
            else if (RdoMacro2Type1.Checked)
            {
                macroType2 = "1";
            }
            else
            {
                macroType2 = "2";
            }

            List<WorkerHealthCheckMacrowordDto> list2 = workerHealthCheckMacroService.healthChecMacroRepository.FindAll(CommonService.Instance.Session.UserId, "2", macroType2);
            ssMacroSugesstionList.SetDataSource(list2);
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            bool bOK = false;

            int i = 0;
            int nRow = 0;
            string strSiteList = "";
            string dept = "";
            string panjeong = "";
            long reportId = 0;

            string strID = "";
            string strName = "";
            string strDept = "";
            string strIpsaDate = "";
            string strTesaDate = "";
            string strPanjeng1 = "";
            string strPanjeng2 = "";
            string strPanjeng = "";

            if (CboDept.SelectedItem != null)
            {
                dept = CboDept.SelectedItem.ToString();
            }
            if (CboPanjeong.SelectedItem != null)
            {
                panjeong = CboPanjeong.SelectedItem.ToString();
            }

            if (ChkSangDam.Checked)
            {
                if (StatusReportNurseDto != null)
                {
                    reportId = StatusReportNurseDto.ID;
                }
                else if (StatusReportDoctorDto != null)
                {
                    reportId = StatusReportDoctorDto.ID;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            // 조회할 관계회사 목록을 생성
            strSiteList = READ_Relation_LTD(base.SelectedSite.ID);
            try
            {
                SQL = "SELECT ID,NAME,DEPT,WORKER_ROLE,IPSADATE,END_DATE,ISMANAGEOSHA ";
                SQL = SQL + ComNum.VBLF + " FROM HIC_SITE_WORKER ";
                SQL = SQL + ComNum.VBLF + " WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SITEID IN (" + strSiteList + ") ";
                SQL = SQL + ComNum.VBLF + "   AND ISDELETED='N' ";
                if (ChkEnd.Checked==false) SQL = SQL + ComNum.VBLF + " AND END_DATE IS NULL ";
                //이름검색
                if (txtSearchName.Text.Trim()!="") SQL = SQL + ComNum.VBLF + " AND NAME LIKE '%" + txtSearchName.Text.Trim() + "%' ";
                //중점관리대상자
                if (ChkSearchIsManageOsha.Checked == true) SQL = SQL + ComNum.VBLF + " AND ISMANAGEOSHA='Y' ";
                //부서별 조회
                if (dept != "전체") SQL = SQL + ComNum.VBLF + " AND DEPT='" + dept + "' ";
                //상담자 명단 조회
                if (reportId > 0)
                {
                    SQL = SQL + ComNum.VBLF + " AND ID IN (SELECT WORKER_ID FROM HIC_OSHA_HEALTHCHECK ";
                    SQL = SQL + ComNum.VBLF + "             WHERE REPORT_ID=" + reportId + " ";
                    SQL = SQL + ComNum.VBLF + "               AND SWLicense='" + clsType.HosInfo.SwLicense + "') ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY NAME ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    SSWorkerList_Sheet1.RowCount = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strID = dt.Rows[i]["ID"].ToString().Trim();
                        strName = dt.Rows[i]["NAME"].ToString().Trim();
                        strDept = dt.Rows[i]["DEPT"].ToString().Trim();
                        strIpsaDate = dt.Rows[i]["IPSADATE"].ToString().Trim();
                        strTesaDate = dt.Rows[i]["END_DATE"].ToString().Trim();
                        strPanjeng1 = Get_Last_Panjeng(strID, "특수");
                        strPanjeng2 = Get_Last_Panjeng(strID, "일반");
                        bOK = false;
                        if (panjeong != "")
                        {
                            if (panjeong == "A")
                            {
                                if (VB.Pstr(strPanjeng1, "{}", 6)==panjeong)
                                {
                                    strPanjeng = strPanjeng1;
                                    bOK = true;
                                }
                                else if (VB.Pstr(strPanjeng2, "{}", 6)==panjeong) 
                                {
                                    strPanjeng = strPanjeng2;
                                    bOK = true;
                                }
                            }
                            else if (VB.InStr(VB.Pstr(strPanjeng1, "{}", 6), panjeong) > 0)
                            {
                                strPanjeng = strPanjeng1;
                                bOK = true;
                            }
                            else if (VB.InStr(VB.Pstr(strPanjeng2, "{}", 6), panjeong) > 0)
                            {
                                strPanjeng = strPanjeng2;
                                bOK = true;
                            }
                        }
                        else
                        {
                            if (strPanjeng1 != "")
                            {
                                strPanjeng = strPanjeng1;
                                bOK = true;
                            }
                            else if (strPanjeng2 != "")
                            {
                                strPanjeng = strPanjeng2;
                                bOK = true;
                            }
                            else
                            {
                                strPanjeng = "";
                                bOK = true;
                            }
                        }

                        if (bOK == true)
                        {
                            SSWorkerList_Sheet1.Cells[nRow, 0].Text = strID;
                            SSWorkerList_Sheet1.Cells[nRow, 1].Text = strName;
                            SSWorkerList_Sheet1.Cells[nRow, 2].Text = VB.Pstr(strPanjeng, "{}", 4) + "(" + VB.Pstr(strPanjeng, "{}", 5) + ")"; //성별(연령)
                            SSWorkerList_Sheet1.Cells[nRow, 3].Text = VB.Pstr(strPanjeng, "{}", 3); //부서
                            SSWorkerList_Sheet1.Cells[nRow, 4].Text = VB.Pstr(strPanjeng, "{}", 6); //건강구분
                            SSWorkerList_Sheet1.Cells[nRow, 5].Text = VB.Pstr(strPanjeng, "{}", 2); //년도
                            SSWorkerList_Sheet1.Cells[nRow, 6].Text = VB.Pstr(strPanjeng, "{}", 7); //검진소견
                            SSWorkerList_Sheet1.Cells[nRow, 7].Text = dt.Rows[i]["ISMANAGEOSHA"].ToString().Trim();
                            SSWorkerList_Sheet1.Cells[nRow, 8].Text = strTesaDate;
                            if (dt.Rows[i]["ISMANAGEOSHA"].ToString().Trim() == "Y")
                            {
                                SSWorkerList.ActiveSheet.Rows[nRow].BackColor = Color.FromArgb(237, 211, 237);
                            }
                            else
                            {
                                SSWorkerList.ActiveSheet.Rows[nRow].BackColor = Color.White;
                            }
                            nRow++;
                        }
                    }
                }
                Cursor.Current = Cursors.Default;

                SSWorkerList_Sheet1.RowCount = nRow;
                LblCount.Text = "총: " + nRow + " 명";

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

        public void SetPanjeong()
        {
            CboPanjeong.Items.Clear();
            CboPanjeong.Items.Add("전체");
            CboPanjeong.Items.Add("A");
            CboPanjeong.Items.Add("U");
            CboPanjeong.Items.Add("AAA");
            CboPanjeong.Items.Add("U");
            CboPanjeong.Items.Add("C");
            CboPanjeong.Items.Add("C1");
            CboPanjeong.Items.Add("C2");
            CboPanjeong.Items.Add("D");
            CboPanjeong.Items.Add("D1");
            CboPanjeong.Items.Add("D2");
            CboPanjeong.Items.Add("CN");
            CboPanjeong.Items.Add("DN");
            CboPanjeong.Items.Add("확진검사대상");
        }
        public void Init()
        {
            Clear();
            CboDept.Items.Clear();
            txtSearchName.Text = "";
            SSWorkerList.ActiveSheet.RowCount = 0;
            SSHistory.ActiveSheet.RowCount = 0;

            panHealthCheck.Initialize();
            SSMemo.ActiveSheet.RowCount = 0;
            SSHealthCheck.ActiveSheet.RowCount = 0;

            BtnDeleteRemark.ForeColor = Color.Red;
            label3.TextAlign = ContentAlignment.MiddleLeft;
            label4.TextAlign = ContentAlignment.MiddleLeft;
            label17.TextAlign = ContentAlignment.MiddleLeft;
            label19.TextAlign = ContentAlignment.MiddleLeft;

            DtpWorkerEndDate.SetValue(null);
        }
        public void SetDept()
        {
            if (CboDept.Items.Count > 1)
            {
                return;
            }
            if (base.SelectedSite == null)
            {
                return;
            }
            List<HC_SITE_WORKER> list = hcSiteWorkerService.hcSiteWorkerRepository.FindAllGroupByDept(base.SelectedSite.ID);
            CboDept.Items.Clear();
            CboDept.Items.Add("전체");
            foreach (HC_SITE_WORKER worker in list)
            {
                if (worker.DEPT != null)
                {
                    if (!CboDept.Items.Contains(worker.DEPT))
                    {
                        CboDept.Items.Add(worker.DEPT);
                    }
                }
            }
            int iiii = CboDept.SelectedIndex;
            Object ob = CboDept.SelectedItem;
            if (CboDept.SelectedIndex < 0)
            {
                CboDept.SelectedIndex = 0;
            }
        }

        private void Search()
        {
            if (base.SelectedSite == null)
            {
                SSWorkerList.SetDataSource(new List<HealthCheckWorkerModel>());
            }
            else
            {
                try
                {
                    string dept = string.Empty;
                    string panjeong = string.Empty;
                    if (CboDept.SelectedItem != null)
                    {
                        dept = CboDept.SelectedItem.ToString();
                    }
                    if (CboPanjeong.SelectedItem != null)
                    {
                        panjeong = CboPanjeong.SelectedItem.ToString();
                    }
                    // CboPanjeong.SelectedItem.ToString()

                    Cursor.Current = Cursors.WaitCursor;
                    //     this.StatusReportNurseDto.ID
                    long reportId = 0;
                    if (ChkSangDam.Checked)
                    {
                        if (StatusReportNurseDto != null)
                        {
                            reportId = StatusReportNurseDto.ID;
                        }
                        else if (StatusReportDoctorDto != null)
                        {
                            reportId = StatusReportDoctorDto.ID;
                        }
                        
                    }
                    List<HealthCheckWorkerModel> list = healthCheckService.FindAll(base.SelectedSite.ID, txtSearchName.Text, dept, panjeong, ChkSearchIsManageOsha.Checked, reportId, ChkEnd.Checked);

                    //  판정구분 쿼리가 바껴서 따로 처리함
                    if (!panjeong.IsNullOrEmpty() && !panjeong.Equals("전체"))
                    {
                        list = list.Where(r => panjeong.Equals(r.Panjeong)).ToList();
                    }


                    SSWorkerList.SetDataSource(list);

                    var listGroup = from p in list
                                group p by p.Worker_ID;
                  

                    LblCount.Text = "총: " + listGroup.Count() +" 명";
                    for (int i = 0; i < SSWorkerList.ActiveSheet.RowCount; i++)
                    {
                        HealthCheckWorkerModel model = SSWorkerList.GetRowData(i) as HealthCheckWorkerModel;
                        if (model.IsManageOsha == "Y")
                        {
                            SSWorkerList.ActiveSheet.Rows[i].BackColor = Color.FromArgb(237, 211, 237);
                        }

                        if(model.REMARK.NotEmpty())
                        {
                            SSWorkerList.ActiveSheet.Rows[i].BackColor = Color.FromArgb(255, 255, 208);
                        }

                    }
                }
                catch(Exception ex)
                { 
                    Log.Error(ex);
                    MessageUtil.Alert(ex.Message);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
        
            }
        }

        private void ChangeRowColor(string workerId, bool isManageOsha)
        {
            for (int i = 0; i < SSWorkerList.ActiveSheet.RowCount; i++)
            {
                
                HealthCheckWorkerModel model = SSWorkerList.GetRowData(i) as HealthCheckWorkerModel;
                if (model.Worker_ID == workerId)
                {
                    if (isManageOsha)
                    {
                        model.IsManageOsha = "Y";
                    }
                    else
                    {
                        model.IsManageOsha = "N";
                    }

                    SSWorkerList.SetRowData(i, model);
                    if (isManageOsha)
                    {
                        SSWorkerList.ActiveSheet.Rows[i].BackColor = Color.FromArgb(237, 211, 237);
                    }
                    else
                    {
                        SSWorkerList.ActiveSheet.Rows[i].BackColor = Color.White;
                    }
                    
                }
            }
        }
        public void Select(ISiteModel siteModel)
        {
            SSWorkerList.SetDataSource(new List<HealthCheckWorkerModel>());
            base.SelectedSite = siteModel;
            SetDept();
            SetPanjeong();
            // Search();
        }
        /// <summary>
        /// 상담이력 조회
        /// </summary>
        /// <param name="worker_id"></param>
        public void SearchHistory(string worker_id)
        {
            SSHistory.ActiveSheet.RowCount = 0;
            List<HealthCheckDto> list = healthCheckService.healthCheckRepository.FindAll(worker_id);
            foreach (HealthCheckDto healthCheckDto in list)
            {
                //healthCheckDto.WriteDate = DateUtil.stringToDateTime(healthCheckDto.CHARTDATE + healthCheckDto.CHARTTIME, DateTimeType.YYYYMMDDHHMM);
                healthCheckDto.WriteDate = DateUtil.stringToDateTime(healthCheckDto.CHARTDATE, DateTimeType.YYYYMMDD);
                healthCheckDto.WriteDateString = DateUtil.DateTimeToStrig(healthCheckDto.WriteDate, DateTimeType.YYYY_MM_DD_HH_MM); // healthCheckDto.CHARTDATE + " " + healthCheckDto.CHARTTIME;
                if(healthCheckDto.bpl.IsNullOrEmpty() && healthCheckDto.bpr.IsNullOrEmpty())
                {
                    healthCheckDto.bp = string.Empty;
                }
                else
                {
                    healthCheckDto.bp = healthCheckDto.bpl + "/" + healthCheckDto.bpr;
                }
                
            }
            SSHistory.SetDataSource(list);

        }
     

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if(TxtContent.Text. IsNullOrEmpty() && txtSugesstion.Text.IsNullOrEmpty())
            //{
            //    MessageUtil.Alert(" 상담내뇽, 상담 후 건의사항 내용이 없습니다");
            //    return;
            //}
            if (base.SelectedSite == null)
            {
                return;
            }
            if (IsDoctor)
            {
                if (StatusReportDoctorDto == null)
                {
                    MessageUtil.Alert("상태보고서를 작성하거나 선택하셔야 합니다");
                    return;
                }
            }
            else
            {
                if (StatusReportNurseDto == null)
                {
                    MessageUtil.Alert("상태보고서를 작성하거나 선택하셔야 합니다");
                    return;
                }
            }

            HealthCheckDto dto = panHealthCheck.GetData<HealthCheckDto>();
            if (dto.worker_id.IsNullOrEmpty())
            {
                MessageUtil.Alert("근로자를 선택하세요");
                return;
            }
           
            // 상담내역은 본인만 수정 가능함
            if (dto.id > 0)
            {
                if (dto.ISDOCTOR == "Y" && StatusReportDoctorDto == null)
                {
                    MessageUtil.Alert("의사 상담내역은 의사만 수정이 가능합니다.");
                    return;
                }
                if (dto.ISDOCTOR == "N" && StatusReportNurseDto == null)
                {
                    MessageUtil.Alert("간호사 상담내역은 간호사만 수정이 가능합니다.");
                    return;
                }
            }

            if (panHealthCheck.Validate<HealthCheckDto>())
            {
                dto.site_id = base.SelectedSite.ID;
                
                dto.CHARTDATE = DateUtil.DateTimeToStrig(DtpChartDate.Value, DateTimeType.YYYYMMDD);
                if(StatusReportDoctorDto != null)
                {
                    dto.REPORT_ID = StatusReportDoctorDto.ID;
                    dto.ISDOCTOR = "Y";
                }
                if (StatusReportNurseDto != null)
                {
                    dto.REPORT_ID = StatusReportNurseDto.ID;
                    dto.ISDOCTOR = "N";
                }
                if (dto.REPORT_ID <= 0)
                {
                    MessageUtil.Alert("상태보고서를 작성하거나 선택하셔야 합니다");
                    return;
                }

                healthCheckService.Save(dto);

                //  퇴직자정보 저장
                HIC_OSHA_WORKER_END worker = new HIC_OSHA_WORKER_END();
                worker.SITE_ID = SELECTED_WORKED.SITEID;
                worker.ID = dto.worker_id;
                worker.WORKER_ID = dto.worker_id;
                worker.END_DATE = null;
                if (DtpWorkerEndDate.Checked) worker.END_DATE = DateUtil.DateTimeToStrig(DtpWorkerEndDate.Value, DateTimeType.YYYYMMDD);
                worker.CREATEDUSER = clsType.User.Sabun;

                workerEndRepository.Update(worker);

                //  퇴직자인경우 데이터를 저장하지 않는다.
                if(worker.END_DATE.NotEmpty())
                {
                    return;
                }

                if (TxtMemo.Text.NotEmpty())
                {
                    BtnSaveMemo.PerformClick();
                }

                if (TxtRemark.Text.NotEmpty())
                {
                    BtnSaveRemark.PerformClick();
                }

                Clear();

                LogService.Instance.Task(base.SelectedSite.ID, TaskName.HEALTHCHECK);

                SearchHistory(SELECTED_WORKED.Worker_ID);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public void Clear()
        {
            //  this.worker = null;
            labelMsg.Text = "";
            HealthCheckDto dto = panHealthCheck.GetData<HealthCheckDto>();
            if (dto != null)
            {
                panHealthCheck.Initialize();
                dto.id = 0;
                dto.content = "";
                dto.suggestion = "";
                dto.bp = "";
                dto.bpl = "";
                dto.bpr = "";
                dto.dan = "";
                dto.bst = "";
                SetHealthCheckData(dto);
            }
            else
            {
                panHealthCheck.Initialize();
            }

        }
        HealthCheckDto noChangeDto;
        private void SetHealthCheckData(HealthCheckDto dto)
        {
            noChangeDto = new HealthCheckDto();
            noChangeDto.content = dto.content;
            noChangeDto.suggestion = dto.suggestion;
            
            panHealthCheck.SetData(dto);
        }
        private void panHealthCheck_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Search();
        }

        private void SSHistory_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            HealthCheckDto dto = SSHistory.GetRowData(e.Row) as HealthCheckDto;

            SetHealthCheckData(dto);
            DtpChartDate.Value = DateUtil.stringToDateTime(dto.CHARTDATE, DateTimeType.YYYYMMDD);
            
            // 간호사가 작성한 근로자 상담내역을 의사가 클릭한 경우
            // 1.상담일이 오늘이고 의사가 작성한 오늘 상담내역이 없으면 저장 시 의사상담으로 신규 등록함
            if (dto.ISDOCTOR != "Y" && StatusReportDoctorDto != null)
            {
                if (dto.CHARTDATE== DateTime.Now.ToString("yyyyMMdd"))
                {
                    //의사 상담이 없으면
                    if (Doctor_Sangdam_Check(StatusReportDoctorDto.ID, dto.worker_id)==false)
                    {
                        dto.id = 0;
                        dto.REPORT_ID = StatusReportDoctorDto.ID;
                        dto.ISDOCTOR = "Y";
                        labelMsg.Text = "▶의사상담으로 복사됨";
                    }
                }
            }
        }

        // 오늘 의사상담이 있는지 점검
        private bool Doctor_Sangdam_Check(long reportId,string  workerId)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            bool bOK = false;

            SQL = "SELECT ID FROM HIC_OSHA_HEALTHCHECK ";
            SQL = SQL + ComNum.VBLF + "WHERE WORKER_ID='" + workerId + "' ";
            SQL = SQL + ComNum.VBLF + "  AND REPORT_ID=" + reportId + " ";
            SQL = SQL + ComNum.VBLF + "  AND ISDELETED = 'N' ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0) bOK = true;
            dt.Dispose();
            dt = null;

            return bOK;
        }

        private void btnMacro_Click(object sender, EventArgs e)
        {
            WorkerHealthCheckMacroForm form = new WorkerHealthCheckMacroForm();
            form.ShowDialog();

            SearchMacro();
            SearchMacro2();
            //List<WorkerHealthCheckMacrowordDto> list = workerHealthCheckMacroService.healthChecMacroRepository.FindAll(CommonService.Instance.Session.UserId);
            //ssMacroContentList.SetDataSource(list);
        }

        private void ssMacroList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            WorkerHealthCheckMacrowordDto dto = ssMacroContentList.GetRowData(e.Row) as WorkerHealthCheckMacrowordDto;
            if (dto.CONTENT != null)
            {
                if (ChkOverWrite.Checked)
                {
                    string content = "";
                    int selectionIndex = TxtContent.SelectionStart;
                    if (TxtContent.Text.Length > 0)
                    {
                        content += "\r\n" + dto.CONTENT;
                    }
                    else
                    {
                        content = dto.CONTENT;
                    }
                 
                    TxtContent.Text = TxtContent.Text.Insert(selectionIndex, content);
                    TxtContent.SelectionStart = selectionIndex + content.Length;
                }
                else
                {
                    TxtContent.Text = dto.CONTENT;
                }
            }
            else
            {
                MessageUtil.Alert("상용구 내용이 없습니다");
            }
        }

        private void SSWorkerList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            bool NotSaved = false;
            if (noChangeDto != null)
            {
                HealthCheckDto noSaved = panHealthCheck.GetData<HealthCheckDto>();
                if (noSaved.worker_id.NotEmpty())
                {
                    if (TxtContent.Text.NotEmpty())
                    {
                        if(noSaved.id== 0)
                        {
                            NotSaved = true;
                        }
                        if(noChangeDto.content.Length != TxtContent.Text.Length)
                        {
                            NotSaved = true;
                        }
                    }
                    if (txtSugesstion.Text.NotEmpty())
                    {
                        if (noSaved.id == 0)
                        {
                            NotSaved = true;
                        }
                        if (noChangeDto.suggestion.Length != txtSugesstion.Text.Length)
                        {
                            NotSaved = true;
                        }
                    }
                }
            }

            if (NotSaved)
            {
                if(MessageUtil.Confirm("상담(지도)내용 또는 상담 후 건의사항이 변경되었습니다 저장하겠습니까? ") == DialogResult.Yes){
                    btnSave.PerformClick();
                }
            }

            HealthCheckWorkerModel worker = new HealthCheckWorkerModel();
            worker.Worker_ID = SSWorkerList_Sheet1.Cells[e.Row, 0].Text.Trim();
            worker.Name = SSWorkerList_Sheet1.Cells[e.Row, 1].Text.Trim();
            worker.Gender = "";
            worker.Age = 0;
            if (VB.Pstr(SSWorkerList_Sheet1.Cells[e.Row, 2].Text.Trim(), "(", 1)!= "")
            {
                worker.Gender = VB.Pstr(SSWorkerList_Sheet1.Cells[e.Row, 2].Text.Trim(), "(", 1);
                worker.Age = Int32.Parse(VB.Pstr(VB.Pstr(SSWorkerList_Sheet1.Cells[e.Row, 2].Text.Trim(), "(", 2), ")", 1));
            }
            worker.Dept = SSWorkerList_Sheet1.Cells[e.Row, 3].Text.Trim();
            worker.SITEID = base.SelectedSite.ID;
            worker.END_DATE = SSWorkerList_Sheet1.Cells[e.Row, 8].Text.Trim();
            worker.IsManageOsha = SSWorkerList_Sheet1.Cells[e.Row, 7].Text.Trim();

            SELECTED_WORKED = worker;
            DtpWorkerEndDate.SetValue(null);

            HealthCheckDto dto = new HealthCheckDto();
            dto.name = worker.Name;
            dto.age = worker.Age;
            dto.dept = worker.Dept;
            dto.gender = worker.Gender;
            dto.site_id = base.SelectedSite.ID;
            dto.worker_id = worker.Worker_ID;
            dto.END_DATE = worker.END_DATE;
            
            SetHealthCheckData(dto);

            if (worker.IsManageOsha == "Y")
            {
                ChkIsManageOsha.Checked = true;
            }
            else
            {
                ChkIsManageOsha.Checked = false;
            }

            SearchHistory(worker.Worker_ID);
            SearchMemo(worker.Worker_ID);
            SearchRemark(worker.Worker_ID);

            SSHealthCheck.ActiveSheet.RowCount = 0;
            ClearSScard();

            //질병유소견자 
            SSHealthCheck_Show(worker.Worker_ID); 

            //List<HealthCareReciptModel> receiptList = healthCareReceiptService.FindHealthCareByPano(worker.Pano);
            //if (receiptList != null)
            //{
            //    if (receiptList.Count >= 1)
            //    {
            //        try
            //        {
            //            frmHcPanOpinionAfterMgmtGenSpc form = new frmHcPanOpinionAfterMgmtGenSpc();
            //            //근로자 건강상담관리(SSCard), 질병유소견자 대행(SSHealthCheck)
            //            form.SetSpread(SSCard, SSHealthCheck, receiptList[receiptList.Count - 1].JEPDATE, receiptList[0].JEPDATE, receiptList[0].SiteId.ToString(), "", worker.Pano);

            //        }
            //        catch (Exception ex)
            //        {
            //            Log.Error(ex);
            //            MessageUtil.Alert("질병유소견자(대행) 정보를 가져오는중 오류가 발생하였습니다");
            //        }

            //        try
            //        {
            //            SetPatient(receiptList[0]);
            //        }
            //        catch (Exception ex)
            //        {
            //            Log.Error(ex);
            //            MessageUtil.Alert("근로자 건강 상담 및 사후정보를 가져오는중 오류가 발생하였습니다");
            //        }

            //        try
            //        {
            //            List<WrtnoAndYear> wrtNoList = new List<WrtnoAndYear>();
            //            for (int i = 0; i < receiptList.Count; i++)
            //            {
            //                HealthCareReciptModel model = receiptList[i];
            //                if (model.UCodes.IsNullOrEmpty() == false)
            //                {
            //                    //일반특수 최대 3건의 wrtno 구하기
            //                    WrtnoAndYear year = new WrtnoAndYear();
            //                    year.WRTNO = model.WRTNO;
            //                    year.YEAR = model.JEPDATE.Substring(0, 4);
            //                    wrtNoList.Add(year);
            //                    if (wrtNoList.Count == 3)
            //                    {
            //                        break;
            //                    }
            //                }
            //            }
            //            if (wrtNoList.Count > 0)
            //            {
            //                int count = wrtNoList.Count;
            //                if (count > 3)
            //                {
            //                    count = 3;
            //                }
            //                //청력 오디오그램
            //                for (int row = 1; row <= count; row++)
            //                {

            //                    SetAudioGram(row, wrtNoList[row - 1]);
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Log.Error(ex);
            //            MessageUtil.Alert("청력 오디오그램 가져오는중 오류가 발생하였습니다");
            //        }
            //    }
            //}
        }

        //질병유소견자 
        private void SSHealthCheck_Show(string strWorkId)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;

            SSHealthCheck_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT GONGJENG,NAME,SEX,AGE,GUNSOK,YUHE,GGUBUN,SOGEN,SAHU,";
                SQL = SQL + ComNum.VBLF + " UPMU,YEAR,JINDATE,JIPYO  ";
                SQL = SQL + ComNum.VBLF + "  FROM HIC_LTD_RESULT3 ";
                SQL = SQL + ComNum.VBLF + " WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ID='" + strWorkId + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY YEAR DESC,JINDATE DESC,JONG ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    SSHealthCheck_Sheet1.RowCount = dt.Rows.Count;
                    SSHealthCheck_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SSHealthCheck_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GONGJENG"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GUNSOK"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 5].Text = dt.Rows[i]["YUHE"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 6].Text = dt.Rows[i]["JIPYO"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 7].Text = dt.Rows[i]["GGUBUN"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SOGEN"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 9].Text = dt.Rows[i]["SAHU"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 10].Text = dt.Rows[i]["UPMU"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 11].Text = dt.Rows[i]["YEAR"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 12].Text = VB.Right(dt.Rows[i]["JINDATE"].ToString().Trim(),5);
                        SSHealthCheck_Sheet1.Rows[i].Height = SSHealthCheck_Sheet1.Rows[i].GetPreferredHeight();
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        public void ClearSScard()
        {
            SSCard.ActiveSheet.Cells[6, 2].Value = "";
            SSCard.ActiveSheet.Cells[6, 6].Value = "";
            SSCard.ActiveSheet.Cells[6, 9].Value = "";
            SSCard.ActiveSheet.Cells[6, 10].Value = "";
            SSCard.ActiveSheet.Cells[6, 26].Value = "";
            SSCard.ActiveSheet.Cells[6, 35].Value = "";
            SSCard.ActiveSheet.Cells[9, 5].Value = "";
            SSCard.ActiveSheet.Cells[8, 3].Value = "";
            SSCard.ActiveSheet.Cells[8, 5].Value = "";
            SSCard.ActiveSheet.Cells[8, 10].Value = "";
            SSCard.ActiveSheet.Cells[8, 12].Value = "";
            SSCard.ActiveSheet.Cells[8, 14].Value = "";
            SSCard.ActiveSheet.Cells[8, 26].Value = "";
            SSCard.ActiveSheet.Cells[8, 35].Value = "";
            SSCard.ActiveSheet.Cells[8, 10].Value = "";
            SSCard.ActiveSheet.Cells[8, 12].Value = "";
            SSCard.ActiveSheet.Cells[8, 14].Value = "";
            SSCard.ActiveSheet.Cells[8, 26].Value = "";
            SSCard.ActiveSheet.Cells[8, 35].Value = "";

            //건강진단실시현황
            for (int i = 0; i < 6; i++)
            {
                SSCard.ActiveSheet.Cells[12 + i, 2].Value = ""; //년도
                SSCard.ActiveSheet.Cells[12 + i, 3].Value = "";
                SSCard.ActiveSheet.Cells[12 + i, 38].Value = "";
            }

            for (int index = 25; index <=31; index++)
            {
                SSCard.ActiveSheet.Cells[index, 3].Value = "";
                SSCard.ActiveSheet.Cells[index, 4].Value = "";
                SSCard.ActiveSheet.Cells[index, 5].Value = "";
                SSCard.ActiveSheet.Cells[index, 6].Value = "";
                SSCard.ActiveSheet.Cells[index, 7].Value = "";
                SSCard.ActiveSheet.Cells[index, 8].Value = "";
                SSCard.ActiveSheet.Cells[index, 9].Value = "";
            }
           
        }
        private void SetAudioGram(int row, WrtnoAndYear model)
        {
            ExResultModel result = healthCareService.GetExResult(model.WRTNO);
            if(row == 1)
            {
                int left_index = 25;
                //좌청력
                SSCard.ActiveSheet.Cells[left_index, 3].Value = model.YEAR;
                SSCard.ActiveSheet.Cells[left_index, 4].Value = result.TH11;
                SSCard.ActiveSheet.Cells[left_index, 5].Value = result.TH12;
                SSCard.ActiveSheet.Cells[left_index, 6].Value = result.TH13;
                SSCard.ActiveSheet.Cells[left_index, 7].Value = result.TH14;
                SSCard.ActiveSheet.Cells[left_index, 8].Value = result.TH15;
                SSCard.ActiveSheet.Cells[left_index, 9].Value = result.TH16;

                int right_index = 29;
                //좌청력
                SSCard.ActiveSheet.Cells[right_index, 3].Value = model.YEAR;
                SSCard.ActiveSheet.Cells[right_index, 4].Value = result.TH21;
                SSCard.ActiveSheet.Cells[right_index, 5].Value = result.TH22;
                SSCard.ActiveSheet.Cells[right_index, 6].Value = result.TH23;
                SSCard.ActiveSheet.Cells[right_index, 7].Value = result.TH24;
                SSCard.ActiveSheet.Cells[right_index, 8].Value = result.TH25;
                SSCard.ActiveSheet.Cells[right_index, 9].Value = result.TH26;
            }
            if (row == 2)
            {
                int left_index = 26;
                //좌청력
                SSCard.ActiveSheet.Cells[left_index, 3].Value = model.YEAR;
                SSCard.ActiveSheet.Cells[left_index, 4].Value = result.TH11;
                SSCard.ActiveSheet.Cells[left_index, 5].Value = result.TH12;
                SSCard.ActiveSheet.Cells[left_index, 6].Value = result.TH13;
                SSCard.ActiveSheet.Cells[left_index, 7].Value = result.TH14;
                SSCard.ActiveSheet.Cells[left_index, 8].Value = result.TH15;
                SSCard.ActiveSheet.Cells[left_index, 9].Value = result.TH16;

                int right_index = 30;
                //좌청력
                SSCard.ActiveSheet.Cells[right_index, 3].Value = model.YEAR;
                SSCard.ActiveSheet.Cells[right_index, 4].Value = result.TH21;
                SSCard.ActiveSheet.Cells[right_index, 5].Value = result.TH22;
                SSCard.ActiveSheet.Cells[right_index, 6].Value = result.TH23;
                SSCard.ActiveSheet.Cells[right_index, 7].Value = result.TH24;
                SSCard.ActiveSheet.Cells[right_index, 8].Value = result.TH25;
                SSCard.ActiveSheet.Cells[right_index, 9].Value = result.TH26;
            }
            if (row == 3)
            {
                int left_index = 27;
                //좌청력
                SSCard.ActiveSheet.Cells[left_index, 3].Value = model.YEAR;
                SSCard.ActiveSheet.Cells[left_index, 4].Value = result.TH11;
                SSCard.ActiveSheet.Cells[left_index, 5].Value = result.TH12;
                SSCard.ActiveSheet.Cells[left_index, 6].Value = result.TH13;
                SSCard.ActiveSheet.Cells[left_index, 7].Value = result.TH14;
                SSCard.ActiveSheet.Cells[left_index, 8].Value = result.TH15;
                SSCard.ActiveSheet.Cells[left_index, 9].Value = result.TH16;

                int right_index = 31;
                //좌청력
                SSCard.ActiveSheet.Cells[right_index, 3].Value = model.YEAR;
                SSCard.ActiveSheet.Cells[right_index, 4].Value = result.TH21;
                SSCard.ActiveSheet.Cells[right_index, 5].Value = result.TH22;
                SSCard.ActiveSheet.Cells[right_index, 6].Value = result.TH23;
                SSCard.ActiveSheet.Cells[right_index, 7].Value = result.TH24;
                SSCard.ActiveSheet.Cells[right_index, 8].Value = result.TH25;
                SSCard.ActiveSheet.Cells[right_index, 9].Value = result.TH26;
            }

        }
         
        private void SetPatient(HealthCareReciptModel model )
        {
            //     SSCard.ActiveSheet.Cells[12, 3].Value = "x2xxx";
            //마지막 접수정보로 인적사항
            HealthCareReciptModel patient = model;
            SSCard.ActiveSheet.Cells[6, 2].Value = SelectedSite.NAME;
            SSCard.ActiveSheet.Cells[6, 6].Value = patient.NAME;
            SSCard.ActiveSheet.Cells[6, 9].Value = patient.AGE;
            SSCard.ActiveSheet.Cells[6, 10].Value = patient.BuseName;
            SSCard.ActiveSheet.Cells[6, 26].Value = patient.IpsaDate;
            SSCard.ActiveSheet.Cells[6, 35].Value = patient.Tel;
            SSCard.ActiveSheet.Cells[9, 5].Value = clsHcMain.UCode_Names_Display(patient.UCodes);


            ExResultModel exResult = healthCareService.GetExResult(patient.WRTNO);
            SSCard.ActiveSheet.Cells[8, 3].Value = exResult.HEIGHT;
            SSCard.ActiveSheet.Cells[8, 5].Value = exResult.WEIGHT;
            // 민철샘 물어볼것
            //SSCard.ActiveSheet.Cells[8, 3].Value = clsHcMain.Biman_Gesan(patient.WRTNO);
            SSCard.ActiveSheet.Cells[8, 3].Value = exResult.BMI;

            HIC_RES_BOHUM1 hicResult = healthCareService.GetFirstExaminationQuestionnaire(patient.WRTNO);
            string gajok = "무";
            for (int i = 1; i <= 6; i++)
            {
                object obj = hicResult.GetPropertieValue("Gajok" + i);
                if (obj != null)
                {
                    if (obj.ToString() == "2")
                    {
                        gajok = "유";
                    }
                }

            }
            SSCard.ActiveSheet.Cells[8, 10].Value = gajok;

            //과거병력
            string OLDBYENG = "무";
            for (int i = 1; i <= 7; i++)
            {
                object obj = hicResult.GetPropertieValue("OLDBYENG" + i);
                if (obj != null)
                {
                    if (obj.ToString() == "2")
                    {
                        OLDBYENG = "유";
                    }
                }

            }
            SSCard.ActiveSheet.Cells[8, 12].Value = OLDBYENG;

            if (hicResult.JINCHAL2 == "1")
            {
                SSCard.ActiveSheet.Cells[8, 14].Value = "양호";
            }
            else if (hicResult.JINCHAL2 == "2")
            {
                SSCard.ActiveSheet.Cells[8, 14].Value = "보통";
            }
            else if (hicResult.JINCHAL2 == "3")
            {
                SSCard.ActiveSheet.Cells[8, 14].Value = "불량";
            }
            //흡연
            if (hicResult.SMOKING1 == "N")
            {
                SSCard.ActiveSheet.Cells[8, 26].Value = "무";
            }
            else
            {
                SSCard.ActiveSheet.Cells[8, 26].Value = "유";
            }
            //음주
            if (hicResult.DRINK2 == "N")
            {
                SSCard.ActiveSheet.Cells[8, 35].Value = "무";
            }
            else
            {
                SSCard.ActiveSheet.Cells[8, 35].Value = "유";
            }

            if (patient.UCodes.IsNullOrEmpty() == false)
            {
                //일반+특수
                HIC_RES_SPECIAL special = healthCareService.GetSpecialExaminationQuestionnaire(patient.WRTNO);
                //가족력
                if (special.MUN_GAJOK != "")
                {
                    SSCard.ActiveSheet.Cells[8, 10].Value = "유";
                }
                else
                {
                    SSCard.ActiveSheet.Cells[8, 10].Value = "무";
                }
                //과거병력
                OLDBYENG = "무";
                for (int i = 1; i <= 5; i++)
                {
                    object obj = special.GetPropertieValue("OLDMYEAR" + i);
                    if (obj != null)
                    {
                        if (obj.ToString() != "")
                        {
                            OLDBYENG = "유";
                        }
                    }

                }
                SSCard.ActiveSheet.Cells[8, 12].Value = OLDBYENG;

                //일반상태
                if (special.GBSANGTAE == "1")
                {
                    SSCard.ActiveSheet.Cells[8, 14].Value = "양호";
                }
                else if (special.GBSANGTAE == "2")
                {
                    SSCard.ActiveSheet.Cells[8, 14].Value = "보통";
                }
                else if (special.GBSANGTAE == "3")
                {
                    SSCard.ActiveSheet.Cells[8, 14].Value = "불량";
                }
                //흡연
                if (special.HABIT2 == "Y")
                {
                    SSCard.ActiveSheet.Cells[8, 26].Value = "유";
                }
                else
                {
                    SSCard.ActiveSheet.Cells[8, 26].Value = "무";
                }
                //음주
                if (special.HABIT1 == "Y")
                {
                    SSCard.ActiveSheet.Cells[8, 35].Value = "유";
                }
                else
                {
                    SSCard.ActiveSheet.Cells[8, 35].Value = "무";
                }
            }//특수검진
        }
        private void SSHealthCare_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
          //  HealthCareReciptModel model = SSHealthCare.GetRowData(e.Row) as HealthCareReciptModel;

          //  SetPatient(model);
        }

        private void SSWorkerList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(SelectedSite != null)
            {
                HealthCheckDto dto = panHealthCheck.GetData<HealthCheckDto>();

                // 상담내역은 본인만 삭제 가능함
                if (dto.id > 0)
                {
                    if (dto.ISDOCTOR == "Y" && StatusReportDoctorDto == null)
                    {
                        MessageUtil.Alert("의사 상담내역은 의사만 삭제가 가능합니다.");
                        return;
                    }
                    if (dto.ISDOCTOR == "N" && StatusReportNurseDto == null)
                    {
                        MessageUtil.Alert("간호사 상담내역은 간호사만 삭제가 가능합니다.");
                        return;
                    }
                }

                if (MessageUtil.Confirm("삭제 하시겠습니까?") == DialogResult.Yes)
                {

                    healthCheckService.healthCheckRepository.Delete(dto.id);

                    SearchHistory(SELECTED_WORKED.Worker_ID);

                    Clear();
                }
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null)
            {
                return;
            }

            if(this.StatusReportDoctorDto == null && this.StatusReportNurseDto == null)
            {
                return;
            }
          
            WorkerHealthCheckPrint form = new WorkerHealthCheckPrint();
            form.SelectedSite = SelectedSite;
            form.SetStatusReportNurseDto(StatusReportNurseDto, SelectedSite);
            form.SetStatusReportDoctorDto(StatusReportDoctorDto, SelectedSite);
            form.Show();
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void BtnBpCopy_Click(object sender, EventArgs e)
        {
            string text = "";
            if (txtbpr.Text.Length > 0)
            {
                text += "혈압: " + Txtbpl.Text + " / "+ txtbpr.Text + Environment.NewLine;
            }

            if (TxtBst.Text.Length > 0)
            {
                text +=  "혈당: " + TxtBst.Text + Environment.NewLine;

            }
            if (TxtDan.Text.Length > 0)
            {
                text +=  "단백뇨: " + TxtDan.Text + Environment.NewLine;

            }
            if (textBox4.Text.Length > 0)
            {
                text += "체지방: " + textBox4.Text + Environment.NewLine;
            }
            if (textBox3.Text.Length > 0)
            {
                text += "음주량: " + textBox3.Text + Environment.NewLine;
            }
            if (textBox2.Text.Length > 0)
            {
                text += "흡연량: " + textBox2.Text + Environment.NewLine;
            }

            text += Environment.NewLine;
            int selectionIndex = TxtContent.SelectionStart;
            TxtContent.Text = TxtContent.Text.Insert(selectionIndex, text);
            TxtContent.SelectionStart = selectionIndex + text.Length;


        }

        private void CboPanjeong_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Search();
        }

        private void ssMacroSugesstionList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            WorkerHealthCheckMacrowordDto dto = ssMacroSugesstionList.GetRowData(e.Row) as WorkerHealthCheckMacrowordDto;
            if (dto.SUGESSTION != null)
            {
                if (ChkOverWrite2.Checked)
                {
                    string sugesstion = "";
                    int selectionIndex2 = txtSugesstion.SelectionStart;
                    if (txtSugesstion.Text.Length > 0)
                    {
                        sugesstion += "\r\n" + dto.SUGESSTION;
                    }
                    else
                    {
                        sugesstion = dto.SUGESSTION;
                    }
                  
                    txtSugesstion.Text = txtSugesstion.Text.Insert(selectionIndex2, sugesstion);
                    txtSugesstion.SelectionStart = selectionIndex2 + sugesstion.Length;
                }
                else
                {
                    txtSugesstion.Text = dto.SUGESSTION;
                }
            }
        }

        private void RdoMacroType0_CheckedChanged(object sender, EventArgs e)
        {
            SearchMacro();
        }

        private void RdoMacro2Type0_CheckedChanged(object sender, EventArgs e)
        {
            SearchMacro2();
        }

        private void SSMemo_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            HIC_OSHA_PATIENT_MEMO dto = SSMemo.GetRowData(e.Row) as HIC_OSHA_PATIENT_MEMO;
            TxtMemo.Text = dto.MEMO;
        }

        private void SearchMemo(string workerId)
        {
            TxtMemo.Text = "";
            List <HIC_OSHA_PATIENT_MEMO> list = healthCheckMemoRepository.FindAll(workerId);
            SSMemo.SetDataSource(list);
            if (list.Count > 0)
            {
                TxtMemo.Text = list[0].MEMO;
                TxtMemo.Tag = list[0].ID;
            }
        }

        private void SearchRemark(string workerId)
        {
            TxtRemark.Text = string.Empty;
            List<HIC_OSHA_PATIENT_REMARK> list = healthCheckMemoRepository.FindRemarkAll(workerId);
            if (list.Count > 0)
            {
                TxtRemark.Text = list[0].REMARK;
                TxtRemark.Tag = list[0].ID;
            }
        }

        private void WorkerHealthCheckForm_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            if(MessageUtil.Confirm("메모를 삭제 하시겠습니까?") == DialogResult.Yes)
            {
                HIC_OSHA_PATIENT_MEMO dto = SSMemo.GetRowData(e.Row) as HIC_OSHA_PATIENT_MEMO;
                healthCheckMemoRepository.Delete(dto.ID);

                SearchMemo(dto.WORKER_ID);
            }
       
        }

        /// <summary>
        /// 메모저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveMemo_Click(object sender, EventArgs e)
        {
            if (!TxtMemo.Text.IsNullOrEmpty())
            {
                HIC_OSHA_PATIENT_MEMO memoDto = new HIC_OSHA_PATIENT_MEMO();
                memoDto.MEMO = TxtMemo.Text;
                memoDto.WORKER_ID = SELECTED_WORKED.Worker_ID;
                healthCheckMemoRepository.Insert(memoDto);
                TxtMemo.Text = "";
                SearchMemo(SELECTED_WORKED.Worker_ID);
            }
        }

        /// <summary>
        /// 특이사항 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveRemark_Click(object sender, EventArgs e)
        {
            if (TxtRemark.Text.NotEmpty())
            {
                HIC_OSHA_PATIENT_REMARK dto = new HIC_OSHA_PATIENT_REMARK();
                dto.REMARK = TxtRemark.Text;
                dto.WORKER_ID = SELECTED_WORKED.Worker_ID;
                dto.SITE_ID = base.SelectedSite.ID;

                healthCheckMemoRepository.InsertRemark(dto);
                TxtRemark.Text = string.Empty;
                SearchRemark(SELECTED_WORKED.Worker_ID);
            }
        }

        /// <summary>
        /// 특이사항 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteRemark_Click(object sender, EventArgs e)
        {
            TxtRemark.Text = string.Empty;
            if (TxtRemark.Tag != null)
            {
                HIC_OSHA_PATIENT_REMARK memoDto = new HIC_OSHA_PATIENT_REMARK();
                memoDto.ID = TxtRemark.Tag.To<long>(0);
                memoDto.WORKER_ID = SELECTED_WORKED.Worker_ID;
                healthCheckMemoRepository.DeleteRemark(memoDto);
            }
        }

        private void ChkIsManageOsha_CheckedChanged(object sender, EventArgs e)
        {
            HealthCheckDto dto = panHealthCheck.GetData<HealthCheckDto>();
            if (dto.worker_id.IsNullOrEmpty())
            {
                MessageUtil.Alert("근로자를 선택하세요");
                return;
            }

            if (hcSiteWorkerService.UpdateManageOsha(dto.worker_id, ChkIsManageOsha.Checked))
            {
                if (ChkIsManageOsha.Checked)
                {
                    ChangeRowColor(dto.worker_id, true);
                }
                else
                {
                    ChangeRowColor(dto.worker_id, false);
                }
            }
        }

        private void ChkSangDam_CheckedChanged(object sender, EventArgs e)
        {
            //Search();
        }

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            if (txtSearchName.Text.Length <= 0)
            {
                //Search();
            }
        }

        private void SSHistory_CellClick(object sender, CellClickEventArgs e)
        {

        }

        private void SSWorkerList_CellClick_1(object sender, CellClickEventArgs e)
        {

        }
    }
}
