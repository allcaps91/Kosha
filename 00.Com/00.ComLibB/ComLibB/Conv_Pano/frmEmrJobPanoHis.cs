using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : EmrJob
    /// File Name       : frmEmrJobPanoHis
    /// Description     : 이중챠트 정리 내역
    /// Author          : 이현종
    /// Create Date     : 2020-02-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= "PSMH\mid\midchdl\FrmPanoHIS(FrmPanoHIS.frm) >> frmEmrJobPanoHis.cs 폼이름 재정의" />
    public partial class frmEmrJobPanoHis : Form
    {
        public frmEmrJobPanoHis()
        {
            InitializeComponent();
        }

        private void frmEmrJobPanoHis_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SS1_Sheet1.Columns[4].Visible = false;

            dtpSDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "D").Substring(0, 4) + "-01-01");
            dtpEDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

            TxtPano.Clear();
            //GetSearchData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetSearchData();
        }

        private void GetSearchData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            SS1_Sheet1.RowCount = 0;

            #region 변수
            string strPano = VB.Val(TxtPano.Text.Trim()).ToString("00000000");
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            #endregion


            Cursor.Current = Cursors.WaitCursor;

            #region 쿼리
            SQL = " SELECT TO_CHAR(A.BDATE, 'YYYY-MM-DD HH24:MI') BDATE, A.PANO, A.TO_PANO, B.SNAME ";
            SQL += ComNum.VBLF + " FROM ADMIN.ETC_PANO_HIS A, ADMIN.BAS_PATIENT B ";
            SQL += ComNum.VBLF + "  WHERE A.PANO = B.PANO(+) ";
            if ((TxtPano.Text).Trim() != "")
            {
                if (rdoSort2.Checked)
                {
                    SQL += ComNum.VBLF + " AND A.PANO = '" + strPano + "' ";
                }
                else if(rdoSort3.Checked)
                {
                    SQL += ComNum.VBLF + " AND A.TO_PANO = '" + strPano + "' ";
                }
            }

            if (rdoDate.Checked)
            {
                SQL += ComNum.VBLF + " AND A.BDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd 00:00") + "', 'YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + " AND A.BDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd 23:59") + "', 'YYYY-MM-DD HH24:MI')";
            }

            SQL += ComNum.VBLF + " GROUP BY TO_CHAR (A.BDATE, 'YYYY-MM-DD HH24:MI'), A.PANO, A.TO_PANO, B.SNAME ";


            if (rdoSort0.Checked)
            {
                SQL += ComNum.VBLF + " ORDER BY TO_CHAR (A.BDATE, 'YYYY-MM-DD HH24:MI') DESC, A.PANO ";
            }
            else if (rdoSort1.Checked)
            {
                SQL += ComNum.VBLF + " ORDER BY A.PANO DESC ";
            }
            //else if (rdoSort2.Checked)
            //{
            //    SQL += ComNum.VBLF + " ORDER BY A.TO_PANO ";
            //}
            
            SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    SS1_Sheet1.RowCount += 1;
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0].Text = reader.GetValue(0).ToString().Trim();
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = reader.GetValue(2).ToString().Trim();
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 2].Text = reader.GetValue(1).ToString().Trim();
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 3].Text = reader.GetValue(3).ToString().Trim();
                }
            }

            reader.Dispose();
            #endregion

            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TxtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                TxtPano.Text = VB.Val(TxtPano.Text.Trim()).ToString("00000000");
                GetSearchData();
            }
        }

        private void SS1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(SS1, e.Column);
                return;
            }
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SS1_Sheet1.RowCount == 0)
                return;

            if (e.Column == 1 || e.Column == 2)
            {
                ViewEmr(this, SS1_Sheet1.Cells[e.Row, e.Column].Text.Trim());
            }
        }


        /// <summary>
        /// EMR 뷰어 실행
        /// </summary>
        private void ViewEmr(Form pForm, string strPtno)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name.Equals("frmEmrViewer"))
                {
                    (frm as frmEmrViewer).SetNewPatient(strPtno);
                    return;
                }
                else
                {
                    //fEmrViewer.SetNewPatient(GstrPANO);
                }
            }
            ComFunc.MsgBoxEx(pForm, "EMR 뷰어를 실행해주세요.");

        }
    }
}
