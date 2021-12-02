using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaJengSanYearEndIssue.cs
    /// Description     : 연말정산 발급 내역
    /// Author          : 이정현
    /// Create Date     : 2018-08-16
    /// <history> 
    /// 연말정산 발급 내역
    /// </history>
    /// <seealso>
    /// PSMH\OPD\jengsan\Frm연말정산발급.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\OPD\jengsan\jengsan.vbp
    /// </vbp>
    /// </summary>
    public partial class frmPmpaJengSanYearEndIssue : Form
    {
        public frmPmpaJengSanYearEndIssue()
        {
            InitializeComponent();
        }

        private void frmPmpaJengSanYearEndIssue_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            ssView_Sheet1.RowCount = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            int intTotCnt = 0;

            ssView_Sheet1.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.SABUN, A.PRTCNT, B.KORNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_PRTCNT A, " + ComNum.DB_ERP + "INSA_MST B ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.JOBDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.SABUN <> '4349' ";
                SQL = SQL + ComNum.VBLF + "         AND A.SABUN = B.SABUN  ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SABUN";

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
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PRTCNT"].ToString().Trim();

                        intTotCnt += (int)VB.Val(dt.Rows[i]["PRTCNT"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "총 건 수";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = intTotCnt.ToString("##,###");

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
    }
}
