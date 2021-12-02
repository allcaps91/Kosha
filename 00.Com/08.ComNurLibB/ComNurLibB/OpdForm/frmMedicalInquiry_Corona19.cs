using ComBase; //기본 클래스
using ComEmrBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComNurLibB
{

    public partial class frmMedicalInquiry_Corona19 : Form
    {
        string FstrPano = "";
        string FstrSName = "";
        string FstrDept = "";
        string FstrDrCode = "";
        string FstrBDate = "";
        string FstrROWID = "";

        public frmMedicalInquiry_Corona19()
        {
            InitializeComponent();
        }

        public frmMedicalInquiry_Corona19(string strPaNo, string strPaName, string strDept, string strDrCode, string strBDate)
        {
            InitializeComponent();

            this.FstrPano = strPaNo;
            this.FstrSName = strPaName;
            this.FstrDept = strDept;
            this.FstrDrCode = strDrCode;
            this.FstrBDate = strBDate;
        }

        private bool SaveErInfoXML()
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            double dblEmrNo = 0;

            string strHead = "";
            string strChartX1 = "";
            string strChartX2 = "";
            string strXML = "";
            string strXMLCert = "";
            string strTagHead = "";
            string strTagTail = "";
            string strTagVal = "";

            double dblEmrHisNo = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                ComFunc.ReadSysDate(clsDB.DbCon);
                dblEmrNo = VB.Val(txtEMRNO.Text);

                if (dblEmrNo != 0)
                {
                    #region // EMR 백업
                    dblEmrHisNo = clsOpdNr.GetSequencesNo(clsDB.DbCon, "KOSMOS_EMR.EMRXMLHISNO");

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_EMR.EMRXMLHISTORY";
                    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                    SQL = SQL + ComNum.VBLF + "      '" + clsPublic.GstrSysDate.Replace("-", "") + "',";
                    SQL = SQL + ComNum.VBLF + "      '" + clsPublic.GstrSysTime.Replace(":", "") + "', '" + clsType.User.Sabun + "',CERTNO";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }


                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                    #endregion
                }

                #region // XML 생성
                strXML = "";
                strHead = "<?xml version=" + VB.Chr(34) + "1.0" + VB.Chr(34) + " encoding=" + VB.Chr(34) + "UTF-8" + VB.Chr(34) + "?>";
                strChartX1 = "<chart>";
                strChartX2 = "</chart>";


                strXML = strHead + strChartX1;


                //'동반자/정보제공자
                strTagHead = @"<ta1 type=""textArea"" label=""Progress""><![CDATA[";
                strTagVal = VB.Trim(txtSendData.Text);
                strTagTail = "]]></ta1>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;
                strXML = strXML + strChartX2;
                strXMLCert = strXML;
                #endregion

                #region // EMR 시퀀스 생성
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT KOSMOS_EMR.GetEmrXmlNo() FunSeqNo FROM Dual";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    dblEmrNo = VB.Val(dt.Rows[0]["FunSeqNo"].ToString().Trim());
                }
                #endregion


                string strChartDate = "";
                string strChartTime = "";

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYYMMDD') AS CURRENTDATE, ";
                SQL = SQL + ComNum.VBLF + "             TO_CHAR(SYSDATE,'HH24MISS') AS CURRENTTIME ";
                SQL = SQL + ComNum.VBLF + "    FROM DUAL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strChartDate = dt.Rows[0]["CURRENTDATE"].ToString().Trim();
                    strChartTime = dt.Rows[0]["CURRENTTIME"].ToString().Trim();
                }

                //'면허번호로 의사코드 가져오기
                string strDrCd = "";
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_OCS.OCS_DOCTOR WHERE DOCCODE = " + clsType.User.Sabun;

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();
                }


                if (clsNurse.CREATE_EMR_XMLINSRT3(dblEmrNo, "963", clsType.User.IdNumber,
                strChartDate, strChartTime, 0, FstrPano, "O", VB.Replace(FstrBDate, "-", ""), "120000",
                "", "", FstrDept, FstrDrCode, "0", 1, strXML) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("KOSMOS_EMR.EMRXML 테이블에 자료 추가시 에러 발생함");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                txtEMRNO.Text = Convert.ToString(dblEmrNo);

                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE KOSMOS_PMPA.OPD_DEPT_MUNJIN SET     ";
                SQL = SQL + ComNum.VBLF + " EMRNO = " + dblEmrNo;
                SQL = SQL + ComNum.VBLF + " , CHK = 'Y'";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + FstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "  AND BDATE =TO_DATE('" + FstrBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND DEPTCODE ='" + FstrDept + "' ";
                SQL = SQL + ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='')";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (clsType.User.JobMan == "간호사")
            {
                ComFunc.MsgBox("간호사일 경우 경과기록을 작성할 수 없습니다.");
                return;
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Build_Data();

            if (txtSendData.Text.Trim() != "")
            {
                EmrPatient pAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, FstrPano, "O", FstrBDate.Replace("-", ""), FstrDept);
                EmrForm pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "963");
                if (pForm.FmOLDGB == 1)
                {
                    SaveErInfoXML();
                }
                else
                {
                    double dblNewEmrNo = 0;
                    clsEmrQuery.SaveNewProgress(clsDB.DbCon, this, pAcp, VB.Val(txtEMRNO.Text), txtSendData.Text.Trim(), ref dblNewEmrNo, false);
                    txtEMRNO.Text = Convert.ToString(dblNewEmrNo);
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (FstrROWID != "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "OPD_DEPT_MUNJIN_CORONA19 SET ";
                    SQL = SQL + ComNum.VBLF + "CHK ='Y' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";
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

                //clsDB.setRollbackTran(clsDB.DbCon);  //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                MessageBox.Show("전송되었습니다.");
                btnExit.PerformClick();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (btnSaveClick() == true)
            {
                this.Close();
            }
        }

        private bool btnSaveClick()
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            //string strH = ""; //키
            //string strW = ""; //몸무게
            //string strMun1 = "";
            string strMun1_1 = "";
            string strMun1_2 = "";
            //string strMun2 = "";
            string strMun2_1 = "";
            string strMun2_2 = "";
            //string strMun3 = "";
            string strMun3_1 = "";
            string strMun3_2 = "";
            //string strMun4 = "";
            string strMun4_1 = "";
            string strMun4_2 = "";
            //string strMun5 = "";
            string strMun5_1 = "";
            string strMun5_2 = "";
            string strMun5_2_memo = "";
            //string strMun6 = "";
            string strMun6_1 = "";
            string strMun6_1_memo = "";
            string strMun6_2 = "";
            string strMun6_3 = "";
            string strMun6_4 = "";
            string strMun6_5 = "";
            string strMun6_6 = "";
            string strMun6_7 = "";
            string strMun6_8 = "";
            string strMun6_9 = "";
            string strMun6_9_memo = "";
            //string strMun7 = "";
            string strMun7_memo = "";

            
            if (chkNo_1_1.Checked == true)
                strMun1_1 = "1";

            if (chkNo_1_2.Checked == true)
                strMun1_2 = "1";

            if (chkNo_2_1.Checked == true)
                strMun2_1 = "1";

            if (chkNo_2_2.Checked == true)
                strMun2_2 = "1";

            if (chkNo_3_1.Checked == true)
                strMun3_1 = "1";

            if (chkNo_3_2.Checked == true)
                strMun3_2 = "1";

            if (chkNo_4_1.Checked == true)
                strMun4_1 = "1";

            if (chkNo_4_2.Checked == true)
                strMun4_2 = "1";

            if (chkNo_5_1.Checked == true)
                strMun5_1 = "1";

            if (chkNo_5_2.Checked == true)
                strMun5_2 = "1";

            strMun5_2_memo = txtNo_5_Memo.Text.Trim();

            if (chkNo_6_1.Checked == true)
                strMun6_1 = "1";

            strMun6_1_memo = txtNo_6_1_memo.Text.Trim();

            if (chkNo_6_2.Checked == true)
                strMun6_2 = "1";

            if (chkNo_6_3.Checked == true)
                strMun6_3 = "1";

            if (chkNo_6_4.Checked == true)
                strMun6_4 = "1";

            if (chkNo_6_5.Checked == true)
                strMun6_5 = "1";

            if (chkNo_6_6.Checked == true)
                strMun6_6 = "1";

            if (chkNo_6_7.Checked == true)
                strMun6_7 = "1";

            if (chkNo_6_8.Checked == true)
                strMun6_8 = "1";

            if (chkNo_6_9.Checked == true)
                strMun6_9 = "1";

            strMun6_9_memo = txtNo_6_9_memo.Text.Trim();

            strMun7_memo = txtNo_7_memo.Text.Trim();


            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_DEPT_MUNJIN_CORONA19 ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + FstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + FstrDept + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + FstrBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE ='')";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (FstrROWID == "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_PMPA.OPD_DEPT_MUNJIN_CORONA19 (";
                    SQL = SQL + ComNum.VBLF + " BDate , Pano, DeptCode, DrCode, ";
                    SQL = SQL + ComNum.VBLF + " ENTSABUN, ENTDATE, ";
                    SQL = SQL + ComNum.VBLF + " MUN1_1, MUN1_2, MUN2_1, MUN2_2, ";
                    SQL = SQL + ComNum.VBLF + " MUN3_1, MUN3_2, MUN4_1, MUN4_2, ";
                    SQL = SQL + ComNum.VBLF + " MUN5_1, MUN5_2, MUN5_2_MEMO, ";
                    SQL = SQL + ComNum.VBLF + " MUN6_1, MUN6_1_MEMO, MUN6_2, MUN6_3, ";
                    SQL = SQL + ComNum.VBLF + " MUN6_4, MUN6_5, MUN6_6, MUN6_7, ";
                    SQL = SQL + ComNum.VBLF + " MUN6_8, MUN6_9, MUN6_9_MEMO, MUN7 ) VALUES (";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + FstrBDate + "','YYYY-MM-DD'),'" + FstrPano + "','" + FstrDept + "','" + FstrDrCode + "',";
                    SQL = SQL + ComNum.VBLF + clsType.User.Sabun + ",SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + " '" + strMun1_1 + "', '" + strMun1_2 + "','" + strMun2_1 + "','" + strMun2_2 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun3_1 + "', '" + strMun3_2 + "','" + strMun4_1 + "','" + strMun4_2 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun5_1 + "', '" + strMun5_2 + "','" + strMun5_2_memo + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun6_1 + "', '" + strMun6_1_memo + "','" + strMun6_2 + "','" + strMun6_3 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun6_4 + "', '" + strMun6_5 + "','" + strMun6_6 + "','" + strMun6_7 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun6_8 + "', '" + strMun6_9 + "','" + strMun6_9_memo + "','" + strMun7_memo + "')";
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "OPD_DEPT_MUNJIN_CORONA19 SET ";
                    SQL = SQL + ComNum.VBLF + "ENTSABUN =" + clsType.User.Sabun + ", ";
                    SQL = SQL + ComNum.VBLF + "ENTDATE = SYSDATE,  ";
                    SQL = SQL + ComNum.VBLF + " MUN1_1 = '" + strMun1_1 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN1_2 = '" + strMun1_2 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN2_1 = '" + strMun2_1 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN2_2 = '" + strMun2_2 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN3_1 = '" + strMun3_1 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN3_2 = '" + strMun3_2 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN4_1 = '" + strMun4_1 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN4_2 = '" + strMun4_2 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN5_1 = '" + strMun5_1 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN5_2 = '" + strMun5_2 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN5_2_MEMO = '" + strMun5_2_memo + "',";
                    SQL = SQL + ComNum.VBLF + " MUN6_1 = '" + strMun6_1 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN6_1_MEMO = '" + strMun6_1_memo + "',";
                    SQL = SQL + ComNum.VBLF + " MUN6_2 = '" + strMun6_2 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN6_3 = '" + strMun6_3 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN6_4 = '" + strMun6_4 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN6_5 = '" + strMun6_5 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN6_6 = '" + strMun6_6 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN6_7 = '" + strMun6_7 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN6_8 = '" + strMun6_8 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN6_9 = '" + strMun6_9 + "',";
                    SQL = SQL + ComNum.VBLF + " MUN6_9_MEMO = '" + strMun6_9_memo + "',";
                    SQL = SQL + ComNum.VBLF + " MUN7 = '" + strMun7_memo + "'";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("정상 처리되었습니다.");
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


        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        void Search_Data()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Screen_Clear();

            Read_Munjin_Data("1", FstrPano, FstrDept, FstrBDate);

            DATA_BULID_PRE();
        }

        private void DATA_BULID_PRE()
        {

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                
                chkNo_1.Enabled = false;
                chkNo_2.Enabled = false;
                chkNo_3.Enabled = false;
                chkNo_4.Enabled = false;
                chkNo_5.Enabled = false;
                chkNo_6.Enabled = false;
                chkNo_7.Enabled = false;
                
                btnChart.Enabled = false;
                btnAll.Enabled = false;
                txtSendData.Text = "";

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DRCODE ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "  WHERE DOCCODE = " + clsType.User.Sabun;

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                chkNo_1.Enabled = true;
                chkNo_2.Enabled = true;
                chkNo_3.Enabled = true;
                chkNo_4.Enabled = true;
                chkNo_5.Enabled = true;
                chkNo_6.Enabled = true;
                chkNo_7.Enabled = true;
                

                chkNo_1.Checked = true;
                chkNo_2.Checked = true;
                chkNo_3.Checked = true;
                chkNo_4.Checked = true;
                chkNo_5.Checked = true;
                chkNo_6.Checked = true;
                chkNo_7.Checked = true;
                
                btnChart.Enabled = true;
                btnAll.Enabled = true;

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Screen_Clear()
        {
            //2
            chkNo_1.Checked = false;
            chkNo_1_1.Checked = false;
            chkNo_1_2.Checked = false;
            chkNo_2.Checked = false;
            chkNo_2_1.Checked = false;
            chkNo_2_2.Checked = false;
            chkNo_3.Checked = false;
            chkNo_3_1.Checked = false;
            chkNo_3_2.Checked = false;
            chkNo_4.Checked = false;
            chkNo_4_1.Checked = false;
            chkNo_4_2.Checked = false;
            chkNo_5.Checked = false;
            chkNo_5_1.Checked = false;
            chkNo_5_2.Checked = false;
            txtNo_5_Memo.Text = "";
            chkNo_6.Checked = false;
            chkNo_6_1.Checked = false;
            txtNo_6_1_memo.Text = "";
            chkNo_6_2.Checked = false;
            chkNo_6_3.Checked = false;
            chkNo_6_4.Checked = false;
            chkNo_6_5.Checked = false;
            chkNo_6_6.Checked = false;
            chkNo_6_7.Checked = false;
            chkNo_6_8.Checked = false;
            chkNo_6_9.Checked = false;
            txtNo_6_9_memo.Text = "";
            txtNo_7_memo.Text = "";

            txtSendData.Text = "";
            txtEMRNO.Text = "";
        }

        void Read_Munjin_Data(string argSTS, string argPano, string argDeptCode, string argBDate)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            Screen_Clear();

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT BDATE, PANO, DEPTCODE, DRCODE, DELDATE, CHK,";
                SQL = SQL + ComNum.VBLF + " ENTSABUN, ENTDATE, ROWID, EMRNO, ";
                SQL = SQL + ComNum.VBLF + " MUN1_1, MUN1_2, MUN2_1, MUN2_2, ";
                SQL = SQL + ComNum.VBLF + " MUN3_1, MUN3_2, MUN4_1, MUN4_2, ";
                SQL = SQL + ComNum.VBLF + " MUN5_1, MUN5_2, MUN5_2_MEMO, ";
                SQL = SQL + ComNum.VBLF + " MUN6_1, MUN6_1_MEMO, MUN6_2, MUN6_3, ";
                SQL = SQL + ComNum.VBLF + " MUN6_4, MUN6_5, MUN6_6, MUN6_7, ";
                SQL = SQL + ComNum.VBLF + " MUN6_8, MUN6_9, MUN6_9_MEMO, MUN7 ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_DEPT_MUNJIN_CORONA19 ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + argDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + argBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE ='')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = SQL + ComNum.VBLF + " MUN1_1, MUN1_2, MUN2_1, MUN2_2, ";
                    SQL = SQL + ComNum.VBLF + " MUN3_1, MUN3_2, MUN4_1, MUN4_2, ";
                    SQL = SQL + ComNum.VBLF + " MUN5_1, MUN5_2, MUN5_2_MEMO, ";
                    SQL = SQL + ComNum.VBLF + " MUN6_1, MUN6_1_MEMO, MUN6_2, MUN6_3, ";
                    SQL = SQL + ComNum.VBLF + " MUN6_4, MUN6_5, MUN6_6, MUN6_7, ";
                    SQL = SQL + ComNum.VBLF + " MUN6_8, MUN6_9, MUN6_9_MEMO, MUN7 ";

                    if (argSTS != "2")
                    {
                        FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    }
                    if (dt.Rows[0]["MUN1_1"].ToString().Trim() == "1")
                    {
                        chkNo_1_1.Checked = true;
                    }
                    if (dt.Rows[0]["MUN1_2"].ToString().Trim() == "1")
                    {
                        chkNo_1_2.Checked = true;
                    }
                    if (dt.Rows[0]["MUN2_1"].ToString().Trim() == "1")
                    {
                        chkNo_2_1.Checked = true;
                    }
                    if (dt.Rows[0]["MUN2_2"].ToString().Trim() == "1")
                    {
                        chkNo_2_2.Checked = true;
                    }
                    if (dt.Rows[0]["MUN3_1"].ToString().Trim() == "1")
                    {
                        chkNo_3_1.Checked = true;
                    }
                    if (dt.Rows[0]["MUN3_2"].ToString().Trim() == "1")
                    {
                        chkNo_3_2.Checked = true;
                    }
                    if (dt.Rows[0]["MUN4_1"].ToString().Trim() == "1")
                    {
                        chkNo_4_1.Checked = true;
                    }
                    if (dt.Rows[0]["MUN4_2"].ToString().Trim() == "1")
                    {
                        chkNo_4_2.Checked = true;
                    }
                    if (dt.Rows[0]["MUN5_1"].ToString().Trim() == "1")
                    {
                        chkNo_5_1.Checked = true;
                    }
                    if (dt.Rows[0]["MUN5_2"].ToString().Trim() == "1")
                    {
                        chkNo_5_2.Checked = true;
                    }
                    txtNo_5_Memo.Text = dt.Rows[0]["MUN5_2_MEMO"].ToString().Trim();

                    if (dt.Rows[0]["MUN6_1"].ToString().Trim() == "1")
                    {
                        chkNo_6_1.Checked = true;
                    }
                    txtNo_6_1_memo.Text = dt.Rows[0]["MUN6_1_MEMO"].ToString().Trim();

                    if (dt.Rows[0]["MUN6_2"].ToString().Trim() == "1")
                    {
                        chkNo_6_2.Checked = true;
                    }

                    if (dt.Rows[0]["MUN6_3"].ToString().Trim() == "1")
                    {
                        chkNo_6_3.Checked = true;
                    }
                    if (dt.Rows[0]["MUN6_4"].ToString().Trim() == "1")
                    {
                        chkNo_6_4.Checked = true;
                    }
                    if (dt.Rows[0]["MUN6_5"].ToString().Trim() == "1")
                    {
                        chkNo_6_5.Checked = true;
                    }
                    if (dt.Rows[0]["MUN6_6"].ToString().Trim() == "1")
                    {
                        chkNo_6_6.Checked = true;
                    }
                    if (dt.Rows[0]["MUN6_7"].ToString().Trim() == "1")
                    {
                        chkNo_6_7.Checked = true;
                    }
                    if (dt.Rows[0]["MUN6_8"].ToString().Trim() == "1")
                    {
                        chkNo_6_8.Checked = true;
                    }
                    if (dt.Rows[0]["MUN6_9"].ToString().Trim() == "1")
                    {
                        chkNo_6_9.Checked = true;
                    }
                    txtNo_6_9_memo.Text = dt.Rows[0]["MUN6_9_MEMO"].ToString().Trim();

                    txtNo_7_memo.Text = dt.Rows[0]["MUN7"].ToString().Trim();

                    txtEMRNO.Text = dt.Rows[0]["EMRNO"].ToString().Trim();


                    ComFunc.MsgBox(dt.Rows[0]["DeptCode"].ToString().Trim() + "과 에서 " + argBDate + "에 등록된 자료입니다.");
                }
                else
                {
                    ComFunc.MsgBox("등록된 예진표자료가 없습니다.");
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }


        private void frmMedicalInquiry_integration_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssPatInfo_Sheet1.Columns[6].Visible = false;

            // 의사인 경우 버튼활성화
            if (clsType.User.DrCode == "")
            {
                btnChart.Enabled = false;
                btnAll.Enabled = false;
            }
            else
            {
                btnChart.Enabled = true;
                btnChart.Enabled = true;
            }

            Screen_Clear();
            Set_Info();
            Search_Data();
        }

        void Set_Info()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strSex = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SEX ,Age ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + FstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDate =TO_DATE('" + FstrBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode ='" + FstrDept + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strSex = dt.Rows[0]["Age"].ToString().Trim() + "/" + dt.Rows[0]["Sex"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                ssPatInfo_Sheet1.Cells[0, 0].Text = FstrPano;
                ssPatInfo_Sheet1.Cells[0, 1].Text = FstrSName;
                ssPatInfo_Sheet1.Cells[0, 2].Text = strSex;
                ssPatInfo_Sheet1.Cells[0, 3].Text = FstrDept;
                ssPatInfo_Sheet1.Cells[0, 4].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, FstrDrCode);
                ssPatInfo_Sheet1.Cells[0, 5].Text = FstrBDate;
                ssPatInfo_Sheet1.Cells[0, 6].Text = FstrDrCode;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }


        void Build_Data()
        {
            string strData = "";
            string strSUB = "";
            //string strSUB2 = "";

            strData = "";

            strData += ComNum.VBLF;

            if (chkNo_1.Checked == true)
            {
                if (chkNo_1_1.Checked == true)
                {
                    strData = strData + " ▣ 최근 2주 이내 대구, 청도, 경산 등 국내 지역사회유행지역을 방문한 적이 있습니까?" + ComNum.VBLF + "    => 예 " + ComNum.VBLF + ComNum.VBLF;
                }
                else if (chkNo_1_2.Checked == true)
                {
                    strData = strData + " ▣ 최근 2주 이내 대구, 청도, 경산 등 국내 지역사회유행지역을 방문한 적이 있습니까?" + ComNum.VBLF + "    => 아니요 " + ComNum.VBLF + ComNum.VBLF;
                }
            }

            if (chkNo_2.Checked == true)
            {
                if (chkNo_2_1.Checked == true)
                {
                    strData = strData + " ▣ 확진환자와 접촉하거나 확진자 동선이 확실하게 겹치나요?" + ComNum.VBLF +
                                        "  (예: 동일 일시에 밀폐공간에 있었거나 함께 식사 등)" + ComNum.VBLF + "    => 예 " + ComNum.VBLF + ComNum.VBLF;
                }
                else if (chkNo_2_2.Checked == true)
                {
                    strData = strData + " ▣ 확진환자와 접촉하거나 확진자 동선이 확실하게 겹치나요?" + ComNum.VBLF +
                                        "  (예: 동일 일시에 밀폐공간에 있었거나 함께 식사 등)" + ComNum.VBLF + "    => 아니요 " + ComNum.VBLF + ComNum.VBLF;
                }
            }

            if (chkNo_3.Checked == true)
            {
                if (chkNo_3_1.Checked == true)
                {
                    strData = strData + " ▣ 확진환자와 밀접접촉자로 통보받았습니까?" + ComNum.VBLF + "    => 예 " + ComNum.VBLF + ComNum.VBLF;
                }
                else if (chkNo_3_2.Checked == true)
                {
                    strData = strData + " ▣ 확진환자와 밀접접촉자로 통보받았습니까?" + ComNum.VBLF + "    => 아니요 " + ComNum.VBLF + ComNum.VBLF;
                }
            }

            if (chkNo_4.Checked == true)
            {
                if (chkNo_4_1.Checked == true)
                {
                    strData = strData + " ▣ 신천지 교인이신가요?" + ComNum.VBLF + "    => 예 " + ComNum.VBLF + ComNum.VBLF;
                }
                else if (chkNo_4_2.Checked == true)
                {
                    strData = strData + " ▣ 신천지 교인이신가요?" + ComNum.VBLF + "    => 아니요 " + ComNum.VBLF + ComNum.VBLF;
                }
            }

            if (chkNo_5.Checked == true)
            {
                if (chkNo_5_1.Checked == true)
                {
                    strData = strData + " ▣ 3주 이내 해외여행을 하였습니까?" + ComNum.VBLF +
                                        "  ( 중국, 홍콩, 마카오, 싱가폴, 베트남, 필리핀, 일본, 이탈리아, 이란, 말레이시아 등)" +
                                        ComNum.VBLF + "    => 예 " + ComNum.VBLF + ComNum.VBLF;

                    if (txtNo_5_Memo.Text.Trim() != "")
                    {
                        strData = strData + "    ● 방문기간 : " + txtNo_5_Memo.Text.Trim() + ComNum.VBLF + ComNum.VBLF;
                    }
                }
                else if (chkNo_5_2.Checked == true)
                {
                    strData = strData + " ▣ 3주 이내 해외여행을 하였습니까?" + ComNum.VBLF +
                                        "  ( 중국, 홍콩, 마카오, 싱가폴, 베트남, 필리핀, 일본, 이탈리아, 이란, 말레이시아 등)" +
                                        ComNum.VBLF + "    => 아니요 " + ComNum.VBLF + ComNum.VBLF;
                }

            }


            strSUB = "";
            if (chkNo_6_1.Checked == true)
            {
                strSUB += " 발열 : " + txtNo_6_1_memo.Text + "℃,";
            }

            if (chkNo_6_2.Checked == true)
            {
                strSUB += " 기침, ";
            }

            if (chkNo_6_3.Checked == true)
            {
                strSUB += " 가래, ";
            }

            if (chkNo_6_4.Checked == true)
            {
                strSUB += " 오한, ";
            }

            if (chkNo_6_5.Checked == true)
            {
                strSUB += " 근육통, ";
            }

            if (chkNo_6_6.Checked == true)
            {
                strSUB += " 인후통, ";
            }

            if (chkNo_6_7.Checked == true)
            {
                strSUB += " 호흡곤란, ";
            }

            if (chkNo_6_8.Checked == true)
            {
                strSUB += " 설사, ";
            }

            if (chkNo_6_9.Checked == true)
            {
                strSUB += " 기타 : " + txtNo_6_9_memo.Text.Trim();
            }

            if (chkNo_6.Checked == true)
            {
                strData = strData + " ▣ 증상 : " + strSUB;
            }
            
            if (chkNo_7.Checked == true)
            {
                strData += ComNum.VBLF + ComNum.VBLF; 
                strData += " ▣ 참고사항";
                strData += ComNum.VBLF + txtNo_7_memo.Text.Trim();
            }

            txtSendData.Text = strData + ComNum.VBLF;

        }

        private string READ_DEPT_CHOJEA(string ArgPano, string ArgDeptCode, string ArgDrCode)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = " SELECT ROWID FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE = '" + ArgDeptCode + "'";
                SQL = SQL + ComNum.VBLF + "    AND ACTDATE < TRUNC(SYSDATE) ";
                if (ArgDeptCode == "MD")
                {
                    if (ArgDrCode == "1107")
                    {
                        SQL = SQL + ComNum.VBLF + " AND DRCODE = '1107' ";
                    }
                    else if (ArgDrCode == "1125")
                    {
                        SQL = SQL + ComNum.VBLF + " AND DRCODE = '1125'  ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND DRCODE NOT IN ('1107','1125') ";
                    }
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = "";
                }
                else
                {
                    rtnVal = "과초진";
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        private void txtDate_DoubleClick(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text != "")
            {
                clsPublic.GstrCalDate = ((TextBox)sender).Text;
            }

            frmCalendar frmCalendarX = new frmCalendar();
            frmCalendarX.StartPosition = FormStartPosition.CenterParent;
            frmCalendarX.ShowDialog();


            if (VB.IsDate(clsPublic.GstrCalDate) == true)
            {
                ((TextBox)sender).Text = Convert.ToDateTime(clsPublic.GstrCalDate).ToShortDateString();
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            if (btnAll.Text.Trim() == "모든항목 해제")
            {
                chkNo_1.Checked = false;
                chkNo_2.Checked = false;
                chkNo_3.Checked = false;
                chkNo_4.Checked = false;
                chkNo_5.Checked = false;
                chkNo_6.Checked = false;
                chkNo_7.Checked = false;
                btnAll.Text = "모든항목 선택";
            }
            else
            {
                chkNo_1.Checked = true;
                chkNo_2.Checked = true;
                chkNo_3.Checked = true;
                chkNo_4.Checked = true;
                chkNo_5.Checked = true;
                chkNo_6.Checked = true;
                chkNo_7.Checked = true;
                btnAll.Text = "모든항목 해제";
            }
        }
    }
}
