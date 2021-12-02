using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmPaVIEWJicwonmisu : Form
    {
        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : frmPmPaVIEWJicwonmisu.cs
        /// Description     : 직원 미수금
        /// Author          : 김효성
        /// Create Date     : 2017-09-12
        /// Update History  : f
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "D:\psmh\misu\misuper.vbp(Frm직원미수금표.frm) >> frmPmPaVIEWJicwonmisu.cs 폼이름 재정의" />	
        /// 
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmPaVIEWJicwonmisu()
        {

            InitializeComponent();
        }

        private void frmPmPaVIEWJicwonmisu_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth (clsDB.DbCon, this) == false)
            //{ this.Close (); return; } //폼 권한 조회
            //ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            dtpFdate.Text = VB.Left(strDTP, 8) + "01";
            dtpTdate.Text = strDTP;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            int i = 0;
            int j = 0;
            string strTdate = "";
            string strFdate = "";
            string strBuse_New = "";
            string strBuse_Old = "";
            int nSubSum = 0;
            int nTotal = 0;
            int nJSubSum = 0;
            int nJTotal = 0;
            DataTable dt = null;
            DataTable dtFn = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            btnExit.Enabled = true;
            btnPrint.Enabled = true;
            btnView.Enabled = false;

            strFdate = dtpFdate.Text;
            strTdate = dtpTdate.Text;

            FarPoint.Win.ComplexBorder BorderBottom = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), false, false);


            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT   A.SABUN, C.KORNAME, SUBSTR(C.BUSE,1,4) BUSE, BUSE BUSECODE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.BDATE, 'YYYY-MM-DD') BDATE, A.AMT, A.PANO               ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP A, " + ComNum.DB_PMPA + "BAS_BUSE B, KOSMOS_ADM.INSA_MST C           ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1         ";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE >= TO_DATE('" + strFdate + "','YYYY-MM-DD')         ";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + strTdate + "','YYYY-MM-DD')         ";
                SQL = SQL + ComNum.VBLF + "   AND A.SABUN = C.SABUN                                           ";
                SQL = SQL + ComNum.VBLF + "   AND C.BUSE = B.BUCODE                                           ";

                if (rdoSabun.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY BUSE, SABUN, KORNAME                           ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY BUSE, KORNAME, SABUN                           ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    btnView.Enabled = true;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnView.Enabled = true;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                strBuse_New = "";
                strBuse_Old = "";
                nSubSum = 0;
                nTotal = 0;
                nJSubSum = 0;
                nJTotal = 0;

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.RowCount = 1;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBuse_New = dt.Rows[i]["BUSE"].ToString().Trim();
                    if (ssView_Sheet1.RowCount == 1)
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT NAME ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
                        SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                        SQL = SQL + ComNum.VBLF + "   AND BUCODE = '" + strBuse_New + "00' ";

                        SqlErr = clsDB.GetDataTable(ref dtFn, SQL, clsDB.DbCon);

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dtFn.Rows[0]["NAME"].ToString().Trim();

                        dtFn.Dispose();
                        dtFn = null;

                        strBuse_Old = strBuse_New;
                    }
                    else if (strBuse_New != strBuse_Old)
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "소   계";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = nSubSum.ToString("###,###,###,### ");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nJSubSum.ToString("###,###,###,### ");
                        nSubSum = 0;
                        nJSubSum = 0;

                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT NAME ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
                        SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                        SQL = SQL + ComNum.VBLF + "   AND BUCODE = '" + strBuse_New + "00' ";

                        SqlErr = clsDB.GetDataTable(ref dtFn, SQL, clsDB.DbCon);

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dtFn.Rows[0]["NAME"].ToString().Trim();

                        dtFn.Dispose();
                        dtFn = null;
                        strBuse_Old = strBuse_New;
                    }
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = VB.Val(dt.Rows[i]["AMT"].ToString().Trim()).ToString("###,###,###,### ");

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT JAMT ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_GAINMST ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "   AND PANO  = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";

                    SqlErr = clsDB.GetDataTable(ref dtFn, SQL, clsDB.DbCon);

                    if (dtFn.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = VB.Val(dtFn.Rows[0]["JAMT"].ToString().Trim()).ToString("###,###,###,### ");
                    }

                    nTotal = Convert.ToInt32(nTotal + VB.Val(dt.Rows[i]["AMT"].ToString().Trim()));
                    nSubSum = Convert.ToInt32(nSubSum + VB.Val(dt.Rows[i]["AMT"].ToString().Trim()));

                    for (j = 0; j < dtFn.Rows.Count; j++)
                    {
                        nJTotal = Convert.ToInt32(nJTotal + VB.Val(dtFn.Rows[j]["JAMT"].ToString().Trim()));
                        nJSubSum = Convert.ToInt32(nJSubSum + VB.Val(dtFn.Rows[j]["JAMT"].ToString().Trim()));
                    }
                    dtFn.Dispose();
                    dtFn = null;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Border = BorderBottom;
                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "소   계";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = nSubSum.ToString("###,###,###,### ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nJSubSum.ToString("###,###,###,### ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Border = BorderBottom;
                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "총   계";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = nTotal.ToString("###,###,###,### ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nJTotal.ToString("###,###,###,### ");

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Border = BorderBottom;
                Cursor.Current = Cursors.Default;
                btnView.Enabled = true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                btnView.Enabled = true;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            Cursor.Current = Cursors.WaitCursor;

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string sFont3 = "";
            string sFoot = "";

            DateTime mdtp;
            mdtp = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            strFont1 = "/c/fn\"굴림체\" /fz\"20\"  /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont1 = "/fn\"굴림\" /fz\"18\" /fb1 /fi0 /fu0 /fk0 /fs1";

            strHead1 = "/c/f1" + "직원 미수 현황" + "/f1/n";   //제목 센터
            strHead2 = "/l/f2" + "작업년월 : " + dtpFdate.Text + " ~" + dtpTdate.Text + "/f2";
            strHead2 = strHead2 + VB.Space(10) + "인쇄일자 : " + mdtp + "/n";

            btnPrint.Enabled = false;


            ssPrint_Sheet1.Cells[2, 0].Text = "작업년월 : " + dtpFdate.Text + " ~" + dtpTdate.Text;
            ssPrint_Sheet1.Cells[3, 0].Text = "인쇄일자 : " + mdtp;

            for (int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                ssPrint_Sheet1.RowCount += 1;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].ColumnSpan = 2;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].ColumnSpan = 2;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 7].ColumnSpan = 2;

                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = ssView_Sheet1.Cells[i, 0].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = ssView_Sheet1.Cells[i, 1].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].Text = ssView_Sheet1.Cells[i, 2].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].Text = ssView_Sheet1.Cells[i, 3].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].Text = ssView_Sheet1.Cells[i, 4].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 7].Text = ssView_Sheet1.Cells[i, 5].Text;
            }
            ssPrint_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);


            ssPrint_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssPrint_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssPrint_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssPrint_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n";
            ssPrint_Sheet1.PrintInfo.Footer = sFont3 + sFoot;
            ssPrint_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPrint_Sheet1.PrintInfo.Margin.Top = 50;
            ssPrint_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssPrint_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssPrint_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssPrint_Sheet1.PrintInfo.ShowBorder = false;
            ssPrint_Sheet1.PrintInfo.ShowColor = false;
            ssPrint_Sheet1.PrintInfo.ShowGrid = false;
            ssPrint_Sheet1.PrintInfo.ShowShadows = true;
            ssPrint_Sheet1.PrintInfo.UseMax = true;
            ssPrint_Sheet1.PrintInfo.PrintType = 0;
            ssPrint.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;

        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CmdCancel_Click()
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;

            btnView.Enabled = true;
            btnPrint.Enabled = false;
            btnExit.Enabled = false;
        }
    }
}
