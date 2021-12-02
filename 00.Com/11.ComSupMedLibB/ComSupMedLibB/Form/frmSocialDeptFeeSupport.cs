using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupMedLibB
{
    public partial class frmSocialDeptFeeSupport : Form
    {
        public frmSocialDeptFeeSupport()
        {
            InitializeComponent();
        }

        private void frmSocialDeptFeeSupport_Load(object sender, EventArgs e)
        {
            GetData();
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.Rows.Add(ssView_Sheet1.RowCount, 1);
            ssView_Sheet1.SetRowHeight(-1, 30);
            ssView.ActiveSheet.Cells[ssView_Sheet1.RowCount - 1, 16].Text = "Y";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strFlag = "";
            string strRowid = "";
            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    SqlErr = "";

                    strFlag = ssView.ActiveSheet.Cells[i, 0].Text;
                    strRowid = ssView.ActiveSheet.Cells[i, 15].Text;
                    if (strFlag == "True")
                    {
                        if (strRowid != "")
                        {

                            SQL = " DELETE KOSMOS_ADM.SOCIAL_DEPTFEE_SUPPORT                                                  \r\n";
                            SQL += "   WHERE ROWID = '" + strRowid + "'                                                     \r\n";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon, 250);

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
                    else
                    {
                        if (string.IsNullOrEmpty(ssView_Sheet1.Cells[i, 1].Text) == false && string.IsNullOrEmpty(ssView_Sheet1.Cells[i, 16].Text) == false)
                        {
                            SQL = "";
                            SQL = "INSERT INTO KOSMOS_ADM.SOCIAL_DEPTFEE_SUPPORT";
                            SQL = SQL + ComNum.VBLF + "(";
                            SQL = SQL + ComNum.VBLF + "     SEQNO, SARENO, LASTDAY, PTNAME, SEX, AGE, QUALIFY, DEPTCODE, DISNAME, CONNECTS, TOTALFEE, BIGO, REGDAY, SABUN	";
                            SQL = SQL + ComNum.VBLF + ")";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "(";
                            SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 1].Text + "',";
                            SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 2].Text + "',";
                            SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 3].Text + "',";
                            SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 4].Text + "',";
                            SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 5].Text + "',";
                            SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 6].Text + "',";
                            SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 7].Text + "',";
                            SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 8].Text + "',";
                            SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 9].Text + "',";
                            SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 10].Text + "',";
                            SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 11].Text + "',";
                            SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 12].Text + "',";
                            SQL = SQL + ComNum.VBLF + "     SYSDATE,";
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
                        }
                        else if (string.IsNullOrEmpty(strRowid) == false)
                        {
                            SQL = "";
                            SQL = "UPDATE KOSMOS_ADM.SOCIAL_DEPTFEE_SUPPORT SET";
                            SQL = SQL + ComNum.VBLF + "  SEQNO =   '" + ssView_Sheet1.Cells[i, 1].Text + "',";
                            SQL = SQL + ComNum.VBLF + "  SARENO =   '" + ssView_Sheet1.Cells[i, 2].Text + "',";
                            SQL = SQL + ComNum.VBLF + "  LASTDAY =   '" + ssView_Sheet1.Cells[i, 3].Text + "',";
                            SQL = SQL + ComNum.VBLF + "  PTNAME =   '" + ssView_Sheet1.Cells[i, 4].Text + "',";
                            SQL = SQL + ComNum.VBLF + "  SEX =   '" + ssView_Sheet1.Cells[i, 5].Text + "',";
                            SQL = SQL + ComNum.VBLF + "  AGE =   '" + ssView_Sheet1.Cells[i, 6].Text + "',";
                            SQL = SQL + ComNum.VBLF + "  QUALIFY =   '" + ssView_Sheet1.Cells[i, 7].Text + "',";
                            SQL = SQL + ComNum.VBLF + "  DEPTCODE =   '" + ssView_Sheet1.Cells[i, 8].Text + "',";
                            SQL = SQL + ComNum.VBLF + "  DISNAME =   '" + ssView_Sheet1.Cells[i, 9].Text + "',";
                            SQL = SQL + ComNum.VBLF + "  CONNECTS =  '" + ssView_Sheet1.Cells[i, 10].Text + "',";
                            SQL = SQL + ComNum.VBLF + "  TOTALFEE =   '" + ssView_Sheet1.Cells[i, 11].Text + "',";
                            SQL = SQL + ComNum.VBLF + "  BIGO =   '" + ssView_Sheet1.Cells[i, 12].Text + "',";
                            SQL = SQL + ComNum.VBLF + "  REGDAY =   SYSDATE,";
                            SQL = SQL + ComNum.VBLF + "  SABUN =   '" + clsType.User.Sabun + "'";
                            SQL = SQL + ComNum.VBLF + "  WHERE ROWID =   '" + strRowid + "'";
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

            GetData();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            strTitle = "";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
            CS = null;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            ssView_Sheet1.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT SEQNO, SARENO, LASTDAY, PTNAME, SEX, AGE, QUALIFY, DEPTCODE, DISNAME, CONNECTS, TOTALFEE, BIGO, REGDAY, SABUN, ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.SOCIAL_DEPTFEE_SUPPORT";
                SQL = SQL + ComNum.VBLF + " WHERE (LASTDAY >= '" + dtpSdate.Value.ToString("yyyy-MM-dd") + "'";
                SQL = SQL + ComNum.VBLF + "      AND LASTDAY <=  TO_DATE('" + dtpEdate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI'))";
                SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO";

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
                    ssView_Sheet1.SetRowHeight(-1, 30);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SARENO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["LASTDAY"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PTNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["QUALIFY"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DISNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["CONNECTS"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["TOTALFEE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["BIGO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["REGDAY"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 14].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 15].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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
    }
}
