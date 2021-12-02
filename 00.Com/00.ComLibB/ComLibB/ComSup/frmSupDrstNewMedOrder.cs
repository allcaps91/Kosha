using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstNewMedOrder.cs
    /// Description     : 신약처방조회
    /// Author          : 이정현
    /// Create Date     : 2017-11-29
    /// <history> 
    /// 신약처방조회
    /// </history>
    /// <seealso>
    /// PSMH\drug\drinfo\Frm신약처방조회.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drinfo\drinfo.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstNewMedOrder : Form
    {
        public frmSupDrstNewMedOrder()
        {
            InitializeComponent();
        }

        private void frmSupDrstNewMedOrder_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpFDate.Value = Convert.ToDateTime(Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddYears(-1).ToString("yyyy-01-01"));
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            dtpFDate2.Value = Convert.ToDateTime(Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddYears(-1).ToString("yyyy-01-01"));
            dtpTDate2.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            txtSuName.Text = "";
            txtSuNext.Text = "";

            ssLeft_Sheet1.RowCount = 0;
            ssRight_Sheet1.RowCount = 0;

            GetData();
        }

        private void btnSearchLeft_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssLeft_Sheet1.RowCount = 0;
            ssRight_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUNEXT, HNAME, TO_CHAR(SDATE,'YYYY-MM-DD') AS SDATE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW ";
                SQL = SQL + ComNum.VBLF + "     WHERE SDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND SDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssLeft_Sheet1.RowCount = dt.Rows.Count;
                    ssLeft_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssLeft_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                        ssLeft_Sheet1.Cells[i, 1].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                        ssLeft_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SDATE"].ToString().Trim();
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dtpFDate.Value = Convert.ToDateTime(Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddYears(-1).ToString("yyyy-01-01"));
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            dtpFDate2.Value = Convert.ToDateTime(Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddYears(-1).ToString("yyyy-01-01"));
            dtpTDate2.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            txtSuName.Text = "";
            txtSuNext.Text = "";

            ssLeft_Sheet1.RowCount = 0;
            ssRight_Sheet1.RowCount = 0;
        }

        private void txtSuNext_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtSuNameLoad();
            }
            
        }

        private void txtSuNext_Leave(object sender, EventArgs e)
        {
            txtSuNameLoad();
        }

        private void txtSuNameLoad()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            txtSuName.Text = "";

            if (txtSuNext.Text.Trim() == "") { return; }

            txtSuNext.Text = txtSuNext.Text.ToUpper();

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUNEXT, HNAME, TO_CHAR(SDATE,'YYYY-MM-DD') AS SDATE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW ";
                SQL = SQL + ComNum.VBLF + "     WHERE SUNEXT = '" + txtSuNext.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    txtSuName.Text = dt.Rows[0]["HNAME"].ToString().Trim();

                    if (dt.Rows[0]["SDATE"].ToString().Trim() != "")
                    {
                        dtpFDate2.Value = Convert.ToDateTime(dt.Rows[0]["SDATE"].ToString().Trim());
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSearchRight_Click(object sender, EventArgs e)
        {
            GetRight();
        }

        private void GetRight()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssRight_Sheet1.RowCount = 0;

            if (txtSuNext.Text.Trim() == "")
            {
                ComFunc.MsgBox("약품코드를 입력해주세요.");
                return;
            }

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(MIN(A.BDATE),'YYYY-MM-DD') AS BDATE, A.PANO, B.SNAME, 'O' AS IO, A.DEPTCODE, C.DRNAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B, " + ComNum.DB_PMPA + "BAS_DOCTOR C";
                SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE >= TO_DATE('" + dtpFDate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND A.BDATE <= TO_DATE('" + dtpTDate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND A.SUNEXT = '" + txtSuNext.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+)";
                SQL = SQL + ComNum.VBLF + "         AND A.DRCODE = C.DRCODE";
                SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, B.SNAME, A.DEPTCODE, C.DRNAME";
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(MIN(A.BDATE),'YYYY-MM-DD') AS BDATE, A.PANO, B.SNAME, 'I' AS IO, A.DEPTCODE, C.DRNAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B, " + ComNum.DB_PMPA + "BAS_DOCTOR C";
                SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE >= TO_DATE('" + dtpFDate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND A.BDATE <= TO_DATE('" + dtpTDate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND A.SUNEXT = '" + txtSuNext.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+)";
                SQL = SQL + ComNum.VBLF + "         AND A.DRCODE = C.DRCODE";
                SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, B.SNAME, A.DEPTCODE, C.DRNAME";
                SQL = SQL + ComNum.VBLF + "ORDER BY BDATE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssRight_Sheet1.RowCount = dt.Rows.Count;
                    ssRight_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssRight_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssRight_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssRight_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssRight_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IO"].ToString().Trim();
                        ssRight_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssRight_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssRight_Sheet1.Cells[i, 6].Text = " ";
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssLeft_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            txtSuNext.Text = ssLeft_Sheet1.Cells[e.Row, 0].Text.Trim();
            txtSuName.Text = ssLeft_Sheet1.Cells[e.Row, 1].Text.Trim();
            dtpFDate2.Value = Convert.ToDateTime(ssLeft_Sheet1.Cells[e.Row, 2].Text.Trim());

            if (dtpFDate2.Value.AddMonths(2) <= Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")))
            {
                dtpTDate2.Value = dtpFDate2.Value.AddMonths(2);
            }
            else
            {
                dtpTDate2.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            }

            GetRight();
        }
    }
}
