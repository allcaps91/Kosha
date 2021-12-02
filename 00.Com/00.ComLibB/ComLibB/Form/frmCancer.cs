using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 암등록
/// Author : 김형범
/// Create Date : 2017.06.27
/// </summary>
/// <history>
/// FormInfo_History함수 생성필요, MfrmMain form생성 필요
/// </history>
namespace ComLibB
{
    /// <summary> 암등록 </summary>
    public partial class frmCancer : Form
    {
        /// <summary> 암등록 </summary>
        public frmCancer()
        {
            InitializeComponent();
        }

        //TODO: FormInfo_History함수 생성필요
        void frmCancer_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            cboJob_Load();
            cboDiagnisis_Load();
            cboArea_Load();
            Init();

            ssView1.Cursor = Cursors.Arrow;
            ssView2.Cursor = Cursors.Arrow;
        }

        void cboJob_Load()
        {
            cboJob.Items.Clear();

            cboJob.Items.Add("1.전문기술직");
            cboJob.Items.Add("2.행정직");
            cboJob.Items.Add("3.사무직");
            cboJob.Items.Add("4.판매종사자");
            cboJob.Items.Add("5.서비스직");
            cboJob.Items.Add("6.농업,축산업,임업,수산업,수렵업");
            cboJob.Items.Add("7.생산직");
            cboJob.Items.Add("8.운수업");
            cboJob.Items.Add("9.단순노무자");
            cboJob.Items.Add("10.분류불능자");
            cboJob.Items.Add("11.학생");
            cboJob.Items.Add("12.군인");
            cboJob.Items.Add("13.주부");
            cboJob.Items.Add("14.기타");
            cboJob.Items.Add("99.직업모름");
        }

        void cboDiagnisis_Load()
        {
            cboDiagnisis.Items.Clear();

            cboDiagnisis.Items.Add("1.검사없이 임상만으로");
            cboDiagnisis.Items.Add("2.임상검사");
            cboDiagnisis.Items.Add("3.조직검사없는 진단적 수술 또는 부검");
            cboDiagnisis.Items.Add("4.특수 생화학적 또는 면역학적검사");
            cboDiagnisis.Items.Add("5.세포학적 또는 혈액학적 검사");
            cboDiagnisis.Items.Add("6.전이부위의 조직학적 검사");
            cboDiagnisis.Items.Add("7.원발부위의 조직학적 검사");
            cboDiagnisis.Items.Add("8.조직검사를 시행한 진단적 수술 또는 부검");
        }

        void cboArea_Load()
        {

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;

            cboArea.Items.Clear();

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT JICODE, JINAME FROM BAS_AREA   ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY JICODE                      ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboArea.Items.Add(dt.Rows[i]["JICODE"].ToString().Trim() + "," + dt.Rows[i]["JINAME"].ToString().Trim());
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

        void Init()
        {
            txtEtc.Text = "";
            txtNo.Text = "";
            txtName.Text = "";
            txtJumin1.Text = "";
            txtJumin2.Text = "";
            txtCancerNo.Text = "";

            txtAddress.Text = "";
            txtExistence.Text = "";

            txtCure.Text = "";
            txtNote.Text = "";
            cboJob.SelectedIndex = 0;
            cboDiagnisis.SelectedIndex = 0;
            cboArea.SelectedIndex = 0;
            txtInfo.Text = "";

            chkMedical0.Checked = false;
            chkMedical1.Checked = true;
            chkMedical2.Checked = false;
            chkMedical3.Checked = false;
            chkMedical4.Checked = false;
            chkMedical5.Checked = false;
            chkMedical6.Checked = false;
        }

        void btnSave_Click(object sender, EventArgs e)
        {

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (txtNo.Text == "")
            {
                ComFunc.MsgBox("병록번호를 입력 하세요", "경 고");
                txtNo.Enabled = true;
                txtNo.Focus();
                return;
            }

            //퇴원일이 입원일보다 작으면 멘트
            if (dtpDateHospital.Value < dtpDatehospialaization.Value)
            {
                ComFunc.MsgBox("입원일보다 퇴원일이 작습니다..", "");
                return;
            }

            try
            {
                SQL = "";
                SQL = " SELECT * ";
                SQL = SQL + ComNum.VBLF + "  FROM MID_CANCER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + VB.Format(VB.Val(txtNo.Text), "00000000") + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Outdate = TO_DATE('" + dtpDateHospital.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    Update_Mid_Patient();
                }
                else
                {
                    Insert_Mid_Patient();
                }

                dt.Dispose();
                dt = null;

                ssView1_Sheet1.RowCount = 0;
                ssView2_Sheet1.RowCount = 0;
                Init();
                txtNo.Focus();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }


        }

        //TODO: MfrmMain.PanelMsg.Caption form생성 필요
        void Update_Mid_Patient()
        {
            string strt1code1 = "";
            string strt1code2 = "";
            string strt1code3 = "";
            string strm1code1 = "";
            string strm1code2 = "";
            string strm1code3 = "";
            int intAge = 0;
            string strHo = ""; //호르몬요법
            string strCare = ""; //치료시행여부 Y

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            strHo = "0";

            if (chkMedical6.Checked == true)
            {
                strHo = "1";
            }

            strCare = "0";

            if (rdoCare0.Checked == true)
            {
                strCare = "1";
            }

            clsDB.setBeginTran(clsDB.DbCon);


            intAge = ComFunc.AgeCalcEx(txtJumin1.Text + txtJumin2.Text, dtpDateHospital.Value.ToString("yyyy-MM-dd"));

            strt1code1 = VB.Left(ssView1_Sheet1.Cells[0, 0].Text, 6).Trim();
            strt1code2 = VB.Left(ssView1_Sheet1.Cells[1, 0].Text, 6).Trim();
            strt1code3 = VB.Left(ssView1_Sheet1.Cells[2, 0].Text, 6).Trim();

            strm1code1 = VB.Left(ssView2_Sheet1.Cells[0, 0].Text, 6).Trim();
            strm1code2 = VB.Left(ssView2_Sheet1.Cells[1, 0].Text, 6).Trim();
            strm1code3 = VB.Left(ssView2_Sheet1.Cells[2, 0].Text, 6).Trim();

            try
            {
                SQL = "";
                SQL = " UPDATE MID_CANCER SET";
                SQL = SQL + ComNum.VBLF + "        SNAME   = '" + txtName.Text + "', ";
                SQL = SQL + ComNum.VBLF + "        JUMIN1  = '" + VB.Format(VB.Val(txtJumin1.Text), "000000") + "', ";
                SQL = SQL + ComNum.VBLF + "        Age = " + intAge + ", ";
                SQL = SQL + ComNum.VBLF + "        JUMIN2  = '" + VB.Left(VB.Format(VB.Val(txtJumin2.Text), "000000"), 1) + "******', ";
                SQL = SQL + ComNum.VBLF + "        JUMIN3  = '" + clsAES.AES(VB.Format(VB.Val(txtJumin2.Text), "000000")) + "', ";
                SQL = SQL + ComNum.VBLF + "        CYY     = '" + dtpCyy.Value.ToString("yyyy-MM-dd") + "', ";
                SQL = SQL + ComNum.VBLF + "        Sex= '" + VB.Left(txtJumin2.Text, 1) + "', ";
                SQL = SQL + ComNum.VBLF + "        CNO     = '" + txtCancerNo.Text + "', ";
                SQL = SQL + ComNum.VBLF + "        INDATE  = TO_DATE('" + dtpDatehospialaization.Text + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "        JUSO    = '" + txtAddress.Text + "',= ";
                SQL = SQL + ComNum.VBLF + "        OUTDATE = TO_DATE('" + dtpDateHospital.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "        JICODE  = '" + VB.Mid(cboArea.Text, 1, 2) + "', ";
                SQL = SQL + ComNum.VBLF + "        MTREAT  = '" + txtCure.Text + "', ";
                SQL = SQL + ComNum.VBLF + "        TRDATE  = TO_DATE('" + dtpTrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "        T1CODE1 = '" + strt1code1 + "', ";
                SQL = SQL + ComNum.VBLF + "        T1CODE2 = '" + strt1code2 + "', ";
                SQL = SQL + ComNum.VBLF + "        T1CODE3 = '" + strt1code3 + "', ";
                SQL = SQL + ComNum.VBLF + "        M1CODE1 = '" + strm1code1 + "', ";
                SQL = SQL + ComNum.VBLF + "        M1CODE2 = '" + strm1code2 + "', ";
                SQL = SQL + ComNum.VBLF + "        M1CODE3 = '" + strm1code3 + "', ";
                SQL = SQL + ComNum.VBLF + "        mtreat_ho = '" + strHo + "', ";  //치료시행여부
                SQL = SQL + ComNum.VBLF + "        GbCare = '" + strCare + "', ";  //치료시행여부
                SQL = SQL + ComNum.VBLF + "        JTREAT  = '" + VB.Left(cboDiagnisis.Text, 1) + "', ";
                SQL = SQL + ComNum.VBLF + "        SILSU   = '" + txtExistence.Text + "', ";
                SQL = SQL + ComNum.VBLF + "        JIKUP   = '" + VB.Left(cboJob.Text, 2).Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "        DIDATE  = TO_DATE('" + dtpDateDeath.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "        FREE    = '" + txtNote.Text + "', ";
                SQL = SQL + ComNum.VBLF + "        MGTREAT = '" + txtEtc.Text + "' ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + VB.Format(VB.Val(txtNo.Text), "00000000") + "' ";
                SQL = SQL + ComNum.VBLF + "    AND OUTDATE = TO_DATE('" + dtpDateHospital.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (intRowAffected == -1)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    //MfrmMain.PanelMsg.Caption = " Data 처리 실패 !!! "  TODO: MfrmMain.PanelMsg.Caption form생성 필요
                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    //MfrmMain.PanelMsg.Caption = " Data 처리 완료 !!! "  TODO: MfrmMain.PanelMsg.Caption form생성 필요
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        //TODO: MfrmMain.PanelMsg.Caption form생성 필요
        void Insert_Mid_Patient()
        {

            string strt1code1 = "";
            string strt1code2 = "";
            string strt1code3 = "";
            string strm1code1 = "";
            string strm1code2 = "";
            string strm1code3 = "";
            string strHo = ""; //호르몬요법
            string strCare = ""; //치료시행여부
            int intAge = 0;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            strHo = "0";

            if (chkMedical6.Checked == true)
            {
                strHo = "1";
            }

            strCare = "0";

            if (rdoCare0.Checked == true)
            {
                strCare = "1";
            }

            clsDB.setBeginTran(clsDB.DbCon);

            ssView1_Sheet1.Rows.Count = 3;
            ssView2_Sheet1.Rows.Count = 3;

            intAge = ComFunc.AgeCalcEx(txtJumin1.Text + txtJumin2.Text, dtpDateHospital.Value.ToString("yyyy-MM-dd"));

            strt1code1 = VB.Left(ssView1_Sheet1.Cells[0, 0].Text, 6).Trim();
            strt1code2 = VB.Left(ssView1_Sheet1.Cells[1, 0].Text, 6).Trim();
            strt1code3 = VB.Left(ssView1_Sheet1.Cells[2, 0].Text, 6).Trim();

            strm1code1 = VB.Left(ssView2_Sheet1.Cells[0, 0].Text, 6).Trim();
            strm1code2 = VB.Left(ssView2_Sheet1.Cells[1, 0].Text, 6).Trim();
            strm1code3 = VB.Left(ssView2_Sheet1.Cells[2, 0].Text, 6).Trim();

            try
            {
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MID_CANCER ";
                SQL = SQL + ComNum.VBLF + "     (PANO, SNAME, JUMIN1, JUMIN2, CYY, CNO, INDATE, OUTDATE, BJUSO, JUSO, ";
                SQL = SQL + ComNum.VBLF + "     JICODE, MTREAT,MGTREAT, T1CODE1, T1CODE2, T1CODE3, Age, Sex, M1CODE1, M1CODE2, M1CODE3, ";
                SQL = SQL + ComNum.VBLF + "     TRDATE, JTREAT, SILSU, JIKUP, DIDATE, FREE, GbCare, mtreat_ho, JUMIN3) ";
                SQL = SQL + ComNum.VBLF + "VALUES ";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         '" + VB.Format(VB.Val(txtNo.Text), "00000000") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtName.Text + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + VB.Format(VB.Val(txtJumin1.Text), "000000") + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + VB.Left(txtJumin2.Text, 1) + "******', ";
                SQL = SQL + ComNum.VBLF + "         '" + dtpCyy.Value.Year + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtCancerNo.Text + "', ";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpDatehospialaization.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpDateHospital.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "         '" + txtHome.Text + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtAddress.Text + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + VB.Mid(cboArea.Text, 1, 2) + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtCure.Text + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtEtc.Text + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strt1code1 + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strt1code2 + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strt1code3 + "', ";
                SQL = SQL + ComNum.VBLF + "         " + intAge + ", ";
                SQL = SQL + ComNum.VBLF + "         '" + VB.Left(txtJumin2.Text, 1) + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strm1code1 + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strm1code2 + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strm1code3 + "', ";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpTrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboDiagnisis.Text, 1) + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtExistence.Text + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboJob.Text, 2).Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpDateDeath.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "         '" + txtNote.Text + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strCare + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strHo + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + clsAES.AES(txtJumin2.Text) + "' ";
                SQL = SQL + ComNum.VBLF + "     )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }


                if (intRowAffected == -1)
                {
                    //    MfrmMain.PanelMsg.Caption = " Data 처리 완료 !!! "  TODO: MfrmMain.PanelMsg.Caption form생성 필요
                    clsDB.setCommitTran(clsDB.DbCon);
                    return;
                }
                else
                {
                    //    MfrmMain.PanelMsg.Caption = " Data 처리 실패 !!! "  TODO: MfrmMain.PanelMsg.Caption form생성 필요
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            Init();

            ssView1_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 0;
            txtNo.Focus();
        }

        void chkMedical_CheckedChanged(object sender, EventArgs e)
        {
            int intval = 0;

            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {
                if (ctl is CheckBox)
                {
                    if (VB.Left(((CheckBox)ctl).Name, 10) == "chkMedical")
                    {
                        if (((CheckBox)ctl).Checked == true)
                        {
                            intval = intval + 1;
                        }
                    }
                }
            }

            txtCure.Text = intval.ToString();
        }

        void chkMedical_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                return;
            }
        }

        void cboArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dtpDatehospialaization.Focus();
                return;
            }
        }

        void cboJob_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                return;
            }
        }

        void cboDiagnisis_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                return;
            }
        }

        void dtpDateDeath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                return;
            }
        }

        void dtpDatehospialaization_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                return;
            }
        }

        void dtpDateHospital_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                return;
            }
        }

        //TODO: MfrmMain form필요
        void txtNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                int j = 0;
                int k = 0;
                string strDiagnosis = "";
                string strjob = "";
                string strCure = "";
                string strArea = "";
                string strJumin = "";
                string strJumin1 = "";

                string SQL = "";
                string SqlErr = "";
                DataTable dt = null;
                DataTable dt1 = null;
                DataTable dt2 = null;

                if (e.KeyCode == Keys.Enter)
                {
                    txtNo.Text = VB.Format(VB.Val(txtNo.Text), "00000000");

                    if (txtNo.Text.Trim() == "")
                    {
                        return;
                    }

                    if (VB.IsNumeric(txtNo.Text) == false)
                    {
                        return;
                    }

                    //MfrmMain.PanelMsg.Caption = ""

                    try
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT * FROM MID_CANCER                                              ";
                        SQL = SQL + ComNum.VBLF + "  WHERE Pano='" + txtNo.Text + "'                                      ";
                        SQL = SQL + ComNum.VBLF + "    AND OUTDATE = TO_DATE('" + dtpDateHospital.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            SQL = "";
                            SQL = " SELECT SName, Jumin1, Jumin2, BJuso, Juso, JiCode, TO_CHAR(TrDate,'YYYY-mm-DD') Trdate,";
                            SQL = SQL + ComNum.VBLF + "CNO,   MTREAT, MGTREAT,JTREAT, SILSU, JIKUP, CYY, FREE,";
                            SQL = SQL + ComNum.VBLF + "T1CODE1, T1CODE2, T1CODE3,";
                            SQL = SQL + ComNum.VBLF + "M1CODE1, M1CODE2, M1CODE3,";
                            SQL = SQL + ComNum.VBLF + "TO_CHAR(INDATE,'YYYY-MM-DD') IDATE, TO_CHAR(OUTDATE,'YYYY-MM-DD') ODATE,";
                            SQL = SQL + ComNum.VBLF + "TO_CHAR(DIDATE,'YYYY-MM-DD') DDATE,GbCare,mtreat_ho, JUMIN3";
                            SQL = SQL + ComNum.VBLF + "FROM MID_CANCER";
                            SQL = SQL + ComNum.VBLF + "WHERE Outdate = TO_DATE('" + dtpDateHospital.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "AND PANO = '" + txtNo.Text + "'";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                dt1.Dispose();
                                dt1 = null;
                                ComFunc.MsgBox("해당 DATA가 없습니다.");
                                return;
                            }

                            ssView1_Sheet1.RowCount = 3;
                            ssView2_Sheet1.RowCount = 3;

                            txtName.Text = dt1.Rows[0]["SName"].ToString().Trim();
                            strJumin = VB.Mid(dt1.Rows[0]["Jumin1"].ToString().Trim(), 1, 6);
                            txtJumin1.Text = strJumin;
                            strJumin1 = clsAES.DeAES(dt1.Rows[0]["Jumin3"].ToString().Trim());
                            txtJumin2.Text = strJumin1;
                            txtAddress.Text = dt1.Rows[0]["Juso"].ToString().Trim();
                            strArea = dt1.Rows[0]["JICODE"].ToString().Trim();

                            for (k = 0; k < cboArea.Items.Count; k++)
                            {
                                if (VB.Mid(cboArea.Items[k].ToString(), 1, 2) == strArea)
                                {
                                    cboArea.SelectedIndex = k;
                                    break;
                                }
                            }

                            txtCancerNo.Text = dt1.Rows[0]["CNO"].ToString().Trim();
                            dtpCyy.Value = Convert.ToDateTime(dt1.Rows[0]["CYY"].ToString().Trim() + "-01-01");
                            dtpDatehospialaization.Value = Convert.ToDateTime(dt1.Rows[0]["IDATE"].ToString().Trim());
                            dtpDateHospital.Value = Convert.ToDateTime(dt1.Rows[0]["ODATE"].ToString().Trim());
                            dtpTrDate.Value = Convert.ToDateTime(dt1.Rows[0]["Trdate"].ToString().Trim());
                            strCure = dt1.Rows[0]["MTREAT"].ToString().Trim();

                            if (strCure != "")
                            {
                                Control[] controls = ComFunc.GetAllControls(this);

                                foreach (Control ctl in controls)
                                {
                                    if (ctl is CheckBox)
                                    {
                                        if (VB.Left(((CheckBox)ctl).Name, 10) == "chkMedical")
                                        {
                                            ((CheckBox)ctl).Checked = true;
                                        }
                                    }
                                }
                            }

                            //호르몬요법 2010-03-15
                            chkMedical6.Checked = false;

                            if (dt1.Rows[0]["mtreat_ho"].ToString().Trim() == "1")
                            {
                                chkMedical6.Checked = true;
                            }

                            txtEtc.Text = dt1.Rows[0]["MGTREAT"].ToString().Trim();
                            strDiagnosis = VB.Left(dt1.Rows[0]["JTREAT"].ToString().Trim(), 1);

                            for (j = 0; j < VB.Val(strDiagnosis); j++)
                            {
                                cboDiagnisis.SelectedIndex = Convert.ToInt32(strDiagnosis) - 1;

                                if (VB.Left(cboDiagnisis.Items[j].ToString(), 1) == strDiagnosis)
                                {
                                    cboDiagnisis.SelectedIndex = j;
                                    break;
                                }
                            }

                            txtExistence.Text = dt1.Rows[0]["SILSU"].ToString();
                            strjob = VB.Pstr(dt1.Rows[0]["JIKUP"].ToString().Trim(), ".", 1);
                            cboJob.Text = cboJob_Select(strjob);
                            dtpDateDeath.Text = dt1.Rows[0]["DDATE"].ToString().Trim();
                            txtNote.Text = dt1.Rows[0]["FREE"].ToString().Trim();

                            ssView1_Sheet1.Cells[0, 0].Text = dt1.Rows[0]["T1CODE1"].ToString().Trim();
                            ssView1_Sheet1.Cells[1, 0].Text = dt1.Rows[0]["T1CODE2"].ToString().Trim();
                            ssView1_Sheet1.Cells[2, 0].Text = dt1.Rows[0]["T1CODE3"].ToString().Trim();

                            ssView1_Sheet1.Cells[0, 1].Text = ILL_LOAD(dt1.Rows[0]["T1CODE1"].ToString().Trim());
                            ssView1_Sheet1.Cells[1, 1].Text = ILL_LOAD(dt1.Rows[0]["T1CODE2"].ToString().Trim());
                            ssView1_Sheet1.Cells[2, 1].Text = ILL_LOAD(dt1.Rows[0]["T1CODE3"].ToString().Trim());

                            ssView2_Sheet1.Cells[0, 0].Text = dt1.Rows[0]["M1CODE1"].ToString().Trim();
                            ssView2_Sheet1.Cells[1, 0].Text = dt1.Rows[0]["M1CODE2"].ToString().Trim();
                            ssView2_Sheet1.Cells[2, 0].Text = dt1.Rows[0]["M1CODE3"].ToString().Trim();

                            ssView2_Sheet1.Cells[0, 1].Text = ILL_LOAD(dt.Rows[0]["M1CODE1"].ToString().Trim());
                            ssView2_Sheet1.Cells[1, 1].Text = ILL_LOAD(dt.Rows[0]["M1CODE2"].ToString().Trim());
                            ssView2_Sheet1.Cells[2, 1].Text = ILL_LOAD(dt.Rows[0]["M1CODE3"].ToString().Trim());


                            //치료시행여부
                            if (dt1.Rows[0]["GbCare"].ToString().Trim() == "1")
                            {
                                rdoCare0.Checked = true;
                            }
                            else
                            {
                                rdoCare1.Checked = true;
                            }

                            txtAddress.Focus();

                            dt1.Dispose();
                            dt1 = null;
                        }
                        else
                        {
                            SQL = "";
                            SQL = " SELECT Pano, SNAME, JUMIN1, JUMIN2, JUMIN3, JICODE,               ";
                            SQL = SQL + ComNum.VBLF + "        ZipName1, ZipName2,ZipName3, JUSO, BI              ";
                            SQL = SQL + ComNum.VBLF + "   FROM BAS_PATIENT A, BAS_ZIPS B                          ";
                            SQL = SQL + ComNum.VBLF + "  WHERE A.PANO= '" + VB.Format(txtNo.Text, "00000000") + "'   ";
                            SQL = SQL + ComNum.VBLF + "    AND A.ZipCode1 = B.ZipCode1(+)                         ";
                            SQL = SQL + ComNum.VBLF + "    AND A.ZipCode2 = B.ZipCode2(+)                         ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                dt1.Dispose();
                                dt1 = null;
                                ComFunc.MsgBox("해당 DATA가 없습니다.");
                                return;
                            }

                            //MfrmMain.PanelMsg.Caption = ""

                            if (dt1.Rows.Count != 0)
                            {
                                txtName.Text = dt1.Rows[0]["SNAME"].ToString().Trim();
                                strJumin = VB.Mid(dt1.Rows[0]["JUMIN1"].ToString().Trim(), 1, 6);
                                txtJumin1.Text = strJumin;
                                strJumin1 = clsAES.DeAES(dt1.Rows[0]["JUMIN3"].ToString().Trim());
                                txtJumin2.Text = strJumin1;
                                txtAddress.Text = dt1.Rows[0]["ZipName1"].ToString().Trim() + "  " + dt1.Rows[0]["ZipName2"].ToString().Trim();
                                txtAddress.Text = txtAddress.Text + " " + dt1.Rows[0]["ZipName3"].ToString().Trim() + " " + dt1.Rows[0]["JUSO"].ToString().Trim();
                                strArea = dt1.Rows[0]["JICODE"].ToString().Trim();

                                for (k = 0; k < cboArea.Items.Count; k++)
                                {
                                    if (VB.Mid(cboArea.Items[k].ToString(), 1, 2) == strArea)
                                    {
                                        cboArea.SelectedIndex = k;
                                        break;
                                    }
                                }

                                SQL = "";
                                SQL = " SELECT Cno  FROM MID_CANCER   ";
                                SQL = SQL + ComNum.VBLF + "  ORDER BY Cno desc            ";

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }
                                if (dt2.Rows.Count == 0)
                                {
                                    dt2.Dispose();
                                    dt2 = null;
                                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                                    return;
                                }

                                txtCancerNo.Text = (Convert.ToInt32(dt2.Rows[0]["Cno"]) + 1).ToString();
                                dtpCyy.Text = VB.Left(dtpDateHospital.Text, 4);

                                dt2.Dispose();
                                dt2 = null;

                                txtAddress.Focus();

                                //MfrmMain.PanelMsg.Caption = "신규 암 환자 입니다 ! "
                            }
                            else
                            {
                                //Beep
                                if (txtNo.Text != dt1.Rows[0]["Pano"].ToString().Trim())
                                {
                                    txtNo.Focus();

                                    //MfrmMain.PanelMsg.Caption = "신환환자 입니다.  전산실에 문의 하십시오 !!";   //환자MASTER에 Data가 없슴 해당환자 입력 요망 !!
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        dt.Dispose();
                        dt = null;

                        SQL = "";
                        SQL = " SELECT TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE ";
                        SQL = SQL + ComNum.VBLF + " FROM MID_SUMMARY ";
                        SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + VB.Format(VB.Val(txtNo.Text), "00000000") + "'      ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY OUTDATE DESC ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            txtInfo.Text = "입원일자 : " + dt.Rows[0]["INDATE"].ToString().Trim() + " " + "퇴원일자 : " + dt.Rows[0]["OUTDATE"].ToString().Trim();
                        }

                        dt.Dispose();
                        dt = null;
                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }

                    return;
                }
            }
        }

        string cboJob_Select(string strAtg)
        {
            string strVal = "";

            switch (strAtg)
            {
                case "1":
                    strVal = "1.전문기술직";
                    break;
                case "2":
                    strVal = "2.행정직";
                    break;
                case "3":
                    strVal = "3.사무직";
                    break;
                case "4":
                    strVal = "4.판매종사자";
                    break;
                case "5":
                    strVal = "5.서비스직";
                    break;
                case "6":
                    strVal = "6.농업,축산업,임업,수산업,수렵업";
                    break;
                case "7":
                    strVal = "7.생산직";
                    break;
                case "8":
                    strVal = "8.운수업";
                    break;
                case "9":
                    strVal = "9.단순노무자";
                    break;
                case "10":
                    strVal = "10.분류불능자";
                    break;
                case "11":
                    strVal = "11.학생";
                    break;
                case "12":
                    strVal = "12.군인";
                    break;
                case "13":
                    strVal = "13.주부";
                    break;
                case "14":
                    strVal = "14.기타";
                    break;
                case "99":
                    strVal = "99.직업모름";
                    break;
            }

            return strVal;
        }

        string ILL_LOAD(string strArg)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strVal = "";

            try
            {
                SQL = "";
                SQL = " SELECT IllCode, IllNameE FROM BAS_ILLS      ";
                SQL = SQL + ComNum.VBLF + " WHERE IllCode = '" + strArg.Trim() + "'";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return strVal;
                }

                if (dt.Rows.Count == 1)
                {
                    strVal = dt.Rows[0]["IllNameE"].ToString().Trim();
                }
                else
                {
                    strVal = "";
                }

                dt.Dispose();
                dt = null;

                return strVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strVal;
            }
        }

        private void txtNote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                return;
            }
        }

        private void ssView2_EditModeOff(object sender, EventArgs e)
        {
            string strIllcode1 = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (ssView2_Sheet1.ActiveColumnIndex != 0)
            {
                return;
            }

            strIllcode1 = ssView2_Sheet1.ActiveCell.Text.Trim();

            if (strIllcode1 == "")
            {
                ssView2_Sheet1.ActiveCell.Text = "";
                //SS2.Col = Col + 1
                //SS2.Text = ""
                return;
            }

            try
            {
                SQL = "";
                SQL = "SELECT IllCode, IllNameE";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_ILLS";
                SQL = SQL + ComNum.VBLF + " WHERE IllCode = '" + strIllcode1 + "' ";
                SQL = SQL + ComNum.VBLF + "   AND IllCode >=  'M000'";
                SQL = SQL + ComNum.VBLF + "   AND IllCode <=  'M9999'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("해당 상병 없음 확인후 작업", "주 의");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssView2_Sheet1.ActiveCell.Text = "";
                ssView2_Sheet1.Cells[ssView2_Sheet1.ActiveRowIndex, 1].Text = "";

                ssView2_Sheet1.ActiveCell.Text = dt.Rows[0]["IllNameE"].ToString();
                ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssView1_EditModeOff(object sender, EventArgs e)
        {
            string strIllcode = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (ssView1_Sheet1.ActiveColumnIndex != 0)
            {
                return;
            }

            strIllcode = ssView1_Sheet1.ActiveCell.Text;

            if (strIllcode == "")
            {
                ssView1_Sheet1.ActiveCell.Text = "";
                ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 1].Text = "";
                return;
            }

            try
            {
                SQL = "";
                SQL = "SELECT IllCode, IllNameE";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_ILLS";
                SQL = SQL + ComNum.VBLF + " WHERE IllCode = '" + strIllcode + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 상병 없음 확인후 작업", "주 의");

                    ssView1_Sheet1.ActiveCell.Text = "";
                    ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 1].Text = "";

                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 1].Text = dt.Rows[0]["IllNameE"].ToString().Trim();

                ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                dt.Dispose();
                dt = null;

                return;
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssView2_KeyDown(object sender, KeyEventArgs e)
        {
            string strIlls = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strIlls = ssView2_Sheet1.ActiveCell.Text.Trim();
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SQL = "";
                    SQL = "SELECT IllCode, IllNameE";
                    SQL = SQL + ComNum.VBLF + "  FROM BAS_ILLS";
                    SQL = SQL + ComNum.VBLF + " WHERE IllCode = '" + strIlls + "'";
                    SQL = SQL + ComNum.VBLF + "   AND IllCode >=  'M000'";
                    SQL = SQL + ComNum.VBLF + "   AND IllCode <=  'M9999'";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 상병 없음 확인후 작업", "주 의");

                        ssView2_Sheet1.ActiveCell.Text = "";
                        ssView2_Sheet1.Cells[ssView2_Sheet1.ActiveRowIndex, 1].Text = "";
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    ssView2_Sheet1.Cells[ssView2_Sheet1.ActiveRowIndex, 1].Text = dt.Rows[0]["IllNameE"].ToString();

                    ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    dt.Dispose();
                    dt = null;
                }
                strIlls = "";
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
                Cursor.Current = Cursors.Default;
            }

        }

        private void ssView1_KeyDown(object sender, KeyEventArgs e)
        {
            string strIlls = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strIlls = ssView1_Sheet1.ActiveCell.Text.Trim();
            try
            {
                if (e.KeyCode == Keys.Enter)
                {



                    SQL = "";
                    SQL = "SELECT IllCode, IllNameE";
                    SQL = SQL + ComNum.VBLF + "  FROM BAS_ILLS";
                    SQL = SQL + ComNum.VBLF + " WHERE IllCode = '" + strIlls + "'";
                    SQL = SQL + ComNum.VBLF + "   AND IllCode >=  'C000'";
                    SQL = SQL + ComNum.VBLF + "   AND IllCode <=  'C9999'";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 상병 없음 확인후 작업", "주 의");

                        ssView1_Sheet1.ActiveCell.Text = "";
                        ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 1].Text = "";
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 1].Text = dt.Rows[0]["IllNameE"].ToString().Trim();

                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    dt.Dispose();
                    dt = null;
                }

                strIlls = "";

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
                Cursor.Current = Cursors.Default;
            }
        }
    }
}