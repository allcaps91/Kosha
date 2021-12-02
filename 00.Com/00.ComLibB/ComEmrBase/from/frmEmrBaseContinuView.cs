using ComBase;
using Microsoft.Win32;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrBaseContinuView : Form, MainFormMessage, FormEmrMessage
    {
        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public string mstrFormNo = "497";  //499 입원
        public string mstrUpdateNo = "0";
        public string mstrFormText = "Progress Note";
        public string mstrControl = "I0000014754";
        public string mstrEmrNo = "0";
        public string mstrMode = "W";
        public MaskedTextBox mMaskBox = null;
        #endregion

        #region //폼에서 사용하는 변수

        EmrPatient AcpEmr = null;
        EmrPatient pView = null;
        
        EmrForm fView = null;
        /// <summary>
        /// 출력용
        /// </summary>
        EmrForm pForm = null;

        string mPTNO = string.Empty;
        string mINOUTCLS = string.Empty;
        string mstrUserDept = string.Empty;

        Form ActiveFormView = null;
        EmrChartForm ActiveFormViewChart = null;

        /// <summary>
        /// 검색용 함수
        /// </summary>
        List<Control> lstTextBox = null;

        /// <summary>
        /// //중요차트 등록됨
        /// </summary>
        const string IMPORT_SAVED = "★★★";
        /// <summary>
        /// //중요차트 해제됨
        /// </summary>
        const string IMPORT_UNSAVED = "☆중요";

        /// <summary>
        /// //중요차트 등록됨
        /// </summary>
        const string REMARK_SAVED = "♣♣♣";
        /// <summary>
        /// //중요차트 해제됨
        /// </summary>
        const string REMARK_UNSAVED = "♧주석";

        /// <summary>
        ///   패널 높이
        /// </summary>
        private long PanelHeight;
        /// <summary>
        /// 패널 안의 차트 높이
        /// </summary>
        private long ChartHeight;

        /// <summary>
        ///  차트전체 데이터
        /// </summary>
        DataTable ChartDataList = null;

        /// <summary>
        /// 중요차트 조회여부
        /// </summary>
        bool isImport = false;

        ///// <summary>
        ///// 차트복사 과
        ///// </summary>
        //string DeptCode = string.Empty;

        ///// <summary>
        ///// 차트복사 신청 번호
        ///// </summary>
        //string CopyReqNo = string.Empty;

        #endregion //폼에서 사용하는 변수

        #region //서브폼선언
        frmEmrBaseChartDrawing fEmrBaseChartDrawing = null;
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
            if(mEmrCallForm != null)
            {
                mEmrCallForm.MsgSave("0");
            }


            SetContinuView();
            CloseWriteForm();
        }
        public void MsgDelete()
        {
            if(mEmrCallForm != null)
            {
                mEmrCallForm.MsgDelete();
            }

            SetContinuView();
            CloseWriteForm();
        }
        public void MsgClear()
        {
            //ComFunc.MsgBoxEx(this, "MsgClear");
        }
        public void MsgPrint()
        {

        }
        #endregion

        #region //이벤트 전달
        //폼이 Close될 경우
        //public delegate void EventClosed();
        //public event EventClosed rEventClosed;

        //차트정보
        //public delegate void SetEmrChart(string strFormNo, string strUpdateNo, EmrPatientNew po, string strEmrNo, string strMode, bool bolClick);
        //public event SetEmrChart rSetEmrChart;

        /// <summary>
        /// 연속보기 수정 화면에서 기록지 내용이 변경되었는지 확인한다.
        /// </summary>
        /// <returns></returns>
        public string CheckChartChangeData()
        {
            string rtnVal = string.Empty;
            if(ActiveFormView != null && ActiveFormView is frmEmrChartNew)
            {
                rtnVal = ((frmEmrChartNew)ActiveFormView).CheckChartChangeData();
            }

            return rtnVal;
        }

        /// <summary>
        /// 연속보기 수정 화면에서 기록 저장
        /// </summary>
        /// <returns></returns>
        public double SaveData()
        {
            double rtnVal = 0;
            if (ActiveFormView != null && ActiveFormView is frmEmrChartNew)
            {
                rtnVal = ((frmEmrChartNew)ActiveFormView).SaveData("0", true);
            }

            return rtnVal;
        }

        public void SubFormClear()
        {
            if(fEmrBaseChartDrawing != null)
            {
                fEmrBaseChartDrawing.Dispose();
                fEmrBaseChartDrawing = null;
            }

            CloseWriteForm();
        }

        public void ClearForm()
        {
            AcpEmr = null;
            pView = null;            
            fView = null;
            mPTNO = string.Empty;
            mINOUTCLS = string.Empty;

            pClearForm();
        }

        //public void SetEmrAcpInfo(EmrPatient pAcpEmr)
        //{
        //    AcpEmr = pAcpEmr;
        //    SetEmrInfo("0", "0");
        //}

        public void GetContinuView(string pPTNO, string pINOUTCLS)
        {
            mPTNO = pPTNO;
            mINOUTCLS = pINOUTCLS;

            PanelHeight = pnlChart.Height;
            SetDtpOption();
            SetContinuView();
        }

        #endregion

        #region //옵션버튼

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAll.Checked == true)
            {
                chkOrd.Checked = true;
                chkDis.Checked = true;
                chkChart0.Checked = true;
                chkChart1.Checked = true;
                chkChart2.Checked = true;
                chkChart3.Checked = true;
                chkChart4.Checked = true;
                chkChart5.Checked = true;

                using (RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("EmrSetting"))
                {
                    reg.SetValue("CHKRECORDALL", chkAll.Checked ? "1" : "0");
                }
            }
        }

        private void chkOrd_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrd.Checked == false)
            {
                chkAll.Checked = false;
            }
        }

        private void chkDis_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDis.Checked == false)
            {
                chkAll.Checked = false;
            }
        }

        private void chkChart1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkChart1.Checked == false)
            {
                chkAll.Checked = false;
            }
        }

        private void chkChart0_CheckedChanged(object sender, EventArgs e)
        {
            using (RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("EmrSetting"))
            {
                reg.SetValue("CHKRECORDCHO", chkChart0.Checked ? "1" : "0");
            }

            if (chkChart0.Checked == false)
            {
                chkAll.Checked = false;
            }
        }

        private void chkChart2_CheckedChanged(object sender, EventArgs e)
        {
            using (RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("EmrSetting"))
            {
                reg.SetValue("CHKRECORDIPD", chkChart2.Checked ? "1" : "0");
            }

            if (chkChart2.Checked == false)
            {
                chkAll.Checked = false;
            }
        }

        private void chkChart3_CheckedChanged(object sender, EventArgs e)
        {
            using (RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("EmrSetting"))
            {
                reg.SetValue("CHKRECORDOUT", chkChart3.Checked ? "1" : "0");
            }
            if (chkChart3.Checked == false)
            {
                chkAll.Checked = false;
            }
        }

        private void chkChart4_CheckedChanged(object sender, EventArgs e)
        {
            using (RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("EmrSetting"))
            {
                reg.SetValue("CHKRECORDOP", chkChart4.Checked ? "1" : "0");
            }

            if (chkChart4.Checked == false)
            {
                chkAll.Checked = false;
            }
        }

        private void chkChart5_CheckedChanged(object sender, EventArgs e)
        {
            using (RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("EmrSetting"))
            {
                reg.SetValue("CHKRECORDER", chkChart5.Checked ? "1" : "0");
            }

            if (chkChart5.Checked == false)
            {
                chkAll.Checked = false;
            }
        }

        private void optInOut3_CheckedChanged(object sender, EventArgs e)
        {
            if (clsType.User.BuseCode.Equals("044201") || clsType.User.BuseCode.Equals("077402"))
                return;


            using (RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("EmrSetting"))
            {
                reg.SetValue("OPTKIND", "0");
            }

            //if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "OOORD", "OPTKIND", "0") == false)
            //{

            //}
        }

        private void optInOut1_CheckedChanged(object sender, EventArgs e)
        {
            if (clsType.User.BuseCode.Equals("044201") || clsType.User.BuseCode.Equals("077402"))
                return;
            using (RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("EmrSetting"))
            {
                reg.SetValue("OPTKIND", "1");
            }

            //if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "OOORD", "OPTKIND", "1") == false)
            //{

            //}
        }

        private void optInOut2_CheckedChanged(object sender, EventArgs e)
        {
            if (clsType.User.BuseCode.Equals("044201") || clsType.User.BuseCode.Equals("077402"))
                return;

            using (RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("EmrSetting"))
            {
                reg.SetValue("OPTKIND", "2");
            }

            //if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "OOORD", "OPTKIND", "2") == false)
            //{

            //}
        }
        #endregion

        #region 생성자

        public frmEmrBaseContinuView()
        {
            InitializeComponent();
        }

        public frmEmrBaseContinuView(FormEmrMessage formMessage)
        {
            mEmrCallForm = formMessage;
            InitializeComponent();

        }

        /// <summary>
        /// 경과기록지 보는용도
        /// </summary>
        /// <param name="emrPatient"></param>
        /// <param name="strDeptCode"></param>
        /// <param name="reqNo"></param>
        public frmEmrBaseContinuView(EmrPatient emrPatient, EmrForm emrForm)
        {
            AcpEmr = emrPatient;
            pForm = emrForm;
            InitializeComponent();
        }
        #endregion

        private void frmEmrBaseContinuView_Load(object sender, EventArgs e)
        {
            ssProgHis_Sheet1.ColumnHeader.Visible = false;
            ssProgHis_Sheet1.RowHeader.Visible = false;

            panContinuView.Dock = DockStyle.Fill;

            panContinuViewAll.Dock = DockStyle.Fill;
            panContinuViewImport.Dock = DockStyle.Fill;
           
            panChartOne.Dock = DockStyle.Fill;
            panChartOne.Visible = false;

            //chkAll.Checked = true;
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

            SetDtpOption();
            //SetReCordOption();
            //dtpEDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
            //dtpSDate.Value = VB.DateAdd("m", -6, dtpEDate.Value);

            panImageConv.Width = 600;
            panImageConv.Height = 700;

            ssProgHis_Sheet1.RowCount = 0;
            ssProgImport_Sheet1.RowCount = 0;
            optViewChartAll.Checked = true;

            if(clsType.User.DeptCode.Equals("ER"))
            {
                //ER과 일경우 응급실 자동 체크;
                chkChart5.Checked = true;
            }

            if (clsType.User.DeptCode.Equals("NS") || clsType.User.DeptCode.Equals("GS") || clsType.User.DeptCode.Equals("OS"))
            {
                //수술 많은 과 일경우 자동 수술체크
                chkChart4.Checked = true;
            }

            chkSortAs2.Checked = clsEmrPublic.bOrderAsc;

            #region 2020-01-04 전체, 입원, 외래 컴퓨터 별로 가져오게 수정(데이터 없을시 DB에서 읽어오게)
            RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("EmrSetting");
            string strOptKind = reg.GetValue("OPTKIND", string.Empty).ToString();
            if (string.IsNullOrWhiteSpace(strOptKind))
            {
                strOptKind = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "OPTKIND");
            }

            if (strOptKind == "1")
            {
                optInOut1.Checked = true;
            }
            else if (strOptKind == "2")
            {
                optInOut2.Checked = true;
            }
            else
            {
                optInOut3.Checked = true;
            }

            string rtnVal = reg.GetValue("CHKDEPTALL", string.Empty).ToString();
            if (rtnVal == "1")
            {
                chkDeptAll.Checked = true;
            }

            rtnVal = reg.GetValue("CHKRECORDCHO", string.Empty).ToString();
            if (rtnVal == "1")
            {
                chkChart0.Checked = true;
            }

            rtnVal = reg.GetValue("CHKRECORDIPD", string.Empty).ToString();
            if (rtnVal == "1")
            {
                chkChart2.Checked = true;
            }

            rtnVal = reg.GetValue("CHKRECORDOUT", string.Empty).ToString();
            if (rtnVal == "1")
            {
                chkChart3.Checked = true;
            }

            rtnVal = reg.GetValue("CHKRECORDOP", string.Empty).ToString();
            if (rtnVal == "1")
            {
                chkChart4.Checked = true;
            }

            rtnVal = reg.GetValue("CHKRECORDER", string.Empty).ToString();
            if (rtnVal == "1")
            {
                chkChart5.Checked = true;
            }

            rtnVal = reg.GetValue("CHKRECORDALL", string.Empty).ToString();
            if (rtnVal == "1")
            {
                chkAll.Checked = true;
            }
            #endregion


            PanelHeight = pnlChart.Height;

            #region 과 등록
            GetUserDept();

            if (clsType.User.IdNumber == "19094")
            {
                MakeUserDept_19094();
            }
            #endregion

            //SetEmrInfo("0", "0");

            //  연속보기 마우스 휠 이벤트
            pnlChart.MouseWheel += PanContinuViewAll_MouseWheel;
            pnlChart.Scroll += PanContinuViewAll_Scroll;

            //경과기록지등으로 들어왔을때.
            if (AcpEmr != null)
            {
                dtpSSDATE.Value = Convert.ToDateTime(clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, AcpEmr.inOutCls, AcpEmr.medDeptCd, clsType.User.IdNumber,
                        DateTime.ParseExact(AcpEmr.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd")));
                dtpEEDATE.Value = Convert.ToDateTime(AcpEmr.medEndDate.Length == 0 ? ComQuery.CurrentDateTime(clsDB.DbCon, "S") : ComFunc.FormatStrToDate(AcpEmr.medEndDate, "D"));

                #region 환자 내원날짜
                //dtpSSDATE.Value = Convert.ToDateTime(DateTime.ParseExact(AcpEmr.medFrDate, "yyyyMMdd", null));
                //if (AcpEmr.medEndDate.Length > 0)
                //{
                //    dtpEEDATE.Value = Convert.ToDateTime(DateTime.ParseExact(AcpEmr.medEndDate, "yyyyMMdd", null));
                //}
                //else
                //{
                //    dtpEEDATE.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                //}
                #endregion

                #region 조회 패널 사이즈 조정
                mbtnHisSeek.Height = panDate.Height;
                mbtnHisSeek.Left = panDate.Left + panDate.Width + 5;
                panSearch.Height = panDate.Height + 5;
                panel1.Height = panSearch.Height + panel9.Height + 5;
                #endregion

                btnImport.Visible = false;
                btnPrint.Visible = clsType.User.AuAPRINTIN.Equals("1");
                btnSearchContent.Visible = clsType.User.AuAPRINTIN.Equals("1");

                #region 패널 숨김
                lblCount.Visible = true;
                chkSortAs2.Visible = true;

                panDefalut.Visible = false;
                panAll.Visible = false;
                panAcpInList.Visible = false;
                panDate.Visible = true;
                panel6.Visible = true;
                chkSortAs.Visible = true;
                ssProgHis_Sheet1.RowCount = 0;
                ssProgImport_Sheet1.RowCount = 0;
                ssAipList_Sheet1.RowCount = 0;
                #endregion
            }

        }

        /// <summary>
        /// EMR 날짜 옵션 가져오는 함수
        /// </summary>
        private void SetDtpOption()
        {
            string strDTPEDATE = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            string strDTPSDate = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "DTPSDATE");
            if (string.IsNullOrEmpty(strDTPSDate))
            {
                strDTPSDate = (VB.DateAdd("m", -3, strDTPEDATE)).ToString("yyyy-MM-dd");
            }

            dtpSDate.Value = Convert.ToDateTime(strDTPSDate);
            dtpEDate.Value = Convert.ToDateTime(strDTPEDATE);
        }

        /// <summary>
        /// 기록지 옵션 가져오는 함수.
        /// </summary>
        private void SetReCordOption()
        {
            // 【연속보기 - 기록지 설정 초진】
            string strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "CONTINUVIEW_FIRST");
            if (VB.Val(strEmrOption) == 1 || string.IsNullOrEmpty(strEmrOption))
            {
                chkChart0.Checked = true;
            }
            else
            {
                chkChart0.Checked = false;
            }

            // 【연속보기 - 기록지 설정 입원】
            strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "CONTINUVIEW_ADMISSION");
            if (VB.Val(strEmrOption) == 1 || string.IsNullOrEmpty(strEmrOption))
            {
                chkChart2.Checked = true;
            }
            else
            {
                chkChart2.Checked = false;
            }

            // 【연속보기 - 기록지 설정 퇴원】
            strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "CONTINUVIEW_DISCHARGE");
            if (VB.Val(strEmrOption) == 1 || string.IsNullOrEmpty(strEmrOption))
            {
                chkChart3.Checked = true;
            }
            else
            {
                chkChart3.Checked = false;
            }

            // 【연속보기 - 기록지 설정 수술】
            strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "CONTINUVIEW_OPERATION");
            if (VB.Val(strEmrOption) == 1)
            {
                chkChart4.Checked = true;
            }
            else
            {
                chkChart4.Checked = false;
            }

            // 【연속보기 - 기록지 설정 응급실】
            strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "CONTINUVIEW_ER");
            if (VB.Val(strEmrOption) == 1)
            {
                chkChart5.Checked = true;
            }
            else
            {
                chkChart5.Checked = false;
            }


            // 【연속보기 - 기록지 설정 전체】 맨마지막에 함 이게 체크 되면 자동으로 다 체크 되기 때문.
            strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "CONTINUVIEW_ALL");
            if (VB.Val(strEmrOption) == 1)
            {
                chkAll.Checked = true;
            }
            else
            {
                chkAll.Checked = false;
            }
        }

        /// <summary>
        /// 스크롤바 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanContinuViewAll_Scroll(object sender, ScrollEventArgs e)
        {
            ChartAddCheck();
        }

        /// <summary>
        /// 연속보기 패널 마우스 휠 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanContinuViewAll_MouseWheel(object sender, MouseEventArgs e)
        {
            ChartAddCheck();
        }

        /// <summary>
        /// 차트 추가체크
        /// </summary>
        private void ChartAddCheck()
        {
            //  패널높이 - 차트높이 = 스크롤 최대값
            long maxValue = PanelHeight - ChartHeight;

            //if ((maxValue + 300) >= pnlChart.AutoScrollPosition.Y ||
            //    pnlChart.AutoScrollPosition.Y == 0 ||
            //    pnlChart.AutoScrollPosition.IsEmpty)
            //{
            //    ChartAdd();
            //}

            if ((maxValue + 300) >= pnlChart.AutoScrollPosition.Y)
            {
                ChartAdd();
            }

            SetChartRemark();
        }

        public void SetChartRemark()
        {
            //return;

            foreach (ucEmrChart item in EmrChartList)
            {
                item.SetRemarkChart();
            }
        }

        /// <summary>
        /// 텍스트 박스 내용 드래그시 내려가게
        /// </summary>
        private void ChartTextControlMouseDrag(Point e, int LastY)
        {
            if (!pnlChart.VerticalScroll.Visible)
                return;


            int Val = 85;
            if(e.Y > LastY && pnlChart.VerticalScroll.Value + Val <= pnlChart.VerticalScroll.Maximum)
            {
                pnlChart.VerticalScroll.Value += Val;
            }
            //else if (e.Y > LastY && pnlChart.VerticalScroll.Value + Val >= pnlChart.VerticalScroll.Maximum)
            //{
            //    pnlChart.VerticalScroll.Value = pnlChart.VerticalScroll.Maximum;
            //}
            else if (LastY > e.Y && pnlChart.VerticalScroll.Value > Val && pnlChart.VerticalScroll.Value - Val > 0)
            {
                pnlChart.VerticalScroll.Value -= Val;
            }
            //if (LastY > e.Y && pnlChart.VerticalScroll.Value - Val <= 0 || LastY <= 0 || e.Y <= 0)
            //{
            //    Console.WriteLine("아래2 last Y: {0} e.Y: {1} 패널 스크롤 값 : {2}", LastY, e.Y, pnlChart.VerticalScroll.Value - Val);
            //    pnlChart.VerticalScroll.Value = 0;
            //}

            //ChartAddCheck();
        }

        /// <summary>
        /// 텍스트 박스 마우스 휠 이벤트 
        /// </summary>
        private void ChartTextControlMouseWheel(int delta)
        {
            //long maxValue = PanelHeight - ChartHeight;

            if(delta <= 0)
            {
                if (pnlChart.VerticalScroll.Value + 100 < pnlChart.VerticalScroll.Maximum)
                {
                    pnlChart.VerticalScroll.Value += 100;
                }
                //else
                //{
                //    pnlChart.VerticalScroll.Value -= 0;
                //}
            }
            else
            {
                if(pnlChart.VerticalScroll.Value - 100 <= 0)
                {
                    pnlChart.VerticalScroll.Value = 0;
                }
                else
                {
                    pnlChart.VerticalScroll.Value -= 100;
                }
            }
            ChartAddCheck();
        }

        private void pClearForm()
        {
            CloseWriteForm();

            ssProgHis_Sheet1.RowCount = 0;
            mstrEmrNo = "0";
            mstrMode = string.Empty;

            ssAipList_Sheet1.RowCount = 0;
            ssProgImport_Sheet1.RowCount = 0;
            pnlChart.Controls.Clear();            
            panChartOne.Visible = false;
            //panLastDschDate.Visible = false;
            lblLastDschDate.Text = string.Empty;
            lblPatInfo.Text = string.Empty;
            btnImport.Text = "중요차트보기";
            optViewChartAll.Checked = true;

            //pClearNew();
            //p = clsEmrChartNew.ClearPatient();
        }

        private void mbtnHisSeek_Click(object sender, EventArgs e)
        {
            PanelHeight = pnlChart.Height;
            SetContinuView();

            if (clsType.User.BuseCode.Equals("044201"))
                return;

            if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "OOORD", "DTPSDATE", dtpSDate.Value.ToString("yyyy-MM-dd")) == false)
            {

            }
        }

        public void SetContinuView()
        {
            ChartHeight = 0;
            pnlChart.Controls.Clear();

            lblPatInfo.Text = string.Empty;
            GetPatInfo();


            //경과기록지용
            if (AcpEmr != null)
            {
                GetLapseChartList();

                int intDateCnt = (from DataRow dr in ChartDataList.Rows.AsParallel() group dr by dr["CHARTDATE"]).Count();
                lblCount.Text = string.Format("일수:{0}({1})", intDateCnt, ChartDataList.Rows.Count);
            }
            else
            {
                if (clsType.User.DrCode.Length == 0)
                    return;

                MakeUserDept();

                if (chkAll.Checked == false && string.IsNullOrEmpty(mstrUserDept))
                {
                    mstrUserDept = "'" + clsType.User.DeptCode + "'"; 
                    if(clsType.User.DeptCode.Length == 0)
                    {
                        chkAll.Checked = true;
                    }
                }
 
                if (optInOut2.Checked == true)
                {
                    ssProgHis_Sheet1.RowCount = 0;
                    ssProgImport_Sheet1.RowCount = 0;
                    ssAipList_Sheet1.RowCount = 0;
                    //panAcpInList.Visible = true;
                    //panLastDschDate.Visible = false;
                    lblLastDschDate.Text = string.Empty;               
                    GetChartList();
                }
                else
                {
                    ssProgHis_Sheet1.RowCount = 0;
                    ssProgImport_Sheet1.RowCount = 0;
                    ssAipList_Sheet1.RowCount = 0;
                    //panAcpInList.Visible = false;
                    //panLastDschDate.Visible = false;
                    lblLastDschDate.Text = string.Empty;
                    GetLastDsch();
                    GetChartList();
                }
            }

            EmrChartList = new List<ucEmrChart>();
            ChartAdd(true);

            SetChartRemark();
        }

        /// <summary>
        /// 환자정보 가져오기
        /// </summary>
        private void GetPatInfo()
        {
            string SQL = string.Empty;
            string sqlErr = string.Empty;
            OracleDataReader reader = null;

            lblPatInfo.Text = string.Empty;
            string strCurDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToShortDateString();

            try
            {
                SQL = " SELECT JUMIN1, SUBSTR(JUMIN2, 1, 1) JUMIN2, SEX, JUMIN3";
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
                    lblPatInfo.Text = string.Format("주민번호:     {0} - {1}  ({2}/{3})",
                        reader.GetValue(0).ToString().Trim(),
                        reader.GetValue(1).ToString().Trim(),
                    reader.GetValue(2).ToString().Trim().Equals("M") ? "남" :"여",
                    ComFunc.AgeCalcEx(
                        reader.GetValue(0).ToString().Trim() + clsAES.DeAES(reader.GetValue(3).ToString().Trim()), strCurDate)
                    );

                    lblPatInfo.Visible = true;
                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// 경과기록지 차트 리스트
        /// </summary>
        private void GetLapseChartList()
        {
            ChartDataList = null;
            StringBuilder SQL    = new StringBuilder();    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            string strSysDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            bool mViewNpChart = clsEmrQueryOld.ViewNPChart(clsType.User.Sabun);

            #region 이전 쿼리
            //SQL.AppendLine( "SELECT SORT");
            //SQL.AppendLine( "     , GB");
            //SQL.AppendLine( "     , EMRNO");
            //SQL.AppendLine( "     , INOUTCLS");
            //SQL.AppendLine( "     , CHARTDATE");
            //SQL.AppendLine( "     , CHARTTIME");
            //SQL.AppendLine( "     , WRITEDATE");
            //SQL.AppendLine( "     , WRITETIME");
            //SQL.AppendLine( "     , USEID");
            //SQL.AppendLine( "     , FORMNO");
            //SQL.AppendLine( "     , UPDATENO");
            //SQL.AppendLine( "     , OLDGB");
            //SQL.AppendLine( "     , FORMNAME");
            //SQL.AppendLine( "     , NAME");
            //SQL.AppendLine( "     , MEDDEPTCD");
            //SQL.AppendLine( "     , DEPTNAMEK");
            //SQL.AppendLine( "     , EMRNOHIS");
            //SQL.AppendLine( "  FROM ");
            //SQL.AppendLine( "  (");
            //SQL.AppendLine( "        --경과(재진/경과)");
            //SQL.AppendLine( "        SELECT 1 AS SORT");
            //SQL.AppendLine( "             , '경과' AS GB");
            //SQL.AppendLine( "             , C.EMRNO");
            //SQL.AppendLine( "             , C.INOUTCLS");
            //SQL.AppendLine( "             , C.CHARTDATE");
            //SQL.AppendLine( "             , C.CHARTTIME");
            //SQL.AppendLine( "             , C.WRITEDATE");
            //SQL.AppendLine( "             , C.WRITETIME");
            //if (pForm.FmOLDGB == 1)
            //{
            //    SQL.AppendLine("             , C.USEID");
            //}
            //else
            //{
            //    SQL.AppendLine("             , C.CHARTUSEID AS USEID");
            //}
            //SQL.AppendLine("             , C.MEDDEPTCD");
            //SQL.AppendLine( "             , D.DEPTNAMEK");
            //SQL.AppendLine( "             , F.FORMNO");
            //SQL.AppendLine( "             , F.UPDATENO");
            //SQL.AppendLine( "             , F.OLDGB");
            //SQL.AppendLine( "             , F.FORMNAME");
            //if (pForm.FmOLDGB == 1)
            //{
            //    SQL.AppendLine("     ,(SELECT NAME FROM KOSMOS_EMR.EMR_USERT WHERE USERID = LTRIM(C.USEID, '0')) AS NAME");
            //}
            //else
            //{
            //    SQL.AppendLine("     ,(SELECT NAME FROM KOSMOS_EMR.EMR_USERT WHERE USERID = LTRIM(C.CHARTUSEID, '0')) AS NAME");
            //}

            //SQL.AppendLine("             , 0 AS EMRNOHIS");


            //if (pForm.FmOLDGB == 1)
            //{
            //    SQL.AppendLine("FROM KOSMOS_EMR.EMRXMLMST C ");
            //}
            //else
            //{
            //    SQL.AppendLine("FROM KOSMOS_EMR.AEMRCHARTMST C ");
            //}

            //SQL.AppendLine( "          INNER JOIN KOSMOS_EMR.AEMRFORM F ");
            //SQL.AppendLine( "                  ON F.FORMNO       = C.FORMNO ");
            //SQL.AppendLine( "                 AND F.GRPFORMNO    = 1001 ");
            //SQL.AppendLine( "                 AND F.UPDATENO     = " + pForm.FmUPDATENO);
            //SQL.AppendLine( "          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
            //SQL.AppendLine( "                  ON C.MEDDEPTCD = D.DEPTCODE");
            //SQL.AppendLine( "         WHERE C.PTNO       = '" + AcpEmr.ptNo + "' ");
            //SQL.AppendLine( "           AND C.FORMNO  = " + pForm.FmFORMNO);

            //if (AcpEmr.inOutCls == "O")
            //{
            //    if (AcpEmr.medDeptCd == "RA" || (AcpEmr.medDeptCd == "MD" && (AcpEmr.medDrCd == "1107" || AcpEmr.medDrCd == "1125")))
            //    {
            //        SQL.AppendLine( "           AND C.MEDDEPTCD = 'MD'");
            //        SQL.AppendLine( "           AND C.MEDDRCD IN ('1107','1125')");
            //    }
            //    else
            //    {
            //        SQL.AppendLine( "           AND C.MEDDEPTCD = '" + AcpEmr.medDeptCd + "'");
            //        SQL.AppendLine( "           AND C.MEDDRCD NOT IN ('1107','1125')");
            //    }
            //}
            //else
            //{
            //    if (AcpEmr.medDeptCd == "ER")
            //    {
            //        SQL.AppendLine( "           AND C.INOUTCLS IN ('O') ");
            //    }
            //    else
            //    {
            //        SQL.AppendLine( "           AND C.INOUTCLS IN ('I') ");
            //    }
            //}

            //if (AcpEmr.inOutCls == "O" && AcpEmr.medDeptCd == "HD")
            //{
            //    SQL.AppendLine( "           AND C.INOUTCLS IN ('O','I') ");
            //}
            //else if (AcpEmr.inOutCls == "O" && AcpEmr.medDeptCd != "HD")
            //{
            //    SQL.AppendLine( "           AND C.INOUTCLS IN ('O') ");
            //}

            //if (mViewNpChart == false)
            //{
            //    SQL.AppendLine( "           AND C.MEDDEPTCD <> 'NP'");
            //}

            //SQL.AppendLine( "           AND C.MEDFRDATE = '" + AcpEmr.medFrDate + "' ");
            //SQL.AppendLine( "           AND C.CHARTDATE >= '" + dtpSSDATE.Value.ToString("yyyyMMdd") + "' ");
            //SQL.AppendLine( "           AND C.CHARTDATE <= '" + dtpEEDATE.Value.ToString("yyyyMMdd") + "' ");

            //if (isImport == true)
            //{
            //    SQL.AppendLine( "                  AND ( ");
            //    SQL.AppendLine( "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP");
            //    SQL.AppendLine( "                                 WHERE IMP.EMRNO = C.EMRNO");
            //    SQL.AppendLine( "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )");
            //    SQL.AppendLine( "                         OR");
            //    SQL.AppendLine( "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP");
            //    SQL.AppendLine( "                                 WHERE IMP.EMRNO = C.EMRNO");
            //    //SQL.AppendLine( "                                 AND IMP.EMRNOHIS = C.EMRNOHIS");
            //    SQL.AppendLine( "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )");
            //    SQL.AppendLine( "                  ) ");
            //}

            //SQL.AppendLine( "    )");


            //if (chkSortAs2.Checked)
            //{
            //    SQL.AppendLine( "ORDER BY (CHARTDATE || CHARTTIME), INOUTCLS ASC, SORT ");
            //}
            //else
            //{
            //    SQL.AppendLine( "ORDER BY (CHARTDATE || CHARTTIME) DESC, INOUTCLS ASC, SORT ");
            //}
            #endregion

            #region XML, 신규 서식지 UNION
            SQL.AppendLine("SELECT SORT");
            SQL.AppendLine("     , GB");
            SQL.AppendLine("     , EMRNO");
            SQL.AppendLine("     , INOUTCLS");
            SQL.AppendLine("     , CHARTDATE");
            SQL.AppendLine("     , CHARTTIME");
            SQL.AppendLine("     , WRITEDATE");
            SQL.AppendLine("     , WRITETIME");
            SQL.AppendLine("     , USEID");
            SQL.AppendLine("     , FORMNO");
            SQL.AppendLine("     , UPDATENO");
            SQL.AppendLine("     , OLDGB");
            SQL.AppendLine("     , FORMNAME");
            SQL.AppendLine("     , NAME");
            SQL.AppendLine("     , MEDDEPTCD");
            SQL.AppendLine("     , DEPTNAMEK");
            SQL.AppendLine("     , EMRNOHIS");
            SQL.AppendLine("  FROM ");
            SQL.AppendLine("  (");
            #region XML
            SQL.AppendLine("        --경과(재진/경과)");
            SQL.AppendLine("        SELECT 1 AS SORT");
            SQL.AppendLine("             , '경과' AS GB");
            SQL.AppendLine("             , C.EMRNO");
            SQL.AppendLine("             , C.INOUTCLS");
            SQL.AppendLine("             , C.CHARTDATE");
            SQL.AppendLine("             , C.CHARTTIME");
            SQL.AppendLine("             , C.WRITEDATE");
            SQL.AppendLine("             , C.WRITETIME");
            SQL.AppendLine("             , C.USEID");
            SQL.AppendLine("             , C.MEDDEPTCD");
            SQL.AppendLine("             , D.DEPTNAMEK");
            SQL.AppendLine("             , F.FORMNO");
            SQL.AppendLine("             , F.UPDATENO");
            SQL.AppendLine("             , F.OLDGB");
            SQL.AppendLine("             , F.FORMNAME");
            SQL.AppendLine("             , (SELECT NAME FROM KOSMOS_EMR.EMR_USERT WHERE USERID = LTRIM(C.USEID, '0')) AS NAME");
            SQL.AppendLine("             , 0 AS EMRNOHIS");
            SQL.AppendLine("  FROM KOSMOS_EMR.EMRXMLMST C ");
            SQL.AppendLine("    INNER JOIN KOSMOS_EMR.AEMRFORM F ");
            SQL.AppendLine("       ON F.FORMNO       = C.FORMNO ");
            SQL.AppendLine("      AND F.GRPFORMNO    = 1001 ");
            SQL.AppendLine("      AND F.OLDGB = '1'");
            SQL.AppendLine("    INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
            SQL.AppendLine("       ON C.MEDDEPTCD = D.DEPTCODE");
            SQL.AppendLine(" WHERE C.PTNO    = '" + AcpEmr.ptNo + "' ");
            SQL.AppendLine("   AND C.FORMNO  = " + pForm.FmFORMNO);

            if (AcpEmr.inOutCls == "O")
            {
                if (AcpEmr.medDeptCd == "RA" || (AcpEmr.medDeptCd == "MD" && (AcpEmr.medDrCd == "1107" || AcpEmr.medDrCd == "1125")))
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD = 'MD'");
                    SQL.AppendLine("           AND C.MEDDRCD IN ('1107','1125')");
                }
                else
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD = '" + AcpEmr.medDeptCd + "'");
                    SQL.AppendLine("           AND C.MEDDRCD NOT IN ('1107','1125')");
                }
            }
            else
            {
                if (AcpEmr.medDeptCd == "ER")
                {
                    SQL.AppendLine("           AND C.INOUTCLS IN ('O') ");
                }
                else
                {
                    SQL.AppendLine("           AND C.INOUTCLS IN ('I') ");
                }
            }

            if (AcpEmr.inOutCls == "O" && AcpEmr.medDeptCd == "HD")
            {
                SQL.AppendLine("           AND C.INOUTCLS IN ('O','I') ");
            }
            else if (AcpEmr.inOutCls == "O" && AcpEmr.medDeptCd != "HD")
            {
                SQL.AppendLine("           AND C.INOUTCLS IN ('O') ");
            }

            if (mViewNpChart == false)
            {
                SQL.AppendLine("           AND C.MEDDEPTCD <> 'NP'");
            }

            SQL.AppendLine("           AND C.MEDFRDATE = '" + AcpEmr.medFrDate + "' ");
            SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSSDATE.Value.ToString("yyyyMMdd") + "' ");
            SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEEDATE.Value.ToString("yyyyMMdd") + "' ");

            if (isImport == true)
            {
                SQL.AppendLine("                  AND ( ");
                SQL.AppendLine("                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP");
                SQL.AppendLine("                                 WHERE IMP.EMRNO = C.EMRNO");
                if (clsType.User.DeptCode == "MN")
                {
                    SQL.AppendLine("                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))");
                }
                else
                {
                    SQL.AppendLine("                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )");
                }
                
                SQL.AppendLine("                         OR");
                SQL.AppendLine("                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP");
                SQL.AppendLine("                                 WHERE IMP.EMRNO = C.EMRNO");
                //SQL.AppendLine( "                                 AND IMP.EMRNOHIS = C.EMRNOHIS");
                SQL.AppendLine("                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )");
                SQL.AppendLine("                  ) ");
            }
            #endregion

            #region 신규기록지
            SQL.AppendLine("        UNION ALL");
            SQL.AppendLine("        SELECT 1 AS SORT");
            SQL.AppendLine("             , '경과' AS GB");
            SQL.AppendLine("             , C.EMRNO");
            SQL.AppendLine("             , C.INOUTCLS");
            SQL.AppendLine("             , C.CHARTDATE");
            SQL.AppendLine("             , C.CHARTTIME");
            SQL.AppendLine("             , C.WRITEDATE");
            SQL.AppendLine("             , C.WRITETIME");
            SQL.AppendLine("             , C.CHARTUSEID");
            SQL.AppendLine("             , C.MEDDEPTCD");
            SQL.AppendLine("             , D.DEPTNAMEK");
            SQL.AppendLine("             , F.FORMNO");
            SQL.AppendLine("             , F.UPDATENO");
            SQL.AppendLine("             , F.OLDGB");
            SQL.AppendLine("             , F.FORMNAME");
            SQL.AppendLine("             , (SELECT NAME FROM KOSMOS_EMR.EMR_USERT WHERE USERID = C.CHARTUSEID) AS NAME");
            SQL.AppendLine("             , 0 AS EMRNOHIS");
            SQL.AppendLine("  FROM KOSMOS_EMR.AEMRCHARTMST C ");
            SQL.AppendLine("    INNER JOIN KOSMOS_EMR.AEMRFORM F ");
            SQL.AppendLine("       ON F.FORMNO       = C.FORMNO ");
            SQL.AppendLine("      AND F.GRPFORMNO    = 1001 ");
            SQL.AppendLine("      AND F.UPDATENO     = " + pForm.FmUPDATENO);
            SQL.AppendLine("    INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
            SQL.AppendLine("       ON C.MEDDEPTCD = D.DEPTCODE");
            SQL.AppendLine(" WHERE C.PTNO    = '" + AcpEmr.ptNo + "' ");
            SQL.AppendLine("   AND C.FORMNO  = " + pForm.FmFORMNO);

            if (AcpEmr.inOutCls == "O")
            {
                if (AcpEmr.medDeptCd == "RA" || (AcpEmr.medDeptCd == "MD" && (AcpEmr.medDrCd == "1107" || AcpEmr.medDrCd == "1125")))
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD = 'MD'");
                    SQL.AppendLine("           AND C.MEDDRCD IN ('1107','1125')");
                }
                else
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD = '" + AcpEmr.medDeptCd + "'");
                    SQL.AppendLine("           AND C.MEDDRCD NOT IN ('1107','1125')");
                }
            }
            else
            {
                if (AcpEmr.medDeptCd == "ER")
                {
                    SQL.AppendLine("           AND C.INOUTCLS IN ('O') ");
                }
                else
                {
                    SQL.AppendLine("           AND C.INOUTCLS IN ('I') ");
                }
            }

            if (AcpEmr.inOutCls == "O" && AcpEmr.medDeptCd == "HD")
            {
                SQL.AppendLine("           AND C.INOUTCLS IN ('O','I') ");
            }
            else if (AcpEmr.inOutCls == "O" && AcpEmr.medDeptCd != "HD")
            {
                SQL.AppendLine("           AND C.INOUTCLS IN ('O') ");
            }

            if (mViewNpChart == false)
            {
                SQL.AppendLine("           AND C.MEDDEPTCD <> 'NP'");
            }

            SQL.AppendLine("           AND C.MEDFRDATE = '" + AcpEmr.medFrDate + "' ");
            SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSSDATE.Value.ToString("yyyyMMdd") + "' ");
            SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEEDATE.Value.ToString("yyyyMMdd") + "' ");

            if (isImport == true)
            {
                SQL.AppendLine("                  AND ( ");
                SQL.AppendLine("                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP");
                SQL.AppendLine("                                 WHERE IMP.EMRNO = C.EMRNO");
                if (clsType.User.DeptCode == "MN")
                {
                    SQL.AppendLine("                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))");
                }
                else
                {
                    SQL.AppendLine("                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )");
                }
                //SQL.AppendLine("                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )");
                SQL.AppendLine("                         OR");
                SQL.AppendLine("                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP");
                SQL.AppendLine("                                 WHERE IMP.EMRNO = C.EMRNO");
                //SQL.AppendLine( "                                 AND IMP.EMRNOHIS = C.EMRNOHIS");
                SQL.AppendLine("                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )");
                SQL.AppendLine("                  ) ");
            }
            #endregion

            SQL.AppendLine("    )");


            if (chkSortAs2.Checked)
            {
                SQL.AppendLine("ORDER BY (CHARTDATE || CHARTTIME), INOUTCLS ASC, SORT ");
            }
            else
            {
                SQL.AppendLine("ORDER BY (CHARTDATE || CHARTTIME) DESC, INOUTCLS ASC, SORT ");
            }
            #endregion


            SqlErr = clsDB.GetDataTableREx(ref ChartDataList, SQL.ToString().Trim(), clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        /// <summary>
        /// 전체 차트 리스트
        /// </summary>
        private void GetChartList()
        {
            if (ParentForm != null && ParentForm.Name.Equals("frmEmrBaseChartView") && 
                ParentForm.ParentForm != null && ParentForm.ParentForm.Name.Equals("frmEmrViewMain") == false)
                return;

            ChartDataList = null;
            StringBuilder SQL = new StringBuilder();    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            string ptNo = mPTNO; /// "06792410";

            bool mViewNpChart = clsEmrQueryOld.ViewNPChart(clsType.User.Sabun);
            bool mViewFMChart = clsEmrQueryOld.ViewFMChart(clsType.User.Sabun);

            #region 쿼리

            SQL.AppendLine("SELECT SORT");
            SQL.AppendLine("     , GB");
            SQL.AppendLine("     , EMRNO");
            SQL.AppendLine("     , INOUTCLS");
            SQL.AppendLine("     , CHARTDATE");
            SQL.AppendLine("     , CHARTTIME");
            SQL.AppendLine("     , USEID");
            SQL.AppendLine("     , FORMNO");
            SQL.AppendLine("     , UPDATENO");
            SQL.AppendLine("     , OLDGB");
            SQL.AppendLine("     , FORMNAME");
            SQL.AppendLine("     , NAME");
            SQL.AppendLine("     , MEDDEPTCD");
            SQL.AppendLine("     , DEPTNAMEK");
            SQL.AppendLine("     , EMRNOHIS");
            SQL.AppendLine("  FROM ");
            SQL.AppendLine("  (");

            //*********************************//
            //*********************************//
            #region //Old EMR
            #region 경과기록지
            SQL.AppendLine("        --경과(재진/경과)");
            SQL.AppendLine("        SELECT 4 AS SORT");
            SQL.AppendLine("             , '경과' AS GB");
            SQL.AppendLine("             , C.EMRNO");
            SQL.AppendLine("             , C.INOUTCLS");
            SQL.AppendLine("             , C.CHARTDATE");
            SQL.AppendLine("             , C.CHARTTIME");
            SQL.AppendLine("             , C.USEID");
            SQL.AppendLine("             , C.MEDDEPTCD");
            SQL.AppendLine("             , D.DEPTNAMEK");
            SQL.AppendLine("             , F.FORMNO");
            SQL.AppendLine("             , F.UPDATENO");
            SQL.AppendLine("             , F.OLDGB");
            SQL.AppendLine("             , F.FORMNAME");
            SQL.AppendLine("             , U.NAME");
            SQL.AppendLine("             , 0 AS EMRNOHIS");
            SQL.AppendLine("          FROM KOSMOS_EMR.EMRXMLMST C ");
            SQL.AppendLine("          INNER JOIN KOSMOS_EMR.AEMRFORM F ");
            SQL.AppendLine("                  ON F.FORMNO       = C.FORMNO ");
            SQL.AppendLine("                 AND F.UPDATENO     = 1 ");
            SQL.AppendLine("                 AND F.GRPFORMNO    = 1001");
            SQL.AppendLine("          INNER JOIN KOSMOS_EMR.EMR_USERT U");
            SQL.AppendLine("                  ON C.USEID = U.USERID                 ");
            SQL.AppendLine("          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
            SQL.AppendLine("                  ON C.MEDDEPTCD = D.DEPTCODE");
            SQL.AppendLine("         WHERE C.PTNO       = '" + ptNo + "' ");
            //SQL.AppendLine("           AND C.INOUTCLS   = '" + AcpEmr.inOutCls + "' ");
            //SQL.AppendLine("           AND C.MEDDEPTCD  = '" + AcpEmr.medDeptCd + "' ");

            #region 경과기록지 처리 필요
            //if (AcpEmr.inOutCls == "O")
            //{
            //    if (AcpEmr.medDeptCd == "RA" || (AcpEmr.medDeptCd == "MD" && (AcpEmr.medDrCd == "1107" || AcpEmr.medDrCd == "1125")))
            //    {
            //        SQL.AppendLine("                AND C.MEDDEPTCD = 'MD'");
            //        SQL.AppendLine("                AND C.MEDDRCD IN ('1107','1125')");
            //    }
            //    else
            //    {
            //        SQL.AppendLine("                AND C.MEDDEPTCD = '" + AcpEmr.medDeptCd + "'");
            //        SQL.AppendLine("                AND C.MEDDRCD NOT IN ('1107','1125')");
            //    }
            //}
            //else
            //{
            //    SQL.AppendLine("                AND C.INOUTCLS IN ('I') ");
            //}

            //if (AcpEmr.inOutCls == "O" && AcpEmr.medDeptCd == "HD")
            //{
            //    SQL.AppendLine("                AND C.INOUTCLS IN ('O','I') ");
            //}
            //else if (AcpEmr.inOutCls == "O" && AcpEmr.medDeptCd != "HD")
            //{
            //    SQL.AppendLine("                AND C.INOUTCLS IN ('O') ");
            //}
            #endregion

            if (optInOut1.Checked)
            {
                SQL.AppendLine("           AND C.INOUTCLS = 'O'");
            }
            else if (optInOut2.Checked)
            {
                SQL.AppendLine("           AND C.INOUTCLS = 'I'");
            }


            if (chkDeptAll.Checked == false && chkUserOpt.Checked)
            {
                SQL.AppendLine("           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ");
            }

            if (mViewNpChart == false)
            {
                SQL.AppendLine("        AND C.MEDDEPTCD <> 'NP'");
            }

            if (mViewFMChart == false)
            {
                SQL.AppendLine("        AND C.MEDDEPTCD <> 'FM'");
            }

            SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ");
            SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ");
            #endregion 경과기록지

            #region //진단 : 쿼리 수정이 필요
            //if (chkAll.Checked || chkDis.Checked)
            //{
            //    SQL.AppendLine("        UNION ALL ");
            //    SQL.AppendLine("        --진단(외래는 일별 발생,입원은 __)");
            //    SQL.AppendLine("        --외래 등록번호, 내원일자, 진료과 로 진단을 뿌린다");
            //    SQL.AppendLine("        --입원 등록번호, 입원일자 로 한다");
            //    SQL.AppendLine("        SELECT 1 AS SORT");
            //    SQL.AppendLine("             , '진단' AS GB");
            //    SQL.AppendLine("             , C.EMRNO");
            //    SQL.AppendLine("             , C.INOUTCLS");
            //    SQL.AppendLine("             , C.CHARTDATE");
            //    SQL.AppendLine("             , C.CHARTTIME");
            //    SQL.AppendLine("             , C.USEID");
            //    SQL.AppendLine("             , C.MEDDEPTCD");
            //    SQL.AppendLine("             , D.DEPTNAMEK");
            //    SQL.AppendLine("             , F.FORMNO");
            //    SQL.AppendLine("             , F.UPDATENO");
            //    SQL.AppendLine("             , F.OLDGB");
            //    SQL.AppendLine("             , F.FORMNAME");
            //    SQL.AppendLine("             , U.NAME");
            //    SQL.AppendLine("             , 0 AS EMRNOHIS");
            //    SQL.AppendLine("          FROM KOSMOS_EMR.EMRXMLMST C ");
            //    SQL.AppendLine("         INNER JOIN KOSMOS_EMR.AEMRFORM F ");
            //    SQL.AppendLine("                 ON F.FORMNO    = C.FORMNO ");
            //    SQL.AppendLine("                AND F.UPDATENO  = 1 ");
            //    SQL.AppendLine("                AND F.GRPFORMNO = 1001");
            //    SQL.AppendLine("         INNER JOIN KOSMOS_EMR.EMR_USERT U");
            //    SQL.AppendLine("                 ON C.USEID = U.USERID");
            //    SQL.AppendLine("          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
            //    SQL.AppendLine("                  ON C.MEDDEPTCD = D.DEPTCODE");
            //    SQL.AppendLine("         WHERE C.PTNO       = '" + ptNo + "' ");
            //    SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ");
            //    SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ");
            //    if (isImport == true)
            //    {
            //        SQL.AppendLine("                  AND ( ");
            //        SQL.AppendLine("                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP");
            //        SQL.AppendLine("                                 WHERE IMP.EMRNO = C.EMRNO");
            //        SQL.AppendLine("                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )");
            //        SQL.AppendLine("                         OR");
            //        SQL.AppendLine("                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP");
            //        SQL.AppendLine("                                 WHERE IMP.EMRNO = C.EMRNO");
            //        //SQL.AppendLine("                                 AND IMP.EMRNOHIS = C.EMRNOHIS");
            //        SQL.AppendLine("                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )");
            //        SQL.AppendLine("                  ) ");
            //    }
            //}
            #endregion //진단

            #region //초진
            if (chkAll.Checked || chkChart0.Checked)
            {
                SQL.AppendLine("        UNION ALL ");
                SQL.AppendLine("	--초진기록지 ");
                SQL.AppendLine("        SELECT 3 AS SORT");
                SQL.AppendLine("             , '초진' AS GB");
                SQL.AppendLine("             , C.EMRNO");
                SQL.AppendLine("             , C.INOUTCLS");
                SQL.AppendLine("             , C.CHARTDATE");
                SQL.AppendLine("             , C.CHARTTIME");
                SQL.AppendLine("             , C.USEID");
                SQL.AppendLine("             , C.MEDDEPTCD");
                SQL.AppendLine("             , D.DEPTNAMEK");
                SQL.AppendLine("             , F.FORMNO");
                SQL.AppendLine("             , F.UPDATENO");
                SQL.AppendLine("             , F.OLDGB");
                SQL.AppendLine("             , F.FORMNAME");
                SQL.AppendLine("             , U.NAME");
                SQL.AppendLine("             , 0 AS EMRNOHIS");
                SQL.AppendLine("          FROM KOSMOS_EMR.EMRXMLMST C ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.AEMRFORM F ");
                SQL.AppendLine("                  ON C.FORMNO       = F.FORMNO ");
                SQL.AppendLine("                 AND F.UPDATENO     = 1 ");
                SQL.AppendLine("                 AND F.GRPFORMNO    = 1000");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.EMR_USERT U");
                SQL.AppendLine("                  ON C.USEID = U.USERID                 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
                SQL.AppendLine("                  ON C.MEDDEPTCD = D.DEPTCODE");
                SQL.AppendLine("         WHERE C.PTNO       = '" + ptNo + "' ");
                SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ");
                SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ");

                if (optInOut1.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'O'");
                }
                else if (optInOut2.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'I'");
                }

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ");
                }
            }
            #endregion //초진

            #region //입원
            if (chkAll.Checked || chkChart2.Checked)
            {
                SQL.AppendLine("        UNION ALL");
                SQL.AppendLine("        --입원기록지 ");
                SQL.AppendLine("        SELECT 3 AS SORT");
                SQL.AppendLine("             , '입원' AS GB");
                SQL.AppendLine("             , C.EMRNO");
                SQL.AppendLine("             , C.INOUTCLS");
                SQL.AppendLine("             , C.CHARTDATE");
                SQL.AppendLine("             , C.CHARTTIME");
                SQL.AppendLine("             , C.USEID");
                SQL.AppendLine("             , C.MEDDEPTCD");
                SQL.AppendLine("             , D.DEPTNAMEK");
                SQL.AppendLine("             , F.FORMNO");
                SQL.AppendLine("             , F.UPDATENO");
                SQL.AppendLine("             , F.OLDGB");
                SQL.AppendLine("             , F.FORMNAME");
                SQL.AppendLine("             , U.NAME");
                SQL.AppendLine("             , 0 AS EMRNOHIS");
                SQL.AppendLine("          FROM KOSMOS_EMR.EMRXMLMST C ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.AEMRFORM F ");
                SQL.AppendLine("                  ON F.FORMNO     = C.FORMNO ");
                SQL.AppendLine("                 AND F.UPDATENO   = 1 ");
                SQL.AppendLine("                 AND F.GRPFORMNO  = 1002");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.EMR_USERT U");
                SQL.AppendLine("                  ON C.USEID = U.USERID                 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
                SQL.AppendLine("                  ON C.MEDDEPTCD = D.DEPTCODE");
                SQL.AppendLine("         WHERE C.PTNO       = '" + ptNo + "' ");
                SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ");
                SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ");

                if (optInOut1.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'O'");
                }
                else if (optInOut2.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'I'");
                }

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ");
                }

           
            }
            #endregion //입원

            #region //퇴원, 전과
            if (chkAll.Checked || chkChart3.Checked)
            {
                SQL.AppendLine("        UNION ALL");
                SQL.AppendLine("        --퇴원요약지 ");
                SQL.AppendLine("        SELECT 3 AS SORT");
                SQL.AppendLine("             , '퇴원' AS GB");
                SQL.AppendLine("             , C.EMRNO");
                SQL.AppendLine("             , C.INOUTCLS");
                SQL.AppendLine("             , C.CHARTDATE");
                SQL.AppendLine("             , C.CHARTTIME");
                SQL.AppendLine("             , C.USEID");
                SQL.AppendLine("             , C.MEDDEPTCD");
                SQL.AppendLine("             , D.DEPTNAMEK");
                SQL.AppendLine("             , F.FORMNO");
                SQL.AppendLine("             , F.UPDATENO");
                SQL.AppendLine("             , F.OLDGB");
                SQL.AppendLine("             , F.FORMNAME");
                SQL.AppendLine("             , U.NAME");
                SQL.AppendLine("             , 0 AS EMRNOHIS");
                SQL.AppendLine("          FROM KOSMOS_EMR.EMRXMLMST C ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.AEMRFORM F ");
                SQL.AppendLine("                  ON F.FORMNO     = C.FORMNO ");
                SQL.AppendLine("                 AND F.UPDATENO   = 1 ");
                SQL.AppendLine("                 AND F.GRPFORMNO  = 1009");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.EMR_USERT U");
                SQL.AppendLine("                  ON C.USEID = U.USERID                 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
                SQL.AppendLine("                  ON C.MEDDEPTCD = D.DEPTCODE");
                SQL.AppendLine("         WHERE C.PTNO       = '" + ptNo + "' ");
                SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ");
                SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ");

                if (optInOut1.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'O'");
                }
                else if (optInOut2.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'I'");
                }

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ");
                }

                SQL.AppendLine("        UNION ALL");
                SQL.AppendLine("        --전과기록지 ");
                SQL.AppendLine("        SELECT 3 AS SORT");
                SQL.AppendLine("             , '전과' AS GB");
                SQL.AppendLine("             , C.EMRNO");
                SQL.AppendLine("             , C.INOUTCLS");
                SQL.AppendLine("             , C.CHARTDATE");
                SQL.AppendLine("             , C.CHARTTIME");
                SQL.AppendLine("             , C.USEID");
                SQL.AppendLine("             , C.MEDDEPTCD");
                SQL.AppendLine("             , D.DEPTNAMEK");
                SQL.AppendLine("             , F.FORMNO");
                SQL.AppendLine("             , F.UPDATENO");
                SQL.AppendLine("             , F.OLDGB");
                SQL.AppendLine("             , F.FORMNAME");
                SQL.AppendLine("             , U.NAME");
                SQL.AppendLine("             , 0 AS EMRNOHIS");
                SQL.AppendLine("          FROM KOSMOS_EMR.EMRXMLMST C ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.AEMRFORM F ");
                SQL.AppendLine("                  ON F.FORMNO     = C.FORMNO ");
                SQL.AppendLine("                 AND F.UPDATENO   = 1 ");
                SQL.AppendLine("                 AND F.GRPFORMNO  = 1010 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.EMR_USERT U");
                SQL.AppendLine("                  ON C.USEID = U.USERID                 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
                SQL.AppendLine("                  ON C.MEDDEPTCD = D.DEPTCODE");
                SQL.AppendLine("         WHERE C.PTNO       = '" + ptNo + "' ");
                SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ");
                SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ");

                if (optInOut1.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'O'");
                }
                else if (optInOut2.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'I'");
                }

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ");
                }


            }
            #endregion //퇴원, 전과

            #region //수술
            if (chkAll.Checked || chkChart4.Checked)
            {
                SQL.AppendLine("        UNION ALL");
                SQL.AppendLine("        --수술기록지");
                SQL.AppendLine("        SELECT 4 AS SORT");
                SQL.AppendLine("             , '수술' AS GB");
                SQL.AppendLine("             , C.EMRNO");
                SQL.AppendLine("             , C.INOUTCLS");
                SQL.AppendLine("             , C.CHARTDATE");
                SQL.AppendLine("             , C.CHARTTIME");
                SQL.AppendLine("             , C.USEID");
                SQL.AppendLine("             , C.MEDDEPTCD");
                SQL.AppendLine("             , D.DEPTNAMEK");
                SQL.AppendLine("             , F.FORMNO");
                SQL.AppendLine("             , F.UPDATENO");
                SQL.AppendLine("             , F.OLDGB");
                SQL.AppendLine("             , F.FORMNAME");
                SQL.AppendLine("             , U.NAME");
                SQL.AppendLine("             , 0 AS EMRNOHIS");
                SQL.AppendLine("          FROM  KOSMOS_EMR.EMRXMLMST C ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.AEMRFORM F ");
                SQL.AppendLine("                  ON F.FORMNO       = C.FORMNO ");
                SQL.AppendLine("                 AND F.UPDATENO     = 1 ");
                SQL.AppendLine("                 AND F.GRPFORMNO    = 1004 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.EMR_USERT U");
                SQL.AppendLine("                  ON C.USEID = U.USERID                 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
                SQL.AppendLine("                  ON C.MEDDEPTCD = D.DEPTCODE");
                SQL.AppendLine("         WHERE C.PTNO       = '" + ptNo + "' ");
                SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ");
                SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ");

                if (optInOut1.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'O'");
                }
                else if (optInOut2.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'I'");
                }

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ");
                }

            }
            #endregion //수술

            #region //ER기록지
            if (chkAll.Checked || chkChart5.Checked)
            {
                SQL.AppendLine("        UNION ALL");
                SQL.AppendLine("        --ER기록지");
                SQL.AppendLine("        SELECT 5 AS SORT");
                SQL.AppendLine("             , 'ER' AS GB");
                SQL.AppendLine("             , C.EMRNO");
                SQL.AppendLine("             , C.INOUTCLS");
                SQL.AppendLine("             , C.CHARTDATE");
                SQL.AppendLine("             , C.CHARTTIME");
                SQL.AppendLine("             , C.USEID");
                SQL.AppendLine("             , C.MEDDEPTCD");
                SQL.AppendLine("             , D.DEPTNAMEK");
                SQL.AppendLine("             , F.FORMNO");
                SQL.AppendLine("             , F.UPDATENO");
                SQL.AppendLine("             , F.OLDGB");
                SQL.AppendLine("             , F.FORMNAME");
                SQL.AppendLine("             , U.NAME");
                SQL.AppendLine("             , 0 AS EMRNOHIS");
                SQL.AppendLine("          FROM KOSMOS_EMR.EMRXMLMST C ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.AEMRFORM F ");
                SQL.AppendLine("                  ON F.FORMNO       = C.FORMNO ");
                SQL.AppendLine("                 AND F.UPDATENO     = 1");
                SQL.AppendLine("                 AND F.GRPFORMNO    = 1075 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.EMR_USERT U");
                SQL.AppendLine("                  ON C.USEID = U.USERID                 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
                SQL.AppendLine("                  ON C.MEDDEPTCD = D.DEPTCODE");
                SQL.AppendLine("         WHERE C.PTNO       = '" + ptNo + "' ");
                SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ");
                SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ");

                if (optInOut1.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'O'");
                }
                else if (optInOut2.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'I'");
                }

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ");
                }
            }
            #endregion //ER기록지

            #endregion //Old EMR

            //*********************************//
            //*********************************//

            #region //New EMR
            SQL.AppendLine("        UNION ALL");
            SQL.AppendLine("        --경과(재진/경과)");
            SQL.AppendLine("        SELECT 4 AS SORT");
            SQL.AppendLine("             , '경과' AS GB");
            SQL.AppendLine("             , C.EMRNO");
            SQL.AppendLine("             , C.INOUTCLS");
            SQL.AppendLine("             , C.CHARTDATE");
            SQL.AppendLine("             , C.CHARTTIME");
            SQL.AppendLine("             , C.CHARTUSEID AS USEID");
            SQL.AppendLine("             , C.MEDDEPTCD");
            SQL.AppendLine("             , D.DEPTNAMEK");
            SQL.AppendLine("             , F.FORMNO");
            SQL.AppendLine("             , F.UPDATENO");
            SQL.AppendLine("             , F.OLDGB");
            SQL.AppendLine("             , F.FORMNAME");
            SQL.AppendLine("             , U.NAME");
            SQL.AppendLine("             , C.EMRNOHIS");
            SQL.AppendLine("          FROM KOSMOS_EMR.AEMRCHARTMST C ");
            SQL.AppendLine("          INNER JOIN KOSMOS_EMR.AEMRFORM F ");
            SQL.AppendLine("                  ON F.FORMNO       = C.FORMNO ");
            SQL.AppendLine("                 AND F.UPDATENO     = C.UPDATENO ");
            SQL.AppendLine("                 AND F.GRPFORMNO    = 1001 ");
            SQL.AppendLine("          INNER JOIN KOSMOS_EMR.EMR_USERT U");
            SQL.AppendLine("                  ON C.CHARTUSEID = U.USERID                 ");
            SQL.AppendLine("          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
            SQL.AppendLine("                  ON C.MEDDEPTCD = D.DEPTCODE");
            SQL.AppendLine("         WHERE C.PTNO       = '" + ptNo + "' ");
            SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ");
            SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ");

            if (chkDeptAll.Checked == false && chkUserOpt.Checked)
            {
                SQL.AppendLine("           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ");
            }

            if (optInOut1.Checked)
            {
                SQL.AppendLine("           AND C.INOUTCLS = 'O'");
            }
            else if (optInOut2.Checked)
            {
                SQL.AppendLine("           AND C.INOUTCLS = 'I'");
            }


            if (chkAll.Checked || chkOrd.Checked)
            {
                //SQL.AppendLine("        UNION ALL");
                //SQL.AppendLine("        --처방 : 일별로 뿌려준다.");
                //SQL.AppendLine("        --외래 등록번호, 내원일자, 진료과 로 진단을 뿌린다");
                //SQL.AppendLine("        --입원 등록번호, 입원일자, 처방일자 로 한다");
                //SQL.AppendLine("        SELECT 5 AS SORT");
                //SQL.AppendLine("             , '처방' AS GB");
                //SQL.AppendLine("             , C.EMRNO");
                //SQL.AppendLine("             , C.INOUTCLS");
                //SQL.AppendLine("             , C.CHARTDATE");
                //SQL.AppendLine("             , C.CHARTTIME");
                //SQL.AppendLine("             , C.CHARTUSEID AS USEID");
                //SQL.AppendLine("             , C.MEDDEPTCD");
                //SQL.AppendLine("             , D.DEPTNAMEK");
                //SQL.AppendLine("             , F.FORMNO");
                //SQL.AppendLine("             , F.UPDATENO");
                //SQL.AppendLine("             , F.OLDGB");
                //SQL.AppendLine("             , F.FORMNAME");
                //SQL.AppendLine("             , U.NAME");
                //SQL.AppendLine("             , C.EMRNOHIS");
                //SQL.AppendLine("          FROM KOSMOS_EMR.AEMRCHARTMST C ");
                //SQL.AppendLine("          INNER JOIN KOSMOS_EMR.AEMRFORM F ");
                //SQL.AppendLine("                  ON F.FORMNO       = C.FORMNO ");
                //SQL.AppendLine("                 AND F.UPDATENO     = C.UPDATENO ");
                //SQL.AppendLine("                 AND F.GRPFORMNO    = 1001 ");
                //SQL.AppendLine("          INNER JOIN KOSMOS_EMR.EMR_USERT U");
                //SQL.AppendLine("                  ON C.CHARTUSEID = U.USERID                 ");
                //SQL.AppendLine("          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
                //SQL.AppendLine("                  ON C.MEDDEPTCD = D.DEPTCODE");
                //SQL.AppendLine("         WHERE C.PTNO       = '" + ptNo + "' ");
                //SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ");
                //SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ");

                //if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                //{
                //    SQL.AppendLine("           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ");
                //}


            }

            if (chkAll.Checked || chkChart0.Checked)
            {
                SQL.AppendLine("        ------*** 신규서식 ***--------");
                SQL.AppendLine("        UNION ALL");
                SQL.AppendLine("        --초진기록지 ");
                SQL.AppendLine("        SELECT 3 AS SORT");
                SQL.AppendLine("             , '초진' AS GB");
                SQL.AppendLine("             , C.EMRNO");
                SQL.AppendLine("             , C.INOUTCLS");
                SQL.AppendLine("             , C.CHARTDATE");
                SQL.AppendLine("             , C.CHARTTIME");
                SQL.AppendLine("             , C.CHARTUSEID AS USEID");
                SQL.AppendLine("             , C.MEDDEPTCD");
                SQL.AppendLine("             , D.DEPTNAMEK");
                SQL.AppendLine("             , F.FORMNO");
                SQL.AppendLine("             , F.UPDATENO");
                SQL.AppendLine("             , F.OLDGB");
                SQL.AppendLine("             , F.FORMNAME");
                SQL.AppendLine("             , U.NAME");
                SQL.AppendLine("             , C.EMRNOHIS");
                SQL.AppendLine("          FROM KOSMOS_EMR.AEMRCHARTMST C ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.AEMRFORM F ");
                SQL.AppendLine("                  ON F.FORMNO       = C.FORMNO ");
                SQL.AppendLine("                 AND F.UPDATENO     = C.UPDATENO ");
                SQL.AppendLine("                 AND F.GRPFORMNO    = 1000 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.EMR_USERT U");
                SQL.AppendLine("                  ON C.CHARTUSEID = U.USERID                 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
                SQL.AppendLine("                  ON C.MEDDEPTCD = D.DEPTCODE");
                SQL.AppendLine("         WHERE C.PTNO       = '" + ptNo + "' ");
                SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ");
                SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ");

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ");
                }


            }

            if ((chkAll.Checked || chkChart0.Checked || chkChart3.Checked ||
                chkChart4.Checked || chkChart5.Checked) && chkChart2.Checked)
            {
                //SQL.AppendLine("        UNION ALL");
            }

            if (chkAll.Checked || chkChart2.Checked)
            {
                SQL.AppendLine("        UNION ALL");
                SQL.AppendLine("        --입원기록지 ");
                SQL.AppendLine("        SELECT 3 AS SORT");
                SQL.AppendLine("             , '입원' AS GB");
                SQL.AppendLine("             , C.EMRNO");
                SQL.AppendLine("             , C.INOUTCLS");
                SQL.AppendLine("             , C.CHARTDATE");
                SQL.AppendLine("             , C.CHARTTIME");
                SQL.AppendLine("             , C.CHARTUSEID AS USEID");
                SQL.AppendLine("             , C.MEDDEPTCD");
                SQL.AppendLine("             , D.DEPTNAMEK");
                SQL.AppendLine("             , F.FORMNO");
                SQL.AppendLine("             , F.UPDATENO");
                SQL.AppendLine("             , F.OLDGB");
                SQL.AppendLine("             , F.FORMNAME");
                SQL.AppendLine("             , U.NAME");
                SQL.AppendLine("             , C.EMRNOHIS");
                SQL.AppendLine("          FROM KOSMOS_EMR.AEMRCHARTMST C ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.AEMRFORM F ");
                SQL.AppendLine("                  ON F.FORMNO    = C.FORMNO ");
                SQL.AppendLine("                 AND F.UPDATENO     = C.UPDATENO ");
                SQL.AppendLine("                 AND F.GRPFORMNO = 1002 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.EMR_USERT U");
                SQL.AppendLine("                  ON C.CHARTUSEID = U.USERID                 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
                SQL.AppendLine("                  ON C.MEDDEPTCD = D.DEPTCODE");
                SQL.AppendLine("         WHERE C.PTNO        = '" + ptNo + "' ");
                SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ");
                SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ");

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ");
                }

                if (optInOut1.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'O'");
                }
                else if (optInOut2.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'I'");
                }

                SQL.AppendLine("        UNION ALL");
                SQL.AppendLine("        --퇴원요약지, 전과기록지 ");
                SQL.AppendLine("        SELECT 3 AS SORT");
                SQL.AppendLine("             , '전과' AS GB");
                SQL.AppendLine("             , C.EMRNO");
                SQL.AppendLine("             , C.INOUTCLS");
                SQL.AppendLine("             , C.CHARTDATE");
                SQL.AppendLine("             , C.CHARTTIME");
                SQL.AppendLine("             , C.CHARTUSEID AS USEID");
                SQL.AppendLine("             , C.MEDDEPTCD");
                SQL.AppendLine("             , D.DEPTNAMEK");
                SQL.AppendLine("             , F.FORMNO");
                SQL.AppendLine("             , F.UPDATENO");
                SQL.AppendLine("             , F.OLDGB");
                SQL.AppendLine("             , F.FORMNAME");
                SQL.AppendLine("             , U.NAME");
                SQL.AppendLine("             , EMRNOHIS");
                SQL.AppendLine("          FROM KOSMOS_EMR.AEMRCHARTMST C ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.AEMRFORM F ");
                SQL.AppendLine("                  ON F.FORMNO     = C.FORMNO ");
                SQL.AppendLine("                 AND F.UPDATENO   = C.UPDATENO ");
                SQL.AppendLine("                 AND F.GRPFORMNO IN (1009, 1010) ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.EMR_USERT U");
                SQL.AppendLine("                  ON C.CHARTUSEID = U.USERID                 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
                SQL.AppendLine("                  ON C.MEDDEPTCD = D.DEPTCODE");
                SQL.AppendLine("         WHERE C.PTNO       = '" + ptNo + "' ");
                SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ");
                SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ");

                if (optInOut1.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'O'");
                }
                else if (optInOut2.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'I'");
                }

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ");
                }



            }

            if ((chkAll.Checked || chkChart0.Checked || chkChart2.Checked ||
                chkChart4.Checked || chkChart5.Checked) && chkChart3.Checked)
            {
                //SQL.AppendLine("        UNION ALL");
            }



            if ((chkAll.Checked || chkChart0.Checked || chkChart2.Checked || chkChart3.Checked ||
                chkChart5.Checked) && chkChart4.Checked)
            {
                //SQL.AppendLine("        UNION ALL");
            }

            if (chkAll.Checked || chkChart4.Checked)
            {
                SQL.AppendLine("        UNION ALL");
                SQL.AppendLine("        --수술기록지");
                SQL.AppendLine("        SELECT 4 AS SORT");
                SQL.AppendLine("             , '수술' AS GB");
                SQL.AppendLine("             , C.EMRNO");
                SQL.AppendLine("             , C.INOUTCLS");
                SQL.AppendLine("             , C.CHARTDATE");
                SQL.AppendLine("             , C.CHARTTIME");
                SQL.AppendLine("             , C.CHARTUSEID AS USEID");
                SQL.AppendLine("             , C.MEDDEPTCD");
                SQL.AppendLine("             , D.DEPTNAMEK");
                SQL.AppendLine("             , F.FORMNO");
                SQL.AppendLine("             , F.UPDATENO");
                SQL.AppendLine("             , F.OLDGB");
                SQL.AppendLine("             , F.FORMNAME");
                SQL.AppendLine("             , U.NAME");
                SQL.AppendLine("             , C.EMRNOHIS");
                SQL.AppendLine("          FROM KOSMOS_EMR.AEMRCHARTMST C ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.AEMRFORM F ");
                SQL.AppendLine("                  ON F.FORMNO       = C.FORMNO ");
                SQL.AppendLine("                 AND F.UPDATENO     = C.UPDATENO ");
                SQL.AppendLine("                 AND F.GRPFORMNO    = 1004 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.EMR_USERT U");
                SQL.AppendLine("                  ON C.CHARTUSEID = U.USERID                 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
                SQL.AppendLine("                  ON C.MEDDEPTCD = D.DEPTCODE");
                SQL.AppendLine("         WHERE C.PTNO       = '" + ptNo + "' ");
                SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ");
                SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ");

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ");
                }

                if (optInOut1.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'O'");
                }
                else if (optInOut2.Checked)
                {
                    SQL.AppendLine("           AND C.INOUTCLS = 'I'");
                }
            }

            if ((chkAll.Checked || chkChart0.Checked || chkChart2.Checked || chkChart3.Checked ||
                chkChart4.Checked) && chkChart5.Checked)
            {
                //SQL.AppendLine("        UNION ALL");
            }

            if (chkAll.Checked || chkChart5.Checked)
            {
                #region 19-08-12 ER기록지 추가
                SQL.AppendLine("        UNION ALL");
                SQL.AppendLine("        --ER기록지");
                SQL.AppendLine("        SELECT 4 AS SORT");
                SQL.AppendLine("             , 'ER' AS GB");
                SQL.AppendLine("             , C.EMRNO");
                SQL.AppendLine("             , C.INOUTCLS");
                SQL.AppendLine("             , C.CHARTDATE");
                SQL.AppendLine("             , C.CHARTTIME");
                SQL.AppendLine("             , C.CHARTUSEID AS USEID");
                SQL.AppendLine("             , C.MEDDEPTCD");
                SQL.AppendLine("             , D.DEPTNAMEK");
                SQL.AppendLine("             , F.FORMNO");
                SQL.AppendLine("             , F.UPDATENO");
                SQL.AppendLine("             , F.OLDGB");
                SQL.AppendLine("             , F.FORMNAME");
                SQL.AppendLine("             , U.NAME");
                SQL.AppendLine("             , C.EMRNOHIS");
                SQL.AppendLine("          FROM KOSMOS_EMR.AEMRCHARTMST C ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.AEMRFORM F ");
                SQL.AppendLine("                  ON F.FORMNO       = C.FORMNO ");
                SQL.AppendLine("                 AND F.UPDATENO     = C.UPDATENO ");
                SQL.AppendLine("                 AND F.GRPFORMNO    = 1075 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_EMR.EMR_USERT U");
                SQL.AppendLine("                  ON C.CHARTUSEID = U.USERID                 ");
                SQL.AppendLine("          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D");
                SQL.AppendLine("                  ON C.MEDDEPTCD = D.DEPTCODE");
                SQL.AppendLine("         WHERE C.PTNO       = '" + ptNo + "' ");
                SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ");
                SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ");
                #endregion
            }

            #endregion //New EMR

            //*********************************//
            //*********************************//

            SQL.AppendLine("    ) C");

            if (isImport == true)
            {
                SQL.AppendLine("WHERE EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP");
                SQL.AppendLine("         WHERE IMP.EMRNO = C.EMRNO");
                if (clsType.User.DeptCode == "MN")
                {
                    SQL.AppendLine("                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))");
                }
                else
                {
                    SQL.AppendLine("                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )");
                }

//                SQL.AppendLine("         AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )");
                SQL.AppendLine("      OR");
                SQL.AppendLine("      EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP");
                SQL.AppendLine("         WHERE IMP.EMRNO = C.EMRNO");
                //                SQL.AppendLine("         AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )");
                if (clsType.User.DeptCode == "MN")
                {
                    SQL.AppendLine("                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))");
                }
                else
                {
                    SQL.AppendLine("                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )");
                }

            }
            //SQL.AppendLine("ORDER BY SORT, CHARTDATE DESC, INOUTCLS DESC");
            SQL.AppendLine("ORDER BY (CHARTDATE || CHARTTIME) DESC, INOUTCLS ASC, SORT ");

            #endregion

            SqlErr = clsDB.GetDataTableREx(ref ChartDataList, SQL.ToString().Trim(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void GetChartList_OLD()
        {
            ChartDataList = null;
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            string ptNo = mPTNO; /// "06792410";

            bool mViewNpChart = clsEmrQueryOld.ViewNPChart(clsType.User.Sabun);

            #region 쿼리

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT SORT";
            SQL = SQL + ComNum.VBLF + "     , GB";
            SQL = SQL + ComNum.VBLF + "     , EMRNO";
            SQL = SQL + ComNum.VBLF + "     , INOUTCLS";
            SQL = SQL + ComNum.VBLF + "     , CHARTDATE";
            SQL = SQL + ComNum.VBLF + "     , CHARTTIME";
            SQL = SQL + ComNum.VBLF + "     , USEID";
            SQL = SQL + ComNum.VBLF + "     , FORMNO";
            SQL = SQL + ComNum.VBLF + "     , UPDATENO";
            SQL = SQL + ComNum.VBLF + "     , OLDGB";
            SQL = SQL + ComNum.VBLF + "     , FORMNAME";
            SQL = SQL + ComNum.VBLF + "     , NAME";
            SQL = SQL + ComNum.VBLF + "     , MEDDEPTCD";
            SQL = SQL + ComNum.VBLF + "     , DEPTNAMEK";
            SQL = SQL + ComNum.VBLF + "     , EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "  FROM ";
            SQL = SQL + ComNum.VBLF + "  (";
            SQL = SQL + ComNum.VBLF + "        --진단(외래는 일별 발생,입원은 __)";
            SQL = SQL + ComNum.VBLF + "        --외래 등록번호, 내원일자, 진료과 로 진단을 뿌린다";
            SQL = SQL + ComNum.VBLF + "        --입원 등록번호, 입원일자 로 한다";
            SQL = SQL + ComNum.VBLF + "        /*SELECT 1 AS SORT";
            SQL = SQL + ComNum.VBLF + "             , '진단' AS GB";
            SQL = SQL + ComNum.VBLF + "             , C.EMRNO";
            SQL = SQL + ComNum.VBLF + "             , C.INOUTCLS";
            SQL = SQL + ComNum.VBLF + "             , C.CHARTDATE";
            SQL = SQL + ComNum.VBLF + "             , C.CHARTTIME";
            SQL = SQL + ComNum.VBLF + "             , C.USEID";
            SQL = SQL + ComNum.VBLF + "             , C.MEDDEPTCD";
            SQL = SQL + ComNum.VBLF + "             , D.DEPTNAMEK";
            SQL = SQL + ComNum.VBLF + "             , F.FORMNO";
            SQL = SQL + ComNum.VBLF + "             , F.UPDATENO";
            SQL = SQL + ComNum.VBLF + "             , F.OLDGB";
            SQL = SQL + ComNum.VBLF + "             , F.FORMNAME";
            SQL = SQL + ComNum.VBLF + "             , U.NAME";
            SQL = SQL + ComNum.VBLF + "             , 0 AS EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.EMRXMLMST C ";
            SQL = SQL + ComNum.VBLF + "         INNER JOIN KOSMOS_EMR.AEMRFORM F ";
            SQL = SQL + ComNum.VBLF + "                 ON F.FORMNO    = C.FORMNO ";
            SQL = SQL + ComNum.VBLF + "                AND F.UPDATENO  = 1 ";
            SQL = SQL + ComNum.VBLF + "                AND F.GRPFORMNO = 1001";
            SQL = SQL + ComNum.VBLF + "         INNER JOIN KOSMOS_EMR.EMR_USERT U";
            SQL = SQL + ComNum.VBLF + "                 ON C.USEID = U.USERID";
            SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D";
            SQL = SQL + ComNum.VBLF + "                  ON C.MEDDEPTCD = D.DEPTCODE";
            SQL = SQL + ComNum.VBLF + "         WHERE C.PTNO       = '" + ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ";
            if (isImport == true)
            {
                SQL = SQL + ComNum.VBLF + "                  AND ( ";
                SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP";
                SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                if (clsType.User.DeptCode == "MN")
                {
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                }
                //SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                SQL = SQL + ComNum.VBLF + "                         OR";
                SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP";
                SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                //SQL = SQL + ComNum.VBLF + "                                 AND IMP.EMRNOHIS = C.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                SQL = SQL + ComNum.VBLF + "                  ) ";
            }
            SQL = SQL + ComNum.VBLF + "        UNION ALL*/ ";

            if (chkAll.Checked || chkChart0.Checked)
            {
                SQL = SQL + ComNum.VBLF + "	--초진기록지 ";
                SQL = SQL + ComNum.VBLF + "        SELECT 3 AS SORT";
                SQL = SQL + ComNum.VBLF + "             , '초진' AS GB";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNO";
                SQL = SQL + ComNum.VBLF + "             , C.INOUTCLS";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTDATE";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTTIME";
                SQL = SQL + ComNum.VBLF + "             , C.USEID";
                SQL = SQL + ComNum.VBLF + "             , C.MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "             , D.DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNO";
                SQL = SQL + ComNum.VBLF + "             , F.UPDATENO";
                SQL = SQL + ComNum.VBLF + "             , F.OLDGB";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNAME";
                SQL = SQL + ComNum.VBLF + "             , U.NAME";
                SQL = SQL + ComNum.VBLF + "             , 0 AS EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.EMRXMLMST C ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.AEMRFORM F ";
                SQL = SQL + ComNum.VBLF + "                  ON C.FORMNO       = F.FORMNO ";
                SQL = SQL + ComNum.VBLF + "                 AND F.UPDATENO     = 1 ";
                SQL = SQL + ComNum.VBLF + "                 AND F.GRPFORMNO    = 1000";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.EMR_USERT U";
                SQL = SQL + ComNum.VBLF + "                  ON C.USEID = U.USERID                 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D";
                SQL = SQL + ComNum.VBLF + "                  ON C.MEDDEPTCD = D.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE C.PTNO       = '" + ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ";

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL = SQL + ComNum.VBLF + "           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ";
                }

                if (isImport == true)
                {
                    SQL = SQL + ComNum.VBLF + "                  AND ( ";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    if (clsType.User.DeptCode == "MN")
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    }

//                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                         OR";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.EMRNOHIS = C.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                  ) ";
                }
            }

            if ((chkAll.Checked || chkChart0.Checked) && chkChart2.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        UNION ALL";
            }

            if (chkAll.Checked || chkChart2.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        --입원기록지 ";
                SQL = SQL + ComNum.VBLF + "        SELECT 3 AS SORT";
                SQL = SQL + ComNum.VBLF + "             , '입원' AS GB";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNO";
                SQL = SQL + ComNum.VBLF + "             , C.INOUTCLS";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTDATE";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTTIME";
                SQL = SQL + ComNum.VBLF + "             , C.USEID";
                SQL = SQL + ComNum.VBLF + "             , C.MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "             , D.DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNO";
                SQL = SQL + ComNum.VBLF + "             , F.UPDATENO";
                SQL = SQL + ComNum.VBLF + "             , F.OLDGB";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNAME";
                SQL = SQL + ComNum.VBLF + "             , U.NAME";
                SQL = SQL + ComNum.VBLF + "             , 0 AS EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.EMRXMLMST C ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.AEMRFORM F ";
                SQL = SQL + ComNum.VBLF + "                  ON F.FORMNO     = C.FORMNO ";
                SQL = SQL + ComNum.VBLF + "                 AND F.UPDATENO   = 1 ";
                SQL = SQL + ComNum.VBLF + "                 AND F.GRPFORMNO  = 1002";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.EMR_USERT U";
                SQL = SQL + ComNum.VBLF + "                  ON C.USEID = U.USERID                 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D";
                SQL = SQL + ComNum.VBLF + "                  ON C.MEDDEPTCD = D.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE C.PTNO       = '" + ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ";

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL = SQL + ComNum.VBLF + "           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ";
                }

                if (isImport == true)
                {
                    SQL = SQL + ComNum.VBLF + "                  AND ( ";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    if (clsType.User.DeptCode == "MN")
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    }

                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                         OR";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.EMRNOHIS = C.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                  ) ";
                }
            }

            if ((chkAll.Checked || chkChart0.Checked || chkChart2.Checked) && chkChart3.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        UNION ALL";
            }

            if (chkAll.Checked || chkChart3.Checked)
            {
                #region //19-08-12 퇴원요약지,전과기록지 추가
                SQL = SQL + ComNum.VBLF + "        --퇴원요약지 ";
                SQL = SQL + ComNum.VBLF + "        SELECT 3 AS SORT";
                SQL = SQL + ComNum.VBLF + "             , '퇴원' AS GB";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNO";
                SQL = SQL + ComNum.VBLF + "             , C.INOUTCLS";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTDATE";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTTIME";
                SQL = SQL + ComNum.VBLF + "             , C.USEID";
                SQL = SQL + ComNum.VBLF + "             , C.MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "             , D.DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNO";
                SQL = SQL + ComNum.VBLF + "             , F.UPDATENO";
                SQL = SQL + ComNum.VBLF + "             , F.OLDGB";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNAME";
                SQL = SQL + ComNum.VBLF + "             , U.NAME";
                SQL = SQL + ComNum.VBLF + "             , 0 AS EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.EMRXMLMST C ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.AEMRFORM F ";
                SQL = SQL + ComNum.VBLF + "                  ON F.FORMNO     = C.FORMNO ";
                SQL = SQL + ComNum.VBLF + "                 AND F.UPDATENO   = 1 ";
                SQL = SQL + ComNum.VBLF + "                 AND F.GRPFORMNO  = 1009";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.EMR_USERT U";
                SQL = SQL + ComNum.VBLF + "                  ON C.USEID = U.USERID                 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D";
                SQL = SQL + ComNum.VBLF + "                  ON C.MEDDEPTCD = D.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE C.PTNO       = '" + ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ";

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL = SQL + ComNum.VBLF + "           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ";
                }

                if (isImport == true)
                {
                    SQL = SQL + ComNum.VBLF + "                  AND ( ";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    if (clsType.User.DeptCode == "MN")
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    }

                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                         OR";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.EMRNOHIS = C.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                  ) ";
                }

                SQL = SQL + ComNum.VBLF + "        UNION ALL";
                SQL = SQL + ComNum.VBLF + "        --전과기록지 ";
                SQL = SQL + ComNum.VBLF + "        SELECT 3 AS SORT";
                SQL = SQL + ComNum.VBLF + "             , '전과' AS GB";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNO";
                SQL = SQL + ComNum.VBLF + "             , C.INOUTCLS";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTDATE";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTTIME";
                SQL = SQL + ComNum.VBLF + "             , C.USEID";
                SQL = SQL + ComNum.VBLF + "             , C.MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "             , D.DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNO";
                SQL = SQL + ComNum.VBLF + "             , F.UPDATENO";
                SQL = SQL + ComNum.VBLF + "             , F.OLDGB";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNAME";
                SQL = SQL + ComNum.VBLF + "             , U.NAME";
                SQL = SQL + ComNum.VBLF + "             , 0 AS EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.EMRXMLMST C ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.AEMRFORM F ";
                SQL = SQL + ComNum.VBLF + "                  ON F.FORMNO     = C.FORMNO ";
                SQL = SQL + ComNum.VBLF + "                 AND F.UPDATENO   = 1 ";
                SQL = SQL + ComNum.VBLF + "                 AND F.GRPFORMNO  = 1010 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.EMR_USERT U";
                SQL = SQL + ComNum.VBLF + "                  ON C.USEID = U.USERID                 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D";
                SQL = SQL + ComNum.VBLF + "                  ON C.MEDDEPTCD = D.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE C.PTNO       = '" + ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ";

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL = SQL + ComNum.VBLF + "           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ";
                }

                if (isImport == true)
                {
                    SQL = SQL + ComNum.VBLF + "                  AND ( ";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    if (clsType.User.DeptCode == "MN")
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    }
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                         OR";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.EMRNOHIS = C.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                  ) ";
                }
                #endregion
            }

            if (chkAll.Checked || chkChart0.Checked || chkChart2.Checked || chkChart3.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        UNION ALL";
            }

            SQL = SQL + ComNum.VBLF + "        --경과(재진/경과)";
            SQL = SQL + ComNum.VBLF + "        SELECT 4 AS SORT";
            SQL = SQL + ComNum.VBLF + "             , '경과' AS GB";
            SQL = SQL + ComNum.VBLF + "             , C.EMRNO";
            SQL = SQL + ComNum.VBLF + "             , C.INOUTCLS";
            SQL = SQL + ComNum.VBLF + "             , C.CHARTDATE";
            SQL = SQL + ComNum.VBLF + "             , C.CHARTTIME";
            SQL = SQL + ComNum.VBLF + "             , C.USEID";
            SQL = SQL + ComNum.VBLF + "             , C.MEDDEPTCD";
            SQL = SQL + ComNum.VBLF + "             , D.DEPTNAMEK";
            SQL = SQL + ComNum.VBLF + "             , F.FORMNO";
            SQL = SQL + ComNum.VBLF + "             , F.UPDATENO";
            SQL = SQL + ComNum.VBLF + "             , F.OLDGB";
            SQL = SQL + ComNum.VBLF + "             , F.FORMNAME";
            SQL = SQL + ComNum.VBLF + "             , U.NAME";
            SQL = SQL + ComNum.VBLF + "             , 0 AS EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.EMRXMLMST C ";
            SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.AEMRFORM F ";
            SQL = SQL + ComNum.VBLF + "                  ON F.FORMNO       = C.FORMNO ";
            SQL = SQL + ComNum.VBLF + "                 AND F.UPDATENO     = 1 ";
            SQL = SQL + ComNum.VBLF + "                 AND F.GRPFORMNO    = 1001";
            SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.EMR_USERT U";
            SQL = SQL + ComNum.VBLF + "                  ON C.USEID = U.USERID                 ";
            SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D";
            SQL = SQL + ComNum.VBLF + "                  ON C.MEDDEPTCD = D.DEPTCODE";
            SQL = SQL + ComNum.VBLF + "         WHERE C.PTNO       = '" + ptNo + "' ";
            //SQL = SQL + ComNum.VBLF + "           AND C.INOUTCLS   = '" + AcpEmr.inOutCls + "' ";
            //SQL = SQL + ComNum.VBLF + "           AND C.MEDDEPTCD  = '" + AcpEmr.medDeptCd + "' ";

            #region 경과기록지 처리 필요
            //if (AcpEmr.inOutCls == "O")
            //{
            //    if (AcpEmr.medDeptCd == "RA" || (AcpEmr.medDeptCd == "MD" && (AcpEmr.medDrCd == "1107" || AcpEmr.medDrCd == "1125")))
            //    {
            //        SQL = SQL + ComNum.VBLF + "                AND C.MEDDEPTCD = 'MD'";
            //        SQL = SQL + ComNum.VBLF + "                AND C.MEDDRCD IN ('1107','1125')";
            //    }
            //    else
            //    {
            //        SQL = SQL + ComNum.VBLF + "                AND C.MEDDEPTCD = '" + AcpEmr.medDeptCd + "'";
            //        SQL = SQL + ComNum.VBLF + "                AND C.MEDDRCD NOT IN ('1107','1125')";
            //    }
            //}
            //else
            //{
            //    SQL = SQL + ComNum.VBLF + "                AND C.INOUTCLS IN ('I') ";
            //}

            //if (AcpEmr.inOutCls == "O" && AcpEmr.medDeptCd == "HD")
            //{
            //    SQL = SQL + ComNum.VBLF + "                AND C.INOUTCLS IN ('O','I') ";
            //}
            //else if (AcpEmr.inOutCls == "O" && AcpEmr.medDeptCd != "HD")
            //{
            //    SQL = SQL + ComNum.VBLF + "                AND C.INOUTCLS IN ('O') ";
            //}
            #endregion

            if (chkDeptAll.Checked == false && chkUserOpt.Checked)
            {
                SQL = SQL + ComNum.VBLF + "           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ";
            }

            if (mViewNpChart == false)
            {
                SQL = SQL + ComNum.VBLF + "        AND C.MEDDEPTCD <> 'NP'";
            }

            SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ";

            if (isImport == true)
            {
                SQL = SQL + ComNum.VBLF + "                  AND ( ";
                SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP";
                SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                if (clsType.User.DeptCode == "MN")
                {
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                }
                //SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                SQL = SQL + ComNum.VBLF + "                         OR";
                SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP";
                SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                //SQL = SQL + ComNum.VBLF + "                                 AND IMP.EMRNOHIS = C.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                SQL = SQL + ComNum.VBLF + "                  ) ";
            }

            if ((chkAll.Checked || chkChart0.Checked || chkChart2.Checked || chkChart3.Checked) && chkChart4.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        UNION ALL";
            }

            if (chkAll.Checked || chkChart4.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        --수술기록지";
                SQL = SQL + ComNum.VBLF + "        SELECT 4 AS SORT";
                SQL = SQL + ComNum.VBLF + "             , '수술' AS GB";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNO";
                SQL = SQL + ComNum.VBLF + "             , C.INOUTCLS";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTDATE";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTTIME";
                SQL = SQL + ComNum.VBLF + "             , C.USEID";
                SQL = SQL + ComNum.VBLF + "             , C.MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "             , D.DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNO";
                SQL = SQL + ComNum.VBLF + "             , F.UPDATENO";
                SQL = SQL + ComNum.VBLF + "             , F.OLDGB";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNAME";
                SQL = SQL + ComNum.VBLF + "             , U.NAME";
                SQL = SQL + ComNum.VBLF + "             , 0 AS EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "          FROM  KOSMOS_EMR.EMRXMLMST C ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.AEMRFORM F ";
                SQL = SQL + ComNum.VBLF + "                  ON F.FORMNO       = C.FORMNO ";
                SQL = SQL + ComNum.VBLF + "                 AND F.UPDATENO     = 1 ";
                SQL = SQL + ComNum.VBLF + "                 AND F.GRPFORMNO    = 1004 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.EMR_USERT U";
                SQL = SQL + ComNum.VBLF + "                  ON C.USEID = U.USERID                 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D";
                SQL = SQL + ComNum.VBLF + "                  ON C.MEDDEPTCD = D.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE C.PTNO       = '" + ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ";

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL = SQL + ComNum.VBLF + "           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ";
                }

                if (isImport == true)
                {
                    SQL = SQL + ComNum.VBLF + "                  AND ( ";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    if (clsType.User.DeptCode == "MN")
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    }
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                         OR";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.EMRNOHIS = C.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                  ) ";
                }
            }

            if ((chkAll.Checked || chkChart0.Checked || chkChart2.Checked || chkChart3.Checked ||
                chkChart4.Checked) && chkChart5.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        UNION ALL";
            }

            if (chkAll.Checked || chkChart5.Checked)
            {
                #region 19-08-12 ER기록지 추가
                SQL = SQL + ComNum.VBLF + "        --ER기록지";
                SQL = SQL + ComNum.VBLF + "        SELECT 5 AS SORT";
                SQL = SQL + ComNum.VBLF + "             , 'ER' AS GB";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNO";
                SQL = SQL + ComNum.VBLF + "             , C.INOUTCLS";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTDATE";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTTIME";
                SQL = SQL + ComNum.VBLF + "             , C.USEID";
                SQL = SQL + ComNum.VBLF + "             , C.MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "             , D.DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNO";
                SQL = SQL + ComNum.VBLF + "             , F.UPDATENO";
                SQL = SQL + ComNum.VBLF + "             , F.OLDGB";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNAME";
                SQL = SQL + ComNum.VBLF + "             , U.NAME";
                SQL = SQL + ComNum.VBLF + "             , 0 AS EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.EMRXMLMST C ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.AEMRFORM F ";
                SQL = SQL + ComNum.VBLF + "                  ON F.FORMNO       = C.FORMNO ";
                SQL = SQL + ComNum.VBLF + "                 AND F.GRPFORMNO    = 1075 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.EMR_USERT U";
                SQL = SQL + ComNum.VBLF + "                  ON C.USEID = U.USERID                 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D";
                SQL = SQL + ComNum.VBLF + "                  ON C.MEDDEPTCD = D.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE C.PTNO       = '" + ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ";

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL = SQL + ComNum.VBLF + "           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ";
                }

                if (isImport == true)
                {
                    SQL = SQL + ComNum.VBLF + "                  AND ( ";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    if (clsType.User.DeptCode == "MN")
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    }
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                         OR";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.EMRNOHIS = C.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                  ) ";
                }
                #endregion
            }


            if ((chkAll.Checked || chkChart2.Checked || chkChart3.Checked ||
                chkChart4.Checked || chkChart5.Checked) && chkChart0.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        UNION ALL";
            }

            SQL = SQL + ComNum.VBLF + "        --처방 : 일별로 뿌려준다.";
            SQL = SQL + ComNum.VBLF + "        --외래 등록번호, 내원일자, 진료과 로 진단을 뿌린다";
            SQL = SQL + ComNum.VBLF + "        --입원 등록번호, 입원일자, 처방일자 로 한다";
            SQL = SQL + ComNum.VBLF + "        /*SELECT 6 AS SORT";
            SQL = SQL + ComNum.VBLF + "             , '처방' AS GB";
            SQL = SQL + ComNum.VBLF + "             , C.EMRNO";
            SQL = SQL + ComNum.VBLF + "             , C.INOUTCLS";
            SQL = SQL + ComNum.VBLF + "             , C.CHARTDATE";
            SQL = SQL + ComNum.VBLF + "             , C.CHARTTIME";
            SQL = SQL + ComNum.VBLF + "             , C.USEID";
            SQL = SQL + ComNum.VBLF + "             , C.MEDDEPTCD";
            SQL = SQL + ComNum.VBLF + "             , D.DEPTNAMEK";
            SQL = SQL + ComNum.VBLF + "             , F.FORMNO";
            SQL = SQL + ComNum.VBLF + "             , F.UPDATENO";
            SQL = SQL + ComNum.VBLF + "             , F.OLDGB";
            SQL = SQL + ComNum.VBLF + "             , F.FORMNAME";
            SQL = SQL + ComNum.VBLF + "             , U.NAME";
            SQL = SQL + ComNum.VBLF + "             , C.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.AEMRCHARTMST C ";
            SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.AEMRFORM F ";
            SQL = SQL + ComNum.VBLF + "                  ON F.FORMNO       = C.FORMNO ";
            SQL = SQL + ComNum.VBLF + "                 AND F.GRPFORMNO    = 1001 ";
            SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.EMR_USERT U";
            SQL = SQL + ComNum.VBLF + "                  ON C.USEID = U.USERID                 ";
            SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D";
            SQL = SQL + ComNum.VBLF + "                  ON C.MEDDEPTCD = D.DEPTCODE";
            SQL = SQL + ComNum.VBLF + "         WHERE C.PTNO       = '" + ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ";

            if (chkDeptAll.Checked == false && chkUserOpt.Checked)
            {
                SQL = SQL + ComNum.VBLF + "           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ";
            }

            if (isImport == true)
            {
                SQL = SQL + ComNum.VBLF + "                  AND ( ";
                SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP";
                SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                if (clsType.User.DeptCode == "MN")
                {
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                }
                //SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                SQL = SQL + ComNum.VBLF + "                         OR";
                SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP";
                SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                //SQL = SQL + ComNum.VBLF + "                                 AND IMP.EMRNOHIS = C.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                SQL = SQL + ComNum.VBLF + "                  ) ";
            }
            SQL = SQL + ComNum.VBLF + "        UNION ALL */";

            if (chkAll.Checked || chkChart0.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        ------*** 신규서식 ***--------";
                SQL = SQL + ComNum.VBLF + "        --초진기록지 ";
                SQL = SQL + ComNum.VBLF + "        SELECT 3 AS SORT";
                SQL = SQL + ComNum.VBLF + "             , '초진' AS GB";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNO";
                SQL = SQL + ComNum.VBLF + "             , C.INOUTCLS";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTDATE";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTTIME";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTUSEID";
                SQL = SQL + ComNum.VBLF + "             , C.MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "             , D.DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNO";
                SQL = SQL + ComNum.VBLF + "             , F.UPDATENO";
                SQL = SQL + ComNum.VBLF + "             , F.OLDGB";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNAME";
                SQL = SQL + ComNum.VBLF + "             , U.NAME";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.AEMRCHARTMST C ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.AEMRFORM F ";
                SQL = SQL + ComNum.VBLF + "                  ON F.FORMNO       = C.FORMNO ";
                SQL = SQL + ComNum.VBLF + "                 AND F.GRPFORMNO    = 1000 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.EMR_USERT U";
                SQL = SQL + ComNum.VBLF + "                  ON C.CHARTUSEID = U.USERID                 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D";
                SQL = SQL + ComNum.VBLF + "                  ON C.MEDDEPTCD = D.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE C.PTNO       = '" + ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ";

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL = SQL + ComNum.VBLF + "           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ";
                }


                if (isImport == true)
                {
                    SQL = SQL + ComNum.VBLF + "                  AND ( ";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    if (clsType.User.DeptCode == "MN")
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    }
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                         OR";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.EMRNOHIS = C.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                  ) ";
                }
            }

            if ((chkAll.Checked || chkChart0.Checked || chkChart3.Checked ||
                chkChart4.Checked || chkChart5.Checked) && chkChart2.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        UNION ALL";
            }

            if (chkAll.Checked || chkChart2.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        --입원기록지 ";
                SQL = SQL + ComNum.VBLF + "        SELECT 3 AS SORT";
                SQL = SQL + ComNum.VBLF + "             , '입원' AS GB";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNO";
                SQL = SQL + ComNum.VBLF + "             , C.INOUTCLS";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTDATE";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTTIME";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTUSEID";
                SQL = SQL + ComNum.VBLF + "             , C.MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "             , D.DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNO";
                SQL = SQL + ComNum.VBLF + "             , F.UPDATENO";
                SQL = SQL + ComNum.VBLF + "             , F.OLDGB";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNAME";
                SQL = SQL + ComNum.VBLF + "             , U.NAME";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.AEMRCHARTMST C ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.AEMRFORM F ";
                SQL = SQL + ComNum.VBLF + "                  ON F.FORMNO    = C.FORMNO ";
                SQL = SQL + ComNum.VBLF + "                 AND F.GRPFORMNO = 1002 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.EMR_USERT U";
                SQL = SQL + ComNum.VBLF + "                  ON C.CHARTUSEID = U.USERID                 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D";
                SQL = SQL + ComNum.VBLF + "                  ON C.MEDDEPTCD = D.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE C.PTNO        = '" + ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ";

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL = SQL + ComNum.VBLF + "           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ";
                }

                if (isImport == true)
                {
                    SQL = SQL + ComNum.VBLF + "                  AND ( ";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    if (clsType.User.DeptCode == "MN")
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    }
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                         OR";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.EMRNOHIS = C.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                  ) ";
                }
            }

            if ((chkAll.Checked || chkChart0.Checked || chkChart2.Checked ||
                chkChart4.Checked || chkChart5.Checked) && chkChart3.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        UNION ALL";
            }

            if (chkAll.Checked || chkChart3.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        --경과(재진/경과)";
                SQL = SQL + ComNum.VBLF + "        SELECT 4 AS SORT";
                SQL = SQL + ComNum.VBLF + "             , '경과' AS GB";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNO";
                SQL = SQL + ComNum.VBLF + "             , C.INOUTCLS";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTDATE";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTTIME";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTUSEID";
                SQL = SQL + ComNum.VBLF + "             , C.MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "             , D.DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNO";
                SQL = SQL + ComNum.VBLF + "             , F.UPDATENO";
                SQL = SQL + ComNum.VBLF + "             , F.OLDGB";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNAME";
                SQL = SQL + ComNum.VBLF + "             , U.NAME";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.AEMRCHARTMST C ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.AEMRFORM F ";
                SQL = SQL + ComNum.VBLF + "                  ON F.FORMNO       = C.FORMNO ";
                SQL = SQL + ComNum.VBLF + "                 AND F.GRPFORMNO    = 1001 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.EMR_USERT U";
                SQL = SQL + ComNum.VBLF + "                  ON C.CHARTUSEID = U.USERID                 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D";
                SQL = SQL + ComNum.VBLF + "                  ON C.MEDDEPTCD = D.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE C.PTNO       = '" + ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ";

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL = SQL + ComNum.VBLF + "           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ";
                }

                if (isImport == true)
                {
                    SQL = SQL + ComNum.VBLF + "                  AND ( ";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    if (clsType.User.DeptCode == "MN")
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    }
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                         OR";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.EMRNOHIS = C.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                  ) ";
                }
            }

            if ((chkAll.Checked || chkChart0.Checked || chkChart2.Checked || chkChart3.Checked ||
                chkChart5.Checked) && chkChart4.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        UNION ALL";
            }

            if (chkAll.Checked || chkChart4.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        --수술기록지";
                SQL = SQL + ComNum.VBLF + "        SELECT 4 AS SORT";
                SQL = SQL + ComNum.VBLF + "             , '수술' AS GB";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNO";
                SQL = SQL + ComNum.VBLF + "             , C.INOUTCLS";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTDATE";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTTIME";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTUSEID";
                SQL = SQL + ComNum.VBLF + "             , C.MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "             , D.DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNO";
                SQL = SQL + ComNum.VBLF + "             , F.UPDATENO";
                SQL = SQL + ComNum.VBLF + "             , F.OLDGB";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNAME";
                SQL = SQL + ComNum.VBLF + "             , U.NAME";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.AEMRCHARTMST C ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.AEMRFORM F ";
                SQL = SQL + ComNum.VBLF + "                  ON F.FORMNO       = C.FORMNO ";
                SQL = SQL + ComNum.VBLF + "                 AND F.GRPFORMNO    = 1004 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.EMR_USERT U";
                SQL = SQL + ComNum.VBLF + "                  ON C.CHARTUSEID = U.USERID                 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D";
                SQL = SQL + ComNum.VBLF + "                  ON C.MEDDEPTCD = D.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE C.PTNO       = '" + ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ";

                if (chkDeptAll.Checked == false && chkUserOpt.Checked)
                {
                    SQL = SQL + ComNum.VBLF + "           AND C.MEDDEPTCD IN(" + mstrUserDept + ") ";
                }

                if (isImport == true)
                {
                    SQL = SQL + ComNum.VBLF + "                  AND ( ";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    if (clsType.User.DeptCode == "MN")
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    }
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                         OR";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.EMRNOHIS = C.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                  ) ";
                }
            }

            if ((chkAll.Checked || chkChart0.Checked || chkChart2.Checked || chkChart3.Checked ||
                chkChart4.Checked) && chkChart5.Checked)
            {
                SQL = SQL + ComNum.VBLF + "        UNION ALL";
            }

            if (chkAll.Checked || chkChart5.Checked)
            {
                #region 19-08-12 ER기록지 추가
                SQL = SQL + ComNum.VBLF + "        --ER기록지";
                SQL = SQL + ComNum.VBLF + "        SELECT 4 AS SORT";
                SQL = SQL + ComNum.VBLF + "             , 'ER' AS GB";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNO";
                SQL = SQL + ComNum.VBLF + "             , C.INOUTCLS";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTDATE";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTTIME";
                SQL = SQL + ComNum.VBLF + "             , C.CHARTUSEID";
                SQL = SQL + ComNum.VBLF + "             , C.MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "             , D.DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNO";
                SQL = SQL + ComNum.VBLF + "             , F.UPDATENO";
                SQL = SQL + ComNum.VBLF + "             , F.OLDGB";
                SQL = SQL + ComNum.VBLF + "             , F.FORMNAME";
                SQL = SQL + ComNum.VBLF + "             , U.NAME";
                SQL = SQL + ComNum.VBLF + "             , C.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.AEMRCHARTMST C ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.AEMRFORM F ";
                SQL = SQL + ComNum.VBLF + "                  ON F.FORMNO       = C.FORMNO ";
                SQL = SQL + ComNum.VBLF + "                 AND F.GRPFORMNO    = 1075 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.EMR_USERT U";
                SQL = SQL + ComNum.VBLF + "                  ON C.CHARTUSEID = U.USERID                 ";
                SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT D";
                SQL = SQL + ComNum.VBLF + "                  ON C.MEDDEPTCD = D.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE C.PTNO       = '" + ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "           AND C.CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "' ";
                if (isImport == true)
                {
                    SQL = SQL + ComNum.VBLF + "                  AND ( ";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTIMPORT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    if (clsType.User.DeptCode == "MN")
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER IN (SELECT DOCCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE DEPTCODE = 'MN'))";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    }
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                         OR";
                    SQL = SQL + ComNum.VBLF + "                         EXISTS (SELECT 1 FROM KOSMOS_EMR.AEMRCHARTCOMMENT IMP";
                    SQL = SQL + ComNum.VBLF + "                                 WHERE IMP.EMRNO = C.EMRNO";
                    //SQL = SQL + ComNum.VBLF + "                                 AND IMP.EMRNOHIS = C.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "                                 AND IMP.IDNUMBER = '" + clsType.User.IdNumber + "' )";
                    SQL = SQL + ComNum.VBLF + "                  ) ";
                }
                #endregion
            }
            SQL = SQL + ComNum.VBLF + "    )";
            //SQL = SQL + ComNum.VBLF + "ORDER BY SORT, CHARTDATE DESC, INOUTCLS DESC";
            SQL = SQL + ComNum.VBLF + "ORDER BY (CHARTDATE || CHARTTIME) DESC, INOUTCLS ASC, SORT ";

            #endregion

            SqlErr = clsDB.GetDataTableREx(ref ChartDataList, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        /// <summary>
        /// 차트 추가
        /// </summary>
        private void ChartAdd(bool isFirst = false)
        {
            if (ChartDataList == null)
                return;

            int i = 0;
            int max = 1;

            if(isFirst)
            {
                max = 8;
            }
            
            if (AcpEmr != null && (clsType.User.AuAPRINTIN.Equals("1") || clsType.User.BuseCode.Equals("077402")))
            {
                max = ChartDataList.Rows.Count;
            }
            Thread thread = new Thread(new ThreadStart(delegate ()
            {
                foreach (DataRow row in ChartDataList.Rows)
                {
                    DataRow newRow = ChartDataList.NewRow();
                    newRow.ItemArray = row.ItemArray;

                    if (InvokeRequired)
                    {
                        BeginInvoke(new Action(delegate ()
                        {
                            CreateForm(newRow);
                        }));
                    }
                    else
                    {
                        CreateForm(newRow);
                    }


                    if (AcpEmr != null && (clsType.User.AuAPRINTIN.Equals("1") || clsType.User.BuseCode.Equals("077402")))
                    {

                    }

                    i++;
                    if(isFirst)
                    {
                        //if(ChartHeight > 1500)
                        //{
                        //    break;
                        //}
                        if (i == max)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (i == max)
                        {
                            break;
                        }
                    }

                    //long maxValue = PanelHeight - ChartHeight;
                }

                for (int j = i - 1; j >= 0; j--)
                {
                    ChartDataList.Rows.RemoveAt(j);
                }
            }));
            thread.IsBackground = true;
            thread.Start();
        }

        private void SetPatEmr()
        {
            if (AcpEmr == null) return;

            SetContinuView();
        }

        /// <summary>
        /// 최종입원 내역 조회
        /// </summary>
        private void GetLastDsch()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //panLastDschDate.Visible = false;
            lblLastDschDate.Text = string.Empty;

            SQL = string.Empty;
            //SQL = " SELECT  ";
            //SQL = SQL + ComNum.VBLF + "    A.MEDFRDATE, A.MEDENDDATE, A.MEDDEPTCD  ";
            //SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AVIEWACP A  ";
            //SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + mPTNO + "' ";
            //SQL = SQL + ComNum.VBLF + "    AND A.INOUTCLS = 'I'  ";
            //SQL = SQL + ComNum.VBLF + "    AND A.CNCLYN = '0' ";
            //SQL = SQL + ComNum.VBLF + "    AND A.MEDFRDATE = (SELECT MAX(B.MEDFRDATE) FROM " + ComNum.DB_EMR + "AVIEWACP B ";
            //SQL = SQL + ComNum.VBLF + "                            WHERE B.PTNO = A.PTNO  ";
            //SQL = SQL + ComNum.VBLF + "                                AND B.INOUTCLS = 'I'  ";
            //SQL = SQL + ComNum.VBLF + "                                AND B.MEDFRDATE <= TO_CHAR(SYSDATE , 'YYYYMMDD')) ";

            SQL = SQL + ComNum.VBLF + "SELECT 	  TO_CHAR(A.INDATE, 'YYYY-MM-DD') MEDFRDATE                                             ";
            SQL = SQL + ComNum.VBLF + "		, COALESCE(TO_CHAR(A.OUTDATE, 'YYYY-MM-DD'), '재원중') AS MEDENDDATE                                               ";
            SQL = SQL + ComNum.VBLF + "		, A.DEPTCODE AS MEDDEPTCD                                                                 ";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.IPD_NEW_MASTER A                                                              ";
            SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + mPTNO + "'                                                                        ";
            SQL = SQL + ComNum.VBLF + "  AND IPDNO  = (SELECT MAX(IPDNO) FROM KOSMOS_PMPA.IPD_NEW_MASTER WHERE PANO = A.PANO)       ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                //clsComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                return;
            }

            //if (strMEDENDDATE == "99991231" || strMEDENDDATE == "99981231" || strMEDENDDATE == "")
            //{
            //    strMEDENDDATE = "재원중";
            //}
            //else
            //{
            //    strMEDENDDATE = ComFunc.FormatStrToDate(dt.Rows[0]["MEDENDDATE"].ToString().Trim(), "D");
            //}

            //lblLastDschDate.Text = ComFunc.FormatStrToDate(dt.Rows[0]["MEDFRDATE"].ToString().Trim(), "D") + " ~ " +
            //                        strMEDENDDATE + " [" + dt.Rows[0]["MEDDEPTCD"].ToString().Trim() + "]";

            lblLastDschDate.Text = string.Format("{0} ~ {1} [{2}]",dt.Rows[0]["MEDFRDATE"].ToString().Trim(),
                                    dt.Rows[0]["MEDENDDATE"].ToString().Trim(), dt.Rows[0]["MEDDEPTCD"].ToString().Trim());

            dt.Dispose();
            dt = null;
            panLastDschDate.Visible = true;
        }


        List<ucEmrChart> EmrChartList = new List<ucEmrChart>();
        private void CreateForm(DataRow chartInfo)
        {
            DataTable dt = null;
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            //mstrFormNo = "2550";
            //mstrUpdateNo = "1";

            FormXml[] mFormXml = null;

            mFormXml = FormDesignQuery.GetDataFormXml(chartInfo["FORMNO"].ToString(), chartInfo["UPDATENO"].ToString());
            if (mFormXml == null)
            {
                return;
            }

            SQL = FormDesignQuery.Query_AEMRFORMXMLandIMAGE(chartInfo["FORMNO"].ToString(), chartInfo["UPDATENO"].ToString());
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            //  기존방식
            //ucEmrChart ucEmrChart = new ucEmrChart(mFormXml, chartInfo, dt);
            //  변경 방식

            clsApi.SuspendDrawing(pnlChart);
            //Application.DoEvents();
      
            ucEmrChart ucEmrChart = new ucEmrChart(dt, chartInfo, ChartTextControlMouseWheel, ChartTextControlMouseDrag);
            ucEmrChart.ChartModify += UcEmrChart_ChartModify;
            //ucEmrChart.ChartReg += UcEmrChart_ChartReg;
            //Application.DoEvents();
            ucEmrChart.Dock = DockStyle.Top;
                
            pnlChart.Controls.Add(ucEmrChart);
            //Application.DoEvents();
            ucEmrChart.BringToFront();
            //Application.DoEvents();
            ChartHeight += ucEmrChart.Height;
            //Application.DoEvents();
            //  차트리스트 추가
            EmrChartList.Add(ucEmrChart);
            //Application.DoEvents();
            //Application.DoEvents();

            clsApi.ResumeDrawing(pnlChart);
            //Application.DoEvents();
        }

        /// <summary>
        /// 차트 수정
        /// </summary>
        /// <param name="emrNo"></param>
        private void UcEmrChart_ChartModify(string emrNo, string formNo, string updateNo)
        {
            pView = clsEmrChart.ClearPatient();
            pView = clsEmrChart.SetEmrPatInfoXml(clsDB.DbCon, emrNo);
            if (pView == null)
            {
                ComFunc.MsgBoxEx(this, "서식지 작성 내역을 찾을 수 없습니다.");
                return;
            }
            pView.formNo = (long)VB.Val(formNo);
            pView.updateNo = (int)VB.Val(updateNo);

            fView = clsEmrChart.ClearEmrForm();
            fView = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, pView.formNo.ToString(), pView.updateNo.ToString());

            //오동호 과장 지난 경과기록 수정 막아달라요청 add 2021 - 06 - 16
            //string strSysDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            //if (clsType.User.IdNumber == "19094" && formNo == "963" && pView.inOutCls == "O")
            //{
            //    if (string.Compare(strSysDate, VB.Replace(pView.medFrDate, "-", "")) > 0)
            //    {
            //        ComFunc.MsgBox("지난날의 경과기록입니다.!!");
            //        return;
            //    }
            //}

            //fView = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFORMNO, "0");
            ViewChart(pView, fView, emrNo, "0", "N", "0", panEmrViewMain);

            //panChartOne.Visible = true;
            //panContinuView.Visible = false;
            //panChartOne.BringToFront();
        }

        private void UcEmrChart_ChartReg(string emrNo, string formNo, string updateNo)
        {
            //string strCmd = string.Empty;
            ////ComFunc.MsgBoxEx(this, "2019년 8월 중순 2차 EMR 오픈시 사용가능합니다.");
            ////return;

            //if (strCmd == IMPORT_UNSAVED)
            //{
            //    SaveOrDeleteImport(0, emrNo, "C");
            //}
            //else
            //{
            //    SaveOrDeleteImport(0, emrNo, "D");
            //}
        }

        #region 사용안함
        ////
        //private void SetEmrInfo(string strACPNO, string strCLSNO)
        //{
        //    int i = 0;
        //    DataTable dt = null;
        //    string SQL = string.Empty;    //Query문
        //    string SqlErr = string.Empty; //에러문 받는 변수
            
        //    ssProgHis_Sheet1.RowCount = 0;

        //    if (mPTNO == "") return;

        //    if (chkAll.Checked == false && chkOrd.Checked == false && chkDis.Checked == false && chkChart0.Checked == false
        //            && chkChart1.Checked == false && chkChart2.Checked == false && chkChart3.Checked == false && chkChart4.Checked == false)
        //    {
        //        ComFunc.MsgBoxEx(this, "조건을 선택해 주십시요.");
        //        return;
        //    }

        //    bool ChkChartBefore = false;
        //    bool ChkChart = false;

        //    #region //Query

        //    #region //상단쿼리
        //    SQL = string.Empty;
        //    SQL = " SELECT  ";
        //    SQL = SQL + ComNum.VBLF + "    X.SORT, ";
        //    SQL = SQL + ComNum.VBLF + "    X.GB, ";
        //    SQL = SQL + ComNum.VBLF + "    X.INOUTCLS, ";
        //    SQL = SQL + ComNum.VBLF + "    X.MEDFRDATE, ";
        //    SQL = SQL + ComNum.VBLF + "    X.MEDDEPTCD,  ";
        //    SQL = SQL + ComNum.VBLF + "    X.MEDDRCD, ";
        //    SQL = SQL + ComNum.VBLF + "    X.BDATE, ";
        //    SQL = SQL + ComNum.VBLF + "    X.CODE,  ";
        //    SQL = SQL + ComNum.VBLF + "    X.NAMEK,  ";
        //    SQL = SQL + ComNum.VBLF + "    X.NAMEE, ";
        //    SQL = SQL + ComNum.VBLF + "    X.CHARTDEPTCD, ";
        //    SQL = SQL + ComNum.VBLF + "    X.CHRARTUSEID,  ";
        //    SQL = SQL + ComNum.VBLF + "    X.CHARTDATE,  ";
        //    SQL = SQL + ComNum.VBLF + "    X.CHARTTIME, ";
        //    SQL = SQL + ComNum.VBLF + "    X.CHARTDATA, ";
        //    SQL = SQL + ComNum.VBLF + "    X.EMRNO, ";
        //    SQL = SQL + ComNum.VBLF + "    X.FORMNO, ";
        //    SQL = SQL + ComNum.VBLF + "    X.UPDATENO, ";
        //    SQL = SQL + ComNum.VBLF + "    X.ENTDATE , ";
        //    SQL = SQL + ComNum.VBLF + "    X.GBORD, ";
        //    SQL = SQL + ComNum.VBLF + "    X.GBORDERNM, ";
        //    SQL = SQL + ComNum.VBLF + "    X.BUN, ";
        //    SQL = SQL + ComNum.VBLF + "    X.REALQTY,  ";
        //    SQL = SQL + ComNum.VBLF + "    X.GbDiv,  ";
        //    SQL = SQL + ComNum.VBLF + "    X.Nal,  ";
        //    SQL = SQL + ComNum.VBLF + "    X.Contents,  ";
        //    SQL = SQL + ComNum.VBLF + "    X.BContents, ";
        //    SQL = SQL + ComNum.VBLF + "    X.GbOrder, ";
        //    SQL = SQL + ComNum.VBLF + "    X.Seqno, ";
        //    SQL = SQL + ComNum.VBLF + "    X.SUCODE, ";
        //    SQL = SQL + ComNum.VBLF + "    U.USENAME AS CHARTUSENAME, ";
        //    SQL = SQL + ComNum.VBLF + "    CASE ";
        //    SQL = SQL + ComNum.VBLF + "         WHEN X.GB = '초진' THEN " + ComNum.DB_EMR + "OLDCHARTCOMPACT(X.FORMNO, X.EMRNO)";
        //    SQL = SQL + ComNum.VBLF + "         ELSE NULL";
        //    SQL = SQL + ComNum.VBLF + "    END AS C_SUMMARY, ";
        //    SQL = SQL + ComNum.VBLF + " CASE  ";
        //    SQL = SQL + ComNum.VBLF + "         WHEN X.GB = '초진' THEN KOSMOS_EMR.OLDCHARTCOMPACT(X.FORMNO, X.EMRNO) ";
        //    SQL = SQL + ComNum.VBLF + "         ELSE NULL ";
        //    SQL = SQL + ComNum.VBLF + "    END AS C_SUMMARY, ";
        //    SQL = SQL + ComNum.VBLF + "    CASE  ";
        //    SQL = SQL + ComNum.VBLF + "        WHEN X.GB = '진단' THEN 0 ";
        //    SQL = SQL + ComNum.VBLF + "        WHEN X.GB = '처방' THEN 0 ";
        //    SQL = SQL + ComNum.VBLF + "        ELSE (SELECT IM.EMRNO FROM KOSMOS_EMR.AEMRCHARTIMPORT IM ";
        //    SQL = SQL + ComNum.VBLF + "                WHERE IM.IDNUMBER = '15200' AND IM.PTNO = '" + mPTNO + "' AND IM.EMRNO = X.EMRNO) ";
        //    SQL = SQL + ComNum.VBLF + "    END AS C_IMPORT, ";
        //    SQL = SQL + ComNum.VBLF + "    CASE  ";
        //    SQL = SQL + ComNum.VBLF + "        WHEN X.GB = '진단' THEN 0 ";
        //    SQL = SQL + ComNum.VBLF + "        WHEN X.GB = '처방' THEN 0 ";
        //    SQL = SQL + ComNum.VBLF + "        ELSE (SELECT IM.EMRNO FROM KOSMOS_EMR.AEMRCHARTIMPORTREMARK IM ";
        //    SQL = SQL + ComNum.VBLF + "                WHERE IM.IDNUMBER = '15200' AND IM.PTNO = '" + mPTNO + "' AND IM.EMRNO = X.EMRNO) ";
        //    SQL = SQL + ComNum.VBLF + "    END AS C_IMPORT_REMARK ";
        //    #endregion //상단쿼리

        //    SQL = SQL + ComNum.VBLF + "FROM ( ";

        //    #region //진단
        //    if (chkDis.Checked == true)
        //    {
        //        SQL = SQL + ComNum.VBLF + "        --외래진단 ";
        //        SQL = SQL + ComNum.VBLF + "        SELECT  ";
        //        SQL = SQL + ComNum.VBLF + "            0 AS SORT, ";
        //        SQL = SQL + ComNum.VBLF + "            '진단' AS GB, ";
        //        SQL = SQL + ComNum.VBLF + "            'O' AS INOUTCLS, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDFRDATE, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDEPTCD,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDRCD, ";
        //        SQL = SQL + ComNum.VBLF + "            TO_CHAR(D.BDate,'YYYYMMDD') AS BDATE, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(D.ILLCODE) AS CODE,  ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(B.ILLNAMEK) AS NAMEK,  ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(B.ILLNAMEE) AS NAMEE, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(D.DEPTCODE) AS CHARTDEPTCD, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHRARTUSEID,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDATE,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTTIME, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDATA, ";
        //        SQL = SQL + ComNum.VBLF + "            0 AS EMRNO, ";
        //        SQL = SQL + ComNum.VBLF + "            0 AS FORMNO, ";
        //        SQL = SQL + ComNum.VBLF + "            0 AS UPDATENO, ";
        //        SQL = SQL + ComNum.VBLF + "            TO_CHAR(D.BDate,'YYYYMMDD') AS ENTDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS  GBORD, ";
        //        SQL = SQL + ComNum.VBLF + "            '정규' AS GBORDERNM, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS BUN, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS REALQTY,  ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS GbDiv,  ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS Nal,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS Contents,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS BContents, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS GbOrder, ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS Seqno, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS SUCODE ";
        //        SQL = SQL + ComNum.VBLF + "        FROM " + ComNum.DB_MED + "OCS_OILLS D ";
        //        SQL = SQL + ComNum.VBLF + "        INNER JOIN " + ComNum.DB_PMPA + "BAS_ILLS B ";
        //        SQL = SQL + ComNum.VBLF + "            ON D.IllCode  = B.IllCode ";
        //        SQL = SQL + ComNum.VBLF + "            AND B.IllCLASS = '1' ";
        //        SQL = SQL + ComNum.VBLF + "        WHERE D.PTNO = '" + mPTNO + "' ";
        //        SQL = SQL + ComNum.VBLF + "        AND D.BDate = TO_DATE('2015-08-20','YYYY-MM-DD') ";
        //        SQL = SQL + ComNum.VBLF + "        UNION ALL ";
        //        SQL = SQL + ComNum.VBLF + "        --입원진단 ";
        //        SQL = SQL + ComNum.VBLF + "        SELECT  ";
        //        SQL = SQL + ComNum.VBLF + "            0 AS SORT, ";
        //        SQL = SQL + ComNum.VBLF + "            '진단' AS GB, ";
        //        SQL = SQL + ComNum.VBLF + "            'I' AS INOUTCLS, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDFRDATE, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDEPTCD,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDRCD, ";
        //        SQL = SQL + ComNum.VBLF + "            TO_CHAR(D.BDate,'YYYYMMDD') AS BDATE, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(D.ILLCODE) AS CODE,  ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(B.ILLNAMEK) AS NAMEK,  ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(B.ILLNAMEE) AS NAMEE, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(D.DEPTCODE) AS CHARTDEPTCD, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHRARTUSEID,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDATE,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTTIME, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDATA, ";
        //        SQL = SQL + ComNum.VBLF + "            0 AS EMRNO, ";
        //        SQL = SQL + ComNum.VBLF + "            0 AS FORMNO, ";
        //        SQL = SQL + ComNum.VBLF + "            0 AS UPDATENO, ";
        //        SQL = SQL + ComNum.VBLF + "            TO_CHAR(D.BDate,'YYYYMMDD') AS ENTDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS  GBORD, ";
        //        SQL = SQL + ComNum.VBLF + "            '정규' AS GBORDERNM, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS BUN, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS REALQTY,  ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS GbDiv,  ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS Nal,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS Contents,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS BContents, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS GbOrder, ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS Seqno, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS SUCODE ";
        //        SQL = SQL + ComNum.VBLF + "        FROM " + ComNum.DB_MED + "OCS_IILLS D ";
        //        SQL = SQL + ComNum.VBLF + "        INNER JOIN " + ComNum.DB_PMPA + "BAS_ILLS B ";
        //        SQL = SQL + ComNum.VBLF + "            ON D.IllCode  = B.IllCode ";
        //        SQL = SQL + ComNum.VBLF + "            AND B.IllCLASS = '1' ";
        //        SQL = SQL + ComNum.VBLF + "        WHERE D.PTNO = '" + mPTNO + "' ";
        //        SQL = SQL + ComNum.VBLF + "        AND D.EntDate = TO_DATE('2015-08-20','YYYY-MM-DD') ";
        //        ChkChartBefore = true;
        //    }

        //    #endregion //진단

        //    #region //초진기록지

        //    if (chkAll.Checked == true)
        //    {
        //        ChkChartBefore = true;
        //    }

        //    ChkChart = false;
        //    if (chkAll.Checked == false)
        //    {
        //        if (chkChart0.Checked == true)
        //        {
        //            ChkChart = true;
        //            if (ChkChartBefore == true)
        //            {
        //                SQL = SQL + ComNum.VBLF + "        UNION ALL ";
        //            }
        //            ChkChartBefore = true;
        //        }
        //    }
        //    else
        //    {
        //        ChkChart = true;
        //        SQL = SQL + ComNum.VBLF + "        UNION ALL ";
        //    }

        //    if (ChkChart == true)
        //    {
        //        SQL = SQL + ComNum.VBLF + "        --초진기록지 ";
        //        SQL = SQL + ComNum.VBLF + "        SELECT  ";
        //        SQL = SQL + ComNum.VBLF + "            2 AS SORT, ";
        //        SQL = SQL + ComNum.VBLF + "            '초진' AS GB, ";
        //        SQL = SQL + ComNum.VBLF + "            'I' AS INOUTCLS, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDFRDATE, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDEPTCD,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDRCD, ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTDATE AS BDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CODE,  ";
        //        SQL = SQL + ComNum.VBLF + "            F.FORMNAME AS NAMEK,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS NAMEE, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDEPTCD, ";
        //        SQL = SQL + ComNum.VBLF + "            C.USEID AS CHRARTUSEID,  ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTDATE,  ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTTIME, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDATA, ";
        //        SQL = SQL + ComNum.VBLF + "            C.EMRNO, ";
        //        SQL = SQL + ComNum.VBLF + "            F.FORMNO, ";
        //        SQL = SQL + ComNum.VBLF + "            F.UPDATENO, ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTDATE AS ENTDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS  GBORD, ";
        //        SQL = SQL + ComNum.VBLF + "            '정규' AS GBORDERNM, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS BUN, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS REALQTY,  ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS GbDiv,  ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS Nal,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS Contents,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS BContents, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS GbOrder, ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS Seqno, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS SUCODE ";
        //        SQL = SQL + ComNum.VBLF + "        FROM " + ComNum.DB_EMR + "EMRXML C ";
        //        SQL = SQL + ComNum.VBLF + "        INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F ";
        //        SQL = SQL + ComNum.VBLF + "            ON F.FORMNO = C.FORMNO ";
        //        SQL = SQL + ComNum.VBLF + "            AND F.UPDATENO = 0 ";
        //        SQL = SQL + ComNum.VBLF + "            AND F.GRPFORMNO = '1000' ";
        //        SQL = SQL + ComNum.VBLF + "        WHERE C.PTNO = '" + mPTNO + "' ";
        //        SQL = SQL + ComNum.VBLF + "            AND C.CHARTDATE >= '20150820' ";
        //        SQL = SQL + ComNum.VBLF + "            AND C.INOUTCLS = 'O' ";
        //    }

        //    #endregion //초진기록지

        //    #region //입원기록지
        //    ChkChart = false;
        //    if (chkAll.Checked == false)
        //    {
        //        if (chkChart2.Checked == true)
        //        {
        //            ChkChart = true;
        //            if (ChkChartBefore == true)
        //            {
        //                SQL = SQL + ComNum.VBLF + "        UNION ALL ";
        //            }
        //            ChkChartBefore = true;
        //        }
        //    }
        //    else
        //    {
        //        ChkChart = true;
        //        SQL = SQL + ComNum.VBLF + "        UNION ALL ";
        //    }

        //    if (ChkChart == true)
        //    {
        //        SQL = SQL + ComNum.VBLF + "        --입원기록지 ";
        //        SQL = SQL + ComNum.VBLF + "        SELECT  ";
        //        SQL = SQL + ComNum.VBLF + "            3 AS SORT, ";
        //        SQL = SQL + ComNum.VBLF + "            '입원' AS GB, ";
        //        SQL = SQL + ComNum.VBLF + "            'I' AS INOUTCLS, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDFRDATE, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDEPTCD,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDRCD, ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTDATE AS BDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CODE,  ";
        //        SQL = SQL + ComNum.VBLF + "            F.FORMNAME AS NAMEK,   ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS NAMEE, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDEPTCD, ";
        //        SQL = SQL + ComNum.VBLF + "            C.USEID AS CHRARTUSEID,  ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTDATE,  ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTTIME, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDATA, ";
        //        SQL = SQL + ComNum.VBLF + "            C.EMRNO, ";
        //        SQL = SQL + ComNum.VBLF + "            F.FORMNO, ";
        //        SQL = SQL + ComNum.VBLF + "            F.UPDATENO, ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTDATE AS ENTDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS  GBORD, ";
        //        SQL = SQL + ComNum.VBLF + "            '정규' AS GBORDERNM, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS BUN, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS REALQTY,  ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS GbDiv,  ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS Nal,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS Contents,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS BContents, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS GbOrder, ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS Seqno, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS SUCODE ";
        //        SQL = SQL + ComNum.VBLF + "        FROM " + ComNum.DB_EMR + "EMRXML C ";
        //        SQL = SQL + ComNum.VBLF + "        INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F ";
        //        SQL = SQL + ComNum.VBLF + "            ON F.FORMNO = C.FORMNO ";
        //        SQL = SQL + ComNum.VBLF + "            AND F.UPDATENO = 0 ";
        //        SQL = SQL + ComNum.VBLF + "            AND F.GRPFORMNO = '1002' ";
        //        SQL = SQL + ComNum.VBLF + "        WHERE C.PTNO = '" + mPTNO + "' ";
        //        SQL = SQL + ComNum.VBLF + "            AND C.CHARTDATE >= '20150820' ";
        //        SQL = SQL + ComNum.VBLF + "            AND C.INOUTCLS = 'O' ";
        //    }
        //    #endregion //입원기록지

        //    #region //퇴원기록지
        //    ChkChart = false;
        //    if (chkAll.Checked == false)
        //    {
        //        if (chkChart3.Checked == true)
        //        {
        //            ChkChart = true;
        //            if (ChkChartBefore == true)
        //            {
        //                SQL = SQL + ComNum.VBLF + "        UNION ALL ";
        //            }
        //            ChkChartBefore = true;
        //        }
        //    }
        //    else
        //    {
        //        ChkChart = true;
        //        SQL = SQL + ComNum.VBLF + "        UNION ALL ";
        //    }
        //    if (ChkChart == true)
        //    {
        //        SQL = SQL + ComNum.VBLF + "        --퇴원기록지 ";
        //        SQL = SQL + ComNum.VBLF + "        SELECT  ";
        //        SQL = SQL + ComNum.VBLF + "            4 AS SORT, ";
        //        SQL = SQL + ComNum.VBLF + "            '퇴원' AS GB, ";
        //        SQL = SQL + ComNum.VBLF + "            'I' AS INOUTCLS, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDFRDATE, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDEPTCD,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDRCD, ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTDATE AS BDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CODE,  ";
        //        SQL = SQL + ComNum.VBLF + "            F.FORMNAME AS NAMEK,   ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS NAMEE, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDEPTCD, ";
        //        SQL = SQL + ComNum.VBLF + "            C.USEID AS CHRARTUSEID,  ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTDATE,  ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTTIME, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDATA, ";
        //        SQL = SQL + ComNum.VBLF + "            C.EMRNO, ";
        //        SQL = SQL + ComNum.VBLF + "            F.FORMNO, ";
        //        SQL = SQL + ComNum.VBLF + "            F.UPDATENO, ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTDATE AS ENTDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS  GBORD, ";
        //        SQL = SQL + ComNum.VBLF + "            '정규' AS GBORDERNM, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS BUN, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS REALQTY,  ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS GbDiv,  ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS Nal,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS Contents,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS BContents, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS GbOrder, ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS Seqno, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS SUCODE ";
        //        SQL = SQL + ComNum.VBLF + "        FROM " + ComNum.DB_EMR + "EMRXML C ";
        //        SQL = SQL + ComNum.VBLF + "        INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F ";
        //        SQL = SQL + ComNum.VBLF + "            ON F.FORMNO = C.FORMNO ";
        //        SQL = SQL + ComNum.VBLF + "            AND F.UPDATENO = 0 ";
        //        SQL = SQL + ComNum.VBLF + "            AND F.GRPFORMNO = '1009' ";
        //        SQL = SQL + ComNum.VBLF + "        WHERE C.PTNO = '" + mPTNO + "' ";
        //        SQL = SQL + ComNum.VBLF + "            AND C.CHARTDATE >= '20150820' ";
        //        SQL = SQL + ComNum.VBLF + "            AND C.INOUTCLS = 'O' ";
        //    }
        //    #endregion //퇴원기록지

        //    #region //수술기록지
        //    ChkChart = false;
        //    if (chkAll.Checked == false)
        //    {
        //        if (chkChart4.Checked == true)
        //        {
        //            ChkChart = true;
        //            if (ChkChartBefore == true)
        //            {
        //                SQL = SQL + ComNum.VBLF + "        UNION ALL ";
        //            }
        //            ChkChartBefore = true;
        //        }
        //    }
        //    else
        //    {
        //        ChkChart = true;
        //        SQL = SQL + ComNum.VBLF + "        UNION ALL ";
        //    }
            
        //    if (ChkChart == true)
        //    {
        //        SQL = SQL + ComNum.VBLF + "        --수술기록지 ";
        //        SQL = SQL + ComNum.VBLF + "        SELECT  ";
        //        SQL = SQL + ComNum.VBLF + "            5 AS SORT, ";
        //        SQL = SQL + ComNum.VBLF + "            '수술' AS GB, ";
        //        SQL = SQL + ComNum.VBLF + "            'I' AS INOUTCLS, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDFRDATE, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDEPTCD,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDRCD, ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTDATE AS BDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CODE,  ";
        //        SQL = SQL + ComNum.VBLF + "            F.FORMNAME AS NAMEK,   ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS NAMEE, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDEPTCD, ";
        //        SQL = SQL + ComNum.VBLF + "            C.USEID AS CHRARTUSEID,  ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTDATE,  ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTTIME, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDATA, ";
        //        SQL = SQL + ComNum.VBLF + "            C.EMRNO, ";
        //        SQL = SQL + ComNum.VBLF + "            F.FORMNO, ";
        //        SQL = SQL + ComNum.VBLF + "            F.UPDATENO, ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTDATE AS ENTDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS  GBORD, ";
        //        SQL = SQL + ComNum.VBLF + "            '정규' AS GBORDERNM, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS BUN, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS REALQTY,  ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS GbDiv,  ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS Nal,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS Contents,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS BContents, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS GbOrder, ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS Seqno, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS SUCODE ";
        //        SQL = SQL + ComNum.VBLF + "        FROM " + ComNum.DB_EMR + "EMRXML C ";
        //        SQL = SQL + ComNum.VBLF + "        INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F ";
        //        SQL = SQL + ComNum.VBLF + "            ON F.FORMNO = C.FORMNO ";
        //        SQL = SQL + ComNum.VBLF + "            AND F.UPDATENO = 0 ";
        //        SQL = SQL + ComNum.VBLF + "            AND F.GRPFORMNO = '1004' ";
        //        SQL = SQL + ComNum.VBLF + "        WHERE C.PTNO = '" + mPTNO + "' ";
        //        SQL = SQL + ComNum.VBLF + "            AND C.CHARTDATE >= '20150820' ";
        //        SQL = SQL + ComNum.VBLF + "            AND C.INOUTCLS = 'O' ";
        //    }
        //    #endregion //수술기록지

        //    #region //프로그래스
        //    ChkChart = false;
        //    if (chkAll.Checked == false)
        //    {
        //        if (chkChart1.Checked == true)
        //        {
        //            ChkChart = true;
        //            if (ChkChartBefore == true)
        //            {
        //                SQL = SQL + ComNum.VBLF + "        UNION ALL ";
        //            }
        //            ChkChartBefore = true;
        //        }
        //    }
        //    else
        //    {
        //        ChkChart = true;
        //        SQL = SQL + ComNum.VBLF + "        UNION ALL ";
        //    }
        //    if (ChkChart == true)
        //    {
        //        SQL = SQL + ComNum.VBLF + "        --프로그래스 ";
        //        SQL = SQL + ComNum.VBLF + "        SELECT  ";
        //        SQL = SQL + ComNum.VBLF + "            6 AS SORT, ";
        //        SQL = SQL + ComNum.VBLF + "            '경과' AS GB, ";
        //        SQL = SQL + ComNum.VBLF + "            'I' AS INOUTCLS, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDFRDATE, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDEPTCD,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDRCD, ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTDATE AS BDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CODE,  ";
        //        SQL = SQL + ComNum.VBLF + "            F.FORMNAME AS NAMEK,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS NAMEE, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDEPTCD, ";
        //        SQL = SQL + ComNum.VBLF + "            C.USEID AS CHRARTUSEID,  ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTDATE,  ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTTIME, ";
        //        SQL = SQL + ComNum.VBLF + "            EXTRACTVALUE(CHARTXML, '//ta1') AS CHARTDATA, ";
        //        SQL = SQL + ComNum.VBLF + "            C.EMRNO, ";
        //        SQL = SQL + ComNum.VBLF + "            F.FORMNO, ";
        //        SQL = SQL + ComNum.VBLF + "            F.UPDATENO, ";
        //        SQL = SQL + ComNum.VBLF + "            C.CHARTDATE AS ENTDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS  GBORD, ";
        //        SQL = SQL + ComNum.VBLF + "            '정규' AS GBORDERNM, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS BUN, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS REALQTY,  ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS GbDiv,  ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS Nal,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS Contents,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS BContents, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS GbOrder, ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS Seqno, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS SUCODE ";
        //        SQL = SQL + ComNum.VBLF + "        FROM " + ComNum.DB_EMR + "EMRXML C ";
        //        SQL = SQL + ComNum.VBLF + "        INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F ";
        //        SQL = SQL + ComNum.VBLF + "            ON F.FORMNO = C.FORMNO ";
        //        SQL = SQL + ComNum.VBLF + "            AND F.UPDATENO = 0 ";
        //        SQL = SQL + ComNum.VBLF + "        WHERE C.PTNO = '" + mPTNO + "' ";
        //        SQL = SQL + ComNum.VBLF + "        AND C.CHARTDATE >= '20150820' ";
        //        SQL = SQL + ComNum.VBLF + "        AND C.INOUTCLS = 'O' ";
        //        SQL = SQL + ComNum.VBLF + "        AND C.FORMNO = 963 ";
        //    }
        //    #endregion //프로그래스

        //    #region //처방
        //    ChkChart = false;
        //    if (chkAll.Checked == false)
        //    {
        //        if (chkOrd.Checked == true)
        //        {
        //            ChkChart = true;
        //            if (ChkChartBefore == true)
        //            {
        //                SQL = SQL + ComNum.VBLF + "        UNION ALL ";
        //            }
        //            ChkChartBefore = true;
        //        }
        //    }
        //    else
        //    {
        //        ChkChart = true;
        //        SQL = SQL + ComNum.VBLF + "        UNION ALL ";
        //    }
        //    if (ChkChart == true)
        //    {
        //        SQL = SQL + ComNum.VBLF + "        --외래처방 ";
        //        SQL = SQL + ComNum.VBLF + "        SELECT  ";
        //        SQL = SQL + ComNum.VBLF + "            /*+ index( kosmos_ocs.ocs_iorder INXOCS_IORDER1) */  ";
        //        SQL = SQL + ComNum.VBLF + "            7 AS SORT, ";
        //        SQL = SQL + ComNum.VBLF + "            '처방' AS GB, ";
        //        SQL = SQL + ComNum.VBLF + "            'O' AS INOUTCLS, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDFRDATE, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDEPTCD,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDRCD, ";
        //        SQL = SQL + ComNum.VBLF + "            TO_CHAR(O.BDate,'YYYYMMDD') AS BDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(O.ORDERCODE) AS CODE, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(C.ORDERNAME) AS NAMEK, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(C.ORDERNAME) AS NAMEE, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(O.DEPTCODE) AS CHARTDEPTCD, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(O.DRCODE) AS CHRARTUSEID,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDATE,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTTIME, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDATA, ";
        //        SQL = SQL + ComNum.VBLF + "            0 AS EMRNO, ";
        //        SQL = SQL + ComNum.VBLF + "            0 AS FORMNO, ";
        //        SQL = SQL + ComNum.VBLF + "            0 AS UPDATENO, ";
        //        SQL = SQL + ComNum.VBLF + "            TO_CHAR(O.EntDate,'YYYYMMDD') AS ENTDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            1 AS  GBORD, ";
        //        SQL = SQL + ComNum.VBLF + "            '정규' AS GBORDERNM, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(O.Bun) AS BUN, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(O.RealQty) AS REALQTY,  ";
        //        SQL = SQL + ComNum.VBLF + "            O.GbDiv,  ";
        //        SQL = SQL + ComNum.VBLF + "            O.Nal,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS Contents,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS BContents, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS GbOrder, ";
        //        SQL = SQL + ComNum.VBLF + "            O.Seqno, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(C.SUCODE) AS SUCODE     ";
        //        SQL = SQL + ComNum.VBLF + "        FROM " + ComNum.DB_MED + "OCS_OORDER O   ";
        //        SQL = SQL + ComNum.VBLF + "        LEFT OUTER JOIN " + ComNum.DB_MED + "OCS_ORDERCODE C ";
        //        SQL = SQL + ComNum.VBLF + "            ON O.ORDERCODE = C.ORDERCODE ";
        //        SQL = SQL + ComNum.VBLF + "        LEFT OUTER JOIN " + ComNum.DB_MED + "OCS_DOCTOR D ";
        //        SQL = SQL + ComNum.VBLF + "            ON O.DRCODE = D.Sabun ";
        //        SQL = SQL + ComNum.VBLF + "        WHERE O.Ptno     = '" + mPTNO + "' ";
        //        SQL = SQL + ComNum.VBLF + "          AND O.BDate >= TO_DATE('2015-08-20','YYYY-MM-DD') ";
        //        SQL = SQL + ComNum.VBLF + "          AND O.BDate <= TO_DATE('2015-08-20','YYYY-MM-DD') ";
        //        SQL = SQL + ComNum.VBLF + "          AND O.GbSunap = '1' ";
        //        SQL = SQL + ComNum.VBLF + "        UNION ALL ";
        //        SQL = SQL + ComNum.VBLF + "        --입원처방 ";
        //        SQL = SQL + ComNum.VBLF + "        SELECT  ";
        //        SQL = SQL + ComNum.VBLF + "            /*+ index( kosmos_ocs.ocs_iorder INXOCS_IORDER1) */  ";
        //        SQL = SQL + ComNum.VBLF + "            8 AS SORT, ";
        //        SQL = SQL + ComNum.VBLF + "            '처방' AS GB, ";
        //        SQL = SQL + ComNum.VBLF + "            'I' AS INOUTCLS, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDFRDATE, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDEPTCD,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS MEDDRCD, ";
        //        SQL = SQL + ComNum.VBLF + "            TO_CHAR(O.BDate,'YYYYMMDD') AS BDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(O.ORDERCODE) AS CODE, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(C.ORDERNAME) AS NAMEK, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(C.ORDERNAME) AS NAMEE, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(O.DEPTCODE) AS CHARTDEPTCD, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(O.DRCODE) AS CHRARTUSEID,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDATE,  ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTTIME, ";
        //        SQL = SQL + ComNum.VBLF + "            '' AS CHARTDATA, ";
        //        SQL = SQL + ComNum.VBLF + "            0 AS EMRNO, ";
        //        SQL = SQL + ComNum.VBLF + "            0 AS FORMNO, ";
        //        SQL = SQL + ComNum.VBLF + "            0 AS UPDATENO, ";
        //        SQL = SQL + ComNum.VBLF + "            TO_CHAR(O.EntDate,'YYYYMMDD') AS ENTDATE , ";
        //        SQL = SQL + ComNum.VBLF + "            CASE  ";
        //        SQL = SQL + ComNum.VBLF + "                WHEN O.GbOrder = 'F' THEN 2 ";
        //        SQL = SQL + ComNum.VBLF + "                WHEN O.GbOrder = 'T' THEN 3 ";
        //        SQL = SQL + ComNum.VBLF + "                WHEN O.GbOrder = 'M' THEN 0 ";
        //        SQL = SQL + ComNum.VBLF + "                ELSE 1 ";
        //        SQL = SQL + ComNum.VBLF + "            END AS GBORD, ";
        //        SQL = SQL + ComNum.VBLF + "            CASE  ";
        //        SQL = SQL + ComNum.VBLF + "                WHEN O.GbOrder = 'F' THEN 'Pre' ";
        //        SQL = SQL + ComNum.VBLF + "                WHEN O.GbOrder = 'T' THEN 'Post' ";
        //        SQL = SQL + ComNum.VBLF + "                WHEN O.GbOrder = 'M' THEN 'Adm' ";
        //        SQL = SQL + ComNum.VBLF + "                ELSE '정규' ";
        //        SQL = SQL + ComNum.VBLF + "            END AS GBORDERNM, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(O.Bun) AS BUN, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(O.RealQty) AS REALQTY,  ";
        //        SQL = SQL + ComNum.VBLF + "            O.GbDiv,  ";
        //        SQL = SQL + ComNum.VBLF + "            O.Nal,  ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(O.Contents) AS Contents,  ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(O.BContents) AS BContents, ";
        //        SQL = SQL + ComNum.VBLF + "            O.GbOrder, ";
        //        SQL = SQL + ComNum.VBLF + "            O.Seqno, ";
        //        SQL = SQL + ComNum.VBLF + "            TRIM(C.SUCODE) AS SUCODE     ";
        //        SQL = SQL + ComNum.VBLF + "        FROM " + ComNum.DB_MED + "OCS_IORDER O   ";
        //        SQL = SQL + ComNum.VBLF + "        LEFT OUTER JOIN " + ComNum.DB_MED + "OCS_ORDERCODE C ";
        //        SQL = SQL + ComNum.VBLF + "            ON O.ORDERCODE = C.ORDERCODE ";
        //        SQL = SQL + ComNum.VBLF + "        LEFT OUTER JOIN " + ComNum.DB_MED + "OCS_DOCTOR D ";
        //        SQL = SQL + ComNum.VBLF + "            ON O.DRCODE = D.Sabun ";
        //        SQL = SQL + ComNum.VBLF + "        WHERE O.Ptno     = '" + mPTNO + "' ";
        //        SQL = SQL + ComNum.VBLF + "          AND O.BDate >= TO_DATE('2015-08-20','YYYY-MM-DD') ";
        //        SQL = SQL + ComNum.VBLF + "          AND O.BDate <= TO_DATE('2015-08-20','YYYY-MM-DD') ";
        //        SQL = SQL + ComNum.VBLF + "          AND O.GbStatus IN (' ','D','D+')   ";
        //        SQL = SQL + ComNum.VBLF + "          AND O.OrderSite Not Like 'DC%'  ";
        //        SQL = SQL + ComNum.VBLF + "          AND O.OrderSite <>  'CAN'  ";
        //    }
        //    #endregion //처방

        //    SQL = SQL + ComNum.VBLF + "    ) X ";
        //    SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER U ";
        //    SQL = SQL + ComNum.VBLF + "    ON X.CHRARTUSEID = U.USEID ";
        //    SQL = SQL + ComNum.VBLF + "ORDER BY X.INOUTCLS DESC, X.BDATE DESC, X.SORT, X.SEQNO ";

        //    #endregion

        //    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

        //    if (SqlErr != "")
        //    {
        //        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
        //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //        Cursor.Current = Cursors.Default;
        //        return;
        //    }
        //    if (dt.Rows.Count == 0)
        //    {
        //        dt.Dispose();
        //        dt = null;
        //        //ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
        //        Cursor.Current = Cursors.Default;
        //        return;
        //    }

        //    foreach(DataRow row in dt.Rows)
        //    {

        //    }

        //    return;
        //    FarPoint.Win.Spread.CellType.TextCellType TypeTextSingle = new FarPoint.Win.Spread.CellType.TextCellType();
        //    TypeTextSingle.Multiline = false;
        //    TypeTextSingle.MaxLength = 9999;
        //    TypeTextSingle.WordWrap = false;

        //    FarPoint.Win.Spread.CellType.TextCellType TypeTextMulti = new FarPoint.Win.Spread.CellType.TextCellType();
        //    TypeTextMulti.Multiline = true;
        //    TypeTextMulti.MaxLength = 99999;
        //    TypeTextMulti.WordWrap = true;
        //    TypeTextMulti.ScrollBars = ScrollBars.Vertical;
            
        //    string strGb = string.Empty;
        //    string strConts = string.Empty;
        //    string strUSERFORMNO = string.Empty;
        //    string strCurDate = string.Empty;
        //    string sFileName = string.Empty;
        //    string strMEDFRDATE = string.Empty;
        //    string strORDDATE = string.Empty;
        //    int CellHeight = 550;
            
        //    for (i = 0; i < dt.Rows.Count; i++)
        //    {
        //        string sTitle = string.Empty;
        //        if (strGb != dt.Rows[i]["GB"].ToString().Trim())
        //        {
        //            ssProgHis_Sheet1.RowCount = ssProgHis_Sheet1.RowCount + 1;
        //            ssProgHis_Sheet1.SetRowHeight(ssProgHis_Sheet1.RowCount - 1, 24);
        //            ComView(dt, ssProgHis_Sheet1.RowCount - 1, i);  //공통표시

        //            if (dt.Rows[i]["GB"].ToString().Trim() == "초진" || dt.Rows[i]["GB"].ToString().Trim() == "입원"
        //                || dt.Rows[i]["GB"].ToString().Trim() == "퇴원" || dt.Rows[i]["GB"].ToString().Trim() == "수술"
        //                || dt.Rows[i]["GB"].ToString().Trim() == "경과")
        //            {
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].ColumnSpan = 3;

        //                ButtonView(dt, i, ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.C_UPDATE + 1);
        //                ButtonView(dt, i, ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.C_IMPORT + 1);
        //                ButtonView(dt, i, ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.C_IMP_REMARK + 1);

        //                sTitle = dt.Rows[i]["NAMEK"].ToString().Trim() + " [" + ComFunc.FormatStrToDate(dt.Rows[i]["BDATE"].ToString().Trim(), "D")
        //                        + "] [" + dt.Rows[i]["CHARTUSENAME"].ToString().Trim() + "]";
        //            }
        //            else
        //            {
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].ColumnSpan = 6;
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHARTUSENAME].Text = string.Empty;

        //                sTitle = dt.Rows[i]["GB"].ToString().Trim() + " : " + ComFunc.FormatStrToDate(dt.Rows[i]["BDATE"].ToString().Trim(), "D");
        //            }
                    
        //            ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].Font = new Font("굴림체", 9, FontStyle.Bold);
        //            ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].CellType = TypeTextSingle;
        //            ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //            ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
        //            ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].Locked = true;
        //            ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].Text = sTitle;

        //            ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.HEADYN].Text = "Y";
                    

        //        }
        //        else
        //        {
        //            if (dt.Rows[i]["GB"].ToString().Trim() == "초진" || dt.Rows[i]["GB"].ToString().Trim() == "입원"
        //                || dt.Rows[i]["GB"].ToString().Trim() == "퇴원" || dt.Rows[i]["GB"].ToString().Trim() == "수술"
        //                || dt.Rows[i]["GB"].ToString().Trim() == "경과")
        //            {
        //                ssProgHis_Sheet1.RowCount = ssProgHis_Sheet1.RowCount + 1;
        //                ssProgHis_Sheet1.SetRowHeight(ssProgHis_Sheet1.RowCount - 1, 24);
        //                ComView(dt, ssProgHis_Sheet1.RowCount - 1, i);  //공통표시

        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].ColumnSpan = 3;

        //                ButtonView(dt, i, ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.C_UPDATE + 1);
        //                ButtonView(dt, i, ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.C_IMPORT + 1);
        //                ButtonView(dt, i, ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.C_IMP_REMARK + 1);

        //                sTitle = dt.Rows[i]["NAMEK"].ToString().Trim() + " [" + ComFunc.FormatStrToDate(dt.Rows[i]["BDATE"].ToString().Trim(), "D")
        //                        + "] [" + dt.Rows[i]["CHARTUSENAME"].ToString().Trim() + "]";

        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].Font = new Font("굴림체", 9, FontStyle.Bold);
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].CellType = TypeTextSingle;
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].Locked = true;
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].Text = sTitle;

        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.HEADYN].Text = "Y";

        //            }
        //            else
        //            {
        //                if (strORDDATE != dt.Rows[i]["BDATE"].ToString().Trim())
        //                {
        //                    ssProgHis_Sheet1.RowCount = ssProgHis_Sheet1.RowCount + 1;
        //                    ssProgHis_Sheet1.SetRowHeight(ssProgHis_Sheet1.RowCount - 1, 24);
        //                    ComView(dt, ssProgHis_Sheet1.RowCount - 1, i);  //공통표시

        //                    if (dt.Rows[i]["GB"].ToString().Trim() == "초진" || dt.Rows[i]["GB"].ToString().Trim() == "입원"
        //                    || dt.Rows[i]["GB"].ToString().Trim() == "퇴원" || dt.Rows[i]["GB"].ToString().Trim() == "수술"
        //                    || dt.Rows[i]["GB"].ToString().Trim() == "경과")
        //                    {
        //                        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].ColumnSpan = 3;

        //                        ButtonView(dt, i, ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.C_UPDATE + 1);
        //                        ButtonView(dt, i, ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.C_IMPORT + 1);
        //                        ButtonView(dt, i, ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.C_IMP_REMARK + 1);

        //                        sTitle = dt.Rows[i]["NAMEK"].ToString().Trim() + " [" + ComFunc.FormatStrToDate(dt.Rows[i]["BDATE"].ToString().Trim(), "D")
        //                            + "] [" + dt.Rows[i]["CHARTUSENAME"].ToString().Trim() + "]";
        //                    }
        //                    else
        //                    {
        //                        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].ColumnSpan = 6;
        //                        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHARTUSENAME].Text = string.Empty;

        //                        sTitle = dt.Rows[i]["GB"].ToString().Trim() + " : " + ComFunc.FormatStrToDate(dt.Rows[i]["BDATE"].ToString().Trim(), "D");
        //                    }

        //                    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].Font = new Font("굴림체", 9, FontStyle.Bold);
        //                    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].CellType = TypeTextSingle;
        //                    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //                    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
        //                    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].Locked = true;
        //                    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].Text = sTitle;
        //                    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].ColumnSpan = 2;

        //                    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.HEADYN].Text = "Y";

        //                }
        //            }
        //        }


        //        strGb = dt.Rows[i]["GB"].ToString().Trim();
        //        strORDDATE = dt.Rows[i]["BDATE"].ToString().Trim();
                
        //        //내용표시
        //        ssProgHis_Sheet1.RowCount = ssProgHis_Sheet1.RowCount + 1;
        //        ComView(dt, ssProgHis_Sheet1.RowCount - 1, i);  //공통표시

        //        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].ColumnSpan = (int)clsEmrType.ChartView.C_IMP_REMARK ;

        //        if ("경과" == dt.Rows[i]["GB"].ToString().Trim())
        //        {
        //            ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Font = new Font("굴림체", 9, FontStyle.Regular);
        //            ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].CellType = TypeTextMulti;
        //            ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //            ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
        //            ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Locked = false;
        //            ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Text = dt.Rows[i]["CHARTDATA"].ToString().Trim();

        //            if (CellHeight > Convert.ToInt32(ssProgHis_Sheet1.GetPreferredRowHeight(ssProgHis_Sheet1.RowCount - 1)) + 25)
        //            {
        //                ssProgHis_Sheet1.SetRowHeight(ssProgHis_Sheet1.RowCount - 1, Convert.ToInt32(ssProgHis_Sheet1.GetPreferredRowHeight(ssProgHis_Sheet1.RowCount - 1)) + 25);
        //            }
        //            else
        //            {
        //                ssProgHis_Sheet1.SetRowHeight(ssProgHis_Sheet1.RowCount - 1, CellHeight);
        //            }
        //        }
        //        else
        //        {
        //            string strName = string.Empty;

        //            if ("진단" == dt.Rows[i]["GB"].ToString().Trim())
        //            {
        //                strName = ComFunc.SetFillString(dt.Rows[i]["CODE"].ToString().Trim(), "R", 8, " ") + dt.Rows[i]["NAMEK"].ToString().Trim();

        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Font = new Font("굴림체", 9, FontStyle.Regular);
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].CellType = TypeTextSingle;
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Locked = true;
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Text = strName;
        //                ssProgHis_Sheet1.SetRowHeight(ssProgHis_Sheet1.RowCount - 1, 24);
        //            }
        //            else if ("처방" == dt.Rows[i]["GB"].ToString().Trim())
        //            {

        //                strName = ComFunc.SetFillString(dt.Rows[i]["CODE"].ToString().Trim(), "R", 6, " ")
        //                        + ComFunc.SetFillString(VB.Left(dt.Rows[i]["NAMEK"].ToString().Trim(), 20), "R", 22, " ")
        //                        + ComFunc.SetFillString(dt.Rows[i]["REALQTY"].ToString().Trim(), "R", 8, " ")
        //                        + ComFunc.SetFillString(dt.Rows[i]["GbDiv"].ToString().Trim(), "R", 3, " ")
        //                        + ComFunc.SetFillString(dt.Rows[i]["Nal"].ToString().Trim(), "R", 3, " ");

        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Font = new Font("굴림체", 9, FontStyle.Regular);
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].CellType = TypeTextSingle;
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Locked = true;
        //                ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Text = strName;
        //                ssProgHis_Sheet1.SetRowHeight(ssProgHis_Sheet1.RowCount - 1, 24);
        //            }
        //            else
        //            {
        //                if (dt.Rows[i]["GB"].ToString().Trim() == "초진" || dt.Rows[i]["GB"].ToString().Trim() == "입원"
        //                    || dt.Rows[i]["GB"].ToString().Trim() == "퇴원" || dt.Rows[i]["GB"].ToString().Trim() == "수술")
        //                {
        //                    if (dt.Rows[i]["GB"].ToString().Trim() == "초진")
        //                    {
        //                        strName = ConvertSummaryToString(dt.Rows[i]["C_SUMMARY"].ToString().Trim());

        //                        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Font = new Font("굴림체", 9, FontStyle.Regular);
        //                        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].CellType = TypeTextMulti;
        //                        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //                        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
        //                        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Locked = false;
        //                        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Text = strName;
        //                        if (CellHeight > Convert.ToInt32(ssProgHis_Sheet1.GetPreferredRowHeight(ssProgHis_Sheet1.RowCount - 1)) + 25)
        //                        {
        //                            ssProgHis_Sheet1.SetRowHeight(ssProgHis_Sheet1.RowCount - 1, Convert.ToInt32(ssProgHis_Sheet1.GetPreferredRowHeight(ssProgHis_Sheet1.RowCount - 1)) + 25);
        //                        }
        //                        else
        //                        {
        //                            ssProgHis_Sheet1.SetRowHeight(ssProgHis_Sheet1.RowCount - 1, CellHeight);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        strName = dt.Rows[i]["NAMEK"].ToString().Trim();

        //                        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Font = new Font("굴림체", 9, FontStyle.Regular);
        //                        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].CellType = TypeTextSingle;
        //                        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //                        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
        //                        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Locked = true;
        //                        ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Text = strName;
        //                        ssProgHis_Sheet1.SetRowHeight(ssProgHis_Sheet1.RowCount - 1, 24);
        //                    }
        //                }
        //                else
        //                {
        //                    strName = dt.Rows[i]["NAMEK"].ToString().Trim();

        //                    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Font = new Font("굴림체", 9, FontStyle.Regular);
        //                    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].CellType = TypeTextSingle;
        //                    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //                    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
        //                    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Locked = true;
        //                    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Text = strName;
        //                    ssProgHis_Sheet1.SetRowHeight(ssProgHis_Sheet1.RowCount - 1, 24);
        //                }
        //            }
        //        }
        //    }
        //    dt.Dispose();
        //    dt = null;
        //}
        //private void ButtonView(DataTable dt, int i,  int Row, int Column)
        //{
        //    string strText = string.Empty;
        //    string strTag = string.Empty;
        //    bool FontBold = false;
        //    Color pForColor = Color.Black;

        //    if (Column == (int)clsEmrType.ChartView.C_UPDATE + 1)
        //    {
        //        strText = "수정";
        //        FontBold = true;
        //    }
        //    else if (Column == (int)clsEmrType.ChartView.C_IMPORT + 1)  //중요차트
        //    {
        //        if (VB.Val(dt.Rows[i]["C_IMPORT"].ToString().Trim()) != 0 )
        //        {
        //            strText = IMPORT_SAVED;
        //            pForColor = Color.Red;
        //            FontBold = true;
        //            strTag = (VB.Val(dt.Rows[i]["C_IMPORT"].ToString().Trim())).ToString();
        //        }
        //        else
        //        {
        //            strText = IMPORT_UNSAVED;
        //        }
        //    }
        //    else if (Column == (int)clsEmrType.ChartView.C_IMP_REMARK + 1) //주석차트
        //    {
        //        if (VB.Val(dt.Rows[i]["C_IMPORT_REMARK"].ToString().Trim()) != 0)
        //        {
        //            strText = REMARK_SAVED;
        //            pForColor = Color.Red;
        //            FontBold = true;
        //            strTag = (VB.Val(dt.Rows[i]["C_IMPORT_REMARK"].ToString().Trim())).ToString();
        //        }
        //        else
        //        {
        //            strText = REMARK_UNSAVED;
        //        }
        //    }

        //    ButtonViewSub(Row, Column, strText, strTag, FontBold, pForColor);
        //}

        //private void ButtonViewSub(int Row, int Column, string strText, string strTag, bool FontBold, Color pForColor)
        //{
        //    FarPoint.Win.Spread.CellType.TextCellType TypeTextSingle = new FarPoint.Win.Spread.CellType.TextCellType();
        //    TypeTextSingle.Multiline = false;
        //    TypeTextSingle.MaxLength = 9999;
        //    TypeTextSingle.WordWrap = false;

        //    if (FontBold == true)
        //    {
        //        ssProgHis_Sheet1.Cells[Row, Column].Font = new Font("굴림체", 9, FontStyle.Bold);
        //    }
        //    else
        //    {
        //        ssProgHis_Sheet1.Cells[Row, Column].Font = new Font("굴림체", 9, FontStyle.Regular);
        //    }

        //    ssProgHis_Sheet1.Cells[Row, Column].CellType = TypeTextSingle;
        //    ssProgHis_Sheet1.Cells[Row, Column].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //    ssProgHis_Sheet1.Cells[Row, Column].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
        //    ssProgHis_Sheet1.Cells[Row, Column].Locked = true;
        //    ssProgHis_Sheet1.Cells[Row, Column].Text = strText;
        //    ssProgHis_Sheet1.Cells[Row, Column].Tag = strTag;
        //    ssProgHis_Sheet1.Cells[Row, Column].ForeColor = pForColor;
        //}

        //private void ComView(DataTable dt, int Row, int i)
        //{
        //    ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.GB].Text = (dt.Rows[i]["GB"].ToString() + "").Trim();
        //    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].Locked = true;
        //    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHARTUSENAME].Text = dt.Rows[i]["CHARTUSENAME"].ToString().Trim();
        //    ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHARTUSENAME].Locked = true;
        //    // ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.PTNO].Text = dt.Rows[i]["PTNO"].ToString().Trim();
        //    // ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.ACPNO].Text = dt.Rows[i]["ACPNO"].ToString().Trim();
        //    ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.INOUTCLS].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
        //    ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.MEDFRDATE].Text = dt.Rows[i]["MEDFRDATE"].ToString().Trim();
        //    ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.MEDDEPTCD].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
        //   // ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.MEDDEPTNAME].Text = dt.Rows[i]["MEDDEPTNAME"].ToString().Trim();
        //    ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.MEDDRCD].Text = dt.Rows[i]["MEDDRCD"].ToString().Trim();
        //   // ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.MEDDRNAME].Text = dt.Rows[i]["MEDDRNAME"].ToString().Trim();
        //    ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.EMRNO].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
        //   // ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.CHARTUSEID].Text = dt.Rows[i]["CHARTUSEID"].ToString().Trim();
        //    ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.FORMNO].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
        //    ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.UPDATENO].Text = dt.Rows[i]["UPDATENO"].ToString().Trim();
        //    ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.CHARTDATE].Text = dt.Rows[i]["CHARTDATE"].ToString().Trim();
        //    ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.CHARTTIME].Text = dt.Rows[i]["CHARTTIME"].ToString().Trim();
        //    // ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.ORDDATE].Text = dt.Rows[i]["ORDDATE"].ToString().Trim();
        //    // ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.ORDTIME].Text = dt.Rows[i]["ORDTIME"].ToString().Trim();
        //    // ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.ORDNO].Text = dt.Rows[i]["ORDNO"].ToString().Trim();
        //    // ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.ORDSEQNO].Text = dt.Rows[i]["ORDSEQNO"].ToString().Trim();
        //    // ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.ORDGB].Text = dt.Rows[i]["ORDGB"].ToString().Trim();
        //    ssProgHis_Sheet1.Cells[Row, (int)clsEmrType.ChartView.GB_HIDDEN].Text = (dt.Rows[i]["GB"].ToString() + "").Trim();
        //}

        //private string ConvertSummaryToString(string strSS)
        //{
        //    string rtnVal = string.Empty;

        //    if (strSS == "") return rtnVal;

        //    rtnVal = strSS.Replace("||", ComNum.VBLF + ComNum.VBLF);
        //    rtnVal = rtnVal.Replace("^^", ComNum.VBLF);

        //    return rtnVal;
        //}

        ///// <summary>
        ///// 사용안함 -- 지우지 마시요
        ///// </summary>
        ///// <param name="strSS"></param>
        ///// <returns></returns>
        //private string ConvertSummaryToRtf(string strSS)
        //{
        //    string rtnVal = string.Empty;

        //    string strHead = @"{\rtf1\ansi\ansicpg949\deff0\deflang1033\deflangfe1042{\fonttbl{\f0\fnil\fcharset129 \'b8\'bc\'c0\'ba \'b0\'ed\'b5\'f1;}}
        //                        {\colortbl ;\red0\green77\blue187;}
        //                        {\*\generator Msftedit 5.41.21.2510;}\viewkind4\uc1\pard\sa200\sl276\slmult1\cf1\lang18\b\f0\fs24";
        //    string strTitleTail = @"\cf0\b0\fs20\par";
        //    string strContentTail = @"\par";
        //    string strLineTail = @"\cf1\b\fs24 ";
        //    string strEnd = "}";

        //    string[] arryLine = VB.Split(strSS,"||");

        //    if (arryLine == null) return rtnVal;

        //    rtnVal = rtnVal + strHead;

        //    for (int i = 0; i < arryLine.Length; i++ )
        //    {
        //        string strTit = ComFunc.SptChar("", 0, "^^");
        //        string strContent = ComFunc.SptChar("", 0, "^^");

        //        if (i != 0)
        //        {
        //            rtnVal = rtnVal + strLineTail;
        //        }

        //        rtnVal = rtnVal + strTit + strTitleTail;
        //        rtnVal = rtnVal + strTit + strContentTail;
        //    }

        //    rtnVal = rtnVal + strEnd;
        //    return rtnVal;


        //    #region //Rtf 형식은 한글이 문제가 됨..
        //    //strName = ConvertSummaryToRtf(dt.Rows[i]["C_SUMMARY"].ToString().Trim());
        //    //strName = @"{\rtf1\ansi\ansicpg949\deff0\deflang1033\deflangfe1042{\fonttbl{\f0\fnil\fcharset129 \'b8\'bc\'c0\'ba \'b0\'ed\'b5\'f1;}}
        //    //        {\colortbl ;\red0\green77\blue187;}
        //    //        {\*\generator Msftedit 5.41.21.2510;}\viewkind4\uc1\pard\sa200\sl276\slmult1\cf1\lang18\b\f0\fs24 CC\cf0\b0\fs20\par
        //    //        FFFFFFF\par
        //    //        \cf1\b\fs24 DDDDDD\cf0\b0\fs20\par
        //    //        \'bf\'ec\'b8\'ae\'b4\'c2 \'c0\'da\'b6\'fb\'bd\'ba\'b7\'b1\par
        //    //        \cf1\b\fs24 CC\cf0\b0\fs20\par
        //    //        FFFFFFF\par
        //    //        }";
        //    //FarPoint.Win.Spread.CellType.RichTextCellType rtf = new FarPoint.Win.Spread.CellType.RichTextCellType();
        //    //rtf.WordWrap = true;
        //    //rtf.Multiline = true;
        //    //rtf.AllowArrowKeysMoveActiveCell = FarPoint.Win.Spread.FlagArrowKeys.All;

        //    //ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.GB].Text = (dt.Rows[i]["GB"].ToString() + "").Trim();
        //    //ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Font = new Font("굴림체", 9, FontStyle.Regular);
        //    //ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].CellType = rtf;
        //    //ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //    //ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
        //    //ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Locked = false;
        //    //ssProgHis_Sheet1.Cells[ssProgHis_Sheet1.RowCount - 1, (int)clsEmrType.ChartView.CHART].Value = strName;
        //    //if (CellHeight > Convert.ToInt32(ssProgHis_Sheet1.GetPreferredRowHeight(ssProgHis_Sheet1.RowCount - 1)) + 25)
        //    //{
        //    //    ssProgHis_Sheet1.SetRowHeight(ssProgHis_Sheet1.RowCount - 1, Convert.ToInt32(ssProgHis_Sheet1.GetPreferredRowHeight(ssProgHis_Sheet1.RowCount - 1)) + 25);
        //    //}
        //    //else
        //    //{
        //    //    ssProgHis_Sheet1.SetRowHeight(ssProgHis_Sheet1.RowCount - 1, CellHeight);
        //    //}
        //    #endregion

        //}
        #endregion

        private void ssProgHis_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssProgHis_Sheet1.RowCount <= 0) return;
            if (e.ColumnHeader == true) return;

            string strGB = ssProgHis_Sheet1.Cells[e.Row, (int)clsEmrType.ChartView.GB_HIDDEN].Text.Trim();
            string strCmd = ssProgHis_Sheet1.Cells[e.Row, e.Column].Text.Trim();
            string strFORMNO = ssProgHis_Sheet1.Cells[e.Row, (int)clsEmrType.ChartView.FORMNO].Text.Trim();
            string strUPDATENO = ssProgHis_Sheet1.Cells[e.Row, (int)clsEmrType.ChartView.UPDATENO].Text.Trim();
            string strEMRNO = ssProgHis_Sheet1.Cells[e.Row, (int)clsEmrType.ChartView.EMRNO].Text.Trim();
            string strCHARTDATE = ssProgHis_Sheet1.Cells[e.Row, (int)clsEmrType.ChartView.CHARTDATE].Text.Trim();
            string strHEADYN = ssProgHis_Sheet1.Cells[e.Row, (int)clsEmrType.ChartView.HEADYN].Text.Trim();

            if (strHEADYN != "Y") return;

            if (strCmd == "수정" || strCmd == IMPORT_SAVED || strCmd == REMARK_SAVED || strCmd == IMPORT_UNSAVED || strCmd == REMARK_UNSAVED)
            {

            }
            else
            {
                return;
            }
            if (e.Column == (int)clsEmrType.ChartView.C_UPDATE + 1)  //수정
            {
                if (strGB == "초진" || strGB == "재진" || strGB == "경과" || strGB == "입원" || strGB == "퇴원" || strGB == "수술")
                {
                    pView = clsEmrChart.ClearPatient();
                    pView = clsEmrChart.SetEmrPatInfoXml(clsDB.DbCon, strEMRNO);
                    if (pView == null)
                    {
                        ComFunc.MsgBoxEx(this, "서식지 작성 내역을 찾을 수 없습니다.");
                        return;
                    }
                    pView.formNo = (long)VB.Val(strFORMNO);
                    pView.updateNo = 1;

                    fView = clsEmrChart.ClearEmrForm();
                    fView = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, pView.formNo.ToString(), pView.updateNo.ToString());
                    //fView = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFORMNO, "0");
                    ViewChart(pView, fView, strEMRNO, "0", "N", "0", panEmrViewMain);

                    panChartOne.Visible = true;
                    panContinuView.Visible = false;
                    panChartOne.BringToFront();
                }
            }
            else if (e.Column == (int)clsEmrType.ChartView.C_IMPORT + 1)  //중요차트 등록  Tag로 움직임
            {
                //if (strCmd == IMPORT_UNSAVED)
                //{
                //    SaveOrDeleteImport(e.Row, strEMRNO, "C");
                //}
                //else
                //{
                //    SaveOrDeleteImport(e.Row, strEMRNO, "D");
                //}
            }
            else if (e.Column == (int)clsEmrType.ChartView.C_IMP_REMARK + 1)  //차트 주석  Tag로 움직임
            {
                if (fEmrBaseChartDrawing != null)
                {
                    fEmrBaseChartDrawing.Dispose();
                    fEmrBaseChartDrawing = null;
                }

                pView = clsEmrChart.ClearPatient();
                pView = clsEmrChart.SetEmrPatInfoXml(clsDB.DbCon, strEMRNO);
                if (pView == null)
                {
                    ComFunc.MsgBoxEx(this, "서식지 작성 내역을 찾을 수 없습니다.");
                    return;
                }
                pView.formNo = (long)VB.Val(strFORMNO);
                pView.updateNo = 1;

                fView = clsEmrChart.ClearEmrForm();
                fView = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, pView.formNo.ToString(), pView.updateNo.ToString());

                if (strCmd == REMARK_UNSAVED)
                {
                    ViewChart(pView, fView, strEMRNO, "0", "N", "0", panImageConv);

                    Application.DoEvents();

                    ActiveFormViewChart.ToImageFormMsg("P");
                }

                if (strCmd == REMARK_UNSAVED)
                {
                    fEmrBaseChartDrawing = new frmEmrBaseChartDrawing(e.Row, VB.Val(strEMRNO), fView.FmFORMNAME, strCHARTDATE,"W");
                }
                else
                {
                    fEmrBaseChartDrawing = new frmEmrBaseChartDrawing(e.Row, VB.Val(strEMRNO), fView.FmFORMNAME, strCHARTDATE, "V");
                }
                    
                fEmrBaseChartDrawing.rDeleteChartRemark += new frmEmrBaseChartDrawing.DeleteChartRemark(frmEmrBaseChartDrawing_DeleteChartRemark);
                fEmrBaseChartDrawing.rSaveChartRemark += new frmEmrBaseChartDrawing.SaveChartRemark(frmEmrBaseChartDrawing_SaveChartRemark);
                fEmrBaseChartDrawing.rEventClosed += new frmEmrBaseChartDrawing.EventClosed(frmEmrBaseChartDrawing_EventClosed);
                fEmrBaseChartDrawing.StartPosition = FormStartPosition.CenterParent;
                fEmrBaseChartDrawing.ShowDialog();
            }
            else
            {
                return;
            }
        }

        private void frmEmrBaseChartDrawing_DeleteChartRemark(int Row, double pEMRNO)
        {
            fEmrBaseChartDrawing.Dispose();
            fEmrBaseChartDrawing = null;

            SaveOrDeleteImportRemark(Row, pEMRNO.ToString(), "D");
        }

        private void frmEmrBaseChartDrawing_SaveChartRemark(int Row, double pEMRNO)
        {
            fEmrBaseChartDrawing.Dispose();
            fEmrBaseChartDrawing = null;

            SaveOrDeleteImportRemark(Row, pEMRNO.ToString(), "C");
        }

        private void frmEmrBaseChartDrawing_EventClosed()
        {
            fEmrBaseChartDrawing.Dispose();
            fEmrBaseChartDrawing = null;
        }

        private void SaveOrDeleteImportRemark(int Row, string strEMRNO, string strFlag)
        {
            if (strFlag == "C")
            {
                if (SaveImportRemark(strEMRNO) == true)
                {
                    //ButtonViewSub(Row, (int)clsEmrType.ChartView.C_IMP_REMARK + 1, REMARK_SAVED, strEMRNO, true, Color.Red);
                }
            }
            else
            {
                if (DeleteImportRemark(strEMRNO) == true)
                {
                    //ButtonViewSub(Row, (int)clsEmrType.ChartView.C_IMP_REMARK + 1, REMARK_UNSAVED, "0", false, Color.Black);
                }
            }
        }

        private bool SaveImportRemark(string strEMRNO)
        {
            DataTable dt = null;
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = string.Empty;
                SQL = SQL + ComNum.VBLF + "SELECT EMRNO                                      ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTIMPORTREMARK ";
                SQL = SQL + ComNum.VBLF + " WHERE IDNUMBER = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "   AND PTNO = '" + mPTNO + "'";
                SQL = SQL + ComNum.VBLF + "   AND EMRNO = " + strEMRNO;
                
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

                    SQL = string.Empty;
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTIMPORTREMARK ";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "     IDNUMBER, PTNO, EMRNO, INPDATE, INPTIME";
                    SQL = SQL + ComNum.VBLF + "     )";
                    SQL = SQL + ComNum.VBLF + "VALUES (";
                    SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + mPTNO + "',";
                    SQL = SQL + ComNum.VBLF + "     " + VB.Val(strEMRNO) + ",";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(SYSDATE, 'YYYYMMDD'),";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(SYSDATE, 'HH24MISS')";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private bool DeleteImportRemark(string strEMRNO)
        {
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = string.Empty;
                SQL = SQL + ComNum.VBLF + "DELETE FROM " + ComNum.DB_EMR + "AEMRCHARTIMPORTREMARK ";
                SQL = SQL + ComNum.VBLF + "WHERE IDNUMBER = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "     AND PTNO = '" + mPTNO + "'";
                SQL = SQL + ComNum.VBLF + "     AND EMRNO = " + strEMRNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void ViewChart(EmrPatient tAcp, EmrForm tForm, string strEmrNo, string strTreatNo, string strSCANYN, string strFormCode, Panel ViewPanel)
        {
            string NewRecord = clsEmrQuery.NewArgreeRecord(clsDB.DbCon);
            lblViewFORMNAME.Text = tForm.FmFORMNAME;

            if (ActiveFormView != null)
            {
                ActiveFormView.Dispose();
                ActiveFormView = null;
                ActiveFormViewChart = null;
            }

            if (strSCANYN == "S")
            {
              //  ActiveFormView = new frmScanImageViewNew2(this, strTreatNo, strFormCode, "0");
            }
            else if(tForm.FmFORMNO == 1232 && tForm.FmOLDGB == 1)
            {
                ActiveFormView = new frmEmrBasePictureView(strEmrNo, tAcp, this);
            }
            //경과이미지(서식지), 설명서 등 처리
            else if(NewRecord.Equals("N") && (tForm.FmGRPFORMNO >= 1050 && tForm.FmGRPFORMNO <= 1055 || tForm.FmGRPFORMNO == 1066 || tForm.FmGRPFORMNO == 1068 ||
                    tForm.FmFORMNO == 2148 && tForm.FmOLDGB == 1))
            {
                ActiveFormView = new frmEmrBaseEmrChartOld(this, null, "OLD", "", "", tForm.FmFORMNO.ToString(), tForm.FmFORMNAME, strEmrNo);
                ((frmEmrBaseEmrChartOld)ActiveFormView).rSaveOrDelete += frmEmrBaseEmrChartOld_SaveOrDelete;
                ((frmEmrBaseEmrChartOld)ActiveFormView).rEventClosed += frmEmrBaseEmrChartOld_EventClosed;
            }
            else
            {
                ActiveFormView = new frmEmrChartNew(tForm.FmFORMNO.ToString(), tForm.FmUPDATENO.ToString(), tAcp, strEmrNo, "V", this);
            }

            if (NewRecord.Equals("N"))
            {
                if (tForm.FmGRPFORMNO >= 1050 && tForm.FmGRPFORMNO <= 1055 || tForm.FmGRPFORMNO == 1066 || tForm.FmGRPFORMNO == 1068 || tForm.FmFORMNO == 2148)
                {
                }
                else
                {
                    ActiveFormViewChart = (EmrChartForm)ActiveFormView;
                }
            }
            else
            {
                ActiveFormViewChart = (EmrChartForm)ActiveFormView;
            }


            #region 기존
            //ActiveFormView.TopLevel = false;
            //this.Controls.Add(ActiveFormView);
            //ActiveFormView.Parent = ViewPanel;
            //ActiveFormView.Text = tForm.FmFORMNAME;
            //ActiveFormView.ControlBox = false;
            //ActiveFormView.FormBorderStyle = FormBorderStyle.None;
            //ActiveFormView.Top = 0;
            //ActiveFormView.Left = 0;

            //if (strSCANYN == "S")
            //{
            //    ActiveFormView.WindowState = FormWindowState.Maximized;
            //}
            //else
            //{
            //    if (tForm.FmALIGNGB == 1)   //Left
            //    {
            //        //panOption.Visible = false;
            //        ActiveFormView.Height = ViewPanel.Height - 20;
            //    }
            //    else if (tForm.FmALIGNGB == 2)  //Top
            //    {
            //        //panOption.Visible = false;
            //        ActiveFormView.Width = ViewPanel.Width - 20;
            //    }
            //    else  //None
            //    {
            //        ActiveFormView.Dock = DockStyle.Fill;
            //    }
            //}
            #endregion
            
            #region 신규

            Screen screen = Screen.FromControl(this);

            ActiveFormView.FormClosed += ActiveFormView_FormClosed;
            ActiveFormView.Text = tForm.FmFORMNAME;
            ActiveFormView.StartPosition = FormStartPosition.Manual;
            
            ActiveFormView.Location = new Point(screen.WorkingArea.Right - ActiveFormView.Width - 80, 220);
            #endregion

            ActiveFormView.Show(this);
        }

        /// <summary>
        /// 수정 창 닫힐때 이벤트 체크.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActiveFormView_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ActiveFormView == null)
                return;

            ActiveFormView.Dispose();
            ActiveFormView = null;
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

        private void mbtnClose_Click(object sender, EventArgs e)
        {
            CloseWriteForm();
        }

        private void CloseWriteForm()
        {
            if(ActiveFormView != null)
            {
                ActiveFormView.Dispose();
                ActiveFormView = null;
            }

            //panChartOne.Visible = false;
            //panContinuView.Visible = true;
            //panContinuView.BringToFront();
        }
        
        private void btnPrint_Click(object sender, EventArgs e)
        {
            using (frmEmrProgressView frm = new frmEmrProgressView(this, AcpEmr, pForm, dtpSSDATE, dtpEEDATE))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                //frm.ShowDialog(this);
                frm.Show();
                frm.Hide();
                frm.pPrintForm();
            }
            //pPrintForm();
        }

        private void optViewChartAll_CheckedChanged(object sender, EventArgs e)
        {
            if (optViewChartAll.Checked == true)
            {
                panContinuViewImport.Visible = false;
                panContinuViewAll.Visible = true;
                panContinuViewAll.BringToFront();
            }
        }

        private void optViewChartImport_CheckedChanged(object sender, EventArgs e)
        {
            if (optViewChartImport.Checked == true)
            {
                panContinuViewAll.Visible = false;
                panContinuViewImport.Visible = true;
                panContinuViewImport.BringToFront();
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (btnImport.Text == "중요차트보기")
            {
                btnImport.Text = "전체차트보기";
                isImport = true;
            }
            else
            {
                btnImport.Text = "중요차트보기";
                isImport = false;
            }
            SetContinuView();
        }

        #region 사용안함 주석
        private void btnImport_Click_Old()
        {
            int i = 0;
            double dblEmrNo = 0;

            bool isImport = false;
            if (btnImport.Text == "중요차트보기")
            {
                btnImport.Text = "전체차트보기";
                isImport = true;
            }
            else
            {
                btnImport.Text = "중요차트보기";
            }

            for (i = 0; i < ssProgHis_Sheet1.RowCount; i++)
            {
                double dblImport = 0;
                double dblImportRmk = 0;

                string strHEADYN = ssProgHis_Sheet1.Cells[i, (int)clsEmrType.ChartView.HEADYN].Text.Trim();
                string strGB = ssProgHis_Sheet1.Cells[i, (int)clsEmrType.ChartView.GB_HIDDEN].Text.Trim();

                if (isImport == true)
                {
                    if (strGB == "초진" || strGB == "입원" || strGB == "퇴원" || strGB == "수술" || strGB == "경과")
                    {
                        if (strHEADYN == "Y")
                        {
                            dblEmrNo = 0;

                            if (ssProgHis_Sheet1.Cells[i, (int)clsEmrType.ChartView.C_IMPORT + 1].Tag != null)
                            {
                                dblImport = VB.Val(ssProgHis_Sheet1.Cells[i, (int)clsEmrType.ChartView.C_IMPORT + 1].Tag.ToString());
                            }

                            if (ssProgHis_Sheet1.Cells[i, (int)clsEmrType.ChartView.C_IMP_REMARK + 1].Tag != null)
                            {
                                dblImportRmk = VB.Val(ssProgHis_Sheet1.Cells[i, (int)clsEmrType.ChartView.C_IMP_REMARK + 1].Tag.ToString());
                            }
                            dblEmrNo = VB.Val(ssProgHis_Sheet1.Cells[i, (int)clsEmrType.ChartView.EMRNO].Text);

                            if (dblImport == 0 && dblImportRmk == 0)
                            {
                                ssProgHis_Sheet1.Rows[i].Visible = false;
                                dblEmrNo = 0;
                            }
                        }
                        else
                        {
                            if (dblEmrNo == 0)
                            {
                                ssProgHis_Sheet1.Rows[i].Visible = false;
                            }
                            else
                            {
                                if (dblEmrNo != VB.Val(ssProgHis_Sheet1.Cells[i, (int)clsEmrType.ChartView.EMRNO].Text))
                                {
                                    ssProgHis_Sheet1.Rows[i].Visible = false;
                                }
                            }
                            dblEmrNo = 0;
                        }
                    }
                    else
                    {
                        ssProgHis_Sheet1.Rows[i].Visible = false;
                    }
                }
                else
                {
                    ssProgHis_Sheet1.Rows[i].Visible = true;
                }
            }
        }

        private void frmEmrBaseContinuView_Activated(object sender, EventArgs e)
        {
            
        }

        private void frmEmrBaseContinuView_Deactivate(object sender, EventArgs e)
        {
            
        }

        #endregion

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
        }

        #region 과 등록

        private void BtnSetUserDept_Click(object sender, EventArgs e)
        {
            panUserDept.Top = btnSetUserDept.Top + btnSetUserDept.Height + 40;
            panUserDept.Visible = true;
            panUserDept.BringToFront();
            ssUserDept.Focus();
        }

        private void SsUserDept_Leave(object sender, EventArgs e)
        {
            SaveUserDept();
            GetUserDept();
            panUserDept.Visible = false;
        }


        private bool SaveUserDept()
        {
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = string.Empty;
                SQL = SQL + ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRUSERDEPT";
                SQL = SQL + ComNum.VBLF + " WHERE USEID = '" + clsType.User.IdNumber + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                for (int i = 0; i < ssUserDept_Sheet1.RowCount; i++)
                {
                    if (ssUserDept_Sheet1.Cells[i, 0].Text.Trim() == "True")
                    {
                        string strDeptCode = ssUserDept_Sheet1.Cells[i, 2].Text.Trim();

                        SQL = string.Empty;
                        SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_EMR.EMRUSERDEPT ";
                        SQL = SQL + ComNum.VBLF + "      (USEID, MEDDEPTCD, DISPSEQ, WRITEDATE, WRITETIME)";
                        SQL = SQL + ComNum.VBLF + " VALUES (";
                        SQL = SQL + ComNum.VBLF + "  '" + clsType.User.IdNumber + "',";
                        SQL = SQL + ComNum.VBLF + "  '" + strDeptCode + "',";
                        SQL = SQL + ComNum.VBLF + "  1,";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(SYSDATE, 'YYYYMMDD'),";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(SYSDATE, 'HH24MISS'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void GetUserDept()
        {
            DataTable dt = null;
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            ssUserDept_Sheet1.RowCount = 0;
            try
            {
                SQL = string.Empty;
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "     A.MEDDEPTCD, A.DEPTKORNAME, B.USEID ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.VIEWBMEDDEPT A ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_EMR.EMRUSERDEPT B";
                SQL = SQL + ComNum.VBLF + "    ON A.MEDDEPTCD = B.MEDDEPTCD ";
                SQL = SQL + ComNum.VBLF + "     AND B.USEID = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.DEPTKORNAME, A.PRTGRD ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssUserDept_Sheet1.RowCount = dt.Rows.Count;
                ssUserDept_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["USEID"].ToString().Trim() != "")
                    {
                        ssUserDept_Sheet1.Cells[i, 0].Value = true;
                    }
                    ssUserDept_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DEPTKORNAME"].ToString().Trim();
                    ssUserDept_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                MakeUserDept();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void MakeUserDept()
        {
            StringBuilder builder = new StringBuilder();
            mstrUserDept = string.Empty;
            for (int i = 0; i < ssUserDept_Sheet1.RowCount; i++)
            {
                if (ssUserDept_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                {
                    builder.Append("'" + ssUserDept_Sheet1.Cells[i, 2].Text.Trim() + "',");
                }
            }
            
            if (builder.Length == 0)
            {
                mstrUserDept = "'" + clsType.User.DeptCode + "'";
                return;
            }

            mstrUserDept = builder.Remove(builder.Length - 1, 1).ToString().Trim();
        }

        private void MakeUserDept_19094()
        {
            int i;

            for (i = 0; i < ssUserDept_Sheet1.RowCount; i++)
            {
                ssUserDept_Sheet1.Cells[i, 0].Value = false;
                if (ssUserDept_Sheet1.Cells[i, 2].Text.Trim() == "RA" || ssUserDept_Sheet1.Cells[i, 2].Text.Trim() == "MR")
                {
                    ssUserDept_Sheet1.Cells[i, 0].Value = true;
                }
            }

            mstrUserDept = string.Empty;
            for (i = 0; i < ssUserDept_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssUserDept_Sheet1.Cells[i, 0].Value) == true)
                {
                    mstrUserDept = mstrUserDept + ",'" + ssUserDept_Sheet1.Cells[i, 2].Text.Trim() + "'";
                }
            }
            if (mstrUserDept.Length > 0)
            {
                mstrUserDept = VB.Right(mstrUserDept, mstrUserDept.Length - 1);
            }
        }

        #endregion

        private void BtnExitDept_Click(object sender, EventArgs e)
        {
            panUserDept.Visible = false;
        }

        private void btnSearchContent_Click(object sender, EventArgs e)
        {
            if (pnlChart.Controls.Count == 0)
                return;

            string SearchText = VB.InputBox("검색하실 코드를 입력하세요.", "입력");
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                ComFunc.MsgBoxEx(this, "내용을 입력해주세요");
                return;
            }

            if (lstTextBox == null || lstTextBox != null && lstTextBox.Count == 0)
            {
                lstTextBox = ComFunc.GetAllControls(pnlChart).Where(d => d is TextBox && d.Text.IndexOf(SearchText) != -1).ToList();
            }

            foreach(Control control in lstTextBox)
            {
                pnlChart.ScrollControlIntoView(control);
                control.Focus();
                (control as TextBox).SelectionStart = control.Text.IndexOf(SearchText);
                (control as TextBox).SelectionLength = SearchText.Length;
                lstTextBox.Remove(control);
                break;
            }
        }

        private void chkSortAs2_CheckedChanged(object sender, EventArgs e)
        {
            clsEmrPublic.bOrderAsc = chkSortAs2.Checked;
        }

        private void chkDeptAll_CheckedChanged(object sender, EventArgs e)
        {
            using (RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("EmrSetting"))
            {
                reg.SetValue("CHKDEPTALL", chkDeptAll.Checked ?"1" : "0");
            }
        }

        private void frmEmrBaseContinuView_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (lstTextBox != null && lstTextBox.Count > 0)
            {
                lstTextBox.Clear();
            }

            foreach(Control control in  pnlChart.Controls)
            {
                if (control.IsDisposed == false)
                {
                    control.Dispose();
                }
            }

            if (ActiveFormView != null)
            {
                ActiveFormView.Dispose();
                ActiveFormView = null;
            }

            if (fEmrBaseChartDrawing != null)
            {
                fEmrBaseChartDrawing.Dispose();
                fEmrBaseChartDrawing = null;    
            }
        }
    }
}
