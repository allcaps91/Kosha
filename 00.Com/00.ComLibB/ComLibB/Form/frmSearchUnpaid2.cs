using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Drawing;

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmSearchUnpaid2
    /// File Name : frmSearchUnpaid2.cs
    /// Title or Description : 비급여고지항목 조정
    /// Author : 김해수
    /// Create Date : 2020-12-18
    /// Update Histroy : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso> 
    /// </seealso>
    /// <vbp>
    /// </vbp>
    public partial class frmSearchUnpaid2 : Form
    {
        public frmSearchUnpaid2()
        {
            InitializeComponent();
        }

        void frmSearchUnpaid_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ReadSugastrGbSelfHang();

            ssView_Sheet1.Columns[3].Visible = false;
        }

        void ReadSugastrGbSelfHang()
        {
            int i = 0;
            string SQL = string.Empty;
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                ssView_Sheet1.RowCount = 0;

                SQL = "";
                SQL = " SELECT A.SUGBF, B.SUNEXT, B.SUNAMEK, B.ROWID, A.BAMT, (a.BAMT*1.25) BBAMT,A.IAMT, B.BCODE ,a.SUGBE ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_SUT A, BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "  WHERE B.GBSELFHANG = 'Y' ";
                SQL = SQL + ComNum.VBLF + "    AND A.SUNEXT = B.SUNEXT";

                if (optYak.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.BUN IN ('11','12') ";
                }
                else if (optJusa.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.BUN IN ('20') ";
                }
                else if (optGumsa.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.BUN IN ('41','42','43','44','45','46','47','48','49','50','51','52','53','54','55','56','57','58','59','60','61','62','63','64') ";
                }
                else if (optEtc.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.BUN NOT IN ('11','12','13','20','41','42','43','44','45','46','47','48','49','50','51','52','53','54','55','56','57','58','59','60','61','62','63','64') ";
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY B.SUNEXT ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장 
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = (dt.Rows[i]["SugbF"].ToString().Trim() == "1" ? "◎" : "");
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BAMT"].ToString().Trim(); 
                    if(dt.Rows[i]["SUGBE"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["BBAMT"].ToString().Trim(); //원금액 보험수가 * 25%
                    }
                    else
                    {
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["BAMT"].ToString().Trim();  //원금액 보험수가 
                    }

                    ssView_Sheet1.Columns[5].BackColor = Color.FromArgb(250, 244, 192);

                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["IAMT"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["BCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SUGBE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void opt_CheckedChanged(object sender, EventArgs e)
        {
            ReadSugastrGbSelfHang();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            ReadSugastrGbSelfHang();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void btnSelfInsert_Click(object sender, EventArgs e)
        {
            frmSearchUnpaid3 frm = new frmSearchUnpaid3();
            frm.Show();
        }
    }
}
