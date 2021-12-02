using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewDayCount.cs
    /// Description     : 외래/입원 과장님별 처방일수 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-09-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iviewa\ipdsim101.frm(FrmDayCount.frm) >> frmPmpaViewDayCount.cs 폼이름 재정의" />	
    public partial class frmPmpaViewDayCount : Form
    {
        public frmPmpaViewDayCount()
        {
            InitializeComponent();
        }

        private void frmPmpaViewDayCount_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept);
            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            cboDr.Items.Clear();
            cboDr.Items.Add("****.전체");
            cboDr.SelectedIndex = 0;

            cboGB.Items.Clear();
            cboGB.Items.Add("1.약");
            cboGB.Items.Add("2.주사");
            cboGB.Items.Add("3.MRI");
            cboGB.Items.Add("4.검사");
            cboGB.Items.Add("5.방사선(일반)");
            cboGB.SelectedIndex = 0;
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
            string strHead1 = "";
            string JobDate = "";
            string PrintDate = "";
            string JobMan = "";

            PrintDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");
            JobDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            JobMan = clsType.User.JobMan;

            //Print Head 지정
            strFont1 = "/fn\"바탕체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            if (rdoIO0.Checked == true)
            {
                strHead1 = "/f1" + VB.Space(30) + "(" + JobDate + ")  외 래 과 장 님 처 방 일 수 조 회 [" + cboDr.Text + "]" + "/n";
            }
            else if (rdoIO1.Checked == true)
            {
                strHead1 = "/f1" + VB.Space(30) + "(" + JobDate + ")  입 원 과 장 님 처 방 일 수 조 회 [" + cboDr.Text + "]" + "/n";
            }
            else
            {
                strHead1 = "/f1" + VB.Space(30) + "(" + JobDate + ")  수술예방적 항생제 처방일수 조회 [" + cboDr.Text + "]" + "/n";
            }

            //Print Body
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1;
            ssView_Sheet1.PrintInfo.Margin.Left = 1;
            ssView_Sheet1.PrintInfo.Margin.Right = 5;
            ssView_Sheet1.PrintInfo.Margin.Top = 5;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 130;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            progressBar1.Value = 0;

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRow = 0;
            int nRead = 0;
            string strFDate = "";
            string strTDate = "";
            string strDrCode = "";
            string strDeptCode = "";
            string strPano = "";
            long nCountP = 0;       //총환자수
            long nSumTot = 0;       //총 투약 일당 비
            long nAmtTot = 0;       //총 금액
            long nSum = 0;          //1인당 약값
            long nCountB = 0;       //1인당 품목수
            long nNal = 0;          //1인당 처방날수
            long nQty = 0;          //1인당 처방수량
            long nAmt = 0;          //1인당 총금액

            Cursor.Current = Cursors.WaitCursor;

            strDrCode = VB.Left(cboDr.Text, 4);
            strDeptCode = VB.Left(cboDept.Text, 2);
            if (strDrCode.Trim() == "" || strDeptCode.Trim() == "")
            {
                ComFunc.MsgBox("해당하는 진료과와 진료과장을 선택해주세요.");
                cboDept.Focus();
                return;
            }
            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");

            ssView_Sheet1.RowCount = 0;
            nRow = 0;

            try
            {
                if (rdoIO0.Checked == true)
                {
                    SQL = "";
                    SQL = " SELECT B.PANO, B.SNAME, A.DEPTCODE, A.DRCODE, A.BI, A.SUNEXT, A.QTY, A.NAL, A.GBSELF, ";
                    SQL = SQL + ComNum.VBLF + " C.SUNAMEK,C.DAICODE, D.BAMT, '' INDATE ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "OPD_MASTER B, ";
                    SQL = SQL + ComNum.VBLF + ComNum.DB_PMPA + "BAS_SUN C, " + ComNum.DB_PMPA + "BAS_SUT D";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    if (strDeptCode != "**")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE  ='" + strDeptCode + "'";
                    }
                    if (strDrCode != "****")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND A.DRCODE = '" + strDrCode + "'";
                    }

                    switch (VB.Left(cboGB.Text, 1))
                    {
                        case "1":
                            SQL = SQL + ComNum.VBLF + " AND A.BUN IN ('11','12')";
                            break;
                        case "2":
                            SQL = SQL + ComNum.VBLF + " AND A.BUN ='20' ";
                            break;
                        case "3":
                            SQL = SQL + ComNum.VBLF + " AND A.BUN ='73' ";
                            break;
                        case "4":
                            SQL = SQL + ComNum.VBLF + " AND A.BUN >='41' AND A.BUN <= '73'";
                            break;
                        case "5":
                            SQL = SQL + ComNum.VBLF + " AND A.BUN >='65' And A.BUN <= '68'";
                            break;
                    }
                    SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO ";
                    SQL = SQL + ComNum.VBLF + "   AND A.BDATE =B.BDATE";
                    SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = C.SUNEXT(+)";
                    SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = D.SUNEXT(+)";
                    SQL = SQL + ComNum.VBLF + " GROUP BY B.PANO, B.SNAME, A.DEPTCODE, A.DRCODE, A.BI, A.SUNEXT, A.NAL, A.QTY, A.GBSELF, ";
                    SQL = SQL + ComNum.VBLF + "          C.SUNAMEK,C.DAICODE,D.BAMT";
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT B.PANO, B.SNAME,B.BI, B.ROOMCODE, A.DEPTCODE, A.DRCODE, A.SUNEXT, A.QTY, A.NAL, ";
                    SQL = SQL + ComNum.VBLF + " A.GBSELF, C.SUNAMEK,C.DAICODE, D.BAMT, TO_CHAR(B.INDATE,'YYYY-MM-DD') INDATE ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B, ";
                    SQL = SQL + ComNum.VBLF + ComNum.DB_PMPA + "BAS_SUN C, " + ComNum.DB_PMPA + "BAS_SUT D";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    if (strDeptCode != "**")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND a.DEPTCODE  ='" + strDeptCode + "'";
                    }
                    if (strDrCode != "****")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND A.DRCODE = '" + strDrCode + "'";
                    }

                    switch (VB.Left(cboGB.Text, 1))
                    {
                        case "1":
                            SQL = SQL + ComNum.VBLF + " AND A.BUN IN ('11','12')";
                            break;
                        case "2":
                            SQL = SQL + ComNum.VBLF + " AND A.BUN ='20' ";
                            break;
                        case "3":
                            SQL = SQL + ComNum.VBLF + " AND A.BUN ='73' ";
                            break;
                        case "4":
                            SQL = SQL + ComNum.VBLF + "   AND A.BUN >='41' AND A.BUN <='73'";
                            break;
                        case "5":
                            SQL = SQL + ComNum.VBLF + " AND A.BUN >='65' And A.BUN <= '68'";
                            break;
                    }
                    if (rdoIO2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND B.OP_JIPYO ='Y'";
                    }
                    SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO ";
                    SQL = SQL + ComNum.VBLF + "   AND B.OUTDATE IS NULL";
                    SQL = SQL + ComNum.VBLF + "   AND B.ACTDATE IS NULL";
                    SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = C.SUNEXT(+)";
                    SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = D.SUNEXT(+)";
                    SQL = SQL + ComNum.VBLF + " GROUP BY B.PANO, B.SNAME, B.BI, B.ROOMCODE, A.DEPTCODE, A.DRCODE, A.SUNEXT, A.NAL, ";
                    SQL = SQL + ComNum.VBLF + " A.QTY, A.GBSELF, C.SUNAMEK, C.DAICODE, D.BAMT, TO_CHAR(B.INDATE,'YYYY-MM-DD') ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = nRead + 200;
                nRow = 1;
                nCountP = 0;
                nCountB = 0;
                nQty = 0;
                nNal = 0;
                nSum = 0;

                ssView_Sheet1.ColumnHeader.Cells[0, 16].Text = "비고";
                if (rdoIO1.Checked == true && VB.Left(cboGB.Text, 1) == "2")
                {
                    ssView_Sheet1.ColumnHeader.Cells[0, 16].Text = "수술실";
                }
                if (rdoIO1.Checked == true && VB.Left(cboGB.Text, 1) == "4")
                {
                    ssView_Sheet1.ColumnHeader.Cells[0, 16].Text = "입원일";
                }

                progressBar1.Maximum = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    progressBar1.Value = i + 1;

                    if (strPano != dt.Rows[i]["Pano"].ToString().Trim())
                    {
                        if (nRow != 1)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 12].Text = "소   계";
                            ssView_Sheet1.Cells[nRow - 1, 13].Text = nAmt.ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = nSum.ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 15].Text = nCountB.ToString("###,###,###,##0");

                            nRow += 1;
                            ssView_Sheet1.RowCount = nRow;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        strPano = dt.Rows[i]["Pano"].ToString().Trim();
                        nCountP += 1;
                        nCountB = 0;
                        nSum = 0;
                        nAmt = 0;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["BI"].ToString().Trim();
                    if (rdoIO1.Checked == true || rdoIO2.Checked == true)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    }
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["DAICODE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = ReadDaiCodeName(dt.Rows[i]["DAICODE"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()).ToString("###,###,###,##0");
                    nQty = (long)VB.Val(dt.Rows[i]["QTY"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["NAL"].ToString().Trim() + " ";
                    nNal = (long)VB.Val(dt.Rows[i]["Nal"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GBSELF"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = VB.Val(dt.Rows[i]["BAMT"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = (nQty * nNal * VB.Val(dt.Rows[i]["BAMT"].ToString().Trim())).ToString("###,###,###,##0 ");

                    if ((rdoIO1.Checked == true || rdoIO2.Checked == true) && VB.Left(cboGB.Text, 1) == "2")
                    {
                        SQL = "";
                        SQL = "SELECT TO_CHAR(OPDATE,'YYYY-MM-DD') OPDATE FROM " + ComNum.DB_PMPA + "ORAN_MASTER ";
                        SQL = SQL + "  WHERE 1 = 1";
                        SQL = SQL + "    AND PANO ='" + strPano + "' ";
                        SQL = SQL + "    AND FLAG ='2'";
                        SQL = SQL + " ORDER BY OPDATE DESC ";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt.Rows.Count != 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 16].Text = VB.Mid(dt1.Rows[0]["OPDATE"].ToString().Trim(), 3, dt1.Rows[0]["OPDATE"].ToString().Trim().Length);
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }

                    if ((rdoIO1.Checked == true || rdoIO2.Checked == true) && VB.Left(cboGB.Text, 1) == "4" || VB.Left(cboGB.Text, 1) == "1")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    }

                    nSum += nQty * (long)VB.Val(dt.Rows[i]["BAMT"].ToString().Trim());
                    nAmt += nQty * nNal * (long)VB.Val(dt.Rows[i]["BAMT"].ToString().Trim());
                    nAmtTot += nQty * nNal * (long)VB.Val(dt.Rows[i]["BAMT"].ToString().Trim());
                    nSumTot += nQty * (long)VB.Val(dt.Rows[i]["BAMT"].ToString().Trim());
                    nCountB += 1;
                    nRow += 1;
                    ssView_Sheet1.RowCount = nRow;
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.Cells[nRow - 1, 12].Text = "소   계";
                ssView_Sheet1.Cells[nRow - 1, 13].Text = nAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 14].Text = nSum.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 15].Text = nCountB.ToString("###,###,###,##0");

                nRow += 1;
                ssView_Sheet1.RowCount = nRow;

                ssView_Sheet1.Cells[nRow - 1, 12].Text = "총   계";
                ssView_Sheet1.Cells[nRow - 1, 13].Text = (nAmtTot / nCountP).ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 14].Text = (nSumTot / nCountP).ToString("###,###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 15].Text = (i / nCountP).ToString("###,###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 16].Text = "총" + nCountP + "명";
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        string ReadDaiCodeName(string argCode)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Val(argCode) == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL = "SELECT ClassName FROM " + ComNum.DB_PMPA + "BAS_CLASS ";
                SQL = SQL + ComNum.VBLF + "WHERE ClassCode=" + VB.Val(argCode) + " ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["ClassName"].ToString().Trim();
                }
                else
                {
                    rtnVal = "** ERROR **";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strCode = "";
            string strName = "";

            cboDr.Items.Clear();
            cboDr.Items.Add("****.전체");

            try
            {
                if (cboDept.Text != "")
                {
                    SQL = "";
                    SQL = " SELECT DrCode, DrName FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                    if (VB.Left(cboDept.Text, 2) != "**.전체")
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE DrDept1 = '" + VB.Left(cboDept.Text, 2) + "'";
                    }
                    SQL = SQL + ComNum.VBLF + "  AND Tour   != 'Y' ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strCode = dt.Rows[i]["DrCode"].ToString().Trim();
                        strName = dt.Rows[i]["DrName"].ToString().Trim();
                        cboDr.Items.Add(strCode + " " + strName);
                    }
                    dt.Dispose();
                    dt = null;
                }
                cboDr.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }



        private void dtpFDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dtpTDate.Focus();
            }
        }

        private void dtpTDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cboDept.Focus();
            }
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }
    }
}
