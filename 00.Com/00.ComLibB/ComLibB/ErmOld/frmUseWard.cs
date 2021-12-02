using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{

    /// Class Name      : ComLibB.dll
    /// File Name       : frmUseWard.cs
    /// Description     : 상용구 등록 및 조회
    /// Author          : 최익준
    /// Create Date     : 2017-08-28
    /// Update History  : 
    /// </summary>
    /// <vbp>
    /// default : VB\Ocs\viewetc\ViewETC.vbp
    /// </vbp>


    public partial class frmUseWard : Form
    {
        public frmUseWard()
        {
            InitializeComponent();
        }

        private void frmUseWard_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            int nRow = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "Code,WardName,ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "XRAY_RESULTWARD";
                SQL = SQL + ComNum.VBLF + "WHERE Sabun= " + clsType.User.Sabun + " ";

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


                for (i = 0; i < dt.Rows.Count; i++)
                {
                    switch (dt.Rows[i]["Code"].ToString().Trim())
                    {
                        case "F1":
                            nRow = 1;
                            break;

                        case "F2":
                            nRow = 2;
                            break;

                        case "F3":
                            nRow = 3;
                            break;

                        case "4F":
                            nRow = 4;
                            break;

                        case "F5":
                            nRow = 5;
                            break;

                        case "6F":
                            nRow = 6;
                            break;

                        case "7F":
                            nRow = 7;
                            break;

                        case "8F":
                            nRow = 8;
                            break;

                        case "9F":
                            nRow = 9;
                            break;

                        case "10F":
                            nRow = 10;
                            break;

                    }

                    
                    ssView_Sheet1.Cells[nRow-1, 1].Text = dt.Rows[i]["WardName"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow-1, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            string strCODE = "";
            string strWard = "";
            string strROWID = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < 10; i++)
                {

                    strCODE = ssView_Sheet1.Cells[i, 0].Text.Trim();
                    strWard = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i, 2].Text.Trim();

                    strWard = clsVbfunc.QuotationChange(strWard);

                    if (strROWID == "" && strWard != "")
                    {
                        SQL = "";                        
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "XRAY_RESULTWARD (Sabun,Code,WardName) ";
                        SQL = SQL + ComNum.VBLF + "VALUES (" + clsType.User.Sabun + ",'" + strCODE + "',";
                        SQL = SQL + ComNum.VBLF + "'" + strWard + "') ";                        
                    }
                    else if(strROWID != "")
                    {
                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_PMPA + "XRAY_RESULTWARD SET WardName='" + strWard + "'";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID='" + strROWID + "'";
                    }

                    if (SQL != "")
                    {
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    
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

        }
    }
}
