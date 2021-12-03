using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using System.Drawing;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComSupLibB
{
    public partial class frmInfectionSabun : Form
    {
        public delegate void GetData(string strSabun, string strName); //델리게이트 선언문
        public event GetData rGetData; //델리게이트 이벤트 선언문

        public frmInfectionSabun()
        {
            InitializeComponent();
        }

        private void frmInfectionSabun_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetUser("USER", "", txtPtInfo.Text.Trim());
        }

        private void GetUser(string strOption, string strDeptCd, string strUser)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            string SQL = "";
            DataTable dt = null;
            int i = 0;

            string SqlErr = "";

            ssUser_Sheet1.RowCount = 0;

            try
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
                //SQL = SQL + ComNum.VBLF + "    AND GRPCD = '작업그룹'";
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
                    ssUser_Sheet1.Cells[i, 4].Text = dt.Rows[i]["JOBGROUP"].ToString().Trim();
                    ssUser_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GRPCD"].ToString().Trim() + " (" + dt.Rows[i]["BASNAME"].ToString().Trim() + ")";
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

        private void txtPtInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetUser("USER", "", txtPtInfo.Text.Trim());
            }
        }

        private void ssUser_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssUser_Sheet1.RowCount == 0)
            {
                return;
            }

            rGetData(ssUser_Sheet1.Cells[e.Row, 2].Text, ssUser_Sheet1.Cells[e.Row, 3].Text);

            this.Close();
        }
    }
}
