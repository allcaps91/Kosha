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
    /// File Name       : frmPmPaVIEWSpecialJewon.cs
    /// Description     : 재원자특수내용조회
    /// Author          : 김효성
    /// Create Date     : 2017-09-12
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\IPD\iument\iument.vbp\Frm재원자특수내용조회  >> frmPmPaVIEWSpecialJewon.cs 폼이름 재정의" />	
    /// 

    public partial class frmPmPaVIEWSpecialJewon : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmPaVIEWSpecialJewon()
        {
            InitializeComponent();
        }

        private void frmPmPaVIEWSpecialJewon_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            View();
        }

        private void View()
        {
            int i = 0;
            int j = 0;
            int nCount = 0;
            string nDate = "";
            string nTime = "";
            string strBi = "";
            string strSet1 = "";
            string strSet5 = "";
            string strSet6 = "";
            string strSet8 = "";
            string strSet9 = "";
            string strDate = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            ssView_Sheet1.RowCount = 0;

            nCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Pano,   Sname,  Bi,     RoomCode,  DeptCode,       ";
                SQL = SQL + ComNum.VBLF + "        AmSet1, AmSet5, AmSet6, AmSet8,    AmSet9,         ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(InDate,'YY-MM-DDHH24MI')   InDate,         ";
                SQL = SQL + ComNum.VBLF + "        GbSTS                                              ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER             ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                                ";
                SQL = SQL + ComNum.VBLF + "    AND ActDate IS NULL                                    ";
                SQL = SQL + ComNum.VBLF + "    AND (AmSet5 = '5' or AmSet6 = '*'  or AmSet8 = '1'     ";
                SQL = SQL + ComNum.VBLF + "     OR AmSet9 = '1'  OR AmSet1 = '2'  OR AmSet1 = '3')    ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Indate, RoomCode                                ";

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

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nCount = nCount + 1;

                    strBi = dt.Rows[i]["Bi"].ToString().Trim();
                    strDate = dt.Rows[i]["InDate"].ToString().Trim();
                    nDate = VB.Left(strDate, 8);
                    nTime = VB.Mid(strDate, 9, 2) + ":" + VB.Mid(strDate, 11, 2);
                    strSet1 = dt.Rows[i]["AmSet1"].ToString().Trim();
                    strSet5 = dt.Rows[i]["AmSet5"].ToString().Trim();
                    strSet6 = dt.Rows[i]["AmSet6"].ToString().Trim();
                    strSet8 = dt.Rows[i]["AmSet8"].ToString().Trim();
                    strSet9 = dt.Rows[i]["AmSet9"].ToString().Trim();

                    ssView_Sheet1.Cells[nCount - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[nCount - 1, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[nCount - 1, 2].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", strBi);
                    ssView_Sheet1.Cells[nCount - 1, 3].Text = nDate.ToString();
                    ssView_Sheet1.Cells[nCount - 1, 4].Text = nTime.ToString();
                    ssView_Sheet1.Cells[nCount - 1, 5].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nCount - 1, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nCount - 1, 7].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "IPD_입원상태", dt.Rows[i]["GbSTS"].ToString().Trim());

                    if (strSet5 == "5")
                    {
                        ssView_Sheet1.Cells[nCount - 1, 8].Text = "지병";
                    }

                    if (strSet5 == "5")
                    {
                        ssView_Sheet1.Cells[nCount - 1, 8].Text = "변환";
                    }

                    if (strSet5 == "5")
                    {
                        ssView_Sheet1.Cells[nCount - 1, 8].Text = "외국인";
                    }

                    if (strSet5 == "5")
                    {
                        ssView_Sheet1.Cells[nCount - 1, 8].Text = "예약";
                    }

                }
                dt.Dispose();
                dt = null;
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
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "재원자 특수 내용 명단";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 27, 0);
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            View();
        }
    }
}
