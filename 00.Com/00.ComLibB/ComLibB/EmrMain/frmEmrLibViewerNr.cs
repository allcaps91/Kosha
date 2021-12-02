using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using ComBase;
using ComEmrBase;
using Oracle.DataAccess.Client;

namespace ComLibB
{
    public partial class frmEmrLibViewerNr : Form, MainFormMessage, FormEmrMessage
    {
        #region //마우스이동 변수
        private Point start_p;
        private Point end_p;
        private bool mouse_move = false;
        #endregion //마우스이동 변수

        #region //폼에서 사용하는 변수
        EmrPatient AcpEmr = null;
        EmrForm fWrite = null;

        Form ActiveFormWrite = null;
        EmrChartForm ActiveFormWriteChart = null;

        double mFORMNO_W = 0;
        double mUPDATENO_W = 0;
        #endregion //폼에서 사용하는 변수

        #region //서브폼 선언부

        /// <summary>
        /// 내시경 기록지 작성점검
        /// </summary>
        frmEmrEndoMIBI fEmrEndoMIBI = null;

        /// <summary>
        /// 간호EMR
        /// </summary>
        frmEmrLibViewerNr fEmrLibViewerNr = null;

        /// <summary>
        /// EMR뷰어
        /// </summary>
        frmEmrViewer fEmrViewer = null;

        /// <summary>
        /// 검사결과 조회
        /// </summary>
        frmViewResult fViewResult = null;

        /// <summary>
        /// ORDER 오더지
        /// </summary>
        frmTextEmrOrder fEmrTextEmrOrder = null;

        /// <summary>
        /// CarePlan
        /// </summary>
        frmCarePlan fEmrCarePlan = null;

        /// <summary>
        /// 간호 챠트미비체크
        /// </summary>
        frmNurMibi fEmrNurMibi = null;
        /// <summary>
        /// Vital
        /// </summary>
        Form fEmrPatientState = null; //Vital 
        /// <summary>
        /// 폼조회
        /// </summary>
        frmEmrFormSearch fEmrFormSearch = null;  //
        /// <summary>
        /// EMR 조회
        /// </summary>
        frmEmrBaseChartView fEmrBaseChartView = null;  //
        /// <summary>
        /// 환자 리스트
        /// </summary>
        frmEmrBaseInNrAcpList fEmrBaseInNrAcpList = null;  //
        /// <summary>
        /// 이전내역
        /// </summary>
        frmEmrChartHisList fEmrChartHisList = null;
        /// <summary>
        /// 사용자 서식
        /// </summary>
        frmEmrBaseUserChartForm fEmrBaseUserChartForm = null; //사용자 서식

        private void SubFormToControl(Form frm, Control pControl)
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
            frm.Height = pControl.Height;
            frm.Width = pControl.Width;
            //frm.Dock = DockStyle.Fill;
            frm.Show();

        }
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
        public ComEmrBase.FormEmrMessage mEmrCallForm = null;
        public void MsgSave(string strSaveFlag)
        {
            if (tabEmr.SelectedIndex == 0 && ActiveFormWrite is frmEmrBaseVitalAndActing)
            {
                GetRRSScore();
                return;
            }
            else if (tabEmr.SelectedIndex == 0 && mFORMNO_W == 2641 && ActiveFormWrite is frmEmrChartFlowOld)
            {
                lblCath.Visible = READ_CATH();
            }

            if (fEmrBaseChartView == null)
            {
                return;
            }

            if (strSaveFlag == "ORD")
            {

            }
            else
            {
                fEmrBaseChartView.GetJupHis(AcpEmr.ptNo);
            }

        }
        public void MsgDelete()
        {
            if (tabEmr.SelectedIndex == 0 && mFORMNO_W == 2641 && ActiveFormWrite is frmEmrChartFlowOld)
            {
                lblCath.Visible = READ_CATH();
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

        #region //SubForm Event
        //private void frmEmrBaseContinuView_EventClosed()
        //{
        //    fEmrBaseContinuView.Dispose();
        //    fEmrBaseContinuView = null;
        //}


        private void FEmrViewer_rEventClosed()
        {
            if (fEmrViewer != null)
            {
                fEmrViewer.Dispose();
                fEmrViewer = null;
            }
        }

        private void fEmrLibViewerNr_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fEmrLibViewerNr != null)
            {
                fEmrLibViewerNr.Dispose();
                fEmrLibViewerNr = null;
            }
        }

        private void fEmrEndoMIBI_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fEmrEndoMIBI != null)
            {
                fEmrEndoMIBI.Dispose();
                fEmrEndoMIBI = null;
            }
        }

        private void frmEmrBaseChartView_EventClosed()
        {
            fEmrBaseChartView.Dispose();
            fEmrBaseChartView = null;
        }

        private void frmEmrBaseUserChartForm_EventSetUserChart(double dblMACRONO)
        {
            fEmrBaseUserChartForm.Dispose();
            fEmrBaseUserChartForm = null;

            if (ActiveFormWrite == null)
            {
                return;
            }

            ActiveFormWriteChart.SetUserFormMsg(dblMACRONO);
        }

        private void frmEmrBaseUserChartForm_EventClosed()
        {
            fEmrBaseUserChartForm.Dispose();
            fEmrBaseUserChartForm = null;
        }

        private void fEmrNurMibi_rClosed()
        {
            if (fEmrNurMibi != null)
            {
                fEmrNurMibi.Dispose();
                fEmrNurMibi = null;
            }
        }

        private void FEmrCarePlan_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fEmrCarePlan != null)
            {
                fEmrCarePlan.Dispose();
                fEmrCarePlan = null;
            }
        }

        private void FEmrTextEmrOrder_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fEmrTextEmrOrder != null)
            {
                fEmrTextEmrOrder.Dispose();
                fEmrTextEmrOrder = null;
            }
        }

        private void fViewResult_rEventClosed()
        {
            if (fViewResult != null)
            {
                fViewResult.Dispose();
                fViewResult = null;
            }
        }

        #endregion

        #region //Form 이벤트
        public frmEmrLibViewerNr()
        {
            InitializeComponent();
        }


        public frmEmrLibViewerNr(MainFormMessage pform)
        {
            mCallForm = pform;
            InitializeComponent();
        }

        public frmEmrLibViewerNr(MainFormMessage pform, string strPara)
        {
            mCallForm = pform;
            InitializeComponent();
        }

        //EmrPatient AcpEmr = null;
        //AcpEmr = clsEmrChart.ClearPatient();
        //AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtNo, strInoutCls, strMedFrDate, strDeptCd);
        //if (AcpEmr == null)
        //{
        //    ComFunc.MsgBoxEx(this, ("접수내역을 찾을 수 없습니다.");
        //    return;
        //}
        public frmEmrLibViewerNr(EmrPatient pAcpEmr)
        {
            InitializeComponent();
            AcpEmr = pAcpEmr;
        }

        private void frmEmrLibViewerNr_Load(object sender, EventArgs e)
        {
            //if (clsType.User.IsNurse.Equals("OK") == false)
            //{
            //    if (mCallForm != null)
            //    {
            //        mCallForm.MsgUnloadForm(this);
            //    }
            //    else
            //    {
            //        Close();
            //    }
            //    return;
            //}

            

            //if (clsEmrFunc.GetMenuAuth(this, clsDB.DbCon) == true)
            //{
            //    ShoNewFormChart(true);
            //}
            //else
            //{
            //    ShoNewFormChart(false);
            //}

            if (clsType.User.AuAVIEW == "0")
            {
                ComFunc.MsgBoxEx(this, "조회 권한이 없습니다.");
                Close();
                return;
            }

            #region 작성권한 인증서 점검
            if (clsType.User.AuAWRITE.Equals("1"))
            {
                clsEmrFunc.UserCertCheck(clsDB.DbCon, this);
            }
            #endregion

            if (clsEmrQueryPohangS.CHECK_NUR_SCHEDULE(clsDB.DbCon))
            {
                ComFunc.MsgBoxEx(this, "현재 스케쥴(간호부)은 근무시간이 아닙니다. 프로그램을 종료합니다.");
                Close();
                return;
            }

            FormInit();
            clsEmrPublic.gUserGrade = clsEmrFunc.SET_GRADE();

            toolStripMenuItem37.Visible = clsVbfunc.GetBCodeCODE(clsDB.DbCon, "NUR_간호부관리자사번IP", "").Trim().Equals(clsCompuInfo.gstrCOMIP);
            //신규기록지ToolStripMenuItem.Visible = clsEmrFunc.GetMenuAuth(this, clsDB.DbCon);
            //toolStripMenuItem25.Visible = clsType.User.BuseCode.Equals("033108");
            //eR신규기록지ToolStripMenuItem.Visible = clsType.User.BuseCode.Equals("033109");

            pSetUserOption();

            if (AcpEmr != null)
            {
                SetAcpInfo();
            }
        }

        /// <summary>
        /// 폼초기화
        /// </summary>
        private void FormInit()
        {
            panFormSearch.Left = 306;
            panFormSearch.Top = 46;
            panFormSearch.Width = 520;
            panFormSearch.Height = 771;
            panFormSearch.Visible = false;

            panEmrWrite.Dock = DockStyle.Fill;
            panEmrView.Dock = DockStyle.Fill;
            panEmrView.Visible = false;
            panEmrWrite.BringToFront();

            panEmrWriteMain.Dock = DockStyle.Fill;

            panSideBarLeft.Visible = false;

            tabEmr.SelectedTab = tabView;

            panTopSub.Top = panPatInfo.Height + mnuMain.Height;
            SetFormTitle();

            bool isNewEmrStart = clsEmrQueryEtc.NewEmrStart();

            if (isNewEmrStart == true)
            {
                ShoOldFormChart(false);
            }
            else
            {
                ShoOldFormChart(true);
            }

            ShoNewFormChart(true);
            if (clsType.User.BuseCode.Equals("033109"))
            {
                mnuNewG_4.Visible = false;
            }
            else if(clsType.User.BuseCode.Equals("044510") || clsType.User.BuseCode.Equals("044520"))
            {
                foreach(ToolStripMenuItem menuItem in mnuMain.Items)
                {
                    if (menuItem.Equals(mnuG_22) == false)
                    {
                        menuItem.Visible = false;
                    }
                }
            }


            SetBuseMenu();

            Application.DoEvents();
            LoadSubForm();
            Application.DoEvents();

            tabEmr.SelectedTab = tabWrite;
            
        }

        /// <summary>
        /// 폼 타이틀 세팅
        /// </summary>
        private void SetFormTitle()
        {
            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = "";


            SQL = "";
            SQL = " SELECT  ";
            SQL = SQL + ComNum.VBLF + "    PROJECTNAME, FORMNAME, FORMNAME1, FORMAUTH, FORMAUTHTEL ";
            SQL = SQL + ComNum.VBLF + "FROM BAS_PROJECTFORM ";
            SQL = SQL + ComNum.VBLF + "WHERE FORMNAME = 'frmEmrLibViewerNr'";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            string[] arryAssName = VB.Split(dt.Rows[0]["PROJECTNAME"].ToString().Trim(), ".");
            string strAssName = arryAssName[0];
            strAssName += ".dll";

            string strPROJECTNAME = dt.Rows[0]["PROJECTNAME"].ToString().Trim();
            string strFORMNAME = dt.Rows[0]["FORMNAME"].ToString().Trim();
            string strFORMAUTH = dt.Rows[0]["FORMAUTH"].ToString().Trim();
            string strFORMAUTHTEL = dt.Rows[0]["FORMAUTHTEL"].ToString().Trim();
            dt.Dispose();

            string strUpdateIniFile = @"C:\PSMHEXE\PSMHAutoUpdate.ini";
            clsIniFile myIniFile = new clsIniFile(strUpdateIniFile);
            double dblVerClt = myIniFile.ReadValue("DEFAULT_UPDATE_LIST", strAssName, 0);

            Text = "간호EMR - (" + strFORMAUTH + " ☎ " + strFORMAUTHTEL + ")"
                        + VB.Space(6) + " (" + strPROJECTNAME + " : Ver " + dblVerClt.ToString() + ")"
                        + VB.Space(10) + " 사용자:" + clsType.User.UserName + " (" + clsCompuInfo.gstrCOMIP + ")";
            return;

        }

        private void ShoOldFormChart(bool blnVisible)
        {
            //mnuG_1_1.Visible = blnVisible;  //간호정보조사지
            //mnuG_1_2.Visible = blnVisible;  //간호정보조사지(소아)
            //mnuG_1_3.Visible = blnVisible;  //간호정보조사지(산모)
            //mnuG_1_4.Visible = blnVisible;  //간호정보조사지(신생아)
            //mnuG_1_5.Visible = blnVisible;  //간호정보조사지(정신과)

            //mnuG_2_1.Visible = blnVisible;  //피부사정

            //기본간호활동ToolStripMenuItem.Visible = blnVisible;  //기본간호활동(기본)
            //간호활동신경정신과ToolStripMenuItem.Visible = blnVisible;  //기본간호활동(신경정신과)
            //신체보호간호활동기록지ToolStripMenuItem.Visible = blnVisible;  //신체보호간호활동기록지
            //낙상예방간호활동기록지ToolStripMenuItem.Visible = blnVisible;  //낙상예방간호활동기록지
            //신생아낙상예방간호활동기록지ToolStripMenuItem.Visible = blnVisible;  //신생아낙상예방간호활동기록지
            //통합병동기본간호활동기록지ToolStripMenuItem.Visible = blnVisible;  //통합병동기본간호활동기록지
            //통합병동면담기록지ToolStripMenuItem.Visible = blnVisible;  //통합병동면담기록지
            //욕창예방간호활동기록지ToolStripMenuItem.Visible = blnVisible;  //욕창예방간호활동기록지

            //mnuG_4.Visible = blnVisible;  //활력측정

            //mnuG_5.Visible = blnVisible;  //섭취배설
            //기본섭취배설ToolStripMenuItem.Visible = blnVisible;  //기본섭취배설
            //처방섭취배설ToolStripMenuItem.Visible = blnVisible;  //처방섭취배설
            //nEW섭취배설ToolStripMenuItem.Visible = blnVisible;  //NEW섭취배설

            //간호기록지ToolStripMenuItem.Visible = blnVisible;  //간호기록지

            //mnuG_7_1.Visible = blnVisible;  //퇴원간호

            //전동기록지SendToolStripMenuItem.Visible = blnVisible;  //전동기록지(Send)
            //전동기록지ReceiveToolStripMenuItem.Visible = blnVisible;  //전동기록지(Receive)

            //당뇨기록지ToolStripMenuItem.Visible = blnVisible;  //당뇨기록지
            //피하주사순서ToolStripMenuItem.Visible = blnVisible;  //피하주사순서

            //mnuG_10.Visible = blnVisible;  //통증기록
            //통증초기평가ToolStripMenuItem.Visible = blnVisible;  //통증초기평가
            //통증재평가기록지ToolStripMenuItem.Visible = blnVisible;  //통증재평가기록지

            //상처간호기록지ToolStripMenuItem.Visible = blnVisible;  //상처간호기록지
            //욕창간호기록지ToolStripMenuItem.Visible = blnVisible;  //욕창간호기록지

            //시술검사전기록지sendToolStripMenuItem.Visible = blnVisible;  //시술검사전기록지
            //pREOPCHECKLIST병동ToolStripMenuItem.Visible = blnVisible;  //PREOPCHECKLIST(병동)
            //NEUROSURGICALSPECIALWATCHRECORD.Visible = blnVisible;  //NEUROSURGICALSPECIALWATCHRECORD

            //마취전평가서ToolStripMenuItem.Visible = blnVisible;  //
            //마취전평가서NEWToolStripMenuItem.Visible = blnVisible;  //
            //마취기록지ToolStripMenuItem.Visible = blnVisible;  //
            //pREOPCHECKLIST수술ToolStripMenuItem.Visible = blnVisible;  //
            //마취회복반사능력ToolStripMenuItem.Visible = blnVisible;  //
            //시술검사전기록지ReceiveToolStripMenuItem.Visible = blnVisible;  //
            //회복실기록지ToolStripMenuItem.Visible = blnVisible;  //
            //회복실기록지NEWToolStripMenuItem.Visible = blnVisible;  //
            //vitalSheetToolStripMenuItem.Visible = blnVisible;  //
            //섭취배설기록지ToolStripMenuItem.Visible = blnVisible;  //
            //간호기록ToolStripMenuItem1.Visible = blnVisible;  //

            //수술간호기록지2ToolStripMenuItem.Visible = blnVisible;  //
            //수술간호기록지3ToolStripMenuItem.Visible = blnVisible;  //
            //surgicalSafetyChecklistToolStripMenuItem.Visible = blnVisible;  //

            //혈관조영술및중재술전CheckList병동용ToolStripMenuItem.Visible = blnVisible;  //
            //혈관조영술및중재술전CheckList조영실ToolStripMenuItem.Visible = blnVisible;  //
            //활력측정조영실ToolStripMenuItem.Visible = blnVisible;  //
            //혈관조영실간호기록지ToolStripMenuItem.Visible = blnVisible;  //
            //혈관조영실간호기록지심혈관ToolStripMenuItem.Visible = blnVisible;  //
            //혈관조영실간호기록지뇌혈관toolStripMenuItem.Visible = blnVisible;  //
            //AngioRoomSafetyChecklistToolStripMenuItem.Visible = blnVisible;  //
            //toolStripMenuItem353.Visible = blnVisible;  //

            //mnuG_16.Visible = blnVisible;  //ICU기록 : 전체

            //mnuG_17.Visible = blnVisible; // NSWR

            //수혈기록지ToolStripMenuItem.Visible = blnVisible; //
            //복막투석기록지ToolStripMenuItem.Visible = blnVisible; //
            //혈액투석기록지ToolStripMenuItem.Visible = blnVisible; //
            //혈액투석기록지NewToolStripMenuItem.Visible = blnVisible; //
            //beck자기평가불안척도ToolStripMenuItem.Visible = blnVisible; //
            //스트레스위험도측정밀러박사ToolStripMenuItem.Visible = blnVisible; //
            //투석간호력ToolStripMenuItem.Visible = blnVisible; //
            //혈액투석치료계획서ToolStripMenuItem.Visible = blnVisible; //
            //혈관접근로점검기록지Cath용ToolStripMenuItem.Visible = blnVisible; //
            //혈관접근로점검기록지AVF용ToolStripMenuItem.Visible = blnVisible; //
            //인공신장실VSSheetToolStripMenuItem.Visible = blnVisible; //
            //CRRT기록지속적신대체기록.Visible = blnVisible; //

            //신생아출생정보기록지분만실용ToolStripMenuItem.Visible = blnVisible; //
            //신생아신체사정ToolStripMenuItem.Visible = blnVisible; //
            //신생아간호활동기록지ToolStripMenuItem.Visible = blnVisible; //
            //신생아퇴원교육지시서ToolStripMenuItem.Visible = blnVisible; //
            //신생아확인표ToolStripMenuItem.Visible = blnVisible; //
            //인공호흡기경과기록지ToolStripMenuItem1.Visible = blnVisible; //
            //fEEDINGIOCHARTNew1ToolStripMenuItem.Visible = blnVisible; //
            //신생아출생정보기록지신생아실ToolStripMenuItem.Visible = blnVisible; //
            //신생아간호관리ToolStripMenuItem.Visible = blnVisible; //
            //lABORPROGRESSCHARTToolStripMenuItem.Visible = blnVisible; //
            //분만요약지ToolStripMenuItem.Visible = blnVisible; //

            //eR간호기록ToolStripMenuItem.Visible = blnVisible; //
            //응급센터진료기록ToolStripMenuItem.Visible = blnVisible; //
            //sPECIALWATCHRECORDToolStripMenuItem.Visible = blnVisible; //
            //emergencyMedicineNoteToolStripMenuItem.Visible = blnVisible; //
            //eR간호정보조사지ToolStripMenuItem.Visible = blnVisible; //
            //외상기록지간호ToolStripMenuItem.Visible = blnVisible; //
            //인공호흡기경과기록지ToolStripMenuItem2.Visible = blnVisible; //
            //인공호흡기경과기록지newToolStripMenuItem1.Visible = blnVisible; //

            //toolStripMenuItem30.Visible = blnVisible; //진정 검사 전 기록지
            //toolStripMenuItem32.Visible = blnVisible; //
            //toolStripMenuItem33.Visible = blnVisible; //
            //toolStripMenuItem34.Visible = blnVisible; //
            //toolStripMenuItem35.Visible = blnVisible; //시술/검사전 기록지(Receive)

            //내시경진정검사전기록지ToolStripMenuItem.Visible = blnVisible; //
            //전처치및진정약제투약기록지ToolStripMenuItem.Visible = blnVisible; //
            //환자모니터및진정환자평가ToolStripMenuItem.Visible = blnVisible; //
            //내시경진정회복평가및퇴실시환자교육ToolStripMenuItem.Visible = blnVisible; //
            //시술검사전기록지ReceiveToolStripMenuItem1.Visible = blnVisible; //

            //완화의료간호사조기상담기록지ToolStripMenuItem.Visible = blnVisible; //
            //완화의료간호사경과상담기록지ToolStripMenuItem.Visible = blnVisible; //
            //완화의료사회복지사조기상담기록지ToolStripMenuItem.Visible = blnVisible; //
            //완화의료사회복지사경과상담기록지ToolStripMenuItem.Visible = blnVisible; //
            //요법프로그램일지ToolStripMenuItem.Visible = blnVisible; //
            //암성통증초기평가기록지ToolStripMenuItem.Visible = blnVisible; //
            //연명의료계획서ToolStripMenuItem.Visible = blnVisible; //
            //임종과정기록지ToolStripMenuItem.Visible = blnVisible; //
            //임종과정초기평가기록지ToolStripMenuItem.Visible = blnVisible; //
            //임종확인기록지ToolStripMenuItem.Visible = blnVisible; //
        }

        private void ShoNewFormChart(bool blnVisible)
        {
            mnuNewG_1_1.Visible = blnVisible;  //(신)간호정보조사지
            mnuNewG_1_2.Visible = blnVisible;  //(신)간호정보조사지(소아)
            mnuNewG_1_3.Visible = blnVisible;  //(신)간호정보조사지(산모)
            mnuNewG_1_4.Visible = blnVisible;  //(신)간호정보조사지(신생아)
            mnuNewG_1_5.Visible = false;  //(신)간호정보조사지(정신과) : 사용안함

            mnuNewG_2_1.Visible = blnVisible;  //(신)피부사정

            mnuNewG_3_1.Visible = blnVisible;  //(신)낙상예방간호활동기록지
            mnuNewG_3_2.Visible = blnVisible;  //(신)신생아낙상예방간호활동기록지
            mnuNewG_3_3.Visible = blnVisible;  //(신)통합병동 면담 기록지
            mnuNewG_3_4.Visible = blnVisible;  //(신)욕창 예방 간호활동 기록지

            mnuNewG_4.Visible = blnVisible;  //(신)임상관찰 및 활동기록

            mnuNewG_6_1.Visible = blnVisible;  //(신)간호기록지
            mnuNewG_6_2.Visible = blnVisible;  //(신)투약기록지

            mnuNewG_7_1.Visible = blnVisible;  //(신)퇴원간호

            mnuNewG_8_1.Visible = blnVisible;  //(신)전동기록지(Send)
            mnuNewG_8_2.Visible = blnVisible;  //(신)전동기록지(Receive)

            mnuNewG_9_1.Visible = blnVisible;  //(신)당뇨기록지
            mnuNewG_9_2.Visible = blnVisible;  //(신)피하주사순서

            //mnuNewG_10_1.Visible = blnVisible;  //(신)통증재평가기록지

            mnuNewG_11_1.Visible = blnVisible;  //(신)상처간호기록지
            mnuNewG_11_2.Visible = blnVisible;  //(신)욕창간호기록지

            mnuNewG_12_1.Visible = blnVisible;  //(신)시술/검사전 기록지(send)
            mnuNewG_12_2.Visible = blnVisible;  //(신)PRE-OP CHECKLIST(병동)

            mnuNewG_13_1.Visible = blnVisible;  //(신)마취전평가서(NEW)
            mnuNewG_13_2.Visible = blnVisible;  //(신)마취기록지
            mnuNewG_13_3.Visible = blnVisible;  //(신)PRE-OP CHECKLIST(수술)
            mnuNewG_13_4.Visible = blnVisible;  //(신)시술/검사전 기록지(Receive)
            mnuNewG_13_5.Visible = blnVisible;  //(신)회복실기록지(NEW)
            mnuNewG_13_6.Visible = blnVisible;  //(신)회복실 Vital Sheet

            mnuNewG_13_7.Visible = blnVisible;  //(신)회복실 간호기록

            mnuNewG_14_1.Visible = blnVisible;  //(신)수술간호기록지(3)
            mnuNewG_14_2.Visible = blnVisible;  //(신)Surgical Safety Checklist

            mnuNewG_15_1.Visible = blnVisible;  //(신)활력측정(조영실)
            mnuNewG_15_2.Visible = blnVisible;  //(신)혈관조영실 간호기록지
            mnuNewG_15_3.Visible = blnVisible;  //(신)혈관조영실 간호기록지 (심혈관)
            mnuNewG_15_4.Visible = blnVisible;  //(신)혈관조영실 간호기록지 (뇌혈관)
            mnuNewG_15_5.Visible = blnVisible;  //(신)AngioRoomSafetyChecklist
            mnuNewG_15_6.Visible = blnVisible;  //(신)시술/검사전 기록지(Receive)

            mnuNewG_25_1.Visible = blnVisible;  //(신)중심도관 삽입 bundle
            mnuNewG_25_2.Visible = blnVisible;  //(신)중심도관 유지관리 bundle
            mnuNewG_25_3.Visible = blnVisible;  //(신)중심정맥관 관리기록
            mnuNewG_25_4.Visible = blnVisible;  //(신)인공호흡기 관련 폐렴예방 bundle
            mnuNewG_25_5.Visible = blnVisible;  //(신)유치도뇨관 삽입 bundle
            mnuNewG_25_6.Visible = blnVisible;  //(신)유치도뇨관 유지 bundel

            mnuNewG_18_1.Visible = blnVisible;  //(신)수혈기록지
            mnuNewG_18_2.Visible = blnVisible;  //(신)복막투석 기록지
            mnuNewG_18_3.Visible = blnVisible;  //(신)혈액투석 기록지
            mnuNewG_18_4.Visible = blnVisible;  //(신)투석간호력
            mnuNewG_18_5.Visible = blnVisible;  //(신)혈액투석치료계획서
            mnuNewG_18_6.Visible = blnVisible;  //(신)혈관접근로 점검 기록지(Cath용)

            mnuNewG_19_1.Visible = blnVisible;  //(신)신생아 출생 정보 기록지(분만실용)
            mnuNewG_19_2.Visible = blnVisible;  //(신)신생아 신체 사정
            mnuNewG_19_3.Visible = blnVisible;  //(신)신생아 퇴원교육 지시서
            mnuNewG_19_4.Visible = blnVisible;  //(신)신생아확인표
            mnuNewG_19_5.Visible = blnVisible;  //(신)신생아 출생 정보 기록지(신생아실)
            //mnuNewG_19_6.Visible = blnVisible;  //(신)신생아간호관리
            mnuNewG_19_7.Visible = blnVisible;  //(신)분만요약지

            mnuNewG_20_1.Visible = blnVisible;  //(신)ER간호기록

            mnuNewG_20_2.Visible = blnVisible;  //(신)SPECIAL WATCH RECORD
            mnuNewG_20_3.Visible = blnVisible;  //(신)ER간호정보조사지

            mnuNewG_24_1.Visible = blnVisible;  //(신)진정 검사 전 기록지
            mnuNewG_24_2.Visible = blnVisible;  //(신)환자모니터 및 진정 환자 평가
            mnuNewG_24_3.Visible = blnVisible;  //(신)진정 회복평가 및 퇴실시 환자교육
            mnuNewG_24_4.Visible = blnVisible;  //(신)시술/검사전 기록지(Receive)

            mnuNewG_22_1.Visible = blnVisible;  //(신)내시경(진정)검사전기록지
            mnuNewG_22_2.Visible = blnVisible;  //(신)환자모니터 및 진정 환자 평가
            mnuNewG_22_3.Visible = blnVisible;  //(신)내시경(진정) 회복평가 및 퇴실시 환자교육
            mnuNewG_22_4.Visible = blnVisible;  //(신)시술/검사전 기록지(Receive)

            //mnuNewG_23_1.Visible = blnVisible;  //(신)완화의료 간호사 초기상담 기록지
            mnuNewG_23_2.Visible = blnVisible;  //(신)완화의료 간호사 경과상담 기록지
            //mnuNewG_23_3.Visible = blnVisible;  //(신)완화의료 사회복지사 초기상담 기록지
            mnuNewG_23_4.Visible = blnVisible;  //(신)완화의료 사회복지사 경과상담 기록지
            mnuNewG_23_16.Visible = blnVisible; //(신)완화의료 사회복지사 종결 기록지
            mnuNewG_23_5.Visible = blnVisible;  //(신)요법 프로그램 일지
            mnuNewG_23_6.Visible = blnVisible;  //(신)암성 통증 초기평가 기록지
            mnuNewG_23_7.Visible = blnVisible;  //(신)연명의료 계획서
            mnuNewG_23_8.Visible = blnVisible;  //(신)임종과정 기록지
            mnuNewG_23_9.Visible = blnVisible;  //(신)임종과정 초기평가 기록지
            mnuNewG_23_10.Visible = blnVisible;  //(신)임종확인 기록지
            mnuNewG_23_11.Visible = blnVisible;  //(신)영적 돌봄 초기 면담지
            mnuNewG_23_12.Visible = blnVisible;  //(신)사별가족 관리 기록지
            mnuNewG_23_13.Visible = blnVisible;  //(신)사별 위험도 사정 기록지
        }

        /// <summary>
        /// 각 부서 메뉴 자동 설정
        /// 설정 한 메뉴가 있을경우만 클리어 후 새로 메뉴 등록
        /// 아닐경우 기본 메뉴 설정.
        /// </summary>
        void SetBuseMenu()
        {
            StringBuilder SQL = new StringBuilder();
            //DataTable dt = null;
            OracleDataReader reader = null;

            //46476 
            try
            {
                #region 쿼리
                SQL.AppendLine("SELECT 1 AS CNT ");
                SQL.AppendLine("FROM DUAL");
                SQL.AppendLine("WHERE EXISTS");
                SQL.AppendLine("(");
                SQL.AppendLine("SELECT 1");
                SQL.AppendLine("FROM " + ComNum.DB_PMPA + "BAS_BASCD A");
                SQL.AppendLine("WHERE A.GRPCDB = '간호EMR 관리'");
                SQL.AppendLine("  AND A.GRPCD = '사용여부'");
                SQL.AppendLine("  AND A.BASCD = 'Y'");
                SQL.AppendLine(")");

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    return;
                }

                reader.Dispose();
                #endregion


                #region 쿼리
                SQL.Clear();
                SQL.AppendLine("SELECT ");
                SQL.AppendLine("    A.MENUCODE, A.MENUNAME, A.FORMNO");
                SQL.AppendLine("FROM " + ComNum.DB_PMPA + "BAS_NUREMRMENU A");
                SQL.AppendLine("  INNER JOIN " + ComNum.DB_PMPA + "BAS_BASCD B");
                SQL.AppendLine("     ON B.BASCD = '" + clsType.User.BuseCode + "'");
                SQL.AppendLine("WHERE A.IDNUMBER = B.GRPCD");
                SQL.AppendLine("  AND A.PARENTCODE = 0");
                SQL.AppendLine("  AND A.DELGB = '0'");
                SQL.AppendLine("ORDER BY A.DISPSEQ");

                sqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }
                #endregion

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    return;
                }


                 List<ToolStripItem> lstItem = mnuMain.Items.OfType<ToolStripItem>().Where(f => f.Name.Equals("신규기록지ToolStripMenuItem") == false &&
                        f.Name.Equals("toolStripMenuItem20") == false &&
                        f.Name.Equals("toolStripMenuItem37") == false &&
                        f.Name.Equals("toolStripMenuItem54") == false).ToList();

                for(int i = 0; i < lstItem.Count; i++)
                {
                    mnuMain.Items.Remove(lstItem[i]);
                }
                //mnuMain.Items.Clear();

                int index = 0;
                while(reader.Read())
                {
                    ToolStripMenuItem toolStripItem = new ToolStripMenuItem(reader.GetValue(1).ToString().Trim());
                    mnuMain.Items.Insert(index, toolStripItem);
                    index++;

                    //서식지 번호 있으면 클릭 이벤트 추가
                    if (reader.GetValue(2).ToString().Trim().Length > 0)
                    {
                        toolStripItem.Tag = reader.GetValue(2).ToString().Trim();
                        toolStripItem.Click += mnuFormNew_Click;
                    }
                    //없으면 서브메뉴 추가 함수 이동
                    else
                    {
                        SubMenuAdd(reader.GetValue(0).ToString().Trim(), toolStripItem);
                    }
                }


                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        /// <summary>
        /// 메뉴에 서브메뉴 추가 하는 함수
        /// </summary>
        /// <param name="strMenuCd">메뉴 코드</param>
        /// <param name="toolStripMenuItem">메뉴 아이템 객체</param>
        void SubMenuAdd(string strMenuCd, ToolStripMenuItem toolStripMenuItem)
        {
            StringBuilder SQL = new StringBuilder();
            OracleDataReader reader = null;

            #region 서브 메뉴 추가

            SQL.Clear();
            SQL.AppendLine("SELECT MENUCODE, MENUNAME, FORMNO");
            SQL.AppendLine(", CASE WHEN EXISTS(");
            SQL.AppendLine("   SELECT 1");
            SQL.AppendLine("   FROM  KOSMOS_PMPA.BAS_NUREMRMENU");
            SQL.AppendLine("   WHERE PARENTCODE = A.MENUCODE");
            SQL.AppendLine("     AND DELGB ='0'");
            SQL.AppendLine("  ) THEN '1' END CNT");
            SQL.AppendLine("FROM " + ComNum.DB_PMPA + "BAS_NUREMRMENU A");
            SQL.AppendLine("WHERE PARENTCODE = " + strMenuCd);
            SQL.AppendLine("  AND DELGB = '0'");
            SQL.AppendLine("ORDER BY DISPSEQ");

            string sqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, sqlErr);
                return;
            }

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem(reader.GetValue(1).ToString());
                    toolStripMenuItem.DropDownItems.Add(menuItem);
                    //서브메뉴 없을때
                    if (string.IsNullOrWhiteSpace(reader.GetValue(3).ToString()))
                    {
                        menuItem.Tag = reader.GetValue(2).ToString();
                        menuItem.Click += mnuFormNew_Click;
                    }
                    else
                    {
                        SubMenuAdd(reader.GetValue(0).ToString(), menuItem);
                    }
                }
            }

            reader.Dispose();
            #endregion
        }

        private void LoadSubForm()
        {
            fEmrBaseInNrAcpList = new frmEmrBaseInNrAcpList();
            fEmrBaseInNrAcpList.rSetEmrAcpInfo += new frmEmrBaseInNrAcpList.SetEmrAcpInfo(frmEmrBaseInNrAcpList_SetEmrAcpInfo);
            if (fEmrBaseInNrAcpList != null)
            {
                SubFormToControl(fEmrBaseInNrAcpList, panAcpMain);
            }

            fEmrBaseChartView = new frmEmrBaseChartView();
            //fEmrBaseChartView.rEventClosed += new frmEmrBaseChartView.EventClosed(frmEmrBaseChartView_EventClosed);
            if (fEmrBaseChartView != null)
            {
                SubFormToControl(fEmrBaseChartView, panEmrView);
            }
        }

        private void LoadSubForm_ChartView()
        {
            fEmrBaseChartView = new frmEmrBaseChartView();
            //fEmrBaseChartView.rEventClosed += new frmEmrBaseChartView.EventClosed(frmEmrBaseChartView_EventClosed);
            if (fEmrBaseChartView != null)
            {
                SubFormToControl(fEmrBaseChartView, panEmrView);
            }
        }

        private void frmEmrBaseInNrAcpList_SetEmrAcpInfo(EmrPatient tAcp)
        {
            AcpEmr = null;
            AcpEmr = tAcp;

            //BST사 자동발생 자동 결과값생성하여 처방 발생 확인창 제거 2021.08.06
            //READ_MSG2(tAcp.ptNo);

            if (tAcp.acpNo.Equals("0"))
            {
                clsImgcvt.NEW_PohangTreatInterface(clsDB.DbCon, this, tAcp.ptNo);
                tAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, tAcp.ptNo, tAcp.inOutCls, tAcp.medFrDate, tAcp.medDeptCd);
            }

            lblDressing.Text = READ_DRESSING(tAcp.ptNo, tAcp.medFrDate);
            lblDressing.Visible = lblDressing.Text.Length > 0;

            lblSelfMed.Visible = READ_SELFMED(tAcp.ptNo, tAcp.medFrDate);

            lblCath.Visible = READ_CATH();

            GetRRSScore();
            SetAcpInfo();
        }

        #region NEW_PohangTreatInterface
        public static bool NEW_PohangTreatInterface(Form msgForm, string strPatid)
        {
            #region 변수
            bool rtnVal = false;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;
            OracleDataReader reader = null;
            OracleDataReader reader2 = null;
            #endregion

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                #region 쿼리
                SQL = " SELECT P.PANO, P.SNAME, P.SEX, P.JUMIN1||P.JUMIN2  JUMIN, E.PATID , E.ROWID " +
                      "   FROM KOSMOS_PMPA.BAS_PATIENT  P , KOSMOS_EMR.EMR_PATIENTT E" +
                      " WHERE E.PATID (+)=P.PANO AND " +
                      "  P.PANO ='" + strPatid.Trim() + "' ";
                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(msgForm, SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return rtnVal;
                }


                if (reader.Read())
                {
                    #region EMR_PATIENTT 테이블에 환자가 없다.
                    if (reader.GetValue(4).ToString().Trim().Length == 0)
                    {
                        SQL = "INSERT INTO KOSMOS_EMR.EMR_PATIENTT(PATID, JUMINNO, NAME, SEX  ) " + " " +
                              " VALUES('" + reader.GetValue(0).ToString().Trim() + "' ," +
                              "'" + VB.Left(reader.GetValue(3).ToString().Trim(), 7) + "******" + "', " +
                              "'" + reader.GetValue(1).ToString().Trim() + "', " +
                              "'" + reader.GetValue(2).ToString().Trim() + "', " +
                              " ) ";
                    }
                    else
                    {
                        SQL = "UPDATE KOSMOS_EMR.EMR_PATIENTT" + " ";
                        SQL += ComNum.VBLF + "  SET NAME = '" + reader.GetValue(1).ToString().Trim() + "'";
                        SQL += ComNum.VBLF + "    , SEX  = '" + reader.GetValue(2).ToString().Trim() + "'";
                        SQL += ComNum.VBLF + "    , JUMINNO = '" + VB.Left(reader.GetValue(3).ToString().Trim(), 7) + "******" + "' ";
                        SQL += ComNum.VBLF + "  WHERE ROWID = '" + reader.GetValue(5).ToString().Trim() + "' ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBoxEx(msgForm, SqlErr);
                        return rtnVal;
                    }
                    #endregion
                }

                reader.Dispose();

                #region 진료정보 가져오기 외래 입원 따로 가져오기.
                SQL = "SELECT m.pano, TO_CHAR(M.BDATE, 'YYYYMMDD') Bdate ,m.deptcode, d.sabun, M.ROWID   " +
                      " from kosmos_pmpa.opd_master m,  kosmos_ocs.VIEW_ocs_doctor_NEW d " +
                      "  where d.drcode = m.drcode AND M.BDATE >= TO_DATE('2005-01-01', 'YYYY-MM-DD') " +
                      "  and  PANO = '" + strPatid.Trim() + "'" +
                      "  AND EMR ='0'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(msgForm, SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string strDept = reader.GetValue(2).ToString().Trim();

                        #region 서브쿼리
                        SQL = "SELECT TREATNO, ROWID  FROM KOSMOS_EMR.EMR_TREATT ";
                        SQL += ComNum.VBLF + "  WHERE PATID = '" + reader.GetValue(0).ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND INDATE  ='" + reader.GetValue(1).ToString().Trim() + "'";
                        SQL += ComNum.VBLF + "    AND CLINCODE = '" + strDept + "'";
                        SQL += ComNum.VBLF + "    AND CLASS = 'O'";

                        SqlErr = clsDB.GetAdoRs(ref reader2, SQL, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            ComFunc.MsgBoxEx(msgForm, SqlErr);
                            return rtnVal;
                        }

                        if (reader2.HasRows == false)
                        {
                            SQL = "INSERT INTO KOSMOS_EMR.EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE, OUTDATE, DOCCODE, ";
                            SQL += ComNum.VBLF + " ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED";
                            SQL += ComNum.VBLF + " ) values(KOSMOS_EMR.SEQ_TREATNO.NEXTVAL, '" + (strPatid).Trim() + "' ,";
                            SQL += ComNum.VBLF + "'O' ,";//  'CLASS;
                            SQL += ComNum.VBLF + "'" + reader.GetValue(1).ToString().Trim() + "' ,";// 'INDATE
                            SQL += ComNum.VBLF + "'" + strDept + "' ,";// 'CLINCODE
                            SQL += ComNum.VBLF + "'' ,";//                          'OUTDATE
                            SQL += ComNum.VBLF + "'" + VB.Val(reader.GetValue(3).ToString().Trim()) + "',  ";// 'DOCCODE
                            SQL += ComNum.VBLF + "'0',  ";//                         'ERFLAG
                            SQL += ComNum.VBLF + "'000000',  ";//                       'INITTIME
                            SQL += ComNum.VBLF + "'" + (strPatid).Trim() + "',  ";//     'OLDPATID
                            SQL += ComNum.VBLF + "'2',  ";//      'FST
                            SQL += ComNum.VBLF + "'',  ";//                           'WARD
                            SQL += ComNum.VBLF + "'', ";//                            'ROOM
                            SQL += ComNum.VBLF + "'1' )";//                          'COMPLETE
                        }
                        else
                        {
                            if (reader2.Read())
                            {
                                SQL = " UPDATE KOSMOS_EMR.EMR_TREATT SET ";
                                SQL += ComNum.VBLF + "  DOCCODE = '" + VB.Val(reader.GetValue(3).ToString().Trim()) + "'";
                                SQL += ComNum.VBLF + "  WHERE ROWID = '" + reader2.GetValue(1).ToString().Trim() + "' ";
                            }

                        }
                        reader2.Dispose();

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            ComFunc.MsgBoxEx(msgForm, SqlErr);
                            return rtnVal;
                        }
                        #endregion

                        #region ocs서버 업데이트. 적용시점에 새로 시작
                        SQL = " UPDATE kosmos_pmpa.opd_master SET   EMR = '1' WHERE ROWID = '" + reader.GetValue(4).ToString().Trim() + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            ComFunc.MsgBoxEx(msgForm, SqlErr);
                            return rtnVal;
                        }
                        #endregion
                    }
                }

                reader.Dispose();
                #endregion


                #region 입원
                string strOK = string.Empty;

                SQL = " SELECT  S.PANO, TO_CHAR(S.INDATE, 'YYYYMMDD') INDATE,  TO_CHAR(S.OUTDATE, 'YYYYMMDD') OUTDATE, S.DeptCode, S.ROWID,  D.SABUN ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.ipd_new_master S, kosmos_ocs.ocs_doctor d ";
                SQL += ComNum.VBLF + "  WHERE S.DrCode = d.drcode ";
                SQL += ComNum.VBLF + "    AND S.PANO = '" + strPatid + "' ";
                SQL += ComNum.VBLF + "    AND (S.EMR = '0'  OR S.EMR IS NULL)";// '나중에 적용

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(msgForm, SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    //clsDB.setRollbackTran(pDbCon);
                    clsDB.setCommitTran(clsDB.DbCon);
                    return rtnVal;
                }

                while (reader.Read())
                {
                    strOK = "OK";

                    if (reader.GetValue(5).ToString().Trim().Length == 0)
                    {
                        strOK = "NO";
                    }

                    string strDept = reader.GetValue(3).ToString().Trim();


                    #region 서브 쿼리
                    SQL = "SELECT TREATNO, ROWID  FROM KOSMOS_EMR.EMR_TREATT ";
                    SQL += ComNum.VBLF + " WHERE PATID  = '" + strPatid + "'";
                    SQL += ComNum.VBLF + "   AND INDATE = '" + reader.GetValue(1).ToString().Trim() + "'";
                    SQL += ComNum.VBLF + "   AND CLASS  = 'I'";

                    SqlErr = clsDB.GetAdoRs(ref reader2, SQL, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBoxEx(msgForm, SqlErr);
                        return rtnVal;
                    }

                    if (reader2.HasRows == false)
                    {
                        SQL = "INSERT INTO KOSMOS_EMR.EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE, OUTDATE, DOCCODE, ";
                        SQL += ComNum.VBLF + " ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED ) ";
                        SQL += ComNum.VBLF + " VALUES(KOSMOS_EMR.SEQ_TREATNO.NEXTVAL, '" + (strPatid).Trim() + "' ,";
                        SQL += ComNum.VBLF + "'I' ,";//                          'CLASS
                        SQL += ComNum.VBLF + "'" + reader.GetValue(1).ToString().Trim() + "' ,";// 'INDATE
                        SQL += ComNum.VBLF + "'" + strDept + "' ,";// 'CLINCODE
                        SQL += ComNum.VBLF + "'" + reader.GetValue(2).ToString().Trim() + "' ,";// 'OUTDATE
                        SQL += ComNum.VBLF + "'" + VB.Val(reader.GetValue(5).ToString().Trim()) + "',  ";// 'DOCCODE
                        SQL += ComNum.VBLF + "'0',  ";//                         'ERFLAG
                        SQL += ComNum.VBLF + "'000000',  ";//                      'INITTIME
                        SQL += ComNum.VBLF + "'" + (strPatid).Trim() + "',  ";//    'OLDPATID
                        SQL += ComNum.VBLF + "'2',  ";//     'FST
                        SQL += ComNum.VBLF + "'',  ";//                           'WARD
                        SQL += ComNum.VBLF + "'', ";//                            'ROOM
                        SQL += ComNum.VBLF + "'1') ";//                           'COMPLETE
                    }
                    else
                    {
                        if (reader2.Read())
                        {
                            SQL = " UPDATE KOSMOS_EMR.EMR_TREATT SET ";
                            SQL += ComNum.VBLF + "   CLINCODE = '" + strDept + "' ,";
                            SQL += ComNum.VBLF + "   DOCCODE = '" + VB.Val(reader.GetValue(5).ToString().Trim()) + "' ,";
                            SQL += ComNum.VBLF + "   OUTDATE = '" + reader.GetValue(2).ToString().Trim() + "' ";
                            SQL += ComNum.VBLF + "  WHERE ROWID = '" + reader2.GetValue(1).ToString().Trim() + "'";
                        }
                    }

                    reader2.Dispose();

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBoxEx(msgForm, SqlErr);
                        return rtnVal;
                    }
                    #endregion

                    #region ocs서버 업데이트. 사용시 풀기
                    SQL = " UPDATE kosmos_pmpa.MID_SUMMARY SET  EMR = '1'";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + reader.GetValue(4).ToString().Trim() + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBoxEx(msgForm, SqlErr);
                        return rtnVal;
                    }
                    #endregion
                }


                reader.Dispose();
                #endregion


                rtnVal = true;
                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, "", clsDB.DbCon);
                ComFunc.MsgBoxEx(msgForm, ex.Message);
            }
            return rtnVal;
        }
        #endregion

        #region RRS Score
        private void GetRRSScore()
        {
            int Score = 0;
            string Warring = string.Empty;
            Color WarringCol = Color.Green;
            FormPatInfoFunc.Set_FormPatInfo_RRSSCORE(clsDB.DbCon, AcpEmr, ref Score, ref Warring, ref WarringCol);
            lblRRS.BackColor = WarringCol;
            lblRRS.Text = "RRS(" + Score + "점/" + Warring + ")";
            lblRRS.Visible = true;
        }
        #endregion

        /// <summary>
        /// '반환값이 공백이면 해당없음    
        /// '반환값이 있으면 표시
        /// </summary>
        /// <param name="Ptno"></param>
        /// <param name="MedFrdate"></param>
        string READ_DRESSING(string Ptno, string MedFrdate)
        {
            OracleDataReader reader = null;
            string cDressingOrder = string.Empty;
            string rtnVal = string.Empty;

            DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

            try
            {

                string SQL = "SELECT SUM(QTY*NAL) QTY, SUCODE, BDATE";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + Ptno + "'";
                SQL += ComNum.VBLF + " AND BDATE = TO_DATE('" + dtpSys.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + " AND ORDERCODE IN  ('M0111','N0057A','N0052','N0057','GS0240','GS0841','GS1512') ";
                SQL += ComNum.VBLF + " GROUP BY QTY, SUCODE, BDATE";
                SQL += ComNum.VBLF + " HAVING SUM(QTY*NAL) > 0 ";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    cDressingOrder = "OK";
                }

                reader.Dispose();

                if (string.IsNullOrWhiteSpace(cDressingOrder))
                {
                    return rtnVal;
                }

                string cDressingChart1 = string.Empty;
                string cDressingChart2 = string.Empty;

                SQL = " SELECT FORMNO ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST ";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + Ptno + "' ";
                SQL += ComNum.VBLF + "   AND CHARTDATE = '" + dtpSys.ToString("yyyyMMdd") + "' ";
                SQL += ComNum.VBLF + "   AND FORMNO IN (1573, 1725)";

                #region 신규 기록지 로직 추가
                SQL += ComNum.VBLF + "  UNION ALL";
                SQL += ComNum.VBLF + "  SELECT FORMNO  ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST ";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + Ptno + "' ";
                SQL += ComNum.VBLF + "   AND CHARTDATE = '" + dtpSys.ToString("yyyyMMdd") + "' ";
                SQL += ComNum.VBLF + "   AND FORMNO IN (1573, 1725)";
                #endregion

                sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        if (reader.GetValue(0).ToString().Trim().Equals("1573"))
                        {
                            cDressingChart1 = "OK";
                        }

                        if (reader.GetValue(0).ToString().Trim().Equals("1725"))
                        {
                            cDressingChart2 = "OK";
                        }
                    }
                }

                reader.Dispose();

                if (string.IsNullOrWhiteSpace(cDressingChart1) && string.IsNullOrWhiteSpace(cDressingChart2))
                {
                    rtnVal = "욕창/상처간호기록지 작성요망";
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// 유치도뇨관 작성
        /// </summary>
        /// <param name="Ptno"></param>
        /// <param name="MedFrdate"></param>
        bool READ_CATH()
        {
            string strInsertDay = string.Empty;
            string strMaintaintDay = string.Empty;
            string strSize = string.Empty;

            FormPatInfoFunc.Set_FormPatInfo_VentilatorUseDay(clsDB.DbCon, AcpEmr, ref strInsertDay, ref strMaintaintDay, ref strSize);
            bool rtnVal = false;
            OracleDataReader reader = null;
            string SQL = string.Empty;

            if (!string.IsNullOrWhiteSpace(strMaintaintDay))
            {
                DateTime dtpSysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                string strSChart = string.Empty;
                string strEChart = string.Empty;

                if (dtpSysDate.Hour >= 7 && dtpSysDate.Hour <= 14 || dtpSysDate.Hour == 15 && dtpSysDate.Minute == 0)
                {
                    strSChart = dtpSysDate.ToString("yyyyMMdd") + "070000";
                    strEChart = dtpSysDate.ToString("yyyyMMdd") + "150000";
                }
                else if (dtpSysDate.Hour >= 15 && dtpSysDate.Hour <= 22 || dtpSysDate.Hour == 23 && dtpSysDate.Minute == 0 )
                {
                    strSChart = dtpSysDate.ToString("yyyyMMdd") + "150100";
                    strEChart = dtpSysDate.ToString("yyyyMMdd") + "230000";
                }
                else if (dtpSysDate.Hour >= 23 && dtpSysDate.Minute > 0) //Night 24시전이면 다음날꺼 까지.
                {
                    strSChart = dtpSysDate.ToString("yyyyMMdd") + "230100";
                    strEChart = dtpSysDate.AddDays(1).ToString("yyyyMMdd") + "060000";
                }

                else if (dtpSysDate.Hour <= 6) //Night 다음날 이면 이전날꺼 확
                {
                    strSChart = dtpSysDate.AddDays(-1).ToString("yyyyMMdd") + "230100";
                    strEChart = dtpSysDate.ToString("yyyyMMdd") + "060000";
                }

                SQL = string.Empty;
                SQL += ComNum.VBLF + "  SELECT 1 AS CNT";
                SQL += ComNum.VBLF + "  FROM DUAL";
                SQL += ComNum.VBLF + "  WHERE EXISTS";
                SQL += ComNum.VBLF + "  (";
                SQL += ComNum.VBLF + "  SELECT 1";
                SQL += ComNum.VBLF + "    FROM KOSMOS_EMR.AEMRCHARTMST ";
                SQL += ComNum.VBLF + "   WHERE PTNO = '" + AcpEmr.ptNo + "' ";
                SQL += ComNum.VBLF + "     AND MEDFRDATE = '" + AcpEmr.medFrDate + "'";
                SQL += ComNum.VBLF + "     AND FORMNO = 2641";
                SQL += ComNum.VBLF + "     AND (CHARTDATE || RPAD(CHARTTIME, 6, '0')) >= '" + strSChart + "'";
                SQL += ComNum.VBLF + "     AND (CHARTDATE || RPAD(CHARTTIME, 6, '0')) <= '" + strEChart + "'";
                SQL += ComNum.VBLF + "  )";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return rtnVal;
                }

                rtnVal = !reader.HasRows;

                reader.Dispose();
            }

            return rtnVal;
        }

        /// <summary>
        /// 자가약
        /// </summary>
        /// <param name="Ptno"></param>
        /// <param name="MedFrdate"></param>
        bool READ_SELFMED(string Ptno, string MedFrdate)
        {
            OracleDataReader reader = null;
            bool rtnVal = false;

            try
            {

                string SQL = "SELECT PTNO";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMR_CADEX_SELFMED ";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + Ptno + "'";
                SQL += ComNum.VBLF + " AND MEDFRDATE = '" + MedFrdate + "'";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                return rtnVal;
            }
        }

        void READ_MSG2(string Ptno)
        {
            OracleDataReader reader = null;
            try
            {

                string SQL = " SELECT 1 AS CNT";
                SQL += ComNum.VBLF + " FROM DUAL";
                SQL += ComNum.VBLF + " WHERE EXISTS";
                SQL += ComNum.VBLF + " (";
                SQL += ComNum.VBLF + "    SELECT 1";
                SQL += ComNum.VBLF + "    From KOSMOS_OCS.OCS_IORDER";
                SQL += ComNum.VBLF + "    WHERE PTNO = '" + Ptno + "' ";
                SQL += ComNum.VBLF + "      AND BDATE = TRUNC(SYSDATE)  ";
                SQL += ComNum.VBLF + "      AND SLIPNO = 'A6'";
                SQL += ComNum.VBLF + "      AND ORDERCODE IN (";
                SQL += ComNum.VBLF + "          SELECT ORDERCODE FROM KOSMOS_OCS.OCS_ORDERCODE";
                SQL += ComNum.VBLF + "          WHERE SUCODE = 'C3710'";
                SQL += ComNum.VBLF + "          AND SLIPNO = 'A6')";
                SQL += ComNum.VBLF + " )";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if(reader.HasRows)
                {
                    using (frmMSG_BST frm = new frmMSG_BST())
                    {
                        frm.StartPosition = FormStartPosition.CenterParent;
                        frm.ShowDialog(this);
                    }
                }

                reader.Dispose();

            }
            catch(Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void SetAcpInfo()
        {
            ClearFormData();

            conPatInfo1.SetDisPlay(clsType.User.IdNumber, AcpEmr.inOutCls, Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToShortDateString() , AcpEmr.ptNo, AcpEmr.medDeptCd);

            if (ActiveFormWrite != null)
            {
                if (ActiveFormWrite is frmEmrChartFlowOld)
                {
                    (ActiveFormWrite as frmEmrChartFlowOld).gPatientinfoRecive(mFORMNO_W.ToString(), mUPDATENO_W.ToString(), AcpEmr, "0", "W");
                }
                else if (ActiveFormWrite is frmNursingRecordOld)
                {
                    (ActiveFormWrite as frmNursingRecordOld).gPatientinfoRecive(mFORMNO_W.ToString(), mUPDATENO_W.ToString(), AcpEmr, "0", "W");
                }
                else
                {
                    ActiveFormWrite.Close();

                    if (ActiveFormWrite is frmNewHemodialysis)
                    {
                        (ActiveFormWrite as frmNewHemodialysis).SubFormClear();
                    }

                    ActiveFormWrite.Dispose();
                    ActiveFormWrite = null;
                    ActiveFormWriteChart = null;

                    if (fWrite != null)
                    {
                        LoadChart(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString());
                    }
                }
            }

            Application.DoEvents();

            if (fEmrBaseChartView == null)
            {
                LoadSubForm_ChartView();
            }

            fEmrBaseChartView.GetJupHis(AcpEmr.ptNo);
        }

        private void ClearFormData()
        {
            conPatInfo1.SetItemClear();

            if (fEmrBaseChartView != null)
            {
                fEmrBaseChartView.ClearForm();
            }

            //if (ActiveFormWrite != null)
            //{
            //    ActiveFormWrite.Dispose();
            //    ActiveFormWrite = null;
            //    ActiveFormWriteChart = null;
            //}

        }

        private void frmEmrLibViewerNr_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmEmrLibViewerNr_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
            try
            {
                if (fEmrCarePlan != null)
                {
                    fEmrCarePlan.Dispose();
                    fEmrCarePlan = null;
                }

                if (fEmrLibViewerNr != null)
                {
                    fEmrLibViewerNr.Dispose();
                    fEmrLibViewerNr = null;
                }

                if (ActiveFormWrite != null)
                {
                    ActiveFormWrite.Dispose();
                    ActiveFormWrite = null;
                }

                if (fEmrEndoMIBI != null)
                {
                    fEmrEndoMIBI.Dispose();
                    fEmrEndoMIBI = null;
                }

                if (fEmrFormSearch != null)
                {
                    fEmrFormSearch.Dispose();
                    fEmrFormSearch = null;
                }
                if (fEmrBaseChartView != null)
                {
                    fEmrBaseChartView.Dispose();
                    fEmrBaseChartView = null;
                }
                if (fEmrBaseInNrAcpList != null)
                {
                    fEmrBaseInNrAcpList.Dispose();
                    fEmrBaseInNrAcpList = null;
                }

                if (fEmrNurMibi != null)
                {
                    fEmrNurMibi.Dispose();
                    fEmrNurMibi = null;
                }

                if (fEmrPatientState != null)
                {
                    fEmrPatientState.Dispose();
                    fEmrPatientState = null;
                }

                if (fEmrTextEmrOrder != null)
                {
                    fEmrTextEmrOrder.Dispose();
                    fEmrTextEmrOrder = null;
                }

                if (fViewResult != null)
                {
                    fViewResult.Dispose();
                    fViewResult = null;
                }

                if (fEmrViewer != null)
                {
                    fEmrViewer.Dispose();
                    fEmrViewer = null;
                }
            }
            catch
            {

            }
        }

        private void frmEmrLibViewerNr_Resize(object sender, EventArgs e)
        {
            ResizeForm();
        }

        private void ResizeForm()
        {
            try
            {
                panTopSub.Top = panPatInfo.Height + mnuMain.Height;

                if (fEmrBaseChartView != null)
                {
                    fEmrBaseChartView.WindowState = FormWindowState.Normal;
                    fEmrBaseChartView.Height = panEmrView.Height;
                    fEmrBaseChartView.Width = panEmrView.Width;
                }
                Application.DoEvents();
                if (fEmrBaseInNrAcpList != null)
                {
                    fEmrBaseInNrAcpList.WindowState = FormWindowState.Normal;
                    fEmrBaseInNrAcpList.Height = panAcpMain.Height;
                    fEmrBaseInNrAcpList.Width = panAcpMain.Width;
                }
                Application.DoEvents();
            }
            catch
            {

            }
        }
        #endregion //Form 이벤트

        #region //side Bar
        private void btnSideBarLeft_Click(object sender, EventArgs e)
        {
            if (panAcp.Visible == false) return;
            panAcp.Visible = false;
            panSideBarLeft.Visible = true;
            panSplitC.Visible = false;
            ResizeForm();
        }

        private void lblSideBarLeft_Click(object sender, EventArgs e)
        {
            panAcp.Visible = true;
            panSideBarLeft.Visible = false;
            panSplitC.Visible = true;
            //panAcp.Width = SIZE_ACP_DEFAULT;
            ResizeForm();
        }

        private void panSplitC_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ResizeForm();
        }

        #endregion //side Bar

        #region //기록지 관련

        /// <summary>
        /// 폼조회 폼이 닫힐때
        /// </summary>
        private void frmEmrFormSearch_EventClosed()
        {
            //panFormSearch.Visible = false;
            fEmrFormSearch.Dispose();
            fEmrFormSearch = null;
        }

        /// <summary>
        /// 폼조회후에 폼을 띄운다
        /// </summary>
        /// <param name="aWrite"></param>
        private void frmEmrFormSearch_SetWriteForm(EmrForm aWrite)
        {
            //panFormSearch.Visible = false;
            //fEmrFormSearch.Close();
            //fEmrFormSearch = null;

            fWrite = aWrite;

            if (mFORMNO_W == fWrite.FmFORMNO )
            {
                return;
            }

            mFORMNO_W = fWrite.FmFORMNO;
            mUPDATENO_W = fWrite.FmUPDATENO;

            lblFormName.Text = "";

            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;
            }

            LoadChart(mFORMNO_W.ToString(), mUPDATENO_W.ToString());
        }

        /// <summary>
        /// 서직지(Old) 클릭시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuForm_Click(object sender, EventArgs e)
        {
            lblFormName.Text = "";

            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;

                fWrite = null;
            }


            if (((ToolStripMenuItem)sender).Tag == null)
            {
                return;
            }
            string FormNo = ((ToolStripMenuItem)sender).Tag.ToString();

            //if (mFORMNO_W == VB.Val(FormNo))
            //{
            //    return;
            //}

            double dblUpdateNo = clsEmrQuery.GetMaxUpdateNo(clsDB.DbCon, VB.Val(FormNo));

            if (dblUpdateNo == -1)
            {
                return;
            }

            mFORMNO_W = VB.Val(FormNo);
            mUPDATENO_W = dblUpdateNo;

            fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mFORMNO_W.ToString(), mUPDATENO_W.ToString());
 
            if (AcpEmr == null)
            {
                ComFunc.MsgBoxEx(this, "환자부터 선택해주세요.");
                return;
            }

            if (clsEmrQueryPohangS.CHECK_NUR_SCHEDULE(clsDB.DbCon))
            {
                ComFunc.MsgBoxEx(this, "현재 스케쥴(간호부)은 근무시간이 아닙니다. 프로그램을 종료합니다.");
                return;
            }

            LoadChart(mFORMNO_W.ToString(), mUPDATENO_W.ToString());

            tabEmr.SelectedTab = tabWrite;
        }

        /// <summary>
        /// 서직지(New) 클릭시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuFormNew_Click(object sender, EventArgs e)
        {
            if (clsEmrQueryPohangS.CHECK_NUR_SCHEDULE(clsDB.DbCon))
            {
                if (System.Diagnostics.Debugger.IsAttached == false)
                {
                    ComFunc.MsgBoxEx(this, "현재 스케쥴(간호부)은 근무시간이 아닙니다. 프로그램을 종료합니다.");
                    return;
                }
            }


            if (((ToolStripMenuItem)sender).Tag == null)
            {
                return;
            }
            string FormNo = ((ToolStripMenuItem)sender).Tag.ToString();

            #region CPR발생보고서
            if (FormNo == "888888")
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile("http://221.157.239.2/content/board/down.php?a_tablename=board_13&filename=CPR+%EB%B0%9C%EC%83%9D+%EB%B3%B4%EA%B3%A0%EC%84%9C.xlsx", @"C:\PSMHEXE\CPR발생보고서.xlsx");
                }
                Process.Start(@"C:\PSMHEXE\CPR발생보고서.xlsx");
                return;
            }
            #endregion

            #region BST 인터페이스
            if (FormNo == "888889")
            {
                using (frmBSTInterface frm = new frmBSTInterface())
                {
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog(this);
                }
                return;
            }
            #endregion

            #region BST HD 인터페이스
            if (FormNo == "888890")
            {
                using (frmBSTHDInterface frm = new frmBSTHDInterface())
                {
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog(this);
                }
                return;
            }
            #endregion

            if (AcpEmr == null)
            {
                ComFunc.MsgBoxEx(this, "환자부터 선택해주세요.");
                return;
            }

            lblFormName.Text = "";

            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Close();
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;

                fWrite = null;
            }



            //if (mFORMNO_W == VB.Val(FormNo))
            //{
            //    return;
            //}

            double dblUpdateNo = -1;
            if (FormNo == "999999999")
            {
                dblUpdateNo = 1;
            }
            else
            {
                dblUpdateNo = clsEmrQuery.GetNewFormMaxUpdateNo(clsDB.DbCon, VB.Val(FormNo));
            }

            if (dblUpdateNo == -1)
            {
                return;
            }

            mFORMNO_W = VB.Val(FormNo);
            mUPDATENO_W = dblUpdateNo;

            if (FormNo == "999999999" || FormNo.Equals("1575") || FormNo.Equals("3150"))
            {
                fWrite = clsEmrChart.ClearEmrForm();
                fWrite.FmFORMNO = (long)mFORMNO_W;
                fWrite.FmUPDATENO = 1;
                fWrite.FmFORMTYPE = "4";
                fWrite.FmPROGFORMNAME = "frmEmrBaseVitalAndActing";
                fWrite.FmFORMNAME = "임상관찰 및 활동기록";
            }
            else
            {
                fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mFORMNO_W.ToString(), mUPDATENO_W.ToString());
            }


            LoadChart(mFORMNO_W.ToString(), mUPDATENO_W.ToString());
            tabEmr.SelectedTab = tabWrite;
        }

        /// <summary>
        /// 서직지를 띄운다
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        private void LoadChart(string strFormNo, string strUpdateNo)
        {
            //동의서와 기록지를 구분해서 보여 준다

            if (AcpEmr == null || fWrite == null)
            {
                return;
            }

            lblFormName.Text = fWrite.FmFORMNAME;

            if (fWrite.FmDOCFORMNAME.Trim() != "")
            {
                LoadWirteDoc(strFormNo, strUpdateNo);
            }
            else
            {
                LoadWriteForm(strFormNo, strUpdateNo);
            }
        }

        /// <summary>
        /// 동의서를 로드한다(사용안함)
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        private void LoadWirteDoc(string strFormNo, string strUpdateNo)
        {
            if (VB.Val(strFormNo) != 0)
            {
                fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFormNo, strUpdateNo);

                if (fWrite == null)
                {
                    ComFunc.MsgBoxEx(this, "등록된 기록지 폼이 없습니다.");
                    fWrite = clsEmrChart.ClearEmrForm();
                    return;
                }

                lblFormName.Text = fWrite.FmFORMNAME;

                if (ActiveFormWrite != null)
                {
                    ActiveFormWrite.Dispose();
                    ActiveFormWrite = null;
                    ActiveFormWriteChart = null;
                }

                //frmOcrPrintX.Show();
            }
        }

        /// <summary>
        /// 서식지를 로드한다
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        private void LoadWriteForm(string strFormNo, string strUpdateNo)
        {
            if (VB.Val(strFormNo) != 0)
            {
                //fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFormNo, strUpdateNo);

                if (fWrite == null)
                {
                    ComFunc.MsgBoxEx(this, "등록된 기록지 폼이 없습니다.");
                    fWrite = clsEmrChart.ClearEmrForm();
                    return;
                }

                lblER.Visible = false;

                switch (fWrite.FmFORMNO)
                {
                    case 2294:
                    case 2295:
                    case 2296:
                    case 2305:
                    case 2311:
                    case 2356:

                        string strErFrDate = clsEmrQuery.READ_ER_TRANS_TIME(clsDB.DbCon, AcpEmr);
                        if (strErFrDate.Length > 0)
                        {
                            lblER.Top = lblFormName.Top - 5;
                            lblER.Left = lblFormName.Left + lblFormName.Width + 10;
                            lblER.Visible = true;
                            lblER.Text = "ER출발일시 : " + strErFrDate; // clsEmrQuery.READ_ER_TRANS_TIME(clsDB.DbCon, AcpEmr);
                        }
                        break;
                }

                lblFormName.Text = fWrite.FmFORMNAME;

                //if (ActiveFormWrite != null)
                //{
                //    ActiveFormWrite.Dispose();
                //    ActiveFormWrite = null;
                //    ActiveFormWriteChart = null;
                //}

                //if (frmOcrPrintX != null)
                //{
                //    frmOcrPrintX.Dispose();
                //    frmOcrPrintX = null;
                //}

                string strEmrNo = "0";
                string NewRecord = clsEmrQuery.NewArgreeRecord(clsDB.DbCon);

                if (fWrite.FmFORMNO == 1562 && fWrite.FmUPDATENO == 1)
                {
                    fWrite.FmFORMTYPE = "4";
                    fWrite.FmPROGFORMNAME = "frmEmrForm_1562_Nurse";

                }
                else if (fWrite.FmFORMNO == 1567 && fWrite.FmUPDATENO == 1)
                {
                    fWrite.FmFORMTYPE = "4";
                    fWrite.FmPROGFORMNAME = "frmTextEmrTRFS";
                }
                //SPECIAL WATCH RECORD, 간호기록(ER)
                else if (fWrite.FmFORMNO == 1969 || fWrite.FmFORMNO == 2049)
                {
                    if (AcpEmr.medDeptCd.Equals("ER") == false && AcpEmr.ptNo.Equals("81000004") == false)
                    {
                        ComFunc.MsgBoxEx(this, "ER 내원내역 에만 작성 가능합니다.");
                        return;
                    }
                }
                else if (fWrite.FmFORMNO == 1577 && fWrite.FmOLDGB == 0)
                {
                    fWrite.FmFORMTYPE = "4";
                    fWrite.FmPROGFORMNAME = "frmNewHemodialysis";
                }


                //내원, 재원중 한번만 작성을 하는 서식지인 경우 이전 기록을 불러 온다
                //아니면 작성 히스토리를 불러 온다

                //ActiveFormWrite = new frmEmrChartNew(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), "0");
                //if (strFormNo != "999999999")
                //{
                //    ActiveFormWrite = new frmEmrChartNew(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
                //}

                if (fWrite.FmGRPFORMNO >= 1050 && fWrite.FmGRPFORMNO <= 1055 || fWrite.FmGRPFORMNO == 1066 || fWrite.FmGRPFORMNO == 1068 || fWrite.FmFORMNO == 2148 || fWrite.FmFORMNO == 1959 || fWrite.FmGRPFORMNO == 1078 || fWrite.FmGRPFORMNO == 1081)
                {
                    if (fWrite.FmFORMTYPE == "2")
                    {
                        EasManager easManager = EasManager.Instance;
                        ActiveFormWrite = easManager.GetEasFormViewer();
                        easManager.Write(fWrite, AcpEmr);

                        easManager.ShowTabletMoniror();
                       
                    }
                    else
                    {
                        if (NewRecord.Equals("N"))
                        {
                            #region 이전 동의서
                            ActiveFormWrite = new frmEmrBaseEmrChartOld(this, AcpEmr, "NEW", "", "", strFormNo, fWrite.FmFORMNAME, strEmrNo);
                            ((frmEmrBaseEmrChartOld)ActiveFormWrite).rSaveOrDelete += frmEmrBaseEmrChartOld_SaveOrDelete;
                            //((frmEmrBaseEmrChartOld)ActiveFormWrite).rEventClosed += frmEmrBaseEmrChartOld_EventClosed;
                            #endregion
                        }
                        else
                        {
                            ActiveFormWrite = new frmEmrChartNew(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
                        }
                    }

                }
                else if (fWrite.FmFORMTYPE == "4") //개발자가 만든것
                {
                    if (strFormNo == "999999999")
                    {
                        //clsFormMap.EmrFormMapping("MHENRINS", strNameSpace, frmFORM.FmPROGFORMNAME, strFormNo, strUpdateNo, pEmrPatient, strEmrNo, "W"); 
                        //ActiveFormWrite = clsEmrFormMap.EmrFormMappingEx("frmEmrVitalSign", fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
                        ActiveFormWrite = clsEmrFormMap.EmrFormMappingEx("frmEmrBaseVitalAndActing", fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
                    }
                    else
                    {
                        //clsFormMap.EmrFormMapping("MHENRINS", strNameSpace, frmFORM.FmPROGFORMNAME, strFormNo, strUpdateNo, pEmrPatient, strEmrNo, "W");
                        ActiveFormWrite = clsEmrFormMap.EmrFormMappingEx(fWrite.FmPROGFORMNAME, fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
                    }
                }
                else if (fWrite.FmFORMTYPE == "3") //Flow
                {
                    ActiveFormWrite = new frmEmrChartFlowOld(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
                }
                else if (fWrite.FmFORMTYPE == "2") //전자동의서
                {
                    ActiveFormWrite = new frmEmrChartNew(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
                }
                else if (fWrite.FmFORMTYPE == "1") //동의서
                {
                    ActiveFormWrite = new frmEmrChartNew(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
                }
                else if (fWrite.FmFORMTYPE == "0") //정형화 서식
                {
                    ActiveFormWrite = new frmEmrChartNew(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
                }

                ActiveFormWrite.TopLevel = false;
                this.Controls.Add(ActiveFormWrite);
                ActiveFormWrite.Parent = panEmrWriteMain;
                ActiveFormWrite.Text = fWrite.FmFORMNAME;
                ActiveFormWrite.ControlBox = false;
                ActiveFormWrite.FormBorderStyle = FormBorderStyle.None;
                ActiveFormWrite.Top = 0;
                ActiveFormWrite.Left = 0;
                if (fWrite.FmALIGNGB == 1)   //Left
                {
                    panOption.Visible = false;
                    ActiveFormWrite.Height = panEmr.Height - 20;
                }
                else if (fWrite.FmALIGNGB == 2)  //Top
                {
                    panOption.Visible = false;
                    ActiveFormWrite.Width = panEmr.Width - 20;
                }
                else  //None
                {
                    ActiveFormWrite.Dock = DockStyle.Fill;
                }


                //ActiveFormWrite.FormClosed += ActiveFormWrite_FormClosed;
                if (NewRecord.Equals("N"))
                {
                    #region 이전 동의서
                    if (fWrite.FmGRPFORMNO >= 1050 && fWrite.FmGRPFORMNO <= 1055 || fWrite.FmGRPFORMNO == 1066 || fWrite.FmGRPFORMNO == 1068 || fWrite.FmFORMNO == 2148)
                    {
                    }
                    else
                    {
                        ActiveFormWriteChart = (EmrChartForm)ActiveFormWrite;
                    }
                    #endregion
                }
                else
                {
                    ActiveFormWriteChart = (EmrChartForm)ActiveFormWrite;
                }


                ActiveFormWrite.Show();

                //optVIEWOPT01
                //DataTable dt = null;
                //string strVIEWOPT = "0";
                //dt = clsQuery.GetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "EMROPTION", "VIEWOPT");
                //if (dt != null)
                //{
                //    if (dt.Rows.Count != 0)
                //    {
                //        strVIEWOPT = Convert.ToString(VB.Val((dt.Rows[0]["OPTVALUE"].ToString() + "").Trim()));
                //    }
                //    dt.Dispose();
                //    dt = null;
                //}
                //if (strVIEWOPT == "0")
                //{
                //    panOption.Visible = ShowOption(fWrite.FmFORMTYPE);
                //}
            }
        }

        /// <summary>
        /// 사용자 서식 버튼으로 생성한 폼이 닫힐때.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActiveFormWrite_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender.Equals(ActiveFormWrite) && e.CloseReason == CloseReason.FormOwnerClosing)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;
            }
        }

        /// <summary>
        /// 사용자 서식(Old) 버튼으로 생성한 폼이 삭제후 닫힐때
        /// </summary>
        private void frmEmrBaseEmrChartOld_SaveOrDelete()
        {
            if (ActiveFormWrite == null)
                return;

            ActiveFormWrite.Dispose();
            ActiveFormWrite = null;
            //GetChartHis();
        }

        /// <summary>
        /// 사용자 서식(Old) 버튼으로 생성한 폼이 닫힐때.
        /// </summary>
        private void frmEmrBaseEmrChartOld_EventClosed()
        {
            if (ActiveFormWrite == null)
                return;

            ActiveFormWrite.Dispose();
            ActiveFormWrite = null;
        }
        #endregion //기록지 관련

        #region //Option Panel
        private void mbtnOption_Click(object sender, EventArgs e)
        {
            pSetUserOption();
            panOption.Top = 200;
            panOption.Left = this.Width - panOption.Width - 50;
            panOption.Visible = true;
            panOption.BringToFront();
        }

        private void mbtnClose_Click(object sender, EventArgs e)
        {
            panOption.Visible = false;
        }

        private void panOptionTop_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_move = true;
            start_p = panOption.PointToScreen(new Point(e.X, e.Y));
        }

        private void panOptionTop_MouseMove(object sender, MouseEventArgs e)
        {
            Point th;
            if (mouse_move == true)
            {
                th = this.Location;
                end_p = panOption.PointToScreen(new Point(e.X, e.Y));
                Point tmp = new Point((panOption.Location.X + (end_p.X - start_p.X)), (panOption.Location.Y + (end_p.Y - start_p.Y)));
                start_p = panOption.PointToScreen(new Point(e.X, e.Y));

                panOption.Location = tmp;
                //if (panOption.Top < 1680)
                //{
                //    panOption.Top = 1680;
                //}
                //if (panOption.Left < 300)
                //{
                //    panOption.Left = 300;
                //}
            }
        }

        private void panOptionTop_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_move = false;
        }

        private void optMcrAll_CheckedChanged(object sender, EventArgs e)
        {
            if (optMcrAll.Checked == false)
            {
                return;
            }
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "EmrNrMain", "optMcrAllFlag", "1") == true)
            {
                clsEmrPublic.gstrMcrAllFlag = "1";
            }
        }

        private void optMcrDept_CheckedChanged(object sender, EventArgs e)
        {
            if (optMcrDept.Checked == false)
            {
                return;
            }
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "EmrNrMain", "optMcrAllFlag", "2") == true)
            {
                clsEmrPublic.gstrMcrAllFlag = "2";
            }
        }

        private void optMcrUser_CheckedChanged(object sender, EventArgs e)
        {
            if (optMcrUser.Checked == false)
            {
                return;
            }
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "EmrNrMain", "optMcrAllFlag", "3") == true)
            {
                clsEmrPublic.gstrMcrAllFlag = "3";
            }

            //옵션 보기 변경시 상용구 리스트 재로드
            //if (ActiveFormWrite != null && ActiveFormWrite is frmEmrForm_Progress_New)
            //{
            //    ((frmEmrForm_Progress_New)ActiveFormWrite).ChangeBoilerplate();
            //}
        }

        private void optMcrAdd_CheckedChanged(object sender, EventArgs e)
        {
            if (optMcrAdd.Checked == false)
            {
                return;
            }
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "EmrNrMain", "optMcrAddFlag", "1") == true)
            {
                clsEmrPublic.gstrMcrAddFlag = "1";
            }
        }

        private void optMcrRpl_CheckedChanged(object sender, EventArgs e)
        {
            if (optMcrRpl.Checked == false)
            {
                return;
            }
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "EmrNrMain", "optMcrAddFlag", "2") == true)
            {
                clsEmrPublic.gstrMcrAddFlag = "2";
            }
        }

        private void pSetUserOption()
        {
            DataTable dt = null;

            dt = clsQuery.GetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "EmrNrMain", "optMcrAllFlag");
            if (dt == null)
            {
                optMcrAll.Checked = true;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                optMcrAll.Checked = true;
            }
            else
            {
                string optMcrAllFlag = Convert.ToString(VB.Val((dt.Rows[0]["OPTVALUE"].ToString() + "").Trim()));
                dt.Dispose();
                if (optMcrAllFlag == "1")
                {
                    optMcrAll.Checked = true;
                    clsEmrPublic.gstrMcrAllFlag = "1";
                }
                else if (optMcrAllFlag == "2")
                {
                    optMcrDept.Checked = true;
                    clsEmrPublic.gstrMcrAllFlag = "2";
                }
                else
                {
                    optMcrUser.Checked = true;
                    clsEmrPublic.gstrMcrAllFlag = "3";
                }
            }

            dt = clsQuery.GetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "EmrNrMain", "optMcrAddFlag");
            if (dt == null)
            {
                optMcrAdd.Checked = true;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                optMcrAdd.Checked = true;
                clsEmrPublic.gstrMcrAddFlag = "1";
            }
            else
            {
                string optMcrAddFlag = Convert.ToString(VB.Val((dt.Rows[0]["OPTVALUE"].ToString() + "").Trim()));
                dt.Dispose();
                dt = null;
                if (optMcrAddFlag == "2")
                {
                    optMcrRpl.Checked = true;
                    clsEmrPublic.gstrMcrAddFlag = "2";
                }
                else
                {
                    optMcrAdd.Checked = true;
                    clsEmrPublic.gstrMcrAddFlag = "1";
                }
            }
        }

        #endregion //Option Panel

        #region //컨트롤 이벤트

        private void tabEmr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabEmr.SelectedTab == tabView)
            {
                panEmrWrite.Visible = false;
                panEmrView.Visible = true;
                panEmrView.BringToFront();
                if (fEmrBaseChartView != null)
                {
                    fEmrBaseChartView.WindowState = FormWindowState.Maximized;
                }
            }
            else
            {
                panEmrWrite.Visible = true;
                panEmrView.Visible = false;
                panEmrWrite.BringToFront();
            }
        }

        private void mbtnMacro1_Click(object sender, EventArgs e)
        {
            if (fEmrChartHisList != null)
            {
                fEmrChartHisList.Dispose();
                fEmrChartHisList = null;
            }

            if (ActiveFormWrite == null)
            {
                return;
            }

            fEmrChartHisList = new frmEmrChartHisList(ActiveFormWrite, AcpEmr.ptNo, AcpEmr.acpNo, fWrite.FmFORMNO.ToString(), fWrite.FmFORMNAME, "");
            fEmrChartHisList.StartPosition = FormStartPosition.CenterParent;
            fEmrChartHisList.ShowDialog(this);
        }

        private void btnPatientVital_Click(object sender, EventArgs e)
        {
            if (fEmrPatientState != null)
            {
                fEmrPatientState.Dispose();
                fEmrPatientState = null;
            }

            fEmrPatientState = new frmPatientState_New();
            fEmrPatientState.StartPosition = FormStartPosition.CenterParent;
            fEmrPatientState.Show(this);
        }

        private void mbtnMacro3_Click(object sender, EventArgs e)
        {
            if (fWrite == null || fWrite.FmFORMNO == 0 || ActiveFormWrite == null)
            {
                ComFunc.MsgBoxEx(this, "서식지를 선택 해주세요.");
                return;
            }

            if (fEmrBaseUserChartForm != null)
            {
                fEmrBaseUserChartForm.Dispose();
                fEmrBaseUserChartForm = null;
            }

            fEmrBaseUserChartForm = new frmEmrBaseUserChartForm(this, ActiveFormWrite, fWrite.FmFORMNO, fWrite.FmUPDATENO);
            fEmrBaseUserChartForm.rEventSetUserChart += new frmEmrBaseUserChartForm.EventSetUserChart(frmEmrBaseUserChartForm_EventSetUserChart);
            fEmrBaseUserChartForm.rEventClosed += new frmEmrBaseUserChartForm.EventClosed(frmEmrBaseUserChartForm_EventClosed);
            fEmrBaseUserChartForm.StartPosition = FormStartPosition.CenterParent;
            fEmrBaseUserChartForm.ShowDialog();
        }

        private void btnCopyList_Click(object sender, EventArgs e)
        {
            if (AcpEmr == null)
                return;

            using (frmEMRCopyList frm = new frmEMRCopyList(AcpEmr.ptNo))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        private void btnSearchForm_Click(object sender, EventArgs e)
        {
            if(fEmrFormSearch != null)
            {
                fEmrFormSearch.Dispose();
                fEmrFormSearch = null;
            }

            //----전체기록
            Screen screen = Screen.FromControl(this);
            fEmrFormSearch = new frmEmrFormSearch();
            fEmrFormSearch.rSetWriteForm += new frmEmrFormSearch.SetWriteForm(frmEmrFormSearch_SetWriteForm);
            fEmrFormSearch.rEventClosed += new frmEmrFormSearch.EventClosed(frmEmrFormSearch_EventClosed);
            if (fEmrFormSearch != null)
            {
                fEmrFormSearch.FormBorderStyle = FormBorderStyle.None;
                fEmrFormSearch.WindowState = FormWindowState.Normal;
                fEmrFormSearch.Width = 520;
                fEmrFormSearch.Height = 771;
                fEmrFormSearch.StartPosition = FormStartPosition.Manual;
                fEmrFormSearch.Location = new Point(screen.WorkingArea.Right - fEmrFormSearch.Width, 5);
                fEmrFormSearch.Show(this);
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            bool isNewEmrStart = clsEmrQueryEtc.NewEmrStart();

            if (isNewEmrStart == true)
            {
                using (frmTextEmrWardAllNew frm = new frmTextEmrWardAllNew())
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(this);
                    //if (mFORMNO_W == 999999999)
                    //{
                    //    if (ActiveFormWrite != null)
                    //    {
                    //        ActiveFormWrite.Dispose();
                    //        ActiveFormWrite = null;
                    //        ActiveFormWriteChart = null;
                    //        Application.DoEvents();
                    //    }
                    //}

                    if (ActiveFormWrite != null)
                    {
                        ActiveFormWrite.Dispose();
                        ActiveFormWrite = null;
                        ActiveFormWriteChart = null;

                        fWrite = null;
                    }

                    string FormNo = mFORMNO_W.ToString();
                    double dblUpdateNo = -1;

                    if (FormNo == "999999999")
                    {
                        dblUpdateNo = 1;
                    }
                    else
                    {
                        //dblUpdateNo = clsEmrQuery.GetNewFormMaxUpdateNo(clsDB.DbCon, VB.Val(FormNo));
                    }

                    if (dblUpdateNo == -1)
                    {
                        return;
                    }

                    mFORMNO_W = VB.Val(FormNo);
                    mUPDATENO_W = dblUpdateNo;

                    if (FormNo == "999999999")
                    {
                        fWrite = clsEmrChart.ClearEmrForm();
                        fWrite.FmFORMNO = (long)mFORMNO_W;
                        fWrite.FmUPDATENO = 1;
                        fWrite.FmFORMTYPE = "4";
                        fWrite.FmPROGFORMNAME = "frmEmrBaseVitalAndActing";
                        fWrite.FmFORMNAME = "임상관찰 및 활동기록";
                    }
                    else
                    {
                        fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mFORMNO_W.ToString(), mUPDATENO_W.ToString());
                    }

                    LoadChart(mFORMNO_W.ToString(), mUPDATENO_W.ToString());
                }
            }
            else
            {
                using (frmTextEmrWardAll frm = new frmTextEmrWardAll())
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(this);
                }
            }
        }

        private void btnOcrHis_Click(object sender, EventArgs e)
        {
            using (frmOcrHis frm = new frmOcrHis())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        private void btnMIBI_Click(object sender, EventArgs e)
        {
            if (fEmrNurMibi != null)
            {
                fEmrNurMibi.Dispose();
                fEmrNurMibi = null;
            }

            fEmrNurMibi = new frmNurMibi();
            fEmrNurMibi.StartPosition = FormStartPosition.CenterParent;
            //fEmrNurMibi.rClosed += fEmrNurMibi_rClosed;
            fEmrNurMibi.Show(this);
        }

        private void btnVital_Click(object sender, EventArgs e)
        {
            using(frmEmrBaseVitalSign frm = new frmEmrBaseVitalSign())
            {
                clsPublic.GstrHelpCode = AcpEmr == null ? "" : AcpEmr.ptNo;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        private void btnCarePlan_Click(object sender, EventArgs e)
        {
            if (AcpEmr == null)
                return;

            if (fEmrCarePlan != null)
            {
                fEmrCarePlan.Dispose();
                fEmrCarePlan = null;
            }

            fEmrCarePlan = new frmCarePlan(AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.acpNoIn, clsType.User.IdNumber);
            fEmrCarePlan.FormClosed += FEmrCarePlan_FormClosed;
            fEmrCarePlan.StartPosition = FormStartPosition.CenterParent;
            fEmrCarePlan.Show(this);

            //if (File.Exists(@"C:\cmc\ocsexe\careplan.exe") == false)
            //{
            //    using (Ftpedt FtpedtX = new Ftpedt())
            //    {
            //        FtpedtX.FtpDownload("192.168.100.33", "pcnfs", "pcnfs1", @"C:\cmc\ocsexe\careplan.exe", "careplan.exe", "/pcnfs/ocsexe");
            //    }
            //    ComFunc.MsgBoxEx(this, "Care Plan 설치 중입니다. 버튼을 다시 클릭하십시오", "확인");
            //}

            //VB.Shell(@"C:\cmc\ocsexe\careplan.exe " + AcpEmr.ptNo + "|" + AcpEmr.medFrDate + "|" + AcpEmr.acpNoIn + "|" + clsType.User.Sabun + " ");
        }

        private void btnSearchDrOrder_Click(object sender, EventArgs e)
        {
            if (fEmrTextEmrOrder != null)
            {
                fEmrTextEmrOrder.Dispose();
                fEmrTextEmrOrder = null;
            }

            if (AcpEmr == null)
                return;

            if (clsEmrQueryPohangS.CHECK_NUR_SCHEDULE(clsDB.DbCon))
            {
                ComFunc.MsgBoxEx(this, "현재 스케쥴(간호부)은 근무시간이 아닙니다. 프로그램을 종료합니다.");
                return;
            }

            fEmrTextEmrOrder = new frmTextEmrOrder(AcpEmr.medDeptCd.Equals("ER") ? "2090" : "1680", "1", AcpEmr, "0", "V", this);
            fEmrTextEmrOrder.Width = 970;
            fEmrTextEmrOrder.Height = 860;
            fEmrTextEmrOrder.StartPosition = FormStartPosition.CenterParent;
            fEmrTextEmrOrder.FormClosed += FEmrTextEmrOrder_FormClosed;
            fEmrTextEmrOrder.Show(this);
        }

        private void btnSearchResult_Click(object sender, EventArgs e)
        {
            if (fViewResult != null)
            {
                fViewResult.Dispose();
                fViewResult = null;
            }

            if (AcpEmr == null)
                return;

            fViewResult = new frmViewResult(AcpEmr.ptNo, this, 1);
            fViewResult.rEventClosed += fViewResult_rEventClosed;
            fViewResult.StartPosition = FormStartPosition.CenterScreen;
            fViewResult.Show(this);
        }

        private void btnEmrViewer_Click(object sender, EventArgs e)
        {
            //if (clsType.User.BuseCode.Equals("033124"))
            //{
            //    if (fEmrLibViewerNr != null)
            //    {
            //        fEmrLibViewerNr.Dispose();
            //        fEmrLibViewerNr = null;
            //    }

            //    fEmrLibViewerNr = new frmEmrLibViewerNr(AcpEmr, false);
            //    fEmrLibViewerNr.StartPosition = FormStartPosition.CenterParent;
            //    fEmrLibViewerNr.FormClosed += fEmrLibViewerNr_FormClosed;
            //    fEmrLibViewerNr.Show(this);
            //}
            //else
            //{
                if (fEmrViewer != null)
                {
                    fEmrViewer.Dispose();
                    fEmrViewer = null;
                }

                fEmrViewer = new frmEmrViewer(AcpEmr == null ? "" : AcpEmr.ptNo);
                fEmrViewer.StartPosition = FormStartPosition.CenterParent;
                fEmrViewer.rEventClosed += FEmrViewer_rEventClosed;
                fEmrViewer.Show(this);
            //}
        }

        #endregion //컨트롤 이벤트

        #region //임시 기록지 : 추후 삭제
        private void iCU기록지ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Tag == null)
            {
                return;
            }
            string FormNo = ((ToolStripMenuItem)sender).Tag.ToString();

            //if (mFORMNO_W == VB.Val(FormNo))
            //{
            //    return;
            //}

            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;

                fWrite = null;
            }
            lblFormName.Text = "";

            double dblUpdateNo = clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, VB.Val(FormNo));

            mFORMNO_W = VB.Val(FormNo);
            mUPDATENO_W = dblUpdateNo;

            fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mFORMNO_W.ToString(), mUPDATENO_W.ToString());

            LoadChart(mFORMNO_W.ToString(), mUPDATENO_W.ToString());

            tabEmr.SelectedTab = tabWrite;
        }

        private void 신규활력증상ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Tag == null)
            {
                return;
            }
            string FormNo = ((ToolStripMenuItem)sender).Tag.ToString();

            //if (mFORMNO_W == VB.Val(FormNo))
            //{
            //    return;
            //}

            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;

                fWrite = null;
            }
            lblFormName.Text = "";

            double dblUpdateNo = clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, VB.Val(FormNo));

            mFORMNO_W = VB.Val(FormNo);
            mUPDATENO_W = 2;

            fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mFORMNO_W.ToString(), mUPDATENO_W.ToString());

            LoadChart(mFORMNO_W.ToString(), mUPDATENO_W.ToString());

            tabEmr.SelectedTab = tabWrite;
        }

        private void toolStripMenuItem36_Click(object sender, EventArgs e)
        {
            if (fEmrEndoMIBI != null)
            {
                fEmrEndoMIBI.Dispose();
                fEmrEndoMIBI = null;
            }

            fEmrEndoMIBI = new frmEmrEndoMIBI();
            fEmrEndoMIBI.StartPosition = FormStartPosition.CenterParent;
            fEmrEndoMIBI.FormClosed += fEmrEndoMIBI_FormClosed;
            fEmrEndoMIBI.Show(this);
        }

        private void toolStripMenuItem37_Click(object sender, EventArgs e)
        {
            using(frmNrEmrSchedule nrEmrSchedule = new frmNrEmrSchedule())
            {
                nrEmrSchedule.StartPosition = FormStartPosition.CenterParent;
                nrEmrSchedule.ShowDialog(this);
            }
        }

        private void toolStripMenuItem54_Click(object sender, EventArgs e)
        {
            using(frmSetup01 frmSetup01 = new frmSetup01())
            {
                frmSetup01.StartPosition = FormStartPosition.CenterParent;
                frmSetup01.ShowDialog(this);
            }
        }

        private void toolStripMenuItem58_Click(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Tag == null)
            {
                return;
            }
            string FormNo = ((ToolStripMenuItem)sender).Tag.ToString();

            //if (mFORMNO_W == VB.Val(FormNo))
            //{
            //    return;
            //}

            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;

                fWrite = null;
            }
            lblFormName.Text = "";

            double dblUpdateNo = 2; // clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, VB.Val(FormNo));

            mFORMNO_W = VB.Val(FormNo);
            mUPDATENO_W = dblUpdateNo;

            fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mFORMNO_W.ToString(), mUPDATENO_W.ToString());

            LoadChart(mFORMNO_W.ToString(), mUPDATENO_W.ToString());

            tabEmr.SelectedTab = tabWrite;
        }

        private void toolStripMenuItem59_Click(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Tag == null)
            {
                return;
            }
            string FormNo = ((ToolStripMenuItem)sender).Tag.ToString();

            //if (mFORMNO_W == VB.Val(FormNo))
            //{
            //    return;
            //}

            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;

                fWrite = null;
            }
            lblFormName.Text = "";

            double dblUpdateNo = clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, VB.Val(FormNo));

            mFORMNO_W = VB.Val(FormNo);
            mUPDATENO_W = dblUpdateNo;

            fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mFORMNO_W.ToString(), mUPDATENO_W.ToString());

            LoadChart(mFORMNO_W.ToString(), mUPDATENO_W.ToString());

            tabEmr.SelectedTab = tabWrite;
        }

        private void toolStripMenuItem38_Click(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Tag == null)
            {
                return;
            }
            string FormNo = ((ToolStripMenuItem)sender).Tag.ToString();

            //if (mFORMNO_W == VB.Val(FormNo))
            //{
            //    return;
            //}

            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;

                fWrite = null;
            }
            lblFormName.Text = "";

            double dblUpdateNo = 1;

            mFORMNO_W = VB.Val(FormNo);
            mUPDATENO_W = dblUpdateNo;

            fWrite = clsEmrChart.ClearEmrForm();

            fWrite.FmFORMNO = (long)mFORMNO_W;
            fWrite.FmUPDATENO = (int)mUPDATENO_W;
            fWrite.FmFORMTYPE = "4";
            fWrite.FmPROGFORMNAME = "frmEmrBaseVitalAndActing";
            fWrite.FmFORMNAME = "임상관찰 및 활동기록";
            LoadChart(mFORMNO_W.ToString(), mUPDATENO_W.ToString());

            tabEmr.SelectedTab = tabWrite;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Tag == null)
            {
                return;
            }
            string FormNo = ((ToolStripMenuItem)sender).Tag.ToString();

            //if (mFORMNO_W == VB.Val(FormNo))
            //{
            //    return;
            //}

            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;

                fWrite = null;
            }
            lblFormName.Text = "";

            double dblUpdateNo = clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, VB.Val(FormNo));

            mFORMNO_W = VB.Val(FormNo);
            ///*mUPDATENO_W*/ = 2;

            fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mFORMNO_W.ToString(), dblUpdateNo.ToString());

            LoadChart(mFORMNO_W.ToString(), dblUpdateNo.ToString());

            tabEmr.SelectedTab = tabWrite;
        }

        private void toolStripMenuItem58_Click_1(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Tag == null)
            {
                return;
            }
            string FormNo = ((ToolStripMenuItem)sender).Tag.ToString();

            //if (mFORMNO_W == VB.Val(FormNo))
            //{
            //    return;
            //}

            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;

                fWrite = null;
            }
            lblFormName.Text = "";

            double dblUpdateNo = clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, VB.Val(FormNo));

            mFORMNO_W = VB.Val(FormNo);
            mUPDATENO_W = 2;

            fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mFORMNO_W.ToString(), mUPDATENO_W.ToString());

            LoadChart(mFORMNO_W.ToString(), mUPDATENO_W.ToString());

            tabEmr.SelectedTab = tabWrite;
        }

        #endregion //임시 기록지 : 추후 삭제

        private void lblRRS_DoubleClick(object sender, EventArgs e)
        {
            if (AcpEmr == null)
                return;

            using(frmEmrBaseRRSDetail frmEmrBaseRRSDetailX = new frmEmrBaseRRSDetail(AcpEmr))
            {
                frmEmrBaseRRSDetailX.StartPosition = FormStartPosition.CenterScreen;
                frmEmrBaseRRSDetailX.ShowDialog(this);
            }
        }

        private void rRS병동별조회ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmEmrBaseRRSWardView frmEmrBaseRRSWard = new frmEmrBaseRRSWardView())
            {
                frmEmrBaseRRSWard.StartPosition = FormStartPosition.CenterScreen;
                frmEmrBaseRRSWard.ShowDialog(this);
            }
        }

        private void btnSearchCatheter_Click(object sender, EventArgs e)
        {
            if (AcpEmr == null)
            {
                ComFunc.MsgBoxEx(this, "환자를 선택하고 조회 해주세요.");
                return;
            }

            using (Form frm = new frmEmrNrCatheterView(AcpEmr))
            {
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog(this);
            }
        }

        private void tESTBSTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clsType.User.IdNumber.Equals("8822") == false)
                return;

            using (Form frm = new frmBSTInterface())
            {
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog(this);
            }
        }

        private void 수가연동기초코드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clsType.User.IdNumber.Equals("8822") == false &&
                clsType.User.IdNumber.Equals("14472") == false &&
                clsType.User.IdNumber.Equals("35472") == false)
            {
                return;
            }

            using(Form frm = new frmEmrSugaLink())
            {
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog(this);
            }
        }

        private void 수가연동약속처방설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clsEmrQuery.GetEmrSettingAuth(clsDB.DbCon) == false)
                return;

            using (Form frm = new frmNurActOrderEntry_EMR())
            {
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog(this);
            }
        }

        private void 수가연동일단위설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clsEmrQuery.GetEmrSettingAuth(clsDB.DbCon) == false)
                return;

            using (Form frm = new frmDaySugaInterface())
            {
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog(this);
            }
        }

        private void bST인터페이스ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Form frm = new frmBSTInterface())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
            return;
        }

        private void 차트저장오류점검ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Form frm = new frmEmrBaseEmrCertify())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
            return;
        }

        private void 오더내역보기ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (AcpEmr.ptNo != "")
            {
                using (FrmOrderDrugInfoView FrmOrderDrugInfoViewX = new FrmOrderDrugInfoView(AcpEmr.ptNo))
                {
                    FrmOrderDrugInfoViewX.StartPosition = FormStartPosition.CenterParent;
                    FrmOrderDrugInfoViewX.ShowDialog();
                }
            }

        }
    }
}
