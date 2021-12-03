using FarPoint.Win;
using FarPoint.Win.Spread.CellType;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using ComLibB.Dto;
using ComLibB.Service;
using System.Collections.Generic;

namespace ComLibB
{
    public partial class frmViewResult : Form, MainFormMessage
    {


        frmHcResultView FrmHcResultView = null;
        frmHaResultView FrmHaResultView = null;
        ComHpcService comHpcService = null;

        Control mSetButton = null;
        clsSpread methodSpd = new clsSpread();
        ComFunc CF = new ComFunc();
        const int RESULT_ROW_NEW = 29;
        int FnPrintMesu = 0;
         
        string[] arystrUseWord = new string[11];//'상용단어 FstrUseWord

        string mstrDrCode1 = "";//'longin drcode FstrDrCode1
        string mstrDrCode = "";// FstrDrCode

        string mstrXRayDept = ""; // 방사선 특정과만 조회시 사용함 FstrXRayDept
        string mstrXRayDate = ""; // 방사선 특정일자만 조회시 사용함 FstrXRayDate

        string mstrRowIdCVR = ""; //FstrROWID_CVR
        string mstrAnatno = ""; // FstrAnatno'2015-07-02

        string mstrGubun = "";  //'etc_jupmst 에 gubun 구분

        string mstrROWID = "";  //'xray_result_new 의 rowid //  fstrROWID

        int mintDrWrtno = 0;//'처방의 판독 drwrtno  //FnDrWrtno

        int mintWrtno = 0;  //'xray_result_new 의 wrtno //FnWrtno

        string mstrEXEName = "";
        int FnDrWrtno = 0;


        int nIndex = 0;

        string mstrPano = "";
        string mstrSName = "";
        string mstrJumin1 = "";
        string mstrJumin2 = "";
        string mstrSex = "";
        int mintAge = 0;

        string mstrSpecNo = "";
        int mintExamRow = 0;
        
        #region //폼을 모달리스로 띄울경우 처리함
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        private Form mModalCallForm = null;
        private int mModalMonitor = 1;

        //모니터 사이즈, 폼 위치
        private int mintTop = 0;
        private int mintLeft = 0;
        private int mintMonitor = 0;
        private int[] mintWidth = null;
        private int[] mintHeight = null;
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
        /// <summary>
        /// 모니터
        /// </summary>
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
        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }
        /// <summary>
        /// 2번 모니터 띄우기
        /// </summary>
        private void viewFormMonitor2()
        {
            Screen[] screens = Screen.AllScreens;
            Screen secondary_screen = null;

            if (screens.Length == 1)    //모니터 하나
            {
                this.Show();
                //this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                foreach (Screen screen in screens)
                {
                    if (screen.Primary == false)
                    {
                        secondary_screen = screen;
                        if (clsType.User.IdNumber == "31544")
                        {
                            this.Top = 10;
                            this.Left = secondary_screen.Bounds.X + (secondary_screen.Bounds.Width - this.Width) - 10;
                        }
                        else
                        {
                            this.Bounds = secondary_screen.Bounds;
                        }
                        this.Show();
                        //this.WindowState = FormWindowState.Maximized;
                        break;
                    }
                }
            }
        }
        #endregion

        public frmViewResult()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
        }
        public frmViewResult(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
        }
        public frmViewResult(string strPtNo)
        {
            InitializeComponent(); 
            mstrPano = strPtNo;
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
        } 


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strEXEName">"HAMAIN","VCONSULT","MTSOORDER","" 중 하나</param>
        /// <param name="intDrWrtno"></param>
        public frmViewResult(string strPtNo, string strEXEName)
        {
            InitializeComponent();
            mstrPano = strPtNo;
            mstrEXEName = strEXEName;
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
        }


        #region //폼을 모달리스로 띄울경우 처리함
        public frmViewResult(string strPtNo, Form pModalCallForm, int pModalMonitor)
        {
            InitializeComponent();
            mstrPano = strPtNo;

            mModalCallForm = pModalCallForm;
            mModalMonitor = pModalMonitor;
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
        }

        public frmViewResult(string strPtNo, Form pModalCallForm, int pModalMonitor, string strEXEName)
        {
            InitializeComponent();
            mstrPano = strPtNo;

            mModalCallForm = pModalCallForm;
            mModalMonitor = pModalMonitor;

            mstrEXEName = strEXEName;
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
        }
        #endregion 

        private void frmViewResult_Load(object sender, EventArgs e)
        {
            string strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            int intSetDtpsDate = 0;

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            chkOBST.Checked = false;
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            mSetButton = btnXray;

            Control[] con = ComFunc.GetAllControls(pan1);

            foreach (Control conVal in con)
            {
                if (conVal is Button)
                {
                    ((Button)conVal).Click += FrmViewResult_Click;
                }
            }

            panXray.Dock = DockStyle.Fill;
            panConsult.Dock = DockStyle.Fill;
            panDiscern.Dock = DockStyle.Fill;
            panTDM.Dock = DockStyle.Fill;
            panExam3.Dock = DockStyle.Fill;
            panCVR.Dock = DockStyle.Fill;
            panPFT.Dock = DockStyle.Fill;
            panDem.Dock = DockStyle.Fill;
            panEMG.Dock = DockStyle.Fill;
            panECG.Dock = DockStyle.Fill;
            panEEG.Dock = DockStyle.Fill;
            panNST.Dock = DockStyle.Fill;
            panPConsult.Dock = DockStyle.Fill;
            panSixWalk.Dock = DockStyle.Fill;
            panEtcJupmst.Dock = DockStyle.Fill;
            panExam.Dock = DockStyle.Fill;
            panEtc.Dock = DockStyle.Fill;

            #region Form_Load
            panExam.Visible = false;
            ssExam02_Sheet1.Columns.Get(5).Visible = false; // 검사코드

            panDiscern.Visible = false;
            panXray.Visible = false;
            panEtc.Visible = false;

            cboPacs.Items.Clear();
            cboPacs.Items.Add("당일영상전체");
            cboPacs.Items.Add("선택영상전체");
            cboPacs.Items.Add("선택영상만");
            cboPacs.Items.Add("전체영상");

            cboPacs.SelectedIndex = 0;

            ss1_Sheet1.Columns.Get(3, 6).Visible = false;
            ss1_Sheet1.Columns.Get(8, 9).Visible = false;

            ss2_Sheet1.Columns.Get(9).Visible = false;
            ss2_Sheet1.Columns.Get(11, 20).Visible = false;
            ss2_Sheet1.Columns[25].Visible = false;
            ss2_Sheet1.Columns[26].Visible = false;

            ssEEG_Sheet1.Columns.Get(7, 8).Visible = false;

            ssConsult_Sheet1.Columns.Get(5, 10).Visible = false;


            intSetDtpsDate = SetDtpsDate(clsType.User.Sabun);

            if(VB.IsDate(strDate))
            {
                if (intSetDtpsDate == -1)
                {
                    dtpSdate.Value = Convert.ToDateTime(strDate).AddYears(-3);
                }
                else
                {
                    dtpSdate.Value = Convert.ToDateTime(strDate).AddDays(intSetDtpsDate * -1);
                }

                dtpTdate.Value = Convert.ToDateTime(strDate);

                //TODO : 의사별 조회기간 다르게 세팅 할꺼임 - 영록
                dtpEx3SDate.Value = Convert.ToDateTime(strDate).AddYears(-1);
                dtpEx3TDate.Value = Convert.ToDateTime(strDate);

            }


            FileInfo di = new FileInfo("C:\\cmc\\ocsviewer\\ocsviewer.exe");

            if (di.Exists == false)
                btnEMR.Enabled = false;

            if (EtcViewCert(VB.Pstr(clsPublic.GstrHelpCode, "{}", 1), txtPtNo) == false)
            {
                ComFunc.MsgBox("검사결과는 재원자 및 당일 진료자만 조회가 가능합니다.", "확인");

                txtPtNo.Text = "";
                clsPublic.GstrHelpCode = "";
                return;
            }

            if (clsPublic.GstrHelpName == "DRUG")
            {
                txtPtNo.Text = mstrPano;

                btnDiscern.PerformClick();
                return;
            }

            mstrXRayDept = "";
            mstrXRayDate = "";

            if (clsPublic.GstrHelpCode != "" && mstrPano == "")
            {
                txtPtNo.Text = VB.Pstr(clsPublic.GstrHelpCode, "{}", 1);
                mstrXRayDept = VB.Pstr(clsPublic.GstrHelpCode, "{}", 2);
                mstrXRayDate = VB.Pstr(clsPublic.GstrHelpCode, "{}", 3);
                clsPublic.GstrPANO = txtPtNo.Text;
                SetPaatient(txtPtNo.Text);

                if (mstrXRayDate != "" && VB.Len(mstrXRayDate) > 8)
                {
                    dtpSdate.Value = Convert.ToDateTime(mstrXRayDate);
                    dtpTdate.Value = Convert.ToDateTime(mstrXRayDate);
                    dtpSdate.Enabled = false;
                    dtpTdate.Enabled = false;
                }
                else
                {
                    if (intSetDtpsDate == -1)
                    {
                        dtpSdate.Value = Convert.ToDateTime(strDate).AddYears(-3);
                    }
                    else
                    {
                        dtpSdate.Value = Convert.ToDateTime(strDate).AddDays(intSetDtpsDate * -1);
                    }

                    dtpTdate.Value = Convert.ToDateTime(strDate);
                }
                lblSName.Text = mstrSName;
                lblSexAge.Text = mstrSex + "/" + Convert.ToString(mintAge);
                lblBi.Text = "";
                clsPublic.GstrHelpCode = "";
                btnXray.PerformClick();
            }
            else
            {
                txtPtNo.Text = "";
                lblSName.Text = "";
                lblSexAge.Text = "";
                lblBi.Text = "";
            }

            lblBlood.Text = "";
            txtExName.Text = "";

            #endregion

            #region //폼을 모달리스로 띄울경우 처리함
            GetMonitorInfo();
            if (mModalMonitor == 2)
            {
                viewFormMonitor2();
            }

            #endregion

            #region Form_Activate

            Form_Activate();

            #endregion

            if (txtPtNo.Text.Trim() == "" && mstrPano != "")
            {
                txtPtNo.Text = mstrPano;
                txtPtNoKeyDown();
                btnXray.PerformClick();
            }
            
            if (mstrEXEName == "MID_CONSULT")   //의료정보팀 협의진단 바로 보게 해달라~ ^ㅁ^
            {
                btnConsult.PerformClick();
            }

        }

        private void FrmViewResult_Click(object sender, EventArgs e)
        {
            mSetButton = ((Button)sender);
        }

        private int SetDtpsDate(string sabun)
        {
            int rtnVal = -1;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수


            try
            {
                SQL = "";
                SQL = "SELECT ITEM51";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_ENVSETTING";
                SQL = SQL + ComNum.VBLF + "WHERE USERID = '" + sabun + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = Convert.ToInt32(VB.Val(dt.Rows[0]["ITEM51"].ToString().Trim()));
                }

                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

        }

        private void Form_Activate()
        {
            //'오류: 등록번호에 다른 번호를 입력 후 달력띄운면 다시 공용변수에 등록된 환자로 변경됨.

            if (EtcViewCert(clsPublic.GstrPANO, txtPtNo) == false)
            {
                txtPtNo.Text = "";
                clsPublic.GstrHelpCode = "";
                return;
            }

            txtExName.Text = "";
            panEtcCVR.Visible = false;  //'2015-07-01

            btnFMInbody.Visible = false;
            btnFMStress.Visible = false;

            if (clsType.User.IdNumber == "34626" || clsType.User.IdNumber == "23515")
            {
                btnFMInbody.Visible = true;
                btnFMStress.Visible = true;
            }

            //'종검에서 스트레스 결과보기 2014-04-11

            if (mstrEXEName.ToUpper() == "HAMAIN" || clsType.User.IdNumber == "32158" || clsType.User.IdNumber == "34902" || clsType.User.IdNumber == "53935" || clsType.User.BuseCode == "044510")
            {
                btnFMStress.Visible = true;
            }

            if (clsPublic.GstrPANO != "")
            {
                txtPtNo.Text = clsPublic.GstrPANO;
                txtPtNo.Focus();


                txtPtNoKeyDown();


                if (clsType.User.IdNumber == "35104" || mstrEXEName.ToUpper() == "VCONSULT")
                {
                    clsPublic.GstrPANO = "";
                }
            }

            //'의사코드 READ
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT SABUN FROM ADMIN.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE SABUN='" + clsType.User.IdNumber + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    mstrDrCode1 = dt.Rows[0]["SABUN"].ToString().Trim();
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            btnCVR.Enabled = true;
        }

        private void txtPtNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPtNoKeyDown();

                ((Button)mSetButton).PerformClick();
            }
        }

        private void txtPtNoKeyDown()
        {

            if (txtPtNo.Text.Trim() == "")
            {
                return;
            }

            txtPtNo.Text = ComFunc.LPAD(txtPtNo.Text.Trim(), 8, "0");

            if (EtcViewCert(txtPtNo.Text, txtPtNo) == false)
            {
                ComFunc.MsgBox("검사결과는 재원자 및 당일 진료자만 조회가 가능합니다.", "확인");
                txtPtNo.Text = "";
                clsPublic.GstrHelpCode = "";
                return;
            }

            SetPaatient(txtPtNo.Text);


            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "SELECT ABO FROM ADMIN.EXAM_BLOOD_MASTER ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + txtPtNo.Text.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                lblBlood.Text = "";

                if (dt.Rows.Count > 0)
                {
                    lblBlood.Text = dt.Rows[0]["ABO"].ToString().Trim();

                    if (VB.Right(lblBlood.Text, 1) == "-")
                    {
                        lblBlood.ForeColor = Color.FromArgb(255, 255, 0);
                    }
                    else
                    {
                        lblBlood.ForeColor = Color.FromArgb(255, 0, 0);
                    }
                }

                dt.Dispose();
                dt = null;

                ssECG_Sheet1.RowCount = 0;
                ssEEG_Sheet1.RowCount = 0;
                ssDiscern_Sheet1.RowCount = 0;
                ssConsult_Sheet1.RowCount = 0;
                ss1_Sheet1.RowCount = 0;
                ssExam01_Sheet1.RowCount = 0;
                ssPFT_Sheet1.RowCount = 0;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private bool EtcViewCert(string strPtNo, TextBox argTextBox)
        {
            //2016-08-08 계장 김현욱
            //검사결과 조회 제한
            //병원장님 지시사항입니다.
            //1) 재원자 및 당일 외래 진료자만 조회 가능
            //2) 병록번호 수정 불가하게 막음, 파라메터로 전달 받은 내용은 조회 가능
            //3) 파라메터로 전달 받아도 조회 조건에 맞지 않으면 조회 안됨
            //4) 의사 사번 제외

            DataTable dt = null;
            bool bolVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            argTextBox.Enabled = true;


            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return bolVal; //권한 확인

                SQL = "";
                SQL = " SELECT NAME";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'OCS_기타검사결과_조회제한'";
                SQL = SQL + ComNum.VBLF + " AND CODE = '시행'";
                SQL = SQL + ComNum.VBLF + " AND NAME = 'N'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    bolVal = true;
                    dt.Dispose();
                    dt = null;
                    return bolVal;
                }

                dt.Dispose();
                dt = null;

                //'1) 의사일 경우 OK
                argTextBox.Enabled = false;

                SQL = "";
                SQL = " SELECT DRCODE";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.OCS_DOCTOR";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + ComFunc.LPAD(clsType.User.IdNumber.Trim(), 5, "0") + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    bolVal = true;
                    argTextBox.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    return bolVal;
                }

                dt.Dispose();
                dt = null;

                //'2) 의료정보팀에서 세팅한 경우 OK
                SQL = "";
                SQL = " SELECT USERID ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMR_USERT ";
                SQL = SQL + ComNum.VBLF + " WHERE USERID = '" + clsType.User.IdNumber + "' ";
                SQL = SQL + ComNum.VBLF + "   AND H_VIEW = '*'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    bolVal = true;
                    argTextBox.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    return bolVal;
                }

                dt.Dispose();
                dt = null;

                //'3) 당일 외래 접수 내역이 있을 경우 OK
                SQL = "";
                SQL = " SELECT PANO ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.OPD_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    bolVal = true;
                    dt.Dispose();
                    dt = null;
                    return bolVal;
                }

                dt.Dispose();
                dt = null;

                //'4) 현재 재원자이거나 당일 퇴원자일 경우 OK
                SQL = "";
                SQL = " SELECT PANO ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND (JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     OR OUTDATE = TO_DATE('" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "       )";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    bolVal = true;
                    dt.Dispose();
                    dt = null;
                    return bolVal;
                }

                dt.Dispose();
                dt = null;

                bolVal = false;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return bolVal;

        }

        private void GetExam3(string strGb)
        {
            int i = 0;
            int intRow = 0;
            int intCol = 0;
            string SQL = "";
            string strExam = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            ssExam3_Sheet1.RowCount = 0;
            //ssExam3_Sheet1.ColumnCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = "SELECT  B.SUBCODE, TO_CHAR(B.RESULTDATE,'MM-DD') RESULTDATE ,";
                SQL = SQL + ComNum.VBLF + "B.RESULT, B.REFER,  B.UNIT,  C.EXAMYNAME "; //' ,  B.*  "
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.EXAM_SPECMST A, ADMIN.EXAM_RESULTC B, ADMIN.EXAM_MASTER C ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PANO ='" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "     AND  A.SPECNO = B.SPECNO(+)  ";

                if (strGb == "A")
                {
                    SQL = SQL + ComNum.VBLF + "     AND  A.BDATE >=TRUNC(SYSDATE -365) ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND  A.BDATE >=TO_DATE('" + dtpEx3SDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND  A.BDATE <=TO_DATE('" + dtpEx3TDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                }

                SQL = SQL + ComNum.VBLF + "     AND B.SUBCODE IN ('HR01A','HR01C','HR01D','HR01I','HR03','CR31','CR31A','CR32','CR32C','CR34','CR34A',";
                SQL = SQL + ComNum.VBLF + "                    'CR35','CR35A','CR41','CR41A','CR42','CR42A','CR51','CR51A','CR52','CR52A','CR65','SE041','SE04A','SE04B')";
                SQL = SQL + ComNum.VBLF + "     AND B.STATUS ='V' ";
                SQL = SQL + ComNum.VBLF + "     AND B.SUBCODE = C.MASTERCODE(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY  DECODE(B.SUBCODE ,'HR01A','01', 'HR01C','02','HR01D','03','HR01I','04','HR03','05','CR31','06','CR31A','06','CR32','07','CR32C','07',";
                SQL = SQL + ComNum.VBLF + "                              'CR34','08','CR34A','08','CR35','09','CR35A','09','CR41','10','CR41A','10',";
                SQL = SQL + ComNum.VBLF + "                              'CR42','11','CR42A','11','CR51','12','CR51A','12','CR52','13','CR65','14','SE041','15','SE004A','15','SE04B','15' ) , B.RESULTDATE DESC  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    intRow = -1;
                    strExam = "";

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (strExam != dt.Rows[i]["SUBCODE"].ToString().Trim())
                        {
                            intCol = 0;

                            intRow = intRow + 2;

                            if (ssExam3_Sheet1.RowCount < intRow + 1)
                            {
                                ssExam3_Sheet1.RowCount = intRow + 2;
                            }

                            //'검사명 표시

                            ssExam3_Sheet1.Cells[intRow, 0].Text = dt.Rows[i]["EXAMYNAME"].ToString().Trim();
                            strExam = dt.Rows[i]["SUBCODE"].ToString().Trim();

                            //'Determines the section of the cell border displayed around the entire spreadsheet
                            //SSExam3.SetCellBorder - 1, nRow, -1, nRow, SS_BORDER_TYPE_LEFT, &HFFFFFFFF, SS_BORDER_STYLE_DEFAULT
                            //SSExam3.SetCellBorder - 1, nRow, -1, nRow, SS_BORDER_TYPE_TOP, &HFFFFFFFF, SS_BORDER_STYLE_DEFAULT
                            //SSExam3.SetCellBorder - 1, nRow, -1, nRow, SS_BORDER_TYPE_RIGHT, &HFFFFFFFF, SS_BORDER_STYLE_DEFAULT
                            //SSExam3.SetCellBorder - 1, nRow, -1, nRow, SS_BORDER_TYPE_BOTTOM, &HFFFFFFFF, SS_BORDER_STYLE_SOLID

                            ssExam3_Sheet1.Rows.Get(intRow).Border = new LineBorder(Color.Black, 1, false, false, true, true);

                        }

                        intCol = intCol + 1;

                        if (ssExam3_Sheet1.ColumnCount < intCol + 1)
                        {
                            ssExam3_Sheet1.ColumnCount = intCol + 1;
                            ssExam3_Sheet1.ColumnHeader.Columns[intCol].Label = "결과";
                            ssExam3_Sheet1.ColumnHeader.Columns[intCol].Width = 119;

                        }

                        ssExam3_Sheet1.Cells[intRow - 1, intCol].Text = dt.Rows[i]["RESULTDATE"].ToString().Trim();
                        ssExam3_Sheet1.Cells[intRow - 1, intCol].Font = new Font("굴림", 8, FontStyle.Regular);

                        ssExam3_Sheet1.Cells[intRow, intCol].Text = dt.Rows[i]["RESULT"].ToString().Trim();
                        ssExam3_Sheet1.Cells[intRow, intCol].Font = new Font("굴림", 11, FontStyle.Bold);
                        ssExam3_Sheet1.Cells[intRow, intCol].ForeColor = Color.FromArgb(0, 0, 255);

                        intCol = intCol + 1;

                        if (ssExam3_Sheet1.ColumnCount < intCol + 1)
                        {
                            ssExam3_Sheet1.ColumnCount = intCol + 1;
                            ssExam3_Sheet1.ColumnHeader.Columns[intCol].Label = "R";
                            ssExam3_Sheet1.ColumnHeader.Columns[intCol].Width = 18;
                        }

                        ssExam3_Sheet1.Cells[intRow, intCol].Text = dt.Rows[i]["REFER"].ToString().Trim();
                    }
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
        }

        private void GetExamSpecmst()
        {
            if (txtPtNo.Text.Trim() == "")
                return;

            int i = 0;
            string strSpecNo = "";
            string strJumin = "";
            string strYear1 = "";
            string strYear2 = "";
            bool bolColor = false;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            panXray.Visible = false;

            ss1.Visible = true;
            txtResult1.Visible = false;
            panExam.Visible = true;

            ssExam02_Sheet1.RowCount = 0;
            ssExam03_Sheet1.RowCount = 0;
            //Call SS_Clear(SS_Exam2)
            //Call SS_Clear(SS_Exam3)
            ssExam02_Sheet1.RowCount = 20;
            ssExam03_Sheet1.RowCount = 20;

            lblExamName.Text = "";

            strJumin = clsAES.Read_Jumin_AES(clsDB.DbCon, txtPtNo.Text.Trim());

            //'검체마스타를 SELECT
            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = "SELECT A.SPECNO,A.DEPTCODE,A.ROOM,A.DRCODE,A.WORKSTS,A.SPECCODE,A.STATUS,";
                SQL = SQL + ComNum.VBLF + " A.IPDOPD,A.BI,TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.BLOODDATE,'YYYY-MM-DD HH24:MI') BLOODDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI') RECEIVEDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI') RESULTDATE,A.PRINT ";
                SQL = SQL + ComNum.VBLF + " ,(SELECT NAME FROM ADMIN.EXAM_SPECODE WHERE GUBUN = '14' AND CODE = A.SPECCODE) AS SPECCODENAME        ";
                SQL = SQL + ComNum.VBLF + " ,(SELECT WM_CONCAT(TRIM(BB.EXAMNAME))        ";
                SQL = SQL + ComNum.VBLF + "     FROM ADMIN.EXAM_RESULTC AA        ";
                SQL = SQL + ComNum.VBLF + "     INNER JOIN ADMIN.EXAM_MASTER BB        ";
                SQL = SQL + ComNum.VBLF + "         ON AA.MASTERCODE = AA.SUBCODE        ";
                SQL = SQL + ComNum.VBLF + "         AND AA.MASTERCODE = BB.MASTERCODE        ";
                SQL = SQL + ComNum.VBLF + "     WHERE AA.SPECNO = A.SPECNO) AS EXAMNAME        ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_SPECMST A, ADMIN.EXAM_RESULTC B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.PANO='" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.WORKSTS NOT IN ('A','T') ";  //'세포학,조직학은 제외
                SQL = SQL + ComNum.VBLF + "  AND NVL(SUBSTR(A.ANATNO,1,2),0) != 'IH'         ";
                if (chkAll.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.STATUS IN ('04','14','05') ";
                    SQL = SQL + ComNum.VBLF + "  AND B.STATUS IN ('V') ";  //'TLA 미검증은 VERIFY 된것 제외함 JJY 2014-06-19 '2014 - 10 - 15 감염관리실의뢰 모든 항목 표시
                }
                SQL = SQL + ComNum.VBLF + " AND A.BDATE >= TO_DATE('" + dtpSdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND A.BDATE <= TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND A.SPECNO = B.SPECNO(+) ";

                //2020-08-14 안정수 추가, 병동 글로코스제외 체크시, 조회안되도록
                if (chkBST.Checked == true && chkOBST.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "  AND NOT (IPDOPD = 'I' AND B.MASTERCODE IN ('CR59','CR59B')) ";  
                }
                else if(chkOBST.Checked == true && chkBST.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "  AND NOT (IPDOPD = 'O' AND B.MASTERCODE IN ('CR59','CR59B')) ";
                }
                else if (chkOBST.Checked == true && chkBST.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND NOT ( B.MASTERCODE IN ('CR59','CR59B')) ";
                }

                if (txtExName.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.SUBCODE IN (";
                    SQL = SQL + ComNum.VBLF + "         SELECT  MASTERCODE  FROM  ADMIN.EXAM_MASTER ";
                    SQL = SQL + ComNum.VBLF + "          WHERE  ( UPPER(EXAMNAME) LIKE '%" + txtExName.Text.Trim().ToUpper() + "%' OR UPPER(EXAMFNAME) LIKE '%" + txtExName.Text.Trim().ToUpper() + "%' OR UPPER(EXAMYNAME) LIKE '%" + txtExName.Text.Trim().ToUpper() + "%'  ) ";
                    SQL = SQL + ComNum.VBLF + "       ) ";
                }
                SQL = SQL + ComNum.VBLF + " GROUP BY A.SPECNO , A.DEPTCODE, A.ROOM, A.DRCODE, A.WORKSTS, A.SPECCODE, A.STATUS, ";
                SQL = SQL + ComNum.VBLF + " A.IPDOPD,A.BI,TO_CHAR(A.BDATE,'YYYY-MM-DD') ,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.BLOODDATE,'YYYY-MM-DD HH24:MI') ,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI') ,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI') , A.PRINT ";

                if (txtExName.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY 12 DESC,A.SPECNO ";
                }
                else
                {
                    //'2015-03-13
                    if (chkResult.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "ORDER BY TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI') DESC,A.SPECNO ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "ORDER BY TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI') DESC,A.SPECNO ";
                    }
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssExam01_Sheet1.RowCount = dt.Rows.Count;
                    methodSpd.setSpdSort(ssExam01, 1, true);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        strSpecNo = dt.Rows[i]["SPECNO"].ToString().Trim();

                        ssExam01_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SPECNO"].ToString().Trim();


                        if (dt.Rows[i]["RESULTDATE"].ToString().Trim() != "")
                        {
                            ssExam01_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RECEIVEDATE"].ToString().Trim() + " (" + Convert.ToDateTime(dt.Rows[i]["RESULTDATE"].ToString().Trim()).ToString("yyyy-MM-dd") + ")";
                        }
                        else
                        {
                            ssExam01_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RECEIVEDATE"].ToString().Trim() + " ()";
                        }

                        switch (dt.Rows[i]["IPDOPD"].ToString().Trim())
                        {
                            case "I":
                                ssExam01_Sheet1.Cells[i, 2].Text = "입원";
                                break;
                            default:
                                switch (dt.Rows[i]["BI"].ToString().Trim())
                                {
                                    case "61":
                                        ssExam01_Sheet1.Cells[i, 2].Text = "종검";
                                        break;
                                    case "62":
                                        ssExam01_Sheet1.Cells[i, 2].Text = "건진";
                                        break;
                                    case "81":
                                        ssExam01_Sheet1.Cells[i, 2].Text = "수탁";
                                        break;
                                    default:
                                        ssExam01_Sheet1.Cells[i, 2].Text = "외래";
                                        break;
                                }
                                break;
                        }

                        ssExam01_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssExam01_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROOM"].ToString().Trim();
                        ssExam01_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                        ssExam01_Sheet1.Cells[i, 6].Text = dt.Rows[i]["WORKSTS"].ToString().Trim();
                        ssExam01_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SPECCODENAME"].ToString().Trim();//'검체구분 조회
                        ssExam01_Sheet1.Cells[i, 8].Text = dt.Rows[i]["EXAMNAME"].ToString();//GetSpecnoExamName(strSpecNo);

                        switch (dt.Rows[i]["STATUS"].ToString().Trim())
                        {
                            case "00":
                                ssExam01_Sheet1.Cells[i, 9].Text = "미접수";

                                break;
                            case "01":
                                ssExam01_Sheet1.Cells[i, 9].Text = "검사중";
                                ssExam01_Sheet1.Cells[i, 9].BackColor = Color.Pink;
                                break;
                            case "02":
                                ssExam01_Sheet1.Cells[i, 9].Text = "부분입력";
                                ssExam01_Sheet1.Cells[i, 9].BackColor = Color.Pink;
                                break;
                            case "03":
                                ssExam01_Sheet1.Cells[i, 9].Text = "모두입력";
                                break;
                            case "04":
                            case "14":
                                ssExam01_Sheet1.Cells[i, 9].Text = "부분완료";
                                ssExam01_Sheet1.Cells[i, 9].BackColor = Color.Pink;
                                break;
                            case "05":
                                if (VB.Val(dt.Rows[i]["PRINT"].ToString().Trim()) == 0)
                                    ssExam01_Sheet1.Cells[i, 9].Text = "검사완료";
                                if (VB.Val(dt.Rows[i]["PRINT"].ToString().Trim()) > 0)
                                    ssExam01_Sheet1.Cells[i, 9].Text = "인쇄완료";
                                break;
                            case "06":
                                ssExam01_Sheet1.Cells[i, 9].Text = "취소";
                                break;
                            default:
                                ssExam01_Sheet1.Cells[i, 9].Text = "ERROR";
                                break;
                        }


                        ssExam01_Sheet1.Cells[i, 13].Text = dt.Rows[i]["SPECCODE"].ToString().Trim();
                        ssExam01_Sheet1.Cells[i, 14].Text = dt.Rows[i]["BDATE"].ToString().Trim();

                        strYear1 = VB.Left(dt.Rows[i]["RESULTDATE"].ToString().Trim(), 4);

                        if (strYear2 != strYear1)
                        {
                            if (bolColor == true)
                            {
                                bolColor = false;
                            }
                            else
                            {
                                bolColor = true;
                            }
                            strYear2 = strYear1;
                        }
                        if (bolColor == true)
                        {
                            ssExam01_Sheet1.Cells[i, 2].BackColor = Color.White;
                        }
                        else
                        {
                            ssExam01_Sheet1.Cells[i, 2].BackColor = Color.FromArgb(255, 255, 192);
                        }

                        if (ssExam01_Sheet1.Cells[i, 9].Text == "일부완료")
                        {
                            ssExam01_Sheet1.Cells[i, 9].BackColor = Color.LightPink;
                        }
                        else if (ssExam01_Sheet1.Cells[i, 9].Text == "검사완료" || ssExam01_Sheet1.Cells[i, 9].Text == "인쇄완료")
                        {
                            ssExam01_Sheet1.Cells[i, 9].BackColor = Color.LightGreen;
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                clsPublic.GstrHelpCode = "";
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// '검체별 검사명칭
        /// </summary>
        /// <param name="strSPECCODE"></param>
        /// <returns></returns>
        private string GetSpecnoExamName(string strSpecno)
        {
            DataTable dt = null;
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = "SELECT B.WSCODE1,B.WSCODE1POS,A.MASTERCODE,B.EXAMNAME,COUNT(A.MASTERCODE) CNT        ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.EXAM_RESULTC A, ADMIN.EXAM_MASTER B ";
                SQL = SQL + ComNum.VBLF + "WHERE A.SPECNO = '" + strSpecno + "'    ";
                SQL = SQL + ComNum.VBLF + "  AND A.MASTERCODE = A.SUBCODE   ";
                SQL = SQL + ComNum.VBLF + "  AND A.MASTERCODE = B.MASTERCODE(+) ";
                SQL = SQL + ComNum.VBLF + "GROUP BY B.WSCODE1,B.WSCODE1POS,A.MASTERCODE,B.EXAMNAME  ";
                SQL = SQL + ComNum.VBLF + "ORDER BY B.WSCODE1,B.WSCODE1POS,A.MASTERCODE,B.EXAMNAME  ";




                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["EXAMNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                clsPublic.GstrHelpCode = txtPtNo.Text.Trim();

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
            return strVal;
        }

        private void ECGFILE_DBToFile(string strROWID, string strPtNo, string strViewerExe)
        {
            bool bolWin7 = true;
            bool bolWin10 = true;

            string strECGFile = "";

            string strFileName = "";
            string strRemotePath = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            if (clsVbfunc.GetFile("C:\\Program Files\\NKC\\ECG Viewer3\\FileViewer.exe") == "")
            {
                bolWin7 = false;
            }
            else
            {
                strECGFile = @"C:\Program Files\NKC\ECG Viewer3\FileViewer.exe";
            }

            if (clsVbfunc.GetFile("C:\\Program Files (x86)\\NKC\\ECG Viewer3\\FileViewer.exe") == "")
            {
                bolWin10 = false;
            }
            else
            {
                strECGFile = @"C:\Program Files (x86)\NKC\ECG Viewer3\FileViewer.exe";
            }

            if (bolWin7 == false && bolWin10 == false)
            {
                ComFunc.MsgBox("ECG(EKG) Viewer가 설치되지 안았습니다.");
                return;
            }

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인


                //'2014-11-18 FTP저장체크
                SQL = " SELECT ROWID,TO_CHAR(RDATE,'YYYYMMDD') AS RDATE, FILEPATH FROM ADMIN.ETC_JUPMST ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GbFTP ='Y' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                strFileName = @"C:\CMC\ECG_" + strPtNo + ".ecg";

                if (dt.Rows.Count > 0)
                {

                    strRemotePath = "/data/ocs_etc/" + dt.Rows[0]["RDATE"].ToString().Trim() + "/";


                    //'2014-11-18 서버에서 PC로 파일을 다운로드함
                    using (Ftpedt FtpedtX = new Ftpedt())
                    {
                        FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFileName, dt.Rows[0]["FILEPATH"].ToString().Trim(), strRemotePath);
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "SELECT IMAGE ";
                    SQL = SQL + ComNum.VBLF + "FROM ADMIN.ETC_JUPMST ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        byte[] b = (byte[])(dt.Rows[0]["IMAGE"]);

                        using (MemoryStream stream = new MemoryStream(b))
                        {
                            using (Bitmap image = new Bitmap(stream))
                            {
                                image.Save(strFileName);
                            }
                        }
                            
                    }

                    dt.Dispose();
                    dt = null;
                }

                //'ecg viwer 실행
                if (clsVbfunc.GetFile(strFileName) != "")
                {
                    if (strViewerExe == "1")
                    {
                        //프로그램이 사용중인지 체크
                        bool isRunning = false;

                        System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("FileViewer");
                        if (ProcessEx.Length > 0)
                        {
                            System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("FileViewer");
                            System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                            foreach (System.Diagnostics.Process Proc in Pro1)
                            {
                                if (Proc.Id != CurPro.Id)
                                {
                                    isRunning = true;
                                    Proc.Kill();
                                    Delay(500);
                                    isRunning = false;
                                    break;
                                }
                            }
                        }

                        if (isRunning == false)
                        {
                            Delay(500);
                            System.Diagnostics.Process program = System.Diagnostics.Process.Start(strECGFile, strFileName + " ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
        }

        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);
            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }

        private void ETC_FILE_DBToFile(string strROWID, string strPtNo, string strViewerExe)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            if (clsVbfunc.GetFile("C:\\WINDOWS\\SYSTEM32\\SHIMGVW.DLL") == "")
            {
                ComFunc.MsgBox("WINDOWS IMAGE Viewer가 설치되지 안았습니다.");
                return;
            }

            string strFileName = "";
            string strRemotePath = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strAudioFile = "";

            DataTable dt = null;

            try
            {

                //'2014-11-18 FTP저장체크

                SQL = " SELECT ROWID,TO_CHAR(RDATE,'YYYYMMDD') AS RDATE, TO_CHAR(BDATE,'YYYYMMDD') AS BDATE,  GUBUN,FILEPATH       ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.ETC_JUPMST ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GBFTP ='Y' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                strFileName = @"C:\CMC\ETC.jpg";

                if (dt.Rows.Count > 0)
                {

                    if (dt.Rows[0]["GUBUN"].ToString().Trim() == "6" || dt.Rows[0]["GUBUN"].ToString().Trim() == "23")
                    {
                        strRemotePath = "/data/ocs_etc/" + dt.Rows[0]["BDATE"].ToString().Trim() + "/";
                    }
                    else
                    {
                        strRemotePath = "/data/ocs_etc/" + dt.Rows[0]["RDATE"].ToString().Trim() + "/";
                    }

                    //    '2014-11-18 서버에서 PC로 파일을 다운로드함
                    using (Ftpedt FtpedtX = new Ftpedt())
                    {
                        FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFileName, dt.Rows[0]["FILEPATH"].ToString().Trim(), strRemotePath);
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "SELECT IMAGE FROM ADMIN.ETC_JUPMST  WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        byte[] b = (byte[])(dt.Rows[0]["IMAGE"]);

                        using (MemoryStream stream = new MemoryStream(b))
                        {
                            using (Bitmap image = new Bitmap(stream))
                            {
                                image.Save(strFileName);
                            }
                        }

                    }

                    dt.Dispose();
                    dt = null;
                }

                //'ecg viwer 실행
                if (strViewerExe == "1")
                {
                    if (clsVbfunc.GetFile("%ProgramFiles%\\Windows Photo Gallery\\PhotoViewer.dll") != "")
                    {
                        strAudioFile = "rundll32.exe %ProgramFiles%\\Windows Photo Gallery\\PhotoViewer.dll, ImageView_Fullscreen " + strFileName;
                    }
                    else
                    {
                        strAudioFile = "rundll32.exe shimgvw.dll, ImageView_Fullscreen " + strFileName;

                    }
                    VB.Shell(strAudioFile, "MaximizedFocus");
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

        }

        private void EMG_FILE_DBToFile(string argWRTNO, string ArgPano, string strViewerExe)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            if (clsVbfunc.GetFile("C:\\WINDOWS\\SYSTEM32\\SHIMGVW.DLL") == "")
            {
                ComFunc.MsgBox("WINDOWS IMAGE Viewer가 설치되지 안았습니다.");
                return;
            }

            byte[] b = null;
            MemoryStream stream = null;
            Bitmap image = null;

            int i = 0;
            string strFileName = "";
            string strRemotePath = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strEMGFile = "";

            DataTable dt = null;

            try
            {
                DirectoryInfo dir = new DirectoryInfo(@"c:\cmc\");
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo F in files)
                {
                    if (F.Name.IndexOf("EMG") != -1)
                    {
                        if (F.Extension == ".jpg")
                        {
                            F.Delete();
                        }
                    }
                }

                strFileName = @"c:\CMC\EMG"; // '.jpg";

                SQL = " SELECT  IMAGE,TO_CHAR(SDATE,'YYYYMMDD') SDATE,FILENAME FROM ADMIN.ETC_RESULT  ";
                SQL = SQL + ComNum.VBLF + " WHERE  WRTNO = '" + argWRTNO + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GBFTP ='Y' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    SQL = " SELECT  IMAGE FROM ADMIN.ETC_RESULT  ";
                    SQL = SQL + ComNum.VBLF + " WHERE  WRTNO = '" + argWRTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND IMAGE IS NOT NULL ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        b = (byte[])(dt.Rows[i]["IMAGE"]);

                        stream = new MemoryStream(b);
                        image = new Bitmap(stream);
                        image.Save(strFileName + Convert.ToString(i + 1) + ".jpg");
                    }
                }
                else
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strRemotePath = "/data/ocs_etc/" + dt.Rows[i]["SDATE"].ToString().Trim() + "/";
                        //'2014-11-18 서버에서 PC로 파일을 다운로드함
                        Ftpedt FtpedtX = new Ftpedt();
                        FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFileName + Convert.ToString(i + 1) + ".jpg", dt.Rows[i]["FILENAME"].ToString().Trim(), strRemotePath);
                        FtpedtX = null;
                    }

                    dt.Dispose();
                    dt = null;
                }

                //'ecg viwer 실행

                if (strViewerExe == "1")
                {
                    if (clsVbfunc.GetFile("%ProgramFiles%\\Windows Photo Gallery\\PhotoViewer.dll") != "")
                    {
                        strEMGFile = "rundll32.exe %ProgramFiles%\\Windows Photo Gallery\\PhotoViewer.dll, ImageView_Fullscreen " + strFileName + "1.jpg";
                    }
                    else
                    {
                        strEMGFile = "rundll32.exe shimgvw.dll, ImageView_Fullscreen " + strFileName + "1.jpg";
                    }

                    VB.Shell(strEMGFile, "MaximizedFocus");
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void Exam_Anat_FILE_DBToFile(string ArgANATNO)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            if (clsVbfunc.GetFile("C:\\WINDOWS\\SYSTEM32\\SHIMGVW.DLL") == "")
            {
                ComFunc.MsgBox("WINDOWS IMAGE Viewer가 설치되지 안았습니다.");
                return;
            }

            Ftpedt FtpedtX = new Ftpedt();
            byte[] b = null;
            MemoryStream stream = null;
            Bitmap image = null;

            int i = 0;
            string strFileName = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strAnatFile = "";

            DataTable dt = null;

            strFileName = @"c:\cmc\";

            try
            {
                SQL = " SELECT IMAGE ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_ANATMST_IMG    ";
                SQL = SQL + ComNum.VBLF + " WHERE ANATNO = '" + ArgANATNO + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        b = (byte[])(dt.Rows[i]["IMAGE"]);

                        stream = new MemoryStream(b);
                        image = new Bitmap(stream);
                        image.Save(strFileName + "anat" + i + ".jpg");
                    }
                }

                dt.Dispose();
                dt = null;

                if (clsVbfunc.GetFile("%ProgramFiles%\\Windows Photo Gallery\\PhotoViewer.dll") != "")
                {
                    strAnatFile = "rundll32.exe %ProgramFiles%\\Windows Photo Gallery\\PhotoViewer.dll, ImageView_Fullscreen " + strFileName + "anat0.jpg";
                }
                else
                {
                    strAnatFile = "rundll32.exe shimgvw.dll, ImageView_Fullscreen " + strFileName + "anat0.jpg";
                }

                VB.Shell(strAnatFile, "MaximizedFocus");

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }


        }

        private void Exam_FILE_DBToFile(string argWRTNO, string ArgPano, string strViewerExe)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            if (clsVbfunc.GetFile("C:\\WINDOWS\\SYSTEM32\\SHIMGVW.DLL") == "")
            {
                ComFunc.MsgBox("WINDOWS IMAGE Viewer가 설치되지 안았습니다.");
                return;
            }
            Ftpedt FtpedtX = new Ftpedt();
            byte[] b = null;
            MemoryStream stream = null;
            Bitmap image = null;

            int i = 0;
            string strFileName = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strExamFile = "";

            DataTable dt = null;

            try
            {
                DirectoryInfo dir = new DirectoryInfo(@"c:\cmc\");
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo F in files)
                {
                    if (F.Name.IndexOf("exam") != -1)
                    {
                        if (F.Extension == ".jpg")
                        {
                            F.Delete();
                        }
                    }
                }

                strFileName = @"C:\CMC\exam"; //'.jpg";

                SQL = " SELECT  IMAGE FROM ADMIN.EXAM_RESULT_IMG ";
                SQL = SQL + ComNum.VBLF + " WHERE  WRTNO = '" + argWRTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND IMAGE IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        b = (byte[])(dt.Rows[i]["IMAGE"]);

                        stream = new MemoryStream(b);
                        image = new Bitmap(stream);
                        image.Save(strFileName + i + ".jpg");
                    }
                }

                dt.Dispose();
                dt = null;

                //'ecg viwer 실행
                if (strViewerExe == "1")
                {
                    if (clsVbfunc.GetFile("%ProgramFiles%\\Windows Photo Gallery\\PhotoViewer.dll") != "")
                    {
                        strExamFile = "rundll32.exe %ProgramFiles%\\Windows Photo Gallery\\PhotoViewer.dll, ImageView_Fullscreen " + strFileName + "0.jpg";
                    }
                    else
                    {
                        strExamFile = "rundll32.exe shimgvw.dll, ImageView_Fullscreen " + strFileName + "0.jpg";
                    }
                    VB.Shell(strExamFile, "MaximizedFocus");
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }


        }

        private void SetXResultWord()
        {
            int i = 0;
            int intInx = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            //'기존의 내용을 Clear
            for (i = 0; i < arystrUseWord.Length; i++)
            {
                arystrUseWord[i] = "";
            }

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                //'DB에서 자료를 SELECT
                SQL = "";
                SQL = "SELECT CODE,WARDNAME ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.XRAY_RESULTWARD ";
                SQL = SQL + ComNum.VBLF + "WHERE SABUN = " + clsType.User.IdNumber + " ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["CODE"].ToString().Trim())
                        {
                            case "F1":
                                intInx = 1;
                                break;
                            case "F2":
                                intInx = 2;
                                break;
                            case "F3":
                                intInx = 3;
                                break;
                            case "F4":
                                intInx = 4;
                                break;
                            case "F5":
                                intInx = 5;
                                break;
                            case "F6":
                                intInx = 6;
                                break;
                            case "F7":
                                intInx = 7;
                                break;
                            case "F8":
                                intInx = 8;
                                break;
                            case "F9":
                                intInx = 9;
                                break;
                            case "F10":
                                intInx = 10;
                                break;
                        }
                        arystrUseWord[intInx] = dt.Rows[i]["WARDNAME"].ToString().Trim();
                    }
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 코드를 '검체,용기,결과단위,WS, 검체량로 변환한다.
        /// </summary>
        /// <seealso cref="READ_BasCode_NEW"/>>
        /// <param name="strGuBun">검체명,검체약어,용기명,용기약어,결과단위,검체량,WS명,WS약어,WS그룹만으로 사용 가능
        /// ※ 16은 검체량으로 사용</param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        private string GetBasCode(string strGuBun, string strCode)
        {
            string strVal = "";
            string strCGuBun = "";
            string strDTColName = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;

            switch (strGuBun)
            {
                case "검체명":
                    strCGuBun = "14";
                    strDTColName = "NAME";
                    break;
                case "검체약어":
                    strCGuBun = "14";
                    strDTColName = "YNAME";
                    break;
                case "용기명":
                    strCGuBun = "15";
                    strDTColName = "NAME";
                    break;
                case "용기약어":
                    strCGuBun = "15";
                    strDTColName = "YNAME";
                    break;
                case "결과단위":
                    strCGuBun = "20";
                    strDTColName = "NAME";
                    break;
                case "검체량":
                    strCGuBun = "16";
                    strDTColName = "NAME";
                    break;
                case "WS명":
                    strCGuBun = "12";
                    strDTColName = "NAME";
                    break;
                case "WS약어":
                    strCGuBun = "12";
                    strDTColName = "YNAME";
                    break;
                case "WS그룹":
                    strCGuBun = "12";
                    strDTColName = "WSGROUP";
                    break;
                default:
                    return strVal;
            }

            try
            {

                SQL = "";
                SQL = "SELECT CODE, NAME, YNAME, WSGROUP FROM ADMIN.EXAM_SPECODE       ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = '" + strCGuBun + "'       ";
                SQL = SQL + ComNum.VBLF + "    AND CODE = '" + strCode + "'        ";
                SQL = SQL + ComNum.VBLF + "    AND DELDATE IS NULL      ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SORT,CODE       ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0][strDTColName].ToString().Trim();
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return strVal;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="Patient_Name_READ"/>
        /// <param name="strPtNo"></param>
        private void SetPaatient(string strPtNo)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (clsType.User.JobGroup == "JOB013051")
            {
                btnHea.Enabled = true;
                btnHic.Enabled = true;
                btnEMR.Enabled = false;
                btnNVC.Enabled = false;
            }
            else
            {
                btnHea.Enabled = false;
                btnHic.Enabled = false;
                btnEMR.Enabled = false;
                btnNVC.Enabled = false;
            }

            if(clsType.User.IdNumber == "41827" || clsType.User.IdNumber == "19094" || clsType.User.IdNumber == "30322")
            {
                btnNVC.Enabled = true;
            }

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = "SELECT PANO,SNAME,JUMIN1,JUMIN2,JUMIN3,SEX ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + strPtNo.Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    mstrPano = dt.Rows[0]["PANO"].ToString().Trim();
                    mstrSName = dt.Rows[0]["SNAME"].ToString().Trim();
                    mstrJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    clsAES.Read_Jumin_AES(clsDB.DbCon, mstrPano);
                    mstrJumin2 = clsAES.GstrAesJumin2;
                    mstrSex = dt.Rows[0]["SEX"].ToString().Trim();
                    mintAge = ComFunc.AgeCalc(clsDB.DbCon, mstrJumin1 + mstrJumin2);
                }
                else
                {
                    mstrPano = "";
                    mstrSName = "";
                    mstrJumin1 = "";
                    mstrJumin2 = "";
                    mstrSex = "";
                    mintAge = 0;
                }

                dt.Dispose();
                dt = null;

                lblSName.Text = mstrSName;
                lblSexAge.Text = mstrSex + "/" + Convert.ToString(mintAge);

                //'등록번호로 종합건진 접수번호를 찾음 => 등록번호 -> 주민번호
                SQL = "";
                SQL = "SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,WRTNO ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.HEA_JEPSU ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO IN (SELECT PANO FROM ADMIN.HIC_PATIENT ";
                SQL = SQL + ComNum.VBLF + "      WHERE JUMIN2='" + clsAES.AES(mstrJumin1 + mstrJumin2) + "') ";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "ORDER BY JEPDATE DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    btnHea.Enabled = true;
                }

                dt.Dispose();
                dt = null;

                //'등록번호로 일반건진 접수번호를 찾음 => 등록번호 -> 주민번호
                SQL = "";
                SQL = "SELECT TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,WRTNO ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.HIC_JEPSU ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO IN (SELECT PANO FROM ADMIN.HIC_PATIENT ";
                SQL = SQL + ComNum.VBLF + "      WHERE JUMIN2 = '" + clsAES.AES(mstrJumin1 + mstrJumin2) + "') ";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "ORDER BY JEPDATE DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    btnHic.Enabled = true;
                }

                dt.Dispose();
                dt = null;

                //'EMR영상 존재여부확인
                SQL = "";
                SQL = " SELECT PATID FROM ADMIN.EMR_TREATT ";
                SQL = SQL + ComNum.VBLF + "WHERE PATID = '" + mstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "  AND CHECKED ='1'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    btnEMR.Enabled = true;
                }

                dt.Dispose();
                dt = null;

                //'생체현미경 검사 존재여부를 읽음
                SQL = "";
                SQL = "SELECT PtNo FROM ADMIN.ETC_RESULT_NVC ";
                SQL = SQL + ComNum.VBLF + "WHERE PtNo='" + mstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "  AND ResultTime IS NOT NULL";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    btnNVC.Enabled = true;
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
        }

        private string GetOrderName(string strCode)
        {
            DataTable dt = null;
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT ORDERNAME FROM ADMIN.OCS_ORDERCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE ORDERCODE = '" + strCode.Trim() + "'   ";
                //2019-12-03 안정수 제외처리함
                //SQL = SQL + ComNum.VBLF + "   AND (SENDDEPT <> 'N' OR SENDDEPT IS NULL) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["ORDERNAME"].ToString().Trim();
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);

            }
            return strVal;
        }


        private string GetReference(string astrCode, string astrAge, string astrSex, string argRdate)
        {
            int i = 0;
            string strVal = "";
            string strCode = "";
            string strNormal = "";
            string strSex = "";
            string strAgeFrom = "";
            string strAgeTo = "";
            string strRefValFrom = "";
            string strRefValTo = "";

            string strAllReference = "";
            string strReference = "";
            string strReferenceVal = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = " SELECT MASTERCODE, NORMAL, SEX, AGEFROM, AGETO, REFVALFROM, REFVALTO ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_MASTER_SUB ";
                SQL = SQL + ComNum.VBLF + "WHERE  1=1"; //'41:Reference Value
                //2018-12-12 안정수 조건 추가함 
                SQL = SQL + ComNum.VBLF + "AND MASTERCODE = '" + astrCode + "'";
                SQL = SQL + ComNum.VBLF + "AND GUBUN = '41'";
                SQL = SQL + ComNum.VBLF + "AND (SEX IS NULL OR SEX = ' ' OR SEX= '" + astrSex + "')  ";
                SQL = SQL + ComNum.VBLF + "AND ((AGEFROM = 0 AND AGETO = 99) OR  (AGEFROM <= '" + astrAge + "' AND AGETO >= '" + astrAge + "'))  ";
                if (argRdate.Length > 1)
                {
                    SQL = SQL + ComNum.VBLF + "AND ((EXPIREDATE IS NOT NULL AND EXPIREDATE >= '" + argRdate + "') OR (EXPIREDATE IS NULL)) ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY EXPIREDATE";
                }                

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strCode = dt.Rows[i]["MASTERCODE"].ToString().Trim();
                        strNormal = dt.Rows[i]["NORMAL"].ToString().Trim();
                        strSex = dt.Rows[i]["SEX"].ToString().Trim();
                        strAgeFrom = dt.Rows[i]["AGEFROM"].ToString().Trim();
                        strAgeTo = dt.Rows[i]["AGETO"].ToString().Trim();
                        strRefValFrom = dt.Rows[i]["REFVALFROM"].ToString().Trim();
                        strRefValTo = dt.Rows[i]["REFVALTO"].ToString().Trim();

                        strAllReference = strAllReference + "|" + strCode + "|" + strNormal + "|" + strSex + "|" + strAgeFrom + "|" +
                                        strAgeTo + "|" + strRefValFrom + "|" + strRefValTo;
                    }
                }

                dt.Dispose();
                dt = null;

                if (strCode == "")
                {
                    return strVal;
                }

                strReference = strAllReference.Replace(strCode, "^");

                for (i = 1; i < VB.Split(strReference, "|^").Length; i++)
                {
                    strReferenceVal = VB.Split(strReference, "|^")[i];

                    strNormal = VB.Split(strReferenceVal, "|")[1];
                    strSex = VB.Split(strReferenceVal, "|")[2];
                    strAgeFrom = VB.Split(strReferenceVal, "|")[3];
                    strAgeTo = VB.Split(strReferenceVal, "|")[4];
                    strRefValFrom = VB.Split(strReferenceVal, "|")[5];
                    strRefValTo = VB.Split(strReferenceVal, "|")[6];

                    if (strNormal != "")
                    {
                        strVal = strNormal;
                        break;
                    }

                    if (VB.Val(astrAge) >= 100)
                    {
                        strAgeTo = "150";
                    }

                    if (strSex == "" || strSex == astrSex)
                    {
                        if (strAgeFrom != "" && strAgeTo != "")
                        {
                            if (VB.Val(strAgeFrom) <= VB.Val(astrAge) && VB.Val(astrAge) <= VB.Val(strAgeTo))
                            {
                                strVal = strRefValFrom + " ~ " + strRefValTo;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        private void SelectBiopsy(int intRow)
        {
            string strROWID = "";
            string strAnatNo = "";            

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            strROWID = ss1_Sheet1.Cells[intRow, 3].Text.Trim();
            strAnatNo = ss1_Sheet1.Cells[intRow, 8].Text.Trim();

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = "SELECT ANATNO, GBJOB,  RESULT1, RESULT2, BDATE, RESULT_IMG,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(RESULTDATE,'YYYY-MM-DD HH24:MI') RESULTDATE ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_ANATMST ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["GBJOB"].ToString().Trim() != "V")
                    {
                        txtResult1.Text = ComNum.VBLF + "▶병리과에서 검사 결과 확인중";
                    }
                    else
                    {
                        txtResult1.Text = "▶병리번호: " + dt.Rows[0]["ANATNO"].ToString().Trim() 
                        + ComNum.VBLF + "▶처방일: " + dt.Rows[0]["BDATE"].ToString().Trim()
                        + ComNum.VBLF + "▶결과일: " + dt.Rows[0]["RESULTDATE"].ToString().Trim()
                        + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["RESULT1"].ToString().Trim()
                        + ComNum.VBLF + dt.Rows[0]["RESULT2"].ToString().Trim();

                        if (dt.Rows[0]["RESULT_IMG"].ToString().Trim() == "Y")
                        {
                            Exam_Anat_FILE_DBToFile(strAnatNo);
                        }

                        //'CV 결과
                        if (mstrDrCode1 == "")
                        {
                            UpdateCvrConfirmDate(strAnatNo, strROWID);
                        }
                    }
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// CV 결과
        /// </summary>
        /// <param name="strAnatNo"></param>
        /// <param name="strROWID"></param>
        private void UpdateCvrConfirmDate(string strAnatNo, string strROWID)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);


            try
            {


                SQL = "";
                SQL = " SELECT ROWID FROM ADMIN.EXAM_ANATMST ";
                SQL = SQL + ComNum.VBLF + " WHERE ANATNO ='" + strAnatNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CVR_CONFIRM_DATE2 IS NULL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                        return; //권한 확인

                    SQL = "";
                    SQL = "UPDATE ADMIN.EXAM_ANATMST SET ";
                    SQL = SQL + ComNum.VBLF + "        CVR_CONFIRM_DATE2 =SYSDATE,";
                    SQL = SQL + ComNum.VBLF + "        CVR_CONFIRM_SABUN2 = '" + clsType.User.IdNumber + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("병리조직 CVR확인일자 UPDATE시 Error 발생!!" + ComNum.VBLF + "DB에 Update중 오류가 발생함.");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }
        
        private void SelectCytology(int intRow)
        {
            string strROWID = "";

            strROWID = ss1_Sheet1.Cells[intRow, 3].Text.Trim();

            txtResult1.Text = GetCytologyResult(strROWID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strROWID"></param>
        /// <returns></returns>
        public string GetCytologyResult(string strROWID)
        {
            string strVal = "";
            ;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strAnatNo = "";
            string strHRRemark1 = "";
            string strHRRemark2 = "";
            string strHRRemark3 = "";
            string strHRRemark4 = "";
            string strHRRemark5 = "";

            string strResultSabun = "";

            string strResult = "";
            string strRDate = "";
            string strRDate2 = "";
            string strReDate = "";

            DataTable dt = null;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return strVal; //권한 확인

                SQL = "";
                SQL = " SELECT A.ANATNO, A.GBJOB, A.RESULT1, A.RESULT2, A.HRREMARK1, A.HRREMARK2,";
                SQL = SQL + ComNum.VBLF + "  A.HRREMARK3, A.HRREMARK4, A.HRREMARK5,";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.RESULTDATE ,'YYYY-MM-DD') RDATE, ";
                SQL = SQL + ComNum.VBLF + "   TO_CHAR(B.RECEIVEDATE, 'YYYY-MM-DD') REDATE, A.RESULTDATE, A.RESULTSABUN ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.EXAM_ANATMST A , ADMIN.EXAM_SPECMST B";
                SQL = SQL + ComNum.VBLF + " WHERE A.ROWID = '" + strROWID + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.SPECNO = B.SPECNO(+) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["GBJOB"].ToString().Trim() != "V")
                    {
                        strVal = ComNum.VBLF + " ▶병리과에서 검사 결과 확인중";
                        dt.Dispose();
                        dt = null;
                        return strVal;
                    }
                    else
                    {
                        strRDate = dt.Rows[0]["RDATE"].ToString().Trim();
                        strReDate = dt.Rows[0]["REDATE"].ToString().Trim();

                        strResult = ComNum.VBLF + ComFunc.LeftH("▶ 병리번호: " + VB.Space(20), 20) + dt.Rows[0]["ANATNO"].ToString().Trim() + ComNum.VBLF;
                        strResult = strResult + ComNum.VBLF + ComFunc.LeftH("▶ 검사일: " + VB.Space(20), 20) + strReDate;
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 보고일: " + VB.Space(20), 20) + strRDate;
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + "---------------------------------------------------------------";
                        strResult = strResult + ComNum.VBLF + dt.Rows[0]["RESULT1"].ToString().Trim() + dt.Rows[0]["RESULT2"].ToString().Trim();

                        strAnatNo = dt.Rows[0]["ANATNO"].ToString().Trim();
                        strHRRemark1 = dt.Rows[0]["HRREMARK1"].ToString().Trim();
                        strHRRemark2 = dt.Rows[0]["HRREMARK2"].ToString().Trim();
                        strHRRemark3 = dt.Rows[0]["HRREMARK3"].ToString().Trim();
                        strHRRemark4 = dt.Rows[0]["HRREMARK4"].ToString().Trim();
                        strHRRemark5 = dt.Rows[0]["HRREMARK5"].ToString().Trim();
                        strRDate2 = dt.Rows[0]["RESULTDATE"].ToString().Trim();
                        strResultSabun = dt.Rows[0]["RESULTSABUN"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                if (VB.Left(strAnatNo, 2) == "PS") //객담 --------------------------------------------------------------------------
                {
                    //'검체상태
                    strResult += ComNum.VBLF + ComFunc.LeftH("▶ 병리번호: " + VB.Space(20), 20) + strAnatNo + ComNum.VBLF;

                    strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 검체상태 : " + VB.Space(20), 20);

                    if (VB.Mid(strHRRemark1, 1, 1) == "1") //적정
                    {
                        strResult = strResult + "적정 ";
                    }
                    else if (VB.Mid(strHRRemark1, 2, 1) == "1") //제한적
                    {
                        strResult = strResult + "제한적 ";
                    }
                    else if (VB.Mid(strHRRemark1, 3, 1) == "1") //불량
                    {
                        strResult = strResult + "불량 ";
                    }

                    //'결과
                    strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 결    과 : " + VB.Space(20), 20);

                    if (VB.Mid(strHRRemark1, 4, 1) == "1") //'음성(I)
                    {
                        strResult = strResult + "음성(I)";
                    }
                    else if (VB.Mid(strHRRemark1, 5, 1) == "1") //음성(II)
                    {
                        strResult = strResult + "음성(II)";
                    }
                    else if (VB.Mid(strHRRemark1, 6, 1) == "1") //'의양성(III)
                    {
                        strResult = strResult + "의양성(III)";
                    }
                    else if (VB.Mid(strHRRemark1, 7, 1) == "1") //'양성(IV)
                    {
                        strResult = strResult + "양성(IV)";
                    }
                    else if (VB.Mid(strHRRemark1, 8, 1) == "1") //'양성(V)
                    {
                        strResult = strResult + "양성(V)";
                    }

                    //'종합판정
                    strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 종합판정 : " + VB.Space(20), 20);

                    if (VB.Mid(strHRRemark1, 9, 1) == "1") //'가래 채취 방법 설명 후 재검사(A)
                    {
                        strResult = strResult + "가래 채취 방법 설명 후 재검사(A)";
                    }
                    else if (VB.Mid(strHRRemark1, 10, 1) == "1") // '정기적 조사(B)
                    {
                        strResult = strResult + "정기적 조사(B)";
                    }
                    else if (VB.Mid(strHRRemark1, 11, 1) == "1") // '재검(C)
                    {
                        strResult = strResult + "재검(C)";
                        if (VB.Mid(strHRRemark1, 14, 1) == "1") // '즉시
                        {
                            strResult = strResult + ": 즉시";
                        }
                        else if (VB.Mid(strHRRemark1, 15, 1) == "1") // '6개월이내
                        {
                            strResult = strResult + ": 6개월이내";
                        }
                    }
                    else if (VB.Mid(strHRRemark1, 12, 1) == "1") // '정밀검사필요(D)
                    {
                        strResult = strResult + "정밀검사필요(D)";
                    }
                    else if (VB.Mid(strHRRemark1, 13, 1) == "1") // '암(E)
                    {
                        strResult = strResult + "암(E)";
                    }

                    strResult = strResult + ComNum.VBLF + strHRRemark5;
                }
                else if (VB.Left(strAnatNo, 2) == "PU") //'소변
                {
                    //'검체상태
                    strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 검체상태 : " + VB.Space(20), 20);

                    if (VB.Mid(strHRRemark1, 1, 1) == "1") //적정
                    {
                        strResult = strResult + "적정 ";
                    }
                    else if (VB.Mid(strHRRemark1, 2, 1) == "1") //제한적
                    {
                        strResult = strResult + "제한적 ";
                    }
                    else if (VB.Mid(strHRRemark1, 3, 1) == "1") //불량
                    {
                        strResult = strResult + "불량 ";
                    }

                    //'결과
                    strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 결    과 : " + VB.Space(20), 20);

                    if (VB.Mid(strHRRemark1, 4, 1) == "1") //'음성(I)
                    {
                        strResult = strResult + "음성(I)";
                    }
                    else if (VB.Mid(strHRRemark1, 5, 1) == "1") //음성(II)
                    {
                        strResult = strResult + "음성(II): 비정형성(악성근거없음)";
                    }
                    else if (VB.Mid(strHRRemark1, 6, 1) == "1") //'의양성(III)
                    {
                        strResult = strResult + "음성(III): 악성의심(확실하지않음)";
                    }
                    else if (VB.Mid(strHRRemark1, 7, 1) == "1") //'양성(IV)
                    {
                        strResult = strResult + "음성(IV): 악성이 강하게 의심";
                    }
                    else if (VB.Mid(strHRRemark1, 8, 1) == "1") //'양성(V)
                    {
                        strResult = strResult + "음성(V): 악성세포";
                    }

                    //'대책
                    strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 대    책 : " + VB.Space(20), 20);

                    if (VB.Mid(strHRRemark1, 9, 1) == "1") //'소변 채취 방법 설명 후 재검사
                    {
                        strResult = strResult + "소변 채취 방법 설명 후 재검사";
                    }
                    else if (VB.Mid(strHRRemark1, 10, 1) == "1") // '정밀검사필요
                    {
                        strResult = strResult + "정밀검사필요";
                    }

                    strResult = strResult + ComNum.VBLF + strHRRemark5;
                }

                else if (VB.Left(strAnatNo, 1) == "P") //'부인과암(자궁도말)
                {
                    if (VB.Val((strAnatNo).Replace("P", "")) >= VB.Val("0900134"))  //if (strAnatNo >= "P0900134")
                    {
                        strResult = ComNum.VBLF + ComFunc.LeftH("▶ 병    리   번   호: " + VB.Space(25), 25) + strAnatNo + ComNum.VBLF;
                        strResult += ComNum.VBLF + ComFunc.LeftH("▶ 검      사      일 : " + VB.Space(25), 25) + strReDate;
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 보      고      일 : " + VB.Space(25), 25) + strRDate;

                        //'검체상태
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 검    체   상   태 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 1, 1) == "1") //'적정
                        {
                            strResult = strResult + "1.적정";
                        }
                        else if (VB.Mid(strHRRemark1, 2, 1) == "1") //'부적절
                        {
                            strResult = strResult + "2.부적절";
                        }

                        //'자궁경부 선상피세포(유,무)
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 자궁경부선상피세포 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 3, 1) == "1") //'유
                        {
                            strResult = strResult + "1.유 ";
                        }
                        else if (VB.Mid(strHRRemark1, 4, 1) == "1") //'무
                        {
                            strResult = strResult + "2.무 ";
                        }

                        //'유형별 진단
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 유  형  별  진  단 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 5, 1) == "1") //1.음성
                        {
                            strResult = strResult + "1.음성 ";
                        }
                        else if (VB.Mid(strHRRemark1, 6, 1) == "1") //2.상피세포이상
                        {
                            strResult = strResult + "2.상피세포이상";
                        }
                        else if (VB.Mid(strHRRemark1, 7, 1) == "1") //3.기타(자궁내막세포출현)
                        {
                            strResult = strResult + "3.기타(자궁내막세포출현)";
                        }

                        //'편평상피세포이상
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 편평 상피세포 이상 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 8, 1) == "1") // '비정형 편평상피세포
                        {
                            strResult = strResult + "1.비정형 편평상피세포";
                            if (VB.Mid(strHRRemark1, 9, 1) == "1") //'일반
                            {
                                strResult = strResult + ":일반";
                            }
                            else if (VB.Mid(strHRRemark1, 10, 1) == "1") //'고위험
                            {
                                strResult = strResult + ":고위험";
                            }
                        }
                        else if (VB.Mid(strHRRemark1, 11, 1) == "1") //2.저등급 편평상피내 병변
                        {
                            strResult = strResult + "2.저등급 편평상피내 병변";
                        }
                        else if (VB.Mid(strHRRemark1, 12, 1) == "1") //3.고등급 편평상피내 병변
                        {
                            strResult = strResult + "3.고등급 편평상피내 병변";
                        }
                        else if (VB.Mid(strHRRemark1, 13, 1) == "1") //4.침윤성 편평세표암종
                        {
                            strResult = strResult + "4.침윤성 편평세표암종";
                        }

                        //'선상피세포이상
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 선 상 피 세포 이상 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 14, 1) == "1") //'비정형 선상피세포
                        {
                            strResult = strResult + "1.비정형 선상피세포";

                            #region 2018-07-10 안정수, 병리과요청으로 일반, 종양성 항목 추가 

                            if (VB.Mid(strHRRemark1, 31, 1) == "1") //'일반
                            {
                                strResult = strResult + ":일반";
                            }
                            else if (VB.Mid(strHRRemark1, 32, 1) == "1") //'종양성
                            {
                                strResult = strResult + ":종양성";
                            }

                            #endregion
                        }
                        else if (VB.Mid(strHRRemark1, 15, 1) == "1") //'상피내 선압종
                        {
                            strResult = strResult + "2.상피내 선압종";
                        }
                        else if (VB.Mid(strHRRemark1, 16, 1) == "1") //'침윤성 선암종
                        {
                            strResult = strResult + "3.침윤성 선암종";
                        }
                        else if (VB.Mid(strHRRemark1, 17, 1) == "1") //'기타
                        {
                            strResult = strResult + "4.기타: ";
                            strResult = strResult + strHRRemark2;
                        }

                        //'추가소견
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 추   가    소   견 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 18, 1) == "1") //'반응성 세포변화
                        {
                            strResult = strResult + "1.반응성 세포변화" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 19, 1) == "1") //'트리코모나스
                        {
                            strResult = strResult + "2.트리코모나스 " + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 20, 1) == "1") //캔디다
                        {
                            strResult = strResult + "3.캔디다" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 21, 1) == "1") //'방선균
                        {
                            strResult = strResult + "4.방선균" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 22, 1) == "1") //'헤르페스 바이러스
                        {
                            strResult = strResult + "5.헤르페스 바이러스" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 23, 1) == "1") //'기타
                        {
                            strResult = strResult + ComNum.VBLF + VB.Space(25) + "6.기타 : ";
                            strResult = strResult + strHRRemark3;
                        }

                        //'종합판정
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 종   합    판   정 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 24, 1) == "1")
                        {
                            strResult = strResult + "1.이상소견 없음 ";
                        }
                        else if (VB.Mid(strHRRemark1, 25, 1) == "1")
                        {
                            strResult = strResult + "2.염증성 또는 감염성질환";
                        }
                        else if (VB.Mid(strHRRemark1, 26, 1) == "1")
                        {
                            strResult = strResult + "3.상피세포 이상";
                        }
                        else if (VB.Mid(strHRRemark1, 30, 1) == "1")
                        {
                            strResult = strResult + "4.자궁경부암 전구단계";
                        }
                        else if (VB.Mid(strHRRemark1, 27, 1) == "1")
                        {
                            strResult = strResult + "5.자궁경부암 의심";
                        }
                        else if (VB.Mid(strHRRemark1, 28, 1) == "1")
                        {
                            strResult = strResult + "6.기타:";
                            strResult = strResult + strHRRemark4;
                        }
                        else if (VB.Mid(strHRRemark1, 29, 1) == "1")
                        {
                            strResult = strResult + "기존 자궁경부암환자";
                        }

                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + strHRRemark5;
                    }
                    else
                    {
                        strResult = ComNum.VBLF + ComFunc.LeftH("▶ 병    리   번    호: " + VB.Space(25), 25) + strAnatNo + ComNum.VBLF;
                        strResult += ComNum.VBLF + ComFunc.LeftH("▶ 검      사      일 : " + VB.Space(25), 25) + strReDate;
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 보      고      일 : " + VB.Space(25), 25) + strRDate;

                        // '검체상태
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 검    체   상   태 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 1, 1) == "1") //'적정
                        {
                            strResult = strResult + "1.적정 ";
                        }
                        else if (VB.Mid(strHRRemark1, 2, 1) == "1") //'부적절
                        {
                            strResult = strResult + "2.부적절 ";
                        }

                        //'자궁경부 선상피세포(유,무)
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 자궁경부선상피세포 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 3, 1) == "1") //1.유
                        {
                            strResult = strResult + "1.유 ";
                        }
                        else if (VB.Mid(strHRRemark1, 4, 1) == "1") //2.무
                        {
                            strResult = strResult + "2.무 ";
                        }

                        //'유형별 진단
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 유  형  별  진  단 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 5, 1) == "1") //음성
                        {
                            strResult = strResult + "1.음성 ";
                        }
                        else if (VB.Mid(strHRRemark1, 6, 1) == "1") //상피세포이상
                        {
                            strResult = strResult + "2.상피세포이상";
                        }
                        else if (VB.Mid(strHRRemark1, 7, 1) == "1") //기타(자궁내막세포출현)
                        {
                            strResult = strResult + "3.기타(자궁내막세포출현)";
                        }

                        //'편평상피세포이상
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 편평 상피세포 이상 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 8, 1) == "1") //'비정형 편평상피세포
                        {
                            strResult = strResult + "1.비정형 편평상피세포";
                        }
                        else if (VB.Mid(strHRRemark1, 9, 1) == "1") //'저등급 편평상피내 병변
                        {
                            strResult = strResult + "2.저등급 편평상피내 병변";
                        }
                        else if (VB.Mid(strHRRemark1, 10, 1) == "1") //'고등급 편평상피내 병변
                        {
                            strResult = strResult + "3.고등급 편평상피내 병변";
                        }
                        else if (VB.Mid(strHRRemark1, 11, 1) == "1") //침윤성 편평세표암종
                        {
                            strResult = strResult + "4.침윤성 편평세표암종";
                        }

                        //'선상피세포이상
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 선 상 피 세포 이상 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 12, 1) == "1") //'비정형 선상피세포
                        {
                            strResult = strResult + "1.비정형 선상피세포";
                        }
                        else if (VB.Mid(strHRRemark1, 13, 1) == "1")//'상피내 선압종
                        {
                            strResult = strResult + "2.상피내 선압종";
                        }
                        else if (VB.Mid(strHRRemark1, 14, 1) == "1")//'침윤성 선암종
                        {
                            strResult = strResult + "3.침윤성 선암종";
                        }
                        else if (VB.Mid(strHRRemark1, 15, 1) == "1") //'기타
                        {
                            strResult = strResult + "4.기타: ";
                        }

                        //'추가소견
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 추   가    소   견 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 16, 1) == "1")  //'반응성 세포변화
                        {
                            strResult = strResult + "1.반응성 세포변화" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 17, 1) == "1") //'트리코모나스
                        {
                            strResult = strResult + "2.트리코모나스 " + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 18, 1) == "1") //'캔디다
                        {
                            strResult = strResult + "3.캔디다" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 19, 1) == "1")//'방선균
                        {
                            strResult = strResult + "4.방선균" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 20, 1) == "1") //'헤르페스 바이러스
                        {
                            strResult = strResult + "5.헤르페스 바이러스" + ComNum.VBLF + VB.Space(25);
                        }
                        if (VB.Mid(strHRRemark1, 21, 1) == "1") //'기타
                        {
                            strResult = strResult + ComNum.VBLF + VB.Space(25) + "6.기타 : ";
                            strResult = strResult + strHRRemark3;
                        }

                        //'종합판정
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + ComFunc.LeftH("▶ 종   합    판   정 : " + VB.Space(25), 25);

                        if (VB.Mid(strHRRemark1, 22, 1) == "1") //'정상
                        {
                            strResult = strResult + "1.정상";
                        }
                        else if (VB.Mid(strHRRemark1, 23, 1) == "1") //'염증성 또는 감염성질환
                        {
                            strResult = strResult + "2.추적검사필요";
                        }
                        else if (VB.Mid(strHRRemark1, 24, 1) == "1") //'상피세포 이상
                        {
                            strResult = strResult + "3.암의심정밀검사필요";
                        }
                        else if (VB.Mid(strHRRemark1, 25, 1) == "1") //'자궁경부암 의심
                        {
                            strResult = strResult + "4.기타질환:";
                        }
                        else if (VB.Mid(strHRRemark1, 26, 1) == "1") //'기타
                        {
                            strResult = strResult + "5.기 암환자";
                        }
                        strResult = strResult + ComNum.VBLF + ComNum.VBLF + strHRRemark5;
                    }
                }
                strResult = strResult + ComNum.VBLF + ComNum.VBLF + "판정일자 : " + strRDate2 + "    판정의사 : " + CF.Read_SabunName(clsDB.DbCon, strResultSabun);
                strVal = strResult;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }


            return strVal;
        }

        private void SelectVerify(int intRow)
        {
            string strROWID = "";
            string strResult = "";
            string strPano = "";
            string strSname = "";
            string strAgeSex = "";
            string strWard = "";
            string strDeptName = "";

            string strJDate = "";
            string strSDate = "";
            string strEdate = "";
            string[] strRESULTDATE = new string[10];
            int intResultSabun = 0;
            string strRDRName = "";
            string strRDrBunho = "";
            string strDrname = "";
            DateTime date;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            strROWID = ss1_Sheet1.Cells[intRow, 3].Text.Trim();

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                //'자료를 DB에서 READ
                SQL = "SELECT PANO, SNAME, SEX, AGE, DEPTCODE, DRCODE, WARD, STATUS,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(JDATE,'YYYY-MM-DD') JDATE,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(RESULTDATE,'YYYY-MM-DD') RESULTDATE,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(SDATE,'YYYY-MM-DD') SDATE,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(EDATE,'YYYY-MM-DD') EDATE,";
                SQL = SQL + ComNum.VBLF + "RESULTSABUN, ITEMS1, ITEMS2, DISEASE,";
                SQL = SQL + ComNum.VBLF + "VERIFY1, VERIFY2, VERIFY3, VERIFY4, VERIFY5, VERIFY6,";
                SQL = SQL + ComNum.VBLF + "COMMENTS, RECOMMENDATION, PRINT ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_VERIFY ";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strPano = dt.Rows[0]["PANO"].ToString().Trim();
                    strSname = dt.Rows[0]["SNAME"].ToString().Trim();
                    strAgeSex = dt.Rows[0]["AGE"].ToString().Trim() + "/" + dt.Rows[0]["SEX"].ToString().Trim();
                    strWard = dt.Rows[0]["WARD"].ToString().Trim();
                    strDeptName = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[0]["DEPTCODE"].ToString().Trim());

                    strDrname = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());
                    strJDate = dt.Rows[0]["JDATE"].ToString().Trim();
                    strRESULTDATE[0] = dt.Rows[0]["RESULTDATE"].ToString().Trim();
                    strSDate = dt.Rows[0]["SDATE"].ToString().Trim();
                    strEdate = dt.Rows[0]["EDATE"].ToString().Trim();
                    intResultSabun = Convert.ToInt32(VB.Val(dt.Rows[0]["RESULTSABUN"].ToString().Trim()));

                    //'전문의번호 및 성명을 READ
                    switch (intResultSabun)
                    {
                        case 9089:
                            strRDRName = "김성철";
                            strRDrBunho = "301";
                            break;
                        case 18210:
                            strRDRName = "은상진";
                            strRDrBunho = "424";
                            break;
                        default:
                            strRDRName = "의사오류";
                            strRDrBunho = "***";
                            break;
                    }

                    strResult = "";
                    strResult = strResult + "등록 번호: " + strPano + ComNum.VBLF;
                    strResult = strResult + "환자 성명: " + strSname + ComNum.VBLF;
                    strResult = strResult + "나이/성별: " + strAgeSex + ComNum.VBLF;
                    strResult = strResult + "진료 병동: " + strWard + ComNum.VBLF;
                    strResult = strResult + "진  료 과: " + strDeptName + ComNum.VBLF;
                    strResult = strResult + "주치 의사: " + strDrname + ComNum.VBLF + ComNum.VBLF;

                    //'임상소견
                    strResult = strResult + "임상 소견: " + dt.Rows[0]["DISEASE"].ToString().Trim() + ComNum.VBLF; //'임상소견
                    strResult = strResult + "---------------------------------------------------------------" + ComNum.VBLF + ComNum.VBLF;

                    if (strSDate == "")
                    {
                        strResult = strResult + "■ 검증항목(열거)" + ComNum.VBLF;
                    }
                    else
                    {
                        strResult = strResult + "■ 검증항목(검사기간:  " + strSDate + " ~ " + strEdate + ")" + ComNum.VBLF;
                    }

                    strResult = strResult + dt.Rows[0]["ITEMS1"].ToString().Trim() + ComNum.VBLF;

                    strResult = strResult + "---------------------------------------------------------------" + ComNum.VBLF + ComNum.VBLF;

                    //'비정상 결과 혹은 유의한 결과를 보이는 항목
                    strResult = strResult + "■ 비정상 결과 혹은 유의한 결과를 보이는 항목" + ComNum.VBLF;
                    strResult = strResult + dt.Rows[0]["ITEMS2"].ToString().Trim() + ComNum.VBLF;


                    strResult = strResult + "---------------------------------------------------------------" + ComNum.VBLF + ComNum.VBLF;

                    //'검증방법
                    strResult = strResult + "■ 검증방법" + ComNum.VBLF;
                    strResult = strResult + (dt.Rows[0]["VERIFY1"].ToString().Trim() == "Y" ? "●" : "○") + "Calibratrion Verification" + ComNum.VBLF;
                    strResult = strResult + (dt.Rows[0]["VERIFY4"].ToString().Trim() == "Y" ? "●" : "○") + "Internal Quality Control" + ComNum.VBLF;
                    strResult = strResult + (dt.Rows[0]["VERIFY2"].ToString().Trim() == "Y" ? "●" : "○") + "Delta Check Verification" + ComNum.VBLF;
                    strResult = strResult + (dt.Rows[0]["VERIFY5"].ToString().Trim() == "Y" ? "●" : "○") + "Panic/Alert Value Verification" + ComNum.VBLF;
                    strResult = strResult + (dt.Rows[0]["VERIFY3"].ToString().Trim() == "Y" ? "●" : "○") + "Repeat/Recheck" + ComNum.VBLF;

                    if (dt.Rows[0]["VERIFY6"].ToString().Trim() != "")
                    {
                        strResult = strResult + "●Others" + dt.Rows[0]["VERIFY6"].ToString().Trim() + ComNum.VBLF;
                    }
                    else
                    {
                        strResult = strResult + "○Others" + ComNum.VBLF;
                    }

                    strResult = strResult + "* 검사결과는 위의 표시된 방법에 의하여 검증되었습니다." + ComNum.VBLF;

                    strResult = strResult + "---------------------------------------------------------------" + ComNum.VBLF + ComNum.VBLF;

                    //'검증/판독 소견(Comments)
                    strResult = strResult + "■ 검증/판독 소견(Comments)" + ComNum.VBLF;
                    strResult = strResult + dt.Rows[0]["COMMENTS"].ToString().Trim() + ComNum.VBLF;
                    strResult = strResult + "---------------------------------------------------------------" + ComNum.VBLF + ComNum.VBLF;

                    //'추천(Recommendation)
                    strResult = strResult + "■ 추천(Recommendation)" + ComNum.VBLF;

                    strResult = strResult + dt.Rows[0]["ReCommendation"].ToString().Trim() + ComNum.VBLF;

                    date = Convert.ToDateTime(strJDate);

                    strResult = strResult + "보고일 : " + date.Year.ToString() + "년 " + date.Month.ToString("#0") + "월 " + date.Day.ToString("#0") + "일" + ComNum.VBLF;
                    strResult = strResult + "보고자 : 진단검사의학전문의 " + strRDRName + ComNum.VBLF;
                    strResult = strResult + "         전문의 번호( " + strRDrBunho + " ) " + ComNum.VBLF;

                    txtResult1.Text = strResult;
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void SelectEndo(int intRow)
        {
            double dblSeqno = 0;
            string strROWID = "";
            string strDate = "";
            string strInfo = "";  //'2013-01-15
            string strTPro = "";  //'2015-07-23

            picPicturePic.Visible = true;
            picPicturePic.Image = null;

            txtResult1.Text = "";

            if (intRow < 0)
                return;

            dblSeqno = VB.Val(ss1_Sheet1.Cells[intRow, 3].Text.Trim());
            strROWID = ss1_Sheet1.Cells[intRow, 4].Text.Trim();

            switch (ss1_Sheet1.Cells[intRow, 5].Text.Trim())
            {
                case "1":
                    picPicturePic.Image = Bitmap.FromFile("c:\\cmc\\icons\\내시경1.bmp");
                    break;
                case "2":
                    picPicturePic.Image = Bitmap.FromFile("c:\\cmc\\icons\\내시경2.bmp");
                    break;
                case "3":
                    picPicturePic.Image = Bitmap.FromFile("c:\\cmc\\icons\\내시경3.bmp");
                    break;
            }

            Read_Result(intRow, dblSeqno, ref strDate, ref strInfo, ref strTPro);

            Read_Remark(strROWID);

            if (strDate != "")
            {
                txtResult1.Text = strDate + ComNum.VBLF + txtResult1.Text;
            }

        }

        private void Read_Remark(string strROWID)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strRemark = "";

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = " SELECT REMARKC, REMARKX, REMARKP, REMARKD ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.ENDO_JUPMST A,   ";
                SQL = SQL + ComNum.VBLF + "      ADMIN.ENDO_REMARK B    ";
                SQL = SQL + ComNum.VBLF + " WHERE A.ROWID      = '" + strROWID + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO       = B.PTNO      ";
                SQL = SQL + ComNum.VBLF + "   AND A.JDATE      = B.JDATE     ";
                SQL = SQL + ComNum.VBLF + "   AND A.ORDERCODE  = B.ORDERCODE ";
                SQL = SQL + ComNum.VBLF + "   AND ROWNUM       = 1 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strRemark = strRemark + "◈ Chief Complaints : " + dt.Rows[0]["REMARKC"].ToString().Trim();
                    strRemark = strRemark + ComNum.VBLF + "◈ Clinical Diagnosis : " + dt.Rows[0]["REMARKD"].ToString().Trim();
                }

                if (strRemark != "")
                {
                    txtResult1.Text = strRemark + ComNum.VBLF + ComNum.VBLF + txtResult1.Text;
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void Read_Result(int intRow, double dblSeqno, ref string strDate, ref string strInfo, ref string strTPro)
        {
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            //Read_Result:
            string strRemark = "";
            string strKorName = "";
            string strResultDrCode = "";
            string strResult6_new = "";
            string strGUBUN2 = "";
            string strLowTime = "";

            string strGubun_Gue = "";
            string strGubun = "";

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                if (dblSeqno == 0)
                    return;

                SQL = "";
                SQL = " SELECT A.REMARK1, A.REMARK2, A.REMARK3, A.REMARK4, A.REMARK5, A.REMARK6, A.PICXY, ";
                SQL = SQL + ComNum.VBLF + " B.GBPRE_1,B.GBPRE_2,B.GBPRE_21,B.GBPRE_22,B.GBPRE_3,B.GBCON_1,B.GBCON_2,B.GBCON_21,";
                SQL = SQL + ComNum.VBLF + " B.GBCON_22,B.GBCON_3,B.GBCON_31,B.GBCON_32,B.GBCON_4,B.GBCON_41,B.GBCON_42,B.GBPRO_1,B.GBPRO_2,B.GBPRE_31,B.GB_CLEAN, ";
                //'2014-10-24 기관지경부분
                SQL = SQL + ComNum.VBLF + " B.GBPRE_4,B.GBPRE_41,B.GBPRE_42,B.GBCON_5,B.GBCON_51,B.GBMED_1, ";
                SQL = SQL + ComNum.VBLF + " B.GBMED_2 , B.GBMED_21, B.GBMED_22, B.GBMED_3, B.GBMED_31, B.GBMED_32, B.GBMED_4, B.GBMED_41,";

                //'2015-07-23
                SQL = SQL + ComNum.VBLF + " GUBUN, GUBUN_GUE,MOAAS,D_INTIME1,D_INTIME2,D_EXTIME1,D_EXTIME2,";
                SQL = SQL + ComNum.VBLF + " PRO_BX1,PRO_BX2,PRO_PP1,PRO_PP2,PRO_ESD1,PRO_ESD2,PRO_ESD3_1,PRO_ESD3_2,PRO_EMR1,";
                SQL = SQL + ComNum.VBLF + " PRO_EMR2,PRO_EMR3_1,PRO_EMR3_2,PRO_APC,PRO_ELEC,PRO_HEMO1,PRO_HEMO2,PRO_EPNA1,";
                SQL = SQL + ComNum.VBLF + " PRO_EPNA2,PRO_BAND1,PRO_BAND2,PRO_MBAND,PRO_HIST1,PRO_HIST2,PRO_DETA,PRO_EST,";
                SQL = SQL + ComNum.VBLF + " PRO_BALL,PRO_BASKET,PRO_EPBD1,PRO_EPBD2,PRO_EPBD3,PRO_EPBD4,PRO_ENBD1,PRO_ENBD2,";
                SQL = SQL + ComNum.VBLF + " PRO_ENBD3 , PRO_ERBD1, PRO_ERBD2, PRO_ERBD3, PRO_ERBD4, PRO_EST_STS, PRO_RUT,";

                SQL = SQL + ComNum.VBLF + " A.REMARK6_2,A.REMARK6_3,A.REMARK, ";

                SQL = SQL + ComNum.VBLF + " TO_CHAR(B.JDATE,'YYYY-MM-DD HH24:MI') JDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(B.RDATE,'YYYY-MM-DD HH24:MI') RDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(B.RESULTDATE,'YYYY-MM-DD HH24:MI') RESULTDATE,B.GBNEW, ";
                SQL = SQL + ComNum.VBLF + " B.RESULTDRCODE ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ENDO_RESULT A, ADMIN.ENDO_JUPMST B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.SEQNO  = " + dblSeqno + " ";
                SQL = SQL + ComNum.VBLF + "   AND A.SEQNO = B.SEQNO ";
                SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                strRemark = "";
                strDate = "";

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count == 1)
                {
                    strGUBUN2 = ss1_Sheet1.Cells[intRow, 5].Text.Trim(); //'검사구분

                    if (dblSeqno != 0) //TODO : 2017.06.26 Display_Picture(); - 영록 //'add

                        strResultDrCode = dt.Rows[0]["RESULTDRCODE"].ToString().Trim();

                    if (strResultDrCode != "")
                    {
                        SQL = "";
                        SQL = " SELECT KORNAME FROM ADMIN.INSA_MST ";
                        SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + strResultDrCode + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            strKorName = dt1.Rows[0]["KORNAME"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                    else
                    {
                        strResultDrCode = "";
                        strKorName = "";
                    }

                    strDate = strDate + "◈===================================================" + ComNum.VBLF;
                    strDate = strDate + "◈접수일자:  " + dt.Rows[0]["JDATE"].ToString().Trim() + ComNum.VBLF;
                    strDate = strDate + "◈검사일자:  " + dt.Rows[0]["RDATE"].ToString().Trim() + ComNum.VBLF;
                    strDate = strDate + "◈결과일자:  " + dt.Rows[0]["RESULTDATE"].ToString().Trim() + ComNum.VBLF;
                    strDate = strDate + "◈===================================================" + ComNum.VBLF + ComNum.VBLF;

                    //'2013-01-15 new add -------------------------------------------------------
                    strInfo = "◈ Premedication ◈" + ComNum.VBLF;

                    //'2014-10-24 기관지경 변경추가
                    if (strGUBUN2 == "1")
                    {
                        if (dt.Rows[0]["GBPRE_1"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + "None ";
                        }
                        else
                        {
                            strInfo = strInfo + "";
                        }

                        if (dt.Rows[0]["GBCON_4"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + "Petihdine ";
                        }


                        if (dt.Rows[0]["GBCON_41"].ToString().Trim() != "")
                        {
                            strInfo = strInfo + dt.Rows[0]["GBCON_41"].ToString().Trim() + "mg " + dt.Rows[0]["GBCON_22"].ToString().Trim() + ", ";
                        }

                        if (dt.Rows[0]["GBPRE_3"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + " " + dt.Rows[0]["GBPRE_31"].ToString().Trim();
                        }
                        else
                        {
                            strInfo = strInfo + dt.Rows[0]["GBPRE_31"].ToString().Trim();
                        }

                        //'atropine
                        if (dt.Rows[0]["GBPRE_4"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + "Atropine ";
                        }

                        if (dt.Rows[0]["GBPRE_41"].ToString().Trim() != "")
                        {
                            strInfo = strInfo + dt.Rows[0]["GBPRE_41"].ToString().Trim() + "mg " + dt.Rows[0]["GBPRE_42"].ToString().Trim() + ", ";
                        }

                        strInfo = strInfo + ComNum.VBLF;

                        strInfo = strInfo + ComNum.VBLF + "◈ Conscious Sedation ◈" + ComNum.VBLF;

                        //'add2
                        if (dt.Rows[0]["MOAAS"].ToString().Trim() != "")
                        {
                            strInfo = strInfo + "MOAAS //Children`s Hospital of Wisconsin sedation scale " + dt.Rows[0]["MOAAS"].ToString().Trim() + ", " + ComNum.VBLF;
                        }

                        if (dt.Rows[0]["GBCON_1"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + "None ";
                        }

                        if (dt.Rows[0]["GBCON_2"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + "Mediazolam ";
                        }

                        if (dt.Rows[0]["GBCON_21"].ToString().Trim() != "")
                        {
                            strInfo = strInfo + dt.Rows[0]["GBCON_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBCON_22"].ToString().Trim() + ", ";
                        }

                        if (dt.Rows[0]["GBCON_3"].ToString().Trim() == "Y")
                        {
                            if (Convert.ToDateTime(dt.Rows[0]["RDATE"].ToString().Trim()) <= Convert.ToDateTime("2020-10-06"))
                            {
                                strInfo = strInfo + "Propofol ";
                            }
                            else
                            {
                                strInfo = strInfo + "Anepol ";
                            }

                            
                        }

                        if (dt.Rows[0]["GBCON_31"].ToString().Trim() != "")
                        {
                            strInfo = strInfo + dt.Rows[0]["GBCON_31"].ToString().Trim() + "mg " + dt.Rows[0]["GBCON_32"].ToString().Trim() + ", ";
                        }

                        //'remark
                        if (dt.Rows[0]["GBCON_5"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + " " + dt.Rows[0]["GBCON_51"].ToString().Trim();
                        }
                        else
                        {
                            strInfo = strInfo + dt.Rows[0]["GBCON_51"].ToString().Trim();
                        }

                        strInfo = strInfo + ComNum.VBLF;

                        strInfo = strInfo + ComNum.VBLF + "◈ Medication ◈" + ComNum.VBLF;

                        //'MED
                        if (dt.Rows[0]["GBMED_1"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + "None ";
                        }
                        else
                        {
                            strInfo = strInfo + "";
                        }

                        if (dt.Rows[0]["GBMED_2"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + "Epinephrine ";
                        }

                        if (dt.Rows[0]["GBMED_21"].ToString().Trim() != "")
                        {
                            strInfo = strInfo + dt.Rows[0]["GBMED_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBMED_22"].ToString().Trim() + ", ";
                        }

                        if (dt.Rows[0]["GBMED_3"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + "Botrooase ";
                        }

                        if (dt.Rows[0]["GBMED_31"].ToString().Trim() != "")
                        {
                            strInfo = strInfo + dt.Rows[0]["GBMED_31"].ToString().Trim() + "KU " + dt.Rows[0]["GBMED_32"].ToString().Trim() + ", ";
                        }

                        //'remark
                        if (dt.Rows[0]["GBMED_4"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + " " + dt.Rows[0]["GBMED_41"].ToString().Trim();
                        }
                        else
                        {
                            strInfo = strInfo + dt.Rows[0]["GBMED_4"].ToString().Trim();
                        }
                    }
                    else
                    {
                        if (dt.Rows[0]["GBPRE_1"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + "None ";
                        }
                        else
                        {
                            strInfo = strInfo + "";
                        }

                        if (dt.Rows[0]["GBPRE_2"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + "Aigiron ";
                        }

                        if (dt.Rows[0]["GBPRE_21"].ToString().Trim() != "")
                        {
                            strInfo = strInfo + dt.Rows[0]["GBPRE_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBPRE_22"].ToString().Trim() + ", ";
                        }

                        if (dt.Rows[0]["GBPRE_3"].ToString().Trim() != "")
                        {
                            strInfo = strInfo + " " + dt.Rows[0]["GBPRE_31"].ToString().Trim();
                        }
                        else
                        {
                            strInfo = strInfo + dt.Rows[0]["GBPRE_31"].ToString().Trim();
                        }

                        strInfo = strInfo + ComNum.VBLF;
                        strInfo = strInfo + ComNum.VBLF + "◈ Conscious Sedation ◈" + ComNum.VBLF;

                        //'add2
                        if (dt.Rows[0]["MOAAS"].ToString().Trim() != "")
                        {
                            strInfo = strInfo + "MOAAS /Children`s Hospital of Wisconsin sedation scale " + dt.Rows[0]["MOAAS"].ToString().Trim() + ", " + ComNum.VBLF;
                        }

                        if (dt.Rows[0]["GBCON_1"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + "None ";
                        }

                        if (dt.Rows[0]["GBCON_2"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + "Mediazolam ";
                        }

                        if (dt.Rows[0]["GBCON_21"].ToString().Trim() != "")
                        {
                            strInfo = strInfo + dt.Rows[0]["GBCON_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBCON_22"].ToString().Trim() + ", ";
                        }

                        if (dt.Rows[0]["GBCON_3"].ToString().Trim() == "Y")
                        {
                            if (Convert.ToDateTime(dt.Rows[0]["RDATE"].ToString().Trim()) >= Convert.ToDateTime("2020-10-07"))
                            {
                                strInfo = strInfo + "Anepol ";
                            }
                            else
                            {
                                strInfo = strInfo + "Propofol ";
                            }
                        }

                        if (dt.Rows[0]["GBCON_31"].ToString().Trim() != "")
                        {
                            strInfo = strInfo + dt.Rows[0]["GBCON_31"].ToString().Trim() + "mg " + dt.Rows[0]["GBCON_32"].ToString().Trim() + ", ";
                        }

                        if (dt.Rows[0]["GBCON_4"].ToString().Trim() == "Y")
                        {
                            strInfo = strInfo + "Pethidine ";
                        }

                        if (dt.Rows[0]["GBCON_41"].ToString().Trim() != "")
                        {
                            strInfo = strInfo + dt.Rows[0]["GBCON_41"].ToString().Trim() + "mg " + dt.Rows[0]["GBCON_42"].ToString().Trim() + ", ";
                        }
                        if (dt.Rows[0]["D_INTIME1"].ToString().Trim() != "")
                        {
                            strLowTime = "내시경 삽입시간:" + dt.Rows[0]["D_INTIME1"].ToString().Trim() + "분" + dt.Rows[0]["D_INTIME2"].ToString().Trim() + "초";
                            strLowTime = strLowTime + "  회수시간:" + dt.Rows[0]["D_EXTIME1"].ToString().Trim() + "분" + dt.Rows[0]["D_EXTIME2"].ToString().Trim() + "초";
                        }

                    }

                    strInfo = strInfo + ComNum.VBLF;

                    //'2015-07-23
                    strTPro = "";

                    if (dt.Rows[0]["PRO_BX1"].ToString().Trim() == "Y")
                        strTPro = strTPro + "Bx. bottle ";
                    if (dt.Rows[0]["PRO_BX2"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_BX2"].ToString().Trim() + "ea, ";

                    if (dt.Rows[0]["PRO_PP1"].ToString().Trim() == "Y")
                        strTPro = strTPro + "PP ";
                    if (dt.Rows[0]["PRO_PP2"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_PP2"].ToString().Trim() + "ea, ";

                    //2019-05-17 안정수, Rapid Urease Test 추가
                    if (dt.Rows[0]["PRO_RUT"].ToString().Trim() == "Y")
                        strTPro = strTPro + "Rapid Urease Test, ";

                    if (dt.Rows[0]["PRO_ESD1"].ToString().Trim() == "Y")
                        strTPro = strTPro + "ESD, ";
                    if (dt.Rows[0]["PRO_ESD2"].ToString().Trim() == "Y")
                        strTPro = strTPro + "en-bloc, ";
                    if (dt.Rows[0]["PRO_ESD3_1"].ToString().Trim() == "Y")
                        strTPro = strTPro + "piecemeal ";
                    if (dt.Rows[0]["PRO_ESD3_2"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_ESD3_2"].ToString().Trim() + ", " + ComNum.VBLF;

                    if (dt.Rows[0]["PRO_EMR1"].ToString().Trim() == "Y")
                        strTPro = strTPro + "EMR, ";
                    if (dt.Rows[0]["PRO_EMR2"].ToString().Trim() == "Y")
                        strTPro = strTPro + "en-bloc, ";
                    if (dt.Rows[0]["PRO_EMR3_1"].ToString().Trim() == "Y")
                        strTPro = strTPro + "piecemeal ";
                    if (dt.Rows[0]["PRO_EMR3_2"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_EMR3_2"].ToString().Trim() + ", " + ComNum.VBLF;

                    if (dt.Rows[0]["PRO_APC"].ToString().Trim() == "Y")
                        strTPro = strTPro + "APC, ";
                    if (dt.Rows[0]["PRO_ELEC"].ToString().Trim() == "Y")
                        strTPro = strTPro + "Electrocauterization, ";

                    if (dt.Rows[0]["PRO_HEMO1"].ToString().Trim() == "Y")
                        strTPro = strTPro + "Hemoclip ";
                    if (dt.Rows[0]["PRO_HEMO2"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_HEMO2"].ToString().Trim() + "ea, " + ComNum.VBLF;

                    if (dt.Rows[0]["PRO_EPNA1"].ToString().Trim() == "Y")
                        strTPro = strTPro + "EPNA ";
                    if (dt.Rows[0]["PRO_EPNA2"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_EPNA2"].ToString().Trim() + "cc, ";

                    if (dt.Rows[0]["PRO_MBAND"].ToString().Trim() == "Y")
                        strTPro = strTPro + "multi-band, " + ComNum.VBLF;

                    if (dt.Rows[0]["PRO_EST"].ToString().Trim() == "Y")
                        strTPro = strTPro + "EST (";
                    if (dt.Rows[0]["PRO_EST_STS"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_EST_STS"].ToString().Trim() + ") ";

                    if (strGUBUN2 == "3")
                    {
                        if (dt.Rows[0]["PRO_BAND1"].ToString().Trim() == "Y")
                            strTPro = strTPro + "band ";
                    }
                    else
                    {
                        if (dt.Rows[0]["PRO_BAND1"].ToString().Trim() == "Y")
                            strTPro = strTPro + "Single-band ";
                    }
                    if (dt.Rows[0]["PRO_BAND2"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_BAND2"].ToString().Trim() + "ea, ";

                    if (dt.Rows[0]["PRO_HIST1"].ToString().Trim() == "Y")
                        strTPro = strTPro + "Histoacyl ";
                    if (dt.Rows[0]["PRO_HIST2"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_HIST2"].ToString().Trim() + "ample, ";

                    if (dt.Rows[0]["PRO_DETA"].ToString().Trim() == "Y")
                        strTPro = strTPro + "Detachable snare, " + ComNum.VBLF;

                    if (dt.Rows[0]["PRO_BALL"].ToString().Trim() == "Y")
                        strTPro = strTPro + "Ballooon, ";
                    if (dt.Rows[0]["PRO_BASKET"].ToString().Trim() == "Y")
                        strTPro = strTPro + "Basket, " + ComNum.VBLF;
                     
                    if (dt.Rows[0]["PRO_EPBD1"].ToString().Trim() == "Y")
                        strTPro = strTPro + "EPBD ";
                    if (dt.Rows[0]["PRO_EPBD2"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_EPBD2"].ToString().Trim() + "mm ";
                    if (dt.Rows[0]["PRO_EPBD3"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_EPBD3"].ToString().Trim() + "atm ";
                    if (dt.Rows[0]["PRO_EPBD4"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_EPBD4"].ToString().Trim() + "sec" + ComNum.VBLF;

                    if (dt.Rows[0]["PRO_ENBD1"].ToString().Trim() == "Y")
                        strTPro = strTPro + "ENBD ";
                    if (dt.Rows[0]["PRO_ENBD2"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_ENBD2"].ToString().Trim() + "Fr.";
                    if (dt.Rows[0]["PRO_ENBD3"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_ENBD3"].ToString().Trim() + "type ";

                    if (dt.Rows[0]["PRO_ERBD1"].ToString().Trim() == "Y")
                        strTPro = strTPro + "ERBD ";
                    if (dt.Rows[0]["PRO_ERBD2"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_ERBD2"].ToString().Trim() + "Fr.";
                    if (dt.Rows[0]["PRO_ERBD3"].ToString().Trim() != "")
                        strTPro = strTPro + dt.Rows[0]["PRO_ERBD3"].ToString().Trim() + "type ";

                    //'바뀐서식
                    if (dt.Rows[0]["GBNEW"].ToString().Trim() == "Y")
                    {
                        strDate = "";
                        strDate = strDate + "◈==========================================================" + ComNum.VBLF;
                        strDate = strDate + "◈접수일자:  " + dt.Rows[0]["JDATE"].ToString().Trim() + " " + "◈검사일자:  " + dt.Rows[0]["RDATE"].ToString().Trim() + ComNum.VBLF;
                        strDate = strDate + "◈결과일자:  " + dt.Rows[0]["RESULTDATE"].ToString().Trim() + ComNum.VBLF;
                        strDate = strDate + "◈==========================================================";

                        strResult6_new = "";

                        switch (ss1_Sheet1.Cells[intRow, 5].Text.Trim())
                        {

                            case "1":
                                strResult6_new = dt.Rows[0]["REMARK6"].ToString().Trim();

                                strRemark = strRemark + "◈ Vocal Cord ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Carina ◈ " + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Bronchi ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;


                                if (strTPro != "")
                                {
                                    strRemark = strRemark + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + strTPro + ComNum.VBLF + dt.Rows[0]["REMARK3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                }
                                else
                                {
                                    strRemark = strRemark + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK4"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                }

                                strRemark = strRemark + "◈ Endoscopic Biopsy ◈" + ComNum.VBLF + ComNum.VBLF + strResult6_new + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + strInfo + ComNum.VBLF + ComNum.VBLF;  //'add
                                strRemark = strRemark + "◈ 처치의사 : " + strResultDrCode + "  " + strKorName;
                                break;

                            case "2": //'위

                                strResult6_new = "";
                                if (dt.Rows[0]["REMARK6"].ToString().Trim() != "")
                                {
                                    strResult6_new = "Esophagus:" + dt.Rows[0]["REMARK6"].ToString().Trim();
                                }

                                if (dt.Rows[0]["REMARK6_2"].ToString().Trim() != "")
                                {
                                    strResult6_new = strResult6_new + ComNum.VBLF + "Stomach:" + dt.Rows[0]["REMARK6_2"].ToString().Trim();
                                }

                                if (dt.Rows[0]["REMARK6_3"].ToString().Trim() != "")
                                {
                                    strResult6_new = strResult6_new + ComNum.VBLF + "Duodenum:" + dt.Rows[0]["REMARK6_3"].ToString().Trim();
                                }

                                strRemark = strRemark + "◈ Esophagus ◈" + ComNum.VBLF + dt.Rows[0]["REMARK1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Stomach ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Duodenum ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Endoscopic Diagnosis ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK4"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;

                                strRemark = strRemark + strInfo + ComNum.VBLF + ComNum.VBLF;  //'add

                                if (dt.Rows[0]["GBPRO_2"].ToString().Trim() == "Y")
                                {
                                    if (strTPro != "")
                                    {
                                        strRemark = strRemark + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + "CLO" + ComNum.VBLF + strTPro + ComNum.VBLF + dt.Rows[0]["REMARK5"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;                                        
                                    }
                                    else
                                    {
                                        strRemark = strRemark + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + "CLO" + ComNum.VBLF + dt.Rows[0]["REMARK5"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;                                        
                                    }
                                }
                                else
                                {
                                    if (strTPro != "")
                                    {
                                        strRemark = strRemark + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + strTPro + ComNum.VBLF + dt.Rows[0]["REMARK5"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                    }
                                    else
                                    {
                                        strRemark = strRemark + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK5"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                    }
                                }

                                strRemark = strRemark + "◈ Endoscopic Biopsy ◈" + ComNum.VBLF + ComNum.VBLF + strResult6_new + ComNum.VBLF + ComNum.VBLF;


                                //'참고사항
                                #region //2018-11-07 안정수, 내시경실 요청으로 궤양, 합병증, 응급환자, CVR 관련내용 추가표기 요청
                                if (dt.Rows[0]["GUBUN_GUE"].ToString().Trim() == "Y")
                                {
                                    strGubun_Gue = "궤양(+) ";
                                }
                                else if (dt.Rows[0]["GUBUN_GUE"].ToString().Trim() == "N")
                                {
                                    strGubun_Gue = "궤양(-) ";
                                }

                                if (dt.Rows[0]["GUBUN"].ToString().Trim() == "00")
                                {
                                    strGubun = "합병증(-) 응급검사(-) CVR(-)";
                                }
                                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "01") // 합병증
                                {
                                    strGubun = "합병증(+) 응급검사(-) CVR(-)";
                                }
                                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "02") // 응급환자
                                {
                                    strGubun = "합병증(-) 응급검사(+) CVR(-)";
                                }
                                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "03") // 합병+응급
                                {
                                    strGubun = "합병증(+) 응급검사(+) CVR(-)";
                                }
                                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "04") // CVR
                                {
                                    strGubun = "합병증(-) 응급검사(-) CVR(+)";
                                }
                                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "05") // 합병 + CVR
                                {
                                    strGubun = "합병증(+) 응급검사(-) CVR(+)";
                                }
                                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "06") // 응급 + CVR
                                {
                                    strGubun = "합병증(-) 응급검사(+) CVR(+)";
                                }
                                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "07") // 합병 + 응급 + CVR
                                {
                                    strGubun = "합병증(+) 응급검사(+) CVR(+)";
                                }
                                #endregion

                                if(strGubun != "" && strGubun_Gue != "")
                                {
                                    strRemark = strRemark + "◈ Remark ◈" + ComNum.VBLF + dt.Rows[0]["REMARK"].ToString().Trim() + ComNum.VBLF + strGubun_Gue + strGubun + ComNum.VBLF + ComNum.VBLF;
                                }
                                else
                                {
                                    strRemark = strRemark + "◈ Remark ◈" + ComNum.VBLF + dt.Rows[0]["REMARK"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                }
                                
                                strRemark = strRemark + "◈ 처치의사 : " + strResultDrCode + "  " + strKorName;

                                break;

                            case "3": //'대장

                                strResult6_new = "";

                                if (dt.Rows[0]["REMARK6"].ToString().Trim() != "")
                                {
                                    strResult6_new = "small Intestinal:" + dt.Rows[0]["REMARK6"].ToString().Trim();
                                }

                                if (dt.Rows[0]["REMARK6_2"].ToString().Trim() != "")
                                {
                                    strResult6_new = strResult6_new + ComNum.VBLF + "large Intestinal:" + dt.Rows[0]["REMARK6_2"].ToString().Trim();
                                }

                                if (dt.Rows[0]["REMARK6_3"].ToString().Trim() != "")
                                {
                                    strResult6_new = strResult6_new + ComNum.VBLF + "rectum:" + dt.Rows[0]["REMARK6_3"].ToString().Trim();
                                }

                                strRemark = strRemark + "◈ small Intestinal ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;


                                strRemark = strRemark + "◈ large Intestinal ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK4"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;


                                strRemark = strRemark + "◈ rectum ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK5"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;


                                strRemark = strRemark + "◈ Endoscopic Diagnosis ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;

                                if (strLowTime != "")
                                {
                                    strRemark = strRemark + "◈ 장정결도 ◈" + ComNum.VBLF + dt.Rows[0]["GB_CLEAN"].ToString().Trim() + ComNum.VBLF + strLowTime + ComNum.VBLF + ComNum.VBLF;   //'2013-06-17
                                }
                                else
                                {
                                    strRemark = strRemark + "◈ 장정결도 ◈" + ComNum.VBLF + dt.Rows[0]["GB_CLEAN"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;   //'2013-06-17
                                }

                                strRemark = strRemark + strInfo + ComNum.VBLF + ComNum.VBLF;  //'add

                                if (strTPro != "")
                                {
                                    strRemark = strRemark + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + strTPro + ComNum.VBLF + dt.Rows[0]["REMARK3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                }
                                else
                                {
                                    strRemark = strRemark + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                }
                                strRemark = strRemark + "◈ Endoscopic Biopsy ◈" + ComNum.VBLF + ComNum.VBLF + strResult6_new + ComNum.VBLF + ComNum.VBLF;

                                if (dt.Rows[0]["GUBUN"].ToString().Trim() == "00")
                                {
                                    strGubun = "합병증(-) 응급검사(-) CVR(-)";
                                }
                                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "01") // 합병증
                                {
                                    strGubun = "합병증(+) 응급검사(-) CVR(-)";
                                }
                                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "02") // 응급환자
                                {
                                    strGubun = "합병증(-) 응급검사(+) CVR(-)";
                                }
                                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "03") // 합병+응급
                                {
                                    strGubun = "합병증(+) 응급검사(+) CVR(-)";
                                }
                                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "04") // CVR
                                {
                                    strGubun = "합병증(-) 응급검사(-) CVR(+)";
                                }
                                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "05") // 합병 + CVR
                                {
                                    strGubun = "합병증(+) 응급검사(-) CVR(+)";
                                }
                                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "06") // 응급 + CVR
                                {
                                    strGubun = "합병증(-) 응급검사(+) CVR(+)";
                                }
                                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "07") // 합병 + 응급 + CVR
                                {
                                    strGubun = "합병증(+) 응급검사(+) CVR(+)";
                                }

                                //'참고사항
                                if (strGubun != "" )
                                {
                                    strRemark = strRemark + "◈ Remark ◈" + ComNum.VBLF + dt.Rows[0]["REMARK"].ToString().Trim() + ComNum.VBLF + strGubun + ComNum.VBLF + ComNum.VBLF;
                                }
                                else
                                {
                                    strRemark = strRemark + "◈ Remark ◈" + ComNum.VBLF + dt.Rows[0]["REMARK"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                }

                                strRemark = strRemark + "◈ 처치의사 : " + strResultDrCode + "  " + strKorName;

                                break;

                            case "4":
                                strResult6_new = "";
                                //2019-10-08 안정수 추가  
                                if (dt.Rows[0]["REMARK6"].ToString().Trim() != "")
                                {
                                    strResult6_new = dt.Rows[0]["REMARK6"].ToString().Trim();
                                }

                                strRemark = strRemark + "◈ ERCP Finding ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Disgnosis ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Paln & Tx ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;

                                strRemark = strRemark + strInfo + ComNum.VBLF + ComNum.VBLF;  //'add

                                if (strTPro != "")
                                {
                                    strRemark = strRemark + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + strTPro + ComNum.VBLF + dt.Rows[0]["REMARK4"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                }
                                else
                                {
                                    strRemark = strRemark + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK4"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                }

                                strRemark = strRemark + "◈ Endoscopic Biopsy ◈" + ComNum.VBLF + ComNum.VBLF + strResult6_new + ComNum.VBLF + ComNum.VBLF;


                                strRemark = strRemark + "◈ 처치의사 : " + strResultDrCode + "  " + strKorName;

                                break;
                        }
                    }
                    else
                    {

                        switch (ss1_Sheet1.Cells[intRow, 5].Text.Trim())
                        {
                            case "1":
                                strRemark = strRemark + "◈ Vocal Cord ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK1"].ToString().Trim() + ComNum.VBLF;
                                strRemark = strRemark + "◈ Carina ◈ " + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK2"].ToString().Trim() + ComNum.VBLF;
                                strRemark = strRemark + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK3"].ToString().Trim() + ComNum.VBLF;
                                strRemark = strRemark + "◈ Endoscopic Biopsy ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK6"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ 처치의사 : " + strResultDrCode + "  " + strKorName;
                                break;

                            case "2": //'위
                                strRemark = strRemark + "◈ Esophagus ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Stomach ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Duodenum ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Endoscopic Diagnosis ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK4"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK5"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Endoscopic Biopsy ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK6"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ 처치의사 : " + strResultDrCode + "  " + strKorName;
                                break;

                            case "3": //'대장
                                strRemark = strRemark + "◈ Endoscopic Findings ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Endoscopic Diagnosis ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Endoscopic Biopsy ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK6"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ 처치의사 : " + strResultDrCode + "  " + strKorName;
                                break;

                            case "4":
                                strRemark = strRemark + "◈ ERCP Finding ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Disgnosis ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Paln + Tx ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK4"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ Endoscopic Biopsy ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["REMARK6"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                                strRemark = strRemark + "◈ 처치의사 : " + strResultDrCode + "  " + strKorName;
                                break;
                        }
                    }

                    if (dt.Rows[0]["GBNEW"].ToString().Trim() == "Y")
                    {
                        txtResult1.Text = ComNum.VBLF + strRemark + ComNum.VBLF;
                    }
                    else
                    {
                        txtResult1.Text = ComNum.VBLF + ComNum.VBLF + strRemark + ComNum.VBLF;
                    }
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void SelectXray(int intWrtno)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT XDRCODE1,RESULT,RESULT1,APPROVE , ADDENDUM1, ADDENDUM2, TEMP, READTIME, ADDDATE, (SELECT DRNAME FROM ADMIN.OCS_DOCTOR WHERE SABUN = XDRCODE1) AS DRGUBUN ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.XRAY_RESULTNEW ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + Convert.ToString(intWrtno) + " ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["APPROVE"].ToString().Trim() == "N")
                    {
                        txtResult.Text = ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                        txtResult.Text = txtResult.Text + "      ▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                    }
                    else if (dt.Rows[0]["TEMP"].ToString().Trim() == "Y")
                    {
                        txtResult.Text = ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                        txtResult.Text = txtResult.Text + "      ▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(dt.Rows[0]["DRGUBUN"].ToString().Trim()) == true && int.Parse(dt.Rows[0]["XDRCODE1"].ToString().Trim()) < 99001)
                        {
                            txtResult.Text = dt.Rows[0]["RESULT"].ToString().Trim() + dt.Rows[0]["RESULT1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF + "입력일자 : " + dt.Rows[0]["READTIME"].ToString().Trim() + "         입력자 : " + CF.Read_SabunName(clsDB.DbCon, dt.Rows[0]["XDrCode1"].ToString().Trim());
                            if (string.IsNullOrEmpty(dt.Rows[0]["ADDDATE"].ToString().Trim()) == false)
                            {
                                txtAddendum.Text = dt.Rows[0]["ADDENDUM1"].ToString().Trim() + dt.Rows[0]["ADDENDUM2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF + "입력일자 : " + dt.Rows[0]["ADDDATE"].ToString().Trim() + "         입력자 : " + CF.Read_SabunName(clsDB.DbCon, dt.Rows[0]["XDrCode1"].ToString().Trim());
                            }
                            else
                            {
                                txtAddendum.Text = dt.Rows[0]["ADDENDUM1"].ToString().Trim() + dt.Rows[0]["ADDENDUM2"].ToString().Trim();
                            }
                        }
                        else
                        {
                          
                            txtResult.Text = dt.Rows[0]["RESULT"].ToString().Trim() + dt.Rows[0]["RESULT1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF + "판독일자 : " + dt.Rows[0]["READTIME"].ToString().Trim() + "         판독의사 : " + CF.Read_SabunName(clsDB.DbCon, dt.Rows[0]["XDrCode1"].ToString().Trim());
                            if (string.IsNullOrEmpty(dt.Rows[0]["ADDDATE"].ToString().Trim()) == false)
                            {
                                txtAddendum.Text = dt.Rows[0]["ADDENDUM1"].ToString().Trim() + dt.Rows[0]["ADDENDUM2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF + "추가판독일자 : " + dt.Rows[0]["ADDDATE"].ToString().Trim() + "         판독의사 : " + CF.Read_SabunName(clsDB.DbCon, dt.Rows[0]["XDrCode1"].ToString().Trim());
                            }
                            else
                            {
                                txtAddendum.Text = dt.Rows[0]["ADDENDUM1"].ToString().Trim() + dt.Rows[0]["ADDENDUM2"].ToString().Trim();
                            }
                        }
                    }
                }

                mstrDrCode = "";
                mstrDrCode = dt.Rows[0]["XDRCODE1"].ToString().Trim();

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void Select_Xray_HIC(string strROWID) //'건진 판독결과
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                //'해당 판독번호로 판독결과를 READ
                SQL = "";
                SQL = "SELECT PANO,SNAME,TO_CHAR(READTIME1,'YYYY-MM-DD') READDATE,READTIME1,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE,GBREAD,";
                SQL = SQL + ComNum.VBLF + " SEX,AGE,READDOCT1,READDOCT2,GBSTS, ";
                SQL = SQL + ComNum.VBLF + " XCODE,RESULT1,RESULT2,RESULT3,RESULT4,RESULT1_1,RESULT2_1,RESULT3_1,RESULT4_1,(SELECT DRNAME FROM ADMIN.OCS_DOCTOR WHERE SABUN = READDOCT1) AS DRGUBUN ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.HIC_XRAY_RESULT ";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID ='" + strROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("판독결과가 없습니다.", "오류");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["RESULT1"].ToString().Trim() == "")
                    {
                        txtResult.Text = ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                        txtResult.Text = txtResult.Text + "      ▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                    }
                    else
                    {
                        txtResult.Text = "판독분류: " + dt.Rows[0]["RESULT2"].ToString().Trim() + ComNum.VBLF;
                        txtResult.Text = txtResult.Text + "판독분류명: " + dt.Rows[0]["RESULT3"].ToString().Trim() + ComNum.VBLF;
                        txtResult.Text = txtResult.Text + "판독소견: " + dt.Rows[0]["RESULT4"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;

                        //'분진일경우 (판독이 2개)
                        if (dt.Rows[0]["GBREAD"].ToString().Trim() == "2")
                        {
                            txtResult.Text = txtResult.Text + "판독분류: " + dt.Rows[0]["RESULT2_1"].ToString().Trim() + ComNum.VBLF;
                            txtResult.Text = txtResult.Text + "판독분류명: " + dt.Rows[0]["RESULT3_1"].ToString().Trim() + ComNum.VBLF;
                            txtResult.Text = txtResult.Text + "판독소견: " + dt.Rows[0]["RESULT4_1"].ToString().Trim() + ComNum.VBLF;
                        }
                        
                    }

                    //2019-12-10 안정수 추가, 폐암판독CT 관련 결과 
                    if(dt.Rows[0]["XCODE"].ToString().Trim() == "TY10")
                    {
                        txtResult.Text = ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                        txtResult.Text = txtResult.Text + "판독소견: " + dt.Rows[0]["RESULT4"].ToString().Trim() + ComNum.VBLF;
                    }
                    if((string.IsNullOrEmpty(dt.Rows[0]["DRGUBUN"].ToString().Trim()) == true) && int.Parse(dt.Rows[0]["READDOCT1"].ToString().Trim()) < 99001)
                    {
                        txtResult.Text = txtResult.Text + ComNum.VBLF + ComNum.VBLF + "입력일자 : " + dt.Rows[0]["READTIME1"].ToString().Trim() + "         입력자 : " + CF.Read_SabunName(clsDB.DbCon, dt.Rows[0]["READDOCT1"].ToString().Trim());
                    }
                    else
                    {
                        txtResult.Text = txtResult.Text + ComNum.VBLF + ComNum.VBLF + "판독일자 : " + dt.Rows[0]["READTIME1"].ToString().Trim() + "         판독의사 : " + CF.Read_SabunName(clsDB.DbCon, dt.Rows[0]["READDOCT1"].ToString().Trim());
                    }
                    
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void SelectXrayDR(int intWrtno)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT XDRCODE1,RESULT,RESULT1,APPROVE FROM ADMIN.XRAY_RESULTNEW_DR ";
                SQL = SQL + ComNum.VBLF + " WHERE DRWRTNO = " + Convert.ToString(intWrtno) + " ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["APPROVE"].ToString().Trim() == "N")
                    {
                        txtResult.Text = ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                        txtResult.Text = txtResult.Text + "      ▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                    }
                    else
                    {
                        txtResult.Text = dt.Rows[0]["RESULT"].ToString().Trim() + dt.Rows[0]["RESULT1"].ToString().Trim();
                    }
                }

                mstrDrCode = "";
                mstrDrCode = dt.Rows[0]["XDRCODE1"].ToString().Trim();

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnTdmPrint_Click(object sender, EventArgs e)
        {
            //string strFont1 = "";
            //string strFont2 = "";
            //string strFont3 = "";
            //string strHead = "";

            //int i = 0;
            //TODO : 스프레드 출력 영록

            //ss1_Sheet1.Rows.Count = 

            //.MaxRows = SS1.DataRowCnt

            //strFont1 = "/l/fn""굴림체""/fz""20"""
            //strFont2 = "/l"
            //strFont3 = "/n/fn""굴림체""/fb0/fu0/fz""11"""

            //strHead = "( " & TxtPtno.Text & ":" & PanSName.Caption & ")"

            //ssTDM.PrintHeader = strFont1 & "/n" & strHead & " "

            //ssTDM.PrintMarginLeft = 1000
            //ssTDM.PrintMarginRight = 1000
            //ssTDM.PrintMarginTop = 600
            //ssTDM.PrintMarginBottom = 600
            //ssTDM.PrintColHeaders = True
            //ssTDM.PrintRowHeaders = False

            //ssTDM.PrintBorder = True
            //ssTDM.PrintColor = True
            //ssTDM.PrintGrid = True
            //ssTDM.PrintShadows = True
            //ssTDM.PrintUseDataMax = False
            //ssTDM.PrintOrientation = 1
            //ssTDM.PrintType = SS_PRINT_ALL

            //ssTDM.Action = SS_ACTION_SMARTPRINT

            //Call SQL_LOG("", ssTDM.PrintHeader)
        }

        private void btnDrugReturn01_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            SetPAN(panPConsult);
            GetPConsult(txtPtNo.Text.Trim());

            lblTitleSub11.Text = ((Button)sender).Text.Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="SET_PAN"/>
        /// <param name="panPConsult"></param>
        private void SetPAN(Panel argPanel)
        {
            panXray.Visible = false;
            panDiscern.Visible = false;
            panConsult.Visible = false;
            panEtc.Visible = false;
            panExam3.Visible = false;
            panExam.Visible = false;
            panPFT.Visible = false;
            panDem.Visible = false;
            panEMG.Visible = false;
            panEtcJupmst.Visible = false;
            panECG.Visible = false;
            panEEG.Visible = false;
            panNST.Visible = false;
            panPConsult.Visible = false;
            panTDM.Visible = false;
            panCVR.Visible = false;
            panEtcCVR.Visible = false; //'2015-07-01
            picPicturePic.Visible = false; //'2015-07-01
            panSixWalk.Visible = false;

            argPanel.Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="readPConsult"/>
        /// <param name="strPtNo"></param>
        private void GetPConsult(string strPtNo)
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssPConsult_Sheet1.RowCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT PROGRESS, TO_CHAR(A.WRITEDATE,'YYYY-MM-DD') WDATE, A.WRITESABUN, ORDERCODE, A.SEQNO, IPDNO, USED, B.ROWID ROWID2, A.RETURN_TEXT, A.RETURN_TEXT2, A.BIGO ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.DRUG_PCONSULT A, ADMIN.DRUG_PCONSULT_RETURN B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.PROGRESS >= '0' ";
                SQL = SQL + ComNum.VBLF + "   AND A.DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND A.SEQNO = B.SEQNO(+)";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.WRITEDATE ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssPConsult_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssPConsult_Sheet1.Cells[i, 0].Text = codeProgress(dt.Rows[i]["PROGRESS"].ToString().Trim());
                        ssPConsult_Sheet1.Cells[i, 1].Text = dt.Rows[i]["WDATE"].ToString().Trim();
                        ssPConsult_Sheet1.Cells[i, 2].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["WRITESABUN"].ToString().Trim());

                        SQL = "";
                        SQL = " SELECT ROWID FROM ADMIN.INSA_MST ";
                        SQL = SQL + ComNum.VBLF + " WHERE BUSE IN ('044101','044100') ";
                        SQL = SQL + ComNum.VBLF + "   AND SABUN = '" + ComFunc.LPAD(dt.Rows[i]["WRITESABUN"].ToString().Trim(), 5, "0") + "' ";
                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssPConsult_Sheet1.Cells[i, 2].Text = "";
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (dt.Rows[i]["BIGO"].ToString().Trim().IndexOf("자동발생") != -1)
                        {
                            ssPConsult_Sheet1.Cells[i, 2].Text = "약제팀";
                        }

                        ssPConsult_Sheet1.Cells[i, 10].Text = SetBun(dt.Rows[i]["ORDERCODE"].ToString().Trim());
                        ssPConsult_Sheet1.Cells[i, 11].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                        ssPConsult_Sheet1.Cells[i, 12].Text = (dt.Rows[i]["USED"].ToString().Trim() == "0" ? "최초" : "있음");
                        ssPConsult_Sheet1.Cells[i, 13].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssPConsult_Sheet1.Cells[i, 14].Text = dt.Rows[i]["ROWID2"].ToString().Trim();
                        ssPConsult_Sheet1.Cells[i, 15].Text = dt.Rows[i]["RETURN_TEXT"].ToString().Trim();
                        SetPatInfoPConsult(dt.Rows[i]["IPDNO"].ToString().Trim(), i);
                    }
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string codeProgress(string strGubun)
        {
            string rtnVal = "";

            switch (strGubun)
            {
                case "1": rtnVal = ""; break;
                case "2": rtnVal = "진행중"; break;
                case "3": rtnVal = "취소"; break;
                case "C": rtnVal = "완료"; break;
            }

            return rtnVal;
        }
        private void SetPatInfoPConsult(string strIpdNo, int intRow)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT WARDCODE, ROOMCODE, PANO, SNAME, AGE, SEX, DEPTCODE";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + strIpdNo;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssPConsult_Sheet1.Cells[intRow, 3].Text = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    ssPConsult_Sheet1.Cells[intRow, 4].Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    ssPConsult_Sheet1.Cells[intRow, 5].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ssPConsult_Sheet1.Cells[intRow, 6].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssPConsult_Sheet1.Cells[intRow, 7].Text = dt.Rows[0]["AGE"].ToString().Trim();
                    ssPConsult_Sheet1.Cells[intRow, 8].Text = dt.Rows[0]["SEX"].ToString().Trim();
                    ssPConsult_Sheet1.Cells[intRow, 9].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string SetBun(string strCode)
        {
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = " SELECT COMMENTS";
                SQL = SQL + ComNum.VBLF + "  From ADMIN.DRUG_SETCODE";
                SQL = SQL + ComNum.VBLF + "  WHERE PART = (";
                SQL = SQL + ComNum.VBLF + "  SELECT PART FROM ADMIN.DRUG_SETCODE";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = '11'";
                SQL = SQL + ComNum.VBLF + "  AND JEPCODE = '" + strCode + "')";
                SQL = SQL + ComNum.VBLF + "  AND JEPCODE = '분류명칭'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["COMMENTS"].ToString().Trim();
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return strVal;
        }

        private void btnJusa_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text == "")
                return;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int i = 0;

            string strDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            ss2_Sheet1.Rows.Count = 20;

            ss2_Sheet1.Cells[0, 0, ss2_Sheet1.RowCount - 1, ss2_Sheet1.ColumnCount - 1].Text = "";

            txtJusaMemo.Visible = true;

            panDem.Visible = true;
            panEMG.Visible = false;
            panXray.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnWordEntry.Visible = false;
            panPFT.Visible = false;

            ssXrayDr.Visible = false;

            txtResult1.Visible = false;
            ss1.Visible = false;

            panExam.Visible = false;
            lblTitleSub0.Text = ""; //PanXrayJong.Caption = "";

            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = false;
            panExam3.Visible = false;
            nIndex = 1;

            SetPAN(panDem);

            ssDEM_Sheet1.ColumnHeader.Columns[1].Label = "주사일자";
            ssDEM_Sheet1.ColumnHeader.Columns[1].Width = 130;
            ssDEM_Sheet1.ColumnHeader.Columns[8].Label = "참고사항";
            ssDEM_Sheet1.ColumnHeader.Columns[8].Width = 130;
            ssDEM_Sheet1.ColumnHeader.Columns[9].Label = "오더명칭";

            ssDEM_Sheet1.Columns.Get(3).Visible = false;
            ssDEM_Sheet1.Columns.Get(4).Visible = false;
            ssDEM_Sheet1.Columns.Get(7).Visible = false;

            txtEmgResult.Text = "";

            //'자료를 찾기
            try
            {
                ssDEM_Sheet1.RowCount = 0;

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT A.PTNO,B.SNAME, B.SEX, A.DEPTCODE, A.DRCODE, A.ORDERCODE,A.SUCODE,A.REMARK,B.JUSAMSG, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.ACTDATE2,'YYYY-MM-DD HH24:MI') ACTDATE2,TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.ETC_JUSASUB A,ADMIN.BAS_PATIENT B";
                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO=B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO='" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE<=TO_DATE('" + ComFunc.FormatStrToDate(strDate, "D") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.BDATE DESC,A.ACTDATE2 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssDEM_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        //'참고사항표시
                        if (i == 0)
                        {
                            if (dt.Rows[i]["JUSAMSG"].ToString().Trim() != "")
                            {
                                txtJusaMemo.Text = "주사메모 : " + dt.Rows[i]["JUSAMSG"].ToString().Trim();
                            }
                        }

                        ssDEM_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssDEM_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ACTDATE2"].ToString().Trim();
                        ssDEM_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();

                        ssDEM_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssDEM_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRCODE"].ToString().Trim();

                        ssDEM_Sheet1.Cells[i, 8].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssDEM_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim() + "[" + GetOrderName(dt.Rows[i]["ORDERCODE"].ToString().Trim()) + "]";
                    }
                }

                dt.Dispose();
                dt = null;

                lblTitleSub11.Text = ((Button)sender).Text.Trim();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void btnTDM_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            SetPAN(panTDM);

            GetTDMList(txtPtNo.Text.Trim());

            lblTitleSub11.Text = ((Button)sender).Text.Trim();
        }

        private void GetTDMList(string strPtNo)
        {

            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            ssTDMList_Sheet1.RowCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, PANO, SNAME, DRUGYAK1, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.DRUG_TDM ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SDATE, PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssTDMList_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssTDMList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                        ssTDMList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssTDMList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssTDMList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssTDMList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DRUGYAK1"].ToString().Trim();
                    }
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void chkResult_CheckedChanged(object sender, EventArgs e)
        {
            btnRView.PerformClick();
        }

        private void btnRView_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            GetExamSpecmst();
        }

        private void btnBiopsy_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text.Trim() == "")
                return;

            int i = 0;

            string strHicPano = "";
            string strHeaPano = "";
            string strJumin = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;

            ss1_Sheet1.ColumnHeader.Columns[8].Label = "Anatno";

            ss1_Sheet1.Columns.Get(4).Visible = true;
            ss1_Sheet1.Columns.Get(9).Visible = true;
            ss1.ActiveSheet.Columns[9].Locked = true;
            ss1_Sheet1.Columns.Get(10).Visible = false;
            ss1_Sheet1.ColumnHeader.Columns[9].Label = "접수여부";

            txtJusaMemo.Visible = false;

            panEtc.Visible = true;
            panEMG.Visible = false;

            panXray.Visible = false;
            ss1.Visible = true;
            txtResult1.Visible = true;
            panExam.Visible = false;
            panPFT.Visible = false;
            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = false;
            panExam3.Visible = false;
            nIndex = 3;

            SetPAN(panEtc);

            txtResult1.Text = "";

            ss1_Sheet1.RowCount = 0;

            //ss1_Sheet1.Cells[0, 0, ss1_Sheet1.RowCount - 1, ss1_Sheet1.ColumnCount - 1].Text = "";

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.GBIO,A.DEPTCODE, B.ORDERNAME,A.ANATNO, ";
                SQL = SQL + ComNum.VBLF + "A.CREMARK1,A.RESULT_IMG,  A.ROWID, A.GBJOB ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.EXAM_ANATMST A, ADMIN.OCS_ORDERCODE B ,ADMIN.EXAM_SPECMST C  ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PTNO     = '" + txtPtNo.Text.Trim() + "' ";

                SQL = SQL + ComNum.VBLF + "   AND A.SPECNO =C.SPECNO ";
                SQL = SQL + ComNum.VBLF + "   AND A.ORDERCODE = B.ORDERCODE       ";

                //'2013-06-10
                SQL = SQL + ComNum.VBLF + "   AND (SUBSTR(A.ANATNO,1,1) = 'S' OR SUBSTR(A.ANATNO,1,2) = 'OS' OR SUBSTR(A.ANATNO,1,2) = 'IH' )        ";
                //SQL = SQL + ComNum.VBLF + "   AND A.GBJOB ='V'";  //'JJY(2005-06-17) 해부병리과장님요
                //2019-07-22 안정수, 호흡기전담 요청으로 조건 변경 
                SQL = SQL + ComNum.VBLF + "   AND A.GBJOB IS NOT NULL";  
                SQL = SQL + ComNum.VBLF + " ORDER BY A.BDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    ss1_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["RESULT_IMG"].ToString().Trim() == "Y" ? "▦" : "");
                        ss1_Sheet1.Cells[i, 7].Text = (dt.Rows[i]["CREMARK1"].ToString().Trim() != "" ? "◎" : "");   //'2015-07-01
                        ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ANATNO"].ToString().Trim();
                        switch (dt.Rows[i]["GBJOB"].ToString().Trim())
                        {
                            case "":
                                ss1_Sheet1.Cells[i, 9].Text = "미접수";
                                break;
                            case "N":
                                ss1_Sheet1.Cells[i, 9].Text = "접수";
                                break;
                            case "Y":
                                ss1_Sheet1.Cells[i, 9].Text = "검사중";
                                break;
                            case "V":
                                ss1_Sheet1.Cells[i, 9].Text = "완료";
                                break;
                        }
                        
                    }
                }

                dt.Dispose();
                dt = null;


                //'등록번호 찾기
                strJumin = clsAES.Read_Jumin_AES(clsDB.DbCon, txtPtNo.Text.Trim());

                //'등록번호로 일반건진 접수번호를 찾음(일반건진은 SPECMST -> 일반건진 접수번호로 전송)
                SQL = "";
                SQL = "SELECT WRTNO FROM ADMIN.HIC_JEPSU ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO IN (SELECT PANO FROM ADMIN.HIC_PATIENT ";
                SQL = SQL + ComNum.VBLF + "      WHERE JUMIN2= '" + clsAES.AES(strJumin) + "') ";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strHicPano = "";
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strHicPano = strHicPano + "'" + ComFunc.LPAD(dt.Rows[i]["WRTNO"].ToString().Trim(), 8, "0") + "',";
                    }

                    if (strHicPano != "")
                        strHicPano = VB.Left(strHicPano, strHicPano.Length - 1);
                }

                dt.Dispose();
                dt = null;

                //'종합건진 등록번호 찾음(종검은 SPECTMST -> 종검접수번호로 전송)
                SQL = "";
                SQL = "SELECT WRTNO FROM ADMIN.HEA_JEPSU ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO IN ( SELECT PANO FROM ADMIN.HEA_PATIENT ";
                SQL = SQL + ComNum.VBLF + "      WHERE JUMIN2 = '" + clsAES.AES(strJumin) + "')  ";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                strHeaPano = "";

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strHeaPano = strHeaPano + "'" + ComFunc.LPAD(dt.Rows[i]["WRTNO"].ToString().Trim(), 8, "0") + "',";
                    }

                    if (strHeaPano != "")
                        strHeaPano = VB.Left(strHeaPano, strHeaPano.Length - 1);
                }

                dt.Dispose();
                dt = null;

                if (strHeaPano != "")
                {
                    //'종검(건진은 외래번호로 접수됨)

                    SQL = " SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.GBIO, B.ORDERNAME,A.ANATNO, A.ROWID,A.CREMARK1, A.GBJOB ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.EXAM_ANATMST A, ADMIN.OCS_ORDERCODE B ,ADMIN.EXAM_SPECMST C  ";
                    SQL = SQL + ComNum.VBLF + " WHERE A.PTNO IN (" + strHeaPano + ")  ";
                    SQL = SQL + ComNum.VBLF + "   AND A.SPECNO =C.SPECNO ";
                    SQL = SQL + ComNum.VBLF + "   AND A.ORDERCODE = B.ORDERCODE       ";
                    SQL = SQL + ComNum.VBLF + "   AND C.BI IN ('61') ";
                    SQL = SQL + ComNum.VBLF + "   AND SUBSTR(A.ANATNO,1,1) = 'S'        ";
                    //SQL = SQL + ComNum.VBLF + "   AND A.GBJOB ='V'"; //'JJY(2005-06-17) 해부병리과장님요;
                    //2019-07-22 안정수, 호흡기전담 요청으로 조건 변경 
                    SQL = SQL + ComNum.VBLF + "   AND A.GBJOB IS NOT NULL";
                    SQL = SQL + ComNum.VBLF + "   AND (B.SENDDEPT <> 'N' OR B.SENDDEPT IS NULL) ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.BDATE DESC ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ss1_Sheet1.RowCount = ss1_Sheet1.RowCount + 1;

                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 2].Text = (dt.Rows[i]["GBIO"].ToString().Trim() == "0" ? "외래" : "입원");
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 7].Text = (dt.Rows[i]["CREMARK1"].ToString().Trim() != "" ? "◎" : ""); //'2015-07-01;
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["ANATNO"].ToString().Trim();

                            switch (dt.Rows[i]["GBJOB"].ToString().Trim())
                            {
                                case "":
                                    ss1_Sheet1.Cells[i, 9].Text = "미접수";
                                    break;
                                case "N":
                                    ss1_Sheet1.Cells[i, 9].Text = "접수";
                                    break;
                                case "Y":
                                    ss1_Sheet1.Cells[i, 9].Text = "검사중";
                                    break;
                                case "V":
                                    ss1_Sheet1.Cells[i, 9].Text = "완료";
                                    break;
                                default:
                                    ss1_Sheet1.Cells[i, 9].Text = "";
                                    break;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;

                }
                lblTitleSub11.Text = ((Button)sender).Text.Trim();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnXray.PerformClick();
        }

        private void btnXray_Click(object sender, EventArgs e)
        {
            int i = 0;
            int nPACS_Service = 0;
            string strXJong = "";
            string strXCode = "";
            string strXName = "";
            string strYear1 = "";
            string strYear2 = "";
            bool BnColor = false;

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            clsPublic.GnLogOutCNT = 0;

            //'0.정상 1.PACS고장 2.웹서버고장 3.PACS 및 웹서버고장
            nPACS_Service = GetPACSService();

            ss2_Sheet1.RowCount = 0;

            txtJusaMemo.Visible = false;

            panXray.Visible = true;
            panEMG.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnWordEntry.Visible = false;
            panPFT.Visible = false;
            panExam3.Visible = false;

            ssXrayDr.Visible = false;

            txtResult1.Visible = false;
            ss1.Visible = false;

            panExam.Visible = false;

            lblTitleSub0.Text = "";

            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = false;

            SetPAN(panXray);

            nIndex = 1;

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                //'자료를 찾기;
                SQL = "";
                SQL = "SELECT A.PANO,B.SNAME,TO_CHAR(A.SEEKDATE,'YYYY-MM-DD HH24:MI') SEEKDATE,A.DEPTCODE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.DRDATE,'YYYY-MM-DD') DRDATE, A.DRWRTNO, ";
                SQL = SQL + ComNum.VBLF + " A.DRCODE,C.DRNAME,D.XNAME,A.EXINFO,A.PACSNO,A.XJONG,A.XCODE,A.XSUBCODE,A.IPDOPD,A.ORDERNO,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.ENTERDATE,'MM/DD HH24:MI') ORDERDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.SEEKDATE,'MM/DD HH24:MI') JEPSUTIME,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.XSENDDATE,'MM/DD HH24:MI') XSENDDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.BDATE,'YYYY/MM/DD') BDATE,";
                SQL = SQL + ComNum.VBLF + " A.PACSSTUDYID,A.ORDERNAME,A.REMARK,A.ROWID, GBPACS ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.XRAY_DETAIL A,ADMIN.BAS_PATIENT B,ADMIN.BAS_DOCTOR C,";
                SQL = SQL + ComNum.VBLF + "      ADMIN.XRAY_CODE D ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PANO='" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.GBRESERVED >='6' "; //'접수 또는 촬영완료;
                //SQL = SQL + ComNum.VBLF + "  AND A.XJONG NOT IN ('E') "; // '근전도 별도로 조회;
                //2018-12-06 안정수, 류마티스 박채희s 요청으로 현미경검사 보이도록 보완 
                SQL = SQL + ComNum.VBLF + "  AND (A.XJONG NOT IN ('E') OR A.XCODE = 'E7190')"; // '근전도 별도로 조회;
                SQL = SQL + ComNum.VBLF + "  AND (A.GBHIC IS NULL OR A.GBHIC <> 'Y') ";

                if (mstrXRayDate != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND A.SEEKDATE>=TO_DATE('" + dtpSdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND A.SEEKDATE<=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.SEEKDATE<=TRUNC(SYSDATE+1) ";
                }

                if (mstrXRayDept != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND A.DEPTCODE='" + mstrXRayDept + "' ";
                }

                SQL = SQL + ComNum.VBLF + "  AND A.PANO=B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "  AND A.DRCODE=C.DRCODE(+) ";
                SQL = SQL + ComNum.VBLF + "  AND A.XCODE=D.XCODE(+) ";

                //'--------( 일반건진 )------------;

                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT A.PTNO PANO,B.SNAME,TO_CHAR(A.JEPDATE,'YYYY-MM-DD') SEEKDATE,";
                SQL = SQL + ComNum.VBLF + " 'HR' DEPTCODE,'' DRDATE,0 DRWRTNO, ";
                SQL = SQL + ComNum.VBLF + " '7101' DRCODE,'건진' DRNAME,D.HNAME XNAME, A.READDOCT1 EXINFO,A.XRAYNO PACSNO,'1' XJONG,";
                SQL = SQL + ComNum.VBLF + " A.XCODE,'' XSUBCODE,'O' IPDOPD,0 ORDERNO,";
                SQL = SQL + ComNum.VBLF + " '' ORDERDATE,'' JEPSUTIME,'' XSENDDATE, '' BDATE, ";
                SQL = SQL + ComNum.VBLF + " A.XRAYNO PACSSTUDYID,D.HNAME ORDERNAME,'' REMARK,A.ROWID, A.GBPACS ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.HIC_XRAY_RESULT A,ADMIN.BAS_PATIENT B, ";
                SQL = SQL + ComNum.VBLF + "      ADMIN.HIC_EXCODE D ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PTNO='" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.DELDATE IS NULL ";

                if (mstrXRayDate != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND A.JEPDATE>=TO_DATE('" + dtpSdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND A.JEPDATE<=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.JEPDATE<=TRUNC(SYSDATE+1) ";
                }

                if (mstrXRayDept != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND A.PTNO='ZZ' ";
                }

                SQL = SQL + ComNum.VBLF + "  AND A.PTNO=B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "  AND A.XCODE=D.CODE(+) ";

                SQL = SQL + ComNum.VBLF + "ORDER BY 3 DESC,1 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                } 

                if (dt.Rows.Count > 0)
                {
                    ss2_Sheet1.RowCount = dt.Rows.Count;
                    methodSpd.setSpdSort(ss2, 1, true);
                    methodSpd.setSpdSort(ss2, 6, true);
                    methodSpd.setSpdSort(ss2, 10, true);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strXName = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        strXJong = dt.Rows[i]["XJONG"].ToString().Trim();
                        strXCode = dt.Rows[i]["XCODE"].ToString().Trim();

                        if (strXName == "")
                            strXName = dt.Rows[i]["XNAME"].ToString().Trim();
                        strXName = strXName + " " + dt.Rows[i]["REMARK"].ToString().Trim();
                    
                        ss2_Sheet1.Cells[i, 0].Text = "";
                        ss2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SEEKDATE"].ToString().Trim();

                        strYear1 = VB.Left(dt.Rows[i]["SEEKDATE"].ToString().Trim(), 4);

                        if (strYear2 != strYear1)
                        {
                            if (BnColor == true)
                            {
                                BnColor = false;
                            }
                            else
                            {
                                BnColor = true;
                            }
                            strYear2 = strYear1;
                        }

                        if (BnColor == true)
                        {
                            ss2_Sheet1.Cells[i, 1].BackColor = Color.White; //&HFFFFFF
                        }
                        else
                        {
                            ss2_Sheet1.Cells[i, 1].BackColor = Color.Beige;//&HC0FFFF
                        }

                        ss2_Sheet1.Cells[i, 2].Text = (dt.Rows[i]["DRDATE"].ToString().Trim() != "" ? "P" : "");
                        ss2_Sheet1.Cells[i, 3].Text = (VB.Val(dt.Rows[i]["DRWRTNO"].ToString().Trim()) != 0 ? "★" : "");

                        ss2_Sheet1.Cells[i, 4].Text = "";

                        if (VB.Val(dt.Rows[i]["EXINFO"].ToString().Trim()) > 1000)
                            ss2_Sheet1.Cells[i, 4].Text = "◎";

                        if (dt.Rows[i]["DRNAME"].ToString().Trim() == "건진")
                        {
                            if (VB.Val(dt.Rows[i]["EXINFO"].ToString().Trim()) == 99)
                            {
                                ss2_Sheet1.Cells[i, 4].Text = "◎";
                            }
                        }

                        ss2_Sheet1.Cells[i, 5].Text = "";

                        if (dt.Rows[i]["PACSSTUDYID"].ToString().Trim() != "")
                        {
                            ss2_Sheet1.Cells[i, 5].Text = "▦";
                        }
                        else
                        {
                            ss2_Sheet1.Cells[i, 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            ss2_Sheet1.Cells[i, 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            ss2_Sheet1.Cells[i, 5].Text = "";
                        }

                        if(dt.Rows[i]["DRNAME"].ToString().Trim() =="건진")
                        {
                            if(dt.Rows[i]["GBPACS"].ToString().Trim() == "")
                            {
                                ss2_Sheet1.Cells[i, 5].Text = "";
                            }
                        }


                        ss2_Sheet1.Cells[i, 6].Text = "";

                        switch (dt.Rows[i]["XJONG"].ToString().Trim())
                        {
                            case "1":
                                ss2_Sheet1.Cells[i, 6].Text = "일반";
                                break;
                            case "2":
                                ss2_Sheet1.Cells[i, 6].Text = "특수";
                                break;
                            case "3":
                                ss2_Sheet1.Cells[i, 6].Text = "SONO";
                                break;
                            case "4":
                                ss2_Sheet1.Cells[i, 6].Text = "CT";
                                break;
                            case "5":
                                ss2_Sheet1.Cells[i, 6].Text = "MRI";
                                break;
                            case "6":
                                ss2_Sheet1.Cells[i, 6].Text = "RI";
                                break;
                            case "7":
                                ss2_Sheet1.Cells[i, 6].Text = "BMD";
                                break;
                            case "8":
                                ss2_Sheet1.Cells[i, 6].Text = "PET-CT";
                                break;
                            case "E":
                                //ss2_Sheet1.Cells[i, 6].Text = "EMG";
                                break;
                        }

                        ss2_Sheet1.Cells[i, 7].Text = " " + strXName;

                        if (chkSpec.Checked == true && String.Compare(strXJong, "1") > 0 && strXCode != "XCDC" && strXCode != "PACS-C" && strXCode != "CAGCOPY" && strXCode != "CUSCOPY")
                        {
                            ss2_Sheet1.Cells[i, 7].Font = new Font("굴림", 9, FontStyle.Bold);
                        }
                        else
                        {
                            ss2_Sheet1.Cells[i, 7].Font = new Font("굴림", 9, FontStyle.Regular);
                        }

                        ss2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 10].Text = dt.Rows[i]["XCODE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 11].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 12].Text = dt.Rows[i]["PACSNO"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 13].Text = dt.Rows[i]["EXINFO"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 14].Text = dt.Rows[i]["PACSSTUDYID"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 15].Text = dt.Rows[i]["XJONG"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 16].Text = dt.Rows[i]["ORDERDATE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 17].Text = dt.Rows[i]["JEPSUTIME"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 18].Text = dt.Rows[i]["XSENDDATE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 19].Text = dt.Rows[i]["XSUBCODE"].ToString().Trim();

                        //'PACS서버 고장으로 웹1000만 서비스가 가능한 경우
                        //''0.정상 1.PACS고장 2.웹서버고장 3.PACS 및 웹서버고장

                        if (nPACS_Service == 1 || nPACS_Service == 3)
                        {
                            if (dt.Rows[i]["PACSNO"].ToString().Trim() != "")
                            {
                                ss2_Sheet1.Cells[i, 5].Text = "▦";
                            }
                        } 
                        ss2_Sheet1.Cells[i, 20].Text = dt.Rows[i]["DRWRTNO"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 23].Text = dt.Rows[i]["IPDOPD"].ToString().Trim(); 
                        ss2_Sheet1.Cells[i, 24].Text = dt.Rows[i]["BDATE"].ToString().Trim();


                        //2019-10-28 김해수 판독일자 가져오는부분 추가작업
                        //2020-11-09 심사팀만 보이게 작업 
                        switch (clsType.User.IdNumber)
                        {
                            case "00468": //심경순
                            case "15273": //정희정
                            case "19399": //김준수
                            case "21181": //이향숙
                            case "22699": //김연서
                            case "27176": //이은주
                            case "13635": //이민주
                            case "37074": //김성열
                            case "38320": //현미정
                            case "46000": //정지애
                            case "50773": //이현정
                            case "45316": //김해수
                                SQL = "SELECT TO_CHAR(READDATE,'YYYY-MM-DD') READDATE, XDRCODE1 FROM ADMIN.XRAY_RESULTNEW";
                                SQL = SQL + ComNum.VBLF + "WHERE 1=1 ";
                                SQL = SQL + ComNum.VBLF + "AND WRTNO = '" + dt.Rows[i]["EXINFO"].ToString().Trim() + "'";

                                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                if (dt1.Rows.Count > 0)
                                {
                                    ss2_Sheet1.Cells[i, 25].Text = dt1.Rows[0]["READDATE"].ToString().Trim();
                                    ss2_Sheet1.Cells[i, 26].Text = dt1.Rows[0]["XDRCODE1"].ToString().Trim();
                                }

                                ss2_Sheet1.Columns[25].Visible = true;
                                ss2_Sheet1.Columns[26].Visible = true;
                                break;
                            default:
                                ss2_Sheet1.Columns[25].Visible = false;
                                ss2_Sheet1.Columns[26].Visible = false;
                                break;
                        }
                    }

                    txtResult.Text = "";
                    txtAddendum.Text = "";
                }
                dt.Dispose();
                dt = null;

                if (sender != null)
                {
                    lblTitleSub11.Text = ((Button)sender).Text.Trim();
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private int GetPACSService()
        {
            int intVal = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {

                SQL = "";
                SQL = "SELECT CODE FROM ADMIN.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = 'PACS_서비스' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return intVal;
                }

                if (dt.Rows.Count > 0)
                {
                    intVal = Convert.ToInt32(VB.Val(dt.Rows[0]["CODE"].ToString().Trim()));
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return intVal;
        }

        private void btnConsult_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text.Trim() == "")
                return;

            int i = 0;

            ssConsult_Sheet1.RowCount = 0;

            txtJusaMemo.Visible = false;
            panConsult.Visible = true;


            panXray.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnWordEntry.Visible = false;
            panPFT.Visible = false;

            ssXrayDr.Visible = false;

            txtResult1.Visible = false;
            ss1.Visible = false;
            panEMG.Visible = false;
            panExam.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = false;
            panExam3.Visible = false;
            lblTitleSub0.Text = "";

            panEEG.Visible = false;

            SetPAN(panConsult);

            txtConsult.Text = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT A.PTNO, B.SNAME, A.FRDEPTCODE, A.FRDRCODE, A.TODEPTCODE, A.TODRCODE, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.SDATE,'YYYY-MM-DD HH24:MI')  SDATE, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.EDATE,'YYYY-MM-DD HH24:MI')  EDATE, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD')  BDATE, ";

                SQL = SQL + ComNum.VBLF + "  A.FRREMARK, A.TOREMARK , A.ROWID,A.GBEMSMS,  ";
                SQL = SQL + ComNum.VBLF + "  C.DRNAME CDRNAME, D.DRNAME DDRNAME  ";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.OCS_ITRANSFER  A, ADMIN.BAS_PATIENT B, ";
                SQL = SQL + ComNum.VBLF + "        ADMIN.BAS_DOCTOR C ,   ADMIN.BAS_DOCTOR D ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO ='" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.EDATE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "    AND (A.GBDEL <>'*' OR A.GBDEL IS NULL ) "; //'삭제 제외;
                SQL = SQL + ComNum.VBLF + "    AND A.GBCONFIRM ='*'"; //'완료된 내역만;
                SQL = SQL + ComNum.VBLF + "    AND A.PTNO  = B.PANO(+)";

                SQL = SQL + ComNum.VBLF + "    AND A.FRDRCODE = C.DRCODE(+) ";
                SQL = SQL + ComNum.VBLF + "    AND A.TODRCODE = D.DRCODE(+) ";
                //2020-03-16 안정수, 조건 추가 전산의뢰 1694
                //SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE DESC ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE DESC, SDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssConsult_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {

                        ssConsult_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssConsult_Sheet1.Cells[i, 1].Text = dt.Rows[i]["FRDEPTCODE"].ToString().Trim();
                        ssConsult_Sheet1.Cells[i, 2].Text = dt.Rows[i]["CDRNAME"].ToString().Trim();
                        ssConsult_Sheet1.Cells[i, 3].Text = dt.Rows[i]["TODEPTCODE"].ToString().Trim();
                        ssConsult_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DDRNAME"].ToString().Trim();
                        ssConsult_Sheet1.Cells[i, 5].Text = dt.Rows[i]["FRDRCODE"].ToString().Trim();
                        ssConsult_Sheet1.Cells[i, 6].Text = dt.Rows[i]["TODRCODE"].ToString().Trim();
                        ssConsult_Sheet1.Cells[i, 7].Text = dt.Rows[i]["FRREMARK"].ToString();
                        ssConsult_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                        ssConsult_Sheet1.Cells[i, 9].Text = dt.Rows[i]["EDATE"].ToString().Trim();
                        ssConsult_Sheet1.Cells[i, 10].Text = dt.Rows[i]["TOREMARK"].ToString().Trim();

                        //   'add  GbERSMS 응급dd
                        ssConsult_Sheet1.Cells[i, 11].Text = "";

                        switch (dt.Rows[i]["TOREMARK"].ToString().Trim())
                        {
                            case "Y":
                                ssConsult_Sheet1.Cells[i, 11].Text = "■응급 □비응급";
                                break;
                            default:
                                ssConsult_Sheet1.Cells[i, 11].Text = "□응급 ■비응급";
                                break;
                        }
                        ssConsult_Sheet1.Cells[i, 12].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                lblTitleSub11.Text = ((Button)sender).Text.Trim();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            //2019-07-26 복사하기
            ssExam02_Sheet1.ClipboardCopy();
            ssExam02_Sheet1.ClearSelection();
        }

        private void btnCVR_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            txtJusaMemo.Visible = false;

            panPFT.Visible = false;
            panEtc.Visible = false;
            panEMG.Visible = false;

            panXray.Visible = false;

            panExam.Visible = false;
            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = true;
            panExam3.Visible = false;
            nIndex = 3;

            txtCRemark1.Text = "";
            txtCRemark2.Text = "";

            ssCVR_Sheet1.RowCount = 0;

            mstrRowIdCVR = "";

            SetPAN(panCVR);
            GetCVR(txtPtNo.Text.Trim());

            lblTitleSub11.Text = ((Button)sender).Text.Trim();
        }

        private void GetCVR(string strPtNo)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssCVR_Sheet1.RowCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = "    SELECT S.SNAME,A.*, TO_CHAR(A.BDATE, 'YYYY-MM-DD') BDATE, A.ROWID,";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(S.RECEIVEDATE, 'YYYY-MM-DD') RECEIVEDATE,          ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE, A.ROWID, A.MASTERCODE,";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.CVR_ENTDATE,'YYYY-MM-DD HH24:MI') CVRENTDATE,    ";
                SQL = SQL + ComNum.VBLF + "  A.CVR_ENTSABUN, A.CREMARK1,A.CVR_CONFIRM_SABUN,A.CREMARK2, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.CVR_CONFIRM_DATE,'YYYY-MM-DD HH24:MI') CVRCDATE  ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.EXAM_ANATMST A, ADMIN.EXAM_SPECMST S                ";
                SQL = SQL + ComNum.VBLF + " WHERE A.SPECNO = S.SPECNO                           ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.ANATNO = S.ANATNO       ";
                SQL = SQL + ComNum.VBLF + "   AND A.CVR_ENTDATE IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY A.ANATNO, A.SPECNO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssCVR_Sheet1.RowCount = dt.Rows.Count;

                    ssCVR_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ANATNO"].ToString().Trim();
                    ssCVR_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssCVR_Sheet1.Cells[i, 2].Text = dt.Rows[i]["GBIO"].ToString().Trim();
                    ssCVR_Sheet1.Cells[i, 3].Text = dt.Rows[i]["CVRENTDATE"].ToString().Trim();
                    ssCVR_Sheet1.Cells[i, 4].Text = dt.Rows[i]["CVR_ENTSABUN"].ToString().Trim();
                    ssCVR_Sheet1.Cells[i, 5].Text = dt.Rows[i]["CREMARK1"].ToString().Trim();
                    ssCVR_Sheet1.Cells[i, 6].Text = dt.Rows[i]["CVRCDATE"].ToString().Trim();
                    ssCVR_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                    ssCVR_Sheet1.Cells[i, 8].Text = dt.Rows[i]["CVR_CONFIRM_SABUN"].ToString().Trim();
                    ssCVR_Sheet1.Cells[i, 9].Text = dt.Rows[i]["CREMARK2"].ToString().Trim();
                    ssCVR_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnCVROK_Click(object sender, EventArgs e)
        {
            if (UpDateCVROK() == true)
            {
                ComFunc.MsgBox("저장완료!!", "작업완료");
            }
        }

        private bool UpDateCVROK()
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                return false; //권한 확인

            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (mstrRowIdCVR == "")
            {
                ComFunc.MsgBox("조치내용을 입력할 항목을 선택해 주세요.", "CVR 등록건 확인");
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " UPDATE ADMIN.EXAM_ANATMST SET ";
                SQL = SQL + ComNum.VBLF + " CVR_CONFIRM_DATE = SYSDATE, ";
                SQL = SQL + ComNum.VBLF + " CVR_CONFIRM_SABUN = " + clsType.User.IdNumber + ", ";
                SQL = SQL + ComNum.VBLF + " CREMARK2 = '" + txtCRemark2.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + mstrRowIdCVR + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
            return rtVal;
        }

        private void btnCVROK2_Click(object sender, EventArgs e)
        {
            if (UpDateCVROK2() == true)
            {
                ComFunc.MsgBox("저장완료!!", "작업완료");
            }


        }

        private bool UpDateCVROK2()
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                return false; //권한 확인

            bool rtVal = false;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (mstrRowIdCVR == "")
            {
                ComFunc.MsgBox("조치내용을 입력할 항목을 선택해 주세요.", "CVR 등록건 확인");
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {


                SQL = "";
                SQL = " UPDATE ADMIN.EXAM_ANATMST SET ";
                SQL = SQL + ComNum.VBLF + " CVR_CONFIRM_DATE = SYSDATE, ";
                SQL = SQL + ComNum.VBLF + " CVR_CONFIRM_SABUN = " + clsType.User.IdNumber + ", ";
                SQL = SQL + ComNum.VBLF + " CREMARK2 = '" + txtCVR2.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE ANATNO ='" + mstrAnatno + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
            return rtVal;
        }

        private void btnCytology_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            clsPublic.GnLogOutCNT = 0;

            txtJusaMemo.Visible = false;

            panEtc.Visible = true;
            panEMG.Visible = false;
            panXray.Visible = false;
            ss1.Visible = true;
            txtResult1.Visible = true;
            panExam.Visible = false;
            panPFT.Visible = false;
            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = false;
            panExam3.Visible = false;

            nIndex = 4;

            ss1_Sheet1.Columns.Get(9).Label = "Anatno";

            SetPAN(panEtc);

            txtResult1.Text = "";

            ss1_Sheet1.RowCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.GBIO, B.ORDERNAME, A.DEPTCODE,A.ANATNO,A.ROWID,A.CREMARK1 ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.EXAM_ANATMST A, ADMIN.OCS_ORDERCODE B  ,ADMIN.EXAM_SPECMST C  ";
                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO     = '" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.SPECNO = C.SPECNO ";
                SQL = SQL + ComNum.VBLF + "   AND A.ORDERCODE = B.ORDERCODE       ";
                //SQL = SQL + ComNum.VBLF + "   AND SUBSTR(A.ANATNO,1,1) IN ('C','P') ";
                //2018-08-28 안정수, AC 추가
                SQL = SQL + ComNum.VBLF + "   AND (SUBSTR(A.ANATNO,1,1) IN ('C','P') OR SUBSTR(A.ANATNO,1,2) IN ('AC')) ";
                //SQL = SQL + ComNum.VBLF + "   AND (B.SENDDEPT <> 'N' OR B.SENDDEPT IS NULL) "; //내분비내과 요청-기존내역보는것이라 풀어줌
                SQL = SQL + ComNum.VBLF + " ORDER BY A.BDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ss1_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 7].Text = (dt.Rows[i]["CREMARK1"].ToString().Trim() != "" ? "◎" : ""); //'2015-07-01;
                        ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ANATNO"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                lblTitleSub11.Text = ((Button)sender).Text.Trim();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        } 

        private void btnDiscern_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text.Trim() == "")
                return;

            int i = 0;

            txtJusaMemo.Visible = false;

            panConsult.Visible = false;

            panXray.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnWordEntry.Visible = false;
            panPFT.Visible = false;

            ssXrayDr.Visible = false;

            txtResult1.Visible = false;
            ss1.Visible = false;

            panEMG.Visible = false;
            panExam.Visible = false;
            panEEG.Visible = false;

            panDiscern.Visible = true;
            panECG.Visible = false;
            panEtcJupmst.Visible = false;
            panExam3.Visible = false;
            lblTitleSub0.Text = "";

            txtConsult.Text = "";

            SetPAN(panDiscern);

            ssDiscern_Sheet1.RowCount = 0;

            tabControl1.SelectedIndex = 2;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT PANO,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,                ";
                SQL = SQL + ComNum.VBLF + "  ROWID, IPDOPD, BUN, WRTNO                                    ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.DRUG_HOIMST               ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPtNo.Text.Trim() + "'                                       ";
                //2020-09-11 GBJOB 조건 제외 
                //SQL = SQL + ComNum.VBLF + " AND GBJOB IS NULL ";
                //'2016-11-07 접수번호:1226 (약품식별 조회시 완료되것만 병동에서 조회);
                SQL = SQL + ComNum.VBLF + " AND BUN = '2'  ";
                SQL = SQL + ComNum.VBLF + " ORDER BY BDATE DESC ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssDiscern_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {

                        ssDiscern_Sheet1.Cells[i, 0].Text = "Y";
                        ssDiscern_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDATE"].ToString().Trim();

                        switch (dt.Rows[i]["BUN"].ToString().Trim())
                        {
                            case "1":
                                ssDiscern_Sheet1.Cells[i, 2].Text = "부분";
                                break;
                            case "2":
                                ssDiscern_Sheet1.Cells[i, 2].Text = "완료";
                                break;
                        }

                        ssDiscern_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssDiscern_Sheet1.Cells[i, 4].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                if (sender != null)
                {
                    lblTitleSub11.Text = ((Button)sender).Text.Trim();
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnEar_Click(object sender, EventArgs e)
        {

            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text.Trim() == "")
                return;

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            txtJusaMemo.Visible = false;

            panPFT.Visible = false;
            panEtc.Visible = false;
            panEMG.Visible = false;
            panXray.Visible = false;
            panExam.Visible = false;
            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = true;
            panExam3.Visible = false;
            nIndex = 4;

            SetPAN(panEtcJupmst);

            txtResult1.Text = "";

            mstrGubun = "6";

            ssAudio_Sheet1.RowCount = 0;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE , TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE ,A.GBFTP, ";
                SQL = SQL + ComNum.VBLF + "  A.AGE, A.SEX, A.GBIO, A.DEPTCODE,   B.ORDERNAME, A.ROWID, A.IMAGE , C.DRNAME  ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ETC_JUPMST A, ADMIN.OCS_ORDERCODE B, ADMIN.BAS_DOCTOR C";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO= '" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.GUBUN ='" + mstrGubun + "' "; //' AUDIO;
                SQL = SQL + ComNum.VBLF + "  AND A.GBJOB NOT IN ('9') ";
                SQL = SQL + ComNum.VBLF + "  AND A.DRCODE = C.DRCODE(+) ";
                //'1미접수 2.예약 3.접수 9.취소);
                SQL = SQL + ComNum.VBLF + " AND A.ORDERCODE= B.ORDERCODE(+)";
                SQL = SQL + ComNum.VBLF + " AND A.BDATE <=TRUNC(SYSDATE) "; //'INDEX 태우기위해사용;
                SQL = SQL + ComNum.VBLF + " ORDER BY A.RDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssAudio_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssAudio_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["GBIO"].ToString().Trim() == "O" ? "외래" : "입원");
                        ssAudio_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 7].Text = (VB.Val(dt.Rows[i]["IMAGE"].ToString().Trim()) == 0 ? "" : "▦");

                        if (dt.Rows[i]["GBFTP"].ToString().Trim() == "Y")
                            ssAudio_Sheet1.Cells[i, 7].Text = "▦"; // '2014-11-20;

                        ssAudio_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }

                }

                dt.Dispose();
                dt = null;

                lblTitleSub11.Text = ((Button)sender).Text.Trim();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }


        }

        private void btnEC_Click(object sender, EventArgs e)
        {

            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text.Trim() == "")
                return;

            int nPACS_Service = 0;
            int i = 0;
            string strXName = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            //'0.정상 1.PACS고장 2.웹서버고장 3.PACS 및 웹서버고장
            nPACS_Service = GetPACSService();

            ss2_Sheet1.RowCount = 0;

            txtJusaMemo.Visible = false;

            panXray.Visible = true;
            txtResult1.Visible = false;
            panEMG.Visible = false;
            ss1.Visible = false;
            panExam.Visible = false;
            panPFT.Visible = false;
            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = false;
            panExam3.Visible = false;
            nIndex = 1;

            SetPAN(panXray);

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "SELECT A.PANO,B.SNAME,TO_CHAR(A.SEEKDATE,'YYYY-MM-DD') SEEKDATE,A.DEPTCODE, A.DRDATE, A.DRWRTNO, ";
                SQL = SQL + ComNum.VBLF + " A.DRCODE,C.DRNAME,D.XNAME,A.EXINFO,A.PACSNO,A.XJONG,A.XCODE,A.XSUBCODE,A.IPDOPD,A.ORDERNO,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.ENTERDATE,'MM/DD HH24:MI') ORDERDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.SEEKDATE,'MM/DD HH24:MI') JEPSUTIME,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.XSENDDATE,'MM/DD HH24:MI') XSENDDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, ";
                SQL = SQL + ComNum.VBLF + " A.PACSSTUDYID,A.ORDERNAME,A.REMARK,A.ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.XRAY_DETAIL A,ADMIN.BAS_PATIENT B,ADMIN.BAS_DOCTOR C,";
                SQL = SQL + ComNum.VBLF + "      ADMIN.XRAY_CODE D ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PANO='" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND (A.XJONG ='C' OR A.XCODE IN ('EB521A', 'EB561'))"; //'심장초음파;
                SQL = SQL + ComNum.VBLF + "  AND A.GBRESERVED >= '6' "; //'접수 또는 촬영완료;
                SQL = SQL + ComNum.VBLF + "  AND A.SEEKDATE <= TRUNC(SYSDATE + 1) ";
                SQL = SQL + ComNum.VBLF + "  AND A.PANO = B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "  AND A.DRCODE = C.DRCODE(+) ";
                SQL = SQL + ComNum.VBLF + "  AND A.XCODE = D.XCODE(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.SEEKDATE DESC,A.PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss2_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strXName = dt.Rows[i]["ORDERNAME"].ToString().Trim();

                        if (strXName == "")
                            strXName = dt.Rows[i]["XNAME"].ToString().Trim();

                        strXName = strXName + " " + dt.Rows[i]["REMARK"].ToString().Trim();

                        ss2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SEEKDATE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 2].Text = (dt.Rows[i]["DRDATE"].ToString().Trim() != "" ? "P" : "");
                        ss2_Sheet1.Cells[i, 3].Text = (VB.Val(dt.Rows[i]["DRWRTNO"].ToString().Trim()) != 0 ? "★" : "");
                        ss2_Sheet1.Cells[i, 4].Text = (VB.Val(dt.Rows[i]["EXINFO"].ToString().Trim()) > 1000 ? "◎" : "");
                        ss2_Sheet1.Cells[i, 5].Text = (dt.Rows[i]["PACSSTUDYID"].ToString().Trim() != "" ? "▦" : "");

                        ss2_Sheet1.Cells[0, 6].Text = "";
                        switch (dt.Rows[i]["XJONG"].ToString().Trim())
                        {
                            case "1":
                                ss2_Sheet1.Cells[i, 6].Text = "일반";
                                break;
                            case "2":
                                ss2_Sheet1.Cells[i, 6].Text = "특수";
                                break;
                            case "3":
                                ss2_Sheet1.Cells[i, 6].Text = "SONO";
                                break;
                            case "4":
                                ss2_Sheet1.Cells[i, 6].Text = "CT";
                                break;
                            case "5":
                                ss2_Sheet1.Cells[i, 6].Text = "MRI";
                                break;
                            case "6":
                                ss2_Sheet1.Cells[i, 6].Text = "RI";
                                break;
                            case "7":
                                ss2_Sheet1.Cells[i, 6].Text = "BMD";
                                break;
                            case "8":
                                ss2_Sheet1.Cells[i, 6].Text = "PET-CT";
                                break;
                            case "E":
                                ss2_Sheet1.Cells[i, 6].Text = "EMG";
                                break;
                            case "C":
                                ss2_Sheet1.Cells[i, 6].Text = "심장초음파";
                                break;
                        }

                        ss2_Sheet1.Cells[i, 7].Text = " " + strXName;
                        ss2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 10].Text = dt.Rows[i]["XCODE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 11].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 12].Text = dt.Rows[i]["PACSNO"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 13].Text = dt.Rows[i]["EXINFO"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 14].Text = dt.Rows[i]["PACSSTUDYID"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 15].Text = dt.Rows[i]["XJONG"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 16].Text = dt.Rows[i]["ORDERDATE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 17].Text = dt.Rows[i]["JEPSUTIME"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 18].Text = dt.Rows[i]["XSENDDATE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 19].Text = dt.Rows[i]["XSUBCODE"].ToString().Trim();

                        //'PACS서버 고장으로 웹1000만 서비스가 가능한 경우
                        //''0.정상 1.PACS고장 2.웹서버고장 3.PACS 및 웹서버고장
                        if (nPACS_Service == 1 || nPACS_Service == 3)
                        {
                            if (dt.Rows[i]["PACSNO"].ToString().Trim() != "")
                            {
                                ss2_Sheet1.Cells[i, 5].Text = "▦";
                            }
                        }
                        ss2_Sheet1.Cells[i, 22].Text = GetOrderStsChk(dt.Rows[i]["IPDOPD"].ToString().Trim(), dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["BDATE"].ToString().Trim(), dt.Rows[i]["DEPTCODE"].ToString().Trim(), dt.Rows[i]["ORDERNO"].ToString().Trim());
                    }

                    txtResult.Text = "";
                    txtAddendum.Text = "";

                }

                dt.Dispose();
                dt = null;

                lblTitleSub11.Text = ((Button)sender).Text.Trim();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string GetOrderStsChk(string strIO, string strPano, string strBDate, string strDept, string strOrderNo)
        {
            DataTable dt = null;
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Val(strOrderNo) == 0)
                return strVal;

            try
            {
                //IF ARGIO = "I" OR ARGDEPT = "ER" THEN;
                if (strIO == "I" || strDept == "ER")
                {
                    SQL = " SELECT PTNO FROM ADMIN.OCS_IORDER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + " AND BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND ORDERNO =" + strOrderNo + " ";
                    SQL = SQL + ComNum.VBLF + " AND GBSTATUS IN ( 'D','-D')  "; //'DC;
                }
                else
                {
                    SQL = " SELECT PTNO FROM ADMIN.OCS_OORDER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + " AND BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND ORDERNO =" + strOrderNo + " ";
                    SQL = SQL + ComNum.VBLF + " AND GBSUNAP IN ( '2')  "; //'DC;
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = "DC";
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return strVal;

        }

        private void btnECG_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text.Trim() == "")
                return;

            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;

            txtJusaMemo.Visible = false;

            panPFT.Visible = false;
            panEtc.Visible = false;
            panEMG.Visible = false;
            panXray.Visible = false;
            panExam.Visible = false;
            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = true;
            panExam3.Visible = false;

            nIndex = 4;

            SetPAN(panEtcJupmst);

            txtResult1.Text = "";

            mstrGubun = "1";
            ssAudio_Sheet1.RowCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE , TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE ,A.GBFTP,A.IMAGE_GBN, ";
                SQL = SQL + ComNum.VBLF + "  A.AGE, A.SEX, A.GBIO, A.DEPTCODE,   B.ORDERNAME, A.ROWID, A.IMAGE , C.DRNAME  ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ETC_JUPMST A, ADMIN.OCS_ORDERCODE B, ADMIN.BAS_DOCTOR C";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO= '" + txtPtNo.Text + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.GUBUN ='" + mstrGubun + "'";  //AUDIO
                SQL = SQL + ComNum.VBLF + "  AND A.GBJOB NOT IN ('9') ";
                SQL = SQL + ComNum.VBLF + "  AND A.DRCODE = C.DRCODE(+) ";
                //'1미접수 2.예약 3.접수 9.취소);
                SQL = SQL + ComNum.VBLF + " AND A.ORDERCODE= B.ORDERCODE(+)";
                SQL = SQL + ComNum.VBLF + " AND A.BDATE <=TRUNC(SYSDATE) "; //'INDEX 태우기위해사용;
                SQL = SQL + ComNum.VBLF + " ORDER BY A.RDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssAudio_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssAudio_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["GBIO"].ToString().Trim() == "O" ? "외래" : "입원");
                        ssAudio_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 7].Text = (dt.Rows[i]["IMAGE"].ToString().Trim().Length == 0 ? "" : "▦");

                        if (dt.Rows[i]["GBFTP"].ToString().Trim() == "Y") //'2014-11-18
                        {
                            ssAudio_Sheet1.Cells[i, 7].Text = "▦";
                        }

                        ssAudio_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 10].Text = dt.Rows[i]["IMAGE_GBN"].ToString().Trim();

                    }
                }

                dt.Dispose();
                dt = null;

                lblTitleSub11.Text = ((Button)sender).Text.Trim();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnEEG_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text.Trim() == "")
                return;

            DataTable dt = null;

            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ss2_Sheet1.RowCount = 0;

            txtJusaMemo.Visible = false;

            panEMG.Visible = false;
            panXray.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnWordEntry.Visible = false;
            panPFT.Visible = false;
            panEEG.Visible = true;
            panConsult.Visible = false;
            panDiscern.Visible = false;

            ssXrayDr.Visible = false;

            txtResult1.Visible = false;

            //ss1_Sheet1.Visible = false;

            panExam.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = false;
            panExam3.Visible = false;
            nIndex = 1;

            SetPAN(panEEG);

            ssEEG_Sheet1.RowCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT  TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE ,  A.READ_WRTNO,   DECODE(A.GBJOB ,'3','EEG','기타') GBJOB, A.ORDERNO, A.ROWID,  ";
                SQL = SQL + ComNum.VBLF + "   B.ORDERNAME, A.DEPTCODE, A.DRCODE ,  C.DRNAME   ";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.ETC_JUPMST  A,  ADMIN.OCS_ORDERCODE B , ADMIN.BAS_DOCTOR C ";
                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.GUBUN = '2' ";
                SQL = SQL + ComNum.VBLF + "  AND A.ORDERCODE = B.ORDERCODE(+)";
                SQL = SQL + ComNum.VBLF + "  AND A.DRCODE =C.DRCODE ";
                SQL = SQL + ComNum.VBLF + "  AND A.GBJOB <> '9' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.RDATE DESC,A.PTNO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssEEG_Sheet1.RowCount = dt.Rows.Count;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssEEG_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                            ssEEG_Sheet1.Cells[i, 1].Text = (VB.Val(dt.Rows[i]["READ_WRTNO"].ToString().Trim()) > 0 ? "◎" : "");
                            ssEEG_Sheet1.Cells[i, 2].Text = dt.Rows[i]["GBJOB"].ToString().Trim();
                            ssEEG_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            ssEEG_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssEEG_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                            ssEEG_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                            ssEEG_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                            ssEEG_Sheet1.Cells[i, 8].Text = dt.Rows[i]["READ_WRTNO"].ToString().Trim();
                        }
                        txtEEG.Text = "";
                    }
                }

                dt.Dispose();
                dt = null;

                lblTitleSub11.Text = ((Button)sender).Text.Trim();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnEkgEtc_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text.Trim() == "")
                return;

            txtJusaMemo.Visible = false;

            panPFT.Visible = false;
            panEtc.Visible = false;
            panEMG.Visible = false;
            panXray.Visible = false;
            panExam.Visible = false;
            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = true;
            panExam3.Visible = false;
            nIndex = 4;

            SetPAN(panEtcJupmst);

            txtResult1.Text = "";

            ssAudio_Sheet1.RowCount = 0;

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE , TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE ,A.GBFTP, ";
                SQL = SQL + ComNum.VBLF + "  A.AGE, A.SEX, A.GBIO, A.DEPTCODE,   B.ORDERNAME, A.ROWID, A.IMAGE , C.DRNAME  ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ETC_JUPMST A, ADMIN.OCS_ORDERCODE B, ADMIN.BAS_DOCTOR C";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO= '" + txtPtNo.Text.Trim() + "' ";
                //'ABI 검사 함께 보여달라고 의뢰서 올라옴
                //'2016-05-03 계장 김현욱
                SQL = SQL + ComNum.VBLF + "  AND A.GUBUN IN ('3','9','10','11','7')";
                SQL = SQL + ComNum.VBLF + "  AND A.GBJOB IN ('3') ";
                SQL = SQL + ComNum.VBLF + "  AND A.DRCODE = C.DRCODE(+) ";
                //'1미접수 2.예약 3.접수 9.취소)
                SQL = SQL + ComNum.VBLF + " AND A.ORDERCODE= B.ORDERCODE(+)";
                SQL = SQL + ComNum.VBLF + " AND A.BDATE <=TRUNC(SYSDATE) "; //'INDEX 태우기위해사용
                SQL = SQL + ComNum.VBLF + " ORDER BY A.RDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssAudio_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssAudio_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["GBIO"].ToString().Trim() == "O" ? "외래" : "입원");
                        ssAudio_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 7].Text = (dt.Rows[i]["IMAGE"].ToString().Trim().Length == 0 ? "" : "▦");

                        if (dt.Rows[i]["GBFTP"].ToString().Trim() == "Y")
                            ssAudio_Sheet1.Cells[i, 7].Text = "▦"; //'2014-11-20

                        ssAudio_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                lblTitleSub11.Text = ((Button)sender).Text.Trim();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnEMG_Click(object sender, EventArgs e)
        {

            clsPublic.GnLogOutCNT = 0;

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strXName = "";
            string strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            clsPublic.GnLogOutCNT = 0;

            ss2_Sheet1.RowCount = 0;

            txtJusaMemo.Visible = false;

            panEMG.Visible = true;
            panXray.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnWordEntry.Visible = false;
            panPFT.Visible = false;
            ssXrayDr.Visible = false;
            ss1.Visible = false;
            txtResult1.Visible = false;
            panExam.Visible = false;
            lblTitleSub0.Text = "";
            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = false;
            panExam3.Visible = false;
            nIndex = 1;
            SetPAN(panEMG);
            txtEmgResult.Text = "";

            panEMG1.Width = (panEMG.Width - txtEmgResult.Width) / 2;
            panEMG2.Width = (panEMG.Width - txtEmgResult.Width) / 2;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                //'자료를 찾기
                SQL = "";
                SQL = "SELECT A.PANO,B.SNAME,TO_CHAR(A.SEEKDATE,'YYYY-MM-DD') SEEKDATE,A.DEPTCODE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.DRDATE,'YYYY-MM-DD') DRDATE, A.DRWRTNO, ";
                SQL = SQL + ComNum.VBLF + " A.DRCODE,C.DRNAME,D.XNAME,A.EXINFO,A.PACSNO,A.XJONG,A.XCODE,A.XSUBCODE,A.IPDOPD,A.ORDERNO,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.ENTERDATE,'MM/DD HH24:MI') ORDERDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.SEEKDATE,'MM/DD HH24:MI') JEPSUTIME,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.XSENDDATE,'MM/DD HH24:MI') XSENDDATE, E.GBJOB, ";
                SQL = SQL + ComNum.VBLF + " A.PACSSTUDYID,A.ORDERNAME,A.REMARK,A.ROWID , A.EMGWRTNO, TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.SEX, A.AGE   ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.XRAY_DETAIL A,ADMIN.BAS_PATIENT B,ADMIN.BAS_DOCTOR C,";
                SQL = SQL + ComNum.VBLF + "      ADMIN.XRAY_CODE D , ADMIN.ETC_JUPMST E ";
                SQL = SQL + ComNum.VBLF + " WHERE A.PANO='" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.PANO=E.PTNO(+) ";
                SQL = SQL + ComNum.VBLF + "  AND A.ORDERNO=E.ORDERNO(+) ";
                SQL = SQL + ComNum.VBLF + "  AND A.XJONG = 'E' ";
                SQL = SQL + ComNum.VBLF + "  AND A.GBRESERVED >='6' ";// '접수 또는 촬영완료;
                SQL = SQL + ComNum.VBLF + "  AND (A.GBHIC IS NULL OR A.GBHIC <> 'Y') ";
                SQL = SQL + ComNum.VBLF + "  AND A.SEEKDATE<=TRUNC(SYSDATE+1) ";
                SQL = SQL + ComNum.VBLF + "  AND A.PANO=B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "  AND A.DRCODE=C.DRCODE(+) ";
                SQL = SQL + ComNum.VBLF + "  AND A.XCODE=D.XCODE(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY 3 DESC,1 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssEMG_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strXName = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        if (strXName == "")
                            strXName = dt.Rows[i]["XNAME"].ToString().Trim();

                        strXName = strXName + " " + dt.Rows[i]["REMARK"].ToString().Trim();

                        ssEMG_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        //'2013-12-10
                        if (Convert.ToDateTime(strDate) >= Convert.ToDateTime("2013-12-10"))
                        {
                            //        '근전도실 요청건 - 접수한것만 일자표시
                            if (dt.Rows[i]["GBJOB"].ToString().Trim() == "3")
                            {
                                ssEMG_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SEEKDATE"].ToString().Trim();
                            }
                            else
                            {
                                ssEMG_Sheet1.Cells[i, 1].Text = "";
                            }
                        }
                        else
                        {
                            ssEMG_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SEEKDATE"].ToString().Trim();
                        }

                        ssEMG_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssEMG_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssEMG_Sheet1.Cells[i, 4].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                        ssEMG_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssEMG_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                        ssEMG_Sheet1.Cells[i, 7].Text = (VB.Val(dt.Rows[i]["EXINFO"].ToString().Trim()) > 1000 ? "◎" : "");

                        if (dt.Rows[i]["EMGWRTNO"].ToString().Trim() != "")
                        {
                            //'이미지 장수 표시
                            SQL = " SELECT ROWID FROM ADMIN.ETC_RESULT    ";
                            SQL = SQL + ComNum.VBLF + "WHERE WRTNO = '" + dt.Rows[i]["EMGWRTNO"].ToString().Trim() + "' AND GBFTP ='Y' ";

                            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {

                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                ssEMG_Sheet1.Cells[i, 8].Text = "▦" + Convert.ToString(dt1.Rows.Count) + "장";
                            }
                            else
                            {
                                dt1.Dispose();
                                dt1 = null;

                                SQL = " SELECT COUNT(*) CNT FROM ADMIN.ETC_RESULT ";
                                SQL = SQL + ComNum.VBLF + "WHERE WRTNO = '" + dt.Rows[i]["EMGWRTNO"].ToString().Trim() + "' AND IMAGE IS NOT NULL ";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {

                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return;
                                }

                                if (dt1.Rows.Count > 0)
                                {

                                    ssEMG_Sheet1.Cells[i, 8].Text = "▦" + dt1.Rows[0]["CNT"].ToString().Trim() + "장";

                                }
                                dt1.Dispose();
                                dt1 = null;
                            }

                        }


                        ssEMG_Sheet1.Cells[i, 9].Text = strXName;
                        ssEMG_Sheet1.Cells[i, 10].Text = dt.Rows[i]["EXINFO"].ToString().Trim();
                        ssEMG_Sheet1.Cells[i, 11].Text = dt.Rows[i]["EMGWRTNO"].ToString().Trim();
                    }

                    txtEmgResult.Text = "";
                }

                dt.Dispose();
                dt = null;

                lblTitleSub11.Text = ((Button)sender).Text.Trim();

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnEMR_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            lblTitleSub11.Text = ((Button)sender).Text.Trim();

            SetPAN(pan2);

            //'과거 차트 조회 할 경우 사용함 절대로 막지   막지마삼.
            clsVbEmr.ExecuteEmr(clsDB.DbCon, txtPtNo.Text.Trim(), clsType.User.IdNumber);
        }

        private void btnEndo_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            txtJusaMemo.Visible = false;


            panEtc.Visible = true;
            panEMG.Visible = false;
            panXray.Visible = false;
            ss1.Visible = true;
            txtResult1.Visible = true;
            panExam.Visible = false;
            panPFT.Visible = false;


            panExam3.Visible = false;
            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = false;
            nIndex = 2;

            txtResult1.Text = "";

            ss1_Sheet1.ColumnHeader.Columns[8].Label = "PacsUID";
            ss1_Sheet1.ColumnHeader.Columns[4].Visible = false;

            SetPAN(panEtc);

            GetENDO();

            lblTitleSub11.Text = ((Button)sender).Text.Trim();
        }

        private void GetENDO()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수


            if (txtPtNo.Text.Trim() == "")
                return;

            ss1_Sheet1.RowCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = "SELECT  TO_CHAR(JDATE,'YYYY-MM-DD') BDATE1,TO_CHAR(RDATE,'YY/MM/DD') RDATE, ORDERCODE, GBJOB,GBNEW, ";
                SQL = SQL + ComNum.VBLF + "      SEQNO,  REMARK, PACSUID,PACSNO, ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.ENDO_JUPMST ";
                SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GBSUNAP <> '*'  ";
                SQL = SQL + ComNum.VBLF + "ORDER BY 1 DESC   ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = GetOrderName(dt.Rows[i]["ORDERCODE"].ToString().Trim());

                        ss1_Sheet1.Cells[i, 2].Text = (dt.Rows[i]["PACSUID"].ToString().Trim() != "" ? "▦" : "");

                        ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GBJOB"].ToString().Trim();

                        if (ComFunc.MidH(dt.Rows[i]["REMARK"].ToString().Trim(), 1, 50) != "")
                        {
                            ss1_Sheet1.Cells[i, 6].Text = ComFunc.MidH(dt.Rows[i]["REMARK"].ToString().Trim(), 1, 50);
                        }

                        if (ComFunc.MidH(dt.Rows[i]["REMARK"].ToString().Trim(), 51, 50) != "")
                        {
                            ss1_Sheet1.Cells[i, 6].Text = ss1_Sheet1.Cells[i, 6].Text.Trim() + ", " + ComFunc.MidH(dt.Rows[i]["REMARK"].ToString().Trim(), 51, 50);
                        }

                        if (ComFunc.MidH(dt.Rows[i]["REMARK"].ToString().Trim(), 101, 50) != "")
                        {
                            ss1_Sheet1.Cells[i, 6].Text = ss1_Sheet1.Cells[i, 6].Text.Trim() + ", " + ComFunc.MidH(dt.Rows[i]["REMARK"].ToString().Trim(), 101, 50);
                        }

                        ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["PACSNO"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["GBNEW"].ToString().Trim(); //2013-03-12
                        ss1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["RDATE"].ToString().Trim(); //2016-02-22
                    }
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void btnExam_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            txtExName.Text = "";

            txtJusaMemo.Visible = false;

            panXray.Visible = false;
            ss1.Visible = true;
            panEMG.Visible = false;
            txtResult1.Visible = false;
            panPFT.Visible = false;
            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = false;
            panExam3.Visible = false;
            panExam.Visible = true;

            SetPAN(panExam);

            ssExam01_Sheet1.RowCount = 0;
            ssExam02_Sheet1.RowCount = 0;
            ssExam03_Sheet1.RowCount = 0;

            lblExamName.Text = "";

            GetExamSpecmst();

            lblTitleSub11.Text = ((Button)sender).Text.Trim();
        }

        private void btnExam2_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strSpecNo = "";
            string strStatus = "";
            string strJumin = "";
            string strHicPano = "";
            string strHeaPano = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt1 = null;

            clsPublic.GnLogOutCNT = 0;

            txtExName.Text = "";

            txtJusaMemo.Visible = false;

            panXray.Visible = false;
            ss1.Visible = true;
            panEMG.Visible = false;
            txtResult1.Visible = false;
            panPFT.Visible = false;
            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = false;
            panExam.Visible = true;
            panExam3.Visible = false;

            SetPAN(panExam);

            ssExam01_Sheet1.RowCount = 0;
            ssExam02_Sheet1.RowCount = 0;
            ssExam03_Sheet1.RowCount = 0;

            lblExamName.Text = "";

            strJumin = clsAES.Read_Jumin_AES(clsDB.DbCon, txtPtNo.Text.Trim());

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                //    '등록번호로 일반건진 접수번호를 찾음(일반건진은 SPECMST -> 일반건진 접수번호로 전송)

                SQL = "";
                SQL = "SELECT WRTNO FROM ADMIN.HIC_JEPSU ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO IN (SELECT PANO FROM ADMIN.HIC_PATIENT ";
                SQL = SQL + ComNum.VBLF + "      WHERE JUMIN2 = '" + clsAES.AES(strJumin) + "') ";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                strHicPano = "";

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strHicPano = strHicPano + "'" + dt.Rows[i]["WRTNO"].ToString().Trim() + "',";
                    }

                    if (strHicPano != "")
                    {
                        strHicPano = VB.Left(strHicPano, strHicPano.Length - 1);
                    }
                }
                dt.Dispose();
                dt = null;

                //'종합건진 등록번호 찾음(종검은 SPECTMST -> 종검접수번호로 전송)
                SQL = "";
                SQL = "SELECT WRTNO FROM ADMIN.HEA_JEPSU ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO IN ( SELECT PANO FROM ADMIN.HEA_PATIENT ";
                SQL = SQL + ComNum.VBLF + "      WHERE JUMIN2 = '" + clsAES.AES(strJumin) + "')  ";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                strHeaPano = "";

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strHeaPano = strHeaPano + "'" + ComFunc.LPAD(dt.Rows[i]["WRTNO"].ToString().Trim(), 8, "0") + "',";
                    }

                    if (strHeaPano != "")
                        strHeaPano = VB.Left(strHeaPano, strHeaPano.Length - 1);
                }

                dt.Dispose();
                dt = null;


                //'검체마스타를 SELECT
                SQL = "";
                SQL = "SELECT A.SPECNO,A.DEPTCODE,A.ROOM,A.DRCODE,A.WORKSTS,A.SPECCODE,A.STATUS, ";
                SQL = SQL + ComNum.VBLF + " A.IPDOPD,A.BI,TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.BLOODDATE,'YYYY-MM-DD HH24:MI') BLOODDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI') RECEIVEDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.ORDERDATE,'MM/DD HH24:MI') ORDERDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.RESULTDATE,'MM/DD HH24:MI') RESULTDATE1,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI') RESULTDATE,A.PRINT ";
                SQL = SQL + ComNum.VBLF + " ,(SELECT NAME FROM ADMIN.EXAM_SPECODE WHERE GUBUN = '14' AND CODE = A.SPECCODE) AS SPECCODENAME        ";
                SQL = SQL + ComNum.VBLF + " ,(SELECT WM_CONCAT(TRIM(BB.EXAMNAME))        ";
                SQL = SQL + ComNum.VBLF + "     FROM ADMIN.EXAM_RESULTC AA        ";
                SQL = SQL + ComNum.VBLF + "     INNER JOIN ADMIN.EXAM_MASTER BB        ";
                SQL = SQL + ComNum.VBLF + "         ON AA.MASTERCODE = AA.SUBCODE        ";
                SQL = SQL + ComNum.VBLF + "         AND AA.MASTERCODE = BB.MASTERCODE        ";
                SQL = SQL + ComNum.VBLF + "     WHERE AA.SPECNO = A.SPECNO) AS EXAMNAME        ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_SPECMST A  ";
                SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE  >= TRUNC(SYSDATE-1000) ";
                SQL = SQL + ComNum.VBLF + "   AND A.WORKSTS  IN ('M') ";   //'미생물만;
                SQL = SQL + ComNum.VBLF + "   AND A.STATUS IN ('01','02','03','04','05','14') ";
                SQL = SQL + ComNum.VBLF + "  AND BI NOT IN ('61','62')    ";    //'건진,종검 제외;
                SQL = SQL + ComNum.VBLF + "ORDER BY RECEIVEDATE DESC,SPECNO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssExam01_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSpecNo = dt.Rows[i]["SPECNO"].ToString().Trim();

                        ssExam01_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SPECNO"].ToString().Trim();
                        ssExam01_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RECEIVEDATE"].ToString().Trim()
                        + (dt.Rows[i]["RESULTDATE"].ToString().Trim() != "" ? " (" + Convert.ToDateTime(dt.Rows[i]["RESULTDATE"].ToString().Trim()).ToString("yyyy-MM-dd") + ")" : "");

                        switch (dt.Rows[i]["IPDOPD"].ToString().Trim())
                        {
                            case "I":
                                ssExam01_Sheet1.Cells[i, 2].Text = "입원";
                                break;
                            default:

                                switch (dt.Rows[i]["BI"].ToString().Trim())
                                {
                                    case "61":
                                        ssExam01_Sheet1.Cells[i, 2].Text = "종검";
                                        break;
                                    case "62":
                                        ssExam01_Sheet1.Cells[i, 2].Text = "건진";
                                        break;
                                    case "81":
                                        ssExam01_Sheet1.Cells[i, 2].Text = "수탁";
                                        break;

                                    default:
                                        ssExam01_Sheet1.Cells[i, 2].Text = "외래";
                                        break;

                                }
                                break;
                        }

                        ssExam01_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssExam01_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROOM"].ToString().Trim();
                        ssExam01_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                        ssExam01_Sheet1.Cells[i, 6].Text = dt.Rows[i]["WORKSTS"].ToString().Trim();
                        ssExam01_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SPECCODENAME"].ToString().Trim();
                        ssExam01_Sheet1.Cells[i, 8].Text = dt.Rows[i]["EXAMNAME"].ToString().Trim();

                        switch (dt.Rows[i]["STATUS"].ToString().Trim())
                        {
                            case "00":
                                strStatus = "미접수";
                                break;
                            case "01":
                                strStatus = "검사중";
                                ssExam01_Sheet1.Cells[i, 9].BackColor = Color.Pink;
                                break;
                            case "02":
                                strStatus = "부분입력";
                                ssExam01_Sheet1.Cells[i, 9].BackColor = Color.Pink;
                                break;
                            case "03":
                                strStatus = "모두입력";
                                break;
                            case "04":
                            case "14":
                                strStatus = "부분완료";
                                ssExam01_Sheet1.Cells[i, 9].BackColor = Color.Pink;
                                break;
                            case "05":
                                if (VB.Val(dt.Rows[i]["PRINT"].ToString().Trim()) == 0)
                                    strStatus = "검사완료";
                                if (VB.Val(dt.Rows[i]["PRINT"].ToString().Trim()) > 0)
                                    strStatus = "인쇄완료";
                                break;
                            case "06":
                                strStatus = "취소";
                                break;
                            default:
                                strStatus = "ERROR";
                                break;
                        }

                        ssExam01_Sheet1.Cells[i, 9].Text = strStatus;

                        if (strStatus == "일부완료")
                        {
                            ssExam01_Sheet1.Cells[i, 9].BackColor = Color.LightPink;
                        }
                        else if (strStatus == "검사완료" || strStatus == "인쇄완료")
                        {
                            ssExam01_Sheet1.Cells[i, 9].BackColor = Color.LightGreen;
                        }

                        ssExam01_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ORDERDATE"].ToString().Trim();
                        ssExam01_Sheet1.Cells[i, 11].Text = VB.Right(dt.Rows[i]["RECEIVEDATE"].ToString().Trim(), 11);
                        ssExam01_Sheet1.Cells[i, 12].Text = dt.Rows[i]["RESULTDATE1"].ToString().Trim();
                        ssExam01_Sheet1.Cells[i, 13].Text = dt.Rows[i]["SPECCODE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                if (strHicPano != "")
                {
                    //'/----------------------------------------------------------------------------------------------------------------
                    //'검체마스타를 SELECT
                    //'일반건진

                    SQL = "";
                    SQL = "SELECT SPECNO,DEPTCODE,ROOM,DRCODE,WORKSTS,SPECCODE,STATUS, ";
                    SQL = SQL + ComNum.VBLF + " IPDOPD,BI,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(BLOODDATE,'YYYY-MM-DD HH24:MI') BLOODDATE,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(RECEIVEDATE,'YYYY-MM-DD HH24:MI') RECEIVEDATE,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(ORDERDATE,'MM/DD HH24:MI') ORDERDATE,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(RESULTDATE,'MM/DD HH24:MI') RESULTDATE1,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(RESULTDATE,'YYYY-MM-DD HH24:MI') RESULTDATE,PRINT ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_SPECMST ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPtNo.Text.Trim() + "'  ";
                    SQL = SQL + ComNum.VBLF + "   AND HICNO IN (" + strHicPano + ") ";
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE  = 'HR'";
                    SQL = SQL + ComNum.VBLF + "   AND WORKSTS IN ('M') ";  //   '미생물만;
                    SQL = SQL + ComNum.VBLF + "   AND STATUS IN ('04','14','05') ";
                    SQL = SQL + ComNum.VBLF + "   AND BI='62' ";  //      '건진;
                    SQL = SQL + ComNum.VBLF + "   AND HICNO IS NOT NULL ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY RECEIVEDATE DESC,SPECNO ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {


                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strSpecNo = dt.Rows[i]["SPECNO"].ToString().Trim();

                            ssExam01_Sheet1.Rows.Count = ssExam01_Sheet1.Rows.Count + 1;

                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 0].Text = dt.Rows[i]["SPECNO"].ToString().Trim();
                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 1].Text = dt.Rows[i]["RECEIVEDATE"].ToString().Trim()
                            + Convert.ToDateTime(dt.Rows[i]["RESULTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");

                            switch (dt.Rows[i]["IPDOPD"].ToString().Trim())
                            {
                                case "I":
                                    ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 2].Text = "입원";
                                    break;
                                default:
                                    switch (dt.Rows[i]["BI"].ToString().Trim())
                                    {
                                        case "61":
                                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 2].Text = "종검";
                                            break;
                                        case "62":
                                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 2].Text = "건진";
                                            break;
                                        case "81":
                                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 2].Text = "수탁";
                                            break;
                                        default:
                                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 2].Text = "외래";
                                            break;
                                    }
                                    break;

                            }

                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 4].Text = dt.Rows[i]["ROOM"].ToString().Trim();
                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 5].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 6].Text = dt.Rows[i]["WORKSTS"].ToString().Trim();

                            //'검체구분 조회
                            SQL = "";
                            SQL = "SELECT NAME,YNAME FROM ADMIN.EXAM_SPECODE ";
                            SQL = SQL + ComNum.VBLF + " WHERE GUBUN ='14' ";
                            SQL = SQL + ComNum.VBLF + "   AND  CODE = '" + dt.Rows[i]["SPECCODE"].ToString().Trim() + "' ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 7].Text = dt1.Rows[0]["NAME"].ToString().Trim();
                            }

                            dt1.Dispose();
                            dt1 = null;

                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 8].Text = GetSpecnoExamName(strSpecNo);

                            switch (dt.Rows[i]["STATUS"].ToString().Trim())
                            {
                                case "00":
                                    strStatus = "미접수";
                                    break;
                                case "01":
                                    strStatus = "검사중";
                                    ssExam01_Sheet1.Cells[i, 9].BackColor = Color.Pink;
                                    break;
                                case "02":
                                    strStatus = "부분입력";
                                    ssExam01_Sheet1.Cells[i, 9].BackColor = Color.Pink;
                                    break;
                                case "03":
                                    strStatus = "모두입력";
                                    break;
                                case "04":
                                case "14":
                                    strStatus = "부분완료";
                                    ssExam01_Sheet1.Cells[i, 9].BackColor = Color.Pink;
                                    break;
                                case "05":
                                    if (VB.Val(dt.Rows[i]["PRINT"].ToString().Trim()) == 0)
                                        strStatus = "검사완료";
                                    if (VB.Val(dt.Rows[i]["PRINT"].ToString().Trim()) > 0)
                                        strStatus = "인쇄완료";
                                    break;
                                case "06":
                                    strStatus = "취소";
                                    break;
                                default:
                                    strStatus = "ERROR";
                                    break;
                            }

                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 9].Text = strStatus;

                            if (strStatus == "일부완료")
                            {
                                ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 9].BackColor = Color.LightPink;
                            }
                            else if (strStatus == "검사완료" || strStatus == "인쇄완료")
                            {
                                ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 9].BackColor = Color.LightGreen;
                            }

                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 10].Text = dt.Rows[i]["ORDERDATE"].ToString().Trim();
                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 11].Text = VB.Right(dt.Rows[i]["RECEIVEDATE"].ToString().Trim(), 11);
                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 12].Text = dt.Rows[i]["RESULTDATE1"].ToString().Trim();
                            ssExam01_Sheet1.Cells[ssExam01_Sheet1.Rows.Count - 1, 13].Text = dt.Rows[i]["SPECCODE"].ToString().Trim();
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                lblTitleSub11.Text = ((Button)sender).Text.Trim();

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnExam3_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            txtJusaMemo.Visible = false;

            panPFT.Visible = false;
            panEtc.Visible = false;
            panEMG.Visible = false;
            panXray.Visible = false;
            panExam.Visible = false;
            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = false;


            panExam3.Visible = true;
            nIndex = 4;

            SetPAN(panExam3);


            mstrGubun = "6";
            ssExam3_Sheet1.RowCount = 0;

            GetExam3("A");

            lblTitleSub11.Text = ((Button)sender).Text.Trim();
        }

        private void btnExam3Print_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "검사결과( " + txtPtNo.Text + ":" + lblSName.Text + ")";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("인쇄일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "DM"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssExam3, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
        }

        private void btnExam3S_Click(object sender, EventArgs e)
        {
            GetExam3("B");
        }

        private void btnExamBae_Click(object sender, EventArgs e)
        {
            panExamBae.Visible = false;

        }

        private void btnExit_Click(object sender, EventArgs e) 
        {
            #region //폼을 모달리스로 띄울경우 처리함
            if (mModalCallForm != null)
            {
                rEventClosed(); 
            }
            else
            {
                this.Close();
            }
            #endregion
        }

        private void btnFMInbody_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text.Trim() == "")
                return;

            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;


            txtJusaMemo.Visible = false;


            panPFT.Visible = false;
            panEtc.Visible = false;
            panEMG.Visible = false;
            panXray.Visible = false;
            panExam.Visible = false;
            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = true;
            panExam3.Visible = false;
            nIndex = 4;

            SetPAN(panEtcJupmst);

            txtResult1.Text = "";
            ssAudio_Sheet1.RowCount = 0;



            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE , TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE ,A.GUBUN, A.GBFTP,";
                SQL = SQL + ComNum.VBLF + "  A.AGE, A.SEX, A.GBIO, A.DEPTCODE,   B.ORDERNAME, A.ROWID, A.IMAGE , C.DRNAME  ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ETC_JUPMST A, ADMIN.OCS_ORDERCODE B, ADMIN.BAS_DOCTOR C";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO= '" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.GUBUN IN ('17')  "; //' INBODY;
                SQL = SQL + ComNum.VBLF + "  AND A.GBJOB NOT IN ('9') ";
                SQL = SQL + ComNum.VBLF + "  AND A.DRCODE = C.DRCODE(+) ";
                //' 1미접수 2.예약 3.접수 9.취소);
                SQL = SQL + ComNum.VBLF + " AND A.ORDERCODE= B.ORDERCODE(+)";
                SQL = SQL + ComNum.VBLF + " AND A.DEPTCODE ='FM' ";  //'FM과만;
                SQL = SQL + ComNum.VBLF + " AND A.BDATE <=TRUNC(SYSDATE) "; //'INDEX 태우기위해사용;
                SQL = SQL + ComNum.VBLF + " ORDER BY A.RDATE DESC  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssAudio_Sheet1.RowCount = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssAudio_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["GBIO"].ToString().Trim() == "0" ? "외래" : "입원");
                        ssAudio_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 7].Text = (dt.Rows[i]["IMAGE"].ToString().Trim().Length == 0 ? "" : "▦");

                        if (dt.Rows[i]["GBFTP"].ToString().Trim() == "Y")
                            ssAudio_Sheet1.Cells[i, 7].Text = "▦";// 2014-11-20

                        ssAudio_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();

                        switch (dt.Rows[i]["GUBUN"].ToString().Trim())
                        {
                            case "17":
                                break;
                            case "18":
                                break;
                            case "19":
                                ssAudio_Sheet1.Cells[i, 8].Text = ssAudio_Sheet1.Cells[i, 8].Text + " 말초혈관";
                                break;
                            case "20":
                                ssAudio_Sheet1.Cells[i, 8].Text = ssAudio_Sheet1.Cells[i, 8].Text + " HRV";
                                break;
                        }

                        ssAudio_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                lblTitleSub11.Text = ((Button)sender).Text.Trim();

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnFMStress_Click(object sender, EventArgs e)
        {

            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text.Trim() == "")
                return;

            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            txtJusaMemo.Visible = false;

            panPFT.Visible = false;
            panEtc.Visible = false;
            panEMG.Visible = false;
            panXray.Visible = false;
            panExam.Visible = false;
            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = true;
            panExam3.Visible = false;
            nIndex = 4;

            SetPAN(panEtcJupmst);

            txtResult1.Text = "";


            ssAudio_Sheet1.RowCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE , TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE ,A.GUBUN, A.GBFTP,";
                SQL = SQL + ComNum.VBLF + "  A.AGE, A.SEX, A.GBIO, A.DEPTCODE,   B.ORDERNAME, A.ROWID, A.IMAGE , C.DRNAME  ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ETC_JUPMST A, ADMIN.OCS_ORDERCODE B, ADMIN.BAS_DOCTOR C";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO= '" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.GUBUN IN ('18','19','20')  "; //' INBODY;
                SQL = SQL + ComNum.VBLF + "  AND A.GBJOB NOT IN ('9') ";
                SQL = SQL + ComNum.VBLF + "  AND A.DRCODE = C.DRCODE(+) ";
                //' 1미접수 2.예약 3.접수 9.취소);
                SQL = SQL + ComNum.VBLF + " AND A.ORDERCODE= B.ORDERCODE(+)";
                SQL = SQL + ComNum.VBLF + " AND A.BDATE <=TRUNC(SYSDATE) "; //'INDEX 태우기위해사용;
                SQL = SQL + ComNum.VBLF + " ORDER BY A.RDATE DESC  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssAudio_Sheet1.RowCount = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssAudio_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["GBIO"].ToString().Trim() == "O" ? "외래" : "입원");
                        ssAudio_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 7].Text = (dt.Rows[i]["IMAGE"].ToString().Trim().Length == 0 ? "" : "▦");
                        
                        if (dt.Rows[i]["GBFTP"].ToString().Trim() == "Y")
                            ssAudio_Sheet1.Cells[i, 7].Text = "▦";// 2014-11-20

                        ssAudio_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();

                        switch (dt.Rows[i]["GUBUN"].ToString().Trim())
                        {
                            case "17":
                                break;
                            case "18":
                                break;
                            case "19":
                                ssAudio_Sheet1.Cells[i, 8].Text = ssAudio_Sheet1.Cells[i, 8].Text + " 말초혈관";
                                break;
                            case "20":
                                ssAudio_Sheet1.Cells[i, 8].Text = ssAudio_Sheet1.Cells[i, 8].Text + " HRV";
                                break;
                        }

                        ssAudio_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                lblTitleSub11.Text = ((Button)sender).Text.Trim();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnHic_Click(object sender, EventArgs e)
        {
            //PSMHOCSOLD.clsOcs_old psmhOcsOld = null;
            //psmhOcsOld = new PSMHOCSOLD.clsOcs_old();
            //psmhOcsOld.DbCon();
            //psmhOcsOld.ShowForm_FrmViewHicResult(mstrJumin1 + mstrJumin2);
            //psmhOcsOld.DbDisCon();
            //psmhOcsOld = null;

            comHpcService = new ComHpcService();

            FrmHcResultView = new frmHcResultView("HIC", 0, mstrPano);
            FrmHcResultView.StartPosition = FormStartPosition.CenterParent;
            FrmHcResultView.ShowDialog(this);
        }

        private void btnHea_Click(object sender, EventArgs e)
        {
            //clsPublic.GnLogOutCNT = 0;

            //lblTitleSub11.Text = ((Button)sender).Text.Trim();
            //PSMHOCSOLD.clsOcs_old psmhOcsOld = null;
            //psmhOcsOld = new PSMHOCSOLD.clsOcs_old();
            //psmhOcsOld.DbCon();
            //psmhOcsOld.ShowForm_FrmViewHeaResult(mstrJumin1 + mstrJumin2);
            //psmhOcsOld.DbDisCon();
            //psmhOcsOld = null;

            comHpcService = new ComHpcService();

            List <COMHPC> list = comHpcService.GetHeaJepsuitembyPtno(mstrPano);
            if( list.Count > 0)
            {
                FrmHaResultView = new frmHaResultView(list[0].WRTNO);
                FrmHaResultView.StartPosition = FormStartPosition.CenterParent;
                FrmHaResultView.ShowDialog(this);
            }
        }

        private void btnModifyExit_Click(object sender, EventArgs e)
        {
            panModify.Visible = false;
        }

        private void btnNST_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            SetPAN(panNST);
            ssNSTList_Sheet1.RowCount = 0;

            if (txtPtNo.Text.Trim() == "")
                return;

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT      TO_CHAR(BDATE ,'YYYY-MM-DD') AS BDATE, WRTNO, PANO, IPDNO, ";
                SQL = SQL + ComNum.VBLF + " SNAME, TO_CHAR(INDATE ,'YYYY-MM-DD') AS INDATE, DEPTCODE, DRCODE, ";
                SQL = SQL + ComNum.VBLF + " WARDCODE, ROOMCODE, DIAGNOSIS, DRSABUN, ";
                SQL = SQL + ComNum.VBLF + " NRSABUN, PMSABUN, DTSABUN ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.DIET_NST_PROGRESS ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY BDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssNSTList_Sheet1.RowCount = dt.Rows.Count;
                    ssNSTList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssNSTList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssNSTList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                        ssNSTList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssNSTList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                        ssNSTList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssNSTList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        ssNSTList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssNSTList_Sheet1.Cells[i, 7].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                        ssNSTList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssNSTList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssNSTList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DIAGNOSIS"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                lblTitleSub11.Text = ((Button)sender).Text.Trim();

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnNVC_Click(object sender, EventArgs e)
        {

            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text.Trim() == "")
            {
                return;
            }
            else
            {                
                if (clsType.User.IdNumber == "41827" || clsType.User.IdNumber == "19094" || clsType.User.IdNumber == "30322")
                {
                    frmComSupFnExRSLT01 f = new frmComSupFnExRSLT01("MASTER", txtPtNo.Text.Trim());
                    f.ShowDialog();

                    if (f != null)
                    {
                        f.Dispose();
                        f = null;
                    }
                }
                else
                {
                    frmComSupFnExRSLT01 f = new frmComSupFnExRSLT01("VIEW", txtPtNo.Text.Trim());
                    f.ShowDialog();

                    if (f != null)
                    {
                        f.Dispose();
                        f = null;
                    }
                }                
            }


            //FileInfo di = new FileInfo("c:\\cmc\\ocsexe\\nvcView.Exe");

            //lblTitleSub11.Text = ((Button)sender).Text.Trim();

            //if (di.Exists == false)
            //{
            //    ComFunc.MsgBox("생체현미경 사진조회 프로그램이 설치 안됨", "오류");
            //    return;
            //}

            //#region //2018-10-29 안정수

            //TODO : ini 파일에 정보를 넣어서 프로그램 실행 수정 해야함.
            //int Fn = 0;
            //Fn = FreeFile()
            //Open "c:\nvcView.INI" For Output As #Fn
            //Print #Fn, TxtPtno.Text; "{}"; GnJobSabun
            //Close #Fn

            //Shell "c:\cmc\ocsexe\nvcView.Exe", vbNormalFocus

            //string savePath = @"c:\nvcView.INI";
            //string textValue = txtPtNo.Text.Trim() + "{}" + " " + clsType.User.IdNumber;
            //System.IO.File.WriteAllText(savePath, textValue, Encoding.Default); 

            //VB.Shell("c:\\cmc\\ocsexe\\nvcView.Exe", "NormalFocus");
            //#endregion


        }

        private void btnPFT_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text.Trim() == "")
                return;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;

            txtJusaMemo.Visible = false;

            panPFT.Visible = false;
            panEtc.Visible = false;
            panEMG.Visible = false;
            panXray.Visible = false;
            panExam.Visible = false;
            panEEG.Visible = false;
            panConsult.Visible = false; 
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = true;
            panExam3.Visible = false;
            nIndex = 4;

            SetPAN(panEtcJupmst);

            txtResult1.Text = "";

            mstrGubun = "4";
            ssAudio_Sheet1.RowCount = 0;

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE , TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE ,A.GBFTP, ";
                SQL = SQL + ComNum.VBLF + "  A.AGE, A.SEX, A.GBIO, A.DEPTCODE,   B.ORDERNAME, A.ROWID, A.IMAGE , C.DRNAME  ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ETC_JUPMST A, ADMIN.OCS_ORDERCODE B, ADMIN.BAS_DOCTOR C";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO= '" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.GUBUN ='" + mstrGubun + "' "; //' AUDIO;
                SQL = SQL + ComNum.VBLF + "  AND A.GBJOB NOT IN ('9') ";
                SQL = SQL + ComNum.VBLF + "  AND A.DRCODE = C.DRCODE(+) ";
                //'1미접수 2.예약 3.접수 9.취소);
                SQL = SQL + ComNum.VBLF + " AND A.ORDERCODE= B.ORDERCODE(+)";
                SQL = SQL + ComNum.VBLF + " AND A.BDATE <=TRUNC(SYSDATE) "; //'INDEX 태우기위해사용;
                SQL = SQL + ComNum.VBLF + " ORDER BY A.RDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssAudio_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssAudio_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["GBIO"].ToString().Trim() == "O" ? "외래" : "입원");
                        ssAudio_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssAudio_Sheet1.Cells[i, 7].Text = (VB.Val(dt.Rows[i]["IMAGE"].ToString().Trim()) == 0 ? "" : "▦");

                        if (dt.Rows[i]["GBFTP"].ToString().Trim() == "Y")
                            ssAudio_Sheet1.Cells[i, 7].Text = "▦"; // '2014-11-20;

                        if (mstrGubun == "4")
                        {

                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT";
                            SQL += ComNum.VBLF + "  PTNO";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "ETC_RESULT_PFT";
                            SQL += ComNum.VBLF + "WHERE 1=1";
                            SQL += ComNum.VBLF + "   AND PTNO = '" + txtPtNo.Text.Trim() + "' ";
                            SQL += ComNum.VBLF + "   AND BDATE =TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD')"; 
                            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                dt.Dispose();
                                dt = null;
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt2.Rows.Count == 1)
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " SELECT FEV1_PRED2, FEV1_PRED FROM ADMIN.ETC_RESULT_PFT ";
                                SQL += ComNum.VBLF + "  WHERE PTNO = '" + txtPtNo.Text.Trim() + "' ";
                                SQL += ComNum.VBLF + "    AND BDATE =TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD')";
                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                            }
                            else if(dt2.Rows.Count > 1)
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " SELECT FEV1_PRED2, FEV1_PRED FROM ADMIN.ETC_RESULT_PFT ";
                                SQL += ComNum.VBLF + "  WHERE PTNO = '" + txtPtNo.Text.Trim() + "' ";
                                SQL += ComNum.VBLF + "    AND BDATE =TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD')";
                                SQL += ComNum.VBLF + "    AND RDATE =TO_DATE('" + dt.Rows[i]["RDATE"].ToString().Trim() + "','YYYY-MM-DD')";
                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                            }
                            else if(dt2.Rows.Count == 0)
                            {
                                dt2.Dispose();
                                dt2 = null;
                            }                           

                            if (dt1 != null && dt1.Rows.Count > 0)
                            {
                                if (dt1.Rows[0]["FEV1_PRED2"].ToString().Trim() != "")
                                {

                                    ssAudio_Sheet1.Cells[i, 8].Text = "[FEV1: " + dt1.Rows[0]["FEV1_PRED2"].ToString().Trim()
                                    + "] " + dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                }
                                else
                                {
                                    ssAudio_Sheet1.Cells[i, 8].Text = "[FEV1: " + dt1.Rows[0]["FEV1_PRED"].ToString().Trim()
                                    + "] " + dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                            else
                            {
                                ssAudio_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            }                           
                        }
                        else
                        {
                            ssAudio_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        }
                        ssAudio_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                lblTitleSub11.Text = ((Button)sender).Text.Trim();

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ssPrint.AllowCellOverflow = true;
            Result_Print_Specno_NEW(ssExam02, ssPrint, mstrSpecNo, "OK");
            btnPrint.Enabled = false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssWork">인쇄할 검사결과 내역을 보관할 Work SS_Sheet</param>
        /// <param name="ssWPrint">인쇄할 결과지 FORM 양식</param>
        /// <param name="ArgSpecNo">인쇄할 검체번호</param>
        /// <param name="ArgPrint">인쇄여부(OK:인쇄,NO:인쇄안함)</param>
        private string Result_Print_Specno_NEW(FpSpread ssWork, FpSpread ssWPrint, string ArgSpecNo, string ArgPrint)
        {

            int i = 0;
            int h = 0;
            int g = 0;
            int nRow = 0;
            int nRead = 0;
            int nREAD2 = 0;

            int nWsCNT = 0;
            int nResultSabun = 0;
            string strSpecCode = "";
            string strWS = "";
            int nPrintCnt = 0;

            string strPano = "";
            string strSName = "";
            int nAge = 0;
            string strSex = "";
            string strDeptCode = "";
            string strDeptName = "";
            string strDrname = "";
            string strWard = "";
            string strIpdOpd = "";

            string strSpecName = "";
            string strWsName = "";
            int nSpecPrtCNT = 0; //        '종전까지 결과지 인쇄횟수

            string strMasterCode = "";
            string strSubCode = "";
            string strExamName = "";
            string strFootNote = "";
            string strResult = "";
            string strStatus = "";
            string strRPD = "";
            string strRef = "";
            string strResultName = "";
            string strNewTime = ""; //            '최종작업자 사번 READ용

            string sRefValFrom = "";
            string sRefValTo = "";

            string strList = "";
            string strResultDate = "";

            DataTable dt = null;
            DataTable dt1 = null;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssWork.ActiveSheet.RowCount = 0;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return "NO"; //권한 확인
            }

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return "NO"; //권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                #region Result_Print_Specno_NEW_SUB1 //'검체번호별 검사결과,FootNote,결과사번을 READ

                //'나이 및 성별을 READ
                SQL = "SELECT AGE,SEX FROM ADMIN.EXAM_SPECMST ";
                SQL = SQL + ComNum.VBLF + "WHERE SPECNO = '" + ArgSpecNo + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return "NO";
                }

                if (dt.Rows.Count > 0)
                {
                    nAge = Convert.ToInt32(VB.Val(dt.Rows[0]["AGE"].ToString().Trim()));
                    strSex = dt.Rows[0]["Sex"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;                     

                //'검사결과를 READ
                SQL = "        SELECT M.EXAMNAME, R.SEQNO,R.MASTERCODE, R.SUBCODE, R.RESULT, TO_CHAR(R.RESULTDATE, 'YYYY-MM-DD') RESULTDATE,";
                SQL = SQL + ComNum.VBLF + "         R.STATUS, R.REFER, R.UNIT, R.RESULTSABUN, ";
                SQL = SQL + ComNum.VBLF + "         TO_CHAR(R.RESULTDATE,'YYYY-MM-DD HH24:MI') RESULTTIME ";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EXAM_RESULTC R, ADMIN.EXAM_MASTER M ";
                SQL = SQL + ComNum.VBLF + "   WHERE R.SPECNO= '" + ArgSpecNo + "' ";
                SQL = SQL + ComNum.VBLF + "     AND R.SUBCODE = M.MASTERCODE ";
                SQL = SQL + ComNum.VBLF + "ORDER BY R.SEQNO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    dt.Dispose();
                    dt = null;

                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return "NO";
                }

                if (dt.Rows.Count > 0)
                {
                    nRow = 0;
                    nResultSabun = 0;
                    strNewTime = "";

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strMasterCode = dt.Rows[i]["MASTERCODE"].ToString().Trim();
                        strSubCode = dt.Rows[i]["SUBCODE"].ToString().Trim();
                        strExamName = dt.Rows[i]["EXAMNAME"].ToString().Trim();
                        strResult = dt.Rows[i]["RESULT"].ToString().Trim();
                        strStatus = dt.Rows[i]["STATUS"].ToString().Trim();
                        //2018-12-12 안정수 추가
                        strResultDate = dt.Rows[i]["RESULTDATE"].ToString().Trim();

                        //    'Foot를 Read
                        SQL = "";
                        SQL = "SELECT FOOTNOTE FROM ADMIN.EXAM_RESULTCF ";
                        SQL = SQL + ComNum.VBLF + " WHERE SPECNO= '" + ArgSpecNo + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND SEQNO = " + dt.Rows[i]["SEQNO"].ToString().Trim() + " ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY SORT";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return "NO";
                        }

                        //'결과가 있거나 검사항목이 Head성 이거나 Foot Note가 있다면

                        if (strResult != "" || strStatus == "H" || dt1.Rows.Count > 0 || strMasterCode == strSubCode)
                        {
                            strRPD = dt.Rows[i]["REFER"].ToString().Trim(); //'Reference 결과(H:높음, L:낮음)
                            strRef = GetReference(strSubCode, nAge.ToString("#0"), strSex, strResultDate);


                            //'참고치를 조회하여 출력시에 "H" 또는 "L"를 표시(2002,1,16추가)
                            if (strRef.Trim() != "" && strRPD.Trim() == "")    // '참고치가 없으면 Exit
                            {
                                if (VB.Split(strRef, " ~ ").Length > 1)
                                {
                                    sRefValFrom = VB.Split(strRef, " ~ ")[0].Trim();
                                    sRefValTo = VB.Split(strRef, " ~ ")[1].Trim();

                                    if (VB.Val(strResult) < VB.Val(sRefValFrom))
                                    {
                                        strRPD = "L";
                                    }
                                    else if (VB.Val(strResult) > VB.Val(sRefValTo))
                                    {
                                        strRPD = "H";
                                    }
                                    else
                                    {
                                        strRPD = "";
                                    }

                                    //'참고치를 초과하거나 미달인 검사수치의 "H","L"표시를 EXAM_RESULTC에 저장

                                    if (strRPD != "")
                                    {
                                        Update_EXAM_RESULTC(strRPD, ArgSpecNo, strSubCode);
                                    }
                                }
                            }

                            ssWork.ActiveSheet.RowCount = ssWork.ActiveSheet.RowCount + 1;

                            ssWork.ActiveSheet.Cells[ssWork.ActiveSheet.RowCount - 1, 0].Text = strExamName;//    '검사명
                            ssWork.ActiveSheet.Cells[ssWork.ActiveSheet.RowCount - 1, 1].Text = strResult;//      '결과치
                            ssWork.ActiveSheet.Cells[ssWork.ActiveSheet.RowCount - 1, 2].Text = strRPD;
                            //'결과단위 None는 표시 안함(2000.4.3)
                            ssWork.ActiveSheet.Cells[ssWork.ActiveSheet.RowCount - 1, 3].Text =
                                (dt.Rows[i]["UNIT"].ToString().Trim() != "NONE" ? dt.Rows[i]["UNIT"].ToString().Trim() : "");//'결과단위
                            ssWork.ActiveSheet.Cells[ssWork.ActiveSheet.RowCount - 1, 4].Text = strRef;//                           '참고치

                            //'결과일시가 마지막인 사번을 찾음
                            if (VB.Val(dt.Rows[i]["RESULTSABUN"].ToString().Trim()) > 0)
                            {

                                if (strNewTime != "")
                                {
                                    if (Convert.ToDateTime(dt.Rows[i]["RESULTTIME"].ToString().Trim()) > Convert.ToDateTime(strNewTime))
                                    {
                                        nResultSabun = Convert.ToInt32(VB.Val(dt.Rows[i]["RESULTSABUN"].ToString().Trim()));//'검사자 사번
                                        strNewTime = dt.Rows[i]["RESULTTIME"].ToString().Trim();
                                    }
                                }
                                else
                                {
                                    nResultSabun = Convert.ToInt32(VB.Val(dt.Rows[i]["RESULTSABUN"].ToString().Trim()));//'검사자 사번
                                    strNewTime = dt.Rows[i]["RESULTTIME"].ToString().Trim();
                                }

                            }
                        }

                        //    'Foot Note를 결과지 Sheet에 Move
                        if (dt1.Rows.Count > 0)
                        {
                            for (h = 0; h < dt1.Rows.Count; h++)
                            {
                                ssWork.ActiveSheet.RowCount = ssWork.ActiveSheet.RowCount + 1;

                                ssWork.ActiveSheet.Cells[ssWork.ActiveSheet.RowCount - 1, 0].Text = "   " + dt1.Rows[h]["FOOTNOTE"].ToString().Trim(); // 'Foot Note
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }

                }

                dt.Dispose();
                dt = null;

                //'검체번호당 인쇄할 결과지 장수
                if (ssWork.ActiveSheet.RowCount <= RESULT_ROW_NEW)
                {
                    FnPrintMesu = 1;
                }
                else if (ssWork.ActiveSheet.RowCount <= RESULT_ROW_NEW * 2)
                {
                    FnPrintMesu = 2;
                }
                else if (ssWork.ActiveSheet.RowCount <= RESULT_ROW_NEW * 3)
                {
                    FnPrintMesu = 3;
                }
                else if (ssWork.ActiveSheet.RowCount <= RESULT_ROW_NEW * 4)
                {
                    FnPrintMesu = 4;
                }
                else
                {
                    FnPrintMesu = 5;
                }

                #endregion

                #region Result_Print_Specno_NEW_SUB2 //'인쇄할 결과지양식 Sheet에 인적사항을 Display

                //'~1 부서코드변경

                SQL = "";
                SQL = "SELECT PANO,SNAME,WARD,ROOM,DEPTCODE,DRCODE,AGE,SEX,WORKSTS,SPECCODE,IPDOPD,";
                SQL = SQL + ComNum.VBLF + " DEPTCODE,DRCODE,TO_CHAR(BLOODDATE,'YYMMDD-HH24:MI') BLOODDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(RESULTDATE,'YYMMDD-HH24:MI') RESULTDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(RECEIVEDATE,'YYMMDD-HH24:MI') RECEIVEDATE,PRINT ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_SPECMST ";
                SQL = SQL + ComNum.VBLF + "WHERE SPECNO = '" + ArgSpecNo + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return "NO";
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return "NO";
                }

                //'Work Station을 READ
                strWS = dt.Rows[0]["WORKSTS"].ToString().Trim();

                strWsName = "";

                for (i = 0; i < VB.Split(strWS, ",").Length; i++)
                {
                    SQL = "";
                    SQL = "SELECT NAME FROM ADMIN.EXAM_SPECODE ";
                    SQL = SQL + ComNum.VBLF + "WHERE GUBUN = '12' ";
                    SQL = SQL + ComNum.VBLF + " AND YNAME = '" + VB.Split(strWS, ",")[i] + "' ";
                    SQL = SQL + ComNum.VBLF + " AND CODE LIKE '%0' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return "NO";
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        strWsName = strWsName + VB.Split(dt1.Rows[0]["NAME"].ToString().Trim(), "검사")[0] + ",";
                    }

                    dt1.Dispose();
                    dt1 = null;
                }


                strWsName = VB.Left(strWsName, strWsName.Length - 1); //'마지막 컴마를 제거

                ssWPrint.ActiveSheet.Cells[0, 0].Text = "";

                ssWPrint.ActiveSheet.Cells[0, 1].Text = "";



                if (strWsName.Length < 5)
                {
                    ssWPrint.ActiveSheet.Cells[0, 1].Text = VB.Space(4) + strWsName + " 검사";
                }
                else if (strWsName.Length < 7)
                {
                    ssWPrint.ActiveSheet.Cells[0, 1].Text = VB.Space(3) + strWsName + " 검사";
                }
                else if (strWsName.Length < 15)
                {
                    ssWPrint.ActiveSheet.Cells[0, 1].Text = strWsName + " 검사";
                }
                else
                {
                    ssWPrint.ActiveSheet.Cells[0, 1].Text = strWsName + "검사";
                }

                ssWPrint.ActiveSheet.Cells[37, 4].Text = ssWPrint.ActiveSheet.Cells[37, 4].Text = "(" + strWsName + ")";

                //'등록번호,성명,나이,성별
                strPano = dt.Rows[0]["PANO"].ToString().Trim();
                strSName = dt.Rows[0]["SNAME"].ToString().Trim();
                nAge = Convert.ToInt32(VB.Val(dt.Rows[0]["AGE"].ToString().Trim()));
                strSex = dt.Rows[0]["SEX"].ToString().Trim();
                strList = " 등록번호: " + VB.Left(strPano + VB.Space(10), 10);
                strList = strList + "이  름: " + VB.Left(strSName + VB.Space(13), 13);
                strList = strList + " 나이: " + nAge + "/" + strSex;

                ssWPrint.ActiveSheet.Cells[1, 0].Text = strList;

                //'병동,진료과,진료의사
                strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                strDrname = clsVbfunc.GetOCSDrCodeDrName(clsDB.DbCon, dt.Rows[0]["DrCode"].ToString().Trim());

                if (strDeptCode == "EM" || strDeptCode == "ER")
                {
                    strDrname = "응급실";
                }

                strWard = dt.Rows[0]["WARD"].ToString().Trim() + "-" + VB.Val(dt.Rows[0]["ROOM"].ToString().Trim()).ToString("000");

                if (dt.Rows[0]["IPDOPD"].ToString().Trim() == "0")
                {
                    strWard = "외래";
                }

                //'진료과명을 READ
                SQL = "SELECT DEPTNAMEK FROM ADMIN.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + "WHERE DEPTCODE = '" + strDeptCode + "'";


                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return "NO";
                }

                strDeptName = "";

                if (dt1.Rows.Count > 0)
                {
                    strDeptName = dt1.Rows[0]["DEPTNAMEK"].ToString().Trim(); //        '진료과명
                }

                dt1.Dispose();
                dt1 = null;

                strList = " 병    동: " + VB.Left(strWard + VB.Space(10), 10);
                strList = strList + "진료과: " + VB.Left(strDeptName + VB.Space(13), 13);
                strList = strList + "의사: " + strDrname;

                ssWPrint.ActiveSheet.Cells[2, 0].Text = strList;

                //'검체번호,채취일시,보고일시
                strList = " 번호:" + VB.Left(ArgSpecNo + VB.Space(15), 15);
                strList = strList + "채취일시:" + VB.Left(dt.Rows[0]["BLOODDATE"].ToString().Trim() + VB.Space(14), 14);
                strList = strList + "보고일시:" + dt.Rows[0]["RESULTDATE"].ToString().Trim();

                ssWPrint.ActiveSheet.Cells[35, 0].Text = strList;

                //'검체명,접수일시,검사자
                strSpecCode = dt.Rows[0]["SPECCODE"].ToString().Trim();
                strSpecName = GetBasCode("검체명", strSpecCode);    //'검체명
                strResultName = "검사실";   //'기본을 검사실로 함

                if (nResultSabun > 0)
                {
                    strResultName = clsVbfunc.GetPassName(clsDB.DbCon, nResultSabun.ToString("00000"));
                }

                strList = " 검체:" + VB.Left(strSpecName + VB.Space(15), 15);
                strList = strList + "접수일시:" + VB.Left(dt.Rows[0]["RECEIVEDATE"].ToString().Trim() + VB.Space(14), 14);
                strList = strList + "검사자:" + strResultName.Trim() + " " + "Dr:ESJ";

                ssWPrint.ActiveSheet.Cells[36, 0].Text = strList;


                //TODO '인쇄매수를 READ 
                nSpecPrtCNT = Convert.ToInt32(VB.Val(dt.Rows[0]["PRINT"].ToString().Trim()));


                dt.Dispose();
                dt = null;

                #endregion


                #region Result_Print_Specno_NEW_SUB3  '검사결과지를 프린터로 인쇄

                if (ArgPrint != "OK")//'인쇄를 하지 안으면
                {
                    return "OK";
                }

                nPrintCnt = 0;  //'인쇄매수 Clear
                for (i = 0; i < FnPrintMesu; i++)
                {
                    //'인쇄할 자료를 SSPrint에 Move & 줄그리기
                    Result_Print_DataSet_NEW(ssWork, ssWPrint, i + 1);
                    Result_Print_PRINT_NEW(ssWPrint);

                    ComFunc.Delay(2000);

                    nPrintCnt = nPrintCnt + 1; //'인쇄장수
                }

                //'출력한 횟수를 EXAM_SPECMST에 UPDATE
                if (nSpecPrtCNT < 9)
                {
                    nSpecPrtCNT = nSpecPrtCNT + 1;

                    UPDATE_ExamSpecmst_Print(nSpecPrtCNT, ArgSpecNo);
                }

                if (nPrintCnt == 0)
                {
                    return "NO";
                }
                #endregion

                return "OK";
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return "NO";
            }
        }

         
        private void button1_Click(object sender, EventArgs e)
        {

        }
        public FarPoint.Win.Spread.SheetView CopySheet(FarPoint.Win.Spread.SheetView sheet)
        {
            FarPoint.Win.Spread.SheetView newSheet = null;
            if (sheet != null)
            {
                newSheet = (FarPoint.Win.Spread.SheetView)FarPoint.Win.Serializer.LoadObjectXml(sheet.GetType(), FarPoint.Win.Serializer.GetObjectXml(sheet, "CopySheet"), "CopySheet");
            }
            return newSheet;
        }

        /// <summary>
        /// '출력한 횟수 증가
        /// </summary>
        /// <param name="nSpecPrtCNT"></param>
        /// <param name="argSpecNo"></param>
        private void UPDATE_ExamSpecmst_Print(int nSpecPrtCNT, string argSpecNo)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = "UPDATE ADMIN.EXAM_SPECMST SET PRINT = " + nSpecPrtCNT + " ";
                SQL = SQL + ComNum.VBLF + "WHERE SPECNO = '" + argSpecNo + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함." + ComNum.VBLF + "출력횟수 적용 안됨.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void Result_Print_PRINT_NEW(FpSpread ssWPrint)
        {
            clsSpread SP = new clsSpread();

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            setMargin = new clsSpread.SpdPrint_Margin(0, 0, 0, 0, 0, 0);
            setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Auto, FarPoint.Win.Spread.PrintType.All, 0, 0, false, false, true, false, true, false, false);

            SP.setSpdPrint(ssWPrint, false, setMargin, setOption, "", "", Centering.Horizontal);
        }

        private void Result_Print_DataSet_NEW(FpSpread ssWork, FpSpread ssWPrint, int ArgPage)
        {

            int i = 0;
            int nRow = 0;
            int nSRow = 0;
            int nERow = 0;
            string strJulOK = "";

            //'인쇄할 Sheet를 Clear
            ssWPrint.ActiveSheet.Cells[5, 0, RESULT_ROW_NEW + 4, ssWPrint.ActiveSheet.ColumnCount - 1].Text = "";

            //'인쇄Page Set
            ssWPrint.ActiveSheet.Cells[0, 4].Text = "(" + FnPrintMesu + "/" + ArgPage + ") ";

            nSRow = (ArgPage - 1) * RESULT_ROW_NEW;
            nERow = ArgPage * RESULT_ROW_NEW;

            if (nERow > ssWork.ActiveSheet.RowCount)
            {
                nERow = ssWork.ActiveSheet.RowCount;
            }

            nRow = 0;
            for (i = nSRow; i < nERow; i++)
            {
                ssWPrint.ActiveSheet.Cells[nRow + 5, 0].Text = ssWork.ActiveSheet.Cells[i, 0].Text.Trim();
                ssWPrint.ActiveSheet.Cells[nRow + 5, 1].Text = ssWork.ActiveSheet.Cells[i, 1].Text.Trim();
                ssWPrint.ActiveSheet.Cells[nRow + 5, 2].Text = ssWork.ActiveSheet.Cells[i, 2].Text.Trim();
                ssWPrint.ActiveSheet.Cells[nRow + 5, 3].Text = ssWork.ActiveSheet.Cells[i, 3].Text.Trim();
                ssWPrint.ActiveSheet.Cells[nRow + 5, 4].Text = ssWork.ActiveSheet.Cells[i, 4].Text.Trim();



                strJulOK = "NO";

                if (nRow == 2 || nRow == 5 || nRow == 8)
                {
                    strJulOK = "OK";
                }

                if (nRow == 11 || nRow == 14 || nRow == 17)
                {
                    strJulOK = "OK";
                }

                if (nRow == 20 || nRow == 23)
                {
                    strJulOK = "OK";
                }

                if (i == ssWork.ActiveSheet.RowCount - 1)
                {
                    strJulOK = "OK";
                }

                if (strJulOK == "OK")
                {
                    ssWPrint.ActiveSheet.Cells[nRow + 5, 0].Border = new LineBorder(Color.Black, 1, true, false, false, true);
                    ssWPrint.ActiveSheet.Cells[nRow + 5, 1].Border = new LineBorder(Color.Black, 1, false, false, false, true);
                    ssWPrint.ActiveSheet.Cells[nRow + 5, 2].Border = new LineBorder(Color.Black, 1, false, false, false, true);
                    ssWPrint.ActiveSheet.Cells[nRow + 5, 3].Border = new LineBorder(Color.Black, 1, false, false, false, true);
                    ssWPrint.ActiveSheet.Cells[nRow + 5, 4].Border = new LineBorder(Color.Black, 1, false, false, true, true);
                }
                else
                {

                    ssWPrint.ActiveSheet.Cells[nRow + 5, 0].Border = new LineBorder(Color.Black, 1, true, false, false, false);
                    ssWPrint.ActiveSheet.Cells[nRow + 5, 4].Border = new LineBorder(Color.Black, 1, false, false, true, false);
                }

                nRow = nRow + 1;
            }
        }

        private void Update_EXAM_RESULTC(string strRPD, string argSpecNo, string strSubCode)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE ADMIN.EXAM_RESULTC SET REFER = '" + strRPD + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE SPECNO = '" + argSpecNo + "'";
                SQL = SQL + ComNum.VBLF + "  AND SUBCODE = '" + strSubCode + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("작업도중에 오류가 발생하여 RollBack을 합니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strPano = "";
            string strName = "";
            string strSeekDate = "";
            string strDeptCode = "";
            string strDrCode = "";
            string strDrname = "";
            string strIpdOpd = "";
            string strWardCode = "";
            string strRoomCode = "";
            int nAge = 0;
            string strSex = "";
            string strXJong = "";
            string strGisa = "";
            string strResult = "";
            string strResult1 = "";
            string strResult2 = "";
            int nWRTNO = 0;
            int nDRWRTNO = 0;
            string strREADDATE = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            string strXCode = "";
            string strXName = "";

            //'다중판독 관련
            bool bolChk = false;
            string strxReadF = "";
            string strDept = "";
            string strXOk = "";
            int nXCnt = 0;

            int i = 0;
            int intRowAffected = 0;
            string SQL = "";
            string strChar = "";

            //'저장권한체크 2016-09-20

            if (READ_XrayResult_CHK(clsType.User.IdNumber) == false)
            {
                ComFunc.MsgBox("판독 권한(의사, 판독의)이 없습니다..!!", "확인");
                return;
            }




            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                READ_XRAY_DETAIL(ref strPano, ref strName, ref strSeekDate, ref strDeptCode, ref strDrCode, ref strDrname,
                 ref strIpdOpd, ref strWardCode, ref strRoomCode, ref nAge, ref strSex, ref strXJong, ref strXCode, ref strGisa, ref strXName, ref SQL);


                #region Result_Edit_Process '2000Byte단위로 짜름

                strResult = txtResult.Text.Trim();
                strResult = clsVbfunc.QuotationChange(strResult);  //'문장중 "'" => "`"로 변경

                // '2014-02-13 산부인과 초음파 신규 처방의판독시
                if (strXJong == "G" && mintDrWrtno == 0)
                {
                    strResult = strResult + ComNum.VBLF + ComNum.VBLF + "Dr." + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);
                }

                strResult1 = "";
                strResult2 = "";

                if (ComFunc.LenH(strResult) < 2001)
                {
                    strResult1 = strResult;
                }
                else
                {
                    //'Result1 Setting
                    strResult1 = ComFunc.LeftH(strResult, 2000);
                    strChar = VB.Right(strResult1, 1);
                    // '한글 반글자가 짤리면
                    if ((String.Compare(strChar, " ") < 0 || String.Compare(strChar, "~") > 0) && ComFunc.LenH(strChar) == 1)
                    {
                        strResult1 = ComFunc.LeftH(strResult1, Convert.ToInt32(ComFunc.LenH(strResult1) - 1));
                    }

                    strResult = ComFunc.RightH(strResult, Convert.ToInt32(ComFunc.LenH(strResult) - ComFunc.LenH(strResult1)));

                    if (ComFunc.LenH(strResult) < 2001)
                    {
                        strResult2 = strResult;
                    }
                    else
                    {
                        // 'Result2 Setting

                        strResult2 = ComFunc.LeftH(strResult, 2000);
                        strChar = VB.Right(strResult2, 1);
                        // '한글 반글자가 짤리면

                        if ((String.Compare(strChar, " ") < 0 || String.Compare(strChar, "~") > 0) && ComFunc.LenH(strChar) == 1)
                        {
                            strResult2 = ComFunc.LeftH(strResult2, Convert.ToInt32(ComFunc.LenH(strResult2) - 1));
                        }
                    }
                }

                #endregion

                if (lblTitleSub0.Text.Trim() == "처방의 판독")
                {

                    if (chkMulti.Checked == true)
                    {
                        for (i = 0; i < ss2_Sheet1.RowCount; i++)
                        {
                            bolChk = Convert.ToBoolean(ss2_Sheet1.Cells[i, 0].Value);

                            strxReadF = ss2_Sheet1.Cells[i, 3].Text.Trim();
                            strXOk = ss2_Sheet1.Cells[i, 5].Text.Trim();
                            strDept = ss2_Sheet1.Cells[i, 8].Text.Trim();

                            mstrROWID = ss2_Sheet1.Cells[i, 11].Text.Trim();

                            //'건당 검사읽기
                            READ_XRAY_DETAIL(ref strPano, ref strName, ref strSeekDate, ref strDeptCode, ref strDrCode, ref strDrname,
                            ref strIpdOpd, ref strWardCode, ref strRoomCode, ref nAge, ref strSex, ref strXJong, ref strXCode, ref strGisa, ref strXName, ref SQL);

                            //'선택된것중 과 os 영상있고, 처방의판독 안된것 만대상 - 다중 저장
                            if (bolChk == true && strDept == "OS" && strXOk == "▦" && strxReadF == "")
                            {
                                nXCnt = nXCnt + 1;

                                if (mintDrWrtno == 0)
                                {
                                    if (CmdOK_INSERT_RTN_DR(ref nDRWRTNO, ref strPano, ref strREADDATE, ref strSeekDate, ref strXJong,
                                     ref strName, ref strSex, ref nAge, ref strIpdOpd, ref strDeptCode, ref strDrCode, ref strWardCode,
                                      ref strRoomCode, ref strXCode, ref strXName, ref strResult1, ref strResult2, ref intRowAffected, ref SQL) == false)
                                    {
                                        return;
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        if (mintDrWrtno == 0)
                        {
                            if (CmdOK_INSERT_RTN_DR(ref nDRWRTNO, ref strPano, ref strREADDATE, ref strSeekDate, ref strXJong,
                                     ref strName, ref strSex, ref nAge, ref strIpdOpd, ref strDeptCode, ref strDrCode, ref strWardCode,
                                      ref strRoomCode, ref strXCode, ref strXName, ref strResult1, ref strResult2, ref intRowAffected, ref SQL) == false)
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (CmdOK_UPDATE_RTN_DR(ref strREADDATE, ref strXCode, ref strXName, ref strResult1, ref strResult2, ref intRowAffected, ref SQL) == false)
                            {
                                return;
                            }
                        }
                    }
                }
                else if (lblTitleSub0.Text.Trim() == "영상의학 판독")
                {
                    if (mintWrtno <= 1000)
                    {
                        if (CmdOK_INSERT_RTN(ref nWRTNO, ref strPano, ref strREADDATE, ref strSeekDate, ref strXJong, ref strName,
                        ref strSex, ref nAge, ref strIpdOpd, ref strDeptCode, ref strDrCode, ref strWardCode, ref strRoomCode, ref strXCode,
                        ref strXName, ref strResult1, ref strResult2, ref intRowAffected, ref SQL) == false)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (CmdOK_UPDATE_Rtn(ref strXCode, ref strXName, ref strResult1, ref strResult2, ref strREADDATE, ref intRowAffected, ref SQL) == false)
                        {
                            return;

                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                btnXray.PerformClick();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

        }

        private bool CmdOK_UPDATE_Rtn(ref string strXCode, ref string strXName, ref string strREADDATE,
        ref string strResult1, ref string strResult2, ref int intRowAffected, ref string SQL)
        {
            //'내용을 변경 저장    
            bool bolVal = false;
            string SqlErr = "";

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return bolVal; //권한 확인
            }
            SQL = "";
            SQL = "UPDATE ADMIN.XRAY_RESULTNEW SET ";
            SQL = SQL + ComNum.VBLF + " READDATE=TO_DATE('" + strREADDATE + "','YYYY-MM-DD'), ";
            SQL = SQL + ComNum.VBLF + " READTIME=TO_DATE('" + strREADDATE + " " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "D") + "','YYYY-MM-DD HH24:MI'),";  //'2015-05-27;
            SQL = SQL + ComNum.VBLF + " XCODE='" + strXCode + "',";
            SQL = SQL + ComNum.VBLF + " XNAME='" + strXName + "',";
            SQL = SQL + ComNum.VBLF + " RESULT='" + strResult1 + "',";
            SQL = SQL + ComNum.VBLF + " RESULT1='" + strResult2 + "',";
            SQL = SQL + ComNum.VBLF + " APPROVE='Y',";
            SQL = SQL + ComNum.VBLF + " SENDEMR = NULL,";
            SQL = SQL + ComNum.VBLF + " ENTDATE=SYSDATE ";
            SQL = SQL + ComNum.VBLF + " WHERE WRTNO=" + mintWrtno + " ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("판독결과를 UPDATE시 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return bolVal;
            }

            //'PACS 전송용 TABLE에 INSERT(판독변경);
            SQL = "";
            SQL = "INSERT INTO ADMIN.XRAY_PACSSEND (ENTDATE,PACSNO,SENDGBN,PANO,SNAME,";
            SQL = SQL + ComNum.VBLF + "SEX,AGE,IPDOPD,DEPTCODE,DRCODE,WARDCODE,ROOMCODE,XJONG,XSUBCODE,";
            SQL = SQL + ComNum.VBLF + "XCODE,ORDERCODE,SEEKDATE,REMARK,XRAYROOM,READNO,GBINFO) ";
            SQL = SQL + ComNum.VBLF + "SELECT SYSDATE,PACSNO,'5',PANO,SNAME,";
            SQL = SQL + ComNum.VBLF + "       SEX,AGE,IPDOPD,DEPTCODE,DRCODE,WARDCODE,ROOMCODE,XJONG,XSUBCODE,";
            SQL = SQL + ComNum.VBLF + "       XCODE,ORDERCODE,SEEKDATE,REMARK,XRAYROOM,EXINFO,GBINFO ";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.XRAY_DETAIL ";
            SQL = SQL + ComNum.VBLF + "   WHERE ROWID = '" + mstrROWID + "' ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("PACS 전송용 Table에 INSERT시 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return bolVal;
            }
            return true;
        }

        private bool CmdOK_INSERT_RTN(ref int nWRTNO, ref string strPano, ref string strREADDATE, ref string strSeekDate,
         ref string strXJong, ref string strName, ref string strSex, ref int nAge, ref string strIpdOpd, ref string strDeptCode,
         ref string strDrCode, ref string strWardCode, ref string strRoomCode, ref string strXCode, ref string strXName,
          ref string strResult1, ref string strResult2, ref int intRowAffected, ref string SQL)
        {
            bool bolVal = false;
            DataTable dt = null;
            string SqlErr = "";

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return bolVal; //권한 확인
            }

            //'새로운 WRTNO를 부여함

            SQL = "SELECT ADMIN.SEQ_XRAYREAD.NEXTVAL XRAYWRTNO FROM DUAL";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return bolVal;
            }

            if (dt.Rows.Count > 0)
            {
                nWRTNO = Convert.ToInt32(dt.Rows[0]["XRAYWRTNO"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            //'자료를 INSERT

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return bolVal; //권한 확인
            }

            SQL = "INSERT INTO ADMIN.XRAY_RESULTNEW (WRTNO,PANO,READDATE,READTIME,SEEKDATE,XJONG,SNAME,SEX,AGE,";
            SQL = SQL + ComNum.VBLF + "IPDOPD,DEPTCODE,DRCODE,WARDCODE,ROOMCODE,XDRCODE1,XDRCODE2,XDRCODE3,";
            SQL = SQL + ComNum.VBLF + "ILLCODE1,ILLCODE2,ILLCODE3,XCODE,XNAME,RESULT,RESULT1,ENTDATE,APPROVE) VALUES ";
            SQL = SQL + ComNum.VBLF + "(" + nWRTNO + ",";
            SQL = SQL + ComNum.VBLF + "'" + strPano + "',";
            SQL = SQL + ComNum.VBLF + "TO_DATE('" + strREADDATE + "','YYYY-MM-DD'),";
            SQL = SQL + ComNum.VBLF + "TO_DATE('" + strREADDATE + " " + ComFunc.FormatStrToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T") + "','YYYY-MM-DD HH24:MI'),";
            SQL = SQL + ComNum.VBLF + "TO_DATE('" + strSeekDate + "','YYYY-MM-DD'),";
            SQL = SQL + ComNum.VBLF + "'" + strXJong + "',";
            SQL = SQL + ComNum.VBLF + "'" + strName.Trim() + "',";
            SQL = SQL + ComNum.VBLF + "'" + strSex + "',";
            SQL = SQL + ComNum.VBLF + nAge + ",";
            SQL = SQL + ComNum.VBLF + "'" + strIpdOpd + "',";
            SQL = SQL + ComNum.VBLF + "'" + strDeptCode + "',";
            SQL = SQL + ComNum.VBLF + "'" + strDrCode + "',";
            SQL = SQL + ComNum.VBLF + "'" + strWardCode.Trim() + "',";
            SQL = SQL + ComNum.VBLF + "'" + strRoomCode.Trim() + "',";
            SQL = SQL + ComNum.VBLF + "'" + mstrDrCode1 + "',";
            SQL = SQL + ComNum.VBLF + "'','','','','', ";
            SQL = SQL + ComNum.VBLF + "'" + strXCode + "',";
            SQL = SQL + ComNum.VBLF + "'" + strXName + "',";
            SQL = SQL + ComNum.VBLF + "'" + strResult1 + "',";
            SQL = SQL + ComNum.VBLF + "'" + strResult2 + "',";
            SQL = SQL + ComNum.VBLF + "SYSDATE,'Y') ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("판독결과를 INSERT시 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return bolVal;
            }

            //'XRAY_DETAIL에 판독번호를 UPDATE
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return bolVal; //권한 확인
            }

            SQL = "UPDATE ADMIN.XRAY_DETAIL SET EXINFO=" + nWRTNO + " ";
            SQL = SQL + ComNum.VBLF + " WHERE ROWID IN ( '" + mstrROWID + "'" + " ) ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("방사선 촬영내역에 판독번호 UPDATE시 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return bolVal;
            }

            //'PACS 전송용 Table에 INSERT(판독)

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return bolVal; //권한 확인
            }

            SQL = "INSERT INTO ADMIN.XRAY_PACSSEND (ENTDATE,PACSNO,SENDGBN,PANO,SNAME,";
            SQL = SQL + ComNum.VBLF + "SEX,AGE,IPDOPD,DEPTCODE,DRCODE,WARDCODE,ROOMCODE,XJONG,XSUBCODE,";
            SQL = SQL + ComNum.VBLF + "XCODE,ORDERCODE,SEEKDATE,REMARK,XRAYROOM,READNO,GBINFO) ";
            SQL = SQL + ComNum.VBLF + "SELECT SYSDATE,PACSNO,'5',PANO,SNAME,";
            SQL = SQL + ComNum.VBLF + "       SEX,AGE,IPDOPD,DEPTCODE,DRCODE,WARDCODE,ROOMCODE,XJONG,XSUBCODE,";
            SQL = SQL + ComNum.VBLF + "       XCODE,ORDERCODE,SEEKDATE,REMARK,XRAYROOM,EXINFO,GBINFO ";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.XRAY_DETAIL ";
            SQL = SQL + ComNum.VBLF + " WHERE ROWID IN ( '" + mstrROWID + "'" + " ) ";
            SQL = SQL + ComNum.VBLF + "   AND PACSNO IS NOT NULL ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("PACS 전송용 Table에 INSERT시 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return bolVal;
            }
            return true;
        }

        private bool CmdOK_UPDATE_RTN_DR(ref string strREADDATE, ref string strXCode, ref string strXName, ref string strResult1,
         ref string strResult2, ref int intRowAffected, ref string SQL)
        {
            bool bolVal = false;
            string SqlErr = ""; //에러문 받는 변수

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return bolVal; //권한 확인
            }

            SQL = "";
            SQL = "UPDATE ADMIN.XRAY_RESULTNEW_DR SET ";
            SQL = SQL + " READDATE=TO_DATE('" + strREADDATE + "','YYYY-MM-DD'), ";
            SQL = SQL + " XCODE ='" + strXCode + "',";
            SQL = SQL + " XNAME ='" + strXName + "',";
            SQL = SQL + " RESULT ='" + strResult1 + "',";
            SQL = SQL + " RESULT1 ='" + strResult2 + "',";
            SQL = SQL + " APPROVE ='Y',";
            SQL = SQL + " ENTDATE =SYSDATE ";
            SQL = SQL + " WHERE DRWRTNO=" + mintDrWrtno + " ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("판독결과를 UPDATE중 오류가 발생함.", "오류발생");
                Cursor.Current = Cursors.Default;
                return bolVal;
            }

            return true;
        }

        private bool CmdOK_INSERT_RTN_DR(ref int nDRWRTNO, ref string strPano, ref string strREADDATE, ref string strSeekDate, ref string strXJong,
         ref string strName, ref string strSex, ref int nAge, ref string strIpdOpd, ref string strDeptCode, ref string strDrCode, ref string strWardCode,
          ref string strRoomCode, ref string strXCode, ref string strXName, ref string strResult1, ref string strResult2, ref int intRowAffected, ref string SQL)
        {
            bool bolVal = false;

            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            SQL = "SELECT ADMIN.SEQ_XRAYREAD_DR.NEXTVAL XRAYWRTNO FROM DUAL";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return bolVal;
            }

            if (dt.Rows.Count > 0)
            {
                nDRWRTNO = Convert.ToInt32(VB.Val(dt.Rows[0]["XRAYWRTNO"].ToString().Trim()));
            }

            dt.Dispose();
            dt = null;

            //'자료를 INSERT
            SQL = "INSERT INTO ADMIN.XRAY_RESULTNEW_DR (DRWRTNO,PANO,READDATE,SEEKDATE,XJONG,SNAME,SEX,AGE,";
            SQL = SQL + ComNum.VBLF + "IPDOPD,DEPTCODE,DRCODE,WARDCODE,ROOMCODE,XDRCODE1,XDRCODE2,XDRCODE3,";
            SQL = SQL + ComNum.VBLF + "ILLCODE1,ILLCODE2,ILLCODE3,XCODE,XNAME,RESULT,RESULT1,ENTDATE,APPROVE)";
            SQL = SQL + ComNum.VBLF + " VALUES";
            SQL = SQL + ComNum.VBLF + "(" + nDRWRTNO + ",";
            SQL = SQL + ComNum.VBLF + "'" + strPano + "',";
            SQL = SQL + ComNum.VBLF + "TO_DATE('" + strREADDATE + "','YYYY-MM-DD'),";
            SQL = SQL + ComNum.VBLF + "TO_DATE('" + strSeekDate + "','YYYY-MM-DD'),";
            SQL = SQL + ComNum.VBLF + "'" + strXJong + "',";
            SQL = SQL + ComNum.VBLF + "'" + strName.Trim() + "',";
            SQL = SQL + ComNum.VBLF + "'" + strSex + "',";
            SQL = SQL + ComNum.VBLF + nAge + ",";
            SQL = SQL + ComNum.VBLF + "'" + strIpdOpd + "',";
            SQL = SQL + ComNum.VBLF + "'" + strDeptCode + "',";
            SQL = SQL + ComNum.VBLF + "'" + strDrCode + "',";
            SQL = SQL + ComNum.VBLF + "'" + strWardCode.Trim() + "',";
            SQL = SQL + ComNum.VBLF + "'" + strRoomCode.Trim() + "',";
            SQL = SQL + ComNum.VBLF + "'" + mstrDrCode1 + "',";
            SQL = SQL + ComNum.VBLF + "'','','','','',";
            SQL = SQL + ComNum.VBLF + "'" + strXCode + "', ";
            SQL = SQL + ComNum.VBLF + "'" + strXName + "',";
            SQL = SQL + ComNum.VBLF + "'" + strResult1 + "',";
            SQL = SQL + ComNum.VBLF + "'" + strResult2 + "',";
            SQL = SQL + ComNum.VBLF + "SYSDATE,'Y') ";

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return bolVal; //권한 확인
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("판독결과를 INSERT중 오류가 발생함.", "오류발생");
                Cursor.Current = Cursors.Default;
                return bolVal;
            }

            //'XRAY_DETAIL에 판독번호를 UPDATE
            SQL = "";
            SQL = "UPDATE ADMIN.XRAY_DETAIL";
            SQL = SQL + ComNum.VBLF + " SET DRWRTNO =" + nDRWRTNO + " ";
            SQL = SQL + ComNum.VBLF + " WHERE ROWID IN ( '" + mstrROWID + "'" + " ) ";

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return bolVal; //권한 확인
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("방사선 촬영내역에 판독번호 UPDATE 중 오류가 발생함.", "오류발생");
                Cursor.Current = Cursors.Default;
                return bolVal;
            }

            return true;
        }

        private void READ_XRAY_DETAIL(ref string strPano, ref string strName, ref string strSeekDate, ref string strDeptCode,
         ref string strDrCode, ref string strDrname, ref string strIpdOpd, ref string strWardCode, ref string strRoomCode,
          ref int nAge, ref string strSex, ref string strXJong, ref string strXCode, ref string strGisa, ref string strXName, ref string SQL)
        {
            string SqlErr = "";
            DataTable dt = null;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            SQL = "     SELECT PANO,SNAME,DEPTCODE,DRCODE,IPDOPD,WARDCODE,";
            SQL = SQL + ComNum.VBLF + "      ROOMCODE,XJONG,XCODE,REMARK,EXID,SEX,AGE, XCODE,";
            SQL = SQL + ComNum.VBLF + "      TO_CHAR(SEEKDATE,'YYYY-MM-DD') AS SEEKDATE,EXINFO,EXID,";
            SQL = SQL + ComNum.VBLF + "      PACSNO,PACSSTUDYID,ORDERNAME,ROWID ";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.XRAY_DETAIL ";
            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + mstrROWID + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                strPano = dt.Rows[0]["PANO"].ToString().Trim();
                strName = dt.Rows[0]["SNAME"].ToString().Trim();
                strSeekDate = dt.Rows[0]["SEEKDATE"].ToString().Trim();
                strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                strDrCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                strDrname = clsVbfunc.GetBASDoctorName(clsDB.DbCon, strDrCode);
                strIpdOpd = dt.Rows[0]["IPDOPD"].ToString().Trim();
                strWardCode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                strRoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                nAge = Convert.ToInt32(dt.Rows[0]["AGE"].ToString().Trim());
                strSex = dt.Rows[0]["SEX"].ToString().Trim();
                strXJong = dt.Rows[0]["XJONG"].ToString().Trim();
                strXCode = dt.Rows[0]["XCODE"].ToString().Trim();

                strGisa = dt.Rows[0]["EXID"].ToString().Trim();

                dt.Dispose();
                dt = null;

                SQL = "SELECT XNAME FROM ADMIN.XRAY_CODE ";
                SQL = SQL + ComNum.VBLF + "WHERE XCODE = '" + strXCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 1)
                {
                    strXName = dt.Rows[0]["XNAME"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;
        }

        private bool READ_XrayResult_CHK(string idNumber)
        {
            bool bolVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strSaBun = "";

            try
            {
                if (idNumber.Length <= 5)
                {
                    strSaBun = ComFunc.LPAD(idNumber.Trim(), 5, "0");
                }

                //'의사체크;
                SQL = "SELECT DRNAME FROM ADMIN.OCS_DOCTOR";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN ='" + strSaBun + "' ";
                SQL = SQL + ComNum.VBLF + " AND GBOUT='N' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    bolVal = true;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    SQL = "SELECT KORNAME FROM ADMIN.INSA_MST ";
                    SQL = SQL + ComNum.VBLF + " WHERE SABUN =" + strSaBun + " ";
                    SQL = SQL + ComNum.VBLF + " AND JIKJONG IN ('15') ";
                    SQL = SQL + ComNum.VBLF + " AND (TOIDAY IS NULL OR TOIDAY ='') ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return bolVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.Dispose();
                        dt = null;
                        bolVal = true;
                    }

                    dt.Dispose();
                    dt = null;
                }

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return bolVal;
        }

        private void btnSixWalk_Click(object sender, EventArgs e)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수    

            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text.Trim() == "")
                return;

            SetPAN(panSixWalk);

            ClearSheetSixwalk();

            ssSixWalkList_Sheet1.RowCount = 0;

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT TO_CHAR(BDATE, 'YYYY-MM-DD') AS BDATE, PTNO, SNAME, AGE, SEX, EXAMNAME, TO_CHAR(WRITEDATE, 'YYYY-MM-DD HH24:MI') WRITEDATE, ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ETC_SIXMINWARK ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + txtPtNo.Text.Trim() + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY BDATE, PTNO, SNAME ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssSixWalkList_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssSixWalkList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssSixWalkList_Sheet1.Cells[i, 1].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["EXAMNAME"].ToString().Trim());
                        ssSixWalkList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                lblTitleSub11.Text = ((Button)sender).Text.Trim();

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ClearSheetSixwalk()
        {
            ssSixWalk_Sheet1.Cells[2, 2, 6, 2].Text = "";
            ssSixWalk_Sheet1.Cells[2, 5].Text = "";

            ssSixWalk_Sheet1.Cells[9, 3, 9, 5].Text = "";

            ssSixWalk_Sheet1.Cells[10, 3, 10, 5].Text = "";

            ssSixWalk_Sheet1.Cells[11, 3, 11, 5].Text = "";

            ssSixWalk_Sheet1.Cells[12, 3, 12, 5].Text = "";

            ssSixWalk_Sheet1.Cells[13, 3, 13, 5].Text = "";

            ssSixWalk_Sheet1.Cells[16, 1].Text = "";
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text.Trim() == "")
                return;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;
            DataTable dt = null;

            txtJusaMemo.Visible = false;

            panEtc.Visible = true;
            panXray.Visible = false;
            ss1.Visible = true;
            panEMG.Visible = false;
            txtResult1.Visible = true;
            panExam.Visible = false;
            panPFT.Visible = false;
            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = false;
            panExam3.Visible = false;
            nIndex = 5;

            SetPAN(panEtc);

            txtResult1.Text = "";

            ss1_Sheet1.RowCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE,  WARD, ROOM, DEPTCODE  ,A.ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_VERIFY A ";
                SQL = SQL + ComNum.VBLF + " WHERE A.PANO  = '" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.STATUS ='3'"; // '결과 완료된것;
                SQL = SQL + ComNum.VBLF + "   AND A.RESULTDATE IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.JDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["JDATE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = "종합검증 결과보고서" + dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                lblTitleSub11.Text = ((Button)sender).Text.Trim();

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnWordEntry_Click(object sender, EventArgs e)
        {
            //TODO : FrmUseWard 폼 있으면 붙치기~~
            //'F:\VB60_NEW\XRAY\XUREAD\XUREAD05.FRM
            frmUseWard frm = new frmUseWard();
            frm.ShowDialog();

            SetXResultWord(); //'상용단어를 SET
        }

        private void ssExam01_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
                return;
            if (ssExam01_Sheet1.RowCount == 0)
                return;

            clsPublic.GnLogOutCNT = 0;
        }

        private void ssXrayDr_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true || e.RowHeader == true)
                return;
            if (ssXrayDr_Sheet1.RowCount == 0)
                return;

            string strMsg = "";

            strMsg = ssXrayDr_Sheet1.Cells[e.Row, 1].Text.Trim();

            strMsg = VB.InputBox("처방의 판독 내용확인!!", "처방의 판독", strMsg);

            txtResult.Text = strMsg;

            if (strMsg != "")
            {
                btnSave.PerformClick();
            }
            else
            {
                ComFunc.MsgBox("판독내용 없음 !! 저장안됨!!");
            }
        }

        private void ssAudio_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
                return;
            if (ssAudio_Sheet1.RowCount == 0)
                return;

            clsPublic.GnLogOutCNT = 0;


            string strROWID = "";
            string strResul = "";
            string strImageGbn = "";

            strResul = ssAudio_Sheet1.Cells[e.Row, 7].Text.Trim();
            strROWID = ssAudio_Sheet1.Cells[e.Row, 9].Text.Trim();
            strImageGbn = ssAudio_Sheet1.Cells[e.Row, 10].Text.Trim();

            if (strResul != "")
            {
                //    '파일 다온로드 '파일 실행
                if (mstrGubun == "1" && strImageGbn == "")
                {
                    ECGFILE_DBToFile(strROWID, txtPtNo.Text.Trim(), "1");
                }
                else
                {
                    ETC_FILE_DBToFile(strROWID, txtPtNo.Text.Trim(), "1");
                }
            }
        }

        private void pic2_DoubleClick(object sender, EventArgs e)
        {
            //TODO : 디버깅후 수정 해야함.
            //If Picture1.Left = 4680 Then
            //   Picture1.Left = 120
            //   Picture1.Width = 12735
            //Else
            //   Picture1.Left = 4680
            //   Picture1.Width = 8145
            //End If
        }

        private void ssExam01_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true || e.RowHeader == true)
                return;
            if (ssExam01_Sheet1.RowCount == 0)
                return;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            clsPublic.GnLogOutCNT = 0;

            int i = 0;
            int k = 0;
            string strSpecNo = "";
            string strRef = "";
            string strResultDate = "";   //'결과일자
            string strStatus = "";   //'상태
            string strResult = "";   //'결과
            string strOK = "";   //'Display여부
            string strSpecCode = "";

            string strResultOK = "NO";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            DataTable dt = null;
            DataTable dt1 = null;

            clsDB.setBeginTran(clsDB.DbCon);


            try
            {

                strSpecNo = ssExam01_Sheet1.Cells[e.Row, 0].Text.Trim();
                strSpecCode = ssExam01_Sheet1.Cells[e.Row, 13].Text.Trim();

                ssExam02.Height = panel14.Height - 48;

                ssExam02_Sheet1.RowCount = 0;
                ssExam03_Sheet1.RowCount = 0;
                lblExamName.Text = "";

                mstrSpecNo = strSpecNo;

                //'(JJY2004-11-19)

                switch (clsType.User.IdNumber)
                {
                    case "4349":
                    case "7834":
                    case "7843":
                    case "13537":
                    case "12306":
                    case "468":
                    case "2749":
                    case "15273":
                    case "19399":
                    case "21181":
                    case "2186":
                    case "12576":
                    case "13662":
                    case "21430":
                    case "13635":
                    case "28253":
                    case "8822":
                    case "22456":
                    case "35481":
                    case "37074":
                    case "39005":
                    case "38320":
                    case "44974":
                    case "27120": //김영숙(적정관리실장)
                    case "19684": //박시철(원무과)
                    case "18266": //전정훈(원무과)
                    case "20175": //함종현(원무과) '2009 - 08 - 31
                    case "11701": //정규복(원무과)
                    case "17812": //오영선(심사과)
                    case "29635": //박은향(원무과)
                    case "22699": //김진숙(심사과)
                    case "33674": //우영란(심사과)
                    case "27176": //이은주(심사과)
                    case "45450": //(심사과)     '2018 - 04 - 10
                    case "46000": //(심사과)     '2018 - 04 - 10
                    case "50773": //(심사과)     '2020 - 04 - 08
                    case "41827": //Test
                        btnPrint.Enabled = true;
                        break;
                    default:
                        //2020-11-18 임시로 권한 열어둠
                        btnPrint.Enabled = true;
                        break;

                }

                if (NurseSystemManagerCheck(clsType.User.IdNumber) == true)
                {
                    btnPrint.Enabled = true;
                }

                SQL = "";
                SQL = "SELECT R.STATUS,R.MASTERCODE,R.SUBCODE, R.RESULT, R.REFER, R.PANIC, R.IMGWRTNO,  ";
                SQL = SQL + ComNum.VBLF + " R.DELTA, R.UNIT, R.SEQNO, M.EXAMNAME, TO_CHAR(R.RESULTDATE,'YYYY-MM-DD') RESULTDATE ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_RESULTC R, ADMIN.EXAM_MASTER M ";
                SQL = SQL + ComNum.VBLF + "WHERE SPECNO='" + strSpecNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND R.SUBCODE = M.MASTERCODE(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY R.SEQNO ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("자료가 1건도 없습니다.", "확인");
                    return;
                }

                //'기존에 선택한 Row의 BackColor를 지움
                //'Select a block of cells
                //'Determine the color of background, foreground and border color
                if (mintExamRow < ssExam01_Sheet1.RowCount)
                {
                    ssExam01_Sheet1.Cells[mintExamRow, 1, mintExamRow, 8].ForeColor = Color.FromArgb(0, 0, 0);
                    ssExam01_Sheet1.Cells[mintExamRow, 1, mintExamRow, 8].BackColor = Color.FromArgb(255, 255, 255);
                }

                //SS_Exam1.CellBorderColor = RGB(0, 0, 0)

                //'현재 클릭한 Row의 BackColor를 변경
                //'Select a block of cells
                ssExam01_Sheet1.Cells[e.Row, 1, e.Row, 8].ForeColor = Color.FromArgb(0, 0, 0);
                ssExam01_Sheet1.Cells[e.Row, 1, e.Row, 8].BackColor = Color.FromArgb(255, 255, 128);

                mintExamRow = e.Row;

                ssExam02_Sheet1.RowCount = 0;


                //2018-12-07 안정수, 이도경s 요청으로 검사완료건인지 체크하는 로직 추가                
                strResultOK = ResultStatusCheck(strSpecNo);

                for (i = 0; i < dt.Rows.Count; i++)
                {                    
                    strResultDate = dt.Rows[i]["RESULTDATE"].ToString().Trim();
                    strStatus = dt.Rows[i]["STATUS"].ToString().Trim();

                    //2018-12-07 안정수, 검사완료일 경우 OK, OK인경우만 결과 보이도록 
                    if (strResultOK == "OK")
                    {
                        strResult = dt.Rows[i]["RESULT"].ToString().Trim();
                    }
                    else
                    {
                        strResult = "-< 검사중 >-";
                    }

                    if (strStatus == "H")
                    {
                        strOK = "OK";
                    }
                    else if (strStatus == "V")
                    {
                        strOK = "OK";
                        if (strResult == "")
                            strOK = "NO";
                        if (dt.Rows[i]["MASTERCODE"].ToString().Trim() == dt.Rows[i]["SUBCODE"].ToString().Trim())
                            strOK = "OK";
                    }
                    else
                    {
                        strOK = "OK";
                        strResult = "-< 검사중 >-";

                        SQL = "";
                        SQL = " SELECT PANO FROM ADMIN.EXAM_RESULT_BAE ";
                        SQL = SQL + ComNum.VBLF + " WHERE SPECNO = '" + strSpecNo + "' ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            dt.Dispose();
                            dt = null;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            strResult = "-    < 중간결과 >-";
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }

                    if (strOK == "OK")
                    {
                        ssExam02_Sheet1.RowCount = ssExam02_Sheet1.RowCount + 1;
                        ssExam02_Sheet1.Cells[ssExam02_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["EXAMNAME"].ToString();  //검사이름
                        //'SS_Exam2.Text = AdoGetString(rs, "Result", i)      '결과치
                        //'允(2006-08-22) 검사실에서 verfy안한것은 검사중으로 표시하도록합니다.
                        //'아래 루틴수정신 전계장에게 협의후 수정 요망.
                        ssExam02_Sheet1.Cells[ssExam02_Sheet1.RowCount - 1, 1].Text = strResult;

                        ssExam02_Sheet1.Cells[ssExam02_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["REFER"].ToString().Trim();
                        ssExam02_Sheet1.Cells[ssExam02_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["UNIT"].ToString().Trim();  //'결과단위

                        strRef = GetReference(dt.Rows[i]["SUBCODE"].ToString().Trim(), Convert.ToString(mintAge), mstrSex, strResultDate);

                        ssExam02_Sheet1.Cells[ssExam02_Sheet1.RowCount - 1, 4].Text = strRef; //'참고치
                        ssExam02_Sheet1.Cells[ssExam02_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["SUBCODE"].ToString().Trim(); //검사이름
                        ssExam02_Sheet1.Cells[ssExam02_Sheet1.RowCount - 1, 6].Text = strSpecCode; //'검사구분


                        //'이미지 장수 표시
                        if (VB.Val(dt.Rows[i]["IMGWRTNO"].ToString().Trim()) > 0)
                        {
                            SQL = "";
                            SQL = " SELECT COUNT(*) CNT ";
                            SQL = SQL + ComNum.VBLF + "FROM  ADMIN.EXAM_RESULT_IMG ";
                            SQL = SQL + ComNum.VBLF + " WHERE WRTNO = '" + dt.Rows[i]["IMGWRTNO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + " AND SPECNO = '" + strSpecNo + "' ";
                            SQL = SQL + ComNum.VBLF + " AND SUBCODE = '" + dt.Rows[i]["SUBCODE"].ToString().Trim() + "' ";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                dt.Dispose();
                                dt = null;
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                ssExam02_Sheet1.Cells[ssExam02_Sheet1.RowCount - 1, 7].Text = "▦" + dt1.Rows[0]["CNT"].ToString().Trim() + "장";
                            }
                            dt1.Dispose();
                            dt1 = null;
                        }

                        ssExam02_Sheet1.Cells[ssExam02_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["IMGWRTNO"].ToString().Trim(); //'검사구분

                        if (dt.Rows[i]["REFER"].ToString().Trim() != "")
                        {
                            ssExam02_Sheet1.Rows.Get(ssExam02_Sheet1.RowCount - 1).BackColor = Color.FromArgb(189, 255, 189);
                        }

                        if (GetResultcHis(strSpecNo) == true)
                        {
                            ssExam02_Sheet1.Cells[ssExam02_Sheet1.RowCount - 1, 9].Text = "◎";
                        }
                    }

                    //'혈액배양중간결과
                    if (strResult == "-    < 중간결과 >-" || i == 0)
                    {
                        SQL = " SELECT TO_CHAR(ENTDATE,'YYYY/MM/DD HH24:MI') ENTDATE , RESULT  FROM ADMIN.EXAM_RESULT_BAE ";
                        SQL = SQL + ComNum.VBLF + " WHERE SPECNO = '" + strSpecNo + "' ";

                        if (i == 0)
                        {
                            SQL = SQL + ComNum.VBLF + "    AND ( SUBCODE IS NULL  OR  SUBCODE ='" + dt.Rows[i]["SUBCODE"].ToString().Trim() + "' ) ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "    AND SUBCODE = '" + dt.Rows[i]["SUBCODE"].ToString().Trim() + "' ";
                        }
                        SQL = SQL + ComNum.VBLF + "   ORDER BY ENTDATE ASC ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            dt.Dispose();
                            dt = null;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            strResult = "-    < 중간결과 >-";

                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                ssExam02_Sheet1.RowCount = ssExam02_Sheet1.RowCount + 1;

                                ssExam02_Sheet1.Cells[ssExam02_Sheet1.RowCount - 1, 1].Text = "  " + dt1.Rows[k]["RESULT"].ToString().Trim();
                                ssExam02_Sheet1.Cells[ssExam02_Sheet1.RowCount - 1, 1].BackColor = Color.SkyBlue;
                                ssExam02_Sheet1.Cells[ssExam02_Sheet1.RowCount - 1, 4].Text = dt1.Rows[k]["ENTDATE"].ToString().Trim();
                            }
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }

                    //'Foot Note를 READ
                    SQL = " SELECT FOOTNOTE FROM ADMIN.EXAM_RESULTCF ";
                    SQL = SQL + ComNum.VBLF + "WHERE SPECNO = '" + strSpecNo + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SEQNO =  " + dt.Rows[i]["SEQNO"].ToString().Trim() + "  ";
                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        dt.Dispose();
                        dt = null;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        strOK = "OK";

                        for (k = 0; k < dt1.Rows.Count; k++)
                        {
                            ssExam02_Sheet1.RowCount = ssExam02_Sheet1.RowCount + 1;

                            ssExam02_Sheet1.Cells[ssExam02_Sheet1.RowCount - 1, 0].Text = "  " + dt1.Rows[k]["FOOTNOTE"].ToString().Trim();
                            ssExam02_Sheet1.Cells[ssExam02_Sheet1.RowCount - 1, 0].BackColor = Color.SkyBlue;
                        }
                    }
                    dt1.Dispose();
                    dt1 = null;

                    if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) //권한 확인
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("DB에 Update 권한이 없습니다.");
                        return; //권한 확인
                    }


                    SQL = "";
                    SQL = "UPDATE ADMIN.EXAM_RESULTC_CV SET CHKDATE = SYSDATE, CHKSABUN = '" + clsType.User.IdNumber + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE SPECNO = '" + strSpecNo + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND CHKDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + "   AND GBN IN ('1','2') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                }
                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(clsDB.DbCon);
                lblExamName.Text = "검체번호:" + strSpecNo;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        string ResultStatusCheck(string argSpecno)
        {
            string rtnVal = "NO";
            string SQL = "";
            string SqlErr = "";
            DataTable dt2 = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT STATUS";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_SPECMST";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND SPECNO = '" + argSpecno + "'";
            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {                
                dt2.Dispose();
                dt2 = null;
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return rtnVal; 
            }

            if ( dt2.Rows.Count > 0)
            {
                if(dt2.Rows[0]["STATUS"].ToString().Trim() == "05" || dt2.Rows[0]["STATUS"].ToString().Trim() == "04")
                {
                    rtnVal = "OK";
                }
                else
                {
                    rtnVal = "NO";
                }
            }

            dt2.Dispose();
            dt2 = null;

            return rtnVal;
        }

        private bool GetResultcHis(string strSpecNo)
        {
            bool bolVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.EXAM_HISRESULTC ";
                SQL = SQL + ComNum.VBLF + "WHERE SPECNO ='" + strSpecNo + "' ";
                SQL = SQL + ComNum.VBLF + "     AND JOBGBN <> '3' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    bolVal = true;
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return bolVal;
        }

        private bool NurseSystemManagerCheck(string strSaBun)
        {
            //'2014-02-03 주임 김현욱 추가
            //'간호부 관리자 보다 한단계 높은 권한을 주는 부분입니다.
            //'일반적으로 프로그램 변경사항, 전산실 연습 등을 미리 볼수 있는 권한입니다.(예전 고경자 과장 권한 기준)
            //'모든 수간호사가 다 봐야한다면 NUR_MANAGER_CHECK를 이용하시고
            //'좀 더 높은 권한(간호부 과장급)의 경우에 사용하시면 되겠습니다.

            bool bolVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT CODE";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_시스템관리자'";
                SQL = SQL + ComNum.VBLF + "   AND CODE=" + strSaBun + " ";
                SQL = SQL + ComNum.VBLF + "      AND DELDATE IS NULL";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (VB.Val(dt.Rows[0]["CODE"].ToString().Trim()) > 0)
                    {
                        bolVal = true;
                    }
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return bolVal;
        }

        private void ssExam02_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
                return;
            if (ssExam02_Sheet1.RowCount == 0)
                return;

            clsPublic.GnLogOutCNT = 0;

            string strCODE = "";
            string strSpecCode = "";
            string strResult = "";
            string strImgWrtno = "";
            string strModify = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;
            DataTable dt = null;

            panModify.Visible = false;
            panExamBae.Visible = false;

            strCODE = ssExam02_Sheet1.Cells[e.Row, 5].Text.Trim();
            strSpecCode = ssExam02_Sheet1.Cells[e.Row, 6].Text.Trim();
            strImgWrtno = ssExam02_Sheet1.Cells[e.Row, 8].Text.Trim();
            strModify = ssExam02_Sheet1.Cells[e.Row, 9].Text.Trim();

            lblExamName.Text = strCODE + ":" + ssExam02_Sheet1.Cells[e.Row, 0].Text.Trim();
            strResult = ssExam02_Sheet1.Cells[e.Row, 1].Text.Trim();

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                //'자료를 SELECT;
                SQL = "";
                SQL = "SELECT TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI') RECEIVEDATE,B.RESULT ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_SPECMST A,ADMIN.EXAM_RESULTC B ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PANO='" + txtPtNo.Text.Trim() + "' ";

                if(clsPublic.GstrDeptCode == "MR")
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.BDATE  >= TRUNC(SYSDATE-5000) ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.BDATE  >= TRUNC(SYSDATE-1000) ";
                }
                
                SQL = SQL + ComNum.VBLF + "  AND A.WORKSTS NOT IN ('A','T') "; // '세포학,조직학은 제외;
                SQL = SQL + ComNum.VBLF + "  AND A.STATUS IN ('14','05') "; //'임상병리과;
                SQL = SQL + ComNum.VBLF + "  AND A.SPECNO=B.SPECNO(+) ";
                SQL = SQL + ComNum.VBLF + "  AND B.SUBCODE='" + strCODE + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.SPECCODE = '" + strSpecCode + "' "; //'검체종류별로 조회;
                SQL = SQL + ComNum.VBLF + "  AND A.BI NOT IN ('61','62')    ";// '건진,종검 제외;
                SQL = SQL + ComNum.VBLF + "ORDER BY A.RECEIVEDATE DESC,A.SPECNO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssExam03_Sheet1.RowCount = 0;

                if (dt.Rows.Count > 0)
                {
                    ssExam03_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssExam03_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RECEIVEDATE"].ToString().Trim();
                        ssExam03_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RESULT"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                //'혈액배양검사;
                SQL = "";
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, ";
                SQL = SQL + ComNum.VBLF + "   TO_CHAR(ENTDATE,'YYYY-MM-DD') ENTDATE, ";
                SQL = SQL + ComNum.VBLF + " RESULT FROM ADMIN.EXAM_RESULT_BAE ";
                SQL = SQL + ComNum.VBLF + " WHERE SPECNO  = '" + mstrSpecNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + txtPtNo.Text.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssEaxmBae_Sheet1.RowCount = 0;

                if (dt.Rows.Count > 0)
                {
                    ssEaxmBae_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssEaxmBae_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                        ssEaxmBae_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RESULT"].ToString().Trim();
                    }
                    panExamBae.Visible = true;
                }
                else
                {
                    panExamBae.Visible = false;
                }

                if (VB.Val(strImgWrtno) > 0)
                {
                    Exam_FILE_DBToFile(strImgWrtno, txtPtNo.Text.Trim(), "1");
                }

                if (strModify == "◎")
                {

                    ssExModify_Sheet1.RowCount = 0;
                    panModify.Visible = true;
                    GetResultcHisDisplay(mstrSpecNo);
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void ssExam02_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
                return;
            if (ssExam02_Sheet1.RowCount == 0)
                return;

            clsPublic.GnLogOutCNT = 0;

            string strCODE = "";
            string strSpecCode = "";
            string strResult = "";
            string strImgWrtno = "";
            string strModify = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;
            DataTable dt = null;

            panModify.Visible = false;
            panExamBae.Visible = false;

            strCODE = ssExam02_Sheet1.Cells[e.Row, 5].Text.Trim();
            strSpecCode = ssExam02_Sheet1.Cells[e.Row, 6].Text.Trim();
            strImgWrtno = ssExam02_Sheet1.Cells[e.Row, 8].Text.Trim();
            strModify = ssExam02_Sheet1.Cells[e.Row, 9].Text.Trim();

            lblExamName.Text = strCODE + ":" + ssExam02_Sheet1.Cells[e.Row, 0].Text.Trim();
            strResult = ssExam02_Sheet1.Cells[e.Row, 1].Text.Trim();

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                //'자료를 SELECT;
                SQL = "";
                SQL = "SELECT TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI') RECEIVEDATE,B.RESULT ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_SPECMST A,ADMIN.EXAM_RESULTC B ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PANO='" + txtPtNo.Text.Trim() + "' ";

                if (clsPublic.GstrDeptCode == "MR")
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.BDATE  >= TRUNC(SYSDATE-5000) ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.BDATE  >= TRUNC(SYSDATE-1000) ";
                }

                SQL = SQL + ComNum.VBLF + "  AND A.WORKSTS NOT IN ('A','T') "; // '세포학,조직학은 제외;
                SQL = SQL + ComNum.VBLF + "  AND A.STATUS IN ('14','05') "; //'임상병리과;
                SQL = SQL + ComNum.VBLF + "  AND A.SPECNO=B.SPECNO(+) ";
                SQL = SQL + ComNum.VBLF + "  AND B.SUBCODE='" + strCODE + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.SPECCODE = '" + strSpecCode + "' "; //'검체종류별로 조회;
                SQL = SQL + ComNum.VBLF + "  AND A.BI NOT IN ('61','62')    ";// '건진,종검 제외;
                SQL = SQL + ComNum.VBLF + "ORDER BY A.RECEIVEDATE DESC,A.SPECNO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssExam03_Sheet1.RowCount = 0;

                if (dt.Rows.Count > 0)
                {
                    ssExam03_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssExam03_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RECEIVEDATE"].ToString().Trim();
                        ssExam03_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RESULT"].ToString().Trim();
                    }
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void GetResultcHisDisplay(string strSpecNo)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssExModify_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT M.EXAMNAME,TO_CHAR(R.JOBDATE,'YYYY-MM-DD HH24:MI') JOBDATE, R.JOBSABUN,  ";
                SQL = SQL + ComNum.VBLF + " R.JOBGBN, R.RESULT,R.SAYU  ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_HISRESULTC R, ADMIN.EXAM_MASTER M ";
                SQL = SQL + ComNum.VBLF + "WHERE SPECNO='" + strSpecNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND R.SUBCODE = M.MASTERCODE(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY R.SEQNO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssExModify_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssExModify_Sheet1.Cells[i, 0].Text = dt.Rows[i]["EXAMNAME"].ToString().Trim();
                        ssExModify_Sheet1.Cells[i, 1].Text = (dt.Rows[i]["JOBGBN"].ToString().Trim() == "1" ? "전" : "후");
                        ssExModify_Sheet1.Cells[i, 2].Text = dt.Rows[i]["JOBDATE"].ToString().Trim();
                        ssExModify_Sheet1.Cells[i, 3].Text = dt.Rows[i]["RESULT"].ToString().Trim();
                        ssExModify_Sheet1.Cells[i, 4].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["JOBSABUN"].ToString().Trim());
                        ssExModify_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SAYU"].ToString().Trim();
                    }
                }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
                return;
            if (ss1_Sheet1.RowCount == 0)
                return;

            clsPublic.GnLogOutCNT = 0;

            string strROWID = "";
            string strSeekDate = "";
            string strUID = "";
            string strPano = "";

            string strTemp = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;



            //SS1.Row = Row: SS1.Col = 5
            strROWID = ss1_Sheet1.Cells[e.Row, 4].Text.Trim();
            panEtcCVR.Visible = false;    //'2015-07-01

            //'2015-07-01
            strTemp = ss1_Sheet1.Cells[e.Row, 7].Text.Trim();
            mstrAnatno = ss1_Sheet1.Cells[e.Row, 8].Text.Trim();

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                if (mstrAnatno != "" && strTemp == "◎")
                {
                    panEtcCVR.Visible = true;
                    strPano = txtPtNo.Text.Trim();
                    SQL = "";
                    SQL = " SELECT CREMARK1,CREMARK2 FROM ADMIN.EXAM_ANATMST ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND ANATNO = '" + mstrAnatno + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        txtCVR.Text = VB.Pstr(dt.Rows[0]["CREMARK1"].ToString().Trim(), ".", 2);
                        txtCVR2.Text = dt.Rows[0]["CREMARK2"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;
                }

                switch (nIndex)
                {
                    case 1:
                        SelectXray(e.Row);//'xray
                        break;
                    case 2:
                        SelectEndo(e.Row);//'내시경
                        break;
                    case 3:
                        SelectBiopsy(e.Row);
                        break;
                    case 4:
                        SelectCytology(e.Row);
                        break;
                    case 5:
                        SelectVerify(e.Row);
                        break;
                }

                //'PACS 영상을 Display

                if (e.Column == 2)
                {
                    //    SS1.Row = Row
                    //    SS1.Col = 1:        strSeekDate = Trim(SS1.Text)
                    strSeekDate = ss1_Sheet1.Cells[e.Row, 0].Text.Trim();
                    strTemp = ss1_Sheet1.Cells[e.Row, 2].Text.Trim();
                    strUID = ss1_Sheet1.Cells[e.Row, 8].Text.Trim();

                    if (strUID != "" && strTemp == "▦")
                    {
                        strPano = txtPtNo.Text.Trim();
                        //'공용모듈 vb60_new\basefile\VB인피니트.bas 에 있음
                        if (chkSunTeg.Checked == false)
                            strUID = "";

                        clsPacs.PACS_Image_View(clsDB.DbCon, strPano, strUID, clsType.User.IdNumber);
                    }

                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
                return;
            if (ss1_Sheet1.RowCount == 0)
                return;

            string strTemp = "";   //'2015-07-01
            string strROWID = "";
            string strSeekDate = "";
            string strUID = "";
            string strPano = "";

            strROWID = ss1_Sheet1.Cells[e.Row, 4].Text.Trim();
            panEtcCVR.Visible = false; //'2015-07-01

            strTemp = ss1_Sheet1.Cells[e.Row, 7].Text.Trim();
            mstrAnatno = ss1_Sheet1.Cells[e.Row, 8].Text.Trim();

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                if (strUID != "" && strTemp == "◎")
                {
                    panEtcCVR.Visible = true;
                    strPano = txtPtNo.Text.Trim();

                    SQL = "";
                    SQL = " SELECT CREMARK1,CREMARK2 FROM ADMIN.EXAM_ANATMST ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND ANATNO = '" + mstrAnatno + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        txtCVR.Text = VB.Pstr(dt.Rows[0]["CREMARK1"].ToString().Trim(), ".", 2);
                        txtCVR2.Text = dt.Rows[0]["CREMARK2"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }

                switch (nIndex)
                {
                    case 1:
                        SelectXray(e.Row);
                        break;
                    case 2:
                        SelectEndo(e.Row);
                        break;
                    case 3:
                        SelectBiopsy(e.Row);
                        break;
                    case 4:
                        SelectCytology(e.Row);
                        break;
                }

                //'PACS 영상을 Display
                if (e.Column == 2)
                {
                    //    ss1.Row = Row

                    strSeekDate = ss1_Sheet1.Cells[e.Row, 0].Text.Trim();
                    strUID = ss1_Sheet1.Cells[e.Row, 8].Text.Trim();

                    if (strUID != "")
                    {
                        strPano = txtPtNo.Text.Trim();
                        if (chkSunTeg.Checked == true)
                            strUID = "";

                        clsPacs.PACS_Image_View(clsDB.DbCon, strPano, strUID, clsType.User.IdNumber);
                    }

                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ss2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
                return;
            if (ss2_Sheet1.RowCount == 0)
                return;

            clsPublic.GnLogOutCNT = 0;

            SetXResultWord(); //'상용단어를 SET

            int i = 0;
            int intRow = 0;
            int intWRTNO = 0;
            string strUID = "";
            string strSeekDate = "";
            string strXJong = "";
            string strOK = "";

            string strPano = "";
            string strPacsNo = "";
            string strEmgWrtno = "";

            bool bolHIC = false;
            bool bolPACSChange = false;

            string strMsg = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            lblSTS.Visible = false;  //'2015-05-19

            txtResult.Text = "";
            txtAddendum.Text = "";

            if (ss2_Sheet1.Cells[e.Row, 9].Text.Trim() == "건진")
            {
                bolHIC = true;
            }

            mstrROWID = ss2_Sheet1.Cells[e.Row, 11].Text.Trim();
            strEmgWrtno = ss2_Sheet1.Cells[e.Row, 21].Text.Trim();

            chkMulti.Visible = false;

            try
            {
                if (e.Column == 3)
                {
                    chkMulti.Visible = true;

                    if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                        return; //권한 확인

                    //'기존의 자료를 읽음;
                    SQL = "";
                    SQL = "SELECT CODE,WARDNAME,ROWID ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.XRAY_RESULTWARD ";
                    SQL = SQL + ComNum.VBLF + "WHERE SABUN=" + clsType.User.IdNumber + " ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            switch (dt.Rows[i]["CODE"].ToString().Trim())
                            {
                                case "F1":
                                    intRow = 0;
                                    break;
                                case "F2":
                                    intRow = 1;
                                    break;
                                case "F3":
                                    intRow = 2;
                                    break;
                                case "F4":
                                    intRow = 3;
                                    break;
                                case "F5":
                                    intRow = 4;
                                    break;
                                case "F6":
                                    intRow = 5;
                                    break;
                                case "F7":
                                    intRow = 6;
                                    break;
                                case "F8":
                                    intRow = 7;
                                    break;
                                case "F9":
                                    intRow = 8;
                                    break;
                                case "F10":
                                    intRow = 9;
                                    break;
                            }
                            ssXrayDr_Sheet1.Cells[intRow, 1].Text = dt.Rows[i]["WARDNAME"].ToString().Trim();
                        }
                    }
                    dt.Dispose();
                    dt = null;

                    lblTitleSub0.Text = "처방의 판독";
                    intWRTNO = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[e.Row, 20].Text.Trim()));

                    mintDrWrtno = intWRTNO;
                    strXJong = ss2_Sheet1.Cells[e.Row, 15].Text.Trim();

                    strOK = "NO";

                    if (bolHIC == false)
                    {
                        if (intWRTNO > 0)
                            SelectXrayDR(intWRTNO);

                        if (intWRTNO == 0)
                            strOK = "OK";

                        if (mstrDrCode == mstrDrCode1)
                            strOK = "OK";
                    }

                    if (strOK == "OK")
                    {
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        btnWordEntry.Visible = true;
                        txtResult.ReadOnly = false;
                        ssXrayDr.Visible = true;
                        txtResult.Focus();
                    }
                    else
                    {
                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                        btnWordEntry.Visible = false;
                        txtResult.BackColor = Color.White;
                        txtResult.ReadOnly = true;
                        ssXrayDr.Visible = false;
                    }

                } 

                if (e.Column == 4)
                {

                    if (clsPacs.ChkPacsLogin(clsDB.DbCon, clsType.User.IdNumber, "XRESULT") == false)//'vb60_new_basefile\VbOrder_Login_Chk.bas
                    {
                        strMsg = "";
                        intWRTNO = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[e.Row, 13].Text.Trim()));

                        if (bolHIC == true)
                        {
                            //--건진
                            //-- RESULT1 == "" 판독중....

                            SQL = "SELECT RESULT1       ";
                            SQL = SQL + ComNum.VBLF + " FROM ADMIN.HIC_XRAY_RESULT        ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + mstrROWID + "'      ";
                        }
                        else
                        {
                            //--일반
                            //-- APPROVE == "N" 판독중
                            //--TEMP == "N" 판독중
                            if (intWRTNO < 1000)
                            {
                                ComFunc.MsgBox("판독결과 권한이 없습니다..!!"
                                + ComNum.VBLF + ComNum.VBLF + "▶ 판독 정보가 없습니다. ◀", "확인");
                                return;
                            }

                            SQL = "SELECT APPROVE, TEMP";
                            SQL = SQL + ComNum.VBLF + " FROM ADMIN.XRAY_RESULTNEW";
                            SQL = SQL + ComNum.VBLF + "WHERE WRTNO = " + Convert.ToString(intWRTNO);
                        }

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {

                            if (bolHIC == true)
                            {
                                if (dt.Rows[0]["RESULT1"].ToString().Trim() == "")
                                {

                                    strMsg = "▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                                }
                                else
                                {
                                    strMsg = "▶ 판독 완료되었습니다. ◀";
                                }
                            }
                            else
                            {
                                if (dt.Rows[0]["APPROVE"].ToString().Trim() == "N")
                                {
                                    strMsg = "▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                                }
                                else if (dt.Rows[0]["TEMP"].ToString().Trim() == "Y")
                                {
                                    strMsg = "▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                                }
                                else
                                {
                                    strMsg = "▶ 판독 완료되었습니다. ◀";
                                }
                            }
                        }
                        else
                        {
                            strMsg = "▶ 판독 정보가 없습니다. ◀";
                        }

                        dt.Dispose();
                        dt = null;


                        ComFunc.MsgBox("판독결과 권한이 없습니다..!!"
                                + ComNum.VBLF + ComNum.VBLF + strMsg, "확인");
                        return;
                    }

                    lblTitleSub0.Text = "영상의학 판독";
                    intWRTNO = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[e.Row, 13].Text.Trim()));
                    mintWrtno = intWRTNO;

                    strPacsNo = ss2_Sheet1.Cells[e.Row, 14].Text.Trim();
                    strXJong = ss2_Sheet1.Cells[e.Row, 15].Text.Trim();

                    strPano = txtPtNo.Text.Trim();

                    if (bolHIC == true)
                    {
                        Select_Xray_HIC(mstrROWID);
                    }
                    else
                    {
                        if (intWRTNO > 1000)
                            SelectXray(intWRTNO);
                        strOK = "NO";

                        switch (clsType.User.IdNumber)
                        {
                            case "23515":
                            case "18435":
                            case "20110":
                            case "20273":

                                switch (strXJong)
                                {
                                    case "A":
                                    case "3":
                                    case "4":
                                    case "5":
                                    case "2":
                                        if (intWRTNO < 1000)
                                            strOK = "OK";
                                        if (mstrDrCode == mstrDrCode1)
                                            strOK = "OK";
                                        break;
                                }
                                break;
                        }

                        //'외주판독 2015-05-19
                        if (VB.Val(mstrDrCode) >= 99001 && VB.Val(mstrDrCode) <= 99099)
                        {
                            lblSTS.Visible = true;
                        }

                        if (strOK == "OK")
                        {
                            btnSave.Visible = true;
                            btnCancel.Visible = true;
                            btnWordEntry.Visible = true;
                            txtResult.ReadOnly = false;
                            ssXrayDr.Visible = true;
                        }
                        else
                        {
                            btnSave.Visible = false;
                            btnCancel.Visible = false;
                            btnWordEntry.Visible = false;
                            txtResult.BackColor = Color.White;
                            txtResult.ReadOnly = true;
                            ssXrayDr.Visible = false;
                        }

                        //'인피니트 팍스 DB - 판독갱신 2012-03-14
                        clsPacs.SET_XRAY_READ_UPDATE_INFINITT(clsDB.DbCon, strPano, strPacsNo);   //'vb60_new\basefile\vb인피니트.bas
                    }
                }
                else if (e.Column >= 5) //'영상조회
                {
                    ssXrayDr.Visible = false;

                    if (clsPacs.ChkPacsLogin(clsDB.DbCon, clsType.User.IdNumber, "MVIEW") == false)    //'vb60_new_basefile\VbOrder_Login_Chk.bas
                    {
                        ComFunc.MsgBox("판독결과 권한이 없습니다..!!", "확인");
                        return;
                    }


                    if (bolHIC == true)//'건진
                    {
                        strUID = ss2_Sheet1.Cells[e.Row, 12].Text.Trim();
                        if (strUID != "")
                        {
                            //SS2_PACS_VIEW_HIC: '건진

                            strSeekDate = ss2_Sheet1.Cells[e.Row, 1].Text.Trim();
                            strXJong = "1";
                            strPacsNo = ss2_Sheet1.Cells[e.Row, 12].Text.Trim() + " ";

                            //'건진결과에서 PACS를 변경하였는지 점검
                            SQL = "";
                            SQL = "SELECT Pano,PTno,GbConv FROM ADMIN.HIC_XRAY_RESULT ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID='" + mstrROWID + "' ";

                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            bolPACSChange = false;

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt.Rows.Count == 0)
                            {
                                dt.Dispose();
                                dt = null;
                                ComFunc.MsgBox("검진 방사선촬영 자료가 없습니다.", "오류");
                                return;
                            }
                            else
                            {
                                if (dt.Rows[0]["GBCONV"].ToString().Trim() == "Y")
                                {
                                    bolPACSChange = true;
                                    strPano = txtPtNo.Text.Trim();
                                }
                                else
                                {
                                    strPano = VB.Val(dt.Rows[0]["PANO"].ToString().Trim()).ToString("#0");
                                }
                            }
                            dt.Dispose();
                            dt = null;

                            if (cboPacs.Text.Trim() == "당일영상전체")
                            {
                                SQL = "";
                                SQL = "SELECT XRAYNO FROM ADMIN.HIC_XRAY_RESULT ";
                                if (bolPACSChange == true)
                                {
                                    SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + strPano + "' ";
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + "WHERE PANO = " + strPano + " ";
                                }
                                SQL = SQL + ComNum.VBLF + "  AND JEPDATE>=TO_DATE('" + VB.Left(strSeekDate, 10) + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "  AND JEPDATE<=TO_DATE('" + VB.Left(strSeekDate, 10) + " 23:59','YYYY-MM-DD HH24:MI') ";
                                SQL = SQL + ComNum.VBLF + "  AND XRAYNO > ' ' ";

                                if (chkCombine.Checked == true)
                                    SQL = SQL + ComNum.VBLF + " AND XJONG = '1' ";

                                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return;
                                }

                                if (dt.Rows.Count == 0)
                                {
                                    dt.Dispose();
                                    dt = null;

                                    ComFunc.MsgBox("해당 영상이 없습니다.", "오류");
                                    return;
                                }
                                else
                                {
                                    for (i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //'클릭한 영상을 처음으로 표시하기 위한 루틴
                                        if (strPacsNo.IndexOf(dt.Rows[i]["XRAYNO"].ToString().Trim()) == -1)
                                        {
                                            strPacsNo = strPacsNo + dt.Rows[i]["XRAYNO"].ToString().Trim() + " ";
                                        }
                                    }
                                    strPacsNo = strPacsNo.Trim();
                                }
                                dt.Dispose();
                                dt = null;
                            }
                            else if (cboPacs.Text.Trim() == "전체영상")
                            {
                                SQL = "";
                                SQL = "SELECT XRAYNO FROM ADMIN.HIC_XRAY_RESULT ";

                                if (bolPACSChange == true)
                                {
                                    SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + strPano + "' ";
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + "WHERE PANO = " + strPano + " ";
                                }

                                SQL = SQL + ComNum.VBLF + "  AND XRAYNO > ' ' ";

                                if (chkCombine.Checked == true)
                                    SQL = SQL + ComNum.VBLF + " AND XJONG = '1' ";

                                SQL = SQL + ComNum.VBLF + "ORDER BY JEPDATE DESC ";

                                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return;
                                }

                                if (dt.Rows.Count == 0)
                                {
                                    dt.Dispose();
                                    dt = null;
                                    ComFunc.MsgBox("해당 영상이 없습니다.", "오류");
                                    return;
                                }
                                else
                                {
                                    for (i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //'클릭한 영상을 처음으로 표시하기 위한 루틴
                                        if (strPacsNo.IndexOf(dt.Rows[i]["XRAYNO"].ToString().Trim()) == -1)
                                        {
                                            strPacsNo = strPacsNo + dt.Rows[i]["XRAYNO"].ToString().Trim() + " ";
                                        }
                                    }
                                    strPacsNo = strPacsNo.Trim();
                                }
                                dt.Dispose();
                                dt = null;
                            }
                            else if (cboPacs.Text == "선택영상전체")
                            {
                                strPacsNo = "";

                                for (i = 0; i < ss1_Sheet1.RowCount; i++)
                                {
                                    if (Convert.ToBoolean(ss2_Sheet1.Cells[i, 0].Value) == true)
                                    {
                                        ss2_Sheet1.Cells[i, 0].Value = false;

                                        strPacsNo = strPacsNo + ss2_Sheet1.Cells[i, 12].Text.Trim() + " ";
                                    }
                                }

                                strPacsNo = strPacsNo.Trim();

                                if (strPacsNo == "")
                                {
                                    ComFunc.MsgBox("영상을 1건도 선택 안함", "오류");
                                    return;
                                }
                            }

                            if (bolPACSChange == true)
                            {
                                clsPacs.SET_XRAY_CVR_READ_UPDATE(clsDB.DbCon, strPano, strPacsNo.Trim());


                                clsPacs.PACS_Image_View(clsDB.DbCon, strPano, strPacsNo.Trim(), clsType.User.IdNumber, chkCombine.Checked);
                            }
                            else
                            {
                                clsPacs.PACS_Image_View(clsDB.DbCon, "H" + strPano, strPacsNo.Trim(), clsType.User.IdNumber, chkCombine.Checked);
                            }
                        }
                    }
                    else
                    {
                        if (mstrXRayDate != "")
                            return;

                        strUID = ss2_Sheet1.Cells[e.Row, 12].Text.Trim();

                        if (strUID != "")
                        {
                            //SS2_PACS_VIEW: '병원
                            strSeekDate = ss2_Sheet1.Cells[e.Row, 1].Text.Trim();

                            switch (ss2_Sheet1.Cells[e.Row, 6].Text.Trim())
                            {
                                case "":
                                    strXJong = "1";
                                    break;
                                case "특수":
                                    strXJong = "2";
                                    break;
                                case "SONO":
                                    strXJong = "3";
                                    break;
                                case "CT":
                                    strXJong = "4";
                                    break;
                                case "MRI":
                                    strXJong = "5";
                                    break;
                                case "RI":
                                    strXJong = "6";
                                    break;
                                case "BMD":
                                    strXJong = "7";
                                    break;
                                case "EMG":
                                    strXJong = "E";
                                    break;
                            }

                            strPano = txtPtNo.Text.Trim();
                            strPacsNo = ss2_Sheet1.Cells[e.Row, 12].Text.Trim() + " ";

                            if (strXJong == "E")
                            {
                                //'EMG 영상 표시
                                EMG_FILE_DBToFile(strEmgWrtno, strPano, "1");
                                return;
                            }

                            if (cboPacs.Text.Trim() == "당일영상전체")
                            {
                                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                                    return; //권한 확인

                                SQL = "";
                                SQL = "SELECT PACSNO FROM ADMIN.XRAY_DETAIL ";
                                SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "  AND SEEKDATE>=TO_DATE('" + VB.Left(strSeekDate, 10) + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "  AND SEEKDATE<=TO_DATE('" + VB.Left(strSeekDate, 10) + " 23:59','YYYY-MM-DD HH24:MI') ";
                                SQL = SQL + ComNum.VBLF + "  AND PACSSTUDYID > ' ' ";

                                if (chkCombine.Checked == true)
                                    SQL = SQL + ComNum.VBLF + " AND XJONG = '1' ";

                                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return;
                                }

                                if (dt.Rows.Count == 0)
                                {
                                    dt.Dispose();
                                    dt = null;
                                    ComFunc.MsgBox("해당 영상이 없습니다.", "오류");
                                    return;
                                }
                                else
                                {
                                    for (i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //'클릭한 영상을 처음으로 표시하기 위한 루틴
                                        if (strPacsNo.IndexOf(dt.Rows[i]["PACSNO"].ToString().Trim()) == -1)
                                        {
                                            strPacsNo = strPacsNo + dt.Rows[i]["PACSNO"].ToString().Trim() + " ";
                                        }
                                    }
                                    strPacsNo = strPacsNo.Trim();
                                }

                                dt.Dispose();
                                dt = null;

                            }
                            else if (cboPacs.Text.Trim() == "전체영상")
                            {
                                SQL = "";
                                SQL = "SELECT PACSNO FROM ADMIN.XRAY_DETAIL ";
                                SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "  AND PACSSTUDYID > ' ' ";

                                if (chkCombine.Checked == true)
                                    SQL = SQL + ComNum.VBLF + " AND XJONG = '1' ";

                                SQL = SQL + ComNum.VBLF + "ORDER BY SEEKDATE DESC ";

                                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return;
                                }

                                if (dt.Rows.Count == 0)
                                {
                                    dt.Dispose();
                                    dt = null;
                                    ComFunc.MsgBox("해당 영상이 없습니다.", "오류");
                                    return;
                                }
                                else
                                {
                                    for (i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //'클릭한 영상을 처음으로 표시하기 위한 루틴
                                        if (strPacsNo.IndexOf(dt.Rows[i]["PACSNO"].ToString().Trim()) == -1)
                                        {
                                            strPacsNo = strPacsNo + dt.Rows[i]["PACSNO"].ToString().Trim() + " ";
                                        }
                                    }
                                    strPacsNo = strPacsNo.Trim();
                                }

                                dt.Dispose();
                                dt = null;
                            }
                            else if (cboPacs.Text.Trim() == "선택영상전체")
                            {
                                strPacsNo = "";
                                for (i = 0; i < ss1_Sheet1.RowCount; i++)
                                {
                                    if (Convert.ToBoolean(ss2_Sheet1.Cells[i, 0].Value) == true)
                                    {
                                        ss2_Sheet1.Cells[i, 0].Value = false;

                                        strPacsNo = strPacsNo + ss2_Sheet1.Cells[i, 12].Text.Trim() + " ";
                                    }
                                }

                                strPacsNo = strPacsNo.Trim();

                                if (strPacsNo == "")
                                {
                                    ComFunc.MsgBox("영상을 1건도 선택 안함", "오류");
                                    return;
                                }
                            }
                            //'2015-09-25
                            clsPacs.SET_XRAY_CVR_READ_UPDATE(clsDB.DbCon, strPano, strPacsNo.Trim());
                            clsPacs.PACS_Image_View(clsDB.DbCon, strPano, strPacsNo.Trim(), clsType.User.IdNumber, chkCombine.Checked);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnDementia_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            if (txtPtNo.Text.Trim() == "")
                return;

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ss2_Sheet1.RowCount = 0;

            txtJusaMemo.Visible = false;

            panDem.Visible = true;
            panEMG.Visible = false;
            panXray.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnWordEntry.Visible = false;
            panPFT.Visible = false;

            ssXrayDr.Visible = false;

            txtResult1.Visible = false;
            ss1.Visible = false;

            panExam.Visible = false;
            lblTitleSub0.Text = "";


            panEEG.Visible = false;
            panConsult.Visible = false;
            panDiscern.Visible = false;
            panECG.Visible = false;
            panEtcJupmst.Visible = false;
            panExam3.Visible = false;
            nIndex = 1;

            SetPAN(panDem);

            ssDEM_Sheet1.Columns.Get(1).Label = "검사일자";
            ssDEM_Sheet1.Columns.Get(1).Width = 90;
            ssDEM_Sheet1.Columns.Get(7).Label = "등록자";
            ssDEM_Sheet1.Columns.Get(9).Label = "검사명";


            ssDEM_Sheet1.Columns.Get(3, 4).Visible = true;
            ssDEM_Sheet1.Columns.Get(7, 8).Visible = true;

            txtEmgResult.Text = "";

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                //'자료를 찾기
                SQL = "";
                SQL = "SELECT A.PTNO,A.SNAME, A.AGE, A.SEX, TO_CHAR(A.BDATE, 'YYYY-MM-DD') AS BDATE,TO_CHAR(A.JDATE, 'YYYY-MM-DD') AS JDATE,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(A.RDATE, 'YYYY-MM-DD') AS RDATE,  A.DEPTCODE, A.DRCODE, A.GBIO,  A.RESULT, A.RESULTSABUN, B.DRNAME,";
                SQL = SQL + ComNum.VBLF + " A.ORDERCODE, C.ORDERNAME ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.ETC_RESULT_DEMENTIA A,ADMIN.BAS_DOCTOR B,";
                SQL = SQL + ComNum.VBLF + "      ADMIN.OCS_ORDERCODE C ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PTNO='" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.BDATE<=TRUNC(SYSDATE+1) ";
                SQL = SQL + ComNum.VBLF + "  AND A.DRCODE=B.DRCODE(+) ";
                SQL = SQL + ComNum.VBLF + "  AND A.ORDERCODE=C.ORDERCODE(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BDATE  DESC,1 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssDEM_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssDEM_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssDEM_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                        ssDEM_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssDEM_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssDEM_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GBIO"].ToString().Trim();
                        ssDEM_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssDEM_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssDEM_Sheet1.Cells[i, 7].Text = dt.Rows[i]["RESULT"].ToString().Trim();
                        ssDEM_Sheet1.Cells[i, 8].Text = clsVbfunc.GetInSaName(clsDB.DbCon, VB.Val(dt.Rows[i]["RESULTSABUN"].ToString().Trim()).ToString("00000"));
                        ssDEM_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                lblTitleSub11.Text = ((Button)sender).Text.Trim();

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void ssConsult_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
                return;
            if (ssConsult_Sheet1.RowCount == 0)
                return;

            string strConsult = "";

            strConsult = "";

            strConsult = "======================================================================= " + ComNum.VBLF;
            strConsult = strConsult + "▶ 처 방 일 : " + ssConsult_Sheet1.Cells[e.Row, 0].Text.Trim() + ComNum.VBLF;

            strConsult = strConsult + "▶ 의뢰일시 : " + ssConsult_Sheet1.Cells[e.Row, 8].Text.Trim() + ComNum.VBLF;
            strConsult = strConsult + "▶ 의 뢰 과 : " + ssConsult_Sheet1.Cells[e.Row, 1].Text.Trim() + " ( " + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, ssConsult_Sheet1.Cells[e.Row, 1].Text.Trim()) + " ) " + ComNum.VBLF;
            strConsult = strConsult + "▶ 의 뢰 의 : " + ssConsult_Sheet1.Cells[e.Row, 2].Text.Trim() + ComNum.VBLF;

            strConsult = strConsult + "▶ 응급구분 : " + ssConsult_Sheet1.Cells[e.Row, 11].Text.Trim() + ComNum.VBLF;

            strConsult = strConsult + "======================================================================= " + ComNum.VBLF;
            strConsult = strConsult + "▶ 의뢰내용 : " + ComNum.VBLF;
            strConsult = strConsult + "======================================================================= " + ComNum.VBLF;

            strConsult = strConsult + ssConsult_Sheet1.Cells[e.Row, 7].Text.Trim() + ComNum.VBLF + ComNum.VBLF;

            strConsult = strConsult + "======================================================================= " + ComNum.VBLF;
            strConsult = strConsult + "▶ 결과 일시 : " + ssConsult_Sheet1.Cells[e.Row, 9].Text.Trim() + ComNum.VBLF;
            strConsult = strConsult + "▶ Consult과 : " + ssConsult_Sheet1.Cells[e.Row, 3].Text.Trim() + " ( " + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, ssConsult_Sheet1.Cells[e.Row, 3].Text.Trim()) + " ) " + ComNum.VBLF;
            strConsult = strConsult + "▶ Consult의 : " + ssConsult_Sheet1.Cells[e.Row, 4].Text.Trim() + ComNum.VBLF;
            strConsult = strConsult + "======================================================================= " + ComNum.VBLF;
            strConsult = strConsult + "▶ 결과 내용 : " + ComNum.VBLF;
            strConsult = strConsult + "======================================================================= " + ComNum.VBLF;
            strConsult = strConsult + ssConsult_Sheet1.Cells[e.Row, 10].Text + ComNum.VBLF;

            txtConsult.Text = strConsult;
        }

        private void ssCVR_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
                return;
            if (ssCVR_Sheet1.RowCount == 0)
                return;

            txtCRemark1.Text = ssConsult_Sheet1.Cells[e.Row, 5].Text.Trim();
            txtCRemark2.Text = ssConsult_Sheet1.Cells[e.Row, 9].Text.Trim();
            mstrRowIdCVR = ssConsult_Sheet1.Cells[e.Row, 10].Text.Trim();
        }

        private void ssDiscern_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strBun = "";
            string strROWID = "";

            int intWRTNO = 0;

            strBun = ssDiscern_Sheet1.Cells[e.Row, 2].Text.Trim();
            strROWID = ssDiscern_Sheet1.Cells[e.Row, 3].Text.Trim();
            intWRTNO = Convert.ToInt32(VB.Val(ssDiscern_Sheet1.Cells[e.Row, 4].Text.Trim()));

            //'2010-11-15 김현욱 추가함

            SetDrughoi(Convert.ToString(intWRTNO));
            SetDrughoi2(Convert.ToString(intWRTNO));
            SET_DRUGHOI3(Convert.ToString(intWRTNO));
        }

        private void SET_DRUGHOI3(string strWRTNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strFDRUGCD = "";
            string strSAYU1 = "";
            string strSAYU2 = "";
            string strSAYU3 = "";
            string strSAYU4 = "";
            string strSAYU5 = "";
            string strSAYU6 = "";
            string strSAYU7 = "";
            string strSAYU8 = "";
            string strSAYU9 = "";
            string strSAYU10 = "";
            string strSAYU11 = "";
            string strSAYU12 = "";
            string strSAYU13 = "";
            string strSAYU14 = "";
            string strSAYU15 = "";
            string strSAYU16 = "";
            string strSAYU17 = "";
            string strREMARK14 = "";
            string strTemp = "";
            string strTemp2 = "";
            string strJEP1 = "";
            string strJEP2 = "";
            string strJEP3 = "";

            string strJEP4 = "";
            string strJEP5 = "";
            string strJEP6 = "";

            ssPRT3_CLEAR();

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.JDATE, A.BDATE, A.DEPTCODE, A.DRCODE, A.DRSABUN, A.WARDCODE, ";
                SQL = SQL + ComNum.VBLF + "     A.ROOMCODE, A.PANO, A.REMCODE1, A.REMCODE2, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE3, A.REMCODE4, A.REMCODE5, A.REMCODE6, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE7, A.REMCODE8, A.REMCODE9, A.REMCODE10, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE11, A.REMCODE12, A.REMCODE13, A.REMCODE14, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE15, A.REMCODE16, A.REMCODE17, ";
                SQL = SQL + ComNum.VBLF + "     A.HOSP, A.PHAR, B.SNAME, B.JUMIN1, B.JUMIN2, A.DABCODE, ";
                SQL = SQL + ComNum.VBLF + "     A.FASTRETURN, A.RETURNMEMO, A.HDATE, A.DRUGNAME, ";
                SQL = SQL + ComNum.VBLF + "     A.PANO, A.DRUGGIST, A.ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOIMST A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.WRTNO = '" + strWRTNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssPRT3_Sheet1.Cells[3, 3].Text = Convert.ToDateTime(dt.Rows[0]["BDATE"].ToString().Trim()).ToString("yyyy/MM/dd HH:mm");

                    switch (dt.Rows[0]["DABCODE"].ToString().Trim())
                    {
                        case "02": ssPRT3_Sheet1.Cells[4, 3].Text = "( 30분 이내 )"; break;
                        case "03": ssPRT3_Sheet1.Cells[4, 3].Text = "( 1시간 이내 )"; break;
                        case "05": ssPRT3_Sheet1.Cells[4, 3].Text = "( 3시간 이내 )"; break;
                        case "06": ssPRT3_Sheet1.Cells[4, 3].Text = "( 금일 이내 )"; break;
                        case "07": ssPRT3_Sheet1.Cells[4, 3].Text = "( 48시간 이내 )"; break;
                    }

                    //접수일시
                    if (dt.Rows[0]["JDATE"].ToString().Trim() != "")
                    {
                        ssPRT3_Sheet1.Cells[3, 13].Text = Convert.ToDateTime(dt.Rows[0]["JDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        ssPRT3_Sheet1.Cells[4, 13].Text = Convert.ToDateTime(dt.Rows[0]["JDATE"].ToString().Trim()).ToString("HH:mm");
                    }

                    //회신일자
                    if (dt.Rows[0]["HDATE"].ToString().Trim() != "")
                    {
                        ssPRT3_Sheet1.Cells[5, 3].Text = Convert.ToDateTime(dt.Rows[0]["HDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        ssPRT3_Sheet1.Cells[6, 3].Text = Convert.ToDateTime(dt.Rows[0]["HDATE"].ToString().Trim()).ToString("HH:mm");
                    }

                    //회신자
                    ssPRT3_Sheet1.Cells[7, 3].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DRUGGIST"].ToString().Trim()) + " 약사";

                    //환자정보
                    ssPRT3_Sheet1.Cells[1, 3].Text = clsVbfunc.GetPatientName(clsDB.DbCon, dt.Rows[0]["PANO"].ToString().Trim());
                    ssPRT3_Sheet1.Cells[1, 5].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ssPRT3_Sheet1.Cells[1, 8].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[0]["DEPTCODE"].ToString().Trim());

                    ssPRT3_Sheet1.Cells[2, 5].Text = dt.Rows[0]["WARDCODE"].ToString().Trim() + "/" + dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    ssPRT3_Sheet1.Cells[2, 8].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());

                    //의뢰자
                    ssPRT3_Sheet1.Cells[3, 10].Text = clsVbfunc.READ_INSA_BUSE(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());
                    ssPRT3_Sheet1.Cells[3, 11].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());

                    strSAYU1 = dt.Rows[0]["REMCODE1"].ToString().Trim();
                    strSAYU2 = dt.Rows[0]["REMCODE2"].ToString().Trim();
                    strSAYU3 = dt.Rows[0]["REMCODE3"].ToString().Trim();
                    strSAYU4 = dt.Rows[0]["REMCODE4"].ToString().Trim();
                    strSAYU5 = dt.Rows[0]["REMCODE5"].ToString().Trim();
                    strSAYU6 = dt.Rows[0]["REMCODE6"].ToString().Trim();
                    strSAYU7 = dt.Rows[0]["REMCODE7"].ToString().Trim();
                    strSAYU8 = dt.Rows[0]["REMCODE8"].ToString().Trim();
                    strSAYU9 = dt.Rows[0]["REMCODE9"].ToString().Trim();
                    strSAYU10 = dt.Rows[0]["REMCODE10"].ToString().Trim();
                    strSAYU11 = dt.Rows[0]["REMCODE11"].ToString().Trim();
                    strSAYU12 = dt.Rows[0]["REMCODE12"].ToString().Trim();
                    strSAYU13 = dt.Rows[0]["REMCODE13"].ToString().Trim();
                    strSAYU14 = dt.Rows[0]["REMCODE14"].ToString().Trim();
                    strSAYU15 = dt.Rows[0]["REMCODE15"].ToString().Trim();
                    strSAYU16 = dt.Rows[0]["REMCODE16"].ToString().Trim();
                    strSAYU17 = dt.Rows[0]["REMCODE17"].ToString().Trim();


                    strTemp = "";
                    strTemp = strTemp + (strSAYU1 != "" ? strSAYU1 + ", " : "");
                    strTemp = strTemp + (strSAYU1 != "" ? strSAYU1 + ", " : "");
                    strTemp = strTemp + (strSAYU3 != "" ? strSAYU3 + ", " : "");
                    strTemp = strTemp + (strSAYU4 != "" ? strSAYU4 + ", " : "");
                    strTemp = strTemp + (strSAYU5 != "" ? strSAYU5 + ", " : "");
                    strTemp = strTemp + (strSAYU7 != "" ? strSAYU7 + ", " : "");
                    strTemp = strTemp + (strSAYU8 != "" ? strSAYU8 + ", " : "");
                    strTemp = strTemp + (strSAYU9 != "" ? strSAYU9 + ", " : "");
                    strTemp = strTemp + (strSAYU10 != "" ? strSAYU10 + ", " : "");
                    strTemp = strTemp + (strSAYU11 != "" ? strSAYU11 + ", " : "");
                    strTemp = strTemp + (strSAYU12 != "" ? strSAYU12 + ", " : "");
                    strTemp = strTemp + (strSAYU13 != "" ? strSAYU13 + ", " : "");
                    strTemp = strTemp + (strSAYU14 != "" ? strSAYU14 + ", " : "");
                    strTemp = strTemp + (strSAYU15 != "" ? strSAYU15 + ", " : "");
                    strTemp = strTemp + (strSAYU16 != "" ? strSAYU16 + ", " : "");
                    strTemp = strTemp + (strSAYU17 != "" ? strSAYU17 + ", " : "");
                    strTemp = strTemp + (strSAYU6 != "" ? strSAYU6 + ", " : "");

                    if (strTemp != "")
                    {
                        strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2);
                    }

                    //복용사유
                    ssPRT3_Sheet1.Cells[1, 10].Text = strTemp;
                    //긴급요청사유
                    ssPRT3_Sheet1.Cells[3, 7].Text = dt.Rows[0]["FASTRETURN"].ToString().Trim();
                    //처방병원
                    ssPRT3_Sheet1.Cells[2, 10].Text = dt.Rows[0]["HOSP"].ToString().Trim();
                    //회신자 전달사항
                    ssPRT3_Sheet1.Cells[5, 5].Text = dt.Rows[0]["RETURNMEMO"].ToString().Trim();
                    //조제약국
                    ssPRT3_Sheet1.Cells[2, 13].Text = dt.Rows[0]["PHAR"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                ssPRT3_Sheet1.Cells[8, 5].Value = false;
                ssPRT3_Sheet1.Cells[8, 6].Value = true;

                ssPRT3_Sheet1.Cells[9, 5].Value = false;
                ssPRT3_Sheet1.Cells[9, 6].Value = true;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     REMARK1, REMARK2, REMARK3, REMARK4, ";
                SQL = SQL + ComNum.VBLF + "     REMARK5, REMARK6, REMARK7, REMARK8, ";
                SQL = SQL + ComNum.VBLF + "     REMARK9, REMARK10, REMARK11, REMARK12, ";
                SQL = SQL + ComNum.VBLF + "     REMARK13, BLOOD, ROWID, EDICODE, METFORMIN,";
                SQL = SQL + ComNum.VBLF + "     REMARK14, QTY, NAL, DOSCODE, DECODE(TUYAKGBN, '1', '●', '') AS TUYAKGBN, RP, ";
                SQL = SQL + ComNum.VBLF + "     REMARK15, REMARK16, REMARK17, REMARK18, ";
                SQL = SQL + ComNum.VBLF + "     NOT_SIKBYUL, IMGYN, ROWID  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOISLIP ";
                SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = " + strWRTNO;
                SQL = SQL + ComNum.VBLF + "ORDER BY ENTDATE ASC, RP ASC, EDICODE ASC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        DRAW_LINE3();

                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 0].Text = (i + 1).ToString();
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 1].Text = dt.Rows[i]["RP"].ToString().Trim();

                        //이미지
                        if (dt.Rows[i]["IMGYN"].ToString().Trim() != "1")
                        {
                            strFDRUGCD = READ_FDRUGCD(dt.Rows[i]["EDICODE"].ToString().Trim());
                            if (strFDRUGCD != "")
                            {
                                GetDrugInfoImg(strFDRUGCD, ssPRT3_Sheet1.RowCount - 2, 2);
                            }
                        }

                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].Text = dt.Rows[i]["REMARK1"].ToString().Trim()
                            + (dt.Rows[i]["EDICODE"].ToString().Trim() != "" ? ComNum.VBLF + "(" + dt.Rows[i]["EDICODE"].ToString().Trim() + ")" : "")
                            + (dt.Rows[i]["REMARK7"].ToString().Trim() != "" ? ComNum.VBLF + "(" + dt.Rows[i]["REMARK7"].ToString().Trim() + ")" : "");
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["REMARK2"].ToString().Trim();

                        //성상
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 4].Text = dt.Rows[i]["REMARK6"].ToString().Trim();
                        //제형
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 4].Text = "제형:" + dt.Rows[i]["REMARK8"].ToString().Trim();

                        //식별 (앞)
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 2].Text = "앞) " + dt.Rows[i]["REMARK9"].ToString().Trim();

                        //식별 (뒤)
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 3].Text = "뒤) " + dt.Rows[i]["REMARK10"].ToString().Trim();

                        //효능/효과
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 8].Text = dt.Rows[i]["REMARK3"].ToString().Trim();

                        //일투량
                        if (VB.Val(dt.Rows[i]["QTY"].ToString().Trim()) > 0)
                        {
                            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 9].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        }

                        //if (VB.Val(dt.Rows[i]["NAL"].ToString().Trim()) > 0)
                        //{
                        //    ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 14].Text = "(" + dt.Rows[i]["NAL"].ToString().Trim() + "일)";
                        //}

                        strREMARK14 = dt.Rows[i]["REMARK14"].ToString().Trim();

                        if (VB.Val(strREMARK14) > 0)
                        {
                            strREMARK14 = "(수량:" + strREMARK14 + ")";
                        }

                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 9].Text = READ_DOSNAME(clsDB.DbCon, dt.Rows[i]["DOSCODE"].ToString().Trim()) + ComNum.VBLF + strREMARK14;

                        strJEP1 = dt.Rows[i]["REMARK5"].ToString().Trim();
                        strJEP2 = dt.Rows[i]["REMARK12"].ToString().Trim();
                        strJEP3 = dt.Rows[i]["REMARK13"].ToString().Trim();
                        if (strJEP1 != "") { strJEP1 = ComNum.VBLF + strJEP1 + READ_DRUGNAME(strJEP1); }
                        if (strJEP2 != "") { strJEP2 = ComNum.VBLF + strJEP2 + READ_DRUGNAME(strJEP2); }
                        if (strJEP3 != "") { strJEP3 = ComNum.VBLF + strJEP3 + READ_DRUGNAME(strJEP3); }

                        strTemp2 = "";
                        strTemp2 += READ_USED_GUBUN(dt.Rows[i]["REMARK11"].ToString().Trim());
                        strTemp2 += ((strJEP1 + strJEP2 + strJEP3) != "" ? strJEP1 + strJEP2 + strJEP3 : "");

                        //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].Text = READ_USED_GUBUN(dt.Rows[i]["REMARK11"].ToString().Trim());
                        //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 11].Text = ((strJEP1 + strJEP2 + strJEP3) != "" ? strJEP1 + strJEP2 + strJEP3 : "");

                        if (dt.Rows[i]["REMARK15"].ToString().Trim() != "")
                        {
                            strJEP4 = dt.Rows[i]["REMARK16"].ToString().Trim();
                            strJEP5 = dt.Rows[i]["REMARK17"].ToString().Trim();
                            strJEP6 = dt.Rows[i]["REMARK18"].ToString().Trim();
                            if (strJEP4 != "") { strJEP4 = ComNum.VBLF + strJEP4 + READ_DRUGNAME(strJEP4); }
                            if (strJEP5 != "") { strJEP5 = ComNum.VBLF + strJEP5 + READ_DRUGNAME(strJEP5); }
                            if (strJEP6 != "") { strJEP6 = ComNum.VBLF + strJEP6 + READ_DRUGNAME(strJEP6); }

                            strTemp2 += ComNum.VBLF + "----------------------------------------" + ComNum.VBLF;
                            strTemp2 += READ_USED_GUBUN(dt.Rows[i]["REMARK15"].ToString().Trim());
                            strTemp2 += ((strJEP4 + strJEP5 + strJEP6) != "" ? strJEP4 + strJEP5 + strJEP6 : "");
                        }
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].Text = strTemp2;

                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 13].Text = dt.Rows[i]["TUYAKGBN"].ToString().Trim();
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 14].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        //항혈전제 표시
                        if (dt.Rows[i]["BLOOD"].ToString().Trim() == "1")
                        {
                            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].Text = "★" + ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].Text;

                            ssPRT3_Sheet1.Cells[8, 5].Value = true;
                            ssPRT3_Sheet1.Cells[8, 6].Value = false;

                            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 0, ssPRT3_Sheet1.RowCount - 2, ssPRT3_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(10, 10, 220);
                            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0, ssPRT3_Sheet1.RowCount - 1, ssPRT3_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(10, 10, 220);
                        }

                        //METFORMIN
                        if (dt.Rows[i]["METFORMIN"].ToString().Trim() == "1")
                        {
                            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].Text = "▣" + ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].Text;

                            ssPRT3_Sheet1.Cells[9, 5].Value = true;
                            ssPRT3_Sheet1.Cells[9, 6].Value = false;

                            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 0, ssPRT3_Sheet1.RowCount - 2, ssPRT3_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(220, 10, 10);
                            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0, ssPRT3_Sheet1.RowCount - 1, ssPRT3_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(220, 10, 10);
                        }


                        byte[] a = System.Text.Encoding.Default.GetBytes(ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 5].Text);
                        int intHeight = Convert.ToInt32(a.Length / 18);

                        if (intHeight > 2)
                        {
                            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT + (intHeight * 16));
                        }

                        //a = System.Text.Encoding.Default.GetBytes(ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 9].Text);

                        //if (intHeight < Convert.ToInt32(a.Length / 23))
                        //{
                        //    intHeight = Convert.ToInt32(a.Length / 23);

                        //    if (intHeight > VB.Split(ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 9].Text, ComNum.VBLF).Length)
                        //    {
                        //        intHeight = VB.Split(ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 9].Text, ComNum.VBLF).Length;
                        //    }
                        //}

                        //ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT + (intHeight * 20));

                        a = System.Text.Encoding.Default.GetBytes(ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].Text);
                        intHeight = Convert.ToInt32(a.Length / 18);

                        if (intHeight > 2)
                        {
                            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 2, ComNum.SPDROWHT + (intHeight * 16));
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                DRAW_BOTTOM3();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SetDrughoi(string strWRTNO)
        {
            string strTemp = "";
            string strTemp2 = "";

            string strJEP1 = "";
            string strJEP2 = "";
            string strJEP3 = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int i = 0;
            int k = 0;

            ssPRT_Sheet1.Cells[3, 6].Text = "";

            ssPRT_Sheet1.Cells[3, 4].Text = "";
            ssPRT_Sheet1.Cells[3, 7].Text = "";
            ssPRT_Sheet1.Cells[3, 9].Text = "";



            ssPRT_Sheet1.Cells[4, 4].Text = "";
            ssPRT_Sheet1.Cells[4, 6].Text = "";
            ssPRT_Sheet1.Cells[4, 9].Text = "";



            ssPRT_Sheet1.Cells[5, 4].Text = "";
            ssPRT_Sheet1.Cells[5, 6].Text = "";
            ssPRT_Sheet1.Cells[5, 9].Text = "";



            ssPRT_Sheet1.Cells[6, 4].Text = "";
            ssPRT_Sheet1.Cells[6, 9].Text = "";



            ssPRT_Sheet1.Cells[7, 4].Text = "";
            ssPRT_Sheet1.Cells[7, 9].Text = "";


            ssPRT_Sheet1.RowCount = 17;

            for (i = 16; i < ssPRT_Sheet1.RowCount; i++)
            {
                for (k = 0; k < ssPRT_Sheet1.ColumnCount; k++)
                {
                    ssPRT_Sheet1.Cells[i, k].Text = "";
                }
            }

            ssPRT_Sheet1.Cells[12, 5].Value = false;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인
                SQL = "";
                SQL = " SELECT TO_CHAR(A.JDATE,'YYYY-MM-DD HH24:MM') AS JDATE, A.BDATE, A.DEPTCODE, A.DRCODE, A.DRSABUN, A.WARDCODE, ";
                SQL = SQL + ComNum.VBLF + " A.ROOMCODE, A.PANO, A.REMCODE1, A.REMCODE2, ";
                SQL = SQL + ComNum.VBLF + " A.REMCODE3, A.REMCODE4, A.REMCODE5, A.REMCODE6, ";
                SQL = SQL + ComNum.VBLF + " A.REMCODE7, A.REMCODE8, A.REMCODE9, A.REMCODE10, ";
                SQL = SQL + ComNum.VBLF + " A.REMCODE11, A.REMCODE12, A.REMCODE13, A.REMCODE14, ";
                SQL = SQL + ComNum.VBLF + " A.REMCODE15, A.REMCODE16, A.REMCODE17, ";
                SQL = SQL + ComNum.VBLF + " A.HOSP, A.PHAR, B.SNAME, B.JUMIN1, B.JUMIN2, A.DABCODE ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.DRUG_HOIMST A, ADMIN.BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.WRTNO = '" + strWRTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssPRT_Sheet1.Cells[3, 4].Text = dt.Rows[0]["BDATE"].ToString().Trim();
                    ssPRT_Sheet1.Cells[3, 6].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    ssPRT_Sheet1.Cells[3, 9].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());

                    ssPRT_Sheet1.Cells[4, 4].Text = dt.Rows[0]["JDATE"].ToString().Trim();


                    ssPRT_Sheet1.Cells[5, 4].Text = dt.Rows[0]["WARDCODE"].ToString().Trim() + " / " + dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    ssPRT_Sheet1.Cells[5, 6].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssPRT_Sheet1.Cells[5, 9].Text = dt.Rows[0]["PANO"].ToString().Trim();

                    switch (dt.Rows[0]["DABCODE"].ToString().Trim())
                    {
                        case "01":
                            ssPRT_Sheet1.Cells[6, 4].Text = "10분 이내";
                            break;
                        case "02":
                            ssPRT_Sheet1.Cells[6, 4].Text = "30분 이내";
                            break;
                        case "03":
                            ssPRT_Sheet1.Cells[6, 4].Text = "1시간 이내";
                            break;
                        case "04":
                            ssPRT_Sheet1.Cells[6, 4].Text = "2시간 이내";
                            break;
                        case "05":
                            ssPRT_Sheet1.Cells[6, 4].Text = "3시간 이내";
                            break;
                        case "06":
                            ssPRT_Sheet1.Cells[6, 4].Text = "금일 이내";
                            break;
                        case "07":
                            ssPRT_Sheet1.Cells[6, 4].Text = "48시간 이내";
                            break;
                    }

                    ssPRT_Sheet1.Cells[6, 6].Text = clsVbfunc.READ_INSA_BUSE(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());
                    ssPRT_Sheet1.Cells[6, 9].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());

                    strTemp = "";

                    for (i = 1; i < 18; i++)
                    {
                        if (i == 6)
                            continue;

                        strTemp = (dt.Rows[0]["REMCODE" + Convert.ToString(i)].ToString().Trim() != "" ? dt.Rows[0]["REMCODE" + Convert.ToString(i)].ToString().Trim() + ", " : "");
                    }

                    strTemp = (dt.Rows[0]["REMCODE6"].ToString().Trim() != "" ? dt.Rows[0]["REMCODE6"].ToString().Trim() + ", " : "");

                    if (strTemp != "")
                        strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2);


                    ssPRT_Sheet1.Cells[7, 4].Text = strTemp;
                    clsAES.Read_Jumin_AES(clsDB.DbCon, dt.Rows[0]["PANO"].ToString().Trim());
                    ssPRT_Sheet1.Cells[7, 8].Text = clsAES.GstrAesJumin1 + " - " + clsAES.GstrAesJumin2;
                    ssPRT_Sheet1.Cells[7, 8].Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + dt.Rows[0]["JUMIN2"].ToString().Trim();

                    ssPRT_Sheet1.Cells[8, 4].Text = dt.Rows[0]["HOSP"].ToString().Trim();
                    ssPRT_Sheet1.Cells[8, 8].Text = dt.Rows[0]["PHAR"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = " SELECT REMARK1, REMARK2, REMARK3, REMARK4, ";
                SQL = SQL + ComNum.VBLF + " REMARK5, REMARK6, REMARK7, REMARK8, ";
                SQL = SQL + ComNum.VBLF + " REMARK9, REMARK10, REMARK11, REMARK12, ";
                SQL = SQL + ComNum.VBLF + " REMARK13, REMARK14, BLOOD, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.DRUG_HOISLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + strWRTNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssPRT_Sheet1.RowCount = ssPRT_Sheet1.RowCount + 1;

                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 2].Text = Convert.ToString(i + 1);

                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["REMARK1"].ToString().Trim() + (dt.Rows[i]["REMARK7"].ToString().Trim() != "" ? ComNum.VBLF + "(" + dt.Rows[i]["REMARK7"].ToString().Trim() + ")" : "");
                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["REMARK2"].ToString().Trim();
                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["REMARK3"].ToString().Trim();
                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["REMARK8"].ToString().Trim();
                        strTemp2 = dt.Rows[i]["REMARK14"].ToString().Trim();
                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["REMARK6"].ToString().Trim() + (strTemp2 != "" ? ComNum.VBLF + "──────" + ComNum.VBLF + "수량 : " + strTemp2 : "");
                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 8].Text =
                        (dt.Rows[i]["REMARK9"].ToString().Trim() != "" ? dt.Rows[i]["REMARK9"].ToString().Trim() : "") +
                        (dt.Rows[i]["REMARK10"].ToString().Trim() != "" ? ComNum.VBLF + "──────" + ComNum.VBLF + dt.Rows[i]["REMARK10"].ToString().Trim() : "");

                        strJEP1 = dt.Rows[i]["REMARK5"].ToString().Trim();
                        strJEP2 = dt.Rows[i]["REMARK12"].ToString().Trim();
                        strJEP3 = dt.Rows[i]["REMARK13"].ToString().Trim();

                        if (strJEP1 != "")
                            strJEP1 = ComNum.VBLF + strJEP1;
                        if (strJEP2 != "")
                            strJEP2 = ComNum.VBLF + strJEP2;
                        if (strJEP3 != "")
                            strJEP3 = ComNum.VBLF + strJEP3;

                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 9].Text = READ_USED_GUBUN(dt.Rows[i]["REMARK11"].ToString().Trim()) + (strJEP1 + strJEP2 + strJEP3 != "" ? ComNum.VBLF + strJEP1 + strJEP2 + strJEP3 : "");
                        //ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        if (dt.Rows[i]["BLOOD"].ToString().Trim() == "1")
                        {
                            ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 3].Text = "★";
                            ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 2, ssPRT_Sheet1.RowCount - 1, 9].BackColor = Color.FromArgb(255, 220, 255);

                            ssPRT_Sheet1.Cells[12, 5].Value = true;
                        }
                        else
                        {
                            ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 2, ssPRT_Sheet1.RowCount - 1, 9].BackColor = Color.FromArgb(255, 255, 255);
                        }
                        ssPRT_Sheet1.SetRowHeight(ssPRT_Sheet1.RowCount - 1, Convert.ToInt32(ssPRT_Sheet1.GetPreferredRowHeight(ssPRT_Sheet1.RowCount - 1)));
                    }
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private string READ_USED_GUBUN(string strGUBUN)
        {
            string rtnVal = "";

            switch (strGUBUN.Trim())
            {
                case "01": rtnVal = "◎원내/외 동종ㆍ동효약 없음"; break;
                case "02": rtnVal = "◎원내-동일약"; break;
                case "03": rtnVal = "◎원내-대체약(성분 동일, 함량 동일)"; break;
                case "04": rtnVal = "◎원내-동종약(성분 동일, 함량 다름)"; break;
                case "05": rtnVal = "◎원내-단일성분만 사용"; break;
                case "06": rtnVal = "◎원외전용-동일약"; break;
                case "07": rtnVal = "◎원외전용-대체약(성분 동일, 함량 동일)"; break;
                case "08": rtnVal = "◎원외전용-동종약(성분 동일, 함량 다름)"; break;
                case "09": rtnVal = "◎원내-효능유사약(성분 다름)"; break;
                case "10": rtnVal = "◎원내-동일성분,제형다름(약동학적 차이 있음)"; break;
            }

            return rtnVal;
        }

        private void SetDrughoi2(string strWRTNO)
        {
            int i = 0;
            int intRow = 0;

            string strREMARK14 = "";
            string strTemp = "";
            string strJEP1 = "";
            string strJEP2 = "";
            string strJEP3 = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SSPRT2_CLEAR();

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인
                SQL = "";
                SQL = " SELECT TO_CHAR(A.JDATE, 'YYYY/MM/DD HH24:MM') AS JDATE, TO_CHAR(A.BDATE, 'YYYY/MM/DD HH24:MM') AS BDATE, A.DEPTCODE, A.DRCODE, A.DRSABUN, A.WARDCODE, ";
                SQL = SQL + ComNum.VBLF + " A.ROOMCODE, A.PANO, A.REMCODE1, A.REMCODE2, ";
                SQL = SQL + ComNum.VBLF + " A.REMCODE3, A.REMCODE4, A.REMCODE5, A.REMCODE6, ";
                SQL = SQL + ComNum.VBLF + " A.REMCODE7, A.REMCODE8, A.REMCODE9, A.REMCODE10, ";
                SQL = SQL + ComNum.VBLF + " A.REMCODE11, A.REMCODE12, A.REMCODE13, A.REMCODE14, ";
                SQL = SQL + ComNum.VBLF + " A.REMCODE15, A.REMCODE16, A.REMCODE17, ";
                SQL = SQL + ComNum.VBLF + " A.HOSP, A.PHAR, B.SNAME, B.JUMIN1, B.JUMIN2, A.DABCODE, ";
                SQL = SQL + ComNum.VBLF + " A.FASTRETURN, A.RETURNMEMO, TO_CHAR(A.HDATE, 'YYYY/MM/DD HH24:MM') AS HDATE, A.DRUGNAME, ";
                SQL = SQL + ComNum.VBLF + " A.PANO, A.DRUGGIST, A.ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.DRUG_HOIMST A, ADMIN.BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.WRTNO = '" + strWRTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    ssPRT2_Sheet1.Cells[1, 4].Text = dt.Rows[0]["BDATE"].ToString().Trim();

                    switch (dt.Rows[i]["DABCODE"].ToString().Trim())
                    {
                        case "02":
                            ssPRT2_Sheet1.Cells[1, 4].Text = ssPRT2_Sheet1.Cells[1, 4].Text + ComNum.VBLF + "( 30분 이내 )";
                            break;
                        case "03":
                            ssPRT2_Sheet1.Cells[1, 4].Text = ssPRT2_Sheet1.Cells[1, 4].Text + ComNum.VBLF + "( 1시간 이내 )";
                            break;
                        case "05":
                            ssPRT2_Sheet1.Cells[1, 4].Text = ssPRT2_Sheet1.Cells[1, 4].Text + ComNum.VBLF + "( 3시간 이내 )";
                            break;
                        case "06":
                            ssPRT2_Sheet1.Cells[1, 4].Text = ssPRT2_Sheet1.Cells[1, 4].Text + ComNum.VBLF + "( 금일 이내 )";
                            break;
                        case "07":
                            ssPRT2_Sheet1.Cells[1, 4].Text = ssPRT2_Sheet1.Cells[1, 4].Text + ComNum.VBLF + "( 48시간 이내 )";
                            break;
                    }

                    ssPRT2_Sheet1.Cells[1, 8].Text = dt.Rows[0]["JDATE"].ToString().Trim();
                    ssPRT2_Sheet1.Cells[1, 12].Text = dt.Rows[0]["HDATE"].ToString().Trim();
                    ssPRT2_Sheet1.Cells[1, 16].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DRUGGIST"].ToString().Trim()) + "약사";

                    ssPRT2_Sheet1.Cells[2, 4].Text = clsVbfunc.GetPatientName(clsDB.DbCon, dt.Rows[0]["PANO"].ToString().Trim());
                    ssPRT2_Sheet1.Cells[2, 8].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ssPRT2_Sheet1.Cells[2, 12].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());
                    ssPRT2_Sheet1.Cells[2, 16].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[0]["DEPTCODE"].ToString().Trim());

                    ssPRT2_Sheet1.Cells[3, 8].Text = dt.Rows[0]["WARDCODE"].ToString().Trim() + " / " + dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    ssPRT2_Sheet1.Cells[3, 12].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());
                    ssPRT2_Sheet1.Cells[3, 16].Text = clsVbfunc.READ_INSA_BUSE(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());

                    strTemp = "";

                    for (i = 1; i < 18; i++)
                    {
                        if (i == 6)
                            continue;

                        strTemp = (dt.Rows[0]["REMCODE" + Convert.ToString(i)].ToString().Trim() != "" ? dt.Rows[0]["REMCODE" + Convert.ToString(i)].ToString().Trim() + ", " : "");
                    }

                    strTemp = (dt.Rows[0]["REMCODE6"].ToString().Trim() != "" ? dt.Rows[0]["REMCODE6"].ToString().Trim() + ", " : "");

                    if (strTemp != "")
                        strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2);

                    ssPRT2_Sheet1.Cells[4, 4].Text = strTemp;
                    ssPRT2_Sheet1.Cells[4, 8].Text = dt.Rows[0]["FASTRETURN"].ToString().Trim();
                    ssPRT2_Sheet1.Cells[4, 14].Text = dt.Rows[0]["HOSP"].ToString().Trim();

                    ssPRT2_Sheet1.Cells[5, 4].Text = dt.Rows[0]["RETURNMEMO"].ToString().Trim();
                    ssPRT2_Sheet1.Cells[5, 14].Text = dt.Rows[0]["PHAR"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                ssPRT2_Sheet1.Cells[6, 6].Value = false;
                ssPRT2_Sheet1.Cells[6, 8].Value = true;

                ssPRT2_Sheet1.Cells[7, 6].Value = false;
                ssPRT2_Sheet1.Cells[7, 8].Value = true;

                SQL = "";
                SQL = " SELECT REMARK1, REMARK2, REMARK3, REMARK4, ";
                SQL = SQL + ComNum.VBLF + " REMARK5, REMARK6, REMARK7, REMARK8, ";
                SQL = SQL + ComNum.VBLF + " REMARK9, REMARK10, REMARK11, REMARK12, ";
                SQL = SQL + ComNum.VBLF + " REMARK13, BLOOD, ROWID, EDICODE, METFORMIN,";
                SQL = SQL + ComNum.VBLF + " REMARK14, QTY, NAL, DOSCODE, DECODE(TUYAKGBN, '1', '●', '') TUYAKGBN, RP, ";
                SQL = SQL + ComNum.VBLF + " NOT_SIKBYUL, ROWID  ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.DRUG_HOISLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + strWRTNO;
                SQL = SQL + ComNum.VBLF + " ORDER BY RP ASC, EDICODE ASC";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        intRow = DRAW_LINE();

                        ssPRT2_Sheet1.Cells[intRow, 0].Text = Convert.ToString(i + 1);
                        ssPRT2_Sheet1.Cells[intRow, 2].Text = dt.Rows[i]["RP"].ToString().Trim();

                        ssPRT2_Sheet1.Cells[intRow, 4].Text = dt.Rows[i]["REMARK1"].ToString().Trim()
                        + (dt.Rows[i]["EDICODE"].ToString().Trim() != "" ? ComNum.VBLF + dt.Rows[i]["EDICODE"].ToString().Trim() : "")
                        + (dt.Rows[i]["REMARK7"].ToString().Trim() != "" ? ComNum.VBLF + dt.Rows[i]["REMARK7"].ToString().Trim() : "");
                        ssPRT2_Sheet1.Cells[intRow + 1, 4].Text = dt.Rows[i]["REMARK2"].ToString().Trim();


                        ssPRT2_Sheet1.Cells[intRow, 6].Text = dt.Rows[i]["REMARK6"].ToString().Trim(); // '색상모양
                        ssPRT2_Sheet1.Cells[intRow, 8].Text = "앞) " + dt.Rows[i]["REMARK9"].ToString().Trim();

                        ssPRT2_Sheet1.Cells[intRow + 1, 6].Text = "제형:" + dt.Rows[i]["REMARK8"].ToString().Trim(); // '색상모양
                        ssPRT2_Sheet1.Cells[intRow + 1, 8].Text = "앞) " + dt.Rows[i]["REMARK10"].ToString().Trim();

                        ssPRT2_Sheet1.Cells[intRow, 10].Text = dt.Rows[i]["REMARK3"].ToString().Trim();

                        ssPRT2_Sheet1.Cells[intRow, 12].Text = dt.Rows[i]["QTY"].ToString().Trim();

                        if (VB.Val(ssPRT2_Sheet1.Cells[intRow, 12].Text.Trim()) <= 0)
                        {
                            ssPRT2_Sheet1.Cells[intRow, 12].Text = "";
                        }

                        ssPRT2_Sheet1.Cells[intRow, 14].Text = "";

                        if (VB.Val(dt.Rows[i]["NAL"].ToString().Trim()) > 0)
                        {
                            ssPRT2_Sheet1.Cells[intRow, 14].Text = dt.Rows[i]["NAL"].ToString().Trim() + "일";
                        }
                        else
                        {
                            ssPRT2_Sheet1.Cells[intRow, 14].Text = "";
                        }

                        strREMARK14 = dt.Rows[i]["REMARK14"].ToString().Trim();

                        if (VB.Val(strREMARK14) > 0)
                        {
                            strREMARK14 = "(수량:" + strREMARK14 + ")";
                        }

                        ssPRT2_Sheet1.Cells[intRow + 1, 12].Text = READ_DOSNAME(dt.Rows[i]["DOSCODE"].ToString().Trim()) + ComNum.VBLF + strREMARK14;

                        //            SSPRT2.Row = nRow
                        strJEP1 = dt.Rows[i]["REMARK5"].ToString().Trim();
                        strJEP2 = dt.Rows[i]["REMARK12"].ToString().Trim();
                        strJEP3 = dt.Rows[i]["REMARK13"].ToString().Trim();

                        if (strJEP1 == "")
                            strJEP1 = ComNum.VBLF + strJEP1;
                        if (strJEP2 == "")
                            strJEP2 = ComNum.VBLF + strJEP2;
                        if (strJEP3 == "")
                            strJEP3 = ComNum.VBLF + strJEP3;

                        ssPRT2_Sheet1.Cells[intRow, 16].Text = READ_USED_GUBUN(dt.Rows[i]["REMARK11"].ToString().Trim()) + (strJEP1 + strJEP2 + strJEP3 != "" ? ComNum.VBLF + strJEP1 + strJEP2 + strJEP3 : "").Trim();
                        ssPRT2_Sheet1.Cells[intRow, 18].Text = dt.Rows[i]["TUYAKGBN"].ToString().Trim();
                        ssPRT2_Sheet1.Cells[intRow, 20].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        ssPRT2_Sheet1.Rows.Get(intRow).ForeColor = Color.FromArgb(0, 0, 0);
                        ssPRT2_Sheet1.Rows.Get(intRow + 1).ForeColor = Color.FromArgb(0, 0, 0);

                        if (dt.Rows[i]["BLOOD"].ToString().Trim() == "1") // '항 혈전제 표시
                        {
                            ssPRT2_Sheet1.Cells[intRow, 4].Text = "★" + ssPRT2_Sheet1.Cells[intRow, 4].Text;

                            ssPRT2_Sheet1.Cells[6, 6].Value = true;
                            ssPRT2_Sheet1.Cells[6, 8].Value = false;

                            ssPRT2_Sheet1.Rows.Get(intRow).ForeColor = Color.FromArgb(10, 10, 220);
                            ssPRT2_Sheet1.Rows.Get(intRow + 1).ForeColor = Color.FromArgb(10, 10, 220);
                        }

                        if (dt.Rows[i]["METFORMIN"].ToString().Trim() == "1") //'METFORMIN
                        {
                            ssPRT2_Sheet1.Cells[intRow, 4].Text = "▣" + ssPRT2_Sheet1.Cells[intRow, 4].Text;

                            ssPRT2_Sheet1.Cells[7, 6].Value = true;
                            ssPRT2_Sheet1.Cells[7, 8].Value = false;

                            ssPRT2_Sheet1.Rows.Get(intRow).ForeColor = Color.FromArgb(220, 10, 10);
                            ssPRT2_Sheet1.Rows.Get(intRow + 1).ForeColor = Color.FromArgb(220, 10, 10);
                        }

                        ssPRT2_Sheet1.SetRowHeight(intRow, Convert.ToInt32(ssPRT2_Sheet1.GetPreferredRowHeight(intRow)));
                        ssPRT2_Sheet1.SetRowHeight(intRow + 1, Convert.ToInt32(ssPRT2_Sheet1.GetPreferredRowHeight(intRow + 1)));
                    }
                }

                dt.Dispose();
                dt = null;

                DRAW_BOTTOM();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }


        }

        private void DRAW_BOTTOM()
        {
            int nRow1 = 0;
            int nROW2 = 0;
            int nROW3 = 0;
            int nROW4 = 0;
            int nROW5 = 0;
            int nROW6 = 0;
            int nROW7 = 0;

            TextCellType cellText = new TextCellType();

            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 5;
            nRow1 = ssPRT2_Sheet1.RowCount - 1;
            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            nROW2 = ssPRT2_Sheet1.RowCount - 1;
            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            nROW3 = ssPRT2_Sheet1.RowCount - 1;
            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            nROW4 = ssPRT2_Sheet1.RowCount - 1;
            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            nROW5 = ssPRT2_Sheet1.RowCount - 1;
            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            nROW6 = ssPRT2_Sheet1.RowCount - 1;
            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            nROW7 = ssPRT2_Sheet1.RowCount - 1;

            ssPRT2_Sheet1.AddSpanCell(nRow1, 0, 1, 20);
            ssPRT2_Sheet1.AddSpanCell(nROW2, 3, 1, 17);
            ssPRT2_Sheet1.AddSpanCell(nROW3, 3, 1, 17);
            ssPRT2_Sheet1.AddSpanCell(nROW4, 3, 1, 17);
            ssPRT2_Sheet1.AddSpanCell(nROW5, 3, 1, 17);
            ssPRT2_Sheet1.AddSpanCell(nROW6, 3, 1, 17);
            ssPRT2_Sheet1.AddSpanCell(nROW7, 3, 1, 17);

            ssPRT2_Sheet1.Cells[nRow1, 0].CellType = cellText;
            ssPRT2_Sheet1.Cells[nRow1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPRT2_Sheet1.Cells[nRow1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[nRow1, 0].Text = "[회신서 작성 및 지참약 투여에 대한 참고사항]";
            ssPRT2_Sheet1.SetRowHeight(nRow1, 20);

            ssPRT2_Sheet1.Cells[nRow1, 0].Font = new Font("맑은 고딕", 11, FontStyle.Bold);

            ssPRT2_Sheet1.Cells[nROW2, 3].Text = "1. 원내 사용여부 및 대체약 정보에 '◎원내-동일성분, 제형다름(약동학적 차이 있음)'으로 표시된 경우,";
            ssPRT2_Sheet1.SetRowHeight(nROW2, 17);
            ssPRT2_Sheet1.Cells[nROW3, 3].Text = "   대체약 처방 시 용량, 용법을 신중히 고려하시기 바랍니다.";
            ssPRT2_Sheet1.SetRowHeight(nROW3, 17);
            ssPRT2_Sheet1.Cells[nROW4, 3].Text = "2. 다음과 같은 경우 입원 시 지참약의 투여가 불가능합니다.";
            ssPRT2_Sheet1.SetRowHeight(nROW4, 17);
            ssPRT2_Sheet1.Cells[nROW5, 3].Text = "   1) 1회분 포장으로 되어 있는 2종 이상의 약품(No. 우측의 그룹 표시가 같음) 중 일부 투여";
            ssPRT2_Sheet1.SetRowHeight(nROW5, 17);
            ssPRT2_Sheet1.Cells[nROW6, 3].Text = "   2) 식별불가능한 약품 및 식별불가능한 약품과 함께 1회분 포장으로 되어 있는 약품 전체";
            ssPRT2_Sheet1.SetRowHeight(nROW6, 17);
            ssPRT2_Sheet1.Cells[nROW7, 3].Text = "   3) 타의료기관에서 처방된 마약";
            ssPRT2_Sheet1.SetRowHeight(nROW7, 17);

            ssPRT2_Sheet1.Cells[nROW2, 3, nROW7, 4].CellType = cellText;
            ssPRT2_Sheet1.Cells[nROW2, 3, nROW7, 4].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT2_Sheet1.Cells[nROW2, 3, nROW7, 4].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            ssPRT2_Sheet1.Cells[nROW2, 3, nROW7, 4].Font = new Font("돋움체", 9, FontStyle.Regular);

            for (int i = nRow1; i <= nROW7; i++)
            {
                if (nRow1 == i)
                {
                    ssPRT2_Sheet1.Cells[nRow1, 0].Border = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                                new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                                new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                                null);
                    ssPRT2_Sheet1.Cells[nRow1, 1, nRow1, 3].Border = new ComplexBorder(null,
                                                                new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                                null,
                                                                null);
                    ssPRT2_Sheet1.Cells[nROW7, 3].Border = new ComplexBorder(null,
                                                                new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                                new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                                null);
                }
                else if (nROW7 == i)
                {
                    ssPRT2_Sheet1.Cells[nROW7, 0].Border = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                                null,
                                                                null,
                                                                new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black));
                    ssPRT2_Sheet1.Cells[nROW7, 1, nROW7, 3].Border = new ComplexBorder(null,
                                                                null,
                                                                null,
                                                                new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black));
                    ssPRT2_Sheet1.Cells[nROW7, 3].Border = new ComplexBorder(null,
                                                                null,
                                                                new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                                new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black));
                }
                else
                {
                    ssPRT2_Sheet1.Cells[i, 0].Border = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                                null,
                                                                null,
                                                                null);
                    ssPRT2_Sheet1.Cells[i, 3].Border = new ComplexBorder(null,
                                                                null,
                                                                new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                                null);
                }
            }

            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;

            ssPRT2_Sheet1.AddSpanCell(ssPRT2_Sheet1.RowCount - 1, 0, 1, 20);
            ssPRT2_Sheet1.SetRowHeight(ssPRT2_Sheet1.RowCount - 1, 20);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Font = new Font("돋움체", 11, FontStyle.Regular);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].CellType = cellText;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Text = "포항성모병원 약제과";
        }

        private void DRAW_LINE3()
        {
            FarPoint.Win.ComplexBorder BorderThreeThin = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderTwoThin = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderRightDashed = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dashed, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderBottomDashed = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dashed, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderTwoDashed = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dashed, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dashed, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.Spread.CellType.TextCellType TEXTTYPE = new FarPoint.Win.Spread.CellType.TextCellType();
            TEXTTYPE.Multiline = true;
            TEXTTYPE.WordWrap = true;
            TEXTTYPE.MaxLength = 2000;

            FarPoint.Win.Spread.CellType.ImageCellType IMGTYPE = new FarPoint.Win.Spread.CellType.ImageCellType();
            IMGTYPE.Style = FarPoint.Win.RenderStyle.Stretch;

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 2;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 2, 80);
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, 40);

            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 0].RowSpan = 2;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 0].Border = BorderThreeThin;

            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 1].RowSpan = 2;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 1].Border = BorderTwoThin;

            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 2].ColumnSpan = 2;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 2].CellType = IMGTYPE;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 2].Border = BorderTwoDashed;

            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 2].Border = BorderRightDashed;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 3].Border = BorderRightDashed;

            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 4].Border = BorderBottomDashed;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 4].Border = BorderTwoThin;


            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].ColumnSpan = 3;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].Border = BorderTwoDashed;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].Font = new Font("맑은 고딕", 9F);

            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 5].ColumnSpan = 3;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 5].Border = BorderRightDashed;


            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 8].RowSpan = 2;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 8].Border = BorderTwoThin;


            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 9].Border = BorderBottomDashed;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 9].Border = BorderTwoThin;


            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].RowSpan = 2;
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].Border = BorderTwoDashed;

            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].ColumnSpan = 3;
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 11].Border = BorderTwoDashed;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].RowSpan = 2;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].Border = BorderRightDashed;


            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 13].RowSpan = 2;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 13].Border = BorderTwoThin;

        }

        private void DRAW_BOTTOM3()
        {
            FarPoint.Win.ComplexBorder BorderWhite = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderTop = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderMid = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderBottom = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0, ssPRT3_Sheet1.RowCount - 1, ssPRT3_Sheet1.ColumnCount - 1].Border = BorderWhite;

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT + 3);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderTop;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = "[회신서 작성 및 지참약 투여에 대한 참고사항]";

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderMid;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = VB.Space(10) + "1. 원내 사용여부 및 대체약 정보에 '◎원내-동일성분, 제형다름(약동학적 차이 있음)'으로 표시된 경우, 대체약 처방 시 용량, 용법을 신중히 고려하시기 바랍니다.";

            //ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            //ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderMid;
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = VB.Space(13) + "대체약 처방 시 용량, 용법을 신중히 고려하시기 바랍니다.";

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderMid;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = VB.Space(10) + "2. 다음과 같은 경우 입원 시 지참약의 투여가 불가능합니다.";

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderMid;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = VB.Space(13) + "1) 1회분 포장으로 되어 있는 2종 이상의 약품(No. 우측의 그룹 표시가 같음) 중 일부 투여";

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderMid;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = VB.Space(13) + "2) 식별불가능한 약품 및 식별불가능한 약품과 함께 1회분 포장으로 되어 있는 약품 전체";

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderBottom;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = VB.Space(13) + "3) 타의료기관에서 처방된 마약";

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, 5);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0, ssPRT3_Sheet1.RowCount - 1, ssPRT3_Sheet1.ColumnCount - 1].Border = BorderWhite;

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderWhite;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 10F);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = "포항성모병원 약제팀";
        }

        private string READ_DOSNAME(string strDOSCODE)
        {
            DataTable dt = null;
            string strVal = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT IDOSNAME ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.OCS_ODOSAGE ";
                SQL = SQL + ComNum.VBLF + " WHERE DOSCODE = '" + strDOSCODE + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["IDOSNAME"].ToString().Trim();
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return strVal;

        }

        private int DRAW_LINE()
        {
            int intRow1 = 0;
            int intRow2 = 0;

            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            intRow1 = ssPRT2_Sheet1.RowCount - 1;
            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            intRow2 = ssPRT2_Sheet1.RowCount - 1;

            ssPRT2_Sheet1.AddSpanCell(intRow1, 0, 2, 2);
            ssPRT2_Sheet1.AddSpanCell(intRow1, 2, 2, 2);
            ssPRT2_Sheet1.AddSpanCell(intRow1, 4, 1, 2);
            ssPRT2_Sheet1.AddSpanCell(intRow2, 4, 1, 2);
            ssPRT2_Sheet1.AddSpanCell(intRow1, 6, 1, 2);
            ssPRT2_Sheet1.AddSpanCell(intRow2, 6, 1, 2);
            ssPRT2_Sheet1.AddSpanCell(intRow1, 8, 1, 2);
            ssPRT2_Sheet1.AddSpanCell(intRow2, 8, 1, 2);
            ssPRT2_Sheet1.AddSpanCell(intRow1, 10, 2, 2);
            ssPRT2_Sheet1.AddSpanCell(intRow1, 12, 1, 2);
            ssPRT2_Sheet1.AddSpanCell(intRow1, 14, 1, 2);
            ssPRT2_Sheet1.AddSpanCell(intRow2, 12, 1, 4);
            ssPRT2_Sheet1.AddSpanCell(intRow1, 16, 2, 2);
            ssPRT2_Sheet1.AddSpanCell(intRow1, 18, 2, 2);

            LineBorder Border4 = new LineBorder(Color.Black, 1, true, true, true, true);

            ssPRT2_Sheet1.Cells[intRow1, 0, intRow2, 1].Border = Border4;
            ssPRT2_Sheet1.Cells[intRow1, 2, intRow2, 3].Border = Border4;
            ssPRT2_Sheet1.Cells[intRow1, 4, intRow2, 5].Border = Border4;
            ssPRT2_Sheet1.Cells[intRow1, 6, intRow2, 9].Border = Border4;
            ssPRT2_Sheet1.Cells[intRow1, 10, intRow2, 11].Border = Border4;
            ssPRT2_Sheet1.Cells[intRow1, 12, intRow2, 15].Border = Border4;
            ssPRT2_Sheet1.Cells[intRow1, 16, intRow2, 17].Border = Border4;
            ssPRT2_Sheet1.Cells[intRow1, 18, intRow2, 19].Border = Border4;

            ComplexBorder Border1 = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                          new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                          new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                          new ComplexBorderSide(ComplexBorderSideStyle.HairLine, Color.Black));

            ComplexBorder Border2 = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                      new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                      new ComplexBorderSide(ComplexBorderSideStyle.HairLine, Color.Black),
                                                      new ComplexBorderSide(ComplexBorderSideStyle.HairLine, Color.Black));

            ComplexBorder Border3 = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.HairLine, Color.Black),
                                                      new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                      new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                      new ComplexBorderSide(ComplexBorderSideStyle.HairLine, Color.Black));

            ComplexBorder Border5 = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                      new ComplexBorderSide(ComplexBorderSideStyle.HairLine, Color.Black),
                                                      new ComplexBorderSide(ComplexBorderSideStyle.HairLine, Color.Black),
                                                      new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black));

            ComplexBorder Border6 = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.HairLine, Color.Black),
                                                      new ComplexBorderSide(ComplexBorderSideStyle.HairLine, Color.Black),
                                                      new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                      new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black));

            ComplexBorder Border7 = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                      new ComplexBorderSide(ComplexBorderSideStyle.HairLine, Color.Black),
                                                      new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black),
                                                      new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black));

            ssPRT2_Sheet1.Cells[intRow1, 4].Border = Border1;
            ssPRT2_Sheet1.Cells[intRow1, 5].Border = Border1;
            ssPRT2_Sheet1.Cells[intRow1, 6].Border = Border2;
            ssPRT2_Sheet1.Cells[intRow1, 7].Border = Border2;
            ssPRT2_Sheet1.Cells[intRow1, 8].Border = Border3;
            ssPRT2_Sheet1.Cells[intRow1, 9].Border = Border3;

            ssPRT2_Sheet1.Cells[intRow2, 4].Border = Border7;
            ssPRT2_Sheet1.Cells[intRow2, 5].Border = Border7;
            ssPRT2_Sheet1.Cells[intRow2, 6].Border = Border5;
            ssPRT2_Sheet1.Cells[intRow2, 7].Border = Border5;
            ssPRT2_Sheet1.Cells[intRow2, 8].Border = Border6;
            ssPRT2_Sheet1.Cells[intRow2, 9].Border = Border6;

            ssPRT2_Sheet1.Cells[intRow1, 12].Border = Border2;
            ssPRT2_Sheet1.Cells[intRow1, 13].Border = Border2;
            ssPRT2_Sheet1.Cells[intRow1, 14].Border = Border3;
            ssPRT2_Sheet1.Cells[intRow1, 15].Border = Border3;

            ssPRT2_Sheet1.Cells[intRow2, 12].Border = Border7;
            ssPRT2_Sheet1.Cells[intRow2, 13].Border = Border7;
            ssPRT2_Sheet1.Cells[intRow2, 14].Border = Border7;
            ssPRT2_Sheet1.Cells[intRow2, 15].Border = Border7;

            TextCellType txtcellType = new TextCellType();
            txtcellType.Multiline = true;
            txtcellType.WordWrap = true;

            ssPRT2_Sheet1.Cells[intRow1, 0].CellType = txtcellType;
            ssPRT2_Sheet1.Cells[intRow1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 0].Text = "";

            ssPRT2_Sheet1.Cells[intRow1, 2].CellType = txtcellType;
            ssPRT2_Sheet1.Cells[intRow1, 2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 2].Text = "";

            //txtcellType.WordWrap = true;
            ssPRT2_Sheet1.Cells[intRow1, 4, intRow2, 4].CellType = txtcellType;
            ssPRT2_Sheet1.Cells[intRow1, 4, intRow2, 4].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT2_Sheet1.Cells[intRow1, 4, intRow2, 4].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 4, intRow2, 4].Text = "";
            ssPRT2_Sheet1.Cells[intRow1, 4, intRow2, 4].Locked = true;

            ssPRT2_Sheet1.Cells[intRow1, 6, intRow2, 6].CellType = txtcellType;
            ssPRT2_Sheet1.Cells[intRow1, 6, intRow2, 6].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT2_Sheet1.Cells[intRow1, 6, intRow2, 6].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 6, intRow2, 6].Text = "";
            ssPRT2_Sheet1.Cells[intRow1, 6, intRow2, 6].Locked = true;

            ssPRT2_Sheet1.Cells[intRow1, 8, intRow2, 8].CellType = txtcellType;
            ssPRT2_Sheet1.Cells[intRow1, 8, intRow2, 8].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT2_Sheet1.Cells[intRow1, 8, intRow2, 8].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 8, intRow2, 8].Text = "";
            ssPRT2_Sheet1.Cells[intRow1, 8, intRow2, 8].Locked = true;

            ssPRT2_Sheet1.Cells[intRow1, 10].CellType = txtcellType;
            ssPRT2_Sheet1.Cells[intRow1, 10].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT2_Sheet1.Cells[intRow1, 10].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 10].Text = "";

            ssPRT2_Sheet1.Cells[intRow2, 12].CellType = txtcellType;
            ssPRT2_Sheet1.Cells[intRow2, 12].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow2, 12].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow2, 12].Text = "";

            //txtcellType.WordWrap = false;
            ssPRT2_Sheet1.Cells[intRow1, 12].CellType = txtcellType;
            ssPRT2_Sheet1.Cells[intRow1, 12].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 12].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 12].Text = "";

            ssPRT2_Sheet1.Cells[intRow1, 14].CellType = txtcellType;
            ssPRT2_Sheet1.Cells[intRow1, 14].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 14].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 14].Text = "";

            //txtcellType.WordWrap = true;
            ssPRT2_Sheet1.Cells[intRow1, 16].CellType = txtcellType;
            ssPRT2_Sheet1.Cells[intRow1, 16].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 16].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 16].Text = "";

            ssPRT2_Sheet1.Cells[intRow1, 18].CellType = txtcellType;
            ssPRT2_Sheet1.Cells[intRow1, 18].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 18].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[intRow1, 18].Text = "";

            //ssPRT2_Sheet1.Rows.Get(intRow1, intRow2).Font = new Font("굴림", 8, FontStyle.Regular);

            return intRow1;

        }

        private void SSPRT2_CLEAR()
        {
            ssPRT2_Sheet1.RowCount = 10;

            ssPRT2_Sheet1.Cells[1, 4, 3, 4].Text = "";
            ssPRT2_Sheet1.Cells[1, 8, 3, 8].Text = "";
            ssPRT2_Sheet1.Cells[1, 12, 3, 12].Text = "";
            ssPRT2_Sheet1.Cells[1, 16, 3, 16].Text = "";

            ssPRT2_Sheet1.Cells[4, 4].Text = "";
            ssPRT2_Sheet1.Cells[4, 8].Text = "";
            ssPRT2_Sheet1.Cells[4, 14].Text = "";

            ssPRT2_Sheet1.Cells[5, 5].Text = "";
            ssPRT2_Sheet1.Cells[5, 14].Text = "";
        }

        private void ssPRT3_CLEAR()
        {
            ssPRT3_Sheet1.RowCount = 12;

            ssPRT3_Sheet1.Cells[1, 3].Text = "";
            ssPRT3_Sheet1.Cells[1, 5].Text = "";
            ssPRT3_Sheet1.Cells[1, 8].Text = "";
            ssPRT3_Sheet1.Cells[1, 10].Text = "";

            ssPRT3_Sheet1.Cells[2, 3].Text = "";
            ssPRT3_Sheet1.Cells[2, 5].Text = "";
            ssPRT3_Sheet1.Cells[2, 8].Text = "";
            ssPRT3_Sheet1.Cells[2, 10].Text = "";
            ssPRT3_Sheet1.Cells[2, 13].Text = "";

            ssPRT3_Sheet1.Cells[3, 3].Text = "";
            ssPRT3_Sheet1.Cells[3, 7].Text = "";
            ssPRT3_Sheet1.Cells[3, 10].Text = "";
            ssPRT3_Sheet1.Cells[3, 11].Text = "";
            ssPRT3_Sheet1.Cells[3, 13].Text = "";

            ssPRT3_Sheet1.Cells[4, 3].Text = "";
            ssPRT3_Sheet1.Cells[4, 13].Text = "";

            ssPRT3_Sheet1.Cells[5, 3].Text = "";
            ssPRT3_Sheet1.Cells[5, 5].Text = "";

            ssPRT3_Sheet1.Cells[6, 3].Text = "";
            ssPRT3_Sheet1.Cells[7, 3].Text = "";
        }

        private void ssECG_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strROWID = "";
            string strResult = "";

            if (ssECG_Sheet1.RowCount == 0)
                return;
            if (e.RowHeader == true || e.ColumnHeader == true)
                return;

            strResult = ssECG_Sheet1.Cells[e.Row, 7].Text.Trim();
            strROWID = ssECG_Sheet1.Cells[e.Row, 9].Text.Trim();

            //'EKG VIEWER 실행
            if (strResult != "")
            {
                // '파일 다온로드 '파일 실행
                ECGFILE_DBToFile(strROWID, txtPtNo.Text, "1");
            }
        }

        private void ssEEG_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            if (ssEEG_Sheet1.RowCount == 0)
                return;
            if (e.RowHeader == true || e.ColumnHeader == true)
                return;

            DataTable dt = null;
            int nWRTNO = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            txtEEG.Text = "";

            //      '판독결과
            try
            {

                nWRTNO = Convert.ToInt32(VB.Val(ssEEG_Sheet1.Cells[e.Row, 8].Text.Trim()));

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT XDRCODE1,RESULT,RESULT1,APPROVE FROM ADMIN.XRAY_RESULTNEW ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + nWRTNO + " ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["APPROVE"].ToString().Trim() == "N")
                    {
                        txtEEG.Text = ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                        txtEEG.Text = txtEEG.Text + "      ▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                    }
                    else
                    {
                        txtEEG.Text = dt.Rows[0]["RESULT"].ToString().Trim() + dt.Rows[0]["RESULT1"].ToString().Trim();
                    }
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssEMG_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            if (ssEMG_Sheet1.RowCount == 0)
                return;
            if (e.RowHeader == true || e.ColumnHeader == true)
                return;

            DataTable dt = null;
            string strWRTNO = "";
            string strEmgWrtno = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            txtEEG.Text = "";

            //      '판독결과
            try
            {
                if (e.Column >= 0 && e.Column <= 7) // '판독 결과 표시
                {
                    strWRTNO = ssEMG_Sheet1.Cells[e.Row, 10].Text.Trim();

                    SQL = " SELECT XDRCODE1,RESULT,RESULT1,APPROVE FROM ADMIN.XRAY_RESULTNEW ";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + strWRTNO + " ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["APPROVE"].ToString().Trim() == "N")
                        {
                            txtEmgResult.Text = ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                            txtEmgResult.Text = txtEmgResult.Text + "      ▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                        }
                        else
                        {
                            txtEmgResult.ReadOnly = true;
                            txtEmgResult.Text = dt.Rows[0]["RESULT"].ToString().Trim() + dt.Rows[0]["RESULT1"].ToString().Trim();
                        }
                    }

                    mstrDrCode = "";
                    mstrDrCode = dt.Rows[0]["XDRCODE1"].ToString().Trim();

                    dt.Dispose();
                    dt = null;

                }
                else if (e.Column == 8)
                {
                    strEmgWrtno = ssEMG_Sheet1.Cells[e.Row, 11].Text.Trim();

                    if (strEmgWrtno != "")
                    {
                        EMG_FILE_DBToFile(strEmgWrtno, txtPtNo.Text.Trim(), "1");       //'EMG 영상 표시
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssNSTList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            if (ssNSTList_Sheet1.RowCount == 0)
                return;
            if (e.RowHeader == true || e.ColumnHeader == true)
                return;

            clsPublic.GnLogOutCNT = 0;
            string strWRTNO = "";
            string strPano = "";
            string strIPDNO = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;
            DataTable dt = null;

            strWRTNO = ssNSTList_Sheet1.Cells[e.Row, 1].Text.Trim();

            if (strWRTNO == "")
                return;

            strWRTNO = ssNSTList_Sheet1.Cells[e.Row, 1].Text.Trim();
            strPano = ssNSTList_Sheet1.Cells[e.Row, 2].Text.Trim();
            strIPDNO = ssNSTList_Sheet1.Cells[e.Row, 3].Text.Trim();

            frmNSTView_New frmNSTView_NewX = new frmNSTView_New();
            clsPat.PATi.WRTNO = Convert.ToDouble(strWRTNO);
            clsPat.PATi.IPDNO = Convert.ToDouble(strIPDNO);
            clsPat.PATi.Pano = strPano;
            frmNSTView_NewX.ShowDialog();

            frmNSTView_NewX = null;


            //PSMHOCSOLD.clsOcs_old psmhOcsOld = null;
            //psmhOcsOld = new PSMHOCSOLD.clsOcs_old();
            //psmhOcsOld.DbCon();
            //psmhOcsOld.ShowForm_FrmNSTView_New(strWRTNO, strIPDNO, strPano, "", "",
            //                    "", "", "", "", "",
            //                    "", "", "", "", "",
            //                    "", "", "");
            //psmhOcsOld.DbDisCon();
            //psmhOcsOld = null;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                clsPublic.GstrHelpCode = "";

                SQL = "";
                SQL = " SELECT A.DRCODE, B.SABUN ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.IPD_NEW_MASTER A, ADMIN.OCS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + strPano + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = " + strIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND A.DRCODE = B.DRCODE ";
                SQL = SQL + ComNum.VBLF + "   AND B.SABUN = '" + VB.Val(clsType.User.IdNumber).ToString("00000") + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                        return; //권한 확인

                    SQL = "";
                    SQL = "UPDATE ADMIN.DIET_NST_PROGRESS SET ";
                    SQL = SQL + ComNum.VBLF + " VIEW_SABUN = " + clsType.User.IdNumber + ",";
                    SQL = SQL + ComNum.VBLF + " VIEW_WRITEDATE = SYSDATE ";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + strWRTNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                dt.Dispose();
                dt = null;


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssPConsult_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            if (ssPConsult_Sheet1.RowCount == 0)
                return;
            if (e.RowHeader == true || e.ColumnHeader == true)
                return;

            string strPART = "";
            string strDRUGCODE = "";
            string strPano = "";
            string strIPDNO = "";
            string strTREATNO = "";
            string strPROGRESS = "";
            string strROWID = "";
            string strSEQNO = "";

            //if (strSEQNO != "")
            //{
            //    frmComSupPharConsultReturnDetail frm = new frmComSupPharConsultReturnDetail(strSEQNO);
            //    frm.ShowDialog();
            //}

            try
            {

                strPROGRESS = ssPConsult_Sheet1.Cells[e.Row, 0].Text.Trim();
                strPano = ssPConsult_Sheet1.Cells[e.Row, 5].Text.Trim();
                strSEQNO = ssPConsult_Sheet1.Cells[e.Row, 13].Text.Trim();
                strROWID = ssPConsult_Sheet1.Cells[e.Row, 14].Text.Trim();

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                                                        ";
                SQL = SQL + ComNum.VBLF + "    PART, SEQNO, IPDNO, TREATNO, PANO, PROGRESS, ORDERCODE    ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.DRUG_PCONSULT A                             ";
                SQL = SQL + ComNum.VBLF + "  INNER JOIN ADMIN.DRUG_SETCODE B                        ";
                SQL = SQL + ComNum.VBLF + "    ON A.ORDERCODE = B.JEPCODE                                ";
                SQL = SQL + ComNum.VBLF + "    AND B.GUBUN = '11'                                        ";
                SQL = SQL + ComNum.VBLF + "  WHERE SEQNO = '" + strSEQNO + "'                            ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                 
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strPART = dt.Rows[0]["PART"].ToString().Trim();
                    strSEQNO = dt.Rows[0]["SEQNO"].ToString().Trim();
                    strIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                    strPano = dt.Rows[0]["PANO"].ToString().Trim();
                    strPROGRESS = dt.Rows[0]["PROGRESS"].ToString().Trim();
                    strDRUGCODE = dt.Rows[0]["ORDERCODE"].ToString().Trim();
                    strTREATNO = dt.Rows[0]["TREATNO"].ToString().Trim();

                    if ((int)VB.Val(strSEQNO) > 1040)
                    {
                        using (frmComSupPharConsultReturnDetailNew f = new frmComSupPharConsultReturnDetailNew(strPART, strSEQNO, strIPDNO, strPano, strPROGRESS, strDRUGCODE))
                        {
                            f.GstrTREATNO = strTREATNO;
                            f.StartPosition = FormStartPosition.CenterParent;
                            f.ShowDialog();
                        }
                    }
                    else
                    {
                        using (frmComSupPharConsultReturnDetail f = new frmComSupPharConsultReturnDetail(strSEQNO))
                        {
                            f.StartPosition = FormStartPosition.CenterParent;
                            f.ShowDialog();
                        }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssPFT_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssPFT_Sheet1.RowCount == 0)
                return;
            if (e.RowHeader == true || e.ColumnHeader == true)
                return;

            string strROWID = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            byte[] Byte = null;
            MemoryStream stream = null;


            strROWID = ssPFT_Sheet1.Cells[e.Row, 4].Text.Trim();

            if (pic2.Top != 0)
                pic2.Top = 0;
            if (pic2.Left != 0)
                pic2.Left = 0;
            if (strROWID == "")
                return;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT  IMAGE ";
                SQL = SQL + ComNum.VBLF + "  FROM  ADMIN.ETC_JUPMST ";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    pic2.Image = null;
                    ComFunc.MsgBox("DB에 없습니다.", "확인");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    Byte = (byte[])(dt.Rows[0]["IMAGE"]);

                    stream = new MemoryStream(Byte);
                    pic2.Image = new Bitmap(stream);
                }

                pic2.Size = pic2.Image.Size;

                dt.Dispose();
                dt = null;

                pic1.Left = 32;
                pic1.Width = 900;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssSixWalkList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssSixWalkList_Sheet1.RowCount == 0)
                return;
            if (e.RowHeader == true || e.ColumnHeader == true)
                return;

            string strROWID = "";

            CLEAR_SHEET_SIXWALK();

            strROWID = ssSixWalkList_Sheet1.Cells[e.Row, 2].Text.Trim();

            if (strROWID != "")
            {
                DISPLAY_SIXWALK(strROWID);
            }
        }

        private void DISPLAY_SIXWALK(string strROWID)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT PTNO, SNAME, AGE, SEX, BDATE, IO, ";
                SQL = SQL + ComNum.VBLF + " MMRC1, MMRC2, MMRC3, ";
                SQL = SQL + ComNum.VBLF + " SAO21, SAO22, SAO23, ";
                SQL = SQL + ComNum.VBLF + " HR1, HR2, HR3, ";
                SQL = SQL + ComNum.VBLF + " DISTANCE1, DISTANCE2, DISTANCE3, ";
                SQL = SQL + ComNum.VBLF + " MIN1, MIN2, MIN3, ";
                SQL = SQL + ComNum.VBLF + " END_SAYU, ROWID, EMRNO, DRCODE, ";
                SQL = SQL + ComNum.VBLF + " IO, DEPTCODE, INDATE, EXAMNAME   ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.ETC_SIXMINWARK";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssSixWalk_Sheet1.Cells[2, 2].Text = dt.Rows[0]["PTNO"].ToString().Trim();
                    ssSixWalk_Sheet1.Cells[3, 2].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssSixWalk_Sheet1.Cells[4, 2].Text = dt.Rows[0]["AGE"].ToString().Trim();
                    ssSixWalk_Sheet1.Cells[5, 2].Text = dt.Rows[0]["SEX"].ToString().Trim();
                    ssSixWalk_Sheet1.Cells[6, 2].Text = dt.Rows[0]["BDATE"].ToString().Trim();

                    ssSixWalk_Sheet1.Cells[9, 3].Text = dt.Rows[0]["MMRC1"].ToString().Trim();
                    ssSixWalk_Sheet1.Cells[9, 4].Text = dt.Rows[0]["MMRC2"].ToString().Trim();
                    ssSixWalk_Sheet1.Cells[9, 5].Text = dt.Rows[0]["MMRC3"].ToString().Trim();

                    ssSixWalk_Sheet1.Cells[10, 3].Text = dt.Rows[0]["SAO21"].ToString().Trim();
                    ssSixWalk_Sheet1.Cells[10, 4].Text = dt.Rows[0]["SAO22"].ToString().Trim();
                    ssSixWalk_Sheet1.Cells[10, 5].Text = dt.Rows[0]["SAO23"].ToString().Trim();

                    ssSixWalk_Sheet1.Cells[11, 3].Text = dt.Rows[0]["HR1"].ToString().Trim();
                    ssSixWalk_Sheet1.Cells[11, 4].Text = dt.Rows[0]["HR2"].ToString().Trim();
                    ssSixWalk_Sheet1.Cells[11, 5].Text = dt.Rows[0]["HR3"].ToString().Trim();

                    ssSixWalk_Sheet1.Cells[12, 3].Text = dt.Rows[0]["DISTANCE1"].ToString().Trim();
                    ssSixWalk_Sheet1.Cells[12, 4].Text = dt.Rows[0]["DISTANCE2"].ToString().Trim();
                    ssSixWalk_Sheet1.Cells[12, 5].Text = dt.Rows[0]["DISTANCE3"].ToString().Trim();

                    ssSixWalk_Sheet1.Cells[13, 3].Text = dt.Rows[0]["MIN1"].ToString().Trim();
                    ssSixWalk_Sheet1.Cells[13, 4].Text = dt.Rows[0]["MIN2"].ToString().Trim();
                    ssSixWalk_Sheet1.Cells[13, 5].Text = dt.Rows[0]["MIN3"].ToString().Trim();

                    ssSixWalk_Sheet1.Cells[16, 1].Text = dt.Rows[0]["END_SAYU"].ToString().Trim();

                    ssSixWalk_Sheet1.Cells[6, 5].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["EXAMNAME"].ToString().Trim());
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void CLEAR_SHEET_SIXWALK()
        {
            ssSixWalk_Sheet1.Cells[2, 2].Text = "";
            ssSixWalk_Sheet1.Cells[3, 2].Text = "";
            ssSixWalk_Sheet1.Cells[4, 2].Text = "";
            ssSixWalk_Sheet1.Cells[5, 2].Text = "";
            ssSixWalk_Sheet1.Cells[6, 2].Text = "";

            ssSixWalk_Sheet1.Cells[6, 5].Text = "";

            ssSixWalk_Sheet1.Cells[9, 3].Text = "";
            ssSixWalk_Sheet1.Cells[9, 4].Text = "";
            ssSixWalk_Sheet1.Cells[9, 5].Text = "";

            ssSixWalk_Sheet1.Cells[10, 3].Text = "";
            ssSixWalk_Sheet1.Cells[10, 4].Text = "";
            ssSixWalk_Sheet1.Cells[10, 5].Text = "";

            ssSixWalk_Sheet1.Cells[11, 3].Text = "";
            ssSixWalk_Sheet1.Cells[11, 4].Text = "";
            ssSixWalk_Sheet1.Cells[11, 5].Text = "";

            ssSixWalk_Sheet1.Cells[12, 3].Text = "";
            ssSixWalk_Sheet1.Cells[12, 4].Text = "";
            ssSixWalk_Sheet1.Cells[12, 5].Text = "";

            ssSixWalk_Sheet1.Cells[13, 3].Text = "";
            ssSixWalk_Sheet1.Cells[13, 4].Text = "";
            ssSixWalk_Sheet1.Cells[13, 5].Text = "";

            ssSixWalk_Sheet1.Cells[16, 1].Text = "";
        }

        private void ssTDMList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            string strPtNo = "";
            string strBDATE = "";

            if (ssTDMList_Sheet1.RowCount == 0)
                return;
            if (e.RowHeader == true || e.ColumnHeader == true)
                return;

            strBDATE = ssTDMList_Sheet1.Cells[e.Row, 0].Text.Trim();
            strPtNo = ssTDMList_Sheet1.Cells[e.Row, 1].Text.Trim();

            if (strPtNo == "")
                return;

            GetTDM(strPtNo, strBDATE);
        }

        private void GetTDM(string argPtNo, string argBDATE)
        {
            int i = 0;

            string strSDate = "";
            string strPano = "";
            string strIllName = "";
            string strOutDate = "";
            string strInDate = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;
            DataTable dt1 = null;

            strPano = argPtNo;
            strSDate = argBDATE;

            if (strPano == "")
                return;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT ROWID FROM ADMIN.DRUG_TDM ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SDATE = TO_DATE('" + strSDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    mstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }
                else
                {
                    mstrROWID = "";
                }

                dt.Dispose();
                dt = null;

                if (mstrROWID == "")
                {
                    SQL = "";
                    SQL = " SELECT TO_CHAR(OUTDATE, 'YYYY-MM-DD') OUTDATE, SNAME, AGE, SEX, ";
                    SQL = SQL + ComNum.VBLF + " WARDCODE, ROOMCODE, DEPTCODE, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, GBSTS ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.IPD_NEW_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE INDATE <= TO_DATE('" + strSDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND (OUTDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD') OR ACTDATE IS NULL) ";
                    SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["GBSTS"].ToString().Trim() == "7")
                        {
                            SQL = "";
                            SQL = " SELECT B.ILLNAMEK FROM ADMIN.MID_DIAGNOSIS A, ADMIN.BAS_ILLS B ";
                            SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND OUTDATE = TO_DATE('" + dt.Rows[0]["OUTDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND B.ILLCODE = TRIM(A.DIAGNOSIS1) ";
                            SQL = SQL + ComNum.VBLF + "   AND SEQNO = 1 ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                strIllName = dt1.Rows[0]["ILLNAMEK"].ToString().Trim();
                            }
                            else
                            {
                                strIllName = "";
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                        else
                        {
                            //        '병동 환자마스타를 읽음

                            SQL = "";
                            SQL = "SELECT DIAGNOSIS FROM ADMIN.NUR_MASTER ";
                            SQL = SQL + ComNum.VBLF + "WHERE PANO='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND INDATE=TO_DATE('" + dt.Rows[0]["INDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                strIllName = dt1.Rows[0]["INDATEDIAGNOSIS"].ToString().Trim();
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        ssTDM_Sheet1.Cells[3, 3].Text = strPano;
                        ssTDM_Sheet1.Cells[4, 3].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                        ssTDM_Sheet1.Cells[5, 3].Text = dt.Rows[0]["SEX"].ToString().Trim();
                        ssTDM_Sheet1.Cells[6, 3].Text = dt.Rows[0]["AGE"].ToString().Trim();
                        ssTDM_Sheet1.Cells[7, 3].Text = strIllName;

                        ssTDM_Sheet1.Cells[3, 5].Text = dt.Rows[0]["WARDCODE"].ToString().Trim() + "/" + dt.Rows[0]["ROOMCODE"].ToString().Trim();
                        ssTDM_Sheet1.Cells[4, 5].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();

                        strOutDate = dt.Rows[0]["OUTDATE"].ToString().Trim();
                        strInDate = dt.Rows[0]["INDATE"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    string strBdate_OLD = "";
                    string strBdate_New = "";

                    SQL = "";
                    SQL = " SELECT SUBCODE, RESULT, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.EXAM_SPECMST A, ADMIN.EXAM_RESULTC B ";
                    SQL = SQL + ComNum.VBLF + " WHERE A.PANO  = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.BDATE >= TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.WORKSTS  = 'C' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.SPECCODE = '013' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.TUBE = '011' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.SPECNO = B.SPECNO ";
                    SQL = SQL + ComNum.VBLF + "   AND B.SUBCODE IN ('CR34','CR35','CR32') ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY BDATE DESC ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strBdate_New = dt.Rows[0]["BDATE"].ToString().Trim();
                        strBdate_OLD = dt.Rows[0]["BDATE"].ToString().Trim();

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i != 0)
                            {
                                strBdate_New = dt.Rows[i]["BDATE"].ToString().Trim();
                            }

                            if (strBdate_New == strBdate_OLD)
                            {

                                switch (dt.Rows[i]["SUBCODE"].ToString().Trim())
                                {
                                    case "CR34":
                                        ssTDM_Sheet1.Cells[16, 3].Text = dt.Rows[i]["RESULT"].ToString().Trim();
                                        break;
                                    case "CR35":
                                        ssTDM_Sheet1.Cells[17, 3].Text = dt.Rows[i]["RESULT"].ToString().Trim();
                                        break;
                                    case "CR32":
                                        ssTDM_Sheet1.Cells[16, 5].Text = dt.Rows[i]["RESULT"].ToString().Trim();
                                        break;
                                }
                                strBdate_OLD = strBdate_New;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, TO_CHAR(JDATE ,'YYYY-MM-DD') JDATE, ";
                    SQL = SQL + ComNum.VBLF + " PANO,SNAME,DEPT,DRNAME,WARDCODE,ROOM,DEPTCODE,SEX,AGE,KI, ";
                    SQL = SQL + ComNum.VBLF + " KG,ILLNAME,CHTIME,CHNUNG,DRUGYAK1,DRUGYAK2,DRUGYAK3,ALT,AST,ALB,";
                    SQL = SQL + ComNum.VBLF + " SCR,OTHERS,VD,T12,CPEAK,CTRO,REMAKR,DRUGNAME ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.DRUG_TDM ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND SDATE = TO_DATE('" + strSDate + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ssTDM_Sheet1.Cells[32, 1].Text = dt.Rows[0]["REMAKR"].ToString().Trim();

                        ssTDM_Sheet1.Cells[0, 2].Text = dt.Rows[0]["JDATE"].ToString().Trim();
                        ssTDM_Sheet1.Cells[9, 2].Text = dt.Rows[0]["DRUGYAK1"].ToString().Trim();
                        ssTDM_Sheet1.Cells[14, 2].Text = dt.Rows[0]["DRUGYAK3"].ToString().Trim();
                        ssTDM_Sheet1.Cells[34, 2].Text = dt.Rows[0]["SDATE"].ToString().Trim();

                        ssTDM_Sheet1.Cells[1, 3].Text = dt.Rows[0]["DEPT"].ToString().Trim();
                        ssTDM_Sheet1.Cells[3, 3].Text = dt.Rows[0]["PANO"].ToString().Trim();
                        ssTDM_Sheet1.Cells[4, 3].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                        ssTDM_Sheet1.Cells[5, 3].Text = dt.Rows[0]["SEX"].ToString().Trim();
                        ssTDM_Sheet1.Cells[6, 3].Text = dt.Rows[0]["AGE"].ToString().Trim();
                        ssTDM_Sheet1.Cells[7, 3].Text = dt.Rows[0]["ILLNAME"].ToString().Trim();
                        ssTDM_Sheet1.Cells[11, 3].Text = dt.Rows[0]["CHTIME"].ToString().Trim();
                        ssTDM_Sheet1.Cells[13, 3].Text = dt.Rows[0]["DRUGYAK2"].ToString().Trim();
                        ssTDM_Sheet1.Cells[16, 3].Text = dt.Rows[0]["ALT"].ToString().Trim();
                        ssTDM_Sheet1.Cells[17, 3].Text = dt.Rows[0]["AST"].ToString().Trim();
                        ssTDM_Sheet1.Cells[18, 3].Text = dt.Rows[i]["OTHERS"].ToString().Trim();
                        ssTDM_Sheet1.Cells[24, 3].Text = dt.Rows[0]["VD"].ToString().Trim();
                        ssTDM_Sheet1.Cells[25, 3].Text = dt.Rows[0]["T12"].ToString().Trim();
                        ssTDM_Sheet1.Cells[28, 3].Text = dt.Rows[0]["CPEAK"].ToString().Trim();
                        ssTDM_Sheet1.Cells[29, 3].Text = dt.Rows[0]["CTRO"].ToString().Trim();

                        ssTDM_Sheet1.Cells[1, 5].Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                        ssTDM_Sheet1.Cells[3, 5].Text = dt.Rows[0]["WARDCODE"].ToString().Trim() + "/" + dt.Rows[0]["ROOM"].ToString().Trim();
                        ssTDM_Sheet1.Cells[4, 5].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                        ssTDM_Sheet1.Cells[5, 5].Text = dt.Rows[0]["KI"].ToString().Trim();
                        ssTDM_Sheet1.Cells[6, 5].Text = dt.Rows[0]["KG"].ToString().Trim();
                        ssTDM_Sheet1.Cells[11, 5].Text = dt.Rows[0]["CHNUNG"].ToString().Trim();
                        ssTDM_Sheet1.Cells[16, 5].Text = dt.Rows[0]["ALB"].ToString().Trim();
                        ssTDM_Sheet1.Cells[17, 5].Text = dt.Rows[0]["SCR"].ToString().Trim();
                        ssTDM_Sheet1.Cells[34, 5].Text = dt.Rows[0]["DRUGNAME"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }


        private void frmViewResult_FormClosed(object sender, FormClosedEventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(@"C:\CMC\");
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo F in files)
            {
                if (F.Name.IndexOf("ECG_") != -1)
                {
                    if (F.Extension == ".ecg")
                    {
                        F.Delete();
                    }
                }
            }
        }

        private void txtExName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnRView.PerformClick();
            }
        }


        /// <summary>
        /// 폼이 Show 상태에서 환자정보를 받아 새로 조회한다.                                                                 
        /// </summary>
        /// <param name="strPano"></param>
        public void rGetDate(string strPano)
        {
            if (strPano.Trim() == "")
            {
                return;
            }

            txtPtNo.Text = strPano;

            txtPtNoKeyDown();

            if (mSetButton == null)
                return;

            ((Button)mSetButton).PerformClick();
        }

        private void btnDiscernPrint_Click(object sender, EventArgs e)
        {
            FarPoint.Win.Spread.FpSpread ssSpread = null;

            ssSpread = ssPRT3;

            //if (tabControl1.SelectedIndex == 0)
            //{ ssSpread = ssPRT; }
            //else if (tabControl1.SelectedIndex == 1)
            //{ ssSpread = ssPRT2; }
            //else if (tabControl1.SelectedIndex == 2)
            //{ ssSpread = ssPRT3; }

            //ssSpread.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssSpread.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssSpread.ActiveSheet.PrintInfo.Margin.Top = 20;
            ssSpread.ActiveSheet.PrintInfo.Margin.Bottom = 20;
            ssSpread.ActiveSheet.PrintInfo.ShowColor = false;
            ssSpread.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssSpread.ActiveSheet.PrintInfo.ShowBorder = false;
            ssSpread.ActiveSheet.PrintInfo.ShowGrid = false;
            ssSpread.ActiveSheet.PrintInfo.ShowShadows = false;
            ssSpread.ActiveSheet.PrintInfo.UseMax = true;
            ssSpread.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssSpread.ActiveSheet.PrintInfo.UseSmartPrint = false;
            ssSpread.ActiveSheet.PrintInfo.ShowPrintDialog = false;
            ssSpread.ActiveSheet.PrintInfo.Preview = false;
            ssSpread.PrintSheet(0);
        }

        private void Dir_Check(string sDirPath, string sExe = "*.jpg")
        {
            DirectoryInfo Dir = new DirectoryInfo(sDirPath);

            if (Dir.Exists == false)
            {
                Dir.Create();
            }
            else
            {
                FileInfo[] File = Dir.GetFiles(sExe, SearchOption.AllDirectories);

                foreach (FileInfo file in File)
                {
                    //file.Delete();
                }
            }
        }

        private void GetDrugInfoImg(string strDifKey, int iRow, int iCol)
        {
            if (strDifKey == "") { return; }

            Dir_Check(@"c:\cmc\ocsexe\dif");

            string strLocal = "";
            string strPath = "";
            string strHost = "";
            string strImgFileName = "";

            Ftpedt FtpedtX = new Ftpedt();

            if (FtpedtX.FtpConnetBatch("192.168.100.33", "pcnfs", "pcnfs1") == false)
            {
                ComFunc.MsgBox("FTP Server Connect ERROR !!!", "오류");
                return;
            }

            Image img = null;

            strImgFileName = strDifKey + ".jpg";

            strLocal = "c:\\cmc\\ocsexe\\dif\\" + strImgFileName;

            strPath = "/pcnfs/firstdis/" + strImgFileName;
            strHost = "/pcnfs/firstdis";

            FileInfo f = new FileInfo(strLocal);
                        
            try
            {
                ////기존파일 삭제
                if (f.Exists == true)
                {
                    f.Delete();
                }
                if (FtpedtX.FtpDownloadEx("192.168.100.33", "pcnfs", "pcnfs1", strLocal, strPath, strHost) == true)
                {
                    //img = Image.FromFile(strLocal);
                    //ssPRT3_Sheet1.Cells[iRow, iCol].Value = img;
                    //img = null;

                    MemoryStream ms = new MemoryStream();
                    Image.FromFile(strLocal).Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    ssPRT3_Sheet1.Cells[iRow, iCol].Value = Image.FromStream(ms);
                }

                FtpedtX.FtpDisConnetBatch();
                FtpedtX = null;
            }
            catch
            {
                if (f.Exists == true)
                {
                    img = Image.FromFile(strLocal);
                    ssPRT3_Sheet1.Cells[iRow, iCol].Value = img;

                    img = null;
                }
                else
                {
                    if (FtpedtX.FtpDownloadEx("192.168.100.33", "pcnfs", "pcnfs1", strLocal, strPath, strHost) == true)
                    {
                        img = Image.FromFile(strLocal);
                        ssPRT3_Sheet1.Cells[iRow, iCol].Value = img;

                        img = null;
                    }
                }
            }
        }

        private string READ_FDRUGCD(string strEDICODE)
        {
            string rtnVal = "";

            if (strEDICODE == "") return rtnVal;

            OracleCommand cmd = new OracleCommand();
            PsmhDb pDbCon = clsDB.DbCon;
            OracleDataReader reader = null;
            DataTable dt = new DataTable();

            string pSearchType = "06";
            string pKeyword = strEDICODE;
            string pScope = "02";


            cmd.Connection = pDbCon.Con;
            cmd.InitialLONGFetchSize = 1000;
            cmd.CommandText = "KOSMOS_DRUG.up_DrugSearch";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("pSearchType", OracleDbType.Varchar2, 9, pSearchType, ParameterDirection.Input);
            cmd.Parameters.Add("pKeyword", OracleDbType.Varchar2, 9, pKeyword, ParameterDirection.Input);
            cmd.Parameters.Add("pScope", OracleDbType.Varchar2, 9, pScope, ParameterDirection.Input);

            cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

            reader = cmd.ExecuteReader();

            dt.Load(reader);
            reader.Dispose();
            reader = null;

            cmd.Dispose();
            cmd = null;

            if (dt == null)
            {
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["FdrugCd"].ToString().Trim();
            }

            return rtnVal;
        }

        private string READ_DRUGNAME(string strCode)
        {
            DataTable dt = null;
            StringBuilder SQL = new StringBuilder();
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL.Clear();
                SQL.AppendLine("SELECT A.SUNEXT, A.HNAME, A.SNAME, A.UNIT, A.EFFECT, ");
                SQL.AppendLine("       A.JEHENG, A.JEHENG2, A.JEHENG3_1, A.JEHENG3_2,  ");
                SQL.AppendLine("       A.JEYAK, B.GELCODE, C.NAME, D.DELDATE ");
                SQL.AppendLine(" FROM ADMIN.OCS_DRUGINFO_NEW A, ADMIN.DRUG_JEP B, ADMIN.AIS_LTD C, ADMIN.BAS_SUT D ");
                SQL.AppendLine(" WHERE A.SUNEXT = '" + strCode + "' ");
                SQL.AppendLine("   AND A.SUNEXT = B.JEPCODE(+) ");
                SQL.AppendLine("   AND B.GELCODE = C.LTDCODE(+) ");
                SQL.AppendLine("   AND A.SUNEXT = D.SUNEXT(+) ");
                SQL.AppendLine("  ORDER BY SUNEXT ASC ");

                SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString(), clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "(" + dt.Rows[0]["HNAME"].ToString().Trim() + ")";
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString(), clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string READ_DOSNAME(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DOSNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ODOSAGE";
                SQL = SQL + ComNum.VBLF + "     WHERE DOSCODE = '" + strSuCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DOSNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return rtnVal;
            }
        }

        private void btnDiscernPrint_old_Click(object sender, EventArgs e)
        {
            FarPoint.Win.Spread.FpSpread ssSpread = null;

            ssSpread = ssPRT2;

            //if (tabControl1.SelectedIndex == 0)
            //{ ssSpread = ssPRT; }
            //else if (tabControl1.SelectedIndex == 1)
            //{ ssSpread = ssPRT2; }
            //else if (tabControl1.SelectedIndex == 2)
            //{ ssSpread = ssPRT3; }

            ssSpread.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;            
            ssSpread.ActiveSheet.PrintInfo.Margin.Top = 20;
            ssSpread.ActiveSheet.PrintInfo.Margin.Bottom = 20;
            ssSpread.ActiveSheet.PrintInfo.ShowColor = false;
            ssSpread.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssSpread.ActiveSheet.PrintInfo.ShowBorder = false;
            ssSpread.ActiveSheet.PrintInfo.ShowGrid = false;
            ssSpread.ActiveSheet.PrintInfo.ShowShadows = false;
            ssSpread.ActiveSheet.PrintInfo.UseMax = true;
            ssSpread.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssSpread.ActiveSheet.PrintInfo.UseSmartPrint = false;
            ssSpread.ActiveSheet.PrintInfo.ShowPrintDialog = false;
            ssSpread.ActiveSheet.PrintInfo.Preview = false;
            ssSpread.PrintSheet(0);
        }

        private void btnPrintConsult_Click(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
                        
            string strFont = "";
            string strFoot = "";

            string strROWID = ssConsult.ActiveSheet.Cells[ssConsult.ActiveSheet.ActiveRowIndex, 12].Text.Trim();
            if (strROWID == "") return;

            Cursor.Current = Cursors.WaitCursor;
            
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(SDATE, 'YYYY-MM-DD HH24:MI') AS SDATE ";
                SQL = SQL + ComNum.VBLF + "      , TO_CHAR(EDATE,'YYYY-MM-DD HH24:MI') AS EDATE ";
                SQL = SQL + ComNum.VBLF + "      , A.Ptno, A.GbConfirm, TO_CHAR(A.InpDate, 'YYYY-MM-DD') InpDate, A.TODEPTCODE,  A.FRDeptCode        ";
                SQL = SQL + ComNum.VBLF + "      , TO_CHAR(A.BDate, 'YYYY-MM-DD') BDate, nvl(A.ToDrCode, '000000') ToDrCode                         ";
                SQL = SQL + ComNum.VBLF + "      , A.FrDeptCode Dept, C.DrName, B.RoomCode, B.SName, B.Age, B.Sex, B.GBPT, B.IPDNO,a.GbSTS          ";
                SQL = SQL + ComNum.VBLF + "      , TO_CHAR(B.InDate, 'YYYY-MM-DD') InDate, A.FrRemark, A.TOREMARK,  A.FrDrCode FrDrCode, A.INPID    ";
                SQL = SQL + ComNum.VBLF + "      , A.BInpID,  a.PICXY, a.Gubun,a.Bohum,TO_CHAR(a.KDATE1, 'YYYY-MM-DD') KDate1                       ";
                SQL = SQL + ComNum.VBLF + "      , TO_CHAR(a.KDATE2, 'YYYY-MM-DD') KDate2,TO_CHAR(a.KDATE3, 'YYYY-MM-DD') KDate3                    ";
                SQL = SQL + ComNum.VBLF + "      , TO_CHAR(a.KDATE4, 'YYYY-MM-DD') KDate4                                                           ";
                SQL = SQL + ComNum.VBLF + "      , B.GbSpc, B.Bi, B.DrCode, B.AmSet1, B.WardCode                                                    ";
                SQL = SQL + ComNum.VBLF + "      , TO_CHAR(B.INDATE, 'YYYY-MM-DD') EntDate, A.GbPrint, A.ROWID                                      ";
                SQL = SQL + ComNum.VBLF + "      , ADMIN.FC_BAS_DOCTOR_DRNAME(TRIM(A.FRDRCODE)) FRDRNAME                                       ";
                SQL = SQL + ComNum.VBLF + "      , ADMIN.FC_BAS_DOCTOR_DRNAME(TRIM(A.TODRCODE)) TODRNAME                                       ";
                SQL = SQL + ComNum.VBLF + "      , ADMIN.FC_BAS_USER_USERNAME(A.BINPID) BINPNAME                                               ";
                SQL = SQL + ComNum.VBLF + "      , ADMIN.FC_BAS_USER_USERNAME(A.INPID) INPNAME                                                 ";
                SQL = SQL + ComNum.VBLF + "      , to_char(D.BIRTH, 'yyyy-mm-dd') BIRTH                                                             ";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.OCS_ITRANSFER A                                                                       ";
                SQL = SQL + ComNum.VBLF + "      , ADMIN.IPD_NEW_MASTER B                                                                     ";
                SQL = SQL + ComNum.VBLF + "      , ADMIN.BAS_DOCTOR C                                                                         ";
                SQL = SQL + ComNum.VBLF + "      , ADMIN.BAS_PATIENT D                                                                        ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.ROWID = '"+ strROWID + "'                                                                      ";
                SQL = SQL + ComNum.VBLF + "    AND A.Ptno = B.Pano                                                                                  ";
                SQL = SQL + ComNum.VBLF + "    AND A.IPDNO = B.IPDNO                                                                                ";
                SQL = SQL + ComNum.VBLF + "    AND A.Ptno = D.Pano                                                                                  ";
                SQL = SQL + ComNum.VBLF + "    AND B.GBSTS IN ('0', '1', '2', '3', '4', '5', '6', '7')                                              ";       
                SQL = SQL + ComNum.VBLF + "    AND A.FrDrCode = C.DrCode(+)                                                                         ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY B.RoomCode, A.FrDeptCode                                                                      ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    string[] arrTOREMARK = dt.Rows[0]["TOREMARK"].ToString().Trim().Split('\n');

                    if (arrTOREMARK.Length < 16)
                    {
                        ssConsultPrint_Sheet1.Cells[1, 0].Text = "Attending Physician : " + dt.Rows[0]["FRDRNAME"].ToString().Trim();
                        ssConsultPrint_Sheet1.Cells[2, 0].Text = "Consultant : " + dt.Rows[0]["TODRNAME"].ToString().Trim();
                        ssConsultPrint_Sheet1.Cells[3, 0].Text = "Patient    : " + dt.Rows[0]["SNAME"].ToString().Trim()
                                                    + VB.Space(3) + "( " + dt.Rows[0]["SEX"].ToString() + "/"
                                                    + dt.Rows[0]["AGE"].ToString() + " )  등록번호 : " + dt.Rows[0]["PTNO"].ToString().Trim() + "   "
                                                    + dt.Rows[0]["ROOMCODE"].ToString() + "호실";

                        ssConsultPrint_Sheet1.Cells[5, 0].Text = dt.Rows[0]["FRREMARK"].ToString().Trim();
                        ssConsultPrint_Sheet1.Cells[7, 3].Text = dt.Rows[0]["SDATE"].ToString().Trim() + VB.Space(3) + "/" + VB.Space(3)
                                                    + dt.Rows[0]["FRDRNAME"].ToString().Trim()
                                                    + "[" + dt.Rows[0]["FRDEPTCODE"].ToString().Trim() + "]";

                        ssConsultPrint_Sheet1.Cells[10, 0].Text = dt.Rows[0]["TOREMARK"].ToString().Trim();
                        ssConsultPrint_Sheet1.Cells[12, 3].Text = dt.Rows[0]["EDATE"].ToString().Trim() + VB.Space(3) + "/" + VB.Space(3)
                                                    + dt.Rows[0]["TODRNAME"].ToString().Trim()
                                                    + "[" + dt.Rows[0]["TODEPTCODE"].ToString().Trim() + "]";
                        ssConsultPrint_Sheet1.Cells[15, 3].Text = " ";

                        strFont = "/fn\"맑은 고딕\" /fz\"10\" /fb1 /fi0 /fu0 /fk0 /fs2";
                        strFoot = "/c/f2" + "포항성모병원" + "/f2/n";

                        ssConsultPrint_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                        ssConsultPrint_Sheet1.PrintInfo.Footer = strFont + strFoot;
                        ssConsultPrint_Sheet1.PrintInfo.Margin.Top = 20;
                        ssConsultPrint_Sheet1.PrintInfo.Margin.Bottom = 20;
                        ssConsultPrint_Sheet1.PrintInfo.Margin.Footer = 10;
                        ssConsultPrint_Sheet1.PrintInfo.ShowColor = false;
                        ssConsultPrint_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                        ssConsultPrint_Sheet1.PrintInfo.ShowBorder = false;
                        ssConsultPrint_Sheet1.PrintInfo.ShowGrid = false;
                        ssConsultPrint_Sheet1.PrintInfo.ShowShadows = false;
                        ssConsultPrint_Sheet1.PrintInfo.UseMax = true;
                        ssConsultPrint_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                        ssConsultPrint_Sheet1.PrintInfo.UseSmartPrint = false;
                        ssConsultPrint_Sheet1.PrintInfo.ShowPrintDialog = false;
                        ssConsultPrint_Sheet1.PrintInfo.Preview = false;
                        ssConsultPrint.PrintSheet(0);
                    }
                    else
                    {
                        string strTemp = "";

                        ssConsultPrint2_Sheet1.Cells[1, 0].Text = "Attending Physician : " + dt.Rows[0]["FRDRNAME"].ToString().Trim();
                        ssConsultPrint2_Sheet1.Cells[2, 0].Text = "Consultant : " + dt.Rows[0]["TODRNAME"].ToString().Trim();
                        ssConsultPrint2_Sheet1.Cells[3, 0].Text = "Patient    : " + dt.Rows[0]["SNAME"].ToString().Trim()
                                                    + VB.Space(3) + "( " + dt.Rows[0]["SEX"].ToString() + "/"
                                                    + dt.Rows[0]["AGE"].ToString() + " )  등록번호 : " + dt.Rows[0]["PTNO"].ToString().Trim() + "   "
                                                    + dt.Rows[0]["ROOMCODE"].ToString() + "호실";

                        ssConsultPrint2_Sheet1.Cells[5, 0].Text = dt.Rows[0]["FRREMARK"].ToString().Trim();
                        ssConsultPrint2_Sheet1.Cells[7, 3].Text = dt.Rows[0]["SDATE"].ToString().Trim() + VB.Space(3) + "/" + VB.Space(3)
                                                    + dt.Rows[0]["FRDRNAME"].ToString().Trim()
                                                    + "[" + dt.Rows[0]["FRDEPTCODE"].ToString().Trim() + "]";
                        strTemp = "";
                        for (int i = 0; i < 15; i++)
                        {
                            strTemp = strTemp + arrTOREMARK[i].Replace("\r", "\r\n");
                        }

                        ssConsultPrint2_Sheet1.Cells[10, 0].Text = strTemp;
                        //ssConsultPrint2_Sheet1.Cells[12, 3].Text = dt.Rows[0]["EDATE"].ToString().Trim() + VB.Space(3) + "/" + VB.Space(3)
                        //                            + dt.Rows[0]["TODRNAME"].ToString().Trim()
                        //                            + "[" + dt.Rows[0]["TODEPTCODE"].ToString().Trim() + "]";
                        //ssConsultPrint2_Sheet1.Cells[15, 3].Text = " ";

                        strFont = "/fn\"맑은 고딕\" /fz\"10\" /fb1 /fi0 /fu0 /fk0 /fs2";
                        strFoot = "/c/f2" + "포항성모병원" + "/f2/n";

                        ssConsultPrint2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                        ssConsultPrint2_Sheet1.PrintInfo.Footer = strFont + strFoot;
                        ssConsultPrint2_Sheet1.PrintInfo.Margin.Top = 20;
                        ssConsultPrint2_Sheet1.PrintInfo.Margin.Bottom = 20;
                        ssConsultPrint2_Sheet1.PrintInfo.Margin.Footer = 10;
                        ssConsultPrint2_Sheet1.PrintInfo.ShowColor = false;
                        ssConsultPrint2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                        ssConsultPrint2_Sheet1.PrintInfo.ShowBorder = false;
                        ssConsultPrint2_Sheet1.PrintInfo.ShowGrid = false;
                        ssConsultPrint2_Sheet1.PrintInfo.ShowShadows = false;
                        ssConsultPrint2_Sheet1.PrintInfo.UseMax = true;
                        ssConsultPrint2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                        ssConsultPrint2_Sheet1.PrintInfo.UseSmartPrint = false;
                        ssConsultPrint2_Sheet1.PrintInfo.ShowPrintDialog = false;
                        ssConsultPrint2_Sheet1.PrintInfo.Preview = false;
                        ssConsultPrint2.PrintSheet(0);

                        Application.DoEvents();

                        ssConsultPrint3_Sheet1.Cells[1, 0].Text = "Attending Physician : " + dt.Rows[0]["FRDRNAME"].ToString().Trim();
                        ssConsultPrint3_Sheet1.Cells[2, 0].Text = "Consultant : " + dt.Rows[0]["TODRNAME"].ToString().Trim();
                        ssConsultPrint3_Sheet1.Cells[3, 0].Text = "Patient    : " + dt.Rows[0]["SNAME"].ToString().Trim()
                                                    + VB.Space(3) + "( " + dt.Rows[0]["SEX"].ToString() + "/"
                                                    + dt.Rows[0]["AGE"].ToString() + " )  등록번호 : " + dt.Rows[0]["PTNO"].ToString().Trim() + "   "
                                                    + dt.Rows[0]["ROOMCODE"].ToString() + "호실";

                        strTemp = "";
                        for (int i = 15; i < arrTOREMARK.Length; i++)
                        {
                            strTemp = strTemp + arrTOREMARK[i].Replace("\r", "\r\n");
                        }

                        ssConsultPrint3_Sheet1.Cells[5, 0].Text = strTemp;
                        ssConsultPrint3_Sheet1.Cells[12, 3].Text = dt.Rows[0]["EDATE"].ToString().Trim() + VB.Space(3) + "/" + VB.Space(3)
                                                    + dt.Rows[0]["TODRNAME"].ToString().Trim()
                                                    + "[" + dt.Rows[0]["TODEPTCODE"].ToString().Trim() + "]";
                        ssConsultPrint3_Sheet1.Cells[15, 3].Text = " ";

                        strFont = "/fn\"맑은 고딕\" /fz\"10\" /fb1 /fi0 /fu0 /fk0 /fs2";
                        strFoot = "/c/f2" + "포항성모병원" + "/f2/n";

                        ssConsultPrint3_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                        ssConsultPrint3_Sheet1.PrintInfo.Footer = strFont + strFoot;
                        ssConsultPrint3_Sheet1.PrintInfo.Margin.Top = 20;
                        ssConsultPrint3_Sheet1.PrintInfo.Margin.Bottom = 20;
                        ssConsultPrint3_Sheet1.PrintInfo.Margin.Footer = 10;
                        ssConsultPrint3_Sheet1.PrintInfo.ShowColor = false;
                        ssConsultPrint3_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                        ssConsultPrint3_Sheet1.PrintInfo.ShowBorder = false;
                        ssConsultPrint3_Sheet1.PrintInfo.ShowGrid = false;
                        ssConsultPrint3_Sheet1.PrintInfo.ShowShadows = false;
                        ssConsultPrint3_Sheet1.PrintInfo.UseMax = true;
                        ssConsultPrint3_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                        ssConsultPrint3_Sheet1.PrintInfo.UseSmartPrint = false;
                        ssConsultPrint3_Sheet1.PrintInfo.ShowPrintDialog = false;
                        ssConsultPrint3_Sheet1.PrintInfo.Preview = false;
                        ssConsultPrint3.PrintSheet(0);
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
    }

}