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
    /// <summary> 환자 조회 </summary>
    public partial class frmSearchPatient : Form
    {
        public delegate void EventExit();
        public event EventExit rEventExit;

        public delegate void SendPano(string strPano);
        public event SendPano rSendPano;

        /// <summary> 환자 조회 </summary>
        public frmSearchPatient()
        {
            InitializeComponent();
        }

        void frmSearchPatient_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            txtName.Focus();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            rEventExit();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            int i = 0;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (txtName.Text == "")
                {
                    ComFunc.MsgBox("환자 성명을 입력 해주십시오!!   ", " ▶▷▶ 환자명 공란 ◀◁◀");
                    Cursor.Current = Cursors.Default;
                    txtName.Focus();
                    return;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.PANO, A.SNAME, B.SEX, B.JUMIN1,B.JUMIN2, b.JUMIN3, ";
                SQL = SQL + ComNum.VBLF + "        A.WARDCODE, A.ROOMCODE";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_MASTER A, BAS_PATIENT B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.PANO = B.PANO(+)";
                SQL = SQL + ComNum.VBLF + "    AND A.SNAME LIKE '%" + txtName.Text + "%'";
                SQL = SQL + ComNum.VBLF + "  ORDER BY A.SNAME";

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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();

                    if (dt.Rows[i]["JUMIN3"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[i]["JUMIN3"].ToString().Trim());
                    }
                    else
                    {
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["JUMIN1"].ToString().Trim() + "-" + dt.Rows[i]["JUMIN2"].ToString().Trim();
                    }

                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                txtName.Text = "";

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.KeyCode)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.Rows.Count - 1, ssView_Sheet1.Columns.Count - 1].BackColor = Color.FromArgb(255, 255, 255);

            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.Columns.Count - 1].BackColor = Color.FromArgb(200, 230, 255);

            if (e.Row == 0)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
            }
        }

        void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            rSendPano(ssView_Sheet1.Cells[e.Row, 0].Text.Trim());
        }
    }
}
