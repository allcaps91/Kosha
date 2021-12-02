using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmConvEMR
    /// Description     : 검사결과 경과기록지 변환 내역
    /// Author          : 이현종
    /// Create Date     : 2019-09-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " PSMH\mtsEmr(frmConvEMR.frm) >> frmConvEMR.cs 폼이름 재정의" />
    /// 

    public partial class frmConvEMR : Form
    {
        public frmConvEMR()
        {
            InitializeComponent();
        }

        private void FrmConvEMR_Load(object sender, EventArgs e)
        {
            dtpEDATE.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            dtpSDATE.Value = dtpEDATE.Value.AddDays(-1);
            txtPano.Clear();

            GetSearhData();
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

            SS1_Sheet1.RowCount = 1;

            string cSDATE = dtpSDATE.Value.ToString("yyyyMMdd");
            string cEDATE = dtpEDATE.Value.ToString("yyyyMMdd");
            string cGBN = rdo1.Checked ? "1" : "2";
            string cPTNO = txtPano.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT PTNO, B.SNAME, MEDFRDATE, CONVDATE, CONVTIME, GUBUN, EMRNO";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMR_CONV_RESULT A";
                SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_PMPA.BAS_PATIENT B";
                SQL += ComNum.VBLF + "      ON B.PANO = A.PTNO";

                if (cGBN == "1")
                {
                    SQL += ComNum.VBLF + "  WHERE MEDFRDATE >= '" + cSDATE + "'";
                    SQL += ComNum.VBLF + "    AND MEDFRDATE <= '" + cEDATE + "'";
                }
                else if (cGBN == "2")
                {
                    SQL += ComNum.VBLF + "  WHERE CONVDATE >= '" + cSDATE + "'";
                    SQL += ComNum.VBLF + "    AND CONVDATE <= '" + cEDATE + "'";
                }

                if (cPTNO != "")
                {
                    SQL += ComNum.VBLF + "   AND PTNO = '" + cPTNO + "' ";
                }

                if (cGBN == "1")
                {
                    SQL += ComNum.VBLF + " ORDER BY MEDFRDATE ASC";
                }
                else if (cGBN == "2")
                {
                    SQL += ComNum.VBLF + " ORDER BY CONVDATE ASC";
                }

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

                    SS1_Sheet1.RowCount = 1 + dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i + 1, 0].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        SS1_Sheet1.Cells[i + 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i + 1, 2].Text = dt.Rows[i]["MEDFRDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[i + 1, 3].Text = dt.Rows[i]["CONVDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[i + 1, 4].Text = dt.Rows[i]["CONVTIME"].ToString().Trim();
                        SS1_Sheet1.Cells[i + 1, 5].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        SS1_Sheet1.Cells[i + 1, 6].Text = dt.Rows[i]["EMRNO"].ToString().Trim();

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
            if (txtPano.Text.Trim().Length == 0)
                return;

            if (e.KeyCode == Keys.Enter)
            {
                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
            }
        }


        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
                
        }
    }
}
