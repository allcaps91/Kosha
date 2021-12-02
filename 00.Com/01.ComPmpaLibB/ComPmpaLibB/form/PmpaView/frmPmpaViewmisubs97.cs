using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewmisubs97.cs
    /// Description     : 청구개인별과 집계급액 차이 비교
    /// Author          : 김효성
    /// Create Date     : 2017-09-04
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "psmh\misu\misubs\misubs.vbp(misubs97.frm) >> frmPmpaViewmisubs97.cs 폼이름 재정의" />	
    /// 
    public partial class frmPmpaViewmisubs97 : Form
    {
        public frmPmpaViewmisubs97()
        {
            InitializeComponent();
        }

        private void frmPmpaViewmisubs97_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "1");
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            int i = 0;
            int j = 0;
            string strYYMM = "";
            string strJong = "";
            int nRow = 0;
            string strPano = "";
            string strBi = "";
            int nCamt = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dtFunc = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);

                if (otpBI0.Checked == true)
                {
                    strJong = "1";
                }
                if (otpBI1.Checked == true)
                {
                    strJong = "5";
                }

                Cursor.Current = Cursors.WaitCursor;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT YYMM, Pano,Bi,SName, JOHAP, JepJAmt,Remark,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                SQL = SQL + ComNum.VBLF + "WHERE YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + " AND Gubun = '4' ";// '외래;

                if (strJong != "0")
                {
                    SQL = SQL + ComNum.VBLF + " AND SuBi = '" + strJong + "' ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY Bi,Pano";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 1)
                {
                    nRow = 0;
                    ssView_Sheet1.RowCount = 0;
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    progressBar1.Value = 0;
                    progressBar1.Maximum = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        progressBar1.Value = i + 1;
                        Cursor.Current = Cursors.WaitCursor;

                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strBi = dt.Rows[i]["BI"].ToString().Trim();
                        nCamt = Convert.ToInt32(dt.Rows[i]["JEPJAMT"].ToString().Trim());

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT SUM(EDIJAMT) + SUM(EDIBOAMT)  nSAMT ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MIR_INSID ";
                        SQL = SQL + ComNum.VBLF + " WHERE YYMM='" + strYYMM + "'";
                        SQL = SQL + ComNum.VBLF + "   AND IPDOPD='O'";
                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND JOHAP = '" + strJong + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND BI = '" + strBi + "'";
                        SQL = SQL + ComNum.VBLF + "   AND BOHOJONG ='0'";
                        SQL = SQL + ComNum.VBLF + "   AND EDIMIRNO > '0' ";

                        SQL = clsDB.GetDataTable(ref dtFunc, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dtFunc.Rows.Count == 0)
                        {

                            dtFunc.Dispose();
                            dtFunc = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return;

                        }

                        if (nCamt != VB.Val(dtFunc.Rows[0]["NSAMT"].ToString().Trim()))
                        {
                            nRow = nRow + 1;
                            ssView_Sheet1.RowCount = nRow;

                            ssView_Sheet1.Cells[nRow - 1, 0].Text = strPano;
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["BI"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 3].Text = nCamt.ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = VB.Val(dtFunc.Rows[0]["NSAMT"].ToString().Trim()).ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = (nCamt - VB.Val(dtFunc.Rows[0]["NSAMT"].ToString().Trim())).ToString("###,###,###,##0 ");
                            Application.DoEvents();

                        }
                        dtFunc.Dispose();
                        dtFunc = null;
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
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + " 외래 개인별과 집계표차이  LIST   " + "/f1/n";

            strHead2 = "/c/f2" + "작업 월 : " + cboYYMM.Text;
            strHead2 = strHead2 + VB.Space(10) + "인쇄일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A") + VB.Space(20);
            strHead2 = strHead2 + VB.Space(2) + "Page : /p";

            btnPrint.Enabled = false;

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2 + "/n";
            ssView_Sheet1.PrintInfo.Footer = sFont3 + sFoot;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
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
    }
}
