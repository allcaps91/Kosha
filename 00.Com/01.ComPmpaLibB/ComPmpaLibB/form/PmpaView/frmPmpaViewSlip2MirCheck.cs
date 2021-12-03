using ComBase; //기본 클래스
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComPmpaLibB.form.PmpaView
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewSlip2MirCheck.cs
    /// Description     : 처방내역과청구내역비교
    /// Author          : 김효성
    /// Create Date     : 2017-09-06
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// Flag를 사용해서 출력문 조회
    /// 
    /// </history>
    /// <seealso cref= "\psmh\mir\miretc\miretc55.frm  >> frmPmpaViewSlip2MirCheck.cs 폼이름 재정의" />	
    /// <seealso cref= "\psmh\misu\misubs\misubs71.frm >> frmPmpaViewSlip2MirCheck.cs 폼이름 재정의" />	
    /// <seealso cref= "\psmh\misu\misubs\misubs75.frm >> frmPmpaViewSlip2MirCheck.cs 폼이름 재정의" />	
    /// <seealso cref= "\psmh\misu\misubs\misubs77.frm >> frmPmpaViewSlip2MirCheck.cs 폼이름 재정의" />	
    /// <seealso cref= "\psmh\misu\misubs\misubs78.frm >> frmPmpaViewSlip2MirCheck.cs 폼이름 재정의" />	

    public partial class frmPmpaViewSlip2MirCheck : Form
    {
        //clsSpread.SpdPrint_Margin setMargin;
        //clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();
        frmPmpaMagamIpdMirCheckUpdate2 frmPmpaMagamIpdMirCheckUpdate2S = new frmPmpaMagamIpdMirCheckUpdate2();
        frmPmpaViewSlip1 frmPmpaViewSlip1S = null;
        Timer t1 = null;        
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
        //string strDeptOld = "";
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
        string strTRSNO = "";

        string strTemp = "";
        
        public frmPmpaViewSlip2MirCheck(string strGstrWrtno, string strGstrPano, string strGstrSname, string strGstrYYMM, string strGstrIO)
        {
            GstrPano = strGstrPano;
            GstrWrtno = strGstrWrtno;
            GstrSname = strGstrSname;
            GstrYYMM = strGstrYYMM;
            GstrIO = strGstrIO;

            InitializeComponent();
        }

        public frmPmpaViewSlip2MirCheck(string gstr) //2018-11-07 김해수
        {
            strTemp  = gstr;

            GstrWrtno = VB.Val(VB.Pstr(strTemp, ",", 1)).ToString();
            GstrPano = VB.Pstr(strTemp, ",", 3);
            GstrSname = VB.Pstr(strTemp, ",", 4);
            GstrYYMM = VB.Pstr(strTemp, ",", 7);
            GstrIO = VB.Pstr(strTemp, ",", 8);
 

            InitializeComponent();
        }

        public frmPmpaViewSlip2MirCheck()
        {
            InitializeComponent();
        }

        private void frmPmpaViewSlip2MirCheck_Load(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            this.ControlBox = false;
            Screen_Display2();
            
            setTimer();
        }

        private void Screen_Display2()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;

            nWRTNO = VB.Val(VB.Pstr(strTemp, ",", 1)).ToString();
            strActdate = VB.Pstr(strTemp, ",", 2);
            strPano = VB.Pstr(strTemp, ",", 3);
            strSname = VB.Pstr(strTemp, ",", 4);
            strBi = VB.Pstr(strTemp, ",", 5);
            strInDate = VB.Pstr(strTemp, ",", 6);
            strYYMM = VB.Pstr(strTemp, ",", 7);
            strIpdOpd = VB.Pstr(strTemp, ",", 8);
            strGbMir = VB.Pstr(strTemp, ",", 9);
            strBDate = VB.Pstr(strTemp, ",", 10);

            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTdate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);

            strTRSNO = "";

            lblMsg.Text = "등록번호:" + GstrPano + " 성명:" + strSname;

            if (strIpdOpd == "I")
            {
                lblMsg.Text = lblMsg.Text + " 퇴원수납일:" + strActdate + " 입원일:" + strInDate;
            }
            lblMsg.Text = lblMsg.Text + " 청구월:" + strYYMM + " 청구번호:" + nWRTNO;

            if(strBDate != "")
            {
                lblMsg.Text = lblMsg.Text + " 처방일 : " + strBDate;
            }

            nTotAmt1 = 0;
            nTotAmt2 = 0;

            nRate_I = Convert.ToDouble(CPF.READ_BonRate(clsDB.DbCon, "IPD", strBi, VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01"));
            nRate_O = Convert.ToDouble(CPF.READ_BonRate(clsDB.DbCon, "OPD", strBi, VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01"));
            nGaSan = Convert.ToDouble(CPF.READ_RateGasan(clsDB.DbCon, strBi, VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01"));

            //clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strBi == "31")
                {
                    #region GoSub Sanje_Display
                    strEdiOK = "NO";
                    SQL = "";
                    SQL = "SELECT EdiTAmt, TRSNO ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_SANID ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1=1 ";
                    SQL = SQL + ComNum.VBLF + "    AND PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND YYMM = '" + strYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "    AND IPDOPD ='" + strIpdOpd + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND GBNEDI ='0'   ";
                    SQL = SQL + ComNum.VBLF + "    AND MIRGBN ='" + strGbMir + "'   ";


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
                            strTRSNO = dt.Rows[0]["TRSNO"].ToString().Trim();
                        }
                    }
                    dt.Dispose();
                    dt = null;

                    SQL = " CREATE OR REPLACE VIEW  VIEW_MIR_DETAIL AS ";
                    if (strEdiOK == "OK")
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT A.SUNEXT SORT, a.SuNext,c.SuNameK,'1' Gubun,SUM(a.EdiQty*a.EdiNal) Qty,";
                        SQL = SQL + ComNum.VBLF + "SUM(DECODE(a.GbGisul,'1',(a.EdiAmt*(100+b.RateGasan)/100),a.EdiAmt)) Amt ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT A.SUNEXT SORT, a.SuNext,c.SuNameK,'1' Gubun,SUM(a.Qty*a.Nal) Qty,";
                        SQL = SQL + ComNum.VBLF + "SUM(DECODE(a.GbGisul,'1',(a.Amt*(100+b.RateGasan)/100),a.Amt)) Amt ";
                    }
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_SANDTL a," + ComNum.DB_PMPA + "MIR_SANID b," + ComNum.DB_PMPA + "BAS_SUN c ";
                    SQL = SQL + ComNum.VBLF + "   WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "     AND b.PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND b.YYMM = '" + strYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "     AND b.IPDOPD ='" + strIpdOpd + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND b.WRTNO = a.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "     AND a.SuNext=c.SuNext(+) ";
                    SQL = SQL + ComNum.VBLF + "     AND (b.MIRGBN ='" + strGbMir + "' OR b.MIRGBN IS NULL )  ";

                    if (strEdiOK == "OK")
                        SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,c.SuNameK ";
                    else
                        SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,c.SuNameK ";

                    SQL = SQL + ComNum.VBLF + "UNION ALL ";

                    //'약제 상한 발생
                    SQL = SQL + ComNum.VBLF + "SELECT '#' SORT, 'BBBBBB' , '약가상한차액', '1' Gubun,  1 QTY , ";
                    SQL = SQL + ComNum.VBLF + "SUM(A.EDIDRUGAMT)  AMT  ";
                    SQL = SQL + ComNum.VBLF + " FROM MIR_SANDTL a,MIR_SANID b,BAS_SUN c ";
                    SQL = SQL + ComNum.VBLF + " WHERE b.PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND b.YYMM = '" + strYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "   AND b.IPDOPD ='" + strIpdOpd + "' ";
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
                            SQL = SQL + ComNum.VBLF + "SELECT A.SUNEXT SORT, a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt ";
                            SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_SLIP a,BAS_SUN b, IPD_trans C ";
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
                            SQL = SQL + ComNum.VBLF + "SELECT  A.SUNEXT SORT, a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt ";
                            SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_SLIP a,BAS_SUN b ";
                            SQL = SQL + ComNum.VBLF + " WHERE a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Right(strYYMM, 4) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.GbSelf='0' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";
                            SQL = SQL + ComNum.VBLF + "   AND a.BDATE >= TO_DATE('" + strInDate + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "   AND a.SuNext=b.SuNext(+) ";
                            switch (strTRSNO)
                            {
                                case "1384640":
                                    SQL = SQL + ComNum.VBLF + "   AND A.TRSNO = '" + strTRSNO + "' ";
                                    break;
                            }
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK ";
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "SELECT A.SUNEXT SORT, a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt ";
                        SQL = SQL + ComNum.VBLF + "  FROM OPD_SLIP a,BAS_SUN b ";
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

                    SQL = " SELECT SuNext,SuNameK, Gubun,  Qty, AMT ";// ', DRUGAMT , RateBon , BOHUN, GBSLIP, VCode ";
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
                        nRow = 1;
                        strOldData = "";
                        strSuNext = "";
                        nQty1 = 0;
                        nQty2 = 0;
                        nAmt1 = 0;
                        nAmt2 = 0;
                        strSunameK = "";

                        ssView1_Sheet1.Rows.Count = dt.Rows.Count;
                        ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
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
                                nQty1 = nQty1 + Convert.ToDouble(dt.Rows[i]["Qty"].ToString().Trim());
                                nAmt1 = nAmt1 + Convert.ToDouble(VB.Val(dt.Rows[i]["Amt"].ToString().Trim()));
                            }
                            else
                            {
                                nQty2 = nQty2 + Convert.ToDouble(dt.Rows[i]["Qty"].ToString().Trim());
                                nAmt2 = nAmt2 + Convert.ToDouble(VB.Val(dt.Rows[i]["Amt"].ToString().Trim()));
                            }
                        }
                        Display_SUB(ref nRow, ref nTotAmt1, ref nTotAmt2);

                        nRow = nRow + 1;
                        ssView1_Sheet1.RowCount = nRow;

                        if (nRate != 0)
                        {
                            //ssView1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nTotAmt1 * nRate, "###,###,###,##0 ");
                            //ssView1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(VB.Fix((int)(nTotAmt2 * nRate) * 10), "###,###,###,##0 ");
                            //ssView1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(VB.Fix((int)(((nTotAmt2 * nRate) * 10) - (nTotAmt1 * nRate))), "###,###,###,##0 ");
                            ssView1_Sheet1.Cells[0, 3].Text = VB.Format(nTotAmt1 * nRate, "###,###,###,##0 ");
                            ssView1_Sheet1.Cells[0, 4].Text = VB.Format(VB.Fix((int)(nTotAmt2 * nRate) * 10), "###,###,###,##0 ");
                            ssView1_Sheet1.Cells[0, 5].Text = VB.Format(VB.Fix((int)(((nTotAmt2 * nRate) * 10) - (nTotAmt1 * nRate))), "###,###,###,##0 ");
                        }
                        else
                        {
                            //ssView1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nTotAmt1, "###,###,###,##0 ");
                            //ssView1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10, "###,###,###,##0 ");
                            //ssView1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10 - nTotAmt1, "###,###,###,##0 ");
                            ssView1_Sheet1.Cells[0, 3].Text = VB.Format(nTotAmt1, "###,###,###,##0 ");
                            ssView1_Sheet1.Cells[0, 4].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10, "###,###,###,##0 ");
                            ssView1_Sheet1.Cells[0, 5].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10 - nTotAmt1, "###,###,###,##0 ");
                        }

                        ssView1_Sheet1.Cells[0, 6].Text = "** 합 계 **";

                        dt.Dispose();
                        dt = null;
                    }
                    #endregion
                }
                else if (strBi == "52")
                {
                    #region TA_Display
                    SQL = " CREATE OR REPLACE VIEW  VIEW_MIR_DETAIL AS ";
                    SQL = SQL + ComNum.VBLF + "SELECT A.SUNEXT SORT, a.SuNext,c.SuNameK,'1' Gubun,SUM(a.Qty*a.Nal) Qty,";// ' SUM(AMT + nvl(amt2,0)) AMT"nvl(amt2,0)) AMT";
                    SQL = SQL + ComNum.VBLF + "SUM(DECODE(a.GbGisul,'1',((a.Amt ) *(100+b.RateGasan)/100)  + nvl(a.amt2,0), a.Amt + nvl(a.amt2,0)  ))  Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_TADTL a," + ComNum.DB_PMPA + "MIR_TAID b," + ComNum.DB_PMPA + "BAS_SUN c ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "   AND b.PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND b.IPDOPD ='" + strIpdOpd + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND b.YYMM = '" + strYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "   AND B.MISUNO IS NOT NULL";
                    SQL = SQL + ComNum.VBLF + "   AND b.WRTNO = a.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "   AND a.SuNext=c.SuNext(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND (b.MIRGBN ='" + strGbMir + "' OR b.MIRGBN IS NULL )  ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY a.SuNext,c.SuNameK  ";
                    SQL = SQL + ComNum.VBLF + " UNION ALL ";


                    //'약제 상한 발생
                    SQL = SQL + ComNum.VBLF + "SELECT '0' SORT, 'BBBBBB' , '약가상한차액', '1' Gubun,  1 QTY , ";
                    SQL = SQL + ComNum.VBLF + "       SUM(A.DRUGAMT)  AMT  ";
                    SQL = SQL + ComNum.VBLF + "  FROM MIR_TADTL a,MIR_TAID b,BAS_SUN c ";
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
                            SQL = SQL + ComNum.VBLF + "SELECT a.SUNEXT SORT,  a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1 + nvl(a.amt2,0) ) Amt ";
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
                        SQL = SQL + ComNum.VBLF + " SELECT  A.SUNEXT SORT, a.SuNext,b.SuNameK,'2' Gubun,SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1 + nvl( a.amt2,0) ) Amt ";
                        SQL = SQL + ComNum.VBLF + "   FROM OPD_SLIP a,BAS_SUN b ";
                        SQL = SQL + ComNum.VBLF + "  WHERE a.Pano='" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND a.ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "    AND a.ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
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


                    SQL = " SELECT SuNext,SuNameK, Gubun,  Qty, AMT ";// ', DRUGAMT , RateBon , BOHUN, GBSLIP, VCode ";
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
                        nRow = 1;
                        strOldData = "";
                        strSuNext = "";
                        nQty1 = 0;
                        nQty2 = 0;
                        nAmt1 = 0;
                        nAmt2 = 0;
                        strSunameK = "";

                        ssView1_Sheet1.Rows.Count = dt.Rows.Count;
                        ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
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
                                nQty1 = Convert.ToDouble(nQty1 + dt.Rows[i]["Qty"].ToString().Trim());
                                nAmt1 = Convert.ToDouble(nAmt1 + VB.Val(dt.Rows[i]["Amt"].ToString().Trim()));
                            }
                            else
                            {
                                nQty2 = Convert.ToDouble(nQty2 + dt.Rows[i]["Qty"].ToString().Trim());
                                nAmt2 = Convert.ToDouble(nAmt2 +VB.Val(dt.Rows[i]["Amt"].ToString().Trim()));
                            }
                        }
                        Display_SUB(ref nRow, ref nTotAmt1, ref nTotAmt2);

                        nRow = nRow + 1;
                        ssView1_Sheet1.RowCount = nRow;

                        if (nRate != 0)
                        {
                            //ssView1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nTotAmt1 * nRate, "###,###,###,##0 ");
                            //ssView1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(VB.Fix((int)(nTotAmt2 * nRate) * 10), "###,###,###,##0 ");
                            //ssView1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(VB.Fix((int)(((nTotAmt2 * nRate) * 10) - (nTotAmt1 * nRate))), "###,###,###,##0 ");
                            ssView1_Sheet1.Cells[0, 3].Text = VB.Format(nTotAmt1 * nRate, "###,###,###,##0 ");
                            ssView1_Sheet1.Cells[0, 4].Text = VB.Format(VB.Fix((int)(nTotAmt2 * nRate) * 10), "###,###,###,##0 ");
                            ssView1_Sheet1.Cells[0, 5].Text = VB.Format(VB.Fix((int)(((nTotAmt2 * nRate) * 10) - (nTotAmt1 * nRate))), "###,###,###,##0 ");

                        }
                        else
                        {
                            //ssView1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nTotAmt1, "###,###,###,##0 ");
                            //ssView1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10, "###,###,###,##0 ");
                            //ssView1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10 - nTotAmt1, "###,###,###,##0 ");
                            ssView1_Sheet1.Cells[0, 3].Text = VB.Format(nTotAmt1, "###,###,###,##0 ");
                            ssView1_Sheet1.Cells[0, 4].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10, "###,###,###,##0 ");
                            ssView1_Sheet1.Cells[0, 5].Text = VB.Format(VB.Fix((int)nTotAmt2 / 10) * 10 - nTotAmt1, "###,###,###,##0 ");
                        }

                        //ssView1_Sheet1.Cells[nRow - 1, 6].Text = "** 합 계 **";
                        ssView1_Sheet1.Cells[0, 6].Text = "** 합 계 **";

                        dt.Dispose();
                        dt = null;
                    }
                    #endregion
                }
                else
                //GoSub Bohum_Display
                {
                    //'자료를 SELECT
                    SQL = " SELECT TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE , TO_DATE(JINDATE1,'YYYY-MM-DD') JINDATE1 , VCode";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_INSID";
                    SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "  AND PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND YYMM = '" + strYYMM + "'  ";
                    SQL = SQL + ComNum.VBLF + "  AND IPDOPD ='" + strIpdOpd + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND BI ='" + strBi + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND EDIMIRNO <>'0'";
                    SQL = SQL + ComNum.VBLF + "  AND BOHOJONG NOT IN ('1','2') ";
                    SQL = SQL + ComNum.VBLF + "  AND (GBMIR ='" + strGbMir + "' OR GBMIR IS NULL )  ";

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
                        strMirJindate = ComFunc.LeftH(strMirJindate, 10);
                    }

                    dt.Dispose();
                    dt = null;


                    SQL = " CREATE OR REPLACE VIEW  VIEW_MIR_DETAIL AS ";
                    if (strIpdOpd == "O")
                    {
                        SQL = SQL + ComNum.VBLF + "SELECT b.deptcode1 DEPTCODE, a.SuNext,c.SuNameK,'1' Gubun,A.BUN, A.GBGISUL, A.GBCHILD, SUM(a.Qty*a.Nal) Qty, ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "SELECT '' DEPTCODE, a.SuNext,c.SuNameK,'1' Gubun,A.BUN, A.GBGISUL, A.GBCHILD, SUM(a.Qty*a.Nal) Qty, ";
                    }
                    
                    SQL = SQL + ComNum.VBLF + " SUM(a.Amt) AMT, 0  DRUGAMT, A.RateBon , B.BOHUN, '' GBSLIP, b.VCODE ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_INSDTL a," + ComNum.DB_PMPA + "MIR_INSID b," + ComNum.DB_PMPA + "BAS_SUN c ";
                    SQL = SQL + ComNum.VBLF + " WHERE B.PANO ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND B.YYMM = '" + strYYMM + "'  ";
                    SQL = SQL + ComNum.VBLF + "  AND B.IPDOPD ='" + strIpdOpd + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND B.BI ='" + strBi + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND B.EDIMIRNO <>'0'";
                    SQL = SQL + ComNum.VBLF + "  AND B.BOHOJONG NOT IN ('1','2') ";
                    SQL = SQL + ComNum.VBLF + "  AND (B.GBMIR ='" + strGbMir + "' OR B.GBMIR IS NULL )  ";
                    SQL = SQL + ComNum.VBLF + "  AND A.SUNEXT NOT IN('########')";
                    SQL = SQL + ComNum.VBLF + "  AND B.WRTNO = A.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "  AND A.SuNext=c.SuNext(+) ";

                    if(strIpdOpd == "O")
                    {
                        SQL = SQL + ComNum.VBLF + " GROUP BY b.deptcode1, a.SuNext,c.SuNameK, A.BUN, A.GBGISUL, A.GBCHILD, A.RATEBON, B.BOHUN, b.VCode ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " GROUP BY a.SuNext,c.SuNameK, A.BUN, A.GBGISUL, A.GBCHILD, A.RATEBON, B.BOHUN, b.VCode ";
                    }
                    


                    SQL = SQL + ComNum.VBLF + "  UNION ALL ";
                    //    '약제 상한 발생
                    if(strIpdOpd == "O")
                    {
                        SQL = SQL + ComNum.VBLF + "SELECT b.deptcode1 DEPTCODE, 'BBBBBB' , '약가상한차액', '1' Gubun, '15' BUN , '0' GBGISUL, '0' GBCHILD , 1 QTY , ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "SELECT '' DEPTCODE, 'BBBBBB' , '약가상한차액', '1' Gubun, '15' BUN , '0' GBGISUL, '0' GBCHILD , 1 QTY , ";
                    }
                    
                    SQL = SQL + ComNum.VBLF + "SUM(A.EDIDRUGAMT)  AMT, 0 DRUGAMT, B.RateBon , B.BOHUN, '' GBSLIP, b.VCODE ";
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

                    if(strIpdOpd == "O")
                    {
                        SQL = SQL + ComNum.VBLF + " GROUP BY b.deptcode1, B.RATEBON, B.BOHUN, B.VCODE";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " GROUP BY B.RATEBON, B.BOHUN, B.VCODE";
                    }
                    

                    if (string.Compare(strYYMM, "200801") >= 0)
                    {

                        SQL = SQL + ComNum.VBLF + " UNION ALL ";
                        //'대불금
                        if (strIpdOpd == "O")
                        {
                            SQL = SQL + ComNum.VBLF + " SELECT b.deptcode1 DEPTCODE, '*대불금' SuNext, '대불금' SuNameK,'1' Gubun,'' BUN, '' GBGISUL, '' GBCHILD, 1 Qty, ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " SELECT '' DEPTCODE, '*대불금' SuNext, '대불금' SuNameK,'1' Gubun,'' BUN, '' GBGISUL, '' GBCHILD, 1 Qty, ";
                        }
                        
                        SQL = SQL + ComNum.VBLF + "        DAMT  AMT, 0 DRUGAMT,   0  RateBon ,  '' BOHUN, '' GBSLIP,  '' VCODE ";
                        SQL = SQL + ComNum.VBLF + " FROM MIR_INSID b ";
                        SQL = SQL + ComNum.VBLF + " WHERE B.PANO ='" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND B.YYMM = '" + strYYMM + "'  ";
                        SQL = SQL + ComNum.VBLF + "  AND B.IPDOPD ='" + strIpdOpd + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND B.BI ='" + strBi + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND B.EDIMIRNO <>'0'";
                        SQL = SQL + ComNum.VBLF + "  AND DAMT <> 0 ";
                        SQL = SQL + ComNum.VBLF + "  AND B.BOHOJONG NOT IN ('1','2') ";
                        SQL = SQL + ComNum.VBLF + "  AND (B.GBMIR ='" + strGbMir + "' OR B.GBMIR IS NULL )  ";
                        SQL = SQL + ComNum.VBLF + " UNION ALL ";

                        //        '장애기금
                        if (strIpdOpd == "O")
                        {
                            SQL = SQL + ComNum.VBLF + " SELECT b.deptcode1 DEPTCODE, '*장애기금' SuNext, '장애기금' SuNameK,'1' Gubun,'' BUN, '' GBGISUL, '' GBCHILD, 1 Qty, ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " SELECT '' DEPTCODE, '*장애기금' SuNext, '장애기금' SuNameK,'1' Gubun,'' BUN, '' GBGISUL, '' GBCHILD, 1 Qty, ";
                        }
                        
                        SQL = SQL + ComNum.VBLF + "        BOAMT  AMT, 0 DRUGAMT  , 0  RateBon ,  '' BOHUN, '' GBSLIP,  '' VCODE ";
                        SQL = SQL + ComNum.VBLF + " FROM MIR_INSID b ";
                        SQL = SQL + ComNum.VBLF + " WHERE B.PANO ='" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND B.YYMM = '" + strYYMM + "'  ";
                        SQL = SQL + ComNum.VBLF + "  AND B.IPDOPD ='" + strIpdOpd + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND B.BI ='" + strBi + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND B.EDIMIRNO <>'0'";
                        SQL = SQL + ComNum.VBLF + "  AND BOAMT <> 0 ";
                        SQL = SQL + ComNum.VBLF + "  AND B.BOHOJONG NOT IN ('1','2') ";
                        SQL = SQL + ComNum.VBLF + "  AND (B.GBMIR ='" + strGbMir + "' OR B.GBMIR IS NULL )  ";
                    }

                    SQL = SQL + ComNum.VBLF + "UNION ALL ";

                    //'외래/입원 저방금액은 병원가산율,소아가산 적용이 모두 된금액입니다.
                    //'그래서 GBGISUL, GBCHILD 값을 0으로 SETTING
                    if(strIpdOpd == "I" && strGbMir != "3") // 2018-12-11 김해수 NEW 소스 라디오 값 추가
                    {
                        if (strGbMir == "1")//Then '퇴원청구
                        {

                            SQL = SQL + ComNum.VBLF + "SELECT '' DEPTCODE, a.SuNext,b.SuNameK,'2' Gubun, A.BUN, '0' GBGISUL,'0' GBCHILD,  ";
                            SQL = SQL + ComNum.VBLF + "       SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt, 0 DRUGAMT , C.BONRATE RateBon, C.BOHUN , '' GBSLIP, C.VCode ";
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b , " + ComNum.DB_PMPA + "IPD_TRANS C ";
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
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK,A.BUN, C.BONRATE, C.BOHUN, C.VCode  ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "SELECT '' DEPTCODE, a.SuNext,b.SuNameK,'2' Gubun, A.BUN, '0' GBGISUL,'0' GBCHILD, ";
                            SQL = SQL + ComNum.VBLF + "       SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt, 0 DRUGAMT, C.BONRATE RateBon,  C.BOHUN , '' GBSLIP, C.VCode ";
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
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK, A.BUN, C.BONRATE, C.BOHUN, C.VCode ";
                        }
                    }
                    else
                    {
                        //'GBSLIP K는 인공신장, H는 혈우병
                        if (strGbMir == "3") //NEW 소스 추가 작업 김해수
                        {
                            SQL = SQL + ComNum.VBLF + "SELECT '' DEPTCODE, a.SuNext,b.SuNameK,'2' Gubun, A.BUN, '0' GBGISUL,'0' GBCHILD, ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "SELECT a.deptcode DEPTCODE, a.SuNext,b.SuNameK,'2' Gubun, A.BUN, '0' GBGISUL,'0' GBCHILD, ";
                        }
                        SQL = SQL + ComNum.VBLF + "   SUM(a.Qty*a.Nal) Amt,SUM(a.Amt1) Amt, 0 DRUGAMT,  0  RateBon, c.BOHUN , A.GBSLIP, C.VCode ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b, " + ComNum.DB_PMPA + "OPD_MASTER C  ";
                        SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                        SQL = SQL + ComNum.VBLF + "   AND a.Pano='" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND a.ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND a.ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND a.Bi='" + strBi + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND (a.YYMM = '" + VB.Right(strYYMM, 4) + "'  OR a.NU ='01' OR a.GbSelf ='0') ";// '조제료 때문에 조건을 or 로 gbself 체크함(jjy:2003-06-26)
                        SQL = SQL + ComNum.VBLF + "   AND a.amt1 <>'0' ";
                        SQL = SQL + ComNum.VBLF + "   AND a.Nu < '21' ";

                        if (strGbMir == "3") //NEW 소스 추가 작업 김해수
                        {
                            SQL = SQL + ComNum.VBLF + " AND a.GbSlip IN ('Z','Q','E') ";      //'응급실 6시간이상
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " AND nvl(a.GbSlip, ' ') NOT IN ('Z','Q','E') ";
                        }

                        SQL = SQL + ComNum.VBLF + "  AND A.PANO = C.PANO(+) ";
                        SQL = SQL + ComNum.VBLF + "  AND A.DEPTCODE = C.DEPTCODE(+)";
                        SQL = SQL + ComNum.VBLF + "  AND A.BDATE = C.BDATE (+)";
                        SQL = SQL + ComNum.VBLF + "  AND A.ACTDATE = C.ACTDATE (+)";
                        SQL = SQL + ComNum.VBLF + "  AND A.BI =C.BI(+) ";

                        SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";
                        if (strGbMir == "3") //NEW 소스 추가 작업 김해수
                        {
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.SuNext,b.SuNameK, A.BUN, A.GBSLIP, C.BOHUN, C.VCode ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "GROUP BY a.deptcode, a.SuNext,b.SuNameK, A.BUN, A.GBSLIP, C.BOHUN, C.VCode ";
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
                    SQL = SQL + ComNum.VBLF + " AMT, DRUGAMT , RateBon , BOHUN, GBSLIP, VCode ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "VIEW_MIR_DETAIL";
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

                        nRow = 1;
                        strOldData = "";
                        strSuNext = "";
                        nQty1 = 0;
                        nQty2 = 0;
                        nAmt1 = 0;
                        nAmt2 = 0;
                        strSunameK = "";

                        ssView1_Sheet1.Rows.Count = dt.Rows.Count;
                        ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strGubun = dt.Rows[i]["GUBUN"].ToString().Trim();
                            strBun = dt.Rows[i]["Bun"].ToString().Trim();
                            strGbGisul = dt.Rows[i]["GbGisul"].ToString().Trim();
                            strGbChild = dt.Rows[i]["GbChild"].ToString().Trim();
                            strRateBon = dt.Rows[i]["RateBon"].ToString().Trim();
                            strBohun = dt.Rows[i]["Bohun"].ToString().Trim();
                            nAmt = VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                            nDrugAmt = VB.Val(dt.Rows[i]["DRUGAMT"].ToString().Trim());
                            strGbSlip = dt.Rows[i]["GBSLIP"].ToString().Trim();
                            strVCode = dt.Rows[i]["VCODE"].ToString().Trim();
                            strNewData = dt.Rows[i]["SuNext"].ToString().Trim();

                            //if (strNewData == "BBBBBB")
                            //{
                            //    i = i;
                            //}

                            #region BONAMT_GESAN_NEW 
                            //GoSub 
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
                                                nRate = 22;
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

                                    switch (strNewData)
                                    {
                                        case "AS100":
                                        case "AS110":
                                        case "AS700":
                                        case "AS800":
                                        case "AS900":
                                        case "AS102":
                                        case "AS112":
                                        case "AS702":
                                        case "AS802":
                                        case "AS902":
                                        case "AS101":
                                        case "AS111":
                                        case "AS701":
                                        case "AS801":
                                        case "AS901":
                                        case "AS200":
                                        case "F02T":
                                            nRate = 10;
                                            break;
                                    }
                                    break;
                                case "72":
                                case "73":
                                    if(strGubun == "1")
                                    {
                                        nRate_I = Convert.ToDouble(strRateBon);
                                        nRate_O = Convert.ToDouble(strRateBon);    
                                    }
                                    //nRate = nRate_O ;
                                    nRate = nRate_O / 100;
                                    break;
                                default:
                                    if (strRateBon == "0")// '외래/입원 실처방
                                    {
                                        if (strIpdOpd == "I")
                                        {
                                            VB.IIf(strVCode == "V193", "10", nRate_I);
                                            nRate = nRate_I;
                                        }                                      
                                        else
                                            nRate = nRate_O;
                                    }  
                                    else
                                        nRate = VB.Val(strRateBon);

                                    if (nRate != 0 && (strGbSlip == "K" || strGbSlip == "G" || strGbSlip == "F"))
                                        //nRate = nRate_I;
                                        nRate = nRate / 100;
                                    break;
                            }

                            if (strGbSlip == "V")
                                nRate = 10;
                            if (strGbSlip == "R")
                                nRate = 35;// '소아(6세미만) 35
                            if (strGbSlip == "U")
                                nRate = 15;// '소아(6세미만) 15

                            if (strNewData == "BBBBBB")
                                nRate = 0;
                            if (string.Compare(strYYMM, "200801") < 0 && strBohun == "3")
                                nRate = 0;

                            nRate = nRate / 100; 

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
                            nJohapAmt = nAmt - nBonAmt;// 50-0.1

                            #endregion

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
                                nQty1 = nQty1 +Convert.ToDouble(dt.Rows[i]["QTY"].ToString().Trim());
                                nAmt1 = Convert.ToDouble(nAmt1 + nJohapAmt);  //0+49.9
                            }
                            else
                            {
                                nQty2 = nQty2 + Convert.ToDouble(dt.Rows[i]["QTY"].ToString().Trim());
                                nAmt2 = Convert.ToDouble(nAmt2 + nJohapAmt);
                            }
                        }
                        Display_SUB(ref nRow, ref nTotAmt1, ref nTotAmt2);
                       
                        nTotAmt2 = nTotAmt2 + nAmt3;
                        nTotAmt2 = (int)(nTotAmt2 / 10) * 10;

                        ssView1_Sheet1.RowCount = nRow;
                        ssView1_Sheet1.Cells[0, 3].Text = VB.Format(nTotAmt1, "###,###,###,##0 ");
                        ssView1_Sheet1.Cells[0, 4].Text = VB.Format(nTotAmt2, "###,###,###,##0 ");
                        ssView1_Sheet1.Cells[0, 5].Text = VB.Format((nTotAmt2 - nTotAmt1), "###,###,###,##0 ");
                        ssView1_Sheet1.Cells[0, 6].Text = " ** 합 계 **";
                    
                        dt.Dispose();
                        dt = null;

                        //SQL = "DROP VIEW VIEW_MIR_DETAIL";

                        //SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        //if (SqlErr != "")
                        //{
                        //    clsDB.setRollbackTran(clsDB.DbCon);
                        //    ComFunc.MsgBox(SqlErr);
                        //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                        //    return;
                        //}
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

        public void reScreen_Display2(string gstr)
        {

            strTemp = gstr;

            GstrWrtno = VB.Val(VB.Pstr(gstr, ",", 1)).ToString();
            GstrPano = VB.Pstr(gstr, ",", 3);
            GstrSname = VB.Pstr(gstr, ",", 4);
            GstrYYMM = VB.Pstr(gstr, ",", 7);
            GstrIO = VB.Pstr(gstr, ",", 8);
            ssView1.ActiveSheet.RowCount = 0;
            Screen_Display2();

            setTimer();
        } //김해수 2018-11-16

        private void Display_SUB(ref int nRow, ref double nTotAmt1, ref double nTotAmt2)
        {
            if (nQty1 == 0 && nQty2 == 0 && nAmt1 == 0 && nAmt2 == 0)
            {
                return;
            }

            nRow = nRow + 1;
            if (nRow > ssView1_Sheet1.RowCount)
            {
                ssView1_Sheet1.RowCount = nRow;
            }

            ssView1_Sheet1.Cells[nRow - 1, 0].Text = strSuNext;
            ssView1_Sheet1.Cells[nRow - 1, 1].Text = VB.Format(nQty1, "###,###,##0.0");
            ssView1_Sheet1.Cells[nRow - 1, 2].Text = VB.Format(nQty2, "###,###,##0.0");
            ssView1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nAmt1, "###,###,###,##0");
            ssView1_Sheet1.Cells[nRow - 1, 4].Text = VB.Format(nAmt2, "###,###,###,##0");
            ssView1_Sheet1.Cells[nRow - 1, 5].Text = VB.Format((nAmt2 - nAmt1), "###,###,###,###");

            ssView1_Sheet1.Cells[nRow - 1, 6].Text = strSunameK;

            nTotAmt1 = nTotAmt1 + nAmt1;
            nTotAmt2 = nTotAmt2 + nAmt2;

            nSTotAmt1 = nSTotAmt1 + nAmt1;
            nSTotAmt2 = nSTotAmt2 + nAmt2;
            
        }

        private void btnExit_Click(object sender, EventArgs e) //김해수 2018-11-16
        {
            foreach (Form frm2 in Application.OpenForms) //중복로드 방지
            {
                if (frm2.Name == "frmPmpaViewSlip1")
                {
                    frm2.Close();
                    break;
                }
            }
            this.Close();
            return;
        }

        private void btnIView_Click(object sender, EventArgs e)
        {
            show_form();
        }

        void show_form()
        {
            Cursor.Current = Cursors.WaitCursor;

            foreach (Form frm2 in Application.OpenForms) //중복로드 방지
            {
                if (frm2.Name == "frmPmpaViewSlip1") 
                {
                    frm2.Visible = true;
                    frm2.Activate();
                    frmPmpaViewSlip1S.RE_READ_DATE(GstrPano);
                    return;
                }
            }
            frmPmpaViewSlip1S = new frmPmpaViewSlip1(GstrPano);
            frmPmpaViewSlip1S.StartPosition = FormStartPosition.Manual;
            frmPmpaViewSlip1S.Location = new Point(0, 50);
            frmPmpaViewSlip1S.Show();

            Cursor.Current = Cursors.Default;

        } //김해수 2018-11-16

        void setTimer()
        {
            t1 = new Timer();
            t1.Enabled = true;
            t1.Interval = 100;
            t1.Tick += new EventHandler(eTimer);
        } //김해수 2018-11-16

        void eTimer(object sender, EventArgs e)
        {
            show_form(); //폼 show   

            t1.Enabled = false;
            t1.Dispose();
        } //김해수 2018-11-16
        

    }
}