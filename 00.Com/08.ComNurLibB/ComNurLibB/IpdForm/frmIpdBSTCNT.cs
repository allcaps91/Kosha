using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;

namespace ComNurLibB
{
    public partial class frmIpdBSTCNT : Form
    {

        clsSpread SP = new clsSpread();
        string mstrWard = "";

        public frmIpdBSTCNT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strWard"> ex)ER</param>
        public frmIpdBSTCNT(string strWard)
        {
            InitializeComponent();
            mstrWard = strWard;
        }


        private void frmIpdBST_Load(object sender, EventArgs e)
        {

            string gsWard = "";

            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate((ComQuery.CurrentDateTime(clsDB.DbCon, "D")), "D")).AddDays(-0);
            dtpFDate.Value = dtpTDate.Value.AddDays(-6);

            cboWard.Items.Clear();
            cboWard.Items.Add("****.전체");
            cboWard.Items.Add("ER.응급실");
            cboWard.Items.Add("HD.인공신장");
            cboWard.Items.Add("OR.수술실");

            clsVbfunc.SetWardCodeCombo(clsDB.DbCon, cboWard, "1", false, 1);

            ComFunc.ComboFind(cboWard, "L", 2, mstrWard);

            if (clsType.User.Sabun == "40024" || clsType.User.Sabun == "EDPS")
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
            Cursor.Current = Cursors.WaitCursor;
            if (SelectData())
            {
                MessageBox.Show("조회가 완료되었습니다.");

            }
            Cursor.Current = Cursors.Default;

           


        }
        private bool SaveData()
        {
            bool rtnVal = true;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;
            //OracleDataReader reader = null;



            clsDB.setBeginTran(clsDB.DbCon);
            try
            {

                for (int i = 0; i < ssView1_Sheet1.RowCount; i++)
                {
                    if (ssView1_Sheet1.Cells[i, 9].Text == "" )
                    {
                        SQL = string.Empty;
                        SQL += ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.NUR_BSTREMARK ";
                        SQL += ComNum.VBLF + "    (BDATE, WARDCODE, REMARK1, REMARK2,REMARK3, ENTSABUN, ENTDATE )";
                        SQL += ComNum.VBLF + "VALUES ( TO_DATE('";
                        SQL += ComNum.VBLF + ssView1_Sheet1.Cells[i, 0].Text.Trim() + "','YYYY-MM-DD'),";    
                        SQL += ComNum.VBLF + "'" + VB.Left(cboWard.Text.Trim(), 2) + "',";    
                        SQL += ComNum.VBLF + "'" + ssView1_Sheet1.Cells[i, 6].Text.Trim() + "',";   
                        SQL += ComNum.VBLF + "'" + ssView1_Sheet1.Cells[i, 7].Text.Trim() + "',";
                        SQL += ComNum.VBLF + "'" + ssView1_Sheet1.Cells[i, 8].Text.Trim() + "',";
                        SQL += ComNum.VBLF + "'" + clsType.User.Sabun + "',";   
                        SQL += ComNum.VBLF + "SYSDATE ";   
                        SQL += ComNum.VBLF + ")";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, "NUR_BSTREMARK저장 중 에러가 발생했습니다.");
                            return false;
                        }

                    }
                    else
                    {
                      
                        SQL = "UPDATE KOSMOS_PMPA.NUR_BSTREMARK SET ";
                        SQL = SQL + ComNum.VBLF + " REMARK1 = '" + ssView1_Sheet1.Cells[i, 6].Text + "',  ";
                        SQL = SQL + ComNum.VBLF + " REMARK2 = '" + ssView1_Sheet1.Cells[i, 7].Text + "',  ";
                        SQL = SQL + ComNum.VBLF + " REMARK3 = '" + ssView1_Sheet1.Cells[i, 8].Text + "',  ";
                        SQL = SQL + ComNum.VBLF + " ENTSABUN = '" + clsType.User.Sabun + "',  ";
                        SQL = SQL + ComNum.VBLF + " ENTDATE = SYSDATE  ";
                        SQL = SQL + ComNum.VBLF + "   WHERE ROWID = '" + ssView1_Sheet1.Cells[i, 9].Text.Trim()  + "' ";


                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, "NUR_BSTREMARK저장 중 에러가 발생했습니다.");
                            return false;
                        }
                    }

                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, "당뇨기록지를 저장 하는 도중 에러가 발생했습니다.");
                clsDB.SaveSqlErrLog(ex.Message, "BSTInterface - SaveData", clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
            }

            return rtnVal;
        }

        private bool SelectData()
        {
            bool rtnVal = true;
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

            //ssView1_Sheet1.SetColumnMerge(0, FarPoint.Win.Spread.Model.MergePolicy.Always);
            //ssView1_Sheet1.SetColumnMerge(1, FarPoint.Win.Spread.Model.MergePolicy.Always);
            //ssView1_Sheet1.SetColumnMerge(2, FarPoint.Win.Spread.Model.MergePolicy.Always);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                ssView1_Sheet1.RowCount = 0;

                SQL = "select to_char(MEASURE_DT,'yyyy-mm-dd') MEASURE_DT,WARD,remark1,remark2,remark3, BB.rowid, sum(decode(timeline,1,1,0)) timeline,sum(decode(timeline1,1,1,0)) timeline1,sum(decode(timeline2,1,1,0)) timeline2,sum(decode(timeline3,1,1,0)) timeline3 from ( ";
                SQL = SQL + ComNum.VBLF + " select      to_date(substr(MEASURE_DT,1,8),'yyyy-mm-dd') MEASURE_DT, ";
                SQL = SQL + ComNum.VBLF + "  1   timeline, ";
                SQL = SQL + ComNum.VBLF + " case  when   substr(MEASURE_DT,9,2) >= '00' and substr(MEASURE_DT,9,2) <= '09'  then 1 end  timeline1, ";
                SQL = SQL + ComNum.VBLF + " case  when   substr(MEASURE_DT,9,2) >= '10' and substr(MEASURE_DT,9,2) <= '13'  then 1 end  timeline2, ";
                SQL = SQL + ComNum.VBLF + " case  when   substr(MEASURE_DT,9,2) >= '14' and substr(MEASURE_DT,9,2) <= '23'  then 1 end  timeline3, ";
                SQL = SQL + ComNum.VBLF + " substr(WARD,1,2) WARD ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.EXAM_INTERFACE_BST ";
                SQL = SQL + ComNum.VBLF + "WHERE MEASURE_DT >= '" + dtpFDate.Value.ToString("yyyyMMdd") + "000000'";
                SQL = SQL + ComNum.VBLF + "  AND MEASURE_DT <= '" + dtpTDate.Value.ToString("yyyyMMdd") + "235959'";
                SQL = SQL + ComNum.VBLF + "  AND WARD LIKE '" + VB.Left(cboWard.Text.Trim(), 2) + "%'";
                SQL = SQL + ComNum.VBLF + "  AND KIND IS NULL ) AA , NUR_BSTREMARK BB";
                SQL = SQL + ComNum.VBLF + "     WHERE aa.MEASURE_DT =BB.bdate(+)";
                SQL = SQL + ComNum.VBLF + "     AND AA.WARD = BB.wardcode(+)";
                SQL = SQL + ComNum.VBLF + "  group by MEASURE_DT, remark1,remark2,remark3,WARD,BB.rowid order by MEASURE_DT";


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false; 
                }

                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    //

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["MEASURE_DT"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["timeline1"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["timeline2"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["timeline3"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["timeline"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 5].Text = string.Format("{0:#,###}", (VB.Val(txtEtc.Text.Trim()) - VB.Val(dt.Rows[i]["timeline"].ToString().Trim())));
                        ssView1_Sheet1.Cells[i, 6].Text = " " + dt.Rows[i]["remark1"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 7].Text = " " + dt.Rows[i]["remark2"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 8].Text = " " + dt.Rows[i]["remark3"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["rowid"].ToString().Trim();


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
                    return false;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void optOut_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
          

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            strTitle = "** "+ VB.Left(cboWard.Text.Trim(), 2) + "병동 BST 시행 건수 **" + "\r\n\r\n";

            strHeader = SP.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strHeader += SP.setSpdPrint_String(" 시행일자: " + dtpFDate.Text +" ~ "+ dtpTDate.Text, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(2, 10, 5, 10, 5, 10);
            setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Landscape, FarPoint.Win.Spread.PrintType.All, 0, 0, true, false, true, true, true, false, false);

            SP.setSpdPrint(ssView1, true, setMargin, setOption, strHeader, strFooter);

            MessageBox.Show("출력 되었습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (SaveData())
            {
                MessageBox.Show("저장이 완료 되었습니다.");

            }
            Cursor.Current = Cursors.Default;

            Cursor.Current = Cursors.WaitCursor;
            if (SelectData())
            {
                
            }
            Cursor.Current = Cursors.Default;
        }
    }
}
