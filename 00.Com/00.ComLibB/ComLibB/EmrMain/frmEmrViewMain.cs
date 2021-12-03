using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ComBase;
using ComEmrBase;
using FarPoint.CalcEngine;
using Microsoft.Win32;
using Oracle.ManagedDataAccess.Client;

namespace ComLibB
{
    public partial class frmEmrViewMain : Form, MainFormMessage, FormEmrMessage
    {

        #region 사용안함

        private void GetJupHisOld()
        {
            //int i = 0;
            //string SQL = "";
            //string SqlErr = ""; //에러문 받는 변수
            //DataTable dt = null;

            //ssAcpEmr1_Sheet1.RowCount = 0;

            //string strPtNo = txtPtNo.Text.Trim();
            //strPtNo = ComFunc.SetAutoZero(strPtNo, 8);

            //Cursor.Current = Cursors.WaitCursor;

            //SQL = "";
            //SQL = " SELECT  ";
            //SQL = SQL + ComNum.VBLF + "  XX.INOUTCLS, XX.PTNO, XX.PTNAME, XX.SEX, XX.AGE, ";
            //SQL = SQL + ComNum.VBLF + "  XX.MEDDEPTCD, XX.MEDDRCD, XX.MEDFRDATE, XX.MEDFRTIME, XX.MEDENDDATE, XX.MEDENDTIME, ";
            //SQL = SQL + ComNum.VBLF + "  (SELECT DEPTKORNAME FROM ADMIN.VIEWBMEDDEPT WHERE MEDDEPTCD = XX.MEDDEPTCD) AS DEPTKORNAME,  ";
            //SQL = SQL + ComNum.VBLF + "  XX.DRNAME AS DRUSENAME, XX.GBSPC  , XX.GBSTS  ";
            //SQL = SQL + ComNum.VBLF + "FROM ( ";
            //SQL = SQL + ComNum.VBLF + " SELECT 'O' AS INOUTCLS, A.Pano AS PTNO,A.SName AS PTNAME, A.Sex, A.Age, A.DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD, ";
            //SQL = SQL + ComNum.VBLF + "    TO_CHAR(A.BDATE,'YYYYMMDD') AS MEDFRDATE, TO_CHAR(A.JTime,'HH24MI') || '00' AS MEDFRTIME, ";
            //SQL = SQL + ComNum.VBLF + "    TO_CHAR(A.BDATE,'YYYYMMDD') AS MEDENDDATE, TO_CHAR(A.JTime,'HH24MI') || '00' AS MEDENDTIME, D.DRNAME, A.GBSPC , '0' GBSTS  ";
            //SQL = SQL + ComNum.VBLF + "FROM ADMIN.OPD_MASTER A, ADMIN.BAS_LASTEXAM B , ADMIN.EXAM_INFECTMASTER C , ADMIN.BAS_DOCTOR D ";
            //SQL = SQL + ComNum.VBLF + "WHERE A.PANO = '" + strPtNo + "' ";
            //SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PANO(+)  ";
            //SQL = SQL + ComNum.VBLF + "   AND A.BDATE = B.LASTDATE(+) ";
            //SQL = SQL + ComNum.VBLF + "    AND A.PANO = C.PANO(+)  ";
            //SQL = SQL + ComNum.VBLF + "    AND A.DEPTCODE =B.DEPTCODE(+)  ";
            //SQL = SQL + ComNum.VBLF + "UNION ALL ";
            //SQL = SQL + ComNum.VBLF + " SELECT INOUTCLS, PTNO, PTNAME, SEX, AGE, MEDDEPTCD, MEDDRCD, MEDFRDATE, MEDFRTIME, MEDENDDATE, MEDENDTIME, DRNAME, GBSPC, GBSTS ";
            //SQL = SQL + ComNum.VBLF + " FROM ( ";
            //SQL = SQL + ComNum.VBLF + " SELECT 'I' AS INOUTCLS, A.Pano AS PTNO,  A.SName AS PTNAME, A.Sex, A.Age,  ";
            //SQL = SQL + ComNum.VBLF + "    A.DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD, ";
            //SQL = SQL + ComNum.VBLF + "    TO_CHAR(A.InDate,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME, ";
            //SQL = SQL + ComNum.VBLF + "    TO_CHAR(A.OutDate,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, D.DRNAME, A.GBSPC , A.GBSTS  ";
            //SQL = SQL + ComNum.VBLF + "FROM ADMIN.IPD_NEW_MASTER A , ADMIN.BAS_DOCTOR D ";
            //SQL = SQL + ComNum.VBLF + "WHERE A.PANO = '" + strPtNo + "'  ";
            //SQL = SQL + ComNum.VBLF + "   AND A.DRCODE = D.DRCODE   ";
            //SQL = SQL + ComNum.VBLF + " GROUP BY 'I', A.PANO, A.SNAME, A.SEX, A.AGE, A.DEPTCODE, A.DRCODE, TO_CHAR(A.InDate,'YYYYMMDD'), '120000', TO_CHAR(A.OutDate,'YYYYMMDD'), '120000', D.DRNAME, A.GBSPC, A.GBSTS) ";
            //SQL = SQL + ComNum.VBLF + ") XX ";
            //SQL = SQL + ComNum.VBLF + "ORDER BY XX.INOUTCLS ASC, XX.MEDFRDATE DESC, XX.MEDDEPTCD ";

            //SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            //if (SqlErr != "")
            //{
            //    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
            //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //    return;
            //}
            //if (dt.Rows.Count == 0)
            //{
            //    dt.Dispose();
            //    dt = null;
            //    Cursor.Current = Cursors.Default;
            //    return;
            //}

            //ssAcpEmr1_Sheet1.RowCount = dt.Rows.Count;
            //ssAcpEmr1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            //for (i = 0; i < dt.Rows.Count; i++)
            //{
            //    ssAcpEmr1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
            //    ssAcpEmr1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DEPTKORNAME"].ToString().Trim();
            //    ssAcpEmr1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DRUSENAME"].ToString().Trim();
            //    ssAcpEmr1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MEDFRDATE"].ToString().Trim();
            //    ssAcpEmr1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["MEDENDDATE"].ToString().Trim();
            //    ssAcpEmr1_Sheet1.Cells[i, 5].Text = "";
            //}

            //dt.Dispose();
            //dt = null;
            //Cursor.Current = Cursors.Default;

        }

        private void ssAcpEmr1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //if (ssAcpEmr1_Sheet1.RowCount == 0) return;

            //if (e.ColumnHeader == true)
            //{
            //    clsSpread.gSpdSortRow(ssAcpEmr1, e.Column);
            //    return;
            //}

            //ssAcpEmr1_Sheet1.Cells[0, 0, ssAcpEmr1_Sheet1.RowCount - 1, ssAcpEmr1_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            //ssAcpEmr1_Sheet1.Cells[e.Row, 0, e.Row, ssAcpEmr1_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;


            //frmEmrChartNew frm = new frmEmrChartNew();
            //frm.Show();
        }

        #endregion 

        #region //이벤트 전달
        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;
        #endregion //이벤트 전달

        #region //마우스이동 변수
        private Point start_p;
        private Point end_p;
        private bool mouse_move = false;
        #endregion //마우스이동 변수

        #region //컨트롤 기본사이즈 정의 변수
        private const int SIZE_VIEW_DEFAULT = 660;
        private const int SIZE_WRITE_DEFAULT = 690;
        private const int SIZE_VIEW_MIN = 30;
        private const int SIZE_WRITE_MIN = 30;
        #endregion //컨트롤 기본사이즈 정의 변수

        #region //폼에서 사용하는 변수

        /// <summary>
        /// 외부에서 전달받은 현재의 접수 정보 ; 차트 작성용
        /// </summary>
        EmrPatient AcpEmr = null; //
        /// <summary>
        /// 차트 작성용
        /// </summary>
        EmrPatient pWrite = null; //
        /// <summary>
        /// 
        /// </summary>
        EmrForm fWrite = null;
        /// <summary>
        /// 컨설트
        /// </summary>
        //frmEmrConsult frmEmrConsult = null; // 컨설트 폼
        /// <summary>
        /// Vital
        /// </summary>
        Form fEmrPatientState = null; //Vital 
        /// <summary>
        /// 미비 보는 화면
        /// </summary>
        frmTextEmrMibi fEmrTextEmrMibi = null;
        /// <summary>
        /// //전공의 챠트 확인 폼
        /// </summary>
        frmDualSign fEmrDualSign = null; 
        /// <summary>
        /// 문제목록 작성 및 조회 화면,.
        /// </summary>
        frmEmrPatMemo fEmrPatMemo = null;

        /// <summary>
        /// EMR 뷰어
        /// </summary>
        frmEmrViewer fEmrViewer = null;

        /// <summary>
        /// 활력측정 폼
        /// </summary>
        FrmVital_D frmVital = null;

        /// <summary>
        /// IO폼
        /// </summary>
        Form frmNrIO = null;

        Form ActiveFormWrite = null;
        EmrChartForm ActiveFormWriteChart = null;

        /// <summary>
        /// //진료의 경우 사용함 : 사용자가 폼을 닫을 경우 이벤트 처리
        /// </summary>
        Form mCallOrdForm = null;

        string LastrtnVal = string.Empty;

        //string mPTNO = "";
        //double mFORMNO_V = 0;
        //double mUPDATENO_V = 0;
        //double mFORMNO_W = 0;
        //double mUPDATENO_W = 0;

        //bool mViewNpChart = false;

        //모니터 사이즈, 폼 위치
        //private int mintTop = 0;
        //private int mintLeft = 0;
        private int mintMonitor = 0;
        private int[] mintWidth = null;
        private int[] mintHeight = null;

        //private int WRITE_VIEW = 0; //0:작성오른쪽, 1:작성 왼쪽

        #endregion //폼에서 사용하는 변수

        #region //서브폼 선언부

        /// <summary>
        /// BST
        /// </summary>
        Form fBST = null;

        /// <summary>
        /// 상병코드 조회
        /// </summary>
        frmCodeSearch fCodeSearch = null;
        /// <summary>
        /// 서식 조회
        /// </summary>
        frmEmrFormSearch fEmrFormSearch = null;
        /// <summary>
        /// 연속보기
        /// </summary>
        frmEmrBaseChartView fEmrBaseChartView = null;
        //frmEmrBaseContinuView fEmrBaseContinuView = null;  //차트 연속보기
        /// <summary>
        /// 사용자 서식
        /// </summary>
        frmEmrBaseUserChartForm fEmrBaseUserChartForm = null; //사용자 서식
        /// <summary>
        /// 이전 작성내역 조회
        /// </summary>
        frmEmrChartHisList fEmrChartHisList = null; //이전 작성내역 조회
        /// <summary>
        /// 검사결과
        /// </summary>
        frmViewResult fViewResult = null; //검사결과
        /// <summary>
        /// 컨설트
        /// </summary>
        frmEmrConsult fEmrConsult = null; //컨설트

        /// <summary>
        /// 복막투석
        /// </summary>
        Form frmEmrPeritonealDialysisX = null;

        /// <summary>
        /// 복막투석
        /// </summary>
        Form frmEmrRecordView = null;

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
        #endregion

        #region //임시변수
        //string GstrView01 = string.Empty;
        //string gJinGubun  = string.Empty;
        //string gJinState  = string.Empty;
        //string gUserGrade = string.Empty;
        //bool gDateSET    = false;
        const string NewEmrStartDate = "2020-04-22 07:00";

        #endregion //임시변수

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
            if (fEmrBaseChartView == null)
            {
                return;
            }

            if (strSaveFlag == "ORD")
            {

            }
            else
            {
                fEmrBaseChartView.GetJupHis(txtPtNo.Text.Trim());
            }
        }

        public void MsgDelete()
        {
            if (fEmrBaseChartView == null)
            {
                return;
            }
            fEmrBaseChartView.GetJupHis(txtPtNo.Text.Trim());
        }

        public void MsgClear()
        {
        }

        public void MsgPrint()
        {

        }

        #endregion

        #region //Private Function

        /// <summary>
        /// 환자를 받아서 화면 정보 등을 세팅한다
        /// </summary>
        private void SetPationtOtherInfo()
        {
            SetDefaultChart();
        }

        private void SetDefaultChart()
        {
            if(AcpEmr == null)
            {
                return;
            }
            if (AcpEmr.inOutCls == "I")
            {
                SetDefaultChartIn();
            }
            else
            {
                SetDefaultChartOut();
            }
        }

        private void SetDefaultChartIn()
        {
            mbtnFormClick(1002);
        }

        private void SetDefaultChartOut()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            bool blnCho = false;

            //과 신환이지 확인한다
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT GWACHOJAE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
            SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "     AND BDATE = TO_DATE('" + AcpEmr.medFrDate + "', 'YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "     AND DEPTCODE = '" + AcpEmr.medDeptCd + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0)
            {
                blnCho = true;
            }
            dt.Dispose();
            dt = null;

            //당일 작성한 내역이 있으면 해당 기록지를 띄운다
            if (blnCho == true)
            {
                mbtnFormClick(1000);
            }
            else
            {
                mbtnFormClick(1001);
            }
        }
        #endregion

        #region //Public Function

        /// <summary>
        /// 차트가 변경된 내용이 있는지 체크한다.
        /// </summary>
        /// <returns></returns>
        public string CheckChartChangeData()
        {
            string rtnVal = string.Empty;

            if (IsDisposed == true || Visible == false)
            {
                return rtnVal;
            }

            if (ActiveFormWrite != null && ActiveFormWrite.IsDisposed == false)
            {
                if(ActiveFormWrite is frmEmrForm_Progress_New )
                {
                    rtnVal = ((frmEmrForm_Progress_New)ActiveFormWrite).CheckChartChangeData();
                }
                else if (ActiveFormWrite is frmEmrForm_Progress_New2)
                {
                    rtnVal = ((frmEmrForm_Progress_New2)ActiveFormWrite).CheckChartChangeData();
                }
                else if(ActiveFormWrite is frmEmrChartNew)
                {
                    rtnVal = ((frmEmrChartNew)ActiveFormWrite).CheckChartChangeData();
                }

                if(rtnVal.Length > 0)
                {
                    LastrtnVal = "차트작성";
                }
            }

            if (fEmrBaseChartView != null && fEmrBaseChartView.IsDisposed == false && string.IsNullOrWhiteSpace(rtnVal))
            {
                rtnVal = fEmrBaseChartView.CheckChartChangeData();

                if (rtnVal.Length > 0)
                {
                    LastrtnVal = "연속보기";
                    rtnVal = "<연속보기>" + rtnVal;
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 차트를 저장한다
        /// </summary>
        /// <returns></returns>
        public double SaveChart()
        {
            double rtnVal = 0;
            if(LastrtnVal == "차트작성")
            {
                if (ActiveFormWrite != null)
                {
                    if (ActiveFormWrite is frmEmrForm_Progress_New)
                    {
                        rtnVal =  ((frmEmrForm_Progress_New)ActiveFormWrite).SetSaveData();
                    }
                    else if (ActiveFormWrite is frmEmrForm_Progress_New2)
                    {
                        rtnVal = ((frmEmrForm_Progress_New2)ActiveFormWrite).SetSaveData();
                    }
                    else
                    {
                        rtnVal = ((frmEmrChartNew)ActiveFormWrite).SaveData("0", true);
                    }
                }
            }
            else
            {
                rtnVal = fEmrBaseChartView.SaveData();
            }

            GetMiBi();
            return rtnVal;
        }

        #endregion //Public Function


        #region //Form 기본 이벤트
        public frmEmrViewMain()
        {
            InitializeComponent();

            LoadForm();
        }

        //EmrPatient AcpEmr = null;
        //AcpEmr = clsEmrChart.ClearPatient();
        //AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtNo, strInoutCls, strMedFrDate, strDeptCd);
        //if (AcpEmr == null)
        //{
        //    ComFunc.MsgBox("접수내역을 찾을 수 없습니다.");
        //    return;
        //}
        public frmEmrViewMain(EmrPatient pAcpEmr)
        {
            InitializeComponent();
            AcpEmr = pAcpEmr;

            LoadForm();
        }

        public frmEmrViewMain(EmrPatient pAcpEmr, Form pCallOrdForm)
        {
            InitializeComponent();
            AcpEmr = pAcpEmr;
            mCallOrdForm = pCallOrdForm;
            LoadForm();
        }

        private void GetMonitorInfo()
        {
            Screen[] screens = Screen.AllScreens;

            mintMonitor = screens.Length;
            mintWidth = new int[mintMonitor];
            mintHeight = new int[mintMonitor];

            int i = 0;
            foreach (Screen screen in screens)
            {
                mintWidth[i] = screen.Bounds.Width;
                mintHeight[i] = screen.Bounds.Height;
                i = i + 1;
            }
        }

        private void viewFormMonitor2()
        {
            Screen[] screens = Screen.AllScreens;
            Screen secondary_screen = null;

            if (screens.Length == 1)    //모니터 하나
            {
                this.Show();
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                foreach (Screen screen in screens)
                {
                    if (screen.Primary == false)
                    {
                        secondary_screen = screen;
                        this.Bounds = secondary_screen.Bounds;
                        //this.Top = 0;
                        //this.Left = 0;
                        this.WindowState = FormWindowState.Maximized;
                        if(this.IsDisposed == false)
                        {
                            this.Show();
                        }
                        break;
                    }
                }
            }
        }

        private void LoadForm()
        {
            pSetUserOption();

            FormInit();

            //AcpEmr = null;
            //AcpEmr = clsEmrChart.ClearPatient();
            //AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, "03983614", "O", "20170803", "GS");
            ////AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, "03983614", "I", "20170803", "MN");
            if (AcpEmr == null)
            {
                //ComFunc.MsgBox("접수내역을 찾을 수 없습니다.");
                return;
            }
        }

        /// <summary>
        /// 외부에서 환자정보를 받아서 갱신을 할 경우
        /// </summary>
        /// <param name="pAcpEmr"></param>
        public void SetNewPatient(EmrPatient pAcpEmr, Form pCallOrdForm)
        {
            //같은 등록번호, 같은입원일, 같은DRCODE, 입원/외래가 같다면 갱신 안함.
            if (AcpEmr.ptNo.Equals(pAcpEmr.ptNo) && AcpEmr.medFrDate.Equals(pAcpEmr.medFrDate) && 
                AcpEmr.medDrCd.Equals(pAcpEmr.medDrCd) && AcpEmr.inOutCls.Equals(pAcpEmr.inOutCls))
            {
                return;
            }

            panProblem.Visible = false;
            AcpEmr = pAcpEmr;
            UnloadSubForm(true);
            LoadSubForm(true);
            SetFormPatInfo();
        }

        private void SetFormPatInfo()
        {
            if (AcpEmr == null)
            {
                return;
            }

            txtPtNo.Text = AcpEmr.ptNo;
            ClearPatInfo();
            //조회한 환자가 있으면 내역을 업데이트 한다
            //SaveChartView("");
            GetPatientInfoSearch();

            conPatInfo1.SetDisPlay(clsType.User.IdNumber, AcpEmr.inOutCls, ComQuery.CurrentDateTime(clsDB.DbCon, "D"), AcpEmr.ptNo, AcpEmr.medDeptCd);
            txtPtNo.Enabled = false;

            //SetPationtOtherInfo();
            //mbtnFormClick(1001);

            string strFORMNO = "";
            string strUPDATENO = "";

            if (AcpEmr.inOutCls == "I")
            {
                //mbtnFormClick(1001);
                strFORMNO = "963";
                strUPDATENO = "1";
                strUPDATENO = "2"; //NEW EMR
            }
            else if (clsType.User.DeptCode.Equals("ER"))
            {
                //mbtnFormClick(1001);
                strFORMNO = "2605";
                strUPDATENO = clsEmrQuery.GetNewFormMaxUpdateNo(clsDB.DbCon, 2605).ToString();
            }
            else
            {
                //mbtnFormClick(1001);
                strFORMNO = "963";
                strUPDATENO = "1";
                strUPDATENO = "2"; //NEW EMR
                //if (clsType.User.DeptCode.Equals("OG") || clsType.User.DeptCode.Equals("NP") || clsType.User.DeptCode.Equals("DM"))
                //{
                //    strUPDATENO = "2";
                //}
                //else
                //{
                //    strUPDATENO = "1";
                //}
            }

            clsEmrPublic.gUserGrade = clsEmrFunc.SET_GRADE();

            GetProblem();


            fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFORMNO, strUPDATENO);
            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Close();
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;
            }

            pWrite = clsEmrChart.ClearPatient();
            pWrite = AcpEmr;

            LoadChart(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString());

        }

        private void frmEmrViewMain_Load(object sender, EventArgs e)
        {
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

            //LoadForm();            

            GetMiBi();
            GetDualSign();

            Application.DoEvents();

            GetMonitorInfo();
            viewFormMonitor2();

            SetFormPatInfo();

            // 【EMR 뷰어 설정-차트작성】
            RegistryKey reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("EmrSetting");
            string strEmrGb = reg.GetValue("VIEWWRITEUSE", string.Empty).ToString();
            if(strEmrGb.Equals("1"))
            {
                btnSideBarWriteClick();
            }
            reg.Close();
            reg.Dispose();

            //// 【EMR 뷰어 설정-차트작성】
            //string strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "VIEWWRITEUSE");
            //if (VB.Val(strEmrOption) == 1)
            //{
            //    btnSideBarWriteClick();
            //}
        }

        /// <summary>
        /// 환자의 문제목록
        /// </summary>
        private void GetProblem()
        {
            string strSql = string.Empty;
            OracleDataReader reader = null;

            strSql = " SELECT CONTENT, INPDATE";
            strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_EMR  + "AEMRPROBLEMLIST A";
            strSql = strSql + ComNum.VBLF + "  WHERE PTNO  = '" + AcpEmr.ptNo + "'";
            strSql = strSql + ComNum.VBLF + "    AND USEID = '" + clsType.User.IdNumber + "'";
            strSql = strSql + ComNum.VBLF + "    AND DELYN = '0'";
            strSql = strSql + ComNum.VBLF + "    AND PROBLEMNO = ";
            strSql = strSql + ComNum.VBLF + "    ( ";
            strSql = strSql + ComNum.VBLF + "     SELECT MAX(PROBLEMNO) ";
            strSql = strSql + ComNum.VBLF + "     FROM ADMIN.AEMRPROBLEMLIST";
            strSql = strSql + ComNum.VBLF + "     WHERE PTNO  = '" + AcpEmr.ptNo + "'";
            strSql = strSql + ComNum.VBLF + "       AND USEID = '" + clsType.User.IdNumber + "'";
            strSql = strSql + ComNum.VBLF + "       AND DELYN = '0'";
            strSql = strSql + ComNum.VBLF + "    ) ";

            string sqlErr = clsDB.GetAdoRs(ref reader, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(sqlErr, sqlErr, clsDB.DbCon);
                ComFunc.MsgBox(sqlErr);
                return;
            }

            if (reader.HasRows && reader.Read())
            {
                txtProblem.Text = reader.GetValue(0).ToString().Trim();
                lblProblem.Text = string.Format("{0}에 작성한 메모입니다.",
                    DateTime.ParseExact(reader.GetValue(1).ToString().Trim(), "yyyyMMdd", null).ToShortDateString());
                panProblem.BringToFront();
                panProblem.Visible = true;
            }

            reader.Dispose();
        }

        /// <summary>
        /// 미비 리스트 표시
        /// </summary>
        private void GetMiBi()
        {
            //btnMiBi.Visible = false;

            string strSql = string.Empty;
            OracleDataReader reader = null;

            strSql = " SELECT COUNT(A.PTNO) AS CNT";
            strSql = strSql + ComNum.VBLF + "    FROM ADMIN.EMRMIBI A";
            strSql = strSql + ComNum.VBLF + "      INNER JOIN ADMIN.BAS_PATIENT B";
            strSql = strSql + ComNum.VBLF + "         ON B.PANO = A.PTNO";

            if (clsType.User.DeptCode == "MD" || clsType.User.Sabun == "31606" || clsType.User.Sabun == "34241")
            {
                strSql = strSql + ComNum.VBLF + "    WHERE A.MEDDEPTCD IN ('MG','MC','ME','MN','MP','MR','MD','MI','MO') ";
            }
            else if (clsType.User.IdNumber == "1367")
            {
                strSql = strSql + ComNum.VBLF + "    WHERE A.MEDDEPTCD IN ('GS','HU') ";
            }
            else
            {
                strSql = strSql + ComNum.VBLF + "    WHERE A.MEDDEPTCD = '" + clsType.User.DeptCode + "' ";
            }
            strSql = strSql + ComNum.VBLF + "    AND A.MEDDRCD = '" + clsType.User.IdNumber + "' ";
            strSql = strSql + ComNum.VBLF + "    AND A.MIBICLS = 1";
            strSql = strSql + ComNum.VBLF + "    AND A.MIBIFNDATE IS NULL";
            strSql = strSql + ComNum.VBLF + "    AND TO_NUMBER(CONCAT(A.MIBIINDATE, A.MIBIINTIME)) >= NVL(CONCAT(A.WRITEDATE, A.WRITETIME), 0)";
            strSql = strSql + ComNum.VBLF + "    GROUP BY A.PTNO, A.MEDFRDATE ";

            string sqlErr = clsDB.GetAdoRs(ref reader, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(sqlErr, sqlErr, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, sqlErr);
                return;
            }

            if (reader.HasRows)
            {
                int TotalCnt = 0;
                while (reader.Read())
                {
                    TotalCnt += 1;
                }

                btnMiBi.Text = "미비작성 및 조회(" + TotalCnt + ")";
                btnMiBi.BackColor = Color.FromArgb(255, 192, 192);
                //btnMiBi.Visible = true;
            }
            else
            {
                btnMiBi.Text = "미비작성 및 조회(0)";
                btnMiBi.BackColor = Color.White;

            }

            reader.Dispose();
        }

        /// <summary>
        /// 전공의 
        /// </summary>
        private void GetDualSign()
        {
            btnDualSign.Visible = false;

            string strSql = string.Empty;
            OracleDataReader reader = null;

            DateTime dtp = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

            strSql = "  SELECT COUNT(EMRNO) AS CNT";
            strSql = strSql + ComNum.VBLF + "  FROM ADMIN.EMRXMLMST A, ADMIN.IPD_NEW_MASTER B";
            strSql = strSql + ComNum.VBLF + "   WHERE B.INDATE >= TO_DATE('2017-06-01 00:00','YYYY-MM-DD HH24:MI')";
            strSql = strSql + ComNum.VBLF + "     AND B.OUTDATE >= TO_DATE('" + dtp.AddDays(-7).ToShortDateString() + "','YYYY-MM-DD')";
            strSql = strSql + ComNum.VBLF + "     AND B.OUTDATE <= TO_DATE('" + dtp.ToShortDateString() + "','YYYY-MM-DD')";
            strSql = strSql + ComNum.VBLF + "     AND A.PTNO = B.PANO";
            strSql = strSql + ComNum.VBLF + "     AND A.MEDFRDATE = TO_CHAR(B.INDATE,'YYYYMMDD')";
            strSql = strSql + ComNum.VBLF + "   AND A.USEID IN (";
            strSql = strSql + ComNum.VBLF + "                   SELECT TO_CHAR(SABUN3)";
            strSql = strSql + ComNum.VBLF + "                     FROM ADMIN.INSA_MST";
            //'('022101','022105','022150','022160')    --내과, 정형외과, 인턴, 일반의
            switch (VB.Left(clsType.User.DrCode, 2))

            {
                case "22"://      '정형외과면
                    strSql = strSql + ComNum.VBLF + "                    WHERE BUSE IN ('022105'))";
                    break;
                case "01":
                case "02":
                case "03":
                case "04":
                case "05":
                case "07":
                case "09":
                case "11": //'내과계면
                    strSql = strSql + ComNum.VBLF + "                    WHERE BUSE IN ('022101'))";
                    break;
                default:
                    strSql = strSql + ComNum.VBLF + "                    WHERE BUSE IN ('022150','022160'))";
                    break;
            }
            strSql = strSql + ComNum.VBLF + "     AND A.FORMNO NOT IN ('963','1232')";
            strSql = strSql + ComNum.VBLF + "     AND B.DRCODE = '0301'";
            strSql = strSql + ComNum.VBLF + "     AND B.DRCODE = '" + clsType.User.DrCode + "'";
            strSql = strSql + ComNum.VBLF + "     AND NOT EXISTS";
            strSql = strSql + ComNum.VBLF + "   ( SELECT * FROM ADMIN.EMRXML_DUALSIGN SUB";
            strSql = strSql + ComNum.VBLF + "       WHERE SUB.EMRNO = A.EMRNO)";

            string sqlErr = clsDB.GetAdoRs(ref reader, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(sqlErr, sqlErr, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, sqlErr);
                return;
            }

            if (reader.HasRows && reader.Read() && reader.GetValue(0).ToString().Trim() != "0")
            {
                btnDualSign.Text = "전공의 챠트 미점검(" + reader.GetValue(0).ToString().Trim() + ")";
                btnDualSign.Visible = true;
                btnDualSign.BackColor = Color.FromArgb(255, 192, 192);
            }
            else
            {
                btnDualSign.Visible = false;
                btnDualSign.BackColor = Color.White;
            }

            reader.Dispose();
        }


        private void frmEmrViewMain_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmEmrViewMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }

            UnloadSubForm();

            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string mstrViewPath = @"C:\PSMHEXE\ScanTmp\Formname\\" + strCurDate;

            #region 당일 스캔 이미지 지우기
            if (Directory.Exists(mstrViewPath))
            {
                DirectoryInfo dir = new DirectoryInfo(mstrViewPath);

                System.IO.FileInfo[] files = dir.GetFiles("*.*",

                SearchOption.AllDirectories);

                foreach (System.IO.FileInfo file in files)

                    file.Attributes = FileAttributes.Normal;


                Directory.Delete(mstrViewPath, true);
            }
            #endregion

            //진료에서 호출 할 경우 폼을 닫는 이벤트를 발행한다
            if (mCallOrdForm != null && rEventClosed != null)
            {
                rEventClosed();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReLoad">True: 검사결과 및 ChartView 미삭제</param>
        private void UnloadSubForm(bool ReLoad = true)
        {
            try
            {
                if (ActiveFormWrite != null)
                {
                    ActiveFormWrite.Dispose();
                    ActiveFormWrite = null;
                }

                if (frmEmrRecordView != null)
                {
                    frmEmrRecordView.Dispose();
                    frmEmrRecordView = null;
                }

                if (frmEmrPeritonealDialysisX != null)
                {
                    frmEmrPeritonealDialysisX.Dispose();
                    frmEmrPeritonealDialysisX = null;
                }

                if (fBST != null)
                {
                    fBST.Dispose();
                    fBST = null;
                }

                if (frmNrIO != null)
                {
                    frmNrIO.Dispose();
                    frmNrIO = null;
                }

                if (fCodeSearch != null)
                {
                    fCodeSearch.Dispose();
                    fCodeSearch = null;
                }

                if (fEmrViewer != null)
                {
                    fEmrViewer.Dispose();
                    fEmrViewer = null;
                }

                if (fEmrPatMemo != null)
                {
                    fEmrPatMemo.Dispose();
                    fEmrPatMemo = null;
                }
                if (fEmrDualSign != null)
                {
                    fEmrDualSign.Dispose();
                    fEmrDualSign = null;
                }
                if (fEmrTextEmrMibi != null)
                {
                    fEmrTextEmrMibi.Dispose();
                    fEmrTextEmrMibi = null;
                }
                if (fEmrPatientState != null)
                {
                    fEmrPatientState.Dispose();
                    fEmrPatientState = null;
                }
                if (fViewResult != null && ReLoad == false)
                {
                    fViewResult.Dispose();
                    fViewResult = null;
                }
                if (fEmrConsult != null)
                {
                    fEmrConsult.Dispose();
                    fEmrConsult = null;
                }
                if (fEmrBaseChartView != null)
                {
                    if(ReLoad)
                    {
                        fEmrBaseChartView.SubFormClear();
                    }
                    else
                    {
                        fEmrBaseChartView.SubFormClear();
                        fEmrBaseChartView.Dispose();
                        fEmrBaseChartView = null;
                    }
                }

                if (frmVital != null)
                {
                    frmVital.Dispose();
                    frmVital = null;
                }

            }
            catch
            {

            }
        }

        private void frmEmrViewMain_Resize(object sender, EventArgs e)
        {
            ResizeForm();
        }

        private void FormInit()
        {
            panFormSearch.Left = 200;
            panFormSearch.Top = 80;
            panFormSearch.Width = 520;
            panFormSearch.Height = 771;
            panFormSearch.Visible = false;

            panSideBarRight.Visible = false;
            panSideBarLeft.Visible = false;

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);


            #region //작성 조회 이름 세팅
            //WRITE_VIEW = 0;
            //if (WRITE_VIEW == 1)
            //{
            //    lblSideBarRight.Text = "차 트 작 성        ";
            //    lblViewTitle.Text = "차 트 작 성";
            //    lblSideBarLeft.Text = "차 트 조 회          ";
            //    lblWriteTitle.Text = "차 트 조 회";
            //    panHeadViewSub.Parent = panRight;
            //    panHeadViewSub.Left = 109;
            //    panHeadViewSub.Top = 1;

            //    panHeadView.Left = 693;
            //    panHeadWrite.Left = 226;

            //    panEmrWrite.Parent = panLeft;
            //    panViewEmr.Parent = panRight;
            //    panViewLab.Parent = panRight;
            //}
            //else
            //{
            //    lblSideBarLeft.Text = "차 트 작 성        ";
            //    lblWriteTitle.Text = "차 트 작 성";
            //    lblSideBarRight.Text = "차 트 조 회          ";
            //    lblViewTitle.Text = "차 트 조 회";
            //    panHeadViewSub.Parent = panLeft;
            //    panHeadViewSub.Left = 109;
            //    panHeadViewSub.Top = 1;

            //    panHeadView.Left = 226;
            //    panHeadWrite.Left = 693;

            //    panEmrWrite.Parent = panRight;
            //    panViewEmr.Parent = panLeft;
            //    panViewLab.Parent = panLeft;
            //}
            #endregion //작성 조회 이름 세팅

            #region //Left Right
            panLeft.Width = SIZE_WRITE_DEFAULT;
            panLeft.Dock = DockStyle.Left;
            panLeft.BringToFront();

            //SplitC 재정렬
            panSplitC.BringToFront();

            panRight.Dock = DockStyle.Fill;
            panRight.BringToFront();
            #endregion //Left Right

            #region //View
            panViewEmr.Dock = DockStyle.Fill;
            panViewLab.Dock = DockStyle.Fill;
            panViewConsult.Dock = DockStyle.Fill;
            panViewLab.Visible = false;
            panViewConsult.Visible = false;
            panViewEmrMain.Dock = DockStyle.Fill;
            #endregion //View

            #region //Write
            panEmrWrite.Dock = DockStyle.Fill;
            panEmr.Dock = DockStyle.Fill;
            #endregion //Write

            LoadSubForm();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReLoad">false: 검사결과 미로드</param>
        private void LoadSubForm(bool ReLoad = false)
        {
            panViewEmrMain.Visible = true;
            panViewEmrMain.BringToFront();

            if(ReLoad == false)
            {
                fEmrBaseChartView = new frmEmrBaseChartView(this);
                //fEmrBaseChartView.rEventClosed += new frmEmrBaseChartView.EventClosed(frmEmrBaseChartView_EventClosed);
                if (fEmrBaseChartView != null)
                {
                    SubFormToControl(fEmrBaseChartView, panViewEmrMain, "Fill", true, false);
                }
            }
            
            if (AcpEmr == null && ReLoad == false)
            {
                fViewResult = new frmViewResult();
            }
            else if(AcpEmr != null)
            {
                if (ReLoad == false)
                {
                    fViewResult = new frmViewResult(AcpEmr.ptNo);
                }
                fEmrConsult = new frmEmrConsult();
            }

            if (fViewResult != null)
            {
                if(ReLoad == false)
                {
                    SubFormToControl(fViewResult, panViewLab, "None", false, false);
                    fViewResult.Width -= 70;
                    fViewResult.Height -= 70;

                    Control[] controls = fViewResult.Controls.Find("panTitle", true);
                    if (controls.Length > 0)
                    {
                        controls[0].Visible = false;
                    }


                    controls = fViewResult.Controls.Find("txtPtNo", true);
                    if (controls.Length > 0 && controls[0].Text.Length == 0)
                    {
                        controls[0].Text = txtPtNo.Text;
                    }

                    controls = fViewResult.Controls.Find("pan2", true);
                    if (controls.Length > 0)
                    {
                        controls[0].Top = 0;
                    }
                }
                else
                {
                    fViewResult.rGetDate(AcpEmr.ptNo);
                }

            }

            if (fEmrConsult != null)
            {
                SubFormToControl(fEmrConsult, panViewConsult, "None", false, false);
            }
        }

        private void ResizeForm()
        {
            try
            {
                #region //Left Right
                panViewEmrMain.Width = panLeft.Width;
                panViewEmrMain.Height = panLeft.Height;
                Application.DoEvents();
                panViewLab.Width = panLeft.Width;
                panViewLab.Height = panLeft.Height;
                Application.DoEvents();

                panEmrWrite.Width = panRight.Width;
                panEmrWrite.Height = panRight.Height;
                Application.DoEvents();
                #endregion //Left Right


                if (fEmrBaseChartView != null)
                {
                    fEmrBaseChartView.WindowState = FormWindowState.Normal;
                    fEmrBaseChartView.Height = panViewEmrMain.Height;
                    fEmrBaseChartView.Width = panViewEmrMain.Width;
                }
                Application.DoEvents();
            }
            catch
            {

            }
        }

        #endregion //Form 기본 이벤트

        #region //SubForm Event
        private void FrmNrIO_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmNrIO != null)
            {
                frmNrIO.Dispose();
                frmNrIO = null;
            }
        }

        private void FBST_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fBST != null)
            {
                fBST.Dispose();
                fBST = null;
            }
        }

        private void FCodeSearch_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrPeritonealDialysisX != null)
            {
                frmEmrPeritonealDialysisX.Dispose();
                frmEmrPeritonealDialysisX = null;
            }
        }

        private void frmEmrRecordView_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrRecordView != null)
            {
                frmEmrRecordView.Dispose();
                frmEmrRecordView = null;
            }
        }

        private void fCodeSearch_rClosed()
        {
            if (fCodeSearch != null)
            {
                fCodeSearch.Dispose();
                fCodeSearch = null;
            }
        }
        private void EmrViewer_rEventClosed()
        {
            if (fEmrViewer != null)
            {
                fEmrViewer.Dispose();
                fEmrViewer = null;
            }
        }

        private void frmEmrBaseChartView_EventClosed()
        {
            fEmrBaseChartView.SubFormClear();
            fEmrBaseChartView.Dispose();
            fEmrBaseChartView = null;
        }

        private void frmTextEmrMibi_rEventMiBiUserSend(string strPtNo, string strInDate, string strOutDate)
        {
            GetMiBi();
            EmrPatient emr = clsEmrChart.ClearPatient();
            emr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtNo, "I", strInDate);

            #region 미비 화면 뜨게
            if (fEmrViewer != null && emr != null)
            {
                fEmrViewer.SetNewPatient(emr.ptNo, emr.medFrDate, emr.medEndDate);
                return;
            }

            fEmrViewer = new frmEmrViewer(strPtNo);
            fEmrViewer.rEventClosed += EmrViewer_rEventClosed;
            fEmrViewer.StartPosition = FormStartPosition.CenterParent;
            fEmrViewer.Show(this);
            if(emr != null)
            {
                fEmrViewer.SetNewPatient(emr.ptNo, emr.medFrDate, emr.medEndDate);
            }
            #endregion
            return;

        }
        private void frmEmrTextEmrMibi_rEventClosed()
        {
            GetMiBi();
            if (fEmrTextEmrMibi != null)
            {
                fEmrTextEmrMibi.Dispose();
                fEmrTextEmrMibi = null;
            }
        }
        private void FEmrPatientState_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fEmrPatientState != null)
            {
                fEmrPatientState.Dispose();
                fEmrPatientState = null;
            }
        }

        private void FEmrPatMemo_rEventClosed()
        {
            if (fEmrPatMemo != null)
            {
                fEmrPatMemo.Dispose();
                fEmrPatMemo = null;
            }
        }

        #endregion

        #region //side Bar

        private void lblSideBarLeft_Click(object sender, EventArgs e)
        {
            lblSideBarLeftClick();
        }

        private void lblSideBarLeftClick()
        {
            panHeadWrite.Visible = true;
            panHeadView.Left = 622;
            panLeft.Visible = true;
            panSideBarLeft.Visible = false;
            panSplitC.Visible = true;
            panLeft.Width = SIZE_WRITE_DEFAULT;
            //panWriteMenu.Visible = true;

            ResizeForm();
        }

        private void btnSideBarWrite_Click(object sender, EventArgs e)
        {
            btnSideBarWriteClick();
        }

        private void btnSideBarWriteClick()
        {
            if (panLeft.Visible == false) return;
            panHeadWrite.Visible = false;
            panHeadView.Left = 2;
            panLeft.Visible = false;
            panSideBarLeft.Visible = true;
            panSplitC.Visible = false;
            //panWriteMenu.Visible = false;

            ResizeForm();
        }

        private void lblSideBarRight_Click(object sender, EventArgs e)
        {
            panRight.Visible = true;
            panSideBarRight.Visible = false;
            panLeft.Width = SIZE_WRITE_DEFAULT;
            panLeft.Dock = DockStyle.Left;
            panSplitC.Visible = true;

            ResizeForm();
        }

        private void btnSideBarView_Click(object sender, EventArgs e)
        {
            if (panRight.Visible == false) return;
            panRight.Visible = false;
            panSideBarRight.Visible = true;
            panSplitC.Visible = false;
            panLeft.Dock = DockStyle.Fill;

            ResizeForm();
        }

        private void panSplitC_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ResizeForm();
        }

        #endregion //side Bar

        #region //환자정보조회
        private void btnSearchPt_Click(object sender, EventArgs e)
        {

        }

        private void ClearPatInfo()
        {
            fEmrBaseChartView.ClearForm();
            
        }

        private void txtPtNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                ClearPatInfo();
                //조회한 환자가 있으면 내역을 업데이트 한다
                //SaveChartView("");
                GetPatientInfoSearch();

                SetConPatInfo();

                mbtnFormClick(1000);

            }
        }

        private void SetConPatInfo()
        {
            conPatInfo1.SetItemClear();

            if (txtPtNo.Text.Trim() == "") return;

            //외부에서 전달 받은 것이 없으면
            //최근내원내역을 가지고 와서 뿌린다
            string strPTNO = "";
            string strINOUTCLS = "";
            string strMEDFRDATE = "";
            string strMEDDEPTCD = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = " SELECT  ";
            SQL = SQL + ComNum.VBLF + "    A.PTNO, A.INOUTCLS, A.MEDFRDATE, A.MEDDEPTCD ";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.AVIEWACP A ";
            SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + txtPtNo.Text.Trim() + "' ";
            SQL = SQL + ComNum.VBLF + "AND A.ACPNO = (SELECT MAX(A1.ACPNO)  ";
            SQL = SQL + ComNum.VBLF + "                FROM ADMIN.AVIEWACP A1 ";
            SQL = SQL + ComNum.VBLF + "                WHERE A1.PTNO = '" + txtPtNo.Text.Trim() + "') ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }

            strPTNO = dt.Rows[0]["PTNO"].ToString().Trim();
            strINOUTCLS = dt.Rows[0]["INOUTCLS"].ToString().Trim();
            strMEDFRDATE = dt.Rows[0]["MEDFRDATE"].ToString().Trim();
            strMEDDEPTCD = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
            dt.Dispose();
            dt = null;

            conPatInfo1.SetDisPlay(clsType.User.IdNumber, strINOUTCLS, ComQuery.CurrentDateTime(clsDB.DbCon, "D"), strPTNO, strMEDDEPTCD);
        }

        private void GetPatientInfoSearch()
        {
            if (txtPtNo.Text.Trim() == "")
            {
                return;
            }

            string strPtNo = txtPtNo.Text.Trim();

            if (VB.IsNumeric(strPtNo) == false)
            {
                if (strPtNo.Length < 2)
                {
                    ComFunc.MsgBox("조회 하고자 하는 이름은 2자리 이상만 가능합니다!.");
                    txtPtNo.Focus();
                    return;
                }
                if (GetPatientInfo("", strPtNo) == 1)
                {
                    fEmrBaseChartView.GetJupHis(txtPtNo.Text.Trim());
                }
            }
            else
            {
                strPtNo = ComFunc.SetAutoZero(strPtNo, 8);

                if (GetPatientInfo(strPtNo, "") == 1)
                {
                    fEmrBaseChartView.GetJupHis(txtPtNo.Text.Trim());
                    //여기여기
                }
            }
        }

        private int GetPatientInfo(string strPtNo, string strPtName)
        {
            int rtnVal = -1;

            rtnVal = SetPatInfo(strPtNo, strPtName);
            
            if (rtnVal != 1)
            {
                return rtnVal;
            }

            return rtnVal;
        }

        private int SetPatInfo(string strPtNo, string strPtName)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            int rtnVal = -1;

            Cursor.Current = Cursors.WaitCursor;
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT A.PTNO, A.PTNAME, A.SSNO1, A.SSNO2 ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "VIEWBPT  A ";
            if (strPtNo != "")
            {
                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + strPtNo + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " WHERE A.PTNAME LIKE '" + strPtName + "%'";
            }

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (dt.Rows.Count == 1)
            {
                txtPtNo.Text = dt.Rows[0]["PTNO"].ToString().Trim();
                rtnVal = 1;
            }
            else
            {

                rtnVal = 2;
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
            return rtnVal;
        }
        #endregion

        #region //공통이벤트

        private bool ShowOption(string strFORMTYPE)
        {
            if (strFORMTYPE != "0")
            {
                return false;
            }

            panOption.Top = 50;
            panOption.Left = panEmr.Width - panOption.Width - 15;
            panOption.Visible = true;
            panOption.BringToFront();
            return true;
        }

        #endregion
        
        #region //차트작성 부분

        private void LoadChart(string strFormNo, string strUpdateNo)
        {
            //동의서와 기록지를 구분해서 보여 준다
            lblFormName.Text = fWrite.FmFORMNAME;


            if (fWrite.FmDOCFORMNAME.Trim() != "")
            {
                LoadWirteDoc(strFormNo, strUpdateNo);
            }
            else
            {
                LoadWirteForm(strFormNo, strUpdateNo);
            }
        }

        private void btnForm01_Click(object sender, EventArgs e)
        {
            mbtnFormClick(1000);
        }

        private void mbtnForm02_Click(object sender, EventArgs e)
        {
            mbtnFormClick(1001);
        }

        private void mbtnForm03_Click(object sender, EventArgs e)
        {
            mbtnFormClick(1004);
        }

        private void mbtnForm04_Click(object sender, EventArgs e)
        {
            mbtnFormClick(1002);
        }

        private void mbtnForm05_Click(object sender, EventArgs e)
        {
            mbtnFormClick(1003); //경과이미지
        }

        private void mbtnForm06_Click(object sender, EventArgs e)
        {
            mbtnFormClick(1009);
        }

        private void mbtnForm99_Click(object sender, EventArgs e)
        {
            mbtnFormClick(1075);
        }

        private void mbtnForm07_Click(object sender, EventArgs e)
        {
            mbtnFormClick(-1);
        }

        private void mbtnFormClick(double dblGrpFormNo)
        {

            //ssForm.Visible = false;
            panForm.Visible = false;

            ssForm_Sheet1.RowCount = 0;
            string strFORMNO = "0";
            string strUPDATENO = "0";
            string strFORMTYPE = "0";

            //if (dblGrpFormNo == 1003) //경과이미지
            //{
            //    strFORMNO = "1232";
            //    strUPDATENO = "1";

            //    //fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFORMNO, strUPDATENO);
            //    //if (ActiveFormWrite != null)
            //    //{
            //    //    ActiveFormWrite.Close();
            //    //    ActiveFormWrite = null;
            //    //    ActiveFormWriteChart = null;
            //    //}

            //    pWrite = clsEmrChart.ClearPatient();
            //    pWrite = AcpEmr;

            //    //LoadChart(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString());

            //    return;
            //}

            #region 19-08-20 주소영 간호사 요청으로 추가
            string strChangeChart = CheckChartChangeData();
            if (strChangeChart != "")
            {
                #region //EMR 저장시 메시지 박스 기본 버튼(예1, 아니오0)
                RegistryKey reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("EmrSetting");
                string strEmrGb = reg.GetValue("EmrSaveMsg", "0").ToString();
                reg.Close();
                reg.Dispose();

                MessageBoxDefaultButton defaultButton = strEmrGb.Equals("1") ? MessageBoxDefaultButton.Button1 : MessageBoxDefaultButton.Button2;
                #endregion

                if (ComFunc.MsgBoxQEx(this, strChangeChart + ComNum.VBLF + "저장 하시겠습니까?", "저장", defaultButton) == DialogResult.Yes)
                {
                    double pEmrNo = SaveChart();
                    if (pEmrNo == 0)
                    {
                        return;
                    }
                }
            }
            #endregion

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            // 기록지 조회
            SQL = " SELECT  ";
            SQL = SQL + ComNum.VBLF + "A.GRPFORMNO, A.FORMNO, A.UPDATENO, A.FORMNAME, A.FORMNAMEPRINT, A.USECHECK, A.FORMTYPE  ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRUSERFORM U ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRFORM A ";
            SQL = SQL + ComNum.VBLF + "    ON U.FORMNO = A.FORMNO ";
            SQL = SQL + ComNum.VBLF + "WHERE U.GRPTYPE = 'FD' ";
            SQL = SQL + ComNum.VBLF + "    AND U.GRPGB = 'U' ";
            SQL = SQL + ComNum.VBLF + "    AND U.USEGB = '" + clsType.User.IdNumber + "' ";
            if (dblGrpFormNo == -1)
            {
                SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = 454"; //응급실 환자 분류표
            }
            else if (dblGrpFormNo == 0)
            {
                SQL = SQL + ComNum.VBLF + "    AND A.FORMTYPE = '2'"; //출력서식
            }
            else
            {
                switch ((int) dblGrpFormNo)
                {
                    case 1001:
                        SQL = SQL + ComNum.VBLF + "    AND A.FORMNO NOT IN (1232, 2148)";
                        break;
                    case 1003:
                        SQL = SQL + ComNum.VBLF + "    AND A.FORMNO IN (1232, 2148)";
                        dblGrpFormNo = 1001;
                        break;
                }
                SQL = SQL + ComNum.VBLF + "    AND A.GRPFORMNO = " + dblGrpFormNo;
                //SQL = SQL + ComNum.VBLF + "    AND A.FORMNO NOT IN (336, 337, 338, 454)";
            }
            SQL = SQL + ComNum.VBLF + "    AND A.USECHECK = '1'";
            SQL = SQL + ComNum.VBLF + "    AND A.UPDATENO = (SELECT MAX(B.UPDATENO) FROM " + ComNum.DB_EMR + "AEMRFORM B ";
            SQL = SQL + ComNum.VBLF + "                                        WHERE B.FORMNO = A.FORMNO AND B.USECHECK = '1') ";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.FORMNAME, A.DISPSEQ ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }
            if (dt.Rows.Count == 1)
            {
                strFORMNO = dt.Rows[i]["FORMNO"].ToString().Trim();
                strUPDATENO = dt.Rows[i]["UPDATENO"].ToString().Trim();
                strFORMTYPE = dt.Rows[i]["FORMTYPE"].ToString().Trim();
                dt.Dispose();

                if (strFORMNO == "963" || strFORMNO == "1232") //NEW EMR
                {
                    // 신규EMR
                    strUPDATENO = "2";

                    //if (AcpEmr.inOutCls == "I")
                    //{
                    //    strUPDATENO = "2";
                    //}
                    //else if (AcpEmr.inOutCls == "O")
                    //{
                    //    if (clsType.User.DeptCode.Equals("OG") || clsType.User.DeptCode.Equals("NP") || clsType.User.DeptCode.Equals("DM"))
                    //    {
                    //        strUPDATENO = "2";
                    //    }
                    //}
                }

                fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFORMNO, strUPDATENO);
                if (ActiveFormWrite != null)
                {
                    ActiveFormWrite.Dispose();
                    ActiveFormWrite = null;
                    ActiveFormWriteChart = null;
                }

                pWrite = clsEmrChart.ClearPatient();
                pWrite = AcpEmr;

                LoadChart(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString());

                return;
            }

            ssForm_Sheet1.RowCount = dt.Rows.Count;
            ssForm_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssForm_Sheet1.Cells[i, 0].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 2].Text = dt.Rows[i]["FORMTYPE"].ToString().Trim();
            }
            dt.Dispose();

            panForm.Top = 80;
            panForm.Left = 229;
            panForm.BringToFront();
            panForm.Visible = true;

            //ssForm.Top = 80;
            ////ssForm.Left = 695;  
            //ssForm.Left = 229;
            //ssForm.BringToFront();
            //ssForm.Visible = true;
        }


        private void ssForm_Leave(object sender, EventArgs e)
        {
            //panForm.Visible = false;
            //ssForm.Visible = false;
        }

        private void ssForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return )
            {
                //ssForm.Visible = false;
                panForm.Visible = false;
            }
        }

        private void ssForm_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssForm_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssForm, e.Column);
                return;
            }

            ComFunc.SelectRowColor(ssForm_Sheet1, e.Row);
        }

        private void ssForm_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssForm_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssForm, e.Column);
                return;
            }

            //ssForm.Visible = false;
            panForm.Visible = false;

            string strFORMNO = ssForm_Sheet1.Cells[e.Row, 1].Text.Trim();
            string strUPDATENO = "0";
            string strFORMTYPE = ssForm_Sheet1.Cells[e.Row, 2].Text.Trim();

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "SELECT MAX(UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM ";
            SQL = SQL + ComNum.VBLF + "      WHERE FORMNO = " + VB.Val(strFORMNO);
            SQL = SQL + ComNum.VBLF + "        AND USECHECK = '1'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
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
                ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }
            strUPDATENO = dt.Rows[0]["UPDATENO"].ToString().Trim();
            dt.Dispose();
            dt = null;

            if (strFORMNO == "963" || strFORMNO == "1232") //NEW EMR
            {
                //신규EMR
                strUPDATENO = "2";
                //if (AcpEmr.inOutCls == "I")
                //{
                //    strUPDATENO = "2";
                //}
                //else if (AcpEmr.inOutCls == "O")
                //{
                //    if (clsType.User.DeptCode.Equals("OG") || clsType.User.DeptCode.Equals("NP") || clsType.User.DeptCode.Equals("DM"))
                //    {
                //        strUPDATENO = "2";
                //    }
                //}
            }

            fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFORMNO, strUPDATENO);
            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;
            }

            pWrite = clsEmrChart.ClearPatient();
            pWrite = AcpEmr;

            LoadChart(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString());
        }

        private void btnUserImageReg_Click(object sender, EventArgs e)
        {
            using (frmEmrUserImageReg frm = new frmEmrUserImageReg("U", ""))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
        }

        private void btnUserFormReg_Click(object sender, EventArgs e)
        {
            using (frmEmrUserFormReg frm = new frmEmrUserFormReg("U", ""))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
        }

        private void btnSearchForm_Click(object sender, EventArgs e)
        {
            //----전체기록
            if (fEmrFormSearch == null)
            {
                fEmrFormSearch = new frmEmrFormSearch();
                fEmrFormSearch.rSetWriteForm += new frmEmrFormSearch.SetWriteForm(frmEmrFormSearch_SetWriteForm);
                fEmrFormSearch.rEventClosed += new frmEmrFormSearch.EventClosed(frmEmrFormSearch_EventClosed);
                if (fEmrFormSearch != null)
                {
                    fEmrFormSearch.Owner = this;
                    fEmrFormSearch.TopLevel = false;
                    this.Controls.Add(fEmrFormSearch);
                    fEmrFormSearch.Parent = panFormSearch;
                    fEmrFormSearch.Text = "";
                    fEmrFormSearch.ControlBox = false;
                    fEmrFormSearch.FormBorderStyle = FormBorderStyle.None;
                    fEmrFormSearch.Top = 0;
                    fEmrFormSearch.Left = 0;
                    fEmrFormSearch.WindowState = FormWindowState.Normal;
                    fEmrFormSearch.Height = panFormSearch.Height;
                    fEmrFormSearch.Width = panFormSearch.Width;
                    //fEmrFormSearch.Dock = DockStyle.Fill;
                    fEmrFormSearch.Show();
                    
                }
            }
            panFormSearch.BringToFront();
            panFormSearch.Visible = true;
            panForm.Visible = false;
        }

        private void frmEmrFormSearch_EventClosed()
        {
            panFormSearch.Visible = false;
            fEmrFormSearch.Dispose();
            fEmrFormSearch = null;
        }

        private void frmEmrFormSearch_SetWriteForm(EmrForm aWrite)
        {
            #region 19-09-05 산부인과 지경석 과장님 요청으로 추가.
            string strChangeChart = CheckChartChangeData();
            if (strChangeChart != "")
            {
                #region //EMR 저장시 메시지 박스 기본 버튼(예1, 아니오0)
                RegistryKey reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("EmrSetting");
                string strEmrGb = reg.GetValue("EmrSaveMsg", "0").ToString();
                reg.Close();
                reg.Dispose();

                MessageBoxDefaultButton defaultButton = strEmrGb.Equals("1") ? MessageBoxDefaultButton.Button1 : MessageBoxDefaultButton.Button2;
                #endregion

                if (ComFunc.MsgBoxQEx(this, strChangeChart + ComNum.VBLF + "저장 하시겠습니까?", "저장", defaultButton) == DialogResult.Yes)
                {
                    double pEmrNo = SaveChart();
                    if (pEmrNo == 0)
                    {
                        return;
                    }
                }
            }
            #endregion

            panFormSearch.Visible = false;
            //fEmrFormSearch.Close();
            //fEmrFormSearch = null;

            fWrite = aWrite;
            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;
            }

            pWrite = clsEmrChart.ClearPatient();
            pWrite = AcpEmr;

            LoadChart(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString());
        }

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

        private void LoadWirteForm(string strFormNo, string strUpdateNo)
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


                #region 기록지 입원, 외래 체크

                //폼에 등록한 과와 현재 
                if (fWrite.FmVISITSDEPT.Length > 0 && fWrite.FmVISITSDEPT.Equals(AcpEmr.medDeptCd) == false)
                {
                    ComFunc.MsgBoxEx(this, "현재 기록지는 " + fWrite.FmVISITSDEPT + "환자만 작성이 가능합니다.\r\n지금 챠트로 작성하시려고 하시는 환자는 '" + AcpEmr.medDeptCd + "'과입니다.\r\n해당 환자리스트에서 챠트버튼을 눌러서 작성하시거나,\r\n'EMR작성' 화면에서 맞는 내원내역을 선택후 작성해주세요.");
                    return;
                }

                //기록지 저장은 외래만 가능한데 환자 정보가 외래가 아닐경우
                if (fWrite.FmINOUTCLS == "1" && AcpEmr.inOutCls != "O")
                {
                    ComFunc.MsgBoxEx(this, "현재 기록지는 외래 혹은 응급실 환자만 작성이 가능합니다.\r\n해당 환자리스트에서 챠트버튼을 눌러서 작성하시거나,\r\n'EMR작성' 화면에서 맞는 내원내역을 선택후 작성해주세요.");
                    return;
                }

                //기록지 저장은 입원만 가능한데 환자 정보가 입원이 아닐경우
                if (fWrite.FmINOUTCLS == "2" && AcpEmr.inOutCls != "I")
                {
                    ComFunc.MsgBoxEx(this, "현재 기록지는 입원 환자만 작성이 가능합니다.\r\n입원 환자리스트에서 챠트버튼을 눌러서 작성하시거나,\r\n'EMR작성' 화면에서 맞는 내원내역을 선택후 작성해주세요.");
                    return;
                }
                #endregion

                //if (fWrite.FmPROGFORMNAME == "")
                //{
                //    ComFunc.MsgBoxEx(this, "등록된 기록지 폼이 없습니다.");
                //    fWrite = clsEmrChart.ClearEmrForm();
                //    return;
                //}
                string strEmrNo = "0";
                string NewRecord = clsEmrQuery.NewArgreeRecord(clsDB.DbCon);

                lblFormName.Text = fWrite.FmFORMNAME;

                if (ActiveFormWrite != null)
                {
                    ActiveFormWrite.Dispose();
                    ActiveFormWrite = null;
                    ActiveFormWriteChart = null;
                }

                //string strVal = tAcp.medFrDate;
                string strVal = "";

                // 경과기록지
                if (strFormNo == "963")
                {
                    if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(NewEmrStartDate))
                    {
                        #region // 외래 진료과별 부분오픈
                        //if (clsOrdFunction.GstrGbJob.Equals("OPD") &&
                        //    (!clsType.User.DeptCode.Equals("NP") && !clsType.User.DeptCode.Equals("DM") && !clsType.User.DeptCode.Equals("OG") &&
                        //     !clsType.User.DeptCode.Equals("OS") && !clsType.User.DeptCode.Equals("NS") && !clsType.User.DeptCode.Equals("CS") &&
                        //     !clsType.User.DeptCode.Equals("UR") && !clsType.User.DeptCode.Equals("RM") && !clsType.User.DeptCode.Equals("NE") && !clsType.User.DeptCode.Equals("MI"))
                        //    )
                        //{
                        //    fWrite.FmPROGFORMNAME = "frmEmrForm_Progress_New";
                        //    fWrite.FmFORMTYPE = "4";
                        //}
                        //else
                        //{
                        //    // TODO 신규기록지 변경
                        //    fWrite.FmPROGFORMNAME = "frmEmrForm_Progress_New2";
                        //    fWrite.FmFORMTYPE = "4";
                        //}
                        #endregion

                        // TODO 신규기록지 변경
                        fWrite.FmPROGFORMNAME = "frmEmrForm_Progress_New2";
                        fWrite.FmFORMTYPE = "4";
                    }
                    else
                    {
                        fWrite.FmPROGFORMNAME = "frmEmrForm_Progress_New";
                        fWrite.FmFORMTYPE = "4";
                    }
                }

                // 경과이미지
                if (fWrite.FmFORMNO == 1232 || fWrite.FmFORMNO == 2148)
                {
                    if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(NewEmrStartDate))
                    {
                        #region // 외래 진료과별 부분오픈
                        //if (clsOrdFunction.GstrGbJob.Equals("OPD") &&
                        //    (!clsType.User.DeptCode.Equals("NP") && !clsType.User.DeptCode.Equals("DM") && !clsType.User.DeptCode.Equals("OG") &&
                        //     !clsType.User.DeptCode.Equals("OS") && !clsType.User.DeptCode.Equals("NS") && !clsType.User.DeptCode.Equals("CS") &&
                        //     !clsType.User.DeptCode.Equals("UR") && !clsType.User.DeptCode.Equals("RM") && !clsType.User.DeptCode.Equals("NE") && !clsType.User.DeptCode.Equals("MI"))
                        //    )
                        //{
                        //    fWrite.FmPROGFORMNAME = "frmEmrBaseProgressImage";
                        //    fWrite.FmFORMTYPE = "4";
                        //}
                        //else
                        //{
                        //    // TODO 신규기록지 변경
                        //    // 신규는 정형화 서식
                        //}
                        #endregion

                        // TODO 신규기록지 변경
                        // 신규는 정형화 서식
                    }
                    else
                    {
                        fWrite.FmPROGFORMNAME = "frmEmrBaseProgressImage";
                        fWrite.FmFORMTYPE = "4";
                    }
                }

                //1050~1055(동의서, 설문지, 안내문, 신청서, 정보정정관련자료, 개인정보관련동의서) 
                //1066(환자교육)
                //1068(설명문)
                if (fWrite.FmGRPFORMNO >= 1050 && fWrite.FmGRPFORMNO <= 1055 || fWrite.FmGRPFORMNO == 1066 || fWrite.FmGRPFORMNO == 1068 || fWrite.FmGRPFORMNO == 1078 || fWrite.FmGRPFORMNO == 1081)
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
                            ActiveFormWrite = new frmEmrBaseEmrChartOld(this, AcpEmr, "NEW", "", "", strFormNo, fWrite.FmFORMNAME, strEmrNo);
                            ((frmEmrBaseEmrChartOld)ActiveFormWrite).rSaveOrDelete += frmEmrBaseEmrChartOld_SaveOrDelete;
                        }
                        else
                        {
                            ActiveFormWrite = new frmEmrChartNew(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
                        }

                        //((frmEmrBaseEmrChartOld)ActiveFormWrite).rEventClosed += frmEmrBaseEmrChartOld_EventClosed;
                    }
                }
                else if (fWrite.FmFORMTYPE == "4") //개발자가 만든것
                {
                    //clsFormMap.EmrFormMapping("MHENRINS", strNameSpace, frmFORM.FmPROGFORMNAME, strFormNo, strUpdateNo, pEmrPatient, strEmrNo, "W");
                    #region 19-07-05 추가 PrtSeq, 작성일자 생성자로 넘겨주기 위해서 추가된 생성자.
                    ActiveFormWrite = clsEmrFormMap.EmrFormMappingEx(fWrite.FmPROGFORMNAME, fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", strVal, this);
                    #endregion
                    //해당 생성자 없을경우 아래로 
                    if (ActiveFormWrite == null)
                    {
                        ActiveFormWrite = clsEmrFormMap.EmrFormMappingEx(fWrite.FmPROGFORMNAME, fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, "0", "W", this);

                    }

                }
                else if (fWrite.FmFORMTYPE == "3") //Flow
                {
                    ActiveFormWrite = new frmEmrChartFlowOld(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", "", strVal, this);
                    //ActiveFormView = new frmEmrPrintFlowSheet(tForm.FmFORMNO.ToString(), tForm.FmUPDATENO.ToString(), tAcp, strEmrNo, "V", strVal, this);
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



                //응급실의 경우는 입원 기록을 작성을 할 수 있도록 : 추후 입원시에 넘기도록 한다.
                //(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
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

                ActiveFormWrite.TopLevel = false;
                this.Controls.Add(ActiveFormWrite);
                ActiveFormWrite.Parent = panEmr;
                ActiveFormWrite.Text = fWrite.FmFORMNAME;
                ActiveFormWrite.ControlBox = false;
                ActiveFormWrite.FormBorderStyle = FormBorderStyle.None;
                ActiveFormWrite.Top = 0;
                ActiveFormWrite.Left = 0;
                if (fWrite.FmALIGNGB == 1)   //Left
                {
                    panOption.Visible = false;
                    //ActiveFormWrite.Height = panEmr.Height - 20;
                    ActiveFormWrite.Dock = DockStyle.Left;
                }
                else if (fWrite.FmALIGNGB == 2)  //Top
                {
                    panOption.Visible = false;
                    //ActiveFormWrite.Width = panEmr.Width - 20;
                    ActiveFormWrite.Dock = DockStyle.Fill;
                }
                else  //None
                {
                    panOption.Visible = false;
                    ActiveFormWrite.Dock = DockStyle.Fill;
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

        private void frmEmrBaseEmrChartOld_SaveOrDelete()
        {
            ActiveFormWrite.Dispose();
            ActiveFormWrite = null;
            //GetChartHis();
        }

        private void frmEmrBaseEmrChartOld_EventClosed()
        {
            ActiveFormWrite.Dispose();
            ActiveFormWrite = null;
        }

        #endregion

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
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "ErmMain", "optMcrAllFlag", "1") == true)
            {
                clsEmrPublic.gstrMcrAllFlag = "1";
            }

            //옵션 보기 변경시 상용구 리스트 재로드
            if (ActiveFormWrite != null && ActiveFormWrite is frmEmrForm_Progress_New)
            {
                ((frmEmrForm_Progress_New)ActiveFormWrite).ChangeBoilerplate();
            }

            if (ActiveFormWrite != null && ActiveFormWrite is frmEmrForm_Progress_New2)
            {
                ((frmEmrForm_Progress_New2)ActiveFormWrite).ChangeBoilerplate();
            }
        }

        private void optMcrDept_CheckedChanged(object sender, EventArgs e)
        {
            if (optMcrDept.Checked == false)
            {
                return;
            }
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "ErmMain", "optMcrAllFlag", "2") == true)
            {
                clsEmrPublic.gstrMcrAllFlag = "2";
            }

            //옵션 보기 변경시 상용구 리스트 재로드
            if (ActiveFormWrite != null && ActiveFormWrite is frmEmrForm_Progress_New)
            {
                ((frmEmrForm_Progress_New)ActiveFormWrite).ChangeBoilerplate();
            }

            if (ActiveFormWrite != null && ActiveFormWrite is frmEmrForm_Progress_New2)
            {
                ((frmEmrForm_Progress_New2)ActiveFormWrite).ChangeBoilerplate();
            }
        }

        private void optMcrUser_CheckedChanged(object sender, EventArgs e)
        {
            if (optMcrUser.Checked == false)
            {
                return;
            }
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "ErmMain", "optMcrAllFlag", "3") == true)
            {
                clsEmrPublic.gstrMcrAllFlag = "3";
            }

            //옵션 보기 변경시 상용구 리스트 재로드
            if (ActiveFormWrite != null && ActiveFormWrite is frmEmrForm_Progress_New)
            {
                ((frmEmrForm_Progress_New)ActiveFormWrite).ChangeBoilerplate();
            }

            if (ActiveFormWrite != null && ActiveFormWrite is frmEmrForm_Progress_New2)
            {
                ((frmEmrForm_Progress_New2)ActiveFormWrite).ChangeBoilerplate();
            }
        }

        private void optMcrAdd_CheckedChanged(object sender, EventArgs e)
        {
            if (optMcrAdd.Checked == false)
            {
                return;
            }
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "ErmMain", "optMcrAddFlag", "1") == true)
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
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "ErmMain", "optMcrAddFlag", "2") == true)
            {
                clsEmrPublic.gstrMcrAddFlag = "2";
            }
        }

        private void pSetUserOption()
        {
            DataTable dt = null;

            dt = clsQuery.GetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "ErmMain", "optMcrAllFlag");
            if (dt == null)
            {
                optMcrUser.Checked = true;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                optMcrUser.Checked = true;
            }
            else
            {
                string optMcrAllFlag = Convert.ToString(VB.Val((dt.Rows[0]["OPTVALUE"].ToString() + "").Trim()));
                dt.Dispose();
                dt = null;
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

            dt = clsQuery.GetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "ErmMain", "optMcrAddFlag");
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

        #region //각종 버튼 관련

        private void btnViewChartOrHis_Click(object sender, EventArgs e)
        {
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void mbtnMacro3_Click(object sender, EventArgs e)
        {
            if (fWrite == null)
            {
                ComFunc.MsgBoxEx(this, "서식지를 선택해 주십시요.");
                return;
            }

            if (fWrite.FmFORMNO == 0)
            {
                ComFunc.MsgBoxEx(this, "서식지를 선택해 주십시요.");
                return;
            }

            if (ActiveFormWrite == null)
            {
                ComFunc.MsgBoxEx(this, "서식지를 선택해 주십시요.");
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
            //fEmrChartHisList.rSetOldChartInfo += new frmEmrChartHisList.SetOldChartInfo(frmEmrBaseUserChartForm_SetOldChartInfo);
            //fEmrChartHisList.rEventClosed += new frmEmrChartHisList.EventClosed(frmEmrChartHisList_EventClosed);
            fEmrChartHisList.StartPosition = FormStartPosition.CenterParent;
            fEmrChartHisList.ShowDialog(this);

        }

        private void frmEmrBaseUserChartForm_SetOldChartInfo(string strEmrNo, string strOldGb)
        {
            fEmrBaseUserChartForm.Dispose();
            fEmrBaseUserChartForm = null;

            if (ActiveFormWrite == null)
            {
                return;
            }

            ActiveFormWriteChart.SetChartHisMsg(strEmrNo, strOldGb);
        }

        private void frmEmrChartHisList_EventClosed()
        {
            fEmrBaseUserChartForm.Dispose();
            fEmrBaseUserChartForm = null;
        }

        private void btnViewEmr_Click(object sender, EventArgs e)
        {
            panViewLab.Visible = false;
            panViewConsult.Visible = false;
            panViewEmr.Visible = true;
        }

        private void btnChartView_Click(object sender, EventArgs e)
        {
            panViewConsult.Visible = false;
            panViewLab.Visible = false;
            panViewEmr.Visible = true;
        }

        private void btnViewLab_Click(object sender, EventArgs e)
        {
            //검사조회 창에서 닫기 눌렀을경우 null이 아님. dispose 해주고 새로 표시해준다.
            if (fViewResult != null)
            {
                if (panViewLab.Controls.Count == 0)
                {
                    fViewResult.Dispose();
                    fViewResult = null;
                    fViewResult = new frmViewResult(AcpEmr.ptNo);
                    SubFormToControl(fViewResult, panViewLab, "None", false, false);
                    Control[] controls = fViewResult.Controls.Find("panTitle", true);
                    if(controls.Length > 0)
                    {
                        controls[0].Visible = false;
                    }

                    controls = fViewResult.Controls.Find("txtPtNo", true);
                    if (controls.Length > 0 && controls[0].Text.Length == 0)
                    {
                        controls[0].Text = AcpEmr.ptNo;
                    }

                    controls = fViewResult.Controls.Find("pan1", true);
                    if (controls.Length > 0)
                    {
                        controls[0].Width = 101;
                    }

                    controls = fViewResult.Controls.Find("pan2", true);
                    if (controls.Length > 0)
                    {
                        controls[0].Top = 0;
                    }

                    fViewResult.Width -= 70;
                    fViewResult.Height -= 60;
                }
                else
                {
                    Control[] controls = fViewResult.Controls.Find("txtPtNo", true);
                    if (controls.Length > 0 && controls[0].Text.Length == 0)
                    {
                        fViewResult.rGetDate(AcpEmr.ptNo);
                    }
                }
            }

            panViewEmr.Visible = false;
            panViewConsult.Visible = false;
            panViewLab.Visible = true;
        }

        private void btnViewConsult_Click(object sender, EventArgs e)
        {
            panViewEmr.Visible = false;
            panViewLab.Visible = false;
            panViewConsult.Visible = true;
        }
        #endregion

        private void btnVital_Click(object sender, EventArgs e)
        {
            if (fEmrPatientState != null)
            {
                fEmrPatientState.Dispose();
                fEmrPatientState = null;
            }

            if (mCallOrdForm != null && mCallOrdForm.Name.Equals("FrmMedViewConsult"))
            {
                //if (clsType.User.IdNumber.Equals("4387") ||
                //    clsType.User.IdNumber.Equals("47310") ||
                //    clsType.User.IdNumber.Equals("45684") ||
                //    clsType.User.IdNumber.Equals("35623")) //||
                //    //clsType.User.IdNumber.Equals("38732")
                //{
                    fEmrPatientState = new frmPatientState_New(AcpEmr);
                //}
                //else
                //{
                //    fEmrPatientState = new frmPatientState(AcpEmr);
                //}
            }
            else
            {
                //fEmrPatientState = new frmPatientState();
                //if (clsType.User.IdNumber.Equals("4387")  ||
                //    clsType.User.IdNumber.Equals("47310") ||
                //    clsType.User.IdNumber.Equals("45684") ||
                //    clsType.User.IdNumber.Equals("35623"))
                //    //clsType.User.IdNumber.Equals("38732")
                //{
                    fEmrPatientState = new frmPatientState_New(AcpEmr);
                //}
                //else
                //{
                //    fEmrPatientState = new frmPatientState(AcpEmr);
                //}
            }
            fEmrPatientState.StartPosition = FormStartPosition.CenterParent;
            fEmrPatientState.FormClosed += FEmrPatientState_FormClosed;
            fEmrPatientState.Show(this);
        } 

        private void btnMiBi_Click(object sender, EventArgs e)
        {
            GetMiBi();
            if (fEmrTextEmrMibi != null)
            {
                fEmrTextEmrMibi.Dispose();
                fEmrTextEmrMibi = null;
            }

            fEmrTextEmrMibi = new frmTextEmrMibi();
            fEmrTextEmrMibi.StartPosition = FormStartPosition.CenterScreen;
            fEmrTextEmrMibi.rEventMiBiUserSend += frmTextEmrMibi_rEventMiBiUserSend;
            fEmrTextEmrMibi.rEventClosed += frmEmrTextEmrMibi_rEventClosed;
            fEmrTextEmrMibi.Show(this);
        }


     

        private void btnDualSign_Click(object sender, EventArgs e)
        {
            if(fEmrDualSign != null)
            {
                fEmrDualSign.Dispose();
                fEmrDualSign = null;
            }

            fEmrDualSign = new frmDualSign();
            fEmrDualSign.StartPosition = FormStartPosition.CenterScreen;
            fEmrDualSign.Show(this);
        }

        private void btnSearchRmk_Click(object sender, EventArgs e)
        {
            if (AcpEmr == null)
                return;

            using (frmEmrBaseSingularRemark frm = new frmEmrBaseSingularRemark(AcpEmr.ptNo))
            {

                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (fEmrViewer != null)
            {
                fEmrViewer.Dispose();
                fEmrViewer = null;
            }

            //fEmrViewer = new frmEmrViewer(txtPtNo.Text.Trim());
            fEmrViewer = new frmEmrViewer();
            fEmrViewer.rEventClosed += EmrViewer_rEventClosed;
            fEmrViewer.StartPosition = FormStartPosition.CenterParent;
            fEmrViewer.Show(this);
        }


        private void btnProblem_Click(object sender, EventArgs e)
        {
            if (AcpEmr == null)
                return;

            if (fEmrPatMemo != null)
            {
                fEmrPatMemo.Dispose();
                fEmrPatMemo = null;
            }

            fEmrPatMemo = new frmEmrPatMemo(AcpEmr, this);
            fEmrPatMemo.StartPosition = FormStartPosition.CenterParent;
            fEmrPatMemo.rEventClosed += FEmrPatMemo_rEventClosed;
            fEmrPatMemo.Show(this);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            panProblem.Visible = false;
        }

        /// <summary>
        /// 텍스트 챠트 복사신청 내역 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyList_Click(object sender, EventArgs e)
        {
            using (frmEMRCopyList frm = new frmEMRCopyList(AcpEmr.ptNo))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        /// <summary>
        /// 활력측정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetail1_Click(object sender, EventArgs e)
        {
            if (frmVital != null)
            {
                frmVital.Dispose();
                frmVital = null;
            }

            frmVital = new FrmVital_D(AcpEmr.ptNo);
            frmVital.StartPosition = FormStartPosition.CenterParent;
            frmVital.FormClosed += FrmVital_FormClosed;
            //frmVital.rEventClosed += FrmVital_FormClosed;
            frmVital.Show(this);
        }

        private void FrmVital_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmVital != null)
            {
                frmVital.Dispose();
                frmVital = null;
            }
        }

        /// <summary>
        /// 섭취배설
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetail2_Click(object sender, EventArgs e)
        {
            if (frmNrIO != null)
            {
                frmNrIO.Dispose();
                frmNrIO = null;
            }

            frmNrIO = new frmNrIONew2(AcpEmr);
            frmNrIO.StartPosition = FormStartPosition.CenterScreen;
            frmNrIO.FormClosed += FrmNrIO_FormClosed;
            frmNrIO.Show();
        }

        /// <summary>
        /// BST
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetail3_Click(object sender, EventArgs e)
        {
            if (fBST != null)
            {
                fBST.Dispose();
                fBST = null;
            }

            fBST = new frmBST_D(AcpEmr.ptNo);
            fBST.StartPosition = FormStartPosition.CenterScreen;
            fBST.FormClosed += FBST_FormClosed;
            fBST.Show();
        }


        private void btnClose2_Click(object sender, EventArgs e)
        {
            panForm.Visible = false;
        }

        private void FrmEmrViewMain_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F7)
            {
                if(lblSideBarLeft.Visible == false)
                {
                    btnSideBarWrite.PerformClick();
                }
                else
                {
                    lblSideBarLeft_Click(null, null);
                }
            }
        }

        /// <summary>
        /// 상병코드 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDiagCode_Click(object sender, EventArgs e)
        {
            if (fCodeSearch != null)
            {
                fCodeSearch.Dispose();
                fCodeSearch = null;
            }

            fCodeSearch = new frmCodeSearch();
            fCodeSearch.rClosed += fCodeSearch_rClosed;
            fCodeSearch.StartPosition = FormStartPosition.CenterParent;
            fCodeSearch.Show(this);
        }

        private void btnDetail4_Click(object sender, EventArgs e)
        {
            if (frmEmrPeritonealDialysisX != null)
            {
                frmEmrPeritonealDialysisX.Dispose();
                frmEmrPeritonealDialysisX = null;
            }

            frmEmrPeritonealDialysisX = new frmEmrPeritonealDialysis(AcpEmr);
            frmEmrPeritonealDialysisX.FormClosed += FCodeSearch_FormClosed;
            frmEmrPeritonealDialysisX.StartPosition = FormStartPosition.CenterParent;
            frmEmrPeritonealDialysisX.Show(this);
        }

        private void btnViewRecord_Click(object sender, EventArgs e)
        {
            if (frmEmrRecordView != null)
            {
                frmEmrRecordView.Dispose();
                frmEmrRecordView = null;
            }

            frmEmrRecordView = new frmEmrRecordView(AcpEmr);
            frmEmrRecordView.FormClosed += frmEmrRecordView_FormClosed;
            frmEmrRecordView.StartPosition = FormStartPosition.CenterParent;
            frmEmrRecordView.Show(this);
        }

        private void btnEMRCheck_Click(object sender, EventArgs e)
        {
            using (Form frm = new frmEmrBaseEmrCertify())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
            return;
        }
    }
}
