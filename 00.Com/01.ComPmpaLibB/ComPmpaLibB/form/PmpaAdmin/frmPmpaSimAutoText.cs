using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComPmpaLibB
{

    /// <summary>
    /// Class Name : frmPmpaSimAutoText
    /// File Name : frmPmpaSimAutoText.cs
    /// Title or Description : 재원심사시 심사자별 심사상용구 설정창
    /// Author : 김민철
    /// Create Date : 2017-06-14
    /// Update History : 
    /// <seealso cref="d:\psmh\IPD\ipdSim2\Frm심사용상용구.frm"/>
    /// </summary>

    public partial class frmPmpaSimAutoText : Form
    {
        clsSpread spd = new clsSpread();

        public frmPmpaSimAutoText()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPmpaSimAutoText_Load(object sender, EventArgs e)
        {
            cboGbn.Items.Clear();
            cboGbn.Items.Add("1. 상병 상용구");
            cboGbn.Items.Add("2. 참고사항 상용구");
            cboGbn.Items.Add("3. 재원심사 상용구");
            cboGbn.SelectedIndex = 0;

            Spd_Header_Set();
            Screen_Display();
        }

        private void Spd_Header_Set()
        {
            if (VB.Left(cboGbn.Text, 1) == "1")
            {
                spd.setColStyle(SS1, -1, 1, clsSpread.enmSpdType.View);

                SS1_Sheet1.Cells[0, 1].Text = "상병코드";
                SS1_Sheet1.Cells[0, 2].Text = "상병명칭";
            }
            else if (VB.Left(cboGbn.Text, 1) == "2")
            {
                spd.setColStyle(SS1, -1, 1, clsSpread.enmSpdType.Hide);
                SS1_Sheet1.Cells[0, 1].Text = "";
                SS1_Sheet1.Cells[0, 2].Text = "참고사항";
            }
            else if (VB.Left(cboGbn.Text, 1) == "3")
            {
                spd.setColStyle(SS1, -1, 1, clsSpread.enmSpdType.Hide);
                SS1_Sheet1.Cells[0, 1].Text = "";
                SS1_Sheet1.Cells[0, 2].Text = "재원심사 참고사항";
            }
        }

        private void Screen_Display()
        {
            int i = 0;

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, false);
            SS1_Sheet1.Rows.Count = 0;
            
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SABUN , Code, Memo, TO_CHAR(ENTDATE,'YYYY-MM-DD') ENTDATE, NO, Gubun, ROWID ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_SET_REMARK ";
                SQL += ComNum.VBLF + "  WHERE GUBUN = '" + VB.Left(cboGbn.Text, 1) + "' ";
                SQL += ComNum.VBLF + "    AND SABUN = '" + clsType.User.IdNumber + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                SS1_Sheet1.Rows.Count = Dt.Rows.Count + 10;

                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    SS1_Sheet1.Cells[i, 0].Text = "";
                    SS1_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["CODE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["MEMO"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["ENTDATE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 5].Text = "";
                }
                
                Dt.Dispose();
                Dt = null;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void cboGbn_SelectedIndexChanged(object sender, EventArgs e)
        {
            Spd_Header_Set();
            Screen_Display();
        }

        private void SS1_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strSuCode = "";

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (e.Row < 0 || e.Column < 0) { return; }

            SS1_Sheet1.Cells[e.Row, 5].Text = "Y";
            strSuCode = SS1_Sheet1.Cells[e.Row, 1].Text.ToUpper().Trim();
            
            try
            {
                if (SS1_Sheet1.Cells[e.Row, 2].Text.Trim() == "")
                {


                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ILLNAMEK ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS ";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                    SQL += ComNum.VBLF + "    AND ILLCODE = '" + strSuCode + "' ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        SS1_Sheet1.Cells[e.Row, 2].Text = Dt.Rows[0]["ILLNAMEK"].ToString().Trim();
                    }

                    Dt.Dispose();
                    Dt = null;
                }

                if (SS1_Sheet1.Cells[e.Row, 3].Text.Trim() == "")
                {
                    SS1_Sheet1.Cells[e.Row, 3].Text = clsPublic.GstrSysDate;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;
            bool bDel = false;
            string strGubun = "";
            string strCode = "";
            string strName = "";
            string strJDate = "";
            string strRowid = "";
            string strChange = "";

            //string SQL = "";
            string SqlErr = "";
            
            strGubun = VB.Left(cboGbn.Text, 1);
            
            for (i = 0; i < SS1_Sheet1.Rows.Count; i++)
            {
                bDel = Convert.ToBoolean(SS1_Sheet1.Cells[i, 0].Value);
                strCode = SS1_Sheet1.Cells[i, 1].Text;
                strName = SS1_Sheet1.Cells[i, 2].Text;
                strJDate = SS1_Sheet1.Cells[i, 3].Text;
                strRowid= SS1_Sheet1.Cells[i, 4].Text;
                strChange = SS1_Sheet1.Cells[i, 5].Text;

                if (bDel)
                {
                    if (strRowid != "")
                    {
                        SqlErr = Delete_Data(strRowid);
                        if (SqlErr != ""){ return; }
                    }
                }
                else if (strChange == "Y")
                {
                    if (strRowid == "")
                    {
                        SqlErr = Insert_Data(strCode, strName, strGubun);
                        if (SqlErr != ""){ return; }
                    }
                    else
                    {
                        SqlErr = Update_Data(strCode, strName, strRowid);
                        if (SqlErr != ""){ return; }
                    }
                }
            }
                
            Screen_Display();
            
        }

        private string Insert_Data(string strCode, string strName, string strGbn)
        {
            string rtnVal = "";

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int intRowAffected = 0; //변경된 Row 받는 변수

             

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_SET_REMARK (SABUN,CODE,MEMO,ENTDATE,NO,GUBUN ) ";
                SQL += ComNum.VBLF + " VALUES (" + clsType.User.IdNumber + ", '" + strCode + "', '" + strName + "', ";
                SQL += ComNum.VBLF + " SYSDATE, 1, '" + VB.Left(cboGbn.Text, 1) + "' ) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    rtnVal = SqlErr;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                return rtnVal;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                rtnVal = ex.Message;
                return rtnVal;
            }
        }

        private string Update_Data(string strCode, string strName, string strRowid)
        {
            string rtnVal = "";

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int intRowAffected = 0; //변경된 Row 받는 변수

             

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ETC_SET_REMARK ";
                SQL += ComNum.VBLF + "    SET CODE = '" + strCode + "', ";
                SQL += ComNum.VBLF + "        MEMO = '" + strName + "', ";
                SQL += ComNum.VBLF + "        ENTDATE = SYSDATE ";
                SQL += ComNum.VBLF + "  WHERE ROWID = '" + strRowid + "' ";
                SQL += ComNum.VBLF + "    AND SABUN = '" + clsType.User.IdNumber + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    rtnVal = SqlErr;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                return rtnVal;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                rtnVal = ex.Message;
                return rtnVal;
            }

        }

        private string Delete_Data(string strRowid)
        {
            string rtnVal = "";

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int intRowAffected = 0; //변경된 Row 받는 변수

             

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "ETC_SET_REMARK ";
                SQL += ComNum.VBLF + "  WHERE ROWID = '" + strRowid + "' ";
                SQL += ComNum.VBLF + "    AND SABUN = '" + clsType.User.IdNumber + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    rtnVal = SqlErr;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                return rtnVal;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                rtnVal = ex.Message;
                return rtnVal;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Spd_Header_Set();
            Screen_Display();
        }
    }
}
