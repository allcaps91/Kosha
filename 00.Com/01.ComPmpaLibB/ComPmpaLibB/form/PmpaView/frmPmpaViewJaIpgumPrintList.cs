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
    /// File Name       : frmPmpaViewJaIpgumPrintList
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-10-16
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\misu\misuta.vbp\MISUT204.FRM(FrmIpgumReport.frm)" >> frmPmpaViewJaIpgumPrintList.cs 폼이름 재정의" />

    public partial class frmPmpaViewJaIpgumPrintList : Form
    {

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        clsPmpaFunc CPF = new clsPmpaFunc();
        ComFunc CF = new ComFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();

        string strdtP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string strYYMM = "";
        string strFDate = "";
        string strTDate = "";
        string strOldData = "";
        string strNewData = "";
        string strCoIPDOPD = "";
        string strCoGel = "";
        string strCoWrtno = "";
        string strFirst = "";
        int nCnt = 0;
        int nSCnt = 0;
        int nTCnt = 0;
        double nSTAmt = 0;
        double nSMAmt = 0;
        double nSIAmt = 0;
        double nSSAmt = 0;
        double nSJAmt = 0;
        double nSMSak = 0;
        double nTTAmt = 0;
        double nTMAmt = 0;
        double nTIAmt = 0;
        double nTSAmt = 0;
        double nTJAmt = 0;
        double nTMSak = 0;
        int nOTcnt = 0;
        double nOTTAmt = 0;
        double nOTMAmt = 0;
        double nOTIAmt = 0;
        double nOTSAmt = 0;
        double nOTJAmt = 0;
        double nOTMSak = 0;
        int nITcnt = 0;
        double nITTAmt = 0;
        double nITMAmt = 0;
        double nITIAmt = 0;
        double nITSAmt = 0;
        double nITJAmt = 0;
        double nITMSak = 0;
        double nILsu0 = 0;             //'외래 회수 건수
        double nILsuI = 0;             //'입원 회수 건수

        string strGelCode = "";

        public frmPmpaViewJaIpgumPrintList()
        {
            InitializeComponent();
        }

        private void frmPmpaViewJaIpgumPrintList_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int nMM = 0;
            int i = 0;

            nYY = (int)VB.Val(VB.Left(strdtP, 4));
            nMM = (int)VB.Val(VB.Mid(strdtP, 6, 2));

            for (i = 1; i <= 15; i++)
            {
                cboYYMM.Items.Add((nYY).ToString("0000") + "년 " + (nMM).ToString("00") + "월분");

                nMM = nMM - 1;
                if (nMM == 0)
                {
                    nMM = 12;
                    nYY = nYY - 1;
                }
            }

            cboYYMM.SelectedIndex = 0;
            panHidden1.Visible = false;
            dtpFDate.Value = Convert.ToDateTime(strdtP);
            dtpTDate.Value = Convert.ToDateTime(strdtP);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

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
            nITMSak = 0;
            nOTMSak = 0;
            nTMSak = 0;
            nSMSak = 0;

            if (rdoGB1.Checked == true)
            {
                strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
                strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
                strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            }
            else
            {
                strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
                strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");
                strYYMM = VB.Left(dtpFDate.Value.ToString("yyyy-MM-dd"), 4) + VB.Mid(dtpTDate.Value.ToString("yyyy-MM-dd"), 6, 2);

                if (string.Compare(strFDate, strTDate) > 0)
                {
                    dtpFDate.Select();
                }
            }
            // ' 해당월 마감여부 Checking

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Count(*) Cnt  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_MONTHLY ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1      ";
                SQL = SQL + ComNum.VBLF + "    AND YYMM = '" + strYYMM + "'       ";
                SQL = SQL + ComNum.VBLF + "    AND Class = '07'                   ";


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
                    ComFunc.MsgBox("해당월의 통계형성이 안되었습니다", "확인");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) == 0)
                {
                    ComFunc.MsgBox("해당월의 통계형성이 안되었습니다", "확인");
                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    return;
                }
                dt.Dispose();
                dt = null;
                btnSearch.Enabled = true;



                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT WRTNO,GelCode,IpdOpd,MisuID, TO_CHAR(Bdate,'YYYY-MM-DD') IDate,    ";
                SQL = SQL + ComNum.VBLF + "        SUM(Amt) IAmt                                                      ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP                                                          ";
                SQL = SQL + ComNum.VBLF + "  WHERE Bdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                  ";
                SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                  ";
                SQL = SQL + ComNum.VBLF + "    AND Class = '07'                                                       ";
                SQL = SQL + ComNum.VBLF + "    AND Gubun >= '21'                                                      ";
                SQL = SQL + ComNum.VBLF + "    AND Gubun <= '29'                                                      ";

                if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND IPDOPD = 'I' ";
                }
                else if (rdoIO2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND IPDOPD = 'O' ";
                }
                SQL = SQL + ComNum.VBLF + "  GROUP BY WRTNO,GelCode,IpdOpd,MisuID ,Bdate                              ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY 2,3,4,5                                                         ";

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
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                strFirst = "OK";

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ADD_SPREAD(dt, i, strYYMM);
                }
                dt.Dispose();
                dt = null;

                Misu_Sub_IPDOPD_Rtn();
                Misu_Sub_Rtn();
                MISU_Total_Rtn();

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

        private void ADD_SPREAD(DataTable dt, int i, string strYYMM)
        {
            string strIpdOpd = "";
            string strWrtno = "";
            string strPano = "";
            string strSname = "";
            string strGelName = "";
            string strDeptCode = "";
            string strDate1 = "";
            string strDate2 = "";
            string strDate3 = "";
            string strDate4 = "";
            double nTAmt = 0;   //'입금완료분 청구액
            double nMAmt = 0;   //'청구액
            double nSAmt = 0;   //'삭감액
            double nIAmt = 0;   //'회입액
            double nJamt = 0;   //'현잔액
            DataTable dtFucn1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strWrtno = dt.Rows[i]["Wrtno"].ToString().Trim();
            //' 월말 현재 잔액을 READ

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT JanAmt ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_Monthly   ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1=1 ";
            SQL = SQL + ComNum.VBLF + "    AND WRTNO = '" + strWrtno + "' ";
            SQL = SQL + ComNum.VBLF + "    AND YYMM  = '" + strYYMM + "'  ";

            SqlErr = clsDB.GetDataTable(ref dtFucn1, SQL, clsDB.DbCon);
            nJamt = 0;

            if (SqlErr != "")
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dtFucn1.Rows.Count == 1)
            {
                nJamt = VB.Val(dtFucn1.Rows[0]["JanAmt"].ToString().Trim());
            }

            dtFucn1.Dispose();
            dtFucn1 = null;

            CPM.READ_MISU_IDMST(Convert.ToInt32(strWrtno));

            nCnt = nCnt + 1;
            nSCnt = nSCnt + 1;
            nTCnt = nTCnt + 1;

            strIpdOpd = clsPmpaType.TMM.IpdOpd;
            strPano = clsPmpaType.TMM.MisuID;

            strSname = CPF.Read_Bas_Patient(clsDB.DbCon, clsPmpaType.TMM.MisuID.Trim());
            strGelCode = clsPmpaType.TMM.GelCode;
            strDeptCode = clsPmpaType.TMM.DeptCode;
            strDate1 = dt.Rows[i]["IDate"].ToString().Trim(); //'입금일
            strDate2 = clsPmpaType.TMM.BDate;    //   '청구일
            strDate3 = clsPmpaType.TMM.FromDate;         //   '진료기간(From)
            strDate4 = clsPmpaType.TMM.ToDate;          //   '진료기간(To)
            nMAmt = clsPmpaType.TMM.Amt[2];                   //'청구액
            nIAmt = VB.Val(dt.Rows[i]["IAmt"].ToString().Trim());     //'입금액
            nSAmt = 0;

            if (nJamt == 0)
            {
                nJamt = nJamt;
            }

            if (nJamt == 0)
            {
                nTAmt = clsPmpaType.TMM.Amt[2];  //'청구액(입금완료분)

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SUM(AMT) SAKAMT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_SLIP                      ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1    ";
                SQL = SQL + ComNum.VBLF + "    AND Bdate = TO_DATE('" + strDate1 + "','YYYY-MM-DD')    ";
                SQL = SQL + ComNum.VBLF + "    AND Class = '07'                                        ";
                SQL = SQL + ComNum.VBLF + "    AND GUBUN = '31'                                        ";
                SQL = SQL + ComNum.VBLF + "    AND WRTNO = '" + strWrtno + "'                          ";

                SqlErr = clsDB.GetDataTable(ref dtFucn1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dtFucn1.Rows.Count > 0)
                {
                    nSAmt = VB.Val(dtFucn1.Rows[0]["SAKAMT"].ToString().Trim()); //'TMM.Amt(4)    '삭감액
                }

                dtFucn1.Dispose();
                dtFucn1 = null;
            }
            strGelName = "";

            if (strFirst == "OK")
            {
                strCoGel = strGelCode;
                strCoIPDOPD = strIpdOpd;
                strFirst = "NO";
                strGelName = CPM.READ_BAS_MIA(strGelCode);
                nCnt = 0;
                nSCnt = 0;
                nTCnt = 0;
            }

            if (strGelCode != strCoGel)
            {
                Misu_Sub_IPDOPD_Rtn();
                Misu_Sub_Rtn();
                strCoIPDOPD = strIpdOpd;
                strGelName = CPM.READ_BAS_MIA(strGelCode);
            }

            if (strIpdOpd != strCoIPDOPD)
            {
                Misu_Sub_IPDOPD_Rtn();
                strCoIPDOPD = strIpdOpd;
            }

            if (clsPmpaType.TMM.IpdOpd == "O")
            {
                strIpdOpd = "외래";
                nILsu0 = nILsu0 + VB.DateDiff("d", strDate2, strDate1);
            }
            else if (clsPmpaType.TMM.IpdOpd == "I")
            {
                nILsuI = nILsuI + VB.DateDiff("d", strDate2, strDate1);
                strIpdOpd = "입원";
            }

            strCoWrtno = strWrtno;

            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1 - 1].Text = strGelName;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2 - 1].Text = strDate1;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3 - 1].Text = strDate2;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4 - 1].Text = VB.DateDiff("d", strDate2, strDate1).ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5 - 1].Text = strIpdOpd;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6 - 1].Text = strPano;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7 - 1].Text = strSname;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8 - 1].Text = strDate3 + "-" + strDate4;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9 - 1].Text = strDeptCode;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10 - 1].Value = nMAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11 - 1].Value = nIAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12 - 1].Value = nSAmt.ToString();

            if (nMAmt != 0 && nSAmt != 0)
            {
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13 - 1].Text = (nSAmt / nMAmt * 100).ToString("#0.00") + "%";
            }
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14 - 1].Value = nJamt.ToString();
            nSTAmt = nSTAmt + nTAmt;
            nSMAmt = nSMAmt + nMAmt;
            nSIAmt = nSIAmt + nIAmt;
            nSSAmt = nSSAmt + nSAmt;
            nSJAmt = nSJAmt + nJamt;

            if (nJamt < 1)
            {
                nSMSak = nSMSak + nMAmt;
            }

            nTTAmt = nTTAmt + nTAmt;
            nTMAmt = nTMAmt + nMAmt;
            nTIAmt = nTIAmt + nIAmt;
            nTSAmt = nTSAmt + nSAmt;
            nTJAmt = nTJAmt + nJamt;

            if (nJamt < 1)
            {
                nTMSak = nTMSak + nMAmt;
            }

            if (strIpdOpd == "입원")
            {
                nITcnt = nITcnt + 1;
                nITTAmt = nITTAmt + nTAmt;
                nITMAmt = nITMAmt + nMAmt;
                nITIAmt = nITIAmt + nIAmt;
                nITSAmt = nITSAmt + nSAmt;
                nITJAmt = nITJAmt + nJamt;
                if (nJamt < 1)
                {
                    nITMSak = nITMSak + nMAmt;
                }
            }
            else
            {
                nOTcnt = nOTcnt + 1;
                nOTTAmt = nOTTAmt + nTAmt;
                nOTMAmt = nOTMAmt + nMAmt;
                nOTIAmt = nOTIAmt + nIAmt;
                nOTSAmt = nOTSAmt + nSAmt;
                nOTJAmt = nOTJAmt + nJamt;
                if (nJamt < 1)
                {
                    nOTMSak = nOTMSak + nMAmt;
                }
            }

        }

        private void Misu_Sub_IPDOPD_Rtn()
        {
            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7 - 1].Text = strCoIPDOPD + " 소계 ";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8 - 1].Text = nCnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10 - 1].Value = nSMAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11 - 1].Value = nSIAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12 - 1].Value = nSSAmt.ToString();

            if (nSMSak != 0 && nSSAmt != 0)
            {
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13 - 1].Text = (nSSAmt / nSMSak * 100).ToString("#0.00") + "%";
            }

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14 - 1].Value = nSJAmt.ToString();

            nCnt = 0;

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 128);

            nSTAmt = 0;
            nSMAmt = 0;
            nSIAmt = 0;
            nSSAmt = 0;
            nSJAmt = 0;
            nSMSak = 0;
            strCoGel = strGelCode;
        }

        private void Misu_Sub_Rtn()
        {
            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7 - 1].Text = " 소계 ";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8 - 1].Text = nSCnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10 - 1].Value = nTMAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11 - 1].Value = nTIAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12 - 1].Value = nTSAmt.ToString();

            if (nTMSak != 0 && nTSAmt != 0)
            {
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13 - 1].Text = ((nTSAmt / nTMSak) * 100).ToString("##0.00") + "%";
            }

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14 - 1].Value = nTJAmt.ToString();

            nSCnt = 0;

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(128, 255, 128);
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Border = new FarPoint.Win.LineBorder(Color.Black, 1, false, false, false, true);


            nTTAmt = 0;
            nTMAmt = 0;
            nTSAmt = 0;
            nTIAmt = 0;
            nTJAmt = 0;
            nTMSak = 0;
            strCoGel = strGelCode;
        }

        private void MISU_Total_Rtn()
        {
            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(120, 240, 180);
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1 - 1].Text = "    외        래";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4 - 1].Text = nILsu0 + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8 - 1].Text = nOTcnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10 - 1].Value = nOTMAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11 - 1].Value = nOTIAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12 - 1].Value = nOTSAmt.ToString();

            if (nOTMSak != 0 && nOTSAmt != 0)
            {
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13 - 1].Text = (nOTSAmt / nOTMSak * 100).ToString("#0.00") + "%";
            }
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14 - 1].Value = nOTJAmt.ToString();

            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(120, 240, 180);
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1 - 1].Text = "    입        원";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4 - 1].Text = nILsuI + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8 - 1].Text = nITcnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10 - 1].Value = nITMAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11 - 1].Value = nITIAmt.ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12 - 1].Value = nITSAmt.ToString();

            if (nITSAmt != 0 && nITMSak != 0)
            {
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13 - 1].Text = (nITSAmt / nITMSak * 100).ToString("#0.00") + "%";
            }

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14 - 1].Value = nITJAmt.ToString();

            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(120, 240, 180);
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1 - 1].Text = "    전        체 ";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8 - 1].Text = (nOTcnt + nITcnt) + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10 - 1].Value = (nOTMAmt + nITMAmt).ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11 - 1].Value = (nOTIAmt + nITIAmt).ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12 - 1].Value = (nOTSAmt + nITSAmt).ToString();

            Application.DoEvents();
            if ((nOTSAmt + nITSAmt) != 0 && (nOTMSak + nITMSak) != 0)
            {
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13 - 1].Text =
                    ((nOTSAmt + nITSAmt) / (nOTMSak + nITMSak) * 100).ToString("#0.00") + "%";
            }
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14 - 1].Value = (nOTJAmt + nITJAmt).ToString();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 15 - 1].Value = " ";
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = " 자보 진료비 입금 현황 ";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "                                                                               " + "담당자:" + clsType.User.JobName + " 인", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoGB_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoGB0.Checked == true)
            {
                panHidden0.Visible = true;
                panHidden1.Visible = false;
            }
            else
            {
                panHidden0.Visible = false;
                panHidden1.Visible = true;

            }
        }
    }
}
