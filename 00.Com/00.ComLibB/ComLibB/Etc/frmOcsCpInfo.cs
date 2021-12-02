using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    public partial class frmOcsCpInfo : Form
    {
        public delegate void SendDataHandler(string strGubun, string strCode, string strName);
        public event SendDataHandler SendEvent;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        private string GstrGUBUN = "";

        public frmOcsCpInfo(string strGUBUN)
        {
            InitializeComponent();

            GstrGUBUN = strGUBUN;
        }

        private void frmOcsCpInfo_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            lblTitle.Text = GstrGUBUN;
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "    BASCD, BASNAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD";
                SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = 'CP관리' ";
                SQL = SQL + ComNum.VBLF + "    AND GRPCD = '" + GstrGUBUN + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BASCD";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) { return; }

            for (int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                {
                    SendEvent(GstrGUBUN, ssView_Sheet1.Cells[i, 1].Text.Trim(), ssView_Sheet1.Cells[i, 2].Text.Trim());
                }
            }

            rEventClosed();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }
    }
}
