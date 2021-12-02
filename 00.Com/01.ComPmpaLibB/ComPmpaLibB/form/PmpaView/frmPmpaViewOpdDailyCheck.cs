using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewOpdDailyCheck.cs
    /// Description     : 외래 일자별 발생 점검
    /// Author          : 박창욱
    /// Create Date     : 2017-08-31
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs96.frm(FrmOpdBalDailCheck.frm) >> frmPmpaViewOpdDailyCheck.cs 폼이름 재정의" />	
    public partial class frmPmpaViewOpdDailyCheck : Form
    {
        public frmPmpaViewOpdDailyCheck()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            Cursor.Current = Cursors.WaitCursor;

            //Print Head
            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fs2";
            strHead1 = "/f1" + VB.Space(15);
            strHead1 = strHead1 + "외래 발생주의 오류 점검";
            strHead2 = "/f2" + "작업일: " + dtpDate.Value.ToString("yyyy-MM-dd");
            strHead2 = strHead2 + VB.Space(30) + "인쇄일자: " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");

            //Print Body
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 20;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);

            Cursor.Current = Cursors.Default;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            int i = 0;
            int nRead = 0;
            int nRow = 0;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;
            DataTable dt1 = null;

            string strPano = "";
            string strDate1 = "";

            double nRBAmt7 = 0;

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            //단수 오류CHECK
            try
            {
                SQL = "";
                SQL = "SELECT COUNT(*) AS CNT FROM ALL_VIEWS";
                SQL = SQL + ComNum.VBLF + "     WHERE VIEW_NAME = 'VIEW_OPD_DANSU_CHECK'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }
                else
                {
                    if (VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) > 0)
                    {
                        SQL = "";
                        SQL = "DROP VIEW VIEW_OPD_DANSU_CHECK";

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
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "CREATE VIEW VIEW_OPD_DANSU_CHECK AS";
                SQL = SQL + ComNum.VBLF + "     SELECT actdate, Pano, Bi,";
                SQL = SQL + ComNum.VBLF + "         SUM(DECODE(Bun,'92',0,'96',0,'98',0,'99',0,Amt1+Amt2)) AS TAmt,";
                SQL = SQL + ComNum.VBLF + "         SUM(DECODE(Bun,'92',Amt1+Amt2,0)) AS Bun92,";
                SQL = SQL + ComNum.VBLF + "         SUM(DECODE(Bun,'96',Amt1+Amt2,0)) AS Bun96,";
                SQL = SQL + ComNum.VBLF + "         SUM(DECODE(Bun,'98',Amt1+Amt2,0)) AS Bun98,";
                SQL = SQL + ComNum.VBLF + "         SUM(DECODE(Bun,'99',Amt1+Amt2,0)) AS Bun99";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                SQL = SQL + ComNum.VBLF + "         WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "             AND ACTDATE>=TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "             AND ACTDATE<=TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "     GROUP BY actdate, Pano, BI";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     ACtdate, Pano, Bi, TAmt, Bun92, Bun96, Bun98, Bun99, (TAmt - Bun92 - Bun96 - Bun98 - Bun99) AS Dansu";
                SQL = SQL + ComNum.VBLF + "FROM VIEW_OPD_DANSU_CHECK";
                SQL = SQL + ComNum.VBLF + "     WHERE ((TAmt - Bun92 - Bun96 - Bun98 - Bun99) > 100  or (TAmt - Bun92 - Bun96 - Bun98 - Bun99) < -100) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다.");
                    return;
                }

                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();

                    nRow += 1;

                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strPano;
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = "";
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = "";
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = "";
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = "";
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dt.Rows[i]["TAmt"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = "단수오류자입니다.";
                }

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, BI, SUM(AMT1) AS AMT1,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(ACTDATE, 'YYYY-MM-DD') AS ACTDATE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                SQL = SQL + ComNum.VBLF + "     WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "         AND ACTDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND PART = '#'";
                SQL = SQL + ComNum.VBLF + "         AND BUN = '99'";
                SQL = SQL + ComNum.VBLF + "GROUP BY PANO, BI, ACTDATE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    strPano = dt.Rows[i]["PANO"].ToString().Trim();
                    strDate1 = dt.Rows[i]["ACTDATE"].ToString().Trim();
                    nRBAmt7 = VB.Val(dt.Rows[i]["Amt1"].ToString().Trim());

                    SQL = "";

                    if (dtpDate.Value.ToString("yyyy-MM-dd") == ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"))
                    {
                        SQL = " SELECT SUM(AMT) AS AMT FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH";
                    }
                    else
                    {
                        SQL = " SELECT SUM(AMT) AS AMT FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH";
                    }

                    SQL = SQL + "       WHERE PANO ='" + strPano + "'";
                    SQL = SQL + "           AND ACTDATE = TO_DATE('" + strDate1 + " ','YYYY-MM-DD') ";
                    SQL = SQL + "           AND PART = '#'";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        if (nRBAmt7 != (VB.Val(dt1.Rows[0]["AMT"].ToString().Trim()) * (-1)))
                        {
                            nRow += 1;
                            if (nRow > ssView_Sheet1.RowCount)
                            {
                                ssView_Sheet1.RowCount = nRow;
                            }
                            ssView_Sheet1.Cells[nRow - 1, 0].Text = strPano;
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = "";
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = "";
                            ssView_Sheet1.Cells[nRow - 1, 3].Text = "";
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = "";
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dt.Rows[i]["Amt1"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = "외래금액이 당일입원 금액 차이 확인 & 누락";
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;
                }
                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                ComFunc.MsgBox("작업완료");
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void frmPmpaViewOpdDailyCheck_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");
            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-1);
        }
    }
}
