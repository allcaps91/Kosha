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
    /// Create Date     : 2017-10-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref=  D:\psmh\misu\misubs\misubs.vbp\misubs08.frm(FrmMonthCheck.frm) >> frmPmpaViewMisubs08.cs 폼이름 재정의" /> 

    public partial class frmPmpaViewMisubs08 : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        double[,,] FnAmt = new double[5, 6, 6];


        string strYYMM = "";
        string strFDate = "";
        string strTdate = "";
        string strBi = "";
        int nBiNo = 0;
        double nAmt = 0;
        double nJepJAmt = 0;
        int nRow = 0;
        string strBiGbn = "";
        int nX = 0;
        int nY = 0;
        int nZ = 0;
        string strJungFDate = "";
        string strJungTDate = "";


        public frmPmpaViewMisubs08()
        {
            InitializeComponent();
        }

        private void frmPmpaViewMisubs08_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFDate, 36, "", "1");
            cboFDate.DropDownStyle = ComboBoxStyle.DropDown;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            int K = 0;
            string SQL = "";    //Query문

            for (i = 1; i <= 4; i++)
            {
                for (j = 1; j <= 5; j++)
                {
                    for (K = 1; K <= 5; K++)
                    {
                        FnAmt[i, j, K] = 0;
                    }
                }
            }

            //ssView_Sheet1.Cells[0, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                for (j = 2; j < ssView_Sheet1.ColumnCount; j++)
                {
                    ssView_Sheet1.Cells[i, j].Text = "";
                }
            }
  
            strYYMM = VB.Left(cboFDate.Text, 4) + VB.Mid(cboFDate.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTdate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;

            try
            {
                //청구액점검표
                JOHAP_MISU_READ();
                Em6TimveOver_READ();
                ChungGu_Amt_Read();
                JunGan_Amt_Read();

                //개인별(조합미수)
                JOHAP_MISU_READ2();


                //DISPLAY
                for (i = 1; i <= 4; i++)
                {
                    for (j = 1; j <= 5; j++)
                    { 
                        for (K = 1; K <= 5; K++)
                        {
                            ssView_Sheet1.Cells[(j - 1) * 5 + K - 1, (2 + i) - 1].Text = FnAmt[i, j, K].ToString("##,###,###,##0");
                        }
                    }
                }

                //차액
                for (i = 1; i <= ssView_Sheet1.RowCount; i++)
                {
                    nAmt = Convert.ToDouble(Convert.ToDouble(ssView_Sheet1.Cells[i - 1, 3 - 1].Text));
                    nAmt = nAmt - Convert.ToDouble(Convert.ToDouble(ssView_Sheet1.Cells[i - 1, 5 - 1].Text));
                    ssView_Sheet1.Cells[i - 1, 7 - 1].Text = nAmt.ToString("##,###,###,###,##0");

                    nAmt = Convert.ToDouble(ssView_Sheet1.Cells[i - 1, 4 - 1].Text);
                    nAmt = nAmt - (Convert.ToDouble(ssView_Sheet1.Cells[i - 1, 6 - 1].Text));
                    ssView_Sheet1.Cells[i - 1, 8 - 1].Text = nAmt.ToString("##,###,###,###,##0");
                }
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
        /// 외래/입원 조합부담 발생액을 ADD
        /// </summary>
        private void JOHAP_MISU_READ()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT BiGbn, IPDOPD, SUM(Amt33) Amt";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_BALDAILY ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "  AND ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND ActDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";

            if (strBiGbn != "")
            {
                SQL = SQL + ComNum.VBLF + " AND BIGBN IN (" + strBiGbn + " ) ";
            }

            SQL = SQL + ComNum.VBLF + "GROUP BY BiGbn, IPDOPD ";

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
            for (i = 0; i < dt.Rows.Count; i++)
            {
                nBiNo = (int)VB.Val(dt.Rows[i]["BiGbn"].ToString().Trim());
                nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());

                if (nAmt != 0)
                {
                    if (dt.Rows[i]["IPDOPD"].ToString().Trim() == "O")
                    {
                        nY = 1;
                    }
                    else
                    {
                        nY = 2;
                    }

                    switch (nBiNo)
                    {
                        case 1:
                            nZ = 1;
                            break;
                        case 2:
                            nZ = 2;
                            break;
                        case 3:
                            nZ = 3;
                            break;
                        case 4:
                            nZ = 4;
                            break;
                        default:
                            nZ = 1;
                            break;
                    }
                }

                FnAmt[1, nY, nZ] = FnAmt[1, nY, nZ] + nAmt;
                FnAmt[1, nY, 5] = FnAmt[1, nY, 5] + nAmt;
            }

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 응급실6시간이상,NP낮병동 조합부담액 ADD
        /// </summary>
        private void Em6TimveOver_READ()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(ACTDATE,'YYYYMM') YYYYMM,  SuBi,SUM(Johap) Amt, SUM(JepJAmt) JepJAmt  ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "  AND ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND ActDate <= TO_DATE ('" + strTdate + "' , 'YYYY-MM-DD')"; 
            SQL = SQL + ComNum.VBLF + "  AND Gubun='3' ";
            if (strBiGbn != "")
            {
                SQL = SQL + ComNum.VBLF + " AND SUBI IN (" + strBiGbn + " ) ";
            } 
            
            SQL = SQL + ComNum.VBLF + "GROUP BY SuBi, TO_CHAR(ACTDATE,'YYYYMM')  ";

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

            nX = 1;// '청구액점검표(조합미수)

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nBiNo = (int)VB.Val(dt.Rows[i]["SuBi"].ToString().Trim());
                nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                nJepJAmt = VB.Val(dt.Rows[i]["JEPJAMT"].ToString().Trim());

                if (nAmt != 0)
                {
                    switch (nBiNo)
                    {
                        case 1:
                            nZ = 1;
                            break;
                        case 2:
                            nZ = 2;
                            break;
                        case 3:
                            nZ = 3;
                            break;
                        case 4:
                            nZ = 4;
                            break;
                        default:
                            nZ = 1;
                            break;
                    }

                    if (nBiNo == 1 && string.Compare(dt.Rows[i]["YYYYMM"].ToString().Trim(), "201601") >= 0)
                    {
                        FnAmt[2, 2, nZ] = FnAmt[2, 2, nZ] - nJepJAmt;
                        FnAmt[2, 2, 5] = FnAmt[2, 2, 5] - nJepJAmt;
                    }

                    else if (nBiNo == 2 && string.Compare(dt.Rows[i]["YYYYMM"].ToString().Trim(), "201602") >= 0)
                    {
                        FnAmt[2, 2, nZ] = FnAmt[2, 2, nZ] - nJepJAmt;
                        FnAmt[2, 2, 5] = FnAmt[2, 2, 5] - nJepJAmt;
                    }

                    FnAmt[1, 1, nZ] = FnAmt[1, 1, nZ] - nAmt;
                    FnAmt[1, 1, 5] = FnAmt[1, 1, 5] - nAmt;
                    FnAmt[1, 4, nZ] = FnAmt[1, 4, nZ] + nAmt;
                    FnAmt[1, 4, 5] = FnAmt[1, 4, 5] + nAmt;
                    FnAmt[2, 4, nZ] = FnAmt[2, 4, nZ] + nJepJAmt;
                    FnAmt[2, 4, 5] = FnAmt[2, 4, 5] + nJepJAmt;
                }
            }

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 청구액
        /// </summary>
        private void ChungGu_Amt_Read()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT Class, IPDOPD, SUM(Amt2) Amt ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "  AND MirYYMM = '" + strYYMM + "' ";

            if (strBiGbn != "")
            {
                SQL = SQL + ComNum.VBLF + " AND CLASS IN (" + strBiGbn + " ) ";
            }

            SQL = SQL + ComNum.VBLF + "   AND TongGbn IN ('1') ";
            SQL = SQL + ComNum.VBLF + "GROUP BY Class, IPDOPD ";

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

            nX = 2; // '청구액점검표(청구액)

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nBiNo = (int)VB.Val(dt.Rows[i]["Class"].ToString().Trim());
                nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());

                if (nAmt != 0)
                {
                    if (dt.Rows[i]["IPDOPD"].ToString().Trim() == "O")
                    {
                        nY = 1;
                    }
                    else
                    {
                        nY = 2;
                    }

                    switch (nBiNo)
                    {
                        case 1:
                        case 2:
                        case 3:
                            nZ = 1;
                            break;
                        case 4:
                            nZ = 2;
                            break;
                        case 5:
                            nZ = 3;
                            break;
                        case 7:
                            nZ = 4;
                            break;
                        default:
                            nZ = 1;
                            break;
                    }
                }

                FnAmt[nX, nY, nZ] = FnAmt[nX, nY, nZ] + nAmt;
                FnAmt[nX, nY, 5] = FnAmt[nX, nY, 5] + nAmt;
            }

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 중간청구
        /// </summary>
        private void JunGan_Amt_Read()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수



            strJungFDate = Convert.ToDateTime(CF.READ_LASTDAY(clsDB.DbCon, strFDate)).AddDays(1).ToString("yyyy-MM-dd");  //DATE_ADD (READ_LASTDAY (clsDB.DbCon, strFDate) , 1)
            strJungTDate = Convert.ToDateTime(CF.READ_LASTDAY(clsDB.DbCon, strTdate)).AddDays(1).ToString("yyyy-MM-dd");//DATE_ADD (READ_LASTDAY (clsDB.DbCon, strTdate) , 1)
            strJungTDate = Convert.ToDateTime(CF.READ_LASTDAY(clsDB.DbCon, strJungTDate)).AddDays(1).ToString("yyyy-MM-dd");//DATE_ADD (READ_LASTDAY (clsDB.DbCon, strJungTDate) , 1)

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT Bi, SUBI,SUM(BuildJAmt) Amt,SUM(JepJAmt) JepJAmt";
            SQL = SQL + ComNum.VBLF + " FROM MIR_IPDID ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "  AND YYMM>='200112' ";
            SQL = SQL + ComNum.VBLF + "  AND BuildDate>=TO_DATE('" + strJungFDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND BuildDate< TO_DATE('" + strJungTDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND Flag='1' ";

            if (strBiGbn != "")
            {
                SQL = SQL + ComNum.VBLF + " AND SUBI IN (" + strBiGbn + " ) ";
            }

            SQL = SQL + ComNum.VBLF + "GROUP BY Bi, SUBI, Pano";

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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nBiNo = (int)VB.Val(dt.Rows[i]["SUBI"].ToString().Trim());

                if (dt.Rows[i]["SUBI"].ToString().Trim() == "")
                {
                    nBiNo = CPM.READ_Bi_SuipTong(dt.Rows[i]["Bi"].ToString().Trim(), strJungFDate);
                }

                nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());

                switch (nBiNo)
                {
                    case 1:
                        nZ = 1;
                        break;
                    case 2:
                        nZ = 2;
                        break;
                    case 3:
                        nZ = 3;
                        break;
                    case 4:
                        nZ = 4;
                        break;
                    default:
                        nZ = 1;
                        break;
                }
                //'중간청구 대상 금액
                FnAmt[1, 3, nZ] = FnAmt[1, 3, nZ] + nAmt;
                FnAmt[1, 3, 5] = FnAmt[1, 3, 5] + nAmt;
                FnAmt[3, 3, nZ] = FnAmt[3, 3, nZ] + nAmt;
                FnAmt[3, 3, 5] = FnAmt[3, 3, 5] + nAmt;


                //'중간청구 접수 금액
                nAmt = VB.Val(dt.Rows[i]["JepJAmt"].ToString().Trim());
                FnAmt[2, 3, nZ] = FnAmt[2, 3, nZ] + nAmt;
                FnAmt[2, 3, 5] = FnAmt[2, 3, 5] + nAmt;
                FnAmt[4, 3, nZ] = FnAmt[4, 3, nZ] + nAmt;
                FnAmt[4, 3, 5] = FnAmt[4, 3, 5] + nAmt;
            }

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 개인별(조합미수)
        /// </summary>
        private void JOHAP_MISU_READ2()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = "SELECT SUBI, IPDOPD, GUBUN, SUM(JOHAP) JOHAP, SUM(JEPJAMT) JEPJAMT ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
            SQL = SQL + ComNum.VBLF + " WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "   AND YYMM ='" + strYYMM + "' ";
            SQL = SQL + ComNum.VBLF + "  GROUP BY SUBI, IPDOPD, GUBUN  ";

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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nBiNo = (int)VB.Val(dt.Rows[i]["SUBI"].ToString().Trim());

                if (dt.Rows[i]["SUBI"].ToString().Trim() == "")
                {
                    nBiNo = CPM.READ_Bi_SuipTong(dt.Rows[i]["Bi"].ToString().Trim(), strJungFDate);
                }

                nAmt = VB.Val(dt.Rows[i]["JOHAP"].ToString().Trim());

                if (dt.Rows[i]["GUBUN"].ToString().Trim() == "3")
                {
                    nY = 4; // '응급6시간;
                }
                else
                {
                    if (dt.Rows[i]["IPDOPD"].ToString().Trim() == "O")
                    {
                        nY = 1;
                    }
                    else
                    {
                        nY = 2;
                    }
                }

                switch (nBiNo)
                {
                    case 1:
                        nZ = 1;
                        break;
                    case 2:
                        nZ = 2;
                        break;
                    case 3:
                        nZ = 3;
                        break;
                    case 4:
                        nZ = 4;
                        break;
                    default:
                        nZ = 1;
                        break;
                }

                //'중간청구 대상 금액
                FnAmt[3, nY, nZ] = FnAmt[3, nY, nZ] + nAmt;
                FnAmt[3, nY, 5] = FnAmt[3, nY, 5] + nAmt;
                //'중간청구 접수 금액
                nAmt = VB.Val(dt.Rows[i]["JepJAmt"].ToString().Trim());
                FnAmt[4, nY, nZ] = FnAmt[4, nY, nZ] + nAmt;
                FnAmt[4, nY, 5] = FnAmt[4, nY, 5] + nAmt;
            }

            dt.Dispose();
            dt = null;

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = cboFDate.Text + " 월별 통계 점검(입원/외래)";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 0, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog mDlg = new SaveFileDialog())
            {
                mDlg.InitialDirectory = Application.StartupPath;
                mDlg.Filter = "Excel files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                mDlg.FilterIndex = 1;
                if (mDlg.ShowDialog() == DialogResult.OK)
                {
                    ssView.SaveExcel(mDlg.FileName,
                    FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
                }
            }
        }
    }
}
