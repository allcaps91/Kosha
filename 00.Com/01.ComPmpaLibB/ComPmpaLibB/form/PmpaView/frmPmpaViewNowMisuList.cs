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
    /// File Name       : frmPmpaViewNowMisuList.cs
    /// Description     : 금일현재미수금명단조회
    /// Author          : 박창욱
    /// Create Date     : 2017-10-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 2017-10-26 박창욱 : MISUG311.FRM과 MUMAIN06.FRM을 통합함
    /// </history>
    /// <seealso cref= "\misu\MISUG311.FRM(FrmMisuView.frm) >> frmPmpaViewNowMisuList.cs 폼이름 재정의" />	
    /// <seealso cref= "\misu\MUMAIN06.FRM(FrmMisuView.frm) >> frmPmpaViewNowMisuList.cs 폼이름 재정의" />	
    public partial class frmPmpaViewNowMisuList : Form
    {
        double GnWRTNO = 0;

        //배열
        //1.건당 2.조합소계 3.미수종류별 4.전체합계
        //1.총진료비 2.청구 3.입금 4.삭감 5.반송 6.과지급 7.계산착오 8.잔액
        double[,] nTotAmt = new double[5, 9];

        clsPmpaFunc cpf = new clsPmpaFunc();
        clsPmpaMisu cpm = new clsPmpaMisu();

        public frmPmpaViewNowMisuList()
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

            int k = 0;
            int nRow = 0;
            int nIlsu = 0;
            string strNewData = "";
            string strOldData = "";

            btnSearch.Enabled = false;
            btnPrint.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;

            for (i = 0; i < 5; i++)
            {
                for (k = 0; k < 9; k++)
                {
                    nTotAmt[i, k] = 0;
                }
            }

            try
            {
                if (rdoSort0.Checked == true)
                {
                    #region Data_Display1

                    //조합별 미수 상세내역 Display
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT WRTNO, Class, GelCode,";
                    SQL = SQL + ComNum.VBLF + "        TO_CHAR(Bdate,'YYYY-MM-DD') Bdate, MisuID, IpdOpd,";
                    SQL = SQL + ComNum.VBLF + "        Bun, JepsuNo, Amt1,";
                    SQL = SQL + ComNum.VBLF + "        Amt2, Amt3, Amt4,";
                    SQL = SQL + ComNum.VBLF + "        Amt5, Amt6, Amt7,";
                    SQL = SQL + ComNum.VBLF + "        Qty1, Qty2, Qty3,";
                    SQL = SQL + ComNum.VBLF + "        Qty4";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_IDMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE GbEnd = '1'";
                    if (VB.Left(cboClass.Text, 2) == "05")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Class < '05'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Class='" + VB.Left(cboClass.Text, 2) + "'";
                    }
                    SQL = SQL + ComNum.VBLF + "  ORDER BY Class,GelCode,Bdate,MisuID,IpdOpd,Bun";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        btnSearch.Enabled = true;
                        Cursor.Current = Cursors.Default;
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        return;
                    }

                    strOldData = VB.Space(18);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nIlsu = (int)VB.DateDiff("D", Convert.ToDateTime(dt.Rows[i]["Bdate"].ToString().Trim()), Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")));
                        if (nIlsu >= VB.Val(txtIlsu.Text))
                        {
                            strNewData = dt.Rows[i]["Class"].ToString().Trim() + VB.Left(dt.Rows[i]["GelCode"].ToString().Trim() + VB.Space(8), 8);
                            strNewData += dt.Rows[i]["Bdate"].ToString().Trim();

                            if (strOldData.Trim() != "")
                            {
                                if (VB.Left(strOldData, 2) != VB.Left(strNewData, 2))
                                {
                                    #region Total_Class_Rtn

                                    //미수종류별 소계
                                    nRow += 1;
                                    if (nRow > ssView_Sheet1.RowCount)
                                    {
                                        ssView_Sheet1.RowCount = nRow;
                                    }
                                    ssView_Sheet1.Cells[nRow - 1, 1].Text = "** 종류별계 **";
                                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[3, 2].ToString("###,###,###,##0");
                                    ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotAmt[3, 8].ToString("###,###,###,##0");
                                    ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotAmt[3, 3].ToString("###,###,###,##0");
                                    ssView_Sheet1.Cells[nRow - 1, 13].Text = nTotAmt[3, 4].ToString("###,###,###,##0");
                                    ssView_Sheet1.Cells[nRow - 1, 15].Text = nTotAmt[3, 5].ToString("###,###,###,##0");
                                    ssView_Sheet1.Cells[nRow - 1, 16].Text = nTotAmt[3, 6].ToString("###,###,###,##0");
                                    ssView_Sheet1.Cells[nRow - 1, 17].Text = nTotAmt[3, 7].ToString("###,###,###,##0");
                                    ssView_Sheet1.Cells[nRow - 1, 18].Text = nTotAmt[3, 1].ToString("###,###,###,##0");

                                    for (k = 1; k < 9; k++)
                                    {
                                        nTotAmt[3, k] = 0;
                                    }

                                    #endregion
                                }
                                else if (VB.Left(strOldData, 12) != VB.Left(strNewData, 12))
                                {
                                    #region Total_GelCode_Rtn

                                    //조합별 소계
                                    nRow += 1;
                                    if (nRow > ssView_Sheet1.RowCount)
                                    {
                                        ssView_Sheet1.RowCount = nRow;
                                    }
                                    ssView_Sheet1.Cells[nRow - 1, 3].Text = "* 소계 *";
                                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[2, 2].ToString("###,###,###,##0");
                                    ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotAmt[2, 8].ToString("###,###,###,##0");
                                    ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotAmt[2, 3].ToString("###,###,###,##0");
                                    ssView_Sheet1.Cells[nRow - 1, 13].Text = nTotAmt[2, 4].ToString("###,###,###,##0");
                                    ssView_Sheet1.Cells[nRow - 1, 15].Text = nTotAmt[2, 5].ToString("###,###,###,##0");
                                    ssView_Sheet1.Cells[nRow - 1, 16].Text = nTotAmt[2, 6].ToString("###,###,###,##0");
                                    ssView_Sheet1.Cells[nRow - 1, 17].Text = nTotAmt[2, 7].ToString("###,###,###,##0");
                                    ssView_Sheet1.Cells[nRow - 1, 18].Text = nTotAmt[2, 1].ToString("###,###,###,##0");

                                    for (k = 1; k < 9; k++)
                                    {
                                        nTotAmt[2, k] = 0;
                                    }

                                    #endregion
                                }
                            }

                            nRow += 1;
                            if (nRow > ssView_Sheet1.RowCount)
                            {
                                ssView_Sheet1.RowCount = nRow;
                            }

                            if (strNewData != strOldData)
                            {
                                strOldData = strNewData;
                            }

                            ssView_Sheet1.Cells[nRow - 1, 0].Text = VB.Right(strNewData, 10);
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = cpf.GET_BAS_MIA(clsDB.DbCon, VB.Mid(strNewData, 3, 8).Trim());
                            if (VB.Left(cboClass.Text, 2) == "01" || VB.Left(cboClass.Text, 2) == "02" || VB.Left(cboClass.Text, 2) == "03" ||
                                VB.Left(cboClass.Text, 2) == "04" || VB.Left(cboClass.Text, 2) == "05")
                            {
                                ssView_Sheet1.Cells[nRow - 1, 2].Text = cpm.READ_MisuClass(VB.Left(strNewData, 2));
                            }
                            else
                            {
                                ssView_Sheet1.Cells[nRow - 1, 2].Text = VB.Left(strNewData, 2);
                            }
                            ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = cpm.READ_MisuIpdOpd(dt.Rows[i]["IpdOpd"].ToString().Trim());
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = cpm.READ_MisuBunya(dt.Rows[i]["Bun"].ToString().Trim());

                            nTotAmt[1, 1] = VB.Val(dt.Rows[i]["Amt1"].ToString().Trim());
                            nTotAmt[1, 2] = VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());
                            nTotAmt[1, 3] = VB.Val(dt.Rows[i]["Amt3"].ToString().Trim());
                            nTotAmt[1, 4] = VB.Val(dt.Rows[i]["Amt4"].ToString().Trim());
                            nTotAmt[1, 5] = VB.Val(dt.Rows[i]["Amt5"].ToString().Trim());
                            nTotAmt[1, 6] = VB.Val(dt.Rows[i]["Amt6"].ToString().Trim());
                            nTotAmt[1, 7] = VB.Val(dt.Rows[i]["Amt7"].ToString().Trim());
                            nTotAmt[1, 8] = nTotAmt[1, 2] - nTotAmt[1, 3] - nTotAmt[1, 4];
                            nTotAmt[1, 8] = nTotAmt[1, 8] - nTotAmt[1, 5] - nTotAmt[1, 6] - nTotAmt[1, 7];

                            ssView_Sheet1.Cells[nRow - 1, 6].Text = nIlsu.ToString("##,##0");
                            ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dt.Rows[i]["Qty1"].ToString().Trim()).ToString("###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[1, 2].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotAmt[1, 8].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 10].Text = VB.Val(dt.Rows[i]["Qty2"].ToString().Trim()).ToString("###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotAmt[1, 3].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 12].Text = VB.Val(dt.Rows[i]["Qty3"].ToString().Trim()).ToString("###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 13].Text = nTotAmt[1, 4].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = VB.Val(dt.Rows[i]["Qty4"].ToString().Trim()).ToString("###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 15].Text = nTotAmt[1, 5].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 16].Text = nTotAmt[1, 6].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 17].Text = nTotAmt[1, 7].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 18].Text = nTotAmt[1, 1].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 19].Text = dt.Rows[i]["JepsuNo"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 20].Text = dt.Rows[i]["WRTNO"].ToString().Trim();

                            for (k = 1; k < 9; k++)
                            {
                                nTotAmt[2, k] += nTotAmt[1, k];
                                nTotAmt[3, k] += nTotAmt[1, k];
                                nTotAmt[4, k] += nTotAmt[1, k];
                            }
                        }
                    }
                    dt.Dispose();
                    dt = null;

                    #region Total_GelCode_Rtn

                    //조합별 소계
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = "* 소계 *";
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[2, 2].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotAmt[2, 8].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotAmt[2, 3].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = nTotAmt[2, 4].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 15].Text = nTotAmt[2, 5].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 16].Text = nTotAmt[2, 6].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 17].Text = nTotAmt[2, 7].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 18].Text = nTotAmt[2, 1].ToString("###,###,###,##0");

                    for (k = 1; k < 9; k++)
                    {
                        nTotAmt[2, k] = 0;
                    }

                    #endregion

                    if (VB.Left(cboClass.Text, 2) == "05")
                    {
                        #region Total_Class_Rtn

                        //미수종류별 소계
                        nRow += 1;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = "** 종류별계 **";
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[3, 2].ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotAmt[3, 8].ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotAmt[3, 3].ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 13].Text = nTotAmt[3, 4].ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 15].Text = nTotAmt[3, 5].ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = nTotAmt[3, 6].ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 17].Text = nTotAmt[3, 7].ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 18].Text = nTotAmt[3, 1].ToString("###,###,###,##0");

                        for (k = 1; k < 9; k++)
                        {
                            nTotAmt[3, k] = 0;
                        }

                        #endregion
                    }

                    #region Total_TOT_Rtn

                    //전체합계
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = "* 전체합계 *";
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[4, 2].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotAmt[4, 8].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotAmt[4, 3].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = nTotAmt[4, 4].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 15].Text = nTotAmt[4, 5].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 16].Text = nTotAmt[4, 6].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 17].Text = nTotAmt[4, 7].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 18].Text = nTotAmt[4, 1].ToString("###,###,###,##0");

                    for (k = 1; k < 9; k++)
                    {
                        nTotAmt[4, k] = 0;
                    }

                    #endregion

                    #endregion
                }
                else if (rdoSort1.Checked == true)
                {
                    #region Data_Display2

                    //발생일자별 미수 상세내역 Display
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT WRTNO, Class, GelCode,";
                    SQL = SQL + ComNum.VBLF + "        TO_CHAR(Bdate,'YYYY-MM-DD') Bdate, MisuID, IpdOpd,";
                    SQL = SQL + ComNum.VBLF + "        Bun, JepsuNo, Amt1,";
                    SQL = SQL + ComNum.VBLF + "        Amt2, Amt3, Amt4,";
                    SQL = SQL + ComNum.VBLF + "        Amt5, Amt6, Amt7,";
                    SQL = SQL + ComNum.VBLF + "        Qty1, Qty2, Qty3,";
                    SQL = SQL + ComNum.VBLF + "        Qty4";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_IDMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE GbEnd = '1'";
                    if (VB.Left(cboClass.Text, 2) == "05")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Class < '05'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Class='" + VB.Left(cboClass.Text, 2) + "'";
                    }
                    SQL = SQL + ComNum.VBLF + "  ORDER BY Bdate,Class,GelCode,MisuID,IpdOpd,Bun";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        btnSearch.Enabled = true;
                        Cursor.Current = Cursors.Default;
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        return;
                    }

                    strOldData = VB.Space(10);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nIlsu = (int)VB.DateDiff("D", Convert.ToDateTime(dt.Rows[i]["Bdate"].ToString().Trim()), Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")));
                        if (nIlsu >= VB.Val(txtIlsu.Text))
                        {
                            strNewData = dt.Rows[i]["Bdate"].ToString().Trim();

                            nRow += 1;
                            if (nRow > ssView_Sheet1.RowCount)
                            {
                                ssView_Sheet1.RowCount = nRow;
                            }

                            if (strNewData != strOldData)
                            {
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = strNewData;
                                strOldData = strNewData;
                            }

                            ssView_Sheet1.Cells[nRow - 1, 1].Text = cpf.GET_BAS_MIA(clsDB.DbCon, dt.Rows[i]["GelCode"].ToString().Trim());
                            if (VB.Left(cboClass.Text, 2) == "01" || VB.Left(cboClass.Text, 2) == "02" || VB.Left(cboClass.Text, 2) == "03" ||
                                VB.Left(cboClass.Text, 2) == "04" || VB.Left(cboClass.Text, 2) == "05")
                            {
                                ssView_Sheet1.Cells[nRow - 1, 2].Text = cpm.READ_MisuClass(dt.Rows[i]["Class"].ToString().Trim());
                            }
                            else
                            {
                                ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Class"].ToString().Trim();
                            }
                            ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = cpm.READ_MisuIpdOpd(dt.Rows[i]["IpdOpd"].ToString().Trim());
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = cpm.READ_MisuBunya(dt.Rows[i]["Bun"].ToString().Trim());

                            nTotAmt[1, 1] = VB.Val(dt.Rows[i]["Amt1"].ToString().Trim());
                            nTotAmt[1, 2] = VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());
                            nTotAmt[1, 3] = VB.Val(dt.Rows[i]["Amt3"].ToString().Trim());
                            nTotAmt[1, 4] = VB.Val(dt.Rows[i]["Amt4"].ToString().Trim());
                            nTotAmt[1, 5] = VB.Val(dt.Rows[i]["Amt5"].ToString().Trim());
                            nTotAmt[1, 6] = VB.Val(dt.Rows[i]["Amt6"].ToString().Trim());
                            nTotAmt[1, 7] = VB.Val(dt.Rows[i]["Amt7"].ToString().Trim());
                            nTotAmt[1, 8] = nTotAmt[1, 2] - nTotAmt[1, 3] - nTotAmt[1, 4];
                            nTotAmt[1, 8] = nTotAmt[1, 8] - nTotAmt[1, 5] - nTotAmt[1, 6] - nTotAmt[1, 7];

                            ssView_Sheet1.Cells[nRow - 1, 6].Text = nIlsu.ToString("##,##0");
                            ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dt.Rows[i]["Qty1"].ToString().Trim()).ToString("###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[1, 2].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotAmt[1, 8].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 10].Text = VB.Val(dt.Rows[i]["Qty2"].ToString().Trim()).ToString("###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotAmt[1, 3].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 12].Text = VB.Val(dt.Rows[i]["Qty3"].ToString().Trim()).ToString("###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 13].Text = nTotAmt[1, 4].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = VB.Val(dt.Rows[i]["Qty4"].ToString().Trim()).ToString("###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 15].Text = nTotAmt[1, 5].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 16].Text = nTotAmt[1, 6].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 17].Text = nTotAmt[1, 7].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 18].Text = nTotAmt[1, 1].ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 19].Text = dt.Rows[i]["JepsuNo"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 20].Text = dt.Rows[i]["WRTNO"].ToString().Trim();

                            for (k = 1; k < 9; k++)
                            {
                                nTotAmt[4, k] += nTotAmt[1, k];
                            }
                        }
                    }
                    dt.Dispose();
                    dt = null;

                    #region Total_TOT_Rtn

                    //전체합계
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = "* 전체합계 *";
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[4, 2].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotAmt[4, 8].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotAmt[4, 3].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = nTotAmt[4, 4].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 15].Text = nTotAmt[4, 5].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 16].Text = nTotAmt[4, 6].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 17].Text = nTotAmt[4, 7].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 18].Text = nTotAmt[4, 1].ToString("###,###,###,##0");

                    for (k = 1; k < 9; k++)
                    {
                        nTotAmt[4, k] = 0;
                    }

                    #endregion

                    #endregion
                }

                btnPrint.Enabled = true;
                btnSearch.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
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
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = VB.Now().ToString("yyyy-MM-dd").ToString()  + "일 현재 미수자 명부";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("미수종류:" + cboClass.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName + " 인 ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.8f);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void frmPmpaViewNowMisuList_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView_Sheet1.Columns[20].Visible = false;
            txtIlsu.Text = "30";

            cboClass.Items.Clear();
            cboClass.Items.Add("01.공단");
            cboClass.Items.Add("02.직장");
            cboClass.Items.Add("03.지역");
            cboClass.Items.Add("04.보호");
            cboClass.Items.Add("05.전체");
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

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            GnWRTNO = VB.Val(ssView_Sheet1.Cells[e.Row, 20].Text);
            //TODO:폼 호출
            //FrmMisuidView.Show
        }

        private void txtIlsu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }
    }
}
