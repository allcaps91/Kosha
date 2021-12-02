using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmDataDictionary : Form
    {
        enum col_ss1 { Del, Code, Name, JDate, Deldate, Rowid, Change }

        public frmDataDictionary()
        {
            InitializeComponent();
        }

        void Screen_Clear()
        {
            grbJong.Enabled = true;
            btnView.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnPrint.Enabled = true;
            ss1_Sheet1.Rows.Count = 0;
            ss1.Enabled = false;
        }

        void ComboGubun_Set()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            cboJong.Items.Clear();
            cboJong.Items.Add("자료사전목록");

            //DB에 저장된 기본코드 종류를 읽음
            SQL = "SELECT Code,Name FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL = SQL + ComNum.VBLF + "WHERE Gubun='자료사전목록' ";
            SQL = SQL + ComNum.VBLF + "  AND (DelDate IS NULL OR DelDate>TRUNC(SYSDATE)) ";
            SQL = SQL + ComNum.VBLF + "ORDER BY Code ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cboJong.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
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
        }

        bool SaveData()
        {
            string strDel = "";
            string strGubun = "";
            string strCode = "";
            string strName = "";
            string strJDate = "";
            string strDeldate = "";
            string strRowid = "";
            string strChange = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                strGubun = cboJong.Text.Trim();

                for (int i = 0; i < ss1_Sheet1.RowCount; i++)
                {
                    strDel = ss1_Sheet1.Cells[i, (int)col_ss1.Del].Text;
                    strCode = ss1_Sheet1.Cells[i, (int)col_ss1.Code].Text;
                    strName = ss1_Sheet1.Cells[i, (int)col_ss1.Name].Text;
                    strJDate = ss1_Sheet1.Cells[i, (int)col_ss1.JDate].Text;
                    strDeldate = ss1_Sheet1.Cells[i, (int)col_ss1.Deldate].Text;
                    strRowid = ss1_Sheet1.Cells[i, (int)col_ss1.Rowid].Text;
                    strChange = ss1_Sheet1.Cells[i, (int)col_ss1.Change].Text;

                    if (strDel == "True")
                    {
                        //Delete
                        if (strRowid != "")
                        {
                            SQL = "DELETE KOSMOS_PMPA.BAS_BCODE ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strRowid + "' ";
                        }
                        else if (strChange == "Y")
                        {
                            if (strRowid == "")
                            {
                                //INSERT
                                SQL = "INSERT INTO KOSMOS_PMPA.BAS_BCODE (Gubun,Code,Name,JDate,DelDate,EntSabun,EntDate) ";
                                SQL = SQL + ComNum.VBLF + "VALUES ('" + strGubun + "','" + strCode + "','" + strName + "',";
                                SQL = SQL + ComNum.VBLF + "TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                                SQL = SQL + ComNum.VBLF + "TO_DATE('" + strDeldate + "','YYYY-MM-DD'),";
                                SQL = SQL + ComNum.VBLF + clsPublic.GnJobSabun + ",SYSDATE) ";
                            }
                            else if (strRowid != "")
                            {
                                //UPDATE
                                SQL = "UPDATE KOSMOS_PMPA.BAS_BCODE SET Code='" + strCode + "',";
                                SQL = SQL + ComNum.VBLF + "      Name='" + strName + "',";
                                SQL = SQL + ComNum.VBLF + "      JDate=TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                                SQL = SQL + ComNum.VBLF + "      DelDate=TO_DATE('" + strDeldate + "','YYYY-MM-DD'),";
                                SQL = SQL + ComNum.VBLF + "      EntSabun=" + clsPublic.GnJobSabun + ",";
                                SQL = SQL + ComNum.VBLF + "      EntDate=SYSDATE ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strRowid + "' ";
                            }
                        }
                    }
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

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("체크한 행 처리 완료.");
                Cursor.Current = Cursors.Default;

                if (strGubun == "자료사전목록") { ComboGubun_Set(); }
                Screen_Clear();
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

        bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            string strGubun = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            ss1.Enabled = true;

            if (cboJong.Text.Trim() == "") { MessageBox.Show("구분이 공란입니다.", "오류"); return false; }

            string[] separators = { "." };
            strGubun = cboJong.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries)[0];

            if (strGubun == "자료사전목록")
            {
                ss1_Sheet1.SetColumnWidth((int)col_ss1.Code, 180);
                ss1_Sheet1.Columns[3].Visible = false; //적용일자
                ss1_Sheet1.Columns[4].Visible = false; //삭제일자
            }
            else
            {
                ss1_Sheet1.SetColumnWidth((int)col_ss1.Code, 50);
                ss1_Sheet1.Columns[3].Visible = true; //적용일자
                ss1_Sheet1.Columns[4].Visible = true; //삭제일자
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT Code,Name,TO_CHAR(JDate,'YYYY-MM-DD') JDate,  ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,ROWID   ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE                    ";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun='" + strGubun + "'                 ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Code                                  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }

                ss1_Sheet1.RowCount = dt.Rows.Count + 20;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, (int)col_ss1.Del].Text = "";
                    ss1_Sheet1.Cells[i, (int)col_ss1.Code].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ss1_Sheet1.Cells[i, (int)col_ss1.Name].Text = dt.Rows[i]["Name"].ToString().Trim();
                    if (strGubun != "자료사전목록")
                    {
                        ss1_Sheet1.Cells[i, (int)col_ss1.JDate].Text = dt.Rows[i]["JDate"].ToString().Trim();
                        ss1_Sheet1.Cells[i, (int)col_ss1.Deldate].Text = dt.Rows[i]["DelDate"].ToString().Trim();
                    }
                    ss1_Sheet1.Cells[i, (int)col_ss1.Rowid].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ss1_Sheet1.Cells[i, (int)col_ss1.Change].Text = "";
                }

                dt.Dispose();
                dt = null;
                grbJong.Enabled = false;
                btnView.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void frmDataDictionary_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ss1_Sheet1.Columns[5].Visible = false; //ROWID
            ss1_Sheet1.Columns[6].Visible = false; //변경
            ssPrint_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            ComboGubun_Set();
            Screen_Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrintDic_Click(object sender, EventArgs e)
        {
            int nRow = 0;
            int nLen = 0;
            string strList = "";
            string strData = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt2 = null;

            ssPrint_Sheet1.Rows.Count = 50;
            ssPrint_Sheet1.ClearRange(0, 0, ss1_Sheet1.Rows.Count, ss1_Sheet1.Columns.Count, true);

            Cursor.Current = Cursors.WaitCursor;

            //자료사전 목록
            SQL = "SELECT Code FROM " + ComNum.VBLF + "BAS_BCODE ";
            SQL = SQL + ComNum.VBLF + "WHERE Gubun='자료사전목록' ";
            SQL = SQL + ComNum.VBLF + "  AND DelDate IS NULL ";
            SQL = SQL + ComNum.VBLF + "ORDER BY Code ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            ss1_Sheet1.RowCount = dt.Rows.Count;

            nRow = -1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nRow += 2;
                if (nRow > ssPrint_Sheet1.Rows.Count) { ssPrint_Sheet1.Rows.Count = nRow; }
                ssPrint_Sheet1.Cells[nRow - 1, 0].Text = " ▶" + dt.Rows[i]["Code"].ToString().Trim();

                //세부항목
                SQL = "SELECT Code,Name,TO_CHAR(JDate,'YYYY-MM-DD') JDate,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.VBLF + "BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun='" + dt.Rows[i]["Code"].ToString().Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DelDate IS NULL ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Code ";

                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                strList = VB.Space(3);

                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    strData = dt2.Rows[j]["Code"].ToString().Trim() + ".";
                    strData += dt2.Rows[j]["Name"].ToString().Trim();
                    nLen = strList.Length + strData.Length;
                    if (nLen >= 70)
                    {
                        nRow += 1;
                        if (nRow > ssPrint_Sheet1.Rows.Count) { ssPrint_Sheet1.Rows.Count = nRow; }
                        ssPrint_Sheet1.Cells[nRow - 1, 0].Text = strList;
                        strList = VB.Space(3) + strData + " ";
                    }
                    else
                    {
                        strList += strData + " ";
                    }
                }
                nRow += 1;
                if (nRow > ssPrint_Sheet1.Rows.Count) { ssPrint_Sheet1.Rows.Count = nRow; }
                ssPrint_Sheet1.Cells[nRow - 1, 0].Text = strList;
                strList = "";
            }
            dt.Dispose();
            dt2.Dispose();
            dt = null;
            dt2 = null;
            ssPrint_Sheet1.Rows.Count = nRow;

            //자료를 인쇄
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "♧ 자 료 사 전   코 드 집 ♧ ";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("☞출력일자:" + VB.Now().ToString() + "☞Page:" + "/p" + clsPublic.GstrJobName, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, true, false, false);

            SPR.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);
            Cursor.Current = Cursors.Default;
        }

        private void ss1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != 0) { return; }

            ss1_Sheet1.Cells[e.Row, 6].Text = "Y";
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void ss1_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            ss1_Sheet1.Cells[e.Row, 6].Text = "Y";
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }
        private void cboJong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ViewData() == false) return;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == false) return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            btnView.Focus();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ss1_Sheet1.Rows.Count = ss1_Sheet1.NonEmptyRowCount;

            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = VB.Right(cboJong.Text, cboJong.Text.Length - 4);

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, true, false, false);

            SPR.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);

            ss1_Sheet1.Rows.Count += 20;
        }
    }
}
