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
    /// File Name       : frmPmpaTongGamTot.cs
    /// Description     : 감액 집계표
    /// Author          : 박창욱
    /// Create Date     : 2017-10-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs26.frm(FrmGamTot.frm) >> frmPmpaTongGamTot.cs 폼이름 재정의" />	
    public partial class frmPmpaTongGamTot : Form
    {
        public frmPmpaTongGamTot()
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

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "감 액 집 계 표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업월:" + cboFYYMM.Text + " ~ " + cboTYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

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
            double nOpdAmt = 0;
            double nIpdAmt = 0;
            double nTotOCNT = 0;
            double nTotICNT = 0;
            double nTotOAmt = 0;
            double nTotIAmt = 0;
            string strFYYMM = "";
            string strTYYMM = "";

            strFYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Right(cboFYYMM.Text, 2);
            strTYYMM = VB.Left(cboTYYMM.Text, 4) + VB.Right(cboTYYMM.Text, 2);

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 20;

            Cursor.Current = Cursors.WaitCursor;
            ssView.Enabled = true;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.HalCode, b.SuNameK, SUM(a.OpdCNT) OpdCNT,";
                SQL = SQL + ComNum.VBLF + "       SUM(a.OpdAmt) OpdAmt, SUM(a.IpdCNT) IpdCNT, SUM(a.IpdAmt) IpdAmt ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "TONG_HALINDTL a,BAS_SUN b";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND a.YYMM>='" + strFYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND a.YYMM<='" + strTYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND a.HalCode=b.SuNext(+)";
                SQL = SQL + ComNum.VBLF + " GROUP BY a.HalCode,b.SuNameK";
                SQL = SQL + ComNum.VBLF + " ORDER BY a.HalCode,b.SuNameK";

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

                nRead = dt.Rows.Count;
                nRow = 0;

                for (i = 0; i < nRead; i++)
                {
                    nOpdAmt = VB.Val(dt.Rows[i]["OpdAmt"].ToString().Trim());
                    nIpdAmt = VB.Val(dt.Rows[i]["IpdAmt"].ToString().Trim());
                    if (nOpdAmt != 0 || nIpdAmt != 0)
                    {
                        nRow += 1;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["HalCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = " " + dt.Rows[i]["SuNameK"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["OpdCNT"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = nOpdAmt.ToString("###,###,###,##0 ");
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["IpdCNT"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = nIpdAmt.ToString("###,###,###,##0 ");
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = (nOpdAmt + nIpdAmt).ToString("###,###,###,##0 ");

                        //합계에 ADD
                        nTotOAmt += nOpdAmt;
                        nTotIAmt += nIpdAmt;
                        nTotOCNT += VB.Val(dt.Rows[i]["OpdCNT"].ToString().Trim());
                        nTotICNT += VB.Val(dt.Rows[i]["IpdCNT"].ToString().Trim());
                    }
                }
                dt.Dispose();
                dt = null;

                nRow += 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.Cells[nRow - 1, 1].Text = "** 합계 **";
                ssView_Sheet1.Cells[nRow - 1, 2].Text = nTotOCNT.ToString();
                ssView_Sheet1.Cells[nRow - 1, 3].Text = nTotOAmt.ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 4].Text = nTotICNT.ToString();
                ssView_Sheet1.Cells[nRow - 1, 5].Text = nTotIAmt.ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 6].Text = (nTotOAmt + nTotIAmt).ToString("###,###,###,##0 ");

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

        private void frmPmpaTongGamTot_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFYYMM, 60, "", "0");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTYYMM, 60, "", "0");
            cboFYYMM.SelectedIndex = 1;
            cboTYYMM.SelectedIndex = 1;
            btnPrint.Enabled = false;
            ssView.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
