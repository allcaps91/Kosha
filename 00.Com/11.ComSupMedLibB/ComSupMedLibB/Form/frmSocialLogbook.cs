using ComBase;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupMedLibB
{
    public partial class frmSocialLogbook : Form
    {
        public frmSocialLogbook()
        {
            InitializeComponent();
        }

        private void frmSocialLogbook_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); return;
            //}
            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");
            GetData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            //InitSsView();
            GetData();
        }

        private void InitSsView()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT GUBUN2, NAME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'SOCIAL_업무일지'";
                SQL = SQL + ComNum.VBLF + " ORDER BY SORT";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.RowCount++;
                    ssView_Sheet1.SetRowHeight(i, ComNum.SPDROWHT);
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GUBUN2"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                }

                ssView_Sheet1.SetColumnMerge(0, MergePolicy.Restricted);

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            Screen_Clear();

            try
            {
                SQL = "";
                SQL = "SELECT GUBUN, CNT, PTCNT, GCONTENT";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "SOCIAL_LOGBOOK";
                SQL = SQL + ComNum.VBLF + " WHERE JOBDATE = '" + dtpSdate.Value.ToString("yyyy-MM-dd") + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY GUBUN ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["CNT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PTCNT"].ToString().Trim();

                        if (i == 18)
                        {
                            ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["GCONTENT"].ToString().Trim();
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GCONTENT"].ToString().Trim();
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "SELECT GUBUN2, NAME, CODE";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'SOCIAL_업무일지'";
                SQL = SQL + ComNum.VBLF + " ORDER BY SORT";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["CODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.SetRowHeight(i - 1 , Convert.ToInt32(ssView_Sheet1.GetPreferredRowHeight(i -1) + 10));

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {                 
            //if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "DELETE FROM KOSMOS_ADM.SOCIAL_LOGBOOK";
                SQL = SQL + ComNum.VBLF + " WHERE JOBDATE = '" + dtpSdate.Value.ToString("yyyy-MM-dd") + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    SQL = "";
                    SQL = "INSERT INTO KOSMOS_ADM.SOCIAL_LOGBOOK";
                    SQL = SQL + ComNum.VBLF + "(";
                    SQL = SQL + ComNum.VBLF + "     JOBDATE, GUBUN, CNT, PTCNT, GCONTENT";
                    SQL = SQL + ComNum.VBLF + ")";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "(";
                    SQL = SQL + ComNum.VBLF + "     '" + dtpSdate.Value.ToString("yyyy-MM-dd") + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 5].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 2].Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 3].Text + "',";

                    if (i == 18)
                    {
                        SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 1].Text + "'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 4].Text + "'";
                    }

                    SQL = SQL + ComNum.VBLF + ")";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            strTitle = "";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("등록기간 : " + dtpSdate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
            CS = null;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Screen_Clear()
        {
            int i = 0;

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                ssView_Sheet1.Cells[i, 2].Text = "";
                ssView_Sheet1.Cells[i, 3].Text = "";
                ssView_Sheet1.Cells[i, 4].Text = "";
                ssView_Sheet1.Cells[i, 5].Text = "";

                if (i == 18)
                {
                    ssView_Sheet1.Cells[i, 1].Text = "";
                }
            }
        }

        private void ssView_EditModeOff(object sender, EventArgs e)
        {
            int i = 0;

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                ssView_Sheet1.SetRowHeight(i, Convert.ToInt32(ssView_Sheet1.GetPreferredRowHeight(i) + 10));
            }
        }
    }
}
