using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Description : Cytology 소견
    /// Author : 이상훈
    /// Create Date : 2017.11.08
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="FrmViewAnat.frm"/>
    public partial class FrmViewAnat : Form
    {
        string SQL;
        DataTable dt = null;
        string SqlErr = "";     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        string strFocusForm = "";
        string strAnatCode = "";
        string strAnatName = "";
        string strGBIO = "";
        bool FrtnVal = false;

        public delegate void SendResultValue(Form frm, FpSpread spd, int Row, string Result);
        public event SendResultValue SendEvent;
        int nRow = -1;
        FpSpread spd = null;

        clsOrdFunction OF = new clsOrdFunction();
        List<string> lstCode = null;

        private FrmViewRemark FrmViewRemarkEvent = null;
        string GstrResultChk = string.Empty;

        public FrmViewAnat()
        {
            InitializeComponent();
        }

        public FrmViewAnat(string sAnatCode, string sAnatName, string sGBIO, bool rtnval = false)
        {
            InitializeComponent();

            strAnatCode = sAnatCode;
            strAnatName = sAnatName;
            strGBIO = sGBIO;
        }


        /// <summary>
        ///  2021-04-22 폼 모달리스용도로 추가
        /// </summary>
        /// <param name="sAnatCode"></param>
        /// <param name="sAnatName"></param>
        /// <param name="sGBIO"></param>
        /// <param name="rtnval"></param>
        /// <param name="nRow"></param>
        public FrmViewAnat(string sAnatCode, string sAnatName, string sGBIO, bool rtnval = false, FpSpread spd = null, int nRow = -1)
        {
            InitializeComponent();

            strAnatCode = sAnatCode;
            strAnatName = sAnatName;
            strGBIO = sGBIO;
            this.spd = spd;
            this.nRow = nRow;
        }

        public FrmViewAnat(string sAnatCode, bool rtnVal = false)
        {
            InitializeComponent();

            strAnatCode = sAnatCode;
            FrtnVal = rtnVal;
        }

        private void FrmViewAnat_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsOrdFunction.GstrResultChk = "";
            panCP.Visible = clsOrdFunction.GstrSetRegYN.Equals("Y"); //약속처방 일때만 보임. 그외 사용XXXXX
            lstCode = new List<string>();

            lblOrder.Text = string.Format("오더코드: {0},  오더명칭: {1}", strAnatCode, strAnatName);

            if (strAnatName != "")
            {
                txtNature.Text = strAnatName.Trim();
            }

            GetData();
        }

        private void GetData()
        {
            try
            {
                SQL = "";
                SQL += " SELECT * FROM KOSMOS_OCS.EXAM_ANATMST                                      \r";
                SQL += "  WHERE Ptno      = '" + clsOrdFunction.Pat.PtNo + "'                       \r";
                SQL += "    AND BDate     = TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')   \r";
                SQL += "    AND OrderCode = '" + strAnatCode.Trim() + "'                            \r";
                SQL += "    AND GbIO      = '" + strGBIO + "'                                       \r";
                SQL += "    AND DeptCode  = '" + (clsOrdFunction.Pat.DeptCode != null ? clsOrdFunction.Pat.DeptCode.Trim() : "") + "'            \r";
                SQL += "    AND ROWNUM    = 1                                                       \r";

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
                        txtNature.Text = dt.Rows[0]["REMARK1"].ToString();
                        txtDiagnosis.Text = dt.Rows[0]["REMARK4"].ToString();
                        txtClinicalHis.Text = dt.Rows[0]["REMARK2"].ToString();
                        txtInformation.Text = dt.Rows[0]["REMARK3"].ToString();
                        clsOrdFunction.GstrResultChk = "1";
                        GstrResultChk = "1";
                    }
                }
                else
                {
                    //2018.06.22 OG 요청 PAP`S(Pap`s Smear(Cervicovaginal)-(1)) 검사 자동입력 skip
                    if (clsOrdFunction.Pat.DeptCode != null && clsOrdFunction.Pat.DeptCode.Trim() == "OG" && clsOrdFunction.GstrAnatCode.Trim() == "PAP`S")
                    {
                        txtNature.Text = "routine cervix";
                        txtClinicalHis.Text = "routine check";
                        txtInformation.Text = "No specific";
                        txtDiagnosis.Text = "No specific";

                        dt.Dispose();
                        dt = null;

                        btnSave_Click(btnSave, new EventArgs());
                        FrtnVal = true;
                        return;
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (txtNature.Text.Trim() == "")
            {
                txtNature.Text = strAnatName.Trim();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strRowId = "";
            string strOK;

            strOK = "NO";
            strOK = clsOrdFunction.fn_SpecialCharCheck(txtNature.Text);
            //for (int i = 0; i < txtNature.Text.Length; i++)
            //{
            //    if ((VB.Asc(VB.Mid(txtNature.Text, i, 1)) >= 65 && VB.Asc(VB.Mid(txtNature.Text, i, 1)) <= 90) ||
            //        (VB.Asc(VB.Mid(txtNature.Text, i, 1)) >= 97 && VB.Asc(VB.Mid(txtNature.Text, i, 1)) <= 122))
            //    {
            //        strOK = "OK";
            //    }
            //}

            if (strOK == "NO")
            {
                txtNature.Focus();
                MessageBox.Show("Nature_Source of Specimen을 정확하게 입력하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; 
            }

            strOK = "NO";
            strOK = clsOrdFunction.fn_SpecialCharCheck(txtClinicalHis.Text);

            if (strOK == "NO")
            {
                txtClinicalHis.Focus();
                MessageBox.Show("Clinical History & Information을 정확하게 입력하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            strOK = "NO";
            strOK = clsOrdFunction.fn_SpecialCharCheck(txtDiagnosis.Text);

            if (strOK == "NO")
            {
                txtDiagnosis.Focus();
                MessageBox.Show("Clinical Diagnosis을 정확하게 입력하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            if (txtNature.Text.Trim().Length < 3)
            {
                MessageBox.Show("Nature_Source of Specimen을 3글자이상 입력하십시오!!!", "소견", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNature.Focus();
                return;
            }

            if (txtClinicalHis.Text.Trim().Length < 5)
            {
                MessageBox.Show("Clinical Information을 5글자이상 입력하십시오!!!", "소견", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtClinicalHis.Focus();
                return;
            }

            if (txtInformation.Text.Trim() == "")
            {
                MessageBox.Show("Infomation on previou Cytology Examination이 공란입니다!!!", "소견", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInformation.Focus();
                return;
            }

            if (txtDiagnosis.Text.Trim() == "")
            {
                MessageBox.Show("Clinical Diagnosis!!!이 공란입니다!!!", "소견", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDiagnosis.Focus();
                return;
            }

            txtNature.Text = txtNature.Text.Replace("'", "`");
            txtClinicalHis.Text = txtClinicalHis.Text.Replace("'", "`");
            txtInformation.Text = txtInformation.Text.Replace("'", "`");
            txtDiagnosis.Text = txtDiagnosis.Text.Replace("'", "`");

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                #region CP가 아닐경우
                if (rdoCP0.Checked)
                {
                    SQL = "";
                    SQL += " SELECT ROWID FROM KOSMOS_OCS.EXAM_ANATMST                                  \r";
                    SQL += "  WHERE Ptno      = '" + clsOrdFunction.Pat.PtNo + "'                       \r";
                    SQL += "    AND BDate     = TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')\r";
                    SQL += "    AND OrderCode = '" + strAnatCode.Trim() + "'                            \r";
                    SQL += "    AND GbIO      = '" + strGBIO + "'                                       \r";
                    SQL += "    AND DeptCode  = '" + clsOrdFunction.Pat.DeptCode.Trim() + "'            \r";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strRowId = dt.Rows[0]["ROWID"].ToString();
                    }
                    dt.Dispose();
                    dt = null;


                    SQL = "";
                    SQL += "   merge into KOSMOS_OCS.EXAM_ANATMST b                                 \r";
                    SQL += "   using dual d                                                         \r";
                    SQL += "      on (b.ROWID = '" + strRowId + "')                                 \r";
                    SQL += "    when matched then                                                   \r";
                    SQL += "  update set                                                            \r";
                    SQL += "         Remark1 = '" + txtNature.Text.Trim() + "'                      \r";
                    SQL += "	   , Remark2 = '" + txtClinicalHis.Text.Trim() + "'                 \r";
                    SQL += "	   , Remark3 = '" + txtInformation.Text.Trim() + "'                 \r";
                    SQL += "	   , Remark4 = '" + txtDiagnosis.Text.Trim() + "'                   \r";
                    SQL += "    when not matched then                                               \r";
                    SQL += "  insert                                                                \r";
                    SQL += "        (Ptno                                                           \r";
                    SQL += "       , BDate                                                          \r";
                    SQL += "       , OrderCode                                                      \r";
                    SQL += "       , GbIO                                                           \r";
                    SQL += "       , Remark1                                                        \r";
                    SQL += "       , Remark2                                                        \r";
                    SQL += "       , Remark3                                                        \r";
                    SQL += "       , Remark4                                                        \r";
                    SQL += "       , DeptCode                                                       \r";
                    SQL += "       , DrCode)                                                        \r";
                    SQL += "  values                                                                \r";
                    SQL += "        ( '" + clsOrdFunction.Pat.PtNo + "'                             \r";
                    SQL += "       , to_date('" + clsOrdFunction.GstrBDate + "', 'yyyy-mm-dd')      \r";
                    SQL += "       , '" + strAnatCode.Trim() + "'                                   \r";
                    SQL += "       , '" + strGBIO + "'                                              \r";
                    SQL += "       , '" + txtNature.Text.Trim() + "'                                \r";
                    SQL += "       , '" + txtClinicalHis.Text.Trim() + "'                           \r";
                    SQL += "       , '" + txtInformation.Text.Trim() + "'                           \r";
                    SQL += "       , '" + txtDiagnosis.Text.Trim() + "'                             \r";
                    SQL += "       , '" + clsOrdFunction.Pat.DeptCode.Trim() + "'                   \r";
                    SQL += "       , '" + VB.Right(clsOrdFunction.Pat.DrCode.Trim(), 4) + "')       \r";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr + " Cytology 의뢰소견 등록시 오류 발생!!!");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        clsOrdFunction.GstrResultChk = "";
                        return;
                    }
                }
                #endregion
                #region CP일경우
                else
                {
                    SQL = "";
                    SQL += " SELECT ROWID FROM KOSMOS_OCS.EXAM_ANATMST_CP                                  \r";
                    SQL += "  WHERE CPCODE    = '" + lstCode[cboCP.SelectedIndex] + "'                     \r";
                    SQL += "    AND OrderCode = '" + strAnatCode.Trim() + "'                               \r";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strRowId = dt.Rows[0]["ROWID"].ToString();
                    }
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL += "   merge into KOSMOS_OCS.EXAM_ANATMST_CP b                              \r";
                    SQL += "   using dual d                                                         \r";
                    SQL += "      on (b.ROWID = '" + strRowId + "')                                 \r";
                    SQL += "    when matched then                                                   \r";
                    SQL += "  update set                                                            \r";
                    SQL += "         Remark1 = '" + txtNature.Text.Trim() + "'                      \r";
                    SQL += "	   , Remark2 = '" + txtClinicalHis.Text.Trim() + "'                 \r";
                    SQL += "	   , Remark3 = '" + txtInformation.Text.Trim() + "'                 \r";
                    SQL += "	   , Remark4 = '" + txtDiagnosis.Text.Trim() + "'                   \r";
                    SQL += "    when not matched then                                               \r";
                    SQL += "  insert                                                                \r";
                    SQL += "        (CPCODE                                                         \r";
                    SQL += "       , OrderCode                                                      \r";
                    SQL += "       , GbIO                                                           \r";
                    SQL += "       , Remark1                                                        \r";
                    SQL += "       , Remark2                                                        \r";
                    SQL += "       , Remark3                                                        \r";
                    SQL += "       , Remark4                                                        \r";
                    SQL += "       , DeptCode                                                       \r";
                    SQL += "       , DrCode)                                                        \r";
                    SQL += "  values                                                                \r";
                    SQL += "        ( '" + lstCode[cboCP.SelectedIndex] + "'                        \r";
                    SQL += "       , '" + strAnatCode.Trim() + "'                                   \r";
                    SQL += "       , '" + strGBIO + "'                                              \r";
                    SQL += "       , '" + txtNature.Text.Trim() + "'                                \r";
                    SQL += "       , '" + txtClinicalHis.Text.Trim() + "'                           \r";
                    SQL += "       , '" + txtInformation.Text.Trim() + "'                           \r";
                    SQL += "       , '" + txtDiagnosis.Text.Trim() + "'                             \r";
                    SQL += "       , '" + clsType.User.DeptCode + "'                                \r";
                    SQL += "       , '" + clsType.User.DrCode + "')                                 \r";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr + " Cytology 의뢰소견 등록시 오류 발생!!!");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        clsOrdFunction.GstrResultChk = "";
                        return;
                    }
                }
                #endregion


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                clsOrdFunction.GstrResultChk = "1";
                GstrResultChk = "1";

                this.Close();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void fn_Color_Set()
        {
            txtNature.BackColor = Color.White;
            txtDiagnosis.BackColor = Color.White;
            txtClinicalHis.BackColor = Color.White;
            txtInformation.BackColor = Color.White;
        }

        private void txtNature_Click(object sender, EventArgs e)
        {
            fn_Color_Set();

            txtNature.BackColor = Color.LavenderBlush;
            strFocusForm = "Remark1";
        }

        private void txtDiagnosis_Click(object sender, EventArgs e)
        {
            fn_Color_Set();

            txtDiagnosis.BackColor = Color.LavenderBlush;
            strFocusForm = "Remark2";
        }

        private void txtClinicalHis_Click(object sender, EventArgs e)
        {
            fn_Color_Set();

            txtClinicalHis.BackColor = Color.LavenderBlush;
            strFocusForm = "Remark3";
        }

        private void txtInformation_Click(object sender, EventArgs e)
        {
            fn_Color_Set();

            txtInformation.BackColor = Color.LavenderBlush;
            strFocusForm = "Remark4";
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            FrmViewRemarkEvent = new FrmViewRemark(clsOrdFunction.GstrDrCode.Trim());
            FrmViewRemarkEvent.SendEvent += F_SendEvent;
            FrmViewRemarkEvent.rEventClosed += F_rEventClosed;
            FrmViewRemarkEvent.StartPosition = FormStartPosition.CenterParent;
            FrmViewRemarkEvent.ShowDialog(this);
        }

        private void F_rEventClosed()
        {
            OF.fn_ClearMemory(FrmViewRemarkEvent);
        }

        private void F_SendEvent(string SendRetValue)
        {
            if (SendRetValue != null)
            {
                if (SendRetValue.Trim() != "")
                {
                    if (strFocusForm == "Remark1")
                    {
                        txtNature.Text = SendRetValue.Trim();
                    }
                    else if (strFocusForm == "Remark2")
                    {
                        txtDiagnosis.Text = SendRetValue.Trim();
                    }
                    else if (strFocusForm == "Remark3")
                    {
                        txtClinicalHis.Text = SendRetValue.Trim();
                    }
                    else if (strFocusForm == "Remark4")
                    {
                        txtInformation.Text = SendRetValue.Trim();
                    }
                }
            }
        }

        private void FrmViewAnat_FormClosed(object sender, FormClosedEventArgs e)
        {
            SendEvent?.Invoke(this, spd, this.nRow, GstrResultChk);
        }


        #region CP용
        private void rdoCP1_CheckedChanged(object sender, EventArgs e)
        {
            GetDataCpCode();
        }

        private void GetDataCpCode()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboCP.Items.Clear();
            lstCode.Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "    BASCD, BASNAME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_BASCD";
                SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = 'CP관리' ";
                SQL = SQL + ComNum.VBLF + "  AND GRPCD  = 'CP코드관리' ";
                SQL = SQL + ComNum.VBLF + "  AND BASNAME1  = 'IPD' ";
                SQL = SQL + ComNum.VBLF + "  AND VFLAG1  = '" + clsType.User.DeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BASCD";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboCP.Items.Add(dt.Rows[i]["BASNAME"].ToString().Trim());
                        lstCode.Add(dt.Rows[i]["BASCD"].ToString().Trim());
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

                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }
    }
}
