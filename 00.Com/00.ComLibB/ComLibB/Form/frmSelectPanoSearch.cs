using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmSelectPanoSearch
    /// File Name : frmSelectPanoSearch.cs
    /// Title or Description : 입력할 수술환자 선택 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-02
    /// <history> 
    /// </history>
    /// </summary>
    public partial class frmSelectPanoSearch : Form
    {
        //TODO:clspublic에 없는 글로벌 변수
        public string GstrOpDate = "";
        public string GstrTime = "";
        public string GstrTestPano = "";

        public frmSelectPanoSearch()
        {
            InitializeComponent();
        }

        private void frmSelectPanoSearch_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            dtpDate.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ViewData() == false) return;
        }

        private bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            btnView.Enabled = false;
            ss1_Sheet1.Rows.Count = 0;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                GstrOpDate = dtpDate.Text;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Pano,Sname,DeptCode,IpdOpd,OpGuBun, OpTimeFROM         ";
                SQL = SQL + ComNum.VBLF + "   FROM ORAN_MASTER                                            ";
                SQL = SQL + ComNum.VBLF + "  WHERE OPDATE = TO_DATE('" + GstrOpDate + "','YYYY-MM-DD')    ";
                if (optSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY Sname,Pano                                      ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY Pano                                            ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }

                ss1_Sheet1.Rows.Count = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    if (dt.Rows[i]["OpGuBun"].ToString().Trim() == "1")
                    {
                        ss1_Sheet1.Cells[i, 4].Text = "추가수술";
                    }
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["OpTimeFROM"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                btnView.Enabled = true;
                ss1.Focus();
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            clsPublic.GstrPANO = "";
            //TODO:처리해야되는 글로벌변수
            GstrOpDate = "";
            this.Close();
        }

        private void dtpDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == 0 || e.Row == 0) { return; }
            clsPublic.GstrPANO = ss1_Sheet1.Cells[e.Row, e.Column].Text;
            //TODO:처리해야될 글로벌 변수
            GstrTime = ss1_Sheet1.Cells[e.Row, e.Column].Text;
            GstrOpDate = dtpDate.Text;
            GstrTestPano = clsPublic.GstrPANO;
            this.Hide();
        }
    }
}
