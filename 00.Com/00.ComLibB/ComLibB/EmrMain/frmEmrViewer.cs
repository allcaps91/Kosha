using ComBase;
using ComEmrBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmEmrViewer : Form, MainFormMessage, FormEmrMessage
    {
        //08924160

        #region 최상위 폼
        /// <summary>
        /// 최상위 핸들값 
        /// </summary>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern IntPtr GetForegroundWindow();


        /// <summary>
        /// 최상위 윈도우 설정
        /// </summary>
        /// <param name="hWnd">핸들</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 해당 창 최대화, 최소화, 보통 표시
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;
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
        private const int SIZE_VIEW_DEFAULT = 638;
        private const int SIZE_WRITE_DEFAULT = 666;
        private const int SIZE_VIEW_MIN = 30;
        private const int SIZE_WRITE_MIN = 30;
        #endregion //컨트롤 기본사이즈 정의 변수

        #region //폼에서 사용하는 변수

        /// <summary>
        /// //외부에서 전달받은 현재의 접수 정보 ; 차트 작성용
        /// </summary>
        //EmrPatient AcpEmr = null;

        ///// <summary>
        ///// //차트 작성용
        ///// </summary>
        //EmrPatient pWrite = null; 

        //EmrForm fWrite = null;

        /// <summary>
        /// 문제목록 작성 및 조회 화면,.
        /// </summary>
        frmEmrPatMemo fEmrPatMemo = null;

        //Form ActiveFormWrite = null;
        //EmrChartForm ActiveFormWriteChart = null;

        /// <summary>
        /// 대출 여부
        /// </summary>
        bool ReqChart = false;

        string mPTNO = string.Empty;
        //double mFORMNO_V = 0;
        //double mUPDATENO_V = 0;
        //double mFORMNO_W = 0;
        //double mUPDATENO_W = 0;

        //bool mViewNpChart = false;

        #endregion //폼에서 사용하는 변수

        #region //서브폼 선언부
        /// <summary>
        /// 검수 완료 후 수정한 내역(입퇴원 요약지)
        /// </summary>
        FrmCompleteAfterModfiy frmCompleteAfterModfiyX = null;

        /// <summary>
        /// 복사신청내역 조회
        /// </summary>
        frmEMRCopyList fEMRCopyList = null;

        /// <summary>
        /// 미비 보는 화면
        /// </summary>
        frmTextEmrMibi fEmrTextEmrMibi = null;

        /// <summary>
        /// 심사팀용 소견서
        /// </summary>
        frmMcrtJobMCBohumView frmMCBohumView = null;
        /// <summary>
        /// Vital
        /// </summary>
        Form fEmrPatientState = null; //Vital 

        /// <summary>
        /// 환자리스트
        /// </summary>
        frmEmrBaseChartView fEmrBaseChartView = null;
        /// <summary>
        /// 연속보기
        /// </summary>
        frmEmrBaseChartWrite fEmrBaseChartWrite = null;

        /// <summary>
        /// 상병코드 조회
        /// </summary>
        frmCodeSearch fCodeSearch = null;
        /// <summary>
        /// 당뇨약 사용내역
        /// </summary>
        frmBSTList fBSTList = null;

        /// <summary>
        /// 전공의 작성 챠트 확인
        /// </summary>
        frmDualSign fDualSign = null;
        /// <summary>
        /// 챠트로그 조회
        /// </summary>
        frmChartLog fChartLog = null;
        /// <summary>
        /// ER
        /// </summary>
        frmERPatient fERPatient = null;
        /// <summary>
        /// 차트 완료 조회 폼
        /// </summary>
        frmChartComplete fChartComplete = null;
        /// <summary>
        /// 퇴사자 권한 부여
        /// </summary>
        frmToiSabun_Cert fToiSabun_Cert = null;
        /// <summary>
        /// NP챠트 일괄권한 주기
        /// </summary>
        frmNPChartBatch fNPChartBatch = null;

        /// <summary>
        /// 기록실 기준 퇴원환자 조회
        /// </summary>
        frmOutPatientSearch frmMidOutSearch = null;

        /// <summary>
        /// 활력측정 폼
        /// </summary>
        Form frmVital = null;

        /// <summary>
        /// BST
        /// </summary>
        Form fBST = null;

        /// <summary>
        /// 진료정보
        /// </summary>
        Form frmDTL = null;

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
            //if (IsOrderSave == true;)
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
            //if (IsOrderSave == true;)
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

        #region //Form 기본이벤트
        public frmEmrViewer()
        {
            InitializeComponent();
        }

        //public frmEmrViewer(EmrPatient pAcpEmr)
        //{
        //    InitializeComponent();
        //    AcpEmr = pAcpEmr;
        //}

        public frmEmrViewer(string pPTNO)
        {
            mPTNO = pPTNO;
            InitializeComponent();
        }


        public frmEmrViewer(MainFormMessage pform)
        {
            mCallForm = pform;
            InitializeComponent();
        }

        public frmEmrViewer(MainFormMessage pform, string strPara)
        {
            mCallForm = pform;
            InitializeComponent();
        }

        private void frmEmrViewer_Load(object sender, EventArgs e)
        {
            ////EmrPatient AcpEmr = null;
            ////AcpEmr = clsEmrChart.ClearPatient();
            ////AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, "03983614", "O", "20170803", "GS");
            //////AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, "03983614", "I", "20170803", "MN");
            ////if (AcpEmr == null)
            ////{
            ////    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
            ////    return;
            ////}
            ///

            clsPublic.GnJobSabun = Convert.ToInt64(clsType.User.IdNumber);

            if (clsType.User.AuAVIEW.Equals("0") && clsType.User.AuAIMAGE.Equals("0"))
            {
                menuStrip1.Visible = false;
                Enabled = false;
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

            clsEmrPublic.gUserGrade = clsEmrFunc.SET_GRADE();

            if (clsType.User.BuseCode.Equals("044201") || clsEmrPublic.gUserGrade.Equals("SIMSA"))
            {
                btnDetail1.Visible = false;
                btnDetail2.Visible = false;
                btnDetail3.Visible = false;
                //mbtnOption.Visible = false;
            }

            #region 약제팀 검색 못하게 막음.
            if (clsType.User.BuseCode.Equals("044101"))
            {
                if (clsPublic.GnJobSabun != 48579)
                {
                    txtPtNo.Enabled = false;
                }
            }
            #endregion

            GetMiBi();
            MENU_SET();
            //READ_TOTAL_DATE();
            #region --
            clsEmrPublic.gDateSET = true;
            mnuDateSET1.Text = "전체기간 - 적용";
            #endregion

            FormInit();

            viewFormMonitor2();

            this.WindowState = FormWindowState.Maximized;

            ResizeForm();

            //GetDualSign();

            #region 물리치료실은 작성화면 보이게
            if (clsType.User.BuseCode.Equals("055307") == false)
            {
                panViewEmrMain.Visible = true;
                panWriteMain.Visible = false;
            }
            #endregion

            txtPtNo.Focus();

            if (mPTNO != "")
            {
                txtPtNo.Text = mPTNO;
                ClearPatInfo();
                //조회한 환자가 있으면 내역을 업데이트 한다
                //SaveChartView("");
                GetPatientInfoSearch();

                //conPatInfo1.SetDisPlay(clsType.User.IdNumber, AcpEmr.inOutCls, AcpEmr.medFrDate, AcpEmr.ptNo, AcpEmr.medDeptCd);
                txtPtNo.Focus();
                txtPtNo.SelectAll();
            }

            #region 물리치료실은 작성화면 보이게
            if (clsType.User.BuseCode.Equals("055307"))
            {
                btnWrite.PerformClick();
            }
            #endregion

        }

        private void viewFormMonitor2()
        {
            //이채경 판독 과장님 현재 모니터에서 뜨게.
            if (clsType.User.BuseCode.Equals("077402") || clsType.User.IdNumber.Equals("22115"))
            {
                return;
            }

            Screen[] screens = Screen.AllScreens;
            Screen secondary_screen = null;

            if (screens.Length == 1)    //모니터 하나
            {
                this.Show();
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                bool isHasAccuMonitor = false;
                Screen originalScreen = null;
                foreach (Screen screen in screens)
                {
                    if (screen.WorkingArea.Width == 720)
                    {
                        isHasAccuMonitor = true;
                    }
                    else
                    {
                        originalScreen = screen;
                    }
                }
                if (isHasAccuMonitor)
                {
                    this.Bounds = originalScreen.Bounds;
                    this.WindowState = FormWindowState.Maximized;
                    this.Show();
                }
                else
                {
                    Screen MaxScreen = Screen.AllScreens.Where(d => d.Primary == false).FirstOrDefault();

                    if (MaxScreen == null)
                    {
                        return;
                    }

                    #region 영상의학과 과장님 3번째 모니터
                    if (clsType.User.IdNumber.Equals("22115") && MaxScreen.Equals(Screen.AllScreens[2]) == false)
                    {
                        MaxScreen = Screen.AllScreens[2];
                    }
                    #endregion

                    secondary_screen = MaxScreen;
                    this.Bounds = secondary_screen.Bounds;
                    //this.Top = 0;
                    //this.Left = 0;
                    this.WindowState = FormWindowState.Maximized;
                    if (this.IsDisposed == false)
                    {
                        this.Show();
                    }

                    #region 이전
                    //foreach (Screen screen in screens)
                    //{
                    //    if (screen.Primary == false)
                    //    {
                    //        secondary_screen = screen;
                    //        this.Bounds = secondary_screen.Bounds;
                    //        //this.Top = 0;
                    //        //this.Left = 0;
                    //        this.WindowState = FormWindowState.Maximized;
                    //        if (this.IsDisposed == false)
                    //        {
                    //            this.Show();
                    //        }
                    //        break;
                    //    }
                    //}
                    #endregion
                }


            }
        }

        /// <summary>
        /// 미비 리스트 표시
        /// </summary>
        private void GetMiBi()
        {
            if(clsType.User.DrCode.Length == 0)
            {
                btnMiBi.Visible = false;
                return;
            }
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
            if (clsType.User.BuseCode == "044201")
            {
                strSql = strSql + ComNum.VBLF + "                    WHERE BUSE IN ('022101','022105', '022150'))";
            }
            strSql = strSql + ComNum.VBLF + "     AND A.FORMNO NOT IN ('963','1232')";
            strSql = strSql + ComNum.VBLF + "     AND NOT EXISTS";
            strSql = strSql + ComNum.VBLF + "   ( SELECT * FROM ADMIN.EMRXML_DUALSIGN SUB";
            strSql = strSql + ComNum.VBLF + "       WHERE SUB.EMRNO = A.EMRNO)";

            string sqlErr = clsDB.GetAdoRs(ref reader, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(sqlErr, sqlErr, clsDB.DbCon);
                ComFunc.MsgBox(sqlErr);
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


        /// <summary>
        /// 메뉴 권한 설정
        /// </summary>
        void MENU_SET()
        {

            if(clsEmrPublic.gUserGrade.Equals("SIMSA"))
            {
                mnuJin.Visible = true;
                mnuConfigView.Visible = true;
                mnuMCadmin2.Visible = true;
                mnuDateSET.Visible = true;
                mnuDateSET1.Visible = true;
            }
            else
            {
                mnuConfigView.Visible = false;
                mnuJin.Visible = false;
                mnuMCadmin2.Visible = false;
                mnuDateSET.Visible = false;
                mnuDateSET1.Visible = false;
            }

            mnuTest.Visible = clsType.User.DrCode.Length > 0;

            if (clsEmrPublic.gUserGrade.Equals("WRITE"))
            {
                //menuStrip1.Visible = true;
                mnuModify.Visible = true;
                mnuToiSabun_Cert.Visible = true;
                mnuModify.Visible  = true;
                mnuCertA_M.Visible = true;
                mnuNPCHART.Visible = true;
                mnuComplete.Visible = true;
                mnuTest.Visible = true;
            }
            else
            {
                mnuModify.Visible = false;
                mnuToiSabun_Cert.Visible = false;
                mnuModify.Visible  = false;
                mnuCertA_M.Visible = false;
                mnuNPCHART.Visible = false;
                mnuComplete.Visible = false;
            }

            //'2019-02-13 영상의학과의 경우 모든 메뉴 불활성화
            if (clsEmrPublic.gUserGrade.Equals("XRAY"))
            {
                menuStrip1.Visible = false;
            }

            #region
            mnuChartLog.Visible = false;
            string SQL = string.Empty;
            OracleDataReader reader = null;

            try
            {
                SQL = "SELECT 1 AS CNT";
                SQL = SQL + ComNum.VBLF + " FROM DUAL";
                SQL = SQL + ComNum.VBLF + " WHERE EXISTS";
                SQL = SQL + ComNum.VBLF + " (";
                SQL = SQL + ComNum.VBLF + " SELECT 1";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "    WHERE GUBUN = 'EMR_권역평가' ";
                SQL = SQL + ComNum.VBLF + "      AND CODE = '과거력' ";
                SQL = SQL + ComNum.VBLF + "      AND NAME = 'Y'";
                SQL = SQL + ComNum.VBLF + " )";

                string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);

                if (SqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (reader.HasRows && reader.Read() && reader.GetValue(0).ToString().Trim().Equals("0") == false)
                {
                    mnuChartLog.Visible = true;
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            clsEmrPublic.ModifyCert = clsEmrFunc.TOISABUN_CERT_B(clsDB.DbCon, clsType.User.IdNumber);
            #endregion

            clsEmrPublic.GstrView01 = "0";
        }

        void READ_TOTAL_DATE()
        {
            #region
            clsEmrPublic.gDateSET = false;
            string SQL = string.Empty;
            OracleDataReader reader = null;

            try
            {
                SQL = " SELECT USED FROM ADMIN.EMR_OPTION_TOTALDATE ";
                SQL = SQL + ComNum.VBLF + " WHERE USEID = " + clsType.User.IdNumber;

                string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);

                if (SqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (reader.HasRows && reader.Read() && reader.GetValue(0).ToString().Trim().Equals("1") == false)
                {
                    clsEmrPublic.gDateSET = true;
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            mnuDateSET1.Text = clsEmrPublic.gDateSET ? "전체기간 - 적용" : "전체기간 - 미적용";
            #endregion
        }


        private void frmEmrViewer_Resize(object sender, EventArgs e)
        {
            ResizeForm();
        }

        private void FormInit()
        {
            panView.Dock = DockStyle.Fill;
            panView.BringToFront();

            panViewEmrMain.Dock = DockStyle.Fill;
            panWriteMain.Dock = DockStyle.Fill;

            pSetUserOption();
            LoadSubForm();
        }

        private void LoadSubForm()
        {
            panViewEmrMain.Visible = true;
            panViewEmrMain.BringToFront();

            fEmrBaseChartView = new frmEmrBaseChartView();
            //fEmrBaseChartView.rEventClosed += new frmEmrBaseChartView.EventClosed(frmEmrBaseChartView_EventClosed);
            if (fEmrBaseChartView != null)
            {
                SubFormToControl(fEmrBaseChartView, panViewEmrMain);
            }
            Application.DoEvents();
            panViewEmrMain.Visible = false;
            panWriteMain.Visible = true;
            panWriteMain.BringToFront();

            fEmrBaseChartWrite = new frmEmrBaseChartWrite();
            //fEmrBaseChartWrite.rEventClosed += new frmEmrBaseChartWrite.EventClosed(frmEmrBaseChartWrite_EventClosed);
            if (fEmrBaseChartWrite != null)
            {
                SubFormToControl(fEmrBaseChartWrite, panWriteMain);
            }
        }

        private void ResizeForm()
        {
            try
            {
                panViewEmrMain.Width = panView.Width;
                panViewEmrMain.Height = panView.Height;
                Application.DoEvents();
                panWriteMain.Width = panView.Width;
                panWriteMain.Height = panView.Height;
                Application.DoEvents();
                Application.DoEvents();
                if (fEmrBaseChartView != null)
                {
                    fEmrBaseChartView.WindowState = FormWindowState.Normal;
                    fEmrBaseChartView.Height = panViewEmrMain.Height;
                    fEmrBaseChartView.Width = panViewEmrMain.Width;
                }
                Application.DoEvents();
                if (fEmrBaseChartWrite != null)
                {
                    fEmrBaseChartWrite.WindowState = FormWindowState.Normal;
                    fEmrBaseChartWrite.Height = panWriteMain.Height;
                    fEmrBaseChartWrite.Width = panWriteMain.Width;
                }
                Application.DoEvents();
            }
            catch
            {

            }
        }
        #endregion

        #region //SubForm Event

        private void FBST_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fBST != null)
            {
                fBST.Dispose();
                fBST = null;
            }
        }

        private void FrmVital_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmVital != null)
            {
                frmVital.Dispose();
                frmVital = null;
            }
        }

        private void frmTextEmrMibi_rEventMiBiUserSend(string strPtNo, string strInDate, string strOutDate)
        {
            GetMiBi();
            Control[] controls = panViewEmrMain.Controls.Find("frmEmrBaseAcpList", true);
            if (controls.Length > 0)
            {
                frmEmrBaseAcpList frm = (frmEmrBaseAcpList)controls[0];

                if (mPTNO != strPtNo)
                {
                    frm.ClearForm();
                    mPTNO = strPtNo;
                    txtPtNo.Text = mPTNO;
                    ClearPatInfo();
                    GetPatientInfoSearch();
                }

                frm.SetMibiCell(strInDate, strOutDate);
                return;
            }
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

        private void frmEMRCopyList_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fEMRCopyList != null)
            {
                fEMRCopyList.Dispose();
                fEMRCopyList = null;
            }
        }

        private void FrmDTL_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmDTL != null)
            {
                frmDTL.Dispose();
                frmDTL = null;
            }
        }

        private void FrmMCBohumView_rClosed()
        {
            if (frmMCBohumView != null)
            {
                frmMCBohumView.Dispose();
                frmMCBohumView = null;
            }
        }

        private void FNPChartBatch_rClosed()
        {
            if (fNPChartBatch != null)
            {
                fNPChartBatch.Dispose();
                fNPChartBatch = null;
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

        private void FToiSabun_Cert_rClosed()
        {
            if (fToiSabun_Cert != null)
            {
                fToiSabun_Cert.Dispose();
                fToiSabun_Cert = null;
            }
        }

        private void FrmMidOutSearch_rClosed()
        {
            if (frmMidOutSearch != null)
            {
                frmMidOutSearch.Dispose();
                frmMidOutSearch = null;
            }
        }

        private void FChartLog_rClosed()
        {
            if (fChartLog != null)
            {
                fChartLog.Dispose();
                fChartLog = null;
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

        private void frmEmrBaseChartWrite_EventClosed()
        {
            if (fEmrBaseChartWrite != null)
            {
                fEmrBaseChartWrite.Close();
                fEmrBaseChartWrite.Dispose();
                fEmrBaseChartWrite = null;
            }
        }

        private void frmEmrBaseChartView_EventClosed()
        {
            if (fEmrBaseChartView != null)
            {
                fEmrBaseChartView.Close();
                fEmrBaseChartView.Dispose();
                fEmrBaseChartView = null;
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

        private void FERPatient_rClosed()
        {
            if (fERPatient != null)
            {
                fERPatient.Dispose();
                fERPatient = null;
            }
        }

        private void FChartComplete_rClosed()
        {
            if (fChartComplete != null)
            {
                fChartComplete.Dispose();
                fChartComplete = null;
            }
        }

        private void FBSTList_rClosed()
        {
            if (fBSTList != null)
            {
                fBSTList.Dispose();
                fBSTList = null;
            }
        }

        private void fDualSign_rClosed()
        {
            if (fDualSign != null)
            {
                fDualSign.Dispose();
                fDualSign = null;
            }
        }

        private void frmERPatient_rSendPatInfo(string strPtNo)
        {
            txtPtNo.Text = strPtNo;
            ClearPatInfo();
            //조회한 환자가 있으면 내역을 업데이트 한다
            //SaveChartView("");
            GetPatientInfoSearch();
        }


        private void FrmChartComplete_rSendPatInfo(string strPtNo, string strOutDate)
        {
            txtPtNo.Text = strPtNo;
            ClearPatInfo();
            GetPatientInfoSearch();

            frmEmrBaseAcpList frm = (frmEmrBaseAcpList)fEmrBaseChartView.Controls.Find("frmEmrBaseAcPList", true)[0];
            frm.SetOutDateSearch(strOutDate);
        }

        private void FrmOutPatient_rSendPatInfo(string strPtNo)
        {
            txtPtNo.Text = strPtNo;
            ClearPatInfo();
            GetPatientInfoSearch();
        }

        private void frmCompleteAfterModfiyX_rEventClosed()
        {
            if (frmCompleteAfterModfiyX != null)
            {
                frmCompleteAfterModfiyX.Dispose();
                frmCompleteAfterModfiyX = null;
            }
        }

        private void frmCompleteAfterModfiyX_rEventCompleUserSend(string strPtNo, string strOutDate)
        {
            txtPtNo.Text = strPtNo;
            ClearPatInfo();
            GetPatientInfoSearch();

            frmEmrBaseAcpList frm = (frmEmrBaseAcpList)fEmrBaseChartView.Controls.Find("frmEmrBaseAcPList", true)[0];
            frm.SetOutDateSearch(strOutDate);
        }


        #endregion

        #region //환자 정보 조회

        /// <summary>
        /// 외부에서 환자정보를 받아서 갱신을 할 경우
        /// </summary>
        /// <param name="pAcpEmr"></param>
        public void SetNewPatient(string strPano)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }

            if (clsType.User.AuAVIEW.Equals("0") && clsType.User.AuAIMAGE.Equals("0"))
            {
                menuStrip1.Visible = false;
                Enabled = false;
                ComFunc.MsgBoxEx(this, "조회 권한이 없습니다.");
                Close();
                return;
            }

            if (!GetForegroundWindow().Equals(Handle) && clsType.User.BuseCode.Equals("078201") == false)
            {
                // 윈도우가 최소화 되어 있다면 활성화 시킨다
                ShowWindowAsync(Handle, SW_SHOWMAXIMIZED);

                // 윈도우에 포커스를 줘서 최상위로 만든다
                SetForegroundWindow(Handle);
            }

            if (strPano.Equals(txtPtNo.Text.Trim()))
                return;

            txtPtNo.Text = strPano;
            ClearPatInfo();
            
            //조회한 환자가 있으면 내역을 업데이트 한다
            //SaveChartView("");
            GetPatientInfoSearch();

            if (clsType.User.BuseCode.Equals("055307"))
            {
                panViewEmrMain.Visible = false;
                panWriteMain.Visible = true;
            }
        }

        /// <summary>
        /// 진료 미비용도
        /// </summary>
        /// <param name="pAcpEmr"></param>
        public void SetNewPatient(string strPano, string strFrDate, string strOutDate)
        {
            txtPtNo.Text = strPano;
            ClearPatInfo();
            GetPatientInfoSearch();
            frmTextEmrMibi_rEventMiBiUserSend(strPano, strFrDate, strOutDate);
        }

        /// <summary>
        /// EKG클릭
        /// </summary>
        /// <param name="ROWID"></param>
        public void SetEkg(string ROWID)
        {
            frmEmrBaseAcpList frm = (frmEmrBaseAcpList)fEmrBaseChartView.Controls.Find("frmEmrBaseAcPList", true)[0];
            frm.SetEkg(ROWID);
        }

        private void ClearPatInfo()
        {
            //AcpEmr = null;
            lblName.Text = "";
            if (fEmrBaseChartView != null)
            {
                fEmrBaseChartView.ClearForm();
            }

            if (fEmrBaseChartWrite != null)
            {
                fEmrBaseChartWrite.ClearForm();
            }

            panViewEmrMain.Visible = true;
            panWriteMain.Visible = false;
            //btnWrite.ForeColor = Color.Black;
            //btnView.ForeColor = Color.Black;
        }

        private void GetPatientInfoSearch()
        {
            if (txtPtNo.Text.Trim() == "")
            {
                return;
            }

            ReqChart = false;
            string strPtNo = txtPtNo.Text.Trim();
            //int intResult = -1;

            if (VB.IsNumeric(strPtNo) == false)
            {
                if (strPtNo.Length < 2)
                {
                    ComFunc.MsgBoxEx(this, "조회 하고자 하는 이름은 2자리 이상만 가능합니다!.");
                    txtPtNo.Focus();
                    return;
                }
                if (GetPatientInfo("", strPtNo) == 1)
                {
                    if (GetPatInfo() == false)
                        return;

                    fEmrBaseChartView.GetJupHis(txtPtNo.Text.Trim());
                    if(fEmrBaseChartWrite != null)
                    {
                        fEmrBaseChartWrite.ClearForm();
                        fEmrBaseChartWrite.GetJupHis(txtPtNo.Text.Trim());
                    }
                }
                else
                {
                    using (frmEmrPatientInfoSearch frmEmrPatientInfo = new frmEmrPatientInfoSearch(strPtNo))
                    {
                        frmEmrPatientInfo.rSendPatInfo += FrmEmrPatientInfo_rSendPatInfo;
                        frmEmrPatientInfo.StartPosition = FormStartPosition.CenterParent;
                        frmEmrPatientInfo.ShowDialog(this);
                        frmEmrPatientInfo.rSendPatInfo -= FrmEmrPatientInfo_rSendPatInfo;
                    }
                }
            }
            else
            {
                strPtNo = ComFunc.SetAutoZero(strPtNo, 8);

                if (GetPatientInfo(strPtNo, "") == 1)
                {
                    if (GetPatInfo() == false)
                        return;

                    fEmrBaseChartView.GetJupHis(txtPtNo.Text.Trim());
                    if (fEmrBaseChartWrite != null)
                    {
                        fEmrBaseChartWrite.ClearForm();
                        fEmrBaseChartWrite.GetJupHis(txtPtNo.Text.Trim());
                    }
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

        private bool GetPatInfo()
        {
            bool rtnVal = false;
            //if (clsVbfunc.EtcViewCert(clsDB.DbCon, txtPtNo.Text.Trim(), clsType.User.Sabun, btnSearchPt) == false)
            //{
            //    ComFunc.MsgBoxEx(this, "챠트조회는 재원자 및 당일 진료자만 조회가 가능합니다.");
            //    txtPtNo.Enabled = true;
            //    txtPtNo.Clear();
            //    return;
            //}
            #region 외래 : 내원내역이 있으면, 입원 재원 환자의 경우는 차트 대출 신청을 하지 않는다.
            if (IsNowPatient(txtPtNo.Text.Trim()) == false)
            {
                if (clsType.User.BuseCode.Equals("044201") == false)
                {
                    #region 여기서 차트대출신청이 일어나지 않으면 빠져나간다.
                    ReqChart = SetReqChart(txtPtNo.Text.Trim());
                    if (ReqChart == false)
                    {
                        ComFunc.MsgBoxEx(this, "차트대출신청이 되지 않았습니다.\r\n차트조회를 실행할 수 없습니다.");
                        return rtnVal;
                    }
                    #endregion
                }
            }

            clsEmrQuery.CREATE_CHARTVIEW_LOG(clsDB.DbCon, txtPtNo.Text.Trim(), "END");
            rtnVal = true;
            return rtnVal;
            #endregion
        }

        /// <summary>
        ///  외래 : 내원내역이 있으면, 입원 재원 환자의 경우는 차트 대출 신청을 하지 않는다.
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <returns></returns>
        bool IsNowPatient(string strPtNo)
        {
            bool rtnVal = false;

            OracleDataReader reader = null;

            string strSql = " SELECT PANO";
            strSql += ComNum.VBLF + "    FROM ADMIN.OPD_MASTER";
            strSql += ComNum.VBLF + " WHERE PANO = '" + strPtNo + "'";
            strSql += ComNum.VBLF + "  AND TO_CHAR(BDATE,'YYYY-MM-DD') = TO_CHAR(SYSDATE,'YYYY-MM-DD')";
            strSql += ComNum.VBLF + " UNION ALL";
            strSql += ComNum.VBLF + " SELECT PANO";
            strSql += ComNum.VBLF + "    FROM ADMIN.IPD_NEW_MASTER";
            strSql += ComNum.VBLF + " WHERE PANO = '" + strPtNo + "'";
            strSql += ComNum.VBLF + "      AND TO_CHAR(INDATE,'YYYYMMDD') <= TO_CHAR(SYSDATE,'YYYYMMDD')";
            strSql += ComNum.VBLF + "      AND (OUTDATE IS NULL OR TO_CHAR(OUTDATE,'YYYYMMDD') >= TO_CHAR(SYSDATE,'YYYYMMDD'))";

            string sqlErr = clsDB.GetAdoRs(ref reader, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(sqlErr, sqlErr, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, sqlErr);
                return rtnVal;
            }

            if (reader.HasRows)
            {
                rtnVal = true;
            }

            reader.Dispose();
            return rtnVal;
        }

        bool SetReqChart(string strPtNo)
        {
            bool rtnVal = false;

            string strSayuYes = GetReqChart(strPtNo, clsType.User.IdNumber, ComQuery.CurrentDateTime(clsDB.DbCon, "D"));
            string strSayu = string.Empty;
            string strSayuRmk = string.Empty;
            string[] sPara;

            if (string.IsNullOrWhiteSpace(strSayuYes) == false)
            {
                sPara = strSayuYes.Split('^');
                strSayu = sPara[0];
                strSayuRmk = sPara[1];

                rtnVal = clsEmrQuery.SetViewLog(clsDB.DbCon, txtPtNo.Text.Trim(), clsType.User.IdNumber, clsType.User.UserName, strSayu, strSayuRmk);
                return rtnVal;
            }

            string sDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string sDateF = ComQuery.CurrentDateTime(clsDB.DbCon, "S").Substring(0, 10);
            string sTime = ComQuery.CurrentDateTime(clsDB.DbCon, "T");
            string sDateE = Convert.ToDateTime(sDateF).AddDays(7).ToString("yyyyMMdd");

            if (clsType.User.BuseCode.Equals("078201") || clsType.User.BuseCode.Equals("077405"))
            {
                strSayu = "010";
                strSayuRmk = "";

                if (SetChartReq(txtPtNo.Text.Trim(), clsType.User.IdNumber, sDate, sDateE, strSayu, strSayuRmk, "", sDate, sTime) == false)
                {
                    return false;
                }

                if (clsEmrQuery.SetViewLog(clsDB.DbCon, txtPtNo.Text.Trim(), clsType.User.IdNumber, clsType.User.UserName, strSayu, strSayuRmk) == false)
                {
                    return false;
                }
            }
            else
            {
                using(frmEmrSayu fEmrSayu = new frmEmrSayu())
                {
                    fEmrSayu.StartPosition = FormStartPosition.CenterParent;
                    fEmrSayu.ShowDialog(this);
                }

                strSayu = clsEmrPublic.mstrSayu;
                strSayuRmk = clsEmrPublic.mstrSayuRemark;
            }

            if (string.IsNullOrWhiteSpace(strSayu))
            {
                return rtnVal;
            }

            if (SetChartReq(txtPtNo.Text.Trim(), clsType.User.IdNumber, sDate, sDateE, strSayu, strSayuRmk, "", sDate, sTime) == false)
            {
                return rtnVal;
            }

            if (clsEmrQuery.SetViewLog(clsDB.DbCon, txtPtNo.Text.Trim(), clsType.User.IdNumber, clsType.User.UserName, strSayu, strSayuRmk) == false)
            {
                return rtnVal;
            }

            rtnVal = true;

            return rtnVal;
        }


        public static bool SetChartReq(string strPtNo, string strUseId, string strREQSTDDATE, string strREQENDDATE, string strREQTYPE, string strREQMEMO, string strREQTEL, string strREQDATE, string strREQTIME)
        {
            if (strUseId.Equals("4349"))
            {
                return true;
            }

            bool rtnVal = false;

            string SQL = string.Empty;
            string sqlErr = string.Empty;

            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL += ComNum.VBLF + " INSERT INTO ADMIN.EMRCHARREQ ";
                SQL += ComNum.VBLF + "  (PTNO,REQUSEID,REQSTDDATE,REQENDDATE,";
                SQL += ComNum.VBLF + "  REQTYPE,REQMEMO,REQTEL,REQDATE,REQTIME,";
                SQL += ComNum.VBLF + "  CONUSEID,CONDATE,CONTIME,CONNUM,CONGB,";
                SQL += ComNum.VBLF + "  CONTYPE,CONMEMO)";
                SQL += ComNum.VBLF + "  Values(";
                SQL += ComNum.VBLF + "  '" + strPtNo + "',";
                SQL += ComNum.VBLF + "  '" + strUseId + "',";
                SQL += ComNum.VBLF + "  '" + strREQSTDDATE + "',";
                SQL += ComNum.VBLF + "  '" + strREQENDDATE + "',";
                SQL += ComNum.VBLF + "  '" + strREQTYPE.Trim() + "',";
                SQL += ComNum.VBLF + "  '" + strREQMEMO.Trim() + "',";
                SQL += ComNum.VBLF + "  '" + strREQTEL.Trim() + "',";
                SQL += ComNum.VBLF + "  '" + strREQDATE + "',";
                SQL += ComNum.VBLF + "  '" + strREQTIME + "',";
                SQL += ComNum.VBLF + "  '16109',";
                SQL += ComNum.VBLF + "  TO_CHAR(SYSDATE, 'YYYYMMDD'),";
                SQL += ComNum.VBLF + "  TO_CHAR(SYSDATE, 'HH24MISS'),";
                SQL += ComNum.VBLF + "  ADMIN.EMRCHARREQ_SEQ.NextVal,";
                SQL += ComNum.VBLF + "  '1',";
                SQL += ComNum.VBLF + "  '1',";
                SQL += ComNum.VBLF + "  '')";

                sqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return rtnVal;
                }

                rtnVal = true;
                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("차트 열람 사유 입력 도중 오류\r\n" + ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }


        string GetReqChart(string strPtNo, string strUseId, string strDate)
        {
            string rtnVal = string.Empty;
            OracleDataReader reader = null;

            string strSql = " SELECT A.REQTYPE, A.REQMEMO";
            strSql += ComNum.VBLF + "    FROM ADMIN.EMRCHARREQ A ";
            strSql += ComNum.VBLF + " WHERE A.PTNO = '" + strPtNo + "'";
            strSql += ComNum.VBLF + "      AND A.REQUSEID = '" + strUseId + "'";
            strSql += ComNum.VBLF + "      AND A.REQSTDDATE <= '" + strDate + "'";
            strSql += ComNum.VBLF + "      AND A.REQENDDATE >= '" + strDate + "'";
            strSql += ComNum.VBLF + "      AND A.CONGB = '1'";

            string sqlErr = clsDB.GetAdoRs(ref reader, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(sqlErr, sqlErr, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, sqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim() + "^" + reader.GetValue(1).ToString().Trim();
            }

            reader.Dispose();

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
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
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

                string strDblChart = clsEmrChart.GetDoubleChart(txtPtNo.Text.Trim());
                if (string.IsNullOrEmpty(strDblChart) == false)
                {
                    lblName.Text = strDblChart;
                }
                else
                {
                    lblName.Text = dt.Rows[0]["PTNAME"].ToString().Trim();
                }

                rtnVal = 1;

                clsEmrQuery.CREATE_CHARTVIEW_LOG(clsDB.DbCon, txtPtNo.Text.Trim(), "START", ReqChart ? "1" : "");
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

        private void btnView_Click(object sender, EventArgs e)
        {
            btnView.ForeColor = Color.Blue;
            btnWrite.ForeColor = Color.Black;

            panViewEmrMain.Visible = true;
            panWriteMain.Visible = false;
            //btnWrite.ForeColor = Color.DarkGray;
            //btnView.ForeColor = Color.Black;
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            btnView.ForeColor = Color.Black;
            btnWrite.ForeColor = Color.Blue;

            panWriteMain.Visible = true;
            panViewEmrMain.Visible = false;
            //btnView.ForeColor = Color.DarkGray;
            //btnWrite.ForeColor = Color.Black;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if(rEventClosed != null)
            {
                rEventClosed();
            }
            else
            {
                this.Close();
            }
        }

        private void txtPtNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (clsType.User.AuAVIEW == "0" && clsType.User.AuAIMAGE == "0")
                {
                    ComFunc.MsgBoxEx(this, "조회 권한이 없습니다.");
                    return;
                }
                ClearPatInfo();
                //조회한 환자가 있으면 내역을 업데이트 한다
                //SaveChartView("");
                GetPatientInfoSearch();
            }
        }

        private void FrmEmrPatientInfo_rSendPatInfo(string strPtNo, string strPtName)
        {

            if (GetPatientInfo(strPtNo, strPtName) == 1)
            {
                if (GetPatInfo() == false)
                    return;
            }

            txtPtNo.Text = strPtNo;

            string strDblChart = clsEmrChart.GetDoubleChart(txtPtNo.Text.Trim());
            if (string.IsNullOrEmpty(strDblChart) == false)
            {
                lblName.Text = strDblChart;
            }
            else
            {
                lblName.Text = strPtName;
            }

            fEmrBaseChartView.GetJupHis(txtPtNo.Text.Trim());

            if(fEmrBaseChartWrite != null)
            {
                fEmrBaseChartWrite.ClearForm();
                fEmrBaseChartWrite.GetJupHis(txtPtNo.Text.Trim());
            }

            clsEmrQuery.CREATE_CHARTVIEW_LOG(clsDB.DbCon, txtPtNo.Text.Trim(), "START", ReqChart ? "1" : "");
        }

        private void btnSearchPt_Click(object sender, EventArgs e)
        {
            ClearPatInfo();
            //조회한 환자가 있으면 내역을 업데이트 한다
            //SaveChartView("");
            GetPatientInfoSearch();
        }

        private void BtnProblem_Click(object sender, EventArgs e)
        {
            if (fEmrPatMemo != null)
            {
                fEmrPatMemo.Dispose();
                fEmrPatMemo = null;
            }

            fEmrPatMemo = new frmEmrPatMemo(mPTNO, this);
            fEmrPatMemo.StartPosition = FormStartPosition.CenterScreen;
            fEmrPatMemo.rEventClosed += FEmrPatMemo_rEventClosed;
            fEmrPatMemo.Show();
        }


        private void FrmEmrViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");


            #region 당일 스캔 이미지 지우기
            int ChartCopyForm = Application.OpenForms.Cast<Form>().Where(f => f.Name.Equals("frmEmrJobChartCopy")).Count();
            if (ChartCopyForm == 0)
            {
                string mstrViewPath = @"C:\PSMHEXE\ScanTmp\Formname\\" + strCurDate;
                if (Directory.Exists(mstrViewPath))
                {
                    clsImgcvt.DelAllFile(mstrViewPath);
                }
            }

            clsEmrQuery.CREATE_CHARTVIEW_LOG(clsDB.DbCon, txtPtNo.Text.Trim(), "END");

            //if (Directory.Exists(mstrViewPath))
            //{
            //    DirectoryInfo dir = new DirectoryInfo(mstrViewPath);

            //    System.IO.FileInfo[] files = dir.GetFiles("*.*",

            //    SearchOption.AllDirectories);

            //    foreach (System.IO.FileInfo file in files)

            //        file.Attributes = FileAttributes.Normal;


            //    Directory.Delete(mstrViewPath, true);
            //}
            #endregion

            if (frmDTL != null)
            {
                frmDTL.Dispose();
                frmDTL = null;
            }

            if (fBST != null)
            {
                fBST.Dispose();
                fBST = null;
            }

            if (frmCompleteAfterModfiyX != null)
            {
                frmCompleteAfterModfiyX.Dispose();
                frmCompleteAfterModfiyX = null;
            }

            if (fEMRCopyList != null)
            {
                fEMRCopyList.Dispose();
                fEMRCopyList = null;
            }

            if (frmVital != null)
            {
                frmVital.Dispose();
            }

            if (fEmrPatientState != null)
            {
                fEmrPatientState.Dispose();
                fEmrPatientState = null;
            }

            fDualSign_rClosed();

            FBSTList_rClosed();

            FrmMCBohumView_rClosed();

            FNPChartBatch_rClosed();

            FToiSabun_Cert_rClosed();

            FrmMidOutSearch_rClosed();

            FChartLog_rClosed();

            fCodeSearch_rClosed();

            FERPatient_rClosed();

            FChartComplete_rClosed();

            frmEmrBaseChartWrite_EventClosed();

            frmEmrBaseChartView_EventClosed();

            FEmrPatMemo_rEventClosed();

            if (rEventClosed != null)
            {
                rEventClosed();
            }


            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        #region //Option Panel
        private void mbtnOption_Click(object sender, EventArgs e)
        {
            pSetUserOption();
            panOption.Top = 200;
            panOption.Left = this.Width - panOption.Width - 50;
            panOption.Visible = true;;
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
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, clsType.User.IsNurse.Equals("OK") ? "EmrNrMain" : "ErmMain", "optMcrAllFlag", "1") == true)
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
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, clsType.User.IsNurse.Equals("OK") ? "EmrNrMain" : "ErmMain", "optMcrAllFlag", "2") == true)
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
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, clsType.User.IsNurse.Equals("OK") ? "EmrNrMain" : "ErmMain", "optMcrAllFlag", "3") == true)
            {
                clsEmrPublic.gstrMcrAllFlag = "3";
            }
        }

        private void optMcrAdd_CheckedChanged(object sender, EventArgs e)
        {
            if (optMcrAdd.Checked == false)
            {
                return;
            }
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, clsType.User.IsNurse.Equals("OK") ? "EmrNrMain" : "ErmMain", "optMcrAddFlag", "1") == true)
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
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, clsType.User.IsNurse.Equals("OK") ? "EmrNrMain" : "ErmMain", "optMcrAddFlag", "2") == true)
            {
                clsEmrPublic.gstrMcrAddFlag = "2";
            }
        }

        private void pSetUserOption()
        {
            DataTable dt = null;

            dt = clsQuery.GetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, clsType.User.IsNurse.Equals("OK") ? "EmrNrMain" : "ErmMain", "optMcrAllFlag");
            if (dt == null)
            {
                optMcrUser.Checked = true;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                optMcrUser.Checked = true;
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

            dt = clsQuery.GetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, clsType.User.IsNurse.Equals("OK") ? "EmrNrMain" : "ErmMain", "optMcrAddFlag");
            if (dt == null)
            {
                optMcrAdd.Checked = true;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                optMcrAdd.Checked = true;
                clsEmrPublic.gstrMcrAddFlag = "1";
            }
            else
            {
                string optMcrAddFlag = Convert.ToString(VB.Val((dt.Rows[0]["OPTVALUE"].ToString() + "").Trim()));
                dt.Dispose();
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

        #region 메뉴 버튼
        private void MnuModify01_Click(object sender, EventArgs e)
        {

        }
        private void MnuER_Click(object sender, EventArgs e)
        {
            if(fERPatient != null)
            {
                fERPatient.Dispose();
                fERPatient = null;
            }

            fERPatient = new frmERPatient();
            fERPatient.rSendPatInfo += frmERPatient_rSendPatInfo;
            fERPatient.rClosed += FERPatient_rClosed;
            fERPatient.StartPosition = FormStartPosition.CenterParent;
            fERPatient.Show();
        }


        private void MnuToiView_Click(object sender, EventArgs e)
        {
            if (frmMidOutSearch != null)
            {
                frmMidOutSearch.Dispose();
                frmMidOutSearch = null;
            }

            frmMidOutSearch = new frmOutPatientSearch();
            frmMidOutSearch.rSendPatInfo += FrmChartComplete_rSendPatInfo;
            //frmMidOutSearch.rClosed += FrmMidOutSearch_rClosed;
            frmMidOutSearch.StartPosition = FormStartPosition.CenterParent;
            frmMidOutSearch.Show();
        }

   

        private void MnuJin_Click(object sender, EventArgs e)
        {

        }

        private void MnuMCadmin_Click(object sender, EventArgs e)
        {

        }

        private void MnuMCadmin2_Click(object sender, EventArgs e)
        {
            if(frmMCBohumView != null)
            {
                frmMCBohumView.Dispose();
                frmMCBohumView = null;
            }

            frmMCBohumView = new frmMcrtJobMCBohumView();
            frmMCBohumView.rClosed += FrmMCBohumView_rClosed;
            frmMCBohumView.StartPosition = FormStartPosition.CenterParent;
            frmMCBohumView.Show();
        }

        private void MnuILLS_Click(object sender, EventArgs e)
        {
            using(Form frm = new frmViewills())
            {
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog(this);
            }
        }

        private void MnuConvResult_Click(object sender, EventArgs e)
        {
            using(frmConvEMR frm = new frmConvEMR())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        private void MnuViewBi_Click(object sender, EventArgs e)
        {
        }

        private void MnuTest_Click(object sender, EventArgs e)
        {
            if (fDualSign != null)
            {
                fDualSign.Dispose();
                fDualSign = null;
            }

            fDualSign = new frmDualSign();
            //fDualSign.rClosed += FBSTList_rClosed;
            fDualSign.StartPosition = FormStartPosition.CenterParent;
            fDualSign.Show();
        }

        private void MnuHHUM_Click(object sender, EventArgs e)
        {
            if (fBSTList != null)
            {
                fBSTList.Dispose();
                fBSTList = null;
            }

            fBSTList = new frmBSTList();
            //fBSTList.rClosed += fDualSign_rClosed;
            fBSTList.StartPosition = FormStartPosition.CenterParent;
            fBSTList.Show();
        }

        private void MnuSetColor_Click(object sender, EventArgs e)
        {
            using (frmChartColorRank frmChartColorRank = new frmChartColorRank())
            {
                frmChartColorRank.StartPosition = FormStartPosition.CenterParent;
                frmChartColorRank.ShowDialog(this);
            }
        }

        private void MnuCopy_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuCertA_M_Click(object sender, EventArgs e)
        {
            if (frmCompleteAfterModfiyX != null)
            {
                frmCompleteAfterModfiyX.BringToFront();
                frmCompleteAfterModfiyX.Show();
                return;
            }

            Screen screen = Screen.FromControl(this);
            frmCompleteAfterModfiyX = new FrmCompleteAfterModfiy();
            frmCompleteAfterModfiyX.StartPosition = FormStartPosition.Manual;
            frmCompleteAfterModfiyX.Location = new Point()
            {
                X = Math.Max(screen.WorkingArea.X, screen.WorkingArea.X + (screen.WorkingArea.Width - this.Width) / 2),
                Y = Math.Max(screen.WorkingArea.Y, screen.WorkingArea.Y + (screen.WorkingArea.Height - this.Height) / 2)
            };
            frmCompleteAfterModfiyX.rEventCompleUserSend += frmCompleteAfterModfiyX_rEventCompleUserSend;
            frmCompleteAfterModfiyX.rEventClosed += frmCompleteAfterModfiyX_rEventClosed;
            frmCompleteAfterModfiyX.Show(this);
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuComplete_Click(object sender, EventArgs e)
        {
            if(fChartComplete != null)
            {
                fChartComplete.Dispose();
                fChartComplete = null;
            }

            fChartComplete = new frmChartComplete();
            fChartComplete.rSendPatInfo += FrmChartComplete_rSendPatInfo;
            fChartComplete.rClosed += FChartComplete_rClosed;
            fChartComplete.StartPosition = FormStartPosition.CenterParent;
            fChartComplete.Show();
        }

       

        private void MnucodeSearch_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 퇴사
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuToiSabun_Cert_Click(object sender, EventArgs e)
        {
            if(fToiSabun_Cert != null)
            {
                fToiSabun_Cert.Dispose();
                fToiSabun_Cert = null;
            }

            fToiSabun_Cert = new frmToiSabun_Cert();
            fToiSabun_Cert.StartPosition = FormStartPosition.CenterParent;
            fToiSabun_Cert.rClosed += FToiSabun_Cert_rClosed; ;
            fToiSabun_Cert.Show();
        }



        private void MnuCopyList_Click(object sender, EventArgs e)
        {
            if(fEMRCopyList != null)
            {
                fEMRCopyList.Dispose();
                fEMRCopyList = null;
            }

            fEMRCopyList = new frmEMRCopyList(txtPtNo.Text.Trim());
            fEMRCopyList.StartPosition = FormStartPosition.CenterParent;
            fEMRCopyList.FormClosed += frmEMRCopyList_FormClosed;
            fEMRCopyList.Show();
        }

        private void MnuSetupRM_Click(object sender, EventArgs e)
        {

        }

        private void mnuDTL_Click(object sender, EventArgs e)
        {
            if (frmDTL != null)
            {
                frmDTL.Dispose();
                frmDTL = null;
            }

            frmDTL = new frmEmrJobDTL();
            frmDTL.FormClosed += FrmDTL_FormClosed;
            frmDTL.StartPosition = FormStartPosition.CenterScreen;
            frmDTL.Show();
        }
 
        /// <summary>
        /// NP,FM챠트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuNPCHART_Click(object sender, EventArgs e)
        {
            if (fNPChartBatch != null)
            {
                fNPChartBatch.Dispose();
                fNPChartBatch = null;
            }

            fNPChartBatch = new frmNPChartBatch((sender as ToolStripMenuItem).Text);
            fNPChartBatch.rClosed += FNPChartBatch_rClosed;
            fNPChartBatch.StartPosition = FormStartPosition.CenterParent;
            fNPChartBatch.Show(this);
        }

        #endregion

        /// <summary>
        /// 퇴원환자 리스트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuOutPatientList_Click(object sender, EventArgs e)
        {
            using(frmOutPatientList frmOutPatient = new frmOutPatientList())
            {
                frmOutPatient.rSendPatInfo += FrmOutPatient_rSendPatInfo;
                frmOutPatient.StartPosition = FormStartPosition.CenterParent;
                frmOutPatient.ShowDialog(this);
            }
        }
  
        /// <summary>
        /// 챠트로그
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuChartLog_Click(object sender, EventArgs e)
        {
            if (clsType.User.Sabun.Equals("16109") == false)
                return;

            if(fChartLog != null)
            {
                fChartLog.Dispose();
                fChartLog = null;
            }

            fChartLog = new frmChartLog();
            fChartLog.StartPosition = FormStartPosition.CenterParent;
            fChartLog.rClosed += FChartLog_rClosed;
            fChartLog.Show(this);
        }



        /// <summary>
        /// 바이탈
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void MnuPatientVital_Click(object sender, EventArgs e)
        {
            if (fEmrPatientState != null)
            {
                fEmrPatientState.Dispose();
                fEmrPatientState = null;
            }

            fEmrPatientState = new frmPatientState_New();
            fEmrPatientState.FormClosed += FEmrPatientState_FormClosed;
            fEmrPatientState.StartPosition = FormStartPosition.CenterParent;
            fEmrPatientState.Show(this);
        }



        private void MnucodeSearch_Click_1(object sender, EventArgs e)
        {
            if(fCodeSearch != null)
            {
                fCodeSearch.Dispose();
                fCodeSearch = null;
            }

            fCodeSearch = new frmCodeSearch();
            fCodeSearch.rClosed += fCodeSearch_rClosed;
            fCodeSearch.StartPosition = FormStartPosition.CenterParent;
            fCodeSearch.Show(this);
        }

        private void TxtPtNo_Click(object sender, EventArgs e)
        {
            if (txtPtNo.TextLength != 8)
                return;

            txtPtNo.SelectAll();
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

        private void mnuDateSET_Click(object sender, EventArgs e)
        {
            if (clsEmrPublic.gUserGrade.Equals("SIMSA") == false)
            {
                ComFunc.MsgBoxEx(this, "보험심사과 전용 메뉴입니다.");
                return;
            }

            using (frmDateSET frm = new frmDateSET())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        private void mnuView01_Click(object sender, EventArgs e)
        {
            if (mnuView01.Text.Equals("챠트내역없음-모두보기"))
            {
                clsEmrPublic.GstrView01 = "1";
                mnuView01.Text = "챠트내역없음-숨기기";
            }
            else if(mnuView01.Text.Equals("챠트내역없음-숨기기"))
            {
                clsEmrPublic.GstrView01 = "0";
                mnuView01.Text = "챠트내역없음-모두보기";
            }
        }

        private void mnuDateSET1_Click(object sender, EventArgs e)
        {
            if (clsEmrPublic.gUserGrade.Equals("SIMSA") == false)
            {
                ComFunc.MsgBoxEx(this, "사용권한이 없습니다.(보험심사과전용)");
                clsEmrPublic.gDateSET = false;
                return;
            }

            clsEmrPublic.gDateSET = !clsEmrPublic.gDateSET;

            int RowAffected = 0;


            clsDB.setBeginTran(clsDB.DbCon);
            #region 삭제
            try
            {
                string SQL = " DELETE ADMIN.EMR_OPTION_TOTALDATE ";
                SQL += ComNum.VBLF + " WHERE USEID = " + clsType.User.IdNumber;

                string sqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                SQL = " INSERT INTO ADMIN.EMR_OPTION_TOTALDATE(USEID, USED) VALUES (";
                SQL += ComNum.VBLF + clsType.User.IdNumber + ",'" + (clsEmrPublic.gDateSET ? "1" : "0") + "')";

                sqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            #endregion

            mnuDateSET1.Text = clsEmrPublic.gDateSET ? "전체기간 - 적용" : "전체기간 - 미적용";
        }

        private void btnDetail2_Click(object sender, EventArgs e)
        {
   

            //using (frmIO_D frm = new frmIO_D(txtPtNo.Text.Trim()))
            //{
            //    frm.StartPosition = FormStartPosition.CenterParent;
            //    frm.ShowDialog(this);
            //}
        }

        private void btnDetail1_Click(object sender, EventArgs e)
        {
            if (frmVital != null)
            {
                frmVital.Dispose();
                frmVital = null;
            }

            frmVital = new FrmVital_D(txtPtNo.Text.Trim());
            frmVital.StartPosition = FormStartPosition.CenterParent;
            frmVital.FormClosed += FrmVital_FormClosed;
            frmVital.Show();
        }

        private void btnDetail2_Click_1(object sender, EventArgs e)
        {
            using (Form frm = new frmIO_D(txtPtNo.Text.Trim()))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        private void btnDetail3_Click(object sender, EventArgs e)
        {
            if (fBST != null)
            {
                fBST.Dispose();
                fBST = null;
            }

            //fBST = new frmBST_D(txtPtNo.Text);
            //fBST.StartPosition = FormStartPosition.CenterScreen;
            //fBST.FormClosed += FBST_FormClosed;
            //fBST.Show();
        }
    }
}
