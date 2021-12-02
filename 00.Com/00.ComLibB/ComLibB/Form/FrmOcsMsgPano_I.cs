using ComBase; //기본 클래스
using ComDbB; //DB연결
using FarPoint.Win.Spread;
using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : FrmOcsMsg.cs
    /// Description     : 심사과 환자메세지등록(입원환자)
    /// Author          : 최익준
    /// Create Date     : 2017-11-02
    /// Update History  : 
    /// <seealso>
    /// PSMH\basic\busuga\BuSuga55.frm
    /// </seealso>
    /// <history>  
    /// 
    /// </history>
    /// </summary>
    public partial class FrmOcsMsgPano_I : Form
    {
        private frmSpecialText frmSpecialTextX = null;
        string GstrPANO = "";
        double GnJobSabun = 0;

        string FGstrROWID = "";
        string FstrPano = "";
        string FstrSName = "";
        double FdblWrtno = 0;

        public FrmOcsMsgPano_I()
        {
            InitializeComponent();
        }

        public FrmOcsMsgPano_I(string strPano)
        {
            InitializeComponent();
            GstrPANO = strPano;
        }

        public FrmOcsMsgPano_I(string strROWID, string strPano, string strSName, double dblWrtno)
        {
            InitializeComponent();
            FGstrROWID = strROWID;
            FstrPano = strPano;
            FstrSName = strSName;
            FdblWrtno = dblWrtno;
        }

        void SCREEN_CLEAR()
        {
            txtInfo.Text = "";

            dtpFDate.Text = "";
            dtpTDate.Text = "";

            FGstrROWID = "";

            btnSave.Enabled = false;
            btnDelete.Enabled = false;

            panMain.Enabled = false;
        }

        void ComboWard_SET()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = "SELECT WardCode, WardName  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('NP','2W','NR','DR','IQ','ER') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                cboWard.Items.Clear();
                cboWard.Items.Add("**.전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }

                cboWard.SelectedIndex = 0;

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
                Cursor.Current = Cursors.Default;
            }
        }

        private void FrmOcsMsgPano_I_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            txtPano.Text = "";
            txtName.Text = "";

            SCREEN_CLEAR();
            ComboWard_SET();

            cboSabun.Items.Clear();

            cboSabun.Items.Add("******.전체");

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            try
            {
                SQL = "";
                SQL = " SELECT SABUN, KORNAME  FROM " + ComNum.DB_ERP + "INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE TOIDAY IS NULL "; //재직자
                SQL = SQL + ComNum.VBLF + "   AND BUSE ='078201' ";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboSabun.Items.Add(dt.Rows[i]["SABUN"].ToString().Trim() + "." + dt.Rows[i]["KORNAME"].ToString().Trim());
                }

                cboSabun.SelectedIndex = 0;

                if (GstrPANO != "")
                {
                    txtPano.Text = GstrPANO;
                    DataSearch();
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

            ssMsg_Sheet1.Columns[4].Visible = false;
            ssMsg_Sheet1.Columns[5].Visible = false;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoRegis_Click(object sender, EventArgs e)
        {
            txtPano.Text = "";
            txtName.Text = "";

            SCREEN_CLEAR();
        }

        private void rdoDays_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDays.Checked == true)
            {
                dtpFDate.Enabled = true;
                dtpTDate.Enabled = true;
            }
            else
            {
                dtpFDate.Enabled = false;
                dtpTDate.Enabled = false;
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (txtPano.Text == "")
            {
                txtName.Text = "";
                return;
            }

            SCREEN_CLEAR();

            txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

            try
            {
                SQL = "";
                SQL = "SELECT PANO , SNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPano.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    FstrPano = txtPano.Text;
                    FstrSName = dt.Rows[0]["SNAME"].ToString().Trim();
                    txtName.Text = FstrSName;
                }
                else
                {
                    txtPano.Text = "";
                    txtName.Text = "";
                }

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            DataNew();
        }

        void DataNew()
        {
            SCREEN_CLEAR();

            btnSave.Enabled = true;
            panMain.Enabled = true;
            txtInfo.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            FromCancel();
        }

        void FromCancel()
        {
            SCREEN_CLEAR();

            btnNew.Enabled = false;
            panMain.Enabled = false;
            ssView_Sheet1.RowCount = 0;
            ssMsg_Sheet1.RowCount = 0;
            DataSearch();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (FGstrROWID == "")
            {
                ComFunc.MsgBox("전산오류입니다.", "확인");
                return;
            }
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "";
                SQL = " UPDATE " + ComNum.DB_PMPA + "BAS_OCSMEMO_I ";
                SQL = SQL + ComNum.VBLF + " SET DDATE = TRUNC(SYSDATE) WHERE ROWID = '" + FGstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);         //해당 트랜젝션을 바로 실행하기
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");

                SCREEN_CLEAR();
                DataSearch();
                panMain.Enabled = false;

                Cursor.Current = Cursors.Default;
                return;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if (string.IsNullOrEmpty(FstrPano) || string.IsNullOrEmpty(FstrSName))
            {
                txtPano.Focus();
                ComFunc.MsgBox("등록번호를 확인해주세요.");
                return;
            }

            string strData = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            strData = VB.Replace(txtInfo.Rtf, "'", "'");

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (FGstrROWID == "")
                {
                    SQL = "";
                    SQL = SQL + "SELECT MAX(WRTNO) MWRTNO ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_I ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                 

                    FdblWrtno = VB.Val(dt.Rows[0]["MWRTNO"].ToString());

                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "BAS_OCSMEMO_I ";
                    SQL = SQL + ComNum.VBLF + "     (  PANO, SNAME, MEMO, SDATE, DDATE, WRTNO, ENTSABUN )";
                    SQL = SQL + ComNum.VBLF + "VALUES (";
                    SQL = SQL + ComNum.VBLF + "         '" + FstrPano + "', '" + FstrSName + "', ";
                    SQL = SQL + ComNum.VBLF + "         :MEMO, ";
                    SQL = SQL + ComNum.VBLF + "         trunc(sysdate), '' , '" + FdblWrtno + "', '" + clsType.User.Sabun + "' ) ";

                    SqlErr = clsDB.ExecuteLongQuery(SQL, txtInfo.Rtf , ref intRowAffected, clsDB.DbCon);         //해당 트랜젝션을 바로 실행하기
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }


                    //SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_OCSMEMO_I SET ";
                    //SQL = SQL + ComNum.VBLF + "  MEMO = :MEMO ";
                    //SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPano + "'  AND WRTNO = '" + FdblWrtno + "' ";

                    //PsmhDb pDbCon = clsDB.DbCon;
                    //OracleCommand cmd = new OracleCommand(SQL, pDbCon.Con);

                    //cmd.Parameters.Add("MEMO", txtInfo.Rtf);
                    //cmd.ExecuteNonQuery();


                    SQL = "";
                    SQL = "SELECT ROWID FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_I WHERE PANO = '" + FstrPano + "'  AND WRTNO = '" + FdblWrtno + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                  

                    FGstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }
                else
                {

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_OCSMEMO_I";
                    SQL = SQL + ComNum.VBLF + "     SET";
                    SQL = SQL + ComNum.VBLF + "         MEMO = '' , ";
                    SQL = SQL + ComNum.VBLF + "         ENTSABUN = '" + GnJobSabun + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);         //해당 트랜젝션을 바로 실행하기
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_OCSMEMO_I SET ";
                    SQL = SQL + ComNum.VBLF + "  MEMO = :MEMO ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrROWID + "' ";

                    PsmhDb pDbCon = clsDB.DbCon;
                    OracleCommand cmd = new OracleCommand(SQL, pDbCon.Con);

                    cmd.Parameters.Add("MEMO", txtInfo.Rtf);
                    cmd.ExecuteNonQuery();

                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();

                ssMsg_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = 0;

                return;
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            DataSearch();
        }

        void DataSearch()
        {
            string strPano = "";

            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT A.PANO  APANO , A.SNAME, A.WARDCODE, a.ROOMCODE, A.DEPTCODE, B.PANO BPANO, B.SNAME BSNAME ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_OCSMEMO_I B ";

                if (rdoRegis.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.PANO = B.PANO";
                }

                if (rdoJewon.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.PANO = B.PANO(+)";      //재원자
                }

                if (rdoDays.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.PANO(+) = B.PANO";        //전체(일자별)
                    SQL = SQL + ComNum.VBLF + "    AND B.SDATE >=TO_DATE('" + dtpFDate.Text + "' ,'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND B.SDATE <=TO_DATE('" + dtpTDate.Text + "' ,'YYYY-MM-DD') ";

                    if (txtPano.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "AND B.PANO = '" + txtPano.Text + "' ";       //등록자 등록번호
                    }

                    SQL = SQL + ComNum.VBLF + "   AND B.DDATE IS NULL ";        //삭제 제외
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND A.ACTDATE IS NULL ";
                    if (txtPano.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "AND A.PANO = '" + txtPano.Text + "' ";       //입원자 등록번호
                    }
                }

                if (VB.Pstr(cboSabun.Text.Trim(), ".", 1) != "******")
                {
                    SQL = SQL + ComNum.VBLF + "AND B.ENTSABUN = '" + VB.Pstr(cboSabun.Text.Trim(), ".", 1) + "' ";
                }

                if (VB.Left(cboWard.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + "AND A.WARDCODE = '" + VB.Left(cboWard.Text, 2) + "'";
                }

                if (rdoNum.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.PANO, A.SNAME, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, B.PANO, B.SNAME ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.PANO, A.SNAME, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, B.PANO, B.SNAME ";
                }
                else if (rdoName.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.SNAME, A.PANO, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, B.PANO, B.SNAME ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.SNAME, A.PANO, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, B.PANO, B.SNAME ";
                }


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }
               

                ssView_Sheet1.RowCount = 0;
                ssMsg_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (rdoDays.Checked == true)
                    {
                        strPano = dt.Rows[i]["BPANO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 0].Text = strPano;
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BSNAME"].ToString().Trim();
                    }
                    else
                    {
                        strPano = dt.Rows[i]["APANO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 0].Text = strPano;
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    }
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["WARDCODE"].ToString().Trim() + " / " + dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    if (dt.Rows[i]["BPANO"].ToString().Trim() != "")
                    {
                        //내용중 삭제된것이 있을 경우 색을 달리표시
                        SQL = " SELECT COUNT(*) CNT ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_I ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND DDATE IS NULL ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt2.Rows.Count == 0)
                        {
                            dt2.Dispose();
                            dt2 = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (VB.Val(dt2.Rows[0]["CNT"].ToString().Trim()) > 0)
                        {
                            ssView_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 233, 233);                            
                        }
                        else
                        {
                            ssView_Sheet1.Rows[i].BackColor = Color.FromArgb(119, 119, 119);                            
                        }

                        dt2.Dispose();
                        dt2 = null;
                    }

                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                }
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                dt.Dispose();
                dt = null;

                ssView.ActiveSheet.ColumnHeader.Cells[0, 0, 0, ssView.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;

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

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            FstrPano = ssView_Sheet1.Cells[e.Row, 0].Text;
            FstrSName = ssView_Sheet1.Cells[e.Row, 1].Text;

            if (FstrPano == "")
            {
                return;
            }

            try
            {
                SQL = "";
                SQL = " SELECT PANO, SNAME, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, TO_CHAR(DDATE,'YYYY-MM-DD') DDATE,";
                SQL = SQL + ComNum.VBLF + " WRTNO, ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_I ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPano + "'";

                if (VB.Pstr(cboSabun.Text, ".", 1) != "******")
                {
                    SQL = SQL + ComNum.VBLF + "  AND ENTSABUN = '" + VB.Pstr(cboSabun.Text, ".", 1) + "' ";
                }

                SQL = SQL + ComNum.VBLF + "  ORDER BY SDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssMsg_Sheet1.RowCount = 0;
                ssMsg_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssMsg_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                    ssMsg_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DDATE"].ToString().Trim();
                    ssMsg_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssMsg_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssMsg_Sheet1.Cells[i, 4].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    ssMsg_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                if (dt.Rows.Count == 0)
                {
                    DataNew();
                }
                else
                {
                    FGstrROWID = ssMsg_Sheet1.Cells[0, 5].Text;
                    GetMsg();
                }

                btnNew.Enabled = true;

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssMsg_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssMsg_Sheet1.Rows.Count == 0)
            {
                return;
            }

            FGstrROWID = ssMsg_Sheet1.Cells[e.Row, 5].Text;

            if (FGstrROWID == "")
            {
                return;
            }

            if(e.RowHeader)
            {
                if (ComFunc.MsgBoxQ("완전삭제 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No) return;
                
                Delete_Data();
                return;
            }

            GetMsg();
   
        }

        void Delete_Data()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " DELETE " + ComNum.DB_PMPA + "BAS_OCSMEMO_I";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrROWID + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);         //해당 트랜젝션을 바로 실행하기
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    return;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                FromCancel();
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

        void GetMsg()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {

                SQL = "";
                SQL = " SELECT MEMO ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_I ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrROWID + "' ";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                txtInfo.Rtf = VB.Replace(dt.Rows[0]["MEMO"].ToString().Trim(), "`", "'");
                dt.Dispose();
                dt = null;

                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                panMain.Enabled = true;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnSpecial_Click(object sender, EventArgs e)
        {            
            if (frmSpecialTextX != null)
            {
                frmSpecialTextX.Dispose();
                frmSpecialTextX = null;
            }
            frmSpecialTextX = new frmSpecialText();
            frmSpecialTextX.rSendText += new frmSpecialText.SendText(GetText);
            frmSpecialTextX.rEventExit += new frmSpecialText.EventExit(frmSpecialTextX_rEventExit);
            frmSpecialTextX.Show();
        }

        private void frmSpecialTextX_rEventExit()
        {
            frmSpecialTextX.Dispose();
            frmSpecialTextX = null;
        }

        private void GetText(string strText)
        {
            txtInfo.Text += strText;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            //TODO : 글꼴 선택

            //CommonDialog1.CancelError = True;
            //On Error GoTo ErrHandler;
            ////Flags 속성을 설정합니다.
            //CommonDialog1.Flags = cdlCFEffects Or cdlCFBoth;
            ////[글꼴] 대화 상자를 표시합니다.
            //CommonDialog1.ShowFont;

            //FontDialog f = new FontDialog(Font.Name, Font.Size, Font.Style, Font.Bold, Font.GdiCharSet, Font.GdiVerticalFont, Font.Italic);
            //f.ShowDialog();

            //txtInfo.Font = new Font(f.Font.Name, f.Font.Size, f.Font.Style, f.Font.Bold, f.Font.GdiCharSet, f.Font.GdiVerticalFont, f.Font.Italic);
            //txtInfo.ForeColor = f.Color;

            //TxtInfo.SelFontName = CommonDialog1.FontName;
            //TxtInfo.SelFontSize = CommonDialog1.FontSize;
            //TxtInfo.SelBold = CommonDialog1.FontBold;
            //TxtInfo.SelItalic = CommonDialog1.FontItalic;
            //TxtInfo.SelUnderline = CommonDialog1.FontUnderline;
            //TxtInfo.SelStrikeThru = CommonDialog1.FontStrikethru;
            //TxtInfo.SelColor = CommonDialog1.Color;

            //TxtInfo.SetFocus;
            //ErrHandler:;
            ////사용자가 [취소] 단추를 눌렀습니다.
            //Exit Sub;
          

           DialogResult dr = this.fontDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                txtInfo.SelectionFont = fontDialog1.Font ;
                txtInfo.SelectionColor = fontDialog1.Color;
            }

            txtInfo.Focus();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.ssView)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column); //sort 정렬 기능 
                    return;
                }
                if (e.RowHeader == true)
                {
                    return;
                }
            }
        }
    }
    
}

