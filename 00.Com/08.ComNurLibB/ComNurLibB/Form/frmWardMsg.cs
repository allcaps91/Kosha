using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComNurLibB
{
    public partial class frmWardMsg : Form
    {

        string mstrWard = "";

        string mstrTODATE = "";
        string mstrTOTIME = "";
        string mstrTOSABUN = "";

        public frmWardMsg()
        {
            InitializeComponent();
        }

        private void frmWardMsg_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //else
            //{
            //    ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            //    ComFunc.ReadSysDate(clsDB.DbCon);
            //}

            btnClear_Click(null, null);

            cboWard_SET();

            mstrWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");

            dtpSDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            dtpEDate.Value = dtpSDate.Value;

            optGuBunSend.Checked = true;

        }

        private void cboWard_SET()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT WARDCODE, WARDNAME  ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_WARD ";
                SQL = SQL + ComNum.VBLF + "WHERE WARDCODE NOT IN ('IU','NP','2W','IQ') ";
                SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'";
                SQL = SQL + ComNum.VBLF + "ORDER BY WARDCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                cboSENDWARD.Items.Clear();

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboSENDWARD.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                    }
                    cboSENDWARD.Items.Add("HD");
                    cboSENDWARD.Items.Add("ER");
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void optGuBun_CheckedChanged(object sender, EventArgs e)
        {
            btnSearch_Click(btnSearch, new EventArgs());
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.RowCount = 0;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";

                if (optGuBunTo.Checked == true || optGuBunAll.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "SELECT '전송' AS GUBUN, TODATE, TOTIME, TOSABUN, TOWARD, TOMEMO, SENDWARD, SENDDATE, SENDTIME, SENDSABUN, SENDMEMO, CHK, ROWID     ";
                    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.ETC_MSG_WARD        ";
                    SQL = SQL + ComNum.VBLF + "WHERE TOWARD = '" + mstrWard + "'        ";
                    SQL = SQL + ComNum.VBLF + "    AND TODATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "'        ";
                    SQL = SQL + ComNum.VBLF + "    AND TODATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "'        ";
                }

                if (optGuBunAll.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "UNION ALL        ";
                }

                if (optGuBunSend.Checked == true || optGuBunAll.Checked == true)
                {

                    SQL = SQL + ComNum.VBLF + "SELECT '회신' AS GUBUN, TODATE, TOTIME, TOSABUN, TOWARD, TOMEMO, SENDWARD, SENDDATE, SENDTIME, SENDSABUN, SENDMEMO, CHK, ROWID     ";
                    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.ETC_MSG_WARD        ";
                    SQL = SQL + ComNum.VBLF + "WHERE SENDWARD = '" + mstrWard + "'      ";
                    SQL = SQL + ComNum.VBLF + "    AND TODATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "'     ";
                    SQL = SQL + ComNum.VBLF + "    AND TODATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "'     ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY TODATE DESC, TOTIME DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {

                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = ComFunc.FormatStrToDate(dt.Rows[i]["TODATE"].ToString().Trim(), "D");
                        ssView_Sheet1.Cells[i, 2].Text = ComFunc.FormatStrToDate(dt.Rows[i]["TOTIME"].ToString().Trim(), "T");
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["TOSABUN"].ToString().Trim();

                        if (dt.Rows[i]["GUBUN"].ToString().Trim() == "전송")
                        {
                            ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SENDWARD"].ToString().Trim();
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["TOWARD"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["TOMEMO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["SENDMEMO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = ComFunc.FormatStrToDate(dt.Rows[i]["SENDDATE"].ToString().Trim() + " " + dt.Rows[i]["SENDTIME"].ToString().Trim(), "A");
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        if (dt.Rows[i]["CHK"].ToString().Trim() == "Y")
                        {
                            ssView_Sheet1.Cells[i, 8].Value = true;
                            ssView_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(200, 200, 250);
                        }
                        

                    }

                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

        }
        private void Update_chk()
        {
            //arggbn 에 OK라는 값이 있을 경우 강제 업데이트

            int intRow = ssView_Sheet1.ActiveRowIndex;
            int intCol = ssView_Sheet1.ActiveColumnIndex;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strROWID = "";
            string strCHECK = "";

            if (intCol == 8)
            {
                strROWID = ssView_Sheet1.Cells[intRow, 9].Text.Trim();
                if (ssView_Sheet1.Cells[intRow, 8].Value.ToString() == "True")
                {
                    strCHECK = "Y";
                }
                else
                {
                    strCHECK = "";
                }
                
                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL = " UPDATE KOSMOS_PMPA.ETC_MSG_WARD SET ";
                    SQL += ComNum.VBLF + "   CHK = '" + strCHECK + "', ";
                    SQL += ComNum.VBLF + "   CHKDATE = SYSDATE, ";
                    SQL += ComNum.VBLF + "   CHKSABUN = " + clsType.User.Sabun;
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("참고사항 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(ex.Message);
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                return;
            }

            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            mstrTODATE = ssView_Sheet1.Cells[e.Row, 1].Text.Trim();
            mstrTOTIME = ssView_Sheet1.Cells[e.Row, 2].Text.Trim();
            mstrTOSABUN = ssView_Sheet1.Cells[e.Row, 3].Text.Trim();

            if (ssView_Sheet1.Cells[e.Row, 0].Text.Trim() == "전송")
            {
                ComFunc.ComboFind(cboSENDWARD, "", 0, ssView_Sheet1.Cells[e.Row, 4].Text.Trim());

                btnSave1.Enabled = true;
                btnDelete.Enabled = true;
                btnSave2.Enabled = false;
            }
            else
            {
                ComFunc.ComboFind(cboSENDWARD, "", 0, mstrWard);

                btnSave1.Enabled = false;
                btnDelete.Enabled = false;
                btnSave2.Enabled = true;
            }

            txtTOMEMO.Text = ssView_Sheet1.Cells[e.Row, 5].Text.Trim();

            lblSENDDATE.Text = ssView_Sheet1.Cells[e.Row, 7].Text.Trim();

            txtSENDMEMO.Text = ssView_Sheet1.Cells[e.Row, 6].Text.Trim();

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            mstrTODATE = "";
            mstrTOTIME = "";
            mstrTOSABUN = "";

            txtTOMEMO.Text = "";
            lblSENDDATE.Text = "";
            txtSENDMEMO.Text = "";

            btnDelete.Enabled = false;
            btnSave1.Enabled = true;
            btnSave2.Enabled = false;

        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (mstrTODATE == "" && mstrTOTIME == "" && mstrTOSABUN == "")
                {
                    SQL = "";
                    SQL = "INSERT INTO KOSMOS_PMPA.ETC_MSG_WARD     ";
                    SQL = SQL + ComNum.VBLF + "(     ";
                    SQL = SQL + ComNum.VBLF + "   TODATE, TOTIME, TOSABUN,     ";
                    SQL = SQL + ComNum.VBLF + "   TOWARD, TOMEMO, SENDWARD     ";
                    SQL = SQL + ComNum.VBLF + ")     ";
                    SQL = SQL + ComNum.VBLF + "VALUES     ";
                    SQL = SQL + ComNum.VBLF + "(     ";
                    SQL = SQL + ComNum.VBLF + "    TO_CHAR(SYSDATE,'YYYYMMDD'),     ";
                    SQL = SQL + ComNum.VBLF + "    TO_CHAR(SYSDATE,'HH24MISS'),     ";
                    SQL = SQL + ComNum.VBLF + "    '" + clsType.User.Sabun + "',     ";
                    SQL = SQL + ComNum.VBLF + "    '" + mstrWard + "',     ";
                    SQL = SQL + ComNum.VBLF + "    '" + txtTOMEMO.Text + "',     ";
                    SQL = SQL + ComNum.VBLF + "    '" + cboSENDWARD.Text.Trim() + "'     ";
                    SQL = SQL + ComNum.VBLF + ")     ";
                }
                else if (mstrTODATE != "" && mstrTOTIME != "" && mstrTOSABUN != "")
                {
                    if (mstrTOSABUN != clsType.User.Sabun)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("작성한 사용자가 아니면 수정 할 수 없습니다.");
                        return;
                    }

                    if (ChkSENDSABUN() == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("이미 회신한 메세지 입니다");
                        return;
                    }

                    SQL = "";
                    SQL = "UPDATE KOSMOS_PMPA.ETC_MSG_WARD      ";
                    SQL = SQL + ComNum.VBLF + "SET    TODATE = TO_CHAR(SYSDATE,'YYYYMMDD'),      ";
                    SQL = SQL + ComNum.VBLF + "       TOTIME = TO_CHAR(SYSDATE,'HH24MISS'),      ";
                    SQL = SQL + ComNum.VBLF + "       TOMEMO = '" + txtTOMEMO.Text + "'      ";
                    SQL = SQL + ComNum.VBLF + "WHERE  TODATE = '" + mstrTODATE.Replace("-", "") + "'      ";
                    SQL = SQL + ComNum.VBLF + "     AND    TOTIME = '" + mstrTOTIME.Replace(":", "") + "'      ";
                    SQL = SQL + ComNum.VBLF + "     AND    TOSABUN = '" + mstrTOSABUN + "'      ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                btnClear_Click(null, null);
                btnSearch_Click(null, null);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private bool ChkSENDSABUN()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT SENDMEMO         ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.ETC_MSG_WARD        ";
                SQL = SQL + ComNum.VBLF + "WHERE  TODATE = '" + mstrTODATE.Replace("-", "") + "'        ";
                SQL = SQL + ComNum.VBLF + "     AND TOTIME = '" + mstrTOTIME.Replace(":", "") + "'        ";
                SQL = SQL + ComNum.VBLF + "     AND TOSABUN = '" + mstrTOSABUN + "'        ";
                SQL = SQL + ComNum.VBLF + "     AND SENDMEMO IS NOT NULL        ";


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;

                    return false;
                }

                dt.Dispose();
                dt = null;

                return true;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (mstrTODATE == "" && mstrTOTIME == "" && mstrTOSABUN == "")
            {
                return;
            }

            //if (ChkSENDSABUN() == false)
            //{
            //    ComFunc.MsgBox("이미 회신한 메세지 입니다");
            //    return;
            //}


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE KOSMOS_PMPA.ETC_MSG_WARD      ";
                SQL = SQL + ComNum.VBLF + "SET    SENDDATE = TO_CHAR(SYSDATE,'YYYYMMDD'),      ";
                SQL = SQL + ComNum.VBLF + "       SENDTIME = TO_CHAR(SYSDATE,'HH24MISS'),      ";
                SQL = SQL + ComNum.VBLF + "       SENDSABUN = '" + clsType.User.Sabun + "',      ";
                SQL = SQL + ComNum.VBLF + "       SENDMEMO = '" + txtSENDMEMO.Text + "'      ";
                SQL = SQL + ComNum.VBLF + "WHERE  TODATE = '" + mstrTODATE.Replace("-", "") + "'      ";
                SQL = SQL + ComNum.VBLF + "     AND    TOTIME = '" + mstrTOTIME.Replace(":", "") + "'      ";
                SQL = SQL + ComNum.VBLF + "     AND    TOSABUN = '" + mstrTOSABUN + "'      ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                btnClear_Click(null, null);
                btnSearch_Click(null, null);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (mstrTODATE == "" && mstrTOTIME == "" && mstrTOSABUN == "")
            {
                return;
            }

            if (ChkSENDSABUN() == false)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("이미 회신한 메세지 입니다");
                return;
            }


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "DELETE KOSMOS_PMPA.ETC_MSG_WARD      ";
                SQL = SQL + ComNum.VBLF + "WHERE  TODATE = '" + mstrTODATE.Replace("-", "") + "'      ";
                SQL = SQL + ComNum.VBLF + "     AND    TOTIME = '" + mstrTOTIME.Replace(":", "") + "'      ";
                SQL = SQL + ComNum.VBLF + "     AND    TOSABUN = '" + mstrTOSABUN + "'      ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                btnClear_Click(null, null);
                btnSearch_Click(null, null);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssView_EditModeOff(object sender, EventArgs e)
        {
            Update_chk();
        }
    }
}
