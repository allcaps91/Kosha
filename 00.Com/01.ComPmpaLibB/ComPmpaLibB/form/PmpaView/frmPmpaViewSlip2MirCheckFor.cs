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
    /// <seealso cref= D:\psmh\misu\misubs\misubs77.frm\" >> frmPmpaViewSlip2MirCheckThree.cs 폼이름 재정의" />

    public partial class frmPmpaViewSlip2MirCheckFor : Form
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
        string strDeptCode = "";
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
        string strGbMir = "";
        string strIpdOpd = "";
        string strFDate = "";
        string strTdate = "";
        string strBDate = "";
        double nRate_I = 0;
        double nRate_O = 0;
        double nGaSan = 0;
        double nRate = 0;
        string strMirOutdate = "";
        string strMirJindate = "";
        string strDeptOld = "";
        double nSTotAmt1 = 0;
        double nSTotAmt2 = 0;
        string strGubun = "";
        string strBun = "";
        string strGbGisul = "";
        string strGbChild = "";
        string strRateBon = "";
        string strBohun = "";
        double nAmt = 0;
        double nDrugAmt = 0;
        string strGbSlip = "";
        string strVCode = "";
        double nJohapAmt = 0;
        double nAmt3 = 0;
        double NAMT11 = 0;
        double NAMTGASAN = 0;
        double nBonAmt = 0;
        double NAMT12 = 0;


        public frmPmpaViewSlip2MirCheckFor()
        {
            InitializeComponent();
        }

        public frmPmpaViewSlip2MirCheckFor(string strGstrWrtno, string strGstrPano, string strGstrSname, string strGstrYYMM, string strGstrIO)
        {
            GstrPano = strGstrPano;
            GstrWrtno = strGstrWrtno;
            GstrSname = strGstrSname;
            GstrYYMM = strGstrYYMM;
            GstrIO = strGstrIO;

            InitializeComponent();
        }

        private void frmPmpaViewSlip2MirCheckFor_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            lblItem0.Text = "";
            Screen_Display();
        }

        private void Screen_Display()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;

            nWRTNO = VB.Val(VB.Pstr(clsPublic.GstrRetValue, ",", 1)).ToString();
            strActdate = VB.Pstr(clsPublic.GstrRetValue, ",", 2);
            strPano = VB.Pstr(clsPublic.GstrRetValue, ",", 3);
            strSname = VB.Pstr(clsPublic.GstrRetValue, ",", 4);
            strBi = VB.Pstr(clsPublic.GstrRetValue, ",", 5);
            strInDate = VB.Pstr(clsPublic.GstrRetValue, ",", 6);
            strYYMM = VB.Pstr(clsPublic.GstrRetValue, ",", 7);
            strIpdOpd = VB.Pstr(clsPublic.GstrRetValue, ",", 8);
            strGbMir = VB.Pstr(clsPublic.GstrRetValue, ",", 9);
            strBDate = VB.Pstr(clsPublic.GstrRetValue, ",", 10);

            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTdate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);

            lblItem0.Text = "등록번호:" + GstrPano + " 성명:" + strSname;

            if (strIpdOpd == "I")
            {
                lblItem0.Text = lblItem0.Text + " 퇴원수납일:" + strActdate + " 입원일:" + strInDate;
            }
            lblItem0.Text = lblItem0.Text + " 청구월:" + strYYMM + " 청구번호:" + nWRTNO;

            if (strBDate != "")
            {
                lblItem0.Text = lblItem0.Text + " 처방일:" + strBDate;
            }

            nTotAmt1 = 0;
            nTotAmt2 = 0;

            nRate_I = Convert.ToDouble(CPF.READ_BonRate(clsDB.DbCon, "IPD", strBi, VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01"));
            nRate_O = Convert.ToDouble(CPF.READ_BonRate(clsDB.DbCon, "OPD", strBi, VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01"));
            nGaSan = Convert.ToDouble(CPF.READ_RateGasan(clsDB.DbCon, strBi, VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01"));

            clsDB.setBeginTran(clsDB.DbCon);

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
                    SQL = SQL + ComNum.VBLF + "    AND GBNEDI ='0'   ";

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

                    SQL = " CREATE OR REPLACE VIEW  VIEW_MIR_DETAIL AS ";
                    if (strEdiOK == "OK")
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT '' deptcode, A.SUNEXT SORT, a.SuNext,c.SuNameK,'1' Gubun,SUM(a.EdiQty*a.EdiNal) Qty,";
                        SQL = SQL + ComNum.VBLF + "SUM(DECODE(a.GbGisul,'1',(a.EdiAmt*(100+b.RateGasan)/100),a.EdiAmt)) Amt ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT '' deptcode, A.SUNEXT SORT, a.SuNext,c.SuNameK,'1' Gubun,SUM(a.Qty*a.Nal) Qty,";
                        SQL = SQL + ComNum.VBLF + "SUM(DECODE(a.GbGisul,'1',(a.Amt*(100+b.RateGasan)/100),a.Amt)) Amt ";
                    }
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_SANDTL a," + ComNum.DB_PMPA + "MIR_SANID b," + ComNum.DB_PMPA + "BAS_SUN c ";
                    SQL = SQL + ComNum.VBLF + "   WHERE B.WRTNO IN (    ";
                    SQL = SQL + ComNum.VBLF + "  SELECT  A.WRTNO";
                    SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "MIR_SANID A, " + ComNum.DB_PMPA + "MISU_IDMST B ";
                    SQL = SQL + ComNum.VBLF + "   WHERE A.PANO = B.MISUID ";
                    SQL = SQL + ComNum.VBLF + "     AND A.FRDATE = B.FROMDATE ";
                    SQL = SQL + ComNum.VBLF + "     AND A.TODATE = B.TODATE ";
                    SQL = SQL + ComNum.VBLF + "     AND B.MISUID ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND B.MIRYYMM = '" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND B.TONGGBN IN ('1','2') ";
                    SQL = SQL + ComNum.VBLF + "     AND B.IpdOpd = '" + strIpdOpd + "'";
                    SQL = SQL + ComNum.VBLF + "     AND B.CLASS IN ('05') ";//  '산재
                    SQL = SQL + ComNum.VBLF + "       )";
                    SQL = SQL + ComNum.VBLF + "   AND b.WRTNO = a.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND a.SuNext=c.SuNext(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND (b.MIRGBN ='" + strGbMir + "' OR b.MIRGBN IS NULL )  ";

                    if (strEdiOK == "OK")
                        SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,c.SuNameK ";
                    else
                        SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,c.SuNameK ";

                    SQL = SQL + ComNum.VBLF + "UNION ALL ";

                    //'약제 상한 발생
                    SQL = SQL + ComNum.VBLF + "SELECT '' deptcode,  '#' SORT, 'BBBBBB' , '약가상한차액', '1' Gubun,  1 QTY , ";
                    SQL = SQL + ComNum.VBLF + "SUM(A.EDIDRUGAMT)  AMT  ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_SANDTL a," + ComNum.DB_PMPA + "MIR_SANID b," + ComNum.DB_PMPA + "BAS_SUN c ";

                    SQL = SQL + ComNum.VBLF + "   WHERE B.WRTNO IN (    ";
                    SQL = SQL + ComNum.VBLF + "  SELECT  A.WRTNO";
                    SQL = SQL + ComNum.VBLF + "    FROM MIR_SANID A, MISU_IDMST B ";
                    SQL = SQL + ComNum.VBLF + "   WHERE A.PANO = B.MISUID ";
                    SQL = SQL + ComNum.VBLF + "     AND A.FRDATE = B.FROMDATE ";
                    SQL = SQL + ComNum.VBLF + "     AND A.TODATE = B.TODATE ";
                    SQL = SQL + ComNum.VBLF + "     AND B.MISUID ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND B.MIRYYMM = '" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND B.TONGGBN IN ('1','2') ";
                    SQL = SQL + ComNum.VBLF + "     AND B.IpdOpd = '" + strIpdOpd + "'";
                    SQL = SQL + ComNum.VBLF + "     AND B.CLASS IN ('05') ";//  '산재
                    SQL = SQL + ComNum.VBLF + "       )";
                    SQL = SQL + ComNum.VBLF + "   AND b.WRTNO = a.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND a.SuNext=c.SuNext(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND (b.MIRGBN ='" + strGbMir + "' OR b.MIRGBN IS NULL )  ";
                    SQL = SQL + ComNum.VBLF + "  AND A.EDIDRUGAMT <> 0 ";
                    SQL = SQL + ComNum.VBLF + "UNION ALL ";

                    if (strIpdOpd == "I")
                    {
                        //'퇴원청구
                        if (strGbMir == "1")
                        {
                            SQL = SQL + ComNum.VBLF + "SELECT '' deptcode,  A.SUNEXT SORT, a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt ";
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b, " + ComNum.DB_PMPA + "IPD_trans C ";
                            SQL = SQL + ComNum.VBLF + " WHERE C.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND C.ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND C.ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Right(strYYMM, 4) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.GbSelf='0' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.SuNext=b.SuNext(+) ";
                            SQL = SQL + ComNum.VBLF + "   AND A.TRSNO = C.TRSNO ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "SELECT  '' deptcode, A.SUNEXT SORT, a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt ";
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                            SQL = SQL + ComNum.VBLF + " WHERE a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Right(strYYMM, 4) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.GbSelf='0' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.BDATE >= TO_DATE('" + strInDate + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "   AND a.SuNext=b.SuNext(+) ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "SELECT  '' deptcode, A.SUNEXT SORT, a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                        SQL = SQL + ComNum.VBLF + " WHERE a.Pano='" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND a.ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND a.ACTDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";

                        if (strIpdOpd == "O")
                        {

                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "   AND (a.YYMM = '" + VB.Right(strYYMM, 4) + "' OR A.NU ='01') ";
                        }

                        SQL = SQL + ComNum.VBLF + "   AND a.GbSelf='0' ";
                        SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";
                        SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";
                        SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = " SELECT  deptcode, SuNext,SuNameK, Gubun,  Qty, AMT ";// ', DRUGAMT , RateBon , BOHUN, GBSLIP, VCode ";
                    SQL = SQL + ComNum.VBLF + " FROM VIEW_MIR_DETAIL";
                    SQL = SQL + ComNum.VBLF + " ORDER BY DECODE(RTRIM(SUNEXT),'BBBBBB', 1,99), 1,2";

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

                        SS1_Sheet1.Rows.Count = dt.Rows.Count;
                        SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strNewData = dt.Rows[i]["SuNext"].ToString().Trim();

                            if (strOldData != strNewData)
                            {
                                strOldData = strNewData;
                                Display_SUB(ref nRow, ref nTotAmt1, ref nTotAmt2);
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
                        Display_SUB(ref nRow, ref nTotAmt1, ref nTotAmt2);

                        nRow = nRow + 1;
                        SS1_Sheet1.RowCount = nRow;

                        if (nRate != 0)
                        {
                            SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(nTotAmt1 * nRate, "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(VB.Fix((int)(nTotAmt2 * nRate) * 10), "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 6].Text = VB.Format(VB.Fix((int)(((nTotAmt2 * nRate) * 10) - (nTotAmt1 * nRate))), "###,###,###,##0 ");
                        }
                        else
                        {
                            SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(nTotAmt1, "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10, "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 6].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10 - nTotAmt1, "###,###,###,##0 ");
                        }

                        SS1_Sheet1.Cells[nRow - 1, 7].Text = "** 합 계 **";

                        dt.Dispose();
                        dt = null;
                    }

                }
                else if (strBi == "52")
                {

                    SQL = " CREATE OR REPLACE VIEW  VIEW_MIR_DETAIL AS ";
                    SQL = SQL + ComNum.VBLF + "SELECT '' deptcode,  A.SUNEXT SORT, a.SuNext,c.SuNameK,'1' Gubun,SUM(a.Qty*a.Nal) Qty,";// ' SUM(AMT + nvl(amt2,0)) AMT";
                    SQL = SQL + ComNum.VBLF + "SUM(DECODE(a.GbGisul,'1',((a.Amt ) *(100+b.RateGasan)/100)  + nvl(a.amt2,0), a.Amt + nvl(a.amt2,0)  ))  Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_TADTL a," + ComNum.DB_PMPA + "MIR_TAID b," + ComNum.DB_PMPA + "BAS_SUN c ";
                    SQL = SQL + ComNum.VBLF + " WHERE b.PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND b.IPDOPD ='" + strIpdOpd + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND b.YYMM = '" + strYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "   AND B.MISUNO IS NOT NULL";
                    SQL = SQL + ComNum.VBLF + "   AND b.WRTNO = a.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND a.SuNext=c.SuNext(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND (b.MIRGBN ='" + strGbMir + "' OR b.MIRGBN IS NULL )  ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY a.SuNext,c.SuNameK  ";


                    SQL = SQL + ComNum.VBLF + " UNION ALL ";

                    //'약제 상한 발생
                    SQL = SQL + ComNum.VBLF + "SELECT '' deptcode, '0' SORT, 'BBBBBB' , '약가상한차액', '1' Gubun,  1 QTY , ";
                    SQL = SQL + ComNum.VBLF + "       SUM(A.DRUGAMT)  AMT  ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MIR_TADTL a," + ComNum.DB_PMPA + "MIR_TAID b," + ComNum.DB_PMPA + "BAS_SUN c ";
                    SQL = SQL + ComNum.VBLF + " WHERE b.PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND b.IPDOPD ='" + strIpdOpd + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND b.YYMM = '" + strYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "   AND B.MISUNO IS NOT NULL";
                    SQL = SQL + ComNum.VBLF + "   AND b.WRTNO = a.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND a.SuNext=c.SuNext(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND (b.MIRGBN ='" + strGbMir + "' OR b.MIRGBN IS NULL )  ";
                    SQL = SQL + ComNum.VBLF + "   AND A.DRUGAMT <> 0 ";
                    SQL = SQL + ComNum.VBLF + " UNION ALL ";

                    if (strIpdOpd == "I")
                    {
                        if (strActdate != "")// '퇴원청구
                        {
                            SQL = SQL + ComNum.VBLF + "SELECT '' deptcode, a.SUNEXT SORT, a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1 + nvl(a.amt2,0) ) Amt ";
                            SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_SLIP a,BAS_SUN b, IPD_TRANS C ";
                            SQL = SQL + ComNum.VBLF + " WHERE a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND C.ActDate >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND C.ActDate <=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND ( a.YYMM = '" + VB.Right(strYYMM, 4) + "' OR A.YYMM IS NULL )  ";
                            SQL = SQL + ComNum.VBLF + "   AND (a.Nu < '21' OR (a.Nu > '35' AND a.Nu < '41')) ";
                            SQL = SQL + ComNum.VBLF + "   AND a.SuNext=b.SuNext(+) ";
                            SQL = SQL + ComNum.VBLF + "   AND A.TRSNO = C.TRSNO ";
                            SQL = SQL + ComNum.VBLF + "   AND C.BI ='52'";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "SELECT '' deptcode, a.SUNEXT SORT,  a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1 + nvl(a.amt2,0) ) Amt ";
                            SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_SLIP a,BAS_SUN b ";
                            SQL = SQL + ComNum.VBLF + " WHERE a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Right(strYYMM, 4) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND (a.Nu < '21' OR (a.Nu > '35' AND a.Nu < '41')) ";
                            SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT '' deptcode,  A.SUNEXT SORT, a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1 + nvl( a.amt2,0) ) Amt ";
                        SQL = SQL + ComNum.VBLF + "   FROM OPD_SLIP a,BAS_SUN b ";
                        SQL = SQL + ComNum.VBLF + "  WHERE a.Pano='" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND a.ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "    AND a.ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "    AND a.Bi='" + strBi + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND (a.YYMM = '" + VB.Right(strYYMM, 4) + "' OR a.NU='01') ";
                        SQL = SQL + ComNum.VBLF + "    AND (a.Nu <= '21' OR A.BUN ='82')  ";// '치과처지 비급여포함
                        SQL = SQL + ComNum.VBLF + "    AND a.SuNext=b.SuNext(+) ";
                        SQL = SQL + ComNum.VBLF + "  GROUP BY a.SuNext,b.SuNameK ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = " SELECT  deptcode, SuNext,SuNameK, Gubun,  Qty, AMT ";// ', DRUGAMT , RateBon , BOHUN, GBSLIP, VCode "
                    SQL = SQL + ComNum.VBLF + " FROM VIEW_MIR_DETAIL";
                    SQL = SQL + ComNum.VBLF + " ORDER BY DECODE(RTRIM(SUNEXT),'BBBBBB', 1,99), 1,2";

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

                        SS1_Sheet1.Rows.Count = dt.Rows.Count;
                        SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strNewData = dt.Rows[i]["SuNext"].ToString().Trim();

                            if (strOldData != strNewData)
                            {
                                strOldData = strNewData;
                                Display_SUB(ref nRow, ref nTotAmt1, ref nTotAmt2);
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

                        Display_SUB(ref nRow, ref nTotAmt1, ref nTotAmt2);

                        nRow = nRow + 1;
                        SS1_Sheet1.RowCount = nRow;

                        if (nRate != 0)
                        {
                            SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(nTotAmt1 * nRate, "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(VB.Fix((int)(nTotAmt2 * nRate) * 10), "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 6].Text = VB.Format(VB.Fix((int)(((nTotAmt2 * nRate) * 10) - (nTotAmt1 * nRate))), "###,###,###,##0 ");
                        }
                        else
                        {
                            SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(nTotAmt1, "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10, "###,###,###,##0 ");
                            SS1_Sheet1.Cells[nRow - 1, 6].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10 - nTotAmt1, "###,###,###,##0 ");
                        }

                        SS1_Sheet1.Cells[nRow - 1, 7].Text = "** 합 계 **";

                        dt.Dispose();
                        dt = null;
                    }

                }
                else
                //GoSub Bohum_Display
                {
                    //'자료를 SELECT
                    SQL = " SELECT TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE , TO_DATE(JINDATE1,'YYYY-MM-DD') JINDATE1 , VCode, GbGs, DRGCODE ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_INSID";
                    SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "  AND PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND YYMM = '" + strYYMM + "'  ";
                    SQL = SQL + ComNum.VBLF + "  AND IPDOPD ='" + strIpdOpd + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND BI ='" + strBi + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND EDIMIRNO <>'0'";
                    SQL = SQL + ComNum.VBLF + "  AND BOHOJONG NOT IN ('1','2') ";
                    SQL = SQL + ComNum.VBLF + "  AND (GBMIR ='" + strGbMir + "' OR GBMIR IS NULL )  ";

                    if (strBDate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND JINDATE1 = '" + VB.Replace(strBDate, "-", "") + "'  ";
                    }

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    strMirOutdate = "";
                    strMirJindate = "";

                    if (dt.Rows.Count > 0)
                    {
                        strMirOutdate = dt.Rows[0]["OUTDATE"].ToString().Trim();
                        strMirJindate = dt.Rows[0]["JINDATE1"].ToString().Trim();
                    }
                    if (dt.Rows[0]["DRGCODE"].ToString().Trim() != "")
                    {
                        ComFunc.MsgBox("DRG 환자는 처방 상세내역 차액은 맞지 않습니다.", "확인");
                    }

                    dt.Dispose();
                    dt = null;


                    SQL = " CREATE OR REPLACE VIEW  VIEW_MIR_DETAIL AS ";
                    if (strIpdOpd == "O")
                        SQL = SQL + ComNum.VBLF + "SELECT b.deptcode1 DEPTCODE, a.SuNext,c.SuNameK,'1' Gubun,A.BUN, A.GBGISUL, A.GBCHILD, SUM(a.Qty*a.Nal) Qty, ";
                    else
                        SQL = SQL + ComNum.VBLF + "SELECT '' DEPTCODE, a.SuNext,c.SuNameK,'1' Gubun,A.BUN, A.GBGISUL, A.GBCHILD, SUM(a.Qty*a.Nal) Qty, ";
                    SQL = SQL + ComNum.VBLF + " SUM(a.Amt) AMT, 0  DRUGAMT, A.RateBon , B.BOHUN, '' GBSLIP, b.VCODE, B.GBGS,  '' OgPdBun,  '' KSJIN , B.IPDRATE  ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_INSDTL a," + ComNum.DB_PMPA + "MIR_INSID b," + ComNum.DB_PMPA + "BAS_SUN c ";
                    SQL = SQL + ComNum.VBLF + " WHERE B.PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND B.YYMM = '" + strYYMM + "'  ";
                    SQL = SQL + ComNum.VBLF + "  AND B.IPDOPD ='" + strIpdOpd + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND B.BI ='" + strBi + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND B.EDIMIRNO <>'0'";
                    SQL = SQL + ComNum.VBLF + "  AND B.BOHOJONG NOT IN ('1','2') ";
                    SQL = SQL + ComNum.VBLF + "  AND (B.GBMIR ='" + strGbMir + "' OR B.GBMIR IS NULL )  ";
                    SQL = SQL + ComNum.VBLF + "  AND A.SUNEXT NOT IN ('########') ";
                    SQL = SQL + ComNum.VBLF + "  AND B.WRTNO = A.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "  AND A.SuNext=c.SuNext(+) ";

                    if (strBDate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND B.JINDATE1 = '" + VB.Replace(strBDate, "-", "") + "' ";
                    }

                    if (strIpdOpd == "O")
                        SQL = SQL + ComNum.VBLF + " GROUP BY b.deptcode1, a.SuNext,c.SuNameK, A.BUN, A.GBGISUL, A.GBCHILD, A.RATEBON, B.BOHUN, b.VCode , B.Gbgs, B.IPDRATE  ";
                    else
                        SQL = SQL + ComNum.VBLF + " GROUP BY  a.SuNext,c.SuNameK, A.BUN, A.GBGISUL, A.GBCHILD, A.RATEBON, B.BOHUN, b.VCode, B.Gbgs, B.IPDRATE  ";

                    SQL = SQL + ComNum.VBLF + "  UNION ALL ";

                    if (strIpdOpd == "O")
                        SQL = SQL + ComNum.VBLF + "SELECT b.deptcode1 DEPTCODE, 'BBBBBB' , '약가상한차액', '1' Gubun, '15' BUN , '0' GBGISUL, '0' GBCHILD , 1 QTY , ";
                    else
                        SQL = SQL + ComNum.VBLF + "SELECT '' DEPTCODE, 'BBBBBB' , '약가상한차액', '1' Gubun, '15' BUN , '0' GBGISUL, '0' GBCHILD , 1 QTY , ";

                    SQL = SQL + ComNum.VBLF + "SUM(A.EDIDRUGAMT)  AMT, 0 DRUGAMT, B.RateBon , B.BOHUN, '' GBSLIP, b.VCODE,  '' GbGs , '' OgPdBun , '' KSJIN , B.IPDRATE ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_INSDTL a," + ComNum.DB_PMPA + "MIR_INSID b," + ComNum.DB_PMPA + "BAS_SUN c ";
                    SQL = SQL + ComNum.VBLF + " WHERE B.PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND B.YYMM = '" + strYYMM + "'  ";
                    SQL = SQL + ComNum.VBLF + "  AND B.IPDOPD ='" + strIpdOpd + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND B.BI ='" + strBi + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND B.EDIMIRNO <>'0'";
                    SQL = SQL + ComNum.VBLF + "  AND B.BOHOJONG NOT IN ('1','2') ";
                    SQL = SQL + ComNum.VBLF + "  AND (B.GBMIR ='" + strGbMir + "' OR B.GBMIR IS NULL )  ";
                    SQL = SQL + ComNum.VBLF + "  AND B.WRTNO = A.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "  AND A.EDIDRUGAMT <> 0 ";
                    SQL = SQL + ComNum.VBLF + "  AND A.SuNext=c.SuNext(+) ";

                    if (strBDate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND B.JINDATE1 = '" + VB.Replace(strBDate, "-", "") + "' ";
                    }

                    if (strIpdOpd == "O")
                        SQL = SQL + ComNum.VBLF + " GROUP BY b.deptcode1, B.RATEBON, B.BOHUN, B.VCODE, B.IPDRATE  ";
                    else
                        SQL = SQL + ComNum.VBLF + " GROUP BY  B.RATEBON, B.BOHUN, B.VCODE, B.IPDRATE, B.IPDRATE ";

                    if (string.Compare(strYYMM, "200801") >= 0)
                    {

                        SQL = SQL + ComNum.VBLF + " UNION ALL ";
                        //'대불금
                        if (strIpdOpd == "O")
                            SQL = SQL + ComNum.VBLF + " SELECT b.deptcode1 DEPTCODE , '*대불금' SuNext, '대불금' SuNameK,'1' Gubun,'' BUN, '' GBGISUL, '' GBCHILD, 1 Qty, ";
                        else
                            SQL = SQL + ComNum.VBLF + " SELECT '' DEPTCODE , '*대불금' SuNext, '대불금' SuNameK,'1' Gubun,'' BUN, '' GBGISUL, '' GBCHILD, 1 Qty, ";

                        SQL = SQL + ComNum.VBLF + "        DAMT  AMT, 0 DRUGAMT,   0  RateBon ,  '' BOHUN, '' GBSLIP,  '' VCODE , '' GbGs , '' OgPdBun , '' KSJIN,  '' IPDRATE  ";
                        SQL = SQL + ComNum.VBLF + " FROM MIR_INSID b ";
                        SQL = SQL + ComNum.VBLF + " WHERE B.PANO ='" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND B.YYMM = '" + strYYMM + "'  ";
                        SQL = SQL + ComNum.VBLF + "  AND B.IPDOPD ='" + strIpdOpd + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND B.BI ='" + strBi + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND B.EDIMIRNO <>'0'";
                        SQL = SQL + ComNum.VBLF + "  AND DAMT <> 0 ";
                        SQL = SQL + ComNum.VBLF + "  AND B.BOHOJONG NOT IN ('1','2') ";
                        SQL = SQL + ComNum.VBLF + "  AND (B.GBMIR ='" + strGbMir + "' OR B.GBMIR IS NULL )  ";
                        if (strBDate != "")
                            SQL = SQL + ComNum.VBLF + "  AND B.JINDATE1 = '" + VB.Replace(strBDate, "-", "") + "' ";
                        SQL = SQL + ComNum.VBLF + " UNION ALL ";

                        //'장애기금
                        if (strIpdOpd == "O")
                            SQL = SQL + ComNum.VBLF + " SELECT b.deptcode1 DEPTCODE, '*장애기금' SuNext, '장애기금' SuNameK,'1' Gubun,'' BUN, '' GBGISUL, '' GBCHILD, 1 Qty, ";
                        else
                            SQL = SQL + ComNum.VBLF + " SELECT '' DEPTCODE, '*장애기금' SuNext, '장애기금' SuNameK,'1' Gubun,'' BUN, '' GBGISUL, '' GBCHILD, 1 Qty, ";

                        SQL = SQL + ComNum.VBLF + "        BOAMT  AMT, 0 DRUGAMT  , 0  RateBon ,  '' BOHUN, '' GBSLIP,  '' VCODE, '' GbGs , '' OgPdBun ,'' KSJIN, '' IPDRATE  ";
                        SQL = SQL + ComNum.VBLF + " FROM MIR_INSID b ";
                        SQL = SQL + ComNum.VBLF + " WHERE B.PANO ='" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND B.YYMM = '" + strYYMM + "'  ";
                        SQL = SQL + ComNum.VBLF + "  AND B.IPDOPD ='" + strIpdOpd + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND B.BI ='" + strBi + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND B.EDIMIRNO <>'0'";
                        SQL = SQL + ComNum.VBLF + "  AND BOAMT <> 0 ";
                        SQL = SQL + ComNum.VBLF + "  AND B.BOHOJONG NOT IN ('1','2') ";
                        SQL = SQL + ComNum.VBLF + "  AND (B.GBMIR ='" + strGbMir + "' OR B.GBMIR IS NULL )  ";
                        if (strBDate != "")
                            SQL = SQL + ComNum.VBLF + "  AND B.JINDATE1 = '" + VB.Replace(strBDate, "-", "") + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "UNION ALL ";

                    //'외래/입원 저방금액은 병원가산율,소아가산 적용이 모두 된금액입니다.
                    //'그래서 GBGISUL, GBCHILD 값을 0으로 SETTING
                    //TODO 부모 폼 찾아서 생성자로 라디오 버튼 값 던져줘야함.
                    //if strIpdOpd = "I" And FrmIpdMirCheckUpdate2.OptMir(3).Value = False Then
                    if (strIpdOpd == "I")
                    {
                        if (strGbMir == "1")//Then '퇴원청구
                        {

                            SQL = SQL + ComNum.VBLF + "SELECT '' DEPTCODE, a.SuNext,b.SuNameK,'2' Gubun, A.BUN, '0' GBGISUL,'0' GBCHILD,  ";
                            SQL = SQL + ComNum.VBLF + "       SUM(a.Qty*a.Nal) Qty,SUM(a.Amt1) Amt, 0 DRUGAMT , C.BONRATE RateBon, C.BOHUN , '' GBSLIP, C.VCode , ''           GbGs ,c.OgPdBun  , '' KSJIN,  '' IPDRATE  ";
                            SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_SLIP a,BAS_SUN b , IPD_TRANS C ";
                            SQL = SQL + ComNum.VBLF + " WHERE C.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND C.ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND C.ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Right(strYYMM, 4) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.GbSelf='0' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";
                            SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = C.IPDNO ";
                            SQL = SQL + ComNum.VBLF + "   AND A.TRSNO = C.TRSNO ";
                            SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK,A.BUN, C.BONRATE, C.BOHUN, C.VCode, c.OgPdBun    ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "SELECT  '' DEPTCODE, a.SuNext,b.SuNameK,'2' Gubun, A.BUN, '0' GBGISUL,'0' GBCHILD, ";
                            SQL = SQL + ComNum.VBLF + "       SUM(a.Qty*a.Nal) Qty,SUM(a.Amt1) Amt, 0 DRUGAMT, C.BONRATE RateBon,  C.BOHUN , '' GBSLIP, C.VCode, '' GbGs ,c.OgPdBun , '' KSJIN, '' IPDRATE  ";
                            SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_SLIP a,BAS_SUN b, IPD_TRANS C ";
                            SQL = SQL + ComNum.VBLF + " WHERE a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Right(strYYMM, 4) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.GbSelf='0' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";

                            if (strMirOutdate != "" && strMirJindate != "")
                            {
                                SQL = SQL + ComNum.VBLF + " AND A.BDATE >= TO_DATE('" + strMirJindate + "' ,'YYYY-MM-DD')";
                                SQL = SQL + ComNum.VBLF + " AND A.BDATE <= TO_DATE('" + strMirOutdate + "' ,'YYYY-MM-DD')";
                            }

                            SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = C.IPDNO ";
                            SQL = SQL + ComNum.VBLF + "   AND A.TRSNO = C.TRSNO ";
                            SQL = SQL + ComNum.VBLF + "   AND a.SuNext=b.SuNext(+) ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK, A.BUN, C.BONRATE, C.BOHUN, C.VCode, c.OgPdBun  ";
                        }
                    }
                    else
                    {
                        if (strGbMir == "3")
                        {
                            //'GBSLIP K는 인공신장, H는 혈우병
                            SQL = SQL + ComNum.VBLF + "SELECT '' deptcode, a.SuNext,b.SuNameK,'2' Gubun, A.BUN, '0' GBGISUL,'0' GBCHILD, ";
                            SQL = SQL + ComNum.VBLF + "       SUM(a.Qty*a.Nal) Qty,SUM(a.Amt1) Amt, 0 DRUGAMT,  0  RateBon, c.BOHUN , A.GBSLIP, C.VCode,'' GbGs , ' 'OgPdBun, A.KSJIN, '' IPDRATE  ";
                            SQL = SQL + ComNum.VBLF + "  FROM OPD_SLIP a,BAS_SUN b, OPD_MASTER C  ";
                            SQL = SQL + ComNum.VBLF + " WHERE a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND a.ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND (a.YYMM = '" + VB.Right(strYYMM, 4) + "'  OR a.NU ='01' OR a.GbSelf ='0') ";// '조제료 때문에 조건을 or 로 gbself 체크함(jjy:2003-06-26)
                            SQL = SQL + ComNum.VBLF + "   AND a.amt1 <>'0' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";

                            //If FrmIpdMirCheckUpdate2.OptMir(3).Value = True Then
                            //TODO
                            if (strGbMir == "1")
                            {
                                SQL = SQL + ComNum.VBLF + " AND a.GbSlip IN ('Z','Q','E') ";      //'응급실 6시간이상
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + " AND a.GbSlip NOT IN ('Z','Q','E') ";
                            }

                            SQL = SQL + ComNum.VBLF + "  AND A.PANO = C.PANO(+) ";
                            SQL = SQL + ComNum.VBLF + "  AND A.DEPTCODE = C.DEPTCODE(+)";
                            SQL = SQL + ComNum.VBLF + "  AND A.BDATE = C.BDATE (+)";
                            SQL = SQL + ComNum.VBLF + "  AND A.ACTDATE = C.ACTDATE (+)";
                            SQL = SQL + ComNum.VBLF + "  AND A.BI =C.BI(+) ";

                            SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";

                            if (strBDate != "")
                            {
                                SQL = SQL + ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                            }
                            SQL = SQL + ComNum.VBLF + "GROUP BY  a.SuNext,b.SuNameK, A.BUN, A.GBSLIP, C.BOHUN, C.VCode,  A.KSJIN   ";
                        }
                        else
                        {
                            //'GBSLIP K는 인공신장, H는 혈우병
                            SQL = SQL + ComNum.VBLF + "SELECT a.deptcode, a.SuNext,b.SuNameK,'2' Gubun, A.BUN, '0' GBGISUL,'0' GBCHILD, ";
                            SQL = SQL + ComNum.VBLF + "       SUM(a.Qty*a.Nal) Qty,SUM(a.Amt1) Amt, 0 DRUGAMT,  0  RateBon, c.BOHUN , A.GBSLIP, C.VCode, '' GbGs ,'' OgPdBun , A.KSJIN , '' IPDRATE  ";
                            SQL = SQL + ComNum.VBLF + "  FROM OPD_SLIP a,BAS_SUN b, OPD_MASTER C  ";
                            SQL = SQL + ComNum.VBLF + " WHERE a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND a.ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND (a.YYMM = '" + VB.Right(strYYMM, 4) + "'  OR a.NU ='01' OR a.GbSelf ='0') ";// '조제료 때문에 조건을 or 로 gbself 체크함(jjy:2003-06-26)
                            SQL = SQL + ComNum.VBLF + "   AND a.amt1 <>'0' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";

                            //If FrmIpdMirCheckUpdate2.OptMir(3).Value = True Then
                            //TODO
                            if (SQL == "1")
                                SQL = SQL + ComNum.VBLF + " AND a.GbSlip IN ('Z','Q','E') ";//      '응급실 6시간이상
                            else
                                SQL = SQL + ComNum.VBLF + " AND a.GbSlip NOT IN ('Z','Q','E') ";

                            SQL = SQL + ComNum.VBLF + "  AND A.PANO = C.PANO(+) ";
                            SQL = SQL + ComNum.VBLF + "  AND A.DEPTCODE = C.DEPTCODE(+)";
                            SQL = SQL + ComNum.VBLF + "  AND A.BDATE = C.BDATE (+)";
                            SQL = SQL + ComNum.VBLF + "  AND A.ACTDATE = C.ACTDATE (+)";
                            SQL = SQL + ComNum.VBLF + "  AND A.BI =C.BI(+) ";

                            SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";

                            if (strBDate != "")
                                SQL = SQL + ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.deptcode,  a.SuNext,b.SuNameK, A.BUN, A.GBSLIP, C.BOHUN, C.VCode, A.KSJIN ";
                        }
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = " SELECT DEPTCODE, SuNext,SuNameK, Gubun, BUN, GBGISUL, GBCHILD, Qty, ";
                    SQL = SQL + ComNum.VBLF + " AMT, DRUGAMT , RateBon , BOHUN, GBSLIP, VCode , GBGS , KSJIN, IPDRATE , OGPDBUN  ";
                    SQL = SQL + ComNum.VBLF + " FROM VIEW_MIR_DETAIL";
                    SQL = SQL + ComNum.VBLF + " ORDER BY DEPTCODE, DECODE(RTRIM(SUNEXT),'BBBBBB', 1,99), 1,2";


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

                        SS1_Sheet1.Rows.Count = dt.Rows.Count;
                        SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                        strNewData = dt.Rows[0]["SuNext"].ToString().Trim();
                        strNewData = dt.Rows[0]["DEPTCODE"].ToString().Trim();

                        for (i = 0; i < dt.Rows.Count; i++)
                        {

                            if (strOldData != dt.Rows[i]["SUNEXT"].ToString().Trim())
                            {
                                Display_SUB(ref nRow, ref nTotAmt1, ref nTotAmt2);
                                strOldData = dt.Rows[i]["SUNEXT"].ToString().Trim();
                                nQty1 = 0;
                                nQty2 = 0;
                                nAmt1 = 0;
                                nAmt2 = 0;
                            }

                            if (strDeptOld != dt.Rows[i]["DeptCode"].ToString().Trim())
                            {
                                if (i != 0)
                                {
                                    nRow = nRow + 1;
                                    if (nRow > SS1_Sheet1.RowCount)
                                    {
                                        SS1_Sheet1.RowCount = nRow;
                                    }
                                    nSTotAmt2 = (int)(nSTotAmt2 / 10) * 10;

                                    SS1_Sheet1.Cells[nRow - 1, -1].Text = "과소계";
                                    SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(nSTotAmt1, "###,###,###,##0 ");
                                    SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(nSTotAmt2, "###,###,###,##0 ");
                                    SS1_Sheet1.Cells[nRow - 1, 6].Text = VB.Format((nSTotAmt2 - nSTotAmt1), "###,###,###,##0 ");
                                    nSTotAmt1 = 0;
                                    nSTotAmt2 = 0;
                                }
                                strDeptOld = dt.Rows[i]["DeptCode"].ToString().Trim();
                            }

                            strGubun = dt.Rows[i]["Gubun"].ToString().Trim();
                            strBun = dt.Rows[i]["Bun"].ToString().Trim();
                            strGbGisul = dt.Rows[i]["GbGisul"].ToString().Trim();
                            strGbChild = dt.Rows[i]["GbChild"].ToString().Trim();
                            strRateBon = dt.Rows[i]["RateBon"].ToString().Trim();
                            strBohun = dt.Rows[i]["Bohun"].ToString().Trim();
                            nAmt = VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                            nDrugAmt = VB.Val(dt.Rows[i]["DRUGAMT"].ToString().Trim());
                            strGbSlip = dt.Rows[i]["GBSLIP"].ToString().Trim();
                            strVCode = dt.Rows[i]["VCODE"].ToString().Trim();
                            strSuNext = dt.Rows[i]["SUNEXT"].ToString().Trim();
                            strDeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
                            strSunameK = dt.Rows[i]["SUNAMEK"].ToString().Trim();

                            if (strDeptCode == "NE")
                            {
                                i = i;
                            }

                            #region BONAMT_GESAN_NEW


                            //GoSub 
                            if (dt.Rows[i]["SUNEXT"].ToString().Trim() == "X - P37100")
                            {
                                i = i;
                            }

                            if (dt.Rows[i]["SUNEXT"].ToString().Trim() == "*대불금" || dt.Rows[i]["SUNEXT"].ToString().Trim() == "*장애기금")
                            {
                                nJohapAmt = nAmt;
                            }

                            switch (strBun)
                            {
                                case "74":
                                    if (strBi == "21")
                                    {
                                        switch (strVCode)
                                        {
                                            case "V193":
                                            case "V191":
                                            case "V192":
                                                nRate = 10;
                                                break;
                                            default:
                                                nRate = 20;
                                                break;
                                        }
                                    }
                                    else if (strBi == "22")
                                    {
                                        switch (strVCode)
                                        {
                                            case "V193":
                                            case "V191":
                                            case "V192":
                                                nRate = 10;
                                                break;
                                            default:
                                                nRate = 20;
                                                break;
                                        }
                                    }

                                    else
                                    {
                                        if (string.Compare(strYYMM, "200801") >= 0)
                                            nRate = 50;
                                        else
                                            nRate = 20;
                                    }
                                    break;
                                case "72":
                                case "73":
                                    if (strGubun == "1") //'청구만 처방 별로 .. 본인부담적용 읽어서 처리
                                    {
                                        nRate_I = Convert.ToDouble(strRateBon);
                                        nRate_O = Convert.ToDouble(strRateBon);
                                    }
                                    nRate = nRate_O;
                                    break;
                                default:
                                    if (strRateBon == "0")// '외래/입원 실처방
                                    {
                                        if (strIpdOpd == "I")
                                            nRate = nRate_I;
                                        else
                                            nRate = nRate_O;
                                    }
                                    else
                                        nRate = VB.Val(strRateBon);
                                    if (nRate != 0 && (strGbSlip == "K" || strGbSlip == "G" || strGbSlip == "F"))
                                    {
                                        nRate = nRate_I;
                                    }

                                    if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "O")
                                        nRate = 0;//   '정상분만 면제
                                    break;
                            }

                            if (strGbSlip == "V")
                            {
                                if (string.Compare(strYYMM, "200912") >= 0)
                                {
                                    nRate = 5;// '암등
                                }
                                else
                                {
                                    nRate = 10;// '암등록환자 10
                                }
                            }

                            if (strGbSlip == "R")
                                nRate = 35;// '소아(6세미만) 35
                            if (strGbSlip == "U")
                                nRate = 15;// '소아(6세미만) 15

                            if (dt.Rows[i]["KSJIN"].ToString().Trim() == "V")
                            {
                                nRate = 10; //'     V.등록 희귀.난치성(건강보험 산정특례)
                            }

                            //'2016-12-15
                            if (string.Compare(strYYMM, "201608") >= 0 && strVCode == "V000")
                            {
                                nRate = 0;
                            }
                            //2021-06-29 잠복결핵 추가
                            if (string.Compare(strYYMM, "202107") >= 0 && strVCode == "V010")
                            {
                                nRate = 0;
                            }

                            nRate = nRate / 100;

                            if (strNewData == "BBBBBB")
                                nRate = 0;
                            if (string.Compare(strYYMM, "200801") < 0 && strBohun == "3")
                                nRate = 0;
                            //'지원금이 있는지 점검
                            if (dt.Rows[i]["GbGs"].ToString().Trim() == "H" || dt.Rows[i]["GbGs"].ToString().Trim() == "P")
                                nRate = 0;//  '희귀난치성, 보호2종 6세미반 은 본인부담금 0 처리

                            if (VB.Val(strGbGisul) > 0)
                            {

                                NAMT11 = NAMT11 + nAmt;
                                NAMTGASAN = NAMTGASAN + (nAmt * nGaSan / 100) - nAmt;
                                nAmt = nAmt * nGaSan / 100;
                            }
                            else
                            {
                                NAMT12 = NAMT12 + nAmt;
                            }

                            nBonAmt = nAmt;
                            nBonAmt = nBonAmt * nRate;
                            nJohapAmt = nAmt - nBonAmt;

                            #endregion
                            if (strGubun == "2")//'처방발생
                            {
                                nQty1 = nQty1 + VB.Val(dt.Rows[i]["QTY"].ToString().Trim());
                                nAmt1 = nAmt1 + nJohapAmt;
                            }
                            else
                            {
                                nQty2 = nQty2 + VB.Val(dt.Rows[i]["QTY"].ToString().Trim());
                                nAmt2 = nAmt2 + nJohapAmt;
                            }
                        }
                        Display_SUB(ref nRow, ref nTotAmt1, ref nTotAmt2);

                        nSTotAmt2 = (int)(nSTotAmt2 / 10) * 10;

                        nRow = nRow + 1;
                        SS1_Sheet1.RowCount = nRow;


                        SS1_Sheet1.Cells[nRow - 1, 1].Text = "과소계";
                        SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(nSTotAmt1, "###,###,###,##0 ");
                        SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(nSTotAmt2, "###,###,###,##0 ");
                        SS1_Sheet1.Cells[nRow - 1, 6].Text = VB.Format(nSTotAmt2 - nSTotAmt1, "###,###,###,##0 ");

                        nTotAmt2 = nTotAmt2 + nAmt3;
                        nTotAmt2 = (int)(nTotAmt2 / 10) * 10;


                        SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(nTotAmt1, "###,###,###,##0 ");
                        SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(nTotAmt2, "###,###,###,##0 ");
                        SS1_Sheet1.Cells[nRow - 1, 6].Text = VB.Format(nTotAmt2 - nTotAmt1, "###,###,###,##0 ");
                        SS1_Sheet1.Cells[nRow - 1, 7].Text = " ** 합 계 **";

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

        private void Display_SUB(ref int nRow, ref double nTotAmt1, ref double nTotAmt2)
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

            SS1_Sheet1.Cells[nRow - 1, 0].Text = strDeptCode;
            SS1_Sheet1.Cells[nRow - 1, 1].Text = strSuNext;
            SS1_Sheet1.Cells[nRow - 1, 2].Text = VB.Format(nQty1, "###,###,##0.0");
            SS1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nQty2, "###,###,##0.0");

            if (nRate != 0)
            {
                SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(nAmt1, "###,###,##0.0");
                SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(nAmt2, "###,###,##0.0");
                SS1_Sheet1.Cells[nRow - 1, 6].Text = VB.Format((nAmt2 - nAmt1), "###,###,###,###");
            }
            else
            {
                SS1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(nAmt1, "###,###,##0.0");
                SS1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(nAmt2, "###,###,##0.0");
                SS1_Sheet1.Cells[nRow - 1, 6].Text = VB.Format((nAmt2 - nAmt1), "###,###,###,###");
            }

            SS1_Sheet1.Cells[nRow - 1, 7].Text = strSunameK;

            nTotAmt1 = nTotAmt1 + nAmt1;
            nTotAmt2 = nTotAmt2 + nAmt2;

            nSTotAmt1 = nSTotAmt1 + nAmt1;
            nSTotAmt2 = nSTotAmt2 + nAmt2;
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
