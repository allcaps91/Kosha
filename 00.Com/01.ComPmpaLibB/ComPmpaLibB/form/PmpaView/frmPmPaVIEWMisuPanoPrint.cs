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
    /// File Name       : frmPmPaVIEWMisuPanoPrint
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\misu\misuta.vbp\MISUT208.FRM (FrmMisuPanoPrint.frm)>> frmPmPaVIEWMisuPanoPrint.cs 폼이름 재정의" />

    public partial class frmPmPaVIEWMisuPanoPrint : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string strdtP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string[] GstrGels = new string[51];

        public frmPmPaVIEWMisuPanoPrint()
        {
            InitializeComponent();
        }

        private void frmPmPaVIEWMisuPanoPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            cboGel.Items.Add("전   체");
            GstrGels[0] = "9999";
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT MiaCode GelCode, MiaName                   ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA                  ";
                SQL = SQL + ComNum.VBLF + "  WHERE (MiaCode LIKE 'JK%' OR MiaCode LIKE 'KB%') ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY MiaCode                                 ";

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

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboGel.Items.Add(dt.Rows[i]["MiaName"].ToString().Trim());
                    GstrGels[i + 1] = dt.Rows[i]["GelCode"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                cboGel.SelectedIndex = 0;
                clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "1");
                cboYYMM.SelectedIndex = 1;
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

            int nGelFLAG = 0;
            int i = 0;
            int j = 0;
            int nGelCnt = 0;
            double nAmt1 = 0;
            double nAmt2 = 0;
            double nAmt3 = 0;
            double nAmt4 = 0;
            double nTotAmt1 = 0;
            double nTotAmt2 = 0;
            double nTotAmt3 = 0;
            double nTotAmt4 = 0;
            double nGelTot = 0;
            string strYYMM = "";
            string strLdate = "";
            string strSqlGel = "";
            string strOldGel = "";
            string strNewGel = "";
            string strFirst = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strLdate = CF.READ_LASTDAY(clsDB.DbCon, VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01");

            ssView_Sheet1.RowCount = 0;

            nGelCnt = 0;
            nGelTot = 0;
            nGelFLAG = 1;
            strOldGel = "";
            strFirst = "OK";

            strSqlGel = GstrGels[cboGel.SelectedIndex];

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " ";
                SQL = SQL + ComNum.VBLF + " SELECT M.GelCode,M.MisuID,M.Remark,                               ";
                SQL = SQL + ComNum.VBLF + "        SUM(M.Amt2) MisuAmt,SUM(M.Amt3+Amt5+Amt6+Amt7) IpgumAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(M.Amt4) SakAmt                                         ";
                SQL = SQL + ComNum.VBLF + "   FROM MISU_Monthly a,MISU_IDMST M                                ";
                SQL = SQL + ComNum.VBLF + "  WHERE a.YYMM = '" + strYYMM + "'                                 ";
                SQL = SQL + ComNum.VBLF + "    AND a.Class = '07'                                             ";  //'자보
                SQL = SQL + ComNum.VBLF + "    AND a.JanAmt > 0                                               ";

                if (strSqlGel != "9999")
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.GelCode = '" + (strSqlGel).Trim() + "'                  ";
                }
                SQL = SQL + ComNum.VBLF + "    And a.WRTNO = M.WRTNO                                          ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY M.GelCode,M.MisuID,M.Remark                             ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY M.Gelcode,M.MisuID,M.Remark                             ";

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
                    nAmt1 = VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());
                    nAmt2 = VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());
                    nAmt3 = VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim());
                    nAmt4 = nAmt1 - nAmt2 - nAmt3;

                    if (nAmt4 != 0)
                    {
                        #region  ADD_SPREAD
                        ADD_SPREAD(ref strNewGel, dt, i, ref strOldGel, ref nGelFLAG, ref nGelCnt, ref nGelTot, nAmt1, nAmt2, nAmt3, nAmt4, ref nTotAmt1, ref nTotAmt2, ref nTotAmt3, ref nTotAmt4);
                        #endregion
                    }
                }
                Misu_Sub_Gel(ref nGelCnt, ref nTotAmt1, ref nTotAmt2, ref nTotAmt3, ref nTotAmt4);

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ADD_SPREAD(ref string strNewGel, DataTable dtFn, int i, ref string strOldGel, ref int nGelFLAG, ref int nGelCnt, ref double nGelTot, double nAmt1, double nAmt2, double nAmt3, double nAmt4, ref double nTotAmt1, ref double nTotAmt2, ref double nTotAmt3, ref double nTotAmt4)
        {
            string strPano = "";
            string strSname = "";
            string strCarNo = "";
            string strBname = "";
            string strGelName = "";
            string strGelCode = "";
            string strDate4 = "";   //'사고일
            string strDate5 = "";   //'진료개시일
            string strRemark = "";

            strNewGel = dtFn.Rows[i]["GelCode"].ToString().Trim();

            if (strOldGel == "")
            {
                strOldGel = strNewGel;
            }

            if (strOldGel != strNewGel)
            {
                Misu_Sub_Gel(ref nGelCnt, ref nTotAmt1, ref nTotAmt2, ref nTotAmt3, ref nTotAmt4);
                strOldGel = strNewGel;
                nGelFLAG = 1;
            }

            strPano = dtFn.Rows[i]["MisuID"].ToString().Trim();
            strSname = CPF.Get_BasPatient(clsDB.DbCon, strPano).Rows[0]["SNAME"].ToString().Trim();
            strGelCode = dtFn.Rows[i]["GelCode"].ToString().Trim();
            strRemark = ComFunc.LeftH(dtFn.Rows[i]["Remark"].ToString().Trim() + VB.Space(40), 40);
            strCarNo = (VB.Mid(strRemark, 17, 9).Trim());
            strBname = (ComFunc.RightH(strRemark, 10)).Trim();


            strDate4 = (VB.Mid(strRemark, 1, 8)).Trim();
            strDate5 = (VB.Mid(strRemark, 9, 8)).Trim();

            nGelCnt = nGelCnt + 1;
            nGelTot = nGelTot + nAmt4;

            strGelName = CPF.GET_BAS_MIA(clsDB.DbCon, (strGelCode).Trim());

            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

            if (nGelFLAG == 0)
            {
                strGelName = "";
            }
            if (nGelFLAG == 1)
            {
                nGelFLAG = 0;
            }

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strGelName;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = strPano;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = strSname;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = strDate4;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = strBname;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = strCarNo;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = strDate5;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nAmt1.ToString("###,###,###,##0");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nAmt2.ToString("###,###,###,##0");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nAmt3.ToString("###,###,###,##0");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nAmt4.ToString("###,###,###,##0");

            nTotAmt1 = nTotAmt1 + nAmt1;
            nTotAmt2 = nTotAmt2 + nAmt2;
            nTotAmt3 = nTotAmt3 + nAmt3;
            nTotAmt4 = nTotAmt4 + nAmt4;
        }

        private void Misu_Sub_Gel(ref int nGelCnt, ref double nTotAmt1, ref double nTotAmt2, ref double nTotAmt3, ref double nTotAmt4)
        {
            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

            ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(80, 240, 180);
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "계약처계";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = nGelCnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nTotAmt1.ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nTotAmt2.ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nTotAmt3.ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nTotAmt4.ToString("###,###,###,##0 ");



            nGelCnt = 0;
            nTotAmt1 = 0;
            nTotAmt2 = 0;
            nTotAmt3 = 0;
            nTotAmt4 = 0;
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

            strTitle = "자보 진료비 개인별 미수 현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

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
