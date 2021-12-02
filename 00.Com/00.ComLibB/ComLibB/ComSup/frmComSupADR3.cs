using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupADR3.cs
    /// Description     : 약물이상반응(ADR) 인과성 평가 2차
    /// Author          : 이정현
    /// Create Date     : 2018-01-15
    /// <history> 
    /// 약물이상반응(ADR) 인과성 평가 2차
    /// </history>
    /// <seealso>
    /// PSMH\drug\dradr\FrmADR3.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\dradr\dradr.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupADR3 : Form
    {
        private frmComSupADR1_1 frmComSupADR1_1Event = null;
        private frmComSupADR2_1 frmComSupADR2_1Event = null;
        private frmComSupADR3_1 frmComSupADR3_1Event = null;

        private string GstrSEQNO = "";
        private string GstrROWID = "";

        public frmComSupADR3()
        {
            InitializeComponent();
        }

        public frmComSupADR3(string strSEQNO)
        {
            InitializeComponent();

            GstrSEQNO = strSEQNO;
        }

        private void frmComSupADR3_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnSaveTemp.Enabled = false;

            if (clsType.User.BuseCode == "044101" || VB.Left(clsType.User.BuseCode, 2) == "01")
            {
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                btnSaveTemp.Enabled = true;
            }

            dtpwdate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            frmComSupADR1_1Event = new frmComSupADR1_1(GstrSEQNO);

            if (frmComSupADR1_1Event != null)
            {
                pSubFormToControl(frmComSupADR1_1Event, panADR1_1);
            }

            frmComSupADR2_1Event = new frmComSupADR2_1(GstrSEQNO);

            if (frmComSupADR2_1Event != null)
            {
                pSubFormToControl(frmComSupADR2_1Event, panADR2_1);
            }

            frmComSupADR3_1Event = new frmComSupADR3_1(GstrSEQNO);

            if (frmComSupADR3_1Event != null)
            {
                pSubFormToControl(frmComSupADR3_1Event, panADR3_1);
            }

            if (GstrSEQNO != "")
            {
                GstrROWID = readROWID(clsDB.DbCon, GstrSEQNO);
                dataView();

                switch (READ_TEMP(GstrSEQNO))
                {
                    case "Y":
                        lblTemp.Visible = true;
                        break;
                    default:
                        lblTemp.Visible = false;
                        break;
                }
            }
        }

        private void dataView()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     WDATE, WNAME, WBUSE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR3";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + GstrROWID + "' ";

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
                    dtpwdate.Value = Convert.ToDateTime(dt.Rows[0]["WDATE"].ToString().Trim());
                    txtwname.Text = dt.Rows[0]["WNAME"].ToString().Trim();
                    txtwbuse.Text = dt.Rows[0]["WBUSE"].ToString().Trim();
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

        private void pSubFormToControl(Form frm, Control pControl)
        {
            frm.Owner = this;
            frm.TopLevel = false;
            this.Controls.Add(frm);
            frm.Parent = pControl;
            frm.Text = "";
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.WindowState = FormWindowState.Normal;
            frm.Height = pControl.Height;
            frm.Width = pControl.Width;
            //frm.Dock = DockStyle.Fill;
            frm.Show();
        }

        private string readROWID(PsmhDb pDbCon, string strSEQNO)
        {
            if (ComQuery.IsJobAuth(this, "R", pDbCon) == false) return ""; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT ROWID FROM " + ComNum.DB_ERP + "DRUG_ADR3";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["ROWID"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private string READ_TEMP(string strSEQNO)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ""; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT TEMP FROM " + ComNum.DB_ERP + "DRUG_ADR3";
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
                    rtnVal = dt.Rows[0]["TEMP"].ToString().Trim();
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

        private void btnSaveTemp_Click(object sender, EventArgs e)
        {
            if (SAVE_DATA("TEMP") == true) { }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SAVE_DATA() == true) { }
        }

        private bool SAVE_DATA(string strGBN = "")
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strROWID = "";
            string strSUCODE = "";
            string strSUNAMEK = "";
            string strINSPEED = "";
            string strQTY = "";
            string strSERIALNO = "";
            string strDRBUN = "";
            string strEFFECT = "";
            string strGBDIV = "";
            string strDOSNAME = "";
            string strCheck1 = "";
            string strCheck2 = "";
            string strCheck3 = "";
            int nSabun = 0;

            if (txtwname.Text == "")
            {
                ComFunc.MsgBox("작성자가 공란입니다. 작성자 란에 사번을 입력하세요.");
                return rtnVal;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (GstrSEQNO == "")
                {
                    GstrSEQNO = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_ERP.Replace(".", ""), "SEQ_ADR").ToString();
                }
                else
                {
                    strROWID = readROWID(clsDB.DbCon, GstrSEQNO);

                    if (strROWID != "")
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR3_HISTORY";
                        SQL = SQL + ComNum.VBLF + "     SELECT * FROM " + ComNum.DB_ERP + "DRUG_ADR3";
                        SQL = SQL + ComNum.VBLF + "         WHERE ROWID = '" + strROWID + "' ";

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
                        SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR3";
                        SQL = SQL + ComNum.VBLF + "         WHERE ROWID = '" + strROWID + "' ";

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

                if (txtwSabun.Text.Trim() != "")
                {
                    nSabun = Convert.ToInt32(txtwSabun.Text.Trim());
                }

                SQL = frmComSupADR3_1Event.SaveData(dtpwdate.Value.ToString("yyyy-MM-dd"), txtwname.Text, txtwbuse.Text, clsType.User.Sabun, nSabun);

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (setConfirmAllergy(clsDB.DbCon, GstrSEQNO, SQL, ref SqlErr, ref intRowAffected) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "";
                SQL = "UPDATE " + ComNum.DB_ERP + "DRUG_ADR3";
                SQL = SQL + ComNum.VBLF + "     SET";

                if (strGBN == "TEMP")
                {
                    SQL = SQL + ComNum.VBLF + "         TEMP = 'Y' ";
                    lblTemp.Visible = true;
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         TEMP = NULL ";
                    lblTemp.Visible = false;
                }

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

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
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

        private bool setConfirmAllergy(PsmhDb pDbCon, string strSEQNO, string SQL, ref string SqlErr, ref int intRowAffected)
        {
            DataTable dt = null;
            bool rtnVal = false;

            string strPANO = "";
            string strSNAME = "";
            string strRemark = "";
            string strREMARK1 = "";
            string strREMARK2 = "";
            string strREMARK3 = "";

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
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    rtnVal = true;
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PTNO, SNAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR1 ";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strPANO = dt.Rows[0]["PTNO"].ToString().Trim();
                    strSNAME = dt.Rows[0]["SNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     REMARK ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_ALLERGY_MST ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPANO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND CODE = '004' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_ALLERGY_MST_HIS ";
                    SQL = SQL + ComNum.VBLF + "     (PANO, SNAME, CODE, ENTDATE, REMARK, WRITEDATE, SABUN, DELSABUN) ";
                    SQL = SQL + ComNum.VBLF + "     SELECT";
                    SQL = SQL + ComNum.VBLF + "         PANO, SNAME, CODE, ENTDATE, REMARK, SYSDATE, SABUN, " + clsType.User.Sabun;
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "ETC_ALLERGY_MST ";
                    SQL = SQL + ComNum.VBLF + "         WHERE PANO = '" + strPANO + "' ";
                    SQL = SQL + ComNum.VBLF + "             AND CODE = '004' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = "DELETE " + ComNum.DB_PMPA + "ETC_ALLERGY_MST ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPANO + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND CODE = '004'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        return rtnVal;
                    }
                }

                dt.Dispose();
                dt = null;

                strREMARK1 = "※의심의약품 : " + readOrder(pDbCon, strSEQNO);
                strREMARK2 = "※약물이상반응 : " + readReaction(pDbCon, strSEQNO);
                strREMARK3 = "※인과관계평가 : " + ReadRelation(pDbCon, strSEQNO);

                strRemark = strREMARK1 + ComNum.VBLF + strREMARK2 + ComNum.VBLF + strREMARK3;

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_ALLERGY_MST";
                SQL = SQL + ComNum.VBLF + "     (PANO, SNAME, CODE, ENTDATE, REMARK, SABUN)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         '" + strPANO + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strSNAME + "', ";
                SQL = SQL + ComNum.VBLF + "         '004', ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "         '" + strRemark + "', ";
                SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun;
                SQL = SQL + ComNum.VBLF + "     )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    return rtnVal;
                }

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

                SqlErr = ex.Message;
                return rtnVal;
            }
        }

        private string readOrder(PsmhDb pDbCon, string strSEQNO)
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
                SQL = SQL + ComNum.VBLF + "     SUCODE, SUNAMEK ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR1_ORDER";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUCODE, SUNAMEK ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR2_ORDER";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUCODE, SUNAMEK ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR1_ORDER_JO";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;
                SQL = SQL + ComNum.VBLF + "ORDER BY SUCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        rtnVal = rtnVal + dt.Rows[i]["SUCODE"].ToString().Trim() + "(" + dt.Rows[i]["SUNAMEK"].ToString().Trim() + "), ";
                    }

                    rtnVal = VB.Mid(rtnVal, 1, rtnVal.Length - 2);
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

                return rtnVal;
            }
        }

        private string readReaction(PsmhDb pDbCon, string strSEQNO)
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
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_A1, '1', '발열, ', '') || DECODE(A.RACT_A2, '1', '식욕감소, ', '') || DECODE(A.RACT_A3, '1', '전신부종, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_A4, '1', '전신쇠약, ', '') || DECODE(A.RACT_A5, '1', '체중감소, ', '') || DECODE(A.RACT_A6, '1', '체중증가, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_B1, '1', '가려움증, ', '') || DECODE(A.RACT_B2, '1', '가려운 발진, ', '') || DECODE(A.RACT_B3, '1', '농포성 발진, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_B4, '1', '두드러기, ', '') || DECODE(A.RACT_B5, '1', '여드름성 발진, ', '') || DECODE(A.RACT_B6, '1', '피부작리, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_B7, '1', '피부변색, ', '') || DECODE(A.RACT_B8, '1', '혈관부종, ', '') || DECODE(A.RACT_B9, '1', '탈모, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_C1, '1', '구강칸디다증, ', '') || DECODE(A.RACT_C2, '1', '구강건조증, ', '') || DECODE(A.RACT_C3, '1', '귀울림, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_C4, '1', '급성청각이상, ', '') || DECODE(A.RACT_C5, '1', '미각이상, ', '') || DECODE(A.RACT_C6, '1', '시각장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_C7, '1', '안압상승, ', '') || DECODE(A.RACT_C8, '1', '음성변화, ', '') || DECODE(A.RACT_D1, '1', '가슴불편함, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_D2, '1', '부정맥, ', '') || DECODE(A.RACT_D3, '1', '빈맥, ', '') || DECODE(A.RACT_D4, '1', '실신, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_D5, '1', '심부전, ', '') || DECODE(A.RACT_D6, '1', '저혈압, ', '') || DECODE(A.RACT_D7, '1', '고혈압, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_E1, '1', '오심/구토, ', '') || DECODE(A.RACT_E2, '1', '변비, ', '') || DECODE(A.RACT_E3, '1', '복통, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_E4, '1', '설사, ', '') || DECODE(A.RACT_E5, '1', '소화불량, ', '') || DECODE(A.RACT_E6, '1', '위장관통증, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_E7, '1', '위출혈, ', '') || DECODE(A.RACT_F1, '1', '빌리루빈증가, ', '') || DECODE(A.RACT_F2, '1', 'AST/ALT 증가, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_G1, '1', '기침, ', '') || DECODE(A.RACT_G2, '1', '호흡곤란, ', '') || DECODE(A.RACT_G3, '1', '폐부종, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_H1, '1', '백혈구감소증, ', '') || DECODE(A.RACT_H2, '1', '빈혈, ', '') || DECODE(A.RACT_H3, '1', '응고장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_H4, '1', '혈소판감소증, ', '') || DECODE(A.RACT_I1, '1', '단백뇨, ', '') || DECODE(A.RACT_I2, '1', '신기능장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_I3, '1', '혈뇨, ', '') || DECODE(A.RACT_I4, '1', '혈중 Creatinine 증가, ', '') || DECODE(A.RACT_J1, '1', '기억력장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_J2, '1', '두통, ', '') || DECODE(A.RACT_J3, '1', '보행곤란, ', '') || DECODE(A.RACT_J4, '1', '사지떨림, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_J5, '1', '수면장애, ', '') || DECODE(A.RACT_J6, '1', '어지러움, ', '') || DECODE(A.RACT_J7, '1', '언어장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_J8, '1', '의식저하, ', '') || DECODE(A.RACT_J9, '1', '운동이상증, ', '') || DECODE(A.RACT_J10, '1', '졸림, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_J11, '1', '피부저림, ', '') || DECODE(A.RACT_J12, '1', '불안, ', '') || DECODE(A.RACT_J13, '1', '섬망, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_J14, '1', '신경과민, ', '') || DECODE(A.RACT_J15, '1', '우울, ', '') || DECODE(A.RACT_J16, '1', '과행동, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_K1, '1', '고혈당증, ', '') || DECODE(A.RACT_K2, '1', '저혈당증, ', '') || DECODE(A.RACT_K3, '1', '배뇨장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_K4, '1', '성기능장애, ', '') || DECODE(A.RACT_K5, '1', '성욕감소, ', '') || DECODE(A.RACT_K6, '1', '여성형유방, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_K7, '1', '월경불순, ', '') || DECODE(A.RACT_L1, '1', '관절통, ', '') || DECODE(A.RACT_L2, '1', '골다공증, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_L3, '1', '근육통, ', '') AS RACT ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR1 A";
                SQL = SQL + ComNum.VBLF + "     Where SEQNO = " + strSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["RACT"].ToString().Trim();

                    if (rtnVal != "")
                    {
                        rtnVal = VB.Mid(rtnVal, 1, rtnVal.Length - 2);
                    }
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

                return rtnVal;
            }
        }

        private string ReadRelation(PsmhDb pDbCon, string strSEQNO)
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
                SQL = SQL + ComNum.VBLF + "     relation1, relation2, relation3, relation4 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR3";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["relation1"].ToString().Trim() == "1")
                    {
                        rtnVal = "확실함(Certain)";
                    }
                    else if (dt.Rows[0]["relation2"].ToString().Trim() == "1")
                    {
                        rtnVal = "상당히 확실함(Probable)";
                    }
                    else if (dt.Rows[0]["relation3"].ToString().Trim() == "1")
                    {
                        rtnVal = "가능함(Possible)";
                    }
                    else if (dt.Rows[0]["relation4"].ToString().Trim() == "1")
                    {
                        rtnVal = "가능성 적음(Unlikely)";
                    }
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

                return rtnVal;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (GstrSEQNO == "")
            {
                ComFunc.MsgBox("저장 된 내용이 없습니다.");
            }
            else
            {
                if (ComFunc.MsgBoxQ("작성 된 내용을 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (DEL_DATA() == true)
                    {
                        this.Close();
                    }
                }
            }
        }

        private bool DEL_DATA()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strROWID = readROWID(clsDB.DbCon, GstrSEQNO);

            if (strROWID == "") { return rtnVal; }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR3_HISTORY";
                SQL = SQL + ComNum.VBLF + "     SELECT * FROM " + ComNum.DB_ERP + "DRUG_ADR3";
                SQL = SQL + ComNum.VBLF + "         WHERE ROWID = '" + strROWID + "' ";

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
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR3";
                SQL = SQL + ComNum.VBLF + "         WHERE ROWID = '" + strROWID + "' ";

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
                ComFunc.MsgBox("삭제하였습니다.");
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            frmComSupPrtADR frm = new frmComSupPrtADR(GstrSEQNO);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            frmComSupADR1_1Event.Dispose();
            frmComSupADR1_1Event = null;

            frmComSupADR2_1Event.Dispose();
            frmComSupADR2_1Event = null;

            frmComSupADR3_1Event.Dispose();
            frmComSupADR3_1Event = null;

            this.Close();
        }

        private void txtwname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //GetSabun();
            }
        }

        private void txtwname_Leave(object sender, EventArgs e)
        {
            //GetSabun();
        }

        private void GetSabun()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (txtwname.Text.Trim() == "") { return; }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strSaBun = txtwname.Text.Trim();
            string strBuse = txtwbuse.Text.Trim();
            string strBUSECODE = "";

            txtwname.Text = "";
            txtwbuse.Text = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (VB.IsNumeric(strSaBun) == true)
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     KORNAME, B.NAME";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_PMPA + "BAS_BUSE B";
                    SQL = SQL + ComNum.VBLF + "     WHERE SABUN = '" + strSaBun + "'";
                    SQL = SQL + ComNum.VBLF + "         AND A.BUSE = B.BUCODE";
                    SQL = SQL + ComNum.VBLF + "         AND A.TOIDAY IS NULL";

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
                        txtwname.Text = dt.Rows[0]["KORNAME"].ToString().Trim();
                        txtwbuse.Text = dt.Rows[0]["NAME"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    if (strBuse != "")
                    {
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     A.BUSE, B.NAME ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_PMPA + "BAS_BUSE B";
                        SQL = SQL + ComNum.VBLF + "     WHERE A.BUSE = B.BUCODE";
                        SQL = SQL + ComNum.VBLF + "         AND A.KORNAME = '" + strSaBun + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND B.NAME = '" + strBuse + "' ";

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
                            strBUSECODE = dt.Rows[0]["BUSE"].ToString().Trim();
                        }

                        dt.Dispose();
                        dt = null;
                    }

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.KORNAME , B.NAME, A.BUSE";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_PMPA + "BAS_BUSE B";
                    SQL = SQL + ComNum.VBLF + "     WHERE KORNAME = '" + strSaBun + "'";
                    SQL = SQL + ComNum.VBLF + "         AND A.BUSE = B.BUCODE";
                    SQL = SQL + ComNum.VBLF + "         AND A.TOIDAY IS NULL";

                    if (strBUSECODE != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.BUSE > '" + strBUSECODE + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "ORDER BY A.BUSE ASC";

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
                        txtwname.Text = strSaBun;
                        //GetSabun();
                    }
                    else
                    {
                        txtwname.Text = dt.Rows[0]["KORNAME"].ToString().Trim();
                        txtwbuse.Text = dt.Rows[0]["NAME"].ToString().Trim();
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

        private void btnEqual_Click(object sender, EventArgs e)
        {
            frmComSupADR3_1Event.GetData(GstrSEQNO, "2");
            //frmComSupADR3_1Event = new frmComSupADR3_1(GstrSEQNO, "2");

            //if (frmComSupADR3_1Event != null)
            //{
            //    pSubFormToControl(frmComSupADR3_1Event, panADR3_1);
            //}
            
            dtpwdate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            txtwname.Text = "";
            txtwbuse.Text = "";
        }

        void read_sabun2Name(string argSabun)
        {
            if (txtwSabun.Text.Trim() == "") { return; }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strSaBun = txtwSabun.Text.Trim();

            if (VB.Len(strSaBun) == 4)
            {
                strSaBun = ComFunc.SetAutoZero(strSaBun, 5);
            }

            txtwname.Text = "";
            txtwbuse.Text = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     KORNAME, B.NAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_PMPA + "BAS_BUSE B";
                SQL = SQL + ComNum.VBLF + "     WHERE SABUN = '" + strSaBun + "'";
                SQL = SQL + ComNum.VBLF + "         AND A.BUSE = B.BUCODE";
                SQL = SQL + ComNum.VBLF + "         AND A.TOIDAY IS NULL";

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
                    txtwname.Text = dt.Rows[0]["KORNAME"].ToString().Trim();
                    txtwbuse.Text = dt.Rows[0]["NAME"].ToString().Trim();
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
                Cursor.Current = Cursors.Default;
            }
        }

        void txtwSabun_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtwSabun.Text.Trim() != "")
                {
                    read_sabun2Name(txtwSabun.Text.Trim());
                }
            }

        }
    }
}
