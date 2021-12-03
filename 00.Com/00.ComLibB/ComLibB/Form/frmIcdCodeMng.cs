using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmIcdCodeMng : Form
    {
        public frmIcdCodeMng()
        {
            InitializeComponent();
        }

        private void frmIcdCodeMng_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            cboIcdLevel.Items.Add("0");
            cboIcdLevel.Items.Add("1");
            cboIcdLevel.Items.Add("2");

            dtpEdate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            GetSearchData();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if (Save_Data() == false) return;

            GetSearchData();

            txtIcdNameE.Text = "";
            txtIcdNameK.Text = "";
            txtICode.Text = "";
            txtKcdCode.Text = "";

            dtpEdate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
        }

        bool Save_Data()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " SELECT * FROM " + ComNum.DB_PMPA + "BAS_ICDCODE ";
                SQL += ComNum.VBLF + " WHERE ICDCODE ='" + txtICode.Text + "'";
                SQL += ComNum.VBLF + " AND SDATE ='" + dtpSdate.Text.Replace("-", "") + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if(dt.Rows.Count == 0)
                {
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_ICDCODE (";
                    SQL = SQL + "\r" + " ICDCODE, ";
                    SQL = SQL + "\r" + " ICDCLASS, ";
                    SQL = SQL + "\r" + " ICDNAMEK, ";
                    SQL = SQL + "\r" + " ICDNAMEE, ";
                    SQL = SQL + "\r" + " KCDCODE,  ";
                    SQL = SQL + "\r" + " SDATE, ";
                    SQL = SQL + "\r" + " EDATE ) ";
                    SQL = SQL + "\r" + " VALUES ( ";
                    SQL = SQL + "\r '" + txtICode.Text + "',";
                    SQL = SQL + "\r '" + cboIcdLevel.Text + "',";
                    SQL = SQL + "\r '" + txtIcdNameK.Text + "',";
                    SQL = SQL + "\r '" + txtIcdNameE.Text + "',";
                    SQL = SQL + "\r '" + txtKcdCode.Text + "',";
                    SQL = SQL + "\r '" + dtpSdate.Text.Replace("-", "") + "',";
                    SQL = SQL + "\r '" + dtpEdate.Text.Replace("-", "") + "' )";
                }
                else
                {
                    if(ComFunc.MsgBoxQ("이미 데이터가 있습니다 데이터를 수정 하시겠습니까?", "확인") == DialogResult.No)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return rtnVal;
                    }

                    SQL = " UPDATE " + ComNum.DB_PMPA + "BAS_ICDCODE SET ";
                    SQL += ComNum.VBLF + " ICDCODE ='" + txtICode.Text + "', ";
                    SQL += ComNum.VBLF + " ICDCLASS ='" + cboIcdLevel.Text + "', ";
                    SQL += ComNum.VBLF + " ICDNAMEK ='" + txtIcdNameK.Text + "', ";
                    SQL += ComNum.VBLF + " ICDNAMEE ='" + txtIcdNameE.Text + "', ";
                    SQL += ComNum.VBLF + " KCDCODE ='" + txtKcdCode.Text + "', ";
                    SQL += ComNum.VBLF + " SDATE ='" + dtpSdate.Text.Replace("-", "") + "', ";
                    SQL += ComNum.VBLF + " EDATE ='" + dtpEdate.Text.Replace("-", "") + "'";
                    SQL += ComNum.VBLF + " WHERE SDATE ='" + dtpSdate.Text.Replace("-", "") + "'";
                    SQL += ComNum.VBLF + " AND ICDCODE ='" + txtICode.Text + "'";
                }

                dt.Dispose();
                dt = null;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (Delete_Data() == false) return;

            GetSearchData();
        }

        bool Delete_Data()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " DELETE " + ComNum.DB_PMPA + "BAS_ICDCODE ";
                SQL += ComNum.VBLF + " WHERE ICDCODE ='" + ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 0].Text + "'";
                SQL += ComNum.VBLF + " AND SDATE ='" + dtpSdate.Text.Replace("-", "") + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제 하였습니다.");
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetSearchData(txtSearch.Text.Trim());
        }

        void GetSearchData(string strICode = "")
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT * FROM ADMIN.BAS_ICDCODE";
                if(strICode != "")
                {
                    SQL += ComNum.VBLF + " WHERE ICDCODE ='" + strICode + "'";
                }
                SQL += ComNum.VBLF + " ORDER BY ICDCODE";

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
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssList_Sheet1.RowCount = dt.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ICDCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ICDCLASS"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ICDNAMEK"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ICDNAMEE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["KCDCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["EDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssList, e.Column);
                return;
            }

            ssList_Sheet1.Cells[0, 0, ssList_Sheet1.RowCount - 1, ssList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssList_Sheet1.Cells[e.Row, 0, e.Row, ssList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0) return;

            txtICode.Text    = ssList_Sheet1.Cells[e.Row, 0].Text;
            cboIcdLevel.Text = ssList_Sheet1.Cells[e.Row, 1].Text;
            txtIcdNameK.Text = ssList_Sheet1.Cells[e.Row, 2].Text;
            txtIcdNameE.Text = ssList_Sheet1.Cells[e.Row, 3].Text;
            txtKcdCode.Text  = ssList_Sheet1.Cells[e.Row, 4].Text;
            dtpSdate.Value   = Convert.ToDateTime(ComFunc.FormatStrToDate(ssList_Sheet1.Cells[e.Row, 5].Text, "D"));
            dtpEdate.Value   = Convert.ToDateTime(ComFunc.FormatStrToDate(ssList_Sheet1.Cells[e.Row, 6].Text, "D"));
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtSearch.Text == "") return;

            if(e.KeyCode == Keys.Enter)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                GetSearchData();
            }
        }
    }
}
