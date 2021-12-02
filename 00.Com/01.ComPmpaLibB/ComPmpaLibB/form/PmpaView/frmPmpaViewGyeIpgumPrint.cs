using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewGyeIpgumPrint.cs
    /// Description     : 계약처 진료비 입금표
    /// Author          : 박창욱
    /// Create Date     : 2017-10-16
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUR206.FRM(FrmIpgumPrint.frm) >> frmPmpaViewGyeIpgumPrint.cs 폼이름 재정의" />	
    public partial class frmPmpaViewGyeIpgumPrint : Form
    {
        clsPmpaMisu cpm = new clsPmpaMisu();
        clsPmpaFunc cpf = new clsPmpaFunc();

        string[] GstrGels = new string[401];

        public frmPmpaViewGyeIpgumPrint()
        {
            InitializeComponent();
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

            int i = 0;
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            ssPrint_Sheet1.RowCount = 6;
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            ssPrint_Sheet1.Cells[1, 0].Text = "입금기간 : " + dtpFDate.Value + "=>" + dtpTDate.Value;
            ssPrint_Sheet1.Cells[2, 0].Text = "거 래 처 : " + cboGel.Text.Trim();
            ssPrint_Sheet1.Cells[3, 0].Text = "입금확인 : 재무회계팀     (인)";
            FarPoint.Win.ComplexBorder BorderBottom = new FarPoint.Win.ComplexBorder(
              new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
              new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
              new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
              new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
              new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), false, false);

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                ssPrint_Sheet1.RowCount += 1;
                //ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 7].ColumnSpan = 2;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = ssView_Sheet1.Cells[i, 0].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = ssView_Sheet1.Cells[i, 1].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].Text = ssView_Sheet1.Cells[i, 2].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].Text = ssView_Sheet1.Cells[i, 3].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 4].Text = ssView_Sheet1.Cells[i, 4].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].Text = ssView_Sheet1.Cells[i, 5].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 6].Text = ssView_Sheet1.Cells[i, 6].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 7].Text = ssView_Sheet1.Cells[i, 7].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 9].Text = ssView_Sheet1.Cells[i, 8].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 10].Text = ssView_Sheet1.Cells[i, 9].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0, ssPrint_Sheet1.RowCount - 1, ssPrint_Sheet1.ColumnCount - 1].Border = BorderBottom;
                //ssPrint_Sheet1.CellSpan
                    CS.CellSpan(ssPrint, ssPrint_Sheet1.RowCount - 1, 7, 1, 2);
            }
            ssPrint_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            strTitle = "계약처 진료비 입금표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, false, false, false, false);

            CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);
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
            double nWRTNO = 0;
            double nJanAmt = 0;
            double nIpgumAmt = 0;
            double nTotMirAmt = 0;
            double nTotSakAmt = 0;
            double nTotIpgumAmt = 0;
            double nTotChungAmt = 0;
            string strFDate = "";
            string strTDate = "";

            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");

            if (dtpTDate.Value < dtpFDate.Value)
            {
                ComFunc.MsgBox("종료일자가 시작일자보다 작음");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            btnPrint.Enabled = false;
            btnSearch.Enabled = false;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 20;

            try
            {
                //해당기간의 입금내역을 Read
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT WRTNO, TO_CHAR(BDate,'YY-MM-DD') vBDate, MisuID vMisuID,";
                SQL = SQL + ComNum.VBLF + "        SUM(Amt) vIAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND Class  = '08'"; //계약처
                SQL = SQL + ComNum.VBLF + "    AND Gubun  >= '21'";
                SQL = SQL + ComNum.VBLF + "    AND Gubun  <= '29'";
                if (cboGel.Text.Trim() != "*.전체")
                {
                    SQL = SQL + ComNum.VBLF + "    AND GelCode = '" + GstrGels[cboGel.SelectedIndex].Trim() + "'";
                }
                SQL = SQL + ComNum.VBLF + "  GROUP BY WRTNO, BDate, MisuID";
                SQL = SQL + ComNum.VBLF + "  ORDER BY BDate, MisuID";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }
                btnPrint.Enabled = true;

                nREAD = dt.Rows.Count;

                nTotMirAmt = 0;
                nTotSakAmt = 0;
                nTotIpgumAmt = 0;
                nTotChungAmt = 0;

                if (nREAD > 19)
                {
                    ssView_Sheet1.RowCount = nREAD + 1;
                }

                for (i = 0; i < nREAD; i++)
                {
                    nWRTNO = VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim());

                    //미수 MATSTER를 READ
                    cpm.READ_MISU_IDMST((long)nWRTNO);
                    nJanAmt = clsPmpaType.TMM.Amt[2];   //청구액
                    nIpgumAmt = VB.Val(dt.Rows[i]["vIAmt"].ToString().Trim());


                    //전월의 미수잔액을 READ
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT SUM(Amt) IpgumAmt";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND WRTNO = '" + nWRTNO + "'";
                    SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND Gubun >= '21'";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        btnSearch.Enabled = true;
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

                    ssView_Sheet1.Cells[i, 0].Text = (i + 1).ToString();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["vBdate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["vMisuID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = cpf.Get_BasPatient(clsDB.DbCon, clsPmpaType.TMM.MisuID.Trim()).Rows[0]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = VB.Right(clsPmpaType.TMM.FromDate, 8) + "-" + VB.Right(clsPmpaType.TMM.ToDate, 8);
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

                if (nREAD < 18)
                {
                    ssView_Sheet1.Cells[nREAD, 5].Text = "< 이   하  ";
                    ssView_Sheet1.Cells[nREAD, 6].Text = "공   란 >  ";
                }

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = "** 전체합계 **";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nTotChungAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = nTotIpgumAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nTotSakAmt.ToString("###,###,###,##0");   //삭감액
                if (nTotMirAmt != 0 && nTotSakAmt != 0)
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = (nTotSakAmt / nTotMirAmt * 100).ToString("##0.00") + "%";
                }

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void frmPmpaViewGyeIpgumPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView.Dock = DockStyle.Fill;

            try
            {
                SQL = "";
                SQL = SQL + " SELECT MiaCode GelCode, MiaName                   ";
                SQL = SQL + "   FROM BAS_MIA                                    ";
                SQL = SQL + "  WHERE MiaCode >= 'H001' AND MiaCode <= 'H999'    ";
                SQL = SQL + "  ORDER BY MiaCode                                 ";

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

                cboGel.Items.Clear();
                cboGel.Items.Add("*.전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboGel.Items.Add(dt.Rows[i]["MiaName"].ToString().Trim());
                    GstrGels[i + 1] = dt.Rows[i]["GelCode"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                cboGel.SelectedIndex = 0;

                dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                dtpFDate.Value = Convert.ToDateTime(VB.Left(dtpTDate.Value.ToString("yyyy-MM-dd"), 8) + "01");

                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = 20;
                btnPrint.Enabled = false;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
