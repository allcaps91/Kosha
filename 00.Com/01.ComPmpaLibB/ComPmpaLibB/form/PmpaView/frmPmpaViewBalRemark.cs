using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewHistory.cs
    /// Description     : 발생주의통계수치자료설명
    /// Author          : 박창욱
    /// Create Date     : 2017-11-07
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs99.frm(FrmBalRemark.frm) >> frmPmpaViewHistory.cs 폼이름 재정의" />	
    public partial class frmPmpaViewBalRemark : Form
    {
        string GstrYYMM = "";
        string GstrMenu = "";
        string GstrSMenu = "";

        public frmPmpaViewBalRemark()
        {
            InitializeComponent();
        }

        public frmPmpaViewBalRemark(string strYYMM, string strMenu, string strSMenu)
        {
            InitializeComponent();
            GstrYYMM = strYYMM;
            GstrMenu = strMenu;
            GstrSMenu = strSMenu;
        }

        private void frmPmpaViewBalRemark_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등


            rtbRemark.Rtf = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT REMARK FROM " + ComNum.DB_PMPA + "MISU_BALREMARK";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM ='" + GstrYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND MENU ='" + GstrMenu + "'";
                SQL = SQL + ComNum.VBLF + "   AND SUBMENU = '" + GstrSMenu + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    rtbRemark.Rtf = dt.Rows[0]["REMARK"].ToString().Trim();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (rtbRemark.Text.Trim() == "")
            {
                return;
            }

            ssPrint_Sheet1.Cells[0, 0].Text = "";
            ssPrint_Sheet1.Cells[0, 0].Text = rtbRemark.Text;

            ssPrint_Sheet1.Cells[0, 0].Font = rtbRemark.Font;
            ssPrint_Sheet1.Cells[0, 0].ForeColor = rtbRemark.ForeColor;

            ssPrint_Sheet1.PrintInfo.ShowBorder = false;
            ssPrint_Sheet1.PrintInfo.ShowGrid = false;
            ssPrint_Sheet1.PrintInfo.ShowShadows = false;
            ssPrint_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPrint_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssPrint_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssPrint.PrintSheet(0);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = "";    //Query문
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strCommit = "";
            string strRowid = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            strCommit = "OK";

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALREMARK";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM='" + GstrYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND MENU ='" + GstrMenu + "'";
                SQL = SQL + ComNum.VBLF + "   AND SUBMENU = '" + GstrSMenu + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    //자료를 Insert
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALREMARK (MENU, SUBMENU, YYMM, REMARK ) ";
                    SQL = SQL + ComNum.VBLF + "  VALUES ('" + GstrMenu + "','" + GstrSMenu + "','" + GstrYYMM + "','" + rtbRemark.Rtf + "' )";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALREMARK ";
                    SQL = SQL + ComNum.VBLF + " WHERE YYMM='" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND MENU ='" + GstrMenu + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND SUBMENU = '" + GstrSMenu + "' ";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    strRowid = dt1.Rows[0]["ROWID"].ToString().Trim();

                    dt1.Dispose();
                    dt1 = null;
                }





                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }



            PrintDialog o = new PrintDialog();



        }

        private void btnText_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = rtbRemark.Font;
            fd.Color = rtbRemark.ForeColor;
            fd.ShowDialog();

            rtbRemark.Font = fd.Font;
            rtbRemark.ForeColor = fd.Color;
        }
    }
}
