using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmCostReceipt
    /// File Name : frmCostReceipt.cs
    /// Title or Description : 수납내역확인 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-01
    /// <history> 
    /// 2017-06-20 3개의 변수를 받아서 이 폼에서 처리함 부모폼에서 데이터를 넘겨줘야한다.(FrmEkgJob - Ekg09.frm에서)
    /// </history>
    /// </summary>
    public partial class frmCostReceipt : Form
    {
        //넘겨 받는 변수
        string GstrPANO = "";
        string GstrHelpCode = "";
        string GstrOrderCode = "";
        public frmCostReceipt()
        {
            InitializeComponent();
        }
        
        public frmCostReceipt(string Pano, string HelpCode, string OrderCode)
        {
            InitializeComponent();
            GstrPANO = Pano;
            GstrHelpCode = HelpCode;
            GstrOrderCode = OrderCode;
        }

        private void frmCostReceipt_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpFDate.Text = GstrHelpCode;
            dtpTDate.Text = GstrHelpCode;

            if (GstrPANO != "")
            {
                btnView.PerformClick();
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ViewData() == false) return;
        }

        /// <summary>
        /// 조회 함수
        /// </summary>
        /// <returns></returns>
        private bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            clsDB.setBeginTran(clsDB.DbCon);
            

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " CREATE OR REPLACE VIEW VIEW_OPD_IPD_SLIP AS ";
                SQL += ComNum.VBLF + "   SELECT A.SUNEXT, A.SUCODE, B.SUNAMEK, TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE,";
                SQL += ComNum.VBLF + "             A.DEPTCODE, A.DRCODE , A.PANO, C.DRNAME ";
                SQL += ComNum.VBLF + "       ,SUM(A.QTY * A.NAL) QTY ";
                SQL += ComNum.VBLF + "   from " + ComNum.DB_PMPA + "OPD_SLIP A, KOSMOS_PMPA.BAS_SUN B , KOSMOS_PMPA.BAS_DOCTOR C";
                SQL += ComNum.VBLF + "   WHERE A.BDATE >= TO_DATE('" + dtpFDate.Text + "' ,'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "     AND A.BDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "     AND A.PANO = '" + GstrPANO + "' ";
                SQL += ComNum.VBLF + "     AND A.SUCODE IN (";
                SQL += ComNum.VBLF + " SELECT SUCODE FROM " + ComNum.DB_MED + "OCS_ORDERCODE  ";
                SQL += ComNum.VBLF + "   WHERE ORDERCODE = '" + GstrOrderCode + "' ";
                SQL += ComNum.VBLF + " GROUP BY SUCODE ";
                SQL += ComNum.VBLF + " ) ";
                SQL += ComNum.VBLF + "     AND A.SUNEXT = B.SUNEXT";
                SQL += ComNum.VBLF + "     AND A.DRCODE =C.DRCODE(+)";
                SQL += ComNum.VBLF + "    GROUP BY A.SUNEXT, A.SUCODE, B.SUNAMEK, A.BDATE,";
                SQL += ComNum.VBLF + "             A.DEPTCODE, A.DRCODE , A.PANO, C.DRNAME";
                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + "   SELECT A.SUNEXT, A.SUCODE, B.SUNAMEK, TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE,";
                SQL += ComNum.VBLF + "             A.DEPTCODE, A.DRCODE , A.PANO, C.DRNAME ";
                SQL += ComNum.VBLF + "       ,SUM(A.QTY * A.NAL) QTY ";
                SQL += ComNum.VBLF + "   from " + ComNum.DB_PMPA + "IPD_NEW_SLIP A, KOSMOS_PMPA.BAS_SUN B , KOSMOS_PMPA.BAS_DOCTOR C";
                SQL += ComNum.VBLF + "   WHERE A.BDATE >= TO_DATE('" + dtpFDate.Text + "' ,'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "     AND A.BDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "     AND A.PANO = '" + GstrPANO + "' ";
                SQL += ComNum.VBLF + "     AND A.SUCODE IN (";
                SQL += ComNum.VBLF + " SELECT SUCODE FROM " + ComNum.DB_MED + "OCS_ORDERCODE  ";
                SQL += ComNum.VBLF + "   WHERE ORDERCODE = '" + GstrOrderCode + "' ";
                SQL += ComNum.VBLF + " GROUP BY SUCODE ";
                SQL += ComNum.VBLF + " ) ";
                SQL += ComNum.VBLF + "     AND A.SUNEXT = B.SUNEXT";
                SQL += ComNum.VBLF + "     AND A.DRCODE =C.DRCODE(+)";
                SQL += ComNum.VBLF + "    GROUP BY A.SUNEXT, A.SUCODE, B.SUNAMEK, A.BDATE,";
                SQL += ComNum.VBLF + "             A.DEPTCODE, A.DRCODE , A.PANO, C.DRNAME";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUNEXT, SUCODE, SUNAMEK, BDATE, ";
                SQL += ComNum.VBLF + "             DEPTCODE, DRCODE , PANO, DRNAME ";
                SQL += ComNum.VBLF + "       ,SUM(QTY) QTY ";
                SQL += ComNum.VBLF + "  FROM VIEW_OPD_IPD_SLIP ";
                SQL += ComNum.VBLF + " GROUP BY SUNEXT, SUCODE, SUNAMEK, BDATE,";
                SQL += ComNum.VBLF + "          DEPTCODE, DRCODE , PANO, DRNAME ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }

                ss1_Sheet1.Rows.Count = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["qty"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
