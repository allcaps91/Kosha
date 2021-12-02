using ComBase; //기본 클래스
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupPharConsultReturnDetail.cs
    /// Description     : 복약상담 회신서 상세보기
    /// Author          : 이정현
    /// Create Date     : 2017-08-24
    /// <history> 
    /// 복약상담 회신서 상세보기
    /// </history>
    /// <seealso>
    /// PSMH\drug\drinfo\FrmPharConsultReturnDetail.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drinfo\drinfo.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupPharConsultReturnDetail : Form
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

        public frmComSupPharConsultReturnDetail()
        {
            InitializeComponent();
        }

        public frmComSupPharConsultReturnDetail(string strSEQNO, string strIPDNO = "", string strPTNO = "", string strPROGRESS = "", string strDRUGCODE = "")
        {
            InitializeComponent();

            GstrSEQNO = strSEQNO;
            GstrIPDNO = strIPDNO;
            GstrPROGRESS = strPROGRESS;
            GstrPTNO = strPTNO;
            GstrDRUGCODE = strDRUGCODE;
        }

        private void frmComSupPharConsultReturnDetail_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ChkNurse();

            txtMemo.Text = "";

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
                ssConsult1_Sheet1.Cells[13, 12].Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            }

            if (GstrIPDNO != "")
            {
                setPatientInfo(ssConsult0_Sheet1, GstrIPDNO);
                setPatientInfo(ssConsult1_Sheet1, GstrIPDNO);
            }

            btnSearch.Click += new EventHandler(btnSearch_Click);

            if (GbolNurse == true)
            {
                NurseDisplay();
            }
            else
            {
                DefaultDisplay();
            }

            if (GstrSEQNO == "DRUG")
            {
                ssConsult0_Sheet1.Cells[8, 3].Text = GstrDRUGCODE;
                ssConsult0_Sheet1.Cells[8, 4].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, GstrDRUGCODE);
                ssConsult1_Sheet1.Cells[8, 3].Text = GstrDRUGCODE;
                ssConsult1_Sheet1.Cells[8, 4].Text = clsVbfunc.READ_SugaName(clsDB.DbCon,GstrDRUGCODE);
            }
        }

        private void ChkNurse()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = "SELECT * FROM " + ComNum.DB_ERP + "INSA_MST";
                SQL = SQL + ComNum.VBLF + "     WHERE JIK IN ";
                SQL = SQL + ComNum.VBLF + "             (SELECT CODE FROM " + ComNum.DB_ERP + "INSA_CODE";
                SQL = SQL + ComNum.VBLF + "                 WHERE (NAME LIKE '%간호사%' OR NAME LIKE '%응급구조사%') AND GUBUN = '2')";
                SQL = SQL + ComNum.VBLF + "         AND SABUN = '" + clsType.User.Sabun + "'";

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

                GbolNurse = clsVbfunc.NurseSystemManagerChk(clsDB.DbCon, clsType.User.Sabun);
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

        private void readPConsult(string strSEQNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     IPDNO, PANO, ORDERCODE, USED, BIGO, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(WRITEDATE,'YYYY-MM-DD HH24:MI') AS WRITEDATE, ";
                SQL = SQL + ComNum.VBLF + "     WRITESABUN, ORDERCODE2, ORDERCODE3";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_PCONSULT ";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;

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
                    GstrWRITESABUN = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["WRITESABUN"].ToString().Trim());

                    ssConsult0_Sheet1.Cells[8, 3].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();
                    ssConsult0_Sheet1.Cells[8, 4].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[0]["ORDERCODE"].ToString().Trim());
                    ssConsult0_Sheet1.Cells[9, 3].Text = dt.Rows[0]["ORDERCODE2"].ToString().Trim();
                    ssConsult0_Sheet1.Cells[9, 4].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[0]["ORDERCODE2"].ToString().Trim());
                    ssConsult0_Sheet1.Cells[10, 3].Text = dt.Rows[0]["ORDERCODE3"].ToString().Trim();
                    ssConsult0_Sheet1.Cells[10, 4].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[0]["ORDERCODE3"].ToString().Trim());

                    ssConsult1_Sheet1.Cells[8, 3].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();
                    ssConsult1_Sheet1.Cells[8, 4].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[0]["ORDERCODE"].ToString().Trim());
                    ssConsult1_Sheet1.Cells[9, 3].Text = dt.Rows[0]["ORDERCODE2"].ToString().Trim();
                    ssConsult1_Sheet1.Cells[9, 4].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[0]["ORDERCODE2"].ToString().Trim());
                    ssConsult1_Sheet1.Cells[10, 3].Text = dt.Rows[0]["ORDERCODE3"].ToString().Trim();
                    ssConsult1_Sheet1.Cells[10, 4].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[0]["ORDERCODE3"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;
                
                SQL = "";
                SQL = "SELECT * FROM " + ComNum.DB_MED + "OCS_DOCTOR";
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

        private void setPatientInfo(FarPoint.Win.Spread.SheetView ssSpread_Sheet, string strIPDNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            
            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, SNAME, SEX, AGE, ROOMCODE, DEPTCODE, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + "     WHERE IPDNO = " + strIPDNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssSpread_Sheet.Cells[4, 3].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ssSpread_Sheet.Cells[4, 5].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssSpread_Sheet.Cells[4, 8].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + dt.Rows[0]["AGE"].ToString().Trim();
                    ssSpread_Sheet.Cells[4, 10].Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    ssSpread_Sheet.Cells[4, 12].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    ssSpread_Sheet.Cells[4, 14].Text = dt.Rows[0]["INDATE"].ToString().Trim();
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
                    ssSpread_Sheet.Cells[5, 3].Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                    ssSpread_Sheet.Cells[5, 9].Text = dt.Rows[0]["ILLNAMEK"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                ssSpread_Sheet.Cells[12, 8].Text = GstrBIGO;

                ssSpread_Sheet.Cells[13, 3].Text = GstrWRITEDATE;
                ssSpread_Sheet.Cells[13, 6].Text = GstrWRITESABUN;
                ssSpread_Sheet.Cells[13, 9].Text = GstrDeptCode;
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

        private void NurseDisplay()
        {
            panReConsult.Visible = true;

            lblReturnDate.Text = "";
            lblReturnDate.Text = "★ 회신일 : " + ssConsult0_Sheet1.Cells[13, 12].Text.Trim().Trim();
        }

        private void DefaultDisplay()
        {
            panReConsult.Visible = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (GstrSEQNO == "DRUG")
            {
                return;
            }

            if (newDetailGbn(GstrSEQNO, "1") == false)
            {
                viewDetail("1");
            }
            
            if (newDetailGbn(GstrSEQNO, "2") == false)
            {
                viewDetail("2");
            }
        }

        private bool newDetailGbn(string strSEQNO, string strGubun)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "SELECT SEQNO FROM " + ComNum.DB_ERP + "DRUG_PCONSULT_DETAIL" + strGubun;
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
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
                return rtnVal;
            }
        }

        private void viewDetail(string strGubun)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int k = 0;

            string strColName = "";

            FarPoint.Win.Spread.SheetView ssSpread_Sheet = null;
            FarPoint.Win.Spread.SheetView ssSpreadBack_Sheet = null;

            if (strGubun == "1")
            {
                ssSpread_Sheet = ssConsult0_Sheet1;
                ssSpreadBack_Sheet = ssConsultBack0_Sheet1;
            }
            else if (strGubun == "2")
            {
                ssSpread_Sheet = ssConsult1_Sheet1;
                ssSpreadBack_Sheet = ssConsultBack1_Sheet1;
            }

            clearDetail(strGubun, ssSpread_Sheet, ssSpreadBack_Sheet);

            try
            {
                SQL = "";
                SQL = "SELECT * FROM " + ComNum.DB_ERP + "DRUG_PCONSULT_DETAIL" + strGubun;
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
                    for(i = 0; i < ssSpreadBack_Sheet.RowCount; i++)
                    {
                        for (k = 0; k < ssSpreadBack_Sheet.ColumnCount; k++)
                        {
                            if (verifyColumn(ssSpreadBack_Sheet.Cells[i, k].Text.Trim()))
                            {
                                strColName = ssSpreadBack_Sheet.Cells[i, k].Text.Trim();

                                ssSpread_Sheet.Cells[i, k].Text = setData(dt, strColName);
                            }
                        }
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

        private void clearDetail(string strGubun, FarPoint.Win.Spread.SheetView ssSpread_Sheet, FarPoint.Win.Spread.SheetView ssSpreadBack_Sheet)
        {
            int i = 0;
            int k = 0;

            for (i = 0; i < ssSpreadBack_Sheet.RowCount; i++)
            {
                for(k = 0; k < ssSpreadBack_Sheet.ColumnCount; k++)
                {
                    if (verifyColumn(ssSpreadBack_Sheet.Cells[i, k].Text.Trim()))
                    {
                        ssSpread_Sheet.Cells[i, k].Text = "";
                    }
                }
            }
        }

        private bool verifyColumn(string strData)
        {
            bool rtnVal = false;

            if (VB.Left(strData.Trim(), 1) == "㉿" && VB.Len(strData.Trim()) > 1)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        private string setData(DataTable dt, string strColName)
        {
            int i = 0;
            string rtnVal = "";

            strColName = strColName.Replace("㉿", "");

            for(i = 0; i < dt.Rows.Count; i++)
            {
                foreach(DataColumn column in dt.Columns)
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
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            string strComment = "";
            string strGubun = "";

            switch (tabConsult.SelectedIndex)
            {
                case 0:
                    strGubun = "1";
                    break;
                case 1:
                    strGubun = "2";
                    break;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (strGubun == "1")
                {
                    strComment = ssConsult0_Sheet1.Cells[30, 5].Text.Trim();

                    if (GstrSEQNO == "DRUG")
                    {
                        if (saveDrugNew() == false)
                        {
                            return;
                        }
                    }
                }
                else if (strGubun == "2")
                {
                    strComment = ssConsult1_Sheet1.Cells[36, 5].Text.Trim();
                }

                if (saveDetail(strGubun) == true)
                {
                    if (progressUpdate("C", strComment, strGubun) == false)
                    {
                        return;
                    }
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private bool saveDrugNew()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            
            try
            {
                GstrSEQNO = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_ERP + "SEQ_PCONSULT.NEXTVAL").ToString();

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_PCONSULT";
                SQL = SQL + ComNum.VBLF + "     (SEQNO, IPDNO, PANO, ORDERCODE, USED, BIGO, PROGRESS, WRITEDATE, WRITESABUN, ORDERCODE2, ORDERCODE3)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         " + GstrSEQNO + ", ";           //SEQNO
                SQL = SQL + ComNum.VBLF + "         " + GstrIPDNO + ", ";           //IPDNO
                SQL = SQL + ComNum.VBLF + "         '" + GstrPTNO + "', ";          //PANO
                SQL = SQL + ComNum.VBLF + "         '" + GstrDRUGCODE + "', ";     //ORDERCODE
                SQL = SQL + ComNum.VBLF + "         '0',";                          //USED
                SQL = SQL + ComNum.VBLF + "         '약제과 자체', ";               //BIGO
                SQL = SQL + ComNum.VBLF + "         '1', ";                         //PROGRESS
                SQL = SQL + ComNum.VBLF + "         SYSDATE, ";                     //WRITEDATE
                SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun + ", ";  //WRITESABUN
                SQL = SQL + ComNum.VBLF + "         '', ";                          //ORDERCODE2
                SQL = SQL + ComNum.VBLF + "         '' ";                           //ORDERCODE3
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

        private bool saveDetail(string strGubun)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            int k = 0;

            string[] strCol = new string[0];
            string[] strValue = new string[0];

            FarPoint.Win.Spread.SheetView ssSpread_Sheet = null;
            FarPoint.Win.Spread.SheetView ssSpreadBack_Sheet = null;

            if (strGubun == "1")
            {
                ssSpread_Sheet = ssConsult0_Sheet1;
                ssSpreadBack_Sheet = ssConsultBack0_Sheet1;
            }
            else if (strGubun == "2")
            {
                ssSpread_Sheet = ssConsult1_Sheet1;
                ssSpreadBack_Sheet = ssConsultBack1_Sheet1;
            }

            try
            {
                for(i = 0; i < ssSpreadBack_Sheet.RowCount; i++)
                {
                    for(k = 0; k < ssSpreadBack_Sheet.ColumnCount; k++)
                    {
                        if (verifyColumn(ssSpreadBack_Sheet.Cells[i, k].Text.Trim()))
                        {
                            Array.Resize<string>(ref strCol, strCol.Length + 1);
                            Array.Resize<string>(ref strValue, strValue.Length + 1);

                            strCol[strCol.Length - 1] = ssSpreadBack_Sheet.Cells[i, k].Text.Replace("㉿", "");
                            strValue[strValue.Length - 1] = "'" + ssSpread_Sheet.Cells[i, k].Text.Trim() + "'";
                        }
                    }
                }

                SQL = "";
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_PCONSULT_DETAIL" + strGubun;
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
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_PCONSULT_DETAIL" + strGubun;
                SQL = SQL + ComNum.VBLF + "     (SEQNO, WRITEDATE, WRITESABUN,";

                for (i = 0; i < VB.UBound(strCol); i++)
                {
                    if (i == VB.UBound(strValue) - 1)
                    {
                        SQL = SQL + strCol[i];
                    }
                    else
                    {
                        SQL = SQL + strCol[i] + ",";
                    }
                }

                SQL = SQL + ")";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         " + GstrSEQNO + ", ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun + ",";

                for (i = 0; i < VB.UBound(strCol); i++)
                {
                    if (i == VB.UBound(strCol) - 1)
                    {
                        SQL = SQL + ComNum.VBLF + "         " + strValue[i];
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         " + strValue[i] + ",";
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

        private bool progressUpdate(string strBun, string strComment, string strGubun)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_ERP + "DRUG_PCONSULT";
                SQL = SQL + ComNum.VBLF + "     SET ";

                if (strGubun == "1")
                {
                    SQL = SQL + ComNum.VBLF + "         RETURN_TEXT = '" + strComment + "', ";
                }
                else if (strGubun == "2")
                {
                    SQL = SQL + ComNum.VBLF + "         RETURN_TEXT2 = '" + strComment + "', ";
                }

                SQL = SQL + ComNum.VBLF + "         PROGRESS = '" + strBun + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE SEQNO = " + GstrSEQNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

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
            
            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (deleteDetail("1") == false)
                {
                    return;
                }

                if (deleteDetail("2") == false)
                {
                    return;
                }
                
                if (progressUpdate("1", "", "1") == false)
                {
                    return;
                }

                if (progressUpdate("1", "", "2") == false)
                {
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private bool deleteDetail(string strGubun)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_PCONSULT_DETAIL" + strGubun;
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            FarPoint.Win.Spread.FpSpread ssSpread = null;

            if (tabConsult.SelectedIndex == 0)
            {
                ssSpread = ssConsult0;
            }
            else if (tabConsult.SelectedIndex == 1)
            {
                ssSpread = ssConsult1;
            }

            ssSpread.ActiveSheet.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssSpread.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssSpread.ActiveSheet.PrintInfo.Margin.Top = 20;
            ssSpread.ActiveSheet.PrintInfo.Margin.Bottom = 20;
            ssSpread.ActiveSheet.PrintInfo.ShowColor = false;
            ssSpread.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssSpread.ActiveSheet.PrintInfo.ShowBorder = true;
            ssSpread.ActiveSheet.PrintInfo.ShowGrid = true;
            ssSpread.ActiveSheet.PrintInfo.ShowShadows = false;
            ssSpread.ActiveSheet.PrintInfo.UseMax = true;
            ssSpread.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssSpread.ActiveSheet.PrintInfo.UseSmartPrint = false;
            ssSpread.ActiveSheet.PrintInfo.ShowPrintDialog = false;
            ssSpread.ActiveSheet.PrintInfo.Preview = false;
            ssSpread.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReTry_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (txtMemo.Text.Trim() == "")
                {
                    if (ComFunc.MsgBoxQ("전달할 내용이 없습니다. 재의뢰 내용을 입력하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        txtMemo.Focus();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }

                if (ComFunc.MsgBoxQ("해당 회신서와 동일한 내용으로 재의뢰를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                double dblSeqNo = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_ERP + "SEQ_PCONSULT.NEXTVAL");

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_PCONSULT";
                SQL = SQL + ComNum.VBLF + "     (SEQNO, IPDNO, PANO, ORDERCODE, USED, BIGO, PROGRESS, WRITEDATE, WRITESABUN, ORDERCODE2, ORDERCODE3, RETRY, RE_SEQNO) ";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "     " + dblSeqNo + ", IPDNO, PANO, ORDERCODE, '1', '" + txtMemo.Text.Trim() + "', '1', SYSDATE, " + clsType.User.Sabun + ", ORDERCODE2, ORDERCODE3, '1', SEQNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_PCONSULT";
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
                ComFunc.MsgBox("재의뢰하였습니다.");
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

        private void ssConsult_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row == 13 && e.Column == 12)
            {
                using (frmCalendar frm = new frmCalendar())
                {
                    frm.ShowDialog();
                }

                ((FarPoint.Win.Spread.FpSpread)sender).ActiveSheet.Cells[13, 12].Text = clsPublic.GstrCalDate;
                clsPublic.GstrCalDate = "";
            }
        }

        private void ssConsult_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            if (e.Column == 3 && (e.Row >= 8 && e.Row <= 10))
            {
                ((FarPoint.Win.Spread.FpSpread)sender).ActiveSheet.Cells[e.Row, e.Column].Text = ((FarPoint.Win.Spread.FpSpread)sender).ActiveSheet.Cells[e.Row, e.Column].Text.ToUpper();
                ((FarPoint.Win.Spread.FpSpread)sender).ActiveSheet.Cells[e.Row, e.Column + 1].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, ((FarPoint.Win.Spread.FpSpread)sender).ActiveSheet.Cells[e.Row, e.Column].Text);
            }

            if (e.Column == 15 && e.Row == 13)
            {
                ((FarPoint.Win.Spread.FpSpread)sender).ActiveSheet.Cells[e.Row, e.Column].Text = clsVbfunc.GetInSaName(clsDB.DbCon, ((FarPoint.Win.Spread.FpSpread)sender).ActiveSheet.Cells[13, 15].Text);
            }
        }
    }
}
