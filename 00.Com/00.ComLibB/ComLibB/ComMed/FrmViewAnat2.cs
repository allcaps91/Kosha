using ComBase;
using ComBase.Controls;
using ComDbB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Description : Pathology 소견
    /// Author : 이상훈
    /// Create Date : 2017.11.09
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="FrmViewAnat2.frm"/>
    public partial class FrmViewAnat2 : Form
    {
        string strAnatCode;
        string strAnatName;
        string strGBIO;
        string strSuCode;

        public delegate void SendResultValue(Form frm, FpSpread spd, int Row, string Result);
        public event SendResultValue SendEvent;
        int nRow = -1;
        FpSpread spd = null;
        List<string> lstCode = null;

        string SQL;
        DataTable dt = null;
        string SqlErr = "";     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        string strOK = "";

        clsPrint ClsPrint = new clsPrint();
        clsSpread SP = new clsSpread();

        DateTime dtpSyDate;
        string GstrResultChk = string.Empty;

        public FrmViewAnat2()
        {
            InitializeComponent();
        }

        public FrmViewAnat2(string sAnatCode, string sAnatName, string sGBIO)
        {
            InitializeComponent();

            strAnatCode = sAnatCode;
            strAnatName = sAnatName;
            strGBIO = sGBIO;
        }

        /// <summary>
        /// 전산업무 의뢰서(2021-460) 작업용
        /// </summary>
        /// <param name="sAnatCode"></param>
        /// <param name="sAnatName"></param>
        /// <param name="sGBIO"></param>
        /// <param name="sSuCode"></param>
        public FrmViewAnat2(string sAnatCode, string sAnatName, string sGBIO, string sSuCode)
        {
            InitializeComponent();

            strAnatCode = sAnatCode;
            strAnatName = sAnatName;
            strGBIO = sGBIO;
            strSuCode = sSuCode;
        }

        /// <summary>
        /// 2021-04-22 폼 모달리스용도로 추가
        /// </summary>
        /// <param name="sAnatCode"></param>
        /// <param name="sAnatName"></param>
        /// <param name="sGBIO"></param>
        /// <param name="nRow"></param>
        public FrmViewAnat2(string sAnatCode, string sAnatName, string sGBIO, string sSucode, FpSpread spd, int nRow = -1)
        {
            InitializeComponent();

            strAnatCode = sAnatCode;
            strAnatName = sAnatName;
            strGBIO = sGBIO;
            strSuCode = sSucode;
            this.spd = spd;
            this.nRow = nRow;
        }

        private void FrmViewAnat2_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            dtpSyDate = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();

            fn_Pathology_Clear();

            lblOrder.Text = string.Format("오더코드: {0},  오더명칭: {1}", strAnatCode, strAnatName);

            clsOrdFunction.GstrResultChk = "";
            panCP.Visible = clsOrdFunction.GstrSetRegYN.Equals("Y"); //약속처방 일때만 보임. 그외 사용XXXXX
            lstCode = new List<string>();

            rtxtRemark1.Text =  strAnatName;

            try
            {
                SQL = "";
                SQL += " SELECT A.*                                                                                                                                         \r";
                SQL += "   , (SELECT TRIM(SUNAMEK)                                                                                                                          \r";
                SQL += "        FROM KOSMOS_PMPA.BAS_SUN                                                                                                                    \r";
                SQL += "       WHERE SUNEXT = '" + strSuCode.PadRight(12, ' ') +        "'                                                                                  \r";
                SQL += "     ) AS SUNAMEK                                                                                                                                   \r";

                SQL += "   FROM KOSMOS_OCS.EXAM_ANATMST A                                                                                                                   \r";
                SQL += "  WHERE PTNO      = '" + (clsOrdFunction.Pat.PtNo != null ? clsOrdFunction.Pat.PtNo.Trim() : "") + "'                                               \r";
                SQL += "    AND BDATE     = TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')                                                                        \r";
                SQL += "    AND ORDERCODE = '" + strAnatCode.Trim() + "'                                                                                                    \r";
                SQL += "    AND GBIO      = '" + strGBIO + "'                                                                                                               \r";
                SQL += "    AND DEPTCODE  = '" + (clsOrdFunction.Pat.DeptCode != null ? clsOrdFunction.Pat.DeptCode.Trim() : "") + "'                                       \r";
                SQL += "    AND ROWNUM    = 1                                                                                                                               \r";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    #region 이전 서식
                    ssPrint_OLD_Sheet1.Cells[8, 4].Text = strSuCode.Trim() + "(" + dt.Rows[0]["SUNAMEK"].ToString() + ")";
                    ssPrint_OLD_Sheet1.Rows[8].Height = ssPrint_OLD_Sheet1.Rows[8].GetPreferredHeight() + 8;
                    #endregion

                    #region 신규 서식
                    ssPrint_Sheet1.Cells[8, 5].Text = strSuCode.Trim() + "(" + dt.Rows[0]["SUNAMEK"].ToString() + ")";
                    ssPrint_Sheet1.Rows[8].Height = ssPrint_Sheet1.Rows[8].GetPreferredHeight() + 8;
                    #endregion

                    rtxtRemark1.Text = dt.Rows[0]["REMARK1"].ToString();
                    rtxtRemark2.Text = dt.Rows[0]["REMARK2"].ToString();
                    rtxtRemark3.Text = dt.Rows[0]["REMARK3"].ToString();
                    rtxtRemark4.Text = dt.Rows[0]["REMARK4"].ToString();
                    rtxtRemark5.Text = dt.Rows[0]["REMARK5"].ToString();

                    clsOrdFunction.GstrResultChk = "1";
                    GstrResultChk = "1";
                }

                dt.Dispose();
                dt = null;

                #region 처방코드 못 가져 왔을시
                if (ssPrint_OLD_Sheet1.Cells[8, 4].Text.Trim().IsNullOrEmpty() && 
                    ssPrint_Sheet1.Cells[8, 5].Text.Trim().IsNullOrEmpty())
                {
                    SQL = "";
                    SQL += "   SELECT TRIM(SUNAMEK)  AS SUNAMEK                                                                                                       \r";
                    SQL += "     FROM KOSMOS_PMPA.BAS_SUN                                                                                                             \r";
                    SQL += "    WHERE SUNEXT = '" + strSuCode.PadRight(12, ' ') + "'                                                                                  \r";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        #region 이전 서식
                        ssPrint_OLD_Sheet1.Cells[8, 4].Text = strSuCode.Trim() + "(" + dt.Rows[0]["SUNAMEK"].ToString() + ")";
                        ssPrint_OLD_Sheet1.Rows[8].Height = ssPrint_OLD_Sheet1.Rows[8].GetPreferredHeight() + 8;
                        #endregion

                        #region 신규 서식
                        ssPrint_Sheet1.Cells[8, 5].Text = strSuCode.Trim() + "(" + dt.Rows[0]["SUNAMEK"].ToString() + ")";
                        ssPrint_Sheet1.Rows[8].Height = ssPrint_Sheet1.Rows[8].GetPreferredHeight() + 8;
                        #endregion
                    }

                    dt.Dispose();
                    dt = null;
                }
                #endregion
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (rtxtRemark1.Text.Trim() == "")
            {
                rtxtRemark1.Text = strAnatName.Trim();
            }
        }

        void fn_Pathology_Clear()
        {
            #region 이전서식
            ssPrint_OLD.ActiveSheet.Cells[5, 2].Text = "";
            ssPrint_OLD.ActiveSheet.Cells[6, 2].Text = "";
            ssPrint_OLD.ActiveSheet.Cells[6, 4].Text = "";
            ssPrint_OLD.ActiveSheet.Cells[7, 4].Text = "";
            ssPrint_OLD.ActiveSheet.Cells[7, 2].Text = "";
            ssPrint_OLD.ActiveSheet.Cells[4, 4].Text = "";
            ssPrint_OLD.ActiveSheet.Cells[8, 2].Text = "";
            ssPrint_OLD.ActiveSheet.Cells[12, 1].Text = "";
            ssPrint_OLD.ActiveSheet.Cells[15, 1].Text = "";
            ssPrint_OLD.ActiveSheet.Cells[18, 1].Text = "";
            ssPrint_OLD.ActiveSheet.Cells[21, 1].Text = "";
            ssPrint_OLD.ActiveSheet.Cells[24, 1].Text = "";
            ssPrint_OLD.ActiveSheet.Cells[26, 4].Text = "";
            #endregion

            #region 신규 서식
            ssPrint.ActiveSheet.Cells[5, 1].Text = "";
            ssPrint.ActiveSheet.Cells[6, 1].Text = "";
            ssPrint.ActiveSheet.Cells[7, 1].Text = "";
            ssPrint.ActiveSheet.Cells[8, 1].Text = "";

            ssPrint.ActiveSheet.Cells[5, 5].Text = "";
            ssPrint.ActiveSheet.Cells[6, 5].Text = "";
            ssPrint.ActiveSheet.Cells[7, 5].Text = "";
            ssPrint.ActiveSheet.Cells[8, 5].Text = "";

            ssPrint.ActiveSheet.Cells[11 + 1, 0].Text = "";
            ssPrint.ActiveSheet.Cells[13 + 1, 0].Text = "";
            ssPrint.ActiveSheet.Cells[15 + 1, 0].Text = "";
            ssPrint.ActiveSheet.Cells[17 + 1, 0].Text = "";
            ssPrint.ActiveSheet.Cells[19 + 1, 0].Text = "";
            #endregion

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            fn_Pathology_Insert(sender);

        }

        void fn_Pathology_Insert(object sender)
        {
            string strRowId = "";

            strOK = "NO";

            strOK = clsOrdFunction.fn_SpecialCharCheck(rtxtRemark1.Text);

            //if (strOK == "NO")
            //{
            //    rtxtRemark1.Focus();
            //    MessageBox.Show("Organ and location of Specimen ObTained를 정확하게 입력하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            //strOK = "NO";
            //strOK = clsOrdFunction.fn_SpecialCharCheck(rtxtRemark2.Text);

            //if (strOK == "NO")
            //{
            //    rtxtRemark2.Focus();
            //    MessageBox.Show("Clinical History and Contributory Information을 정확하게 입력하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            //strOK = "NO";
            //strOK = clsOrdFunction.fn_SpecialCharCheck(rtxtRemark3.Text);

            //if (strOK == "NO")
            //{
            //    rtxtRemark3.Focus();
            //    MessageBox.Show("Clinical Diagnosis를 정확하게 입력하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            //strOK = "NO";
            //strOK = clsOrdFunction.fn_SpecialCharCheck(rtxtRemark4.Text);

            //if (strOK == "NO")
            //{
            //    rtxtRemark4.Focus();
            //    MessageBox.Show("Postoperative Findings and Diagnosis를 정확하게 입력하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            if (rtxtRemark1.Text.Trim().Length < 3)
            {
                MessageBox.Show("Organ and location of Specimen ObTained를 3글자이상 입력하십시오!!!", "소견", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rtxtRemark1.Focus();
                return;
            }

            if (rtxtRemark2.Text.Trim().Length < 5)
            {
                MessageBox.Show("Clinical History and Contributory Information을 5글자이상 입력하십시오!!!", "소견", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rtxtRemark2.Focus();
                return;
            }

            if (rtxtRemark4.Text.Trim() == "")
            {
                MessageBox.Show("Clinical Diagnosis가 공란입니다!!!", "소견", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rtxtRemark4.Focus();
                return;
            }

            if (rtxtRemark5.Text.Trim() == "")
            {
                MessageBox.Show("Postoperative Findings and Diagnosis이 공란입니다!!!", "소견", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rtxtRemark5.Focus();
                return;
            }

            rtxtRemark1.Text = rtxtRemark1.Text.Replace("'", "`");
            rtxtRemark2.Text = rtxtRemark2.Text.Replace("'", "`");
            rtxtRemark3.Text = rtxtRemark3.Text.Replace("'", "`");
            rtxtRemark4.Text = rtxtRemark4.Text.Replace("'", "`");
            rtxtRemark5.Text = rtxtRemark5.Text.Replace("'", "`");

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
                    SQL += "   merge into KOSMOS_OCS.EXAM_ANATMST b                                     \r";
                    SQL += "   using dual d                                                             \r";
                    SQL += "      on (b.ROWID = '" + strRowId + "'                                      \r";
                    SQL += "     AND  b.PTNO = '" + clsOrdFunction.Pat.PtNo + "'                        \r";
                    SQL += "     AND BDate     = TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')  \r";
                    SQL += "     AND OrderCode = '" + strAnatCode.Trim() + "')                           \r";
                    SQL += "    when matched then                                                       \r";
                    SQL += "  update set                                                                \r";
                    SQL += "         Remark1 = '" + rtxtRemark1.Text.Trim() + "'                        \r";
                    SQL += "	   , Remark2 = '" + rtxtRemark2.Text.Trim() + "'                        \r";
                    SQL += "	   , Remark3 = '" + rtxtRemark3.Text.Trim() + "'                        \r";
                    SQL += "	   , Remark4 = '" + rtxtRemark4.Text.Trim() + "'                        \r";
                    SQL += "	   , Remark5 = '" + rtxtRemark5.Text.Trim() + "'                        \r";
                    SQL += "    when not matched then                                                   \r";
                    SQL += "  insert                                                                    \r";
                    SQL += "        (Ptno                                                               \r";
                    SQL += "       , BDate                                                              \r";
                    SQL += "       , OrderCode                                                          \r";
                    SQL += "       , GbIO                                                               \r";
                    SQL += "       , Remark1                                                            \r";
                    SQL += "       , Remark2                                                            \r";
                    SQL += "       , Remark3                                                            \r";
                    SQL += "       , Remark4                                                            \r";
                    SQL += "       , Remark5                                                            \r";
                    SQL += "       , DeptCode                                                           \r";
                    SQL += "       , DrCode)                                                            \r";
                    SQL += "  values                                                                    \r";
                    SQL += "        ('" + clsOrdFunction.Pat.PtNo + "'                                  \r";
                    SQL += "       , to_date('" + clsOrdFunction.GstrBDate + "', 'yyyy-mm-dd')          \r";
                    SQL += "       , '" + strAnatCode + "'                                              \r";
                    SQL += "       , '" + strGBIO + "'                                                  \r";
                    SQL += "       , '" + rtxtRemark1.Text.Trim() + "'                                  \r";
                    SQL += "       , '" + rtxtRemark2.Text.Trim() + "'                                  \r";
                    SQL += "       , '" + rtxtRemark3.Text.Trim() + "'                                  \r";
                    SQL += "       , '" + rtxtRemark4.Text.Trim() + "'                                  \r";
                    SQL += "       , '" + rtxtRemark5.Text.Trim() + "'                                  \r";
                    SQL += "       , '" + clsOrdFunction.Pat.DeptCode.Trim() + "'                       \r";
                    SQL += "       , '" + VB.Right(clsOrdFunction.Pat.DrCode.Trim(), 4) + "')           \r";

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
                    SQL += " SELECT ROWID FROM KOSMOS_OCS.EXAM_ANATMST_CP                               \r";
                    SQL += "  WHERE CPCODE      = '" + lstCode[cboCP.SelectedIndex] + "'                \r";
                    SQL += "    AND OrderCode = '" + strAnatCode.Trim() + "'                            \r";

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
                    SQL += "   merge into KOSMOS_OCS.EXAM_ANATMST_CP b                                  \r";
                    SQL += "   using dual d                                                             \r";
                    SQL += "      on (b.ROWID  = '" + strRowId + "'                                     \r";
                    SQL += "     AND  b.CPCODE = '" + lstCode[cboCP.SelectedIndex] + "'                 \r";                    
                    SQL += "     AND OrderCode = '" + strAnatCode.Trim() + "')                          \r";
                    SQL += "    when matched then                                                       \r";
                    SQL += "  update set                                                                \r";
                    SQL += "         Remark1 = '" + rtxtRemark1.Text.Trim() + "'                        \r";
                    SQL += "	   , Remark2 = '" + rtxtRemark2.Text.Trim() + "'                        \r";
                    SQL += "	   , Remark3 = '" + rtxtRemark3.Text.Trim() + "'                        \r";
                    SQL += "	   , Remark4 = '" + rtxtRemark4.Text.Trim() + "'                        \r";
                    SQL += "	   , Remark5 = '" + rtxtRemark5.Text.Trim() + "'                        \r";
                    SQL += "    when not matched then                                                   \r";
                    SQL += "  insert                                                                    \r";
                    SQL += "        (CPCODE                                                             \r";
                    SQL += "       , OrderCode                                                          \r";
                    SQL += "       , GbIO                                                               \r";
                    SQL += "       , Remark1                                                            \r";
                    SQL += "       , Remark2                                                            \r";
                    SQL += "       , Remark3                                                            \r";
                    SQL += "       , Remark4                                                            \r";
                    SQL += "       , Remark5                                                            \r";
                    SQL += "       , DeptCode                                                           \r";
                    SQL += "       , DrCode)                                                            \r";
                    SQL += "  values                                                                    \r";
                    SQL += "        ('" + lstCode[cboCP.SelectedIndex] + "'                             \r";
                    SQL += "       , '" + strAnatCode + "'                                              \r";
                    SQL += "       , '" + strGBIO + "'                                                  \r";
                    SQL += "       , '" + rtxtRemark1.Text.Trim() + "'                                  \r";
                    SQL += "       , '" + rtxtRemark2.Text.Trim() + "'                                  \r";
                    SQL += "       , '" + rtxtRemark3.Text.Trim() + "'                                  \r";
                    SQL += "       , '" + rtxtRemark4.Text.Trim() + "'                                  \r";
                    SQL += "       , '" + rtxtRemark5.Text.Trim() + "'                                  \r";
                    SQL += "       , '" + clsType.User.DeptCode + "'                                    \r";
                    SQL += "       , '" + clsType.User.DrCode + "')                                     \r";

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
                strOK = "OK";

                if (sender.Equals(btnPrint) && strOK.Equals("OK"))
                {
                    fn_Pathology_Print();
                    ComFunc.Delay(500);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void fn_Pathology_Print()
        {
            #region 이전 서식
            ssPrint_OLD.ActiveSheet.Cells[5, 2].Text = clsOrdFunction.Pat.PtNo;
            ssPrint_OLD.ActiveSheet.Cells[6, 2].Text = clsOrdFunction.Pat.sName;
            ssPrint_OLD.ActiveSheet.Cells[6, 4].Text = clsOrdFunction.Pat.Sex + " / " + clsOrdFunction.Pat.Age;
            ssPrint_OLD.ActiveSheet.Cells[7, 4].Text = clsOrdFunction.Pat.DeptCode.Trim() + " / " + clsOrdFunction.Pat.WardCode;
            #endregion

            #region 신규 서식
            ssPrint.ActiveSheet.Cells[5, 1].Text = clsOrdFunction.Pat.sName;
            ssPrint.ActiveSheet.Cells[6, 1].Text = clsOrdFunction.Pat.PtNo;

            ssPrint.ActiveSheet.Cells[5, 5].Text = clsOrdFunction.Pat.Sex + " / " + clsOrdFunction.Pat.Age;
            ssPrint.ActiveSheet.Cells[7, 5].Text = clsOrdFunction.Pat.DeptCode.Trim() + " / " + clsOrdFunction.Pat.WardCode;
            #endregion

            try
            {
                SQL = "";
                SQL += " SELECT JUMIN1,JUMIN2, JUMIN3                       \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_PATIENT                     \r";
                SQL += "  WHERE PANO = '" + clsOrdFunction.Pat.PtNo + "'    \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["JUMIN3"].ToString() != "")
                    {
                        ssPrint_OLD.ActiveSheet.Cells[7, 2].Text = dt.Rows[0]["JUMIN1"].ToString() + "-" + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString());
                        ssPrint.ActiveSheet.Cells[6, 5].Text = dt.Rows[0]["JUMIN1"].ToString() + "-" + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString());
                    }
                    else
                    {
                        ssPrint_OLD.ActiveSheet.Cells[7, 2].Text = dt.Rows[0]["JUMIN1"].ToString() + "-" + dt.Rows[0]["JUMIN2"].ToString();
                        ssPrint.ActiveSheet.Cells[6, 5].Text = dt.Rows[0]["JUMIN1"].ToString() + "-" + dt.Rows[0]["JUMIN2"].ToString();
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
        
            #region 이전 서식
            ssPrint_OLD.ActiveSheet.Cells[4, 4].Text = clsOrdFunction.Pat.DeptCode + " / " + clsOrdFunction.Pat.WardCode;
            ssPrint_OLD.ActiveSheet.Cells[8, 2].Text = VB.Left(clsPublic.GstrSysDate, 4) + "년 " + VB.Mid(clsPublic.GstrSysDate, 6, 2) + "월 " + VB.Right(clsPublic.GstrSysDate, 2) + "일";
            ssPrint_OLD.ActiveSheet.Cells[12, 1].Text = rtxtRemark1.Text;
            ssPrint_OLD.ActiveSheet.Cells[15, 1].Text = rtxtRemark2.Text;
            ssPrint_OLD.ActiveSheet.Cells[18, 1].Text = rtxtRemark3.Text;
            ssPrint_OLD.ActiveSheet.Cells[21, 1].Text = rtxtRemark4.Text;
            ssPrint_OLD.ActiveSheet.Cells[24, 1].Text = rtxtRemark5.Text;

            ssPrint_OLD.ActiveSheet.Cells[26, 4].Text = clsOrdFunction.Pat.DrName;
            ssPrint_OLD.ActiveSheet.Cells[26, 4].Text = clsType.User.UserName;
            ssPrint_OLD.ActiveSheet.Cells[29, 4].Text = "";

            //전산업무의뢰서 2019 - 882
            SetDrSign(ssPrint_OLD, 27, 4, clsType.User.Sabun);
            #endregion

            #region 신규 서식
            ssPrint.ActiveSheet.Cells[7, 1].Text = clsOrdFunction.Pat.DeptCode + " / " + clsOrdFunction.Pat.WardCode;
            ssPrint.ActiveSheet.Cells[7, 5].Text = clsOrdFunction.Pat.DrName;

            ssPrint.ActiveSheet.Cells[8, 1].Text = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>().ToString("yyyy-MM-dd");

            ssPrint.ActiveSheet.Cells[11 + 1, 0].Text = rtxtRemark1.Text;
            ssPrint.ActiveSheet.Cells[13 + 1, 0].Text = rtxtRemark2.Text;
            ssPrint.ActiveSheet.Cells[15 + 1, 0].Text = rtxtRemark4.Text;
            ssPrint.ActiveSheet.Cells[17 + 1, 0].Text = rtxtRemark5.Text;
            ssPrint.ActiveSheet.Cells[19 + 1, 0].Text = rtxtRemark3.Text;
            #endregion
           

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;
            
            strHeader = "";            
            strFooter = "";

            setMargin = new clsSpread.SpdPrint_Margin(30, 10, 40, 10, 20, 20);
            setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, false, false, false, false, true, false, false);

            //SP.setSpdPrint(ssPrint_OLD, PrePrint, setMargin, setOption, strHeader, strFooter);
            SP.setSpdPrint(dtpSyDate.Date < Convert.ToDateTime("2021-06-10") ? ssPrint_OLD : ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);

            MessageBox.Show("출력 되었습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            fn_Pathology_Insert(sender);
        }

        private void SetDrSign(FarPoint.Win.Spread.FpSpread spd, int row, int Col, string sabun)
        {
            Image ImageX = GetDrSign(clsDB.DbCon, sabun, "");
            FarPoint.Win.Spread.CellType.TextCellType cellType = new FarPoint.Win.Spread.CellType.TextCellType();
            cellType.BackgroundImage = new FarPoint.Win.Picture(ImageX, FarPoint.Win.RenderStyle.Stretch);
            spd.ActiveSheet.Cells[row, Col].CellType = cellType;

            ImageX = null;
            cellType = null;
        }

        static Image GetDrSign(PsmhDb pDbCon, string strSabun, string strgubun)
        {
            Image rtnVAL = null;

            if (string.IsNullOrEmpty(strSabun)) return rtnVAL;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "SIGNATURE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                if (strgubun == "1")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE TRIM(drcode) = '" + strSabun + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE TRIM(SABUN) = '" + strSabun + "'";
                }


                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return rtnVAL;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVAL;
                }

                if (dt.Rows[0]["SIGNATURE"] == DBNull.Value)
                {
                    ComFunc.MsgBox("현재 의사는 서명이 없습니다 확인해주세요.");
                    return rtnVAL;
                }

                using (MemoryStream memStream = new MemoryStream((byte[])dt.Rows[0]["SIGNATURE"]))
                {
                    rtnVAL = Image.FromStream(memStream);
                }

                return rtnVAL;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVAL;
            }
        }

        private void FrmViewAnat2_FormClosed(object sender, FormClosedEventArgs e)
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
