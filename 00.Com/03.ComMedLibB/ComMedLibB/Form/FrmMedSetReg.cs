using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMedLibB
{
    public partial class FrmMedSetReg : Form
    {
        string SQL;
        DataTable dt = null;
        string SqlErr = "";     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        string FstrOrderGubun = "";
        int nIndexSet;

        public delegate void SetOrderReg_Click(string PrmName, string DeptDr, string GbOrder, string strDeptDrGubun);
        public static event SetOrderReg_Click SetOrderReg;
        

        public FrmMedSetReg()
        {
            InitializeComponent();
        }

        private void FrmMedSetReg_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            //{
            //    this.Close();
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            this.Location = new Point(50, 50);
            clsOrdFunction.GStrDeptDR = "A";
            fn_Control_Set();            
            txtSetName.Focus();
        }

        void fn_Control_Set()
        {
            grbCP.Enabled = false;
            txtSetName.Text = "";
            txtSetName.Focus();

            switch (FstrOrderGubun)
            {
                case "M":
                    rdoGbOrder2.Checked = true;
                    break;
                case "F":
                    rdoGbOrder3.Checked = true;
                    break;
                case "T":
                    rdoGbOrder4.Checked = true;
                    break;
                case "P":
                    rdoGbOrder5.Checked = true;
                    break;
                case "E":
                    rdoGbOrder6.Checked = true;
                    break;
                case "C":
                    rdoGbOrder7.Checked = true;
                    if (clsPublic.GstrCP처방Chk == "OK")
                    {
                        grbCP.Enabled = true;
                    }
                    break;
                default:
                    rdoGbOrder1.Checked = true;
                    break;
            }

            btnPersonal.Enabled = true;
            btnDept.Enabled = true;

            if (clsOrdFunction.GStrDeptDR == " ")
            {
                btnPersonal.Enabled = false;
                btnDept.Enabled = false;
            }
            else if (clsOrdFunction.GStrDeptDR == "A")
            {
                btnPersonal_Click(btnPersonal, new EventArgs());
            }
            else
            {
                btnDept_Click(btnDept, new EventArgs());
            }            
            
            rdoGbOrder6.Text = clsPublic.GstrWardCode + "간호사처방";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += " SELECT CPCODE , CPNAME                             \r";
                SQL += "   FROM ADMIN.OCS_CP_CODE                      \r";
                SQL += "  WHERE (DeptDr ='" + clsType.User.Sabun + "'       \r";
                SQL += "     OR DeptDr ='" + clsPublic.GstrDeptCode + "')   \r";
                SQL += "  ORDER BY CPCODE                                   \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                cboCP.Items.Clear();
                cboCP.Items.Add("");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboCP.Items.Add(dt.Rows[i]["CPCODE"].ToString().Trim() + "." + dt.Rows[i]["CPNAME"].ToString().Trim());
                }

                cboCP.SelectedIndex = -1;

                dt.Dispose();
                dt = null;

                cboCpDay.Items.Clear();
                cboCpDay.Items.Add(" ");
                for (int i = 0; i < 11; i++)
                {
                    cboCpDay.Items.Add(i);
                }
                cboCpDay.SelectedIndex = 0;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
        
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPersonal_Click(object sender, EventArgs e)
        {
            BtnColor_Clear();
            btnPersonal.BackColor = Color.RoyalBlue;
            btnPersonal.ForeColor = Color.White;

            pnlGbOrd.Enabled = true;

            clsOrdFunction.GstrGbPrm = "A";

            SetOrder_Read(1, clsType.User.Sabun.Trim());
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            BtnColor_Clear();
            btnDept.BackColor = Color.RoyalBlue;
            btnDept.ForeColor = Color.White;

            pnlGbOrd.Enabled = true;

            clsOrdFunction.GstrGbPrm = "B";

            SetOrder_Read(2, clsPublic.GstrDeptCode.Trim());
        }

        private void btnEr_Click(object sender, EventArgs e)
        {
            BtnColor_Clear();
            btnEr.BackColor = Color.RoyalBlue;
            btnEr.ForeColor = Color.White;

            pnlGbOrd.Enabled = true;

            clsOrdFunction.GstrGbPrm = "C";

            SetOrder_Read(5, "EM");
        }


        void BtnColor_Clear()
        {
            btnPersonal.BackColor = Color.White;
            btnPersonal.ForeColor = Color.Black;

            btnDept.BackColor = Color.White;
            btnDept.ForeColor = Color.Black;

            btnEr.BackColor = Color.White;
            btnEr.ForeColor = Color.Black;
        }

        void SetOrder_Read(int sGbn, string sGubun)
        {
            string strDeptDr = "";

            strDeptDr = sGubun;

            LstName.Items.Clear();

            if (VB.Left(sGubun.Trim(), 2) == "IU")
            {
                strDeptDr = VB.Left(strDeptDr, 5);
            }

            try
            {
                SQL = "";
                SQL += " SELECT distinct PRMNAME                    \r";
                SQL += "   FROM ADMIN.OCS_OPRM                 \r";
                SQL += "  WHERE DeptDr = '" + strDeptDr.Trim() + "' \r";
                
                if (rdoGbOrder1.Checked == true)
                {
                    SQL += "    AND GbOrder = ' '                   \r";
                }
                else if (rdoGbOrder2.Checked == true)
                {
                    SQL += "    AND GbOrder = 'M'                   \r";
                }
                else if (rdoGbOrder3.Checked == true)
                {
                    SQL += "    AND GbOrder = 'F'                   \r";
                }
                else if (rdoGbOrder4.Checked == true)
                {
                    SQL += "    AND GbOrder = 'T'                   \r";
                }
                else if (rdoGbOrder5.Checked == true)
                {
                    SQL += "    AND GbOrder = 'P'                   \r";
                }
                else if (rdoGbOrder6.Checked == true)
                {
                    SQL += "    AND GbOrder = 'N'                   \r";
                }
                else if (rdoGbOrder7.Checked == true)
                {
                    SQL += "    AND GbOrder = 'E'                   \r";
                }
                else if (rdoGbOrder8.Checked == true)
                {
                    SQL += "    AND GbOrder = 'C'                   \r";
                }
                SQL += "  GROUP BY PRMNAME                          \r";
                SQL += "  ORDER BY PRMNAME                          \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        LstName.Items.Add(dt.Rows[i]["PRMNAME"].ToString());
                    }
                }

                if (LstName.Items.Count > 0)
                {
                    LstName.SelectedIndex = 0;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            
            txtSetName.Focus();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            //string strOK = "";
            string strDeptDr = "";
            string strGbOrder = "";

            clsOrdFunction.GstrCPCode = "";
            clsOrdFunction.GstrCPName = "";
            clsOrdFunction.GnCPDay = 0;
            strGbOrder = " ";

            if (txtSetName.Text.Trim() == "")
            {
                MessageBox.Show("약속처방명을 입력해 주세요!!!    ");
                txtSetName.Focus();
                return;
            }

            if (rdoGbOrder1.Checked == true)
            {
                strGbOrder = " ";
            }
            else if (rdoGbOrder2.Checked == true)
            {
                strGbOrder = "M";
            }
            else if (rdoGbOrder3.Checked == true)
            {
                strGbOrder = "F";
            }
            else if (rdoGbOrder4.Checked == true)
            {
                strGbOrder = "T";
            }
            else if (rdoGbOrder5.Checked == true)
            {
                strGbOrder = "P";
            }
            else if (rdoGbOrder6.Checked == true)
            {
                strGbOrder = "N";
            }
            else if (rdoGbOrder7.Checked == true)
            {
                strGbOrder = "E";
            }
            else if (rdoGbOrder8.Checked == true)
            {
                strGbOrder = "C";
            }

            clsOrdFunction.GStrDeptDR = " ";
            if (btnPersonal.BackColor == Color.RoyalBlue)
            {
                clsOrdFunction.GStrDeptDR = "A";
            }
            else if (btnDept.BackColor == Color.RoyalBlue)
            {
                clsOrdFunction.GStrDeptDR = "B";
            }
            else if (btnEr.BackColor == Color.RoyalBlue)
            {
                clsOrdFunction.GStrDeptDR = "C";
            }

            if (rdoGbOrder6.Checked == true)
            {
                //if (cboCP.Text.Trim() == "")
                //{
                //    strOK = "CP";
                //}
                //else
                //{
                    clsOrdFunction.GstrCPCode = VB.Pstr(cboCP.Text.Trim(), ".", 1);
                    clsOrdFunction.GstrCPCode = VB.Pstr(cboCP.Text.Trim(), ".", 1);
                    clsOrdFunction.GnCPDay = int.Parse(cboCpDay.Text);
                //}
            }
            
            if (clsOrdFunction.GstrGbPrm == "A")
            {
                strDeptDr = clsType.User.Sabun.Trim();
            }
            else if (clsOrdFunction.GstrGbPrm == "B")
            {
                strDeptDr = clsPublic.GstrDeptCode.Trim();
            }
            else if (clsOrdFunction.GstrGbPrm == "C")
            {
                strDeptDr = clsType.User.Sabun.Trim();
                strGbOrder = "E";
            }

            if (rdoGbOrder1.Checked == true)
            {
                strGbOrder = " ";
            }
            else if (rdoGbOrder2.Checked == true)
            {
                strGbOrder = "M";
            }
            if (rdoGbOrder3.Checked == true)
            {
                strGbOrder = "F";
            }
            if (rdoGbOrder4.Checked == true)
            {
                strGbOrder = "T";
            }

            if (SetOrder_Check(strDeptDr, txtSetName.Text.Trim()) == "OK")
            {
                // 신규
                SetOrderReg(txtSetName.Text.Trim(), strDeptDr, strGbOrder, clsOrdFunction.GstrGbPrm);
            }
            else
            {
                // 수정
                if (MessageBox.Show("이미 같은 명칭의 약속처방이 존재 합니다. 삭제 후 저장 하시겠습니까?", "약속처방명 중복확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
                else
                {
                    fn_SetDelete(txtSetName.Text.Trim(), strDeptDr);
                    SetOrderReg(txtSetName.Text.Trim(), strDeptDr, strGbOrder, clsOrdFunction.GstrGbPrm);
                }
            }

            if (btnPersonal.BackColor == Color.RoyalBlue)
            {
                btnPersonal_Click(btnPersonal, e);
            }
            else if (btnDept.BackColor == Color.RoyalBlue)
            {
                btnDept_Click(btnDept, e);
            }
            else if (btnEr.BackColor == Color.RoyalBlue)
            {
                btnEr_Click(btnEr, e);
            }
        }

        void fn_SetDelete(string strSetName, string strDeptDr)
        {
            //clsDB.setBeginTran(clsDB.DbCon);

            //try
            //{
                SQL = "";
                SQL += " DELETE                                         \r";
                SQL += "   FROM ADMIN.OCS_OPRM                     \r";
                SQL += "  WHERE DeptDr = '" + strDeptDr.Trim() + "'     \r";
                SQL += "    AND PRMNAME = '" + strSetName.Trim() + "'   \r";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr + " 약속처방 삭제중 오류가 발생되었습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                        
                    return;
                }

                //clsDB.setCommitTran(clsDB.DbCon);
            //}
            //catch (Exception ex)
            //{
            //    clsDB.setRollbackTran(clsDB.DbCon);
            //    ComFunc.MsgBox(ex.Message);
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //}
        }

        string SetOrder_Check(string sGubun, string strSetName)
        {
            string rtnVal = "NO";

            try
            {
                SQL = "";
                SQL += " SELECT distinct PRMNAME                        \r";
                SQL += "   FROM ADMIN.OCS_OPRM                     \r";
                SQL += "  WHERE DeptDr = '" + sGubun.Trim() + "'        \r";
                SQL += "    AND PRMNAME = '" + strSetName.Trim() + "'   \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    rtnVal = "OK";
                }
                else
                {
                    rtnVal = "NO";
                }

                if (LstName.Items.Count > 0)
                {
                    LstName.SelectedIndex = 0;
                }

                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void rdoGbOrder8_Click(object sender, EventArgs e)
        {
            if (rdoGbOrder8.Checked == true)
            {
                if (clsOrdFunction.GstrCPOrderChk == "OK")
                {
                    grbCP.Enabled = true;
                }
            }
            
            if (btnPersonal.BackColor == Color.RoyalBlue && clsOrdFunction.GstrCPOrderChk == "OK")
            {
                grbCP.Enabled = true;
            }

            fn_GbOrder_Click();
        }

        private void LstName_Click(object sender, EventArgs e)
        {
            if (LstName.SelectedIndex >= 0)
            {
                txtSetName.Text = "";
                txtSetName.Text = LstName.Items[LstName.SelectedIndex].ToString().Trim();
            }
        }

        private void rdoGbOrder1_Click(object sender, EventArgs e)
        {
            fn_GbOrder_Click();
        }

        void fn_GbOrder_Click()
        {
            if (btnPersonal.BackColor == Color.RoyalBlue)
            {
                btnPersonal_Click(btnPersonal, null);
            }
            else if (btnDept.BackColor == Color.RoyalBlue)
            {
                btnDept_Click(btnDept, null);
            }
            else if (btnEr.BackColor == Color.RoyalBlue)
            {
                btnEr_Click(btnEr, null);
            }
        }

        private void rdoGbOrder2_Click(object sender, EventArgs e)
        {
            fn_GbOrder_Click();
        }

        private void rdoGbOrder3_Click(object sender, EventArgs e)
        {
            fn_GbOrder_Click();
        }

        private void rdoGbOrder4_Click(object sender, EventArgs e)
        {
            fn_GbOrder_Click();
        }

        private void rdoGbOrder5_Click(object sender, EventArgs e)
        {
            fn_GbOrder_Click();
        }

        private void rdoGbOrder6_Click(object sender, EventArgs e)
        {
            fn_GbOrder_Click();
        }

        private void rdoGbOrder7_Click(object sender, EventArgs e)
        {
            fn_GbOrder_Click();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtSetName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
