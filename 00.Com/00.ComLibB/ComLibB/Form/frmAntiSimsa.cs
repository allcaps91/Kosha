using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmAntiSimsa.cs
    /// Description     : 항생제관리
    /// Author          : 유진호
    /// Create Date     : 2018-05-23        
    /// <seealso>
    /// ..\basic\busuga\FrmAntiSimsa.frm(FrmAntiSimsa)
    /// </seealso>    
    /// </summary>
    public partial class frmAntiSimsa : Form
    {
        public frmAntiSimsa()
        {
            InitializeComponent();
        }

        private void frmAntiSimsa_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //Form 권한조회
            {
                this.Close();
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //Form 기본값 셋팅

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            btnDeleteClick();

            READ_DATA("1");
        }

        private bool btnDeleteClick()
        {
            bool rtnVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strROWID = "";

            if (ComFunc.MsgBoxQ("선택한 내용을 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return rtnVal;
            }

            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strROWID = ssView_Sheet1.Cells[i, 7].Text;

                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true && strROWID != "")
                    {
                        SQL = " UPDATE ADMIN.BAS_ANTI_SIMSA SET";
                        SQL = SQL + ComNum.VBLF + " DELDATE = SYSDATE ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }                    
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제되었습니다.");
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            btnSaveClick();
            READ_DATA("1");
        }

        private bool btnSaveClick()
        {
            bool rtnVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strSuCode = "";
            string strSunameK = "";
            string strGBN1 = "";
            string strGBN2 = "";
            string strROWID = "";
                        
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strSuCode = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strSunameK = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    strGBN1 = ssView_Sheet1.Cells[i, 3].Text.Trim() == "True" ? "1" : "0";
                    strGBN2 = ssView_Sheet1.Cells[i, 4].Text.Trim() == "True" ? "1" : "0";
                    strROWID = ssView_Sheet1.Cells[i, 7].Text.Trim();

                    if (strSuCode != "" && strSunameK != "" && strROWID == "")
                    {
                        SQL = " INSERT INTO ADMIN.BAS_ANTI_SIMSA(SUCODE, WRITEDATE, WRITESABUN, GBN1, GBN2) VALUES (";
                        SQL = SQL + ComNum.VBLF + "'" + strSuCode + "', ";
                        SQL = SQL + ComNum.VBLF + " SYSDATE, " + clsType.User.Sabun + ", ";
                        SQL = SQL + ComNum.VBLF + "'" + strGBN1 + "','" + strGBN2 + "')";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                    else if (strSuCode != "" && strSunameK != "" && strROWID != "")
                    {
                        SQL = " UPDATE ADMIN.BAS_ANTI_SIMSA SET ";
                        SQL = SQL + ComNum.VBLF + " GBN1 = '" + strGBN1 + "', ";
                        SQL = SQL + ComNum.VBLF + " GBN2 = '" + strGBN2 + "' ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장되었습니다.");
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            READ_DATA("1");
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            READ_DATA("");
        }

        private void READ_DATA(string strValue)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.RowCount = 0;

            try
            {                                
                SQL = " SELECT A.SUCODE, (SELECT SUNAMEK FROM ADMIN.BAS_SUN WHERE SUNEXT = A.SUCODE) SUNAMEK, GBN1, GBN2,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(WRITEDATE, 'YYYY-MM-DD') WDATE, TO_CHAR(DELDATE,'YYYY-MM-DD') DDATE, ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_ANTI_SIMSA A";
                if (strValue == "1")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE DelDate Is Null";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY SUCODE ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count + 10;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["GBN1"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GBN2"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["WDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DDATE"].ToString().Trim();
                        ssView_Sheet1.Rows[i].ForeColor = ssView_Sheet1.Cells[i, 6].Text != "" ?  Color.FromArgb(255, 0, 0) : ssView_Sheet1.Rows[i].ForeColor;
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssView_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strCODE = "";

            if (e.Column != 1) return;

            strCODE = VB.UCase(ssView_Sheet1.Cells[e.Row, 1].Text);
            ssView_Sheet1.Cells[e.Row, 1].Text = strCODE;

            try
            {
                SQL = " SELECT SUNAMEK ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_SUN ";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + strCODE + "' ";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {                   
                    ssView_Sheet1.Cells[e.Row, 2].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();                                            
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
