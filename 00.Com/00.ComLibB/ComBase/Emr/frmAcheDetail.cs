using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using ComDbB;
using Oracle.ManagedDataAccess.Client;
using static ComBase.clsEmrFunc;

namespace ComBase
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-03-08
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= d:\psmh\nurse\nrinfo\Frm통증기록지Detail.frm" >> frmAcheDetail.cs 폼이름 재정의" />
    /// 
    public partial class frmAcheDetail : Form
    {
        double FNIPDNO = 0;
        string FstrPANO = "";
        string FstrSname = "";
        string FstrSex = "";
        string FstrAge = "";
        string FstrDeptCode = "";
        string FstrRoomCode = "";
        string FstrInDate = "";
        string FstrER = "";

        string GstrHelpCode = "";
        string GstrHelpName = "";

        public frmAcheDetail()
        {
            InitializeComponent();
        }

        public frmAcheDetail(string strHelpCode, string strHelpName)
        {
            InitializeComponent();

            GstrHelpCode = strHelpCode;
            GstrHelpName = strHelpName;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            if (DELETE_DATA() == false)
            {
                return;
            }

            READ_DATA();
        }

        private void Clear()
        {
            SS2_Sheet1.RowCount = 0;
            SS1_Sheet1.Cells[0, 0].Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            SS1_Sheet1.Cells[0, 1].Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
            //SS1_Sheet1.Cells[0, 14].Text = "";

            btnSave.Enabled = true;
            btnDelete.Enabled = false;
        }

        private void chkCompER_CheckedChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";

                if (FNIPDNO == 0 && FstrDeptCode == "ER")
                {
                    SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_ER_PATIENT SET ";
                    if (chkCompER.Checked == true)
                    {
                        SQL += ComNum.VBLF + " PAIN_COMP = '1'";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + " PAIN_COMP = NULL ";
                    }
                    SQL += ComNum.VBLF + " WHERE JDATE = TO_DATE('" + FstrInDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND PANO = '" + FstrPANO + "' ";

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
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            READ_DATA();
        }

        private void READ_DATA()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT PANO,  IPDNO, WRITEDATE, WRITESABUN, ";
                SQL += ComNum.VBLF + "  EMRNO, ACTDATE, ACTTIME, SNAME, ";
                SQL += ComNum.VBLF + "  SEX, AGE, DEPTCODE, ROOMCODE, ";
                SQL += ComNum.VBLF + "  CYCLE, REGION, ASPECT, DETERIORATION, ";
                SQL += ComNum.VBLF + "  MITIGATION, SCORE, TOOLS, DURATION, ";
                SQL += ComNum.VBLF + " DRUG, NODRUG, ROWID, TIMES, INDATE";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_PAIN_SCALE ";

                if (FstrER == "OK")
                {
                    SQL += ComNum.VBLF + " WHERE IPDNO = 0";
                    SQL += ComNum.VBLF + "   AND PANO = '" + FstrPANO + "' ";
                    SQL += ComNum.VBLF + "   AND INDATE = TO_DATE('" + FstrInDate + "','YYYY-MM-DD') ";

                }
                else
                {
                    SQL += ComNum.VBLF + " WHERE IPDNO = " + FNIPDNO;
                }

                SQL += ComNum.VBLF + "      AND ACTDATE >= TRUNC(SYSDATE-60)";
                SQL += ComNum.VBLF + " ORDER BY ACTDATE DESC, ACTTIME DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }


                SS2_Sheet1.Rows.Count = dt.Rows.Count;
                SS2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SS2_Sheet1.Cells[i, 0].Text = VB.Left(dt.Rows[i]["ACTDATE"].ToString().Trim(), 10) + " " + dt.Rows[i]["ACTTIME"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CYCLE"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["REGION"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ASPECT"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DETERIORATION"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["MITIGATION"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["SCORE"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["TOOLS"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DURATION"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 9].Text = dt.Rows[i]["TIMES"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DRUG"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 11].Text = dt.Rows[i]["NODRUG"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 12].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 13].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 14].Text = READ_SABUN(dt.Rows[i]["WRITESABUN"].ToString().Trim());
                    SS2_Sheet1.Cells[i, 15].Text = dt.Rows[i]["INDATE"].ToString().Trim();

                    if (i == 0)
                    {
                        SET_DATA(dt.Rows[i]["ROWID"].ToString().Trim(), "체크");
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

        private string READ_SABUN(string arg)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strVal = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";

                SQL = " SELECT B.NAME, A.KORNAME ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.INSA_MST A, KOSMOS_PMPA.BAS_BUSE B";
                SQL = SQL + ComNum.VBLF + " WHERE A.BUSE = B.BUCODE ";
                SQL = SQL + ComNum.VBLF + "    AND A.SABUN = '" + ComFunc.LPAD(arg, 5, "0") + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return strVal;
                }
                strVal = dt.Rows[0]["NAME"].ToString().Trim() + "/" + dt.Rows[0]["KORNAME"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return strVal;

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
                return strVal;

            }
        }

        private void SET_DATA(string argROWID, string arg = "")
        {

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            btnSave.Enabled = false;
            btnDelete.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT WRITESABUN, EMRNO, ACTDATE, ACTTIME, ";
                SQL += ComNum.VBLF + " CYCLE, REGION, ASPECT, DETERIORATION, ";
                SQL += ComNum.VBLF + " MITIGATION, SCORE, TOOLS, DURATION,";
                SQL += ComNum.VBLF + " DRUG, NODRUG, TIMES, INDATE";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_PAIN_SCALE ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + argROWID + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    return;
                }

                if (VB.Val(dt.Rows[0]["WRITESABUN"].ToString().Trim()) != VB.Val(clsType.User.Sabun) && arg == "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("수정 및 삭제는 작성자만 가능합니다.");
                    //  return;
                }
                else
                {
                    btnSave.Enabled = true;
                    btnDelete.Enabled = true;

                    SS1_Sheet1.RowCount = 1;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    SS1_Sheet1.Cells[0, 3].Text = dt.Rows[0]["REGION"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 4].Text = dt.Rows[0]["ASPECT"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 5].Text = dt.Rows[0]["DETERIORATION"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 6].Text = dt.Rows[0]["MITIGATION"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 7].Text = dt.Rows[0]["SCORE"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 8].Text = dt.Rows[0]["TOOLS"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 9].Text = dt.Rows[0]["DURATION"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 10].Text = dt.Rows[0]["TIMES"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 11].Text = dt.Rows[0]["DRUG"].ToString().Trim();
                    SS1_Sheet1.Cells[0, 12].Text = dt.Rows[0]["NODRUG"].ToString().Trim();
                    //SS1_Sheet1.Cells[0, 15].Text = dt.Rows[0]["INDATE"].ToString().Trim(); VB스프레드 디자인에 16칼럼 존재 하지 않음

                    if (arg != "")
                    {
                        SS1_Sheet1.Cells[0, 0].Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                        SS1_Sheet1.Cells[0, 1].Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");
                        SS1_Sheet1.Cells[0, 2].Text = "재평가";
                    }
                    else
                    {
                        SS1_Sheet1.Cells[0, 13].Text = argROWID.ToString();
                        SS1_Sheet1.Cells[0, 14].Text = dt.Rows[0]["EMRNO"].ToString().Trim();
                        SS1_Sheet1.Cells[0, 0].Text = dt.Rows[0]["ACTDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[0, 1].Text = dt.Rows[0]["ACTTIME"].ToString().Trim();
                        SS1_Sheet1.Cells[0, 2].Text = dt.Rows[0]["CYCLE"].ToString().Trim();
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

        private bool DELETE_DATA()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            double nEMRNO = 0;
            double dblEmrHisNo = 0;
            string strROWID = "";

            Cursor.Current = Cursors.WaitCursor;

            strROWID = SS1_Sheet1.Cells[0, 13].Text;
            nEMRNO = VB.Val(SS1_Sheet1.Cells[0, 14].Text);

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDate = (ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).Replace("-", "");
                string strCurTime = (ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M")).Replace(":", "");

                if (nEMRNO > 0)
                {
                    //'기존차트를 변경할 경우 : 백업 테이블로 백업을 하고 신규 data를 입력한다
                    //'KOSMOS_EMR.EMRXMLHISTORY_HISTORYNO_SEQ

                    dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO");

                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO KOSMOS_EMR.EMRXMLHISTORY";
                    SQL += ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL += ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL += ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                    SQL += ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL += ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL += ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL += ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                    SQL += ComNum.VBLF + "      '" + (ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).Replace("-", "") + "',";
                    SQL += ComNum.VBLF + "      '" + (ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M")).Replace(":", "") + "', '" + clsType.User.Sabun + "',CERTNO";
                    SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML";
                    SQL += ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("챠트 백업 중에 오류가 발생함" + ComNum.VBLF + SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL += ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXML";
                    SQL += ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("챠트 백업 중에 오류가 발생함" + ComNum.VBLF + SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL += ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXMLMST";
                    SQL += ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("챠트 백업 중에 오류가 발생함" + ComNum.VBLF + SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (SaveChartMastHis(clsDB.DbCon, nEMRNO.ToString(), dblEmrHisNo, strCurDate, strCurTime, "C", "") != "OK")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("챠트 백업 중에 오류가 발생함" + ComNum.VBLF + SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                }



                SQL = " INSERT INTO KOSMOS_PMPA.NUR_PAIN_SCALE_HISTORY ";
                SQL += ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.NUR_PAIN_SCALE";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = " DELETE KOSMOS_PMPA.NUR_PAIN_SCALE";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (Save_Data() == false)
            {
                return;
            }
            READ_DATA();


        }

        private double EMRXML_INSERT(double ArgIPDNO, string ArgDate, string ArgTime, string nTOT1, string nTOT2, string nTOT3, string nTOT4,
                                string nTOT5, string nTOT6, string nTOT7, string nTOT8, string nTOT9, string nTOT10, string NTOT11, double newEmrNo = 0)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
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
            double DouVal = 0;

            string strNowDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string strNowtime = ComQuery.CurrentDateTime(clsDB.DbCon, "T");

            try
            {
                if (FstrER == "OK")
                {
                    SQL = " SELECT PANO, SNAME, BDATE OUTDATE, BDATE INDATE, DRCODE, DEPTCODE ";
                    SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_MASTER ";
                    SQL += ComNum.VBLF + " WHERE DEPTCODE = 'ER'";
                    SQL += ComNum.VBLF + "   AND PANO = '" + FstrPANO + "' ";
                    SQL += ComNum.VBLF + "   AND BDATE = TO_DATE('" + FstrInDate + "','YYYY-MM-DD')";
                }
                else
                {
                    SQL = " SELECT PANO, SNAME, INDATE, OUTDATE, DRCODE, DEPTCODE ";
                    SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER";
                    SQL += ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return DouVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strPano = dt.Rows[0]["PANO"].ToString().Trim();
                    strInDate = VB.Left(dt.Rows[0]["INDATE"].ToString().Trim(), 10);
                    strOutDate = dt.Rows[0]["OUTDATE"].ToString().Trim();
                    strDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();
                    strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                }
                else
                {
                    ComFunc.MsgBox("환자 정보가 없습니다 전자차트 전송에 실패하였습니다.", "확인");
                    dt.Dispose();
                    dt = null;
                    return DouVal;
                }
                dt.Dispose();
                dt = null;

                EmrPatient pAcp = SetEmrPatInfoOcs(clsDB.DbCon, strPano, (FstrER == "OK" ? "O" : "I"), strInDate, strDeptCode);
                EmrForm pForm = SerEmrFormUpdateNo(clsDB.DbCon, "2281");

                if (pForm.FmOLDGB == 1)
                {
                    #region XML
                    strXML = "";

                    strHead = "<?xml version=" + VB.Chr(34) + "1.0" + VB.Chr(34) + " encoding=" + VB.Chr(34) + "UTF-8" + VB.Chr(34) + "?>";
                    strChartX1 = "<chart>";
                    strChartX2 = "</chart>";

                    strXML = strHead + strChartX1;

                    strTagHead = "<it1 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "평가주기" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT1;
                    strTagTail = "]]></it1>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;


                    strTagHead = "<it2 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "통증위치" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT2;
                    strTagTail = "]]></it2>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;


                    strTagHead = "<it3 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "통증양상" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT3;
                    strTagTail = "]]></it3>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;


                    strTagHead = "<it4 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "악화요인" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT4;
                    strTagTail = "]]></it4>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;


                    strTagHead = "<it5 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "완화요인" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT5;
                    strTagTail = "]]></it5>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;


                    strTagHead = "<it6 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "통증강도" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT6;
                    strTagTail = "]]></it6>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;


                    strTagHead = "<it7 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "평가도구" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT7;
                    strTagTail = "]]></it7>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;


                    strTagHead = "<it8 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "통증빈도" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT8;
                    strTagTail = "]]></it8>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;


                    strTagHead = "<it9 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "중재-약물" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT9;
                    strTagTail = "]]></it9>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;


                    strTagHead = "<it10 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "중재-비약물" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = nTOT10;
                    strTagTail = "]]></it10>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;

                    strTagHead = "<it11 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "지속시간" + VB.Chr(34) + "><![CDATA[";
                    strTagVal = NTOT11;
                    strTagTail = "]]></it11>";
                    strXML = strXML + strTagHead + strTagVal + strTagTail;

                    strXMLCert = strXML;

                    strXML = strXML + strChartX2;

                    strXMLCert = strXML;

                    SQL = "";
                    SQL = "SELECT " + ComNum.DB_EMR + "GetEmrXmlNo() FunSeqNo FROM Dual";

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


                    if (CREATE_EMR_XMLINSRT3(nEMRNO, "2281", clsType.User.IdNumber, ArgDate.Replace("-", ""), ArgTime.Replace(":", ""),
                    0, strPano, (FstrER == "OK" ? "O" : "I"), strInDate.Replace("-", ""), "120000",
                    strOutDate.Replace("-", ""), strOutTime, strDeptCode, strDrCd, "0", 1, strXML) == false)
                    {
                        nEMRNO = 0;
                    }
                    #endregion
                }
                else
                {
                    #region NEW
                    Dictionary<string, string> strContent = new Dictionary<string, string>();
                    strContent.Add("I0000037702", nTOT1);
                    strContent.Add("I0000029881", nTOT2);
                    strContent.Add("I0000013278", nTOT3);
                    strContent.Add("I0000036171", nTOT4);
                    strContent.Add("I0000036172", nTOT5);
                    strContent.Add("I0000027172", nTOT6);
                    strContent.Add("I0000037703", nTOT7);
                    strContent.Add("I0000028671", nTOT8);
                    strContent.Add("I0000000456", NTOT11);
                    strContent.Add("I0000037704", nTOT9);
                    strContent.Add("I0000037705", nTOT10);
                    nEMRNO = SaveNurChartFlow(clsDB.DbCon, this, newEmrNo, pAcp, pForm, ArgDate.Replace("-", ""), ArgTime.Replace(":", ""), strContent);
                    #endregion
                }


                DouVal = nEMRNO;
                return DouVal;
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
                return DouVal;
            }

        }

        private bool Save_Data()
        {

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strPano = "";
            string strIPDNO = "";
            string strWRITEDATE = "";
            string strWRITESABUN = "";
            double nEMRNO = 0;
            string strACTDATE = "";
            string strActTime = "";
            string strSName = "";
            string strSex = "";
            string strAge = "";
            string strDeptCode = "";
            string strROOMCODE = "";
            string strCYCLE = "";
            string strREGION = "";
            string strASPECT = "";
            string strDETERIORATION = "";
            string strMITIGATION = "";
            string strSCORE = "";
            string strTOOLS = "";
            string strDURATION = "";
            string strTIMES = "";
            string str중재약물 = "";
            string str중재비약물 = "";
            string strROWID = "";
            double dblEmrHisNo = 0;

            if (clsType.User.Sabun == "")
            {
                ComFunc.MsgBox("사번 오류입니다.", "확인");
                return false;
            }

            strACTDATE = SS1_Sheet1.Cells[0, 0].Text;
            strActTime = SS1_Sheet1.Cells[0, 1].Text;
            strCYCLE = SS1_Sheet1.Cells[0, 2].Text;
            strREGION = SS1_Sheet1.Cells[0, 3].Text;
            strASPECT = SS1_Sheet1.Cells[0, 4].Text;
            strDETERIORATION = SS1_Sheet1.Cells[0, 5].Text;
            strMITIGATION = SS1_Sheet1.Cells[0, 6].Text;
            strSCORE = SS1_Sheet1.Cells[0, 7].Text;
            strTOOLS = SS1_Sheet1.Cells[0, 8].Text;
            strDURATION = SS1_Sheet1.Cells[0, 9].Text;
            strTIMES = SS1_Sheet1.Cells[0, 10].Text;
            str중재약물 = SS1_Sheet1.Cells[0, 11].Text;
            str중재비약물 = SS1_Sheet1.Cells[0, 12].Text;
            strROWID = SS1_Sheet1.Cells[0, 13].Text;
            nEMRNO = VB.Val(SS1_Sheet1.Cells[0, 14].Text);

            if (strACTDATE == "" || strActTime == "")
            {
                ComFunc.MsgBox("작성일시가 공란입니다. ", "확인");
                return false;
            }
            if (strSCORE == "")
            {
                ComFunc.MsgBox("점수가 공란입니다. ", "확인");
                return false;
            }
            if (strTOOLS == "")
            {
                ComFunc.MsgBox("평가도구가 공란입니다. ", "확인");
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                EmrForm pForm = SerEmrFormUpdateNo(clsDB.DbCon, "2281");

                if (nEMRNO > 0 && pForm.FmOLDGB == 1)
                {
                    //'기존차트를 변경할 경우 : 백업 테이블로 백업을 하고 신규 data를 입력한다
                    //'KOSMOS_EMR.EMRXMLHISTORY_HISTORYNO_SEQ
                    dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO");

                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO KOSMOS_EMR.EMRXMLHISTORY";
                    SQL += ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL += ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL += ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                    SQL += ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL += ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL += ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL += ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                    SQL += ComNum.VBLF + "      '" + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D").Replace("-", "") + "',";
                    SQL += ComNum.VBLF + "      '" + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M").Replace(":", "") + "', '" + clsType.User.Sabun + "',CERTNO";
                    SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML";
                    SQL += ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr, ComNum.VBLF + "챠트 백업 중에 오류가 발생함");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }


                    SQL = "";
                    SQL += ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXML";
                    SQL += ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr, ComNum.VBLF + "챠트 백업 중에 오류가 발생함");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }


                    SQL = "";
                    SQL += ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXMLMST";
                    SQL += ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr, ComNum.VBLF + "챠트 백업 중에 오류가 발생함");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                if (clsType.User.Sabun != "04349")
                {
                    if (pForm.FmOLDGB == 1)
                    {
                        nEMRNO = 0;
                    }
                    nEMRNO = EMRXML_INSERT(FNIPDNO, strACTDATE, strActTime, strCYCLE, strREGION, strASPECT,
                            strDETERIORATION, strMITIGATION, strSCORE, strTOOLS, strDURATION, str중재약물, str중재비약물, strTIMES, nEMRNO);

                    if (nEMRNO == 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("EMR 저장 중 오류가 발생했습니다.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                if (strROWID != "")
                {
                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_PAIN_SCALE_HISTORY ";
                    SQL = SQL + ComNum.VBLF + " SELECT * FROM " + ComNum.DB_PMPA + "NUR_PAIN_SCALE";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr, ComNum.VBLF + "챠트 백업 중에 오류가 발생함");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }


                    SQL = "";
                    SQL = " DELETE " + ComNum.DB_PMPA + "NUR_PAIN_SCALE";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr, ComNum.VBLF + "챠트 백업 중에 오류가 발생함");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }


                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_PAIN_SCALE( ";
                SQL = SQL + ComNum.VBLF + " PANO, IPDNO, WRITEDATE, WRITESABUN,";
                SQL = SQL + ComNum.VBLF + " EMRNO, ACTDATE, ACTTIME, SNAME, ";
                SQL = SQL + ComNum.VBLF + " SEX, AGE, DEPTCODE, ROOMCODE, ";
                SQL = SQL + ComNum.VBLF + " CYCLE, REGION, ASPECT, DETERIORATION,";
                SQL = SQL + ComNum.VBLF + " MITIGATION, SCORE, TOOLS, DURATION, ";
                SQL = SQL + ComNum.VBLF + " DRUG, NODRUG, TIMES, INDATE) VALUES ( ";
                SQL = SQL + ComNum.VBLF + "'" + FstrPANO + "', " + FNIPDNO + ", SYSDATE, " + clsType.User.Sabun + ", ";
                SQL = SQL + ComNum.VBLF + nEMRNO + ", TO_DATE('" + strACTDATE + "','YYYY-MM-DD'), '" + strActTime + "', '" + FstrSname + "', ";
                SQL = SQL + ComNum.VBLF + "'" + FstrSex + "','" + FstrAge + "','" + FstrDeptCode + "'," + FstrRoomCode + ", ";
                SQL = SQL + ComNum.VBLF + "'" + strCYCLE + "','" + strREGION + "','" + strASPECT + "','" + strDETERIORATION + "', ";
                SQL = SQL + ComNum.VBLF + "'" + strMITIGATION + "', " + strSCORE + ",'" + strTOOLS + "','" + strDURATION + "',";
                SQL = SQL + ComNum.VBLF + "'" + str중재약물 + "','" + str중재비약물 + "','" + strTIMES + "',TO_DATE('" + FstrInDate + "','YYYY-MM-DD'))";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr, ComNum.VBLF + "챠트 백업 중에 오류가 발생함");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }



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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAcheDetail_Load(object sender, EventArgs e)
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

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            SS2_Sheet1.Columns[12].Visible = false;
            SS2_Sheet1.Columns[13].Visible = false;
            SS2_Sheet1.Columns[16].Visible = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                chkCompER.Checked = false;
                FstrER = "";

                if (GstrHelpName != "")
                {
                    FstrER = "OK";
                    FstrInDate = GstrHelpCode;
                    chkCompER.Visible = true;

                    SQL = "";
                    SQL = " SELECT 0 IPDNO, A.PANO, SNAME, A.SEX, A.AGE, 'ER' DEPTCODE, 0 ROOMCODE, NVL(A.PAIN_COMP, '0') PAIN_COMP";
                    SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_PATIENT A, KOSMOS_PMPA.BAS_PATIENT B";
                    SQL += ComNum.VBLF + " WHERE JDATE = TO_DATE('" + GstrHelpName + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "   AND A.PANO = B.PANO";
                    SQL += ComNum.VBLF + "   AND A.PANO = '" + GstrHelpCode + "' ";
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT IPDNO, PANO, SNAME, SEX, AGE, DEPTCODE, ROOMCODE, '' PAIN_COMP ";
                    SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                    SQL += ComNum.VBLF + " WHERE IPDNO = " + GstrHelpCode;
                    chkCompER.Visible = false;
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }
                FNIPDNO = VB.Val(dt.Rows[0]["IPDNO"].ToString().Trim());
                FstrPANO = dt.Rows[0]["PANO"].ToString().Trim();
                FstrSname = dt.Rows[0]["SNAME"].ToString().Trim();
                FstrSex = dt.Rows[0]["SEX"].ToString().Trim();
                FstrAge = dt.Rows[0]["AGE"].ToString().Trim();
                FstrDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                FstrRoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();

                if (Convert.ToBoolean(VB.Val(dt.Rows[0]["PAIN_COMP"].ToString().Trim()) == 1 ? 1 : 0) == true)
                {
                    chkCompER.Checked = true;
                }
                else
                {
                    chkCompER.Checked = false;
                }

                dt.Dispose();
                dt = null;

                SSPanel44.Text =
                    "※ 등록번호:" + FstrPANO + "(" + FstrSname + ")   성별 / 나이 : " + FstrSex + " / " + FstrAge + "    진료과 / 호실 : " + FstrDeptCode + " / " + FstrRoomCode;

                READ_DATA();

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

        private void SS2_ComboSelChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string strTEMP1 = "";
            string strTEMP2 = "";

            if (e.Column == 3)
            {
                strTEMP1 = SS1_Sheet1.Cells[0, 2].Text.Trim();
                strTEMP2 = SS1_Sheet1.Cells[0, 3].Text.Trim();

                if (strTEMP1 == "초기평가" && strTEMP2 == "통증 없음")
                {
                    SS1_Sheet1.Cells[0, 4].Text = "통증 없음";
                    SS1_Sheet1.Cells[0, 5].Text = "통증 없음";
                    SS1_Sheet1.Cells[0, 6].Text = "통증 없음";
                    SS1_Sheet1.Cells[0, 8].Text = "NRS";
                    SS1_Sheet1.Cells[0, 9].Text = "무";
                    SS1_Sheet1.Cells[0, 10].Text = "무";
                    SS1_Sheet1.Cells[0, 11].Text = "없음";
                    SS1_Sheet1.Cells[0, 12].Text = "해당없음";

                    //TODO
                    GstrHelpCode = "";
                    GstrHelpName = "";

                    frmPainScore1 frmPainScore1x = new frmPainScore1();
                    frmPainScore1x.rSetHelpCode += FrmPainScore1x_rSetHelpCode;
                    frmPainScore1x.ShowDialog();

                    SS1_Sheet1.Cells[0, 7].Text = GstrHelpName;
                }
            }

            if (e.Column == 9)
            {
                GstrHelpCode = "";
                GstrHelpName = "";

                switch (SS1_Sheet1.Cells[0, 8].Text.Trim())
                {
                    case "NRS":
                    case "FPRS":
                        frmPainScore1 frmPainScore1xx = new frmPainScore1();
                        frmPainScore1xx.rSetHelpCode += FrmPainScore1x_rSetHelpCode;
                        frmPainScore1xx.ShowDialog();

                        if (GstrHelpCode == "1")
                        {
                            SS1_Sheet1.Cells[0, 8].Text = "NRS";
                        }
                        else
                        {
                            SS1_Sheet1.Cells[0, 8].Text = "FPRS";
                        }
                        break;
                    case "FLACC":
                        frmPainScore2 frmPainScore2x = new frmPainScore2();
                        frmPainScore2x.rSetHelpCode += FrmPainScore2x_rSetHelpCode;
                        frmPainScore2x.ShowDialog();
                        break;
                    case "NIPS":
                        frmPainScore2 frmPainScore2xx = new frmPainScore2();
                        frmPainScore2xx.rSetHelpCode += FrmPainScore2xx_rSetHelpCode;
                        frmPainScore2xx.ShowDialog();
                        break;
                    case "CPOT":
                        frmPainScore3 frmPainScore3x = new frmPainScore3();
                        frmPainScore3x.rSetHelpCode += FrmPainScore3x_rSetHelpCode;
                        frmPainScore3x.ShowDialog();
                        break;
                }

                SS1_Sheet1.Cells[0, 7].Text = GstrHelpName;
            }
        }

        private void FrmPainScore3x_rSetHelpCode(string strHelpCode)
        {
            GstrHelpName = strHelpCode;
        }

        private void FrmPainScore2xx_rSetHelpCode(string strHelpCode)
        {
            GstrHelpName = strHelpCode;
        }

        private void FrmPainScore2x_rSetHelpCode(string strHelpCode)
        {
            GstrHelpName = strHelpCode;
        }

        private void FrmPainScore1x_rSetHelpCode(string strHelpCode, string strHelpName)
        {
            GstrHelpCode = strHelpCode;
            GstrHelpName = strHelpName;
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == 0 && e.Row == 0)
            {
                frmCalendar1 frmCalendar1X = new frmCalendar1();
                frmCalendar1X.StartPosition = FormStartPosition.CenterParent;
                frmCalendar1X.ShowDialog();

                SS1_Sheet1.Cells[0, 0].Text = clsPublic.GstrCalDate;
            }
        }

        private void SS2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0)
            {
                return;
            }

            SET_DATA(SS2_Sheet1.Cells[e.Row, 12].Text, "");

        }

        private void FrmPainScore4X_rSetHelpCode(string strHelpCode)
        {
            GstrHelpName = strHelpCode;
        }

        private void FrmPainScore3X_rSetHelpCode(string strHelpCode)
        {
            GstrHelpName = strHelpCode;
        }

        private void FrmPainScore2X_rSetHelpCode(string strHelpCode)
        {
            GstrHelpName = strHelpCode;
        }

        private void FrmPainScore1X_rSetHelpCode(string strHelpCode, string strHelpName)
        {
            GstrHelpCode = strHelpCode;
            GstrHelpName = strHelpName;
        }

        private void SS1_ComboCloseUp(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string strTemp1 = "";
            string strTemp2 = "";

            if (e.Column == 3)
            {
                strTemp1 = SS1_Sheet1.Cells[0, 2].Text.Trim();
                strTemp2 = SS1_Sheet1.Cells[0, 3].Text.Trim();

                if (strTemp1 == "초기평가" && strTemp2 == "통증 없음")
                {
                    SS1_Sheet1.Cells[0, 4].Text = "통증 없음";
                    SS1_Sheet1.Cells[0, 5].Text = "통증 없음";
                    SS1_Sheet1.Cells[0, 6].Text = "통증 없음";
                    SS1_Sheet1.Cells[0, 8].Text = "NRS";
                    SS1_Sheet1.Cells[0, 9].Text = "무";
                    SS1_Sheet1.Cells[0, 10].Text = "무";
                    SS1_Sheet1.Cells[0, 11].Text = "없음";
                    SS1_Sheet1.Cells[0, 12].Text = "해당없음";

                    GstrHelpCode = "";
                    GstrHelpName = "";

                    //Frm통증스코어1
                    frmPainScore1 frmPainScore1X = new frmPainScore1();
                    frmPainScore1X.rSetHelpCode += FrmPainScore1X_rSetHelpCode;
                    frmPainScore1X.ShowDialog();
                    frmPainScore1X = null;

                    SS1_Sheet1.Cells[0, 7].Text = GstrHelpName;
                }
            }
            else if (e.Column == 8)
            {
                GstrHelpCode = "";
                GstrHelpName = "";

                switch (SS1_Sheet1.Cells[0, 8].Text.Trim())
                {
                    case "NRS":
                    case "FPRS":
                        //Frm통증스코어1
                        frmPainScore1 frmPainScore1X = new frmPainScore1();
                        frmPainScore1X.rSetHelpCode += FrmPainScore1X_rSetHelpCode;
                        frmPainScore1X.ShowDialog();
                        frmPainScore1X = null;
                        if (GstrHelpCode == "1")
                        {
                            SS1_Sheet1.Cells[0, 8].Text = "NRS";
                        }
                        else
                        {
                            SS1_Sheet1.Cells[0, 8].Text = "FPRS";
                        }
                        break;
                    case "FLACC":
                        //Frm통증스코어2.Show 1
                        frmPainScore2 frmPainScore2X = new frmPainScore2();
                        frmPainScore2X.rSetHelpCode += FrmPainScore2X_rSetHelpCode;
                        frmPainScore2X.ShowDialog();
                        frmPainScore2X = null;
                        break;
                    case "NIPS":
                        //Frm통증스코어3.Show 1
                        frmPainScore3 frmPainScore3X = new frmPainScore3();
                        frmPainScore3X.rSetHelpCode += FrmPainScore3X_rSetHelpCode;
                        frmPainScore3X.ShowDialog();
                        frmPainScore3X = null;
                        break;
                    case "CPOT":
                        //Frm통증스코어4.Show 1
                        frmPainScore4 frmPainScore4X = new frmPainScore4();
                        frmPainScore4X.rSetHelpCode += FrmPainScore4X_rSetHelpCode;
                        frmPainScore4X.ShowDialog();
                        frmPainScore4X = null;
                        break;
                }

                SS1_Sheet1.Cells[0, 7].Text = GstrHelpName;
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
    }
}
