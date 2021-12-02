using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedIpdNr
    /// File Name       : frmReportMonthICU.cs
    /// Description     : ICU관련 월보고 기록지
    /// Author          : 박창욱
    /// Create Date     : 2018-05-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrinfo\FrmReportMonthICU.frm(FrmReportMonthICU.frm) >> frmReportMonthICU.cs 폼이름 재정의" />	
    public partial class frmReportMonthICU : Form
    {
        public frmReportMonthICU()
        {
            InitializeComponent();
        }

        private void btnClose1_Click(object sender, EventArgs e)
        {
            panList1.Visible = false;
        }

        private void btnClose2_Click(object sender, EventArgs e)
        {
            panList2.Visible = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            panList1.Visible = false;
            panList2.Visible = false;

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
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

            int j = 0;
            int nRead = 0;
            int nRow = 0;

            int nSumFMs1 = 0;          //Number previous - First Day of Month
            int nSumFMs2 = 0;          //Number previous - First Day of Next Month

            int nSumI = 0;             //입원환자수 합계
            int nSumT = 0;             //퇴원환자수 합계
            int nSumJ = 0;             //재원환자수 합계

            int nSumCathe = 0;         //Catheter 합계
            int nSumCline = 0;         //C-line(s) 합계
            int nSumVenti = 0;         //Ventilator 합계

            string strYYYY = "";
            string strMM = "";

            string strFDate = "";
            string strTDate = "";
            string nEndDay = "";

            ComFunc cf = new ComFunc();

            strFDate = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 7, 2) + "-01";
            strTDate = cf.READ_LASTDAY(clsDB.DbCon, strFDate);

            if (Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")) < Convert.ToDateTime(strTDate))
            {
                strTDate = cf.DATE_ADD(clsDB.DbCon, ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), -1);
            }

            nEndDay = VB.Val(VB.Right(strTDate, 2)).ToString();

            strYYYY = VB.Left(cboYYMM.Text, 4);
            strMM = VB.Mid(cboYYMM.Text, 7, 2);

            //if (rdoMICU.Checked == true)
            //{
            //    rdoMICU_Check();
            //}
            //else if (rdoSICU.Checked == true)
            //{
            //    rdoSICU_Check();
            //}

            //if (rdoMICU.Checked == true)
            //{
            //    ssView_Sheet1.Cells[0, 0].Text = "월보고 기록지 (부서 : MICU)";
            //}
            //else if (rdoSICU.Checked == true)
            //{
            //    ssView_Sheet1.Cells[0, 0].Text = "월보고 기록지 (부서 : SICU)";
            //}
            //else if (rdo32.Checked == true)
            //{
            //    ssView_Sheet1.Cells[0, 0].Text = "월보고 기록지 (부서 : 32)";
            //}
            if (rdo33.Checked == true)
            {
                ssView_Sheet1.Cells[0, 0].Text = "월보고 기록지 (부서 : 33)";
            }
            else if (rdo35.Checked == true)
            {
                ssView_Sheet1.Cells[0, 0].Text = "월보고 기록지 (부서 : 35)";
            }
            else
            {
                ssView_Sheet1.Cells[0, 0].Text = "월보고 기록지 (부서 : ICU)";
            }

            ssView_Clear();
            ss_Clear(ssList1);
            ss_Clear(ssList2);

            btnPrint.Enabled = false;
            ssView.Enabled = false;
            panList1.Visible = false;
            panList2.Visible = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //작업년월
                ssView_Sheet1.Cells[1, 3].Text = cboYYMM.Text;

                //Number of previous days in ICU for these patients
                //그 달과 다음달의 첫날 자료 조회
                for (j = 1; j <= 2; j++)
                {
                    //SQL = "";
                    //SQL = "SELECT M.PANO, M.SNAME, M.SEX, M.AGE, M.DEPTCODE, MAX(N.INDATE) INDATE, ";
                    //SQL = SQL + ComNum.VBLF + "TO_DATE('" + strYYYY + "-" + strMM + "-01', 'YYYY-MM-DD') - MAX(TRUNC(N.INDATE)) COUNT, M.IPDNO ";
                    ////2018-10-08 안정수, NUR_P_LIST가 아닌 IPD_NEW_MASTER로 변경
                    //SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.IPD_BM M, KOSMOS_PMPA.IPD_NEW_MASTER N ";
                    //SQL = SQL + ComNum.VBLF + "WHERE M.PANO = N.PANO(+) ";
                    //SQL = SQL + ComNum.VBLF + "  AND M.IPDNO = N.IPDNO(+) ";
                    //SQL = SQL + ComNum.VBLF + "  AND M.WARDCODE IN ('32','IU','33','35') ";


                    ////If OptMICU.Value = True Then
                    ////    SQL = SQL & "AND M.ROOMCODE IN ('234') " & vbLf
                    ////ElseIf OptSICU.Value = True Then
                    ////    SQL = SQL & "AND M.ROOMCODE IN ('233') " & vbLf
                    //if (rdo33.Checked == true)
                    //{
                    //    SQL = SQL + ComNum.VBLF + "  AND M.WARDCODE = '33'";
                    //}
                    //else if (rdo35.Checked == true)
                    //{
                    //    SQL = SQL + ComNum.VBLF + "  AND M.WARDCODE = '35'";
                    //}
                    ////ElseIf Opt32.Value Then
                    ////    SQL = SQL & " AND M.WARDCODE = '32'"
                    //else
                    //{
                    //    SQL = SQL + ComNum.VBLF + "  AND M.WARDCODE IN ('32','33','35','IU')  ";
                    //}

                    //SQL = SQL + ComNum.VBLF + "  AND M.JOBDATE = TO_DATE('" + strYYYY + "-" + strMM + "-01', 'YYYY-MM-DD') ";
                    //SQL = SQL + ComNum.VBLF + "  AND M.GBBACKUP = 'J' ";
                    ////'SQL = SQL & "AND ( M.OUTDATE IS NULL) OR M.OUTDATE >= TO_DATE('" & strYYYY & " - " & strMM & " - 01','YYYY - MM - DD')) " & vbLf
                    //SQL = SQL + ComNum.VBLF + "  AND N.INDATE < TO_DATE('" + strYYYY + "-" + strMM + "-02','YYYY-MM-DD') ";
                    //SQL = SQL + ComNum.VBLF + "  AND M.PANO < '90000000' ";
                    //SQL = SQL + ComNum.VBLF + "  AND M.GBSTS <> '9' ";
                    //SQL = SQL + ComNum.VBLF + "GROUP BY M.PANO, M.SNAME, M.SEX, M.AGE, M.DEPTCODE, M.IPDNO ";
                    //SQL = SQL + ComNum.VBLF + "ORDER BY INDATE, M.PANO, M.IPDNO, M.SNAME ";

                    //2018-11-06 전상원 쿼리새로만듬 ICU재원일수로 수정
                    SQL = "";
                    SQL = "SELECT ";
                    SQL = SQL + ComNum.VBLF + "    PANO, SNAME, SEX, AGE, DEPTCODE, ICUINDATE, IPDNO,";
                    SQL = SQL + ComNum.VBLF + "    TO_DATE('"+ strYYYY + "-" + strMM + "-02','YYYY -MM-DD') - TO_DATE(TO_CHAR(ICUINDATE,'YYYY-MM-DD'),'YYYY-MM-DD') AS ILSU ";
                    SQL = SQL + ComNum.VBLF + "FROM     ";
                    SQL = SQL + ComNum.VBLF + "(SELECT  ";
                    SQL = SQL + ComNum.VBLF + "    PANO, SNAME, SEX, AGE, DEPTCODE,  ";
                    SQL = SQL + ComNum.VBLF + "    DECODE(TRSDATE,'',INDATE,TRSDATE) ICUINDATE,     ";
                    SQL = SQL + ComNum.VBLF + "    IPDNO ";
                    SQL = SQL + ComNum.VBLF + "FROM     ";
                    SQL = SQL + ComNum.VBLF + "(SELECT  ";
                    SQL = SQL + ComNum.VBLF + "    A.PANO, A.SNAME, A.SEX, A.AGE, A.DEPTCODE, A.IPDNO, ";
                    SQL = SQL + ComNum.VBLF + "    MAX(B.TRSDATE) AS TRSDATE, C.INDATE      ";
                    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.IPD_BM A ";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.IPD_NEW_MASTER C ";
                    SQL = SQL + ComNum.VBLF + "   ON A.IPDNO = C.IPDNO ";
                    SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_PMPA.IPD_TRANSFOR B ";
                    SQL = SQL + ComNum.VBLF + "  ON A.IPDNO = B.IPDNO ";
                    SQL = SQL + ComNum.VBLF + " AND B.TRSDATE <= TO_DATE('" + strYYYY + "-" + strMM + "-02','YYYY-MM-DD')  ";
                    SQL = SQL + ComNum.VBLF + " AND B.FRWARD <> B.TOWARD ";

                    if (rdo33.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND B.TOWARD = '33'";
                    }
                    else if (rdo35.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND B.TOWARD = '35'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND B.TOWARD IN ('32', '33', '35', 'IU')  ";
                    }

                    
                    SQL = SQL + ComNum.VBLF + "WHERE A.JOBDATE = TO_DATE('" + strYYYY + "-" + strMM + "-01','YYYY-MM-DD')            ";

                    if (rdo33.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.WARDCODE = '33'          ";
                    }
                    else if (rdo35.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.WARDCODE = '35'          ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.WARDCODE IN ('32', '33', '35', 'IU')  ";
                    }

                    SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, A.SNAME, A.SEX, A.AGE, A.DEPTCODE, A.IPDNO, C.INDATE)) M1 ";
                    SQL = SQL + ComNum.VBLF + "WHERE NOT EXISTS(SELECT 1 FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER M2";
                    SQL = SQL + ComNum.VBLF + "                 WHERE M1.IPDNO = M2.IPDNO";
                    SQL = SQL + ComNum.VBLF + "                   AND M2.OUTDATE < TO_DATE('" + strYYYY + "-" + strMM + "-01','YYYY-MM-DD'))            ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY SNAME";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    nRead = dt.Rows.Count;

                    //First Day of Month
                    if (j == 1)
                    {
                        ssView_Sheet1.Cells[4, 14].Text = nRead.ToString();    //그 달의 첫 날 재원환자 수

                        //if (rdoMICU.Checked == true)
                        //{
                        //    lblTitle1.Text = "(MICU) 조회기준 일자 : " + strYYYY + "년 " + VB.Val(strMM).ToString("00") + "월 01일";
                        //}
                        //else if (rdoSICU.Checked == true)
                        //{
                        //    lblTitle1.Text = "(SICU) 조회기준 일자 : " + strYYYY + "년 " + VB.Val(strMM).ToString("00") + "월 01일";
                        //}
                        //else
                        //{
                        //    lblTitle1.Text = "(ICU) 조회기준 일자 : " + strYYYY + "년 " + VB.Val(strMM).ToString("00") + "월 01일";
                        //}

                        lblTitle1.Text = "(ICU) 조회기준 일자 : " + strYYYY + "년 " + VB.Val(strMM).ToString("00") + "월 01일";

                        ssList1_Sheet1.RowCount = dt.Rows.Count;

                        nRow = 0;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            //그 달의 첫날 재원환자 리스트 조회
                            nRow++;
                            ssList1_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssList1_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                            ssList1_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                            ssList1_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                            ssList1_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssList1_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["ICUINDATE"].ToString().Trim();
                            ssList1_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["ILSU"].ToString().Trim();
                            ssList1_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["IPDNO"].ToString().Trim();

                            //그 달의 첫날 재원환자 재원일 합계
                            nSumFMs1 = nSumFMs1 + Convert.ToInt32(VB.Val(dt.Rows[i]["ILSU"].ToString().Trim()));
                        }

                        ssList1_Sheet1.RowCount = nRow;

                        ssView_Sheet1.Cells[5, 14].Text = nSumFMs1.ToString();
                    }
                    else
                    {
                        ssView_Sheet1.Cells[4, 22].Text = nRead.ToString(); //다음 달의 첫날 재원환자수

                        //if (rdoMICU.Checked == true)
                        //{
                        //    lblTitle2.Text = "(MICU) 조회기준 일자 : " + strYYYY + "년 " + VB.Val(strMM).ToString("00") + "월 01일";
                        //}
                        //else if (rdoSICU.Checked == true)
                        //{
                        //    lblTitle2.Text = "(SICU) 조회기준 일자 : " + strYYYY + "년 " + VB.Val(strMM).ToString("00") + "월 01일";
                        //}
                        //else
                        //{
                        //    lblTitle2.Text = "(ICU) 조회기준 일자 : " + strYYYY + "년 " + VB.Val(strMM).ToString("00") + "월 01일";
                        //}

                        lblTitle2.Text = "(ICU) 조회기준 일자 : " + strYYYY + "년 " + VB.Val(strMM).ToString("00") + "월 01일";

                        ssList2_Sheet1.RowCount = nRead;
                        nRow = 0;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            //그 달의 첫날 재원환자 리스트 조회
                            nRow++;
                            ssList2_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssList2_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                            ssList2_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                            ssList2_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                            ssList2_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssList2_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["ICUINDATE"].ToString().Trim();
                            ssList2_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["ILSU"].ToString().Trim();
                            ssList2_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["IPDNO"].ToString().Trim();

                            //그 달의 첫날 재원환자 재원일 합계
                            nSumFMs2 = nSumFMs2 + Convert.ToInt32(VB.Val(dt.Rows[i]["ILSU"].ToString().Trim()));
                        }

                        ssList2_Sheet1.RowCount = nRow;

                        ssView_Sheet1.Cells[5, 22].Text = nSumFMs2.ToString();
                    }

                    //년도가 넘어갈 때 1월로 초기화
                    if (VB.Val(strMM) != 12)
                    {
                        strMM = (VB.Val(strMM) + 1).ToString();
                    }
                    else
                    {
                        strYYYY = (VB.Val(strYYYY) + 1).ToString();
                        strMM = "1";
                    }

                    dt.Dispose();
                    dt = null;
                }

                //일별 입원, 퇴원, 재원환자수 조회
                SQL = "";
                SQL = "SELECT  TO_CHAR(ACTDATE,'DD') DAY, ";
                SQL = SQL + ComNum.VBLF + "SUM(DECODE( CODE,'02', QTY1+ QTY2+ QTY3 +QTY4, '04', QTY1+QTY2+QTY3+QTY4,0)) INWON, ";     //입원
                SQL = SQL + ComNum.VBLF + "SUM(DECODE( CODE,'03', QTY1+ QTY2+ QTY3 +QTY4, '09', QTY1+QTY2+QTY3+QTY4,0)) TEWON, ";     //퇴원
                SQL = SQL + ComNum.VBLF + "SUM(DECODE( CODE,'01', QTY4,0)) JEWON ";                                                   //재원
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_INOUT ";
                SQL = SQL + ComNum.VBLF + "WHERE ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                //if (rdoMICU.Checked == true)
                //{
                //    SQL = SQL + ComNum.VBLF + "AND WARDCODE = 'MICU' ";
                //}
                //else if (rdoSICU.Checked == true)
                //{
                //    SQL = SQL + ComNum.VBLF + "AND WARDCODE = 'SICU' ";
                //}
                if (rdo33.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND WARDCODE = '33' ";
                }
                else if (rdo35.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND WARDCODE = '35' ";
                }
                //else if (rdo32.Checked == true)
                //{
                //    SQL = SQL + ComNum.VBLF + "AND WARDCODE = '32' ";
                //}
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND WARDCODE IN ('32','MICU','SICU','33','35')  ";
                }

                SQL = SQL + ComNum.VBLF + "   AND CODE IN ('01','02','03','04','09') ";
                SQL = SQL + ComNum.VBLF + "GROUP BY TO_CHAR(ACTDATE,'DD') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[Convert.ToInt32(VB.Val(dt.Rows[i]["Day"].ToString().Trim())) + 7, 2].Text = dt.Rows[i]["INWON"].ToString().Trim();  //입원
                    nSumI += Convert.ToInt32(VB.Val(dt.Rows[i]["INWON"].ToString().Trim()));
                    ssView_Sheet1.Cells[Convert.ToInt32(VB.Val(dt.Rows[i]["Day"].ToString().Trim())) + 7, 5].Text = dt.Rows[i]["TEWON"].ToString().Trim();  //퇴원
                    nSumT += Convert.ToInt32(VB.Val(dt.Rows[i]["TEWON"].ToString().Trim()));
                    ssView_Sheet1.Cells[Convert.ToInt32(VB.Val(dt.Rows[i]["Day"].ToString().Trim())) + 7, 8].Text = dt.Rows[i]["JEWON"].ToString().Trim();  //재원
                    nSumJ += Convert.ToInt32(VB.Val(dt.Rows[i]["JEWON"].ToString().Trim()));
                }

                dt.Dispose();
                dt = null;


                //Indewelling urinary catieter, C-line(s), Ventilator 조회
                SQL = "";
                SQL = "SELECT WARD, TO_CHAR(JOBDATE,'DD') JOBDATE, ";
                SQL = SQL + ComNum.VBLF + "ININWON, TEINWON, JEINWON, FOLEY, CLINE, VENTILATOR, INFACTION, ROWID  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_INFECTION ";
                SQL = SQL + ComNum.VBLF + "WHERE JOBDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND JOBDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                //if (rdoMICU.Checked == true)
                //{
                //    SQL = SQL + ComNum.VBLF + "AND WARD = 'MICU' ";
                //}
                //else if (rdoSICU.Checked == true)
                //{
                //    SQL = SQL + ComNum.VBLF + "AND WARD = 'SICU' ";
                //}
                if (rdo33.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND WARD = '33' ";
                }
                else if (rdo35.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND WARD = '35' ";
                }
                //else if (rdo32.Checked == true)
                //{
                //    SQL = SQL + ComNum.VBLF + "AND WARD = '32' ";
                //}
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND WARD IN ('32','MICU','SICU','33','35')  ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[Convert.ToInt32(VB.Val(dt.Rows[i]["JOBDATE"].ToString().Trim())) + 7, 11].Text = dt.Rows[i]["FOLEY"].ToString().Trim();  //Catheter
                    nSumCathe += Convert.ToInt32(VB.Val(dt.Rows[i]["FOLEY"].ToString().Trim()));
                    ssView_Sheet1.Cells[Convert.ToInt32(VB.Val(dt.Rows[i]["JOBDATE"].ToString().Trim())) + 7, 22].Text = dt.Rows[i]["CLINE"].ToString().Trim();  //C-line(s)
                    nSumCline += Convert.ToInt32(VB.Val(dt.Rows[i]["CLINE"].ToString().Trim()));
                    ssView_Sheet1.Cells[Convert.ToInt32(VB.Val(dt.Rows[i]["JOBDATE"].ToString().Trim())) + 7, 26].Text = dt.Rows[i]["VENTILATOR"].ToString().Trim();  //Ventilator
                    nSumVenti += Convert.ToInt32(VB.Val(dt.Rows[i]["VENTILATOR"].ToString().Trim()));
                }

                dt.Dispose();
                dt = null;


                //입원, 퇴원, 재원환자수, Catheter, C-line(s), Ventilator 합계
                ssView_Sheet1.Cells[39, 2].Text = nSumI.ToString();
                ssView_Sheet1.Cells[39, 5].Text = nSumT.ToString();
                ssView_Sheet1.Cells[39, 8].Text = nSumJ.ToString();
                ssView_Sheet1.Cells[39, 11].Text = nSumCathe.ToString();
                ssView_Sheet1.Cells[39, 22].Text = nSumCline.ToString();
                ssView_Sheet1.Cells[39, 26].Text = nSumVenti.ToString();


                //Monthly number of patients discharged on the same day as admitted
                //그 달의 입원환자중 입원일과 퇴원일이 같은 사람의 합계
                SQL = "SELECT  DISTINCT * FROM ( ";
                SQL = SQL + ComNum.VBLF + "SELECT a.PANO, a.SNAME, C.SEX, C.AGE, DEPTCODE, C.INDATE, C.OUTDATE, ILSU, a.IPDNO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A ," + ComNum.DB_PMPA + "NUR_MASTER C ";
                SQL = SQL + ComNum.VBLF + "WHERE C.wardindate >= TO_DATE('" + strFDate + " 00:00:00', 'YYYY-MM-DD HH24:MI:SS') ";
                SQL = SQL + ComNum.VBLF + "  AND C.wardindate <= TO_DATE('" + strTDate + " 23:59:59', 'YYYY-MM-DD HH24:MI:SS') ";
                SQL = SQL + ComNum.VBLF + "  AND TRUNC(C.wardindate) = TRUNC(nvl(C.OUTDATE,sysdate)) AND GBSTS <> '9' AND A.IPDNO = C.IPDNO";
                if (rdo33.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND WARDCODE = '33' ";
                }
                else if (rdo35.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND WARDCODE = '35' ";
                }

                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND WARDCODE IN ('32','MICU','SICU','33','35')  ";
                }
                SQL = SQL + ComNum.VBLF + "   UNION ALL ";
                SQL = SQL + ComNum.VBLF + "  SELECT a.PANO, a.SNAME, C.SEX, C.AGE, DEPTCODE, C.INDATE, C.OUTDATE, ILSU, a.IPDNO ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A ," + ComNum.DB_PMPA + "IPD_TRANSFOR B," + ComNum.DB_PMPA + "NUR_MASTER C ";
                SQL = SQL + ComNum.VBLF + "WHERE  TRUNC(B.TRSDATE) >= TO_DATE('" + strFDate + " 00:00:00', 'YYYY-MM-DD HH24:MI:SS') ";
                SQL = SQL + ComNum.VBLF + "  AND  TRUNC(B.TRSDATE) <= TO_DATE('" + strTDate + " 23:59:59', 'YYYY-MM-DD HH24:MI:SS') ";
                SQL = SQL + ComNum.VBLF + "  AND A.IPDNO = B.IPDNO AND A.IPDNO = C.IPDNO";
                SQL = SQL + ComNum.VBLF + "  AND TRUNC(C.wardindate) <> TRUNC(nvl(C.OUTDATE,sysdate)) ";
                SQL = SQL + ComNum.VBLF + "  AND TRUNC(nvl(C.OUTDATE,sysdate)) = TRUNC(B.TRSDATE)";
                SQL = SQL + ComNum.VBLF + "  AND FRWARD <> TOWARD  AND GBSTS <> '9' ";
                if (rdo33.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND TOWARD = '33' ";
                }
                else if (rdo35.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND TOWARD = '35' ";
                }

                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND TOWARD IN ('32','MICU','SICU','33','35')  ";
                }
                SQL = SQL + ComNum.VBLF + "   UNION ALL ";
                SQL = SQL + ComNum.VBLF + "  SELECT a.PANO, a.SNAME, C.SEX, C.AGE, DEPTCODE, C.INDATE, C.OUTDATE, ILSU, a.IPDNO ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A ," + ComNum.DB_PMPA + "IPD_TRANSFOR B," + ComNum.DB_PMPA + "NUR_MASTER C ";
                SQL = SQL + ComNum.VBLF + "WHERE  TRUNC(B.TRSDATE) >= TO_DATE('" + strFDate + " 00:00:00', 'YYYY-MM-DD HH24:MI:SS') ";
                SQL = SQL + ComNum.VBLF + "  AND  TRUNC(B.TRSDATE) <= TO_DATE('" + strTDate + " 23:59:59', 'YYYY-MM-DD HH24:MI:SS') ";
                SQL = SQL + ComNum.VBLF + "  AND A.IPDNO = B.IPDNO AND A.IPDNO = C.IPDNO";
                SQL = SQL + ComNum.VBLF + "  AND TRUNC(C.wardindate) <> TRUNC(nvl(C.OUTDATE,sysdate)) ";
                SQL = SQL + ComNum.VBLF + "  AND TRUNC(nvl(C.OUTDATE,sysdate)) = TRUNC(B.TRSDATE)";
                SQL = SQL + ComNum.VBLF + "  AND FRWARD <> TOWARD  AND GBSTS <> '9' ";
                if (rdo33.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND TOWARD = '33' ";
                }
                else if (rdo35.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND TOWARD = '35' ";
                }

                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND TOWARD IN ('32','MICU','SICU','33','35')  ";
                }
                SQL = SQL + ComNum.VBLF + "   UNION ALL ";
                SQL = SQL + ComNum.VBLF + "  SELECT a.PANO, a.SNAME, C.SEX, C.AGE, DEPTCODE, C.INDATE, C.OUTDATE, ILSU, a.IPDNO ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A ," + ComNum.DB_PMPA + "IPD_TRANSFOR B," + ComNum.DB_PMPA + "NUR_MASTER C ";
                SQL = SQL + ComNum.VBLF + "WHERE  TRUNC(B.TRSDATE) >= TO_DATE('" + strFDate + " 00:00:00', 'YYYY-MM-DD HH24:MI:SS') ";
                SQL = SQL + ComNum.VBLF + "  AND  TRUNC(B.TRSDATE) <= TO_DATE('" + strTDate + " 23:59:59', 'YYYY-MM-DD HH24:MI:SS') ";
                SQL = SQL + ComNum.VBLF + "  AND A.IPDNO = B.IPDNO AND A.IPDNO = C.IPDNO";
                SQL = SQL + ComNum.VBLF + "  AND TRUNC(C.wardindate) <> TRUNC(nvl(C.OUTDATE,sysdate)) ";
                SQL = SQL + ComNum.VBLF + "  AND TRUNC(nvl(C.OUTDATE,sysdate)) <> TRUNC(B.TRSDATE)";
                SQL = SQL + ComNum.VBLF + "  AND FRWARD <> TOWARD  AND GBSTS <> '9' ";
                if (rdo33.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND FRWARD = '33' ";
                }
                else if (rdo35.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND FRWARD = '35' ";
                }

                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND FRWARD IN ('32','MICU','SICU','33','35')  ";
                }
                SQL = SQL + ComNum.VBLF + "   AND 0 <> ( SELECT mod(count(*),2)  FROM IPD_TRANSFOR SUB    WHERE B.IPDNO = SUB.IPDNO  ";
                SQL = SQL + ComNum.VBLF + "           AND TRUNC(SUB.TRSDATE) = TRUNC(B.TRSDATE) AND SUB.FRWARD <> SUB.TOWARD and b.FRWARD =SUB.TOWARD  )   ";

                SQL = SQL + ComNum.VBLF + "   UNION ALL ";
                SQL = SQL + ComNum.VBLF + "  SELECT a.PANO, a.SNAME, C.SEX, C.AGE, DEPTCODE, C.INDATE, C.OUTDATE, ILSU, a.IPDNO ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A ," + ComNum.DB_PMPA + "IPD_TRANSFOR B," + ComNum.DB_PMPA + "NUR_MASTER C ";
                SQL = SQL + ComNum.VBLF + "WHERE  TRUNC(B.TRSDATE) >= TO_DATE('" + strFDate + " 00:00:00', 'YYYY-MM-DD HH24:MI:SS') ";
                SQL = SQL + ComNum.VBLF + "  AND  TRUNC(B.TRSDATE) <= TO_DATE('" + strTDate + " 23:59:59', 'YYYY-MM-DD HH24:MI:SS') ";
                SQL = SQL + ComNum.VBLF + "  AND A.IPDNO = B.IPDNO AND A.IPDNO = C.IPDNO";
                SQL = SQL + ComNum.VBLF + "  AND TRUNC(C.wardindate) <> TRUNC(nvl(C.OUTDATE,sysdate)) ";
                SQL = SQL + ComNum.VBLF + "  AND TRUNC(C.wardindate) = TRUNC(B.TRSDATE)";
                SQL = SQL + ComNum.VBLF + "  AND FRWARD <> TOWARD  AND GBSTS <> '9' ";
                if (rdo33.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND FRWARD = '33' ";
                }
                else if (rdo35.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND FRWARD = '35' ";
                }

                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND FRWARD IN ('32','MICU','SICU','33','35')  ";
                }

                SQL = SQL + ComNum.VBLF + " ) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PANO, IPDNO, SNAME, INDATE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt.Rows.Count;

                ssView_Sheet1.Cells[41, 22].Text = nRead.ToString();

                dt.Dispose();
                dt = null;

                btnPrint.Enabled = true;
                ssView.Enabled = true;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void ss_Clear(FpSpread ssArg)
        {
            ssArg.ActiveSheet.RowCount = 0;
            //ssArg.ActiveSheet.Cells[0, 0, ssArg.ActiveSheet.RowCount - 1, ssArg.ActiveSheet.ColumnCount - 1].Text = "";
        }

        private void frmReportMonthICU_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            string strYYMM = "";
            string strSysDate = "";
            string gsWard = "";

            gsWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");

            if (gsWard == "33")
            {
                rdo33.Checked = true;
            }
            else if (gsWard == "35")
            {
                rdo35.Checked = true;
            }
            else
            {
                rdoICU.Checked = true;
            }

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            strYYMM = VB.Left(strSysDate, 4) + VB.Mid(strSysDate, 6, 2);

            strYYMM = clsVbfunc.DateYYMMAdd(strYYMM, 0);

            cboYYMM.Items.Clear();

            for (i = 1; i <= 12; i++)
            {
                cboYYMM.Items.Add(VB.Left(strYYMM, 4) + "년 " + VB.Right(strYYMM, 2) + "월");
                strYYMM = clsVbfunc.DateYYMMAdd(strYYMM, -1);
            }

            cboYYMM.SelectedIndex = 0;

            ssView_Clear();
            ss_Clear(ssList1);
            ss_Clear(ssList2);

            btnPrint.Enabled = false;
            ssView.Enabled = false;
            panList1.Visible = false;
            panList2.Visible = false;
        }

        private void rdoMICU_Click(object sender, EventArgs e)
        {
            rdoMICU_Check();
        }

        void rdoMICU_Check()
        {
            //if (rdoMICU.Checked == true)
            //{
            //    ssView_Sheet1.Cells[0, 0].Text = "월보고 기록지 (부서 : MICU)";
            //}

            ssView_Sheet1.Cells[0, 0].Text = "월보고 기록지 (부서 : ICU)";

            ssView_Clear();
            ss_Clear(ssList1);
            ss_Clear(ssList2);

            btnPrint.Enabled = false;
            ssView.Enabled = false;
            panList1.Visible = false;
            panList2.Visible = false;
        }

        private void rdoSICU_Click(object sender, EventArgs e)
        {
            rdoSICU_Check();
        }

        void rdoSICU_Check()
        {
            //if (rdoMICU.Checked == true)
            //{
            //    ssView_Sheet1.Cells[0, 0].Text = "월보고 기록지 (부서 : SICU)";
            //}

            ssView_Sheet1.Cells[0, 0].Text = "월보고 기록지 (부서 : ICU)";

            ssView_Clear();
            ss_Clear(ssList1);
            ss_Clear(ssList2);

            btnPrint.Enabled = false;
            ssView.Enabled = false;
            panList1.Visible = false;
            panList2.Visible = false;
        }

        private void ssView_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            //그 달의 첫날 재원환자
            if (e.Column == 19 && e.Row == 5)
            {
                if (panList1.Visible == true)
                {
                    panList1.Visible = false;
                }
                else if (panList1.Visible == false)
                {
                    panList1.Visible = true;
                }

                panList2.Visible = false;
            }
            //다음 달의 첫날 재원환자
            else if (e.Column == 27 && e.Row == 5)
            {
                if (panList2.Visible == true)
                {
                    panList2.Visible = false;
                }
                else if (panList2.Visible == false)
                {
                    panList2.Visible = true;
                }

                panList1.Visible = false;
            }
        }

        void ssView_Clear()
        {
            ssView_Sheet1.Cells[1, 3].Text = cboYYMM.Text;

            ssView_Sheet1.Cells[4, 14].Text = "";   //Number of patients in ICU - First Day of Month
            ssView_Sheet1.Cells[4, 22].Text = "";   //Number of patients in ICU - First Day of Next Month

            ssView_Sheet1.Cells[5, 14].Text = "";   //Number of patients days in ICU for these patients - First Day of Month
            ssView_Sheet1.Cells[5, 22].Text = "";   //Number of patients days in ICU for these patients - First Day of Next Month

            //1
            ssView_Sheet1.Cells[8, 2].Text = "";
            ssView_Sheet1.Cells[8, 5].Text = "";
            ssView_Sheet1.Cells[8, 8].Text = "";
            ssView_Sheet1.Cells[8, 11].Text = "";
            ssView_Sheet1.Cells[8, 22].Text = "";
            ssView_Sheet1.Cells[8, 26].Text = "";

            //2
            ssView_Sheet1.Cells[9, 2].Text = "";
            ssView_Sheet1.Cells[9, 5].Text = "";
            ssView_Sheet1.Cells[9, 8].Text = "";
            ssView_Sheet1.Cells[9, 11].Text = "";
            ssView_Sheet1.Cells[9, 22].Text = "";
            ssView_Sheet1.Cells[9, 26].Text = "";

            //3
            ssView_Sheet1.Cells[10, 2].Text = "";
            ssView_Sheet1.Cells[10, 5].Text = "";
            ssView_Sheet1.Cells[10, 8].Text = "";
            ssView_Sheet1.Cells[10, 11].Text = "";
            ssView_Sheet1.Cells[10, 22].Text = "";
            ssView_Sheet1.Cells[10, 26].Text = "";

            //4
            ssView_Sheet1.Cells[11, 2].Text = "";
            ssView_Sheet1.Cells[11, 5].Text = "";
            ssView_Sheet1.Cells[11, 8].Text = "";
            ssView_Sheet1.Cells[11, 11].Text = "";
            ssView_Sheet1.Cells[11, 22].Text = "";
            ssView_Sheet1.Cells[11, 26].Text = "";

            //5
            ssView_Sheet1.Cells[12, 2].Text = "";
            ssView_Sheet1.Cells[12, 5].Text = "";
            ssView_Sheet1.Cells[12, 8].Text = "";
            ssView_Sheet1.Cells[12, 11].Text = "";
            ssView_Sheet1.Cells[12, 22].Text = "";
            ssView_Sheet1.Cells[12, 26].Text = "";

            //6
            ssView_Sheet1.Cells[13, 2].Text = "";
            ssView_Sheet1.Cells[13, 5].Text = "";
            ssView_Sheet1.Cells[13, 8].Text = "";
            ssView_Sheet1.Cells[13, 11].Text = "";
            ssView_Sheet1.Cells[13, 22].Text = "";
            ssView_Sheet1.Cells[13, 26].Text = "";

            //7
            ssView_Sheet1.Cells[14, 2].Text = "";
            ssView_Sheet1.Cells[14, 5].Text = "";
            ssView_Sheet1.Cells[14, 8].Text = "";
            ssView_Sheet1.Cells[14, 11].Text = "";
            ssView_Sheet1.Cells[14, 22].Text = "";
            ssView_Sheet1.Cells[14, 26].Text = "";

            //8
            ssView_Sheet1.Cells[15, 2].Text = "";
            ssView_Sheet1.Cells[15, 5].Text = "";
            ssView_Sheet1.Cells[15, 8].Text = "";
            ssView_Sheet1.Cells[15, 11].Text = "";
            ssView_Sheet1.Cells[15, 22].Text = "";
            ssView_Sheet1.Cells[15, 26].Text = "";

            //9
            ssView_Sheet1.Cells[16, 2].Text = "";
            ssView_Sheet1.Cells[16, 5].Text = "";
            ssView_Sheet1.Cells[16, 8].Text = "";
            ssView_Sheet1.Cells[16, 11].Text = "";
            ssView_Sheet1.Cells[16, 22].Text = "";
            ssView_Sheet1.Cells[16, 26].Text = "";

            //10
            ssView_Sheet1.Cells[17, 2].Text = "";
            ssView_Sheet1.Cells[17, 5].Text = "";
            ssView_Sheet1.Cells[17, 8].Text = "";
            ssView_Sheet1.Cells[17, 11].Text = "";
            ssView_Sheet1.Cells[17, 22].Text = "";
            ssView_Sheet1.Cells[17, 26].Text = "";

            //11
            ssView_Sheet1.Cells[18, 2].Text = "";
            ssView_Sheet1.Cells[18, 5].Text = "";
            ssView_Sheet1.Cells[18, 8].Text = "";
            ssView_Sheet1.Cells[18, 11].Text = "";
            ssView_Sheet1.Cells[18, 22].Text = "";
            ssView_Sheet1.Cells[18, 26].Text = "";

            //12
            ssView_Sheet1.Cells[19, 2].Text = "";
            ssView_Sheet1.Cells[19, 5].Text = "";
            ssView_Sheet1.Cells[19, 8].Text = "";
            ssView_Sheet1.Cells[19, 11].Text = "";
            ssView_Sheet1.Cells[19, 22].Text = "";
            ssView_Sheet1.Cells[19, 26].Text = "";

            //13
            ssView_Sheet1.Cells[20, 2].Text = "";
            ssView_Sheet1.Cells[20, 5].Text = "";
            ssView_Sheet1.Cells[20, 8].Text = "";
            ssView_Sheet1.Cells[20, 11].Text = "";
            ssView_Sheet1.Cells[20, 22].Text = "";
            ssView_Sheet1.Cells[20, 26].Text = "";

            //14
            ssView_Sheet1.Cells[21, 2].Text = "";
            ssView_Sheet1.Cells[21, 5].Text = "";
            ssView_Sheet1.Cells[21, 8].Text = "";
            ssView_Sheet1.Cells[21, 11].Text = "";
            ssView_Sheet1.Cells[21, 22].Text = "";
            ssView_Sheet1.Cells[21, 26].Text = "";

            //15
            ssView_Sheet1.Cells[22, 2].Text = "";
            ssView_Sheet1.Cells[22, 5].Text = "";
            ssView_Sheet1.Cells[22, 8].Text = "";
            ssView_Sheet1.Cells[22, 11].Text = "";
            ssView_Sheet1.Cells[22, 22].Text = "";
            ssView_Sheet1.Cells[22, 26].Text = "";

            //16
            ssView_Sheet1.Cells[23, 2].Text = "";
            ssView_Sheet1.Cells[23, 5].Text = "";
            ssView_Sheet1.Cells[23, 8].Text = "";
            ssView_Sheet1.Cells[23, 11].Text = "";
            ssView_Sheet1.Cells[23, 22].Text = "";
            ssView_Sheet1.Cells[23, 26].Text = "";

            //17
            ssView_Sheet1.Cells[24, 2].Text = "";
            ssView_Sheet1.Cells[24, 5].Text = "";
            ssView_Sheet1.Cells[24, 8].Text = "";
            ssView_Sheet1.Cells[24, 11].Text = "";
            ssView_Sheet1.Cells[24, 22].Text = "";
            ssView_Sheet1.Cells[24, 26].Text = "";

            //18
            ssView_Sheet1.Cells[25, 2].Text = "";
            ssView_Sheet1.Cells[25, 5].Text = "";
            ssView_Sheet1.Cells[25, 8].Text = "";
            ssView_Sheet1.Cells[25, 11].Text = "";
            ssView_Sheet1.Cells[25, 22].Text = "";
            ssView_Sheet1.Cells[25, 26].Text = "";

            //19
            ssView_Sheet1.Cells[26, 2].Text = "";
            ssView_Sheet1.Cells[26, 5].Text = "";
            ssView_Sheet1.Cells[26, 8].Text = "";
            ssView_Sheet1.Cells[26, 11].Text = "";
            ssView_Sheet1.Cells[26, 22].Text = "";
            ssView_Sheet1.Cells[26, 26].Text = "";

            //20
            ssView_Sheet1.Cells[27, 2].Text = "";
            ssView_Sheet1.Cells[27, 5].Text = "";
            ssView_Sheet1.Cells[27, 8].Text = "";
            ssView_Sheet1.Cells[27, 11].Text = "";
            ssView_Sheet1.Cells[27, 22].Text = "";
            ssView_Sheet1.Cells[27, 26].Text = "";

            //21
            ssView_Sheet1.Cells[28, 2].Text = "";
            ssView_Sheet1.Cells[28, 5].Text = "";
            ssView_Sheet1.Cells[28, 8].Text = "";
            ssView_Sheet1.Cells[28, 11].Text = "";
            ssView_Sheet1.Cells[28, 22].Text = "";
            ssView_Sheet1.Cells[28, 26].Text = "";

            //22
            ssView_Sheet1.Cells[29, 2].Text = "";
            ssView_Sheet1.Cells[29, 5].Text = "";
            ssView_Sheet1.Cells[29, 8].Text = "";
            ssView_Sheet1.Cells[29, 11].Text = "";
            ssView_Sheet1.Cells[29, 22].Text = "";
            ssView_Sheet1.Cells[29, 26].Text = "";

            //23
            ssView_Sheet1.Cells[30, 2].Text = "";
            ssView_Sheet1.Cells[30, 5].Text = "";
            ssView_Sheet1.Cells[30, 8].Text = "";
            ssView_Sheet1.Cells[30, 11].Text = "";
            ssView_Sheet1.Cells[30, 22].Text = "";
            ssView_Sheet1.Cells[30, 26].Text = "";

            //24
            ssView_Sheet1.Cells[31, 2].Text = "";
            ssView_Sheet1.Cells[31, 5].Text = "";
            ssView_Sheet1.Cells[31, 8].Text = "";
            ssView_Sheet1.Cells[31, 11].Text = "";
            ssView_Sheet1.Cells[31, 22].Text = "";
            ssView_Sheet1.Cells[31, 26].Text = "";

            //25
            ssView_Sheet1.Cells[32, 2].Text = "";
            ssView_Sheet1.Cells[32, 5].Text = "";
            ssView_Sheet1.Cells[32, 8].Text = "";
            ssView_Sheet1.Cells[32, 11].Text = "";
            ssView_Sheet1.Cells[32, 22].Text = "";
            ssView_Sheet1.Cells[32, 26].Text = "";

            //26
            ssView_Sheet1.Cells[33, 2].Text = "";
            ssView_Sheet1.Cells[33, 5].Text = "";
            ssView_Sheet1.Cells[33, 8].Text = "";
            ssView_Sheet1.Cells[33, 11].Text = "";
            ssView_Sheet1.Cells[33, 22].Text = "";
            ssView_Sheet1.Cells[33, 26].Text = "";

            //27
            ssView_Sheet1.Cells[34, 2].Text = "";
            ssView_Sheet1.Cells[34, 5].Text = "";
            ssView_Sheet1.Cells[34, 8].Text = "";
            ssView_Sheet1.Cells[34, 11].Text = "";
            ssView_Sheet1.Cells[34, 22].Text = "";
            ssView_Sheet1.Cells[34, 26].Text = "";

            //28
            ssView_Sheet1.Cells[35, 2].Text = "";
            ssView_Sheet1.Cells[35, 5].Text = "";
            ssView_Sheet1.Cells[35, 8].Text = "";
            ssView_Sheet1.Cells[35, 11].Text = "";
            ssView_Sheet1.Cells[35, 22].Text = "";
            ssView_Sheet1.Cells[35, 26].Text = "";

            //29
            ssView_Sheet1.Cells[36, 2].Text = "";
            ssView_Sheet1.Cells[36, 5].Text = "";
            ssView_Sheet1.Cells[36, 8].Text = "";
            ssView_Sheet1.Cells[36, 11].Text = "";
            ssView_Sheet1.Cells[36, 22].Text = "";
            ssView_Sheet1.Cells[36, 26].Text = "";

            //30
            ssView_Sheet1.Cells[37, 2].Text = "";
            ssView_Sheet1.Cells[37, 5].Text = "";
            ssView_Sheet1.Cells[37, 8].Text = "";
            ssView_Sheet1.Cells[37, 11].Text = "";
            ssView_Sheet1.Cells[37, 22].Text = "";
            ssView_Sheet1.Cells[37, 26].Text = "";

            //31
            ssView_Sheet1.Cells[38, 2].Text = "";
            ssView_Sheet1.Cells[38, 5].Text = "";
            ssView_Sheet1.Cells[38, 8].Text = "";
            ssView_Sheet1.Cells[38, 11].Text = "";
            ssView_Sheet1.Cells[38, 22].Text = "";
            ssView_Sheet1.Cells[38, 26].Text = "";

            //합계
            ssView_Sheet1.Cells[39, 2].Text = "";
            ssView_Sheet1.Cells[39, 5].Text = "";
            ssView_Sheet1.Cells[39, 8].Text = "";
            ssView_Sheet1.Cells[39, 11].Text = "";
            ssView_Sheet1.Cells[39, 22].Text = "";
            ssView_Sheet1.Cells[39, 26].Text = "";

            //Monthly number of patients discharged on the same day as admitted
            ssView_Sheet1.Cells[41, 22].Text = "";

        }
    }
}
