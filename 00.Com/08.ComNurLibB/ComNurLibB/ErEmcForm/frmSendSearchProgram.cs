using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComNurLibB
{
    public partial class frmSendSearchProgram : Form
    {
        public frmSendSearchProgram()
        {
            InitializeComponent();
        }

        private void frmSendSearchProgram_Load(object sender, EventArgs e)
        {            
            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpFDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpTDate.Value = dtpFDate.Value;
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {            
            GetSearchData();
        }

        void GetSearchData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT TO_CHAR(SDATE,'YYYY-MM-DD HH24:MI') SDATE, ERCNT, ORCNT, PDIU, NRIU, CSIU,";
                SQL += ComNum.VBLF + " ICU, ERNAME, CT, MRI, ANGIO, VT, ROOM, SREMARK, SEND ";
                SQL += ComNum.VBLF + " From KOSMOS_PMPA.ETC_EMC ";
                SQL += ComNum.VBLF + " WHERE SDATE >= TO_DATE('" + dtpFDate.Text.Trim() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND SDATE < TO_DATE('" + dtpTDate.Value.AddDays(+1).ToShortDateString() + "','YYYY-MM-DD') ";

                if(rdoSend1.Checked == true)
                {
                    SQL += ComNum.VBLF + " AND SEND  = '1' ";
                }
                else if(rdoSend2.Checked == true)
                {
                    SQL += ComNum.VBLF + " AND SEND  = '0' ";
                }

                SQL += ComNum.VBLF + " ORDER BY SDATE DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ss1_Sheet1.RowCount = 0;
                ss1_Sheet1.RowCount = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SDATE"].ToString();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ERCNT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ORCNT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PDIU"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["NRIU"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["CSIU"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ICU"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ERNAME"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["CT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["MRI"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ANGIO"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["VT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["ROOM"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["SREMARK"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {            
            SETPRINT();
        }

        void SETPRINT()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";

            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "전 송 리 스 트";
            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("조회일자 : " +  dtpFDate.Text + "~" + dtpTDate.Text, new Font("굴림체", 11), clsSpread.enmSpdHAlign.Left, false, true);
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, false, true, false, false, false);

            CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ss1_Sheet1.RowCount = 0;
            ss1_Sheet1.RowCount = 50;
            ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
