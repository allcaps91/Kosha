using ComBase; //기본 클래스
using ComEmrBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmMedicalInquiry_MN.cs
    /// Description     : 진료예진표_신장내과
    /// Author          : 유진호
    /// Create Date     : 2018-01-09
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\emr\emrprt\Frm진료예진표_MN
    /// </history>
    /// </summary>
    public partial class frmMedicalInquiry_MN : Form
    {
        ComFunc CF = new ComFunc();
        private string FstrPaNo = "";
        private string FstrPaName = "";
        private string FstrDept = "";
        private string FstrDrCode = "";
        private string FstrBDate = "";

        private string FstrROWID = "";

        public frmMedicalInquiry_MN()
        {
            InitializeComponent();
        }

        public frmMedicalInquiry_MN(string strPaNo, string strPaName, string strDept, string strDrCode, string strBDate)
        {
            InitializeComponent();
            this.FstrPaNo = strPaNo;
            this.FstrPaName = strPaName;
            this.FstrDept = strDept;
            this.FstrDrCode = strDrCode;
            this.FstrBDate = strBDate;
        }

        private void frmMedicalInquiry_MN_Load(object sender, EventArgs e)
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
                btnAll.Enabled = false;
            }
            else
            {
                btnChart.Enabled = true;
                btnAll.Enabled = true;
            }

            SCREEN_CLEAR();
            SET_Info();
            btnSearchClick();
        }

        private void SCREEN_CLEAR()
        {
            //int i = 0;

            txtHeight.Text = "";
            txtWeight.Text = "";

            //'1
            txt1.Text = "";

            //'2

            //'3

            //'4
            chk4_1.Checked = false;
            chk4_2.Checked = false;
            txtOs4.Text = "";

            //'5

            //'6
            chkOs61.Checked = false;
            chkOs62.Checked = false;

            //'7
            chk2_1N.Checked = false;
            chk2_1Y.Checked = false;
            chk2_1.Checked = false; txt2_1.Text = "";
            chk2_2.Checked = false; txt2_2.Text = "";
            chk2_3.Checked = false; txt2_3.Text = "";
            chk2_4.Checked = false; txt2_4.Text = "";
            chk2_5.Checked = false; txt2_5.Text = "";
            chk2_6.Checked = false; txt2_6.Text = "";
            chk2_7.Checked = false; txt2_7.Text = "";
            chk2_8.Checked = false; txt2_8.Text = "";
            chk2_9.Checked = false; txt2_9.Text = "";
            chk2_10.Checked = false; txt2_10.Text = ""; //'기타               
            chk2_11.Checked = false; txt2_11.Text = ""; //'갑상선
            chk2_12.Checked = false; txt2_12.Text = ""; //'골다공증

            //'8            
            chk8_Y.Checked = false; chk8_N.Checked = true;
            chk8_1.Checked = false; chk8_2.Checked = false; chk8_3.Checked = false;
            chk8_4.Checked = false; chk8_5.Checked = false; chk8_6.Checked = false;
            chk8_7.Checked = false;
            txt8_7_1.Text = "";

            //'9

            //'10
            chk10_1.Checked = false;
            chk10_2.Checked = false; txt10_2_1.Text = "";
            txt10_3_1.Text = ""; txt10_3_2.Text = ""; txt10_3_3.Text = ""; txt10_3_4.Text = "";
            txt10_4_1.Text = ""; txt10_4_2.Text = ""; txt10_4_3.Text = ""; txt10_4_4.Text = ""; txt10_4_5.Text = "";
            chk10_5_1.Checked = false; chk10_5_2.Checked = false; chk10_5_3.Checked = false;
            chk10_5_4.Checked = false; chk10_5_5.Checked = false; chk10_5_6.Checked = false;
            chk10_5_7.Checked = false; chk10_5_8.Checked = false; chk10_5_9.Checked = false;
            chk10_5_10.Checked = false; chk10_5_11.Checked = false;
            txt10_5_11_1.Text = "";
            txt11_Fa.Text = "";

            //'12 해외여행력
            chk12_1.Checked = false; chk12_2.Checked = false;

            txtRemark.Text = "";
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
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            btnSearchClick();
        }

        private void btnSearchClick()
        {
            SCREEN_CLEAR();

            READ_Munjin_Data("1", FstrPaNo, FstrDept, FstrBDate);
            READ_Munjin_Dept(FstrPaNo, FstrDept, FstrBDate);    //'당일접수
            DATA_BULID_PRE();
        }

        private void READ_Munjin_Data(string argSTS, string strPaNo, string strDept, string strBDate)
        {

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

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
                SQL = SQL + ComNum.VBLF + " TMUN10_5_10,TMUN10_5_11,TMUN10_5_11_1,TMUN11_F1,TMUN12_T1,";
                SQL = SQL + ComNum.VBLF + " OG_MUN2,OG_MUN3,OG_MUN3_1_1,";
                SQL = SQL + ComNum.VBLF + " OG_MUN3_1_2,OG_MUN3_2,OG_MUN3_3,OG_MUN3_4,OG_MUN3_5,OG_MUN3_6,OG_MUN3_6_1,";
                SQL = SQL + ComNum.VBLF + " OG_MUN3_7,OG_MUN3_8,OG_MUN4_1,OG_MUN4_2,OG_MUN4_3,OG_MUN5,OG_MUN6,OG_MUN7_1,";
                SQL = SQL + ComNum.VBLF + " OG_MUN7_2,OG_MUN7_3,OG_MUN7_4,OG_MUN7_4_1,OG_MUN8,OG_MUN9_1_1,OG_MUN9_1_2,";
                SQL = SQL + ComNum.VBLF + " OG_MUN9_2 , OG_MUN9_3, ";

                //'ADD;
                SQL = SQL + ComNum.VBLF + " TMUN2_12,TMUN2_12_1,TMUN2_13,TMUN2_13_1,TMUN8_6,TMUN8_7,    ";
                SQL = SQL + ComNum.VBLF + " REMARK, ENTSABUN, ENTDATE,ROWID, EMRNO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_DEPT_MUNJIN ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPaNo + "' ";
                if (argSTS == "1" || argSTS == "2")
                {
                    SQL = SQL + "  AND DEPTCODE ='" + strDept + "' ";
                }
                else
                {
                    if (Convert.ToDateTime(strBDate) >= Convert.ToDateTime("2015-09-08"))
                    {
                        //'통합 최초과 저장시 만 과저장됨 , 정신과제외;
                        SQL = SQL + "  AND DEPTCODE <> 'NP' ";
                    }
                    else
                    {
                        SQL = SQL + "  AND DEPTCODE ='" + strDept + "' ";
                    }
                }
                SQL = SQL + "  AND BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                SQL = SQL + "  AND (DELDATE IS NULL OR DELDATE ='')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("등록된 예진표자료가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                FstrROWID = "";

                if (dt.Rows.Count > 0)
                {
                    if (argSTS != "2")
                    {
                        FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    }

                    txtHeight.Text = dt.Rows[0]["TMUN_H"].ToString().Trim();
                    txtWeight.Text = dt.Rows[0]["TMUN_W"].ToString().Trim();

                    //'1
                    txt1.Text = dt.Rows[0]["TMUN1"].ToString().Trim();

                    //'2

                    //'3

                    //'4

                    chk4_1.Checked = false;
                    if (dt.Rows[0]["TMUN4_1"].ToString().Trim() == "Y") chk4_1.Checked = true;
                    chk4_2.Checked = false;
                    if (dt.Rows[0]["TMUN4_2"].ToString().Trim() == "Y") chk4_2.Checked = true;

                    txtOs4.Text = dt.Rows[0]["TMUN4_2_5_1"].ToString().Trim();

                    //'5

                    //'6
                    if (dt.Rows[0]["TMUN5_1"].ToString().Trim() == "Y")
                    {
                        chkOs62.Checked = true;
                    }
                    else if (dt.Rows[0]["TMUN5_1"].ToString().Trim() == "N")
                    {
                        chkOs61.Checked = true;
                    }

                    //'7
                    chk2_1N.Checked = false;
                    if (dt.Rows[0]["TMUN2N"].ToString().Trim() == "Y") chk2_1N.Checked = true;
                    chk2_1Y.Checked = false;
                    if (dt.Rows[0]["TMUN2Y"].ToString().Trim() == "Y") chk2_1Y.Checked = true;
                    chk2_1.Checked = false;
                    if (dt.Rows[0]["TMUN2_1"].ToString().Trim() == "Y") chk2_1.Checked = true;
                    txt2_1.Text = dt.Rows[0]["TMUN2_1_1"].ToString().Trim();
                    chk2_2.Checked = false;
                    if (dt.Rows[0]["TMUN2_2"].ToString().Trim() == "Y") chk2_2.Checked = true;
                    txt2_2.Text = dt.Rows[0]["TMUN2_2_1"].ToString().Trim();
                    chk2_3.Checked = false;
                    if (dt.Rows[0]["TMUN2_3"].ToString().Trim() == "Y") chk2_3.Checked = true;
                    txt2_3.Text = dt.Rows[0]["TMUN2_3_1"].ToString().Trim();
                    chk2_4.Checked = false;
                    if (dt.Rows[0]["TMUN2_4"].ToString().Trim() == "Y") chk2_4.Checked = true;
                    txt2_4.Text = dt.Rows[0]["TMUN2_4_1"].ToString().Trim();
                    chk2_5.Checked = false;
                    if (dt.Rows[0]["TMUN2_5"].ToString().Trim() == "Y") chk2_5.Checked = true;
                    txt2_5.Text = dt.Rows[0]["TMUN2_5_1"].ToString().Trim();
                    chk2_6.Checked = false;
                    if (dt.Rows[0]["TMUN2_6"].ToString().Trim() == "Y") chk2_6.Checked = true;
                    txt2_6.Text = dt.Rows[0]["TMUN2_6_1"].ToString().Trim();
                    chk2_7.Checked = false;
                    if (dt.Rows[0]["TMUN2_7"].ToString().Trim() == "Y") chk2_7.Checked = true;
                    txt2_7.Text = dt.Rows[0]["TMUN2_7_1"].ToString().Trim();
                    chk2_8.Checked = false;
                    if (dt.Rows[0]["TMUN2_8"].ToString().Trim() == "Y") chk2_8.Checked = true;
                    txt2_8.Text = dt.Rows[0]["TMUN2_8_1"].ToString().Trim();
                    chk2_9.Checked = false;
                    if (dt.Rows[0]["TMUN2_9"].ToString().Trim() == "Y") chk2_9.Checked = true;
                    txt2_9.Text = dt.Rows[0]["TMUN2_9_1"].ToString().Trim();
                    chk2_10.Checked = false;
                    if (dt.Rows[0]["TMUN2_10"].ToString().Trim() == "Y") chk2_10.Checked = true;
                    txt2_10.Text = dt.Rows[0]["TMUN2_10_1"].ToString().Trim();      //'기타
                    chk2_11.Checked = false;
                    if (dt.Rows[0]["TMUN2_12"].ToString().Trim() == "Y") chk2_11.Checked = true;
                    txt2_11.Text = dt.Rows[0]["TMUN2_12_1"].ToString().Trim();      //'갑상선
                    chk2_12.Checked = false;
                    if (dt.Rows[0]["TMUN2_13"].ToString().Trim() == "Y") chk2_12.Checked = true;
                    txt2_12.Text = dt.Rows[0]["TMUN2_13_1"].ToString().Trim();      //'골다공

                    //'8
                    chk8_N.Checked = true;

                    chk8_1.Checked = false;
                    if (dt.Rows[0]["TMUN8_1"].ToString().Trim() == "Y")
                    {
                        chk8_1.Checked = true;
                        chk8_Y.Checked = true;
                        chk8_N.Checked = false;
                    }
                    chk8_2.Checked = false;
                    if (dt.Rows[0]["TMUN8_2"].ToString().Trim() == "Y")
                    {
                        chk8_2.Checked = true;
                        chk8_Y.Checked = true;
                        chk8_N.Checked = false;
                    }
                    chk8_3.Checked = false;
                    if (dt.Rows[0]["TMUN8_3"].ToString().Trim() == "Y")
                    {
                        chk8_3.Checked = true;
                        chk8_Y.Checked = true;
                        chk8_N.Checked = false;
                    }
                    chk8_4.Checked = false;
                    if (dt.Rows[0]["TMUN8_4"].ToString().Trim() == "Y")
                    {
                        chk8_4.Checked = true;
                        chk8_Y.Checked = true;
                        chk8_N.Checked = false;
                    }
                    chk8_5.Checked = false;  //'진단서
                    if (dt.Rows[0]["TMUN8_6"].ToString().Trim() == "Y")
                    {
                        chk8_5.Checked = true;
                        chk8_Y.Checked = true;
                        chk8_N.Checked = false;
                    }
                    chk8_6.Checked = false;  //'의무기록사본
                    if (dt.Rows[0]["TMUN8_7"].ToString().Trim() == "Y")
                    {
                        chk8_6.Checked = true;
                        chk8_Y.Checked = true;
                        chk8_N.Checked = false;
                    }

                    //'기타
                    chk8_7.Checked = false;
                    if (dt.Rows[0]["TMUN8_5"].ToString().Trim() == "Y")
                    {
                        chk8_7.Checked = true;
                        chk8_Y.Checked = true;
                        chk8_N.Checked = false;
                    }
                    txt8_7_1.Text = dt.Rows[0]["TMUN8_5_1"].ToString().Trim();

                    //'9

                    //'10
                    chk10_1.Checked = false;
                    if (dt.Rows[0]["TMUN10_1"].ToString().Trim() == "Y") chk10_1.Checked = true;
                    chk10_2.Checked = false;
                    if (dt.Rows[0]["TMUN10_2"].ToString().Trim() == "Y") chk10_2.Checked = true;
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

                    chk10_5_1.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_1"].ToString().Trim() == "Y") chk10_5_1.Checked = true;
                    chk10_5_2.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_2"].ToString().Trim() == "Y") chk10_5_2.Checked = true;
                    chk10_5_3.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_3"].ToString().Trim() == "Y") chk10_5_3.Checked = true;
                    chk10_5_4.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_4"].ToString().Trim() == "Y") chk10_5_4.Checked = true;
                    chk10_5_5.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_5"].ToString().Trim() == "Y") chk10_5_5.Checked = true;
                    chk10_5_6.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_6"].ToString().Trim() == "Y") chk10_5_6.Checked = true;
                    chk10_5_7.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_7"].ToString().Trim() == "Y") chk10_5_7.Checked = true;
                    chk10_5_8.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_8"].ToString().Trim() == "Y") chk10_5_8.Checked = true;
                    chk10_5_9.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_9"].ToString().Trim() == "Y") chk10_5_9.Checked = true;
                    chk10_5_10.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_10"].ToString().Trim() == "Y") chk10_5_10.Checked = true;
                    chk10_5_11.Checked = false;
                    if (dt.Rows[0]["TMUN10_5_11"].ToString().Trim() == "Y") chk10_5_11.Checked = true;
                    txt10_5_11_1.Text = dt.Rows[0]["TMUN10_5_11_1"].ToString().Trim();

                    txt11_Fa.Text = dt.Rows[0]["tmun11_f1"].ToString().Trim();

                    if (dt.Rows[0]["TMUN12_T1"].ToString().Trim() == "N")
                    {
                        chk12_1.Checked = true;
                    }
                    else if (dt.Rows[0]["TMUN12_T1"].ToString().Trim() == "Y")
                    {
                        chk12_2.Checked = true;
                    }

                    txtRemark.Text = dt.Rows[0]["Remark"].ToString().Trim();
                    txtEMRNO.Text = dt.Rows[0]["EMRNO"].ToString().Trim();

                    ComFunc.MsgBox(dt.Rows[0]["DeptCode"].ToString().Trim() + "과 에서 " + strBDate + "에 등록된 자료입니다..");
                }
                else
                {
                    ComFunc.MsgBox("등록된 예진표자료가 없습니다.." + ComNum.VBLF + ComNum.VBLF + "타과 내역을 불러오실려면 과코드를 더블클릭하세요");
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

        private void READ_Munjin_Dept(string strPaNo, string strDept, string strBDate)
        {

            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DEPTCODE,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_MASTER  ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPaNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssDept_Sheet1.Cells[0, i].Text = dt.Rows[i]["DeptCode"].ToString().Trim();

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT DEPTCODE,BDATE ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_DEPT_MUNJIN B ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPaNo + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND DEPTCODE = '" + dt.Rows[i]["DeptCode"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='')";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (dt2.Rows.Count > 0)
                        {
                            ssDept_Sheet1.Cells[0, i].BackColor = Color.FromArgb(128, 255, 128);
                        }
                        else
                        {
                            ssDept_Sheet1.Cells[0, i].BackColor = Color.FromArgb(255, 255, 255);
                        }

                        dt2.Dispose();
                        dt2 = null;
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

        private bool btnChartClick()
        {
            bool rtnVal = false;
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
            return rtnVal;
        }

        private bool btnSaveClick()
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strH = "";    //'키
            string strW = "";   //'몸무게

            string strMun1 = "";
            string strMun4 = ""; //'알러지
            string strMun4_1 = "";
            string strMun4_2 = "";
            string strMun6 = ""; //'임신

            //'지금항목 7
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
            string strMun2_11 = "";
            string strMun2_11_1 = "";
            string strMun2_12 = "";
            string strMun2_12_1 = "";
            string strMun8_1 = "";
            string strMun8_2 = "";
            string strMun8_3 = "";
            string strMun8_4 = "";
            string strMun8_5 = "";
            string strMun8_6 = "";
            string strMun8_7 = "";
            string strMun8_7_1 = "";
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


            strH = VB.Trim(txtHeight.Text);
            strW = VB.Trim(txtWeight.Text);

            //'1
            strMun1 = VB.Trim(txt1.Text);

            //'2

            //'3

            //'4
            strMun4 = VB.Trim(txtOs4.Text);

            strMun4_1 = "N";
            if (chk4_1.Checked == true) strMun4_1 = "Y";
            strMun4_2 = "N";
            if (chk4_2.Checked == true) strMun4_2 = "Y";

            //'5

            //'6
            strMun6 = "N";
            if (chkOs62.Checked == true) strMun6 = "Y";

            //'7
            strMun2N = "N";
            if (chk2_1N.Checked == true) strMun2N = "Y";
            strMun2Y = "N";
            if (chk2_1Y.Checked == true) strMun2Y = "Y";


            strMun2_1 = "N";
            if (chk2_1.Checked == true) strMun2_1 = "Y";
            strMun2_1_1 = VB.Trim(txt2_1.Text);
            strMun2_2 = "N";
            if (chk2_2.Checked == true) strMun2_2 = "Y";
            strMun2_2_1 = VB.Trim(txt2_2.Text);
            strMun2_3 = "N";
            if (chk2_3.Checked == true) strMun2_3 = "Y";
            strMun2_3_1 = VB.Trim(txt2_3.Text);
            strMun2_4 = "N";
            if (chk2_4.Checked == true) strMun2_4 = "Y";
            strMun2_4_1 = VB.Trim(txt2_4.Text);
            strMun2_5 = "N";
            if (chk2_5.Checked == true) strMun2_5 = "Y";
            strMun2_5_1 = VB.Trim(txt2_5.Text);
            strMun2_6 = "N";
            if (chk2_6.Checked == true) strMun2_6 = "Y";
            strMun2_6_1 = VB.Trim(txt2_6.Text);
            strMun2_7 = "N";
            if (chk2_7.Checked == true) strMun2_7 = "Y";
            strMun2_7_1 = VB.Trim(txt2_7.Text);
            strMun2_8 = "N";
            if (chk2_8.Checked == true) strMun2_8 = "Y";
            strMun2_8_1 = VB.Trim(txt2_8.Text);
            strMun2_9 = "N";
            if (chk2_9.Checked == true) strMun2_9 = "Y";
            strMun2_9_1 = VB.Trim(txt2_9.Text);
            strMun2_10 = "N"; //'기타
            if (chk2_10.Checked == true) strMun2_10 = "Y";
            strMun2_10_1 = VB.Trim(txt2_10.Text);

            strMun2_11 = "N";
            if (chk2_11.Checked == true) strMun2_11 = "Y";
            strMun2_11_1 = VB.Trim(txt2_11.Text);
            strMun2_12 = "N";
            if (chk2_12.Checked == true) strMun2_12 = "Y";
            strMun2_12_1 = VB.Trim(txt2_12.Text);

            //'8
            strMun8_1 = "N";
            if (chk8_1.Checked == true) strMun8_1 = "Y";
            strMun8_2 = "N";
            if (chk8_2.Checked == true) strMun8_2 = "Y";
            strMun8_3 = "N";
            if (chk8_3.Checked == true) strMun8_3 = "Y";
            strMun8_4 = "N";
            if (chk8_4.Checked == true) strMun8_4 = "Y";
            strMun8_5 = "N";
            if (chk8_5.Checked == true) strMun8_5 = "Y";
            strMun8_6 = "N";
            if (chk8_6.Checked == true) strMun8_6 = "Y";

            strMun8_7 = "N";
            if (chk8_7.Checked == true) strMun8_7 = "Y";
            strMun8_7_1 = VB.Trim(txt8_7_1.Text);

            //'9

            //'10
            strMun10_1 = "N";
            if (chk10_1.Checked == true) strMun10_1 = "Y";
            strMun10_2 = "N";
            if (chk10_2.Checked == true) strMun10_2 = "Y";
            strMun10_2_1 = VB.Trim(txt10_2_1.Text);
            strMun10_3_1 = VB.Trim(txt10_3_1.Text);
            strMun10_3_2 = VB.Trim(txt10_3_2.Text);
            strMun10_3_3 = VB.Trim(txt10_3_3.Text);
            strMun10_3_4 = VB.Trim(txt10_3_4.Text);
            strMun10_4_1 = VB.Trim(txt10_4_1.Text);
            strMun10_4_2 = VB.Trim(txt10_4_2.Text);
            strMun10_4_3 = VB.Trim(txt10_4_3.Text);
            strMun10_4_4 = VB.Trim(txt10_4_4.Text);
            strMun10_4_5 = VB.Trim(txt10_4_5.Text);

            if (strMun10_4_1 != "" && VB.Val(strMun10_4_1) >= VB.Val("3"))
            {
                ComFunc.MsgBox("0~2 까지 값만 가능합니다..");
                return rtVal;
            }

            if (strMun10_4_2 != "" && VB.Val(strMun10_4_2) >= VB.Val("3"))
            {
                ComFunc.MsgBox("0~2 까지 값만 가능합니다..");
                return rtVal;
            }
            if (strMun10_4_3 != "" && VB.Val(strMun10_4_3) >= VB.Val("3"))
            {
                ComFunc.MsgBox("0~2 까지 값만 가능합니다..");
                return rtVal;
            }
            if (strMun10_4_4 != "" && VB.Val(strMun10_4_4) >= VB.Val("3"))
            {
                ComFunc.MsgBox("0~2 까지 값만 가능합니다..");
                return rtVal;
            }
            if (strMun10_4_5 != "" && VB.Val(strMun10_4_5) >= VB.Val("3"))
            {
                ComFunc.MsgBox("0~2 까지 값만 가능합니다..");
                return rtVal;
            }


            strMun10_5_1 = "N";
            if (chk10_5_1.Checked == true) strMun10_5_1 = "Y";
            strMun10_5_2 = "N";
            if (chk10_5_2.Checked == true) strMun10_5_2 = "Y";
            strMun10_5_3 = "N";
            if (chk10_5_3.Checked == true) strMun10_5_3 = "Y";
            strMun10_5_4 = "N";
            if (chk10_5_4.Checked == true) strMun10_5_4 = "Y";
            strMun10_5_5 = "N";
            if (chk10_5_5.Checked == true) strMun10_5_5 = "Y";
            strMun10_5_6 = "N";
            if (chk10_5_6.Checked == true) strMun10_5_6 = "Y";
            strMun10_5_7 = "N";
            if (chk10_5_7.Checked == true) strMun10_5_7 = "Y";
            strMun10_5_8 = "N";
            if (chk10_5_8.Checked == true) strMun10_5_8 = "Y";
            strMun10_5_9 = "N";
            if (chk10_5_9.Checked == true) strMun10_5_9 = "Y";
            strMun10_5_10 = "N";
            if (chk10_5_10.Checked == true) strMun10_5_10 = "Y";
            strMun10_5_11 = "N";
            if (chk10_5_11.Checked == true) strMun10_5_11 = "Y";
            strMun10_5_11_1 = VB.Trim(txt10_5_11_1.Text);

            strMun11_1 = VB.Trim(txt11_Fa.Text);

            if (chk12_1.Checked == true)
            {
                strMun12_1 = "N";
            }
            else if (chk12_2.Checked == true)
            {
                strMun12_1 = "Y";
            }

            strRemark = VB.Trim(txtRemark.Text);


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_DEPT_MUNJIN ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + FstrPaNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DEPTCODE ='" + FstrDept + "' ";
                SQL = SQL + ComNum.VBLF + "  AND BDATE =TO_DATE('" + FstrBDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='')";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }
                else
                {
                    FstrROWID = "";
                }
                dt.Dispose();
                dt = null;

                if (FstrROWID == "")
                {

                    SQL = " INSERT INTO KOSMOS_PMPA.OPD_DEPT_MUNJIN (BDate , Pano, DeptCode, DrCode,TMUN_H, TMUN_W, ";
                    SQL = SQL + " TMUN1,";

                    //'--add
                    SQL = SQL + " TMUN4_1,TMUN4_2,TMUN4_2_5_1,TMUN5_1, ";
                    SQL = SQL + " TMUN2Y,TMUN2N, ";
                    SQL = SQL + " TMUN2_1,TMUN2_1_1,TMUN2_2,TMUN2_2_1,TMUN2_3 ,TMUN2_3_1,TMUN2_4 ,TMUN2_4_1,TMUN2_5 ,TMUN2_5_1,TMUN2_6,TMUN2_6_1, ";
                    SQL = SQL + " TMUN2_7,TMUN2_7_1,TMUN2_8 ,TMUN2_8_1,TMUN2_9 ,TMUN2_9_1,TMUN2_10 ,TMUN2_10_1,TMUN2_12 ,TMUN2_12_1,TMUN2_13 ,TMUN2_13_1, ";
                    SQL = SQL + " TMUN8_1,TMUN8_2,TMUN8_3,TMUN8_4,TMUN8_5,TMUN8_5_1,TMUN8_6,TMUN8_7,";

                    //'--add
                    SQL = SQL + " TMUN10_1,TMUN10_2,TMUN10_2_1,TMUN10_3_1,TMUN10_3_2,TMUN10_3_3,TMUN10_3_4, ";
                    SQL = SQL + " TMUN10_4_1,TMUN10_4_2,TMUN10_4_3,TMUN10_4_4,TMUN10_4_5,TMUN10_5_1,TMUN10_5_2, ";
                    SQL = SQL + " TMUN10_5_3,TMUN10_5_4,TMUN10_5_5,TMUN10_5_6,TMUN10_5_7,TMUN10_5_8,TMUN10_5_9, ";
                    SQL = SQL + " TMUN10_5_10,TMUN10_5_11,TMUN10_5_11_1,tmun11_f1,tmun12_t1,";
                    SQL = SQL + " Remark, ENTSABUN, ENTDATE ) VALUES ( ";
                    SQL = SQL + " TO_DATE('" + FstrBDate + "','YYYY-MM-DD'),'" + FstrPaNo + "','" + FstrDept + "','" + FstrDrCode + "','" + strH + "','" + strW + "', ";
                    SQL = SQL + " '" + strMun1 + "', ";

                    //'--add
                    SQL = SQL + " '" + strMun4_1 + "','" + strMun4_2 + "','" + strMun4 + "','" + strMun6 + "', ";
                    SQL = SQL + " '" + strMun2Y + "', '" + strMun2N + "', ";
                    SQL = SQL + " '" + strMun2_1 + "', '" + strMun2_1_1 + "', '" + strMun2_2 + "',";
                    SQL = SQL + " '" + strMun2_2_1 + "', '" + strMun2_3 + "', '" + strMun2_3_1 + "',";
                    SQL = SQL + " '" + strMun2_4 + "', '" + strMun2_4_1 + "', '" + strMun2_5 + "',";
                    SQL = SQL + " '" + strMun2_5_1 + "', '" + strMun2_6 + "', '" + strMun2_6_1 + "',";
                    SQL = SQL + " '" + strMun2_7 + "', '" + strMun2_7_1 + "', '" + strMun2_8 + "',";
                    SQL = SQL + " '" + strMun2_8_1 + "', '" + strMun2_9 + "', '" + strMun2_9_1 + "',";
                    SQL = SQL + " '" + strMun2_10 + "', '" + strMun2_10_1 + "',";
                    SQL = SQL + " '" + strMun2_11 + "', '" + strMun2_11_1 + "','" + strMun2_12 + "', '" + strMun2_12_1 + "',  ";
                    SQL = SQL + " '" + strMun8_1 + "', '" + strMun8_2 + "', '" + strMun8_3 + "',";
                    SQL = SQL + " '" + strMun8_4 + "',  '" + strMun8_7 + "','" + strMun8_7_1 + "','" + strMun8_5 + "','" + strMun8_6 + "',";

                    //'--add
                    SQL = SQL + " '" + strMun10_1 + "', '" + strMun10_2 + "', '" + strMun10_2_1 + "',";
                    SQL = SQL + " '" + strMun10_3_1 + "', '" + strMun10_3_2 + "', '" + strMun10_3_3 + "',";
                    SQL = SQL + " '" + strMun10_3_4 + "', '" + strMun10_4_1 + "', '" + strMun10_4_2 + "',";
                    SQL = SQL + " '" + strMun10_4_3 + "', '" + strMun10_4_4 + "', '" + strMun10_4_5 + "',";
                    SQL = SQL + " '" + strMun10_5_1 + "', '" + strMun10_5_2 + "', '" + strMun10_5_3 + "',";
                    SQL = SQL + " '" + strMun10_5_4 + "', '" + strMun10_5_5 + "', '" + strMun10_5_6 + "',";
                    SQL = SQL + " '" + strMun10_5_7 + "', '" + strMun10_5_8 + "', '" + strMun10_5_9 + "',";
                    SQL = SQL + " '" + strMun10_5_10 + "', '" + strMun10_5_11 + "', '" + strMun10_5_11_1 + "',";
                    SQL = SQL + " '" + strMun11_1 + "',";
                    SQL = SQL + " '" + strMun12_1 + "',";
                    SQL = SQL + " '" + strRemark + "'," + clsType.User.Sabun + ",SYSDATE ) ";

                }
                else
                {
                    SQL = " UPDATE KOSMOS_PMPA.OPD_DEPT_MUNJIN SET ";
                    SQL = SQL + " TMUN_H ='" + strH + "', ";
                    SQL = SQL + " TMUN_W ='" + strW + "', ";
                    SQL = SQL + " TMUN1 ='" + strMun1 + "', ";

                    //'--add
                    SQL = SQL + " TMUN4_1 ='" + strMun4_1 + "', ";
                    SQL = SQL + " TMUN4_2 ='" + strMun4_2 + "', ";
                    SQL = SQL + " TMUN4_2_5_1 ='" + strMun4 + "', ";
                    SQL = SQL + " TMUN5_1 ='" + strMun6 + "', ";
                    SQL = SQL + " TMUN2Y ='" + strMun2Y + "', ";
                    SQL = SQL + " TMUN2N ='" + strMun2N + "', ";
                    SQL = SQL + " TMUN2_1 ='" + strMun2_1 + "', ";
                    SQL = SQL + " TMUN2_1_1 ='" + strMun2_1_1 + "', ";
                    SQL = SQL + " TMUN2_2 ='" + strMun2_2 + "', ";
                    SQL = SQL + " TMUN2_2_1 ='" + strMun2_2_1 + "', ";
                    SQL = SQL + " TMUN2_3 ='" + strMun2_3 + "', ";
                    SQL = SQL + " TMUN2_3_1 ='" + strMun2_3_1 + "', ";
                    SQL = SQL + " TMUN2_4 ='" + strMun2_4 + "', ";
                    SQL = SQL + " TMUN2_4_1 ='" + strMun2_4_1 + "', ";
                    SQL = SQL + " TMUN2_5 ='" + strMun2_5 + "', ";
                    SQL = SQL + " TMUN2_5_1 ='" + strMun2_5_1 + "', ";
                    SQL = SQL + " TMUN2_6 ='" + strMun2_6 + "', ";
                    SQL = SQL + " TMUN2_6_1 ='" + strMun2_6_1 + "', ";
                    SQL = SQL + " TMUN2_7 ='" + strMun2_7 + "', ";
                    SQL = SQL + " TMUN2_7_1 ='" + strMun2_7_1 + "', ";
                    SQL = SQL + " TMUN2_8 ='" + strMun2_8 + "', ";
                    SQL = SQL + " TMUN2_8_1 ='" + strMun2_8_1 + "', ";
                    SQL = SQL + " TMUN2_9 ='" + strMun2_9 + "', ";
                    SQL = SQL + " TMUN2_9_1 ='" + strMun2_9_1 + "', ";
                    SQL = SQL + " TMUN2_10 ='" + strMun2_10 + "', ";
                    SQL = SQL + " TMUN2_10_1 ='" + strMun2_10_1 + "', ";
                    SQL = SQL + " TMUN2_12 ='" + strMun2_11 + "', ";
                    SQL = SQL + " TMUN2_12_1 ='" + strMun2_11_1 + "', ";
                    SQL = SQL + " TMUN2_13 ='" + strMun2_12 + "', ";
                    SQL = SQL + " TMUN2_13_1 ='" + strMun2_12_1 + "', ";
                    SQL = SQL + " TMUN8_1 ='" + strMun8_1 + "', ";
                    SQL = SQL + " TMUN8_2 ='" + strMun8_2 + "', ";
                    SQL = SQL + " TMUN8_3 ='" + strMun8_3 + "', ";
                    SQL = SQL + " TMUN8_4 ='" + strMun8_4 + "', ";
                    SQL = SQL + " TMUN8_5 ='" + strMun8_7 + "', ";
                    SQL = SQL + " TMUN8_5_1 ='" + strMun8_7_1 + "', ";
                    SQL = SQL + " TMUN8_6 ='" + strMun8_5 + "', ";
                    SQL = SQL + " TMUN8_7 ='" + strMun8_6 + "', ";

                    //'--add
                    SQL = SQL + " TMUN10_1 ='" + strMun10_1 + "', ";
                    SQL = SQL + " TMUN10_2 ='" + strMun10_2 + "', ";
                    SQL = SQL + " TMUN10_2_1 ='" + strMun10_2_1 + "', ";
                    SQL = SQL + " TMUN10_3_1 ='" + strMun10_3_1 + "', ";
                    SQL = SQL + " TMUN10_3_2 ='" + strMun10_3_2 + "', ";
                    SQL = SQL + " TMUN10_3_3 ='" + strMun10_3_3 + "', ";
                    SQL = SQL + " TMUN10_3_4 ='" + strMun10_3_4 + "', ";
                    SQL = SQL + " TMUN10_4_1 ='" + strMun10_4_1 + "', ";
                    SQL = SQL + " TMUN10_4_2 ='" + strMun10_4_2 + "', ";
                    SQL = SQL + " TMUN10_4_3 ='" + strMun10_4_3 + "', ";
                    SQL = SQL + " TMUN10_4_4 ='" + strMun10_4_4 + "', ";
                    SQL = SQL + " TMUN10_4_5 ='" + strMun10_4_5 + "', ";
                    SQL = SQL + " TMUN10_5_1 ='" + strMun10_5_1 + "', ";
                    SQL = SQL + " TMUN10_5_2 ='" + strMun10_5_2 + "', ";
                    SQL = SQL + " TMUN10_5_3 ='" + strMun10_5_3 + "', ";
                    SQL = SQL + " TMUN10_5_4 ='" + strMun10_5_4 + "', ";
                    SQL = SQL + " TMUN10_5_5 ='" + strMun10_5_5 + "', ";
                    SQL = SQL + " TMUN10_5_6 ='" + strMun10_5_6 + "', ";
                    SQL = SQL + " TMUN10_5_7 ='" + strMun10_5_7 + "', ";
                    SQL = SQL + " TMUN10_5_8 ='" + strMun10_5_8 + "', ";
                    SQL = SQL + " TMUN10_5_9 ='" + strMun10_5_9 + "', ";
                    SQL = SQL + " TMUN10_5_10 ='" + strMun10_5_10 + "', ";
                    SQL = SQL + " TMUN10_5_11 ='" + strMun10_5_11 + "', ";
                    SQL = SQL + " TMUN10_5_11_1 ='" + strMun10_5_11_1 + "', ";

                    SQL = SQL + " tmun11_f1 ='" + strMun11_1 + "', ";
                    SQL = SQL + " tmun12_t1 ='" + strMun12_1 + "', ";

                    SQL = SQL + "Remark ='" + strRemark + "', ";
                    SQL = SQL + "ENTSABUN =" + clsType.User.Sabun + ", ";
                    SQL = SQL + "ENTDATE =SYSDATE  ";

                    SQL = SQL + " WHERE ROWID ='" + FstrROWID + "' ";
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

        private void DATA_BUILD()
        {
            string strData = "";
            string strSUB = "";
            string strSUB2 = "";


            if (chkNo_0.Checked == true)
            {
                strData = strData + ComNum.VBLF;
                strData = strData + " ▣ 주호소";
                strData = strData + ComNum.VBLF + VB.Trim(txt1.Text);
            }


            if (chkNo_1.Checked == true)
            {
                strData = strData + ComNum.VBLF;
                strData = strData + " ▣ 현재통증정도";


                if (chk10_1.Checked == true) strData = strData + " : 없음";


                if (chk10_2.Checked == true)
                {
                    strSUB = "";
                    if (VB.Trim(txt10_2_1.Text) != "")
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " 통증 초기평가 : " + VB.Trim(txt10_2_1.Text); ;
                    }


                    if (VB.Trim(txt10_3_1.Text) != "")
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " 가장 심할 때 : " + VB.Trim(txt10_3_1.Text);
                    }


                    if (VB.Trim(txt10_3_2.Text) != "")
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " 가장 덜할 때 : " + VB.Trim(txt10_3_2.Text);
                    }


                    if (VB.Trim(txt10_3_3.Text) != "")
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " 견딜수 있는 수준 : " + VB.Trim(txt10_3_3.Text);
                    }


                    if (VB.Trim(txt10_3_4.Text) != "")
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " 하루평균 : " + VB.Trim(txt10_3_4.Text);
                    }


                    if (strSUB != "") strData = strData + strSUB;


                    if (chk10_5_1.Checked == true) strSUB2 = strSUB2 + " 날카로운 느낌(sharp)";
                    if (chk10_5_2.Checked == true) strSUB2 = strSUB2 + " 둔한 느낌(dull)";
                    if (chk10_5_3.Checked == true) strSUB2 = strSUB2 + " 타는듯한 느낌(burning)";
                    if (chk10_5_4.Checked == true) strSUB2 = strSUB2 + " 누르는듯한 느낌(pressing)";
                    if (chk10_5_5.Checked == true) strSUB2 = strSUB2 + " 칼로 벤 것처럼 아픔(cutting)";
                    if (chk10_5_6.Checked == true) strSUB2 = strSUB2 + " 저린 느낌(numbing)";
                    if (chk10_5_7.Checked == true) strSUB2 = strSUB2 + " 쑤시는 느낌(tingling)";
                    if (chk10_5_8.Checked == true) strSUB2 = strSUB2 + " 죄는듯한 느낌(mumbing)";
                    if (chk10_5_9.Checked == true) strSUB2 = strSUB2 + " 아팠다 안아팠다함(come and goes)";
                    if (chk10_5_10.Checked == true) strSUB2 = strSUB2 + " 다른 부위로 퍼지듯 아픔(radiating)";
                    if (chk10_5_11.Checked == true)
                    {
                        strSUB2 = strSUB2 + " 기타";
                        if (VB.Trim(txt10_5_11_1.Text) != "")
                        {
                            strSUB2 = strSUB2 + " : " + VB.Trim(txt10_5_11_1.Text);
                        }
                    }

                    if (strSUB2 != "") strSUB2 = " <통증의 양상> " + ComNum.VBLF + strSUB2;
                    strData = strData + ComNum.VBLF + strSUB2;
                }
            }




            if (chkNo_2.Checked == true)
            {
                if (VB.Trim(txtHeight.Text) != "" || VB.Trim(txtWeight.Text) != "")
                {
                    strData = strData + ComNum.VBLF;
                    strData = strData + " ▣ 키/몸무게 : " + VB.Trim(txtHeight.Text) + "/" + VB.Trim(txtWeight.Text);
                }
            }


            if (chkNo_3.Checked == true)
            {
                if (VB.Trim(txtOs4.Text) != "")
                {
                    strData = strData + ComNum.VBLF;
                    strData = strData + " ▣ 알러지 : " + VB.Trim(txtOs4.Text);
                }
            }

            if (chkNo_4.Checked == true)
            {
                strData = strData + ComNum.VBLF;
                strData = strData + " ▣ 최근 1개월 내 해외여행력 : ";
                if (chk12_1.Checked == true)
                {
                    strData = strData + " 무";
                }
                else if (chk12_2.Checked == true)
                {
                    strData = strData + " 유";
                }
            }


            if (chkNo_5.Checked == true)
            {
                strData = strData + ComNum.VBLF;
                strData = strData + " ▣ 임신 여부 : ";
                if (chkOs61.Checked == true)
                {
                    strData = strData + " 무";
                }
                else if (chkOs62.Checked == true)
                {
                    strData = strData + " 유";
                }
            }



            if (chkNo_6.Checked == true)
            {
                strData = strData + ComNum.VBLF;
                strData = strData + " ▣ 과거병력과 현재 복용중인 약 종류, 복용 시기";


                if (chk2_1N.Checked == true) strData = strData + " : 없음";


                if (chk2_1Y.Checked == true)
                {
                    strSUB = "";
                    if (chk2_1.Checked == true)
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " - 간질환";
                        if (VB.Trim(txt2_1.Text) != "")
                        {
                            strSUB = strSUB + " ( 약종류와 복용시기 : " + VB.Trim(txt2_1.Text) + " )";
                        }
                    }


                    if (chk2_2.Checked == true)
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " - 고혈압";
                        if (VB.Trim(txt2_2.Text) != "")
                        {
                            strSUB = strSUB + " ( 약종류와 복용시기 : " + VB.Trim(txt2_2.Text) + " )";
                        }
                    }


                    if (chk2_3.Checked == true)
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " - 당뇨";
                        if (VB.Trim(txt2_3.Text) != "")
                        {
                            strSUB = strSUB + " ( 약종류와 복용시기 : " + VB.Trim(txt2_3.Text) + " )";
                        }
                    }


                    if (chk2_4.Checked == true)
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " - 뇌질환";
                        if (VB.Trim(txt2_4.Text) != "")
                        {
                            strSUB = strSUB + " ( 약종류와 복용시기 : " + VB.Trim(txt2_4.Text) + " )";
                        }
                    }


                    if (chk2_5.Checked == true)
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " - 심질환";
                        if (VB.Trim(txt2_5.Text) != "")
                        {
                            strSUB = strSUB + " ( 약종류와 복용시기 : " + VB.Trim(txt2_5.Text) + " )";
                        }
                    }


                    if (chk2_6.Checked == true)
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " - 신질환";
                        if (VB.Trim(txt2_6.Text) != "")
                        {
                            strSUB = strSUB + " ( 약종류와 복용시기 : " + VB.Trim(txt2_6.Text) + " )";
                        }
                    }


                    if (chk2_7.Checked == true)
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " - 호흡기";
                        if (VB.Trim(txt2_7.Text) != "")
                        {
                            strSUB = strSUB + " ( 약종류와 복용시기 : " + VB.Trim(txt2_7.Text) + " )";
                        }
                    }


                    if (chk2_8.Checked == true)
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " - 소화기";
                        if (VB.Trim(txt2_8.Text) != "")
                        {
                            strSUB = strSUB + " ( 약종류와 복용시기 : " + VB.Trim(txt2_8.Text) + " )";
                        }
                    }


                    if (chk2_9.Checked == true)
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " - 수술력";
                        if (VB.Trim(txt2_9.Text) != "")
                        {
                            strSUB = strSUB + " ( 약종류와 복용시기 : " + VB.Trim(txt2_9.Text) + " )";
                        }
                    }


                    if (chk2_10.Checked == true)
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " - 기타";
                        if (VB.Trim(txt2_10.Text) != "")
                        {
                            strSUB = strSUB + " ( 약종류와 복용시기 : " + VB.Trim(txt2_10.Text) + " )";
                        }
                    }


                    if (chk2_11.Checked == true)
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " - 갑상선";
                        if (VB.Trim(txt2_11.Text) != "")
                        {
                            strSUB = strSUB + " ( 약종류와 복용시기 : " + VB.Trim(txt2_11.Text) + " )";
                        }
                    }


                    if (chk2_12.Checked == true)
                    {
                        strSUB = strSUB + ComNum.VBLF;
                        strSUB = strSUB + " - 골다공증";
                        if (VB.Trim(txt2_12.Text) != "")
                        {
                            strSUB = strSUB + " ( 약종류와 복용시기 : " + VB.Trim(txt2_12.Text) + " )";
                        }
                    }
                    
                    strData = strData + strSUB;
                }
            }


            if (chkNo_7.Checked == true)
            {
                strSUB = "";
                if (chk8_1.Checked == true) strSUB = strSUB + " 소견서";
                if (chk8_2.Checked == true) strSUB = strSUB + " 진료의뢰서";
                if (chk8_3.Checked == true) strSUB = strSUB + " CD";
                if (chk8_4.Checked == true) strSUB = strSUB + " 검사결과지";
                if (chk8_5.Checked == true) strSUB = strSUB + " 진단서";
                if (chk8_6.Checked == true) strSUB = strSUB + " 의무기록사본";
                if (chk8_7.Checked == true) strSUB = strSUB + " 기타 : " + (VB.Trim(txt8_7_1.Text) != "" ? VB.Trim(txt8_7_1.Text) : "");


                if (strSUB != "")
                {
                    strData = strData + ComNum.VBLF;
                    strData = strData + " ▣ 타병원 소견서 및 CD";
                    strData = strData + ComNum.VBLF;
                    strData = strData + strSUB;
                }
            }



            if (chkNo_8.Checked == true)
            {
                if (VB.Trim(txtRemark.Text) != "")
                {
                    strData = strData + ComNum.VBLF;
                    strData = strData + " ▣ 참고사항";
                    strData = strData + ComNum.VBLF + VB.Trim(txtRemark.Text);
                }
            }


            txtSendData.Text = strData;

            
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
                
                chkNo_0.Checked = true;
                chkNo_1.Checked = true;
                chkNo_2.Checked = true;
                chkNo_3.Checked = true;
                chkNo_4.Checked = true;
                chkNo_5.Checked = true;
                chkNo_6.Checked = true;
                chkNo_7.Checked = true;
                chkNo_8.Checked = true;
                
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

        private bool SaveErInfoXML()
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            //string strPtNo = "";
            //string strSname = "";
            //string strDeptCode = "";
            //string StrDrCode = "";
            //string strBDATE = "";

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
                SQL += " UPDATE KOSMOS_PMPA.OPD_DEPT_MUNJIN SET                 \r";
                SQL += "        EMRNO = " + dblEmrNo + "                        \r";
                SQL += "      , CHK = 'Y'                                       \r";
                SQL += " WHERE PANO = '" + FstrPaNo + "'                        \r";
                SQL += "  AND BDATE =TO_DATE('" + FstrBDate + "','YYYY-MM-DD')  \r";
                SQL += "  AND DEPTCODE ='" + FstrDept + "'                      \r";
                SQL += "  AND (DELDATE IS NULL OR DELDATE ='')                  \r";
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

        private void ssDept_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strDept = "";

            if (e.ColumnHeader == true || e.RowHeader == true) return;

            strDept = ssDept_Sheet1.Cells[e.Row, e.Column].Text;

            if (strDept == "NP" || strDept == "OG" || strDept == "OS" || strDept == "MN")
            {
                ComFunc.MsgBox("전용예진표에서는 다른과 내역을 확인할수 없습니다..");
            }
            else
            {
                READ_Munjin_Data("2", FstrPaNo, strDept, FstrBDate);
            }
        }

        private void txt10_2_1_Leave(object sender, EventArgs e)
        {
            if (VB.Val(txt10_2_1.Text) >= 4)
            {
                txt10_2_1.BackColor = Color.FromArgb(255, 128, 128);
            }
            else
            {
                txt10_2_1.BackColor = Color.FromArgb(0, 0, 0);
            }            
        }

        private void txt10_3_1_Leave(object sender, EventArgs e)
        {
            Gesan_tot();
        }

        private void Gesan_tot()
        {
            txt10_2_1.Text = Convert.ToString(VB.Val(txt10_3_4.Text) + VB.Val(txt10_4_1.Text) + VB.Val(txt10_4_2.Text) + VB.Val(txt10_4_3.Text) + VB.Val(txt10_4_4.Text) + VB.Val(txt10_4_5.Text));

            if (VB.Val(txt10_2_1.Text) >= 4)
            {
                txt10_2_1.BackColor = Color.FromArgb(255, 128, 128);
            }
            else
            {
                txt10_2_1.BackColor = Color.FromArgb(0, 0, 0);
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
                btnAll.Text = "모든항목 해제";
            }
        }
    }
}
