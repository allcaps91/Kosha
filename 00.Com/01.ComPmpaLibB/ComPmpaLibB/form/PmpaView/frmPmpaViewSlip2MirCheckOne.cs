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
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-11-06
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\mir\miretc\miretc.vbp\miretc55.frm" >> frmPmpaViewSlip2MirCheckOne.cs 폼이름 재정의" />

    public partial class frmPmpaViewSlip2MirCheckOne : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string GstrWrtno = "";
        string GstrPano_1 = "";
        string GstrSname = "";
        string GstrYYMM = "";
        string GstrIO = "";


        int nRow = 0;
        //string strPano = "";
        //string strActDate = "";
        //string strSname = "";
        //string strDeptCode = "";
        string strBi = "";
        //string strInDate = "";
        //string StrYYMM = "";
        string nWRTNO = "";
        string strEdiOK = "";
        //int nRateGasan = 0;
        string strOldData = "";
        string strNewData = "";
        string strSuNext = "";
        string strSunameK = "";
        double nQty1 = 0;
        double nQty2 = 0;
        double nAmt1 = 0;
        double nAmt2 = 0;
        //double nChaAmt = 0;
        double nTotAmt1 = 0;
        double nTotAmt2 = 0;
        string strSdate = "";
        string strEDATE = "";
        string strFlag = "";

        public frmPmpaViewSlip2MirCheckOne()
        {
            InitializeComponent();
        }

        public frmPmpaViewSlip2MirCheckOne(string gstrRetValue)
        {
            this.gstrRetValue = gstrRetValue;
        }

        public frmPmpaViewSlip2MirCheckOne(string strGstrWrtno, string strGstrPano_1, string strGstrSname, string strGstrYYMM, string strGstrIO)
        {
            GstrPano_1 = strGstrPano_1;
            GstrWrtno = strGstrWrtno;
            GstrSname = strGstrSname;
            GstrYYMM = strGstrYYMM;
            GstrIO = strGstrIO;

            InitializeComponent();
        }


        private void frmPmpaViewSlip2MirCheckOne_Load(object sender, EventArgs e)
        {
            lblItem0.Text = "";
            Screen_Display();

        }

        private void Screen_Display()
        {
            int i = 0;
            int nREAD = 0;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            nWRTNO = VB.Val(GstrWrtno).ToString();

            lblItem0.Text = "등록번호:" + GstrPano_1 + " 성명:" + GstrSname;
            lblItem0.Text = lblItem0.Text + " 진료월:" + GstrYYMM + " 청구번호:" + nWRTNO;

            nTotAmt1 = 0;
            nTotAmt2 = 0;

            try
            {
                if (strBi == "31")
                {//GoSub Sanje_Display
                    strEdiOK = "NO";
                    SQL = "";
                    SQL = "SELECT EdiTAmt ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_SANID ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1=1 ";
                    SQL = SQL + ComNum.VBLF + "     AND WRTNO=" + nWRTNO + " ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (VB.Val(dt.Rows[0]["EdiTAmt"].ToString().Trim()) > 0)
                        {
                            strEdiOK = "OK";
                        }
                    }
                    dt.Dispose();
                    dt = null;

                    //'자료를 SELECT
                    if (strEdiOK == "OK")
                    {
                        SQL = "";
                        SQL = "SELECT a.SuNext,c.SuNameK,'1' Gubun,SUM(a.EdiQty*a.EdiNal) Qty,";
                        SQL = SQL + ComNum.VBLF + "SUM(DECODE(a.GbGisul,'1',(a.EdiAmt*(100+b.RateGasan)/100),a.EdiAmt)) Amt ";
                    }
                    else
                    {
                        SQL = "";
                        SQL = "SELECT a.SuNext,c.SuNameK,'1' Gubun,SUM(a.Qty*a.Nal) Qty,";
                        SQL = SQL + ComNum.VBLF + "SUM(DECODE(a.GbGisul,'1',(a.Amt*(100+b.RateGasan)/100),a.Amt)) Amt ";
                    }
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_SANDTL a," + ComNum.DB_PMPA + "MIR_SANID b," + ComNum.DB_PMPA + "BAS_SUN c ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "  AND a.WRTNO = " + nWRTNO + " ";
                    SQL = SQL + ComNum.VBLF + "  AND a.WRTNO = b.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "  AND a.SuNext=c.SuNext(+) ";

                    if (strEdiOK == "OK")
                        SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,c.SuNameK ";
                    else
                        SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,c.SuNameK ";

                    SQL = SQL + ComNum.VBLF + "ORDER BY 1,2 ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        nRow = 0;
                        strOldData = "";
                        strSuNext = "";
                        nQty1 = 0;
                        nQty2 = 0;
                        nAmt1 = 0;
                        nAmt2 = 0;
                        strSunameK = "";

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strNewData = dt.Rows[i]["SuNext"].ToString().Trim();

                            if (strOldData != strNewData)
                            {
                                strOldData = strNewData;
                                Display_SUB(ref nRow, nTotAmt1, nTotAmt2);
                                strSuNext = strNewData;
                                strSunameK = dt.Rows[i]["SuNameK"].ToString().Trim();
                                nQty1 = 0;
                                nQty2 = 0;
                                nAmt1 = 0;
                                nAmt2 = 0;
                            }

                        }
                        Display_SUB(ref nRow, nTotAmt1, nTotAmt2);

                        nRow = nRow + 1;
                        SS1_Sheet1.RowCount = nRow;
                        SS1_Sheet1.Cells[nRow - 1, 2].Text = VB.Format(nTotAmt2, "###,###,###,##0 ");
                        SS1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nTotAmt2 - nTotAmt1, "###,###,###,##0 ");
                        SS1_Sheet1.Cells[nRow - 1, 4].Text = "** 합 계 **";

                        dt.Dispose();
                        dt = null;
                    }

                }
                else if (strBi == "52")
                {
                    //'자료를 SELECT
                    SQL = "";
                    SQL = "SELECT a.SuNext,c.SuNameK,'1' Gubun,SUM(a.Qty*a.Nal) Qty,";
                    SQL = SQL + ComNum.VBLF + "SUM(DECODE(a.GbGisul,'1',(a.Amt*(100+b.RateGasan)/100),a.Amt)) Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_TADTL a," + ComNum.DB_PMPA + "MIR_TAID b," + ComNum.DB_PMPA + "BAS_SUN c ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "  AND a.WRTNO = " + nWRTNO + " ";
                    SQL = SQL + ComNum.VBLF + "  AND a.WRTNO = b.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "  AND a.SuNext=c.SuNext(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,c.SuNameK ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY 1,2 ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    nRow = 0;
                    strOldData = "";
                    strSuNext = "";
                    nQty1 = 0;
                    nQty2 = 0;
                    nAmt1 = 0;
                    nAmt2 = 0;
                    strSunameK = "";

                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strNewData = dt.Rows[i]["SuNext"].ToString().Trim();

                            if (strOldData != strNewData)
                            {
                                strOldData = strNewData;
                                Display_SUB(ref nRow, nTotAmt1, nTotAmt2);
                                strSuNext = strNewData;
                                strSunameK = dt.Rows[i]["SuNameK"].ToString().Trim();
                                nQty1 = 0;
                                nQty2 = 0;
                                nAmt1 = 0;
                                nAmt2 = 0;
                            }
                            if (dt.Rows[i]["Gubun"].ToString().Trim() == "2")//'처방발생
                            {
                                nQty1 = nQty1 + Convert.ToDouble(dt.Rows[i]["Qty"].ToString().Trim());
                                nAmt1 = nAmt1 + Convert.ToDouble(dt.Rows[i]["Amt"].ToString().Trim());
                            }
                            else
                            {
                                nQty2 = nQty2 + Convert.ToDouble(dt.Rows[i]["Qty"].ToString().Trim());
                                nAmt2 = nAmt2 + Convert.ToDouble(dt.Rows[i]["Amt"].ToString().Trim());
                            }
                        }

                        Display_SUB(ref nRow, nTotAmt1, nTotAmt2);

                        nRow = nRow + 1;
                        SS1_Sheet1.RowCount = nRow;
                        SS1_Sheet1.Cells[nRow - 1, 2].Text = VB.Format(nTotAmt2, "###,###,###,##0 ");
                        SS1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nTotAmt2 - nTotAmt1, "###,###,###,##0 ");
                        SS1_Sheet1.Cells[nRow - 1, 4].Text = "** 합 계 **";

                        dt.Dispose();
                        dt = null;
                    }

                }
                else
                //GoSub Bohum_Display
                {
                    strEdiOK = "NO";
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT EdiTAmt";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_INSID ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "    AND WRTNO=" + nWRTNO + " ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (VB.Val(dt.Rows[0]["EdiTAmt"].ToString().Trim()) > 0)
                        {
                            strEdiOK = "OK";
                        }
                        dt.Dispose();
                        dt = null;
                    }

                    //'자료를 SELECT
                    if (strEdiOK == "OK")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT a.SuNext,c.SuNameK,'1' Gubun,SUM(a.EdiQty*a.EdiNal) Qty,";
                        SQL = SQL + ComNum.VBLF + "SUM(DECODE(a.GbGisul,'1',(a.EdiAmt*(100+b.RateGasan)/100),a.EdiAmt)) Amt ";
                    }
                    else
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT a.SuNext,c.SuNameK,'1' Gubun,SUM(a.Qty*a.Nal) Qty,";
                        SQL = SQL + ComNum.VBLF + "SUM(DECODE(a.GbGisul,'1',(a.Amt*(100+b.RateGasan)/100),a.Amt)) Amt ";
                    }
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_INSDTL a," + ComNum.DB_PMPA + "MIR_INSID b," + ComNum.DB_PMPA + "BAS_SUN c ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "  AND a.WRTNO = " + nWRTNO + " ";
                    SQL = SQL + ComNum.VBLF + "  AND a.WRTNO = b.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "  AND a.SuNext=c.SuNext(+) ";

                    if (strEdiOK == "OK")
                    {
                        SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,c.SuNameK ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,c.SuNameK ";
                    }
                    SQL = SQL + ComNum.VBLF + "ORDER BY 1,2 ";

                    nRow = 0;
                    strOldData = "";
                    strSuNext = "";
                    nQty1 = 0;
                    nQty2 = 0;
                    nAmt1 = 0;
                    nAmt2 = 0;
                    strSunameK = "";

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strNewData = dt.Rows[i]["SuNext"].ToString().Trim();

                        if (strOldData != strNewData)
                        {

                            strOldData = strNewData;
                            Display_SUB(ref nRow, nTotAmt1, nTotAmt2);
                            strSuNext = strNewData;
                            strSunameK = dt.Rows[i]["SuNameK"].ToString().Trim();
                            nQty1 = 0;
                            nQty2 = 0;
                            nAmt1 = 0;
                            nAmt2 = 0;
                        }
                        if (dt.Rows[i]["Gubun"].ToString().Trim() == "2")//'처방발생
                        {
                            nQty1 = nQty1 + Convert.ToDouble(dt.Rows[i]["Qty"].ToString().Trim());
                            nAmt1 = nAmt1 + Convert.ToDouble(dt.Rows[i]["Amt"].ToString().Trim());
                        }
                        else
                        {
                            nQty2 = nQty2 + Convert.ToDouble(dt.Rows[i]["Qty"].ToString().Trim());
                            nAmt2 = nAmt2 + Convert.ToDouble(dt.Rows[i]["Amt"].ToString().Trim());
                        }
                    }
                    Display_SUB(ref nRow, nTotAmt1, nTotAmt2);

                    nRow = nRow + 1;
                    SS1_Sheet1.RowCount = nRow;
                    SS1_Sheet1.Cells[nRow - 1, 2].Text = VB.Format(nTotAmt2, "###,###,###,##0 ");
                    SS1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nTotAmt2 - nTotAmt1, "###,###,###,##0 ");
                    SS1_Sheet1.Cells[nRow - 1, 4].Text = "** 합 계 **";

                    dt.Dispose();
                    dt = null;
                }

                if (GstrIO != "O")
                {
                    return;
                }

                strSdate = VB.Left(GstrYYMM, 4) + "-" + VB.Mid(GstrYYMM, 1, 5) + "-01";
                strEDATE = CF.READ_LASTDAY(clsDB.DbCon, strSdate);
                SQL = "";
                SQL = SQL + ComNum.DB_PMPA + " SELECT TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, A.SUNEXT, A.QTY, ";
                SQL = SQL + ComNum.DB_PMPA + "  A.NAL, A.GBSELF, A.BASEAMT, A.AMT1, A.BUN, B.SUNAMEK, ";
                SQL = SQL + ComNum.DB_PMPA + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE ";
                SQL = SQL + ComNum.DB_PMPA + "  FROM OPD_SLIP A, BAS_SUN B ";
                SQL = SQL + ComNum.DB_PMPA + " WHERE PANO = '" + GstrPano_1 + "' ";
                SQL = SQL + ComNum.DB_PMPA + "   AND ACTDATE >= TO_DATE('" + strSdate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.DB_PMPA + "   AND ACTDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.DB_PMPA + "   AND A.SUNEXT =B.SUNEXT";
                SQL = SQL + ComNum.DB_PMPA + " ORDER BY ACTDATE DESC , SEQNO, BUN,NU, SUNEXT";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS2_Sheet1.RowCount = 0;
                    SS2_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (strFlag != dt.Rows[i]["ACTDATE"].ToString().Trim())
                        {
                            strFlag = dt.Rows[i]["ACTDATE"].ToString().Trim();
                            SS2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                        }

                        SS2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["NAL"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 5].Text = VB.Format(dt.Rows[i]["BASEAMT"].ToString().Trim(), "###,###,##0 ");
                        SS2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GBSELF"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 7].Text = VB.Format(dt.Rows[i]["AMT1"].ToString().Trim(), "###,###,##0 ");
                        SS2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();

                        // Select Case AdoGetString(rs, "BUN", i)
                        //  Case Is > 90: SS2.Col = -1: SS2.BackColor = RGB(222, 222, 255)
                        //  Case Else:
                        //            If AdoGetNumber(rs, "GBSELF", i) > 0 Then
                        //              SS2.Col = -1: SS2.ForeColor = RGB(255, 0, 0)
                        //            End If
                        //End Select
                    }
                }
                dt.Dispose();
                dt = null;
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
            }
        }

        private void Display_SUB(ref int nRow, double nTotAmt1, double nTotAmt2)
        {
            if (nQty1 == 0 && nQty2 == 0 && nAmt1 == 0 && nAmt2 == 0)
            {
                return;
            }

            nRow = nRow + 1;
            if (nRow > SS1_Sheet1.RowCount)
            {
                SS1_Sheet1.RowCount = nRow;
            }

            SS1_Sheet1.Cells[nRow - 1, 0].Text = strSuNext;
            SS1_Sheet1.Cells[nRow - 1, 1].Text = VB.Format(nQty2, "###,###,###,##0 ");
            SS1_Sheet1.Cells[nRow - 1, 2].Text = VB.Format(nAmt2, "###,###,###,##0 ");
            SS1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nAmt2 - nAmt1, "###,###,###,### ");
            SS1_Sheet1.Cells[nRow - 1, 4].Text = strSunameK;

            nTotAmt1 = nTotAmt1 + nAmt1;
            nTotAmt2 = nTotAmt2 + nAmt2;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
