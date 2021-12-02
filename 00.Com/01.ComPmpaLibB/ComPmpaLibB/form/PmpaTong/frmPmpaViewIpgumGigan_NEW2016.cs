using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-10-17
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= d:\psmh\misu\misubs\misubs.vbp\입금회수기간분석2016.frm" >> frmPmpaViewIpgumGigan_NEW2016.cs 폼이름 재정의" />

    public partial class frmPmpaViewIpgumGigan_NEW2016 : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strdtP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string GstrSQL = "";
        string GstrDate = "";
        string GstrRet = "";

        string FstrFYYMM = "";
        string FstrTYYMM = "";
        int nWRTNO = 0;
        int nRow = 0;
        int nIlsu = 0;
        double nIlsuTot = 0;
        string strFd = "";
        string strTd = "";
        string strGDate = "";
        double nSlipJan = 0;//       ' 미수일수*미수잔액의 합계금액
        double nSlipJan2 = 0;//      ' 외래금액
        double nSlipJan3 = 0;//      ' 건진금액
        double nJanAmt = 0;//        ' 미수잔액
        double nJanAmtTot = 0;//     ' 미수잔액 합계
        double nDayDiv = 0;//        ' 평균미수일수
        double nIlsuTot2 = 0;//      ' 일수합계
        int nReadtot = 0;
        double nAmtTot1 = 0; //      ' 외래금액
        double nAmtTot2 = 0; //      ' 건진금액
        double[] nData = new double[7];          //' 부분별 미수잔액합계
        double[] nAmtData1 = new double[7];      //' 전체미수잔액합계
        double[] nAmtData2 = new double[7];      //' 전체미수잔액합계

        public frmPmpaViewIpgumGigan_NEW2016()
        {
            InitializeComponent();
        }

        public frmPmpaViewIpgumGigan_NEW2016(string strGstrSQL, string strGstrDate, string strGstrRet)
        {

            GstrSQL = strGstrSQL;
            GstrDate = strGstrDate;
            GstrRet = strGstrRet;

            InitializeComponent();
        }

        private void frmPmpaViewIpgumGigan_NEW2016_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int nMM = 0;

            nYY = (int)VB.Val(VB.Left(strdtP, 4));
            nMM = (int)VB.Val(VB.Mid(strdtP, 6, 2)) - 1;

            cboFDate.Text = "";
            cboTDate.Text = "";

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFDate, 60, "", "0");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTDate, 60, "", "0");

            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboFDate.DropDownStyle = ComboBoxStyle.DropDown;
                    cboTDate.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }


            int i = 0;
            int j = 0;
            int k = 0;
            string SQL = "";

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;

            nAmtTot1 = 0; nAmtTot2 = 0;
            nSlipJan2 = 0; nSlipJan3 = 0;
            nRow = 0;
            for (i = 0; i <= 6; i++)
            {
                nAmtData1[i] = 0;
                nAmtData2[i] = 0;
            }

            FstrFYYMM = VB.Left(cboFDate.Text, 4) + VB.Right(cboFDate.Text, 2);
            FstrTYYMM = VB.Left(cboTDate.Text, 4) + VB.Right(cboTDate.Text, 2);

            if (string.Compare(FstrFYYMM, "201601") < 0)
            {
                ComFunc.MsgBox("2016년 이전은 다른 메뉴를 선택 해주세요", "확인");
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                btnSearch.Enabled = false;

                ssView_Sheet1.RowCount = 14;
                ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

                for (j = 1; j <= 4; j++)
                {
                    CmdOK_Data_Display(j);               //'일반 미수
                }
                for (k = 1; k <= 7; k++)
                {
                    CmdOK_Data_Display3(k);            //'건진 미수
                }
                Total_All_TOT_Rtn();                  //'일반+센터 합계

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 발생일별 미수 상세내역 Display
        /// </summary>
        private void CmdOK_Data_Display(int j)
        {
            int i = 0;
            DataTable dt = null;
            DataTable dtFc = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT a.WRTNO,a.Class,a.GelCode,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate,";
            SQL = SQL + ComNum.VBLF + "       a.ipgumamt,b.amt3,a.YYMM  ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_MONTHLY a," + ComNum.DB_PMPA + "MISU_IDMST b  ";
            SQL = SQL + ComNum.VBLF + " WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "   AND a.YYMM >='" + FstrFYYMM + "' ";
            SQL = SQL + ComNum.VBLF + "   AND a.YYMM <='" + FstrTYYMM + "' ";

            if (rdoIO1.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " AND A.IPDOPD ='I'";
            }
            if (rdoIO2.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " AND A.IPDOPD ='O'"; ;
            }

            SQL = SQL + ComNum.VBLF + "   AND a.IPGUMAMT <> 0 ";
            SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO(+) ";

            if (j == 1)
            {
                SQL = SQL + ComNum.VBLF + " AND a.Class in('01','02','03') ";//   '공단,직장,지역
            }
            else if (j == 2)
            {
                SQL = SQL + ComNum.VBLF + " AND a.Class = '04' ";//   '보호
            }
            else if (j == 3)
            {
                SQL = SQL + ComNum.VBLF + " AND a.Class = '07' ";//   '자보
            }
            else if (j == 4)
            {
                SQL = SQL + ComNum.VBLF + " AND a.Class = '05' ";//   '산재
            }
            SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {

                dt.Dispose();
                dt = null;
                btnSearch.Enabled = true;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;

            }

            //스프레드 출력문
            // ssView_Sheet1.RowCount = dt.Rows.Count;
            nReadtot = nReadtot + dt.Rows.Count;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nWRTNO = Convert.ToInt32(dt.Rows[i]["WRTNO"].ToString().Trim());
                strFd = VB.Left(dt.Rows[i]["YYMM"].ToString().Trim(), 4) + "-" + VB.Right(dt.Rows[i]["YYMM"].ToString().Trim(), 2) + "-" + "01";
                strTd = CF.READ_LASTDAY(clsDB.DbCon, strFd.Trim());
                //'미수 종료일 조회
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(MAX(BDATE),'YYYY-MM-DD') bdate ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND WRTNO = " + nWRTNO + "";
                SQL = SQL + ComNum.VBLF + "   and to_char(bdate,'yyyy-mm-dd') >= '" + dt.Rows[i]["bdate"].ToString().Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   and to_char(bdate,'yyyy-mm-dd') <= '" + strTd + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN LIKE '2%'  ";

                SqlErr = clsDB.GetDataTable(ref dtFc, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dtFc.Rows.Count > 0)
                {
                    strGDate = dtFc.Rows[0]["bdate"].ToString().Trim();               //'미수 종료일
                }
                else
                {
                    strGDate = strTd;               //'미수 종료일
                }

                nIlsu = (int)VB.DateDiff("d",  dt.Rows[i]["Bdate"].ToString().Trim(), strGDate);

                nIlsuTot = nIlsuTot + nIlsu;//                                    '일자소계
                nIlsuTot2 = nIlsuTot2 + nIlsu;//                                 '일자 총계
                nJanAmt = VB.Val(dt.Rows[i]["ipgumamt"].ToString().Trim());
                nSlipJan = nSlipJan + (nIlsu * nJanAmt);
                nSlipJan2 = nSlipJan2 + (nIlsu * nJanAmt);
                nJanAmtTot = nJanAmtTot + nJanAmt;//                               '미수금 소계
                nAmtTot1 = nAmtTot1 + nJanAmt;//                                     '미수금 총계

                if (nIlsu < 31)
                {
                    nData[1] = nData[1] + nJanAmt;
                    nAmtData1[1] = nAmtData1[1] + nJanAmt;
                }
                else if (nIlsu > 30 && nIlsu < 61)
                {
                    nData[2] = nData[2] + nJanAmt;
                    nAmtData1[2] = nAmtData1[2] + nJanAmt;
                }
                else if (nIlsu > 60 && nIlsu < 91)
                {
                    nData[3] = nData[3] + nJanAmt;
                    nAmtData1[3] = nAmtData1[3] + nJanAmt;
                }
                else if (nIlsu > 90 && nIlsu < 181)
                {
                    nData[4] = nData[4] + nJanAmt;
                    nAmtData1[4] = nAmtData1[4] + nJanAmt;
                }
                else if (nIlsu > 180 && nIlsu < 366)
                {
                    nData[5] = nData[5] + nJanAmt;
                    nAmtData1[5] = nAmtData1[5] + nJanAmt;
                }
                else
                {
                    nData[6] = nData[6] + nJanAmt;
                    nAmtData1[6] = nAmtData1[6] + nJanAmt;
                }
            }

            if (nIlsuTot != 0)
            {
                nDayDiv = (int)(nSlipJan / nJanAmtTot);//                '평균미수일 계산
            }
            Total_TOT_Rtn();

            if (j == 4)
            {
                nJanAmtTot = nAmtTot1;

                for (i = 1; i <= 6; i++)
                {
                    nData[i] = nAmtData1[i];
                }

                if (nAmtTot1 != 0)
                {
                    nDayDiv = (int)(nSlipJan2 / nAmtTot1);
                }
                nAmtData1[0] = nDayDiv;
                Total_TOT_Rtn();
            }
            dt.Dispose();
            dt = null;

            Application.DoEvents();
        }

        /// <summary>
        /// 건진  발생일별 미수 상세내역 Display
        /// </summary>
        private void CmdOK_Data_Display3(int j)
        {
            int i = 0;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dtFc = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, A.IPGUMAMT,a.YYMM ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
            SQL = SQL + ComNum.VBLF + " WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "     AND a.YYMM >='" + FstrFYYMM + "' ";
            SQL = SQL + ComNum.VBLF + "     AND a.YYMM <='" + FstrTYYMM + "' ";
            SQL = SQL + ComNum.VBLF + "     AND a.IPGUMAMT <> 0 ";
            SQL = SQL + ComNum.VBLF + "     AND a.WRTNO=b.WRTNO(+) ";
            SQL = SQL + ComNum.VBLF + "     AND b.GJong=c.Code(+) ";

            if (j == 1)
            {
                SQL = SQL + ComNum.VBLF + "   AND b.GJONG = '83'    ";
                SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '7'    ";    //'종검
            }
            else if (j == 2)
            {
                SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG IN( '1','2','3')     ";//    '성인병, 공무원, 사업장
            }
            else if (j == 3)
            {
                SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '4'    ";   // '암검진
                SQL = SQL + ComNum.VBLF + "   AND b.GJONG IN ('31','35')    ";
            }
            else if (j == 4)
            {
                SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '4'    "; //   '학생검진
                SQL = SQL + ComNum.VBLF + "   AND b.GJONG IN ('56')    ";
            }
            else if (j == 5)
            {
                SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '4'    ";//    '기타검진
                SQL = SQL + ComNum.VBLF + "   AND b.GJONG NOT IN ('31','35','56')    ";
            }
            else if (j == 6)
            {
                SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '5'    ";//    '작업측정
            }
            else if (j == 7)
            {
                SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '6'    ";  //'보건대행

            }
            SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            nReadtot = nReadtot + dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nWRTNO = Convert.ToInt32(dt.Rows[i]["WRTNO"].ToString().Trim());

                strFd = VB.Left(dt.Rows[i]["YYMM"].ToString().Trim(), 4) + "-" + VB.Right(dt.Rows[i]["YYMM"].ToString().Trim(), 2) + "-" + "01";
                strTd = CF.READ_LASTDAY(clsDB.DbCon, strFd.Trim());

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(MAX(BDATE),'YYYY-MM-DD') bdate ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND WRTNO = " + nWRTNO + " ";

                if (dt.Rows[i]["bdate"].ToString().Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "  AND to_char(bdate,'yyyy-mm-dd') >= '" + dt.Rows[i]["bdate"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND TO_CHAR(BDate,'YYYY-MM-DD') <= '" + strTd + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND TO_CHAR(BDate,'YYYY-MM-DD') >= '" + strFd + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND TO_CHAR(BDate,'YYYY-MM-DD') <= '" + strTd + "' ";
                }
                SQL = SQL + ComNum.VBLF + " AND ( GEACODE LIKE '2%' or GEACODE ='55')  ";

                SqlErr = clsDB.GetDataTable(ref dtFc, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dtFc.Rows.Count > 0)
                {
                    strGDate = dtFc.Rows[0]["bdate"].ToString().Trim();
                }
                else
                {
                    strGDate = strTd;
                }

                //nIlsu = (int)VB.DateDiff("d", dt.Rows[i]["Bdate"].ToString().Trim(), strGDate);
                nIlsu = CF.DATE_ILSU(clsDB.DbCon, strGDate, dt.Rows[i]["Bdate"].ToString().Trim());
                nIlsuTot = nIlsuTot + nIlsu;//                                    '일자소계
                nIlsuTot2 = nIlsuTot2 + nIlsu;//                                 '일자 총계
                nJanAmt = VB.Val(dt.Rows[i]["IPGUMAMT"].ToString().Trim());
                nSlipJan = nSlipJan + (nIlsu * nJanAmt);
                nSlipJan3 = nSlipJan3 + (nIlsu * nJanAmt);
                nJanAmtTot = nJanAmtTot + nJanAmt;//                               '미수금 소계
                nAmtTot2 = nAmtTot2 + nJanAmt;//                                     '미수금 총계

                if (nIlsu < 31)
                {
                    nData[1] = nData[1] + nJanAmt;
                    nAmtData2[1] = nAmtData2[1] + nJanAmt;
                }
                else if (nIlsu > 30 && nIlsu < 61)
                {
                    nData[2] = nData[2] + nJanAmt;
                    nAmtData2[2] = nAmtData2[2] + nJanAmt;
                }
                else if (nIlsu > 60 && nIlsu < 91)
                {
                    nData[3] = nData[3] + nJanAmt;
                    nAmtData2[3] = nAmtData2[3] + nJanAmt;
                }
                else if (nIlsu > 90 && nIlsu < 181)
                {
                    nData[4] = nData[4] + nJanAmt;
                    nAmtData2[4] = nAmtData2[4] + nJanAmt;
                }
                else if (nIlsu > 180 && nIlsu < 366)
                {
                    nData[5] = nData[5] + nJanAmt;
                    nAmtData2[5] = nAmtData2[5] + nJanAmt;
                }
                else
                {
                    nData[6] = nData[6] + nJanAmt;
                    nAmtData2[6] = nAmtData2[6] + nJanAmt;
                }

            }

            if (nIlsuTot != 0)
            {
                nDayDiv = (int)(nSlipJan / nJanAmtTot);//                '평균미수일 계산
            }

            Total_TOT_Rtn();

            if (j == 7)
            {
                nJanAmtTot = nAmtTot2;

                for (i = 1; i <= 6; i++)
                {
                    nData[i] = nAmtData2[i];
                }

                nDayDiv = 0;

                if (nAmtTot2 != 0)
                {
                    nDayDiv = (int)(nSlipJan3 / nAmtTot2); //소계 미수일수
                }
                nAmtData2[0] = nDayDiv;
                Total_TOT_Rtn();    //소계 display
            }
            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 합계
        /// </summary>
        private void Total_TOT_Rtn()
        {
            int i = 0;

            nRow = nRow + 1;
            ssView_Sheet1.Cells[nRow - 1, 1].Text = nJanAmtTot.ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 2].Text = nData[1].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 3].Text = nData[2].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 4].Text = nData[3].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 5].Text = nData[4].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 6].Text = nData[5].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 7].Text = nData[6].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 8].Text = nDayDiv.ToString();

            nJanAmtTot = 0; nDayDiv = 0; nIlsuTot = 0; nIlsu = 0; nSlipJan = 0;

            for (i = 0; i <= 6; i++)
            {
                nData[i] = 0;
            }
        }

        /// <summary>
        /// 외래+센터 합계
        /// </summary>
        private void Total_All_TOT_Rtn()
        {
            nRow = nRow + 1;
            ssView_Sheet1.Cells[nRow - 1, 1].Text = (nAmtTot1 + nAmtTot2).ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 2].Text = (nAmtData1[1] + nAmtData2[1]).ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 3].Text = (nAmtData1[2] + nAmtData2[2]).ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 4].Text = (nAmtData1[3] + nAmtData2[3]).ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 5].Text = (nAmtData1[4] + nAmtData2[4]).ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 6].Text = (nAmtData1[5] + nAmtData2[5]).ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 7].Text = (nAmtData1[6] + nAmtData2[6]).ToString("###,###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 8].Text = ((int)((nAmtData1[0] + nAmtData2[0]) / 2.0)).ToString();

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            Cursor.Current = Cursors.WaitCursor;

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";
            string sFont3 = "";
            string sFoot = "";

            DateTime mdtp;
            mdtp = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            strFont1 = "/c/fn\"굴림체\" /fz\"16\"  /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "미수입금 회수기간조회 "+ cboFDate.Text + "    ";
            strHead2 = strHead2 + "/n/c" + "인쇄일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A") + VB.Space(70) + "               PAGE :" + "/p";

            btnPrint.Enabled = false;

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2 + "/n";
            ssView_Sheet1.PrintInfo.Footer = sFont3 + sFoot;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {

            GstrSQL = "";
            GstrDate = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            try
            {
                if (e.Row == 0 || e.Row == 1 || e.Row == 2 || e.Row == 3)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT  B.WRTNO, B.MISUID,a.Class,a.ipdopd,a.GelCode,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate,";
                    SQL = SQL + ComNum.VBLF + "       b.deptcode,a.ipgumamt,b.amt3,a.yymm,a.iwolamt,a.misuamt,a.sakamt,a.etcamt,  ";
                    SQL = SQL + ComNum.VBLF + "       a.janamt,a.banamt,b.remark ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_MONTHLY a," + ComNum.DB_PMPA + "MISU_IDMST b  ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM >='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   and a.YYMM <='" + FstrTYYMM + "' ";

                    if (rdoIO1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND A.IPDOPD ='I'";
                    }
                    if (rdoIO2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND A.IPDOPD ='O'";
                    }

                    SQL = SQL + ComNum.VBLF + "   and a.IPGUMAMT <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO(+) ";
                    if (e.Row == 0)
                    {
                        SQL = SQL + ComNum.VBLF + " AND a.Class in('01','02','03') ";//   '공단,직장,지역
                    }
                    else if (e.Row == 1)
                    {
                        SQL = SQL + ComNum.VBLF + " AND a.Class = '04' "; //   '보호
                    }
                    else if (e.Row == 2)
                    {
                        SQL = SQL + ComNum.VBLF + " AND a.Class = '07' ";//   '자보
                    }
                    else if (e.Row == 3)
                    {
                        SQL = SQL + ComNum.VBLF + " AND a.Class = '05' ";//  '산재
                    }

                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }

                else if (e.Row == 5)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO, '' MISUID, '' Class, '' ipdopd, TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.YYMM, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt, 0 etcamt,  ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt ,b.remark";
                    SQL = SQL + ComNum.VBLF + " FROM HIC_MISU_MONTHLY a,HIC_MISU_MST b, HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM >='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   and a.YYMM <='" + FstrTYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   and a.IPGUMAMT <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG='83'    ";        //'종검 미수 종류 = '83'
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '7'    ";    //'종검
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }

                else if (e.Row == 6)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,'' MISUID, '' Class, '' ipdopd,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.YYMM, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt,  0 etcamt, ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt,b.remark ";
                    SQL = SQL + ComNum.VBLF + " FROM HIC_MISU_MONTHLY a,HIC_MISU_MST b, HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM >='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.YYMM <='" + FstrTYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.IPGUMAMT <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG IN ('1','2','3')     ";    //'성인병, 공무원, 사업장
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }


                else if (e.Row == 7)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,'' MISUID, '' Class,'' ipdopd,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.YYMM, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt, 0 etcamt,  ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt,b.remark ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM >='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.YYMM <='" + FstrTYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.IPGUMAMT <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '4'    ";   // '암검진
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG IN ('31','35')    ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }

                else if (e.Row == 8)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,'' MISUID, '' Class,'' ipdopd,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.YYMM, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt,  0 etcamt, ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt ,b.remark";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM >='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.YYMM <='" + FstrTYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.IPGUMAMT <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '4'    ";    //'학생검진
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG IN ('56')    ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }

                else if (e.Row == 9)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,'' MISUID ,'' Class,'' ipdopd,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.YYMM, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt,   0 etcamt,";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt ,b.remark";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM >='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.YYMM <='" + FstrTYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.IPGUMAMT <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '4'    ";    //'기타검진
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG NOT IN ('31','35','56')    ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }

                else if (e.Row == 10)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,'' MISUID, '' Class,'' ipdopd,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.YYMM, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt,  0 etcamt, ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt ,b.remark";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM >='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.YYMM <='" + FstrTYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.IPGUMAMT <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '5'    ";    //'작업측정
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }

                else if (e.Row == 11)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,'' MISUID, '' Class,'' ipdopd,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.YYMM, ";
                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt, 0 etcamt,  ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt ,b.remark";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b, " + ComNum.DB_PMPA + "HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM >='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.YYMM <='" + FstrTYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.IPGUMAMT <> 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
                    SQL = SQL + ComNum.VBLF + "   AND c.MISUJONG = '6'    ";    //'보건대행
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (SQL != "")
                {
                    GstrRet = e.Row.ToString();
                    frmPmpaViewGiganDetail frm = new frmPmpaViewGiganDetail(SQL, GstrRet);
                    frm.Show();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }
    }
}
