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
    /// File Name       : frmComSupADR3_1.cs
    /// Description     : 약물이상반응에 대한 2차 평가자 의견
    /// Author          : 이정현
    /// Create Date     : 2018-01-15
    /// <history> 
    /// 약물이상반응에 대한 2차 평가자 의견
    /// </history>
    /// <seealso>
    /// PSMH\drug\dradr\FrmADR3_1.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\dradr\dradr.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupADR3_1 : Form
    {
        private string GstrSEQNO = "";
        private string GstrROWID = "";
        private string GstrGubun = "";

        public frmComSupADR3_1()
        {
            InitializeComponent();
        }

        public frmComSupADR3_1(string strSEQNO, string strGubun = "3")
        {
            InitializeComponent();

            GstrSEQNO = strSEQNO;
            GstrGubun = strGubun;
        }

        private void frmComSupADR3_1_Load(object sender, EventArgs e)
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

            if (GstrSEQNO != "")
            {
                GstrROWID = readROWID(clsDB.DbCon, GstrSEQNO, GstrGubun);
                dataView(GstrGubun);
            }
        }

        public void GetData(string strSEQNO, string strGubun)
        {
            rdoclass1.Checked = false;
            rdoclass2.Checked = false;
            rdoclass3.Checked = false;
            rdoclass4.Checked = false;
            rdorelation1.Checked = false;
            rdorelation2.Checked = false;
            rdorelation3.Checked = false;
            rdorelation4.Checked = false;

            GstrROWID = readROWID(clsDB.DbCon, strSEQNO, strGubun);
            dataView(strGubun);
        }

        public string SaveData(string strWDATE, string strWNAME, string strWBUSE, string strSABUN, int argwsabun=0)
        {
            string SQL = "";

            SQL = "";
            SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR3";
            SQL = SQL + ComNum.VBLF + "    (SEQNO, RELATION1, RELATION2, RELATION3, RELATION4, RELATIONMEMO,";
            SQL = SQL + ComNum.VBLF + "    CLASS1, CLASS2, CLASS3, CLASS4, WDATE,WSabun, WNAME, WBUSE, WRITEDATE, WRITESABUN)";
            SQL = SQL + ComNum.VBLF + "VALUES";
            SQL = SQL + ComNum.VBLF + "    (";
            SQL = SQL + ComNum.VBLF + "         " + GstrSEQNO + ", ";
            SQL = SQL + ComNum.VBLF + "         '" + (rdorelation1.Checked == true ? "1" : "0") + "', ";
            SQL = SQL + ComNum.VBLF + "         '" + (rdorelation2.Checked == true ? "1" : "0") + "', ";
            SQL = SQL + ComNum.VBLF + "         '" + (rdorelation3.Checked == true ? "1" : "0") + "', ";
            SQL = SQL + ComNum.VBLF + "         '" + (rdorelation4.Checked == true ? "1" : "0") + "', ";
            SQL = SQL + ComNum.VBLF + "         '" + txtrelationMemo.Text.Trim() + "', ";
            SQL = SQL + ComNum.VBLF + "         '" + (rdoclass1.Checked == true ? "1" : "0") + "', ";
            SQL = SQL + ComNum.VBLF + "         '" + (rdoclass2.Checked == true ? "1" : "0") + "', ";
            SQL = SQL + ComNum.VBLF + "         '" + (rdoclass3.Checked == true ? "1" : "0") + "', ";
            SQL = SQL + ComNum.VBLF + "         '" + (rdoclass4.Checked == true ? "1" : "0") + "', ";
            SQL = SQL + ComNum.VBLF + "         '" + strWDATE + "', ";
            SQL = SQL + ComNum.VBLF + "         " + argwsabun + ", "; //2019-03-06
            SQL = SQL + ComNum.VBLF + "         '" + strWNAME + "', ";
            SQL = SQL + ComNum.VBLF + "         '" + strWBUSE + "', ";
            SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
            SQL = SQL + ComNum.VBLF + "         '" + strSABUN + "' ";
            SQL = SQL + ComNum.VBLF + "    )";

            return SQL;
        }

        private string readROWID(PsmhDb pDbCon, string strSEQNO, string strGubun)
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
                SQL = "SELECT ROWID FROM " + ComNum.DB_ERP + "DRUG_ADR" + strGubun;
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

        private void dataView(string strGubun)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM " + ComNum.DB_ERP + "DRUG_ADR" + strGubun;
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

                if (strGubun == "2")
                {
                    txtrelationMemo.Text = "1차 평가와 동일";
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
    }
}
