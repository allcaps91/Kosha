using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmWonhangHelp : Form
    {
        /// Class Name      : ComLibB.dll
        /// File Name       : frmWonhangHelp.cs
        /// Description     : 원가 항목 찾기
        /// Author          : 김효성
        /// Create Date     : 2017-06-20
        /// Update History  : 델리게이트 선언
        /// </summary>
        /// <history>  
        /// VB\basic\busuga\BuSuga19.frm => frmWonhangHelp.cs 으로 변경함
        /// </history>
        /// <seealso> 
        /// VB\basic\busuga\BuSuga19.frm
        /// </seealso>
        /// <vbp>
        /// default : VB\basic\busuga\bvsuga.vbp
        /// </vbp>
        /// 
        string GstrHelpCode = "";

        //이벤트를 전달할 경우
        public delegate void SetCodeName (string strCode);
        public event SetCodeName rSetCodeName;

        //폼이 Close될 경우
        public delegate void EventClosed ();
        public event EventClosed rEventClosed;

        public frmWonhangHelp ()
        {
            InitializeComponent ();
        }

        public frmWonhangHelp (string strHelpCode)
        {
            InitializeComponent ();

            GstrHelpCode = strHelpCode;
        }

        private void frmWonhangHelp_Load (object sender , EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등


            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.Rows.Count = 0;

            try
            {
                SQL = "SELECT Hang,HangName FROM " + ComNum.DB_ERP + "WON_HANG ";
                SQL = SQL + ComNum.VBLF + "WHERE Hang >= '1000' AND Hang <= '1999' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY HangName, Hang ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    return;
                }
                
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight (-1 , ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells [i , 0].Text = VB.Left ((dt.Rows [i] ["Hang"]).ToString ().Trim () + VB.Space (5) , 5);
                    ssView_Sheet1.Cells [i , 1].Text = dt.Rows [i] ["HangName"].ToString ().Trim ();
                }

                GstrHelpCode = "";

                dt.Dispose ();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            GstrHelpCode = "";
            rEventClosed ();
        }

        private void ssView_CellDoubleClick (object sender , FarPoint.Win.Spread.CellClickEventArgs e)
        {
            GstrHelpCode = ssView_Sheet1.Cells [e.Row , 0].Text;

            rSetCodeName (GstrHelpCode);
            rEventClosed ();
        }
    }
}
