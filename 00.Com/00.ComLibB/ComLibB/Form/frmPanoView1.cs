using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmPanoView1 : Form
    {
        public delegate void GetValue(string strData);
        public event GetValue rGetValue;

        public frmPanoView1()
        {
            InitializeComponent();
        }

        private void frmPanoView1_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //Form 권한조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y");                       //Form 기본값 셋팅
            
            ComFunc.ReadSysDate(clsDB.DbCon);
            SCREEN_CLEAR();
            ss1_Sheet1.RowCount = 0;
            txtData.Text = "";
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            btnViewClick();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SCREEN_CLEAR()
        {
            txtPano.Text = "";
            txtSName.Text = "";
            txtSex.Text = "";
            txtBi.Text = "";
            txtTel.Text = "";
            txtJuso.Text = "";
            txtDept.Text = "";
            txtLastD.Text = "";
            txtJumin.Text = "";
        }

        private void btnViewClick()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "   SELECT Pano, SName,jumin1,jumin2,BI,TEL,ZIPCODE1,ZIPCODE2, ZIPCODE1 || ZIPCODE2 AS ZipCode, JUSO,SEX,deptcode,  ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(LASTDATE,'YYYY-MM-DD') LASTDATE ";
                SQL = SQL + ComNum.VBLF + " FROM  BAS_PATIENT ";
                if (optJob_0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + ComFunc.SetAutoZero(txtData.Text, 8) + "'  ";
                }
                else if (optJob_1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE Sname = '" + VB.Trim(txtData.Text) + "'  ";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY  jumin1,jumin2 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["pano"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["sname"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["jumin1"].ToString().Trim() + "-" + dt.Rows[i]["jumin2"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["sex"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["deptcode"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["bi"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["tel"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["lastdate"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ZipCode"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["Juso"].ToString().Trim();                        
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strValue = "";

            strValue = strValue + ss1_Sheet1.Cells[e.Row, 0].Text + "@@";
            strValue = strValue + ss1_Sheet1.Cells[e.Row, 1].Text + "@@";
            strValue = strValue + ss1_Sheet1.Cells[e.Row, 2].Text + "@@";
            strValue = strValue + ss1_Sheet1.Cells[e.Row, 3].Text + "@@";
            strValue = strValue + ss1_Sheet1.Cells[e.Row, 4].Text + "@@";
            strValue = strValue + ss1_Sheet1.Cells[e.Row, 5].Text + "@@";
            strValue = strValue + ss1_Sheet1.Cells[e.Row, 6].Text + "@@";
            strValue = strValue + ss1_Sheet1.Cells[e.Row, 7].Text + "@@";
            strValue = strValue + ss1_Sheet1.Cells[e.Row, 8].Text + "@@";
            strValue = strValue + ss1_Sheet1.Cells[e.Row, 9].Text + "@@";

            rGetValue(strValue);
            this.Close();
        }

        private void ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            SCREEN_CLEAR();

            txtPano.Text = ss1_Sheet1.Cells[e.Row, 0].Text;
            txtSName.Text =  ss1_Sheet1.Cells[e.Row, 1].Text;
            txtJumin.Text = ss1_Sheet1.Cells[e.Row, 2].Text;
            txtSex.Text = ss1_Sheet1.Cells[e.Row, 3].Text;
            txtDept.Text = ss1_Sheet1.Cells[e.Row, 4].Text;
            txtBi.Text = ss1_Sheet1.Cells[e.Row, 5].Text;
            txtTel.Text = ss1_Sheet1.Cells[e.Row, 6].Text;
            txtLastD.Text =  ss1_Sheet1.Cells[e.Row, 7].Text;            
            txtJuso.Text = ss1_Sheet1.Cells[e.Row, 9].Text;
        }

        private void txtData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnViewClick();
            }
        }
    }
}
