using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    public partial class frmEmrBaseChartView : Form, MainFormMessage, FormEmrMessage
    {
        #region //폼에서 사용하는 변수

        EmrPatient AcpEmr = null;
        EmrForm fView = null;

        string mPTNO = string.Empty;

        /// <summary>
        /// 차트 여러개일경우 해당차트들 EMRNO 집어넣는 변수
        /// </summary>
        List<string> lstEmrNo = new List<string>();

        int SelectIndex = 0;

        //bool mViewNpChart = false;

        /// <summary>
        /// 최종 입원내역
        /// </summary>
        string strMaxIndate = string.Empty;

        #endregion //폼에서 사용하는 변수

        #region //서브폼 선언부

        /// <summary>
        /// 활력측정 폼
        /// </summary>
        FrmVital_D frmVital = null;

        /// <summary>
        /// 내원내역
        /// </summary>
        frmEmrBaseAcpList fEmrBaseAcpList;  //내원내역
        /// <summary>
        /// 차트 연속보기
        /// </summary>
        frmEmrBaseContinuView fEmrBaseContinuView = null;  //차트 연속보기
        /// <summary>
        /// 심사팀용
        /// </summary>
        frmEmrBaseViewVitalandActing frmEmrBaseViewVitalandActingX = null;

        /// <summary>
        /// 임상관찰 기록지
        /// </summary>
        frmEmrVitalSign fEmrVitalSignX = null;

        /// <summary>
        /// 기록지 폼 변수
        /// </summary>
        Form ActiveFormView = null;
        /// <summary>
        /// 스캔 폼 변수
        /// </summary>
        Form ActiveScanView = null;
        EmrChartForm ActiveFormViewChart = null;


        #endregion

        #region //MainFormMessage
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion //MainFormMessage

        #region //FormEmrMessage
        public FormEmrMessage mEmrCallForm = null;
        public void MsgSave(string strSaveFlag)
        {
            if (fEmrBaseAcpList != null && AcpEmr != null)
            {
                fEmrBaseAcpList.SetAutoRefresh(AcpEmr.inOutCls, AcpEmr.medFrDate, AcpEmr.medDeptCd, strSaveFlag);
                if (panNext.Visible)
                {
                    SetChartList();
                }
            }
            //if (IsOrderSave == true)
            //{
            //    IsOrderSave = false;
            //    return;
            //}
            //IsOrderSave = false;
            ////----상용기록
            ////----전체기록
            //if (mstrEMRVIEWNEW == "1")
            //{
            //    if (fEmrChartHisUserDrNew != null)
            //    {
            //        fEmrChartHisUserDrNew.RvClearForm();
            //        fEmrChartHisUserDrNew.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //    if (fEmrChartHisAllDrNew != null)
            //    {
            //        fEmrChartHisAllDrNew.RvClearForm();
            //        fEmrChartHisAllDrNew.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //}
            //else
            //{
            //    if (fEmrChartHisUserDr != null)
            //    {
            //        fEmrChartHisUserDr.RvClearForm();
            //        fEmrChartHisUserDr.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //    if (fEmrChartHisAllDr != null)
            //    {
            //        fEmrChartHisAllDr.RvClearForm();
            //        fEmrChartHisAllDr.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //}

        }
        public void MsgDelete()
        {
            if (fEmrBaseAcpList != null && AcpEmr != null)
            {
                fEmrBaseAcpList.SetAutoRefresh(AcpEmr.inOutCls, AcpEmr.medFrDate, AcpEmr.medDeptCd);
                if (panNext.Visible)
                {
                    SetChartList();
                }
            }
            //if (IsOrderSave == true)
            //{
            //    IsOrderSave = false;
            //    return;
            //}
            //IsOrderSave = false;
            ////----상용기록
            ////----전체기록
            //if (mstrEMRVIEWNEW == "1")
            //{
            //    if (fEmrChartHisUserDrNew != null)
            //    {
            //        fEmrChartHisUserDrNew.RvClearForm();
            //        fEmrChartHisUserDrNew.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //    if (fEmrChartHisAllDrNew != null)
            //    {
            //        fEmrChartHisAllDrNew.RvClearForm();
            //        fEmrChartHisAllDrNew.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //}
            //else
            //{
            //    if (fEmrChartHisUserDr != null)
            //    {
            //        fEmrChartHisUserDr.RvClearForm();
            //        fEmrChartHisUserDr.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //    if (fEmrChartHisAllDr != null)
            //    {
            //        fEmrChartHisAllDr.RvClearForm();
            //        fEmrChartHisAllDr.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //}

        }
        public void MsgClear()
        {
            ComFunc.MsgBoxEx(this, "MsgClear");
        }
        public void MsgPrint()
        {

        }
        #endregion

        #region //이벤트 전달
        //폼이 Close될 경우
        //public delegate void EventClosed();
        //public event EventClosed rEventClosed;

        //EasManager easManager = null;

        /// <summary>
        /// 연속보기 기록지 내용이 변경되었는지 확인한다.
        /// </summary>
        /// <returns></returns>
        public string CheckChartChangeData()
        {
            string rtnVal = string.Empty;
            if (fEmrBaseContinuView != null)
            {
                rtnVal = fEmrBaseContinuView.CheckChartChangeData();
            }

            return rtnVal;
        }

        /// <summary>
        /// 연속보기 기록지 저장 함수
        /// </summary>
        /// <returns></returns>
        public double SaveData()
        {
            double rtnVal = 0;
            if (fEmrBaseContinuView != null)
            {
                rtnVal = fEmrBaseContinuView.SaveData();
            }

            return rtnVal;
        }

        public void SubFormClear()
        {
            if (frmVital != null)
            {
                frmVital.Dispose();
                frmVital = null;
            }

            if (fEmrBaseAcpList != null)
            {
                fEmrBaseAcpList.Dispose();
                fEmrBaseAcpList = null;
            }

            if (fEmrBaseContinuView != null)
            {
                fEmrBaseContinuView.SubFormClear();
                fEmrBaseContinuView.Dispose();
                fEmrBaseContinuView = null;
            }

            if (ActiveScanView != null)
            {
                ActiveScanView.Dispose();
                ActiveScanView = null;
                ActiveFormViewChart = null;
            }

            if (ActiveFormView != null)
            {
                ActiveFormView.Dispose();
                ActiveFormView = null;
                ActiveFormViewChart = null;
            }

            if (frmEmrBaseViewVitalandActingX != null)
            {
                frmEmrBaseViewVitalandActingX.Dispose();
                frmEmrBaseViewVitalandActingX = null;
            }
        }

        public void ClearForm()
        {
            AcpEmr = null;
            fView = null;
            mPTNO = "";

            ClearChartRecInfo();

            if (fEmrBaseAcpList != null)
            {
                fEmrBaseAcpList.ClearForm();
            }

            if (fEmrBaseContinuView != null)
            {
                fEmrBaseContinuView.ClearForm();
            }

            if (ActiveScanView != null)
            {
                ActiveScanView.Visible = false;
            }

            if (ActiveFormView != null)
            {
                ActiveFormView.Dispose();
                ActiveFormView = null;
                ActiveFormViewChart = null;
            }

            if (frmEmrBaseViewVitalandActingX != null)
            {
                frmEmrBaseViewVitalandActingX.Dispose();
                frmEmrBaseViewVitalandActingX = null;
            }
        }

        public void GetJupHis(string pPTNO)
        {
            mPTNO = pPTNO;

            if (mPTNO.Trim() != "")
            {
                GetPatInfo();
                if (fEmrBaseAcpList == null)
                {
                    fEmrBaseAcpList = new frmEmrBaseAcpList();
                    //fEmrBaseAcpList.rEventClosed += new frmEmrBaseAcpList.EventClosed(frmEmrBaseAcpList_EventClosed);
                    fEmrBaseAcpList.rViewChart += new frmEmrBaseAcpList.ViewChart(frmEmrBaseAcpList_ViewChart);
                    fEmrBaseAcpList.rViewPanOCSFirstOpen += new frmEmrBaseAcpList.ViewPanOCSFirstOpen(ViewPanOCSFirstOpen);
                    if (fEmrBaseAcpList != null)
                    {
                        SubFormToControl(fEmrBaseAcpList, panViewEmrAcp, "Fill", true, true);
                    }
                }
                fEmrBaseAcpList.GetJupHis(mPTNO);

                if (fEmrBaseContinuView == null)
                {
                    fEmrBaseContinuView = new frmEmrBaseContinuView(this);
                    //fEmrBaseContinuView.rEventClosed += new frmEmrBaseContinuView.EventClosed(frmEmrBaseContinuView_EventClosed);
                    if (fEmrBaseContinuView != null)
                    {
                        SubFormToControl(fEmrBaseContinuView, panContinueView, "None", true, true);
                    }
                }
                fEmrBaseContinuView.ClearForm();
                fEmrBaseContinuView.GetContinuView(mPTNO.Trim(), "I");
            }
        }

        public void SetPTNO(string pPTNO)
        {

        }

        #endregion

        #region //공통함수

        private void ClearChartRecInfo()
        {
            lblDept.Text = "";
            lblMedDate.Text = "";
            lblWard.Text = "";
            lblViewFORMNAME.Text = "";
        }

        private void SetChartRecInfo(EmrPatient tAcp, EmrForm tForm)
        {
            if (tAcp == null || tForm == null)
                return;

            lblDept.Text = tAcp.medDeptCd;
            //lblDrName.Text = tAcp.medDrName != "" ? tAcp.medDrName : GetDrNm(tAcp.medDrCd);
            //lblUseName.Text = tAcp.writeName;
            lblFrDate.Text = tAcp.inOutCls == "O" ? "내원일 : " : "입원일 : ";
            lblMedDate.Text = string.IsNullOrEmpty(tAcp.medFrDate) == false ? DateTime.ParseExact(tAcp.medFrDate, "yyyyMMdd", null).ToShortDateString() : "";

            if (string.IsNullOrEmpty(tAcp.medEndDate) == false && tAcp.inOutCls == "I")
            {
                lblMedDate.Text += " ~ " + DateTime.ParseExact(tAcp.medEndDate, "yyyyMMdd", null).ToShortDateString();
            }
            else if (string.IsNullOrEmpty(tAcp.medEndDate) && tAcp.inOutCls == "I")
            {
                lblMedDate.Text += " ~ ";
            }

            lblWard.Text = tAcp.ward;
            lblMedDate.Left = lblFrDate.Left + lblFrDate.Width;
            panNext.Left = lblJumin.Left + lblJumin.Width + 5;
            btnSearchHis.Left = panNext.Left + panNext.Width + 5;
            ssTemp.Left = btnSearchHis.Left;

            if (tForm == null)
            {
                lblViewFORMNAME.Text = "영상EMR";
            }
            else if (string.IsNullOrEmpty(tAcp.chartDate) == false && tForm.FmFORMTYPE != "3")
            {
                //의료정보팀
                if (clsType.User.BuseCode.Equals("044201"))
                {
                    if (string.IsNullOrWhiteSpace(tAcp.writeSabun))
                    {
                        lblViewFORMNAME.Text = string.Format("{0}(작성자:{1} {2} {3})", tForm.FmFORMNAME, tAcp.writeName, VB.Val(tAcp.writeDate).ToString("0000-00-00"), VB.Val(VB.Left(tAcp.writeTime, 4)).ToString("00:00"));
                    }
                    else
                    {
                        if (tAcp.compuseSabun.NotEmpty() && tAcp.writeSabun.Equals(tAcp.compuseSabun) == false)
                        {
                            lblViewFORMNAME.Text = string.Format("{0}(작성자:[{1}] {2} {3} {4}) 확인자: {5}", tForm.FmFORMNAME, tAcp.writeSabun, tAcp.writeName, VB.Val(tAcp.writeDate).ToString("0000-00-00"), VB.Val(VB.Left(tAcp.writeTime, 4)).ToString("00:00"), tAcp.compuseName);
                        }
                        else
                        {
                            lblViewFORMNAME.Text = string.Format("{0}(작성자:[{1}] {2}  {3} {4})", tForm.FmFORMNAME, tAcp.writeSabun, tAcp.writeName, VB.Val(tAcp.writeDate).ToString("0000-00-00"), VB.Val(VB.Left(tAcp.writeTime, 4)).ToString("00:00"));
                        }
                    }
                }
                else
                {
                    if (tAcp.compuseSabun.NotEmpty() && tAcp.writeSabun.Equals(tAcp.compuseSabun) == false)
                    {
                        lblViewFORMNAME.Text = string.Format("{0}(작성자: {1} {2} {3}) 확인자: {4}", tForm.FmFORMNAME, tAcp.writeName, VB.Val(tAcp.chartDate).ToString("0000-00-00"), VB.Val(VB.Left(tAcp.chartTime, 4)).ToString("00:00"), tAcp.compuseName);
                    }
                    else
                    {
                        lblViewFORMNAME.Text = string.Format("{0}(작성자: {1} {2} {3})", tForm.FmFORMNAME, tAcp.writeName, VB.Val(tAcp.chartDate).ToString("0000-00-00"), VB.Val(VB.Left(tAcp.chartTime, 4)).ToString("00:00"));
                    }
                }
            }
            else
            {
                if (tForm.FmFORMTYPE == "2")
                {
                    //전자동의서
                    lblViewFORMNAME.Text = string.Format("{0}(작성자: {1}  {2})", tForm.FmFORMNAME, tAcp.writeName, tAcp.writeDate.ToString());
                }
                else
                {
                    lblViewFORMNAME.Text = tForm.FmFORMNAME;
                }

            }

            panInfo.Left = lblViewFORMNAME.Left + lblViewFORMNAME.Width + 10;

            //lblViewFORMNAME.Text = tForm.FmFORMNAME;
        }

        private void SubFormToControl(Form frm, Control pControl, string DockForm, bool FitSize_H, bool FitSize_W)
        {
            frm.Owner = this;
            frm.TopLevel = false;
            this.Controls.Add(frm);
            frm.Parent = pControl;
            frm.Text = "";
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.WindowState = FormWindowState.Normal;
            if (FitSize_H == true)
            {
                frm.Height = pControl.Height;
            }
            if (FitSize_W == true)
            {
                frm.Width = pControl.Width;
            }
            if (DockForm == "Fill")
            {
                frm.Dock = DockStyle.Fill;
            }
            frm.Show();

        }

        private void ViewPanOCSFirstOpen(string argFirstOpen)
        {
            //김병욱 의무원장 사번 46037 OCS에서 환자 선택 시 
            //최상단 내역 자동 더블클릭하여 하단 챠트 목록 나오고
            //자동으로 연속보기로 보길 원하셔서 하드코딩(2021-03-22)
            SetPanelView("연속보기");
        }

        private void LoadSubForm()
        {
            fEmrBaseAcpList = new frmEmrBaseAcpList();
            //fEmrBaseAcpList.rEventClosed += new frmEmrBaseAcpList.EventClosed(frmEmrBaseAcpList_EventClosed);
            fEmrBaseAcpList.rViewChart += new frmEmrBaseAcpList.ViewChart(frmEmrBaseAcpList_ViewChart);
            fEmrBaseAcpList.rViewPanOCSFirstOpen += new frmEmrBaseAcpList.ViewPanOCSFirstOpen(ViewPanOCSFirstOpen);

            if (fEmrBaseAcpList != null)
            {
                SubFormToControl(fEmrBaseAcpList, panViewEmrAcp, "Fill", true, true);
            }

            fEmrBaseContinuView = new frmEmrBaseContinuView(this);
            //fEmrBaseContinuView.rEventClosed += new frmEmrBaseContinuView.EventClosed(frmEmrBaseContinuView_EventClosed);
            if (fEmrBaseContinuView != null)
            {
                SubFormToControl(fEmrBaseContinuView, panContinueView, "None", true, true);
            }

            //string NewScanTest = clsEmrQuery.NewScanViewTest(clsDB.DbCon);
            //if (NewScanTest.Equals("Y") && clsEmrQuery.NewScanViewTestIP(clsDB.DbCon, clsCompuInfo.gstrCOMIP))
            //{
             ActiveScanView = new frmScanImageViewNew3(this, "0", "0", "0");
            //}
            //else
            //{
            //    ActiveScanView = new frmScanImageViewNew2(this, "0", "0", "0");
            //}
            
            SubFormToControl(ActiveScanView, panEmrViewMain, "None", true, true);
            ActiveScanView.Visible = false;
        }

        private void FrmVital_rEventClosed()
        {
            if (frmVital != null)
            {
                frmVital.Dispose();
                frmVital = null;
            }
        }

        private void frmEmrBaseContinuView_EventClosed()
        {
            fEmrBaseContinuView.Close();
            fEmrBaseContinuView.Dispose();
            fEmrBaseContinuView = null;
        }

        private void frmEmrBaseAcpList_EventClosed()
        {
            fEmrBaseAcpList.Close();
            fEmrBaseAcpList.Dispose();
            fEmrBaseAcpList = null;
        }

        private void frmEmrBaseAcpList_ViewChart(EmrPatient tAcp, EmrForm tForm, string strEmrNo, string strTreatNo, string strSCANYN, string strFormCode, string strFormCnt, string strInOutCls)
        {
            panNext.Visible = false;
            btnSearchHis.Visible = false;
            ssTemp.Visible = false;

            ssTemp_Sheet1.RowCount = 0;
            lstEmrNo.Clear();
            SelectIndex = 0;

            SetPanelView("구분보기");
            
            //if (mEmrCallForm == null)
            //{
            //    panViewEmrChart.Visible = true;
            //    panContinueView.Visible = false;
            //    btnViewChartOrHis.Text = "구분보기";
            //    btnViewChartOrHis.Visible = false;
            //}
            //else
            //{
            //    if (((Form)mEmrCallForm).Name != "frmEmrViewMain")
            //    {
            //        panViewEmrChart.Visible = true;
            //        panContinueView.Visible = false;
            //        btnViewChartOrHis.Text = "구분보기";
            //        btnViewChartOrHis.Visible = false;
            //    }
            //    else
            //    {
            //        //if (tForm != null)
            //        //{
            //        //    //경과기록지 호출및 입원이고 과도 같으며 최종 입원일짜가 동일할때만
            //        //    if (tForm.FmFORMNO == 963 && tAcp.inOutCls == "I" && clsType.User.DeptCode == tAcp.medDeptCd &&
            //        //        strMaxIndate == tAcp.medFrDate)
            //        //    {
            //        //        SetPanelView("연속보기");
            //        //    }
            //        //    else
            //        //    {
            //        //        SetPanelView("구분보기");
            //        //    }
            //        //}
            //        //else
            //        //{
            //        //    SetPanelView("구분보기");
            //        //}

            //        SetPanelView("구분보기");
            //    }
            //}

            ViewChart(tAcp, tForm, strEmrNo, strTreatNo, strSCANYN, strFormCode, strFormCnt, strInOutCls);
        }

        private void ResizeSubForm()
        {
            try
            {
                //ActiveScanView.WindowState = FormWindowState.Normal;
                //ActiveScanView.WindowState = FormWindowState.Maximized;

                if (ActiveFormView == null) return;

                if (fView.FmALIGNGB == 1)   //Left
                {
                    panOption.Visible = false;
                    ActiveFormView.Height = panEmrViewMain.Height - 20;
                }
                else if (fView.FmALIGNGB == 2)  //Top
                {
                    panOption.Visible = false;
                    ActiveFormView.Width = panEmrViewMain.Width - 20;
                }
                else  //None
                {
                    ActiveFormView.Dock = DockStyle.None;
                    ActiveFormView.Dock = DockStyle.Fill;
                }
            }
            catch { }
        }

        #endregion 

        public frmEmrBaseChartView()
        {
            InitializeComponent();
            //easManager = new EasManager();
        }

        public frmEmrBaseChartView(FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();
            mEmrCallForm = pEmrCallForm;
            //easManager = new EasManager();
        }

        public frmEmrBaseChartView(string pPTNO)
        {
            InitializeComponent();
            mPTNO = pPTNO;
            //easManager = new EasManager();
        }

        public frmEmrBaseChartView(FormEmrMessage pEmrCallForm, string pPTNO)
        {
            InitializeComponent();
            mEmrCallForm = pEmrCallForm;
            mPTNO = pPTNO;
            //easManager = new EasManager();
        }

        private void frmEmrBaseChartView_Load(object sender, EventArgs e)
        {
            //if (mPTNO.Trim() != "")
            //{
            //    fEmrBaseAcpList.GetJupHis(mPTNO);
            //}

            ClearChartRecInfo();

            panViewEmrChart.Dock = DockStyle.Fill;
            panContinueView.Dock = DockStyle.Fill;
            LoadSubForm();

            panViewEmrChart.Visible = false;
            panContinueView.Visible = true;
            panContinueView.BringToFront();

            panSideBarLeft.Visible = false;

            if (clsType.User.BuseCode == "078201")
            {
                btnViewVitalAndActing.Visible = true;
                btnDetail1.Visible = true;
                btnDetail2.Visible = true;

                btnDetail1.Left = btnViewChartOrHis.Left;
                btnDetail2.Left = btnDetail1.Left + btnDetail2.Width;
                btnViewVitalAndActing.Left = btnDetail2.Left + btnDetail2.Width;
            }

            if (mEmrCallForm == null)
            {
                panViewEmrChart.Visible = true;
                panContinueView.Visible = false;
                btnViewChartOrHis.Text = "구분보기";
                btnViewChartOrHis.Visible = false;
                btnViewRecord.Visible = false;
            }
            else
            {
                if (((Form)mEmrCallForm).Name != "frmEmrViewMain")
                {
                    panViewEmrChart.Visible = true;
                    panContinueView.Visible = false;
                    btnViewChartOrHis.Text = "구분보기";
                    btnViewChartOrHis.Visible = false;
                    btnViewRecord.Visible = false;
                }
            }

            if (mPTNO != "")
            {
                fEmrBaseContinuView.ClearForm();
                fEmrBaseContinuView.GetContinuView(mPTNO.Trim(), "I");
            }

            //【EMR 뷰어 설정-내원내역】
            /*   RegistryKey reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("EmrSetting");
               string strEmrGb = reg.GetValue("VIEWACPLISTVIEW", string.Empty).ToString();
               if(strEmrGb.Equals("1"))
               {
                   btnSideBarLeftClick();
               }
               reg.Close();
               reg.Dispose();
             */
            //string strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "VIEWACPLISTVIEW");
            //if (VB.Val(strEmrOption) == 1)
            //{
            //    btnSideBarLeftClick();
            //}
        }

        /// <summary>
        /// 환자정보 가져오기
        /// </summary>
        private void GetPatInfo()
        {
            string SQL = string.Empty;
            string sqlErr = string.Empty;
            OracleDataReader reader = null;


            lblPtName.Text = string.Empty;
            lblJumin.Text = string.Empty;
            string strCurDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToShortDateString();

            try
            {
                SQL = " SELECT JUMIN1, SUBSTR(JUMIN2, 1, 1) JUMIN2, SEX, JUMIN3, SNAME";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                SQL += ComNum.VBLF + "WHERE PANO = '" + mPTNO + "'";

                sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "차트리스트를 가져오는 도중 오류가 발생했습니다.");
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    lblPtName.Text = string.Format("등록번호: {0}  이름 : {1}     ", mPTNO, reader.GetValue(4).ToString().Trim());

                    string Jumin3 = clsAES.DeAES(reader.GetValue(3).ToString().Trim());

                    lblJumin.Text = string.Format("주민번호: {0}-{1} ({2}/{3})",
                        reader.GetValue(0).ToString().Trim(),
                        clsType.User.BuseCode.Equals("044201") ? Jumin3 : reader.GetValue(1).ToString().Trim(),
                    reader.GetValue(2).ToString().Trim().Equals("M") ? "남" : "여",
                    ComFunc.AgeCalcEx(
                        reader.GetValue(0).ToString().Trim() + Jumin3, strCurDate)
                    );

                    lblJumin.Left = lblPtName.Left + lblPtName.Width + 5;
                    lblPtName.Visible = true;
                    lblJumin.Visible = true;
                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void ViewChart(EmrPatient tAcp, EmrForm tForm, string strEmrNo, string strTreatNo, string strSCANYN, string strFormCode, string strFormCnt, string strInOutCls)
        {
            //try
            //{
            AcpEmr = tAcp;
            fView = tForm;

            if (ActiveFormView != null)
            {
                ActiveFormView.Close();
                ActiveFormView.Dispose();
                ActiveFormView = null;
                ActiveFormViewChart = null;
            }

            ClearChartRecInfo();
            string NewScanTest = clsEmrQuery.NewScanViewTest(clsDB.DbCon);
            if (strSCANYN == "S")
            {
                panel21.Visible = false;
                if (ActiveScanView != null && ActiveScanView.IsDisposed == false)
                {
                    if (strTreatNo.Equals("0"))
                    {
                        ActiveScanView.Visible = false;
                        return;
                    }

                    ActiveScanView.WindowState = FormWindowState.Normal;
                    ActiveScanView.Width = panEmrViewMain.Width;
                    ActiveScanView.Height = panEmrViewMain.Height;
                    ActiveScanView.Visible = true;
                    //if (NewScanTest.Equals("Y") && clsEmrQuery.NewScanViewTestIP(clsDB.DbCon, clsCompuInfo.gstrCOMIP))
                    //{
                    //    if (ActiveScanView is frmScanImageViewNew2)
                    //    {
                        //    ActiveScanView.Dispose();
                        //    ActiveScanView = new frmScanImageViewNew3(this, strTreatNo, strFormCode, "0");

                        //    ActiveScanView.TopLevel = false;
                        //    this.Controls.Add(ActiveScanView);
                        //    ActiveScanView.Parent = panEmrViewMain;
                        //    ActiveScanView.Text = "영상EMR";
                        //    ActiveScanView.ControlBox = false;
                        //    ActiveScanView.FormBorderStyle = FormBorderStyle.None;
                        //    ActiveScanView.Top = 0;
                        //    ActiveScanView.Left = 0;
                        //    ActiveScanView.WindowState = FormWindowState.Maximized;
                        //    ActiveScanView.Dock = DockStyle.Fill;
                        //    ActiveScanView.Show();
                        //}
                        //else
                        //{
                            (ActiveScanView as frmScanImageViewNew3).gPatientinfoRecive(strTreatNo, strFormCode, "0");
                        //}

                    //}
                    //else
                    //{
                    //    if (ActiveScanView is frmScanImageViewNew3)
                    //    {
                    //        ActiveScanView.Dispose();
                    //        ActiveScanView = new frmScanImageViewNew2(this, strTreatNo, strFormCode, "0");

                    //        ActiveScanView.TopLevel = false;
                    //        this.Controls.Add(ActiveScanView);
                    //        ActiveScanView.Parent = panEmrViewMain;
                    //        ActiveScanView.Text = "영상EMR";
                    //        ActiveScanView.ControlBox = false;
                    //        ActiveScanView.FormBorderStyle = FormBorderStyle.None;
                    //        ActiveScanView.Top = 0;
                    //        ActiveScanView.Left = 0;
                    //        ActiveScanView.WindowState = FormWindowState.Maximized;
                    //        ActiveScanView.Dock = DockStyle.Fill;
                    //        ActiveScanView.Show();
                    //    }
                    //    else
                    //    {
                    //        (ActiveScanView as frmScanImageViewNew2).gPatientinfoRecive(strTreatNo, strFormCode, "0");
                    //    }
                    //}
                    return;
                }
            }
            else
            {
                panel21.Visible = true;

                if (ActiveScanView != null)
                {
                    ActiveScanView.Visible = false;
                }
                if (ActiveFormView != null)
                {
                    ActiveFormView.Close();
                    ActiveFormView.Dispose();
                    ActiveFormView = null;
                    ActiveFormViewChart = null;
                }
            }

            if (tForm == null && strSCANYN.Equals("S") == false) // && strInOutCls.Equals("I"))
                return;

            //경과기록지, 경과이미지 일때 연속보기로 볼 수 있게.
            if (tForm != null && (tForm.FmFORMNO == 963 && strSCANYN.Equals("T") || tForm.FmFORMNO == 1232))
            {
                if (clsType.User.AuAMANAGE != "1" && clsType.User.BuseCode.Equals("078201") == false || clsType.User.BuseCode.Equals("078201") && tForm.FmFORMNO == 963)
                {
                    ActiveFormView = new frmEmrBaseContinuView(AcpEmr, tForm);
                    ActiveFormView.TopLevel = false;
                    this.Controls.Add(ActiveFormView);
                    ActiveFormView.Parent = panEmrViewMain;
                    ActiveFormView.Text = tForm.FmFORMNAME;
                    ActiveFormView.ControlBox = false;
                    ActiveFormView.FormBorderStyle = FormBorderStyle.None;
                    ActiveFormView.Top = 0;
                    ActiveFormView.Left = 0;
                    ActiveFormView.Dock = DockStyle.Fill;
                    ActiveFormView.Show();

                    ((frmEmrBaseContinuView)ActiveFormView).SetContinuView();
                    SetChartInfo(strEmrNo, ref tAcp);
                    SetChartRecInfo(tAcp, tForm);
                    return;
                }

                tForm.FmFORMTYPE = "3";
            }

            string NewRecord = clsEmrQuery.NewArgreeRecord(clsDB.DbCon);

            if (strSCANYN == "S")
            {
                if (ActiveScanView == null)
                {
                    //if (NewScanTest.Equals("Y") && clsEmrQuery.NewScanViewTestIP(clsDB.DbCon, clsCompuInfo.gstrCOMIP))
                    //{
                        ActiveScanView = new frmScanImageViewNew3(this, strTreatNo, strFormCode, "0");
                    //}
                    //else
                    //{
                    //    ActiveScanView = new frmScanImageViewNew2(this, strTreatNo, strFormCode, "0");
                    //}

                    ActiveScanView.TopLevel = false;
                    this.Controls.Add(ActiveScanView);
                    ActiveScanView.Parent = panEmrViewMain;
                    ActiveScanView.Text = "영상EMR";
                    ActiveScanView.ControlBox = false;
                    ActiveScanView.FormBorderStyle = FormBorderStyle.None;
                    ActiveScanView.Top = 0;
                    ActiveScanView.Left = 0;
                    ActiveScanView.WindowState = FormWindowState.Maximized;
                    ActiveScanView.Dock = DockStyle.Fill;
                    ActiveScanView.Show();
                    return;
                }
            }
            else if (strSCANYN == "E")
            {

                if (VB.Val(strFormCnt) > 1)
                {
                    SetChartList(strSCANYN);
                    strEmrNo = lstEmrNo[0];
                }

                //전자 동의서
                EasManager easManager = new EasManager();
                //ActiveFormView = easManager.GetEasFormDesigner();
                //easManager.View(tAcp.formNo, strEmrNo);

                ActiveFormView = easManager.GetEasFormViewer(panSplitV);
                easManager.View(tForm, tAcp, tForm.FmFORMNO, strEmrNo);

            }
            else
            {
                //ActiveFormView = new frmEmrChartNew(tAcp.formNo.ToString(), tAcp.updateNo.ToString(), tAcp, strEmrNo, "V", this);

                if (tForm.FmFORMNO == 965 || tForm.FmFORMNO == 2137 || tForm.FmFORMNO == 2049) //간호기록
                {
                    tForm.FmFORMTYPE = "3";
                }
                else if ((tForm.FmFORMNO == 1965 || tForm.FmFORMNO == 3535) && tForm.FmOLDGB != 1)
                {
                    if (clsType.User.BuseCode.Equals("078201") == false)
                    {
                        tForm.FmFORMTYPE = "0";
                    }
                }
                else if (tForm.FmFORMNO == 3150 || tForm.FmFORMNO == 1969 && tForm.FmOLDGB == 0 ||
                         tForm.FmFORMNO == 1575 && tForm.FmOLDGB == 0 || tForm.FmPROGFORMNAME.Equals("frmEmrVitalSign")) //임상관찰, 응급실 V/S 기본간호활동)
                {
                    if (clsType.User.BuseCode.Equals("044201") || clsType.User.IdNumber.Equals("42388") || clsType.User.BuseCode.Equals("078201") || clsType.User.AuAWRITE.Equals("0"))
                    {
                        tForm.FmFORMTYPE = "3";
                    }
                }

                //장수가 1개가 아니고 정형화 서식일경우만 
                if (VB.Val(strFormCnt) > 1 && (tForm.FmFORMTYPE == "0" || tForm.FmFORMNO == 1568))
                {
                    SetChartList();
                    strEmrNo = lstEmrNo.Count > 0 ? lstEmrNo[0] : "";
                }

                //string strVal = "";


                #region 이전 동의서용
                if (NewRecord.Equals("N") && (tForm.FmGRPFORMNO >= 1050 && tForm.FmGRPFORMNO <= 1055 || tForm.FmGRPFORMNO == 1066 || tForm.FmGRPFORMNO == 1068 || tForm.FmFORMNO == 2148 && tForm.FmOLDGB == 1))
                {
                    ActiveFormView = new frmEmrBaseEmrChartOld(this, null, "OLD", "", "", tForm.FmFORMNO.ToString(), tForm.FmFORMNAME, strEmrNo);
                    ((frmEmrBaseEmrChartOld)ActiveFormView).rSaveOrDelete += frmEmrBaseEmrChartOld_SaveOrDelete;
                    //((frmEmrBaseEmrChartOld)ActiveFormView).rEventClosed += frmEmrBaseEmrChartOld_EventClosed;
                }
                #endregion

                ////else if (tForm.FmFORMTYPE == "4") //개발자가 만든것
                else if (tForm.FmFORMTYPE == "4") //개발자가 만든것
                {
                    //clsFormMap.EmrFormMapping("MHENRINS", strNameSpace, frmFORM.FmPROGFORMNAME, strFormNo, strUpdateNo, pEmrPatient, strEmrNo, "W");
                    ActiveFormView = clsEmrFormMap.EmrFormMappingEx(tForm.FmPROGFORMNAME, tForm.FmFORMNO.ToString(), tForm.FmUPDATENO.ToString(), tAcp, strEmrNo, "V", this);
                }
                else if (tForm.FmFORMTYPE == "3") //Flow
                {
                    #region 19-09-24 의료정보팀 요청으로 '간호기록'만 입퇴원 기간 안을 넘어서서 쓴 기록을 보기 위해서 추가함
                    if (tForm.FmFORMNO == 965 && clsType.User.BuseCode.Equals("044201"))
                    {
                        strInOutCls += "|" + strFormCnt;
                    }
                    #endregion
                    ActiveFormView = new frmEmrChartFlowOld(tForm.FmFORMNO.ToString(), tForm.FmUPDATENO.ToString(), tAcp, strEmrNo, "V", "", strInOutCls, this);
                    //ActiveFormView = new frmEmrPrintFlowSheet(tForm.FmFORMNO.ToString(), tForm.FmUPDATENO.ToString(), tAcp, strEmrNo, "V", strVal, this);
                }
                else if (tForm.FmFORMTYPE == "2") //전자동의서
                {
                    ActiveFormView = new frmEmrChartNew(tForm.FmFORMNO.ToString(), tForm.FmUPDATENO.ToString(), tAcp, strEmrNo, "V", this);
                }
                else if (tForm.FmFORMTYPE == "1") //동의서
                {
                    ActiveFormView = new frmEmrChartNew(tForm.FmFORMNO.ToString(), tForm.FmUPDATENO.ToString(), tAcp, strEmrNo, "V", this);
                }
                else if (tForm.FmFORMTYPE == "0") //정형화 서식
                {
                    ActiveFormView = new frmEmrChartNew(tForm.FmFORMNO.ToString(), tForm.FmUPDATENO.ToString(), tAcp, strEmrNo, "V", this);
                }


            }

            if (NewRecord.Equals("N"))
            {
                #region 이전 동의서용
                if (strSCANYN != "S" && tForm != null &&
                (tForm.FmGRPFORMNO >= 1050 && tForm.FmGRPFORMNO <= 1055 || tForm.FmGRPFORMNO == 1066 || tForm.FmGRPFORMNO == 1068 || tForm.FmFORMNO == 2148 && tForm.FmOLDGB == 1))
                {


                }
                else
                {
                    ActiveFormViewChart = (EmrChartForm)ActiveFormView;
                }
                #endregion
            }
            else
            {
                ActiveFormViewChart = (EmrChartForm)ActiveFormView;
            }

            if (ActiveFormView == null)
                return;

            ActiveFormView.TopLevel = false;
            this.Controls.Add(ActiveFormView);
            ActiveFormView.Parent = panEmrViewMain;
            ActiveFormView.Text = strSCANYN == "S" ? "영상EMR" : tForm.FmFORMNAME;
            ActiveFormView.ControlBox = false;
            ActiveFormView.FormBorderStyle = FormBorderStyle.None;
            ActiveFormView.Top = 0;
            ActiveFormView.Left = 0;
            if (strSCANYN == "S")
            {
                ActiveFormView.WindowState = FormWindowState.Maximized;
                ActiveFormView.Dock = DockStyle.Fill;
            }
            else
            {
                if (tForm.FmALIGNGB == 1)   //Left
                {
                    panOption.Visible = false;
                    ActiveFormView.Height = panEmrViewMain.Height - 20;
                }
                else if (tForm.FmALIGNGB == 2)  //Top
                {
                    panOption.Visible = false;
                    ActiveFormView.Width = panEmrViewMain.Width - 20;
                }
                else  //None
                {
                    ActiveFormView.Dock = DockStyle.Fill;
                }
            }
            if (ActiveFormView.IsDisposed == false)
            {
                ActiveFormView.Show();
            }

            if (tForm.FmFORMNO != 1680 && tForm.FmFORMNO != 2090)
            {
                SetChartInfo(strEmrNo, ref tAcp);
            }
            SetChartRecInfo(tAcp, tForm);
            //}
            //catch(Exception ex)
            //{
            //    ComFunc.MsgBoxEx(this, ex.Message);
            //}
        }

        /// <summary>
        /// EMRNO로 CHARTDATE, CHARTTIME, WRITEDATE, WRITETIME등을 가져온다.
        /// </summary>
        /// <param name="patient"></param>
        private void SetChartInfo(string strEmrNo, ref EmrPatient patient)
        {
            if (patient == null || fView == null)
                return;

            string SQL = string.Empty;
            string sqlErr = string.Empty;
            OracleDataReader reader = null;

            try
            {
                if ((VB.Val(strEmrNo) == 0))
                {
                    SQL = "SELECT JUMIN3";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                    SQL += ComNum.VBLF + "WHERE PANO = '" + patient.ptNo + "'";
                }
                else
                {

                    if (fView.FmOLDGB == 1)
                    {
                        SQL = "SELECT CHARTDATE";
                        SQL += ComNum.VBLF + "      , CHARTTIME";
                        switch (fView.FmFORMNO)
                        {
                            case 2678:
                            case 3129:
                                SQL += ComNum.VBLF + "      , (SELECT NAME FROM KOSMOS_PMPA.BAS_PASS WHERE IDNUMBER = TO_NUMBER(A.USEID)  AND PROGRAMID = '      ') AS NAME";
                                break;
                            default:
                                SQL += ComNum.VBLF + "      , (SELECT NAME FROM KOSMOS_EMR.EMR_USERT WHERE USERID = LTRIM(A.USEID, '0')) AS NAME";
                                break;
                        }

                        SQL += ComNum.VBLF + "      , A.WRITEDATE";
                        SQL += ComNum.VBLF + "      , A.WRITETIME";
                        SQL += ComNum.VBLF + "      , C.JUMIN3";
                        SQL += ComNum.VBLF + "      , CASE WHEN EXISTS(SELECT 1 FROM KOSMOS_OCS.OCS_DOCTOR WHERE DOCCODE = LTRIM(A.USEID, '0')) THEN LTRIM(A.USEID, '0') END USEID";
                        SQL += ComNum.VBLF + "      , '' AS COMPUSENAME";
                        SQL += ComNum.VBLF + "      , '' AS COMPUSEID";

                        if (fView.FmFORMNO == 1796)
                        {
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXML_TUYAK A";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXMLMST A";
                        }
                    }
                    else
                    {
                        SQL = "SELECT CHARTDATE";
                        SQL += ComNum.VBLF + "      , CHARTTIME";
                        switch (fView.FmFORMNO)
                        {
                            case 2678:
                            case 3129:
                                SQL += ComNum.VBLF + "      , (SELECT NAME FROM KOSMOS_PMPA.BAS_PASS WHERE IDNUMBER = TO_NUMBER(A.CHARTUSEID)  AND PROGRAMID = '      ') AS NAME";
                                break;
                            default:
                                SQL += ComNum.VBLF + "      , (SELECT NAME FROM KOSMOS_EMR.EMR_USERT WHERE USERID = A.CHARTUSEID) AS NAME";
                                break;
                        }
                        SQL += ComNum.VBLF + "      , A.WRITEDATE";
                        SQL += ComNum.VBLF + "      , A.WRITETIME";
                        SQL += ComNum.VBLF + "      , C.JUMIN3";
                        SQL += ComNum.VBLF + "      , CASE WHEN EXISTS(SELECT 1 FROM KOSMOS_OCS.OCS_DOCTOR WHERE DOCCODE = A.CHARTUSEID) THEN A.CHARTUSEID END USEID";
                        SQL += ComNum.VBLF + "      , (SELECT NAME FROM KOSMOS_EMR.EMR_USERT WHERE USERID = A.COMPUSEID) AS COMPUSENAME";
                        SQL += ComNum.VBLF + "      , CASE WHEN EXISTS(SELECT 1 FROM KOSMOS_OCS.OCS_DOCTOR WHERE DOCCODE = A.COMPUSEID) THEN A.COMPUSEID END COMPUSEID";


                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                        SQL += ComNum.VBLF + "  INNER JOIN " + ComNum.DB_EMR + "EMR_USERT B";
                        SQL += ComNum.VBLF + "     ON B.USERID = A.CHARTUSEID";
                    }

                    SQL += ComNum.VBLF + "  INNER JOIN " + ComNum.DB_PMPA + "BAS_PATIENT C";
                    SQL += ComNum.VBLF + "     ON A.PTNO = C.PANO";
                    SQL += ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                }


                sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "차트리스트를 가져오는 도중 오류가 발생했습니다.");
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    if (VB.Val(strEmrNo) > 0)
                    {
                        patient.chartDate = reader.GetValue(0).ToString().Trim();
                        patient.chartTime = reader.GetValue(1).ToString().Trim();
                        patient.writeName = reader.GetValue(2).ToString().Trim();
                        patient.writeDate = reader.GetValue(3).ToString().Trim();
                        patient.writeTime = reader.GetValue(4).ToString().Trim();
                        patient.writeSabun = reader.GetValue(6).ToString().Trim();
                        patient.compuseName = reader.GetValue(7).ToString().Trim();
                        patient.compuseSabun = reader.GetValue(8).ToString().Trim();

                        if (clsType.User.BuseCode == "044201")
                        {
                            patient.ssno2 = clsAES.DeAES(reader.GetValue(5).ToString().Trim());
                        }
                    }
                    else
                    {
                        if (clsType.User.BuseCode == "044201")
                        {
                            patient.ssno2 = clsAES.DeAES(reader.GetValue(0).ToString().Trim());
                        }
                    }

                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void frmEmrBaseEmrChartOld_SaveOrDelete()
        {
            if (ActiveFormView == null)
                return;

            ActiveFormView.Dispose();
            ActiveFormView = null;
            //GetChartHis();
        }

        private void frmEmrBaseEmrChartOld_EventClosed()
        {
            if (ActiveFormView == null)
                return;

            ActiveFormView.Dispose();
            ActiveFormView = null;
        }

        /// <summary>
        /// 차트가 여러개일경우 
        /// lstEmrNo에 집어넣는다.
        /// </summary>
        private void SetChartList(string strSCANYN = "")
        {
            btnSearchHis.Visible = false;
            panNext.Visible = false;

            if (fView == null || AcpEmr == null)
                return;

            string SQL = string.Empty;
            string sqlErr = string.Empty;
            OracleDataReader reader = null;

            try
            {
                if (fView.FmOLDGB == 1)
                {
                    SQL = "SELECT EMRNO, CHARTDATE, CHARTTIME, USEID, B.FORMNAME, '인증' AS SAVECERT";
                }
                else
                {
                    SQL = "SELECT EMRNO, CHARTDATE, CHARTTIME, CHARTUSEID, B.FORMNAME, CASE WHEN A.SAVECERT = '0' THEN '임시' ELSE '인증' END SAVECERT";
                }


                if (fView.FmOLDGB == 1)
                {
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXMLMST A";
                }
                else
                {
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                }
                SQL += ComNum.VBLF + "  INNER JOIN " + ComNum.DB_EMR + "AEMRFORM B";
                SQL += ComNum.VBLF + "     ON A.FORMNO = B.FORMNO";
                SQL += ComNum.VBLF + "    AND B.UPDATENO = " + fView.FmUPDATENO;
                SQL += ComNum.VBLF + "WHERE MEDFRDATE = '" + AcpEmr.medFrDate + "'";
                SQL += ComNum.VBLF + "  AND A.PTNO   = '" + AcpEmr.ptNo + "'";
                SQL += ComNum.VBLF + "  AND A.FORMNO = " + fView.FmFORMNO;
                SQL += ComNum.VBLF + "  AND A.INOUTCLS = '" + AcpEmr.inOutCls + "'";
                SQL += ComNum.VBLF + "ORDER BY (CHARTDATE || CHARTTIME) DESC";


                if (strSCANYN == "E")
                {
                    SQL = "SELECT C.ID, C.CREATED, '18:00:00', C.USERID, B.FORMNAME, '' AS SAVECERT";
                    SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.AEASFORMCONTENT A ";
                    SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEMRFORM B ";
                    SQL += ComNum.VBLF + "       ON A.FORMNO = b.FORMNO ";
                    SQL += ComNum.VBLF + "      AND A.UPDATENO = B.UPDATENO ";
                    SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEASFORMDATA C ";
                    SQL += ComNum.VBLF + "       ON C.EASFORMCONTENT = A.ID ";
                    SQL += ComNum.VBLF + " WHERE C.PTNO      = '" + AcpEmr.ptNo + "'";
                    SQL += ComNum.VBLF + "   AND C.MEDDEPTCD = '" + AcpEmr.medDeptCd + "'";
                    SQL += ComNum.VBLF + "   AND C.INOUTCLS  = '" + AcpEmr.inOutCls + "'";
                    SQL += ComNum.VBLF + "   AND B.FORMNO    = " + fView.FmFORMNO;
                    SQL += ComNum.VBLF + "   AND B.UPDATENO  = " + fView.FmUPDATENO;
                    SQL += ComNum.VBLF + " ORDER BY C.CREATED DESC ";
                }


                sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "차트리스트를 가져오는 도중 오류가 발생했습니다.");
                    return;
                }

                if (reader.HasRows)
                {
                    ssTemp_Sheet1.RowCount = 0;
                    lstEmrNo.Clear();
                    while (reader.Read())
                    {
                        ssTemp_Sheet1.RowCount += 1;
                        lstEmrNo.Add(reader.GetValue(0).ToString().Trim());
                        ssTemp_Sheet1.Cells[ssTemp_Sheet1.RowCount - 1, 0].Text = reader.GetValue(4).ToString().Trim(); //FORMANME
                        ssTemp_Sheet1.Cells[ssTemp_Sheet1.RowCount - 1, 1].Text = reader.GetValue(1).ToString().Trim(); //CHARTDATE
                        ssTemp_Sheet1.Cells[ssTemp_Sheet1.RowCount - 1, 2].Text = reader.GetValue(2).ToString().Trim(); //CHARTTIME
                        ssTemp_Sheet1.Cells[ssTemp_Sheet1.RowCount - 1, 3].Text = reader.GetValue(5).ToString().Trim(); //SAVEGB
                        ssTemp_Sheet1.Cells[ssTemp_Sheet1.RowCount - 1, 4].Text = reader.GetValue(0).ToString().Trim(); //EMRNO
                        ssTemp_Sheet1.Cells[ssTemp_Sheet1.RowCount - 1, 5].Text = reader.GetValue(3).ToString().Trim(); //USEID
                    }

                    SelectIndex = 0;
                    lblCount.Text = string.Format("1 / {0}", lstEmrNo.Count);
                    if (lstEmrNo.Count > 1)
                    {
                        btnSearchHis.Visible = true;
                        panNext.Visible = true;
                    }
                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void btnSideBarLeft_Click(object sender, EventArgs e)
        {
            btnSideBarLeftClick();
        }

        private void btnSideBarLeftClick()
        {
            if (panAcp.Visible == false) return;
            panAcp.Visible = false;
            panSideBarLeft.Visible = true;
            panSplitV.Visible = false;
            ResizeSubForm();
        }

        private void lblSideBarLeft_Click(object sender, EventArgs e)
        {
            lblSideBarLeftClick();
        }

        private void lblSideBarLeftClick()
        {
            panAcp.Visible = true;
            panSideBarLeft.Visible = false;
            panSplitV.Visible = true;
        }

        private void panSplitV_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ResizeForm();
        }

        private void ResizeForm()
        {
            try
            {

                if (fEmrBaseAcpList != null)
                {
                    fEmrBaseAcpList.WindowState = FormWindowState.Normal;
                    fEmrBaseAcpList.Height = panViewEmrAcp.Height;
                    fEmrBaseAcpList.Width = panViewEmrAcp.Width;
                }
                Application.DoEvents();
                if (fEmrBaseContinuView != null)
                {
                    fEmrBaseContinuView.WindowState = FormWindowState.Normal;
                    fEmrBaseContinuView.Height = panContinueView.Height;
                    fEmrBaseContinuView.Width = panContinueView.Width;
                }
                if (ActiveFormView != null)
                {
                    ActiveFormView.WindowState = FormWindowState.Normal;
                    ActiveFormView.Height = panEmrViewMain.Height;
                    ActiveFormView.Width = panEmrViewMain.Width;
                }

                if (ActiveScanView != null)
                {
                    ActiveScanView.WindowState = FormWindowState.Maximized;
                    ActiveScanView.Height = panEmrViewMain.Height;
                    ActiveScanView.Width = panEmrViewMain.Width;
                }
                Application.DoEvents();
            }
            catch
            {

            }
        }

        private void frmEmrBaseChartView_Resize(object sender, EventArgs e)
        {
            ResizeForm();
        }

        private void btnViewChartOrHis_Click(object sender, EventArgs e)
        {
            if (btnViewChartOrHis.Text == "구분보기")
            {
                SetPanelView("구분보기");
            }
            else
            {
                SetPanelView("연속보기");
            }
        }

        private void SetPanelView(string strFlag)
        {
            if (strFlag == "연속보기")
            {
                panViewEmrChart.Visible = false;
                panContinueView.Visible = true;
                btnViewChartOrHis.Text = "구분보기";
                panNext.Visible = false;
                btnSearchHis.Visible = false;
            }
            else
            {
                panViewEmrChart.Visible = true;
                panContinueView.Visible = false;
                btnViewChartOrHis.Text = "연속보기";

                if (lstEmrNo.Count > 0)
                {
                    panNext.Visible = true;
                    btnSearchHis.Visible = true;
                }
            }
        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            if (SelectIndex == 0)
                return;

            SelectIndex -= 1;

            lblCount.Text = lblCount.Text = string.Format("{0} / {1}", (SelectIndex + 1), lstEmrNo.Count);
            if (fView.FmFORMTYPE == "2")
            {
                ViewChart(AcpEmr, fView, lstEmrNo[SelectIndex], "0", "E", fView.FmFORMNO.ToString(), "1", "");
            }
            else
            {
                ViewChart(AcpEmr, fView, lstEmrNo[SelectIndex], "0", "T", fView.FmFORMNO.ToString(), "1", "");
            }

        }

        private void BtnRight_Click(object sender, EventArgs e)
        {
            if (SelectIndex == lstEmrNo.Count - 1)
                return;

            SelectIndex += 1;

            lblCount.Text = lblCount.Text = string.Format("{0} / {1}", (SelectIndex + 1), lstEmrNo.Count);
            if (fView.FmFORMTYPE == "2")
            {
                ViewChart(AcpEmr, fView, lstEmrNo[SelectIndex], "0", "E", fView.FmFORMNO.ToString(), "1", "");
            }
            else
            {
                ViewChart(AcpEmr, fView, lstEmrNo[SelectIndex], "0", "T", fView.FmFORMNO.ToString(), "1", "");
            }

        }

        private void FrmEmrBaseChartView_FormClosing(object sender, FormClosingEventArgs e)
        {
            //내원내역
            if (fEmrBaseAcpList != null)
            {
                fEmrBaseAcpList.Dispose();
                fEmrBaseAcpList = null;
            }

            if (fEmrBaseContinuView != null)
            {
                fEmrBaseContinuView.Dispose();
                fEmrBaseContinuView = null;  //차트 연속보기
            }

            if (frmEmrBaseViewVitalandActingX != null)
            {
                frmEmrBaseViewVitalandActingX.Dispose();
                frmEmrBaseViewVitalandActingX = null;
            }

            if (fEmrVitalSignX != null)
            {
                fEmrVitalSignX.Dispose();
                fEmrVitalSignX = null;
            }

        }

        private void btnSearchHis_Click(object sender, EventArgs e)
        {
            ssTemp.Visible = !ssTemp.Visible;
        }

        private void ssTemp_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssTemp_Sheet1.RowCount == 0)
                return;

            ssTemp.Visible = false;
            ViewChart(AcpEmr, fView, ssTemp_Sheet1.Cells[e.Row, 4].Text.Trim(), "0", "T", fView.FmFORMNO.ToString(), "1", "");
        }

        private void btnViewVitalAndActing_Click(object sender, EventArgs e)
        {
            if (AcpEmr == null) return;

            if (frmEmrBaseViewVitalandActingX != null)
            {
                frmEmrBaseViewVitalandActingX.Dispose();
                frmEmrBaseViewVitalandActingX = null;
            }

            Screen screen = Screen.FromControl(this);

            frmEmrBaseViewVitalandActingX = new frmEmrBaseViewVitalandActing(AcpEmr.ptNo, AcpEmr.inOutCls, AcpEmr.medFrDate, AcpEmr.medDeptCd);
            frmEmrBaseViewVitalandActingX.StartPosition = FormStartPosition.Manual;
            frmEmrBaseViewVitalandActingX.Location = Screen.AllScreens.Where(d => d.Primary == false).FirstOrDefault().Bounds.Location;
            frmEmrBaseViewVitalandActingX.WindowState = FormWindowState.Maximized;
            frmEmrBaseViewVitalandActingX.FormClosed += FrmEmrBaseViewVitalandActingX_FormClosed;
            frmEmrBaseViewVitalandActingX.Show();
        }

        private void FrmEmrBaseViewVitalandActingX_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrBaseViewVitalandActingX != null)
            {
                frmEmrBaseViewVitalandActingX.Dispose();
                frmEmrBaseViewVitalandActingX = null;
            }
        }

        private void btnDetail1_Click(object sender, EventArgs e)
        {
            if (frmVital != null)
            {
                frmVital.Dispose();
                frmVital = null;
            }

            frmVital = new FrmVital_D(mPTNO, AcpEmr);
            frmVital.StartPosition = FormStartPosition.CenterParent;
            //frmVital.rEventClosed += FrmVital_rEventClosed;
            frmVital.Show();
        }

        private void btnDetail2_Click(object sender, EventArgs e)
        {
            if (AcpEmr == null)
                return;

            using (Form frm = new frmNrIONew2(AcpEmr))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        private void btnViewRecord_Click(object sender, EventArgs e)
        {
            if (AcpEmr == null)
            {
                ComFunc.MsgBoxEx(this, "내원 내역 선택 후 눌러주세요.");
                return;
            }

            if (fEmrVitalSignX != null)
            {
                fEmrVitalSignX.Dispose();
                fEmrVitalSignX = null;
            }

            Screen screen = Screen.FromControl(this);

            fEmrVitalSignX = new frmEmrVitalSign("3150", clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, 3150).ToString(), AcpEmr, "0", "V", mEmrCallForm);
            fEmrVitalSignX.StartPosition = FormStartPosition.Manual;
            fEmrVitalSignX.Location = Screen.AllScreens.Where(d => d.Primary == false).FirstOrDefault().Bounds.Location;
            fEmrVitalSignX.WindowState = FormWindowState.Maximized;
            fEmrVitalSignX.FormClosed += FEmrVitalSignX_FormClosed;
            fEmrVitalSignX.Text = string.Format("성명: {0} / 등록번호: {1}", AcpEmr.ptName, AcpEmr.ptNo);
            fEmrVitalSignX.Show();

            if (fEmrVitalSignX.Controls.Find("mbtnExit", true).Length > 0)
            {
                fEmrVitalSignX.Controls.Find("mbtnExit", true)[0].Visible = true;
            }
        }

        private void FEmrVitalSignX_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fEmrVitalSignX != null)
            {
                fEmrVitalSignX.Dispose();
                fEmrVitalSignX = null;
            }
        }
    }
}
