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
    public partial class frmILLHCodeCancer : Form
    {
        private string GstrROWID = "";

        public frmILLHCodeCancer()
        {
            InitializeComponent();
        }

        private void frmILLHCode_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            FormClear();

            GetData(26, "D");

        }

        private void FormClear()
        {
            txtIllCode_S.Text = "";

            txtILLCode.Text = "";
            txtIllNameK.Text = "";
            txtIllNameE.Text = "";
            txtGiJun.Text = "";
            txtExcep.Text = "";

            ssView_Sheet1.RowCount = 0;

            ssGB_Sheet1.Cells[0, 1].Text = "";
            ssGB_Sheet1.Cells[1, 1].Text = "";
            ssGB_Sheet1.Cells[2, 1].Text = "";
            ssGB_Sheet1.Cells[3, 1].Text = "";
            ssGB_Sheet1.Cells[4, 1].Text = "";
            ssGB_Sheet1.Cells[5, 1].Text = "";
            ssGB_Sheet1.Cells[6, 1].Text = "";
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (txtIllCode_S.Text.Trim() == "")
            {
                return;
            }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT ILLCODE, ILLNAMEK, ROWID  ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_ILLS_CANCER ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "   AND ILLCODE = '" + txtIllCode_S.Text + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ILLNAMEK"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int intIndex = Convert.ToInt32(VB.Val(VB.Right(((Button)sender).Name, 2)));
            string strText = ((Button)sender).Text;

            GetData(intIndex, strText);
        }

        private void GetData(int intIndex, string strText)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strIllCode = strText + "%";

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT ILLCODE, ILLNAMEK, ROWID   ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_ILLS_CANCER ";
                if (intIndex != 26)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE ILLCODE LIKE  ('" + strIllCode + "' ) ";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY ILLCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ILLNAMEK"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            if (e.ColumnHeader == true || e.RowHeader == true)
            {
                return;
            }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            GstrROWID = ssView_Sheet1.Cells[e.Row, 2].Text;

            try
            {
                SQL = "";
                SQL = "SELECT ILLCODE, ILLNAMEK, ILLSEQ, ILLNAMEE, ";
                SQL = SQL + ComNum.VBLF + " GIJUN, GBN1, GBN2, GBN3, GBN4, GBN5, GBN6, JIGUN, EXCEP ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_ILLS_CANCER ";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + GstrROWID + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                txtILLCode.Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                txtIllNameK.Text = dt.Rows[0]["ILLNAMEK"].ToString().Trim();
                txtIllNameE.Text = dt.Rows[0]["ILLNAMEE"].ToString().Trim();
                txtGiJun.Text = dt.Rows[0]["GIJUN"].ToString().Trim();

                ssGB_Sheet1.Cells[0, 1].Text = dt.Rows[0]["GBN1"].ToString().Trim();
                ssGB_Sheet1.Cells[1, 1].Text = dt.Rows[0]["GBN2"].ToString().Trim();
                ssGB_Sheet1.Cells[2, 1].Text = dt.Rows[0]["GBN3"].ToString().Trim();
                ssGB_Sheet1.Cells[3, 1].Text = dt.Rows[0]["GBN4"].ToString().Trim();
                ssGB_Sheet1.Cells[4, 1].Text = dt.Rows[0]["GBN5"].ToString().Trim();
                ssGB_Sheet1.Cells[5, 1].Text = dt.Rows[0]["GBN6"].ToString().Trim();
                ssGB_Sheet1.Cells[6, 1].Text = dt.Rows[0]["JIGUN"].ToString().Trim();

                txtExcep.Text = dt.Rows[0]["EXCEP"].ToString().Trim();

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

        private void btnGuide_Click(object sender, EventArgs e)
        {
            frmILLHCodeCancerHelp fm = new frmILLHCodeCancerHelp();
            fm.Show();
        }
    }
}
