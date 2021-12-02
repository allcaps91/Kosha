using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComEmrBase;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmMedicalInquiry_OG.cs
    /// Description     : 산부인과전용 예진표
    /// Author          : 박창욱
    /// Create Date     : 2018-01-17
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\emr\emrprt\Frm진료예진표_OG.frm(Frm진료예진표_OG.frm) >> frmMedicalInquiry_OG.cs 폼이름 재정의" />	
    public partial class frmMedicalInquiry_OG : Form
    {
        string FstrPano = "";
        string FstrSName = "";
        string FstrDept = "";
        string FstrDrCode = "";
        string FstrBDate = "";
        string FstrROWID = "";

        public frmMedicalInquiry_OG()
        {
            InitializeComponent();
        }

        public frmMedicalInquiry_OG(string strPaNo, string strPaName, string strDept, string strDrCode, string strBDate)
        {
            InitializeComponent();
            this.FstrPano = strPaNo;
            this.FstrSName = strPaName;
            this.FstrDept = strDept;
            this.FstrDrCode = strDrCode;
            this.FstrBDate = strBDate;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            if (clsType.User.JobMan == "간호사")
            {
                ComFunc.MsgBox("간호사일 경우 경과기록을 작성할 수 없습니다.");
                return;
            }

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

        void Build_Data()
        {
            string strData = "";
            string strSUB = "";
            string strSUB2 = "";

            if (chkNo_0.Checked == true)
            {
                if (txtHeight.Text.Trim() != "" || txtWeight.Text.Trim() != "")
                {
                    strData = strData + ComNum.VBLF;
                    strData = strData + " ▣ 키/몸무게 : " + txtHeight.Text.Trim() + "/" + txtWeight.Text.Trim();
                }
            }

            if (chkNo_13.Checked == true)
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
                strData = strData + ComNum.VBLF;
                strData = strData + " ▣ 주호소";
                strData = strData + ComNum.VBLF + txt1.Text.Trim();
            }

            if (chkNo_2.Checked == true)
            {
                strData = strData + ComNum.VBLF;
                strData = strData + " ▣ 마지막 자궁경부암 검사 일자 : ";
                strData = strData + ComNum.VBLF + txtOG2.Text.Trim();
            }

            if (chkNo_3.Checked == true)
            {
                strData = strData + ComNum.VBLF;
                strData = strData + " ▣ 결혼상태 : ";
                if (chkNo_Og31.Checked == true)
                {
                    strData = strData + " 기혼";
                }
                else if (chkNo_Og32.Checked == true)
                {
                    strData = strData + " 미혼";
                }
            }

            strSUB = "";
            strSUB = strSUB + ComNum.VBLF + "  - 마지막 생리 시작일 : " + txtOG311.Text.Trim() + "월 " + txtOG312.Text.Trim() + "일 ";
            strSUB = strSUB + ComNum.VBLF + "  - 평균 생리 주기 : " + txtOG32.Text.Trim() + "일 (마다)";
            strSUB = strSUB + ComNum.VBLF + "  - 평균 생리 일수 : " + txtOG33.Text.Trim() + "일 (동안)";
            if (chkNo_Og341.Checked == true)
            {
                strSUB = strSUB + ComNum.VBLF + "  - 생리양 : 많다";
            }
            else if (chkNo_Og342.Checked == true)
            {
                strSUB = strSUB + ComNum.VBLF + "  - 생리양 : 보통";
            }
            else if (chkNo_Og343.Checked == true)
            {
                strSUB = strSUB + ComNum.VBLF + "  - 생리양 : 적다";
            }

            if (chkNo_Og351.Checked == true)
            {
                strSUB = strSUB + ComNum.VBLF + "  - 생리통의 심한 정도 : 상";
            }
            else if (chkNo_Og352.Checked == true)
            {
                strSUB = strSUB + ComNum.VBLF + "  - 생리통의 심한 정도 : 중";
            }
            else if (chkNo_Og353.Checked == true)
            {
                strSUB = strSUB + ComNum.VBLF + "  - 생리통의 심한 정도 : 하";
            }

            if (chkNo_Og361.Checked == true)
            {
                strSUB = strSUB + ComNum.VBLF + "  - 피임 : 예";
                strSUB = strSUB + ComNum.VBLF + "  - 피임방법 : " + txtOG36.Text;
            }
            else if (chkNo_Og362.Checked == true)
            {
                strSUB = strSUB + ComNum.VBLF + "  - 피임 : 아니오";
            }

            strSUB = strSUB + ComNum.VBLF + "  - 자녀수 : " + txtOG37.Text;
            if (chkNo_Og381.Checked == true)
            {
                strSUB = strSUB + ComNum.VBLF + "  - 분만 방법 : 자연분만";
            }
            else if (chkNo_Og382.Checked == true)
            {
                strSUB = strSUB + ComNum.VBLF + "  - 분만 방법 : 제왕절개";
            }

            strData = strData + strSUB;


            if (chkNo_4.Checked == true)
            {
                if (txtOG411.Text.Trim() != "" || txtOG412.Text.Trim() != "")
                {
                    strData = strData + ComNum.VBLF;
                    strData = strData + " ▣ 임신주수 : " + txtOG411.Text.Trim() + "주 " + txtOG412.Text.Trim() + "일 ";
                    strData = strData + ComNum.VBLF;
                    strData = strData + " ▣ 출산예정일 : " + txtOG413.Text.Trim();
                }
            }

            if (chkNo_5.Checked == true)
            {
                if (chkNo_Og51.Checked == true)
                {
                    strData = strData + " ▣ 폐경여부 : 예";
                }
                else if (chkNo_Og52.Checked == true)
                {
                    strData = strData + " ▣ 폐경여부 : 아니요";
                }
            }

            if (chkNo_6.Checked == true)
            {
                if (txtOG6.Text.Trim() != "")
                {
                    strData = strData + " ▣ 과거수술력 : " + txtOG6.Text.Trim();
                }
            }

            if (chkNo_7.Checked == true)
            {
                strSUB = "";
                if (chkNo_Og71.Checked == true)
                {
                    strSUB = strSUB + " 고혈압";
                }
                if (chkNo_Og72.Checked == true)
                {
                    strSUB = strSUB + " 당뇨";
                }
                if (chkNo_Og73.Checked == true)
                {
                    strSUB = strSUB + " 심질환";
                }
                if (chkNo_Og74.Checked == true)
                {
                    strSUB = strSUB + " 기타" + txtOG74.Text.Trim() != "" ? "(" + txtOG74.Text.Trim() + ")" : "";
                }

                if (strSUB != "")
                {
                    strData = strData + ComNum.VBLF;
                    strData = strData + " ▣ 과거병력 : " + strSUB;
                }
            }

            if (chkNo_8.Checked == true)
            {
                if (chkNo_Og81.Checked == true)
                {
                    strData = strData + ComNum.VBLF;
                    strData = strData + " ▣ 복용중인 약물 여부 : 예";
                }
                else if (chkNo_Og82.Checked == true)
                {
                    strData = strData + ComNum.VBLF;
                    strData = strData + " ▣ 복용중인 약물 여부 : 아니오";
                }
            }


            if (chkNo_9.Checked == true)
            {
                strSUB = "";
                if (chkNo_Og91.Checked == true)
                {
                    strSUB = strSUB + " 약" + txtOG91.Text.Trim() != "" ? "(" + txtOG91.Text.Trim() + ") " : " ";
                }
                if (chkNo_Og92.Checked == true)
                {
                    strSUB = strSUB + " 계절";
                }
                if (chkNo_Og93.Checked == true)
                {
                    strSUB = strSUB + " 음식";
                }
                if (strSUB != "")
                {
                    strData = strData + ComNum.VBLF;
                    strData = strData + " ▣ 알러지 : " + strSUB;
                }
            }


            if (chkNo_10.Checked == true)
            {
                strData = strData + ComNum.VBLF;
                strData = strData + " ▣ 현재통증정도";

                if (chkNo_10_1.Checked == true)
                {
                    strData = strData + " : 없음";
                }

                if (chkNo_10_2.Checked == true)
                {
                    strSUB = "";
                    if (txt10_2_1.Text.Trim() != "")
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " 통증 초기평가 : " + txt10_2_1.Text.Trim();
                    }

                    if (txt10_3_1.Text.Trim() != "")
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " 가장 심할 때 : " + txt10_3_1.Text.Trim();
                    }

                    if (txt10_3_2.Text.Trim() != "")
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " 가장 덜할 때 : " + txt10_3_2.Text.Trim();
                    }

                    if (txt10_3_3.Text.Trim() != "")
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " 견딜수 있는 수준 : " + txt10_3_3.Text.Trim();
                    }

                    if (txt10_3_4.Text.Trim() != "")
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " 하루평균 : " + txt10_3_4.Text.Trim();
                    }

                    if (strSUB != "")
                    {
                        strData = strData + strSUB;
                    }

                    if (chkNo_10_5_1.Checked == true)
                    {
                        strSUB2 = strSUB2 + " 날카로운 느낌(sharp)";
                    }
                    if (chkNo_10_5_2.Checked == true)
                    {
                        strSUB2 = strSUB2 + " 둔한 느낌(dull)";
                    }
                    if (chkNo_10_5_3.Checked == true)
                    {
                        strSUB2 = strSUB2 + " 타는듯한 느낌(burning)";
                    }
                    if (chkNo_10_5_4.Checked == true)
                    {
                        strSUB2 = strSUB2 + " 누르는듯한 느낌(pressing)";
                    }
                    if (chkNo_10_5_5.Checked == true)
                    {
                        strSUB2 = strSUB2 + " 칼로 벤 것처럼 아픔(cutting)";
                    }
                    if (chkNo_10_5_6.Checked == true)
                    {
                        strSUB2 = strSUB2 + " 저린 느낌(numbing)";
                    }
                    if (chkNo_10_5_7.Checked == true)
                    {
                        strSUB2 = strSUB2 + " 쑤시는 느낌(tingling)";
                    }
                    if (chkNo_10_5_8.Checked == true)
                    {
                        strSUB2 = strSUB2 + " 죄는듯한 느낌(mumbing)";
                    }
                    if (chkNo_10_5_9.Checked == true)
                    {
                        strSUB2 = strSUB2 + " 아팠다 안아팠다함(come and goes)";
                    }
                    if (chkNo_10_5_10.Checked == true)
                    {
                        strSUB2 = strSUB2 + " 다른 부위로 퍼지듯 아픔(radiating)";
                    }
                    if (chkNo_10_5_11.Checked == true)
                    {
                        strSUB2 = strSUB2 + " 기타";
                        if (txt10_5_11_1.Text.Trim() != "")
                        {
                            strSUB2 = strSUB2 + " : " + txt10_5_11_1.Text.Trim();
                        }
                    }

                    if (strSUB2 != "")
                    {
                        strSUB2 = " <통증의 양상> " + ComNum.VBLF + strSUB2;
                    }
                    strData = strData + ComNum.VBLF + strSUB2;
                }
            }

            if (chkNo_11.Checked == true)
            {
                if (txt11_Fa.Text.Trim() != "")
                {
                    strData = strData + ComNum.VBLF;
                    strData = strData + " ▣ 가족력 : " + txt11_Fa.Text.Trim();
                }
            }

            if (chkNo_12.Checked == true)
            {
                if (txtRemark.Text.Trim() != "")
                {
                    strData = strData + ComNum.VBLF;
                    strData = strData + " ▣ 참고사항";
                    strData = strData + ComNum.VBLF + txtRemark.Text.Trim();
                }
            }

            txtSendData.Text = strData;
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
            string strH = ""; //키
            string strW = ""; //몸무게

            string strMun1 = "";
            string strMun2 = "";
            string strMun3 = "";
            string strMun311 = "";
            string strMun312 = "";
            string strMun32 = "";
            string strMun33 = "";
            string strMun34 = "";
            string strMun35 = "";  //생리통
            string strMun36 = "";  //피임
            string strMun361 = "";  //피임방법
            string strMun37 = "";  //자녀수
            string strMun38 = "";  //분만방법

            string strMun41 = "";  //임신주
            string strMun42 = "";  //임신일
            string strMun43 = "";  //임신예정일

            string strMun5 = "";  //폐경

            string strMun6 = "";  //수술

            string strMun71 = "";  //질환 고혈압
            string strMun72 = "";  //질환 당뇨
            string strMun73 = "";  //질환 심질환
            string strMun74 = "";  //질환 기타
            string strMun741 = "";  //질환 기타내역

            string strMun8 = "";  //복용약

            string strMun91 = "";  //알러지 약
            string strMun911 = "";  //       약내역
            string strMun92 = "";  //       계절
            string strMun93 = "";  //       음식

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

            string strRemark = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            strH = txtHeight.Text.Trim();
            strW = txtWeight.Text.Trim();

            //1
            strMun1 = txt1.Text.Trim();

            //2
            strMun2 = txtOG2.Text.Trim();

            //3
            strMun3 = "2"; //미혼
            if (chkNo_Og31.Checked == true)
            {
                strMun3 = "1"; //기혼
            }

            strMun311 = txtOG311.Text.Trim();
            strMun312 = txtOG312.Text.Trim();
            strMun32 = txtOG32.Text.Trim();
            strMun33 = txtOG33.Text.Trim();
            strMun34 = "";
            if (chkNo_Og341.Checked == true)
            {
                strMun34 = "1";
            }//많다
            if (chkNo_Og342.Checked == true)
            {
                strMun34 = "2";
            } //보통
            if (chkNo_Og343.Checked == true)
            {
                strMun34 = "3";
            }//적다

            strMun35 = "";
            if (chkNo_Og351.Checked == true)
            {
                strMun35 = "1";
            } //상
            if (chkNo_Og352.Checked == true)
            {
                strMun35 = "2";
            }//중
            if (chkNo_Og353.Checked == true)
            {
                strMun35 = "3";
            }//하

            strMun36 = "N";
            if (chkNo_Og361.Checked == true)
            {
                strMun36 = "Y";
            }
            strMun361 = txtOG36.Text.Trim(); //피임방법

            strMun37 = txtOG37.Text.Trim(); //자녀수
            strMun38 = "0"; //선택안함
            if (chkNo_Og381.Checked == true)
            {
                strMun38 = "1";
            } //자연분만
            if (chkNo_Og382.Checked == true)
            {
                strMun38 = "2";
            }//제왕절개 


            //4
            strMun41 = txtOG411.Text.Trim();
            strMun42 = txtOG412.Text.Trim();
            strMun43 = txtOG413.Text.Trim();


            //5
            strMun5 = "N"; //폐경
            if (chkNo_Og51.Checked == true)
            {
                strMun5 = "Y";
            }

            //6
            strMun6 = txtOG6.Text.Trim();

            //7
            strMun71 = "N";
            if (chkNo_Og71.Checked == true)
            {
                strMun71 = "Y";
            } //고혈압
            strMun72 = "N";
            if (chkNo_Og72.Checked == true)
            {
                strMun72 = "Y";
            } //당뇨
            strMun73 = "N";
            if (chkNo_Og73.Checked == true)
            {
                strMun73 = "Y";
            } //심질환
            strMun74 = "N";
            if (chkNo_Og74.Checked == true)
            {
                strMun74 = "Y";
            } //기타
            strMun741 = txtOG74.Text.Trim();

            //8
            strMun8 = "N";
            if (chkNo_Og81.Checked == true)
            {
                strMun8 = "Y";
            }


            //9
            strMun91 = "N";
            if (chkNo_Og91.Checked == true)
            {
                strMun91 = "Y";
            }
            strMun911 = txtOG91.Text.Trim();
            strMun92 = "N";
            if (chkNo_Og92.Checked == true)
            {
                strMun92 = "Y";
            }
            strMun93 = "N";
            if (chkNo_Og93.Checked == true)
            {
                strMun93 = "Y";
            }


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

                    SQL = SQL + ComNum.VBLF + " TMUN1,";

                    //--add
                    SQL = SQL + ComNum.VBLF + " OG_MUN2,OG_MUN3,OG_MUN3_1_1,";
                    SQL = SQL + ComNum.VBLF + " OG_MUN3_1_2,OG_MUN3_2,OG_MUN3_3,OG_MUN3_4,OG_MUN3_5,OG_MUN3_6,OG_MUN3_6_1,";
                    SQL = SQL + ComNum.VBLF + " OG_MUN3_7,OG_MUN3_8,OG_MUN4_1,OG_MUN4_2,OG_MUN4_3,OG_MUN5,OG_MUN6,OG_MUN7_1,";
                    SQL = SQL + ComNum.VBLF + " OG_MUN7_2,OG_MUN7_3,OG_MUN7_4,OG_MUN7_4_1,OG_MUN8,OG_MUN9_1_1,OG_MUN9_1_2,";
                    SQL = SQL + ComNum.VBLF + " OG_MUN9_2 , OG_MUN9_3, ";
                    //--add

                    SQL = SQL + ComNum.VBLF + " TMUN10_1,TMUN10_2,TMUN10_2_1,TMUN10_3_1,TMUN10_3_2,TMUN10_3_3,TMUN10_3_4, ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_4_1,TMUN10_4_2,TMUN10_4_3,TMUN10_4_4,TMUN10_4_5,TMUN10_5_1,TMUN10_5_2, ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_3,TMUN10_5_4,TMUN10_5_5,TMUN10_5_6,TMUN10_5_7,TMUN10_5_8,TMUN10_5_9, ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_10,TMUN10_5_11,TMUN10_5_11_1,tmun11_f1,tmun12_t1,";

                    SQL = SQL + ComNum.VBLF + " Remark, ENTSABUN, ENTDATE ) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + FstrBDate + "','YYYY-MM-DD'),'" + FstrPano + "','" + FstrDept + "','" + FstrDrCode + "','" + strH + "','" + strW + "', ";

                    SQL = SQL + ComNum.VBLF + " '" + strMun1 + "', ";

                    //--add

                    SQL = SQL + ComNum.VBLF + " '" + strMun2 + "','" + strMun3 + "','" + strMun311 + "','" + strMun312 + "','" + strMun32 + "','" + strMun33 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strMun34 + "','" + strMun35 + "','" + strMun36 + "','" + strMun361 + "','" + strMun37 + "','" + strMun38 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strMun41 + "','" + strMun42 + "','" + strMun43 + "','" + strMun5 + "','" + strMun6 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strMun71 + "','" + strMun72 + "','" + strMun73 + "','" + strMun74 + "','" + strMun741 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strMun8 + "','" + strMun91 + "','" + strMun911 + "','" + strMun92 + "','" + (chkNo_Og93.Checked == true ? "Y" : "N") + "',";

                    //--add

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

                    SQL = SQL + ComNum.VBLF + " '" + strRemark + "'," + clsType.User.Sabun + ",SYSDATE ) ";
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "OPD_DEPT_MUNJIN SET ";

                    SQL = SQL + ComNum.VBLF + " TMUN_H ='" + strH + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_W ='" + strW + "', ";

                    SQL = SQL + ComNum.VBLF + " TMUN1 ='" + strMun1 + "', ";


                    //--add

                    SQL = SQL + ComNum.VBLF + " OG_MUN2 ='" + strMun2 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN3 ='" + strMun3 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN3_1_1 ='" + strMun311 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN3_1_2 ='" + strMun312 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN3_2 ='" + strMun32 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN3_3 ='" + strMun33 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN3_4 ='" + strMun34 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN3_5 ='" + strMun35 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN3_6 ='" + strMun36 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN3_6_1 ='" + strMun361 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN3_7 ='" + strMun37 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN3_8 ='" + strMun38 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN4_1 ='" + strMun41 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN4_2 ='" + strMun42 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN4_3 ='" + strMun43 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN5 ='" + strMun5 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN6 ='" + strMun6 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN7_1 ='" + strMun71 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN7_2 ='" + strMun72 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN7_3 ='" + strMun73 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN7_4 ='" + strMun74 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN7_4_1 ='" + strMun741 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN8 ='" + strMun8 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN9_1_1 ='" + strMun91 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN9_1_2 ='" + strMun911 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN9_2 ='" + strMun92 + "', ";
                    SQL = SQL + ComNum.VBLF + " OG_MUN9_3 ='" + strMun93 + "', ";
                    //--add

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
        }

        private void frmMedicalInquiry_OG_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            // 의사인 경우 버튼활성화
            if (clsType.User.DrCode == "")
            {
                btnChart.Enabled = false;
            }
            else
            {
                btnChart.Enabled = true;
            }

            Screen_Clear();
            Set_Info();
            Search_Data();
        }

        void Read_Munjin_Dept(string ArgPano, string ArgDeptCode, string ArgBDate)
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
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "";

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
                    SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + ArgPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + dt.Rows[i]["DeptCode"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
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

        void Read_Munjin_Data(string ArgSTS, string ArgPano, string ArgDeptCode, string ArgBDate)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT BDATE,PANO,DEPTCODE,DRCODE,REMARK,ENTSABUN,ENTDATE,DELDATE,CHK,TMUN_H, TMUN_W, ";

                SQL = SQL + ComNum.VBLF + " TMUN1,TMUN2Y,TMUN2N,TMUN2_1,TMUN2_1_1,TMUN2_2,TMUN2_2_1,TMUN2_3,TMUN2_3_1,TMUN2_4,TMUN2_4_1,TMUN2_5,TMUN2_5_1,TMUN2_6,TMUN2_6_1,";
                SQL = SQL + ComNum.VBLF + " TMUN2_7,TMUN2_7_1,TMUN2_8,TMUN2_8_1,TMUN2_9,TMUN2_9_1, ";
                SQL = SQL + ComNum.VBLF + " TMUN2_10,TMUN2_10_1,TMUN3_1,TMUN3_2,TMUN3_2_1,TMUN3_2_1_1,TMUN3_2_2,TMUN3_2_2_1,TMUN3_2_3,TMUN3_2_3_1, ";
                SQL = SQL + ComNum.VBLF + " TMUN3_2_4,TMUN3_2_4_1,TMUN3_2_5,TMUN3_2_5_1,TMUN3_2_6,TMUN3_2_6_1,TMUN3_2_7,TMUN3_2_7_1,TMUN4_1,TMUN4_2, ";
                SQL = SQL + ComNum.VBLF + " TMUN4_2_1,TMUN4_2_2,TMUN4_2_3,TMUN4_2_4,TMUN4_2_5,TMUN4_2_5_1,TMUN5_1,TMUN5_1_1,TMUN5_2,TMUN5_2_1, ";
                SQL = SQL + ComNum.VBLF + " TMUN6_1,TMUN6_1_1,TMUN6_1_2,TMUN6_1_3,TMUN6_2,TMUN6_2_1,TMUN6_2_2,TMUN6_2_3, ";
                SQL = SQL + ComNum.VBLF + " TMUN7_1,TMUN7_2,TMUN7_2_1,TMUN7_2_2,TMUN8_1,TMUN8_2,TMUN8_3,TMUN8_4,TMUN8_5, ";
                SQL = SQL + ComNum.VBLF + " TMUN8_5_1,TMUN9_1,TMUN9_2,TMUN9_2_1,TMUN9_2_1_1,TMUN9_2_1_2,TMUN9_2_2,TMUN9_2_2_1,TMUN9_2_2_2,TMUN9_2_3, ";
                SQL = SQL + ComNum.VBLF + " TMUN9_2_3_1,TMUN9_2_3_2,TMUN9_2_4,TMUN9_2_4_1,TMUN9_2_4_2,TMUN9_2_5,TMUN9_2_5_1,TMUN9_2_5_2,TMUN9_2_6,TMUN9_2_6_1, ";
                SQL = SQL + ComNum.VBLF + " TMUN9_2_6_2,TMUN9_2_7,TMUN9_2_7_1,TMUN9_2_7_2,TMUN10_1,TMUN10_2,TMUN10_2_1,TMUN10_3_1,TMUN10_3_2,TMUN10_3_3,TMUN10_3_4, ";
                SQL = SQL + ComNum.VBLF + " TMUN10_4_1,TMUN10_4_2,TMUN10_4_3,TMUN10_4_4,TMUN10_4_5,TMUN10_5_1,TMUN10_5_2, ";
                SQL = SQL + ComNum.VBLF + " TMUN10_5_3,TMUN10_5_4,TMUN10_5_5,TMUN10_5_6,TMUN10_5_7,TMUN10_5_8,TMUN10_5_9, ";
                SQL = SQL + ComNum.VBLF + " TMUN10_5_10,TMUN10_5_11,TMUN10_5_11_1,tmun11_f1,tmun12_t1,";

                SQL = SQL + ComNum.VBLF + " OG_MUN2,OG_MUN3,OG_MUN3_1_1,";
                SQL = SQL + ComNum.VBLF + " OG_MUN3_1_2,OG_MUN3_2,OG_MUN3_3,OG_MUN3_4,OG_MUN3_5,OG_MUN3_6,OG_MUN3_6_1,";
                SQL = SQL + ComNum.VBLF + " OG_MUN3_7,OG_MUN3_8,OG_MUN4_1,OG_MUN4_2,OG_MUN4_3,OG_MUN5,OG_MUN6,OG_MUN7_1,";
                SQL = SQL + ComNum.VBLF + " OG_MUN7_2,OG_MUN7_3,OG_MUN7_4,OG_MUN7_4_1,OG_MUN8,OG_MUN9_1_1,OG_MUN9_1_2,";
                SQL = SQL + ComNum.VBLF + " OG_MUN9_2 , OG_MUN9_3, ";

                SQL = SQL + ComNum.VBLF + " Remark, ENTSABUN, ENTDATE,ROWID, EMRNO ";

                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_DEPT_MUNJIN ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + ArgPano + "' ";

                if (ArgSTS == "1" || ArgSTS == "2")
                {
                    SQL = SQL + ComNum.VBLF + "  AND DEPTCODE ='" + ArgDeptCode + "' ";
                }
                else
                {
                    if (string.Compare(ArgBDate, "2015-09-08") >= 0)
                    {
                        //통합 최초과 저장시 만 과저장됨 , 정신과제외
                        SQL = SQL + ComNum.VBLF + "  AND DEPTCODE <> 'NP' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND DEPTCODE ='" + ArgDeptCode + "' ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "  AND BDATE =TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";

                SQL = SQL + ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                FstrROWID = "";

                if (dt.Rows.Count > 0)
                {
                    if (ArgSTS != "2")
                    {
                        FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    }

                    txtHeight.Text = dt.Rows[0]["TMUN_H"].ToString().Trim();
                    txtWeight.Text = dt.Rows[0]["TMUN_W"].ToString().Trim();

                    //1
                    txt1.Text = dt.Rows[0]["TMUN1"].ToString().Trim();


                    //2
                    txtOG2.Text = dt.Rows[0]["OG_MUN2"].ToString().Trim();

                    //3
                    chkNo_Og32.Checked = true;
                    if (dt.Rows[0]["OG_MUN3"].ToString().Trim() == "1")
                    {
                        chkNo_Og32.Checked = false;
                        chkNo_Og31.Checked = true;
                    }

                    txtOG311.Text = dt.Rows[0]["OG_MUN3_1_1"].ToString().Trim();
                    txtOG312.Text = dt.Rows[0]["OG_MUN3_1_2"].ToString().Trim();

                    txtOG32.Text = dt.Rows[0]["OG_MUN3_2"].ToString().Trim();
                    txtOG33.Text = dt.Rows[0]["OG_MUN3_3"].ToString().Trim();


                    switch (dt.Rows[0]["OG_MUN3_4"].ToString().Trim())
                    {
                        case "1":
                            chkNo_Og341.Checked = true;
                            break;
                        case "2":
                            chkNo_Og342.Checked = true;
                            break;
                        case "3":
                            chkNo_Og343.Checked = true;
                            break;
                    }

                    switch (dt.Rows[0]["OG_MUN3_5"].ToString().Trim())
                    {
                        case "1":
                            chkNo_Og351.Checked = true;
                            break;
                        case "2":
                            chkNo_Og352.Checked = true;
                            break;
                        case "3":
                            chkNo_Og353.Checked = true;
                            break;
                    }

                    chkNo_Og362.Checked = true;
                    if (dt.Rows[0]["OG_MUN3_6"].ToString().Trim() == "Y")
                    {
                        chkNo_Og362.Checked = false;
                        chkNo_Og361.Checked = true;
                    }
                    if (dt.Rows[0]["OG_MUN3_6_1"].ToString().Trim() != "")
                    {
                        txtOG36.Text = dt.Rows[0]["OG_MUN3_6_1"].ToString().Trim();
                    }

                    txtOG37.Text = dt.Rows[0]["OG_MUN3_7"].ToString().Trim();

                    switch (dt.Rows[0]["OG_MUN3_8"].ToString().Trim())
                    {
                        case "1":
                            chkNo_Og381.Checked = true;
                            break;
                        case "2":
                            chkNo_Og382.Checked = true;
                            break;
                    }

                    //4
                    txtOG411.Text = dt.Rows[0]["OG_MUN4_1"].ToString().Trim();
                    txtOG412.Text = dt.Rows[0]["OG_MUN4_2"].ToString().Trim();
                    txtOG413.Text = dt.Rows[0]["OG_MUN4_3"].ToString().Trim();

                    //5
                    chkNo_Og52.Checked = true;
                    if (dt.Rows[0]["OG_MUN5"].ToString().Trim() == "Y")
                    {
                        chkNo_Og51.Checked = true;
                        chkNo_Og52.Checked = false;
                    }

                    //6
                    txtOG6.Text = dt.Rows[0]["OG_MUN6"].ToString().Trim();

                    //7
                    chkNo_Og71.Checked = false;
                    if (dt.Rows[0]["OG_MUN7_1"].ToString().Trim() == "Y")
                    {
                        chkNo_Og71.Checked = true;
                    }
                    chkNo_Og72.Checked = false;
                    if (dt.Rows[0]["OG_MUN7_2"].ToString().Trim() == "Y")
                    {
                        chkNo_Og72.Checked = true;
                    }
                    chkNo_Og73.Checked = false;
                    if (dt.Rows[0]["OG_MUN7_3"].ToString().Trim() == "Y")
                    {
                        chkNo_Og73.Checked = true;
                    }
                    chkNo_Og74.Checked = false;
                    if (dt.Rows[0]["OG_MUN7_4"].ToString().Trim() == "Y")
                    {
                        chkNo_Og74.Checked = true;
                    }
                    if (dt.Rows[0]["OG_MUN7_4_1"].ToString().Trim() != "")
                    {
                        txtOG74.Text = dt.Rows[0]["OG_MUN7_4_1"].ToString().Trim();
                    }

                    //8
                    chkNo_Og82.Checked = true;
                    if (dt.Rows[0]["OG_MUN8"].ToString().Trim() == "Y")
                    {
                        chkNo_Og81.Checked = true;
                        chkNo_Og82.Checked = false;
                    }

                    //9
                    chkNo_Og91.Checked = false;
                    if (dt.Rows[0]["OG_MUN9_1_1"].ToString().Trim() == "Y")
                    {
                        chkNo_Og91.Checked = true;
                    }
                    if (dt.Rows[0]["OG_MUN9_1_2"].ToString().Trim() != "")
                    {
                        txtOG91.Text = dt.Rows[0]["OG_MUN9_1_2"].ToString().Trim();
                    }

                    chkNo_Og92.Checked = false;
                    if (dt.Rows[0]["OG_MUN9_2"].ToString().Trim() == "Y")
                    {
                        chkNo_Og92.Checked = true;
                    }
                    chkNo_Og93.Checked = false;
                    if (dt.Rows[0]["OG_MUN9_3"].ToString().Trim() == "Y")
                    {
                        chkNo_Og93.Checked = true;
                    }


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

                    txtRemark.Text = dt.Rows[0]["Remark"].ToString().Trim();
                    txtEMRNO.Text = dt.Rows[0]["EMRNO"].ToString().Trim();

                    ComFunc.MsgBox(dt.Rows[0]["DeptCode"].ToString().Trim() + "과 에서 " + ArgBDate + "에 등록된 자료입니다..");
                }
                else
                {
                    ComFunc.MsgBox("등록된 예진표자료가 없습니다.." + ComNum.VBLF + ComNum.VBLF + "타과 내역을 불러오실려면 과코드를 더블클릭하세요");
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

        void Screen_Clear()
        {
            txtHeight.Text = "";
            txtWeight.Text = "";

            //1
            txt1.Text = "";

            //2
            txtOG2.Text = "";

            //3
            chkNo_Og31.Checked = false;
            chkNo_Og32.Checked = false;

            txtOG311.Text = "";
            txtOG312.Text = "";
            txtOG32.Text = "";
            txtOG33.Text = "";
            chkNo_Og341.Checked = false;
            chkNo_Og342.Checked = false;
            chkNo_Og343.Checked = false;
            chkNo_Og351.Checked = false;
            chkNo_Og352.Checked = false;
            chkNo_Og353.Checked = false;
            chkNo_Og361.Checked = false;
            chkNo_Og362.Checked = false;
            txtOG36.Text = "";
            txtOG37.Text = "";
            chkNo_Og381.Checked = false;
            chkNo_Og382.Checked = false;

            //4
            txtOG411.Text = "";
            txtOG412.Text = "";
            txtOG413.Text = "";

            //5
            chkNo_Og51.Checked = false;
            chkNo_Og52.Checked = false;

            //6
            txtOG6.Text = "";

            //7
            chkNo_Og71.Checked = false;
            chkNo_Og72.Checked = false;
            chkNo_Og73.Checked = false;
            chkNo_Og74.Checked = false;
            txtOG74.Text = "";

            //8
            chkNo_Og81.Checked = false;
            chkNo_Og82.Checked = false;

            //9
            chkNo_Og91.Checked = false;
            chkNo_Og92.Checked = false;
            chkNo_Og93.Checked = false;
            txtOG91.Text = "";

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

            txtRemark.Text = "";
            txtEMRNO.Text = "";
        }

        private void ssDept_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strDept = "";

            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            strDept = ssDept_Sheet1.Cells[e.Row, e.Column].Text.Trim();

            if (strDept == "NP" || strDept == "OG" || strDept == "OS" || strDept == "MN")
            {
                ComFunc.MsgBox("전용예진표에서는 다른과 내역을 확인할 수 없습니다.");
            }
            else
            {
                Read_Munjin_Data("2", FstrPano, strDept, FstrBDate);
            }
        }

        private void txt10_2_1_TextChanged(object sender, EventArgs e)
        {
            if (VB.Val(txt10_2_1.Text) >= 4)
            {
                txt10_2_1.BackColor = ColorTranslator.FromWin32(int.Parse("&H8080FF", System.Globalization.NumberStyles.AllowHexSpecifier));
            }
            else
            {
                txt10_2_1.BackColor = ColorTranslator.FromWin32(int.Parse("&H80000005", System.Globalization.NumberStyles.AllowHexSpecifier));
            }
        }

        void Gesan_Tot()
        {
            txt10_2_1.Text = (VB.Val(txt10_3_4.Text) + VB.Val(txt10_4_1.Text) + VB.Val(txt10_4_2.Text) + VB.Val(txt10_4_3.Text) + VB.Val(txt10_4_4.Text) + VB.Val(txt10_4_5.Text)).ToString();

            if (VB.Val(txt10_2_1.Text) >= 4)
            {
                txt10_2_1.BackColor = ColorTranslator.FromWin32(int.Parse("&H8080FF", System.Globalization.NumberStyles.AllowHexSpecifier));
            }
            else
            {
                txt10_2_1.BackColor = ColorTranslator.FromWin32(int.Parse("&H80000005", System.Globalization.NumberStyles.AllowHexSpecifier));
            }
        }

        private void ssView_txt_TextChanged(object sender, EventArgs e)
        {
            Gesan_Tot();
        }

        private void txtOG413_DoubleClick(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text != "")
            {
                clsPublic.GstrCalDate = ((TextBox)sender).Text;
            }

            frmCalendar frmCalendarX = new frmCalendar();
            frmCalendarX.StartPosition = FormStartPosition.CenterParent;
            frmCalendarX.ShowDialog();

            ((TextBox)sender).Text = clsPublic.GstrCalDate;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            if (btnAll.Text.Trim() == "모든항목 해제")
            {
                chkNo_0.Checked = false;
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
                chkNo_13.Checked = false;
                btnAll.Text = "모든항목 선택";
            }
            else
            {
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
                chkNo_13.Checked = false;
                btnAll.Text = "모든항목 해제";
            }
        }
    }
}
