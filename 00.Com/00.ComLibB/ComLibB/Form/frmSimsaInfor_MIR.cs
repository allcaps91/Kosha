using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmSimsaInfor_MIR : Form
    {
        private string GstrHelpCode = "";
        string FGstrRowid;

        public frmSimsaInfor_MIR()
        {
            InitializeComponent();
        }

        public frmSimsaInfor_MIR(string GstrHelpCode)
        {
            InitializeComponent();
            this.GstrHelpCode = GstrHelpCode;
        }

        private void frmSimsaInfor_MIR_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept, "1");

            SCREEN_CLEAR();
            btnSearchClick();

            if (GstrHelpCode != "")
            {
                txtSuNext.Text = GstrHelpCode;
            }
        }

        private void SCREEN_CLEAR()
        {
            txtSuNext.Text = "";
            txtHname.Text = "";            
            txtBCode.Text = "";            
            txtInfo.Text = "";
            FGstrRowid = "";
            
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            panMain.Enabled = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            btnSearchClick();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strData = "";

            if (clsType.User.Sabun == "16412" || clsType.User.Sabun == "19684") //'원무과 이희목, 박시철
            {
                ComFunc.MsgBox("등록권한이 없습니다.", "확인");
                return;
            }
            
            strData = VB.Replace(txtInfo.Text, "'", "`");

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인

                if (FGstrRowid == "")
                {                    
                    SQL = " INSERT INTO ADMIN.BAS_SIMSAINFOR_MIR ( SUNEXT, REMARK, DEPTCODE ) VALUES (";
                    SQL = SQL + ComNum.VBLF + " '" + txtSuNext.Text + "',  :REMARK, '" + VB.Left(cboDept.Text, 2) + "' )  ";

                    SqlErr = clsDB.ExecuteClobQuery(SQL, strData, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }                    
                }
                else
                {                    
                    SQL = " UPDATE ADMIN.BAS_SIMSAINFOR_MIR SET REMARK = '" + strData + "', DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "'  ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrRowid + "' ";

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

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();
                btnSearchClick();

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            ss1_Sheet1.RowCount = 0;
            btnSearch.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (clsType.User.Sabun == "16412" || clsType.User.Sabun == "19684") //'원무과 이희목, 박시철
            {
                ComFunc.MsgBox("등록권한이 없습니다.", "확인");
                return;
            }

            if (FGstrRowid == "")
            {
                ComFunc.MsgBox("전산오류입니다.", "확인");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인

                SQL = " DELETE ADMIN.BAS_SIMSAINFOR_MIR WHERE ROWID = '" + FGstrRowid + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (clsType.User.Sabun == "16412" || clsType.User.Sabun == "19684") //'원무과 이희목, 박시철
            {
                ComFunc.MsgBox("등록권한이 없습니다.", "확인");
                return;
            }

            SCREEN_CLEAR();

            panMain.Enabled = true;
            txtSuNext.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == -1 || e.Row == -1) return;

            SCREEN_CLEAR();

            txtSuNext.Text = ss1_Sheet1.Cells[e.Row, 0].Text;
            getData();
        }

        private void txtSuNext_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            getData();            
        }

        private void getData()
        {
            if (txtSuNext.Text.Trim() == "") return;

            txtSuNext.Text = txtSuNext.Text.ToUpper();

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                //'수가READ;
                SQL = " SELECT A.SUNAMEK, A.SUNAMEE, B.BUN, A.WONCODE, A.BCODE, B.BAMT, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(B.DELDATE,'YYYY-MM-DD') DELDATE ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_SUN A, ADMIN.BAS_SUT B ";
                SQL = SQL + ComNum.VBLF + "   WHERE A.SUNEXT = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = '" + txtSuNext.Text + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtHname.Text = dt.Rows[0]["SUNAMEK"].ToString().Trim();
                    txtBCode.Text = dt.Rows[0]["BCODE"].ToString().Trim();
                    switch (dt.Rows[0]["BUN"].ToString().Trim())
                    {
                        case "71":
                        case "72":
                        case "73":
                            txtAmt.Text = VB.Format(dt.Rows[0]["Bamt"].ToString().Trim(), "###,###,###,##0 ");
                            lblAmt.Visible = true;
                            txtAmt.Visible = true;
                            break;
                        default:
                            lblAmt.Visible = false;
                            txtAmt.Visible = false;
                            break;
                    }
                }

                //'심사기준 READ
                SQL = " SELECT  REMARK, DEPTCODE, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_SIMSAINFOR_MIR ";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + txtSuNext.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    FGstrRowid = dt.Rows[i]["ROWID"].ToString().Trim();
                    txtInfo.Text = VB.Replace(dt.Rows[i]["REMARK"].ToString().Trim(), "`", "'");
                    if (dt.Rows[0]["DeptCode"].ToString().Trim() == "**")
                    {
                        cboDept.Text = "**.전체";
                    }
                    else
                    {
                        cboDept.Text = dt.Rows[0]["DeptCode"].ToString().Trim() + "." + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[0]["DeptCode"].ToString().Trim());
                    }

                    btnDelete.Enabled = true;
                }

                dt.Dispose();
                dt = null;

                switch (clsType.User.Sabun)
                {
                    case "04349":
                    case "07834":
                    case "07843":
                    case "13537":
                    case "468":
                    case "02749":
                    case "15273":
                    case "19399":
                    case "21181":
                    case "13635":
                    case "22699":
                    case "22456":
                    case "33674":
                    case "27176":
                        btnSave.Enabled = true;
                        break;
                    default:
                        btnSave.Enabled = false;
                        break;
                }

                panMain.Enabled = true;
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

        private void btnSearchClick()
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SCREEN_CLEAR();
                
                SQL = "SELECT A.SUNEXT, B.SUNAMEK , A.ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_SIMSAINFOR_MIR A, ADMIN.BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  A.SUNEXT =B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.SUNEXT ";

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
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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
    }
}
