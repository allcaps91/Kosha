using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComEmrBase
{
    /// <summary>
    /// 서식 생성기 : frmDesignUserFunction 와 동일함
    /// 아이템의 기본 함수
    /// </summary>
    public partial class frmItemUserFunction : Form
    {
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public delegate void SetFuncCode(string strFUNCGB, string strFuncCode, string strFuncName);
        public event SetFuncCode rSetFuncCode;

        string mFUNCGB = "01";

        public frmItemUserFunction()
        {
            InitializeComponent();
        }

        public frmItemUserFunction(string pFUNCGB)
        {
            InitializeComponent();
            mFUNCGB = pFUNCGB;
        }

        private void frmItemUserFunction_Load(object sender, EventArgs e)
        {
            GateFuncAll();
        }

        /// <summary>
        /// 등록된 함수 혹은 초기값을 불러온다
        /// </summary>
        private void GateFuncAll()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            txtFuncRmk1.Text = "";
            ssFunc_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + " FUNCCD, FUNCNAME, FUNCPARA, FUNCREMARK";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFUNCTION  ";
            SQL = SQL + ComNum.VBLF + "WHERE FUNCGB = '" + mFUNCGB + "'  ";
            SQL = SQL + ComNum.VBLF + "ORDER BY FUNCNAME ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

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
            ssFunc_Sheet1.RowCount = dt.Rows.Count;
            ssFunc_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssFunc_Sheet1.Cells[i, 0].Text = dt.Rows[i]["FUNCCD"].ToString().Trim();
                ssFunc_Sheet1.Cells[i, 1].Text = dt.Rows[i]["FUNCNAME"].ToString().Trim();
                ssFunc_Sheet1.Cells[i, 2].Text = dt.Rows[i]["FUNCREMARK"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void ssFunc_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssFunc_Sheet1.RowCount <= 0) return;
            if (e.ColumnHeader == true) return;

            txtFuncRmk1.Text = ssFunc_Sheet1.Cells[e.Row, 2].Text.Trim();
        }

        private void ssFunc_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssFunc_Sheet1.RowCount <= 0) return;
            if (e.ColumnHeader == true) return;

            txtFuncRmk1.Text = ssFunc_Sheet1.Cells[e.Row, 2].Text.Trim();
            string strFuncCode = "";
            string strFuncName = "";
            strFuncCode = ssFunc_Sheet1.Cells[e.Row, 0].Text.Trim();
            strFuncName = ssFunc_Sheet1.Cells[e.Row, 1].Text.Trim();
            rSetFuncCode(mFUNCGB, strFuncCode, strFuncName);
        }

    }
}
