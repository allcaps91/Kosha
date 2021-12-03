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
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaJengSanBabyMatching.cs
    /// Description     : 애기 등록번호 매칭
    /// Author          : 이정현
    /// Create Date     : 2018-08-23
    /// <history> 
    /// 애기 등록번호 매칭
    /// </history>
    /// <seealso>
    /// PSMH\OPD\jengsan\Frm애기매칭.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\OPD\jengsan\jengsan.vbp
    /// </vbp>
    /// </summary>
    public partial class frmPmpaJengSanBabyMatching : Form
    {
        private int GintRow = 0;

        public frmPmpaJengSanBabyMatching()
        {
            InitializeComponent();
        }

        private void frmPmpaJengSanBabyMatching_Load(object sender, EventArgs e)
        {
            ////폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            string strSysDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            dtpFDate.Value = Convert.ToDateTime(strSysDate);
            dtpTDate.Value = Convert.ToDateTime(strSysDate);

            ssView_Sheet1.RowCount = 0;
            ssView2_Sheet1.Cells[0, 0, ssView2_Sheet1.RowCount - 1, ssView2_Sheet1.ColumnCount - 1].Text = "";

            if (clsType.User.Sabun == "4349")
            {
                btnOK.Enabled = true;
            }

            GintRow = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
          //  if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;

            string strPANO = "";
            string strSname = "";
            string strJumin = "";
            string strPano2 = "";
            string strSname2 = "";
            string strJumin2 = "";
            string strYear = dtpYear.Value.Year.ToString();
            string strRowId = "";
            string strKiho = "";

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, JUMIN, SNAME,JUMIN_new ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_JUNSLIP ";
                //원 SQL
                SQL = SQL + ComNum.VBLF + "     WHERE baby ='Y'  ";

               // SQL = SQL + ComNum.VBLF + "     WHERE (JUMIN LIKE '%3000000' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%4000000' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%3100000' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%1000000' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%2000000' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%1100000' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%2100000' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%0000000' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%5000000' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%3000001' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%3000002' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%3000003' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%3000004' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%4000001' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%4000002' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%4000003' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%4000004' OR ";
               // SQL = SQL + ComNum.VBLF + "             JUMIN LIKE '%4400000') ";
                SQL = SQL + ComNum.VBLF + "         AND YEAR = '" + strYear + "' ";
                SQL = SQL + ComNum.VBLF + "         AND ACTDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND ACTDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND PANO NOT IN";
                SQL = SQL + ComNum.VBLF + "                     (SELECT";
                SQL = SQL + ComNum.VBLF + "                         PANO";
                SQL = SQL + ComNum.VBLF + "                     FROM " + ComNum.DB_PMPA + "ETC_JUNREMARK";
                SQL = SQL + ComNum.VBLF + "                         WHERE YEAR ='" + strYear + "' ) ";
                SQL = SQL + ComNum.VBLF + "GROUP BY PANO, JUMIN, JUMIN_new, SNAME ";

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
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strJumin = clsAES.DeAES(dt.Rows[i]["JUMIN_new"].ToString().Trim());  
                        strSname = dt.Rows[i]["SNAME"].ToString().Trim();
                        strPANO = dt.Rows[i]["PANO"].ToString().Trim();

                        strKiho = "";
                     
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     GKIHO";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT  ";
                        SQL = SQL + ComNum.VBLF + "     WHERE PANO  = '" + strPANO + "' ";

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
                            strKiho = dt1.Rows[0]["GKIHO"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;

                        ssView_Sheet1.Cells[i, 0].Text = strPANO;
                        ssView_Sheet1.Cells[i, 1].Text = strSname;
                        ssView_Sheet1.Cells[i, 2].Text = VB.Left(strJumin, 6);
                        ssView_Sheet1.Cells[i, 3].Text = VB.Right(strJumin, 7);
                        ssView_Sheet1.Cells[i, 4].Text = strKiho;

                        strJumin2 = "";
                        strSname2 = "";
                        strPano2 = "";
                        strRowId = "";

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     A.PANO2, A.JUMIN2, A.JUMIN2_New, B.SNAME, A.ROWID ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_JUNMATCH A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                        SQL = SQL + ComNum.VBLF + "     WHERE JUMIN_New = '" + clsAES.AES(strJumin) + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND A.PANO2 = B.PANO(+) ";
                        SQL = SQL + ComNum.VBLF + "         AND A.PANO = '" + strPANO + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND A.year = '" + strYear + "' ";

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
                            strJumin2 = clsAES.DeAES(dt1.Rows[0]["JUMIN2_NEW"].ToString().Trim());
                            strSname2 = dt1.Rows[0]["SNAME"].ToString().Trim();
                            strPano2 = dt1.Rows[0]["PANO2"].ToString().Trim();
                            strRowId = dt1.Rows[0]["ROWID"].ToString().Trim();

                            ssView_Sheet1.Cells[i, 5].Text = strPano2;
                            ssView_Sheet1.Cells[i, 6].Text = strSname2;
                            ssView_Sheet1.Cells[i, 7].Text = VB.Left(strJumin2, 6);
                            ssView_Sheet1.Cells[i, 8].Text = VB.Right(strJumin2, 7);
                            ssView_Sheet1.Cells[i, 9].Text = strRowId;
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }

                dt.Dispose();
                dt = null;

                GintRow = 0;

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

        private void btnSave_Click(object sender, EventArgs e)
        {
          //  if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
           // if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (SaveData() == true)
            {
                GetData();
            }
        }

        private bool SaveData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strPANO = "";
            string strSname = "";
            string strJumin = "";
            string strPano2 = "";
            string strSname2 = "";
            string strJumin2 = "";
            string strYear = dtpYear.Value.Year.ToString();
            string strRowId = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.NonEmptyRowCount; i++)
                {
                    strPANO = ssView_Sheet1.Cells[i, 0].Text.Trim();
                    strJumin = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    strJumin += ssView_Sheet1.Cells[i, 3].Text.Trim();

                    strPano2 = ssView_Sheet1.Cells[i, 5].Text.Trim();
                    strJumin2 = ssView_Sheet1.Cells[i, 7].Text.Trim();
                    strJumin2 += ssView_Sheet1.Cells[i, 8].Text.Trim();
                    strRowId = ssView_Sheet1.Cells[i, 9].Text.Trim();

                    if (strPano2 != "")
                    {
                        if (strRowId == "")
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_JUNMATCH";
                            SQL = SQL + ComNum.VBLF + "     (PANO, JUMIN, PANO2, JUMIN2, YEAR, JUMIN_New, JUMIN2_New)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         '" + strPANO + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + VB.Left(strJumin, 7) + "******', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strPano2 + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + VB.Left(strJumin2, 7) + "******', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strYear + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + clsAES.AES(strJumin) + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + clsAES.AES(strJumin2) + "' ";
                            SQL = SQL + ComNum.VBLF + "     )";
                        }
                        else
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "ETC_JUNMATCH";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         PANO2 = '" + strPano2 + "', ";
                            SQL = SQL + ComNum.VBLF + "         JUMIN2 = '" + VB.Left(strJumin2, 1) + "******', ";
                            SQL = SQL + ComNum.VBLF + "         JUMIN2_New = '" + clsAES.AES(strJumin2) + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strRowId + "' ";
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            GintRow = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (BuildData() == true)
            {
                GetData();
            }
        }

        private bool BuildData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strBPano = "";
            string strMPano = "";
            string strSname = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strYear = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.NonEmptyRowCount; i++)
                {
                    strBPano = ssView_Sheet1.Cells[i, 0].Text.Trim();
                    strMPano = ssView_Sheet1.Cells[i, 5].Text.Trim();
                    strSname = ssView_Sheet1.Cells[i, 6].Text.Trim();
                    strJumin1 = ssView_Sheet1.Cells[i, 7].Text.Trim();
                    strJumin2 = ssView_Sheet1.Cells[i, 8].Text.Trim();

                    if (strMPano != "")
                    {
                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_PMPA + "ETC_JUNSLIP";
                        SQL = SQL + ComNum.VBLF + "     SET ";
                        SQL = SQL + ComNum.VBLF + "         PANO = '" + strMPano + "', ";
                        SQL = SQL + ComNum.VBLF + "         SNAME = '" + strSname + "', ";
                        SQL = SQL + ComNum.VBLF + "         JUMIN = '" + strJumin1 + VB.Left(strJumin2, 1) + "******'," ;
                        SQL = SQL + ComNum.VBLF + "         JUMIN_new = '" + clsAES.AES(strJumin1 + strJumin2) + "', "; //주민암호화
                        SQL = SQL + ComNum.VBLF + "         BPANO = '" + strBPano + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE YEAR = '" + strYear + "' ";
                        SQL = SQL + ComNum.VBLF + "     AND PANO = '" + strBPano + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtpYear_ValueChanged(object sender, EventArgs e)
        {
            dtpFDate.Value = Convert.ToDateTime(dtpYear.Value.Year + "-01-01");
            dtpTDate.Value = Convert.ToDateTime(dtpYear.Value.Year + "-12-31");
        }

        private void ssView_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
          //  if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (e.Column != 10) { return; }

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";

            string strPANO = "";
            string strFDate = "";
            string strTDate = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strTemp = "";

            GintRow = 0;

            ssView2_Sheet1.Cells[0, 0, ssView2_Sheet1.RowCount - 1, ssView2_Sheet1.ColumnCount - 1].Text = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                strPANO = ssView_Sheet1.Cells[e.Row, 0].Text.Trim();
                strFDate = dtpYear.Value.AddYears(-1).ToString("yyyy-12-01");
                strTDate = dtpYear.Value.ToString("yyyy-11-30");

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     JUPBONO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPANO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND INDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND INDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND JUPBONO IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "GROUP BY JUPBONO ";

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
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SNAME, JUMIN1, JUMIN2, JUMIN3";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + ComFunc.LPAD(dt.Rows[0]["JUPBONO"].ToString().Trim(), 8, "0") + "' ";

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
                        ssView_Sheet1.Cells[e.Row, 5].Text = ComFunc.LPAD(dt.Rows[0]["JUPBONO"].ToString().Trim(), 8, "0");
                        ssView_Sheet1.Cells[e.Row, 6].Text = dt1.Rows[0]["SNAME"].ToString().Trim() + "의애기";
                        ssView_Sheet1.Cells[e.Row, 7].Text = dt1.Rows[0]["JUMIN1"].ToString().Trim();
                        ssView_Sheet1.Cells[e.Row, 8].Text = clsAES.DeAES(dt1.Rows[0]["JUMIN2"].ToString().Trim());
                    }

                    dt1.Dispose();
                    dt1 = null;
                }
                else if (dt.Rows.Count >= 2)
                {
                    ComFunc.MsgBox("엄마번호 두개 이상임.!!!");
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SNAME, Remark";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPANO + "' ";

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
                    strTemp = dt.Rows[0]["REMARK"].ToString().Trim();

                    ssView2_Sheet1.Cells[0, 0].Text = strTemp;
                    ssView2_Sheet1.Cells[0, 1].Text = dt.Rows[0]["SNAME"].ToString().Trim();

                    if (strTemp != "" && VB.L(strTemp, ":") > 1 && VB.L(strTemp, "-") > 1)
                    {
                        GintRow = e.Row;

                        ssView2_Sheet1.Cells[0, 3].Text = VB.Pstr(VB.Pstr(strTemp, ":", 2), "-", 1).Trim();
                        ssView2_Sheet1.Cells[0, 4].Text = VB.Pstr(VB.Pstr(strTemp, ":", 2), "-", 2).Trim();

                        strJumin1 = VB.Pstr(VB.Pstr(strTemp, ":", 2), "-", 1).Trim();

                        if (VB.Len(VB.Pstr(VB.Pstr(strTemp, ":", 2), "-", 2)) > 7)
                        {
                            strJumin2 = VB.Pstr(VB.Pstr(strTemp, ":", 2), "-", 2);
                        }
                        else
                        {
                            strJumin2 = clsAES.AES(VB.Pstr(VB.Pstr(strTemp, ":", 2), "-", 2));
                        }

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     Pano";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                        SQL = SQL + ComNum.VBLF + "     WHERE Jumin1 = '" + strJumin1 + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND Jumin3 = '" + strJumin2 + "' ";

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
                            ssView2_Sheet1.Cells[0, 2].Text = dt1.Rows[0]["PANO"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;
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

        private void ssView2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true) { return; }

            string strSname = "";
            string strPANO = "";
            string strJumin1 = "";
            string strJumin2 = "";

            strSname = ssView2_Sheet1.Cells[e.Row, 1].Text.Trim();
            strPANO = ssView2_Sheet1.Cells[e.Row, 2].Text.Trim();
            strJumin1 = ssView2_Sheet1.Cells[e.Row, 3].Text.Trim();
            strJumin2 = ssView2_Sheet1.Cells[e.Row, 4].Text.Trim();

            if (strJumin1.Length == 6 && strJumin2.Length == 7 && strPANO != "")
            {
                ssView_Sheet1.Cells[GintRow, 5].Text = strPANO;
                ssView_Sheet1.Cells[GintRow, 6].Text = strSname;
                ssView_Sheet1.Cells[GintRow, 7].Text = strJumin1;
                ssView_Sheet1.Cells[GintRow, 8].Text = strJumin2;
            }
        }

        private void ssView_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strPano = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string strDEPTCODE = "";

            if (e.Column == 0)
            {
                strPano = ssView_Sheet1.Cells[e.Row, 0].Text.Trim(); ;

                SQL = "";
                SQL = "SELECT SNAME, JUMIN1, JUMIN2, JUMIN3 FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + strPano + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장 
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count >= 0)
                {
                    ssView_Sheet1.Cells[e.Row, 1].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[e.Row, 2].Text = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    ssView_Sheet1.Cells[e.Row, 3].Text = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim()); 
                        
                }
                dt.Dispose();
                dt = null;

            }
            else if (e.Column == 5)

            {
                strPano = ssView_Sheet1.Cells[e.Row, 5].Text.Trim(); 

                SQL = "";
                SQL = "SELECT SNAME, JUMIN1, JUMIN2, JUMIN3 FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + strPano + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장 
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count >= 0)
                {
                    ssView_Sheet1.Cells[e.Row, 6].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[e.Row, 7].Text = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    ssView_Sheet1.Cells[e.Row, 8].Text = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim()); 

                }
                dt.Dispose();
                dt = null;
                
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            string strPrintName = clsPrint.gGetDefaultPrinter();//기본프린터 이름을 가져온다

            if (strPrintName != "")
            {
                strTitle = "애기 등록번호 매칭" + "/n";
                strSubTitle = "출력일자 : " + DateTime.Now.ToString("yyyy-MM-dd");


                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 1, true, true, true, true, true, false, false);

                SPR.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
            }
        }
    }
}
