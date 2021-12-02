using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmOcsCpDiagnosisCodeSearch : Form
    {
        //Messgae Send
        public delegate void SendMsg(string strCode, string strName);
        public event SendMsg rSendMsg;

        public frmOcsCpDiagnosisCodeSearch()
        {
            InitializeComponent();
        }

        private void frmOcsCpDiagnosisCodeSearch_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0) return;

            bool bSend = false;

            for (int i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                if (ssList_Sheet1.Cells[i, 0].Text.Trim() == "True")
                {
                    rSendMsg?.Invoke(ssList_Sheet1.Cells[i, 1].Text.Trim(), ssList_Sheet1.Cells[i, rdoVal0.Checked ? 2 : 3].Text.Trim());
                    bSend = true;
                }
            }

            if (bSend == false) return;

            Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetSearchData();
        }

        void GetSearchData()
        {
            if (txtSearch.Text.Length == 0) return;

            txtSearch.Text = txtSearch.Text.ToUpper();

            ssList_Sheet1.RowCount = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;


            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT ILLCODE, ILLNAMEE, ILLNAMEK  FROM BAS_ILLS";
                SQL += ComNum.VBLF + "WHERE ILLCLASS = '1'";
                if(rdoGubun0.Checked)
                {
                    SQL += ComNum.VBLF + "AND ILLCODE LIKE '%" + txtSearch.Text.Trim() + "%'";

                }
                else if(rdoGubun1.Checked)
                {
                    SQL += ComNum.VBLF + "AND ILLNAMEE LIKE '%" + txtSearch.Text.Trim() + "%'";

                }
                else if(rdoGubun2.Checked)
                {
                    SQL += ComNum.VBLF + "AND ILLNAMEK LIKE '%" + txtSearch.Text.Trim() + "%'";

                }
                SQL += ComNum.VBLF + "ORDER BY ILLCODE";

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
                    ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ILLNAMEE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ILLNAMEK"].ToString().Trim();
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

            if (e.Column == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssList, e.Column);
                return;
            }
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0) return;

            rSendMsg?.Invoke(ssList_Sheet1.Cells[e.Row, 1].Text.Trim(), ssList_Sheet1.Cells[e.Row, rdoVal0.Checked ? 2 : 3].Text.Trim());
            Close();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }
    }
}
