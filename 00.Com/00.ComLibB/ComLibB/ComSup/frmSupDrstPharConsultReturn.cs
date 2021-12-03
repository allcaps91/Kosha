using ComBase;
using ComBase.Controls;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstPharConsultReturn.cs
    /// Description     : 복약상담 결과 등록
    /// Author          : 이정현
    /// Create Date     : 2017-12-19
    /// <history> 
    /// 복약상담 결과 등록
    /// </history>
    /// <seealso>
    /// PSMH\drug\drinfo\FrmPharConsultReturn.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drinfo\drinfo.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstPharConsultReturn : Form
    {
        private string GstrGubun = "NEW";
        private string GstrIPDNO = "";
        private string GstrSEQNO = "";
        private string GstrROWID = "";
        private string GstrPANO = "";
        private string GstrPROGRESS = "";

        public frmSupDrstPharConsultReturn()
        {
            InitializeComponent();
        }

        private void frmSupDrstPharConsultReturn_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            cboWard_SET();

            if (ChkYakuk(clsType.User.Sabun) == false)
            {
                rdoGbn3.Checked = true;
            }

            dtpSDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-7);
            dtpEDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            txtPANO.Text = "";

            readBun();

            GetData();
        }

        public static bool ChkYakuk(string strSabun)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "SELECT* FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_ERP + "INSA_CODE B";
                SQL = SQL + ComNum.VBLF + "WHERE A.SABUN = '" + strSabun + "'";
                SQL = SQL + ComNum.VBLF + "  AND B.GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "  AND B.CODE IN ('40', '41', '42', '43')";
                SQL = SQL + ComNum.VBLF + "  AND A.JIK = TRIM(B.CODE)";

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
                return rtnVal;
            }
        }

        private void cboWard_SET()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboWard.Text = "";
            cboWard.Items.Clear();
            cboWard.Items.Add("전체");

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     WardCode, WardName";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + "WHERE WARDCODE NOT IN ('IU','NP','2W','IQ','ER') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY WardCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboWard.Items.Add("SICU");
                cboWard.Items.Add("MICU");
                cboWard.Items.Add("HD");
                cboWard.Items.Add("ER");
                cboWard.Items.Add("RA");
                cboWard.Items.Add("TTE"); //심장초음파

                cboWard.SelectedIndex = 0;
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

        private void readBun()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboCode.Text = "";
            cboCode.Items.Clear();

            cboBun.Text = "";
            cboBun.Items.Clear();
            cboBun.Items.Add("**.전체");

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PART, COMMENTS";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "DRUG_SETCODE ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = '11'";
                SQL = SQL + ComNum.VBLF + "  AND JEPCODE = '분류명칭' ";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "ORDER BY PART ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboBun.Items.Add(dt.Rows[i]["PART"].ToString().Trim() + "." + dt.Rows[i]["COMMENTS"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboBun.SelectedIndex = 0;
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

        private void txtPANO_Leave(object sender, EventArgs e)
        {
            if (txtPANO.Text.Trim() == "") { return; }

            txtPANO.Text = ComFunc.LPAD(txtPANO.Text.Trim(), 8, "0");
        }

        private void rdoGbn_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                GetData();
            }
        }

        private void cboBun_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strBun = "";

            strBun = VB.Left(cboBun.Text, 2);

            cboCode.Text = "";
            cboCode.Items.Clear();
            
            if (strBun == "**") { return; }

            readCode(strBun);
        }

        private void readCode(string strBun)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboCode.Text = "";
            cboCode.Items.Clear();
            cboCode.Items.Add("전체");

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     JEPCODE, COMMENTS ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_SETCODE ";
                SQL = SQL + ComNum.VBLF + "     WHERE DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '11' ";
                SQL = SQL + ComNum.VBLF + "         AND PART = '" + strBun + "' ";
                SQL = SQL + ComNum.VBLF + "         AND JEPCODE <> '분류명칭' ";

                if (chkDel.Checked == true)
                {

                }
                else
                {
                    SQL += ComNum.VBLF + "          AND JEPCODE NOT IN ( SELECT SUCODE                      ";
                    SQL += ComNum.VBLF + "                               FROM KOSMOS_PMPA.BAS_SUT           ";
                    SQL += ComNum.VBLF + "                               WHERE 1=1                          ";
                    SQL += ComNum.VBLF + "                                 AND DELDATE IS NOT NULL          ";
                    SQL += ComNum.VBLF + "                                 AND BUN IN ('11','12','20')      ";
                    SQL += ComNum.VBLF + "                             )                                    ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY JEPCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboCode.Items.Add(dt.Rows[i]["JEPCODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboCode.SelectedIndex = 0;
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
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;

            string strSDate = dtpSDATE.Value.ToString("yyyy-MM-dd 00:00");
            string strEDate = dtpEDATE.Value.ToString("yyyy-MM-dd 23:59");

            ssView_Sheet1.RowCount = 0;

            clearPCONSULT();

            if (txtPANO.Text.Trim() != "")
            {
                rdoGbn4.Checked = true;
                cboWard.SelectedIndex = 0;
            }

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PROGRESS, TO_CHAR(A.WRITEDATE,'YYYY-MM-DD') AS WDATE, A.WRITESABUN, ORDERCODE, A.SEQNO, A.IPDNO, A.PANO, A.TREATNO, ";
                SQL = SQL + ComNum.VBLF + "     USED, B.ROWID AS ROWID2, A.RETURN_TEXT, A.RETURN_TEXT2, A.RETRY, A.RE_SEQNO, A.BIGO ";

                if (ChkYakuk(clsType.User.Sabun) == false)
                {
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_PCONSULT A, " + ComNum.DB_ERP + "DRUG_PCONSULT_RETURN B, " + ComNum.DB_PMPA + "IPD_NEW_MASTER C ";

                    if (txtPANO.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + txtPANO.Text.Trim() + "'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE A.WRITEDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD HH24:MI') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.WRITEDATE <= TO_DATE('" + strEDate + "','YYYY-MM-DD HH24:MI') ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = C.PANO";
                    SQL = SQL + ComNum.VBLF + "         AND A.IPDNO = C.IPDNO ";

                    if (cboWard.Text.Trim() != "전체")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND C.WARDCODE = '" + cboWard.Text + "' ";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_PCONSULT A, " + ComNum.DB_ERP + "DRUG_PCONSULT_RETURN B ";

                    if (txtPANO.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + txtPANO.Text.Trim() + "'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE A.WRITEDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD HH24:MI') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.WRITEDATE <= TO_DATE('" + strEDate + "','YYYY-MM-DD HH24:MI') ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "         AND A.PROGRESS >= '0' ";
                SQL = SQL + ComNum.VBLF + "         AND A.DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = B.SEQNO(+)";

                if (rdoGbn1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND PROGRESS = '1'";
                }
                else if (rdoGbn2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND PROGRESS = '2'";
                }
                else if (rdoGbn3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND PROGRESS = 'C'";
                }

                if (VB.Left(cboBun.Text, 2) != "**")
                {
                    if (cboCode.Text.Trim() == "전체")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND ORDERCODE IN (" + setCode(VB.Left(cboBun.Text, 2)) + ") ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         AND ORDERCODE = '" + cboCode.Text.Trim() + "' ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY A.WRITEDATE ASC ";

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
                        ssView_Sheet1.Cells[i, 0].Text = readProgress(dt.Rows[i]["PROGRESS"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["WDATE"].ToString().Trim();

                        if (dt.Rows[i]["BIGO"].ToString().Trim().IndexOf("자동발생") == -1)
                        {
                            ssView_Sheet1.Cells[i, 2].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["WRITESABUN"].ToString().Trim());
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 2].Text = "약제팀";
                        }

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     ROWID";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST ";
                        SQL = SQL + ComNum.VBLF + "     WHERE BUSE IN ('044101', '044100') ";
                        SQL = SQL + ComNum.VBLF + "         AND SABUN = '" + ComFunc.LPAD(dt.Rows[i]["WRITESABUN"].ToString().Trim(), 5, "0") + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[i, 2].Text = "";
                        }

                        dt1.Dispose();
                        dt1 = null;

                        ssView_Sheet1.Cells[i, 11].Text = setBun(dt.Rows[i]["ORDERCODE"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["USED"].ToString().Trim() == "0" ? "최초" : "있음";
                        ssView_Sheet1.Cells[i, 14].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 15].Text = dt.Rows[i]["ROWID2"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 16].Text = dt.Rows[i]["RETURN_TEXT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 19].Tag = "";

                        if (dt.Rows[i]["IPDNO"].ToString().Trim() != "0")
                        {
                            SetPatInfo(dt.Rows[i]["IPDNO"].ToString().Trim(), i, ssView);
                        }
                        else if (dt.Rows[i]["TREATNO"].ToString().Trim().To<int>(0) != 0)
                        {
                            //SetPatInfoOpd(dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["WRITESABUN"].ToString().Trim(), dt.Rows[i]["WDATE"].ToString().Trim(), i, ssView);
                            SetPatInfoOpd2(dt.Rows[i]["TREATNO"].ToString().Trim(), i, ssView);
                        }


                        ssView_Sheet1.Cells[i, 18].Text = ReConsultYN(dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["ORDERCODE"].ToString().Trim(), dt.Rows[i]["SEQNO"].ToString().Trim());

                        //if (dt.Rows[i]["RETRY"].ToString().Trim() == "1")
                        //{
                        //    ssView_Sheet1.Cells[i, 17].Text = dt.Rows[i]["RE_SEQNO"].ToString().Trim();
                        //}


                        ssView_Sheet1.Cells[i, 21].Text = READ_REPLY_NAME(dt.Rows[i]["SEQNO"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 22].Text = READ_REPLY_DATE(dt.Rows[i]["SEQNO"].ToString().Trim());
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

        private void clearPCONSULT()
        {
            GstrIPDNO = "";
            GstrPANO = "";
            GstrPROGRESS = "";
            GstrROWID = "";
            GstrSEQNO = "";
        }

        private string setCode(string strBun)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string rtnVal = "";

            //cboCode.Text = "";
            //cboCode.Items.Clear();
            //cboCode.Items.Add("전체");

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     JEPCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_SETCODE ";
                SQL = SQL + ComNum.VBLF + "     WHERE DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '11' ";
                SQL = SQL + ComNum.VBLF + "         AND PART = '" + strBun + "' ";
                SQL = SQL + ComNum.VBLF + "         AND JEPCODE <> '분류명칭' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY JEPCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    //rtnVal = rtnVal + dt.Rows[i]["JEPCODE"].ToString().Trim() + "', '";

                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        rtnVal = rtnVal + dt.Rows[i]["JEPCODE"].ToString().Trim() + "', '";
                    }
                }

                dt.Dispose();
                dt = null;

                rtnVal = "'" + VB.Mid(rtnVal, 1, rtnVal.Length - 3);

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

        private string readProgress(string strGubun)
        {
            string rtnVal = "";

            switch (strGubun)
            {
                case "1": rtnVal = ""; break;
                case "2": rtnVal = "진행중"; break;
                case "3": rtnVal = "취소"; break;
                case "C": rtnVal = "완료"; break;
            }

            return rtnVal;
        }

        private string setBun(string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     COMMENTS";
                SQL = SQL + ComNum.VBLF + "From " + ComNum.DB_ERP + "DRUG_SETCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE PART IN ";
                SQL = SQL + ComNum.VBLF + "             (SELECT PART FROM " + ComNum.DB_ERP + "DRUG_SETCODE";
                SQL = SQL + ComNum.VBLF + "                 WHERE GUBUN = '11'";
                SQL = SQL + ComNum.VBLF + "                     AND JEPCODE = '" + strCode + "')";
                SQL = SQL + ComNum.VBLF + "         AND JEPCODE = '분류명칭'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["COMMENTS"].ToString().Trim();
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

        private void SetPatInfo(string strIPDNO, int intRow, FarPoint.Win.Spread.FpSpread spd)
        {
            if (strIPDNO.IsNullOrEmpty())
                return;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                                                                                           ";
                SQL = SQL + ComNum.VBLF + "    A.WARDCODE, A.ROOMCODE, A.PANO, A.SNAME, A.AGE, A.SEX, A.DEPTCODE, IPDNO, A.DRCODE, B.DRNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER A                                                              ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_OCS.OCS_DOCTOR B                                                              ";
                SQL = SQL + ComNum.VBLF + "    ON A.DRCODE = B.DRCODE                                                                       ";
                SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + strIPDNO;
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    spd.ActiveSheet.Cells[intRow, 3].Text = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 4].Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 5].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 6].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 7].Text = dt.Rows[0]["AGE"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 8].Text = dt.Rows[0]["SEX"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 9].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 10].Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 19].Text = dt.Rows[0]["IPDNO"].ToString().Trim();
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

        private void SetPatInfoOpd(string strPANO, string strSABUN, string strBDATE, int intRow, FarPoint.Win.Spread.FpSpread spd)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ACTDATE, PANO, SNAME, AGE, SEX, A.DEPTCODE, A.DRCODE, B.DRNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER A ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_OCS.OCS_DOCTOR B ";
                SQL = SQL + ComNum.VBLF + "ON A.DRCODE = B.DRCODE ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PANO = '"+ strPANO + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.DRCODE = (SELECT DRCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE SABUN = '"+ strSABUN + "' AND ROWNUM = 1)   ";
                SQL = SQL + ComNum.VBLF + "  AND A.ACTDATE IN( ";
                SQL = SQL + ComNum.VBLF + "                SELECT MAX(ACTDATE) ACTDATE ";
                SQL = SQL + ComNum.VBLF + "                  FROM KOSMOS_PMPA.OPD_MASTER A ";
                SQL = SQL + ComNum.VBLF + "                 WHERE A.PANO = '"+ strPANO + "' ";
                SQL = SQL + ComNum.VBLF + "                   AND A.DRCODE = (SELECT DRCODE FROM KOSMOS_OCS.OCS_DOCTOR WHERE SABUN = '"+ strSABUN + "' AND ROWNUM = 1) ";
                SQL = SQL + ComNum.VBLF + "                   AND A.ACTDATE <= TO_DATE('"+ strBDATE + "', 'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "                ) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    //spd.ActiveSheet.Cells[intRow, 3].Text = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    //spd.ActiveSheet.Cells[intRow, 4].Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 5].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 6].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 7].Text = dt.Rows[0]["AGE"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 8].Text = dt.Rows[0]["SEX"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 9].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 10].Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                    //spd.ActiveSheet.Cells[intRow, 19].Text = dt.Rows[0]["IPDNO"].ToString().Trim();
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

        private void SetPatInfoOpd2(string strTREATNO, int intRow, FarPoint.Win.Spread.FpSpread spd)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT   PATID                                           ";
                SQL = SQL + ComNum.VBLF + "     ,   O.SNAME                                         ";
                SQL = SQL + ComNum.VBLF + "     ,   O.AGE                                           ";
                SQL = SQL + ComNum.VBLF + "     ,   O.SEX                                           ";
                SQL = SQL + ComNum.VBLF + "     ,   O.DEPTCODE                                      ";
                SQL = SQL + ComNum.VBLF + "     ,   B.DRNAME                                        ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_TREATT A                           ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_OCS.OCS_DOCTOR B                      ";
                SQL = SQL + ComNum.VBLF + "    ON A.DOCCODE = TRIM(B.DOCCODE)                       ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_PMPA.OPD_MASTER O                     ";
                SQL = SQL + ComNum.VBLF + "    ON B.DRCODE = O.DRCODE                               ";
                SQL = SQL + ComNum.VBLF + "   AND A.PATID  = TRIM(O.PANO)                           ";
                SQL = SQL + ComNum.VBLF + "   AND O.BDATE  = TO_DATE(A.INDATE, 'YYYYMMDD')          ";
                SQL = SQL + ComNum.VBLF + "WHERE A.TREATNO = " + strTREATNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    //spd.ActiveSheet.Cells[intRow, 3].Text = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    //spd.ActiveSheet.Cells[intRow, 4].Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 5].Text = dt.Rows[0]["PATID"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 6].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 7].Text = dt.Rows[0]["AGE"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 8].Text = dt.Rows[0]["SEX"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 9].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 10].Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                    spd.ActiveSheet.Cells[intRow, 19].Tag = strTREATNO;
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";

            strFont1 = "/fn\"맑은 고딕\" /fz\"20\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"10\" /fb1 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "복약상담 의뢰내역" + "/f1/n";
            strHead2 = "/l/f2" + "출력일자 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "/f2/n";

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 20;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.Margin.Header = 10;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.UseSmartPrint = false;
            ssView_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssView_Sheet1.PrintInfo.Preview = false;
            ssView.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPharConsultReturn_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ssView_Sheet1.RowCount == 0) { return; }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strSIMPLE = "";
            string strJepCode = "";


            if (GstrGubun == "NEW")
            {
                ssViewCellDoubleClick(ssView_Sheet1.ActiveRowIndex, ssView_Sheet1.ActiveColumnIndex, ssView);
                return;
            }

            clearPCONSULT();

            GstrPROGRESS = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 0].Text.Trim();
            GstrPANO = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 5].Text.Trim();
            strJepCode = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 12].Text.Trim();
            GstrSEQNO = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 14].Text.Trim();
            GstrROWID = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 15].Text.Trim();

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PART, A.COMMENTS AS GUBUN, B.JEPCODE, B.COMMENTS";
                SQL = SQL + ComNum.VBLF + "FROM";
                SQL = SQL + ComNum.VBLF + "     (SELECT * FROM " + ComNum.DB_ERP + "DRUG_SETCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE GUBUN = '11'";
                SQL = SQL + ComNum.VBLF + "             AND JEPCODE = '분류명칭'";
                SQL = SQL + ComNum.VBLF + "             AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "     ORDER BY PART) A,";
                SQL = SQL + ComNum.VBLF + "     (SELECT * FROM " + ComNum.DB_ERP + "DRUG_SETCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE GUBUN = '11'";
                SQL = SQL + ComNum.VBLF + "             AND JEPCODE <> '분류명칭'";
                SQL = SQL + ComNum.VBLF + "             AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "     ORDER BY JEPCODE) B";
                SQL = SQL + ComNum.VBLF + "Where a.PART = b.PART";
                SQL = SQL + ComNum.VBLF + "  AND A.PART IN  ('01','02')";
                SQL = SQL + ComNum.VBLF + "  AND B.JEPCODE = '" + strJepCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    strSIMPLE = "OK";
                }
                else
                {
                    strSIMPLE = "NO";
                }

                dt.Dispose();
                dt = null;

                if (strSIMPLE == "OK")
                {
                    if (GstrSEQNO != "")
                    {
                        using (frmComSupPharConsultReturnSimple frm = new frmComSupPharConsultReturnSimple(GstrSEQNO, GstrIPDNO, GstrPANO, GstrPROGRESS, strJepCode))
                        {
                            frm.StartPosition = FormStartPosition.CenterParent;
                            frm.ShowDialog();
                        }
                    }
                }
                else
                {
                    if (GstrSEQNO != "")
                    {
                        using (frmComSupPharConsultReturnDetail frm = new frmComSupPharConsultReturnDetail(GstrSEQNO, GstrIPDNO, GstrPANO, GstrPROGRESS, strJepCode))
                        {
                            frm.StartPosition = FormStartPosition.CenterParent;
                            frm.ShowDialog();
                        }
                    }
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

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPano = ssView_Sheet1.Cells[e.Row, 5].Text.Trim();
            GetAllData(strPano);
            ssViewCellDoubleClick(e.Row, e.Column, ssView);
        }

        private void ssViewCellDoubleClick(int iRow, int iCol, FarPoint.Win.Spread.FpSpread spd)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strPART = "";
            string strDRUGCODE = spd.ActiveSheet.Cells[iRow, 12].Text.Trim();
            string strPano = spd.ActiveSheet.Cells[iRow, 5].Text.Trim();
            string strPROGRESS = spd.ActiveSheet.Cells[iRow, 0].Text.Trim();
            string strSEQNO = spd.ActiveSheet.Cells[iRow, 14].Text.Trim();
            string strIPDNO = spd.ActiveSheet.Cells[iRow, 19].Text.Trim();
            string strTREATNO = spd.ActiveSheet.Cells[iRow, 19].Tag.ToString().Trim();

            if (GstrGubun != "NEW") return;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PART, A.COMMENTS AS GUBUN, B.JEPCODE, B.COMMENTS";
                SQL = SQL + ComNum.VBLF + "FROM ";
                SQL = SQL + ComNum.VBLF + "     (SELECT PART, COMMENTS    FROM " + ComNum.DB_ERP + "DRUG_SETCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE GUBUN = '11'";
                SQL = SQL + ComNum.VBLF + "             AND JEPCODE = '분류명칭'";
                SQL = SQL + ComNum.VBLF + "             AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "     ORDER BY PART) A,";
                SQL = SQL + ComNum.VBLF + "     (SELECT JEPCODE, COMMENTS, PART FROM " + ComNum.DB_ERP + "DRUG_SETCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE GUBUN = '11'";
                SQL = SQL + ComNum.VBLF + "             AND JEPCODE <> '분류명칭'";
                SQL = SQL + ComNum.VBLF + "             AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "     ORDER BY JEPCODE) B";
                SQL = SQL + ComNum.VBLF + "WHERE A.PART = B.PART";
                //SQL = SQL + ComNum.VBLF + "     AND A.PART IN ('01', '02')";
                SQL = SQL + ComNum.VBLF + "     AND B.JEPCODE = '" + strDRUGCODE + "' ";

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
                    strPART = dt.Rows[0]["PART"].ToString().Trim();

                    using (frmComSupPharConsultReturnDetailNew f = new frmComSupPharConsultReturnDetailNew(strPART, strSEQNO, strIPDNO, strPano, strPROGRESS, strDRUGCODE))
                    {
                        f.GstrTREATNO = strTREATNO;
                        f.StartPosition = FormStartPosition.CenterParent;
                        f.ShowDialog();
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

        private string ReConsultYN(string strPANO, string strORDERCODE, string strSEQNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";
            
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";                
                SQL = SQL + ComNum.VBLF + "SELECT SEQNO FROM KOSMOS_ADM.DRUG_PCONSULT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPANO + "'            ";
                SQL = SQL + ComNum.VBLF + "   AND ORDERCODE = '" + strORDERCODE + "'  ";
                SQL = SQL + ComNum.VBLF + "   AND PROGRESS NOT IN ('3')               ";
                SQL = SQL + ComNum.VBLF + "   AND SEQNO <> '" + strSEQNO + "'         ";

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
                    rtnVal = "○";
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

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPano = ssView_Sheet1.Cells[e.Row, 5].Text.Trim();
            GetAllData(strPano);
        }
        
        private string READ_REPLY_NAME(string strSEQNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ITEMVALUE FROM KOSMOS_ADM.DRUG_PCONSULT_ROW ";
                SQL = SQL + ComNum.VBLF + " WHERE SEQNO = '" + strSEQNO + "'                 ";
                SQL = SQL + ComNum.VBLF + "   AND ITEMCD = 'I00000033'                        ";

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
                    rtnVal = dt.Rows[0]["ITEMVALUE"].ToString().Trim();
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

        private string READ_REPLY_DATE(string strSEQNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ITEMVALUE FROM KOSMOS_ADM.DRUG_PCONSULT_ROW ";
                SQL = SQL + ComNum.VBLF + " WHERE SEQNO = '" + strSEQNO + "'                 ";
                SQL = SQL + ComNum.VBLF + "   AND ITEMCD = 'I00000031'                        ";
                
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
                    rtnVal = dt.Rows[0]["ITEMVALUE"].ToString().Trim();
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

        private void GetAllData(string strPano)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;
            
            ssView2_Sheet1.RowCount = 0;
            //clearPCONSULT();
            
            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PROGRESS, TO_CHAR(A.WRITEDATE,'YYYY-MM-DD') AS WDATE, A.WRITESABUN, ORDERCODE, A.SEQNO, A.IPDNO, A.TREATNO, A.PANO,  ";
                SQL = SQL + ComNum.VBLF + "     USED, B.ROWID AS ROWID2, A.RETURN_TEXT, A.RETURN_TEXT2, A.RETRY, A.RE_SEQNO, A.BIGO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_PCONSULT A, " + ComNum.DB_ERP + "DRUG_PCONSULT_RETURN B ";                
                SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + strPano + "'";                
                SQL = SQL + ComNum.VBLF + "         AND A.PROGRESS >= '0' ";
                SQL = SQL + ComNum.VBLF + "         AND A.DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = B.SEQNO(+)";                
                SQL = SQL + ComNum.VBLF + "ORDER BY A.WRITEDATE ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView2_Sheet1.RowCount = dt.Rows.Count;
                    ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView2_Sheet1.Cells[i, 0].Text = readProgress(dt.Rows[i]["PROGRESS"].ToString().Trim());
                        ssView2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["WDATE"].ToString().Trim();

                        if (dt.Rows[i]["BIGO"].ToString().Trim().IndexOf("자동발생") == -1)
                        {
                            ssView2_Sheet1.Cells[i, 2].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["WRITESABUN"].ToString().Trim());
                        }
                        else
                        {
                            ssView2_Sheet1.Cells[i, 2].Text = "약제팀";
                        }

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     ROWID";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST ";
                        SQL = SQL + ComNum.VBLF + "     WHERE BUSE IN ('044101', '044100') ";
                        SQL = SQL + ComNum.VBLF + "         AND SABUN = '" + ComFunc.LPAD(dt.Rows[i]["WRITESABUN"].ToString().Trim(), 5, "0") + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            ssView2_Sheet1.Cells[i, 2].Text = "";
                        }

                        dt1.Dispose();
                        dt1 = null;

                        ssView2_Sheet1.Cells[i, 11].Text = setBun(dt.Rows[i]["ORDERCODE"].ToString().Trim());
                        ssView2_Sheet1.Cells[i, 12].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 13].Text = dt.Rows[i]["USED"].ToString().Trim() == "0" ? "최초" : "있음";
                        ssView2_Sheet1.Cells[i, 14].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 15].Text = dt.Rows[i]["ROWID2"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 16].Text = dt.Rows[i]["RETURN_TEXT"].ToString().Trim();

                        if (dt.Rows[i]["IPDNO"].ToString().Trim() != "")
                        {
                            SetPatInfo(dt.Rows[i]["IPDNO"].ToString().Trim(), i, ssView2);
                        }
                        else
                        {
                            //SetPatInfoOpd(dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["WRITESABUN"].ToString().Trim(), dt.Rows[i]["WDATE"].ToString().Trim(), i, ssView2);
                            SetPatInfoOpd2(dt.Rows[i]["TREATNO"].ToString().Trim(), i, ssView2);
                        }

                        ssView2_Sheet1.Cells[i, 18].Text = ReConsultYN(dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["ORDERCODE"].ToString().Trim(), dt.Rows[i]["SEQNO"].ToString().Trim());
                        
                        ssView2_Sheet1.Cells[i, 21].Text = READ_REPLY_NAME(dt.Rows[i]["SEQNO"].ToString().Trim());
                        ssView2_Sheet1.Cells[i, 22].Text = READ_REPLY_DATE(dt.Rows[i]["SEQNO"].ToString().Trim());
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

        private void ssView2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssViewCellDoubleClick(e.Row, e.Column, ssView2);
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
                using (frmEmrViewer f = new frmEmrViewer(ssView.ActiveSheet.Cells[ssView.ActiveSheet.ActiveRowIndex, 5].Text.Trim()))
                {
                    f.ShowDialog();
                }
            }
        }
    }
}
