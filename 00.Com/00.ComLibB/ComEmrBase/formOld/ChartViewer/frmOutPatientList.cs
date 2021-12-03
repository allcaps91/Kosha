using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmOutPatientList
    /// Description     : 퇴원자 조회
    /// Author          : 이현종
    /// Create Date     : 2019-08-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " PSMH\mtsEmr(frmOutPatientList.frm) >> frmOutPatientList.cs 폼이름 재정의" />
    /// 
    public partial class frmOutPatientList : Form
    {
        //환자정보 전달
        public delegate void SendPatNo(string strPtNo);
        public event SendPatNo rSendPatInfo;

        public frmOutPatientList()
        {
            InitializeComponent();
        }

        private void FrmOutPatientList_Load(object sender, EventArgs e)
        {
            SS1_Sheet1.RowCount = 0;

            dtpDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            GetSearhData();
        }

        void GetSearhData()
        {

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            SS1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT PANO, SNAME, DEPTCODE, DRNAME, WARDCODE ";
                SQL += ComNum.VBLF + " FROM ADMIN.IPD_NEW_MASTER A";
                SQL += ComNum.VBLF + "   INNER JOIN ADMIN.BAS_DOCTOR B";
                SQL += ComNum.VBLF + "      ON B.DRCODE = A.DRCODE";
                SQL += ComNum.VBLF + " WHERE OUTDATE = TO_DATE('" + dtpDate.Value.ToShortDateString() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND GBSTS NOT IN ('9') ";
                SQL += ComNum.VBLF + "   ORDER BY PANO ASC ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    SS1_Sheet1.RowCount = dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = string.Format("{0}/{1}", dt.Rows[i]["DEPTCODE"].ToString().Trim(), dt.Rows[i]["DRNAME"].ToString().Trim());
                        SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    }
                }

                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SS1_Sheet1.RowCount == 0)
                return;

            rSendPatInfo(SS1_Sheet1.Cells[e.Row, 0].Text.Trim());
            Close();
        }
    }
}
