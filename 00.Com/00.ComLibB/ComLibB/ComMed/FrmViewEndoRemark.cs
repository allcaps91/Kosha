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
    /// Description : 내시경소견
    /// Author : 이상훈
    /// Create Date : 2017.11.10
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="FrmViewEndoRemark.frm"/>
    public partial class FrmViewEndoRemark : Form
    {
        string strOrdercode;

        string SQL;
        DataTable dt = null;
        string SqlErr = "";     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        string strFocusForm;

        private FrmViewRemark FrmViewRemarkEvent = null;

        int nRow = -1;
        FpSpread spd = null;
        public delegate void SendResultValue(Form frm, FpSpread spd, int Row, string Result);
        public event SendResultValue SendEvent;

        List<string> lstCode = null;

        public FrmViewEndoRemark()
        {
            InitializeComponent();
        }

        public FrmViewEndoRemark(string sOrderCode)
        {
            InitializeComponent();

            strOrdercode = sOrderCode;
        }

        public FrmViewEndoRemark(string sOrderCode, FpSpread spd, int nRow)
        {
            InitializeComponent();

            strOrdercode = sOrderCode;
            this.spd = spd;
            this.nRow = nRow;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmViewEndoRemark_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            clsOrdFunction.GstrResultChk = "";
            lstCode = new List<string>();
            grpCP.Visible = clsOrdFunction.GstrSetRegYN.Equals("Y"); //약속처방 일때만 보임. 그외 사용XXXXX

            cboSelect.Items.Add("A. 전  체");
            cboSelect.Items.Add("C.Chief Complaints     ");
            cboSelect.Items.Add("D. Clinical Diagnosis   ");

            try
            {
                SQL = "";
                SQL += " SELECT * FROM  ADMIN.ENDO_REMARK                                          \r";
                SQL += "  WHERE Ptno      = '" + clsOrdFunction.Pat.PtNo + "'                           \r";
                SQL += "    AND JDate     = TO_DATE('" + clsOrdFunction.GstrBDate + "', 'YYYY-MM-DD')   \r";
                SQL += "    AND OrderCode = '" + strOrdercode + "'                                      \r";
                SQL += "    AND ROWNUM    = 1                                                           \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                        rtxtRemark1.Text = dt.Rows[0]["REMARKC"].ToString();
                        rtxtRemark2.Text = dt.Rows[0]["REMARKD"].ToString();

                        clsOrdFunction.GstrResultChk = "1";
                    //}
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            rtxtRemark1.Focus();
            rtxtRemark1.Select();
            //btnSave.Enabled = false;
        }

        private void cboSelect_Click(object sender, EventArgs e)
        {
            cboSRemark.Items.Clear();

            try
            {
                SQL = "";
                SQL += " SELECT RemarkName                                          \r";
                SQL += "   FROM ADMIN.ENDO_SREMARK                             \r";
                SQL += "  WHERE GbJob = '" + ComFunc.LeftH(cboSelect.Text, 1) + "'  \r";
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
                        cboSRemark.Items.Add(dt.Rows[i]["REMARKNAME"].ToString().Trim());
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
        }

        private void cboSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {

            }
        }

        private void cboSRemark_Click(object sender, EventArgs e)
        {
            try
            {
                SQL = "";
                SQL += " SELECT * FROM ADMIN.ENDO_SREMARK                      \r";
                SQL += "  WHERE GbJob = '" + ComFunc.LeftH(cboSelect.Text, 1) + "'  \r";
                SQL += "    AND RemarkName = '" + cboSRemark.Text.Trim() + "'       \r";
                SQL += "    AND ROWNUM = 1                                          \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    switch (ComFunc.LeftH(cboSelect.Text, 1))
                    {
                        case "A":
                            rtxtRemark1.Text = dt.Rows[0]["REMARKC"].ToString().Trim();
                            rtxtRemark2.Text = dt.Rows[0]["REMARKD"].ToString().Trim();
                            break;
                        case "c":
                            rtxtRemark1.Text = dt.Rows[0]["REMARKC"].ToString().Trim();
                            break;
                        case "D":
                            rtxtRemark2.Text = dt.Rows[0]["REMARKD"].ToString().Trim();
                            break;
                        default:
                            break;
                    }
                }
                dt.Dispose();
                dt = null;
                btnSave.Enabled = true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void cboSRemark_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strOK = "";

            if (rtxtRemark1.Text.Trim() == "")
            {
                rtxtRemark1.Focus();
                return;
            }

            if (rtxtRemark2.Text.Trim() == "")
            {
                rtxtRemark2.Focus();
                return;
            }

            strOK = "NO";
            strOK = clsOrdFunction.fn_SpecialCharCheck(rtxtRemark1.Text);

            if (strOK == "NO")
            {
                rtxtRemark1.Focus();
                MessageBox.Show("Chiep Complaints을 영문으로 등록주세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            strOK = "NO";
            strOK = clsOrdFunction.fn_SpecialCharCheck(rtxtRemark1.Text);

            if (strOK == "NO")
            {
                rtxtRemark1.Focus();
                MessageBox.Show("Nature_Source of Specimen을 정확하게 입력하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (rtxtRemark1.Text.Trim() == "" && rtxtRemark2.Text.Trim() == "")
            {
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    if (rdoCP0.Checked)
                    {
                        SQL = "";
                        SQL += " DELETE ADMIN.ENDO_REMARK                                              \r";
                        SQL += "  WHERE Ptno  = '" + clsOrdFunction.Pat.PtNo + "'                           \r";
                        SQL += "    AND JDate = TO_DATE('" + clsOrdFunction.GstrBDate + "', 'YYYY-MM-DD')   \r";
                        SQL += "    AND OrderCode = '" + strOrdercode + "'                                  \r";
                    }
                    else
                    {
                        SQL = "";
                        SQL += " DELETE ADMIN.ENDO_REMARK_CP                                           \r";
                        SQL += "  WHERE CPCODE    = '" + lstCode[cboCP.SelectedIndex] + "'                  \r";
                        SQL += "    AND OrderCode = '" + strOrdercode + "'                                  \r";
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
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }
                clsOrdFunction.GstrResultChk = "0";
            }
            else
            {
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    if (rdoCP0.Checked)
                    {
                        #region CP가 안리경우
                        SQL = "";
                        SQL += " UPDATE ADMIN.ENDO_REMARK                                              \r";
                        SQL += "    SET REMARKC = '" + rtxtRemark1.Text.Trim() + "'                         \r";
                        SQL += "      , REMARKD = '" + rtxtRemark2.Text.Trim() + "'                         \r";
                        SQL += "  WHERE Ptno  = '" + clsOrdFunction.Pat.PtNo + "'                           \r";
                        SQL += "    AND JDate = TO_DATE('" + clsOrdFunction.GstrBDate + "', 'YYYY-MM-DD')   \r";
                        SQL += "    AND OrderCode = '" + strOrdercode + "'                                  \r";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (intRowAffected == 0)
                        {
                            SQL = "";
                            SQL += " INSERT INTO ADMIN.ENDO_REMARK                                 \r";
                            SQL += "       (PTNO, JDATE, ORDERCODE, REMARKC, REMARKD)                   \r";
                            SQL += " VALUES                                                             \r";
                            SQL += "       ('" + clsOrdFunction.Pat.PtNo + "'                           \r";
                            SQL += "      , TO_DATE('" + clsOrdFunction.GstrBDate + "', 'YYYY-MM-DD')   \r";
                            SQL += "      , '" + strOrdercode + "'                                      \r";
                            SQL += "      , '" + rtxtRemark1.Text.Trim() + "'                           \r";
                            SQL += "      , '" + rtxtRemark2.Text.Trim() + "')                          \r";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        #endregion
                    }
                    #region CPㅇㄹ경우
                    else
                    {
                        SQL = "";
                        SQL += " UPDATE ADMIN.ENDO_REMARK_CP                                           \r";
                        SQL += "    SET REMARKC   = '" + rtxtRemark1.Text.Trim() + "'                       \r";
                        SQL += "      , REMARKD   = '" + rtxtRemark2.Text.Trim() + "'                       \r";
                        SQL += "  WHERE CPCODE    = '" + lstCode[cboCP.SelectedIndex] + "'                  \r";
                        SQL += "    AND OrderCode = '" + strOrdercode + "'                                  \r";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (intRowAffected == 0)
                        {
                            SQL = "";
                            SQL += " INSERT INTO ADMIN.ENDO_REMARK_CP                              \r";
                            SQL += "       (CPCODE, ORDERCODE, REMARKC, REMARKD)                        \r";
                            SQL += " VALUES                                                             \r";
                            SQL += "       ('" + lstCode[cboCP.SelectedIndex] + "'                      \r";
                            SQL += "      , '" + strOrdercode + "'                                      \r";
                            SQL += "      , '" + rtxtRemark1.Text.Trim() + "'                           \r";
                            SQL += "      , '" + rtxtRemark2.Text.Trim() + "')                          \r";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                    #endregion



                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }
                clsOrdFunction.GstrResultChk = "1";
            }

            this.Close();
        }

        private void rtxtRemark1_Click(object sender, EventArgs e)
        {
            //rtxtRemark1.SelectionStart = 0;
            //rtxtRemark1.SelectionLength = rtxtRemark1.Text.Length + 1;
            fn_Color_Set();
            rtxtRemark1.BackColor = Color.LavenderBlush;
            strFocusForm = "Remark1";
        }

        private void rtxtRemark2_Click(object sender, EventArgs e)
        {
            //rtxtRemark2.SelectionStart = 0;
            //rtxtRemark2.SelectionLength = rtxtRemark2.Text.Length + 1;
            fn_Color_Set();
            rtxtRemark2.BackColor = Color.LavenderBlush;
            strFocusForm = "Remark2";
        }

        void fn_Color_Set()
        {
            rtxtRemark1.BackColor = Color.White;
            rtxtRemark2.BackColor = Color.White;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            if (FrmViewRemarkEvent != null)
            {
                FrmViewRemarkEvent.Dispose();
                FrmViewRemarkEvent = null;
            }

            FrmViewRemarkEvent = new FrmViewRemark(clsOrdFunction.GstrDrCode.Trim());
            FrmViewRemarkEvent.SendEvent += F_SendEvent;
            FrmViewRemarkEvent.rEventClosed += F_rEventClosed;
            FrmViewRemarkEvent.StartPosition = FormStartPosition.CenterParent;
            FrmViewRemarkEvent.ShowDialog(this);
        }

        private void F_rEventClosed()
        {
            if (FrmViewRemarkEvent != null)
            {
                FrmViewRemarkEvent.Dispose();
                FrmViewRemarkEvent = null;
            }
        }

        private void F_SendEvent(string SendRetValue)
        {
            if (SendRetValue != null)
            {
                if (SendRetValue.Trim() != "")
                {
                    if (strFocusForm == "Remark1")
                    {
                        rtxtRemark1.Text = SendRetValue.Trim();                        
                    }
                    else
                    {
                        rtxtRemark2.Text = SendRetValue.Trim();
                    }
                }
            }
        }

        private void FrmViewEndoRemark_FormClosed(object sender, FormClosedEventArgs e)
        {
            SendEvent?.Invoke(this, spd, this.nRow, clsOrdFunction.GstrResultChk);
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
    }
}
