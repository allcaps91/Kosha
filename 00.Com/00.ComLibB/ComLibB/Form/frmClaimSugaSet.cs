using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmClaimSugaSet
    /// File Name : frmClaimSugaSet.cs
    /// Title or Description : 기타 코드 설정 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-05
    /// <history> 
    /// </history>
    /// </summary>
    public partial class frmClaimSugaSet : Form
    {
        public delegate void REventExit(object sender, EventArgs e);
        public static event REventExit rEventExit;

        //부모 폼에서 넘겨받는 데이터
        string GstrHelpCode = "";
        string GstrPano = "";

        public frmClaimSugaSet()
        {
            InitializeComponent();
        }

        public frmClaimSugaSet(string HelpCode)
        {
            InitializeComponent();
            GstrHelpCode = HelpCode;
        }

        public frmClaimSugaSet(string HelpCode, string ArgPano)
        {
            InitializeComponent();
            GstrHelpCode = HelpCode;
            GstrPano = ArgPano;
        }

        private void Screen_Clear()
        {
            cboJong.Enabled = true;
            btnView.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            ss1_Sheet1.Rows.Count = 0;
            ss1.Enabled = false;
        }

        private void ComboGubun_SET(string argGubun)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            cboJong.Items.Clear();

            //DB에 저장된 기본코드 종류를 읽음
            SQL = "";
            SQL += ComNum.VBLF + "SELECT Code,Name FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "WHERE Gubun ='자료사전목록' ";
            SQL += ComNum.VBLF + "  AND Name IN ('" + argGubun + "')";
            SQL += ComNum.VBLF + "  AND (DelDate IS NULL OR DelDate>TRUNC(SYSDATE)) ";
            SQL += ComNum.VBLF + "ORDER BY Code ";
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

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cboJong.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
            }
            dt.Dispose();
            dt = null;

            cboJong.SelectedIndex = 0;
            cboJong.Enabled = false;
        }

        private void frmClaimSugaSet_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ss1_Sheet1.Columns[3].Visible = false;
            ss1_Sheet1.Columns[4].Visible = false;
            ss1_Sheet1.Columns[5].Visible = false;
            ss1_Sheet1.Columns[6].Visible = false;
            ss1_Sheet1.Columns[7].Visible = false;

            lblInfo.Text = GstrHelpCode;

            if (GstrHelpCode.Contains("특수약제_등록번호별담당자"))
            {
                ss1_Sheet1.Columns[7].Visible = true;

                ss1_Sheet1.ColumnHeader.Columns[8].Label = "성명";
                ss1_Sheet1.Columns[8].Visible = true;
            }
            else
            {
                ss1_Sheet1.ColumnHeader.Columns[8].Label = "";
                ss1_Sheet1.Columns[8].Visible = false;
            }

            //TEST 할때는 코드 넣어서 함
            ComboGubun_SET(GstrHelpCode);
            Screen_Clear();
            btnView.PerformClick();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Screen_Clear();

            if (rEventExit != null)
            {
                rEventExit(sender, e);
            }

            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ViewData() == false) return;
        }

        private bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            char separator = '.';
            string[] valGUBUN = cboJong.Text.Split(separator);
            string strGUBUN = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            ss1.Enabled = true;

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                if (cboJong.Text.Trim() == "")
                {
                    MessageBox.Show("구분이 공란입니다.", "오류");
                    return false;
                }
                strGUBUN = valGUBUN[0].Trim();

                if (strGUBUN == "자료사전등록")
                {
                    ss1_Sheet1.Columns[3].Visible = false;
                    ss1_Sheet1.Columns[4].Visible = false;
                }
                else
                {
                    //주석처리됨
                }

                SQL = "";
                SQL += "SELECT Code,Name,TO_CHAR(JDate,'YYYY-MM-DD') JDate,  " + ComNum.VBLF;
                SQL += " TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,Gubun2,ROWID  " + ComNum.VBLF;
                SQL += " FROM " + ComNum.DB_PMPA + "BAS_BCODE                " + ComNum.VBLF;
                SQL += "WHERE Gubun='" + strGUBUN + "'                       " + ComNum.VBLF;
                SQL += "ORDER BY Code                                        " + ComNum.VBLF;
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ss1_Sheet1.Rows.Count = 20;
                    cboJong.Enabled = false;
                    btnView.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    return false;
                }
                ss1_Sheet1.Rows.Count = dt.Rows.Count + 20;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = "";
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Name"].ToString().Trim();
                    if(strGUBUN != "자료사전목록")
                    {
                        ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["JDate"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DelDate"].ToString().Trim();
                    }
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 6].Text = "";
                    ss1_Sheet1.Cells[i, 7].Text = "";
                    if (strGUBUN.Contains("특수약제_등록번호별담당자"))
                    {
                        ss1_Sheet1.Columns[3].Visible = true;
                        ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Gubun2"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 8].Text = Read_PatientName(dt.Rows[i]["Code"].ToString().Trim());
                        //ss1_Sheet1.Cells[i, 9].Text = Read_PatientName(dt.Rows[i]["JDATE"].ToString().Trim());
                        if(GstrPano == dt.Rows[i]["Code"].ToString().Trim())
                        {
                            ss1_Sheet1.Cells[i, 0].Text = "1";
                            ss1_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 193, 255);
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                cboJong.Enabled = false;
                btnView.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        //TODO: VB READ_PatientName(vbfunc.bas 모듈)함수 임시로 만들어서 사용
        private string Read_PatientName(string argPano)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";
            DataTable dt = null;
            try
            {
                if (VB.Val(argPano) == 0)
                {
                    rtnVal = "";
                }

                SQL = "";
                SQL += ComNum.VBLF + "SELECT SName FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + " WHERE Pano='" + argPano + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    //dt.Dispose();
                    //dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SName"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }

            return rtnVal;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == false) return;
        }

        private bool SaveData()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인

            string strDel = "";
            string strGUBUN = "";
            string strCODE = "";
            string strName = "";
            string strJDate = "";
            string strDeldate = "";
            string strROWID = "";
            string strChange = "";
            string strGUBUN2 = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strMSG = "";
            DataTable dt = null;

            strGUBUN = cboJong.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            
            try
            {
                for (int i = 0; i < ss1_Sheet1.NonEmptyRowCount; i++)
                {
                    strDel = ss1_Sheet1.Cells[i, 0].Text;
                    strCODE = ss1_Sheet1.Cells[i, 1].Text.Trim().ToUpper();
                    strName = ss1_Sheet1.Cells[i, 2].Text.Trim();
                    strJDate = ss1_Sheet1.Cells[i, 3].Text.Trim();
                    strDeldate = ss1_Sheet1.Cells[i, 4].Text.Trim();
                    strROWID = ss1_Sheet1.Cells[i, 5].Text.Trim();
                    strChange = ss1_Sheet1.Cells[i, 6].Text.Trim();

                    if(i == 439)
                    {
                        i = i;
                    }

                    if (strGUBUN.Contains("특수약제_등록번호별담당자"))
                    {
                        strGUBUN2 = ss1_Sheet1.Cells[i, 7].Text.Trim().ToUpper();
                    }
                    else
                    {
                        strGUBUN2 = "";
                    }

                    if (strDel == "True")
                    {
                        if (strROWID != "")
                        {
                            //GoSub CmdOK_DELETE 체크된 데이터 삭제한다.
                            SQL = "DELETE " + ComNum.DB_PMPA + "BAS_BCODE ";
                            SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                    }

                    else if (strChange == "Y")
                    {
                        if (strROWID == "")
                        {
                            SQL = "";
                            SQL += "SELECT * FROM KOSMOS_PMPA.BAS_BCODE " + ComNum.VBLF;
                            SQL += "WHERE Gubun='" + strGUBUN + "'      " + ComNum.VBLF;
                            SQL += "AND CODE='" + strCODE + "'          " + ComNum.VBLF;
                            
                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            if(dt.Rows.Count > 0)
                            {
                                strMSG += "중복 등록번호 : " + strCODE + ComNum.VBLF;
                            }

                            if(dt.Rows.Count == 0)
                            {
                                //GoSub CmdOK_INSERT ROWID가 NULL인 행의 데이터를 INSERT
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_BCODE (Gubun,Gubun2,Code,Name,JDate,DelDate,EntSabun,EntDate) ";
                                SQL += ComNum.VBLF + "VALUES ('" + strGUBUN + "','" + strGUBUN2 + "','" + strCODE + "','" + strName + "',";
                                SQL += ComNum.VBLF + "TO_DATE('" + ComFunc.FormatStrToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"), "D") + "','YYYY-MM-DD'),";
                                SQL += ComNum.VBLF + "TO_DATE('" + strDeldate + "','YYYY-MM-DD'),";
                                SQL += ComNum.VBLF + clsType.User.Sabun + ",SYSDATE) ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            //GoSub CmdOK_UPDATE ROWID가 NULL이 아닌 행 데이터 Update
                            SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_BCODE SET Code='" + strCODE + "',";
                            SQL += ComNum.VBLF + "      Gubun2='" + strGUBUN2 + "',";
                            SQL += ComNum.VBLF + "      Name='" + strName + "',";
                            SQL += ComNum.VBLF + "      JDate=TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                            SQL += ComNum.VBLF + "      DelDate=TO_DATE('" + strDeldate + "','YYYY-MM-DD'),";
                            SQL += ComNum.VBLF + "      EntSabun=" + clsType.User.Sabun + ",";
                            SQL += ComNum.VBLF + "      EntDate=SYSDATE ";
                            SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox(strMSG);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                if ( strGUBUN == "자료사전목록")
                {
                    ComboGubun_SET(GstrHelpCode);
                }

                Screen_Clear();
                btnView.PerformClick();
                return true;
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            btnView.Focus();
        }

        private void ss1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != 0)
            {
                return;
            }

            ss1_Sheet1.Cells[e.Row, 6].Text = "Y";

            if (ss1_Sheet1.Cells[e.Row, e.Column].Text == "True")
            {
                ss1_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(255, 193, 255);
            }
            else
            {
                ss1_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(255, 255, 255);
            }

            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void ss1_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strData = "";
            string strChk = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            switch (lblInfo.Text)
            {
                case "소화성궤양용제_공격인자": 
                case "소화성궤양용제_공격인자2": 
                case "소화성궤양용제_방어인자":
                    strChk = "OK";
                    break;
                case "비스테로이드성_소염진통제":
                    strChk = "OK";
                    break;
            }

            ss1_Sheet1.Cells[e.Row, 6].Text = "Y";
            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            if (e.Column == 1 && strChk == "OK")
            {
                strData = ss1_Sheet1.Cells[e.Row, e.Column].Text.ToUpper().Trim();
                SQL = "SELECT SuNameK FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                SQL += ComNum.VBLF + "WHERE SuNext = '" + strData + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if ( dt.Rows.Count == 0)
                {
                    ss1_Sheet1.Cells[e.Row, 1].Text = "";
                    ss1_Sheet1.Cells[e.Row, 2].Text = "";
                }
                else
                {
                    ss1_Sheet1.Cells[e.Row, 2].Text = dt.Rows[0]["SuNameK"].ToString();
                }                
            }
            else if( e.Column == 1 && lblInfo.Text.Contains("특수약제_등록번호별담당자") && ss1.ActiveSheet.Cells[e.Row, e.Column].Text.Trim() != "")
            {
                strData = ss1_Sheet1.Cells[e.Row, e.Column].Text.ToUpper().Trim();
                ss1_Sheet1.Cells[e.Row, 8].Text = Read_PatientName(strData);
            }
        }

        private void ss1_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {

        }
    }
}
