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
    /// Description : PB smear 소견
    /// Author : 이상훈
    /// Create Date : 2017.11.09
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="FrmViewAnat3.frm"/>
    public partial class FrmViewAnat3 : Form
    {
        string strillName = string.Empty;
        string strGBIO = string.Empty;

        int nRow = -1;
        public delegate void SendResultValue(Form frm, FpSpread spd, int Row, string Result);
        public event SendResultValue SendEvent;
        FpSpread spd = null;
        List<string> lstCode = null;

        string SQL = string.Empty;
        string SqlErr = string.Empty;     //에러문 받는 변수

        DataTable dt = null;
        int intRowAffected = 0; //변경된 Row 받는 변수
        string strFocusForm = string.Empty;

        clsOrdFunction OF = new clsOrdFunction();

        string GstrResultChk = string.Empty;

        private FrmViewRemark FrmViewRemarkEvent = null;

        public FrmViewAnat3()
        {
            InitializeComponent();
        }

        public FrmViewAnat3(string sillName, string sGBIO)
        {
            InitializeComponent();

            strillName = sillName;
            strGBIO = sGBIO;
        }

        /// <summary>
        /// 2021-04-22 모달리스용 추가
        /// </summary>
        /// <param name="sillName"></param>
        /// <param name="sGBIO"></param>
        /// <param name="nRow"></param>
        public FrmViewAnat3(string sillName, string sGBIO, FpSpread spd, int nRow)
        {
            InitializeComponent();

            strillName = sillName;
            strGBIO = sGBIO;
            this.spd = spd;
            this.nRow = nRow;
        }

        private void FrmViewAnat3_Load(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            clsOrdFunction.GstrResultChk = "";

            if (strillName != "")
            {
                rtxtRequest1.Text = strillName;
            }

            lstCode = new List<string>();
            panCP.Visible = clsOrdFunction.GstrSetRegYN.Equals("Y"); //약속처방 일때만 보임. 그외 사용XXXXX

            try
            {
                SQL = "";
                SQL += " SELECT REQUEST1, REQUEST2                                                                                               \r";
                SQL += "   FROM KOSMOS_OCS.EXAM_ORDER_PB                                                                                         \r";
                SQL += "  WHERE PAno      = '" + clsOrdFunction.Pat.PtNo + "'                                                                    \r";
                SQL += "    AND BDate     = TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')                                             \r";
                SQL += "    AND GbIO      = '" + strGBIO + "'                                                                                    \r";
                SQL += "    AND DeptCode  = '" + (clsOrdFunction.Pat.DeptCode != null ? clsOrdFunction.Pat.DeptCode.Trim() : "") + "'            \r";
                SQL += "    AND ROWNUM    = 1                                                                                                    \r";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    rtxtRequest1.Text = dt.Rows[0]["REQUEST1"].ToString();
                    rtxtRequest2.Text = dt.Rows[0]["REQUEST2"].ToString();

                    clsOrdFunction.GstrResultChk = "1";
                    GstrResultChk = "1";
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strRowId = "";
            string strOK = "";

            strOK = "NO";
            strOK = clsOrdFunction.fn_SpecialCharCheck(rtxtRequest1.Text);

            if (strOK == "NO")
            {
                rtxtRequest1.Focus();
                MessageBox.Show("Clinical Diagnosis or Impression을 정확하게 입력하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            strOK = "NO";
            strOK = clsOrdFunction.fn_SpecialCharCheck(rtxtRequest2.Text);

            if (strOK == "NO")
            {
                rtxtRequest2.Focus();
                MessageBox.Show("Specific previous Hematologic Disease을 정확하게 입력하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (rtxtRequest1.Text.Trim().Length < 3)
            {
                MessageBox.Show("Clinical Diagnosis or Impression을 3글자이상 입력하십시오!!!", "소견", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rtxtRequest1.Focus();
                return;
            }

            if (rtxtRequest2.Text.Trim().Length < 4)
            {
                MessageBox.Show("Specific previous Hematologic Disease을 4글자이상 입력하십시오!!!", "소견", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rtxtRequest2.Focus();
                return;
            }

            rtxtRequest1.Text = rtxtRequest1.Text.Replace("'", "`");
            rtxtRequest2.Text = rtxtRequest2.Text.Replace("'", "`");

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                #region CP 아닐경우
                if (rdoCP0.Checked)
                {
                    SQL = "";
                    SQL += " SELECT ROWID FROM  KOSMOS_OCS.EXAM_ORDER_PB                                                                             \r";
                    SQL += "  WHERE Pano      = '" + clsOrdFunction.Pat.PtNo + "'                                                                    \r";
                    SQL += "    AND BDate     = TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')                                             \r";
                    SQL += "    AND GbIO      = '" + strGBIO + "'                                                                                    \r";
                    SQL += "    AND DeptCode  = '" + (clsOrdFunction.Pat.DeptCode != null ? clsOrdFunction.Pat.DeptCode.Trim() : "") + "'            \r";
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
                    SQL += "   merge into KOSMOS_OCS.EXAM_ORDER_PB b                                \r";
                    SQL += "   using dual d                                                         \r";
                    SQL += "      on (b.ROWID = '" + strRowId + "')                                 \r";
                    SQL += "    when matched then                                                   \r";
                    SQL += "  update set                                                            \r";
                    SQL += "         REQUEST1 = '" + rtxtRequest1.Text.Trim() + "'                  \r";
                    SQL += "	   , REQUEST2 = '" + rtxtRequest2.Text.Trim() + "'                  \r";
                    SQL += "    when not matched then                                               \r";
                    SQL += "  insert                                                                \r";
                    SQL += "        (Pano                                                           \r";
                    SQL += "       , BDate                                                          \r";
                    SQL += "       , GbIO                                                           \r";
                    SQL += "       , DeptCode                                                       \r";
                    SQL += "       , DrCode                                                         \r";
                    SQL += "       , WARDCODE                                                       \r";
                    SQL += "       , ROOMCODE                                                       \r";
                    SQL += "       , SPECNO                                                         \r";
                    SQL += "       , REQUEST1                                                       \r";
                    SQL += "       , REQUEST2                                                       \r";
                    SQL += "       , ENTDATE)                                                       \r";
                    SQL += "  values                                                                \r";
                    SQL += "         ('" + clsOrdFunction.Pat.PtNo + "'                             \r";
                    SQL += "       , to_date('" + clsOrdFunction.GstrBDate + "', 'yyyy-mm-dd')      \r";
                    SQL += "       , '" + strGBIO + "'                                              \r";
                    SQL += "       , '" + clsOrdFunction.Pat.DeptCode + "'                          \r";
                    SQL += "       , '" + clsOrdFunction.Pat.DrCode + "'                            \r";
                    SQL += "       , ''                                                             \r";
                    SQL += "       , ''                                                             \r";
                    SQL += "       , ''                                                             \r";
                    SQL += "       , '" + rtxtRequest1.Text.Trim() + "'                             \r";
                    SQL += "       , '" + rtxtRequest2.Text.Trim() + "'                             \r";
                    SQL += "       , SYSDATE)                                                       \r";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr + " PB smear 의뢰소견 등록시 오류 발생!!!");
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
                    SQL += " SELECT ROWID FROM  KOSMOS_OCS.EXAM_ORDER_PB_CP                                                                          \r";
                    SQL += "  WHERE CPCODE    = '" + lstCode[cboCP.SelectedIndex] + "'                                                               \r";
                    SQL += "    AND GbIO      = '" + strGBIO + "'                                                                                    \r";

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
                    SQL += "   merge into KOSMOS_OCS.EXAM_ORDER_PB_CP b                             \r";
                    SQL += "   using dual d                                                         \r";
                    SQL += "      on (b.ROWID = '" + strRowId + "')                                 \r";
                    SQL += "    when matched then                                                   \r";
                    SQL += "  update set                                                            \r";
                    SQL += "         REQUEST1 = '" + rtxtRequest1.Text.Trim() + "'                  \r";
                    SQL += "	   , REQUEST2 = '" + rtxtRequest2.Text.Trim() + "'                  \r";
                    SQL += "    when not matched then                                               \r";
                    SQL += "  insert                                                                \r";
                    SQL += "        (CPCODE                                                         \r";
                    SQL += "       , GbIO                                                           \r";
                    SQL += "       , DeptCode                                                       \r";
                    SQL += "       , DrCode                                                         \r";
                    SQL += "       , REQUEST1                                                       \r";
                    SQL += "       , REQUEST2                                                       \r";
                    SQL += "       , ENTDATE)                                                       \r";
                    SQL += "  values                                                                \r";
                    SQL += "         ('" + lstCode[cboCP.SelectedIndex] + "'                        \r";
                    SQL += "       , '" + strGBIO + "'                                              \r";
                    SQL += "       , '" + clsType.User.DeptCode +       "'                          \r";
                    SQL += "       , '" + clsType.User.DrCode   +       "'                          \r";
                    SQL += "       , '" + rtxtRequest1.Text.Trim() + "'                             \r";
                    SQL += "       , '" + rtxtRequest2.Text.Trim() + "'                             \r";
                    SQL += "       , SYSDATE)                                                       \r";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr + " PB smear 의뢰소견 등록시 오류 발생!!!");
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
                        rtxtRequest1.Text = SendRetValue.Trim();
                    }
                    else if (strFocusForm == "Remark2")
                    {
                        rtxtRequest2.Text = SendRetValue.Trim();
                    }
                }
            }
        }

        void fn_Color_Set()
        {
            rtxtRequest1.BackColor = Color.White;
            rtxtRequest2.BackColor = Color.White;
        }

        private void rtxtRequest1_Click(object sender, EventArgs e)
        {
            fn_Color_Set();

            rtxtRequest1.BackColor = Color.LavenderBlush;
            strFocusForm = "Remark1";
        }

        private void rtxtRequest2_Click(object sender, EventArgs e)
        {
            fn_Color_Set();

            rtxtRequest2.BackColor = Color.LavenderBlush;
            strFocusForm = "Remark2";
        }

        private void FrmViewAnat3_FormClosed(object sender, FormClosedEventArgs e)
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
    }
}
