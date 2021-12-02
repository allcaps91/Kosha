using System;
using System.Data;
using System.Windows.Forms;
using ComBase;
using ComLibB;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : SupLbEx
    /// File Name       : frmSupLbExVIEW10.cs
    /// Description     : 검사결과 조회
    /// Author          : 최익준
    /// Create Date     : 2018-01-22
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\exam\exmain\ExMain08.frm(FrmViewExam.frm) >> frmSupLbExVIEW10.cs 폼이름 재정의" />	
    public partial class frmComSupLbExVIEW10 : Form, MainFormMessage
    {
        private frmSearchPano frmSearchPanoEvent = null; 

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

        private string GstrPano = "";
        private string GstrWS = "";
        private int GintAge = 0;
        private string GstrSex = "";

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
                //this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                foreach (Screen screen in screens)
                {
                    if (screen.Primary == false)
                    {
                        secondary_screen = screen;
                        //this.Bounds = secondary_screen.Bounds;
                        this.Left = secondary_screen.Bounds.X + 1;
                        this.Top = 0;
                        this.Show();
                        //this.WindowState = FormWindowState.Maximized;
                        break;
                    }
                }
            }
        }
        #endregion

        #region MainFormMessage InterFace

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

        #endregion


        public frmComSupLbExVIEW10()
        {
            InitializeComponent();
            setEvent();
        } 

        public frmComSupLbExVIEW10(string strPano, string strWS)  
        {
            InitializeComponent();
            GstrPano = strPano; 
            GstrWS = strWS;
            setEvent();
        }

        public frmComSupLbExVIEW10(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;         
            setEvent();
        }

        #region //폼을 모달리스로 띄울경우 처리함
        public frmComSupLbExVIEW10(string strHelpCode, Form pModalCallForm, int pModalMonitor)
        {
            InitializeComponent();

            GstrPano = strHelpCode;
            mModalCallForm = pModalCallForm;
            mModalMonitor = pModalMonitor;

            setEvent();
        }
        #endregion

        void setEvent()
        {
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
        }

        void setCombo()
        {
            cboWs.Items.Clear();
            cboWs.Items.Add("*.전체항목");
            cboWs.Items.Add("C.생화학");
            cboWs.Items.Add("F.체액검사");
            cboWs.Items.Add("H.혈액학");
            cboWs.Items.Add("S.혈청학");
            cboWs.Items.Add("E.면역학");
            cboWs.Items.Add("B.혈액은행");
            cboWs.Items.Add("U.소변검사");
            cboWs.Items.Add("P.분변검사");
            cboWs.Items.Add("M.미생물");
            cboWs.Items.Add("W.외부의뢰");
            cboWs.SelectedIndex = 0;
        }

        void setComboIndex()
        {
            if (GstrWS.Length == 1)
            {
                foreach (string s in cboWs.Items)
                {
                    if (VB.Left(s, 1) == GstrWS)
                    {
                        cboWs.SelectedItem = s;
                        break;
                    }
                }
            }

            else if (GstrWS.Length > 1)
            {
                cboWs.SelectedIndex = 0;
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                #region //폼을 모달리스로 띄울경우 처리함
                if (mModalCallForm != null)
                {
                    rEventClosed();
                }
                #endregion
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }
        
        private void frmComSupLbExVIEW10_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            //{
            //    this.Close();
            //    return;
            //}

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            if (mModalCallForm != null)
            {
                #region //폼을 모달리스로 띄울경우 처리함
                GetMonitorInfo();
                if (mModalMonitor == 2)
                {
                    viewFormMonitor2();
                }
                else
                {
                    //this.StartPosition = FormStartPosition.CenterParent;
                    this.Top = 150;
                    this.Left = 500;
                }
                #endregion
            }
            else
            {
                //this.WindowState = FormWindowState.Maximized;
            }           

            setCombo();

            if (GstrPano != "")
            {
                txtPano.Text = GstrPano.Trim();
            }

            if (GstrWS != "")
            {
                setComboIndex();
            }

            lblSname.Text = "";
            rdoSel0.Checked = true;
            ssView1.Visible = true;
            ssView1.Dock = DockStyle.Fill;
            ssView2.Visible = false;
            ssView2.Dock = DockStyle.None;
            ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-31);
            dtpDate2.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            double dblITEM51 = GetITEM51();

            // 2018.07.20 류마티스내과(오동호 과장 요청 : 환경설정 제외 6개월로 고정)
            if (clsPublic.GstrDeptCode.Trim() == "MR")
            {
                dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-180);
            }
            else
            {
                if (dblITEM51 > 0)
                {
                    dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(dblITEM51 * -1);
                }
            }

            if (txtPano.Text != "")
            {
                GetPANO();
                GetData();
            }

            else
            {
                txtPano.Select();
            }

            ssView1.ActiveSheet.Columns[0, ssView1.ActiveSheet.Columns.Count - 1].Locked = true;
            ssView2.ActiveSheet.Columns[0, ssView1.ActiveSheet.Columns.Count - 1].Locked = true;

        }

        private double GetITEM51()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            double rtnVal = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     ITEM51";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ENVSETTING";
                SQL = SQL + ComNum.VBLF + "     WHERE USERID = '" + clsType.User.Sabun + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = VB.Val(dt.Rows[0]["ITEM51"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = false;
            GetData();
            btnSearch.Enabled = true;
        }

        private void GetData()
        {
            txtPano.Text = txtPano.Text.Trim();

            if (txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호가 공란입니다.");
                txtPano.Focus();
                return;
            }

            if (VB.IsNumeric(txtPano.Text) == false)
            {
                ComFunc.MsgBox("등록번호의 (-)는 입력하지 마십시오.");
                txtPano.Focus();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            //GetPANO();
            SCREEN_CLEAR();

            btnExit.Enabled = true;
            ssView1.Enabled = true;
            ssView2.Enabled = true;

            if (rdoSel0.Checked == true)
            {
                Result_View_Single();
            }
            else
            {
                Result_View_Nu();
            }

            Cursor.Current = Cursors.Default;
        }

        private void SCREEN_CLEAR()
        {
            ssView1_Sheet1.RowCount = 0;
            ssView1_Sheet1.RowCount = 21;
            ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            ssView2_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 21;
            ssView2_Sheet1.ColumnCount = 2;
            ssView2_Sheet1.ColumnCount = 50;

            //스프레드 검사명에 필터 적용
            //필터 사용
            ssView1.ActiveSheet.Columns[1].AllowAutoFilter = true;
            ssView1.Sheets[0].AutoFilterMode = FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu;

            for (int i = 2; i < ssView2_Sheet1.Columns.Count; i++)
            {
                ssView2_Sheet1.Columns[i].Width = 90;
            }

            btnSearch.Enabled = true;
            btnExit.Enabled = true;
            ssView1.Enabled = false;
            ssView2.Enabled = false;
        }

        private void Result_View_Single()
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int iRow = 0;
            string sSpecNo = "";
            string sCompare = "";
            string sRef = "";
            string sFDate = "";
            string sTDate = "";
            string sWsCode = "";   //WS약어
            string sResultDate = "";   //결과일자
            string sStatus = "";   //상태
            string sResult = "";   //결과
            string strOK = "";   //Display여부
            string sFootNote = "";   //FootNote
            string sREF_VALUE = "";

            int nREAD = 0;
            int nREAD2 = 0;

            sFDate = dtpDate.Value.ToString("yyyy-MM-dd");
            sTDate = dtpDate2.Value.AddDays(1).ToString("yyyy-MM-dd");
            sWsCode = VB.Left(cboWs.Text, 1);
            ssView1_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "           SELECT S.Sname, S.Age, S.Sex, S.Room, S.DeptCode, S.DrCode,S.SpecNo,S.ResultDate SRDate, ";
                SQL = SQL + ComNum.VBLF + "         TO_CHAR(R.ResultDate,'YYYY-MM-DD') ResultDate,";
                SQL = SQL + ComNum.VBLF + "         TO_CHAR(S.ReceiveDate,'YYYY-MM-DD') ReceiveDate,";
                SQL = SQL + ComNum.VBLF + "         R.Status, R.MasterCode,R.SubCode, R.Result, R.Refer, R.Panic,  ";
                SQL = SQL + ComNum.VBLF + "         R.Delta, R.Unit, R.SeqNo, M.ExamName,B.Drname ";
                SQL = SQL + ComNum.VBLF + "       , KOSMOS_OCS.FC_EXAM_MASTER_SUB_REF('0',R.SUBCODE,S.SEX,S.AGE,R.RESULT) AS REF_VALUE ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "Exam_Specmst S, " + ComNum.DB_MED + "Exam_ResultC R, " + ComNum.DB_MED + "Exam_Master M," + ComNum.DB_PMPA + "Bas_Doctor B ";
                SQL = SQL + ComNum.VBLF + "   WHERE S.Pano = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "     AND S.Status IN ('04','05') ";
                SQL = SQL + ComNum.VBLF + "     AND S.Bi < '60' "; //종합건진 제외
                if (sWsCode != "*")
                { SQL = SQL + ComNum.VBLF + " AND S.WorkSts LIKE '%" + sWsCode + "%' "; }
                SQL = SQL + ComNum.VBLF + "     AND S.WorkSTS NOT IN ('A','T') ";  //세포학,조직학은 제외
                SQL = SQL + ComNum.VBLF + "     AND S.ResultDate >= TO_DATE('" + sFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     AND S.ResultDate <  TO_DATE('" + sTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     AND S.SpecNo = R.SpecNo(+) ";
                SQL = SQL + ComNum.VBLF + "     AND R.SubCode = M.MasterCode(+) ";
                SQL = SQL + ComNum.VBLF + "     AND R.ResultWS NOT IN ('A','T') "; //세포학,조직학은 제외
                if (sWsCode != "*")
                { SQL = SQL + ComNum.VBLF + " AND R.ResultWs = '" + sWsCode + "' "; }
                SQL = SQL + ComNum.VBLF + "     AND S.Drcode  = B.DrCode(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY S.ReceiveDate DESC,R.SpecNo DESC, R.SeqNo ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                sCompare = "";
                iRow = 0;
                nREAD = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    sResultDate = dt.Rows[i]["ResultDate"].ToString().Trim();
                    sStatus = dt.Rows[i]["Status"].ToString().Trim();
                    sResult = dt.Rows[i]["Result"].ToString().Trim();
                    sSpecNo = dt.Rows[i]["SpecNo"].ToString().Trim();
                    sREF_VALUE = dt.Rows[i]["REF_VALUE"].ToString().Trim();
                    if (sStatus == "H")
                    {
                        strOK = "OK";
                    }

                    else if (sStatus == "V")
                    {
                        strOK = "OK";
                        if (sResult == "")
                        {
                            strOK = "NO";
                        }
                        if (dt.Rows[i]["MasterCode"].ToString().Trim() == dt.Rows[i]["SubCode"].ToString().Trim())
                        {
                            strOK = "OK";

                        }
                    }

                    else
                    {
                        strOK = "OK";
                        sResult = "-< 검사중 >-";
                    }


                    //Foot Note를 READ
                    SQL = "     SELECT FootNote FROM " + ComNum.DB_MED + "Exam_ResultCf ";
                    SQL = SQL + ComNum.VBLF + "WHERE SpecNo = '" + sSpecNo + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SeqNo =  " + dt.Rows[i]["SeqNo"].ToString().Trim() + " ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY SORT";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    sFootNote = "";

                    nREAD2 = dt2.Rows.Count;

                    if (nREAD2 == 1)
                    {
                        strOK = "OK";
                        sFootNote = dt2.Rows[0]["FootNote"].ToString().Trim();
                    }

                    if (strOK == "OK")
                    {
                        iRow += 1;
                        if (iRow > ssView1_Sheet1.RowCount)
                        {
                            ssView1_Sheet1.RowCount = iRow + 20;
                        }
                        sSpecNo = dt.Rows[i]["SpecNo"].ToString().Trim();

                        if (sCompare != sSpecNo)
                        {
                            ssView1_Sheet1.Cells[iRow - 1, 0].Text = sSpecNo;       //검체번호
                        }

                        ssView1_Sheet1.Cells[iRow - 1, 1].Text = dt.Rows[i]["ExamName"].ToString().Trim();      //검사이름
                        ssView1_Sheet1.Cells[iRow - 1, 2].Text = sResult;       //검사결과
                        ssView1_Sheet1.Cells[iRow - 1, 3].Text = dt.Rows[i]["Refer"].ToString().Trim();
                        ssView1_Sheet1.Cells[iRow - 1, 4].Text = dt.Rows[i]["Unit"].ToString().Trim();      //결과단위

                        sRef = Reference(dt.Rows[i]["Subcode"].ToString().Trim(), GintAge.ToString(), GstrSex);

                        ssView1_Sheet1.Cells[iRow - 1, 5].Text = sREF_VALUE;          //참고치
                        ssView1_Sheet1.Cells[iRow - 1, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();      //진료과코드
                        ssView1_Sheet1.Cells[iRow - 1, 7].Text = dt.Rows[i]["DrName"].ToString().Trim();        //의사코드
                        ssView1_Sheet1.Cells[iRow - 1, 8].Text = dt.Rows[i]["ResultDate"].ToString().Trim();        //의사코드

                        if (dt.Rows[i]["DeptCode"].ToString().Trim() == "EM" || dt.Rows[i]["DeptCode"].ToString().Trim() == "ER")
                        {
                            ssView1_Sheet1.Cells[iRow - 1, 7].Text = "응급실";     //~1
                        }

                        if (dt.Rows[i]["REFER"].ToString().Trim() != "")
                        {
                            ssView1_Sheet1.Cells[iRow - 1, 0, iRow - 1, ssView1_Sheet1.ColumnCount - 1].BackColor = System.Drawing.Color.FromArgb(190, 255, 190);
                        }
                    }

                    for (j = 0; j < nREAD2; j++)
                    {
                        iRow += 1;
                        if (iRow > ssView1_Sheet1.RowCount)
                        {
                            ssView1_Sheet1.RowCount = iRow + 20;
                        }
                        //ssView1_Sheet1.Cells[iRow, 1].Text = " " + dt2.Rows[j]["FootNote"].ToString().Trim();
                        ssView1_Sheet1.Cells[iRow - 1, 1].Text = " " + dt2.Rows[j]["FootNote"].ToString().Trim();
                        // ssView1_Sheet1.Cells[iRow - 1, 1].ForeColor = Color.CadetBlue;
                    }

                    //2018.05.14.김홍록: 오류 접수후 수정
                    //for (j = 0; j < nREAD2 -2; j++)
                    //{
                    //    iRow += 1;
                    //    if (iRow > ssView1_Sheet1.RowCount)
                    //    {
                    //        ssView1_Sheet1.RowCount = iRow + 20;
                    //    }
                    //    //ssView1_Sheet1.Cells[iRow, 1].Text = " " + dt2.Rows[j]["FootNote"].ToString().Trim();
                    //    ssView1_Sheet1.Cells[iRow - 1, 1].Text = " " + dt2.Rows[j]["FootNote"].ToString().Trim();
                    //    // ssView1_Sheet1.Cells[iRow - 1, 1].ForeColor = Color.CadetBlue;
                    //}

                    dt2.Dispose();
                    sCompare = sSpecNo;
                }

                dt.Dispose();
                dt = null;

                if (iRow > 21)
                {
                    ssView1_Sheet1.RowCount = iRow;
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
                Cursor.Current = Cursors.Default;
            }
        }

        void Result_View_Nu()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int K = 0;
            int M = 0;

            string strFDate = dtpDate.Value.ToString("yyyy-MM-dd");
            string strTDate = dtpDate2.Value.AddDays(1).ToString("yyyy-MM-dd");
            string strWsCode = VB.Left(cboWs.Text, 1);

            string strSpecNo = "";
            string strCompare = "";
            string strRef = "";
            string strAllName = "";
            string strName = "";
            string strCheck = "";
            string strAllTemp = "";
            string strStatus = "";
            string strOldData = "";
            string strNewData = "";

            int intCol = 1;
            double iTemp = 0;

            ssView2_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     S.Sname, S.Age, S.Sex, S.Room, R.MasterCode, R.SeqNo, ";
                SQL = SQL + ComNum.VBLF + "     R.SubCode, R.ResultWS, M.ExamName ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "Exam_Specmst S, " + ComNum.DB_MED + "Exam_ResultC R, " + ComNum.DB_MED + "Exam_Master M ";
                SQL = SQL + ComNum.VBLF + "     WHERE S.Pano = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "         AND S.ResultDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND S.ResultDate <  TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND S.Status IN ('04','05') ";
                SQL = SQL + ComNum.VBLF + "         AND S.Bi < '60' "; //종합건진 제외

                if (strWsCode != "*")
                {
                    SQL = SQL + ComNum.VBLF + "         AND S.WorkSts LIKE '%" + strWsCode + "%' ";
                }

                SQL = SQL + ComNum.VBLF + "         AND S.WorkSTS NOT IN ('A','T') ";  //세포학,조직학은 제외
                SQL = SQL + ComNum.VBLF + "         AND S.SpecNo = R.SpecNo(+) ";

                if (strWsCode != "*")
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.ResultWS = '" + strWsCode + "' ";
                }

                SQL = SQL + ComNum.VBLF + "         AND R.ResultWS NOT IN ('A','T') "; //세포학,조직학은 제외
                SQL = SQL + ComNum.VBLF + "         AND R.ResultDate IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "         AND R.SubCode = M.MasterCode(+) ";
                SQL = SQL + ComNum.VBLF + "GROUP BY R.SubCode, M.ExamName, R.MasterCode, R.SeqNo, R.ResultWS, ";
                SQL = SQL + ComNum.VBLF + "     S.Sname, S.Age, S.Sex, S.Room ";
                SQL = SQL + ComNum.VBLF + "ORDER BY R.MasterCode, R.SeqNo, R.ResultWS ";

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
                    ssView2_Sheet1.RowCount = dt.Rows.Count;
                    ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    K = 0;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strNewData = VB.Left(dt.Rows[i]["RESULTWS"].ToString().Trim(), 1);          //검사ws
                        strNewData += dt.Rows[i]["EXAMNAME"].ToString().Trim();                     //검사이름

                        strCheck = "";

                        for (M = 0; M <= K; M++)
                        {
                            strOldData = VB.Left(ssView2_Sheet1.Cells[M, 1].Text, 1);
                            strOldData += ssView2_Sheet1.Cells[M, 0].Text;

                            if (strNewData == strOldData) { strCheck = "Y"; }       //같은 검사 항목이라면
                        }

                        if (strCheck == "")
                        {
                            ssView2_Sheet1.Cells[K, 0].Text = VB.Right(strNewData, VB.Len(strNewData) - 1);
                            ssView2_Sheet1.Cells[K, 1].Text = VB.Left(strNewData, 1);

                            strAllName += strNewData + "^" + (K + 1) + ",";
                            strRef = Reference(dt.Rows[i]["SUBCODE"].ToString().Trim(), GintAge.ToString(), GstrSex);
                            ssView2_Sheet1.Cells[K, 1].Text = strRef;
                            K++;
                        }
                    }
                }

                ssView2_Sheet1.RowCount = ssView2_Sheet1.NonEmptyRowCount;

                dt.Dispose();
                dt = null;
                
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     S.ReceiveDate,S.ResultDate,R.ResultWS, R.SpecNo, R.Result,";
                SQL = SQL + ComNum.VBLF + "     R.Status, M.ExamName ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "Exam_Specmst S, " + ComNum.DB_MED + "Exam_ResultC R, " + ComNum.DB_MED + "Exam_Master M ";
                SQL = SQL + ComNum.VBLF + "     WHERE S.Pano = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "         AND S.ResultDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND S.ResultDate <  TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND S.Status IN ('04','05') ";
                SQL = SQL + ComNum.VBLF + "         AND S.Bi < '60' "; //종합건진 제외

                if (strWsCode != "*")
                {
                    SQL = SQL + ComNum.VBLF + "         AND S.WorkSts LIKE '%" + strWsCode + "%' ";
                }

                SQL = SQL + ComNum.VBLF + "         AND S.WorkSTS NOT IN ('A','T') ";  //세포학,조직학은 제외
                SQL = SQL + ComNum.VBLF + "         AND S.SpecNo = R.SpecNo(+) ";

                if (strWsCode != "*")
                {
                    SQL = SQL + ComNum.VBLF + "         AND R.ResultWS = '" + strWsCode + "' ";
                }

                SQL = SQL + ComNum.VBLF + "         AND R.ResultWS NOT IN ('A','T') "; //세포학,조직학은 제외
                SQL = SQL + ComNum.VBLF + "         AND R.ResultDate IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "         AND R.SubCode = M.MasterCode(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY S.ReceiveDate DESC,R.SpecNo DESC,R.MasterCode, R.SeqNo, R.ResultWS ";

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
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSpecNo = dt.Rows[i]["SPECNO"].ToString().Trim();
                        strStatus = dt.Rows[i]["STATUS"].ToString().Trim();

                        if (strCompare != strSpecNo)
                        {
                            intCol++;
                            ssView2_Sheet1.ColumnHeader.Cells[0, intCol].Text = strSpecNo;
                        }

                        strName = VB.Left(dt.Rows[i]["RESULTWS"].ToString().Trim(), 1);
                        strName += dt.Rows[i]["EXAMNAME"].ToString().Trim();
                        strName += "^";

                        strAllTemp = strAllName;

                        iTemp = VB.Val(VB.PP(VB.PP(strAllTemp.Replace(strName, "|"), "|", 2), ",", 1)) - 1;

                        switch (strStatus)
                        {
                            case "H": break;
                            case "V": ssView2_Sheet1.Cells[(int)iTemp, intCol].Text = dt.Rows[i]["RESULT"].ToString().Trim(); break;
                            default: ssView2_Sheet1.Cells[(int)iTemp, intCol].Text = "-< 검사중 >-"; break;
                        }

                        strCompare = strSpecNo;
                    }
                }

                ssView2_Sheet1.ColumnCount = ssView2_Sheet1.NonEmptyColumnCount;

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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        string Reference(string strCode, string strAge, string strSex)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int j = 0;

            string rtnVal = "";

            string sCode = "";
            string sNormal = "";
            string sSex = "";
            string sAgeFrom = "";
            string sAgeTo = "";
            string sRefValFrom = "";
            string sRefValTo = "";
            string sAllReference = "";
            string sReference = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     MasterCode, Normal, Sex, AgeFrom, AgeTo, RefvalFrom, RefvalTo ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "Exam_Master_Sub ";
                SQL = SQL + ComNum.VBLF + "     WHERE MasterCode = '" + strCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND Gubun = '41'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        sCode = dt.Rows[i]["MasterCode"].ToString().Trim();
                        sNormal = dt.Rows[i]["Normal"].ToString().Trim();
                        sSex = dt.Rows[i]["Sex"].ToString().Trim();
                        sAgeFrom = dt.Rows[i]["AgeFrom"].ToString().Trim();
                        sAgeTo = dt.Rows[i]["AgeTo"].ToString().Trim();
                        sRefValFrom = dt.Rows[i]["RefvalFrom"].ToString().Trim();
                        sRefValTo = dt.Rows[i]["RefvalTo"].ToString().Trim();

                        sAllReference = sAllReference + sCode + "|" + sNormal + "|" + sSex + "|" + sAgeFrom + "|" +
                                        sAgeTo + "|" + sRefValFrom + "|" + sRefValTo + "|" + "|";
                    }

                    sReference = sAllReference.Replace(sCode, "^");

                    i = (int)VB.L(sReference, "^");

                    if (i == 1) { return rtnVal; }

                    for (j = 2; j <= i; j++)
                    {
                        sNormal = VB.PP(VB.PP(sReference, "^", j), "|", 2);
                        sSex = VB.PP(VB.PP(sReference, "^", j), "|", 3);
                        sAgeFrom = VB.PP(VB.PP(sReference, "^", j), "|", 4);
                        sAgeTo = VB.PP(VB.PP(sReference, "^", j), "|", 5);
                        sRefValFrom = VB.PP(VB.PP(sReference, "^", j), "|", 6);
                        sRefValTo = VB.PP(VB.PP(sReference, "^", j), "|", 7);

                        if (sNormal != "")
                        {
                            rtnVal = sNormal;
                            return rtnVal;
                        }

                        if (sSex == "" || sSex == strSex)
                        {
                            if (sAgeFrom != "" && sAgeTo != "")
                            {
                                if (VB.Val(sAgeFrom) <= VB.Val(strAge) && VB.Val(strAge) <= VB.Val(sAgeTo))
                                {
                                    rtnVal = sRefValFrom + " ~ " + sRefValTo;
                                    return rtnVal;                                    
                                }
                            }
                        }
                    }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            if (rEventClosed != null)
            {
                rEventClosed();
            }
            else
            {
                this.Close();
            }
        }

      
        void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtPano.Text != "")
            {
                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text.Trim(), ComNum.LENPTNO);
                GetPANO();
                GetData();
            }
        }

        void GetPANO()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            if (txtPano.Text.Trim() == "") { return; }

            if (VB.IsNumeric(txtPano.Text) == false)
            {
                ComFunc.MsgBox("등록번호의 (-)는 입력하지 마십시오.");
                txtPano.Focus();
                return;
            }

            txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //환자마스타에서 인적사항을 READ
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     Sname, Jumin1, Jumin2, Jumin3, Sex";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_PATIENT";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + txtPano.Text.Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 1)
                {
                    if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    {
                        GintAge = ComFunc.AgeCalc(clsDB.DbCon, dt.Rows[0]["JUMIN1"].ToString().Trim() + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim()));
                    }
                    else
                    {
                        GintAge = ComFunc.AgeCalc(clsDB.DbCon, dt.Rows[0]["JUMIN1"].ToString().Trim() + dt.Rows[0]["JUMIN2"].ToString().Trim());
                    }

                    GstrSex = dt.Rows[0]["SEX"].ToString().Trim();
                    lblSname.Text = dt.Rows[0]["SNAME"].ToString().Trim() + VB.Space(2) + GintAge + "/" + GstrSex;
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

        void btnPatient_Click(object sender, EventArgs e)
        {
            GstrPano = "";
            frmSearchPanoEvent = new frmSearchPano();
            frmSearchPanoEvent.rGetData += frmSearchPano_rGetData;
            frmSearchPanoEvent.rEventClose += frmSearchPano_rEventClose;
            frmSearchPanoEvent.ShowDialog();

            if (GstrPano != "")
            {
                txtPano.Focus();
                txtPano.Text = VB.Left(GstrPano, 8);
                SendKeys.Send("{Enter}");
                return;
            }
        }

        void frmSearchPano_rEventClose()
        {
            frmSearchPanoEvent.Dispose();
            frmSearchPanoEvent = null;
        }

        void frmSearchPano_rGetData(string GstrHelpCode)
        {
            this.GstrPano = GstrHelpCode;
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            txtPano.Text = "";
            lblSname.Text = "";
            cboWs.SelectedIndex = 0;
            ssView1.ActiveSheet.Rows.Count = 0;
            ssView2.ActiveSheet.Rows.Count = 0;

            txtPano.Select();
        }

        void rdoSel0_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSel0.Checked == true)
            {
                ssView1.Visible = true;
                ssView1.Dock = DockStyle.Fill;
                ssView2.Visible = false;
                ssView2.Dock = DockStyle.None;

                txtPano.Select();
            }            
        }

        void rdoSel1_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSel1.Checked == true)
            {
                ssView2.Visible = true;
                ssView2.Dock = DockStyle.Fill;
                ssView1.Visible = false;
                ssView1.Dock = DockStyle.None;

                txtPano.Select();
            }
        }
    }
}
