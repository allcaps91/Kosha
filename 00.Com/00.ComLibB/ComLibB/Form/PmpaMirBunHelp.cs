using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComLibB
{
    /// Class Name      : PmpaMir.dll
    /// File Name       : PmpaMirHelpCode.cs
    /// Description     : 분류찾기
    /// Author          : 최익준
    /// Create Date     : 2017-08-30
    /// Update History  : 2017-11-01 유진호 델리게이트 추가    
    /// 
    /// </summary>
    /// <vbp>
    /// default : Z:\차세대 의료정보시스템\1-0 착수단계\1-5 참고 소스\포항성모병원 VB Source(2017.01.11)_분석설계제작용\basic\busuga\BuSuga05.frm
    /// </vbp>
    public partial class PmpaMirBunHelp : Form
    {
        string GstrHelpCode = "";
        
        //이벤트를 전달할 경우
        public delegate void SetHelpCode(string strHelpCode);
        public event SetHelpCode rSetHelpCode;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public PmpaMirBunHelp()
        {
            InitializeComponent();
        }

        public PmpaMirBunHelp(string rGstrHelpCode)
        {
            InitializeComponent();
            GstrHelpCode = rGstrHelpCode;
        }

        private void PmpaMirBunHelp_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strList = "";

            try
            {
                SQL = "";
                SQL = "SELECT CODE, NAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BUN";
                SQL = SQL + ComNum.VBLF + " WHERE JONG ='1' ";  // 분류
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
                    strList += dt.Rows[i]["Name"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 0].Text = strList;
                }
                dt.Dispose();
                dt = null;
                GstrHelpCode = "";
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

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

                        
            GstrHelpCode = VB.Left(ssView_Sheet1.Cells[e.Row, 0].Text, 4);
            rSetHelpCode(GstrHelpCode);
            rEventClosed();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            GstrHelpCode = "";
            rEventClosed();
        }


    }
}
