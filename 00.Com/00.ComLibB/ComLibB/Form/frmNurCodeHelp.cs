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
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmNurCodeHelp.cs
    /// Description     : 간호활동 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06
    /// Update History  : try - catch문 수정, GstrHelpCode를 던져주는 델리게이트사용 
    /// <history>       
    /// D:\타병원\PSMHH\basic\busuga\BuSuga26.frm(FrmNurCodeHelp) => frmNurCodeHelp.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\busuga\BuSuga26.frm(FrmNurCodeHelp)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busuga\busuga.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmNurCodeHelp : Form
    {
        //이벤트를 전달할 경우
        public delegate void SetHelpCode(string strHelpCode);
        public event SetHelpCode rSetHelpCode;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmNurCodeHelp()
        {
            InitializeComponent();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        void frmNurCodeHelp_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            GetData();
        }

        void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            string SQL = string.Empty;
            string SqlErr = "";
            DataTable dt = null;
            ssNurCodeHelp_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "   CODE, GUBUN1, GUBUN2";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "ABC_NURSE_CODE";
                SQL = SQL + ComNum.VBLF + "ORDER BY Gubun1, Gubun2";
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

                ssNurCodeHelp_Sheet1.RowCount = dt.Rows.Count;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssNurCodeHelp_Sheet1.Cells[i, 0].Text = dt.Rows[i]["CODE"].ToString().Trim();
                    ssNurCodeHelp_Sheet1.Cells[i, 1].Text = dt.Rows[i]["GUBUN1"].ToString().Trim();
                    ssNurCodeHelp_Sheet1.Cells[i, 2].Text = dt.Rows[i]["GUBUN2"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
        }

        void ssNurCodeHelp_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == 0)
            {
                rSetHelpCode(VB.Left(ssNurCodeHelp_Sheet1.Cells[e.Row, 0].Text + VB.Space(4), 4));
            }
            else if(e.Column == 1)
            {
                rSetHelpCode(clsPublic.GstrHelpCode = clsPublic.GstrHelpCode + ", " + ssNurCodeHelp_Sheet1.Cells[e.Row, 1].Text);
            }
            else if(e.Column == 2)
            {
                rSetHelpCode(clsPublic.GstrHelpCode = clsPublic.GstrHelpCode + " " + ssNurCodeHelp_Sheet1.Cells[e.Row, 2].Text);
            }

            rEventClosed();
        }
    }
}
