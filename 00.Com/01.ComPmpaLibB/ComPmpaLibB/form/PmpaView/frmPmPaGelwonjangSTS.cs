using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{       /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : frmPmPaVIEWJicwonmisu.cs
        /// Description     : 조합별 미수금 현황
        /// Author          : 김효성
        /// Create Date     : 2017-09-12
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "d:\psmh\misu\misumir.vbp\FrmGelwonjang(MISUM301.FRM)  >> frmPmPaVIEWJicwonmisu.cs 폼이름 재정의" />	
        /// 
    public partial class frmPmPaGelwonjangSTS : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu pm = new clsPmpaMisu();
        clsPmpaFunc CPF = new clsPmpaFunc();
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        frmPmpaViewGelSearch frm = null;

        string GstrMiaCode = "";
        string GstrMiaName = "";

        public frmPmPaGelwonjangSTS(string strMiaCode, string strMiaName)
        {
            GstrMiaCode = strMiaCode;
            GstrMiaName = strMiaName;
            InitializeComponent();
        }

        public frmPmPaGelwonjangSTS()
        {
            InitializeComponent();
        }

        private void frmPmPaGelwonjangSTS_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int nMM = 0;
            int i = 0;

            nYY = (int)VB.Val(VB.Left(strDTP, 4));
            nMM = (int)VB.Val(VB.Mid(strDTP, 6, 2));

            for (i = 1; i <= 60; i++)
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
            txtGelcode.Text = "";
            lblGelName.Text = "";
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return;//권한 확인
            //}
            int i = 0;
            int j = 0;
            int nREAD = 0;
            int nRow = 0;

            string strYYMM = "";
            string strBackYYMM = "";    //'전월
            string strFDate = "";
            string strTDate = "";
            string strNewData = "";
            string strOldData = "";
            string strGubun = "";

            double nAmt = 0;
            double nQty = 0;
            double nTotIwolAmt = 0;
            double nTotMirAmt = 0;
            double nTotIpgumAmt = 0;
            double nTotJanAmt = 0;

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strBackYYMM = CPF.DATE_YYMM_ADD(strYYMM, -1);

            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);

            btnView.Enabled = false;
            btnPrint.Enabled = false;

            CmdOK_Screen_Clear();
            CmdOK_Iwol_Display(strBackYYMM, ref nRow, ref nTotIwolAmt);
            CmdOK_Slip_Display(strFDate, strTDate, ref strOldData, ref nRow, ref strNewData, ref strGubun, ref nQty, ref nAmt, nTotIwolAmt, nTotMirAmt, nTotIpgumAmt);

            btnView.Enabled = true;
            btnPrint.Enabled = true;
        }
        #region   조회 
        private void CmdOK_Screen_Clear()
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

        }

        private void CmdOK_Iwol_Display(string strBackYYMM, ref int nRow, ref double nTotIwolAmt)   //조합별 전월이월액 Display
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "	SELECT SUM(JanAmt) cIwolAmt                                 ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GELTOT                    ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1				                    ";
                SQL = SQL + ComNum.VBLF + "    AND YYMM = '" + strBackYYMM + "'                          ";
                SQL = SQL + ComNum.VBLF + "    AND GelCode = '" + txtGelcode.Text + "'        ";

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

                nTotIwolAmt = VB.Val(dt.Rows[0]["cIwolAmt"].ToString().Trim());

                dt.Dispose();
                dt = null;

                nRow = nRow + 1;
                if (nRow > ssView_Sheet1.RowCount)
                {
                    ssView_Sheet1.RowCount = nRow;
                }
                ssView_Sheet1.Cells[nRow - 1, 1].Text = "** 전월이월 **";
                ssView_Sheet1.Cells[nRow - 1, 10].Text = nTotIwolAmt.ToString("###,###,###,##0 ");


                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private void CmdOK_Slip_Display(string strFDate, string strTDate, ref string strOldData, ref int nRow, ref string strNewData, ref string strGubun, ref double nQty, ref double nAmt, double nTotIwolAmt, double nTotMirAmt, double nTotIpgumAmt)   //미수 상세내역 Display
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(a.Bdate,'YYYY-MM-DD') Bdate,a.IpdOpd,a.Gubun,                      ";
                SQL = SQL + ComNum.VBLF + "        a.MisuID,a.Qty,a.Amt,a.Remark,b.Bun,TO_CHAR(b.Bdate,'YYYY-MM-DD') MirDate  ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP a," + ComNum.DB_PMPA + "MISU_IDMST b        ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1 ";
                SQL = SQL + ComNum.VBLF + "    AND a.Bdate >= TO_DATE('" + strFDate + "','yyyy-mm-dd')                        ";
                SQL = SQL + ComNum.VBLF + "    AND a.Bdate <= TO_DATE('" + strTDate + "','yyyy-mm-dd')                        ";
                SQL = SQL + ComNum.VBLF + "    AND a.GelCode = '" + txtGelcode.Text + "'                                ";
                SQL = SQL + ComNum.VBLF + "    AND a.WRTNO = b.WRTNO(+)                                                       ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.Bdate,a.MisuID,a.Gubun                                                ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                strOldData = "";

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

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow = nRow + 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    strNewData = dt.Rows[i]["Bdate"].ToString().Trim();

                    if (strNewData != strOldData)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = strNewData;
                        strOldData = strNewData;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 1].Text = pm.READ_MisuGye_TA(dt.Rows[i]["Gubun"].ToString().Trim(), false);
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = pm.READ_MisuIpdOpd(dt.Rows[i]["IpdOpd"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["MirDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = pm.READ_MisuBunya(dt.Rows[i]["Bun"].ToString().Trim());

                    strGubun = dt.Rows[i]["Gubun"].ToString().Trim();
                    nQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim());
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());

                    switch (strGubun)
                    {
                        case "11":
                        case "12":
                        case "13":
                        case "14":
                        case "15":
                            ssView_Sheet1.Cells[nRow - 1, 6].Text = nQty.ToString("###,###,##0 ");
                            ssView_Sheet1.Cells[nRow - 1, 7].Text = nAmt.ToString("###,###,##0 ");
                            nTotMirAmt = nTotMirAmt + nAmt;
                            break;
                        default:
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = nQty.ToString("###,###,##0 ");
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = nAmt.ToString("###,###,##0 ");
                            nTotIpgumAmt = nTotIpgumAmt + nAmt;
                            break;
                    }

                    nAmt = nTotIwolAmt + nTotMirAmt - nTotIpgumAmt;
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = nAmt.ToString("###,###,##0 ");
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["Remark"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                nRow = nRow + 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.Cells[nRow - 1, 1].Text = "**월계**";
                ssView_Sheet1.Cells[nRow - 1, 7].Text = nTotMirAmt.ToString("###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotMirAmt.ToString("###,###,##0 ");
                nAmt = nTotIwolAmt + nTotMirAmt - nTotIpgumAmt;
                ssView_Sheet1.Cells[nRow - 1, 10].Text = nAmt.ToString("###,###,###,##0 ");

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "신 자 감 액 명 단";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 27, 0);
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtGelcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            if (e.KeyCode == Keys.Enter)
            {
                txtGelcode.Text = "";
                return;
            }
            else
            {
                lblGelName.Text = CF.Read_MiaName(clsDB.DbCon, txtGelcode.Text, false);
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            frm = new frmPmpaViewGelSearch();

            frm.rGetData += frm_rGetData;
            frm.rEventClose += frm_rEventClose;
            frm.Show();

            if (GstrMiaCode != "")
            {
                return;
            }
            txtGelcode.Text = GstrMiaCode;
            lblGelName.Text = GstrMiaName;
            GstrMiaCode = "";
            btnView.Focus();
        }

        private void frm_rEventClose()
        {
            if (frm != null)
            {
                frm.Dispose();
                frm = null;
            }
        }

        private void frm_rGetData(string strMiaCode, string strMiaName)
        {
            GstrMiaCode = strMiaCode;
            GstrMiaName = strMiaName;

            if (GstrMiaCode != "")
            {
                txtGelcode.Text = GstrMiaCode.Trim();
                lblGelName.Text = GstrMiaName.Trim();
                GstrMiaCode = "";
                btnView.Focus();
            }

            frm.Dispose();
            frm = null;
        }

    }
}

