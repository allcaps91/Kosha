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
    /// File Name       : frmPmpaViewDrgTong.cs
    /// Description     : DRGDATACHECK_LIST
    /// Author          : 박창욱
    /// Create Date     : 2017-10-24
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iviewa\IVIEWAF.FRM(FrmDrgTong.frm) >> frmPmpaViewDrgTong.cs 폼이름 재정의" />	
    public partial class frmPmpaViewDrgTong : Form
    {
        double[] nAmAMT = new double[61];

        clsOrdFunction cof = new clsOrdFunction();
        ComFunc CF = new ComFunc();

        public frmPmpaViewDrgTong()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = true;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;
            btnExit.Enabled = true;
            dtpFDate.Enabled = true;
            dtpTDate.Enabled = true;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 30;

            dtpFDate.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            panView.Visible = false;
            ssView.Focus();
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

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "** DRG DATA CHECK LIST **";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.9f);

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
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nREAD = 0;
            int nRow = 0;

            string strIllNameK = "";
            double nIIAmt = 0;

            btnSearch.Enabled = false;
            btnExit.Enabled = false;
            dtpFDate.Enabled = false;
            dtpTDate.Enabled = false;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(a.Indate, 'YYYY-MM-DD') InDate, TO_CHAR(a.OutDate,'YYYY-MM-DD') OutDate, a.Pano,";
                SQL = SQL + ComNum.VBLF + "       b.Sname, a.Bi, a.DeptCode,";
                SQL = SQL + ComNum.VBLF + "       a.Ilsu, b.Sex, b.Age,";
                SQL = SQL + ComNum.VBLF + "       a.IllCode1,";
                SQL = SQL + ComNum.VBLF + "       Amt01+Amt02+Amt03+Amt04+Amt05+Amt06+Amt07+Amt08+Amt09+Amt10+";
                SQL = SQL + ComNum.VBLF + "       Amt11+Amt12+Amt13+Amt14+Amt15+Amt16+Amt17+Amt18+Amt19+Amt20 PayTot,";
                SQL = SQL + ComNum.VBLF + "       Amt21+Amt22+Amt23+Amt24+Amt25+Amt26+Amt27+Amt28+Amt29+Amt30+Amt31+Amt32+Amt33+Amt36+";
                SQL = SQL + ComNum.VBLF + "       Amt37+Amt38+Amt39+Amt40+Amt41+Amt42+Amt43+Amt44+Amt45+Amt46+Amt47+Amt48+Amt49 Bigup,";
                SQL = SQL + ComNum.VBLF + "       Amt34,Amt35,Amt50,";
                SQL = SQL + ComNum.VBLF + "       a.ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_TRANS a, ";
                SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "IPD_NEW_MASTER b ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND a.ActDate >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND a.ActDate <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND a.Amt50 > 0";
                SQL = SQL + ComNum.VBLF + "   AND a.Pano = b.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "   AND a.IPDNO = b.IPDNO ";
                SQL = SQL + ComNum.VBLF + "   AND a.IllCode1 IN ('H25','H259','H250','H260','J03','J350','K35','K350',";
                SQL = SQL + ComNum.VBLF + "                    'K351','O641','O610','O332','O342','O80','O80A','O63',";
                SQL = SQL + ComNum.VBLF + "                    'K36','K37','K61','K610','K605','K40','K35','K350','K351',";
                SQL = SQL + ComNum.VBLF + "                    'I84','K604','K61','C56','D25','D27','C532','N870','H25','H250',";
                SQL = SQL + ComNum.VBLF + "                    'N800','O63','O80','072','N812','J03','J351','J353','J038')";
                SQL = SQL + ComNum.VBLF + "   AND a.AmSet5 <> '5'";
                SQL = SQL + ComNum.VBLF + "   AND a.Amt10  >  0";
                SQL = SQL + ComNum.VBLF + "   AND (a.Bi <> '31' AND a.Bi <> '52')";
                SQL = SQL + ComNum.VBLF + " ORDER BY a.DeptCode,a.IllCode1,a.InDate,a.Pano";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRow = 0;
                nREAD = dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Age"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["Ilsu"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["IllCode1"].ToString().Trim();

                    //상병명칭을 READ
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT IllNameK ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                    SQL = SQL + ComNum.VBLF + "  WHERE IllCode = '" + dt.Rows[i]["IllCode1"].ToString().Trim() + "'";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count == 0)
                    {
                        strIllNameK = "** ERROR **";
                    }
                    else
                    {
                        strIllNameK = dt1.Rows[0]["IllNameK"].ToString().Trim();
                    }
                    dt1.Dispose();
                    dt1 = null;

                    ssView_Sheet1.Cells[nRow - 1, 9].Text = strIllNameK;
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = VB.Val(dt.Rows[i]["Amt50"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = VB.Val(dt.Rows[i]["PayTot"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = VB.Val(dt.Rows[i]["Amt35"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = VB.Val(dt.Rows[i]["Amt34"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 14].Text = VB.Val(dt.Rows[i]["Bigup"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 16].Text = dt.Rows[i]["ROWID"].ToString().Trim();


                    //외래에서 II과 수납액을 READ
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT SUM(Amt1+Amt2) IIAmt";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND Pano = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + dt.Rows[i]["InDate"].ToString().Trim() + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + dt.Rows[i]["OutDate"].ToString() + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND WardCode = 'II'";
                    SQL = SQL + ComNum.VBLF + "    AND Bun = '99'"; //외래영수액
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count == 0)
                    {
                        nIIAmt = 0;
                    }
                    else
                    {
                        nIIAmt = VB.Val(dt1.Rows[0]["IIAmt"].ToString().Trim());
                    }
                    ssView_Sheet1.Cells[nRow - 1, 15].Text = nIIAmt.ToString();
                    dt1.Dispose();
                    dt1 = null;
                }
                dt.Dispose();
                dt = null;

                if (nRow == 0)
                {
                    ComFunc.MsgBox("자료가 1건도 없습니다.");
                    Cursor.Current = Cursors.Default;
                    dtpFDate.Enabled = true;
                    dtpTDate.Enabled = true;
                    btnSearch.Enabled = true;
                    btnCancel.Enabled = false;
                    btnPrint.Enabled = false;
                    btnExit.Enabled = true;
                    dtpFDate.Focus();
                    return;
                }
                else
                {
                    ssView_Sheet1.RowCount = nRow;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                }

                Cursor.Current = Cursors.Default;
                dtpFDate.Enabled = true;
                dtpTDate.Enabled = true;
                btnSearch.Enabled = true;
                btnCancel.Enabled = false;
                btnPrint.Enabled = false;
                btnExit.Enabled = true;
                dtpFDate.Focus();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                dtpFDate.Enabled = true;
                dtpTDate.Enabled = true;
                btnSearch.Enabled = true;
                btnCancel.Enabled = false;
                btnPrint.Enabled = false;
                btnExit.Enabled = true;
                dtpFDate.Focus();
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewDrgTong_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            dtpFDate.Value = Convert.ToDateTime(VB.Left(strSysDate, 8) + "01");
            dtpTDate.Value = Convert.ToDateTime(strSysDate);

            ssView_Sheet1.Columns[16].Visible = false;

            panView.Visible = false;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;
            dtpFDate.Enabled = true;
            dtpTDate.Enabled = true;
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            double nTot1 = 0;
            double nTot2 = 0;
            string strAmt = "";
            string strROWID = "";
            string strLabel = "";
            string strIllCode = "";

            strROWID = ssView_Sheet1.Cells[e.Row, 16].Text.Trim();

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(a.Indate, 'YYYY-MM-DD') Indate1, a.Pano, b.Sname, a.Bi, a.DeptCode, a.Ilsu, b.Sex, b.Age,";
                SQL = SQL + ComNum.VBLF + "        a.IllCode1, a.IllCode2, a.IllCode3, a.IllCode4, a.IllCode5, a.IllCode6,";
                SQL = SQL + ComNum.VBLF + "        Amt01,Amt02,Amt03,Amt04,Amt05,Amt06,Amt07,Amt08,Amt09,Amt10,";
                SQL = SQL + ComNum.VBLF + "        Amt11,Amt12,Amt13,Amt14,Amt15,Amt16,Amt17,Amt18,Amt19,Amt20,";
                SQL = SQL + ComNum.VBLF + "        Amt21,Amt22,Amt23,Amt24,Amt25,Amt26,Amt27,Amt28,Amt29,Amt30,";
                SQL = SQL + ComNum.VBLF + "        Amt31,Amt32,Amt33,Amt34,Amt35,Amt36,Amt37,Amt38,Amt39,Amt40,";
                SQL = SQL + ComNum.VBLF + "        Amt41,Amt42,Amt43,Amt44,Amt45,Amt46,Amt47,Amt48,Amt49,Amt50,";
                SQL = SQL + ComNum.VBLF + "        Amt51,Amt52,Amt53,Amt54,Amt55,Amt56,Amt57,Amt58,Amt59,Amt60, a.ROWID";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS a,";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_NEW_MASTER b";
                SQL = SQL + ComNum.VBLF + "  WHERE a.ROWID = '" + strROWID + "'";
                SQL = SQL + ComNum.VBLF + "    AND a.PANO = b.PANO(+)";
                SQL = SQL + ComNum.VBLF + "    AND a.IPDNO = b.IPDNO";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                panView.Visible = true;
                lblID0.Text = "";
                lblID1.Text = "";
                lblID2.Text = "";

                //Amt Area Move
                for (i = 1; i < 61; i++)
                {
                    strAmt = "AMT" + i.ToString("00");
                    nAmAMT[i] = VB.Val(dt.Rows[0][strAmt].ToString().Trim());
                }

                //급여, 비급여 소계
                nTot1 = 0;
                nTot2 = 0;

                for (k = 1; k < 50; k++)
                {
                    if (k > 1 && k < 21)
                    {
                        nTot1 += nAmAMT[k];
                    }
                    else
                    {
                        nTot2 += nAmAMT[k];
                    }
                }


                //Label_Show
                lblID0.Text = "등록번호 : " + dt.Rows[0]["Pano"].ToString().Trim() + " " + dt.Rows[0]["Sname"].ToString().Trim();
                lblID1.Text = "입원일자 : " + dt.Rows[0]["Indate1"].ToString().Trim() + " " + "(" + dt.Rows[0]["Ilsu"].ToString().Trim() + "일)";
                strLabel = "성별나이 : " + dt.Rows[0]["Age"].ToString().Trim() + " / " + dt.Rows[0]["Sex"].ToString().Trim() + " ";
                strLabel += " (" + CF.Read_Bi_Name(clsDB.DbCon, dt.Rows[0]["Bi"].ToString().Trim(), "2") + ") ";
                
                lblID2.Text = strLabel;


                //Sheet_Show
                for (i = 1; i < 7; i++)
                {
                    switch (i)
                    {
                        case 1:
                            strIllCode = dt.Rows[0]["IllCode1"].ToString().Trim();
                            if (strIllCode == "")
                            {
                                break;
                            }
                            ssView3_Sheet1.Cells[i - 1, 0].Text = strIllCode;
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " SELECT IllCode, IllNameK";
                            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                            SQL = SQL + ComNum.VBLF + "  WHERE IllCode = '" + strIllCode.Trim().ToUpper() + "'";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }
                            ssView3_Sheet1.Cells[i - 1, 1].Text = dt1.Rows[0]["IllNameK"].ToString().Trim();
                            dt1.Dispose();
                            dt1 = null;
                            break;
                        case 2:
                            strIllCode = dt.Rows[0]["IllCode2"].ToString().Trim();
                            if (strIllCode == "")
                            {
                                break;
                            }
                            ssView3_Sheet1.Cells[i - 1, 0].Text = strIllCode;
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " SELECT IllCode, IllNameK";
                            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                            SQL = SQL + ComNum.VBLF + "  WHERE IllCode = '" + strIllCode.Trim().ToUpper() + "'";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }
                            ssView3_Sheet1.Cells[i - 1, 1].Text = dt1.Rows[0]["IllNameK"].ToString().Trim();
                            dt1.Dispose();
                            dt1 = null;
                            break;
                        case 3:
                            strIllCode = dt.Rows[0]["IllCode3"].ToString().Trim();
                            if (strIllCode == "")
                            {
                                break;
                            }
                            ssView3_Sheet1.Cells[i - 1, 0].Text = strIllCode;
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " SELECT IllCode, IllNameK";
                            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                            SQL = SQL + ComNum.VBLF + "  WHERE IllCode = '" + strIllCode.Trim().ToUpper() + "'";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }
                            ssView3_Sheet1.Cells[i - 1, 1].Text = dt1.Rows[0]["IllNameK"].ToString().Trim();
                            dt1.Dispose();
                            dt1 = null;
                            break;
                        case 4:
                            strIllCode = dt.Rows[0]["IllCode4"].ToString().Trim();
                            if (strIllCode == "")
                            {
                                break;
                            }
                            ssView3_Sheet1.Cells[i - 1, 0].Text = strIllCode;
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " SELECT IllCode, IllNameK";
                            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                            SQL = SQL + ComNum.VBLF + "  WHERE IllCode = '" + strIllCode.Trim().ToUpper() + "'";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }
                            ssView3_Sheet1.Cells[i - 1, 1].Text = dt1.Rows[0]["IllNameK"].ToString().Trim();
                            dt1.Dispose();
                            dt1 = null;
                            break;
                        case 5:
                            strIllCode = dt.Rows[0]["IllCode5"].ToString().Trim();
                            if (strIllCode == "")
                            {
                                break;
                            }
                            ssView3_Sheet1.Cells[i - 1, 0].Text = strIllCode;
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " SELECT IllCode, IllNameK";
                            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                            SQL = SQL + ComNum.VBLF + "  WHERE IllCode = '" + strIllCode.Trim().ToUpper() + "'";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }
                            ssView3_Sheet1.Cells[i - 1, 1].Text = dt1.Rows[0]["IllNameK"].ToString().Trim();
                            dt1.Dispose();
                            dt1 = null;
                            break;
                        case 6:
                            strIllCode = dt.Rows[0]["IllCode6"].ToString().Trim();
                            if (strIllCode == "")
                            {
                                break;
                            }
                            ssView3_Sheet1.Cells[i - 1, 0].Text = strIllCode;
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " SELECT IllCode, IllNameK";
                            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                            SQL = SQL + ComNum.VBLF + "  WHERE IllCode = '" + strIllCode.Trim().ToUpper() + "'";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }
                            ssView3_Sheet1.Cells[i - 1, 1].Text = dt1.Rows[0]["IllNameK"].ToString().Trim();
                            dt1.Dispose();
                            dt1 = null;
                            break;
                    }
                }


                //AMT_SHOW
                //Spread Move Show
                for (k = 1; k < 61; k++)
                {
                    if (k > 10 && k < 21)
                    {
                        ssView2_Sheet1.Cells[k - 11, 3].Text = nAmAMT[k].ToString("###,###,##0") + " ";
                    }
                    else if (k > 20 && k < 31)
                    {
                        ssView2_Sheet1.Cells[k - 21, 5].Text = nAmAMT[k].ToString("###,###,##0") + " ";
                    }
                    else if (k > 30 && k < 41)
                    {
                        ssView2_Sheet1.Cells[k - 31, 7].Text = nAmAMT[k].ToString("###,###,##0") + " ";
                    }
                    else if (k > 40 && k < 50)
                    {
                        ssView2_Sheet1.Cells[k - 41, 9].Text = nAmAMT[k].ToString("###,###,##0") + " ";
                    }
                    else if (k == 50)
                    {
                        ssView2_Sheet1.Cells[10, 9].Text = nAmAMT[k].ToString("###,###,##0") + " ";
                    }
                    else if (k > 50 && k < 56)
                    {
                        ssView2_Sheet1.Cells[12, ((k - 50) * 2) - 1].Text = nAmAMT[k].ToString("###,###,##0") + " ";
                    }
                    else if (k > 55 && k < 61)
                    {
                        ssView2_Sheet1.Cells[13, ((k - 55) * 2) - 1].Text = nAmAMT[k].ToString("###,###,##0") + " ";
                    }
                    else
                    {
                        ssView2_Sheet1.Cells[k - 1, 1].Text = nAmAMT[k].ToString("###,###,##0") + " ";
                    }
                }
                ssView2_Sheet1.Cells[10, 3].Text = nTot1.ToString("###,###,##0") + " ";
                ssView2_Sheet1.Cells[10, 7].Text = nTot2.ToString("###,###,##0") + " ";

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

    }
}
