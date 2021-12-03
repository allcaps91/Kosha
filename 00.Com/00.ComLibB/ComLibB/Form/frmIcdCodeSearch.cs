using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmIcdCodeSearch : Form
    {

        public frmIcdCodeSearch()
        {
            InitializeComponent();
        }

        private void frmIcdCodeSearch_Load(object sender, EventArgs e)
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetSearchData();
        }

        void GetSearchData()
        {
            if (txtSearch.Text.Length == 0) return;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT * FROM ADMIN.BAS_ICDCODE";
                SQL += ComNum.VBLF + " WHERE ICDCODE Like '" + txtSearch.Text + "'";
                SQL += ComNum.VBLF + " OR ICDNAMEK Like '" + txtSearch.Text + "'";
                SQL += ComNum.VBLF + " OR ICDNAMEE Like '" + txtSearch.Text + "'";
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

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtSearch.Text == "") return;

            if (e.KeyCode == Keys.Enter)
            {
                GetSearchData();
            }
        }
    }
}
