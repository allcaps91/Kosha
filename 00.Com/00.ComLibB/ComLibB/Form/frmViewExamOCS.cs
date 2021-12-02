using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmViewExamOCS.cs
    /// Description     : 임상병리 검사결과 조회
    /// Author          : 이정현
    /// Create Date     : 2018-05-29
    /// <history> 
    /// 임상병리 검사결과 조회
    /// </history>
    /// <seealso>
    /// PSMH\FrmViewExam.frm
    /// </seealso>
    /// <vbp>
    /// default 		: 
    /// </vbp>
    /// </summary>
    public partial class frmViewExamOCS : Form
    {
        private string GstrIPDOPD = "";
        private string GstrPtNo = "";
        private string GstrSName = "";
        private string GstrBi = "";
        private string GstrJumin1 = "";
        private string GstrJumin2 = "";
        private string GstrSex = "";
        private string GstrAge = "";
        private string GstrSpecNo = "";        

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
                        if (clsType.User.IdNumber == "31544")
                        {
                            this.Top = 10;
                            this.Left = secondary_screen.Bounds.X + (secondary_screen.Bounds.Width - this.Width) - 10;
                        }
                        else
                        {
                            this.Left = secondary_screen.Bounds.X + 1;
                            this.Top = 0;
                        }
                        
                        this.Show();
                        //this.WindowState = FormWindowState.Maximized;
                        break;
                    }
                }
            }
        }
        #endregion

        public frmViewExamOCS(string strPtNo, string strIPDOPD)
        {
            InitializeComponent();

            GstrPtNo = strPtNo;
            GstrIPDOPD = strIPDOPD;
        }

        #region //폼을 모달리스로 띄울경우 처리함
        public frmViewExamOCS(string strPtNo, string strIPDOPD, Form pModalCallForm, int pModalMonitor)
        {
            InitializeComponent();

            GstrPtNo = strPtNo;
            GstrIPDOPD = strIPDOPD;

            mModalCallForm = pModalCallForm;
            mModalMonitor = pModalMonitor;
        }
        #endregion

        private void frmViewExamOCS_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpSDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-365);
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            double dblITEM51 = GetITEM51();

            if (dblITEM51 > 0)
            {
                dtpSDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(dblITEM51 * -1);
            }

            ssView1_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 0;
            ssView3_Sheet1.RowCount = 0;
            ssExamBae_Sheet1.RowCount = 0;
            lblExamName.Text = "";

            if (GstrPtNo != "")
            {
                txtPtno.Text = GstrPtNo;
                Patient_Name_READ(GstrPtNo);

                lblSName.Text = GstrSName;
                lblSexAge.Text = GstrSex + "/" + GstrAge;
                lblBi.Text = clsVbfunc.GetBiName(GstrBi);

                GetData();
            }
            else
            {
                txtPtno.Text = "";
                lblSName.Text = "";
                lblSexAge.Text = "";
                lblBi.Text = "";
            }

            if (mModalCallForm != null)
            {
                #region //폼을 모달리스로 띄울경우 처리함
                GetMonitorInfo();
                if (mModalMonitor == 2)
                {
                    viewFormMonitor2();
                }
                #endregion
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
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

        private void Patient_Name_READ(string strPtNo)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     Pano, Sname, Jumin1, Jumin2, Jumin3, Sex, Bi ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPtNo + "' ";

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
                    GstrPtNo = "";
                    GstrSName = "";
                    GstrJumin1 = "";
                    GstrJumin2 = "";
                    GstrSex = "";
                    GstrAge = "";
                    GstrBi = "";
                }
                else
                {
                    GstrPtNo = dt.Rows[0]["PANO"].ToString().Trim();
                    GstrSName = dt.Rows[0]["SNAME"].ToString().Trim();
                    GstrBi = dt.Rows[0]["BI"].ToString().Trim();
                    GstrJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();

                    if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    {
                        GstrJumin2 = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    }
                    else
                    {
                        GstrJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();
                    }

                    GstrSex = dt.Rows[0]["SEX"].ToString().Trim();
                    GstrAge = ComFunc.AgeCalc(clsDB.DbCon, GstrJumin1 + GstrJumin2).ToString();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtPtno.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호를 입력해주세요.");
                return;
            }

            txtPtno.Text = ComFunc.LPAD(txtPtno.Text, 8, "0");
            GstrPtNo = txtPtno.Text.Trim();
            Patient_Name_READ(GstrPtNo);

            lblSName.Text = GstrSName;
            lblSexAge.Text = GstrSex + "/" + GstrAge;
            lblBi.Text = clsVbfunc.GetBiName(GstrBi);

            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strJumin = GstrJumin1 + GstrJumin2;
            string strSpecNo = "";
            string strStatus = "";
            string strHicPano = "";
            string strHeaPano = "";

            ssView1_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 0;
            ssView3_Sheet1.RowCount = 0;
            ssExamBae_Sheet1.RowCount = 0;
            lblExamName.Text = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //등록번호로 일반건진 접수번호를 찾음(일반건진은 SPECMST -> 일반건진 접수번호로 전송)
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     WRTNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "HIC_JEPSU ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "HIC_PATIENT WHERE Jumin = '" + strJumin + "') ";
                SQL = SQL + ComNum.VBLF + "         AND JepDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND JepDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND DelDate IS NULL ";

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
                        strHicPano += "'" + ComFunc.LPAD(dt.Rows[i]["WRTNO"].ToString().Trim(), 8, "0") + "',";
                    }

                    strHicPano = VB.Left(strHicPano, strHicPano.Length - 1);
                }

                dt.Dispose();
                dt = null;

                //종합건진 등록번호 찾음(종검은 SPECTMST -> 종검접수번호로 전송)
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     WRTNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "HEA_JEPSU ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano IN ( SELECT Pano FROM " + ComNum.DB_PMPA + "HEA_PATIENT WHERE Jumin = '" + strJumin + "') ";
                SQL = SQL + ComNum.VBLF + "         AND sDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND sDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND DelDate IS NULL   ";

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
                        strHeaPano += "'" + ComFunc.LPAD(dt.Rows[i]["WRTNO"].ToString().Trim(), 8, "0") + "',";
                    }

                    strHeaPano = VB.Left(strHeaPano, strHeaPano.Length - 1);
                }

                dt.Dispose();
                dt = null;

                //검체마스타를 SELECT
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.SpecNo, A.DeptCode, A.Room, A.DrCode, A.WorkSTS, A.SpecCode, A.Status, ";
                SQL = SQL + ComNum.VBLF + "     A.IpdOpd, A.Bi, TO_CHAR(A.BDate,'YYYY-MM-DD') AS BDate,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.BloodDate,'YYYY-MM-DD HH24:MI') AS BloodDate,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.ReceiveDate,'YYYY-MM-DD HH24:MI') AS ReceiveDate,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.ResultDate,'YYYY-MM-DD HH24:MI') AS ResultDate, A.Print, B.NAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "Exam_Specmst A";
                SQL = SQL + ComNum.VBLF + "     LEFT OUTER JOIN " + ComNum.DB_MED + "EXAM_SPECODE B";
                SQL = SQL + ComNum.VBLF + "         ON A.SPECCODE = B.CODE";
                SQL = SQL + ComNum.VBLF + "         AND B.GUBUN = '14'";
                SQL = SQL + ComNum.VBLF + "     WHERE A.Pano = '" + txtPtno.Text + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.BDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.BDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.WorkSTS NOT IN ('A', 'T') ";  //세포학,조직학은 제외
                if (chkAll.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.STATUS IN ('04','14','05') ";
                }
                if (chkGlucoseex.Checked == true)
                {
                    //SQL = SQL + ComNum.VBLF + "  AND NOT (IPDOPD = 'I' AND B.MASTERCODE IN ('CR59','CR59B')) ";
                    SQL = SQL + ComNum.VBLF + "  AND NOT (B.YNAME IN ('CR59','CR59B')) ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY A.ReceiveDate DESC, A.SpecNo ";

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
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSpecNo = dt.Rows[i]["SPECNO"].ToString().Trim();

                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SPECNO"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["RECEIVEDATE"].ToString().Trim() + " (";

                        if (dt.Rows[i]["RESULTDATE"].ToString().Trim() != "")
                        {
                            ssView1_Sheet1.Cells[i, 2].Text += Convert.ToDateTime(dt.Rows[i]["RESULTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }

                        ssView1_Sheet1.Cells[i, 2].Text += ")";

                        switch (dt.Rows[i]["IPDOPD"].ToString().Trim())
                        {
                            case "I":
                                ssView1_Sheet1.Cells[i, 3].Text = "입원";
                                break;
                            default:
                                switch (dt.Rows[i]["BI"].ToString().Trim())
                                {
                                    case "61": ssView1_Sheet1.Cells[i, 3].Text = "종검"; break;
                                    case "71": ssView1_Sheet1.Cells[i, 3].Text = "건진"; break;
                                    case "81": ssView1_Sheet1.Cells[i, 3].Text = "수탁"; break;
                                    default: ssView1_Sheet1.Cells[i, 3].Text = "외래"; break;
                                }
                                break;
                        }

                        ssView1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROOM"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["WORKSTS"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 9].Text = READ_Specno_ExamName(strSpecNo);

                        switch (dt.Rows[i]["STATUS"].ToString().Trim())
                        {
                            case "00": strStatus = "미접수"; break;
                            case "01": strStatus = "검사중"; break;
                            case "02": strStatus = "부분입력"; break;
                            case "03": strStatus = "모두입력"; break;
                            case "04":
                            case "14": strStatus = "부분완료"; break;
                            case "05":
                                if (VB.Val(dt.Rows[i]["PRINT"].ToString().Trim()) == 0) { strStatus = "검사완료"; }
                                if (VB.Val(dt.Rows[i]["PRINT"].ToString().Trim()) > 1) { strStatus = "인쇄완료"; }
                                break;
                            case "06": strStatus = "취소"; break;
                            default: strStatus = "ERROR"; break;
                        }

                        ssView1_Sheet1.Cells[i, 10].Text = strStatus;

                        if (strStatus == "일부완료")
                        {
                            ssView1_Sheet1.Cells[i, 10].BackColor = Color.LightPink;
                        }
                        else if (strStatus == "검사완료" || strStatus == "인쇄완료")
                        {
                            ssView1_Sheet1.Cells[i, 10].BackColor = Color.LightGreen;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                if (strHicPano != "")
                {
                    //검체마스타를 SELECT
                    //일반건진
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.SpecNo, A.DeptCode, A.Room, A.DrCode, A.WorkSTS, A.SpecCode, A.Status, ";
                    SQL = SQL + ComNum.VBLF + "     A.IpdOpd, A.Bi, TO_CHAR(A.BDate,'YYYY-MM-DD') AS BDate,";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.BloodDate,'YYYY-MM-DD HH24:MI') AS BloodDate,";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.ReceiveDate,'YYYY-MM-DD HH24:MI') AS ReceiveDate,";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.OrderDate,'MM/DD HH24:MI') AS OrderDate,";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.ResultDate,'MM/DD HH24:MI') AS ResultDate1,";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.ResultDate,'YYYY-MM-DD HH24:MI') AS ResultDate, A.Print, B.NAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "Exam_Specmst A";
                    SQL = SQL + ComNum.VBLF + "     LEFT OUTER JOIN " + ComNum.DB_MED + "EXAM_SPECODE B";
                    SQL = SQL + ComNum.VBLF + "         ON A.SPECCODE = B.CODE";
                    SQL = SQL + ComNum.VBLF + "         AND B.GUBUN = '14'";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.PANO IN ( " + strHicPano + ") ";
                    SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE  = 'HR'";
                    SQL = SQL + ComNum.VBLF + "         AND A.WorkSTS NOT IN ('A','T') ";  //세포학,조직학은 제외
                    if (chkAll.Checked == false)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.STATUS IN ('04','14','05') ";
                    }
                    SQL = SQL + ComNum.VBLF + "         AND A.BI = '62' ";     //건진
                    if (chkGlucoseex.Checked == true)
                    {
                        //SQL = SQL + ComNum.VBLF + "  AND NOT (IPDOPD = 'I' AND B.MASTERCODE IN ('CR59','CR59B')) ";
                        SQL = SQL + ComNum.VBLF + "  AND NOT (B.YNAME IN ('CR59','CR59B')) ";
                    }
                    SQL = SQL + ComNum.VBLF + "ORDER BY A.ReceiveDate DESC, A.SpecNo ";

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
                            ssView1_Sheet1.RowCount = ssView1_Sheet1.RowCount + 1;
                            ssView1_Sheet1.SetRowHeight(ssView1_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                            strSpecNo = dt.Rows[i]["SPECNO"].ToString().Trim();

                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SPECNO"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["RECEIVEDATE"].ToString().Trim() + " (";

                            if (dt.Rows[i]["RESULTDATE"].ToString().Trim() != "")
                            {
                                ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 2].Text += Convert.ToDateTime(dt.Rows[i]["RESULTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                            }

                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 2].Text += ")";

                            switch (dt.Rows[i]["IPDOPD"].ToString().Trim())
                            {
                                case "I":
                                    ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = "입원";
                                    break;
                                default:
                                    switch (dt.Rows[i]["BI"].ToString().Trim())
                                    {
                                        case "61": ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = "종검"; break;
                                        case "71": ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = "건진"; break;
                                        case "81": ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = "수탁"; break;
                                        default: ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = "외래"; break;
                                    }
                                    break;
                            }

                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["ROOM"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["WORKSTS"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["NAME"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 9].Text = READ_Specno_ExamName(strSpecNo);

                            switch (dt.Rows[i]["STATUS"].ToString().Trim())
                            {
                                case "00": strStatus = "미접수"; break;
                                case "01": strStatus = "검사중"; break;
                                case "02": strStatus = "부분입력"; break;
                                case "03": strStatus = "모두입력"; break;
                                case "04":
                                case "14": strStatus = "부분완료"; break;
                                case "05":
                                    if (VB.Val(dt.Rows[i]["PRINT"].ToString().Trim()) == 0) { strStatus = "검사완료"; }
                                    if (VB.Val(dt.Rows[i]["PRINT"].ToString().Trim()) > 1) { strStatus = "인쇄완료"; }
                                    break;
                                case "06": strStatus = "취소"; break;
                                default: strStatus = "ERROR"; break;
                            }

                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 10].Text = strStatus;

                            if (strStatus == "일부완료")
                            {
                                ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 0, ssView1_Sheet1.RowCount - 1, ssView1_Sheet1.ColumnCount - 1].BackColor = Color.LightPink;
                            }
                            else if (strStatus == "검사완료" || strStatus == "인쇄완료")
                            {
                                ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 0, ssView1_Sheet1.RowCount - 1, ssView1_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;
                            }

                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["ORDERDATE"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 12].Text = VB.Right(dt.Rows[i]["RECEIVEDATE"].ToString().Trim(), 11);
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 13].Text = dt.Rows[i]["RESULTDATE1"].ToString().Trim();
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strHeaPano != "")
                {
                    //검체마스타를 SELECT
                    //종합건진
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.SpecNo, A.DeptCode, A.Room, A.DrCode, A.WorkSTS, A.SpecCode, A.Status, ";
                    SQL = SQL + ComNum.VBLF + "     IpdOpd,Bi,TO_CHAR(BDate,'YYYY-MM-DD') AS BDate,";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.BloodDate,'YYYY-MM-DD HH24:MI') AS BloodDate,";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.ReceiveDate,'YYYY-MM-DD HH24:MI') AS ReceiveDate,";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.OrderDate,'MM/DD HH24:MI') AS OrderDate,";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.ResultDate,'MM/DD HH24:MI') AS ResultDate1,";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.ResultDate,'YYYY-MM-DD HH24:MI') AS ResultDate, A.Print, B.NAME";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "Exam_Specmst A";
                    SQL = SQL + ComNum.VBLF + "     LEFT OUTER JOIN " + ComNum.DB_MED + "EXAM_SPECODE B";
                    SQL = SQL + ComNum.VBLF + "         ON A.SPECCODE = B.CODE";
                    SQL = SQL + ComNum.VBLF + "         AND B.GUBUN = '14'";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.PANO IN (" + strHeaPano + " ) ";
                    SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE = 'TO' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.WorkSTS NOT IN ('A','T') ";  //세포학,조직학은 제외
                    if (chkAll.Checked == false)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.STATUS IN ('04','14','05') ";
                    }
                    SQL = SQL + ComNum.VBLF + "         AND A.BI = '61' ";     //종검
                    if (chkGlucoseex.Checked == true)
                    {
                        //SQL = SQL + ComNum.VBLF + "  AND NOT (IPDOPD = 'I' AND B.MASTERCODE IN ('CR59','CR59B')) ";
                        SQL = SQL + ComNum.VBLF + "  AND NOT (B.YNAME IN ('CR59','CR59B')) ";
                    }
                    SQL = SQL + ComNum.VBLF + "ORDER BY A.ReceiveDate DESC, A.SpecNo ";

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
                            ssView1_Sheet1.RowCount = ssView1_Sheet1.RowCount + 1;
                            ssView1_Sheet1.SetRowHeight(ssView1_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                            strSpecNo = dt.Rows[i]["SPECNO"].ToString().Trim();

                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SPECNO"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["RECEIVEDATE"].ToString().Trim() + " (";

                            if (dt.Rows[i]["RESULTDATE"].ToString().Trim() != "")
                            {
                                ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 2].Text += Convert.ToDateTime(dt.Rows[i]["RESULTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                            }

                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 2].Text += ")";


                            switch (dt.Rows[i]["IPDOPD"].ToString().Trim())
                            {
                                case "I":
                                    ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = "입원";
                                    break;
                                default:
                                    switch (dt.Rows[i]["BI"].ToString().Trim())
                                    {
                                        case "61": ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = "종검"; break;
                                        case "71": ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = "건진"; break;
                                        case "81": ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = "수탁"; break;
                                        default: ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = "외래"; break;
                                    }
                                    break;
                            }

                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["ROOM"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["WORKSTS"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["NAME"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 9].Text = READ_Specno_ExamName(strSpecNo);

                            switch (dt.Rows[i]["STATUS"].ToString().Trim())
                            {
                                case "00": strStatus = "미접수"; break;
                                case "01": strStatus = "검사중"; break;
                                case "02": strStatus = "부분입력"; break;
                                case "03": strStatus = "모두입력"; break;
                                case "04":
                                case "14": strStatus = "부분완료"; break;
                                case "05":
                                    if (VB.Val(dt.Rows[i]["PRINT"].ToString().Trim()) == 0) { strStatus = "검사완료"; }
                                    if (VB.Val(dt.Rows[i]["PRINT"].ToString().Trim()) > 1) { strStatus = "인쇄완료"; }
                                    break;
                                case "06": strStatus = "취소"; break;
                                default: strStatus = "ERROR"; break;
                            }

                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 10].Text = strStatus;

                            if (strStatus == "일부완료")
                            {
                                ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 0, ssView1_Sheet1.RowCount - 1, ssView1_Sheet1.ColumnCount - 1].BackColor = Color.LightPink;
                            }
                            else if (strStatus == "검사완료" || strStatus == "인쇄완료")
                            {
                                ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 0, ssView1_Sheet1.RowCount - 1, ssView1_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;
                            }

                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["ORDERDATE"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 12].Text = VB.Right(dt.Rows[i]["RECEIVEDATE"].ToString().Trim(), 11);
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 13].Text = dt.Rows[i]["RESULTDATE1"].ToString().Trim();
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                
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

        private string READ_Specno_ExamName(string strSPECNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     b.WsCode1, b.WsCode1Pos, a.MasterCode, b.ExamName, COUNT(a.MasterCode) AS CNT ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_RESULTC a, " + ComNum.DB_MED + "EXAM_MASTER b ";
                SQL = SQL + ComNum.VBLF + "     WHERE a.SpecNo = '" + strSPECNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND a.MasterCode = a.SubCode ";
                SQL = SQL + ComNum.VBLF + "         AND a.MasterCode = b.MasterCode(+) ";
                SQL = SQL + ComNum.VBLF + "GROUP BY b.WsCode1, b.WsCode1Pos, a.MasterCode, b.ExamName ";
                SQL = SQL + ComNum.VBLF + "ORDER BY b.WsCode1, b.WsCode1Pos, a.MasterCode, b.ExamName ";

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
                        rtnVal += dt.Rows[i]["EXAMNAME"].ToString().Trim();

                        if (VB.Val(dt.Rows[i]["CNT"].ToString().Trim()) > 1)
                        {
                            rtnVal += "*" + dt.Rows[i]["CNT"].ToString().Trim() + ",";
                        }
                        else
                        {
                            rtnVal += ",";
                        }
                    }

                    rtnVal = VB.Left(rtnVal, rtnVal.Length - 1);
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

        private void btnPrintSelect_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            if (ssView1_Sheet1.RowCount == 0) { return; }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;

            string strSpecNo = "";
            string strReqCnt = "";

            if (txtPtno.Text.Trim() == "" || GstrPtNo == "" || txtPtno.Text.Trim() != GstrPtNo)
            {
                ComFunc.MsgBox("해당하는 환자 등록번호를 다시 입력후 조회해주십시오.");
                return;
            }

            if (ComFunc.MsgBoxQ("정말로 원무과로 검사결과 인쇄 요청 하시겠습니까?", 
                "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            strReqCnt = VB.InputBox("인쇄 신청 부수를 입력하시기 바랍니다(숫자만 입력가능)", "신청 부수 입력");

            if (strReqCnt == "")
            {
                ComFunc.MsgBox("신청 부수를 입력하지 않으셨습니다.");
                return;
            }

            if (VB.IsNumeric(strReqCnt) == false)
            {
                ComFunc.MsgBox("잘못된 신청 부수 입력입니다.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView1_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssView1_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strSpecNo = ssView1_Sheet1.Cells[i, 1].Text.Trim();

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     ROWID";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_RESULTPRINT ";
                        SQL = SQL + ComNum.VBLF + "     WHERE SPECNO = '" + strSpecNo + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND ACTDATE = TRUNC(SYSDATE)";

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
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_MED + "EXAM_RESULTPRINT";
                            SQL = SQL + ComNum.VBLF + "     (ACTDATE, SPECNO, PANO, BI, SNAME, IPDOPD, SEX, DEPTCODE, ";
                            SQL = SQL + ComNum.VBLF + "     WARD, ROOM, DRCODE, PRINT, SENDDATE, PRINTDATE, PRINTDATE2, SABUN, REQCNT)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         TRUNC(SYSDATE), ";
                            SQL = SQL + ComNum.VBLF + "         '" + strSpecNo + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + GstrPtNo + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + GstrBi + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + ComFunc.LeftH(GstrSName, 10) + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + GstrIPDOPD + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + GstrSex + "', ";
                            SQL = SQL + ComNum.VBLF + "         '', ";
                            SQL = SQL + ComNum.VBLF + "         '', ";
                            SQL = SQL + ComNum.VBLF + "         '', ";
                            SQL = SQL + ComNum.VBLF + "         '', ";
                            SQL = SQL + ComNum.VBLF + "         '', ";
                            SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                            SQL = SQL + ComNum.VBLF + "         '', ";
                            SQL = SQL + ComNum.VBLF + "         '', ";
                            SQL = SQL + ComNum.VBLF + "         '" + clsType.User.Sabun + "', ";
                            SQL = SQL + ComNum.VBLF + "         " + strReqCnt;
                            SQL = SQL + ComNum.VBLF + "     )";
                        }
                        else
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "EXAM_RESULTPRINT";
                            SQL = SQL + ComNum.VBLF + "     SET ";
                            SQL = SQL + ComNum.VBLF + "         PRINTDATE = '', ";
                            SQL = SQL + ComNum.VBLF + "         PRINTDATE2 = '' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        dt.Dispose();
                        dt = null;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("인쇄요청완료");
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
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPtno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPtno.Text.Trim() == "") { return; }

                txtPtno.Text = ComFunc.LPAD(txtPtno.Text, 8, "0");
                GstrPtNo = txtPtno.Text.Trim();
                Patient_Name_READ(GstrPtNo);

                lblSName.Text = GstrSName;
                lblSexAge.Text = GstrSex + "/" + GstrAge;
                lblBi.Text = clsVbfunc.GetBiName(GstrBi);

                GetData();
            }
        }

        private void ssView1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (Convert.ToBoolean(ssView1_Sheet1.Cells[e.Row, 0].Value) == true)
            {
                ssView1_Sheet1.Cells[e.Row, 0, e.Row, ssView1_Sheet1.ColumnCount - 1].ForeColor = Color.Red;
            }
            else
            {
                ssView1_Sheet1.Cells[e.Row, 0, e.Row, ssView1_Sheet1.ColumnCount - 1].ForeColor = Color.Black;
            }
        }

        private void ssView1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;
            int k = 0;

            string sSpecNo = "";
            string sRef = "";
            string sResultDate = "";    //결과일자
            string sStatus = "";        //상태
            string sResult = "";        //결과
            string strOK = "";          //Display여부

            int intCnt1 = 0;

            sSpecNo = ssView1_Sheet1.Cells[e.Row, 1].Text.Trim();
            GstrSpecNo = sSpecNo;
            ssView2_Sheet1.RowCount = 0;
            ssView3_Sheet1.RowCount = 0;
            lblExamName.Text = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     R.Status, R.MasterCode, R.SubCode, R.Result, R.Refer, R.Panic, ";
                SQL = SQL + ComNum.VBLF + "     R.Delta, R.Unit, R.SeqNo, M.ExamName, TO_CHAR(R.ResultDate,'YYYY-MM-DD') AS ResultDate ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "Exam_ResultC R, " + ComNum.DB_MED + "Exam_Master M ";
                SQL = SQL + ComNum.VBLF + "     WHERE SpecNo = '" + sSpecNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND R.SubCode = M.MasterCode(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY R.SeqNo ";

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
                        sResultDate = dt.Rows[i]["RESULTDATE"].ToString().Trim();
                        sStatus = dt.Rows[i]["STATUS"].ToString().Trim();
                        sResult = dt.Rows[i]["RESULT"].ToString().Trim();

                        if (sStatus == "H")
                        {
                            strOK = "OK";
                        }
                        else if (sStatus == "V")
                        {
                            strOK = "OK";
                            if (sResult == "") { strOK = "OK"; }
                            if (dt.Rows[i]["MASTERCODE"].ToString().Trim() == dt.Rows[i]["SUBCODE"].ToString().Trim()) { strOK = "OK"; }
                        }
                        else
                        {
                            strOK = "OK";

                            if (dt.Rows[i]["MASTERCODE"].ToString().Trim() == "M132")
                            {
                                SQL = ""; 
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     PANO";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_RESULT_BAE ";
                                SQL = SQL + ComNum.VBLF + "     WHERE SPECNO = '" + sSpecNo + "' ";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                if (dt1.Rows.Count == 0)
                                {
                                    sResult = "-< 검사중 >-";
                                }
                                else
                                {
                                    sResult = "-< 혈액배양검사 중간결과 >-";
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                            else
                            { 
                                sResult = "-< 검사중 >-";
                            }
                        }

                        ////Foot Note를 READ
                        //SQL = "";
                        //SQL = "SELECT";
                        //SQL = SQL + ComNum.VBLF + "     FootNote";
                        //SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "Exam_ResultCf ";
                        //SQL = SQL + ComNum.VBLF + "     WHERE SpecNo = '" + sSpecNo + "' ";
                        ////SQL = SQL + ComNum.VBLF + "         AND SeqNo =  '" + VB.Val(dt.Rows[i]["SeqNo"].ToString().Trim()).ToString("###") + "' ";
                        //SQL = SQL + ComNum.VBLF + "         AND SeqNo =  '" + dt.Rows[i]["SeqNo"].ToString().Trim() + "' ";

                        //SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        //if (SqlErr != "")
                        //{
                        //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        //    Cursor.Current = Cursors.Default;
                        //    return;
                        //}
                        //if (dt1.Rows.Count > 0)
                        //{
                        //    strOK = "OK";
                        //    intCnt1 = dt1.Rows.Count;
                        //}

                        //dt1.Dispose();
                        //dt1 = null;

                        if (strOK == "OK")
                        {
                            ssView2_Sheet1.RowCount = ssView2_Sheet1.RowCount + 1;
                            ssView2_Sheet1.SetRowHeight(ssView2_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["EXAMNAME"].ToString().Trim();
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 1].Text = sResult;
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["REFER"].ToString().Trim();
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["UNIT"].ToString().Trim();

                            sRef = Reference(dt.Rows[i]["SUBCODE"].ToString().Trim(), GstrAge, GstrSex, sResultDate);

                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 4].Text = sRef;
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["SUBCODE"].ToString().Trim();

                            if (dt.Rows[i]["REFER"].ToString().Trim() != "")
                            {
                                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 0, ssView2_Sheet1.RowCount - 1, ssView2_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(190, 255, 190);
                            }
                        }

                       
                        //Foot Note를 READ
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     FootNote";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "Exam_ResultCf ";
                        SQL = SQL + ComNum.VBLF + "     WHERE SpecNo = '" + sSpecNo + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND SeqNo = '" + dt.Rows[i]["SeqNo"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                ssView2_Sheet1.RowCount = ssView2_Sheet1.RowCount + 1;
                                ssView2_Sheet1.SetRowHeight(ssView2_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 1].Text = dt1.Rows[k]["FOOTNOTE"].ToString().Trim();
                                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 1].ForeColor = Color.Blue;
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;
                        

                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_MED + "EXAM_RESULTC_CV";
                        SQL = SQL + ComNum.VBLF + "     SET";
                        SQL = SQL + ComNum.VBLF + "         CHKDATE = SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         CHKSABUN = '" + clsType.User.Sabun + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE SPECNO = '" + sSpecNo + "' ";
                        SQL = SQL + ComNum.VBLF + "     AND CHKDATE IS NULL ";
                        SQL = SQL + ComNum.VBLF + "     AND GBN IN ('1', '2') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                lblExamName.Text = "검체번호 : " + sSpecNo;
                
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

        private string Reference(string strCode, string strAge, string strSex, string strRDate)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int k = 0;
            int intCnt = 0;

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
                SQL = SQL + ComNum.VBLF + "         AND Gubun = '41' ";   //41:Reference Value
                //2019-09-02 안정수 추가  
                SQL = SQL + ComNum.VBLF + "AND (SEX IS NULL OR SEX = ' ' OR SEX= '" + strSex + "')  ";
                SQL = SQL + ComNum.VBLF + "AND ((AGEFROM = 0 AND AGETO = 99) OR  (AGEFROM <= '" + strAge + "' AND AGETO >= '" + strAge + "'))  ";
                if (strRDate.Length > 1)
                {
                    SQL = SQL + ComNum.VBLF + "AND ((EXPIREDATE IS NOT NULL AND EXPIREDATE >= '" + strRDate + "') OR (EXPIREDATE IS NULL)) ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY EXPIREDATE";
                }

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

                        if (sAllReference == "") { return rtnVal; }

                        sReference = sAllReference.Replace(sCode, "^");

                        intCnt = Convert.ToInt32(VB.L(sReference, "^"));

                        if (intCnt == 1) { return rtnVal; }

                        for (k = 2; k <= intCnt; k++)
                        {
                            sNormal = VB.Split(VB.Split(sReference, "^")[k - 1], "|")[1];
                            sSex = VB.Split(VB.Split(sReference, "^")[k - 1], "|")[2];
                            sAgeFrom = VB.Split(VB.Split(sReference, "^")[k - 1], "|")[3];
                            sAgeTo = VB.Split(VB.Split(sReference, "^")[k - 1], "|")[4];
                            sRefValFrom = VB.Split(VB.Split(sReference, "^")[k - 1], "|")[5];
                            sRefValTo = VB.Split(VB.Split(sReference, "^")[k - 1], "|")[6];

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

        private void ssView2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strCODE = "";
            string strJumin = GstrJumin1 + GstrJumin2;
            string strHicPano = "";
            string strHeaPano = "";

            ssView3_Sheet1.RowCount = 0;
            ssExamBae_Sheet1.RowCount = 0;
            panExamBae.Visible = false;

            strCODE = ssView2_Sheet1.Cells[e.Row, 5].Text.Trim();
            lblExamName.Text = strCODE + " : " + ssView2_Sheet1.Cells[e.Row, 0].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //등록번호로 일반건진 접수번호를 찾음(일반건진은 SPECMST -> 일반건진 접수번호로 전송)
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     WRTNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "HIC_JEPSU ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "HIC_PATIENT ";
                SQL = SQL + ComNum.VBLF + "                         WHERE Jumin = '" + strJumin + "') ";
                SQL = SQL + ComNum.VBLF + "         AND DelDate IS NULL ";

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
                        strHicPano += "'" + ComFunc.LPAD(dt.Rows[i]["WRTNO"].ToString().Trim(), 8, "0") + "',";
                    }

                    strHicPano = VB.Left(strHicPano, strHicPano.Length - 1);
                }

                dt.Dispose();
                dt = null;

                //종합건진 등록번호 찾음(종검은 SPECTMST -> 종검번호로 전송)
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     Pano";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "HEA_PATIENT ";
                SQL = SQL + ComNum.VBLF + "     WHERE Jumin = '" + strJumin + "' ";

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
                    strHeaPano = ComFunc.LPAD(dt.Rows[0]["PANO"].ToString().Trim(), 8, "0");
                }

                dt.Dispose();
                dt = null;

                //자료를 SELECT
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(a.ReceiveDate,'YYYY-MM-DD') AS ReceiveDate, b.Result ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "Exam_Specmst a, " + ComNum.DB_MED + "EXAM_RESULTC b ";
                SQL = SQL + ComNum.VBLF + "     WHERE a.Pano = '" + txtPtno.Text + "' ";
                SQL = SQL + ComNum.VBLF + "         AND a.BDate >= TRUNC(SYSDATE-1000) ";
                SQL = SQL + ComNum.VBLF + "         AND a.WorkSTS NOT IN ('A','T') ";  //세포학,조직학은 제외
                SQL = SQL + ComNum.VBLF + "         AND a.Status IN ('04','14','05') ";
                SQL = SQL + ComNum.VBLF + "         AND a.SpecNo = b.SpecNo(+) ";
                SQL = SQL + ComNum.VBLF + "         AND b.SubCode = '" + strCODE + "' ";
                SQL = SQL + ComNum.VBLF + "         AND a.BI NOT IN ('61', '62') ";      //건진,종검 제외
                SQL = SQL + ComNum.VBLF + "ORDER BY a.ReceiveDate DESC, a.SpecNo ";

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
                    ssView3_Sheet1.RowCount = dt.Rows.Count;
                    ssView3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RECEIVEDATE"].ToString().Trim();
                        ssView3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RESULT"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                //혈액배양검사
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE, RESULT";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_RESULT_BAE ";
                SQL = SQL + ComNum.VBLF + "     WHERE SPECNO = '" + GstrSpecNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + txtPtno.Text + "' ";

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
                    ssExamBae_Sheet1.RowCount = dt.Rows.Count;
                    ssExamBae_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssExamBae_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssExamBae_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RESULT"].ToString().Trim();
                    }

                    panExamBae.Visible = true;
                }

                dt.Dispose();
                dt = null;

                if (strHicPano != "")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(a.ReceiveDate,'YYYY-MM-DD') AS ReceiveDate, b.Result ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "Exam_Specmst a, " + ComNum.DB_MED + "EXAM_RESULTC b ";
                    SQL = SQL + ComNum.VBLF + "     WHERE a.Pano = '" + txtPtno.Text + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND a.BDate >= TRUNC(SYSDATE-1000) ";
                    SQL = SQL + ComNum.VBLF + "         AND a.WorkSTS NOT IN ('A','T') ";  //세포학,조직학은 제외
                    SQL = SQL + ComNum.VBLF + "         AND a.Status IN ('04','14','05') ";
                    SQL = SQL + ComNum.VBLF + "         AND a.SpecNo = b.SpecNo(+) ";
                    SQL = SQL + ComNum.VBLF + "         AND b.SubCode = '" + strCODE + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND a.BI = '62' ";   //건진
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.ReceiveDate DESC, a.SpecNo ";

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
                            ssView3_Sheet1.RowCount = ssView3_Sheet1.RowCount + 1;
                            ssView3_Sheet1.SetRowHeight(ssView3_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                            ssView3_Sheet1.Cells[ssView3_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["RECEIVEDATE"].ToString().Trim();
                            ssView3_Sheet1.Cells[ssView3_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["RESULT"].ToString().Trim();
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strHeaPano != "")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(a.ReceiveDate,'YYYY-MM-DD') AS ReceiveDate, b.Result ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "Exam_Specmst a, " + ComNum.DB_MED + "EXAM_RESULTC b ";
                    SQL = SQL + ComNum.VBLF + "     WHERE a.Pano = '" + txtPtno.Text + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND a.BDate >= TRUNC(SYSDATE-1000) ";
                    SQL = SQL + ComNum.VBLF + "         AND a.WorkSTS NOT IN ('A','T') "; //세포학,조직학은 제외
                    SQL = SQL + ComNum.VBLF + "         AND a.Status IN ('04','14','05') ";
                    SQL = SQL + ComNum.VBLF + "         AND a.SpecNo = b.SpecNo(+) ";
                    SQL = SQL + ComNum.VBLF + "         AND b.SubCode = '" + strCODE + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND a.BI = '61' "; //종검
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.ReceiveDate DESC,a.SpecNo ";

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
                            ssView3_Sheet1.RowCount = ssView3_Sheet1.RowCount + 1;
                            ssView3_Sheet1.SetRowHeight(ssView3_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                            ssView3_Sheet1.Cells[ssView3_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["RECEIVEDATE"].ToString().Trim();
                            ssView3_Sheet1.Cells[ssView3_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["RESULT"].ToString().Trim();
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            panExamBae.Visible = false;
            ssExamBae_Sheet1.RowCount = 0;
        }

        private void frmViewExamOCS_FormClosed(object sender, FormClosedEventArgs e)
        {
            #region //폼을 모달리스로 띄울경우 처리함
            if (mModalCallForm != null)
            {
                rEventClosed();
            }
            #endregion
        }
    }
}
