using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComLibB
{
    public partial class FrmOcsMsgPano_O : Form
    {
        private frmSpecialText frmSpecialTextX = null;

        public delegate void REventExit(object sender, EventArgs e);
        public static event REventExit rEventExit;


        string GstrJobSabun = "";
        string FGstrROWID = "";
        string FstrPANO = "";
        string FstrSNAME = "";
        string GstrPANO = "";
        double FdblWrtno = 0; 
        
        public FrmOcsMsgPano_O()
        { 
            InitializeComponent(); 
            GstrJobSabun = clsType.User.IdNumber; 
        }

        public FrmOcsMsgPano_O(string dblJobSabun, string strPANO, string SuChk)
        {
            InitializeComponent();
            GstrPANO = strPANO;
            GstrJobSabun = dblJobSabun;
        }

        public FrmOcsMsgPano_O(string dblJobSabun, string strPANO)
        {
            InitializeComponent();            
            GstrPANO = strPANO;
            
            GstrJobSabun = clsType.User.IdNumber;
        }

        void SCREEN_CLEAR()
        {
            txtInfo.Text = "";
            txtPano.Text = "";
            txtName.Text = "";
            FGstrROWID = "";

            btnSave.Enabled = false;
            btnDelete.Enabled = false;

            panMain.Enabled = false;
        }

        private void FrmOcsMsgPano_O_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SCREEN_CLEAR();
            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept);

            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            if (GstrPANO != "")
            {
                txtPano.Text = GstrPANO;
                DataSearch();
            }

            ssMsg_Sheet1.Columns[5].Visible = false;
            ssMsg_Sheet1.Columns[6].Visible = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if(rEventExit != null)
            {
                rEventExit(sender,e);
            }

            this.Close();
        }

        private void rdoRegis_CheckedChanged(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
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
            try
            {
                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

                SQL = "";
                SQL = " SELECT PANO, SNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
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
                    txtName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (FGstrROWID == "")
            {
                ComFunc.MsgBox("전산오류입니다.", "확인");

                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = "";
                SQL = " UPDATE " + ComNum.DB_PMPA + "BAS_OCSMEMO_O ";
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
                Cursor.Current = Cursors.Default;

                //  SCREEN_CLEAR();
                txtInfo.Text = "";
                DataSearch();
                panMain.Enabled = false;

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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataCnacel();
        }

        void DataCnacel()
        {
            SCREEN_CLEAR();
            btnNew.Enabled = false;
            panMain.Enabled = false;
            ssView_Sheet1.RowCount = 0;
            ssMsg_Sheet1.RowCount = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if (string.IsNullOrEmpty(FstrPANO) || string.IsNullOrEmpty(FstrSNAME))
            {
                txtPano.Focus();
                ComFunc.MsgBox("등록번호를 확인해주세요.");
                return;
            }

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strData = "";

            strData = VB.Replace(txtInfo.Text, "'", "'");
            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon); 

            try
            {
                if (FGstrROWID == "")
                {

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT MAX(WRTNO) MWRTNO ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_O";

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
                    }

                    FdblWrtno = VB.Val(dt.Rows[0]["MWRTNO"].ToString().Trim()) + 1;

                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_OCSMEMO_O (  PANO, SNAME, MEMO, SDATE, DDATE, WRTNO, ENTSABUN, DEPTCODE ) VALUES (";
                    SQL = SQL + ComNum.VBLF + " '" + FstrPANO + "', '" + FstrSNAME + "', :MEMO, ";
                    SQL = SQL + ComNum.VBLF + "trunc(sysdate), '' , '" + FdblWrtno + "', " + GstrJobSabun + ", ";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Left(cboDept.Text, 2) + "' ) ";

                    SqlErr = clsDB.ExecuteLongQuery(SQL, txtInfo.Rtf, ref intRowAffected, clsDB.DbCon);         //해당 트랜젝션을 바로 실행하기

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }


                    //SQL = " UPDATE " + ComNum.DB_PMPA + "BAS_OCSMEMO_O SET ";
                    //SQL = SQL + ComNum.VBLF + "  MEMO = :MEMO ";
                    //SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPANO + "'  AND WRTNO = '" + FdblWrtno + "' ";

                    //PsmhDb pDbCon = clsDB.DbCon;
                    //OracleCommand cmd = new OracleCommand(SQL, pDbCon.Con);

                    //cmd.Parameters.Add("MEMO", txtInfo.Rtf);
                    //cmd.ExecuteNonQuery();


                    SQL = "";
                    SQL = "SELECT ROWID";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_O";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPANO + "'  AND WRTNO = '" + FdblWrtno + "' ";

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
                    }

                    FGstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "BAS_OCSMEMO_O";
                    SQL = SQL + ComNum.VBLF + " SET MEMO = :MEMO, ENTSABUN = " + GstrJobSabun + " ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrROWID + "' ";

                    SqlErr = clsDB.ExecuteLongQuery(SQL, txtInfo.Rtf, ref intRowAffected, clsDB.DbCon);         //해당 트랜젝션을 바로 실행하기

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }


                    //SQL = " UPDATE " + ComNum.DB_PMPA + "BAS_OCSMEMO_O SET ";
                    //SQL = SQL + ComNum.VBLF + "  MEMO = :MEMO ";
                    //SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrROWID + "' ";

                    //PsmhDb pDbCon = clsDB.DbCon;
                    //OracleCommand cmd = new OracleCommand(SQL, pDbCon.Con);

                    //cmd.Parameters.Add("MEMO", txtInfo.Rtf);
                    //cmd.ExecuteNonQuery();
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();

                ssView_Sheet1.RowCount = 0;
                ssMsg_Sheet1.RowCount = 0;
                DataCnacel();
                btnNew.Enabled = false;

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
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            btnNew.Enabled = true;
            if (rdoOut.Checked == true && txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호를 입력해야합니다.", "확인");
                return;
            }

            try
            {
                SQL = "";
                SQL = "SELECT A.PANO  APANO , A.SNAME,  B.PANO BPANO";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_PATIENT A, " + ComNum.DB_PMPA + "BAS_OCSMEMO_O B";

                if (rdoRegis.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.PANO = B.PANO";
                }

                if (rdoOut.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.PANO = B.PANO(+)";      // 재원자
                }

                if (txtPano.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND A.PANO = '" + txtPano.Text + "' ";
                }

                if (rdoNum.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.PANO, A.SNAME,   B.PANO";
                }
                if (rdoName.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.SNAME, A.PANO,   B.PANO ";
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
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["APANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();

                    if (dt.Rows[i]["BPANO"].ToString().Trim() != "")
                    {
                        //SS1.Col = -1: SS1.BackColor = RGB(255, 233, 233)
                    }
                }
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                dt.Dispose();
                dt = null;

                ssView.ActiveSheet.ColumnHeader.Cells[0, 0, 0, ssView.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        private void btnSpecial_Click(object sender, EventArgs e)
        {
            //ComLibB.frmSpecialText frm = new ComLibB.frmSpecialText();
            //frm.Show();
            //txtInfo.Focus();

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

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (ssView_Sheet1.Rows.Count == 0 || ssView_Sheet1.Rows.Count == 0)
            {
                return;
            }

            FstrPANO = ssView_Sheet1.Cells[e.Row, 0].Text;
            FstrSNAME = ssView_Sheet1.Cells[e.Row, 1].Text;

            if (FstrPANO == "")
            {
                return;
            }

            try
            {
                SQL = "";
                SQL = " SELECT PANO, SNAME, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, TO_CHAR(DDATE,'YYYY-MM-DD') DDATE,";
                SQL = SQL + ComNum.VBLF + " WRTNO, ROWID, ENTSABUN ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_O ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPANO + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY SDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }
               // if (dt.Rows.Count == 0)
               // {
               //     ComFunc.MsgBox("해당 DATA가 없습니다.");
               //     Cursor.Current = Cursors.Default;

               //   dt.Dispose();
               //     dt = null;

               //     return;
               // }
                ssMsg_Sheet1.RowCount = 0;
                ssMsg_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssMsg_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                    ssMsg_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DDATE"].ToString().Trim();
                    ssMsg_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssMsg_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssMsg_Sheet1.Cells[i, 4].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["ENTSABUN"].ToString().Trim());
                    ssMsg_Sheet1.Cells[i, 5].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    ssMsg_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
                
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count == 0)
                {
                    DataNew();
                }
                else
                {
                    FGstrROWID = ssMsg_Sheet1.Cells[0, 6].Text.Trim();
                    GetMsg();
                    ssMsg_Sheet1.SetActiveCell(0, 0);
                }

                btnNew.Enabled = true;

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

            FGstrROWID = ssMsg_Sheet1.Cells[e.Row, 6].Text;

            if (FGstrROWID == "")
            {
                return;
            }

            if (e.RowHeader == true)
            {
                if (ComFunc.MsgBoxQ("완전삭제 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No) return;
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
                if (Delete_Data() == false) return;

                DataCnacel();
            }
            else
            {
                GetMsg();
            }
        }

        bool Delete_Data()
        {

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "DELETE " + ComNum.DB_PMPA + "BAS_OCSMEMO_O ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrROWID + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);         //해당 트랜젝션을 바로 실행하기
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
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
                SQL = "SELECT MEMO, DEPTCODE  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_O ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                txtInfo.Rtf = VB.Replace(dt.Rows[0]["MEMO"].ToString(), "`", "'");
                cboDept.Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + "." + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[0]["DEPTCODE"].ToString().Trim());

                dt.Dispose();
                dt = null;

                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                panMain.Enabled = true;
                //txtInfo.SelectionFont = new System.Drawing.Font("", 14);
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

        private void btnSelect_Click(object sender, EventArgs e)
        {
            //CommonDialog1.CancelError = True
            //On Error GoTo ErrHandler
            // Flags 속성을 설정합니다.
            //CommonDialog1.Flags = cdlCFEffects Or cdlCFBoth
            // [글꼴] 대화 상자를 표시합니다.
            //CommonDialog1.ShowFont

            //TxtInfo.SelFontName = CommonDialog1.FontName
            //TxtInfo.SelFontSize = CommonDialog1.FontSize
            //TxtInfo.SelBold = CommonDialog1.FontBold
            //TxtInfo.SelItalic = CommonDialog1.FontItalic
            //TxtInfo.SelUnderline = CommonDialog1.FontUnderline
            //TxtInfo.SelStrikeThru = CommonDialog1.FontStrikethru
            //TxtInfo.SelColor = CommonDialog1.Color

            //TxtInfo.SetFocus
            //ErrHandler:
            // 사용자가 [취소] 단추를 눌렀습니다.
            //Exit Sub


            DialogResult dr = this.fontDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                txtInfo.SelectionFont= fontDialog1.Font;
                //txtInfo.SelectionFont = new System.Drawing.Font("맑은 고딕", 14);
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
