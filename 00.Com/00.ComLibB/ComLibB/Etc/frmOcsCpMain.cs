using System;
using System.Data;
using System.Windows.Forms;
using ComBase;
using System.IO;
using System.Drawing;
using FarPoint.Win.Spread;

namespace ComLibB
{
    public partial class frmOcsCpMain : Form, MainFormMessage
    { 
        private frmOcsCpInfo frmOcsCpInfoEvent = null;
        private frmOcsCpInfoRef frmOcsCpInfoRefEvent = null;

        #region //MainFormMessage
        string mPara1 = "";
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion //MainFormMessage


        public frmOcsCpMain()
        {
            InitializeComponent();
        }

        public frmOcsCpMain(string sPara1)
        {
            InitializeComponent();
            mPara1 = sPara1;
        }

        public frmOcsCpMain(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmOcsCpMain(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;

        }

        private void frmOcsCpMain_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmOcsCpMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmOcsCpInfoEvent != null)
            {
                frmOcsCpInfoEvent.Dispose();
                frmOcsCpInfoEvent = null;
            }

            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        private void frmOcsCpMain_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            InitForm();


            if (clsType.User.JobGroup != "JOB016002" && clsType.User.JobGroup != "JOB000001")
            {
                ssList_Sheet1.SetColumnAllowFilter(-1, true);

                btnClear.Enabled = false;
                btnSave.Enabled = false;
                //panContent.Visible = false;
                btnSaveUpload.Enabled = false;
                btnSaveUpload_1.Enabled = false;
                btnSaveUpload2.Enabled = false;
                btnSaveUpload2_1.Enabled = false;
                
            }


        }

        private void InitForm()
        {
            FormClear();

            ssList_Sheet1.RowCount = 0;
            ssHis_Sheet1.RowCount = 0;
            txtCPCODE.Text = "";
            txtCPNAME.Text = "";


            btnExcept       .BackColor = Color.LightGray;
            btnStop         .BackColor = Color.LightGray;
            btnINDICATOR    .BackColor = Color.LightGray;
            btnINDICATORSUB .BackColor = Color.LightGray;
            btnAGREE.BackColor = Color.LightGray;

            GetDataCpCode();
        }

        private void FormClear()
        {
            lblRowid.Text = "";
            txtSDATE.Text = "";
            txtEDATE.Text = "";

            dtpSDate.Text = "";
            dtpStopDate.Text = "";

            txtSCALE.Text = "";
            txtFRAGE.Text = "";
            txtTOAGE.Text = "";
            txtDay.Text = "";

            rdoIO0.Checked = true;
            rdoGUBUN0.Checked = true;
            rdoINDICATOR1.Checked = true;
            rdoAGREE1.Checked = true;

            ssILLCode_Sheet1.RowCount = 0;
            ssILLCode_Sheet1.RowCount = 1;
            ssILLCode_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            ssOpCode_Sheet1.RowCount = 0;
            ssOpCode_Sheet1.RowCount = 1;
            ssOpCode_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            ssExcept_Sheet1.RowCount = 0;
            ssStop_Sheet1.RowCount = 0;
            ssINDICATOR_Sheet1.RowCount = 0;
            ssAGREE_Sheet1.RowCount = 0;
        }

        private void GetDataCpCode()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "    BASCD, BASNAME, BASNAME1";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD";
                SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = 'CP관리' ";
                SQL = SQL + ComNum.VBLF + "    AND GRPCD = 'CP코드관리' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BASCD";

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
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASNAME1"].ToString().Trim();
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            FormClear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (SaveData() == true)
            {
                InitForm();
            }
        }

        private bool SaveData()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            int k = 0;

            string strSysDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            string strROWID = lblRowid.Text.Trim();
            string strCPCODE = txtCPCODE.Text.Trim();
            string strSDATE = dtpSDate.Value.ToString("yyyyMMdd");
            string strEDATE = dtpStopDate.Value.ToString("yyyyMMdd");
            string strCPNAME = txtCPNAME.Text.Trim();
            string strGBIO = rdoIO0.Checked == true ? "E" : "I";
            string strGUBUN = rdoGUBUN0.Checked == true ? "01" : rdoGUBUN1.Checked == true ? "02" : "03";
            string strSCALE = chkSCALE0.Checked == true ? "01" : chkSCALE1.Checked == true ? "02" : "03";
            string strSCALERMK = txtSCALE.Text.Trim();
            string strFRAGE = txtFRAGE.Text.Trim();
            string strTOAGE = txtTOAGE.Text.Trim();
            string strCPDAY = txtDay.Text.Trim();
            string strGBINDICATOR = rdoINDICATOR0.Checked == true ? "Y" : "N";
            string strGBAGREE = rdoAGREE0.Checked == true ? "Y" : "N";
            
            FarPoint.Win.Spread.FpSpread ssSpread = null;

            Cursor.Current = Cursors.WaitCursor;
            
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (lblRowid.Text.Trim() == "")
                {
                    strEDATE = "99981231";

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_CP_MAIN";
                    SQL = SQL + ComNum.VBLF + "    (CPCODE, SDATE, EDATE, GBIO, GUBUN, SCALE, SCALERMK, FRAGE, TOAGE, CPDAY, GBINDICATOR, GBAGREE)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "    (";
                    SQL = SQL + ComNum.VBLF + "         '" + strCPCODE + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSDATE + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEDATE + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGBIO + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGUBUN + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSCALE + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSCALERMK + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strFRAGE + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strTOAGE + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strCPDAY + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGBINDICATOR + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGBAGREE + "' ";
                    SQL = SQL + ComNum.VBLF + "    )";
                }
                else
                {
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_MED + "OCS_CP_MAIN";
                    SQL = SQL + ComNum.VBLF + "     SET";
                    SQL = SQL + ComNum.VBLF + "         GBIO = '" + strGBIO + "', ";
                    SQL = SQL + ComNum.VBLF + "         GUBUN = '" + strGUBUN + "', ";
                    SQL = SQL + ComNum.VBLF + "         SCALE = '" + strSCALE + "', ";
                    SQL = SQL + ComNum.VBLF + "         SCALERMK = '" + strSCALERMK + "', ";
                    SQL = SQL + ComNum.VBLF + "         FRAGE = '" + strFRAGE + "', ";
                    SQL = SQL + ComNum.VBLF + "         TOAGE = '" + strTOAGE + "', ";
                    SQL = SQL + ComNum.VBLF + "         CPDAY = '" + strCPDAY + "', ";
                    SQL = SQL + ComNum.VBLF + "         GBINDICATOR = '" + strGBINDICATOR + "', ";
                    SQL = SQL + ComNum.VBLF + "         GBAGREE = '" + strGBAGREE + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
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

                SQL = "";
                SQL = "DELETE " + ComNum.DB_MED + "OCS_CP_SUB";
                SQL = SQL + ComNum.VBLF + "WHERE CPCODE = '" + strCPCODE + "' ";
                SQL = SQL + ComNum.VBLF + "    AND SDATE = '" + strSDATE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (i = 1; i <= 7; i++)
                {
                    string strTYPE = "";
                    string strSCODE = "";
                    int intValue = 0;

                    if (i == 1)
                    {
                        if (chkBI0.Checked == false && chkBI1.Checked == false) { continue; }

                        if (chkBI0.Checked == true)
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_CP_SUB";
                            SQL = SQL + ComNum.VBLF + "    (CPCODE, SDATE, GUBUN, CODE, NAME, REMARK, TYPE, SCODE, CPVALUE, DSPSEQ)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "    (";
                            SQL = SQL + ComNum.VBLF + "         '" + strCPCODE + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strSDATE + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + i.ToString("00") + "', ";
                            SQL = SQL + ComNum.VBLF + "         '01', ";
                            SQL = SQL + ComNum.VBLF + "         '" + chkBI0.Text + "', ";
                            SQL = SQL + ComNum.VBLF + "         '', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strTYPE + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strSCODE + "', ";
                            SQL = SQL + ComNum.VBLF + "         " + intValue + ", ";
                            SQL = SQL + ComNum.VBLF + "         0 ";
                            SQL = SQL + ComNum.VBLF + "    )";

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

                        if (chkBI1.Checked == true)
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_CP_SUB";
                            SQL = SQL + ComNum.VBLF + "    (CPCODE, SDATE, GUBUN, CODE, NAME, REMARK, TYPE, SCODE, CPVALUE, DSPSEQ)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "    (";
                            SQL = SQL + ComNum.VBLF + "         '" + strCPCODE + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strSDATE + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + i.ToString("00") + "', ";
                            SQL = SQL + ComNum.VBLF + "         '02', ";
                            SQL = SQL + ComNum.VBLF + "         '" + chkBI1.Text + "', ";
                            SQL = SQL + ComNum.VBLF + "         '', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strTYPE + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strSCODE + "', ";
                            SQL = SQL + ComNum.VBLF + "         " + intValue + ", ";
                            SQL = SQL + ComNum.VBLF + "         0 ";
                            SQL = SQL + ComNum.VBLF + "    )";

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
                    }
                    else
                    {
                        if (i == 2)
                        {
                            if (ssStop_Sheet1.RowCount == 0) { continue; }
                            ssSpread = ssStop;
                        }
                        else if (i == 3)
                        {
                            if (ssExcept_Sheet1.RowCount == 0) { continue; }
                            ssSpread = ssExcept;
                        }
                        else if (i == 4)
                        {
                            if (ssILLCode_Sheet1.NonEmptyRowCount == 0) { continue; }
                            ssSpread = ssILLCode;
                        }
                        else if (i == 5)
                        {
                            if (ssOpCode_Sheet1.NonEmptyRowCount == 0) { continue; }
                            ssSpread = ssOpCode;
                        }
                        else if (i == 6)
                        {
                            if (ssINDICATOR_Sheet1.RowCount == 0) { continue; }
                            ssSpread = ssINDICATOR;
                        }
                        else if (i == 7)
                        {
                            if (ssAGREE_Sheet1.RowCount == 0) { continue; }
                            ssSpread = ssAGREE;
                        }

                        for (k = 0; k < ssSpread.ActiveSheet.RowCount; k++)
                        {
                            if (ssSpread == ssINDICATOR)
                            {
                                strTYPE = ssSpread.ActiveSheet.Cells[k, 2].Text.Trim();
                                strSCODE = ssSpread.ActiveSheet.Cells[k, 4].Text.Trim();
                                intValue = (int)VB.Val(ssSpread.ActiveSheet.Cells[k, 7].Text.Trim());
                            }

                            if (ssSpread.ActiveSheet.Cells[k, 0].Text.Trim() != "")
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_CP_SUB";
                                SQL = SQL + ComNum.VBLF + "    (CPCODE, SDATE, GUBUN, CODE, NAME, REMARK, TYPE, SCODE, CPVALUE, INPUTGBC, INPUTGBS, DSPSEQ)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "    (";
                                SQL = SQL + ComNum.VBLF + "         '" + strCPCODE + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + strSDATE + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + i.ToString("00") + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + ssSpread.ActiveSheet.Cells[k, 0].Text.Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + ssSpread.ActiveSheet.Cells[k, 1].Text.Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '', ";
                                SQL = SQL + ComNum.VBLF + "         '" + strTYPE + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + strSCODE + "', ";
                                SQL = SQL + ComNum.VBLF + "         " + intValue + ", ";

                                SQL = SQL + ComNum.VBLF + "         '" + (ssSpread.ActiveSheet.ColumnCount > 3 ? ssSpread.ActiveSheet.Cells[k, 3].Text.Trim() : "") + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + (ssSpread.ActiveSheet.ColumnCount > 6 ? ssSpread.ActiveSheet.Cells[k, 6].Text.Trim() : "") + "', ";
                                
                                if (ssSpread.ActiveSheet.ColumnCount < 8)
                                {
                                    SQL = SQL + ComNum.VBLF + "         " + (k + 1);
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + "         " + (VB.Val(ssSpread.ActiveSheet.Cells[k, 8].Text.Trim()) + 1);
                                }
                                SQL = SQL + ComNum.VBLF + "    )";

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
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            if (lblRowid.Text.Trim() == "") { return; }

            if (UpdateData() == true)
            {
                InitForm();
            }
        }

        private bool UpdateData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strSysDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_BASCD";
                SQL = SQL + ComNum.VBLF + "     SET DELDATE = TO_DATE('" + strSysDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + lblRowid.Text.Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
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

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0) return;
            if (e.ColumnHeader == true) return;

            FormClear();

            string strCPCODE = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();
            string strCPNAME = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();

            
            panIndicator.Enabled = ssList_Sheet1.Cells[e.Row, 2].Text.Trim() != "IPD";

            txtCPCODE.Text = strCPCODE.Trim();
            txtCPNAME.Text = strCPNAME.Trim();

            GetDataCpHis(strCPCODE, strCPNAME);

        }

        private void GetDataCpHis(string strCPCODE, string strCPNAME)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssHis_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.SDATE, A.EDATE, A.CPCODE, A.ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_CP_MAIN A";
                SQL = SQL + ComNum.VBLF + "WHERE A.CPCODE = '" + strCPCODE + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.EDATE > TO_CHAR(SYSDATE, 'YYYYMMDD')";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.SDATE DESC ";

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
                    ssHis_Sheet1.RowCount = dt.Rows.Count;
                    ssHis_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssHis_Sheet1.Cells[i, 0].Text = ComFunc.FormatStrToDate(dt.Rows[i]["SDATE"].ToString().Trim(),"D");
                        ssHis_Sheet1.Cells[i, 1].Text = ComFunc.FormatStrToDate(dt.Rows[i]["EDATE"].ToString().Trim(), "D");
                        ssHis_Sheet1.Cells[i, 2].Text = dt.Rows[i]["CPCODE"].ToString().Trim();
                        ssHis_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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
        
        private void chkSCALE2_CheckedChanged(object sender, EventArgs e)
        {
            txtSCALE.Visible = chkSCALE2.Checked;
        }

        private void btnOcsCpInfo_Click(object sender, EventArgs e)
        {
            string strGubun = ((Button)sender).Tag.ToString();

            frmOcsCpInfoEvent = new frmOcsCpInfo(strGubun);
            frmOcsCpInfoEvent.SendEvent += FrmOcsCpInfoEvent_SendEvent;
            frmOcsCpInfoEvent.rEventClosed += FrmOcsCpInfoEvent_rEventClosed;
            frmOcsCpInfoEvent.StartPosition = FormStartPosition.CenterParent;
            frmOcsCpInfoEvent.ShowDialog();
        }


        private void FrmOcsCpInfoEvent_rEventClosed()
        {
            frmOcsCpInfoEvent.Dispose();
            frmOcsCpInfoEvent = null;
        }

        private void FrmOcsCpInfoEvent_SendEvent(string strGubun, string strCode, string strName)
        {
            FarPoint.Win.Spread.FpSpread mSpread = null;

            switch (strGubun)
            {
                case "CP제외기준":
                    mSpread = ssExcept;
                    break;
                case "CP중단사유":
                    mSpread = ssStop;
                    break;
                case "CP지표":
                    mSpread = ssINDICATOR;
                    break;
                case "CP동의서":
                    mSpread = ssAGREE;
                    break;
            }

            mSpread.ActiveSheet.RowCount = mSpread.ActiveSheet.RowCount + 1;
            mSpread.ActiveSheet.SetRowHeight(mSpread.ActiveSheet.RowCount - 1, ComNum.SPDROWHT);

            mSpread.ActiveSheet.Cells[mSpread.ActiveSheet.RowCount - 1, 0].Text = strCode;
            mSpread.ActiveSheet.Cells[mSpread.ActiveSheet.RowCount - 1, 1].Text = strName;
        }

        private void btnINDICATORSUB_Click(object sender, EventArgs e)
        {
            string strGubun = "";

            frmOcsCpInfoRefEvent = new frmOcsCpInfoRef(strGubun);
            frmOcsCpInfoRefEvent.rSendEvent += frmOcsCpInfoRefEvent_SendEvent;
            frmOcsCpInfoRefEvent.rEventClosed += frmOcsCpInfoRefEvent_rEventClosed;
            frmOcsCpInfoRefEvent.StartPosition = FormStartPosition.CenterParent;
            frmOcsCpInfoRefEvent.ShowDialog();
        }


        private void frmOcsCpInfoRefEvent_rEventClosed()
        {
            frmOcsCpInfoRefEvent.Dispose();
            frmOcsCpInfoRefEvent = null;
        }

        private void frmOcsCpInfoRefEvent_SendEvent(string strCode, string strName)
        {
            frmOcsCpInfoRefEvent.Dispose();
            frmOcsCpInfoRefEvent = null;
            ssINDICATOR.ActiveSheet.Cells[ssINDICATOR.ActiveSheet.ActiveRowIndex, 4].Text = strCode;
            ssINDICATOR.ActiveSheet.Cells[ssINDICATOR.ActiveSheet.ActiveRowIndex, 5].Text = strName;
        }

        private void ssILLCode_EditModeOff(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strCode = ssILLCode_Sheet1.Cells[ssILLCode_Sheet1.ActiveRowIndex, 0].Text.ToUpper().Trim();

            ssILLCode_Sheet1.Cells[ssILLCode_Sheet1.ActiveRowIndex, 0].Text = strCode;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     ILLCODE, ILLNAMEK, ILLNAMEE, ";
                SQL = SQL + ComNum.VBLF + "     nvl(ILLNAMEK, ILLNAMEE) AS ILLNAME, NOUSE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ILLS ";
                SQL = SQL + ComNum.VBLF + "     WHERE IllClass = '1' ";
                SQL = SQL + ComNum.VBLF + "         AND (NOUSE <> 'N' OR NOUSE IS NULL) ";
                SQL = SQL + ComNum.VBLF + "         AND DDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "         AND (UPPER(ILLCODE) = '" + strCode + "' ";
                SQL = SQL + ComNum.VBLF + "             OR UPPER(ILLNAMEK) = '" + strCode + "' ";
                SQL = SQL + ComNum.VBLF + "             OR UPPER(ILLNAMEE) = '" + strCode + "') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY ILLCODE, ILLNAMEE ";

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
                    ssILLCode_Sheet1.Cells[ssILLCode_Sheet1.ActiveRowIndex, 1].Text = dt.Rows[0]["ILLNAMEK"].ToString().Trim();

                    if (ssILLCode_Sheet1.ActiveRowIndex == ssILLCode_Sheet1.RowCount - 1)
                    {
                        ssILLCode_Sheet1.RowCount = ssILLCode_Sheet1.RowCount + 1;
                        ssILLCode_Sheet1.SetRowHeight(ssILLCode_Sheet1.RowCount - 1, ComNum.SPDROWHT);
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
              
        private void ssSpread_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true) { return; }

            if (e.Column == 1)
            {
                if (ComFunc.MsgBoxQ("삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {
                    ((FarPoint.Win.Spread.FpSpread)sender).ActiveSheet.RemoveRows(e.Row, 1);
                }
            }
        }

        private void ssHis_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssHis_Sheet1.RowCount == 0) return;
            if (e.ColumnHeader == true) return;

            FormClear();

            string strSDATE = ssHis_Sheet1.Cells[e.Row, 0].Text.Trim();
            string strEDATE = ssHis_Sheet1.Cells[e.Row, 1].Text.Trim();
            string strCPCODE = ssHis_Sheet1.Cells[e.Row, 2].Text.Trim();
            string strROWID = ssHis_Sheet1.Cells[e.Row, 3].Text.Trim();

            txtCPCODE.Text = strCPCODE;
            lblRowid.Text = strROWID;
            txtSDATE.Text = strSDATE;
            txtEDATE.Text = strEDATE;
            dtpSDate.Value = Convert.ToDateTime(strSDATE);
            dtpStopDate.Value = Convert.ToDateTime(strEDATE);

            ssHisCellDoubleClick(strROWID);

        }

        private void ssHisCellDoubleClick(string strROWID)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int k = 0;

            string strCPCODE = "";
            string strSDATE = "";

            FarPoint.Win.Spread.FpSpread ssSpread = null;
            
            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "    CPCODE, SDATE, EDATE, GBIO, GUBUN, SCALE, SCALERMK, FRAGE, TOAGE, CPDAY, GBINDICATOR, GBAGREE, OCSEDUFILE,OCSEDUFILE1, PATEDUFILE,PATEDUFILE1";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_CP_MAIN ";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strROWID.Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("데이타가 없습니다.");
                    Cursor.Current = Cursors.Default;
                }

                strCPCODE = dt.Rows[0]["CPCODE"].ToString().Trim();
                strSDATE = dt.Rows[0]["SDATE"].ToString().Trim();

                if (dt.Rows[0]["GBIO"].ToString().Trim() == "E") { rdoIO0.Checked = true; }
                else if (dt.Rows[0]["GBIO"].ToString().Trim() == "I") { rdoIO1.Checked = true; }

                if (dt.Rows[0]["GUBUN"].ToString().Trim() == "01") { rdoGUBUN0.Checked = true; }
                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "02") { rdoGUBUN1.Checked = true; }
                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "03") { rdoGUBUN2.Checked = true; }

                if (dt.Rows[0]["SCALE"].ToString().Trim() == "01") { chkSCALE0.Checked = true; }
                else if (dt.Rows[0]["SCALE"].ToString().Trim() == "02") { chkSCALE1.Checked = true; }
                else if (dt.Rows[0]["SCALE"].ToString().Trim() == "03")
                {
                    chkSCALE2.Checked = true;
                    txtSCALE.Text = dt.Rows[0]["SCALERMK"].ToString().Trim();
                }

                txtFRAGE.Text = dt.Rows[0]["FRAGE"].ToString().Trim();
                txtTOAGE.Text = dt.Rows[0]["TOAGE"].ToString().Trim();
                txtDay.Text = dt.Rows[0]["CPDAY"].ToString().Trim();

                if (dt.Rows[0]["OCSEDUFILE"].ToString().Trim() != "")
                {
                    btnSearchUpload.BackColor = Color.DeepSkyBlue;
                }
                else
                {
                    btnSearchUpload.BackColor = Color.Transparent;
                }
                if (dt.Rows[0]["OCSEDUFILE1"].ToString().Trim() != "")
                {
                    btnSearchUpload_1.BackColor = Color.DeepSkyBlue;
                }
                else
                {
                    btnSearchUpload_1.BackColor = Color.Transparent;
                }


                if (dt.Rows[0]["PATEDUFILE"].ToString().Trim() != "")
                {
                    btnSearchUpload2.BackColor = Color.DeepSkyBlue;
                }
                else
                {
                    btnSearchUpload2.BackColor = Color.Transparent;
                }
                if (dt.Rows[0]["PATEDUFILE1"].ToString().Trim() != "")
                {
                    btnSearchUpload2_1.BackColor = Color.DeepSkyBlue;
                }
                else
                {
                    btnSearchUpload2_1.BackColor = Color.Transparent;
                }

                if (dt.Rows[0]["GBINDICATOR"].ToString().Trim() == "Y") { rdoINDICATOR0.Checked = true; }
                else { rdoINDICATOR1.Checked = true; }

                if (dt.Rows[0]["GBAGREE"].ToString().Trim() == "Y") { rdoAGREE0.Checked = true; }
                else { rdoAGREE1.Checked = true; }

                dt.Dispose();
                dt = null;

                for (i = 1; i <= 7; i++)
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     CODE, NAME, REMARK, TYPE, SCODE, CPVALUE, INPUTGBC, INPUTGBS, DSPSEQ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_CP_SUB";
                    SQL = SQL + ComNum.VBLF + "     WHERE CPCODE = '" + strCPCODE + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND SDATE = '" + strSDATE + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND GUBUN = '" + i.ToString("00") + "' ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY  DSPSEQ";

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
                        if (i == 1)
                        {
                            for (k = 0; k < dt.Rows.Count; k++)
                            {
                                if (dt.Rows[k]["CODE"].ToString().Trim() == "01") { chkBI0.Checked = true; }
                                if (dt.Rows[k]["CODE"].ToString().Trim() == "02") { chkBI1.Checked = true; }
                            }

                            continue;
                        }
                        else if (i == 2) { ssSpread = ssStop; }
                        else if (i == 3) { ssSpread = ssExcept; }
                        else if (i == 4) { ssSpread = ssILLCode; }
                        else if (i == 5) { ssSpread = ssOpCode; }
                        else if (i == 6) { ssSpread = ssINDICATOR; }
                        else if (i == 7) { ssSpread = ssAGREE; }

                        if (ssSpread == ssILLCode || ssSpread == ssOpCode)
                        {
                            ssSpread.ActiveSheet.RowCount = dt.Rows.Count + 1;
                        }
                        else
                        {
                            ssSpread.ActiveSheet.RowCount = dt.Rows.Count;
                        }
                        ssSpread.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                        for (k = 0; k < dt.Rows.Count; k++)
                        {
                            ssSpread.ActiveSheet.Cells[k, 0].Text = dt.Rows[k]["CODE"].ToString().Trim();
                            ssSpread.ActiveSheet.Cells[k, 1].Text = dt.Rows[k]["NAME"].ToString().Trim();
                            if (ssSpread == ssINDICATOR)
                            {
                                ssSpread.ActiveSheet.Cells[k, 2].Text = dt.Rows[k]["TYPE"].ToString().Trim();
                                ssSpread.ActiveSheet.Cells[k, 3].Text = dt.Rows[k]["INPUTGBC"].ToString().Trim();
                                ssSpread.ActiveSheet.Cells[k, 4].Text = dt.Rows[k]["SCODE"].ToString().Trim();
                                ssSpread.ActiveSheet.Cells[k, 6].Text = dt.Rows[k]["INPUTGBS"].ToString().Trim();
                                ssSpread.ActiveSheet.Cells[k, 8].Text = dt.Rows[k]["DSPSEQ"].ToString().Trim();
                                if (VB.Val(dt.Rows[k]["CPVALUE"].ToString().Trim()) != 0)
                                {
                                    ssSpread.ActiveSheet.Cells[k, 7].Text = dt.Rows[k]["CPVALUE"].ToString().Trim();
                                }

                                if (dt.Rows[k]["SCODE"].ToString().Trim() != "")
                                {
                                    DataTable dt1 = null;
                                    SQL = "";
                                    SQL = "SELECT";
                                    SQL = SQL + ComNum.VBLF + "    BASCD, BASNAME";
                                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD";
                                    SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = 'CP관리' ";
                                    SQL = SQL + ComNum.VBLF + "    AND GRPCD = 'CP지표참조' ";
                                    SQL = SQL + ComNum.VBLF + "    AND BASCD = '" + dt.Rows[k]["SCODE"].ToString().Trim() + "' ";

                                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        return;
                                    }
                                    if (dt1.Rows.Count > 0)
                                    {
                                        ssSpread.ActiveSheet.Cells[k, 5].Text = dt1.Rows[0]["BASNAME"].ToString().Trim();
                                    }
                                    else
                                    {
                                        ssSpread.ActiveSheet.Cells[k, 5].Text = "";
                                    }
                                    dt1.Dispose();
                                    dt1 = null;
                                }
                            }
                        }
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

        private void btnDeleteSub_Click(object sender, EventArgs e)
        {
            if (ssINDICATOR_Sheet1.RowCount == 0) return;

            ssINDICATOR_Sheet1.Cells[ssINDICATOR_Sheet1.ActiveRowIndex, 3].Text = "";
            ssINDICATOR_Sheet1.Cells[ssINDICATOR_Sheet1.ActiveRowIndex, 4].Text = "";
            ssINDICATOR_Sheet1.Cells[ssINDICATOR_Sheet1.ActiveRowIndex, 5].Text = "";
        }

        private void lblOpCode_DoubleClick(object sender, EventArgs e)
        {
            lblOpCode.BackColor = Color.Silver;
            frmOcsCpOperationCodeSearch frm = new frmOcsCpOperationCodeSearch();
            frm.rSendMsg += frm_OpCodeSendMsg;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
            frm.Dispose();
            frm = null;
            lblOpCode.BackColor = Color.White;
        }

        private void frm_OpCodeSendMsg(string strCode, string strName)
        {
            ssOpCode_Sheet1.Rows.Add(0, 1);
            ssOpCode_Sheet1.Cells[0, 0].Text = strCode;
            ssOpCode_Sheet1.Cells[0, 1].Text = strName;
        }

        private void lblDiag_DoubleClick(object sender, EventArgs e)
        {
            frmOcsCpDiagnosisCodeSearch frm = new frmOcsCpDiagnosisCodeSearch();
            frm.rSendMsg += frm_DiagnosisCodeSendMsg;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
            frm.Dispose();
            frm = null;
        }

        private void frm_DiagnosisCodeSendMsg(string strCode, string strName)
        {
            ssILLCode_Sheet1.Rows.Add(0, 1);
            ssILLCode_Sheet1.Cells[0, 0].Text = strCode;
            ssILLCode_Sheet1.Cells[0, 1].Text = strName;
        }

        private void ssAGREE_EditModeOff(object sender, EventArgs e)
        {
        }

        private void btnSaveUpload_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (ssList_Sheet1.RowCount == 0) return;

            string strFileName = sender == btnSaveUpload ? txtOcsFile.Text : txtPatFile.Text;

            if (strFileName.Length == 0) return;

            //파일명 CP코드_OCS or PAT.확장자
            string strServerFileName = ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim() + (sender == btnSaveUpload ? "_OCS" : "_PAT") + strFileName.Substring(strFileName.LastIndexOf(".")); 

            Ftpedt ftp = new Ftpedt();
            if (ftp.FtpUpload("192.168.100.31", "oracle", READ_FTP("192.168.100.31", "oracle"), strFileName , strServerFileName, "/data/EDMS_DATA/QI") == true)
            {
                Save_FileName(strServerFileName, sender == btnSaveUpload ? true : false);
                ComFunc.MsgBox("저장 하였습니다.");
            }

            ftp = null;
        }

        private void btnSearchUpload_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            try
            {
                string strServerFileName = GetDownloadName(ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim() + (sender == btnSearchUpload ? "_OCS" : "_PAT"), sender == btnSearchUpload ? true : false);
                string strFileName = @"C:\PSMHEXE\exenet\" + strServerFileName;

                if (strServerFileName.Length == 0)
                {
                    strServerFileName = null;
                    strFileName = null;
                    return;
                }

                if (File.Exists(strFileName))
                {
                    File.Delete(strFileName);
                }

                Ftpedt ftp = new Ftpedt();
                if (ftp.FtpDownload("192.168.100.31", "oracle", READ_FTP("192.168.100.31", "oracle"), strFileName, strServerFileName, "/data/EDMS_DATA/QI") == false)
                {
                    ComFunc.MsgBox("다운로드 실패");
                }
                else
                {
                    if(picUpload.Image != null)
                    {
                        picUpload.Image.Dispose();
                        picUpload.Image = null;
                    }

                    picUpload.Image = Image.FromFile(strFileName);
                    panImage.Visible = true;
                    panImage.Left = panel3.Width;
                    panImage.Top = 62;
                    panImage.Height = Height - (panTitle.Height + panTitleSub1.Height);
                    panImage.Width = Width - panel2.Width;

                }
                ftp = null;

                return;
            }
            catch
            {
                return;
            }
        }

        bool Save_FileName(string strFileName, bool bOCS)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "UPDATE " + ComNum.DB_MED + "OCS_CP_MAIN";
                SQL += ComNum.VBLF + "SET";
                SQL += ComNum.VBLF + (bOCS ? "OCSEDUFILE" : "PATEDUFILE") + " = '" + strFileName + "'";
                SQL += ComNum.VBLF + "WHERE CPCODE = '" + ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim() + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
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


        bool Save_FileName_2(string strFileName, bool bOCS)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "UPDATE " + ComNum.DB_MED + "OCS_CP_MAIN";
                SQL += ComNum.VBLF + "SET";
                SQL += ComNum.VBLF + (bOCS ? "OCSEDUFILE1" : "PATEDUFILE1") + " = '" + strFileName + "'";
                SQL += ComNum.VBLF + "WHERE CPCODE = '" + ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim() + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
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
        string READ_FTP(string strIP, string strUser)
        {
            string strVal = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT USERPASS FROM ADMIN.BAS_ACCOUNT_SERVER      ";
                SQL = SQL + ComNum.VBLF + "WHERE IP = '" + strIP + "'      ";
                SQL = SQL + ComNum.VBLF + "    AND USERID = '" + strUser + "'        ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SDATE DESC      ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = clsAES.DeAES(dt.Rows[0]["USERPASS"].ToString().Trim());
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return strVal;
        }

        string GetDownloadName(string strServerFileName, bool bOCS)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            string strVal = string.Empty;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT OCSEDUFILE, PATEDUFILE";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_CP_MAIN";
                SQL += ComNum.VBLF + "WHERE CPCODE = '" + ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim() + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return strVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return strVal;
                }

                strVal = dt.Rows[0][bOCS ? "OCSEDUFILE" : "PATEDUFILE"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return strVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strVal;
            }
        }

        string GetDownloadName_2(string strServerFileName, bool bOCS)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            string strVal = string.Empty;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT OCSEDUFILE1, PATEDUFILE1";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_CP_MAIN";
                SQL += ComNum.VBLF + "WHERE CPCODE = '" + ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim() + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return strVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return strVal;
                }

                strVal = dt.Rows[0][bOCS ? "OCSEDUFILE1" : "PATEDUFILE1"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return strVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strVal;
            }
        }
        private void txtUploadFile_DoubleClick(object sender, EventArgs e)
        {

        }

        private void picUpload_Click(object sender, EventArgs e)
        {
            panImage.Visible = false;
            picUpload.Image.Dispose();
            picUpload.Image = null;
        }

        private void lblEdu_DoubleClick(object sender, EventArgs e)
        {
            using (OpenFileDialog OpenFile = new OpenFileDialog())
            {
                OpenFile.Title = "열기";
                OpenFile.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;";
                OpenFile.ReadOnlyChecked = true;
                OpenFile.ShowDialog();

                (sender == lblEduOcs ? txtOcsFile : txtPatFile).Text = OpenFile.FileName;
            }
        }

        private void BtnFileSearch_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OpenFile = new OpenFileDialog())
            {
                OpenFile.Title = "열기";
                OpenFile.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;";
                OpenFile.ReadOnlyChecked = true;
                OpenFile.ShowDialog();

                txtOcsFile.Text = OpenFile.FileName;
            }
        }

        private void BtnFileSearch2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OpenFile = new OpenFileDialog())
            {
                OpenFile.Title = "열기";
                OpenFile.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;";
                OpenFile.ReadOnlyChecked = true;
                OpenFile.ShowDialog();

                txtPatFile.Text = OpenFile.FileName;
            }
        }

        private void btnGetHISTORY_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strCode = "";
            string strROWID = "";

            strCode = txtCPCODE.Text.Trim();

            if (strCode == "")
            {
                ComFunc.MsgBox("선택된 CP종류 없습니다.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID ROWID1 ";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.OCS_CP_MAIN ";
                SQL = SQL + ComNum.VBLF + "  WHERE CPCODE = '" + strCode + "' ";
                SQL = SQL + ComNum.VBLF + "    AND SDATE = ( ";
                SQL = SQL + ComNum.VBLF + "                SELECT MAX(SDATE) SDATE ";
                SQL = SQL + ComNum.VBLF + "                  FROM ADMIN.OCS_CP_MAIN ";
                SQL = SQL + ComNum.VBLF + "                 WHERE CPCODE = '" + strCode + "') ";
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
                    strROWID = dt.Rows[0]["ROWID1"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                if (strROWID == "")
                {
                    ComFunc.MsgBox("해당 CP는 과거 내역이 없습니다. 확인하시기 바랍니다.");
                    return;
                }

                FormClear();

                txtCPCODE.Text = strCode;
                lblRowid.Text = "";
                txtSDATE.Text = clsPublic.GstrSysDate;
                txtEDATE.Text = "9998-12-31";
                dtpSDate.Value = Convert.ToDateTime(txtSDATE.Text);
                dtpStopDate.Value = Convert.ToDateTime(txtEDATE.Text);

                ssHisCellDoubleClick(strROWID);


                ComFunc.MsgBox("이전 CP내용 가져오기를 완료하였습니다. 수정 후 저장을 하셔야 신규 CP가 생성이 됩니다.");

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

        private void BtnFileSearch_1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OpenFile = new OpenFileDialog())
            {
                OpenFile.Title = "열기";
                OpenFile.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;";
                OpenFile.ReadOnlyChecked = true;
                OpenFile.ShowDialog();

                txtOcsFile1.Text = OpenFile.FileName;
            }
        }

        private void BtnFileSearch2_1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OpenFile = new OpenFileDialog())
            {
                OpenFile.Title = "열기";
                OpenFile.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;";
                OpenFile.ReadOnlyChecked = true;
                OpenFile.ShowDialog();

                txtPatFile1.Text = OpenFile.FileName;
            }
        }

        private void btnSaveUpload_1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (ssList_Sheet1.RowCount == 0) return;

            string strFileName = sender == btnSaveUpload_1 ? txtOcsFile1.Text : txtPatFile1.Text;

            if (strFileName.Length == 0) return;

            //파일명 CP코드_OCS or PAT.확장자
            string strServerFileName = ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim() + (sender == btnSaveUpload_1 ? "_OCS1" : "_PAT1") + strFileName.Substring(strFileName.LastIndexOf("."));

            Ftpedt ftp = new Ftpedt();
            if (ftp.FtpUpload("192.168.100.31", "oracle", READ_FTP("192.168.100.31", "oracle"), strFileName, strServerFileName, "/data/EDMS_DATA/QI") == true)
            {
                Save_FileName_2(strServerFileName, sender == btnSaveUpload_1 ? true : false);
                ComFunc.MsgBox("저장 하였습니다.");
            }

            ftp = null;
        }

        private void btnSearchUpload_1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            try
            {
                string strServerFileName = GetDownloadName_2(ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim() + (sender == btnSearchUpload_1 ? "_OCS1" : "_PAT1"), sender == btnSearchUpload_1 ? true : false);
                string strFileName = @"C:\PSMHEXE\exenet\" + strServerFileName;

                if (strServerFileName.Length == 0)
                {
                    strServerFileName = null;
                    strFileName = null;
                    return;
                }

                if (File.Exists(strFileName))
                {
                    File.Delete(strFileName);
                }

                Ftpedt ftp = new Ftpedt();
                if (ftp.FtpDownload("192.168.100.31", "oracle", READ_FTP("192.168.100.31", "oracle"), strFileName, strServerFileName, "/data/EDMS_DATA/QI") == false)
                {
                    ComFunc.MsgBox("다운로드 실패");
                }
                else
                {
                    if (picUpload.Image != null)
                    {
                        picUpload.Image.Dispose();
                        picUpload.Image = null;
                    }

                    picUpload.Image = Image.FromFile(strFileName);
                    panImage.Visible = true;
                    panImage.Left = panel3.Width;
                    panImage.Top = 62;
                    panImage.Height = Height - (panTitle.Height + panTitleSub1.Height);
                    panImage.Width = Width - panel2.Width;

                }
                ftp = null;

                return;
            }
            catch
            {
                return;
            }
        }

        private void ssOpCode_EditModeOff(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strCode = ssOpCode_Sheet1.Cells[ssOpCode_Sheet1.ActiveRowIndex, 0].Text.ToUpper().Trim();

            ssOpCode_Sheet1.Cells[ssOpCode_Sheet1.ActiveRowIndex, 0].Text = strCode;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUNAMEK ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "bas_sun ";
                SQL = SQL + ComNum.VBLF + "     WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "         AND UPPER(SUNEXT) = '" + strCode + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SUNAMEK ";

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
                    ssOpCode_Sheet1.Cells[ssOpCode_Sheet1.ActiveRowIndex, 1].Text = dt.Rows[0]["SUNAMEK"].ToString().Trim();

                    if (ssOpCode_Sheet1.ActiveRowIndex == ssOpCode_Sheet1.RowCount - 1)
                    {
                        ssOpCode_Sheet1.RowCount = ssOpCode_Sheet1.RowCount + 1;
                        ssOpCode_Sheet1.SetRowHeight(ssOpCode_Sheet1.RowCount - 1, ComNum.SPDROWHT);
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

        void Set_Spread()
        {
            int i = 0;
            int k = 0;
            ss10_Sheet1.Cells[2, 1].Text = txtCPNAME.Text;
            ss10_Sheet1.Cells[2, 8].Text = dtpSDate.Text  + " 등록";

            if (rdoIO0.Checked == true)
            {
                ss10_Sheet1.Cells[3, 1].Text = "■ 응급     □ 입원  ";
            }
            else if (rdoIO1.Checked == true)
            {
                ss10_Sheet1.Cells[3, 1].Text = "□ 응급     ■ 입원  ";
            }
            else
            {
                ss10_Sheet1.Cells[3, 1].Text = "□ 응급     □ 입원  ";
            }
            if (rdoGUBUN0.Checked == true)
            {
                ss10_Sheet1.Cells[4, 1].Text = "■ 진단  □ 수술  □ 진단+수술 ";
            }
            else if (rdoGUBUN1.Checked == true)
            {
                ss10_Sheet1.Cells[4, 1].Text = "□ 진단  ■ 수술  □ 진단+수술 ";
            }
            else  if (rdoGUBUN2.Checked == true)
            {
                ss10_Sheet1.Cells[4, 1].Text = "□ 진단  □ 수술  ■ 진단+수술 ";
            }
            else
            {
                ss10_Sheet1.Cells[4, 1].Text = "□ 진단  □ 수술  □ 진단+수술 ";
            }

            if (chkSCALE0.Checked == true)
            {
                ss10_Sheet1.Cells[5, 1].Text = "■ 입퇴원  □  POST-OP 퇴원  □ 기타 ";
            }
            else if (chkSCALE1.Checked == true)
            {
                ss10_Sheet1.Cells[5, 1].Text = "□ 입퇴원  ■  POST-OP 퇴원  □ 기타";
            }
            else if (chkSCALE2.Checked == true)
            {
                ss10_Sheet1.Cells[5, 1].Text = "□ 입퇴원  □  POST-OP 퇴원  ■ 기타";
            }
            else
            {
                ss10_Sheet1.Cells[5, 1].Text = "□ 입퇴원  □  POST-OP 퇴원  □ 기타";
            }


            for (i = 0; i < ssILLCode_Sheet1.RowCount; i++)
            {

                ss10_Sheet1.Cells[i + 7, 2].Text = ssILLCode_Sheet1.Cells[i, 0].Text;
                ss10_Sheet1.Cells[i + 7, 3].Text = ssILLCode_Sheet1.Cells[i, 1].Text;
                
            }
            for (i = 0; i < ssOpCode_Sheet1.RowCount; i++)
            {

                ss10_Sheet1.Cells[i + 7, 5].Text = ssOpCode_Sheet1.Cells[i, 0].Text;
                ss10_Sheet1.Cells[i + 7, 6].Text = ssOpCode_Sheet1.Cells[i, 1].Text;

            }
            ss10_Sheet1.Cells[27, 2].Text = "(  " + txtFRAGE.Text + "   )세이상 " + "(  " + txtTOAGE.Text + "   )세이하 ";
            

            if (chkBI0.Checked == true)
            {
                ss10_Sheet1.Cells[28, 2].Text = "■ 보험     □ 보호  ";
            }
            else
            {
                ss10_Sheet1.Cells[28, 2].Text = "□ 보험     ■ 보호  ";
            }


            for (i = 0; i < ssExcept_Sheet1.RowCount; i++)
            {
                ss10_Sheet1.Cells[i + 29, 1].Text = ssExcept_Sheet1.Cells[i, 1].Text;
            }

            for (i = 0; i < ssStop_Sheet1.RowCount; i++)
            {
                ss10_Sheet1.Cells[i + 29, 5].Text = ssStop_Sheet1.Cells[i, 1].Text ;
            }


            ss10_Sheet1.Cells[44, 1].Text = "(  " + txtDay.Text + "   )일";

            if (rdoINDICATOR0.Checked == true)
            {
                ss10_Sheet1.Cells[45, 1].Text = "■ 유    □ 무 ";
            }
            else
            {
                ss10_Sheet1.Cells[45, 1].Text = "□ 유    ■ 무 ";
            }
            

            for (i = 0; i < ssINDICATOR_Sheet1.RowCount; i++)
            {
                ss10_Sheet1.Cells[i + 47, 1].Text = ssINDICATOR_Sheet1.Cells[i, 1].Text;
                ss10_Sheet1.Cells[i + 47, 3].Text = ssINDICATOR_Sheet1.Cells[i, 5].Text;
                ss10_Sheet1.Cells[i + 47, 7].Text = ssINDICATOR_Sheet1.Cells[i, 7].Text;
                ss10_Sheet1.Cells[i + 47, 9].Text = ssINDICATOR_Sheet1.Cells[i, 8].Text;
      
            }
            if (rdoAGREE0.Checked ==true)
            {
                ss10_Sheet1.Cells[53, 1].Text = "■ 유    □ 무 ";
            }
            else
            {
                ss10_Sheet1.Cells[53, 1].Text = "□ 유    ■ 무 ";
            }
           

            for (i = 0; i < ssAGREE_Sheet1.RowCount; i++)
            {
                ss10_Sheet1.Cells[54, 1].Text += ssAGREE_Sheet1.Cells[i, 1].Text + ComNum.VBLF;
             
            }
           
        }

        void Set_Spread_Clear()
        {
            int i = 0;
            int k = 0;
            ss10_Sheet1.Cells[2, 1].Text = "";
            ss10_Sheet1.Cells[2, 8].Text = "";

            ss10_Sheet1.Cells[3, 1].Text = "";
            ss10_Sheet1.Cells[4, 1].Text = "";

            ss10_Sheet1.Cells[5, 1].Text = "";


            for (i = 0; i < 20; i++)
            {

                ss10_Sheet1.Cells[i + 7, 2].Text = "";
                ss10_Sheet1.Cells[i + 7, 3].Text = "";

            }
            for (i = 0; i < 20; i++)
            {

                ss10_Sheet1.Cells[i + 7, 5].Text = "";
                ss10_Sheet1.Cells[i + 7, 6].Text = "";

            }
            ss10_Sheet1.Cells[27, 2].Text = "";


            ss10_Sheet1.Cells[28, 2].Text = "";


            for (i = 0; i < 15; i++)
            {
                ss10_Sheet1.Cells[i + 29, 1].Text = "";
            }

            for (i = 0; i < 15; i++)
            {
                ss10_Sheet1.Cells[i + 29, 5].Text = "";
            }


            ss10_Sheet1.Cells[44, 1].Text = "";

            ss10_Sheet1.Cells[45, 1].Text = "";


            for (i = 0; i < 6; i++)
            {
                ss10_Sheet1.Cells[i + 47, 1].Text = "";
                ss10_Sheet1.Cells[i + 47, 3].Text = "";
                ss10_Sheet1.Cells[i + 47, 7].Text = "";
                ss10_Sheet1.Cells[i + 47, 9].Text = "";

            }
            ss10_Sheet1.Cells[53, 1].Text = "";

            ss10_Sheet1.Cells[54, 1].Text = "";

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            if (ss10.ActiveSheet.RowCount == 0)
            {
                return;
            }
            Set_Spread_Clear();
            Set_Spread();
            clsSpread SPR = new clsSpread();
            

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            setMargin = new clsSpread.SpdPrint_Margin(0, 0, 0, 0, 0, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, false, false);

            SPR.setSpdPrint(ss10, true, setMargin, setOption, "", "");
        }
    }
}
