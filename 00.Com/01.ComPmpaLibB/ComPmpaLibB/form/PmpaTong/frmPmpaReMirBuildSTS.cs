using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpaReMirBuildSTS : Form
    {
        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : 
        /// Description     : 
        /// Author          : 김효성
        /// Create Date     : 2017-09-14
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= d:\psmh\misu\misubs\misubs1.vbp\misubs59.frm(FrmReMirBuild)" >> frmSupLbExSTS15.cs 폼이름 재정의" />

        string FstrYYMM = "";//  '작업년월
        string FstrFDate = "";// '시작일자
        string FstrTDate = "";// '종료일자

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaReMirBuildSTS()
        {
            InitializeComponent();
        }

        private void frmPmpaReMirBuildSTS_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboyyyy, 24, "", "1");
            cboyyyy.SelectedIndex = 1;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            string strJepno = "";
            string strYear = "";
            string strYYMM = "";
            string strJepDate = "";
            string strJohap = "";
            string strIpdOpd = "";
            string strROWID = "";
            double nSakAmt = 0;
            double nReMir = 0;
            double nResultAmt = 0;
            double nSakAmtOUT = 0;
            double nReMirOUT = 0;
            double nResultAmtOUT = 0;
            DataTable dt = null;
            DataTable dtFunc = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            FstrYYMM = VB.Left(cboyyyy.Text, 4) + VB.Mid(cboyyyy.Text, 7, 2);
            FstrFDate = VB.Left(FstrYYMM, 4) + "-" + VB.Right(FstrYYMM, 2) + "-01";
            FstrTDate = CF.READ_LASTDAY(clsDB.DbCon, FstrFDate);

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
             

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'해당월의 접수번호 select ----------------------------------------------------
                SQL = "";
                SQL = " SELECT ";
                SQL += ComNum.VBLF + " YYMM, IPDOPD, TO_CHAR(JEPDATE, 'YYYY-MM-DD') JEPDATE, JEPNO, YEAR, JOHAP , ROWID  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "EDI_JEPSU";
                SQL += ComNum.VBLF + " WHERE 1=1";
                SQL += ComNum.VBLF + "   AND JEPDATE >=TO_DATE('" + FstrFDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND JEPDATE <=TO_DATE('" + FstrTDate + "','YYYY-MM-DD')";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(clsDB.DbCon);
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strJepno = dt.Rows[i]["jepno"].ToString().Trim();
                    strYear = dt.Rows[i]["Year"].ToString().Trim();
                    strYYMM = dt.Rows[i]["YYMM"].ToString().Trim();
                    strJepDate = dt.Rows[i]["JEPDATE"].ToString().Trim();
                    strJohap = dt.Rows[i]["JOHAP"].ToString().Trim();
                    strROWID = dt.Rows[i]["ROWID"].ToString().Trim();
                    strIpdOpd = dt.Rows[i]["IPDOPD"].ToString().Trim();
                    // '삭감액 READ
                    nSakAmt = 0;

                    SQL = "";
                    SQL += ComNum.VBLF + "  SELECT ";
                    SQL += ComNum.VBLF + " SUM(AMT4) SAKAMT ";
                    SQL += ComNum.VBLF + "  FROM  " + ComNum.DB_PMPA + "MISU_IDMST";
                    SQL += ComNum.VBLF + "  WHERE 1=1";
                    SQL += ComNum.VBLF + "    AND MISUID ='" + Convert.ToInt32(strJepno).ToString("00000000") + "'  ";
                    SQL += ComNum.VBLF + "    AND BDATE = TO_DATE('" + strJepDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND CLASS IN ('01','02','03','04') ";

                    SqlErr = clsDB.GetDataTable(ref dtFunc, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    nSakAmt = VB.Val(dtFunc.Rows[0]["SAKAMT"].ToString().Trim());

                    dtFunc.Dispose();
                    dtFunc = null;

                    //'이의신청 및 회수 READ
                    nReMir = 0;
                    nResultAmt = 0;
                    nReMirOUT = 0;

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ";
                    SQL += ComNum.VBLF + " SUM(REAMT1 + REAMT2) REMIR, SUM(RESULTAMT) RESULTAMT ,SUM(REOUTAMT1 + REOUTAMT2) REMIROUT ";
                    SQL += ComNum.VBLF + "  FROM " + ComNum.VBLF + "MIR_REMIRMST";
                    SQL += ComNum.VBLF + " WHERE JEPNO ='" + strJepno + "'";
                    SQL += ComNum.VBLF + "   AND YYMM ='" + strYYMM + "' ";
                    SQL += ComNum.VBLF + "   AND IPDOPD ='" + strIpdOpd + "' ";

                    SqlErr = clsDB.GetDataTable(ref dtFunc, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    nReMir = VB.Val(dtFunc.Rows[0]["REMIR"].ToString().Trim());
                    nResultAmt = VB.Val(dtFunc.Rows[0]["RESULTAMT"].ToString().Trim());
                    nReMirOUT = VB.Val(dtFunc.Rows[0]["REMIROUT"].ToString().Trim());

                    dtFunc.Dispose();
                    dtFunc = null;

                    nSakAmtOUT = 0;

                    //'원외약제비 삭감-----------------------------------------------------------------
                    if (string.Compare(strJepDate, "2012-12-01") >= 0)
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  sum(AMT3) AMT3";
                        SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "EDI_F0902";
                        SQL += ComNum.VBLF + " WHERE JEPNO ='" + strJepno + "'";
                        SQL += ComNum.VBLF + "   AND YEAR = '" + strYear + "' ";
                    }
                    else
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT";
                        SQL = "  sum(AMT5) AMT3";
                        SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "EDI_F0902";
                        SQL += ComNum.VBLF + " WHERE JEPNO ='" + strJepno + "'";
                        SQL += ComNum.VBLF + "   AND YEAR = '" + strYear + "' ";
                    }
                    SqlErr = clsDB.GetDataTable(ref dtFunc, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    nSakAmtOUT = VB.Val(dtFunc.Rows[0]["AMT3"].ToString().Trim());

                    dtFunc.Dispose();
                    dtFunc = null;

                    //'약제비추가 인정----------------------------------------------------------------
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ";
                    SQL += ComNum.VBLF + "SUM(AMT6) AMT6";
                    SQL += ComNum.VBLF + " From " + ComNum.DB_PMPA + "EDI_F0802";
                    SQL += ComNum.VBLF + " WHERE JEPNO ='" + strJepno + "' ";
                    SQL += ComNum.VBLF + "   AND YEAR = '" + strYear + "' ";

                    SqlErr = clsDB.GetDataTable(ref dtFunc, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    nResultAmtOUT = VB.Val(dtFunc.Rows[0]["AMT6"].ToString().Trim());

                    dtFunc.Dispose();
                    dtFunc = null;

                    nResultAmtOUT = 0;

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ";
                    SQL += ComNum.VBLF + " SUM(BAMT4) BAMT4";
                    SQL += ComNum.VBLF + "  FROM  " + ComNum.DB_PMPA + "EDI_F0702";
                    SQL += ComNum.VBLF + " WHERE JEPNO ='" + strJepno + "' ";
                    SQL += ComNum.VBLF + "   AND YEAR = '" + strYear + "' ";

                    SqlErr = clsDB.GetDataTable(ref dtFunc, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default; 
                        return;
                    }

                    nResultAmtOUT = nResultAmtOUT + VB.Val(dtFunc.Rows[0]["BAMT4"].ToString().Trim());

                    dtFunc.Dispose();
                    dtFunc = null;

                    // 'UPDATE ------------------------------------------------------------------------

                    SQL = "UPDATE " + ComNum.DB_PMPA + "EDI_JEPSU SET ";
                    SQL += ComNum.VBLF + "  SAKAMT = '" + nSakAmt + "', ";
                    SQL += ComNum.VBLF + "  REMIR = '" + nReMir + "', ";
                    SQL += ComNum.VBLF + "  RESULTAMT = '" + nResultAmt + "', ";
                    SQL += ComNum.VBLF + "  SAKAMTOUT = '" + nSakAmtOUT + "', ";
                    SQL += ComNum.VBLF + "  REMIROUT = '" + nReMirOUT + "', ";
                    SQL += ComNum.VBLF + "  RESULTAMTOUT = '" + nResultAmtOUT + "' ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                btnSearch.Enabled = true;
                dt.Dispose();
                dt = null;
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                return;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
