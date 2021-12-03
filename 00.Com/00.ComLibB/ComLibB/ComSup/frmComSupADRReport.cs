using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupADRReport.cs
    /// Description     : 약물이상반응(ADR) 발생 보고서
    /// Author          : 이정현
    /// Create Date     : 2018-01-15
    /// <history> 
    /// 약물이상반응(ADR) 발생 보고서
    /// </history>
    /// <seealso>
    /// PSMH\drug\dradr\FrmMain.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\dradr\dradr.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupADRReport : Form
    {
        private string GstrOPTION = "";
        private string GstrSEQNO = "";
        private string GstrPANO = "";
        private string GstrDeptCode = "";
        private string GstrIO = "";

        public frmComSupADRReport()
        {
            InitializeComponent();
        }

        private void frmComSupADRReport_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpSDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddMonths(-1);
            dtpEDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            ssView_Sheet1.RowCount = 0;

            if (clsType.User.BuseCode == "044101")
            {
                btnExcel.Visible = true;
            }

            SetCbo();
            GetData();
        }

        private void SetCbo()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboDept.Text = "";
            cboDept.Items.Clear();
            cboDept.Items.Add("전체");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DeptCode";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ClinicDept ";
                SQL = SQL + ComNum.VBLF + "     WHERE DeptCode NOT IN ('II','HR','TO','R6','HD','PT','AN') ";  //2005-08-09 ER제외
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboDept.SelectedIndex = 0;

                Cursor.Current = Cursors.Default;
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void clearADRVariable()
        {
            GstrOPTION = "";
            GstrSEQNO = "";
            GstrPANO = "";
            GstrDeptCode = "";
            GstrOPTION = "";
            GstrIO = "";
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            clearADRVariable();

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PTNO, SNAME, AGESEX, PATIENT_BUN, ROOMCODE, DEPTCODE, DIAGNAME, ALLERGY, SEQNO, IPDNO, GUBUN, WDATE, TEMP";
                SQL = SQL + ComNum.VBLF + "FROM ";
                SQL = SQL + ComNum.VBLF + "     (SELECT";
                SQL = SQL + ComNum.VBLF + "         PTNO, SNAME, AGESEX, PATIENT_BUN, ROOMCODE, DEPTCODE, DIAGNAME, ALLERGY, SEQNO, IPDNO, 'F' AS GUBUN, WDATE, TEMP";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         WHERE WDATE >= TO_DATE('" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "             AND WDATE <= TO_DATE('" + dtpEDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (cboDept.Text.Trim() != "전체")
                {
                    SQL = SQL + ComNum.VBLF + "             AND DEPTCODE = '" + cboDept.Text.Trim() + "' ";
                }

                SQL = SQL + ComNum.VBLF + "     UNION ALL";
                SQL = SQL + ComNum.VBLF + "     SELECT";
                SQL = SQL + ComNum.VBLF + "         PTNO, SNAME, AGESEX, PATIENT_BUN, '' AS ROOMCODE, '' AS DEPTCODE, '' AS DIAGNAME, '' AS ALLERGY, SEQNO, 0 AS IPDNO, 'S' AS GUBUN, WDATE, '' AS TEMP  ";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_ERP + "DRUG_ADR_SIMPLE ";
                SQL = SQL + ComNum.VBLF + "         WHERE WDATE >= TO_DATE('" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "             AND WDATE <= TO_DATE('" + dtpEDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                //if (cboDept.Text.Trim() != "전체")
                //{
                //    SQL = SQL + ComNum.VBLF + "             AND DEPTCODE = '" + cboDept.Text.Trim() + "' ";
                //}

                SQL = SQL + ComNum.VBLF + "     )";
                SQL = SQL + ComNum.VBLF + "ORDER BY WDATE ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT * 2);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PATIENT_BUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["AGESEX"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DIAGNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = ReadADRDrug(dt.Rows[i]["SEQNO"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 13].Text = ReadRelation(dt.Rows[i]["SEQNO"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 14].Text = ReadAlert(dt.Rows[i]["SEQNO"].ToString().Trim());

                        setADRProgress(dt.Rows[i]["SEQNO"].ToString().Trim(), dt.Rows[i]["GUBUN"].ToString().Trim(), i);

                        if (dt.Rows[i]["TEMP"].ToString().Trim() == "Y")
                        {
                            ssView_Sheet1.Cells[i, 11].ColumnSpan = 5;
                            ssView_Sheet1.Cells[i, 11].Text = "임시 저장된 보고서 입니다.";
                        }
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
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private string ReadADRDrug(string strSEQNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            
            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUCODE, SUNAMEK";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR1_ORDER ";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUCODE, SUNAMEK";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR1_ORDER_JO ";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            rtnVal = dt.Rows[i]["SUCODE"].ToString().Trim();
                        }
                        else
                        {
                            rtnVal += ComNum.VBLF + dt.Rows[i]["SUCODE"].ToString().Trim();
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
                return rtnVal;
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
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private string ReadRelation(string strSEQNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     RELATION1, RELATION2, RELATION3, RELATION4";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR3";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["RELATION1"].ToString().Trim() == "1")
                    {
                        rtnVal = "확실함" + ComNum.VBLF + "(Certain)";
                    }
                    else if (dt.Rows[0]["RELATION2"].ToString().Trim() == "1")
                    {
                        rtnVal = "상당히 확실함" + ComNum.VBLF + "(Probable)";
                    }
                    else if (dt.Rows[0]["RELATION3"].ToString().Trim() == "1")
                    {
                        rtnVal = "가능함" + ComNum.VBLF + "(Possible)";
                    }
                    else if (dt.Rows[0]["RELATION4"].ToString().Trim() == "1")
                    {
                        rtnVal = "가능성 적음" + ComNum.VBLF + "(Unlikely)";
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
                return rtnVal;
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
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private string ReadAlert(string strSEQNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SEQNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR2";
                SQL = SQL + ComNum.VBLF + "     WHERE (RELATION1 = '1' OR RELATION2 = '1' OR RELATION3 = '1')";
                SQL = SQL + ComNum.VBLF + "         AND SEQNO = " + strSEQNO;
                SQL = SQL + ComNum.VBLF + "Union All";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     SEQNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR3";
                SQL = SQL + ComNum.VBLF + "     WHERE (RELATION1 = '1' OR RELATION2 = '1' OR RELATION3 = '1')";
                SQL = SQL + ComNum.VBLF + "         AND SEQNO = " + strSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "Y";
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
                return rtnVal;
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
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void setADRProgress(string strSEQNO, string strGubun, int intRow)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (strGubun == "F")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.WDATE,'YYYY-MM-DD') AS WDATE1, A.WNAME AS WNAME1, A.TEMP AS TEMP1, ";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(B.WDATE,'YYYY-MM-DD') AS WDATE2, B.WNAME AS WNAME2, B.TEMP AS TEMP2, ";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(C.WDATE,'YYYY-MM-DD') AS WDATE3, C.WNAME AS WNAME3, C.TEMP AS TEMP3, ";
                    SQL = SQL + ComNum.VBLF + "     DECODE(TRIM(D.REPORT1 || D.REPORT2), '00', '', '01', '식약처 보고', '10', '위원회보고', '') AS ADR4";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR1 A,";
                    SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_ERP + "DRUG_ADR2 B,";
                    SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_ERP + "DRUG_ADR3 C,";
                    SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_ERP + "DRUG_ADR4 D";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.SEQNO = B.SEQNO(+)";
                    SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = C.SEQNO(+)";
                    SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = D.SEQNO(+)";
                    SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = " + strSEQNO;

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[intRow, 0].Text = dt.Rows[0]["WDATE1"].ToString().Trim();
                        ssView_Sheet1.Cells[intRow, 10].Text = dt.Rows[0]["WNAME1"].ToString().Trim();
                        ssView_Sheet1.Cells[intRow, 11].Text = dt.Rows[0]["WDATE2"].ToString().Trim() + dt.Rows[0]["WNAME2"].ToString().Trim();

                        if (dt.Rows[0]["TEMP2"].ToString().Trim() == "Y")
                        {
                            ssView_Sheet1.Cells[intRow, 12].ColumnSpan = 4;
                            ssView_Sheet1.Cells[intRow, 12].Text = "임시 저장된 보고서 입니다.";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[intRow, 12].Text = dt.Rows[0]["WDATE3"].ToString().Trim() + ComNum.VBLF + dt.Rows[0]["WNAME3"].ToString().Trim();

                            if (dt.Rows[0]["TEMP3"].ToString().Trim() == "Y")
                            {
                                ssView_Sheet1.Cells[intRow, 13].ColumnSpan = 3;
                                ssView_Sheet1.Cells[intRow, 13].Text = "임시 저장된 보고서 입니다.";
                            }
                            else
                            {
                                ssView_Sheet1.Cells[intRow, 15].Text = dt.Rows[0]["ADR4"].ToString().Trim();
                                ssView_Sheet1.Cells[intRow, 16].Text = "F";
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(WDATE,'YYYY-MM-DD') AS WDATE1, SNAME AS WNAME1 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR_SIMPLE";
                    SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[intRow, 0].Text = dt.Rows[0]["WDATE1"].ToString().Trim();
                        ssView_Sheet1.Cells[intRow, 10].Text = dt.Rows[0]["WNAME1"].ToString().Trim();
                        ssView_Sheet1.Cells[intRow, 10].ForeColor = Color.FromArgb(0, 0, 230);
                        ssView_Sheet1.Cells[intRow, 10].Font = new Font("맑은 고딕", 10f, FontStyle.Bold);

                        ssView_Sheet1.Cells[intRow, 16].Text = "S";
                    }

                    dt.Dispose();
                    dt = null;
                }
                
                Cursor.Current = Cursors.Default;
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) { return; }

            clearADRVariable();

            GstrPANO = ssView_Sheet1.Cells[e.Row, 2].Text.Trim();
            GstrDeptCode = ssView_Sheet1.Cells[e.Row, 4].Text.Trim();
            GstrOPTION = ssView_Sheet1.Cells[e.Row, 8].Text.Trim();

            switch (ssView_Sheet1.Cells[e.Row, 1].Text.Trim())
            {
                case "입원": GstrIO = "I"; break;
                case "외래": GstrIO = "O"; break;
            }

            GstrSEQNO = ssView_Sheet1.Cells[e.Row, 9].Text.Trim();

            if (GstrSEQNO == "")
            {
                ComFunc.MsgBox("발생보고서가 작성되지 않았습니다."
                    + ComNum.VBLF + "발생보고서를 작성하십시오.");
                clearADRVariable();
                return;
            }
        }



        private void btnNew_Click(object sender, EventArgs e)
        {
            clearADRVariable();

            if (ComFunc.MsgBoxQ("신규 보고서를 작성하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                frmComSupADR1 frm = new frmComSupADR1();
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }

            GetData();
        }

        private void btnADRSimple_Click(object sender, EventArgs e)
        {
            frmComSupADRSimple frm = new frmComSupADRSimple(GstrSEQNO);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void btnADR1_Click(object sender, EventArgs e)
        {
            if (GstrPANO == "") { ComFunc.MsgBox("대상자를 선택해주세요"); return; }

            using (frmComSupADR1 frm = new frmComSupADR1(GstrPANO, GstrDeptCode, GstrIO, GstrSEQNO, GstrOPTION))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
        }

        private void btnADR2_Click(object sender, EventArgs e)
        {
            if (GstrSEQNO == "") { ComFunc.MsgBox("대상자를 선택해주세요"); return; }

            using (frmComSupADR2 frm = new frmComSupADR2(GstrSEQNO, GstrPANO))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
        }

        private void btnADR3_Click(object sender, EventArgs e)
        {
            if (GstrSEQNO == "") { ComFunc.MsgBox("대상자를 선택해주세요"); return; }

            using (frmComSupADR3 frm = new frmComSupADR3(GstrSEQNO))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
        }

        private void btnADR4_Click(object sender, EventArgs e)
        {
            if (GstrSEQNO == "") { ComFunc.MsgBox("대상자를 선택해주세요"); return; }

            using (frmComSupADR4 frm = new frmComSupADR4(GstrSEQNO))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
        }


        private void btnPrtADR_Click(object sender, EventArgs e)
        {
            if (GstrSEQNO == "") { ComFunc.MsgBox("대상자를 선택해주세요"); return; }

            using (frmComSupPrtADR frm = new frmComSupPrtADR(GstrSEQNO))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
        }

        private void btnADRTong_Click(object sender, EventArgs e)
        {
            using (frmComSupAdrTong frm = new frmComSupAdrTong())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog mDlg = new SaveFileDialog())
            {
                mDlg.InitialDirectory = Application.StartupPath;
                mDlg.Filter = "Excel files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                mDlg.FilterIndex = 1;
                if (mDlg.ShowDialog() == DialogResult.OK)
                {
                    ssView.SaveExcel(mDlg.FileName,
                    FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
                }
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(e.Column <= 10) 
            {
                btnADR1_Click(sender,e);
            }
            else if (e.Column == 11)
            {
                btnADR2_Click(sender, e);
            }
            else if (e.Column ==12)
            {
                btnADR3_Click(sender, e);
            }

            else if (e.Column == 13)
            {
                btnADR4_Click(sender, e);
            }
            else if (e.Column == 15)
            {
                btnPrtADR_Click(sender, e);
            }

        }

        private void btnTongNew_Click(object sender, EventArgs e)
        {
            frmComSupAdrTongNew frm = new frmComSupAdrTongNew();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void btnEMR_Click(object sender, EventArgs e)
        {
            if(ssView.ActiveSheet.ActiveRowIndex < 0)
            {
                ComFunc.MsgBox("환자를 선택해주시기 바랍니다.");
                return;
            }
            else
            {
                frmEmrViewer f = new frmEmrViewer(ssView.ActiveSheet.Cells[ssView.ActiveSheet.ActiveRowIndex, 2].Text.Trim());
                f.ShowDialog();

                f.Dispose();
                f = null;
                clsApi.FlushMemory();
            }
        }
    }
}
