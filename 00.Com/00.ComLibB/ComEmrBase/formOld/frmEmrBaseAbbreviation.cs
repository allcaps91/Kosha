using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmEmrBaseAbbreviation.cs
    /// Description     : [표준약어지침 DB 조회] ※ 대소문자 구분없이 검색됩니다.
    /// Author          : 이정현
    /// Create Date     : 2018-05-23
    /// <history> 
    /// [표준약어지침 DB 조회] ※ 대소문자 구분없이 검색됩니다.
    /// </history>
    /// <seealso>
    /// PSMH\mid\midu000\Frm표준약어조회.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\mid\midu000.vbp
    /// </vbp>
    /// </summary>
    public partial class frmEmrBaseAbbreviation : Form
    {
        public frmEmrBaseAbbreviation()
        {
            InitializeComponent();
        }

        private void frmEmrBaseAbbreviation_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssView_Sheet1.RowCount = 0;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmEmrBaseAbbreviationDes frm = new frmEmrBaseAbbreviationDes();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
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

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SEQNO, CODE, YNAME, KNAME, GUBUN1, GUBUN2, ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MID_STD_WORD ";
                SQL = SQL + ComNum.VBLF + "     WHERE SeqNo IS NOT NULL ";

                if (rdoGbn0.Checked == true) { SQL = SQL + ComNum.VBLF + "          AND ( CODE LIKE '%" + txtName.Text + "%' OR CODE LIKE '%" + txtName.Text.ToUpper() + "%' OR CODE LIKE '%" + txtName.Text.ToUpper() + "%' )  "; }
                if (rdoGbn1.Checked == true) { SQL = SQL + ComNum.VBLF + "          AND ( YNAME LIKE '%" + txtName.Text + "%' OR YNAME LIKE '%" + txtName.Text.ToUpper() + "%' OR YNAME LIKE '%" + txtName.Text.ToUpper() + "%' ) "; }
                if (rdoGbn2.Checked == true) { SQL = SQL + ComNum.VBLF + "          AND KNAME LIKE '%" + txtName.Text + "%' "; }
                
                SQL = SQL + ComNum.VBLF + "ORDER BY CODE ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
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
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["YNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["KNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["GUBUN1"].ToString().Trim();
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

                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";

            strFont1 = "/fn\"맑은 고딕\" /fz\"20\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"11\" /fb1 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "표준약어지침 DB" + "/f1/n";
            strHead2 = "/l/f2" + "출력일자 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + "/f2/n";

            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 20;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.Margin.Header = 10;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.UseSmartPrint = false;
            ssView_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssView_Sheet1.PrintInfo.Preview = false;
            ssView.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtName.Text.Trim() == "") { return; }

                GetData();
            }
        }
    }
}
