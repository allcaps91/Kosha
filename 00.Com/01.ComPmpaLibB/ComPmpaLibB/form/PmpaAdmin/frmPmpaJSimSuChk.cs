using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name : frmPmpaJSimSuChk
    /// File Name : frmPmpaJSimSuChk.cs
    /// Title or Description : 재원심사시 심사자별 특정수가 설정
    /// Author : 김민철
    /// Create Date : 2017-06-15
    /// Update History : 
    /// <seealso cref="d:\psmh\IPD\ipdSim2\Frm재원심사수가설정.frm"/>
    /// </summary>
    public partial class frmPmpaJSimSuChk : Form
    {
        public frmPmpaJSimSuChk()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPmpaJSimSuChk_Load(object sender, EventArgs e)
        {
            Read_JSim_Sabun(clsType.User.IdNumber);
        }

        private void Read_JSim_Sabun(string strSabun)
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
                SQL += ComNum.VBLF + " SELECT SuCode,SuName,TO_CHAR(JDATE,'YYYY-MM-DD') JDATE,ROWID,USE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_JSIM_SUCHK ";
                SQL += ComNum.VBLF + "  WHERE SABUN = '" + strSabun + "' ";
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
                    SS1_Sheet1.Cells[i, 0].Value = false;
                    SS1_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["SUCODE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["SUNAME"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["JDATE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 6].Value = Convert.ToBoolean(VB.IIf(Dt.Rows[i]["USE"].ToString().Trim() == "Y", true, false));
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

        private void SS1_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strSuCode = "";

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (e.Row < 0 || e.Column < 0) { return; }

            SS1_Sheet1.Cells[e.Row, 5].Text = "Y";
            strSuCode = SS1_Sheet1.Cells[e.Row, 1].Text.ToUpper().Trim();
            SS1_Sheet1.Cells[e.Row, 1].Text = strSuCode;

            try
            {
                if (SS1_Sheet1.Cells[e.Row, 2].Text.Trim() == "")
                {


                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SUNAMEK ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                    SQL += ComNum.VBLF + "    AND SUNEXT = '" + strSuCode + "' ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        SS1_Sheet1.Cells[e.Row, 2].Text = Dt.Rows[0]["SUNAMEK"].ToString().Trim();
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
            string strCode = "";
            string strName = "";
            string strJDate = "";
            string strRowid = "";
            string strChange = "";
            string strUse = "";
            
            string SqlErr = "";            

            for (i = 0; i < SS1_Sheet1.Rows.Count; i++)
            {
                bDel = Convert.ToBoolean(SS1_Sheet1.Cells[i, 0].Value);
                strCode = SS1_Sheet1.Cells[i, 1].Text;
                strName = SS1_Sheet1.Cells[i, 2].Text;
                strJDate = SS1_Sheet1.Cells[i, 3].Text;
                strRowid = SS1_Sheet1.Cells[i, 4].Text;
                strChange = SS1_Sheet1.Cells[i, 5].Text;
                strUse = (string)VB.IIf(Convert.ToBoolean(SS1_Sheet1.Cells[i, 6].Value), "Y", "N");

                if (bDel)
                {
                    if (strRowid != "")
                    {
                        SqlErr = Delete_Data(strRowid);
                        if (SqlErr != "") { return; }
                    }
                }
                else if (strChange == "Y")
                {
                    if (strRowid == "")
                    {
                        SqlErr = Insert_Data(strCode, strName, strUse);
                        if (SqlErr != "") { return; }
                    }
                    else
                    {
                        SqlErr = Update_Data(strCode, strName, strJDate, strUse, strRowid);
                        if (SqlErr != "") { return; }
                    }
                }
            }

            Read_JSim_Sabun(clsPublic.GstrJobSabun);
        }

        private string Insert_Data(string strCode, string strName, string strUse)
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
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_JSIM_SUCHK (SuCode,SuName,JDate,Sabun,EntDate,Use ) ";
                SQL += ComNum.VBLF + " VALUES ('" + strCode + "', '" + strName + "', TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + " '" + clsType.User.IdNumber + "', SYSDATE, '" + strUse + "' ) ";
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

        private string Update_Data(string strCode, string strName, string strJDate, string strUse, string strRowid)
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
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ETC_JSIM_SUCHK ";
                SQL += ComNum.VBLF + "    SET SUCODE = '" + strCode + "', ";
                SQL += ComNum.VBLF + "        SUNAME = '" + strName + "', ";
                SQL += ComNum.VBLF + "        JDATE = TO_DATE('" + strJDate + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "        SABUN = '" + clsType.User.IdNumber + "', ";
                SQL += ComNum.VBLF + "        ENTDATE = SYSDATE, ";
                SQL += ComNum.VBLF + "        USE = '" + strUse + "' ";
                SQL += ComNum.VBLF + "  WHERE ROWID = '" + strRowid + "' ";
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
                SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "ETC_JSIM_SUCHK ";
                SQL += ComNum.VBLF + "  WHERE ROWID = '" + strRowid + "' ";
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
    }
}
