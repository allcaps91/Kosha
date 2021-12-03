using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComLibB;
using ComEmrBase;

namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmMedicalInquiry.cs
    /// Description     : 진료예진표_기본
    /// Author          : 유진호
    /// Create Date     : 2018-01-09
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\emr\emrprt\Frm진료예진표1
    /// </history>
    /// </summary>
    public partial class frmMedicalInquiry : Form
    {
        ComFunc CF = new ComFunc();
        private string FstrPaNo = "";
        private string FstrPaName = "";
        private string FstrDept = "";
        private string FstrDrCode = "";
        private string FstrBDate = "";

        private string FstrROWID = "";

        public frmMedicalInquiry()
        {
            InitializeComponent();
        }

        public frmMedicalInquiry(string strPaNo, string strPaName, string strDept, string strDrCode, string strBDate)
        {
            InitializeComponent();
            this.FstrPaNo = strPaNo;
            this.FstrPaName = strPaName;
            this.FstrDept = strDept;
            this.FstrDrCode = strDrCode;
            this.FstrBDate = strBDate;
        }

        private void frmMedicalInquiry_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
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

            SCREEN_CLEAR();
            SET_Info();
            btnSearchClick();
        }

        private void SCREEN_CLEAR()
        {
            txtAnswer_1.Text = "";
            txtAnswer_2.Text = "";

            chkAnswer_31.Checked = false;
            chkAnswer_32.Checked = false;
            chkAnswer_33.Checked = false;
            txtAnswer_3.Text = "";

            chkAnswer_41.Checked = false;
            chkAnswer_42.Checked = false;
            txtAnswer_4.Text = "";

            txtAnswer_5.Text = "";
            txtAnswer_6.Text = "";

            chkAnswer_71.Checked = false;
            chkAnswer_72.Checked = false;
            txtAnswer_7.Text = "";

            txtAnswer_81.Text = "";
            txtAnswer_82.Text = "";

            chkAnswer_91.Checked = false;
            chkAnswer_92.Checked = false;
            chkAnswer_93.Checked = false;
            chkAnswer_94.Checked = false;
            chkAnswer_95.Checked = false;
            txtAnswer_91.Text = "";
            txtAnswer_92.Text = "";

            chkAnswer_101.Checked = false;
            chkAnswer_102.Checked = false;
            txtAnswer_101.Text = "";
            txtAnswer_102.Text = "";

            txtEMRNO.Text = "";
        }

        private void SET_Info()
        {
            ssPatInfo_Sheet1.Cells[0, 0].Text = FstrPaNo;
            ssPatInfo_Sheet1.Cells[0, 1].Text = FstrPaName;
            ssPatInfo_Sheet1.Cells[0, 2].Text = FstrDept;
            ssPatInfo_Sheet1.Cells[0, 3].Text = CF.READ_DrName(clsDB.DbCon, FstrDrCode);
            ssPatInfo_Sheet1.Cells[0, 4].Text = FstrBDate;
            ssPatInfo_Sheet1.Cells[0, 5].Text = FstrDrCode;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            btnSearchClick();
        }

        private void btnSearchClick()
        {
            SCREEN_CLEAR();

            READ_Munjin_Data(FstrPaNo, FstrDept, FstrBDate);
            DATA_BULID_PRE();
        }

        private void READ_Munjin_Data(string strPaNo, string strDept, string strBDate)
        {

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT BDATE,PANO,DEPTCODE,DRCODE,MUN1,MUN2,MUN3_1,MUN3_2,MUN3_3,MUN3_4,MUN4_1,";
                SQL = SQL + ComNum.VBLF + " MUN4_2,MUN5,MUN6,MUN7_1,MUN7_2,MUN8_1,MUN8_2,MUN9_1,MUN9_2,MUN9_3,MUN9_4,MUN9_5,";
                SQL = SQL + ComNum.VBLF + " MUN9_6 , MUN9_7, MUN10_1, MUN10_2, MUN10_3, REMARK, ENTSABUN, ENTDATE,ROWID, EMRNO ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_DEPT_MUNJIN ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + strPaNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDept + "' ";
                SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE ='')";

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
                    ComFunc.MsgBox("등록된 예진표자료가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                    txtAnswer_1.Text = dt.Rows[0]["MUN1"].ToString().Trim();
                    txtAnswer_2.Text = dt.Rows[0]["MUN2"].ToString().Trim();

                    chkAnswer_31.Checked = false;
                    chkAnswer_32.Checked = false;
                    chkAnswer_33.Checked = false;
                    if (dt.Rows[0]["MUN3_1"].ToString().Trim() == "Y") chkAnswer_31.Checked = true;
                    if (dt.Rows[0]["MUN3_2"].ToString().Trim() == "Y") chkAnswer_32.Checked = true;
                    if (dt.Rows[0]["MUN3_3"].ToString().Trim() == "Y") chkAnswer_33.Checked = true;
                    txtAnswer_3.Text = dt.Rows[0]["MUN3_4"].ToString().Trim();

                    chkAnswer_41.Checked = false;
                    chkAnswer_42.Checked = false;
                    if (dt.Rows[0]["MUN4_1"].ToString().Trim() == "Y")
                    {
                        chkAnswer_41.Checked = true;
                    }
                    else
                    {
                        chkAnswer_42.Checked = true;
                    }
                    txtAnswer_4.Text = dt.Rows[0]["MUN4_2"].ToString().Trim();

                    txtAnswer_5.Text = dt.Rows[0]["MUN5"].ToString().Trim();
                    txtAnswer_6.Text = dt.Rows[0]["MUN6"].ToString().Trim();

                    chkAnswer_71.Checked = false;
                    chkAnswer_72.Checked = false;
                    if (dt.Rows[0]["MUN7_1"].ToString().Trim() == "Y")
                    {
                        chkAnswer_71.Checked = true;
                    }
                    else
                    {
                        chkAnswer_72.Checked = true;
                    }
                    txtAnswer_7.Text = dt.Rows[0]["MUN7_2"].ToString().Trim();

                    txtAnswer_81.Text = dt.Rows[0]["MUN8_1"].ToString().Trim();
                    txtAnswer_82.Text = dt.Rows[0]["MUN8_2"].ToString().Trim();

                    chkAnswer_91.Checked = false;
                    chkAnswer_92.Checked = false;
                    chkAnswer_93.Checked = false;
                    chkAnswer_94.Checked = false;
                    chkAnswer_95.Checked = false;
                    if (dt.Rows[0]["MUN9_1"].ToString().Trim() == "Y") chkAnswer_91.Checked = true;
                    if (dt.Rows[0]["MUN9_2"].ToString().Trim() == "Y") chkAnswer_92.Checked = true;
                    if (dt.Rows[0]["MUN9_3"].ToString().Trim() == "Y") chkAnswer_93.Checked = true;
                    if (dt.Rows[0]["MUN9_4"].ToString().Trim() == "Y") chkAnswer_94.Checked = true;
                    if (dt.Rows[0]["MUN9_5"].ToString().Trim() == "Y") chkAnswer_95.Checked = true;

                    txtAnswer_91.Text = dt.Rows[0]["MUN9_6"].ToString().Trim();
                    txtAnswer_92.Text = dt.Rows[0]["MUN9_7"].ToString().Trim();

                    txtAnswer_101.Text = dt.Rows[0]["MUN10_1"].ToString().Trim();
                    txtAnswer_102.Text = dt.Rows[0]["MUN10_2"].ToString().Trim();
                    chkAnswer_101.Checked = false;
                    chkAnswer_102.Checked = false;
                    if (dt.Rows[0]["MUN10_3"].ToString().Trim() == "Y")
                    {
                        chkAnswer_101.Checked = true;
                    }
                    else
                    {
                        chkAnswer_102.Checked = true;
                    }

                    txtRemark.Text = dt.Rows[0]["Remark"].ToString().Trim();
                    txtEMRNO.Text = dt.Rows[0]["EMRNO"].ToString().Trim();
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
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
                chkNo_8.Enabled = false;
                chkNo_9.Enabled = false;
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
                chkNo_8.Enabled = true;
                chkNo_9.Enabled = true;
                chkNo_10.Enabled = true;
                chkNo_11.Enabled = true;

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인
            if (btnSaveClick() == true)
            {
                this.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인
            btnChartClick();
        }

        private bool btnSaveClick()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strMun1 = "";
            string strMun2 = "";
            string strMun3_1 = "";
            string strMun3_2 = "";
            string strMun3_3 = "";
            string strMun3_4 = "";
            string strMun4_1 = "";
            string strMun4_2 = "";
            string strMun5 = "";
            string strMun6 = "";
            string strMun7_1 = "";
            string strMun7_2 = "";
            string strMun8_1 = "";
            string strMun8_2 = "";
            string strMun9_1 = "";
            string strMun9_2 = "";
            string strMun9_3 = "";
            string strMun9_4 = "";
            string strMun9_5 = "";
            string strMun9_6 = "";
            string strMun9_7 = "";
            string strMun10_1 = "";
            string strMun10_2 = "";
            string strMun10_3 = "";

            string strRemark = "";
            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                strMun1 = "";
                strMun2 = "";
                strMun3_1 = "N";
                strMun3_2 = "N";
                strMun3_3 = "N";
                strMun3_4 = "";
                strMun4_1 = "N";
                strMun4_2 = "";
                strMun5 = "";
                strMun6 = "";
                strMun7_1 = "N";
                strMun7_2 = "";
                strMun8_1 = "";
                strMun8_2 = "";
                strMun9_1 = "N";
                strMun9_2 = "N";
                strMun9_3 = "N";
                strMun9_4 = "N";
                strMun9_5 = "N";
                strMun9_6 = "";
                strMun9_7 = "";
                strMun10_1 = "";
                strMun10_2 = "";
                strMun10_3 = "N";

                strRemark = "";

                strMun1 = VB.Trim(txtAnswer_1.Text);
                strMun2 = VB.Trim(txtAnswer_2.Text);
                if (chkAnswer_31.Checked == true) strMun3_1 = "Y";
                if (chkAnswer_32.Checked == true) strMun3_2 = "Y";
                if (chkAnswer_33.Checked == true) strMun3_3 = "Y";
                strMun3_4 = VB.Trim(txtAnswer_3.Text);
                if (chkAnswer_41.Checked == true) strMun4_1 = "Y";
                strMun4_2 = VB.Trim(txtAnswer_4.Text);
                strMun5 = VB.Trim(txtAnswer_5.Text);
                strMun6 = VB.Trim(txtAnswer_6.Text);
                if (chkAnswer_71.Checked == true) strMun7_1 = "Y";
                strMun7_2 = VB.Trim(txtAnswer_7.Text);
                strMun8_1 = VB.Trim(txtAnswer_81.Text);
                strMun8_2 = VB.Trim(txtAnswer_82.Text);
                if (chkAnswer_91.Checked == true) strMun9_1 = "Y";
                if (chkAnswer_92.Checked == true) strMun9_2 = "Y";
                if (chkAnswer_93.Checked == true) strMun9_3 = "Y";
                if (chkAnswer_94.Checked == true) strMun9_4 = "Y";
                if (chkAnswer_95.Checked == true) strMun9_5 = "Y";
                strMun9_6 = VB.Trim(txtAnswer_91.Text);
                strMun9_7 = VB.Trim(txtAnswer_92.Text);
                strMun10_1 = VB.Trim(txtAnswer_101.Text);
                strMun10_2 = VB.Trim(txtAnswer_102.Text);
                if (chkAnswer_101.Checked == true) strMun10_3 = "Y";

                strRemark = VB.Trim(txtRemark.Text);

                if (FstrROWID == "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_PMPA.OPD_DEPT_MUNJIN (BDATE , PANO, DEPTCODE, DRCODE, MUN1, MUN2, MUN3_1, MUN3_2, MUN3_3, MUN3_4, MUN4_1, ";
                    SQL = SQL + ComNum.VBLF + " MUN4_2,MUN5,MUN6,MUN7_1,MUN7_2,MUN8_1,MUN8_2,MUN9_1,MUN9_2,MUN9_3,MUN9_4,MUN9_5,";
                    SQL = SQL + ComNum.VBLF + " MUN9_6 , MUN9_7, MUN10_1, MUN10_2, MUN10_3, REMARK, ENTSABUN, ENTDATE ) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + "  TO_DATE('" + FstrBDate + "','YYYY-MM-DD'),'" + FstrPaNo + "','" + FstrDept + "','" + FstrDrCode + "', ";
                    SQL = SQL + ComNum.VBLF + "  '" + strMun1 + "','" + strMun2 + "','" + strMun3_1 + "','" + strMun3_2 + "','" + strMun3_3 + "','" + strMun3_4 + "',";
                    SQL = SQL + ComNum.VBLF + "  '" + strMun4_1 + "','" + strMun4_2 + "','" + strMun5 + "','" + strMun6 + "','" + strMun7_1 + "',";
                    SQL = SQL + ComNum.VBLF + "  '" + strMun7_2 + "','" + strMun8_1 + "','" + strMun8_2 + "','" + strMun9_1 + "','" + strMun9_2 + "','" + strMun9_3 + "',";
                    SQL = SQL + ComNum.VBLF + "  '" + strMun9_4 + "','" + strMun9_5 + "','" + strMun9_6 + "','" + strMun9_7 + "','" + strMun10_1 + "','" + strMun10_2 + "',";
                    SQL = SQL + ComNum.VBLF + "  '" + strMun10_3 + "','" + strRemark + "'," + clsType.User.Sabun + ",SYSDATE ) ";
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " UPDATE KOSMOS_PMPA.OPD_DEPT_MUNJIN SET ";
                    SQL = SQL + ComNum.VBLF + "  MUN1 ='" + strMun1 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN2 ='" + strMun2 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN3_1 ='" + strMun3_1 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN3_2 ='" + strMun3_2 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN3_3 ='" + strMun3_3 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN3_4 ='" + strMun3_4 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN4_1 ='" + strMun4_1 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN4_2 ='" + strMun4_2 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN5 ='" + strMun5 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN6 ='" + strMun6 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN7_1 ='" + strMun7_1 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN7_2 ='" + strMun7_2 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN8_1 ='" + strMun8_1 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN8_2 ='" + strMun8_2 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN9_1 ='" + strMun9_1 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN9_2 ='" + strMun9_2 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN9_3 ='" + strMun9_3 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN9_4 ='" + strMun9_4 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN9_5 ='" + strMun9_5 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN9_6 ='" + strMun9_6 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN9_7 ='" + strMun9_7 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN10_1 ='" + strMun10_1 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN10_2 ='" + strMun10_2 + "', ";
                    SQL = SQL + ComNum.VBLF + "  MUN10_3 ='" + strMun10_3 + "', ";
                    SQL = SQL + ComNum.VBLF + "  Remark ='" + strRemark + "', ";
                    SQL = SQL + ComNum.VBLF + "  ENTSABUN =" + clsType.User.Sabun + ", ";
                    SQL = SQL + ComNum.VBLF + "  ENTDATE =SYSDATE  ";
                    SQL = SQL + ComNum.VBLF + "   WHERE ROWID ='" + FstrROWID + "' ";
                }

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
                ComFunc.MsgBox("저장 하였습니다.");
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

        private void btnChartClick()
        {
            if (clsType.User.JobMan == "간호사")
            {
                ComFunc.MsgBox("간호사일 경우 경과기록을 작성할 수 없습니다.");
                return;
            }

            DATA_BUILD();
            if (txtSendData.Text != "")
            {
                EmrPatient pAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, FstrPaNo, "O", FstrBDate.Replace("-", ""), FstrDept);
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
                //    if (clsType.User.DeptCode.Equals("NP") || clsType.User.DeptCode.Equals("DM") || clsType.User.DeptCode.Equals("OG"))
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

        private void DATA_BUILD()
        {
            //int i = 0;
            string strData = "";
            string strSUB = "";

            if (chkNo_1.Checked == true)
            {
                if (VB.Trim(txtAnswer_1.Text) != "")
                {
                    strData = strData + ComNum.VBLF + " " + VB.Trim(txtAnswer_1.Text);
                    strData = strData + ComNum.VBLF;
                }
                else
                {
                    chkNo_1.Checked = false;
                }
            }


            if (chkNo_2.Checked == true)
            {
                if (VB.Trim(txtAnswer_2.Text) != "")
                {
                    strData = strData + ComNum.VBLF + " " + VB.Trim(txtAnswer_2.Text);
                    strData = strData + ComNum.VBLF;
                }
                else
                {
                    chkNo_2.Checked = false;
                }
            }
            

            strSUB = "";
            if (chkNo_3.Checked == true)
            {
                if (chkAnswer_91.Checked == true) strSUB = " 고혈압";
                if (chkAnswer_92.Checked == true) strSUB = strSUB + " 폐질환/결핵/천식";
                if (chkAnswer_93.Checked == true) strSUB = strSUB + " 당뇨";
                if (chkAnswer_94.Checked == true) strSUB = strSUB + " 협심증/심근경색";
                if (chkAnswer_95.Checked == true) strSUB = strSUB + " 간질환";
                if (VB.Trim(txtAnswer_91.Text) != "") strSUB = strSUB + " 기타 : " + VB.Trim(txtAnswer_91.Text);

                if (VB.Trim(txtAnswer_92.Text) != "")
                {
                    strSUB = strSUB + ComNum.VBLF + " " + VB.Trim(txtAnswer_92.Text);
                }

                if (VB.Trim(strSUB) != "")
                {
                    strData = strData + ComNum.VBLF + " " + VB.Trim(strSUB);
                    strData = strData + ComNum.VBLF;
                }
                else
                {
                    chkNo_3.Checked = false;
                }
            }

            strSUB = "";
            if (chkNo_4.Checked == true)
            {
                if (VB.Trim(txtAnswer_101.Text) != "") strSUB = " 복용약 : " + VB.Trim(txtAnswer_101.Text);
                if (VB.Trim(txtAnswer_102.Text) != "") strSUB = strSUB + ComNum.VBLF + " 언제부터 : " + VB.Trim(txtAnswer_102.Text);
                if (chkAnswer_101.Checked == true) strSUB = strSUB + ComNum.VBLF + " 식별 : 예";
                if (chkAnswer_102.Checked == true) strSUB = strSUB + ComNum.VBLF + " 식별 : 아니요";
                
                if (VB.Trim(strSUB) != "")
                {
                    strData = strData + ComNum.VBLF + " " + VB.Trim(strSUB);
                    strData = strData + ComNum.VBLF;
                }
                else
                {
                    chkNo_4.Checked = false;
                }        
            }

            strSUB = "";
            if (chkNo_5.Checked == true)
            {
                if (chkAnswer_31.Checked == true) strSUB = strSUB + " 소견서";
                if (chkAnswer_32.Checked == true) strSUB = strSUB + " 진료의뢰서";
                if (chkAnswer_33.Checked == true) strSUB = strSUB + " CD";
                if (VB.Trim(txtAnswer_3.Text) != "") strSUB = strSUB + " 기타 : " + VB.Trim(txtAnswer_101.Text);

                if (VB.Trim(strSUB) != "")
                {
                    strData = strData + ComNum.VBLF + " " + VB.Trim(strSUB);
                    strData = strData + ComNum.VBLF;
                }
                else
                {
                    chkNo_5.Checked = false;
                }
            }
            
            strSUB = "";
            if (chkNo_6.Checked == true)
            {
                if (chkAnswer_41.Checked == true) strSUB = strSUB + " 예";
                if (chkAnswer_42.Checked == true) strSUB = strSUB + " 아니요";
                if (VB.Trim(txtAnswer_4.Text) != "") strSUB = strSUB + ComNum.VBLF + "  - 검사종류 : " + VB.Trim(txtAnswer_4.Text);

                if (VB.Trim(strSUB) != "")
                {
                    strData = strData + ComNum.VBLF + " " + VB.Trim(strSUB);
                    strData = strData + ComNum.VBLF;
                }
                else
                {
                    chkNo_6.Checked = false;
                }
            }
            
            if (chkNo_7.Checked == true)
            {
                if (VB.Trim(txtAnswer_5.Text) != "")
                {
                    strData = strData + ComNum.VBLF + " " + VB.Trim(txtAnswer_5.Text);
                    strData = strData + ComNum.VBLF;
                }
                else
                {
                    chkNo_7.Checked = false;
                }                
            }

            if (chkNo_8.Checked == true)
            {
                if (VB.Trim(txtAnswer_6.Text) != "")
                {
                    strData = strData + ComNum.VBLF + " " + VB.Trim(txtAnswer_6.Text);
                    strData = strData + ComNum.VBLF;
                }
                else
                {
                    chkNo_8.Checked = false;
                }
            }

            strSUB = "";
            if (chkNo_9.Checked == true)
            {
                if (chkAnswer_71.Checked == true) strSUB = strSUB + " 예";
                if (chkAnswer_72.Checked == true) strSUB = strSUB + " 아니오";
                if (VB.Trim(txtAnswer_7.Text) != "") strSUB = strSUB + ComNum.VBLF + "  - 내시경 결과 : " + VB.Trim(txtAnswer_7.Text);

                if (VB.Trim(strSUB) != "")
                {
                    strData = strData + ComNum.VBLF + " " + VB.Trim(strSUB);
                    strData = strData + ComNum.VBLF;
                }
                else
                {
                    chkNo_9.Checked = false;
                }
            }

            strSUB = "";
            if (chkNo_10.Checked == true)
            {
                if (VB.Trim(txtAnswer_81.Text) != "") strSUB = strSUB + " 장소 : " + VB.Trim(txtAnswer_81.Text);
                if (VB.Trim(txtAnswer_82.Text) != "") strSUB = strSUB + ComNum.VBLF + "  - 언제 : " + VB.Trim(txtAnswer_82.Text);

                if (VB.Trim(strSUB) != "")
                {
                    strData = strData + ComNum.VBLF + " " + VB.Trim(strSUB);
                    strData = strData + ComNum.VBLF;
                }
                else
                {
                    chkNo_10.Checked = false;
                }
            }


            if (chkNo_11.Checked == true)
            {
                if (VB.Trim(txtRemark.Text) != "")
                {
                    strData = strData + ComNum.VBLF + " ▣ 참고사항 ";
                    strData = strData + ComNum.VBLF + VB.Trim(txtRemark.Text);
                }
                else
                {
                    chkNo_11.Checked = false;
                }
            }

            txtSendData.Text = strData;
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
                    clsDB.setRollbackTran(clsDB.DbCon);
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
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();
                }
                
                if (clsNurse.CREATE_EMR_XMLINSRT3(dblEmrNo, "963", clsType.User.IdNumber,
                strChartDate, strChartTime, 0, FstrPaNo, "O", VB.Replace(FstrBDate, "-", ""), "120000",
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
                SQL = SQL + ComNum.VBLF + " , CHK = 'Y' ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + FstrPaNo + "' ";
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
    }
}
