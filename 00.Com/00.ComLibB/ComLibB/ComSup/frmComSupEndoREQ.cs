using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupEndoREQ.cs
    /// Description     : 내시경 약품 청구
    /// Author          : 이정현
    /// Create Date     : 2017-10-31
    /// <history> 
    /// 내시경 약품 청구
    /// </history>
    /// <seealso>
    /// PSMH\drug\drmain\FrmEndo_REQ.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drmain\drmain.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupEndoREQ : Form
    {
        public frmComSupEndoREQ()
        {
            InitializeComponent();
        }

        private void frmComSupEndoREQ_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            GetData();
            READ_ENDOJEP();
            panJep.Visible = true;
        }

        private void READ_ENDOJEP()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            
            ssJep_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     JEPCODE, COMMENTS, ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_SETCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '02' ";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND VFLAG1 = '1' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY JEPCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssJep_Sheet1.RowCount = dt.Rows.Count + 5;
                    ssJep_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssJep_Sheet1.Cells[i, 0].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                        ssJep_Sheet1.Cells[i, 1].Text = dt.Rows[i]["COMMENTS"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnJepADD_Click(object sender, EventArgs e)
        {
            READ_ENDOJEP();
            panJep.Visible = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     BDATE, ORDERCODE, ORDERNO, REQDATE, REQQTY, REQSABUN, OUTDATE, BIGO, ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_REQENDO ";
                SQL = SQL + ComNum.VBLF + "     WHERE BDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (rdoMiChulgo.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND OUTDATE IS NULL ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY BDATE, ORDERCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[i]["ORDERCODE"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 3].Text = Convert.ToDateTime(dt.Rows[i]["BDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["REQQTY"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = "1";
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["BIGO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == true)
            {
                panJep.Visible = true;
            }
        }

        private bool SaveData()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strBDate = "";
            string strORDERCODE = "";
            string strORDERNO = "";
            string strPtNo = "";
            string strSname = "";
            string strREQDATE = "";
            string strREQQTY = "";
            string strREQSABUN = "";
            string strBIGO = "";
            string strROWID = "";

            strREQSABUN = ComFunc.LPAD(clsType.User.Sabun, 5, "0");
            strREQDATE = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (i == 0)
                    {
                        strORDERCODE = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    }
                    else
                    {
                        strORDERCODE = (ssView_Sheet1.Cells[i, 1].Text.Trim() != "" ? ssView_Sheet1.Cells[i, 1].Text.Trim() : strORDERCODE);
                    }

                    strBDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                    strPtNo = ssView_Sheet1.Cells[i, 4].Text.Trim();
                    strSname = ssView_Sheet1.Cells[i, 5].Text.Trim();
                    strREQQTY = ssView_Sheet1.Cells[i, 6].Text.Trim();
                    strBIGO = ssView_Sheet1.Cells[i, 8].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i, 9].Text.Trim();
                    strORDERNO = ssView_Sheet1.Cells[i, 10].Text.Trim();

                    if (ssView_Sheet1.Cells[i, 11].Text.Trim() == "")
                    {
                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_ERP + "DRUG_REQENDO";
                            SQL = SQL + ComNum.VBLF + "     SET ";
                            SQL = SQL + ComNum.VBLF + "         REQQTY = " + strREQQTY + ", ";
                            SQL = SQL + ComNum.VBLF + "         BIGO = '" + strBIGO + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND OUTDATE IS NULL ";
                        }
                        else
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_REQENDO";
                            SQL = SQL + ComNum.VBLF + "     (BDATE, ORDERCODE, PTNO, SNAME, REQDATE, REQQTY, REQSABUN, BIGO)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strBDate + "','YYYY-MM-DD'), ";
                            SQL = SQL + ComNum.VBLF + "         '" + strORDERCODE + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strPtNo + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strSname + "', ";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strREQDATE + "','YYYY-MM-DD'), ";
                            SQL = SQL + ComNum.VBLF + "         " + strREQQTY + ", ";
                            SQL = SQL + ComNum.VBLF + "         '" + strREQSABUN + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strBIGO + "' ";
                            SQL = SQL + ComNum.VBLF + "     )";
                        }
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssJep_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssJep_Sheet1.RowCount == 0) { return; }
            if (e.ColumnHeader == true) { return; }
            if (ssJep_Sheet1.Cells[e.Row, 0].Text.Trim() == "") { return; }

            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
            ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = ssJep_Sheet1.Cells[e.Row, 0].Text.Trim();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = ssJep_Sheet1.Cells[e.Row, 1].Text.Trim();

            panJep.Visible = false;
        }

        private void ssView_EditModeOff(object sender, EventArgs e)
        {
            ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 13].Text = "Y";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            panJep.Visible = false;
        }
    }
}
