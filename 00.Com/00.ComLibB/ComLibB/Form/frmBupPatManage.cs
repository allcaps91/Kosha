using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB.dll
    /// File Name       : frmBupPatManage.cs
    /// Description     : 건강보험(희귀난치/중증화상), 중증암 환자 관리
    /// Author          : 이정현
    /// Create Date     : 2017-06-17
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// frmBupPatManage.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\buppat\Frm희귀난치환자관리.frm
    /// VB\basic\buppat\Frm중증화상환자관리.frm
    /// VB\basic\buppat\FrmCancer.frm
    /// </seealso>
    /// <vbp>
    /// default : VB\basic\buppat\buppat.vbp
    /// </vbp>
    public partial class frmBupPatManage : Form
    {
        private frmNHICCancer frmNHICCancerEvent = null;

        private int GintGubun = 0;
        private string GstrPtNo = "";
        private string GstrRowID = "";
        private string GstrSabun = "";

        /// <summary>
        /// BUPPAT.VBP : 환자관리 통합 폼
        /// 0 : 건강보험 희귀난치성 환자 관리
        /// 1 : 건강보험 중증화상 환자 관리
        /// 2 : 중증(암) 환자 관리
        /// </summary>
        /// 

        private void setEvent()
        {
            this.dtpIDate.ValueChanged += new System.EventHandler(this.edtpValueChanged);
            this.dtpSDate.ValueChanged += new System.EventHandler(this.edtpValueChanged);
            this.dtpPDate.ValueChanged += new System.EventHandler(this.edtpValueChanged);
            this.dtpFDate.ValueChanged += new System.EventHandler(this.edtpValueChanged);
            this.dtpTDate.ValueChanged += new System.EventHandler(this.edtpValueChanged);
            this.dtpCanDate.ValueChanged += new System.EventHandler(this.edtpValueChanged);
            this.dtpDelDate.ValueChanged += new System.EventHandler(this.edtpValueChanged);



            this.dtpIDate.KeyDown += new KeyEventHandler(edtpKeyDown);
            this.dtpSDate.KeyDown += new KeyEventHandler(edtpKeyDown);
            this.dtpPDate.KeyDown += new KeyEventHandler(edtpKeyDown);
            this.dtpFDate.KeyDown += new KeyEventHandler(edtpKeyDown);
            this.dtpTDate.KeyDown += new KeyEventHandler(edtpKeyDown);
            this.dtpCanDate.KeyDown += new KeyEventHandler(edtpKeyDown);
            this.dtpDelDate.KeyDown += new KeyEventHandler(edtpKeyDown);

            
            this.dtpIDate.KeyPress += new KeyPressEventHandler(edtpKeyPress);
            this.dtpSDate.KeyPress += new KeyPressEventHandler(edtpKeyPress);
            this.dtpPDate.KeyPress += new KeyPressEventHandler(edtpKeyPress);
            this.dtpFDate.KeyPress += new KeyPressEventHandler(edtpKeyPress);
            this.dtpTDate.KeyPress += new KeyPressEventHandler(edtpKeyPress);
            this.dtpCanDate.KeyPress += new KeyPressEventHandler(edtpKeyPress);
            this.dtpDelDate.KeyPress += new KeyPressEventHandler(edtpKeyPress);

        }
        
        public frmBupPatManage()
        {
            InitializeComponent();
            setEvent();
        }

        /// <summary>
        /// 환자관리 기본셋팅
        /// 등록번호, 폼로드 구분자(0/1/2)
        /// 0 : 건강보험 희귀난치성 환자 관리
        /// 1 : 건강보험 중증화상 환자 관리
        /// 2 : 중증(암) 환자 관리
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="intGubun"></param>
        public frmBupPatManage(string strPtNo, int intGubun, string strSabun)
        {
            InitializeComponent();
            setEvent();
            GstrPtNo = strPtNo;
            GintGubun = intGubun;
            GstrSabun = strSabun;
        }

        private void frmBupPatManage_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            // ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등



            FormClear();
            SetCbo();

            if (GstrSabun.Trim() == "")
            {
                GstrSabun = clsType.User.IdNumber;
            }

            if (GstrPtNo != "")
            {
                txtSPtNo.Text = GstrPtNo;
                txtSPtNoEnter();
                GetData();
            }
        }

        private void FormClear()
        {
            //왼쪽
            txtSPtNo.Text = "";
            txtSPtName.Text = "";
            GstrRowID = "";
            ssView_Sheet1.RowCount = 0;

            //오른쪽
            txtPtNo.Text = "";
            txtPtName.Text = "";

            cboSex.Text = "";
            //cboSex.Items.Clear();

            txtAge.Text = "";
            

            dtpIDate.CustomFormat = " ";
            dtpSDate.CustomFormat = " ";
            dtpPDate.CustomFormat = " ";
            dtpFDate.CustomFormat = " ";
            dtpTDate.CustomFormat = " ";
            txtTDate.Text = "";
            dtpCanDate.CustomFormat = " ";
            dtpDelDate.CustomFormat = " ";

            lblSabun.Text = "";

            cboDept0.Text = "";
            //cboDept0.Items.Clear();

            cboDept1.Text = "";
            //cboDept1.Items.Clear();

            cboDept2.Text = "";
            //cboDept2.Items.Clear();

            cboDr.Text = "";
            //cboDr.Items.Clear();

            txtGKiho.Text = "";
            txtJBunho.Text = "";

            txtILLCode0.Text = "";
            //txtILLCode0.CharacterCasing = CharacterCasing.Upper;
            txtILLCode1.Text = "";
            //txtILLCode1.CharacterCasing = CharacterCasing.Upper;
            txtILLCode2.Text = "";
            //txtILLCode2.CharacterCasing = CharacterCasing.Upper;
            txtILLCode3.Text = "";
            //txtILLCode3.CharacterCasing = CharacterCasing.Upper;
            txtILLCode4.Text = "";
            //txtILLCode4.CharacterCasing = CharacterCasing.Upper;

            txtILLName0.Text = "";
            txtILLName1.Text = "";
            txtILLName2.Text = "";
            txtILLName3.Text = "";
            txtILLName4.Text = "";

            txtVCode0.Text = ""; 
            txtVCode1.Text = "";
            txtVCode2.Text = "";
            txtVCode3.Text = "";
            txtVCode4.Text = "";

            chkTube.Checked = false;
            chkTube1.Checked = false;
            chkTube2.Checked = false;

            txtMS007.Text = "";
            txtMS008.Text = "";
            txtMS009.Text = "";

            txtMemo.Text = "";

            ssVCode_Sheet1.Cells[0, 0].Text = "";
            ssVCode_Sheet1.Cells[0, 1].Text = "";
            ssVCode_Sheet1.Cells[0, 2].Text = "";
            ssVCode_Sheet1.Cells[0, 3].Text = "";
            ssVCode_Sheet1.Cells[0, 4].Text = "";

            txtVCode.Text = "";

            if (GintGubun == 0) { rdo0.Checked = true; }
            if (GintGubun == 1) { rdo1.Checked = true; }
            if (GintGubun == 2) { rdo2.Checked = true; }


            lstIllnamek.Items.Clear();
            grpIlls.Visible = false;


        }

        private void SetCbo()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboDept0.Items.Clear();
            cboDept1.Items.Clear();
            cboDept2.Items.Clear();

            try
            {
                SQL = "";
                SQL = "SELECT DEPTCODE, DEPTNAMEK FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
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
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept0.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                        cboDept1.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                        cboDept2.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboDept0.SelectedIndex = 0;
                cboDept1.SelectedIndex = 0;
                cboDept2.SelectedIndex = 0;


                cboDept0.Text = "";
                cboDept1.Text = "";
                cboDept2.Text = "";

                cboSex.Items.Clear();

                cboSex.Items.Add("M.남자");
                cboSex.Items.Add("F.여자");
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

        private void opt_CheckedChanged(object sender, EventArgs e)
        {
            //건강보험 희귀난치성 환자 관리
            if (rdo0.Checked == true)
            {
                btnNHICCancer.Text = "대상자 자격확인";

                lblCanDate0.Visible = true;
                lblCanDate1.Visible = false;

                panTube.Visible = true;
                panMS.Visible = false;
                panVCode0.Visible = true;
                panVCode1.Visible = true;
                panBurnVCode.Visible = false;
                GintGubun = 0;
            }
            //건강보험 중증화상 환자 관리
            else if (rdo1.Checked == true)
            {
                btnNHICCancer.Text = "대상자 자격확인";

                lblCanDate0.Visible = true;
                lblCanDate1.Visible = false;

                panTube.Visible = false;
                panMS.Visible = false;
                panVCode0.Visible = false;
                panVCode1.Visible = false;
                panBurnVCode.Visible = true;
                GintGubun = 1;
            }
            //중증(암) 환자 관리
            else if (rdo2.Checked == true)
            {
                btnNHICCancer.Text = "암등록 자격확인";

                lblCanDate0.Visible = false;
                lblCanDate1.Visible = true;

                panTube.Visible = false;
                panMS.Visible = true;
                panVCode0.Visible = false;
                panVCode1.Visible = false;
                panBurnVCode.Visible = false;
                GintGubun = 2;
            }
        }

        private void txtSPtNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtSPtNoEnter();
            }
        }

        private void txtSPtNoEnter()
        {
            txtSPtName.Text = "";

            txtSPtNo.Text = ComFunc.LPAD(txtSPtNo.Text.Trim(), 8, "0");

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = "SELECT REGEXP_REPLACE(SNAME,'[A-Z]','') SNAME FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtSPtNo.Text.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당하는 번호의 환자는 없습니다.");
                    return;
                }

                txtSPtName.Text = dt.Rows[0]["SNAME"].ToString().Trim();

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

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetData();
        }

        private void GetData()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            //btnNew.Enabled = false;

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT PANO, REGEXP_REPLACE(SNAME,'[A-Z]','') SNAME, SEX, AGE, JBUNHO, ILLCODE1, TO_CHAR(TDATE,'YYYY-MM-DD') TDATE,  ROWID,decode(GUBUN,'2','희귀','1','암','3','화상',GUBUN ) GUBUN ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_CANCER ";

                //if (rdo0.Checked == true)
                //{
                //    SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='2' "; //희귀난치
                //}
                //else if (rdo1.Checked == true)
                //{
                //    SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='3' "; //중증화상
                //}
                //else if (rdo2.Checked == true)
                //{
                //    SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='1' "; //암
                //}

                if (txtSPtNo.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtSPtNo.Text.Trim() + "' ";
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY SNAME, IDATE DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    //ComFunc.MsgBox("해당 DATA가 없습니다."); 심사팀요청 메세지 않뜨도록 요청옮
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ILLCODE1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GUBUN"].ToString().Trim();

                    if (dt.Rows[i]["JBUNHO"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[i, 0, i, ssView_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 200, 200);
                    }

                    if (dt.Rows[i]["TDATE"].ToString().Trim() != "")
                    {
                        if (Convert.ToDateTime(dt.Rows[i]["TDATE"].ToString().Trim()) < Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")))
                        {
                            ssView_Sheet1.Cells[i, 0, i, ssView_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(200, 255, 200);
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                btnNew.Enabled = true;
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

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }
       
            if (ssView_Sheet1.Cells[e.Row, 6].Text.Trim() == "희귀")
            {
                rdo0.Checked = true;
                GintGubun = 0;
            }
            if (ssView_Sheet1.Cells[e.Row, 6].Text.Trim() == "화상")
            {
                rdo1.Checked = true;
                GintGubun = 1;
            }
            if (ssView_Sheet1.Cells[e.Row, 6].Text.Trim() == "암")
            {
                rdo2.Checked = true;
                GintGubun = 2;
            }


            string strPtNo = ssView_Sheet1.Cells[e.Row, 0].Text.Trim();
            GstrRowID = ssView_Sheet1.Cells[e.Row, 5].Text.Trim();

            READ_BAS_CANCER(GstrRowID);

            if (rdo0.Checked == true)
            {
                //희귀난치특정기호_slip읽기
                READ_RARE_INTRACTABLE_KIHO_slip(strPtNo);
            }
        }

        private void READ_BAS_CANCER(string strRowID)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;

            if (strRowID == "")
            {
                ComFunc.MsgBox("환자를 다시 선택하세요.");
                return;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            //btnSave.Enabled = false;
            //btnDelete.Enabled = false;

            try
            {
                SQL = "";
                SQL = "SELECT PANO, SNAME, SEX, AGE, JBUNHO, GKIHO, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(IDATE,'YYYY-MM-DD') IDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(PDATE,'YYYY-MM-DD') PDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(FDATE,'YYYY-MM-DD') FDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(TDATE,'YYYY-MM-DD') TDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE, "; 
                SQL = SQL + ComNum.VBLF + " TO_CHAR(CANDATE,'YYYY-MM-DD') CANDATE, ";
                SQL = SQL + ComNum.VBLF + " DEPT1, DEPT2, DEPT3,";
                SQL = SQL + ComNum.VBLF + " ILLCODE1, ILLCODE2, ILLCODE3, ILLCODE4, ILLCODE5, Gubun2, VCode,";
                SQL = SQL + ComNum.VBLF + " illvcode1, illvcode2, illvcode3, illvcode4, illvcode5, ";
                SQL = SQL + ComNum.VBLF + " MEMO, DRCODE, ENTSABUN, MS007, MS008, MS009 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_CANCER ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strRowID + "' ";

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

                txtPtNo.Text = dt.Rows[0]["PANO"].ToString().Trim();
                txtPtName.Text = dt.Rows[0]["SNAME"].ToString().Trim();

                //ComFunc.ComboFind(cboSex, "L", 1, dt.Rows[0]["SEX"].ToString().Trim());

               cboSex.Text =dt.Rows[0]["SEX"].ToString().Trim();

                txtAge.Text = dt.Rows[0]["AGE"].ToString().Trim();
                txtGKiho.Text = dt.Rows[0]["GKIHO"].ToString().Trim();
                txtJBunho.Text = dt.Rows[0]["JBUNHO"].ToString().Trim();

                //dtpIDate.ResetText();
                //dtpSDate.ResetText();
                //dtpPDate.ResetText();
                //dtpFDate.ResetText();
                //dtpTDate.ResetText();
                //dtpCanDate.ResetText();
                //dtpDelDate.ResetText();

                dtpIDate.Text = "";
                dtpSDate.Text = "";
                dtpPDate.Text = "";
                dtpFDate.Text = "";
                dtpTDate.Text = "";
                txtTDate.Text = "";
                dtpCanDate.Text = "";
                dtpDelDate.Text = "";


                dtpIDate.CustomFormat = " ";
                dtpSDate.CustomFormat = " ";
                dtpPDate.CustomFormat = " ";
                dtpFDate.CustomFormat = " ";
                dtpTDate.CustomFormat = " ";
                dtpCanDate.CustomFormat = " ";
                dtpDelDate.CustomFormat = " ";


                if (dt.Rows[0]["IDATE"].ToString().Trim() != "") { dtpIDate.Value = Convert.ToDateTime(dt.Rows[0]["IDATE"].ToString().Trim()); }
                if (dt.Rows[0]["SDATE"].ToString().Trim() != "") { dtpSDate.Value = Convert.ToDateTime(dt.Rows[0]["SDATE"].ToString().Trim()); }
                if (dt.Rows[0]["PDATE"].ToString().Trim() != "") { dtpPDate.Value = Convert.ToDateTime(dt.Rows[0]["PDATE"].ToString().Trim()); }
                if (dt.Rows[0]["FDATE"].ToString().Trim() != "") { dtpFDate.Value = Convert.ToDateTime(dt.Rows[0]["FDATE"].ToString().Trim()); }
                if (dt.Rows[0]["TDATE"].ToString().Trim() != "")
                {
                    txtTDate.Text = dt.Rows[0]["TDATE"].ToString().Trim();
                    //if (dt.Rows[0]["TDATE"].ToString().Trim() == "9999-12-31")
                    //{
                    //    dtpTDate.Value = Convert.ToDateTime("9998-12-31");
                    //}
                    //else
                    //{

                    //    dtpTDate.Value = Convert.ToDateTime(dt.Rows[0]["TDATE"].ToString().Trim());
                    //}
                }
                if (dt.Rows[0]["CANDATE"].ToString().Trim() != "") { dtpCanDate.Value = Convert.ToDateTime(dt.Rows[0]["CANDATE"].ToString().Trim()); }
                if (dt.Rows[0]["DELDATE"].ToString().Trim() != "") { dtpDelDate.Value = Convert.ToDateTime(dt.Rows[0]["DELDATE"].ToString().Trim()); }


                //dtpIDate.Text = dt.Rows[0]["IDATE"].ToString().Trim(); 
                //dtpSDate.Text = dt.Rows[0]["SDATE"].ToString().Trim();
                //dtpPDate.Text = dt.Rows[0]["PDATE"].ToString().Trim();
                //dtpFDate.Text = dt.Rows[0]["FDATE"].ToString().Trim();
                //dtpTDate.Text = dt.Rows[0]["TDATE"].ToString().Trim();
                //dtpCanDate.Text = dt.Rows[0]["CANDATE"].ToString().Trim();
                //dtpDelDate.Text = dt.Rows[0]["DELDATE"].ToString().Trim();


                //ComFunc.ComboFind(cboDept0, "L", 2, dt.Rows[0]["DEPT1"].ToString().Trim());
                //ComFunc.ComboFind(cboDept1, "L", 2, dt.Rows[0]["DEPT2"].ToString().Trim());
                //ComFunc.ComboFind(cboDept2, "L", 2, dt.Rows[0]["DEPT3"].ToString().Trim());

                cboDept0.Text = dt.Rows[0]["DEPT1"].ToString().Trim();
                cboDept1.Text = dt.Rows[0]["DEPT2"].ToString().Trim();
                cboDept2.Text = dt.Rows[0]["DEPT3"].ToString().Trim();

                


                txtVCode0.Text = "";
                txtVCode1.Text = "";
                txtVCode2.Text = "";
                txtVCode3.Text = "";
                txtVCode4.Text = "";

                txtILLCode0.Text = dt.Rows[0]["ILLCODE1"].ToString().Trim();
                txtILLName0.Text = Read_Bas_ILL(txtILLCode0.Text.Trim());
                //  txtVCode0.Text = Read_Bas_ILL_H2VCode(txtILLCode0.Text.Trim(), dt.Rows[0]["FDATE"].ToString().Trim());
                if (dt.Rows[0]["illvcode1"].ToString().Trim() == "")
                {
                    txtVCode0.Text = Read_Bas_ILL_H2VCode(txtILLCode0.Text.Trim(), dt.Rows[0]["FDATE"].ToString().Trim());
                } 
                else
                {
                    txtVCode0.Text = dt.Rows[0]["illvcode1"].ToString().Trim();
                }
                txtILLCode1.Text = dt.Rows[0]["ILLCODE2"].ToString().Trim();
                txtILLName1.Text = Read_Bas_ILL(txtILLCode1.Text.Trim());

                if (dt.Rows[0]["illvcode2"].ToString().Trim() == "")
                {
                    txtVCode1.Text = Read_Bas_ILL_H2VCode(txtILLCode1.Text.Trim(), dt.Rows[0]["FDATE"].ToString().Trim());
                }
                else
                {
                    txtVCode1.Text = dt.Rows[0]["illvcode2"].ToString().Trim();
                }

                txtILLCode2.Text = dt.Rows[0]["ILLCODE3"].ToString().Trim();
                txtILLName2.Text = Read_Bas_ILL(txtILLCode2.Text.Trim());

                if (dt.Rows[0]["illvcode3"].ToString().Trim() == "")
                {
                    txtVCode2.Text = Read_Bas_ILL_H2VCode(txtILLCode2.Text.Trim(), dt.Rows[0]["FDATE"].ToString().Trim());
                }
                else
                {
                    txtVCode2.Text = dt.Rows[0]["illvcode3"].ToString().Trim();
                }


                txtILLCode3.Text = dt.Rows[0]["ILLCODE4"].ToString().Trim();
                txtILLName3.Text = Read_Bas_ILL(txtILLCode3.Text.Trim());

                if (dt.Rows[0]["illvcode4"].ToString().Trim() == "")
                {
                    txtVCode3.Text = Read_Bas_ILL_H2VCode(txtILLCode3.Text.Trim(), dt.Rows[0]["FDATE"].ToString().Trim());
                }
                else
                {
                    txtVCode3.Text = dt.Rows[0]["illvcode4"].ToString().Trim();
                }


                txtILLCode4.Text = dt.Rows[0]["ILLCODE5"].ToString().Trim();
                txtILLName4.Text = Read_Bas_ILL(txtILLCode4.Text.Trim());

                if (dt.Rows[0]["illvcode5"].ToString().Trim() == "")
                {
                    txtVCode4.Text = Read_Bas_ILL_H2VCode(txtILLCode4.Text.Trim(), dt.Rows[0]["FDATE"].ToString().Trim());
                }
                else
                {
                    txtVCode4.Text = dt.Rows[0]["illvcode5"].ToString().Trim();
                }



                lblSabun.Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["ENTSABUN"].ToString().Trim());

                if (rdo0.Checked == true)
                {
                    if (txtILLCode0.Text.Trim() == "V000")
                    {
                        txtILLName0.Text = "결핵";
                        txtVCode0.Text = "V000";
                    }

                    if (txtILLCode1.Text.Trim() == "V000")
                    {
                        txtILLName1.Text = "결핵";
                        txtVCode1.Text = "V000";
                    }

                    if (txtILLCode2.Text.Trim() == "V000")
                    {
                        txtILLName2.Text = "결핵";
                        txtVCode2.Text = "V000";
                    }

                    chkTube.Checked = false;
                    chkTube1.Checked = false;
                    chkTube2.Checked = false;
                    chkTube3.Checked = false;

                    if (dt.Rows[0]["GUBUN2"].ToString().Trim() == "1")
                    {
                        chkTube.Checked = true;
                    }
                    if (dt.Rows[0]["GUBUN2"].ToString().Trim() == "3")
                    {
                        chkTube1.Checked = true;
                    }
                    if (dt.Rows[0]["GUBUN2"].ToString().Trim() == "2")
                    {
                        chkTube2.Checked = true;
                    }
                    if (dt.Rows[0]["GUBUN2"].ToString().Trim() == "4")
                    {
                        chkTube3.Checked = true;
                    }
                }
                else if (rdo1.Checked == true)
                {
                    txtVCode.Text = dt.Rows[0]["VCODE"].ToString().Trim();
                }
                else if (rdo2.Checked == true)
                {
                    txtMS007.Text = dt.Rows[0]["MS007"].ToString().Trim();
                    txtMS008.Text = dt.Rows[0]["MS008"].ToString().Trim();
                    txtMS009.Text = dt.Rows[0]["MS009"].ToString().Trim();
                }

                SQL = "";
                SQL = "SELECT DRCODE, DRNAME FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                SQL = SQL + ComNum.VBLF + " WHERE DRDEPT1 = '" + VB.Left(cboDept0.Text, 2) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TOUR ='N'";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt1.Rows.Count > 0)
                {
                    for (i = 0; i < dt1.Rows.Count; i++)
                    {
                        cboDr.Items.Add(dt1.Rows[i]["DRCODE"].ToString().Trim() + "." + dt1.Rows[i]["DRNAME"].ToString().Trim());
                    }
                }

                dt1.Dispose();
                dt1 = null;

                //cboDept0의 과의사임
                ComFunc.ComboFind(cboDr, "L", 4, dt.Rows[0]["DRCODE"].ToString().Trim());

                txtMemo.Text = dt.Rows[0]["MEMO"].ToString().Trim();

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

        private string Read_Bas_ILL(string strCode)
        {
            string rtnVal = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = "SELECT IllNameK FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                SQL = SQL + ComNum.VBLF + " WHERE IllCode = '" + strCode + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["IllNameK"].ToString().Trim();

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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string Read_Bas_ILL_H2VCode(string strCode, string strDate = "")
        {
            string rtnVal = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = "SELECT VCODE FROM " + ComNum.DB_PMPA + "BAS_ILLS_H";
                SQL = SQL + ComNum.VBLF + " WHERE IllCode = '" + strCode + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["VCODE"].ToString().Trim();

                dt.Dispose();
                dt = null;

                if (strDate != "")
                {
                    if (Convert.ToDateTime(strDate) >= Convert.ToDateTime("2016-07-01"))
                    {
                        if (rtnVal == "V246")
                        {
                            rtnVal = "V000";
                        }
                    }
                }

                return rtnVal;
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
                return rtnVal;
            }
        }

        private void READ_RARE_INTRACTABLE_KIHO_slip(string strPtNo)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssVCode_Sheet1.Cells[0, 0].Text = "";
            ssVCode_Sheet1.Cells[0, 1].Text = "";
            ssVCode_Sheet1.Cells[0, 2].Text = "";
            ssVCode_Sheet1.Cells[0, 3].Text = "";
            ssVCode_Sheet1.Cells[0, 4].Text = "";

            try
            {
                SQL = "";
                SQL = "SELECT SUCODE FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND ACTDATE >=TRUNC(SYSDATE -300) ";
                SQL = SQL + ComNum.VBLF + "  AND SUCODE LIKE '@V%' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY SUCODE ";

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
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 5)
                    {
                        break;
                    }

                    ssVCode_Sheet1.Cells[0, i].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
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

        private void txtPtNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strRowID = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            txtPtName.Text = "";
            txtPtNo.Text = ComFunc.LPAD(txtPtNo.Text.Trim(), 8, "0");

            try
            {
                SQL = "";
                SQL = "SELECT REGEXP_REPLACE(SNAME,'[A-Z]','') SNAME, SEX, JUMIN1 || JUMIN2 JUMIN, GKIHO";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPtNo.Text.Trim() + "' ";

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
                    ComFunc.MsgBox("해당하는 번호의 환자는 없습니다.");
                    return;
                }

                txtPtName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                //ComFunc.ComboFind(cboSex, "L", 1, dt.Rows[0]["SEX"].ToString().Trim());
                cboSex.Text = dt.Rows[0]["SEX"].ToString().Trim();
                txtAge.Text = ComFunc.AgeCalcEx(dt.Rows[0]["JUMIN"].ToString().Trim(), ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).ToString();
                txtGKiho.Text = dt.Rows[0]["GKIHO"].ToString().Trim();
                btnSave.Enabled = true;

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT ROWID FROM " + ComNum.DB_PMPA + "BAS_CANCER ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + txtPtNo.Text.Trim() + "' ";

                if (rdo0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND GUBUN ='2' ";    //희귀난치
                }
                else if (rdo1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND GUBUN ='3' ";    //중증화상
                }
                else if (rdo2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND GUBUN ='1' ";    //암
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    if (ComFunc.MsgBoxQ("이미 등록된 환자 입니다!!" + ComNum.VBLF + " 1.Y:추가로 등록!!    2.N:기존자료 갱신 !!", "등록선택", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                    {
                        strRowID = dt.Rows[0]["ROWID"].ToString().Trim();

                        READ_BAS_CANCER(strRowID);

                        if (rdo0.Checked == true)
                        {
                            READ_RARE_INTRACTABLE_KIHO_slip(txtPtNo.Text.Trim());
                        }
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string strTemp = "";

            strTemp = txtSPtNo.Text.Trim();
            GstrRowID = "";
            FormClear();

            txtPtNo.Enabled = true;
            txtPtNo.Focus();

            if (strTemp != "")
            {
                txtPtNo.Text = strTemp;
                txtSPtNo.Text = strTemp;
                txtSPtNoEnter();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FormClear();
        }

        private void btnPut_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if (txtPtName.Text.Trim() == "")
            {
                MessageBox.Show("환자목록에서 환자를 선택해주세요");
                return;
            }

            GstrPtNo = txtPtNo.Text.Trim();

            int intIndex = 0;
            string strPtNo = "";

            if (rdo0.Checked == true)
            {
                intIndex = 0;
            }
            else if (rdo1.Checked == true)
            {
                intIndex = 1;
            }
            else if (rdo2.Checked == true)
            {
                intIndex = 2;
            }

            if (txtSPtNo.Text.Trim() != "")
            {
                strPtNo = txtSPtNo.Text.Trim();
            }
            else
            {
                strPtNo = txtPtNo.Text.Trim();
            }

            frmBupPatEntry_new frm = new frmBupPatEntry_new(strPtNo, intIndex);
            frm.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtILLCode_KeyDown(object sender, KeyEventArgs e)
        {
            //string strDate = "";

            //if (e.KeyCode != Keys.Enter)
            //{
            //    return;
            //}

            //switch (VB.Right(((TextBox)sender).Name, 1))
            //{
            //    case "0":
            //        txtILLName0.Text = Read_Bas_ILL(VB.UCase(((TextBox)sender).Text.Trim()));
            //        break;
            //    case "1":
            //        txtILLName1.Text = Read_Bas_ILL(VB.UCase(((TextBox)sender).Text.Trim()));
            //        break;
            //    case "2":
            //        txtILLName2.Text = Read_Bas_ILL(VB.UCase(((TextBox)sender).Text.Trim()));
            //        break;
            //    case "3":
            //        txtILLName3.Text = Read_Bas_ILL(VB.UCase(((TextBox)sender).Text.Trim()));
            //        break;
            //    case "4":
            //        txtILLName4.Text = Read_Bas_ILL(VB.UCase(((TextBox)sender).Text.Trim()));
            //        break;
            //}

            ////2016-09-19 유효시작일에 따라 V246 => V000 강제치환해줌
            //strDate = dtpFDate.Value.ToString("yyyy-MM-dd");

            //if (strDate == "")
            //{
            //    strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            //}

            //switch (VB.Right(((TextBox)sender).Name, 1))
            //{
            //    case "0":
            //        txtVCode0.Text = Read_Bas_ILL_H2VCode(VB.UCase(((TextBox)sender).Text.Trim()));
            //        break;
            //    case "1":
            //        txtVCode1.Text = Read_Bas_ILL_H2VCode(VB.UCase(((TextBox)sender).Text.Trim()));
            //        break;
            //    case "2":
            //        txtVCode2.Text = Read_Bas_ILL_H2VCode(VB.UCase(((TextBox)sender).Text.Trim()));
            //        break;
            //    case "3":
            //        txtVCode3.Text = Read_Bas_ILL_H2VCode(VB.UCase(((TextBox)sender).Text.Trim()));
            //        break;
            //    case "4":
            //        txtVCode4.Text = Read_Bas_ILL_H2VCode(VB.UCase(((TextBox)sender).Text.Trim()));
            //        break;
            //}
        }

        private void cboDept_Leave(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            if (cboDept0.Text.Trim() == "")
            {
                ComFunc.MsgBox("진료과를 선택해주세요.");
                cboDept0.Focus();
                return;
            }

            try
            {
                SQL = "";
                SQL = "SELECT DRCODE, DRNAME FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                SQL = SQL + ComNum.VBLF + " WHERE DRDEPT1 = '" + VB.Left(cboDept0.Text, 2) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TOUR ='N'";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

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

                cboDr.Items.Clear();

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDr.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim());
                }

                cboDr.SelectedIndex = 0;

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strTube = "";
            string strGubun = "";


            //if (txtTDate.Text =="")
            //{
            //    MessageBox.Show(txtTDate.Text);
            //}

            if (ComFunc.MsgBoxQ("해당정보를 수정하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            if (rdo0.Checked == true)
            {
                strTube = "0";

                if (chkTube.Checked == true)
                {
                    strTube = "1";
                }
                if (chkTube1.Checked == true)
                {
                    strTube = "3";
                }
                if (chkTube2.Checked == true)
                {
                    strTube = "2";
                }
                if (chkTube3.Checked == true)
                {
                    strTube = "4";
                }

                strGubun = "2";
            }
            else if (rdo1.Checked == true)
            {
                strGubun = "3";
            }
            else if (rdo2.Checked == true)
            {
                strGubun = "1";
            }

            if (cboDept0.Text.Trim() == "")
            {
                ComFunc.MsgBox("해당진료과를 선택해주세요.");
                return;
            }

            if (GstrSabun == "")
            {
                GstrSabun = clsType.User.IdNumber;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);


            try
            {
                if (GstrRowID == "")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_CANCER ";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         PANO, SNAME , SEX, AGE, JBUNHO, GKIHO, IDATE, SDATE, PDATE, FDATE, TDATE,";
                    SQL = SQL + ComNum.VBLF + "         DELDATE , CANDATE, DEPT1, DEPT2, DEPT3, ILLCODE1, ILLCODE2, ILLCODE3,";
                    SQL = SQL + ComNum.VBLF + "         ILLCODE4 , ILLCODE5, Memo, illvcode1,illvcode2,illvcode3,illvcode4,illvcode5,DRCODE, ENTSABUN, GUBUN";
                    if (rdo0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "         , Gubun2";
                    }
                    else if (rdo2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "         , VCODE, MS007, MS008, MS009";
                    }
                    else if (rdo1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "         , VCODE";
                    }
                    
                    SQL = SQL + ComNum.VBLF + "     )";
                    SQL = SQL + ComNum.VBLF + "VALUES ";
                    SQL = SQL + ComNum.VBLF + "     ('" + txtPtNo.Text + "', '" + txtPtName.Text + "',  ";
                    SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboSex.Text, 1) + "',  '" + txtAge.Text + "' , ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtJBunho.Text + "', '" + txtGKiho.Text + "' ,";
                    //2020-06-22 등록 누를 시 날짜로 변경
                    //SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpIDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE ,";

                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpSDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpPDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpFDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";
                    //SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpTDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + txtTDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";

                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpDelDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpCanDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboDept0.Text, 2) + "', '" + VB.Left(cboDept1.Text, 2) + "', '" + VB.Left(cboDept2.Text, 2) + "' ,";
                    SQL = SQL + ComNum.VBLF + "         '" + txtILLCode0.Text + "' ,'" + txtILLCode1.Text + "' ,'" + txtILLCode2.Text + "' ,";
                    SQL = SQL + ComNum.VBLF + "         '" + txtILLCode3.Text + "' ,'" + txtILLCode4.Text + "' ,'" + txtMemo.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtVCode0.Text + "' ,'" + txtVCode1.Text + "' ,'" + txtVCode2.Text + "' ,";
                    SQL = SQL + ComNum.VBLF + "         '" + txtVCode3.Text + "' ,'" + txtVCode4.Text + "' , ";
                    SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboDr.Text, 4) + "', " + GstrSabun + " ,'" + strGubun + "' ";

                    if (rdo0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + ",'" + strTube + "'";
                    }
                    else if (rdo2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + ",'V193', '" + txtMS007.Text.Trim() + "', '" + txtMS008.Text.Trim() + "', '" + txtMS009.Text.Trim() + "'";
                    }
                    else if (rdo1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + ",'" + VB.UCase(txtVCode.Text.Trim()) + "'";
                    }

                    SQL = SQL + ComNum.VBLF + "     ) ";
                }
                else
                {
                    
                    
                    SQL = "";
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "";

                    SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_CANCER";
                    SQL = SQL + ComNum.VBLF + " SET ";
                    SQL = SQL + ComNum.VBLF + "     SEX = '" + VB.Left(cboSex.Text, 1) + "' ,";
                    SQL = SQL + ComNum.VBLF + "     AGE = '" + txtAge.Text + "' , ";
                    SQL = SQL + ComNum.VBLF + "     JBUNHO =  '" + txtJBunho.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "     GKIHO = '" + txtGKiho.Text.Trim() + "' ,";
                    //2020-06-22 등록 누를 시 날자로 변경
                    //SQL = SQL + ComNum.VBLF + "     IDATE =   TO_DATE('" + dtpIDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "     IDATE =   SYSDATE,";

                    SQL = SQL + ComNum.VBLF + "     SDATE =   TO_DATE('" + dtpSDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "     PDATE =   TO_DATE('" + dtpPDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "     FDATE =   TO_DATE('" + dtpFDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";
                    //SQL = SQL + ComNum.VBLF + "     TDATE =   TO_DATE('" + dtpTDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "     TDATE =   TO_DATE('" + txtTDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "     DELDATE =  TO_DATE('" + dtpCanDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "     CANDATE =  TO_DATE('" + dtpDelDate.Text.Trim() + "' ,'YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "     DEPT1 =  '" + VB.Left(cboDept0.Text, 2) + "', ";
                    SQL = SQL + ComNum.VBLF + "     DEPT2 =  '" + VB.Left(cboDept1.Text, 2) + "',";
                    SQL = SQL + ComNum.VBLF + "     DEPT3 =  '" + VB.Left(cboDept2.Text, 2) + "' ,";
                    SQL = SQL + ComNum.VBLF + "     ILLCODE1 = '" + txtILLCode0.Text + "' ,";
                    SQL = SQL + ComNum.VBLF + "     ILLCODE2 = '" + txtILLCode1.Text + "' ,";
                    SQL = SQL + ComNum.VBLF + "     ILLCODE3 = '" + txtILLCode2.Text + "' ,";
                    SQL = SQL + ComNum.VBLF + "     ILLCODE4 = '" + txtILLCode3.Text + "' ,";
                    SQL = SQL + ComNum.VBLF + "     ILLCODE5 = '" + txtILLCode4.Text + "' ,";
                    SQL = SQL + ComNum.VBLF + "     ILLVCODE1 = '" + txtVCode0.Text + "' ,";
                    SQL = SQL + ComNum.VBLF + "     ILLVCODE2 = '" + txtVCode1.Text + "' ,";
                    SQL = SQL + ComNum.VBLF + "     ILLVCODE3 = '" + txtVCode2.Text + "' ,";
                    SQL = SQL + ComNum.VBLF + "     ILLVCODE4 = '" + txtVCode3.Text + "' ,";
                    SQL = SQL + ComNum.VBLF + "     ILLVCODE5 = '" + txtVCode4.Text + "' ,";
                    SQL = SQL + ComNum.VBLF + "     MEMO = '" + txtMemo.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "     DRCODE =   '" + VB.Left(cboDr.Text, 4) + "', ";
                    SQL = SQL + ComNum.VBLF + "     ENTSABUN = " + GstrSabun + ",";
                    SQL = SQL + ComNum.VBLF + "     GUBUN  =   '" + strGubun + "', ";

                    if (rdo0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     VCode  =   '" + VB.UCase(txtVCode.Text.Trim()) + "',";
                        SQL = SQL + ComNum.VBLF + "     GUBUN2  =   '" + strTube + "'";
                    }
                    else if (rdo2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     MS007  =   '" + txtMS007.Text.Trim() + "',";
                        SQL = SQL + ComNum.VBLF + "     MS008  =   '" + txtMS008.Text.Trim() + "',";
                        SQL = SQL + ComNum.VBLF + "     MS009  =   '" + txtMS009.Text.Trim() + "',";
                        SQL = SQL + ComNum.VBLF + "     VCode  =   '" + VB.UCase(txtVCode.Text.Trim()) + "'";
                    }
                    else if (rdo1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     VCode  =   '" + VB.UCase(txtVCode.Text.Trim()) + "'";
                    }

                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + GstrRowID + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (GstrRowID == "")
            {
                ComFunc.MsgBox("환자선택오류");
                return;
            }

            if (ComFunc.MsgBoxQ("해당 환자정보를 정말로 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
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
                SQL = "DELETE " + ComNum.DB_PMPA + "BAS_CANCER ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + GstrRowID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                FormClear();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnNHICCancer_Click(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strMsg = "";
            string strGubun = "0";


            if (cboDept0.Text =="")
            {
                MessageBox.Show ( "진료과를 선택해주세요");
                return;
            }

            if (txtPtNo.Text.Trim() == "")
            {
                if (rdo0.Checked == true)
                {
                    strMsg = "희귀난치성";
                }
                else if (rdo1.Checked == true)
                {
                    strMsg = "중증화상";
                }
                else if (rdo2.Checked == true)
                {
                    strMsg = "암";
                }

                ComFunc.MsgBox(strMsg + " 등록 환자를 선택주세요.");
                return;
            }

            try
            {
                SQL = "";
                SQL = "SELECT PANO, JUMIN1 || JUMIN2 JUMIN, REGEXP_REPLACE(SNAME,'[A-Z]','') SNAME ";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + txtPtNo.Text.Trim() + "' ";

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
                    ComFunc.MsgBox("등록된 환자가 아닙니다.");
                    return;
                }

                if (rdo0.Checked == true) { strGubun = "2"; }
                else if (rdo1.Checked == true) { strGubun = "3"; }
                else if (rdo2.Checked == true) { strGubun = "1"; }

                frmNHICCancerEvent = new frmNHICCancer(txtPtNo.Text.Trim(), VB.Left(cboDept0.Text, 2), txtPtName.Text.Trim(), clsAES.Read_Jumin_AES(clsDB.DbCon, txtPtNo.Text.Trim()), ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), strGubun, GstrSabun);
                frmNHICCancerEvent.rSetSetNHIC += Frm_rSetSetNHIC;
                frmNHICCancerEvent.rEventClosed += Frm_rEventClosed;
                
                //frmNHICCancerEvent.Show();
                frmNHICCancerEvent.ShowDialog();
                
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

        private void Frm_rEventClosed()
        {
            frmNHICCancerEvent.Dispose();
            frmNHICCancerEvent = null;
        }

        private void Frm_rSetSetNHIC(string strNHICNO, string strNHICFDATE, string strNHICTDATE, string strNHICILLCODE, string strNHICJISA, string strNHICNAME, string strNHICKIHO, string strNHICNUM, string strNHICBONIN)
        {   //jjy:2018-07-25 심사팀 김준수 요청으로아래  점검로직 로직 삭제
            //if (rdo2.Checked == true)
            //{
            //    //2014-07-02
            //    if (dtpFDate.Value.ToString("yyyy-MM-dd") != strNHICFDATE && dtpFDate.Text != "")
            //    {
            //        btnSave.Enabled = false;
            //    }
            //}

            txtJBunho.Text = strNHICNO;  //중증등록번호
            dtpFDate.Format = DateTimePickerFormat.Short;


            dtpFDate.Value = Convert.ToDateTime(strNHICFDATE);     //적용시작일
            //if (strNHICTDATE == "9999-12-31") strNHICTDATE = "9998-12-30";
            //dtpTDate.Value = Convert.ToDateTime(strNHICTDATE);   //적용종료일
            txtTDate.Text = strNHICTDATE;
            //if (rdo1.Checked == true)
            //{
            if (strNHICILLCODE != "")
            {
                txtILLCode0.Text = strNHICILLCODE;      //상병코드
                if (rdo1.Checked == true)
                {
                    txtVCode.Text = strNHICILLCODE;      //상병코드
                }

            }
           
            //}
            frmNHICCancerEvent.Dispose();
            frmNHICCancerEvent = null;


            //괄호 안에 문자 추출 작업
            int sFrom = strNHICBONIN.IndexOf("(") + "(".Length;
            int sTo = strNHICBONIN.LastIndexOf(")");

            string strResult = strNHICBONIN.Substring(sFrom, sTo - sFrom);

            switch (strResult)
            { 
                case "중증희귀":
                    chkTube.Checked = false;
                    chkTube1.Checked = true;
                    chkTube2.Checked = false;
                    chkTube3.Checked = false;
                    break;
                case "희귀":
                    chkTube.Checked = false;
                    chkTube1.Checked = false;
                    chkTube2.Checked = true;
                    chkTube3.Checked = false;

                    break;
                case "중증치매":
                    chkTube.Checked = false;
                    chkTube1.Checked = false;
                    chkTube2.Checked = false;
                    chkTube3.Checked = true;
                    break;

            }
        }

        private void edtpValueChanged(object sender, EventArgs e)
        {
            //DateTimePicker o = new DateTimePicker();

            if (sender == this.dtpIDate)
            {
                dtpIDate.CustomFormat = "yyyy-MM-dd";
                return;
            }
            if (sender == this.dtpSDate)
            {
                dtpSDate.CustomFormat = "yyyy-MM-dd";
                return;
            }
            if (sender == this.dtpPDate)
            {
                dtpPDate.CustomFormat = "yyyy-MM-dd";
                return;
            }
            if (sender == this.dtpFDate)
            {
                dtpFDate.CustomFormat = "yyyy-MM-dd";
                return;
            }
            if (sender == this.dtpTDate)
            {
                dtpTDate.CustomFormat = "yyyy-MM-dd";
                return;
            }

            if (sender == this.dtpCanDate)
            {
                dtpCanDate.CustomFormat = "yyyy-MM-dd";
                return;
            }

            if (sender == this.dtpDelDate)
            {
                dtpDelDate.CustomFormat = "yyyy-MM-dd";
                return;
            }
        }


        private void edtpKeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete && e.KeyCode != Keys.Escape) return;


            if (sender == this.dtpIDate) 
            {
                dtpIDate.CustomFormat = " ";
                //dtpIDate.Text = "";
                return;
            }
            if (sender == this.dtpSDate) 
            {
                dtpSDate.CustomFormat = " ";
                //dtpSDate.Text = "";
                return;
            }
            if (sender == this.dtpPDate) 
            {
                dtpPDate.CustomFormat = " ";
                //dtpPDate.Text = "";
                return;
            }
            if (sender == this.dtpFDate) 
            {
                dtpFDate.CustomFormat = " ";
                //dtpFDate.Text = "";
                return;
            }
            if (sender == this.dtpTDate) 
            {
                dtpTDate.CustomFormat = " ";
                //dtpTDate.Text = "";
                return;
            }

            if (sender == this.dtpCanDate) 
            {
                dtpCanDate.CustomFormat = " ";
                //dtpCanDate.Text = "";
                return;
            }

            if (sender == this.dtpDelDate) 
            {
                dtpDelDate.CustomFormat = " ";
                //dtpDelDate.Text = "";
                return;
            }
        }


        private void edtpKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (int)Keys.Enter) return;
            SendKeys.Send("{TAB}");

            if (sender == this.dtpFDate)
            {
                dtpTDate.Value = dtpFDate.Value.AddDays(365 * 5);

                return;
            }


        }

        private void txtILLCode0_KeyPress(object sender, KeyPressEventArgs e)
        {
          if (e.KeyChar == (char)13)
            {
                if (txtILLCode0.Text.Trim() != "")
                {
                    //txtIllNameNew.Text = Read_Bas_ILL(txtIllCodeNew.Text.Trim());
                    SetIllsListBox(txtILLCode0.Text.Trim(), "txtILLCode0");
                    //txtGiho.Text = Read_Bas_IllsToVCode(txtIllCodeNew.Text.Trim());
                }
            }
        }

        private void SetIllsListBox(string strSang, string strgubun)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            //string rtnVal = "";

            int i = 0;

            lstIllnamek.Items.Clear();
            grpIlls.Visible = false;
            labGubun.Text = "";
            grpIlls.Location = new Point(384, 318);




            if (rdo2.Checked == true)       //암
            {
                SQL = "  SELECT SUBSTR(REPLACE(ILLCODE, '.', '') || '        ', 1, 16) || ILLNAMEK || ' (V193)' ILLNAMEK   ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_ILLS_CANCER ";
                SQL += ComNum.VBLF + " WHERE REPLACE(ILLCODE, '.', '') LIKE '" + strSang + "%' ";
                SQL += ComNum.VBLF + "   ORDER BY ILLCODE ASC, ILLNAMEK ASC ";
            }
            else
            {
                SQL = "  SELECT SUBSTR(REPLACE(ILLCODE, '.', '') || '        ', 1, 16) || ILLNAMEK || ' (' || VCODE || ')' ILLNAMEK   ";
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
                    labGubun.Text = strgubun;
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

        private void lstIllnamek_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int selectedIndex = -1;

            Point point = e.Location;
            selectedIndex = lstIllnamek.IndexFromPoint(point);

            if (labGubun.Text == "txtILLCode0")
            {
                if (selectedIndex != -1)
                {
                    string strData = lstIllnamek.Items[selectedIndex] as string;
                    txtILLCode0.Text = VB.Left(strData, 10).Trim();
                    txtILLName0.Text = VB.Mid(strData, 12, VB.InStr(strData, " (V") - 12).Trim();
                    txtVCode0.Text = VB.Mid(strData, VB.InStr(strData, " (V") + 2, 4).Trim();
                    grpIlls.Visible = false;
                }
            }
            else  if (labGubun.Text == "txtILLCode0")
                
            {
                    if (selectedIndex != -1)
                    {
                        string strData = lstIllnamek.Items[selectedIndex] as string;
                        txtILLCode0.Text = VB.Left(strData, 10).Trim();
                        txtILLName0.Text = VB.Mid(strData, 12, VB.InStr(strData, " (V") - 12).Trim();
                        txtVCode0.Text = VB.Mid(strData, VB.InStr(strData, " (V") + 2, 4).Trim();
                        grpIlls.Visible = false;
                    }
            }
            else if (labGubun.Text == "txtILLCode1")

            {
                if (selectedIndex != -1)
                {
                    string strData = lstIllnamek.Items[selectedIndex] as string;
                    txtILLCode1.Text = VB.Left(strData, 10).Trim();
                    txtILLName1.Text = VB.Mid(strData, 12, VB.InStr(strData, " (V") - 12).Trim();
                    txtVCode1.Text = VB.Mid(strData, VB.InStr(strData, " (V") + 2, 4).Trim();
                    grpIlls.Visible = false;
                }
            }
            else if (labGubun.Text == "txtILLCode2")

            {
                if (selectedIndex != -1)
                {
                    string strData = lstIllnamek.Items[selectedIndex] as string;
                    txtILLCode2.Text = VB.Left(strData, 10).Trim();
                    txtILLName2.Text = VB.Mid(strData, 12, VB.InStr(strData, " (V") - 12).Trim();
                    txtVCode2.Text = VB.Mid(strData, VB.InStr(strData, " (V") + 2, 4).Trim();
                    grpIlls.Visible = false;
                }
            }
            else if (labGubun.Text == "txtILLCode3")

            {
                if (selectedIndex != -1)
                {
                    string strData = lstIllnamek.Items[selectedIndex] as string; 
                    txtILLCode3.Text = VB.Left(strData, 10).Trim();
                    txtILLName3.Text = VB.Mid(strData, 12, VB.InStr(strData, " (V") - 12).Trim();
                    txtVCode3.Text = VB.Mid(strData, VB.InStr(strData, " (V") + 2, 4).Trim();
                    grpIlls.Visible = false;
                }
            }
            else if (labGubun.Text == "txtILLCode4")

            {
                if (selectedIndex != -1)
                {
                    string strData = lstIllnamek.Items[selectedIndex] as string;
                    txtILLCode4.Text = VB.Left(strData, 10).Trim();
                    txtILLName4.Text = VB.Mid(strData, 12, VB.InStr(strData, " (V") - 12).Trim();
                    txtVCode4.Text = VB.Mid(strData, VB.InStr(strData, " (V") + 2, 4).Trim();
                    grpIlls.Visible = false;
                }
            }


        }

        private void txtILLCode1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (txtILLCode0.Text.Trim() != "")
                {
                    //txtIllNameNew.Text = Read_Bas_ILL(txtIllCodeNew.Text.Trim());
                    SetIllsListBox(txtILLCode1.Text.Trim(), "txtILLCode1");
                    //txtGiho.Text = Read_Bas_IllsToVCode(txtIllCodeNew.Text.Trim());
                }
            }
        }

        private void txtILLCode2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (txtILLCode0.Text.Trim() != "")
                {
                    //txtIllNameNew.Text = Read_Bas_ILL(txtIllCodeNew.Text.Trim());
                    SetIllsListBox(txtILLCode2.Text.Trim(), "txtILLCode2");
                    //txtGiho.Text = Read_Bas_IllsToVCode(txtIllCodeNew.Text.Trim());
                }
            }
        }

        private void txtILLCode3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (txtILLCode0.Text.Trim() != "")
                {
                    //txtIllNameNew.Text = Read_Bas_ILL(txtIllCodeNew.Text.Trim());
                    SetIllsListBox(txtILLCode3.Text.Trim(), "txtILLCode3");
                    //txtGiho.Text = Read_Bas_IllsToVCode(txtIllCodeNew.Text.Trim());
                }
            }
        }

        private void txtILLCode4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (txtILLCode0.Text.Trim() != "")
                {
                    //txtIllNameNew.Text = Read_Bas_ILL(txtIllCodeNew.Text.Trim());
                    SetIllsListBox(txtILLCode4.Text.Trim(), "txtILLCode4");
                    //txtGiho.Text = Read_Bas_IllsToVCode(txtIllCodeNew.Text.Trim());
                }
            }

        }

      
    }
}