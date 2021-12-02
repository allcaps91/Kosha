using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace ComLibB
{
    /// <summary>
    /// Description : 필요시 처방(PRN) 처방 상세내용 입력
    /// Author : 이상훈
    /// Create Date : 2017.12.18
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="FrmPRN처방입력.frm"/>
    public partial class FrmMedPrnOrdInput : Form
    {
        string SQL;
        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;
        string SqlErr = "";     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        string GstrPRN_ROWID;

        string FstrSuCode;
        string FstrSuName;
        string FstrSTS1;
        string FstrSTS2;

        string FstrNarcoticsChk;    //마약류
        string FstrExeStdSub;       //실시기준

        double FnMax; //1회 최대
        double FnMax2; //1일 최대
        int FnMaxQtyCnt;
        string FstrSelData;

        string FS1;

        public delegate void PrnOrd_Click(string strOK, string ExeStd, double Qty, string Div, string InTerm, string InDiv, string InTermMin, string InDivMax, string Notify);
        public event PrnOrd_Click PrnOrdInput;



        public FrmMedPrnOrdInput()
        {
            InitializeComponent();
        }

        public FrmMedPrnOrdInput(string sSuCode, string sSuName, string sSTS1, string sSTS2)
        {
            InitializeComponent();

            FstrSuCode = sSuCode;
            FstrSuName = sSuName;
            FstrSTS1 = sSTS1;
            FstrSTS2 = sSTS2;
        }
        
        public FrmMedPrnOrdInput(string sSuCode, string sSuName)
        {
            InitializeComponent();

            FstrSuCode = sSuCode;
            FstrSuName = sSuName;
        }

        public FrmMedPrnOrdInput(string sSuCode, string sSuName, string strPRN_ROWID)
        {
            InitializeComponent();

            FstrSuCode = sSuCode;
            FstrSuName = sSuName;
            GstrPRN_ROWID = strPRN_ROWID;
        }

        private void FrmMedPrnOrdInput_Load(object sender, EventArgs e)
        {
            this.Location = new Point(100, 100);

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            FstrNarcoticsChk = clsOrdFunction.Read_Drug_SuCode(clsDB.DbCon, FstrSuCode);

            fn_Read_Max_Check(FstrSuCode);
            fn_Screen_Clear();
            fn_CobSet();

            FstrSelData = "";

            lblPInfo.Text = "[약품명 : " + FstrSuName + " ] [ 의약품코드 : " + FstrSuCode + " ]";

            if (FstrNarcoticsChk == "OK")
            {
                lblPinfo2.Text = "마약류";
            }
            else
            {
                lblPinfo2.Text = "일반약";
            }
            lblV3.Text = "Notifying 시기 (다음 중 선택후 내용입력하십시오)";
            txtV4.ReadOnly = false;

            txtS1.Text = FstrSTS1;
            txtS2.Text = FstrSTS2;

            //적응증 및 실시기준(공통) 세부설정
            if (clsPublic.GstrPRN_Sub_Chk == "OK" && FstrExeStdSub == "S")
            {
                txtS1.Visible = false;
                cboSub.Visible = true;

                lblV1.Text = "선택후 내용입력하세요";
                lblV2.Text = "적응증 및 실시기준(공통)";

                txtS2.Text = "";

                //실시기준 Sub
                try
                {
                    SQL = "";
                    SQL += " SELECT b.REMARK1, b.REMARK2                        \r";  
                    SQL += "   FROM KOSMOS_ADM.DRUG_MASTER4     a               \r";
                    SQL += "      , KOSMOS_ADM.DRUG_MASTER4_PRN b               \r";
                    SQL += "  WHERE a.JEPCODE = b.JEPCODE                       \r";
                    SQL += "    AND a.JepCode = '" + FstrSuCode + "'            \r";
                    SQL += "    AND a.INSULIN_SCALE = 'S'                       \r";
                    SQL += "    AND (b.DELDATE IS NULL OR b.DELDATE = '')       \r";
                    SQL += "  ORDER BY b.SORT                                   \r";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    cboSub.Items.Clear();
                    cboSub.Items.Add(" ");

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            cboSub.Items.Add(dt.Rows[i]["REMARK1"].ToString().Trim() + " : " + dt.Rows[i]["REMARK2"].ToString().Trim());
                        }
                    }

                    cboSub.SelectedIndex = 0;

                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
            }
            else
            {
                txtS1.Visible = true;
                cboSub.Visible = false;
                lblV1.Text = "적응증";
                lblV2.Text = "실시기준(공통)";

                cboSub.Items.Clear();
                cboSub.Items.Add(" ");
                cboSub.SelectedIndex = 0;
            }

            if (GstrPRN_ROWID != "" && GstrPRN_ROWID != null)
            {   
                if (FS1 != null)
                {
                    txtS1.Text = FS1.Trim(); //적응증
                }
                txtS2.Text = VB.Pstr(GstrPRN_ROWID, "^^", 3).Trim();    //실시기준(공통)
                txtV1.Text = VB.Pstr(GstrPRN_ROWID, "^^", 4).Trim();    //일투량
                cboDosCode.Text = VB.Pstr(GstrPRN_ROWID, "^^", 5).Trim();    //용법
                txtV2.Text = VB.Pstr(GstrPRN_ROWID, "^^", 6).Trim();    //투여간격
                txtV3.Text = VB.Pstr(GstrPRN_ROWID, "^^", 7).Trim();    //투여횟수
                txtV4.Text = VB.Pstr(GstrPRN_ROWID, "^^", 8).Trim();    //Notify
            }
        }

        void fn_Screen_Clear()
        {
            lblPInfo.Text = "";
            lblPinfo2.Text = "";

            txtS1.Text = "";
            txtS2.Text = "";
            txtV1.Text = "";
            txtV2.Text = "";
            txtV3.Text = "";
            txtV4.Text = "Notifying 시기를 선택하거나 프리타이핑 하십시오 !!";

            txtV4.ReadOnly = true;

            txtS1.Visible = true;
            lblV1.Visible = true;
            lblV2.Text = "실시기준(공통)";
        }

        void fn_CobSet()
        {
            string strTemp = clsOrderEtc.Read_PRN_IV_CHK(clsDB.DbCon, FstrSuCode);

            cboDosCode.Items.Clear();
            cboDosCode.Items.Add(" ");

            if (VB.Split(strTemp, ",").Length > 0)
            {
                for (int i = 0; i < VB.Split(strTemp, ",").Length; i++)
                {
                    cboDosCode.Items.Add(VB.Split(strTemp, ",")[i].ToString());
                }
            }

            cboDosCode.SelectedIndex = 0;

            cboNoti.Items.Clear();
            cboNoti.Items.Add(" ");

            cboNoti.Items.Add("최대 투여횟수 수행후 호전되지 않으면 Notifying 하세요!");
            cboNoti.Items.Add("투여 전 Notifying 하세요!");
            cboNoti.Items.Add("첫번째 투여 후 Notifying 하세요!");
            cboNoti.Items.Add("[  ] 회 투여 후 Notifying 하세요!");
            cboNoti.Items.Add("[ 내용입력 ] Notifying 하세요!");
            cboNoti.SelectedIndex = 0;

            //if (FstrNarcoticsChk == "OK")
            //{
            //    cboNoti.Items.Add("최대 투여횟수 수행후 호전되지 않으면 Notifying 하세요!");
            //    cboNoti.Items.Add("투여 전 Notifying 하세요!");
            //    cboNoti.Items.Add("첫번째 투여 후 Notifying 하세요!");
            //    cboNoti.Items.Add("[  ] 회 투여 후 Notifying 하세요!");
            //    cboNoti.Items.Add("[ 내용입력 ] Notifying 하세요!");
            //    cboNoti.SelectedIndex = 0;
            //}
            //else
            //{
            //    cboNoti.Items.Add("최대 투여횟수 수행후 호전되지 않으면 Notifying 하세요!");
            //    cboNoti.Items.Add("투여 전 Notifying 하세요!");
            //    cboNoti.Items.Add("첫번째 투여 후 Notifying 하세요!");
            //    cboNoti.Items.Add("[  ] 회 투여 후 Notifying 하세요!");
            //    cboNoti.Items.Add("[ 내용입력 ] Notifying 하세요!");
            //    cboNoti.SelectedIndex = 0;
            //}
        }

        void fn_Read_Max_Check(string sSuCode)
        {
            FstrExeStdSub = "";

            try
            {
                SQL = "";
                SQL += " SELECT b.MaxQty_Gubun1                     \r";
                SQL += "      , b.MAXQTY_CNT                        \r";
                SQL += "      , b.MaxQty_1Time_Qty                  \r";
                SQL += "      , b.MaxQty_1Day_Qty                   \r";
                SQL += "      , Insulin_Scale                       \r";
                SQL += "   FROM KOSMOS_ADM.DRUG_MASTER4 b           \r";
                SQL += "  WHERE b.JepCode = '" + sSuCode.Trim() + "'       \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    FnMax = double.Parse(dt.Rows[0]["MaxQty_1Time_Qty"].ToString());
                    FnMax2 = double.Parse(dt.Rows[0]["MaxQty_1Day_Qty"].ToString());
                    FstrExeStdSub = dt.Rows[0]["Insulin_Scale"].ToString().Trim();
                    FS1 = dt.Rows[0]["MaxQty_Gubun1"].ToString().Trim();
                    //2019-05-27 추가
                    FnMaxQtyCnt = (int)VB.Val(dt.Rows[0]["MAXQTY_CNT"].ToString());
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            clsPublic.GstrPRN_New_Data = "";
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string sExeStd = "";
            double nQty = 1;
            string nDiv = "";
            string sInTerm = "";
            string sInDiv = "";
            string sNotif = "";

            double nV1 = 0;
            double nV2 = 0;

            //2019-06-18 전산업무 의뢰서 2019-596 추가
            string sInTermMin = "";     //최소투여간격
            string sInDivMax = "";      //최대투여횟수

            if (txtS2.Text.Trim() == "")
            {
                MessageBox.Show("실시기준(공통) 값을 입력 하십시오!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtS2.Focus();
                return;
            }
            else
            {
                switch (FstrSuCode)
                {
                    case "FSMA":
                        if (FstrSTS2 == txtV2.Text.Trim())
                        {
                            MessageBox.Show("실시기준(공통) 값을 정확히 넣으십시오!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtV2.Focus();
                            return;
                        }
                        break;
                    default:
                        break;
                }
            }

            if (FstrExeStdSub == "OK")
            {
                if (FstrSelData != "")
                {
                    if (VB.L(txtS2.Text.Trim(), VB.Pstr(FstrSelData, ":", 1)) <= 1)
                    {
                        MessageBox.Show("실시기준(공통) 값을 다시 선택후 입력하십시오!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtV2.Focus();
                        return;
                    }
                }
            }

            if (txtV1.Text.Trim() == "")
            {
                MessageBox.Show("일투량(1회용량) 값을 입력 하십시오!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtV1.Focus();
                return;
            }

            if (cboDosCode.Text.Trim() == "")
            {
                MessageBox.Show("용법을 선택 하십시오!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboDosCode.Focus();
                return;
            }

            if (txtV2.Text.Trim() == "")
            {
                MessageBox.Show("투여간격 ( ) 시간 입력 하십시오!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtV2.Focus();
                return;
            }

            //2019-05-27 추가 약품마스터 최대투여횟수
            //if (txtV2_Min.Text.Trim() == "")
            //{
            //    MessageBox.Show("최소투여간격 ( ) 시간 입력 하십시오!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    txtV2.Focus();
            //    return;
            //}

            if (txtV3.Text.Trim() == "")
            {
                MessageBox.Show("투여횟수( ) 회까지 입력 하십시오!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtV3.Focus();
                return;
            }

            //2019-05-27 추가 약품마스터 최대투여횟수
            //if (txtV3_Max.Text.Trim() == "")
            //{
            //    MessageBox.Show("최대투여횟수( ) 회까지 입력 하십시오!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    txtV3.Focus();
            //    return;
            //}

            if (txtV4.Text.Trim() == "Notifying 시기를 선택하거나 프리타이핑 하십시오 !!")
            {
                MessageBox.Show("Notifying 항목을 선택하거나 타이핑 하십시오!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtV4.Focus();
                return;
            }

            nV1 = double.Parse(txtV1.Text);
            nV2 = double.Parse(txtV3.Text);

            //1회 최대 체크
            if (nV1 > FnMax)
            {
                MessageBox.Show("1회 최대일투량은 " + FnMax + " 입니다!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //1일 최대 체크
            if ((nV1 * nV2) > FnMax2)
            {
                MessageBox.Show("1일 최대일투량(일투량*투여횟수)은 " + FnMax2 + " 입니다!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //2019-05-27 추가 약품마스터 최대투여횟수
            //if ((int)VB.Val(txtV3_Max.Text.Trim()) > FnMaxQtyCnt)
            //{
            //    MessageBox.Show("최대 투여횟수는 " + FnMaxQtyCnt + " 입니다!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);                
            //    return;
            //}

            sExeStd = txtS2.Text.Trim();// 실시기준(공통)
            nQty = double.Parse(txtV1.Text);
            nDiv = cboDosCode.Text.Trim();
            sInTerm = txtV2.Text.Trim();
            sInDiv = txtV3.Text.Trim();
            sNotif = txtV4.Text.Trim();

            //2019-06-18 전산업무 의뢰서 2019-596 추가
            sInTermMin = txtV2_Min.Text.Trim();
            sInDivMax = txtV3_Max.Text.Trim();

            PrnOrdInput("OK", sExeStd, nQty, nDiv, sInTerm, sInDiv, sInTermMin, sInDivMax, sNotif);

            this.Close();
        }
        
        private void txtS2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                txtV1.Focus();
            }
        }

        private void txtV1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                cboDosCode.Focus();
            }
        }

        private void cboDosCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                txtV2.Focus();
            }
        }

        private void txtV2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                txtV3.Focus();
            }
        }

        private void txtV3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                cboNoti.Focus();
            }
        }

        //private void cboNoti_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == (char)13)
        //    {
        //        txtV4.Focus();
        //    }
        //}
        
        
        private void cboNoti_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNoti.Text.Trim() != "")
            {
                FstrSelData = cboNoti.Text.Trim();
                txtV4.Text = cboNoti.Text.Trim().Replace("내용입력", " ");

                if (VB.L(FstrSelData, "내용입력") > 1)
                {
                    txtV4.SelectionStart = 2;
                }
                else
                {
                    txtV4.SelectionStart = txtV4.Text.Length;
                }
                txtV4.Focus();
            }
        }

        private void cboSub_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSub.Text.Trim() != "")
            {
                FstrSelData = cboSub.Text.Trim();
                txtS2.Text = cboSub.Text.Trim().Replace("내용입력", " ");

                if (VB.L(FstrSelData, "내용입력") > 1)
                {
                    txtS2.SelectionStart = txtS2.Text.Length - 3;

                }
                else
                {
                    txtS2.SelectionStart = txtS2.Text.Length;
                }
                txtS2.Focus();
            }
        }
    }
}
