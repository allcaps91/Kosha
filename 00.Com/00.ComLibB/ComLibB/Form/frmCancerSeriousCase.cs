using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmCancerSeriousCase
    /// File Name : frmCancerSeriousCase.cs
    /// Title or Description : 중증환자(암) 관리
    /// Author : 박창욱
    /// Create Date : 2017-06-14
    /// Update Histroy :
    /// </summary>
    /// <history>  
    /// VB\Frm암중증환자관리.frm(Frm암중증환자관리) -> frmCancerSeriousCase.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\buppat\Frm암중증환자관리.frm(Frm암중증환자관리)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\buppat\\buppat.vbp
    /// </vbp>
    public partial class frmCancerSeriousCase : Form
    {
        string FstrRowid = "";

        public frmCancerSeriousCase()
        {
            InitializeComponent();
        }

        private void select_view()
        {
            int i = 0;
            int j = 0;
            int nRead = 0;
            string strGubun = "";
            string strPano = "";
            string strDept = "";
            string strDrcode = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (rdoCancer.Checked == true)
            {
                strGubun = "1";
            }
            else
            {
                strGubun = "2";
            }

            strPano = txtPano1.Text;

            try
            {
                SQL = "";
                SQL = " SELECT GUBUN,PANO,SNAME,SEX,AGE,JBUNHO,GKIHO, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(IDATE,'YYYY-MM-DD') IDATE, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(PDATE,'YYYY-MM-DD') PDATE, TO_CHAR(FDATE,'YYYY-MM-DD') FDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(TDATE,'YYYY-MM-DD') TDATE, TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(CANDATE,'YYYY-MM-DD') CANDATE, DEPT1, ILLCODE1,ILLCODE2,ILLCODE3,ILLCODE4, ";
                SQL = SQL + ComNum.VBLF + " MEMO,DRCODE,VCODE ";
                SQL = SQL + ComNum.VBLF + " From " + ComNum.DB_PMPA + "BAS_CANCER ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '" + strGubun + "' ";
                if (txtPano1.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "' ";
                }
                if (chkDel.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL ";
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = nRead;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);


                for (i = 0; i < nRead; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SEX"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["AGE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["JBUNHO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GKIHO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["VCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["IDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["PDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["DELDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["FDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["TDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 14].Text = dt.Rows[i]["CANDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 15].Text = dt.Rows[i]["DEPT1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 16].Text = dt.Rows[i]["ILLCODE1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 17].Text = dt.Rows[i]["ILLCODE2"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 18].Text = dt.Rows[i]["ILLCODE3"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 19].Text = dt.Rows[i]["ILLCODE4"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 20].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 21].Text = dt.Rows[i]["MEMO"].ToString().Trim();
                }

                if (nRead == 1)
                {
                    i = 0;
                    txtPano.Text = dt.Rows[i]["PANO"].ToString().Trim();
                    cboGubun.SelectedIndex = (int)(VB.Val(dt.Rows[i]["GUBUN"].ToString().Trim())) - 1;
                    txtSname.Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    switch (dt.Rows[i]["SEX"].ToString().Trim())
                    {
                        case "M":
                            cboSex.SelectedIndex = 0;
                            break;
                        case "F":
                            cboSex.SelectedIndex = 1;
                            break;
                    }
                    txtAge.Text = dt.Rows[i]["AGE"].ToString().Trim();
                    txtJbunHo.Text = dt.Rows[i]["JBUNHO"].ToString().Trim();
                    txtGKiho.Text = dt.Rows[i]["GKIHO"].ToString().Trim();
                    txtVCode.Text = dt.Rows[i]["VCODE"].ToString().Trim();
                    dtpIDate.Value = Convert.ToDateTime(dt.Rows[i]["IDATE"].ToString().Trim());
                    dtpSDate.Value = Convert.ToDateTime(dt.Rows[i]["SDATE"].ToString().Trim());
                    dtpPDate.Value = Convert.ToDateTime(dt.Rows[i]["PDATE"].ToString().Trim());
                    dtpDelDate.Value = Convert.ToDateTime(dt.Rows[i]["DELDATE"].ToString().Trim());
                    dtpFDate.Value = Convert.ToDateTime(dt.Rows[i]["FDATE"].ToString().Trim());
                    dtpTDate.Value = Convert.ToDateTime(dt.Rows[i]["TDATE"].ToString().Trim());
                    dtpCanDate.Value = Convert.ToDateTime(dt.Rows[i]["CANDATE"].ToString().Trim());

                    strDept = dt.Rows[i]["DEPT1"].ToString().Trim();
                    for (j = 0; j < cboDept1.Items.Count; j++)
                    {
                        if (strDept == VB.Left(cboDept1.Items[j].ToString(), 2))
                        {
                            cboDept1.SelectedIndex = j;
                            break;
                        }
                    }
                    txtiLLCode1.Text = dt.Rows[i]["ILLCODE1"].ToString().Trim();
                    txtiLLCode2.Text = dt.Rows[i]["ILLCODE2"].ToString().Trim();
                    txtiLLCode3.Text = dt.Rows[i]["ILLCODE3"].ToString().Trim();
                    txtiLLCode4.Text = dt.Rows[i]["ILLCODE4"].ToString().Trim();

                    strDrcode = dt.Rows[i]["DRCODE"].ToString().Trim();
                    for (j = 0; j < cboDr.Items.Count; j++)
                    {
                        if (strDrcode == VB.Left(cboDr.Items[j].ToString(), 4))
                        {
                            cboDr.SelectedIndex = j;
                            break;
                        }
                    }

                    txtMemo.Text = dt.Rows[i]["MEMO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void txt_Clear()
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            txtPano.Text = "";
            txtSname.Text = "";
            txtAge.Text = "";
            txtJbunHo.Text = "";
            txtGKiho.Text = "";
            dtpIDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpSDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpPDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpSDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpFDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpTDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            txtiLLCode1.Text = "";
            txtiLLCode2.Text = "";
            txtiLLCode3.Text = "";
            txtiLLCode4.Text = "";
            txtMemo.Text = "";
            txtVCode.Text = "";
            dtpCanDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpDelDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            FstrRowid = "";

            cboGubun.SelectedIndex = 0;
            cboDept1.SelectedIndex = 0;
            cboDr.SelectedIndex = 0;
            cboSex.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            string strPano = "";
            string strGubun = "";
            string strSname = "";
            string strSex = "";
            int nAge = 0;
            string strJbunHo = "";
            string strGKiho = "";
            string strIDate = "";
            string strSDate = "";
            string strPDate = "";
            string strDelDate = "";
            string strFDate = "";
            string strTDate = "";
            string strDept = "";
            string striLLCode1 = "";
            string striLLCode2 = "";
            string striLLCode3 = "";
            string striLLCode4 = "";
            string strDrCode = "";
            string strMemo = "";
            string strVCode = "";
            string strCanDate = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (txtPano.Text.Trim() == "")
            {
                return;
            }

            strPano = txtPano.Text.Trim();
            strGubun = VB.Left(cboGubun.Text, 1);
            strSname = txtSname.Text.Trim();
            strSex = VB.Left(cboSex.Text, 1);
            strJbunHo = txtJbunHo.Text.Trim();
            strGKiho = txtGKiho.Text.Trim();
            nAge = (int)VB.Val(txtAge.Text);
            strIDate = dtpIDate.Text;
            strSDate = dtpSDate.Text;
            strPDate = dtpPDate.Text;
            strDelDate = dtpDelDate.Text;
            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Text;
            strCanDate = dtpCanDate.Text;
            strDept = VB.Left(cboDept1.Text, 2);
            striLLCode1 = txtiLLCode1.Text;
            striLLCode2 = txtiLLCode2.Text;
            striLLCode3 = txtiLLCode3.Text;
            striLLCode4 = txtiLLCode4.Text;
            strDrCode = VB.Left(cboDr.Text.Trim(), 4);
            strVCode = txtVCode.Text;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (FstrRowid == "")
                {
                    SQL = "";
                    SQL = " INSERT INTO BAS_CANCER(GUBUN, PANO,SNAME,SEX,AGE,JBUNHO,GKIHO,IDATE,SDATE,PDATE,";
                    SQL = SQL + ComNum.VBLF + " FDATE,TDATE,DELDATE,CANDATE,DEPT1,DEPT2,DEPT3,ILLCODE1,ILLCODE2,";
                    SQL = SQL + ComNum.VBLF + " ILLCODE3,ILLCODE4,MEMO,DRCODE , ENTSABUN, VCODE) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + "'" + strGubun + "',  '" + strPano + "','" + strSname + "', '" + strSex + "', ";
                    SQL = SQL + ComNum.VBLF + " " + nAge + ", '" + strJbunHo + "', '" + strGKiho + "', ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strIDate + "','YYYY-MM-DD'), TO_DATE('" + strSDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strPDate + "','YYYY-MM-DD'), TO_DATE('" + strFDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strTDate + "','YYYY-MM-DD'), TO_DATE('" + strDelDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strCanDate + "','YYYY-MM-DD'), '" + strDept + "','','', ";
                    SQL = SQL + ComNum.VBLF + " '" + striLLCode1 + "', '" + striLLCode2 + "', '" + striLLCode3 + "', '" + striLLCode4 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strMemo + "', '" + strDrCode + "', " + clsPublic.GnJobSabun + ",'" + strVCode + "') ";
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE BAS_CANCER SET ";
                    SQL = SQL + ComNum.VBLF + " GUBUN = '" + strGubun + "', PANO = '" + strPano + "', ";
                    SQL = SQL + ComNum.VBLF + " SNAME = '" + strSname + "', SEX ='" + strSex + "', ";
                    SQL = SQL + ComNum.VBLF + " AGE = " + nAge + ", JBUNHO = '" + strJbunHo + "', ";
                    SQL = SQL + ComNum.VBLF + " GKIHO = '" + strGKiho + "', IDATE = TO_DATE('" + strIDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " SDATE = TO_DATE('" + strSDate + "','YYYY-MM-DD'), PDATE = TO_DATE('" + strPDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " FDATE = TO_DATE('" + strFDate + "','YYYY-MM-DD'), TDATE = TO_DATE('" + strTDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " DELDATE = TO_DATE('" + strDelDate + "','YYYY-MM-DD'), CANDATE = TO_DATE('" + strCanDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " DEPT1 = '" + strDept + "', ILLCODE1 = '" + striLLCode1 + "', ";
                    SQL = SQL + ComNum.VBLF + " ILLCODE2 = '" + striLLCode2 + "', ILLCODE3 = '" + striLLCode3 + "', ";
                    SQL = SQL + ComNum.VBLF + " ILLCODE4 = '" + striLLCode4 + "', MEMO = '" + strMemo + "', ";
                    SQL = SQL + ComNum.VBLF + " DRCODE = '" + strDrCode + "', ENTSABUN = " + clsPublic.GnJobSabun + ", VCODE = '" + strVCode + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrRowid + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            select_view();
        }



        private void cboDept1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void cboDr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void cboGubun_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void frmCancerSeriousCase_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            cboGubun.Items.Add("1.암환자");
            cboGubun.Items.Add("2.중증환자");
            cboGubun.SelectedIndex = 0;

            cboSex.Items.Add("M.남자");
            cboSex.Items.Add("F.여자");
            cboSex.SelectedIndex = 0;

            try
            {
                SQL = "";
                SQL = " SELECT DEPTCODE, DEPTNAMEK FROM BAS_CLINICDEPT ";
                SQL = SQL + " WHERE DEPTCODE IN ('MD','GS','OG','PD','OS','NS','NP','EN','OT','UR','DM','DT','PC') ";
                SQL = SQL + " ORDER BY PRINTRANKING ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept1.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + VB.Space(20) + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            cboDept1.SelectedIndex = 0;
            txt_Clear();
            txtPano1.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtAge_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void dtpCanDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void dtpDelDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void dtpFDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtGKiho_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void dtpIDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtiLLCode1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtiLLCode2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtiLLCode3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtiLLCode4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtJbunHo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtMemo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            txtPano.Text = VB.Format(txtPano.Text);
        }

        private void txtPano1_Leave(object sender, EventArgs e)
        {
            txtPano1.Text = Convert.ToInt32(txtPano1.Text).ToString("00000000");
        }

        private void dtpPDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void dtpSDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void cboSex_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtSname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void dtpTDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtVCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void cboDr_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblDrName.Text = VB.Right(cboDr.Text, 20).Trim();
        }

        private void cboDept1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT DRCODE, DRNAME FROM BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DRDEPT1  = '" + VB.Left(cboDept1.Text, 2) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TOUR = 'N' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }
                cboDr.Items.Clear();
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDr.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + VB.Space(20) + dt.Rows[i]["DRNAME"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            cboDr.SelectedIndex = 0;
            lblDept.Text = VB.Right(cboDept1.Text, 20).Trim();
        }
    }
}
