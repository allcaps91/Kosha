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
    /// Create Date     : 2017-09-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= d:\psmh\misu\misumir.vbp\MISUM305.FRM" >> frmPmpaTongMisum305.cs 폼이름 재정의" />

    public partial class frmPmpaTongMisum305 : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        double[,] nTotAmt = new double[3, 7];

        int nRow = 0;
        string strYYMM = "";
        string strNewData = "";
        string strOldData = "";

        public frmPmpaTongMisum305()
        {
            InitializeComponent();
        }

        private void frmPmpaTongMisum305_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            //ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            int i = 0;
            int nYY = 0;
            int nMM = 0;

            nYY = (int)VB.Val(VB.Left(strDTP, 4));
            nMM = (int)VB.Val(VB.Mid(strDTP, 6, 2));

            for (i = 1; i < 16; i++)
            {
                cboFDate.Items.Add(nYY.ToString("0000") + "년 " + nMM.ToString("00") + "월분");

                nMM -= 1;
                if (nMM == 0)
                {
                    nMM = 12;
                    nYY -= 1;
                }
            }
            cboFDate.SelectedIndex = 1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            string SQL = "";

            strYYMM = VB.Left(cboFDate.Text, 4) + VB.Mid(cboFDate.Text, 7, 2);

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            try
            {

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;

                ssView_Sheet1.RowCount = 50;

                for (i = 0; i <= 6; i++)
                {
                    nTotAmt[1, i] = 0;
                    nTotAmt[2, i] = 0;
                }

                nRow = 0;
                CmdOK_Display_Main();
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void CmdOK_Display_Main()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Class,GelCode,SUM(IwolAmt) cIwolAmt,SUM(MisuAmt) cMisuAmt,                 ";
                SQL = SQL + ComNum.VBLF + "        SUM(IpgumAmt) cIpgumAmt,SUM(SakAmt) cSakAmt,SUM(BanAmt+EtcAmt) cEtcAmt,    ";
                SQL = SQL + ComNum.VBLF + "        SUM(JanAmt) cJanAmt                                                        ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.VBLF + "MISU_GELTOT                                             ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                                  ";
                SQL = SQL + ComNum.VBLF + "     AND YYMM = '" + strYYMM + "'                                                   ";

                if (rdoBi0.Checked == true)
                    SQL = SQL + ComNum.VBLF + "    AND Class = '01'                                                           ";
                else if (rdoBi1.Checked == true)
                    SQL = SQL + ComNum.VBLF + "    AND Class = '02'                                                           ";
                else if (rdoBi2.Checked == true)
                    SQL = SQL + ComNum.VBLF + "    AND Class = '03'                                                           ";
                else if (rdoBi3.Checked == true)
                    SQL = SQL + ComNum.VBLF + "    AND Class = '04'                                                           ";
                else
                    SQL = SQL + ComNum.VBLF + "    AND Class < '05'                                                           ";

                SQL = SQL + ComNum.VBLF + "  GROUP BY Class,GelCode                                                           ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Class,GelCode                                                           ";

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


                strOldData = "";
                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strNewData = dt.Rows[i]["Class"].ToString().Trim();

                    if (strNewData != strOldData && strOldData != "")
                    {
                        SubTot_Rtn();
                    }

                    nRow = nRow + 1;

                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    if (strNewData != strOldData)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = CPM.READ_MisuClass(strNewData);
                        strOldData = strNewData;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 2 - 1].Text = CPM.READ_BAS_MIA(dt.Rows[i]["GelCode"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 3 - 1].Text = VB.Val(dt.Rows[i]["cIwolAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[nRow - 1, 4 - 1].Text = VB.Val(dt.Rows[i]["cMisuAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[nRow - 1, 5 - 1].Text = VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[nRow - 1, 6 - 1].Text = VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[nRow - 1, 7 - 1].Text = VB.Val(dt.Rows[i]["cEtcAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[nRow - 1, 8 - 1].Text = VB.Val(dt.Rows[i]["cJanAmt"].ToString().Trim()).ToString("###,###,###,##0 ");

                    nTotAmt[1, 1] = nTotAmt[1, 1] + VB.Val(dt.Rows[i]["cIwolAmt"].ToString().Trim());
                    nTotAmt[1, 2] = nTotAmt[1, 2] + VB.Val(dt.Rows[i]["cMisuAmt"].ToString().Trim());
                    nTotAmt[1, 3] = nTotAmt[1, 3] + VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim());
                    nTotAmt[1, 4] = nTotAmt[1, 4] + VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());
                    nTotAmt[1, 5] = nTotAmt[1, 5] + VB.Val(dt.Rows[i]["cEtcAmt"].ToString().Trim());
                    nTotAmt[1, 6] = nTotAmt[1, 6] + VB.Val(dt.Rows[i]["cJanAmt"].ToString().Trim());

                    nTotAmt[2, 1] = nTotAmt[2, 1] + VB.Val(dt.Rows[i]["cIwolAmt"].ToString().Trim());
                    nTotAmt[2, 2] = nTotAmt[2, 2] + VB.Val(dt.Rows[i]["cMisuAmt"].ToString().Trim());
                    nTotAmt[2, 3] = nTotAmt[2, 3] + VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim());
                    nTotAmt[2, 4] = nTotAmt[2, 4] + VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());
                    nTotAmt[2, 5] = nTotAmt[2, 5] + VB.Val(dt.Rows[i]["cEtcAmt"].ToString().Trim());
                    nTotAmt[2, 6] = nTotAmt[2, 6] + VB.Val(dt.Rows[i]["cJanAmt"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                SubTot_Rtn();

                nRow = nRow + 1;
                ssView_Sheet1.RowCount = nRow;

                ssView_Sheet1.Cells[nRow - 1, 2 - 1].Text = " ** 합 계 **";
                ssView_Sheet1.Cells[nRow - 1, 3 - 1].Text = nTotAmt[2, 1].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 4 - 1].Text = nTotAmt[2, 2].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 5 - 1].Text = nTotAmt[2, 3].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 6 - 1].Text = nTotAmt[2, 4].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 7 - 1].Text = nTotAmt[2, 5].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 8 - 1].Text = nTotAmt[2, 6].ToString("###,###,###,##0 ");


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

        private void SubTot_Rtn()
        {
            int j = 0;


            nRow = nRow + 1;
            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }
            ssView_Sheet1.Cells[nRow - 1, 2 - 1].Text = " ** 소 계 **";
            ssView_Sheet1.Cells[nRow - 1, 3 - 1].Text = nTotAmt[1, 1].ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[nRow - 1, 4 - 1].Text = nTotAmt[1, 2].ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[nRow - 1, 5 - 1].Text = nTotAmt[1, 3].ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[nRow - 1, 6 - 1].Text = nTotAmt[1, 4].ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[nRow - 1, 7 - 1].Text = nTotAmt[1, 5].ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[nRow - 1, 8 - 1].Text = nTotAmt[1, 6].ToString("###,###,###,##0 ");

            for (j = 1; j <= 6; j++)
            {
                nTotAmt[1, j] = 0;
            }
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

            strTitle = "조합별 미수금 통계";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);


        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
