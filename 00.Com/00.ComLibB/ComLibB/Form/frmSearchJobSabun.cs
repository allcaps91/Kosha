using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmSearchJobSabun.cs
    /// Description     : 업무별 작업가능 사번 관리
    /// Author          : 박창욱
    /// Create Date     : 2019-02-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\basic\bucode\Frm업무별작업가능사번.frm(Frm업무별작업가능사번) >> frmSearchJobSabun.cs 폼이름 재정의" />	
    public partial class frmSearchJobSabun : Form
    {
        public frmSearchJobSabun()
        {
            InitializeComponent();
        }

        private void frmSearchJobSabun_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            ssView_Sheet1.Columns[5].Visible = false;   //ROWID
            ssView_Sheet1.Columns[6].Visible = false;   //변경
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            ssPrint_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strList = string.Empty;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT a.Buse,b.Name,COUNT(*) CNT ";
                SQL += ComNum.VBLF + " FROM KOSMOS_ADM.INSA_MST a,KOSMOS_PMPA.BAS_BUSE b ";
                SQL += ComNum.VBLF + " WHERE a.ToiDay IS NULL "; //퇴사자 제외
                SQL += ComNum.VBLF + "   AND a.Gubun='0' ";
                SQL += ComNum.VBLF + "   AND a.Buse=b.BuCode(+) ";
                SQL += ComNum.VBLF + " GROUP BY a.Buse,b.Name ";
                SQL += ComNum.VBLF + " ORDER BY a.Buse ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                cboBuse.Items.Clear();
                cboBuse.Items.Add("전체부서");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strList = dt.Rows[i]["BUSE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim();
                    cboBuse.Items.Add(strList);
                }
                dt.Dispose();
                dt = null;

                cboBuse.SelectedIndex = 0;

                cboGubun_Set();
                Screen_Clear();

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

        void Screen_Clear()
        {
            cboJong.Enabled = true;
            btnSearch.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnPrint.Enabled = true;
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;
            ssView.Enabled = false;
            ssList.Enabled = false;
        }

        void cboGubun_Set()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strDat = string.Empty;

            cboJong.Items.Clear();
            cboJong.Items.Add("작업목록");

            //DB에 저장된 기본코드 종류를 읽음
            SQL = "";
            SQL = "SELECT Code,Name FROM KOSMOS_PMPA.BAS_JOB_SABUN ";
            SQL += ComNum.VBLF + "WHERE Gubun='작업목록' ";
            SQL += ComNum.VBLF + "  AND (DelDate IS NULL OR DelDate>TRUNC(SYSDATE)) ";
            SQL += ComNum.VBLF + "ORDER BY Code ";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboJong.Items.Add(dt.Rows[i]["CODE"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                if (cboJong.Items.Count > 1)
                {
                    cboJong.SelectedIndex = 1;
                }
                else
                {
                    cboJong.SelectedIndex = 0;
                }
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            btnSearch.Focus();
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

            string strDel = string.Empty;
            string strGubun = string.Empty;
            string strCode = string.Empty;
            string strName = string.Empty;
            string strJDate = string.Empty;
            string strDeldate = string.Empty;
            string strROWID = string.Empty;
            string strChange = string.Empty;

            strGubun = cboJong.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.NonEmptyRowCount; i++)
                {
                    strDel = ssView_Sheet1.Cells[i, 0].Text.Trim();
                    strCode = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strName = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    strJDate = ssView_Sheet1.Cells[i, 3].Text.Trim();
                    strDeldate = ssView_Sheet1.Cells[i, 4].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i, 5].Text.Trim();
                    strChange = ssView_Sheet1.Cells[i, 6].Text.Trim();

                    if (strDel.Equals("1"))
                    {
                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = "DELETE KOSMOS_PMPA.BAS_JOB_SABUN ";
                            SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

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
                    else if (strChange.Equals("Y"))
                    {
                        if (strROWID == "")
                        {
                            SQL = "";
                            SQL = "INSERT INTO KOSMOS_PMPA.BAS_JOB_SABUN (Gubun,Code,Name,JDate,DelDate,EntSabun,EntDate) ";
                            SQL += ComNum.VBLF + "VALUES ('" + strGubun + "','" + strCode + "','" + strName + "',";
                            SQL += ComNum.VBLF + "TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                            SQL += ComNum.VBLF + "TO_DATE('" + strDeldate + "','YYYY-MM-DD'),";
                            SQL += ComNum.VBLF + clsType.User.IdNumber + ",SYSDATE) ";

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
                        else
                        {
                            SQL = "";
                            SQL = "UPDATE KOSMOS_PMPA.BAS_JOB_SABUN SET Code='" + strCode + "',";
                            SQL += ComNum.VBLF + "      Name='" + strName + "',";
                            SQL += ComNum.VBLF + "      JDate=TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                            SQL += ComNum.VBLF + "      DelDate=TO_DATE('" + strDeldate + "','YYYY-MM-DD'),";
                            SQL += ComNum.VBLF + "      EntSabun=" + clsType.User.IdNumber + ",";
                            SQL += ComNum.VBLF + "      EntDate=SYSDATE ";
                            SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

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
                }

                if (strGubun.Equals("작업목록"))
                {
                    cboGubun_Set();
                }

                Screen_Clear();

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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            ssView_Sheet1.RowCount = ssView_Sheet1.NonEmptyRowCount;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = VB.Right(cboJong.Text, cboJong.Text.Length - 4) + "자료사전";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);

            ssView_Sheet1.RowCount += 20;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strGubun = string.Empty;

            if (cboJong.Text.Trim() == "")
            {
                ComFunc.MsgBox("구분이 공란입니다.");
                return;
            }

            strGubun = VB.Pstr(cboJong.Text, ".", 1).Trim();

            if (strGubun.Equals("작업목록"))
            {
                ssList.Enabled = false;
                ssView_Sheet1.Columns[1].Width = 60;
                ssView_Sheet1.Columns[3].Visible = false;   //적용일자
                ssView_Sheet1.Columns[4].Visible = false;   //삭제일자
            }
            else
            {
                ssList.Enabled = true;
                ssView_Sheet1.Columns[1].Width = 2;
                ssView_Sheet1.Columns[3].Visible = true;   //적용일자
                ssView_Sheet1.Columns[4].Visible = true;   //삭제일자
            }

            ssView.Enabled = true;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //현재 자료를 READ
                SQL = "";
                SQL = "SELECT Code,Name,TO_CHAR(JDate,'YYYY-MM-DD') JDate,";
                SQL += ComNum.VBLF + " TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,ROWID ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_JOB_SABUN ";
                SQL += ComNum.VBLF + "WHERE Gubun='" + strGubun + "' ";
                SQL += ComNum.VBLF + "ORDER BY Code ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count + 20;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = "";
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    if (strGubun.Equals("작업목록") == false)
                    {
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["JDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DELDATE"].ToString().Trim();
                    }
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = "";
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                cboJong.Enabled = false;
                btnSearch.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void cboBuse_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //근무중인 모든 직원명단을 표시
                SQL = "";
                SQL = "SELECT Sabun,KorName FROM KOSMOS_ADM.INSA_MST ";
                SQL += ComNum.VBLF + " WHERE ToiDay IS NULL "; //퇴사자 제외
                if (cboBuse.Text != "전체부서")
                {
                    SQL += ComNum.VBLF + " AND Buse='" + VB.Pstr(cboBuse.Text, ".", 1).Trim() + "' ";
                }
                SQL += ComNum.VBLF + "   AND Sabun < '80' ";
                SQL += ComNum.VBLF + " ORDER BY KorName ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssList_Sheet1.RowCount = 0;
                ssList_Sheet1.RowCount = dt.Rows.Count + 1;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = VB.Left(dt.Rows[i]["SABUN"].ToString().Trim() + VB.Space(7), 7);
                    ssList_Sheet1.Cells[i, 0].Text += dt.Rows[i]["KORNAME"].ToString().Trim();
                }
                ssList_Sheet1.Cells[dt.Rows.Count, 0].Text = "04349  의료정보과";

                dt.Dispose();
                dt = null;
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

        private void cboJong_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                btnSearch.Focus();
            }
        }

        private void ssList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            int i = 0;
            string strChk = string.Empty;
            string strSabun = string.Empty;
            string strName = string.Empty;

            strSabun = VB.Left(ssView_Sheet1.Cells[e.Row, 0].Text.Trim(), 6);
            strName = VB.Right(ssView_Sheet1.Cells[e.Row, 0].Text.Trim(), ssView_Sheet1.Cells[e.Row, 0].Text.Trim().Length - 6);
            strName = VB.TR(strName, " ", "");

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                strChk = Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true ? "1" : "";
                if (strChk != "1")
                {
                    if (ssView_Sheet1.Cells[i, 1].Text.Trim().Equals(strSabun))
                    {
                        return;
                    }

                    if (ssView_Sheet1.Cells[i, 1].Text.Trim() == "")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = strSabun;
                        ssView_Sheet1.Cells[i, 2].Text = strName;
                        ssView_Sheet1.Cells[i, 6].Text = "Y";
                    }
                }
            }

            ssView_Sheet1.RowCount++;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = strSabun;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = strName;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "Y";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCode_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            int nLen = 0;
            int nRow = 0;
            string strList = string.Empty;
            string strData = string.Empty;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            Cursor.Current = Cursors.WaitCursor;

            ssPrint_Sheet1.RowCount = 0;
            ssPrint_Sheet1.RowCount = 50;

            try
            {
                //자료사전 목록을 읽음
                SQL = "";
                SQL = "SELECT Code FROM KOSMOS_PMPA.BAS_JOB_SABUN ";
                SQL += ComNum.VBLF + "WHERE Gubun='작업목록' ";
                SQL += ComNum.VBLF + "  AND DelDate IS NULL ";
                SQL += ComNum.VBLF + "ORDER BY Code ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRow = -1;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow += 2;
                    if (nRow > ssPrint_Sheet1.RowCount)
                    {
                        ssPrint_Sheet1.RowCount = nRow;
                    }

                    ssPrint_Sheet1.Cells[nRow - 1, 0].Text = " ▶" + dt.Rows[i]["CODE"].ToString().Trim();

                    //세부항목을 읽음
                    SQL = "";
                    SQL = "SELECT Code,Name,TO_CHAR(JDate,'YYYY-MM-DD') JDate,";
                    SQL += ComNum.VBLF + " TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,ROWID ";
                    SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_JOB_SABUN ";
                    SQL += ComNum.VBLF + "WHERE Gubun='" + dt.Rows[i]["Code"].ToString().Trim() + "' ";
                    SQL += ComNum.VBLF + "  AND DelDate IS NULL ";
                    SQL += ComNum.VBLF + "ORDER BY Code ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    strList = VB.Space(3);

                    for (k = 0; k < dt1.Rows.Count; k++)
                    {
                        strData = dt1.Rows[k]["CODE"].ToString().Trim() + "." + dt1.Rows[k]["NAME"].ToString().Trim();
                        nLen = (int)(ComFunc.LenH(strList) + ComFunc.LenH(strData));
                        if (nLen >= 100)
                        {
                            nRow++;
                            if (nRow > ssPrint_Sheet1.RowCount)
                            {
                                ssPrint_Sheet1.RowCount = nRow;
                            }
                            ssPrint_Sheet1.Cells[nRow - 1, 0].Text = strList;
                            strList = VB.Space(3) + strData + " ";
                        }
                        else
                        {
                            strList += strData + " ";
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    nRow++;
                    if (nRow > ssPrint_Sheet1.RowCount)
                    {
                        ssPrint_Sheet1.RowCount = nRow;
                    }

                    ssPrint_Sheet1.Cells[nRow - 1, 0].Text = strList;
                    strList = "";
                }
                dt.Dispose();
                dt = null;

                ssPrint_Sheet1.RowCount = nRow;
                ssPrint_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "♧ 업무별 작업가능 직원 명단 ♧";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
        }

        private void ssView_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column != 0)
            {
                return;
            }

            ssView_Sheet1.Cells[e.Row, 6].Text = "Y";
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void ssView_Change(object sender, ChangeEventArgs e)
        {
            ssView_Sheet1.Cells[e.Row, 6].Text = "Y";
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }
    }
}

