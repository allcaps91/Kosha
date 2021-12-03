using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComNurLibB
{
    public partial class frmPickupR : Form
    {

        public delegate void PikupRemark(string strRemark, int intRow);
        public event PikupRemark rSetRemark;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        int mintRow = 0;

        public frmPickupR(string strName, int intRow)
        {
            InitializeComponent();

            lblName.Text = strName.Trim();
            mintRow = intRow;
        }

        private void frmPickupR_Load(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT NAME FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + " WHERE GUBUN ='NUR_금식정보' ";
                SQL = SQL + " ORDER BY CODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            rSetRemark(txtRemark.Text.Trim(), mintRow);
            rEventClosed();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount <= 0)
            {
                return;
            }

            txtRemark.Text = ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim();

        }
    }
}
