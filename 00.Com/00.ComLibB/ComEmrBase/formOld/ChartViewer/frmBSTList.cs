using System;
using System.Data;
using System.Windows.Forms;
using ComBase;
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmBSTList
    /// Description     : 당뇨약사용내역
    /// Author          : 이현종
    /// Create Date     : 2019-09-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " PSMH\mtsEmr(frmBSTList.frm) >> frmBSTList.cs 폼이름 재정의" />
    /// 
    public partial class frmBSTList : Form
    {
        //public delegate void CloseEvent();
        //public event CloseEvent rClosed;

        string GstrHelpCode = string.Empty;


        public frmBSTList(string strHelpCode)
        {
            GstrHelpCode = strHelpCode;
            InitializeComponent();
        }

        public frmBSTList()
        {
            InitializeComponent();
        }

        private void FrmBSTList_Load(object sender, EventArgs e)
        {
            txtPtno.Clear();
            lblName.Text = "";

            dtpSDATE.Value = dtpEDATE.Value.AddDays(-7);
            dtpEDATE.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

            if (GstrHelpCode.Length > 0)
            {
                //string[] strTEMP = GstrHelpCode.Split
                //TxtPtno.Text = strTEMP(0)
                //TxtSDATE.Text = strTEMP(1)
                //TxtEDATE.Text = strTEMP(2)
                //lbName.Caption = READ_PatientName(TxtPtno.Text)
            }

            btnSearch.PerformClick();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ReadData(txtPtno.Text.Trim());
        }

        void ReadData(string argPTNO)
        {

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            SS1_Sheet1.RowCount = 0;

            string argSDATE = dtpSDATE.Value.ToString("yyyyMMdd");
            string argEDATE = dtpEDATE.Value.ToString("yyyyMMdd");

            Cursor.Current = Cursors.WaitCursor;
            try
            {

                SQL = " SELECT CHARTDATE, SUM(HHUM1) HHUM1, SUM(SC) SC";
                SQL += ComNum.VBLF + " FROM (";
                SQL += ComNum.VBLF + "  SELECT CHARTDATE,";
                SQL += ComNum.VBLF + "     CASE";
                SQL += ComNum.VBLF + "         WHEN TRIM(UPPER(EXTRACTVALUE(CHARTXML,'//ta4'))) = 'H-HUMI' THEN TO_NUMBER(EXTRACTVALUE(CHARTXML,'//ta5'))";
                SQL += ComNum.VBLF + "         WHEN TRIM(UPPER(EXTRACTVALUE(CHARTXML,'//ta4'))) = 'H-HUM1' THEN TO_NUMBER(EXTRACTVALUE(CHARTXML,'//ta5'))";
                SQL += ComNum.VBLF + "         ELSE 0";
                SQL += ComNum.VBLF + "     END HHUM1,";
                SQL += ComNum.VBLF + "     CASE";
                SQL += ComNum.VBLF + "         WHEN TRIM(UPPER(EXTRACTVALUE(CHARTXML,'//ta4'))) IS NOT NULL THEN 1";
                SQL += ComNum.VBLF + "         ELSE 0";
                SQL += ComNum.VBLF + "     END SC";
                SQL += ComNum.VBLF + " FROM ADMIN.EMRXML";
                SQL += ComNum.VBLF + "  WHERE EMRNO IN (";
                SQL += ComNum.VBLF + "   SELECT EMRNO FROM ADMIN.EMRXMLMST";
                SQL += ComNum.VBLF + "    WHERE FORMNO = '1572'";
                SQL += ComNum.VBLF + "     AND PTNO = '" + argPTNO + "' ";
                SQL += ComNum.VBLF + "     AND INOUTCLS = 'I' )";
                SQL += ComNum.VBLF + "     AND CHARTDATE >= '" + argSDATE + "'";
                SQL += ComNum.VBLF + "     AND CHARTDATE <= '" + argEDATE + "')";
                SQL += ComNum.VBLF + "    GROUP BY CHARTDATE";
                SQL += ComNum.VBLF + "    HAVING SUM(HHUM1) + SUM(SC) <> 0 ";
                SQL += ComNum.VBLF + "    ORDER BY CHARTDATE";

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
                    SS1_Sheet1.RowCount =  dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 0].Text = VB.Val(dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("0000-00-00");
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["HHUM1"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SC"].ToString().Trim();
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

        private void TxtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtPtno.Text.Trim().Length == 0 || e.KeyCode != Keys.Enter)
                return;

            if (e.KeyCode == Keys.Enter)
            {
                txtPtno.Text = ComFunc.SetAutoZero(txtPtno.Text, 8);

                lblName.Text = GetPatientName(txtPtno.Text.Trim());
            }
        }

        /// <summary>
        /// 환자 이름 가져오기
        /// </summary>
        /// <param name="strPano"></param>
        /// <returns></returns>
        private string GetPatientName(string strPano)
        {
            string rtnVal = string.Empty;
            string strSql = string.Empty;
            OracleDataReader reader = null;

            strSql = " SELECT SNAME";
            strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
            strSql = strSql + ComNum.VBLF + "  WHERE PANO = '" + strPano + "'";

            string sqlErr = clsDB.GetAdoRs(ref reader, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(sqlErr, sqlErr, clsDB.DbCon);
                ComFunc.MsgBox(sqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();

            return rtnVal;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
