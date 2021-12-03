using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : 포스코 통보서 의사설정
/// Author : 박병규
/// Create Date : 2017.05.31
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaSetPoscoDr : Form
    {
        clsQuery CQ = new clsQuery();
        ComFunc CF = new ComFunc();
        DataTable Dt = new DataTable();
        string SQL = "";
        string SqlErr = "";
        int intRowCnt = 0;

        string strGubun = "";

        public frmPmpaSetPoscoDr()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.rdoGubun0.Click += new EventHandler(rdoGubun_Click);
            this.rdoGubun1.Click += new EventHandler(rdoGubun_Click);
            this.rdoGubun2.Click += new EventHandler(rdoGubun_Click);

            this.txtDrSabun.GotFocus += new EventHandler(txtDrSabun_GotFocus);
        }

        private void txtDrSabun_GotFocus(object sender, EventArgs e)
        {
            txtDrSabun.SelectAll();
            txtDrSabun.SelectionLength = txtDrSabun.Text.Trim().Length;
        }

        private void rdoGubun_Click(object sender, EventArgs e)
        {
            if (sender == this.rdoGubun0) { strGubun = "포스코통보서_검사_의사설정_0"; Get_DataLoad(); }
            if (sender == this.rdoGubun1) { strGubun = "포스코통보서_검사_의사설정_1"; Get_DataLoad(); }
            if (sender == this.rdoGubun2) { strGubun = "포스코통보서_검사_의사설정_2"; Get_DataLoad(); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ComFunc.SetAllControlClear(pnlBody);
            Get_DataLoad();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPmpaSetPoscoDr_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            frmPmpaSetPoscoDr frm = new frmPmpaSetPoscoDr();
            ComFunc.Form_Center(frm);

            strGubun = "포스코통보서_검사_의사설정_0";
            Get_DataLoad();
        }

        private void Get_DataLoad()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ComFunc.SetAllControlClear(pnlBody);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT A.SABUN, B.KORNAME, A.ROWID";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO_DR_LIST A,";
                SQL += ComNum.VBLF +            ComNum.DB_ERP + "INSA_MST B";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND A.GUBUN = '" + strGubun + "'";
                SQL += ComNum.VBLF + "    AND A.SABUN = B.SABUN";
                SQL += ComNum.VBLF + "    AND (B.TOIDAY IS NULL OR B.TOIDAY < TRUNC(SYSDATE))";
                SQL += ComNum.VBLF + "  ORDER BY B.KORNAME ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Dt.Dispose();
                    Dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssList_Sheet1.RowCount = Dt.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["SABUN"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["KORNAME"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                }
                Dt.Dispose();
                Dt = null;

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ComFunc.SelectRowColor(ssList_Sheet1, e.Row);

            txtDrSabun.Text = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
            txtDrName.Text = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();
            txtRowid.Text = ssList_Sheet1.Cells[e.Row, 2].Text.Trim();
        }

        private void txtDrSabun_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (txtRowid.Text == "") { CheckUp_OverlapData(); }

                if (txtDrSabun.Text == "") return;

                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SABUN, KORNAME";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_ERP + "INSA_MST";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1";
                    SQL += ComNum.VBLF + "    AND SABUN = '" + txtDrSabun.Text + "'";
                    SQL += ComNum.VBLF + "    AND (TOIDAY IS NULL OR TOIDAY < TRUNC(SYSDATE))";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                        return;
                    }

                    if (Dt.Rows.Count == 0)
                    {
                        txtDrName.Text = "";
                        ComFunc.MsgBox("사원번호가 존재하지 않거나, 퇴직한 사원입니다.", "알림");
                        Dt.Dispose();
                        Dt = null;
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    else if (Dt.Rows.Count == 1)
                    {
                        txtDrName.Text = Dt.Rows[0]["KORNAME"].ToString().Trim();
                    }
                    else if (Dt.Rows.Count > 1)
                    {
                        txtDrName.Text = "";
                        ComFunc.MsgBox("중복된 사원번호입니다.", "알림");
                        Dt.Dispose();
                        Dt = null;
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    Dt.Dispose();
                    Dt = null;

                    Cursor.Current = Cursors.Default;

                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SendKeys.Send("{TAB}");
            }
        }

        private void CheckUp_OverlapData()
        {
            String strChkCode = "";

            for (int i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                strChkCode = ssList_Sheet1.Cells[i, 0].Text.Trim();

                if (strChkCode == txtDrSabun.Text.Trim())
                {
                    if (i != ssList_Sheet1.ActiveRowIndex && strChkCode != "")
                    {
                        ComFunc.MsgBox(i + 1 + "번째줄의 코드와 중복입니다.", "경고");
                        txtDrSabun.Text = "";
                        txtDrName.Text = "";
                        txtDrSabun.Focus();
                        return;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) { return; }     //권한확인


            if (txtDrSabun.Text == "")
            {
                ComFunc.MsgBox("의사 사원번호를 입력하시기 바랍니다.", "알림");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
             
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (txtRowid.Text == "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO_DR_LIST";
                    SQL += ComNum.VBLF + "        (GUBUN, SABUN, REG_DATE)";
                    SQL += ComNum.VBLF + " VALUES ('" + strGubun + "',";
                    SQL += ComNum.VBLF + "         '" + txtDrSabun.Text + "',";
                    SQL += ComNum.VBLF + "         TRUNC(SYSDATE))";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO_DR_LIST";
                    SQL += ComNum.VBLF + "    SET SABUN = '" + txtDrSabun.Text + "'";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1";
                    SQL += ComNum.VBLF + "    AND ROWID = '" + txtRowid.Text + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);
                }

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장되었습니다.", "알림");
                Cursor.Current = Cursors.Default;

                Get_DataLoad();
            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) { return; }     //권한확인

            if (txtRowid.Text == "")
            {
                ComFunc.MsgBox("삭제할 사원번호를 선택하시기 바랍니다.", "알림");
                return;
            }

            DialogResult result = ComFunc.MsgBoxQ( txtDrName.Text +  " 님을 삭제하시겠습니까?", "삭제요청", MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                 
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO_DR_LIST";
                    SQL += ComNum.VBLF + "  WHERE ROWID = '" + txtRowid.Text + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("삭제되었습니다.", "알림");
                    Cursor.Current = Cursors.Default;

                    Get_DataLoad();
                }

                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
        }


    }
}
