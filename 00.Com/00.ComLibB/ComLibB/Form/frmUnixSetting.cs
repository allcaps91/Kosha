using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmUnixSetting.cs
    /// Description     : 유닉스 계정 및 패스워드 관리
    /// Author          : 박창욱
    /// Create Date     : 2019-02-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\basic\bucode\Frm유닉스세팅.frm(Frm유닉스세팅) >> frmUnixSetting.cs 폼이름 재정의" />	
    public partial class frmUnixSetting : Form
    {
        public frmUnixSetting()
        {
            InitializeComponent();
        }

        void Screen_Clear()
        {
            grbJob.Enabled = true;
            grbDate.Enabled = true;
            btnSearch.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 20;
            ssView.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //AS-IS에서 주석된 기능

            //Dim i               As Integer
            //Dim strIDName       As String
            //Dim strStartDate    As String

            //'''    GstrMsgList = "화면의 자료를 정말로 삭제 하시겠습니까?"
            //'''    If MsgBox(GstrMsgList, vbYesNo + vbDefaultButton2, "삭제여부") = vbNo Then Exit Sub
            //'''
            //'''
            //'''    adoConnect.BeginTrans
            //'''
            //'''    '자료를 삭제
            //'''    SQL = "DELETE BAS_ACCOUNT_SERVER "
            //'''    SQL = SQL & "WHERE IDname = '" & strIDName & "' "
            //'''    SQL = SQL & "  AND StartDate=TO_DATE('" & ComboDate.Text & "','YYYY - MM - DD') "
            //'''    Result = AdoExecute(SQL)
            //'''    If Result <> 0 Then
            //'''        adoConnect.RollbackTrans
            //'''        MsgBox "자료를 삭제 도중에 오류가 발생함", vbInformation, "RollBack"
            //'''        Exit Sub
            //'''    End If
            //'''
            //'''    adoConnect.CommitTrans
            //'''    Call SCREEN_CLEAR
            //'''    Call ComboDate_SET
            //'''
            //'''    ComboJob.SetFocus

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

            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strGubun = string.Empty;
            string strSDate = string.Empty;
            string strServer_IP = string.Empty;
            string strUser = string.Empty;
            string strUserPass = string.Empty;
            string strRemark = string.Empty;
            string strROWID = string.Empty;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.NonEmptyRowCount; i++)
                {
                    strGubun = VB.Val(ssView_Sheet1.Cells[i, 0].Text).ToString("00");
                    strServer_IP = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strUser = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    strUserPass = ssView_Sheet1.Cells[i, 3].Text.Trim();
                    strRemark = ssView_Sheet1.Cells[i, 4].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i, 5].Text.Trim();

                    if (rdoJob0.Checked)
                    {
                        SQL = "";
                        SQL = "INSERT INTO BAS_ACCOUNT_SERVER (Gubun,SDate,IP,UserID,UserPass,Remark ";
                        SQL += ComNum.VBLF + " ) VALUES ('" + strGubun;
                        SQL += ComNum.VBLF + "',TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),";
                        SQL += ComNum.VBLF + " '" + strServer_IP + "','" + strUser + "','" + clsAES.AES(strUserPass) + "','" + strRemark + "') ";
                    }
                    else
                    {
                        SQL = "";
                        SQL = "UPDATE BAS_ACCOUNT_SERVER SET IP='" + strServer_IP + "',";
                        SQL += ComNum.VBLF + " UserID='" + strUser + "',";
                        SQL += ComNum.VBLF + " UserPass='" + clsAES.AES(strUserPass) + "',";
                        SQL += ComNum.VBLF + " Remark = '" + strRemark + "' ";
                        SQL += ComNum.VBLF + " WHERE ROWID='" + strROWID + "' ";
                    }
                }

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

                Screen_Clear();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strIDName = string.Empty;
            string strSDate = string.Empty;

            ssView.Enabled = true;
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 20;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (rdoJob0.Checked)
                {
                    //신규자료를 READ
                    SQL = "";
                    SQL = "SELECT TO_CHAR(SDate,'YYYY-MM-DD') SDate,COUNT(*) CNT ";
                    SQL += ComNum.VBLF + " FROM BAS_ACCOUNT_SERVER ";
                    SQL += ComNum.VBLF + "GROUP BY SDate ";
                    SQL += ComNum.VBLF + "ORDER BY SDate DESC ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    strSDate = "";
                    if (dt.Rows.Count > 0)
                    {
                        strSDate = dt.Rows[0]["SDATE"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;
                    grbDate.Enabled = true;

                    if (strSDate == "")
                    {
                        return;
                    }

                    //최종자료를 Sheet에 Display
                    SQL = "";
                    SQL = "SELECT Gubun,IP,UserID,UserPass,Remark,ROWID ";
                    SQL += ComNum.VBLF + " FROM BAS_ACCOUNT_SERVER ";
                    SQL += ComNum.VBLF + "WHERE SDate = TO_DATE('" + strSDate + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    ssView_Sheet1.RowCount = dt.Rows.Count + 10;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["IP"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["USERID"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = clsAES.DeAES(dt.Rows[i]["USERPASS"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = "";
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    //수정작업
                    SQL = "";
                    SQL = "SELECT Gubun,IP,UserID,UserPass,Remark,ROWID ";
                    SQL += ComNum.VBLF + " FROM BAS_ACCOUNT_SERVER ";
                    SQL += ComNum.VBLF + "WHERE SDate = TO_DATE('" + cboDate.Text + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    ssView_Sheet1.RowCount = dt.Rows.Count + 10;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["IP"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["USERID"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = clsAES.DeAES(dt.Rows[i]["USERPASS"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    grbDate.Enabled = false;
                    btnDelete.Enabled = true;
                }

                grbJob.Enabled = false;
                btnSearch.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void frmUnixSetting_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            Screen_Clear();
            cboDate.Items.Clear();
            cboDate.Visible = false;
            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
        }

        private void rdoJob_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoJob0.Checked)
            {
                cboDate.Visible = false;
                dtpDate.Visible = true;
            }
            else
            {
                cboDate.Visible = true;
                dtpDate.Visible = false;
                cboDate_Set();
            }
        }

        void cboDate_Set()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strIDName = string.Empty;

            cboDate.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //자료를 READ
                SQL = "";
                SQL = "SELECT TO_CHAR(SDate,'YYYY-MM-DD') SDate,COUNT(*) CNT ";
                SQL += ComNum.VBLF + " FROM BAS_ACCOUNT_SERVER ";
                SQL += ComNum.VBLF + "GROUP BY SDate ";
                SQL += ComNum.VBLF + "ORDER BY SDate DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
               
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDate.Items.Add(dt.Rows[i]["SDATE"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                cboDate.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }
    }
}
