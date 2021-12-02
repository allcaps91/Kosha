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
using ComLibB;

namespace ComNurLibB
{
    public partial class frmNrCodeBedCount : Form
    {
        public frmNrCodeBedCount()
        {
            InitializeComponent();
        }

        private void frmNrCodeBedCount_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssBedCount_Sheet1.Columns[0].Visible = false;
            ssBedCount_Sheet1.Columns[4].Visible = false;
            ssBedCount_Sheet1.Columns[5].Visible = false;
        }
        
        private void SCREEN_CLEAR()
        {
            ssBedCount_Sheet1.Cells[0, 1, ssBedCount_Sheet1.RowCount - 1, 2].Text = "";
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            SCREEN_CLEAR();
            btnSearch.Focus();
        }
        void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    CODE,NAME,JIK,Rowid ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN ='7'";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODE";
                
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    ssBedCount_Sheet1.RowCount = dt.Rows.Count;
                    ssBedCount_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {                                                
                        ssBedCount_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ssBedCount_Sheet1.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssBedCount_Sheet1.Cells[i, 3].Text = dt.Rows[i]["JIK"].ToString().Trim();
                        ssBedCount_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Rowid"].ToString().Trim();
                    }                    
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            btnSave.Enabled = true;
        }
        
        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            SaveData();
            btnSearch.PerformClick();
        }

        string SetGubun(string strGubun)
        {
            string str = strGubun.Trim();

            if (str == "7")
                return str;

            return str;                
        }

        private bool SaveData()
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int intRowAffected = 0;

            string strCode = "";
            string strRowid = "";
            string strName = "";
            string strJik = "";            
            string strChange = "";
            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {                
                for (i = 0; i < ssBedCount_Sheet1.RowCount; i++)
                {
                    strCode = ssBedCount_Sheet1.Cells[i, 1].Text;
                    strName = ssBedCount_Sheet1.Cells[i, 2].Text;
                    strJik = ssBedCount_Sheet1.Cells[i, 3].Text;
                    strChange = ssBedCount_Sheet1.Cells[i, 4].Text;
                    strRowid = ssBedCount_Sheet1.Cells[i, 5].Text;

                    if (strCode != "")
                    {
                        SQL = " SELECT CODE FROM NUR_CODE WHERE GUBUN ='7' ";
                        SQL = SQL + " AND CODE = '" + strCode + "'";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return rtVal;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE ";
                            SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_CODE ";
                            SQL = SQL + ComNum.VBLF + " SET Code = '" + strCode + "', NAME = '" + strName + "', ";
                            SQL = SQL + ComNum.VBLF + " Jik = '" + strJik + "'";
                            SQL = SQL + ComNum.VBLF + " WHERE Rowid = '" + strRowid + "' ";
                            SQL = SQL + ComNum.VBLF + " AND GUBUN = '" + SetGubun("7") + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }
                        }
                        else
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "INSERT INTO ";
                            SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_CODE ";
                            SQL = SQL + ComNum.VBLF + " (GUBUN, CODE, NAME, JIK) ";
                            SQL = SQL + ComNum.VBLF + " VALUES ('" + SetGubun("7") + "','" + strCode + "', ";
                            SQL = SQL + ComNum.VBLF + "  '" + strName + "', '" + strJik + "' )";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }
                        }
                    }                    
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }            
        }        
    }
}

    

