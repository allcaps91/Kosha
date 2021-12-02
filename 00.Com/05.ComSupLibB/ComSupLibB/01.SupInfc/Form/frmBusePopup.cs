using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComSupLibB.SupInfc
{
    public partial class frmBusePopup : Form
    {
        public string USERNAME = string.Empty;
        public string USERID = string.Empty;
        public string BUCODE = string.Empty;
        public string BUNAME = string.Empty;

        public frmBusePopup()
        {
            InitializeComponent();
        }

        public frmBusePopup(string name)
        {
            InitializeComponent();
            txtBuName.Text = name;
        }

        private void frmBusePopup_Load(object sender, EventArgs e)
        {
            Init();
            btnDeptSearch.PerformClick();
        }

        private void Init()
        {
            ssMain_Sheet1.Rows.Clear();
            ssUser_Sheet1.Rows.Clear();
        }

        private void btnDeptSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            ssMain_Sheet1.Rows.Clear();

            txtBuName.Text = txtBuName.Text.Trim();

            string mstrViewBun = "ACC";

            try
            {
                string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "BuCode,Name";
                SQL = SQL + ComNum.VBLF + "FROM";
                SQL = SQL + ComNum.VBLF + "KOSMOS_PMPA.BAS_BUSE";

                if (mstrViewBun == "ACC")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE ACC='*'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE JAS='*' ";
                }

                if (strCurDate != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND (DelDate IS NULL OR DelDate>=TO_DATE('" + ComFunc.FormatStrToDate(strCurDate, "D") + "','YYYY-MM-DD')) ";
                }

                if (txtBuName.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + "AND Name LIKE '%" + txtBuName.Text + "%' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY BuCode ";
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

                ssMain_Sheet1.Rows.Count = dt.Rows.Count;
                ssMain_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssMain_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BuCode"].ToString().Trim();
                    ssMain_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ssMain_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            btnDeptSearch.PerformClick();
        }

        private void ssMain_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssMain_Sheet1.RowCount <= 0) return;
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssMain, e.Column);
                return;
            }

            string strBUCODE = ssMain_Sheet1.Cells[e.Row, 0].Text.Trim();

            ssMain_Sheet1.Cells[0, 0, ssMain_Sheet1.RowCount - 1, 1].BackColor = ComNum.SPDESELCOLOR;
            ssMain_Sheet1.Cells[e.Row, 0, e.Row, 1].BackColor = ComNum.SPSELCOLOR;

            GetUser("DEPT", strBUCODE, "");
        }


        private void GetUser(string strOption, string strDeptCd, string strUser)
        {
            string SQL = string.Empty;
            DataTable dt = null;
            int i = 0;

            string SqlErr = string.Empty;
            ssUser_Sheet1.Rows.Clear();

            try
            {
                if (strOption == "DEPT")
                {
                    SQL = "";
                    SQL = " SELECT  ";
                    SQL = SQL + ComNum.VBLF + "    U.IDNUMBER, U.USERNAME, U.SABUN, U.JOBGROUP, ";
                    SQL = SQL + ComNum.VBLF + "    I.BUSE, B.NAME AS BUSENAME, D.GRPCD, D.BASNAME";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST I ";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_PMPA + "BAS_BUSE B ";
                    SQL = SQL + ComNum.VBLF + "    ON I.BUSE = B.BUCODE ";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_PMPA + "BAS_USER U ";
                    SQL = SQL + ComNum.VBLF + "    ON I.SABUN = U.SABUN ";
                    SQL = SQL + ComNum.VBLF + "     AND U.ABORTYN = '0'";
                    SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_BASCD D";
                    SQL = SQL + ComNum.VBLF + "    ON U.JOBGROUP = D.BASCD ";
                    SQL = SQL + ComNum.VBLF + "    AND GRPCDB = '권한관리'";
                    //SQL = SQL + ComNum.VBLF + "    AND GRPCD = '작업그룹'";
                    SQL = SQL + ComNum.VBLF + "WHERE I.BUSE = '" + strDeptCd + "' ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY U.USERNAME ";
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT  ";
                    SQL = SQL + ComNum.VBLF + "    U.IDNUMBER, U.USERNAME, U.SABUN, U.JOBGROUP, ";
                    SQL = SQL + ComNum.VBLF + "    I.BUSE, B.NAME AS BUSENAME, D.GRPCD, D.BASNAME";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_USER U ";
                    SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_ERP + "INSA_MST I ";
                    SQL = SQL + ComNum.VBLF + "    ON U.SABUN = I.SABUN ";
                    SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_BUSE B ";
                    SQL = SQL + ComNum.VBLF + "    ON I.BUSE = B.BUCODE ";
                    SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_BASCD D";
                    SQL = SQL + ComNum.VBLF + "    ON U.JOBGROUP = D.BASCD ";
                    SQL = SQL + ComNum.VBLF + "    AND GRPCDB = '권한관리'";
                    //SQL = SQL + ComNum.VBLF + "    AND GRPCD = ''";
                    if (strUser != "")
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE (U.USERNAME LIKE '%" + strUser + "%'";
                        SQL = SQL + ComNum.VBLF + "      OR U.IDNUMBER LIKE '%" + strUser + "%')";
                        SQL = SQL + ComNum.VBLF + "    AND U.ABORTYN = '0'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE U.ABORTYN = '0'";
                    }
                    SQL = SQL + ComNum.VBLF + "ORDER BY U.USERNAME ";
                }

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

                ssUser_Sheet1.RowCount = dt.Rows.Count;
                ssUser_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssUser_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BUSE"].ToString().Trim();
                    ssUser_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BUSENAME"].ToString().Trim();
                    ssUser_Sheet1.Cells[i, 2].Text = dt.Rows[i]["IDNUMBER"].ToString().Trim();
                    ssUser_Sheet1.Cells[i, 3].Text = dt.Rows[i]["USERNAME"].ToString().Trim();
                    //ssUser_Sheet1.Cells[i, 4].Text = dt.Rows[i]["JOBGROUP"].ToString().Trim();
                    //ssUser_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GRPCD"].ToString().Trim() + " (" + dt.Rows[i]["BASNAME"].ToString().Trim() + ")";
                }

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

        private void btnUserSearch_Click(object sender, EventArgs e)
        {
            GetUser("USER", "", txtPtInfo.Text.Trim());
        }

        private void ssUser_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(e.ColumnHeader || e.RowHeader || e.Row < 0)
            {
                return;
            }

            BUCODE = ssUser_Sheet1.Cells[e.Row, 0].Text;
            BUNAME = ssUser_Sheet1.Cells[e.Row, 1].Text;
            USERID = ssUser_Sheet1.Cells[e.Row, 2].Text;
            USERNAME = ssUser_Sheet1.Cells[e.Row, 3].Text;

            btnExit.PerformClick();
        }
    }
}