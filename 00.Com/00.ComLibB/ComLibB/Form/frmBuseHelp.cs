using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// Class Name      : ComLibB.dll
    /// File Name       : frmBuseHelp.cs
    /// Description     : 부서코드 찾기
    /// Author          : 김형범
    /// Create Date     : 2017-06-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// VB\Frm부서코드찾기.frm(FrmBuseHelp) => frmBuseHelp.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\nurse\nrcs\Frm부서코드찾기.frm(FrmBuseHelp)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\nurse\nrQI\nrQI.vbp
    /// seealso : VB\타병원\PSMHH\nurse\nrcs\nrcs.vbp
    /// </vbp>
    public partial class frmBuseHelp : Form
    {
        //이벤트를 전달할 경우
        public delegate void SetBuseHelp(string strBuCode, string strBuName); //Old : FstrView_Bun, FstrView_Date
        public event SetBuseHelp rSetBuseHelp;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        string mstrViewBun = ""; //FstrViewBun
        string mstrViewDate = ""; //FstrViewDate

        public frmBuseHelp()
        {
            InitializeComponent();
        }

        public frmBuseHelp(string strViewBun, string strViewDate)
        {
            InitializeComponent();
            mstrViewBun = strViewBun; //Trim(P(GstrRetValue, "{}", 1))  GstrRetValue가 글로벌
            mstrViewDate = strViewDate;  // Trim(P(GstrRetValue, "{}", 2))  GstrRetValue가 글로벌
        }

        private void frmBuseHelp_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            txtBuName.Text = "";
            ssView_Sheet1.RowCount = 0;

            //mstrViewBun = ""; // Trim(P(GstrRetValue, "{}", 1))  GstrRetValue가 글로벌
            //mstrViewDate = ""; // Trim(P(GstrRetValue, "{}", 2))  GstrRetValue가 글로벌
            //GstrRetValue = ""; GstrRetValue가 글로벌
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            string strBuCode = "";
            string strBuName = "";
            ssView_Sheet1.RowCount = 0;

            txtBuName.Text = txtBuName.Text.Trim();

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "BuCode,Name";
                SQL = SQL + ComNum.VBLF + "FROM";
                SQL = SQL + ComNum.VBLF + "ADMIN.BAS_BUSE";

                if (mstrViewBun == "ACC")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE ACC='*'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE JAS='*' ";
                }

                if (mstrViewDate != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND (DelDate IS NULL OR DelDate>=TO_DATE('" + mstrViewDate + "','YYYY-MM-DD')) ";
                }

                if (txtBuName.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + "AND Name LIKE '%" + txtBuName.Text + "%' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY Name ";
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

                //'자료가 1건만 있으면 자동 선택함
                if (dt.Rows.Count == 1)
                {
                    strBuCode = dt.Rows[0]["BuCode"].ToString().Trim(); // FstrView_Bun 글로벌
                    strBuName = dt.Rows[0]["Name"].ToString().Trim(); // FstrView_Date 글로벌
                    dt.Dispose();
                    dt = null;
                    rSetBuseHelp(strBuCode, strBuName);
                    return;
                }

                ssView_Sheet1.Rows.Count = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void txtBuName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }
            rSetBuseHelp(ssView_Sheet1.Cells[e.Row, 0].Text.Trim(), ssView_Sheet1.Cells[e.Row, 1].Text.Trim());
        }
    }
}
