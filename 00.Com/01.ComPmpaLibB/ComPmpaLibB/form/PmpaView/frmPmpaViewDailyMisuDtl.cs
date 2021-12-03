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
    /// File Name       : frmPmpaViewDailyMisuDtl.cs
    /// Description     : 기타미수 계정별 상세내역
    /// Author          : 박창욱
    /// Create Date     : 2017-10-10
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs04.frm(FrmDailyMisuDtl.frm) >> frmPmpaViewDailyMisuDtl.cs 폼이름 재정의" />	
    public partial class frmPmpaViewDailyMisuDtl : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        public frmPmpaViewDailyMisuDtl()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "기타미수 계정별 상세내역";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업기간: " + dtpFDate.Value.ToString("yyyy-MM-dd") + "일부터 " + dtpTDate.Value.ToString("yyyy-MM-dd") + "일까지", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("인쇄일자: " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            int nRow = 0;
            double nSubAmt = 0;
            double nTotAmt = 0;
            string strFDate = "";
            string strTDate = "";
            string strOldData = "";
            string strNewData = "";

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;

            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");

            Cursor.Current = Cursors.Default;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.IpdOpd, a.MisuGye, c.SuNameK,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(a.ActDate,'YYYY-MM-DD') ActDate, a.Pano, b.SName,";
                SQL = SQL + ComNum.VBLF + "       a.Bi, a.DeptCode, a.MisuAmt";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BalEtcMisu a, " + ComNum.DB_PMPA + "BAS_PATIENT b, " + ComNum.DB_PMPA + "BAS_SUN c ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND a.ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND a.ActDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND a.Pano=b.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "   AND a.MisuGye=c.SuNext(+) ";
                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.IpdOpd,a.MisuGye, A.PANO, a.ActDate ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.IpdOpd,a.MisuGye, a.ActDate,A.PANO ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                nRow = 0;
                nSubAmt = 0;
                nTotAmt = 0;
                strOldData = "";

                for (i = 0; i < nRead; i++)
                {
                    strNewData = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    strNewData += dt.Rows[i]["MisuGye"].ToString().Trim();
                    strNewData += dt.Rows[i]["SuNameK"].ToString().Trim();

                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    if (strOldData != strNewData)
                    {
                        if (strOldData != "")
                        {
                            Display_SubTotal(ref nRow, ref nSubAmt);
                        }
                        strOldData = strNewData;
                        switch (dt.Rows[i]["IpdOpd"].ToString().Trim())
                        {
                            case "I":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "입원";
                                break;
                            default:
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "외래";
                                break;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["MisuGye"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                    }
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["SName"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim()).ToString("###,###,###,##0 ");

                    nSubAmt += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());
                    nTotAmt += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());
                }

                nRow += 1;
                if (nRow > ssView_Sheet1.RowCount)
                {
                    ssView_Sheet1.RowCount = nRow;
                }

                Display_SubTotal(ref nRow, ref nSubAmt);

                //합계를 Display
                ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 합계 **";
                ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt.ToString("###,###,###,##0 ");

                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                dt.Dispose();
                dt = null;

                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Display_SubTotal(ref int nRow, ref double nSubAmt)
        {
            ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 소계 **";
            ssView_Sheet1.Cells[nRow - 1, 8].Text = nSubAmt.ToString("###,###,###,##0 ");
            nSubAmt = 0;

            nRow += 1;
            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }
        }

        private void frmPmpaViewDailyMisuDtl_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-1);
            dtpFDate.Value = Convert.ToDateTime(VB.Left(dtpTDate.Value.ToString("yyyy-MM-dd"), 8) + "01");

            btnPrint.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
