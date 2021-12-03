using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.ManagedDataAccess.Client;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB.dll
    /// File Name       : frmBupPatEntry.cs
    /// Description     : 건강보험(희귀난치성, 중증화상, 중증암) 신청서 / 수술예방적작성지 인쇄
    /// Author          : 이정현
    /// Create Date     : 2017-06-17
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// frmBupPatEntry.cs 으로 변경함
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
    public partial class frmBupPatEntry : Form
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
        
        public frmBupPatEntry()
        {
            InitializeComponent();
        }

        public frmBupPatEntry(string strPtNo, int intIndex, bool bolOrder = false)
        {
            InitializeComponent();

            GstrPtNo = strPtNo;
            GintIndex = intIndex;
            GbolOrder = bolOrder;
        }

        public frmBupPatEntry(string strPtNo, string strIO, string strDept, string strDrCode, string strOP, string strRare, string strSabun)
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

        private void frmBupPatEntry_Load(object sender, EventArgs e)
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

            SetCbo();

            btnExt.Text = "▲내용입력";
            
            //2016-01-27 OCS프로그램에서 사용시
            //if (VB.UCase(App.EXEName).Trim() == "MTSOORDER")
            if (GbolOrder == true)
            {
                btnExt.Click += new EventHandler(btnExt_Click);
            }

            GintTimer = 0;

            if (GintIndex == 0)
            {
                rdo3.Checked = true;
            }
            else if (GintIndex == 1)
            {
                rdo2.Checked = true;
            }
            else if (GintIndex == 2)
            {
                rdo0.Checked = true;
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

                    cboDr.Items.Clear();

                    SQL = "";
                    SQL = "SELECT DRCODE, DRNAME FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                    SQL = SQL + ComNum.VBLF + " WHERE DRDEPT1 = '" + VB.Left(cboDept.Text, 2) + "' ";
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

                        for(i = 0; i < dt.Rows.Count; i++)
                        {
                            cboDr.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim());
                        }

                        cboDr.SelectedIndex = 0;
                    }

                    dt.Dispose();
                    dt = null;
                }

                if(GstrDrCode != "")
                {
                    ComFunc.ComboFind(cboDr, "L", 4, GstrDrCode);
                }

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

                txtPtNoEnter();

                if (GstrOP == "수술")
                {
                    toolBohum1.Enabled = false;
                    toolGub1.Enabled = false;
                }

                if (GstrRare == "희귀")
                {
                    rdo1.Checked = true;
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

        private void FormClear()
        {
            GstrOK = "";

            toolDelete.Visible = false;

            rdo0.Checked = true;

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
        }

        private void SetCbo()
        {
            clsVbfunc.SetBCodeCombo(clsDB.DbCon, cboGan, "ETC_중증환자관계", 1, true, "N");
            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept);

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
                SQL = SQL + ComNum.VBLF + "   From ADMIN.BAS_CANCER";
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
                SQL = SQL + ComNum.VBLF + " WHERE IllCode = '" + strSang + "'";

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
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (SaveData() == true)
            {
                FormClear();
            }
        }

        private bool SaveData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strGbn = "";
            string strGu = "";

            strGbn = VB.IIf(rdo0.Checked == true, "1", VB.IIf(rdo1.Checked == true, "2", VB.IIf(rdo2.Checked == true, "3", "4"))).ToString();
            strGu = (rdoGU0.Checked == true ? "1" : (rdoGU1.Checked == true ? "2" : "3"));

            if (cboDr.Text.Trim() == "")
            {
                ComFunc.MsgBox("의사를 선택해주세요.");
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if(GstrROWID2 != "")
                {
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_CANCER_DTL";
                    SQL = SQL + ComNum.VBLF + "     SET ";
                    SQL = SQL + ComNum.VBLF + "         DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "' , ";
                    SQL = SQL + ComNum.VBLF + "         DRCODE = '" + VB.Left(cboDr.Text, 4) + "', ";
                    SQL = SQL + ComNum.VBLF + "         IPDOPD = '" + VB.Left(cboIO.Text, 1) + "',";
                    SQL = SQL + ComNum.VBLF + "         CANDATE = TO_DATE('" + dtpCanDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "         ILLCODE = '" + txtILLCode.Text + "' ,";

                    if(chkJinDan0.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN1 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN1 = '0', "; }
                    if(chkJinDan1.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN2 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN2 = '0', "; }
                    if(chkJinDan2.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN3 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN3 = '0', "; }
                    if(chkJinDan3.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN4 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN4 = '0', "; }
                    if(chkJinDan4.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN5 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN5 = '0', "; }

                    SQL = SQL + ComNum.VBLF + "         DTLGUBN = '" + strGbn + "' , ";
                    SQL = SQL + ComNum.VBLF + "         REMARK1 = '" + txtRemark.Text + "' , ";

                    if(chkJinDan5.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN6 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN6 = '0', "; }
                    if(chkJinDan6.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN7 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN7 = '0', "; }
                    if(chkJinDan7.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN8 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN8 = '0', "; }
                    if(chkJinDan8.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN9 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN9 = '0', "; }
                    if(chkJinDan9.Checked == true) { SQL = SQL + ComNum.VBLF + "         JINDAN10 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN10 = '0', "; }
                    if(chkJinDan10.Checked == true) { SQL = SQL + ComNum.VBLF + "        JINDAN11 = '1' , "; } else { SQL = SQL + ComNum.VBLF + "         JINDAN11 = '0', "; }

                    SQL = SQL + ComNum.VBLF + "         REMARK2 = '" + txtRemark2.Text + "' , ";
                    SQL = SQL + ComNum.VBLF + "         SDATE = TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "         ASSENTIENT = '" + txtDongSName.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "         ASSENTIENTDATE = TO_DATE('" + dtpDongDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "         NAME = '" + txtSinName.Text + "' ,";
                    SQL = SQL + ComNum.VBLF + "         TEL = '" + txtSTel.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "         GANGE = '" + VB.Left(cboGan.Text, 2) + "',";
                    SQL = SQL + ComNum.VBLF + "         ENTDATE = SYSDATE ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + GstrROWID2 + "'  ";
                }
                else
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_CANCER_DTL";
                    SQL = SQL + ComNum.VBLF + "         (PANO, DEPTCODE , IPDOPD, CANDATE, ILLCODE, JINDAN1, JINDAN2, JINDAN3, JINDAN4, JINDAN5,";
                    SQL = SQL + ComNum.VBLF + "         REMARK1, JINDAN6, JINDAN7, JINDAN8, JINDAN9, JINDAN10, JINDAN11, REMARK2, DRCODE,";
                    SQL = SQL + ComNum.VBLF + "         SDATE, ASSENTIENT,ASSENTIENTDate, NAME, TEL, GANGE, SENDDATE,  RESDATE,   ENTDATE,GUBUN, DTLGUBN )";
                    SQL = SQL + ComNum.VBLF + "VALUES ";
                    SQL = SQL + ComNum.VBLF + "         (";
                    SQL = SQL + ComNum.VBLF + "         '" + txtPtNo.Text + "','" + VB.Left(cboDept.Text, 2) + "', '" + VB.Left(cboIO.Text, 1) + "', ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpCanDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtILLCode.Text + "', ";

                    if(chkJinDan0.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
                    if(chkJinDan1.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
                    if(chkJinDan2.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
                    if(chkJinDan3.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
                    if(chkJinDan4.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }

                    SQL = SQL + ComNum.VBLF + "         '" + txtRemark.Text + "' , ";

                    if(chkJinDan5.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
                    if(chkJinDan6.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
                    if(chkJinDan7.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
                    if(chkJinDan8.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
                    if(chkJinDan9.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }
                    if(chkJinDan10.Checked == true) { SQL = SQL + ComNum.VBLF + "         '1' , "; } else { SQL = SQL + ComNum.VBLF + "         '0', "; }

                    SQL = SQL + ComNum.VBLF + "         '" + txtRemark2.Text + "' , '" + VB.Left(cboDr.Text, 4) + "', ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtDongSName.Text + "', TO_DATE('" + dtpDongDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtSinName.Text + "' , '" + txtSTel.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboGan.Text, 2) + "' ,'','',SYSDATE,'" + strGu + "','" + strGbn + "' ";
                    SQL = SQL + ComNum.VBLF + "         ) ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if(SqlErr != "")
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
            catch(Exception ex)
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
                this.Height = 971;
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
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;

            if(ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            //FormClear();

            try
            {
                if(VB.Left(cboIO.Text, 1) == "O")
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
                else
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

                    if (ComFunc.MsgBoxQ("주소,휴대전화,자택전화,통보방법(기본SMS)을 기존병원 데이타로 불러오시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
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
                    }
                }

                dt.Dispose();
                dt = null;

                //암 및 희귀난치성환자 신청시 READ
                SQL = "";
                SQL = "SELECT PANO, DEPTCODE , IPDOPD, CANDATE, ILLCODE,Code2, JINDAN1, JINDAN2, JINDAN3, JINDAN4, JINDAN5,";
                SQL = SQL + ComNum.VBLF + "     REMARK1, JINDAN6, JINDAN7, JINDAN8, JINDAN9, JINDAN10, JINDAN11, REMARK2,REMARK3, DRCODE,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SDATE,'YYYY-MM-DD') SDATE,  NAME, TEL, GANGE,PJumin, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SENDDATE,'YYYY-MM-DD') SENDATE, TO_CHAR(RESDATE,'YYYY-MM-DD') RESDATE, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(ASSENTIENTDATE,'YYYY-MM-DD') ASSENTIENTDATE,ASSENTIENT,HPhone,Email,TongboGbn, nvl(DTLGUBN,'0') DTLGUBN,";
                SQL = SQL + ComNum.VBLF + "     ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_CANCER_DTL ";
                SQL = SQL + ComNum.VBLF + "   WHERE PANO = '" + txtPtNo.Text + "' ";

                if(rdo0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     AND GUBUN ='1' ";       // 암
                }
                else if(rdo1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     AND GUBUN ='2' ";       // 희귀난치
                }
                else if(rdo2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     AND GUBUN ='3' ";       // 중증화상
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 0)
                {
                    ComFunc.ComboFind(cboDept, "L", 2, dt.Rows[0]["DEPTCODE"].ToString().Trim());

                    SQL = "";
                    SQL = "SELECT DRCODE, DRNAME FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                    SQL = SQL + ComNum.VBLF + " WHERE DRDEPT1 = '" + dt.Rows[0]["DEPTCODE"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND TOUR ='N'";
                    SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if(SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if(dt1.Rows.Count > 0)
                    {
                        cboDr.Items.Clear();
                        cboDr.Items.Add("");

                        for(i = 0; i < dt1.Rows.Count; i++)
                        {
                            cboDr.Items.Add(dt1.Rows[i]["DRCODE"].ToString().Trim() + "." + dt1.Rows[i]["DRNAME"].ToString().Trim());
                        }

                        cboDr.SelectedIndex = 0;
                    }

                    dt1.Dispose();
                    dt1 = null;

                    ComFunc.ComboFind(cboDr, "L", 4, dt.Rows[0]["DRCODE"].ToString().Trim());
                    ComFunc.ComboFind(cboIO, "L", 1, dt.Rows[0]["IPDOPD"].ToString().Trim());

                    if(dt.Rows[0]["JINDAN1"].ToString().Trim() == "1") { chkJinDan0.Checked = true; }
                    if(dt.Rows[0]["JINDAN2"].ToString().Trim() == "1") { chkJinDan1.Checked = true; }
                    if(dt.Rows[0]["JINDAN3"].ToString().Trim() == "1") { chkJinDan2.Checked = true; }
                    if(dt.Rows[0]["JINDAN4"].ToString().Trim() == "1") { chkJinDan3.Checked = true; }
                    if(dt.Rows[0]["JINDAN5"].ToString().Trim() == "1") { chkJinDan4.Checked = true; }
                    if(dt.Rows[0]["JINDAN6"].ToString().Trim() == "1") { chkJinDan5.Checked = true; }
                    if(dt.Rows[0]["JINDAN7"].ToString().Trim() == "1") { chkJinDan6.Checked = true; }
                    if(dt.Rows[0]["JINDAN8"].ToString().Trim() == "1") { chkJinDan7.Checked = true; }
                    if(dt.Rows[0]["JINDAN9"].ToString().Trim() == "1") { chkJinDan8.Checked = true; }
                    if(dt.Rows[0]["JINDAN10"].ToString().Trim() == "1") { chkJinDan9.Checked = true; }

                    if (rdo0.Checked == true || rdo3.Checked == true)
                    {
                        if(dt.Rows[0]["JINDAN11"].ToString().Trim() == "1") { chkJinDan10.Checked = true; }
                    }

                    if (rdo0.Checked == true)
                    {
                        switch (dt.Rows[0]["DTLGUBN"].ToString().Trim())
                        {
                            case "0": rdoGU0.Checked = true; break;
                            case "1": rdoGU1.Checked = true; break;
                            case "2": rdoGU2.Checked = true; break;
                        }
                    }

                    txtRemark.Text = dt.Rows[0]["REMARK1"].ToString().Trim();
                    txtRemark2.Text = dt.Rows[0]["REMARK2"].ToString().Trim();
                    txtRemark3.Text = dt.Rows[0]["REMARK3"].ToString().Trim();

                    txtILLCode.Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                    txtIllName.Text = Read_Bas_ILL(txtILLCode.Text);

                    //중증화상
                    txtCode2.Text = dt.Rows[0]["CODE2"].ToString().Trim();

                    dtpDongDate.Value = Convert.ToDateTime(VB.IIf(dt.Rows[0]["ASSENTIENTDATE"].ToString().Trim() == "", ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), dt.Rows[0]["ASSENTIENTDATE"].ToString().Trim()));
                    txtDongSName.Text = dt.Rows[0]["ASSENTIENT"].ToString().Trim();

                    if (dt.Rows[0]["CANDATE"].ToString().Trim() != "")
                    {
                        dtpCanDate.Value = Convert.ToDateTime(dt.Rows[0]["CANDATE"].ToString().Trim());
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

                    switch(dt.Rows[0]["TONGBOGBN"].ToString().Trim())
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

                //중증등록여부표시-원내
                SQL = "";
                SQL = "SELECT Gubun FROM ADMIN.BAS_CANCER";
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
                SQL = "SELECT ADMIN.SEQ_OPD_NHIC.NEXTVAL WRTNO FROM DUAL";

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
            //중증암, 희귀난치 설정함
            if(rdo0.Checked == true)
            {
                rdo0.BackColor = Color.FromArgb(192, 255, 192);
                rdo1.BackColor = Color.White;
                rdo2.BackColor = Color.White;
                rdo3.BackColor = Color.White;
                rdo4.BackColor = Color.White;

                label18.Text = "암 확인일";

                chkJinDan5.Text = "② 조직검사 없는 진단적 수술";
                chkJinDan6.Text = "③ 특수 생화학적 또는 면역학적 검사";

                chkJinDan7.Text = "④ 세포학적 또는 혈액학적 검사";
                chkJinDan8.Text = "⑤ 전이부위의 조직학적 검사";

                chkJinDan9.Visible = true;
                chkJinDan10.Visible = true;
                chkJinDan9.Text = "⑥ 원발부위의 조직학적 생검";
                chkJinDan10.Text = "⑦ 기타";

                txtRemark2.Visible = true;
                txtRemark2.Location = new Point(239, 118);

                txtRemark3.Visible = true;

                grbCode2.Visible = false;
            }
            else if(rdo1.Checked == true)
            {
                rdo0.BackColor = Color.White;
                rdo1.BackColor = Color.FromArgb(255, 192, 255);
                rdo2.BackColor = Color.White;
                rdo3.BackColor = Color.White;
                rdo4.BackColor = Color.White;

                label18.Text = "희귀확인일";

                chkJinDan5.Text = "② 특수 생화학/면역학적검사,도말/배양검사";
                chkJinDan6.Text = "③ 유전학적 검사";

                chkJinDan7.Text = "④ 조직학적 검사";
                chkJinDan8.Text = "⑤ 임상적 소견으로 최종진단 시 기재";

                chkJinDan9.Visible = false;
                chkJinDan10.Visible = true;
                chkJinDan10.Text = "⑦ 기타";

                txtRemark2.Visible = true;
                txtRemark2.Location = new Point(75, 118);

                txtRemark3.Visible = true;

                grbCode2.Visible = false;
            }
            else if(rdo2.Checked == true)
            {
                rdo0.BackColor = Color.White;
                rdo1.BackColor = Color.White;
                rdo2.BackColor = Color.Red;
                rdo3.BackColor = Color.White;
                rdo4.BackColor = Color.White;

                label18.Text = "화상확인일";

                chkJinDan5.Text = "② 특수 생화학/면역학적검사,도말/배양검사";
                chkJinDan6.Text = "③ 유전학적 검사";

                chkJinDan7.Text = "④ 조직학적 검사";
                chkJinDan8.Text = "⑤ 임상적 소견으로 최종진단 시 기재";

                chkJinDan9.Visible = false;
                chkJinDan10.Visible = false;

                txtRemark2.Visible = true;
                txtRemark2.Location = new Point(75, 118);

                txtRemark3.Visible = false;

                grbCode2.Visible = true;
            }
            else if(rdo3.Checked == true)
            {
                rdo0.BackColor = Color.White;
                rdo1.BackColor = Color.White;
                rdo2.BackColor = Color.White;
                rdo3.BackColor = Color.FromArgb(255, 192, 255);
                rdo4.BackColor = Color.White;

                label18.Text = "희귀확인일";

                chkJinDan5.Text = "② 특수 생화학/면역학적검사,도말/배양검사";
                chkJinDan6.Text = "③ 유전학적 검사";

                chkJinDan7.Text = "④ 조직학적 검사";
                chkJinDan8.Text = "⑤ 임상적 소견으로 최종진단 시 기재";

                chkJinDan9.Visible = false;
                chkJinDan10.Visible = true;
                chkJinDan10.Text = "⑦ 기타";

                txtRemark2.Visible = true;
                txtRemark2.Location = new Point(75, 118);

                txtRemark3.Visible = false;

                grbCode2.Visible = false;
            }
            else if (rdo4.Checked == true)
            {
                rdo0.BackColor = Color.White;
                rdo1.BackColor = Color.White;
                rdo2.BackColor = Color.White;
                rdo3.BackColor = Color.White;
                rdo4.BackColor = Color.FromArgb(255, 192, 255);

                label18.Text = "희귀확인일";

                chkJinDan5.Text = "② 특수 생화학/면역학적검사,도말/배양검사";
                chkJinDan6.Text = "③ 유전학적 검사";

                chkJinDan7.Text = "④ 조직학적 검사";
                chkJinDan8.Text = "⑤ 임상적 소견으로 최종진단 시 기재";

                chkJinDan9.Visible = false;
                chkJinDan10.Visible = true;
                chkJinDan10.Text = "⑦ 기타";

                txtRemark2.Visible = true;
                txtRemark2.Location = new Point(75, 118);

                txtRemark3.Visible = false;

                grbCode2.Visible = false;
            }

            //txtPtNoEnter();
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDrCombo();
        }

        private void SetDrCombo()
        {
            if (VB.Split(cboDept.Text.Trim(), "전체").Length > 1) { return; }

            if (cboDept.Text.Trim() == "")
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

                SQL = "";
                SQL = "SELECT DRCODE, DRNAME FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                SQL = SQL + ComNum.VBLF + " WHERE DRDEPT1 = '" + VB.Left(cboDept.Text, 2) + "' ";
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

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDr.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim());
                    }

                    cboDr.SelectedIndex = 0;
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
                    SQL = "SELECT ROWID FROM ADMIN.BAS_CANCER";
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
                if(rdo1.Checked == true || rdo2.Checked == true)
                {
                    ComFunc.MsgBox("중증암을 체크하고 인쇄하세요.");
                    return;
                }
            }
            else if(strIndex == "2")
            {
                if(rdo1.Checked == true || rdo2.Checked == true)
                {
                    ComFunc.MsgBox("희귀난치질환을 체크하고 인쇄하세요.");
                    return;
                }
            }
            else if(strIndex == "3")
            {
                if(rdo0.Checked == true || rdo1.Checked == true)
                {
                    ComFunc.MsgBox("중증화상을 체크하고 인쇄하세요.");
                    return;
                }
            }
            else if(strIndex == "4")
            {
                if (rdo4.Checked == false)
                {
                    ComFunc.MsgBox("결핵을 체크하고 인쇄하세요.");
                    return;
                }
            }

            //if (GstrBi == "21" || GstrBi == "22")
            //{
            //    ComFunc.MsgBox("자격이 의료급여입니다.. 신청서를 급여신청으로 출력하십시오.");
            //    return;
            //}
            
            DATA_SET3_new2(strIndex);

            ssBohum1new_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssBohum1new_Sheet1.PrintInfo.Margin.Top = 60;
            ssBohum1new_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssBohum1new_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssBohum1new_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssBohum1new_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssBohum1new_Sheet1.PrintInfo.ShowBorder = true;
            ssBohum1new_Sheet1.PrintInfo.ShowColor = false;
            ssBohum1new_Sheet1.PrintInfo.ShowGrid = true;
            ssBohum1new_Sheet1.PrintInfo.ShowShadows = false;
            ssBohum1new_Sheet1.PrintInfo.UseMax = false;
            ssBohum1new_Sheet1.PrintInfo.UseSmartPrint = false;
            ssBohum1new_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssBohum1new_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssBohum1new_Sheet1.PrintInfo.Preview = true;
            ssBohum1new.PrintSheet(0);

            ComFunc.Delay(200);

            ssBohum1new2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssBohum1new2_Sheet1.PrintInfo.Margin.Top = 60;
            ssBohum1new2_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssBohum1new2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssBohum1new2_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssBohum1new2_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssBohum1new2_Sheet1.PrintInfo.ShowBorder = true;
            ssBohum1new2_Sheet1.PrintInfo.ShowColor = false;
            ssBohum1new2_Sheet1.PrintInfo.ShowGrid = true;
            ssBohum1new2_Sheet1.PrintInfo.ShowShadows = false;
            ssBohum1new2_Sheet1.PrintInfo.UseMax = false;
            ssBohum1new2_Sheet1.PrintInfo.UseSmartPrint = false;
            ssBohum1new2_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssBohum1new2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssBohum1new2_Sheet1.PrintInfo.Preview = false;
            ssBohum1new2.PrintSheet(0);
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

            ssBohum1new_Sheet1.Cells[3, 2].Text = txtGKiho.Text;
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

        private void toolGub_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strIndex = VB.Right(((ToolStripMenuItem)sender).Name, 1);
            
            if (strIndex == "1")
            {
                if (rdo0.Checked == false)
                {
                    ComFunc.MsgBox("중증암을 체크하고 인쇄하세요.");
                    return;
                }
            }
            else if (strIndex == "2")
            {
                if(rdo3.Checked == false)
                {
                    ComFunc.MsgBox("희귀난치를 체크하고 인쇄하세요.");
                    return;
                }
            }
            else if (strIndex == "3")
            {
                if(rdo2.Checked == false)
                {
                    ComFunc.MsgBox("중증화상을 체크하고 인쇄하세요.");
                    return;
                }
            }
            else if(strIndex == "4")
            {
                if(rdo1.Checked == false)
                {
                    ComFunc.MsgBox("기타 산정특례를 체크하고 인쇄하세요.");
                    return;
                }
            }
            else if (strIndex == "5")
            {
                if(rdo4.Checked == false)
                {
                    ComFunc.MsgBox("결핵을 체크하고 인쇄하세요.");
                    return;
                }
            }

            if(GstrBi != "21" && GstrBi != "22")
            {
                ComFunc.MsgBox("자격이 의료급여가 아닙니다.. 신청서를 건강보험양식으로 출력하십시오");
                return;
            }

            DATA_Gubnew_TOTAL_SET(strIndex);

            ssGubnew2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssGubnew2_Sheet1.PrintInfo.Margin.Top = 60;
            ssGubnew2_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssGubnew2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssGubnew2_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssGubnew2_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssGubnew2_Sheet1.PrintInfo.ShowBorder = true;
            ssGubnew2_Sheet1.PrintInfo.ShowColor = false;
            ssGubnew2_Sheet1.PrintInfo.ShowGrid = true;
            ssGubnew2_Sheet1.PrintInfo.ShowShadows = false;
            ssGubnew2_Sheet1.PrintInfo.UseMax = false;
            ssGubnew2_Sheet1.PrintInfo.UseSmartPrint = false;
            ssGubnew2_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssGubnew2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssGubnew2_Sheet1.PrintInfo.Preview = false;
            ssGubnew2.PrintSheet(0);

            ComFunc.Delay(200);

            ssGubnew3_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssGubnew3_Sheet1.PrintInfo.Margin.Top = 60;
            ssGubnew3_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssGubnew3_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssGubnew3_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssGubnew3_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssGubnew3_Sheet1.PrintInfo.ShowBorder = true;
            ssGubnew3_Sheet1.PrintInfo.ShowColor = false;
            ssGubnew3_Sheet1.PrintInfo.ShowGrid = true;
            ssGubnew3_Sheet1.PrintInfo.ShowShadows = false;
            ssGubnew3_Sheet1.PrintInfo.UseMax = false;
            ssGubnew3_Sheet1.PrintInfo.UseSmartPrint = false;
            ssGubnew3_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssGubnew3_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssGubnew3_Sheet1.PrintInfo.Preview = false;
            ssGubnew3.PrintSheet(0);
        }

        private void DATA_Gubnew_TOTAL_SET(string strIndex)
        {
            string strOK = "";

            switch(strIndex)
            {
                case "1":
                    ssGubnew2_Sheet1.Cells[1, 1].Text = "( ■ 암 □ 희귀난치 □ 결핵 □ 중증화상 □ 기타 산정특례질환 )";
                    break;
                case "2":
                    ssGubnew2_Sheet1.Cells[1, 1].Text = "( □ 암 ■ 희귀난치 □ 결핵 □ 중증화상 □ 기타 산정특례질환 )";
                    break;
                case "3":
                    ssGubnew2_Sheet1.Cells[1, 1].Text = "( □ 암 □ 희귀난치 □ 결핵 ■ 중증화상 □ 기타 산정특례질환 )";
                    break;
                case "4":
                    ssGubnew2_Sheet1.Cells[1, 1].Text = "( □ 암 □ 희귀난치 □ 결핵 □ 중증화상 ■ 기타 산정특례질환 )";
                    break;
                case "5":
                    ssGubnew2_Sheet1.Cells[1, 1].Text = "( □ 암 □ 희귀난치 ■ 결핵 □ 중증화상 □ 기타 산정특례질환 )";
                    break;
            }

            ssGubnew2_Sheet1.Cells[3, 3].Text = txtPName.Text;
            ssGubnew2_Sheet1.Cells[3, 8].Text = txtPJumin.Text.Replace("-", "");

            ssGubnew2_Sheet1.Cells[4, 3].Text = txtSName.Text;
            ssGubnew2_Sheet1.Cells[4, 8].Text = txtJumin.Text.Replace("-", "");

            ssGubnew2_Sheet1.Cells[5, 3].Text = VB.Right(txtJuso.Text, 32).Trim();
            ssGubnew2_Sheet1.Cells[5, 9].Text = txtTel.Text;

            ssGubnew2_Sheet1.Cells[8, 3].Text = VB.Left(cboDept.Text, 2) + "(" + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2)) + ")";
            ssGubnew2_Sheet1.Cells[8, 7].Text = VB.Mid(cboIO.Text, 3, cboIO.Text.Length);
            ssGubnew2_Sheet1.Cells[8, 10].Text = dtpCanDate.Value.ToString("yyyy.MM.dd");

            ssGubnew2_Sheet1.Cells[10, 3].Text = txtIllName.Text;

            if (txtILLCode.Text != "")
            {
                ssGubnew2_Sheet1.Cells[11, 3].Text = " " + txtILLCode.Text + " ";
            }
            else
            {
                ssGubnew2_Sheet1.Cells[11, 3].Text = "";
            }

            switch(strIndex)
            {
                case "1":
                    ssGubnew2_Sheet1.Cells[13, 1].Text = "■ 암";
                    break;
                case "2":
                    ssGubnew2_Sheet1.Cells[14, 1].Text = " □ ① 검사 □Sono □CT □MRI □기타(             )";
                    ssGubnew2_Sheet1.Cells[15, 1].Text = " □ ② 조직검사 없는 진단적 수술";
                    ssGubnew2_Sheet1.Cells[16, 1].Text = " □ ③ 특수 생화학적 또는 면역학적검사";
                    ssGubnew2_Sheet1.Cells[17, 1].Text = " □ ④ 세포학적 또는 혈액학적 검사";
                    ssGubnew2_Sheet1.Cells[18, 1].Text = " □ ⑤ 전이부위의 조직학적 검사";
                    ssGubnew2_Sheet1.Cells[19, 1].Text = " □ ⑥ 원발부위의 조직학적 생검";
                    ssGubnew2_Sheet1.Cells[20, 1].Text = " □ ⑦ 기타";

                    strOK = "";

                    ssGubnew2_Sheet1.Cells[13, 7].Text = "■ 희귀난치 □ 중증화상 □ 결핵";
                    break;
                case "3":
                    ssGubnew2_Sheet1.Cells[14, 1].Text = " □ ① 검사 □Sono □CT □MRI □기타(             )";
                    ssGubnew2_Sheet1.Cells[15, 1].Text = " □ ② 조직검사 없는 진단적 수술";
                    ssGubnew2_Sheet1.Cells[16, 1].Text = " □ ③ 특수 생화학적 또는 면역학적검사";
                    ssGubnew2_Sheet1.Cells[17, 1].Text = " □ ④ 세포학적 또는 혈액학적 검사";
                    ssGubnew2_Sheet1.Cells[18, 1].Text = " □ ⑤ 전이부위의 조직학적 검사";
                    ssGubnew2_Sheet1.Cells[19, 1].Text = " □ ⑥ 원발부위의 조직학적 생검";
                    ssGubnew2_Sheet1.Cells[20, 1].Text = " □ ⑦ 기타";

                    strOK = "";

                    ssGubnew2_Sheet1.Cells[13, 7].Text = "□ 희귀난치 ■ 중증화상 □ 결핵";
                    break;
                case "4":
                    ssGubnew2_Sheet1.Cells[14, 1].Text = " □ ① 검사 □Sono □CT □MRI □기타(             )";
                    ssGubnew2_Sheet1.Cells[15, 1].Text = " □ ② 조직검사 없는 진단적 수술";
                    ssGubnew2_Sheet1.Cells[16, 1].Text = " □ ③ 특수 생화학적 또는 면역학적검사";
                    ssGubnew2_Sheet1.Cells[17, 1].Text = " □ ④ 세포학적 또는 혈액학적 검사";
                    ssGubnew2_Sheet1.Cells[18, 1].Text = " □ ⑤ 전이부위의 조직학적 검사";
                    ssGubnew2_Sheet1.Cells[19, 1].Text = " □ ⑥ 원발부위의 조직학적 생검";
                    ssGubnew2_Sheet1.Cells[20, 1].Text = " □ ⑦ 기타";

                    strOK = "";

                    ssGubnew2_Sheet1.Cells[13, 1].Text = "□ 암";
                    ssGubnew2_Sheet1.Cells[13, 7].Text = "□ 희귀난치";
                    break;
                case "5":
                    ssGubnew2_Sheet1.Cells[14, 1].Text = " □ ① 검사 □Sono □CT □MRI □기타(             )";
                    ssGubnew2_Sheet1.Cells[15, 1].Text = " □ ② 조직검사 없는 진단적 수술";
                    ssGubnew2_Sheet1.Cells[16, 1].Text = " □ ③ 특수 생화학적 또는 면역학적검사";
                    ssGubnew2_Sheet1.Cells[17, 1].Text = " □ ④ 세포학적 또는 혈액학적 검사";
                    ssGubnew2_Sheet1.Cells[18, 1].Text = " □ ⑤ 전이부위의 조직학적 검사";
                    ssGubnew2_Sheet1.Cells[19, 1].Text = " □ ⑥ 원발부위의 조직학적 생검";
                    ssGubnew2_Sheet1.Cells[20, 1].Text = " □ ⑦ 기타";

                    strOK = "";

                    ssGubnew2_Sheet1.Cells[13, 7].Text = "□ 희귀난치 □ 중증화상 ■ 결핵";
                    break;
            }

            switch(strIndex)
            {
                case "1":
                    if(chkJinDan0.Checked == true) { strOK = strOK + " ■ ①검사"; } else { strOK = strOK + " □ ①검사"; }
                    break;
                case "2":
                case "3":
                case "4":
                    if(chkJinDan5.Checked == true) { strOK = strOK + " ■ ①영상검사"; } else { strOK = strOK + " □ ①영상검사"; }
                    break;
                case "5":
                    if(chkJinDan0.Checked == true) { strOK = strOK + " ■ ①영상검사"; } else { strOK = strOK + " □ ①영상검사"; }
                    break;
            }

            if(chkJinDan1.Checked == true) { strOK = strOK + " ■Sono"; } else { strOK = strOK + " □Sono"; }
            if(chkJinDan2.Checked == true) { strOK = strOK + " ■CT"; } else { strOK = strOK + " □CT"; }
            if(chkJinDan3.Checked == true) { strOK = strOK + " ■MRI"; } else { strOK = strOK + " □MRI"; }
            if(chkJinDan4.Checked == true) { strOK = strOK + " ■기타(" + txtRemark.Text + ")"; } else { strOK = strOK + " □기타(" + VB.Space(5) + ")"; }

            switch(strIndex)
            {
                case "1":
                    ssGubnew2_Sheet1.Cells[14, 1].Text = strOK;

                    if (chkJinDan5.Checked == true) { ssGubnew2_Sheet1.Cells[15, 1].Text = " ■ ② 조직검사 없는 진단적 수술"; } else { ssGubnew2_Sheet1.Cells[15, 1].Text = " □ ② 조직검사 없는 진단적 수술"; }
                    if (chkJinDan6.Checked == true) { ssGubnew2_Sheet1.Cells[16, 1].Text = " ■ ③ 특수 생화학 또는 면역학적검사"; } else { ssGubnew2_Sheet1.Cells[16, 1].Text = " □ ③ 특수 생화학 또는 면역학적검사"; }
                    if (chkJinDan7.Checked == true) { ssGubnew2_Sheet1.Cells[17, 1].Text = " ■ ④ 세포학적 또는 혈액학적 검사"; } else { ssGubnew2_Sheet1.Cells[17, 1].Text = " □ ④ 세포학적 또는 혈액학적 검사"; }
                    if (chkJinDan8.Checked == true) { ssGubnew2_Sheet1.Cells[18, 1].Text = " ■ ⑤ 전이부위의 조직학적 검사"; } else { ssGubnew2_Sheet1.Cells[18, 1].Text = " □ ⑤ 전이부위의 조직학적 검사"; }
                    if (chkJinDan9.Checked == true) { ssGubnew2_Sheet1.Cells[19, 1].Text = " ■ ⑥ 원발부위의 조직학적 생검"; } else { ssGubnew2_Sheet1.Cells[19, 1].Text = " □ ⑥ 원발부위의 조직학적 생검"; }

                    strOK = "";

                    if (chkJinDan9.Checked == true) { strOK = strOK + " ■ ⑦ 기타"; } else { strOK = strOK + " ■ ⑦ 기타"; }
                    if (txtRemark2.Text != "") { strOK = strOK + "(" + txtRemark2.Text + ")"; } else { strOK = strOK + "(" + VB.Space(20) + ")"; }

                    ssGubnew2_Sheet1.Cells[20, 1].Text = strOK;

                    ssGubnew2_Sheet1.Cells[13, 7].Text = "□ 희귀난치 □ 중증화상 □ 결핵";

                    ssGubnew2_Sheet1.Cells[14, 7].Text = " □ ① 영상검사 □Sono □CT □MRI □기타(             )";
                    ssGubnew2_Sheet1.Cells[15, 7].Text = " □ ② 특수 생화학/면역학적검사,도말/배양검사 등";
                    ssGubnew2_Sheet1.Cells[16, 7].Text = " □ ③ 유전학적 검사";
                    ssGubnew2_Sheet1.Cells[17, 7].Text = " □ ④ 조직학적 검사";
                    ssGubnew2_Sheet1.Cells[18, 7].Text = " □ ⑤ 임상적 소견으로 최종 진단 시 기재";
                    ssGubnew2_Sheet1.Cells[20, 7].Text = " □ ⑥ 기타(                     검사)";
                    break;
                case "2":
                case "3":
                case "4":
                case "5":
                    ssGubnew2_Sheet1.Cells[14, 7].Text = strOK;

                    if (chkJinDan5.Checked == true) { ssGubnew2_Sheet1.Cells[15, 7].Text = " ■ ② 특수 생화학/면역학적검사,도말/배양검사 등"; } else { ssGubnew2_Sheet1.Cells[15, 7].Text = " □ ② 특수 생화학/면역학적검사,도말/배양검사 등"; }
                    if (chkJinDan6.Checked == true) { ssGubnew2_Sheet1.Cells[16, 7].Text = " ■ ③ 유전학적 검사"; } else { ssGubnew2_Sheet1.Cells[16, 7].Text = " □ ③ 유전학적 검사"; }
                    if (chkJinDan7.Checked == true) { ssGubnew2_Sheet1.Cells[17, 7].Text = " ■ ④ 조직학적 검사"; } else { ssGubnew2_Sheet1.Cells[17, 7].Text = " □ ④ 조직학적 검사"; }
                    if (chkJinDan8.Checked == true) { ssGubnew2_Sheet1.Cells[18, 7].Text = " ■ ⑤ 임상적 소견으로 최종 진단 시 기재"; } else { ssGubnew2_Sheet1.Cells[18, 7].Text = " □ ⑤ 임상적 소견으로 최종 진단 시 기재"; }

                    if (txtRemark2.Text != "") { ssGubnew2_Sheet1.Cells[19, 7].Text = "(" + txtRemark2.Text + ")"; } else { ssGubnew2_Sheet1.Cells[19, 7].Text = ""; }

                    if (strIndex != "5")
                    {
                        if (chkJinDan10.Checked == true)
                        {
                            if (txtRemark3.Text != "")
                            {
                                ssGubnew2_Sheet1.Cells[21, 7].Text = " ■ ⑥ 기타(" + txtRemark3.Text.Trim() + ")";
                            }
                            else
                            {
                                ssGubnew2_Sheet1.Cells[21, 7].Text = " ■ ⑥ 기타";
                            }
                        }
                        else
                        {
                            ssGubnew2_Sheet1.Cells[21, 7].Text = " □ ⑥ 기타(" + VB.Space(20) + "검사)";
                        }
                    }
                    else
                    {
                        if (chkJinDan10.Checked == true)
                        {
                            if (txtRemark3.Text != "")
                            {
                                ssGubnew2_Sheet1.Cells[20, 7].Text = " ■ ⑥ 기타(" + txtRemark3.Text.Trim() + ")";
                            }
                            else
                            {
                                ssGubnew2_Sheet1.Cells[20, 7].Text = " ■ ⑥ 기타";
                            }
                        }
                        else
                        {
                            ssGubnew2_Sheet1.Cells[20, 7].Text = " □ ⑥ 기타(" + VB.Space(20) + "검사)";
                        }
                    }
                    break;
            }

            if (chkT0.Checked == true) { ssGubnew2_Sheet1.Cells[22, 1].Text = " ■ 뇌혈관질환"; } else { ssGubnew2_Sheet1.Cells[22, 1].Text = " □ 뇌혈관질환"; }
            if (chkT1.Checked == true) { ssGubnew2_Sheet1.Cells[22, 3].Text = " ■ 심장질환"; } else { ssGubnew2_Sheet1.Cells[22, 3].Text = " □ 심장질환"; }
            if (chkT2.Checked == true) { ssGubnew2_Sheet1.Cells[22, 5].Text = " ■ 중증외상"; } else { ssGubnew2_Sheet1.Cells[22, 5].Text = " □ 중증외상"; }

            ssGubnew2_Sheet1.Cells[25, 6].Text = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).ToString("yyyy 년 MM 월 dd 일");

            if (cboDr.Text.Trim() != "")
            {
                ssGubnew2_Sheet1.Cells[28, 6].Text = VB.Split(cboDr.Text, ".")[1];
            }
            else
            {
                ssGubnew2_Sheet1.Cells[28, 6].Text = "";
            }

            if (cboDr.Text.Trim() != "")
            {
                ssGubnew2_Sheet1.Cells[28, 6].Text = ssGubnew2_Sheet1.Cells[28, 6].Text + "(" + clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0]) + ")";
            }
            else
            {
                ssGubnew2_Sheet1.Cells[28, 6].Text = ssGubnew2_Sheet1.Cells[28, 6].Text + "(" + VB.Space(10) + ")";
            }

            ssGubnew2_Sheet1.Cells[31, 4].Text = txtDongSName.Text;
            ssGubnew2_Sheet1.Cells[31, 6].Text = dtpDongDate.Value.ToString("yyyy 년 MM 월 dd 일");

            ssGubnew2_Sheet1.Cells[35, 6].Text = dtpSDate.Value.ToString("yyyy 년 MM 월 dd 일");

            ssGubnew2_Sheet1.Cells[37, 4].Text = txtSinName.Text;
            ssGubnew2_Sheet1.Cells[37, 9].Text = txtTel.Text;

            ssGubnew2_Sheet1.Cells[38, 10].Text = VB.Mid(cboGan.Text, 4, cboGan.Text.Length);

            ssGubnew2_Sheet1.Cells[42, 1].Text = "등록번호 : " + txtPtNo.Text;
        }

        private void toolOP_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            FarPoint.Win.Spread.FpSpread ssSpread = null;
            string strIndex = VB.Right(((ToolStripMenuItem)sender).Name, 1);

            switch (strIndex)
            {
                case "1":
                    ssSpread = ssOP1;
                    break;
                case "2":
                    ssSpread = ssOP2;
                    break;
                case "3":
                    ssSpread = ssOP3;
                    break;
                case "4":
                    ssSpread = ssOP4;
                    break;
                case "5":
                    ssSpread = ssOP5;
                    break;
            }

            ssSpread.ActiveSheet.Cells[0, 5].Text = txtPtNo.Text;
            ssSpread.ActiveSheet.Cells[1, 5].Text = txtSex.Text + "/" + txtAge.Text;
            ssSpread.ActiveSheet.Cells[2, 5].Text = txtSName.Text;

            ssSpread.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssSpread.ActiveSheet.PrintInfo.Margin.Top = 60;
            ssSpread.ActiveSheet.PrintInfo.Margin.Bottom = 20;
            ssSpread.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssSpread.ActiveSheet.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssSpread.ActiveSheet.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssSpread.ActiveSheet.PrintInfo.ShowBorder = false;
            ssSpread.ActiveSheet.PrintInfo.ShowColor = false;
            ssSpread.ActiveSheet.PrintInfo.ShowGrid = true;
            ssSpread.ActiveSheet.PrintInfo.ShowShadows = false;
            ssSpread.ActiveSheet.PrintInfo.UseMax = false;
            ssSpread.ActiveSheet.PrintInfo.UseSmartPrint = false;
            ssSpread.ActiveSheet.PrintInfo.ShowPrintDialog = false;
            ssSpread.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssSpread.ActiveSheet.PrintInfo.Preview = false;
            ssSpread.PrintSheet(0);
        }

        private void toolBst1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("정말로 당뇨 등록신청서를 인쇄를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string strFile = "";
            string strSabun = "";
            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            ssDiabetes1_Sheet1.Cells[3, 3].Text = "";
            ssDiabetes1_Sheet1.Cells[3, 8].Text = "";

            ssDiabetes1_Sheet1.Cells[4, 3].Text = "";

            ssDiabetes1_Sheet1.Cells[5, 3].Text = "";

            ssDiabetes1_Sheet1.Cells[3, 3].Text = txtSName.Text.Trim();
            ssDiabetes1_Sheet1.Cells[3, 8].Text = txtJumin.Text.Trim();

            ssDiabetes1_Sheet1.Cells[4, 3].Text = txtTel.Text.Trim();

            ssDiabetes1_Sheet1.Cells[5, 4].Text = txtHPhone.Text.Trim();

            ssDiabetes1_Sheet1.Cells[7, 3].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2));

            ssDiabetes1_Sheet1.Cells[27, 2].Text = Convert.ToDateTime(strDate).ToString("yyyy 년 MM 월 dd 일");

            if (cboDr.Text.Trim() != "")
            {
                if (cboDr.Text.Replace(".", "").Trim() != "")
                {
                    strSabun = VB.Left(cboDr.Text, 4);

                    ssDiabetes1_Sheet1.Cells[29, 6].Text = VB.Split(cboDr.Text, ".")[1] + "(" + clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0]) + ")";
                }
                else
                {
                    ssDiabetes1_Sheet1.Cells[29, 6].Text = VB.Split(cboDr.Text, ".")[1] + "(" + VB.Space(5) + ")";
                }
            }

            ssDiabetes1_Sheet1.Cells[33, 1].Text = Convert.ToDateTime(strDate).ToString("yyyy 년 MM 월 dd 일");

            ssDiabetes1_Sheet1.Cells[45, 2].Text = txtPtNo.Text;
            ssDiabetes1_Sheet1.Cells[45, 5].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2));
            ssDiabetes1_Sheet1.Cells[45, 7].Text = txtJuso.Text;

            //의사 사인 이미지 로드 ------------------------------------------------------------------------------------
            if (strSabun != "")
            {
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                ssDiabetes1_Sheet1.Cells[29, 9].CellType = imgCellType;
                ssDiabetes1_Sheet1.Cells[29, 9].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssDiabetes1_Sheet1.Cells[29, 9].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(strSabun, 5, "0") + ".jpg";

                Image image = SIGNATUREFILE_DBToFile(strSabun);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    ssDiabetes1_Sheet1.Cells[29, 9].Value = image;
                }
            }
            else
            {
                FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                textCellType.WordWrap = false;
                textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                ssDiabetes1_Sheet1.Cells[29, 9].CellType = textCellType;
                ssDiabetes1_Sheet1.Cells[29, 9].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssDiabetes1_Sheet1.Cells[29, 9].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssDiabetes1_Sheet1.Cells[29, 9].Text = "(서명 또는 인)";
            }

            ssDiabetes1_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssDiabetes1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssDiabetes1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes1_Sheet1.PrintInfo.Margin.Top = 60;
            ssDiabetes1_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssDiabetes1_Sheet1.PrintInfo.ShowColor = false;
            ssDiabetes1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssDiabetes1_Sheet1.PrintInfo.ShowBorder = false;
            ssDiabetes1_Sheet1.PrintInfo.ShowGrid = true;
            ssDiabetes1_Sheet1.PrintInfo.ShowShadows = false;
            ssDiabetes1_Sheet1.PrintInfo.UseMax = true;
            ssDiabetes1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDiabetes1_Sheet1.PrintInfo.UseSmartPrint = false;
            ssDiabetes1_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssDiabetes1_Sheet1.PrintInfo.Preview = false;
            ssDiabetes1.PrintSheet(0);
        }

        private void toolBst2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("당뇨 소모성 재료 처방전을 인쇄를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string strFile = "";
            string strSabun = "";
            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            ssDiabetes2_Sheet1.Cells[5, 3].Text = "";
            ssDiabetes2_Sheet1.Cells[5, 8].Text = "";

            ssDiabetes2_Sheet1.Cells[6, 3].Text = "";
            ssDiabetes2_Sheet1.Cells[6, 9].Text = "";

            ssDiabetes2_Sheet1.Cells[7, 9].Text = "";

            ssDiabetes2_Sheet1.Cells[5, 3].Text = txtGKiho.Text;
            ssDiabetes2_Sheet1.Cells[5, 8].Text = txtJumin.Text;

            ssDiabetes2_Sheet1.Cells[6, 3].Text = txtSName.Text;
            ssDiabetes2_Sheet1.Cells[6, 9].Text = txtTel.Text;

            ssDiabetes2_Sheet1.Cells[7, 9].Text = txtHPhone.Text;

            ssDiabetes2_Sheet1.Cells[9, 2].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2));

            ssDiabetes2_Sheet1.Cells[27, 4].Text = Convert.ToDateTime(strDate).ToString("yyyy 년 MM 월 dd 일");

            if (cboDr.Text.Trim() != "")
            {
                if (cboDr.Text.Replace(".", "") != "")
                {
                    strSabun = VB.Left(cboDr.Text, 4);
                    ssDiabetes2_Sheet1.Cells[29, 4].Text = VB.Split(cboDr.Text, ".")[1] + "(" + clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0]) + ")";
                }
                else
                {
                    ssDiabetes2_Sheet1.Cells[29, 4].Text = VB.Split(cboDr.Text, ".")[1] + "(" + VB.Space(5) + ")";
                }

                //ssDiabetes2_Sheet1.Cells[30, 4].Text = VB.Split(cboDr.Text, ".")[0];
            }

            //의사 사인 이미지 로드 ------------------------------------------------------------------------------------
            if (strSabun != "")
            {
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                ssDiabetes2_Sheet1.Cells[29, 9].CellType = imgCellType;
                ssDiabetes2_Sheet1.Cells[29, 9].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssDiabetes2_Sheet1.Cells[29, 9].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(strSabun, 5, "0") + ".jpg";

                Image image = SIGNATUREFILE_DBToFile(strSabun);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    ssDiabetes2_Sheet1.Cells[29, 9].Value = image;
                }
            }
            else
            {
                FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                textCellType.WordWrap = false;
                textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                ssDiabetes2_Sheet1.Cells[29, 9].CellType = textCellType;
                ssDiabetes2_Sheet1.Cells[29, 9].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssDiabetes2_Sheet1.Cells[29, 9].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssDiabetes2_Sheet1.Cells[29, 9].Text = "(서명 또는 인)";
            }

            ssDiabetes2_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssDiabetes2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssDiabetes2_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes2_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes2_Sheet1.PrintInfo.Margin.Top = 60;
            ssDiabetes2_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssDiabetes2_Sheet1.PrintInfo.ShowColor = false;
            ssDiabetes2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssDiabetes2_Sheet1.PrintInfo.ShowBorder = false;
            ssDiabetes2_Sheet1.PrintInfo.ShowGrid = true;
            ssDiabetes2_Sheet1.PrintInfo.ShowShadows = false;
            ssDiabetes2_Sheet1.PrintInfo.UseMax = true;
            ssDiabetes2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDiabetes2_Sheet1.PrintInfo.UseSmartPrint = false;
            ssDiabetes2_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssDiabetes2_Sheet1.PrintInfo.Preview = false;
            ssDiabetes2.PrintSheet(0);
        }

        private void toolBst3_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("당뇨 소모성 재료 처방전을 인쇄를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string strFile = "";
            string strSabun = "";
            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            ssDiabetes3_Sheet1.Cells[6, 3].Text = txtGKiho.Text;

            ssDiabetes3_Sheet1.Cells[8, 3].Text = txtSName.Text;
            ssDiabetes3_Sheet1.Cells[8, 13].Text = txtJumin.Text;

            ssDiabetes3_Sheet1.Cells[9, 3].Text = txtTel.Text;
            ssDiabetes3_Sheet1.Cells[9, 13].Text = txtHPhone.Text;

            ssDiabetes3_Sheet1.Cells[10, 13].Text = txtJuso.Text;

            ssDiabetes3_Sheet1.Cells[11, 1].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2));

            ssDiabetes3_Sheet1.Cells[55, 0].Text = Convert.ToDateTime(strDate).ToString("yyyy 년 MM 월 dd 일");
            
            if (cboDr.Text.Replace(".", "") != "")
            {
                strSabun = VB.Left(cboDr.Text, 4);
                ssDiabetes3_Sheet1.Cells[57, 6].Text = VB.Split(cboDr.Text, ".")[1].ToString();
                ssDiabetes3_Sheet1.Cells[57, 10].Text = clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0]);
            }
            else
            {
                ssDiabetes3_Sheet1.Cells[57, 6].Text = "";
                ssDiabetes3_Sheet1.Cells[57, 10].Text = "";
            }

            //의사 사인 이미지 로드 ------------------------------------------------------------------------------------
            if (strSabun != "")
            {
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                ssDiabetes3_Sheet1.Cells[57, 13].CellType = imgCellType;
                ssDiabetes3_Sheet1.Cells[57, 13].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssDiabetes3_Sheet1.Cells[57, 13].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(strSabun, 5, "0") + ".jpg";

                Image image = SIGNATUREFILE_DBToFile(strSabun);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    ssDiabetes3_Sheet1.Cells[57, 13].Value = image;
                }
            }
            else
            {
                FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                textCellType.WordWrap = false;
                textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                ssDiabetes3_Sheet1.Cells[57, 13].CellType = textCellType;
                ssDiabetes3_Sheet1.Cells[57, 13].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssDiabetes3_Sheet1.Cells[57, 13].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssDiabetes3_Sheet1.Cells[57, 13].Text = "(서명 또는 인)";
            }

            ssDiabetes3_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssDiabetes3_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssDiabetes3_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes3_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes3_Sheet1.PrintInfo.Margin.Top = 60;
            ssDiabetes3_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssDiabetes3_Sheet1.PrintInfo.ShowColor = false;
            ssDiabetes3_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssDiabetes3_Sheet1.PrintInfo.ShowBorder = false;
            ssDiabetes3_Sheet1.PrintInfo.ShowGrid = true;
            ssDiabetes3_Sheet1.PrintInfo.ShowShadows = false;
            ssDiabetes3_Sheet1.PrintInfo.UseMax = true;
            ssDiabetes3_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDiabetes3_Sheet1.PrintInfo.UseSmartPrint = false;
            ssDiabetes3_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssDiabetes3_Sheet1.PrintInfo.Preview = false;
            ssDiabetes3.PrintSheet(0);
        }

        private void toolDent_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strIndex = VB.Right(((ToolStripMenuItem)sender).Name, 1);

            string strPtNo = "";        //등록번호
            string strDtpSDate = "";    //접수일자(공단신청일
            string strSname = "";       //성명(수진자, 수급권자)
            string strPJumin = "";      //주민등록번호
            string strGKiho = "";       //건강보험증번호
            string strHPhone = "";      //휴대전화
            string strTel = "";         //자택전화
            string strJuso = "";        //주소
            string strIllName = "";     //상병명
            string strIllCode = "";     //상병코드

            string strCHAR1 = "";
            string strCHAR2 = "";
            string strCHAR3 = "";
            string strCHAR4 = "";
            string strCHAR5 = "";

            strPtNo = txtPtNo.Text.Trim();
            strDtpSDate = dtpSDate.Value.ToString("yyyy-MM-dd");
            strSname = txtSName.Text.Trim();
            strPJumin = VB.Mid(txtJumin.Text, 1, 6) + "-" + VB.Mid(txtJumin.Text, 7, 7);
            strGKiho = txtGKiho.Text.Trim();
            strHPhone = txtHPhone.Text.Trim();
            strTel = txtTel.Text.Trim();
            strJuso = txtJuso.Text.Trim();

            switch(strIndex)
            {
                case "1":
                    strIllName = "사고, 발치 또는 국한성 치주병에 의한 치아상실";
                    strIllCode = "K08.1";

                    strCHAR1 = "건강보험";
                    strCHAR2 = "수진자";
                    strCHAR3 = "국민건강보험공단 이사장 귀하";
                    strCHAR4 = "건강보험증번호";
                    strCHAR5 = "②" + ComNum.VBLF + "요양기관" + ComNum.VBLF + "확인란";
                    break;
                case "2":
                    strIllName = txtIllName.Text.Trim();
                    strIllCode = txtILLCode.Text.Trim();

                    strCHAR1 = "의료급여";
                    strCHAR2 = "수급권자";
                    strCHAR3 = "시장·군수·구청장 귀하";
                    strCHAR4 = "종별";
                    strCHAR5 = "②" + ComNum.VBLF + "의료급여기관" + ComNum.VBLF + "확인란";
                    break;
            }

            //필요없음
            //Denture_Clear();

            ssDenture_Sheet1.Cells[5, 0].BackColor = Color.FromArgb(222, 222, 222);
            ssDenture_Sheet1.Cells[4, 6].BackColor = Color.FromArgb(222, 222, 222);
            ssDenture_Sheet1.Cells[5, 6].BackColor = Color.FromArgb(222, 222, 222);
            ssDenture_Sheet1.Cells[4, 25].BackColor = Color.FromArgb(222, 222, 222);

            if (strIndex == "1") { ssDenture_Sheet1.Cells[6, 28].BackColor = Color.FromArgb(222, 222, 222); }

            ssDenture_Sheet1.Cells[1, 0].Text = strCHAR1 + " 완전틀니 대상자 등록 신청서";
            ssDenture_Sheet1.Cells[6, 0].Text = "①" + strCHAR2;

            if (strIndex == "1") { ssDenture_Sheet1.Cells[9, 0].Text = "②" + ComNum.VBLF + "요양기관" + ComNum.VBLF + "확인란"; }
            else if (strIndex == "2") { ssDenture_Sheet1.Cells[9, 0].Text = strCHAR5; }

            ssDenture_Sheet1.Cells[6, 22].Text = strCHAR4;
            ssDenture_Sheet1.Cells[18, 0].Text = "위와 같이 " + strCHAR1 + " 틀니 대상자 등록을 신청합니다.";
            ssDenture_Sheet1.Cells[21, 0].Text = strCHAR2 + "와의 관계";
            ssDenture_Sheet1.Cells[23, 0].Text = strCHAR3;
            ssDenture_Sheet1.Cells[36, 0].Text = strCHAR3;
            
            ssDenture_Sheet1.Cells[25, 1].Text = "본인은 " + strCHAR1 + " 틀니 대상자로 신청하거나 대상자로 선정·등록된 자로, 개인정보보호법 제15조1항제1호";
            ssDenture_Sheet1.Cells[26, 1].Text = "규정에 의거하여 본인의 개인정보1)를 처리할 것을 동의합니다.";
            ssDenture_Sheet1.Cells[27, 1].Text = "본인은 " + strCHAR1 + " 틀니 대상자로 신청하거나 대상자로 선정·등록된 자로, 개인정보보호법 제23조제1호";
            ssDenture_Sheet1.Cells[28, 1].Text = "규정에 의거하여 본인의 민감정보2)를 처리할 것을 동의합니다.";
            ssDenture_Sheet1.Cells[29, 1].Text = "본인은 " + strCHAR1 + " 틀니 대상자로 신청하거나 대상자로 선정·등록된 자로, 개인정보보호법 제24조제1항제1호";
            ssDenture_Sheet1.Cells[30, 1].Text = "규정에 의거하여 본인의 고유식별정보3)를 처리할 것을 동의합니다.";

            switch(strIndex)
            {
                case "1":
                    ssDenture_Sheet1.Cells[31, 0].Text = "";
                    ssDenture_Sheet1.Cells[31, 1].Text = "";
                    ssDenture_Sheet1.Cells[32, 1].Text = "";
                    ssDenture_Sheet1.Cells[32, 25].Text = "";
                    ssDenture_Sheet1.Cells[32, 29].Text = "";
                    break;
                case "2":
                    ssDenture_Sheet1.Cells[31, 0].Text = "○";
                    ssDenture_Sheet1.Cells[31, 1].Text = "본인은 " + strCHAR1 + " 틀니 대상자로 신청하거나 대상자로 선정·등록된 자로, 개인정보보호법 제17조제1항제2호";
                    ssDenture_Sheet1.Cells[32, 1].Text = "규정에 의거하여 본인의 개인정보1)를 제3자에게 제공할 것을 동의합니다.";
                    ssDenture_Sheet1.Cells[32, 25].Text = "□ 동의함";
                    ssDenture_Sheet1.Cells[32, 29].Text = "□ 동의하지 않음";
                    break;
            }

            ssDenture_Sheet1.Cells[35, 0].Text = "④ " + strCHAR2 + " 본인";

            if (strIndex == "1") { ssDenture_Sheet1.Cells[5, 0].Text = strPtNo; }   //등록번호

            ssDenture_Sheet1.Cells[6, 6].Text = strSname;       //성명
            ssDenture_Sheet1.Cells[6, 16].Text = strPJumin;       //주민등록번호

            if (strIndex == "2")
            {
                if (GstrBi == "21")
                {
                    ssDenture_Sheet1.Cells[6, 28].Text = "의료급여1종";
                }
                else if (GstrBi == "22")
                {
                    ssDenture_Sheet1.Cells[6, 28].Text = "의료급여2종";
                }
            }

            ssDenture_Sheet1.Cells[7, 6].Text = strJuso;        //주소

            ssDenture_Sheet1.Cells[8, 8].Text = strHPhone;      //휴대전화
            ssDenture_Sheet1.Cells[8, 18].Text = strTel;        //자택번호

            ssDenture_Sheet1.Cells[9, 8].Text = strIllName;     //상병명
            ssDenture_Sheet1.Cells[9, 27].Text = strIllCode;    //상병코드

            switch(strIndex)
            {
                case "1":
                    ssDenture_Sheet1.Cells[16, 4].Text = "요양기관명(기호)";
                    break;
                case "2":
                    ssDenture_Sheet1.Cells[16, 4].Text = "의료급여기관명(기호)";
                    break;
            }

            ssDenture_Sheet1.Cells[16, 16].Text = "포항성모병원";
            ssDenture_Sheet1.Cells[16, 24].Text = "37100068";

            ssDenture_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssDenture_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssDenture_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDenture_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDenture_Sheet1.PrintInfo.Margin.Top = 60;
            ssDenture_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssDenture_Sheet1.PrintInfo.ShowColor = true;
            ssDenture_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssDenture_Sheet1.PrintInfo.ShowBorder = false;
            ssDenture_Sheet1.PrintInfo.ShowGrid = false;
            ssDenture_Sheet1.PrintInfo.ShowShadows = false;
            ssDenture_Sheet1.PrintInfo.UseMax = false;
            ssDenture_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDenture_Sheet1.PrintInfo.UseSmartPrint = false;
            ssDenture_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssDenture_Sheet1.PrintInfo.Preview = false;
            ssDenture.PrintSheet(0);
        }

        private void toolDent3_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strIndex = VB.Right(((ToolStripMenuItem)sender).Name, 1);

            FarPoint.Win.Spread.FpSpread ssSpread = null;

            string strPtNo = "";        //등록번호
            string strDtpSDate = "";    //접수일자(공단신청일
            string strSname = "";       //성명(수진자, 수급권자)
            string strPJumin = "";      //주민등록번호
            string strGKiho = "";       //건강보험증번호
            string strHPhone = "";      //휴대전화
            string strTel = "";         //자택전화
            string strJuso = "";        //주소

            strPtNo = txtPtNo.Text.Trim();
            strDtpSDate = dtpSDate.Value.ToString("yyyy-MM-dd");
            strSname = txtSName.Text.Trim();
            strPJumin = VB.Mid(txtJumin.Text, 1, 6) + "-" + VB.Mid(txtJumin.Text, 7, 7);
            strGKiho = txtGKiho.Text.Trim();
            strHPhone = txtHPhone.Text.Trim();
            strTel = txtTel.Text.Trim();
            strJuso = txtJuso.Text.Trim();

            if (strIndex == "3") { ssSpread = ssImplant1; } else if (strIndex == "4") { ssSpread = ssImplant2; }

            if (ssSpread == ssImplant1)
            {
                ssSpread.ActiveSheet.Cells[4, 4].Text = "";     //등록번호

                ssSpread.ActiveSheet.Cells[5, 6].Text = "";     //성명
                ssSpread.ActiveSheet.Cells[5, 16].Text = "";    //주민등록번호
                ssSpread.ActiveSheet.Cells[5, 28].Text = "";    //건강보험증번호

                ssSpread.ActiveSheet.Cells[6, 6].Text = "";     //주소
                ssSpread.ActiveSheet.Cells[6, 28].Text = "";    //휴대전화

                ssSpread.ActiveSheet.Cells[7, 8].Text = "";     //자택번호

                ssSpread.ActiveSheet.Cells[19, 16].Text = "37100068";
                ssSpread.ActiveSheet.Cells[19, 24].Text = "포항성모병원";
            }
            else if(ssSpread == ssImplant2)
            {
                ssSpread.ActiveSheet.Cells[4, 5].Text = "";     //등록번호

                ssSpread.ActiveSheet.Cells[5, 7].Text = "";     //성명
                ssSpread.ActiveSheet.Cells[5, 17].Text = "";    //주민등록번호
                ssSpread.ActiveSheet.Cells[5, 28].Text = "";

                ssSpread.ActiveSheet.Cells[6, 7].Text = "";     //주소
                ssSpread.ActiveSheet.Cells[6, 29].Text = "";     //자택번호

                ssSpread.ActiveSheet.Cells[18, 17].Text = "37100068";
                ssSpread.ActiveSheet.Cells[18, 25].Text = "포항성모병원";
            }

            ssSpread.ActiveSheet.Rows[4].BackColor = Color.FromArgb(222, 222, 222);

            if (ssSpread == ssImplant1)
            {
                ssSpread.ActiveSheet.Cells[4, 4].Text = strPtNo;        //등록번호

                ssSpread.ActiveSheet.Cells[5, 6].Text = strSname;       //성명
                ssSpread.ActiveSheet.Cells[5, 16].Text = strPJumin;     //주민등록번호
                ssSpread.ActiveSheet.Cells[5, 28].Text = strGKiho;      //건강보험증번호

                ssSpread.ActiveSheet.Cells[6, 6].Text = strJuso;        //주소
                ssSpread.ActiveSheet.Cells[6, 28].Text = strHPhone;     //휴대전화

                ssSpread.ActiveSheet.Cells[7, 8].Text = strTel;         //자택번호

                ssSpread.ActiveSheet.Cells[19, 16].Text = "37100068";
                ssSpread.ActiveSheet.Cells[19, 24].Text = "포항성모병원";
            }
            else if(ssSpread == ssImplant2)
            {
                ssSpread.ActiveSheet.Cells[4, 5].Text = strPtNo;        //등록번호

                ssSpread.ActiveSheet.Cells[5, 7].Text = strSname;       //성명
                ssSpread.ActiveSheet.Cells[5, 17].Text = strPJumin;     //주민등록번호

                if (GstrBi == "21") { ssSpread.ActiveSheet.Cells[5, 28].Text = "의료급여1종"; } else if (GstrBi == "22") { ssSpread.ActiveSheet.Cells[5, 28].Text = "의료급여2종"; }

                ssSpread.ActiveSheet.Cells[6, 7].Text = strJuso;        //주소
                ssSpread.ActiveSheet.Cells[6, 29].Text = strTel;         //자택번호

                ssSpread.ActiveSheet.Cells[18, 17].Text = "37100068";
                ssSpread.ActiveSheet.Cells[18, 25].Text = "포항성모병원";
            }

            ssSpread.ActiveSheet.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssSpread.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssSpread.ActiveSheet.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssSpread.ActiveSheet.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssSpread.ActiveSheet.PrintInfo.Margin.Top = 60;
            ssSpread.ActiveSheet.PrintInfo.Margin.Bottom = 20;
            ssSpread.ActiveSheet.PrintInfo.ShowColor = true;
            ssSpread.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssSpread.ActiveSheet.PrintInfo.ShowBorder = false;
            ssSpread.ActiveSheet.PrintInfo.ShowGrid = false;
            ssSpread.ActiveSheet.PrintInfo.ShowShadows = false;
            ssSpread.ActiveSheet.PrintInfo.UseMax = false;
            ssSpread.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssSpread.ActiveSheet.PrintInfo.UseSmartPrint = false;
            ssSpread.ActiveSheet.PrintInfo.ShowPrintDialog = false;
            ssSpread.ActiveSheet.PrintInfo.Preview = false;
            ssSpread.PrintSheet(0);
        }

        private void toolBreath_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("건강보험 인공호흡기 급여대상자 등록 신청서를 인쇄 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string strFile = "";
            string strSabun = "";
            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            ssRespirator_Sheet1.Cells[5, 4].Text = txtSName.Text;       //성명

            ssRespirator_Sheet1.Cells[6, 8].Text = txtJumin.Text;       //주민번호
            ssRespirator_Sheet1.Cells[6, 15].Text = txtGKiho.Text;      //건강보험증번호

            ssRespirator_Sheet1.Cells[7, 7].Text = txtTel.Text;         //전화번호

            ssRespirator_Sheet1.Cells[8, 7].Text = txtHPhone.Text;      //휴대폰번호

            ssRespirator_Sheet1.Cells[10, 5].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(cboDept.Text, 2));  //진료과목

            ssRespirator_Sheet1.Cells[35, 3].Text = Convert.ToDateTime(strDate).ToString("yyyy   년        MM   월        dd   일");
            ssRespirator_Sheet1.Cells[42, 1].Text = Convert.ToDateTime(strDate).ToString("yyyy   년        MM   월        dd   일");

            if (cboDr.Text.Replace(".", "") != "")
            {
                strSabun = VB.Left(cboDr.Text, 4);
                ssRespirator_Sheet1.Cells[37, 10].Text = "  " + VB.Split(cboDr.Text, ".")[1];
                ssRespirator_Sheet1.Cells[37, 10].Text = ssRespirator_Sheet1.Cells[37, 10].Text + "    (" + clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDr.Text, ".")[0]) + ")";
            }
            else
            {
                ssRespirator_Sheet1.Cells[37, 10].Text = "";
            }

            //의사 사인 이미지 로드 ------------------------------------------------------------------------------------
            if (strSabun != "")
            {
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                ssRespirator_Sheet1.Cells[37, 17].CellType = imgCellType;
                ssRespirator_Sheet1.Cells[37, 17].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssRespirator_Sheet1.Cells[37, 17].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(strSabun, 5, "0") + ".jpg";

                Image image = SIGNATUREFILE_DBToFile(strSabun);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    ssRespirator_Sheet1.Cells[37, 17].Value = image;
                }
            }
            else
            {
                FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                textCellType.WordWrap = false;
                textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                ssRespirator_Sheet1.Cells[37, 17].CellType = textCellType;
                ssRespirator_Sheet1.Cells[37, 17].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssRespirator_Sheet1.Cells[37, 17].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssRespirator_Sheet1.Cells[37, 17].Text = "(서명 또는 인)";
            }

            ssRespirator_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssRespirator_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssRespirator_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssRespirator_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssRespirator_Sheet1.PrintInfo.Margin.Top = 60;
            ssRespirator_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssRespirator_Sheet1.PrintInfo.ShowColor = false;
            ssRespirator_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssRespirator_Sheet1.PrintInfo.ShowBorder = false;
            ssRespirator_Sheet1.PrintInfo.ShowGrid = true;
            ssRespirator_Sheet1.PrintInfo.ShowShadows = false;
            ssRespirator_Sheet1.PrintInfo.UseMax = true;
            ssRespirator_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssRespirator_Sheet1.PrintInfo.UseSmartPrint = false;
            ssRespirator_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssRespirator_Sheet1.PrintInfo.Preview = false;
            ssRespirator.PrintSheet(0);
        }

        private void toolBst_Click(object sender, EventArgs e)
        {

        }

        private void 치과임플란트ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
