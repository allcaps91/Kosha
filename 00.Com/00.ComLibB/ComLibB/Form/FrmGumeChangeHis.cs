using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComMedLibB
    /// File Name       : FrmOcsMsg.cs
    /// Description     : 물류OCS 물품코드 변경 History
    /// Author          : 최익준
    /// Create Date     : 2017-11-10
    /// Update History  : 
    /// <seealso>
    /// PSMH\basic\busuga\BuSuga50.frm
    /// </seealso>
    /// <history>  
    /// 
    /// </history>
    /// </summary>
    /// 
    public partial class FrmGumeChangeHis : Form
    {
        public FrmGumeChangeHis()
        {
            InitializeComponent();
        }

        private void FrmGumeChangeHis_Load(object sender, EventArgs e)
        {

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            dtpFDate.Value = Convert.ToDateTime(dtpTDate.Value).AddDays(-10);

            //작업구분
            cboJob.Items.Clear();
            cboJob.Items.Add("*.전체");
            cboJob.Items.Add("1.신규등록");
            cboJob.Items.Add("2.수정작업");
            cboJob.Items.Add("3.코드삭제");
            cboJob.SelectedIndex = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtpFDate_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                dtpTDate.Focus();
            }
        }

        private void dtpTDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cboJob.Focus();
            }
        }

        private void cboJob_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            int nREAD = 0;
            int nRow = 0;
            string strJob = "";

            string strSuCode = "";
            string strBCode = "";
            string strGbSunap = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                ssView_Sheet1.RowCount = 30;

                ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

                SQL = "";
                SQL = "SELECT TO_CHAR(JobDate,'YYYYMMDD HH24:MI') JobDate,JobSabun,JobGbn,JepCode,JepName,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,";
                SQL = SQL + ComNum.VBLF + " GbSunap,GbJinUse,SuCode,BCode,BGesu ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "ORD_JEP_HIS ";
                SQL = SQL + ComNum.VBLF + "WHERE JobDate>=TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND JobDate<=TO_DATE('" + dtpTDate.Text + " 23:59','YYYY-MM-DD HH24:MI') ";

                switch (strJob)
                {
                    case "1":
                        SQL = SQL + ComNum.VBLF + " AND JobGbn='1' ";       //신규등록
                        break;

                    case "2":
                        SQL = SQL + ComNum.VBLF + " AND JobGbn IN ('2','3') ";      //변경전, 변경후
                        break;

                    case "3":
                        SQL = SQL + ComNum.VBLF + " AND JobGbn='4' ";       //삭제
                        break;
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY JobDate,JobGbn,JepCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                nREAD = dt.Rows.Count;
                ssView_Sheet1.RowCount = dt.Rows.Count;
                nRow = 0;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strSuCode = dt.Rows[i]["SuCode"].ToString().Trim();
                    strBCode = dt.Rows[i]["BCode"].ToString().Trim();
                    strGbSunap = dt.Rows[i]["GbSunap"].ToString().Trim();

                    nRow = nRow + 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = dt.Rows.Count;
                    }

                    ssView_Sheet1.Cells[nRow - 1 , 0].Text = dt.Rows[i]["JobDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1 , 1].Text = clsVbfunc.GetPassName(clsDB.DbCon, dt.Rows[i]["JobSabun"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1 , 2].Text = dt.Rows[i]["JobGbn"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1 , 3].Text = dt.Rows[i]["JepCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1 , 4].Text = " " + dt.Rows[i]["JepName"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1 , 5].Text = dt.Rows[i]["GbSunap"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1 , 6].Text = dt.Rows[i]["GbJinUse"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1 , 7].Text = dt.Rows[i]["DelDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1 , 8].Text = dt.Rows[i]["BCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1 , 9].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1 , 10].Text = VB.Format(VB.Val(dt.Rows[i]["BGesu"].ToString().Trim()), "###0.0000");

                }

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                dt.Dispose();
                dt = null;
                ssView_Sheet1.RowCount = nRow;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fb1 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "물품OCS 물품코드 변경 History" + "/f1/n";
            strHead2 = "/l/f2" + "작업기간 : " + dtpFDate.Text + " ~ " + dtpTDate.Text;
            strHead2 = strHead2 + VB.Space(10) + "인쇄일자 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + " / "
                 + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":") + "/f2/n";

            //ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Margin.Top = 20;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);

            Cursor.Current = Cursors.Default;
        }
    }
}
