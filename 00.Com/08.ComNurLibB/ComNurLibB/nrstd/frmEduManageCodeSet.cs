using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmEduManageCodeSet.cs
    /// Description     : 교육종류별설정작업
    /// Author          : 박창욱
    /// Create Date     : 2018-01-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrstd\Frm교육관리코드설정.frm(Frm교육관리코드설정.frm) >> frmEduManageCodeSet.cs 폼이름 재정의" />	
    public partial class frmEduManageCodeSet : Form
    {
        public frmEduManageCodeSet()
        {
            InitializeComponent();
        }

        private void frmEduManageCodeSet_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            Search_Data();
        }

        void Search_Data()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strCODE = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strCODE = ssView_Sheet1.Cells[i, 0].Text.Trim();
                    ssView_Sheet1.Cells[i, 1].Value = false;
                    ssView_Sheet1.Cells[i, 2].Text = "";
                    if (strCODE != "")
                    {
                        SQL = "";
                        SQL = " SELECT CODE, GUBUNA, GROUPNAME ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE_SET ";
                        SQL = SQL + ComNum.VBLF + " WHERE CODE = '" + strCODE + "' ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[i, 1].Value = dt.Rows[0]["GUBUNA"].ToString().Trim() == "1" ? true : false;
                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[0]["GROUPNAME"].ToString().Trim();
                        }
                        dt.Dispose();
                        dt = null;
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (Save_Data() == true)
            {
                Search_Data();
            }
        }

        bool Save_Data()
        {
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strCODE = "";
            string strA = "";
            string strB = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "NUR_EDU_CODE_SET ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strCODE = ssView_Sheet1.Cells[i, 0].Text.Trim();
                    strA = Convert.ToBoolean(ssView_Sheet1.Cells[i, 1].Value) == true ? "1" : "0";
                    strB = ssView_Sheet1.Cells[i, 2].Text.Trim();

                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_EDU_CODE_SET(CODE, GUBUNA, GROUPNAME) VALUES(";
                    SQL = SQL + ComNum.VBLF + "'" + strCODE + "','" + strA + "','" + strB + "') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }
    }
}
