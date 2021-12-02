using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : FrmViewRemark.cs
    /// Description     : Remark 및 소견 선택
    /// Author          : 이정현
    /// Create Date     : 2018-04-04
    /// <history> 
    /// Remark 및 소견 선택
    /// </history>
    /// <seealso>
    /// PSMH\Ocs\ipdocs\iorder\mtsiorder\FrmViewRemark.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\Ocs\ipdocs\iorder\mtsiorder.vbp
    /// </vbp>
    /// </summary>
    public partial class FrmViewRemark : Form
    {
        public delegate void SendDataHandler(string SendRetValue);
        public event SendDataHandler SendEvent;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        private string GstrDrCode = "";

        public FrmViewRemark()
        {
            InitializeComponent();
        }

        public FrmViewRemark(string strDrCode)
        {
            InitializeComponent();

            GstrDrCode = strDrCode;
        }

        private void FrmViewRemark_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            Read_PRMremark("ALL");
        }

        private void btnSearchR_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Tag.ToString() == "")
            {
                rEventClosed();
                return;
            }
            
            Read_PRMremark(((Button)sender).Tag.ToString());
        }

        private void Read_PRMremark(string strIndex)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssRemark_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM " + ComNum.DB_MED + "OCS_OREMARK ";
                SQL = SQL + ComNum.VBLF + "     WHERE DrCode = '" + clsType.User.Sabun + "' ";

                if (strIndex.Trim() != "ALL")
                {
                    SQL = SQL + ComNum.VBLF + "         AND (UPPER(RemarkCode) LIKE '" + strIndex + "%'  ";
                    SQL = SQL + ComNum.VBLF + "             OR UPPER(RemarkCode) LIKE '%" + strIndex + "') ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY RemarkCode ";

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
                    ssRemark_Sheet1.RowCount = dt.Rows.Count;
                    ssRemark_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssRemark_Sheet1.Cells[i, 0].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssRemark_Sheet1.Cells[i, 1].Text = dt.Rows[i]["REMARKCODE"].ToString().Trim();
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

        private void ssRemark_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            lblTitleSub0.Text = "RemarkCode : " + ssRemark_Sheet1.Cells[e.Row, 1].Text.Trim();
        }

        private void ssRemark_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            SendEvent(ssRemark_Sheet1.Cells[e.Row, 0].Text.Trim());
            this.Close();
        }
    }
}
