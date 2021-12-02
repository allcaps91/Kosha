using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmYAKHelp
    /// File Name : frmYAKHelp.cs
    /// Title or Description : 약품 분류 찾기
    /// Author : 박창욱
    /// Create Date : 2017-06-01
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    ///     06-15 박창욱 : GstrHelpCode 지역변수 -> 전역변수로 수정
    ///     06-19 박창욱 : delegate 사용
    /// </summary>
    /// <history>  
    /// VB\BuSuga07.frm(FrmYAKHelp) => frmYAKHelp.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\busuga\BuSuga07.frm(FrmYAKHelp)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\busuga\busuga.vbp
    /// </vbp>
    public partial class frmYAKHelp : Form
    {
        private string gstrHelpCode = "";

        public delegate void SetHelpCode(string strHelpCode);
        public event SetHelpCode rSetHelpCode;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmYAKHelp()
        {
            InitializeComponent();
        }

        private void frmYAKHelp_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { rEventClosed (); return; }//폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            GetData();
        }

        private void GetData()
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
                SQL = " SELECT CLASSCODE, CLASSNAME FROM " + ComNum.DB_PMPA + "BAS_CLASS";
                SQL = SQL + ComNum.VBLF + " ORDER BY CLASSCODE ";

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
                    strList = VB.Left(dt.Rows[i]["CLASSCODE"].ToString().Trim() + VB.Space(5), 5);
                    strList = strList + dt.Rows[i]["CLASSName"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 0].Text = strList;
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(ssView_Sheet1.RowCount == 0) { return; }

            if(e.ColumnHeader == true)
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
