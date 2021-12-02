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
    /// File Name       : frmPmpaViewSanSakPrint.cs
    /// Description     : 산재 진료비 삭감분석
    /// Author          : 박창욱
    /// Create Date     : 2017-09-15
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// FrmSakPrint와 FrmSakPrintdtl 통합
    /// </history>
    /// <seealso cref= "\misu\MISUS205.FRM(FrmSakPrint.frm) >> frmPmpaViewSanSakPrint.cs 폼이름 재정의" />	
    /// <seealso cref= "\misu\MISUS209.FRM(FrmSakPrintdtl.frm) >> frmPmpaViewSanSakPrint.cs 폼이름 재정의" />	
    public partial class frmPmpaViewSanSakPrint : Form
    {
        string[] GstrGels = new string[31];

        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsPmpaMisu cpm = new clsPmpaMisu();
        clsPmpaFunc cpf = new clsPmpaFunc();

        public frmPmpaViewSanSakPrint()
        {
            InitializeComponent();
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
            }     //권한확인

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            string PrintDate = "";
            string Jname = "";
            string strGubun = "";
            bool PrePrint = true;

            PrintDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");
            Jname = clsType.User.JobName;

            if (chkDate.Checked == false)
            {
                if (rdoIO1.Checked == true)
                {
                    strGubun = "구분 : 입원 ";
                }
                if (rdoIO2.Checked == true)
                {
                    strGubun = "구분 : 외래 ";
                }

                strTitle = "산재 진료비 삭감 현황";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String(strGubun + "출력시간 : " + PrintDate, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            else
            {
                strTitle = "일자별 산재 진료비 삭감 현황 [" + dtpFDate.Value.ToString("yyyy-MM-dd") + "~" + dtpTDate.Value.ToString("yyyy-MM-dd") + "]";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("출력시간 : " + PrintDate, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }


            strHeader += CS.setSpdPrint_String("출력자 : " + Jname + " 인", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            if (chkDate.Checked == false)
            {
                CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else
            {
                CS.setSpdPrint(ssView2, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            if (chkDate.Checked == false)
            {
                SearchData();
            }
            else
            {
                SearchData_Daily();
            }

        }

        void SearchData()
        {
            int i = 0;
            string strFYYMM = "";
            string strTYYMM = "";

            string strOldData = "";
            string strNewData = "";
            string strIDate = "";
            int nRead = 0;
            long nWRTNO = 0;
            double nSakRATE = 0;
            int nRow = 0;

            long nDaySum = 0;
            long nSakSum2 = 0;
            long nSakSum3 = 0;
            long nSakSum4 = 0;

            //string strIO2 = "";
            int nQty1 = 0;
            int nQty2 = 0;

            nDaySum = 0;
            nSakSum2 = 0;
            nSakSum3 = 0;
            nSakSum4 = 0;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            strFYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 7, 2);
            strTYYMM = VB.Left(cboTYYMM.Text, 4) + VB.Mid(cboTYYMM.Text, 7, 2);

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                //해당월 마감여부 Checking
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Count(*) Cnt ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_MONTHLY";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND YYMM >= '" + strFYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND YYMM <= '" + strTYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND Class = '05'";



                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows[0]["CNT"].ToString().Trim() == "0")
                {
                    ComFunc.MsgBox("해당월의 통계가 형성되어있지 않습니다.");
                }

                dt.Dispose();
                dt = null;

                btnSearch.Enabled = false;
                btnPrint.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;

                //해당월 중 입금완료자 SELECT
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO, a.GelCode, b.Bdate, ";
                SQL = SQL + ComNum.VBLF + "        a.JanAmt, a.misuamt, a.sakamt,";
                SQL = SQL + ComNum.VBLF + "        b.deptcode, b.drcode, b.amt2,";
                SQL = SQL + ComNum.VBLF + "        b.qty1, b.amt4, b.qty3";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_MONTHLY a, " + ComNum.DB_PMPA + "MISU_IDMST b";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM >= '" + strFYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM <= '" + strTYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND a.Class = '05'";
                if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND a.IPDOPD = 'I' ";
                }
                if (rdoIO2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND a.IPDOPD = 'O' ";
                }

                if (chkTong.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.JanAmt < 1";
                }
                SQL = SQL + ComNum.VBLF + "    AND a.WRTNO = b.WRTNO";
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.yymm, b.misuid, a.GelCode, b.Bdate";
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
                nRow = 0;
                nRead = dt.Rows.Count;

                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = nRead;

                for (i = 0; i < nRead; i++)
                {
                    nWRTNO = (long)VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim());
                    if (i == 98)
                    {
                        //i = i;
                    }
                    cpm.READ_MISU_IDMST(nWRTNO);
                    nSakSum2 = nSakSum2 + (long)clsPmpaType.TMM.Amt[2];
                    nSakSum3 = nSakSum3 + (long)clsPmpaType.TMM.Amt[3];
                    nSakSum4 = nSakSum4 + (long)clsPmpaType.TMM.Amt[4];
                    nSakRATE = 0;
                    if (clsPmpaType.TMM.Amt[2] != 0 && clsPmpaType.TMM.Amt[4] != 0)
                    {
                        nSakRATE = clsPmpaType.TMM.Amt[4] / clsPmpaType.TMM.Amt[2] * 100;
                    }
                    if (nSakRATE >= VB.Val(txtYul.Text))
                    {
                        //1건의 삭감내역을 Display
                        strNewData = clsPmpaType.TMM.GelCode.Trim();
                        nRow += 1;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }
                        if (strOldData != strNewData)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 0].Text = cpm.READ_BAS_MIA(strNewData);
                            strOldData = strNewData;
                        }

                        ssView_Sheet1.Cells[nRow - 1, 1].Text = clsPmpaType.TMM.MisuID;

                        //2020-06-11 KHS 공백처리 
                        if(cpf.Get_BasPatient(clsDB.DbCon, clsPmpaType.TMM.MisuID).Rows.Count == 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = "";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = cpf.Get_BasPatient(clsDB.DbCon, clsPmpaType.TMM.MisuID).Rows[0]["Sname"].ToString().Trim();
                        }
                        

                        if (clsPmpaType.TMM.IpdOpd == "I")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 3].Text = "입원";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[nRow - 1, 3].Text = "외래";
                        }
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = clsPmpaType.TMM.DeptCode;

                        //2020-06-11 KHS 공백인것 처리
                        if(cpf.READ_DOCTOR_NAME(clsDB.DbCon, clsPmpaType.TMM.DrCode) == "")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = "";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = cpf.READ_DOCTOR_NAME(clsDB.DbCon, clsPmpaType.TMM.DrCode);
                        }
                        
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = clsPmpaType.TMM.FromDate + "-" + clsPmpaType.TMM.ToDate;
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = clsPmpaType.TMM.BDate;


                        //최종 입금일자 구하기
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(Bdate,'YYYY-MM-DD') IDate";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND WRTNO = " + nWRTNO + "";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY Bdate DESC";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            btnSearch.Enabled = true;
                            btnPrint.Enabled = true;
                            Cursor.Current = Cursors.Default;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        strIDate = dt1.Rows[0]["IDate"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = strIDate;
                        ssView_Sheet1.Cells[nRow - 1, 9].Text = VB.DateDiff("D", Convert.ToDateTime(clsPmpaType.TMM.BDate), Convert.ToDateTime(strIDate)).ToString("###0 ");
                        nDaySum += (long)VB.Val(ssView_Sheet1.Cells[nRow - 1, 9].Text.Trim());
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = clsPmpaType.TMM.Amt[2].ToString("###,###,###,##0 ");   //청구액
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = clsPmpaType.TMM.Amt[3].ToString("###,###,###,##0 ");   //입금액
                        ssView_Sheet1.Cells[nRow - 1, 12].Text = clsPmpaType.TMM.Amt[4].ToString("###,###,###,##0 ");   //삭감액
                        ssView_Sheet1.Cells[nRow - 1, 13].Text = nSakRATE.ToString("###0.00") + "(%) ";  //삭감율
                        dt1.Dispose();
                        dt1 = null;
                    }

                    if (rdoIO1.Checked == true)
                    {
                        //strIO2 = "I";
                    }
                    else if (rdoIO2.Checked == true)
                    {
                        //strIO2 = "O";
                    }
                    else
                    {
                        if (chkTong.Checked == true)
                        {
                            ComFunc.MsgBox("통계형성시 외래입원 구분 필요");
                            return;
                        }
                    }

                    if (chkTong.Checked == true)
                    {
                        nQty1 = (int)VB.Val(dt.Rows[i]["qty1"].ToString().Trim());
                        nQty2 = (int)VB.Val(dt.Rows[i]["qty3"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount += 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = " **** 합계 *****";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nDaySum.ToString("###,###,##0 ");         //미수일계
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nSakSum2.ToString("###,###,###,##0 ");   //청구액
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nSakSum3.ToString("###,###,###,##0 ");   //입금액
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = nSakSum4.ToString("###,###,###,##0 ");   //석검약
                nSakRATE = ((float)nSakSum4 / (float)nSakSum2) * 100;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = nSakRATE.ToString("##0.00") + "(%) ";    //삭감율

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

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
                return;
            }
        }

        void SearchData_Daily()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            string strFDate = "";
            string strTDate = "";
            long nIlsu = 0;
            double nAmt2 = 0;
            double nAmt3 = 0;
            double nAmt4 = 0;
            double nAmt5 = 0;

            nIlsu = 0;
            nAmt2 = 0;
            nAmt3 = 0;
            nAmt4 = 0;

            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");

            ssView2_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 1;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE , A.MISUID, A.IPDOPD,";
                SQL = SQL + ComNum.VBLF + "        B.SNAME, A.DEPTCODE, TO_CHAR(A.FROMDATE,'YYYY-MM-DD') FROMDATE,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(A.ToDate,'YYYY-MM-DD') TODATE, A.Ilsu, A.AMT2,";
                SQL = SQL + ComNum.VBLF + "        A.AMT3, A.AMT4, A.AMT5 ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_IDMST A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND A.CLASS ='05'"; //산재
                SQL = SQL + ComNum.VBLF + "    AND A.BDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND A.BDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.IPDOPD='I'";
                }
                if (rdoIO2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.IPDOPD='O'";
                }
                SQL = SQL + ComNum.VBLF + "    AND A.MISUID = B.PANO";
                SQL = SQL + ComNum.VBLF + "  ORDER BY A.MISUID, A.BDATE";

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
                    ComFunc.MsgBox("해당 일자의 DATA가 없습니다.");
                    return;
                }

                btnSearch.Enabled = false;
                btnPrint.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;

                nRead = dt.Rows.Count;

                ssView2_Sheet1.RowCount = nRead + 2;

                for (i = 0; i < nRead; i++)
                {
                    ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["MISUID"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    if (dt.Rows[i]["IPDOPD"].ToString().Trim() == "O")
                    {
                        ssView2_Sheet1.Cells[i, 3].Text = "외래";
                    }
                    else if (dt.Rows[i]["IPDOPD"].ToString().Trim() == "I")
                    {
                        ssView2_Sheet1.Cells[i, 3].Text = "입원";
                    }
                    ssView2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["FROMDATE"].ToString().Trim() + "~" + dt.Rows[i]["TODATE"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 6].Text = VB.Val(dt.Rows[i]["ILSU"].ToString().Trim()).ToString("###,###,###,##0 ");
                    nIlsu += (long)VB.Val(dt.Rows[i]["ILSU"].ToString().Trim());
                    ssView2_Sheet1.Cells[i, 7].Text = VB.Val(dt.Rows[i]["AMT2"].ToString().Trim()).ToString("###,###,###,##0 ");
                    nAmt2 += VB.Val(dt.Rows[i]["AMT2"].ToString().Trim());
                    ssView2_Sheet1.Cells[i, 8].Text = VB.Val(dt.Rows[i]["AMT3"].ToString().Trim()).ToString("###,###,###,##0 ");
                    nAmt3 += VB.Val(dt.Rows[i]["AMT3"].ToString().Trim());
                    ssView2_Sheet1.Cells[i, 9].Text = VB.Val(dt.Rows[i]["AMT4"].ToString().Trim()).ToString("###,###,###,##0 ");
                    nAmt4 += VB.Val(dt.Rows[i]["AMT4"].ToString().Trim());
                    if (VB.Val(dt.Rows[i]["AMT4"].ToString().Trim()) != 0)
                    {
                        ssView2_Sheet1.Cells[i, 10].Text = (VB.Val(dt.Rows[i]["AMT4"].ToString().Trim()) / VB.Val(dt.Rows[i]["AMT2"].ToString().Trim()) * 100).ToString("##0.00") + "% ";
                    }
                    ssView2_Sheet1.Cells[i, 11].Text = VB.Val(dt.Rows[i]["AMT5"].ToString().Trim()).ToString("###,###,###,##0 ");
                    nAmt5 += VB.Val(dt.Rows[i]["AMT5"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                ssView2_Sheet1.Cells[nRead, 1].Text = "총건수 : " + nRead;
                ssView2_Sheet1.Cells[nRead, 4].Text = "합  계";
                ssView2_Sheet1.Cells[nRead, 6].Text = nIlsu.ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[nRead, 7].Text = nAmt2.ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[nRead, 8].Text = nAmt3.ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[nRead, 9].Text = nAmt4.ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[nRead, 10].Text = (nAmt4 / nAmt2 * 100).ToString("##0.00") + "% ";
                ssView2_Sheet1.Cells[nRead, 11].Text = nAmt5.ToString("###,###,###,##0 ");

                ssView2_Sheet1.Cells[nRead + 1, 4].Text = "평  균";
                ssView2_Sheet1.Cells[nRead + 1, 6].Text = (nIlsu / nRead).ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[nRead + 1, 7].Text = (nAmt2 / nRead).ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[nRead + 1, 8].Text = (nAmt3 / nRead).ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[nRead + 1, 9].Text = (nAmt4 / nRead).ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[nRead + 1, 10].Text = ((nAmt4 / nRead) / (nAmt2 / nRead) * 100).ToString("##0.00") + "% ";
                ssView2_Sheet1.Cells[nRead + 1, 11].Text = (nAmt4 / nRead).ToString("###,###,###,##0 ");

                ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
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
                return;
            }
        }

        private void frmPmpaViewSanSakPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //this.Close(); //폼 권한 조회
            //return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            panView.Dock = DockStyle.Fill;
            panView2.Dock = DockStyle.Fill;
            panView.Visible = true;
            panView2.Visible = false;

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFYYMM, 36, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTYYMM, 36, "", "1");
            cboFYYMM.SelectedIndex = 0;
            cboTYYMM.SelectedIndex = 0;

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            btnPrint.Enabled = false;
            chkTong.Visible = false;
            grbYYMM.Visible = true;
            grbDate.Visible = false;
            if (clsType.User.Sabun == "4349")
            {
                chkTong.Visible = true;
            }
        }

        private void txtYul_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked == true)
            {
                panView.Visible = false;
                panView2.Visible = true;
                chkTong.Visible = false;
                grbYYMM.Visible = false;
                grbDate.Visible = true;
                lblTitleSub0.Text = "일자별 조회 조건";
                lblTitleSub1.Text = "일자별 조회 결과";
            }
            else if (chkDate.Checked == false)
            {
                panView.Visible = true;
                panView2.Visible = false;
                grbYYMM.Visible = true;
                grbDate.Visible = false;
                lblTitleSub0.Text = "일반 조회 조건";
                lblTitleSub1.Text = "일반 조회 결과";
            }
        }
    }
}
