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
    /// Create Date     : 2017-10-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref=  d:\psmh\misu\misubs\misubs57.frm" >> frmPmpaViewmisubs54.cs 폼이름 재정의" />
    /// <seealso cref=  d:\psmh\misu\misubs\misubs53.frm" >> frmPmpaViewmisubs54.cs 폼이름 재정의" />
    /// <seealso cref=  d:\psmh\misu\misubs\misubs54.frm" >> frmPmpaViewmisubs54.cs 폼이름 재정의" />

    public partial class frmPmpaViewmisubs54 : Form
    {

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        double[,] FnAmt = new double[16, 6];
        string GstrYYMM = "";
        //string GstrMenu = "";
        //string GstrSMenu = "";

        string GstrRetValue = "";

        public frmPmpaViewmisubs54()
        {
            InitializeComponent();
        }

        public frmPmpaViewmisubs54(string strstrYYMM, string strstrMenu, string strstrSMenu)
        {
            string GstrYYMM = strstrYYMM;
            string GstrMenu = strstrMenu;
            string GstrSMenu = strstrSMenu;

            InitializeComponent();
        }

        private void frmPmpaViewmisubs54_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 24, "", "2");

            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboYYMM.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }

            cboJong.Items.Clear();
            cboJong.Items.Add("0.전체");
            cboJong.Items.Add("1.건강보험");
            cboJong.Items.Add("2.의료급여");
            cboJong.Items.Add("3.산재");
            cboJong.Items.Add("4.자보");
            cboJong.SelectedIndex = 0;

            cboJob.Items.Add("*전체");
            cboJob.Items.Add("0.누락");
            cboJob.Items.Add("1.차이금액 5% 이상");
            cboJob.Items.Add("2.차이금액 10% 이상");
            cboJob.Items.Add("3.+5만원 이상");
            cboJob.Items.Add("4.-5만원 이하");
            cboJob.Items.Add("5.+2만원 이상");
            cboJob.Items.Add("6.-2만원 이하");
            cboJob.SelectedIndex = 0;

            TabSet();

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFDate, 24, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTDate, 24, "", "1");
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabSet();
        }

        private void TabSet()
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    this.Text = "월별 퇴원자 심사조정 상세내역 (개인별)";
                    lblTitle.Text = "월별 퇴원자 심사조정 상세내역 (개인별)";
                    if (chkOptMir.Checked == true)
                    {
                        pnachk.Visible = true;
                        SS1.Dock = DockStyle.Fill;
                        SS1.Visible = true;
                        SS2.Dock = DockStyle.None;
                        SS2.Visible = false;

                    }
                    else
                    {
                        pnachk.Visible = false;
                        SS2.Dock = DockStyle.Fill;
                        SS2.Visible = true;
                        SS1.Dock = DockStyle.None;
                        SS1.Visible = false;
                    }
                    break;
                case 1:
                    this.Text = "월별 퇴원자 심사조정액";
                    lblTitle.Text = "월별 퇴원자 심사조정액";
                    Tab1SS_SET();
                    break;
            }
        }

        private void Tab1SS_SET()
        {
            if (chk0.Checked == true) //'건강보험
            {
                SS3_Sheet1.Rows[0].Visible = true;
            }
            else
            {
                SS3_Sheet1.Rows[0].Visible = false;
            }

            if (chk1.Checked == true) //'의료급여
            {
                SS3_Sheet1.Rows[1].Visible = true;
            }
            else
            {
                SS3_Sheet1.Rows[1].Visible = false;
            }

            if (chk3.Checked == true) //'자보
            {
                SS3_Sheet1.Rows[2].Visible = true;
            }
            else
            {
                SS3_Sheet1.Rows[2].Visible = false;
            }

            if (chk2.Checked == true) //'산재
            {
                SS3_Sheet1.Rows[3].Visible = true;
            }
            else
            {
                SS3_Sheet1.Rows[3].Visible = false;
            }

            SS3_Sheet1.Columns[0].Visible = false;

            //Clear
            SS3_Sheet1.Cells[0, 2, SS3_Sheet1.RowCount - 1, SS3_Sheet1.ColumnCount - 1].Text = "";

        }

        private void rdoOptMir_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptMir.Checked == true)
            {

                SS1_Sheet1.Columns[7].Visible = false;
                SS1_Sheet1.Columns[8].Visible = false;
                SS1_Sheet1.Columns[10].Visible = false;
                SS1_Sheet1.Columns[11].Visible = false;
                SS1_Sheet1.Columns[12].Visible = false;

                pnachk.Visible = true;
                SS2.Dock = DockStyle.None;
                SS2.Visible = false;
                SS1.Dock = DockStyle.Fill;
                SS1.Visible = true;
            }
            else
            {
                pnachk.Visible = false;
                SS1.Dock = DockStyle.None;
                SS1.Visible = false;
                SS2.Dock = DockStyle.Fill;
                SS2.Visible = true;
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
            string strYYMM = "";
            string strFDate = "";
            string strTdate = "";
            string strJong = "";
            string strBi = "";
            string strOK = "";

            double nToAmtA = 0;
            double nToAmtB = 0;
            double nChaAmt = 0;
            double nChaRate = 0;
            double nTotAmtA = 0;
            double nTotAmtB = 0;
            double nTotChaAmt = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;

            SS1_Sheet1.RowCount = 0;
            nTotAmtA = 0;
            nTotAmtB = 0;
            nTotChaAmt = 0;

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);

            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTdate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            strJong = VB.Left(cboJong.Text, 1);
            try
            {
                if (chkOptMir.Checked == true)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT  Pano,Bi,SName, SUBI, TOTAMT,  SIMAMT,  SIMSAMEMO, ROWID , IPDNO, TRSNO ";
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
                    SQL = SQL + ComNum.VBLF + "         WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "              AND ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "              AND ACTDATE <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL = "";
                    SQL = "     SELECT A.PANO,  A.SNAME, A.BI, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE,  TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE, A.TOTAMT,  B.REMARK ";
                    SQL = SQL + ComNum.VBLF + " FROM  " + ComNum.DB_PMPA + "MISU_BALTEWON A, " + ComNum.DB_PMPA + "MIR_ILLS B";
                    SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND ACTDATE <=TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PANO ";
                    SQL = SQL + ComNum.VBLF + "    AND A.IPDNO = B.IPDNO";
                    SQL = SQL + ComNum.VBLF + "    AND A.TRSNO = B.TRSNO ";
                    SQL = SQL + ComNum.VBLF + "    AND B.RANK ='0'";
                    SQL = SQL + ComNum.VBLF + "    AND B.REMARK IS NOT NULL";

                }

                if (strJong != "0")
                {
                    SQL = SQL + ComNum.VBLF + " AND SuBi = '" + strJong + "' ";
                }

                if (strJong == "3")
                    SQL = SQL + ComNum.VBLF + "ORDER BY Bi,SNAME,Pano ";
                else
                    SQL = SQL + ComNum.VBLF + "ORDER BY Bi,Pano ";

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


                if (chkOptMir.Checked == true)
                {
                    //스프레드 출력문
                    SS1_Sheet1.RowCount = dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {

                        strBi = dt.Rows[i]["Bi"].ToString().Trim();
                        nToAmtA = VB.Val(dt.Rows[i]["TOTAMT"].ToString().Trim()) - VB.Val(dt.Rows[i]["SIMAMT"].ToString().Trim());
                        nToAmtB = VB.Val(dt.Rows[i]["TOTAMT"].ToString().Trim());
                        nChaAmt = nToAmtB - nToAmtA;
                        nChaRate = 0;

                        if (nChaAmt != 0 && nTotAmtB != 0)
                            nChaRate = (nChaAmt / nTotAmtB) * 100;

                        strOK = "NO";

                        switch (VB.Left(cboJob.Text, 1))
                        {
                            case "*":
                                strOK = "OK";
                                break;
                            case "1":
                                if (nToAmtA != 0 && nToAmtB == 0)
                                    strOK = "OK";
                                break;
                            case "2":
                                if (nToAmtA != 0 && nToAmtB == 0)
                                    strOK = "OK";
                                if (nChaRate > 5 || nChaRate < -5)
                                    strOK = "OK";
                                break;
                            case "3":
                                if (nToAmtB >= 50000)
                                    strOK = "OK";
                                break;
                            case "4":
                                if (nToAmtB <= -50000)
                                    strOK = "OK";
                                break;
                            case "5":
                                if (nToAmtB >= 20000)
                                    strOK = "OK";
                                break;
                            case "6":
                                if (nToAmtB <= -20000)
                                    strOK = "OK";
                                break;
                            default:
                                if (nToAmtA != 0 && nTotAmtB == 0)
                                    strOK = "OK";
                                break;
                        }

                        if (strOK == "OK")
                        {
                            nTotAmtA = nTotAmtA + nToAmtA;
                            nTotAmtB = nTotAmtB + nToAmtB;
                            nTotChaAmt = nTotChaAmt + nChaAmt;

                            //nRow = nRow + 1;
                            //if (nRow > SS1_Sheet1.RowCount)
                            //{
                            //    SS1_Sheet1.RowCount = nRow;
                            //}

                            SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                            SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Bi"].ToString().Trim();
                            SS1_Sheet1.Cells[i, 3].Text = nToAmtA.ToString("###,###,###,##0");
                            SS1_Sheet1.Cells[i, 5].Text = nToAmtB.ToString("###,###,###,##0");
                            SS1_Sheet1.Cells[i, 6].Text = nChaAmt.ToString("###,###,###,##0");
                            SS1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SIMSAMEMO"].ToString().Trim();
                            SS1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                            SS1_Sheet1.Cells[i, 9].Text = nTotAmtB.ToString("###,###,###,##0");
                            SS1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["SIMSAMEMO"].ToString().Trim();
                            SS1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                            SS1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["TRSNO"].ToString().Trim();
                        }
                    }

                    //                    nRow = nRow + 1;
                    SS1_Sheet1.RowCount = SS1_Sheet1.RowCount + 1;

                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = "**합 계**";
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 3].Text = nTotAmtA.ToString("###,###,###,##0");
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 5].Text = nTotAmtB.ToString("###,###,###,##0");
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 6].Text = nTotChaAmt.ToString("###,###,###,##0");
                }
                else if (chkOptMir.Checked == false)
                {
                    //스프레드 출력문
                    SS2_Sheet1.RowCount = dt.Rows.Count;
                    SS2_Sheet1.SetRowHeight(-1, 30);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SS2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 5].Text = Convert.ToDouble(dt.Rows[i]["TOTAMT"].ToString().Trim()).ToString("###,###,###,##0");
                        SS2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["remark"].ToString().Trim();
                    }
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인


            strTitle = "월별 퇴원자 심사조정상세내역(개인별) LIST";
            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업월 : " + cboYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

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

        private void btnTab1Serch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int j = 0;
            int nBiNo = 0;
            double nAmt = 0;
            string strYYMM = "";
            string strFYYMM = "";
            string strTYYMM = "";
            string strFDate = "";
            string strTdate = "";
            string strBi = "";
            string strBiGbn = "";
            string SQL = "";    //Query문

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;

            //'누적할 배열을 Clear
            for (i = 1; i <= 15; i++)
            {
                for (j = 1; j <= 5; j++)
                {
                    FnAmt[i, j] = 0;
                }
            }


            strYYMM = VB.Left(cboFDate.Text, 4) + VB.Mid(cboFDate.Text, 7, 2);
            strFYYMM = VB.Left(cboFDate.Text, 4) + VB.Mid(cboFDate.Text, 7, 2);
            strFDate = VB.Left(strFYYMM, 4) + "-" + VB.Right(strFYYMM, 2) + "-01";
            strTYYMM = VB.Left(cboTDate.Text, 4) + VB.Mid(cboTDate.Text, 7, 2);
            strTdate = CF.READ_LASTDAY(clsDB.DbCon, VB.Left(strTYYMM, 4) + "-" + VB.Right(strTYYMM, 2) + "-01");

            //'jjy(2003-01-13) '통계 remark 등록 공용변수
            GstrYYMM = strFYYMM;
            //GstrMenu = "4";
            //GstrSMenu = "1";

            try
            {

                btnSearch.Enabled = false;

                CmdView_Slip_ADD(strYYMM, ref strBiGbn, strFDate, strTdate, ref nBiNo, ref nAmt);         //'퇴원자 조합부담금

                INWON_ADD(strYYMM, ref strBiGbn, strFDate, strTdate, ref nBiNo, ref nAmt);   //'퇴원심사건수
                for (i = 1; i <= 5; i++)
                {
                    //  '청구차액=미수발생액 - 발생미수액
                    FnAmt[i, 3] = FnAmt[i, 2] - FnAmt[i, 1];
                    //'EDI차액=발생미수액 - EDI접수액
                    FnAmt[i, 5] = FnAmt[i, 2] - FnAmt[i, 4];
                    //SS1.Row = i
                    for (j = 1; j <= 5; j++)
                    {
                        //  SS1.Col = j + 2
                        if (j == 4)
                        {
                            j = j;
                        }
                        SS3_Sheet1.Cells[i - 1, j + 1].Text = FnAmt[i, j].ToString("###,###,###,##0 ");
                    }
                }

                btnSearch.Enabled = true;
                //btnMenuRemark.Enabled = true;
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
        /// 입원 당월퇴원 조합부담 발생액을 MISU_BALDAILY에서 READ
        /// </summary>
        /// <param name="strYYMM"></param>
        /// <param name="strBiGbn"></param>
        /// <param name="strFDate"></param>
        /// <param name="strTdate"></param>
        /// <param name="nBiNo"></param>
        /// <param name="nAmt"></param>
        private void CmdView_Slip_ADD(string strYYMM, ref string strBiGbn, string strFDate, string strTdate, ref int nBiNo, ref double nAmt)
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            if (string.Compare(strYYMM, "200112") == 0)
            {
                return;
            }
            //'입원 당월퇴원 조합부담 발생액을 MISU_BALDAILY에서 READ
            //'BIGBN   종류(1.보험 2.보호 3.산재 4.자보 5.일반)


            strBiGbn = "''";
            if (chk0.Checked == true)
            {
                strBiGbn = strBiGbn + ",'1','5'";
            }
            if (chk1.Checked == true)
            {
                strBiGbn = strBiGbn + ",'2'";
            }
            if (chk3.Checked == true)
            {
                strBiGbn = strBiGbn + ",'3'";
            }
            if (chk2.Checked == true)
            {
                strBiGbn = strBiGbn + ",'4'";
            }


            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT SUBI,SUM(TOTAMT ) TOTAmt, SUM(SIMAMT) SIMAMT  ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "  AND  ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND ActDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";

            if (strBiGbn != "")
            {
                SQL = SQL + ComNum.VBLF + " AND SUBI IN (" + strBiGbn + " ) ";
            }


            SQL = SQL + ComNum.VBLF + "GROUP BY SUBI ";


            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {

                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;

            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nBiNo = (int)VB.Val(dt.Rows[i]["SUBI"].ToString().Trim());
                nAmt = VB.Val(dt.Rows[i]["TOTAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["SIMAMT"].ToString().Trim());
                if (nAmt != 0)
                {
                    switch (nBiNo)
                    {
                        case 1:
                            j = 1;  //보험
                            break;
                        case 2:
                            j = 2;  //보호
                            break;
                        case 3:
                            j = 3;  //산재
                            break;
                        case 4:
                            j = 4;  //자보
                            break;
                        default:
                            j = 1;  //기타는 보험으로
                            break;
                    }
                }
                FnAmt[j, 1] += nAmt;
                FnAmt[5, 1] += nAmt;

                nAmt = VB.Val(dt.Rows[i]["TOTAmt"].ToString().Trim());
                if (nAmt != 0)
                {
                    switch (nBiNo)
                    {
                        case 1:
                            j = 1;  //보험
                            break;
                        case 2:
                            j = 2;  //보호
                            break;
                        case 3:
                            j = 3;  //산재
                            break;
                        case 4:
                            j = 4;  //자보
                            break;
                        default:
                            j = 1;  //기타는 보험으로
                            break;
                    }
                    FnAmt[j, 2] += nAmt;
                    FnAmt[5, 2] += nAmt;
                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

        }

        private void INWON_ADD(string strYYMM, ref string strBiGbn, string strFDate, string strTdate, ref int nBiNo, ref double nAmt)
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            strBiGbn = "''";
            if (chk0.Checked == true)
            {
                strBiGbn = strBiGbn + ",'1','5'";
            }
            if (chk1.Checked == true)
            {
                strBiGbn = strBiGbn + ",'2'";
            }
            if (chk3.Checked == true)
            {
                strBiGbn = strBiGbn + ",'3'";
            }
            if (chk2.Checked == true)
            {
                strBiGbn = strBiGbn + ",'4'";
            }


            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT SUBI, COUNT(*) CNT ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "  AND ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND ActDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";

            if (strBiGbn != "")
            {
                SQL = SQL + ComNum.VBLF + " AND SUBI IN (" + strBiGbn + " ) ";
            }


            SQL = SQL + ComNum.VBLF + "GROUP BY SUBI ";


            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {

                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;

            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nBiNo = (int)VB.Val(dt.Rows[i]["SUBI"].ToString().Trim());

                if (nAmt != 0)
                {
                    switch (nBiNo)
                    {
                        case 1:
                            j = 1;  //보험
                            break;
                        case 2:
                            j = 2;  //보호
                            break;
                        case 3:
                            j = 3;  //산재
                            break;
                        case 4:
                            j = 4;  //자보
                            break;
                        default:
                            j = 1;  //기타는 보험으로
                            break;
                    }
                }
                FnAmt[j, 4] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                FnAmt[5, 4] += Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());

            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void btnTab1Print_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            if (cboFDate.Text == cboTDate.Text)
            {
                strTitle = "퇴원자 퇴원심사 조정액";
            }
            else
            {
                strTitle = cboFDate.Text + "~" + cboTDate.Text + "퇴원자 퇴원심사 조정액";
            }
            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업월 : " + cboYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS3, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void SS1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strPano = "";
            string strSname = "";
            string strBi = "";
            string strDeptCode = "";
            string strInDate = "";
            string strOutDate = "";
            string strActdate = "";
            string strYYMM = "";
            double nWRTNO = 0;
            string strIPDNO = "";
            string strTRSNO = "";

            if (SS1_Sheet1.RowCount == e.Row)
            {
                return;
            }
            if (e.Column != 0 && e.Column != 1 && e.Column != 3 && e.Column != 4 && e.Column != 5 && e.Column != 6)
            {
                return;
            }

            strPano = SS1_Sheet1.Cells[e.Row - 1, 0].Text;
            strSname = SS1_Sheet1.Cells[e.Row - 1, 1].Text;
            strBi = SS1_Sheet1.Cells[e.Row - 1, 2].Text;
            strIPDNO = SS1_Sheet1.Cells[e.Row - 1, 11].Text;
            strTRSNO = SS1_Sheet1.Cells[e.Row - 1, 12].Text;

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);

            if (e.Column <= 1)
            {
                GstrRetValue = strPano + strBi;
                frmPmpaViewIpdSimSarCheck4 frm = new frmPmpaViewIpdSimSarCheck4(GstrRetValue);
                frm.Show();
            }
            else
            {
                GstrRetValue = strIPDNO + ",";
                GstrRetValue = GstrRetValue + strTRSNO + ",";
                GstrRetValue = GstrRetValue + strPano + ",";
                GstrRetValue = GstrRetValue + strSname + ",";
                GstrRetValue = GstrRetValue + strBi + ",";
                GstrRetValue = GstrRetValue + strYYMM + ",";

                frmPmPaViewmisubs55 frm = new frmPmPaViewmisubs55(GstrRetValue);
                frm.Show();
            }
        }

        private void SS1_CellClick(object sender, CellClickEventArgs e)
        {

        }

        private void SS1_EditModeOff(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            string strROWID = "";
            string strRemark = "";
            string strOldRemark = "";
            #region MyRegion
            int eCol = SS1_Sheet1.ActiveColumnIndex;
            int eRow = SS1_Sheet1.ActiveRowIndex;
            #endregion

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                if (eCol == 7)
                {
                    strRemark = SS1_Sheet1.Cells[eRow, 7].Text;
                    strROWID = SS1_Sheet1.Cells[eRow, 8].Text;
                    strOldRemark = SS1_Sheet1.Cells[eRow, 10].Text;

                    if (strRemark != strOldRemark)
                    {
                        SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALTEWON SET ";
                        SQL = SQL + ComNum.VBLF + " SIMSAMEMO='" + strRemark + "',";
                        SQL = SQL + ComNum.VBLF + " EntDate=SYSDATE,EntSabun=" + clsType.User.Sabun + " ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID='" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void chk0_CheckedChanged(object sender, EventArgs e)
        {
            Tab1SS_SET();
        }

        private void chk1_CheckedChanged(object sender, EventArgs e)
        {
            Tab1SS_SET();
        }

        private void chk2_CheckedChanged(object sender, EventArgs e)
        {
            Tab1SS_SET();
        }

        private void chk3_CheckedChanged(object sender, EventArgs e)
        {
            Tab1SS_SET();
        }
    }
}
