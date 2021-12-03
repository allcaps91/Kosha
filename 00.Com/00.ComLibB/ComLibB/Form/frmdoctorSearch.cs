using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmdoctorSearch : Form
    {
        public frmdoctorSearch()
        {
            InitializeComponent();
        }

        private void frmdoctorSearch_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            txtDrName.Text = "";
            ssView_Sheet1.RowCount = 0;
        }
        
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            int i = 0;
            DataTable dt= null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false) return;//권한 확인

            ssView_Sheet1.RowCount = 0;

            if (txtDrName.Text.Trim() == "") return;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " SELECT a.DrCode, a.DrDept1, a.DrName, a.GbChoice ";
                SQL = SQL + ComNum.VBLF + "  From BAS_DOCTOR a ";
                SQL = SQL + ComNum.VBLF + " Where a.DrName like '%" + (txtDrName.Text).Trim () + "%' ";
                SQL = SQL + ComNum.VBLF + "   AND a.TOUR <> 'Y' ";
                SQL = SQL + ComNum.VBLF + " ORDER By a.DrName ";

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

                ssView_Sheet1.Rows.Count = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight (-1 , ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells [i , 0].Text = dt.Rows [i] ["DrCode"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 1].Text = dt.Rows [i] ["DrDept1"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 2].Text = dt.Rows [i] ["DrName"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 3].Text = dt.Rows [i] ["DrCode"].ToString ().Trim (); //READ_OCS_Doctor2_Sabun(AdoGetString(Rs, "DrCode", i))
                    ssView_Sheet1.Cells [i , 4].Text = dt.Rows [i] ["GbChoice"].ToString ().Trim ();
                }
                dt.Dispose ();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void txtDrName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }
    }
}
