using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : FrmAntiTong.cs
    /// Description     : 제한항생제 의뢰 통계
    /// Author          : 이정현
    /// Create Date     : 2018-02-27
    /// <history> 
    /// 제한항생제 의뢰 통계
    /// </history>
    /// <seealso>
    /// PSMH\drug\drslip\FrmAntiTong.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drslip\drslip.vbp
    /// </vbp>
    /// </summary>
    public partial class FrmAntiTong : Form
    {
        public FrmAntiTong()
        {
            InitializeComponent();
        }

        private void FrmAntiTong_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssList_Sheet1.ColumnCount = 0;
            ssView_Sheet1.RowCount = 0;

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-14);
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
        }

        private void btnSearchDept_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;
            int k = 0;

            string strDeptCode = "";
            string strSTATE = "";

            double dblCOUNT = 0;
            double dblCOUNT1 = 0;
            double dblCOUNT2 = 0;
            double dblCOUNT3 = 0;
            double dblCOUNT4 = 0;
            double dblMAX = 0;
            
            ssList_Sheet1.ColumnCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     B.DEPTCODE, SUM(1) AS SUM";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ANTI_MST A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B, " + ComNum.DB_MED + "OCS_ORDERCODE D ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.SDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.SDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.IPDNO = B.IPDNO ";
                SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE = D.ORDERCODE ";
                SQL = SQL + ComNum.VBLF + "         AND A.SDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "GROUP BY B.DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "ORDER BY B.DEPTCODE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.ColumnCount = dt.Rows.Count + 1;
                    ssList_Sheet1.SetColumnWidth(-1, 53);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDeptCode = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        dblMAX = VB.Val(dt.Rows[i]["SUM"].ToString().Trim());

                        ssList_Sheet1.Cells[0, i].Text = strDeptCode;
                        ssList_Sheet1.Cells[1, i].Text = dblMAX.ToString();

                        dblCOUNT1 = 0;
                        dblCOUNT2 = 0;
                        dblCOUNT3 = 0;
                        dblCOUNT4 = 0;

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     B.DEPTCODE, A.STATE, SUM(1) AS COUNT ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ANTI_MST A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B, " + ComNum.DB_MED + "OCS_ORDERCODE D ";
                        SQL = SQL + ComNum.VBLF + "     WHERE A.SDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.SDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.IPDNO = B.IPDNO ";
                        SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE = D.ORDERCODE ";
                        SQL = SQL + ComNum.VBLF + "         AND A.SDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND B.DEPTCODE = '" + strDeptCode + "'";
                        SQL = SQL + ComNum.VBLF + "GROUP BY A.STATE, B.DEPTCODE ";
                        SQL = SQL + ComNum.VBLF + "ORDER BY B.DEPTCODE, A.STATE ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                strSTATE = dt1.Rows[k]["STATE"].ToString().Trim();
                                dblCOUNT = VB.Val(dt1.Rows[k]["COUNT"].ToString().Trim());

                                switch (strSTATE.Trim())
                                {
                                    case "1":
                                        dblCOUNT1 = dblCOUNT;
                                        ssList_Sheet1.Cells[2, i].Text = dblCOUNT1.ToString();
                                        break;
                                    case "2":
                                        dblCOUNT2 = dblCOUNT;
                                        ssList_Sheet1.Cells[4, i].Text = dblCOUNT2.ToString();
                                        break;
                                    case "3":
                                        dblCOUNT3 = dblCOUNT;
                                        ssList_Sheet1.Cells[6, i].Text = dblCOUNT3.ToString();
                                        break;
                                    case "":
                                        dblCOUNT4 = dblCOUNT;
                                        ssList_Sheet1.Cells[8, i].Text = dblCOUNT4.ToString();
                                        break;
                                }
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        for (k = 1; k <= 4; k++)
                        {
                            switch (k)
                            {
                                case 1:
                                    if (dblCOUNT1 > 0)
                                    {
                                        ssList_Sheet1.Cells[(k * 2) + 1, i].Text = ((dblCOUNT1 / dblMAX) * 100).ToString("0.0#");
                                    }
                                    break;
                                case 2:
                                    if (dblCOUNT2 > 0)
                                    {
                                        ssList_Sheet1.Cells[(k * 2) + 1, i].Text = ((dblCOUNT2 / dblMAX) * 100).ToString("0.0#");
                                    }
                                    break;
                                case 3:
                                    if (dblCOUNT3 > 0)
                                    {
                                        ssList_Sheet1.Cells[(k * 2) + 1, i].Text = ((dblCOUNT3 / dblMAX) * 100).ToString("0.0#");
                                    }
                                    break;
                                case 4:
                                    if (dblCOUNT4 > 0)
                                    {
                                        ssList_Sheet1.Cells[(k * 2) + 1, i].Text = ((dblCOUNT4 / dblMAX) * 100).ToString("0.0#");
                                    }
                                    break;
                            }
                        }

                        for(k = 0; k < 10; k++)
                        {
                            if (ssList_Sheet1.Cells[k, i].Text.Trim() == "")
                            {
                                ssList_Sheet1.Cells[k, i].Text = "0";
                            }
                        }
                    }

                    dblCOUNT = 0;
                    dblCOUNT1 = 0;

                    for (i = 1; i < 10; i++)
                    {
                        for (k = 0; k < ssList_Sheet1.ColumnCount - 1; k++)
                        {
                            if (i == 3 || i == 5 || i == 7 || i == 9)
                            {
                                ssList_Sheet1.Cells[i, k].Text = (VB.Val(ssList_Sheet1.Cells[i - 1, k].Text) / VB.Val(ssList_Sheet1.Cells[1, k].Text) * 100).ToString("0.0#") + "%";
                            }
                            else
                            {
                                dblCOUNT = VB.Val(ssList_Sheet1.Cells[i, k].Text.Trim());
                                dblCOUNT1 = dblCOUNT + dblCOUNT1;

                                ssList_Sheet1.Cells[i, ssList_Sheet1.ColumnCount - 1].Text = dblCOUNT1.ToString();
                            }
                        }

                        if (i == 3 || i == 5 || i == 7 || i == 9)
                        {
                            ssList_Sheet1.Cells[i, k].Text = (VB.Val(ssList_Sheet1.Cells[i - 1, k].Text) / VB.Val(ssList_Sheet1.Cells[1, k].Text) * 100).ToString("0.0#") + "%";
                        }

                        dblCOUNT = 0;
                        dblCOUNT1 = 0;
                    }

                    ssList_Sheet1.Cells[0, ssList_Sheet1.ColumnCount - 1].Text = "계";
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearchDrug_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;
            int k = 0;

            string strSTATE = "";
            string strSuCode = "";
            string strORDERNAMES = "";
            
            double dblSUM = 0;
            double dblSUM1 = 0;
            double dblSUM2 = 0;
            double dblSUM3 = 0;
            double dblSUM4 = 0;
            double dblMAXSUM = 0;

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     D.SUCODE, D.ORDERNAMES, SUM(1) AS SUM ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ANTI_MST A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B, " + ComNum.DB_MED + "OCS_ORDERCODE D ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.SDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.SDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.IPDNO = B.IPDNO ";
                SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE = D.ORDERCODE ";
                SQL = SQL + ComNum.VBLF + "GROUP BY D.SUCODE, ORDERNAMES";
                SQL = SQL + ComNum.VBLF + "ORDER BY SUM DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSuCode = dt.Rows[i]["SUCODE"].ToString().Trim();
                        strORDERNAMES = dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                        dblMAXSUM = VB.Val(dt.Rows[i]["SUM"].ToString().Trim());

                        ssView_Sheet1.Cells[i, 0].Text = strSuCode;
                        ssView_Sheet1.Cells[i, 1].Text = strORDERNAMES;
                        ssView_Sheet1.Cells[i, 2].Text = dblMAXSUM.ToString();

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     D.SUCODE, A.STATE, SUM(1) AS SUM ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ANTI_MST A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B, " + ComNum.DB_MED + "OCS_ORDERCODE D ";
                        SQL = SQL + ComNum.VBLF + "     WHERE A.SDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.SDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.IPDNO = B.IPDNO ";
                        SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE = D.ORDERCODE ";
                        SQL = SQL + ComNum.VBLF + "         AND D.SUCODE = '" + strSuCode + "'";
                        SQL = SQL + ComNum.VBLF + "GROUP BY D.SUCODE, A.STATE";
                        SQL = SQL + ComNum.VBLF + "ORDER BY A.STATE, SUM DESC";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                strSTATE = dt1.Rows[k]["STATE"].ToString().Trim();
                                dblSUM = Convert.ToInt32(VB.Val(dt1.Rows[k]["SUM"].ToString().Trim()));

                                switch (strSTATE.Trim())
                                {
                                    case "1":
                                        dblSUM1 = dblSUM;
                                        ssView_Sheet1.Cells[i, 3].Text = dblSUM1.ToString();
                                        break;
                                    case "2":
                                        dblSUM2 = dblSUM;
                                        ssView_Sheet1.Cells[i, 5].Text = dblSUM2.ToString();
                                        break;
                                    case "3":
                                        dblSUM3 = dblSUM;
                                        ssView_Sheet1.Cells[i, 7].Text = dblSUM3.ToString();
                                        break;
                                    case "":
                                        dblSUM4 = dblSUM;
                                        ssView_Sheet1.Cells[i, 9].Text = dblSUM4.ToString();
                                        break;
                                }
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (dblSUM1 > 0)
                        {
                            ssView_Sheet1.Cells[i, 4].Text = (dblSUM1 / dblMAXSUM * 100).ToString("0.0#") + "%";
                        }
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            ssList_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssList_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssList_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssList_Sheet1.PrintInfo.Margin.Top = 20;
            ssList_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssList_Sheet1.PrintInfo.ShowColor = false;
            ssList_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssList_Sheet1.PrintInfo.ShowBorder = true;
            ssList_Sheet1.PrintInfo.ShowGrid = true;
            ssList_Sheet1.PrintInfo.ShowShadows = false;
            ssList_Sheet1.PrintInfo.UseMax = true;
            ssList_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssList_Sheet1.PrintInfo.UseSmartPrint = false;
            ssList_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssList_Sheet1.PrintInfo.Preview = false;
            ssList.PrintSheet(0);

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Margin.Top = 20;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.UseSmartPrint = false;
            ssView_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssView_Sheet1.PrintInfo.Preview = false;
            ssView.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
