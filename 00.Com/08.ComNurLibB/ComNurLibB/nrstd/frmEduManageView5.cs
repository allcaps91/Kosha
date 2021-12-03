using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmEduManageView5.cs
    /// Description     : 개인별교육상세조회
    /// Author          : 박창욱
    /// Create Date     : 2018-01-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrstd\Frm교육관리조회5.frm(Frm교육관리조회5.frm) >> frmEduManageView5.cs 폼이름 재정의" />	
    public partial class frmEduManageView5 : Form
    {
        string GstrRetValue = "";
        string FstrSort = "";

        public frmEduManageView5()
        {
            InitializeComponent();
        }

        public frmEduManageView5(string strRetValue)
        {
            InitializeComponent();
            this.GstrRetValue = strRetValue;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strROWID = "";
            string strTIME = "";
            string strEDUNAME = "";
            string strEduDate = "";
            string strSabun = "";

            strSabun = VB.Pstr(GstrRetValue, "^^", 1);
            strSabun = VB.Val(strSabun).ToString("00000");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 1; i < ssView_Sheet1.RowCount - 1; i++)
                {
                    if (ssView_Sheet1.Cells[i - 1, 10].Text.Trim() == "KOSMOS_ADM.INSA_MSTF")
                    {
                        strROWID = ssView_Sheet1.Cells[i - 1, 11].Text.Trim();
                        strTIME = ssView_Sheet1.Cells[i - 1, 5].Text.Trim();
                        strEDUNAME = ssView_Sheet1.Cells[i - 1, 3].Text.Trim();
                        strEduDate = ssView_Sheet1.Cells[i - 1, 4].Text.Trim();

                        SQL = "";
                        SQL = " UPDATE " + ComNum.DB_ERP + "INSA_MSTF SET ";
                        SQL = SQL + ComNum.VBLF + "  EDUTIME = '" + strTIME + "'";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                        }

                        SQL = "";
                        SQL = " INSERT INTO " + ComNum.DB_ERP + "INSA_MSTF_LOG(";
                        SQL = SQL + ComNum.VBLF + " WRITEDATE, WRITESABUN, EDUNAME, EDUDATE, ";
                        SQL = SQL + ComNum.VBLF + " EDUTIME, SABUN) VALUES (";
                        SQL = SQL + ComNum.VBLF + " SYSDATE, " + clsType.User.Sabun + ", '" + strEDUNAME + " ','" + strEduDate + "',";
                        SQL = SQL + ComNum.VBLF + "'" + strTIME + "','" + strSabun + "')";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEduManageView5_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SS_Display(GstrRetValue);
        }

        void SS_Display(string argSabun)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nSum = 0;
            string strEduTime = "";
            string strSabun = "";
            string strSDate = "";
            string strEDate = "";

            if (FstrSort == "ASC")
            {
                FstrSort = "DESC";
            }
            else
            {
                FstrSort = "ASC";
            }

            strSabun = VB.Pstr(argSabun, "^^", 1);
            strSabun = VB.Val(strSabun).ToString("00000");

            strSDate = VB.Pstr(argSabun, "^^", 2).Trim();
            strEDate = VB.Pstr(argSabun, "^^", 3).Trim();

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT A.BUCODE, SABUN, GUBUN, EDUNAME, FRDATE, TODATE, EDUTIME, PLACE, JUMSU, GIKWAN, MAN, COM, B.NAME, TBLNM, ROWID1";
                SQL = SQL + ComNum.VBLF + " FROM (";
                SQL = SQL + ComNum.VBLF + " SELECT BUCODE, SABUN, '간호부교육' GUBUN, EDUNAME, FRDATE, TODATE,translate(EDUTIME, '0123456789' ||EDUTIME , '0123456789')  EDUTIME, ";
                SQL = SQL + ComNum.VBLF + " PLACE, JUMSU, '' GIKWAN, MAN, '' COM, '" + ComNum.DB_PMPA + "NUR_EDU_MST' TBLNM, ROWID ROWID1";
                SQL = SQL + ComNum.VBLF + " From " + ComNum.DB_PMPA + "NUR_EDU_MST";
                SQL = SQL + ComNum.VBLF + " WHERE (SABUN = '" + strSabun + "' OR SABUN = ' " + strSabun + "')";
                SQL = SQL + ComNum.VBLF + "   AND FRDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND FRDATE <= TO_DATE('" + strEDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + " Union All";
                SQL = SQL + ComNum.VBLF + " SELECT A.BUSE, A.SABUN, '사이버교육' GUBUN, NVL(B.EDUTITLE, GRADE ) EDUTITLE, A.SDATE, A.EDATE,to_char(to_number(translate(A.EDUTIME, '0123456789' ||A.EDUTIME , '0123456789'))*60)   EDUTIME, ";
                SQL = SQL + ComNum.VBLF + " EDUHOMEPAGE, '' JUMSU, '' GIKWAN,  '' MAN, A.COMPLETION, '" + ComNum.DB_PMPA + "REPORT_EDU' TBLNM, A.ROWID ROWID1";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "REPORT_EDU A, " + ComNum.DB_PMPA + "REPORT_EDU_CODE B";
                SQL = SQL + ComNum.VBLF + " WHERE A.EDUCODE = B.CODE(+)";
                SQL = SQL + ComNum.VBLF + "   AND A.SABUN = '" + strSabun + "'";
                SQL = SQL + ComNum.VBLF + "   AND CANDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "   AND A.SDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.SDATE <= TO_DATE('" + strEDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + " Union All";
                SQL = SQL + ComNum.VBLF + " SELECT B.BUSE, A.SABUN, '필수교육', NAEYONG, FDATE, TDATE, translate(EDUTIME, '0123456789' ||EDUTIME , '0123456789') TIMELESS, ";
                SQL = SQL + ComNum.VBLF + " JANGSO PLACE, PUNGGA JUMSU, GIKWAN, EDUMAN, '' COM, '" + ComNum.DB_ERP+ "INSA_MSTW' TBLNM, A.ROWID ROWID1";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP+ "INSA_MSTW A, " + ComNum.DB_ERP+ "INSA_MST B";
                SQL = SQL + ComNum.VBLF + " Where a.SABUN = b.SABUN";
                SQL = SQL + ComNum.VBLF + "   AND (A.GUBUN = '1' OR A.GUBUN IS NULL)";
                SQL = SQL + ComNum.VBLF + "   AND A.SABUN = '" + strSabun + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.FDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.FDATE <= TO_DATE('" + strEDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + " Union All";
                SQL = SQL + ComNum.VBLF + " SELECT B.BUSE, A.SABUN, '특성화교육', NAEYONG, FDATE, TDATE, translate(EDUTIME, '0123456789' ||EDUTIME , '0123456789') TIMELESS, ";
                SQL = SQL + ComNum.VBLF + " JANGSO PLACE, PUNGGA JUMSU, GIKWAN, EDUMAN, '' COM, '" + ComNum.DB_ERP+ "INSA_MSTW' TBLNM, A.ROWID ROWID1";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP+ "INSA_MSTW A, " + ComNum.DB_ERP+ "INSA_MST B";
                SQL = SQL + ComNum.VBLF + " Where a.SABUN = b.SABUN";
                SQL = SQL + ComNum.VBLF + "   AND A.GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "   AND A.SABUN = '" + strSabun + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.FDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.FDATE <= TO_DATE('" + strEDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + " Union All";
                SQL = SQL + ComNum.VBLF + " SELECT B.BUSE, A.SABUN, '인증제직무교육', NAEYONG, FDATE, TDATE,  translate(EDUTIME, '0123456789' ||EDUTIME , '0123456789') TIMELESS, ";
                SQL = SQL + ComNum.VBLF + " JANGSO PLACE, PUNGGA JUMSU, GIKWAN, EDUMAN, '' COM, '" + ComNum.DB_ERP+ "INSA_MSTW' TBLNM, A.ROWID ROWID1";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP+ "INSA_MSTW A, " + ComNum.DB_ERP+ "INSA_MST B";
                SQL = SQL + ComNum.VBLF + " Where a.SABUN = b.SABUN";
                SQL = SQL + ComNum.VBLF + "   AND A.GUBUN = '3'";
                SQL = SQL + ComNum.VBLF + "   AND A.SABUN = '" + strSabun + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.FDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.FDATE <= TO_DATE('" + strEDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + " Union All";
                SQL = SQL + ComNum.VBLF + " SELECT B.BUSE, A.SABUN, '보수교육', NAEYONG, FDATE, TDATE,  translate(EDUTIME, '0123456789' ||EDUTIME , '0123456789') TIMELESS, ";
                SQL = SQL + ComNum.VBLF + " JANGSO PLACE, PUNGGA JUMSU, GIKWAN, EDUMAN, '' COM, '" + ComNum.DB_ERP+ "INSA_MSTW' TBLNM, A.ROWID ROWID1";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP+ "INSA_MSTW A, " + ComNum.DB_ERP+ "INSA_MST B";
                SQL = SQL + ComNum.VBLF + " Where a.SABUN = b.SABUN";
                SQL = SQL + ComNum.VBLF + "   AND A.GUBUN = '4'";
                SQL = SQL + ComNum.VBLF + "   AND A.SABUN = '" + strSabun + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.FDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.FDATE <= TO_DATE('" + strEDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + " Union All";
                SQL = SQL + ComNum.VBLF + " SELECT B.BUSE, A.SABUN, '인사등록교육', NAEYONG, FDATE, TDATE, EDUTIME TIMELESS, ";
                SQL = SQL + ComNum.VBLF + " JANGSO PLACE, '' JUMSU, GIKWAN, '' MAN, '' COM, '" + ComNum.DB_ERP+ "INSA_MSTF' TBLNM, A.ROWID ROWID1";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP+ "INSA_MSTF A, " + ComNum.DB_ERP+ "INSA_MST B";
                SQL = SQL + ComNum.VBLF + " Where a.SABUN = b.SABUN";
                SQL = SQL + ComNum.VBLF + " AND A.SABUN = '" + strSabun + "'";
                SQL = SQL + ComNum.VBLF + " AND A.FDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + " AND A.FDATE <= TO_DATE('" + strEDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + " ) A, " + ComNum.DB_PMPA + "BAS_BUSE B";
                SQL = SQL + ComNum.VBLF + " Where a.BuCode = b.BuCode";
                if (FstrSort == "ASC")
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY FRDATE ASC";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY FRDATE DESC";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count + 1;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                ComFunc cf = new ComFunc();

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = cf.Read_SabunName(clsDB.DbCon, dt.Rows[i]["SABUN"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["EDUNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = VB.Left(dt.Rows[i]["FRDATE"].ToString().Trim(), 10) + " ~ " + VB.Left(dt.Rows[i]["TODATE"].ToString().Trim(), 10);
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["EDUTIME"].ToString().Trim();
                    strEduTime = ssView_Sheet1.Cells[i, 5].Text.Trim();
                    nSum += (int)VB.Val(ssView_Sheet1.Cells[i, 5].Text.Trim());
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["PLACE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["JUMSU"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["MAN"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["COM"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["TBLNM"].ToString().Trim();
                    if (ssView_Sheet1.Cells[i, 10].Text.Trim() == "KOSMOS_ADM.INSA_MSTF")
                    {
                        ssView_Sheet1.Cells[i, 5].BackColor = Color.FromArgb(255, 255, 215);
                        ssView_Sheet1.Cells[i, 5].Text = strEduTime;
                        ssView_Sheet1.Cells[i, 5].Locked = false;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[i, 5].Text = strEduTime;
                        ssView_Sheet1.Cells[i, 5].Locked = true;
                    }
                    ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["ROWID1"].ToString().Trim();
                }

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nSum.ToString();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "개인 교육 목록 [" + VB.Pstr(GstrRetValue, "^^", 2).Trim() + "~" + VB.Pstr(GstrRetValue, "^^", 3).Trim() + "]";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            SS_Display(GstrRetValue);
        }
    }
}
