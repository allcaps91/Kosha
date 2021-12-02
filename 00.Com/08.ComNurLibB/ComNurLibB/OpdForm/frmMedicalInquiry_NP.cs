using ComBase; //기본 클래스
using ComEmrBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmMedicalInquiry_NP.cs
    /// Description     : 정신건강의학과 문진
    /// Author          : 박창욱
    /// Create Date     : 2018-01-17
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\emr\emrprt\Frm진료예진표3.frm(Frm진료예진표3.frm) >> frmMedicalInquiry_NP.cs 폼이름 재정의" />	
    public partial class frmMedicalInquiry_NP : Form
    {
        string FstrPano = "";
        string FstrSName = "";
        string FstrDept = "";
        string FstrDrCode = "";
        string FstrBDate = "";
        string FstrROWID = "";

        public frmMedicalInquiry_NP()
        {
            InitializeComponent();
        }

        public frmMedicalInquiry_NP(string strPaNo, string strPaName, string strDept, string strDrCode, string strBDate)
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

        void Build_Data()
        {
            string strData = "";
            string strSUB = "";
            string strSUB2 = "";

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
                strData += ComNum.VBLF;
                strData += " ▣ 주호소";
                strData += ComNum.VBLF + txt1.Text.Trim();
                strData += ComNum.VBLF;
                strData += "   - 지금 증상으로 다른병원에서 진료받으신적 있습니까?";
                strData += ComNum.VBLF;
                if (chkNo_8_1.Checked == true)
                {
                    strData += "      => 아니요";
                }
                else if (chkNo_8_2.Checked == true)
                {
                    strData += "      => 네(" + txt8_1.Text.Trim() + ")";
                }
            }


            if (chkNo_2.Checked == true)
            {
                if (txt2_1.Text.Trim() != "")
                {
                    strData += ComNum.VBLF;
                    strData += " ▣ 직업 : " + txt2_1.Text.Trim();
                }
                if (txt2_2.Text.Trim() != "")
                {
                    strData += ComNum.VBLF;
                    strData += " ▣ 학력 : " + txt2_2.Text.Trim();
                }
            }

            if (txtHeight.Text.Trim() != "" || txtWeight.Text.Trim() != "")
            {
                strData += ComNum.VBLF;
                strData += " ▣ 키/몸무게 : " + txtHeight.Text.Trim() + "/" + txtWeight.Text.Trim();
            }

            if (txtB1.Text.Trim() != "" || txtB2.Text.Trim() != "")
            {
                strData += ComNum.VBLF;
                strData += " ▣ 혈압 : " + txtB1.Text.Trim() + "/" + txtB2.Text.Trim();
            }

            if (chkNo_3.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 가족력";

                if (chkNo_3_1.Checked == true)
                {
                    strData += ComNum.VBLF;
                    strData += " 무";
                }

                if (chkNo_3_2.Checked == true)
                {
                    strData += ComNum.VBLF;
                    strData += " 유(" + txt3.Text.Trim() + ")";
                }
            }

            strSUB = "";
            if (txt3_1_1.Text.Trim() != "")
            {
                strSUB += "나이 : " + txt3_1_1.Text.Trim() + " ";
            }
            if (txt3_1_2.Text.Trim() != "")
            {
                strSUB += "직업 : " + txt3_1_1.Text.Trim() + " ";
            }
            if (txt3_1_3.Text.Trim() != "")
            {
                strSUB += "학력 : " + txt3_1_3.Text.Trim() + " ";
            }
            if (txt3_1_4.Text.Trim() != "")
            {
                strSUB += "기타 : " + txt3_1_4.Text.Trim() + " ";
            }
            if (strSUB != "")
            {
                strData += ComNum.VBLF;
                strData += " - 아버지 " + strSUB;
            }

            strSUB = "";
            if (txt3_2_1.Text.Trim() != "")
            {
                strSUB += "나이 : " + txt3_2_1.Text.Trim() + " ";
            }
            if (txt3_2_2.Text.Trim() != "")
            {
                strSUB += "직업 : " + txt3_2_1.Text.Trim() + " ";
            }
            if (txt3_2_3.Text.Trim() != "")
            {
                strSUB += "학력 : " + txt3_2_3.Text.Trim() + " ";
            }
            if (txt3_2_4.Text.Trim() != "")
            {
                strSUB += "기타 : " + txt3_2_4.Text.Trim() + " ";
            }
            if (strSUB != "")
            {
                strData += ComNum.VBLF;
                strData += " - 어머니 " + strSUB;
            }

            strSUB = "";
            if (txt3_3_1.Text.Trim() != "")
            {
                strSUB += txt3_3_1.Text.Trim() + "남 ";
            }
            if (txt3_3_2.Text.Trim() != "")
            {
                strSUB += txt3_3_2.Text.Trim() + "녀 ";
            }
            if (txt3_3_3.Text.Trim() != "")
            {
                strSUB += txt3_3_3.Text.Trim() + "째 ";
            }
            if (strSUB != "")
            {
                strData += ComNum.VBLF;
                strData += " - 형제관계 : " + strSUB;
            }

            if (chkNo_4.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 결혼여부";
                if (chkNo_4_1.Checked == true)
                {
                    strData += ComNum.VBLF;
                    strData += " 무";
                }
                if (chkNo_4_2.Checked == true)
                {
                    strData += ComNum.VBLF;
                    strData += " 유";
                }

                strSUB = "";
                if (chkNo_4_3.Checked == true)
                {
                    strSUB += "(사별) ";
                }
                if (chkNo_4_4.Checked == true)
                {
                    strSUB += "(이혼) ";
                }
                if (chkNo_4_5.Checked == true)
                {
                    strSUB += "(별거) ";
                }
                if (chkNo_4_6.Checked == true)
                {
                    strSUB += "(재혼) ";
                }

                if (strSUB != "")
                {
                    strData += " " + strSUB;
                }

                if (chkNo_4_2.Checked == true)
                {
                    strSUB = "";
                    if (txt4_1_1.Text.Trim() != "")
                    {
                        strSUB += " 나이:" + txt4_1_1.Text.Trim();
                    }
                    if (txt4_1_2.Text.Trim() != "")
                    {
                        strSUB += " 직업:" + txt4_1_2.Text.Trim();
                    }
                    if (txt4_1_3.Text.Trim() != "")
                    {
                        strSUB += " 학력:" + txt4_1_3.Text.Trim();
                    }
                    if (strSUB != "")
                    {
                        strData += ComNum.VBLF;
                        strData += "  - 배우자 " + strSUB;
                    }
                }

                strSUB = "";
                if (txt4_2_1.Text.Trim() != "")
                {
                    strSUB += txt4_2_1.Text.Trim() + "남 ";
                }
                if (txt4_2_2.Text.Trim() != "")
                {
                    strSUB += txt4_2_2.Text.Trim() + "녀 ";
                }
                if (strSUB != "")
                {
                    strData += "  - 자녀 : " + strSUB;
                }
            }

            if (chkNo_5.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 과거병력";

                if (chkNo_5_1.Checked == true)
                {
                    strData += ComNum.VBLF;
                    strData += " 무";
                }

                if (chkNo_5_2.Checked == true)
                {
                    strData += ComNum.VBLF;
                    strData += " 유";
                }

                strSUB = "";
                if (chkNo_5_3.Checked == true)
                {
                    strSUB += " 간질환";
                }
                if (chkNo_5_4.Checked == true)
                {
                    strSUB += " 고혈압";
                }
                if (chkNo_5_5.Checked == true)
                {
                    strSUB += " 당뇨";
                }
                if (chkNo_5_6.Checked == true)
                {
                    strSUB += " 뇌질환";
                }
                if (chkNo_5_7.Checked == true)
                {
                    strSUB += " 심질환";
                }
                if (chkNo_5_8.Checked == true)
                {
                    strSUB += " 신질환";
                }
                if (chkNo_5_9.Checked == true)
                {
                    strSUB += " 호흡기";
                }
                if (chkNo_5_10.Checked == true)
                {
                    strSUB += " 소화기";
                }
                if (chkNo_5_11.Checked == true)
                {
                    strSUB += " 정신질환";
                }
                if (chkNo_5_12.Checked == true)
                {
                    strSUB += " 기타" + txt5_1.Text.Trim() != "" ? "(" + txt5_1.Text.Trim() + ")" : "";
                }

                if (strSUB != "")
                {
                    strData += ComNum.VBLF;
                    strData += " " + strSUB;
                }
            }

            if (chkNo_6.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 흡연/음주 여부";

                if (chkNo_6_1.Checked == true)
                {
                    strData += ComNum.VBLF;
                    strData += " 무";
                }

                if (chkNo_6_1_1.Checked == true)
                {
                    strData += ComNum.VBLF;
                    strData += " 유";

                    strSUB = "";
                    if (txt6_1_1_1.Text.Trim() != "")
                    {
                        strSUB += " " + txt6_1_1_1.Text.Trim() + "병/회";
                    }
                    if (txt6_1_1_2.Text.Trim() != "")
                    {
                        strSUB += " " + txt6_1_1_2.Text.Trim() + "주/회";
                    }
                    if (strSUB != "")
                    {
                        strData += ComNum.VBLF;
                        strData += " - 음주 : " + strSUB;
                    }

                    strSUB = "";
                    if (txt6_2_1_1.Text.Trim() != "")
                    {
                        strSUB += " " + txt6_2_1_1.Text.Trim() + "갑/일";
                    }
                    if (txt6_2_1_2.Text.Trim() != "")
                    {
                        strSUB += " " + txt6_2_1_2.Text.Trim() + "년";
                    }
                    if (strSUB != "")
                    {
                        strData += ComNum.VBLF;
                        strData += " - 흡연 : " + strSUB;
                    }
                }
            }

            if (chkNo_7.Checked == true)
            {
                strData += ComNum.VBLF;
                strData += " ▣ 현재 타과에서 복용중인 약물 여부";

                if (chkNo_7_1.Checked == true)
                {
                    strData += ComNum.VBLF;
                    strData += " 무";
                }

                if (chkNo_7_2.Checked == true)
                {
                    strData += ComNum.VBLF;
                    strData += " 유";

                    strSUB = "";
                    if (chkNo_7_3.Checked == true)
                    {
                        strSUB += " 고혈압";
                    }
                    if (chkNo_7_4.Checked == true)
                    {
                        strSUB += " 당뇨약";
                    }
                    if (chkNo_7_5.Checked == true)
                    {
                        strSUB += " 혈전용해제";
                    }
                    if (chkNo_7_6.Checked == true)
                    {
                        strSUB += " 항응고제";
                    }
                    if (chkNo_7_7.Checked == true)
                    {
                        strSUB += " 고지혈증";
                    }
                    if (chkNo_7_8.Checked == true)
                    {
                        strSUB += " 골다공증약";
                    }
                    if (chkNo_7_9.Checked == true)
                    {
                        strSUB += " 기타";
                    }
                    if (txt7_1.Text.Trim() != "")
                    {
                        strSUB += "(" + txt7_1.Text.Trim() + ")";
                    }
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
                    if (txt10_2_1.Text.Trim() != "")
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 통증 초기평가 : " + txt10_2_1.Text.Trim();
                    }

                    if (txt10_3_1.Text.Trim() != "")
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 가장 심할 때 : " + txt10_3_1.Text.Trim();
                    }

                    if (txt10_3_2.Text.Trim() != "")
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 가장 덜할 때 : " + txt10_3_2.Text.Trim();
                    }

                    if (txt10_3_3.Text.Trim() != "")
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 견딜수 있는 수준 : " + txt10_3_3.Text.Trim();
                    }

                    if (txt10_3_4.Text.Trim() != "")
                    {
                        strSUB += ComNum.VBLF;
                        strSUB += " 하루평균 : " + txt10_3_4.Text.Trim();
                    }

                    if (strSUB != "")
                    {
                        strData += strSUB;
                    }

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

            txtSendData.Text = strData;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (btnSaveClick() == false)
            {
                this.Close();
            }
        }

        private bool btnSaveClick()
        {
            bool rtnVal = false;
            //int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strH = ""; //키
            string strW = ""; //몸무게

            string strB1 = "";
            string strB2 = "";

            string strMun1 = "";

            //string strMun2Y = "";
            //string strMun2N = "";

            string strMunJob = "";
            string strMunGrade = "";


            string strMun3_1 = "";
            string strMun3_2 = "";
            string strMun3_2_1 = "";
            string strMun3_F1 = "";
            string strMun3_F2 = "";
            string strMun3_F3 = "";
            string strMun3_F4 = "";
            string strMun3_M1 = "";
            string strMun3_M2 = "";
            string strMun3_M3 = "";
            string strMun3_M4 = "";
            string strMun3_B1 = "";
            string strMun3_B2 = "";
            string strMun3_B3 = "";

            string strMun_M_N = "";
            string strMun_M_Y = "";
            string strMun_M_Y1 = "";
            string strMun_M_Y2 = "";
            string strMun_M_Y3 = "";
            string strMun_M_Y4 = "";
            string strMun_M_M1 = "";
            string strMun_M_M2 = "";
            string strMun_M_M3 = "";
            string strMun_M_S1 = "";
            string strMun_M_S2 = "";

            //string strMunOldN = "";
            //string strMunOldY = "";
            string strMunOldY1 = "";
            string strMunOldY2 = "";
            string strMunOldY3 = "";
            string strMunOldY4 = "";
            string strMunOldY5 = "";
            string strMunOldY6 = "";
            string strMunOldY7 = "";
            string strMunOldY8 = "";
            string strMunOldY9 = "";
            string strMunOldY10 = "";
            string strMunOldY10_1 = "";


            string strMun6_1 = "";
            string strMun6_1_1 = "";
            string strMun6_1_2 = "";
            string strMun6_1_3 = "";
            string strMun6_2 = "";
            string strMun6_2_1 = "";
            string strMun6_2_2 = "";
            string strMun6_2_3 = "";

            string strMun7N = "";
            string strMun7Y = "";
            string strMun7Y1 = "";
            string strMun7Y2 = "";
            string strMun7Y3 = "";
            string strMun7Y4 = "";
            string strMun7Y5 = "";
            string strMun7Y6 = "";
            string strMun7Y7 = "";
            string strMun7Y7_1 = "";

            string strMun8N = "";
            string strMun8Y = "";
            string strMun8Y_1 = "";


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

            string strMun12_1 = "";

            string strRemark = "";

            strH = txtHeight.Text.Trim();
            strW = txtWeight.Text.Trim();

            strB1 = txtB1.Text.Trim();
            strB2 = txtB2.Text.Trim();

            //1
            strMun1 = txt1.Text.Trim();

            //2
            strMunJob = txt2_1.Text.Trim();
            strMunGrade = txt2_2.Text.Trim();

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
            strMun3_2_1 = txt3.Text.Trim();
            strMun3_F1 = txt3_1_1.Text.Trim();
            strMun3_F2 = txt3_1_2.Text.Trim();
            strMun3_F3 = txt3_1_3.Text.Trim();
            strMun3_F4 = txt3_1_4.Text.Trim();
            strMun3_M1 = txt3_2_1.Text.Trim();
            strMun3_M2 = txt3_2_2.Text.Trim();
            strMun3_M3 = txt3_2_3.Text.Trim();
            strMun3_M4 = txt3_2_4.Text.Trim();
            strMun3_B1 = txt3_3_1.Text.Trim();
            strMun3_B2 = txt3_3_2.Text.Trim();
            strMun3_B3 = txt3_3_3.Text.Trim();

            //4
            strMun_M_N = "N";
            if (chkNo_4_1.Checked == true)
            {
                strMun_M_N = "Y";
            }
            strMun_M_Y = "N";
            if (chkNo_4_2.Checked == true)
            {
                strMun_M_Y = "Y";
            }

            strMun_M_Y1 = "N";
            if (chkNo_4_3.Checked == true)
            {
                strMun_M_Y1 = "Y";
            }
            strMun_M_Y2 = "N";
            if (chkNo_4_4.Checked == true)
            {
                strMun_M_Y2 = "Y";
            }
            strMun_M_Y3 = "N";
            if (chkNo_4_5.Checked == true)
            {
                strMun_M_Y3 = "Y";
            }
            strMun_M_Y4 = "N";
            if (chkNo_4_6.Checked == true)
            {
                strMun_M_Y4 = "Y";
            }

            strMun_M_M1 = txt4_1_1.Text.Trim();
            strMun_M_M2 = txt4_1_2.Text.Trim();
            strMun_M_M3 = txt4_1_3.Text.Trim();

            strMun_M_S1 = txt4_2_1.Text.Trim();
            strMun_M_S2 = txt4_2_2.Text.Trim();

            //5
            //strMunOldN = "N";
            if (chkNo_5_1.Checked == true)
            {
                //strMunOldN = "Y";
            }
            //strMunOldY = "N";
            if (chkNo_5_2.Checked == true)
            {
                //strMunOldY = "Y";
            }
            strMunOldY1 = "N";
            if (chkNo_5_3.Checked == true)
            {
                strMunOldY1 = "Y";
            }
            strMunOldY2 = "N";
            if (chkNo_5_4.Checked == true)
            {
                strMunOldY2 = "Y";
            }
            strMunOldY3 = "N";
            if (chkNo_5_5.Checked == true)
            {
                strMunOldY3 = "Y";
            }
            strMunOldY4 = "N";
            if (chkNo_5_6.Checked == true)
            {
                strMunOldY4 = "Y";
            }
            strMunOldY5 = "N";
            if (chkNo_5_7.Checked == true)
            {
                strMunOldY5 = "Y";
            }
            strMunOldY6 = "N";
            if (chkNo_5_8.Checked == true)
            {
                strMunOldY6 = "Y";
            }
            strMunOldY7 = "N";
            if (chkNo_5_9.Checked == true)
            {
                strMunOldY7 = "Y";
            }
            strMunOldY8 = "N";
            if (chkNo_5_10.Checked == true)
            {
                strMunOldY8 = "Y";
            }
            strMunOldY9 = "N";
            if (chkNo_5_11.Checked == true)
            {
                strMunOldY9 = "Y";
            }
            strMunOldY10 = "N";
            if (chkNo_5_12.Checked == true)
            {
                strMunOldY10 = "Y";
            }
            strMunOldY10_1 = txt5_1.Text.Trim();

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
            strMun7N = "N";
            if (chkNo_7_1.Checked == true)
            {
                strMun7N = "Y";
            }
            strMun7Y = "N";
            if (chkNo_7_2.Checked == true)
            {
                strMun7Y = "Y";
            }

            strMun7Y1 = "N";
            if (chkNo_7_3.Checked == true)
            {
                strMun7Y1 = "Y";
            }
            strMun7Y2 = "N";
            if (chkNo_7_4.Checked == true)
            {
                strMun7Y2 = "Y";
            }
            strMun7Y3 = "N";
            if (chkNo_7_5.Checked == true)
            {
                strMun7Y3 = "Y";
            }
            strMun7Y4 = "N";
            if (chkNo_7_6.Checked == true)
            {
                strMun7Y4 = "Y";
            }
            strMun7Y5 = "N";
            if (chkNo_7_7.Checked == true)
            {
                strMun7Y5 = "Y";
            }
            strMun7Y6 = "N";
            if (chkNo_7_8.Checked == true)
            {
                strMun7Y6 = "Y";
            }
            strMun7Y7 = "N";
            if (chkNo_7_9.Checked == true)
            {
                strMun7Y7 = "Y";
            }
            strMun7Y7_1 = txt7_1.Text.Trim();


            //8
            strMun8N = "N";
            if (chkNo_8_1.Checked == true)
            {
                strMun8N = "Y";
            }
            strMun8Y = "N";
            if (chkNo_8_2.Checked == true)
            {
                strMun8Y = "Y";
            }
            strMun8Y_1 = txt8_1.Text.Trim();


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
                    SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_DEPT_MUNJIN (BDate , Pano, DeptCode, DrCode,TMUN_H, TMUN_W,TMun_B1,TMun_B2, ";
                    //1
                    SQL = SQL + ComNum.VBLF + " TMUN1,TMUN_JOB,TMUN_GRADE, ";
                    //3
                    SQL = SQL + ComNum.VBLF + " TMUN_FN,TMUN_FY,TMUN_FY_1,TMUN_F_F1,TMUN_F_F2,TMUN_F_F3,TMUN_F_F4, ";
                    SQL = SQL + ComNum.VBLF + " TMUN_F_M1,TMUN_F_M2,TMUN_F_M3,TMUN_F_M4,TMUN_F_B1,TMUN_F_B2,TMUN_F_B3,";
                    //4
                    SQL = SQL + ComNum.VBLF + " TMUN_MARRY_N,TMUN_MARRY_Y,TMUN_MARRY_ETC1,TMUN_MARRY_ETC2,TMUN_MARRY_ETC3,";
                    SQL = SQL + ComNum.VBLF + " TMUN_MARRY_ETC4,TMUN_MARRY_M1,TMUN_MARRY_M2,TMUN_MARRY_M3,TMUN_MARRY_S1,TMUN_MARRY_S2, ";

                    //5
                    SQL = SQL + ComNum.VBLF + " TMUN2_1,TMUN2_2,TMUN2_3,TMUN2_4,TMUN2_5,TMUN2_6,TMUN2_7,TMUN2_8,TMUN2_11,TMUN2_10,TMUN2_10_1, ";

                    //6
                    SQL = SQL + ComNum.VBLF + " TMUN6_1,TMUN6_1_1,TMUN6_1_2,TMUN6_1_3,TMUN6_2,TMUN6_2_1,TMUN6_2_2,TMUN6_2_3, ";

                    //7
                    SQL = SQL + ComNum.VBLF + " TMUN3_1,TMUN3_2,TMUN3_2_1,TMUN3_2_2,TMUN3_2_3,TMUN3_2_4,TMUN3_2_5,TMUN3_2_6,TMUN3_2_7,TMUN3_2_7_1,";

                    //8
                    SQL = SQL + ComNum.VBLF + " TMUN9_1,TMUN9_2,TMUN9_2_8, ";

                    SQL = SQL + ComNum.VBLF + " TMUN10_1,TMUN10_2,TMUN10_2_1,TMUN10_3_1,TMUN10_3_2,TMUN10_3_3,TMUN10_3_4, ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_4_1,TMUN10_4_2,TMUN10_4_3,TMUN10_4_4,TMUN10_4_5,TMUN10_5_1,TMUN10_5_2, ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_3,TMUN10_5_4,TMUN10_5_5,TMUN10_5_6,TMUN10_5_7,TMUN10_5_8,TMUN10_5_9, ";
                    SQL = SQL + ComNum.VBLF + " TMUN10_5_10,TMUN10_5_11,TMUN10_5_11_1,tmun12_t1,";

                    SQL = SQL + ComNum.VBLF + " Remark, ENTSABUN, ENTDATE ) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + FstrBDate + "','YYYY-MM-DD'),'" + FstrPano + "','" + FstrDept + "','" + FstrDrCode + "','" + strH + "','" + strW + "','" + strB1 + "','" + strB2 + "', ";
                    //1
                    SQL = SQL + ComNum.VBLF + " '" + strMun1 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strMunJob + "', '" + strMunGrade + "', ";
                    //3
                    SQL = SQL + ComNum.VBLF + " '" + strMun3_1 + "', '" + strMun3_2 + "','" + strMun3_2_1 + "','" + strMun3_F1 + "','" + strMun3_F2 + "','" + strMun3_F3 + "','" + strMun3_F4 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strMun3_M1 + "','" + strMun3_M2 + "','" + strMun3_M3 + "','" + strMun3_M4 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strMun3_B1 + "','" + strMun3_B2 + "','" + strMun3_B3 + "',";
                    //4
                    SQL = SQL + ComNum.VBLF + " '" + strMun_M_N + "','" + strMun_M_Y + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strMun_M_Y1 + "','" + strMun_M_Y2 + "','" + strMun_M_Y3 + "','" + strMun_M_Y4 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strMun_M_M1 + "','" + strMun_M_M2 + "','" + strMun_M_M3 + "','" + strMun_M_S1 + "','" + strMun_M_S2 + "', ";

                    //5
                    SQL = SQL + ComNum.VBLF + " '" + strMunOldY1 + "','" + strMunOldY2 + "','" + strMunOldY3 + "','" + strMunOldY4 + "','" + strMunOldY5 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMunOldY6 + "','" + strMunOldY7 + "','" + strMunOldY8 + "','" + strMunOldY9 + "','" + strMunOldY10 + "','" + strMunOldY10_1 + "',";

                    //6
                    SQL = SQL + ComNum.VBLF + " '" + strMun6_1 + "', '" + strMun6_1_1 + "', '" + strMun6_1_2 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun6_1_3 + "', '" + strMun6_2 + "', '" + strMun6_2_1 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun6_2_2 + "', '" + strMun6_2_3 + "', ";

                    //7
                    SQL = SQL + ComNum.VBLF + " '" + strMun7N + "','" + strMun7Y + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun7Y1 + "','" + strMun7Y2 + "','" + strMun7Y3 + "','" + strMun7Y4 + "','" + strMun7Y5 + "','" + strMun7Y6 + "','" + strMun7Y7 + "','" + strMun7Y7_1 + "',";

                    //8
                    SQL = SQL + ComNum.VBLF + " '" + strMun8N + "','" + strMun8Y + "','" + strMun8Y_1 + "',";


                    SQL = SQL + ComNum.VBLF + " '" + strMun10_1 + "', '" + strMun10_2 + "', '" + strMun10_2_1 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun10_3_1 + "', '" + strMun10_3_2 + "', '" + strMun10_3_3 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun10_3_4 + "', '" + strMun10_4_1 + "', '" + strMun10_4_2 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun10_4_3 + "', '" + strMun10_4_4 + "', '" + strMun10_4_5 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun10_5_1 + "', '" + strMun10_5_2 + "', '" + strMun10_5_3 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun10_5_4 + "', '" + strMun10_5_5 + "', '" + strMun10_5_6 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun10_5_7 + "', '" + strMun10_5_8 + "', '" + strMun10_5_9 + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strMun10_5_10 + "', '" + strMun10_5_11 + "', '" + strMun10_5_11_1 + "',";

                    SQL = SQL + ComNum.VBLF + " '" + strMun12_1 + "',";

                    SQL = SQL + ComNum.VBLF + " '" + strRemark + "'," + clsType.User.Sabun + ",SYSDATE ) ";
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_DEPT_MUNJIN SET ";

                    SQL = SQL + ComNum.VBLF + " TMUN_H ='" + strH + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_W ='" + strW + "', ";

                    SQL = SQL + ComNum.VBLF + " TMUN_B1 ='" + strB1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_B2='" + strB2 + "', ";

                    SQL = SQL + ComNum.VBLF + " TMUN1 ='" + strMun1 + "', ";

                    SQL = SQL + ComNum.VBLF + " TMUN_JOB ='" + strMunJob + "',";
                    SQL = SQL + ComNum.VBLF + " TMUN_GRADE = '" + strMunGrade + "', ";

                    //3
                    SQL = SQL + ComNum.VBLF + " TMUN_FN ='" + strMun3_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_FY ='" + strMun3_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_FY_1 ='" + strMun3_2_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_F_F1 ='" + strMun3_F1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_F_F2 ='" + strMun3_F2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_F_F3 ='" + strMun3_F3 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_F_F4 ='" + strMun3_F4 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_F_M1 ='" + strMun3_M1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_F_M2 ='" + strMun3_M2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_F_M3 ='" + strMun3_M3 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_F_M4 ='" + strMun3_M4 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_F_B1 ='" + strMun3_B1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_F_B2 ='" + strMun3_B2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_F_B3 ='" + strMun3_B3 + "', ";

                    //4
                    SQL = SQL + ComNum.VBLF + " TMUN_MARRY_N ='" + strMun_M_N + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_MARRY_Y ='" + strMun_M_Y + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_MARRY_ETC1 ='" + strMun_M_Y1 + "',";
                    SQL = SQL + ComNum.VBLF + " TMUN_MARRY_ETC2 ='" + strMun_M_Y2 + "',";
                    SQL = SQL + ComNum.VBLF + " TMUN_MARRY_ETC3 ='" + strMun_M_Y3 + "',";
                    SQL = SQL + ComNum.VBLF + " TMUN_MARRY_ETC4 ='" + strMun_M_Y4 + "',";
                    SQL = SQL + ComNum.VBLF + " TMUN_MARRY_M1 ='" + strMun_M_M1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_MARRY_M2 ='" + strMun_M_M2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_MARRY_M3 ='" + strMun_M_M3 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_MARRY_S1 ='" + strMun_M_S1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN_MARRY_S2 ='" + strMun_M_S2 + "', ";

                    //5
                    SQL = SQL + ComNum.VBLF + " TMUN2_1 ='" + strMunOldY1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_2 ='" + strMunOldY2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_3 ='" + strMunOldY3 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_4 ='" + strMunOldY4 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_5 ='" + strMunOldY5 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_6 ='" + strMunOldY6 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_7 ='" + strMunOldY7 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_8 ='" + strMunOldY8 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_11 ='" + strMunOldY9 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_10 ='" + strMunOldY10 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN2_10_1 ='" + strMunOldY10_1 + "', ";

                    //6
                    SQL = SQL + ComNum.VBLF + " TMUN6_1 ='" + strMun6_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN6_1_1 ='" + strMun6_1_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN6_1_2 ='" + strMun6_1_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN6_1_3 ='" + strMun6_1_3 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN6_2 ='" + strMun6_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN6_2_1 ='" + strMun6_2_1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN6_2_2 ='" + strMun6_2_2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN6_2_3 ='" + strMun6_2_3 + "', ";

                    //7
                    SQL = SQL + ComNum.VBLF + " TMUN3_1 ='" + strMun7N + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2 ='" + strMun7Y + "', ";

                    SQL = SQL + ComNum.VBLF + " TMUN3_2_1 ='" + strMun7Y1 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_2 ='" + strMun7Y2 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_3 ='" + strMun7Y3 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_4 ='" + strMun7Y4 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_5 ='" + strMun7Y5 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_6 ='" + strMun7Y6 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_7 ='" + strMun7Y7 + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN3_2_7_1 ='" + strMun7Y7_1 + "', ";


                    //8
                    SQL = SQL + ComNum.VBLF + " TMUN9_1 ='" + strMun8N + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2 ='" + strMun8Y + "', ";
                    SQL = SQL + ComNum.VBLF + " TMUN9_2_8 ='" + strMun8Y_1 + "', ";


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
                chkNo_10.Enabled = false;
                chkNo_11.Enabled = false;
                btnChart.Enabled = false;
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
                chkNo_10.Enabled = true;
                chkNo_11.Enabled = true;
                                
                chkNo_1.Checked = true;
                chkNo_2.Checked = true;
                chkNo_3.Checked = true;
                chkNo_4.Checked = true;
                chkNo_5.Checked = true;
                chkNo_6.Checked = true;
                chkNo_7.Checked = true;
                chkNo_10.Checked = true;
                chkNo_11.Checked = true;
                btnChart.Enabled = true;

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
            //1
            txt1.Text = "";

            //2
            txt2_1.Text = "";
            txt2_2.Text = "";


            txtHeight.Text = "";
            txtWeight.Text = "";

            txtB1.Text = "";
            txtB2.Text = "";

            //3.
            txt3.Text = "";
            chkNo_3_1.Checked = false;
            chkNo_3_2.Checked = false;
            txt3_1_1.Text = "";
            txt3_1_2.Text = "";
            txt3_1_3.Text = "";
            txt3_1_4.Text = "";
            txt3_2_1.Text = "";
            txt3_2_2.Text = "";
            txt3_2_3.Text = "";
            txt3_2_4.Text = "";
            txt3_3_1.Text = "";
            txt3_3_2.Text = "";
            txt3_3_3.Text = "";

            //4.
            chkNo_4_1.Checked = false;
            chkNo_4_2.Checked = false;
            chkNo_4_3.Checked = false;
            chkNo_4_4.Checked = false;
            chkNo_4_5.Checked = false;
            chkNo_4_6.Checked = false;
            txt4_1_1.Text = "";
            txt4_1_2.Text = "";
            txt4_1_3.Text = "";
            txt4_2_1.Text = "";
            txt4_2_2.Text = "";

            //5.
            chkNo_5_1.Checked = false;
            chkNo_5_2.Checked = false;
            chkNo_5_3.Checked = false;
            chkNo_5_4.Checked = false;
            chkNo_5_5.Checked = false;
            chkNo_5_6.Checked = false;
            chkNo_5_7.Checked = false;
            chkNo_5_8.Checked = false;
            chkNo_5_9.Checked = false;
            chkNo_5_10.Checked = false;
            chkNo_5_11.Checked = false;
            chkNo_5_12.Checked = false;
            txt5_1.Text = "";


            //6.
            chkNo_6_1.Checked = false;
            chkNo_6_1_1.Checked = false;
            txt6_1_1_1.Text = "";
            txt6_1_1_2.Text = "";

            chkNo_6_2.Checked = false;
            chkNo_6_2_1.Checked = false;
            txt6_2_1_1.Text = "";
            txt6_2_1_2.Text = "";


            //7.
            chkNo_7_1.Checked = false;
            chkNo_7_2.Checked = false;
            chkNo_7_3.Checked = false;
            chkNo_7_4.Checked = false;
            chkNo_7_5.Checked = false;
            chkNo_7_6.Checked = false;
            chkNo_7_7.Checked = false;
            chkNo_7_8.Checked = false;
            chkNo_7_9.Checked = false;
            txt7_1.Text = "";


            //8.
            chkNo_8_1.Checked = false;
            chkNo_8_2.Checked = false;
            txt8_1.Text = "";


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

            //12 해외여행력
            chkNo_12_1.Checked = false;
            chkNo_12_2.Checked = false;
            chkNo_13.Checked = false;

            txtRemark.Text = "";

            txtEMRNO.Text = "";
        }

        private void ssDept_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strDEPT = "";

            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            strDEPT = ssDept_Sheet1.Cells[e.Row, e.Column].Text.Trim();

            Read_Munjin_Data("2", FstrPano, strDEPT, FstrBDate);
        }

        private void txt1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt8_1.Focus();
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

        private void frmMedicalInquiry_NP_Load(object sender, EventArgs e)
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

        void Read_Munjin_Data(string ArgSTS, string ArgPano, string ArgDeptCode, string ArgBDate)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strTemp = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT BDATE,PANO,DEPTCODE,DRCODE,REMARK,ENTSABUN,ENTDATE,DELDATE,CHK,TMUN_H, TMUN_W, TMUN_B1, TMUN_B2, ";

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
                SQL = SQL + ComNum.VBLF + " TMUN10_5_10,TMUN10_5_11,TMUN10_5_11_1,";

                //add
                SQL = SQL + ComNum.VBLF + " TMUN_JOB,TMUN_GRADE,TMUN_FN,TMUN_FY,TMUN_FY_1,TMUN_F_F1,TMUN_F_F2,";
                SQL = SQL + ComNum.VBLF + " TMUN_F_F3,TMUN_F_F4,TMUN_F_M1,TMUN_F_M2,TMUN_F_M3,TMUN_F_M4,TMUN_F_B1,TMUN_F_B2,";
                SQL = SQL + ComNum.VBLF + " TMUN_F_B3,TMUN_MARRY_N,TMUN_MARRY_Y,TMUN_MARRY_ETC1,TMUN_MARRY_ETC2,TMUN_MARRY_ETC3,";
                SQL = SQL + ComNum.VBLF + " TMUN_MARRY_ETC4,TMUN_MARRY_M1,TMUN_MARRY_M2,TMUN_MARRY_M3,TMUN_MARRY_S1,";
                SQL = SQL + ComNum.VBLF + " TMUN_MARRY_S2 , TMUN2_11, TMUN3_2_8, TMUN3_2_9, TMUN3_2_10, TMUN3_2_11, TMUN9_2_8,tmun12_t1,";

                SQL = SQL + ComNum.VBLF + " Remark, ENTSABUN, ENTDATE,ROWID, EMRNO ";

                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_DEPT_MUNJIN ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + ArgDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE ='')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                FstrROWID = "";

                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("등록된 예진표자료가 없습니다.");
                    return;
                }

                if (ArgSTS != "2")
                {
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                //1
                txt1.Text = dt.Rows[0]["TMUN1"].ToString().Trim();

                txtHeight.Text = dt.Rows[0]["TMUN_H"].ToString().Trim();
                txtWeight.Text = dt.Rows[0]["TMUN_W"].ToString().Trim();


                txtB1.Text = dt.Rows[0]["TMUN_B1"].ToString().Trim();
                txtB2.Text = dt.Rows[0]["TMUN_B2"].ToString().Trim();

                txt2_1.Text = dt.Rows[0]["TMUN_JOB"].ToString().Trim();
                txt2_2.Text = dt.Rows[0]["TMUN_GRADE"].ToString().Trim();

                //3
                chkNo_3_1.Checked = false;
                if (dt.Rows[0]["TMUN_FN"].ToString().Trim() == "Y")
                {
                    chkNo_3_1.Checked = true;
                }
                chkNo_3_2.Checked = false;
                if (dt.Rows[0]["TMUN_FY"].ToString().Trim() == "Y")
                {
                    chkNo_3_2.Checked = true;
                }
                txt3.Text = dt.Rows[0]["TMUN_FY_1"].ToString().Trim();

                txt3_1_1.Text = dt.Rows[0]["TMUN_F_F1"].ToString().Trim();
                txt3_1_2.Text = dt.Rows[0]["TMUN_F_F2"].ToString().Trim();
                txt3_1_3.Text = dt.Rows[0]["TMUN_F_F3"].ToString().Trim();
                txt3_1_4.Text = dt.Rows[0]["TMUN_F_F4"].ToString().Trim();

                txt3_2_1.Text = dt.Rows[0]["TMUN_F_M1"].ToString().Trim();
                txt3_2_2.Text = dt.Rows[0]["TMUN_F_M2"].ToString().Trim();
                txt3_2_3.Text = dt.Rows[0]["TMUN_F_M3"].ToString().Trim();
                txt3_2_4.Text = dt.Rows[0]["TMUN_F_M4"].ToString().Trim();

                txt3_3_1.Text = dt.Rows[0]["TMUN_F_B1"].ToString().Trim();
                txt3_3_2.Text = dt.Rows[0]["TMUN_F_B2"].ToString().Trim();
                txt3_3_3.Text = dt.Rows[0]["TMUN_F_B3"].ToString().Trim();

                //4
                chkNo_4_1.Checked = false;
                if (dt.Rows[0]["TMUN_MARRY_N"].ToString().Trim() == "Y")
                {
                    chkNo_4_1.Checked = true;
                }
                chkNo_4_2.Checked = false;
                if (dt.Rows[0]["TMUN_MARRY_Y"].ToString().Trim() == "Y")
                {
                    chkNo_4_2.Checked = true;
                }
                chkNo_4_3.Checked = false;
                if (dt.Rows[0]["TMUN_MARRY_ETC1"].ToString().Trim() == "Y")
                {
                    chkNo_4_3.Checked = true;
                }
                chkNo_4_4.Checked = false;
                if (dt.Rows[0]["TMUN_MARRY_ETC2"].ToString().Trim() == "Y")
                {
                    chkNo_4_4.Checked = true;
                }
                chkNo_4_5.Checked = false;
                if (dt.Rows[0]["TMUN_MARRY_ETC3"].ToString().Trim() == "Y")
                {
                    chkNo_4_5.Checked = true;
                }
                chkNo_4_6.Checked = false;
                if (dt.Rows[0]["TMUN_MARRY_ETC4"].ToString().Trim() == "Y")
                {
                    chkNo_4_6.Checked = true;
                }

                txt4_1_1.Text = dt.Rows[0]["TMUN_MARRY_M1"].ToString().Trim();
                txt4_1_2.Text = dt.Rows[0]["TMUN_MARRY_M2"].ToString().Trim();
                txt4_1_3.Text = dt.Rows[0]["TMUN_MARRY_M3"].ToString().Trim();

                txt4_2_1.Text = dt.Rows[0]["TMUN_MARRY_S1"].ToString().Trim();
                txt4_2_2.Text = dt.Rows[0]["TMUN_MARRY_S2"].ToString().Trim();

                chkNo_5_1.Checked = false; //무
                chkNo_5_2.Checked = false; //유
                strTemp = "";

                //5
                chkNo_5_3.Checked = false;
                if (dt.Rows[0]["TMUN2_1"].ToString().Trim() == "Y")
                {
                    chkNo_5_3.Checked = true;
                    strTemp = "Y";
                }
                chkNo_5_4.Checked = false;
                if (dt.Rows[0]["TMUN2_2"].ToString().Trim() == "Y")
                {
                    chkNo_5_4.Checked = true;
                    strTemp = "Y";
                }
                chkNo_5_5.Checked = false;
                if (dt.Rows[0]["TMUN2_3"].ToString().Trim() == "Y")
                {
                    chkNo_5_5.Checked = true;
                    strTemp = "Y";
                }
                chkNo_5_6.Checked = false;
                if (dt.Rows[0]["TMUN2_4"].ToString().Trim() == "Y")
                {
                    chkNo_5_6.Checked = true;
                    strTemp = "Y";
                }
                chkNo_5_7.Checked = false;
                if (dt.Rows[0]["TMUN2_5"].ToString().Trim() == "Y")
                {
                    chkNo_5_7.Checked = true;
                    strTemp = "Y";
                }
                chkNo_5_8.Checked = false;
                if (dt.Rows[0]["TMUN2_6"].ToString().Trim() == "Y")
                {
                    chkNo_5_8.Checked = true;
                    strTemp = "Y";
                }
                chkNo_5_9.Checked = false;
                if (dt.Rows[0]["TMUN2_7"].ToString().Trim() == "Y")
                {
                    chkNo_5_9.Checked = true;
                    strTemp = "Y";
                }
                chkNo_5_10.Checked = false;
                if (dt.Rows[0]["TMUN2_8"].ToString().Trim() == "Y")
                {
                    chkNo_5_10.Checked = true;
                    strTemp = "Y";
                }
                chkNo_5_11.Checked = false;
                if (dt.Rows[0]["TMUN2_11"].ToString().Trim() == "Y")
                {
                    chkNo_5_11.Checked = true;
                    strTemp = "Y";
                }
                chkNo_5_12.Checked = false;
                if (dt.Rows[0]["TMUN2_10"].ToString().Trim() == "Y")
                {
                    chkNo_5_12.Checked = true;
                    strTemp = "Y";
                }
                txt5_1.Text = dt.Rows[0]["TMUN2_10_1"].ToString().Trim();

                if (strTemp == "Y")
                {
                    chkNo_5_2.Checked = true; //유
                }
                else
                {
                    chkNo_5_1.Checked = true; //무
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
                if (dt.Rows[0]["TMUN3_1"].ToString().Trim() == "Y")
                {
                    chkNo_7_1.Checked = true;
                }
                chkNo_7_2.Checked = false;
                if (dt.Rows[0]["TMUN3_2"].ToString().Trim() == "Y")
                {
                    chkNo_7_2.Checked = true;
                }

                chkNo_7_3.Checked = false;
                if (dt.Rows[0]["TMUN3_2_1"].ToString().Trim() == "Y")
                {
                    chkNo_7_3.Checked = true;
                }
                chkNo_7_4.Checked = false;
                if (dt.Rows[0]["TMUN3_2_2"].ToString().Trim() == "Y")
                {
                    chkNo_7_4.Checked = true;
                }

                chkNo_7_5.Checked = false;
                if (dt.Rows[0]["TMUN3_2_3"].ToString().Trim() == "Y")
                {
                    chkNo_7_5.Checked = true;
                }
                chkNo_7_6.Checked = false;
                if (dt.Rows[0]["TMUN3_2_4"].ToString().Trim() == "Y")
                {
                    chkNo_7_6.Checked = true;
                }
                chkNo_7_7.Checked = false;
                if (dt.Rows[0]["TMUN3_2_5"].ToString().Trim() == "Y")
                {
                    chkNo_7_7.Checked = true;
                }
                chkNo_7_8.Checked = false;
                if (dt.Rows[0]["TMUN3_2_6"].ToString().Trim() == "Y")
                {
                    chkNo_7_8.Checked = true;
                }
                chkNo_7_9.Checked = false;
                if (dt.Rows[0]["TMUN3_2_7"].ToString().Trim() == "Y")
                {
                    chkNo_7_9.Checked = true;
                }
                txt7_1.Text = dt.Rows[0]["TMUN3_2_7_1"].ToString().Trim();

                //8
                chkNo_8_1.Checked = false;
                if (dt.Rows[0]["TMUN9_1"].ToString().Trim() == "Y")
                {
                    chkNo_8_1.Checked = true;
                }
                chkNo_8_2.Checked = false;
                if (dt.Rows[0]["TMUN9_2"].ToString().Trim() == "Y")
                {
                    chkNo_8_2.Checked = true;
                }
                txt8_1.Text = dt.Rows[0]["TMUN9_2_8"].ToString().Trim();


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

        void Set_Info()
        {
            ssPatInfo_Sheet1.Cells[0, 0].Text = FstrPano;
            ssPatInfo_Sheet1.Cells[0, 1].Text = FstrSName;
            ssPatInfo_Sheet1.Cells[0, 2].Text = FstrDept;
            ssPatInfo_Sheet1.Cells[0, 3].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, FstrDrCode);
            ssPatInfo_Sheet1.Cells[0, 4].Text = FstrBDate;
            ssPatInfo_Sheet1.Cells[0, 5].Text = FstrDrCode;
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

            ((TextBox)sender).Text = clsPublic.GstrCalDate;
        }

        private void txt8_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt2_1.Focus();
            }
        }

        private void txt2_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt2_2.Focus();
            }            
        }
        
        private void txt2_2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtHeight.Focus();
            }            
        }

        private void txt2_2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtHeight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtWeight.Focus();
            }
        }

        private void txtWeight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtB1.Focus();
            }
        }

        private void txtB1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtB2.Focus();
            }
        }

        private void txtB2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt3.Focus();
            }
        }

        private void txt3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt3_1_1.Focus();
            }
        }

        private void txt3_1_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt3_1_2.Focus();
            }
        }

        private void txt3_1_2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt3_1_3.Focus();
            }
        }

        private void txt3_1_3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt3_1_4.Focus();
            }
        }

        private void txt3_1_4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt3_2_1.Focus();
            }
        }

        private void txt3_2_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt3_2_2.Focus();
            }
        }

        private void txt3_2_2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt3_2_3.Focus();
            }
        }

        private void txt3_2_3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt3_2_4.Focus();
            }
        }

        private void txt3_2_4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt3_3_1.Focus();
            }
        }

        private void txt3_3_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt3_3_2.Focus();
            }
        }

        private void txt3_3_2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt3_3_3.Focus();
            }
        }

        private void txt3_3_3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt4_1_1.Focus();
            }
        }

        private void txt4_1_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt4_1_2.Focus();
            }
        }

        private void txt4_1_2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt4_1_3.Focus();
            }
        }

        private void txt4_1_3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt4_2_1.Focus();
            }
        }

        private void txt4_2_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt4_2_2.Focus();
            }
        }

        private void txt4_2_2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt5_1.Focus();
            }
        }

        private void txt5_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt6_1_1_1.Focus();
            }
        }

        private void txt6_1_1_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt6_1_1_2.Focus();
            }
        }

        private void txt6_1_1_2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt6_2_1_1.Focus();
            }
        }

        private void txt6_2_1_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt6_2_1_2.Focus();
            }
        }

        private void txt6_2_1_2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt7_1.Focus();
            }
        }

        private void txt7_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt10_2_1.Focus();
            }
        }

        private void txt10_2_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt10_5_11_1.Focus();
            }
        }

        private void txt10_5_11_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtRemark.Focus();
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
                chkNo_10.Checked = false;
                chkNo_11.Checked = false;
                chkNo_13.Checked = false;
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
                chkNo_10.Checked = true;
                chkNo_11.Checked = true;
                chkNo_13.Checked = true;
                btnAll.Text = "모든항목 해제";
            }
        }
    }
}
