using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComSupLibB.SupInfc
{
    /// <summary>
    /// Class Name      : ComSupLibB
    /// File Name       : frmComSupInfectionSet.cs
    /// Description     : 격리 정보 관리
    /// Author          : 이정현
    /// Create Date     : 2018-05-15
    /// <history> 
    /// 격리 정보 관리
    /// </history> 
    /// <seealso>
    /// PSMH\exam\exinfect\Exinfect27.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\exam\exinfect\exinfect.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupInfectionSet : Form
    {
        private string GstrPtno = "";
        private string GstrROWID = "";
         
        public frmComSupInfectionSet()
        {
            InitializeComponent();
        }

        public frmComSupInfectionSet(string strPtno)
        {
            InitializeComponent();

            GstrPtno = strPtno;
        }

        private void frmComSupInfectionSet_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            txtPtno.Text = "";
             
            panJob.Enabled = true;

            cboGB.Items.Clear();
            cboGB.Items.Add("**.전체");
            cboGB.Items.Add("01.혈행성 감염");
            cboGB.Items.Add("02.접촉 감염");
            cboGB.Items.Add("03.공기 감염");
            cboGB.Items.Add("04.비말 감염");
            cboGB.SelectedIndex = 0;

            cboGBNEW.Items.Clear();
            cboGBNEW.Items.Add("01.혈행성 감염");
            cboGBNEW.Items.Add("02.접촉 감염");
            cboGBNEW.Items.Add("03.공기 감염");
            cboGBNEW.Items.Add("04.비말 감염");
            cboGBNEW.Items.Add("05.보호 격리");
            //cboGBNEW.Items.Add("06.해외 경유자");
            cboGBNEW.Items.Add("06.신종감염병증후군");
            cboGBNEW.SelectedIndex = 0;

            cboExName.Items.Clear();
            cboExName.Items.Add("A01.HIV");
            cboExName.Items.Add("A02.VDRL");
            cboExName.Items.Add("A03.Hepatitis B");
            cboExName.Items.Add("A04.Hepatitis C");
            cboExName.Items.Add("A05.기타");
            //cboExName.Items.Add("02.CRE");
            //cboExName.Items.Add("02.VRE");
            //cboExName.Items.Add("02.MRAB");
            //cboExName.Items.Add("02.MRPA");
            //cboExName.Items.Add("02.MRSA");
            //cboExName.Items.Add("02.A형간염");
            //cboExName.Items.Add("02.RotaVirus");
            //cboExName.Items.Add("02.C.difficle");
            //cboExName.Items.Add("03.수두");
            //cboExName.Items.Add("03.홍역");
            //cboExName.Items.Add("03.호흡기결핵");

            SCREEN_CLEAR();

            if (GstrPtno != "")
            {
                txtPtno.Text = GstrPtno;
                lblSName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, txtPtno.Text);
                GetData();
            }
        }

        private void SCREEN_CLEAR()
        {
            dtpRDate.Checked = false;
            dtpODate.Checked = false;

            cboExName.Text = "";
            txtSpecNo.Text = "";
            GstrROWID = "";
            panJob.Enabled = false;
            grp_Sel.Visible = false;
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

            grp_Sel.Visible = false;
            ssView_Sheet1.RowCount = 0;

            if (txtPtno.Text.Trim() == "") { return; }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(RDATE,'YYYY-MM-DD') AS RDATE, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(ODATE,'YYYY-MM-DD') AS ODATE, ";
                SQL = SQL + ComNum.VBLF + "     GUBUN, EXNAME, SPECNO, CODE, Info, ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_INFECT_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + txtPtno.Text + "' ";

                if (VB.Left(cboGB.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + "         AND GUBUN = '" + VB.Left(cboGB.Text, 2) + "' ";
                }

                if (chkALL.Checked != true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND ODATE IS NULL ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY RDATE DESC ";

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
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["EXNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SPECNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ODATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["INFO"].ToString().Trim();

                        if (dt.Rows[i]["ODATE"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[i, 0, i, ssView_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 200);
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (SaveData() == true)
            {
                SCREEN_CLEAR();
                GetData();
            }
        }

        private bool SaveData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strCODE = "";
            string strInfo = "";
            string strName = "";
            string strRDate = dtpRDate.Checked == true ? dtpRDate.Value.ToString("yyyy-MM-dd") : "";
            string strODate = dtpODate.Checked == true ? dtpODate.Value.ToString("yyyy-MM-dd") : "";

            if (cboExName.Text.Trim() == "")
            {
                ComFunc.MsgBox("상세질환구분을 입력해주세요.");
                return rtnVal;
            }

            if (txtPtno.Text.Trim() == "") { return rtnVal; }

            strCODE = VB.Split(cboExName.Text, ".")[0];
            strName = VB.Mid(cboExName.Text, 5, cboExName.Text.Length).Trim();
            strInfo = txtInfo.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (GstrROWID == "")
                {
                    if (ComFunc.MsgBoxQ("신규등록입니다. 계속 등록 하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_MED + "EXAM_INFECT_MASTER";
                    SQL = SQL + ComNum.VBLF + "     (PANO, RDATE, ODATE, GUBUN, EXNAME, SPECNO, CODE, INFO)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         '" + txtPtno.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strRDate.Trim() + "', 'YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strODate.Trim() + "', 'YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboGBNEW.Text, 2).Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strName.Trim() + "' ,";
                    SQL = SQL + ComNum.VBLF + "         '" + txtSpecNo.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strCODE.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strInfo + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";
                }
                else
                {
                    if (ComFunc.MsgBoxQ("수정 하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_MED + "EXAM_INFECT_MASTER";
                    SQL = SQL + ComNum.VBLF + "     SET";
                    SQL = SQL + ComNum.VBLF + "         RDATE = TO_DATE('" + strRDate.Trim() + "', 'YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         ODATE = TO_DATE('" + strODate.Trim() + "', 'YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         GUBUN = '" + VB.Left(cboGBNEW.Text, 2).Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         EXNAME = '" + strName.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         SPECNO = '" + txtSpecNo.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         INFO = '" + strInfo + "', ";
                    SQL = SQL + ComNum.VBLF + "         CODE = '" + strCODE + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + GstrROWID + "' ";
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

            if (DelData() == true)
            {
                SCREEN_CLEAR();
                GetData();
            }
        }

        private bool DelData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            if (GstrROWID == "") { return rtnVal; }
            if (ComFunc.MsgBoxQ("삭제 처리하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No) { return rtnVal; }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "DELETE " + ComNum.DB_MED + "EXAM_INFECT_MASTER";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + GstrROWID + "' ";

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

        private void btnNew_Click(object sender, EventArgs e)
        { 
            grp_Sel.Visible = false;
            dtpRDate.Checked = true;

            if (txtPtno.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호를 입력하세요.");
                return;
            }

            GstrROWID = "";
            panJob.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPtno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPtno.Text.Trim() == "") { return; }

                txtPtno.Text = ComFunc.LPAD(txtPtno.Text, 8, "0");
                lblSName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, txtPtno.Text.Trim());
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true || e.RowHeader == true) { return; }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strTmp = "";
            string strCODE = "";
            string strInfo = "";

            dtpRDate.Checked = true;

            if (ssView_Sheet1.Cells[e.Row, 0].Text.Trim() != "")
            {
                dtpRDate.Value = Convert.ToDateTime(ssView_Sheet1.Cells[e.Row, 0].Text.Trim());
            }

            strTmp = ssView_Sheet1.Cells[e.Row, 1].Text.Trim();
            txtSpecNo.Text = ssView_Sheet1.Cells[e.Row, 2].Text.Trim();

            switch (VB.Left(ssView_Sheet1.Cells[e.Row, 3].Text.Trim(), 2))
            {
                case "01": cboGBNEW.Text = "01.혈행성 감염"; break;
                case "02": cboGBNEW.Text = "02.접촉 감염"; break;
                case "03": cboGBNEW.Text = "03.공기 감염"; break;
                case "04": cboGBNEW.Text = "04.비말 감염"; break;
                case "05": cboGBNEW.Text = "05.보호 격리"; break;
                case "06": cboGBNEW.Text = "06.신종감염병증후군"; break;
            }

            if (ssView_Sheet1.Cells[e.Row, 4].Text.Trim() != "")
            {
                dtpODate.Checked = true;
                dtpODate.Value = Convert.ToDateTime(ssView_Sheet1.Cells[e.Row, 4].Text.Trim());
            }

            GstrROWID = ssView_Sheet1.Cells[e.Row, 5].Text.Trim();
            strCODE = ssView_Sheet1.Cells[e.Row, 6].Text.Trim();
            strInfo = ssView_Sheet1.Cells[e.Row, 7].Text.Trim();

            grp_Sel.Visible = false;
            txtInfo.Text = strInfo;

            if (strInfo != "")
            {
                grp_Sel.Visible = true;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                cboExName.Text = "";
                cboExName.Items.Clear();

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     CODE, NAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'INFACT_격리상세질환' ";

                if (VB.Left(cboGBNEW.Text, 2) == "01") { SQL = SQL + ComNum.VBLF + "         AND CODE LIKE 'A%' "; }
                if (VB.Left(cboGBNEW.Text, 2) == "02") { SQL = SQL + ComNum.VBLF + "         AND CODE LIKE 'B%' "; }
                if (VB.Left(cboGBNEW.Text, 2) == "03") { SQL = SQL + ComNum.VBLF + "         AND CODE LIKE 'C%' "; }
                if (VB.Left(cboGBNEW.Text, 2) == "04") { SQL = SQL + ComNum.VBLF + "         AND CODE LIKE 'D%' "; }
                if (VB.Left(cboGBNEW.Text, 2) == "05") { SQL = SQL + ComNum.VBLF + "         AND CODE LIKE 'E%' "; }
                if (VB.Left(cboGBNEW.Text, 2) == "06") { SQL = SQL + ComNum.VBLF + "         AND CODE LIKE 'F%' "; }

                SQL = SQL + ComNum.VBLF + "ORDER BY CODE ";

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
                        cboExName.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboExName.Text = strCODE + "." + strTmp;

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

        void cboGBNEW_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboExName.Text = "";
        }

        void cboGBNEW_Leave(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수 

            cboExName.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT CODE, NAME  FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN ='INFACT_격리상세질환' ";

                if (VB.Left(cboGBNEW.Text, 2) == "01")
                {
                    SQL = SQL + ComNum.VBLF + "  AND CODE LIKE 'A%' ";
                }

                if (VB.Left(cboGBNEW.Text, 2) == "02")
                {
                    SQL = SQL + ComNum.VBLF + "  AND CODE LIKE 'B%' ";
                }

                if (VB.Left(cboGBNEW.Text, 2) == "03")
                {
                    SQL = SQL + ComNum.VBLF + "  AND CODE LIKE 'C%' ";
                }

                if (VB.Left(cboGBNEW.Text, 2) == "04")
                {
                    SQL = SQL + ComNum.VBLF + "  AND CODE LIKE 'D%' ";
                }

                if (VB.Left(cboGBNEW.Text, 2) == "05")
                {
                    SQL = SQL + ComNum.VBLF + "  AND CODE LIKE 'E%' ";
                }

                if (VB.Left(cboGBNEW.Text, 2) == "06")
                {
                    SQL = SQL + ComNum.VBLF + "  AND CODE LIKE 'F%' ";
                }


                SQL = SQL + ComNum.VBLF + "ORDER BY CODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboExName.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
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

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
    }
}
