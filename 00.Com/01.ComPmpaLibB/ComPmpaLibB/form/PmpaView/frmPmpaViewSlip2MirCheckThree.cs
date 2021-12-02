using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB.form.PmpaView
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
    /// <seealso cref= D:\psmh\misu\misubs\misubs75.frm\" >> frmPmpaViewSlip2MirCheckThree.cs 폼이름 재정의" />

    public partial class frmPmpaViewSlip2MirCheckThree : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string GstrWrtno = "";
        string GstrPano = "";
        string GstrSname = "";
        string GstrYYMM = "";
        string GstrIO = "";


        int nRow = 0;
        string strPano = "";
        //string strActDate = "";
        string strSname = "";
        //string strDeptCode = "";
        string strBi = "";
        string strInDate = "";
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
        //string strSdate = "";
        //string strEDATE = "";
        //string strFlag = "";
        string strActdate = "";
        string strYYMM = "";
        double nRate = 0;
        string strGbMir = "";
        string strIpdOpd = "";
        string strFDate = "";
        string strTdate = "";


        public frmPmpaViewSlip2MirCheckThree(string strGstrWrtno, string strGstrPano, string strGstrSname, string strGstrYYMM, string strGstrIO)
        {
            GstrPano = strGstrPano;
            GstrWrtno = strGstrWrtno;
            GstrSname = strGstrSname;
            GstrYYMM = strGstrYYMM;
            GstrIO = strGstrIO;

            InitializeComponent();
        }

        public frmPmpaViewSlip2MirCheckThree()
        {
            InitializeComponent();
        }

        private void frmPmpaViewSlip2MirCheckThree_Load(object sender, EventArgs e)
        {
            lblItem0.Text = "";
            Screen_Display();
        }

        private void Screen_Display()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            nWRTNO = VB.Val(VB.Pstr(clsPublic.GstrRetValue, ",", 1)).ToString();
            strActdate = VB.Pstr(clsPublic.GstrRetValue, ",", 2);
            strPano = VB.Pstr(clsPublic.GstrRetValue, ",", 3);
            strSname = VB.Pstr(clsPublic.GstrRetValue, ",", 4);
            strBi = VB.Pstr(clsPublic.GstrRetValue, ",", 5);
            strInDate = VB.Pstr(clsPublic.GstrRetValue, ",", 6);
            strYYMM = VB.Pstr(clsPublic.GstrRetValue, ",", 7);
            strIpdOpd = VB.Pstr(clsPublic.GstrRetValue, ",", 8);
            strGbMir = VB.Pstr(clsPublic.GstrRetValue, ",", 9);

            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTdate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);

            lblItem0.Text = "등록번호:" + GstrPano + " 성명:" + strSname;

            if (strIpdOpd == "I")
            {
                lblItem0.Text = lblItem0.Text + " 퇴원수납일:" + strActdate + " 입원일:" + strInDate;
            }
            lblItem0.Text = lblItem0.Text + " 청구월:" + strYYMM + " 청구번호:" + nWRTNO;

            nTotAmt1 = 0;
            nTotAmt2 = 0;

            if (strIpdOpd == "I")
                nRate = Convert.ToDouble(CPF.READ_BonRate(clsDB.DbCon, "IPD", strBi, VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01"));
            else
                nRate = Convert.ToDouble(CPF.READ_BonRate(clsDB.DbCon, "OPD", strBi, VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01"));

            nRate = (100 - nRate) / 100;

            try
            {
                if (strBi == "31")
                {
                    //GoSub Sanje_Display
                    strEdiOK = "NO";
                    SQL = "";
                    SQL = "SELECT EdiTAmt ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_SANID ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1=1 ";
                    SQL = SQL + ComNum.VBLF + "    AND PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND YYMM = '" + strYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "    AND IPDOPD ='" + strIpdOpd + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND (EDIGBN='0' OR EDIGBN IS NULL)  ";

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
                    SQL = SQL + ComNum.VBLF + " WHERE b.PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND b.YYMM = '" + strYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "   AND b.IPDOPD ='" + strIpdOpd + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND b.WRTNO = a.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND (B.EDIGBN='0' OR B.EDIGBN IS NULL)  ";
                    SQL = SQL + ComNum.VBLF + "   AND a.SuNext=c.SuNext(+) ";

                    if (strEdiOK == "OK")
                        SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,c.SuNameK ";
                    else
                        SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,c.SuNameK ";

                    SQL = SQL + ComNum.VBLF + "UNION ALL ";

                    if (strIpdOpd == "I")
                    {


                        //'퇴원청구
                        if (strActdate != "")
                        {
                            SQL = SQL + ComNum.VBLF + "SELECT a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt ";
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                            SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                            SQL = SQL + ComNum.VBLF + "    AND a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND a.ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Right(strYYMM, 4) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.GbSelf='0' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";
                            SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "SELECT a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt ";
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                            SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                            SQL = SQL + ComNum.VBLF + "   AND a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Right(strYYMM, 4) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.GbSelf='0' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";
                            SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";
                            SQL = SQL + ComNum.VBLF + "UNION ALL ";
                            SQL = SQL + ComNum.VBLF + "SELECT a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt ";
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                            SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                            SQL = SQL + ComNum.VBLF + "   AND a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Right(strYYMM, 4) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.GbSelf='0' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.SuNext=b.SuNext(+) ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "SELECT a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                        SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                        SQL = SQL + ComNum.VBLF + "   AND a.Pano='" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND a.ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND a.ACTDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND (a.YYMM = '" + VB.Right(strYYMM, 4) + "' OR A.NU ='01') ";
                        SQL = SQL + ComNum.VBLF + "   AND a.GbSelf='0' ";
                        SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";
                        SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";
                        SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";
                    }
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

                            //처방발생
                            if (dt.Rows[i]["Gubun"].ToString().Trim() == "2")
                            {
                                nQty1 = Convert.ToDouble(nQty1 + dt.Rows[i]["Qty"].ToString().Trim());
                                nAmt1 = Convert.ToDouble(nAmt1 + dt.Rows[i]["Amt"].ToString().Trim());
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

                        if (nRate != 0)
                        {
                            SS1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nTotAmt1 * nRate, "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(VB.Fix((int)(nTotAmt2 * nRate) * 10), "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(VB.Fix((int)(((nTotAmt2 * nRate) * 10) - (nTotAmt1 * nRate))), "###,###,###,##0 ");
                        }
                        else
                        {
                            SS1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nTotAmt1, "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10, "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10 - nTotAmt1, "###,###,###,##0 ");
                        }

                        SS1_Sheet1.Cells[nRow - 1, 6].Text = "** 합 계 **";

                        dt.Dispose();
                        dt = null;
                    }

                }
                else if (strBi == "52")
                {
                    //'자료를 SELECT
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SUM(DECODE(a.GbGisul,'1',(a.Amt*(100+b.RateGasan)/100),a.Amt)) Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_TADTL a," + ComNum.DB_PMPA + "MIR_TAID b," + ComNum.DB_PMPA + "BAS_SUN c ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "   AND b.PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND b.IPDOPD ='" + strIpdOpd + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND b.YYMM = '" + strYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "   AND B.MISUNO IS NOT NULL";
                    SQL = SQL + ComNum.VBLF + "   AND b.WRTNO = a.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND a.SuNext=c.SuNext(+) ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY a.SuNext,c.SuNameK  ";
                    SQL = SQL + ComNum.VBLF + " UNION ALL ";

                    if (strIpdOpd == "I")
                    {



                        if (strActdate != "")// '퇴원청구
                        {
                            SQL = SQL + ComNum.VBLF + "SELECT a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty+a.Nal) Amt,SUM(a.Amt1) Amt ";
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                            SQL = SQL + ComNum.VBLF + " WHERE a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.ActDate >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND a.ActDate <=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Right(strYYMM, 4) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND (a.Nu < '21' OR (a.Nu > '35' AND a.Nu < '41')) ";
                            SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "SELECT a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt ";
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                            SQL = SQL + ComNum.VBLF + " WHERE a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Right(strYYMM, 4) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND (a.Nu < '21' OR (a.Nu > '35' AND a.Nu < '41')) ";
                            SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";
                            SQL = SQL + ComNum.VBLF + "UNION ALL ";
                            SQL = SQL + ComNum.VBLF + "SELECT a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt ";
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                            SQL = SQL + ComNum.VBLF + " WHERE a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Right(strYYMM, 4) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND (a.Nu < '21' OR (a.Nu > '35' AND a.Nu < '41')) ";
                            SQL = SQL + ComNum.VBLF + "   AND a.SuNext=b.SuNext(+) ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt ";
                        SQL = SQL + ComNum.VBLF + "   FROM OPD_SLIP a,BAS_SUN b ";
                        SQL = SQL + ComNum.VBLF + "  WHERE a.Pano='" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND a.ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "    AND a.ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "    AND a.Bi='" + strBi + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND (a.YYMM = '" + VB.Right(strYYMM, 4) + "' OR a.NU='01') ";
                        SQL = SQL + ComNum.VBLF + "    AND a.GbSelf='0' ";
                        SQL = SQL + ComNum.VBLF + "    AND a.Nu < '21' ";
                        SQL = SQL + ComNum.VBLF + "    AND a.SuNext=b.SuNext(+) ";
                        SQL = SQL + ComNum.VBLF + "  GROUP BY a.SuNext,b.SuNameK ";
                    }
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

                        if (nRate != 0)
                        {
                            SS1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nTotAmt1 * nRate, "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(VB.Fix((int)(nTotAmt2 * nRate) * 10), "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(VB.Fix((int)(((nTotAmt2 * nRate) * 10) - (nTotAmt1 * nRate))), "###,###,###,##0 ");
                        }
                        else
                        {
                            SS1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nTotAmt1, "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10, "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10 - nTotAmt1, "###,###,###,##0 ");
                        }

                        SS1_Sheet1.Cells[nRow - 1, 6].Text = "** 합 계 **";

                        dt.Dispose();
                        dt = null;
                    }

                }
                else
                //GoSub Bohum_Display
                {
                    strEdiOK = "NO";
                    SQL = "";
                    SQL = "SELECT EdiTAmt, BOHUN, RATEBON ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.VBLF + "MIR_INSID ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND YYMM = '" + strYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "   AND IPDOPD ='" + strIpdOpd + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND BI ='" + strBi + "' ";
                    SQL = SQL + ComNum.VBLF + " AND EDIMIRNO > 0 ";
                    SQL = SQL + ComNum.VBLF + "  AND (GBMIR ='" + strGbMir + "' OR GBMIR IS NULL )  ";

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

                        if (dt.Rows[0]["bohun"].ToString().Trim() == "3")
                        {
                            nRate = 0;
                        }
                        else
                        {
                            nRate = Convert.ToDouble(dt.Rows[0]["RATEBON"].ToString().Trim());
                            nRate = (100 - nRate) / 100;
                        }

                        dt.Dispose();
                        dt = null;
                    }

                    if (strEdiOK == "OK")
                    {
                        //'자료를 SELECT
                        if (strEdiOK == "OK")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "SELECT a.SuNext,c.SuNameK,'1' Gubun,SUM(a.EdiQty*a.EdiNal) Qty,";
                            SQL = SQL + ComNum.VBLF + "SUM(DECODE(a.GbGisul,'1',(a.EdiAmt*(100+b.RateGasan)/100),a.EdiAmt)) Amt, b.RateBon, B.BOHUN ";
                        }
                        else
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "SELECT a.SuNext,c.SuNameK,'1' Gubun,SUM(a.Qty*a.Nal) Qty,";
                            SQL = SQL + ComNum.VBLF + "SUM(DECODE(a.GbGisul,'1',(a.Amt*(100+b.RateGasan)/100),a.Amt)) Amt, b.RateBon , B.BOHUN  ";
                        }
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_INSDTL a," + ComNum.DB_PMPA + "MIR_INSID b," + ComNum.DB_PMPA + "BAS_SUN c ";
                        SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                        SQL = SQL + ComNum.VBLF + "   AND B.PANO ='" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND B.YYMM = '" + strYYMM + "'  ";
                        SQL = SQL + ComNum.VBLF + "   AND B.IPDOPD ='" + strIpdOpd + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND B.BI ='" + strBi + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND B.EDIMIRNO <>'0' ";
                        SQL = SQL + ComNum.VBLF + "   AND (B.GBMIR ='" + strGbMir + "' OR B.GBMIR IS NULL )  ";
                        SQL = SQL + ComNum.VBLF + "  AND B.WRTNO = A.WRTNO(+) ";
                        SQL = SQL + ComNum.VBLF + "  AND A.SuNext=c.SuNext(+) ";

                        if (strEdiOK == "OK")
                        {
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,c.SuNameK,b.RateBon, B.BOHUN ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,c.SuNameK,b.RateBon, B.BOHUN ";
                        }
                        SQL = SQL + ComNum.VBLF + "UNION ALL  ";
                    }
                    else
                    {
                        SQL = "";
                    }


                    //TODO
                    //If strIpdOpd = "I" And FrmIpdMirCheckUpdate2.OptMir(3).Value = False Then
                    if (strIpdOpd == "I")  //이것 아님. 윗 소스
                    {


                        if (strActdate != "")// '퇴원청구
                        {
                            SQL = SQL + ComNum.VBLF + "SELECT a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt, 0 RateBon, '0 ' BOHUN ";
                            SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_SLIP a,BAS_SUN b ";
                            SQL = SQL + ComNum.VBLF + " WHERE a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND a.ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Right(strYYMM, 4) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.GbSelf='0' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";
                            SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "SELECT a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt, 0 RateBon, '0 ' BOHUN ";
                            SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_SLIP a,BAS_SUN b ";
                            SQL = SQL + ComNum.VBLF + " WHERE a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Right(strYYMM, 4) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.GbSelf='0' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";
                            SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";

                            SQL = SQL + ComNum.VBLF + "UNION ALL ";

                            SQL = SQL + ComNum.VBLF + "SELECT a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt, 0 RateBon, '0 ' BOHUN ";
                            SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_SLIP a,BAS_SUN b ";
                            SQL = SQL + ComNum.VBLF + " WHERE a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Right(strYYMM, 4) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.GbSelf='0' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.SuNext=b.SuNext(+) ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "SELECT a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt, 0 RateBon, '0 ' BOHUN  ";
                        SQL = SQL + ComNum.VBLF + "  FROM OPD_SLIP a,BAS_SUN b ";
                        SQL = SQL + ComNum.VBLF + " WHERE a.Pano='" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND a.ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND a.ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND (a.YYMM = '" + VB.Right(strYYMM, 4) + "'  OR a.NU ='01' OR a.GbSelf ='0') ";// '조제료 때문에 조건을 or 로 gbself 체크함(jjy:2003-06-26)
                        SQL = SQL + ComNum.VBLF + "   AND a.amt1 <>'0' ";
                        SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";

                        //TODO
                        // If FrmIpdMirCheckUpdate2.OptMir(3).Value = True Then
                        if (1 == 1) //이것 아님 윗 소스
                            SQL = SQL + ComNum.VBLF + " AND a.GbSlip IN( 'Z','Q','E') ";//      '응급실 6시간이상
                        //else
                        //    SQL = SQL + ComNum.VBLF + " AND a.GbSlip NOT IN (Z','Q','E') ";

                        SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";
                        SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";

                    }
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

                            if (VB.Val(dt.Rows[i]["RATEBON"].ToString().Trim()) != 0)
                            {
                                if (dt.Rows[i]["BOHUN"].ToString().Trim() == "3")
                                {
                                    nRate = 0;
                                }
                                else
                                {
                                    nRate = Convert.ToDouble(dt.Rows[i]["RATEBON"].ToString().Trim());
                                    nRate = (100 - nRate) / 100;
                                }

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

                        if (nRate != 0)
                        {
                            SS1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nTotAmt1 * nRate, "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(VB.Fix((int)(nTotAmt2 * nRate) * 10), "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(VB.Fix((int)(((nTotAmt2 * nRate) * 10) - (nTotAmt1 * nRate))), "###,###,###,##0 ");
                        }
                        else
                        {
                            SS1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nTotAmt1, "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10, "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10 - nTotAmt1, "###,###,###,##0 ");
                        }

                        SS1_Sheet1.Cells[nRow - 1, 6].Text = "** 합 계 **";

                        dt.Dispose();
                        dt = null;
                    }
                }
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
            SS1_Sheet1.Cells[nRow - 1, 1].Text = VB.Format(nQty1, "###,###,##0.0");
            SS1_Sheet1.Cells[nRow - 1, 2].Text = VB.Format(nQty2, "###,###,##0.0");

            if (nRate != 0)
            {
                SS1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format((nAmt1 * nRate), "###,###,##0.0");
                SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format((nAmt2 * nRate), "###,###,##0.0");
                SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(((nAmt2 * nRate) - nAmt1), "###,###,###,###");
            }
            else
            {
                SS1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nAmt1, "###,###,##0.0");
                SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(nAmt2, "###,###,##0.0");
                SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(nAmt2 - nAmt1, "###,###,###,###");
            }

            SS1_Sheet1.Cells[nRow - 1, 6].Text = strSunameK;

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


    }
}
