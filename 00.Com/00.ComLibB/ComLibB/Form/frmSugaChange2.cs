using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmSugaChange2 : Form
    {
        private frmSearchSugaSQL frmSearchSugaSQLX = null;

        string GstrHelpCode = "";

        public frmSugaChange2()
        {
            InitializeComponent();
        }

        void frmSugaChange2_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssSugaChange_Sheet1.Columns[10].Visible = false;
            ssSugaChange_Sheet1.Columns[11].Visible = false;

            opt1.Checked = true;

            ssSugaChange_Sheet1.RowCount = 0;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            btnView.Focus();
        }

        void SCREEN_CLEAR()
        {
            for(int i = 0; i < ssSugaChange_Sheet1.RowCount; i++)
            {
                for(int j = 0; j < ssSugaChange_Sheet1.ColumnCount; j++)
                {
                    ssSugaChange_Sheet1.Cells[i, j].Text = "";
                }
            }

            btnView.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;

            ssSugaChange.Enabled = false;
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            int i, j;
            string strData = "";
            string strOK = "";
            string SQL = "";

            DataTable dt = null;
            string SqlErr = "";

            try
            {
                frmSearchSugaSQLX = new frmSearchSugaSQL();
                frmSearchSugaSQLX.rSetSuGaCodeSQL += frmSearchSugaSQLX_rSetSuGaCodeSQL;
                frmSearchSugaSQLX.rEventClosed += frmSearchSugaSQLX_rEventClosed;
                frmSearchSugaSQLX.ShowDialog();

                SQL = GstrHelpCode;
                if (SQL == "")
                {
                    return;
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssSugaChange.Enabled = true;

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    strOK = "NO";

                    if(opt1.Checked == true && dt.Rows[i]["SugbP"].ToString().Trim() == "1")
                    {
                        strOK = "OK";
                    }
                    if(opt2.Checked == true && dt.Rows[i]["SugbP"].ToString().Trim() == "2")
                    {
                        strOK = "OK";
                    }
                    if(opt3.Checked == true && dt.Rows[i]["SugbP"].ToString().Trim() == "9")
                    {
                        strOK = "OK";
                    }
                    if(opt4.Checked == true && dt.Rows[i]["SugbP"].ToString().Trim() == "")
                    {
                        strOK = "OK";
                    }
                    if(opt5.Checked == true)
                    {
                        strOK = "OK";
                    }

                    if(strOK == "OK")
                    {
                        ssSugaChange_Sheet1.RowCount = dt.Rows.Count + 10;

                        ssSugaChange_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                        ssSugaChange_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Gbn"].ToString().Trim();
                        ssSugaChange_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ssSugaChange_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Bun"].ToString().Trim();
                        ssSugaChange_Sheet1.Cells[i, 4].Text = dt.Rows[i]["sugbp"].ToString().Trim();


                        switch(dt.Rows[i]["SuCode"].ToString().Trim())
                        {
                            case "1":
                                ssSugaChange_Sheet1.Cells[i, 5].Text = "인정";
                                break;
                            case "2":
                                ssSugaChange_Sheet1.Cells[i, 5].Text = "임의";
                                break;
                            case "3":
                                ssSugaChange_Sheet1.Cells[i, 5].Text = "제외";
                                break;
                        }

                        ssSugaChange_Sheet1.Cells[i, 6].Text = dt.Rows[i]["BAmt"].ToString().Trim();
                        ssSugaChange_Sheet1.Cells[i, 7].Text = dt.Rows[i]["TAmt"].ToString().Trim();
                        ssSugaChange_Sheet1.Cells[i, 8].Text = dt.Rows[i]["IAmt"].ToString().Trim();
                        ssSugaChange_Sheet1.Cells[i, 9].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                        ssSugaChange_Sheet1.Cells[i, 10].Text = dt.Rows[i]["NROWID"].ToString().Trim();
                        ssSugaChange_Sheet1.Cells[i, 11].Text = "N";
                    }
                }

                dt.Dispose();
                dt = null;

                btnView.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnExit.Enabled = false;
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void frmSearchSugaSQLX_rEventClosed()
        {
            //닫는 이벤트 내용
            if (frmSearchSugaSQLX != null)
            {
                frmSearchSugaSQLX.Dispose();
                frmSearchSugaSQLX = null;
            }
        }

        private void frmSearchSugaSQLX_rSetSuGaCodeSQL(string argSQL)
        {
            if (argSQL != "")
            {
                GstrHelpCode = argSQL;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            DataSave();
        }

        void DataSave()
        {
            int i = 0;
            string strChange = "";
            string strROWID = "";
            string strSugbP = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for(i = 0; i < ssSugaChange_Sheet1.RowCount; i++)
                {
                    strSugbP = ssSugaChange_Sheet1.Cells[i, 4].Text;
                    strROWID = ssSugaChange_Sheet1.Cells[i, 10].Text;
                    strChange = ssSugaChange_Sheet1.Cells[i, 11].Text;

                    if(strChange == "Y")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_SUN SET";
                        SQL = SQL + ComNum.VBLF + " SUGBP = '" + strSugbP + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if(SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();
                btnView.Focus();
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인
            Print();
        }

        void Print()
        {
            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strHead1 = "/l/f1" + VB.Space(25) + "수가 비급여 항목 SET 내역" + "/n/n";

            strFont2 = "/fn\"굴림체\" /fz\"10\" /fs2";
            strHead2 = "/l/f2" + "인쇄일자 : " + DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            strHead2 = strHead2 + VB.Space(60) + "PAGE : /p";

            ssSugaChange_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;

            ssSugaChange_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;

            ssSugaChange_Sheet1.PrintInfo.Margin.Top = 50;
            ssSugaChange_Sheet1.PrintInfo.Margin.Bottom = 2000;
            ssSugaChange_Sheet1.PrintInfo.Margin.Left = 0;
            ssSugaChange_Sheet1.PrintInfo.Margin.Right = 0;

            ssSugaChange_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssSugaChange_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;

            ssSugaChange_Sheet1.PrintInfo.ShowBorder = true;
            ssSugaChange_Sheet1.PrintInfo.ShowColor = true;
            ssSugaChange_Sheet1.PrintInfo.ShowGrid = false;
            ssSugaChange_Sheet1.PrintInfo.ShowShadows = false;
            ssSugaChange_Sheet1.PrintInfo.UseMax = false;
            ssSugaChange_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssSugaChange_Sheet1.PrintInfo.Preview = true;
            ssSugaChange.PrintSheet(0);
        }

        void ssSugaChange_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            if(e.Column == 4)
            { 
                switch(ssSugaChange_Sheet1.Cells[e.Row, 5].Text)
                {
                    case "1":
                        ssSugaChange_Sheet1.Cells[e.Row, 5].Text = "인정";
                        ssSugaChange_Sheet1.Cells[e.Row, 26].Text = "Y";
                        break;
                    case "2":
                        ssSugaChange_Sheet1.Cells[e.Row, 5].Text = "임의";
                        ssSugaChange_Sheet1.Cells[e.Row, 26].Text = "Y";
                        break;
                    case "3":
                        ssSugaChange_Sheet1.Cells[e.Row, 5].Text = "제외";
                        ssSugaChange_Sheet1.Cells[e.Row, 26].Text = "Y";
                        break;
                    default:
                        ssSugaChange_Sheet1.Cells[e.Row, 5].Text = "";
                        break;

                }
            }
        }
    }
}
