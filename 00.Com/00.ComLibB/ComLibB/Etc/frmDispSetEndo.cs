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

namespace ComLibB
{
    public partial class frmDispSetEndo : Form
    {
        string FstrHref2 = "";
        string GstrHref1 = "";
        string[] FstrFile = new string[10];
        int Fn = 0;

        string FstrROWID = "";

        public frmDispSetEndo()
        {
            InitializeComponent();
        }

        private void frmDispSetEndo_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SCREEN_CLEAR();
            SET_Combo_ENDO();
            btnSearch_Add_Click(null, null);
            btnSearch_Click(null, null);
        }

        private void SET_Combo_ENDO()
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            cboSTS.Items.Clear();
            cboDrName.Items.Clear();

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                //'진행상태;
                SQL = "";
                SQL = " SELECT CODE,NAME ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='ETC_내시경_도착구분'";
                SQL = SQL + ComNum.VBLF + "   AND ( DELDATE IS NULL OR DELDATE ='')";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboSTS.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                    }
                }
                
                dt.Dispose();
                dt = null;

                cboSTS.SelectedIndex = 0;

                cboExam.Items.Clear();

                //'검사종류;
                SQL = "";
                SQL = "SELECT CODE,NAME ";
                SQL = SQL + "FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + "WHERE GUBUN ='ETC_내시경_검사구분'";
                SQL = SQL + "   AND ( DELDATE IS NULL OR DELDATE ='')";
                SQL = SQL + "ORDER BY CODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboExam.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboExam.SelectedIndex = 0;

                //'의사;
                SQL = "";
                SQL = " SELECT CODE,NAME ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='ETC_내시경_의사구분'";
                SQL = SQL + ComNum.VBLF + "   AND ( DELDATE IS NULL OR DELDATE ='')";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDrName.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboDrName.SelectedIndex = 0;

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

        private void SCREEN_CLEAR()
        {
            //'도착처리 clear
            FstrROWID = "";

            txtPano.Text = "";
            lblSName.Text = "";
            txtSTime.Text = "";
            cboExam.Text = "";
            cboDrName.Text = "";
            cboSTS.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strDrCode = "";
            string strApply = "";
            string strROWID = "";

            string strDept = "";
            string strDrName = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView2_Sheet1.RowCount; i++)
                {
                    strApply = (Convert.ToBoolean(ssView2_Sheet1.Cells[i, 0].Value) == true ? "1" : "0");
                    strROWID = ssView2_Sheet1.Cells[i, 4].Text.Trim();
                    strDept = ssView2_Sheet1.Cells[i, 1].Text.Trim(); //'Dept
                    strDrName = ssView2_Sheet1.Cells[i, 2].Text.Trim(); //'drname
                    strDrCode = ssView2_Sheet1.Cells[i, 3].Text.Trim(); //drcode

                    if (strROWID == "" && strApply == "1")
                    {

                        if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_PMPA.ETC_HTML (DRCODE, TIME, HREF, GUBUN, APPLY) VALUES (";
                        SQL = SQL + ComNum.VBLF + " '" + strDrCode + "', '" + strDept + "', '" + strDrName + "' ,";
                        SQL = SQL + ComNum.VBLF + " 'S', '" + strApply + "' ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                        }
                    }
                    else
                    {
                        if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = " UPDATE KOSMOS_PMPA.ETC_HTML SET  APPLY = '" + strApply + "' ";
                            SQL = SQL + " WHERE ROWID = '" + strROWID + "' ";
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                this.Close();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// CmdView2_Click 대체
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <seealso cref="CmdView2_Click"/>
        private void btnSearch_Add_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView3_Sheet1.RowCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                //'진료대기환자 SELECT;
                SQL = "";
                SQL = "SELECT A.PANO,A.SNAME,A.DRCODE,A.GBSTS,A.GBEXAM,A.SEQ_RTIME,A.RTIME,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.DEPTJTIME,'YYYY-MM-DD HH24:MI') DEPTJTIME,";
                SQL = SQL + ComNum.VBLF + " A.ROWID ,B.DRNAME, C.NAME AS STSNAME, D.NAME AS EXAMNAME ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.ENDO_DISP A ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "    ON A.DRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_PMPA.BAS_BCODE C";
                SQL = SQL + ComNum.VBLF + "    ON A.GbSTS = C.CODE";
                SQL = SQL + ComNum.VBLF + "    AND C.GUBUN = 'ETC_내시경_도착구분'";
                SQL = SQL + ComNum.VBLF + "    AND(C.DELDATE IS NULL OR C.DELDATE = '')";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_PMPA.BAS_BCODE D";
                SQL = SQL + ComNum.VBLF + "    ON A.GbEXAM = C.CODE";
                SQL = SQL + ComNum.VBLF + "    AND C.GUBUN = 'ETC_내시경_검사구분'";
                SQL = SQL + ComNum.VBLF + "    AND(C.DELDATE IS NULL OR C.DELDATE = '')";
                SQL = SQL + ComNum.VBLF + "WHERE A.BDATE = TO_DATE('" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND A.GUBUN ='1' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.SEQ_RTIME,A.DEPTJTIME,A.SNAME";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView3_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssView3_Sheet1.Cells[i, 3].Text = dt.Rows[i]["GBSTS"].ToString().Trim() + "." + dt.Rows[i]["STSNAME"].ToString().Trim();
                        ssView3_Sheet1.Cells[i, 4].Text = dt.Rows[i]["RTIME"].ToString().Trim();
                        ssView3_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SEQ_RTIME"].ToString().Trim();
                        ssView3_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GBEXAM"].ToString().Trim() + "." + dt.Rows[i]["EXAMNAME"].ToString().Trim();
                        ssView3_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                        ssView3_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView2_Sheet1.Rows.Count = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                //'모든 의사의 명단을 읽음;
                SQL = "";
                SQL = "SELECT B.PRINTRANKING, A.DRDEPT1, A.DRCODE, A.DRNAME  , C.APPLY, C.ROWID AS ETCROWID";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_DOCTOR A        ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_PMPA.BAS_CLINICDEPT B        ";
                SQL = SQL + ComNum.VBLF + "    ON A.DRDEPT1 = B.DEPTCODE        ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_PMPA.ETC_HTML C        ";
                SQL = SQL + ComNum.VBLF + "    ON A.DRCODE = C.DRCODE        ";
                SQL = SQL + ComNum.VBLF + "    AND C.GUBUN = 'S'        ";
                SQL = SQL + ComNum.VBLF + "WHERE(A.DRDEPT1 IN('MG', 'MP') OR A.DRCODE IN('1402', '1407'))        ";
                SQL = SQL + ComNum.VBLF + "    AND SUBSTR(A.DRCODE, 3, 2) <> '99'        ";
                SQL = SQL + ComNum.VBLF + "    AND A.TOUR = 'N'     ";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.DRCODE, B.PRINTRANKING,A.DRNAME        ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView2_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView2_Sheet1.Cells[i, 0].Value = (dt.Rows[i]["APPLY"].ToString().Trim() == "1" ? true : false);
                        ssView2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DRDEPT1"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ETCROWID"].ToString().Trim();
                    }
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            txtPano.Focus();
        }

        private void btnSave_Add_Click(object sender, EventArgs e)
        {
            string strSTS = "";
            string strSTIME = "";
            string strExam = "";
            string strDrCode = "";
            string strPano = "";
            string strSName = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            strPano = txtPano.Text.Trim();
            strSName = lblSName.Text.Trim();

            strSTS = VB.Left(cboSTS.Text, 2);
            strSTIME = txtSTime.Text.Trim();
            strExam = VB.Left(cboExam.Text.Trim(), 2);
            strDrCode = VB.Left(cboDrName.Text.Trim(), 4);

            if (strSName == "")
            {
                ComFunc.MsgBox("등록된 환자만 가예약등록이 됩니다..", "확인");
                return;
            }


            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (FstrROWID != "")
                {
                    if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

                    SQL = "";
                    SQL = " UPDATE KOSMOS_OCS.ENDO_DISP SET ";
                    SQL = SQL + ComNum.VBLF + " GBSTS ='" + strSTS + "' ,";
                    SQL = SQL + ComNum.VBLF + " GBEXAM ='" + strExam + "' ,";
                    SQL = SQL + ComNum.VBLF + " DEPTJTIME = SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + " RTIME ='" + strSTIME + "',";
                    SQL = SQL + ComNum.VBLF + " DRCODE ='" + strDrCode + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                    }
                }
                else
                {
                    if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

                    SQL = "";
                    SQL = " INSERT INTO KOSMOS_OCS.ENDO_DISP ";
                    SQL = SQL + ComNum.VBLF + " (PANO,BDATE,SNAME,DRCODE,GBSTS,GBEXAM,DEPTJTIME,RTIME,GBJOB,GUBUN ) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + " '" + strPano + "', TRUNC(SYSDATE),'" + strSName + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strDrCode + "','" + strSTS + "','" + strExam + "', SYSDATE ,'" + strSTIME + "','N','1' ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                SCREEN_CLEAR();
                btnSearch_Add_Click(null, null);
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

            if (FstrROWID == "")
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

                SQL = "";
                SQL = " DELETE FROM KOSMOS_OCS.ENDO_DISP  ";
                SQL = SQL + " WHERE ROWID ='" + FstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                SCREEN_CLEAR();
                btnSearch_Add_Click(null, null);
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

        private void ssView3_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strDrCode = "";

            SCREEN_CLEAR();

            FstrROWID = ssView3_Sheet1.Cells[e.Row, 8].Text.Trim();

            txtPano.Text = ssView3_Sheet1.Cells[e.Row, 0].Text.Trim();
            lblSName.Text = ssView3_Sheet1.Cells[e.Row, 1].Text.Trim();
            cboDrName.Text = ssView3_Sheet1.Cells[e.Row, 2].Text.Trim();
            cboSTS.Text = ssView3_Sheet1.Cells[e.Row, 3].Text.Trim();
            txtSTime.Text = ssView3_Sheet1.Cells[e.Row, 4].Text.Trim();
            cboExam.Text = ssView3_Sheet1.Cells[e.Row, 6].Text.Trim();
            strDrCode = ssView3_Sheet1.Cells[e.Row, 7].Text.Trim();

        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
