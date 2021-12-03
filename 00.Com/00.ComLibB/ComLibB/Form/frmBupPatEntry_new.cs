using ComBase; //기본 클래스
using ComDbB; //DB연결
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB.dll
    /// File Name       : frmBupPatEntry_new.cs
    /// Description     : 건강보험(희귀난치성, 중증화상, 중증암) 신청서 / 수술예방적작성지 인쇄
    /// Author          : 김현욱
    /// Create Date     : 2017-06-17 2018-12-20 추가변경
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// frmBupPatEntry_new.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\buppat\FrmCancerPrt.frm
    /// VB\basic\buppat\Frm희귀난치환자상세등록.frm
    /// VB\basic\buppat\Frm중증화상환자상세등록.frm
    /// VB\basic\buppat\FrmCancerPrt2.frm
    /// </seealso>
    /// <vbp>
    /// default : VB\basic\buppat\buppat.vbp
    /// </vbp>
    public partial class frmBupPatEntry_new : Form
    {
        private string GstrPtNo = "";
        private string GstrIO = "";
        private string GstrDept = "";
        private string GstrDrCode = "";
        private string GstrOP = "";
        private string GstrRare = "";
        private string GstrSabun = "";
        private string GstrOK = "";
        private int GintIndex = 0;
        private bool GbolOrder = false;

        private int GintTimer = 0;

        private string GstrDATA = "";
        private string GstrROWID = "";
        private string GstrROWID2 = "";
        //private string GstrExt = "";
        private string GstrBi = "";

        private string GstrE000 = "";       //차상위 여부 확인(2019-08-06)

        clsPublic cpublic = new clsPublic();
        
        public frmBupPatEntry_new()
        {
            InitializeComponent();
        }

        public frmBupPatEntry_new(string strPtNo, int intIndex, bool bolOrder = false)
        {
            InitializeComponent();

            GstrPtNo = strPtNo;
            GintIndex = intIndex;
            GbolOrder = bolOrder;
        }

        public frmBupPatEntry_new(string strPtNo, string strIO, string strDept, string strDrCode, string strOP, string strRare, string strSabun)
        {
            InitializeComponent();

            GstrPtNo = strPtNo;
            GstrIO = strIO;
            GstrDept = strDept;
            GstrDrCode = strDrCode;
            GstrOP = strOP;
            GstrRare = strRare;
            GstrSabun = strSabun;
            GintIndex = 2;
            
        }

        private void frmBupPatEntry_new_Load(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            FormClear();

            read_sysdate();

            SetCbo();

            btnExt.Text = "▲내용입력";
            
            //2016-01-27 OCS프로그램에서 사용시
            //if (VB.UCase(App.EXEName).Trim() == "MTSOORDER")
            if (GbolOrder == true)
            {
                btnExt.Click += new EventHandler(btnExt_Click);
            }

            GintTimer = 0;

            //==========================
            this.Height = 918;
            grbTongbo.Text = "등록결과 통보방법";
            rdoTongbo0.Text = "알림톡";
            rdoTongbo1.Text = "이메일";

            rdo7.Checked = true;
            //rdo8.Checked = true;
            panel27.Visible = true;    // 버튼 옆 콤보박스
            cboGubun.Visible = true;
            dtpIO.Enabled = false;
            panel21.Enabled = false;
            panel25.Enabled = false;
            panel27.Visible = true;
            panel19.Enabled = true;

            rdo7.Checked = true;
            
            PanChkOld.Visible = false;
            PanChkNew.Visible = true;

            PanChkOld.Dock = DockStyle.None;
            PanChkNew.Dock = DockStyle.Fill;

            #region 최초진단 or 최종확진방법
            panel17.Height = 301;

            PanExamOld.Visible = false;
            PanExamNew.Visible = true;

            PanExamOld.Dock = DockStyle.None;
            PanExamNew.Dock = DockStyle.Fill;
            #endregion

            label15.Text = "최종확진방법";

            //================================




            //상단버튼설정
            //toolBohum4.Visible = false;
            //toolGub4.Visible = false;



            if (GintIndex == 0)
            {
                rdo8.Checked = true;
            }
            else if (GintIndex == 1)
            {
                rdo9.Checked = true;
            }
            else if (GintIndex == 2)
            {
                rdo7.Checked = true;
            }              

            try
            {
                if(GstrPtNo == "")
                {
                    panG.Visible = false;
                }
                else 
                {
                    txtPtNo.Text = GstrPtNo; 
                }
                
                if(GstrDept != "")
                {
                    ComFunc.ComboFind(cboDept, "L", 2, GstrDept);
                    ComFunc.ComboFind(cboDeptNew, "L", 2, GstrDept);

                    cboDr.Items.Clear();
                    cboDoct.Items.Clear();

                    SQL = "";
                    SQL = "SELECT DRCODE, DRNAME FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                        //SQL = SQL + ComNum.VBLF + " WHERE DRDEPT1 = '" + VB.Left(cboDept.Text, 2) + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE DRDEPT1 = '" + VB.Left(cboDeptNew.Text, 2) + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND TOUR ='N'";
                    SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if(SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if(dt.Rows.Count > 0)
                    {
                        cboDr.Items.Add("");
                        cboDoct.Items.Add("");                        

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            cboDr.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim());
                            cboDoct.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim());
                        }

                        cboDr.SelectedIndex = 0;
                        cboDoct.SelectedIndex = 0;
                    }

                    dt.Dispose();
                    dt = null;
                }
                                
                if(GstrDrCode != "")
                {
                    ComFunc.ComboFind(cboDr, "L", 4, GstrDrCode);
                    ComFunc.ComboFind(cboDoct, "L", 4, GstrDrCode);
                }

                #region 2019-01-03 안정수, 상단의 로직을 타더라도 콤보박스값들이 Null인경우... 
                read_DeptInfo("Basic", clsType.User.IdNumber);
                read_DeptInfo("New", clsType.User.IdNumber);
                #endregion

                //중증등록여부표시-원내

                SQL = "";
                SQL = "SELECT Gubun FROM " + ComNum.DB_PMPA + "BAS_CANCER";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + txtPtNo.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        if(dt.Rows[i]["GUBUN"].ToString().Trim() == "1") { lblCa1.Visible = true; }     //중증암
                        if(dt.Rows[i]["GUBUN"].ToString().Trim() == "2") { lblCa2.Visible = true; }     //산정특례
                        if(dt.Rows[i]["GUBUN"].ToString().Trim() == "3") { lblCa3.Visible = true; }     //중증화상
                    }
                }

                dt.Dispose();
                dt = null;

                //중증등록여부표시-공단
                SQL = "";
                SQL = "SELECT M2_Disreg2 ,M2_disreg4 ,M2_disreg5";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_NHIC ";
                SQL = SQL + ComNum.VBLF + "  WHERE  PANO ='" + txtPtNo.Text + "'  ";
                SQL = SQL + ComNum.VBLF + "   AND SendTime >= TRUNC(SYSDATE-30) ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY SENDTIME DESC  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        if(VB.Left(dt.Rows[i]["M2_disreg4"].ToString().Trim(), 1) == "V") { lblCa11.Visible = true; }   //중증암
                        if(VB.Left(dt.Rows[i]["M2_disreg2"].ToString().Trim(), 1) == "V") { lblCa11.Visible = true; }   //산정특례
                        if(VB.Left(dt.Rows[i]["M2_disreg5"].ToString().Trim(), 1) == "V") { lblCa11.Visible = true; }   //중증화상
                    }
                }

                dt.Dispose();
                dt = null;



                //차상위 여부 확인
                #region 차상위 여부 확인
                GstrE000 = "";

                SQL = " SELECT MCODE ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + txtPtNo.Text + "' ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE = TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "    AND MCODE = 'E000' ";
                SQL = SQL + ComNum.VBLF + "    AND BI IN ('11','12','13') ";
                SQL = SQL + ComNum.VBLF + "  UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT OGPDBUN ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.IPD_NEW_MASTER  ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + txtPtNo.Text + "' ";
                SQL = SQL + ComNum.VBLF + "    AND (JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "             OR OUTDATE = TRUNC(SYSDATE) ) ";
                SQL = SQL + ComNum.VBLF + "    AND OGPDBUN = 'E' ";
                SQL = SQL + ComNum.VBLF + "    AND BI IN ('11','12','13')      ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    GstrE000 = "■차상위";
                }

                dt.Dispose();
                dt = null; 
                #endregion



                txtPtNoEnter();

                if (GstrOP == "수술")
                {
                    toolBohum1.Enabled = false;
                    toolGub1.Enabled = false;
                }

                if (GstrRare == "희귀")
                {
                    rdo8.Checked = true;
                }

                READ_ILLDATA(txtPtNo.Text);
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string read_DrBunho(string argDRCODE, string argGUBUN)
        {
            //argGUBUN 1:의사면허번호, 2:전문의번호 3:세부전문의번호
            //사용안함
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strRtn = "";

            SQL = "";

            if (argGUBUN == "2")
            {
                //전문의
                SQL += ComNum.VBLF + " SELECT BUNHO  ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_ADM.INSA_MSTL A, KOSMOS_OCS.OCS_DOCTOR B ";
                SQL += ComNum.VBLF + "  WHERE B.DRCODE = '" + argDRCODE + "' ";
                SQL += ComNum.VBLF + "    AND A.SABUN = B.SABUN ";
                
            }
            else if (argGUBUN == "3")
            {
                //세부전문의
                SQL += ComNum.VBLF + " SELECT NAME, BUNHO, '1' GUBUN, CHIDATE FROM KOSMOS_ADM.INSA_MSTL ";
                SQL += ComNum.VBLF + " WHERE NAME LIKE '%전문의%' ";
                SQL += ComNum.VBLF + "   AND NAME LIKE '%분과%' ";
                SQL += ComNum.VBLF + "   AND GUBUN = '1' ";
                SQL += ComNum.VBLF + "   AND SABUN IN ";
                SQL += ComNum.VBLF + "                   (SELECT SABUN FROM KOSMOS_OCS.OCS_DOCTOR ";
                SQL += ComNum.VBLF + "                     WHERE DRCODE = '" + argDRCODE + "' ";
                SQL += ComNum.VBLF + "                       AND GBOUT = 'N') ";
            }
            else
            {
                //의사면허번호
                SQL += ComNum.VBLF + " SELECT MYEN_BUNHO BUNHO";
                SQL += ComNum.VBLF + "   FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B ";
                SQL += ComNum.VBLF + "  WHERE A.SABUN = B.SABUN ";
                SQL += ComNum.VBLF + "    AND B.DRCODE = '" + argDRCODE + "' ";
            }
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return strRtn;
            }

            if (dt.Rows.Count > 0)
            {
                strRtn = dt.Rows[0]["BUNHO"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            return strRtn;
       }

        /// <summary>
        /// Create : 안정수
        /// Date : 2019-01-03
        /// Description : 진료과 및 의사정보를 가져온다
        /// </summary>
        /// <param name="argSabun"></param>
        void read_DeptInfo(string argJob, string argSabun)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "A.DEPTCODE, (SELECT MAX(NAME)      ";
            SQL += ComNum.VBLF + "                           FROM KOSMOS_PMPA.BAS_BUSE B ";
            SQL += ComNum.VBLF + "                           WHERE 1=1";
            SQL += ComNum.VBLF + "                           AND B.DEPTCODE = A.DEPTCODE";
            SQL += ComNum.VBLF + "                           AND DELDATE IS NULL";
            SQL += ComNum.VBLF + "                           AND ORDFLAG <> 'N') AS DEPTNAME, A.DRCODE, A.DRNAME";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR A";
            SQL += ComNum.VBLF + "  WHERE 1=1";
            SQL += ComNum.VBLF + "      AND GBOUT <> 'Y'";
            SQL += ComNum.VBLF + "      AND SABUN ='" + ComFunc.SetAutoZero(argSabun.Trim(), 5) + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                if(argJob == "Basic")
                {
                    cboDept.Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[0]["DEPTNAME"].ToString().Trim();
                    cboDr.Text = dt.Rows[0]["DRCODE"].ToString().Trim() + "." + dt.Rows[0]["DRNAME"].ToString().Trim();
                    cboDeptNew.Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[0]["DEPTNAME"].ToString().Trim();
                    cboDoct.Text = dt.Rows[0]["DRCODE"].ToString().Trim() + "." + dt.Rows[0]["DRNAME"].ToString().Trim();
                }
                else if (argJob == "New")
                {
                    cboDeptNew.Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[0]["DEPTNAME"].ToString().Trim();
                    cboDoct.Text = dt.Rows[0]["DRCODE"].ToString().Trim() + "." + dt.Rows[0]["DRNAME"].ToString().Trim();
                    cboDept.Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[0]["DEPTNAME"].ToString().Trim();
                    cboDr.Text = dt.Rows[0]["DRCODE"].ToString().Trim() + "." + dt.Rows[0]["DRNAME"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        private void FormClear()
        {
            GstrOK = "";

            toolDelete.Visible = false;

            ssDiag_Sheet1.RowCount = 0;
            ssDiag_Sheet1.RowCount = 3;
            ssDiag_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            txtPtNo.Text = "";
            txtSName.Text = "";
            txtPName.Text = "";
            txtPJumin.Text = "";
            txtGKiho.Text = "";
            txtJumin.Text = "";
            txtTel.Text = "";
            txtHPhone.Text = "";
            txtJumin.Text = "";
            txtEmail.Text = "";

            cboDept.Items.Clear();
            cboDr.Items.Clear();
            cboIO.Items.Clear();

            cboDeptNew.Items.Clear();
            cboDoct.Items.Clear();

            txtILLCode.Text = "";
            txtIllName.Text = "";

            dtpCanDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));            

            txtCode2.Text = "";

            Control[] controls = ComFunc.GetAllControls(this);

            foreach(Control ctl in controls)
            {
                if(ctl is CheckBox)
                {
                    if(VB.Left(((CheckBox)ctl).Name, 9) == "chkJinDan" || VB.Left(((CheckBox)ctl).Name, 4) == "chkT")
                    {
                        ((CheckBox)ctl).Checked = false;
                    }
                }
            }

            txtRemark.Text = "";
            txtRemark2.Text = "";
            txtRemark3.Text = "";

            txtDongSName.Text = "";

            dtpDongDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            txtSinName.Text = "";
            txtSTel.Text = "";

            cboGan.Items.Clear();

            dtpSDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            btnSave.Enabled = false;

            rdoTongbo0.Checked = false;
            rdoTongbo1.Checked = false;

            grbCode2.Visible = false;

            lblCa1.Visible = false;
            lblCa2.Visible = false;
            lblCa3.Visible = false;

            lblCa11.Visible = false;
            lblCa21.Visible = false;
            lblCa31.Visible = false;

            txtRemark2.Visible = false;

            lstIllnamek.Items.Clear();
            grpIlls.Visible = false;

            //2018-12-27 안정수, 신규 생성 컨트롤들 값 초기화
            ComFunc.SetAllControlClear(PanChkNew);
            optSinGu0.Checked = true;
            //optI.Checked = true;
            optO.Checked = true;    //2018-01-04 기본 외래로 선택 되도록
            optWonbar.Checked = true;
            optION.Checked = true;

            ComFunc.SetAllControlClear(PanAll);
            cboDeptNew.Items.Clear();
            cboDoct.Items.Clear();
            ComFunc.SetAllControlClear(PanCancer);

            ComFunc.SetAllControlClear(Panrare);
            optFamN.Checked = true;
            ComFunc.SetAllControlClear(Pantuber);
            optTuberN.Checked = true;

            ComFunc.SetAllControlClear(PantuberS);
        }

        void screen_clear(string argJob = "")
        {
            //ComFunc.SetAllControlClear(PanChkNew);
            //optSinGu0.Checked = true;
            //optI.Checked = true;
            //optWonbar.Checked = true;
            //optION.Checked = true;

            if (argJob == "")
            {
                ComFunc.SetAllControlClear(PanAll);
               
            }
            else if(argJob == "1")
            {
                optSinGu0.Checked = true;

                txtIllCodeNew.Text = "";
                txtIllNameNew.Text = "";
                txtGiho.Text = "";
                //optI.Checked = true;
                optO.Checked = true;
                optWonbar.Checked = true;
                optION.Checked = true;

                dtpIO.Text = cpublic.strSysDate;
                dtpJinDate.Text = cpublic.strSysDate;

                //clsVbfunc.SetComboDept(clsDB.DbCon, cboDept);
                //clsVbfunc.SetComboDept(clsDB.DbCon, cboDeptNew);
            }
            ComFunc.SetAllControlClear(PanCancer);

            ComFunc.SetAllControlClear(Panrare);
            optFamN.Checked = true;
            ComFunc.SetAllControlClear(Pantuber);
            optTuberN.Checked = true;

            ComFunc.SetAllControlClear(PantuberS);
        }
        private void SetCbo()
        {
            clsVbfunc.SetBCodeCombo(clsDB.DbCon, cboGan, "ETC_중증환자관계", 1, true, "N");
            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept);
            clsVbfunc.SetComboDept(clsDB.DbCon, cboDeptNew);

            cboIO.Items.Clear();
            cboIO.Items.Add("O.외래");
            cboIO.Items.Add("I.입원");

            if (GstrIO != "")
            {
                ComFunc.ComboFind(cboIO, "L", 1, GstrIO);
            }
        }

        private void READ_ILLDATA(string strPtNo)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssDiag_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT PANO, GUBUN, ILLCODE1, ILLCODE2, ILLCODE3, ILLCODE4, ILLCODE5";
                SQL = SQL + ComNum.VBLF + "  FROM (";
                SQL = SQL + ComNum.VBLF + "   SELECT PANO, '중증암' GUBUN, ILLCODE1, ILLCODE2, ILLCODE3, ILLCODE4, ILLCODE5";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "BAS_CANCER";
                SQL = SQL + ComNum.VBLF + "   WHERE GUBUN = '1'";
                SQL = SQL + ComNum.VBLF + "       AND PANO = '" + strPtNo + "'";
                SQL = SQL + ComNum.VBLF + "   Union";
                SQL = SQL + ComNum.VBLF + "   SELECT PANO, '기타 산정특례' GUBUN, ILLCODE1, ILLCODE2, ILLCODE3, ILLCODE4, ILLCODE5";
                SQL = SQL + ComNum.VBLF + "   From KOSMOS_PMPA.BAS_CANCER";
                SQL = SQL + ComNum.VBLF + "   WHERE GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "       AND PANO = '" + strPtNo + "'";
                SQL = SQL + ComNum.VBLF + "   Union";
                SQL = SQL + ComNum.VBLF + "   SELECT PANO, '중증화상' GUBUN, ILLCODE1, ILLCODE2, ILLCODE3, ILLCODE4, ILLCODE5";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "BAS_CANCER";
                SQL = SQL + ComNum.VBLF + "   WHERE GUBUN = '3'";
                SQL = SQL + ComNum.VBLF + "       AND PANO = '" + strPtNo + "')";
                SQL = SQL + ComNum.VBLF + " ORDER BY GUBUN ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 0)
                {
                    ssDiag_Sheet1.RowCount = dt.Rows.Count + 4;
                    ssDiag_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        ssDiag_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssDiag_Sheet1.Cells[i, 1].Text = Read_Bas_ILL(dt.Rows[i]["ILLCODE1"].ToString().Trim());
                        ssDiag_Sheet1.Cells[i, 2].Text = Read_Bas_ILL(dt.Rows[i]["ILLCODE2"].ToString().Trim());
                        ssDiag_Sheet1.Cells[i, 3].Text = Read_Bas_ILL(dt.Rows[i]["ILLCODE3"].ToString().Trim());
                        ssDiag_Sheet1.Cells[i, 4].Text = Read_Bas_ILL(dt.Rows[i]["ILLCODE4"].ToString().Trim());
                        ssDiag_Sheet1.Cells[i, 5].Text = Read_Bas_ILL(dt.Rows[i]["ILLCODE5"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SetIllsListBox(string strSang)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            //string rtnVal = "";

            int i = 0;

            lstIllnamek.Items.Clear();
            grpIlls.Visible = false;

            grpIlls.Location = new Point(70, 9);

            string strGubun = "";

            if (rdo7.Checked == true)
            { strGubun = "0"; }
            else if (rdo8.Checked == true)
            { strGubun = "1"; }
            else if (rdo9.Checked == true && cboGubun.Text.Trim() == "결핵")
            { strGubun = "2"; }
            else if (rdo9.Checked == true && cboGubun.Text.Trim() == "중증화상")
            { strGubun = "3"; }
            else if (rdo9.Checked == true && cboGubun.Text.Trim() == "잠복결핵")
            { strGubun = "4"; }


            if (strGubun == "")
            { return; }
            else if (strGubun == "0")       //암
            {
                SQL = "  SELECT SUBSTR(ILLCODE || '        ', 1, 16) || ILLNAMEK || '     (V193)' ILLNAMEK   ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_ILLS_CANCER ";
                SQL += ComNum.VBLF + " WHERE REPLACE(ILLCODE, '.', '') LIKE '" + strSang + "%' ";
                SQL += ComNum.VBLF + "   ORDER BY ILLCODE ASC, ILLNAMEK ASC ";
            }
            else
            {
                SQL = "  SELECT SUBSTR(ILLCODE || '        ', 1, 16) || ILLNAMEK || '     (' || VCODE || ')' ILLNAMEK   ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_ILLS_H3 ";
                SQL += ComNum.VBLF + " WHERE REPLACE(ILLCODE, '.', '') LIKE '" + strSang + "%' ";
                SQL += ComNum.VBLF + "  GROUP BY ILLCODE, ILLNAMEK, VCODE ";
                SQL += ComNum.VBLF + "   ORDER BY ILLCODE ASC, ILLNAMEK ASC ";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    grpIlls.Visible = true;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        lstIllnamek.Items.Add(dt.Rows[i]["IllNameK"].ToString().Trim());
                    }
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

        }

        private string Read_Bas_ILL(string strSang)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT IllNameK FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                SQL = SQL + ComNum.VBLF + " WHERE IllCode = '" + strSang.Replace(".", "") + "'";
                SQL = SQL + ComNum.VBLF + " AND illclass ='1'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if(dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["IllNameK"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                
                return rtnVal;
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (GstrBi == "21" || GstrBi == "22")
            {
                ComFunc.MsgBox("자격이 의료급여입니다.. 신청서를 급여신청으로 출력하십시오.");
                return;
            }

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (cpublic.strSysDate.CompareTo("2019-03-01") < 0)
            {

            }

            if (SaveDataNew() == true)
            {
                FormClear();
            }

        }

        private bool SaveData()
        {
            //string SQL = "";
            //string SqlErr = "";
            //int intRowAffected = 0;
            //string strGbn = "";
            //string strGu = "";

            //저장 기능 사용 안함.(2021-01-07)
            return false;

            //strGu = (rdoGU0.Checked == true ? "1" : (rdoGU1.Checked == true ? "2" : "3"));

            //if (cboDr.Text.Trim() == "")
            //{
            //    ComFunc.MsgBox("의사를 선택해주세요.");
            //    return false;
            //}

            //Cursor.Current = Cursors.WaitCursor;

            //clsDB.setBeginTran(clsDB.DbCon);

            //try
            //{
            //    if(GstrROWID2 != "")
            //    {
            //        SQL = "";
            //        SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_CANCER_DTL";
            //        SQL = SQL + ComNum.VBLF + "     SET ";
            //        SQL = SQL + ComNum.VBLF + "         DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "' , ";
            //        SQL = SQL + ComNum.VBLF + "         DRCODE = '" + VB.Left(cboDr.Text, 4) + "', ";
            //        SQL = SQL + ComNum.VBLF + "         IPDOPD = '" + VB.Left(cboIO.Text, 1) + "',";
            //        SQL = SQL + ComNum.VBLF + "         CANDATE = TO_DATE('" + dtpCanDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ,";
            //        SQL = SQL + ComNum.VBLF + "         ILLCODE = '" + txtILLCode.Text + "' ,";

            //        if(chkJinDan0.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN1 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN1 = '0', "; }
            //        if(chkJinDan1.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN2 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN2 = '0', "; }
            //        if(chkJinDan2.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN3 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN3 = '0', "; }
            //        if(chkJinDan3.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN4 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN4 = '0', "; }
            //        if(chkJinDan4.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN5 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN5 = '0', "; }

            //        SQL = SQL + ComNum.VBLF + "         DTLGUBN = '" + strGbn + "' , ";
            //        SQL = SQL + ComNum.VBLF + "         REMARK1 = '" + txtRemark.Text + "' , ";

            //        if(chkJinDan5.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN6 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN6 = '0', "; }
            //        if(chkJinDan6.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN7 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN7 = '0', "; }
            //        if(chkJinDan7.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN8 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN8 = '0', "; }
            //        if(chkJinDan8.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN9 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN9 = '0', "; }
            //        if(chkJinDan9.Checked == true) { SQL = SQL + ComNum.VBLF + "         JINDAN10 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN10 = '0', "; }
            //        if(chkJinDan10.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN11 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN11 = '0', "; }

            //        SQL = SQL + ComNum.VBLF + "         REMARK2 = '" + txtRemark2.Text + "' , ";
            //        SQL = SQL + ComNum.VBLF + "         SDATE = TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ,";
            //        SQL = SQL + ComNum.VBLF + "         ASSENTIENT = '" + txtDongSName.Text.Trim() + "',";
            //        SQL = SQL + ComNum.VBLF + "         ASSENTIENTDATE = TO_DATE('" + dtpDongDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ,";
            //        SQL = SQL + ComNum.VBLF + "         NAME = '" + txtSinName.Text + "' ,";
            //        SQL = SQL + ComNum.VBLF + "         TEL = '" + txtSTel.Text + "', ";
            //        SQL = SQL + ComNum.VBLF + "         GANGE = '" + VB.Left(cboGan.Text, 2) + "',";
            //        SQL = SQL + ComNum.VBLF + "         ENTDATE = SYSDATE ";
            //        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + GstrROWID2 + "'  ";
            //    }
            //    else
            //    {
            //        SQL = "";
            //        SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_CANCER_DTL";
            //        SQL = SQL + ComNum.VBLF + "         (PANO, DEPTCODE , IPDOPD, CANDATE, ILLCODE, JINDAN1, JINDAN2, JINDAN3, JINDAN4, JINDAN5,";
            //        SQL = SQL + ComNum.VBLF + "         REMARK1, JINDAN6, JINDAN7, JINDAN8, JINDAN9, JINDAN10, JINDAN11, REMARK2, DRCODE,";
            //        SQL = SQL + ComNum.VBLF + "         SDATE, ASSENTIENT,ASSENTIENTDate, NAME, TEL, GANGE, SENDDATE,  RESDATE,   ENTDATE,GUBUN, DTLGUBN )";
            //        SQL = SQL + ComNum.VBLF + "VALUES ";
            //        SQL = SQL + ComNum.VBLF + "         (";
            //        SQL = SQL + ComNum.VBLF + "         '" + txtPtNo.Text + "','" + VB.Left(cboDept.Text, 2) + "', '" + VB.Left(cboIO.Text, 1) + "', ";
            //        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpCanDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
            //        SQL = SQL + ComNum.VBLF + "         '" + txtILLCode.Text + "', ";

            //        if(chkJinDan0.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
            //        if(chkJinDan1.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
            //        if(chkJinDan2.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
            //        if(chkJinDan3.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
            //        if(chkJinDan4.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }

            //        SQL = SQL + ComNum.VBLF + "         '" + txtRemark.Text + "' , ";

            //        if(chkJinDan5.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
            //        if(chkJinDan6.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
            //        if(chkJinDan7.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
            //        if(chkJinDan8.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
            //        if(chkJinDan9.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
            //        if(chkJinDan10.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }

            //        SQL = SQL + ComNum.VBLF + "         '" + txtRemark2.Text + "' , '" + VB.Left(cboDr.Text, 4) + "', ";
            //        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
            //        SQL = SQL + ComNum.VBLF + "         '" + txtDongSName.Text + "', TO_DATE('" + dtpDongDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
            //        SQL = SQL + ComNum.VBLF + "         '" + txtSinName.Text + "' , '" + txtSTel.Text + "', ";
            //        SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboGan.Text, 2) + "' ,'','',SYSDATE,'" + strGu + "','" + strGbn + "' ";
            //        SQL = SQL + ComNum.VBLF + "         ) ";
            //    }

            //    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            //    if(SqlErr != "")
            //    {
            //        clsDB.setRollbackTran(clsDB.DbCon);
            //        ComFunc.MsgBox(SqlErr);
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //        Cursor.Current = Cursors.Default;
            //        return false;
            //    }

            //    clsDB.setCommitTran(clsDB.DbCon);
            //    ComFunc.MsgBox("저장하였습니다.");
            //    Cursor.Current = Cursors.Default;

            //    return true;
            //}
            //catch(Exception ex)
            //{
            //    clsDB.setRollbackTran(clsDB.DbCon);
            //    ComFunc.MsgBox(ex.Message);
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //    return false;
            //}
        }


        private bool SaveDataNew()
        {
            
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0; 
            string strGbn = "";
            string strGu = "";
            string strGbn2 = "";
            string strIO = "";
            //string ILLCODE_STS = "";

            //rdo7 : 암, rdo8 : 희귀난치치매, rdo9 : 결핵화상
            //optSinGu0:신규암, 신규등록, optSinGu1:재등록암, 재등록 optSinGu2:중복암

            //신규, 재등록, 중복 등
            if(optSinGu0.Checked == true)
            {
                strGu = "0";
            }
            else if(optSinGu1.Checked == true)
            {
                strGu = "1";
            }
            else if(optSinGu2.Checked == true)
            {
                strGu = "2";
            }
            
            //상단 버튼 및 종류에 따른 구분자 설정부분 
            if(rdo7.Checked == true)
            {
                strGbn = "1";                
            }
            else if (rdo8.Checked == true)
            {
                strGbn = "2";

                if(cboGubun.Text == "희귀")
                {
                    strGbn2 = "2";
                }
                else if (cboGubun.Text == "중증난치")
                {
                    strGbn2 = "3";
                }
                else if (cboGubun.Text == "중증치매")
                {
                    strGbn2 = "4";
                }
            }
            else if (rdo9.Checked == true)
            {
                if(cboGubun.Text == "중증화상")
                {
                    strGbn = "3";
                }
                else if(cboGubun.Text == "결핵")
                {
                    strGbn = "2";
                    strGbn2 = "1";
                }
                else if (cboGubun.Text == "잠복결핵")
                {
                    strGbn = "2";
                    strGbn2 = "9";
                }
            }

            // 입원, 외래 구분 
            if(optI.Checked == true)
            {
                strIO = "I";
            }
            else if(optO.Checked == true)
            {
                strIO = "O";
            }

            ////상병명에서 원발, 전이 구분 
            //if(optWonbar.Checked == true)
            //{
            //    ILLCODE_STS = "1";
            //}
            //else if(optJeon.Checked == true)
            //{
            //    ILLCODE_STS = "2";
            //}

            if (cboDoct.Text.Trim() == "")
            {
                ComFunc.MsgBox("의사를 선택해주세요.");
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (GstrROWID2 != "")
                {                    
                    SQL = "";
                    SQL += "UPDATE " + ComNum.DB_PMPA + "BAS_CANCER_DTL";
                    SQL += ComNum.VBLF + "     SET ";
                    SQL += ComNum.VBLF + "    PANO =  '" + txtPtNo.Text.Trim() + "'                                                ";
                    SQL += ComNum.VBLF + "   ,DEPTCODE = '" + VB.Left(cboDeptNew.Text, 2) + "'                                     ";
                    SQL += ComNum.VBLF + "   ,IPDOPD = '" + strIO + "'                                                             ";
                    SQL += ComNum.VBLF + "   ,CANDATE = TO_DATE('" + dtpJinDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')    ";
                    SQL += ComNum.VBLF + "   ,ILLCODE = '" + txtIllCodeNew.Text.Trim() + "'                                               ";
                    SQL += ComNum.VBLF + "   ,ILLNAMEK = '" + txtIllNameNew.Text.Trim() + "'                                               ";

                    //영상검사 유무
                    if (chkXray.Checked == true || chkMRI.Checked == true || chkCT.Checked == true || chkSono.Checked == true || chkExamETC.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN1 = '1'                                                                    ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN1 = '0'                                                                    ";
                    }

                    //SONO
                    if (chkMRI.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN2 = '1'                                                                    ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN2 = '0'                                                                    ";
                    }

                    //CT
                    if (chkMRI.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN3 = '1'                                                                    ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN3 = '0'                                                                    ";
                    }

                    //MRI
                    if (chkMRI.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN4 = '1'                                                                    ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN4 = '0'                                                                    ";
                    }

                    //기타
                    if (chkExamETC.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN5 = '1'                                                                    ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN5 = '0'                                                                    ";
                    }

                    //기타란
                    if (txtExamETC.Text.Trim() != "")
                    {
                        SQL += ComNum.VBLF + "   ,REMARK1 = '" + txtExamETC.Text.Trim() + "'              ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,REMARK1 = ''              ";
                    }

                    //조직검사 없는 진단적 수술
                    if (chkNoneJojic.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN6  = '1'              ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN6  = '0'              ";
                    }
                    
                    SQL += ComNum.VBLF + "   ,JINDAN7 ='0'               ";
                    SQL += ComNum.VBLF + "   ,JINDAN8  ='0'               ";
                    SQL += ComNum.VBLF + "   ,JINDAN9  ='0'               ";
                    SQL += ComNum.VBLF + "   ,JINDAN10 ='0'               ";
                    if ((rdo7.Checked == true && chkETCExam.Checked == true) || (rdo8.Checked == true && chkJindanNew6.Checked == true)
                        || (rdo9.Checked == true && chkJindanNewT5.Checked == true))
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN11 ='1'              ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN11 ='0'              ";
                    }

                    if ((rdo7.Checked == true || txtETCExam.Text.Trim() != "") || (rdo8.Checked == true || txtJindanNew6.Text.Trim() != "")
                       || (rdo9.Checked == true || txtJindanNewT5.Text.Trim() != ""))
                    {
                        
                        if (rdo7.Checked == true)
                        {
                            SQL += ComNum.VBLF + "   ,REMARK2 = '"+ txtETCExam.Text.Trim() + "'               ";
                        }
                        else if (rdo8.Checked == true)
                        {
                            SQL += ComNum.VBLF + "   ,REMARK2 = '" + txtJindanNew6.Text.Trim() + "'               ";
                        }
                        else if (rdo9.Checked == true)
                        {
                            SQL += ComNum.VBLF + "   ,REMARK2 = '" + txtJindanNewT5.Text.Trim() + "'               ";
                        }
                    }
                        
                    SQL += ComNum.VBLF + "   ,DRCODE = '" + VB.Left(cboDoct.Text, 4) + "'                ";
                    SQL += ComNum.VBLF + "   ,SDATE = TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')                 ";
                    SQL += ComNum.VBLF + "   ,NAME = '" + txtSinName.Text + "'                  ";
                    SQL += ComNum.VBLF + "   ,TEL  = '" + txtTel.Text + "'                  ";
                    SQL += ComNum.VBLF + "   ,GANGE = '" + VB.Left(cboGan.Text, 2) + "'                 ";

                    if(optIOY.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    ,SENDDATE = TO_DATE('" + dtpIO.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') "; 
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,SENDDATE = ''              ";
                    }
                    
                    SQL += ComNum.VBLF + "   ,RESDATE  = ''              ";
                    SQL += ComNum.VBLF + "   ,ENTDATE  = SYSDATE              ";
                    SQL += ComNum.VBLF + "   ,ASSENTIENT = '" + txtDongSName.Text + "'            ";
                    SQL += ComNum.VBLF + "   ,ASSENTIENTDate = TO_DATE('" + dtpDongDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')         ";
                    SQL += ComNum.VBLF + "   ,GUBUN = '" + strGbn + "'                 ";
                    SQL += ComNum.VBLF + "   ,CODE2 = '" + txtGiho.Text.Trim() + "'                 ";
                    SQL += ComNum.VBLF + "   ,DTLGUBN  = '" + strGu + "'              ";
                    SQL += ComNum.VBLF + "   ,JINDAN3_ETC  = '" + txtCT.Text.Trim() + "'          ";
                    if (chkXray.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN12  = '1'             ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN12  = '0'             ";
                    }
                    if (optFamN.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,GAJOKYN = 'N'               ";
                    }
                    else if(optFamY.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,GAJOKYN = 'Y'               ";
                    }

                    if (chkFam1.Checked == true) { SQL += ComNum.VBLF + "      ,GAJOK_01 =  'Y' "; }                                    
                    else { SQL += ComNum.VBLF + "      ,GAJOK_01 =   'N' "; }

                    if (chkFam2.Checked == true) { SQL += ComNum.VBLF + "      ,GAJOK_02 =  'Y' "; }
                    else { SQL += ComNum.VBLF + "      ,GAJOK_02 =   'N' "; }

                    if (chkFam3.Checked == true) { SQL += ComNum.VBLF + "      ,GAJOK_03 =  'Y' "; }
                    else { SQL += ComNum.VBLF + "      ,GAJOK_03 =   'N' "; }

                    if (chkFam4.Checked == true) { SQL += ComNum.VBLF + "      ,GAJOK_04 =  'Y' "; }
                    else { SQL += ComNum.VBLF + "      ,GAJOK_04 =   'N' "; }

                    if (chkFam5.Checked == true) { SQL += ComNum.VBLF + "      ,GAJOK_05 =  'Y' "; }
                    else { SQL += ComNum.VBLF + "      ,GAJOK_05 =   'N' "; }

                    if (chkFam6.Checked == true) { SQL += ComNum.VBLF + "      ,GAJOK_06 =  'Y' "; }
                    else { SQL += ComNum.VBLF + "      ,GAJOK_06 =   'N' "; }

                    if (chkFam7.Checked == true) { SQL += ComNum.VBLF + "      ,GAJOK_07 =  'Y' "; }
                    else { SQL += ComNum.VBLF + "      ,GAJOK_07 =   'N' "; }

                    if (chkFam8.Checked == true) { SQL += ComNum.VBLF + "      ,GAJOK_08 =  'Y' "; }
                    else { SQL += ComNum.VBLF + "      ,GAJOK_08 =   'N' "; }

                    if (chkFam9.Checked == true) { SQL += ComNum.VBLF + "      ,GAJOK_09 =  'Y' "; }
                    else { SQL += ComNum.VBLF + "      ,GAJOK_09 =   'N' "; }

                    if (chkFam10.Checked == true) { SQL += ComNum.VBLF + "     ,GAJOK_10 =  'Y' "; }
                    else { SQL += ComNum.VBLF + "      ,GAJOK_10 =   'N' "; }

                    if (optWonbar.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,ILLCODE_STS = '1'           ";
                    }
                    else if(optJeon.Checked== true)
                    {
                        SQL += ComNum.VBLF + "   ,ILLCODE_STS ='2'           ";
                    }

                    if (chkJojic.Checked == true || chkJindanNewT3.Checked == true || chkJindanNew44.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_01 = '1'         ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_01 = '0'         ";
                    }

                    if (chkBioExam.Checked == true || chkJindanNew2.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_02 = '1'         ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_02 = '0'         ";
                    }

                    if (chkImmExam.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_03 = '1'         ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_03 = '0'         ";
                    }

                    if (chkCytoExam.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_04 = '1'         ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_04 = '0'         ";
                    }

                    if (chkBloodExam.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_05 = '1'         ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_05 = '0'         ";
                    }

                    if (chkCyto.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_061 = 'Y'         ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_061 = 'N'         ";
                    }

                    if (chkNoneCytoSayu1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_0611 = '1'         ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_0611 = '0'         ";
                    }

                    if (chkNoneCytoSayu2.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_0612 = '1'         ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_0612 = '0'         ";
                    }

                    if (chkNoneCytoSayu3.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_0613 = '1'         ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_0613 = '0'         ";
                    }

                    if (chkNoneCytoSayu4.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_0614 = '1'         ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_0614 = '0'         ";
                    }

                    if (chkNoneCytoSayu5.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_0615 = '" + txtNoneCytoSayu5.Text.Trim() + "'     ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_0615 = ''         ";
                    }

                    if (txtPatJinRemark.Text.Trim() != "")
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_062 = 'Y'         "; 
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_0621 = '" + txtPatJinRemark.Text.Trim() + "'     ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_062 = 'N'         ";
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_0621 = ''     ";
                    }

                    if (chkJindanNewT21.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_07 = '1'     ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_07 = '0'      ";
                    }

                    if (chkJindanNewT22.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_08 = '1'     ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_08 = '0'      ";
                    }

                    if (optTuberN.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_09 = 'Y'     ";
                    }
                    else if(optTuberY.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_09 = 'N'     ";
                    }

                    if (chkTuber1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_091 = '1'     ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_091 = '0'      ";
                    }

                    if (chkTuber2.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_092 = '1'     ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_092 = '0'      ";
                    }

                    if (chkTuber3.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_093 = '1'     ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_093 = '0'      ";
                    }

                    if (chkTuber5.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_094 = '1'     ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_094 = '0'      ";
                    }
                 
                    SQL += ComNum.VBLF + "   ,GUBUN2 = '" + strGbn2 + "'        ";

                    if(chkJindanNew3.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_10 = '1'      ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_10 = '0'      ";
                    }
                    ////////////잠복결핵추가
                    if (chkJindanNewS1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_21 = '1'      ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_21 = '0'      ";
                    }
                    if (chkJindanNewS2.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_22 = '1'      ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_22 = '0'      ";
                    }
                    if (chkJindanNewS3.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_23 = '1'      ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_23 = '0'      ";
                    }
                    if (chkJindanNewS4.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_24 = '1'      ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_24 = '0'      ";
                    }
                    if (chkJindanNewS5.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_25 = '1'      ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_25 = '0'      ";
                    }
                    if (chkJindanNewS6.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_26 = '1'      ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_26 = '0'      ";
                    }
                    if (chkJindanNewS6A.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_26A = '1'      ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_26A = '0'      ";
                    }
                    if (chkJindanNewS6B.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_26B = '1'      ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_26B = '0'      ";
                    }




                    if ((rdo8.Checked == true || txtJindanNew5.Text.Trim() != "") || (rdo9.Checked == true || txtJindanNewT4.Text.Trim() != ""))
                    {
                        if(rdo8.Checked == true)
                        {
                            SQL += ComNum.VBLF + "   ,JINDAN_NEW_11 = '"+ txtJindanNew5.Text.Trim() + "'        ";
                        }
                        else if(rdo9.Checked == true)
                        {
                            SQL += ComNum.VBLF + "   ,JINDAN_NEW_11 = '"+ txtJindanNewT4.Text.Trim() + "'        ";
                        }                        
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   ,JINDAN_NEW_11 = ''        ";
                    }


                    SQL += ComNum.VBLF + " WHERE ROWID = '" + GstrROWID2 + "'  ";                                    

                }
                else
                {                    
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_CANCER_DTL";
                    SQL = SQL + ComNum.VBLF + "         (PANO, DEPTCODE , IPDOPD, CANDATE, ILLCODE, ILLNAMEK, JINDAN1, JINDAN2, JINDAN3, JINDAN4, JINDAN5,";
                    SQL = SQL + ComNum.VBLF + "         REMARK1, JINDAN6, JINDAN7, JINDAN8, JINDAN9, JINDAN10, JINDAN11, REMARK2, DRCODE, SDATE, ";
                    SQL = SQL + ComNum.VBLF + "         NAME, TEL, GANGE, SENDDATE,  RESDATE, ENTDATE, ASSENTIENT, ASSENTIENTDate,  GUBUN, CODE2, ";
                    SQL = SQL + ComNum.VBLF + "         DTLGUBN, JINDAN3_ETC, JINDAN12 ,GAJOKYN ,GAJOK_01 ,GAJOK_02 ,GAJOK_03 ,GAJOK_04 ,GAJOK_05 ,GAJOK_06 ,";
                    SQL = SQL + ComNum.VBLF + "         GAJOK_07, GAJOK_08 ,GAJOK_09 ,GAJOK_10 ,ILLCODE_STS ,JINDAN_NEW_01 ,JINDAN_NEW_02 ,JINDAN_NEW_03 ,JINDAN_NEW_04 ,JINDAN_NEW_05, ";
                    SQL = SQL + ComNum.VBLF + "         JINDAN_NEW_061, JINDAN_NEW_0611 ,JINDAN_NEW_0612 ,JINDAN_NEW_0613 ,JINDAN_NEW_0614 ,JINDAN_NEW_0615 ,JINDAN_NEW_062 ,JINDAN_NEW_0621 ,JINDAN_NEW_07 ,JINDAN_NEW_08,";
                    SQL = SQL + ComNum.VBLF + "         JINDAN_NEW_09, JINDAN_NEW_091 ,JINDAN_NEW_092 ,JINDAN_NEW_093 ,JINDAN_NEW_094 ,GUBUN2, JINDAN_NEW_10, JINDAN_NEW_21,JINDAN_NEW_22,JINDAN_NEW_23,JINDAN_NEW_24,JINDAN_NEW_25,JINDAN_NEW_26,JINDAN_NEW_26A,JINDAN_NEW_26B ,JINDAN_NEW_11 )";                        
                    SQL = SQL + ComNum.VBLF + "VALUES ";
                    SQL = SQL + ComNum.VBLF + "         (";
                    SQL = SQL + ComNum.VBLF + "         '" + txtPtNo.Text + "', ";                                                  //PANO
                    SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboDeptNew.Text, 2) + "', ";                                   //DEPTCODE
                    SQL = SQL + ComNum.VBLF + "         '" + strIO + "', ";                                                         //IPDOPD
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpJinDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), "; //CANDATE
                    SQL = SQL + ComNum.VBLF + "         '" + txtIllCodeNew.Text.Trim() + "', ";                                            //ILLCODE
                    SQL = SQL + ComNum.VBLF + "         '" + txtIllNameNew.Text.Trim() + "', ";                                            //ILLCODE

                    //영상검사 유무
                    if (chkXray.Checked == true || chkMRI.Checked == true || chkCT.Checked == true || chkSono.Checked == true || chkExamETC.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1', ";                                                                  //JINDAN1
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0', ";                                                                  
                    }

                    //SONO
                    if (chkSono.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                 //JINDAN2
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0',  ";
                    }

                    //CT
                    if (chkCT.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                 //JINDAN3
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0', ";
                    }

                    //MRI
                    if (chkMRI.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                 //JINDAN4
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0', ";
                    }

                    //기타
                    if (chkExamETC.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                 //JINDAN5
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0', ";
                    }

                    //기타란
                    if (txtExamETC.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "        '" + txtExamETC.Text.Trim() + "', ";                                     //REMARK1
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '', ";
                    }

                    //조직검사 없는 진단적 수술
                    if (chkNoneJojic.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1', ";                                                                  //JINDAN6
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0', ";
                    }

                    //특수 생화학적 또는 면역학적 검사유무                    
                    SQL = SQL + ComNum.VBLF + "             '0', ";                                                                 //JINDAN7                                                    
                    //세포학적 또는 혈액학적 검사 유무                   
                    SQL = SQL + ComNum.VBLF + "             '0', ";                                                                 //JINDAN8
                    //전이부위의 조직학적 검사 유무
                    SQL = SQL + ComNum.VBLF + "             '0', ";                                                                 //JINDAN9
                    //원발부위의 조직학적 검사 유무
                    SQL = SQL + ComNum.VBLF + "             '0', ";                                                                 //JINDAN10                        
                    //기타유무
                    if((rdo7.Checked == true && chkETCExam.Checked == true) || (rdo8.Checked == true && chkJindanNew6.Checked == true)
                        || (rdo9.Checked == true && chkJindanNewT5.Checked == true))
                    {
                        SQL = SQL + ComNum.VBLF + "             '1', ";                                                             //JINDAN11
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "             '0', ";                                                            
                    }

                    //기타 체크한 경우
                    if ((rdo7.Checked == true || txtETCExam.Text.Trim() != "") || (rdo8.Checked == true || txtJindanNew6.Text.Trim() != "") 
                        || (rdo9.Checked == true || txtJindanNewT5.Text.Trim() != ""))
                    {
                        if (rdo7.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "         '" + txtETCExam.Text.Trim() + "', ";                                //REMARK2
                        }
                        else if(rdo8.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "         '" + txtJindanNew6.Text.Trim() + "', ";                             
                        }
                        else if(rdo9.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "         '" + txtJindanNewT5.Text.Trim() + "', ";                            
                        }
                    }                    
                    
                    SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboDoct.Text, 4) + "', ";                                      //DRCODE     
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";   //SDATE
                    SQL = SQL + ComNum.VBLF + "         '" + txtSinName.Text + "' , ";                                              //NAME
                    SQL = SQL + ComNum.VBLF + "         '" + txtTel.Text + "' , ";                                                  //TEL
                    SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboGan.Text, 2) + "' ,";                                       //GANGE
                    if(optIOY.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpIO.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),";   //SENDDATE
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         '' ,";                                                                      
                    }                    
                    SQL = SQL + ComNum.VBLF + "         '' ,";                                                                      //RESDATE
                    SQL = SQL + ComNum.VBLF + "         SYSDATE ,";                                                                 //ENTDATE
                    SQL = SQL + ComNum.VBLF + "         '" + txtDongSName.Text + "', ";                                             //ASSENTIENT
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpDongDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),"; //ASSENTIENTDATE
                    SQL = SQL + ComNum.VBLF + "         '" + strGbn + "', ";                                                        //GUBUN
                    SQL = SQL + ComNum.VBLF + "         '" + txtGiho.Text.Trim() + "', ";                                           //CODE2
                    //기타참고사항
                    //SQL = SQL + ComNum.VBLF + "         '', ";                                                                      //REMARK3
                    //신규, 중복, 재발 구분
                    SQL = SQL + ComNum.VBLF + "         '" + strGu + "', ";                                                         //DTLGUBN
                    // CT CHECK 후 기타
                    SQL = SQL + ComNum.VBLF + "         '" + txtCT.Text.Trim() + "', ";                                             //JINDAN3_ETC
                    // 최종확진 - XRAY
                    if(chkXray.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "         '1', ";                                                                 //JINDAN12
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         '0', ";                                                                  
                    }
                    

                    if(optFamN.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "         'N', ";                                                                  //GAJOKYN
                    }
                    else if(optFamY.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "         'Y', ";                                                                  
                    }

                    if (chkFam1.Checked == true) { SQL = SQL + ComNum.VBLF + "         'Y', "; }                                    //GAJOK_01
                    else { SQL = SQL + ComNum.VBLF + "         'N', "; }

                    if (chkFam2.Checked == true) { SQL = SQL + ComNum.VBLF + "         'Y', "; }                                    //GAJOK_02
                    else { SQL = SQL + ComNum.VBLF + "         'N', "; }

                    if (chkFam3.Checked == true) { SQL = SQL + ComNum.VBLF + "         'Y', "; }                                    //GAJOK_03
                    else { SQL = SQL + ComNum.VBLF + "         'N', "; }

                    if (chkFam4.Checked == true) { SQL = SQL + ComNum.VBLF + "         'Y', "; }                                    //GAJOK_04
                    else { SQL = SQL + ComNum.VBLF + "         'N', "; }

                    if (chkFam5.Checked == true) { SQL = SQL + ComNum.VBLF + "         'Y', "; }                                    //GAJOK_05
                    else { SQL = SQL + ComNum.VBLF + "         'N', "; }

                    if (chkFam6.Checked == true) { SQL = SQL + ComNum.VBLF + "         'Y', "; }                                    //GAJOK_06
                    else { SQL = SQL + ComNum.VBLF + "         'N', "; }

                    if (chkFam7.Checked == true) { SQL = SQL + ComNum.VBLF + "         'Y', "; }                                    //GAJOK_07
                    else { SQL = SQL + ComNum.VBLF + "         'N', "; }

                    if (chkFam8.Checked == true) { SQL = SQL + ComNum.VBLF + "         'Y', "; }                                    //GAJOK_08
                    else { SQL = SQL + ComNum.VBLF + "         'N', "; }

                    if (chkFam9.Checked == true) { SQL = SQL + ComNum.VBLF + "         'Y', "; }                                    //GAJOK_09
                    else { SQL = SQL + ComNum.VBLF + "         'N', "; }

                    if (chkFam10.Checked == true) { SQL = SQL + ComNum.VBLF + "         'Y', "; }                                   //GAJOK_10
                    else { SQL = SQL + ComNum.VBLF + "         'N', "; }

                    if(optWonbar.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "         '1', ";                                                                 //ILLCODE_STS
                    }
                    else if(optJeon.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "         '2', ";                                                                 
                    }

                    //조직학적검사 
                    if (chkJojic.Checked == true || chkJindanNewT3.Checked == true || chkJindanNew44.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_01
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0',  ";
                    }
                    //특수생화학적
                    if (chkBioExam.Checked == true || chkJindanNew2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_02
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0',  ";                                             
                    }
                    //면역학적
                    if (chkImmExam.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_03
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0',  ";
                    }
                    //세포학적
                    if (chkCytoExam.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_04
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0',  ";
                    }
                    //혈액학적
                    if (chkBloodExam.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_05
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0',  ";
                    }
                    //조직학적 세포검사 - 상별필수체크
                    if(chkCyto.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        'Y' , ";                                                                  //JINDAN_NEW_061
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        'N' , ";                                                                  
                    }

                    //조직학적 세포검사 사유 1 ~ 5
                    if(chkNoneCytoSayu1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_0611
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0',  ";
                    }
                    if(chkNoneCytoSayu2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_0612
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0',  ";
                    }
                    if(chkNoneCytoSayu3.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_0613
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0',  ";
                    }
                    if(chkNoneCytoSayu4.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_0614
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0',  ";
                    }
                    if(chkNoneCytoSayu5.Checked == true || txtNoneCytoSayu5.Text.Trim() !="")
                    {
                        SQL = SQL + ComNum.VBLF + "        '"+ txtNoneCytoSayu5.Text.Trim() + "' , ";                                 //JINDAN_NEW_0615
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '',  ";
                    }

                    //세포학적 검사 불가 충족 및 내용
                    if(txtPatJinRemark.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "        'Y' , ";                                                                    //JINDAN_NEW_062
                        SQL = SQL + ComNum.VBLF + "        '" + txtPatJinRemark.Text.Trim() + "' , ";                                 //JINDAN_NEW_0621
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        'N' , ";                                
                        SQL = SQL + ComNum.VBLF + "        '' , ";                                 
                    }

                    //도말
                    if(chkJindanNewT21.Checked == true)
                    {                            
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_07
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0' , ";
                    }

                    //배양검사
                    if(chkJindanNewT22.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_08
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0' , ";
                    }

                    //결핵 확진 여부 Y,N
                    if(optTuberN.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        'N' , ";                                                                  //JINDAN_NEW_09
                    }
                    else if(optTuberY.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        'Y' , ";
                    }

                    if(chkTuber1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_091
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0' , ";
                    }
                    if(chkTuber2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_092
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0' , ";
                    }
                    if (chkTuber3.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_093
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0' , ";
                    }
                    if (chkTuber5.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_094
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0' , ";
                    }

                    //GUBUN2
                    SQL = SQL + ComNum.VBLF + "        '" + strGbn2 + "', ";

                    if (chkJindanNew3.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_10
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0' , ";
                    }
                    /////잠복결핵추가 
                    if (chkJindanNewS1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_10
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0' , ";
                    }
                    if (chkJindanNewS2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_10
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0' , ";
                    }
                    if (chkJindanNewS3.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_10
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0' , ";
                    }
                    if (chkJindanNewS4.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_10
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0' , ";
                    }
                    if (chkJindanNewS5.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_10
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0' , ";
                    }
                    if (chkJindanNewS6.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_10
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0' , ";
                    }
                    if (chkJindanNewS6A.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_10
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0' , ";
                    }
                    if (chkJindanNewS6B.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "        '1' , ";                                                                  //JINDAN_NEW_10
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '0' , ";
                    }




                    if ((rdo8.Checked == true || txtJindanNew5.Text.Trim() != "") || (rdo9.Checked == true || txtJindanNewT4.Text.Trim() != ""))
                    {
                        if(rdo8.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "        '" + txtJindanNew5.Text.Trim() + "'  ) ";                              //JINDAN_NEW_11
                        }
                        else if(rdo9.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "        '" + txtJindanNewT4.Text.Trim() + "' ) ";                              
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        '') ";
                    }


                        

                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnillhCode_Click(object sender, EventArgs e)
        {
            frmILLHCode frm = new frmILLHCode();
            frm.Show();
        }

        private void btnExt_Click(object sender, EventArgs e)
        {
            if (btnExt.Text == "▼내용입력")
            {
                this.Height = 918;
                btnExt.Text = "▲내용입력";
            }
            else if (btnExt.Text == "▲내용입력")
            {
                this.Height = 431;
                btnExt.Text = "▼내용입력";
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtGKiho.Text = "";
            txtPJumin.Text = "";

            txtSinName.Text = "";

            txtDongSName.Text = "";
            txtSTel.Text = "";

            cboGan.Text = "";
        }

        private void txtPtNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPtNoEnter();
            }
        }

        private void txtPtNoEnter()
        {
            string SQL = "";
            DataTable dt = null;
            //DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;

            //if(ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            //FormClear();

            if(txtPtNo.Text.Trim().Length > 8)
            {
                ComFunc.MsgBox("등록번호를 확인해주세요!");
                return;
            }
            else
            {
                txtPtNo.Text = ComFunc.SetAutoZero(txtPtNo.Text.Trim(), 8);
            }

            try
            {
                if(VB.Left(cboIO.Text, 1) == "O" || optO.Checked == true)
                {
                    SQL = "";
                    SQL = "SELECT BI, AGE, SEX FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + txtPtNo.Text + "'";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE =TRUNC(SYSDATE)";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if(SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if(dt.Rows.Count > 0)
                    {
                        txtAge.Text = dt.Rows[0]["AGE"].ToString().Trim();
                        txtSex.Text = dt.Rows[0]["SEX"].ToString().Trim();

                        if (dt.Rows[0]["BI"].ToString().Trim() == "21" || dt.Rows[0]["BI"].ToString().Trim() == "22")
                        {
                            toolBohum1.Enabled = false;
                            toolBohum2.Enabled = false;
                            toolGub1.Enabled = true;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                else if (VB.Left(cboIO.Text, 1) == "I" || optI.Checked == true)
                {
                    SQL = "";
                    SQL = "SELECT BI, AGE, SEX FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + txtPtNo.Text + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND (ACTDATE IS NULL OR ACTDATE = TRUNC(SYSDATE)) ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if(SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if(dt.Rows.Count > 0)
                    {
                        txtAge.Text = dt.Rows[0]["AGE"].ToString().Trim();
                        txtSex.Text = dt.Rows[0]["SEX"].ToString().Trim();

                        if(dt.Rows[0]["BI"].ToString().Trim() == "21" || dt.Rows[0]["BI"].ToString().Trim() == "22")
                        {
                            toolBohum1.Enabled = false;
                            toolBohum2.Enabled = false;
                            toolGub1.Enabled = true;

                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                txtPtNo.Text = ComFunc.LPAD(txtPtNo.Text, 8, "0");
                GstrDATA = "";
                GstrBi = "";

                //환자 마스트 읽기
                SQL = "";
                SQL = "SELECT A.SNAME, A.PNAME, A.JUMIN1 ,A.JUMIN2, JUMIN3,  A.GKIHO, A.ZIPCODE1, A.ZIPCODE2, A.JUSO, A.TEL,a.bi,";
                SQL = SQL + ComNum.VBLF + " A.GKIHO, A.KIHO, A.ROWID, B.MAILJUSO, C.MIANAME, A.HPHONE, A.ROADDETAIL, A.ZIPCODE3, A.BUILDNO  ";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "BAS_PATIENT A, " + ComNum.DB_PMPA + "BAS_MAILNEW B, " + ComNum.DB_PMPA + "BAS_MIA C";
                SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + txtPtNo.Text + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.ZIPCODE1 || A.ZIPCODE2 = B.MAILCODE(+)";
                SQL = SQL + ComNum.VBLF + "   AND A.KIHO = C.MIACODE(+)";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 0)
                {
                    GstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    GstrBi = dt.Rows[0]["BI"].ToString().Trim();

                    txtSName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    txtGKiho.Text = dt.Rows[0]["GKIHO"].ToString().Trim();
                    txtPName.Text = dt.Rows[0]["PNAME"].ToString().Trim();

                    //주민암호화
                    if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    {
                        txtJumin.Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    }
                    else
                    {
                        txtJumin.Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + dt.Rows[0]["JUMIN2"].ToString().Trim();
                    }

                    GstrDATA = "기관기호:" + dt.Rows[0]["KIHO"].ToString().Trim() + "(" + dt.Rows[0]["MIANAME"].ToString().Trim() + ")";

                    btnSave.Enabled = true;

                    //자동으로 불러오도록 
                    //if (ComFunc.MsgBoxQ("주소,휴대전화,자택전화,통보방법(기본SMS)을 기존병원 데이타로 불러오시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    //{
                        //2016-01-18
                        if(dt.Rows[0]["BUILDNO"].ToString().Trim() != "")
                        {
                            txtJuso.Text = clsVbfunc.GetRoadJuSo(clsDB.DbCon, dt.Rows[0]["BUILDNO"].ToString().Trim()) + " " + dt.Rows[0]["ROADDETAIL"].ToString().Trim();
                        }
                        else
                        {
                            txtJuso.Text = dt.Rows[0]["MAILJUSO"].ToString().Trim() + " " + dt.Rows[0]["JUSO"].ToString().Trim();
                        }

                        txtTel.Text = dt.Rows[0]["TEL"].ToString().Trim();
                        txtHPhone.Text = dt.Rows[0]["HPHONE"].ToString().Trim();

                        rdoTongbo0.Checked = true;
                    //}
                }

                dt.Dispose();
                dt = null;

                //
                read_patinfo();

                //중증등록여부표시-원내
                SQL = "";
                SQL = "SELECT Gubun FROM KOSMOS_PMPA.BAS_CANCER";
                SQL = SQL + ComNum.VBLF + "     WHERE  PANO ='" + txtPtNo.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        if(dt.Rows[i]["GUBUN"].ToString().Trim() == "1") { lblCa1.Visible = true; }        //중증암
                        if(dt.Rows[i]["GUBUN"].ToString().Trim() == "2") { lblCa2.Visible = true; }        //산정특례
                        if(dt.Rows[i]["GUBUN"].ToString().Trim() == "3") { lblCa3.Visible = true; }        //중증화상
                    }
                }

                dt.Dispose();
                dt = null;

                INSERT_ONHIC(txtPtNo.Text, txtSName.Text, txtJumin.Text.Replace("-", ""));
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void read_patinfo()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;            

            GstrROWID2 = "";
            screen_clear("1");

            if(txtPtNo.Text.Trim() == "")
            {
                return;
            }

            //TODO : 기존, 신규 구분하여 읽어와야하는 부분 구현 필요 
            //암 및 희귀난치성환자 신청시 READ
            SQL = "";
            SQL = "SELECT PANO, DEPTCODE , IPDOPD, CANDATE, ILLCODE,Code2, JINDAN1, JINDAN2, JINDAN3, JINDAN4, JINDAN5,";
            SQL = SQL + ComNum.VBLF + "     REMARK1, JINDAN6, JINDAN7, JINDAN8, JINDAN9, JINDAN10, JINDAN11, REMARK2,REMARK3, DRCODE,";
            SQL = SQL + ComNum.VBLF + "     TO_CHAR(SDATE,'YYYY-MM-DD') SDATE,  NAME, TEL, GANGE,PJumin, ";
            SQL = SQL + ComNum.VBLF + "     TO_CHAR(SENDDATE,'YYYY-MM-DD') SENDATE, TO_CHAR(RESDATE,'YYYY-MM-DD') RESDATE, ";
            SQL = SQL + ComNum.VBLF + "     TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, ";
            SQL = SQL + ComNum.VBLF + "     ASSENTIENT, TO_CHAR(ASSENTIENTDATE,'YYYY-MM-DD') ASSENTIENTDATE,HPhone,Email,TongboGbn, GUBUN, CODE2, nvl(DTLGUBN,'0') DTLGUBN,";
            SQL = SQL + ComNum.VBLF + "     JINDAN3_ETC, JINDAN12 ,GAJOKYN ,GAJOK_01 ,GAJOK_02 ,GAJOK_03 ,GAJOK_04 ,GAJOK_05 ,GAJOK_06 ,";
            SQL = SQL + ComNum.VBLF + "     GAJOK_07, GAJOK_08 ,GAJOK_09 ,GAJOK_10 ,ILLCODE_STS ,JINDAN_NEW_01 ,JINDAN_NEW_02 ,JINDAN_NEW_03 ,JINDAN_NEW_04 ,JINDAN_NEW_05,";
            SQL = SQL + ComNum.VBLF + "     JINDAN_NEW_061, JINDAN_NEW_0611 ,JINDAN_NEW_0612 ,JINDAN_NEW_0613 ,JINDAN_NEW_0614 ,JINDAN_NEW_0615 ,JINDAN_NEW_062 ,JINDAN_NEW_0621 ,JINDAN_NEW_07 ,JINDAN_NEW_08,";
            SQL = SQL + ComNum.VBLF + "     JINDAN_NEW_21, JINDAN_NEW_22 ,JINDAN_NEW_23 ,JINDAN_NEW_24 ,JINDAN_NEW_25 ,JINDAN_NEW_26 ,JINDAN_NEW_26A ,JINDAN_NEW_26B,";
            SQL = SQL + ComNum.VBLF + "     JINDAN_NEW_09, JINDAN_NEW_091 ,JINDAN_NEW_092 ,JINDAN_NEW_093 ,JINDAN_NEW_094 ,GUBUN2, JINDAN_NEW_10, JINDAN_NEW_11, SENDDATE,";
            SQL = SQL + ComNum.VBLF + "     ROWID, ILLNAMEK ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_CANCER_DTL ";
            SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + txtPtNo.Text + "' ";

            if (rdo7.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "     AND GUBUN ='1' ";       // 암
            }
            else if (rdo8.Checked == true)
            {                    
                SQL = SQL + ComNum.VBLF + "     AND GUBUN ='2' ";       // 희귀,난치,치매

                if(cboGubun.Text == "희귀")
                {
                    SQL = SQL + ComNum.VBLF + "     AND GUBUN2 ='2' ";       // 희귀
                }
                else if (cboGubun.Text == "중증난치")
                {
                    SQL = SQL + ComNum.VBLF + "     AND GUBUN2 ='3' ";       // 희귀
                }
                else if (cboGubun.Text == "중증치매")
                {
                    SQL = SQL + ComNum.VBLF + "     AND GUBUN2 ='4' ";       // 희귀
                }
            }
            else if (rdo9.Checked == true)
            {                    
                if (cboGubun.Text == "결핵")
                {
                    SQL = SQL + ComNum.VBLF + "     AND GUBUN = '2' ";       // 결핵
                    SQL = SQL + ComNum.VBLF + "     AND GUBUN2 ='1' ";       
                }
                if (cboGubun.Text == "잠복결핵")
                {
                    SQL = SQL + ComNum.VBLF + "     AND GUBUN = '2' ";       // 결핵
                    SQL = SQL + ComNum.VBLF + "     AND GUBUN2 ='9' ";
                }
                else if (cboGubun.Text == "중증화상")
                {
                    SQL = SQL + ComNum.VBLF + "     AND GUBUN = '3' ";       // 화상                        
                }
            }
            else
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            SQL = SQL + ComNum.VBLF + "ORDER BY CANDATE DESC ";       // 암


            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                if(cboDept.SelectedIndex == 0)
                ComFunc.ComboFind(cboDept, "L", 2, dt.Rows[0]["DEPTCODE"].ToString().Trim());
                ComFunc.ComboFind(cboDeptNew, "L", 2, dt.Rows[0]["DEPTCODE"].ToString().Trim());

                SQL = "";
                SQL = "SELECT DRCODE, DRNAME FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                SQL = SQL + ComNum.VBLF + " WHERE DRDEPT1 = '" + dt.Rows[0]["DEPTCODE"].ToString().Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TOUR ='N'";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt1.Rows.Count > 0)
                {
                    cboDr.Items.Clear();
                    cboDr.Items.Add("");

                    cboDoct.Items.Clear();
                    cboDoct.Items.Add("");

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        cboDr.Items.Add(dt1.Rows[i]["DRCODE"].ToString().Trim() + "." + dt1.Rows[i]["DRNAME"].ToString().Trim());
                        cboDoct.Items.Add(dt1.Rows[i]["DRCODE"].ToString().Trim() + "." + dt1.Rows[i]["DRNAME"].ToString().Trim());
                    }

                    cboDr.SelectedIndex = 0;
                    cboDoct.SelectedIndex = 0;
                }

                dt1.Dispose();
                dt1 = null;

                ComFunc.ComboFind(cboDr, "L", 4, dt.Rows[0]["DRCODE"].ToString().Trim());
                ComFunc.ComboFind(cboIO, "L", 1, dt.Rows[0]["IPDOPD"].ToString().Trim());

                ComFunc.ComboFind(cboDoct, "L", 4, dt.Rows[0]["DRCODE"].ToString().Trim());



                if (dt.Rows[0]["IPDOPD"].ToString().Trim() == "I")
                {
                    optI.Checked = true;
                }
                else
                {
                    optO.Checked = true;
                }

                                              
                //영상검사유무
                if (dt.Rows[0]["JINDAN1"].ToString().Trim() == "1") { chkRDExam.Checked = true; }
                //SONO
                if (dt.Rows[0]["JINDAN2"].ToString().Trim() == "1") { chkSono.Checked = true; }
                //CT
                if (dt.Rows[0]["JINDAN3"].ToString().Trim() == "1") { chkCT.Checked = true; }
                //MRI
                if (dt.Rows[0]["JINDAN4"].ToString().Trim() == "1") { chkMRI.Checked = true; }
                //기타
                if (dt.Rows[0]["JINDAN5"].ToString().Trim() == "1") { chkExamETC.Checked = true; }
                //조직검사 
                if (dt.Rows[0]["JINDAN6"].ToString().Trim() == "1") { chkNoneJojic.Checked = true; }
                //특수 생화학적 또는 면역학적 검사유무                    
                if (dt.Rows[0]["JINDAN7"].ToString().Trim() == "1") { chkJinDan6.Checked = true; }
                //세포학적 또는 혈액학적 검사 유무                   
                if (dt.Rows[0]["JINDAN8"].ToString().Trim() == "1") { chkJinDan7.Checked = true; }
                //전이부위의 조직학적 검사 유무
                if (dt.Rows[0]["JINDAN9"].ToString().Trim() == "1") { chkJinDan8.Checked = true; }
                //원발부위의 조직학적 검사 유무
                if (dt.Rows[0]["JINDAN10"].ToString().Trim() == "1") { chkJinDan9.Checked = true; }
                //기타유무
                   
                if (dt.Rows[0]["JINDAN11"].ToString().Trim() == "1")
                { 
                    if(rdo7.Checked == true)
                    {
                        chkETCExam.Checked = true;
                    }
                    else if (rdo8.Checked == true)
                    {
                        chkJindanNew6.Checked = true;
                    }
                    else if (rdo9.Checked == true)
                    {
                        chkJindanNewT5.Checked = true;
                    }
                }

                //Xray
                if (dt.Rows[0]["JINDAN12"].ToString().Trim() == "1")
                {
                    chkXray.Checked = true;
                }

                if (dt.Rows[0]["REMARK2"].ToString().Trim() != "")
                {
                    if (rdo7.Checked == true)
                    {
                        txtETCExam.Text = dt.Rows[0]["REMARK2"].ToString().Trim();
                        chkETCExam.Checked = true;
                    }
                    else if (rdo8.Checked == true)
                    {
                        txtJindanNew6.Text = dt.Rows[0]["REMARK2"].ToString().Trim();
                        chkJindanNew6.Checked = true;
                    }
                    else if (rdo9.Checked == true)
                    {
                        txtJindanNewT5.Text = dt.Rows[0]["REMARK2"].ToString().Trim();
                        chkJindanNewT5.Checked = true;
                    }
                }                   

                //신규(2019-01-01 이후)

                if (dt.Rows[0]["GUBUN"].ToString().Trim() == "1")
                {
                    rdo7.Checked = true;
                }
                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "2")
                {
                    if(dt.Rows[0]["GUBUN2"].ToString().Trim() == "1" || dt.Rows[0]["GUBUN2"].ToString().Trim() == "9")
                    {
                        rdo9.Checked = true;
                    }
                    else
                    {
                        rdo8.Checked = true;
                    }
                }
                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "3")
                {
                    rdo9.Checked = true;
                }

                if (dt.Rows[0]["JINDAN3_ETC"].ToString().Trim() != "")
                {
                    txtCT.Text = dt.Rows[0]["JINDAN3_ETC"].ToString().Trim();
                }

                if(dt.Rows[0]["GAJOKYN"].ToString().Trim() == "N")
                {
                    optFamN.Checked = true;
                }
                else if (dt.Rows[0]["GAJOKYN"].ToString().Trim() == "Y")
                {
                    optFamY.Checked = true;
                }

                if(dt.Rows[0]["GAJOK_01"].ToString().Trim() == "Y")
                {
                    chkFam1.Checked = true;
                }
                if (dt.Rows[0]["GAJOK_02"].ToString().Trim() == "Y")
                {
                    chkFam2.Checked = true;
                }
                if (dt.Rows[0]["GAJOK_03"].ToString().Trim() == "Y")
                {
                    chkFam3.Checked = true;
                }
                if (dt.Rows[0]["GAJOK_04"].ToString().Trim() == "Y")
                {
                    chkFam4.Checked = true;
                }
                if (dt.Rows[0]["GAJOK_05"].ToString().Trim() == "Y")
                {
                    chkFam5.Checked = true;
                }
                if (dt.Rows[0]["GAJOK_06"].ToString().Trim() == "Y")
                {
                    chkFam6.Checked = true;
                }
                if (dt.Rows[0]["GAJOK_07"].ToString().Trim() == "Y")
                {
                    chkFam7.Checked = true;
                }
                if (dt.Rows[0]["GAJOK_08"].ToString().Trim() == "Y")
                {
                    chkFam8.Checked = true;
                }
                if (dt.Rows[0]["GAJOK_09"].ToString().Trim() == "Y")
                {
                    chkFam9.Checked = true;
                }
                if (dt.Rows[0]["GAJOK_10"].ToString().Trim() == "Y")
                {
                    chkFam10.Checked = true;
                }

                if (dt.Rows[0]["ILLCODE_STS"].ToString().Trim() == "1")
                {
                    optWonbar.Checked = true;
                }
                else if(dt.Rows[0]["ILLCODE_STS"].ToString().Trim() == "2")
                {
                    optJeon.Checked = true;
                }

                if (dt.Rows[0]["JINDAN_NEW_01"].ToString().Trim() == "1")
                {
                    if (rdo7.Checked == true)
                    {
                        chkJojic.Checked = true;
                    }
                    else if (rdo8.Checked == true)
                    {
                        chkJindanNew44.Checked = true;
                    }
                    else if (rdo9.Checked == true)
                    {
                        chkJindanNewT3.Checked = true;
                    }
                }
                if (dt.Rows[0]["JINDAN_NEW_02"].ToString().Trim() == "1")
                {
                    if (rdo7.Checked == true)
                    {
                        chkBioExam.Checked = true;
                        chkJinDanNew4.Checked = true;
                    }
                    else if (rdo8.Checked == true)
                    {
                        chkJindanNew2.Checked = true;
                        chkJinDanNew4.Checked = true;
                    }
                }
                if (dt.Rows[0]["JINDAN_NEW_03"].ToString().Trim() == "1")
                {
                    chkImmExam.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_04"].ToString().Trim() == "1")
                {
                    chkCytoExam.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_05"].ToString().Trim() == "1")
                {
                    chkBloodExam.Checked = true;
                }


                if (dt.Rows[0]["JINDAN_NEW_061"].ToString().Trim() == "1")
                {
                    chkCyto.Checked = true;
                }

                if (dt.Rows[0]["JINDAN_NEW_0611"].ToString().Trim() == "1")
                {
                    chkNoneCytoSayu1.Checked = true;
                    checkBox17.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_0612"].ToString().Trim() == "1")
                {
                    chkNoneCytoSayu2.Checked = true;
                    checkBox17.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_0613"].ToString().Trim() == "1")
                {
                    chkNoneCytoSayu3.Checked =true;
                    checkBox17.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_0614"].ToString().Trim() == "1")
                {
                    chkNoneCytoSayu4.Checked = true;
                    checkBox17.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_0615"].ToString().Trim() != "")
                {
                    chkNoneCytoSayu5.Checked = true;
                    checkBox17.Checked = true;
                    txtNoneCytoSayu5.Text = dt.Rows[0]["JINDAN_NEW_0615"].ToString().Trim();
                }

                if(dt.Rows[0]["JINDAN_NEW_062"].ToString().Trim() == "Y" && dt.Rows[0]["JINDAN_NEW_0621"].ToString().Trim() != "")
                {
                    txtPatJinRemark.Text = dt.Rows[0]["JINDAN_NEW_0621"].ToString().Trim();
                }

                if(dt.Rows[0]["JINDAN_NEW_07"].ToString().Trim() == "1")
                {
                    chkJindanNewT21.Checked = true;
                    chkJindanNewT2.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_08"].ToString().Trim() == "1")
                {
                    chkJindanNewT22.Checked = true;
                    chkJindanNewT2.Checked = true;
                }                  
                if (dt.Rows[0]["JINDAN_NEW_09"].ToString().Trim() == "Y")
                {
                    optTuberY.Checked = true;                        
                }
                else
                {
                    optTuberN.Checked = true;
                }

                if (dt.Rows[0]["JINDAN_NEW_091"].ToString().Trim() == "1")
                {
                    chkTuber1.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_092"].ToString().Trim() == "1")
                {
                    chkTuber3.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_093"].ToString().Trim() == "1")
                {
                    chkTuber3.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_094"].ToString().Trim() == "1")
                {
                    chkTuber5.Checked = true;
                }

                if (dt.Rows[0]["JINDAN_NEW_10"].ToString().Trim() == "1")
                {
                    chkJindanNew3.Checked = true;
                }

                if (dt.Rows[0]["JINDAN_NEW_11"].ToString().Trim() != "")
                {
                    if(rdo8.Checked == true)
                    {
                        txtJindanNew5.Text = dt.Rows[0]["JINDAN_NEW_11"].ToString().Trim();
                        chkJindanNew5.Checked = true;
                    }
                    else if(rdo9.Checked == true)
                    {
                        txtJindanNewT4.Text = dt.Rows[0]["JINDAN_NEW_11"].ToString().Trim();
                        chkJindanNewT4.Checked = true;
                    }
                }
                //잠복결핵 추가
                if (dt.Rows[0]["JINDAN_NEW_21"].ToString().Trim() == "1")
                {
                    chkJindanNewS1.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_22"].ToString().Trim() == "1")
                {
                    chkJindanNewS2.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_23"].ToString().Trim() == "1")
                {
                    chkJindanNewS3.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_24"].ToString().Trim() == "1")
                {
                    chkJindanNewS4.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_25"].ToString().Trim() == "1")
                {
                    chkJindanNewS5.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_26"].ToString().Trim() == "1")
                {
                    chkJindanNewS6.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_26A"].ToString().Trim() == "1")
                {
                    chkJindanNewS6A.Checked = true;
                }
                if (dt.Rows[0]["JINDAN_NEW_26A"].ToString().Trim() == "1")
                {
                    chkJindanNewS6B.Checked = true;
                }

                if (dt.Rows[0]["SENDDATE"].ToString().Trim() != "")
                {
                    dtpIO.Value = Convert.ToDateTime(dt.Rows[0]["SENDDATE"].ToString().Trim());
                    optIOY.Checked = true;
                }

                //결핵
                if (dt.Rows[0]["GUBUN2"].ToString().Trim() == "1")
                {
                    if(rdo8.Checked == true)
                    {

                    }
                    else if(rdo9.Checked == true)
                    {
                        cboGubun.SelectedIndex = 0;
                    }
                }
                //잠복결핵
                if (dt.Rows[0]["GUBUN2"].ToString().Trim() == "9")
                {
                    if (rdo8.Checked == true)
                    {

                    }
                    else if (rdo9.Checked == true)
                    {
                        cboGubun.SelectedIndex = 2;
                    }
                }
                //희귀
                else if (dt.Rows[0]["GUBUN2"].ToString().Trim() == "2")
                {
                    if (rdo8.Checked == true)
                    {
                        cboGubun.SelectedIndex = 1;
                    }
                    else if (rdo9.Checked == true)
                    {

                    }
                }
                //중증난치
                else if (dt.Rows[0]["GUBUN2"].ToString().Trim() == "3")
                {
                    if (rdo8.Checked == true)
                    {
                        cboGubun.SelectedIndex = 0;
                    }
                    else if (rdo9.Checked == true)
                    {

                    }
                }
                else if (dt.Rows[0]["GUBUN2"].ToString().Trim() == "4")
                {
                    if (rdo8.Checked == true)
                    {
                        cboGubun.SelectedIndex = 2;
                    }
                    else if (rdo9.Checked == true)
                    {

                    }
                }

                if (rdo7.Checked == true)
                {
                    switch (dt.Rows[0]["DTLGUBN"].ToString().Trim())
                    {
                        case "0": optSinGu0.Checked = true; break;
                        case "1": optSinGu1.Checked = true; break;
                        case "2": optSinGu2.Checked = true; break;
                    }
                }
                else if(rdo8.Checked == true)
                {
                    switch (dt.Rows[0]["DTLGUBN"].ToString().Trim())
                    {
                        case "0": optSinGu0.Checked = true; break;
                        case "1": optSinGu1.Checked = true; break;                         
                    }
                }

                txtExamETC.Text = dt.Rows[0]["REMARK1"].ToString().Trim();

                txtIllCodeNew.Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                if (dt.Rows[0]["ILLNAMEK"].ToString().Trim() == "")
                {
                    txtIllNameNew.Text = Read_Bas_ILL(txtIllCodeNew.Text);
                }
                else
                {
                    txtIllNameNew.Text = dt.Rows[0]["ILLNAMEK"].ToString().Trim();
                }
                    
                txtGiho.Text = dt.Rows[0]["CODE2"].ToString().Trim();

                dtpDongDate.Value = Convert.ToDateTime(VB.IIf(dt.Rows[0]["ASSENTIENTDATE"].ToString().Trim() == "", ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), dt.Rows[0]["ASSENTIENTDATE"].ToString().Trim()));
                txtDongSName.Text = dt.Rows[0]["ASSENTIENT"].ToString().Trim();

                if (dt.Rows[0]["CANDATE"].ToString().Trim() != "")
                {
                    dtpJinDate.Value = Convert.ToDateTime(dt.Rows[0]["CANDATE"].ToString().Trim());
                }

                txtSinName.Text = dt.Rows[0]["NAME"].ToString().Trim();
                txtSTel.Text = dt.Rows[0]["TEL"].ToString().Trim();

                ComFunc.ComboFind(cboGan, "L", 2, dt.Rows[0]["GANGE"].ToString().Trim());

                GstrROWID2 = dt.Rows[0]["ROWID"].ToString().Trim();

                if (dt.Rows[0]["SDATE"].ToString().Trim() != "")
                {
                    dtpSDate.Value = Convert.ToDateTime(dt.Rows[0]["SDATE"].ToString().Trim());
                }

                txtEmail.Text = dt.Rows[0]["EMAIL"].ToString().Trim();
                txtHPhone.Text = dt.Rows[0]["HPHONE"].ToString().Trim();

                txtPJumin.Text = dt.Rows[0]["PJUMIN"].ToString().Trim();

                switch (dt.Rows[0]["TONGBOGBN"].ToString().Trim())
                {
                    case "1":
                        rdoTongbo0.Checked = true;
                        break;
                    case "2":
                        rdoTongbo1.Checked = true;
                        break;
                }

                toolDelete.Visible = true;
            
            }
            dt.Dispose();
            dt = null;


        }

        private void INSERT_ONHIC(string strPtNo, string strSName, string strJumin)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            double dblWrtNo = READ_Next_NhicNo();
            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                //주민암호화
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_PMPA + "OPD_NHIC";
                SQL = SQL + ComNum.VBLF + "     (WRTNO, ACTDATE, PANO, ";
                SQL = SQL + ComNum.VBLF + "     DEPTCODE, SNAME, REQTIME, REQTYPE, ";
                SQL = SQL + ComNum.VBLF + "     JUMIN,JUMIN_new, JOB_STS, REQ_SABUN,BDATE)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     ( ";
                SQL = SQL + ComNum.VBLF + "     " + dblWrtNo + ", TO_DATE('" + strDate + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "     '" + strPtNo + "', 'MD', '" + strSName + "', ";
                SQL = SQL + ComNum.VBLF + "     SYSDATE, 'M1', '" + VB.Left(strJumin, 7) + "******" + "', '" + clsAES.AES(strJumin) + "', '0', " + clsType.User.Sabun + ",";
                SQL = SQL + ComNum.VBLF + "     TO_DATE('" + strDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "     ) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if(SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private double READ_Next_NhicNo()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            double rtnVal = 0;

            try
            {
                SQL = "";
                SQL = "SELECT KOSMOS_PMPA.SEQ_OPD_NHIC.NEXTVAL WRTNO FROM DUAL";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if(dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                rtnVal = VB.Val(dt.Rows[0]["WRTNO"].ToString().Trim());

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void rdo_CheckedChanged(object sender, EventArgs e)
        {
            //암
            if (rdo7.Checked == true)
            {
                screen_clear();
                rdo7.BackColor = Color.FromArgb(192, 255, 192);
                rdo8.BackColor = Color.White;
                rdo9.BackColor = Color.White;

                //panel27.Visible = false;             
                panel22.Visible = true;
                cboGubun.Visible = false;
                chkXray.Visible = false;

                chkRDExam.Text = "3. 영상검사";
                chkJojic.Visible = true;
                chkCytoExam.Visible = true;
                chkCT.Text = "CT(소견)";
                txtCT.Visible = true;

                PanCancer.Visible = true;
                Panrare.Visible = false;
                Pantuber.Visible = false;
                PantuberS.Visible = false;
                PanFire.Visible = false;
                PanAll.Visible = true;

                PanFire.Dock = DockStyle.None;

                PanCancer.Dock = DockStyle.Fill;
                Panrare.Dock = DockStyle.None;
                Pantuber.Dock = DockStyle.None;
                PantuberS.Dock = DockStyle.None;

                optSinGu0.Text = "신규암";
                optSinGu1.Text = "재등록암";
                optSinGu2.Text = "중복암";

                optSinGu2.Visible = true;

                cboGubun.Visible = false;

                panel19.Enabled = true;

                txtGiho.Text = "V193";
            }
            //희귀난치치매
            else if (rdo8.Checked == true)
            {
                screen_clear();
                rdo7.BackColor = Color.White;
                rdo8.BackColor = Color.FromArgb(255, 192, 255);
                rdo9.BackColor = Color.White;

                //panel27.Visible = true;
                panel22.Visible = false;
                cboGubun.Visible = true;
                chkXray.Visible = true;

                cboGubun.Items.Clear();
                cboGubun.Items.Add("중증난치");
                cboGubun.Items.Add("희귀");
                cboGubun.Items.Add("중증치매");
                cboGubun.SelectedIndex = 0;

                chkRDExam.Text = "1. 영상검사";
                chkJojic.Visible = false;
                chkCytoExam.Visible = false;
                chkCT.Text = "CT";
                txtCT.Visible = false;

                PanCancer.Visible = false;
                Panrare.Visible = true;
                Pantuber.Visible = false;
                PantuberS.Visible = false;
                PanFire.Visible = false;
                PanAll.Visible = true;

                PanFire.Dock = DockStyle.None;

                PanCancer.Dock = DockStyle.None;
                Panrare.Dock = DockStyle.Fill;
                Pantuber.Dock = DockStyle.None;
                PantuberS.Dock = DockStyle.None;

                optSinGu0.Text = "신규";
                optSinGu1.Text = "재등록";

                optSinGu0.Visible = true;
                optSinGu1.Visible = true;
                optSinGu2.Visible = false;
                cboGubun.Visible = true;
                panel19.Enabled = true;
                
            }
            //결핵,중증화상
            else if (rdo9.Checked == true)
            {
                screen_clear();
                rdo7.BackColor = Color.White;
                rdo8.BackColor = Color.White;
                rdo9.BackColor = Color.FromArgb(255, 192, 255);

                //panel27.Visible = true;
                panel22.Visible = false;
                cboGubun.Visible = true;
                chkXray.Visible = true;

                cboGubun.Items.Clear();
                cboGubun.Items.Add("결핵");
                cboGubun.Items.Add("중증화상");
                cboGubun.Items.Add("잠복결핵");
                cboGubun.SelectedIndex = 0;

                if (cboGubun.Text == "결핵")
                {

                    chkRDExam.Text = "1.영상검사";
                    chkJojic.Visible = false;
                    chkCytoExam.Visible = false;
                    chkCT.Text = "CT";
                    txtCT.Visible = false;

                    PanCancer.Visible = false;
                    Panrare.Visible = false;
                    Pantuber.Visible = true;
                    PantuberS.Visible = false;
                    PanFire.Visible = false;
                    PanAll.Visible = true;
                    PanFire.Dock = DockStyle.None;

                    PanCancer.Dock = DockStyle.None;
                    Panrare.Dock = DockStyle.None;
                    Pantuber.Dock = DockStyle.Fill;
                    PantuberS.Dock = DockStyle.None;

                    panel19.Enabled = false;
                    cboGubun.Visible = true;
                }
                else if (cboGubun.Text == "잠복결핵")
                {

                    chkRDExam.Text = "1.영상검사";
                    chkJojic.Visible = false;
                    chkCytoExam.Visible = false;
                    chkCT.Text = "CT";
                    txtCT.Visible = false;

                    PanCancer.Visible = false;
                    Panrare.Visible = false;
                    Pantuber.Visible = false;
                    PantuberS.Visible = true;
                    PanFire.Visible = false;
                    PanAll.Visible = false;
                    PanFire.Dock = DockStyle.None;

                    PanCancer.Dock = DockStyle.None;
                    Panrare.Dock = DockStyle.None;
                    Pantuber.Dock = DockStyle.None;
                    PantuberS.Dock = DockStyle.Fill;

                    panel19.Enabled = false;
                    cboGubun.Visible = true;
                }
                else
                {

                    PanCancer.Visible = false;
                    Panrare.Visible = false;
                    Pantuber.Visible = false;
                    PantuberS.Visible = false;
                    PanFire.Visible = true;
                    PanAll.Visible = false;

                    PanCancer.Dock = DockStyle.None;
                    Panrare.Dock = DockStyle.None;
                    Pantuber.Dock = DockStyle.None;
                    PantuberS.Dock = DockStyle.None;
                    PanFire.Dock = DockStyle.Fill;

                    panel19.Enabled = false;
                    cboGubun.Visible = true;
                }
            }

            
            

           
            read_patinfo();
        }

        private void rdoETC_CheckedChanged(object sender, EventArgs e)
        {

            if (optION.Checked == true)
            {
                dtpIO.Enabled = false;
            }

            if (optIOY.Checked == true)
            {
                dtpIO.Enabled = true;
            }

            if (optFamN.Checked == true)
            {
                panel21.Enabled = false;
            }

            if (optFamY.Checked == true)
            {
                panel21.Enabled = true;
            }

            if (optTuberN.Checked == true)
            {
                panel25.Enabled = false;
            }

            if (optTuberY.Checked == true)
            {
                panel25.Enabled = true;
            }

        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDrCombo();
        }

        private void SetDrCombo()
        {
            if (VB.Split(cboDept.Text.Trim(), "전체").Length > 1) { return; }
            if (VB.Split(cboDeptNew.Text.Trim(), "전체").Length > 1) { return; }

            if (cboDept.Text.Trim() == ""|| cboDeptNew.Text.Trim() == "")
            {
                ComFunc.MsgBox("진료과를 선택해주세요");
                cboDept.Focus();
                return;
            }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            try
            {
                cboDr.Items.Clear();
                cboDoct.Items.Clear();

                SQL = "";
                SQL = "SELECT DRCODE, DRNAME FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                SQL = SQL + ComNum.VBLF + " WHERE DRDEPT1 = '" + VB.Left(cboDeptNew.Text, 2) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TOUR ='N'";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    cboDr.Items.Add("");
                    cboDoct.Items.Add("");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDr.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim());
                        cboDoct.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim());
                    }

                    cboDr.SelectedIndex = 0;
                    cboDoct.SelectedIndex = 0;
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            GintTimer++;
            timer1.Enabled = false;
            if (GintTimer >= 5)
            {
                GintTimer = 0;

                READ_ONHIC(txtPtNo.Text);
            }
        }

        private void READ_ONHIC(string strPtNo)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            
            string strFlag = "";

            if (strPtNo == "")
            {
                return;
            }
            
            if (GstrOK == "OK")
            {
                return;
            }

            try
            {
                //중증등록여부표시-공단
                SQL = "";
                SQL = "SELECT M2_Disreg2 ,M2_disreg4 ,M2_disreg5";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "OPD_NHIC ";
                SQL = SQL + ComNum.VBLF + "WHERE  PANO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SendTime >=TRUNC(SYSDATE-1) ";
                SQL = SQL + ComNum.VBLF + "   AND Job_STS ='2' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SENDTIME DESC  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        if (VB.Left(dt.Rows[i]["M2_disreg4"].ToString().Trim(), 1) == "V")
                        {
                            lblCa11.Visible = true;     //중증암
                        }

                        if(VB.Left(dt.Rows[i]["M2_disreg2"].ToString().Trim(), 1) == "V")
                        {
                            lblCa21.Visible = true;     //산정특례
                            lblCa21.Text = "산정특례 " + VB.Mid(dt.Rows[i]["M2_disreg2"].ToString().Trim(), 36, 5);

                            if (VB.Mid(dt.Rows[i]["M2_disreg2"].ToString().Trim(), 36, 5) != "")
                            {
                                strFlag = VB.Mid(dt.Rows[i]["M2_disreg2"].ToString().Trim(), 36, 5);
                            }
                        }

                        if(VB.Left(dt.Rows[i]["M2_disreg5"].ToString().Trim(), 1) == "V")
                        {
                            lblCa31.Visible = true;     //중증화상
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                if(strFlag != "" && VB.Val(ComQuery.CurrentDateTime(clsDB.DbCon, "D")) <= 20150120)
                {
                    SQL = "";
                    SQL = "SELECT ROWID FROM KOSMOS_PMPA.BAS_CANCER";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO ='" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND GUBUN ='2'";
                    SQL = SQL + ComNum.VBLF + "     AND FDATE >= TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "','YYYY-MM-DD')";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if(SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if(dt.Rows.Count == 0)
                    {
                        ComFunc.MsgBox("산정특례 상병코드:" + strFlag + "이며 재등록 대상자 입니다..");
                    }

                    dt.Dispose();
                    dt = null;
                }

                GstrOK = "OK";
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void txtILLCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtILLCode.Text = txtILLCode.Text.ToUpper().Trim();
                txtIllName.Text = Read_Bas_ILL(txtILLCode.Text);
            }
        }

        Image SIGNATUREFILE_DBToFile(string strSabun)
        {
            Image rtnVAL = null;

            if (string.IsNullOrEmpty(strSabun)) return rtnVAL;

            string SQL = "";
            IDataReader reader = null;
            OracleCommand cmd = null;

            try
            {
                SQL = "";
                SQL = SQL + "\r\n" + "SELECT SABUN, SIGNATURE ";
                SQL = SQL + "\r\n" + "FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                SQL = SQL + "\r\n" + "WHERE TRIM(DRCODE) = '" + strSabun + "'";

                cmd = clsDB.DbCon.Con.CreateCommand();
                cmd.InitialLONGFetchSize = -1;
                cmd.CommandText = SQL;
                cmd.CommandTimeout = 30;
                reader = cmd.ExecuteReader();

                cmd.Dispose();
                cmd = null;

                if (reader == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                }

                while (reader.Read())
                {
                    byte[] byteArray = (byte[])reader.GetValue(1);
                    MemoryStream memStream = new MemoryStream(byteArray);
                    rtnVAL = Image.FromStream(memStream);
                }
                return rtnVAL;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVAL;
            }
        }
            
        private void toolBohum_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strIndex = VB.Right(((ToolStripMenuItem)sender).Name, 1);

            if(strIndex == "1")
            {
                if(rdo8.Checked == true || rdo9.Checked == true)
                {
                    ComFunc.MsgBox("암을 선택하고 인쇄하세요.");
                    return;
                }
            }
            else if(strIndex == "2")
            {
                if (rdo7.Checked == true || rdo9.Checked == true)
                {
                    ComFunc.MsgBox("희귀난치치매를 선택하고 인쇄하세요.");
                    return;
                }
                if(rdo8.Checked == true && cboGubun.Text == "")
                {
                    ComFunc.MsgBox("버튼 옆 콤보박스에 값을 선택하세요!");
                    return;
                }                    
            }
            else if(strIndex == "3")
            {
                if (rdo7.Checked == true || rdo8.Checked == true)
                {
                    ComFunc.MsgBox("결핵화상을 선택하고 인쇄하세요.");
                    return;
                }
                if (rdo9.Checked == true && cboGubun.Text != "결핵")
                {
                    ComFunc.MsgBox("버튼 옆 콤보박스에 값을 선택하세요!");
                    return;
                }
            }
            else if(strIndex == "4")
            {
                if (rdo7.Checked == true || rdo8.Checked == true)
                {
                    ComFunc.MsgBox("결핵화상을 선택하고 인쇄하세요.");
                    return;
                }
                if (rdo9.Checked == true && cboGubun.Text != "중증화상")
                {
                    ComFunc.MsgBox("버튼 옆 콤보박스에 값을 선택하세요!");
                    return;
                }
            }

            else if (strIndex == "5")
            {
                if (rdo7.Checked == true || rdo8.Checked == true)
                {
                    ComFunc.MsgBox("결핵화상을 선택하고 인쇄하세요.");
                    return;
                }
                if (rdo9.Checked == true && cboGubun.Text != "잠복결핵")
                {
                    ComFunc.MsgBox("버튼 옆 콤보박스에 값을 선택하세요!");
                    return;
                }
            }

            if (GstrBi == "21" || GstrBi == "22")
            {
                ComFunc.MsgBox("자격이 의료급여입니다.. 신청서를 급여신청으로 출력하십시오.");
                return;
            }

            DATA_SET3_new2_NEW(strIndex);

            ssBohumNew1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssBohumNew1_Sheet1.PrintInfo.Margin.Top = 60;
            ssBohumNew1_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssBohumNew1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssBohumNew1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssBohumNew1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssBohumNew1_Sheet1.PrintInfo.ShowBorder = true;
            ssBohumNew1_Sheet1.PrintInfo.ShowColor = false;
            ssBohumNew1_Sheet1.PrintInfo.ShowGrid = true;
            ssBohumNew1_Sheet1.PrintInfo.ShowShadows = false;
            ssBohumNew1_Sheet1.PrintInfo.UseMax = false;
            ssBohumNew1_Sheet1.PrintInfo.UseSmartPrint = false;
            ssBohumNew1_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssBohumNew1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssBohumNew1_Sheet1.PrintInfo.Preview = false;
            ssBohumNew1.PrintSheet(0);

            ssBohumNew1.ActiveSheet.Rows[21].Height = 18;   //임상적소견 란 행 높이 원상복귀
            ComFunc.Delay(200);

            if (chkEtc.Checked == true)
            {
                frmBuppatEtc frmBuppatEtcX = new frmBuppatEtc();
                frmBuppatEtcX.PrintBohum(strIndex);

                if (frmBuppatEtcX != null)
                {
                    frmBuppatEtcX = null;
                }
            }
        }

        private void DATA_SET3_new2(string strIndex)
        {
            //암통합 질환 인쇄
            string strOK = "";

            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            //2015-02-06
            if(strIndex != "1")
            {
                if(cboDr.Text != "")
                {
                    if(ComFunc.MsgBoxQ("기존 [" + cboDr.Text.Trim() + "]의사정보가 있습니다.. 그대로 사용하시겠습니까??", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                    {
                        cboDept.Text = "";
                        cboDr.Text = "";
                    }
                }
            }

            if(strIndex == "1")
            {
                ssBohum1new_Sheet1.Cells[1, 1].Text = "(■  암 □ 희귀난치 □ 중증화상 □ 결핵 ) [□신규 □재등록]";
            }
            else if (strIndex == "2" || strIndex == "3")
            {
                ssBohum1new_Sheet1.Cells[1, 1].Text = "(□  암 ■ 희귀난치 □ 중증화상 □ 결핵 ) [□신규 □재등록]";
            }
            else if (strIndex == "4")
            {
                ssBohum1new_Sheet1.Cells[1, 1].Text = "(□  암 □ 희귀난치 □ 중증화상 ■ 결핵 ) [□신규 □재등록]";
            }

            ssBohum1new_Sheet1.Cells[3, 2].Text = txtGKiho.Text + GstrE000;
            ssBohum1new_Sheet1.Cells[3, 8].Text = txtPName.Text;

            if (txtSName.Text != "")
            {
                ssBohum1new_Sheet1.Cells[4, 2].Text = txtSName.Text + ComNum.VBLF + "(" + VB.Left(txtJumin.Text, 6) + "-" + VB.Right(txtJumin.Text, 7) + ")";
            }
            else
            {
                ssBohum1new_Sheet1.Cells[4, 2].Text = ComNum.VBLF + "(" + VB.Space(6) + "-" + VB.Space(7) + ")";
            }

            ssBohum1new_Sheet1.Cells[4, 8].Text = "□E-mail □문자서비스(SMS)";

            if(rdoTongbo0.Checked == true) { ssBohum1new_Sheet1.Cells[4, 8].Text = "□E-mail ■문자서비스(SMS)"; }
            if(rdoTongbo1.Checked == true) { ssBohum1new_Sheet1.Cells[4, 8].Text = "■E-mail □문자서비스(SMS)"; }

            ssBohum1new_Sheet1.Cells[5, 2].Text = txtEmail.Text;
            ssBohum1new_Sheet1.Cells[5, 9].Text = txtHPhone.Text;

            ssBohum1new_Sheet1.Cells[6, 2].Text = txtJuso.Text.Trim();
            ssBohum1new_Sheet1.Rows[6].Height = ssBohum1new_Sheet1.Rows[6].GetPreferredHeight() + 5;
            ssBohum1new_Sheet1.Cells[6, 9].Text = txtTel.Text;

            ssBohum1new_Sheet1.Cells[8, 2].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2));
            ssBohum1new_Sheet1.Cells[8, 7].Text = VB.Right(cboIO.Text, 2);
            ssBohum1new_Sheet1.Cells[8, 10].Text = dtpCanDate.Value.ToString("yyyy.MM.dd");

            ssBohum1new_Sheet1.Cells[9, 2].Text = txtIllName.Text;

            if(txtILLCode.Text.Trim() != "")
            {
                ssBohum1new_Sheet1.Cells[10, 2].Text = "( 상병기호 : " + txtILLCode.Text + " )";
            }
            else
            {
                ssBohum1new_Sheet1.Cells[10, 2].Text = "( 상병기호 : " + VB.Space(10) + ")";
            }

            ssBohum1new_Sheet1.Cells[10, 8].Text = "( 특정기호 : " + VB.Space(10) + ")";

            if(strIndex == "1")
            {
                ssBohum1new_Sheet1.Cells[12, 1].Text = "■ 암" + (rdoGU0.Checked == true ? "(■  신규 □ 재등록 □ 중복암)" : (rdoGU1.Checked == true ? "(□  신규 ■ 재등록 □ 중복암)" : "(□  신규 □ 재등록 ■ 중복암)"));
                ssBohum1new_Sheet1.Cells[12, 7].Text = "□ 희귀난치 □ 중증화상 □ 결핵";
            }
            else if (strIndex == "2" || strIndex == "3")
            {
                ssBohum1new_Sheet1.Cells[12, 1].Text = "□ 암";
                ssBohum1new_Sheet1.Cells[12, 7].Text = "■ 희귀난치 □ 중증화상 □ 결핵";
            }
            else if (strIndex == "4")
            {
                ssBohum1new_Sheet1.Cells[12, 1].Text = "□ 암";
                ssBohum1new_Sheet1.Cells[12, 7].Text = "□ 희귀난치 □ 중증화상 ■ 결핵";
            }

            //암
            strOK = "";

            if(strIndex == "1")
            {
                ssBohum1new_Sheet1.Cells[13, 1].Text = " □ ① 영상검사 □Sono □CT □MRI □기타(" + VB.Space(5) + ")";
                ssBohum1new_Sheet1.Cells[14, 1].Text = " □ ② 조직검사 없는 진단적 수술";
                ssBohum1new_Sheet1.Cells[15, 1].Text = " □ ③ 특수 생화학적 또는 면역학적검사";
                ssBohum1new_Sheet1.Cells[16, 1].Text = " □ ④ 세포학적 또는 혈액학적 검사";
                ssBohum1new_Sheet1.Cells[17, 1].Text = " □ ⑤ 전이부위의 조직학적 검사";
                ssBohum1new_Sheet1.Cells[18, 1].Text = " □ ⑥ 원발부위의 조직학적 생검";
                ssBohum1new_Sheet1.Cells[19, 1].Text = " □ ⑦ 기타";

                //희귀난치성
                ssBohum1new_Sheet1.Cells[13, 7].Text = " □ ① 검사 □Sono □CT □MRI □기타(             )";
                ssBohum1new_Sheet1.Cells[14, 7].Text = " □ ② 특수 생화학/면역학적검사,도말/배양검사";
                ssBohum1new_Sheet1.Cells[15, 7].Text = " □ ③ 유전학적 검사";
                ssBohum1new_Sheet1.Cells[16, 7].Text = " □ ④ 조직학적 검사";
                ssBohum1new_Sheet1.Cells[17, 7].Text = " □ ⑤ 임상적 소견으로 최종 진단 시 기재";
            }
            else
            {
                ssBohum1new_Sheet1.Cells[13, 1].Text = " □ ① 영상검사 □Sono □CT □MRI □기타(" + VB.Space(5) + ")";
                ssBohum1new_Sheet1.Cells[14, 1].Text = " □ ② 특수 생화학/면역학적검사,도말/배양검사";
                ssBohum1new_Sheet1.Cells[15, 1].Text = " □ ③ 유전학적 검사";
                ssBohum1new_Sheet1.Cells[16, 1].Text = " □ ④ 조직학적 검사";
                ssBohum1new_Sheet1.Cells[17, 1].Text = " □ ⑤ 임상적 소견으로 최종 진단 시 기재";
                ssBohum1new_Sheet1.Cells[18, 1].Text = " □ ⑥ 기타";

                ssBohum1new_Sheet1.Cells[13, 7].Text = " □ ① 영상검사 □Sono □CT □MRI □기타(" + VB.Space(5) + ")";
                ssBohum1new_Sheet1.Cells[14, 7].Text = " □ ② 특수 생화학/면역학적검사,도말/배양검사";
                ssBohum1new_Sheet1.Cells[15, 7].Text = " □ ③ 유전학적 검사";
                ssBohum1new_Sheet1.Cells[16, 7].Text = " □ ④ 조직학적 검사";
                ssBohum1new_Sheet1.Cells[17, 7].Text = " □ ⑤ 임상적 소견으로 최종 진단 시 기재";
                ssBohum1new_Sheet1.Cells[18, 7].Text = " □ ⑥ 기타";
            }

            if(chkJinDan0.Checked == true) { strOK = strOK + " ■ ①영상검사"; } else { strOK = strOK + " □ ①영상검사"; }
            if(chkJinDan1.Checked == true) { strOK = strOK + " ■Sono"; } else { strOK = strOK + " □ Sono"; }
            if(chkJinDan2.Checked == true) { strOK = strOK + " ■CT"; } else { strOK = strOK + " □CT"; }
            if(chkJinDan3.Checked == true) { strOK = strOK + " ■MRI"; } else { strOK = strOK + " □MRI"; }
            if(chkJinDan4.Checked == true) { strOK = strOK + " ■기타(" + txtRemark.Text + ")"; } else { strOK = strOK + " □기타(" + VB.Space(5) + ")"; }

            if(strIndex == "1")
            {
                ssBohum1new_Sheet1.Cells[13, 1].Text = strOK;

                if (chkJinDan5.Checked == true) { ssBohum1new_Sheet1.Cells[14, 1].Text = " ■ ② 조직검사 없는 진단적 수술"; } else { ssBohum1new_Sheet1.Cells[14, 1].Text = " □ ② 조직검사 없는 진단적 수술"; }
                if(chkJinDan6.Checked == true) { ssBohum1new_Sheet1.Cells[15, 1].Text = " ■ ③ 특수 생화학 또는 면역학적검사"; } else { ssBohum1new_Sheet1.Cells[15, 1].Text = " □ ③ 특수 생화학 또는 면역학적검사"; }
                if(chkJinDan7.Checked == true) { ssBohum1new_Sheet1.Cells[16, 1].Text = " ■ ④ 세포학적 또는 혈액학적 검사"; } else { ssBohum1new_Sheet1.Cells[16, 1].Text = " □ ④ 세포학적 또는 혈액학적 검사"; }
                if(chkJinDan8.Checked == true) { ssBohum1new_Sheet1.Cells[17, 1].Text = " ■ ⑤ 전이부위의 조직학적 검사"; } else { ssBohum1new_Sheet1.Cells[17, 1].Text = " □ ⑤ 전이부위의 조직학적 검사"; }
                if(chkJinDan9.Checked == true) { ssBohum1new_Sheet1.Cells[18, 1].Text = " ■ ⑥ 원발부위의 조직학적 생검"; } else { ssBohum1new_Sheet1.Cells[18, 1].Text = " □ ⑥ 원발부위의 조직학적 생검"; }

                strOK = "";

                if(chkJinDan9.Checked == true) { strOK = strOK + " ■ ⑦ 기타"; } else { strOK = strOK + " □ ⑦ 기타"; }
                if(txtRemark2.Text != "") { strOK = strOK + "(" + txtRemark2.Text + ")"; } else { strOK = strOK + "(" + VB.Space(20) + ")"; }

                ssBohum1new_Sheet1.Cells[19, 1].Text = strOK;
            }
            else
            {
                ssBohum1new_Sheet1.Cells[13, 7].Text = strOK;

                if (chkJinDan5.Checked == true) { ssBohum1new_Sheet1.Cells[14, 7].Text = " ■ ② 특수 생화학/면역학적검사,도말/배양검사"; } else { ssBohum1new_Sheet1.Cells[14, 7].Text = " □ ② 특수 생화학/면역학적검사,도말/배양검사"; }
                if(chkJinDan6.Checked == true) { ssBohum1new_Sheet1.Cells[15, 7].Text = " ■ ③ 유전학적 검사"; } else { ssBohum1new_Sheet1.Cells[15, 7].Text = " □ ③ 유전학적 검사"; }
                if(chkJinDan7.Checked == true) { ssBohum1new_Sheet1.Cells[16, 7].Text = " ■ ④ 조직학적 검사"; } else { ssBohum1new_Sheet1.Cells[16, 7].Text = " □ ④ 조직학적 검사"; }
                if(chkJinDan8.Checked == true) { ssBohum1new_Sheet1.Cells[17, 7].Text = " ■ ⑤ 임상적 소견으로 최종 진단 시 기재"; } else { ssBohum1new_Sheet1.Cells[17, 7].Text = " □ ⑤ 임상적 소견으로 최종 진단 시 기재"; }

                if (txtRemark2.Text != "")
                {
                    ssBohum1new_Sheet1.Cells[18, 7].Text = " (" + txtRemark2.Text + ")";
                }

                if (chkJinDan10.Checked == true)
                {
                    if(txtRemark3.Text != "")
                    {
                        ssBohum1new_Sheet1.Cells[19, 7].Text = " ■ ⑥ 기타(" + txtRemark3.Text.Trim() + ")";
                    }
                    else
                    {
                        ssBohum1new_Sheet1.Cells[19, 7].Text = " ■ ⑥ 기타";
                    }
                }
                else
                {
                    ssBohum1new_Sheet1.Cells[19, 7].Text = "  □ ⑥ 기타(" + VB.Space(15) + "검사)";
                }
            }

            ssBohum1new_Sheet1.Cells[22, 6].Text = Convert.ToDateTime(strDate).ToString("yyyy 년 MM 월 dd 일");

            if (cboDr.Text.Trim() != "")
            {
                ssBohum1new_Sheet1.Cells[25, 6].Text = VB.Split(cboDr.Text, ".")[1];
            }

            if(cboDr.Text.Replace(".", "") != "")
            {
                ssBohum1new_Sheet1.Cells[25, 6].Text = ssBohum1new_Sheet1.Cells[25, 6].Text + "(" + clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0]) + ")";
            }
            else
            {
                ssBohum1new_Sheet1.Cells[25, 6].Text = ssBohum1new_Sheet1.Cells[25, 6].Text + "(" + VB.Space(10) + ")";
            }

            //동의일
            ssBohum1new_Sheet1.Cells[28, 4].Text = txtDongSName.Text;
            ssBohum1new_Sheet1.Cells[28, 6].Text = dtpDongDate.Value.ToString("yyyy 년 MM 월 dd 일");

            //신청일
            ssBohum1new_Sheet1.Cells[32, 6].Text = dtpSDate.Value.ToString("yyyy 년 MM 월 dd 일");

            ssBohum1new_Sheet1.Cells[34, 4].Text = txtSinName.Text;
            ssBohum1new_Sheet1.Cells[34, 9].Text = txtTel.Text;

            ssBohum1new_Sheet1.Cells[35, 10].Text = VB.Mid(cboGan.Text, 4, cboGan.Text.Length);

            ssBohum1new_Sheet1.Cells[39, 1].Text = "등록번호 : " + txtPtNo.Text;

            ssBohum1new2_Sheet1.Cells[46, 1].Text = "등록번호 : " + txtPtNo.Text;
        }

        private void DATA_SET3_new2_NEW(string strIndex)
        {
            string strOK = "";
            string strOK2 = "";

            //스프레드 클리어
            ssBohumNew1.ActiveSheet.Cells[15, 1, 33, 1].Text = "";

            //수진자 기본정보
            ssBohumNew1.ActiveSheet.Cells[4, 4].Text = txtGKiho.Text.Trim() + GstrE000;    //건강보험증번호
            ssBohumNew1.ActiveSheet.Cells[4, 9].Text = txtPName.Text.Trim();    //가입자 또는 세대주명
            ssBohumNew1.ActiveSheet.Cells[5, 4].Text = txtSName.Text.Trim();    //성명
            ssBohumNew1.ActiveSheet.Cells[5, 9].Text = txtJumin.Text.Trim();    //주민(외국인)등록번호
            ssBohumNew1.ActiveSheet.Cells[6, 4].Text = txtHPhone.Text.Trim();   //휴대전화번호
            ssBohumNew1.ActiveSheet.Cells[6, 9].Text = txtTel.Text.Trim();      //자택전화번호
            ssBohumNew1.ActiveSheet.Cells[7, 4].Text = txtEmail.Text.Trim();    //이메일주소
            ssBohumNew1.ActiveSheet.Cells[8, 4].Text = txtJuso.Text.Trim();     //자택주소
            ssBohumNew1.ActiveSheet.Cells[47, 1].Text = "등록번호 : " + txtPtNo.Text.Trim();    //등록번호


            //신청자입력란
            //동의일            
            ssBohumNew1.ActiveSheet.Cells[35, 6].Text = dtpDongDate.Value.ToString("yyyy 년 MM 월 dd 일");

            //신청일
            ssBohumNew1.ActiveSheet.Cells[42, 10].Text = dtpSDate.Value.ToString("yyyy 년 MM 월 dd 일");

            //신청자
            ssBohumNew1.ActiveSheet.Cells[43, 9].Text = txtSinName.Text;            

            //관계
            ssBohumNew1.ActiveSheet.Cells[44, 9].Text = VB.Mid(cboGan.Text, 4, cboGan.Text.Length);

            if (strIndex == "1")
            {
                #region 암              

                ssBohumNew1.ActiveSheet.Cells[0, 1].Text = "건강보험 (암) 산정특례 등록 신청서";

                ssBohumNew1.ActiveSheet.Rows[2].Visible = false; // 체크항목 표기란 

                ssBohumNew1.ActiveSheet.Cells[3, 1].Text = "산정특례번호";

                ssBohumNew1.ActiveSheet.Cells[4, 0].Text = "수진자";
                ssBohumNew1.ActiveSheet.Cells[4, 2].Text = "① 건강보험증번호";
                ssBohumNew1.ActiveSheet.Cells[4, 7].Text = "② 가입자(세대주)";

                if (rdoTongbo0.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "■ 알림톡     □ 이메일";
                }
                else if (rdoTongbo1.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "□ 알림톡     ■ 이메일";
                }


                ssBohumNew1.ActiveSheet.Cells[9, 1].Text = "[요양기간 확인란]";
                ssBohumNew1.ActiveSheet.Rows[10].Visible = true; // 신청구분란

                if (optSinGu0.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "■ 신규암      □ 재등록암      □ 중복암";
                }
                else if (optSinGu1.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "□ 신규암      ■ 재등록암      □ 중복암";
                }
                else if (optSinGu2.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "□ 신규암      □ 재등록암      ■ 중복암";
                }

                ssBohumNew1.ActiveSheet.Cells[11, 1].Text = "② 진료과목";
                ssBohumNew1.ActiveSheet.Cells[12, 1].Text = cboDeptNew.Text;

                ssBohumNew1.ActiveSheet.Cells[11, 5].Text = "③ 진료구분";

                if (optI.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "■ 입원      □ 외래";
                }
                else if (optO.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "□ 입원      ■ 외래";
                }

                ssBohumNew1.ActiveSheet.Cells[11, 8].Text = "④ 진단확진일";
                ssBohumNew1.ActiveSheet.Cells[12, 8].Text = dtpJinDate.Text;

                ssBohumNew1.ActiveSheet.Cells[13, 1].Text = "⑤ 상병명";
                ssBohumNew1.ActiveSheet.Cells[14, 1].Text = txtIllNameNew.Text.Trim();


                if (optWonbar.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[13, 2].Text = "(■ 원발   □ 전이)";
                }
                else if (optJeon.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[13, 2].Text = "(□ 원발   ■ 전이)";
                }

                ssBohumNew1.ActiveSheet.Cells[13, 5].Text = "⑥ 상병코드";
                ssBohumNew1.ActiveSheet.Cells[14, 5].Text = txtIllCodeNew.Text.Trim();

                ssBohumNew1.ActiveSheet.Cells[13, 8].Text = "⑦ 특정기호";
                ssBohumNew1.ActiveSheet.Cells[14, 8].Text = txtGiho.Text.Trim();

                ssBohumNew1.ActiveSheet.Cells[15, 1].Text = "⑧ 최종확진방법           ※중복체크가능";

                if (chkJojic.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[16, 1].Text = "   ■ 1. 조직학적 검사";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[16, 1].Text = "   □ 1. 조직학적 검사";
                }

                if (chkCytoExam.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "   ■ 2. 세포학적 검사";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "   □ 2. 세포학적 검사";
                }

                /// 영상검사 

                if (chkMRI.Checked == true || chkCT.Checked == true || chkSono.Checked == true || chkExamETC.Checked == true)
                {
                    strOK += "   ■ 3. 영상검사";
                }
                else
                {
                    strOK += "   □ 3. 영상검사";
                }
                if (chkMRI.Checked == true)
                {
                    strOK += "  ■ MRI";
                }
                else
                {
                    strOK += "  □ MRI";
                }
                if (chkCT.Checked == true)
                {
                    if (txtCT.Text.Trim() != "")
                    {
                        strOK += "               ■ CT (소견 : " + txtCT.Text.Trim() + " )";
                    }
                    else
                    {
                        strOK += "               ■ CT (소견 :                          )";
                    }
                }
                else
                {
                    strOK += "               □ CT (소견 :                          )";
                }
                if (chkSono.Checked == true)
                {
                    strOK2 += "                   ■ Sono";
                }
                else
                {
                    strOK2 += "                   □ Sono";
                }
                if (chkExamETC.Checked == true)
                {
                    if (txtExamETC.Text.Trim() != "")
                    {
                        strOK2 += "              ■ 기타( " + txtExamETC.Text.Trim() + " )";
                    }
                    else
                    {
                        strOK2 += "              ■ 기타 (                              )";
                    }
                }
                else
                {
                    strOK2 += "              □ 기타 (                              )";
                }

                ssBohumNew1.ActiveSheet.Cells[18, 1].Text = strOK;
                ssBohumNew1.ActiveSheet.Cells[19, 1].Text = strOK2;

                ///


                /// 4번항목 체크

                strOK = "";
                if (chkBioExam.Checked == true)
                {
                    strOK += "   ■ 4. 특수 생화학적 검사";
                }
                else
                {
                    strOK += "   □ 4. 특수 생화학적 검사";
                }
                if (chkImmExam.Checked == true)
                {
                    strOK += "    ■ 면역학적 검사";
                }
                else
                {
                    strOK += "    □ 면역학적 검사";
                }
                if (chkCytoExam.Checked == true)
                {
                    strOK += "   ■ 혈액학적 검사";
                }
                else
                {
                    strOK += "   □ 혈액학적 검사";
                }

                ///
                ssBohumNew1.ActiveSheet.Cells[20, 1].Text = strOK;


                if (chkNoneJojic.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[21, 1].Text = "   ■ 5. 조직검사 없는 진단적 수술";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[21, 1].Text = "   □ 5. 조직검사 없는 진단적 수술";
                }

                if (chkETCExam.Checked == true)
                {
                    if (txtETCExam.Text.Trim() != "")
                    {
                        ssBohumNew1.ActiveSheet.Cells[22, 1].Text = "   ■ 6. 기타( " + txtETCExam.Text.Trim() + " )";
                    }
                    else
                    {
                        ssBohumNew1.ActiveSheet.Cells[22, 1].Text = "   ■ 6. 기타(                                )";
                    }
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[22, 1].Text = "   □ 6. 기타(                                )";
                }



                //ssBohumNew1.ActiveSheet.Cells[23, 1].Text = "⑨ 조직학적 □ 세포학적 검사 필수인 상병에서 조직학적 □ 세포학적 검사 불가하여 등록기준 미충족한 경우에만 작성*";
                //ssBohumNew1.ActiveSheet.Cells[24, 1].Text = "     * 상병별 등록기준을 미충족한 경우에는 전문의가 환자상태 및 진료소견을 구체적으로 기재 후 신청서를 발행하여야 함";


                ///  미실시사유
                ///  

                if (chkCyto.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[23, 1].Text = "⑨ 조직학적 ■ 세포학적 검사 필수인 상병에서 조직학적 □ 세포학적 검사 불가하여 등록기준 미충족한 경우 작성*";
                }
                else if (chkNoneCyto.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[23, 1].Text = "⑨ 조직학적 □ 세포학적 검사 필수인 상병에서 조직학적 ■ 세포학적 검사 불가하여 등록기준 미충족한 경우 작성*";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[23, 1].Text = "⑨ 조직학적 □ 세포학적 검사 필수인 상병에서 조직학적 □ 세포학적 검사 불가하여 등록기준 미충족한 경우 작성*";
                }

                ssBohumNew1.ActiveSheet.Cells[24, 1].Text = "     * 상병별 등록기준을 미충족한 경우 전문의가 환자상태 및 진료소견을 구체적으로 기재 후 신청서를 발행";

                ssBohumNew1.ActiveSheet.Cells[25, 1].Text = " ⑨-1 조직학적 □ 세포학적 검사 미실시 사유         ※중복체크가능";

                if (chkNoneCytoSayu1.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[26, 1].Text = "  ■ 1. 전신상태가 ECOG performance status 3 이상인 경우";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[26, 1].Text = "  □ 1. 전신상태가 ECOG performance status 3 이상인 경우";
                }
                if (chkNoneCytoSayu2.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[27, 1].Text = "  ■ 2. 출혈 위험성이 큰 경우";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[27, 1].Text = "  □ 2. 출혈 위험성이 큰 경우";
                }
                if (chkNoneCytoSayu3.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[28, 1].Text = "  ■ 3. 검사를 위한 전신마취 및 수술을 견딜 수 없는 경우";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[28, 1].Text = "  □ 3. 검사를 위한 전신마취 및 수술을 견딜 수 없는 경우";
                }
                if (chkNoneCytoSayu4.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[29, 1].Text = "  ■ 4. 감염 위험성이 높은 경우";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[29, 1].Text = "  □ 4. 감염 위험성이 높은 경우";
                }
                if (chkNoneCytoSayu5.Checked == true || txtNoneCytoSayu5.Text.Trim() != "")
                {
                    if (txtNoneCytoSayu5.Text.Trim() != "")
                    {
                        ssBohumNew1.ActiveSheet.Cells[30, 1].Text = "  ■ 5. 기타( " + txtNoneCytoSayu5.Text.Trim() + " )";
                    }
                    else
                    {
                        ssBohumNew1.ActiveSheet.Cells[30, 1].Text = "  ■ 5. 기타(                                                                                      )";
                    }
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[30, 1].Text = "  □ 5. 기타(                                                                                      )";
                }

                ///

                ssBohumNew1.ActiveSheet.Cells[32, 1].Text = " ⑨-2 환자상태 및 진료소견(확진의견을 포함하여 구체적으로 기재)";

                if (txtPatJinRemark.Text.Trim() != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[33, 1].Text = txtPatJinRemark.Text.Trim();
                }

                //2019-01-08
                if (cboDoct.Text.Trim() != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = "담당의사 (면허번호/전문의 자격번호):  " + VB.Split(cboDoct.Text, ".")[1];
                }

                if (cboDoct.Text.Replace(".", "") != "" && cboDeptNew.Text != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = ssBohumNew1.ActiveSheet.Cells[37, 3].Text + "    (" +
                                                                clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDoct.Text, ".")[0]) + "/" +
                                                                GetInsaCertNo(clsDB.DbCon, VB.Split(cboDoct.Text, ".")[0], VB.Split(cboDeptNew.Text, ".")[0], "2") +
                                                                ")";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = ssBohumNew1.ActiveSheet.Cells[37, 3].Text + "    (" + VB.Space(10) + ")";
                }

                if (cboDeptNew.Text.Replace(".", "") != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[38, 4].Text = ssBohumNew1.ActiveSheet.Cells[38, 4].Text + "  " + VB.Split(cboDeptNew.Text, ".")[1];
                }

                ssBohumNew1.ActiveSheet.Cells[36, 4].Text = "요양기관명 (기 호):";

                ssBohumNew1.ActiveSheet.Cells[45, 1].Text = "국민건강보험공단 이사장 귀하";

                #endregion
            }
            else if (strIndex == "2")
            {
                #region 기타 산정특례

                if (cboGubun.Text != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[0, 1].Text = "건강보험 (" + cboGubun.Text.Trim() + ") 산정특례 등록 신청서";
                }


                ssBohumNew1.ActiveSheet.Rows[2].Visible = true; // 체크항목 표기란
                if (cboGubun.SelectedItem.ToString().Trim() == "희귀")
                {
                    ssBohumNew1.ActiveSheet.Cells[2, 0].Text = "■ 희귀질환          □ 중증난치질환        □ 중증치매";
                }
                else if (cboGubun.SelectedItem.ToString().Trim() == "중증난치질환")
                {
                    ssBohumNew1.ActiveSheet.Cells[2, 0].Text = "□ 희귀질환          ■ 중증난치질환        □ 중증치매";
                }
                else if (cboGubun.SelectedItem.ToString().Trim() == "중증치매")
                {
                    ssBohumNew1.ActiveSheet.Cells[2, 0].Text = "□ 희귀질환          □ 중증난치질환        ■ 중증치매";
                }

                ssBohumNew1.ActiveSheet.Cells[3, 0].Text = "산정특례번호";

                ssBohumNew1.ActiveSheet.Cells[4, 0].Text = "수진자";
                ssBohumNew1.ActiveSheet.Cells[4, 2].Text = "① 건강보험증번호";
                ssBohumNew1.ActiveSheet.Cells[4, 7].Text = "② 가입자(세대주)";

                if (rdoTongbo0.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "■ 알림톡     □ 이메일";
                }
                else if (rdoTongbo1.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "□ 알림톡     ■ 이메일";
                }

                ssBohumNew1.ActiveSheet.Cells[9, 1].Text = "[요양기간 확인란]";
                ssBohumNew1.ActiveSheet.Rows[10].Visible = true; // 신청구분란

                if (optSinGu0.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "■ 신규      □ 재등록";
                }
                else if (optSinGu1.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "□ 신규      ■ 재등록";
                }

                ssBohumNew1.ActiveSheet.Cells[11, 1].Text = "② 진료과목";
                ssBohumNew1.ActiveSheet.Cells[12, 1].Text = cboDeptNew.SelectedItem.ToString().Trim();

                ssBohumNew1.ActiveSheet.Cells[11, 5].Text = "③ 진료구분";

                ssBohumNew1.ActiveSheet.Cells[11, 8].Text = "④ 진단확진일";
                ssBohumNew1.ActiveSheet.Cells[12, 8].Text = dtpJinDate.Text;

                if (optI.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "■ 입원      □ 외래";
                }
                else if (optO.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "□ 입원      ■ 외래";
                }

                ssBohumNew1.ActiveSheet.Cells[13, 1].Text = "⑤ 상병명";
                ssBohumNew1.ActiveSheet.Cells[14, 1].Text = txtIllNameNew.Text.Trim();
                //ssBohumNew1.ActiveSheet.Cells[13, 2].Text = "(□ 원발   □ 전이)";
                ssBohumNew1.ActiveSheet.Cells[13, 5].Text = "⑥ 상병코드";
                ssBohumNew1.ActiveSheet.Cells[14, 5].Text = txtIllCodeNew.Text.Trim();

                ssBohumNew1.ActiveSheet.Cells[13, 8].Text = "⑦ 특정기호";
                ssBohumNew1.ActiveSheet.Cells[14, 8].Text = txtGiho.Text.Trim();
                ssBohumNew1.ActiveSheet.Cells[15, 1].Text = "⑧ 최종확진방법           ※중복체크가능";


                //// 영상체크

                if (chkSono.Checked == true || chkMRI.Checked == true || chkCT.Checked == true || chkExamETC.Checked == true || chkXray.Checked == true)
                {
                    strOK += "  ■ 1. 영상검사";
                }
                else
                {
                    strOK += "  □ 1. 영상검사";
                }
                if (chkXray.Checked == true)
                {
                    strOK += "  ■ X-ray";
                }
                else
                {
                    strOK += "  □ X-ray";
                }
                if (chkCT.Checked == true)
                {
                    strOK += "    ■ CT";
                }
                else
                {
                    strOK += "    □ CT";
                }

                if (chkSono.Checked == true)
                {
                    strOK += "    ■ Sono";
                }
                else
                {
                    strOK += "    □ Sono";
                }

                if (chkMRI.Checked == true)
                {
                    strOK += "    ■ MRI";
                }
                else
                {
                    strOK += "    □ MRI";
                }

                if (chkExamETC.Checked == true)
                {
                    if (txtExamETC.Text.Trim() != "")
                    {
                        strOK += "    ■ 기타( " + txtExamETC.Text.Trim() + " )";
                    }
                    else
                    {
                        strOK += "    ■ 기타(                              )";
                    }
                }
                else
                {
                    strOK += "    □ 기타(                              )";
                }

                ssBohumNew1.ActiveSheet.Cells[16, 1].Text = strOK;

                //// 

                if (chkJindanNew2.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "  ■ 2. 특수생화학/면역학, 도말/배양검사 등";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "  □ 2. 특수생화학/면역학, 도말/배양검사 등";
                }

                if (chkJindanNew3.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[18, 1].Text = "  ■ 3. 유전학적 검사";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[18, 1].Text = "  □ 3. 유전학적 검사";
                }

                if (chkJindanNew44.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[19, 1].Text = "  ■ 4. 조직학적 검사";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[19, 1].Text = "  □ 4. 조직학적 검사";
                }

                if (chkJindanNew5.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[20, 1].Text = "  ■ 5. 임상적 소견";

                    if (txtJindanNew5.Text.Trim() != "")
                    {
                        ssBohumNew1.ActiveSheet.Rows[21].Height = 36;   //임상적소견 란
                        FarPoint.Win.Spread.CellType.TextCellType TCT = new FarPoint.Win.Spread.CellType.TextCellType();
                        TCT.Multiline = true;
                        ssBohumNew1.ActiveSheet.Cells[21, 1].CellType = TCT;

                        ssBohumNew1.ActiveSheet.Cells[21, 1].Text = txtJindanNew5.Text.Trim();
                    }
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[20, 1].Text = "  □ 5. 임상적 소견";
                }

                if (chkJindanNew6.Checked == true && txtJindanNew6.Text.Trim() != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[22, 1].Text = "  ■ 6. 기타 ( " + txtJindanNew6.Text.Trim() + " 검사 )";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[22, 1].Text = "  □ 6. 기타 (                      검사 )";
                }

                //ssBohumNew1.ActiveSheet.Cells[23, 1].Text = "⑨ 조직학적 □ 세포학적 검사 필수인 상병에서 조직학적 □ 세포학적 검사 불가하여 등록기준 미충족한 경우에만 작성*";
                //ssBohumNew1.ActiveSheet.Cells[24, 1].Text = "     * 상병별 등록기준을 미충족한 경우에는 전문의가 환자상태 및 진료소견을 구체적으로 기재 후 신청서를 발행하여야 함";

                ssBohumNew1.ActiveSheet.Cells[23, 1].Text = "⑨ 질병정보 - 가족력                      ※ 희귀질환(극희귀·상세불명 희귀·기타염색제 이상질환 포함) 필수";

                /// 가족력

                strOK = "";
                if (optFamY.Checked == true)
                {
                    if (chkFam1.Checked == true)
                    {
                        strOK += "   □ 없음    ■ 있음( ■ 조부";
                    }
                    else
                    {
                        strOK += "   □ 없음    ■ 있음( □ 조부";
                    }
                    if (chkFam2.Checked == true)
                    {
                        strOK += " ■ 조모";
                    }
                    else
                    {
                        strOK += " □ 조모";
                    }
                    if (chkFam3.Checked == true)
                    {
                        strOK += " ■ 외조부";
                    }
                    else
                    {
                        strOK += " □ 외조부";
                    }
                    if (chkFam4.Checked == true)
                    {
                        strOK += " ■ 외조모";
                    }
                    else
                    {
                        strOK += " □ 외조모";
                    }
                    if (chkFam5.Checked == true)
                    {
                        strOK += " ■ 부";
                    }
                    else
                    {
                        strOK += " □ 부";
                    }
                    if (chkFam6.Checked == true)
                    {
                        strOK += " ■ 모";
                    }
                    else
                    {
                        strOK += " □ 모";
                    }
                    if (chkFam7.Checked == true)
                    {
                        strOK += " ■ 동성형제";
                    }
                    else
                    {
                        strOK += " □ 동성형제";
                    }
                    if (chkFam8.Checked == true)
                    {
                        strOK += " ■ 이성형제";
                    }
                    else
                    {
                        strOK += " □ 이성형제";
                    }
                    if (chkFam9.Checked == true)
                    {
                        strOK += " ■ 자";
                    }
                    else
                    {
                        strOK += " □ 자";
                    }
                    if (chkFam10.Checked == true)
                    {
                        strOK += " ■ 녀)";
                    }
                    else
                    {
                        strOK += " □ 녀)";
                    }
                    ssBohumNew1.ActiveSheet.Cells[24, 1].Text = strOK;
                }
                else if (optFamN.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[24, 1].Text = "   ■ 없음    □ 있음( □ 조부 □ 조모 □ 외조부 □ 외조모 □ 부 □ 모 □ 동성형제 □ 이성형제 □ 자 □ 녀)";
                }

                ///
                //2019-01-08
                if (cboDoct.Text.Trim() != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = "담당의사 (면허번호/전문의 자격번호):  " + VB.Split(cboDoct.Text, ".")[1];
                }

                if (cboDoct.Text.Replace(".", "") != "" && cboDeptNew.Text != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = ssBohumNew1.ActiveSheet.Cells[37, 3].Text + "    (" +
                                                                clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDoct.Text, ".")[0]) + "/" +
                                                                GetInsaCertNo(clsDB.DbCon, VB.Split(cboDoct.Text, ".")[0], VB.Split(cboDeptNew.Text, ".")[0]) +
                                                                ")";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = ssBohumNew1.ActiveSheet.Cells[37, 3].Text + "    (" + VB.Space(10) + ")";
                }

                if (cboDeptNew.Text.Replace(".", "") != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[38, 4].Text = ssBohumNew1.ActiveSheet.Cells[38, 4].Text + "  " + VB.Split(cboDeptNew.Text, ".")[1];
                }

                ssBohumNew1.ActiveSheet.Cells[36, 4].Text = "요양기관명 (기 호):";

                ssBohumNew1.ActiveSheet.Cells[45, 1].Text = "국민건강보험공단 이사장 귀하";

                #endregion
            }
            else if(strIndex == "3" || strIndex == "4" || strIndex == "5")
            {
                #region 결핵, 화상 (빈서식지)

                if (cboGubun.Text != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[0, 1].Text = "건강보험 (" + cboGubun.Text.Trim() + ") 산정특례 등록 신청서";
                }

                ssBohumNew1.ActiveSheet.Rows[2].Visible = false; // 체크항목 표기란 

                //if(cboGubun.Text == "결핵")
                //{
                //    ssBohumNew1.ActiveSheet.Cells[2, 0].Text = "■ 결핵          □ 중증화상";
                //}
                //else if(cboGubun.Text == "중증화상")
                //{
                //    ssBohumNew1.ActiveSheet.Cells[2, 0].Text = "□ 결핵          ■ 중증화상";
                //}
                
                ssBohumNew1.ActiveSheet.Cells[3, 0].Text = "산정특례번호";

                ssBohumNew1.ActiveSheet.Cells[4, 0].Text = "수진자";
                ssBohumNew1.ActiveSheet.Cells[4, 2].Text = "① 건강보험증번호";
                ssBohumNew1.ActiveSheet.Cells[4, 7].Text = "② 가입자(세대주)";

                if (rdoTongbo0.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "■ 알림톡     □ 이메일";
                }
                else if (rdoTongbo1.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "□ 알림톡     ■ 이메일";
                }

                ssBohumNew1.ActiveSheet.Cells[9, 1].Text = "[요양기간 확인란]";

                ssBohumNew1.ActiveSheet.Rows[10].Visible = false; // 신청구분란
                //ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "□ 신규      □ 재등록";

                ssBohumNew1.ActiveSheet.Cells[11, 1].Text = "① 진료과목";
                ssBohumNew1.ActiveSheet.Cells[12, 1].Text = cboDeptNew.SelectedItem.ToString().Trim();

                ssBohumNew1.ActiveSheet.Cells[11, 5].Text = "② 구분";
                ssBohumNew1.ActiveSheet.Cells[11, 8].Text = "③ 진단확진일";
                ssBohumNew1.ActiveSheet.Cells[12, 8].Text = dtpJinDate.Text;

                if (optI.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "■ 입원      □ 외래";
                }
                else if (optO.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "□ 입원      ■ 외래";
                }

                ssBohumNew1.ActiveSheet.Cells[13, 1].Text = "④ 상병명";
                ssBohumNew1.ActiveSheet.Cells[14, 1].Text = txtIllNameNew.Text.Trim();
                //ssBohumNew1.ActiveSheet.Cells[13, 2].Text = "(□ 원발   □ 전이)";
                ssBohumNew1.ActiveSheet.Cells[13, 5].Text = "⑤ 상병코드";
                ssBohumNew1.ActiveSheet.Cells[14, 5].Text = txtIllCodeNew.Text.Trim();

                ssBohumNew1.ActiveSheet.Cells[13, 8].Text = "⑥ 특정기호";
                ssBohumNew1.ActiveSheet.Cells[14, 8].Text = txtGiho.Text.Trim();

                ssBohumNew1.ActiveSheet.Cells[15, 1].Text = "⑦ 최종확진방법           ※중복체크가능";

                //// 영상체크
                ///

                if (cboGubun.Text == "결핵")
                {
                    if (chkSono.Checked == true || chkMRI.Checked == true || chkCT.Checked == true || chkExamETC.Checked == true || chkXray.Checked == true)
                    {
                        strOK += "  ■ 1. 영상검사";
                    }
                    else
                    {
                        strOK += "  □ 1. 영상검사";
                    }
                    if (chkXray.Checked == true)
                    {
                        strOK += "  ■ X-ray";
                    }
                    else
                    {
                        strOK += "  □ X-ray";
                    }
                    if (chkCT.Checked == true)
                    {
                        strOK += "    ■ CT";
                    }
                    else
                    {
                        strOK += "    □ CT";
                    }

                    if (chkSono.Checked == true)
                    {
                        strOK += "    ■ Sono";
                    }
                    else
                    {
                        strOK += "    □ Sono";
                    }

                    if (chkMRI.Checked == true)
                    {
                        strOK += "    ■ MRI";
                    }
                    else
                    {
                        strOK += "    □ MRI";
                    }

                    if (chkExamETC.Checked == true)
                    {
                        if (txtExamETC.Text.Trim() != "")
                        {
                            strOK += "    ■ 기타( " + txtExamETC.Text.Trim() + " )";
                        }
                        else
                        {
                            strOK += "    ■ 기타(                              )";
                        }
                    }
                    else
                    {
                        strOK += "    □ 기타(                              )";
                    }

                    ssBohumNew1.ActiveSheet.Cells[16, 1].Text = strOK;

                    //// 



                    if (chkJindanNewT21.Checked == true)
                    {
                        ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "   ■ 2. 도말/배양검사  ■ 도말    □ 배양";
                    }
                    else if (chkJindanNewT22.Checked == true)
                    {
                        ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "   ■ 2. 도말/배양검사  □ 도말    ■ 배양";
                    }
                    else
                    {
                        ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "   □ 2. 도말/배양검사  □ 도말    □ 배양";
                    }

                    if (chkJindanNewT3.Checked == true)
                    {
                        ssBohumNew1.ActiveSheet.Cells[18, 1].Text = "   ■ 3. 조직학적 검사";
                    }
                    else
                    {
                        ssBohumNew1.ActiveSheet.Cells[18, 1].Text = "   □ 3. 조직학적 검사";
                    }

                    if (chkJindanNewT4.Checked == true || txtJindanNewT4.Text.Trim() != "")
                    {
                        ssBohumNew1.ActiveSheet.Cells[19, 1].Text = "   ■ 4. 임상적 소견 ( " + txtJindanNewT4.Text.Trim() + " )";
                    }
                    else
                    {
                        ssBohumNew1.ActiveSheet.Cells[19, 1].Text = "   □ 4. 임상적 소견 (                                                                                  )";
                    }

                    if (chkJindanNewT5.Checked == true || txtJindanNewT5.Text.Trim() != "")
                    {
                        ssBohumNew1.ActiveSheet.Cells[20, 1].Text = "   ■ 5. 기타 ( " + txtJindanNewT5.Text.Trim() + " 검사)";
                    }
                    else
                    {
                        ssBohumNew1.ActiveSheet.Cells[20, 1].Text = "   □ 5. 기타 (                  검사)";
                    }


                    ////
                    strOK = "";

                    ssBohumNew1.ActiveSheet.Cells[21, 1].Text = "⑧ 타 요양기관의 검사결과로 확진한 경우, 해당사항 체크           ※중복체크가능";

                    if (optTuberN.Checked == true)
                    {
                        ssBohumNew1.ActiveSheet.Cells[22, 1].Text = "   ■ 없음    □ 있음 (□ 1.영상검사 □ 2.도말/배양검사 □ 3.조직학적 검사 □ 5.기타)";
                    }
                    else if (optTuberY.Checked == true)
                    {
                        if (chkTuber1.Checked == true)
                        {
                            strOK += "   □ 없음    ■ 있음 (■ 1.영상검사";
                        }
                        else
                        {
                            strOK += "   □ 없음    ■ 있음 (□ 1.영상검사";
                        }
                        if (chkTuber2.Checked == true)
                        {
                            strOK += " ■ 2.도말/배양검사";
                        }
                        else
                        {
                            strOK += " □ 2.도말/배양검사";
                        }
                        if (chkTuber3.Checked == true)
                        {
                            strOK += " ■ 3.조직학적 검사";
                        }
                        else
                        {
                            strOK += " □ 3.조직학적 검사";
                        }
                        if (chkTuber5.Checked == true)
                        {
                            strOK += " ■ 5.기타)";
                        }
                        else
                        {
                            strOK += " □ 5.기타)";
                        }

                        ssBohumNew1.ActiveSheet.Cells[22, 1].Text = strOK;
                    }
                }

                if (cboGubun.Text == "잠복결핵")
                {

                    if (chkJindanNewS1.Checked == true)
                    {
                        ssBohumNew1.ActiveSheet.Cells[16, 1].Text = "   ■ 1. TST";
                    }
                    else
                    {
                        ssBohumNew1.ActiveSheet.Cells[16, 1].Text = "   □ 1. TST";
                    }

                    if (chkJindanNewS2.Checked == true)
                    {
                        ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "   ■ 2. IGRA";
                    }
                    else
                    {
                        ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "   □ 2. IGRA";
                    }

                    if (chkJindanNewS3.Checked == true)
                    {
                        ssBohumNew1.ActiveSheet.Cells[18, 1].Text = "   ■ 3. 영상검사상 활동성결핵 아님";
                    }
                    else
                    {
                        ssBohumNew1.ActiveSheet.Cells[18, 1].Text = "   □ 3. 영상검사상 활동성결핵 아님";
                    }

                    if (chkJindanNewS4.Checked == true || txtJindanNewT4.Text.Trim() != "")
                    {
                        ssBohumNew1.ActiveSheet.Cells[19, 1].Text = "   ■ 4. 도말/배양검사상 활동성결핵 아님";
                    }
                    else
                    {
                        ssBohumNew1.ActiveSheet.Cells[19, 1].Text = "   □ 4. 도말/배양검사상 활동성결핵 아님";
                    }

                    if (chkJindanNewS5.Checked == true || txtJindanNewT4.Text.Trim() != "")
                    {
                        ssBohumNew1.ActiveSheet.Cells[20, 1].Text = "   ■ 5. 조직학적 검사상 활동성결핵 아님";
                    }
                    else
                    {
                        ssBohumNew1.ActiveSheet.Cells[20, 1].Text = "   □ 5. 조직학적 검사상 활동성결핵 아님";
                    }

                    if (chkJindanNewS6.Checked == true )
                    {
                        if  (chkJindanNewS6A.Checked == true )
                        {
                            ssBohumNew1.ActiveSheet.Cells[21, 1].Text = "   ■ 6. 기타 (■결핵발병 고워험 성인  □전염성 결핵환자의 접촉자) ";
                        }
                        else if (chkJindanNewS6B.Checked == true)
                        {
                            ssBohumNew1.ActiveSheet.Cells[21, 1].Text = "   ■ 6. 기타 (□결핵발병 고워험 성인  ■전염성 결핵환자의 접촉자) ";
                        }
                    }
                    else
                    {
                        ssBohumNew1.ActiveSheet.Cells[21, 1].Text = "   □ 6. 기타 (□ 1.결핵발병 고워험 성인  □ 2.전염성 결핵환자의 접촉자) ";                                                    
                    }

                    
                }

                else if (cboGubun.Text == "중증화상")
                {
                    #region 화상

                    if (cboGubun.Text != "")
                    {
                        ssBohumNew1.ActiveSheet.Cells[0, 1].Text = "건강보험 (" + cboGubun.Text.Trim() + ") 산정특례 등록 신청서";
                    }

                    //ssBohumNew1.ActiveSheet.Rows[2].Visible = true; // 체크항목 표기란 

                    ssBohumNew1.ActiveSheet.Rows[2].Visible = false;

                    //if (cboGubun.Text == "결핵")
                    //{
                    //    ssBohumNew1.ActiveSheet.Cells[2, 0].Text = "■ 결핵          □ 중증화상";
                    //}
                    //else if (cboGubun.Text == "중증화상")
                    //{
                    //    ssBohumNew1.ActiveSheet.Cells[2, 0].Text = "□ 결핵          ■ 중증화상";
                    //}

                    ssBohumNew1.ActiveSheet.Cells[3, 0].Text = "산정특례번호";

                    ssBohumNew1.ActiveSheet.Cells[4, 0].Text = "수진자";
                    ssBohumNew1.ActiveSheet.Cells[4, 2].Text = "① 건강보험증번호";
                    ssBohumNew1.ActiveSheet.Cells[4, 7].Text = "② 가입자(세대주)";

                    if (rdoTongbo0.Checked == true)
                    {
                        ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "■ 알림톡     □ 이메일";
                    }
                    else if (rdoTongbo1.Checked == true)
                    {
                        ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "□ 알림톡     ■ 이메일";
                    }

                    ssBohumNew1.ActiveSheet.Cells[9, 1].Text = "[요양기관 확인란]";
                    ssBohumNew1.ActiveSheet.Rows[10].Visible = true; // 신청구분란

                    if (chkFire1_1.Checked == true)
                    {
                        ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "■ 신규등록      □ 재등록";
                    }
                    else if (chkFire1_2.Checked == true)
                    {
                        ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "□ 신규등록      ■ 재등록";
                    }

                    ssBohumNew1.ActiveSheet.Cells[11, 1].Text = "② 진료과목";
                    ssBohumNew1.ActiveSheet.Cells[12, 1].Text = cboDeptNew.SelectedItem.ToString().Trim();

                    ssBohumNew1.ActiveSheet.Cells[11, 5].Text = "③ 구분";
                    if (optI.Checked == true)
                    {
                        ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "■ 입원      □ 외래";
                    }
                    else if (optO.Checked == true)
                    {
                        ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "□ 입원      ■ 외래";
                    }

                    ssBohumNew1.ActiveSheet.Cells[11, 8].Text = "④ 진단확진일";
                    if (dtpIO.Enabled == true && optIOY.Checked == true)
                    {
                        ssBohumNew1.ActiveSheet.Cells[12, 8].Text = dtpJinDate.Text + "/" + dtpIO.Text;
                    }
                    else
                    {
                        ssBohumNew1.ActiveSheet.Cells[12, 8].Text = dtpJinDate.Text;
                    }

                    ssBohumNew1.ActiveSheet.Cells[13, 1].Text = "⑤ 상병명";
                    ssBohumNew1.ActiveSheet.Cells[14, 1].Text = txtIllNameNew.Text.Trim();
                    ssBohumNew1.ActiveSheet.Cells[13, 2].Text = "";
                    ssBohumNew1.ActiveSheet.Cells[13, 5].Text = "⑥ 상병코드";
                    ssBohumNew1.ActiveSheet.Cells[14, 5].Text = txtIllCodeNew.Text.Trim();

                    ssBohumNew1.ActiveSheet.Cells[13, 8].Text = "⑦ 특정기호";
                    ssBohumNew1.ActiveSheet.Cells[14, 8].Text = txtGiho.Text.Trim();

                    ssBohumNew1.ActiveSheet.Cells[15, 1].Text = "⑧ 최종확진방법(임상적 소견으로 최종진단 시 기재)";
                    ssBohumNew1.ActiveSheet.Cells[16, 1].Text = " " + txt2.Text.Trim();

                    ssBohumNew1.ActiveSheet.Cells[19, 1].Text = "⑨ 재등록 또는 V306으로 신규등록하는 경우에만 작성";
                    ssBohumNew1.ActiveSheet.Cells[20, 1].Text = "  ⑨-1 수술개시일 : " + txt2_1.Text;

                    ssBohumNew1.ActiveSheet.Cells[21, 1].Text = "  ⑨-2 수술명 및 수술코드";
                    ssBohumNew1.ActiveSheet.Cells[22, 1].Text = VB.IIf(chkFire2_2_1.Checked == true, "    [ ■ ] ", "    [   ]") + " 1. 반흔구축성형술(운동제한이 있는 것)(N0241)";
                    ssBohumNew1.ActiveSheet.Cells[23, 1].Text = VB.IIf(chkFire2_2_2.Checked == true, "    [ ■ ] ", "    [   ]") + " 2. 반흔구축성형술 및 식피술(운동제한이 있는 것)(N0242~N0247, NA241~NA243)";
                    ssBohumNew1.ActiveSheet.Cells[24, 1].Text = VB.IIf(chkFire2_2_3.Checked == true, "    [ ■ ] ", "    [   ]") + " 3. 반흔구축성형술 및 국소피판술(운동제한이 있는 것)(N0249) ";

                    #endregion
                }


                //2019-01-08
                if (cboDoct.Text.Trim() != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = "담당의사 (면허번호/전문의 자격번호):  " + VB.Split(cboDoct.Text, ".")[1];
                }

                if (cboDoct.Text.Replace(".", "") != "" && cboDeptNew.Text != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = ssBohumNew1.ActiveSheet.Cells[37, 3].Text + "    (" +
                                                                clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDoct.Text, ".")[0]) + "/" +
                                                                GetInsaCertNo(clsDB.DbCon, VB.Split(cboDoct.Text, ".")[0], VB.Split(cboDeptNew.Text, ".")[0]) +
                                                                ")";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = ssBohumNew1.ActiveSheet.Cells[37, 3].Text + "    (" + VB.Space(10) + ")";
                }
                ////         

                if (cboDeptNew.Text.Replace(".", "") != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[38, 4].Text = "담당의사 전문과목 : " + "  " + VB.Split(cboDeptNew.Text, ".")[1];
                }

                ssBohumNew1.ActiveSheet.Cells[36, 4].Text = "요양기관명 (기 호):";

                ssBohumNew1.ActiveSheet.Cells[45, 1].Text = "국민건강보험공단 이사장 귀하";

                #endregion
            }


            //string strSabun = "";

            //strSabun = VB.Split(cboDoct.Text, ".")[0];

            //SetDrSign(ssBohumNew1, 37, 10, strSabun);
        }

        private string GetInsaCertNo(PsmhDb pDbCon, string strDrCode, string strDeptCode, string strGubun = "")
        {

            // strgubun = "2" 이면 전문의/세부전문의 두개다 가져오기
            string rtnVal = "";
            string strDEPTK = "";

            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            switch (strDeptCode)
            {
                case "MD":
                case "MG":
                case "MC":
                case "MP":
                case "ME":
                case "MO":
                case "MN":
                case "MI":
                case "MR":
                    strDEPTK = "내과";
                    break;
                default:

                    SQL = " SELECT REPLACE(DEPTNAMEK, ' ', '') DEPTK ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
                    SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE = '" + strDeptCode + "' ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        dt.Dispose();
                        dt = null;
                        return "";
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strDEPTK = dt.Rows[0]["DEPTK"].ToString();
                    }

                    dt.Dispose();
                    dt = null;

                    break;
            }

            if(strDEPTK == "")
            { return ""; }
            #region 이전 전문의 번호 가져오기
            //SQL = " SELECT BUNHO FROM KOSMOS_ADM.INSA_MSTL ";
            //SQL = SQL + ComNum.VBLF + " WHERE NAME LIKE '%전문의%' ";
            //SQL = SQL + ComNum.VBLF + "  AND NAME LIKE '%" + strDEPTK + "%'  ";
            //SQL = SQL + ComNum.VBLF + "  AND GIKWAN LIKE '%보건%' ";
            //SQL = SQL + ComNum.VBLF + "  AND GUBUN = '1' ";
            //SQL = SQL + ComNum.VBLF + "  AND SABUN IN ";
            //SQL = SQL + ComNum.VBLF + "                  ( SELECT SABUN FROM KOSMOS_OCS.OCS_DOCTOR ";
            //SQL = SQL + ComNum.VBLF + "                     WHERE DRCODE = '" + strDrCode + "' ";
            //SQL = SQL + ComNum.VBLF + "                       AND GBOUT = 'N') ";
            //SQL = SQL + ComNum.VBLF + " ORDER BY CHIDATE ASC "; 
            #endregion
            //2020-02-17 내과는 세부전문의번호 먼저 가져오고 없으면 그냥 전문의 가져오기
            SQL = "";
            
            SQL += ComNum.VBLF + "  SELECT NAME, BUNHO, '2' GUBUN, CHIDATE FROM KOSMOS_ADM.INSA_MSTL ";
            SQL += ComNum.VBLF + "  WHERE NAME LIKE '%전문의%' ";
            SQL += ComNum.VBLF + "  AND NAME LIKE '%" + strDEPTK + "%'  ";
            SQL += ComNum.VBLF + "    AND GIKWAN LIKE '%보건%' ";
            SQL += ComNum.VBLF + "    AND GUBUN = '1' ";
            SQL += ComNum.VBLF + "    AND SABUN IN ";
            SQL += ComNum.VBLF + "                   (SELECT SABUN FROM KOSMOS_OCS.OCS_DOCTOR ";
            SQL += ComNum.VBLF + "                     WHERE DRCODE = '" + strDrCode + "' ";
            SQL += ComNum.VBLF + "                       AND GBOUT = 'N') ";
            SQL += ComNum.VBLF + "  ORDER BY GUBUN ASC, CHIDATE ASC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                dt.Dispose();
                dt = null;
                return "";
            }
            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["BUNHO"].ToString();
            }

            dt.Dispose();
            dt = null;

            //세부전문의가져오기(2020-08-19
            if (strGubun == "2")
            {
                if (strDEPTK == "내과")
                {
                    SQL = " SELECT NAME, BUNHO, '1' GUBUN, CHIDATE FROM KOSMOS_ADM.INSA_MSTL ";
                    SQL += ComNum.VBLF + " WHERE NAME LIKE '%전문의%' ";
                    SQL += ComNum.VBLF + "   AND NAME LIKE '%분과%' ";
                    SQL += ComNum.VBLF + "   AND GUBUN = '1' ";
                    SQL += ComNum.VBLF + "   AND SABUN IN ";
                    SQL += ComNum.VBLF + "                   (SELECT SABUN FROM KOSMOS_OCS.OCS_DOCTOR ";
                    SQL += ComNum.VBLF + "                     WHERE DRCODE = '" + strDrCode + "' ";
                    SQL += ComNum.VBLF + "                       AND GBOUT = 'N') ";
                    SQL += ComNum.VBLF + "  ORDER BY GUBUN ASC, CHIDATE ASC ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        dt.Dispose();
                        dt = null;
                        return "";
                    }
                    if (dt.Rows.Count > 0)
                    {
                        rtnVal += "/" + dt.Rows[0]["BUNHO"].ToString();
                    }

                    dt.Dispose();
                    dt = null;

                }
            }



            return rtnVal;
        }

        private void toolGub_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strIndex = VB.Right(((ToolStripMenuItem)sender).Name, 1);
            
            if (strIndex == "1")
            {
                if (rdo8.Checked == true || rdo9.Checked == true)
                {
                    ComFunc.MsgBox("암을 선택하고 인쇄하세요.");
                    return;
                }
            }
            else if (strIndex == "2")
            {
                cboGubun.Items.Remove("중증치매");
                if (rdo7.Checked == true || rdo9.Checked == true)
                {
                    ComFunc.MsgBox("희귀난치치매를 선택하고 인쇄하세요.");
                    return;
                }
                if (rdo8.Checked == true && cboGubun.Text == "")
                {
                    ComFunc.MsgBox("버튼 옆 콤보박스에 값을 선택하세요!");
                    return;
                } 
            }
            else if (strIndex == "3")
            {
                if (rdo7.Checked == true || rdo8.Checked == true)
                {
                    ComFunc.MsgBox("결핵화상을 선택하고 인쇄하세요.");
                    return;
                }
                if (rdo9.Checked == true && cboGubun.Text != "결핵")
                {
                    ComFunc.MsgBox("버튼 옆 콤보박스에 값을 선택하세요!");
                    return;
                }
            }
            else if(strIndex == "4")
            {
                if (rdo7.Checked == true || rdo8.Checked == true)
                {
                    ComFunc.MsgBox("결핵화상을 선택하고 인쇄하세요.");
                    return;
                }
                if (rdo9.Checked == true && cboGubun.Text != "중증화상")
                {
                    ComFunc.MsgBox("버튼 옆 콤보박스에 값을 선택하세요!");
                    return;
                }
            }

            if (GstrBi != "21" && GstrBi != "22")
            {
                ComFunc.MsgBox("자격이 의료급여가 아닙니다.. 신청서를 건강보험양식으로 출력하십시오");
                return;
            }

           
            DATA_Gubnew_TOTAL_SET_NEW(strIndex);

            ssBohumNew1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssBohumNew1_Sheet1.PrintInfo.Margin.Top = 60;
            ssBohumNew1_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssBohumNew1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssBohumNew1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssBohumNew1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssBohumNew1_Sheet1.PrintInfo.ShowBorder = true;
            ssBohumNew1_Sheet1.PrintInfo.ShowColor = false;
            ssBohumNew1_Sheet1.PrintInfo.ShowGrid = true;
            ssBohumNew1_Sheet1.PrintInfo.ShowShadows = false;
            ssBohumNew1_Sheet1.PrintInfo.UseMax = false;
            ssBohumNew1_Sheet1.PrintInfo.UseSmartPrint = false;
            ssBohumNew1_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssBohumNew1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssBohumNew1_Sheet1.PrintInfo.Preview = false;
            ssBohumNew1.PrintSheet(0);

            ComFunc.Delay(200);

            if (chkEtc.Checked == true)
            {
                frmBuppatEtc frmBuppatEtcX = new frmBuppatEtc();
                frmBuppatEtcX.PrintGub(strIndex);

                if (frmBuppatEtcX != null)
                {
                    frmBuppatEtcX = null;
                }
            }
        }

        private void DATA_Gubnew_TOTAL_SET_NEW(string strIndex)
        {
            string strOK = "";
            string strOK2 = "";
            //스프레드 클리어
            ssBohumNew1.ActiveSheet.Cells[15, 1, 33, 1].Text = "";

            //수진자 기본정보
            ssBohumNew1.ActiveSheet.Cells[4, 4].Text = txtGKiho.Text.Trim() + GstrE000;    //건강보험증번호
            ssBohumNew1.ActiveSheet.Cells[4, 9].Text = txtPName.Text.Trim();    //가입자 또는 세대주명
            ssBohumNew1.ActiveSheet.Cells[5, 4].Text = txtSName.Text.Trim();    //성명
            ssBohumNew1.ActiveSheet.Cells[5, 9].Text = txtJumin.Text.Trim();    //주민(외국인)등록번호
            ssBohumNew1.ActiveSheet.Cells[6, 4].Text = txtHPhone.Text.Trim();   //휴대전화번호
            ssBohumNew1.ActiveSheet.Cells[6, 9].Text = txtTel.Text.Trim();      //자택전화번호
            ssBohumNew1.ActiveSheet.Cells[7, 4].Text = txtEmail.Text.Trim();    //이메일주소
            ssBohumNew1.ActiveSheet.Cells[8, 4].Text = txtJuso.Text.Trim();     //자택주소
            ssBohumNew1.ActiveSheet.Cells[47, 1].Text = "등록번호 : " + txtPtNo.Text.Trim();    //등록번호

            //신청자입력란
            //동의일            
            ssBohumNew1.ActiveSheet.Cells[35, 6].Text = dtpDongDate.Value.ToString("yyyy 년 MM 월 dd 일");

            //신청일
            ssBohumNew1.ActiveSheet.Cells[42, 10].Text = dtpSDate.Value.ToString("yyyy 년 MM 월 dd 일");

            //신청자
            ssBohumNew1.ActiveSheet.Cells[43, 9].Text = txtSinName.Text;

            //관계
            ssBohumNew1.ActiveSheet.Cells[44, 9].Text = VB.Mid(cboGan.Text, 4, cboGan.Text.Length);

            if (strIndex == "1")
            {
                #region 암 (빈서식지)

                ssBohumNew1.ActiveSheet.Cells[0, 1].Text = "의료급여 (암) 산정특례 등록 신청서";

                ssBohumNew1.ActiveSheet.Rows[2].Visible = false; // 체크항목 표기란 

                ssBohumNew1.ActiveSheet.Cells[3, 1].Text = "산정특례등록번호";

                ssBohumNew1.ActiveSheet.Cells[4, 0].Text = "진료받은 사람";
                ssBohumNew1.ActiveSheet.Cells[4, 2].Text = "① 보장기관명";
                ssBohumNew1.ActiveSheet.Cells[4, 7].Text = "② 세대주 성명";

                if (rdoTongbo0.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "■ 알림톡     □ 이메일";
                }
                else if (rdoTongbo1.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "□ 알림톡     ■ 이메일";
                }


                ssBohumNew1.ActiveSheet.Cells[9, 1].Text = "[의료급여기관 확인란]";
                ssBohumNew1.ActiveSheet.Rows[10].Visible = true; // 신청구분란

                if (optSinGu0.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "■ 신규암      □ 재등록암      □ 중복암";
                }
                else if (optSinGu1.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "□ 신규암      ■ 재등록암      □ 중복암";
                }
                else if (optSinGu2.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "□ 신규암      □ 재등록암      ■ 중복암";
                }

                ssBohumNew1.ActiveSheet.Cells[11, 1].Text = "② 진료과목";
                ssBohumNew1.ActiveSheet.Cells[12, 1].Text = cboDeptNew.SelectedItem.ToString().Trim();

                ssBohumNew1.ActiveSheet.Cells[11, 5].Text = "③ 진료구분";

                if (optI.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "■ 입원      □ 외래";
                }
                else if (optO.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "□ 입원      ■ 외래";
                }

                ssBohumNew1.ActiveSheet.Cells[11, 8].Text = "④ 진단확진일 *입원초일 있는 경우 기재";
                if (dtpIO.Enabled == true && optIOY.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 8].Text = dtpJinDate.Text + "/" + dtpIO.Text;
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 8].Text = dtpJinDate.Text;
                }

                ssBohumNew1.ActiveSheet.Cells[13, 1].Text = "⑤ 상병명";
                ssBohumNew1.ActiveSheet.Cells[14, 1].Text = txtIllNameNew.Text.Trim();


                if (optWonbar.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[13, 2].Text = "(■ 원발   □ 전이)";
                }
                else if (optJeon.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[13, 2].Text = "(□ 원발   ■ 전이)";
                }

                ssBohumNew1.ActiveSheet.Cells[13, 5].Text = "⑥ 상병코드";
                ssBohumNew1.ActiveSheet.Cells[14, 5].Text = txtIllCodeNew.Text.Trim();

                ssBohumNew1.ActiveSheet.Cells[13, 8].Text = "⑦ 특정기호";
                ssBohumNew1.ActiveSheet.Cells[14, 8].Text = txtGiho.Text.Trim();

                ssBohumNew1.ActiveSheet.Cells[15, 1].Text = "⑧ 최종확진방법           ※중복체크가능";

                if (chkJojic.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[16, 1].Text = "   ■ 1. 조직학적 검사";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[16, 1].Text = "   □ 1. 조직학적 검사";
                }

                if (chkCytoExam.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "   ■ 2. 세포학적 검사";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "   □ 2. 세포학적 검사";
                }

                /// 영상검사 

                if (chkMRI.Checked == true || chkCT.Checked == true || chkSono.Checked == true || chkExamETC.Checked == true)
                {
                    strOK += "   ■ 3. 영상검사";
                }
                else
                {
                    strOK += "   □ 3. 영상검사";
                }
                if (chkMRI.Checked == true)
                {
                    strOK += "  ■ MRI";
                }
                else
                {
                    strOK += "  □ MRI";
                }
                if (chkCT.Checked == true)
                {
                    if (txtCT.Text.Trim() != "")
                    {
                        strOK += "               ■ CT (소견 : " + txtCT.Text.Trim() + " )";
                    }
                    else
                    {
                        strOK += "               ■ CT (소견 :                          )";
                    }
                }
                else
                {
                    strOK += "               □ CT (소견 :                          )";
                }
                if (chkSono.Checked == true)
                {
                    strOK2 += "                   ■ Sono";
                }
                else
                {
                    strOK2 += "                   □ Sono";
                }
                if (chkExamETC.Checked == true)
                {
                    if (txtExamETC.Text.Trim() != "")
                    {
                        strOK2 += "              ■ 기타( " + txtCT.Text.Trim() + " )";
                    }
                    else
                    {
                        strOK2 += "              ■ 기타 (                              )";
                    }
                }
                else
                {
                    strOK2 += "              □ 기타 (                              )";
                }

                ssBohumNew1.ActiveSheet.Cells[18, 1].Text = strOK;
                ssBohumNew1.ActiveSheet.Cells[19, 1].Text = strOK2;

                ///


                /// 4번항목 체크

                strOK = "";
                if (chkBioExam.Checked == true)
                {
                    strOK += "   ■ 4. 특수 생화학적 검사";
                }
                else
                {
                    strOK += "   □ 4. 특수 생화학적 검사";
                }
                if (chkImmExam.Checked == true)
                {
                    strOK += "    ■ 면역학적 검사";
                }
                else
                {
                    strOK += "    □ 면역학적 검사";
                }
                if (chkCytoExam.Checked == true)
                {
                    strOK += "   ■ 혈액학적 검사";
                }
                else
                {
                    strOK += "   □ 혈액학적 검사";
                }

                ssBohumNew1.ActiveSheet.Cells[20, 1].Text = strOK;

                ///

                if (chkNoneJojic.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[21, 1].Text = "   ■ 5. 조직검사 없는 진단적 수술";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[21, 1].Text = "   □ 5. 조직검사 없는 진단적 수술";
                }

                if (chkETCExam.Checked == true)
                {
                    if (txtETCExam.Text.Trim() != "")
                    {
                        ssBohumNew1.ActiveSheet.Cells[22, 1].Text = "   ■ 6. 기타( " + txtETCExam.Text.Trim() + " )";
                    }
                    else
                    {
                        ssBohumNew1.ActiveSheet.Cells[22, 1].Text = "   ■ 6. 기타(                                )";
                    }
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[22, 1].Text = "   □ 6. 기타(                                )";
                }



                //ssBohumNew1.ActiveSheet.Cells[23, 1].Text = "⑨ 조직학적 □ 세포학적 검사 필수인 상병에서 조직학적 □ 세포학적 검사 불가하여 등록기준 미충족한 경우에만 작성*";
                //ssBohumNew1.ActiveSheet.Cells[24, 1].Text = "     * 상병별 등록기준을 미충족한 경우에는 전문의가 환자상태 및 진료소견을 구체적으로 기재 후 신청서를 발행하여야 함";


                if (chkCyto.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[23, 1].Text = "⑨ 조직학적 ■ 세포학적 검사 필수인 상병에서 조직학적 □ 세포학적 검사 불가하여 등록기준 미충족한 경우 작성*";
                }
                else if (chkNoneCyto.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[23, 1].Text = "⑨ 조직학적 □ 세포학적 검사 필수인 상병에서 조직학적 ■ 세포학적 검사 불가하여 등록기준 미충족한 경우 작성*";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[23, 1].Text = "⑨ 조직학적 □ 세포학적 검사 필수인 상병에서 조직학적 □ 세포학적 검사 불가하여 등록기준 미충족한 경우 작성*";
                }

                ///  미실시사유
                ///                

                ssBohumNew1.ActiveSheet.Cells[24, 1].Text = "     * 상병별 등록기준을 미충족한 경우 전문의가 환자상태 및 진료소견을 구체적으로 기재 후 신청서를 발행";

                ssBohumNew1.ActiveSheet.Cells[25, 1].Text = " ⑨-1 조직학적 □ 세포학적 검사 미실시 사유         ※중복체크가능";

                if (chkNoneCytoSayu1.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[26, 1].Text = "  ■ 1. 전신상태가 ECOG performance status 3 이상인 경우";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[26, 1].Text = "  □ 1. 전신상태가 ECOG performance status 3 이상인 경우";
                }
                if (chkNoneCytoSayu2.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[27, 1].Text = "  ■ 2. 출혈 위험성이 큰 경우";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[27, 1].Text = "  □ 2. 출혈 위험성이 큰 경우";
                }
                if (chkNoneCytoSayu3.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[28, 1].Text = "  ■ 3. 검사를 위한 전신마취 및 수술을 견딜 수 없는 경우";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[28, 1].Text = "  □ 3. 검사를 위한 전신마취 및 수술을 견딜 수 없는 경우";
                }
                if (chkNoneCytoSayu4.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[29, 1].Text = "  ■ 4. 감염 위험성이 높은 경우";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[29, 1].Text = "  □ 4. 감염 위험성이 높은 경우";
                }
                if (chkNoneCytoSayu5.Checked == true || txtNoneCytoSayu5.Text.Trim() != "")
                {
                    if (txtNoneCytoSayu5.Text.Trim() != "")
                    {
                        ssBohumNew1.ActiveSheet.Cells[30, 1].Text = "  ■ 5. 기타( " + txtNoneCytoSayu5.Text.Trim() + " )";
                    }
                    else
                    {
                        ssBohumNew1.ActiveSheet.Cells[30, 1].Text = "  ■ 5. 기타(                                                                                      )";
                    }
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[30, 1].Text = "  □ 5. 기타(                                                                                      )";
                }

                ///

                ssBohumNew1.ActiveSheet.Cells[32, 1].Text = " ⑨-2 환자상태 및 진료소견(확진의견을 포함하여 구체적으로 기재)";

                if (txtPatJinRemark.Text.Trim() != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[33, 1].Text = txtPatJinRemark.Text.Trim();
                }

                //2019-01-08
                if (cboDoct.Text.Trim() != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = "담당의사 (면허번호/전문의 자격번호):  " + VB.Split(cboDoct.Text, ".")[1];
                }

                if (cboDoct.Text.Replace(".", "") != "" && cboDeptNew.Text != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = ssBohumNew1.ActiveSheet.Cells[37, 3].Text.Trim() + " (" +
                                                                clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDoct.Text, ".")[0]) + "/" +
                                                                GetInsaCertNo(clsDB.DbCon, VB.Split(cboDoct.Text, ".")[0], VB.Split(cboDeptNew.Text, ".")[0]) +
                                                                ")";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = ssBohumNew1.ActiveSheet.Cells[37, 3].Text + "    (" + VB.Space(10) + ")";
                }

                if (cboDeptNew.Text.Replace(".", "") != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[38, 4].Text = "담당의사 전문과목 :" + "  " + VB.Split(cboDeptNew.Text, ".")[1];
                }


                ssBohumNew1.ActiveSheet.Cells[36, 3].Text = "             의료급여기관명 (기호) : ";
                ssBohumNew1.ActiveSheet.Cells[36, 4].Text = "";

                ssBohumNew1.ActiveSheet.Cells[45, 1].Text = "시장·군수·구청장 귀하";

                #endregion

            }
            else if (strIndex == "2")
            {
                #region 기타 산정특례

                if (cboGubun.Text != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[0, 1].Text = "의료급여 (" + cboGubun.Text.Trim() + ") 산정특례 등록 신청서";
                }


                ssBohumNew1.ActiveSheet.Rows[2].Visible = true; // 체크항목 표기란
                if (cboGubun.SelectedItem.ToString().Trim() == "희귀")
                {
                    ssBohumNew1.ActiveSheet.Cells[2, 0].Text = "■ 희귀질환          □ 중증난치질환";
                }
                else if (cboGubun.SelectedItem.ToString().Trim() == "중증난치질환")
                {
                    ssBohumNew1.ActiveSheet.Cells[2, 0].Text = "□ 희귀질환          ■ 중증난치질환";
                }                

                ssBohumNew1.ActiveSheet.Cells[3, 0].Text = "산정특례번호";

                ssBohumNew1.ActiveSheet.Cells[4, 0].Text = "진료받은 사람";
                ssBohumNew1.ActiveSheet.Cells[4, 2].Text = "① 보장기관명";
                ssBohumNew1.ActiveSheet.Cells[4, 7].Text = "② 세대주 성명";

                if (rdoTongbo0.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "■ 알림톡     □ 이메일";
                }
                else if (rdoTongbo1.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "□ 알림톡     ■ 이메일";
                }

                ssBohumNew1.ActiveSheet.Cells[9, 1].Text = "[의료급여기관 확인란]";
                ssBohumNew1.ActiveSheet.Rows[10].Visible = true; // 신청구분란

                if (optSinGu0.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "■ 신규      □ 재등록";
                }
                else if (optSinGu1.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "□ 신규      ■ 재등록";
                }

                ssBohumNew1.ActiveSheet.Cells[11, 1].Text = "② 진료과목";
                ssBohumNew1.ActiveSheet.Cells[12, 1].Text = cboDeptNew.SelectedItem.ToString().Trim();

                ssBohumNew1.ActiveSheet.Cells[11, 5].Text = "③ 진료구분";             

                ssBohumNew1.ActiveSheet.Cells[11, 8].Text = "④ 진단확진일 *입원초일 있는 경우 기재";
                if (dtpIO.Enabled == true && optIOY.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 8].Text = dtpJinDate.Text + "/" + dtpIO.Text;
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 8].Text = dtpJinDate.Text;
                }

                if (optI.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "■ 입원      □ 외래";
                }
                else if (optO.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "□ 입원      ■ 외래";
                }

                ssBohumNew1.ActiveSheet.Cells[13, 1].Text = "⑤ 상병명";
                ssBohumNew1.ActiveSheet.Cells[14, 1].Text = txtIllNameNew.Text.Trim();
                ssBohumNew1.ActiveSheet.Cells[13, 2].Text = "";
                ssBohumNew1.ActiveSheet.Cells[13, 5].Text = "⑥ 상병코드";
                ssBohumNew1.ActiveSheet.Cells[14, 5].Text = txtIllCodeNew.Text.Trim();

                ssBohumNew1.ActiveSheet.Cells[13, 8].Text = "⑦ 특정기호";
                ssBohumNew1.ActiveSheet.Cells[14, 8].Text = txtGiho.Text.Trim();
                ssBohumNew1.ActiveSheet.Cells[15, 1].Text = "⑧ 최종확진방법           ※중복체크가능";

                //// 영상체크

                if (chkSono.Checked == true || chkMRI.Checked == true || chkCT.Checked == true || chkExamETC.Checked == true || chkXray.Checked == true)
                {
                    strOK += "  ■ 1. 영상검사";
                }
                else
                {
                    strOK += "  □ 1. 영상검사";
                }
                if (chkXray.Checked == true)
                {
                    strOK += "  ■ X-ray";
                }
                else
                {
                    strOK += "  □ X-ray";
                }
                if (chkCT.Checked == true)
                {
                    strOK += "    ■ CT";
                }
                else
                {
                    strOK += "    □ CT";
                }

                if (chkSono.Checked == true)
                {
                    strOK += "    ■ Sono";
                }
                else
                {
                    strOK += "    □ Sono";
                }

                if (chkMRI.Checked == true)
                {
                    strOK += "    ■ MRI";
                }
                else
                {
                    strOK += "    □ MRI";
                }

                if (chkExamETC.Checked == true)
                {
                    if (txtExamETC.Text.Trim() != "")
                    {
                        strOK += "    ■ 기타( " + txtExamETC.Text.Trim() + " )";
                    }
                    else
                    {
                        strOK += "    ■ 기타(                              )";
                    }
                }
                else
                {
                    strOK += "    □ 기타(                              )";
                }

                ssBohumNew1.ActiveSheet.Cells[16, 1].Text = strOK;

                //// 

                if (chkJindanNew2.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "  ■ 2. 특수생화학/면역학, 도말/배양검사 등";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "  □ 2. 특수생화학/면역학, 도말/배양검사 등";
                }

                if (chkJindanNew3.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[18, 1].Text = "  ■ 3. 유전학적 검사";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[18, 1].Text = "  □ 3. 유전학적 검사";
                }

                if (chkJindanNew44.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[19, 1].Text = "  ■ 4. 조직학적 검사";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[19, 1].Text = "  □ 4. 조직학적 검사";
                }

                if (chkJindanNew5.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[20, 1].Text = "  ■ 5. 임상적 소견";

                    if (txtJindanNew5.Text.Trim() != "")
                    {
                        ssBohumNew1.ActiveSheet.Rows[21].Height = 36;   //임상적소견 란
                        FarPoint.Win.Spread.CellType.TextCellType TCT = new FarPoint.Win.Spread.CellType.TextCellType();
                        TCT.Multiline = true;
                        ssBohumNew1.ActiveSheet.Cells[21, 1].CellType = TCT;

                        ssBohumNew1.ActiveSheet.Cells[21, 1].Text = txtJindanNew5.Text.Trim();
                    }
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[20, 1].Text = "  □ 5. 임상적 소견";
                }

                if (chkJindanNew6.Checked == true && txtJindanNew6.Text.Trim() != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[22, 1].Text = "  ■ 6. 기타 ( " + txtJindanNew6.Text.Trim() + " 검사 )";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[22, 1].Text = "  □ 6. 기타 (                      검사 )";
                }

                //ssBohumNew1.ActiveSheet.Cells[23, 1].Text = "⑨ 조직학적 □ 세포학적 검사 필수인 상병에서 조직학적 □ 세포학적 검사 불가하여 등록기준 미충족한 경우에만 작성*";
                //ssBohumNew1.ActiveSheet.Cells[24, 1].Text = "     * 상병별 등록기준을 미충족한 경우에는 전문의가 환자상태 및 진료소견을 구체적으로 기재 후 신청서를 발행하여야 함";

                ssBohumNew1.ActiveSheet.Cells[23, 1].Text = "⑨ 질병정보 - 가족력                      ※ 희귀질환(극희귀·상세불명 희귀·기타염색제 이상질환 포함) 필수";

                /// 가족력

                strOK = "";
                if (optFamY.Checked == true)
                {
                    if (chkFam1.Checked == true)
                    {
                        strOK += "   □ 없음    ■ 있음( ■ 조부";
                    }
                    else
                    {
                        strOK += "   □ 없음    ■ 있음( □ 조부";
                    }
                    if (chkFam2.Checked == true)
                    {
                        strOK += " ■ 조모";
                    }
                    else
                    {
                        strOK += " □ 조모";
                    }
                    if (chkFam3.Checked == true)
                    {
                        strOK += " ■ 외조부";
                    }
                    else
                    {
                        strOK += " □ 외조부";
                    }
                    if (chkFam4.Checked == true)
                    {
                        strOK += " ■ 외조모";
                    }
                    else
                    {
                        strOK += " □ 외조모";
                    }
                    if (chkFam5.Checked == true)
                    {
                        strOK += " ■ 부";
                    }
                    else
                    {
                        strOK += " □ 부";
                    }
                    if (chkFam6.Checked == true)
                    {
                        strOK += " ■ 모";
                    }
                    else
                    {
                        strOK += " □ 모";
                    }
                    if (chkFam7.Checked == true)
                    {
                        strOK += " ■ 동성형제";
                    }
                    else
                    {
                        strOK += " □ 동성형제";
                    }
                    if (chkFam8.Checked == true)
                    {
                        strOK += " ■ 이성형제";
                    }
                    else
                    {
                        strOK += " □ 이성형제";
                    }
                    if (chkFam9.Checked == true)
                    {
                        strOK += " ■ 자";
                    }
                    else
                    {
                        strOK += " □ 자";
                    }
                    if (chkFam10.Checked == true)
                    {
                        strOK += " ■ 녀)";
                    }
                    else
                    {
                        strOK += " □ 녀)";
                    }
                    ssBohumNew1.ActiveSheet.Cells[24, 1].Text = strOK;
                }
                else if (optFamN.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[24, 1].Text = "   ■ 없음    □ 있음( □ 조부 □ 조모 □ 외조부 □ 외조모 □ 부 □ 모 □ 동성형제 □ 이성형제 □ 자 □ 녀)";
                }

                ///2019-01-08
                if (cboDoct.Text.Trim() != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = "담당의사 (면허번호/전문의 자격번호):  " + VB.Split(cboDoct.Text, ".")[1];
                }

                if (cboDoct.Text.Replace(".", "") != "" && cboDeptNew.Text != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = ssBohumNew1.ActiveSheet.Cells[37, 3].Text + "    (" +
                                                                clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDoct.Text, ".")[0]) + "/" +
                                                                GetInsaCertNo(clsDB.DbCon, VB.Split(cboDoct.Text, ".")[0], VB.Split(cboDeptNew.Text, ".")[0]) +
                                                                ")";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = ssBohumNew1.ActiveSheet.Cells[37, 3].Text + "    (" + VB.Space(10) + ")";
                }


                if (cboDeptNew.Text.Replace(".", "") != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[38, 4].Text = "담당의사 전문과목 :" + "  " + VB.Split(cboDeptNew.Text, ".")[1];
                }

                ssBohumNew1.ActiveSheet.Cells[36, 3].Text = "             의료급여기관명 (기호) : ";
                ssBohumNew1.ActiveSheet.Cells[36, 4].Text = "";

                ssBohumNew1.ActiveSheet.Cells[45, 1].Text = "시장·군수·구청장 귀하";

                #endregion
            }
            else if (strIndex == "3")
            {
                #region 결핵

                if (cboGubun.Text != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[0, 1].Text = "의료급여 (" + cboGubun.Text.Trim() + ") 산정특례 등록 신청서";
                }                

                ssBohumNew1.ActiveSheet.Rows[2].Visible = false; // 체크항목 표기란 

                //if (cboGubun.Text == "결핵")
                //{
                //    ssBohumNew1.ActiveSheet.Cells[2, 0].Text = "■ 결핵          □ 중증화상";
                //}
                //else if (cboGubun.Text == "중증화상")
                //{
                //    ssBohumNew1.ActiveSheet.Cells[2, 0].Text = "□ 결핵          ■ 중증화상";
                //}

                ssBohumNew1.ActiveSheet.Cells[3, 0].Text = "산정특례번호";

                ssBohumNew1.ActiveSheet.Cells[4, 0].Text = "진료받은 사람";
                ssBohumNew1.ActiveSheet.Cells[4, 2].Text = "① 보장기관명";
                ssBohumNew1.ActiveSheet.Cells[4, 7].Text = "② 세대주 성명";

                if (rdoTongbo0.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "■ 알림톡     □ 이메일";
                }
                else if (rdoTongbo1.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "□ 알림톡     ■ 이메일";
                }

                ssBohumNew1.ActiveSheet.Cells[9, 1].Text = "[의료급여기관 확인란]";

                ssBohumNew1.ActiveSheet.Rows[10].Visible = false; // 신청구분란
                //ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "□ 신규      □ 재등록";

                ssBohumNew1.ActiveSheet.Cells[11, 1].Text = "① 진료과목";
                ssBohumNew1.ActiveSheet.Cells[12, 1].Text = cboDeptNew.SelectedItem.ToString().Trim();

                ssBohumNew1.ActiveSheet.Cells[11, 5].Text = "② 구분";

                if (dtpIO.Enabled == true && optIOY.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[11, 8].Text = "③ 진단확진일   /*입원초일 ";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[11, 8].Text = "③ 진단확진일";
                }

                

                if (dtpIO.Enabled == true && optIOY.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 8].Text = dtpJinDate.Text + "/   " + dtpIO.Text;
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 8].Text = dtpJinDate.Text;
                }

                if (optI.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "■ 입원      □ 외래";
                }
                else if (optO.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "□ 입원      ■ 외래";
                }

                ssBohumNew1.ActiveSheet.Cells[13, 1].Text = "④ 상병명";
                ssBohumNew1.ActiveSheet.Cells[14, 1].Text = txtIllNameNew.Text.Trim();
                ssBohumNew1.ActiveSheet.Cells[13, 2].Text = "";
                ssBohumNew1.ActiveSheet.Cells[13, 5].Text = "⑤ 상병코드";
                ssBohumNew1.ActiveSheet.Cells[14, 5].Text = txtIllCodeNew.Text.Trim();

                ssBohumNew1.ActiveSheet.Cells[13, 8].Text = "⑥ 특정기호";
                ssBohumNew1.ActiveSheet.Cells[14, 8].Text = txtGiho.Text.Trim();

                ssBohumNew1.ActiveSheet.Cells[15, 1].Text = "⑦ 최종확진방법           ※중복체크가능";

                //// 영상체크

                if (chkSono.Checked == true || chkMRI.Checked == true || chkCT.Checked == true || chkExamETC.Checked == true || chkXray.Checked == true)
                {
                    strOK += "  ■ 1. 영상검사";
                }
                else
                {
                    strOK += "  □ 1. 영상검사";
                }
                if (chkXray.Checked == true)
                {
                    strOK += "  ■ X-ray";
                }
                else
                {
                    strOK += "  □ X-ray";
                }
                if (chkCT.Checked == true)
                {
                    strOK += "    ■ CT";
                }
                else
                {
                    strOK += "    □ CT";
                }

                if (chkSono.Checked == true)
                {
                    strOK += "    ■ Sono";
                }
                else
                {
                    strOK += "    □ Sono";
                }

                if (chkMRI.Checked == true)
                {
                    strOK += "    ■ MRI";
                }
                else
                {
                    strOK += "    □ MRI";
                }

                if (chkExamETC.Checked == true)
                {
                    if (txtExamETC.Text.Trim() != "")
                    {
                        strOK += "    ■ 기타( " + txtExamETC.Text.Trim() + " )";
                    }
                    else
                    {
                        strOK += "    ■ 기타(                              )";
                    }
                }
                else
                {
                    strOK += "    □ 기타(                              )";
                }

                ssBohumNew1.ActiveSheet.Cells[16, 1].Text = strOK;

                //// 

                if (chkJindanNewT21.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "   ■ 2. 도말/배양검사  ■ 도말    □ 배양";
                }
                else if (chkJindanNewT22.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "   ■ 2. 도말/배양검사  □ 도말    ■ 배양";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[17, 1].Text = "   □ 2. 도말/배양검사  □ 도말    □ 배양";
                }

                if (chkJindanNewT3.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[18, 1].Text = "   ■ 3. 조직학적 검사";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[18, 1].Text = "   □ 3. 조직학적 검사";
                }

                if (chkJindanNewT4.Checked == true || txtJindanNewT4.Text.Trim() != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[19, 1].Text = "   ■ 4. 임상적 소견 ( " + txtJindanNewT4.Text.Trim() + " )";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[19, 1].Text = "   □ 4. 임상적 소견 (                                                                                  )";
                }

                if (chkJindanNewT5.Checked == true || txtJindanNewT5.Text.Trim() != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[20, 1].Text = "   ■ 5. 기타 ( " + txtJindanNewT5.Text.Trim() + " 검사)";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[20, 1].Text = "   □ 5. 기타 (                  검사)";
                }

                ////
                strOK = "";

                ssBohumNew1.ActiveSheet.Cells[21, 1].Text = "⑧ (결핵만 해당) 타 요양기관의 검사결과로 확진한 경우, 해당사항 체크           ※중복체크가능";

                if (optTuberN.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[22, 1].Text = "   ■ 없음    □ 있음 (□ 1.영상검사 □ 2.도말/배양검사 □ 3.조직학적 검사 □ 5.기타)";
                }
                else if (optTuberY.Checked == true)
                {
                    if (chkTuber1.Checked == true)
                    {
                        strOK += "   □ 없음    ■ 있음 (■ 1.영상검사";
                    }
                    else
                    {
                        strOK += "   □ 없음    ■ 있음 (□ 1.영상검사";
                    }
                    if (chkTuber2.Checked == true)
                    {
                        strOK += " ■ 2.도말/배양검사";
                    }
                    else
                    {
                        strOK += " □ 2.도말/배양검사";
                    }
                    if (chkTuber3.Checked == true)
                    {
                        strOK += " ■ 3.조직학적 검사";
                    }
                    else
                    {
                        strOK += " □ 3.조직학적 검사";
                    }
                    if (chkTuber5.Checked == true)
                    {
                        strOK += " ■ 5.기타)";
                    }
                    else
                    {
                        strOK += " □ 5.기타)";
                    }

                    ssBohumNew1.ActiveSheet.Cells[22, 1].Text = strOK;
                }

                ////         2019-01-08
                if (cboDoct.Text.Trim() != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = "담당의사 (면허번호/전문의 자격번호):  " + VB.Split(cboDoct.Text, ".")[1];
                }

                if (cboDoct.Text.Replace(".", "") != "" && cboDeptNew.Text != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = ssBohumNew1.ActiveSheet.Cells[37, 3].Text + "    (" +
                                                                clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDoct.Text, ".")[0]) + "/" +
                                                                GetInsaCertNo(clsDB.DbCon, VB.Split(cboDoct.Text, ".")[0], VB.Split(cboDeptNew.Text, ".")[0]) +
                                                                ")";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = ssBohumNew1.ActiveSheet.Cells[37, 3].Text + "    (" + VB.Space(10) + ")";
                }


                if (cboDeptNew.Text.Replace(".", "") != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[38, 4].Text = "담당의사 전문과목 :" + "  " + VB.Split(cboDeptNew.Text, ".")[1];
                }

                ssBohumNew1.ActiveSheet.Cells[36, 3].Text = "             의료급여기관명 (기호) : ";
                ssBohumNew1.ActiveSheet.Cells[36, 4].Text = "";

                ssBohumNew1.ActiveSheet.Cells[45, 1].Text = "시장·군수·구청장 귀하";


                #endregion
            }
            else if (strIndex == "4")
            {
                #region 화상

                if (cboGubun.Text != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[0, 1].Text = "의료급여 (" + cboGubun.Text.Trim() + ") 산정특례 등록 신청서";
                }

                ssBohumNew1.ActiveSheet.Rows[2].Visible = false; // 체크항목 표기란 
                ssBohumNew1.ActiveSheet.Cells[3, 0].Text = "산정특례번호";
                ssBohumNew1.ActiveSheet.Cells[4, 0].Text = "진료받은 사람";
                ssBohumNew1.ActiveSheet.Cells[4, 2].Text = "① 보장기관명";
                ssBohumNew1.ActiveSheet.Cells[4, 7].Text = "② 세대주 성명";

                if (rdoTongbo0.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "■ 알림톡     □ 이메일";
                }
                else if (rdoTongbo1.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[7, 9].Text = "□ 알림톡     ■ 이메일";
                }

                ssBohumNew1.ActiveSheet.Cells[9, 1].Text = "[의료급여기관 확인란]";
                ssBohumNew1.ActiveSheet.Rows[10].Visible = true; // 신청구분란

                if (chkFire1_1.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "■ 신규등록      □ 재등록";
                }
                else if (chkFire1_2.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[10, 2].Text = "□ 신규등록      ■ 재등록";
                }

                ssBohumNew1.ActiveSheet.Cells[11, 1].Text = "② 진료과목";
                ssBohumNew1.ActiveSheet.Cells[12, 1].Text = cboDeptNew.SelectedItem.ToString().Trim();

                ssBohumNew1.ActiveSheet.Cells[11, 5].Text = "③ 구분";
                if (optI.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "■ 입원      □ 외래";
                }
                else if (optO.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 5].Text = "□ 입원      ■ 외래";
                }

                ssBohumNew1.ActiveSheet.Cells[11, 8].Text = "④ 진단확진일";
                if (dtpIO.Enabled == true && optIOY.Checked == true)
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 8].Text = dtpJinDate.Text + "/" + dtpIO.Text;
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[12, 8].Text = dtpJinDate.Text;
                }

                ssBohumNew1.ActiveSheet.Cells[13, 1].Text = "⑤ 상병명";
                ssBohumNew1.ActiveSheet.Cells[14, 1].Text = txtIllNameNew.Text.Trim();
                ssBohumNew1.ActiveSheet.Cells[13, 2].Text = "";
                ssBohumNew1.ActiveSheet.Cells[13, 5].Text = "⑥ 상병코드";
                ssBohumNew1.ActiveSheet.Cells[14, 5].Text = txtIllCodeNew.Text.Trim();

                ssBohumNew1.ActiveSheet.Cells[13, 8].Text = "⑦ 특정기호";
                ssBohumNew1.ActiveSheet.Cells[14, 8].Text = txtGiho.Text.Trim();

                ssBohumNew1.ActiveSheet.Cells[15, 1].Text = "⑧ 최종확진방법(임상적 소견으로 최종진단 시 기재)";
                ssBohumNew1.ActiveSheet.Cells[16, 1].Text = " " + txt2.Text.Trim();

                ssBohumNew1.ActiveSheet.Cells[19, 1].Text = "⑨ 재등록 또는 V306으로 신규등록하는 경우에만 작성";
                ssBohumNew1.ActiveSheet.Cells[20, 1].Text = "  ⑨-1 수술개시일 : " + txt2_1.Text;

                ssBohumNew1.ActiveSheet.Cells[21, 1].Text = "  ⑨-2 수술명 및 수술코드";
                ssBohumNew1.ActiveSheet.Cells[22, 1].Text = VB.IIf(chkFire2_2_1.Checked == true, "    [ ■ ] ", "   [   ]") + " 1. 반흔구축성형술(운동제한이 있는 것)(N0241)";
                ssBohumNew1.ActiveSheet.Cells[23, 1].Text = VB.IIf(chkFire2_2_2.Checked == true, "    [ ■ ] ", "   [   ]") + " 2. 반흔구축성형술 및 식피술(운동제한이 있는 것)(N0242~N0247, NA241~NA243)";
                ssBohumNew1.ActiveSheet.Cells[24, 1].Text = VB.IIf(chkFire2_2_3.Checked == true, "    [ ■ ] ", "   [   ]") + " 3. 반흔구축성형술 및 국소피판술(운동제한이 있는 것)(N0249) ";


                if (cboDoct.Text.Trim() != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = "담당의사 (면허번호/전문의 자격번호):  " + VB.Split(cboDoct.Text, ".")[1];
                }

                if (cboDoct.Text.Replace(".", "") != "" && cboDeptNew.Text != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = ssBohumNew1.ActiveSheet.Cells[37, 3].Text + "    (" +
                                                                clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDoct.Text, ".")[0]) + "/" +
                                                                GetInsaCertNo(clsDB.DbCon, VB.Split(cboDoct.Text, ".")[0], VB.Split(cboDeptNew.Text, ".")[0]) +
                                                                ")";
                }
                else
                {
                    ssBohumNew1.ActiveSheet.Cells[37, 3].Text = ssBohumNew1.ActiveSheet.Cells[37, 3].Text + "    (" + VB.Space(10) + ")";
                }


                if (cboDeptNew.Text.Replace(".", "") != "")
                {
                    ssBohumNew1.ActiveSheet.Cells[38, 4].Text = "담당의사 전문과목 :" + "  " + VB.Split(cboDeptNew.Text, ".")[1];
                }

                ssBohumNew1.ActiveSheet.Cells[36, 3].Text = "             의료급여기관명 (기호) : ";
                ssBohumNew1.ActiveSheet.Cells[36, 4].Text = "";

                ssBohumNew1.ActiveSheet.Cells[45, 1].Text = "시장·군수·구청장 귀하";


                #endregion
            }



        }

        private void toolOP_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strIndex = VB.Right(((ToolStripMenuItem)sender).Name, 1);

            frmBuppatOP frmBuppatOPX = new frmBuppatOP();
            frmBuppatOPX.Print1(txtPtNo.Text, txtSex.Text, txtAge.Text, txtSName.Text, strIndex);

            if (frmBuppatOPX != null)
            {
                frmBuppatOPX = null;
            }

            #region 예전 스크립트 사용안함
            //FarPoint.Win.Spread.FpSpread ssSpread = null;
            //string strIndex = VB.Right(((ToolStripMenuItem)sender).Name, 1);

            //switch (strIndex)
            //{
            //    case "1":
            //        ssSpread = ssOP1;
            //        break;
            //    case "2":
            //        ssSpread = ssOP2;
            //        break;
            //    case "3":
            //        ssSpread = ssOP3;
            //        break;
            //    case "4":
            //        ssSpread = ssOP4;
            //        break;
            //    case "5":
            //        ssSpread = ssOP5;
            //        break;
            //}

            //ssSpread.ActiveSheet.Cells[0, 5].Text = txtPtNo.Text;
            //ssSpread.ActiveSheet.Cells[1, 5].Text = txtSex.Text + "/" + txtAge.Text;
            //ssSpread.ActiveSheet.Cells[2, 5].Text = txtSName.Text;

            //ssSpread.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            //ssSpread.ActiveSheet.PrintInfo.Margin.Top = 60;
            //ssSpread.ActiveSheet.PrintInfo.Margin.Bottom = 20;
            //ssSpread.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            //ssSpread.ActiveSheet.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            //ssSpread.ActiveSheet.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            //ssSpread.ActiveSheet.PrintInfo.ShowBorder = false;
            //ssSpread.ActiveSheet.PrintInfo.ShowColor = false;
            //ssSpread.ActiveSheet.PrintInfo.ShowGrid = true;
            //ssSpread.ActiveSheet.PrintInfo.ShowShadows = false;
            //ssSpread.ActiveSheet.PrintInfo.UseMax = false;
            //ssSpread.ActiveSheet.PrintInfo.UseSmartPrint = false;
            //ssSpread.ActiveSheet.PrintInfo.ShowPrintDialog = false;
            //ssSpread.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            //ssSpread.ActiveSheet.PrintInfo.Preview = false;
            //ssSpread.PrintSheet(0); 
            #endregion
        }

        private void SetDrSign(FarPoint.Win.Spread.FpSpread spd, int row, int Col, string sabun)
        {
            Image ImageX = GetDrSign(clsDB.DbCon, sabun, "");
            FarPoint.Win.Spread.CellType.TextCellType cellType = new FarPoint.Win.Spread.CellType.TextCellType();
            cellType.BackgroundImage = new FarPoint.Win.Picture(ImageX, FarPoint.Win.RenderStyle.Stretch);
            spd.ActiveSheet.Cells[row, Col].CellType = cellType;

            ImageX = null;
            cellType = null;
        }

        private Image GetDrSign(PsmhDb pDbCon, string strSabun, string strgubun)
        {
            Image rtnVAL = null;

            if (string.IsNullOrEmpty(strSabun)) return rtnVAL;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "SIGNATURE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE TRIM(drcode) = '" + strSabun + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return rtnVAL;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVAL;
                }

                if (dt.Rows[0]["SIGNATURE"] == DBNull.Value)
                {
                    return rtnVAL;
                }

                using (MemoryStream memStream = new MemoryStream((byte[])dt.Rows[0]["SIGNATURE"]))
                {
                    rtnVAL = Image.FromStream(memStream);
                }

                return rtnVAL;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVAL;
            }
        }

        private void toolBst1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("정말로 당뇨 등록신청서를 인쇄를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            //string strFile = "";
            string strSabun = "";
            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            string strDept = "";
            string strDr = "";
            string strDrLic = "";

            strDept = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDeptNew.Text, 2));

            if (cboDr.Text.Trim() != "")
            {
                if (cboDr.Text.Replace(".", "").Trim() != "")
                {
                    strSabun = VB.Left(cboDr.Text, 4);

                    strDr = VB.Split(cboDr.Text, ".")[1] + "(" + clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0]) + ")";
                    strDrLic = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDeptNew.Text, 2)) + "(" + GetInsaCertNo(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0], VB.Split(cboDept.Text, ".")[0]) + ")";
                }
                else
                {
                    strDr = VB.Split(cboDr.Text, ".")[1] + "(" + VB.Space(5) + ")";
                    strDrLic = "";
                }
            }

            strDept = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDeptNew.Text, 2));
            
            frmBupPatIns frmBupPatInsX = new frmBupPatIns();
            frmBupPatInsX.Print1(txtSName.Text.Trim(), txtJumin.Text.Trim(), txtTel.Text.Trim(),
                                 txtHPhone.Text.Trim(), strDept, strDr, strDrLic, txtPtNo.Text, txtJuso.Text, strSabun);

            if (frmBupPatInsX != null)
            {
                frmBupPatInsX = null;
            }
        }

        private void toolBst2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("당뇨 소모성 재료 처방전을 인쇄를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string strSabun = "";
            string strDept = "";
            string strDr = "";
            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            strDept = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2));

            if (cboDr.Text.Trim() != "")
            {
                if (cboDr.Text.Replace(".", "") != "")
                {
                    strSabun = VB.Left(cboDr.Text, 4);
                    strDr = VB.Split(cboDr.Text, ".")[1] + "(" + clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0]) + ")";
                }
                else
                {
                    strDr = VB.Split(cboDr.Text, ".")[1] + "(" + VB.Space(5) + ")";
                }
            }

            frmBupPatIns frmBupPatInsX = new frmBupPatIns();
            frmBupPatInsX.Print2(txtGKiho.Text + GstrE000, txtJumin.Text, txtSName.Text, txtTel.Text, txtHPhone.Text, strDept, strDr, strSabun);
            
            if (frmBupPatInsX != null)
            { frmBupPatInsX = null; }

        }


        #region 당뇨인쇄-2 사용안함(2019-03-13)

        //private void toolBst2_Click_OLD(object sender, EventArgs e)
        //{
        //    if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

        //    if (ComFunc.MsgBoxQ("당뇨 소모성 재료 처방전을 인쇄를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
        //    {
        //        return;
        //    }

        //    string strFile = "";
        //    string strSabun = "";
        //    string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

        //    ssDiabetes2_Sheet1.Cells[5, 3].Text = "";
        //    ssDiabetes2_Sheet1.Cells[5, 8].Text = "";

        //    ssDiabetes2_Sheet1.Cells[6, 3].Text = "";
        //    ssDiabetes2_Sheet1.Cells[6, 9].Text = "";

        //    ssDiabetes2_Sheet1.Cells[7, 9].Text = "";

        //    ssDiabetes2_Sheet1.Cells[5, 3].Text = txtGKiho.Text;
        //    ssDiabetes2_Sheet1.Cells[5, 8].Text = txtJumin.Text;

        //    ssDiabetes2_Sheet1.Cells[6, 3].Text = txtSName.Text;
        //    ssDiabetes2_Sheet1.Cells[6, 9].Text = txtTel.Text;

        //    ssDiabetes2_Sheet1.Cells[7, 9].Text = txtHPhone.Text;

        //    ssDiabetes2_Sheet1.Cells[9, 2].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2));

        //    ssDiabetes2_Sheet1.Cells[27, 4].Text = Convert.ToDateTime(strDate).ToString("yyyy 년 MM 월 dd 일");

        //    if (cboDr.Text.Trim() != "")
        //    {
        //        if (cboDr.Text.Replace(".", "") != "")
        //        {
        //            strSabun = VB.Left(cboDr.Text, 4);
        //            ssDiabetes2_Sheet1.Cells[29, 4].Text = VB.Split(cboDr.Text, ".")[1] + "(" + clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0]) + ")";
        //        }
        //        else
        //        {
        //            ssDiabetes2_Sheet1.Cells[29, 4].Text = VB.Split(cboDr.Text, ".")[1] + "(" + VB.Space(5) + ")";
        //        }

        //        //ssDiabetes2_Sheet1.Cells[30, 4].Text = VB.Split(cboDr.Text, ".")[0];
        //    }

        //    //의사 사인 이미지 로드 ------------------------------------------------------------------------------------
        //    if (strSabun != "")
        //    {
        //        FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
        //        imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

        //        ssDiabetes2_Sheet1.Cells[29, 9].CellType = imgCellType;
        //        ssDiabetes2_Sheet1.Cells[29, 9].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
        //        ssDiabetes2_Sheet1.Cells[29, 9].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

        //        strFile = "C:\\CMC\\" + ComFunc.LPAD(strSabun, 5, "0") + ".jpg";

        //        Image image = SIGNATUREFILE_DBToFile(strSabun);

        //        //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
        //        if (image != null)
        //        {
        //            ssDiabetes2_Sheet1.Cells[29, 9].Value = image;
        //        }
        //    }
        //    else
        //    {
        //        FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
        //        textCellType.WordWrap = false;
        //        textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

        //        ssDiabetes2_Sheet1.Cells[29, 9].CellType = textCellType;
        //        ssDiabetes2_Sheet1.Cells[29, 9].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
        //        ssDiabetes2_Sheet1.Cells[29, 9].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //        ssDiabetes2_Sheet1.Cells[29, 9].Text = "(서명 또는 인)";
        //    }

        //    ssDiabetes2_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
        //    ssDiabetes2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
        //    ssDiabetes2_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
        //    ssDiabetes2_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
        //    ssDiabetes2_Sheet1.PrintInfo.Margin.Top = 60;
        //    ssDiabetes2_Sheet1.PrintInfo.Margin.Bottom = 20;
        //    ssDiabetes2_Sheet1.PrintInfo.ShowColor = false;
        //    ssDiabetes2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
        //    ssDiabetes2_Sheet1.PrintInfo.ShowBorder = false;
        //    ssDiabetes2_Sheet1.PrintInfo.ShowGrid = true;
        //    ssDiabetes2_Sheet1.PrintInfo.ShowShadows = false;
        //    ssDiabetes2_Sheet1.PrintInfo.UseMax = true;
        //    ssDiabetes2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
        //    ssDiabetes2_Sheet1.PrintInfo.UseSmartPrint = false;
        //    ssDiabetes2_Sheet1.PrintInfo.ShowPrintDialog = false;
        //    ssDiabetes2_Sheet1.PrintInfo.Preview = false;
        //    ssDiabetes2.PrintSheet(0);
        //}


        #endregion


        private void toolBst3_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("당뇨 소모성 재료 처방전을 인쇄를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string strSabun = "";
            string strDept = "";
            string strDr = "";
            string strDrBunho = "";
            
            strDept = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2));

            if (cboDr.Text.Replace(".", "") != "")
            {
                strSabun = VB.Left(cboDr.Text, 4);
                strDr = VB.Split(cboDr.Text, ".")[1].ToString();
                strDrBunho = clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0]);
            }

            frmBupPatIns frmBupPatInsX = new frmBupPatIns();
            frmBupPatInsX.Print3(txtGKiho.Text + GstrE000, txtJumin.Text, txtSName.Text, txtTel.Text,
                                 txtHPhone.Text, txtJuso.Text, strDept, strDr, strDrBunho, strSabun);
            if (frmBupPatInsX != null)
            { frmBupPatInsX = null; }


        }

        #region 당뇨인쇄-3 사용안함(2019-03-13)

        //private void toolBst3_Click_OLD(object sender, EventArgs e)
        //{
        //    if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

        //    if (ComFunc.MsgBoxQ("당뇨 소모성 재료 처방전을 인쇄를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
        //    {
        //        return;
        //    }

        //    string strFile = "";
        //    string strSabun = "";
        //    string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

        //    ssDiabetes3_Sheet1.Cells[6, 3].Text = txtGKiho.Text;

        //    ssDiabetes3_Sheet1.Cells[8, 3].Text = txtSName.Text;
        //    ssDiabetes3_Sheet1.Cells[8, 13].Text = txtJumin.Text;

        //    ssDiabetes3_Sheet1.Cells[9, 3].Text = txtTel.Text;
        //    ssDiabetes3_Sheet1.Cells[9, 13].Text = txtHPhone.Text;

        //    ssDiabetes3_Sheet1.Cells[10, 13].Text = txtJuso.Text;

        //    ssDiabetes3_Sheet1.Cells[11, 1].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2));

        //    ssDiabetes3_Sheet1.Cells[55, 0].Text = Convert.ToDateTime(strDate).ToString("yyyy 년 MM 월 dd 일");

        //    if (cboDr.Text.Replace(".", "") != "")
        //    {
        //        strSabun = VB.Left(cboDr.Text, 4);
        //        ssDiabetes3_Sheet1.Cells[57, 6].Text = VB.Split(cboDr.Text, ".")[1].ToString();
        //        ssDiabetes3_Sheet1.Cells[57, 10].Text = clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0]);
        //    }
        //    else
        //    {
        //        ssDiabetes3_Sheet1.Cells[57, 6].Text = "";
        //        ssDiabetes3_Sheet1.Cells[57, 10].Text = "";
        //    }

        //    //의사 사인 이미지 로드 ------------------------------------------------------------------------------------
        //    if (strSabun != "")
        //    {
        //        FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
        //        imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

        //        ssDiabetes3_Sheet1.Cells[57, 13].CellType = imgCellType;
        //        ssDiabetes3_Sheet1.Cells[57, 13].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
        //        ssDiabetes3_Sheet1.Cells[57, 13].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

        //        strFile = "C:\\CMC\\" + ComFunc.LPAD(strSabun, 5, "0") + ".jpg";

        //        Image image = SIGNATUREFILE_DBToFile(strSabun);

        //        //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
        //        if (image != null)
        //        {
        //            ssDiabetes3_Sheet1.Cells[57, 13].Value = image;
        //        }
        //    }
        //    else
        //    {
        //        FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
        //        textCellType.WordWrap = false;
        //        textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

        //        ssDiabetes3_Sheet1.Cells[57, 13].CellType = textCellType;
        //        ssDiabetes3_Sheet1.Cells[57, 13].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
        //        ssDiabetes3_Sheet1.Cells[57, 13].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //        ssDiabetes3_Sheet1.Cells[57, 13].Text = "(서명 또는 인)";
        //    }

        //    ssDiabetes3_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
        //    ssDiabetes3_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
        //    ssDiabetes3_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
        //    ssDiabetes3_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
        //    ssDiabetes3_Sheet1.PrintInfo.Margin.Top = 60;
        //    ssDiabetes3_Sheet1.PrintInfo.Margin.Bottom = 20;
        //    ssDiabetes3_Sheet1.PrintInfo.ShowColor = false;
        //    ssDiabetes3_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
        //    ssDiabetes3_Sheet1.PrintInfo.ShowBorder = false;
        //    ssDiabetes3_Sheet1.PrintInfo.ShowGrid = true;
        //    ssDiabetes3_Sheet1.PrintInfo.ShowShadows = false;
        //    ssDiabetes3_Sheet1.PrintInfo.UseMax = true;
        //    ssDiabetes3_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
        //    ssDiabetes3_Sheet1.PrintInfo.UseSmartPrint = false;
        //    ssDiabetes3_Sheet1.PrintInfo.ShowPrintDialog = false;
        //    ssDiabetes3_Sheet1.PrintInfo.Preview = false;
        //    ssDiabetes3.PrintSheet(0);
        //}


        #endregion

        private void toolBst4_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("당뇨 소모성 재료 처방전을 인쇄를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string strSabun = "";
            string strDr = "";
            string strDept = "";
            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            strDept = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2));  //진료과

            if (cboDr.Text.Replace(".", "") != "")
            {
                strSabun = VB.Left(cboDr.Text, 4);
                strDr = VB.Split(cboDr.Text, ".")[1].ToString() + 
                                                      "(" + clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0]) + ")";

                strDept = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2)) +
                                                 "(" + GetInsaCertNo(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0], VB.Left(cboDept.Text, 2)) + ")";
            }

            frmBupPatIns frmBupPatInsX = new frmBupPatIns();
            //frmBupPatInsX.Print4(txtGKiho.Text + GstrE000, txtJumin.Text, txtSName.Text, txtTel.Text, txtHPhone.Text, strDept, strDr, strSabun);
            frmBupPatInsX.Print4_20201210(txtGKiho.Text + GstrE000, txtJumin.Text, txtSName.Text, txtTel.Text, txtHPhone.Text, strDept, strDr, strSabun);
            if (frmBupPatInsX != null)
            { frmBupPatInsX = null; }


        }

        private void toolBst5_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("당뇨 소모성 재료 처방전을 인쇄를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string strSabun = "";
            string strDept = "";
            string strDr = "";
            string strDrBunho = "";

            strDept = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2));

            if (cboDr.Text.Replace(".", "") != "")
            {
                strSabun = VB.Left(cboDr.Text, 4);
                strDr = VB.Split(cboDr.Text, ".")[1].ToString();
                strDrBunho = clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0]);
            }

            frmBupPatIns frmBupPatInsX = new frmBupPatIns();
            frmBupPatInsX.Print5(txtGKiho.Text + GstrE000, txtJumin.Text, txtSName.Text, txtTel.Text,
                                 txtHPhone.Text, txtJuso.Text, strDept, strDr, strDrBunho, strSabun);
            if (frmBupPatInsX != null)
            { frmBupPatInsX = null; }



        }

        #region 당뇨인쇄-4 사용안함(2019-03-13)

        //private void toolBst4_Click_OLD(object sender, EventArgs e)
        //{
        //    if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

        //    if (ComFunc.MsgBoxQ("당뇨 소모성 재료 처방전을 인쇄를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
        //    {
        //        return;
        //    }

        //    string strFile = "";
        //    string strSabun = "";
        //    string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");



        //    ssDiabetes4_Sheet1.Cells[8, 3].Text = txtGKiho.Text;        //건강보험증번호
        //    ssDiabetes4_Sheet1.Cells[8, 7].Text = txtJumin.Text;        //주민번호

        //    ssDiabetes4_Sheet1.Cells[9, 3].Text = txtSName.Text;        //환자성명
        //    ssDiabetes4_Sheet1.Cells[9, 7].Text = txtTel.Text;          //자택 전화번호
        //    ssDiabetes4_Sheet1.Cells[10, 7].Text = txtHPhone.Text;      //휴대전화

        //    ssDiabetes4_Sheet1.Cells[14, 2].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2));  //진료과

        //    ssDiabetes4_Sheet1.Cells[35, 1].Text = Convert.ToDateTime(strDate).ToString("yyyy 년 MM 월 dd 일");
        //    //ssDiabetes4_Sheet1.Cells[10, 13].Text = txtJuso.Text;



        //    if (cboDr.Text.Replace(".", "") != "")
        //    {
        //        strSabun = VB.Left(cboDr.Text, 4);
        //        ssDiabetes4_Sheet1.Cells[37, 4].Text = VB.Split(cboDr.Text, ".")[1].ToString() +
        //                                              "(" + clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0]) + ")";

        //        ssDiabetes4_Sheet1.Cells[38, 4].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2)) +
        //                                         "(" + GetInsaCertNo(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0], VB.Left(cboDept.Text, 2)) + ")";
        //    }
        //    else
        //    {
        //        ssDiabetes4_Sheet1.Cells[37, 4].Text = "";
        //        ssDiabetes4_Sheet1.Cells[38, 4].Text = "";
        //    }

        //    //의사 사인 이미지 로드 ------------------------------------------------------------------------------------
        //    if (strSabun != "")
        //    {
        //        FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
        //        imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

        //        ssDiabetes4_Sheet1.Cells[37, 5].CellType = imgCellType;
        //        ssDiabetes4_Sheet1.Cells[37, 5].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
        //        ssDiabetes4_Sheet1.Cells[37, 5].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

        //        strFile = "C:\\CMC\\" + ComFunc.LPAD(strSabun, 5, "0") + ".jpg";

        //        Image image = SIGNATUREFILE_DBToFile(strSabun);

        //        //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
        //        if (image != null)
        //        {
        //            ssDiabetes4_Sheet1.Cells[37, 8].Value = image;
        //        }
        //    }
        //    else
        //    {
        //        FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
        //        textCellType.WordWrap = false;
        //        textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

        //        ssDiabetes4_Sheet1.Cells[37, 5].CellType = textCellType;
        //        ssDiabetes4_Sheet1.Cells[37, 5].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
        //        ssDiabetes4_Sheet1.Cells[37, 5].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        //        ssDiabetes4_Sheet1.Cells[37, 5].Text = "(서명 또는 인)";
        //    }

        //    ssDiabetes4_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
        //    ssDiabetes4_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
        //    ssDiabetes4_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
        //    ssDiabetes4_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
        //    ssDiabetes4_Sheet1.PrintInfo.Margin.Top = 60;
        //    ssDiabetes4_Sheet1.PrintInfo.Margin.Bottom = 20;
        //    ssDiabetes4_Sheet1.PrintInfo.ShowColor = false;
        //    ssDiabetes4_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
        //    ssDiabetes4_Sheet1.PrintInfo.ShowBorder = false;
        //    ssDiabetes4_Sheet1.PrintInfo.ShowGrid = true;
        //    ssDiabetes4_Sheet1.PrintInfo.ShowShadows = false;
        //    ssDiabetes4_Sheet1.PrintInfo.UseMax = true;
        //    ssDiabetes4_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
        //    ssDiabetes4_Sheet1.PrintInfo.UseSmartPrint = false;
        //    ssDiabetes4_Sheet1.PrintInfo.ShowPrintDialog = false;
        //    ssDiabetes4_Sheet1.PrintInfo.Preview = false;
        //    ssDiabetes4.PrintSheet(0);
        //}


        #endregion

        private void toolDent_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            frmBuppatDent frmBuppatDentX = new frmBuppatDent();
            frmBuppatDentX.PrintDent1(VB.Right(((ToolStripMenuItem)sender).Name, 1), txtPtNo.Text.Trim(), dtpSDate.Value.ToString("yyyy-MM-dd"), txtSName.Text.Trim(),
                                      VB.Mid(txtJumin.Text, 1, 6) + "-" + VB.Mid(txtJumin.Text, 7, 7), txtGKiho.Text.Trim(), txtHPhone.Text.Trim(), txtTel.Text.Trim(),
                                      txtJuso.Text.Trim(), txtIllName.Text.Trim(), txtILLCode.Text.Trim(), GstrBi);

            if (frmBuppatDentX != null)
            { frmBuppatDentX = null; }


            #region 사용안함
            //string strIndex = VB.Right(((ToolStripMenuItem)sender).Name, 1);

            //string strPtNo = "";        //등록번호
            //string strDtpSDate = "";    //접수일자(공단신청일
            //string strSname = "";       //성명(수진자, 수급권자)
            //string strPJumin = "";      //주민등록번호
            //string strGKiho = "";       //건강보험증번호
            //string strHPhone = "";      //휴대전화
            //string strTel = "";         //자택전화
            //string strJuso = "";        //주소
            //string strIllName = "";     //상병명
            //string strIllCode = "";     //상병코드

            //string strCHAR1 = "";
            //string strCHAR2 = "";
            //string strCHAR3 = "";
            //string strCHAR4 = "";
            //string strCHAR5 = "";

            //strPtNo = txtPtNo.Text.Trim();
            //strDtpSDate = dtpSDate.Value.ToString("yyyy-MM-dd");
            //strSname = txtSName.Text.Trim();
            //strPJumin = VB.Mid(txtJumin.Text, 1, 6) + "-" + VB.Mid(txtJumin.Text, 7, 7);
            //strGKiho = txtGKiho.Text.Trim();
            //strHPhone = txtHPhone.Text.Trim();
            //strTel = txtTel.Text.Trim();
            //strJuso = txtJuso.Text.Trim();

            //switch (strIndex)
            //{
            //    case "1":
            //        strIllName = "사고, 발치 또는 국한성 치주병에 의한 치아상실";
            //        strIllCode = "K08.1";

            //        strCHAR1 = "건강보험";
            //        strCHAR2 = "수진자";
            //        strCHAR3 = "국민건강보험공단 이사장 귀하";
            //        strCHAR4 = "건강보험증번호";
            //        strCHAR5 = "②" + ComNum.VBLF + "요양기관" + ComNum.VBLF + "확인란";
            //        break;
            //    case "2":
            //        strIllName = txtIllName.Text.Trim();
            //        strIllCode = txtILLCode.Text.Trim();

            //        strCHAR1 = "의료급여";
            //        strCHAR2 = "수급권자";
            //        strCHAR3 = "시장·군수·구청장 귀하";
            //        strCHAR4 = "종별";
            //        strCHAR5 = "②" + ComNum.VBLF + "의료급여기관" + ComNum.VBLF + "확인란";
            //        break;
            //}

            ////필요없음
            ////Denture_Clear();



            //ssDenture_Sheet1.Cells[5, 0].BackColor = Color.FromArgb(222, 222, 222);
            //ssDenture_Sheet1.Cells[4, 6].BackColor = Color.FromArgb(222, 222, 222);
            //ssDenture_Sheet1.Cells[5, 6].BackColor = Color.FromArgb(222, 222, 222);
            //ssDenture_Sheet1.Cells[4, 25].BackColor = Color.FromArgb(222, 222, 222);

            //if (strIndex == "1") { ssDenture_Sheet1.Cells[6, 28].BackColor = Color.FromArgb(222, 222, 222); }

            //ssDenture_Sheet1.Cells[1, 0].Text = strCHAR1 + " 완전틀니 대상자 등록 신청서";
            //ssDenture_Sheet1.Cells[6, 0].Text = "①" + strCHAR2;

            //if (strIndex == "1") { ssDenture_Sheet1.Cells[9, 0].Text = "②" + ComNum.VBLF + "요양기관" + ComNum.VBLF + "확인란"; }
            //else if (strIndex == "2") { ssDenture_Sheet1.Cells[9, 0].Text = strCHAR5; }

            //ssDenture_Sheet1.Cells[6, 22].Text = strCHAR4;
            //ssDenture_Sheet1.Cells[18, 0].Text = "위와 같이 " + strCHAR1 + " 틀니 대상자 등록을 신청합니다.";
            //ssDenture_Sheet1.Cells[21, 0].Text = strCHAR2 + "와의 관계";
            //ssDenture_Sheet1.Cells[23, 0].Text = strCHAR3;
            //ssDenture_Sheet1.Cells[36, 0].Text = strCHAR3;

            //ssDenture_Sheet1.Cells[25, 1].Text = "본인은 " + strCHAR1 + " 틀니 대상자로 신청하거나 대상자로 선정·등록된 자로, 개인정보보호법 제15조1항제1호";
            //ssDenture_Sheet1.Cells[26, 1].Text = "규정에 의거하여 본인의 개인정보1)를 처리할 것을 동의합니다.";
            //ssDenture_Sheet1.Cells[27, 1].Text = "본인은 " + strCHAR1 + " 틀니 대상자로 신청하거나 대상자로 선정·등록된 자로, 개인정보보호법 제23조제1호";
            //ssDenture_Sheet1.Cells[28, 1].Text = "규정에 의거하여 본인의 민감정보2)를 처리할 것을 동의합니다.";
            //ssDenture_Sheet1.Cells[29, 1].Text = "본인은 " + strCHAR1 + " 틀니 대상자로 신청하거나 대상자로 선정·등록된 자로, 개인정보보호법 제24조제1항제1호";
            //ssDenture_Sheet1.Cells[30, 1].Text = "규정에 의거하여 본인의 고유식별정보3)를 처리할 것을 동의합니다.";

            //switch (strIndex)
            //{
            //    case "1":
            //        ssDenture_Sheet1.Cells[31, 0].Text = "";
            //        ssDenture_Sheet1.Cells[31, 1].Text = "";
            //        ssDenture_Sheet1.Cells[32, 1].Text = "";
            //        ssDenture_Sheet1.Cells[32, 25].Text = "";
            //        ssDenture_Sheet1.Cells[32, 29].Text = "";
            //        break;
            //    case "2":
            //        ssDenture_Sheet1.Cells[31, 0].Text = "○";
            //        ssDenture_Sheet1.Cells[31, 1].Text = "본인은 " + strCHAR1 + " 틀니 대상자로 신청하거나 대상자로 선정·등록된 자로, 개인정보보호법 제17조제1항제2호";
            //        ssDenture_Sheet1.Cells[32, 1].Text = "규정에 의거하여 본인의 개인정보1)를 제3자에게 제공할 것을 동의합니다.";
            //        ssDenture_Sheet1.Cells[32, 25].Text = "□ 동의함";
            //        ssDenture_Sheet1.Cells[32, 29].Text = "□ 동의하지 않음";
            //        break;
            //}

            //ssDenture_Sheet1.Cells[35, 0].Text = "④ " + strCHAR2 + " 본인";

            //if (strIndex == "1") { ssDenture_Sheet1.Cells[5, 0].Text = strPtNo; }   //등록번호

            //ssDenture_Sheet1.Cells[6, 6].Text = strSname;       //성명
            //ssDenture_Sheet1.Cells[6, 16].Text = strPJumin;       //주민등록번호

            //if (strIndex == "2")
            //{
            //    if (GstrBi == "21")
            //    {
            //        ssDenture_Sheet1.Cells[6, 28].Text = "의료급여1종";
            //    }
            //    else if (GstrBi == "22")
            //    {
            //        ssDenture_Sheet1.Cells[6, 28].Text = "의료급여2종";
            //    }
            //}

            //ssDenture_Sheet1.Cells[7, 6].Text = strJuso;        //주소

            //ssDenture_Sheet1.Cells[8, 8].Text = strHPhone;      //휴대전화
            //ssDenture_Sheet1.Cells[8, 18].Text = strTel;        //자택번호

            //ssDenture_Sheet1.Cells[9, 8].Text = strIllName;     //상병명
            //ssDenture_Sheet1.Cells[9, 27].Text = strIllCode;    //상병코드

            //switch (strIndex)
            //{
            //    case "1":
            //        ssDenture_Sheet1.Cells[16, 4].Text = "요양기관명(기호)";
            //        break;
            //    case "2":
            //        ssDenture_Sheet1.Cells[16, 4].Text = "의료급여기관명(기호)";
            //        break;
            //}

            //ssDenture_Sheet1.Cells[16, 16].Text = "포항성모병원";
            //ssDenture_Sheet1.Cells[16, 24].Text = "37100068";

            //ssDenture_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            //ssDenture_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            //ssDenture_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            //ssDenture_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            //ssDenture_Sheet1.PrintInfo.Margin.Top = 60;
            //ssDenture_Sheet1.PrintInfo.Margin.Bottom = 20;
            //ssDenture_Sheet1.PrintInfo.ShowColor = true;
            //ssDenture_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            //ssDenture_Sheet1.PrintInfo.ShowBorder = false;
            //ssDenture_Sheet1.PrintInfo.ShowGrid = false;
            //ssDenture_Sheet1.PrintInfo.ShowShadows = false;
            //ssDenture_Sheet1.PrintInfo.UseMax = false;
            //ssDenture_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            //ssDenture_Sheet1.PrintInfo.UseSmartPrint = false;
            //ssDenture_Sheet1.PrintInfo.ShowPrintDialog = false;
            //ssDenture_Sheet1.PrintInfo.Preview = false;
            //ssDenture.PrintSheet(0); 
            #endregion
        }

        private void toolDent3_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            frmBuppatDent frmBuppatDentX = new frmBuppatDent();
            frmBuppatDentX.PrintDent2(VB.Right(((ToolStripMenuItem)sender).Name, 1), txtPtNo.Text.Trim(), dtpSDate.Value.ToString("yyyy-MM-dd"), txtSName.Text.Trim(),
                                      VB.Mid(txtJumin.Text, 1, 6) + "-" + VB.Mid(txtJumin.Text, 7, 7), txtGKiho.Text.Trim(), txtHPhone.Text.Trim(), txtTel.Text.Trim(),
                                      txtJuso.Text.Trim(), GstrBi, GstrE000);

            if (frmBuppatDentX != null)
            { frmBuppatDentX = null; }


            #region 사용안함
            //string strIndex = VB.Right(((ToolStripMenuItem)sender).Name, 1);

            //FarPoint.Win.Spread.FpSpread ssSpread = null;

            //string strPtNo = "";        //등록번호
            //string strDtpSDate = "";    //접수일자(공단신청일
            //string strSname = "";       //성명(수진자, 수급권자)
            //string strPJumin = "";      //주민등록번호
            //string strGKiho = "";       //건강보험증번호
            //string strHPhone = "";      //휴대전화
            //string strTel = "";         //자택전화
            //string strJuso = "";        //주소

            //strPtNo = txtPtNo.Text.Trim();
            //strDtpSDate = dtpSDate.Value.ToString("yyyy-MM-dd");
            //strSname = txtSName.Text.Trim();
            //strPJumin = VB.Mid(txtJumin.Text, 1, 6) + "-" + VB.Mid(txtJumin.Text, 7, 7);
            //strGKiho = txtGKiho.Text.Trim();
            //strHPhone = txtHPhone.Text.Trim();
            //strTel = txtTel.Text.Trim();
            //strJuso = txtJuso.Text.Trim();

            //if (strIndex == "3") { ssSpread = ssImplant1; } else if (strIndex == "4") { ssSpread = ssImplant2; }

            //if (ssSpread == ssImplant1)
            //{
            //    ssSpread.ActiveSheet.Cells[4, 4].Text = "";     //등록번호

            //    ssSpread.ActiveSheet.Cells[5, 6].Text = "";     //성명
            //    ssSpread.ActiveSheet.Cells[5, 16].Text = "";    //주민등록번호
            //    ssSpread.ActiveSheet.Cells[5, 28].Text = "";    //건강보험증번호

            //    ssSpread.ActiveSheet.Cells[6, 6].Text = "";     //주소
            //    ssSpread.ActiveSheet.Cells[6, 28].Text = "";    //휴대전화

            //    ssSpread.ActiveSheet.Cells[7, 8].Text = "";     //자택번호

            //    ssSpread.ActiveSheet.Cells[19, 16].Text = "37100068";
            //    ssSpread.ActiveSheet.Cells[19, 24].Text = "포항성모병원";
            //}
            //else if (ssSpread == ssImplant2)
            //{
            //    ssSpread.ActiveSheet.Cells[4, 5].Text = "";     //등록번호

            //    ssSpread.ActiveSheet.Cells[5, 7].Text = "";     //성명
            //    ssSpread.ActiveSheet.Cells[5, 17].Text = "";    //주민등록번호
            //    ssSpread.ActiveSheet.Cells[5, 28].Text = "";

            //    ssSpread.ActiveSheet.Cells[6, 7].Text = "";     //주소
            //    ssSpread.ActiveSheet.Cells[6, 29].Text = "";     //자택번호

            //    ssSpread.ActiveSheet.Cells[18, 17].Text = "37100068";
            //    ssSpread.ActiveSheet.Cells[18, 25].Text = "포항성모병원";
            //}

            //ssSpread.ActiveSheet.Rows[4].BackColor = Color.FromArgb(222, 222, 222);

            //if (ssSpread == ssImplant1)
            //{
            //    ssSpread.ActiveSheet.Cells[4, 4].Text = strPtNo;        //등록번호

            //    ssSpread.ActiveSheet.Cells[5, 6].Text = strSname;       //성명
            //    ssSpread.ActiveSheet.Cells[5, 16].Text = strPJumin;     //주민등록번호
            //    ssSpread.ActiveSheet.Cells[5, 28].Text = strGKiho + GstrE000;      //건강보험증번호

            //    ssSpread.ActiveSheet.Cells[6, 6].Text = strJuso;        //주소
            //    ssSpread.ActiveSheet.Cells[6, 28].Text = strHPhone;     //휴대전화

            //    ssSpread.ActiveSheet.Cells[7, 8].Text = strTel;         //자택번호

            //    ssSpread.ActiveSheet.Cells[19, 16].Text = "37100068";
            //    ssSpread.ActiveSheet.Cells[19, 24].Text = "포항성모병원";
            //}
            //else if (ssSpread == ssImplant2)
            //{
            //    ssSpread.ActiveSheet.Cells[4, 5].Text = strPtNo;        //등록번호

            //    ssSpread.ActiveSheet.Cells[5, 7].Text = strSname;       //성명
            //    ssSpread.ActiveSheet.Cells[5, 17].Text = strPJumin;     //주민등록번호

            //    if (GstrBi == "21") { ssSpread.ActiveSheet.Cells[5, 28].Text = "의료급여1종"; } else if (GstrBi == "22") { ssSpread.ActiveSheet.Cells[5, 28].Text = "의료급여2종"; }

            //    ssSpread.ActiveSheet.Cells[6, 7].Text = strJuso;        //주소
            //    ssSpread.ActiveSheet.Cells[6, 29].Text = strTel;         //자택번호

            //    ssSpread.ActiveSheet.Cells[18, 17].Text = "37100068";
            //    ssSpread.ActiveSheet.Cells[18, 25].Text = "포항성모병원";
            //}

            //ssSpread.ActiveSheet.PrintInfo.AbortMessage = "프린터 중입니다.";
            //ssSpread.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            //ssSpread.ActiveSheet.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            //ssSpread.ActiveSheet.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            //ssSpread.ActiveSheet.PrintInfo.Margin.Top = 60;
            //ssSpread.ActiveSheet.PrintInfo.Margin.Bottom = 20;
            //ssSpread.ActiveSheet.PrintInfo.ShowColor = true;
            //ssSpread.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            //ssSpread.ActiveSheet.PrintInfo.ShowBorder = false;
            //ssSpread.ActiveSheet.PrintInfo.ShowGrid = false;
            //ssSpread.ActiveSheet.PrintInfo.ShowShadows = false;
            //ssSpread.ActiveSheet.PrintInfo.UseMax = false;
            //ssSpread.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            //ssSpread.ActiveSheet.PrintInfo.UseSmartPrint = false;
            //ssSpread.ActiveSheet.PrintInfo.ShowPrintDialog = false;
            //ssSpread.ActiveSheet.PrintInfo.Preview = false;
            //ssSpread.PrintSheet(0); 
            #endregion
        }

        private void toolBreath_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("건강보험 인공호흡기 급여대상자 등록 신청서를 인쇄 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            frmBuppatDent frmBuppatDentX = new frmBuppatDent();
            frmBuppatDentX.PrintRespirator(txtSName.Text, txtJumin.Text, txtGKiho.Text, GstrE000, txtTel.Text, txtHPhone.Text, cboDept.Text, cboDr.Text);

            if (frmBuppatDentX != null)
            { frmBuppatDentX = null; }

            #region 사용안함
            //string strFile = "";
            //string strSabun = "";
            //string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            //ssRespirator_Sheet1.Cells[5, 4].Text = txtSName.Text;       //성명

            //ssRespirator_Sheet1.Cells[6, 8].Text = txtJumin.Text;       //주민번호
            //ssRespirator_Sheet1.Cells[6, 15].Text = txtGKiho.Text + GstrE000;      //건강보험증번호

            //ssRespirator_Sheet1.Cells[7, 7].Text = txtTel.Text;         //전화번호

            //ssRespirator_Sheet1.Cells[8, 7].Text = txtHPhone.Text;      //휴대폰번호

            //ssRespirator_Sheet1.Cells[10, 5].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2));  //진료과목

            //ssRespirator_Sheet1.Cells[35, 3].Text = Convert.ToDateTime(strDate).ToString("yyyy   년        MM   월        dd   일");
            //ssRespirator_Sheet1.Cells[42, 1].Text = Convert.ToDateTime(strDate).ToString("yyyy   년        MM   월        dd   일");

            //if (cboDr.Text.Replace(".", "") != "")
            //{
            //    strSabun = VB.Left(cboDr.Text, 4);
            //    ssRespirator_Sheet1.Cells[37, 10].Text = "  " + VB.Split(cboDr.Text, ".")[1];
            //    ssRespirator_Sheet1.Cells[37, 10].Text = ssRespirator_Sheet1.Cells[37, 10].Text + "    (" + clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0]) + ")";
            //}
            //else
            //{
            //    ssRespirator_Sheet1.Cells[37, 10].Text = "";
            //}

            ////의사 사인 이미지 로드 ------------------------------------------------------------------------------------
            //if (strSabun != "")
            //{
            //    FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
            //    imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

            //    ssRespirator_Sheet1.Cells[37, 17].CellType = imgCellType;
            //    ssRespirator_Sheet1.Cells[37, 17].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            //    ssRespirator_Sheet1.Cells[37, 17].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            //    strFile = "C:\\CMC\\" + ComFunc.LPAD(strSabun, 5, "0") + ".jpg";

            //    Image image = SIGNATUREFILE_DBToFile(strSabun);

            //    //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
            //    if (image != null)
            //    {
            //        ssRespirator_Sheet1.Cells[37, 17].Value = image;
            //    }
            //}
            //else
            //{
            //    FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
            //    textCellType.WordWrap = false;
            //    textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

            //    ssRespirator_Sheet1.Cells[37, 17].CellType = textCellType;
            //    ssRespirator_Sheet1.Cells[37, 17].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            //    ssRespirator_Sheet1.Cells[37, 17].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //    ssRespirator_Sheet1.Cells[37, 17].Text = "(서명 또는 인)";
            //}

            //ssRespirator_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            //ssRespirator_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            //ssRespirator_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            //ssRespirator_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            //ssRespirator_Sheet1.PrintInfo.Margin.Top = 60;
            //ssRespirator_Sheet1.PrintInfo.Margin.Bottom = 20;
            //ssRespirator_Sheet1.PrintInfo.ShowColor = false;
            //ssRespirator_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            //ssRespirator_Sheet1.PrintInfo.ShowBorder = false;
            //ssRespirator_Sheet1.PrintInfo.ShowGrid = true;
            //ssRespirator_Sheet1.PrintInfo.ShowShadows = false;
            //ssRespirator_Sheet1.PrintInfo.UseMax = true;
            //ssRespirator_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            //ssRespirator_Sheet1.PrintInfo.UseSmartPrint = false;
            //ssRespirator_Sheet1.PrintInfo.ShowPrintDialog = false;
            //ssRespirator_Sheet1.PrintInfo.Preview = false;
            //ssRespirator.PrintSheet(0); 
            #endregion
        }

        private void 치과임플란트ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void optNew_CheckedChanged(object sender, EventArgs e)
        {
            this.Height = 918;
            grbTongbo.Text = "등록결과 통보방법";
            rdoTongbo0.Text = "알림톡";
            rdoTongbo1.Text = "이메일";

            rdo7.Checked = true;
            //rdo8.Checked = true;
            panel27.Visible = true;    // 버튼 옆 콤보박스
            cboGubun.Visible = true;
            dtpIO.Enabled = false;
            panel21.Enabled = false;
            panel25.Enabled = false;
            panel27.Visible = true;
            

            #region 요양기관 확인란
            PanChkOld.Visible = false;
            PanChkNew.Visible = true;

            PanChkOld.Dock = DockStyle.None;
            PanChkNew.Dock = DockStyle.Fill;
            #endregion

            #region 최초진단 or 최종확진방법
            panel17.Height = 301;

            PanExamOld.Visible = false;
            PanExamNew.Visible = true;

            PanExamOld.Dock = DockStyle.None;
            PanExamNew.Dock = DockStyle.Fill;
            #endregion

            label15.Text = "최종확진방법";

        }

        private void cboGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdo9.Checked == true)
            {
                if (cboGubun.Text == "결핵")
                {
                    chkRDExam.Text = "1.영상검사";
                    chkJojic.Visible = false;
                    chkCytoExam.Visible = false;
                    chkCT.Text = "CT";
                    txtCT.Visible = false;

                    PanCancer.Visible = false;
                    Panrare.Visible = false;
                    Pantuber.Visible = true;
                    PanFire.Visible = false;
                    PanAll.Visible = true;
                    PanFire.Dock = DockStyle.None;

                    PanCancer.Dock = DockStyle.None;
                    Panrare.Dock = DockStyle.None;
                    Pantuber.Dock = DockStyle.Fill;

                    panel19.Enabled = false;
                    cboGubun.Visible = true;
                }
                else if (cboGubun.Text == "잠복결핵")
                {
                    
                    chkJojic.Visible = false;
                    chkCytoExam.Visible = false;
                    txtCT.Visible = false;
                    PanAll.Visible = false;
                    PanCancer.Visible = false;
                    Panrare.Visible = false;
                    Pantuber.Visible = false;
                    PantuberS.Visible = true;
                    PanFire.Visible = false;
                    Pantuber.Dock = DockStyle.None;
                    PantuberS.Dock = DockStyle.Fill;

                    panel19.Enabled = false;
                    cboGubun.Visible = true;
                }
                else
                {
                    PanCancer.Visible = false;
                    Panrare.Visible = false;
                    Pantuber.Visible = false;
                    PantuberS.Visible = false;
                    PanFire.Visible = true;
                    PanAll.Visible = false;

                    PanCancer.Dock = DockStyle.None;
                    Panrare.Dock = DockStyle.None;
                    Pantuber.Dock = DockStyle.None;
                    PantuberS.Dock = DockStyle.None;
                    PanFire.Dock = DockStyle.Fill;

                    panel19.Enabled = false;
                    cboGubun.Visible = true;
                }
            }

            read_patinfo();
        }

        private void toolDelete_Click(object sender, EventArgs e)
        {

        }

        private void txtIllCodeNew_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)13)
            {
                if (txtIllCodeNew.Text.Trim() != "")
                {
                    //txtIllNameNew.Text = Read_Bas_ILL(txtIllCodeNew.Text.Trim());
                    SetIllsListBox(txtIllCodeNew.Text.Trim());
                    //txtGiho.Text = Read_Bas_IllsToVCode(txtIllCodeNew.Text.Trim());
                }
            }

        }

        private string Read_Bas_IllsToVCode(string argIlls)
        {
            string rtnVal = "";

            string SQL = "";
            string SqlErr = "";

            DataTable dt = null;

            SQL = " SELECT ILLCODE, VCODE FROM( ";
            SQL += ComNum.VBLF + "  SELECT REPLACE(REPLACE(ILLCODE, '.', ''), '+', '') ILLCODE, VCODE ";
            SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_ILLS_H3 ";
            SQL += ComNum.VBLF + " UNION ALL ";
            SQL += ComNum.VBLF + " SELECT REPLACE(REPLACE(ILLCODE, '.', ''), '+', '') ILLCODE, 'V193' VCODE ";
            SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_ILLS_CANCER) ";
            SQL += ComNum.VBLF + "   WHERE ILLCODE = '" + argIlls + "' ";
            SQL += ComNum.VBLF + "     AND ROWNUM = 1";
            SQL += ComNum.VBLF + " GROUP BY ILLCODE, VCODE ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["VCODE"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        private void btnIllsCancer_Click(object sender, EventArgs e)
        {
            frmILLHCodeCancer frm = new frmILLHCodeCancer();
            frm.Show();
        }

        private void txtIllCodeNew_DoubleClick(object sender, EventArgs e)
        {

            string strGbn = "";
            string strIlls = "";

            strIlls = txtIllCodeNew.Text.Trim();


            if (strGbn == "1")
            {
                
                frmILLHCode frm = new frmILLHCode(strIlls);
                frm.Show();
            }
            else if (strGbn == "2")
            {
                frmILLHCodeCancer frm = new frmILLHCodeCancer();
                frm.Show();
            }

        }

        private void txtIllCodeNew_TextChanged(object sender, EventArgs e)
        {

        }

        private void lstIllnamek_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int selectedIndex = -1;

            Point point = e.Location;
            selectedIndex = lstIllnamek.IndexFromPoint(point);
            if(selectedIndex != -1)
            {
                string strData = lstIllnamek.Items[selectedIndex] as string;
                txtIllCodeNew.Text = VB.Left(strData, 12).Trim();
                //txtIllNameNew.Text = VB.Mid(strData, 12, VB.InStr(strData, " (") - 12).Trim();
                txtIllNameNew.Text = VB.Mid(strData, 12, VB.Len(strData) - 20).Trim();
                //txtGiho.Text = VB.Mid(strData, VB.InStr(strData, " (") + 2, 4).Trim();
                txtGiho.Text = VB.Right(strData, 7).Trim().Replace("(","").Replace(")","");
                grpIlls.Visible = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            grpIlls.Visible = false;
        }

        private void tEST당뇨신청서ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBupPatInsInput frmBupPatInsInputX = new frmBupPatInsInput(txtPtNo.Text.Trim(), VB.Left(cboDept.Text, 2), VB.Left(cboDr.Text, 4));
            frmBupPatInsInputX.ShowDialog();
            
            if (frmBupPatInsInputX != null)
            { frmBupPatInsInputX = null; }
        }

    }
}
