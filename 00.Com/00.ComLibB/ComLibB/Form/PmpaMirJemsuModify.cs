using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class PmpaMirJemsuModify : Form
    {

        /// Class Name      : PmpaMir.dll
        /// File Name       : PmpaMirJemsuModify.cs
        /// Description     : 상대가치 점수 일괄수정
        /// Author          : 최익준
        /// Create Date     : 2017-09-01
        /// Update History  : 
        /// </summary>
        /// <vbp>
        /// default : Z:\차세대 의료정보시스템\1-0 착수단계\1-5 참고 소스\포항성모병원 VB Source(2017.01.11)_분석설계제작용\basic\busuga\BuSuga42.frm
        /// </vbp>
        public PmpaMirJemsuModify()
        {
            InitializeComponent();
        }

        private void PmpaMirJemsuModify_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등


            ssView_Sheet1.Columns[5].Visible = false;
            TxtCode.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nREAD = 0;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 30;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            Cursor.Current = Cursors.WaitCursor;

            if (TxtCode.Text == "" && ChkZero.Checked == false)
            {
                ComFunc.MsgBox("찾으실 코드가 공란입니다.", "확인");
                TxtCode.Focus();
            }

            try
            {
                SQL = "";
                SQL = "SELECT BCode,TO_CHAR(JDate1,'YYYY-MM-DD') JDate1,JemSu1,Price1,Remark,ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUGAJEMSU";

                if (ChkZero.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE (Jemsu1 = 0 Or Price1 = 0)";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE BCode LIKE '" + TxtCode.Text.Trim() + "%'";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY BCode";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nREAD = dt.Rows.Count;
                ssView_Sheet1.RowCount = nREAD;

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["JDate1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Jemsu1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Value = VB.Val(dt.Rows[i]["Price1"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();


                    if (ComFunc.LenH(dt.Rows[i]["Remark"].ToString()) <= 60)
                    {
                        ssView_Sheet1.SetRowHeight(i, 11);
                    }
                    else if (ComFunc.LenH(dt.Rows[i]["Remark"].ToString()) <= 120)
                    {
                        ssView_Sheet1.SetRowHeight(i, 22);
                    }
                    else if (ComFunc.LenH(dt.Rows[i]["Remark"].ToString()) <= 180)
                    {
                        ssView_Sheet1.SetRowHeight(i, 33);
                    }
                    else
                    {
                        ssView_Sheet1.SetRowHeight(i, 44);
                    }



                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                ssView.Focus();

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssView_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strData = "";
            string strROWID = "";

            if (e.Column != 1 && e.Column != 2 && e.Column != 3)
            {
                return;
            }
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            //clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            strData = ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim();
            strROWID = ssView_Sheet1.Cells[e.Row, 5].Text.Trim();
            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_SUGAJEMSU SET";

                if(e.Column == 1)
                {
                    SQL = SQL + ComNum.VBLF + "JDate1=TO_DATE('" + strData + "','YYYY-MM-DD') ";
                }
                else if (e.Column == 2)
                {
                    SQL = SQL + ComNum.VBLF + "Jemsu1=" + VB.Val(strData) + " ";
                }
                else if (e.Column == 3)
                {
                    SQL = SQL + ComNum.VBLF + "Price1=" + VB.Val(strData) + " ";
                }
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("BAS_SUGAJEMSU에 UPDATE 도중 오류가 발생함", "확인");
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void TxtCode_Enter(object sender, EventArgs e)
        {
            TxtCode.Text = "";
        }
    }
}
