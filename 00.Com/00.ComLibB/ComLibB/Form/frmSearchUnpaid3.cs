using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmSearchUnpaid3 : Form
    {
        int i = 0;
        int j = 0;
        string SQL = string.Empty;
        string SqlErr = ""; //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수
        DataTable dt = null;

        public frmSearchUnpaid3()
        {
            InitializeComponent();
        }

        private void frmSearchUnpaid3_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            Search_Suga();

            ssView_Sheet1.Rows.Count = ssView_Sheet1.Rows.Count + 300;
            ssView_Sheet1.Columns[2].Visible = false;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT); 
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save_Suga();
        }

        private void Search_Suga()
        {
            ssView_Sheet1.Rows.Count = 0;

            SQL = "";
            SQL = SQL + " SELECT CODE, ROWID FROM ADMIN.BAS_BCODE " + ComNum.VBLF;
            SQL = SQL + " WHERE 1=1 " + ComNum.VBLF;
            SQL = SQL + " AND GUBUN = 'ETC_비급여고지수가목록' " + ComNum.VBLF;
            SQL = SQL + " ORDER BY CODE " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장 
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ssView_Sheet1.Rows.Count = dt.Rows.Count;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                ssView_Sheet1.Rows.Count = ssView_Sheet1.Rows.Count + 300;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            }

            dt.Dispose();
            dt = null;
        }

        private void Save_Suga()
        {
            string strChk = "";
            string strSuCode = "";
            string strROWID = "";

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.Rows.Count; i++)
                {
                    strChk = ssView_Sheet1.Cells[i, 0].Text.Trim();
                    strSuCode = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i, 2].Text.Trim();

                    for (j = i + 1; j < ssView_Sheet1.Rows.Count; j++)
                    {
                        if (strSuCode.CompareTo(ssView_Sheet1.Cells[j, 1].Text.Trim()) == 0 && strSuCode != "")
                        {
                            ssView_Sheet1.SetActiveCell(j, 1);
                            ssView_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 193, 158);
                            ssView_Sheet1.Rows[j].BackColor = Color.FromArgb(255, 193, 158);
                            ComFunc.MsgBox("'" + strSuCode + "' 가 " + (i + 1) + ", " + (j + 1) + "번째" + " 중복 됩니다.");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }

                    if(strChk == "True")
                    {
                        if(strROWID != "")
                        {
                            SQL = "DELETE ADMIN.BAS_BCODE ";
                            SQL = SQL + " WHERE ROWID = '" + strROWID + "'" + ComNum.VBLF;
                            SQL = SQL + " AND GUBUN = 'ETC_비급여고지수가목록'" + ComNum.VBLF;

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
    
                    if(strSuCode != "" && strROWID == "")
                    {
                        SQL = "INSERT INTO ADMIN.BAS_BCODE(GUBUN,CODE,JDATE,ENTSABUN,ENTDATE) VALUES ";
                        SQL = SQL + "('ETC_비급여고지수가목록','" + ssView_Sheet1.Cells[i, 1].Text.Trim() + "',SYSDATE,'" + clsType.User.Sabun + "',SYSDATE)" + ComNum.VBLF;

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
                
                clsDB.setCommitTran(clsDB.DbCon);

                Search_Suga();

                Cursor.Current = Cursors.Default;

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(e.Row > 0) { return; }
            if(e.ColumnHeader ==true)
            {
                if(e.Column == 0)
                {
                    for (i = 0; i < ssView_Sheet1.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Value = true;
                    }
                }
            }
        }
    }
}
