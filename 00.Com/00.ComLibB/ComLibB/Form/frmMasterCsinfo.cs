using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Collections.Generic;

/// <summary>
/// Description : 고객정보관리
/// Author : 박병규
/// Create Date : 2017.06.02
/// </summary>
/// <history>
/// </history>
/// <seealso cref="Csinfo03.frm"/> 

namespace ComLibB
{
    public partial class frmMasterCsinfo : Form
    {
        ComFunc CF = new ComFunc();

        private string GstrPano = "";
        private string GstrCOMMIT = "";

        public frmMasterCsinfo()
        {
            InitializeComponent();
        }

        public frmMasterCsinfo(string strPano)
        {
            InitializeComponent();

            GstrPano = strPano;
        }

        private void frmMasterCsinfo_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            FormClear();
            SetCbo();

            //TODO : 2017.08.10 추후 권한별 버튼 기능 활성화
            //btnSave.Enabled = false;
            //btnCancel.Enabled = false;
            //btnDelete.Enabled = false;

            if (GstrPano != "")
            {
                txtPano.Text = ComFunc.LPAD(GstrPano, 8, "0");
                txtSName.Text = CF.Read_Patient(clsDB.DbCon, txtPano.Text, "2");
                GstrPano = "";

                Screen_Display();
                Screen_CS_Info();
            }
        }

        private void FormClear()
        {
            lblRowId.Text = "";
            GstrCOMMIT = "";

            rdoJob1.Checked = true;
            txtSearch.Text = "";
            ssList_Sheet1.RowCount = 0;

            txtPano.Text = "";
            txtSName.Text = "";
            dtpBDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            txtBuse.Text = "";
            cboDept.Items.Clear();
            cboGubun.Items.Clear();
            dtpEndDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            txtInfoData.Text = "";
            ssView_Sheet1.RowCount = 0;

            dtpBirthDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            cboBirthGbn.Items.Clear();
            txtHPone.Text = "";
            txtTel.Text = "";
            txtEmail.Text = "";
            cboJikup.Items.Clear();
            cboJonggyo.Items.Clear();
            txtLtdName.Text = "";
            txtWonRemark.Text = "";
            txtRemark.Text = "";
        }

        private void SetCbo()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            
            try
            {
                //구분
                cboGubun.Items.Clear();
                
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     CODE, NAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_CODE";
                SQL = SQL + ComNum.VBLF + "     WHERE Gubun = '1' ";

                if (clsPublic.GnDeleteFlag != 1)
                {
                    SQL = SQL + ComNum.VBLF + "     AND CODE IN";
                    SQL = SQL + ComNum.VBLF + "             (SELECT CODE FROM " + ComNum.DB_PMPA + "ETC_CSINFO_BUSE";
                    SQL = SQL + ComNum.VBLF + "                     WHERE BUCODE = '" + clsType.User.BuseCode + "')";
                }
                
                SQL = SQL + ComNum.VBLF + "ORDER BY SORT, CODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        cboGubun.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                //진료과
                cboDept.Items.Clear();

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PRINTRANKING, DEPTCODE, DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + "     WHERE DEPTCODE NOT IN ('II', 'R6') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PRINTRANKING, DEPTCODE ";

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
                        cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                //종교구분
                cboJonggyo.Items.Clear();
                cboJonggyo.Items.Add("");

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     CODE, NAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA +  "ETC_CSINFO_CODE";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = '3' "; //종교
                SQL = SQL + ComNum.VBLF + "ORDER BY CODE ";

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
                        cboJonggyo.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                //직업구분
                cboJikup.Items.Clear();

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     CODE, NAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_CODE ";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = '4' ";   //직업
                SQL = SQL + ComNum.VBLF + "ORDER BY CODE ";

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
                        cboJikup.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                //생일구분
                cboBirthGbn.Items.Clear();
                cboBirthGbn.Items.Add("양력");
                cboBirthGbn.Items.Add("음력");
                cboBirthGbn.Items.Add("");
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

        private void Screen_Display()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strCode = "";
            string strDeptCode = "";

            txtBuse.Text = clsVbfunc.READ_INSA_BUSE(clsDB.DbCon, clsType.User.Sabun);

            try
            {
                //재원자이면
                SQL = "";
                SQL = "SELECT DEPTCODE FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "         AND OUTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "         AND GBSTS  = '0' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strDeptCode == "")
                {
                    SQL = "";
                    SQL = "SELECT DEPTCODE FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + txtPano.Text + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }

                cboDept.SelectedIndex = 0;

                for(i = 0; i < cboDept.Items.Count; i++)
                {
                    if (VB.Left(cboDept.Items[i].ToString(), 2) == strDeptCode)
                    {
                        cboDept.SelectedIndex = i;
                        break;
                    }
                }

                //환자마스타 정보를 Display
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SNAME, TO_CHAR(BIRTH,'YYYY-MM-DD') AS BIRTH, GBBIRTH, HPHONE, EMAIL, GB_VIP, GB_VIP_REMARK, TEL, JIKUP, RELIGION, GBINFOR ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + txtPano.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox(txtPano.Text + " 등록번호가 등록이 안됨", "오류");
                    return;
                }

                if (dt.Rows[0]["BIRTH"].ToString().Trim() != "")
                {
                    dtpBirthDate.Value = Convert.ToDateTime(dt.Rows[0]["BIRTH"].ToString().Trim());
                }

                switch(dt.Rows[0]["GBBIRTH"].ToString().Trim())
                {
                    case "+":
                        cboBirthGbn.SelectedIndex = 0;
                        break;
                    case "-":
                        cboBirthGbn.SelectedIndex = 1;
                        break;
                    default:
                        cboBirthGbn.SelectedIndex = 2;
                        break;
                }

                txtHPone.Text = dt.Rows[0]["HPHONE"].ToString().Trim();
                txtTel.Text = dt.Rows[0]["TEL"].ToString().Trim();
                txtEmail.Text = dt.Rows[0]["EMAIL"].ToString().Trim();
                txtWonRemark.Text = dt.Rows[0]["GBINFOR"].ToString().Trim();

                //직업을 Display
                cboJikup.SelectedIndex = 0;

                strCode = dt.Rows[0]["JIKUP"].ToString().Trim();

                if (strCode != "")
                {
                    for(i = 0; i < cboJikup.Items.Count; i++)
                    {
                        if (VB.Left(cboJikup.Items[i].ToString(), 2) == strCode)
                        {
                            cboJikup.SelectedIndex = i;
                            break;
                        }
                    }
                }

                //종교를 Display
                cboJonggyo.SelectedIndex = 0;

                strCode = dt.Rows[0]["RELIGION"].ToString().Trim();

                if (strCode != "")
                {
                    for (i = 0; i < cboJonggyo.Items.Count; i++)
                    {
                        if (VB.Left(cboJonggyo.Items[i].ToString(), 1) == strCode)
                        {
                            cboJonggyo.SelectedIndex = i;
                            break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //참고사항을 READ
                SQL = "";
                SQL = "SELECT LTDNAME, REMARK FROM " + ComNum.DB_PMPA + "ETC_CSINFO_MST ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + txtPano.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    txtLtdName.Text = dt.Rows[0]["LTDNAME"].ToString().Trim();
                    txtRemark.Text = dt.Rows[0]["REMARK"].ToString().Trim();
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

        //고객정보를 Display
        private void Screen_CS_Info()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            lblRowId.Text = "";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE, DEPTCODE, GUBUN, CODE, REMARK, BUSENAME, ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_DATA ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "         AND DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BDATE DESC ";

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

                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BUSENAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = CF.Read_Csinfo_Name(clsDB.DbCon, "", dt.Rows[i]["GUBUN"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetList();
            }
        }

        private void btnPtSearch_Click(object sender, EventArgs e)
        {
            GetList();
        }

        private void GetList()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strSName = "";
            string strJumin = "";

            ssList_Sheet1.RowCount = 0;

            if (txtSearch.Text.Trim() != "")
            {
                if (VB.IsNumeric(txtSearch.Text.Trim()))
                {
                    if (txtSearch.Text.Length == 6)
                    {
                        strJumin = txtSearch.Text.Trim();
                    }
                    else
                    {
                        ComFunc.MsgBox("생년월일 6자리를 확인해주세요");
                        return;
                    }
                }
                else
                {
                    strSName = txtSearch.Text.Trim();
                }
            }

            try
            {
                if (rdoJob0.Checked == true)
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.PANO, TO_SINGLE_BYTE(B.SNAME) SNAME, A.AGE, A.SEX, B.TEL, TO_SINGLE_BYTE(B.PNAME) PNAME, A.WARDCODE, A.ROOMCODE,A.DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.GBSTS IN ('0', '2')";
                    SQL = SQL + ComNum.VBLF + "         AND A.OUTDATE IS NULL";
                    SQL = SQL + ComNum.VBLF + "         AND A.PANO < '90000000' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+) ";

                    if (strSName != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND B.SNAME LIKE '%" + strSName + "%' ";
                    }

                    if (strJumin != "")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND B.JUMIN1 LIKE '" + strJumin + "%' ";
                    }

                    SQL = SQL + ComNum.VBLF + " ORDER BY B.SNAME,B.JUMIN1 ";
                }
                else
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     PANO, TO_SINGLE_BYTE(SNAME) SNAME, SEX, JUMIN1, JUMIN2, TEL, TO_SINGLE_BYTE(PNAME) PNAME, DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";

                    if (strSName != "")
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE SNAME = '" + strSName + "' ";
                    }

                    if (strJumin != "")
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE JUMIN1 = '" + strJumin + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "ORDER BY SNAME,JUMIN1 ";
                }
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssList_Sheet1.RowCount = dt.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();

                    if (rdoJob0.Checked == true)
                    {
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["AGE"].ToString().Trim() + "/" + dt.Rows[i]["SEX"].ToString().Trim();
                    }
                    else
                    {
                        ssList_Sheet1.Cells[i, 2].Text = ComFunc.AgeCalcEx(dt.Rows[i]["JUMIN1"].ToString().Trim() + dt.Rows[i]["JUMIN2"].ToString().Trim(), ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")) + "/" + dt.Rows[i]["SEX"].ToString().Trim();
                    }

                    ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["TEL"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PNAME"].ToString().Trim();

                    //재원자
                    if (rdoJob0.Checked == true)
                    {
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    }

                    ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
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

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssListClick(e.Row);
        }

        private void ssListClick(int intRow)
        {
            txtPano.Text = ssList_Sheet1.Cells[intRow, 0].Text.Trim();
            txtSName.Text = ssList_Sheet1.Cells[intRow, 1].Text.Trim();

            Screen_Display();
            Screen_CS_Info();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("환자가 선택되지 않았습니다.");
                return;
            }

            if (txtSName.Text.Trim() == "")
            {
                txtSName.Text = CF.Read_Patient(clsDB.DbCon, txtPano.Text, "2");
            }

            Screen_Display();
            Screen_CS_Info();

            cboGubun.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if (txtBuse.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록부서가 공란입니다.", "오류");
                return;
            }

            if (txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호가 공란입니다.", "오류");
                return;
            }

            if (txtInfoData.Text.Trim() == "")
            {
                ComFunc.MsgBox("고객정보가 공란입니다.", "오류");
                return;
            }

            if (txtRemark.Text.Trim() == "")
            {
                ComFunc.MsgBox("정보구분이 공란입니다.", "오류");
                return;
            }

            if (cboDept.Text.Trim() == "")
            {
                ComFunc.MsgBox("진료과가 공란입니다.", "오류");
                return;
            }

            if (SaveData() == true)
            {
                Screen_CS_Info();

                GstrCOMMIT = "";
                lblRowId.Text = "";
                cboGubun.SelectedIndex = 0;
                txtInfoData.Text = "";
                txtRemark.Text = "";
                btnDelete.Enabled = false;

                cboGubun.Focus();
            }
        }

        private bool SaveData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strGubun = ComFunc.LeftH(cboGubun.Text, 3).Trim();
            string strInfoData = txtInfoData.Text.Replace("'", "`").Trim();
            string strRemark = txtRemark.Text.Replace("'", "`").Trim();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (lblRowId.Text.Trim() != "")
                {
                    //변경전 내용을 HISTORY에 INSERT
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_CSINFO_HISTORY";
                    SQL = SQL + ComNum.VBLF + "     (JOBDATE, JOBSABUN, GBJOB, BDATE, PANO, GUBUN, ";
                    SQL = SQL + ComNum.VBLF + "      CODE, REMARK, DELDATE, BUSENAME, ENTDATE, ENTSABUN, DEPTCODE) ";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SYSDATE, " + clsType.User.Sabun + ", '2', BDATE, PANO, GUBUN,";
                    SQL = SQL + ComNum.VBLF + "     CODE, REMARK, DELDATE, BUSENAME, ENTDATE, ENTSABUN, DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_DATA ";
                    SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + lblRowId.Text.Trim() + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        GstrCOMMIT = "NO";
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        ComFunc.MsgBox("변경전 자료를 HISTORY에 등록중 오류가 발생함");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    //변경내용을 고객정보에 UPDATE
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_PMPA + "ETC_CSINFO_DATA";
                    SQL = SQL + ComNum.VBLF + "     SET ";
                    SQL = SQL + ComNum.VBLF + "         BDate = TO_DATE('" + dtpBDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + "         BuseName = '" + txtBuse.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "         Pano='" + txtPano.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "         DeptCode='" + VB.Left(cboDept.Text, 2) + "',";
                    SQL = SQL + ComNum.VBLF + "         Gubun='" + strGubun + "',";
                    SQL = SQL + ComNum.VBLF + "         Remark='" + strInfoData + "',";
                    SQL = SQL + ComNum.VBLF + "         EndDate=TO_DATE('" + dtpEndDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + "         EntSabun=" + clsType.User.Sabun + ",";
                    SQL = SQL + ComNum.VBLF + "         EntDate=SYSDATE ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID='" + lblRowId.Text.Trim() + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        GstrCOMMIT = "NO";
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        ComFunc.MsgBox("자료를 등록중 오류가 발생함");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    //변경후 내용을 HISTORY에 INSERT
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_CSINFO_HISTORY";
                    SQL = SQL + ComNum.VBLF + "     (JOBDATE, JOBSABUN, GBJOB, BDATE, PANO, GUBUN, ";
                    SQL = SQL + ComNum.VBLF + "      CODE, REMARK, DelDATE, BUSENAME, ENTDATE, ENTSABUN, DEPTCODE) ";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SYSDATE, " + clsType.User.Sabun + ", '3', BDATE, PANO, GUBUN,";
                    SQL = SQL + ComNum.VBLF + "     CODE, REMARK, DELDATE, BUSENAME, ENTDATE, ENTSABUN, DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_DATA ";
                    SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + lblRowId.Text.Trim() + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        GstrCOMMIT = "NO";
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        ComFunc.MsgBox("변경후 자료를 HISTORY에 등록중 오류가 발생함");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }
                else
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_CSINFO_DATA";
                    SQL = SQL + ComNum.VBLF + "     (BDate, Pano, DeptCode, Gubun, Remark, EndDate, BuseName, EntSabun, EntDate)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpBDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtPano.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboDept.Text, 2) + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGubun + "',";
                    SQL = SQL + ComNum.VBLF + "         '" + strInfoData + "',";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpEndDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtBuse.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun + ",";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE";
                    SQL = SQL + ComNum.VBLF + "     ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        GstrCOMMIT = "NO";
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        ComFunc.MsgBox("자료를 신규 등록중 오류가 발생함");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    //신규등록 내용을 HISTORY에 INSERT
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_CSINFO_HISTORY";
                    SQL = SQL + ComNum.VBLF + "     (JobDate, JobSabun, GbJob, BDate, Pano, DeptCode, Gubun, Remark, DelDate, BuseName, EntSabun, EntDate)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE,";
                    SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun + ",";
                    SQL = SQL + ComNum.VBLF + "         '1',";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpBDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + "         '" + txtPano.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboDept.Text, 2) + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGubun + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strInfoData + "',";
                    SQL = SQL + ComNum.VBLF + "         '',";
                    SQL = SQL + ComNum.VBLF + "         '" + txtBuse.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun + ",";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE";
                    SQL = SQL + ComNum.VBLF + "     ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        GstrCOMMIT = "NO";
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        ComFunc.MsgBox("자료를 HISTORY에 등록중 오류가 발생함");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            GstrCOMMIT = "";
            lblRowId.Text = "";
            cboGubun.SelectedIndex = 0;
            txtInfoData.Text = "";
            txtRemark.Text = "";
            btnDelete.Enabled = false;

            cboGubun.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("정말로 삭제를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            
            GstrCOMMIT = "";

            if (DeleteData() == true)
            {
                Screen_CS_Info();

                GstrCOMMIT = "";
                lblRowId.Text = "";
                cboGubun.SelectedIndex = 0;
                txtInfoData.Text = "";
                txtRemark.Text = "";
                btnDelete.Enabled = false;

                cboGubun.Focus();
            }
        }

        private bool DeleteData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                //삭제한 내용을 HISTORY에 INSERT
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_CSINFO_HISTORY";
                SQL = SQL + ComNum.VBLF + "     (JOBDATE, JOBSABUN, GBJOB, BDATE, PANO, GUBUN,";
                SQL = SQL + ComNum.VBLF + "      CODE, REMARK, DELDATE, BUSENAME, ENTDATE, ENTSABUN, DEPTCODE) ";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     SYSDATE, " + clsType.User.Sabun + ", '4', BDATE, PANO, GUBUN,";
                SQL = SQL + ComNum.VBLF + "     CODE, REMARK, DELDATE, BUSENAME, ENTDATE, ENTSABUN, DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_DATA ";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + lblRowId.Text.Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    GstrCOMMIT = "NO";
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    ComFunc.MsgBox("삭제한 자료를 HISTORY에 등록중 오류가 발생함");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //자료를 삭제
                SQL = "";
                SQL = "DELETE " + ComNum.DB_PMPA + "ETC_CSINFO_DATA";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + lblRowId.Text.Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    GstrCOMMIT = "NO";
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    ComFunc.MsgBox("자료를 삭제 도중에 오류가 발생함");
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

        private void btnNext_Click(object sender, EventArgs e)
        {
            FormClear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveInfo_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            string strHisJob = "";
            string strGbBirth = "";

            string strROWID = "";
            string strRemark = "";
            string strLtdName = "";

            string SQL = "";
            DataTable dt = null;

            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                strHisJob = "2";    //변경전

                if (Save_History_INSERT(strHisJob, clsDB.DbCon, strGbBirth, strLtdName, strRemark) == false)
                {
                    return;
                }

                strGbBirth = "";

                if (cboBirthGbn.Text.Trim() == "양력") { strGbBirth = "+"; }
                if (cboBirthGbn.Text.Trim() == "음력") { strGbBirth = "-"; }

                strRemark = txtRemark.Text.Replace("'", "`").Trim();
                strLtdName = txtLtdName.Text.Replace("'", "`").Trim();


                #region 환자인적사항 변경 내역 백업
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("BIRTH", dtpBirthDate.Value.ToString("yyyy-MM-dd"));
                dict.Add("HPHONE", txtHPone.Text.Trim());
                dict.Add("TEL", txtTel.Text.Trim());
                CF.INSERT_BAS_PATIENT_HIS(txtPano.Text.Trim(), dict);
                #endregion

                //환자마스타를 변경함
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT";
                SQL = SQL + ComNum.VBLF + "     SET";
                SQL = SQL + ComNum.VBLF + "         Birth = TO_DATE('" + dtpBirthDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + "         GbBirth = '" + strGbBirth + "',";
                SQL = SQL + ComNum.VBLF + "         HPhone = '" + txtHPone.Text.Trim() + "',";
                SQL = SQL + ComNum.VBLF + "         Tel = '" + txtTel.Text.Trim() + "',";
                SQL = SQL + ComNum.VBLF + "         EMail = '" + txtEmail.Text.Trim() + "',";
                SQL = SQL + ComNum.VBLF + "         Jikup = '" + VB.Left(cboJikup.Text, 2).Trim() + "',";
                SQL = SQL + ComNum.VBLF + "         Religion = '" + VB.Left(cboJonggyo.Text, 1).Trim() + "',";
                SQL = SQL + ComNum.VBLF + "         GbInfor = '" + txtWonRemark.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + txtPano.Text + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    GstrCOMMIT = "NO";
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    ComFunc.MsgBox("환자마스타에 자료를 UPDATE도중 오류 발생");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //마스타에 자료를 UPDATE
                SQL = "";
                SQL = "SELECT ROWID FROM " + ComNum.DB_PMPA + "ETC_CSINFO_MST ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + txtPano.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strROWID != "")
                {
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_PMPA + "ETC_CSINFO_MST";
                    SQL = SQL + ComNum.VBLF + "     SET ";
                    SQL = SQL + ComNum.VBLF + "         LtdName = '" + strLtdName + "',";
                    SQL = SQL + ComNum.VBLF + "         Remark = '" + strRemark + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                }
                else if (strRemark != "" || strLtdName != "")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_CSINFO_MST ";
                    SQL = SQL + ComNum.VBLF + "     (Pano,LtdName,Remark)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         '" + txtPano.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strLtdName + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strRemark + "'";
                    SQL = SQL + ComNum.VBLF + "     ) ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    GstrCOMMIT = "NO";
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    ComFunc.MsgBox("ETC_CSINFO_MST 자료를 등록중 오류가 발생함");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                strHisJob = "3";    //변경후

                if (Save_History_INSERT(strHisJob, clsDB.DbCon, strGbBirth, strLtdName, strRemark) == false)
                {
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
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
            }
        }

        private bool Save_History_INSERT(string strHisJob, PsmhDb pDbCon, string strGbBirth, string strLtdName, string strRemark)
        {
            string SQL = "";
            DataTable dt = null;

            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strEMail = "";
            string strJikup = "";
            string strReligion = "";
            string strGbInfor = "";
            string strTel = "";
            string strHPhone = "";
            string strBirth = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     Tel, HPhone, TO_CHAR(Birth,'YYYY-MM-DD') AS Birth, GbBirth, EMail, Jikup, Religion, GbInfor ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + txtPano.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strTel = dt.Rows[0]["TEL"].ToString().Trim();
                    strHPhone = dt.Rows[0]["HPHONE"].ToString().Trim();
                    strBirth = dt.Rows[0]["BIRTH"].ToString().Trim();
                    strGbBirth = dt.Rows[0]["GBBIRTH"].ToString().Trim();
                    strEMail = dt.Rows[0]["EMAIL"].ToString().Trim();
                    strJikup = dt.Rows[0]["JIKUP"].ToString().Trim();
                    strReligion = dt.Rows[0]["RELIGION"].ToString().Trim();
                    strGbInfor = dt.Rows[0]["GBINFOR"].ToString().Trim();
                }
                else
                {
                    strGbBirth = "";
                }

                dt.Dispose();
                dt = null;

                //고객정보를 READ
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     LtdName,Remark ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_MST ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + txtPano.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strLtdName = dt.Rows[0]["LTDNAME"].ToString().Trim();
                    strRemark = dt.Rows[0]["REMARK"].ToString().Trim();
                }
                else
                {
                    strLtdName = "";
                    strRemark = "";
                }

                dt.Dispose();
                dt = null;

                //변경 History에 INSERT
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_CSINFO_MSTHISTORY";
                SQL = SQL + ComNum.VBLF + "     (JOBDATE, JOBSABUN, GBJOB, Pano, TEL, HPHONE, BIRTH, GBBIRHT, EMAIL,";
                SQL = SQL + ComNum.VBLF + "      JIKUP, RELIGION, GBINFOR, LTDNAME, REMARK)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun + ",";
                SQL = SQL + ComNum.VBLF + "         '" + strHisJob + "',";
                SQL = SQL + ComNum.VBLF + "         '" + txtPano.Text + "',";
                SQL = SQL + ComNum.VBLF + "         '" + strTel + "',";
                SQL = SQL + ComNum.VBLF + "         '" + strHPhone + "',";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strBirth + "','YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + "         '" + strGbBirth + "',";
                SQL = SQL + ComNum.VBLF + "         '" + strEMail + "',";
                SQL = SQL + ComNum.VBLF + "         '" + strJikup + "',";
                SQL = SQL + ComNum.VBLF + "         '" + strReligion + "',";
                SQL = SQL + ComNum.VBLF + "         '" + strGbInfor + "',";
                SQL = SQL + ComNum.VBLF + "         '" + strLtdName + "',";
                SQL = SQL + ComNum.VBLF + "         '" + strRemark + "'";
                SQL = SQL + ComNum.VBLF + "     ) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    GstrCOMMIT = "NO";
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    ComFunc.MsgBox("ETC_CSINFO_MSTHISTORY 자료를 등록중 오류가 발생함");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
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

                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strOK = "";
            string strCode = "";

            lblRowId.Text = ssView_Sheet1.Cells[e.Row, 5].Text.Trim();

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE, BUSENAME, PANO, GUBUN,";
                SQL = SQL + ComNum.VBLF + "     DEPTCODE, REMARK, TO_CHAR(ENDDATE,'YYYY-MM-DD') AS ENDDATE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_CSINFO_DATA ";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + lblRowId.Text.Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strOK = "NO";

                    if (clsPublic.GnDeleteFlag == 1) { strOK = "OK"; }
                    if (dt.Rows[0]["BUSENAME"].ToString().Trim() == clsVbfunc.GetSaBunBuSeName(clsDB.DbCon, clsType.User.Sabun)) { strOK = "OK"; }

                    if (strOK == "NO")
                    {
                        btnDelete.Enabled = true;

                        ComFunc.MsgBox("등록된 자료의 수정, 삭제는 작성부서 및"
                            + ComNum.VBLF + "비서실, 간호부에서만 가능합니다.", "변경불가!!");
                    }
                    else
                    {
                        dtpBDate.Value = Convert.ToDateTime(dt.Rows[0]["BDATE"].ToString().Trim());
                        txtBuse.Text = dt.Rows[0]["BUSENAME"].ToString().Trim();
                        txtPano.Text = dt.Rows[0]["PANO"].ToString().Trim();
                        txtSName.Text = CF.Read_Patient(clsDB.DbCon, txtPano.Text, "2");
                        //정보구분
                        strCode = dt.Rows[0]["GUBUN"].ToString().Trim();
                        
                        for(i = 0; i < cboGubun.Items.Count; i++)
                        {
                            if (ComFunc.LeftH(cboGubun.Items[i].ToString(), 3) == strCode)
                            {
                                cboGubun.SelectedIndex = i;
                                break;
                            }
                        }
                        
                        for(i = 0; i < cboDept.Items.Count; i++)
                        {
                            if (ComFunc.LeftH(cboDept.Items[i].ToString(), 2) == strCode)
                            {
                                cboDept.SelectedIndex = i;
                                break;
                            }
                        }

                        txtInfoData.Text = dt.Rows[0]["REMARK"].ToString().Trim();
                        dtpEndDate.Value = Convert.ToDateTime(dt.Rows[0]["ENDDATE"].ToString().Trim());
                        btnDelete.Enabled = true;
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

        private void rdoJob_CheckedChanged(object sender, EventArgs e)
        {
            //TODO : 2017.08.10 추후 사용
            //if (((RadioButton)sender).Checked == true)
            //{
            //    GetList();
            //}

            if (rdoJob0.Checked == true)
            {
                GetList();
            }
            else
            {
                ssList_Sheet1.RowCount = 0;
            }
        }
    }
}
