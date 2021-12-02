using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaSummary5STS.cs
    /// Description     : 청구 미수 발생 통계 (처방전 기준)
    /// Author          : 김효성
    /// Create Date     : 2017-08-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "psmh\misu\misumir.vbp(MISUM2406.FRM) >> frmPmpaSummary5STS.cs 폼이름 재정의" />	

    public partial class frmPmpaSummary5STS : Form
    {
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaSummary5STS()
        {
            InitializeComponent();
        }

        private void frmPmpaSummary5STS_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int nMM = 0;
            int i = 0;

            nYY = (int)VB.Val(VB.Left(strDTP, 4));
            nMM = (int)VB.Val(VB.Mid(strDTP, 6, 2));

            for (i = 1; i <= 15; i++)
            {
                cboYYYY.Items.Add(nYY.ToString("0000") + "년" + nMM.ToString("00") + "월분");
                nMM = nMM - 1;
                if (nMM == 0)
                {
                    nMM = 12;
                    nYY = nYY - 1;
                }
            }
            cboYYYY.SelectedIndex = 1;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strBi = "";
            int nRow = 0;
            string strYYMM = "";
            double nAmt = 0;
            double nOSum = 0;
            double nISum = 0;
            double[,] ndata = new double[3, 7];
            //string strLastDate = "";

            Cursor.Current = Cursors.WaitCursor;

            ComFunc cf = new ComFunc();

            strYYMM = VB.Left(cboYYYY.Text, 4) + VB.Mid(cboYYYY.Text, 6, 2);
            //strLastDate = cf . READ_LASTDAY (clsDB.DbCon,  VB . Left ( strYYMM , 4 ) + "-" + VB . Right ( strYYMM , 2 ) + "-01" );

            for (i = 0; i <= 6; i++)
            {
                ndata[1, i] = 0;
                ndata[2, i] = 0;
            }

            btnView.Enabled = false;
            btnPrint.Enabled = false;
            ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            //try
            //{
            //'외래청구액 SELECT
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT BI,SUM(AMT1+AMT2) SAMT                                 ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "TONG_INCOM                     ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1=1		                                    ";
            SQL = SQL + ComNum.VBLF + "  AND YYMMDD='" + strYYMM + "'                                 ";
            SQL = SQL + ComNum.VBLF + "    AND CLASS ='M'                                             ";
            SQL = SQL + ComNum.VBLF + "    AND IPDOPD='O'                                             ";
            SQL = SQL + ComNum.VBLF + "    AND BUN='98'                                               ";
            SQL = SQL + ComNum.VBLF + "    AND BI IN ( '11','12', '13','21', '22', '23','31','52')    ";
            SQL = SQL + ComNum.VBLF + "  GROUP BY BI                                                  ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY BI                                                  ";

            SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strBi = dt.Rows[i]["BI"].ToString().Trim();
                nAmt = VB.Val(dt.Rows[i]["SAMT"].ToString().Trim());
                nRow = SELECT_BI(strBi);
                ndata[1, nRow] = ndata[1, nRow] + nAmt;
            }
            dt.Dispose();
            dt = null;

            //'입원청구액 SELECT
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT BI,SUM(AMT1) SAMT                                      ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "TONG_INCOM                      ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                                    ";
            SQL = SQL + ComNum.VBLF + "    AND YYMMDD='" + strYYMM + "'                               ";
            SQL = SQL + ComNum.VBLF + "    AND CLASS ='M'                                             ";
            SQL = SQL + ComNum.VBLF + "    AND IPDOPD='I'                                             ";
            SQL = SQL + ComNum.VBLF + "    AND Gubun = '1'                                            ";
            SQL = SQL + ComNum.VBLF + "    AND Bun   < '85'                                           ";
            SQL = SQL + ComNum.VBLF + "    AND BI IN ( '11','12', '13','21', '22', '23','31','52')    ";
            SQL = SQL + ComNum.VBLF + "  GROUP BY BI                                                  ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY BI                                                  ";

            SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strBi = dt.Rows[i]["BI"].ToString().Trim();
                nAmt = VB.Val(dt.Rows[i]["SAMT"].ToString().Trim());
                nRow = SELECT_BI(strBi);

                if (strBi == "11" || strBi == "12" || strBi == "13" || strBi == "22")
                {
                    nAmt = nAmt * 0.8;
                }
                ndata[2, nRow] = ndata[2, nRow] + nAmt;
            }
            dt.Dispose();
            dt = null;

            //'display
            nOSum = 0;
            nISum = 0;
            //스프레드 출력문

            for (i = 1; i <= 5; i++)
            {
                ssView_Sheet1.Cells[i - 1, 1].Text = ndata[1, i].ToString("###,###,###,##0 ");
                nOSum = nOSum + ndata[1, i];

                ssView_Sheet1.Cells[i - 1, 2].Text = ndata[2, i].ToString("###,###,###,##0 ");
                nISum = nISum + ndata[2, i];

                ssView_Sheet1.Cells[i - 1, 3].Text = (ndata[1, i] + ndata[2, i]).ToString("###,###,###,##0 ");

            }

            //sum

            ssView_Sheet1.Cells[6 - 1, 1].Text = nOSum.ToString("##,###,###,##0 ");
            ssView_Sheet1.Cells[6 - 1, 2].Text = nISum.ToString("##,###,###,##0 ");
            ssView_Sheet1.Cells[6 - 1, 3].Text = (nOSum + nISum).ToString("##,###,###,##0 ");

            Cursor.Current = Cursors.Default;

            btnView.Enabled = true;
            btnPrint.Enabled = true;

            //}
            //   catch ( Exception ex )
            //   {
            //ComFunc . MsgBox ( ex . Message );
            //clsDB . SaveSqlErrLog ( ex . Message , SQL, clsDB.DbCon); //에러로그 저장
            //Cursor . Current = Cursors . Default;
            //   }
        }

        private int SELECT_BI(string strBi)
        {
            int nRow = 0;

            switch (strBi)
            {
                case "11":
                case "12":
                case "13":
                    nRow = 1;   //보험
                    break;
                case "21":
                case "22":
                case "23":
                    nRow = 2;   //보호
                    break;
                case "31":
                    nRow = 3;   //산재
                    break;
                case "52":
                    nRow = 4;   //자보
                    break;
                default:
                    nRow = 5;
                    break;
            }
            return nRow;

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";
            string sFont3 = "";
            string sFoot = "";

            DateTime mdtp;
            mdtp = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            strFont1 = "/c/fn\"굴림체\" /fz\"16\"  /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + cboYYYY.Text + " 청구미수 발생 통계" + "/f1/n";
            strHead2 = strHead2 + "/n/c" + "** 처방전 기준 **" + VB.Space(20) + "출력 시간 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A") + VB.Space(20) + "PAGE :" + "/p";

            btnPrint.Enabled = false;

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2 + "/n";
            ssView_Sheet1.PrintInfo.Footer = sFont3 + sFoot;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
