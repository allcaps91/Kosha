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
    /// File Name       : frmPmpaViewIpgumReport.cs
    /// Description     : 산재 진료비 입금 내역
    /// Author          : 박창욱
    /// Create Date     : 2017-09-13
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// FrmIpgumReport2를 FrmIpgumReport와 통합
    /// </history>
    /// <seealso cref= "\misu\MISUS204.FRM(FrmIpgumReport.frm) >> frmPmpaViewIpgumReport.cs 폼이름 재정의" />	
    /// <seealso cref= "\misu\MISUS208.FRM(FrmIpgumReport2.frm) >> frmPmpaViewIpgumReport.cs 폼이름 재정의" />	
    public partial class frmPmpaViewIpgumReport : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsPmpaFunc cpf = new clsPmpaFunc();
        clsPmpaMisu cpm = new clsPmpaMisu();

        public frmPmpaViewIpgumReport()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (chkFlag.Checked == false)
            {
                if (cboFYYMM.Text != cboTYYMM.Text)
                {
                    strTitle = VB.Left(cboFYYMM.Text, 9) + "-" + VB.Left(cboTYYMM.Text, 9) + " 산재 진료비 입금 현황";
                }
                else
                {
                    strTitle = VB.Left(cboFYYMM.Text, 9) + " 산재 진료비 입금 현황";
                }
            }
            else
            {
                strTitle = VB.Left(cboYYMM.Text, 9) + " 산재 진료비 입금 현황";
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력자: " + clsType.User.JobName + " 인 ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strYYMM = "";
            string strFYYMM = "";
            string strTYYMM = "";

            string strFDate = "";
            string strTDate = "";
            int nRead = 0;

            string strCoIPDOPD = "";
            string strCoGel = "";
            string strCoWrtno = "";
            string strFirst = "";
            int nCnt = 0;
            int nScnt = 0;
            int nTcnt = 0;
            double nSTAmt = 0;
            double nSMAmt = 0;
            double nSIAmt = 0;
            double nSSAmt = 0;
            double nSJAmt = 0;
            double nTTAmt = 0;
            double nTMAmt = 0;
            double nTIAmt = 0;
            double nTSAmt = 0;
            double nTJAmt = 0;

            int nOTcnt = 0;
            double nOTTAmt = 0;
            double nOTMAmt = 0;
            double nOTIAmt = 0;
            double nOTSAmt = 0;
            double nOTJAmt = 0;

            int nITcnt = 0;
            double nITTAmt = 0;
            double nITMAmt = 0;
            double nITIAmt = 0;
            double nITSAmt = 0;
            double nITJAmt = 0;

            string strIpdOpd = "";
            string strWrtno = "";
            string strPano = "";
            string strSName = "";
            string strGelName = "";
            string strGelCode = "";
            string strDeptcode = "";
            string strDate1 = "";
            string strDate2 = "";
            string strDate3 = "";
            string strDate4 = "";
            //double nIMAmt = 0;   //청구액(외래)
            //double nOMAmt = 0;   //청구액(입원)
            double nTAmt = 0;   //입금완료분 청구액
            double nMAmt = 0;   //청구액
            double nSAmt = 0;   //삭감액
            double nIAmt = 0;   //회입액
            double nJamt = 0;   //현잔액

            string strFlag = "";

            ssView_Sheet1.RowCount = 0;

            nSMAmt = 0;
            nSIAmt = 0;
            nSSAmt = 0;
            nSJAmt = 0;
            nSTAmt = 0;
            nTMAmt = 0;
            nTIAmt = 0;
            nTSAmt = 0;
            nTJAmt = 0;
            nTTAmt = 0;
            nOTcnt = 0;
            nOTMAmt = 0;
            nOTIAmt = 0;
            nOTSAmt = 0;
            nOTJAmt = 0;
            nOTTAmt = 0;
            nITcnt = 0;
            nITMAmt = 0;
            nITIAmt = 0;
            nITSAmt = 0;
            nITJAmt = 0;
            nITTAmt = 0;

            if (chkFlag.Checked == false) //일자별 조회 분기
            {
                strYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 7, 2);
                strFYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 7, 2);
                strTYYMM = VB.Left(cboTYYMM.Text, 4) + VB.Mid(cboTYYMM.Text, 7, 2);
                strFDate = VB.Left(strFYYMM, 4) + "-" + VB.Right(strFYYMM, 2) + "-01";
                strTDate = VB.Left(strTYYMM, 4) + "-" + VB.Right(strTYYMM, 2) + "-01";
                strTDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(strTYYMM, 4)), (int)VB.Val(VB.Right(strTYYMM, 2)));
            }
            else
            {
                strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
                strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
                strTDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(strFDate, 4)), (int)VB.Val(VB.Mid(strFDate, 6, 2)));
            }



            try
            {
                //해당월 마감여부 Checking

                SQL = "";
                if (chkFlag.Checked == false) //일자별 조회 분기
                {
                    SQL = SQL + ComNum.VBLF + " SELECT Count(*) Cnt";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_MONTHLY";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND YYMM >= '" + strFYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "    AND YYMM <= '" + strTYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "    AND Class = '05'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " SELECT Count(*) Cnt";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_MONTHLY";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND YYMM = '" + strYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "    AND Class = '05'";
                }

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
                    ComFunc.MsgBox("해당월은 통계가 형성되어있지 않습니다.");
                    return;
                }
                if (dt.Rows[0]["Cnt"].ToString().Trim() == "0")
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당월은 통계가 형성되어있지 않습니다.");
                    return;
                }
                dt.Dispose();
                dt = null;


                SQL = "";
                if (chkFlag.Checked == false) //일자별 조회 분기
                {
                    SQL = SQL + ComNum.VBLF + " SELECT WRTNO, GelCode, IpdOpd,  ";
                    SQL = SQL + ComNum.VBLF + "        MisuID, TO_CHAR(Bdate,'YYYY-MM-DD') IDate, TO_CHAR(Bdate,'YYYYMM') YYMM, ";
                    SQL = SQL + ComNum.VBLF + "        SUM(Amt) IAmt ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND Class = '05' ";
                    SQL = SQL + ComNum.VBLF + "    AND Gubun >= '21' ";
                    SQL = SQL + ComNum.VBLF + "    AND Gubun <= '29' ";

                    if (rdoIO1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND IPDOPD = 'I' ";
                    }
                    else if (rdoIO2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND IPDOPD = 'O' ";
                    }
                    SQL = SQL + ComNum.VBLF + "  GROUP BY WRTNO, GelCode, IpdOpd, MisuID, Bdate ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY 2, 3, 4, 5 ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " SELECT WRTNO, GelCode, IpdOpd, ";
                    SQL = SQL + ComNum.VBLF + "        MisuID, TO_CHAR(Bdate,'YYYY-MM-DD') IDate, SUM(Amt) IAmt";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND Class = '05' ";
                    SQL = SQL + ComNum.VBLF + "    AND Gubun >= '21' ";
                    SQL = SQL + ComNum.VBLF + "    AND Gubun <= '29' ";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY WRTNO, GelCode, IpdOpd, MisuID, Bdate ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY 5, 2, 3, 4 ";
                }


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                strFirst = "OK";
                nRead = dt.Rows.Count;

                if (chkFlag.Checked == true)
                {
                    strFlag = dt.Rows[0]["IDATE"].ToString().Trim();
                }

                for (i = 0; i < nRead; i++)
                {
                    strWrtno = dt.Rows[i]["Wrtno"].ToString().Trim();

                    //월말 현재 잔액을 READ
                    SQL = "";
                    if (chkFlag.Checked == false) //일자별 조회 분기
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT JanAmt FROM MISU_Monthly   ";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND WRTNO = '" + strWrtno + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND YYMM = '" + dt.Rows[i]["YYMM"].ToString().Trim() + "' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT JanAmt FROM MISU_Monthly   ";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND WRTNO = '" + strWrtno + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND YYMM = '" + strYYMM + "' ";
                    }

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    nJamt = 0;
                    if (dt1.Rows.Count == 1)
                    {
                        nJamt = VB.Val(dt1.Rows[0]["JanAmt"].ToString().Trim());
                    }
                    dt1.Dispose();
                    dt1 = null;
                    cpm.READ_MISU_IDMST((long)VB.Val(strWrtno));

                    strIpdOpd = clsPmpaType.TMM.IpdOpd;
                    strPano = clsPmpaType.TMM.MisuID;
                    strSName = cpf.Get_BasPatient(clsDB.DbCon, clsPmpaType.TMM.MisuID.Trim()).Rows[0]["Sname"].ToString().Trim();
                    strGelCode = clsPmpaType.TMM.GelCode;
                    strDeptcode = clsPmpaType.TMM.DeptCode;
                    strDate1 = dt.Rows[i]["IDate"].ToString().Trim();   //입금일
                    strDate2 = clsPmpaType.TMM.BDate;           //청구일
                    strDate3 = clsPmpaType.TMM.FromDate;        //진료기간(FROM)
                    strDate4 = clsPmpaType.TMM.ToDate;          //진료기간(TO)
                    nMAmt = clsPmpaType.TMM.Amt[2];         //청구액
                    nIAmt = VB.Val(dt.Rows[i]["IAmt"].ToString().Trim());   //입금액
                    nSAmt = 0;
                    if (nITJAmt == 0)
                    {
                        nTAmt = clsPmpaType.TMM.Amt[2]; //청구액(입금완료분)
                        nSAmt = clsPmpaType.TMM.Amt[4]; //삭감액
                    }

                    //일자별 조회 분기
                    if (chkFlag.Checked == false)
                    {
                        if (clsPmpaType.TMM.IpdOpd == "O")
                        {
                            strIpdOpd = "외래";
                        }
                        if (clsPmpaType.TMM.IpdOpd == "I")
                        {
                            strIpdOpd = "입원";
                        }
                    }

                    strGelName = "";
                    if (strFirst == "OK")
                    {
                        strCoGel = strGelCode;
                        strCoIPDOPD = strIpdOpd;
                        strFirst = "NO";
                        strGelName = cpm.READ_BAS_MIA(strGelCode);
                        nCnt = 0;
                        nScnt = 0;
                        nTcnt = 0;
                    }

                    if (strGelCode != strCoGel)
                    {
                        Misu_Sub_IPDOPD_Rtn(strCoIPDOPD, ref strCoGel, strGelCode, ref nCnt, ref nSMAmt,
                                 ref nSIAmt, ref nSSAmt, ref nSTAmt, ref nSJAmt);
                        Misu_Sub_Rtn(strCoIPDOPD, ref strCoGel, strGelCode, ref nScnt, ref nTMAmt,
                           ref nTIAmt, ref nTSAmt, nSTAmt, nSSAmt, ref nTTAmt, ref nTJAmt);
                        strCoIPDOPD = strIpdOpd;
                        strGelName = cpm.READ_BAS_MIA(strGelCode);
                    }

                    //일자별 조회 분기
                    if (chkFlag.Checked == false)
                    {
                        if (strIpdOpd != strCoIPDOPD)
                        {
                            Misu_Sub_IPDOPD_Rtn(strCoIPDOPD, ref strCoGel, strGelCode, ref nCnt, ref nSMAmt,
                                     ref nSIAmt, ref nSSAmt, ref nSTAmt, ref nSJAmt);
                            strCoIPDOPD = strIpdOpd;
                        }
                    }
                    else
                    {
                        if (strFlag != dt.Rows[i]["IDATE"].ToString().Trim())
                        {
                            Misu_Sub_IPDOPD_Rtn(strCoIPDOPD, ref strCoGel, strGelCode, ref nCnt, ref nSMAmt,
                                     ref nSIAmt, ref nSSAmt, ref nSTAmt, ref nSJAmt, strFlag);
                            strFlag = dt.Rows[i]["IDATE"].ToString().Trim();
                        }
                    }

                    if (strCoWrtno != strWrtno)
                    {
                        strCoWrtno = strWrtno;
                        ssView_Sheet1.RowCount += 1;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strGelName;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = strDate1;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = strDate2;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = VB.DateDiff("D", Convert.ToDateTime(strDate2), Convert.ToDateTime(strDate1)).ToString();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = strIpdOpd;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = strPano;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = strSName;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = strDate3 + "-" + strDate4;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = strDeptcode;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nMAmt.ToString();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nIAmt.ToString();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nSAmt.ToString();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = nJamt.ToString();
                        nCnt += 1;
                        nScnt += 1;
                        nTcnt += 1;

                        nSTAmt += nTAmt;
                        nSMAmt += nMAmt;
                        nSIAmt += nIAmt;
                        nSSAmt += nSAmt;
                        nSJAmt += nJamt;

                        nTTAmt += nTAmt;
                        nTMAmt += nMAmt;
                        nTIAmt += nIAmt;
                        nTSAmt += nSAmt;
                        nTJAmt += nJamt;

                        if (strIpdOpd == "입원")
                        {
                            nITcnt += 1;
                            nITTAmt += nTAmt;
                            nITMAmt += nMAmt;
                            nITIAmt += nIAmt;
                            nITSAmt += nSAmt;
                            nITJAmt += nJamt;
                        }
                        else
                        {
                            nOTcnt += 1;
                            nOTTAmt += nTAmt;
                            nOTMAmt += nMAmt;
                            nOTIAmt += nIAmt;
                            nOTSAmt += nSAmt;
                            nOTJAmt += nJamt;
                        }
                    }

                }

                dt.Dispose();
                dt = null;

                Misu_Sub_IPDOPD_Rtn(strCoIPDOPD, ref strCoGel, strGelCode, ref nCnt, ref nSMAmt,
                                 ref nSIAmt, ref nSSAmt, ref nSTAmt, ref nSJAmt);
                Misu_Sub_Rtn(strCoIPDOPD, ref strCoGel, strGelCode, ref nScnt, ref nTMAmt,
                           ref nTIAmt, ref nTSAmt, nSTAmt, nSSAmt, ref nTTAmt, ref nTJAmt);
                Misu_Total_Rtn(nOTcnt, nOTMAmt, nOTIAmt, nOTSAmt, nOTTAmt, nOTJAmt,
                            nITcnt, nITMAmt, nITIAmt, nITSAmt, nITTAmt, nITJAmt);

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Misu_Sub_IPDOPD_Rtn(string strCoIPDOPD, ref string strCoGel, string strGelCode, ref int nCnt, ref double nSMAmt,
                                 ref double nSIAmt, ref double nSSAmt, ref double nSTAmt, ref double nSJAmt, string strFlag = "")
        {
            ssView_Sheet1.RowCount += 1;
            if (chkFlag.Checked == true) //일자별 조회 분기
            {
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = strFlag;
            }
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = strCoIPDOPD + "소계";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nCnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nSMAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nSIAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nSSAmt.ToString();

            //2020-06-11 KHS
            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 255, 161);

            if (nSTAmt != 0 && nSSAmt != 0)
            {
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = (nSSAmt / nSTAmt * 100).ToString("#0.00") + "%";
            }
            nCnt = 0;
            nSTAmt = 0;
            nSMAmt = 0;
            nSIAmt = 0;
            nSSAmt = 0;
            nSJAmt = 0;
            strCoGel = strGelCode;

            return;
        }

        void Misu_Sub_Rtn(string strCoIPDOPD, ref string strCoGel, string strGelCode, ref int nScnt, ref double nTMAmt,
                           ref double nTIAmt, ref double nTSAmt, double nSTAmt, double nSSAmt, ref double nTTAmt, ref double nTJAmt)
        {
            ssView_Sheet1.RowCount += 1;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "소계";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nScnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nTMAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nTIAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nTSAmt.ToString();
            if (nTTAmt != 0 && nTSAmt != 0)
            {
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = (nTSAmt / nTTAmt * 100).ToString("#0.00") + "%";
            }
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = nTJAmt.ToString();

            //2020-06-11 KHS
            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(170, 255, 163);

            nScnt = 0;
            nTTAmt = 0;
            nTMAmt = 0;
            nTSAmt = 0;
            nTIAmt = 0;
            nTJAmt = 0;
            strCoGel = strGelCode;
        }

        void Misu_Total_Rtn(int nOTcnt, double nOTMAmt, double nOTIAmt, double nOTSAmt, double nOTTAmt, double nOTJAmt,
                            double nITcnt, double nITMAmt, double nITIAmt, double nITSAmt, double nITTAmt, double nITJAmt)
        {
            if (chkFlag.Checked == false) //일자별 조회 분기
            {
                ssView_Sheet1.RowCount += 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "외     래";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nOTcnt + " 건";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nOTMAmt.ToString();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nOTIAmt.ToString();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nOTSAmt.ToString();
                if (nOTTAmt != 0 && nOTSAmt != 0)
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = (nOTSAmt / nOTTAmt * 100).ToString("#0.00") + "%";
                }
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = nOTJAmt.ToString();
                ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(146, 255, 255);


                ssView_Sheet1.RowCount += 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "입     원";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nITcnt + " 건";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nITMAmt.ToString();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nITIAmt.ToString();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nITSAmt.ToString();
                if (nITSAmt != 0 && nITTAmt != 0)
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = (nITSAmt / nITTAmt * 100).ToString("#0.00") + "%";
                }
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = nITJAmt.ToString();
                ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(146, 255, 255);
            }

            ssView_Sheet1.RowCount += 1;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "전     체";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = (nOTcnt + nITcnt) + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = (nOTMAmt + nITMAmt).ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = (nOTIAmt + nITIAmt).ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = (nOTSAmt + nITSAmt).ToString();
            if ((nOTSAmt + nITSAmt) != 0 && (nOTTAmt + nITTAmt) != 0)
            {
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = ((nOTSAmt + nITSAmt) / (nOTTAmt + nITTAmt) * 100).ToString("#0.00") + "%";
            }
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = (nOTJAmt + nITJAmt).ToString();

            //2020-06-11 KHS
            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(146,255,255);
        }

        private void frmPmpaViewIpgumReport_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //this.Close(); //폼 권한 조회
            //return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int nMM = 0;
            int i = 0;
            string strSysDate = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            nYY = (int)VB.Val(VB.Left(strSysDate, 4));
            nMM = (int)VB.Val(VB.Mid(strSysDate, 6, 2));

            for (i = 1; i < 37; i++)
            {
                cboFYYMM.Items.Add(nYY.ToString("0000") + "년 " + nMM.ToString("00") + "월분");
                cboTYYMM.Items.Add(nYY.ToString("0000") + "년 " + nMM.ToString("00") + "월분");
                cboYYMM.Items.Add(nYY.ToString("0000") + "년 " + nMM.ToString("00") + "월분");
                nMM -= 1;
                if (nMM == 0)
                {
                    nMM = 12;
                    nYY -= 1;
                }
            }

            cboYYMM.SelectedIndex = 0;
            cboFYYMM.SelectedIndex = 0;
            cboTYYMM.SelectedIndex = 0;
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
        }

        private void chkFlag_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFlag.Checked == true)
            {
                lblTitleSub0.Text = "일자별 조회 조건";
                lblTitleSub1.Text = "일자별 조회 결과";
                grbDate.Visible = false;
                grbDate2.Visible = true;
                grbIO.Visible = false;
            }
            else
            {
                lblTitleSub0.Text = "일반 조회 조건";
                lblTitleSub1.Text = "일반 조회 결과";
                grbDate.Visible = true;
                grbDate2.Visible = false;
                grbIO.Visible = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
