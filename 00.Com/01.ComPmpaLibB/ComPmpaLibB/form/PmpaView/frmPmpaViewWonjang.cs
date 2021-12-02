using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewWonjang.cs
    /// Description     : 미수금 총원장
    /// Author          : 박창욱
    /// Create Date     : 2017-09-22
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUG302.FRM(FrmWonjang.frm) >> frmPmpaViewWonjang.cs 폼이름 재정의" />	
    /// <seealso cref= "\misu\MISUM204.FRM(FrmWonjang.frm) >> frmPmpaViewWonjang.cs 폼이름 재정의" />	
    public partial class frmPmpaViewWonjang : Form
    {
        clsPmpaMisu cpm = new clsPmpaMisu();
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        public frmPmpaViewWonjang()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            int nRow = 0;
            string strYYMM = "";
            string strFDate = "";
            string strTDate = "";
            string strNewData = "";
            string strOldData = "";
            string strGubun = "";

            double nAmt = 0;
            double nQty = 0;
            double nTotIwolAmt = 0;
            double nTotMirAmt = 0;
            double nTotIpgumAmt = 0;

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(strYYMM, 4)), (int)VB.Val(VB.Right(strYYMM, 2)));
            nTotIwolAmt = 0;
            nTotMirAmt = 0;
            nTotIpgumAmt = 0;

            btnSearch.Enabled = false;
            btnPrint.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            try
            {
                //조합별 전월이월액 Display
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SUM(IwolAmt) cIwolAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GELTOT";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND YYMM = '" + strYYMM + "'";
                if (rdoInsu.Checked == true)
                {
                    if (rdoClass0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Class = '01'"; //공단
                    }
                    else if (rdoClass1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Class = '02'"; //직장
                    }
                    else if (rdoClass2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Class = '03'"; //지역
                    }
                    else if (rdoClass3.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Class = '04'"; //의보
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Class < '05'"; //전체
                    }
                }
                else if (rdoMisu.Checked == true)
                {
                    if (VB.Left(cboClass.Text, 2) == "00")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Class >= '08'";
                        SQL = SQL + ComNum.VBLF + "    AND Class <= '16'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Class = '" + VB.Left(cboClass.Text, 2) + "' ";
                    }
                }


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 1)
                {
                    nTotIwolAmt = VB.Val(dt.Rows[0]["cIwolAmt"].ToString().Trim());
                }

                nRow += 1;
                if (nRow > ssView_Sheet1.RowCount)
                {
                    ssView_Sheet1.RowCount = nRow;
                }
                ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 전월이월 **";
                ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotIwolAmt.ToString("###,###,###,##0");


                dt.Dispose();
                dt = null;


                //미수 상세내역 Display
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(a.Bdate,'YYYY-MM-DD') Bdate, a.Class, a.Gubun,";
                SQL = SQL + ComNum.VBLF + "        a.IpdOpd, b.Bun,SUM(a.Qty) cQty, SUM(a.Amt) cAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP a, " + ComNum.DB_PMPA + "MISU_IDMST b";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.Bdate >= TO_DATE('" + strFDate + "','yyyy-mm-dd')";
                SQL = SQL + ComNum.VBLF + "    AND a.Bdate <= TO_DATE('" + strTDate + "','yyyy-mm-dd')";
                if (rdoInsu.Checked == true)
                {
                    if (rdoClass0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND a.Class = '01'"; //공단
                    }
                    else if (rdoClass1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND a.Class = '02'"; //직장
                    }
                    else if (rdoClass2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND a.Class = '03'"; //지역
                    }
                    else if (rdoClass3.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND a.Class = '04'"; //의보
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND a.Class < '05'"; //전체
                    }
                }
                else if (rdoMisu.Checked == true)
                {
                    if (VB.Left(cboClass.Text, 2) == "00")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND a.Class >= '08'";
                        SQL = SQL + ComNum.VBLF + "    AND a.Class <= '16'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND a.Class = '" + VB.Left(cboClass.Text, 2) + "' ";
                    }
                }
                SQL = SQL + ComNum.VBLF + "    AND a.WRTNO = b.WRTNO(+)";
                SQL = SQL + ComNum.VBLF + "  GROUP BY a.Bdate,a.Class,a.IpdOpd,b.Bun,a.Gubun";
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.Bdate,a.Class,a.IpdOpd,b.Bun,a.Gubun";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                strOldData = "";

                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    strNewData = dt.Rows[i]["Bdate"].ToString().Trim() + dt.Rows[i]["Class"].ToString().Trim();
                    if (strNewData != strOldData)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = VB.Left(strNewData, 10);
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = cpm.READ_MisuClass(VB.Right(strNewData, 2));
                        strOldData = strNewData;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 2].Text = cpm.READ_MisuGye_TA(dt.Rows[i]["Gubun"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = cpm.READ_MisuIpdOpd(dt.Rows[i]["IpdOpd"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = cpm.READ_MisuBunya(dt.Rows[i]["Bun"].ToString().Trim());

                    strGubun = dt.Rows[i]["Gubun"].ToString().Trim();
                    nQty = VB.Val(dt.Rows[i]["cQty"].ToString().Trim());
                    nAmt = VB.Val(dt.Rows[i]["cAmt"].ToString().Trim());

                    switch (strGubun)
                    {
                        case "11":
                        case "12":
                        case "13":
                        case "14":
                        case "15":
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = nQty.ToString("###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 6].Text = nAmt.ToString("###,###,##0");
                            nTotMirAmt += nAmt;
                            break;
                        default:
                            ssView_Sheet1.Cells[nRow - 1, 7].Text = nQty.ToString("###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = nAmt.ToString("###,###,##0");
                            nTotIpgumAmt += nAmt;
                            break;
                    }

                    nAmt = nTotIwolAmt + nTotMirAmt - nTotIpgumAmt;
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = nAmt.ToString("###,###,###,##0");
                }
                dt.Dispose();
                dt = null;

                //월계 display
                nRow += 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.Cells[nRow - 1, 4].Text = "**월계**";
                ssView_Sheet1.Cells[nRow - 1, 6].Text = nTotMirAmt.ToString("###,###,##0 ");
                if (rdoInsu.Checked == true)
                {
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotMirAmt.ToString("###,###,##0 ");
                }
                else if (rdoMisu.Checked == true)
                {
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotIpgumAmt.ToString("###,###,##0 ");
                }
                nAmt = nTotIwolAmt + nTotMirAmt - nTotIpgumAmt;
                ssView_Sheet1.Cells[nRow - 1, 9].Text = nAmt.ToString("###,###,###,##0 ");


                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            string strInsu = "";
            string strClass = "";
            bool PrePrint = true;

            if (rdoClass0.Checked == true)
            {
                strInsu = "(공단)";
            }
            else if (rdoClass1.Checked == true)
            {
                strInsu = "(직장)";
            }
            else if (rdoClass2.Checked == true)
            {
                strInsu = "(지역)";
            }
            else if (rdoClass3.Checked == true)
            {
                strInsu = "(보호)";
            }
            else
            {
                strInsu = "(전체)";
            }

            strClass = VB.Right(cboClass.Text, cboClass.Text.Length - 3) + ")";

            if (rdoInsu.Checked == true)
            {
                strTitle = cboYYMM.Text + " 미수금 총원장 " + strInsu;
            }
            else if (rdoMisu.Checked == true)
            {
                strTitle = cboYYMM.Text + " 미수금 총원장 (" + strClass;
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력시간 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력자 : " + clsType.User.JobMan + " 인", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false, (float)0.85);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void frmPmpaViewWonjang_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int nMM = 0;
            int i = 0;
            string strSysDate = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            nYY = (int)VB.Val(VB.Left(strSysDate, 4));
            nMM = (int)VB.Val(VB.Mid(strSysDate, 6, 2));

            for (i = 1; i < 16; i++)
            {
                cboYYMM.Items.Add(nYY.ToString("0000") + "년 " + nMM.ToString("00") + "월분");
                nMM -= 1;
                if (nMM == 0)
                {
                    nMM = 12;
                    nYY -= 1;
                }
            }
            cboYYMM.SelectedIndex = 0;

            grbMisu.Location = new Point(372, 5);
            grbMisu.Visible = false;

            cboClass.Items.Clear();
            cboClass.Items.Add("00.전체미수");
            cboClass.Items.Add("08.계약처");
            cboClass.Items.Add("09.헌혈미수");
            cboClass.Items.Add("11.보훈청미수");
            cboClass.Items.Add("12.시각장애자");
            cboClass.Items.Add("13.심신장애진단비");
            cboClass.Items.Add("14.장애인보장구");
            cboClass.Items.Add("15.직원대납");
            cboClass.Items.Add("16.노인장기요양소견서");
            cboClass.Items.Add("17.방문간호지시서");
            cboClass.Items.Add("18.치매검사");
            cboClass.SelectedIndex = 0;
        }

        private void rdoGb_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoInsu.Checked == true)
            {
                grbInsu.Visible = true;
                grbMisu.Visible = false;
                ssView_Sheet1.Columns[4].Visible = true;
                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = 1;
            }
            else if (rdoMisu.Checked == true)
            {
                grbMisu.Visible = true;
                grbInsu.Visible = false;
                ssView_Sheet1.Columns[4].Visible = false;
                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = 1;
            }
        }
    }
}
