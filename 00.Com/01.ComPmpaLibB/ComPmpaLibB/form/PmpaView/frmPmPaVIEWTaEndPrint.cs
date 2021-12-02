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
    /// File Name       : frmPmPaVIEWTaEndPrint
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\misu\misuta.vbp\MISUT209.FRM (FrmTaEndPrint.frm)>> frmPmPaVIEWTaEndPrint.cs 폼이름 재정의" />
    /// 
    public partial class frmPmPaVIEWTaEndPrint : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string strdtP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string[] GstrGels = new string[61];
        int[] nTotCnt = new int[4];
        double[,] nTotAmt = new double[4, 5];

        public frmPmPaVIEWTaEndPrint()
        {
            InitializeComponent();
        }

        private void frmPmPaVIEWTaEndPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            cboGel.Items.Add("전   체");
            GstrGels[0] = "9999";

            dtpFdate.Value = Convert.ToDateTime(strdtP);
            dtpTdate.Value = Convert.ToDateTime(strdtP);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT MiaCode AS GelCode, MiaName                   ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA                  ";
                SQL = SQL + ComNum.VBLF + "  WHERE (MiaCode LIKE 'JK%' OR MiaCode LIKE 'KB%') ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY MiaCode                                 ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboGel.Items.Add(dt.Rows[i]["MiaName"].ToString().Trim());
                    GstrGels[i + 1] = (dt.Rows[i]["GelCode"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                cboGel.SelectedIndex = 0;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int j = 0;
            int nRowCnt = 0;
            int nGelFLAG = 0;
            double nAmt1 = 0;
            double nAmt2 = 0;
            double nAmt3 = 0;
            double nAmt4 = 0;
            string strSqlGel = "";
            string strOldGel = "";
            string strNewGel = "";
            string strOldPano = "";
            string strNewPano = "";
            string strFirst = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.RowCount = 0;
            nGelFLAG = 1;
            strOldGel = "";
            strFirst = "OK";

            strSqlGel = GstrGels[cboGel.SelectedIndex];

            Cursor.Current = Cursors.WaitCursor;

            //try
            //{
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT b.GelCode,a.Pano,a.Sname,a.CoprNo,b.IpdOpd,                ";
            SQL = SQL + ComNum.VBLF + "        TO_CHAR(a.Date2,'YY-MM-DD') Sdate,                         ";
            SQL = SQL + ComNum.VBLF + "        TO_CHAR(a.Date3,'YY-MM-DD') Edate,                         ";
            SQL = SQL + ComNum.VBLF + "        TO_CHAR(b.BDate,'YYYY-MM-DD') Bdate,                       ";
            SQL = SQL + ComNum.VBLF + "        TO_CHAR(b.FromDate,'YYMMDD') FrDate,                       ";
            SQL = SQL + ComNum.VBLF + "        TO_CHAR(b.ToDate,'YYMMDD') ToDate,b.Ilsu,b.DeptCode,       ";
            SQL = SQL + ComNum.VBLF + "        b.Amt2 MisuAmt,b.Amt3 IpgumAmt,                            ";
            SQL = SQL + ComNum.VBLF + "        b.Amt4+b.Amt5+b.Amt6+b.Amt7 SakAmt, b.Remark               ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SANID a," + ComNum.DB_PMPA + "MISU_IDMST b                                   ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "    AND a.Date3 >= TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  ";
            SQL = SQL + ComNum.VBLF + "    AND a.Date3 <= TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  ";
            SQL = SQL + ComNum.VBLF + "    AND a.Bi = '52'                                                ";  //'자보
            SQL = SQL + ComNum.VBLF + "    AND a.Pano = b.MisuID                                          ";
            SQL = SQL + ComNum.VBLF + "    AND b.Class = '07'                                             "; //'자보미수

            if (strSqlGel != "9999")
            {
                SQL = SQL + ComNum.VBLF + "    AND b.GelCode = '" + (strSqlGel).Trim() + "'                  ";
            }

            SQL = SQL + ComNum.VBLF + "  ORDER BY b.GelCode,a.Sname,b.Bdate,b.IpdOpd                      ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                dt.Dispose();
                dt = null;
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


            for (i = 0; i <= 3; i++)
            {
                nTotCnt[i] = 0;
                for (j = 1; j <= 4; j++)
                {
                    nTotAmt[i, j] = 0;
                }
            }

            nRowCnt = dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ADD_SPREAD(dt, ref strNewGel, ref strNewPano, ref nGelFLAG, ref strOldGel, ref strOldPano, i, ref nAmt1, ref nAmt2, ref nAmt3, ref nAmt4);
            }
            dt.Dispose();
            dt = null;

            Misu_Sub_Pano();
            Misu_Sub_Gel();
            Cursor.Current = Cursors.Default;

            //}
            //catch (Exception ex)
            //{
            //    ComFunc.MsgBox(ex.Message);
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //}
        }

        private void ADD_SPREAD(DataTable dtFn, ref string strNewGel, ref string strNewPano, ref int nGelFLAG, ref string strOldGel, ref string strOldPano, int i, ref double nAmt1, ref double nAmt2, ref double nAmt3, ref double nAmt4)
        {
            string strPano = "";
            string strSname = "";
            string strCarNo = "";
            string strGelName = "";
            string strGelCode = "";
            string strSDate = "";  //개시일자
            string strEDate = "";  //종결일자
            string strRemark = "";

            strNewGel = dtFn.Rows[i]["GelCode"].ToString().Trim();
            strNewPano = dtFn.Rows[i]["Pano"].ToString().Trim();

            if (strOldGel != strNewGel && strOldGel != "")
            {
                Misu_Sub_Pano();
                Misu_Sub_Gel();
                nGelFLAG = 1;
            }
            else if (strOldPano != strNewPano && strOldPano != "")
            {
                Misu_Sub_Pano();
            }

            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

            if (strOldGel != strNewGel || strOldPano != strNewPano)
            {
                strPano = dtFn.Rows[i]["Pano"].ToString().Trim();
                strSname = dtFn.Rows[i]["Sname"].ToString().Trim();
                strGelCode = dtFn.Rows[i]["GelCode"].ToString().Trim();
                strRemark = dtFn.Rows[i]["Remark"].ToString().Trim();
                strCarNo = dtFn.Rows[i]["CoprNo"].ToString().Trim();
                strSDate = dtFn.Rows[i]["Sdate"].ToString().Trim();
                strEDate = dtFn.Rows[i]["Edate"].ToString().Trim();
                strGelName = CPF.GET_BAS_MIA(clsDB.DbCon, strGelCode.Trim());
                strOldGel = strNewGel;
                strOldPano = strNewPano;

                if (nGelFLAG == 0)
                {
                    strGelName = "";
                }
                if (nGelFLAG == 1)
                {
                    nGelFLAG = 0;
                }
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1 - 1].Text = strGelName;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2 - 1].Text = strPano;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3 - 1].Text = strSname;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4 - 1].Text = strCarNo;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5 - 1].Text = strSDate;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6 - 1].Text = strEDate;
            }

            strRemark = dtFn.Rows[i]["FrDate"].ToString().Trim() + "=>";
            strRemark = strRemark + dtFn.Rows[i]["ToDate"].ToString().Trim() + " ";

            if (dtFn.Rows[i]["IpdOpd"].ToString().Trim() == "0")
            {
                strRemark = strRemark + "외래 ";
            }
            else
            {
                strRemark = strRemark + "입원 ";
            }
            strRemark = strRemark + dtFn.Rows[i]["DeptCode"].ToString().Trim();

            nAmt1 = VB.Val(dtFn.Rows[i]["MisuAmt"].ToString().Trim());
            nAmt2 = VB.Val(dtFn.Rows[i]["IpgumAmt"].ToString().Trim());
            nAmt3 = VB.Val(dtFn.Rows[i]["SakAmt"].ToString().Trim());
            nAmt4 = nAmt1 - nAmt2 - nAmt3;

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7 - 1].Text = dtFn.Rows[i]["Bdate"].ToString().Trim();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8 - 1].Text = strRemark;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9 - 1].Text = nAmt1.ToString("###,###,###,##0");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10 - 1].Text = nAmt2.ToString("###,###,###,##0");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11 - 1].Text = nAmt3.ToString("###,###,###,##0");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12 - 1].Text = nAmt4.ToString("###,###,###,##0");

            nTotCnt[1] = nTotCnt[1] + 1;
            nTotAmt[1, 1] = nTotAmt[1, 1] + nAmt1;
            nTotAmt[1, 2] = nTotAmt[1, 2] + nAmt2;
            nTotAmt[1, 3] = nTotAmt[1, 3] + nAmt3;
            nTotAmt[1, 4] = nTotAmt[1, 4] + nAmt4;

            nTotCnt[2] = nTotCnt[2] + 1;
            nTotAmt[2, 1] = nTotAmt[2, 1] + nAmt1;
            nTotAmt[2, 2] = nTotAmt[2, 2] + nAmt2;
            nTotAmt[2, 3] = nTotAmt[2, 3] + nAmt3;
            nTotAmt[2, 4] = nTotAmt[2, 4] + nAmt4;

        }

        private void Misu_Sub_Pano()
        {
            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3 - 1].Text = "개인별계";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4 - 1].Text = nTotCnt[1] + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9 - 1].Text = nTotAmt[1, 1].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10 - 1].Text = nTotAmt[1, 2].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11 - 1].Text = nTotAmt[1, 3].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12 - 1].Text = nTotAmt[1, 4].ToString("###,###,###,##0");

            nTotCnt[1] = 0;
            nTotAmt[1, 1] = 0;
            nTotAmt[1, 2] = 0;
            nTotAmt[1, 3] = 0;
            nTotAmt[1, 4] = 0;
        }

        private void Misu_Sub_Gel()
        {
            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3 - 1].Text = "계약처계";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4 - 1].Text = nTotCnt[2] + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9 - 1].Text = nTotAmt[2, 1].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10 - 1].Text = nTotAmt[2, 2].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11 - 1].Text = nTotAmt[2, 3].ToString("###,###,###,##0");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12 - 1].Text = nTotAmt[2, 4].ToString("###,###,###,##0");

            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(0, 255, 0);

            nTotCnt[2] = 0;
            nTotAmt[2, 1] = 0;
            nTotAmt[2, 2] = 0;
            nTotAmt[2, 3] = 0;
            nTotAmt[2, 4] = 0;
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

            strTitle = "자보 종결자 미수 CHECK-LIST";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업기간 : " + dtpFdate.Text + " => " + dtpTdate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
