using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupADR2.cs
    /// Description     : 약물이상반응(ADR) 인과성 평가 1차
    /// Author          : 이정현
    /// Create Date     : 2018-01-15
    /// <history> 
    /// 약물이상반응(ADR) 인과성 평가 1차
    /// </history>
    /// <seealso>
    /// PSMH\drug\dradr\FrmADR2.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\dradr\dradr.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupADR2 : Form
    {
        private frmComSupOrderList frmComSupOrderListEvent = null;

        private string GstrSEQNO      = "";
        private string GstrPANO       = "";
        private string GstrOrderGubun = "";

        public frmComSupADR2()
        {
            InitializeComponent();
        }

        public frmComSupADR2(string strSEQNO, string strPANO)
        {
            InitializeComponent();

            GstrSEQNO = strSEQNO;
            GstrPANO = strPANO;
        }

        private void frmComSupADR2_Load(object sender, EventArgs e)
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

            if (clsType.User.BuseCode == "044101" || VB.Left(clsType.User.BuseCode, 2) == "03")
            {
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                btnSaveTemp.Enabled = true;
            }
            
            SetAllControlClear(this);

            ssDrug_Sheet1.RowCount = 0;
            ssDoubt_Sheet1.RowCount = 0;
            
            if (GstrSEQNO != "")
            {
                dataView(readROWID(clsDB.DbCon, GstrSEQNO));

                switch (READ_TEMP(GstrSEQNO))
                {
                    case "Y":
                        lblTemp.Visible = true;
                        break;
                    default:
                        lblTemp.Visible = false;
                        break;
                }

                viewSS2(GstrSEQNO);
                viewSS3(GstrSEQNO);
            }

            viewSS1(GstrSEQNO);
            viewSS4(GstrSEQNO);
        }

        private void SetAllControlClear(Control objParent, bool bolDtpCheckedDefault = true)
        {
            Control[] controls = ComFunc.GetAllControls(objParent);

            foreach (Control ctl in controls)
            {
                if (ctl is TextBox)
                {
                    ctl.Text = "";
                }
                else if (ctl is CheckBox)
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl is ComboBox)
                {
                    ((ComboBox)ctl).SelectedIndex = -1;
                }
                else if (ctl is DateTimePicker)
                {
                    ((DateTimePicker)ctl).Value = VB.Now();
                    ((DateTimePicker)ctl).Checked = bolDtpCheckedDefault;
                }
            }
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
                SQL = "SELECT ROWID FROM " + ComNum.DB_ERP + "DRUG_ADR2";
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
                SQL = "SELECT TEMP FROM " + ComNum.DB_ERP + "DRUG_ADR2";
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

        private void dataView(string strROWID)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM " + ComNum.DB_ERP + "DRUG_ADR2";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strROWID + "' ";

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
                    dataView(dt);
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

        private void dataView(DataTable dt)
        {
            Control[] controls = ComFunc.GetAllControls(this);
            int i = 0;

            for (i = 0; i < dt.Columns.Count; i++)
            {
                foreach (Control control in controls)
                {
                    if (control.Name.Replace("chk", "").ToUpper().Trim() == dt.Columns[i].ToString())
                    {
                        if (control.Name.Replace("chk", "").Trim() != "gubun_jo")
                        {
                            ((CheckBox)control).ForeColor = Color.Black;
                            ((CheckBox)control).Font = new Font("맑은 고딕", 9f, FontStyle.Regular);
                        }

                        if (dt.Rows[0][i].ToString().Trim() == "1")
                        {
                            ((CheckBox)control).Checked = true;

                            if (control.Name.Replace("chk", "").Trim() != "gubun_jo")
                            {
                                ((CheckBox)control).ForeColor = Color.Red;
                                ((CheckBox)control).Font = new Font("맑은 고딕", 9f, FontStyle.Bold);
                            }
                        }
                    }
                    else if (control.Name.Replace("txt", "").ToUpper().Trim() == dt.Columns[i].ToString())
                    {
                        ((TextBox)control).BackColor = Color.White;

                        if (dt.Rows[0][i].ToString().Trim() != "")
                        {
                            ((TextBox)control).Text = dt.Rows[0][i].ToString().Trim();
                            ((TextBox)control).BackColor = Color.FromArgb(255, 255, 128);
                        }
                    }
                    else if (control.Name.Replace("rdo", "").ToUpper().Trim() == dt.Columns[i].ToString())
                    {
                        ((RadioButton)control).ForeColor = Color.Black;
                        ((RadioButton)control).Font = new Font("맑은 고딕", 9f, FontStyle.Regular);

                        if (dt.Rows[0][i].ToString().Trim() == "1")
                        {
                            ((RadioButton)control).Checked = true;
                            ((RadioButton)control).ForeColor = Color.Red;
                            ((RadioButton)control).Font = new Font("맑은 고딕", 9f, FontStyle.Bold);
                        }
                    }
                    else if (control.Name.Replace("dtp", "").ToUpper().Trim() == dt.Columns[i].ToString())
                    {
                        ((DateTimePicker)control).BackColor = Color.White;

                        if (dt.Rows[0][i].ToString().Trim() != "")
                        {
                            ((DateTimePicker)control).Checked = true;
                            ((DateTimePicker)control).Value = Convert.ToDateTime(dt.Rows[0][i].ToString().Trim());
                            ((DateTimePicker)control).BackColor = Color.FromArgb(255, 255, 128);
                        }
                    }
                    else if (control.Name.Replace("cbo", "").ToUpper().Trim() == dt.Columns[i].ToString())
                    {
                        ((ComboBox)control).BackColor = Color.White;

                        if (dt.Rows[0][i].ToString().Trim() != "")
                        {
                            ((ComboBox)control).Text = dt.Rows[0][i].ToString().Trim();
                            ((ComboBox)control).BackColor = Color.FromArgb(255, 255, 128);
                        }
                    }
                }
            }
        }

        private void viewSS1(string strSEQNO)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssDoubt_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUCODE, SUNAMEK, DRBUN, EFFECT, QTY, GBDIV, DOSNAME, CHECK1, CHECK2, CHECK3, ROWID, DRUGADD";
                SQL = SQL + ComNum.VBLF + "From " + ComNum.DB_ERP + "DRUG_ADR1_ORDER";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO =" + strSEQNO;
                #region 19-05-07 약제팀 데레사 수녀님 요청으로 조영제것도 같이 끌고오게 추가.
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUCODE, SUNAMEK, '', '', QTY, '', '', CHECK1, CHECK2, CHECK3, ROWID, ''";
                SQL = SQL + ComNum.VBLF + "From " + ComNum.DB_ERP + "DRUG_ADR1_ORDER_JO";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO =" + strSEQNO;
                SQL = SQL + ComNum.VBLF + "ORDER BY SUCODE ";
                #endregion

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
                    ssDoubt_Sheet1.RowCount = dt.Rows.Count;
                    ssDoubt_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssDoubt_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssDoubt_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                        ssDoubt_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DRBUN"].ToString().Trim();
                        ssDoubt_Sheet1.Cells[i, 3].Text = dt.Rows[i]["EFFECT"].ToString().Trim();
                        ssDoubt_Sheet1.Cells[i, 4].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        ssDoubt_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                        ssDoubt_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();
                        ssDoubt_Sheet1.Cells[i, 7].Value = dt.Rows[i]["CHECK1"].ToString().Trim() == "1" ? true : false;
                        ssDoubt_Sheet1.Cells[i, 8].Value = dt.Rows[i]["CHECK2"].ToString().Trim() == "1" ? true : false;
                        ssDoubt_Sheet1.Cells[i, 9].Value = dt.Rows[i]["CHECK3"].ToString().Trim() == "1" ? true : false;
                        ssDoubt_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssDoubt_Sheet1.Cells[i, 11].Text = dt.Rows[i]["DRUGADD"].ToString().Trim();

                        if (dt.Rows[i]["DRUGADD"].ToString().Trim() == "Y")
                        {
                            ssDoubt_Sheet1.Cells[i, 0, i, ssDoubt_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 255);
                        }
                        else
                        {
                            ssDoubt_Sheet1.Cells[i, 0, i, ssDoubt_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
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

        private void viewSS2(string strSEQNO)
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
                SQL = SQL + ComNum.VBLF + "     WHO1, WHO2, WHO3, WHO4, WHO5, WHO6 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR2 ";
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
                    ssChk1_Sheet1.Cells[0, 3].Value = dt.Rows[0]["WHO1"].ToString().Trim() == "1" ? true : false;
                    ssChk1_Sheet1.Cells[6, 3].Value = dt.Rows[0]["WHO2"].ToString().Trim() == "1" ? true : false;
                    ssChk1_Sheet1.Cells[12, 3].Value = dt.Rows[0]["WHO3"].ToString().Trim() == "1" ? true : false;
                    ssChk1_Sheet1.Cells[17, 3].Value = dt.Rows[0]["WHO4"].ToString().Trim() == "1" ? true : false;
                    ssChk1_Sheet1.Cells[21, 3].Value = dt.Rows[0]["WHO5"].ToString().Trim() == "1" ? true : false;
                    ssChk1_Sheet1.Cells[25, 3].Value = dt.Rows[0]["WHO6"].ToString().Trim() == "1" ? true : false;
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

        private void viewSS3(string strSEQNO)
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
                SQL = SQL + ComNum.VBLF + "     A1, A2, A3,";
                SQL = SQL + ComNum.VBLF + "     B1, B2, B3, B4,";
                SQL = SQL + ComNum.VBLF + "     C1, C2, C3,";
                SQL = SQL + ComNum.VBLF + "     D1, D2, D3, D4, D5,";
                SQL = SQL + ComNum.VBLF + "     E1, E2, E3, ";
                SQL = SQL + ComNum.VBLF + "     F1, F2, F3,";
                SQL = SQL + ComNum.VBLF + "     G1, G2, G3, G4,";
                SQL = SQL + ComNum.VBLF + "     H1, H2, H3, H4,";
                SQL = SQL + ComNum.VBLF + "     T1, T2, T3, T4,";
                SQL = SQL + ComNum.VBLF + "     TOTAL";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR2";
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
                    ssChk2_Sheet1.Cells[1, 3].Value = dt.Rows[0]["A1"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[2, 3].Value = dt.Rows[0]["A2"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[3, 3].Value = dt.Rows[0]["A3"].ToString().Trim() == "1" ? true : false;

                    ssChk2_Sheet1.Cells[5, 3].Value = dt.Rows[0]["B1"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[6, 3].Value = dt.Rows[0]["B2"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[7, 3].Value = dt.Rows[0]["B3"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[8, 3].Value = dt.Rows[0]["B4"].ToString().Trim() == "1" ? true : false;

                    ssChk2_Sheet1.Cells[10, 3].Value = dt.Rows[0]["C1"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[11, 3].Value = dt.Rows[0]["C2"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[12, 3].Value = dt.Rows[0]["C3"].ToString().Trim() == "1" ? true : false;

                    ssChk2_Sheet1.Cells[14, 3].Value = dt.Rows[0]["D1"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[15, 3].Value = dt.Rows[0]["D2"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[16, 3].Value = dt.Rows[0]["D3"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[17, 3].Value = dt.Rows[0]["D4"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[18, 3].Value = dt.Rows[0]["D5"].ToString().Trim() == "1" ? true : false;

                    ssChk2_Sheet1.Cells[20, 3].Value = dt.Rows[0]["E1"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[21, 3].Value = dt.Rows[0]["E2"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[22, 3].Value = dt.Rows[0]["E3"].ToString().Trim() == "1" ? true : false;

                    ssChk2_Sheet1.Cells[23, 3].Value = dt.Rows[0]["F1"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[24, 3].Value = dt.Rows[0]["F2"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[25, 3].Value = dt.Rows[0]["F3"].ToString().Trim() == "1" ? true : false;

                    ssChk2_Sheet1.Cells[27, 3].Value = dt.Rows[0]["G1"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[28, 3].Value = dt.Rows[0]["G2"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[29, 3].Value = dt.Rows[0]["G3"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[30, 3].Value = dt.Rows[0]["G4"].ToString().Trim() == "1" ? true : false;

                    ssChk2_Sheet1.Cells[32, 3].Value = dt.Rows[0]["H1"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[33, 3].Value = dt.Rows[0]["H2"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[34, 3].Value = dt.Rows[0]["H3"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[35, 3].Value = dt.Rows[0]["H4"].ToString().Trim() == "1" ? true : false;

                    ssChk2_Sheet1.Cells[37, 2].Value = dt.Rows[0]["T1"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[38, 2].Value = dt.Rows[0]["T2"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[39, 2].Value = dt.Rows[0]["T3"].ToString().Trim() == "1" ? true : false;
                    ssChk2_Sheet1.Cells[40, 2].Value = dt.Rows[0]["T4"].ToString().Trim() == "1" ? true : false;

                    ssChk2_Sheet1.Cells[38, 3].Text = dt.Rows[0]["TOTAL"].ToString().Trim();
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

        private void viewSS4(string strSEQNO)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssDrug_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUCODE, SUNAMEK, DRBUN, EFFECT, QTY, GBDIV, DOSNAME, CHECK1, CHECK2, CHECK3, ROWID";
                SQL = SQL + ComNum.VBLF + "From " + ComNum.DB_ERP + "DRUG_ADR2_ORDER";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;
                SQL = SQL + ComNum.VBLF + "ORDER BY SUCODE ";

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
                    ssDrug_Sheet1.RowCount = dt.Rows.Count;
                    ssDrug_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssDrug_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssDrug_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                        ssDrug_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DRBUN"].ToString().Trim();
                        ssDrug_Sheet1.Cells[i, 3].Text = dt.Rows[i]["EFFECT"].ToString().Trim();
                        ssDrug_Sheet1.Cells[i, 4].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        ssDrug_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                        ssDrug_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();
                        ssDrug_Sheet1.Cells[i, 7].Value = dt.Rows[i]["CHECK1"].ToString().Trim() == "1" ? true : false;
                        ssDrug_Sheet1.Cells[i, 8].Value = dt.Rows[i]["CHECK2"].ToString().Trim() == "1" ? true : false;
                        ssDrug_Sheet1.Cells[i, 9].Value = dt.Rows[i]["CHECK3"].ToString().Trim() == "1" ? true : false;
                        ssDrug_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strROWID = "";
            int nSabun = 0;

            if (txtwname.Text.Trim() == "")
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
                        SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR2_HISTORY";
                        SQL = SQL + ComNum.VBLF + "     SELECT * FROM " + ComNum.DB_ERP + "DRUG_ADR2";
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
                        SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR2";
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

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR2";
                SQL = SQL + ComNum.VBLF + "     (SEQNO, PERMIT1, PERMIT2, PERMITMEMO, ";
                SQL = SQL + ComNum.VBLF + "     RELATION1, RELATION2, RELATION3, RELATION4, RELATIONMEMO, ";
                SQL = SQL + ComNum.VBLF + "     CLASS1, CLASS2, CLASS3, CLASS4, REPORT1, REPORT2, ";
                SQL = SQL + ComNum.VBLF + "     WDATE,WSabun, WNAME, WBUSE, WRITEDATE, WRITESABUN)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         " + GstrSEQNO + ", ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdopermit1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdopermit2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtpermitMemo.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorelation1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorelation2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorelation3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorelation4.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtrelationMemo.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoclass1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoclass2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoclass3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoclass4.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkreport1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkreport2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpwdate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "         " + nSabun + ", "; //2019-03-06
                SQL = SQL + ComNum.VBLF + "         '" + txtwname.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtwbuse.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "         '" + clsType.User.Sabun + "' ";
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

                if (saveSS1(clsDB.DbCon, GstrSEQNO, SQL, ref SqlErr, ref intRowAffected) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (saveSS2(clsDB.DbCon, GstrSEQNO, SQL, ref SqlErr, ref intRowAffected) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (saveSS3(clsDB.DbCon, GstrSEQNO, SQL, ref SqlErr, ref intRowAffected) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (saveSS4(clsDB.DbCon, GstrSEQNO, SQL, ref SqlErr, ref intRowAffected) == false)
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
                SQL = "UPDATE " + ComNum.DB_ERP + "DRUG_ADR2";
                SQL = SQL + ComNum.VBLF + "     SET";

                if (strGBN == "TEMP")
                {
                    SQL = SQL + ComNum.VBLF + "         TEMP = 'Y'";
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
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private bool saveSS1(PsmhDb pDbCon, string strSEQNO, string SQL, ref string SqlErr, ref int intRowAffected)
        {
            bool rtnVal = false;
            int i = 0;

            string strSUCODE = "";
            string strSUNAMEK = "";
            string strDRBUN = "";
            string strEFFECT = "";
            string strQTY = "";
            string strGBDIV = "";
            string strDOSNAME = "";
            string strCheck1 = "";
            string strCheck2 = "";
            string strCheck3 = "";
            string strDRUGADD = "";
            string strROWID = "";

            try
            {
                if (strSEQNO != "")
                {
                    SQL = "";
                    SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR1_ORDER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;
                    SQL = SQL + ComNum.VBLF + "         AND DRUGADD = 'Y'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        return rtnVal;
                    }
                }

                for (i = 0; i < ssDoubt_Sheet1.RowCount; i++)
                {
                    strROWID = ssDoubt_Sheet1.Cells[i, 10].Text.Trim();
                    strSUCODE = ssDoubt_Sheet1.Cells[i, 0].Text.Trim();
                    strSUNAMEK = ssDoubt_Sheet1.Cells[i, 1].Text.Trim();
                    strDRBUN = ssDoubt_Sheet1.Cells[i, 2].Text.Trim();
                    strEFFECT = ssDoubt_Sheet1.Cells[i, 3].Text.Trim();
                    strQTY = ssDoubt_Sheet1.Cells[i, 4].Text.Trim();
                    strGBDIV = ssDoubt_Sheet1.Cells[i, 5].Text.Trim();
                    strDOSNAME = ssDoubt_Sheet1.Cells[i, 6].Text.Trim();
                    strCheck1 = Convert.ToBoolean(ssDoubt_Sheet1.Cells[i, 7].Value) == true ? "1" : "0";
                    strCheck2 = Convert.ToBoolean(ssDoubt_Sheet1.Cells[i, 8].Value) == true ? "1" : "0";
                    strCheck3 = Convert.ToBoolean(ssDoubt_Sheet1.Cells[i, 9].Value) == true ? "1" : "0";
                    strDRUGADD = ssDoubt_Sheet1.Cells[i, 11].Text.Trim();

                    if (strDRUGADD == "Y")
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR1_ORDER";
                        SQL = SQL + ComNum.VBLF + "     (SUCODE, SUNAMEK, DRBUN, EFFECT, QTY, GBDIV, DOSNAME, CHECK1, CHECK2, CHECK3, SEQNO, DRUGADD)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         '" + strSUCODE + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strSUNAMEK + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strDRBUN + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strEFFECT + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strQTY + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strGBDIV + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strDOSNAME + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strCheck1 + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strCheck2 + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strCheck3 + "', ";
                        SQL = SQL + ComNum.VBLF + "         " + strSEQNO + ", ";
                        SQL = SQL + ComNum.VBLF + "         '" + strDRUGADD + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            return rtnVal;
                        }
                    }
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                return rtnVal;
            }
        }

        private bool saveSS2(PsmhDb pDbCon, string strSEQNO, string SQL, ref string SqlErr, ref int intRowAffected)
        {
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_ERP + "DRUG_ADR2";
                SQL = SQL + ComNum.VBLF + "     SET ";
                SQL = SQL + ComNum.VBLF + "         WHO1 = '" + (Convert.ToBoolean(ssChk1_Sheet1.Cells[0, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         WHO2 = '" + (Convert.ToBoolean(ssChk1_Sheet1.Cells[6, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         WHO3 = '" + (Convert.ToBoolean(ssChk1_Sheet1.Cells[12, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         WHO4 = '" + (Convert.ToBoolean(ssChk1_Sheet1.Cells[17, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         WHO5 = '" + (Convert.ToBoolean(ssChk1_Sheet1.Cells[21, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         WHO6 = '" + (Convert.ToBoolean(ssChk1_Sheet1.Cells[25, 3].Value) == true ? "1" : "0") + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE SEQNO = " + strSEQNO;

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
                SqlErr = ex.Message;
                return rtnVal;
            }
        }

        private bool saveSS3(PsmhDb pDbCon, string strSEQNO, string SQL, ref string SqlErr, ref int intRowAffected)
        {
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_ERP + "DRUG_ADR2";
                SQL = SQL + ComNum.VBLF + "     SET";
                SQL = SQL + ComNum.VBLF + "         A1 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[1, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         A2 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[2, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         A3 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[3, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         B1 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[5, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         B2 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[6, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         B3 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[7, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         B4 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[8, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         C1 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[10, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         C2 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[11, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         C3 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[12, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         D1 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[14, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         D2 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[15, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         D3 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[16, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         D4 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[17, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         D5 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[18, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         E1 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[20, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         E2 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[21, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         E3 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[22, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         F1 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[23, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         F2 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[24, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         F3 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[25, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         G1 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[27, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         G2 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[28, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         G3 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[29, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         G4 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[30, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         H1 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[32, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         H2 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[33, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         H3 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[34, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         H4 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[35, 3].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         T1 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[37, 2].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         T2 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[38, 2].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         T3 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[39, 2].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         T4 = '" + (Convert.ToBoolean(ssChk2_Sheet1.Cells[40, 2].Value) == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         TOTAL = '" + ssChk2_Sheet1.Cells[38, 3].Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE SEQNO = " + strSEQNO;

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
                SqlErr = ex.Message;
                return rtnVal;
            }
        }

        private bool saveSS4(PsmhDb pDbCon, string strSEQNO, string SQL, ref string SqlErr, ref int intRowAffected)
        {
            bool rtnVal = false;
            int i = 0;

            string strSUCODE = "";
            string strSUNAMEK = "";
            string strDRBUN = "";
            string strEFFECT = "";
            string strQTY = "";
            string strGBDIV = "";
            string strDOSNAME = "";
            string strCheck1 = "";
            string strCheck2 = "";
            string strCheck3 = "";
            string strROWID = "";

            try
            {
                if (strSEQNO != "")
                {
                    SQL = "";
                    SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR2_ORDER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        return rtnVal;
                    }
                }

                for (i = 0; i < ssDrug_Sheet1.RowCount; i++)
                {
                    strROWID = ssDrug_Sheet1.Cells[i, 10].Text.Trim();
                    strSUCODE = ssDrug_Sheet1.Cells[i, 0].Text.Trim();
                    strSUNAMEK = ssDrug_Sheet1.Cells[i, 1].Text.Trim();
                    strDRBUN = ssDrug_Sheet1.Cells[i, 2].Text.Trim();
                    strEFFECT = ssDrug_Sheet1.Cells[i, 3].Text.Trim();
                    strQTY = ssDrug_Sheet1.Cells[i, 4].Text.Trim();
                    strGBDIV = ssDrug_Sheet1.Cells[i, 5].Text.Trim();
                    strDOSNAME = ssDrug_Sheet1.Cells[i, 6].Text.Trim();
                    strCheck1 = Convert.ToBoolean(ssDrug_Sheet1.Cells[i, 7].Value) == true ? "1" : "0";
                    strCheck2 = Convert.ToBoolean(ssDrug_Sheet1.Cells[i, 8].Value) == true ? "1" : "0";
                    strCheck3 = Convert.ToBoolean(ssDrug_Sheet1.Cells[i, 9].Value) == true ? "1" : "0";

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR2_ORDER";
                    SQL = SQL + ComNum.VBLF + "     (SUCODE, SUNAMEK, DRBUN, EFFECT, QTY, GBDIV, DOSNAME, CHECK1, CHECK2, CHECK3, SEQNO)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         '" + strSUCODE + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSUNAMEK + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strDRBUN + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strEFFECT + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strQTY + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGBDIV + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strDOSNAME + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strCheck1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strCheck2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strCheck3 + "', ";
                    SQL = SQL + ComNum.VBLF + "         " + strSEQNO;
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        return rtnVal;
                    }
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
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
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_ALLERGY_MST_HIS (PANO, SNAME, CODE, ENTDATE, REMARK, WRITEDATE, SABUN, DELSABUN) ";
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
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR2";
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
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR2_HISTORY";
                SQL = SQL + ComNum.VBLF + "     SELECT * FROM " + ComNum.DB_ERP + "DRUG_ADR2";
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
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR2";
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDrugAdd_Click(object sender, EventArgs e)
        {
            ssDrug_Sheet1.RowCount = ssDrug_Sheet1.RowCount + 1;
            ssDrug_Sheet1.SetRowHeight(ssDrug_Sheet1.RowCount - 1, ComNum.SPDROWHT);
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            GstrOrderGubun = "1";

            frmComSupOrderListEvent = new frmComSupOrderList(GstrPANO);
            frmComSupOrderListEvent.SendEvent += FrmComSupOrderListEvent_SendEvent;
            frmComSupOrderListEvent.rEventClosed += FrmComSupOrderListEvent_rEventClosed;
            frmComSupOrderListEvent.StartPosition = FormStartPosition.CenterParent;
            frmComSupOrderListEvent.ShowDialog();
        }

        private void FrmComSupOrderListEvent_SendEvent(string[] strSUCODE, string[] strSUNAMEK, string[] strDRBUN, string[] strEFFECT, string[] strQTY, string[] strGBDIV, string[] strDOSNAME)
        {
            FarPoint.Win.Spread.FpSpread ssSpread = null;

            if (GstrOrderGubun == "1")
            {
                ssSpread = ssDrug;
            }
            else
            {
                ssSpread = ssDoubt;
            }

            for (int i = 0; i < strSUCODE.Length; i++)
            {
                ssSpread.ActiveSheet.RowCount = ssSpread.ActiveSheet.RowCount + 1;
                ssSpread.ActiveSheet.SetRowHeight(ssSpread.ActiveSheet.RowCount - 1, ComNum.SPDROWHT);

                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 0].Text = strSUCODE[i];
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 1].Text = strSUNAMEK[i];
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 2].Text = strDRBUN[i];
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 3].Text = strEFFECT[i];
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 4].Text = strQTY[i];
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 5].Text = strGBDIV[i];
                ssSpread.ActiveSheet.Cells[ssSpread.ActiveSheet.RowCount - 1, 6].Text = strDOSNAME[i];
            }

            frmComSupOrderListEvent.Dispose();
            frmComSupOrderListEvent = null;
        }

        private void FrmComSupOrderListEvent_rEventClosed()
        {
            frmComSupOrderListEvent.Dispose();
            frmComSupOrderListEvent = null;
        }

        private void btnDrugDel_Click(object sender, EventArgs e)
        {
            if (ComFunc.MsgBoxQ("선택한 약품을 삭제합니다.", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                ssDrug_Sheet1.RemoveRows(ssDrug_Sheet1.ActiveRowIndex, 1);
            }
        }

        private void ssDrug_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssDrug_Sheet1.Cells[0, 0, ssDrug_Sheet1.RowCount - 1, ssDrug_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssDrug_Sheet1.Cells[e.Row, 0, e.Row, ssDrug_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }

        private void ssDrug_EditModeOff(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            ssDrug_Sheet1.Cells[ssDrug_Sheet1.ActiveRowIndex, 0].Text = ssDrug_Sheet1.Cells[ssDrug_Sheet1.ActiveRowIndex, 0].Text.ToUpper().Trim();
            ssDrug_Sheet1.Cells[ssDrug_Sheet1.ActiveRowIndex, 1].Text = "";
            ssDrug_Sheet1.Cells[ssDrug_Sheet1.ActiveRowIndex, 2].Text = "";
            ssDrug_Sheet1.Cells[ssDrug_Sheet1.ActiveRowIndex, 3].Text = "";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.SUNAMEK, B.EFFECT, B.BUNCODE, C.CLASSNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN A, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW B, KOSMOS_PMPA.BAS_CLASS C";
                SQL = SQL + ComNum.VBLF + "     WHERE A.SUNEXT = B.SUNEXT";
                SQL = SQL + ComNum.VBLF + "         AND B.BUNCODE = C.CLASSCODE";
                SQL = SQL + ComNum.VBLF + "         AND A.SUNEXT = '" + ssDrug_Sheet1.Cells[ssDrug_Sheet1.ActiveRowIndex, 0].Text + "' ";

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
                    ssDrug_Sheet1.Cells[ssDrug_Sheet1.ActiveRowIndex, 1].Text = dt.Rows[0]["SUNAMEK"].ToString().Trim();
                    ssDrug_Sheet1.Cells[ssDrug_Sheet1.ActiveRowIndex, 2].Text = dt.Rows[0]["CLASSNAME"].ToString().Trim();
                    ssDrug_Sheet1.Cells[ssDrug_Sheet1.ActiveRowIndex, 3].Text = dt.Rows[0]["EFFECT"].ToString().Trim();
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

        private void btnDoubtAdd_Click(object sender, EventArgs e)
        {
            ssDoubt_Sheet1.RowCount = ssDoubt_Sheet1.RowCount + 1;
            ssDoubt_Sheet1.SetRowHeight(ssDoubt_Sheet1.RowCount - 1, ComNum.SPDROWHT);
        }

        private void btnOrder2_Click(object sender, EventArgs e)
        {
            GstrOrderGubun = "2";

            frmComSupOrderListEvent = new frmComSupOrderList(GstrPANO);
            frmComSupOrderListEvent.SendEvent += FrmComSupOrderListEvent_SendEvent;
            frmComSupOrderListEvent.rEventClosed += FrmComSupOrderListEvent_rEventClosed;
            frmComSupOrderListEvent.StartPosition = FormStartPosition.CenterParent;
            frmComSupOrderListEvent.ShowDialog();
        }

        private void btnDoubtDel_Click(object sender, EventArgs e)
        {
            if (ComFunc.MsgBoxQ("선택한 약품을 삭제합니다.", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                ssDoubt_Sheet1.RemoveRows(ssDoubt_Sheet1.ActiveRowIndex, 1);
            }
        }

        private void ssDoubt_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssDoubt_Sheet1.Cells[0, 0, ssDoubt_Sheet1.RowCount - 1, ssDoubt_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssDoubt_Sheet1.Cells[e.Row, 0, e.Row, ssDoubt_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }

        private void ssDoubt_EditModeOff(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            ssDoubt_Sheet1.Cells[ssDoubt_Sheet1.ActiveRowIndex, 0].Text = ssDoubt_Sheet1.Cells[ssDoubt_Sheet1.ActiveRowIndex, 0].Text.ToUpper().Trim();
            ssDoubt_Sheet1.Cells[ssDoubt_Sheet1.ActiveRowIndex, 1].Text = "";
            ssDoubt_Sheet1.Cells[ssDoubt_Sheet1.ActiveRowIndex, 2].Text = "";
            ssDoubt_Sheet1.Cells[ssDoubt_Sheet1.ActiveRowIndex, 3].Text = "";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.SUNAMEK, B.EFFECT, B.BUNCODE, C.CLASSNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN A, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW B, KOSMOS_PMPA.BAS_CLASS C";
                SQL = SQL + ComNum.VBLF + "     WHERE A.SUNEXT = B.SUNEXT";
                SQL = SQL + ComNum.VBLF + "         AND B.BUNCODE = C.CLASSCODE";
                SQL = SQL + ComNum.VBLF + "         AND A.SUNEXT = '" + ssDoubt_Sheet1.Cells[ssDoubt_Sheet1.ActiveRowIndex, 0].Text + "' ";

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
                    ssDoubt_Sheet1.Cells[ssDoubt_Sheet1.ActiveRowIndex, 1].Text = dt.Rows[0]["SUNAMEK"].ToString().Trim();
                    ssDoubt_Sheet1.Cells[ssDoubt_Sheet1.ActiveRowIndex, 2].Text = dt.Rows[0]["CLASSNAME"].ToString().Trim();
                    ssDoubt_Sheet1.Cells[ssDoubt_Sheet1.ActiveRowIndex, 3].Text = dt.Rows[0]["EFFECT"].ToString().Trim();
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

        private void ssChk2_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != 3 && (e.Row < 1 && e.Row > 35)) { return; }

            if (Convert.ToBoolean(ssChk2_Sheet1.Cells[e.Row, 3].Value) == true)
            {
                //ssChk2_Sheet1.Cells[38, 3].Text = (VB.Val(ssChk2_Sheet1.Cells[38, 3].Text) + (VB.Val(ssChk2_Sheet1.Cells[e.Row, 3].Tag.ToString()) / 2)).ToString();
                ssChk2_Sheet1.Cells[38, 3].Text = (VB.Val(ssChk2_Sheet1.Cells[38, 3].Text) + VB.Val(ssChk2_Sheet1.Cells[e.Row, 3].Tag.ToString())).ToString();
            }
            else
            {
                //ssChk2_Sheet1.Cells[38, 3].Text = (VB.Val(ssChk2_Sheet1.Cells[38, 3].Text) - (VB.Val(ssChk2_Sheet1.Cells[e.Row, 3].Tag.ToString()) / 2)).ToString();
                ssChk2_Sheet1.Cells[38, 3].Text = (VB.Val(ssChk2_Sheet1.Cells[38, 3].Text) - VB.Val(ssChk2_Sheet1.Cells[e.Row, 3].Tag.ToString())).ToString();
            }
            
            ssChk2_Sheet1.Cells[37, 2].Value = false;
            ssChk2_Sheet1.Cells[38, 2].Value = false;
            ssChk2_Sheet1.Cells[39, 2].Value = false;
            ssChk2_Sheet1.Cells[40, 2].Value = false;

            if (VB.Val(ssChk2_Sheet1.Cells[38, 3].Text) >= 12)
            {
                ssChk2_Sheet1.Cells[37, 2].Value = true;
            }
            else if (VB.Val(ssChk2_Sheet1.Cells[38, 3].Text) >= 6)
            {
                ssChk2_Sheet1.Cells[38, 2].Value = true;
            }
            else if (VB.Val(ssChk2_Sheet1.Cells[38, 3].Text) >= 2)
            {
                ssChk2_Sheet1.Cells[39, 2].Value = true;
            }
            else
            {
                ssChk2_Sheet1.Cells[40, 2].Value = true;
            }
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
