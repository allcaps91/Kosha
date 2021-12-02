using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : FrmOcsMsg.cs
    /// Description     : 심사계 전달사항
    /// Author          : 최익준
    /// Create Date     : 2017-11-02
    /// Update History  : 
    /// <seealso>
    /// PSMH\basic\busuga\BuSuga52.frm
    /// </seealso>
    /// <history>  
    /// 
    /// </history>
    /// </summary>
    public partial class FrmOcsMsg : Form
    {
        string GstrSanCode = "";

        public FrmOcsMsg()
        {
            InitializeComponent();
        }

        public FrmOcsMsg(string strSanCode)
        {
            InitializeComponent();
            GstrSanCode = strSanCode;
        }

        private void SCREEN_CLEAR()
        {
            txtSuCode.Text = "";
            txtSuName.Text = "";
            dtpSdate.Text = "";
            chkEnd.Checked = false;
            txtR.Text = "";
            
            chkInsurance.Checked = false;
            chkJabo.Checked = false;
            chkProtect.Checked = false;
            chkSanje.Checked = false;
        }

        private void FrmOcsMsg_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SCREEN_CLEAR();
            SearchData();

            if (GstrSanCode != "")
            {
                txtSuCode.Text = GstrSanCode;
                DataSearch();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSuCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataSearch();
            }
        }

        void DataSearch()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strSuCode = "";

            txtSuCode.Text = txtSuCode.Text.ToUpper();
            strSuCode = txtSuCode.Text;


            try
            {
                SQL = "";
                SQL = "SELECT SUNAMEK, UNIT FROM " + ComNum.DB_PMPA + "BAS_SUN";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT ='" + strSuCode + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count <= 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("입력하려는 코드는 수가에 등록되지않은 코드입니다. 우선 수가에 등록해주세요", "확인");
                    txtSuCode.Focus();
                    return;
                }
                else
                {
                    txtSuName.Text = dt.Rows[0]["SUNAMEK"].ToString();
                }

                dt.Dispose();
                dt = null;

                SQL = "SELECT REMARK, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DDATE,'YYYY-MM-DD') DDATE, B1, B2,B3, B4";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "JSIM_SIMSAMSG ";
                SQL = SQL + ComNum.VBLF + "  WHERE SUCODE = '" + strSuCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtR.Text = dt.Rows[0]["REMARK"].ToString();
                    dtpSdate.Text = dt.Rows[0]["SDATE"].ToString();

                    if (dt.Rows[0]["B1"].ToString() == "Y")
                    {
                        chkInsurance.Checked = true;
                    }
                    else
                    {
                        chkInsurance.Checked = false;
                    }

                    if (dt.Rows[0]["B2"].ToString() == "Y")
                    {
                        chkProtect.Checked = true;
                    }
                    else
                    {
                        chkProtect.Checked = false;
                    }

                    if (dt.Rows[0]["B3"].ToString() == "Y")
                    {
                        chkSanje.Checked = true;
                    }
                    else
                    {
                        chkSanje.Checked = false;
                    }

                    if (dt.Rows[0]["B4"].ToString() == "Y")
                    {
                        chkJabo.Checked = true;
                    }
                    else
                    {
                        chkJabo.Checked = false;
                    }

                    if (dt.Rows[0]["DDATE"].ToString() == "")
                    {
                        chkEnd.Checked = false;
                        chkEnd.Text = "사용가능";
                    }
                    else
                    {
                        chkEnd.Checked = true;
                        chkEnd.Text = "사용중지(" + dt.Rows[0]["DDATE"].ToString() + ")";
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

        private void chkEnd_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnd.Checked == false)
            {
                chkEnd.Text = "사용가능";
            }
            else
            {
                chkEnd.Text = "사용중지";
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            SSPRT_CLEAR();

            ssPrint_Sheet1.Cells[3, 3].Text = txtSuCode.Text.ToString().Trim();
            ssPrint_Sheet1.Cells[3, 5].Text = txtSuName.Text.ToString().Trim();

            ssPrint_Sheet1.Cells[4, 3].Text = dtpSdate.Text.Trim();

            if (chkInsurance.Checked == true)
            {
                ssPrint_Sheet1.Cells[4, 5].Value = true;
            }

            if (chkProtect.Checked == true)
            {
                ssPrint_Sheet1.Cells[4, 6].Value = true;
            }

            if (chkSanje.Checked == true)
            {
                ssPrint_Sheet1.Cells[4, 7].Value = true;
            }

            if (chkJabo.Checked == true)
            {
                ssPrint_Sheet1.Cells[4, 8].Value = true;
            }

            if (chkEnd.Checked == true)
            {
                ssPrint_Sheet1.Cells[4, 9].Value = true;
            }

            ssPrint_Sheet1.Cells[7, 2].Text = txtR.Text;

            ssPrint_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPrint.PrintSheet(0);
        }

        private void SSPRT_CLEAR()
        {
            ssPrint_Sheet1.Cells[3, 3].Text = "";
            ssPrint_Sheet1.Cells[3, 5].Text = "";

            ssPrint_Sheet1.Cells[4, 3].Text = "";
            ssPrint_Sheet1.Cells[4, 5].Value = false;
            ssPrint_Sheet1.Cells[4, 6].Value = false;
            ssPrint_Sheet1.Cells[4, 7].Value = false;
            ssPrint_Sheet1.Cells[4, 8].Value = false;
            ssPrint_Sheet1.Cells[4, 9].Value = false;

            ssPrint_Sheet1.Cells[7, 2].Text = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        void SearchData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT SUCODE,REMARK, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DDATE,'YYYY-MM-DD') DDATE, B1, B2, B3, B4 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "JSIM_SIMSAMSG ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SUCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssView_Sheet1.RowCount = 0;
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SUCODE"].ToString().Trim();

                        if (dt.Rows[i]["DDATE"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Rows[i].ForeColor = System.Drawing.Color.FromArgb(255, 0, 0);
                        }
                    }
                }
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                dt.Dispose();
                dt = null;

                return;
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
                Cursor.Current = Cursors.Default;

                return;
            }
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if(chkInsurance.Checked == false && chkProtect.Checked == false && chkSanje.Checked == false && chkJabo.Checked == false)
            {
                ComFunc.MsgBox("<해당되는 자격을 모두 Check 하십시요>","체크확인");
                return;
            }


            string strROWID = "";

            string strB1 = "";
            string strB2 = "";
            string strB3 = "";
            string strB4 = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            if (txtSuCode.Text == "")
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "JSIM_SIMSAMSG ";
                SQL = SQL + ComNum.VBLF + " WHERE SUCODE = '" + txtSuCode.Text + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (chkInsurance.Checked == true)       //보험
                {
                    strB1 = "Y";
                }
                else
                {
                    strB1 = "N";
                }

                if (chkProtect.Checked == true)     //보호
                {
                    strB2 = "Y";
                }
                else
                {
                    strB2 = "N";
                }

                if (chkSanje.Checked == true)       //산재
                {
                    strB3 = "Y";
                }
                else
                {
                    strB3 = "N";
                }

                if (chkJabo.Checked == true)        //자보
                {
                    strB4 = "Y";
                }
                else
                {
                    strB4 = "N";
                }

                if (intRowAffected > 0)
                {
                    dt.Rows[0]["ROWID"].ToString();
                }

                if (strROWID == "")
                {
                    SQL = "";
                    SQL = " INSERT INTO KOSMOS_PMPA.JSIM_SIMSAMSG ( SUCODE, SDATE, DDATE,REMARK, B1, B2, B3, B4 )";
                    SQL = SQL + ComNum.VBLF + " VALUES('" + txtSuCode.Text + "', TO_DATE('" + dtpSdate.Text + "','YYYY-MM-DD'), ";

                    if (chkEnd.Checked == true)
                    {
                        SQL = SQL + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                    }
                    else
                    {
                        SQL = SQL + " '', ";
                    }

                    SQL = SQL + ComNum.VBLF + " '" + txtR.Text + "' ,'" + strB1 + "', '" + strB2 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strB3 + "','" + strB4 + "')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;

                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장하였습니다.");
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "JSIM_SIMSAMSG  SET ";
                    SQL = SQL + ComNum.VBLF + " SDATE = TO_DATE('" + dtpSdate.Text + "','YYYY-MM-DD') ,";

                    if (chkEnd.Checked == true)
                    {
                        SQL = SQL + " DDATE = SYSDATE, ";
                    }
                    else
                    {
                        SQL = SQL + " DDATE = '', ";
                    }

                    SQL = SQL + ComNum.VBLF + " REMARK = '" + txtR.Text + "', ";
                    SQL = SQL + ComNum.VBLF + " B1 = '" + strB1 + "', ";
                    SQL = SQL + ComNum.VBLF + " B2 = '" + strB2 + "', ";
                    SQL = SQL + ComNum.VBLF + " B3 = '" + strB3 + "', ";
                    SQL = SQL + ComNum.VBLF + " B4 = '" + strB4 + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (SqlErr != "-1")
                    {
                        ComFunc.MsgBox("등록 완료", "확인");
                        clsDB.setCommitTran(clsDB.DbCon);
                    }
                    else
                    {
                        ComFunc.MsgBox("등록 오류", "확인");
                        clsDB.setRollbackTran(clsDB.DbCon);
                    }
                }

                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                SearchData();

                return;
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (txtSuCode.Text == "")
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "JSIM_SIMSAMSG WHERE SUCODE = '" + txtSuCode.Text + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);         //해당 트랜젝션을 바로 실행하기
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (SqlErr != "-1")
                {
                    ComFunc.MsgBox("삭제 완료", "확인");
                    clsDB.setCommitTran(clsDB.DbCon);
                }
                else
                {
                    ComFunc.MsgBox("삭제 오류", "확인");
                    clsDB.setRollbackTran(clsDB.DbCon);
                }

                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                SearchData();

                return;
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

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;

            txtSuCode.Text = ssView_Sheet1.Cells[e.Row, 0].Text;
            DataSearch();
        }
    }
}
