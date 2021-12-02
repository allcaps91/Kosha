using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmAntiHelp
    /// File Name : frmAntiHelp.cs
    /// Title or Description : 항생제 계열 찾기
    /// Author : 박창욱
    /// Create Date : 2017-06-01
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    ///     06-15 박창욱 : GstrHelpCode 지역변수 -> 전역변수 변경
    ///     06-19 박창욱 : delegate 사용
    /// </summary>
    /// <history>  
    /// VB\BuSuga08.frm(FrmAntiHelp) -> frmAntiHelp.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\busuga\BuSuga08.frm(FrmAntiHelp)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\busuga\\busuga.vbp
    /// </vbp>
    public partial class frmAntiHelp : Form
    {
        private string gstrHelpCode = "";

        public delegate void SetHelpCode(string strHelpCode);
        public event SetHelpCode rSetHelpCode;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmAntiHelp()
        {
            InitializeComponent();
        }

        private void frmAntiHelp_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            getData ();
        }

        private void getData()
        {
            int i = 0;
            string strList = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = " SELECT CODE, NAME FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN ='BAS_항생제계열' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODE ";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strList = VB.Left(dt.Rows[i]["CODE"].ToString().Trim() + VB.Space(5), 5);
                    strList = strList + dt.Rows[i]["NAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 0].Text = strList;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

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

            gstrHelpCode = VB.Left(ssView_Sheet1.Cells[e.Row, 0].Text, 4);
            rSetHelpCode(gstrHelpCode);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

    }
}
