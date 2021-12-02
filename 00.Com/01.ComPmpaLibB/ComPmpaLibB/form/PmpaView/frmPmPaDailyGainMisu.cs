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
    /// File Name       : frmPmPaDailyGainMisu
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\misu\misubs\misubs.vbp\misubs07.frm (FrmDailyGainMisu.frm)" >> frmPmPaVIEWBansongPrint.cs 폼이름 재정의" />
    /// 
    public partial class frmPmPaDailyGainMisu : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmPaDailyGainMisu()
        {
            InitializeComponent();
        }

        private void frmPmPaDailyGainMisu_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpFDate.Value = Convert.ToDateTime(CPM.DATE_ADD(clsDB.DbCon, strDTP, -1));
            dtpTDate.Value = Convert.ToDateTime(CPM.DATE_ADD(clsDB.DbCon, strDTP, -1));

            btnPrint.Enabled = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int j = 0;
            int nRow = 0;
            double nAmt1 = 0;
            double nAmt2 = 0;
            double nTotAmt1 = 0;
            double nTotAmt2 = 0;
            string strOK = "";
            string strFDate = "";
            string strTdate = "";
            DataTable dt = null;
            DataTable dtFn = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strTdate = dtpTDate.Value.ToString("yyyy-MM-dd");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //'자료를 SELECT
                SQL = "";
                SQL = "SELECT a.Pano,b.SName,TO_CHAR(a.ActDate,'YYYY-MM-DD') ActDate,";
                SQL = SQL + ComNum.VBLF + " SUM(a.MisuAmt) MisuAmt ";
                SQL = SQL + ComNum.VBLF + " FROM MISU_BalEtcMisu a,BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + "WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "  AND a.ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.ActDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.MisuGye='Y96' ";
                SQL = SQL + ComNum.VBLF + "  AND a.Pano=b.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "GROUP BY a.Pano,b.Sname,a.ActDate ";
                SQL = SQL + ComNum.VBLF + "ORDER BY  a.Pano,b.Sname,a.ActDate ";

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

                nTotAmt1 = 0;
                nTotAmt2 = 0;

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    //'개인미수의 발생금액을 READ
                    SQL = "";
                    SQL = "SELECT SUM(Amt) Amt FROM MISU_GAINSLIP ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "  AND Pano='" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND BDate=TO_DATE('" + dt.Rows[i]["ActDate"].ToString().Trim() + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND Gubun1='1' ";

                    SqlErr = clsDB.GetDataTable(ref dtFn, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        nAmt2 = (long)VB.Val(dtFn.Rows[0]["Amt"].ToString().Trim());

                    }
                    dtFn.Dispose();
                    dtFn = null;
                    nAmt1 = (long)VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());

                    if (nAmt1 != 0 || nAmt2 != 0)
                    {
                        strOK = "OK";

                        if (chkView.Checked == true)
                        {
                            if (nAmt1 == nAmt2)
                            {
                                strOK = "NO";
                            }
                        }

                        if (strOK == "OK")
                        {
                            nRow = nRow + 1;
                            if (nRow > ssView_Sheet1.RowCount)
                            {
                                ssView_Sheet1.RowCount = nRow;
                            }

                            ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 3].Text = nAmt1.ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = nAmt2.ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = (nAmt2 - nAmt1).ToString("###,###,###,##0 ");
                 

                            nTotAmt1 = nTotAmt1 + nAmt1;
                            nTotAmt2 = nTotAmt2 + nAmt2;
                        }
                    }
                }

                //'합계를 인쇄
                nRow = nRow + 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 합계 **";
                ssView_Sheet1.Cells[nRow - 1, 3].Text = nTotAmt1.ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 4].Text = nTotAmt2.ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 5].Text = (nTotAmt2 - nTotAmt1).ToString("###,###,###,##0 ");


                dt.Dispose();
                dt = null;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                btnPrint.Enabled = true;
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

            strTitle = "일자별 개인미수 발생 점검표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업기간: " + dtpFDate.Value.ToString("yyyy-MM-dd") + "일부터 " + dtpTDate.Value.ToString("yyyy-MM-dd") + "일까지", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("인쇄일자: " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

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
