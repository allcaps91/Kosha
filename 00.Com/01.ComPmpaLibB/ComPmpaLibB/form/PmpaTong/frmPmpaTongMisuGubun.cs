using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaTongMisuGubun.cs
    /// Description     : 월말현재 미수종류별 통계
    /// Author          : 박창욱
    /// Create Date     : 2017-08-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 2017-08-31 박창욱 : Frm개인미수통계표1.FRM과 통합
    /// </history>
    /// <seealso cref= "\misu\MISUP204.frm(FrmMisuGubunTong.frm) >> frmPmpaTongMisuGubun.cs 폼이름 재정의" />	
    /// <seealso cref= "\misu\Frm개인미수통계표1.frm(Frm개인미수통계표1.frm) >> frmPmpaTongMisuGubun.cs 폼이름 재정의" />	
    public partial class frmPmpaTongMisuGubun : Form
    {
        double[] nTotCnt = new double[8];
        double[] nTotAmt = new double[8];
        string PgGubun;  //
        public frmPmpaTongMisuGubun(string Gubun)
        {
            InitializeComponent();
            PgGubun = Gubun;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 2;
            ssView_Sheet1.RowCount = 13;
            
            ssView_Sheet2.RowCount = 2;
            ssView_Sheet2.RowCount = 30;
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
            string strHead = "";
            string strHead1 = "";
            string strHead2 = "";
            string PrintDate = "";
            string JobMan = "";

     
            PrintDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon,"A"), "A");

            //Print Head
            
            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";
            if (ssView.ActiveSheetIndex == 0)
            {
                strHead1 = "/c/f1" + "미수종류별 통계" + "/n/n";
                strHead2 = "/l/f2" + "작업년월 : " + cboFYYMM.Text + "~" + cboTYYMM.Text;
                //strHead2 = "/l/f2" + "작성자 : " + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun) + "/f2/n";
                //  strHead2 += "/l/f2" + VB.Space(11) + "작업년월 : " +  cboFYYMM.Text + "~" + cboTYYMM.Text  +   "/f2/n";
                //  strHead2 += "/l/f2" + VB.Space(11) + "출력시간 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + "/f2/n";

            }
            else if (ssView.ActiveSheetIndex == 1)
            {
             

                strHead1 = "/c/f1" + "개인 및 기관미수 내역 총괄표" + "/n/n" ;
                strHead2 = "/l/f2" + "작업년월 : " + cboFYYMM.Text + "~" + cboTYYMM.Text;
            }

            //Print Body
            ssView.ActiveSheet.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView.ActiveSheet.PrintInfo.Margin.Left = 50;
            ssView.ActiveSheet.PrintInfo.Margin.Right = 10;
            ssView.ActiveSheet.PrintInfo.Margin.Top = 50;
            ssView.ActiveSheet.PrintInfo.Margin.Bottom = 10;
            ssView.ActiveSheet.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView.ActiveSheet.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView.ActiveSheet.PrintInfo.ShowBorder = true;
            ssView.ActiveSheet.PrintInfo.ShowColor = false;
            ssView.ActiveSheet.PrintInfo.ShowGrid = false;
            ssView.ActiveSheet.PrintInfo.ShowShadows = false;
            ssView.ActiveSheet.PrintInfo.UseMax = false;
            ssView.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView.PrintSheet(ssView.ActiveSheet);







        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int nRow = 0;
            string strMisuGyeName = "";
            string strFYYMM = "";
            string strTYYMM = "";

            for (i = 1; i < 8; i++)
            {
                nTotCnt[i] = 0;
                nTotAmt[i] = 0;
            }

            ssView_Sheet1.RowCount = 2;
            ssView_Sheet1.RowCount = 13;

            ssView_Sheet2.RowCount = 2;
            ssView_Sheet2.RowCount = 30;

            strFYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 6, 2);
            strTYYMM = VB.Left(cboTYYMM.Text, 4) + VB.Mid(cboTYYMM.Text, 6, 2);

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            btnPrint.Enabled = false;
            btnExit.Enabled = false;

            //Sheet1
            try
            {
                SQL = "";
                SQL = " SELECT Gubun,                                                                         ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(IwolAmt,0,0,1)) IwolCnt,SUM(IwolAmt) IwolAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(MonMAmt,0,0,1)) MonMCnt,SUM(MonMAmt) MonMAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(MonIAmt,0,0,1)) MonICnt,SUM(MonIAmt) MonIAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(MonBAmt,0,0,1)) MonBCnt,SUM(MonBAmt) MonBAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(MonGAmt,0,0,1)) MonGCnt,SUM(MonGAmt) MonGAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(MonSAmt,0,0,1)) MonSCnt,SUM(MonSAmt) MonSAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(JanAmt,0,0,1))  JanCnt, SUM(JanAmt)  JanAmt     ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINTONG                        ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                      ";
                SQL = SQL + ComNum.VBLF + "    AND YYMM >= '" + strFYYMM + "'                                 ";
                SQL = SQL + ComNum.VBLF + "    AND YYMM <= '" + strTYYMM + "'                                 ";
                if (rdoGbn1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND Gubun <> '11'                                          ";
                }
                if (rdoGbn2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND Gubun =  '11'                                          ";
                }
                SQL = SQL + ComNum.VBLF + "  GROUP BY Gubun                                                   ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Gubun                                                   ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다.");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    btnExit.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRow = dt.Rows.Count;
                ssView_Sheet1.RowCount = 2;
                ssView_Sheet1.RowCount = nRow + 3;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRow; i++)
                {
                    nTotCnt[1] += VB.Val(dt.Rows[i]["IwolCnt"].ToString().Trim());
                    nTotCnt[2] += VB.Val(dt.Rows[i]["MonMCnt"].ToString().Trim());
                    nTotCnt[3] += VB.Val(dt.Rows[i]["MonICnt"].ToString().Trim());
                    nTotCnt[4] += VB.Val(dt.Rows[i]["MonBCnt"].ToString().Trim());
                    nTotCnt[5] += VB.Val(dt.Rows[i]["MonGCnt"].ToString().Trim());
                    nTotCnt[6] += VB.Val(dt.Rows[i]["MonSCnt"].ToString().Trim());
                    nTotCnt[7] += VB.Val(dt.Rows[i]["JanCnt"].ToString().Trim());

                    nTotAmt[1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());
                    nTotAmt[2] += VB.Val(dt.Rows[i]["MonMAmt"].ToString().Trim());
                    nTotAmt[3] += VB.Val(dt.Rows[i]["MonIAmt"].ToString().Trim());
                    nTotAmt[4] += VB.Val(dt.Rows[i]["MonBAmt"].ToString().Trim());
                    nTotAmt[5] += VB.Val(dt.Rows[i]["MonGAmt"].ToString().Trim());
                    nTotAmt[6] += VB.Val(dt.Rows[i]["MonSAmt"].ToString().Trim());
                    nTotAmt[7] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());

                    strMisuGyeName = READ_PerMisuGye(dt.Rows[i]["Gubun"].ToString().Trim());
                    if (strMisuGyeName == "")
                    {
                        strMisuGyeName = "ERROR(" + dt.Rows[i]["Gubun"].ToString().Trim() + ")";
                    }
                    ssView_Sheet1.Cells[i + 2, 0].Text = strMisuGyeName;
                    ssView_Sheet1.Cells[i + 2, 1].Text = VB.Val(dt.Rows[i]["IwolCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 2].Text = VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 3].Text = VB.Val(dt.Rows[i]["MonMCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 4].Text = VB.Val(dt.Rows[i]["MonMAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 5].Text = VB.Val(dt.Rows[i]["MonICnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 6].Text = VB.Val(dt.Rows[i]["MonIAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 7].Text = VB.Val(dt.Rows[i]["MonBCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 8].Text = VB.Val(dt.Rows[i]["MonBAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 9].Text = VB.Val(dt.Rows[i]["MonGCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 10].Text = VB.Val(dt.Rows[i]["MonGAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 11].Text = VB.Val(dt.Rows[i]["MonSCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 12].Text = VB.Val(dt.Rows[i]["MonSAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 13].Text = VB.Val(dt.Rows[i]["JanCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 14].Text = VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.Cells[nRow + 2, 0].Text = "** 합계 **";
                ssView_Sheet1.Cells[nRow + 2, 1].Text = nTotCnt[1].ToString("###,##0 ");
                ssView_Sheet1.Cells[nRow + 2, 2].Text = nTotAmt[1].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow + 2, 3].Text = nTotCnt[2].ToString("###,##0 ");
                ssView_Sheet1.Cells[nRow + 2, 4].Text = nTotAmt[2].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow + 2, 5].Text = nTotCnt[3].ToString("###,##0 ");
                ssView_Sheet1.Cells[nRow + 2, 6].Text = nTotAmt[3].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow + 2, 7].Text = nTotCnt[4].ToString("###,##0 ");
                ssView_Sheet1.Cells[nRow + 2, 8].Text = nTotAmt[4].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow + 2, 9].Text = nTotCnt[5].ToString("###,##0 ");
                ssView_Sheet1.Cells[nRow + 2, 10].Text = nTotAmt[5].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow + 2, 11].Text = nTotCnt[6].ToString("###,##0 ");
                ssView_Sheet1.Cells[nRow + 2, 12].Text = nTotAmt[6].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow + 2, 13].Text = nTotCnt[7].ToString("###,##0 ");
                ssView_Sheet1.Cells[nRow + 2, 14].Text = nTotAmt[7].ToString("###,###,###,##0 ");
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                btnExit.Enabled = true;
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }

            for (i = 1; i < 8; i++)
            {
                nTotCnt[i] = 0;
                nTotAmt[i] = 0;
            }

            //Sheet2
            try
            {
                SQL = "";
                SQL = " SELECT YYMM,                                                                          ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(IwolAmt,0,0,1)) IwolCnt,SUM(IwolAmt) IwolAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(MonMAmt,0,0,1)) MonMCnt,SUM(MonMAmt) MonMAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(MonIAmt,0,0,1)) MonICnt,SUM(MonIAmt) MonIAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(MonBAmt,0,0,1)) MonBCnt,SUM(MonBAmt) MonBAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(MonGAmt,0,0,1)) MonGCnt,SUM(MonGAmt) MonGAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(MonSAmt,0,0,1)) MonSCnt,SUM(MonSAmt) MonSAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(JanAmt,0,0,1))  JanCnt, SUM(JanAmt)  JanAmt     ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINTONG                        ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                      ";
                SQL = SQL + ComNum.VBLF + "    AND YYMM >= '" + strFYYMM + "'                                 ";
                SQL = SQL + ComNum.VBLF + "    AND YYMM <= '" + strTYYMM + "'                                 ";
                if (rdoGbn1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND Gubun <> '11'                                          ";
                }
                if (rdoGbn2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND Gubun =  '11'                                          ";
                }
                SQL = SQL + ComNum.VBLF + "  GROUP BY YYMM                                                    ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY YYMM                                                    ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다.");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    btnExit.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRow = dt.Rows.Count;
                ssView_Sheet2.RowCount = 2;
                ssView_Sheet2.RowCount = 30;
                ssView_Sheet2.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRow; i++)
                {
                    nTotCnt[1] += VB.Val(dt.Rows[i]["IwolCnt"].ToString().Trim());
                    nTotCnt[2] += VB.Val(dt.Rows[i]["MonMCnt"].ToString().Trim());
                    nTotCnt[3] += VB.Val(dt.Rows[i]["MonICnt"].ToString().Trim());
                    nTotCnt[4] += VB.Val(dt.Rows[i]["MonBCnt"].ToString().Trim());
                    nTotCnt[5] += VB.Val(dt.Rows[i]["MonGCnt"].ToString().Trim());
                    nTotCnt[6] += VB.Val(dt.Rows[i]["MonSCnt"].ToString().Trim());
                    nTotCnt[7] += VB.Val(dt.Rows[i]["JanCnt"].ToString().Trim());

                    nTotAmt[1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());
                    nTotAmt[2] += VB.Val(dt.Rows[i]["MonMAmt"].ToString().Trim());
                    nTotAmt[3] += VB.Val(dt.Rows[i]["MonIAmt"].ToString().Trim());
                    nTotAmt[4] += VB.Val(dt.Rows[i]["MonBAmt"].ToString().Trim());
                    nTotAmt[5] += VB.Val(dt.Rows[i]["MonGAmt"].ToString().Trim());
                    nTotAmt[6] += VB.Val(dt.Rows[i]["MonSAmt"].ToString().Trim());
                    nTotAmt[7] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());

                    strMisuGyeName = dt.Rows[i]["YYMM"].ToString().Trim();
                    if (strMisuGyeName == "")
                    {
                        strMisuGyeName = "ERROR(" + dt.Rows[i]["YYMM"].ToString().Trim() + ")";
                    }
                    ssView_Sheet2.Cells[i + 2, 0].Text = strMisuGyeName;
                    ssView_Sheet2.Cells[i + 2, 1].Text = VB.Val(dt.Rows[i]["IwolCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet2.Cells[i + 2, 2].Text = VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet2.Cells[i + 2, 3].Text = VB.Val(dt.Rows[i]["MonMCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet2.Cells[i + 2, 4].Text = VB.Val(dt.Rows[i]["MonMAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet2.Cells[i + 2, 5].Text = VB.Val(dt.Rows[i]["MonICnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet2.Cells[i + 2, 6].Text = VB.Val(dt.Rows[i]["MonIAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet2.Cells[i + 2, 7].Text = VB.Val(dt.Rows[i]["MonBCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet2.Cells[i + 2, 8].Text = VB.Val(dt.Rows[i]["MonBAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet2.Cells[i + 2, 9].Text = VB.Val(dt.Rows[i]["MonGCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet2.Cells[i + 2, 10].Text = VB.Val(dt.Rows[i]["MonGAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet2.Cells[i + 2, 11].Text = VB.Val(dt.Rows[i]["MonSCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet2.Cells[i + 2, 12].Text = VB.Val(dt.Rows[i]["MonSAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet2.Cells[i + 2, 13].Text = VB.Val(dt.Rows[i]["JanCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet2.Cells[i + 2, 14].Text = VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                }

                for (i = 0; i < ssView_Sheet2.RowCount; i++)
                {
                    if (ssView_Sheet2.Cells[i, 1].Text == "")
                    {
                        ssView_Sheet2.RowCount = i;
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                btnExit.Enabled = true;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                btnExit.Enabled = true;
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }






        }

        string READ_PerMisuGye(string arg)
        {
            string rtnVal = "";

            switch (arg)
            {
                case "01":
                    rtnVal = "가퇴원미수";
                    break;
                case "02":
                    rtnVal = "업무착오미수";
                    break;
                case "03":
                    rtnVal = "탈원미수";
                    break;
                case "04":
                    rtnVal = "지불각서";
                    break;
                case "05":
                    rtnVal = "응급미수";
                    break;
                case "06":
                    rtnVal = "외래미수";
                    break;
                case "07":
                    rtnVal = "심사청구미수";
                    break;
                case "08":
                    rtnVal = "책임보험";
                    break;
                case "09":
                    rtnVal = "퇴원";
                    break;
                case "10":
                    rtnVal = "기타";
                    break;
                case "11":
                    rtnVal = "기관청구미수";
                    break;
                case "12":
                    rtnVal = "입원정밀";
                    break;
                case "13":
                    rtnVal = "필수접종국가지원";
                    break;
                case "14":
                    rtnVal = "회사접종";
                    break;
                case "15":
                    rtnVal = "금연처방";
                    break;
                default:
                    rtnVal = "";
                    break;
            }
            return rtnVal;
        }

        private void frmPmpaTongMisuGubun_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFYYMM, 20, "", "0");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTYYMM, 20, "", "0");
            cboFYYMM.SelectedIndex = 0;
            cboTYYMM.SelectedIndex = 0;

            if (PgGubun == "미수구분" )
            {
                ssView_Sheet1.Visible = true;
                ssView_Sheet2.Visible = false;

              
            }
            else
            {
                ssView_Sheet2.Visible = true;
                ssView_Sheet1.Visible = false;
            }

        }

        private void rdoGbn_CheckedChanged(object sender, EventArgs e)
        {
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {
                if (ctl is RadioButton)
                {
                    if (VB.Left(((RadioButton)ctl).Name, 6) == "rdoGbn")
                    {
                        if (((RadioButton)ctl).Checked == true)
                        {
                            ((RadioButton)ctl).ForeColor = Color.FromArgb(0, 0, 255);
                        }
                        else
                        {
                            ((RadioButton)ctl).ForeColor = Color.FromArgb(0, 0, 0);
                        }
                    }
                }
            }
        }

      
    }
}
