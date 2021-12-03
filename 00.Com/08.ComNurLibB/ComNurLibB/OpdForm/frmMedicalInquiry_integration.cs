using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;
using ComLibB;
using ComEmrBase;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmMedicalInquiry_integration.cs
    /// Description     : 진료과 통합 예진표
    /// Author          : 박창욱
    /// Create Date     : 2018-01-16
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\emr\emrprt\Frm진료예진표2.frm(Frm진료예진표2.frm) >> frmMedicalInquiry_integration.cs 폼이름 재정의" />	
    public partial class frmMedicalInquiry_integration : Form
    {
        string FstrPano = "";
        string FstrSName = "";
        string FstrDept = "";
        string FstrDrCode = "";
        string FstrBDate = "";
        string FstrROWID = "";

        public frmMedicalInquiry_integration()
        {
            InitializeComponent();
        }

        public frmMedicalInquiry_integration(string strPaNo, string strPaName, string strDept, string strDrCode, string strBDate)
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

            //string strPtNo = "";
            //string strSname = "";
            //string strDeptCode = "";
            //string StrDrCode = "";
            //string strBDATE = "";

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
                    clsEmrQuery.SaveNewProgress(clsDB.DbCon, this, pAcp, VB.Val(txtEMRNO.Text), txtSendData.Text.Trim(), ref dblNewEmrNo, true);
                    txtEMRNO.Text = Convert.ToString(dblNewEmrNo);
                }

                //if (pForm.FmOLDGB == 1)
                //{
                //    SaveErInfoXML();
                //}
                //else
                //{
                //    if (clsOrdFunction.GstrGbJob.Equals("OPD") &&
                //        (!clsType.User.DeptCode.Equals("NP") && !clsType.User.DeptCode.Equals("DM") && !clsType.User.DeptCode.Equals("OG") &&
                //         !clsType.User.DeptCode.Equals("OS") && !clsType.User.DeptCode.Equals("NS") && !clsType.User.DeptCode.Equals("CS") &&
                //         !clsType.User.DeptCode.Equals("UR") && !clsType.User.DeptCode.Equals("RM") && !clsType.User.DeptCode.Equals("NE") && !clsType.User.DeptCode.Equals("MI"))
                //        )
                //    {
                //        double dblNewEmrNo = 0;
                //        clsEmrQuery.SaveNewProgress(clsDB.DbCon, this, pAcp, VB.Val(txtEMRNO.Text), txtSendData.Text.Trim(), ref dblNewEmrNo, true);
                //        txtEMRNO.Text = Convert.ToString(dblNewEmrNo);
                //    }
                //    else
                //    {
                //        SaveErInfoXML();
                //    }
                //}
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (FstrROWID != "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "OPD_DEPT_MUNJIN SET ";
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

            string strH = ""; //키
            string strW = ""; //몸무게
            string strMun1 = "";
            string strMun2Y = "";
            string strMun2N = "";
            string strMun2_1 = "";
            string strMun2_1_1 = "";
            string strMun2_2 = "";
            string strMun2_2_1 = "";
            string strMun2_3 = "";
            string strMun2_3_1 = "";
            string strMun2_4 = "";
            string strMun2_4_1 = "";
            string strMun2_5 = "";
            string strMun2_5_1 = "";
            string strMun2_6 = "";
            string strMun2_6_1 = "";
            string strMun2_7 = "";
            string strMun2_7_1 = "";
            string strMun2_8 = "";
            string strMun2_8_1 = "";
            string strMun2_9 = "";
            string strMun2_9_1 = "";
            string strMun2_10 = "";
            string strMun2_10_1 = "";
            string strMun3_1 = "";
            string strMun3_2 = "";
            string strMun3_2_1 = "";
            string strMun3_2_1_1 = "";
            string strMun3_2_2 = "";
            string strMun3_2_2_1 = "";
            string strMun3_2_3 = "";
            string strMun3_2_3_1 = "";
            string strMun3_2_4 = "";
            string strMun3_2_4_1 = "";
            string strMun3_2_5 = "";
            string strMun3_2_5_1 = "";
            string strMun3_2_6 = "";
            string strMun3_2_6_1 = "";
            string strMun3_2_7 = "";
            string strMun3_2_7_1 = "";
            string strMun4_1 = "";
            string strMun4_2 = "";
            string strMun4_2_1 = "";
            string strMun4_2_2 = "";
            string strMun4_2_3 = "";
            string strMun4_2_4 = "";
            string strMun4_2_5 = "";
            string strMun4_2_5_1 = "";
            string strMun5_1 = "";
            string strMun5_1_1 = "";
            string strMun5_2 = "";
            string strMun5_2_1 = "";
            string strMun6_1 = "";
            string strMun6_1_1 = "";
            string strMun6_1_2 = "";
            string strMun6_1_3 = "";
            string strMun6_2 = "";
            string strMun6_2_1 = "";
            string strMun6_2_2 = "";
            string strMun6_2_3 = "";
            string strMun7_1 = "";
            string strMun7_2 = "";
            string strMun7_2_1 = "";
            string strMun7_2_2 = "";
            string strMun8_1 = "";
            string strMun8_2 = "";
            string strMun8_3 = "";
            string strMun8_4 = "";
            string strMun8_5 = "";
            string strMun8_5_1 = "";
            string strMun9_1 = "";
            string strMun9_2 = "";
            string strMun9_2_1 = "";
            string strMun9_2_1_1 = "";
            string strMun9_2_1_2 = "";
            string strMun9_2_2 = "";
            string strMun9_2_2_1 = "";
            string strMun9_2_2_2 = "";
            string strMun9_2_3 = "";
            string strMun9_2_3_1 = "";
            string strMun9_2_3_2 = "";
            string strMun9_2_4 = "";
            string strMun9_2_4_1 = "";
            string strMun9_2_4_2 = "";
            string strMun9_2_5 = "";
            string strMun9_2_5_1 = "";
            string strMun9_2_5_2 = "";
            string strMun9_2_6 = "";
            string strMun9_2_6_1 = "";
            string strMun9_2_6_2 = "";
            string strMun9_2_7 = "";
            string strMun9_2_7_1 = "";
            string strMun9_2_7_2 = "";
            string strMun10_1 = "";
            string strMun10_2 = "";
            string strMun10_2_1 = "";
            string strMun10_3_1 = "";
            string strMun10_3_2 = "";
            string strMun10_3_3 = "";
            string strMun10_3_4 = "";
            string strMun10_4_1 = "";
            string strMun10_4_2 = "";
            string strMun10_4_3 = "";
            string strMun10_4_4 = "";
            string strMun10_4_5 = "";
            string strMun10_5_1 = "";
            string strMun10_5_2 = "";
            string strMun10_5_3 = "";
            string strMun10_5_4 = "";
            string strMun10_5_5 = "";
            string strMun10_5_6 = "";
            string strMun10_5_7 = "";
            string strMun10_5_8 = "";
            string strMun10_5_9 = "";
            string strMun10_5_10 = "";
            string strMun10_5_11 = "";
            string strMun10_5_11_1 = "";
            string strMun11_1 = "";
            string strMun12_1 = "";
            string strMun13_1 = "";
            string strRemark = "";

            strH = txtHeight.Text.Trim();
            strW = txtWeight.Text.Trim();

            //1
            strMun1 = txt1.Text.Trim();

            //2

            strMun2N = "N";
            if (chkNo_2_1N.Checked == true)
            {
                strMun2N = "Y";
            }
            strMun2Y = "N";
            if (chkNo_2_1Y.Checked == true)
            {
                strMun2Y = "Y";
            }

            strMun2_1 = "N";
            if (chkNo_2_1.Checked == true)
            {
                strMun2_1 = "Y";
            }
            strMun2_1_1 = txt2_1.Text.Trim();
            strMun2_2 = "N";
            if (chkNo_2_2.Checked == true)
            {
                strMun2_2 = "Y";
            }
            strMun2_2_1 = txt2_2.Text.Trim();
            strMun2_3 = "N";
            if (chkNo_2_3.Checked == true)
            {
                strMun2_3 = "Y";
            }
            strMun2_3_1 = txt2_3.Text.Trim();
            strMun2_4 = "N";
            if (chkNo_2_4.Checked == true)
            {
                strMun2_4 = "Y";
            }
            strMun2_4_1 = txt2_4.Text.Trim();
            strMun2_5 = "N";
            if (chkNo_2_5.Checked == true)
            {
                strMun2_5 = "Y";
            }
            strMun2_5_1 = txt2_5.Text.Trim();
            strMun2_6 = "N";
            if (chkNo_2_6.Checked == true)
            {
                strMun2_6 = "Y";
            }
            strMun2_6_1 = txt2_6.Text.Trim();
            strMun2_7 = "N";
            if (chkNo_2_7.Checked == true)
            {
                strMun2_7 = "Y";
            }
            strMun2_7_1 = txt2_7.Text.Trim();
            strMun2_8 = "N";
            if (chkNo_2_8.Checked == true)
            {
                strMun2_8 = "Y";
            }
            strMun2_8_1 = txt2_8.Text.Trim();
            strMun2_9 = "N";
            if (chkNo_2_9.Checked == true)
            {
                strMun2_9 = "Y";
            }
            strMun2_9_1 = txt2_9.Text.Trim();
            strMun2_10 = "N";
            if (chkNo_2_10.Checked == true)
            {
                strMun2_10 = "Y";
            }
            strMun2_10_1 = txt2_10.Text.Trim();

            //3
            strMun3_1 = "N";
            if (chkNo_3_1.Checked == true)
            {
                strMun3_1 = "Y";
            }
            strMun3_2 = "N";
            if (chkNo_3_2.Checked == true)
            {
                strMun3_2 = "Y";
            }

            strMun3_2_1 = "N";
            if (chkNo_3_2_1.Checked == true)
            {
                strMun3_2_1 = "Y";
            }
            strMun3_2_1_1 = txt3_2_1.Text.Trim();
            strMun3_2_2 = "N";
            if (chkNo_3_2_2.Checked == true)
            {
                strMun3_2_2 = "Y";
            }
            strMun3_2_2_1 = txt3_2_2.Text.Trim();
            strMun3_2_3 = "N";
            if (chkNo_3_2_3.Checked == true)
            {
                strMun3_2_3 = "Y";
            }
            strMun3_2_3_1 = txt3_2_3.Text.Trim();
            strMun3_2_4 = "N";
            if (chkNo_3_2_4.Checked == true)
            {
                strMun3_2_4 = "Y";
            }
            strMun3_2_4_1 = txt3_2_4.Text.Trim();
            strMun3_2_5 = "N";
            if (chkNo_3_2_5.Checked == true)
            {
                strMun3_2_5 = "Y";
            }
            strMun3_2_5_1 = txt3_2_5.Text.Trim();
            strMun3_2_6 = "N";
            if (chkNo_3_2_6.Checked == true)
            {
                strMun3_2_6 = "Y";
            }
            strMun3_2_6_1 = txt3_2_6.Text.Trim();
            strMun3_2_7 = "N";
            if (chkNo_3_2_7.Checked == true)
            {
                strMun3_2_7 = "Y";
            }
            strMun3_2_7_1 = txt3_2_7.Text.Trim();


            //4
            strMun4_1 = "N";
            if (chkNo_4_1.Checked == true)
            {
                strMun4_1 = "Y";
            }
            strMun4_2 = "N";
            if (chkNo_4_2.Checked == true)
            {
                strMun4_2 = "Y";
            }
            strMun4_2_1 = "N";
            if (chkNo_4_2_1.Checked == true)
            {
                strMun4_2_1 = "Y";
            }
            strMun4_2_2 = "N";
            if (chkNo_4_2_2.Checked == true)
            {
                strMun4_2_2 = "Y";
            }
            strMun4_2_3 = "N";
            if (chkNo_4_2_3.Checked == true)
            {
                strMun4_2_3 = "Y";
            }
            strMun4_2_4 = "N";
            if (chkNo_4_2_4.Checked == true)
            {
                strMun4_2_4 = "Y";
            }
            strMun4_2_5 = "N";
            if (chkNo_4_2_5.Checked == true)
            {
                strMun4_2_5 = "Y";
            }
            strMun4_2_5_1 = txt4_2_5_1.Text.Trim();


            //5
            strMun5_1 = "N";
            if (chkNo_5_1.Checked == true)
            {
                strMun5_1 = "Y";
            }
            strMun5_1_1 = txt5_1_1.Text.Trim();
            strMun5_2 = "N";
            if (chkNo_5_2.Checked == true)
            {
                strMun5_2 = "Y";
            }

            strMun5_2_1 = txt5_2_1.Text.Trim();

            //6
            strMun6_1 = "N";
            if (chkNo_6_1.Checked == true)
            {
                strMun6_1 = "Y";
            }
            strMun6_1_1 = "N";
            if (chkNo_6_1_1.Checked == true)
            {
                strMun6_1_1 = "Y";
            }
            strMun6_1_2 = txt6_1_1_1.Text.Trim();
            strMun6_1_3 = txt6_1_1_2.Text.Trim();
            strMun6_2 = "N";
            if (chkNo_6_2.Checked == true)
            {
                strMun6_2 = "Y";
            }
            strMun6_2_1 = "N";
            if (chkNo_6_2_1.Checked == true)
            {
                strMun6_2_1 = "Y";
            }
            strMun6_2_2 = txt6_2_1_1.Text.Trim();
            strMun6_2_3 = txt6_2_1_2.Text.Trim();


            //7
            strMun7_1 = "N";
            if (chkNo_7_1.Checked == true)
            {
                strMun7_1 = "Y";
            }
            strMun7_2 = "N";
            if (chkNo_7_2.Checked == true)
            {
                strMun7_2 = "Y";
            }
            strMun7_2_1 = txt7_2_1.Text.Trim();
            strMun7_2_2 = txt7_2_2.Text.Trim();

            //8
            strMun8_1 = "N";
            if (chkNo_8_1.Checked == true)
            {
                strMun8_1 = "Y";
            }
            strMun8_2 = "N";
            if (chkNo_8_2.Checked == true)
            {
                strMun8_2 = "Y";
            }
            strMun8_3 = "N";
            if (chkNo_8_3.Checked == true)
            {
                strMun8_3 = "Y";
            }
            strMun8_4 = "N";
            if (chkNo_8_4.Checked == true)
            {
                strMun8_4 = "Y";
            }
            strMun8_5 = "N";
            if (chkNo_8_5.Checked == true)
            {
                strMun8_5 = "Y";
            }
            strMun8_5_1 = txt8_5_1.Text.Trim();


            //9
            strMun9_1 = "N";
            if (chkNo_9_1.Checked == true)
            {
                strMun9_1 = "Y";
            }
            strMun9_2 = "N";
            if (chkNo_9_2.Checked == true)
            {
                strMun9_2 = "Y";
            }
            strMun9_2_1 = "N";
            if (chkNo_9_2_1.Checked == true)
            {
                strMun9_2_1 = "Y";
            }
            strMun9_2_1_1 = txt9_2_1_1.Text.Trim();
            strMun9_2_1_2 = txt9_2_1_2.Text.Trim();
            strMun9_2_2 = "N";
            if (chkNo_9_2_2.Checked == true)
            {
                strMun9_2_2 = "Y";
            }
            strMun9_2_2_1 = txt9_2_2_1.Text.Trim();
            strMun9_2_2_2 = txt9_2_2_2.Text.Trim();
            strMun9_2_3 = "N";
            if (chkNo_9_2_3.Checked == true)
            {
                strMun9_2_3 = "Y";
            }
            strMun9_2_3_1 = txt9_2_3_1.Text.Trim();
            strMun9_2_3_2 = txt9_2_3_2.Text.Trim();
            strMun9_2_4 = "N";
            if (chkNo_9_2_4.Checked == true)
            {
                strMun9_2_4 = "Y";
            }
            strMun9_2_4_1 = txt9_2_4_1.Text.Trim();
            strMun9_2_4_2 = txt9_2_4_2.Text.Trim();
            strMun9_2_5 = "N";
            if (chkNo_9_2_5.Checked == true)
            {
                strMun9_2_5 = "Y";
            }
            strMun9_2_5_1 = txt9_2_5_1.Text.Trim();
            strMun9_2_5_2 = txt9_2_5_2.Text.Trim();
            strMun9_2_6 = "N";
            if (chkNo_9_2_6.Checked == true)
            {
                strMun9_2_6 = "Y";
            }
            strMun9_2_6_1 = txt9_2_6_1.Text.Trim();
            strMun9_2_6_2 = txt9_2_6_2.Text.Trim();
            strMun9_2_7 = "N";
            if (chkNo_9_2_7.Checked == true)
            {
                strMun9_2_7 = "Y";
            }
            strMun9_2_7_1 = txt9_2_7_1.Text.Trim();
            strMun9_2_7_2 = txt9_2_7_2.Text.Trim();

            //10
            strMun10_1 = "N";
            if (chkNo_10_1.Checked == true)
            {
                strMun10_1 = "Y";
            }
            strMun10_2 = "N";
            if (chkNo_10_2.Checked == true)
            {
                strMun10_2 = "Y";
            }
            strMun10_2_1 = txt10_2_1.Text.Trim();
            strMun10_3_1 = txt10_3_1.Text.Trim();
            strMun10_3_2 = txt10_3_2.Text.Trim();
            strMun10_3_3 = txt10_3_3.Text.Trim();
            strMun10_3_4 = txt10_3_4.Text.Trim();
            strMun10_4_1 = txt10_4_1.Text.Trim();
            strMun10_4_2 = txt10_4_2.Text.Trim();
            strMun10_4_3 = txt10_4_3.Text.Trim();
            strMun10_4_4 = txt10_4_4.Text.Trim();
            strMun10_4_5 = txt10_4_5.Text.Trim();

            if (strMun10_4_1 != "" && string.Compare(strMun10_4_1, "3") >= 0)
            {
                ComFunc.MsgBox("0~2 까지 값만 가능합니다.");
                return rtnVal;
            }
            if (strMun10_4_2 != "" && string.Compare(strMun10_4_2, "3") >= 0)
            {
                ComFunc.MsgBox("0~2 까지 값만 가능합니다.");
                return rtnVal;
            }
            if (strMun10_4_3 != "" && string.Compare(strMun10_4_3, "3") >= 0)
            {
                ComFunc.MsgBox("0~2 까지 값만 가능합니다.");
                return rtnVal;
            }
            if (strMun10_4_4 != "" && string.Compare(strMun10_4_4, "3") >= 0)
            {
                ComFunc.MsgBox("0~2 까지 값만 가능합니다.");
                return rtnVal;
            }
            if (strMun10_4_5 != "" && string.Compare(strMun10_4_5, "3") >= 0)
            {
                ComFunc.MsgBox("0~2 까지 값만 가능합니다.");
                return rtnVal;
            }

            strMun10_5_1 = "N";
            if (chkNo_10_5_1.Checked == true)
            {
                strMun10_5_1 = "Y";
            }
            strMun10_5_2 = "N";
            if (chkNo_10_5_2.Checked == true)
            {
                strMun10_5_2 = "Y";
            }
            strMun10_5_3 = "N";
            if (chkNo_10_5_3.Checked == true)
            {
                strMun10_5_3 = "Y";
            }
            strMun10_5_4 = "N";
            if (chkNo_10_5_4.Checked == true)
            {
                strMun10_5_4 = "Y";
            }
            strMun10_5_5 = "N";
            if (chkNo_10_5_5.Checked == true)
            {
                strMun10_5_5 = "Y";
            }
            strMun10_5_6 = "N";
            if (chkNo_10_5_6.Checked == true)
            {
                strMun10_5_6 = "Y";
            }
            strMun10_5_7 = "N";
            if (chkNo_10_5_7.Checked == true)
            {
                strMun10_5_7 = "Y";
            }
            strMun10_5_8 = "N";
            if (chkNo_10_5_8.Checked == true)
            {
                strMun10_5_8 = "Y";
            }
            strMun10_5_9 = "N";
            if (chkNo_10_5_9.Checked == true)
            {
                strMun10_5_9 = "Y";
            }
            strMun10_5_10 = "N";
            if (chkNo_10_5_10.Checked == true)
            {
                strMun10_5_10 = "Y";
            }
            strMun10_5_11 = "N";
            if (chkNo_10_5_11.Checked == true)
            {
                strMun10_5_11 = "Y";
            }
            strMun10_5_11_1 = txt10_5_11_1.Text.Trim();

            strMun11_1 = txt11_Fa.Text.Trim();

            if (chkNo_12_1.Checked == true)
            {
                strMun12_1 = "N";
            }
            else if (chkNo_12_2.Checked == true)
            {
                strMun12_1 = "Y";
            }

            if (chkNo_13_1.Checked == true)
            {
                strMun13_1 = "N";
            }
            else if (chkNo_13_2.Checked == true)
            {
                strMun13_1 = "Y";
            }

            strRemark = txtRemark.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_DEPT_MUNJIN ";
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
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "OPD_DEPT_MUNJIN (BDate , Pano, DeptCode, DrCode,TMUN_H, TMUN_W, ";
                    SQL = SQL + ComNum.VBLF + " TMUN1,TMUN2Y,TMUN2N,TMUN2_1,TMUN2_1_1,TMUN2_2,TMUN2_2_1,TMUN2_3,";
                    SQL = SQL + ComNum.VBLF + " TMUN2_3_1,TMUN2_4,TMUN2_4_1,TMUN2_5,TMUN2_5_1,TMUN2_6,TMUN2_6_1,";
                    SQL = SQL + ComNum.VBLF + " TMUN2_7,TMUN2_7_1,TMUN2_8,TMUN2_8_1,TMUN2_9,TMUN2_9_1, ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_10,TMUN2_10_1,TMUN3_1,TMUN3_2,TMUN3_2_1,TMUN3_2_1_1,";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_2,TMUN3_2_2_1,TMUN3_2_3,TMUN3_2_3_1, ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_4,TMUN3_2_4_1,TMUN3_2_5,TMUN3_2_5_1,TMUN3_2_6,";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_6_1,TMUN3_2_7,TMUN3_2_7_1,TMUN4_1,TMUN4_2, ";
                    SQL = SQL + ComNum.VBLF + " TMUN4_2_1,TMUN4_2_2,TMUN4_2_3,TMUN4_2_4,TMUN4_2_5,TMUN4_2_5_1,TMUN5_1,TMUN5_1_1,TMUN5_2,TMUN5_2_1, ";
                    SQL = SQL + ComNum.VBLF + " TMUN6_1,TMUN6_1_1,TMUN6_1_2,TMUN6_1_3,TMUN6_2,TMUN6_2_1,TMUN6_2_2,TMUN6_2_3, ";
                    SQL = SQL + ComNum.VBLF + " TMUN7_1,TMUN7_2,TMUN7_2_1,TMUN7_2_2,TMUN8_1,TMUN8_2,TMUN8_3,TMUN8_4,TMUN8_5, ";
                    SQL = SQL + ComNum.VBLF + " TMUN8_5_1,TMUN9_1,TMUN9_2,TMUN9_2_1,TMUN9_2_1_1,";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_1_2,TMUN9_2_2,TMUN9_2_2_1,TMUN9_2_2_2,TMUN9_2_3, ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_3_1,TMUN9_2_3_2,TMUN9_2_4,TMUN9_2_4_1,TMUN9_2_4_2,";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_5,TMUN9_2_5_1,TMUN9_2_5_2,TMUN9_2_6,TMUN9_2_6_1, ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_6_2,TMUN9_2_7,TMUN9_2_7_1,TMUN9_2_7_2,TMUN10_1,";
                    SQL = SQL + ComNum.VBLF + " TMUN10_2,TMUN10_2_1,TMUN10_3_1,TMUN10_3_2,TMUN10_3_3,TMUN10_3_4, ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_4_1,TMUN10_4_2,TMUN10_4_3,TMUN10_4_4,TMUN10_4_5,TMUN10_5_1,TMUN10_5_2, ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_3,TMUN10_5_4,TMUN10_5_5,TMUN10_5_6,TMUN10_5_7,TMUN10_5_8,TMUN10_5_9, ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_10,TMUN10_5_11,TMUN10_5_11_1,tmun11_f1,tmun12_t1,tmun13_drug1,";

                    SQL = SQL + ComNum.VBLF + " Remark, ENTSABUN, ENTDATE ) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + FstrBDate + "','YYYY-MM-DD'),'" + FstrPano + "',";
                    SQL = SQL + ComNum.VBLF + " '" + FstrDept + "','" + FstrDrCode + "','" + strH + "','" + strW + "', ";

                    SQL = SQL + ComNum.VBLF + " '" + strMun1 + "', ";

                    SQL = SQL + ComNum.VBLF + " '" + strMun2Y + "', '" + strMun2N + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strMun2_1 + "', '" + strMun2_1_1 + "', '" + strMun2_2 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun2_2_1 + "', '" + strMun2_3 + "', '" + strMun2_3_1 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun2_4 + "', '" + strMun2_4_1 + "', '" + strMun2_5 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun2_5_1 + "', '" + strMun2_6 + "', '" + strMun2_6_1 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun2_7 + "', '" + strMun2_7_1 + "', '" + strMun2_8 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun2_8_1 + "', '" + strMun2_9 + "', '" + strMun2_9_1 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun2_10 + "', '" + strMun2_10_1 + "',  ";

                    SQL = SQL + ComNum.VBLF + " '" + strMun3_1 + "', '" + strMun3_2 + "', '" + strMun3_2_1 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun3_2_1_1 + "', '" + strMun3_2_2 + "',  '" + strMun3_2_2_1 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun3_2_3 + "',  '" + strMun3_2_3_1 + "', '" + strMun3_2_4 + "',";
                    SQL = SQL + ComNum.VBLF + "  '" + strMun3_2_4_1 + "', '" + strMun3_2_5 + "',  '" + strMun3_2_5_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strMun3_2_6 + "',  '" + strMun3_2_6_1 + "', '" + strMun3_2_7 + "',";
                    SQL = SQL + ComNum.VBLF + "  '" + strMun3_2_7_1 + "', ";

                    SQL = SQL + ComNum.VBLF + " '" + strMun4_1 + "', '" + strMun4_2 + "', '" + strMun4_2_1 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun4_2_2 + "', '" + strMun4_2_3 + "', '" + strMun4_2_4 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun4_2_5 + "', '" + strMun4_2_5_1 + "',  ";

                    SQL = SQL + ComNum.VBLF + " '" + strMun5_1 + "', '" + strMun5_1_1 + "', '" + strMun5_2 + "',";
                    SQL = SQL + ComNum.VBLF + "  TO_DATE('" + strMun5_2_1 + "','YYYY-MM-DD'),  ";

                    SQL = SQL + ComNum.VBLF + " '" + strMun6_1 + "', '" + strMun6_1_1 + "', '" + strMun6_1_2 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun6_1_3 + "', '" + strMun6_2 + "', '" + strMun6_2_1 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun6_2_2 + "', '" + strMun6_2_3 + "', ";

                    SQL = SQL + ComNum.VBLF + " '" + strMun7_1 + "', '" + strMun7_2 + "',  TO_DATE('" + strMun7_2_1 + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " '" + strMun7_2_2 + "', ";

                    SQL = SQL + ComNum.VBLF + " '" + strMun8_1 + "', '" + strMun8_2 + "', '" + strMun8_3 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun8_4 + "', '" + strMun8_5 + "', '" + strMun8_5_1 + "',";

                    SQL = SQL + ComNum.VBLF + " '" + strMun9_1 + "', '" + strMun9_2 + "', '" + strMun9_2_1 + "',";
                    SQL = SQL + ComNum.VBLF + "  '" + strMun9_2_1_1 + "', '" + strMun9_2_1_2 + "', '" + strMun9_2_2 + "',";
                    SQL = SQL + ComNum.VBLF + "  '" + strMun9_2_2_1 + "', '" + strMun9_2_2_2 + "', '" + strMun9_2_3 + "',";
                    SQL = SQL + ComNum.VBLF + "  '" + strMun9_2_3_1 + "', '" + strMun9_2_3_2 + "', '" + strMun9_2_4 + "',";
                    SQL = SQL + ComNum.VBLF + "  '" + strMun9_2_4_1 + "', '" + strMun9_2_4_2 + "', '" + strMun9_2_5 + "',";
                    SQL = SQL + ComNum.VBLF + "  '" + strMun9_2_5_1 + "', '" + strMun9_2_5_2 + "', '" + strMun9_2_6 + "',";
                    SQL = SQL + ComNum.VBLF + "  '" + strMun9_2_6_1 + "', '" + strMun9_2_6_2 + "', '" + strMun9_2_7 + "',";
                    SQL = SQL + ComNum.VBLF + "  '" + strMun9_2_7_1 + "', '" + strMun9_2_7_2 + "', ";

                    SQL = SQL + ComNum.VBLF + " '" + strMun10_1 + "', '" + strMun10_2 + "', '" + strMun10_2_1 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun10_3_1 + "', '" + strMun10_3_2 + "', '" + strMun10_3_3 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun10_3_4 + "', '" + strMun10_4_1 + "', '" + strMun10_4_2 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun10_4_3 + "', '" + strMun10_4_4 + "', '" + strMun10_4_5 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun10_5_1 + "', '" + strMun10_5_2 + "', '" + strMun10_5_3 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun10_5_4 + "', '" + strMun10_5_5 + "', '" + strMun10_5_6 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun10_5_7 + "', '" + strMun10_5_8 + "', '" + strMun10_5_9 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun10_5_10 + "', '" + strMun10_5_11 + "', '" + strMun10_5_11_1 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun11_1 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun12_1 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun13_1 + "',";

                    SQL = SQL + ComNum.VBLF + " '" + strRemark + "'," + clsType.User.Sabun + ",SYSDATE ) ";
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "OPD_DEPT_MUNJIN SET ";

                    SQL = SQL + ComNum.VBLF + " TMUN_H ='" + strH + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_W ='" + strW + "', ";

                    SQL = SQL + ComNum.VBLF + " TMUN1 ='" + strMun1 + "', ";

                    SQL = SQL + ComNum.VBLF + " TMUN2Y ='" + strMun2Y + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2N ='" + strMun2N + "', ";

                    SQL = SQL + ComNum.VBLF + " TMUN2_1 ='" + strMun2_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_1_1 ='" + strMun2_1_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_2 ='" + strMun2_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_2_1 ='" + strMun2_2_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_3 ='" + strMun2_3 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_3_1 ='" + strMun2_3_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_4 ='" + strMun2_4 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_4_1 ='" + strMun2_4_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_5 ='" + strMun2_5 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_5_1 ='" + strMun2_5_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_6 ='" + strMun2_6 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_6_1 ='" + strMun2_6_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_7 ='" + strMun2_7 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_7_1 ='" + strMun2_7_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_8 ='" + strMun2_8 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_8_1 ='" + strMun2_8_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_9 ='" + strMun2_9 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_9_1 ='" + strMun2_9_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_10 ='" + strMun2_10 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_10_1 ='" + strMun2_10_1 + "', ";

                    SQL = SQL + ComNum.VBLF + " TMUN3_1 ='" + strMun3_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2 ='" + strMun3_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_1 ='" + strMun3_2_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_1_1 = '" + strMun3_2_1_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_2 ='" + strMun3_2_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_2_1 ='" + strMun3_2_2_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_3 ='" + strMun3_2_3 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_3_1 ='" + strMun3_2_3_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_4 ='" + strMun3_2_4 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_4_1 ='" + strMun3_2_4_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_5 ='" + strMun3_2_5 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_5_1 ='" + strMun3_2_5_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_6 ='" + strMun3_2_6 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_6_1 ='" + strMun3_2_6_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_7 ='" + strMun3_2_7 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_7_1 ='" + strMun3_2_7_1 + "',";

                    SQL = SQL + ComNum.VBLF + " TMUN4_1 ='" + strMun4_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN4_2 ='" + strMun4_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN4_2_1 ='" + strMun4_2_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN4_2_2 ='" + strMun4_2_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN4_2_3 ='" + strMun4_2_3 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN4_2_4 ='" + strMun4_2_4 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN4_2_5 ='" + strMun4_2_5 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN4_2_5_1 ='" + strMun4_2_5_1 + "', ";

                    SQL = SQL + ComNum.VBLF + " TMUN5_1 ='" + strMun5_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN5_1_1 ='" + strMun5_1_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN5_2 ='" + strMun5_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN5_2_1 =TO_DATE('" + strMun5_2_1 + "','YYYY-MM-DD'), ";

                    SQL = SQL + ComNum.VBLF + " TMUN6_1 ='" + strMun6_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN6_1_1 ='" + strMun6_1_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN6_1_2 ='" + strMun6_1_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN6_1_3 ='" + strMun6_1_3 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN6_2 ='" + strMun6_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN6_2_1 ='" + strMun6_2_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN6_2_2 ='" + strMun6_2_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN6_2_3 ='" + strMun6_2_3 + "', ";

                    SQL = SQL + ComNum.VBLF + " TMUN7_1 ='" + strMun7_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN7_2 ='" + strMun7_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN7_2_1 =TO_DATE('" + strMun7_2_1 + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " TMUN7_2_2 ='" + strMun7_2_2 + "', ";

                    SQL = SQL + ComNum.VBLF + " TMUN8_1 ='" + strMun8_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN8_2 ='" + strMun8_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN8_3 ='" + strMun8_3 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN8_4 ='" + strMun8_4 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN8_5 ='" + strMun8_5 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN8_5_1 ='" + strMun8_5_1 + "', ";

                    SQL = SQL + ComNum.VBLF + " TMUN9_1 ='" + strMun9_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2 ='" + strMun9_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_1 ='" + strMun9_2_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_1_1 ='" + strMun9_2_1_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_1_2 ='" + strMun9_2_1_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_2 ='" + strMun9_2_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_2_1 ='" + strMun9_2_2_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_2_2 ='" + strMun9_2_2_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_3 ='" + strMun9_2_3 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_3_1 ='" + strMun9_2_3_1 + "',";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_3_2 ='" + strMun9_2_3_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_4 ='" + strMun9_2_4 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_4_1 ='" + strMun9_2_4_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_4_2 ='" + strMun9_2_4_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_5 ='" + strMun9_2_5 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_5_1 ='" + strMun9_2_5_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_5_2 ='" + strMun9_2_5_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_6 ='" + strMun9_2_6 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_6_1 ='" + strMun9_2_6_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_6_2 ='" + strMun9_2_6_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_7 ='" + strMun9_2_7 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_7_1 ='" + strMun9_2_7_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_7_2 ='" + strMun9_2_7_2 + "', ";

                    SQL = SQL + ComNum.VBLF + " TMUN10_1 ='" + strMun10_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_2 ='" + strMun10_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_2_1 ='" + strMun10_2_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_3_1 ='" + strMun10_3_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_3_2 ='" + strMun10_3_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_3_3 ='" + strMun10_3_3 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_3_4 ='" + strMun10_3_4 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_4_1 ='" + strMun10_4_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_4_2 ='" + strMun10_4_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_4_3 ='" + strMun10_4_3 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_4_4 ='" + strMun10_4_4 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_4_5 ='" + strMun10_4_5 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_1 ='" + strMun10_5_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_2 ='" + strMun10_5_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_3 ='" + strMun10_5_3 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_4 ='" + strMun10_5_4 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_5 ='" + strMun10_5_5 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_6 ='" + strMun10_5_6 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_7 ='" + strMun10_5_7 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_8 ='" + strMun10_5_8 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_9 ='" + strMun10_5_9 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_10 ='" + strMun10_5_10 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_11 ='" + strMun10_5_11 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_11_1 ='" + strMun10_5_11_1 + "', ";

                    SQL = SQL + ComNum.VBLF + " tmun11_f1 ='" + strMun11_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " tmun12_t1 ='" + strMun12_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " tmun13_drug1 ='" + strMun13_1 + "', ";

                    SQL = SQL + ComNum.VBLF + "Remark ='" + strRemark + "', ";
                    SQL = SQL + ComNum.VBLF + "ENTSABUN =" + clsType.User.Sabun + ", ";
                    SQL = SQL + ComNum.VBLF + "ENTDATE =SYSDATE  ";

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
            Read_Munjin_Dept(FstrPano, FstrDept, FstrBDate);    //당일접수

            DATA_BULID_PRE();
        }

        private void DATA_BULID_PRE()
        {

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                chkNo_0.Enabled = false;
                chkNo_1.Enabled = false;
                chkNo_2.Enabled = false;
                chkNo_3.Enabled = false;
                chkNo_4.Enabled = false;
                chkNo_5.Enabled = false;
                chkNo_6.Enabled = false;
                chkNo_7.Enabled = false;
                chkNo_8.Enabled = false;
                chkNo_9.Enabled = false;
                chkNo_10.Enabled = false;
                chkNo_11.Enabled = false;
                chkNo_12.Enabled = false;
                chkNo_14.Enabled = false;

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

                chkNo_0.Enabled = true;
                chkNo_1.Enabled = true;
                chkNo_2.Enabled = true;
                chkNo_3.Enabled = true;
                chkNo_4.Enabled = true;
                chkNo_5.Enabled = true;
                chkNo_6.Enabled = true;
                chkNo_7.Enabled = true;
                chkNo_8.Enabled = true;
                chkNo_9.Enabled = true;
                chkNo_10.Enabled = true;
                chkNo_11.Enabled = true;
                chkNo_12.Enabled = true;
                chkNo_14.Enabled = true;

                chkNo_0.Checked = true;
                chkNo_1.Checked = true;
                chkNo_2.Checked = true;
                chkNo_3.Checked = true;
                chkNo_4.Checked = true;
                chkNo_5.Checked = true;
                chkNo_6.Checked = true;
                chkNo_7.Checked = true;
                chkNo_8.Checked = true;
                chkNo_9.Checked = true;
                chkNo_10.Checked = true;
                chkNo_11.Checked = true;
                chkNo_12.Checked = true;
                chkNo_14.Checked = true;

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
            txtHeight.Text = "";
            txtWeight.Text = "";

            //1
            txt1.Text = "";

            //2
            chkNo_2_1N.Checked = false;
            chkNo_2_1Y.Checked = false;
            chkNo_2_1.Checked = false;
            txt2_1.Text = "";
            chkNo_2_2.Checked = false;
            txt2_2.Text = "";
            chkNo_2_3.Checked = false;
            txt2_3.Text = "";
            chkNo_2_4.Checked = false;
            txt2_4.Text = "";
            chkNo_2_5.Checked = false;
            txt2_5.Text = "";
            chkNo_2_6.Checked = false;
            txt2_6.Text = "";
            chkNo_2_7.Checked = false;
            txt2_7.Text = "";
            chkNo_2_8.Checked = false;
            txt2_8.Text = "";
            chkNo_2_9.Checked = false;
            txt2_9.Text = "";
            chkNo_2_10.Checked = false;
            txt2_10.Text = "";


            //3
            chkNo_3_1.Checked = false;
            chkNo_3_2.Checked = false;

            chkNo_3_2_1.Checked = false;
            txt3_2_1.Text = "";
            chkNo_3_2_2.Checked = false;
            txt3_2_2.Text = "";
            chkNo_3_2_3.Checked = false;
            txt3_2_3.Text = "";
            chkNo_3_2_4.Checked = false;
            txt3_2_4.Text = "";
            chkNo_3_2_5.Checked = false;
            txt3_2_5.Text = "";
            chkNo_3_2_6.Checked = false;
            txt3_2_6.Text = "";
            chkNo_3_2_7.Checked = false;
            txt3_2_7.Text = "";

            //4
            chkNo_4_1.Checked = false;
            chkNo_4_2.Checked = false;
            chkNo_4_2_1.Checked = false;
            chkNo_4_2_2.Checked = false;
            chkNo_4_2_3.Checked = false;
            chkNo_4_2_4.Checked = false;
            chkNo_4_2_5.Checked = false;
            txt4_2_5_1.Text = "";

            //5
            chkNo_5_1.Checked = false;
            txt5_1_1.Text = "";
            chkNo_5_2.Checked = false;
            txt5_2_1.Text = "";

            //6
            chkNo_6_1.Checked = false;
            chkNo_6_1_1.Checked = false;
            txt6_1_1_1.Text = "";
            txt6_1_1_2.Text = "";

            chkNo_6_2.Checked = false;
            chkNo_6_2_1.Checked = false;
            txt6_2_1_1.Text = "";
            txt6_2_1_2.Text = "";

            //7
            chkNo_7_1.Checked = false;
            chkNo_7_2.Checked = false;
            txt7_2_1.Text = "";
            txt7_2_2.Text = "";


            //8

            chkNo_8_Y.Checked = false;
            chkNo_8_N.Checked = true;

            chkNo_8_1.Checked = false;
            chkNo_8_2.Checked = false;
            chkNo_8_3.Checked = false;
            chkNo_8_4.Checked = false;
            chkNo_8_5.Checked = false;
            txt8_5_1.Text = "";


            //9
            chkNo_9_1.Checked = false;
            chkNo_9_2.Checked = false;

            chkNo_9_2_1.Checked = false;
            txt9_2_1_1.Text = "";
            txt9_2_1_2.Text = "";
            chkNo_9_2_2.Checked = false;
            txt9_2_2_1.Text = "";
            txt9_2_2_2.Text = "";
            chkNo_9_2_3.Checked = false;
            txt9_2_3_1.Text = "";
            txt9_2_3_2.Text = "";
            chkNo_9_2_4.Checked = false;
            txt9_2_4_1.Text = "";
            txt9_2_4_2.Text = "";
            chkNo_9_2_5.Checked = false;
            txt9_2_5_1.Text = "";
            txt9_2_5_2.Text = "";
            chkNo_9_2_6.Checked = false;
            txt9_2_6_1.Text = "";
            txt9_2_6_2.Text = "";
            chkNo_9_2_7.Checked = false;
            txt9_2_7_1.Text = "";
            txt9_2_7_2.Text = "";


            //10
            chkNo_10_1.Checked = false;
            chkNo_10_2.Checked = false;
            txt10_2_1.Text = "";

            txt10_3_1.Text = "";
            txt10_3_2.Text = "";
            txt10_3_3.Text = "";
            txt10_3_4.Text = "";
            txt10_4_1.Text = "";
            txt10_4_2.Text = "";
            txt10_4_3.Text = "";
            txt10_4_4.Text = "";
            txt10_4_5.Text = "";

            chkNo_10_5_1.Checked = false;
            chkNo_10_5_2.Checked = false;
            chkNo_10_5_3.Checked = false;
            chkNo_10_5_4.Checked = false;
            chkNo_10_5_5.Checked = false;
            chkNo_10_5_6.Checked = false;
            chkNo_10_5_7.Checked = false;
            chkNo_10_5_8.Checked = false;
            chkNo_10_5_9.Checked = false;
            chkNo_10_5_10.Checked = false;
            chkNo_10_5_11.Checked = false;
            txt10_5_11_1.Text = "";

            txt11_Fa.Text = "";


            //12 해외여행력
            chkNo_12_1.Checked = false;
            chkNo_12_2.Checked = false;


            //13.해열제복용
            chkNo_13_1.Checked = false;
            chkNo_13_2.Checked = false;


            txtRemark.Text = "";
            txtEMRNO.Text = "";
        }

        void Read_Munjin_Data(string argSTS, string argPano, string argDeptCode, string argBDate)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT BDATE,PANO,DEPTCODE,DRCODE,REMARK,ENTSABUN,ENTDATE,DELDATE,CHK,TMUN_H, TMUN_W, ";
                SQL = SQL + ComNum.VBLF + " TMUN1,TMUN2Y,TMUN2N,TMUN2_1,TMUN2_1_1,TMUN2_2,TMUN2_2_1,TMUN2_3,";
                SQL = SQL + ComNum.VBLF + " TMUN2_3_1,TMUN2_4,TMUN2_4_1,TMUN2_5,TMUN2_5_1,TMUN2_6,TMUN2_6_1,";
                SQL = SQL + ComNum.VBLF + " TMUN2_7,TMUN2_7_1,TMUN2_8,TMUN2_8_1,TMUN2_9,TMUN2_9_1, ";
                SQL = SQL + ComNum.VBLF + " TMUN2_10,TMUN2_10_1,TMUN3_1,TMUN3_2,TMUN3_2_1,TMUN3_2_1_1,TMUN3_2_2,TMUN3_2_2_1,TMUN3_2_3,TMUN3_2_3_1, ";
                SQL = SQL + ComNum.VBLF + " TMUN3_2_4,TMUN3_2_4_1,TMUN3_2_5,TMUN3_2_5_1,TMUN3_2_6,TMUN3_2_6_1,TMUN3_2_7,TMUN3_2_7_1,TMUN4_1,TMUN4_2, ";
                SQL = SQL + ComNum.VBLF + " TMUN4_2_1,TMUN4_2_2,TMUN4_2_3,TMUN4_2_4,TMUN4_2_5,TMUN4_2_5_1,TMUN5_1,TMUN5_1_1,TMUN5_2,TMUN5_2_1, ";
                SQL = SQL + ComNum.VBLF + " TMUN6_1,TMUN6_1_1,TMUN6_1_2,TMUN6_1_3,TMUN6_2,TMUN6_2_1,TMUN6_2_2,TMUN6_2_3, ";
                SQL = SQL + ComNum.VBLF + " TMUN7_1,TMUN7_2,TMUN7_2_1,TMUN7_2_2,TMUN8_1,TMUN8_2,TMUN8_3,TMUN8_4,TMUN8_5, ";
                SQL = SQL + ComNum.VBLF + " TMUN8_5_1,TMUN9_1,TMUN9_2,TMUN9_2_1,TMUN9_2_1_1,TMUN9_2_1_2,TMUN9_2_2,TMUN9_2_2_1,TMUN9_2_2_2,TMUN9_2_3, ";
                SQL = SQL + ComNum.VBLF + " TMUN9_2_3_1,TMUN9_2_3_2,TMUN9_2_4,TMUN9_2_4_1,TMUN9_2_4_2,";
                SQL = SQL + ComNum.VBLF + " TMUN9_2_5,TMUN9_2_5_1,TMUN9_2_5_2,TMUN9_2_6,TMUN9_2_6_1, ";
                SQL = SQL + ComNum.VBLF + " TMUN9_2_6_2,TMUN9_2_7,TMUN9_2_7_1,TMUN9_2_7_2,TMUN10_1,";
                SQL = SQL + ComNum.VBLF + " TMUN10_2,TMUN10_2_1,TMUN10_3_1,TMUN10_3_2,TMUN10_3_3,TMUN10_3_4, ";
                SQL = SQL + ComNum.VBLF + " TMUN10_4_1,TMUN10_4_2,TMUN10_4_3,TMUN10_4_4,TMUN10_4_5,TMUN10_5_1,TMUN10_5_2, ";
                SQL = SQL + ComNum.VBLF + " TMUN10_5_3,TMUN10_5_4,TMUN10_5_5,TMUN10_5_6,TMUN10_5_7,TMUN10_5_8,TMUN10_5_9, ";
                SQL = SQL + ComNum.VBLF + " TMUN10_5_10,TMUN10_5_11,TMUN10_5_11_1,tmun11_f1,tmun12_t1,tmun13_drug1,";
                SQL = SQL + ComNum.VBLF + " Remark, ENTSABUN, ENTDATE,ROWID, EMRNO ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_DEPT_MUNJIN ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + argPano + "' ";

                if (argSTS == "1" || argSTS == "2")
                {
                    SQL = SQL + ComNum.VBLF + "  AND DEPTCODE ='" + argDeptCode + "' ";
                }
                else
                {
                    if (string.Compare(argBDate, "2015-09-08") >= 0)
                    {
                        //통합 최초과 저장시 만 과저장됨 , 정신과제외
                        SQL = SQL + ComNum.VBLF + "  AND DEPTCODE <> 'NP' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND DEPTCODE ='" + argDeptCode + "' ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "  AND BDATE =TO_DATE('" + argBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='')";

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
                    if (argSTS != "2")
                    {
                        FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    }

                    txtHeight.Text = dt.Rows[0]["TMUN_H"].ToString().Trim();
                    txtWeight.Text = dt.Rows[0]["TMUN_W"].ToString().Trim();

                    //1
                    txt1.Text = dt.Rows[0]["TMUN1"].ToString().Trim();

                    //2
                    chkNo_2_1N.Checked = false;
                    if (dt.Rows[0]["TMUN2N"].ToString().Trim() == "Y")
                    {
                        chkNo_2_1N.Checked = true;
                    }
                    chkNo_2_1Y.Checked = false;
                    if (dt.Rows[0]["TMUN2Y"].ToString().Trim() == "Y")
                    {
                        chkNo_2_1Y.Checked = true;
                    }
                    chkNo_2_1.Checked = false;
                    if (dt.Rows[0]["TMUN2_1"].ToString().Trim() == "Y")
                    {
                        chkNo_2_1.Checked = true;
                    }
                    txt2_1.Text = dt.Rows[0]["TMUN2_1_1"].ToString().Trim();
                    chkNo_2_2.Checked = false;
                    if (dt.Rows[0]["TMUN2_2"].ToString().Trim() == "Y")
                    {
                        chkNo_2_2.Checked = true;
                    }
                    txt2_2.Text = dt.Rows[0]["TMUN2_2_1"].ToString().Trim();
                    chkNo_2_3.Checked = false;
                    if (dt.Rows[0]["TMUN2_3"].ToString().Trim() == "Y")
                    {
                        chkNo_2_3.Checked = true;
                    }
                    txt2_3.Text = dt.Rows[0]["TMUN2_3_1"].ToString().Trim();
                    chkNo_2_4.Checked = false;
                    if (dt.Rows[0]["TMUN2_4"].ToString().Trim() == "Y")
                    {
                        chkNo_2_4.Checked = true;
                    }
                    txt2_4.Text = dt.Rows[0]["TMUN2_4_1"].ToString().Trim();
                    chkNo_2_5.Checked = false;
                    if (dt.Rows[0]["TMUN2_5"].ToString().Trim() == "Y")
                    {
                        chkNo_2_5.Checked = true;
                    }
                    txt2_5.Text = dt.Rows[0]["TMUN2_5_1"].ToString().Trim();
                    chkNo_2_6.Checked = false;
                    if (dt.Rows[0]["TMUN2_6"].ToString().Trim() == "Y")
                    {
                        chkNo_2_6.Checked = true;
                    }
                    txt2_6.Text = dt.Rows[0]["TMUN2_6_1"].ToString().Trim();
                    chkNo_2_7.Checked = false;
                    if (dt.Rows[0]["TMUN2_7"].ToString().Trim() == "Y")
                    {
                        chkNo_2_7.Checked = true;
                    }
                    txt2_7.Text = dt.Rows[0]["TMUN2_7_1"].ToString().Trim();
                    chkNo_2_8.Checked = false;
                    if (dt.Rows[0]["TMUN2_8"].ToString().Trim() == "Y")
                    {
                        chkNo_2_8.Checked = true;
                    }
                    txt2_8.Text = dt.Rows[0]["TMUN2_8_1"].ToString().Trim();
                    chkNo_2_9.Checked = false;
                    if (dt.Rows[0]["TMUN2_9"].ToString().Trim() == "Y")
                    {
                        chkNo_2_9.Checked = true;
                    }
                    txt2_9.Text = dt.Rows[0]["TMUN2_9_1"].ToString().Trim();
                    chkNo_2_10.Checked = false;
                    if (dt.Rows[0]["TMUN2_10"].ToString().Trim() == "Y")
                    {
                        chkNo_2_10.Checked = true;
                    }
                    txt2_10.Text = dt.Rows[0]["TMUN2_10_1"].ToString().Trim();


                    //3
                    chkNo_3_1.Checked = false;
                    if (dt.Rows[0]["TMUN3_1"].ToString().Trim() == "Y")
                    {
                        chkNo_3_1.Checked = true;
                    }
                    chkNo_3_2.Checked = false;
                    if (dt.Rows[0]["TMUN3_2"].ToString().Trim() == "Y")
                    {
                        chkNo_3_2.Checked = true;
                    }


                    chkNo_3_2_1.Checked = false;
                    if (dt.Rows[0]["TMUN3_2_1"].ToString().Trim() == "Y")
                    {
                        chkNo_3_2_1.Checked = true;
                    }
                    txt3_2_1.Text = dt.Rows[0]["TMUN3_2_1_1"].ToString().Trim();
                    chkNo_3_2_2.Checked = false;
                    if (dt.Rows[0]["TMUN3_2_2"].ToString().Trim() == "Y")
                    {
                        chkNo_3_2_2.Checked = true;
                    }
                    txt3_2_2.Text = dt.Rows[0]["TMUN3_2_2_1"].ToString().Trim();
                    chkNo_3_2_3.Checked = false;
                    if (dt.Rows[0]["TMUN3_2_3"].ToString().Trim() == "Y")
                    {
                        chkNo_3_2_3.Checked = true;
                    }
                    txt3_2_3.Text = dt.Rows[0]["TMUN3_2_3_1"].ToString().Trim();
                    chkNo_3_2_4.Checked = false;
                    if (dt.Rows[0]["TMUN3_2_4"].ToString().Trim() == "Y")
                    {
                        chkNo_3_2_4.Checked = true;
                    }
                    txt3_2_4.Text = dt.Rows[0]["TMUN3_2_4_1"].ToString().Trim();
                    chkNo_3_2_5.Checked = false;
                    if (dt.Rows[0]["TMUN3_2_5"].ToString().Trim() == "Y")
                    {
                        chkNo_3_2_5.Checked = true;
                    }
                    txt3_2_5.Text = dt.Rows[0]["TMUN3_2_5_1"].ToString().Trim();
                    chkNo_3_2_6.Checked = false;
                    if (dt.Rows[0]["TMUN3_2_6"].ToString().Trim() == "Y")
                    {
                        chkNo_3_2_6.Checked = true;
                    }
                    txt3_2_6.Text = dt.Rows[0]["TMUN3_2_6_1"].ToString().Trim();
                    chkNo_3_2_7.Checked = false;
                    if (dt.Rows[0]["TMUN3_2_7"].ToString().Trim() == "Y")
                    {
                        chkNo_3_2_7.Checked = true;
                    }
                    txt3_2_7.Text = dt.Rows[0]["TMUN3_2_7_1"].ToString().Trim();


                    //4
                    chkNo_4_1.Checked = false;
                    if (dt.Rows[0]["TMUN4_1"].ToString().Trim() == "Y")
                    {
                        chkNo_4_1.Checked = true;
                    }
                    chkNo_4_2.Checked = false;
                    if (dt.Rows[0]["TMUN4_2"].ToString().Trim() == "Y")
                    {
                        chkNo_4_2.Checked = true;
                    }
                    chkNo_4_2_1.Checked = false;
                    if (dt.Rows[0]["TMUN4_2_1"].ToString().Trim() == "Y")
                    {
                        chkNo_4_2_1.Checked = true;
                    }
                    chkNo_4_2_2.Checked = false;
                    if (dt.Rows[0]["TMUN4_2_2"].ToString().Trim() == "Y")
                    {
                        chkNo_4_2_2.Checked = true;
                    }
                    chkNo_4_2_3.Checked = false;
                    if (dt.Rows[0]["TMUN4_2_3"].ToString().Trim() == "Y")
                    {
                        chkNo_4_2_3.Checked = true;
                    }
                    chkNo_4_2_4.Checked = false;
                    if (dt.Rows[0]["TMUN4_2_4"].ToString().Trim() == "Y")
                    {
                        chkNo_4_2_4.Checked = true;
                    }
                    chkNo_4_2_5.Checked = false;
                    if (dt.Rows[0]["TMUN4_2_5"].ToString().Trim() == "Y")
                    {
                        chkNo_4_2_5.Checked = true;
                    }
                    txt4_2_5_1.Text = dt.Rows[0]["TMUN4_2_5_1"].ToString().Trim();


                    //5
                    chkNo_5_1.Checked = false;
                    if (dt.Rows[0]["TMUN5_1"].ToString().Trim() == "Y")
                    {
                        chkNo_5_1.Checked = true;
                    }
                    txt5_1_1.Text = dt.Rows[0]["TMUN5_1_1"].ToString().Trim();
                    chkNo_5_2.Checked = false;
                    if (dt.Rows[0]["TMUN5_2"].ToString().Trim() == "Y")
                    {
                        chkNo_5_2.Checked = true;
                    }

                    if (VB.IsDate(dt.Rows[0]["TMUN5_2_1"].ToString().Trim()) == true)
                    {
                        txt5_2_1.Text = Convert.ToDateTime(dt.Rows[0]["TMUN5_2_1"].ToString().Trim()).ToShortDateString();
                    }
                    

                    //6
                    chkNo_6_1.Checked = false;
                    if (dt.Rows[0]["TMUN6_1"].ToString().Trim() == "Y")
                    {
                        chkNo_6_1.Checked = true;
                    }
                    chkNo_6_1_1.Checked = false;
                    if (dt.Rows[0]["TMUN6_1_1"].ToString().Trim() == "Y")
                    {
                        chkNo_6_1_1.Checked = true;
                    }
                    txt6_1_1_1.Text = dt.Rows[0]["TMUN6_1_2"].ToString().Trim();
                    txt6_1_1_2.Text = dt.Rows[0]["TMUN6_1_3"].ToString().Trim();
                    chkNo_6_2.Checked = false;
                    if (dt.Rows[0]["TMUN6_2"].ToString().Trim() == "Y")
                    {
                        chkNo_6_2.Checked = true;
                    }
                    chkNo_6_2_1.Checked = false;
                    if (dt.Rows[0]["TMUN6_2_1"].ToString().Trim() == "Y")
                    {
                        chkNo_6_2_1.Checked = true;
                    }
                    txt6_2_1_1.Text = dt.Rows[0]["TMUN6_2_2"].ToString().Trim();
                    txt6_2_1_2.Text = dt.Rows[0]["TMUN6_2_3"].ToString().Trim();


                    //7
                    chkNo_7_1.Checked = false;
                    if (dt.Rows[0]["TMUN7_1"].ToString().Trim() == "Y")
                    {
                        chkNo_7_1.Checked = true;
                    }
                    chkNo_7_2.Checked = false;
                    if (dt.Rows[0]["TMUN7_2"].ToString().Trim() == "Y")
                    {
                        chkNo_7_2.Checked = true;
                    }

                    if (VB.IsDate(dt.Rows[0]["TMUN7_2_1"].ToString().Trim()) == true)
                    {
                        txt7_2_1.Text = Convert.ToDateTime(dt.Rows[0]["TMUN7_2_1"].ToString().Trim()).ToShortDateString();
                    }
                    txt7_2_2.Text = dt.Rows[0]["TMUN7_2_2"].ToString().Trim();

                    //8
                    chkNo_8_N.Checked = true;

                    chkNo_8_1.Checked = false;
                    if (dt.Rows[0]["TMUN8_1"].ToString().Trim() == "Y")
                    {
                        chkNo_8_1.Checked = true;
                        chkNo_8_Y.Checked = true;
                        chkNo_8_N.Checked = false;
                    }
                    chkNo_8_2.Checked = false;
                    if (dt.Rows[0]["TMUN8_2"].ToString().Trim() == "Y")
                    {
                        chkNo_8_2.Checked = true;
                        chkNo_8_Y.Checked = true;
                        chkNo_8_N.Checked = false;
                    }
                    chkNo_8_3.Checked = false;
                    if (dt.Rows[0]["TMUN8_3"].ToString().Trim() == "Y")
                    {
                        chkNo_8_3.Checked = true;
                        chkNo_8_Y.Checked = true;
                        chkNo_8_N.Checked = false;
                    }
                    chkNo_8_4.Checked = false;
                    if (dt.Rows[0]["TMUN8_4"].ToString().Trim() == "Y")
                    {
                        chkNo_8_4.Checked = true;
                        chkNo_8_Y.Checked = true;
                        chkNo_8_N.Checked = false;
                    }
                    chkNo_8_5.Checked = false;
                    if (dt.Rows[0]["TMUN8_5"].ToString().Trim() == "Y")
                    {
                        chkNo_8_5.Checked = true;
                        chkNo_8_Y.Checked = true;
                        chkNo_8_N.Checked = false;
                    }
                    txt8_5_1.Text = dt.Rows[0]["TMUN8_5_1"].ToString().Trim();


                    //9
                    chkNo_9_1.Checked = false;
                    if (dt.Rows[0]["TMUN9_1"].ToString().Trim() == "Y")
                    {
                        chkNo_9_1.Checked = true;
                    }
                    chkNo_9_2.Checked = false;
                    if (dt.Rows[0]["TMUN9_2"].ToString().Trim() == "Y")
                    {
                        chkNo_9_2.Checked = true;
                    }
                    chkNo_9_2_1.Checked = false;
                    if (dt.Rows[0]["TMUN9_2_1"].ToString().Trim() == "Y")
                    {
                        chkNo_9_2_1.Checked = true;
                    }


                    txt9_2_1_1.Text = dt.Rows[0]["TMUN9_2_1_1"].ToString().Trim();
                    txt9_2_1_2.Text = dt.Rows[0]["TMUN9_2_1_2"].ToString().Trim();
                    chkNo_9_2_2.Checked = false;
                    if (dt.Rows[0]["TMUN9_2_2"].ToString().Trim() == "Y")
                    {
                        chkNo_9_2_2.Checked = true;
                    }
                    
                    txt9_2_2_1.Text = dt.Rows[0]["TMUN9_2_2_1"].ToString().Trim();                    
                    txt9_2_2_2.Text = dt.Rows[0]["TMUN9_2_2_2"].ToString().Trim();
                    chkNo_9_2_3.Checked = false;
                    if (dt.Rows[0]["TMUN9_2_3"].ToString().Trim() == "Y")
                    {
                        chkNo_9_2_3.Checked = true;
                    }
                    
                    txt9_2_3_1.Text = dt.Rows[0]["TMUN9_2_3_1"].ToString().Trim(); 
                    txt9_2_3_2.Text = dt.Rows[0]["TMUN9_2_3_2"].ToString().Trim();
                    chkNo_9_2_4.Checked = false;
                    if (dt.Rows[0]["TMUN9_2_4"].ToString().Trim() == "Y")
                    {
                        chkNo_9_2_4.Checked = true;
                    }
                    
                    txt9_2_4_1.Text = dt.Rows[0]["TMUN9_2_4_1"].ToString().Trim();
                    txt9_2_4_2.Text = dt.Rows[0]["TMUN9_2_4_2"].ToString().Trim();
                    chkNo_9_2_5.Checked = false;
                    if (dt.Rows[0]["TMUN9_2_5"].ToString().Trim() == "Y")
                    {
                        chkNo_9_2_5.Checked = true;
                    }
                    
                    txt9_2_5_1.Text = dt.Rows[0]["TMUN9_2_5_1"].ToString().Trim();                    
                    txt9_2_5_2.Text = dt.Rows[0]["TMUN9_2_5_2"].ToString().Trim();
                    chkNo_9_2_6.Checked = false;
                    if (dt.Rows[0]["TMUN9_2_6"].ToString().Trim() == "Y")
                    {
                        chkNo_9_2_6.Checked = true;
                    }
                    
                    txt9_2_6_1.Text = dt.Rows[0]["TMUN9_2_6_1"].ToString().Trim();
                    txt9_2_6_2.Text = dt.Rows[0]["TMUN9_2_6_2"].ToString().Trim();
                    chkNo_9_2_7.Checked = false;
                    if (dt.Rows[0]["TMUN9_2_7"].ToString().Trim() == "Y")
                    {
                        chkNo_9_2_7.Checked = true;
                    }
                    
                    txt9_2_7_1.Text = dt.Rows[0]["TMUN9_2_7_1"].ToString().Trim();                    
                    txt9_2_7_2.Text = dt.Rows[0]["TMUN9_2_7_2"].ToString().Trim();
                    

                    //10
                    chkNo_10_1.Checked = false;
                    if (dt.Rows[0]["TMUN10_1"].ToString().Trim() == "Y")
                    {
                        chkNo_10_1.Checked = true;
                    }
                    chkNo_10_2.Checked = false;
                    if (dt.Rows[0]["TMUN10_2"].ToString().Trim() == "Y")
                    {
                        chkNo_10_2.Checked = true;
                    }
                    txt10_2_1.Text = dt.Rows[0]["TMUN10_2_1"].ToString().Trim();
                    txt10_3_1.Text = dt.Rows[0]["TMUN10_3_1"].ToString().Trim();
                    txt10_3_2.Text = dt.Rows[0]["TMUN10_3_2"].ToString().Trim();
                    txt10_3_3.Text = dt.Rows[0]["TMUN10_3_3"].ToString().Trim();
                    txt10_3_4.Text = dt.Rows[0]["TMUN10_3_4"].ToString().Trim();
                    txt10_4_1.Text = dt.Rows[0]["TMUN10_4_1"].ToString().Trim();
                    txt10_4_2.Text = dt.Rows[0]["TMUN10_4_2"].ToString().Trim();
                    txt10_4_3.Text = dt.Rows[0]["TMUN10_4_3"].ToString().Trim();
                    txt10_4_4.Text = dt.Rows[0]["TMUN10_4_4"].ToString().Trim();
                    txt10_4_5.Text = dt.Rows[0]["TMUN10_4_5"].ToString().Trim();


                    chkNo_10_5_1.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_1"].ToString().Trim() == "Y")
                    {
                        chkNo_10_5_1.Checked = true;
                    }
                    chkNo_10_5_2.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_2"].ToString().Trim() == "Y")
                    {
                        chkNo_10_5_2.Checked = true;
                    }
                    chkNo_10_5_3.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_3"].ToString().Trim() == "Y")
                    {
                        chkNo_10_5_3.Checked = true;
                    }
                    chkNo_10_5_4.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_4"].ToString().Trim() == "Y")
                    {
                        chkNo_10_5_4.Checked = true;
                    }
                    chkNo_10_5_5.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_5"].ToString().Trim() == "Y")
                    {
                        chkNo_10_5_5.Checked = true;
                    }
                    chkNo_10_5_6.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_6"].ToString().Trim() == "Y")
                    {
                        chkNo_10_5_6.Checked = true;
                    }
                    chkNo_10_5_7.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_7"].ToString().Trim() == "Y")
                    {
                        chkNo_10_5_7.Checked = true;
                    }
                    chkNo_10_5_8.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_8"].ToString().Trim() == "Y")
                    {
                        chkNo_10_5_8.Checked = true;
                    }
                    chkNo_10_5_9.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_9"].ToString().Trim() == "Y")
                    {
                        chkNo_10_5_9.Checked = true;
                    }
                    chkNo_10_5_10.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_10"].ToString().Trim() == "Y")
                    {
                        chkNo_10_5_10.Checked = true;
                    }
                    chkNo_10_5_11.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_11"].ToString().Trim() == "Y")
                    {
                        chkNo_10_5_11.Checked = true;
                    }
                    txt10_5_11_1.Text = dt.Rows[0]["TMUN10_5_11_1"].ToString().Trim();


                    txt11_Fa.Text = dt.Rows[0]["tmun11_f1"].ToString().Trim();

                    if (dt.Rows[0]["TMUN12_T1"].ToString().Trim() == "N")
                    {
                        chkNo_12_1.Checked = true;
                    }
                    else if (dt.Rows[0]["TMUN12_T1"].ToString().Trim() == "Y")
                    {
                        chkNo_12_2.Checked = true;
                    }

                    if (dt.Rows[0]["TMUN13_Drug1"].ToString().Trim() == "N")
                    {
                        chkNo_13_1.Checked = true;
                    }
                    else if (dt.Rows[0]["TMUN13_Drug1"].ToString().Trim() == "Y")
                    {
                        chkNo_13_2.Checked = true;
                    }


                    txtRemark.Text = dt.Rows[0]["Remark"].ToString().Trim();
                    txtEMRNO.Text = dt.Rows[0]["EMRNO"].ToString().Trim();


                    ComFunc.MsgBox(dt.Rows[0]["DeptCode"].ToString().Trim() + "과 에서 " + argBDate + "에 등록된 자료입니다.");
                }
                else
                {
                    ComFunc.MsgBox("등록된 예진표자료가 없습니다.." + ComNum.VBLF + ComNum.VBLF + "타과 내역을 불러오실려면 과코드를 더블클릭하세요.");
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

        void Read_Munjin_Dept(string argPano, string argDeptCode, string argBDate)
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssDept_Sheet1.Cells[0, 0, ssDept_Sheet1.RowCount - 1, ssDept_Sheet1.ColumnCount - 1].Text = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DeptCode,TO_CHAR(BDate,'YYYY-MM-DD') BDATE ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER  ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + argBDate + "','YYYY-MM-DD')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssDept_Sheet1.Cells[0, i].Text = dt.Rows[i]["DeptCode"].ToString().Trim();

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT DeptCode,BDate ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_DEPT_MUNJIN b ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + argPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + dt.Rows[i]["DeptCode"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + argBDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE ='')";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssDept_Sheet1.Cells[0, i].BackColor = Color.FromArgb(128, 255, 128);
                    }
                    else
                    {
                        ssDept_Sheet1.Cells[0, i].BackColor = Color.FromArgb(255, 255, 255);
                    }

                    dt1.Dispose();
                    dt1 = null;
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

        private void ssDept_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strDEPT = "";

            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            strDEPT = ssDept_Sheet1.Cells[e.Row, e.Column].Text.Trim();

            if (strDEPT == "NP" || strDEPT == "OG" || strDEPT == "OS")
            {
                ComFunc.MsgBox("전용예진표에서는 다른과 내역을 확인할수 없습니다.");
            }
            else
            {
                Read_Munjin_Data("2", FstrPano, strDEPT, FstrBDate);
            }
        }

        private void txt10_2_1_TextChanged(object sender, EventArgs e)
        {
            if (VB.Val(txt10_2_1.Text) >= 4)
            {
                txt10_2_1.BackColor = Color.FromArgb(255, 128, 128);
            }
            else
            {
                txt10_2_1.BackColor = Color.White;
            }
        }

        void Gesan_Tot()
        {
            txt10_2_1.Text = (VB.Val(txt10_3_4.Text) + VB.Val(txt10_4_1.Text) + VB.Val(txt10_4_2.Text) + VB.Val(txt10_4_3.Text) + VB.Val(txt10_4_4.Text) + VB.Val(txt10_4_5.Text)).ToString();

            if (VB.Val(txt10_2_1.Text) >= 4)
            {
                txt10_2_1.BackColor = Color.FromArgb(255, 128, 128);
            }
            else
            {
                txt10_2_1.BackColor = Color.White;
            }
        }

        private void ssView_txt_TextChanged(object sender, EventArgs e)
        {
            Gesan_Tot();
        }

        void Build_Data()
        {
            string strData = "";
            string strSUB = "";
            string strSUB2 = "";

            if (FstrDept == "EN")
            {
                if (READ_DEPT_CHOJEA(FstrPano, FstrDept, FstrDrCode) == "과초진")
                {
                    strData += " <초진기록지> ---------------- " + ComNum.VBLF + ComNum.VBLF;
                }
            }

            if (chkNo_0.Checked == true)
            {
                strData += " ▣ 키/몸무게 : " + txtHeight.Text.Trim() + "/" + txtWeight.Text.Trim();
            }


            if (chkNo_14.Checked == true)
            {
                strData = strData + ComNum.VBLF;
                if (chkNo_12_1.Checked == true)
                {
                    strData = strData + " ▣ 최근 1개월 내 해외여행력 : 무";
                }
                else if (chkNo_12_2.Checked == true)
                {
                    strData = strData + " ▣ 최근 1개월 내 해외여행력 : 유";
                }
            }





            if (chkNo_1.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 주호소";
                strData += ComNum.VBLF + txt1.Text.Trim();
            }

            if (chkNo_2.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 과거병력";

                if (chkNo_2_1N.Checked == true)
                {
                    strData += " : 없음";
                }

                if (chkNo_2_1Y.Checked == true)
                {
                    strSUB = "";
                    if (chkNo_2_1.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 간질환" + (txt2_1.Text.Trim() != "" ? " : " + txt2_1.Text.Trim() : "");
                    }
                    if (chkNo_2_2.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 고혈압" + (txt2_2.Text.Trim() != "" ? " : " + txt2_2.Text.Trim() : "");
                    }
                    if (chkNo_2_3.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 당뇨" + (txt2_3.Text.Trim() != "" ? " : " + txt2_3.Text.Trim() : "");
                    }
                    if (chkNo_2_4.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 뇌질환" + (txt2_4.Text.Trim() != "" ? " : " + txt2_4.Text.Trim() : "");
                    }
                    if (chkNo_2_5.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 심질환" + (txt2_5.Text.Trim() != "" ? " : " + txt2_5.Text.Trim() : "");
                    }
                    if (chkNo_2_6.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 신질환" + (txt2_6.Text.Trim() != "" ? " : " + txt2_6.Text.Trim() : "");
                    }
                    if (chkNo_2_7.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 호흡기" + (txt2_7.Text.Trim() != "" ? " : " + txt2_7.Text.Trim() : "");
                    }
                    if (chkNo_2_8.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 소화기" + (txt2_8.Text.Trim() != "" ? " : " + txt2_8.Text.Trim() : "");
                    }
                    if (chkNo_2_9.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 수술력" + (txt2_9.Text.Trim() != "" ? " : " + txt2_9.Text.Trim() : "");
                    }
                    if (chkNo_2_10.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 기타" + (txt2_10.Text.Trim() != "" ? " : " + txt2_10.Text.Trim() : "");
                    }
                    strData += strSUB;
                }
            }

            if (chkNo_3.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 복용력";

                if (chkNo_3_1.Checked == true)
                {
                    strData += " : 없음";
                }

                if (chkNo_3_2.Checked == true)
                {
                    strSUB = "";
                    strSUB += ComNum.VBLF;
                    if (chkNo_3_2_1.Checked == true)
                    {
                        strSUB += " 고혈압" + (txt3_2_1.Text.Trim() != "" ? " : " + txt3_2_1.Text.Trim() : "");
                    }
                    strSUB += ComNum.VBLF;
                    if (chkNo_3_2_2.Checked == true)
                    {
                        strSUB += " 당뇨약" + (txt3_2_2.Text.Trim() != "" ? " : " + txt3_2_2.Text.Trim() : "");
                    }
                    strSUB += ComNum.VBLF;
                    if (chkNo_3_2_3.Checked == true)
                    {
                        strSUB += " 혈전용해제" + (txt3_2_3.Text.Trim() != "" ? " : " + txt3_2_3.Text.Trim() : "");
                    }
                    strSUB += ComNum.VBLF;
                    if (chkNo_3_2_4.Checked == true)
                    {
                        strSUB += " 항응고제" + (txt3_2_4.Text.Trim() != "" ? " : " + txt3_2_4.Text.Trim() : "");
                    }
                    strSUB += ComNum.VBLF;
                    if (chkNo_3_2_5.Checked == true)
                    {
                        strSUB += " 고지혈증" + (txt3_2_5.Text.Trim() != "" ? " : " + txt3_2_5.Text.Trim() : "");
                    }
                    strSUB += ComNum.VBLF;
                    if (chkNo_3_2_6.Checked == true)
                    {
                        strSUB += " 골다공증약" + (txt3_2_6.Text.Trim() != "" ? " : " + txt3_2_6.Text.Trim() : "");
                    }
                    strSUB += ComNum.VBLF;
                    if (chkNo_3_2_7.Checked == true)
                    {
                        strSUB += " 기타" + (txt3_2_7.Text.Trim() != "" ? " : " + txt3_2_7.Text.Trim() : "");
                    }
                    strData += strSUB;
                }
            }

            if (chkNo_4.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 알러지";

                if (chkNo_4_1.Checked == true)
                {
                    strData += " : 없음";
                }

                if (chkNo_4_2.Checked == true)
                {
                    strSUB = "";
                    strSUB += ComNum.VBLF;
                    if (chkNo_4_2_1.Checked == true)
                    {
                        strSUB += " 음식";
                    }
                    strSUB += ComNum.VBLF;
                    if (chkNo_4_2_2.Checked == true)
                    {
                        strSUB += " 계절";
                    }
                    strSUB += ComNum.VBLF;
                    if (chkNo_4_2_3.Checked == true)
                    {
                        strSUB += " 조영제";
                    }
                    strSUB += ComNum.VBLF;
                    if (chkNo_4_2_4.Checked == true)
                    {
                        strSUB += " 항생제";
                    }
                    strSUB += ComNum.VBLF;
                    if (chkNo_4_2_5.Checked == true)
                    {
                        strSUB += " 기타" + (txt4_2_5_1.Text.Trim() != "" ? " : " + txt4_2_5_1.Text.Trim() : "");
                    }
                    strData += strSUB;
                }
            }

            if (chkNo_5.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 임신여부";

                if (chkNo_5_1.Checked == true)
                {
                    strData += " : 없음";
                }
                if (chkNo_5_2.Checked == true)
                {
                    strSUB = "";
                    strSUB += ComNum.VBLF;
                    strSUB += txt5_1_1.Text.Trim() != "" ? " 폐경나이 : " + txt5_1_1.Text.Trim() : "";
                    strSUB += ComNum.VBLF;
                    strSUB += txt5_2_1.Text.Trim() != ""? " 최종 월경일(LMP) : " + txt5_2_1.Text.Trim(): "";
                    strData += strSUB;
                }
            }

            if (chkNo_6.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 음주.흡연";

                if (chkNo_6_1.Checked == true)
                {
                    strData += ComNum.VBLF + " 음주 안함";
                }

                if (chkNo_6_1_1.Checked == true)
                {
                    strSUB = "";
                    strSUB += ComNum.VBLF;
                    strSUB += txt6_1_1_1.Text.Trim() != "" ? " 종류 : " + txt6_1_1_1.Text.Trim() + " 병/회" : "";
                    strSUB += txt6_1_1_2.Text.Trim() != "" ? " " + txt6_1_1_2.Text.Trim() + " 주/회" : "";
                    strData += strSUB;
                }

                if (chkNo_6_2.Checked == true)
                {
                    strData += ComNum.VBLF + " 흡연 안함";
                }

                if (chkNo_6_2_1.Checked == true)
                {
                    strSUB = "";
                    strSUB += ComNum.VBLF;
                    strSUB += txt6_2_1_1.Text.Trim() != "" ? " 흡연량 : " + txt6_2_1_1.Text.Trim() + " 갑/일" : "";
                    strSUB += txt6_2_1_2.Text.Trim() != "" ? " 흡연기간 : " + txt6_2_1_2.Text.Trim() + " 년" : "";
                    strData += strSUB;
                }
            }

            if (chkNo_7.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 최근예방접종";

                if (chkNo_7_1.Checked == true)
                {
                    strData += " : 없음";
                }

                if (chkNo_7_2.Checked == true)
                {
                    strSUB = "";
                    strSUB += ComNum.VBLF;
                    strSUB += txt7_2_1.Text.Trim() != "" ? " 접종시기 : " + txt7_2_1.Text.Trim() : "";
                    strSUB += txt7_2_2.Text.Trim() != "" ? " 접종명 : " + txt7_2_2.Text.Trim() : "";
                    strData += strSUB;
                }
            }

            if (chkNo_8.Checked == true)
            {
                strSUB = "";
                if (chkNo_8_1.Checked == true)
                {
                    strSUB += " 소견서";
                }
                if (chkNo_8_2.Checked == true)
                {
                    strSUB += " 진료의뢰서";
                }
                if (chkNo_8_3.Checked == true)
                {
                    strSUB += " CD";
                }
                if (chkNo_8_4.Checked == true)
                {
                    strSUB += " 검사결과지";
                }
                if (chkNo_8_5.Checked == true)
                {
                    strSUB += " 기타 : " + (txt8_5_1.Text.Trim() != "" ? txt8_5_1.Text.Trim() : "");
                }

                if (strSUB != "")
                {
                    strData += ComNum.VBLF;
                    strData += " ▣ 타병원 소견서 및 CD";
                    strData += ComNum.VBLF;
                    strData += strSUB;
                }
            }

            if (chkNo_9.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 최근검사이력";

                if (chkNo_9_1.Checked == true)
                {
                    strData += " : 없음";
                }

                if (chkNo_9_2.Checked == true)
                {
                    strSUB = "";
                    if (chkNo_9_2_1.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 위내시경";
                        if (txt9_2_1_1.Text.Trim() != "")
                        {
                            strSUB += " ( 검사일 : " + txt9_2_1_1.Text.Trim() + " )";
                        }
                        if (txt9_2_1_2.Text.Trim() != "")
                        {
                            strSUB += ComNum.VBLF;
                            strSUB += " ▶ 검사결과" + ComNum.VBLF;
                            strSUB += txt9_2_1_2.Text.Trim();
                        }
                    }

                    if (chkNo_9_2_2.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 대장내시경";
                        if (txt9_2_2_1.Text.Trim() != "")
                        {
                            strSUB += " ( 검사일 : " + txt9_2_2_1.Text.Trim() + " )";
                        }
                        if (txt9_2_2_2.Text.Trim() != "")
                        {
                            strSUB += ComNum.VBLF;
                            strSUB += " ▶ 검사결과" + ComNum.VBLF;
                            strSUB += txt9_2_2_2.Text.Trim();
                        }
                    }

                    if (chkNo_9_2_3.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 초음파";
                        if (txt9_2_3_1.Text.Trim() != "")
                        {
                            strSUB += " ( 검사일 : " + txt9_2_3_1.Text.Trim() + " )";
                        }
                        if (txt9_2_3_2.Text.Trim() != "")
                        {
                            strSUB += ComNum.VBLF;
                            strSUB += " ▶ 검사결과" + ComNum.VBLF;
                            strSUB += txt9_2_3_2.Text.Trim();
                        }
                    }

                    if (chkNo_9_2_4.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " CT";
                        if (txt9_2_4_1.Text.Trim() != "")
                        {
                            strSUB += " ( 검사일 : " + txt9_2_4_1.Text.Trim() + " )";
                        }
                        if (txt9_2_4_2.Text.Trim() != "")
                        {
                            strSUB += ComNum.VBLF;
                            strSUB += " ▶ 검사결과" + ComNum.VBLF;
                            strSUB += txt9_2_4_2.Text.Trim();
                        }
                    }

                    if (chkNo_9_2_5.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " MRI";
                        if (txt9_2_5_1.Text.Trim() != "")
                        {
                            strSUB += " ( 검사일 : " + txt9_2_5_1.Text.Trim() + " )";
                        }
                        if (txt9_2_5_2.Text.Trim() != "")
                        {
                            strSUB += ComNum.VBLF;
                            strSUB += " ▶ 검사결과" + ComNum.VBLF;
                            strSUB += txt9_2_5_2.Text.Trim();
                        }
                    }

                    if (chkNo_9_2_6.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 혈액검사";
                        if (txt9_2_6_1.Text.Trim() != "")
                        {
                            strSUB += " ( 검사일 : " + txt9_2_6_1.Text.Trim() + " )";
                        }
                        if (txt9_2_6_2.Text.Trim() != "")
                        {
                            strSUB += ComNum.VBLF;
                            strSUB += " ▶ 검사결과" + ComNum.VBLF;
                            strSUB += txt9_2_6_2.Text.Trim();
                        }
                    }

                    if (chkNo_9_2_7.Checked == true)
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 기타";
                        if (txt9_2_7_1.Text.Trim() != "")
                        {
                            strSUB += " ( 검사일 : " + txt9_2_7_1.Text.Trim() + " )";
                        }
                        if (txt9_2_7_2.Text.Trim() != "")
                        {
                            strSUB += ComNum.VBLF;
                            strSUB += " ▶ 검사결과" + ComNum.VBLF;
                            strSUB += txt9_2_7_2.Text.Trim();
                        }
                    }

                    strData += strSUB;
                }
            }

            if (chkNo_10.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 현재통증정도";

                if (chkNo_10_1.Checked == true)
                {
                    strData += " : 없음";
                }

                if (chkNo_10_2.Checked == true)
                {
                    strSUB = "";
                    strSUB2 = "";
                    strSUB += ComNum.VBLF;
                    if (txt10_2_1.Text.Trim() != "")
                    {
                        strSUB = " 통증 초기평가 : " + txt10_2_1.Text.Trim();
                    }
                    strSUB += ComNum.VBLF;
                    if (txt10_3_1.Text.Trim() != "")
                    {
                        strSUB = " 가장 심할 때 : " + txt10_3_1.Text.Trim();
                    }
                    strSUB += ComNum.VBLF;
                    if (txt10_3_2.Text.Trim() != "")
                    {
                        strSUB = " 가장 덜할 때 : " + txt10_3_2.Text.Trim();
                    }
                    strSUB += ComNum.VBLF;
                    if (txt10_3_3.Text.Trim() != "")
                    {
                        strSUB = " 견딜수 있는 수준 : " + txt10_3_3.Text.Trim();
                    }
                    strSUB += ComNum.VBLF;
                    if (txt10_3_4.Text.Trim() != "")
                    {
                        strSUB = " 하루평균 : " + txt10_3_4.Text.Trim();
                    }
                    strData += strSUB;

                    if (chkNo_10_5_1.Checked == true)
                    {
                        strSUB2 += " 날카로운 느낌(sharp)";
                    }
                    if (chkNo_10_5_2.Checked == true)
                    {
                        strSUB2 += " 둔한 느낌(dull)";
                    }
                    if (chkNo_10_5_3.Checked == true)
                    {
                        strSUB2 += " 타는듯한 느낌(burning)";
                    }
                    if (chkNo_10_5_4.Checked == true)
                    {
                        strSUB2 += " 누르는듯한 느낌(pressing)";
                    }
                    if (chkNo_10_5_5.Checked == true)
                    {
                        strSUB2 += " 칼로 벤 것처럼 아픔(cutting)";
                    }
                    if (chkNo_10_5_6.Checked == true)
                    {
                        strSUB2 += " 저린 느낌(numbing)";
                    }
                    if (chkNo_10_5_7.Checked == true)
                    {
                        strSUB2 += " 쑤시는 느낌(tingling)";
                    }
                    if (chkNo_10_5_8.Checked == true)
                    {
                        strSUB2 += " 죄는듯한 느낌(mumbing)";
                    }
                    if (chkNo_10_5_9.Checked == true)
                    {
                        strSUB2 += " 아팠다 안아팠다함(come and goes)";
                    }
                    if (chkNo_10_5_10.Checked == true)
                    {
                        strSUB2 += " 다른 부위로 퍼지듯 아픔(radiating)";
                    }
                    if (chkNo_10_5_11.Checked == true)
                    {
                        strSUB2 += " 기타";
                        if (txt10_5_11_1.Text.Trim() != "")
                        {
                            strSUB2 += " : " + txt10_5_11_1.Text.Trim();
                        }
                    }

                    if (strSUB2 != "")
                    {
                        strSUB2 = " <통증의 양상> " + ComNum.VBLF + strSUB2;
                    }

                    strData += ComNum.VBLF + strSUB2;
                }
            }

            if (chkNo_11.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 참고사항";
                strData += ComNum.VBLF + txtRemark.Text.Trim();
            }

            if (chkNo_12.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 가족력";
                strData += ComNum.VBLF + txt11_Fa.Text.Trim();
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
                chkNo_0.Checked = false;
                chkNo_14.Checked = false;
                chkNo_1.Checked = false;
                chkNo_2.Checked = false;
                chkNo_3.Checked = false;
                chkNo_4.Checked = false;
                chkNo_5.Checked = false;
                chkNo_6.Checked = false;
                chkNo_7.Checked = false;
                chkNo_8.Checked = false;
                chkNo_9.Checked = false;
                chkNo_10.Checked = false;
                chkNo_11.Checked = false;
                chkNo_12.Checked = false;
                btnAll.Text = "모든항목 선택";
            }
            else
            {
                chkNo_0.Checked = true;
                chkNo_14.Checked = true;
                chkNo_1.Checked = true;
                chkNo_2.Checked = true;
                chkNo_3.Checked = true;
                chkNo_4.Checked = true;
                chkNo_5.Checked = true;
                chkNo_6.Checked = true;
                chkNo_7.Checked = true;
                chkNo_8.Checked = true;
                chkNo_9.Checked = true;
                chkNo_10.Checked = true;
                chkNo_11.Checked = true;
                chkNo_12.Checked = true;
                btnAll.Text = "모든항목 해제";
            }
        }
    }
}
