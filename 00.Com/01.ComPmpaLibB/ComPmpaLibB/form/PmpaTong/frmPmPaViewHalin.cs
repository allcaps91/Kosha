using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmPaViewHalin
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\magam\mlrepb\Mlrepb02.frm(FrmHalin.frm) >> frmPmPaViewHalin.cs 폼이름 재정의" />

    public partial class frmPmPaViewHalin : Form
    {

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmPaViewHalin()
        {
            InitializeComponent();
        }

        private void frmPmPaViewHalin_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            cboYYYYY.Items.Clear();
            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYYYY, 24, "", "0");
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int nRow = 0;
            int nAmt2 = 0;        //'건별 할인액
            int nTotAmt = 0;        //'할인 합계액
            int nTotCnt = 0;
            string strOldData = "";
            string strNewData = "";
            string strSuNext = "";
            string strFdate = "";
            string strEdate = "";

            strFdate = cboYYYYY.Text + "-01";
            strEdate = CF.READ_LASTDAY(clsDB.DbCon, strFdate);

            if (rdo0.Checked == true)
            {
                CmdView_Opd_Select(strFdate, strEdate, ref nRow, ref strOldData, ref nTotCnt, ref nTotAmt, ref strSuNext, ref nAmt2, ref strNewData);
            }
            if (rdo1.Checked == true)
            {
                CmdView_Ipd_Select(strFdate, strEdate, ref nRow, ref nTotCnt, ref strOldData, ref nTotAmt, ref strSuNext, ref nAmt2, ref strNewData);
            }
        }

        private void CmdView_Opd_Select(string strFdate, string strEdate, ref int nRow, ref string strOldData, ref int nTotCnt, ref int nTotAmt, ref string strSuNext, ref int nAmt2, ref string strNewData)   //외래 감액 SELECT
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SuNext,SUM(Amt1+Amt2) HAmt ";
                SQL = SQL + ComNum.VBLF + " FROM OPD_SLIP ";
                SQL = SQL + ComNum.VBLF + "WHERE ACTDate   >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ACTDate   <= TO_DATE('" + strEdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND Bun      = '92' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY SuNext ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SuNext ";

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

                nRow = 0;
                strOldData = VB.Space(16);
                nTotCnt = 0;
                nTotAmt = 0;

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strSuNext = dt.Rows[i]["SuNext"].ToString().Trim();
                    nAmt2 = Convert.ToInt32(dt.Rows[i]["HAmt"].ToString().Trim()); //할인액

                    if (nAmt2 != 0)
                    {
                        nRow = nRow + 1;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }
                        strNewData = VB.Left(strSuNext + VB.Space(8), 8);
                        //'할인계정이 변경되었으면
                        if (VB.Left(strNewData, 8) != VB.Left(strOldData, 8))
                        {
                            CmdView_Halin_Name(nRow, strSuNext);
                        }
                        //'할인계정,등록번호가 변경되었으면

                        if (strNewData != strOldData)
                        {
                            strOldData = strNewData;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = nAmt2.ToString("###,###,###,##0");
                        // ' 합계
                        nTotAmt = nTotAmt + nAmt2;
                    }
                }

                nRow = nRow + 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.Cells[nRow - 1, 1].Text = " ** 전체합계 **";
                ssView_Sheet1.Cells[nRow - 1, 2].Text = nTotAmt.ToString("###,###,###,##0");

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
        private void CmdView_Ipd_Select(string strFdate, string strEdate, ref int nRow, ref int nTotCnt, ref string strOldData, ref int nTotAmt, ref string strSuNext, ref int nAmt2, ref string strNewData)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //'자료를 SELECT
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SuNext,SUM(Amt) HAmt ";
                SQL = SQL + ComNum.VBLF + " FROM IPD_NEW_CASH ";
                SQL = SQL + ComNum.VBLF + "WHERE ActDate >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ActDate <= TO_DATE('" + strEdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND Bun      = '92' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY SuNext ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SuNext ";

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

                nRow = 0;
                strOldData = VB.Space(16);
                nTotCnt = 0;
                nTotAmt = 0;

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strSuNext = dt.Rows[i]["SuNext"].ToString().Trim();
                    nAmt2 = Convert.ToInt32(dt.Rows[i]["HAmt"].ToString().Trim()); //'할인액

                    if (nAmt2 != 0)
                    {
                        nRow = nRow + 1;
                        if (nRow != ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }


                        //strNewData = Left (strSuNext & Space (8) , 8) & strPano

                        if (VB.Left(strNewData, 8) != VB.Left(strOldData, 8))
                        {
                            CmdView_Halin_Name(nRow, strSuNext);
                        }
                        //'할인계정,등록번호가 변경되었으면

                        if (strNewData != strOldData)
                        {
                            strOldData = strNewData;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = nAmt2.ToString("###,###,###,##0");
                        //'소계, 합계
                        nTotAmt = nTotAmt + nAmt2;
                    }
                }

                nRow = nRow + 1;
                ssView_Sheet1.RowCount = nRow;

                ssView_Sheet1.Cells[nRow - 1, 1].Text = " ** 전체합계 **";
                ssView_Sheet1.Cells[nRow - 1, 2].Text = nTotAmt.ToString("###,###,###,##0");
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

        private void CmdView_Halin_Name(int nRow, string strSuNext)//할인계정 명칭을 READ
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = SQL + ComNum.VBLF + "SELECT SuNameK ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                SQL = SQL + ComNum.VBLF + "WHERE SuNext='" + strSuNext + "' ";

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

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 0].Text = " " + dt.Rows[0]["SuNameK"].ToString().Trim();
                }
                else
                {
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = " " + strSuNext + " " + "구분오류";
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

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) { return; }     //권한확인

            strTitle = "** 월별 할인계정별 상세내역 **";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String(VB.Left(cboYYYYY.Text, 4) + "년 " + VB.Right(cboYYYYY.Text, 2) + "월", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            if (rdo0.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("외래(O),입원", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            if (rdo1.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("외래,입원(O)", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("인쇄일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }
    }
}
