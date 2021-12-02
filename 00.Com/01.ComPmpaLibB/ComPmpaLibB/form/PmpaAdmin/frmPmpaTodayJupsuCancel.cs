
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpaTodayJupsuCancel : Form
    {
        ComFunc CF = null;
        Card CC = null;
        clsSpread CS = null;
        clsPmpaFunc CPF = null;
        clsPmpaQuery CPQ = null;
        clsOumsad CPO = null;
        clsOrdFunction OF = null;

        DataTable Dt = new DataTable();
        DataTable DtSub = new DataTable();
        string rtnVal = string.Empty;
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;

        // clsPmpaType.AcctReqData RSD = new clsPmpaType.AcctReqData();
        // clsPmpaType.AcctResData RD = new clsPmpaType.AcctResData();

        string FstrFlagOM = string.Empty; //접수Master  Read Flag (OPD_MASTER)
        string FstrStart = string.Empty;
        int nIndex = 0;
        long[] FnOldAmts = new long[8];
        string FstrChkBun = string.Empty; //당일 접수비 수납후 미진료 환자

        string FstrPano = "";
        string FstrDept = "";

        public frmPmpaTodayJupsuCancel()
        {
            InitializeComponent();
            setParam();
        }

        public frmPmpaTodayJupsuCancel(string strPano, string strDept)
        {
            InitializeComponent();
            setParam();
            FstrPano = strPano;
            FstrDept = strDept;
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);

            //KeyPress 이벤트
            this.dtpBdate.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtPtno.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboDept.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);

            //LostFocus 이벤트
            this.dtpBdate.LostFocus += new EventHandler(eCtl_LostFocus);

            //SelectedIndexChanged
            this.cboDept.SelectedIndexChanged += new EventHandler(eCtl_SelectedChanged);

            this.chkCard.Click += new EventHandler(eCtl_Click);//조회버튼
            this.chkCash.Click += new EventHandler(eCtl_Click);//조회버튼

            //Click 이벤트
            this.btnSearch.Click += new EventHandler(eCtl_Click);//조회버튼
            this.btnSave.Click += new EventHandler(eCtl_Click);//저장버튼
            this.btnCancel.Click += new EventHandler(eCtl_Click);//취소버튼
            this.btnExit.Click += new EventHandler(eCtl_Click);//닫기버튼
        }



        private void eCtl_SelectedChanged(object sender, EventArgs e)
        {
            if (sender == cboDept)
            {
                if (cboDept.Text != "")
                    txtDeptName.Text = clsPmpaPb.GstrSetDepts[cboDept.SelectedIndex];
            }
        }


        private void eCtl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.dtpBdate)
                clsPublic.GstrBdate = dtpBdate.Text;
        }

        private void DISPLAY_SCREEN(PsmhDb pDbCon)
        {
            string strFlagBP = string.Empty;

            int j = Convert.ToInt32(clsPmpaType.TOM.Chojae);

            //이중감액 : 적용시 주석해제
            //int l = Convert.ToInt32(clsPmpaType.TOM.GbGameKC);

            txtOpdNo.Text = clsPmpaType.TOM.OpdNo.ToString();
            txtSname.Text = clsPmpaType.TOM.sName;
            txtDrCode.Text = clsPmpaType.TOM.DrCode;
            txtChojae.Text = clsPmpaType.TOM.Chojae;
            txtGamek.Text = clsPmpaType.TOM.GbGameK;

            //이중감액 : 주석해제
            //txtGamekC.Text = clsPmpaType.TOM.GbGameKC;
            txtSunap.Text = clsPmpaType.TOM.Jin;
            txtRes.Text = clsPmpaType.TOM.Reserved;

            strFlagBP = CPO.READ_BAS_PATIENT(pDbCon, txtPtno.Text);
            clsPmpaPb.GstrGamMsg = "";

            if (strFlagBP.Equals("OK"))
            {
                CPQ.READ_BAS_GAMF(pDbCon, clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2, clsPmpaType.TOM.BDate); //감액대상자 확인
                if (clsPmpaPb.GstrFlagGam.Equals("OK"))
                {
                    if (clsPmpaPb.GstrGamEnd != "")
                    {
                        if (string.Compare(clsPublic.GstrSysDate, clsPmpaPb.GstrGamEnd) > 0)
                            clsPmpaPb.GstrGamMsg = "감액적용해제";
                    }
                }
                else
                {
                    clsPmpaPb.GstrGamMsg = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_감액코드명", clsPmpaType.TOM.GbGameK);
                }
            }

            txtDrName.Text = CPF.READ_DOCTOR_NAME(pDbCon, clsPmpaType.TOM.DrCode);
            txtChojaeName.Text = clsPmpaPb.GstrSetChoJaes[j];
            txtGamekName.Text = clsPmpaPb.GstrGamMsg;
            txtSunapName.Text = CPF.READ_JIN(pDbCon, clsPmpaType.TOM.Jin) + " ( " + clsPmpaType.TOM.Amt7 + " ) ";

            if (clsPmpaType.TOM.Reserved == "1")
                txtResName.Text = "예약접수자";
            else
                txtResName.Text = "당일접수자";

            if (clsPmpaType.TOM.Jin == "E")
                txtResName.Text = "기록실 전화접수자";
        }

        private void eCtl_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (sender == this.dtpBdate && e.KeyChar == (Char)13)
            {
                txtPtno.Focus();
                sender = null;
            }
            else if (sender == this.txtPtno && e.KeyChar == (Char)13)
            {
                if (txtPtno.Text == "") { return; }

                //2018.05.31 박병규 : 변수초기화
                CC.CardVariable_Clear(ref clsPmpaType.RSD, ref clsPmpaType.RD);

                string Ret_str = string.Empty;

                lblMsg.Text = "";

                if (CF.READ_BARCODE(txtPtno.Text.Trim()) == true)
                {
                    txtPtno.Text = clsPublic.GstrBarPano;
                    cboDept.Text = clsPublic.GstrBarDept;
                }

                txtPtno.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno.Text));

                string rBP = CPO.READ_BAS_PATIENT(clsDB.DbCon, txtPtno.Text);

                Ret_str = CPF.Pano_Check_Digit(clsDB.DbCon, txtPtno.Text);

                if (Ret_str.Equals("NO"))
                {
                    lblMsg.Text = "등록번호 ReEnter";
                    txtPtno.Focus();
                    return;
                }

                cboDept.Focus();
                sender = null;
            }

            if (sender == this.cboDept && e.KeyChar == (Char)13)
            {
                string strGoodFlag = string.Empty;
                CC.CardVariable_Clear(ref clsPmpaType.RSD, ref clsPmpaType.RD);
                txtPart.Text = "";
                lblMsg.Text = "";
                if (clsPmpaPb.GstrLostFocus.Equals("**")) { return; }
                if (txtPtno.Text.Trim() == "") { return; }
                if (cboDept.Text.Trim() == "") { return; }

                //응급실에서 접수,취소가 가능함 진료과 확인
                if (clsPmpaPb.GstrErJobFlag.Equals("OK"))
                {
                    switch (cboDept.Text.ToUpper().Trim())
                    {
                        case "ER":
                        case "HD":
                        case "R6":
                        case "PD":
                            break;

                        default:
                            ComFunc.MsgBox("응급실에서는 ER, HD, PD, R6 진료과만 취소가능합니다.", "확인");
                            return;
                    }
                }

                strGoodFlag = "NO";
                cboDept.Text = cboDept.Text.ToUpper();
                clsPmpaPb.GstrGwa = cboDept.Text;

                for (int i = 0; i < cboDept.Items.Count; i++)
                {
                    if (cboDept.Text == cboDept.Items[i].ToString())
                    {
                        nIndex = i;
                        txtDeptName.Text = clsPmpaPb.GstrSetDepts[i];
                        strGoodFlag = "OK";
                        break;
                    }
                }

                if (strGoodFlag.Equals("NO"))
                {
                    lblMsg.Text = "진료과목 코드오류";
                    cboDept.Focus();
                    return;
                }

                if (clsPmpaPb.GstrErJobFlag.Equals("OK"))
                {
                    if (cboDept.Text.Trim() == "PD")
                        FstrFlagOM = CPO.READ_OPD_MASTER(clsDB.DbCon, txtPtno.Text, cboDept.Text, clsPublic.GstrActDate, dtpBdate.Text);
                    else
                        FstrFlagOM = CPO.READ_OPD_MASTER(clsDB.DbCon, txtPtno.Text, cboDept.Text, dtpBdate.Text, dtpBdate.Text);
                }
                else
                {
                    FstrFlagOM = CPO.READ_OPD_MASTER(clsDB.DbCon, txtPtno.Text, cboDept.Text, clsPublic.GstrActDate, dtpBdate.Text);
                }

                if (FstrFlagOM == "NO")
                {
                    lblMsg.Text = "해당환자는 접수가 안되어 있습니다.";
                    txtPtno.Focus();
                }
                else if (clsPmpaType.TOM.Part != clsType.User.IdNumber && clsPmpaType.TOM.Jin != "5" && clsPmpaType.TOM.Jin != "E")
                {
                    txtPart.Text = "접수 조 : " + clsPmpaType.TOM.Part;

                    if (clsPmpaType.TOM.Reserved != "1")
                    {
                        //Alert 주석요청
                        //ComFunc.MsgBox("본인이 접수한 건이 아닙니다. " + clsPmpaType.TOM.Part + " 접수조", "확인");

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT NVL(SUM(AMT1), 0) nAmt ";
                        SQL += ComNum.VBLF + "   from " + ComNum.DB_PMPA + "opd_slip ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno.Text + "'";
                        SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + cboDept.Text.Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND BUN       > '84' ";
                        SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (Dt.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(Dt.Rows[0]["nAmt"].ToString().Trim()) > 0)
                            {
                                Dt.Dispose();
                                Dt = null;

                                clsPublic.GstrMsgTitle = "경고";
                                clsPublic.GstrMsgList = "이미 수납을 하였습니다." + '\r';
                                clsPublic.GstrMsgList += "접수취소를 할 수 없습니다." + '\r';
                                ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);

                                btnSave.Enabled = false;
                                txtPtno.Focus();
                                return;
                            }
                            else if (Convert.ToInt32(Dt.Rows[0]["nAmt"].ToString().Trim()) == 0)
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " SELECT NVL(SUM(NAL), 0) NAL ";
                                SQL += ComNum.VBLF + "   from " + ComNum.DB_MED + "OCS_OUTDRUG ";
                                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                                SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno.Text + "'";
                                SQL += ComNum.VBLF + "    AND SLIPDATE  = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
                                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + cboDept.Text.Trim() + "' ";
                                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                if (DtSub.Rows.Count > 0)
                                {
                                    if (Convert.ToInt32(DtSub.Rows[0]["NAL"].ToString().Trim()) != 0)
                                    {
                                        DtSub.Dispose();
                                        DtSub = null;

                                        clsPublic.GstrMsgTitle = "경고";
                                        clsPublic.GstrMsgList = "이미 수납을 하였습니다.(원외처방전)" + '\r';
                                        clsPublic.GstrMsgList += "접수취소를 할 수 없습니다." + '\r';
                                        ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);

                                        btnSave.Enabled = false;
                                        txtPtno.Focus();
                                        return;
                                    }
                                }

                                DtSub.Dispose();
                                DtSub = null;
                            }
                        }

                        Dt.Dispose();
                        Dt = null;

                        clsPmpaPb.GstrLostFocus = "**";
                        FnOldAmts[1] = clsPmpaType.TOM.Amt1;
                        clsPmpaPb.gnJinAMT1 = clsPmpaType.TOM.Amt1;
                        FnOldAmts[2] = clsPmpaType.TOM.Amt2;
                        clsPmpaPb.gnJinAMT2 = clsPmpaType.TOM.Amt2;
                        FnOldAmts[3] = clsPmpaType.TOM.Amt3;
                        clsPmpaPb.gnJinAMT3 = clsPmpaType.TOM.Amt3;
                        FnOldAmts[4] = clsPmpaType.TOM.Amt4;
                        clsPmpaPb.gnJinAMT4 = clsPmpaType.TOM.Amt4;
                        FnOldAmts[5] = clsPmpaType.TOM.Amt5;
                        clsPmpaPb.gnJinAMT5 = clsPmpaType.TOM.Amt5;
                        FnOldAmts[6] = clsPmpaType.TOM.Amt6;
                        clsPmpaPb.gnJinAMT6 = clsPmpaType.TOM.Amt6;
                        FnOldAmts[7] = clsPmpaType.TOM.Amt7;
                        clsPmpaPb.gnJinAMT7 = clsPmpaType.TOM.Amt7;

                        pnlLeft.Enabled = false;
                        btnSave.Enabled = true;
                        dtpBdate.Enabled = false;
                        cboDept.Enabled = false;

                        DISPLAY_SCREEN(clsDB.DbCon);
                        btnSave.Focus();
                    }
                    else if (clsPmpaType.TOM.Amt7 == 0 && clsPmpaType.TOM.Reserved != "1")
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT NVL(SUM(AMT1), 0) nAmt ";
                        SQL += ComNum.VBLF + "   from " + ComNum.DB_PMPA + "opd_slip ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno.Text + "'";
                        SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + cboDept.Text.Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND BUN       > '84' ";
                        SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (Dt.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(Dt.Rows[0]["nAmt"].ToString().Trim()) > 0)
                            {
                                Dt.Dispose();
                                Dt = null;

                                clsPublic.GstrMsgTitle = "경고";
                                clsPublic.GstrMsgList = "이미 수납을 하였습니다." + '\r';
                                clsPublic.GstrMsgList += "접수취소를 할 수 없습니다." + '\r';
                                ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);

                                btnSave.Enabled = false;
                                txtPtno.Focus();
                                return;
                            }
                            else if (Convert.ToInt32(Dt.Rows[0]["nAmt"].ToString().Trim()) == 0)
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " SELECT NVL(SUM(NAL), 0) NAL ";
                                SQL += ComNum.VBLF + "   from " + ComNum.DB_MED + "OCS_OUTDRUG ";
                                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                                SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno.Text + "'";
                                SQL += ComNum.VBLF + "    AND SLIPDATE  = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
                                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + cboDept.Text.Trim() + "' ";
                                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                if (DtSub.Rows.Count > 0)
                                {
                                    if (Convert.ToInt32(DtSub.Rows[0]["NAL"].ToString().Trim()) != 0)
                                    {
                                        DtSub.Dispose();
                                        DtSub = null;

                                        clsPublic.GstrMsgTitle = "경고";
                                        clsPublic.GstrMsgList = "이미 수납을 하였습니다.(원외처방전)" + '\r';
                                        clsPublic.GstrMsgList += "접수취소를 할 수 없습니다." + '\r';
                                        ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);

                                        btnSave.Enabled = false;
                                        txtPtno.Focus();
                                        return;
                                    }
                                }

                                DtSub.Dispose();
                                DtSub = null;
                            }
                        }

                        Dt.Dispose();
                        Dt = null;

                        clsPmpaPb.GstrLostFocus = "**";
                        FnOldAmts[1] = clsPmpaType.TOM.Amt1;
                        clsPmpaPb.gnJinAMT1 = clsPmpaType.TOM.Amt1;
                        FnOldAmts[2] = clsPmpaType.TOM.Amt2;
                        clsPmpaPb.gnJinAMT2 = clsPmpaType.TOM.Amt2;
                        FnOldAmts[3] = clsPmpaType.TOM.Amt3;
                        clsPmpaPb.gnJinAMT3 = clsPmpaType.TOM.Amt3;
                        FnOldAmts[4] = clsPmpaType.TOM.Amt4;
                        clsPmpaPb.gnJinAMT4 = clsPmpaType.TOM.Amt4;
                        FnOldAmts[5] = clsPmpaType.TOM.Amt5;
                        clsPmpaPb.gnJinAMT5 = clsPmpaType.TOM.Amt5;
                        FnOldAmts[6] = clsPmpaType.TOM.Amt6;
                        clsPmpaPb.gnJinAMT6 = clsPmpaType.TOM.Amt6;
                        FnOldAmts[7] = clsPmpaType.TOM.Amt7;
                        clsPmpaPb.gnJinAMT7 = clsPmpaType.TOM.Amt7;

                        pnlLeft.Enabled = false;
                        btnSave.Enabled = true;
                        dtpBdate.Enabled = false;
                        cboDept.Enabled = false;

                        DISPLAY_SCREEN(clsDB.DbCon);
                        btnSave.Focus();
                    }
                    else if (clsPmpaType.TOM.Reserved == "1")
                    {
                        lblMsg.Text = "예약은 예약 접수이전에서 삭제하세요 !";
                        txtPtno.Focus();
                    }
                    else
                    {
                        lblMsg.Text = clsPmpaType.TOM.Part + "조에서 취소 가능함 !";
                        txtPtno.Focus();
                    }

                    btnSave.Select();
                }
                else if (clsPmpaType.TOM.Reserved == "1")
                {
                    lblMsg.Text = "예약은 예약 접수이전에서 삭제하시기 바랍니다.";
                    txtPtno.Focus();
                }
                else
                {
                    txtPart.Text = "접수조 : " + clsPmpaType.TOM.Part;

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT NVL(SUM(AMT1), 0) nAmt ";
                    SQL += ComNum.VBLF + "   from " + ComNum.DB_PMPA + "opd_slip ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno.Text + "'";
                    SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + cboDept.Text.Trim() + "' ";
                    SQL += ComNum.VBLF + "    AND BUN       > '84' ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (Dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(Dt.Rows[0]["nAmt"].ToString().Trim()) > 0)
                        {
                            Dt.Dispose();
                            Dt = null;

                            clsPublic.GstrMsgTitle = "경고";
                            clsPublic.GstrMsgList = "이미 수납을 하였습니다." + '\r';
                            clsPublic.GstrMsgList += "접수취소를 할 수 없습니다." + '\r';
                            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);

                            btnSave.Enabled = false;
                            txtPtno.Focus();
                            return;
                        }
                        else if (Convert.ToInt32(Dt.Rows[0]["nAmt"].ToString().Trim()) == 0)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT NVL(SUM(NAL), 0) NAL ";
                            SQL += ComNum.VBLF + "   from " + ComNum.DB_MED + "OCS_OUTDRUG ";
                            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                            SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno.Text + "'";
                            SQL += ComNum.VBLF + "    AND SLIPDATE  = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + cboDept.Text.Trim() + "' ";
                            SqlErr = clsDB.GetDataTable(ref DtSub, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            if (DtSub.Rows.Count > 0)
                            {
                                if (Convert.ToInt32(DtSub.Rows[0]["NAL"].ToString().Trim()) != 0)
                                {
                                    DtSub.Dispose();
                                    DtSub = null;

                                    clsPublic.GstrMsgTitle = "경고";
                                    clsPublic.GstrMsgList = "이미 수납을 하였습니다.(원외처방전)" + '\r';
                                    clsPublic.GstrMsgList += "접수취소를 할 수 없습니다." + '\r';
                                    ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);

                                    btnSave.Enabled = false;
                                    txtPtno.Focus();
                                    return;
                                }
                            }

                            DtSub.Dispose();
                            DtSub = null;
                        }
                    }

                    Dt.Dispose();
                    Dt = null;

                    clsPmpaPb.GstrLostFocus = "**";
                    FnOldAmts[1] = clsPmpaType.TOM.Amt1;
                    clsPmpaPb.gnJinAMT1 = clsPmpaType.TOM.Amt1;
                    FnOldAmts[2] = clsPmpaType.TOM.Amt2;
                    clsPmpaPb.gnJinAMT2 = clsPmpaType.TOM.Amt2;
                    FnOldAmts[3] = clsPmpaType.TOM.Amt3;
                    clsPmpaPb.gnJinAMT3 = clsPmpaType.TOM.Amt3;
                    FnOldAmts[4] = clsPmpaType.TOM.Amt4;
                    clsPmpaPb.gnJinAMT4 = clsPmpaType.TOM.Amt4;
                    FnOldAmts[5] = clsPmpaType.TOM.Amt5;
                    clsPmpaPb.gnJinAMT5 = clsPmpaType.TOM.Amt5;
                    FnOldAmts[6] = clsPmpaType.TOM.Amt6;
                    clsPmpaPb.gnJinAMT6 = clsPmpaType.TOM.Amt6;
                    FnOldAmts[7] = clsPmpaType.TOM.Amt7;
                    clsPmpaPb.gnJinAMT7 = clsPmpaType.TOM.Amt7;

                    pnlLeft.Enabled = false;
                    btnSave.Enabled = true;
                    dtpBdate.Enabled = false;
                    cboDept.Enabled = false;

                    DISPLAY_SCREEN(clsDB.DbCon);
                    btnSave.Focus();
                }
            }
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.chkCard)
            {
                CC.CardVariable_Clear(ref clsPmpaType.RSD, ref clsPmpaType.RD);
                if (chkCard.Checked == false) { return; }

                CC.gstrCdPtno = txtPtno.Text.Trim();
                CC.gstrCdSName = txtSname.Text.Trim();
                CC.gstrCdDeptCode = cboDept.Text.Trim();
                CC.gstrCdPart = clsType.User.IdNumber;
                CC.gstrCdGbIo = "O.외래";
                CC.gstrCdPCode = "JUP-";
                CC.glngCdAmt = 0;

                frmPmpaEntryCardDaou frm = new frmPmpaEntryCardDaou(CC.gstrCdPtno, CC.gstrCdSName, CC.gstrCdDeptCode, CC.gstrCdGbIo, CC.glngCdAmt, "CARD", clsPmpaType.TOM.BDate);
                frm.ShowDialog();
                OF.fn_ClearMemory(frm);

                CC.GstrCardJob = "";
                btnSave.Enabled = true;
                if (chkCard.Checked == true) { chkCard.Checked = false; }
            }

            if (sender == this.chkCash)
            {
                CC.CardVariable_Clear(ref clsPmpaType.RSD, ref clsPmpaType.RD);

                CC.gstrCdPtno = txtPtno.Text.Trim();
                CC.gstrCdSName = txtSname.Text.Trim();
                CC.gstrCdDeptCode = cboDept.Text.Trim();
                CC.gstrCdPart = clsType.User.IdNumber;
                CC.gstrCdGbIo = "O.외래";
                CC.gstrCdPCode = "JUP+";
                CC.glngCdAmt = Convert.ToInt64(clsPmpaPb.gnJinAMT7);
                CC.GstrCardJob = "Menual";
                this.Tag = "TRUE";

                frmPmpaEntryCardDaou frm = new frmPmpaEntryCardDaou(CC.gstrCdPtno, CC.gstrCdSName, CC.gstrCdDeptCode, CC.gstrCdGbIo, CC.glngCdAmt, "CASH", clsPmpaType.TOM.BDate);
                frm.ShowDialog();
                OF.fn_ClearMemory(frm);

                chkCash.Checked = false;
            }


            if (sender == btnHSearch) { READ_H_PROESS(clsDB.DbCon); }
            if (sender == btnSearch) { READ_PROESS(clsDB.DbCon); }
            if (sender == btnSave) { SAVE_PROESS(clsDB.DbCon); }
            if (sender == btnCancel) { eFrm_Clear(); }
            if (sender == btnExit)
            {
                clsPublic.GstrHelpCode = "";
                this.Close();
            }
        }

        private void READ_H_PROESS(PsmhDb pDbCon)
        {
            btnHSearch.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;
            //CS.Spread_All_Clear(ssList);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT OPDNO, Sname, DeptCode, Pano, Sex, Age";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND PART      = 'Z' ";
            SQL += ComNum.VBLF + "    AND JIN       <> '*' ";
            SQL += ComNum.VBLF + "    AND REP       = ' ' ";
            SQL += ComNum.VBLF + "  ORDER BY Sname ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["OPDNO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["Sname"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["Sex"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["Age"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;
            Cursor.Current = Cursors.Default;
        }

        private bool OPD_WORK_DEL_INSERT(PsmhDb pDbCon)
        {
            int nSeq = 0;
            bool rtnVal;

            rtnVal = true;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SEQ_OPDWORK.NEXTVAL NEXTVAL ";
            SQL += ComNum.VBLF + "   FROM DUAL ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                rtnVal = false;
                return rtnVal;
            }

            if (Dt.Rows.Count == 0)
            {
                Dt.Dispose();
                Dt = null;
                rtnVal = false;
                return rtnVal;
            }

            nSeq = Convert.ToInt32(Dt.Rows[0]["NEXTVAL"].ToString().Trim());

            Dt.Dispose();
            Dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_WORK ";
                SQL += ComNum.VBLF + "        (Bdate, SeqNo, Pano, ";
                SQL += ComNum.VBLF + "         DeptCode, DrCode, Sname, ";
                SQL += ComNum.VBLF + "         Bi, Chojae, Singu, ";
                SQL += ComNum.VBLF + "         DelMark, WrtTime, Part, ";
                SQL += ComNum.VBLF + "         Age, ";

                if (clsPmpaPb.GstrErJobFlag == "OK")
                {
                    if (cboDept.Text.Trim() != "PD")
                        SQL += ComNum.VBLF + " PRTOK, ";
                }

                SQL += ComNum.VBLF + "         Emr, JinDtl ) ";
                SQL += ComNum.VBLF + " VALUES (trunc(sysdate), ";
                SQL += ComNum.VBLF + "         " + nSeq + ", ";
                SQL += ComNum.VBLF + "         '" + txtPtno.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + cboDept.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.DrCode + "', ";
                SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.sName + "', ";
                SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.Bi + "', ";
                SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.Chojae + "', ";
                SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.Sinwhan + "', ";
                SQL += ComNum.VBLF + "         '*', ";
                SQL += ComNum.VBLF + "         TO_CHAR(SYSDATE, 'HH24:MI'), ";
                SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                SQL += ComNum.VBLF + "         " + clsPmpaType.TOM.Age + ", ";

                if (clsPmpaPb.GstrErJobFlag == "OK")
                {
                    if (cboDept.Text.Trim() != "PD")
                        SQL += ComNum.VBLF + " 'Y', ";
                }

                SQL += ComNum.VBLF + "         '0', ";
                SQL += ComNum.VBLF + "         '00' ) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    rtnVal = false;
                    return rtnVal;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = false;
                return rtnVal;
            }
        }

        private void SAVE_PROESS(PsmhDb pDbCon)
        {
            //if (ComQuery.IsJobAuth(this, "C", pDbCon) == false) { return; }     //권한확인

            #region //당일 입원 환자인 경우 접수 취소 못함
            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND GBSTS     = '0' ";
            SQL += ComNum.VBLF + "    AND INDATE    >= TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND INDATE    < TO_DATE('" + VB.DateAdd("D", 1, dtpBdate.Text).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            //2018-08-16 원무팀 이채현 강제작업권한 추가
            if (Dt.Rows.Count > 0 && clsPmpaPb.GstrPmpaManager != clsType.User.IdNumber)
            {
                ComFunc.MsgBox("당일 입원환자는 접수취소를 할 수 없습니다.", "확인");
                Dt.Dispose();
                Dt = null;
                return;
            }

            Dt.Dispose();
            Dt = null;
            #endregion

            #region //당일 입원인 경우 외래 모든과가 보증금처리 되기 때문에 해당과가 보증금 처리가 되었다면 접수취소 못함
            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     >= TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND BDATE     < TO_DATE('" + VB.DateAdd("D", 1, dtpBdate.Text).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + cboDept.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND REP       = '#' "; //영수증발행여부(+ 발행, - 환불, # 입원환불)
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            //2018-08-16 원무팀 이채현 강제작업권한 추가
            if (Dt.Rows.Count > 0 && clsPmpaPb.GstrPmpaManager != clsType.User.IdNumber)
            {
                ComFunc.MsgBox("당일 환자는 입원하기 위한 외래환불이 되었기에 접수취소를 할 수 없습니다.", "확인");
                Dt.Dispose();
                Dt = null;
                return;
            }

            Dt.Dispose();
            Dt = null;
            #endregion

            #region //접수건 1개이상일 경우 (응급실은 점검 안함) 접수취소 못함
            if (clsPmpaPb.GstrErJobFlag != "OK")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT COUNT(PANO) CNT ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + cboDept.Text.Trim().ToUpper() + "' ";
                SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(Dt.Rows[0]["CNT"].ToString()) > 1)
                    {
                        ComFunc.MsgBox("당일 환자는 동일 진료과로 접수건이 2개이상이므로 접수취소를 할 수 없습니다.", "확인");
                        Dt.Dispose();
                        Dt = null;
                        return;
                    }
                }

                Dt.Dispose();
                Dt = null;
            }
            #endregion

            #region //진료승인내역 있으면 접수취소 못함
            if (clsPmpaType.TOM.Bi.Equals("21") || clsPmpaType.TOM.Bi.Equals("22"))
            {
                clsPmpaType.BAT.M5_Approve_No = CPF.GET_BOHO_APPROVENO(pDbCon, clsPmpaType.TOM.Pano, clsPmpaType.TOM.BDate, clsPmpaType.TOM.Bi, clsPmpaType.TOM.DeptCode);
                if (clsPmpaType.BAT.M5_Approve_No != "")
                {
                    ComFunc.MsgBox("승인내역이 있으므로 접수취소를 할 수 없습니다.", "확인");
                    return;
                }
            }
            #endregion

            #region //$$12, $$13이 있으면 접수취소 못함
            SQL = "";
            SQL += ComNum.VBLF + " SELECT Sucode ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PAno      = '" + txtPtno.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + cboDept.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND BDate     =  TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND SuCode IN ('$$12','$$13') ";
            SQL += ComNum.VBLF + "  GROUP By Sucode ";
            SQL += ComNum.VBLF + " Having SUM(Qty*Nal) > 0 ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                if (Dt.Rows[0]["SUCODE"].ToString() != "")
                {
                    ComFunc.MsgBox("$$12, $$13코드가 발생하였으므로 접수취소를 할 수 없습니다.", "확인");
                    Dt.Dispose();
                    Dt = null;
                    return;
                }
            }

            Dt.Dispose();
            Dt = null;
            #endregion

            FstrChkBun = "";
            if (chkBun.Checked == true) { FstrChkBun = "1"; }

            #region //저장
            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                if (DELETE_OPD_MASTER(pDbCon) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    return;
                }

                if (clsPmpaType.TOM.GbFlu_Ltd == "Y") //TextEmr 자동 EMR Table INSERT
                {
                    if (VB.Mid(clsPmpaType.TOM.DrCode, 3, 2) == "99")
                        clsQuery.NEW_TextEMR_TreatInterface(pDbCon, txtPtno.Text.Trim(), dtpBdate.Text.Trim(), cboDept.Text.Trim(), "외래", "취소", "600199");
                    else
                        clsQuery.NEW_TextEMR_TreatInterface(pDbCon, txtPtno.Text.Trim(), dtpBdate.Text.Trim(), cboDept.Text.Trim(), "외래", "취소", clsPmpaType.TOM.DrCode);
                }
                else
                    clsQuery.NEW_TextEMR_TreatInterface(pDbCon, txtPtno.Text.Trim(), dtpBdate.Text.Trim(), cboDept.Text.Trim(), "외래", "취소", clsPmpaType.TOM.DrCode);

                if (clsPmpaType.TOM.Reserved == "0")
                {
                    //CPO.Customer_Display("환불", Math.Abs(clsPmpaPb.gnJinAMT7).ToString());

                    if (PRINT_OLD_RETURN(pDbCon) == false) //영수증
                    {
                        clsDB.setRollbackTran(pDbCon);
                        return;
                    }

                    if (clsPmpaType.TOM.Jin == "2" && clsType.User.IdNumber == "111" && cboDept.Text == "GS") //접수||
                    {
                        if (OPD_WORK_DEL_INSERT(pDbCon) == false)
                        {
                            clsDB.setRollbackTran(pDbCon);
                            return;
                        }
                    }
                    else if (clsPmpaType.TOM.Jin != "2" && clsPmpaPb.GstrErJobFlag.Trim() == "")
                    {
                        if (OPD_WORK_DEL_INSERT(pDbCon) == false)
                        {
                            clsDB.setRollbackTran(pDbCon);
                            return;
                        }
                    }
                    else if (clsPmpaPb.GstrErJobFlag.Trim() == "OK" && (clsPmpaType.TOM.DeptCode == "ER" || clsPmpaType.TOM.DeptCode == "PD"))
                    {
                        if (OPD_WORK_DEL_INSERT(pDbCon) == false)
                        {
                            clsDB.setRollbackTran(pDbCon);
                            return;
                        }
                    }
                    else if (clsPmpaType.TOM.Jin != "2" && clsPmpaPb.GstrErJobFlag.Trim() == "OK" && clsPmpaType.TOM.DeptCode == "PD") //야간소아과
                    {
                        if (OPD_WORK_DEL_INSERT(pDbCon) == false)
                        {
                            clsDB.setRollbackTran(pDbCon);
                            return;
                        }
                    }
                }

                if (UPDATE_DOCTOR_SUB(pDbCon) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    return;
                }

                if (UPDATE_DEPT_SUB(pDbCon) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    return;
                }

                if (DELETE_BAS_SHEET(pDbCon) == false) //당일통보서 접수자 삭제
                {
                    clsDB.setRollbackTran(pDbCon);
                    return;
                }

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
            #endregion

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + cboDept.Text.Trim() + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
                ComFunc.MsgBox("접수 취소시 에러 발생함. 전산정보팀으로 전화주시기 바랍니다.", "확인");

            Dt.Dispose();
            Dt = null;

            clsPublic.GstrHelpCode = "OK";
            eFrm_Clear();
            this.Close();
        }

        private bool DELETE_BAS_SHEET(PsmhDb pDbCon)
        {
            bool rtnVal;

            rtnVal = true;

            SQL = "";
            SQL += ComNum.VBLF + " DELETE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SHEET  ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND EntDate   = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + txtPtno.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + cboDept.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND Gubun     = '1' ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = false;
                return rtnVal;
            }

            return rtnVal;
        }

        private bool UPDATE_DEPT_SUB(PsmhDb pDbCon)
        {
            string strSql = string.Empty;
            bool rtnVal;

            rtnVal = true;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'HH24') SYS1 ";
            SQL += ComNum.VBLF + "   FROM DUAL ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                rtnVal = false;
                return rtnVal;
            }

            if (Dt.Rows.Count == 0)
            {
                Dt.Dispose();
                Dt = null;
                rtnVal = false;
                return rtnVal;
            }

            if (string.Compare(Dt.Rows[0]["SYS1"].ToString().Trim(), "00") >= 0 && string.Compare(Dt.Rows[0]["SYS1"].ToString().Trim(), "12") <= 0)
                strSql = " SEQAM = SEQAM - 1 ";
            else
                strSql = " SEQPM = SEQPM - 1 ";

            Dt.Dispose();
            Dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + " BAS_CLINICDEPT ";
            SQL += ComNum.VBLF + "   SET " + strSql;
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + clsPmpaType.TOM.DeptCode + "'  ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = false;
                return rtnVal;
            }

            return rtnVal;
        }

        private bool UPDATE_DOCTOR_SUB(PsmhDb pDbCon)
        {
            string strSql = string.Empty;
            bool rtnVal;

            rtnVal = true;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'HH24') SYS1 ";
            SQL += ComNum.VBLF + "   FROM DUAL ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                rtnVal = false;
                return rtnVal;
            }

            if (Dt.Rows.Count == 0)
            {
                Dt.Dispose();
                Dt = null;
                rtnVal = false;
                return rtnVal;
            }

            if (string.Compare(Dt.Rows[0]["SYS1"].ToString().Trim(), "00") >= 0 && string.Compare(Dt.Rows[0]["SYS1"].ToString().Trim(), "12") <= 0)
                strSql = " SEQAM = SEQAM - 1 ";
            else
                strSql = " SEQPM = SEQPM - 1 ";

            Dt.Dispose();
            Dt = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SeqAM+SeqPM SEQSUM ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND DrCode    = '" + string.Format("{0:0000}", clsPmpaType.TOM.DrCode) + "'  ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                rtnVal = false;
                return rtnVal;
            }

            if (Dt.Rows.Count == 0)
            {
                Dt.Dispose();
                Dt = null;
                rtnVal = false;
                return rtnVal;
            }

            Dt.Dispose();
            Dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + " BAS_DOCTOR ";
            SQL += ComNum.VBLF + "   SET " + strSql;
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND DrCode    = '" + string.Format("{0:0000}", clsPmpaType.TOM.DrCode) + "'  ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = false;
                return rtnVal;
            }

            return rtnVal;
        }

        private bool PRINT_OLD_RETURN(PsmhDb pDbCon)
        {
            clsPmpaPrint CPP = new clsPmpaPrint();

            bool rtnVal;
            long SndOpdno = 0;
            string SndPtno = string.Empty;
            string SndName = string.Empty;
            string SndDept = string.Empty;
            string SndBi = string.Empty;
            string SndBdate = string.Empty;

            rtnVal = true;

            SndOpdno = Convert.ToInt64(VB.Val(txtOpdNo.Text));
            SndPtno = txtPtno.Text.Trim();
            SndDept = txtDeptName.Text.Trim();
            SndName = txtSname.Text.Trim();
            SndBi = clsPmpaType.TOM.Bi;
            SndBdate = dtpBdate.Text;

            clsPmpaPb.gnJinAMT1 = clsPmpaPb.gnJinAMT1 * -1;
            clsPmpaPb.gnJinAMT2 = clsPmpaPb.gnJinAMT2 * -1;
            clsPmpaPb.gnJinAMT3 = clsPmpaPb.gnJinAMT3 * -1;
            clsPmpaPb.gnJinAMT4 = clsPmpaPb.gnJinAMT4 * -1;
            clsPmpaPb.gnJinAMT5 = clsPmpaPb.gnJinAMT5 * -1;
            clsPmpaPb.gnJinAMT6 = clsPmpaPb.gnJinAMT6 * -1;
            clsPmpaPb.gnJinAMT7 = clsPmpaPb.gnJinAMT7 * -1;

            clsPmpaType.RAT[15].Amt1 = clsPmpaPb.gnJinAMT7;
            clsPmpaPb.GstrJeaSunap = "YES";

            if (clsPmpaType.TOM.DeptCode == "HD") //인공신장일 경우
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP  --접수/수납시 등록테이블";
                SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, AMT,           --수납일자,접수번호,등록번호,수납금액";
                SQL += ComNum.VBLF + "         PART, SEQNO, STIME,                  --수납조,수납순번,수납시간";
                SQL += ComNum.VBLF + "         BIGO, REMARK, AMT1,                  --수납비고,수납구분,총진료비";
                SQL += ComNum.VBLF + "         AMT2, AMT3, AMT4,                    --공단부담금,본인부담금,감액";
                SQL += ComNum.VBLF + "         AMT5, SEQNO2, DEPTCODE,              --미수액,영수증일련번호,진료과목";
                SQL += ComNum.VBLF + "         BI, GbSPC)                           --환자유형,선택진료";
                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "          " + SndOpdno + ", ";
                SQL += ComNum.VBLF + "         '" + SndPtno + "', ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT7 + ", ";
                SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                SQL += ComNum.VBLF + "         0 , ";
                SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                SQL += ComNum.VBLF + "         '출력안함', ";
                SQL += ComNum.VBLF + "         'HD접수취소', ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT3 + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT4 + ", ";
                SQL += ComNum.VBLF + "         " + (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT5 + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT6 + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.GDrSeqNo + ", ";
                SQL += ComNum.VBLF + "         '" + cboDept.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + SndBi + "', ";
                SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.GbSpc + "' ) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    rtnVal = false;
                    return rtnVal;
                }
            }
            else if (clsPmpaType.TOM.Jin != "5" && clsPmpaType.TOM.Jin != "E") //5.대리접수 E.전화접수가 아닐 경우
            {
                if (VB.Left(clsPmpaPb.GstrPrtBun, 1) == "2")
                    CPP.Report_Print_Jupsu_A4_New(pDbCon, SndOpdno, SndPtno, SndName, SndDept, " ", " ", SndBi, "", SndBdate, CC.GstrCardJob, "접수취소", cboDept.Text.Trim(), pic_Sign, false, "0", clsPmpaType.TOM.GelCode, clsPmpaType.TOM.MCode, clsPmpaType.TOM.VCode, ssResJupsuPrint);
                else if (VB.Left(clsPmpaPb.GstrPrtBun, 1) == "1")
                    CPP.Report_Print_Jupsu_A4_New(pDbCon, SndOpdno, SndPtno, SndName, SndDept, " ", " ", SndBi, "", SndBdate, CC.GstrCardJob, "접수취소", cboDept.Text.Trim(), pic_Sign, false, "0", clsPmpaType.TOM.GelCode, clsPmpaType.TOM.MCode, clsPmpaType.TOM.VCode, ssResJupsuPrint);
                else if (VB.Left(clsPmpaPb.GstrPrtBun, 1) == "1" && VB.Left(clsPmpaPb.GstrPrtBun2, 1) == "E")
                    CPP.Report_Print_Jupsu_A4_New(pDbCon, SndOpdno, SndPtno, SndName, SndDept, " ", " ", SndBi, "", SndBdate, CC.GstrCardJob, "접수취소", cboDept.Text.Trim(), pic_Sign, false, "0", clsPmpaType.TOM.GelCode, clsPmpaType.TOM.MCode, clsPmpaType.TOM.VCode, ssResJupsuPrint);
                else if (VB.Left(clsPmpaPb.GstrPrtBun, 1) == "5")
                    CPP.Report_Print_Jupsu_A4_New(pDbCon, SndOpdno, SndPtno, SndName, SndDept, " ", " ", SndBi, "", SndBdate, CC.GstrCardJob, "접수취소", cboDept.Text.Trim(), pic_Sign, false, "0", clsPmpaType.TOM.GelCode, clsPmpaType.TOM.MCode, clsPmpaType.TOM.VCode, ssResJupsuPrint);
                else
                    CPP.Report_Print_Jupsu_A4_New(pDbCon, SndOpdno, SndPtno, SndName, SndDept, " ", " ", SndBi, "", SndBdate, CC.GstrCardJob, "접수취소", cboDept.Text.Trim(), pic_Sign, false, "0", clsPmpaType.TOM.GelCode, clsPmpaType.TOM.MCode, clsPmpaType.TOM.VCode, ssResJupsuPrint);

            }
            else if (clsPmpaType.TOM.Jin == "5") //대리접수일 경우
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP ";
                SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, AMT, ";
                SQL += ComNum.VBLF + "         PART, SEQNO, STIME, ";
                SQL += ComNum.VBLF + "         BIGO, REMARK, AMT1, ";
                SQL += ComNum.VBLF + "         AMT2, AMT3, AMT4, ";
                SQL += ComNum.VBLF + "         AMT5, SEQNO2, DEPTCODE, ";
                SQL += ComNum.VBLF + "         BI, GbSPC) ";
                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "          " + SndOpdno + ", ";
                SQL += ComNum.VBLF + "         '" + SndPtno + "', ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT7 + ", ";
                SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                SQL += ComNum.VBLF + "         0, ";
                SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                SQL += ComNum.VBLF + "         '출력안함', ";
                SQL += ComNum.VBLF + "         '대리취소', ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT3 + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT4 + ", ";
                SQL += ComNum.VBLF + "         " + (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT5 + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT6 + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.GDrSeqNo + ", ";
                SQL += ComNum.VBLF + "         '" + cboDept.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + SndBi + "', ";
                SQL += ComNum.VBLF + "         '" + clsPmpaType.TOM.GbSpc + "' ) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    rtnVal = false;
                    return rtnVal;
                }
            }

            return rtnVal;
        }

        private bool DELETE_OPD_MASTER(PsmhDb pDbCon)
        {
            bool rtnVal;

            rtnVal = true;

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_MASTER_DEL ";
            SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, DEPTCODE, ";
            SQL += ComNum.VBLF + "         BI, SNAME, SEX, ";
            SQL += ComNum.VBLF + "         AGE, JICODE, DRCODE, ";
            SQL += ComNum.VBLF + "         RESERVED, CHOJAE, GBGAMEK, GBGAMEKC, ";
            SQL += ComNum.VBLF + "         GBSPC, JIN, MksJin, ";
            SQL += ComNum.VBLF + "         SINGU, BOHUN, CHANGE, ";
            SQL += ComNum.VBLF + "         SHEET, REP, PART, ";
            SQL += ComNum.VBLF + "         JTIME, STIME, FEE1, ";
            SQL += ComNum.VBLF + "         FEE2, FEE3, FEE31, ";
            SQL += ComNum.VBLF + "         FEE5, FEE51, FEE7, ";
            SQL += ComNum.VBLF + "         AMT1, AMT2, AMT3, ";
            SQL += ComNum.VBLF + "         AMT4, AMT5, AMT6, ";
            SQL += ComNum.VBLF + "         AMT7, GELCODE, OCSJIN, ";
            SQL += ComNum.VBLF + "         BDATE, BUNUP, BONRATE, ";
            SQL += ComNum.VBLF + "         TEAGBE, DELDATE, DELGB, ";
            SQL += ComNum.VBLF + "         DELSABUN, DELPART, JinTime,";
            SQL += ComNum.VBLF + "         PNEUMONIA, GBCANCEL, MCode, ";
            SQL += ComNum.VBLF + "         LastDanAmt, JinDtl, INSULIN, ";
            SQL += ComNum.VBLF + "         OUTTIME, Ostime, Ostime2, ";
            SQL += ComNum.VBLF + "         OCtime, OCtime2, ODrCode, ";
            SQL += ComNum.VBLF + "         DrSabun, GwaChoJae, ERPATIENT) ";
            SQL += ComNum.VBLF + "  SELECT ACTDATE, OPDNO, PANO, DEPTCODE, ";
            SQL += ComNum.VBLF + "         BI, SNAME, SEX, ";
            SQL += ComNum.VBLF + "         AGE, JICODE, DRCODE, ";
            SQL += ComNum.VBLF + "         RESERVED, CHOJAE, GBGAMEK, GBGAMEKC, ";
            SQL += ComNum.VBLF + "         GBSPC, JIN, MksJin, ";
            SQL += ComNum.VBLF + "         SINGU, BOHUN, CHANGE, ";
            SQL += ComNum.VBLF + "         SHEET, REP, PART, ";
            SQL += ComNum.VBLF + "         JTIME, STIME, FEE1, ";
            SQL += ComNum.VBLF + "         FEE2, FEE3, FEE31, ";
            SQL += ComNum.VBLF + "         FEE5, FEE51, FEE7, ";
            SQL += ComNum.VBLF + "         AMT1, AMT2, AMT3, ";
            SQL += ComNum.VBLF + "         AMT4, AMT5, AMT6, ";
            SQL += ComNum.VBLF + "         AMT7, GELCODE, OCSJIN, ";
            SQL += ComNum.VBLF + "         BDATE, BUNUP, BONRATE, ";
            SQL += ComNum.VBLF + "         TEAGBE, SYSDATE, '2', ";
            SQL += ComNum.VBLF + "         '" + clsType.User.Sabun + "', '" + clsType.User.IdNumber + "', JinTime, ";
            SQL += ComNum.VBLF + "         PNEUMONIA, '" + FstrChkBun + "', MCode, ";
            SQL += ComNum.VBLF + "         LastDanAmt, JinDtl, INSULIN, ";
            SQL += ComNum.VBLF + "         OUTTIME, Ostime, Ostime2, ";
            SQL += ComNum.VBLF + "         OCtime, OCtime2, ODrCode, ";
            SQL += ComNum.VBLF + "         DrSabun, GwaChoJae, ERPATIENT ";
            SQL += ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "OPD_MASTER  ";
            SQL += ComNum.VBLF + "   WHERE 1        = 1 ";
            SQL += ComNum.VBLF + "     AND ActDate  = TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "     AND Pano     = '" + txtPtno.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "     AND DeptCode = '" + cboDept.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "     AND BDate    = TO_DATE('" + clsPublic.GstrBdate + "','YYYY-MM-DD')  ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = false;
                return rtnVal;
            }

            SQL = "";
            SQL += ComNum.VBLF + " DELETE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";

            if (clsPmpaPb.GstrErJobFlag.Equals("OK"))
            {
                if (cboDept.Text.Trim() == "PD")
                    SQL += ComNum.VBLF + " AND ActDate = TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD') ";
                else
                    SQL += ComNum.VBLF + " AND ActDate = TO_DATE('" + clsPublic.GstrBdate + "','YYYY-MM-DD') ";
            }
            else
            {
                SQL += ComNum.VBLF + " AND ActDate = TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD') ";
            }

            SQL += ComNum.VBLF + "   AND Pano       = '" + txtPtno.Text.Trim() + "'";
            SQL += ComNum.VBLF + "   AND DeptCode   = '" + cboDept.Text.Trim() + "'";
            SQL += ComNum.VBLF + "   AND BDate      = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD') ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = false;
                return rtnVal;
            }

            return rtnVal;
        }

        private void READ_PROESS(PsmhDb pDbCon)
        {
            btnSearch.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;
            //CS.Spread_All_Clear(ssList);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT OPDNO, Sname, DeptCode, Pano, Sex, Age";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND (PART = '" + clsType.User.JobPart + "' ";
            SQL += ComNum.VBLF + "         OR JIN = '5' OR PART = 'Z' OR JIN = 'E') ";
            SQL += ComNum.VBLF + "  ORDER BY Sname ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["OPDNO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["Sname"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["Sex"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["Age"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            CF = new ComFunc();
            CC = new Card();
            CS = new clsSpread();
            CPF = new clsPmpaFunc();
            CPQ = new clsPmpaQuery();
            CPO = new clsOumsad();
            OF = new clsOrdFunction();

            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            this.Location = new System.Drawing.Point(350, 400);
                        
            clsPmpaType.RAT = new clsPmpaType.Return_Amt_Table[20];

            ComFunc.ReadSysDate(clsDB.DbCon);

            LOAD_DEPTCODE();

            cboDept.Items.Clear();

            for (int i = 0; i < 50; i++)
            {
                if (clsPmpaPb.GstrSetDeptCodes[i].Trim().Equals("")) { break; }
                cboDept.Items.Add(clsPmpaPb.GstrSetDeptCodes[i]);
            }
            clsPmpaPb.GstrErJobFlag = "";

            eFrm_Clear();
            if (this.Tag.Equals("TRUE")) { return; }

            FstrFlagOM = "";
            FstrStart = "";

            if (clsPmpaPb.GstrPmpaManager == clsType.User.IdNumber)
                dtpBdate.Enabled = true;

            txtPtno.Select();

            if (FstrPano != "")
            {
                txtPtno.Text = FstrPano;
                SendKeys.Send("{ENTER}");
            }

            if (FstrDept != "")
            {
                cboDept.Text = FstrDept;
                SendKeys.Send("{ENTER}");
            }            
        }

        private void eFrm_Clear()
        {
            clsPmpaPb.GstrLostFocus = "";
            clsPmpaPb.gnJinAMT1 = 0;
            clsPmpaPb.gnJinAMT2 = 0;
            clsPmpaPb.gnJinAMT3 = 0;
            clsPmpaPb.gnJinAMT4 = 0;
            clsPmpaPb.gnJinAMT5 = 0;
            clsPmpaPb.gnJinAMT6 = 0;
            clsPmpaPb.gnJinAMT7 = 0;
            clsPmpaPb.GnGAmt1 = 0;
            clsPmpaPb.GnGAmt2 = 0;
            clsPmpaPb.GnGAmt3 = 0;

            //카드변수초기화
            CC.CardVariable_Clear(ref clsPmpaType.RSD, ref clsPmpaType.RD);

            this.Tag = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            txtPart.Text = "";
            dtpBdate.Text = clsPublic.GstrSysDate;
            clsPublic.GstrBdate = clsPublic.GstrSysDate;
            clsPublic.GstrActDate = clsPublic.GstrSysDate;
            txtPtno.Text = "";
            txtSname.Text = "";
            cboDept.Text = "";
            txtDeptName.Text = "";
            txtDrCode.Text = "";
            txtDrName.Text = "";
            txtChojae.Text = "";
            txtChojaeName.Text = "";
            txtGamek.Text = "";
            txtGamekName.Text = "";
            txtGamekC.Text = "";

            //이중감액 : 주석해제
            //txtGamekCName.Text = "";
            txtSunap.Text = "";
            txtSunapName.Text = "";
            txtRes.Text = "";
            txtResName.Text = "";
            lblMsg.Text = "";

            pnlLeft.Enabled = true;
            btnSave.Enabled = false;

            Array.Clear(clsPmpaType.RAT, 0, clsPmpaType.RAT.Length);
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            txtPtno.Text = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();
            cboDept.Text = ssList_Sheet1.Cells[e.Row, 5].Text.Trim();

            for (int i = 0; i < cboDept.Items.Count; i++)
            {
                if (cboDept.Text == cboDept.Items[i].ToString())
                {
                    nIndex = i;
                    txtDeptName.Text = clsPmpaPb.GstrSetDepts[i];
                    break;
                }
            }

            if (txtPtno.Text.Trim() == "" && cboDept.Text.Trim() == "") { return; }

            FstrFlagOM = CPO.READ_OPD_MASTER(clsDB.DbCon, txtPtno.Text.Trim(), cboDept.Text.Trim(), clsPublic.GstrActDate, clsPublic.GstrBdate);

            if (FstrFlagOM == "NO")
            {
                clsPmpaPb.GstrLostFocus = "";
                lblMsg.Text = "해당환자는 미접수 상태입니다.";
                txtPtno.Focus();
            }
            else if (clsPmpaType.TOM.Part != "222" && clsPmpaType.TOM.Part != clsType.User.IdNumber && clsPmpaType.TOM.Jin != "5" && clsPmpaType.TOM.Jin != "E")
            {
                clsPmpaPb.GstrLostFocus = "";
                lblMsg.Text = clsPmpaType.TOM.Part + "조에서만 취소 가능합니다.";
                txtPtno.Focus();
            }
            else if (clsPmpaType.TOM.Rep.Trim() != "")
            {
                clsPmpaPb.GstrLostFocus = "";
                lblMsg.Text = clsPmpaType.TOM.Part + "수납 처리되어 취소가 불가능합니다.";
                txtPtno.Focus();
            }
            else
            {
                clsPmpaPb.GstrLostFocus = "**";
                FnOldAmts[1] = clsPmpaType.TOM.Amt1;
                FnOldAmts[2] = clsPmpaType.TOM.Amt2;
                FnOldAmts[3] = clsPmpaType.TOM.Amt3;
                FnOldAmts[4] = clsPmpaType.TOM.Amt4;
                FnOldAmts[5] = clsPmpaType.TOM.Amt5;
                FnOldAmts[6] = clsPmpaType.TOM.Amt6;
                FnOldAmts[7] = clsPmpaType.TOM.Amt7;

                clsPmpaPb.gnJinAMT1 = clsPmpaType.TOM.Amt1;
                clsPmpaPb.gnJinAMT2 = clsPmpaType.TOM.Amt2;
                clsPmpaPb.gnJinAMT3 = clsPmpaType.TOM.Amt3;
                clsPmpaPb.gnJinAMT4 = clsPmpaType.TOM.Amt4;
                clsPmpaPb.gnJinAMT5 = clsPmpaType.TOM.Amt5;
                clsPmpaPb.gnJinAMT6 = clsPmpaType.TOM.Amt6;
                clsPmpaPb.gnJinAMT7 = clsPmpaType.TOM.Amt7;

                pnlLeft.Enabled = false;

                DISPLAY_SCREEN(clsDB.DbCon);

                btnSave.Enabled = true;
                btnSave.Select();
            }
        }

        private void LOAD_DEPTCODE()
        {
            for (int i = 0; i < 50; i++)
            {
                clsPmpaPb.GstrSetDeptCodes[i] = "";
                clsPmpaPb.GstrSetDepts[i] = "";
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT * ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND GBJUPSU   = '1' ";
            SQL += ComNum.VBLF + "  ORDER BY PRINTRANKING ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                clsPmpaPb.GstrSetDeptCodes[i] = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                clsPmpaPb.GstrSetDepts[i] = Dt.Rows[i]["DEPTNAMEK"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;
        }
    }
}
