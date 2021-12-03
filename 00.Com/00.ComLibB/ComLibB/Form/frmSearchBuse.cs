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

/// <summary>
/// Description : 부서코드 조회
/// Author : 김형범
/// Create Date : 2017.06.17
/// </summary>
/// <history>
/// 이벤트 점검 필요
/// </history>
namespace ComLibB
{
    /// <summary> 부서코드 찾기 </summary>
    public partial class frmSearchBuse : Form
    {
        public delegate void EventExit();
        public event EventExit rEventExit;

        public delegate void SendCodeName(string strCode, string strName);
        public event SendCodeName rSendCodeName;

        string mstrViewBun = "";
        string mstrViewDate = "";

        /// <summary> 부서코드 찾기 </summary>
        public frmSearchBuse()
        {
            InitializeComponent();
        }

        public frmSearchBuse(string strRetValue)
        {
            InitializeComponent();

            mstrViewBun = VB.Pstr(strRetValue, "{}", 1);
            mstrViewDate = VB.Pstr(strRetValue, "{}", 2);
        }

        void frmSearchBuse_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtBuName.Text = "";
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.Rows.Count - 1, ssView_Sheet1.Columns.Count - 1].Text = "";
            ssView_Sheet1.RowCount = 0;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                ssView_Sheet1.RowCount = 0;

                txtBuName.Text = txtBuName.Text.Trim();

                SQL = "";
                SQL = "SELECT BuCode,Name ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + " WHERE GBABC='Y' ";

                if (mstrViewBun == "ACC")
                {
                    SQL = SQL + ComNum.VBLF + "AND ACC='*' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "AND JAS='*' ";
                }

                if (mstrViewDate != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND (DelDate IS NULL OR DelDate>=TO_DATE('" + mstrViewDate + "','YYYY-MM-DD')) ";
                }

                if (txtBuName.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND Name LIKE '%" + txtBuName.Text + "%' ";
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY Name ";

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

                if (dt.Rows.Count == 1)
                {
                    string strBuseCode = dt.Rows[0]["BuCode"].ToString().Trim();  //부서코드
                    string strBuseName = dt.Rows[0]["name"].ToString().Trim();  //부서이름

                    rSendCodeName(strBuseCode, strBuseName);

                    dt.Dispose();
                    dt = null;

                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BuCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void txtBuName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            // rEventExit();
            this.Dispose();
        }

        void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strBuseCode = ssView_Sheet1.Cells[e.Row, 0].Text;  //부서코드
            string strBuseName = ssView_Sheet1.Cells[e.Row, 1].Text;  //부서이름

            rSendCodeName(strBuseCode, strBuseName);
        }
    }
}
