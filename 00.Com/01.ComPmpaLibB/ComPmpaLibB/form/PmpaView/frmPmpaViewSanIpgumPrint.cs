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
    /// File Name       : frmPmpaViewSanIpgumPrint.cs
    /// Description     : 산재 진료비 입금표
    /// Author          : 박창욱
    /// Create Date     : 2017-09-12
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUS207.FRM(FrmIpgumPrint.frm) >> frmPmpaViewSanIpgumPrint.cs 폼이름 재정의" />	
    public partial class frmPmpaViewSanIpgumPrint : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsPmpaFunc cpf = new clsPmpaFunc();
        clsPmpaMisu cpm = new clsPmpaMisu();
        string[] GstrGels = new string[51];

        public frmPmpaViewSanIpgumPrint()
        {
            InitializeComponent();
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

            strTitle = "산 재 환 자  진 료 비  입 금 표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("입금기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + "=>" + dtpTDate.Value.ToString("yyyy-MM-dd") + "/n", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("거 래 처: " + ComFunc.LeftH(cboGel.Text + VB.Space(25), 25) + " /n", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("입금확인 : 재무회계팀             (인)" + VB.Space(2), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            int nRow = 0;
            long nWRTNO = 0;
            double nJanAmt = 0;
            double nIpgumAmt = 0;
            double nTotMirAmt = 0;
            double nTotSakAmt = 0;
            double nTotIpgumAmt = 0;
            double nTotChungAmt = 0;
            double nAllTotMirAmt = 0;
            double nAllTotSakAmt = 0;
            double nAllTotIpgumAmt = 0;
            double nAllTotChungAmt = 0;
            string strGelCode = "";
            string strGelCode_NEW = "";
            int nCount = 0;

            if (dtpTDate.Value < dtpFDate.Value)
            {
                ComFunc.MsgBox("종료일자가 시작일자보다 작음");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            btnPrint.Enabled = false;
            btnSearch.Enabled = false;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            try
            {
                //해당기간의 입금내역을 READ
                SQL = "";
                SQL += ComNum.VBLF + " SELECT GELCODE SGELCODE, WRTNO, TO_CHAR(BDate,'YY-MM-DD') vBDate,";
                SQL += ComNum.VBLF + "        MisuID vMisuID, SUM(Amt) vIAmt";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND Bdate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND Bdate <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND Class  = '05'";
                SQL += ComNum.VBLF + "    AND Gubun  >= '21'";
                SQL += ComNum.VBLF + "    AND Gubun  <= '29'";
                if (ComFunc.LeftH(cboGel.Text, 4) != "****")
                {
                    SQL += ComNum.VBLF + "  AND GelCode = '" + ComFunc.LeftH(cboGel.Text, 4) + "'";
                }
                SQL += ComNum.VBLF + "  GROUP BY GELCODE,WRTNO,BDate,MisuID";
                SQL += ComNum.VBLF + "  ORDER BY GELCODE,BDate,MisuID";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnPrint.Enabled = true;
                    btnSearch.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    btnPrint.Enabled = true;
                    btnSearch.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("입금된 내역이 1건도 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;

                //1건씩 READ 후 Sheet에 Display
                nRow = 0;
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
                    strGelCode_NEW = dt.Rows[i]["SGELCODE"].ToString().Trim();

                    if (strGelCode.Trim() != strGelCode_NEW.Trim())
                    {
                        if (i != 0)
                        {
                            nRow += 1;
                            if (nRow > ssView_Sheet1.RowCount)
                            {
                                ssView_Sheet1.RowCount = nRow + 1;
                            }
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = cpm.READ_BAS_MIA(strGelCode);
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = nTotChungAmt.ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 6].Text = nTotIpgumAmt.ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 7].Text = nTotSakAmt.ToString("###,###,###,##0");   //삭감액
                            if (nTotMirAmt != 0 && nTotSakAmt != 0)
                            {
                                ssView_Sheet1.Cells[nRow - 1, 8].Text = (nTotSakAmt / nTotChungAmt * 100).ToString("##0.00") + "%";
                            }
                            nCount = 0;
                            nTotMirAmt = 0;
                            nTotSakAmt = 0;
                            nTotIpgumAmt = 0;
                            nTotChungAmt = 0;
                        }
                        strGelCode = strGelCode_NEW;
                    }
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow + 1;
                    }

                    nCount += 1;

                    cpm.READ_MISU_IDMST(nWRTNO);
                    nJanAmt = clsPmpaType.TMM.Amt[2];   //청구금액
                    nIpgumAmt = VB.Val(dt.Rows[i]["vIAmt"].ToString().Trim());

                    //전월의 미수잔액을 Read
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SUM(Amt) IpgumAmt ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1";
                    SQL += ComNum.VBLF + "    AND WRTNO = '" + dt.Rows[i]["WRTNO"].ToString().Trim() + "'";
                    SQL += ComNum.VBLF + "    AND Bdate <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "    AND Gubun >= '21'";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnPrint.Enabled = true;
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

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = nCount.ToString();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["vBdate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["vMisuID"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = cpf.Get_BasPatient(clsDB.DbCon, clsPmpaType.TMM.MisuID.Trim()).Rows[0]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = ComFunc.RightH(clsPmpaType.TMM.FromDate, 10) + "~" + ComFunc.RightH(clsPmpaType.TMM.ToDate, 10);
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = clsPmpaType.TMM.Amt[2].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = nIpgumAmt.ToString("###,###,###,##0");
                    if (nJanAmt < 1)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = clsPmpaType.TMM.Amt[4].ToString("###,###,###,##0");  //삭감액
                        if (clsPmpaType.TMM.Amt[2] != 0 && clsPmpaType.TMM.Amt[4] != 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = (clsPmpaType.TMM.Amt[4] / clsPmpaType.TMM.Amt[2] * 100).ToString("##0.00") + "%";
                            nTotMirAmt += clsPmpaType.TMM.Amt[2];
                            nTotSakAmt += clsPmpaType.TMM.Amt[4];
                            nAllTotMirAmt += clsPmpaType.TMM.Amt[2];
                            nAllTotSakAmt += clsPmpaType.TMM.Amt[4];
                        }
                        else
                        {
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "0.00%";
                        }
                    }
                    dt1.Dispose();
                    dt1 = null;

                    nTotIpgumAmt += nIpgumAmt;
                    nAllTotIpgumAmt += nIpgumAmt;
                    nTotChungAmt += clsPmpaType.TMM.Amt[2];
                    nAllTotChungAmt += clsPmpaType.TMM.Amt[2];
                }
                nRow += 1;
                if (nRow > ssView_Sheet1.RowCount)
                {
                    ssView_Sheet1.RowCount = nRow + 1;
                }
                ssView_Sheet1.Cells[nRow - 1, 4].Text = cpm.READ_BAS_MIA(dt.Rows[nRead - 1]["SGELCODE"].ToString().Trim());
                ssView_Sheet1.Cells[nRow - 1, 5].Text = nTotChungAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 6].Text = nTotIpgumAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 7].Text = nTotSakAmt.ToString("###,###,###,##0");     //삭감액
                if (nTotMirAmt != 0 && nTotSakAmt != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = (nTotSakAmt / nTotChungAmt * 100).ToString("##0.00") + "%";
                }

                nRow += 1;
                if (nRow > ssView_Sheet1.RowCount)
                {
                    ssView_Sheet1.RowCount = nRow + 1;
                }
                ssView_Sheet1.Cells[nRow - 1, 4].Text = " ** 전체합계 **";
                ssView_Sheet1.Cells[nRow - 1, 5].Text = nAllTotChungAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 6].Text = nAllTotIpgumAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 7].Text = nAllTotSakAmt.ToString("###,###,###,##0");     //삭감액
                if (nAllTotMirAmt != 0 && nAllTotSakAmt != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = (nAllTotSakAmt / nAllTotChungAmt * 100).ToString("##0.00") + "%";
                }
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                Cursor.Current = Cursors.Default;

                dt.Dispose();
                dt = null;

                btnPrint.Enabled = true;
                btnSearch.Enabled = true;
            }
            catch (Exception ex)
            {
                btnPrint.Enabled = true;
                btnSearch.Enabled = true;
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewSanIpgumPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //this.Close(); //폼 권한 조회
            //return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            dtpFDate.Value = Convert.ToDateTime(ComFunc.LeftH(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), 8) + "01");
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT MiaCode GelCode, MiaName";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA";
                SQL += ComNum.VBLF + "  WHERE (MiaCode LIKE 'JK%' OR MiaCode LIKE 'KB%')";
                SQL += ComNum.VBLF + "  ORDER BY MiaCode";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                cboGel.Items.Clear();
                cboGel.Items.Add("****.전체");

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboGel.Items.Add(dt.Rows[i]["gelcode"].ToString().Trim() + "." + dt.Rows[i]["MiaName"].ToString().Trim());
                        GstrGels[i + 1] = dt.Rows[i]["gelcode"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                cboGel.SelectedIndex = 0;
                btnPrint.Enabled = true;
                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = 1;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssView_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
