using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmBST_D : Form
    {
        string strPano = string.Empty;

        public frmBST_D(string Pano)
        {
            strPano = Pano;
            InitializeComponent();
        }

        private void FrmBST_D_Load(object sender, EventArgs e)
        {
            SSPatientInfo_Sheet1.Cells[0, 0].Text = strPano;
            SSPatientInfo_Sheet1.Cells[0, 1].Text = clsVbfunc.GetPatientName(clsDB.DbCon, strPano);

            dtpEDATE.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            dtpSDATE.Value = dtpEDATE.Value.AddDays(-5);


            btnSearch.PerformClick();

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            READ_BST();
        }


        void READ_BST()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssBST_Sheet1.ColumnCount = 1;
            FarPoint.Win.ComplexBorder border = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            EmrForm pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "1572");

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                #region XML       
                SQL = " SELECT CHARTDATE, CHARTTIME,";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//ta2') AS TA2,";
                SQL = SQL + ComNum.VBLF + "              (extractValue(chartxml, '//ta4') || ' ' || ";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//ta5') ||";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//ta9')) AS VAL2";
                SQL = SQL + ComNum.VBLF + "    From ADMIN.EMRXML";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO IN (";
                SQL = SQL + ComNum.VBLF + "  SELECT EMRNO FROM ADMIN.EMRXMLMST WHERE FORMNO = 1572";
                SQL = SQL + ComNum.VBLF + "      AND PTNO = '" + strPano + "'";
                SQL = SQL + ComNum.VBLF + "      AND CHARTDATE >= '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "      AND CHARTDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "')";
                #endregion
                #region 신규
                if (pForm.FmOLDGB != 1)
                {
                    SQL += ComNum.VBLF + "UNION ALL";
                    SQL += ComNum.VBLF + " SELECT CHARTDATE, CHARTTIME, B1.ITEMVALUE AS TA2, (B2.ITEMVALUE || ' ' || B3.ITEMVALUE || B4.ITEMVALUE) AS VAL2";
                    SQL += ComNum.VBLF + "    From ADMIN.AEMRCHARTMST A";
                    SQL += ComNum.VBLF + "      LEFT OUTER JOIN ADMIN.AEMRCHARTROW B1";
                    SQL += ComNum.VBLF + "         ON A.EMRNO = B1.EMRNO";
                    SQL += ComNum.VBLF + "        AND A.EMRNOHIS = B1.EMRNOHIS";
                    SQL += ComNum.VBLF + "        AND B1.ITEMCD = 'I0000009122' -- Glucose";
                    SQL += ComNum.VBLF + "      LEFT OUTER JOIN ADMIN.AEMRCHARTROW B2";
                    SQL += ComNum.VBLF + "         ON A.EMRNO = B2.EMRNO";
                    SQL += ComNum.VBLF + "        AND A.EMRNOHIS = B2.EMRNOHIS";
                    SQL += ComNum.VBLF + "        AND B2.ITEMCD = 'I0000004686' -- Insulin";
                    SQL += ComNum.VBLF + "      LEFT OUTER JOIN ADMIN.AEMRCHARTROW B3";
                    SQL += ComNum.VBLF + "         ON A.EMRNO = B3.EMRNO";
                    SQL += ComNum.VBLF + "        AND A.EMRNOHIS = B3.EMRNOHIS";
                    SQL += ComNum.VBLF + "        AND B3.ITEMCD = 'I0000035480' -- 약용량";
                    SQL += ComNum.VBLF + "      LEFT OUTER JOIN ADMIN.AEMRCHARTROW B4";
                    SQL += ComNum.VBLF + "         ON A.EMRNO = B4.EMRNO";
                    SQL += ComNum.VBLF + "        AND A.EMRNOHIS = B4.EMRNOHIS";
                    SQL += ComNum.VBLF + "        AND B4.ITEMCD = 'I0000001311' -- 비고";
                    SQL += ComNum.VBLF + "    WHERE FORMNO = 1572";
                    SQL += ComNum.VBLF + "      AND PTNO = '" + strPano + "'";
                    SQL += ComNum.VBLF + "      AND CHARTDATE >= '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'";
                    SQL += ComNum.VBLF + "      AND CHARTDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "'";
                }
                #endregion

                SQL += ComNum.VBLF + "      ORDER BY CHARTDATE DESC, CHARTTIME DESC";


                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, " EMRSYSMP 조회중 문제가 발생했습니다");
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

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssBST_Sheet1.ColumnCount += 1;
                    ssBST_Sheet1.Cells[0, i + 1].Text = VB.Val(VB.Right(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 4)).ToString("00/00") +
                    " " + VB.Val(VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4)).ToString("00:00");
                    ssBST_Sheet1.Cells[0, i + 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    ssBST_Sheet1.Cells[0, i + 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                    ssBST_Sheet1.Cells[1, i + 1].Text = dt.Rows[i]["TA2"].ToString().Trim();
                    ssBST_Sheet1.Cells[2, i + 1].Text = dt.Rows[i]["VAL2"].ToString().Trim();

                    ssBST_Sheet1.Columns[i + 1].Width = ssBST_Sheet1.Columns[i + 1].GetPreferredWidth() + 5;
                }

                dt.Dispose();
                dt = null;

                ssBST_Sheet1.Columns[1, ssBST_Sheet1.ColumnCount - 1].Border = border;
                ssBST_Sheet1.Columns[1, ssBST_Sheet1.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssBST_Sheet1.Columns[1, ssBST_Sheet1.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                Cursor.Current = Cursors.Default;

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

    }
}
