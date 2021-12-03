using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmComSupDrugListMagamLog : Form
    {
        string mstrBDate = string.Empty;
        public frmComSupDrugListMagamLog()
        {
            InitializeComponent();
        }

        public frmComSupDrugListMagamLog(string strBDate)
        {
            InitializeComponent();
            mstrBDate = strBDate;
        }

        private void frmComSupDrugListMagamLog_Load(object sender, EventArgs e)
        {
            lblTitle.Text = lblTitle.Text + " (" + mstrBDate + ")";
            READ_DATA();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void READ_DATA()
        {
            string SQL = "";
            DataTable dt = null;            
            int i = 0;
            string SqlErr = "";

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    BDATE, WARDCODE, CHASU, BUILD, A.SABUN, B.USERNAME ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_DRUG_MAGAMLOG A ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_USER B ";
                SQL = SQL + ComNum.VBLF + "    ON A.SABUN = B.IDNUMBER ";
                SQL = SQL + ComNum.VBLF + "WHERE BDATE = '"+ mstrBDate.Replace("-","") + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BUILD ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = ComFunc.FormatStrToDateEx(dt.Rows[i]["BDATE"].ToString().Trim(), "D", "-");
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["CHASU"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BUILD"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["USERNAME"].ToString().Trim();
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
    }
}
