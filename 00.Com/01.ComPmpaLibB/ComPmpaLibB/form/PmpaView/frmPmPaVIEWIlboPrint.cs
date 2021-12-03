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
    /// File Name       : frmPmPaVIEWIlboPrint
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\misu\misumir.vbp\MISUM302.FRM(FrmIlboPrint) >> frmPmPaVIEWIlboPrint.cs 폼이름 재정의" />
    public partial class frmPmPaVIEWIlboPrint : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string GstrJobName = "";

        public frmPmPaVIEWIlboPrint(string strJobName)
        {
            GstrJobName = strJobName;

            InitializeComponent();
        }
        public frmPmPaVIEWIlboPrint()
        {
            InitializeComponent();
        }

        private void frmPmPaVIEWIlboPrint_Load(object sender, System.EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //this.Close();
            //return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpFdate.Value = Convert.ToDateTime(strDTP).AddDays(-2);
            dtpTdate.Value = Convert.ToDateTime(strDTP).AddDays(-1);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int j = 0;
            int nRow = 0;
            string strFDate = "";
            string strTDate = "";
            string strNewData = "";
            string strOldData = "";
            string strGubun = "";
            double nAmt = 0;
            double nTotIwolAmt = 0;
            double nTotMirAmt = 0;
            double nTotIpgumAmt = 0;
            double nTotJanAmt = 0;

            if (dtpTdate.Value < dtpFdate.Value)
            {
                ComFunc.MsgBox("종료일자가 시작일자 보다 작습니다", "확인");
                dtpTdate.Select();
                return;
            }

            strFDate = dtpFdate.Value.ToString();
            strTDate = dtpTdate.Value.ToString();

            nTotIwolAmt = 0;
            nTotMirAmt = 0;
            nTotIpgumAmt = 0;
            nTotJanAmt = 0;

            CmdOK_Slip_Display(strFDate, strTDate, ref strOldData, ref nRow, ref strNewData, ref strGubun, ref nAmt, ref nTotIpgumAmt);
        }

        private void CmdOK_Slip_Display(string strFDate, string strTDate, ref string strOldData, ref int nRow, ref string strNewData, ref string strGubun, ref double nAmt, ref double nTotIpgumAmt)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(a.Bdate,'YYYY-MM-DD') Bdate,a.Class,a.GelCode,a.IpdOpd,a.Gubun,    ";
                SQL = SQL + ComNum.VBLF + "        a.MisuID,a.Qty,a.Amt,a.Remark,b.Bun,TO_CHAR(b.Bdate,'yyyy-mm-dd') MirDate, ";
                SQL = SQL + ComNum.VBLF + "        B.MIRYYMM";
                SQL = SQL + ComNum.VBLF + "   FROM MISU_SLIP a,MISU_IDMST b                                                   ";
                SQL = SQL + ComNum.VBLF + "  WHERE a.Bdate >= TO_DATE('" + Convert.ToDateTime(strFDate).ToString("yyyy-MM-dd") + "','yyyy-mm-dd')                        ";
                SQL = SQL + ComNum.VBLF + "    AND a.Bdate <= TO_DATE('" + Convert.ToDateTime(strTDate).ToString("yyyy-MM-dd") + "','yyyy-mm-dd')                        ";

                if (rdoBi0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '01'                                                         ";
                }
                else if (rdoBi1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '02'                                                         ";
                }
                else if (rdoBi2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '03'                                                         ";
                }
                else if (rdoBi3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '04'                                                         ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class < '05'                                                         ";
                }

                SQL = SQL + ComNum.VBLF + "    AND a.Gubun > '09'  ";


                if (rdojob0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun < '20'                                                         ";
                }
                else if (rdojob1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun > '20'                                                         ";
                }
                else if (rdojob3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun IN ('21','22','23','24','25','26')                             ";
                }

                SQL = SQL + ComNum.VBLF + "    AND a.WRTNO = b.WRTNO(+)                                                       ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.Bdate,a.Class,a.GelCode,a.MisuID,a.Gubun                              ";

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

                strOldData = VB.Space(18);

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
                    strNewData = strNewData + dt.Rows[i]["Class"].ToString().Trim();
                    strNewData = strNewData + dt.Rows[i]["GelCode"].ToString().Trim();
                    strNewData = VB.Left(strNewData + VB.Space(20), 20);

                    if (VB.Left(strNewData, 12) != VB.Left(strOldData, 12))//      '일자,종류가 틀린경우
                    {
                        ssView_Sheet1.Cells[nRow - 1, 1 - 1].Text = VB.Left(strNewData, 10);
                        ssView_Sheet1.Cells[nRow - 1, 2 - 1].Text = CPM.READ_MisuClass(VB.Mid(strNewData, 11, 2));
                        ssView_Sheet1.Cells[nRow - 1, 3 - 1].Text = CPM.READ_BAS_MIA(VB.Right(strNewData, 8));
                        strOldData = strNewData;
                    }
                    else if (strNewData != strOldData)                        //'조합이 틀린경우
                    {
                        ssView_Sheet1.Cells[nRow - 1, 3 - 1].Text = CPM.READ_BAS_MIA(VB.Right(strNewData, 8));
                        strOldData = strNewData;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 4 - 1].Text = CPM.READ_MisuGye_TA(dt.Rows[i]["Gubun"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 5 - 1].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6 - 1].Text = CPM.READ_MisuIpdOpd(dt.Rows[i]["IpdOpd"].ToString().Trim());

                    if (dt.Rows[i]["MISUID"].ToString().Trim() == "04208274")
                    {
                        i = i;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 7 - 1].Text = dt.Rows[i]["MIRYYMM"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 8 - 1].Text = CPM.READ_MisuBunya(dt.Rows[i]["Bun"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 9 - 1].Text = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("###,###,##0 ");

                    strGubun = dt.Rows[i]["Gubun"].ToString().Trim();
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());

                    switch (strGubun)
                    {
                        case "11":
                        case "12":
                        case "13":
                        case "14":
                        case "15":
                            ssView_Sheet1.Cells[nRow - 1, 10 - 1].Text = nAmt.ToString("###,###,##0 ");
                            break;
                        default:
                            ssView_Sheet1.Cells[nRow - 1, 11 - 1].Text = nAmt.ToString("###,###,##0 ");
                            nTotIpgumAmt = nTotIpgumAmt + nAmt;
                            break;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 12 - 1].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = nRow + 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10 - 1].Text = "** 합계 **    ";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11 - 1].Text = nTotIpgumAmt.ToString("###,###,###,##0");

                ssView.ActiveSheet.ColumnHeader.Cells[0, 0, 0, ssView.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
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

            if (rdoBi0.Checked == true)
                strTitle = "(건강보험)";
            if (rdoBi1.Checked == true)
                strTitle = "(직장)";
            if (rdoBi2.Checked == true)
                strTitle = "(지역)";
            if (rdoBi3.Checked == true)
                strTitle = "(의료급여)";
            if (rdoBi4.Checked == true)
                strTitle = "(전체)";


            strTitle = "청 구  및  입 금  일 보";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.ssView)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column); //sort 정렬 기능 
                    return;
                }
                if (e.RowHeader == true)
                {
                    return;
                }
            }
        }
    }
}
