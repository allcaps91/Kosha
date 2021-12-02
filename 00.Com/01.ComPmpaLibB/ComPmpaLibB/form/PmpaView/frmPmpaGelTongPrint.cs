using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpaGelTongPrint : Form
    {
        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : frmPmpaGelTongPrint.cs
        /// Description     : 계약처별 청구및입금 통계
        /// Author          : 최익준
        /// Create Date     : 2017-09-18
        /// Update History  : 
        /// <history>       
        /// </history>
        /// <seealso>
        /// d:\psmh\Etc\misu\MISUGYE.vbp
        /// </seealso>
        /// </summary>
        double[] nAmt = new double[10];
        double[] nTot = new double[10];

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        string GstrMiaCode = "";
        string GstrMiaName = "";
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaGelTongPrint(string strMiaCode, string strMiaName)
        {
            GstrMiaCode = strMiaCode;
            GstrMiaName = strMiaName;
            InitializeComponent();
        }

        public frmPmpaGelTongPrint()
        {
            InitializeComponent();
        }

        private void frmPmpaGelTongPrint_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int nMM = 0;
            int i = 0;

            nYY = (int)VB.Val(VB.Left(strDTP, 4));
            nMM = (int)VB.Val(VB.Mid(strDTP, 6, 2));

            for (i = 0; i < 12; i++)
            {
                cboYYMM.Items.Add((nYY).ToString("0000") + "년" + (nMM).ToString("00") + "월분");
                nMM = nMM - 1;
                if (nMM == 0)
                {
                    nMM = 12;
                    nYY = nYY - 1;
                }

                cboYYMM.SelectedIndex = 0;

                cboClass.Items.Clear();

                cboClass.Items.Add("08. 계약처");
                cboClass.Items.Add("09. 헌혈미수");
                cboClass.Items.Add("11. 보훈청미수");
                cboClass.Items.Add("12. 시각장애자");
                cboClass.Items.Add("13. 심신장애단비");
                cboClass.Items.Add("14. 장애인보장구");
                cboClass.Items.Add("15. 직원대납");
                cboClass.Items.Add("16. 노인장기요양소견서");
                cboClass.Items.Add("17. 방문간호지시서");
                cboClass.Items.Add("18. 치매검사");

                cboClass.SelectedIndex = 0;
            }
        }

        private void btnEixt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboClass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cboYYMM.Focus();
            }
        }

        private void cboYYMM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ssView.Focus();
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            DataTable dt = null;
            DataTable dtFc = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            int j = 0;
            int nREAD = 0;
            int nRow = 0;
            string strYYMM = "";

            string strMiaName = "";

            for (i = 0; i < 10; i++)
            {
                nTot[i] = 0;
            }

            strYYMM = VB.Left(cboYYMM.Text.Trim(), 4) + VB.Mid(cboYYMM.Text.Trim(), 6, 2);

            try
            {
                SQL = "";
                SQL = "                     SELECT GelCode, SUM(IwolAmt) cIwolAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(DECODE(IpdOpd,'I',MisuAmt,0)) cIMAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(DECODE(IpdOpd,'O',MisuAmt,0)) cOMAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(IpgumAmt+EtcAmt) cIpgumAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(SakAmt+BanAmt) cSakAmt,SUM(JanAmt) cJanAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(TotMAmt) cTotMAmt,SUM(TotSAmt) cTotSAmt";
                SQL = SQL + ComNum.VBLF + "   FROM MISU_GELTOT";
                SQL = SQL + ComNum.VBLF + "  WHERE YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND Class = '" + VB.Left(cboClass.Text, 2) + "'";
                SQL = SQL + ComNum.VBLF + "  GROUP BY GelCode";
                SQL = SQL + ComNum.VBLF + "  ORDER BY GelCode";
                SQL = SQL + ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당월의 자료가 없습니다.", "확인");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nRow = 0;
                nREAD = dt.Rows.Count;

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nAmt[1] = VB.Val(dt.Rows[i]["cIwolAmt"].ToString().Trim());     //이월
                    nAmt[2] = VB.Val(dt.Rows[i]["cOMAmt"].ToString().Trim());       //외래청구
                    nAmt[3] = VB.Val(dt.Rows[i]["cIMAmt"].ToString().Trim());       //입원청구
                    nAmt[4] = VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim());    //입금
                    nAmt[5] = VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());      //삭감
                    nAmt[6] = 0;                                                    //삭감율
                    nAmt[7] = VB.Val(dt.Rows[i]["cJanAmt"].ToString().Trim());      //현미수액
                    nAmt[8] = VB.Val(dt.Rows[i]["cTotMAmt"].ToString().Trim());     //삭감율계산용 총진료비
                    nAmt[9] = VB.Val(dt.Rows[i]["cTotSAmt"].ToString().Trim());     //삭감율계산용 총삭감액

                    if (nAmt[8] != 0 && nAmt[9] != 0)
                    {
                        nAmt[6] = nAmt[9] / nAmt[8] * 100;
                    }

                    nRow = nRow + 1;

                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Rows.Count = nRow;

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT MiaName";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MIA";
                    SQL = SQL + ComNum.VBLF + "WHERE MiaCode = '" + dt.Rows[i]["GelCode"].ToString().Trim() + "'";

                    SqlErr = clsDB.GetDataTable(ref dtFc, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dtFc.Rows.Count == 0)
                    {
                        strMiaName = "-<" + dt.Rows[i]["GelCode"].ToString().Trim() + ">-";
                    }
                    else
                    {
                        strMiaName = dtFc.Rows[0]["MIANAME"].ToString().Trim();
                    }
                    dtFc.Dispose();
                    dtFc = null;

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strMiaName;

                    for (j = 1; j <= 7; j++)
                    {
                        if (j != 6)
                        {
                            ssView_Sheet1.Cells[nRow - 1, j].Text = VB.Format(nAmt[j], "###,###,###,##0");
                        }
                        if (j == 6)
                        {
                            ssView_Sheet1.Cells[nRow - 1, j].Text = VB.Format(nAmt[j], "##0.00") + "%";
                        }
                        nTot[j] = nTot[j] + nAmt[j];
                    }
                    nTot[8] = nTot[8] + nAmt[8];
                    nTot[9] = nTot[9] + nAmt[9];

                }
                dt.Dispose();
                dt = null;

                nRow = nRow + 1;
                ssView_Sheet1.RowCount = nRow;
                nTot[6] = 0;

                if (nTot[8] != 0 && nTot[9] != 0)
                {
                    nTot[6] = nTot[9] / nTot[8] * 100; // 삭감율
                }
                ssView_Sheet1.Cells[nRow - 1, 0].Text = "**전체합계**";

                for (j = 1; j <= 7; j++)
                {
                    if (j != 6)
                    {
                        ssView_Sheet1.Cells[nRow - 1, j].Text = VB.Format(nTot[j], "###,###,###,##0");
                    }

                    if (j == 6)
                    {
                        ssView_Sheet1.Cells[nRow - 1, j].Text = VB.Format(nTot[j], "##0.00") + "%";
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

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

            strTitle = "청구 및 미수 내역 총괄표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("미수종류 : " + cboClass.Text, new Font("굴림체", 12), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, false);
            strFooter += CS.setSpdPrint_String("출력시간 : " + cboYYMM.Text + clsType.User.JobName, new Font("굴림체", 12), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 20, 20, 0);
            ssView_Sheet1.PrintInfo.Centering = Centering.Horizontal;
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, false, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }
    }
}
