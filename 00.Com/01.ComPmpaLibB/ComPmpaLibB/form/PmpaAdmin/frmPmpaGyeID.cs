using ComBase;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결



namespace ComPmpaLibB
{
    public partial class frmPmpaGyeID : Form
    {
        ComFunc CF = new ComFunc();

        string FstrROWID = string.Empty;

        public frmPmpaGyeID()
        {
            InitializeComponent();
            SetEvent();
        }

        private void SetEvent()
        {
            #region DateTimePicker Null 처리
            this.dtpFDate.ValueChanged += new EventHandler(CF.eDtpFormatSet);
            this.dtpTDate.ValueChanged += new EventHandler(CF.eDtpFormatSet);
            #endregion
        }

        private void Set_Combo_GelCode()
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0;

            try
            {
                cboGel.Items.Clear();
                cboLGel.Items.Clear();
                cboLGel.Items.Add("9999.전체");

                SQL = "";
                SQL += ComNum.VBLF + " SELECT MiaCode GelCode, MiaName                   ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA              ";
                SQL += ComNum.VBLF + "  WHERE MiaCode >= 'H001' AND MiaCode <= 'H999'    ";
                SQL += ComNum.VBLF + "  ORDER BY MiaCode                                 ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        cboGel.Items.Add(Dt.Rows[i]["GelCode"].ToString().Trim() + "." + Dt.Rows[i]["MiaName"].ToString().Trim());  
                        cboLGel.Items.Add(Dt.Rows[i]["GelCode"].ToString().Trim() + "." + Dt.Rows[i]["MiaName"].ToString().Trim());
                    }
                }

                cboLGel.SelectedIndex = 0;

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void frmPmpaGyeID_Load(object sender, EventArgs e)
        {
            clsVbfunc.SetComboDept(clsDB.DbCon,cboDept1, "2", 2);
            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept2, "2", 2);
            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept3, "2", 2);

            CF.Combo_BCode_SET(clsDB.DbCon, cboBi1, "BAS_환자종류", true, 2, "N");
            CF.Combo_BCode_SET(clsDB.DbCon, cboBi2, "BAS_환자종류", true, 2, "N");
            CF.Combo_BCode_SET(clsDB.DbCon, cboBi3, "BAS_환자종류", true, 2, "N");

            lstbox.Items.Clear();
            Set_Combo_GelCode();

            Screen_Clear();
           // Read_GelList();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Screen_Clear()
        {
            txtPano.Text = "";      lblSName.Text = "";
            lblJumin.Text = "";     cboGel.Text = "";
            cboDept1.Text = "";     cboDept2.Text = ""; cboDept3.Text = "";
            cboBi1.Text = "";       cboBi2.Text = "";   cboBi3.Text = "";
            CF.dtpClear(dtpFDate);
            CF.dtpClear(dtpTDate);
            txtRemark.Text = "";
            lblLen.Text = "0/500";
            lblDeptName.Text = "";
            lblBi.Text = "";
            
            FstrROWID = "";
            
            txtPano.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
            panJob.Enabled = false;
            Set_Combo_GelCode();
        }

        private void Read_GelList()
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, nRead = 0;
            string strGelCode = string.Empty;
            string strData = string.Empty;

            try
            {
                lstbox.Items.Clear();

                strGelCode = VB.Left(cboLGel.Text, 4);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT GelCode,Pano,Sname,DeptCode1,DeptCode2,Bi1,Bi2             ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GYEPANO                         ";
                if (strGelCode != "9999")
                { 
                    SQL += ComNum.VBLF + "  WHERE GelCode='" + strGelCode + "'  ";
                }
                SQL += ComNum.VBLF + "  ORDER BY Sname,Pano                                              ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = Dt.Rows.Count;
                    
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strData = "";
                        strData += Dt.Rows[i]["Pano"].ToString().Trim() + "  ";
                        strData += VB.Left(Dt.Rows[i]["Sname"].ToString().Trim() + VB.Space(11), 12);
                        strData += Dt.Rows[i]["GelCode"].ToString().Trim() + "  ";
                        strData += Dt.Rows[i]["DeptCode1"].ToString().Trim() + "  ";
                        strData += Dt.Rows[i]["Bi1"].ToString().Trim();
                        lstbox.Items.Add(strData);                        
                    }
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Read_GelList();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            txtPano.Focus();
        }

        private void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtPano.Text.Trim() == "")
                {
                    return;
                }
                txtPano.Text = VB.Format(VB.Val(txtPano.Text), "00000000");
                Screen_Display();
                cboDept1.Focus();
               // SendKeys.Send("{TAB}");
            }
        }

        private void Screen_Display()
        {
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, nRead = 0;
            string strGelCode = string.Empty;
            
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.Pano,b.Sname,b.Jumin1,b.Jumin2,b.Jumin3,a.GelCode,   ";
                SQL += ComNum.VBLF + "        TO_CHAR(a.FromDate,'YYYY-MM-DD') FrDate,               ";
                SQL += ComNum.VBLF + "        TO_CHAR(a.ToDate,'YYYY-MM-DD') ToDate,                 ";
                SQL += ComNum.VBLF + "        a.DeptCode1,a.DeptCode2,a.DeptCode3,a.Bi1,a.Bi2,a.Bi3, ";
                SQL += ComNum.VBLF + "        a.Remark,a.ROWID                                       ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GYEPANO a,                  ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_Patient b                    ";
                SQL += ComNum.VBLF + "  WHERE a.Pano = '" + txtPano.Text + "'                        ";
                SQL += ComNum.VBLF + "    AND a.Pano = b.Pano(+)                                     ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = Dt.Rows.Count;

                if (nRead > 0)
                {
                    lblSName.Text = Dt.Rows[0]["Sname"].ToString().Trim();
                    //주민암호화
                    lblJumin.Text = Dt.Rows[0]["Jumin1"].ToString().Trim() + "-" + clsAES.DeAES(Dt.Rows[0]["Jumin3"].ToString().Trim());
                    strGelCode = Dt.Rows[0]["GelCode"].ToString().Trim();
                    for (i = 0; i < cboGel.Items.Count; i++)
                    {
                        if (VB.Left(cboGel.Items[i].ToString(), 4) == strGelCode)
                        { 
                            cboGel.SelectedIndex = i;
                            break;
                        }
                    }
                    cboDept1.Text = Dt.Rows[0]["DeptCode1"].ToString().Trim(); 
                    cboDept2.Text = Dt.Rows[0]["DeptCode2"].ToString().Trim(); 
                    cboDept3.Text = Dt.Rows[0]["DeptCode3"].ToString().Trim();     
                    cboBi1.Text = Dt.Rows[0]["Bi1"].ToString().Trim(); 
                    cboBi2.Text = Dt.Rows[0]["Bi2"].ToString().Trim(); 
                    cboBi3.Text = Dt.Rows[0]["Bi3"].ToString().Trim();       
                    dtpFDate.Text = Dt.Rows[0]["FrDate"].ToString().Trim();
                    dtpTDate.Text = Dt.Rows[0]["ToDate"].ToString().Trim();
                    txtRemark.Text = Dt.Rows[0]["Remark"].ToString().Trim(); 
                        
                    FstrROWID = Dt.Rows[0]["ROWID"].ToString().Trim();
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT Sname,Jumin1,Jumin2,Jumin3        ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                    SQL += ComNum.VBLF + "  WHERE Pano='" + txtPano.Text + "'       ";
                    SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (Dt2.Rows.Count == 0)
                    {
                        Dt.Dispose();
                        Dt = null;
                        Dt2.Dispose();
                        Dt2 = null;
                        ComFunc.MsgBox("등록번호가 오류입니다.", "오류");
                        return;
                    }

                    lblSName.Text = Dt2.Rows[0]["Sname"].ToString().Trim();
                    //주민암호화
                    lblJumin.Text = Dt2.Rows[0]["Jumin1"].ToString().Trim() + "-" + clsAES.DeAES(Dt2.Rows[0]["Jumin3"].ToString().Trim());
                    dtpFDate.Text = clsPublic.GstrSysDate;
                    dtpTDate.Text = CF.DATE_ADD(clsDB.DbCon,clsPublic.GstrSysDate, 30);
                    
                    Dt2.Dispose();
                    Dt2 = null;
                }

                Dt.Dispose();
                Dt = null;

                panJob.Enabled = true;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnDelete.Enabled = true;
                if (FstrROWID == "")
                    btnDelete.Enabled = false;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }
                if (Dt2 != null)
                {
                    Dt2.Dispose();
                    Dt2 = null;
                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void cboDept1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblDeptName.Text = CF.READ_DEPTNAMEK(clsDB.DbCon,cboDept1.Text);
        }

        private void cboDept2_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblDeptName.Text = CF.READ_DEPTNAMEK(clsDB.DbCon, cboDept2.Text);
        }

        private void cboDept3_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblDeptName.Text = CF.READ_DEPTNAMEK(clsDB.DbCon, cboDept3.Text);
        }

        private void cboBi3_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblBi.Text = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_환자종류", cboBi3.Text);
        }

        private void cboBi2_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblBi.Text = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_환자종류", cboBi2.Text);
        }

        private void cboBi1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblBi.Text = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_환자종류", cboBi1.Text);
        }

        private void txtRemark_TextChanged(object sender, EventArgs e)
        {
            lblLen.Text = txtRemark.Text.Length + "/500";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            if (ComFunc.MsgBoxQ("정말로 삭제를 하시겠습니까?", "작업여부", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                return;

            try
            {
                SQL = "DELETE " + ComNum.DB_PMPA + "MISU_GYEPANO WHERE ROWID='" + FstrROWID + "' ";
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
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;

                Screen_Clear();

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (dtpFDate.Text == "") { ComFunc.MsgBox("시작일자가 공란입니다.", "오류"); dtpFDate.Focus(); return; }
            if (cboGel.Text == "") { ComFunc.MsgBox("계약처코드가 공란입니다.", "오류"); cboGel.Focus(); return; }
            if (cboDept1.Text == "") { ComFunc.MsgBox("진료과코드가 공란입니다.", "오류"); cboDept1.Focus(); return; }
            if (cboBi1.Text == "") { ComFunc.MsgBox("환자종류가 공란입니다.", "오류"); cboBi1.Focus(); return; }
            if (dtpTDate.Text == "") { ComFunc.MsgBox("종결일자가 공란입니다.", "오류"); dtpTDate.Focus(); return; }
            if (string.Compare(dtpTDate.Text, dtpFDate.Text) < 0) { ComFunc.MsgBox("종결일자가 시작일자보다 적음", "오류"); dtpFDate.Focus(); return; }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            
            try
            {
              

                if (FstrROWID.Equals(""))
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "MISU_GYEPANO                                   ";
                    SQL += ComNum.VBLF + "        (Pano,Sname,GelCode,FromDate,ToDate,                                      ";
                    SQL += ComNum.VBLF + "        DeptCode1,DeptCode2,DeptCode3,Bi1,Bi2,Bi3,Remark,EntDate,EntSabun)        ";
                    SQL += ComNum.VBLF + " VALUES                                                                           ";
                    SQL += ComNum.VBLF + "        ('" + txtPano.Text + "','" + lblSName.Text + "',                          ";
                    SQL += ComNum.VBLF + "        '" + VB.Left(cboGel.Text, 4) + "',                                        ";
                    SQL += ComNum.VBLF + "        TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD'),                            ";
                    SQL += ComNum.VBLF + "        TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD'),                            ";
                    SQL += ComNum.VBLF + "        '" + cboDept1.Text + "','" + cboDept2.Text + "','" + cboDept3.Text + "',  ";
                    SQL += ComNum.VBLF + "        '" + cboBi1.Text + "','" + cboBi2.Text + "','" + cboBi3.Text + "',        ";
                    SQL += ComNum.VBLF + "        '" + txtRemark.Text + "',SYSDATE," + clsType.User.IdNumber + ")           ";
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "MISU_GYEPANO                        ";
                    SQL += ComNum.VBLF + "    SET Sname = '" + lblSName.Text + "',                          ";
                    SQL += ComNum.VBLF + "        GelCode = '" + VB.Left(cboGel.Text, 4) + "',              ";
                    SQL += ComNum.VBLF + "        FromDate = TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "        ToDate = TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD'),   ";
                    SQL += ComNum.VBLF + "        DeptCode1 = '" + cboDept1.Text + "',                      ";
                    SQL += ComNum.VBLF + "        DeptCode2 = '" + cboDept2.Text + "',                      ";
                    SQL += ComNum.VBLF + "        DeptCode3 = '" + cboDept3.Text + "',                      ";
                    SQL += ComNum.VBLF + "        Bi1 = '" + cboBi1.Text + "',                              ";
                    SQL += ComNum.VBLF + "        Bi2 = '" + cboBi2.Text + "',                              ";
                    SQL += ComNum.VBLF + "        Bi3 = '" + cboBi3.Text + "',                              ";
                    SQL += ComNum.VBLF + "        Remark = '" + txtRemark.Text + "',                        ";
                    SQL += ComNum.VBLF + "        EntDate = SYSDATE,                                        ";
                    SQL += ComNum.VBLF + "        EntSabun = " + clsType.User.IdNumber + "                  ";
                    SQL += ComNum.VBLF + "  WHERE ROWID = '" + FstrROWID + "'                               ";
                }
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
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                Screen_Clear();
                txtPano.Focus();                
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

        private void lstbox_ItemDoubleClick(object sender, MouseEventArgs e)
        {
            if (txtPano.Enabled == false) { Screen_Clear(); }
            
            txtPano.Text = VB.Left(lstbox.SelectedItem.ToString(), 8);
            Screen_Display();
        }

        private void cboGel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            SendKeys.Send("{TAB}");
        }

        private void cboDept1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                SendKeys.Send("{TAB}");
        }

        private void cboDept2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                SendKeys.Send("{TAB}");
        }

        private void cboBi1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                SendKeys.Send("{TAB}");
        }

        private void cboBi2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                SendKeys.Send("{TAB}");
        }

        private void cboDept3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                SendKeys.Send("{TAB}");
        }
    }
}
