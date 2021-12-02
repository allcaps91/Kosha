using ComBase;
using ComDbB;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmInfoEntrySimple.cs
    /// Description     : 약품 정보 조회 - Simple
    /// Author          : 
    /// Create Date     : 2019-07-26
    /// <history> 
    /// 약품 정보 조회 - Simple
    /// </history>
    /// <seealso>
    /// PSMH\drug\drinfo\Drinfo01Simple.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drinfo\drinfo.vbp
    /// </vbp>
    /// </summary>
    public partial class frmInfoEntrySimple : Form
    {
        private string GstrROWID     = string.Empty;
        private string GstrImageFile = string.Empty;
        private string GstrImageYN   = string.Empty;
        private string GstrPreInput  = string.Empty;
        private string GstrDrugCode  = string.Empty;
        private string GstrYAKPUM_CD = string.Empty;

        public frmInfoEntrySimple()
        {
            InitializeComponent();
        }

        public frmInfoEntrySimple(string strDrugCode)
        {
            InitializeComponent();
            
            GstrDrugCode = strDrugCode;            
        }

        private void frmInfoEntrySimple_Load(object sender, EventArgs e)
        {
            ////폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("ClinicalRef");

            if (ProcessEx.Length > 0)
            {
                System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("ClinicalRef");
                System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                foreach (System.Diagnostics.Process Proc in Pro1)
                {
                    if (Proc.Id != CurPro.Id)
                    {
                        ComFunc.KillProc("ClinicalRef");
                    }
                }
            }


            panText.Visible = false;
            panText.Dock = DockStyle.None;

            panWeb.Visible = true;
            panWeb.Dock = DockStyle.Fill;

            SetTextBox();
            SetTextBoxSub();
            SetWebBrowser();

            //ssGubunCellClick(1, 5);
            ssGubunWebCellClick(0, 0);

            cboJong.Items.Clear();
            cboJong.Items.Add("");
            cboJong.Items.Add("10.경구약");
            cboJong.Items.Add("20.주사약");
            cboJong.Items.Add("31.외용약");
            cboJong.Items.Add("32.기타 외용약");
            cboJong.Items.Add("33.안약");
            cboJong.Items.Add("34.가글제");
            cboJong.Items.Add("35.패취제");
            cboJong.Items.Add("36.분무제");
            cboJong.Items.Add("37.항문좌제");
            cboJong.Items.Add("38.부인과 정제");
            cboJong.Items.Add("41.마취약");
            cboJong.Items.Add("42.방사선용제");
            cboJong.Items.Add("43.인공신장관류용제");
            cboJong.Items.Add("44.투약관련 소모품");

            SET_CAUTION();
            
            cboDrug.Items.Clear();
            cboDrug.Items.Add("");
            cboDrug.Items.Add("01.전문약");
            cboDrug.Items.Add("02.일반약");
            cboDrug.Items.Add("03.희귀약");
            cboDrug.Items.Add("04.전문약/희귀의약품");
            cboDrug.Items.Add("05.전문약/향정신성의약품");
            cboDrug.Items.Add("06.전문약/마약");

            cboPowder.Items.Clear();
            cboPowder.Items.Add("0.불가능");
            cboPowder.Items.Add("1.가능");
            cboPowder.Items.Add("");

            SCREEN_CLEAR();
            
            if (ChkYakuk(clsType.User.Sabun) == false)
            {
                string SQL = "";
                DataTable dt = null;
                string SqlErr = "";
                
                lblTitleSub0.Text = "약품 정보 등록";

                try
                {
                    //심사계는 보험관련정보만 등록 할수 있도록 함.
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SABUN";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST";
                    SQL = SQL + ComNum.VBLF + "     WHERE BUSE IN ('078201', '077501') ";
                    SQL = SQL + ComNum.VBLF + "         AND SABUN = '" + ComFunc.LPAD(clsType.User.Sabun, 5, "0") + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        lblTitleSub0.Text = "약품 정보 조회";
                        panCancel.Visible = false;

                        Control[] controls = ComFunc.GetAllControls(this);
                        foreach (Control ctl in controls)
                        {
                            if (ctl is TextBox)
                            {
                                if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                                {
                                    ((TextBox)ctl).Visible = false;
                                    ((TextBox)ctl).Dock = DockStyle.None;
                                }
                            }

                            if (ctl is WebBrowser)
                            {
                                if (VB.Left(((WebBrowser)ctl).Name, 7) == "webInfo")
                                {
                                    ((WebBrowser)ctl).Visible = false;
                                    ((WebBrowser)ctl).Dock = DockStyle.None;
                                }
                            }
                        }

                        txtInfo10.Visible = true;

                        //txtBunCode.Enabled = false;
                        //txtDrugHname.Enabled = false;
                        //txtDrugEname.Enabled = false;
                        //txtJeyak.Enabled = false;
                        //txtJeheng.Enabled = false;
                        //txtJeheng2.Enabled = false;
                        //txtJeheng31.Enabled = false;
                        //txtJeheng32.Enabled = false;
                        //txtDrugSname.Enabled = false;
                        //txtUnit.Enabled = false;
                        //cboJong.Enabled = false;
                        //txtDrugHoo.Enabled = false;

                        txtBunCode.ReadOnly = true;
                        txtDrugHname.ReadOnly = true;
                        txtDrugEname.ReadOnly = true;
                        txtJeyak.ReadOnly = true;
                        txtJeheng.ReadOnly = true;
                        txtJeheng2.ReadOnly = true;
                        txtJeheng31.ReadOnly = true;
                        txtJeheng32.ReadOnly = true;
                        txtDrugSname.ReadOnly = true;
                        txtUnit.ReadOnly = true;
                        txtDrugHoo.ReadOnly = true;
                    }
                    else
                    {
                        panCancel.Visible = true;

                        Control[] controls = ComFunc.GetAllControls(this);
                        foreach (Control ctl in controls)
                        {
                            if (ctl is TextBox)
                            {
                                if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                                {
                                    ((TextBox)ctl).Visible = false;
                                    ((TextBox)ctl).Dock = DockStyle.None;
                                }
                            }

                            if (ctl is WebBrowser)
                            {
                                if (VB.Left(((WebBrowser)ctl).Name, 7) == "webInfo")
                                {
                                    ((WebBrowser)ctl).Visible = false;
                                    ((WebBrowser)ctl).Dock = DockStyle.None;
                                }
                            }
                        }

                        txtInfo10.Visible = true;

                        //txtBunCode.Enabled = false;
                        //txtDrugHname.Enabled = false;
                        //txtDrugEname.Enabled = false;
                        //txtJeyak.Enabled = false;
                        //txtJeheng.Enabled = false;
                        //txtJeheng2.Enabled = false;
                        //txtJeheng31.Enabled = false;
                        //txtJeheng32.Enabled = false;
                        //txtDrugSname.Enabled = false;
                        //txtUnit.Enabled = false;
                        //cboJong.Enabled = false;
                        //txtDrugHoo.Enabled = false;

                        txtBunCode.ReadOnly = true;
                        txtDrugHname.ReadOnly = true;
                        txtDrugEname.ReadOnly = true;
                        txtJeyak.ReadOnly = true;
                        txtJeheng.ReadOnly = true;
                        txtJeheng2.ReadOnly = true;
                        txtJeheng31.ReadOnly = true;
                        txtJeheng32.ReadOnly = true;
                        txtDrugSname.ReadOnly = true;
                        txtUnit.ReadOnly = true;
                        txtDrugHoo.ReadOnly = true;
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

                    ComFunc.MsgBoxEx(this, ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (GstrDrugCode != "")
                {
                    Screen_display(GstrDrugCode.Trim());
                }
            }

            if (GstrDrugCode != "")
            {
                txtSuNext.Text = GstrDrugCode;
                txtSuNextKeyDown();
            }
        }

        private bool ChkYakuk(string strSabun)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "SELECT* FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_ERP + "INSA_CODE B";
                SQL = SQL + ComNum.VBLF + "    WHERE A.SABUN = '" + strSabun + "'";
                SQL = SQL + ComNum.VBLF + "        AND B.GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "        AND B.CODE IN ('13', '40', '41', '42', '43')";
                SQL = SQL + ComNum.VBLF + "        AND A.JIK = TRIM(B.CODE)";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
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

                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string READ_BAS_Class(PsmhDb pDbCon, string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            if (strCode.Trim() == "") { return rtnVal; }

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     ClassName";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLASS";
                SQL = SQL + ComNum.VBLF + "     WHERE ClassCode = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["ClassName"].ToString().Trim();
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

                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return rtnVal;
            }
        }

        private void SET_CAUTION()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboCaution.Items.Clear();
            cboCaution.Items.Add("");

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     CODE, NAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'DRUG_취급주의' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY CODE ASC ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboCaution.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
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

                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
        private void SetTextBox()
        {
            Control[] controls = ComFunc.GetAllControls(this);
            foreach (Control ctl in controls)
            {
                if (ctl is TextBox)
                {
                    if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                    {
                        if (VB.Val(VB.Right(((TextBox)ctl).Name, 2)) <= 11)
                        {
                            ((TextBox)ctl).Visible = false;
                            ((TextBox)ctl).Dock = DockStyle.None;
                        }
                    }
                }
            }
        }

        private void SetTextBoxSub()
        {
            Control[] controls = ComFunc.GetAllControls(this);
            foreach (Control ctl in controls)
            {
                if (ctl is TextBox)
                {
                    if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                    {
                        if (VB.Val(VB.Right(((TextBox)ctl).Name, 2)) >= 12)
                        {
                            ((TextBox)ctl).Visible = false;
                            ((TextBox)ctl).Dock = DockStyle.None;
                        }
                    }
                }
            }
        }

        private void SetWebBrowser()
        {
            Control[] controls = ComFunc.GetAllControls(this);

            txtInfo10.Visible = false;
            txtInfo10.Dock = DockStyle.None;

            foreach (Control ctl in controls)
            {
                if (ctl is WebBrowser)
                {
                    if (VB.Left(((WebBrowser)ctl).Name, 7) == "webInfo")
                    {
                        if (VB.Val(VB.Right(((WebBrowser)ctl).Name, 2)) <= 11)
                        {
                            ((WebBrowser)ctl).Visible = false;
                            ((WebBrowser)ctl).Dock = DockStyle.None;
                        }
                    }
                }
            }
        }

        private void SCREEN_CLEAR()
        {
            GstrPreInput = "";

            txtSuNext.Text = "";
            txtBunCode.Text = "";
            txtBCode.Text = "";
            txtDrugHname.Text = "";
            txtDrugEname.Text = "";
            txtJeyak.Text = "";
            txtDrugSname.Text = "";
            txtUnit.Text = "";
            cboJong.Text = "";
            txtYakGbn.Text = "";
            txtBAmt.Text = "";
            txtDelDate.Text = "";
            txtDrugHoo.Text = "";
            txtDate.Text = "";
            txtEntDate.Text = "";
            txtBI.Text = "";
            txtSaveTemp.Text = "";
            txtSaveBright.Text = "";
            txtNotPowder1.Text = "";
            txtNotPowder2.Text = "";

            lblMEMO.Text = "";
            lblMEMO.BackColor = Color.White;

            cboCaution.Text = "";
            chkMetformin.Checked = false;

            Control[] controls = ComFunc.GetAllControls(this);
            foreach (Control ctl in controls)
            {
                if (ctl is TextBox)
                {
                    if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                    {
                        ((TextBox)ctl).Text = "";
                    }
                }

                if (ctl is WebBrowser)
                {
                    if (VB.Left(((WebBrowser)ctl).Name, 7) == "webInfo")
                    {
                        ((WebBrowser)ctl).DocumentText = "";
                    }
                }
            }

            Application.DoEvents();

            txtBunName.Text = "";
            GstrROWID = "";
            GstrImageFile = "";
            GstrImageYN = "";
            cboDrug.Text = "";
            picPhoto.Visible = false;

            GstrPreInput = "";

            cboPowder.Text = "";

            txtJeheng.Text = "";
            txtJeheng2.Text = "";
            txtJeheng31.Text = "";
            txtJeheng32.Text = "";

            panCancel.Enabled = false;
            panExit.Enabled = true;
        }

        private void Screen_display(string strCode, string strGUBUN = "")
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;

            string strRemark = "";
            string strAllRemark = "";
            string strAllRemarkSub = "";
            string strPreJEPCODE = "";

            strPreJEPCODE = strCode;

            SCREEN_CLEAR();

            Control[] controls = ComFunc.GetAllControls(this);

            try
            {
                // 2019-07-01 약제팀 데레사 수녀님 요청으로 수가코드 등록안된 코드는 조회 안함
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     a.SuNext, a.BAmt, a.SugbJ, b.BCode, b.DaiCode, b.SuNameK, B.SUHAM, B.SUGBP, ";
                SQL = SQL + ComNum.VBLF + "     b.SuNameE, b.SugbO, TO_CHAR(a.DelDate,'YYYY-MM-DD') AS DelDate, B.SUNAMEG ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUT a, " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL = SQL + ComNum.VBLF + "     WHERE a.SuNext = '" + strCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND a.SuNext = b.SUNEXT ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                dt.Dispose();
                dt = null;


                //자료를 READ
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SuNext, Jong, BunCode, HName, EName, SName ,Unit, JeHeng, JEHENG2, JEHENG3_1, JEHENG3_2, Jeyak, EffEct, ";
                SQL = SQL + ComNum.VBLF + "     Remark011, Remark012, Remark013, Remark014, Remark015, ";
                SQL = SQL + ComNum.VBLF + "     Remark021, Remark022, Remark023, Remark024, Remark025, ";
                SQL = SQL + ComNum.VBLF + "     Remark031, Remark032, Remark033, Remark034, Remark035, ";
                SQL = SQL + ComNum.VBLF + "     Remark041, Remark042, Remark043, Remark044, Remark045, ";
                SQL = SQL + ComNum.VBLF + "     Remark051, Remark052, Remark053, Remark054, Remark055, ";
                SQL = SQL + ComNum.VBLF + "     Remark061, Remark062, Remark063, Remark064, Remark065, ";
                SQL = SQL + ComNum.VBLF + "     Remark071, Remark072, Remark073, Remark074, Remark075, ";
                SQL = SQL + ComNum.VBLF + "     Remark081, Remark082, Remark083, Remark084, Remark085, ";
                SQL = SQL + ComNum.VBLF + "     Remark091, Remark092, Remark093, Remark094, Remark095, ";
                SQL = SQL + ComNum.VBLF + "     Remark101, Remark102, Remark103, Remark104, Remark105, ";
                SQL = SQL + ComNum.VBLF + "     Remark111, Remark112, Remark113, Remark114, Remark115, ";
                SQL = SQL + ComNum.VBLF + "     Remark121, Remark122, Remark123, Remark124, Remark125, ";
                SQL = SQL + ComNum.VBLF + "     Remark131, Remark132, Remark133, Remark134, Remark135, ";
                SQL = SQL + ComNum.VBLF + "     Remark141, Remark142, Remark143, Remark144, Remark145, ";
                SQL = SQL + ComNum.VBLF + "     Remark151, Remark152, Remark153, Remark154, Remark155, ";
                SQL = SQL + ComNum.VBLF + "     Remark161, Remark162, Remark163, Remark164, Remark165, ";
                SQL = SQL + ComNum.VBLF + "     Remark171, Remark172, Remark173, Remark174, Remark175, ";
                SQL = SQL + ComNum.VBLF + "     Remark181, Remark182, Remark183, Remark184, Remark185, ";
                SQL = SQL + ComNum.VBLF + "     Remark191, Remark192, Remark193, Remark194, Remark195, ";
                SQL = SQL + ComNum.VBLF + "     Remark201, Remark202, Remark203, Remark204, Remark205, ";
                SQL = SQL + ComNum.VBLF + "     Remark211, Remark212, Remark213, Remark214, Remark215, ";
                SQL = SQL + ComNum.VBLF + "     Remark221, Remark222, Remark223, Remark224, Remark225, ";
                SQL = SQL + ComNum.VBLF + "     Remark231, Remark232, Remark233, Remark234, Remark235, ";
                SQL = SQL + ComNum.VBLF + "     Remark241, Remark242, Remark243, Remark244, Remark245, ";
                SQL = SQL + ComNum.VBLF + "     Remark251, Remark252, Remark253, Remark254, Remark255, ";
                SQL = SQL + ComNum.VBLF + "     Remark261, Remark262, Remark263, Remark264, Remark265, ";
                SQL = SQL + ComNum.VBLF + "     Remark271, Remark272, Remark273, Remark274, Remark275, ";
                SQL = SQL + ComNum.VBLF + "     Remark281, Remark282, Remark283, Remark284, Remark285, ";
                SQL = SQL + ComNum.VBLF + "     Image_YN, ROWID, DRBUN, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(ENTDATE,'YYYY-MM-DD') AS ENTDATE, TO_CHAR(SDATE,'YYYY-MM-DD') AS SDATE, ";
                SQL = SQL + ComNum.VBLF + "     POWDER, PCLSCODE, CAUTION, CAUTION_STRING, METFORMIN ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW ";
                SQL = SQL + ComNum.VBLF + "     WHERE SuNext = '" + strCode + "'  ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    //자료가 있으면
                    #region GoSub Screen_Display_SUB

                    GstrImageYN = dt.Rows[0]["Image_YN"].ToString().Trim();
                    txtEntDate.Text = dt.Rows[0]["ENTDATE"].ToString().Trim();
                    txtSuNext.Text = dt.Rows[0]["SUNEXT"].ToString().Trim();
                    txtDrugHname.Text = dt.Rows[0]["HName"].ToString().Trim();
                    txtDrugEname.Text = dt.Rows[0]["EName"].ToString().Trim();
                    txtDrugSname.Text = dt.Rows[0]["SName"].ToString().Trim();
                    txtUnit.Text = dt.Rows[0]["Unit"].ToString().Trim();
                    txtBunCode.Text = dt.Rows[0]["BunCode"].ToString().Trim();
                    txtJeyak.Text = dt.Rows[0]["Jeyak"].ToString().Trim();
                    txtJeheng.Text = dt.Rows[0]["Jeheng"].ToString().Trim();
                    txtJeheng2.Text = dt.Rows[0]["Jeheng2"].ToString().Trim();
                    txtJeheng31.Text = dt.Rows[0]["Jeheng3_1"].ToString().Trim();
                    txtJeheng32.Text = dt.Rows[0]["Jeheng3_2"].ToString().Trim();
                    txtDrugHoo.Text = dt.Rows[0]["EffEct"].ToString().Trim();
                    cboCaution.Text = dt.Rows[0]["CAUTION_STRING"].ToString().Trim();
                    chkMetformin.Checked = VB.Val(dt.Rows[0]["METFORMIN"].ToString().Trim()) == 0 ? false : true;
                    txtBunName.Text = READ_BAS_Class(clsDB.DbCon, txtBunCode.Text.Trim());
                    txtDate.Text = dt.Rows[0]["SDATE"].ToString().Trim();
                    
                    if (dt.Rows[0]["JONG"].ToString().Trim() != "")
                    {
                        cboJong.SelectedIndex = 0;
                        ComFunc.ComboFind(cboJong, "L", 2, dt.Rows[0]["JONG"].ToString().Trim());

                        Application.DoEvents();
                    }
                    
                    if (dt.Rows[0]["DRBUN"].ToString().Trim() != "")
                    {
                        cboDrug.SelectedIndex = 0;
                        ComFunc.ComboFind(cboDrug, "L", 2, dt.Rows[0]["DRBUN"].ToString().Trim());

                        Application.DoEvents();
                    }

                    strAllRemark = "";

                    for (i = 1; i <= 28; i++)
                    {
                        if (i == 6 || i == 12 || i == 13) { }
                        else
                        {
                            strRemark = dt.Rows[0]["REMARK" + i.ToString("00") + "1"].ToString().Trim();
                            strRemark = strRemark + dt.Rows[0]["REMARK" + i.ToString("00") + "2"].ToString().Trim();
                            strRemark = strRemark + dt.Rows[0]["REMARK" + i.ToString("00") + "3"].ToString().Trim();
                            strRemark = strRemark + dt.Rows[0]["REMARK" + i.ToString("00") + "4"].ToString().Trim();
                            strRemark = strRemark + dt.Rows[0]["REMARK" + i.ToString("00") + "5"].ToString().Trim();

                            foreach (Control ctl in controls)
                            {
                                if (ctl is TextBox)
                                {
                                    if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                                    {
                                        if (VB.Right(((TextBox)ctl).Name, 2) == (i - 1).ToString("00"))
                                        {
                                            ((TextBox)ctl).Text = strRemark;
                                        }
                                    }
                                }
                            }
                        }

                        if (strRemark != "")
                        {
                            switch (i)
                            {
                                case 1:
                                    strAllRemark = strAllRemark + "【성분/함량】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 2:
                                    strAllRemark = strAllRemark + "【약리작용】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 3:
                                    strAllRemark = strAllRemark + "【약효분류】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 4:
                                    strAllRemark = strAllRemark + "【효능/효과】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 5:
                                    strAllRemark = strAllRemark + "【용법/용량】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 14:
                                    strAllRemark = strAllRemark + "【부작용,금기】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    strAllRemarkSub = strAllRemarkSub + "【부작용,금기】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 15:
                                    strAllRemark = strAllRemark + "【경고】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    strAllRemarkSub = strAllRemarkSub + "【경고】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 16:
                                    strAllRemark = strAllRemark + "【금기】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    strAllRemarkSub = strAllRemarkSub + "【금기】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 17:
                                    strAllRemark = strAllRemark + "【신중투여】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    strAllRemarkSub = strAllRemarkSub + "【신중투여】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 18:
                                    strAllRemark = strAllRemark + "【이상반응】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    strAllRemarkSub = strAllRemarkSub + "【이상반응】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 19:
                                    strAllRemark = strAllRemark + "【일반적 주의】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    strAllRemarkSub = strAllRemarkSub + "【일반적 주의】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 20:
                                    strAllRemark = strAllRemark + "【상호작용】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    strAllRemarkSub = strAllRemarkSub + "【상호작용】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 21:
                                    strAllRemark = strAllRemark + "【임부/소아】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    strAllRemarkSub = strAllRemarkSub + "【임부/소아】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 22:
                                    strAllRemark = strAllRemark + "【임부/수유부】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    strAllRemarkSub = strAllRemarkSub + "【임부/수유부】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 23:
                                    strAllRemark = strAllRemark + "【소아】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    strAllRemarkSub = strAllRemarkSub + "【소아】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 24:
                                    strAllRemark = strAllRemark + "【고령자】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    strAllRemarkSub = strAllRemarkSub + "【고령자】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 25:
                                    strAllRemark = strAllRemark + "【과량 투여시 대처법】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    strAllRemarkSub = strAllRemarkSub + "【과량 투여시 대처법】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 26:
                                    strAllRemark = strAllRemark + "【적용상 주의】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    strAllRemarkSub = strAllRemarkSub + "【적용상 주의】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 27:
                                    strAllRemark = strAllRemark + "【보관 및 취급상 주의】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    strAllRemarkSub = strAllRemarkSub + "【보관 및 취급상 주의】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 28:
                                    strAllRemark = strAllRemark + "【기타】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    strAllRemarkSub = strAllRemarkSub + "【기타】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 7:
                                    strAllRemark = strAllRemark + "【저장방법】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 8:
                                    strAllRemark = strAllRemark + "【약물동력학】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 9:
                                    strAllRemark = strAllRemark + "【복약지도】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 10:
                                    strAllRemark = strAllRemark + "【심사정보】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                                case 11:
                                    strAllRemark = strAllRemark + "【기타】" + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                                    break;
                            }
                        }
                    }

                    txtInfo11.Text = strAllRemark;
                    txtInfo12.Text = strAllRemarkSub;

                    if (dt.Rows[0]["POWDER"].ToString().Trim() != "")
                    {
                        ComFunc.ComboFind(cboPowder, "L", 1, dt.Rows[0]["POWDER"].ToString().Trim());
                    }

                    GstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                    #endregion

                    for (i = 1; i <= 9; i++)
                    {
                        DrugInfoBodyWeb(i.ToString());
                    }
                }

                dt.Dispose();
                dt = null;

                //수가코드의 내역을 Display
                #region GoSub Screen_Display_SUGA   수가코드(표준코드,삭제일자,보험수가,종류)

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     a.SuNext, a.BAmt, a.SugbJ, b.BCode, b.DaiCode, b.SuNameK, B.SUHAM, B.SUGBP, ";
                SQL = SQL + ComNum.VBLF + "     b.SuNameE, b.SugbO, TO_CHAR(a.DelDate,'YYYY-MM-DD') AS DelDate, B.SUNAMEG ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUT a, " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL = SQL + ComNum.VBLF + "     WHERE a.SuNext = '" + strCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND a.SuNext = b.SUNEXT ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    if (txtDrugHname.Text.Trim() == "") { txtDrugHname.Text = dt.Rows[0]["SUNAMEK"].ToString().Trim(); }
                    if (txtDrugEname.Text.Trim() == "") { txtDrugEname.Text = dt.Rows[0]["SUNAMEE"].ToString().Trim(); }
                    if (txtDrugSname.Text.Trim() == "") { txtDrugSname.Text = dt.Rows[0]["SUNAMEG"].ToString().Trim(); }

                    if (txtBunCode.Text.Trim() == "")
                    {
                        txtBunCode.Text = dt.Rows[0]["DAICODE"].ToString().Trim();
                        txtBunName.Text = READ_BAS_Class(clsDB.DbCon, txtBunCode.Text.Trim());
                    }

                    txtSuNext.Text = dt.Rows[0]["SUNEXT"].ToString().Trim();
                    txtBAmt.Text = VB.Val(dt.Rows[0]["BAMT"].ToString().Trim()).ToString("###,###,##0");
                    txtBCode.Text = dt.Rows[0]["BCODE"].ToString().Trim();
                    txtDelDate.Text = dt.Rows[0]["DELDATE"].ToString().Trim();

                    if (txtDelDate.Text.Trim() != "")
                    {
                        lblMEMO.Text = "사용이 중단된 약품코드입니다!";
                        lblMEMO.BackColor = Color.Red;
                    }

                    switch (dt.Rows[0]["SUGBJ"].ToString().Trim())
                    {
                        case "1": txtYakGbn.Text = "1.원외처방전용"; break;
                        case "2": txtYakGbn.Text = "2.입원처방전용"; break;
                        case "3": txtYakGbn.Text = "3.원내외혼용"; break;
                        case "4": txtYakGbn.Text = "4.원내만전용(입원+외래)"; break;
                        default: txtYakGbn.Text = ""; break;
                    }

                    //표준코드의 제약회사명을 Display
                    if (txtJeyak.Text.Trim() == "")
                    {
                        SQL = "";
                        SQL = "SELECT Compny FROM " + ComNum.DB_PMPA + "EDI_SUGA";
                        SQL = SQL + ComNum.VBLF + "     WHERE Code = '" + dt.Rows[0]["BCODE"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            txtJeyak.Text = dt1.Rows[0]["COMPNY"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }
                else
                {
                    ComFunc.MsgBoxEx(this, "보험심사팀에 등록되지 않은 코드입니다.");
                    txtSuNext.Text = strPreJEPCODE;
                    GstrPreInput = "1";
                    
                    //2019-05-10 추가
                    READ_DRUG_MASTER2(strPreJEPCODE);
                }

                dt.Dispose();
                dt = null;

                txtBI.Text = "";

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     GBSELF";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_MASTER2";
                SQL = SQL + ComNum.VBLF + "     WHERE JEPCODE = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["GBSELF"].ToString().Trim())
                    {
                        case "1": txtBI.Text = "급여"; break;
                        case "2": txtBI.Text = "인정비급여"; break;
                        case "3": txtBI.Text = "비급여"; break;
                    }
                }

                dt.Dispose();
                dt = null;

                txtSaveTemp.Text = READ_SAVEPATH(strCode.Trim());
                txtSaveBright.Text = READ_BRIGHTPATH(strCode.Trim());
                txtNotPowder1.Text = READ_NOTPOWDER(strCode.Trim());
                txtNotPowder2.Text = READ_NOTPOWDER_REPLACE_DRUG(strCode.Trim());

                #endregion

                //전체 약품정보
                Remark_All_SET();

                if (GstrImageYN == "Y")
                {
                    //약사진 Display
                    #region GoSub Screen_Display_Image

                    Dir_Check("C:\\PSMHEXE\\YAK_IMAGE\\");

                    string strFile = "";
                    string strHostFile = "";
                    string strHost = "";

                    strFile = "C:\\PSMHEXE\\YAK_IMAGE\\" + strCode.Trim().Replace("/", "__").ToUpper();
                    strHostFile = "/data/YAK_IMAGE/" + strCode.Trim().Replace("/", "__").ToUpper();
                    strHost = "/data/YAK_IMAGE/";

                    using (Ftpedt FtpedtX = new Ftpedt())
                    {
                        FileInfo f = new FileInfo(strFile);
                        Image temp = null;

                        if (f.Exists == true)
                        {
                            picPhoto.Visible = true;

                            using (MemoryStream mem = new MemoryStream(File.ReadAllBytes(strFile)))
                            {
                                temp = Image.FromStream(mem);
                            }

                            picPhoto.Image = temp;
                            picPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
                            lblInformation.Visible = false;
                        }
                        else
                        {
                            if (FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFile, strHostFile, strHost) == true)
                            {
                                picPhoto.Visible = true;

                                using (MemoryStream mem = new MemoryStream(File.ReadAllBytes(strFile)))
                                {
                                    temp = Image.FromStream(mem);
                                }

                                picPhoto.Image = temp;
                                picPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
                                lblInformation.Visible = false;
                            }
                        }
                    }

                       

                    #endregion
                }

                panCancel.Enabled = true;
                panExit.Enabled = true;

                //ssGubunCellClick(1, 5);
                ssGubunWebCellClick(0, 0);
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_DRUG_MASTER2(string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            //string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT BOHUMCODE ";                
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_MASTER2 ";
                SQL = SQL + ComNum.VBLF + "     WHERE JEPCODE = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    txtBCode.Text = dt.Rows[0]["BOHUMCODE"].ToString().Trim();
                }

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

                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private string READ_SAVEPATH(string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SAVETEMP";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_MASTER2 ";
                SQL = SQL + ComNum.VBLF + "     WHERE JEPCODE = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SAVETEMP"].ToString().Trim();
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

                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string READ_BRIGHTPATH(string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SAVEBRIGHT, SAVEBRIGHTETC";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_MASTER2";
                SQL = SQL + ComNum.VBLF + "     WHERE JEPCODE = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["SAVEBRIGHT"].ToString().Trim())
                    {
                        case "1": rtnVal = "차광"; break;
                        case "2": rtnVal = "비차광"; break;
                        case "3": rtnVal = "기타(" + dt.Rows[0]["SAVEBRIGHTETC"].ToString().Trim() + ")"; break;
                    }
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

                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string READ_NOTPOWDER(string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     NOT_POWDER, NOT_POWDER_SUB, NOT_POWDER_ETC ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_MASTER4 ";
                SQL = SQL + ComNum.VBLF + "     WHERE JEPCODE = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["NOT_POWDER"].ToString().Trim() + dt.Rows[0]["NOT_POWDER_SUB"].ToString().Trim())
                    {
                        case "11": rtnVal = "분쇄 불가-동종약 대체(동일성분 & 동일함량)"; break;
                        case "12": rtnVal = "분쇄 불가-동종/동효약 선택 대체"; break;
                        case "13": rtnVal = "분쇄 불가-대체불가-알약 투여"; break;
                        case "21": rtnVal = "분쇄 주의-동종약 대체(동일성분 & 동일함량)"; break;
                        case "22": rtnVal = "분쇄 주의-동종/동효약 선택 대체"; break;
                        case "23": rtnVal = "분쇄 주의-대체불가-알약 또는 파우더(경관식 환자) 투여"; break;
                        case "31": rtnVal = "분쇄 불필요-붕해정/확산정"; break;
                        case "32": rtnVal = "분쇄 불필요-설하정"; break;
                        case "33": rtnVal = "분쇄 불필요-" + dt.Rows[0]["NOT_POWDER_ETC"].ToString().Trim(); break;
                    }
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

                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string READ_NOTPOWDER_REPLACE_DRUG(string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     R_JEPCODE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_MASTER4_REPLACE ";
                SQL = SQL + ComNum.VBLF + "     WHERE JEPCODE = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        rtnVal = rtnVal + dt.Rows[i]["R_JEPCODE"].ToString().Trim() + ",";
                    }

                    rtnVal = VB.Mid(rtnVal, 1, rtnVal.Length - 1);
                }

                dt.Dispose();
                dt = null;

                if (rtnVal != "") { rtnVal = "대체약:" + rtnVal; }

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }
        
        private void Dir_Check(string sDirPath, string sExe = "*.*")
        {
            DirectoryInfo Dir = new DirectoryInfo(sDirPath);

            if (Dir.Exists == false)
            {
                Dir.Create();
            }
            else
            {
                FileInfo[] File = Dir.GetFiles(sExe, SearchOption.AllDirectories);

                foreach (FileInfo file in File)
                {
                    file.Delete();
                }
            }
        }

        private void Remark_All_SET()
        {
            int i = 0;
            string strRemark = "";
            string strAllRemark = "";

            string strSubTitle = "";
            string strSubTitleUse = "";

            Control[] controls = ComFunc.GetAllControls(this);

            strSubTitle = ComNum.VBLF + "--------【사용상의 주의사항】 --------" + ComNum.VBLF;

            strAllRemark = "";

            for (i = 1; i <= 5; i++)
            {
                foreach (Control ctl in controls)
                {
                    if (ctl is TextBox)
                    {
                        if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                        {
                            if (VB.Right(((TextBox)ctl).Name, 2) == (i - 1).ToString("00"))
                            {
                                strRemark = VB.RTrim(((TextBox)ctl).Text);
                            }
                        }
                    }
                }

                if (strRemark != "")
                {
                    switch (i)
                    {
                        case 1: strAllRemark = strAllRemark + "--------【성분/함량】--------"; break;
                        case 2: strAllRemark = strAllRemark + "--------【약리작용】--------"; break;
                        case 3: strAllRemark = strAllRemark + "--------【약효분류】--------"; break;
                        case 4: strAllRemark = strAllRemark + "--------【효능/효과】--------"; break;
                        case 5: strAllRemark = strAllRemark + "--------【용법/용량】--------"; break;
                    }

                    //유효문자뒤의 CRLF를 제거함
                    while(true)
                    {
                        if (strRemark.Length < 2) { break; }
                        if (VB.Right(strRemark, 2) == ComNum.VBLF)
                        {
                            strRemark = VB.Mid(strRemark, 1, strRemark.Length - 2);
                        }
                        else
                        {
                            break;
                        }

                        strRemark = VB.RTrim(strRemark);
                    }

                    foreach (Control ctl in controls)
                    {
                        if (ctl is TextBox)
                        {
                            if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                            {
                                if (VB.Right(((TextBox)ctl).Name, 2) == (i - 1).ToString("00"))
                                {
                                    ((TextBox)ctl).Text = strRemark;
                                }
                            }
                        }
                    }

                    strAllRemark = strAllRemark + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                }
            }

            for (i = 14; i <= 27; i++)
            {
                foreach (Control ctl in controls)
                {
                    if (ctl is TextBox)
                    {
                        if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                        {
                            if (VB.Right(((TextBox)ctl).Name, 2) == (i - 1).ToString("00"))
                            {
                                strRemark = VB.RTrim(((TextBox)ctl).Text);
                            }
                        }
                    }
                }

                if (strRemark != "")
                {
                    switch (i)
                    {
                        case 14:
                            if (strSubTitleUse == "")
                            {
                                strAllRemark = strAllRemark + strSubTitle + ComNum.VBLF + ComNum.VBLF;
                            }
                            strSubTitleUse = "Y";
                            strAllRemark = strAllRemark + " ▶ 부작용,금기 ◀";
                            break;
                        case 15:
                            if (strSubTitleUse == "")
                            {
                                strAllRemark = strAllRemark + strSubTitle + ComNum.VBLF + ComNum.VBLF;
                            }
                            strSubTitleUse = "Y";
                            strAllRemark = strAllRemark + " ▶ 경고 ◀";
                            break;
                        case 16:
                            if (strSubTitleUse == "")
                            {
                                strAllRemark = strAllRemark + strSubTitle + ComNum.VBLF + ComNum.VBLF;
                            }
                            strSubTitleUse = "Y";
                            strAllRemark = strAllRemark + " ▶ 금기 ◀";
                            break;
                        case 17:
                            if (strSubTitleUse == "")
                            {
                                strAllRemark = strAllRemark + strSubTitle + ComNum.VBLF + ComNum.VBLF;
                            }
                            strSubTitleUse = "Y";
                            strAllRemark = strAllRemark + " ▶ 신중투여 ◀";
                            break;
                        case 18:
                            if (strSubTitleUse == "")
                            {
                                strAllRemark = strAllRemark + strSubTitle + ComNum.VBLF + ComNum.VBLF;
                            }
                            strSubTitleUse = "Y";
                            strAllRemark = strAllRemark + " ▶ 이상반응 ◀";
                            break;
                        case 19:
                            if (strSubTitleUse == "")
                            {
                                strAllRemark = strAllRemark + strSubTitle + ComNum.VBLF + ComNum.VBLF;
                            }
                            strSubTitleUse = "Y";
                            strAllRemark = strAllRemark + " ▶ 일반적 주의 ◀";
                            break;
                        case 20:
                            if (strSubTitleUse == "")
                            {
                                strAllRemark = strAllRemark + strSubTitle + ComNum.VBLF + ComNum.VBLF;
                            }
                            strSubTitleUse = "Y";
                            strAllRemark = strAllRemark + " ▶ 상호작용 ◀";
                            break;
                        case 21:
                            if (strSubTitleUse == "")
                            {
                                strAllRemark = strAllRemark + strSubTitle + ComNum.VBLF + ComNum.VBLF;
                            }
                            strSubTitleUse = "Y";
                            strAllRemark = strAllRemark + " ▶ 임부/소아 ◀";
                            break;
                        case 22:
                            if (strSubTitleUse == "")
                            {
                                strAllRemark = strAllRemark + strSubTitle + ComNum.VBLF + ComNum.VBLF;
                            }
                            strSubTitleUse = "Y";
                            strAllRemark = strAllRemark + " ▶ 임부/수유부 ◀";
                            break;
                        case 23:
                            if (strSubTitleUse == "")
                            {
                                strAllRemark = strAllRemark + strSubTitle + ComNum.VBLF + ComNum.VBLF;
                            }
                            strSubTitleUse = "Y";
                            strAllRemark = strAllRemark + " ▶ 소아 ◀";
                            break;
                        case 24:
                            if (strSubTitleUse == "")
                            {
                                strAllRemark = strAllRemark + strSubTitle + ComNum.VBLF + ComNum.VBLF;
                            }
                            strSubTitleUse = "Y";
                            strAllRemark = strAllRemark + " ▶ 고령자 ◀";
                            break;
                        case 25:
                            if (strSubTitleUse == "")
                            {
                                strAllRemark = strAllRemark + strSubTitle + ComNum.VBLF + ComNum.VBLF;
                            }
                            strSubTitleUse = "Y";
                            strAllRemark = strAllRemark + " ▶ 과량 투여시 대처법 ◀";
                            break;
                        case 26:
                            if (strSubTitleUse == "")
                            {
                                strAllRemark = strAllRemark + strSubTitle + ComNum.VBLF + ComNum.VBLF;
                            }
                            strSubTitleUse = "Y";
                            strAllRemark = strAllRemark + " ▶ 적용상 주의 ◀";
                            break;
                        case 27:
                            if (strSubTitleUse == "")
                            {
                                strAllRemark = strAllRemark + strSubTitle + ComNum.VBLF + ComNum.VBLF;
                            }
                            strSubTitleUse = "Y";
                            strAllRemark = strAllRemark + " ▶ 보관 및 취급상 주의 ◀";
                            break;
                    }

                    //유효문자뒤의 CRLF를 제거함
                    while (true)
                    {
                        if (strRemark.Length < 2) { break; }
                        if (VB.Right(strRemark, 2) == ComNum.VBLF)
                        {
                            strRemark = VB.Mid(strRemark, 1, strRemark.Length - 2);
                        }
                        else
                        {
                            break;
                        }

                        strRemark = VB.RTrim(strRemark);
                    }

                    foreach (Control ctl in controls)
                    {
                        if (ctl is TextBox)
                        {
                            if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                            {
                                if (VB.Right(((TextBox)ctl).Name, 2) == (i - 1).ToString("00"))
                                {
                                    ((TextBox)ctl).Text = strRemark;
                                }
                            }
                        }
                    }

                    strAllRemark = strAllRemark + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                }
            }

            for (i = 7; i <= 11; i++)
            {
                foreach (Control ctl in controls)
                {
                    if (ctl is TextBox)
                    {
                        if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                        {
                            if (VB.Right(((TextBox)ctl).Name, 2) == (i - 1).ToString("00"))
                            {
                                strRemark = VB.RTrim(((TextBox)ctl).Text);
                            }
                        }
                    }
                }

                if (strRemark != "")
                {
                    switch (i)
                    {
                        case 7:
                            strAllRemark = strAllRemark + "--------【저장방법】--------";
                            break;
                        case 8:
                            strAllRemark = strAllRemark + "--------【약물동력학】--------";
                            break;
                        case 9:
                            strAllRemark = strAllRemark + "--------【복약지도】--------";
                            break;
                        case 10:
                            strAllRemark = strAllRemark + "--------【심사정보】--------";
                            break;
                        case 11:
                            strAllRemark = strAllRemark + "--------【기     타】--------";
                            break;
                    }

                    //유효문자뒤의 CRLF를 제거함
                    while (true)
                    {
                        if (strRemark.Length < 2) { break; }
                        if (VB.Right(strRemark, 2) == ComNum.VBLF)
                        {
                            strRemark = VB.Mid(strRemark, 1, strRemark.Length - 2);
                        }
                        else
                        {
                            break;
                        }

                        strRemark = VB.RTrim(strRemark);
                    }

                    foreach (Control ctl in controls)
                    {
                        if (ctl is TextBox)
                        {
                            if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                            {
                                if (VB.Right(((TextBox)ctl).Name, 2) == (i - 1).ToString("00"))
                                {
                                    ((TextBox)ctl).Text = strRemark;
                                }
                            }
                        }
                    }

                    strAllRemark = strAllRemark + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                }
            }

            for (i = 28; i <= 28; i++)
            {
                foreach (Control ctl in controls)
                {
                    if (ctl is TextBox)
                    {
                        if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                        {
                            if (VB.Right(((TextBox)ctl).Name, 2) == (i - 1).ToString("00"))
                            {
                                strRemark = VB.RTrim(((TextBox)ctl).Text);
                            }
                        }
                    }
                }

                if (strRemark != "")
                {
                    switch (i)
                    {
                        case 28:
                            if (strSubTitleUse == "")
                            {
                                strAllRemark = strAllRemark + strSubTitle + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                            }
                            strSubTitleUse = "Y";
                            strAllRemark = strAllRemark + "--------【기타】--------";
                            break;
                    }

                    //유효문자뒤의 CRLF를 제거함
                    while (true)
                    {
                        if (strRemark.Length < 2) { break; }
                        if (VB.Right(strRemark, 2) == ComNum.VBLF)
                        {
                            strRemark = VB.Mid(strRemark, 1, strRemark.Length - 2);
                        }
                        else
                        {
                            break;
                        }

                        strRemark = VB.RTrim(strRemark);
                    }

                    foreach (Control ctl in controls)
                    {
                        if (ctl is TextBox)
                        {
                            if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                            {
                                if (VB.Right(((TextBox)ctl).Name, 2) == (i - 1).ToString("00"))
                                {
                                    ((TextBox)ctl).Text = strRemark;
                                }
                            }
                        }
                    }

                    strAllRemark = strAllRemark + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                }
            }

            txtInfo11.Text = strAllRemark;
        }

        private void Remark_All_SET_SUB()
        {
            string strRemark = "";
            string strAllRemark = "";

            int i = 0;

            Control[] controls = ComFunc.GetAllControls(this);

            strAllRemark = "";
            strAllRemark = ComNum.VBLF + "▣ 사용상의 주의사항 ▣ " + ComNum.VBLF + ComNum.VBLF;
            
            for (i = 14; i <= 28; i++)
            {
                foreach (Control ctl in controls)
                {
                    if (ctl is TextBox)
                    {
                        if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                        {
                            if (VB.Right(((TextBox)ctl).Name, 2) == (i - 1).ToString("00"))
                            {
                                strRemark = VB.RTrim(((TextBox)ctl).Text);
                            }
                        }
                    }
                }

                if (strRemark != "")
                {
                    switch (i)
                    {
                        case 14: strAllRemark = strAllRemark + "--------【구)부작용,금기】--------"; break;
                        case 15: strAllRemark = strAllRemark + "--------【경고】--------"; break;
                        case 16: strAllRemark = strAllRemark + "--------【금기】--------"; break;
                        case 17: strAllRemark = strAllRemark + "--------【신중투여】--------"; break;
                        case 18: strAllRemark = strAllRemark + "--------【이상반응】--------"; break;
                        case 19: strAllRemark = strAllRemark + "--------【일반적 주의】--------"; break;
                        case 20: strAllRemark = strAllRemark + "--------【상호작용】--------"; break;
                        case 21: strAllRemark = strAllRemark + "--------【구)임부/소아】--------"; break;
                        case 22: strAllRemark = strAllRemark + "--------【임부/수유부】--------"; break;
                        case 23: strAllRemark = strAllRemark + "--------【소아】--------"; break;
                        case 24: strAllRemark = strAllRemark + "--------【고령자】--------"; break;
                        case 25: strAllRemark = strAllRemark + "--------【과량 투여시 대처법】--------"; break;
                        case 26: strAllRemark = strAllRemark + "--------【적용상 주의】--------"; break;
                        case 27: strAllRemark = strAllRemark + "--------【보관 및 취급상 주의】--------"; break;
                        case 28: strAllRemark = strAllRemark + "--------【기타】--------"; break;
                    }

                    //유효문자뒤의 CRLF를 제거함
                    while (true)
                    {
                        if (strRemark.Length < 2) { break; }
                        if (VB.Right(strRemark, 2) == ComNum.VBLF)
                        {
                            strRemark = VB.Mid(strRemark, 1, strRemark.Length - 2);
                        }
                        else
                        {
                            break;
                        }

                        strRemark = VB.RTrim(strRemark);
                    }

                    foreach (Control ctl in controls)
                    {
                        if (ctl is TextBox)
                        {
                            if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                            {
                                if (VB.Right(((TextBox)ctl).Name, 2) == (i - 1).ToString("00"))
                                {
                                    ((TextBox)ctl).Text = strRemark;
                                }
                            }
                        }
                    }

                    strAllRemark = strAllRemark + ComNum.VBLF + strRemark + ComNum.VBLF + ComNum.VBLF + ComNum.VBLF;
                }
            }

            txtInfo12.Text = strAllRemark;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            #region //변수선언 등..
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            //string strROWID = "";
            string strInfo = "";

            string strRemark1 = "";
            string strRemark2 = "";
            string strRemark3 = "";
            string strRemark4 = "";
            string strRemark5 = "";
            string strYakImage = "";
            string strChar = "";
            #endregion

            //Data 오류 Check
            #region //오류체크 및 변수값 설정
            if (txtDrugHname.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "한글명칭이 공란입니다.", "오류");
                txtDrugHname.Focus();
                return;
            }

            if (txtDrugEname.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "영문명칭이 공란입니다.", "오류");
                txtDrugEname.Focus();
                return;
            }

            if (txtBunCode.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "분류번호가 공란입니다.", "오류");
                txtBunCode.Focus();
                return;
            } 
           
            //약품사진 여부
            strYakImage = "N";

            if (GstrImageYN == "Y") { strYakImage = "Y"; }
            if (GstrImageFile != "") { strYakImage = "Y"; }

            txtDrugHname.Text = txtDrugHname.Text.Replace("'", "`").Replace("·", ".");
            txtDrugEname.Text = txtDrugEname.Text.Replace("'", "`").Replace("·", ".");
            txtJeheng.Text = txtJeheng.Text.Replace("'", "`").Replace("·", ".");
            txtJeheng2.Text = txtJeheng2.Text.Replace("'", "`").Replace("·", ".");
            txtJeheng31.Text = txtJeheng31.Text.Replace("'", "`").Replace("·", ".");
            txtJeheng32.Text = txtJeheng32.Text.Replace("'", "`").Replace("·", ".");
            txtJeyak.Text = txtJeyak.Text.Replace("'", "`").Replace("·", ".");

            if (txtJeheng.Text.Length > 60)
            {
                ComFunc.MsgBoxEx(this, "제형이 60자 이상입니다.");
                txtJeheng.Focus();
                return;
            }

            if (txtJeheng2.Text.Length > 60)
            {
                ComFunc.MsgBoxEx(this, "색상모양이 60자 이상입니다.");
                txtJeheng2.Focus();
                return;
            }

            if (txtJeheng31.Text.Length > 60)
            {
                ComFunc.MsgBoxEx(this, "식별표시 앞)이 60자 이상입니다.");
                txtJeheng31.Focus();
                return;
            }

            if (txtJeheng32.Text.Length > 60)
            {
                ComFunc.MsgBoxEx(this, "식별표시 뒤)이 60자 이상입니다.");
                txtJeheng32.Focus();
                return;
            }

            Control[] controls = ComFunc.GetAllControls(this);
            
            foreach (Control ctl in controls)
            {
                if (ctl is TextBox)
                {
                    if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                    {
                        ((TextBox)ctl).Text = ((TextBox)ctl).Text.Replace("'", "`").Replace("·", ".");

                        if (((TextBox)ctl).Text.Length > 20000)
                        {
                            switch ((int)VB.Val(VB.Right(((TextBox)ctl).Name, 2)))
                            {
                                case 0: ComFunc.MsgBoxEx(this, "성분/함량이 20000자 이상입니다.", "오류"); break;
                                case 1: ComFunc.MsgBoxEx(this, "약리작용이 20000자 이상입니다.", "오류"); break;
                                case 2: ComFunc.MsgBoxEx(this, "약효분류가 20000자 이상입니다.", "오류"); break;
                                case 3: ComFunc.MsgBoxEx(this, "효능/효과가 20000자 이상입니다.", "오류"); break;
                                case 4: ComFunc.MsgBoxEx(this, "용법/용량이 20000자 이상입니다.", "오류"); break;
                                case 6: ComFunc.MsgBoxEx(this, "저장방법이 20000자 이상입니다.", "오류"); break;
                                case 7: ComFunc.MsgBoxEx(this, "약물동력학이 20000자 이상입니다.", "오류"); break;
                                case 8: ComFunc.MsgBoxEx(this, "복약지도가 20000자 이상입니다.", "오류"); break;
                                case 9: ComFunc.MsgBoxEx(this, "심사정보가 20000자 이상입니다.", "오류"); break;
                                case 10: ComFunc.MsgBoxEx(this, "기타 내용이 20000자 이상입니다.", "오류"); break;
                                case 13: ComFunc.MsgBoxEx(this, "부작용/금기가 20000자 이상입니다.", "오류"); break;
                                case 14: ComFunc.MsgBoxEx(this, "경고가 20000자 이상입니다.", "오류"); break;
                                case 15: ComFunc.MsgBoxEx(this, "경고가 20000자 이상입니다.", "오류"); break;
                                case 16: ComFunc.MsgBoxEx(this, "신중투여가 20000자 이상입니다.", "오류"); break;
                                case 17: ComFunc.MsgBoxEx(this, "이상반응이 20000자 이상입니다.", "오류"); break;
                                case 18: ComFunc.MsgBoxEx(this, "일반적 주의가 20000자 이상입니다.", "오류"); break;
                                case 19: ComFunc.MsgBoxEx(this, "상호작용이 20000자 이상입니다.", "오류"); break;
                                case 20: ComFunc.MsgBoxEx(this, "임부/소아가 20000자 이상입니다.", "오류"); break;
                                case 21: ComFunc.MsgBoxEx(this, "임부/수유부가 20000자 이상입니다.", "오류"); break;
                                case 22: ComFunc.MsgBoxEx(this, "소아가 20000자 이상입니다.", "오류"); break;
                                case 23: ComFunc.MsgBoxEx(this, "고령자가 20000자 이상입니다.", "오류"); break;
                                case 24: ComFunc.MsgBoxEx(this, "과량 투여시 대처법이 20000자 이상입니다.", "오류"); break;
                                case 25: ComFunc.MsgBoxEx(this, "적용상 주의가 20000자 이상입니다.", "오류"); break;
                                case 26: ComFunc.MsgBoxEx(this, "보관 및 취급상 주의가 20000자 이상입니다.", "오류"); break;
                                case 27: ComFunc.MsgBoxEx(this, "기타가 20000자 이상입니다.", "오류"); break;
                            }
                        }
                    }
                }
            }

            #endregion

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //자료가 있으면
                if (GstrROWID != "")
                {
                    #region //자료있으면 갱신
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_MED + "OCS_DRUGINFO_NEW";
                    SQL = SQL + ComNum.VBLF + "     SET ";
                    SQL = SQL + ComNum.VBLF + "         BunCode = '" + txtBunCode.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         HName = '" + txtDrugHname.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         EName = '" + txtDrugEname.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         SName = '" + txtDrugSname.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         Unit = '" + txtUnit.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         Jeheng = '" + txtJeheng.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         Jeheng2 = '" + txtJeheng2.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         Jeheng3_1 = '" + txtJeheng31.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         Jeheng3_2 = '" + txtJeheng32.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         Effect = '" + txtDrugHoo.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         Image_YN = '" + strYakImage + "',";

                    if (cboJong.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         Jong = '" + VB.Left(cboJong.Text, 2) + "', ";
                    }

                    foreach (Control ctl in controls)
                    {
                        if (ctl is TextBox)
                        {
                            if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                            {
                                //사용상의 주의사항 및 전체는 저장 안함
                                if (VB.Val(VB.Right(((TextBox)ctl).Name, 2)) == 5 || VB.Val(VB.Right(((TextBox)ctl).Name, 2)) == 11 || VB.Val(VB.Right(((TextBox)ctl).Name, 2)) == 12) { }
                                else
                                {
                                    strInfo = ((TextBox)ctl).Text;

                                    #region GoSub Remark_Edit_Process   Remark를 4,000Byte로 짜름

                                    byte[] bite = Encoding.Default.GetBytes(strInfo);

                                    strRemark1 = "";
                                    strRemark2 = "";
                                    strRemark3 = "";
                                    strRemark4 = "";
                                    strRemark5 = "";

                                    if (bite.Length < 4001)
                                    {
                                        strRemark1 = strInfo;
                                    }
                                    else
                                    {
                                        //Remark1 Setting
                                        strRemark1 = ComFunc.LeftH(strInfo, 4000);
                                        strChar = VB.Right(strRemark1, 1);

                                        //한글 반글자가 짤리면
                                        if ((strChar == " " || strChar == "~") && ComFunc.LenH(strChar) == 1)
                                        {
                                            strRemark1 = ComFunc.LeftH(strRemark1, (int)(ComFunc.LenH(strRemark1) - 1));
                                        }

                                        strInfo = ComFunc.RightH(strInfo, (int)(ComFunc.LenH(strInfo) - ComFunc.LenH(strRemark1)));

                                        if (ComFunc.LenH(strInfo) < 4001)
                                        {
                                            strRemark2 = strInfo;
                                        }
                                        else
                                        {
                                            //Remark2 Setting
                                            strRemark2 = ComFunc.LeftH(strInfo, 4000);
                                            strChar = VB.Right(strRemark2, 1);

                                            //한글 반글자가 짤리면
                                            if ((strChar == " " || strChar == "~") && ComFunc.LenH(strChar) == 1)
                                            {
                                                strRemark2 = ComFunc.LeftH(strRemark2, (int)(ComFunc.LenH(strRemark2) - 1));
                                            }

                                            strInfo = ComFunc.RightH(strInfo, (int)(ComFunc.LenH(strInfo) - ComFunc.LenH(strRemark2)));

                                            if (ComFunc.LenH(strInfo) < 4001)
                                            {
                                                strRemark3 = strInfo;
                                            }
                                            else
                                            {
                                                //Remark3 Setting
                                                strRemark3 = ComFunc.LeftH(strInfo, 4000);
                                                strChar = VB.Right(strRemark3, 1);

                                                //한글 반글자가 짤리면
                                                if ((strChar == " " || strChar == "~") && ComFunc.LenH(strChar) == 1)
                                                {
                                                    strRemark3 = ComFunc.LeftH(strRemark3, (int)(ComFunc.LenH(strRemark3) - 1));
                                                }

                                                strInfo = ComFunc.RightH(strInfo, (int)(ComFunc.LenH(strInfo) - ComFunc.LenH(strRemark3)));

                                                if (ComFunc.LenH(strInfo) < 4001)
                                                {
                                                    strRemark4 = strInfo;
                                                }
                                                else
                                                {
                                                    //Remark4 Setting
                                                    strRemark4 = ComFunc.LeftH(strInfo, 4000);
                                                    strChar = VB.Right(strRemark4, 1);

                                                    //한글 반글자가 짤리면
                                                    if ((strChar == " " || strChar == "~") && ComFunc.LenH(strChar) == 1)
                                                    {
                                                        strRemark4 = ComFunc.LeftH(strRemark4, (int)(ComFunc.LenH(strRemark4) - 1));
                                                    }

                                                    strInfo = ComFunc.RightH(strInfo, (int)(ComFunc.LenH(strInfo) - ComFunc.LenH(strRemark4)));

                                                    if (ComFunc.LenH(strInfo) < 4001)
                                                    {
                                                        strRemark5 = strInfo;
                                                    }
                                                    else
                                                    {
                                                        //Remark4 Setting
                                                        strRemark5 = ComFunc.LeftH(strInfo, 4000);
                                                        strChar = VB.Right(strRemark5, 1);

                                                        //한글 반글자가 짤리면
                                                        if ((strChar == " " || strChar == "~") && ComFunc.LenH(strChar) == 1)
                                                        {
                                                            strRemark5 = ComFunc.LeftH(strRemark5, (int)(ComFunc.LenH(strRemark5) - 1));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    #endregion

                                    SQL = SQL + ComNum.VBLF + "         Remark" + (VB.Val(VB.Right(((TextBox)ctl).Name, 2)) + 1).ToString("00") + "1 = '" + strRemark1 + "', ";
                                    SQL = SQL + ComNum.VBLF + "         Remark" + (VB.Val(VB.Right(((TextBox)ctl).Name, 2)) + 1).ToString("00") + "2 = '" + strRemark2 + "', ";
                                    SQL = SQL + ComNum.VBLF + "         Remark" + (VB.Val(VB.Right(((TextBox)ctl).Name, 2)) + 1).ToString("00") + "3 = '" + strRemark3 + "', ";
                                    SQL = SQL + ComNum.VBLF + "         Remark" + (VB.Val(VB.Right(((TextBox)ctl).Name, 2)) + 1).ToString("00") + "4 = '" + strRemark4 + "', ";
                                    SQL = SQL + ComNum.VBLF + "         Remark" + (VB.Val(VB.Right(((TextBox)ctl).Name, 2)) + 1).ToString("00") + "5 = '" + strRemark5 + "', ";
                                }
                            }
                        }
                    }

                    SQL = SQL + ComNum.VBLF + "         Jeyak = '" + txtJeyak.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         DRBUN = '" + VB.Left(cboDrug.Text, 2) + "', ";
                    SQL = SQL + ComNum.VBLF + "         SDATE = TO_DATE('" + txtDate.Text.Trim() + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         ENTDATE = SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         POWDER = '" + VB.Left(cboPowder.Text, 1) + "', ";
                    SQL = SQL + ComNum.VBLF + "         CAUTION = '" + VB.Left(cboCaution.Text, 2) + "',";
                    SQL = SQL + ComNum.VBLF + "         CAUTION_STRING = '" + cboCaution.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         METFORMIN = '" + (chkMetformin.Checked == true ? "1" : "0") + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + txtSuNext.Text.Trim() + "'  "; 
                    #endregion
                }
                else
                {
                    #region //신규 자료저장
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUGINFO_NEW";
                    SQL = SQL + ComNum.VBLF + "     (SuNext, Jong, BunCode, Hname, Ename, SName, Unit, Jeheng, JEHENG2, JEHENG3_1, JEHENG3_2, EffEct, ";
                    SQL = SQL + ComNum.VBLF + "     Remark011, Remark012, Remark013, Remark014, Remark015, ";
                    SQL = SQL + ComNum.VBLF + "     Remark021, Remark022, Remark023, Remark024, Remark025, ";
                    SQL = SQL + ComNum.VBLF + "     Remark031, Remark032, Remark033, Remark034, Remark035, ";
                    SQL = SQL + ComNum.VBLF + "     Remark041, Remark042, Remark043, Remark044, Remark045, ";
                    SQL = SQL + ComNum.VBLF + "     Remark051, Remark052, Remark053, Remark054, Remark055, ";
                    SQL = SQL + ComNum.VBLF + "     Remark071, Remark072, Remark073, Remark074, Remark075, ";
                    SQL = SQL + ComNum.VBLF + "     Remark081, Remark082, Remark083, Remark084, Remark085, ";
                    SQL = SQL + ComNum.VBLF + "     Remark091, Remark092, Remark093, Remark094, Remark095, ";
                    SQL = SQL + ComNum.VBLF + "     Remark101, Remark102, Remark103, Remark104, Remark105, ";
                    SQL = SQL + ComNum.VBLF + "     Remark111, Remark112, Remark113, Remark114, Remark115, ";
                    SQL = SQL + ComNum.VBLF + "     Remark141, Remark142, Remark143, Remark144, Remark145, ";
                    SQL = SQL + ComNum.VBLF + "     Remark151, Remark152, Remark153, Remark154, Remark155, ";
                    SQL = SQL + ComNum.VBLF + "     Remark161, Remark162, Remark163, Remark164, Remark165, ";
                    SQL = SQL + ComNum.VBLF + "     Remark171, Remark172, Remark173, Remark174, Remark175, ";
                    SQL = SQL + ComNum.VBLF + "     Remark181, Remark182, Remark183, Remark184, Remark185, ";
                    SQL = SQL + ComNum.VBLF + "     Remark191, Remark192, Remark193, Remark194, Remark195, ";
                    SQL = SQL + ComNum.VBLF + "     Remark201, Remark202, Remark203, Remark204, Remark205, ";
                    SQL = SQL + ComNum.VBLF + "     Remark211, Remark212, Remark213, Remark214, Remark215, ";
                    SQL = SQL + ComNum.VBLF + "     Remark221, Remark222, Remark223, Remark224, Remark225, ";
                    SQL = SQL + ComNum.VBLF + "     Remark231, Remark232, Remark233, Remark234, Remark235, ";
                    SQL = SQL + ComNum.VBLF + "     Remark241, Remark242, Remark243, Remark244, Remark245, ";
                    SQL = SQL + ComNum.VBLF + "     Remark251, Remark252, Remark253, Remark254, Remark255, ";
                    SQL = SQL + ComNum.VBLF + "     Remark261, Remark262, Remark263, Remark264, Remark265, ";
                    SQL = SQL + ComNum.VBLF + "     Remark271, Remark272, Remark273, Remark274, Remark275, ";
                    SQL = SQL + ComNum.VBLF + "     Remark281, Remark282, Remark283, Remark284, Remark285, ";
                    SQL = SQL + ComNum.VBLF + "     Jeyak, Image_YN, DRBUN, ENTDATE, SDATE, ";
                    SQL = SQL + ComNum.VBLF + "     POWDER, PREINPUT, CAUTION, CAUTION_STRING, METFORMIN)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         '" + txtSuNext.Text.Trim() + "', ";

                    if (cboJong.Text.Trim() == "")
                    {
                        SQL = SQL + ComNum.VBLF + "         '', ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboJong.Text, 2) + "', ";
                    }

                    SQL = SQL + ComNum.VBLF + "         '" + txtBunCode.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtDrugHname.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtDrugEname.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtDrugSname.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtUnit.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtJeheng.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtJeheng2.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtJeheng31.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtJeheng32.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtDrugHoo.Text.Trim() + "', ";

                    for (int i = 1; i <= 28; i++)
                    {
                        foreach (Control ctl in controls)
                        {
                            if (ctl is TextBox)
                            {
                                if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                                {
                                    //사용상의 주의사항 및 전체는 저장 안함
                                    if (VB.Val(VB.Right(((TextBox)ctl).Name, 2)) == 5 || VB.Val(VB.Right(((TextBox)ctl).Name, 2)) == 11 || VB.Val(VB.Right(((TextBox)ctl).Name, 2)) == 12) { }
                                    else
                                    {
                                        if (VB.Right(((TextBox)ctl).Name, 2) == (i - 1).ToString("00"))
                                        {
                                            strInfo = ((TextBox)ctl).Text;

                                            #region GoSub Remark_Edit_Process   Remark를 4,000Byte로 짜름

                                            byte[] bite = Encoding.Default.GetBytes(strInfo);

                                            strRemark1 = "";
                                            strRemark2 = "";
                                            strRemark3 = "";
                                            strRemark4 = "";
                                            strRemark5 = "";

                                            if (bite.Length < 4001)
                                            {
                                                strRemark1 = strInfo;
                                            }
                                            else
                                            {
                                                //Remark1 Setting
                                                strRemark1 = ComFunc.LeftH(strInfo, 4000);
                                                strChar = VB.Right(strRemark1, 1);

                                                //한글 반글자가 짤리면
                                                if ((strChar == " " || strChar == "~") && ComFunc.LenH(strChar) == 1)
                                                {
                                                    strRemark1 = ComFunc.LeftH(strRemark1, (int)(ComFunc.LenH(strRemark1) - 1));
                                                }

                                                strInfo = ComFunc.RightH(strInfo, (int)(ComFunc.LenH(strInfo) - ComFunc.LenH(strRemark1)));

                                                if (ComFunc.LenH(strInfo) < 4001)
                                                {
                                                    strRemark2 = strInfo;
                                                }
                                                else
                                                {
                                                    //Remark2 Setting
                                                    strRemark2 = ComFunc.LeftH(strInfo, 4000);
                                                    strChar = VB.Right(strRemark2, 1);

                                                    //한글 반글자가 짤리면
                                                    if ((strChar == " " || strChar == "~") && ComFunc.LenH(strChar) == 1)
                                                    {
                                                        strRemark2 = ComFunc.LeftH(strRemark2, (int)(ComFunc.LenH(strRemark2) - 1));
                                                    }

                                                    strInfo = ComFunc.RightH(strInfo, (int)(ComFunc.LenH(strInfo) - ComFunc.LenH(strRemark2)));

                                                    if (ComFunc.LenH(strInfo) < 4001)
                                                    {
                                                        strRemark3 = strInfo;
                                                    }
                                                    else
                                                    {
                                                        //Remark3 Setting
                                                        strRemark3 = ComFunc.LeftH(strInfo, 4000);
                                                        strChar = VB.Right(strRemark3, 1);

                                                        //한글 반글자가 짤리면
                                                        if ((strChar == " " || strChar == "~") && ComFunc.LenH(strChar) == 1)
                                                        {
                                                            strRemark3 = ComFunc.LeftH(strRemark3, (int)(ComFunc.LenH(strRemark3) - 1));
                                                        }

                                                        strInfo = ComFunc.RightH(strInfo, (int)(ComFunc.LenH(strInfo) - ComFunc.LenH(strRemark3)));

                                                        if (ComFunc.LenH(strInfo) < 4001)
                                                        {
                                                            strRemark4 = strInfo;
                                                        }
                                                        else
                                                        {
                                                            //Remark4 Setting
                                                            strRemark4 = ComFunc.LeftH(strInfo, 4000);
                                                            strChar = VB.Right(strRemark4, 1);

                                                            //한글 반글자가 짤리면
                                                            if ((strChar == " " || strChar == "~") && ComFunc.LenH(strChar) == 1)
                                                            {
                                                                strRemark4 = ComFunc.LeftH(strRemark4, (int)(ComFunc.LenH(strRemark4) - 1));
                                                            }

                                                            strInfo = ComFunc.RightH(strInfo, (int)(ComFunc.LenH(strInfo) - ComFunc.LenH(strRemark4)));

                                                            if (ComFunc.LenH(strInfo) < 4001)
                                                            {
                                                                strRemark5 = strInfo;
                                                            }
                                                            else
                                                            {
                                                                //Remark4 Setting
                                                                strRemark5 = ComFunc.LeftH(strInfo, 4000);
                                                                strChar = VB.Right(strRemark5, 1);

                                                                //한글 반글자가 짤리면
                                                                if ((strChar == " " || strChar == "~") && ComFunc.LenH(strChar) == 1)
                                                                {
                                                                    strRemark5 = ComFunc.LeftH(strRemark5, (int)(ComFunc.LenH(strRemark5) - 1));
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            #endregion

                                            SQL = SQL + ComNum.VBLF + "         '" + strRemark1 + "', ";
                                            SQL = SQL + ComNum.VBLF + "         '" + strRemark2 + "', ";
                                            SQL = SQL + ComNum.VBLF + "         '" + strRemark3 + "', ";
                                            SQL = SQL + ComNum.VBLF + "         '" + strRemark4 + "', ";
                                            SQL = SQL + ComNum.VBLF + "         '" + strRemark5 + "', ";

                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    SQL = SQL + ComNum.VBLF + "         '" + txtJeyak.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strYakImage + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboDrug.Text, 2) + "', ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + txtDate.Text.Trim() + "', 'YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboPowder.Text, 1) + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + GstrPreInput + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboCaution.Text, 2) + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + cboCaution.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + (chkMetformin.Checked == true ? "1" : "0") + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";
                    #endregion
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }                              

                #region GoSub menuSave_Image_Send   약품사진 전송

                if (SqlErr == "" && GstrImageFile != "")
                {
                    string strFile = "";
                    string strHostFile = "";
                    string strHost = "";

                    strFile = @"C:\PSMHEXE\YAK_IMAGE\" + txtSuNext.Text.Trim().Replace("/", "__").ToUpper();
                    strHostFile = "/data/YAK_IMAGE/" + txtSuNext.Text.Trim().Replace("/", "__").ToUpper();
                    strHost = "/data/YAK_IMAGE/";

                    using (Ftpedt FtpedtX = new Ftpedt())
                    {
                        if (GstrImageYN == "Y")
                        {
                            FtpedtX.FtpDeleteFile("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strHostFile, strHost);
                        }

                        if (FtpedtX.FtpUpload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), GstrImageFile, strHostFile, strHost) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, "자료 등록 중 오류 발생");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    //저장 후 파일 삭제
                    file_chk_del(strFile);

                }

                #endregion

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();
                txtSuNext.Focus();

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            txtSuNext.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("현 약품 정보를 삭제합니다."
                + ComNum.VBLF + "진행하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            if (txtSuNext.Text.Trim() == "") { return; }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "DELETE " + ComNum.DB_MED + "OCS_DRUGINFO_new ";
                SQL = SQL + ComNum.VBLF + "     WHERE SuNext = '" + txtSuNext.Text.Trim() + "'  ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();
                txtSuNext.Focus();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인
            //TODO
        }

        private void btnPicSearch_Click(object sender, EventArgs e)
        {
            string strFileName = "";

            OpenFileDialog f = new OpenFileDialog();

            f.Filter = "BMP파일 (*.bmp)|*.bmp|GIF파일(*.gif)|*.gif|Jepg파일(*.jepg)|*.Jepg|Jpg파일(*.jpg)|*.Jpg";
                        
            if (f.ShowDialog() == DialogResult.OK)
            {
                strFileName = f.FileName.Trim();
                GstrImageFile = strFileName;
                picPhoto.Visible = true;
                picPhoto.Load(strFileName);
                picPhoto.Refresh();
                lblInformation.Visible = false;
            }
        }

        private void btnPicDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (GstrImageYN != "Y") { return; }

            if (ComFunc.MsgBoxQ("현재 약품사진을 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_MED + "OCS_DRUGINFO_new";
                SQL = SQL + ComNum.VBLF + "     SET";
                SQL = SQL + ComNum.VBLF + "         Image_YN = '' ";
                SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + txtSuNext.Text.Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                string strHostFile = "";
                string strHost = "";

                using (Ftpedt ftpedtX = new Ftpedt())
                {
                    strHostFile = "/data/YAK_IMAGE/" + txtSuNext.Text.Trim();
                    strHost = "/data/YAK_IMAGE/";

                    ftpedtX.FtpDeleteFile("192.168.100.31", "oracle", ftpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strHostFile, strHost);
                    ftpedtX.FtpDisConnetBatch();
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                GstrImageFile = "";
                GstrImageYN = "";

                picPhoto.Visible = false;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void DrugInfoBodyWeb(string strGu)
        {
            OracleCommand cmd = new OracleCommand();
            PsmhDb pDbCon = clsDB.DbCon;
            OracleDataReader reader = null;

            string SQL = "";
            DataTable dt = new DataTable();
            string SqlErr = "";

            Control[] controls = ComFunc.GetAllControls(this);

            string strWebBrowser = strGu;

            if (strGu == "4")
            { strGu = "10"; }

            GstrYAKPUM_CD = "";

            Cursor.Current = Cursors.WaitCursor;
            
            try
            {
                cmd.Connection = pDbCon.Con;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandText = "KOSMOS_DRUG.up_DrugInfoHeader_PHSM";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("pDrugcode", OracleDbType.Varchar2, 9, txtSuNext.Text.Trim(), ParameterDirection.Input);
                cmd.Parameters.Add("pGu", OracleDbType.Varchar2, 1, "1", ParameterDirection.Input);
                cmd.Parameters.Add("pImgPath", OracleDbType.Varchar2, 9, "127.0.0.1", ParameterDirection.Input);
                cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

                reader = cmd.ExecuteReader();

                dt.Load(reader);
                reader.Dispose();
                reader = null;

                cmd.Dispose();
                cmd = null;

                if (dt == null)
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    GstrYAKPUM_CD = dt.Rows[0]["FDRUGCD"].ToString().Trim();
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    cmd = new OracleCommand();
                    pDbCon = clsDB.DbCon;
                    reader = null;
                    dt = new DataTable();

                    cmd.Connection = pDbCon.Con;
                    cmd.InitialLONGFetchSize = 1000;
                    cmd.CommandText = "KOSMOS_DRUG.up_DrugInfoHeader_PHSM";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("pDrugcode", OracleDbType.Varchar2, 9, txtBCode.Text.Trim(), ParameterDirection.Input);
                    cmd.Parameters.Add("pGu", OracleDbType.Varchar2, 1, "2", ParameterDirection.Input);
                    cmd.Parameters.Add("pImgPath", OracleDbType.Varchar2, 9, "127.0.0.1", ParameterDirection.Input);
                    cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

                    reader = cmd.ExecuteReader();

                    dt.Load(reader);
                    reader.Dispose();
                    reader = null;

                    cmd.Dispose();
                    cmd = null;

                    if (dt == null)
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        GstrYAKPUM_CD = dt.Rows[0]["FDRUGCD"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     KOSMOS_DRUG.UF_DRUGINFO_DSP('" + GstrYAKPUM_CD + "', '1', '" + strGu + "') AS DSP";
                SQL = SQL + ComNum.VBLF + "FROM DUAL";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     KOSMOS_DRUG.UF_DRUGINFO_DSP('" + GstrYAKPUM_CD + "', '2', '" + strGu + "') AS DSP";
                    SQL = SQL + ComNum.VBLF + "FROM DUAL";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        foreach (Control ctl in controls)
                        {
                            if (ctl is WebBrowser)
                            {
                                if (VB.Left(((WebBrowser)ctl).Name, 7) == "webInfo")
                                {
                                    if (VB.Val(VB.Right(((WebBrowser)ctl).Name, 2)) == VB.Val(strWebBrowser))
                                    {
                                        ((WebBrowser)ctl).DocumentText = dt.Rows[0]["DSP"].ToString().Trim();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (Control ctl in controls)
                    {
                        if (ctl is WebBrowser)
                        {
                            if (VB.Left(((WebBrowser)ctl).Name, 7) == "webInfo")
                            {
                                if (VB.Val(VB.Right(((WebBrowser)ctl).Name, 2)) == VB.Val(strWebBrowser))
                                {
                                    ((WebBrowser)ctl).DocumentText = dt.Rows[0]["DSP"].ToString().Trim();
                                }
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //cmd = new OracleCommand();
                //pDbCon = clsDB.DbCon;
                //reader = null;
                //dt = new DataTable();

                //cmd.Connection = pDbCon.Con;
                //cmd.InitialLONGFetchSize = 1000;
                //cmd.CommandText = "KOSMOS_DRUG.up_DrugInfoBody_PHSM2";
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("pDrugcode", OracleDbType.Varchar2, 9, GstrYAKPUM_CD, ParameterDirection.Input);
                //cmd.Parameters.Add("pCodeGu", OracleDbType.Varchar2, 1, "1", ParameterDirection.Input);
                //cmd.Parameters.Add("pGu", OracleDbType.Varchar2, 1, strGu, ParameterDirection.Input);
                //cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

                //reader = cmd.ExecuteReader();

                //dt.Load(reader);
                //reader.Dispose();
                //reader = null;

                //cmd.Dispose();
                //cmd = null;

                //if (dt == null)
                //{
                //    Cursor.Current = Cursors.Default;
                //    return;
                //}
                //if (dt.Rows.Count > 0)
                //{
                //    for (i = 0; i < dt.Rows.Count; i++)
                //    {
                //        foreach (Control ctl in controls)
                //        {
                //            if (ctl is WebBrowser)
                //            {
                //                if (VB.Left(((WebBrowser)ctl).Name, 7) == "webInfo")
                //                {
                //                    if (VB.Val(VB.Right(((WebBrowser)ctl).Name, 2)) == VB.Val(strWebBrowser))
                //                    {
                //                        ((WebBrowser)ctl).DocumentText += dt.Rows[i]["DESCRIPTION"].ToString().Trim();
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    dt.Dispose();
                //    dt = null;

                //    cmd = new OracleCommand();
                //    pDbCon = clsDB.DbCon;
                //    reader = null;
                //    dt = new DataTable();

                //    cmd.Connection = pDbCon.Con;
                //    cmd.InitialLONGFetchSize = 1000;
                //    cmd.CommandText = "KOSMOS_DRUG.up_DrugInfoBody_PHSM2";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add("pDrugcode", OracleDbType.Varchar2, 9, txtBCode.Text.Trim(), ParameterDirection.Input);
                //    cmd.Parameters.Add("pCodeGu", OracleDbType.Varchar2, 1, "2", ParameterDirection.Input);
                //    cmd.Parameters.Add("pGu", OracleDbType.Varchar2, 1, strGu, ParameterDirection.Input);
                //    cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

                //    reader = cmd.ExecuteReader();

                //    dt.Load(reader);
                //    reader.Dispose();
                //    reader = null;

                //    cmd.Dispose();
                //    cmd = null;

                //    if (dt == null)
                //    {
                //        Cursor.Current = Cursors.Default;
                //        return;
                //    }
                //    if (dt.Rows.Count > 0)
                //    {
                //        for (i = 0; i < dt.Rows.Count; i++)
                //        {
                //            foreach (Control ctl in controls)
                //            {
                //                if (ctl is WebBrowser)
                //                {
                //                    if (VB.Left(((WebBrowser)ctl).Name, 7) == "webInfo")
                //                    {
                //                        if (VB.Val(VB.Right(((WebBrowser)ctl).Name, 2)) == VB.Val(strWebBrowser))
                //                        {
                //                            ((WebBrowser)ctl).DocumentText += dt.Rows[i]["DESCRIPTION"].ToString().Trim();
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                //dt.Dispose();
                //dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void DrugInfoBodyText(string strGu)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            if (GstrYAKPUM_CD == "") return;

            try
            {
                SQL = "";
                switch (strGu)
                {
                    case "1":
                        SQL = SQL + ComNum.VBLF + "SELECT ";
                        SQL = SQL + ComNum.VBLF + "    KOSMOS_DRUG.uF_Get_IENGNM_HAM2(YAKPUM_CD, 0, 5) as druginfo";
                        SQL = SQL + ComNum.VBLF + "FROM KOSMOS_DRUG.GIYP_IN01";
                        SQL = SQL + ComNum.VBLF + "WHERE YAKPUM_CD = '" + GstrYAKPUM_CD + "'";
                        break;
                    case "2":
                        SQL = SQL + ComNum.VBLF + "SELECT                                                                            ";
                        SQL = SQL + ComNum.VBLF + "    KOSMOS_DRUG.uF_Get_Indication_Moa(K02.MOA_SEC, K02.MOA_NUM)  AS druginfo      ";
                        SQL = SQL + ComNum.VBLF + "FROM KOSMOS_DRUG.GIYP_IN01 YP                                                     ";
                        SQL = SQL + ComNum.VBLF + "INNER JOIN  KOSMOS_DRUG.FIRSTDIS_MOA_L01 K01                                      ";
                        SQL = SQL + ComNum.VBLF + "    ON YP.KGCN_SEQNO = K01.KGCN_SEQNO                                             ";
                        SQL = SQL + ComNum.VBLF + "INNER JOIN  KOSMOS_DRUG.FIRSTDIS_MOAK_DESC K02                                    ";
                        SQL = SQL + ComNum.VBLF + "    ON K01.MOA_NUM = K02.MOA_NUM                                                  ";
                        SQL = SQL + ComNum.VBLF + "WHERE YAKPUM_CD = '" + GstrYAKPUM_CD + "'                                         ";
                        SQL = SQL + ComNum.VBLF + "    AND MOA_SEC = 'M'                                                             ";
                        break;
                    case "3":
                        SQL = SQL + ComNum.VBLF + "SELECT                                                                 ";
                        SQL = SQL + ComNum.VBLF + "    KOSMOS_DRUG.UF_DRUG_THERAPY(YAKPUM_CD, 2) as druginfo              ";
                        SQL = SQL + ComNum.VBLF + "FROM KOSMOS_DRUG.GIYP_IN01                                             ";
                        SQL = SQL + ComNum.VBLF + "WHERE YAKPUM_CD = '" + GstrYAKPUM_CD + "'                                            ";
                        break;
                    case "4":
                        SQL = SQL + ComNum.VBLF + "SELECT                                                                ";
                        SQL = SQL + ComNum.VBLF + "    KOSMOS_DRUG.UF_DRUG_KINDM(YAKPUM_CD) as druginfo                  ";
                        SQL = SQL + ComNum.VBLF + "FROM KOSMOS_DRUG.GIYP_IN01                                            ";
                        SQL = SQL + ComNum.VBLF + "WHERE YAKPUM_CD = '" + GstrYAKPUM_CD + "'                                           ";
                        break;
                    case "5":
                        SQL = SQL + ComNum.VBLF + "SELECT                                                                ";
                        SQL = SQL + ComNum.VBLF + "    KOSMOS_DRUG.UF_DRUG_KDRC2(YAKPUM_CD) as druginfo                  ";
                        SQL = SQL + ComNum.VBLF + "FROM KOSMOS_DRUG.GIYP_IN01                                            ";
                        SQL = SQL + ComNum.VBLF + "WHERE YAKPUM_CD = '" + GstrYAKPUM_CD + "'                                           ";
                        break;
                    case "6":
                        SQL = SQL + ComNum.VBLF + "SELECT                                                                ";
                        SQL = SQL + ComNum.VBLF + "    KOSMOS_DRUG.UF_DRUG_KPREC(YAKPUM_CD) as druginfo                  ";
                        SQL = SQL + ComNum.VBLF + "FROM KOSMOS_DRUG.GIYP_IN01                                            ";
                        SQL = SQL + ComNum.VBLF + "WHERE YAKPUM_CD = '" + GstrYAKPUM_CD + "'                                           ";
                        break;
                    case "7":
                        SQL = SQL + ComNum.VBLF + "SELECT                                                                 ";
                        SQL = SQL + ComNum.VBLF + "    KOSMOS_DRUG.UF_DRUG_STORED(YAKPUM_CD) as druginfo                  ";
                        SQL = SQL + ComNum.VBLF + "FROM KOSMOS_DRUG.GIYP_IN01                                             ";
                        SQL = SQL + ComNum.VBLF + "WHERE YAKPUM_CD = '" + GstrYAKPUM_CD + "'                                            ";
                        break;
                    case "8":
                        SQL = SQL + ComNum.VBLF + "SELECT                                                                 ";
                        SQL = SQL + ComNum.VBLF + "    KOSMOS_DRUG.uF_Get_CCMM_LIST('01', YAKPUM_CD) as druginfo --한글   ";
                        SQL = SQL + ComNum.VBLF + "FROM KOSMOS_DRUG.GIYP_IN01                                             ";
                        SQL = SQL + ComNum.VBLF + "WHERE YAKPUM_CD = '" + GstrYAKPUM_CD + "'                                            ";
                        break;
                    case "9":
                        SQL = SQL + ComNum.VBLF + "SELECT                                                                 ";
                        SQL = SQL + ComNum.VBLF + "    KOSMOS_DRUG.uF_Get_CCMM_LIST('02', YAKPUM_CD) as druginfo --영문   ";
                        SQL = SQL + ComNum.VBLF + "FROM KOSMOS_DRUG.GIYP_IN01                                             ";
                        SQL = SQL + ComNum.VBLF + "WHERE YAKPUM_CD = '" + GstrYAKPUM_CD + "'                                            ";
                        break;
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }
                switch (strGu)
                {
                    case "1":
                        txtInfo00.Text = dt.Rows[0]["druginfo"].ToString().Trim();
                        break;
                    case "2":
                        txtInfo01.Text = dt.Rows[0]["druginfo"].ToString().Trim();
                        break;
                    case "3":
                        txtInfo02.Text = dt.Rows[0]["druginfo"].ToString().Trim();
                        break;
                    case "4":
                        txtInfo03.Text = dt.Rows[0]["druginfo"].ToString().Trim();
                        break;
                    case "5":
                        txtInfo04.Text = dt.Rows[0]["druginfo"].ToString().Trim();
                        break;
                    case "6":
                        txtInfo05.Text = dt.Rows[0]["druginfo"].ToString().Trim();
                        break;
                    case "7":
                        txtInfo06.Text = dt.Rows[0]["druginfo"].ToString().Trim();
                        break;
                    case "8":
                        txtInfo08.Text += dt.Rows[0]["druginfo"].ToString().Trim();
                        break;
                    case "9":
                        txtInfo08.Text += dt.Rows[0]["druginfo"].ToString().Trim();
                        break;
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }
        
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string READ_INOUT(string strSUNEXT)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SugbJ ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUT ";
                SQL = SQL + ComNum.VBLF + "     WHERE SUNEXT = '" + strSUNEXT + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SUGBJ"].ToString().Trim();
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

                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string READ_BUN(string strBun)
        {
            string rtnVal = "";

            switch (strBun)
            {
                case "11":
                    rtnVal = "경구";
                    break;
                case "12":
                    rtnVal = "외용";
                    break;
                case "20":
                case "23":
                    rtnVal = "주사";
                    break;
            }

            return rtnVal;
        }

        private void txtSuNext_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtSuNextKeyDown();
            }
        }

        private void txtSuNextKeyDown()
        {
            txtSuNext.Text = txtSuNext.Text.ToUpper();
            if (txtSuNext.Text.Trim() == "") { return; }

            Screen_display(txtSuNext.Text.Trim());
        }

        private void ssGubun_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssGubunCellClick(e.Row, e.Column);
        }

        private void ssGubunCellClick(int intRow, int intColumn)
        {
            ssGubun_Sheet1.Cells[0, 0, ssGubun_Sheet1.RowCount - 1, ssGubun_Sheet1.ColumnCount - 1].BackColor = Color.White;
            ssGubun_Sheet1.Cells[intRow, intColumn].BackColor = Color.FromArgb(178, 34, 34);

            ShowTextBox(ssGubun_Sheet1.Cells[intRow, intColumn].Tag.ToString());
        }

        private void ShowTextBox(string strGubun)
        {
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {
                if (ctl is TextBox)
                {
                    if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                    {
                        if (VB.Val(VB.Right(((TextBox)ctl).Name, 2)) < 12)
                        {
                            ((TextBox)ctl).Visible = false;
                            ((TextBox)ctl).Dock = DockStyle.None;
                        }
                    }
                }
            }

            if (VB.Val(strGubun) == 0) { return; }

            if (strGubun == "6")
            {
                ssSub.Visible = true;
                txtInfo12.Visible = true;
                txtInfo12.Dock = DockStyle.Fill;
            }
            else
            {
                ssSub.Visible = false;

                foreach (Control ctl in controls)
                {
                    if (ctl is TextBox)
                    {
                        if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                        {
                            if (VB.Val(VB.Right(((TextBox)ctl).Name, 2)) == (VB.Val(strGubun) - 1))
                            {
                                ((TextBox)ctl).Visible = true;
                                ((TextBox)ctl).Dock = DockStyle.Fill;
                                break;
                            }
                        }
                    }
                }

                ShowTextBoxSub("0");
            }
        }

        private void ShowTextBoxSub(string strGubun)
        {
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {
                if (ctl is TextBox)
                {
                    if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                    {
                        if (VB.Val(VB.Right(((TextBox)ctl).Name, 2)) >= 12 && VB.Val(VB.Right(((TextBox)ctl).Name, 2)) <= 27)
                        {
                            ((TextBox)ctl).Visible = false;
                            ((TextBox)ctl).Dock = DockStyle.None;
                        }
                    }
                }
            }

            if (VB.Val(strGubun) == 0) { return; }

            foreach (Control ctl in controls)
            {
                if (ctl is TextBox)
                {
                    if (VB.Left(((TextBox)ctl).Name, 7) == "txtInfo")
                    {
                        if (VB.Val(VB.Right(((TextBox)ctl).Name, 2)) == (VB.Val(strGubun) - 1))
                        {
                            ((TextBox)ctl).Visible = true;
                            ((TextBox)ctl).Dock = DockStyle.Fill;
                            break;
                        }
                    }
                }
            }
        }

        private void ssSub_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ShowTextBoxSub(ssSub_Sheet1.Cells[e.Row, e.Column].Tag.ToString());
        }

        private void ssGubunWeb_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssGubunWebCellClick(e.Row, e.Column);
        }

        private void ssGubunWebCellClick(int intRow, int intColumn)
        {
            ssGubunWeb_Sheet1.Cells[0, 0, ssGubunWeb_Sheet1.RowCount - 1, ssGubunWeb_Sheet1.ColumnCount - 1].BackColor = Color.White;
            ssGubunWeb_Sheet1.Cells[intRow, intColumn].BackColor = Color.FromArgb(178, 34, 34);
            
            ShowWebBrowser(ssGubunWeb_Sheet1.Cells[intRow, intColumn].Tag.ToString());
        }

        private void ShowWebBrowser(string strGubun)
        {
            Control[] controls = ComFunc.GetAllControls(this);

            txtInfo10.Visible = false;
            txtInfo10.Dock = DockStyle.None;

            foreach (Control ctl in controls)
            {
                if (ctl is WebBrowser)
                {
                    if (VB.Left(((WebBrowser)ctl).Name, 7) == "webInfo")
                    {
                        if (VB.Val(VB.Right(((WebBrowser)ctl).Name, 2)) < 12)
                        {
                            ((WebBrowser)ctl).Visible = false;
                            ((WebBrowser)ctl).Dock = DockStyle.None;
                        }
                    }
                }
            }

            if (VB.Val(strGubun) == 0) { return; }

            if (VB.Val(strGubun) == 10)
            {
                txtInfo10.Visible = true;
                txtInfo10.Dock = DockStyle.Fill;
            }
            else
            {
                foreach (Control ctl in controls)
                {
                    if (ctl is WebBrowser)
                    {
                        if (VB.Left(((WebBrowser)ctl).Name, 7) == "webInfo")
                        {
                            if (VB.Val(VB.Right(((WebBrowser)ctl).Name, 2)) == (VB.Val(strGubun)))
                            {
                                ((WebBrowser)ctl).Visible = true;
                                ((WebBrowser)ctl).Dock = DockStyle.Fill;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void file_chk_del(string filepath)
        {       
            try
            {
                File.Delete(filepath);
            }     
            catch(Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
                        

        }
    }
}
