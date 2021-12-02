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

/// <summary>
/// Description : 국적코드 조회
/// Author : 박병규
/// Create Date : 2017.11.07
/// </summary>
/// <history>
/// </history>
/// <seealso cref="신규"/>

namespace ComPmpaLibB
{
    public partial class frmPmpaViewNational : Form
    {
        clsSpread CS = null;

        DataTable Dt = new DataTable();
        string SQL = "";
        string SqlErr = "";

        private string GstrValue = "";

        public delegate void SetGstrValue(string GstrValue);
        public event SetGstrValue rSetGstrValue;

        public delegate void EventClose();
        public event EventClose rEventClose;


        public frmPmpaViewNational()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);

            this.btnSearch.Click += new EventHandler(eCtl_Click);
            this.btnExit.Click += new EventHandler(eCtl_Click);
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
                GETDATA();
            else if (sender == this.btnExit)
                this.Close();
        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            CS = new clsSpread();
            txtName.Text = "";

            GETDATA();
        }

        private void GETDATA()
        {
            Cursor.Current = Cursors.WaitCursor;
            ComFunc.SetAllControlClear(pnlBody);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT CODE, NAME ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN     = 'BAS_외국인환자국적'";

            if (txtName.Text.Trim() != "")
                SQL += ComNum.VBLF + "    AND NAME LIKE '%" + txtName.Text + "%' ";

            SQL += ComNum.VBLF + "    AND DELDATE IS NULL";
            SQL += ComNum.VBLF + "  ORDER BY decode(code,'KR',1,2) ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["CODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["NAME"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;

        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string rtnVal = string.Empty;

            if (e.Row == -1) return;
            GstrValue = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
            GstrValue += ".";
            GstrValue += ssList_Sheet1.Cells[e.Row, 1].Text.Trim();

            rSetGstrValue(GstrValue);
         

            this.Close();
        }
    }
}
