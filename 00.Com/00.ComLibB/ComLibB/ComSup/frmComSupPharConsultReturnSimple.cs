using ComBase; //기본 클래스
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupPharConsultReturnSimple.cs
    /// Description     : 복약상담 회신서 상세보기
    /// Author          : 이정현
    /// Create Date     : 2017-09-12
    /// <history> 
    /// 복약상담 회신서 상세보기
    /// </history>
    /// <seealso>
    /// PSMH\drug\drinfo\FrmPharConsultReturnSimple.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drinfo\drinfo.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupPharConsultReturnSimple : Form
    {
        private string GstrSEQNO = "";
        private string GstrIPDNO = "";
        private string GstrORDERCODE = "";
        private string GstrBIGO = "";
        private string GstrWRITEDATE = "";
        private string GstrWRITESABUN = "";
        private string GstrDeptCode = "";
        private string GstrPROGRESS = "";
        private string GstrPTNO = "";
        private string GstrDRUGCODE = "";

        private bool GbolNurse = false;

        public frmComSupPharConsultReturnSimple()
        {
            InitializeComponent();
        }

        public frmComSupPharConsultReturnSimple(string strSEQNO, string strIPDNO = "", string strPTNO = "", string strPROGRESS = "", string strDRUGCODE = "")
        {
            InitializeComponent();

            GstrSEQNO = strSEQNO;
            GstrIPDNO = strIPDNO;
            GstrPROGRESS = strPROGRESS;
            GstrPTNO = strPTNO;
            GstrDRUGCODE = strDRUGCODE;
        }

        private void frmComSupPharConsultReturnSimple_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            SetNurse();

            if (ChkYakuk(clsType.User.Sabun) == false)
            {
                panSave.Visible = false;
                panDelete.Visible = false;
                panPrint.Visible = false;
            }

            if (GstrSEQNO != "" && GstrSEQNO != "DRUG")
            {
                readPConsult(GstrSEQNO);
            }
            else if (GstrSEQNO == "DRUG")
            {
                GstrIPDNO = clsVbfunc.readIPDNO(clsDB.DbCon, GstrPROGRESS, GstrPTNO);

                ssConsult0_Sheet1.Cells[13, 12].Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            }

            if (GstrIPDNO != "")
            {
                setPatientInfo1(GstrIPDNO);
            }

            if (GstrSEQNO != "" && GstrSEQNO != "DRUG")
            {
                GetData();
            }

            if (GbolNurse == true)
            {
                lblReturnDate.Text = "";
                lblReturnDate.Text = "★회신일 : " + ssConsult0_Sheet1.Cells[13, 12].Text.Trim();

                lblReturnDate.Visible = true;
            }
            else
            {
                lblReturnDate.Visible = false;
            }

            if (GstrSEQNO == "DRUG")
            {
                ssConsult0_Sheet1.Cells[8, 3].Text = GstrDRUGCODE;
                ssConsult0_Sheet1.Cells[8, 4].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, GstrDRUGCODE);
            }
        }

        private void SetNurse()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = "SELECT * FROM " + ComNum.DB_ERP + "INSA_MST";
                SQL = SQL + ComNum.VBLF + "     WHERE JIK IN (SELECT CODE FROM " + ComNum.DB_ERP + "INSA_CODE";
                SQL = SQL + ComNum.VBLF + "                         WHERE (NAME LIKE '%간호사%' OR NAME LIKE '%응급구조사%')";
                SQL = SQL + ComNum.VBLF + "                             AND GUBUN = '2')";
                SQL = SQL + ComNum.VBLF + "         AND SABUN = '" + ComFunc.LPAD(clsType.User.Sabun, 5, "0") + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    GbolNurse = true;
                }

                dt.Dispose();
                dt = null;

                if (clsVbfunc.NurseSystemManagerChk(clsDB.DbCon, clsType.User.Sabun) == true)
                {
                    GbolNurse = true;
                }
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

        private bool ChkYakuk(string strSabun)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "SELECT* FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_ERP + "INSA_CODE B";
                SQL = SQL + ComNum.VBLF + "    WHERE A.SABUN = '" + strSabun + "'";
                SQL = SQL + ComNum.VBLF + "        AND B.GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "        AND B.CODE IN ('40', '41', '42', '43')";
                SQL = SQL + ComNum.VBLF + "        AND A.JIK = TRIM(B.CODE)";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }

                dt.Dispose();
                dt = null;

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

        private void readPConsult(string strSeqNo)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     IPDNO, PANO, ORDERCODE, USED, BIGO, TO_CHAR(WRITEDATE,'YYYY-MM-DD HH24:MI') AS WRITEDATE, ";
                SQL = SQL + ComNum.VBLF + "     WRITESABUN, ORDERCODE2, ORDERCODE3";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_PCONSULT ";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSeqNo;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    GstrIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                    GstrORDERCODE = dt.Rows[0]["ORDERCODE"].ToString().Trim();
                    GstrBIGO = dt.Rows[0]["BIGO"].ToString().Trim();
                    GstrWRITEDATE = dt.Rows[0]["WRITEDATE"].ToString().Trim();
                    GstrWRITESABUN = clsVbfunc.GetInSaName(clsDB.DbCon, ComFunc.LPAD(dt.Rows[0]["WRITESABUN"].ToString().Trim(), 5, "0"));

                    ssConsult0_Sheet1.Cells[8, 3].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();
                    ssConsult0_Sheet1.Cells[8, 4].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[0]["ORDERCODE"].ToString().Trim());

                    ssConsult0_Sheet1.Cells[9, 3].Text = dt.Rows[0]["ORDERCODE2"].ToString().Trim();
                    ssConsult0_Sheet1.Cells[9, 4].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[0]["ORDERCODE2"].ToString().Trim());

                    ssConsult0_Sheet1.Cells[10, 3].Text = dt.Rows[0]["ORDERCODE3"].ToString().Trim();
                    ssConsult0_Sheet1.Cells[10, 4].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[0]["ORDERCODE3"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT * FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "     WHERE SABUN = '" + GstrWRITESABUN + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    GstrDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
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

        private void setPatientInfo1(string strIPDNO)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = "SELECT PANO, SNAME, SEX, AGE, ROOMCODE, DEPTCODE, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + "WHERE IPDNO = " + strIPDNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssConsult0_Sheet1.Cells[4, 3].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ssConsult0_Sheet1.Cells[4, 5].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssConsult0_Sheet1.Cells[4, 8].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + dt.Rows[0]["AGE"].ToString().Trim();
                    ssConsult0_Sheet1.Cells[4, 10].Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    ssConsult0_Sheet1.Cells[4, 12].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    ssConsult0_Sheet1.Cells[4, 14].Text = Convert.ToDateTime(dt.Rows[0]["INDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.ILLCODE, B.ILLNAMEK";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IILLS A, " + ComNum.DB_PMPA + "BAS_ILLS B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.ILLCODE = B.ILLCODE";
                SQL = SQL + ComNum.VBLF + "         AND A.IPDNO = " + strIPDNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssConsult0_Sheet1.Cells[5, 3].Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                    ssConsult0_Sheet1.Cells[5, 9].Text = dt.Rows[0]["ILLNAMEK"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                ssConsult0_Sheet1.Cells[12, 8].Text = GstrBIGO;

                ssConsult0_Sheet1.Cells[13, 3].Text = GstrWRITEDATE;
                ssConsult0_Sheet1.Cells[13, 6].Text = GstrWRITESABUN;
                ssConsult0_Sheet1.Cells[13, 9].Text = GstrDeptCode;
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (GstrSEQNO == "DRUG")
            {
                return;
            }

            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int k = 0;

            bool bolnewDetailGbn1 = true;

            try
            {
                #region newDetailGbn1

                SQL = "";
                SQL = "SELECT SEQNO FROM " + ComNum.DB_ERP + "DRUG_PCONSULT_DETAIL3 ";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + GstrSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    bolnewDetailGbn1 = false;
                }

                dt.Dispose();
                dt = null;

                #endregion

                if (bolnewDetailGbn1 == true)
                {
                    return;
                }

                #region clearDetail1

                for (i = 0; i < ssConsult0_Sheet1.RowCount; i++)
                {
                    for (k = 0; k < ssConsult0_Sheet1.ColumnCount; k++)
                    {
                        if (VB.Left(ssConsult0_Sheet1.Cells[i, k].Text.Trim(), 1) == "㉿" && ssConsult0_Sheet1.Cells[i, k].Text.Trim().Length > 1)
                        {
                            ssConsult0_Sheet1.Cells[i, k].Text = "";
                        }
                    }
                }

                #endregion

                #region viewDetail1

                SQL = "";
                SQL = "SELECT * FROM " + ComNum.DB_ERP + "DRUG_PCONSULT_DETAIL3 ";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + GstrSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < ssConsult0_Sheet1.RowCount; i++)
                    {
                        for (k = 0; k < ssConsult0_Sheet1.ColumnCount; k++)
                        {
                            if (VB.Left(ssConsult0_Sheet1.Cells[i, k].Text.Trim(), 1) == "㉿" && ssConsult0_Sheet1.Cells[i, k].Text.Trim().Length > 1)
                            {
                                ssConsult0_Sheet1.Cells[i, k].Text = setData(dt, ssConsult0_Sheet1.Cells[i, k].Text.Trim());
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

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

        private string setData(DataTable dt, string strColName)
        {
            int i = 0;
            string rtnVal = "";

            strColName = strColName.Replace("㉿", "");

            for (i = 0; i < dt.Rows.Count; i++)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ToString().ToUpper() == strColName.ToUpper())
                    {
                        rtnVal = dt.Rows[i][column].ToString().Trim();
                        return rtnVal;
                    }
                }
            }

            return rtnVal;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if (GstrSEQNO == "DRUG") { saveDrugNew(); }

            if (SaveData() == true)
            {
                progressUpdate("C", ssConsult0_Sheet1.Cells[16, 5].Text.Trim(), "1");

                ComFunc.MsgBox("저장되었습니다.");
            }
        }

        private void saveDrugNew()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            double dblPCONSULT = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                dblPCONSULT = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_ERP + "SEQ_PCONSULT.NEXTVAL");

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_PCONSULT";
                SQL = SQL + ComNum.VBLF + "     (SEQNO, IPDNO, PANO, ORDERCODE, USED, BIGO, PROGRESS, WRITEDATE, WRITESABUN, ORDERCODE2, ORDERCODE3)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         " + dblPCONSULT + ", ";
                SQL = SQL + ComNum.VBLF + "         " + GstrIPDNO + ", ";
                SQL = SQL + ComNum.VBLF + "         '" + GstrPTNO + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + GstrORDERCODE + "',";
                SQL = SQL + ComNum.VBLF + "         '0', ";
                SQL = SQL + ComNum.VBLF + "         '약제과 자체', ";
                SQL = SQL + ComNum.VBLF + "         '1', ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE,";
                SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun + ", ";
                SQL = SQL + ComNum.VBLF + "         '', ";
                SQL = SQL + ComNum.VBLF + "         '' ";
                SQL = SQL + ComNum.VBLF + "     )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private bool SaveData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string[] strCol = new string[0];
            string[] strValue = new string[0];

            string strColName = "";

            int i = 0;
            int k = 0;

            Cursor.Current = Cursors.WaitCursor;

            for (i = 0; i < ssConsult0_Sheet1.RowCount; i++)
            {
                for (k = 0; k < ssConsult0_Sheet1.ColumnCount; k++)
                {
                    if (VB.Left(ssConsult0_Sheet1.Cells[i, k].Text.Trim(), 1) == "㉿" && ssConsult0_Sheet1.Cells[i, k].Text.Trim().Length > 1)
                    {
                        Array.Resize<string>(ref strCol, strCol.Length + 1);
                        Array.Resize<string>(ref strValue, strValue.Length + 1);

                        strCol[strCol.Length - 1] = ssConsult0_Sheet1.Cells[i, k].Text.Replace("㉿", "");
                        strValue[strValue.Length - 1] = "'" + ssConsult0_Sheet1.Cells[i, k].Text.Trim() + "'";
                    }
                }
            }

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_PCONSULT_DETAIL3";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + GstrSEQNO;

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
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_PCONSULT_DETAIL3";
                SQL = SQL + ComNum.VBLF + "     (SEQNO, WRITEDATE, WRITESABUN, ";

                for (i = 0; i < VB.UBound(strCol); i++)
                {
                    if (i == VB.UBound(strValue) - 1)
                    {
                        SQL = SQL + strCol[i];
                    }
                    else
                    {
                        SQL = SQL + strCol[i] + ", ";
                    }
                }

                SQL = SQL + ")";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         " + GstrSEQNO + ", ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun + ", ";

                for (i = 0; i < VB.UBound(strCol); i++)
                {
                    if (i == VB.UBound(strCol) - 1)
                    {
                        SQL = SQL + ComNum.VBLF + "         " + strValue[i];
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         " + strValue[i] + ", ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "     )";

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("삭제하시겠습니까?", "삭제", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_PCONSULT_DETAIL3 ";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + GstrSEQNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;

                progressUpdate("1", "", "1");
                progressUpdate("1", "", "2");

                this.Close();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void progressUpdate(string strBun, string strComment, string strGbn)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_ERP + "DRUG_PCONSULT";
                SQL = SQL + ComNum.VBLF + "     SET ";
                SQL = SQL + ComNum.VBLF + "         RETURN_TEXT = '" + strComment + "', ";
                SQL = SQL + ComNum.VBLF + "         PROGRESS = '" + strBun + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE SEQNO = " + GstrSEQNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인
            
            ssConsult0_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssConsult0_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssConsult0_Sheet1.PrintInfo.Margin.Top = 20;
            ssConsult0_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssConsult0_Sheet1.PrintInfo.ShowColor = false;
            ssConsult0_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssConsult0_Sheet1.PrintInfo.ShowBorder = true;
            ssConsult0_Sheet1.PrintInfo.ShowGrid = true;
            ssConsult0_Sheet1.PrintInfo.ShowShadows = false;
            ssConsult0_Sheet1.PrintInfo.UseMax = true;
            ssConsult0_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssConsult0_Sheet1.PrintInfo.UseSmartPrint = false;
            ssConsult0_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssConsult0_Sheet1.PrintInfo.Preview = false;
            ssConsult0.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
