using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupMedLibB
{
    public partial class frmBaptismList : Form
    {
        string GstrSEQNO = "";
        string[] GstrColName = null;
        string[] GstrColValue = null;

        public frmBaptismList()
        {
            InitializeComponent();
        }

        private void frmBaptismList_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            int i = 0;
            string strNameS = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.RowCount = 0;

            strNameS = txtAName.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT SEQNO, PANO, PART01, PART02, PART03, PART04, PART05, PART06, PART07, PART08, PART09, PART10, PART11, PART12, PART13, PART14, PART15, PART16, PART17, PART18, PART19, PART20, PART21, PART22, PART23, PART24, PART25, PART26, PART27, PART28, PART29, PART30, ";
                SQL = SQL + ComNum.VBLF + "PART31, PART32, PART33, PART34, PART35, PART36, PART37, PART38, PART39, PART40, PART41, PART42, PART43, PART44, PART45, PART46, PART47, PART48, PART49, PART50, PART51, PART52";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.WONMOK_LIST4 ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";

                if (txtAName.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND PART01 = '" + strNameS + "'";
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY PART51 ASC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();

                        if (dt.Rows[i]["PART41"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PART01"].ToString().Trim() + "(" + dt.Rows[i]["PART41"].ToString().Trim() + ")";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PART01"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PART02"].ToString().Trim() + "-" + dt.Rows[i]["PART52"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PART03"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["PART05"].ToString().Trim() + " " + dt.Rows[i]["PART51"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["PART06"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["PART14"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["PART13"].ToString().Trim();

                        if (dt.Rows[i]["PART07"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["PART07"].ToString().Trim() + "(" + dt.Rows[i]["PART08"].ToString().Trim() + ")";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["PART07"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["PART11"].ToString().Trim();
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

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            GstrSEQNO = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "SELECT MAX(SEQNO) AS SEQNO FROM KOSMOS_ADM.WONMOK_LIST4 WHERE SEQNO LIKE TO_CHAR(SYSDATE, 'YYYY') || '%'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            txtBapName.Text = (Convert.ToInt32(VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim())) + 1).ToString().Trim();

            if (txtBapName.Text == "1")
            {
                txtBapName.Text = (Convert.ToDateTime(VB.Left(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), 8) + "000")).ToString();
            }

            txtTime.Text = "00:00";
            txtName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strROWID = "";

            txtJumin2.Text = VB.Left(txtJumin2.Text, 1) + "******";

            if (VB.Val(VB.Right(txtBapDate.Text, 2)) > 31 || VB.Val(VB.Right(txtBirthDate.Text, 2)) > 31)
            {
                ComFunc.MsgBox("'일' 의 숫자가 1~31에 포함되지 않습니다. 재입력 해주세요.");
                return;
            }
            else if (VB.Val(VB.Mid(txtBapDate.Text, 6, 2)) > 12 || VB.Val(VB.Mid(txtBirthDate.Text, 6, 2)) > 12)
            {
                ComFunc.MsgBox("'월' 의 숫자가 1~12에 포함되지 않습니다. 재입력 해주세요.");
                return;
            }

            strROWID = readROWID(txtBapName.Text);

            if (strROWID != "")
            {
                dataDelete(strROWID);
            }

            SaveData(strROWID);

            //if (clsDB.ExecuteNonQuery(createInsertSql(strROWID, txtBapName.Text, "WRITEDATE, WRITESABUN", "SYSDATE, "), ref intRowAffected, clsDB.DbCon) == "")
            //{
            //    ComFunc.MsgBox("저장 중 에러 발생");
            //    return;
            //}

            Screen_Clear();
            GetData();
            txtBapName.Text = "";
        }

        private void SaveData(string strRowid)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "INSERT INTO KOSMOS_ADM.WONMOK_LIST4";
                SQL = SQL + ComNum.VBLF + "(";
                SQL = SQL + ComNum.VBLF + "     SEQNO, PART01, PART03, PART41, PART02, PART52, PART04, PART19, PART20, PART21, PART22, PART23, PART24, PART25, ";
                SQL = SQL + ComNum.VBLF + "     PART26, PART05, PART51, PART06, PART28, PART27, PART29, PART30, PART32, PART31, PART33, PART34, PART36, PART35, ";
                SQL = SQL + ComNum.VBLF + "     PART37, PART38, PART13, PART14, PART15, PART16, PART17, PART18, PART07, PART08, PART10, PART09, PART11, PART12, ";
                SQL = SQL + ComNum.VBLF + "     PART40, PART39, WRITEDATE, WRITESABUN";
                SQL = SQL + ComNum.VBLF + ")";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "(";
                SQL = SQL + ComNum.VBLF + "     '" + txtBapName.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtName.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + cboSexsel.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtSereName.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtJumin1.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtJumin2.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtAddress.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtBName1.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtBRelation1.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtBAddress1.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtBContact1.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtBName2.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtBRelation2.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtBAddress2.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtBContact2.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtBapDate.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtTime.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtBapSpace.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtFRelation1.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtFName1.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtFSereName1.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtFEtc1.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtFRelation2.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtFName2.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtFSereName2.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtFEtc2.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtFRelation3.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtFName3.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtFSereName3.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtFEtc3.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtHealth.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtDis.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtExplainYN.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtBelieveYN.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtParentYN.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtUnconsciousYN.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtBonName.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtSereName1.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtDAddress.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtDContact.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtJName.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtBirthDate.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtBonSend.Text + "',";
                SQL = SQL + ComNum.VBLF + "     '" + txtEtc.Text + "',";
                SQL = SQL + ComNum.VBLF + "     SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "     '" + clsType.User.Sabun + "'";
                SQL = SQL + ComNum.VBLF + ")";

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
            //if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            strTitle = "";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
            CS = null;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string strROWID = "";

            if (GstrSEQNO == "")
            {
                ComFunc.MsgBox("저장 된 내용이 없습니다.");
            }
            else
            {
                if (ComFunc.MsgBoxQ("작성된 내용을 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                strROWID = readROWID(GstrSEQNO);

                if (strROWID != "")
                {
                    dataDelete(strROWID);

                }

                ComFunc.MsgBox("삭제되었습니다.");

                Screen_Clear();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReadInfo(string arg2)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT KORNAME, JUSO, SERENAME, HTEL";
                SQL = SQL + ComNum.VBLF + " From KOSMOS_ADM.INSA_MST";
                SQL = SQL + ComNum.VBLF + " WHERE KORNAME = '" + arg2 + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtBonName.Text = dt.Rows[0]["KORNAME"].ToString().Trim();
                    txtSereName1.Text = dt.Rows[0]["SERENAME"].ToString().Trim();
                    txtDAddress.Text = dt.Rows[0]["JUSO"].ToString().Trim();
                    txtDContact.Text = dt.Rows[0]["HTEL"].ToString().Trim();
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

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            Screen_Clear();

            GstrSEQNO = ssView_Sheet1.Cells[e.Row, 0].Text;

            txtBapName.Text = GstrSEQNO;

            if (GstrSEQNO != "")
            {
                dataView(readROWID(GstrSEQNO));

                ssPrint_Sheet1.Cells[4, 4].Text = txtName.Text;
                ssPrint_Sheet1.Cells[4, 9].Text = txtSereName.Text;
                ssPrint_Sheet1.Cells[5, 4].Text = txtJumin1.Text + "-" + txtJumin2.Text;
                ssPrint_Sheet1.Cells[5, 9].Text = cboSexsel.Text;
                ssPrint_Sheet1.Cells[6, 4].Text = txtAddress.Text;
                ssPrint_Sheet1.Cells[7, 4].Text = txtBName1.Text;
                ssPrint_Sheet1.Cells[7, 9].Text = txtBRelation1.Text;
                ssPrint_Sheet1.Cells[7, 11].Text = txtBContact1.Text;
                ssPrint_Sheet1.Cells[8, 4].Text = txtBAddress1.Text;
                ssPrint_Sheet1.Cells[9, 4].Text = txtBName2.Text;
                ssPrint_Sheet1.Cells[9, 9].Text = txtBRelation2.Text;
                ssPrint_Sheet1.Cells[9, 11].Text = txtBContact2.Text;
                ssPrint_Sheet1.Cells[10, 4].Text = txtBAddress2.Text;
                ssPrint_Sheet1.Cells[11, 4].Text = txtBapDate.Text;
                ssPrint_Sheet1.Cells[11, 9].Text = txtTime.Text;
                ssPrint_Sheet1.Cells[11, 12].Text = txtBapSpace.Text;
                ssPrint_Sheet1.Cells[16, 1].Text = txtFRelation1.Text;
                ssPrint_Sheet1.Cells[16, 3].Text = txtFName1.Text;
                ssPrint_Sheet1.Cells[16, 4].Text = txtFSereName1.Text;
                ssPrint_Sheet1.Cells[16, 6].Text = txtFEtc1.Text;
                ssPrint_Sheet1.Cells[17, 3].Text = txtFName2.Text;
                ssPrint_Sheet1.Cells[17, 4].Text = txtFSereName2.Text;
                ssPrint_Sheet1.Cells[17, 6].Text = txtFEtc2.Text;
                ssPrint_Sheet1.Cells[18, 3].Text = txtFName3.Text;
                ssPrint_Sheet1.Cells[18, 4].Text = txtFSereName3.Text;
                ssPrint_Sheet1.Cells[18, 6].Text = txtFEtc3.Text;
                ssPrint_Sheet1.Cells[22, 4].Text = txtHealth.Text;
                ssPrint_Sheet1.Cells[22, 9].Text = txtDis.Text;
                ssPrint_Sheet1.Cells[23, 4].Text = txtExplainYN.Text;
                ssPrint_Sheet1.Cells[23, 9].Text = txtBelieveYN.Text;
                ssPrint_Sheet1.Cells[24, 5].Text = txtParentYN.Text;
                ssPrint_Sheet1.Cells[25, 5].Text = txtUnconsciousYN.Text;
                ssPrint_Sheet1.Cells[30, 4].Text = txtBonName.Text;
                ssPrint_Sheet1.Cells[32, 4].Text = txtJName.Text;
                ssPrint_Sheet1.Cells[32, 9].Text = txtBirthDate.Text;
            }
        }

        private string readROWID(string ArgSeqno)
        {
            string rtnval = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ROWID FROM KOSMOS_ADM.WONMOK_LIST4";
                SQL = SQL + ComNum.VBLF + " WHERE SEQNO = " + ArgSeqno;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnval;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnval = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return rtnval;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnval;
            }
        }

        private void dataDelete(string ArgROWID)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ArgROWID != "")
            {
                SQL = "";
                SQL = " INSERT INTO KOSMOS_ADM.WONMOK_LIST4_HISTORY ";
                SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_ADM.WONMOK_LIST4";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + ArgROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                SQL = "";
                SQL = " DELETE FROM KOSMOS_ADM.WONMOK_LIST4";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + ArgROWID + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            }
        }

        private string createInsertSql(string ArgROWID, string ArgSeqno = "", string argColName = "", string argValue = "")
        {
            int i = 0;
            string cColumn = "";
            string cValue = "";
            string SQL = "";
            string rtnVal = "";

            cColumn = "";

            for (i = 1; i < VB.UBound(GstrColName); i++)
            {
                cColumn = cColumn + GstrColName[i];

                if (i != VB.UBound(GstrColName))
                {
                    cColumn = cColumn + ",";
                }

                if (i % 4 == 0)
                {
                    cColumn = cColumn + ComNum.VBLF;
                }
            }

            cValue = "";

            for (i = 1; i < VB.UBound(GstrColName); i++)
            {
                cValue = cValue + returnSqlValue(GstrColValue[i]);

                if (i != VB.UBound(GstrColValue))
                {
                    cValue = cValue + ",";
                }

                if (i % 4 == 0)
                {
                    cValue = cValue + ComNum.VBLF;
                }
            }

            SQL = " INSERT INTO KOSMOS_ADM.WONMOK_LIST4 (";

            if (ArgSeqno != "")
            {
                SQL = SQL + ComNum.VBLF + "SEQNO,";
            }

            if (argColName != "")
            {
                SQL = SQL + ComNum.VBLF + argColName + ",";
            }

            SQL = SQL + ComNum.VBLF + cColumn + ") VALUES (";

            if (ArgSeqno != "")
            {
                SQL = SQL + ComNum.VBLF + ArgSeqno + ",";
            }

            if (argValue != "")
            {
                SQL = SQL + ComNum.VBLF + argValue + ",";
            }

            SQL = SQL + ComNum.VBLF + cValue + ")";

            rtnVal = SQL;

            return rtnVal;
        }

        private string returnSqlValue(string argValue)
        {
            string rtnVal = "";

            if (VB.IsDate(argValue))
            {
                if (VB.IsNumeric(argValue))
                {
                    rtnVal = "'" + argValue + "'";
                }
                else if (argValue.IndexOf("년") != -1 || argValue.IndexOf("월") != -1)
                {
                    rtnVal = "'" + argValue + "'";
                }
                else if (argValue.IndexOf(":") != -1 || argValue.IndexOf("시") != -1)
                {
                    rtnVal = "'" + argValue + "'";
                }
                else
                {
                    rtnVal = VB.Val(argValue).ToString("YYYY-MM-DD");
                }
            }
            else
            {
                rtnVal = "'" + argValue + "'";
            }

            return rtnVal;
        }

        private void dataView(string ArgROWID)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM KOSMOS_ADM.WONMOK_LIST4";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + ArgROWID + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0]["PART01"].ToString().Trim();
                    cboSexsel.Text = dt.Rows[0]["PART03"].ToString().Trim();
                    txtSereName.Text = dt.Rows[0]["PART41"].ToString().Trim();
                    txtJumin1.Text = dt.Rows[0]["PART02"].ToString().Trim();
                    txtJumin2.Text = dt.Rows[0]["PART52"].ToString().Trim();
                    txtAddress.Text = dt.Rows[0]["PART04"].ToString().Trim();
                    txtBName1.Text = dt.Rows[0]["PART19"].ToString().Trim();
                    txtBRelation1.Text = dt.Rows[0]["PART20"].ToString().Trim();
                    txtBAddress1.Text = dt.Rows[0]["PART21"].ToString().Trim();
                    txtBContact1.Text = dt.Rows[0]["PART22"].ToString().Trim();
                    txtBName2.Text = dt.Rows[0]["PART23"].ToString().Trim();
                    txtBRelation2.Text = dt.Rows[0]["PART24"].ToString().Trim();
                    txtBAddress2.Text = dt.Rows[0]["PART25"].ToString().Trim();
                    txtBContact2.Text = dt.Rows[0]["PART26"].ToString().Trim();
                    txtBapDate.Text = dt.Rows[0]["PART05"].ToString().Trim();
                    txtTime.Text = dt.Rows[0]["PART51"].ToString().Trim();
                    txtBapSpace.Text = dt.Rows[0]["PART06"].ToString().Trim();
                    txtFRelation1.Text = dt.Rows[0]["PART28"].ToString().Trim();
                    txtFName1.Text = dt.Rows[0]["PART27"].ToString().Trim();
                    txtFSereName1.Text = dt.Rows[0]["PART29"].ToString().Trim();
                    txtFEtc1.Text = dt.Rows[0]["PART30"].ToString().Trim();
                    txtFRelation2.Text = dt.Rows[0]["PART32"].ToString().Trim();
                    txtFName2.Text = dt.Rows[0]["PART31"].ToString().Trim();
                    txtFSereName2.Text = dt.Rows[0]["PART33"].ToString().Trim();
                    txtFEtc2.Text = dt.Rows[0]["PART34"].ToString().Trim();
                    txtFRelation3.Text = dt.Rows[0]["PART36"].ToString().Trim();
                    txtFName3.Text = dt.Rows[0]["PART35"].ToString().Trim();
                    txtFSereName3.Text = dt.Rows[0]["PART37"].ToString().Trim();
                    txtFEtc3.Text = dt.Rows[0]["PART38"].ToString().Trim();
                    txtHealth.Text = dt.Rows[0]["PART13"].ToString().Trim();
                    txtDis.Text = dt.Rows[0]["PART14"].ToString().Trim();
                    txtExplainYN.Text = dt.Rows[0]["PART15"].ToString().Trim();
                    txtBelieveYN.Text = dt.Rows[0]["PART16"].ToString().Trim();
                    txtParentYN.Text = dt.Rows[0]["PART17"].ToString().Trim();
                    txtUnconsciousYN.Text = dt.Rows[0]["PART18"].ToString().Trim();
                    txtBonName.Text = dt.Rows[0]["PART07"].ToString().Trim();
                    txtSereName1.Text = dt.Rows[0]["PART08"].ToString().Trim();
                    txtDAddress.Text = dt.Rows[0]["PART10"].ToString().Trim();
                    txtDContact.Text = dt.Rows[0]["PART09"].ToString().Trim();
                    txtJName.Text = dt.Rows[0]["PART11"].ToString().Trim();
                    txtBirthDate.Text = dt.Rows[0]["PART12"].ToString().Trim();
                    txtBonSend.Text = dt.Rows[0]["PART40"].ToString().Trim();
                    txtEtc.Text = dt.Rows[0]["PART39"].ToString().Trim();
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

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Screen_Clear()
        {
            txtBapName.Text = "";
            txtName.Text = "";
            cboSexsel.Text = "";
            txtSereName.Text = "";
            txtJumin1.Text = "";
            txtJumin2.Text = "";
            txtAddress.Text = "";
            txtBName1.Text = "";
            txtBRelation1.Text = "";
            txtBAddress1.Text = "";
            txtBContact1.Text = "";
            txtBName2.Text = "";
            txtBRelation2.Text = "";
            txtBAddress2.Text = "";
            txtBContact2.Text = "";
            txtBapDate.Text = "";
            txtTime.Text = "";
            txtBapSpace.Text = "";
            txtFRelation1.Text = "";
            txtFName1.Text = "";
            txtFSereName1.Text = "";
            txtFEtc1.Text = "";
            txtFRelation2.Text = "";
            txtFName2.Text = "";
            txtFSereName2.Text = "";
            txtFEtc2.Text = "";
            txtFRelation3.Text = "";
            txtFName3.Text = "";
            txtFSereName3.Text = "";
            txtFEtc3.Text = "";
            txtHealth.Text = "";
            txtDis.Text = "";
            txtExplainYN.Text = "";
            txtBelieveYN.Text = "";
            txtParentYN.Text = "";
            txtUnconsciousYN.Text = "";
            txtBonName.Text = "";
            txtSereName1.Text = "";
            txtDAddress.Text = "";
            txtDContact.Text = "";
            txtJName.Text = "";
            txtBirthDate.Text = "";
            txtBonSend.Text = "";
            txtEtc.Text = "";
        }
    }
}
