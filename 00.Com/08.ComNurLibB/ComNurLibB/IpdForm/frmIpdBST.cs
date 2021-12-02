using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;

namespace ComNurLibB
{
    public partial class frmIpdBST : Form
    {

        clsSpread SP = new clsSpread();
        string mstrWard = "";

        public frmIpdBST()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strWard"> ex)ER</param>
        public frmIpdBST(string strWard)
        {
            InitializeComponent();
            mstrWard = strWard;
        }


        private void frmIpdBST_Load(object sender, EventArgs e)
        {

            string gsWard = "";

            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate((ComQuery.CurrentDateTime(clsDB.DbCon, "D")), "D")).AddDays(-1);
            dtpFDate.Value = dtpTDate.Value.AddDays(-6);

            cboWard.Items.Clear();
            cboWard.Items.Add("****.전체");
            cboWard.Items.Add("ER.응급실");
            cboWard.Items.Add("HD.인공신장");
            cboWard.Items.Add("OR.수술실");

            clsVbfunc.SetWardCodeCombo(clsDB.DbCon, cboWard, "1", false, 1);

            ComFunc.ComboFind(cboWard, "L", 2, mstrWard);

            if (clsType.User.Sabun == "04349" || clsType.User.Sabun == "EDPS")
            {
                cboWard.Enabled = true;
            }
            else
            {
                cboWard.Enabled = false;
            }

            gsWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");

            if (gsWard != "")
            {
                foreach (string str in cboWard.Items)
                {
                    if (VB.Split(str, ".")[0].Trim() == gsWard)
                    {
                        cboWard.Text = str;
                        cboWard.Enabled = false;
                        break;
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string strWARD = "";
            int i = 0;
            string strFlag = "";
            int nQty = 0;
            int nQtyTot = 0;
            //int nSumQty = 0;
            //int nRow = 0;

            strWARD = VB.Split(cboWard.Text.Trim(), ".")[0].Trim();

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView1_Sheet1.SetColumnMerge(0, FarPoint.Win.Spread.Model.MergePolicy.Always);
            ssView1_Sheet1.SetColumnMerge(1, FarPoint.Win.Spread.Model.MergePolicy.Always);
            ssView1_Sheet1.SetColumnMerge(2, FarPoint.Win.Spread.Model.MergePolicy.Always);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                ssView1_Sheet1.RowCount = 0;

                SQL = "";
                SQL = " SELECT  TO_CHAR(A.BDATE, 'YYYY-MM-DD') BDATE, DECODE(A.WARDCODE,'IQ','NR','ND','NR',A.WARDCODE) WARDCODE, A.PANO, ";
                SQL = SQL + ComNum.VBLF + "   A.SNAME, A.DEPTCODE, SUM(A.QTY) QTY ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.EXAM_EXBST A  ";

                if (optOut0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE >=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <=TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.ODATE IS  NULL ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.ODATE >=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.ODATE <=TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.ODATE IS NOT NULL ";
                }

                if (strWARD != "****")
                {
                    if (strWARD == "ND" || strWARD == "NR")
                    {
                        SQL = SQL + ComNum.VBLF + "AND  A.WARDCODE IN ('ND','IQ','NR') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND A.WARDCODE = '" + strWARD + "' ";
                    }
                }

                SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(A.BDATE, 'YYYY-MM-DD') , A.WARDCODE  , A.PANO, ";
                SQL = SQL + ComNum.VBLF + "   A.SNAME, A.DEPTCODE";
                SQL = SQL + ComNum.VBLF + " HAVING SUM(A.QTY) <> 0 ";

                if (strWARD != "OR")
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY 2,3,1 ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY 1 ASC , 2,3 ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i != 0 && strFlag != dt.Rows[i]["WARDCODE"].ToString().Trim())
                        {
                            ssView1_Sheet1.RowCount = ssView1_Sheet1.RowCount + 1;

                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 0].Text = "소계 [ " + strFlag + " ]";
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 5].Text = Convert.ToString(nQty);
                            ssView1_Sheet1.Rows.Get(ssView1_Sheet1.RowCount - 1).BackColor = Color.FromArgb(255, 220, 220);

                            nQty = 0;
                            strFlag = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        }
                        else
                        {
                            if (i == 0)
                            {
                                strFlag = dt.Rows[i]["WARDCODE"].ToString().Trim();
                            }

                            ssView1_Sheet1.RowCount = ssView1_Sheet1.RowCount + 1;

                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["QTY"].ToString().Trim();

                            nQty = nQty + Convert.ToInt32(dt.Rows[i]["QTY"].ToString().Trim());

                            nQtyTot = nQtyTot + Convert.ToInt32(dt.Rows[i]["QTY"].ToString().Trim());
                        }
                    }


                    ssView1_Sheet1.RowCount = ssView1_Sheet1.RowCount + 1;
                    ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 0].Text = "소계 [ " + strFlag + " ]";
                    ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 5].Text = Convert.ToString(nQty);
                    ssView1_Sheet1.Rows.Get(ssView1_Sheet1.RowCount - 1).BackColor = Color.FromArgb(255, 220, 220);

                    ssView1_Sheet1.RowCount = ssView1_Sheet1.RowCount + 1;
                    ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 0].Text = "총계";
                    ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 5].Text = nQtyTot.ToString("###,###,##0");
                    ssView1_Sheet1.Rows.Get(ssView1_Sheet1.RowCount - 1).BackColor = Color.FromArgb(255, 100, 100);
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void optOut_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                if (optOut0.Checked == true)
                {
                    lblDate.Text = "수납일자";
                }
                else
                {
                    lblDate.Text = "불출일자";
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            strTitle = "** BST 수불 LIST **" + "\r\n\r\n";

            strHeader = SP.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            if (optOut0.Checked == true)
            {
                strHeader += SP.setSpdPrint_String(" 불출일자: " + dtpFDate.Text + "(미불출)", new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            }
            else
            {
                strHeader += SP.setSpdPrint_String(" 불출일자: " + dtpFDate.Text + "(불출)", new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            }

            setMargin = new clsSpread.SpdPrint_Margin(2, 10, 5, 10, 5, 10);
            setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Auto, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

            SP.setSpdPrint(ssView1, PrePrint, setMargin, setOption, strHeader, strFooter);

            MessageBox.Show("출력 되었습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
