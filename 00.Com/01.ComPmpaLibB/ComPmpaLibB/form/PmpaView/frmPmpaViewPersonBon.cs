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
    /// Create Date     : 2017-10-11
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\IPD\iument\iument.vbp\Frm퇴원진료비영수증_NEW.frm" >> frmPmpaViewPersonBon.cs 폼이름 재정의" />

    public partial class frmPmpaViewPersonBon : Form
    {
        string GstrSysDate = "";


        string nAmt = "";
        string strAmt = "";
        string strPANO = "";
        string StrName = "";
        string strRoom = "";
        string StrIDate = "";
        string strGwa = "";
        string StrGwaName = "";
        string StrGam = "";
        string strBi = "";
        string StrAge = "";
        string strSex = "";

        string nSel = "";
        int nRow = 0;
        int nBi = 0;
        int nGel = 0;
        int nSELECT = 0;
        int nSELECT1 = 0;
        int nSELECT2 = 0;
        int nIlsu = 0;
        int nILSUcnt = 0;
        int nCNT = 0;
        int nCount = 0;
        int nCount1 = 0;
        int ChkRow = 0;
        int ChkRowCnt = 0;

        double[] nSubAmt = new double[8];
        double[] nTotAmt = new double[8];

        double[] FnAmt = new double[61];
        double nTot1 = 0;
        double nTot2 = 0;

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();
        clsIument clsENT = new clsIument();
        clsPmpaType TIT = new clsPmpaType();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaViewPersonBon(string strstrSysDate)
        {
            GstrSysDate = strstrSysDate;

            InitializeComponent();
        }

        public frmPmpaViewPersonBon()
        {
            InitializeComponent();
        }

        #region Load
        private void frmPmpaViewPersonBon_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            DataTable dtFc = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            GstrSysDate = strDTP;

            //ssView_Sheet1.Columns[0].Visible = false;
            //ssView_Sheet1.Columns[10].Visible = false;
            //ssView_Sheet1.Columns[11].Visible = false;
            //ssView_Sheet1.Columns[12].Visible = false;
            //ssView_Sheet1.Columns[16].Visible = false;
           // ssView_Sheet1.Columns[17].Visible = false;

            dtpFdate.Value = Convert.ToDateTime(strDTP);
            txtFloat.Text = "";

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;

            //try
            //{
            SQL = "";
            SQL = "SELECT ";
            SQL += ComNum.VBLF + " Sort,Code,Name ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + " WHERE 1=1";
            SQL += ComNum.VBLF + "   AND Gubun='BAS_환자종류' ";
            SQL += ComNum.VBLF + "   AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE)) ";
            SQL += ComNum.VBLF + " ORDER BY Sort,Code ";

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
            ssView_Sheet1.RowCount = dt.Rows.Count;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            cbobi.Items.Add("00.전체");

            for (i = 0; i < dt.Rows.Count; i++)
            {
                cbobi.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
            }
            dt.Dispose();
            dt = null;
            cbobi.SelectedIndex = 0;

            cboWard.Items.Clear();
            cboWard.Items.Add("**.전체");

            SQL = "";
            SQL = "SELECT * ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD";

            SqlErr = clsDB.GetDataTable(ref dtFc, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dtFc.Rows.Count == 0)
            {

                dtFc.Dispose();
                dtFc = null;
                btnSearch.Enabled = true;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;

            }

            for (i = 0; i < dtFc.Rows.Count; i++)
            {
                cboWard.Items.Add(dtFc.Rows[i]["WardCode"].ToString().Trim());
            }

            cboWard.SelectedIndex = 0;

            dtFc.Dispose();
            dtFc = null;

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.Default;
            btnSearch.Enabled = true;

            //}
            //catch (Exception ex)
            //{
            //    btnSearch.Enabled = true;
            //    ComFunc.MsgBox(ex.Message);
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //}

        }
        #endregion

        #region Search

        /// <summary>
        /// btnSearch_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.Compare(dtpFdate.Value.ToString("yyyy-MM-dd"), GstrSysDate) > 0)
            {
                ComFunc.MsgBox("작업일자가 오늘보다 큽니다.", "주의");
                dtpFdate.Value = Convert.ToDateTime(GstrSysDate);
                cbobi.Select();
                return;
            }

            if (cbobi.Text == "" || cbobi.Text == null)
            {
                ComFunc.MsgBox("환자구분이 선택되지 안았습니다.", "주의");
                cbobi.Select();
                return;
            }

            nAmt = VB.Val(txtFloat.Text).ToString("###########");
            nILSUcnt = (int)VB.Val(txtilsu.Text.Trim());

            SS1BuildMain();

            btnPrint.Enabled = true;
        }

        /// <summary>
        /// SS1BuildMain
        /// </summary>
        private void SS1BuildMain()
        {
            int i = 0;
            int j = 0;
            int x = 0;
            int y = 0;
            int ChkFlag = 0;
            int nCntFlag = 0;
            int nOpdBonRate = 0;
            int nRead = 0;
            int nTotCnt = 0;
            int nRead2 = 0;
            double nCTMRAmt = 0;
            double nCTMRBonin = 0;
            double nBonGubyo = 0;
            double nTotBiGubyo = 0;
            double nBonBiGubyo = 0;
            double nBoninAmt = 0;
            double nTRSNo = 0;
            double nIPDNO = 0;
            double nLastTrs = 0;
            double nTotBonAmt = 0;
            double nTotJungAmt = 0;
            double nTotSunapAmt = 0;
            double nTotalAmt = 0;
            string strBDate = "";
            string strBi = "";
            string strGubun = "";
            string strDrg = string.Empty;
            clsPmpaType.TIT.RAmt = new long[34, 3];


            DataTable dt = null;
            DataTable dtFc = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            for (i = 0; i <= 7; i++)
            {
                nTotAmt[i] = 0;
            }
            

            nSel = (VB.Left(cbobi.Text, 2).Trim());

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;

            //try
            //{
            SQL = "";
            SQL = " SELECT ";
            SQL += ComNum.VBLF + " IPDNO, RoomCode, Pano, TO_CHAR(InDate,'YY-MM-DD') IDate,   ";
            SQL += ComNum.VBLF + "        ilsu, SName, age, Sex, Bi,                          ";
            SQL += ComNum.VBLF + "        DeptCode, AMSET1,ReMark, GbGamek, Bohun, Gelcode, LastTrs  ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.VBLF + "IPD_NEW_MASTER                                             ";
            SQL += ComNum.VBLF + "  WHERE 1=1";
            SQL += ComNum.VBLF + "     AND OUTDATE IS NULL                                            "; //'정상애기

            if (nSel != "00")
            {
                SQL += ComNum.VBLF + "  AND Bi = '" + nSel + "'                     ";
            }
            if ((txtilsu.Text) != "")
            {
                SQL += ComNum.VBLF + "    AND ILSU >=  " + VB.Val(txtilsu.Text) + " ";
            }

            SQL += ComNum.VBLF + "    AND GBSTS IN ('0')                                             ";

            if (VB.Left(cboWard.Text, 2) != "**")
            {
                SQL += ComNum.VBLF + " AND WARDCODE = '" + cboWard.Text + "'                       ";
            }
            SQL += ComNum.VBLF + "  ORDER BY RoomCode, Pano, Bi                                      ";

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
            nTotCnt = dt.Rows.Count;
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            progressBar1.Value = 0;
            progressBar1.Maximum = dt.Rows.Count;

            for (j = 0; j < dt.Rows.Count; j++)
            {
                progressBar1.Value = (j + 1);
                for (i = 0; i <= 60; i++)
                {
                    FnAmt[i] = 0;
                }
                nRead2 = 0;
                nIPDNO = VB.Val(dt.Rows[j]["IPDNO"].ToString().Trim());

                SQL = "";
                SQL = " SELECT ";
                SQL += ComNum.VBLF + " TRSNO,Bi,GBDRG From IPD_TRANS Where IPDNO =" + nIPDNO + " ";
                SQL += ComNum.VBLF + " AND GBIPD NOT IN ('9','D') ";
                SQL += ComNum.VBLF + " AND OUTDATE IS NULL ";
                SQL += ComNum.VBLF + " AND GBSTS = '0' ";

                SqlErr = clsDB.GetDataTable(ref dtFc, SQL, clsDB.DbCon);
                nRead2 = dtFc.Rows.Count;
                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (nRead2 > 0)
                {


                    for (i = 0; i < dtFc.Rows.Count; i++)
                    {
                        strBi = VB.Val(dt.Rows[j]["IPDNO"].ToString().Trim()).ToString();
                        nTRSNo = VB.Val(dtFc.Rows[i]["TRSNO"].ToString().Trim());
                        strDrg = dtFc.Rows[i]["GBDRG"].ToString().Trim();

                        for (x = 1; x <= 27; x ++)
                        {
                            clsPmpaType.TIT.RAmt[x, 1] = 0;
                            clsPmpaType.TIT.RAmt[x, 2] = 0;
                  
                        }

                        clsPmpaType.TIT.RAmt[33, 2] = 0;

                        clsIument cit = new clsIument();
                        cit.DISPLAY_IPD_TRANS_AMT_NEW(clsDB.DbCon, SSAmt, SSAmtNew, null, null, 0, (long)nTRSNo, "", dt.Rows[j]["PANO"].ToString().Trim(), strDrg);

                        FnAmt[50] = FnAmt[50] + clsPmpaType.TIT.RAmt[26, 1]; //'총진료비
                        FnAmt[53] = FnAmt[53] + clsPmpaType.TIT.RAmt[25, 1]; //'조합부담금
                        FnAmt[55] = FnAmt[55] + clsPmpaType.TIT.RAmt[27, 1]; //'본인부담금
                    }
                }
                dtFc.Dispose();
                dtFc = null;

                IPD_TRANS_Amt_Display(ref nIPDNO, 0);

                SELECT_본인부담금(ref nCTMRAmt, ref nCTMRBonin, nTRSNo, ref strBDate, ref nOpdBonRate, dt, j, ref nBonGubyo, ref strGubun);

                //if ((FnAmt[55] - FnAmt[52]) >= Convert.ToInt64(VB.Val(txtFloat.Text).ToString("##########")))
                if ((FnAmt[55] - FnAmt[52]) >= VB.Val(txtFloat.Text))
                {
                    ssView_Sheet1.RowCount += 1;

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = ssView_Sheet1.RowCount.ToString();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[j]["ROOMCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[j]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[j]["IDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[j]["ILSU"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[j]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[j]["AGE"].ToString().Trim() + "/" + dt.Rows[j]["SEX"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = dt.Rows[j]["BI"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = dt.Rows[j]["DEPTCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = FnAmt[50].ToString("###,###,###,###");
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = FnAmt[53].ToString("###,###,###,###");
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nBonGubyo.ToString("###,###,###,###");
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = nTot2.ToString("###,###,###,###");
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = FnAmt[55].ToString("###,###,###,###");
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14].Text = FnAmt[52].ToString("###,###,###,###");
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 15].Text = (FnAmt[55] - FnAmt[52]).ToString("###,###,###,###");
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 16].Text = strGubun;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 17].Text = " " + dt.Rows[j]["REMARK"].ToString().Trim();

                    nTotalAmt = nTotalAmt + FnAmt[50];
                    nTotBonAmt = nTotBonAmt + FnAmt[55];
                    nTotJungAmt = nTotJungAmt + FnAmt[52];
                    nTotSunapAmt = nTotSunapAmt + (FnAmt[55] - FnAmt[52]);
                }
             
                Application.DoEvents();
            }
            dt.Dispose();
            dt = null;

            ssView_Sheet1.RowCount += 1;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = "합 계";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nTotalAmt.ToString("###,###,###,###");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = nTotBonAmt.ToString("###,###,###,###");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14].Text = nTotJungAmt.ToString("###,###,###,###");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 15].Text = nTotSunapAmt.ToString("###,###,###,###");

            btnSearch.Enabled = true;
            Cursor.Current = Cursors.Default;

            //}
            //catch (Exception ex)
            //{
            //    btnSearch.Enabled = true;
            //    ComFunc.MsgBox(ex.Message);
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //}
        }

        /// <summary>
        /// IPD_TRANS_Amt_Display
        /// </summary>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTRSNO"></param>
        /// <param name="nIPDNO"></param>
        private void IPD_TRANS_Amt_Display(ref double ArgIpdNo, double ArgTRSNO)
        {
            int i = 0;
            int j = 0;
            string strGbSTS = "";
            double nIPDNO = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //'누적별 금액을 합산함
                SQL = "";
                SQL = "SELECT ";
                SQL += ComNum.VBLF + " Bi,TO_CHAR(InDate,'YYYY-MM-DD') InDate,GBSTS, IPDNO, ";
                SQL += ComNum.VBLF + " Amt01,Amt02,Amt03,Amt04,Amt05,Amt06,Amt07,Amt08,Amt09,Amt10,";
                SQL += ComNum.VBLF + " Amt11,Amt12,Amt13,Amt14,Amt15,Amt16,Amt17,Amt18,Amt19,Amt20,";
                SQL += ComNum.VBLF + " Amt21,Amt22,Amt23,Amt24,Amt25,Amt26,Amt27,Amt28,Amt29,Amt30,";
                SQL += ComNum.VBLF + " Amt31,Amt32,Amt33,Amt34,Amt35,Amt36,Amt37,Amt38,Amt39,Amt40,";
                SQL += ComNum.VBLF + " Amt41,Amt42,Amt43,Amt44,Amt45,Amt46,Amt47,Amt48,Amt49,Amt50,";
                SQL += ComNum.VBLF + " Amt51,Amt52,Amt53,Amt54,Amt55,Amt56,Amt57,Amt58,Amt59,Amt60 ";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_TRANS ";

                if (ArgTRSNO == 0)
                {
                    SQL += ComNum.VBLF + "WHERE IPDNO = " + ArgIpdNo + " ";
                }
                else
                {
                    SQL += ComNum.VBLF + "WHERE TrsNo = " + ArgTRSNO + " ";
                }

                SQL += ComNum.VBLF + " AND OUTDATE IS NULL ";
                SQL += ComNum.VBLF + "ORDER BY InDate,Bi ";

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
                    for (j = 1; j <= 60; j++)
                    {
                        if (j != 50 && j != 53 && j != 55)
                        {
                            FnAmt[j] = FnAmt[j] + VB.Val(dt.Rows[i]["Amt" + j.ToString("00")].ToString().Trim());
                        }
                    }
                }

                strGbSTS = dt.Rows[0]["GBSTS"].ToString().Trim();
                nIPDNO = VB.Val(dt.Rows[0]["IPDNO"].ToString().Trim());

                dt.Dispose();
                dt = null;

                //'보증금,중간납 입금액
                if (string.Compare(strGbSTS, "7") < 0)
                {
                    FnAmt[51] = 0;
                    FnAmt[52] = 0;

                    SQL = "";
                    SQL = "SELECT ";
                    SQL += ComNum.VBLF + "  SUM(CASE WHEN SUNEXT IN ('Y85','Y87') THEN AMT else  AMT *-1 END) Amt ";
                    SQL += ComNum.VBLF + " FROM IPD_NEW_CASH ";
                    SQL += ComNum.VBLF + " WHERE 1=1";
                    SQL += ComNum.VBLF + "   AND IPDNO=" + nIPDNO + " ";
                    SQL += ComNum.VBLF + "   AND SuNext IN ('Y85','Y87','Y88') ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        FnAmt[52] = FnAmt[52] + VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;

                    //'-----------( 급여, 비급여 합계금액을 계산 )---------------
                    nTot1 = 0;
                    nTot2 = 0;

                    for (i = 1; i <= 49; i++)
                    {
                        switch (i)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                            case 12:
                            case 13:
                            case 14:
                            case 15:
                            case 16:
                            case 17:
                            case 18:
                            case 19:
                            case 20:
                                nTot1 = nTot1 + FnAmt[i];
                                break;
                            default:
                                nTot2 = nTot2 + FnAmt[i];
                                break;
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// SELECT_본인부담금
        /// </summary>
        /// <param name="nCTMRAmt"></param>
        /// <param name="nCTMRBonin"></param>
        /// <param name="nTRSNo"></param>
        /// <param name="strBDate"></param>
        /// <param name="nOpdBonRate"></param>
        /// <param name="dtFunc">부르는 포문 dt </param>
        /// <param name="j">부르는 포문 j</param>
        /// <param name="nBonGubyo"></param>
        /// <param name="strGubun"></param>
        private void SELECT_본인부담금(ref double nCTMRAmt, ref double nCTMRBonin, double nTRSNo, ref string strBDate, ref int nOpdBonRate, DataTable dtFunc, int j, ref double nBonGubyo, ref string strGubun)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            nCTMRAmt = 0;
            nCTMRBonin = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
                {
                    SQL = "";
                    SQL = "SELECT ";
                    SQL += ComNum.VBLF + "  TO_CHAR(BDATE,'YYYY-MM-DD') BDate,Bun,SUM(Amt1+Amt2) CTMRIAmt ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE TRSNO=" + nTRSNo + " ";
                    SQL += ComNum.VBLF + "    AND BUN IN ('72','73') ";
                    SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                    SQL += ComNum.VBLF + "  GROUP BY BDATE, BUN ";

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
                       // ComFunc.MsgBox("해당 DATA가 없습니다.");
                       // Cursor.Current = Cursors.Default;
                        return;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strBDate = dt.Rows[i]["BDate"].ToString().Trim();
                        nAmt = Convert.ToString(VB.Val(dt.Rows[i]["CTMRIAmt"].ToString().Trim()));
                        nOpdBonRate = CPF.READ_OpdBonin_Rate_CHK(clsDB.DbCon, dtFunc.Rows[j]["BI"].ToString().Trim(), strBDate);
                        nCTMRAmt = nCTMRAmt + Convert.ToInt64(nAmt);
                        nCTMRBonin = nCTMRBonin + (Convert.ToInt64(nAmt) * nOpdBonRate / 100);
                    }
                    dt.Dispose();
                    dt = null;

                }

                //'---------------------------------------------
                //'   본인부담액을 계산함(long)Math.Round((double)(nFoodAmt * clsPmpaType.TIT.BonRate / 100));
                //'---------------------------------------------

                //'급여 본인부담액을 계산
                Cursor.Current = Cursors.Default;
               
                //nBonGubyo = (double)((nTot1 - nCTMRAmt) * VB.Val(dtFunc.Rows[j]["BONRATE"].ToString().Trim()) / 100) + nCTMRBonin;
                nBonGubyo = (double)((nTot1 - nCTMRAmt) * clsPmpaType.TIT.BonRate / 100) + nCTMRBonin;
                if (clsPmpaType.TIT.Bohun == "3" && clsPmpaType.TIT.Bi == "22")
                {
                    nBonGubyo = 0;         //'장애인기금 (96/05/27:LYJ)
                }
                if (clsPmpaType.TIT.OgPdBun == "O" || clsPmpaType.TIT.OgPdBun == "P")
                {
                    nBonGubyo = 0;    //'신생아, 분만일 경우 본인부담금 없음
                }

                strGubun = "";

                if (string.Compare(clsPmpaType.TIT.Bi, "11") >= 0 && string.Compare(clsPmpaType.TIT.Bi, "24") <= 0)
                {
                    if (nBonGubyo > 5040000)
                    {
                        nBonGubyo = nBonGubyo - 5040000;
                        strGubun = "대불금";
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        private void btnexcel_Click(object sender, EventArgs e)
        {
            bool x = false;

            if (ComFunc.MsgBoxQ("파일로 만드시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                return;

            x = ssView.SaveExcel("C:\\재원미수자료.xlsx", FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat);
            {
                if (x == true)
                    ComFunc.MsgBox("엑셀파일이 생성이 되었습니다.", "확인");
                else
                    ComFunc.MsgBox("엑셀파일 생성에 오류가 발생 하였습니다.", "확인");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼
            int i = 0;
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";
            string strFont3 = "";
            string strFoot1 = "";

            strFont1 = "/fn\"맑은 고딕\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"11\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strFont3 = "/fn\"맑은 고딕\" /fz\"9\" /fb0 /fi0 /fu0 /fk0 /fs3";

            strHead1 = "/c/f1" + "재원자 본인부담 진료비 명부" + "/f1/n";

            strHead2 = "/l/f2" + "작업년월 : " + dtpFdate.Value.ToString("yyyy-MM-dd") + "/f2/n";
            strHead2 += "/l/f2" + VB.Space(11) + "출력시간 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + "/f2/n";

           // strFoot1 = "/r/f3" + "작성자 : " + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun) + "/f3/n";

            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Footer = strFont3 + strFoot1;
            ssView_Sheet1.PrintInfo.Margin.Top = 100;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.Margin.Header = 10;
            ssView_Sheet1.PrintInfo.Margin.Footer = 10;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.UseSmartPrint = false;
            ssView_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssView_Sheet1.PrintInfo.Preview = true;
            ssView.PrintSheet(0);


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
