using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupMedLibB
{
    public partial class frmSereRegister : Form
    {
        public frmSereRegister()
        {
            InitializeComponent();
        }

        private void frmSereRegister_Load(object sender, EventArgs e)
        {
            //string strSysdate = "";

            //strSysdate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            GetPage();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            GetData();
        }

        private void GetData()
        {
            int i = 0;
            string strSDATE = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strSDATE = dtpSDate.Value.Year.ToString();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT PANO, SERENO, SEREDATE, BONDANG, JURE, SNAME, SERENAME, BIRTHDATE, BUNAME, MONAME, BIGNAME, BIGO, TOTAL";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.WONMOK_LIST3 ";
                SQL = SQL + ComNum.VBLF + "  WHERE SUBSTR(SEREDATE, 0, 4) = '" + strSDATE + "'";
                SQL = SQL + ComNum.VBLF + "    AND PAGE = '" + cboPage.Text + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY TO_NUMBER(PANO)";

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
                        ssView_Sheet1.Cells[i + 4, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[i + 4, 2].Text = dt.Rows[i]["SERENO"].ToString().Trim();
                        ssView_Sheet1.Cells[i + 4, 3].Text = dt.Rows[i]["SEREDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i + 4, 4].Text = dt.Rows[i]["BONDANG"].ToString().Trim();
                        ssView_Sheet1.Cells[i + 4, 5].Text = dt.Rows[i]["JURE"].ToString().Trim();
                        ssView_Sheet1.Cells[i + 4, 6].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i + 4, 7].Text = dt.Rows[i]["SERENAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i + 4, 8].Text = dt.Rows[i]["BIRTHDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i + 4, 9].Text = dt.Rows[i]["BUNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i + 4, 10].Text = dt.Rows[i]["MONAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i + 4, 11].Text = dt.Rows[i]["BIGNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i + 4, 12].Text = dt.Rows[i]["BIGO"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
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
            SaveData();
        }

        private void SaveData()
        {
            createData();
            btnSearch.PerformClick();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            strTitle = "";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 10, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Both);
            CS = null;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

        private void DeleteData()
        {
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void createData()
        {
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strSDATE = "";

            strSDATE = dtpSDate.Value.Year.ToString();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "DELETE FROM KOSMOS_ADM.WONMOK_LIST3";
                SQL = SQL + ComNum.VBLF + "  WHERE SUBSTR(SEREDATE, 0, 4) = '" + strSDATE + "'";
                SQL = SQL + ComNum.VBLF + "    AND PAGE = '" + cboPage.Text + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (i = 4; i < 34; i++)
                {
                    if (ssView_Sheet1.Cells[i, 0].Text == "" && ssView_Sheet1.Cells[i, 1].Text != "")
                    {
                        SQL = "";
                        SQL = "INSERT INTO KOSMOS_ADM.WONMOK_LIST3";
                        SQL = SQL + ComNum.VBLF + "(";
                        SQL = SQL + ComNum.VBLF + "     WRITEDATE, WRITESABUN, PANO, SERENO, SEREDATE, BONDANG, JURE, SNAME, SERENAME, BIRTHDATE, BUNAME, MONAME, BIGNAME, BIGO, PAGE";
                        SQL = SQL + ComNum.VBLF + ")";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "(";
                        SQL = SQL + ComNum.VBLF + "     TO_CHAR(SYSDATE,'YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "     '" + clsType.User.Sabun + "',";
                        SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 1].Text + "',";
                        SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 2].Text + "',";
                        SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 3].Text + "',";
                        SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 4].Text + "',";
                        SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 5].Text + "',";
                        SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 6].Text + "',";
                        SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 7].Text + "',";
                        SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 8].Text + "',";
                        SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 9].Text + "',";
                        SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 10].Text + "',";
                        SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 11].Text + "',";
                        SQL = SQL + ComNum.VBLF + "     '" + ssView_Sheet1.Cells[i, 12].Text + "',";
                        SQL = SQL + ComNum.VBLF + "     '" + cboPage.Text + "'";
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
        private void Screen_Clear()
        {
            ssView_Sheet1.Cells[4, 0, 33, 12].Text = "";
            ssView_Sheet1.Cells[34, 7].Text = "";
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            strTitle = "";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
            CS = null;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.Rows.Add(4, 1);
        }

        private void GetPage()
        {
            int i = 0;
            string strSDATE = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT PAGE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.WONMOK_LIST3";
                SQL = SQL + ComNum.VBLF + " WHERE SUBSTR(SEREDATE, 0, 4) = '" + strSDATE + "'";
                SQL = SQL + ComNum.VBLF + " GROUP BY PAGE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                cboPage.Items.Clear();

                if (dt.Rows.Count == 0)
                {
                    cboPage.Items.Add("1");
                }
                else
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboPage.Items.Add(dt.Rows[i]["PAGE"].ToString());
                    }
                }

                cboPage.SelectedIndex = cboPage.Items.Count - 1;

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
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

        private void dtpSDate_ValueChanged(object sender, EventArgs e)
        {
            GetPage();
        }

        private void cboPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Screen_Clear();
            GetData();
        }

        private void btnAddPage_Click(object sender, EventArgs e)
        {
            if (ComFunc.MsgBoxQ("저장하지 않은 내용은 사라집니다. Page 추가를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            cboPage.Items.Add((cboPage.Items.Count + 1).ToString());
            cboPage.SelectedIndex = cboPage.Items.Count - 1;
        }
    }
}
