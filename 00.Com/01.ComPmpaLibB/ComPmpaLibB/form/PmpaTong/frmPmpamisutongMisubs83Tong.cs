using System;
using System.Data;
using System.Windows.Forms;
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
    /// <seealso cref= d:\psmh\misu\misutong\misutong.vbp\misutong02.frm" >> frmPmpaViewIpgumGigan_NEW2014.cs 폼이름 재정의" />
    /// <seealso cref=d:\psmh\misu\misubs\misubs.vbp\misubs83.frm" >> frmPmpaViewIpgumGigan_NEW2014.cs 폼이름 재정의" />

    public partial class frmPmpamisutongMisubs83Tong : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

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
        int nReadTot = 0;
        double nAmtTot1 = 0; //      ' 외래금액
        double nAmtTot2 = 0; //      ' 건진금액
        double[] nData = new double[7];          //' 부분별 미수잔액합계
        double[] nAmtData1 = new double[7];      //' 전체미수잔액합계
        double[] nAmtData2 = new double[7];      //' 전체미수잔액합계

        public frmPmpamisutongMisubs83Tong()
        {
            InitializeComponent();
        }

        public frmPmpamisutongMisubs83Tong(string strGstrSQL, string strGstrDate, string strGstrRet)
        {

            GstrSQL = strGstrSQL;
            GstrDate = strGstrDate;
            GstrRet = strGstrRet;

            InitializeComponent();
        }

        private void frmPmpamisutongMisubs83Tong_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int nMM = 0;

            nYY = (int)VB.Val(VB.Left(strDTP, 4));
            nMM = (int)VB.Val(VB.Mid(strDTP, 6, 2)) - 1;

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

            nAmtTot1 = 0;
            nAmtTot2 = 0;
            nSlipJan2 = 0;
            nSlipJan3 = 0;

            for (i = 0; i <= 6; i++)
            {
                nAmtData1[i] = 0;
                nAmtData2[i] = 0;
            }

            FstrFYYMM = VB.Left(cboFDate.Text, 4) + VB.Right(cboFDate.Text, 2);
            FstrTYYMM = VB.Left(cboTDate.Text, 4) + VB.Right(cboTDate.Text, 2);

            //try
            //{
            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";


            ssView_Sheet1.RowCount = 50;

            for (j = 1; j <= 4; j++)
            {
                CmdOK_Data_Display(j);               //'일반 미수
            }

            CmdOK_Data_Display2(k);                //'종검 미수
            CmdOK_Data_Display3(k);            //'건진 미수

            Cursor.Current = Cursors.Default;
            btnSearch.Enabled = true;
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
            SQL = SQL + ComNum.VBLF + "       a.ipgumamt,b.amt3,a.yymm  ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_MONTHLY a," + ComNum.DB_PMPA + "MISU_IDMST b  ";
            SQL = SQL + ComNum.VBLF + " WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "   AND a.YYMM >='" + FstrFYYMM + "' ";
            SQL = SQL + ComNum.VBLF + "   AND a.YYMM <='" + FstrTYYMM + "' ";

            SQL = SQL + ComNum.VBLF + "   and a.IPGUMAMT > 0 ";
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

            nReadTot = nReadTot + dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nWRTNO = Convert.ToInt32(dt.Rows[i]["WRTNO"].ToString().Trim());
                strFd = VB.Left(dt.Rows[i]["YYMM"].ToString().Trim(), 4) + "-" + VB.Right(dt.Rows[i]["YYMM"].ToString().Trim(), 2) + "-" + "01";
                strTd = CF.READ_LASTDAY(clsDB.DbCon, strFd.Trim());

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(MAX(BDATE),'YYYY-MM-DD') bdate ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND WRTNO = " + nWRTNO + "";
                SQL = SQL + ComNum.VBLF + "   and to_char(bdate,'yyyy-mm-dd') >= '" + strFd + "' ";
                SQL = SQL + ComNum.VBLF + "   and to_char(bdate,'yyyy-mm-dd') <= '" + strTd + "' ";

                SqlErr = clsDB.GetDataTable(ref dtFc, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                strGDate = dtFc.Rows[0]["bdate"].ToString().Trim();

                nIlsu = (int)VB.DateDiff("d", dt.Rows[i]["Bdate"].ToString().Trim(), strGDate);

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

                nDayDiv = (int)(nSlipJan2 / nAmtTot1);

                nAmtData1[0] = nDayDiv;
                Total_TOT_Rtn();
            }
            dt.Dispose();
            dt = null;

            Application.DoEvents();
        }

        /// <summary>
        /// 종검  발생일별 미수 상세내역 Display
        /// </summary>
        /// <param name="j"></param>
        private void CmdOK_Data_Display2(int j)
        {
            int i = 0;
            DataTable dt = null;
            DataTable dtFc = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, b.IPGUMAMT,a.yymm ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_MISU_MONTHLY a,HIC_MISU_MST b ";
            SQL = SQL + ComNum.VBLF + " WHERE a.YYMM >='" + FstrFYYMM + "' ";
            SQL = SQL + ComNum.VBLF + "   and a.YYMM <='" + FstrTYYMM + "' ";
            SQL = SQL + ComNum.VBLF + "   and a.IPGUMAMT > 0 ";
            SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO(+) ";
            SQL = SQL + ComNum.VBLF + "   AND b.GJONG='83'    ";          //'종검 미수 종류 = '83'
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

            nReadTot = nReadTot + dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nWRTNO = Convert.ToInt32(dt.Rows[i]["WRTNO"].ToString().Trim());
                strFd = VB.Left(dt.Rows[i]["YYMM"].ToString().Trim(), 4) + "-" + VB.Right(dt.Rows[i]["YYMM"].ToString().Trim(), 2) + "-" + "01";
                strTd = CF.READ_LASTDAY(clsDB.DbCon, strFd.Trim());

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(MAX(BDATE),'YYYY-MM-DD') bdate ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND WRTNO = " + nWRTNO + "";
                SQL = SQL + ComNum.VBLF + "   and to_char(bdate,'yyyy-mm-dd') >= '" + strFd + "' ";
                SQL = SQL + ComNum.VBLF + "   and to_char(bdate,'yyyy-mm-dd') <= '" + strTd + "' ";

                SqlErr = clsDB.GetDataTable(ref dtFc, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dtFc.Rows[0]["bdate"].ToString().Trim() != "")
                {
                    strGDate = dtFc.Rows[0]["bdate"].ToString().Trim();

                    nIlsu = (int)VB.DateDiff("d", Convert.ToDateTime(dt.Rows[i]["Bdate"].ToString().Trim()), Convert.ToDateTime(strGDate));
                }

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
            SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, b.IPGUMAMT,a.yymm ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY a," + ComNum.DB_PMPA + "HIC_MISU_MST b ";
            SQL = SQL + ComNum.VBLF + " WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "     AND a.YYMM >='" + FstrFYYMM + "' ";
            SQL = SQL + ComNum.VBLF + "     AND a.YYMM <='" + FstrTYYMM + "' ";
            SQL = SQL + ComNum.VBLF + "     AND a.IPGUMAMT > 0 ";
            SQL = SQL + ComNum.VBLF + "     AND a.WRTNO=b.WRTNO(+) ";
            SQL = SQL + ComNum.VBLF + "     AND b.GJONG <> '83'    ";
            SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            nReadTot = nReadTot + dt.Rows.Count;

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
                SQL = SQL + ComNum.VBLF + "   and to_char(bdate,'yyyy-mm-dd') >= '" + strFd + "' ";
                SQL = SQL + ComNum.VBLF + "   and to_char(bdate,'yyyy-mm-dd') <= '" + strTd + "' ";

                SqlErr = clsDB.GetDataTable(ref dtFc, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                strGDate = dtFc.Rows[0]["bdate"].ToString().Trim();

                nIlsu = (int)VB.DateDiff("d", dt.Rows[i]["Bdate"].ToString().Trim(), strGDate);
                nIlsuTot = nIlsuTot + nIlsu;
                nIlsuTot2 = nIlsuTot2 + nIlsu;
                nJanAmt = VB.Val(dt.Rows[i]["IPGUMAMT"].ToString().Trim());
                nSlipJan = nSlipJan + (nIlsu * nJanAmt);
                nSlipJan3 = nSlipJan3 + (nIlsu * nJanAmt);
                nJanAmtTot = nJanAmtTot + nJanAmt;
                nAmtTot2 = nAmtTot2 + nJanAmt;


                if (nIlsu >= 1 && nIlsu < 31)
                {
                    nData[1] = nData[1] + nJanAmt;
                    nAmtData2[1] = nAmtData2[1] + nJanAmt;
                }
                else if (nIlsu >= 30 && nIlsu < 61)
                {
                    nData[2] = nData[2] + nJanAmt;
                    nAmtData2[2] = nAmtData2[2] + nJanAmt;
                }
                else if (nIlsu >= 60 && nIlsu < 91)
                {
                    nData[3] = nData[3] + nJanAmt;
                    nAmtData2[3] = nAmtData2[3] + nJanAmt;
                }
                else if (nIlsu >= 90 && nIlsu < 181)
                {
                    nData[4] = nData[4] + nJanAmt;
                    nAmtData2[4] = nAmtData2[4] + nJanAmt;
                }
                else if (nIlsu >= 180 && nIlsu < 366)
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

            nJanAmtTot = nAmtTot2;

            for (i = 1; i <= 6; i++)
            {
                nData[i] = nAmtData2[i];
            }

            nDayDiv = (int)(nSlipJan2 / nAmtTot2); //소계 미수일수

            Total_TOT_Rtn();


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


            for (i = 0; i <= 6; i++)
            {
                nData[i] = 0;
            }

            nJanAmtTot = 0;
            nDayDiv = 0;
            nIlsuTot = 0;
            nIlsu = 0;
            nSlipJan = 0;
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
            ssView_Sheet1.Cells[nRow - 1, 8].Text = ((nAmtData1[0] + nAmtData2[0]) / 2).ToString();

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
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
                    SQL = SQL + ComNum.VBLF + "SELECT  B.MISUID WRTNO,a.Class,a.ipdopd,a.GelCode,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate,";
                    SQL = SQL + ComNum.VBLF + "       b.deptcode,a.ipgumamt,b.amt3,a.yymm,a.iwolamt,a.misuamt,a.sakamt,a.etcamt,  ";
                    SQL = SQL + ComNum.VBLF + "       a.janamt,a.banamt,b.remark ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_MONTHLY a," + ComNum.DB_PMPA + "MISU_IDMST b  ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM >='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   and a.YYMM <='" + FstrTYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   and a.IPGUMAMT > 0 ";
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

                    SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.yymm, ";


                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt, 0 etcamt,  ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt ,b.remark";
                    SQL = SQL + ComNum.VBLF + " FROM HIC_MISU_MONTHLY a,HIC_MISU_MST b, HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM >='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   and a.YYMM <='" + FstrTYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   and a.IPGUMAMT > 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJong=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG='83'    ";        //'종검 미수 종류 = '83'
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.wrtno,b.BDate ";
                }

                else if (e.Row == 6)
                {


                    SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, a.IPGUMAMT,a.yymm, ";

                    SQL = SQL + ComNum.VBLF + "        a.iwolamt,a.misuamt,a.sakamt,  0 etcamt, ";
                    SQL = SQL + ComNum.VBLF + "        a.janamt,a.banamt,b.remark ";
                    SQL = SQL + ComNum.VBLF + " FROM HIC_MISU_MONTHLY a,HIC_MISU_MST b, HIC_EXJONG c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.YYMM >='" + FstrFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.YYMM <='" + FstrTYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   and a.IPGUMAMT > 0 ";
                    SQL = SQL + ComNum.VBLF + "   AND a.WRTNO=b.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND b.GJONG <> '83'    ";
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

                GstrSQL = SQL;

                if (GstrSQL != "")
                {
                    GstrRet = e.Row.ToString();

                    frmPmpaViewGiganDetail frm = new frmPmpaViewGiganDetail(GstrSQL, GstrRet);
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
