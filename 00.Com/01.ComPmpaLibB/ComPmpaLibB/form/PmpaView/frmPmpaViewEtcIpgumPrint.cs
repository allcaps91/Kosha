using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewEtcIpgumPrint.cs
    /// Description     : 기타미수 진료비 입금표
    /// Author          : 박창욱
    /// Create Date     : 2017-10-10
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUG312.FRM(FrmEtcIpgumPrint.frm) >> frmPmpaViewEtcIpgumPrint.cs 폼이름 재정의" />
    public partial class frmPmpaViewEtcIpgumPrint : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsPmpaMisu cpm = new clsPmpaMisu();
        clsPmpaFunc cpf = new clsPmpaFunc();

        public frmPmpaViewEtcIpgumPrint()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            long nWRTNO = 0;
            double nJanAmt = 0;
            double nIpgumAmt = 0;

            double nTotMirAmt = 0;
            double nTotSakAmt = 0;
            double nTotIpgumAmt = 0;
            double nTotChungAmt = 0;

            if (dtpTDate.Value < dtpFDate.Value)
            {
                ComFunc.MsgBox("종료일자가 시작일자보다 작습니다.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            btnPrint.Enabled = false;
            btnSearch.Enabled = false;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            try
            {
                //해당기간의 입금내역을 Read

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT WRTNO,TO_CHAR(BDate,'YY-MM-DD') vBDate,";
                SQL = SQL + ComNum.VBLF + "        MisuID vMisuID, SUM(Amt) vIAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND Class  = '" + VB.Left(cboClass.Text, 2) + "'";
                SQL = SQL + ComNum.VBLF + "    AND Gubun  >= '21'";
                SQL = SQL + ComNum.VBLF + "    AND Gubun  <= '29'";
                SQL = SQL + ComNum.VBLF + "  GROUP BY WRTNO,BDate,MisuID";
                SQL = SQL + ComNum.VBLF + "  ORDER BY BDate,MisuID";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    btnSearch.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("입금된 내역이 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = 20;

                //1건씩 READ 후 Sheet에 Display

                nTotMirAmt = 0;
                nTotSakAmt = 0;
                nTotIpgumAmt = 0;
                nTotChungAmt = 0;

                if (nRead > 19)
                {
                    ssView_Sheet1.RowCount = nRead + 1;
                }

                for (i = 0; i < nRead; i++)
                {
                    nWRTNO = (long)VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim());

                    //미수 Master를 Read
                    cpm.READ_MISU_IDMST(nWRTNO);
                    nJanAmt = clsPmpaType.TMM.Amt[2];   //청구금액
                    nIpgumAmt = VB.Val(dt.Rows[i]["vIAmt"].ToString().Trim());

                    //전월의 미수잔액을 Read
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT SUM(Amt) IpgumAmt";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND WRTNO = '" + VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim()) + "'";
                    SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND Gubun >= '21'";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count == 1)
                    {
                        nJanAmt -= VB.Val(dt1.Rows[0]["IpgumAmt"].ToString().Trim());
                    }

                    dt1.Dispose();
                    dt1 = null;

                    ssView_Sheet1.Cells[i, 0].Text = (i+1).ToString();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["vBdate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["vMisuID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = cpf.Get_BasPatient(clsDB.DbCon, clsPmpaType.TMM.MisuID.Trim()).Rows[0]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = VB.Right(clsPmpaType.TMM.FromDate, 8) + " ~ " + VB.Right(clsPmpaType.TMM.ToDate, 8);
                    ssView_Sheet1.Cells[i, 5].Text = clsPmpaType.TMM.Amt[2].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[i, 6].Text = nIpgumAmt.ToString("###,###,###,##0");
                    if (nJanAmt < 1)
                    {
                        ssView_Sheet1.Cells[i, 7].Text = clsPmpaType.TMM.Amt[4].ToString("###,###,###,##0");    //삭감액
                        if (clsPmpaType.TMM.Amt[2] != 0 && clsPmpaType.TMM.Amt[4] != 0)
                        {
                            ssView_Sheet1.Cells[i, 8].Text = (clsPmpaType.TMM.Amt[4] / clsPmpaType.TMM.Amt[2] * 100).ToString("##0.00") + "%";
                            nTotMirAmt += clsPmpaType.TMM.Amt[2];
                            nTotSakAmt += clsPmpaType.TMM.Amt[4];
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 8].Text = "0.00%";
                        }
                    }
                    nTotIpgumAmt += nIpgumAmt;
                    nTotChungAmt += clsPmpaType.TMM.Amt[2];
                }
                dt.Dispose();
                dt = null;

                if (nRead < 18)
                {
                    ssView_Sheet1.Cells[nRead, 5].Text = "< 이   하  ";
                    ssView_Sheet1.Cells[nRead, 6].Text = "공   란 >  ";
                }

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = "** 전체합계 **";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nTotChungAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = nTotIpgumAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nTotSakAmt.ToString("###,###,###,##0");   //삭감액
                if (nTotMirAmt != 0 && nTotSakAmt != 0)
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = (nTotSakAmt / nTotMirAmt * 100).ToString("##0.00") + "%";
                }

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                Cursor.Current = Cursors.Default;
                btnPrint.Enabled = true;
                btnSearch.Enabled = true;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void frmPmpaViewEtcIpgumPrint_Load(object sender, EventArgs e)
        {
          //  if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
          //  {
          //      this.Close(); //폼 권한 조회
          //      return;
          //  }
          //  ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            dtpFDate.Value = Convert.ToDateTime(VB.Left(strSysDate, 8) + 01);
            dtpTDate.Value = Convert.ToDateTime(strSysDate);

            cboClass.Items.Clear();
            cboClass.Items.Add("09.헌혈미수");
            cboClass.Items.Add("11.보훈청미수");
            cboClass.Items.Add("12.시각장애자");
            cboClass.Items.Add("13.심신장애진단비");
            cboClass.Items.Add("14.장애인보장구");
            cboClass.Items.Add("15.직원대납");
            cboClass.Items.Add("16.노인장기요양소견서");
            cboClass.Items.Add("17.방문간호지시서");
            cboClass.Items.Add("18.치매검사");
            cboClass.SelectedIndex = 0;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
            btnPrint.Enabled = false;
            ssView.Dock = DockStyle.Fill;

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            //{
            //    return;  //권한확인
            //}

            int i = 0;
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ssPrint_Sheet1.Cells[1, 0].Text = "입금기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + "=>" + dtpTDate.Value.ToString("yyyy-MM-dd");
            ssPrint_Sheet1.Cells[2, 0].Text = "거 래 처 : " + cboClass.Text;
            ssPrint_Sheet1.Cells[3, 0].Text = "입금확인 : 재무회계팀             (인)";

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                ssPrint_Sheet1.RowCount += 1;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 6].ColumnSpan = 2;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 8].ColumnSpan = 2;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = ssView_Sheet1.Cells[i, 0].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = ssView_Sheet1.Cells[i, 1].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].Text = ssView_Sheet1.Cells[i, 2].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].Text = ssView_Sheet1.Cells[i, 3].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 4].Text = ssView_Sheet1.Cells[i, 4].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].Text = ssView_Sheet1.Cells[i, 5].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 6].Text = ssView_Sheet1.Cells[i, 6].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 8].Text = ssView_Sheet1.Cells[i, 7].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 10].Text = ssView_Sheet1.Cells[i, 8].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 11].Text = ssView_Sheet1.Cells[i, 9].Text;
            }
            ssPrint_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            strTitle = "기 타 미 수  진 료 비  입 금 표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, false, false, false, false, 0.9f);

            CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);
        }
    }
}
