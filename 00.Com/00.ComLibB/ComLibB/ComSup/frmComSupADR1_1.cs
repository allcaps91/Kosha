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
    /// File Name       : frmComSupADR1_1.cs
    /// Description     : ADR 발생 보고
    /// Author          : 이정현
    /// Create Date     : 2018-01-15
    /// <history> 
    /// ADR 발생 보고
    /// </history>
    /// <seealso>
    /// PSMH\drug\dradr\FrmADR1_1.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\dradr\dradr.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupADR1_1 : Form
    {
        private string GstrSEQNO = "";
        private string GstrROWID = "";

        public frmComSupADR1_1()
        {
            InitializeComponent();
        }

        public frmComSupADR1_1(string strSEQNO)
        {
            InitializeComponent();

            GstrSEQNO = strSEQNO;
        }

        private void frmComSupADR1_1_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ComFunc.SetAllControlClear(this);

            GetJoyoung();

            if (GstrSEQNO != "")
            {
                GstrROWID = readROWID(clsDB.DbCon, GstrSEQNO);
                dataView();
            }

            viewSS1(GstrSEQNO);
            viewSS2(GstrSEQNO);
            viewSS3(GstrSEQNO);
        }

        private void GetJoyoung()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string[] strJoYoung = new string[0];

            FarPoint.Win.Spread.CellType.ComboBoxCellType cboType = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            cboType.AllowEditorVerticalAlign = true;
            cboType.ButtonAlign = FarPoint.Win.ButtonAlign.Right;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "    JEPCODE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_USERJEP";
                SQL = SQL + ComNum.VBLF + "    WHERE DEPTCODE = '055112' ";
                SQL = SQL + ComNum.VBLF + "        AND STOCK > 0";
                SQL = SQL + ComNum.VBLF + "ORDER BY JEPCODE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strJoYoung = new string[dt.Rows.Count];

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strJoYoung[i] = dt.Rows[i]["JEPCODE"].ToString().Trim();
                    }

                    cboType.Items = strJoYoung;
                    ssJoYung_Sheet1.Columns.Get(0).CellType = cboType;
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

            ssDrug_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUCODE, SUNAMEK, DRBUN, EFFECT, QTY, GBDIV, DOSNAME, CHECK1, CHECK2, CHECK3, ROWID, DRUGADD";
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

                        if (dt.Rows[i]["DRUGADD"].ToString().Trim() == "Y")
                        {
                            ssDrug_Sheet1.Cells[i, 0, i, ssDrug_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 255);
                        }
                        else
                        {
                            ssDrug_Sheet1.Cells[i, 0, i, ssDrug_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
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
            int i = 0;

            ssDoubt_Sheet1.RowCount = 0;

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

                    pan2false.Visible = false;
                    pan2ture.Visible = true;
                    panVital.Visible = true;

                    for (i = 0; i < dt.Rows.Count; i ++)
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

        private void btnADRHelp_Click(object sender, EventArgs e)
        {
            frmComSupADRHelp1 frm = new frmComSupADRHelp1();
            frm.ShowDialog();
        }
    }
}
