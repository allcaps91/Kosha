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
    /// File Name       : frmComSupADR1.cs
    /// Description     : 약물이상반응(ADR) 발생 보고서(일반서식)
    /// Author          : 이정현 
    /// Create Date     : 2018-01-15
    /// <history> 
    /// 약물이상반응(ADR) 발생 보고서(일반서식)
    /// </history>
    /// <seealso>
    /// PSMH\drug\dradr\FrmADR1.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\dradr\dradr.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupADR1 : Form
    {
        private frmComSupOrderList frmComSupOrderListEvent = null;

        private string GstrROWID = "";
        private string GstrPANO = "";
        private string GstrDeptCode = "";
        private string GstrIO = "";
        private string GstrSEQNO = "";
        private string GstrOPTION = "";

        public frmComSupADR1()
        {
            InitializeComponent();
        }

        public frmComSupADR1(string strPANO, string strDeptCode, string strIO, string strSEQNO, string strOPTION)
        {
            InitializeComponent();

            GstrPANO = strPANO;
            GstrDeptCode = strDeptCode;
            GstrIO = strIO;
            GstrSEQNO = strSEQNO;
            GstrOPTION = strOPTION;
        }

        private void frmComSupADR1_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            if (GstrSEQNO == "" && GstrPANO == "")
            {
                GstrPANO = ComFunc.LPAD(VB.InputBox("환자 정보가 없습니다. 환자 등록번호를 입력하여 주십시요", "등록번호입력"), 8, "0");
            }

            panList.Visible = false;

            SetAllControlClear(this, false);

            GetJoyoung();

            ssDrug_Sheet1.RowCount = 0;
            ssJoYung_Sheet1.RowCount = 0;
            ssList_Sheet1.RowCount = 0;

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
                
                if (clsType.User.BuseCode == "044101" || VB.Left(clsType.User.BuseCode, 2) == "01") { }
                else
                {
                    btnSave.Enabled = false;
                    btnDelete.Enabled = false;

                    if (GetWRTIESABUN(GstrSEQNO) != "")
                    {
                        btnSave.Enabled = true;
                        btnDelete.Enabled = true;
                    }
                }
            }
            else
            {
                setPatient(GstrPANO, GstrDeptCode, GstrOPTION, GstrIO);
            }

            viewSS1(GstrSEQNO);
            viewSS3(GstrSEQNO);
            panList.Visible = false;
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

        private void GetJoyoung()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string[] strJoYoung = new string[0];

            //FarPoint.Win.Spread.CellType.ComboBoxCellType cboType = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            //cboType.AllowEditorVerticalAlign = true;
            //cboType.ButtonAlign = FarPoint.Win.ButtonAlign.Right;
            
            //try
            //{
            //    SQL = "";
            //    SQL = "SELECT";
            //    SQL = SQL + ComNum.VBLF + "    JEPCODE";
            //    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_USERJEP";
            //    SQL = SQL + ComNum.VBLF + "    WHERE DEPTCODE = '055112' ";
            //    SQL = SQL + ComNum.VBLF + "        AND STOCK > 0";
            //    SQL = SQL + ComNum.VBLF + "ORDER BY JEPCODE";

            //    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            //    if (SqlErr != "")
            //    {
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //        return;
            //    }
            //    if (dt.Rows.Count > 0)
            //    {
            //        strJoYoung = new string[dt.Rows.Count];

            //        for (i = 0; i < dt.Rows.Count; i++)
            //        {
            //            strJoYoung[i] = dt.Rows[i]["JEPCODE"].ToString().Trim();
            //        }

            //        //cboType.Items = strJoYoung;
            //        ssJoYung_Sheet1.Columns.Get(0).CellType = cboType;
            //    }

            //    dt.Dispose();
            //    dt = null;
            //}
            //catch (Exception ex)
            //{
            //    if (dt != null)
            //    {
            //        dt.Dispose();
            //        dt = null;
            //    }

            //    ComFunc.MsgBox(ex.Message);
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //}
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
                SQL = "SELECT ROWID FROM " + ComNum.DB_ERP + "DRUG_ADR1";
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
                SQL = "SELECT TEMP FROM " + ComNum.DB_ERP + "DRUG_ADR1";
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

        private string GetWRTIESABUN(string strSEQNO)
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
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     WRITESABUN ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR1 ";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;
                SQL = SQL + ComNum.VBLF + "         AND WRITESABUN = " + clsType.User.Sabun;

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
                    rtnVal = dt.Rows[0]["WRITESABUN"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TEMP FROM KOSMOS_ADM.DRUG_ADR2 ";
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
                    if (dt.Rows[0]["TEMP"].ToString().Trim() == "")
                    {
                        rtnVal = "";
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TEMP FROM KOSMOS_ADM.DRUG_ADR3 ";
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
                    if (dt.Rows[0]["TEMP"].ToString().Trim() == "")
                    {
                        rtnVal = "";
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
                SQL = "SELECT * FROM " + ComNum.DB_ERP + "DRUG_ADR1";
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

            for (i = 0; i < dt.Columns.Count; i++)
            {
                foreach (Control control in controls)
                {
                    if (control.Name.Replace("rdo", "").ToUpper().Trim() == dt.Columns[i].ToString())
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
                }
            }                      

        }

        private void setPatient(string strPTNO, string strDEPTCODE, string strOPTION, string strIO)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";

                switch (strIO)
                {
                    case "O":
                        SQL = " SELECT PANO PTNO, SNAME, A.SEX AGESEX, '' ROOMCODE, A.DEPTCODE, C.ILLNAMEK DIAGNAME, '외래' patient_bun, '' IPDNO, D.DRNAME";
                        SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER A, KOSMOS_OCS.OCS_OILLS B, KOSMOS_PMPA.BAS_ILLS C, KOSMOS_PMPA.BAS_DOCTOR D";
                        SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + strPTNO + "'";
                        SQL = SQL + ComNum.VBLF + "    AND A.BDATE = TO_DATE('" + strOPTION + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "    AND A.DEPTCODE = '" + strDEPTCODE + "'";
                        SQL = SQL + ComNum.VBLF + "    AND A.BDATE = B.BDATE(+)";
                        SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PTNO(+)";
                        SQL = SQL + ComNum.VBLF + "    AND B.ILLCODE = C.ILLCODE(+)";
                        SQL = SQL + ComNum.VBLF + "    AND A.DRCODE = D.DRCODE(+)";
                        SQL = SQL + ComNum.VBLF + " ORDER BY B.ILLCODE";
                        break;
                    case "I":
                        SQL = " SELECT PANO PTNO, SNAME, A.SEX AGESEX, ROOMCODE, A.DEPTCODE, C.ILLNAMEK DIAGNAME, '입원' patient_bun, A.IPDNO, D.DRNAME";
                        SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER A, KOSMOS_OCS.OCS_IILLS B, KOSMOS_PMPA.BAS_ILLS C, KOSMOS_PMPA.BAS_DOCTOR D";
                        SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + strPTNO + "'";
                        SQL = SQL + ComNum.VBLF + "    AND A.IPDNO = " + strOPTION;
                        SQL = SQL + ComNum.VBLF + "    AND A.IPDNO = B.IPDNO(+)";
                        SQL = SQL + ComNum.VBLF + "    AND B.ILLCODE = C.ILLCODE(+)";
                        SQL = SQL + ComNum.VBLF + "    AND A.DRCODE = D.DRCODE(+)";
                        SQL = SQL + ComNum.VBLF + " ORDER BY B.ILLCODE";
                        break;
                    default:
                        SQL = " SELECT PANO PTNO, SNAME, SEX AGESEX, '' ROOMCODE, DEPTCODE, '' DIAGNAME, '' IPDNO, '' DRCODE";
                        SQL = SQL + ComNum.VBLF + " From KOSMOS_PMPA.BAS_PATIENT";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPTNO + "' ";
                        break;
                }

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

                txtagesex.Text = clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, strPTNO) + "/" + txtagesex.Text;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     B.NAME || ' => ' || A.REMARK AS Allergy";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_ALLERGY_MST A, " + ComNum.DB_PMPA + "BAS_BCODE B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.CODE = B.CODE";
                SQL = SQL + ComNum.VBLF + "         AND B.GUBUN ='환자정보_알러지종류'";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = '" + strPTNO + "'";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.CODE ASC";

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
                    txtAllergy.Text = dt.Rows[0]["ALLERGY"].ToString().Trim();
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

        private void viewSS1(string strSEQNO)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssDrug_Sheet1.RowCount = 0;

            if (strSEQNO == "") { return; }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUCODE, SUNAMEK, DRBUN, EFFECT, QTY, GBDIV, DOSNAME, CHECK1, CHECK2, CHECK3, ROWID";
                SQL = SQL + ComNum.VBLF + "From " + ComNum.DB_ERP + "DRUG_ADR1_ORDER";
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

        private void viewSS3(string strSEQNO)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssJoYung_Sheet1.RowCount = 0;

            if (strSEQNO == "") { return; }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUCODE, SUNAMEK, INSPEED, QTY, SERIALNO, CHECK1, CHECK2, CHECK3, ROWID";
                SQL = SQL + ComNum.VBLF + "From " + ComNum.DB_ERP + "DRUG_ADR1_ORDER_JO";
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
                    ssJoYung_Sheet1.RowCount = dt.Rows.Count;
                    ssJoYung_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssJoYung_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssJoYung_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                        ssJoYung_Sheet1.Cells[i, 2].Text = dt.Rows[i]["INSPEED"].ToString().Trim();
                        ssJoYung_Sheet1.Cells[i, 3].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        ssJoYung_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SERIALNO"].ToString().Trim();
                        ssJoYung_Sheet1.Cells[i, 5].Value = dt.Rows[i]["CHECK1"].ToString().Trim() == "1" ? true : false;
                        ssJoYung_Sheet1.Cells[i, 6].Value = dt.Rows[i]["CHECK2"].ToString().Trim() == "1" ? true : false;
                        ssJoYung_Sheet1.Cells[i, 7].Value = dt.Rows[i]["CHECK3"].ToString().Trim() == "1" ? true : false;
                        ssJoYung_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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
                if (GstrSEQNO != "")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     WNAME";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR2";
                    SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + GstrSEQNO;

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("이미 접수되어 진행중인 보고서입니다."
                                        + ComNum.VBLF + "수정이 불가능합니다.");
                        Cursor.Current = Cursors.Default;
                        dt.Dispose();
                        dt = null;
                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;
                }
                
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
                        SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR1_HISTORY";
                        SQL = SQL + ComNum.VBLF + "     SELECT * FROM " + ComNum.DB_ERP + "DRUG_ADR1";
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
                        SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR1";
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

                if (txtwSabun.Text.Trim()!="")
                {
                    nSabun =  Convert.ToInt32(txtwSabun.Text.Trim());
                }
                

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "     (SEQNO, PTNO, SNAME, AGESEX, PATIENT_BUN, ROOMCODE, DEPTCODE, DIAGNAME, ALLERGY, ";
                SQL = SQL + ComNum.VBLF + "     IMSIN1, IMSIN2, IMSIN3, DRUNK1, DRUNK2, DRUNK3, SMOKE1, SMOKE2, SMOKE3, ";
                SQL = SQL + ComNum.VBLF + "     BDATE, BTIME1, BTIME1TIME, BTIME1SECOND, BTIME2, BTIME2INSTANCE, BTIME2SECOND, BTIME2TIME, BTIME2DAY, ";
                SQL = SQL + ComNum.VBLF + "     RACT_A1, RACT_A2, RACT_A3, RACT_A4, RACT_A5, RACT_A6, ";
                SQL = SQL + ComNum.VBLF + "     RACT_B1, RACT_B2, RACT_B3, RACT_B4, RACT_B5, RACT_B6, RACT_B7, RACT_B8, RACT_B9, ";
                SQL = SQL + ComNum.VBLF + "     RACT_C1, RACT_C2, RACT_C3, RACT_C4, RACT_C5, RACT_C6, RACT_C7, RACT_C8, ";
                SQL = SQL + ComNum.VBLF + "     RACT_D1, RACT_D2, RACT_D3, RACT_D4, RACT_D5, RACT_D6, RACT_D7, ";
                SQL = SQL + ComNum.VBLF + "     RACT_E1, RACT_E2, RACT_E3, RACT_E4, RACT_E5, RACT_E6, RACT_E7, ";
                SQL = SQL + ComNum.VBLF + "     RACT_F1, RACT_F2, RACT_G1, RACT_G2, RACT_G3, ";
                SQL = SQL + ComNum.VBLF + "     RACT_H1, RACT_H2, RACT_H3, RACT_H4, RACT_I1, RACT_I2, RACT_I3, RACT_I4, ";
                SQL = SQL + ComNum.VBLF + "     RACT_J1, RACT_J2, RACT_J3, RACT_J4, RACT_J5, RACT_J6, RACT_J7, RACT_J8, ";
                SQL = SQL + ComNum.VBLF + "     RACT_J9, RACT_J10, RACT_J11, RACT_J12, RACT_J13, RACT_J14, RACT_J15, RACT_J16, ";
                SQL = SQL + ComNum.VBLF + "     RACT_K1, RACT_K2, RACT_K3, RACT_K4, RACT_K5, RACT_K6, RACT_K7, RACT_K8, ";
                SQL = SQL + ComNum.VBLF + "     RACT_L1, RACT_L2, RACT_L3, CLASS1, CLASS2, CLASS3, CLASS4, ";
                SQL = SQL + ComNum.VBLF + "     RACTMEMO, RECEPT1, RECEPT2, RECEPT3, RECEPT3_1, RECEPT3_2, RECEPT3_3, ";
                SQL = SQL + ComNum.VBLF + "     EMER1, EMER2, EMER3, EMER4, EMER5, EMER6, EMER7, EMER8, EMER9, EMER9MEMO, ";
                SQL = SQL + ComNum.VBLF + "     RESULTDATE, RESULTTIME, RECOVER1, RECOVER2, RECOVER3, RECOVER4, RECOVER5, ";
                SQL = SQL + ComNum.VBLF + "     RETUYAK1, RETUYAK2, RETUYAK3, ";
                SQL = SQL + ComNum.VBLF + "     PROGRESSMEMO, WRITESABUN, WRITEDATE, WDATE,WSabun, WNAME, WBUSE, IPDNO, ";
                SQL = SQL + ComNum.VBLF + "     RECOVER6, RECOVER6MEMO, RETUYAK4, RESULTSEC, ";
                SQL = SQL + ComNum.VBLF + "     JO1, JO2, JO3, JO4, JO5, JO6, ";
                SQL = SQL + ComNum.VBLF + "     VITALUP, VITALBP, VITALBIT, VITALBREATH, VITALHEAT, VITALSPO2, ";
                SQL = SQL + ComNum.VBLF + "     GUBUN_JO, EMER10, EMER11, DRNAME, TEMP)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         " + GstrSEQNO + ", ";
                SQL = SQL + ComNum.VBLF + "         '" + txtptno.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtsname.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtagesex.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + cbopatient_bun.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtroomcode.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtdeptcode.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtdiagname.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtAllergy.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoimsin1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoimsin2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoimsin3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdodrunk1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdodrunk2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdodrunk3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdosmoke1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdosmoke2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdosmoke3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpbdate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdobtime1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtbtime1time.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtbtime1second.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdobtime2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkbtime2instance.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtbtime2second.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtbtime2time.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtbtime2day.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_a1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_a2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_a3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_a4.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_a5.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_a6.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_b1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_b2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_b3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_b4.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_b5.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_b6.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_b7.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_b8.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_b9.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_c1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_c2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_c3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_c4.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_c5.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_c6.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_c7.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_c8.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_d1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_d2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_d3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_d4.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_d5.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_d6.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_d7.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_e1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_e2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_e3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_e4.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_e5.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_e6.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_e7.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_f1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_f2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_g1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_g2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_g3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_h1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_h2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_h3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_h4.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_i1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_i2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_i3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_i4.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j4.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j5.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j6.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j7.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j8.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j9.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j10.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j11.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j12.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j13.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j14.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j15.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_j16.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_k1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_k2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_k3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_k4.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_k5.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_k6.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_k7.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_k8.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_l1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_l2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkract_l3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoclass1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoclass2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoclass3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoclass4.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtractMemo.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorecept1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorecept2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorecept3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorecept3_1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorecept3_2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorecept3_3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkemer1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkemer2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkemer3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkemer4.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkemer5.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkemer6.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkemer7.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkemer8.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkemer9.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtemer9memo.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtresultdate.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtresulttime.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorecover1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorecover2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorecover3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorecover4.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorecover5.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoretuyak1.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoretuyak2.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoretuyak3.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtprogressMemo.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + clsType.User.Sabun + "', ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE,";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpwdate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "         " + nSabun + ", "; //2019-03-06
                SQL = SQL + ComNum.VBLF + "         '" + txtwname.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtwbuse.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtipdno.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdorecover6.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtrecover6memo.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (rdoretuyak4.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtresultsec.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtjo1.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtjo2.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtjo3.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtjo4.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtjo5.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtjo6.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtvitalup.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtvitalbp.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtvitalbit.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtvitalbreath.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtvitalheat.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtvitalspo2.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkgubun_jo.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkemer10.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + (chkemer11.Checked == true ? "1" : "0") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtdrname.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         NULL";
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

                if (chkgubun_jo.Checked == true)
                {
                    #region saveSS3

                    if (GstrSEQNO != "")
                    {
                        SQL = "";
                        SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR1_ORDER_JO ";
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
                    }

                    for (i = 0; i < ssJoYung_Sheet1.RowCount; i++)
                    {
                        strSUCODE = ssJoYung_Sheet1.Cells[i, 0].Text.Trim();
                        strSUNAMEK = ssJoYung_Sheet1.Cells[i, 1].Text.Trim();
                        strINSPEED = ssJoYung_Sheet1.Cells[i, 2].Text.Trim();
                        strQTY = ssJoYung_Sheet1.Cells[i, 3].Text.Trim();
                        strSERIALNO = ssJoYung_Sheet1.Cells[i, 4].Text.Trim();
                        strCheck1 = Convert.ToBoolean(ssJoYung_Sheet1.Cells[i, 5].Value) == true ? "1" : "0";
                        strCheck2 = Convert.ToBoolean(ssJoYung_Sheet1.Cells[i, 6].Value) == true ? "1" : "0";
                        strCheck3 = Convert.ToBoolean(ssJoYung_Sheet1.Cells[i, 7].Value) == true ? "1" : "0";

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR1_ORDER_JO";
                        SQL = SQL + ComNum.VBLF + "     (SUCODE, SUNAMEK, INSPEED, QTY, SERIALNO, CHECK1, CHECK2, CHECK3, SEQNO)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         '" + strSUCODE + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strSUNAMEK + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strINSPEED + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strQTY + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strSERIALNO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strCheck1 + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strCheck2 + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strCheck3 + "', ";
                        SQL = SQL + ComNum.VBLF + "         " + GstrSEQNO;
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
                    }

                    #endregion
                }
                else
                {
                    #region saveSS1

                    if (GstrSEQNO != "")
                    {
                        SQL = "";
                        SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR1_ORDER ";
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
                    }

                    for (i = 0; i < ssDrug_Sheet1.RowCount; i++)
                    {
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
                        SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR1_ORDER";
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
                        SQL = SQL + ComNum.VBLF + "         " + GstrSEQNO;
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
                    }

                    #endregion
                }

                SQL = "";
                SQL = "UPDATE " + ComNum.DB_ERP + "DRUG_ADR1";
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
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR1_HISTORY";
                SQL = SQL + ComNum.VBLF + "     SELECT * FROM " + ComNum.DB_ERP + "DRUG_ADR1";
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
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR1";
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
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            if (chkgubun_jo.Checked == false)
            {
                ComFunc.MsgBox("조영제 부작용일 경우에만 인쇄가 가능합니다.");
                return;
            }
            
            GetPrint();

            ssPRT_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssPRT_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssPRT_Sheet1.PrintInfo.Margin.Top = 20;
            ssPRT_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssPRT_Sheet1.PrintInfo.Margin.Header = 10;
            ssPRT_Sheet1.PrintInfo.ShowColor = false;
            ssPRT_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPRT_Sheet1.PrintInfo.ShowBorder = false;
            ssPRT_Sheet1.PrintInfo.ShowGrid = false;
            ssPRT_Sheet1.PrintInfo.ShowShadows = false;
            ssPRT_Sheet1.PrintInfo.UseMax = true;
            ssPRT_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPRT_Sheet1.PrintInfo.UseSmartPrint = false;
            ssPRT_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssPRT_Sheet1.PrintInfo.Preview = false;
            ssPRT.PrintSheet(0);
        }
        
        private void GetPrint()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            ssPRT_Sheet1.Cells[3, 1].Text = "";
            ssPRT_Sheet1.Cells[4, 1].Text = "";
            ssPRT_Sheet1.Cells[5, 1].Text = "";
            ssPRT_Sheet1.Cells[6, 1].Text = "";
            ssPRT_Sheet1.Cells[7, 1].Text = "";
            ssPRT_Sheet1.Cells[8, 1].Text = "";
            ssPRT_Sheet1.Cells[9, 1].Text = "";
            ssPRT_Sheet1.Cells[10, 1].Text = "";

            ssPRT_Sheet1.Cells[15, 1].Text = "";
            ssPRT_Sheet1.Cells[16, 1].Text = "";
            ssPRT_Sheet1.Cells[17, 1].Text = "";
            ssPRT_Sheet1.Cells[18, 1].Text = "";
            ssPRT_Sheet1.Cells[19, 1].Text = "";
            ssPRT_Sheet1.Cells[20, 1].Text = "";
            ssPRT_Sheet1.Cells[21, 1].Text = "";
            ssPRT_Sheet1.Cells[22, 1].Text = "";

            ssPRT_Sheet1.Cells[3, 3].Text = "";
            ssPRT_Sheet1.Cells[4, 3].Text = "";
            ssPRT_Sheet1.Cells[5, 3].Text = "";
            ssPRT_Sheet1.Cells[6, 3].Text = "";
            ssPRT_Sheet1.Cells[7, 3].Text = "";

            ssPRT_Sheet1.Cells[9, 2].Text = "";

            ssPRT_Sheet1.Cells[15, 3].Text = "";
            ssPRT_Sheet1.Cells[16, 3].Text = "";
            ssPRT_Sheet1.Cells[17, 3].Text = "";
            ssPRT_Sheet1.Cells[18, 3].Text = "";
            ssPRT_Sheet1.Cells[19, 3].Text = "";

            ssPRT_Sheet1.Cells[21, 2].Text = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PTNO, SNAME, AGESEX, PATIENT_BUN, DEPTCODE, DRNAME, JO4, JO5, JO6, WNAME, JO3,";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_A1, '1', '발열, ', '') || DECODE(RACT_A2, '1', '식욕감소, ', '') || DECODE(RACT_A3, '1', '전신부종, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_A4, '1', '전신쇠약, ', '') || DECODE(RACT_A5, '1', '체중감소, ', '') || DECODE(RACT_A6, '1', '체중증가, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_B1, '1', '가려움증, ', '') || DECODE(RACT_B2, '1', '가려운 발진, ', '') || DECODE(RACT_B3, '1', '농포성 발진, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_B4, '1', '두드러기, ', '') || DECODE(RACT_B5, '1', '여드름성 발진, ', '') || DECODE(RACT_B6, '1', '피부작리, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_B7, '1', '피부변색, ', '') || DECODE(RACT_B8, '1', '혈관부종, ', '') || DECODE(RACT_B9, '1', '탈모, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_C1, '1', '구강칸디다증, ', '') || DECODE(RACT_C2, '1', '구강건조증, ', '') || DECODE(RACT_C3, '1', '귀울림, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_C4, '1', '급성청각이상, ', '') || DECODE(RACT_C5, '1', '미각이상, ', '') || DECODE(RACT_C6, '1', '시각장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_C7, '1', '안압상승, ', '') || DECODE(RACT_C8, '1', '음성변화, ', '') || DECODE(RACT_D1, '1', '가슴불편함, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_D2, '1', '부정맥, ', '') || DECODE(RACT_D3, '1', '빈맥, ', '') || DECODE(RACT_D4, '1', '실신, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_D5, '1', '심부전, ', '') || DECODE(RACT_D6, '1', '저혈압, ', '') || DECODE(RACT_D7, '1', '고혈압, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_E1, '1', '오심/구토, ', '') || DECODE(RACT_E2, '1', '변비, ', '') || DECODE(RACT_E3, '1', '복통, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_E4, '1', '설사, ', '') || DECODE(RACT_E5, '1', '소화불량, ', '') || DECODE(RACT_E6, '1', '위장관통증, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_E7, '1', '위출혈, ', '') || DECODE(RACT_F1, '1', '빌리루빈증가, ', '') || DECODE(RACT_F2, '1', 'AST/ALT 증가, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_G1, '1', '기침, ', '') || DECODE(RACT_G2, '1', '호흡곤란, ', '') || DECODE(RACT_G3, '1', '폐부종, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_H1, '1', '백혈구감소증, ', '') || DECODE(RACT_H2, '1', '빈혈, ', '') || DECODE(RACT_H3, '1', '응고장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_H4, '1', '혈소판감소증, ', '') || DECODE(RACT_I1, '1', '단백뇨, ', '') || DECODE(RACT_I2, '1', '신기능장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_I3, '1', '혈뇨, ', '') || DECODE(RACT_I4, '1', '혈중 Creatinine 증가, ', '') || DECODE(RACT_J1, '1', '기억력장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_J2, '1', '두통, ', '') || DECODE(RACT_J3, '1', '보행곤란, ', '') || DECODE(RACT_J4, '1', '사지떨림, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_J5, '1', '수면장애, ', '') || DECODE(RACT_J6, '1', '어지러움, ', '') || DECODE(RACT_J7, '1', '언어장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_J8, '1', '의식저하, ', '') || DECODE(RACT_J9, '1', '운동이상증, ', '') || DECODE(RACT_J10, '1', '졸림, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_J11, '1', '피부저림, ', '') || DECODE(RACT_J12, '1', '불안, ', '') || DECODE(RACT_J13, '1', '섬망, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_J14, '1', '신경과민, ', '') || DECODE(RACT_J15, '1', '우울, ', '') || DECODE(RACT_J16, '1', '과행동, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_K1, '1', '고혈당증, ', '') || DECODE(RACT_K2, '1', '저혈당증, ', '') || DECODE(RACT_K3, '1', '배뇨장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_K4, '1', '성기능장애, ', '') || DECODE(RACT_K5, '1', '성욕감소, ', '') || DECODE(RACT_K6, '1', '여성형유방, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_K7, '1', '월경불순, ', '') || DECODE(RACT_L1, '1', '관절통, ', '') || DECODE(RACT_L2, '1', '골다공증, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RACT_L3, '1', '근육통, ', '') AS RACT,";
                SQL = SQL + ComNum.VBLF + "     DECODE(RECEPT1, '1', '관찰(투약 종료 또는 투약 유지)', '') || DECODE(RECEPT2, '1', '투약중지', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RECEPT3_1, '1', '투약변경(용량 변경)', '') || DECODE(RECEPT3_2, '1', '투약변경(용법 변경)', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RECEPT3_3, '1', '투약변경(약물 변경)', '') AS RACT2,";
                SQL = SQL + ComNum.VBLF + "     DECODE(EMER1, '1', '없음','') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(EMER2, '1', '항히스타민제(IV)','') || DECODE(EMER3, '1', '항히스타민제(PO)','') ||  DECODE(EMER10, '1', '항히스타민제(IM)','') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(EMER4, '1', '스테로이드제(IV)','') || DECODE(EMER5, '1', '스테로이드제(PO)','') ||  DECODE(EMER11, '1', '스테로이드제(IM)','') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(EMER6, '1', '에피네프린','') || DECODE(EMER7, '1', 'Hydration','') || DECODE(EMER8, '1', '기관지 확장제','') || DECODE(EMER9MEMO, '1', '기타' || EMER9MEMO,'') AS RACT3,";
                SQL = SQL + ComNum.VBLF + "     VITALUP , VITALBP, VITALBIT, VITALBREATH, VITALHEAT, VITALSPO2";
                SQL = SQL + ComNum.VBLF + "From " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "     Where SEQNO = " + GstrSEQNO;

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
                    ssPRT_Sheet1.Cells[3, 1].Text = dt.Rows[0]["PTNO"].ToString().Trim();
                    ssPRT_Sheet1.Cells[4, 1].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssPRT_Sheet1.Cells[5, 1].Text = dt.Rows[0]["AGESEX"].ToString().Trim();
                    ssPRT_Sheet1.Cells[6, 1].Text = dt.Rows[0]["PATIENT_BUN"].ToString().Trim();
                    ssPRT_Sheet1.Cells[7, 1].Text = dt.Rows[0]["JO5"].ToString().Trim();
                    ssPRT_Sheet1.Cells[8, 1].Text = dt.Rows[0]["JO4"].ToString().Trim();
                    ssPRT_Sheet1.Cells[9, 1].Text = dt.Rows[0]["JO6"].ToString().Trim();
                    ssPRT_Sheet1.Cells[10, 1].Text = dt.Rows[0]["WNAME"].ToString().Trim();

                    ssPRT_Sheet1.Cells[15, 1].Text = dt.Rows[0]["PTNO"].ToString().Trim();
                    ssPRT_Sheet1.Cells[16, 1].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssPRT_Sheet1.Cells[17, 1].Text = dt.Rows[0]["AGESEX"].ToString().Trim();
                    ssPRT_Sheet1.Cells[18, 1].Text = dt.Rows[0]["PATIENT_BUN"].ToString().Trim();
                    ssPRT_Sheet1.Cells[19, 1].Text = dt.Rows[0]["JO5"].ToString().Trim();
                    ssPRT_Sheet1.Cells[20, 1].Text = dt.Rows[0]["JO4"].ToString().Trim();
                    ssPRT_Sheet1.Cells[21, 1].Text = dt.Rows[0]["JO6"].ToString().Trim();
                    ssPRT_Sheet1.Cells[22, 1].Text = dt.Rows[0]["WNAME"].ToString().Trim();

                    ssPRT_Sheet1.Cells[3, 3].Text = dt.Rows[0]["JO3"].ToString().Trim();
                    ssPRT_Sheet1.Cells[6, 3].Text = dt.Rows[0]["RACT"].ToString().Trim();
                    ssPRT_Sheet1.Cells[7, 3].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + " / " + dt.Rows[0]["DRNAME"].ToString().Trim();
                    
                    if (dt.Rows[0]["RACT2"].ToString().Trim() != "" || dt.Rows[0]["RACT3"].ToString().Trim() != "")
                    {
                        ssPRT_Sheet1.Cells[9, 2].Text = " ▶ 조치내용 : " + dt.Rows[0]["RACT2"].ToString().Trim() + ComNum.VBLF + " ▶응급처치 : " + dt.Rows[0]["RACT3"].ToString().Trim();
                    }

                    ssPRT_Sheet1.Cells[15, 3].Text = dt.Rows[0]["JO3"].ToString().Trim();
                    ssPRT_Sheet1.Cells[18, 3].Text = dt.Rows[0]["RACT"].ToString().Trim();
                    ssPRT_Sheet1.Cells[19, 3].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + " / " + dt.Rows[0]["DRNAME"].ToString().Trim();

                    if (dt.Rows[0]["RACT2"].ToString().Trim() != "" || dt.Rows[0]["RACT3"].ToString().Trim() != "")
                    {
                        ssPRT_Sheet1.Cells[21, 2].Text = " ▶ 조치내용 : " + dt.Rows[0]["RACT2"].ToString().Trim() + ComNum.VBLF + " ▶응급처치 : " + dt.Rows[0]["RACT3"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUNAMEK, INSPEED, QTY, SERIALNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR1_ORDER_JO";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + GstrSEQNO;

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
                    ssPRT_Sheet1.Cells[4, 3].Text = dt.Rows[0]["SUNAMEK"].ToString().Trim() + "  ▶ 제번 : " + dt.Rows[0]["SERIALNO"].ToString().Trim();
                    ssPRT_Sheet1.Cells[5, 3].Text = "주입량(ml) : " + dt.Rows[0]["QTY"].ToString().Trim() + "  주입속도(ml/sec) : " + dt.Rows[0]["INSPEED"].ToString().Trim();

                    ssPRT_Sheet1.Cells[16, 3].Text = dt.Rows[0]["SUNAMEK"].ToString().Trim() + "  ▶ 제번 : " + dt.Rows[0]["SERIALNO"].ToString().Trim();
                    ssPRT_Sheet1.Cells[17, 3].Text = "주입량(ml) : " + dt.Rows[0]["QTY"].ToString().Trim() + "  주입속도(ml/sec) : " + dt.Rows[0]["INSPEED"].ToString().Trim();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkGubunJo_CheckedChanged(object sender, EventArgs e)
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
                if (chkgubun_jo.Checked == true)
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(BDATE, 'YYYY-MM-DD') AS BDATE, DEPTCODE, DRCODE, B.SUNAMEK, REMARK, DECODE(DEPTCODE,'ER','E','I') AS GUBUN";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER A, " + ComNum.DB_PMPA + "BAS_SUN B";
                    SQL = SQL + ComNum.VBLF + "     WHERE BUN = '72'";
                    SQL = SQL + ComNum.VBLF + "         AND BDATE >= TRUNC(SYSDATE-30)";
                    SQL = SQL + ComNum.VBLF + "         AND PTNO = '" + txtptno.Text.Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT";
                    SQL = SQL + ComNum.VBLF + "Union All";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(BDATE, 'YYYY-MM-DD') AS BDATE, DEPTCODE, DRCODE, B.SUNAMEK, REMARK, 'O' AS GUBUN";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER A, " + ComNum.DB_PMPA + "BAS_SUN B";
                    SQL = SQL + ComNum.VBLF + "     WHERE BUN = '72'";
                    SQL = SQL + ComNum.VBLF + "         AND BDATE >= TRUNC(SYSDATE-30)";
                    SQL = SQL + ComNum.VBLF + "         AND PTNO = '" + txtptno.Text.Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT";
                    SQL = SQL + ComNum.VBLF + "ORDER BY BDATE DESC, SUNAMEK";

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
                            ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 2].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                            ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    pan2false.Visible = false;
                    pan2ture.Visible = true;
                    panVital.Visible = true;
                    panList.Visible = true;
                }
                else
                {
                    panList.Visible = false;
                    pan2ture.Visible = false;
                    panVital.Visible = false;
                    pan2false.Visible = true;
                    txtjo4.Text = "";
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

        private void btnDrugAdd_Click(object sender, EventArgs e)
        {
            ssDrug_Sheet1.RowCount = ssDrug_Sheet1.RowCount + 1;
            ssDrug_Sheet1.SetRowHeight(ssDrug_Sheet1.RowCount - 1, ComNum.SPDROWHT);
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            frmComSupOrderListEvent = new frmComSupOrderList(GstrPANO);
            frmComSupOrderListEvent.SendEvent += FrmComSupOrderListEvent_SendEvent;
            frmComSupOrderListEvent.rEventClosed += FrmComSupOrderListEvent_rEventClosed;
            frmComSupOrderListEvent.StartPosition = FormStartPosition.CenterParent;
            frmComSupOrderListEvent.ShowDialog();
        }

        private void FrmComSupOrderListEvent_SendEvent(string[] strSUCODE, string[] strSUNAMEK, string[] strDRBUN, string[] strEFFECT, string[] strQTY, string[] strGBDIV, string[] strDOSNAME)
        {
            for (int i = 0; i < strSUCODE.Length; i++)
            {
                ssDrug_Sheet1.RowCount = ssDrug_Sheet1.RowCount + 1;
                ssDrug_Sheet1.SetRowHeight(ssDrug_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                ssDrug_Sheet1.Cells[ssDrug_Sheet1.RowCount - 1, 0].Text = strSUCODE[i];
                ssDrug_Sheet1.Cells[ssDrug_Sheet1.RowCount - 1, 1].Text = strSUNAMEK[i];
                ssDrug_Sheet1.Cells[ssDrug_Sheet1.RowCount - 1, 2].Text = strDRBUN[i];
                ssDrug_Sheet1.Cells[ssDrug_Sheet1.RowCount - 1, 3].Text = strEFFECT[i];
                ssDrug_Sheet1.Cells[ssDrug_Sheet1.RowCount - 1, 4].Text = strQTY[i];
                ssDrug_Sheet1.Cells[ssDrug_Sheet1.RowCount - 1, 5].Text = strGBDIV[i];
                ssDrug_Sheet1.Cells[ssDrug_Sheet1.RowCount - 1, 6].Text = strDOSNAME[i];
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

        private void btnJoYungAdd_Click(object sender, EventArgs e)
        {
            ssJoYung_Sheet1.RowCount = ssJoYung_Sheet1.RowCount + 1;
            ssJoYung_Sheet1.SetRowHeight(ssJoYung_Sheet1.RowCount - 1, ComNum.SPDROWHT);
        }

        private void btnJoYungDel_Click(object sender, EventArgs e)
        {
            if (ComFunc.MsgBoxQ("선택한 약품을 삭제합니다.", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                ssJoYung_Sheet1.RemoveRows(ssJoYung_Sheet1.ActiveRowIndex, 1);
            }
        }

        private void ssJoYung_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssJoYung_Sheet1.Cells[0, 0, ssJoYung_Sheet1.RowCount - 1, ssJoYung_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssJoYung_Sheet1.Cells[e.Row, 0, e.Row, ssJoYung_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }

        private void ssJoYung_EditModeOff(object sender, EventArgs e)
        {
            ssJoYung_Sheet1.Cells[ssJoYung_Sheet1.ActiveRowIndex, 0].Text = ssJoYung_Sheet1.Cells[ssJoYung_Sheet1.ActiveRowIndex, 0].Text.ToUpper().Trim();
            ssJoYung_Sheet1.Cells[ssJoYung_Sheet1.ActiveRowIndex, 1].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, ssJoYung_Sheet1.Cells[ssJoYung_Sheet1.ActiveRowIndex, 0].Text);
        }

        private void rdo11_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbtime2instance.Checked == false 
                && txtbtime2second.Text == "" 
                && txtbtime2time.Text == ""
                && txtbtime2day.Text == "")
            {
                if (rdobtime2.Checked == true)
                {
                    ComFunc.MsgBox("'투약 종료 후 발현' 은 세부 시간 입력 후 선택이 가능합니다.");
                    rdobtime2.Checked = false;
                }
            }
        }

        private void btnADRHelp_Click(object sender, EventArgs e)
        {
            frmComSupADRHelp1 frm = new frmComSupADRHelp1();
            frm.ShowDialog();
        }

        private void txt20_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //GetSabun();
            }
        }

        private void txt20_Leave(object sender, EventArgs e)
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
                if (VB.IsNumeric(strSaBun) ==true)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            panList.Visible = false;
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (e.ColumnHeader == true || e.RowHeader == true) { return; }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strDate = "";
            string strPANO = "";
            string strDrCode = "";
            string strDeptCode = "";
            string strIPDNO = "";
            string strIO = "";

            strPANO = txtptno.Text.Trim();
            strDate = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
            strDeptCode = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();
            txtjo3.Text = ssList_Sheet1.Cells[e.Row, 3].Text.Trim();
            txtjo4.Text = ssList_Sheet1.Cells[e.Row, 4].Text.Trim();
            strDrCode = ssList_Sheet1.Cells[e.Row, 5].Text.Trim();
            strIO = ssList_Sheet1.Cells[e.Row, 6].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (strIO == "I")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     IPDNO ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPANO + "'";
                    SQL = SQL + ComNum.VBLF + "         AND INDATE <= TO_DATE('" + strDate + " 23:59','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "         AND (JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "             OR OUTDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD'))";

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
                        strIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }

                SQL = "";

                switch (strIO)
                {
                    case "O":
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     AGE || '/' || A.SEX AS AGESEX, '' AS ROOMCODE, A.DEPTCODE, C.ILLNAMEK AS DIAGNAME, '외래' AS patient_bun";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_MED + "OCS_OILLS B, " + ComNum.DB_PMPA + "BAS_ILLS C";
                        SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + strPANO + "'";
                        SQL = SQL + ComNum.VBLF + "         AND A.BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE = '" + strDeptCode + "'";
                        SQL = SQL + ComNum.VBLF + "         AND A.BDATE = B.BDATE(+)";
                        SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PTNO(+)";
                        SQL = SQL + ComNum.VBLF + "         AND B.ILLCODE = C.ILLCODE(+)";
                        SQL = SQL + ComNum.VBLF + "ORDER BY B.ILLCODE";
                        break;
                    case "I":
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     AGE || '/' || A.SEX AS AGESEX, ROOMCODE, A.DEPTCODE, C.ILLNAMEK AS DIAGNAME, '입원' AS patient_bun";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_MED + "OCS_IILLS B, " + ComNum.DB_PMPA + "BAS_ILLS C";
                        SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + strPANO + "'";
                        SQL = SQL + ComNum.VBLF + "         AND A.IPDNO = " + strIPDNO;
                        SQL = SQL + ComNum.VBLF + "         AND A.IPDNO = B.IPDNO(+)";
                        SQL = SQL + ComNum.VBLF + "         AND B.ILLCODE = C.ILLCODE(+)";
                        SQL = SQL + ComNum.VBLF + "ORDER BY B.ILLCODE";
                        break;
                    default:
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     PANO AS PTNO, SNAME, SEX AS AGESEX, '' AS ROOMCODE, DEPTCODE, '' AS DIAGNAME, '' AS PATIENT_BUN";
                        SQL = SQL + ComNum.VBLF + "From " + ComNum.DB_PMPA + "BAS_PATIENT";
                        SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPANO + "' ";
                        break;
                }

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
                    if (strIO == "I" || strIO == "O")
                    {
                        txtagesex.Text = dt.Rows[0]["AGESEX"].ToString().Trim();
                    }
                    else
                    {
                        txtagesex.Text = clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, strPANO) + "/" + dt.Rows[0]["AGESEX"].ToString().Trim();
                    }

                    cbopatient_bun.Text = dt.Rows[0]["PATIENT_BUN"].ToString().Trim();
                    txtroomcode.Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    txtdeptcode.Text = strDeptCode;
                    txtdiagname.Text = dt.Rows[0]["DIAGNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = ""; 
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     B.NAME || ' => ' || A.REMARK AS Allergy";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_ALLERGY_MST A, " + ComNum.DB_PMPA + "BAS_BCODE B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.CODE = B.CODE";
                SQL = SQL + ComNum.VBLF + "         AND B.GUBUN = '환자정보_알러지종류'";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = '" + strPANO + "'";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.CODE ASC";

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
                    txtAllergy.Text = dt.Rows[0]["ALLERGY"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                panList.Visible = false;

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

            if (VB.Len(strSaBun)== 4)
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

        void txtWSabun_KeyDown(object sender, KeyEventArgs e)
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
