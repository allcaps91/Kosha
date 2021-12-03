using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmSearchUnpaid
    /// File Name : frmSearchUnpaid.cs
    /// Title or Description : 비급여항목 조정
    /// Author : 박창욱
    /// Create Date : 2017-06-01
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    ///     06-15 박창욱 : 불필요한 Using 제거
    /// </summary>
    /// <history>  
    /// VB\FrmBuSuga04.frm(FrmSelf) -> frmSearchUnpaid.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\busuga\FrmBuSuga04.frm(FrmSelf)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\busuga\\busuga.vbp
    /// </vbp>
    public partial class frmSearchUnpaid : Form
    {
        public frmSearchUnpaid()
        {
            InitializeComponent();
        }

        void frmSearchUnpaid_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ReadSugaGbsugbf();

            ssView_Sheet1.Columns[3].Visible = false;
        }

        void ReadSugaGbsugbf()
        {
            int i = 0;
            string SQL = string.Empty;
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                ssView_Sheet1.RowCount = 0;

                SQL = "";
                SQL = " SELECT A.SUGBF, B.SUNEXT, B.SUNAMEK, A.ROWID  ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_SUT A, BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "  WHERE B.GBSUGBF ='1' ";
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
            ReadSugaGbsugbf();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            ReadSugaGbsugbf();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) { return; }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            string SQL = string.Empty;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (e.Column != 0)
                {
                    return;
                }

                string strROWID = string.Empty;
                string strSugbF = string.Empty;

                ssView_Sheet1.Cells[e.Row, 0].Text = (ssView_Sheet1.Cells[e.Row, 0].Text == "◎" ? "" : "◎");

                strSugbF = (ssView_Sheet1.Cells[e.Row, 0].Text == "◎" ? "1" : "0");
                strROWID = ssView_Sheet1.Cells[e.Row, 3].Text;

                if (strROWID != "")
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "BAS_SUT SET SUGBF = '" + strSugbF + "'  ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }
    }
}
