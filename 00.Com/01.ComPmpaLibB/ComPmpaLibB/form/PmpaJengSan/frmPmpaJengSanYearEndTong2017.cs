using System;
using System.Data;
using System.Windows.Forms;
using ComBase;
using System.IO;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaJengSanYearEndTong2016.cs
    /// Description     : 2017년 연말 정산 집계 프로그램
    /// Author          : 이정현
    /// Create Date     : 2018-08-16
    /// <history> 
    /// 2017년 연말 정산 집계 프로그램
    /// </history>
    /// <seealso>
    /// PSMH\OPD\jengsan\Frm연말정산집계_2017.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\OPD\jengsan\jengsan.vbp
    /// </vbp>
    /// </summary>
    public partial class frmPmpaJengSanYearEndTong2017 : Form
    {
        private string GstrName = "";
        private string GstrYear = "";
        private string GstrSname = "";
        private string GstrJumin1 = "";
        private string GstrJumin2 = "";
        private string GstrStartDate = "";
        private string GstrLastDate = "";
        private string GstrZipCode = "";
        private string GstrJiname = "";
        private string GstrJuso = "";
        private string GstrJuso1 = "";
        private string GstrSex = "";
        private string GstrFal = "";
        private string GstrJumin = "";
        private string GstrFileChk = "";

        public frmPmpaJengSanYearEndTong2017()
        {
            InitializeComponent();
        }

        public frmPmpaJengSanYearEndTong2017(string strFileChk)
        {
            InitializeComponent();

            GstrFileChk = strFileChk;
        }

        private void frmPmpaJengSanYearEndTong2017_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpFDate.Value = Convert.ToDateTime("2020-01-01");
            dtpTDate.Value = Convert.ToDateTime("2020-12-31");
            dtpFDate2.Value = Convert.ToDateTime("2020-01-01");
            dtpTDate2.Value = Convert.ToDateTime("2020-12-31");
            dtpYear.Value = Convert.ToDateTime("2020-01-01");
            dtpYY.Value = Convert.ToDateTime("2020-01-01");

            txtPano.Text = "";
            txtName.Text = "박시철";
            txtBDate.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            ssView1_Sheet1.RowCount = 0;

            if (System.Diagnostics.Debugger.IsAttached == false)
            {
                if (clsType.User.Sabun != "4349" && clsType.User.Sabun != "40024")
                {
                    btnOK.Enabled = false;
                    btnBuild.Enabled = false;

                    btnData.Enabled = false;
                    btnFile.Enabled = false;
                    btnFileNew.Enabled = false;
                }
            }

            Clear();
        }

        private void Clear()
        {
            GstrName = "";
            GstrYear = "";
            GstrSname = "";
            GstrJumin1 = "";
            GstrJumin2 = "";
            GstrStartDate = "";
            GstrLastDate = "";
            GstrZipCode = "";
            GstrJiname = "";
            GstrJuso = "";
            GstrJuso1 = "";
            GstrSex = "";
            GstrFal = "";
            GstrJumin = "";
        }

        private void btnJob3_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;

            string strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            string strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //포스코 연말정산 제외
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE, DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND JIN = 'N' ";

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
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_PMPA + "OPD_SLIP";
                        SQL = SQL + ComNum.VBLF + "     SET GBPOSCO = 'N' ";
                        SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "     AND BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND DEPTCODE = '" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "' ";

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

                clsDB.setCommitTran(clsDB.DbCon);
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

        private void btnJob_Click(object sender, EventArgs e)
        {
            frmPmpaJengSanSubmitExceptPat frm = new ComPmpaLibB.frmPmpaJengSanSubmitExceptPat();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);
        }

        private void btnJob2_Click(object sender, EventArgs e)
        {
            frmPmpaJengSanBabyMatching frm = new frmPmpaJengSanBabyMatching();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);
        }

        private void btnCard_Click(object sender, EventArgs e)
        {
            frmPmpaViewCreditCardPaymentList frm = new frmPmpaViewCreditCardPaymentList();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);
        }

        private void btnWeb_Click(object sender, EventArgs e)
        {
            frmPmpaJengSanYearEndWeb frm = new frmPmpaJengSanYearEndWeb();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;

            string strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            string strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");

            ssView1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "CREATE OR REPLACE VIEW VIEW_PANO ";
                SQL = SQL + ComNum.VBLF + "     (PANO, JUMIN1, JUMIN3, SNAME) AS ";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     B.PANO, B.JUMIN1,B.JUMIN3, B.SNAME ";
                SQL = SQL + ComNum.VBLF + "From " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "     WHERE ACTDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND ACTDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND BUN IN ('91','99','92') ";
                SQL = SQL + ComNum.VBLF + "         AND (GBPOSCO <> 'N' OR GBPOSCO IS NULL) ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO  = B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "GROUP BY B.PANO, B.JUMIN1, B.JUMIN3, B.SNAME ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                //예약검사
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     B.PANO, B.JUMIN1, B.JUMIN3, B.SNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "     WHERE (ACTDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "             AND ACTDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "             OR TRANSDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND TRUNC(TRANSDate) <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "             OR RETDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "             AND TRUNC(RETDate) <= TO_DATE('" + strTDate + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "GROUP BY B.PANO, B.JUMIN1, B.JUMIN3, B.SNAME ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     B.PANO, B.JUMIN1, B.JUMIN3, B.SNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "     WHERE ACTDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND ACTDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND BUN IN ('85','87','89','92') ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "GROUP BY B.PANO, B.JUMIN1, B.JUMIN3, B.SNAME ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                //종합건비
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     B.PANO, B.JUMIN1, B.JUMIN3, B.SNAME  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "HEA_SUNAP A, " + ComNum.DB_PMPA + "HEA_JEPSU C, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.SUDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.SUDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.BONINAMT <> 0 ";
                SQL = SQL + ComNum.VBLF + "         AND C.PTNO = B.PANO ";
                SQL = SQL + ComNum.VBLF + "         AND A.WRTNO = C.WRTNO(+) ";
                SQL = SQL + ComNum.VBLF + "         AND A.WRTNO > 0 ";
                SQL = SQL + ComNum.VBLF + "         AND C.GbSTS NOT IN ('0') ";
                SQL = SQL + ComNum.VBLF + "GROUP BY B.PANO, B.JUMIN1, B.JUMIN3, B.SNAME ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                //일반건진
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     C.PANO, C.JUMIN1, C.JUMIN3, C.SNAME  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "HIC_SUNAP A, " + ComNum.DB_PMPA + "HIC_PATIENT B, " + ComNum.DB_PMPA + "BAS_PATIENT C ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.SUDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.SUDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO ";
                SQL = SQL + ComNum.VBLF + "         AND B.PTNO = C.PANO ";
                SQL = SQL + ComNum.VBLF + "         AND A.BONINAMT <> 0 ";
                SQL = SQL + ComNum.VBLF + "         AND SUBSTR(C.PANO,1,1) <> '9' ";  //9로시작하는 외래번호 제외 건진만
                SQL = SQL + ComNum.VBLF + "GROUP BY C.PANO, C.JUMIN1, C.JUMIN3, C.SNAME  ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                //개인미수입금은 진료과 없음.강제로 'DE'로 처리함.
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, B.JUMIN1, B.JUMIN3, B.SNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "     WHERE BDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND BDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND GUBUN1 = '2' ";
                SQL = SQL + ComNum.VBLF + "         AND (POBUN IS NULL OR POBUN = '') ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO  = B.PANO ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     '', JUMIN1, JUMIN3, SNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_ETCSLIP ";
                SQL = SQL + ComNum.VBLF + "     WHERE ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                //2009-12-09 윤조연 산전지원금 금액
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     B.PANO, B.JUMIN1, B.JUMIN3, B.SNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PANO  = B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "         AND a.ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND a.ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND a.DEPTCODE = 'OG' ";
                SQL = SQL + ComNum.VBLF + "         AND a.OGAMT <> 0 ";
                SQL = SQL + ComNum.VBLF + "GROUP BY B.PANO, B.JUMIN1, B.JUMIN3, B.SNAME ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, JUMIN1, JUMIN3, SNAME ";
                SQL = SQL + ComNum.VBLF + "FROM VIEW_PANO ";
                SQL = SQL + ComNum.VBLF + "GROUP BY PANO, JUMIN1, JUMIN3, SNAME ";
                SQL = SQL + ComNum.VBLF + "ORDER BY JUMIN1, PANO ";

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
                        ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["JUMIN1"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 3].Text = clsAES.DeAES(dt.Rows[i]["JUMIN3"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "DROP VIEW VIEW_PANO";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
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

        private void btnBuild_Click(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;
            int k = 0;

            string strPANO = "";
            string strSname = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strDept = "";
            string strYear = "";
            string strEDI = "";
            string strActDate = "";
            string strFDate = "";
            string strTDate = "";
            string strSTime = "";
            string strETime = "";
            string strGamsabun = ""; //2017-12-31 add
            string strBABY = ""; //2017-12-31 add

            double dblAmt = 0;
            double dblGamAmt = 0;
            double dblSeqNo = 0;

            strEDI = "37100068";

            strYear = VB.InputBox("작업 연도를 입력해주세요??(YYYY):");

            if (strYear.Length != 4)
            {
                ComFunc.MsgBox("작업 연도를 다시 확인 해주세요!!!");
                return;
            }

            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     COUNT(*) AS CNT";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_JUNMST ";
                SQL = SQL + ComNum.VBLF + "     WHERE TDATE >= '" + Convert.ToDateTime(strTDate).ToString("yyyyMMdd") + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    if (VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("최종파일작성한 일자안에 있습니다. 자료형성을 할 수 없습니다.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     COUNT(*) AS CNT";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_JUNSLIP ";
                SQL = SQL + ComNum.VBLF + "     WHERE ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    if (VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) > 0)
                    {
                        if (ComFunc.MsgBoxQ("형성한 자료가 있습니다. 삭제후 다시 빌더 하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        {
                            SQL = "";
                            SQL = "DELETE " + ComNum.DB_PMPA + "ETC_JUNSLIP ";
                            SQL = SQL + ComNum.VBLF + "     WHERE YEAR = '" + strYear + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "         AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        else
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     MAX(SEQNO) AS SEQNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_JUNSLIP ";
                SQL = SQL + ComNum.VBLF + "     WHERE YEAR = '" + strYear + "' ";

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
                    dblSeqNo = VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim()) + 1;
                }
                else
                {
                    dblSeqNo = 1;
                }

                dt.Dispose();
                dt = null;

                progressBar1.Value = 0;
                progressBar1.Maximum = ssView1_Sheet1.NonEmptyRowCount;

                dblSeqNo = 999;

                strSTime = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");

                for (i = 0; i < ssView1_Sheet1.NonEmptyRowCount; i++)
                {
                    strPANO = ssView1_Sheet1.Cells[i, 0].Text.Trim();
                    strSname = ssView1_Sheet1.Cells[i, 1].Text.Trim();
                    strJumin1 = ssView1_Sheet1.Cells[i, 2].Text.Trim();
                    strJumin2 = ssView1_Sheet1.Cells[i, 3].Text.Trim();

                    lblCnt.Text = i + "/" + ssView1_Sheet1.NonEmptyRowCount;

                    strGamsabun = "";

                    strGamsabun = clsVbfunc.READ_Gamek_infoSabun(strJumin1 + strJumin2);

                    #region JengSanView_New

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "WORK_JENGSAN_NEW ";
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(ACTDATE,'YYYY-MM-DD') AS ACTDATE, DEPTCODE, SUM(AMT1 + AMT2), 0 ";
                    SQL = SQL + ComNum.VBLF + "     From " + ComNum.DB_PMPA + "OPD_SLIP ";
                    SQL = SQL + ComNum.VBLF + "         WHERE ACTDate >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND ACTDate <= TO_DATE('" + strTDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND SUNEXT = 'Y99' ";
                    SQL = SQL + ComNum.VBLF + "             AND (GBPOSCO <> 'N' OR GBPOSCO IS NULL) ";  //포스코 제외
                    SQL = SQL + ComNum.VBLF + "             AND Pano = '" + strPANO +"' ";
                    SQL = SQL + ComNum.VBLF + "             AND PART <> '#' ";
                    SQL = SQL + ComNum.VBLF + "             AND SEQNO <> -1 ";
                    SQL = SQL + ComNum.VBLF + "     GROUP BY ACTDATE, DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "     HAVING SUM(AMT1 + AMT2) <> 0 ";
                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";
                    //감액금
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(ACTDATE,'YYYY-MM-DD') AS ACTDATE, DEPTCODE, 0, SUM(AMT1 + AMT2) ";
                    SQL = SQL + ComNum.VBLF + "     From " + ComNum.DB_PMPA + "OPD_SLIP ";
                    SQL = SQL + ComNum.VBLF + "         WHERE ACTDate >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND ACTDate <= TO_DATE('" + strTDate +"','YYYY-MM-DD') ";
                    
                    if (strGamsabun != "")
                    {
                        SQL = SQL + ComNum.VBLF + "             AND sunext in ('Y92I','Y92J','Y92K','Y92L','Y92M','Y92A','Y92B','Y92C','Y92D') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "             AND 1 = 2 ";
                    }

                    SQL = SQL + ComNum.VBLF + "             AND (GBPOSCO <> 'N' OR GBPOSCO IS NULL) ";  //포스코 제외
                    SQL = SQL + ComNum.VBLF + "             AND Pano = '" + strPANO +"' ";
                    //SQL = SQL + ComNum.VBLF + "             AND PART <> '#' ";
                    //SQL = SQL + ComNum.VBLF + "             AND SEQNO <> -1 ";
                    SQL = SQL + ComNum.VBLF + "     GROUP BY ACTDATE, DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "     HAVING SUM(AMT1 + AMT2) <> 0 ";
                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";

                    //예약금
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(DATE1,'YYYY-MM-DD') AS ACTDATE, DEPTCODE, SUM(AMT7), 0 ";
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                    SQL = SQL + ComNum.VBLF + "         WHERE Pano = '" + strPANO +"' ";
                    SQL = SQL + ComNum.VBLF + "             AND Date1 >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND DATE1 <= TO_DATE('" + strTDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     GROUP BY DATE1, DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";

                    //예약금
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(DATE1,'YYYY-MM-DD') AS ACTDATE, DEPTCODE, 0 ,SUM(AMT5) ";
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                    SQL = SQL + ComNum.VBLF + "         WHERE Pano = '" + strPANO +"' ";
                    SQL = SQL + ComNum.VBLF + "             AND Date1 >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND DATE1 <= TO_DATE('" + strTDate +"','YYYY-MM-DD') ";

                    if (strGamsabun != "")
                    {
                        SQL = SQL + ComNum.VBLF + "             AND gbgamek in ('21','22','23','24','25' ,'11','12','13','14','15') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "             AND 1 = 2 ";
                    }

                    SQL = SQL + ComNum.VBLF + "     GROUP BY DATE1, DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";

                    //예약금 환불
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(RETDATE,'YYYY-MM-DD') AS ACTDATE, DEPTCODE, SUM(RETAMT), 0 ";
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                    SQL = SQL + ComNum.VBLF + "         WHERE Pano = '" + strPANO +"' ";
                    SQL = SQL + ComNum.VBLF + "             AND RETDATE >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND RETDATE < TO_DATE('" + Convert.ToDateTime(strTDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND RETDATE IS NOT NULL ";
                    SQL = SQL + ComNum.VBLF + "     GROUP BY RETDATE, DEPTCODE, BI ";
                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";

                    //예약금 환불
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(RETDATE,'YYYY-MM-DD') AS ACTDATE, DEPTCODE, 0, SUM(AMT5) * -1 ";
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                    SQL = SQL + ComNum.VBLF + "         WHERE Pano = '" + strPANO +"' ";
                    SQL = SQL + ComNum.VBLF + "             AND RETDATE >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND RETDATE < TO_DATE('" + Convert.ToDateTime(strTDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                    if (strGamsabun != "")
                    {
                        SQL = SQL + ComNum.VBLF + "             AND gbgamek in ('21','22','23','24','25' ,'11','12','13','14','15') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "             AND 1 = 2 ";
                    }

                    SQL = SQL + ComNum.VBLF + "     GROUP BY RETDATE, DEPTCODE, BI ";
                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";

                    //--------------------------------------------------------------------------------------------------
                    //예약검사
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(ACTDATE,'YYYY-MM-DD') AS ACTDATE, DEPTCODE, SUM(AMT6), 0 ";
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                    SQL = SQL + ComNum.VBLF + "         WHERE Pano = '" + strPANO +"' ";
                    SQL = SQL + ComNum.VBLF + "             AND ACTDATE >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND ACTDATE <= TO_DATE('" + strTDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     GROUP BY ACTDATE, DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";

                    //예약검사 환불
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(RETDATE,'YYYY-MM-DD') AS ACTDATE, DEPTCODE, SUM(RETAMT), 0 ";
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                    SQL = SQL + ComNum.VBLF + "         WHERE Pano = '" + strPANO +"' ";
                    SQL = SQL + ComNum.VBLF + "             AND RETDATE >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND RETDATE < TO_DATE('" + Convert.ToDateTime(strTDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND RETDATE IS NOT NULL ";
                    SQL = SQL + ComNum.VBLF + "     GROUP BY RETDATE, DEPTCODE, BI ";
                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";

                    //예약검사 대체
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(TRANSDATE,'YYYY-MM-DD') AS ACTDATE, DEPTCODE, SUM(TRANSAMT) * -1, 0 ";
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                    SQL = SQL + ComNum.VBLF + "         WHERE Pano = '" + strPANO +"' ";
                    SQL = SQL + ComNum.VBLF + "             AND TRANSDATE >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND TRANSDATE < TO_DATE('" + Convert.ToDateTime(strTDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND TRANSDATE IS NOT NULL ";
                    SQL = SQL + ComNum.VBLF + "     GROUP BY TRANSDATE, DEPTCODE, BI ";
                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";
                    //--------------------------------------------------------------------------------------------------
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, DEPTCODE, ";
                    SQL = SQL + ComNum.VBLF + "         SUM(CASE WHEN BUN IN ('85','87','89') THEN AMT ELSE AMT*-1 END) AMT, 0 ";
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH ";
                    SQL = SQL + ComNum.VBLF + "         WHERE ACTDate >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND ACTDate <= TO_DATE('" + strTDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND PANO = '" + strPANO +"' ";
                    SQL = SQL + ComNum.VBLF + "             AND BUN IN  ('85','87','89','91') ";
                    SQL = SQL + ComNum.VBLF + "             AND PART <> '#' ";
                    SQL = SQL + ComNum.VBLF + "     GROUP BY ACTDATE, DEPTCODE ";

                    //감액금
                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, DEPTCODE, 0, SUM(AMT) AS AMT";  //2017-12-31 add
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH ";
                    SQL = SQL + ComNum.VBLF + "         WHERE ACTDate >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND ACTDate <= TO_DATE('" + strTDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND PANO = '" + strPANO +"' ";
                    
                    if (strGamsabun != "")
                    { 
                      SQL = SQL + ComNum.VBLF + "               AND sunext in ('Y92I','Y92J','Y92K','Y92L','Y92M','Y92A','Y92B','Y92C','Y92D') ";
                    }
                    else
                    { 
                      SQL = SQL + ComNum.VBLF + "               AND 1 = 2 ";
                    }

                    SQL = SQL + ComNum.VBLF + "             AND PART <> '#' ";
                    SQL = SQL + ComNum.VBLF + "     GROUP BY ACTDATE, DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";

                    //종합건비
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(A.SUDATE,'YYYY-MM-DD') ACTDATE, 'TO', SUM(BONINAMT) AMT, 0 ";    //kyo 20170116
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "HEA_SUNAP A, " + ComNum.DB_PMPA + "HIC_PATIENT B, " + ComNum.DB_PMPA + "HEA_JEPSU C ";
                    SQL = SQL + ComNum.VBLF + "         WHERE A.SUDATE >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND A.SUDATE <= TO_DATE('" + strTDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND A.BONINAMT <> 0 ";
                    SQL = SQL + ComNum.VBLF + "             AND A.PANO = B.PANO ";
                    SQL = SQL + ComNum.VBLF + "             AND A.WRTNO = C.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "             AND C.GBSTS NOT IN ('0') ";
                    SQL = SQL + ComNum.VBLF + "             AND B.PTNO ='" + strPANO +"' ";
                    SQL = SQL + ComNum.VBLF + "     GROUP BY A.SUDATE ";
                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";

                    //일반건진
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(A.SUDATE,'YYYY-MM-DD') ACTDATE, 'HR', SUM(BONINAMT) AMT, 0 ";
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "HIC_SUNAP A, " + ComNum.DB_PMPA + "HIC_PATIENT B ";
                    SQL = SQL + ComNum.VBLF + "         WHERE A.SUDATE >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND A.SUDATE <= TO_DATE('" + strTDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND A.BONINAMT <> 0 ";
                    SQL = SQL + ComNum.VBLF + "             AND A.PANO = B.PANO ";
                    SQL = SQL + ComNum.VBLF + "             AND B.PTNO ='" + strPANO +"' ";
                    SQL = SQL + ComNum.VBLF + "     GROUP BY A.SUDATE ";
                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";

                    //개인미수입금은 진료과 없음.강제로 'MI'로 처리함.
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(BDATE,'YYYY-MM-DD') SDATE, 'MI', SUM(AMT) AMT, 0 ";
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP ";
                    SQL = SQL + ComNum.VBLF + "         WHERE BDate >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND BDate <= TO_DATE('" + strTDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND GUBUN1 = '2' ";
                    SQL = SQL + ComNum.VBLF + "             AND (POBUN  IS NULL OR POBUN = '') ";
                    SQL = SQL + ComNum.VBLF + "             AND PANO  = '" + strPANO +"' ";
                    SQL = SQL + ComNum.VBLF + "     GROUP BY BDATE ";
                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";
                    //기타수납
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, 'R7', SUM(AMT) AMT ,  0 ";
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "OPD_ETCSLIP ";
                    SQL = SQL + ComNum.VBLF + "         WHERE ACTDATE >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND ACTDATE <= TO_DATE('" + strTDate +"','YYYY-MM-DD') ";
                    //SQL = SQL + ComNum.VBLF + "             AND JUMIN1 = '" + strJumin1 + "' ";
                    //SQL = SQL + ComNum.VBLF + "             AND JUMIN3 = '" + clsAES.AES(strJumin2) + "' ";
                    SQL = SQL + ComNum.VBLF + "             AND PANO  = '" + strPANO + "' ";
                    SQL = SQL + ComNum.VBLF + "     GROUP BY ACTDATE ";
                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";

                    //2009-12-09 윤조연 산전지원금 금액
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         TO_CHAR(ACTDATE,'YYYY-MM-DD') AS ACTDATE, 'PX', SUM(DECODE(TRANHEADER,'1',OGAMT * -1,'2',OGAMT)) AS AMT, 0 ";  //산전지원금은 제외해야함으로 -처리함.
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "CARD_APPROV ";
                    SQL = SQL + ComNum.VBLF + "         WHERE ACTDATE >= TO_DATE('" + strFDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND ACTDATE <= TO_DATE('" + strTDate +"','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND Pano = '" + strPANO +"' ";
                    SQL = SQL + ComNum.VBLF + "             AND DEPTCODE = 'OG' ";
                    SQL = SQL + ComNum.VBLF + "             AND OGAMT <> 0 ";
                    SQL = SQL + ComNum.VBLF + "     GROUP BY ACTDATE ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    #endregion

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     ACTDATE, DEPTCODE, SUM(BOAMT) AS AMT, SUM(GAMAMT) AS GAMAMT";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "WORK_JENGSAN_NEW ";   //2017-12-31 add
                    SQL = SQL + ComNum.VBLF + "GROUP BY ACTDATE, DEPTCODE";
                    SQL = SQL + ComNum.VBLF + "ORDER BY ACTDATE ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                        for (k = 0; k < dt.Rows.Count; k++)
                        {
                            dblAmt = VB.Val(dt.Rows[k]["AMT"].ToString().Trim());
                            dblGamAmt = VB.Val(dt.Rows[k]["GAMAMT"].ToString().Trim());
                            strDept = dt.Rows[k]["DEPTCODE"].ToString().Trim();
                            strActDate = dt.Rows[k]["ACTDATE"].ToString().Trim();

                            if (strJumin2 == "3000000" || strJumin2 == "4000000" || strJumin2 == "3100000" || strJumin2 == "1000000" || strJumin2 == "2000000" || strJumin2 == "1100000" || strJumin2 == "2100000" || strJumin2 == "0000000" || strJumin2 == "5000000" || strJumin2 == "3000001" || strJumin2 == "3000002" || strJumin2 == "3000003" || strJumin2 == "3000004" || strJumin2 == "4000001" || strJumin2 == "4000002" || strJumin2 == "4000003" || strJumin2 == "4000004" || strJumin2 == "4400000" )
                            {
                                strBABY = "Y";
                            }
                            else
                            {
                                strBABY = "";
                            }

                            if (dblGamAmt != 0 && strGamsabun == "") { dblGamAmt = 0; }
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_JUNSLIP";
                            SQL = SQL + ComNum.VBLF + "     (YEAR, PANO, SEQNO, EDINO, SNAME, JUMIN, JUMIN_new, ACTDATE, AMT, DEPTCODE, GAMAMT, GAMSABUN ,BABY)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";     //2017-12-31 add GAMAMT
                            SQL = SQL + ComNum.VBLF + "         '" + strYear + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strPANO + "', ";
                            SQL = SQL + ComNum.VBLF + "         " + dblSeqNo + ", ";
                            SQL = SQL + ComNum.VBLF + "         '" + strEDI + "', ";
                            //2014-12-02 연말정산시에는 주민번호 풀로 남기고 집계작업이 끝나면 일괄 암호화 처리 함
                            SQL = SQL + ComNum.VBLF + "         '" + strSname + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strJumin1 + VB.Left(strJumin2, 1) + "******',";
                            SQL = SQL + ComNum.VBLF + "         '" + clsAES.AES(strJumin1 + strJumin2) + "', ";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strActDate + "', 'YYYY-MM-DD'), ";
                            SQL = SQL + ComNum.VBLF + "         " + dblAmt + ", ";
                            SQL = SQL + ComNum.VBLF + "         '" + strDept + "', ";
                            SQL = SQL + ComNum.VBLF + "         " + dblGamAmt + ", ";
                            SQL = SQL + ComNum.VBLF + "         '" + strGamsabun + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strBABY + "' ";
                            SQL = SQL + ComNum.VBLF + "     )";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }

                        SQL = "";
                        SQL = "DELETE WORK_JENGSAN_NEW";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                strETime = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");
                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("자료형성 완료됨..." + ComNum.VBLF + "시작시간:" + strSTime + " 종료시간:" + strETime + ")");
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

        private void btnFile_Click(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;

            string strFDate = "";
            string strTDate = "";
            string strSeq = "";
            string strFile = "";
            string strFileName = "";
            string strEDIno = "";
            string strSname = "";
            string strJumin = "";
            string strAmt = "";
            string strActDate = "";
            string strTaxNo = "";
            string strTaxName = "";
            string strOwner = "";
            string strTel = "";
            string strBDate = "";
            string strBalBuse = "";
            string strBalName = "";
            string strBalTel = "";
            string strYear = "";
            string strChk = "";
            string strChaSu = "";
            //string strSEQNO = "";
            string strYYMM = "";
            string strBuildDate = "";
            string strSTime = "";
            string strETime = "";

            double dblAmt = 0;
            double dblTotCnt = 0;
            double dblTotAmt = 0;
            //double dblTotamt2 = 0;

            if (dtpYY.Value.Year.ToString() != "2020") { ComFunc.MsgBox("2020 선택하세요."); return; }

            strBuildDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "");

            strFDate = "2020-01-01";
            strTDate = "2020-12-31";
            strYYMM = "2020";
            strYear = "2020";

            if (strYear != "2020")
            {
                ComFunc.MsgBox("작업년도를 다시 선택하십시오.");
                return;
            }

            strTaxNo = lblBal0.Text;
            strTaxName = lblBal1.Text;
            strEDIno = lblBal3.Text;
            strOwner = lblBal2.Text;
            strTel = lblBal4.Text;

            strBalBuse = txtBuse.Text.Trim();
            strBalName = txtName.Text.Trim();
            strBalTel = txtTel.Text.Trim();
            strBDate = Convert.ToDateTime(txtBDate.Text.Trim()).ToString("yyyyMMdd");

            dblTotAmt = 0;

            if (strFDate == "" && strTDate == "")
            {
                ComFunc.MsgBox("파일작성시 작업일자를 입력해주세요");
                return;
            }

            if (strBalBuse == "" || strBalName == "")
            {
                ComFunc.MsgBox("담당부서 또는 담당자 또는 담당전화 이중에 자료가 없습니다");
                return;
            }

            strChk =  rdoCnt0.Checked == true ? "YES" : rdoCnt1.Checked == true ? "YES" : "NO";

            if (strChk == "NO")
            {
                ComFunc.MsgBox("차수를 클릭하세요");
                return;
            }

            strSTime = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");
            
            strFileName = "";
            strFileName = "C:\\G0003_506820089637100068" + strBuildDate;

            if (rdoCnt0.Checked == true) { strChaSu = "1"; }
            else if (rdoCnt1.Checked == true) { strChaSu = "2"; }

            strFileName = strFileName + ".D001";

            if (strFileName.Length < 14)
            {
                ComFunc.MsgBox("차수를 클릭하세요.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            Stream stream = new FileStream(strFileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.Default);

            try
            {
                SQL = "";
                SQL = "select";
                SQL = SQL + ComNum.VBLF + "     SUM(AMT) AS AMT";
                SQL = SQL + ComNum.VBLF + "FROM";
                SQL = SQL + ComNum.VBLF + "     (SELECT";
                SQL = SQL + ComNum.VBLF + "         SUM(AMT + GAMAMT) AS AMT"; //2017-12-31 add GAMAMT
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "ETC_JUNSLIP ";
                SQL = SQL + ComNum.VBLF + "         WHERE ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "             AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "             AND PANO NOT IN (SELECT PANO FROM ETC_JUNREMARK WHERE YEAR = '" + strYear + "') ";
                SQL = SQL + ComNum.VBLF + "             AND SNAME NOT IN ('음주채혈') ";
                SQL = SQL + ComNum.VBLF + "     GROUP BY JUMIN_NEW, ACTDATE ";
                SQL = SQL + ComNum.VBLF + "     HAVING SUM(AMT + GAMAMT) <> 0 )";    //2017-12-31 add GAMAMT

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    sw.Close();
                    sw = null;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    dblTotAmt = VB.Val(dt.Rows[0]["AMT"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     JUMIN_NEW, SUM(AMT + GAMAMT) AS AMT, TO_CHAR(ACTDATE,'YYYYMMDD') AS ACTDATE "; //2017-12-31 add GAMAMT
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_JUNSLIP ";
                SQL = SQL + ComNum.VBLF + "     WHERE ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND SNAME NOT IN ('음주채혈') ";
                SQL = SQL + ComNum.VBLF + "         AND PANO NOT IN (SELECT PANO FROM ETC_JUNREMARK WHERE YEAR  = '" + strYear + "' ) ";
                SQL = SQL + ComNum.VBLF + "GROUP BY JUMIN_NEW, ACTDATE ";
                SQL = SQL + ComNum.VBLF + "HAVING SUM(AMT + GAMAMT) <> 0 "; //2017-12-31 add GAMAMT

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    sw.Close();
                    sw = null;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    dblTotCnt = dt.Rows.Count;
                }

                dt.Dispose();
                dt = null;

                strFile = "";
                strFile = strFile + ComFunc.LeftH(strTaxNo + VB.Space(10), 10);                         //사업자번호     10
                strFile = strFile + ComFunc.LeftH(strTaxName + VB.Space(50), 50);                       //상호           50
                strFile = strFile + ComFunc.LeftH(strEDIno + VB.Space(8), 8);                           //요양기관번호   8
                strFile = strFile + ComFunc.LeftH(strOwner + VB.Space(40), 40);                         //대표자성명     40
                strFile = strFile + ComFunc.LeftH(strTel + VB.Space(20), 20);                           //전화번호       20
                strFile = strFile + ComFunc.LeftH(strBDate + VB.Space(8), 8);                           //제출년월일     8
                strFile = strFile + ComFunc.LeftH(strBalBuse + VB.Space(50), 50);                       //담당부서       50
                strFile = strFile + ComFunc.LeftH(strBalName + VB.Space(40), 40);                       //담당자         40
                strFile = strFile + ComFunc.LeftH(strBalTel + VB.Space(20), 20);                        //연락처         20
                strFile = strFile + ComFunc.LeftH(Convert.ToDateTime(strFDate).ToString("yyyyMMdd") + VB.Space(8), 8);       //수납시작일자   8
                strFile = strFile + ComFunc.LeftH(Convert.ToDateTime(strTDate).ToString("yyyyMMdd") + VB.Space(8), 8);       //수납종료일자   8
                strFile = strFile + ComFunc.LeftH(dblTotCnt.ToString("00000000") + VB.Space(8), 8);      //자료제출건수   8
                strFile = strFile + ComFunc.LeftH(dblTotAmt.ToString("000000000000000") + VB.Space(15), 15);   //자료합계금액   15
                strFile = strFile + "1";                                                                //제출대상자료의 범위(무조건 1)
                strFile = strFile + VB.Space(14);                                                       //공란           14

                

                sw.Write(strFile);
                //sw.Close();

                if (GstrFileChk == "OK")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_JUNMST";
                    SQL = SQL + ComNum.VBLF + "     (YEAR,TAXNO,SANGHO,EDINO,OWNER,TEL,BDATE,BALBUSE,BALNAME,BALTEL,FDATE,TDATE,Cnt,TOTAL, CHASU,YYMM )";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         '" + strYear + "', '" + strTaxNo + "', '" + strTaxName + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEDIno + "', '" + strOwner + "', '" + strTel + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strBDate + "', '" + strBalBuse + "', '" + strBalName + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strBalTel + "', '" + strFDate.Replace("-", "") + "', '" + strTDate.Replace("-", "") + "', ";
                    SQL = SQL + ComNum.VBLF + "         " + dblTotCnt + ", " + dblTotAmt + ", '" + strChaSu + "', '" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    GstrFileChk = "";
                }

                progressBar1.Value = 0;

                SQL = "";
                SQL = "SELECT JUMIN_new, SUM(AMT+GAMAMT) AMT, "; //2017-12-31 add GAMAMT
                SQL = SQL + ComNum.VBLF + "TO_CHAR(ACTDATE,'YYYYMMDD') ACTDATE ";
                SQL = SQL + ComNum.VBLF + "FROM ETC_JUNSLIP ";
                SQL = SQL + ComNum.VBLF + "WHERE ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND SNAME NOT IN ('음주채혈') ";
                SQL = SQL + ComNum.VBLF + "  AND PANO NOT IN (SELECT PANO FROM ETC_JUNREMARK WHERE YEAR  = '" + strYear + "') ";
                SQL = SQL + ComNum.VBLF + "GROUP BY JUMIN_new, ACTDATE  ";
                SQL = SQL + ComNum.VBLF + "HAVING SUM(AMT+GAMAMT) <> 0 ";    //2017-12-31 add GAMAMT
                SQL = SQL + ComNum.VBLF + "ORDER BY JUMIN_new ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    sw.Close();
                    sw = null;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    progressBar1.Maximum = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSeq = (VB.Val(strSeq) + 1).ToString();
                        strSname = "";
                        strJumin = ComFunc.LeftH(clsAES.DeAES(dt.Rows[i]["JUMIN_new"].ToString().Trim()) + VB.Space(13), 13);

                        //계명일경우 같은날 2건 발생할 우려있음 bas_paitent 읽어옴
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     SNAME";
                        SQL = SQL + ComNum.VBLF + "FROM BAS_PATIENT";
                        SQL = SQL + ComNum.VBLF + "     WHERE JUMIN1 = '" + VB.Mid(strJumin, 1, 6) + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND JUMIN3 ='" + clsAES.AES(VB.Mid(strJumin, 7, 7)) + "' ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            sw.Close();
                            sw = null;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            strSname = ComFunc.LeftH(dt1.Rows[0]["SNAME"].ToString().Trim() + VB.Space(40), 40);
                        }

                        dt1.Dispose();
                        dt1 = null;

                        //건진
                        if (strSname == "")
                        {
                            SQL = "";
                            SQL = "SELECT SNAME FROM HIC_PATIENT";
                            SQL = SQL + ComNum.VBLF + "     WHERE JUMIN2 ='" + clsAES.AES(strJumin) + "' ";

                            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                sw.Close();
                                sw = null;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                strSname = ComFunc.LeftH(dt1.Rows[0]["SNAME"].ToString().Trim() + VB.Space(40), 40);
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        //종검
                        if (strSname == "")
                        {
                            SQL = "";
                            SQL = "SELECT SNAME FROM HEA_PATIENT";
                            SQL = SQL + ComNum.VBLF + "     WHERE JUMIN2 ='" + clsAES.AES(strJumin) + "'";

                            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                sw.Close();
                                sw = null;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                strSname = ComFunc.LeftH(dt1.Rows[0]["SNAME"].ToString().Trim() + VB.Space(40), 40);
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        if (strSname == "")
                        {
                            SQL = "";
                            SQL = "SELECT PANO From ETC_JUNSLIP";
                            SQL = SQL + ComNum.VBLF + "     Where JUMIN = '" + strJumin + "' ";

                            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                sw.Close();
                                sw = null;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                SQL = "";
                                SQL = "SELECT JUMIN1, JUMIN3, SNAME FROM BAS_PATIENT";
                                SQL = SQL + ComNum.VBLF + "     WHERE PANO ='" + dt1.Rows[0]["PANO"].ToString().Trim() + "' ";

                                SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    sw.Close();
                                    sw = null;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                if (dt2.Rows.Count > 0)
                                {
                                    strJumin = dt2.Rows[0]["JUMIN1"].ToString().Trim() + clsAES.DeAES(dt2.Rows[0]["JUMIN3"].ToString().Trim());
                                    strSname = dt2.Rows[0]["SNAME"].ToString().Trim();
                                }

                                dt2.Dispose();
                                dt2 = null;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        strSname = ComFunc.LeftH(strSname + VB.Space(40), 40);
                        strActDate = dt.Rows[i]["ACTDATE"].ToString().Trim();

                        dblAmt = VB.Val(dt.Rows[i]["AMT"].ToString().Trim());

                        if (dblAmt < 0)
                        {
                            strAmt = dblAmt.ToString("00000000000000");
                        }
                        else
                        {
                            strAmt = dblAmt.ToString("000000000000000");
                        }

                        strFile = VB.Val(strSeq).ToString("00000000") + strEDIno + strSname + strJumin + strActDate + strAmt + VB.Space(8);

                        //Stream stream1 = new FileStream(strFileName, FileMode.Append);
                        //StreamWriter sw1 = new StreamWriter(stream1, System.Text.Encoding.Default);

                        sw.Write(strFile);
                        

                        progressBar1.Value++;
                        lblCnt.Text = i + "/" + progressBar1.Maximum;
                    }
                }
                sw.Close();
                sw = null;

                dt.Dispose();
                dt = null;

                strETime = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");

                ComFunc.MsgBox("디스켓 작성이 완료 되었습니다" + ComNum.VBLF + "시작시간:" + strSTime + " 종료시간:" + strETime);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                sw.Close();
                sw = null;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnFileNew_Click(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strYear = "";
            string strChaSu = "";
            string strYYMM = "";

            if (dtpYY.Value.Year != 2020) { ComFunc.MsgBox("2020년도 선택하세요."); return; }

            strYYMM = "2020";
            strYear = "2020";

            if (strYear != "2020")
            {
                ComFunc.MsgBox("작업년도를 다시 선택하십시오.");
                return;
            }

            strChaSu = (rdoCnt0.Checked == true ? "1" : "2");

            GstrFileChk = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT YEAR FROM ETC_JUNMST ";
                SQL = SQL + ComNum.VBLF + " WHERE YEAR = '" + strYear + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CHASU = '" + strChaSu + "' ";
                SQL = SQL + ComNum.VBLF + "   AND YYMM  = '" + strYYMM + "' ";

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
                    ComFunc.MsgBox("파일작업한 자료입니다. 다시 파일형성 할 수 없습니다.");
                    return;
                }
                
                dt.Dispose();
                dt = null;

                if (ComFunc.MsgBoxQ("최종 파일을 작성하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                GstrFileChk = "OK";

                btnFile_Click(btnFile, new EventArgs());

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

        private void txtPano_Click(object sender, EventArgs e)
        {
            txtPano.Text = "";

            ssView4_Sheet1.Cells[0, 0].Text = "";
            ssView4_Sheet1.Cells[0, 1].Text = "";

            ssView2_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 20;
            ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            ssView3_Sheet1.RowCount = 0;
            ssView3_Sheet1.RowCount = 20;
            ssView3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            GetCancel();
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            if (txtPano.Text.Trim() == "")
            {
                return;
            }

            if (VB.IsNumeric(txtPano.Text) == false)
            {
                frmPmpaViewSname2 frm = new frmPmpaViewSname2("1", txtPano.Text, "");
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();

                txtPano.Text = clsPmpaPb.GstrPANO;
                ssView4_Sheet1.Cells[0, 0].Text = clsPmpaPb.GstrName;
                ssView4_Sheet1.Cells[0, 1].Text = clsPmpaPb.GstrJumin1 + "-" + clsPmpaPb.GstrJumin2;

                btnSearch.Enabled = true;

                btnSearch_Click(btnSearch, new EventArgs());
            }
            else
            {
                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

                DataTable dt = null;
                string SQL = "";    //Query문
                string SqlErr = ""; //에러문 받는 변수
                SQL = " SELECT PANO, JUMIN1, JUMIN3, SNAME ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + txtPano.Text + "' ";
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

                clsPmpaPb.GstrName = dt.Rows[0]["SNAME"].ToString().Trim();
                clsPmpaPb.GstrJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                clsPmpaPb.GstrJumin2 = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());

                ssView4_Sheet1.Cells[0, 0].Text = clsPmpaPb.GstrName;
                ssView4_Sheet1.Cells[0, 1].Text = clsPmpaPb.GstrJumin1 + "-" + clsPmpaPb.GstrJumin2;
                dt.Dispose();
                dt = null;

                btnSearch.Enabled = true;

                btnSearch_Click(btnSearch, new EventArgs());
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                btnSearch.Focus(); 
                //GetDataPatList();
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
            frmPmpaViewSname2 frm = new frmPmpaViewSname2("2", "", txtPano.Text);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();

            txtPano.Text = clsPmpaPb.GstrPANO;
            ssView4_Sheet1.Cells[0, 0].Text = clsPmpaPb.GstrName;
            ssView4_Sheet1.Cells[0, 1].Text = clsPmpaPb.GstrJumin1 + "-" + clsPmpaPb.GstrJumin2;

            btnSearch.Enabled = true;

            btnSearch_Click(btnSearch, new EventArgs());
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetDataPatList();
        }

        private void GetDataPatList()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strYear = "";
            string strPANO = "";
            string strJumin = "";

            double dblTotAmt = 0;

            strPANO = txtPano.Text.Trim();
            strYear = dtpYear.Value.Year.ToString();
            strJumin = ssView4_Sheet1.Cells[0, 1].Text.Trim();
            strJumin = VB.Left(strJumin, 6) + VB.Right(strJumin, 7);
            dblTotAmt = 0;

            if (strJumin == "--") { return; }

            ssView2_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(ACTDATE,'YYYY-MM-DD') AS ACTDATE, ";
                SQL = SQL + ComNum.VBLF + "     SUM(AMT+GAMAMT) AS AMT, DEPTCODE, BPano "; //2017-12-31 add GAMAMT
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_JUNSLIP ";
                SQL = SQL + ComNum.VBLF + "     WHERE YEAR = '" + strYear + "' ";
                SQL = SQL + ComNum.VBLF + "       AND PANO = '" + strPANO + "' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY ACTDATE, DEPTCODE,BPano ";
                SQL = SQL + ComNum.VBLF + "ORDER BY ACTDATE ";

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

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 1].Text = VB.Val(dt.Rows[i]["AMT"].ToString().Trim()).ToString("###,###,###,###");
                        ssView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["BPANO"].ToString().Trim();

                        dblTotAmt += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                ssView2_Sheet1.RowCount = ssView2_Sheet1.RowCount + 1;
                ssView2_Sheet1.SetRowHeight(ssView2_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 0].Text = "합  계";
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 1].Text = dblTotAmt.ToString("###,###,###,###");

                btnSearch.Enabled = false;

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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            GetCancel();
        }

        private void GetCancel()
        {
            Clear();

            ssView2_Sheet1.RowCount = 0;
            ssView3_Sheet1.RowCount = 0;

            ssView4_Sheet1.Cells[0, 0].Text = "";
            ssView4_Sheet1.Cells[0, 1].Text = "";

            txtPano.Text = "";
            txtPano.Focus();

            btnSearch.Enabled = true;
        }

        private void ssView2_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != 4) { return; }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;

            string strPANO = "";
            string strActDate = "";
            string strJumin = "";
            string strDept = "";

            double dblTot = 0;

            ssView3_Sheet1.RowCount = 0;
            
            strActDate = ssView2_Sheet1.Cells[e.Row, 0].Text.Trim();
            if (VB.IsDate(strActDate) == false)
            {
                return;
            }
            strDept = ssView2_Sheet1.Cells[e.Row, 2].Text.Trim();

            strPANO = txtPano.Text.Trim();

            strJumin = ssView4_Sheet1.Cells[0, 1].Text.Trim();
            strJumin = VB.Left(strJumin, 6) + VB.Right(strJumin, 7);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                #region JengSanView_New2

                SQL = "";
                SQL = " CREATE OR REPLACE VIEW VIEW_JENGSAN_NEW2 ";
                SQL = SQL + ComNum.VBLF + " (ACTDATE, DEPTCODE, AMT, BUN, IO ) AS ";
                SQL = SQL + ComNum.VBLF + "  SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, DEPTCODE, SUM(AMT1+AMT2), '99', 'O' ";
                SQL = SQL + ComNum.VBLF + "    From OPD_SLIP ";
                SQL = SQL + ComNum.VBLF + "   WHERE ACTDate = TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     AND SUNEXT = 'Y99' ";
                SQL = SQL + ComNum.VBLF + "     AND Pano = '" + strPANO +"' ";
                SQL = SQL + ComNum.VBLF + "     AND SEQNO <> -1 ";
                SQL = SQL + ComNum.VBLF + "     AND PART <> '#' ";
                SQL = SQL + ComNum.VBLF + "     AND (GBPOSCO IS NULL OR GBPOSCO <> 'N') ";
                SQL = SQL + ComNum.VBLF + "   GROUP BY ACTDATE, DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "   HAVING SUM(AMT1+AMT2) <> 0 ";
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + "  SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, DEPTCODE, SUM(AMT1+AMT2), '92', 'O' ";
                SQL = SQL + ComNum.VBLF + "    From OPD_SLIP ";
                SQL = SQL + ComNum.VBLF + "   WHERE ACTDate = TO_DATE('" + strActDate +"','YYYY-MM-DD') ";

                if (clsVbfunc.READ_Gamek_infoSabun(strJumin) != "")
                {
                    SQL = SQL + ComNum.VBLF + "     AND sunext in ('Y92I','Y92J','Y92K','Y92L','Y92M','Y92A','Y92B','Y92C','Y92D') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND 1=2 ";
                }

                SQL = SQL + ComNum.VBLF + "     AND Pano = '" + strPANO +"' ";
               // SQL = SQL + ComNum.VBLF + "     AND SEQNO <> -1 ";
               // SQL = SQL + ComNum.VBLF + "     AND PART <> '#' ";
                SQL = SQL + ComNum.VBLF + "     AND (GBPOSCO IS NULL OR GBPOSCO <> 'N') ";
                SQL = SQL + ComNum.VBLF + "   GROUP BY ACTDATE, DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "   HAVING SUM(AMT1+AMT2) <> 0 ";
                SQL = SQL + ComNum.VBLF + " UNION ALL ";

                //예약금
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(DATE1,'YYYY-MM-DD') ACTDATE, DEPTCODE, SUM(AMT7) ,'99','RR' ";
                SQL = SQL + ComNum.VBLF + "  FROM OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPANO +"' ";
                SQL = SQL + ComNum.VBLF + "   AND Date1 >= TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DATE1 <= TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY DATE1, DEPTCODE ";
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                //예약금
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(DATE1,'YYYY-MM-DD') ACTDATE, DEPTCODE, SUM(AMT5) ,'99','RR' ";
                SQL = SQL + ComNum.VBLF + "  FROM OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPANO +"' ";
                SQL = SQL + ComNum.VBLF + "   AND Date1 >= TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DATE1 <= TO_DATE('" + strActDate +"','YYYY-MM-DD') ";

                if (clsVbfunc.READ_Gamek_infoSabun(strJumin) != "")
                {
                    SQL = SQL + ComNum.VBLF + "     AND gbgamek  in ('21','22','23','24','25' ,'11','12','13','14','15') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND 1=2 ";
                }

                SQL = SQL + ComNum.VBLF + " GROUP BY DATE1, DEPTCODE ";
                SQL = SQL + ComNum.VBLF + " UNION ALL ";

                //예약금 환불
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(RETDATE,'YYYY-MM-DD') ACTDATE, DEPTCODE, SUM(RETAMT) ,'99','RX'    ";
                SQL = SQL + ComNum.VBLF + "  FROM OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPANO +"' ";
                SQL = SQL + ComNum.VBLF + "   AND RETDATE >= TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND RETDATE < TO_DATE('" + Convert.ToDateTime(strActDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND RETDATE IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + " GROUP BY RETDATE, DEPTCODE, BI ";
                SQL = SQL + ComNum.VBLF + " UNION ALL  ";

                //예약금 환불
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(RETDATE,'YYYY-MM-DD') ACTDATE, DEPTCODE, SUM(AMT5) * -1 ,'99','RX'    ";
                SQL = SQL + ComNum.VBLF + "  FROM OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPANO +"' ";
                SQL = SQL + ComNum.VBLF + "   AND RETDATE >= TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND RETDATE < TO_DATE('" + Convert.ToDateTime(strActDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (clsVbfunc.READ_Gamek_infoSabun(strJumin) != "")
                {
                    SQL = SQL + ComNum.VBLF + "     AND gbgamek  in ('21','22','23','24','25' ,'11','12','13','14','15') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND 1=2 ";
                }

                SQL = SQL + ComNum.VBLF + " GROUP BY RETDATE, DEPTCODE, BI ";
                SQL = SQL + ComNum.VBLF + " UNION ALL  ";

                //--------------------------------------------------------------------------------------------------
                //예약검사
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, DEPTCODE, SUM(AMT6) ,'99','RE'     ";
                SQL = SQL + ComNum.VBLF + "  FROM OPD_RESERVED_EXAM ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPANO +"' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY ACTDATE, DEPTCODE ";
                SQL = SQL + ComNum.VBLF + " UNION ALL ";

                //예약검사 환불
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(RETDATE,'YYYY-MM-DD') ACTDATE, DEPTCODE, SUM(RETAMT) ,'99','RE'    ";
                SQL = SQL + ComNum.VBLF + "  FROM OPD_RESERVED_EXAM ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPANO +"' ";
                SQL = SQL + ComNum.VBLF + "   AND RETDATE >= TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND RETDATE < TO_DATE('" + Convert.ToDateTime(strActDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND RETDATE IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + " GROUP BY RETDATE, DEPTCODE, BI ";
                SQL = SQL + ComNum.VBLF + " UNION ALL  ";

                //예약검사 대체
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(TRANSDATE,'YYYY-MM-DD') ACTDATE, DEPTCODE, SUM(TRANSAMT) * -1 ,'99','RD'    ";
                SQL = SQL + ComNum.VBLF + "  FROM OPD_RESERVED_EXAM ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPANO +"' ";
                SQL = SQL + ComNum.VBLF + "   AND TRANSDATE >= TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND TRANSDATE < TO_DATE('" + Convert.ToDateTime(strActDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND TRANSDATE IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + " GROUP BY TRANSDATE, DEPTCODE, BI ";

                SQL = SQL + ComNum.VBLF + " UNION ALL  ";
                //--------------------------------------------------------------------------------------------------
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, DEPTCODE, ";
                SQL = SQL + ComNum.VBLF + "        SUM(CASE WHEN BUN IN ('85','87','89') THEN AMT ELSE AMT*-1 END) AMT, ";
                SQL = SQL + ComNum.VBLF + "        BUN, 'I' ";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_CASH ";
                SQL = SQL + ComNum.VBLF + "     WHERE ACTDate = TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "       AND PANO = '" + strPANO +"' ";
                SQL = SQL + ComNum.VBLF + "    AND BUN IN  ('85','87','89','91') ";
                SQL = SQL + ComNum.VBLF + "    AND PART <> '#' ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY ACTDATE, DEPTCODE, BUN ";

                SQL = SQL + ComNum.VBLF + " UNION ALL ";

                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, DEPTCODE, ";    //2017-12-31 add
                SQL = SQL + ComNum.VBLF + "        SUM( AMT ) AMT, ";
                SQL = SQL + ComNum.VBLF + "        BUN, 'I' ";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_CASH ";
                SQL = SQL + ComNum.VBLF + "     WHERE ACTDate = TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "       AND PANO = '" + strPANO +"' ";

                if (clsVbfunc.READ_Gamek_infoSabun(strJumin) != "")
                {
                    SQL = SQL + ComNum.VBLF + "     AND sunext in ('Y92I','Y92J','Y92K','Y92L','Y92M','Y92A','Y92B','Y92C','Y92D') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND 1=2 ";
                }

                SQL = SQL + ComNum.VBLF + "    AND PART <> '#' ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY ACTDATE, DEPTCODE, BUN ";
                SQL = SQL + ComNum.VBLF + " UNION ALL ";

                //종합건비
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(A.SUDATE,'YYYY-MM-DD') ACTDATE, 'TO', SUM(BONINAMT) AMT, '99', 'O' ";   //kyo 20170116
                SQL = SQL + ComNum.VBLF + " FROM HEA_SUNAP A, HIC_PATIENT B, HEA_JEPSU C ";
                SQL = SQL + ComNum.VBLF + " WHERE A.SUDATE = TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.BONINAMT <> 0 ";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO ";
                SQL = SQL + ComNum.VBLF + "   AND B.PTNO ='" + strPANO +"' ";
                SQL = SQL + ComNum.VBLF + "   AND A.WRTNO=C.WRTNO(+) ";
                SQL = SQL + ComNum.VBLF + "   AND C.GBSTS NOT IN ('0') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY A.SUDATE ";
                SQL = SQL + ComNum.VBLF + " UNION ALL ";

                //일반건진
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(A.SUDATE,'YYYY-MM-DD') ACTDATE, 'HR', SUM(BONINAMT) AMT, '99', 'O' ";
                SQL = SQL + ComNum.VBLF + " FROM HIC_SUNAP A, HIC_PATIENT B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.SUDATE = TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.BONINAMT <> 0 ";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO ";
                SQL = SQL + ComNum.VBLF + "   AND B.PTNO ='" + strPANO +"' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY A.SUDATE ";
                SQL = SQL + ComNum.VBLF + " UNION ALL ";

                //개인미수입금은 진료과 없음.강제로 'MI'로 처리함.
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') SDATE, 'MI', ";
                SQL = SQL + ComNum.VBLF + " SUM(AMT) AMT,'99','O' ";
                SQL = SQL + ComNum.VBLF + " FROM MISU_GAINSLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE BDate = TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN1 = '2' ";
                SQL = SQL + ComNum.VBLF + "   AND PANO  = '" + strPANO +"' ";
                SQL = SQL + ComNum.VBLF + "   AND (POBUN  IS NULL OR POBUN = '') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY BDATE ";
                SQL = SQL + ComNum.VBLF + " UNION ALL ";

                //기타수납
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, 'R7', SUM(AMT) AMT,'99', 'O' ";
                SQL = SQL + ComNum.VBLF + "   FROM OPD_ETCSLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND JUMIN1= '" + VB.Left(strJumin, 6) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND JUMIN3= '" + clsAES.AES(VB.Right(strJumin, 7)) +"' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY ACTDATE ";

                SQL = SQL + ComNum.VBLF + " UNION ALL ";

                //2009-12-09 윤조연 산전지원금 금액
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, 'PX', SUM(DECODE(TRANHEADER,'1',OGAMT * -1,'2',OGAMT) ) AMT ,'99','PX' ";
                SQL = SQL + ComNum.VBLF + "   FROM CARD_APPROV ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + strActDate +"','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND Pano = '" + strPANO +"' ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = 'OG' ";
                SQL = SQL + ComNum.VBLF + "   AND OGAMT <> 0 ";
                SQL = SQL + ComNum.VBLF + " GROUP BY ACTDATE ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                #endregion

                SQL = "";
                SQL = "SELECT ACTDATE, AMT, DEPTCODE, BUN, IO ";
                SQL = SQL + ComNum.VBLF + "FROM VIEW_JENGSAN_NEW2 ";
                SQL = SQL + ComNum.VBLF + "WHERE DEPTCODE = '" + strDept + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY ACTDATE ";

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
                        ssView3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                        ssView3_Sheet1.Cells[i, 1].Text = VB.Val(dt.Rows[i]["AMT"].ToString().Trim()).ToString("###,###,###");
                        ssView3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView3_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IO"].ToString().Trim();

                        switch (dt.Rows[i]["BUN"].ToString().Trim())
                        {
                            case "85": ssView3_Sheet1.Cells[i, 4].Text = "보증금(입원)"; break;
                            case "87": ssView3_Sheet1.Cells[i, 4].Text = "중간납(입원)"; break;
                            case "91": ssView3_Sheet1.Cells[i, 4].Text = "환불금(입원)"; break;
                            case "89": ssView3_Sheet1.Cells[i, 4].Text = "퇴원금(입원)"; break;
                            case "99":
                                switch (dt.Rows[i]["DEPTCODE"].ToString().Trim())
                                {
                                    case "MI": ssView3_Sheet1.Cells[i, 4].Text = "미수입금"; break;
                                    case "TO": ssView3_Sheet1.Cells[i, 4].Text = "종합검진"; break;
                                    case "HR": ssView3_Sheet1.Cells[i, 4].Text = "일반검진"; break;
                                    case "R7": ssView3_Sheet1.Cells[i, 4].Text = "기타수납(외래)"; break;
                                    case "PX": ssView3_Sheet1.Cells[i, 4].Text = "산전지원금"; break;
                                    default:
                                        ssView3_Sheet1.Cells[i, 4].Text = "수납(외래)";

                                        switch (dt.Rows[i]["IO"].ToString().Trim())
                                        {
                                            case "RR": ssView3_Sheet1.Cells[i, 4].Text = "외래예약금"; break;
                                            case "RX": ssView3_Sheet1.Cells[i, 4].Text = "외래환불금"; break;
                                            case "RE": ssView3_Sheet1.Cells[i, 4].Text = "예약검사수납+환불"; break;
                                            case "RD": ssView3_Sheet1.Cells[i, 4].Text = "예약검사대체"; break;
                                        }
                                        break;
                                }
                                break;
                        }

                        dblTot += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                    }

                    SQL = "";
                    SQL = "DROP VIEW VIEW_JENGSAN_NEW2";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                dt.Dispose();
                dt = null;

                ssView3_Sheet1.RowCount = ssView3_Sheet1.RowCount + 1;
                ssView3_Sheet1.SetRowHeight(ssView3_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                ssView3_Sheet1.Cells[ssView3_Sheet1.RowCount - 1, 0].Text = "합    계";
                ssView3_Sheet1.Cells[ssView3_Sheet1.RowCount - 1, 1].Text = dblTot.ToString("###,###,###,###");

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
