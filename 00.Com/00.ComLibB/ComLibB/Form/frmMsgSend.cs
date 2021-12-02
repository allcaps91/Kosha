using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmMsgSend : Form
    {
        string FGstrRowid;

        string FstrPano;
        string FstrSName;
        int FnWrtno;

        public frmMsgSend()
        {
            InitializeComponent();
        }

        private void frmMsgSend_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SCREEN_CLEAR();

            ss2_Sheet1.Columns[6].Visible = true;
            ss2_Sheet1.Columns[7].Visible = true;

            setCboSabun(cboSabun);
        }

        private void setCboSabun(ComboBox cbo)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = " SELECT SABUN, KORNAME  FROM KOSMOS_ADM.INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE TOIDAY IS NULL ";     //'재직자
                SQL = SQL + ComNum.VBLF + "   AND BUSE ='078201' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                cboSabun.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    cboSabun.Items.Add("******.전체");
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboSabun.Items.Add(dt.Rows[i]["SABUN"].ToString().Trim() + "." + dt.Rows[i]["KORNAME"].ToString().Trim());
                    }
                }
                cboSabun.SelectedIndex = 0;

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

        private void SCREEN_CLEAR()
        {
            txtInfo.Text = "";  //'& "  //'심사과 전달사항, 원무수납메시지

            FGstrRowid = "";
            btnSave.Enabled = false;
            panMain.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancelClick();
        }

        private void btnCancelClick()
        {
            SCREEN_CLEAR();

            txtPano.Text = "";
            lblPaName.Text = "";

            panMain.Enabled = false;
            ss1_Sheet1.RowCount = 0;
            ss2_Sheet1.RowCount = 0;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            newData();
        }

        private void newData()
        {
            if (FstrPano == "")
            {
                ComFunc.MsgBox("신규등록 할 환자를 선택하십시요.", "확인");
                return;
            }

            SCREEN_CLEAR();

            txtPano.Text = "";
            lblPaName.Text = "";
            btnSave.Enabled = true;
            panMain.Enabled = true;
            txtInfo.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            deleteData();
        }

        private bool deleteData()
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strROWID = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {                
                for (i = 0; i < ss2_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ss2_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strROWID = ss2_Sheet1.Cells[i, 7].Text;
                        SQL = " UPDATE KOSMOS_PMPA.BAS_OCSMEMO_SIM  SET DDATE = TRUNC(SYSDATE),JOBSABUN=" + clsType.User.Sabun + ", ENTDATE =SYSDATE    WHERE ROWID = '" + strROWID + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }
                    }
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제 하였습니다.");
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();

                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool saveData()
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strData = "";
            string strRowid = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인

                if (FstrPano == "")
                {
                    FstrPano = VB.Trim(txtPano.Text);
                }


                if (FstrPano == "")
                {
                    ComFunc.MsgBox("신규입력 할 환자를 선택하시기 바랍니다.", "확인");
                    return rtVal;
                }

                strData = txtInfo.Text;

                if (FGstrRowid == "")
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.BAS_OCSMEMO_SIM (  PANO, SNAME, MEMO, SDATE, DDATE,  GBN,JOBSABUN,ENTDATE) VALUES (";
                    SQL = SQL + ComNum.VBLF + " '" + FstrPano + "', '" + FstrSName + "',  '" + VB.Trim(txtInfo.Text) + "' , TRUNC(SYSDATE), '' , '1'," + clsType.User.Sabun + ",SYSDATE ) ";
                }
                else
                {
                    SQL = " UPDATE KOSMOS_PMPA.BAS_OCSMEMO_SIM SET MEMO = '" + VB.Trim(txtInfo.Text) + "' , JOBSABUN = " + clsType.User.Sabun + " ,ENTDATE=SYSDATE  ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrRowid + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
                                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();

                txtPano.Text = "";
                lblPaName.Text = "";

                ss1_Sheet1.RowCount = 0;
                ss2_Sheet1.RowCount = 0;

                btnCancelClick();
                btnNew.Enabled = false;

                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void OptView_0_CheckedChanged(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
        }

        private void ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (e.Row == -1 || e.Column == -1) return;

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                FstrPano = ss1_Sheet1.Cells[e.Row, 0].Text;
                FstrSName = ss1_Sheet1.Cells[e.Row, 1].Text;

                if (FstrPano == "") return;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "";
                SQL = " SELECT PANO, SNAME, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, TO_CHAR(DDATE,'YYYY-MM-DD') DDATE,";
                SQL = SQL + ComNum.VBLF + "  ROWID, GBN";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_OCSMEMO_SIM ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPano + "'";
                if (chkDel.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + " AND (DDATE IS NULL OR DDATE =''  )";
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY SDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss2_Sheet1.RowCount = dt.Rows.Count;
                    ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DDATE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                if (dt.Rows.Count == 0)
                {
                    newData();
                }
                else
                {
                    ss2CellClick(0,0);
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
            }

        }

        private void ss2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ss2CellClick(e.Row, e.Column);
        }

        private void ss2CellClick(int iRow, int iCol)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strROWID = "";

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                strROWID = ss2_Sheet1.Cells[iRow, 7].Text;
                FGstrRowid = strROWID;

                SQL = " SELECT MEMO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_OCSMEMO_SIM ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtInfo.Text = dt.Rows[0]["MEMO"].ToString().Trim();
                }

                btnSave.Enabled = true;
                panMain.Enabled = true;

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

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter) return;
            btnSearch.Focus();
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                if (txtPano.Text == "")
                {
                    lblPaName.Text = "";
                    return;
                }
                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

                SQL = " SELECT PANO , SNAME FROM KOSMOS_PMPA.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPano.Text + "' ";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    lblPaName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    FstrPano = txtPano.Text;
                    FstrSName = dt.Rows[0]["SNAME"].ToString().Trim();
                }
                else
                {
                    txtPano.Text = "";
                    lblPaName.Text = "";
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            viewData();
        }

        private void viewData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                if (OptView_1.Checked == true && txtPano.Text == "")
                {
                    ComFunc.MsgBox("등록번호를 넣고 조회하십시오", "확인");
                    return;
                }
                
                SQL = " SELECT A.PANO  APANO , A.SNAME,  B.PANO BPANO ,B.JOBSABUN ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT A, KOSMOS_PMPA.BAS_OCSMEMO_SIM B ";
                if (OptView_0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.PANO = B.PANO";
                    if (VB.Trim(txtPano.Text) != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND A.PANO = '" + txtPano.Text + "' ";
                    }
                    if (chkOK.Checked == false)
                    {
                        SQL = SQL + ComNum.VBLF + " AND (B.DDATE IS NULL OR B.DDATE =''  )";
                    }                    
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.PANO = B.PANO(+) ";
                    if (VB.Trim(txtPano.Text) != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND A.PANO = '" + txtPano.Text + "' ";
                    }
                }

                if (cboSabun.Text.Split('.')[1] != "전체")
                {
                    SQL = SQL + ComNum.VBLF + " AND b.JobSabun = " + cboSabun.Text.Split('.')[0] + " ";
                }
                
                if( optSort_0.Checked == true) SQL = SQL = SQL + ComNum.VBLF + " GROUP BY A.PANO, A.SNAME,   B.PANO ,B.JOBSABUN ";
                if ( optSort_1.Checked == true) SQL = SQL + ComNum.VBLF + " GROUP BY A.SNAME, A.PANO,   B.PANO ,B.JOBSABUN ";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ss1_Sheet1.RowCount = 0;
                ss2_Sheet1.RowCount = 0;                
                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["APANO"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["JobSabun"].ToString().Trim() + "," + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["JobSabun"].ToString().Trim());

                        if (dt.Rows[i]["BPANO"].ToString().Trim() != "")
                        {
                            ss1_Sheet1.Rows[i].BackColor = Color.FromArgb(255,233,233);
                        }
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
