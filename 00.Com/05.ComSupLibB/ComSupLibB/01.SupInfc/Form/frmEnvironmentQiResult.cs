using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using System.Drawing;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComSupLibB.SupInfc
{
    /// <summary>
    /// Class Name      : ComSupLibB
    /// File Name       : frmEnvironmentQiResult
    /// Description     : 
    /// Author          : 전상원
    /// Create Date     : 2018-09-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= " >> frmEnvironmentQiResult.cs 폼이름 재정의" />
    public partial class frmEnvironmentQiResult : Form
    {
        string GstrSysDate = "";

        public frmEnvironmentQiResult()
        {
            InitializeComponent();
        }

        private void frmEnvironmentQiResult_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            GstrSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            ssView_Sheet1.RowCount = 0;

            dtpFDate.Value = Convert.ToDateTime(GstrSysDate);
            dtpTDate.Value = Convert.ToDateTime(GstrSysDate);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string[] arrCombo = new string[5];

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT A.ENVIRONMENTSEQ, A.TOBUSE, A.TOSABUN, A.CODE1, A.CODE2, A.CODE3, A.CODE4,";
                SQL = SQL + ComNum.VBLF + "A.TODATE, A.TOTIME, A.SENDSABUN, A.SENDDATE, A.SENDTIME, ";
                SQL = SQL + ComNum.VBLF + "A.BACNAME, A.RESULTVALUE, A.STANDARD, A.UNIT, A.RESULTSABUN, ";
                SQL = SQL + ComNum.VBLF + "A.RESULTDATE, A.RESULTTIME, A.DEL, A.DELSABUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "QI_ENVIRONMENT A";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_ENVIRONMENT M1";
                SQL = SQL + ComNum.VBLF + "  ON A.CODE1 = M1.CODE";
                SQL = SQL + ComNum.VBLF + " AND M1.GRADE = '1'";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_ENVIRONMENT M2";
                SQL = SQL + ComNum.VBLF + "  ON A.CODE2 = M2.CODE";
                SQL = SQL + ComNum.VBLF + " AND M2.GRADE = '2'";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_ENVIRONMENT M3";
                SQL = SQL + ComNum.VBLF + "  ON A.CODE3 = M3.CODE";
                SQL = SQL + ComNum.VBLF + " AND M3.GRADE = '3'";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_ENVIRONMENT M4";
                SQL = SQL + ComNum.VBLF + "  ON A.CODE4 = M4.CODE";
                SQL = SQL + ComNum.VBLF + " AND M4.GRADE = '4'";
                SQL = SQL + ComNum.VBLF + "WHERE A.TODATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND A.TODATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "ORDER BY ENVIRONMENTSEQ, TODATE ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (rdoExam0.Checked == true)
                        {
                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ENVIRONMENTSEQ"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["TOBUSE"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["TOSABUN"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["CODE1"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["CODE2"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["CODE3"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["CODE4"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["TODATE"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["TOTIME"].ToString().Trim();
                        }
                        else if (rdoExam1.Checked == true)
                        {
                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ENVIRONMENTSEQ"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["TOBUSE"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["TOSABUN"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["CODE1"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["CODE2"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["CODE3"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["CODE4"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["TODATE"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["TOTIME"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["SENDSABUN"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["SENDDATE"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["SENDTIME"].ToString().Trim();
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ENVIRONMENTSEQ"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["TOBUSE"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["TOSABUN"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["CODE1"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["CODE2"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["CODE3"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["CODE4"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["TODATE"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["TOTIME"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["SENDSABUN"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["SENDDATE"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["SENDTIME"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 14].Text = dt.Rows[i]["BACNAME"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 15].Text = dt.Rows[i]["RESULTVALUE"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 16].Text = dt.Rows[i]["STANDARD"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 17].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 18].Text = dt.Rows[i]["RESULTSABUN"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 19].Text = dt.Rows[i]["RESULTDATE"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 20].Text = dt.Rows[i]["RESULTTIME"].ToString().Trim();
                        }

                        //ssView_Sheet1.Cells[i, 19].Text = dt.Rows[i]["DEL"].ToString().Trim();
                        //ssView_Sheet1.Cells[i, 20].Text = dt.Rows[i]["DELSABUN"].ToString().Trim();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            //if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    SQL = "";
                    SQL = "SELECT * ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "QI_ENVIRONMENT ";
                    SQL = SQL + ComNum.VBLF + "WHERE BACNAME = '" + ssView_Sheet1.Cells[i, 0].Text.Trim() + "'";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox(i + "행 조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_PMPA + "QI_ENVIRONMENT ";
                        SQL = SQL + ComNum.VBLF + " SET ENVIRONMENTSEQ = '" + ssView_Sheet1.Cells[i, 2].Text.Trim() + "',                          ";
                        SQL = SQL + ComNum.VBLF + "     TOBUSE = '" + ssView_Sheet1.Cells[i, 3].Text.Trim() + "',                                  ";
                        SQL = SQL + ComNum.VBLF + "     TOSABUN = '" + ssView_Sheet1.Cells[i, 4].Text.Trim() + "',                                 ";
                        SQL = SQL + ComNum.VBLF + "     CODE1 = '" + ssView_Sheet1.Cells[i, 5].Text.Trim() + "',                                   ";
                        SQL = SQL + ComNum.VBLF + "     CODE2 = '" + ssView_Sheet1.Cells[i, 6].Text.Trim() + "',                                   ";
                        SQL = SQL + ComNum.VBLF + "     CODE3 = '" + ssView_Sheet1.Cells[i, 7].Text.Trim() + "',                                   ";
                        SQL = SQL + ComNum.VBLF + "     CODE4 = '" + ssView_Sheet1.Cells[i, 8].Text.Trim() + "',                                   ";
                        SQL = SQL + ComNum.VBLF + "     TODATE = '" + ssView_Sheet1.Cells[i, 9].Text.Trim() + "',                                  ";
                        SQL = SQL + ComNum.VBLF + "     TOTIME = '" + ssView_Sheet1.Cells[i, 10].Text.Trim() + "',                                 ";
                        SQL = SQL + ComNum.VBLF + "     SENDSABUN = '" + ssView_Sheet1.Cells[i, 11].Text.Trim() + "',                              ";
                        SQL = SQL + ComNum.VBLF + "     SENDDATE = '" + ssView_Sheet1.Cells[i, 12].Text.Trim() + "',                               ";
                        SQL = SQL + ComNum.VBLF + "     SENDTIME = '" + ssView_Sheet1.Cells[i, 13].Text.Trim() + "'                                ";
                        SQL = SQL + ComNum.VBLF + "     BACNAME =  '" + ssView_Sheet1.Cells[i, 14].Text.Trim() + "',                               ";
                        SQL = SQL + ComNum.VBLF + "     RESULTVALUE = '" + ssView_Sheet1.Cells[i, 15].Text.Trim() + "',                            ";
                        SQL = SQL + ComNum.VBLF + "     STANDARD = '" + ssView_Sheet1.Cells[i, 16].Text.Trim() + "',                               ";
                        SQL = SQL + ComNum.VBLF + "     UNIT = '" + ssView_Sheet1.Cells[i, 17].Text.Trim() + "',                                   ";
                        SQL = SQL + ComNum.VBLF + "     RESULTSABUN = '" + ssView_Sheet1.Cells[i, 18].Text.Trim() + "',                            ";
                        SQL = SQL + ComNum.VBLF + "     RESULTDATE = '" + ssView_Sheet1.Cells[i, 19].Text.Trim() + "',                             ";
                        SQL = SQL + ComNum.VBLF + "     RESULTTIME = '" + ssView_Sheet1.Cells[i, 20].Text.Trim() + "'                              ";
                    }
                    else
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "QI_ENVIRONMENT ";
                        SQL = SQL + ComNum.VBLF + "(              ";
                        SQL = SQL + ComNum.VBLF + "   ENVIRONMENTSEQ, TOBUSE, TOSABUN, CODE1, CODE2, CODE3, CODE4, TODATE, TOTIME, SENDSABUN, SENDDATE, SENDTIME, ";
                        SQL = SQL + ComNum.VBLF + "   BACNAME, RESULTVALUE, STANDARD, UNIT, RESULTSABUN, RESULTDATE, RESULTTIME, DEL, DELSABUN ";
                        SQL = SQL + ComNum.VBLF + ")          ";
                        SQL = SQL + ComNum.VBLF + "   VALUES          ";
                        SQL = SQL + ComNum.VBLF + "(          ";
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 2].Text.Trim() + "',           "; //ENVIRONMENTSEQ
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 3].Text.Trim() + "',           "; //TOBUSE
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 4].Text.Trim() + "',           "; //TOSABUN
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 5].Text.Trim() + "',           "; //CODE1
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 6].Text.Trim() + "',           "; //CODE2
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 7].Text.Trim() + "',           "; //CODE3
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 8].Text.Trim() + "',           "; //CODE4
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 9].Text.Trim() + "',           "; //TODATE
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 10].Text.Trim() + "',          "; //TOTIME
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 11].Text.Trim() + "',          "; //SENDSABUN
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 12].Text.Trim() + "',          "; //SENDDATE
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 13].Text.Trim() + "',          "; //SENDTIME
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 14].Text.Trim() + "',          "; //BACNAME
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 15].Text.Trim() + "',          "; //RESULTVALUE
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 16].Text.Trim() + "',          "; //STANDARD
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 17].Text.Trim() + "',          "; //UNIT
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 18].Text.Trim() + "',          "; //RESULTSABUN
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 19].Text.Trim() + "',          "; //RESULTDATE
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 20].Text.Trim() + "',          "; //RESULTTIME
                        SQL = SQL + ComNum.VBLF + "'N',                                                        "; //DEL
                        SQL = SQL + ComNum.VBLF + " '" + ssView_Sheet1.Cells[i, 22].Text.Trim() + "'           "; //DELSABUN
                        SQL = SQL + ComNum.VBLF + ")            ";
                    }
                }

                dt.Dispose();
                dt = null;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                if (intRowAffected != 0)
                {
                    ComFunc.MsgBox("저장하였습니다.");
                    GetData();
                }

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoExam_CheckedChanged(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;

            GetData();
        }
    }
}
