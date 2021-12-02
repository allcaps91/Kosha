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

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmConsultView.cs
    /// Description     : 컨설트 내역 보기
    /// Author          : 이정현
    /// Create Date     : 2018-05-24
    /// <history> 
    /// 컨설트 내역 보기
    /// </history>
    /// <seealso>
    /// PSMH\Frm컨설트내역보기.frm
    /// </seealso>
    /// <vbp>
    /// default 		: 
    /// </vbp>
    /// </summary>
    public partial class frmConsultView2 : Form
    {
        private string GstrPtNo = "";
        private string GstrSName = "";

        public frmConsultView2()
        {
            InitializeComponent();
        }

        public frmConsultView2(string strPtNo, string strSName)
        {
            InitializeComponent();

            GstrPtNo = strPtNo;
            GstrSName = strSName;
        }

        private void frmConsultView2_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            lblFr0.Text = "";
            lblFr1.Text = "";
            lblFr2.Text = "";

            lblTo0.Text = "";
            lblTo1.Text = "";
            lblTo2.Text = "";

            txtPano.Text = GstrPtNo;
            lblSName.Text = GstrSName;

            READ_HISTORY();
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPano.Text.Trim() == "") { return; }
                txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");

                lblSName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, txtPano.Text.Trim());

                READ_HISTORY();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            READ_HISTORY();
        }

        private void READ_HISTORY()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssHistory_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.BDATE,'YY/MM/DD') BDATE, TO_CHAR(A.InpDate, 'YYYY-MM-DD') InpDate,";
                SQL = SQL + ComNum.VBLF + "     A.TODEPTCODE, A.TODRCODE, A.FRREMARK, A.TOREMARK, B.DRNAME, A.GBDEL, a.BInpID, a.InpID, a.FrDRCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ITRANSFER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PTNO = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.TODRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.BDATE desc  ";

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
                    ssHistory_Sheet1.RowCount = dt.Rows.Count;
                    ssHistory_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssHistory_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 1].Text = dt.Rows[i]["TODEPTCODE"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 3].Text = dt.Rows[i]["FRREMARK"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 4].Text = dt.Rows[i]["TOREMARK"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 5].Text = dt.Rows[i]["INPID"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 6].Text = dt.Rows[i]["BINPID"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 7].Text = dt.Rows[i]["TODRCODE"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 8].Text = dt.Rows[i]["INPDATE"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 9].Text = dt.Rows[i]["FRDRCODE"].ToString().Trim();

                        if (dt.Rows[i]["GBDEL"].ToString().Trim() == "*")
                        {
                            ssHistory_Sheet1.Cells[i, 0, i, ssHistory_Sheet1.ColumnCount - 1].ForeColor = Color.Red;
                        }
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssHistory_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) { return; }

            lblFr0.Text = "";
            lblFr1.Text = "";
            lblFr2.Text = "";

            lblTo0.Text = "";
            lblTo1.Text = "";
            lblTo2.Text = "";

            txtFrRemark.Text = ssHistory_Sheet1.Cells[e.Row, 3].Text.Trim();
            txtToRemark.Text = ssHistory_Sheet1.Cells[e.Row, 4].Text.Trim();

            lblFr1.Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, ssHistory_Sheet1.Cells[e.Row, 9].Text.Trim());
            lblTo1.Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, ssHistory_Sheet1.Cells[e.Row, 7].Text.Trim());

            lblFr0.Text = clsVbfunc.GetInSaName(clsDB.DbCon, ssHistory_Sheet1.Cells[e.Row, 6].Text.Trim());
            lblFr2.Text = ssHistory_Sheet1.Cells[e.Row, 0].Text.Trim();

            lblTo0.Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, ssHistory_Sheet1.Cells[e.Row, 7].Text.Trim());
            lblTo2.Text = ssHistory_Sheet1.Cells[e.Row, 8].Text.Trim();
        }
    }
}
