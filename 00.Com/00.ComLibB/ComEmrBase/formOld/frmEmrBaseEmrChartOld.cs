using System;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ComBase;
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    public partial class frmEmrBaseEmrChartOld : Form
    {
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;
        private const int TOPMOST_FLAGS = SWP_NOZORDER | SWP_NOSIZE;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        public delegate void SaveOrDelete();
        public event SaveOrDelete rSaveOrDelete;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        private usTimeSet usTimeSetEvent;
        private ComboBox mMaskBox = null;

        private Form mCallForm = null;
        EmrPatient AcpEmr = null;
        private string mOld = "";
        private string mChartDate = "";
        private string mChartTime = "";
        private string mFormNo = "";
        private string mFormName = "";
        private string mEmrNo = "";

        //string EmrUrlMain = "http://192.168.100.33:8090/Emr/MtsEmrSite.mts";
        string gEmrUrl = "http://192.168.100.33:8090/Emr";
        //string EmrUrlImage = "http://192.168.100.33:8090/Emr/progressImageEditor.mts?formNo=1232";
        string EmrUrlPatSend = "http://192.168.100.33:8090/Emr/emrxmlInfo.mts?";
        string EmrUrlUpt = "http://192.168.100.33:8090/Emr/modifyEmrxmlForm.mts?emrNo="; //기록지 수정
        string EmrUrlHis = "http://192.168.100.33:8090/Emr/emrView.mts?emrNo="; //기록지 조회
        string EmrUrlNewPrint = "http://192.168.100.33:8090/Emr/chartFormPrint.mts?formNo="; //빈서식지 출력
        string EmrUrlOldPrint = "http://192.168.100.33:8090/Emr/emrxmlPrint.mts?emrNo=";

        //모니터 사이즈, 폼 위치
        //private int mintTop = 0;
        //private int mintLeft = 0;
        private int mintMonitor = 0;
        private int[] mintWidth = null;
        private int[] mintHeight = null;

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
                        this.Show();
                        this.WindowState = FormWindowState.Maximized;
                        break;
                    }
                }
            }
        }

        public frmEmrBaseEmrChartOld()
        {
            InitializeComponent();
        }

        public frmEmrBaseEmrChartOld(Form pCallForm, string pOld, string pChartDate, string pChartTime, string pFormNo, string pFormName, string pEmrNo = "")
        {
            InitializeComponent();
            mCallForm = pCallForm;
            mOld = pOld;
            mChartDate = pChartDate;
            mChartTime = pChartTime;
            mFormNo = pFormNo;
            mFormName = pFormName;
            mEmrNo = pEmrNo;
        }

        public frmEmrBaseEmrChartOld(Form pCallForm, EmrPatient pAcpEmr, string pOld, string pChartDate, string pChartTime, string pFormNo, string pFormName, string pEmrNo = "")
        {
            InitializeComponent();
            mCallForm = pCallForm;
            AcpEmr = pAcpEmr;
            mOld = pOld;
            mChartDate = pChartDate;
            mChartTime = pChartTime;
            mFormNo = pFormNo;
            mFormName = pFormName;
            mEmrNo = pEmrNo;
        }

        private void frmEmrBaseEmrChartOld_Load(object sender, EventArgs e)
        {
            GetMonitorInfo();

            //VB에서 사용하는 탑모스트 구현
            SetWindowPos(this.Handle, HWND_TOPMOST, 200, 0, 0, 0, TOPMOST_FLAGS);

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
            txtMedFrTime.Text = ComFunc.FormatStrToDate(VB.Right(strCurDateTime, 6), "M");

            if (mChartDate != "")
            {
                dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(mChartDate, "D"));
            }
            if (mChartTime != "")
            {
                txtMedFrTime.Text = ComFunc.FormatStrToDate(mChartTime, "M");
            }

            EmrForm pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, mFormNo);
            if (pForm.FmFORMNO == 1232 || pForm.FmFORMNO ==  2148 || pForm.FmFORMNO == 2492)
            {
                mbtnSave.Visible = true;
                mbtnDelete.Visible = true;
                mbtnPrint.Visible = pForm.FmFORMNO == 2492 || clsType.User.AuAPRINTIN.Equals("1");
            }

            SetChartInfo();

            if (AcpEmr != null)
            {
                LoadWebForm();
            }
            else
            {
                if (VB.Val(mEmrNo) > 0)
                {
                    LoadWebForm();
                }
            }
        }

        /// <summary>
        /// EMRNO로 CHARTDATE, CHARTTIME, WRITEDATE, WRITETIME등을 가져온다.
        /// </summary>
        /// <param name="patient"></param>
        void SetChartInfo()
        {
            
            if (VB.Val(mEmrNo) == 0)
                return;

            string SQL = string.Empty;
            string sqlErr = string.Empty;
            OracleDataReader reader = null;

            try
            {
                SQL = "SELECT CHARTDATE, CHARTTIME, WRITEDATE, WRITETIME";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXMLMST";
                SQL += ComNum.VBLF + "WHERE EMRNO = " + mEmrNo;

                sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxQ("차트리스트를 가져오는 도중 오류가 발생했습니다.");
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    dtMedFrDate.Value = DateTime.ParseExact(reader.GetValue(0).ToString().Trim(), "yyyyMMdd", null);
                    txtMedFrTime.Text = reader.GetValue(1).ToString().Trim();
                    txtMedFrTime.Text = VB.Left(txtMedFrTime.Text, 2) + ":" +  VB.Mid(txtMedFrTime.Text, 3, 2);



                    //의료정보팀 실제 작성시간 보이게.
                    if (clsType.User.BuseCode.Equals("044201"))
                    {
                        lblServerDate.Visible = true;
                        lblServerDate.Text = string.Format("( {0} {1} )", reader.GetValue(2).ToString().Trim(),
                            VB.Val(reader.GetValue(3).ToString().Trim()).ToString("00:00:00"));
                    }

                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxQ(ex.Message);
            }


        }

        private void mbtnExit_Click(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {            
                if (rEventClosed == null)
                {
                    Close();
                }
                else
                {
                    rEventClosed();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void mbtnTime_Click(object sender, EventArgs e)
        {
            SetTimeCheckShow();
        }

        private void SetTimeCheckShow()
        {
            mMaskBox = txtMedFrTime;
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            usTimeSetEvent = new usTimeSet();
            usTimeSetEvent.rSetTime += new usTimeSet.SetTime(usTimeSetEvent_SetTime);
            usTimeSetEvent.rEventClosed += new usTimeSet.EventClosed(usTimeSetEvent_EventClosed);
            this.Controls.Add(usTimeSetEvent);
            usTimeSetEvent.Top = txtMedFrTime.Top - 5;
            usTimeSetEvent.Left = txtMedFrTime.Left;
            usTimeSetEvent.BringToFront();
        }

        private void usTimeSetEvent_SetTime(string strText)
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            txtMedFrTime.Text = strText;
        }

        private void usTimeSetEvent_EventClosed()
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
        }

        private void mbtnSave_Click(object sender, EventArgs e)
        {
            if (SaveDataWeb() == false)
            {
                return;
            }
            for (int intWeb = 0; intWeb < 400000; intWeb++)
            {
                Application.DoEvents();
                Application.DoEvents();
            }
            if (mOld == "OLD")
            {
                SaveData(VB.Val(mEmrNo));
            }

            //if (mCallForm != null)
            //{
            //    rSaveOrDelete();
            //}
            //else
            //{
            //    this.Close();
            //}
        }

        private bool SaveDataWeb()
        {
            try
            {
                string strChartTime = txtMedFrTime.Text.Replace(":", "");
                if (strChartTime.Length < 6)
                {
                    strChartTime = strChartTime + "00";
                }
                webEMR.Document.GetElementById("chartDate").SetAttribute("value", dtMedFrDate.Value.ToString("yyyyMMdd"));
                webEMR.Document.GetElementById("chartTime").SetAttribute("value", strChartTime);

                string strURL = "javascript:doSave()";
                webEMR.Navigate(strURL);
                //while (webEMR.IsBusy == true)
                //{
                //    Application.DoEvents();
                //    Application.DoEvents();
                //}
                //for (int intWeb = 0; intWeb < 400000; intWeb++)
                //{
                //    Application.DoEvents();
                //    Application.DoEvents();
                //}

                while (webEMR.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool SaveData(double pEmrNo)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strCurDate = VB.Left(strCurDateTime, 8);
                string strCurTime = VB.Right(strCurDateTime, 6);

                SQL = " UPDATE ADMIN.EMRXMLMST ";
                SQL = SQL + ComNum.VBLF + " SET USEID = " + clsType.User.IdNumber;
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + pEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = " UPDATE ADMIN.EMRXML ";
                SQL = SQL + ComNum.VBLF + " SET USEID = " + clsType.User.IdNumber;
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + pEmrNo;
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
                //ComFunc.MsgBoxEx(this, "저장하였습니다.");
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

        private void mbtnDelete_Click(object sender, EventArgs e)
        {
            if (VB.Val(mEmrNo) == 0)
            {
                return;
            }

            if (ComFunc.MsgBoxQ("기존내용을 삭제하시겠습니까?", "PSMH", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            double dblEmrNo = VB.Val(mEmrNo);

            if (DeleteDate(dblEmrNo) == true)
            {
                if (mCallForm != null)
                {
                    rSaveOrDelete();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private bool DeleteDate(double dblEmrNo)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strChartUseId = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "                A.EMRNO, A.USEID";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRXML A";
                SQL = SQL + ComNum.VBLF + "    WHERE A.EMRNO = " + dblEmrNo;

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
                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                strChartUseId = dt.Rows[0]["USEID"].ToString().Trim();
                dt.Dispose();
                dt = null;

                if (clsType.User.IdNumber != strChartUseId)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "기존 작성자와 다릅니다." + ComNum.VBLF + "삭제할 수 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                strCurDateTime = strCurDateTime.Replace("-", "").Replace(":", "");

                double dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, "ADMIN.EMRXMLHISNO");

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO ADMIN.EMRXMLHISTORY";
                SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "', '" + clsType.User.IdNumber + "',CERTNO";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRXMLMST";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
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
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
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

        private void LoadWebForm()
        {
            WebLogin();
            if (VB.Val(mEmrNo) != 0)
            {
                if(clsEmrQuery.CanModify(clsDB.DbCon, mEmrNo) != "NO")
                {
                    //(작성권한이 있으면 수정)
                    webEMR.Navigate(EmrUrlUpt + mEmrNo);
                }
                else
                {
                    //(작성권한이 없으면 조회)
                    webEMR.Navigate(EmrUrlHis + mEmrNo);
                }

                //while (webEMR.IsBusy == true)
                //{
                //    Application.DoEvents();
                //}
                //for (int intWeb = 0; intWeb < 40000; intWeb++)
                //{
                //    Application.DoEvents();
                //}

                while (webEMR.IsDisposed == false &&  webEMR.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                }
            }
            else
            {
                if (webEMR.IsDisposed == true)
                {
                    return; 
                }

                string strURL = EmrUrlPatSend +
                   "ptNo=" + AcpEmr.ptNo +
                   "&acpNo=" + AcpEmr.acpNo +
                   "&inOutCls=" + AcpEmr.inOutCls +
                   "&medFrDate=" + AcpEmr.medFrDate +
                   "&medFrTime=" + AcpEmr.medFrTime +
                   "&medEndDate=" + AcpEmr.medEndDate +
                   "&medEndTime=" + AcpEmr.medEndTime +
                   "&medDeptCd=" + AcpEmr.medDeptCd +
                   "&medDeptName=" + "" +
                   "&medDrCd=" + AcpEmr.medDrCd +
                   "&gubun=" + "1" +
                   "&formNo=" + mFormNo;

                //strURL = "http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=02487371&acpNo=0&inOutCls=O&medFrDate=20180226&medFrTime=141800&medEndDate=&medEndTime=&medDeptCd=GS&medDeptName=&medDrCd=2121&gubun=3&formNo=";
                webEMR.Navigate(strURL); //한장씩 볼 경우
                //while (webEMR.IsBusy == true)
                //{
                //    Application.DoEvents();
                //}
                //for (int intWeb = 0; intWeb < 40000; intWeb++)
                //{
                //    Application.DoEvents();
                //}

                while (webEMR.IsDisposed == false && webEMR.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                }
            }
        }

        private void WebLogin()
        {
            string strURL = "";
            string strUseId = clsType.User.IdNumber;
            string strPw = clsType.User.Passhash256;

            //webEMR.Navigate("http://192.168.100.33:8090/Emr/doLogin.mts?useId=15200&password=5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028&loginType=vb");

            //while (webEMR.ReadyState != WebBrowserReadyState.Complete)
            //{
            //    Application.DoEvents();
            //}

            strURL = gEmrUrl + "/doLogin.mts?useId=" + strUseId + "&password=" + strPw + "&loginType=vb";
            webEMR.Navigate(strURL);
            //while (webEMR.IsBusy == true)
            //{
            //    Application.DoEvents();
            //}
            //for (int intWeb = 0; intWeb < 40000; intWeb++)
            //{
            //    Application.DoEvents();
            //}
            while (webEMR.IsDisposed == false && webEMR.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }

        }

        private void mbtnPrint_Click(object sender, EventArgs e)
        {
            if(VB.Val(mEmrNo) > 0)
            {
                webEMR.Navigate(EmrUrlOldPrint + mEmrNo);
            }
            else
            {
                webEMR.Navigate(EmrUrlNewPrint + mFormNo);
            }

            //while (webEMR.IsBusy == true)
            //{
            //    Application.DoEvents();
            //}
            //for (int intWeb = 0; intWeb < 40000; intWeb++)
            //{
            //    Application.DoEvents();
            //}
            while (webEMR.IsDisposed == false && webEMR.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }

            if(VB.Val(mEmrNo) == 0 && clsType.User.IsNurse.Equals("OK"))
            {
                SetEMROCRPRTHIS();
            }
        }

        void SetEMROCRPRTHIS()
        {
            string SQL = string.Empty;
            int RowAffected = 0;

            if (AcpEmr.inOutCls.Equals("I"))
            {
                SQL = " SELECT A.WARDCODE, B.SNAME";
                SQL += ComNum.VBLF + " FROM ADMIN.IPD_NEW_MASTER A,";
                SQL += ComNum.VBLF + "  ADMIN.BAS_PATIENT B";
                SQL += ComNum.VBLF + "  WHERE A.PANO = B.PANO";
                SQL += ComNum.VBLF + "  AND TO_CHAR(A.INDATE,'YYYYMMDD') = '" + AcpEmr.medFrDate + "'";
                SQL += ComNum.VBLF + "  AND A.DEPTCODE = '" + AcpEmr.medDeptCd + "'";
                SQL += ComNum.VBLF + "  AND A.PANO = '" + AcpEmr.ptNo + "'";
            }
            else
            {
                SQL = " SELECT '' AS WARDCODE, B.SNAME";
                SQL += ComNum.VBLF + " FROM ADMIN.OPD_MASTER A,";
                SQL += ComNum.VBLF + "  ADMIN.BAS_PATIENT B";
                SQL += ComNum.VBLF + "  WHERE A.PANO = B.PANO";
                SQL += ComNum.VBLF + "  AND TO_CHAR(A.BDATE,'YYYYMMDD') = '" + AcpEmr.medFrDate + "'";
                SQL += ComNum.VBLF + "  AND A.DEPTCODE = '" + AcpEmr.medDeptCd + "'";
                SQL += ComNum.VBLF + "  AND A.PANO = '" + AcpEmr.ptNo + "'";
            }

            OracleDataReader reader = null;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                string strWARDCODE = string.Empty;
                string strName = string.Empty;

                if (reader.HasRows && reader.Read())
                {
                    strWARDCODE = reader.GetValue(0).ToString().Trim();
                    strName = reader.GetValue(1).ToString().Trim();
                }

                reader.Dispose();


                SQL = " INSERT INTO ADMIN.EMROCRPRTHIS";
                SQL += ComNum.VBLF + " (OCRDATE,OCRTIME,PTNO,PTNAME,INOUTCLS,";
                SQL += ComNum.VBLF + "  MEDFRDATE,MEDDEPTCD,WARDCODE,";
                SQL += ComNum.VBLF + "  FORMNO,USEID,DEPTCD,DEPTCD1)";
                SQL += ComNum.VBLF + "  VALUES (";
                SQL += ComNum.VBLF + "  TO_CHAR(SYSDATE, 'YYYYMMDD'),";
                SQL += ComNum.VBLF + "  TO_CHAR(SYSDATE, 'HH24MISS'),";
                SQL += ComNum.VBLF + "  '" + AcpEmr.ptNo + "',";
                SQL += ComNum.VBLF + "  '" + strName + "',";
                SQL += ComNum.VBLF + "  '" + AcpEmr.inOutCls + "',";
                SQL += ComNum.VBLF + "  '" + AcpEmr.medFrDate + "',";
                SQL += ComNum.VBLF + "  '" + AcpEmr.medDeptCd + "',";
                SQL += ComNum.VBLF + "  '" + strWARDCODE + "',";
                SQL += ComNum.VBLF + "  '" + mFormNo + "',";
                SQL += ComNum.VBLF + "  '" + clsType.User.IdNumber + "',";
                SQL += ComNum.VBLF + "  '',";
                SQL += ComNum.VBLF + "  '" + clsType.User.BuseCode + "')";

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
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void frmEmrBaseEmrChartOld_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                if (rEventClosed != null)
                {
                    rEventClosed();
                }
            }
            else
            {
                this.Close();
            }
        }
    }
}
