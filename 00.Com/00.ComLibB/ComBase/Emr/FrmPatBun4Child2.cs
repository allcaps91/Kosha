using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using FarPoint.Win.Spread;
using Oracle.DataAccess.Client;
using static ComBase.clsEmrFunc;

namespace ComBase
{
    /// <summary>
    /// Class Name      : MedIpdNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-04-16
    /// Update History  : 
    /// </summary>
    /// <history> 
    /// 2019-07-23 낙상 기록지 기준 변경으로 신규 폼 생성
    /// </history>
    /// <seealso cref= D:\psmh\mtsEmr\CarePlan\FrmPatBun4Child" >> frmSupLbExSTS15.cs 폼이름 재정의" />

    public partial class FrmPatBun4Child2 : Form
    {
        string FstrActdate = "";

        string EXEName = "";
        string GstrHelpCode = "";
        string gsWard = "";

        public FrmPatBun4Child2(string EXENameX, string GstrHelpCodeX, string gsWardX = "")
        {
            InitializeComponent();
            EXEName = EXENameX;
            gsWard = gsWardX;
            GstrHelpCode = GstrHelpCodeX;
        }

        public FrmPatBun4Child2()
        {
            InitializeComponent();
        }

        private void FrmPatBun4Child2_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }


            string[] str = new string[3];
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            if (VB.UCase(EXEName) == "CAREPLAN")
            {
                btnSave.Visible = false;
                btnPrint.Visible = false;
                btnOnePrint.Visible = false;
                btnInfo.Visible = false;
            }

            if (gsWard == "")
            {
                gsWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");
            }

            FstrActdate = "";

            if (gsWard == "ER" || gsWard == "HD")
            {
                str = VB.Split(GstrHelpCode, "|");

                GstrHelpCode = str[0];
                FstrActdate = str[1];
            }

            chkJ.Checked = true;
            chkJ.Checked = false;

            TxtJDATE.Text = FstrActdate;
            TxtJTime.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M", ":");
            chkJ.Visible = true;
            TxtJDATE.Visible = true;
            TxtJTime.Visible = true;
            lbj.Visible = true;


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (gsWard == "ER")
                {
                    chkJ.Checked = true;
                    SQL = "";
                    SQL = " SELECT PTMIINTM ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI";
                    SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + GstrHelpCode + "'";
                    SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + (FstrActdate).Replace("-", "") + "'";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        TxtJDATE.Text = FstrActdate;
                        TxtJTime.Text = Convert.ToDateTime(dt.Rows[0]["PTMIINTM"].ToString().Trim()).ToString("00:00");
                    }
                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            DATA_READ();
        }

        private void chkJ_CheckedChanged(object sender, EventArgs e)
        {
            TxtJDATE.Visible = false;
            TxtJTime.Visible = false;
            lbj.Visible = false;

            if (chkJ.Checked == true)
            {
                TxtJDATE.Visible = true;
                TxtJTime.Visible = true;
                lbj.Visible = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            GstrHelpCode = "";
            this.Close();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            GstrHelpCode = this.Tag.ToString();
            //TODO
            //frmBIGO.Show 1

            GstrHelpCode = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (Save() == false)
            {
                return;
            }

            DATA_READ();
        }

        private bool Save()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            DataTable dt = null;
            int nREAD = 0;
            string strPano = "";
            string strIPDNO = "";
            string strACTDATE = "";
            string strActTime = "";
            string strName = "";
            string strSex = "";
            string strAge = "";
            string strDeptCode = "";
            string strROOMCODE = "";
            string strGubun = "";
            string strJUMSU1 = "";
            string strJUMSU2 = "";
            string strJUMSU3 = "";
            string strJUMSU4 = "";
            string strJUMSU5 = "";
            string strJUMSU6 = "";
            string strJUMSU7 = "";
            string strTOTAL = "";
            string strWARNING = "";
            string strOK = "";
            string strROWID = "";
            double nEMRNO = 0;

            double dblEmrHisNo = 0;

            Cursor.Current = Cursors.WaitCursor;


            strPano = SS2_Sheet1.Cells[0, 1].Text.Trim();
            strName = SS2_Sheet1.Cells[0, 3].Text.Trim();
            strSex = VB.Left(SS2_Sheet1.Cells[0, 5].Text.Trim(), 1);
            strAge = VB.Mid(SS2_Sheet1.Cells[0, 5].Text.Trim(), 3, VB.Len(SS2_Sheet1.Cells[0, 5].Text.Trim()));
            strDeptCode = SS2_Sheet1.Cells[0, 7].Text.Trim();
            strIPDNO = SS2_Sheet1.Cells[0, 9].Text.Trim();

            strROOMCODE = SS2_Sheet1.Cells[1, 3].Text.Trim();
            strACTDATE = SS2_Sheet1.Cells[1, 7].Text.Trim();
            strROWID = SS2_Sheet1.Cells[1, 9].Text.Trim();

            if (chkJ.Checked == true && TxtJDATE.Text == "" || TxtJTime.Text == "")
            {
                MessageBox.Show("조사일시가 공란입니다.", "확인");
                return rtnVal;
            }

            if (chkJ.Checked == true)
            {
                strACTDATE = TxtJDATE.Text.Trim();
                strActTime = TxtJTime.Text.Trim();
            }
            else
            {
                strACTDATE = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                strActTime = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M", ":");

            }

            if (gsWard == "ER" || gsWard == "HD")
            {
                strIPDNO = "0";
            }

            if (READ_CERTIOK(clsType.User.Sabun) == false)
            {
                MessageBox.Show("전자인증서가 없는 사번입니다." + ComNum.VBLF + " 욕창사정도구표를 작성하실 수 없습니다.", "전자인증 에러");
            }

            if (strPano == "")
            {
                MessageBox.Show("등록번호가 공란입니다.");
                return rtnVal;
            }

            if (strIPDNO == "")
            {
                MessageBox.Show("입원번호가 공란입니다.");
                return rtnVal;
            }

            if (strACTDATE == "")
            {
                MessageBox.Show("작업일자가 공란입니다.");
                return rtnVal;
            }

            if (strACTDATE == "")
            {
                strACTDATE = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            }
            strJUMSU1 = SS1_Sheet1.Cells[0, 4].Text.Trim();
            strJUMSU2 = SS1_Sheet1.Cells[4, 4].Text.Trim();
            strJUMSU3 = SS1_Sheet1.Cells[6, 4].Text.Trim();
            strJUMSU4 = SS1_Sheet1.Cells[10, 4].Text.Trim();
            strJUMSU5 = SS1_Sheet1.Cells[13, 4].Text.Trim();
            strJUMSU6 = SS1_Sheet1.Cells[17, 4].Text.Trim();
            strJUMSU7 = SS1_Sheet1.Cells[20, 4].Text.Trim();
            strROWID = SS1_Sheet1.Cells[30, 4].Text.Trim();

            strTOTAL = (VB.Val(strJUMSU1) + VB.Val(strJUMSU2) + VB.Val(strJUMSU3) + VB.Val(strJUMSU4) + VB.Val(strJUMSU5) + VB.Val(strJUMSU6) + VB.Val(strJUMSU7)).ToString();

            if (strJUMSU1 == "" || strJUMSU2 == "" || strJUMSU3 == "" || strJUMSU4 == "" || strJUMSU5 == "" || strJUMSU6 == "" || strJUMSU7 == "")
            {
                MessageBox.Show("점수에 공란이 있습니다. 확인하십시요.", "확인");
                return rtnVal;
            }

            strWARNING = MAKE_WARNING();

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "  SELECT  EMRNO ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALLHUMPDUMP_SCALE ";

                if (gsWard == "HD" || gsWard == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "       AND IPDNO = 0";
                }
                SQL = SQL + ComNum.VBLF + "  WHERE IPDNO =" + strIPDNO + " ";

                SQL = SQL + ComNum.VBLF + "  AND ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    nEMRNO = VB.Val(dt.Rows[0]["EMRNO"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                EmrForm pForm = SerEmrFormUpdateNo(clsDB.DbCon, "2478");
                if (pForm.FmOLDGB == 1)
                {
                    if (nEMRNO > 0)
                    {
                        //기존차트를 변경할 경우 : 백업 테이블로 백업을 하고 신규 data를 입력한다
                        //KOSMOS_EMR.EMRXMLHISTORY_HISTORYNO_SEQ
                        dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, "KOSMOS_EMR.EMRXMLHISNO");

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY";
                        SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                        SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                        SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                        SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                        SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                        SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                        SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                        SQL = SQL + ComNum.VBLF + "      '" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-").Replace("-", "") + "',";
                        SQL = SQL + ComNum.VBLF + "      '" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M", ":").Replace(":", "") + "', '" + clsType.User.Sabun + "',CERTNO";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "";
                        SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXML";
                        SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXMLMST";
                        SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                if (pForm.FmOLDGB == 1)
                {
                    nEMRNO = 0;
                }
                nEMRNO = EMRXML_INSERT(clsDB.DbCon, VB.Val(strIPDNO), strJUMSU1, strJUMSU2, strJUMSU3, strJUMSU4, strJUMSU5, strJUMSU6, strJUMSU7, strTOTAL, strWARNING, nEMRNO);

                if (nEMRNO == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("EMR 전송에 실패하였습니다");
                    return rtnVal;
                }

                if (strROWID == "")
                {
                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_FALLHUMPDUMP_SCALE";
                    SQL = SQL + ComNum.VBLF + " ( PANO, IPDNO, ACTDATE, SNAME,";
                    SQL = SQL + ComNum.VBLF + "  SEX, AGE, DEPTCODE, ROOMCODE,";
                    SQL = SQL + ComNum.VBLF + "  GUBUN, JUMSU1, JUMSU2, JUMSU3,";
                    SQL = SQL + ComNum.VBLF + "  JUMSU4, JUMSU5, JUMSU6, JUMSU7, TOTAL,";
                    SQL = SQL + ComNum.VBLF + "  ENTSABUN, ENTDATE, EMRNO, ACTTIME) VALUES(";
                    SQL = SQL + ComNum.VBLF + "'" + strPano + "'," + VB.Val(strIPDNO) + ",TO_DATE('" + strACTDATE + "','YYYY-MM-DD'),'" + strName + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strSex + "','" + strAge + "','" + strDeptCode + "','" + strROOMCODE + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strGubun + "'," + NVL(strJUMSU1) + "," + NVL(strJUMSU2) + "," + NVL(strJUMSU3) + ", ";
                    SQL = SQL + ComNum.VBLF + "" + NVL(strJUMSU4) + "," + NVL(strJUMSU5) + "," + NVL(strJUMSU6) + "," + NVL(strJUMSU7) + "," + NVL(strTOTAL) + ", ";
                    SQL = SQL + ComNum.VBLF + clsType.User.Sabun + ", SYSDATE," + nEMRNO + ",'" + strActTime + "') ";
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_FALLHUMPDUMP_SCALE SET ";
                    SQL = SQL + ComNum.VBLF + " JUMSU1 = " + NVL(strJUMSU1) + ", ";
                    SQL = SQL + ComNum.VBLF + " JUMSU2 = " + NVL(strJUMSU2) + ", ";
                    SQL = SQL + ComNum.VBLF + " JUMSU3 = " + NVL(strJUMSU3) + ", ";
                    SQL = SQL + ComNum.VBLF + " JUMSU4 = " + NVL(strJUMSU4) + ", ";
                    SQL = SQL + ComNum.VBLF + " JUMSU5 = " + NVL(strJUMSU5) + ", ";
                    SQL = SQL + ComNum.VBLF + " JUMSU6 = " + NVL(strJUMSU6) + ", ";
                    SQL = SQL + ComNum.VBLF + " JUMSU7 = " + NVL(strJUMSU7) + ", ";
                    SQL = SQL + ComNum.VBLF + " TOTAL = " + NVL(strTOTAL) + ", ";
                    SQL = SQL + ComNum.VBLF + " ENTSABUN = " + clsType.User.Sabun + ", ";
                    SQL = SQL + ComNum.VBLF + " ENTDATE = SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + " EMRNO = " + nEMRNO;
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
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

                if (SAVE_WARNING(clsDB.DbCon, strIPDNO, strPano, strACTDATE, strActTime) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("저장 중 에러 발생");
                    return rtnVal;
                }

                if (SAVE_EVAL(clsDB.DbCon, strIPDNO, strPano, strACTDATE, strActTime) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("저장 중 에러 발생");
                    return rtnVal;
                }

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
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

        private string MAKE_WARNING_ERROR()
        {
            string strTemp = "";
            string strDrug = "";
            string strDrug2 = "";
            string strRtn = "";

            strDrug2 = "";

            if (Convert.ToBoolean(SSEval_Sheet1.Cells[0, 3].Value) == true)
            {
                if (strDrug == "")
                { strDrug = "낙상 초래 약물의 초기 투여- "; }
                strDrug2 = strDrug2 + " 진정제";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[0, 4].Value) == true)
            {
                if (strDrug == "")
                { strDrug = "낙상 초래 약물의 초기 투여-"; }
                strDrug2 += " 수면제";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[0, 5].Value) == true)
            {
                if (strDrug == "")
                { strDrug = "낙상 초래 약물의 초기 투여-"; }
                strDrug2 += " 향정신성 약물";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[0, 6].Value) == true)
            {
                if (strDrug == "")
                { strDrug = "낙상 초래 약물의 초기 투여-"; }
                strDrug2 += " 항우울제";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[0, 7].Value) == true)
            {
                if (strDrug == "")
                { strDrug = "낙상 초래 약물의 초기 투여-"; }
                strDrug2 += " 이뇨제";
            }

            strTemp = "";
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[1, 1].Value) == true)
            {
                strTemp += " 주요상태변화:간성 혼수, 알콜 섬망, 발작";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[1, 4].Value) == true)
            {
                strTemp += " 의식상태변화";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[1, 5].Value) == true)
            {
                strTemp += " 전동";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[1, 6].Value) == true)
            {
                strTemp += " 수술/시술";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[1, 7].Value) == true)
            {
                strTemp += " 진정 치료(검사)";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[1, 8].Value) == true)
            {
                strTemp += " 낙상 발생 ";
            }

            strRtn = strDrug + strDrug2 + strTemp;

            return strRtn;
        }

        private bool SAVE_WARNING(PsmhDb pDbCon, string ArgIPDNO, string ArgPano, string argACTDATE, string argACTTIME)
        {
            bool rtnVal = false;
            int intRowAffected = 0;
            string SqlErr = "";
            string SQL = "";

            string strPano = "";
            string strIPDNO = "";
            string strACTDATE = "";
            string strWARNING1 = "";
            string strWARNING2 = "";
            string strWARNING3 = "";
            string strWARNING4 = "";
            string strWARNING5 = "";
            string strWARNING6 = "";
            string strDRUG01 = "";
            string strDRUG02 = "";
            string strDRUG03 = "";
            string strDRUG04 = "";
            string strDRUG05 = "";
            string strDRUG06 = "";
            string strDRUG07 = "";
            string strDRUG08 = "";
            string strDRUG08ETC = "";

            DataTable dt = null;

            strPano = ArgPano;
            strIPDNO = ArgIPDNO;
            strACTDATE = argACTDATE;

            strWARNING1 = Convert.ToBoolean(SSWarning_Sheet1.Cells[1, 1].Value) == true ? "1" : "0";
            strWARNING2 = "0";
            strWARNING3 = "0";
            strWARNING4 = "0";
            strWARNING5 = "0";
            strWARNING6 = Convert.ToBoolean(SSWarning_Sheet1.Cells[0, 1].Value) == true ? "1" : "0";

            strDRUG01 = "0";
            strDRUG02 = "0";
            strDRUG03 = "0";
            strDRUG04 = "0";
            strDRUG05 = "0";

            strDRUG06 = "0";
            strDRUG07 = "0";
            strDRUG08 = "0";
            strDRUG08ETC = "";



            try
            {
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALL_WARNING ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgPano + "' ";

                if (gsWard == "HD" || gsWard == "ER")
                {

                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + argACTDATE + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND IPDNO = 0";
                }
                SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + ArgIPDNO;


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_FALL_WARNING_HISTORY (";
                    SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, ENTSABUN, ";
                    SQL = SQL + ComNum.VBLF + " GUBUN, WARNING1, WARNING2, WARNING3,";
                    SQL = SQL + ComNum.VBLF + " WARNING4, WARNING5, WARNING6, DRUG_01, DRUG_02,";
                    SQL = SQL + ComNum.VBLF + " DRUG_03, DRUG_04, DRUG_05, DRUG_06,";
                    SQL = SQL + ComNum.VBLF + " DRUG_07, DRUG_08, DRUG_08_ETC, DELDATE, ";
                    SQL = SQL + ComNum.VBLF + " DELSABUN, ACTTIME )";
                    SQL = SQL + ComNum.VBLF + " SELECT  ";
                    SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, ENTSABUN, ";
                    SQL = SQL + ComNum.VBLF + " GUBUN, WARNING1, WARNING2, WARNING3,";
                    SQL = SQL + ComNum.VBLF + " WARNING4, WARNING5, WARNING6, DRUG_01, DRUG_02,";
                    SQL = SQL + ComNum.VBLF + " DRUG_03, DRUG_04, DRUG_05, DRUG_06,";
                    SQL = SQL + ComNum.VBLF + " DRUG_07, DRUG_08, DRUG_08_ETC, SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + clsType.User.Sabun + ", ACTTIME";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALL_WARNING ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = " DELETE " + ComNum.DB_PMPA + "NUR_FALL_WARNING ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_FALL_WARNING (";
                SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, ENTSABUN, ";
                SQL = SQL + ComNum.VBLF + " GUBUN, WARNING1, WARNING2, WARNING3,";
                SQL = SQL + ComNum.VBLF + " WARNING4, WARNING5, WARNING6, DRUG_01, DRUG_02,";
                SQL = SQL + ComNum.VBLF + " DRUG_03, DRUG_04, DRUG_05, DRUG_06,";
                SQL = SQL + ComNum.VBLF + " DRUG_07, DRUG_08, DRUG_08_ETC, ACTTIME) VALUES ( ";
                SQL = SQL + ComNum.VBLF + "'" + strPano + "', " + strIPDNO + ", TO_DATE('" + strACTDATE + "','YYYY-MM-DD'), " + clsType.User.Sabun + ", ";
                SQL = SQL + ComNum.VBLF + "'1','" + strWARNING1 + "','" + strWARNING2 + "','" + strWARNING3 + "', ";
                SQL = SQL + ComNum.VBLF + "'" + strWARNING4 + "','" + strWARNING5 + "','" + strWARNING6 + "','" +  strDRUG01 + "','" + strDRUG02 + "', ";
                SQL = SQL + ComNum.VBLF + "'" + strDRUG03 + "','" + strDRUG04 + "','" + strDRUG05 + "','" + strDRUG06 + "', ";
                SQL = SQL + ComNum.VBLF + "'" + strDRUG07 + "','" + strDRUG08 + "','" + strDRUG08ETC + "','" + argACTTIME + "')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private bool SAVE_EVAL(PsmhDb pDbCon, string ArgIPDNO, string ArgPano, string argACTDATE, string argACTTIME)
        {
            bool rtnVal = false;
            int intRowAffected = 0;
            string SqlErr = "";
            string SQL = "";

            string strPano = "";
            string strIPDNO = "";
            string strACTDATE = "";
            string strDRUG_01 = "";
            string strDRUG_02 = "";
            string strDRUG_03 = "";
            string strDRUG_04 = "";
            string strDRUG_05 = "";
            string strDRUG_06 = "";
            string strDRUG_07 = "";
            string strDRUG_08 = "";
            string strDRUG_08ETC = "";
            string strPAT_CHANGE = "";
            string strFall = "";
            string strTRANFOR = "";
            string strRELEX = "";
            string strOP = "";
            string strPAT_CHANGE2 = "";

            DataTable dt = null;

            strPano = ArgPano;
            strIPDNO = ArgIPDNO;
            strACTDATE = argACTDATE;

            strDRUG_01 = Convert.ToBoolean(SSEval_Sheet1.Cells[0, 3].Value) == true ? "1" : "0";    //진정제
            strDRUG_02 = Convert.ToBoolean(SSEval_Sheet1.Cells[0, 4].Value) == true ? "1" : "0";    //수면제
            strDRUG_03 = Convert.ToBoolean(SSEval_Sheet1.Cells[0, 5].Value) == true ? "1" : "0";    //향정신성약물
            strDRUG_04 = Convert.ToBoolean(SSEval_Sheet1.Cells[0, 6].Value) == true ? "1" : "0";    //항우울제
            strDRUG_05 = "0";    //완하제
            strDRUG_06 = Convert.ToBoolean(SSEval_Sheet1.Cells[0, 7].Value) == true ? "1" : "0";    //이뇨제

            strDRUG_07 = "0";    //사용안함
            strDRUG_08 = "0";   //기타약물 사용안함
            strDRUG_08ETC = "";     //사용안함

            strPAT_CHANGE2 = Convert.ToBoolean(SSEval_Sheet1.Cells[1, 1].Value) == true ? "1" : "0";    //주요상태변화
            strPAT_CHANGE = Convert.ToBoolean(SSEval_Sheet1.Cells[1, 4].Value) == true ? "1" : "0";     //의식상태변화
            strTRANFOR = Convert.ToBoolean(SSEval_Sheet1.Cells[1, 5].Value) == true ? "1" : "0";    //전동 시
            strOP = Convert.ToBoolean(SSEval_Sheet1.Cells[1, 6].Value) == true ? "1" : "0";         //수술후/시술후
            strRELEX = Convert.ToBoolean(SSEval_Sheet1.Cells[1, 7].Value) == true ? "1" : "0";      //진검치료(감사
            strFall = Convert.ToBoolean(SSEval_Sheet1.Cells[1, 8].Value) == true ? "1" : "0";       //낙상바랭

            try
            {
                SQL = "";
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALL_EVAL ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgPano + "' ";

                if (gsWard == "HD" || gsWard == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + argACTDATE + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND IPDNO = 0";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + ArgIPDNO;
                }
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_FALL_EVAL_HISTORY (";
                    SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, ENTSABUN, ";
                    SQL = SQL + ComNum.VBLF + " GUBUN, DRUG_01, DRUG_02, DRUG_03, ";
                    SQL = SQL + ComNum.VBLF + " DRUG_04, DRUG_05, DRUG_06, DRUG_07, ";
                    SQL = SQL + ComNum.VBLF + " DRUG_08, DRUG_08ETC, PAT_CHANGE, FALL, ";
                    SQL = SQL + ComNum.VBLF + " TRANFOR, RELEX, OP, PAT_CHANGE2, ";
                    SQL = SQL + ComNum.VBLF + " DELDATE, DELSABUN, ACTTIME )";
                    SQL = SQL + ComNum.VBLF + " SELECT  ";
                    SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, ENTSABUN, ";
                    SQL = SQL + ComNum.VBLF + " GUBUN, DRUG_01, DRUG_02, DRUG_03, ";
                    SQL = SQL + ComNum.VBLF + " DRUG_04, DRUG_05, DRUG_06, DRUG_07, ";
                    SQL = SQL + ComNum.VBLF + " DRUG_08, DRUG_08ETC, PAT_CHANGE, FALL, ";
                    SQL = SQL + ComNum.VBLF + " TRANFOR, RELEX, OP, PAT_CHANGE2, ";
                    SQL = SQL + ComNum.VBLF + " SYSDATE, " + clsType.User.Sabun + ", ACTTIME";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALL_EVAL ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = " DELETE " + ComNum.DB_PMPA + "NUR_FALL_EVAL ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_FALL_EVAL (";
                SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, ENTSABUN, ";
                SQL = SQL + ComNum.VBLF + " GUBUN, DRUG_01, DRUG_02, DRUG_03, ";
                SQL = SQL + ComNum.VBLF + " DRUG_04, DRUG_05, DRUG_06, DRUG_07, ";
                SQL = SQL + ComNum.VBLF + " DRUG_08, DRUG_08ETC, PAT_CHANGE, FALL, ";
                SQL = SQL + ComNum.VBLF + " TRANFOR, RELEX, OP, PAT_CHANGE2, ACTTIME) VALUES ( ";
                SQL = SQL + ComNum.VBLF + "'" + strPano + "', " + strIPDNO + ", TO_DATE('" + strACTDATE + "','YYYY-MM-DD'), " + clsType.User.Sabun + ", ";
                SQL = SQL + ComNum.VBLF + "'1','" + strDRUG_01 + "','" + strDRUG_02 + "','" + strDRUG_03 + "', ";
                SQL = SQL + ComNum.VBLF + "'" + strDRUG_04 + "','" + strDRUG_05 + "','" + strDRUG_06 + "','" + strDRUG_07 + "', ";
                SQL = SQL + ComNum.VBLF + "'" + strDRUG_08 + "','" + strDRUG_08ETC + "','" + strPAT_CHANGE + "','" + strFall + "',";
                SQL = SQL + ComNum.VBLF + "'" + strTRANFOR + "','" + strRELEX + "','" + strOP + "','" + strPAT_CHANGE2 + "','" + argACTTIME + "')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private string NVL(string arg)
        {
            string strRtn = "";
            if (arg == "")
            {
                strRtn = "NULL";
            }
            else
            {
                strRtn = arg;
            }

            return strRtn;
        }

        /// <summary>
        ///     '작성일 : 2010-11-09
        ///     '작성자 : 김현욱
        ///     '용  도 : 전자인증이 된 사번인지 확인
        ///     '         전자인증이 되지 안았을 경우 false를 반환함
        /// </summary>
        /// <param name="argSABUN"></param>
        /// <returns></returns>
        /// 
        private bool READ_CERTIOK(string argSABUN)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;

            if (clsType.User.Sabun == "023515" || clsType.User.Grade == "EDPS")
            {
                rtnVal = true;
                return rtnVal;
            }

            Cursor.Current = Cursors.WaitCursor;

            rtnVal = false;

            try
            {
                SQL = "";
                SQL = "   SELECT B.SABUN, B.CERTIOK";
                SQL = SQL + ComNum.VBLF + "       FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_ERP + "INSA_MSTS B";
                SQL = SQL + ComNum.VBLF + "       WHERE A.SABUN = B.SABUN";
                SQL = SQL + ComNum.VBLF + "         AND A.TOIDAY IS NULL";
                SQL = SQL + ComNum.VBLF + "         AND B.CERTIOK = '1'";
                SQL = SQL + ComNum.VBLF + "         AND A.SABUN = '" + ComFunc.LPAD(argSABUN, 5, "0") + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }
                else
                {
                    rtnVal = false;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

        }

        private double EMRXML_INSERT(PsmhDb pDbCon, double ArgIPDNO, string strIT1, string strIT2, string strIT3, string strIT4, string strIT5, string strIT6, string strIT7, string strTOTAL, string strWARNING, double newEmrNo = 0)
        {
            string SqlErr = "";
            DataTable dt = null;
            string SQL = "";

            double nEMRNO = 0;
            string strInDate = "";
            string strInTime = "";
            string strPano = "";
            string stInDate = "";
            string strOutDate = "";
            string strOutTime = "";
            string strDeptCode = "";
            string strDrCd = "";
            string strWRITEDATE = "";
            string strWriteTime = "";
            string strXML = "";
            string strChartX1 = "";
            string strChartX2 = "";
            string strHead = "";
            string strTagHead = "";
            string strTagVal = "";
            string strTagTail = "";
            string strXMLCert = "";


            if (gsWard == "HD" || gsWard == "ER")
            {
                SQL = " SELECT PANO, SNAME, ACTDATE INDATE, ACTDATE OUTDATE, DRCODE, DEPTCODE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + GstrHelpCode + "' ";

                if (chkJ.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + (TxtJDATE.Value).AddDays(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + TxtJDATE.Text.Trim() + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "','YYYY-MM-DD') ";
                }
                SQL = SQL + ComNum.VBLF + "  AND  DEPTCODE = '" + gsWard + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE DESC";
            }
            else
            {
                SQL = " SELECT PANO, SNAME, INDATE, OUTDATE, DRCODE, DEPTCODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;
            }

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return nEMRNO;
            }
            if (dt.Rows.Count > 0)
            {
                strPano = dt.Rows[0]["PANO"].ToString().Trim();
                strInDate = VB.Left(dt.Rows[0]["INDATE"].ToString().Trim(), 10);
                strOutDate = VB.Left(dt.Rows[0]["OUTDATE"].ToString().Trim(), 10);
                strDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();
                strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
            }
            else
            {
                dt.Dispose();
                dt = null;
                MessageBox.Show("환자 정보가 없습니다. 전자 차트 전송에 실패하였습니다.", "확인");
                return nEMRNO;
            }
            dt.Dispose();
            dt = null;

            string strChartDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string strChartTime = ComQuery.CurrentDateTime(clsDB.DbCon, "T");

            if (chkJ.Checked == true)
            {
                strChartDate = TxtJDATE.Value.ToString("yyyyMMdd");
                strChartTime = TxtJTime.Value.ToString("HHmmss");
            }

            EmrPatient pAcp = SetEmrPatInfoOcs(clsDB.DbCon, strPano, (gsWard == "ER" || gsWard == "HD") == true ? "O" : "I", strInDate.Replace("-", ""), strDeptCode);
            EmrForm pForm = SerEmrFormUpdateNo(clsDB.DbCon, "2478");
            if (pForm.FmOLDGB == 1)
            {
                #region XML
                strXML = "";

                strHead = "<?xml version=" + VB.Chr(34) + "1.0" + VB.Chr(34) + " encoding=" + VB.Chr(34) + "UTF-8" + VB.Chr(34) + "?>";
                strChartX1 = "<chart>";
                strChartX2 = "</chart>";

                strXML = strHead + strChartX1;

                strTagHead = "<it1 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "연령" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT1;
                strTagTail = "]]></it1>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it2 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "성별" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT2;
                strTagTail = "]]></it2>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it3 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "질환" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT3;
                strTagTail = "]]></it3>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it4 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "인지장애" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT4;
                strTagTail = "]]></it4>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it5 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "환경적요인" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT5;
                strTagTail = "]]></it5>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it6 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "수술진정마취 후" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT6;
                strTagTail = "]]></it6>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it7 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "약물사용" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT6;
                strTagTail = "]]></it7>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it8 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "총점" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strTOTAL;
                strTagTail = "]]></it8>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it30 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "재평가" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strWARNING;
                strTagTail = "]]></it30>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strXMLCert = strXML;

                strXML = strXML + strChartX2;

                strXMLCert = strXML;

                SQL = "";
                SQL = "SELECT KOSMOS_EMR.GetEmrXmlNo() FunSeqNo FROM Dual";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return nEMRNO;
                }
                if (dt.Rows.Count > 0)
                {
                    nEMRNO = VB.Val(dt.Rows[0]["FunSeqNo"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                if (CREATE_EMR_XMLINSRT3(nEMRNO, "2478", clsType.User.IdNumber, strChartDate.Replace("-", ""), strChartTime.Replace(":", ""),
                   0, strPano, (gsWard == "ER" || gsWard == "HD") == true ? "O" : "I", strInDate.Replace("-", ""), "120000",
                   strOutDate.Replace("-", ""), strOutTime, strDeptCode, strDrCd, "0", 1, strXML) == false)
                {
                    nEMRNO = 0;
                }
                #endregion
            }
            else
            {
                Dictionary<string, string> strContent = new Dictionary<string, string>();
                strContent.Add("I0000037318", strWARNING);
                strContent.Add("I0000001075", strIT1);
                strContent.Add("I0000001382", strIT2);
                strContent.Add("I0000015717", strIT3);
                strContent.Add("I0000001638", strIT4);
                strContent.Add("I0000037699", strIT5);
                strContent.Add("I0000037700", strIT6);
                strContent.Add("I0000037701", strIT7);
                strContent.Add("I0000000427", strTOTAL);
                nEMRNO = SaveNurChartFlow(clsDB.DbCon, this, newEmrNo, pAcp, pForm, strChartDate.Replace("-", ""), strChartTime.Replace(":", ""), strContent);
            }

            return nEMRNO;
        }

        #region CLEAR


        private void SCREEN_CLEAR()
        {
            int i = 0;
            int j = 0;

            for (i = 5; i < SS1_Sheet1.ColumnCount; i++)
            {
                for (j = 0; j < SS1_Sheet1.RowCount; j++)
                {
                    SS1_Sheet1.Cells[j, i].Text = "";
                }

                SS1_Sheet1.ColumnHeader.Cells[0, i].Text = " ";
            }

            CLEAR_EVAL();
            CLEAR_WARNING();

            SSHistory_Sheet1.RowCount = 0;

            SS2_Sheet1.Cells[0, 1].Text = "";
            SS2_Sheet1.Cells[0, 3].Text = "";
            SS2_Sheet1.Cells[0, 5].Text = "";
            SS2_Sheet1.Cells[0, 7].Text = "";
            SS2_Sheet1.Cells[0, 9].Text = "";

            SS2_Sheet1.Cells[1, 1].Text = "";
            SS2_Sheet1.Cells[1, 3].Text = "";
            SS2_Sheet1.Cells[1, 5].Text = "";
            SS2_Sheet1.Cells[1, 7].Text = "";
            SS2_Sheet1.Cells[1, 9].Text = "";
        }

        private void CLEAR_EVAL()
        {
            SSEval_Sheet1.Cells[0, 3].Value = false;    //진정제
            SSEval_Sheet1.Cells[0, 4].Value = false;    //수면제
            SSEval_Sheet1.Cells[0, 5].Value = false;    //향정신성약물
            SSEval_Sheet1.Cells[0, 6].Value = false;    //항우울제

            SSEval_Sheet1.Cells[0, 7].Value = false;    //이뇨제

            SSEval_Sheet1.Cells[1, 1].Value = false;    //주요상태변화
            SSEval_Sheet1.Cells[1, 4].Value = false;    //의식상태변화
            SSEval_Sheet1.Cells[1, 5].Value = false;    //전동 시
            SSEval_Sheet1.Cells[1, 6].Value = false;    //수술후/시술후
            SSEval_Sheet1.Cells[1, 7].Value = false;    //진검치료(감사
            SSEval_Sheet1.Cells[1, 8].Value = false;    //낙상바랭
        }

        private void CLEAR_WARNING()
        {
            SSWarning_Sheet1.Cells[0, 1].Value = false;
            SSWarning_Sheet1.Cells[1, 1].Value = false;
        }

        #endregion

        private string MAKE_WARNING()
        {
            string strTemp = "";
            string strDrug = "";
            string strDrug2 = "";
            string strRtn = "";

            strDrug2 = "";

            if (Convert.ToBoolean(SSEval_Sheet1.Cells[0, 3].Value) == true)
            {
                if (strDrug == "")
                { strDrug = "낙상 초래 약물의 초기 투여 - "; }
                strDrug2 = strDrug2 + "진정제";

            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[0, 4].Value) == true)
            {
                if (strDrug == "")
                { strDrug = "낙상 초래 약물의 초기 투여 - "; }
                strDrug2 += "수면제";

            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[0, 5].Value) == true)
            {
                if (strDrug == "")
                { strDrug = "낙상 초래 약물의 초기 투여 - "; }
                strDrug2 += "향정신성 약물";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[0, 6].Value) == true)
            {
                if (strDrug == "")
                { strDrug = "낙상 초래 약물의 초기 투여 - "; }
                strDrug2 += "항우울제";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[0, 7].Value) == true)
            {
                if (strDrug == "")
                { strDrug = "낙상 초래 약물의 초기 투여 - "; }
                strDrug2 += "이뇨제";
            }


            strTemp = "";
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[1, 1].Value) == true)
            {
                strTemp += " 주요상태변화:간성 혼수, 알콜 섬망, 발작";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[1, 4].Value) == true)
            {
                strTemp += " 의식상태변화";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[1, 5].Value) == true)
            {
                strTemp += " 전동";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[1, 6].Value) == true)
            {
                strTemp += " 수술/시술";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[1, 7].Value) == true)
            {
                strTemp += " 진정 치료(검사)";
            }
            if (Convert.ToBoolean(SSEval_Sheet1.Cells[1, 8].Value) == true)
            {
                strTemp += " 낙상 발생 ";
            }

            strRtn = strDrug + strDrug2 + strTemp;

            return strRtn;
        }



        private void DATA_READ()
        {
            int i = 0;
            int nAge = 0;
            int nREAD = 0;
            string strACTDATE = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            if (chkJ.Checked == true)
            {
                strACTDATE = TxtJDATE.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                strACTDATE = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            }

            SCREEN_CLEAR();

            for (i = 1; i <= 26; i++)
            {
                SS1_Sheet1.Cells[i - 1, 4].Text = "";
            }


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT A.PANO, A.SNAME, A.SEX, A.AGE, A.DEPTCODE, ";
                SQL = SQL + ComNum.VBLF + "  (SELECT ROOMCODE FROM KOSMOS_PMPA.IPD_NEW_MASTER WHERE A.PANO = PANO  ";
                SQL = SQL + ComNum.VBLF + "     AND (JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') OR OUTDATE = TRUNC(SYSDATE)) AND ROWNUM = 1) ROOMCODE, ";
                SQL = SQL + ComNum.VBLF + "   A.ENTDATE, A.ENTSABUN, B.JUMIN1, A.ACTDATE, A.ROWID   ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_FALLHUMPDUMP_SCALE A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
                SQL = SQL + ComNum.VBLF + " WHERE A.PANO = B.PANO";
                if (gsWard == "HD" || gsWard == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + GstrHelpCode + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = 0";
                    if (chkJ.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + TxtJDATE.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE >= TO_DATE('" + TxtJDATE.Text + "','YYYY-MM-DD') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE >= TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "','YYYY-MM-DD') ";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = " + GstrHelpCode;
                    SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "','YYYY-MM-DD') ";
                }
                SQL = SQL + ComNum.VBLF + "   AND A.ENTDATE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "   ORDER BY ACTDATE DESC, ACTTIME DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS2_Sheet1.Cells[0, 1].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    SS2_Sheet1.Cells[0, 3].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    SS2_Sheet1.Cells[0, 5].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + dt.Rows[0]["AGE"].ToString().Trim();
                    SS2_Sheet1.Cells[0, 7].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    SS2_Sheet1.Cells[0, 9].Text = GstrHelpCode;

                    SS2_Sheet1.Cells[1, 1].Text = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    SS2_Sheet1.Cells[1, 3].Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    SS2_Sheet1.Cells[1, 5].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["ENTSABUN"].ToString().Trim());

                    if (gsWard == "HD" || gsWard == "ER")
                    {
                        SS2_Sheet1.Cells[1, 7].Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                    }
                    else
                    {
                        SS2_Sheet1.Cells[1, 7].Text = VB.Left(dt.Rows[0]["ACTDATE"].ToString().Trim(), 10);
                    }
                    SS2_Sheet1.Cells[1, 9].Text = dt.Rows[0]["ROWID"].ToString().Trim();
                    nAge = (int)VB.Val(dt.Rows[0]["AGE"].ToString().Trim());

                    dt.Dispose();
                    dt = null;

                    READ_MORSEFALL(GstrHelpCode);
                    READ_WARNING("1", "", GstrHelpCode);
                    READ_EVAL("1", "", GstrHelpCode);
                    READ_HISTORY_LIST(GstrHelpCode, "");


                    if (nAge >= 70)
                    {
                        SSWarning_Sheet1.Cells[1, 1].Value = true;
                    }
                    else if (nAge < 7)
                    {
                        SSWarning_Sheet1.Cells[0, 1].Value = true;
                    }
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    MessageBox.Show("신규 입력입니다.", "확인");

                    SCREEN_CLEAR();

                    if (gsWard == "HD" || gsWard == "ER")
                    {
                        SQL = "";
                        SQL = " SELECT A.PANO, A.SNAME, A.SEX, A.AGE, A.DEPTCODE, '' ROOMCODE, TO_CHAR(SYSDATE, 'YYYY-MM-DD') ENTDATE, ";
                        SQL = SQL + ComNum.VBLF + "'" + ComFunc.LPAD(clsType.User.Sabun, 5, "0") + "' ENTSABUN, B.JUMIN1, TO_CHAR(SYSDATE, 'YYYY-MM-DD') ACTDATE, A.ROWID   ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
                        SQL = SQL + ComNum.VBLF + " WHERE A.PANO = B.PANO";
                        SQL = SQL + ComNum.VBLF + "      AND A.PANO = " + GstrHelpCode;
                        SQL = SQL + ComNum.VBLF + "      AND A.DEPTCODE = '" + gsWard + "' ";

                        if (chkJ.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "      AND A.ACTDATE = TO_DATE('" + strACTDATE + "','YYYY-MM-DD') ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "      AND A.ACTDATE = TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "','YYYY-MM-DD') ";
                        }
                    }
                    else
                    {
                        SQL = "";
                        SQL = " SELECT A.PANO, A.SNAME, A.SEX, A.AGE, A.DEPTCODE, A.ROOMCODE, TO_CHAR(SYSDATE, 'YYYY-MM-DD') ENTDATE, ";
                        SQL = SQL + ComNum.VBLF + "'" + ComFunc.LPAD(clsType.User.Sabun, 5, "0") + "' ENTSABUN, B.JUMIN1, TO_CHAR(SYSDATE, 'YYYY-MM-DD') ACTDATE, A.ROWID   ";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
                        SQL = SQL + ComNum.VBLF + " WHERE A.PANO = B.PANO";
                        SQL = SQL + ComNum.VBLF + "      AND A.IPDNO = " + GstrHelpCode;
                    }

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        SS2_Sheet1.Cells[0, 1].Text = dt.Rows[0]["PANO"].ToString().Trim();
                        SS2_Sheet1.Cells[0, 3].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                        SS2_Sheet1.Cells[0, 5].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + dt.Rows[0]["AGE"].ToString().Trim();
                        SS2_Sheet1.Cells[0, 7].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                        SS2_Sheet1.Cells[0, 9].Text = GstrHelpCode;

                        SS2_Sheet1.Cells[1, 1].Text = dt.Rows[0]["JUMIN1"].ToString().Trim();
                        SS2_Sheet1.Cells[1, 3].Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                        SS2_Sheet1.Cells[1, 5].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["ENTSABUN"].ToString().Trim());

                        if (gsWard == "HD")
                        {
                            SS2_Sheet1.Cells[1, 7].Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                        }
                        else
                        {
                            SS2_Sheet1.Cells[1, 7].Text = dt.Rows[0]["ACTDATE"].ToString().Trim();
                        }
                        SS2_Sheet1.Cells[1, 9].Text = dt.Rows[0]["ROWID"].ToString().Trim();
                        nAge = (int)VB.Val(dt.Rows[0]["AGE"].ToString().Trim());


                        if (nAge >= 70)
                        {
                            SSWarning_Sheet1.Cells[1, 1].Value = true;
                        }
                        else if (nAge < 7)
                        {
                            SSWarning_Sheet1.Cells[0, 1].Value = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("환자 정보가 없습니다. 의료정보과 연락 요망", "확인");
                        dt.Dispose();
                        dt = null;
                    }

                    SQL = "";
                }


                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_MORSEFALL(string ArgIPDNO)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT JUMSU1, JUMSU2, JUMSU3, JUMSU4, ";
                SQL = SQL + ComNum.VBLF + " JUMSU5, JUMSU6, JUMSU7, TOTAL, ACTDATE, ACTTIME, ENTSABUN, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALLHUMPDUMP_SCALE ";
                if (gsWard == "ER" || gsWard == "HD")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgIPDNO + "' ";
                    SQL = SQL + ComNum.VBLF + "      AND IPDNO = 0";

                    if (chkJ.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE <= TO_DATE('" + (TxtJDATE.Value).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE >= TO_DATE('" + TxtJDATE.Text + "','YYYY-MM-DD') ";
                    }
                    else
                    {

                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE <= TO_DATE('" + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE >= TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "','YYYY-MM-DD') ";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;
                }

                SQL = SQL + ComNum.VBLF + " AND ENTDATE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE DESC, ACTTIME DESC  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                        if (i + 5 == 12)
                        {
                            break;
                        }

                        SS1_Sheet1.ColumnHeader.Cells[0, i + 5].Text = Convert.ToDateTime(dt.Rows[i]["ACTDATE"].ToString().Trim()).ToString("MM-dd")
                            + ComNum.VBLF + dt.Rows[i]["ACTTIME"].ToString().Trim();

                        SS1_Sheet1.Cells[0, i + 5].Text = dt.Rows[i]["JUMSU1"].ToString().Trim();
                        SS1_Sheet1.Cells[4, i + 5].Text = dt.Rows[i]["JUMSU2"].ToString().Trim();
                        SS1_Sheet1.Cells[6, i + 5].Text = dt.Rows[i]["JUMSU3"].ToString().Trim();
                        SS1_Sheet1.Cells[10, i + 5].Text = dt.Rows[i]["JUMSU4"].ToString().Trim();
                        SS1_Sheet1.Cells[13, i + 5].Text = dt.Rows[i]["JUMSU5"].ToString().Trim();
                        SS1_Sheet1.Cells[17, i + 5].Text = dt.Rows[i]["JUMSU6"].ToString().Trim();
                        SS1_Sheet1.Cells[20, i + 5].Text = dt.Rows[i]["JUMSU7"].ToString().Trim();
                        SS1_Sheet1.Cells[24, i + 5].Text = dt.Rows[i]["TOTAL"].ToString().Trim();
                        SS1_Sheet1.Cells[25, i + 5].Text = clsVbfunc.GetInSaName(clsDB.DbCon, VB.Val(dt.Rows[i]["ENTSABUN"].ToString().Trim()).ToString("00000"));
                        SS1_Sheet1.Cells[30, i + 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }


        }

        private void READ_WARNING(string ArgGubun, string ArgPano, string ArgIPDNO, string argROWID = "")
        {
            string strACTDATE = "";
            string strActTime = "";
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            CLEAR_WARNING();

            if (ArgPano == "")
            {
                ArgPano = SS2_Sheet1.Cells[0, 1].Text.Trim();
            }
            strACTDATE = SS2_Sheet1.Cells[1, 7].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, ENTSABUN, ";
                SQL = SQL + ComNum.VBLF + " GUBUN, WARNING1, WARNING6 ";

                if (ArgGubun == "1")
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALL_WARNING ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_FALL_WARNING_HISTORY ";
                }

                if (argROWID != "")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + argROWID + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgPano + "' ";

                    if (gsWard == "HD" || gsWard == "ER")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + Convert.ToDateTime(strACTDATE).AddDays(1) + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + strACTDATE + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND IPDNO = 0";
                    }
                }
                SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + ArgIPDNO;


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SSWarning_Sheet1.Cells[1, 1].Value = dt.Rows[0]["WARNING1"].ToString().Trim() == "1" ? true : false;
                    SSWarning_Sheet1.Cells[0, 1].Value = dt.Rows[0]["WARNING6"].ToString().Trim() == "1" ? true : false;
                }

                dt.Dispose();
                dt = null;

                if (ArgGubun == "1")
                {
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_EVAL(string ArgGubun, string ArgPano, string ArgIPDNO, string argROWID = "")
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strACTDATE = "";

            CLEAR_EVAL();

            if (ArgPano == "")
            {
                ArgPano = SS2_Sheet1.Cells[0, 1].Text.Trim();
            }

            strACTDATE = SS2_Sheet1.Cells[1, 7].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + " PANO, IPDNO, ACTDATE, ENTSABUN, ";
                SQL = SQL + ComNum.VBLF + " GUBUN, DRUG_01, DRUG_02, DRUG_03,";
                SQL = SQL + ComNum.VBLF + " DRUG_04, DRUG_05, DRUG_06, DRUG_07, ";
                SQL = SQL + ComNum.VBLF + " DRUG_08, DRUG_08ETC, PAT_CHANGE, FALL, ";
                SQL = SQL + ComNum.VBLF + " TRANFOR, RELEX, OP, PAT_CHANGE2 ";

                if (ArgGubun == "1")
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALL_EVAL ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALL_EVAL_HISTORY ";
                }

                if (argROWID != "")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + argROWID + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgPano + "' ";

                    if (gsWard == "HD" || gsWard == "ER")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND IPDNO = 0";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + Convert.ToDateTime(strACTDATE).AddDays(1) + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + strACTDATE + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE DESC, ACTTIME DESC ";
                    }

                    else
                    {
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + strACTDATE + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + ArgIPDNO;
                    }
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SSEval_Sheet1.Cells[0, 3].Value = dt.Rows[0]["DRUG_01"].ToString().Trim() == "1" ? true : false;        //진정제
                    SSEval_Sheet1.Cells[0, 4].Value = dt.Rows[0]["DRUG_02"].ToString().Trim() == "1" ? true : false;        //수면제
                    SSEval_Sheet1.Cells[0, 5].Value = dt.Rows[0]["DRUG_03"].ToString().Trim() == "1" ? true : false;        //향정신성 약물
                    SSEval_Sheet1.Cells[0, 6].Value = dt.Rows[0]["DRUG_04"].ToString().Trim() == "1" ? true : false;        //항우울제
                    SSEval_Sheet1.Cells[0, 7].Value = dt.Rows[0]["DRUG_06"].ToString().Trim() == "1" ? true : false;        //이뇨제

                    SSEval_Sheet1.Cells[1, 1].Value = dt.Rows[0]["PAT_CHANGE2"].ToString().Trim() == "1" ? true : false;    //주요상태변화
                    SSEval_Sheet1.Cells[1, 4].Value = dt.Rows[0]["PAT_CHANGE"].ToString().Trim() == "1" ? true : false;     //의식상태 변화

                    SSEval_Sheet1.Cells[1, 5].Value = dt.Rows[0]["TRANFOR"].ToString().Trim() == "1" ? true : false;        //전동 시
                    SSEval_Sheet1.Cells[1, 6].Value = dt.Rows[0]["OP"].ToString().Trim() == "1" ? true : false;             //수술/시술후
                    SSEval_Sheet1.Cells[1, 7].Value = dt.Rows[0]["RELEX"].ToString().Trim() == "1" ? true : false;          //진검치료(검사)
                    SSEval_Sheet1.Cells[1, 8].Value = dt.Rows[0]["FALL"].ToString().Trim() == "1" ? true : false;           //낙상 발생 시
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_HISTORY_LIST(string ArgIPDNO, string ArgPano)
        {
            string strACTDATE = "";
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            if (ArgPano == "")
            {
                ArgPano = SS2_Sheet1.Cells[0, 1].Text.Trim();
            }
            strACTDATE = SS2_Sheet1.Cells[1, 7].Text.Trim();

            SSHistory_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.ACTDATE, A.PANO, A.IPDNO, '1' GUBUN, A.ROWID ROWID1, B.ROWID ROWID2, A.ACTTIME  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_FALL_WARNING A, " + ComNum.DB_PMPA + "NUR_FALL_EVAL B ";
                if (gsWard == "HD" || gsWard == "ER")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + ArgPano + "' ";
                    SQL = SQL + ComNum.VBLF + "      AND A.ACTDATE >= TO_DATE('" + strACTDATE + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "      AND A.ACTDATE <= TO_DATE('" + Convert.ToDateTime(strACTDATE).AddDays(1) + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "      AND A.IPDNO = 0";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + ArgIPDNO;
                }
                SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PANO";
                SQL = SQL + ComNum.VBLF + "    AND A.ACTDATE = B.ACTDATE ";
                SQL = SQL + ComNum.VBLF + "    AND A.ACTTIME = B.ACTTIME ";
                SQL = SQL + ComNum.VBLF + " UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT A.ACTDATE, A.PANO, A.IPDNO, '2' GUBUN, A.ROWID ROWID1, B.ROWID ROWID2, A.ACTTIME ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_FALL_WARNING_HISTORY A, " + ComNum.DB_PMPA + "NUR_FALL_EVAL_HISTORY B ";

                if (gsWard == "HD" || gsWard == "ER")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + ArgPano + "' ";
                    SQL = SQL + ComNum.VBLF + "      AND A.ACTDATE >= TO_DATE('" + strACTDATE + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "      AND A.ACTDATE <= TO_DATE('" + Convert.ToDateTime(strACTDATE).AddDays(1) + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "      AND A.IPDNO = 0";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + ArgIPDNO;
                }
                SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PANO";
                SQL = SQL + ComNum.VBLF + "    AND A.ACTDATE = B.ACTDATE ";
                SQL = SQL + ComNum.VBLF + "    AND A.ACTTIME = B.ACTTIME ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE DESC, ACTTIME DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SSHistory_Sheet1.Rows.Count = dt.Rows.Count;
                    SSHistory_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SSHistory_Sheet1.Cells[i, 0].Text = VB.Left(dt.Rows[i]["ACTDATE"].ToString().Trim(), 10);
                        SSHistory_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        SSHistory_Sheet1.Cells[i, 2].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                        SSHistory_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID1"].ToString().Trim();
                        SSHistory_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID2"].ToString().Trim();
                        SSHistory_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ACTTIME"].ToString().Trim();
                        SSHistory_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            strTitle = "The Humpty Dumpty Fall Scale(낙상위험 사정 기록지)";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void btnOnePrint_Click(object sender, EventArgs e)
        {


            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;
            int i = 0;
            int j = 0;

            for (i = 0; i < SS1_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                for (j = 5; j <= 6; j++)
                {
                    SS1_Sheet1.Cells[i, j - 1].Text = "";
                }
            }

            strTitle = "Morse Fall Scale(낙상위험 사정 기록지)";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void SS1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strSCORE = "";
            string strROWID = "";
            int eRow = 0;

            if (e.ColumnHeader == true)
            {
                if (e.Column < 5)
                {
                    return;
                }

                strROWID = SS1_Sheet1.Cells[30, e.Column].Text.Trim();

                if (strROWID != "")
                {
                    DELETE_낙상(strROWID);
                    return;
                }
            }

            if (e.Column > 4)
            {
                return;
            }
            if (e.Row < 0)
            {
                return;
            }
            if (e.Row > 22)
            {
                return;
            }

            strSCORE = SS1_Sheet1.Cells[e.Row, 2].Text.Trim();

            switch (e.Row)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    eRow = 1;
                    break;
                case 4:
                case 5:
                    eRow = 5;
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                    eRow = 7;
                    break;
                case 10:
                case 11:
                case 12:
                    eRow = 11;
                    break;
                case 13:
                case 14:
                case 15:
                case 16:
                    eRow = 14;
                    break;
                case 17:
                case 18:
                case 19:
                    eRow = 18;
                    break;
                case 20:
                case 21:
                case 22:
                    eRow = 21;
                    break;
            }

            if (strSCORE == SS1_Sheet1.Cells[eRow - 1, 4].Text.Trim())
            {
                SS1_Sheet1.Cells[eRow - 1, 4].Text = "";
            }
            else
            {
                SS1_Sheet1.Cells[eRow - 1, 4].Text = strSCORE;
            }
        }

        private void DELETE_낙상(string argROWID)
        {
            double nEMRNO = 0;
            string strACTDATE = "";
            string strActTime = "";
            string strPano = "";
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            double dblEmrHisNo = 0;

            if (ComFunc.MsgBoxQ("해당 낙상기록을 삭제합니다.", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT EMRNO, ACTDATE, ACTTIME, PANO ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALLHUMPDUMP_SCALE";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + argROWID + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nEMRNO = VB.Val(dt.Rows[0]["EMRNO"].ToString().Trim());
                    strACTDATE = VB.Left(dt.Rows[0]["ACTDATE"].ToString().Trim(), 10);
                    strActTime = VB.Left(dt.Rows[0]["ACTTIME"].ToString().Trim(), 10);
                    strPano = dt.Rows[0]["PANO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " DELETE KOSMOS_PMPA.NUR_FALLHUMPDUMP_SCALE ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + argROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("차트 백업중 오류가 발생");
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (DELETE_WARNING(clsDB.DbCon, strPano, strACTDATE, strActTime) == false)
                {
                    return;
                }

                //'기존차트를 변경할 경우 : 백업 테이블로 백업을 하고 신규 data를 입력한다
                //'KOSMOS_EMR.EMRXMLHISTORY_HISTORYNO_SEQ
                if (nEMRNO > 0)
                {
                    dblEmrHisNo = GetSequencesNo("" + ComNum.DB_EMR + "EMRXMLHISNO");

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY";
                    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                    SQL = SQL + ComNum.VBLF + "      '" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-").Replace("-", "") + "',";
                    SQL = SQL + ComNum.VBLF + "      '" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M", "-").Replace(":", "") + "', '"
                        + ComFunc.LPAD(clsType.User.Sabun, 5, "0") + "',CERTNO";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("차트 백업중 오류가 발생");
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("차트 백업중 오류가 발생");
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("차트 백업중 오류가 발생");
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    #region 신규
                    if (SaveChartMastHis(clsDB.DbCon, nEMRNO.ToString(), dblEmrHisNo,
                        ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-").Replace("-", ""),
                        ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M", ":").Replace(":", ""),
                        "C", "") != "OK")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    #endregion
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            DATA_READ();
        }

        private bool DELETE_WARNING(PsmhDb pDbCon, string ArgPano, string argACTDATE, string argACTTIME)
        {
            bool rtnVal = false;
            int intRowAffected = 0;
            string SqlErr = "";
            string SQL = "";
            try
            {
                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "NUR_FALL_WARNING ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + argACTDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ACTTIME = '" + argACTTIME + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTTIME IS NOT NULL ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    MessageBox.Show("차트 백업중 오류가 발생" + SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "NUR_FALL_EVAL ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + argACTDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ACTTIME = '" + argACTTIME + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTTIME IS NOT NULL ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    MessageBox.Show("차트 백업중 오류가 발생" + SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "NUR_FALL_WARNING_HISTORY ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + argACTDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ACTTIME = '" + argACTTIME + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTTIME IS NOT NULL ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    MessageBox.Show("차트 백업중 오류가 발생" + SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "NUR_FALL_EVAL_HISTORY ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + argACTDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ACTTIME = '" + argACTTIME + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTTIME IS NOT NULL ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    MessageBox.Show("차트 백업중 오류가 발생" + SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private double GetSequencesNo(string FunSeqName)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            double rtnVal = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT " + FunSeqName + "() FunSeqNo FROM Dual";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = VB.Val(dt.Rows[0]["FunSeqNo"].ToString().Trim());
                }
                else
                {
                    MessageBox.Show("EMR 전송중 에러 발생");
                    rtnVal = 0;
                }

                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

        }

        private void SSHistory_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strGubun = "";
            string strPano = "";
            string strIPDNO = "";
            string strACTDATE = "";
            string strROWID = "";
            string strRowid2 = "";

            if (e.Row < 1)
            {
                return;
            }
            if (e.Column < 1)
            {
                return;
            }

            strACTDATE = SSHistory_Sheet1.Cells[e.Row, 0].Text.Trim();
            strPano = SSHistory_Sheet1.Cells[e.Row, 1].Text.Trim();
            strIPDNO = SSHistory_Sheet1.Cells[e.Row, 2].Text.Trim();
            strROWID = SSHistory_Sheet1.Cells[e.Row, 3].Text.Trim();
            strRowid2 = SSHistory_Sheet1.Cells[e.Row, 4].Text.Trim();
            strGubun = SSHistory_Sheet1.Cells[e.Row, 6].Text.Trim();

            READ_WARNING(strGubun, strPano, strIPDNO, strROWID);
            READ_EVAL(strGubun, strPano, strIPDNO, strRowid2);

            if (strGubun == "1")
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
        }

        private bool CREATE_EMR_XMLINSRT3(double EmrNo, string FormNo, string strSabun, string strChartDate, string strChartTime, int iAcpNo,
            string strPtNo, string strInOutCls, string strMedFrDate, string strMedFrTime, string strMedEndDate, string strMedEndTime,
            string strMedDeptCd, string strMedDrCd, string strMibiCheck, int iUpdateNo, string strXML)
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strWRITEDATE = "";
            string strWRITETIME = "";

            //차팅일자
            if (strChartDate != "" && strChartDate.IndexOf("-") != -1)
            {
                strChartDate = Convert.ToDateTime(strChartDate).ToString("yyyyMMdd");
            }
            if (strChartTime != "" && strChartTime.IndexOf(":") != -1)
            {
                strChartTime = Convert.ToDateTime(strChartTime).ToString("HHmmss");
            }
            //입실일자
            if (strMedFrDate != "" && strMedFrDate.IndexOf("-") != -1)
            {
                strMedFrDate = Convert.ToDateTime(strMedFrDate).ToString("yyyyMMdd");
            }
            if (strMedFrTime != "" && strMedFrTime.IndexOf(":") != -1)
            {
                strMedFrTime = Convert.ToDateTime(strMedFrTime).ToString("HHmmss");
            }
            //퇴실일자
            if (strMedEndDate != "" && strMedEndDate.IndexOf("-") != -1)
            {
                strMedEndDate = Convert.ToDateTime(strMedEndDate).ToString("yyyyMMdd");
            }
            if (strMedEndTime != "" && strMedEndTime.IndexOf(":") != -1)
            {
                strMedEndTime = Convert.ToDateTime(strMedEndTime).ToString("HHmmss");
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYYMMDD') AS CURRENTDATE, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(SYSDATE,'HH24MISS') AS CURRENTTIME ";
                SQL = SQL + ComNum.VBLF + "    FROM DUAL ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strWRITEDATE = dt.Rows[0]["CURRENTDATE"].ToString().Trim();
                    strWRITETIME = dt.Rows[0]["CURRENTTIME"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            int Result = 0;
            OracleCommand cmd = new OracleCommand();
            PsmhDb pDbCon = clsDB.DbCon;

            cmd.Connection = pDbCon.Con;
            cmd.InitialLONGFetchSize = 1000;
            cmd.CommandText = "KOSMOS_EMR.XMLINSRT3";
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("p_EMRNO", OracleDbType.Double, 0, EmrNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_FORMNO", OracleDbType.Double, 0, VB.Val(FormNo), ParameterDirection.Input);
                cmd.Parameters.Add("p_USEID", OracleDbType.Varchar2, 8, strSabun, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTDATE", OracleDbType.Varchar2, 8, strChartDate, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTTIME", OracleDbType.Varchar2, 6, strChartTime, ParameterDirection.Input);
                cmd.Parameters.Add("p_ACPNO", OracleDbType.Double, 0, iAcpNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_PTNO", OracleDbType.Varchar2, 9, strPtNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_INOUTCLS", OracleDbType.Varchar2, 1, strInOutCls, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDFRDATE", OracleDbType.Varchar2, 8, strMedFrDate, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDFRTIME", OracleDbType.Varchar2, 6, strMedFrTime, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDENDDATE", OracleDbType.Varchar2, 8, strMedEndDate, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDENDTIME", OracleDbType.Varchar2, 6, strMedEndTime, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDDEPTCD", OracleDbType.Varchar2, 4, strMedDeptCd, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDDRCD", OracleDbType.Varchar2, 6, strMedDrCd, ParameterDirection.Input);
                cmd.Parameters.Add("p_MIBICHECK", OracleDbType.Varchar2, 1, "0", ParameterDirection.Input);
                cmd.Parameters.Add("p_WRITEDATE", OracleDbType.Varchar2, 8, strWRITEDATE, ParameterDirection.Input);
                cmd.Parameters.Add("p_WRITETIME", OracleDbType.Varchar2, 6, strWRITETIME, ParameterDirection.Input);
                cmd.Parameters.Add("p_UPDATENO", OracleDbType.Int32, 0, iUpdateNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTXML", OracleDbType.Clob, VB.Len(strXML), strXML, ParameterDirection.Input);

                Result = cmd.ExecuteNonQuery();

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception e)
            {
                return rtnVal;
            }
        }

        private void SS2_CellClick(object sender, CellClickEventArgs e)
        {
            if (gsWard == "ER")
            {
                frmCalendar2 frmCalendar2X = new frmCalendar2();
                frmCalendar2X.ShowDialog();
                frmCalendar2X = null;
                SS2_Sheet1.Cells[1, 7].Text = clsPublic.GstrCalDate;
            }
        }
    }
}

