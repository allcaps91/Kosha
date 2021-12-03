using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread.CellType;
using System.Collections;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Text.RegularExpressions;
using System.Collections.Generic;

/// <summary>
/// Description : 예약자 등록 프로그램
/// Author : 박병규
/// Create Date : 2017.08.15
/// </summary>
/// <history>
/// 2017.07.20 : FM 예약승인명단 메뉴는 사용안함
/// </history>
/// <seealso cref="OUMSAD44_new.FRM"/> 

namespace ComPmpaLibB
{
    public partial class frmPmpaMstResvJupsu : Form
    {
        ComboBoxCellType combo = new ComboBoxCellType();

        ComFunc CF = null;
        ComQuery CQ = null;
        clsSpread CS = null;
        Card CC = null;
        clsPmpaFunc CPF = null;
        clsPmpaQuery CPQ = null;
        clsOumsad CPO = null;
        clsOumsadChk COC = null;
        clsPmpaSel CPS = null;
        clsPmpaPrint CPP = null;
        clsOrdFunction OF = null;

        DataTable Dt = new DataTable();
        DataTable DtSub = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;

        clsPmpaType.Table_Resv TRV = new clsPmpaType.Table_Resv();
        clsPmpaType.AcctReqData RSD = new clsPmpaType.AcctReqData();
        clsPmpaType.AcctResData RD = new clsPmpaType.AcctResData();


        string FstrSname = string.Empty;
        string FstrGoodFlag = string.Empty;
        string FstrFlagBP = string.Empty;//환자 MASTER Read Flag(BAS_PATIENT)
        string FstrFlagRV = string.Empty;//예약 MASTER Read Flag(OPD_RESERVE)
        string FstrHjmUpFlag = string.Empty;
        string FstrStart = string.Empty;

        int FnRow = 0;
        string FstrJin = "";
        int FnSpc = 0;
        int FnChoJae = 0;
        int FnBi = 0;
        string FstrGamek = string.Empty;
        string FstrGamekCase = string.Empty;
        string FstrBoil = string.Empty;
        string FstrJangae = string.Empty;
        string FstrRemark = string.Empty;
        string FstrOldData = string.Empty;

        string FstrChk = string.Empty;//이중선택체크
        string FstrRowid_OPDResv = string.Empty;
        string FstrRowid_OPDResvNew = string.Empty;
        string FstrRowid_OCSResv = string.Empty;
        string FstrGwaChoJae = string.Empty;
        //string FstrCboJiyuk = string.Empty;
        //string FstrCboBi = string.Empty;
        int FnInWon = 0; //오전예약인원
        int FnInWon2 = 0; //오후예약인원
        string FstrAmTime = string.Empty;   //오전예약시간
        string FstrAmTime2 = string.Empty;  //오전예약종료
        string FstrPmTime = string.Empty;   //오후예약시간
        string FstrPmTime2 = string.Empty;  //오후예약종료

        string FstrInsDate = string.Empty;
        string FstrInsSname = string.Empty;
        string FstrInsJumin1 = string.Empty;
        string FstrInsJumin2 = string.Empty;
        string FstrInsTel = string.Empty;
        string FstrInsChoJae = string.Empty;
        string FstrInsSpc = string.Empty;
        string FstrInsGamek = string.Empty;
        string FstrInsGamekC = string.Empty;
        string FstrInsJin = string.Empty;
        string FstrInsBi = string.Empty;
        string FstrInsGwange = string.Empty;
        string FstrInsPname = string.Empty;
        string FstrInsKiho = string.Empty;
        string FstrInsGkiho = string.Empty;
        string FstrInsBohun = string.Empty;
        string FstrInsRemark = string.Empty;
        string FstrInsGelCode = string.Empty;

        long[] FnOldAmts = new long[7];//진찰료 수정전 금액

        string FstrComboDoctor  = string.Empty;
        string FstrComboGamek = string.Empty;

        string FstrCommit = string.Empty;
        string FstrDataCheck = string.Empty;

        string FstrTchk = string.Empty;//이중선택 체크
                
        public frmPmpaMstResvJupsu()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);


            //Change Event
            this.txtPtno.TextChanged += new EventHandler(eCtl_Change);
            this.cboBi.TextChanged += new EventHandler(eCtl_Change);
            this.cboDept.TextChanged += new EventHandler(eCtl_Change);
            this.cboResTime.TextChanged += new EventHandler(eCtl_Change);
            this.txtResUptime.TextChanged += new EventHandler(eCtl_Change);

            //MouseWheel
            this.cboBi.MouseWheel += new MouseEventHandler(eCbo_Wheel);
            this.cboDept.MouseWheel += new MouseEventHandler(eCbo_Wheel);
            this.cboDrCode.MouseWheel += new MouseEventHandler(eCbo_Wheel);
            this.cboCan.MouseWheel += new MouseEventHandler(eCbo_Wheel);
            this.cboMcode.MouseWheel += new MouseEventHandler(eCbo_Wheel);
            this.cboChojae.MouseWheel += new MouseEventHandler(eCbo_Wheel);
            this.cboGamek.MouseWheel += new MouseEventHandler(eCbo_Wheel);
            this.cboGamekCase.MouseWheel += new MouseEventHandler(eCbo_Wheel);
            this.cboJin.MouseWheel += new MouseEventHandler(eCbo_Wheel);
            this.cboResTime.MouseWheel += new MouseEventHandler(eCbo_Wheel);


            //GotFocus Event
            this.txtResUptime.GotFocus += new EventHandler(eCtl_GotFocus);

            //KeyPress Event
            this.txtPtno.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboBi.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboDept.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboDrCode.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboCan.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboMcode.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboChojae.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboGamek.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboGamekCase.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboJin.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);

            this.cboResTime.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtResUptime.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);

            //LostFocus Event
            this.txtPtno.LostFocus += new EventHandler(eCtl_LostFocus);
            this.cboBi.LostFocus += new EventHandler(eCtl_LostFocus);
            this.cboDept.LostFocus += new EventHandler(eCtl_LostFocus);
            this.cboDrCode.LostFocus += new EventHandler(eCtl_LostFocus);
            this.cboChojae.LostFocus += new EventHandler(eCtl_LostFocus);
            this.cboGamek.LostFocus += new EventHandler(eCtl_LostFocus);
            this.cboGamekCase.LostFocus += new EventHandler(eCtl_LostFocus);
            this.txtResUptime.LostFocus += new EventHandler(eCtl_LostFocus);

            this.txtResDate.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtResDate.LostFocus += new EventHandler(eCtl_LostFocus);
            this.txtResDate.DoubleClick += new EventHandler(eCtl_DoubleClick);

            this.txtResUpdate.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtResUpdate.LostFocus += new EventHandler(eCtl_LostFocus);
            this.txtResUpdate.DoubleClick += new EventHandler(eCtl_DoubleClick);

            this.chkNhic.Click += new EventHandler(eCtl_Click);
            this.chkCard.Click += new EventHandler(eCtl_Click);
            this.chkCash.Click += new EventHandler(eCtl_Click);

            this.btnSave.Click += new EventHandler(eCtl_Click);
            this.btnDelete.Click += new EventHandler(eCtl_Click);
        
        }
     
        private void eCtl_DoubleClick(object sender, EventArgs e)
        {
            if (sender == this.txtResDate)
                Calendar_Date_Select(txtResDate);
            else if (sender == this.txtResUpdate)
                Calendar_Date_Select(txtResUpdate);
        }

        private void Calendar_Date_Select(Control ArgText)
        {
            clsPublic.GstrCalDate = VB.Trim(ArgText.Text);
            if (VB.Len(VB.Trim(ArgText.Text)) != 10)
            {
                clsPublic.GstrCalDate = clsPublic.GstrSysDate;
            }

            frmCalendar frmCalendarX = new frmCalendar();
            frmCalendarX.ShowDialog();
            OF.fn_ClearMemory(frmCalendarX);


            if (VB.Len(clsPublic.GstrCalDate) == 10)
            {
                ArgText.Text = clsPublic.GstrCalDate;
            }
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.chkCard)
            {
                CardApprov_Amt(this.chkCard, "CARD");
            }
            else if (sender == this.chkCash)
            {
                CardApprov_Amt(this.chkCash, "CASH");
            }
            else if (sender == this.chkNhic)
            {
                txtCanName.Text = "";
                txtMcodeName.Text = "";
                txtJinName.Text = "";

                if (chkNhic.Checked == true)
                {
                    btnKiho.Visible = true;

                    lblCan.Visible = true;
                    cboCan.Visible = true;
                    txtCanName.Visible = true;

                    lblMcode.Visible = true;
                    cboMcode.Visible = true;
                    txtMcodeName.Visible = true;
                }
                else
                {
                    btnKiho.Visible = false;

                    lblCan.Visible = false;
                    cboCan.Visible = false;
                    txtCanName.Visible = false;

                    lblMcode.Visible = false;
                    cboMcode.Visible = false;
                    txtMcodeName.Visible = false;
                }
            }

            else if (sender == this.btnSave)
                Save_Process(clsDB.DbCon);
            else if (sender == this.btnDelete)
                Delete_Process(clsDB.DbCon);
        }

        private void CardApprov_Amt(CheckBox ch, string strJob)
        {
            long nYTot = 0; //영수증 총액

            if (ch.Checked == false) { return; }

            CC.CardVariable_Clear(ref clsPmpaType.RSD, ref clsPmpaType.RD);

            if (txtPtno.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호를 입력하시기 바랍니다.", "확인");
                ch.Checked = false;
                return;
            }


            nYTot = Convert.ToInt64(VB.Replace(clsPmpaPb.gnJinAMT7.ToString(), ",", ""));

            CC.gstrCdPtno = txtPtno.Text.Trim();
            CC.gstrCdSName = txtSname.Text.Trim();
            CC.gstrCdDeptCode = cboDept.Text.Trim();
            CC.gstrCdPart = clsType.User.IdNumber;
            CC.gstrCdGbIo = "O";
            CC.gstrCdPCode = "REV+"; //어느곳에서 카드수납했는지 구분코드
            CC.glngCdAmt = nYTot;

            if (strJob == "CARD")
                CC.GstrCardJob = "";
            else
                CC.GstrCardJob = "Menual2";

            if (clsPmpaPb.GstrCreditBand == "0")
            {
                frmPmpaEntryCardDaou frm = new frmPmpaEntryCardDaou(CC.gstrCdPtno, CC.gstrCdSName, CC.gstrCdDeptCode, CC.gstrCdGbIo, CC.glngCdAmt, strJob, clsPmpaType.TOM.BDate);
                frm.ShowDialog();
                OF.fn_ClearMemory(frm);
            }
            else
            {
                ComFunc.MsgBox("카드 결제사를 세팅해주세요.", "환경세팅 필요");
            }

            ch.Checked = false;
            CC.gstrCdRemark = "";
            sBarMsg1.Text = "";
        }

        private void Delete_Process(PsmhDb pDbCon)
        {
            string strDelOK = string.Empty;
            string strDelRemark = string.Empty;
            Double nJinAMT7 = 0;

            clsPublic.GstrHelpName = CF.READ_JOBSTOP_TIME(pDbCon);
            if (clsPublic.GstrHelpName != "")
            {
                ComFunc.MsgBox(clsPublic.GstrHelpName, "일시중지");
                return;
            }

            if (ComQuery.IsJobAuth(this, "D", pDbCon) == false) { return; }     //권한확인

            ComFunc.MsgBox("당일예약자 삭제는 본인만 가능합니다. 꼭 확인 하시고 작업하시기 바랍니다.", "확인");

            FstrRemark = VB.InputBox("환불 사유를 입력하시기 바랍니다.", "환불사유입력", "");

            if (FstrRemark == "")
            {
                ComFunc.MsgBox("환불 사유가 누락되었습니다. 반드시 사유를 입력하시기 바랍니다.", "확인");
                return;
            }

            if (FstrFlagRV == "OK")
            {
                clsPublic.GstrMsgTitle = "경고";
                clsPublic.GstrMsgList = "해당 환자를 예약마스터에서 삭제합니다." + '\r';
                clsPublic.GstrMsgList += "접수비 환불 영수증이 출력됩니다." + '\r';
                clsPublic.GstrMsgList += "삭제하시겠습니까?" + '\r' + '\r';
                clsPublic.DiResult = ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxDefaultButton.Button1);

                if (TRV.Date1 == clsPublic.GstrActDate)//당일 예약접수자 취소
                {
                    if (clsPublic.DiResult == DialogResult.Yes)
                    {
                        nJinAMT7 = clsPmpaPb.gnJinAMT7 * -1;
                        CC.gstrCdRemark = "";

                        CPO.Customer_Display("환불", clsPmpaPb.gnJinAMT7.ToString());

                        RESV_DELETE(pDbCon);
                        PRINT_PROCESS_RETURN(pDbCon);
                    }
                }
                else//전일 예약접수자 취소
                {
                    if (clsPublic.DiResult == DialogResult.Yes)
                    {
                        nJinAMT7 = clsPmpaPb.gnJinAMT7 * -1;
                        CC.gstrCdRemark = "";

                        CPO.Customer_Display("환불", clsPmpaPb.gnJinAMT7.ToString());

                        RESV_DELETE(pDbCon);
                        PRINT_PROCESS_RETURN(pDbCon);
                    }
                }

                eFrm_Clear();
                strDelOK = "OK";
            }

            btnSave.Enabled = true;

            //if (strDelOK.Equals("OK"))
            //    return;
            //else
            //    dtpResUpdate.Focus();

            //2018.05.20 박병규 : 요청사항적용
            this.Close();
        }

        private string CheckSchedule(string argDRCODE, string argSCHDATE, string argSCHTIME = "")
        {
            //부재중일 경우에만 멘트 띄우는 방식!
            string strResDateTime = argSCHDATE + " " + argSCHTIME;
            string strJinGubun = "";
            string strJinAM = "";
            string strJinPM = "";

            string strMsg = "";

            SQL = " SELECT GBJIN, GBJIN2 ";
            SQL += ComNum.VBLF + " FROM ADMIN.BAS_SCHEDULE ";
            SQL += ComNum.VBLF + " WHERE SCHDATE = TO_DATE('" + argSCHDATE + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND DRCODE = '" + argDRCODE + "'";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
            }

            if (Dt.Rows.Count > 0)
            {
                strJinAM = Dt.Rows[0]["GBJIN"].ToString().Trim();
                strJinPM = Dt.Rows[0]["GBJIN2"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            if (strJinAM != "1" && strJinPM != "1")
            {
                strMsg = "해당의사는 예약일자에 진료가 없습니다." + ComNum.VBLF + "확인하시기 바랍니다.";
            }

            if (argSCHTIME != "")
            {
                if (Convert.ToDateTime(strResDateTime) > Convert.ToDateTime(argSCHDATE + " 12:30"))
                {
                    strJinGubun = "PM";
                }
                else
                {
                    strJinGubun = "AM";
                }

                if (strJinGubun == "AM" && strJinAM != "1")
                {
                    strMsg = "해당의사의 예약일 ★오전★ 진료는 없습니다." + ComNum.VBLF + "확인하시기 바랍니다.";
                }
                else if (strJinGubun == "PM" && strJinPM != "1")
                {
                    strMsg = "해당의사의 예약일 ★오후★ 진료는 없습니다." + ComNum.VBLF + "확인하시기 바랍니다.";
                }
            }
            return strMsg;
        }

        private void Save_Process(PsmhDb pDbCon)
        {
            string strHolyDay_New = string.Empty;
            string strHolyDay_Old = string.Empty;
            string strRDate = string.Empty;
            string strSndDate = string.Empty;
            string strGetFlag = string.Empty;
            string strJiwon = string.Empty;

            clsPublic.GstrHelpName = CF.READ_JOBSTOP_TIME(pDbCon);
            if (clsPublic.GstrHelpName != "")
            {
                ComFunc.MsgBox(clsPublic.GstrHelpName, "일시중지");
                return;
            }

            if (ComQuery.IsJobAuth(this, "C", pDbCon) == false) { return; }     //권한확인

            #region //입력 데이터 점검
            if (txtResDate.Text.Length != 10)
            {
                sBarMsg1.Text = "예약일자 형식이 맞지 않습니다. 재입력 요망!";
                txtResDate.Focus();
                return;
            }

            
            if (FstrFlagRV != "OK" && string.Compare(txtResDate.Text, clsPublic.GstrActDate) <= 0)
            {
                sBarMsg1.Text = "예약일자가 회계일자보다 작습니다.  재입력 요망!";
                txtResDate.Focus();
                return;
            }

            if (txtResUpdate.Text.Trim() != "")
            {
                if (string.Compare(txtResUpdate.Text, clsPublic.GstrActDate) <= 0)
                {
                    sBarMsg1.Text = "예약 변경일자가 회계일자보다 작습니다.  재입력 요망!";
                    txtResUpdate.Focus();
                    return;
                }
            }

            if (CPF.CHECK_HPHONE_NUMBER(txtHphone.Text.Trim()) == false)
            {
                ComFunc.MsgBox("휴대폰번호를 확인하세요. 휴대폰이 없을시 빈 공란으로 입력하시기 바랍니다.", "확인");
                txtHphone.Focus();
                return;

            }

            if (txtResUpdate.Text.Trim() != "")
                strSndDate = txtResUpdate.Text;
            else
                strSndDate = txtResDate.Text;


            strGetFlag = CHECK_OPD_RESV(pDbCon, strSndDate);

            if (strGetFlag.Equals("OK"))
            {
                sBarMsg1.Text = "해당 예약일자에 이미 예약되었습니다.";
                return;
            }

            if (strGetFlag.Equals("OK2"))
            {
                sBarMsg1.Text = "해당 예약일자에 이미 전화예약되었습니다.";
                return;
            }

            if (cboResTime.Text.Trim() == "")
            {
                sBarMsg1.Text = "예약시간을 입력하시기 바랍니다.";
                return;
            }
            else if (cboResTime.Text.Trim().Length != 5)
            {
                sBarMsg1.Text = "예약시간 입력형식이 올바르지 않습니다.(시간:분)";
                return;
            }

            //산재,자보환자 종결일자 Checking
            if (clsPmpaType.TBP.Bi.Equals("31") || clsPmpaType.TBP.Bi.Equals("52"))
            {
                if (CHECK_SANJE_TA(pDbCon, txtResDate.Text).Equals("NO"))
                {
                    sBarMsg1.Text = "예약일자가 종결일자보다 클 수 없습니다.";
                    return;
                }

                if (txtResUpdate.Text.Trim() != "")
                {
                    if (CHECK_SANJE_TA(pDbCon, txtResUpdate.Text).Equals("NO"))
                    {
                        sBarMsg1.Text = "예약 변경일자가 종결일자보다 클 수 없습니다.";
                        return;
                    }
                }
            }

            //전공의 없는과의 의사코드 'xx99','0000' 사용금지
            if (VB.Right(cboDrCode.Text, 2).Equals("99") || cboDrCode.Text.Equals("0000"))
            {
                sBarMsg1.Text = "진료의사코드를 입력하시기 바랍니다.";
                cboDrCode.Focus();
                return;
            }

            if (CPF.CHECK_DOCTOR(pDbCon, cboDrCode.Text) == false)
            {
                ComFunc.MsgBox("진료의사가 퇴사했습니다.", "확인");
                cboDrCode.Focus();
                return;
            }

            if (pnlUp.Enabled == true)
            {
                if (txtResUpdate.Text.Trim() == "")
                {
                    ComFunc.MsgBox("변경일자가 없습니다. 확인 요망", "확인");
                    return;
                }
            }

            if (cboDept.Text.ToUpper().Equals("MD"))
            {
                ComFunc.MsgBox("내과(MD)는 세부 내과로 접수하시기 바랍니다.", "확인");
                return;
            }

            if (clsVbfunc.ChkDateHuIl(pDbCon, txtResDate.Text) == true)
            {
                ComFunc.MsgBox("공휴일은 예약할 수 없습니다.", "확인");
                txtResDate.Focus();
                return;
            }

            if (txtResUpdate.Text.Trim() != "")
            {
                if (clsVbfunc.ChkDateHuIl(pDbCon, txtResUpdate.Text))
                {
                    ComFunc.MsgBox("공휴일은 예약할 수 없습니다.", "확인");
                    txtResDate.Focus();
                    return;
                }
            }

            switch (cboDrCode.Text.Trim())
            {
                case "1102"://이찬우
                case "1120"://김성훈
                    if (Convert.ToInt32(txtPaWon.Text) <= 0)
                    {
                        ComFunc.MsgBox("예약인원이 초과되었습니다. 일자를 변경하시기 바랍니다.", "확인");
                        txtResDate.Focus();
                        return;
                    }

                    break;
            }

            //토요일 12시 이 후에는 예약 않됨.
            if (CF.READ_YOIL(pDbCon, txtResDate.Text) == "토요일")
            {
                if (string.Compare(cboResTime.Text, "12:00") > 0)
                {
                    ComFunc.MsgBox("토요일은 12시 이후에 예약을 할 수 없습니다.", "확인");
                    txtResDate.Focus();
                    return;
                }
            }

            if (txtResUpdate.Text.Trim() != "")
            {
                if (CF.READ_YOIL(pDbCon, txtResUpdate.Text) == "토요일")
                {
                    if (string.Compare(txtResUptime.Text, "12:00") > 0)
                    {
                        ComFunc.MsgBox("토요일은 12시 이후에 예약을 할 수 없습니다.", "확인");
                        txtResUpdate.Focus();
                        return;
                    }
                }
            }

            //평일에서 공휴일로 예약시 변경안됨(반대의 경우도 안됨)
            if (txtResUpdate.Text.Trim() != "")
            {
                clsVbfunc.ChkDateHuIl(pDbCon, txtResDate.Text);
                strHolyDay_New = clsPublic.GstrTempHoliday;
                clsVbfunc.ChkDateHuIl(pDbCon, txtResUpdate.Text);
                strHolyDay_Old = clsPublic.GstrTempHoliday;

                if (strHolyDay_New != strHolyDay_Old)
                {
                    ComFunc.MsgBox("예약금 차액발생!! 변경할 경우 기존 예약취소 후 다시 예약등록 하시기 바랍니다.", "확인");
                    btnDelete.Focus();
                    return;
                }
            }

            if (txtResUpdate.Text.Trim() != "")
                strRDate = txtResUpdate.Text;
            else
                strRDate = txtResDate.Text;

            //기존 전화접수예약이 있는지 확인
            if (COC.CHECK_TEL_RESV(pDbCon, txtPtno.Text, strRDate, cboDept.Text) == true)
            {
                ComFunc.MsgBox("예약일자에 이미 전화예약이 되어있습니다. 기록실 전화접수 취소 후 수납하시기 바랍니다.", "수납불가");
                return;
            }

            if (Convert.ToInt32(VB.Val(txtPaWon.Text)) < 0)
            {
                clsPublic.GstrMsgTitle = "확인";
                clsPublic.GstrMsgList = "예약인원이 초과되었습니다." + '\r';
                clsPublic.GstrMsgList += "예약을 진행하시겠습니까?" + '\r' + '\r';

                clsPublic.DiResult = ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxDefaultButton.Button1);

                if (clsPublic.DiResult == DialogResult.No)
                    return;
            }

            if (clsType.User.IdNumber.Equals("222"))
            {
                clsPublic.GstrMsgTitle = "확인";
                clsPublic.GstrMsgList = "예약비가 종합검진 감액으로 처리됩니다." + '\r';
                clsPublic.GstrMsgList += "해당 환자가 맞습니까?" + '\r' + '\r';

                clsPublic.DiResult = ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxDefaultButton.Button1);

                if (clsPublic.DiResult == DialogResult.No)
                    return;
                else
                {
                    FstrInsGamekC = "56";
                    cboGamekCase.Text = "56";
                }
            }

            if (cboJin.Text.Trim() == "")
            {
                sBarMsg1.Text = "예약 접수구분 공란";
                cboJin.Focus();
                return;
            }

            #endregion

            if (txtDeptName.Text != CF.READ_DEPTNAMEK(clsDB.DbCon, cboDept.Text))
            {
                ComFunc.MsgBox("진료과명 확인요망!!");
                return;
            }

            FstrCommit = "OK";

            DATA_MOVE(pDbCon);

            if (FstrDataCheck.Equals("NO"))
            {
                sBarMsg1.Text = "환자구분 점검요망";
                return;
            }

            //실제입력 ROUTINE

            clsDB.setBeginTran(pDbCon);

            if (FstrHjmUpFlag == "OK")
                Hjm_Update(pDbCon);

            if (FstrFlagRV == "OK")
                Resv_Update(pDbCon);
            else
            {
                CPO.Customer_Display(FstrSname, clsPmpaPb.gnJinAMT7.ToString());

                Resv_Insert(pDbCon);

                if (FstrCommit == "OK")
                {
                    clsPmpaPb.GstrJeaSunap = "YES";
                    PRINT_PROCESS(pDbCon);
                }
            }

            //퇴원환자 예약전달 수납FLAG
            if (FstrRowid_OCSResv != "")
            {
                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_MED + "OCS_RESERVED         --예약 원무과 전달 테이블";
                    SQL += ComNum.VBLF + "    SET GbSunap   = '1'                           --예약수납여부(0.미수납, 1.수납)";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND Pano      = '" + txtPtno.Text.Trim() + "' ";
                    SQL += ComNum.VBLF + "    AND ROWID     = '" + FstrRowid_OCSResv + "' ";
                    SQL += ComNum.VBLF + "    AND GbSunap   = '0' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        Cursor.Current = Cursors.Default;
                        FstrCommit = "NO";
                        return;
                    }
                }
                catch (Exception ex)
                {
                    FstrCommit = "NO";
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }

            //환자마스터 UPDATE
            try
            {
                //#region 환자인적사항 변경 내역 백업
                //Dictionary<string, string> dict = new Dictionary<string, string>();
                //dict.Add("HPHONE", txtHphone.Text.Trim());
                //CF.INSERT_BAS_PATIENT_HIS(txtPtno.Text.Trim(), dict);
                //#endregion
                
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "    SET HPhone    = '" + txtHphone.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND Pano      = '" + txtPtno.Text.Trim() + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    FstrCommit = "NO";
                    return;
                }
            }
            catch (Exception ex)
            {
                FstrCommit = "NO";
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (FstrCommit == "OK")
                clsDB.setCommitTran(pDbCon);
            else
                clsDB.setRollbackTran(pDbCon);

            Cursor.Current = Cursors.Default;
            
            if (cboDept.Text.Trim().Equals("FM"))
            {
                CPQ.UPDATE_FM_RESV(pDbCon, txtSname.Text.Trim(), "FM", "1", txtResDate.Text);
                clsPublic.GFmResvValue = "";
            }

            eFrm_Clear();
            CC.CardVariable_Clear(ref RSD, ref RD);//카드변수 초기화

            this.Close();
        }

        private void Resv_Insert(PsmhDb pDbCon)
        {
            string strJiwon = "";

            if (FstrCommit != "OK") { return; }

            FstrInsDate = txtResDate.Text + " " + cboResTime.Text;

            //의료급여 21종인 금액이 모두 0임.
            if (FstrInsBi == "21")
            {
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
            }

            if (clsPmpaPb.GnGAmt1 > 0 || clsPmpaPb.GnGAmt2 > 0)
                strJiwon = "Y";

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_RESERVED_NEW           --외래 예약자 테이블";
                SQL += ComNum.VBLF + "        (DATE1, PANO, DEPTCODE,                               --접수일자, 등록번호, 진료과";
                SQL += ComNum.VBLF + "         BI, SNAME, DRCODE,                                   --환자구분, 환자명, 의사코드";
                SQL += ComNum.VBLF + "         DATE2, DATE3, CHOJAE,                                --예약일자(시간), 변경일자(시간), 초재구분";
                SQL += ComNum.VBLF + "         GBGAMEK, GBSPC, AMT1,                                --감액구분(자격), 특진구분, 진찰료발생금액";
                SQL += ComNum.VBLF + "         AMT2, AMT3, AMT4,                                    --진찰료특진료, 진찰료총액, 진찰료조합부담";
                SQL += ComNum.VBLF + "         AMT5, AMT6, AMT7,                                    --진찰료감액, 진찰료미수, 진찰료영수금액";
                SQL += ComNum.VBLF + "         DanAmt, LastDanAmt, PART,                            --예약비절사금액,   ,원무조";
                SQL += ComNum.VBLF + "         PRTSEQNO, BOHUN, GELCODE, ";
                SQL += ComNum.VBLF + "         TRANSDATE, TRANSAMT, RETDATE, ";
                SQL += ComNum.VBLF + "         RETAMT, RETPART, CARDSEQNO, ";
                SQL += ComNum.VBLF + "         JIN, MCODE, VCODE, ";
                SQL += ComNum.VBLF + "         GWACHOJAE, JiWon,GBRES, GBGAMEKC) ";
                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrActDate + "', 'YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "         '" + txtPtno.Text + "', ";
                SQL += ComNum.VBLF + "         '" + cboDept.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + FstrInsBi + "', ";
                SQL += ComNum.VBLF + "         '" + FstrInsSname + "', ";
                SQL += ComNum.VBLF + "         '" + cboDrCode.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         TO_DATE('" + FstrInsDate + "', 'YYYY-MM-DD HH24:MI'), ";
                SQL += ComNum.VBLF + "         TO_DATE('" + FstrInsDate + "', 'YYYY-MM-DD HH24:MI'), ";
                SQL += ComNum.VBLF + "         '" + FstrInsChoJae + "', ";
                SQL += ComNum.VBLF + "         '" + FstrInsGamek + "', ";
                SQL += ComNum.VBLF + "         '" + FstrInsSpc + "', ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT1 + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT2 + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT3 + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT4 + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT5 + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT6 + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.gnJinAMT7 + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.GnJinDanAmt + ", ";
                SQL += ComNum.VBLF + "         " + clsPmpaPb.GnJinDanAmt + ", ";
                SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                SQL += ComNum.VBLF + "         0, ";
                SQL += ComNum.VBLF + "         '" + FstrJangae + "', ";
                SQL += ComNum.VBLF + "         '" + FstrInsGelCode + "', ";
                SQL += ComNum.VBLF + "         '', ";
                SQL += ComNum.VBLF + "         '', ";
                SQL += ComNum.VBLF + "         '', ";
                SQL += ComNum.VBLF + "         '', ";
                SQL += ComNum.VBLF + "         '', ";

                if (chkCard.Checked == true)
                    SQL += ComNum.VBLF + "     " + clsPmpaType.RSD.CardSeqNo + ", ";
                else
                    SQL += ComNum.VBLF + "     0, ";

                if (chkNhic.Checked == true)//자격포함
                {
                    SQL += ComNum.VBLF + "     '" + cboJin.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "     '" + cboMcode.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "     '" + cboCan.Text.Trim() + "', ";
                }
                else
                {
                    SQL += ComNum.VBLF + "     '" + FstrInsJin + "', ";
                    SQL += ComNum.VBLF + "     '', ";
                    SQL += ComNum.VBLF + "     '', ";
                }

                SQL += ComNum.VBLF + "         '" + FstrGwaChoJae + "',";
                SQL += ComNum.VBLF + "         '" + strJiwon + "', ";
                SQL += ComNum.VBLF + "         '1', ";
                SQL += ComNum.VBLF + "         '" + FstrInsGamekC + "') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    FstrCommit = "NO";
                    return;
                }
            }
            catch (Exception ex)
            {
                FstrCommit = "NO";
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        private void Resv_Update(PsmhDb pDbCon)
        {
            if (FstrCommit != "OK") { return; }

            if (txtResUpdate.Text.Trim() != "")
                FstrInsDate = txtResUpdate.Text + " " + txtResUptime.Text;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL += ComNum.VBLF + "    SET Bi        = '" + FstrInsBi + "', ";
                SQL += ComNum.VBLF + "        Sname     = '" + FstrInsSname + "', ";
                SQL += ComNum.VBLF + "        DrCode    = '" + cboDrCode.Text + "', ";
                SQL += ComNum.VBLF + "        GwaChoJae = '" + FstrGwaChoJae + "', ";

                if (VB.Left(FstrInsDate, 10) == clsPublic.GstrSysTomorrow)
                {
                    if (VB.Left(FstrInsDate, 10) != VB.Left(TRV.Date3, 10))
                        SQL += ComNum.VBLF + "Date2     = TO_DATE('1995-01-01','YYYY-MM-DD'), ";
                }

                SQL += ComNum.VBLF + "        Date3 = TO_DATE('" + FstrInsDate + "', 'YYYY-MM-DD HH24:MI'), ";
                SQL += ComNum.VBLF + "        PrtSeqNo = 0  ";
                SQL += ComNum.VBLF + "  WHERE ROWID = '" + FstrRowid_OPDResvNew + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    FstrCommit = "NO";
                    return;
                }
            }
            catch (Exception ex)
            {
                FstrCommit = "NO";
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            string strRemark = "접수예약변경" + txtResDate.Text + " " + cboResTime.Text + "→" + FstrInsDate;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP                      --외래접수,수납시 등록테이블";
                SQL += ComNum.VBLF + "        (ACTDATE, PANO, AMT,                                      --수납일자, 등록번호, 수납금액";
                SQL += ComNum.VBLF + "         PART, SEQNO, STIME,                                      --수납조, 수납순번, 수납시간";
                SQL += ComNum.VBLF + "         BIGO, REMARK, DEPTCODE,                                  --수납비고, 수납구분, 진료과목";
                SQL += ComNum.VBLF + "         BI, DELDATE)                                             --자격, 삭제일자";
                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "         '" + txtPtno.Text + "', ";
                SQL += ComNum.VBLF + "         0 , ";
                SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                SQL += ComNum.VBLF + "         0, ";
                SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                SQL += ComNum.VBLF + "         '" + txtResDate.Text + " " + cboResTime.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + strRemark + "', ";
                SQL += ComNum.VBLF + "         '" + cboDept.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         'SS', ";
                SQL += ComNum.VBLF + "         TRUNC(SYSDATE) ) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    FstrCommit = "NO";
                    return;
                }
            }
            catch (Exception ex)
            {
                FstrCommit = "NO";
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void Hjm_Update(PsmhDb pDbCon)
        {
            if (FstrCommit != "OK") { return; }

            try
            {
                //#region 환자인적사항 변경 내역 백업
                //Dictionary<string, string> dict = new Dictionary<string, string>();
                //dict.Add("SNAME", FstrInsSname.Trim());
                //dict.Add("SEX", clsPmpaPb.GstrSex);
                //dict.Add("JUMIN1", FstrInsJumin1);
                //dict.Add("JUMIN2", VB.Left(FstrInsJumin2, 1) + "******");
                //dict.Add("JUMIN3", clsAES.AES(FstrInsJumin2));
                //dict.Add("TEL", FstrInsTel);
                //CF.INSERT_BAS_PATIENT_HIS(txtPtno.Text.Trim(), dict);
                //#endregion


                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "    SET Sname     = '" + FstrInsSname.Trim() + "', ";
                SQL += ComNum.VBLF + "        Sex       = '" + clsPmpaPb.GstrSex + "', ";
                SQL += ComNum.VBLF + "        Jumin1    = '" + FstrInsJumin1 + "', ";
                SQL += ComNum.VBLF + "        Jumin2    = '" + VB.Left(FstrInsJumin2, 1) + "******', ";
                SQL += ComNum.VBLF + "        Jumin3    = '" + clsAES.AES(FstrInsJumin2) + "', ";

                if (FstrInsBi != "51" && string.Compare(FstrInsBi, "53") < 0)
                {
                    SQL += ComNum.VBLF + "       Bi     = '" + FstrInsBi + "', ";
                    SQL += ComNum.VBLF + "       Pname  = '" + FstrInsPname.Trim() + "', ";
                    SQL += ComNum.VBLF + "       Gwange = '" + FstrInsGwange + "', ";
                    SQL += ComNum.VBLF + "       Kiho   = '" + FstrInsKiho + "', ";
                    SQL += ComNum.VBLF + "       Gkiho  = '" + FstrInsGkiho + "', ";
                }

                switch (VB.Left(FstrInsBi, 1))
                {
                    case "2":
                        SQL += ComNum.VBLF + "   Bohun  = '" + FstrInsBohun + "', ";
                        break;
                    default:
                        SQL += ComNum.VBLF + "   Remark = '" + FstrInsRemark + "', ";
                        break;
                }

                SQL += ComNum.VBLF + "       Tel        = '" + FstrInsTel + "' ";
                SQL += ComNum.VBLF + " WHERE Pano       = '" + txtPtno.Text + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    FstrCommit = "NO";
                    return;
                }
            }
            catch (Exception ex)
            {
                FstrCommit = "NO";
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void eCbo_Wheel(object sender, MouseEventArgs e)
        {
            ComboBox CB = sender as ComboBox;

            if (CB.Focused == false)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        private void eCtl_ValueChange(object sender, EventArgs e)
        {
            sBarMsg1.Text = "";

        }

        private void eCtl_GotFocus(object sender, EventArgs e)
        {

            #region //예약일자
            if (sender == this.txtResDate)
            {
                if (txtPtno.Text == "" || cboDept.Text.Trim() == "") { return; }

                FstrGoodFlag = "NO";
                txtDrName.Text = CF.READ_DrName(clsDB.DbCon, cboDrCode.Text.Trim());
                if (txtDrName.Text != "") { FstrGoodFlag = "OK"; }

                if (FstrGoodFlag == "NO") { sBarMsg1.Text = "예약의사를 입력하시기 바랍니다."; }
            }
            #endregion

            #region //변경시간
            if (sender == this.txtResUptime)
            {
                txtResUptime.SelectionStart = 0;
                txtResUptime.SelectionLength = txtResUptime.Text.Length;

                if (txtPtno.Text.Trim().Equals("") || cboDept.Text.Trim().Equals(""))
                    return;

                if (txtResUpdate.Text.Trim() == "")
                {
                    sBarMsg1.Text = "변경일자를 입력하시기 바랍니다.";
                    txtResUpdate.Focus();
                }

            }
            #endregion
        }

        private void READ_RESV_CNT_CHK()
        {
            string strDrCode = string.Empty;
            string strDate = string.Empty;
            string strTime1 = string.Empty;
            string strTime2 = string.Empty;
            int nCnt = 0;//현장예약
            int nTCnt = 0;//전화예약

            strDrCode = cboDrCode.Text.Trim();
            strDate = txtResDate.Text;
            strTime1 = cboResTime.Text.Trim();

            int i = cboResTime.SelectedIndex;

            if (i == cboResTime.Items.Count - 1)
            {
                strTime2 = cboResTime.Text;
            }
            else
            {
                strTime2 = cboResTime.Items[i + 1].ToString();
            }

            //예약인원 COUNT
            SQL = "";
            SQL += ComNum.VBLF + " SELECT COUNT(PANO) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW                                --외래예약자테이블";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND DATE3 >= TO_DATE('" + strDate + strTime1 + "','YYYY-MM-DD HH24:MI')   --변경일자+변경시간";

            if (i == cboResTime.Items.Count - 1)
                SQL += ComNum.VBLF + "AND DATE3 <= TO_DATE('" + strDate + strTime2 + "','YYYY-MM-DD HH24:MI') ";
            else
                SQL += ComNum.VBLF + "AND DATE3 <  TO_DATE('" + strDate + strTime2 + "','YYYY-MM-DD HH24:MI') ";

            SQL += ComNum.VBLF + "   AND DRCODE = '" + strDrCode + "' ";
            SQL += ComNum.VBLF + "   AND RETDATE IS NULL ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count > 0)
                nCnt  = Convert.ToInt32(Dt.Rows[0]["CNT"].ToString());

            Dt.Dispose();
            Dt = null;

            //전화예약인원 COUNT
            SQL = "";
            SQL += ComNum.VBLF + " SELECT COUNT(PANO) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_TELRESV    --전화예약접수 마스터테이블";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND RDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND RTIME >= '" + strTime1 + "' ";

            if (i == cboResTime.Items.Count - 1)
                SQL += ComNum.VBLF + "AND RTIME <= '" + strTime2 + "' ";
            else
                SQL += ComNum.VBLF + "AND RTIME <  '" + strTime2 + "' ";

            SQL += ComNum.VBLF + "   AND DRCODE = '" + strDrCode + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
                nTCnt = Convert.ToInt32(Dt.Rows[0]["CNT"].ToString());

            Dt.Dispose();
            Dt = null;

            if (string.Compare( strTime2, FstrAmTime2) <= 0)
                txtPaWon.Text = (FnInWon - nCnt - nTCnt).ToString();
            else if (string.Compare(strTime2, FstrPmTime2) <= 0)
                txtPaWon.Text = (FnInWon2 - nCnt - nTCnt).ToString();




        }

        private void eCtl_Change(object sender, EventArgs e)
        {

            sBarMsg1.Text = "";

            if (sender == this.cboResTime)
            {
                int nLen = cboResTime.Text.Length;

                if (nLen.Equals(2))
                    cboResTime.Text = cboResTime.Text + ":";
                else if (nLen.Equals(3))
                {
                    cboResTime.SelectionStart = cboResTime.Text.Length;
                    cboResTime.SelectionLength = cboResTime.Text.Length;
                }
            }

            else if (sender == this.txtResUptime)
            {
                int nLen = txtResUptime.Text.Length;

                if (nLen.Equals(2))
                    txtResUptime.Text = txtResUptime.Text + ":";
                else if (nLen.Equals(3))
                {
                    txtResUptime.SelectionStart = txtResUptime.Text.Length;
                    txtResUptime.SelectionLength = txtResUptime.Text.Length;
                }
            }
        }

        private void eCtl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPtno && e.KeyChar == (Char)13)
            {
                txtPtno.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno.Text));
                cboDept.Focus();
            }
                        
            if (sender == this.cboDept && e.KeyChar == (Char)13)
                cboDrCode.Focus();

            if (sender == this.cboDrCode && e.KeyChar == (Char)13)
                cboChojae.Focus();

            if (sender == this.cboCan && e.KeyChar == (Char)13)
                cboMcode.Focus();

            if (sender == this.cboMcode && e.KeyChar == (Char)13)
                cboChojae.Focus();

            if (sender == this.cboChojae && e.KeyChar == (Char)13)
            {
                if (!(VB.IsNumeric(cboChojae.Text)))
                {
                    sBarMsg1.Text = "숫자만 입력하시요 !";
                    cboChojae.Focus();
                }
                cboGamek.Focus();
            }
           
            if (sender == this.cboGamek && e.KeyChar == (Char)13)
                cboJin.Select();

            //이중감액 : 적용시 주석해제
            //if (sender == this.cboGamekCase && e.KeyChar == (Char)13)
            //    cboJin.Focus();
            
            if (sender == this.cboJin && e.KeyChar == (Char)13)
                //   txtResDate.Focus();
                cboBi.Focus();

            if (sender == this.cboBi && e.KeyChar == (Char)13)

                txtResDate.Focus();

                if (sender == this.txtResDate && e.KeyChar == (Char)13)
            {
                if (txtResDate.Text.Length == 8)
                    txtResDate.Text = VB.Left(txtResDate.Text.Trim(), 4) + "-" + VB.Mid(txtResDate.Text.Trim(), 5, 2) + "-" + VB.Right(txtResDate.Text.Trim(), 2);

                cboResTime.Focus();
            }

            //예약시간
            if (sender == this.cboResTime && e.KeyChar == (Char)13)
            {
                FstrInsChoJae = cboChojae.Text;

                READ_RESV_CNT_CHK();

                FnChoJae = Convert.ToInt32( FstrInsChoJae);

                if (chkNhic.Checked == true)//자격포함 선택
                {
                    if (cboJin.Text.Trim() == "")
                    {
                        ComFunc.MsgBox("접수구분 누락입니다");
                        cboJin.Focus();
                        return;
                    }

                    clsPmpaPb.GstrCanCer = cboCan.Text.Trim(); //중증(암)환자 VCODE임

                    //소아6세미만
                    if (ComFunc.AgeCalcEx(clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2, txtResDate.Text) < 6 && string.Compare(FstrBoil, "13") <= 0)
                        CPO.Jin_Amt_Account(clsDB.DbCon, txtPtno.Text, "R", FnChoJae, FnSpc, FstrGamek, FstrGamekCase, FstrBoil, FstrJangae, cboDept.Text.Trim(), cboDrCode.Text.Trim(), txtResDate.Text.Trim() == "" ? clsPublic.GstrSysDate : txtResDate.Text, clsPmpaPb.GnGyeJin, cboMcode.Text.Trim(), cboCan.Text.Trim(), "1", "", "00");
                    else
                        CPO.Jin_Amt_Account(clsDB.DbCon, txtPtno.Text, cboJin.Text.Trim(), FnChoJae, FnSpc, FstrGamek, FstrGamekCase, FstrBoil, FstrJangae, cboDept.Text.Trim(), cboDrCode.Text.Trim(), txtResDate.Text.Trim() == "" ? clsPublic.GstrSysDate : txtResDate.Text, clsPmpaPb.GnGyeJin, cboMcode.Text.Trim(), cboCan.Text.Trim(), "1", "", "00");

                    clsPmpaPb.GstrCanCer = "";
                }
                else
                {
                    //소아6세미만
                    if (ComFunc.AgeCalcEx(clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2, txtResDate.Text) < 6 && String.Compare(FstrBoil, "13") <= 0)
                        CPO.Jin_Amt_Account(clsDB.DbCon, txtPtno.Text, "R", FnChoJae, FnSpc, FstrGamek, FstrGamekCase, FstrBoil, FstrJangae, cboDept.Text.Trim(), cboDrCode.Text.Trim(), txtResDate.Text.Trim() == "" ? clsPublic.GstrSysDate : txtResDate.Text, clsPmpaPb.GnGyeJin, "", "", "1", "", "00");
                    else
                        CPO.Jin_Amt_Account(clsDB.DbCon, txtPtno.Text, cboJin.Text.Trim(), FnChoJae, FnSpc, FstrGamek, FstrGamekCase, FstrBoil, FstrJangae, cboDept.Text.Trim(), cboDrCode.Text.Trim(), txtResDate.Text.Trim() == "" ? clsPublic.GstrSysDate : txtResDate.Text, clsPmpaPb.GnGyeJin, "", "", "1", "", "00");
                }

                CPO.Customer_Display(txtSname.Text, clsPmpaPb.gnJinAMT7.ToString());

                txtResAmt.Text = "예약비 : " + clsPmpaPb.gnJinAMT7;

                txtHphone.Focus();
            }

            if (sender == this.txtResUpdate && e.KeyChar == (Char)13)
                txtResUptime.Focus();

            if (sender == this.txtResUptime && e.KeyChar == (Char)13)
                btnSms.Focus();
        }

        private void eCtl_LostFocus(object sender, EventArgs e)
        {
            int nIndex = 0;
            string strMessage = "";

            #region //등록번호
            if (sender == this.txtPtno)
                READ_RESV_SET(clsDB.DbCon);
            #endregion

            #region //환자구분
            if (sender == this.cboBi)
            {
                if (txtPtno.Text.Trim() == "") { return; }
                if (cboBi.Text.Trim() == "") { return; }

                txtBiName.Text = clsPmpaPb.GstrSetBis[Convert.ToInt32(cboBi.Text.Trim())];
                if (txtBiName.Text == "")
                {
                    cboBi.Text = "11";
                    txtBiName.Text = clsPmpaPb.GstrSetBis[11];
                }
            }
            #endregion

            #region //진료과목
            if (sender == this.cboDept)
            {
                if (txtPtno.Text.Trim() == "") { return; }
                if (cboBi.Text.Trim() == "") { return; }
                if (cboDept.Text.Trim() == "") { return; }
                if (FstrRowid_OCSResv != "") { return; }

                FstrGoodFlag = "NO";
                cboDept.Text = cboDept.Text.ToUpper();
                clsPmpaPb.GstrGwa = cboDept.Text;

                 for (int i =0; i< cboDept.Items.Count; i++)
                 {
                     if (cboDept.Text == cboDept.Items[i].ToString())
                     {
                         nIndex = i;
                         txtDeptName.Text = clsPmpaPb.GstrSetDepts[i];
                        FstrGoodFlag = "OK";
                         break;
                     }
                 }
                //txtDeptName.Text = CF.READ_DEPTNAMEK(clsDB.DbCon, cboDept.Text);
                //FstrGoodFlag = "OK";
                if (FstrGoodFlag == "NO")
                {
                    sBarMsg1.Text = "진료과목코드 Error";
                    cboDept.Focus();
                    return;
                }

                LOAD_COMBO_DOCTOR(nIndex);

                DISPLAY_SCREEN(clsDB.DbCon);
            }
            #endregion

            #region //진료의사
            if (sender == this.cboDrCode)
            {
                FstrGwaChoJae = CPF.READ_GWA_CHOJAE(clsDB.DbCon, txtPtno.Text.Trim(), cboDept.Text.Trim(), cboDrCode.Text.Trim());
                if (FstrGwaChoJae == "C")
                    txtChojae.Text = "과초진";
                else
                    txtChojae.Text = "과재진";
            }
            #endregion

            #region //초재진구분
            if (sender == this.cboChojae)
            {
                txtChojaeName.Text = clsPmpaPb.GstrSetChoJaes[Convert.ToInt32(cboChojae.Text.Trim())];
                if (txtChojaeName.Text == "")
                {
                    cboChojae.Text = "3";
                    txtChojaeName.Text = clsPmpaPb.GstrSetChoJaes[3];
                }

                if (cboJin.Text.Trim() != "" && cboGamek.Text.Trim() != "" && cboGamekCase.Text.Trim() != "")
                    GESAN_JIN_AMT();//진찰료금액계산
            }
            #endregion

            #region //감액구분(자격)
            if (sender == this.cboGamek)
            {
                string strBdate = "";
                string strJumin = "";
                string strBi = "";
                string strGelCode = "";
                string strGamek = "";

                strBdate = txtResDate.Text;

                strJumin = ssPat_Sheet1.Cells[1, 2].Text.ToString();
                strBi = cboBi.Text.Trim();
                strGelCode = ssBohum_Sheet1.Cells[7, 2].Text.ToString();

                if (cboGamek.Text.Trim() == "56")//종합검진후 외래처음진료
                {
                    DataTable Dt = new DataTable();
                    string SQL = string.Empty;
                    string SqlErr = string.Empty;

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, DEPTCODE, SNAME ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                    SQL += ComNum.VBLF + "    AND ACTDATE >= (SELECT MAX(SDATE) ";
                    SQL += ComNum.VBLF + "                      FROM " + ComNum.DB_PMPA + "HEA_JEPSU A, ";
                    //SQL += ComNum.VBLF + "                           " + ComNum.DB_PMPA + "HEA_PATIENT B ";
                    SQL += ComNum.VBLF + "                           " + ComNum.DB_PMPA + "HIC_PATIENT B ";     //2017년도에 변경
                    SQL += ComNum.VBLF + "                     WHERE B.PTNO = '" + txtPtno.Text + "' ";
                    SQL += ComNum.VBLF + "                       AND B.PANO = A.PANO(+) ";
                    SQL += ComNum.VBLF + "                       AND DelDate is null ";
                    SQL += ComNum.VBLF + "                       AND GbSts NOT IN ('0','D')) ";
                    SQL += ComNum.VBLF + "    AND PANO = '" + txtPtno.Text + "' ";
                    SQL += ComNum.VBLF + "    AND GBGAMEK = '56' ";
                    SQL += ComNum.VBLF + "    AND DEPTCODE NOT IN ('TO') ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Dt.Dispose();
                        Dt = null;
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (Dt.Rows.Count > 0)
                    {
                        clsPublic.GstrMsgTitle = "확인";
                        clsPublic.GstrMsgList = "종합건진 감액이 있습니다." + '\r';
                        clsPublic.GstrMsgList += "◆ 감액일자 : " + Dt.Rows[0]["ACTDATE"].ToString() + '\r';
                        clsPublic.GstrMsgList += "◆ 진료과 : " + Dt.Rows[0]["DEPTCODE"].ToString() + '\r';
                        clsPublic.GstrMsgList += "이중 감액을 할 수 없습니다." + '\r';
                        ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);

                        cboGamek.Text = "00";
                        return;
                    }

                    Dt.Dispose();
                    Dt = null;
                }

                if (cboGamek.Text == "55")
                {
                    string strChk = CPF.READ_LTDNAME(clsDB.DbCon, strGelCode);
                    if (clsPmpaPb.GstrMiaFlag == "NO")
                    {
                        ComFunc.MsgBox("계약처 환자인지 확인바랍니다.");
                        cboGamek.Text = "00";
                        ssBohum_Sheet1.Cells[7, 2].Text = "";
                    }

                    if (strGelCode != "H911" && strGelCode != "H20B")
                    {
                        if (strBi == "13")
                        {
                            cboGamek.Text = "00";
                            ssBohum_Sheet1.Cells[7, 2].Text = "";
                        }
                    }
                }

                if (CPF.READ_GAMEK_RATE(clsDB.DbCon, cboGamek.Text, strBi, strBdate, strGelCode, "") == true)
                {
                    if (strGelCode == "H911")//소방전문치료센터 협약 (감액구분 23과 동일하게 적용)
                        CPF.READ_GAMEK_RATE_H911(clsDB.DbCon, cboGamek.Text, strBi, strBdate, strGelCode, "", strJumin);
                    else
                        CPF.READ_GAMEK_RATE_EVENT(clsDB.DbCon, cboGamek.Text, strBi, strBdate, strGelCode, "", strJumin);
                }

                if (strBi == "52" && (cboGamek.Text == "21" || cboGamek.Text == "00"))
                {
                    cboGamek.Text = "00";
                    clsPmpaPb.GstrGamGubun = "00";
                }
                else
                    clsPmpaPb.GstrGamGubun = cboGamek.Text.Trim();

                strGamek = cboGamek.Text.Trim();
                txtGamekName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_감액코드명", strGamek);

                if (txtGamekName.Text == "")
                {
                    cboGamek.Text = "00";
                    txtGamekName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_감액코드명", cboGamek.Text);
                }

                if (cboJin.Text != "" && cboChojae.Text != "")
                    GESAN_JIN_AMT();
            }
            #endregion

            #region //감액구분(CASE)
            if (sender == this.cboGamekCase)
            {
                string strBdate = "";
                string strJumin = "";
                string strBi = "";
                string strGelCode = "";
                string strGamekCase = "";

                strBdate = txtResDate.Text;

                strJumin = ssPat_Sheet1.Cells[1, 2].Text.ToString();
                strBi = cboBi.Text.Trim();
                strGelCode = ssBohum_Sheet1.Cells[7, 2].Text.ToString();

                if (CPF.READ_GAMEK_CASE_RATE(clsDB.DbCon, cboGamekCase.Text, strBi, strBdate, strGelCode, "") == true)
                {
                    if (strGelCode == "H911")//소방전문치료센터 협약 (감액구분 23과 동일하게 적용)
                        CPF.READ_GAMEK_CASE_RATE_H911(cboGamekCase.Text, strBi, strBdate, strGelCode, "", strJumin);
                    else
                        CPF.READ_GAMEK_CASE_RATE_EVENT(cboGamekCase.Text, strBi, strBdate, strGelCode, "", strJumin);
                }

                if (strBi == "52" && cboGamekCase.Text == "50")
                {
                    cboGamekCase.Text = "50";
                    clsPmpaPb.GstrGamCaseGubun = "50";
                }
                else
                    clsPmpaPb.GstrGamCaseGubun = cboGamekCase.Text.Trim();

                strGamekCase = cboGamekCase.Text.Trim();
                txtGamekCaseName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_감액코드명", strGamekCase);

                if (txtGamekCaseName.Text == "")
                {
                    cboGamekCase.Text = "50";//case없음
                    txtGamekCaseName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_감액코드명", cboGamekCase.Text);
                }

                if (cboJin.Text != "" && cboChojae.Text != "")
                    GESAN_JIN_AMT();
            }
            #endregion
            
            #region //예약일자
            if (sender == this.txtResDate)
            {
                //string strGbn = string.Empty;
                //int nTime = 0;

                //if (txtResDate.Text.Trim() == "") { return; }
                //if (VB.Replace(txtResDate.Text, "-", "").Length != 8)
                //{
                //    ComFunc.MsgBox("예약일자 오류!! 정확한 날짜 입력요망!!", "알림");
                //    txtResDate.Select();
                //    return;
                //}

                //if (txtResDate.Text.Length == 8)
                //    txtResDate.Text = VB.Left(txtResDate.Text.Trim(), 4) + "-" + VB.Mid(txtResDate.Text.Trim(), 5, 2) + "-" + VB.Right(txtResDate.Text.Trim(), 2);

                //if (txtResDate.Text.Length == 8)
                //    txtResDate.Text = VB.Left(txtResDate.Text.Trim(), 4) + "-" + VB.Mid(txtResDate.Text.Trim(), 5, 2) + "-" + VB.Right(txtResDate.Text.Trim(), 2);

                //SQL = "";
                //SQL += ComNum.VBLF + " SELECT HOLYDAY ";
                //SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_JOB ";
                //SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                //SQL += ComNum.VBLF + "    AND JOBDATE   = TO_DATE('" + txtResDate.Text + "','YYYY-MM-DD') ";
                //SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                //if (SqlErr != "")
                //{
                //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                //    Dt.Dispose();
                //    Dt = null;
                //    return;
                //}

                //if (Dt.Rows.Count > 0)
                //{
                //    if (Dt.Rows[0]["HOLYDAY"].ToString().Trim() == "*")
                //        sBarMsg1.Text = "해당 예약일은 휴일 입니다.";
                //}

                //Dt.Dispose();
                //Dt = null;

                //SQL = "";
                //SQL += ComNum.VBLF + " SELECT YTIMEGBN, AMTIME, PMTIME, ";
                //SQL += ComNum.VBLF + "        YINWON, AMTIME2, PMTIME2, YINWON2 ";
                //SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                //SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                //SQL += ComNum.VBLF + "    AND DRCODE = '" + cboDrCode.Text.Trim() + "' ";
                //SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                //if (SqlErr != "")
                //{
                //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                //    Dt.Dispose();
                //    Dt = null;
                //    return;
                //}

                //if (Dt.Rows.Count > 0)
                //{
                //    FstrAmTime = Dt.Rows[0]["AMTIME"].ToString().Trim();
                //    FstrAmTime2 = Dt.Rows[0]["AMTIME2"].ToString().Trim();
                //    FstrPmTime = Dt.Rows[0]["PMTIME"].ToString().Trim();
                //    FstrPmTime2 = Dt.Rows[0]["PMTIME2"].ToString().Trim();

                //    FnInWon = Convert.ToInt32(Dt.Rows[0]["YINWON"].ToString().Trim());
                //    FnInWon2 = Convert.ToInt32(Dt.Rows[0]["YINWON2"].ToString().Trim());

                //    strGbn = Dt.Rows[0]["YTIMEGBN"].ToString().Trim();
                //    if (strGbn == "") { strGbn = "4"; }
                //    switch (strGbn)
                //    {
                //        case "1":
                //            nTime = 10;
                //            break;
                //        case "2":
                //            nTime = 15;
                //            break;
                //        case "3":
                //            nTime = 20;
                //            break;
                //        case "4":
                //            nTime = 30;
                //            break;
                //    }
                //}

                //Dt.Dispose();
                //Dt = null;

                //SQL = "";
                //SQL += ComNum.VBLF + " SELECT GBJIN, GBJIN2 ";
                //SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                //SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                //SQL += ComNum.VBLF + "    AND SCHDATE   = TO_DATE('" + txtResDate.Text + "','YYYY-MM-DD') ";
                //SQL += ComNum.VBLF + "    AND DRCODE    = '" + cboDrCode.Text.Trim() + "' ";
                //SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                //if (SqlErr != "")
                //{
                //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                //    Dt.Dispose();
                //    Dt = null;
                //    return;
                //}

                //if (Dt.Rows.Count > 0)
                //{
                //    ssRes_Sheet1.Cells[0, 1].Text = CPF.Get_DrSchedule_Gubun(Dt.Rows[0]["GBJIN"].ToString().Trim());
                //    ssRes_Sheet1.Cells[0, 2].Text = FstrAmTime;
                //    ssRes_Sheet1.Cells[0, 3].Text = FstrAmTime2;

                //    ssRes_Sheet1.Cells[1, 1].Text = CPF.Get_DrSchedule_Gubun(Dt.Rows[0]["GBJIN2"].ToString().Trim());
                //    ssRes_Sheet1.Cells[1, 2].Text = FstrPmTime;
                //    ssRes_Sheet1.Cells[1, 3].Text = FstrPmTime2;

                //    if (FstrRowid_OCSResv == "") { cboResTime.Items.Clear(); }

                //    if (Dt.Rows[0]["GBJIN"].ToString().Trim() == "1")
                //    {
                //        for (int i = CF.TIME_MI(FstrAmTime); i <= CF.TIME_MI(FstrAmTime2); i = i+nTime)
                //            cboResTime.Items.Add(CF.TIME_MI_TIME(i));
                //    }

                //    if (Dt.Rows[0]["GBJIN2"].ToString().Trim() == "1")
                //    {
                //        for (int i = CF.TIME_MI(FstrPmTime); i <= CF.TIME_MI(FstrPmTime2); i = i + nTime)
                //            cboResTime.Items.Add(CF.TIME_MI_TIME(i));
                //    }
                //}
                //else
                //{
                //    ssRes_Sheet1.Cells[0, 1].Text = "";
                //    ssRes_Sheet1.Cells[1, 1].Text = "";
                //}

                //Dt.Dispose();
                //Dt = null;

                //string strJumin = ssPat_Sheet1.Cells[1, 2].Text;
                //strJumin = strJumin + ssPat_Sheet1.Cells[1, 3].Text;

                //clsPmpaPb.GnAge = ComFunc.AgeCalcEx(strJumin, txtResDate.Text);

                //YEYAK_CHOJAE_SET();//진찰료셋팅

                //cboResTime.Focus();
            }
            #endregion

            #region //변경일자
            if (sender == this.txtResUpdate)
            {
                string strGbn = string.Empty;
                int nTime = 0;

                if (txtResUpdate.Text.Trim() == "") { return; }
                if (VB.Replace(txtResUpdate.Text, "-", "").Length != 8)
                {
                    ComFunc.MsgBox("예약일자 오류!! 정확한 날짜 입력요망!!", "알림");
                    txtResDate.Select();
                    return;
                }

                if (txtResUpdate.Text.Length == 8)
                    txtResUpdate.Text = VB.Left(txtResUpdate.Text.Trim(), 4) + "-" + VB.Mid(txtResUpdate.Text.Trim(), 5, 2) + "-" + VB.Right(txtResUpdate.Text.Trim(), 2);


                SQL = "";
                SQL += ComNum.VBLF + " SELECT HOLYDAY ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_JOB ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND JOBDATE   = TO_DATE('" + txtResDate.Text + "','YYYY-MM-DD') ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                }

                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0]["HOLYDAY"].ToString().Trim() == "*")
                        sBarMsg1.Text = "해당 예약일은 휴일 입니다.";
                }

                Dt.Dispose();
                Dt = null;




                SQL = "";
                SQL += ComNum.VBLF + " SELECT YTIMEGBN, AMTIME, PMTIME, ";
                SQL += ComNum.VBLF + "        nvl(YINWON,0) YINWON , AMTIME2, PMTIME2, nvl(YINWON2,0) YINWON2 "; // null 값오류 2018-08-08
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND DRCODE = '" + cboDrCode.Text.Trim() + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                }

                if (Dt.Rows.Count > 0)
                {
                    FstrAmTime = Dt.Rows[0]["AMTIME"].ToString().Trim();
                    FstrAmTime2 = Dt.Rows[0]["AMTIME2"].ToString().Trim();
                    FstrPmTime = Dt.Rows[0]["PMTIME"].ToString().Trim();
                    FstrPmTime2 = Dt.Rows[0]["PMTIME2"].ToString().Trim();

                    FnInWon = Convert.ToInt32(Dt.Rows[0]["YINWON"].ToString().Trim());
                    FnInWon2 = Convert.ToInt32(Dt.Rows[0]["YINWON2"].ToString().Trim());

                    strGbn = Dt.Rows[0]["YTIMEGBN"].ToString().Trim();
                    if (strGbn == "") { strGbn = "4"; }
                    switch (strGbn)
                    {
                        case "1":
                            nTime = 10;
                            break;
                        case "2":
                            nTime = 15;
                            break;
                        case "3":
                            nTime = 20;
                            break;
                        case "4":
                            nTime = 30;
                            break;
                    }
                }

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT GBJIN, GBJIN2 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND SCHDATE   = TO_DATE('" + txtResUpdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DRCODE    = '" + cboDrCode.Text.Trim() + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                }

                if (Dt.Rows.Count > 0)
                {
                    ssUpRes_Sheet1.Cells[0, 1].Text = CPF.Get_DrSchedule_Gubun(Dt.Rows[0]["GBJIN"].ToString().Trim());
                    ssUpRes_Sheet1.Cells[0, 2].Text = FstrAmTime;
                    ssUpRes_Sheet1.Cells[0, 3].Text = FstrAmTime2;

                    ssUpRes_Sheet1.Cells[1, 1].Text = CPF.Get_DrSchedule_Gubun(Dt.Rows[0]["GBJIN2"].ToString().Trim());
                    ssUpRes_Sheet1.Cells[1, 2].Text = FstrPmTime;
                    ssUpRes_Sheet1.Cells[1, 3].Text = FstrPmTime2;

                    if (FstrRowid_OCSResv == "") { txtResUptime.Text = ""; }
                }
                else
                {
                    ssUpRes_Sheet1.Cells[0, 1].Text = "";
                    ssUpRes_Sheet1.Cells[1, 1].Text = "";
                }

                Dt.Dispose();
                Dt = null;



            }
            #endregion

            #region //변경시간
            if (sender == this.txtResUptime)
            {
                string strHH = string.Empty;

                if (txtResUpdate.Text.Trim() == "") { return; }

                strHH = VB.Left(txtResUptime.Text, 2);

                if (strHH == "") { return; }



                SQL = "";
                switch (strHH)
                {
                    case "08":
                    case "09":
                    case "10":
                    case "11":
                    case "12":
                        SQL += ComNum.VBLF + " SELECT GBJIN ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                        SQL += ComNum.VBLF + "  WHERE SCHDATE = TO_DATE('" + txtResUpdate.Text + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND GBJIN NOT IN ('1') ";
                        break;
                    case "13":
                    case "14":
                    case "15":
                    case "16":
                    case "17":
                        SQL += ComNum.VBLF + " SELECT GBJIN2 GBJIN ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                        SQL += ComNum.VBLF + "  WHERE SCHDATE = TO_DATE('" + txtResUpdate.Text + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND GBJIN2 NOT IN ('1') ";
                        break;
                }
                SQL += ComNum.VBLF + "   AND DRCODE ='" + cboDrCode.Text.Trim() + "'";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                if (Dt.Rows.Count >= 1)
                    sBarMsg1.Text = txtDrName.Text + " 과장님 당일 ◆" + CPF.Get_DrSchedule_Gubun(Dt.Rows[0]["GBJIN"].ToString()) + "◆ 입니다.";

                Dt.Dispose();
                Dt = null;
            }

            #endregion
        }

        //진찰료 셋팅
        private void YEYAK_CHOJAE_SET()
        {
            if (FstrFlagRV == "OK") { return; }
            if (txtPtno.Text.Trim() == "") { return; }
            if (cboDept.Text.Trim() == "") { return; }

            if (cboChojae.Text.Trim() == "")
            {
                FstrInsChoJae = "3";
                cboChojae.Text = "3";
                txtChojaeName.Text = clsPmpaPb.GstrSetChoJaes[3];
            }

            FstrInsChoJae = cboChojae.Text.Trim();

            CF.DATE_HUIL_CHECK(clsDB.DbCon, txtResDate.Text);
            if (clsPublic.GstrHoliday.Equals("*") || clsPublic.GstrTempHoliday.Equals("*"))
            {
                if (FstrInsChoJae.Equals("1"))//초진
                {
                    FstrInsChoJae = "5";//초진휴일
                    cboChojae.Text = "5";
                    txtChojaeName.Text = clsPmpaPb.GstrSetChoJaes[5];
                }
                else if (FstrInsChoJae.Equals("3"))//재진
                {
                    FstrInsChoJae = "6";//재진휴일
                    cboChojae.Text = "6";
                    txtChojaeName.Text = clsPmpaPb.GstrSetChoJaes[6];
                }
            }

            //진찰료 display
            FnChoJae = Convert.ToInt32( cboChojae.Text.Trim());
            FstrGamek = cboGamek.Text.Trim();
            FstrGamekCase = cboGamekCase.Text.Trim();
            FstrJin = cboJin.Text.Trim();
            FstrBoil = cboBi.Text.Trim();
            FstrJangae = ssBohum_Sheet1.Cells[6, 2].Text.Trim();
            FstrInsGelCode = ssBohum_Sheet1.Cells[7, 2].Text.Trim();
            FnSpc = 0;

            //종합검진 감액
            if (clsType.User.IdNumber == "222")
                FstrGamekCase = "56";

            if (VB.Left(clsPmpaPb.GstrPrtBun, 1) == "5" && VB.Left(cboDept.Text, 2).Equals("FM"))
                FstrGamekCase = "56";

            if (VB.Left(FstrBoil, 1).Equals("3"))//산재보훈 CLEAR
                FstrJangae = "0";

            //산재공상환자 접수비 미수처리
            clsPmpaPb.GnGyeJin = 0;//산재공상,남부경찰서 후불처리변수(1:후불) Jin_Amt_Account함수에 사용
            if (FstrBoil.Equals("33"))
            {
                if (string.Compare(FstrInsGelCode, "H001") >= 0 && string.Compare(FstrInsGelCode, "H999") >= 0 && FstrJin != "2")
                    clsPmpaPb.GnGyeJin = 1;
            }
            //남부경찰서 접수비 미수처리
            if (FstrInsGelCode.Trim().Equals("H023") || FstrInsGelCode.Trim().Equals("H027"))
                if (FstrJin != "2") { clsPmpaPb.GnGyeJin = 1; }

            if (CPF.READ_GAMEK_RATE(clsDB.DbCon, FstrGamek, FstrBoil, txtResDate.Text, FstrInsGelCode.Trim(),"") == true)
            {
                if (FstrInsGelCode.Trim().Equals("H911") && String.Compare(txtResDate.Text, "2013-12-20") >= 0)
                    CPF.READ_GAMEK_RATE_H911(clsDB.DbCon, FstrGamek, FstrBoil, txtResDate.Text, FstrInsGelCode.Trim(),"", clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2);
                else
                    CPF.READ_GAMEK_RATE_EVENT(clsDB.DbCon, FstrGamek, FstrBoil, txtResDate.Text, FstrInsGelCode.Trim(), "", clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2);
            }

            if (CPF.READ_GAMEK_CASE_RATE(clsDB.DbCon, FstrGamekCase, FstrBoil, txtResDate.Text, FstrInsGelCode.Trim(), "") == true)
            {
                if (FstrInsGelCode.Trim().Equals("H911") && String.Compare(txtResDate.Text, "2013-12-20") >= 0)
                    CPF.READ_GAMEK_CASE_RATE_H911(FstrGamekCase, FstrBoil, txtResDate.Text, FstrInsGelCode.Trim(), "", clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2);
                else
                    CPF.READ_GAMEK_CASE_RATE_EVENT(FstrGamekCase, FstrBoil, txtResDate.Text, FstrInsGelCode.Trim(), "", clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2);
            }

            //계약처미수 대상자 Check(산재공상,남부경찰서) 
            clsPmpaPb.GnGyeJin = 0;
            if (FstrJin != "2" && FstrInsGelCode.Trim() != "")
            {
                if (FstrBoil != "21" && FstrBoil != "31" && FstrBoil != "52")//급여1종, 산재, TA보험
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT Pano gPano ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GYEPANO ";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                    SQL += ComNum.VBLF + "    AND Pano = '" + txtPtno.Text.Trim() + "' ";
                    SQL += ComNum.VBLF + "    AND (Bi1 = '" + FstrBoil + "' OR Bi2 = '" + FstrBoil + "' OR Bi3 = '" + FstrBoil + "' ) ";
                    SQL += ComNum.VBLF + "    AND (DeptCode1 = '" + cboDept.Text.Trim() + "' OR DeptCode2 = '" + cboDept.Text.Trim() + "' OR DeptCode3 = '" + cboDept.Text.Trim() + "' ) ";
                    SQL += ComNum.VBLF + "    AND GelCode = '" + FstrInsGelCode.Trim() + "' ";
                    SQL += ComNum.VBLF + "    AND (ToDate IS NULL OR ToDate >= TO_DATE('" + txtResDate.Text + "','YYYY-MM-DD')) ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        Dt.Dispose();
                        Dt = null;
                        return;
                    }

                    if (Dt.Rows.Count > 0)
                        clsPmpaPb.GnGyeJin = 1;//후불처리

                    Dt.Dispose();
                    Dt = null;
                }
            }


            if (chkNhic.Checked == true)//자격포함 선택
            {
                if (cboJin.Text.Trim() == "")
                {
                    ComFunc.MsgBox("접수구분 누락입니다");
                    cboJin.Focus();
                    return;
                }

                clsPmpaPb.GstrCanCer = cboCan.Text.Trim(); //중증(암)환자 VCODE임

                //소아6세미만
                if (ComFunc.AgeCalcEx(clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2, txtResDate.Text) < 6 && String.Compare(FstrBoil, "13") <= 0)
                    CPO.Jin_Amt_Account(clsDB.DbCon, txtPtno.Text, "R", FnChoJae, FnSpc, FstrGamek, FstrGamekCase, FstrBoil, FstrJangae, cboDept.Text.Trim(), cboDrCode.Text.Trim(), txtResDate.Text.Trim() == "" ? clsPublic.GstrSysDate : txtResDate.Text, clsPmpaPb.GnGyeJin, cboMcode.Text.Trim(), cboCan.Text.Trim(), "1", "","00"  );
                else
                    CPO.Jin_Amt_Account(clsDB.DbCon, txtPtno.Text, cboJin.Text.Trim(), FnChoJae, FnSpc, FstrGamek, FstrGamekCase, FstrBoil, FstrJangae, cboDept.Text.Trim(), cboDrCode.Text.Trim(), txtResDate.Text.Trim() == "" ? clsPublic.GstrSysDate : txtResDate.Text, clsPmpaPb.GnGyeJin, cboMcode.Text.Trim(), cboCan.Text.Trim(), "1", "", "00");

                clsPmpaPb.GstrCanCer = "";
            }
            else
            {
                //소아6세미만
                if (ComFunc.AgeCalcEx(clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2, txtResDate.Text) < 6 && String.Compare(FstrBoil, "13") <= 0)
                    CPO.Jin_Amt_Account(clsDB.DbCon, txtPtno.Text, "R", FnChoJae, FnSpc, FstrGamek, FstrGamekCase, FstrBoil, FstrJangae, cboDept.Text.Trim(), cboDrCode.Text.Trim(), txtResDate.Text.Trim() == "" ? clsPublic.GstrSysDate : txtResDate.Text, clsPmpaPb.GnGyeJin, "", "", "1", "", "00");
                else
                    CPO.Jin_Amt_Account(clsDB.DbCon, txtPtno.Text, cboJin.Text.Trim(), FnChoJae, FnSpc, FstrGamek, FstrGamekCase, FstrBoil, FstrJangae, cboDept.Text.Trim(), cboDrCode.Text.Trim(), txtResDate.Text.Trim() == "" ? clsPublic.GstrSysDate : txtResDate.Text, clsPmpaPb.GnGyeJin, "", "", "1", "", "00");
            }

            string strTemp = string.Empty;

            if (COC.CHECK_TM_OPD(clsDB.DbCon, txtPtno.Text, txtResDate.Text, cboDept.Text))//퇴원후 외래 F/U시 90일 이내면 재진료 산정
            {
                if (clsPublic.GstrHoliday.Equals("*") || clsPublic.GstrTempHoliday.Equals("*"))
                {
                    cboChojae.Text = "6";
                    txtChojaeName.Text = clsPmpaPb.GstrSetChoJaes[6];
                }
                else
                {
                    cboChojae.Text = "3";
                    txtChojaeName.Text = clsPmpaPb.GstrSetChoJaes[3];
                }

                strTemp = "OK";
            }

            if (strTemp != "OK")
            {
                if (COC.CHECK_IPD_CONSULT_OPD(clsDB.DbCon, txtPtno.Text, txtResDate.Text, cboDept.Text))//입원중 협진과 90일 이내면  재진료 산정
                {
                    if (clsPublic.GstrHoliday.Equals("*") || clsPublic.GstrTempHoliday.Equals("*"))
                    {
                        cboChojae.Text = "6";
                        txtChojaeName.Text = clsPmpaPb.GstrSetChoJaes[6];
                    }
                    else
                    {
                        cboChojae.Text = "3";
                        txtChojaeName.Text = clsPmpaPb.GstrSetChoJaes[3];
                    }
                }
            }

            CPO.Customer_Display(txtSname.Text, clsPmpaPb.gnJinAMT7.ToString());

            txtResAmt.Text = "예약비 : " + clsPmpaPb.gnJinAMT7;
        }

        //진찰료금액계산
        private void GESAN_JIN_AMT()
        {
            clsPmpaPb.GnGyeJin = 0;

            string strJin = cboJin.Text.Trim();
            FnChoJae = Convert.ToInt32(cboChojae.Text.Trim());
            FstrGamek = cboGamek.Text.Trim();
            FstrGamekCase = cboGamekCase.Text.Trim();
            string strBdate = txtResDate.Text;

            if (strBdate == "") { strBdate = clsPublic.GstrSysDate; }
            string strJumin = ssPat_Sheet1.Cells[1, 2].Text.Trim();
            string strBi = cboBi.Text.Trim();
            string strJangae = ssBohum_Sheet1.Cells[6, 2].Text.Trim();
            string strInsGelCode = ssBohum_Sheet1.Cells[7, 2].Text.Trim();

            if (FstrJin != "2" && strInsGelCode != "")
            {
                if (strBi != "21" && strBi != "31" && strBi != "52")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT Pano gPano ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GYEPANO ";
                    SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                    SQL += ComNum.VBLF + "    AND Pano  = '" + txtPtno.Text + "' ";
                    SQL += ComNum.VBLF + "    AND (Bi1 = '" + strBi + "' OR Bi2 = '" + strBi + "' OR Bi3 = '" + strBi + "' ) ";
                    SQL += ComNum.VBLF + "    AND (DeptCode1 = '" + cboDept.Text + "' OR DeptCode2 = '" + cboDept.Text + "' OR DeptCode3 = '" + cboDept.Text + "') ";
                    SQL += ComNum.VBLF + "    AND GelCode = '" + strInsGelCode + "' ";
                    SQL += ComNum.VBLF + "    AND (ToDate IS NULL OR ToDate >= TO_DATE('" + strBdate + "','YYYY-MM-DD')) ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        Dt.Dispose();
                        Dt = null;
                        return;
                    }

                    if (Dt.Rows.Count > 0)
                        clsPmpaPb.GnGyeJin = 1; //후불처리

                    Dt.Dispose();
                    Dt = null;
                }
            }

            if (strBi != "22")
                strJangae = "0";

            if (FnChoJae > 0)
            {
                CPO.Jin_Amt_Account(clsDB.DbCon, txtPtno.Text, strJin, FnChoJae, FnSpc, FstrGamek, FstrGamekCase, strBi, strJangae, cboDept.Text.Trim(), cboDrCode.Text.Trim(), strBdate.Trim() == "" ? clsPublic.GstrSysDate : strBdate, clsPmpaPb.GnGyeJin, cboMcode.Text.Trim(), cboCan.Text.Trim(), "1", "", "00");
                //CPO.Customer_Display(FstrSname, clsPmpaPb.gnJinAMT7.ToString());
                txtResAmt.Text = "예약비 : " + string.Format("{0:#,##0}", clsPmpaPb.gnJinAMT7);
            }
        }

        private void LOAD_COMBO_DOCTOR(int nIndex)
        {
            int nCode = 0;
            string strCode = string.Empty;

            cboDrCode.Items.Clear();

            for (int i =0; i < 100; i++)
            {
                if (clsPmpaPb.gstrSetDrCodes[nIndex, i] == "")
                    break;

                nCode = Convert.ToInt32(VB.Mid( clsPmpaPb.gstrSetDrCodes[nIndex, i],3,2));
                strCode = clsPmpaPb.gstrSetDrCodes[nIndex, i];

                cboDrCode.Items.Add(strCode);
                clsPmpaPb.GstrSetDoctors[nCode] = clsPmpaPb.gstrSetDrNames[nIndex, i];
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT A.Pano, A.DeptCode, ";
            SQL += ComNum.VBLF + "       TO_CHAR(A.LastDate,'YYYY-MM-DD') LastDate, ";
            SQL += ComNum.VBLF + "       A.LastTuyak, A.LastIll, A.DrCode, ";
            SQL += ComNum.VBLF + "       A.GbSpc, A.GbGamek, ";
            SQL += ComNum.VBLF + "       TO_CHAR(A.LastOldDate,'YYYY-MM-DD') LastOldDate ";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_LASTEXAM A, ";
            SQL += ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_DOCTOR B ";
            SQL += ComNum.VBLF + " WHERE 1          = 1 ";
            SQL += ComNum.VBLF + "   AND A.Pano     = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "   AND A.DeptCode = '" + cboDept.Text + "' ";
            SQL += ComNum.VBLF + "   AND B.Tour     = 'N' ";
            SQL += ComNum.VBLF + "   AND A.DrCode   = B.DrCode(+) ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
                cboDrCode.Text = Dt.Rows[0]["DrCode"].ToString().Trim();

            Dt.Dispose();
            Dt = null;

        }

        private void READ_RESV_SET(PsmhDb pDbCon)
        {
            string strRdate = string.Empty;

            CC.CardVariable_Clear(ref clsPmpaType.RSD, ref clsPmpaType.RD);

            if (txtPtno.Text == null || txtPtno.Text.Trim() == "") return;

            //등록번호 입력체크
            if (VB.IsNumeric(txtPtno.Text.Trim()) == false && VB.IsHangul(txtPtno.Text.Trim()) == false)
            {
                txtPtno.Text = "";
                return;
            }
            if (!VB.IsNumeric(txtPtno.Text.Trim()))
            {
                sBarMsg1.Text = "등록번호 ReEnter !!";
                txtPtno.Text = "";
                txtPtno.Focus();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            txtPtno.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno.Text.Trim()));
            FstrFlagBP = CPO.READ_BAS_PATIENT(pDbCon, txtPtno.Text.Trim());
            txtSname.Text = clsPmpaType.TBP.Sname;

            if (FstrFlagBP == "NO")
            {
                BaseAPI.Beep(3000, 200);
                sBarMsg1.Text = "해당번호 환자 MASTER에 존재하지 않음 !!";
                txtPtno.Text = "";
                txtPtno.Focus();

                return;
            }

            //내시경 수납 여부 확인함.
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') RDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "ENDO_JUPMST ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PTNO  = '" + txtPtno.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND RDATE = TRUNC(SYSDATE) ";
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

            strRdate = "";
            if (Dt.Rows.Count > 0)
                strRdate = Dt.Rows[0]["RDATE"].ToString().Trim();

            Dt.Dispose();
            Dt = null;

            if (strRdate != "")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER ";
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND PTNO  = '" + txtPtno.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND BUN   = '48' "; //내시경 분류번호
                SQL += ComNum.VBLF + "    AND BDATE >= TO_DATE( '" + VB.DateAdd("D", -21, strRdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND BDATE <= TO_DATE( '" + strRdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND GBSUNAP  = '0' ";//수납여부(0.미수납, 1.수납)
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
                {
                    clsPublic.GstrMsgTitle = "확인";
                    clsPublic.GstrMsgList = "내시경 미수납이 확인됨 ==>" + '\r';
                    clsPublic.GstrMsgList += "★ 처방일자 : " + Dt.Rows[0]["BDATE"].ToString().Trim() + '\r';
                    clsPublic.GstrMsgList += "★ 반드시 수납하시기 바랍니다." + '\r';

                    ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                }

                Dt.Dispose();
                Dt = null;
            }

            FnRow = 1;

            //재원자 확인
            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "    AND OUTDATE IS NOT NULL ";
            SQL += ComNum.VBLF + "    AND ( ActDate IS NULL OR ActDate ='') ";
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
                READ_TEWON_YEYAK(pDbCon);//퇴원자 예약

            Dt.Dispose();
            Dt = null;

            READ_OPDRESV_ALL(pDbCon);
            pnlLeft.Enabled = false;

            if (FstrFlagRV == "NO")
            {
                ssPat.Enabled = true;
                ssBohum.Enabled = true;
                pnlLeft.Enabled = true;
                chkCard.Enabled = true;

                DISPLAY_SCREEN(pDbCon);
                cboDept.Select();
            }
            else
            {
                pnlCount.Visible = true;
                btnCancel.Enabled = false;
                btnSelOK.Enabled = true;
                btnSelNO.Enabled = true;
                ssList.Focus();

                return;
            }



            btnSave.Enabled = true;

            if (clsType.User.IdNumber == "222")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT OPDNO, TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, DEPTCODE, SNAME ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL += ComNum.VBLF + "  WHERE 1       = 1 ";
                SQL += ComNum.VBLF + "    AND ACTDATE >= (SELECT MAX(SDATE) From HEA_JEPSU A, HEA_PATIENT B ";
                SQL += ComNum.VBLF + "                    WHERE B.PTNO = '" + txtPtno.Text + "' ";
                SQL += ComNum.VBLF + "                      AND B.PANO = A.PANO(+) ";
                SQL += ComNum.VBLF + "                      AND  DelDate is null ";
                SQL += ComNum.VBLF + "                      AND  GbSts NOT IN ('0','D')) ";
                SQL += ComNum.VBLF + "    AND PANO    = '" + txtPtno.Text + "' ";
                SQL += ComNum.VBLF + "    AND GBGAMEKC = '56' ";
                SQL += ComNum.VBLF + "    AND DEPTCODE NOT IN ('TO') ";
                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    DtSub.Dispose();
                    DtSub = null;
                    return;
                }

                if (DtSub.Rows.Count > 0)
                {
                    txtRemark.Text = "접수번호: " + DtSub.Rows[0]["OPDNO"].ToString();
                    txtRemark.Text += "   진료일자 : " + DtSub.Rows[0]["ACTDATE"].ToString();
                    txtRemark.Text += "   진료과 : " + DtSub.Rows[0]["DEPTCODE"].ToString();
                    ComFunc.MsgBox("종합건진 감액으로 예약을 할 수 없습니다");
                    btnSave.Enabled = false;
                }
                else
                    txtRemark.Text = "감액 내역이 없음";

                DtSub.Dispose();
                DtSub = null;
            }

            Cursor.Current = Cursors.Default;
        }

        private void DISPLAY_SCREEN(PsmhDb pDbCon)
        {
            string strTBPat = string.Empty;

            if (clsPmpaType.TBP.Ptno.Trim() == null) { return; }

            //환자내역 DISPLAY
            clsPmpaPb.GnAge = ComFunc.AgeCalcEx(clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2, clsPublic.GstrSysDate);
            clsPmpaPb.GstrSex = clsPmpaType.TBP.Sex;

            FstrSname = clsPmpaType.TBP.Sname;
            ssPat_Sheet1.Cells[0, 2].Text = clsPmpaType.TBP.Sname;
            ssPat_Sheet1.Cells[0, 3].Text = clsPmpaPb.GstrSex + "/" + clsPmpaPb.GnAge;
            ssPat_Sheet1.Cells[1, 2].Text = clsPmpaType.TBP.Jumin1;
            ssPat_Sheet1.Cells[1, 3].Text = clsPmpaType.TBP.Jumin2;
            ssPat_Sheet1.Cells[2, 2].Text = clsPmpaType.TBP.Tel;
            
            txtHphone.Text = clsPmpaType.TBP.HPhone.Trim();

            if (clsPmpaType.TBP.GbSMS.Trim().Equals("Y"))
                rdoSMS0.Checked = true;
            else if (clsPmpaType.TBP.GbSMS.Trim().Equals("N"))
                rdoSMS1.Checked = true;
            else if (clsPmpaType.TBP.GbSMS.Trim().Equals("X"))
                rdoSMS2.Checked = true;
            else 
                rdoSMS1.Checked = true;
            

            //진료내역 DISPLAY
            if (string.Compare(clsPmpaType.TBP.Sabun, "H001") >= 0 && string.Compare(clsPmpaType.TBP.Sabun, "H999") <= 0)
            {
                if (clsPmpaType.TBP.Sabun.Equals("H080"))//해군포항병원
                {
                    clsPmpaPb.GstrGamGubun = "00";
                    clsPmpaPb.GstrGamMsg = CF.Read_Bcode_Name(pDbCon, "BAS_감액코드명", clsPmpaPb.GstrGamGubun);
                }
                else
                {
                    clsPmpaPb.GstrGamGubun = "55";
                    clsPmpaPb.GstrGamMsg = CF.Read_Bcode_Name(pDbCon, "BAS_감액코드명", clsPmpaPb.GstrGamGubun);
                }

                CPF.READ_GAMEK_RATE(pDbCon, clsPmpaPb.GstrGamGubun, clsPmpaType.TBP.Bi, txtResDate.Text, clsPmpaType.TBP.Sabun, "");
                CPF.READ_GAMEK_CASE_RATE(pDbCon, clsPmpaPb.GstrGamGubun, clsPmpaType.TBP.Bi, txtResDate.Text, clsPmpaType.TBP.Sabun, "");

                if (clsPmpaType.TBP.Sabun.Equals("H911"))
                {
                    CPF.READ_GAMEK_RATE_H911(pDbCon, clsPmpaPb.GstrGamGubun, clsPmpaType.TBP.Bi, txtResDate.Text, clsPmpaType.TBP.Sabun, "", clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2);
                    CPF.READ_GAMEK_CASE_RATE_H911(clsPmpaPb.GstrGamGubun, clsPmpaType.TBP.Bi, txtResDate.Text, clsPmpaType.TBP.Sabun, "", clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2);
                }
                else
                {
                    CPF.READ_GAMEK_RATE_EVENT(pDbCon, clsPmpaPb.GstrGamGubun, clsPmpaType.TBP.Bi, txtResDate.Text, clsPmpaType.TBP.Sabun, "", clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2);
                    CPF.READ_GAMEK_CASE_RATE_EVENT(clsPmpaPb.GstrGamGubun, clsPmpaType.TBP.Bi, txtResDate.Text, clsPmpaType.TBP.Sabun, "", clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2);
                }
            }
            else
            {
                CPQ.READ_BAS_GAMF(pDbCon, clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2);
                
                if ((clsPmpaType.TBP.Bi.Equals("31") || clsPmpaType.TBP.Bi.Equals("52") || clsPmpaType.TBP.Bi.Equals("55")) && string.Compare(clsPmpaPb.GstrGamGubun, "11") > 0)//산재,자보,자보일반 할인제외
                    clsPmpaPb.GstrFlagGam = "NO";

                if (clsPmpaPb.GstrFlagGam.Equals("OK"))
                {
                    if (clsPmpaPb.GstrGamEnd != "")
                    {
                        if (string.Compare(clsPublic.GstrActDate, clsPmpaPb.GstrGamEnd) > 0)
                        {
                            clsPmpaPb.GstrGamGubun = "00";
                            clsPmpaPb.GstrGamMsg = CF.Read_Bcode_Name(pDbCon, "BAS_감액코드명", clsPmpaPb.GstrGamGubun);
                        }
                    }
                }
                else
                {
                    clsPmpaPb.GstrGamGubun = "00";
                    clsPmpaPb.GstrGamMsg = CF.Read_Bcode_Name(pDbCon, "BAS_감액코드명", clsPmpaPb.GstrGamGubun);
                }

                CPF.READ_GAMEK_RATE(pDbCon, clsPmpaPb.GstrGamGubun, clsPmpaType.TBP.Bi, txtResDate.Text, clsPmpaType.TBP.Sabun, "");
                CPF.READ_GAMEK_CASE_RATE(pDbCon, clsPmpaPb.GstrGamGubun, clsPmpaType.TBP.Bi, txtResDate.Text, clsPmpaType.TBP.Sabun, "");

                if (clsPmpaType.TBP.Sabun.Equals("H911"))
                {
                    CPF.READ_GAMEK_RATE_H911(pDbCon, clsPmpaPb.GstrGamGubun, clsPmpaType.TBP.Bi, txtResDate.Text, clsPmpaType.TBP.Sabun, "", clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2);
                    CPF.READ_GAMEK_CASE_RATE_H911(clsPmpaPb.GstrGamGubun, clsPmpaType.TBP.Bi, txtResDate.Text, clsPmpaType.TBP.Sabun, "", clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2);
                }
                else
                {
                    CPF.READ_GAMEK_RATE_EVENT(pDbCon, clsPmpaPb.GstrGamGubun, clsPmpaType.TBP.Bi, txtResDate.Text, clsPmpaType.TBP.Sabun, "", clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2);
                    CPF.READ_GAMEK_CASE_RATE_EVENT(clsPmpaPb.GstrGamGubun, clsPmpaType.TBP.Bi, txtResDate.Text, clsPmpaType.TBP.Sabun, "", clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2);
                }
            }

            cboChojae.Text = "3";
            cboGamek.Text = clsPmpaPb.GstrGamGubun;

            if (clsPmpaPb.GstrGamCaseGubun == "")
                cboGamekCase.Text = "50";
            else
                cboGamekCase.Text = clsPmpaPb.GstrGamCaseGubun;

            if (clsPmpaPb.GnAge <= 5)
                cboJin.Text = "R";
            else
                cboJin.Text = "0";

            CS.Spread_All_Clear(ssJin);

            int nRead = 0;
            strTBPat = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT OPDNO, TO_CHAR(ACTDATE, 'YYYY-MM-DD') ACTDATE, DEPTCODE, ";
            SQL += ComNum.VBLF + "        DRCODE, ADMIN.FC_BAS_DOCTOR_DRNAME(DRCODE) DRNAME, ";
            SQL += ComNum.VBLF + "        MCODE, VCode  ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "  ORDER BY ACTDATE DESC ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                if (Dt.Rows.Count > 9)
                    nRead = 9;
                else
                    nRead = Dt.Rows.Count;

                ssJin_Sheet1.RowCount = nRead;
                ssJin_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < nRead; i++)
                {
                    ssJin_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["OPDNO"].ToString().Trim();
                    ssJin_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["ACTDATE"].ToString().Trim();
                    ssJin_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssJin_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DRNAME"].ToString().Trim();
                    ssJin_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["MCODE"].ToString().Trim();

                    if (strTBPat == "")
                    {
                        if (Dt.Rows[i]["VCode"].ToString().Trim().Equals("V246") || Dt.Rows[i]["VCode"].ToString().Trim().Equals("V231") || Dt.Rows[i]["VCode"].ToString().Trim().Equals("V206"))
                        {
                            strTBPat = "이전 결핵 접수내역 →";
                            strTBPat += " 접수번호:" + Dt.Rows[i]["OPDNO"].ToString().Trim();
                            strTBPat += " 접 수 일:" + Dt.Rows[i]["ACTDATE"].ToString().Trim();
                            strTBPat += " 진 료 과:" + Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            strTBPat += " 결핵코드:" + Dt.Rows[i]["VCode"].ToString().Trim();
                        }
                    }
                }
            }

            Dt.Dispose();
            Dt = null;

            if (strTBPat != "")
                ComFunc.MsgBox(strTBPat, "확인");

            //보험내역 DISPLAY
            if (string.Compare(TRV.Bi, "11") <= 0)
            {
                FnBi = Convert.ToInt32(VB.Left(clsPmpaType.TBP.Bi, 1));
                cboBi.Text = clsPmpaType.TBP.Bi;
                txtBiName.Text = clsPmpaPb.GstrSetBis[Convert.ToInt32(clsPmpaType.TBP.Bi)];
            }
            else
            {
                FnBi = Convert.ToInt32(VB.Left(TRV.Bi, 1));
                cboBi.Text = TRV.Bi;
                txtBiName.Text = clsPmpaPb.GstrSetBis[Convert.ToInt32(TRV.Bi)];
            }

            ssBohum_Sheet1.Cells[0, 2].Text = clsPmpaType.TBP.Gwange;
            ssBohum_Sheet1.Cells[0, 3].Text = clsPmpaPb.GstrSetGwanges[Convert.ToInt32(VB.Val(clsPmpaType.TBP.Gwange))];
            ssBohum_Sheet1.Cells[1, 2].Text = clsPmpaType.TBP.PName;
            ssBohum_Sheet1.Cells[2, 2].Text = clsPmpaType.TBP.Kiho;
            ssBohum_Sheet1.Cells[5, 3].Text = "";
            ssBohum_Sheet1.Cells[6, 2].Text = "";

            switch (VB.Left(clsPmpaType.TBP.Bi, 1))
            {
                case "2":
                    ssBohum_Sheet1.Cells[2, 1].Text = "기관기호";
                    ssBohum_Sheet1.Cells[3, 1].Text = "관리번호";
                    ssBohum_Sheet1.Cells[3, 2].Text = clsPmpaType.TBP.GKiho;
                    ssBohum_Sheet1.Cells[4, 1].Text = "";
                    ssBohum_Sheet1.Cells[4, 2].Text = "";
                    ssBohum_Sheet1.Cells[4, 3].Text = "";
                    ssBohum_Sheet1.Cells[6, 2].Text = "0";

                    ssBohum_Sheet1.Cells[3, 2].Locked = false;
                    ssBohum_Sheet1.Cells[4, 2].Locked = false;
                    ssBohum_Sheet1.Cells[5, 2].Locked = true;
                    ssBohum_Sheet1.Cells[5, 3].Locked = true;
                    ssBohum_Sheet1.Cells[7, 2].Locked = false;

                    break;

                case "3":
                    ssBohum_Sheet1.Cells[2, 1].Text = "기관기호";
                    ssBohum_Sheet1.Cells[3, 1].Text = "사고발생일";
                    ssBohum_Sheet1.Cells[4, 1].Text = "진료시작일";
                    ssBohum_Sheet1.Cells[5, 1].Text = "진료종료일";
                    ssBohum_Sheet1.Cells[3, 2].Text = VB.Left(clsPmpaType.TBP.GKiho, 6);
                    ssBohum_Sheet1.Cells[4, 2].Text = VB.Left(clsPmpaType.TBP.GKiho, 6);
                    ssBohum_Sheet1.Cells[5, 2].Text = VB.Left(clsPmpaType.TBP.GKiho, 6);

                    ssBohum_Sheet1.Cells[3, 2].Locked = false;
                    ssBohum_Sheet1.Cells[4, 2].Locked = false;
                    ssBohum_Sheet1.Cells[5, 2].Locked = false;
                    ssBohum_Sheet1.Cells[6, 2].Locked = true;
                    ssBohum_Sheet1.Cells[7, 2].Locked = false;

                    break;

                case "5":
                    ssBohum_Sheet1.Cells[2, 1].Text = "계약처코드";
                    ssBohum_Sheet1.Cells[3, 1].Text = "차량번호";
                    ssBohum_Sheet1.Cells[4, 1].Text = "";
                    ssBohum_Sheet1.Cells[3, 2].Text = clsPmpaType.TBP.GKiho; 
                    ssBohum_Sheet1.Cells[4, 2].Text = "";
                    ssBohum_Sheet1.Cells[4, 3].Text = "";

                    ssBohum_Sheet1.Cells[3, 2].Locked = false;
                    ssBohum_Sheet1.Cells[4, 2].Locked = true;
                    ssBohum_Sheet1.Cells[5, 2].Locked = false;
                    ssBohum_Sheet1.Cells[6, 2].Locked = false;
                    ssBohum_Sheet1.Cells[7, 2].Locked = false;
                    ssBohum_Sheet1.Cells[5, 3].Locked = true;

                    break;

                default:
                    ssBohum_Sheet1.Cells[2, 1].Text = "기관기호";
                    ssBohum_Sheet1.Cells[3, 1].Text = "증번호";
                    ssBohum_Sheet1.Cells[4, 1].Text = "승인신청";
                    ssBohum_Sheet1.Cells[3, 2].Text = clsPmpaType.TBP.GKiho;
                    ssBohum_Sheet1.Cells[4, 2].Text = clsPmpaType.TBP.Remark;
                    ssBohum_Sheet1.Cells[5, 2].Text = "";
                    ssBohum_Sheet1.Cells[5, 3].Text = "";

                    ssBohum_Sheet1.Cells[3, 2].Locked = false;
                    ssBohum_Sheet1.Cells[4, 2].Locked = false;
                    ssBohum_Sheet1.Cells[5, 2].Locked = false;
                    ssBohum_Sheet1.Cells[6, 2].Locked = false;
                    ssBohum_Sheet1.Cells[7, 2].Locked = false;

                    break;
            }

            ssBohum_Sheet1.Cells[2, 3].Text = CF.Read_MiaName(pDbCon, clsPmpaType.TBP.Kiho, false);

            if (clsPmpaType.TBP.Bi.Equals("33"))
            {
                ssBohum_Sheet1.Cells[7, 2].Text = clsPmpaType.TBP.Kiho;
                ssBohum_Sheet1.Cells[7, 3].Text = CF.Read_MiaName(pDbCon, clsPmpaType.TBP.Kiho, false);
            }

            if (string.Compare(clsPmpaType.TBP.Sabun, "H001") >= 0 && string.Compare(clsPmpaType.TBP.Sabun, "H999") <= 0)
            {
                ssBohum_Sheet1.Cells[7, 2].Text = clsPmpaType.TBP.Sabun;
                ssBohum_Sheet1.Cells[7, 3].Text = CF.Read_MiaName(pDbCon, clsPmpaType.TBP.Sabun, false);
            }

            //예약내역 DISPLAY
            if (FstrFlagRV == "OK")
            {
                cboDept.Text = TRV.DeptCode;

                 for (int i = 0; i < cboDept.Items.Count; i++)
                 {
                     cboDept.SelectedIndex = i;
                     if (cboDept.Text.Trim() == TRV.DeptCode.Trim())
                     {
                         txtDeptName.Text = clsPmpaPb.GstrSetDepts[i];
                         break;
                     }
                     else
                         cboDept.SelectedIndex = 0;
                 }

                //txtDeptName.Text = CF.READ_DEPTNAMEK(clsDB.DbCon, cboDept.Text);
                cboDrCode.Text = TRV.DrCode.Trim();
                txtDrName.Text = CF.READ_DrName(pDbCon, TRV.DrCode.Trim());

                txtResDate.Text = VB.Left(TRV.Date3, 10);

                eCtl_LostFocus(this.txtResDate, null);

                cboResTime.Text = VB.Mid(TRV.Date3, 12, 5);
                txtResUpdate.Text = VB.Mid(TRV.Date3, 1, 10);
                txtResUptime.Text = VB.Mid(TRV.Date3, 12, 5);

                ssBohum_Sheet1.Cells[6, 2].Text = TRV.Bohun;
                ssBohum_Sheet1.Cells[6, 3].Text = clsPmpaPb.GstrSetJangaes[Convert.ToInt32(VB.Val(TRV.Bohun))];
                ssBohum_Sheet1.Cells[7, 2].Text = TRV.GelCode;

                if (string.Compare(TRV.GelCode,"") > 0)
                    ssBohum_Sheet1.Cells[7, 3].Text = CF.Read_MiaName(pDbCon, TRV.GelCode, false);

                txtResAmt.Text = "접수비 : " + TRV.Amt7;
            }

            FstrStart = "OK";
        }

        private void READ_OPDRESV_ALL(PsmhDb pDbCon)
        {
            DataTable DtT = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            CS.Spread_All_Clear(ssList);
            FstrFlagRV = "NO";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Sname, DeptCode, DrCode, ";
            SQL += ComNum.VBLF + "        TO_CHAR(Date3, 'YYYY-MM-DD') Day3,TO_CHAR(Date1, 'YYYY-MM-DD') Day1, ";
            SQL += ComNum.VBLF + "        TO_CHAR(Date3, 'HH24:Mi') Rtime ,GbSPC, Rowid ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW  ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND Pano  = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "    AND TRANSDATE IS NULL ";
            SQL += ComNum.VBLF + "    AND RETDATE IS NULL ";
            SQL += ComNum.VBLF + "  ORDER BY Date3, deptCode, DEPTCODE  ";
            SqlErr = clsDB.GetDataTable(ref DtT, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                DtT.Dispose();
                DtT = null;
                return;
            }

            if (DtT.Rows.Count > 0)
            {
                ssList_Sheet1.RowCount = DtT.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < DtT.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = DtT.Rows[i]["Sname"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 1].Text = DtT.Rows[i]["DeptCode"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = DtT.Rows[i]["DrCode"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = DtT.Rows[i]["Day3"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = DtT.Rows[i]["Rtime"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 5].Text = DtT.Rows[i]["Rowid"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 6].Text = DtT.Rows[i]["Day1"].ToString().Trim();

                }
            }

            if (DtT.Rows.Count > 0)
            {
                FstrFlagRV = "OK";
                pnlLeft.Enabled = false;
            }

            DtT.Dispose();
            DtT = null;
        }

        private void READ_TEWON_YEYAK(PsmhDb pDbCon)
        {
            DataTable DtT = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DeptCode, ";
            SQL += ComNum.VBLF + "        DrCode, ADMIN.FC_BAS_DOCTOR_DRNAME(DRCODE) DRNAME, ";
            SQL += ComNum.VBLF + "        TO_CHAR(BDate,'YYYY-MM-DD') BDate, ";
            SQL += ComNum.VBLF + "        TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') RDate, Pano, SName, ";
            SQL += ComNum.VBLF + "        GbSunap, ";
            SQL += ComNum.VBLF + "        EntSabun, ADMIN.FC_BAS_PASS_NAME(EntSabun) USERNAME, ";
            SQL += ComNum.VBLF + "        ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_RESERVED ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "    AND BDate     >= TRUNC(SYSDATE-1) ";
            SQL += ComNum.VBLF + "    AND GbSunap   = '0' ";
            SqlErr = clsDB.GetDataTable(ref DtT, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                DtT.Dispose();
                DtT = null;
                return;
            }

            if (DtT.Rows.Count == 0)
            {
                DtT.Dispose();
                DtT = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssWard_Sheet1.RowCount = DtT.Rows.Count;
            ssWard_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            clsPublic.GstrMsgTitle = "확인";
            clsPublic.GstrMsgList = "◆ 퇴원환자 예약 ◆" + '\r';

            for (int i = 0; i < DtT.Rows.Count; i++)
            {
                clsPublic.GstrMsgList += DtT.Rows[i]["DeptCode"].ToString().Trim() + " 과 " + '\r';
                clsPublic.GstrMsgList += DtT.Rows[i]["DRNAME"].ToString().Trim() + " 과장 " + '\r';
                clsPublic.GstrMsgList += DtT.Rows[i]["RDate"].ToString().Trim() + '\r';
                
                ssWard_Sheet1.Cells[i, 0].Text = DtT.Rows[i]["DeptCode"].ToString().Trim();
                ssWard_Sheet1.Cells[i, 1].Text = DtT.Rows[i]["DRNAME"].ToString().Trim();
                ssWard_Sheet1.Cells[i, 2].Text = DtT.Rows[i]["RDate"].ToString().Trim();
                ssWard_Sheet1.Cells[i, 3].Text = DtT.Rows[i]["Pano"].ToString().Trim();
                ssWard_Sheet1.Cells[i, 4].Text = DtT.Rows[i]["SName"].ToString().Trim();
                ssWard_Sheet1.Cells[i, 5].Text = DtT.Rows[i]["USERNAME"].ToString().Trim();
                ssWard_Sheet1.Cells[i, 6].Text = DtT.Rows[i]["DrCode"].ToString().Trim();
                ssWard_Sheet1.Cells[i, 7].Text = DtT.Rows[i]["ROWID"].ToString().Trim();
            }

            DtT.Dispose();
            DtT = null;

            clsPublic.GstrMsgList += "예약을 확인후 외래예약 처리하시기 바랍니다." + '\r';

            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            string strTemp = string.Empty;

            CF = new ComFunc();
            CQ = new ComQuery();
            CS = new clsSpread();
            CC = new Card();
            CPF = new clsPmpaFunc();
            CPQ = new clsPmpaQuery();
            CPO = new clsOumsad();
            COC = new ComPmpaLibB.clsOumsadChk();
            CPS = new ComPmpaLibB.clsPmpaSel();
            CPP = new ComPmpaLibB.clsPmpaPrint();
            OF = new clsOrdFunction();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            clsPmpaType.RAT = new clsPmpaType.Return_Amt_Table[20];

            eFrm_Clear();
            CC.CardVariable_Clear(ref RSD, ref RD);//카드변수 초기화
            //중증(암) visible = false
            lblCan.Visible = false;
            cboCan.Visible = false;
            txtCanName.Visible = false;
            //btnKiho.Visible = false;
            //의료급여(본인부담) visible = false
            lblMcode.Visible = false;
            cboMcode.Visible = false;
            txtMcodeName.Visible = false;

            cboResTime.Items.Clear();//예약시간
            txtResUptime.Text = "";;//변경시간

            pnlCount.Visible = false;

            //combobox 셋팅
            CF.Combo_BCode_SET(clsDB.DbCon,cboBi, "BAS_환자종류", true, 2, "N"); //환자종류
            cboBi.SelectedIndex = -1;

            CF.COMBO_DEPT_SET(clsDB.DbCon,cboDept, "N", "2");//진료과목
            cboDept.SelectedIndex = -1;

            CF.Combo_BCode_SET(clsDB.DbCon,cboCan, "BAS_중증암환자", true, 2, "");//중증(암)
            CF.Combo_BCode_SET(clsDB.DbCon,cboMcode, "BAS_의료급여본인부담", true, 2, "");//의료급여(본인부담)
            CF.Combo_BCode_SET(clsDB.DbCon,cboChojae, "BAS_초재진구분", true, 2, "");//초재진구분

            //이중감액 : 적용시 주석해제
            CF.COMBO_GAMEK_BCODE_SET(clsDB.DbCon, cboGamek, "BAS_감액코드명", "", true, 2, "N");//감액구분(자격)
            //CF.COMBO_GAMEK_BCODE_SET(clsDB.DbCon,cboGamek, "BAS_감액코드명", "J", true, 2, "N");//감액구분(자격)
            //CF.COMBO_GAMEK_BCODE_SET(clsDB.DbCon,cboGamekCase, "BAS_감액코드명", "C", true, 2, "N");//감액구분(CASE)
            CF.COMBO_OPDJIN_SET(clsDB.DbCon,cboJin, true, 2, "N");//진찰료수납(접수)구분

            clsPublic.GstrActDate = clsPublic.GstrSysDate;

            FstrFlagRV = "";
            FstrHjmUpFlag = "";
            FstrStart = "";
            FstrRowid_OPDResv = "";
            FstrRowid_OPDResvNew = "";
            FstrRowid_OCSResv = "";

            btnSave.Enabled = false;
            pnlUp.Enabled = false;
            ssPat.Enabled = false;
            ssBohum.Enabled = false;
            btnPrint.Enabled = false;

            clsPmpaPb.gnJinAMT1 = 0;//진찰료 발생금액
            clsPmpaPb.gnJinAMT2 = 0;//진찰료 특진료
            clsPmpaPb.gnJinAMT3 = 0;//진찰료 총액
            clsPmpaPb.gnJinAMT4 = 0;//진찰료 조합부담
            clsPmpaPb.gnJinAMT5 = 0;//진찰료 감액
            clsPmpaPb.gnJinAMT6 = 0;//진찰료 미수
            clsPmpaPb.gnJinAMT7 = 0;//진찰료 영수금액

            clsPmpaPb.GnGAmt1 = 0;//의료질평가지원금 발생금액
            clsPmpaPb.GnGAmt2 = 0;//교육수련지원금 발생금액
            clsPmpaPb.GnGAmt3 = 0;//교육수련지원금 발생금액

            txtPtno.Select();
        }

        private void eFrm_Clear()
        {

            FstrChk = "";
            clsPmpaPb.GstrLostFocus = "";
            FstrRowid_OPDResv = "";
            FstrRowid_OPDResvNew = "";
            FstrRowid_OCSResv = "";
            FstrGwaChoJae = "";

            txtPtno.Text = "";      txtSname.Text = "";
            cboBi.Text = "";        txtBiName.Text = "";
            cboDept.Text = "";      txtDeptName.Text = "";
            cboDrCode.Text = "";    txtDrName.Text = "";
            txtSpcBdate.Text = "";
            cboCan.Text = "";       txtCanName.Text = "";
            cboMcode.Text = "";     txtMcodeName.Text = "";
            cboChojae.Text = "";    txtChojaeName.Text = "";
            cboGamek.Text = "";     txtGamekName.Text = "";
            cboGamekCase.Text = ""; txtGamekCaseName.Text = "";
            cboJin.Text = "";       txtJinName.Text = "";

            txtResDate.Text = "";
            cboResTime.SelectedIndex = -1;//예약시간
            CS.Spread_Clear_Range(ssRes, 0, 1, ssRes_Sheet1.RowCount, ssRes_Sheet1.ColumnCount);

            txtResAmt.Text = "";
            txtPaWon.Text = "";
            txtChojae.Text = "";

            txtResUpdate.Text = "";//변경일자
            txtResUptime.Text = "";//변경시간
            CS.Spread_Clear_Range(ssUpRes, 0, 1, ssUpRes_Sheet1.RowCount, ssUpRes_Sheet1.ColumnCount);
            pnlUp.Enabled = false;//변경일자,변경시간 panel 

            txtHphone.Text = "";

            chkCard.Checked = false;
            chkCash.Checked = false;
            rdoSMS0.Checked = false;
            rdoSMS1.Checked = false;
            rdoSMS2.Checked = false;

            
            CC.CardVariable_Clear(ref RSD, ref RD);//카드변수 초기화
            chkCard.Checked = false;
            chkCard.Checked = false;

            rdoSMS0.Checked = false;
            rdoSMS1.Checked = false;
            rdoSMS2.Checked = false;

            

            TRV.Ptno = "";
            TRV.DeptCode = "";
            TRV.Bi = "";
            TRV.sName = "";
            TRV.DrCode = "";
            TRV.Date1 = "";
            TRV.Date2 = "";
            TRV.Date3 = "";
            TRV.GelCode = "";
            TRV.Chojae = "";
            TRV.GbGameK = "";
            TRV.GbSpc = "";
            TRV.Jin = "";
            TRV.Amt1 = 0;
            TRV.Amt2 = 0;
            TRV.Amt3 = 0;
            TRV.Amt4 = 0;
            TRV.Amt5 = 0;
            TRV.Amt6 = 0;
            TRV.Amt7 = 0;
            TRV.Part = "";
            TRV.Bohun = "";
            TRV.GelCode = "";
            TRV.CardSeqNo = 0;
            TRV.VCode = "";

            txtRemark.Text = "";//종합검진감액내역
            CS.Spread_Clear_Range(ssPat, 0, 2, ssPat_Sheet1.RowCount, ssPat_Sheet1.ColumnCount);//환자내역
            CS.Spread_Clear_Range(ssBohum, 0, 2, ssBohum_Sheet1.RowCount, ssBohum_Sheet1.ColumnCount);//보험내역
            CS.Spread_All_Clear(ssJin);//진료내역
            CS.Spread_All_Clear(ssWard);//병동예약내역

            sBarMsg1.Text = "";

            btnDelete.Enabled = false;
        }


        private void btnKiho_Click(object sender, EventArgs e)
        {

            if (txtPtno.Text.Trim() == "") { return; }
            if (cboDept.Text.Trim() == "") { return; }
            if (txtSname.Text.Trim() == "") { return; }

            //GM2_HIC(1) : 세대주성명      GM2_HIC(2) : 자격                GM2_HIC(3) : 자격취득일       
            //GM2_HIC(4) :보장기관기호     GM2_HIC(5) : 시설(증번호)        GM2_HIC(6) :급여제한일자    
            //GM2_HIC(9) : 장애인등록일자  GM2_HIC(10) :출국자여부          GM2_HIC(11) : 선택의료급여1  
            //GM2_HIC(12) :선택의료급여2   GM2_HIC(13) :선택의료급여3       GM2_HIC(14) :선택의료급여4
            //GM2_HIC(15): 수신자명        GM2_HIC(16): 의료급여산전지원금  GM2_HIC(17): 희귀V 
            //GM2_HIC(18):특정코드 전체    GM2_HIC(19): 주민번호            GM2_HIC(20): 노인틀니

            frmPmpaCheckNhic frm = new frmPmpaCheckNhic(txtPtno.Text.Trim(), cboDept.Text.Trim(), txtSname.Text.Trim(), clsPmpaType.TBP.Jumin1, clsPmpaType.TBP.Jumin2, clsPublic.GstrSysDate, "");
            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = new System.Drawing.Point(10, 10);
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);


            for (int i = 1; i < 23; i++)
                clsPmpaPb.Gm2_Hic[i] = VB.Pstr(clsPublic.GstrHelpCode, ";", i);

            //장애인이면 자동으로 F000 코드 물리게 작업
            if (clsPmpaPb.Gm2_Hic[9] == "Y" && cboBi.Text.Trim() != "21" && cboBi.Text.Trim() != "22")
            {
                if (VB.Left(clsPmpaPb.Gm2_Hic[2].Trim(), 1) != "7")
                    clsPmpaPb.Gm2_Hic[8] = "F000";
            }

            if (clsPmpaPb.Gm2_Hic[1] != "")
            {
                txtSname.Text = clsPmpaPb.Gm2_Hic[15];

                switch (VB.Left(clsPmpaPb.Gm2_Hic[2], 1))
                {
                    case "1"://지역가입자 및 지역세대원
                    case "2":

                        cboMcode.Text = clsPmpaPb.Gm2_Hic[8].Trim();
                        txtMcodeName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_의료급여본인부담", cboMcode.Text.Trim());

                        if (cboMcode.Text.Trim() == "H000" || cboMcode.Text.Trim() == "V000")
                        {
                            cboJin.Text = "F";
                            txtJinName.Text = CF.READ_JIN(clsDB.DbCon, cboJin.Text.Trim());
                        }

                        break;

                    case "4"://임의계속직장가입자,직장가입자.
                    case "5":
                    case "6":

                        cboMcode.Text = clsPmpaPb.Gm2_Hic[8].Trim();
                        txtMcodeName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_의료급여본인부담", cboMcode.Text.Trim());

                        //희귀.난치성질환자
                        if (cboMcode.Text.Trim() == "H000" || cboMcode.Text.Trim() == "V000")
                        {
                            if (clsPmpaPb.GnAge < 6)
                            {
                                clsPublic.GstrMsgTitle = "확인";
                                clsPublic.GstrMsgList = "상병특례 환자이므로 접수구분을 선택하시기 바랍니다." + '\r' + '\r';
                                clsPublic.GstrMsgList += "[예]     버튼 ▶ 접수구분 : S(소아 상병특례)"  + '\r';
                                clsPublic.GstrMsgList += "[아니오] 버튼 ▶ 접수구분 : T(소아 물리치료상병특례)" + '\r' + '\r';

                                clsPublic.DiResult = ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxDefaultButton.Button1);

                                if (clsPublic.DiResult == DialogResult.Yes)
                                    cboJin.Text = "S";
                                else
                                    cboJin.Text = "T";
                            }
                            else
                            {
                                clsPublic.GstrMsgTitle = "확인";
                                clsPublic.GstrMsgList = "상병특례 환자이므로 접수구분을 선택하시기 바랍니다." + '\r' + '\r';
                                clsPublic.GstrMsgList += "[예]     버튼 ▶ 접수구분 : F(상병특례)" + '\r';
                                clsPublic.GstrMsgList += "[아니오] 버튼 ▶ 접수구분 : G(물리치료상병특례)" + '\r' + '\r';

                                clsPublic.DiResult = ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxDefaultButton.Button1);

                                if (clsPublic.DiResult == DialogResult.Yes)
                                    cboJin.Text = "F";
                                else
                                    cboJin.Text = "G";
                            }

                            txtJinName.Text = CF.READ_JIN(clsDB.DbCon,cboJin.Text.Trim());
                        }

                        break;

                    case "7":

                        if (clsPmpaPb.Gm2_Hic[8] == "")
                        {
                            cboMcode.Text = "M000";
                            txtMcodeName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_의료급여본인부담", cboMcode.Text.Trim());
                        }
                        else if (clsPmpaPb.Gm2_Hic[8] == "M001" || clsPmpaPb.Gm2_Hic[8] == "M002")
                        {
                            cboMcode.Text = "M000";
                            if (clsPublic.GstrHosp[0] == "37100068")
                                cboMcode.Text = clsPmpaPb.Gm2_Hic[8];
                            else if (clsPublic.GstrHosp[1] == "37100068" )
                                cboMcode.Text = clsPmpaPb.Gm2_Hic[8];
                            else if (clsPublic.GstrHosp[2] == "37100068" )
                                cboMcode.Text = clsPmpaPb.Gm2_Hic[8];
                            else if (clsPublic.GstrHosp[3] == "37100068" )
                                cboMcode.Text = clsPmpaPb.Gm2_Hic[8];

                            if (cboMcode.Text.Trim() == "M000")
                            {
                                clsPublic.GstrMsgTitle = "확인";
                                clsPublic.GstrMsgList = "자격조회시 본인부담코드 ◆ M001 또는 M002 ◆ 발생함." + '\r' + '\r';
                                clsPublic.GstrMsgList += "선택의료기관에 포항성모병원이 없습니다." + '\r';
                                clsPublic.GstrMsgList += "승인요청시 ◆ B005 또는 B006 ◆ 를 확인하시고 승인요청 하시기 바랍니다." + '\r' + '\r';

                                ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                            }

                            txtMcodeName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_의료급여본인부담", cboMcode.Text.Trim());
                        }
                        else
                        {
                            cboMcode.Text = clsPmpaPb.Gm2_Hic[8];
                            txtMcodeName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_의료급여본인부담", cboMcode.Text.Trim());
                        }

                        break;

                    case "8"://의료급여2종
                        if (clsPmpaPb.Gm2_Hic[8] == "")
                            cboMcode.Text = "M000";
                        else
                            cboMcode.Text = clsPmpaPb.Gm2_Hic[8];

                        txtMcodeName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_의료급여본인부담", cboMcode.Text.Trim());

                        if (clsPmpaPb.Gm2_Hic[16] != "" && cboDept.Text.Trim() == "OG")
                        {
                            ComFunc.MsgBox("위 환자는 산전지원금 대상환자임(B099). 꼭 확인바랍니다.", "확인");
                            cboMcode.Text = "B099";
                            txtMcodeName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_의료급여본인부담", cboMcode.Text.Trim());
                        }
                        else if (clsPmpaPb.Gm2_Hic[16] != "" && cboDept.Text.Trim() != "OG")
                            ComFunc.MsgBox("해당 환자는 산전지원금이 있으나 진료과목이 산부인과가 아니므로 원래 자격별로 접수하시기 바랍니다.", "확인");


                        break;
                }

                if (cboMcode.Text.Trim() == "E000" || cboMcode.Text.Trim() == "F000")
                {
                    if ((VB.Left(clsPmpaPb.Gm2_Hic[8],1) != VB.Left(clsPmpaPb.Gm2_Hic[17], 1)) && VB.Left(clsPmpaPb.Gm2_Hic[17], 1).Equals("V"))
                    {
                        cboCan.Text = "EV00";
                        txtCanName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_중증암환자", cboCan.Text.Trim());

                        clsPublic.GstrMsgTitle = "확인";
                        clsPublic.GstrMsgList = "차상위2종 E000, F000 자격이며 등록희귀난치(V000) 자격입니다." + '\r';
                        clsPublic.GstrMsgList += "반드시 의료급여항목 [E000, F000] 중증암항목에 [EV00]를 확인 후 접수하시기 바랍니다." + '\r' + '\r';

                        ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                    }
                }
            }



            //2018.06.19 박병규 : 중증루틴 추가
            SQL = "";
            SQL += ComNum.VBLF + " SELECT VCODE, TO_CHAR(TDate,'YYYY-MM-DD') TDate, GUBUN ";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_CANCER                                             --중증환자마스터";
            SQL += ComNum.VBLF + " WHERE 1          = 1 ";
            SQL += ComNum.VBLF + "   AND PANO       = '" + txtPtno.Text + "' ";

            if (VB.Left(cboDept.Text, 1) == "M")
                SQL += ComNum.VBLF + "   AND (DEPT1 = '" + cboDept.Text.Trim() + "' OR DEPT2 = '" + cboDept.Text.Trim() + "' OR DEPT3 = '" + cboDept.Text.Trim() + "'  OR DEPT1 ='MD' ) ";
            else
                SQL += ComNum.VBLF + "   AND (DEPT1 = '" + cboDept.Text.Trim() + "' OR DEPT2 = '" + cboDept.Text.Trim() + "' OR DEPT3 = '" + cboDept.Text.Trim() + "') ";

            SQL += ComNum.VBLF + "   AND FDATE      <= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND TDATE      >= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND DELDATE IS NULL ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    if (Dt.Rows[i]["GUBUN"].ToString().Trim() == "1")
                    {
                        cboCan.Text = Dt.Rows[0]["VCODE"].ToString().Trim();
                        txtCanName.Text = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_중증암환자", cboCan.Text.Trim());
                        clsPmpaPb.GstrCanCer = cboCan.Text.Trim();
                        clsPublic.GstrMsgList = "해당 환자는 등록 암환자 자격이 만료됨. 반드시 확인후 접수요망.";
                    }
                    else if (Dt.Rows[i]["GUBUN"].ToString().Trim() == "3")
                    {
                        clsPublic.DiResult = ComFunc.MsgBoxQ("중증화상 등록환자임. 기본값을 자동으로 설정하시겠습니까?", "확인", MessageBoxDefaultButton.Button1);
                        if (clsPublic.DiResult == DialogResult.Yes)
                        {
                            cboCan.Text = Dt.Rows[0]["VCODE"].ToString().Trim();
                            txtCanName.Text = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_중증암환자", cboCan.Text.Trim());
                            clsPmpaPb.GstrCanCer = cboCan.Text.Trim();
                            clsPublic.GstrMsgList = "해당 환자는 등록 중증화상 자격이 만료됨. 반드시 확인후 접수요망.";
                        }
                    }
                }

                if (string.Compare(clsPublic.GstrSysDate, Dt.Rows[0]["TDate"].ToString().Trim()) > 0)
                    ComFunc.MsgBox(clsPublic.GstrMsgList, "확인");
            }

            Dt.Dispose();
            Dt = null;


        }

        private void btnSms_Click(object sender, EventArgs e)
        {
            clsPublic.GstrChoicePano = txtPtno.Text;

            frmSMSApprove frm = new frmSMSApprove();
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);


            txtHphone.Text = CF.Read_Patient(clsDB.DbCon,txtPtno.Text, "3");//휴대폰
            clsPmpaType.TBP.GbSMS = CF.Read_Patient(clsDB.DbCon,txtPtno.Text, "4");//동의여부
            btnSms.Text = "승인";

            if (clsPmpaType.TBP.GbSMS.Equals("Y"))
                rdoSMS0.Checked = true;
            else if (clsPmpaType.TBP.GbSMS.Equals("N"))
                rdoSMS1.Checked = true;
            else if (clsPmpaType.TBP.GbSMS.Equals("X"))
                rdoSMS2.Checked = true;
            else
                rdoSMS1.Checked = true;
        }


        private void PRINT_PROCESS(PsmhDb pDbCon)
        {
            string strSpc = string.Empty;
            string strSndPtno = string.Empty;
            string strSndBi = string.Empty;
            string strSndSname = string.Empty;
            string strSndDept = string.Empty;
            string strSndDr = string.Empty;
            string strSndResv = string.Empty;

            strSpc = "0";
            strSndPtno = txtPtno.Text.Trim();
            strSndDept = CF.READ_DEPTNAMEK(pDbCon, cboDept.Text.Trim());
            strSndDr = cboDrCode.Text.Trim();
            strSndResv = txtResDate.Text;
            strSndResv += " " + cboResTime.Text.Trim();

            strSndSname = ssPat_Sheet1.Cells[0, 2].Text.Trim();
            strSndBi = cboBi.Text.Trim();

            clsPmpaType.TOM.DeptCode = cboDept.Text.Trim();
            clsPmpaType.TOM.DrCode = cboDrCode.Text.Trim();

            if (VB.Left(clsPmpaPb.GstrPrtBun, 1) == "2")
                CPP.Report_Print_Jupsu_A4_New(pDbCon, 0, strSndPtno, strSndSname, strSndDept, strSndResv, strSndDr, strSndBi, "", "", "1", "예약비", cboDept.Text.Trim(), pic_Sign, false, "0", "", "", "", ssResJupsuPrint);
            else if (VB.Left(clsPmpaPb.GstrPrtBun, 1) == "1")
                CPP.Report_Print_Jupsu_A4_New(pDbCon, 0, strSndPtno, strSndSname, strSndDept, strSndResv, strSndDr, strSndBi, "", "", "1", "예약비", cboDept.Text.Trim(), pic_Sign, false, "0", "", "", "", ssResJupsuPrint);
            else if (VB.Left(clsPmpaPb.GstrPrtBun, 1) == "1" && VB.Left(clsPmpaPb.GstrPrtBun2, 1) == "E")
                CPP.Report_Print_Jupsu_A4_New(pDbCon, 0, strSndPtno, strSndSname, strSndDept, strSndResv, strSndDr, strSndBi, "", "", "1", "예약비", cboDept.Text.Trim(), pic_Sign, false, "0", "", "", "", ssResJupsuPrint);
            else if (VB.Left(clsPmpaPb.GstrPrtBun, 1) == "5")
                CPP.Report_Print_Jupsu_A4_New(pDbCon, 0, strSndPtno, strSndSname, strSndDept, strSndResv, strSndDr, strSndBi, "", "", "1", "예약비", cboDept.Text.Trim(), pic_Sign, false, "0", "", "", "", ssResJupsuPrint);
            else
                CPP.Report_Print_Jupsu_A4_New(pDbCon, 0, strSndPtno, strSndSname, strSndDept, strSndResv, strSndDr, strSndBi, "", "", "1", "예약비", cboDept.Text.Trim(), pic_Sign, false, "0", "", "", "", ssResJupsuPrint);
        }

        private void DATA_MOVE(PsmhDb pDbCon)
        {
            FstrInsSname = ssPat_Sheet1.Cells[0, 2].Text.Trim();//수진자명
            FstrInsJumin1 = ssPat_Sheet1.Cells[1, 2].Text.Trim();//주민번호1
            FstrInsJumin2 = ssPat_Sheet1.Cells[1, 3].Text.Trim();//주민번호2
            FstrInsTel = ssPat_Sheet1.Cells[2, 2].Text.Trim();//전화번호

            FstrInsChoJae = cboChojae.Text.Trim();//초재진구분
            FstrInsGamek = cboGamek.Text.Trim();//감액구분(자격)
            FstrInsGamekC = cboGamekCase.Text.Trim();//감액구분(CASE)
            FstrInsJin = cboJin.Text.Trim();//접수구분

            FstrInsBi = cboBi.Text.Trim(); //환자구분
            FstrInsGwange = ssBohum_Sheet1.Cells[0, 2].Text.Trim();//피보관계
            FstrInsPname = ssBohum_Sheet1.Cells[1, 2].Text.Trim();//피보성명
            FstrInsKiho = ssBohum_Sheet1.Cells[2, 2].Text.Trim();//기관기호
            FstrInsGkiho = ssBohum_Sheet1.Cells[3, 2].Text.Trim();//증번호

            if (VB.Mid(FstrInsBi,1,1).Equals("3"))
            {
                FstrInsGkiho = ssBohum_Sheet1.Cells[3, 2].Text.Trim();//증번호
                FstrInsGkiho += ssBohum_Sheet1.Cells[4, 2].Text.Trim();//승인신청
                FstrInsGkiho += ssBohum_Sheet1.Cells[5, 2].Text.Trim();
            }
            else
            {
                FstrInsRemark = ssBohum_Sheet1.Cells[4, 2].Text.Trim();//승인신청
                FstrInsBohun = ssBohum_Sheet1.Cells[6, 2].Text.Trim();//보훈,장애
                FstrInsGelCode = ssBohum_Sheet1.Cells[7, 2].Text.Trim();//계약처
            }

            FstrInsSpc = "0";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NAME, GUBUN2 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1";
            SQL += ComNum.VBLF + "    AND GUBUN = 'BAS_환자종류' ";
            SQL += ComNum.VBLF + "    AND CODE  = '" + FstrInsBi + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
                FstrDataCheck = "OK";
            else
                FstrDataCheck = "NO";

            Dt.Dispose();
            Dt = null;
        }

        private string CHECK_OPD_RESV(PsmhDb pDbCon, string ArgDate)
        {
            DataTable DtFunc = new DataTable();
            string rtnval = "NO";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + cboDept.Text + "' ";
            SQL += ComNum.VBLF + "    AND Date3     >= TO_DATE('" + ArgDate + "','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "    AND Date3     <  TO_DATE('" + VB.DateAdd("D", 1, ArgDate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "    AND TRANSDATE IS NULL ";
            SQL += ComNum.VBLF + "    AND RETDATE IS NULL ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnval;
            }

            if (DtFunc.Rows.Count > 0)
                rtnval = "OK";

            DtFunc.Dispose();
            DtFunc = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_TelResv ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + cboDept.Text + "' ";
            SQL += ComNum.VBLF + "    AND RDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD')  ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnval;
            }

            if (DtFunc.Rows.Count > 0)
                rtnval = "OK2";

            DtFunc.Dispose();
            DtFunc = null;

            return rtnval;
        }

        private string CHECK_SANJE_TA(PsmhDb pDbCon, string ArgDate)
        {
            DataTable DtFunc = new DataTable();
            string rtnval = "OK";
            string strBiMsg = string.Empty;
            string strEndDate = string.Empty;
            string strGwa1 = string.Empty;
            string strGwa2 = string.Empty;
            string strGwa3 = string.Empty;

            strBiMsg = "자보";
            if (clsPmpaType.TBP.Bi.Equals("31")) { strBiMsg = "산재"; }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Pano, Dept1, Dept2, ";
            SQL += ComNum.VBLF + "        Dept3, TO_CHAR(Date3,'YYYY-MM-DD') EndDate ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SANID ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND Pano  = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "    AND Bi    = '" + clsPmpaType.TBP.Bi + "' ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnval;
            }

            if (DtFunc.Rows.Count > 0)
            {
                strEndDate = DtFunc.Rows[0]["ENDDATE"].ToString().Trim();
                strGwa1 = DtFunc.Rows[0]["Dept1"].ToString().Trim();
                strGwa2 = DtFunc.Rows[0]["Dept2"].ToString().Trim();
                strGwa3 = DtFunc.Rows[0]["Dept3"].ToString().Trim();

                if (strEndDate == "")
                {
                    if (cboDept.Text.Equals(strGwa1) || cboDept.Text.Equals(strGwa2) || cboDept.Text.Equals(strGwa3))
                        return rtnval;
                    else if (cboDept.Text.Trim().Equals("ER"))
                        return rtnval;
                    else
                    {
                        clsPublic.GstrMsgTitle = "알림";
                        clsPublic.GstrMsgList = "환자의 진료과가 등록되지 않았습니다." + '\r';
                        clsPublic.GstrMsgList += "진료과를 확인하시기 바랍니다." + '\r' + '\r';

                        ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                        rtnval = "NO";

                        DtFunc.Dispose();
                        DtFunc = null;
                        return rtnval;
                    }
                }
                else if (strEndDate != "" && string.Compare(ArgDate, strEndDate) > 0)
                {
                    clsPublic.GstrMsgTitle = "알림";
                    clsPublic.GstrMsgList = "환자의 치료가 종결되었습니다. <담당자 확인요망>" + '\r';
                    clsPublic.GstrMsgList += "환자구분을 변경 입력하시기 바랍니다." + '\r' ;
                    clsPublic.GstrMsgList += "강제 변경시 [예] 버튼을 클릭하시기 바랍니다." + '\r' + '\r';

                    clsPublic.DiResult = ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxDefaultButton.Button1);
                    if (clsPublic.DiResult == DialogResult.No)
                        rtnval = "NO";
                }
                else if (strEndDate != "" && string.Compare(ArgDate, strEndDate) <= 0)
                {
                    if (cboDept.Text.Equals(strGwa1) || cboDept.Text.Equals(strGwa2) || cboDept.Text.Equals(strGwa3))
                    {
                        DtFunc.Dispose();
                        DtFunc = null;
                        return rtnval;
                    }
                    else if (cboDept.Text.Trim().Equals("ER"))
                    {
                        DtFunc.Dispose();
                        DtFunc = null;
                        return rtnval;
                    }
                    else
                    {
                        clsPublic.GstrMsgTitle = "알림";
                        clsPublic.GstrMsgList = "환자의 진료과가 등록되지 않았습니다. <담당자 확인요망>" + '\r';
                        clsPublic.GstrMsgList += "진료과를 확인하시기 바랍니다." + '\r' + '\r';
                        clsPublic.GstrMsgList += "강제 변경시 [예] 버튼을 클릭하시기 바랍니다." + '\r' + '\r';

                        clsPublic.DiResult = ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxDefaultButton.Button1);
                        if (clsPublic.DiResult == DialogResult.No)
                            rtnval = "NO";

                        DtFunc.Dispose();
                        DtFunc = null;
                        return rtnval;
                    }
                }
            }
            else
            {
                clsPublic.GstrMsgTitle = "알림";
                clsPublic.GstrMsgList = "환자 등록이 없습니다. <담당자 확인요망>" + '\r';
                clsPublic.GstrMsgList += strBiMsg + " 환자인지 확인하시고, 환자구분을 변경하여 입력하시기 바랍니다." + '\r' + '\r';

                ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                rtnval = "NO";
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnval;
        }
        
        private void RESV_DELETE(PsmhDb pDbCon)
        {
             
            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL += ComNum.VBLF + "    SET RETDATE   = TRUNC(SYSDATE) , ";
                SQL += ComNum.VBLF + "        RETAMT    = AMT7 * -1, ";
                SQL += ComNum.VBLF + "        REMARK    = '" + FstrRemark + "',  ";

                if (clsPmpaType.RSD.CardSeqNo > 0)
                    SQL += ComNum.VBLF + "    RETCARDSEQNO = " + clsPmpaType.RSD.CardSeqNo + ", ";

                SQL += ComNum.VBLF + "        RETPART   = '" + clsType.User.IdNumber + "' ";
                SQL += ComNum.VBLF + "  WHERE ROWID     = '" + FstrRowid_OPDResvNew + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return;
                }

                clsDB.setCommitTran(pDbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return;
            }

        }



        private void PRINT_PROCESS_RETURN(PsmhDb pDbCon)
        {
            string strSndPtno = string.Empty;
            string strSndName = string.Empty;
            string strSndDept = string.Empty;
            string strSndResv = string.Empty;
            string strSndDr   = string.Empty;
            string strSndBi = string.Empty;

            clsPmpaPb.gnJinAMT1 = TRV.Amt1 * -1;
            clsPmpaPb.gnJinAMT2 = TRV.Amt2 * -1;
            clsPmpaPb.gnJinAMT3 = TRV.Amt3 * -1;
            clsPmpaPb.gnJinAMT4 = TRV.Amt4 * -1;
            clsPmpaPb.gnJinAMT5 = TRV.Amt5 * -1;
            clsPmpaPb.gnJinAMT6 = TRV.Amt6 * -1;
            clsPmpaPb.gnJinAMT7 = TRV.Amt7 * -1;


            strSndPtno = txtPtno.Text.Trim();
            strSndDept = cboDept.Text.Trim();
            strSndDr = cboDrCode.Text.Trim();;
            strSndResv = txtResDate.Text + cboResTime.Text.Trim();
            strSndName = ssPat_Sheet1.Cells[0, 2].Text.Trim();
            strSndBi = cboBi.Text.Trim();

            clsPmpaType.RAT[15].Amt1 = clsPmpaPb.gnJinAMT7;

            clsPmpaPb.GstrJeaSunap = "YES";

            CPP.Report_Print_Jupsu_Sign_New(pDbCon, 0, strSndPtno, strSndName, strSndDept, strSndResv, strSndDr, strSndBi, "", "", "1", "예약취소", cboDept.Text.Trim(), pic_Sign, false, "", "", "", ssResJupsuPrint);
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (chkCard.Checked == true)
                chkCard.Checked = false;

            if (txtPtno.Text.Trim() == "" && cboDept.Text.Trim() == "")
                this.Close();
            else
            {
                clsPublic.GstrMsgTitle = "확인";
                clsPublic.GstrMsgList = "화면의 데이터를 컴퓨터에 수록하지 않습니다." + '\r';
                clsPublic.GstrMsgList += "취소 하시겠습니까?" + '\r' + '\r';

                clsPublic.DiResult = ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxDefaultButton.Button1);

                if (clsPublic.DiResult == DialogResult.Yes)
                {
                    eFrm_Clear();

                    //2018.06.28 박병규 : 박예지요청(CLEAR만 되도록)
                    //this.Close();
                }
            }

            clsPublic.GFmResvValue = "";
            btnSave.Enabled = true;

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsPmpaPb.GstrJeaSunap = "NO";
            PRINT_PROCESS(clsDB.DbCon);
            this.Close();
        }

        private void ssPat_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strData = string.Empty;

            if (FstrStart.Equals("OK"))
            {
                strData = ssPat_Sheet1.Cells[e.Row, e.Column].Text.Trim();

                if (strData != "")
                {
                    if (e.Row.Equals(0) && e.Column.Equals(2))
                        FstrSname = strData;
                }

                FstrHjmUpFlag = "OK";
            }
        }


        private void ssPat_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            string strData = string.Empty;

            strData = ssPat_Sheet1.Cells[e.Row, e.Column].Text.Trim();

            if (e.Column.Equals(2))
            {
                if (e.Row.Equals(0))//CHECK SNAME
                {
                    if (strData == "") { ssPat_Sheet1.Cells[0, 2].Text = FstrOldData; }
                }
                else if (e.Row.Equals(1))//CHECK JUMIN1
                {
                    strData = VB.Left(strData + "000000", 6);
                    ssPat_Sheet1.Cells[e.Row, e.Column].Text = strData;

                    clsPmpaPb.GstrJumin1 = string.Format("{0:000000}", ssPat_Sheet1.Cells[1, 2].Text);
                    clsPmpaPb.GstrJumin2 = string.Format("{0:0000000}", ssPat_Sheet1.Cells[1, 3].Text);
                    clsPmpaPb.GnAge = ComFunc.AgeCalcEx(clsPmpaPb.GstrJumin1 + clsPmpaPb.GstrJumin2, clsPublic.GstrSysDate);

                    ssPat_Sheet1.Cells[0, 3].Text = clsPmpaPb.GstrSex + "/" + clsPmpaPb.GnAge;
                }
                else if (e.Row.Equals(2))//CHECK TEL
                {
                    if (strData == "") { ssPat_Sheet1.Cells[2, 2].Text = FstrOldData; }
                }
            }

            if (e.Row.Equals(1) && e.Column.Equals(3))
            {
                clsPmpaPb.GstrSex = ComFunc.SexCheck(ssPat_Sheet1.Cells[1, 2].Text + strData, "2");

                clsPmpaPb.GstrJumin1 = string.Format("{0:000000}", ssPat_Sheet1.Cells[1, 2].Text);
                clsPmpaPb.GstrJumin2 = string.Format("{0:0000000}", ssPat_Sheet1.Cells[1, 3].Text);
                clsPmpaPb.GnAge = ComFunc.AgeCalcEx(clsPmpaPb.GstrJumin1 + clsPmpaPb.GstrJumin2, clsPublic.GstrSysDate);

                ssPat_Sheet1.Cells[0, 3].Text = clsPmpaPb.GstrSex + "/" + clsPmpaPb.GnAge;
            }
        }

        private void ssBohum_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strData = string.Empty;

            if (FstrStart.Equals("OK"))
            {
                strData = ssBohum_Sheet1.Cells[e.Row, e.Column].Text.Trim();
                FstrHjmUpFlag = "OK";

                if (e.Column.Equals(2))
                {
                    switch (e.Row)
                    {
                        case 0:
                            if (strData != "")
                                ssBohum_Sheet1.Cells[0, 3].Text = clsPmpaPb.GstrSetGwanges[Convert.ToInt32(strData)];

                            break;

                        case 6:
                            if (strData != "")
                                ssBohum_Sheet1.Cells[6, 3].Text = clsPmpaPb.GstrSetJangaes[Convert.ToInt32(strData)];

                            clsPmpaType.TBP.Bohun = ssBohum_Sheet1.Cells[6, 2].Text.Trim(); 

                            break;
                    }

                    if (e.Row.Equals(0) || e.Row.Equals(7))
                    {
                        FnChoJae = Convert.ToInt32(cboChojae.Text);
                        FstrGamek = cboGamek.Text.Trim();
                        FstrGamekCase = cboGamekCase.Text.Trim();
                        FstrJin = cboJin.Text.Trim();

                        FstrBoil = cboBi.Text.Trim();
                        FstrJangae = ssBohum_Sheet1.Cells[6, 2].Text.Trim();
                        FstrInsGelCode = ssBohum_Sheet1.Cells[7, 2].Text.Trim();

                        FnSpc = 0;

                        //산재공상환자 접수비 미수처리
                        clsPmpaPb.GnGyeJin = 0;
                        if (FstrBoil.Equals("33"))
                        {
                            if (string.Compare(FstrInsGelCode, "H001") >= 0 && string.Compare(FstrInsGelCode, "H999") <= 0 && FstrJin != "2")
                                clsPmpaPb.GnGyeJin = 1;
                        }
                        //남부경찰서 43종환자 접수비 미수처리
                        if (FstrInsGelCode.Trim().Equals("H023") || FstrInsGelCode.Trim().Equals("H027"))
                            if (FstrJin != "2") { clsPmpaPb.GnGyeJin = 1; }

                        CPO.Jin_Amt_Account(clsDB.DbCon, txtPtno.Text, FstrJin, FnChoJae, FnSpc, FstrGamek, FstrGamekCase, FstrBoil, FstrJangae, cboDept.Text.Trim(), cboDrCode.Text.Trim(), txtResDate.Text.Trim() == "" ? clsPublic.GstrSysDate : txtResDate.Text, clsPmpaPb.GnGyeJin, "", "", "1", "", "00");

                        txtResAmt.Text = "접수비 : " + clsPmpaPb.gnJinAMT7;
                    }
                }
            }
        }


        private void ssBohum_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            string strData = string.Empty;
            string strBi = string.Empty;

            strData = ssBohum_Sheet1.Cells[e.Row, e.Column].Text.Trim();
            
            if (e.Column.Equals(2))
            {
                if (e.Row.Equals(0))//CHECK GWANGE
                {
                    if (strData.Equals("0")) { ssBohum_Sheet1.Cells[1, 2].Text = FstrSname; }
                }
                else if (e.Row.Equals(1))//CHECK PNAME
                {
                    if (strData.Equals("")) { ssBohum_Sheet1.Cells[e.Row, e.Column].Text = FstrOldData; }
                }
                else if (e.Row.Equals(2))//CHECK KIHO
                {
                    strBi = cboBi.Text.Trim();

                    if (strData.Equals("") && strBi != "51" && strBi != "55")
                        ssBohum_Sheet1.Cells[e.Row, e.Column].Text = FstrOldData;

                    if (strData != FstrOldData && strBi != "51" && strBi != "55")
                    {
                        CPF.GET_BAS_MIA(clsDB.DbCon, strData);

                        if (clsPmpaPb.GstrMiaFlag != "OK")
                            ssBohum_Sheet1.Cells[e.Row, 2].Text = FstrOldData;
                        else if (clsPmpaPb.GstrReturnMiaClass.Equals("20") && string.Compare(strBi,"21")< 0 && string.Compare(strBi, "25") > 0)
                            ssBohum_Sheet1.Cells[e.Row, 2].Text = FstrOldData;
                        else if (clsPmpaPb.GstrReturnMiaClass.Equals("90") && string.Compare(strBi, "31") < 0)
                            ssBohum_Sheet1.Cells[e.Row, 2].Text = FstrOldData;
                        else if (string.Compare(clsPmpaPb.GstrReturnMiaClass, "20") < 0 && strBi != clsPmpaPb.GstrReturnMiaClass)
                        {
                            ssBohum_Sheet1.Cells[0, 2].Text = clsPmpaPb.GstrReturnMiaClass;
                            ssBohum_Sheet1.Cells[e.Row, 3].Text = clsPmpaPb.GstrReturnMiaName;
                        }
                    }
                }
                else if (e.Row.Equals(7))//CHECK GELCODE
                {
                    strBi = cboBi.Text.Trim();
                    ssBohum_Sheet1.Cells[e.Row, e.Column].Text = ssBohum_Sheet1.Cells[e.Row, e.Column].Text.ToUpper();
                    strData = strData.ToUpper();

                    ssBohum_Sheet1.Cells[e.Row, 3].Text = "";

                    if (string.Compare(strData, "") > 0)
                    {
                        CPF.GET_BAS_MIA(clsDB.DbCon, strData);

                        if (clsPmpaPb.GstrMiaFlag != "OK")
                        {
                            ssBohum_Sheet1.Cells[e.Row, 2].Text = "";
                            clsPmpaPb.GstrReturnMiaName = "";
                        }
                        else if (strBi.Equals("31") || strBi.Equals("32") || strBi.Equals("52"))
                        {
                            ssBohum_Sheet1.Cells[e.Row, 2].Text = "";
                            clsPmpaPb.GstrReturnMiaName = "";
                        }
                        else if (clsPmpaPb.GstrMiaDetail != "99")
                        {
                            ssBohum_Sheet1.Cells[e.Row, 2].Text = "";
                            clsPmpaPb.GstrReturnMiaName = "";
                        }

                        FstrInsGelCode = ssBohum_Sheet1.Cells[e.Row, 2].Text.Trim();
                        ssBohum_Sheet1.Cells[e.Row, 3].Text = clsPmpaPb.GstrReturnMiaName;
                    }

                    FnChoJae = Convert.ToInt32(cboChojae.Text);
                    FstrJin = cboJin.Text;
                    FstrGamek = cboGamek.Text.Trim();
                    FstrGamekCase = cboGamekCase.Text.Trim();

                    FstrBoil = cboBi.Text.Trim();
                    FstrJangae = ssBohum_Sheet1.Cells[6, 2].Text.Trim();
                    FstrInsGelCode = ssBohum_Sheet1.Cells[7, 2].Text.Trim();

                    FnSpc = 0;

                    //산재공상환자 접수비 미수처리
                    clsPmpaPb.GnGyeJin = 0;
                    if (FstrBoil.Equals("33"))
                    {
                        if (string.Compare(FstrInsGelCode, "H001") >= 0 && string.Compare(FstrInsGelCode, "H999") <= 0 && FstrJin != "2")
                            clsPmpaPb.GnGyeJin = 1;
                    }
                    //남부경찰서 43종환자 접수비 미수처리
                    if (FstrInsGelCode.Trim().Equals("H023") || FstrInsGelCode.Trim().Equals("H027"))
                        if (FstrJin != "2") { clsPmpaPb.GnGyeJin = 1; }

                    CPO.Jin_Amt_Account(clsDB.DbCon, txtPtno.Text, FstrJin.ToString(), FnChoJae, FnSpc, FstrGamek, FstrGamekCase, FstrBoil, FstrJangae, cboDept.Text.Trim(), cboDrCode.Text.Trim(), txtResDate.Text.Trim() == "" ? clsPublic.GstrSysDate : txtResDate.Text, clsPmpaPb.GnGyeJin, "", "", "1", "", "00");

                    txtResAmt.Text = "접수비 : " + clsPmpaPb.gnJinAMT7;
                }
            }
            ssBohum_Sheet1.OperationMode = clsSpread.NORMAL;
        }

        private void ssWard_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strDeptCode = string.Empty;
            string strDrCode = string.Empty;
            string strRtime = string.Empty;

            FstrRowid_OCSResv = "";

            ssPat.Enabled = true;
            ssBohum.Enabled = true;

            FstrFlagRV = "NO";
            pnlLeft.Enabled = true;
            btnSave.Enabled = true;
            chkCard.Enabled = true;
            btnCancel.Enabled = true;

            DISPLAY_SCREEN(clsDB.DbCon);

            if (FstrTchk.Equals("Y"))
            {
                ComFunc.MsgBox("선택한 예약이 이미 존재합니다." + "\r" + "퇴원예약을 하실려면 창을 닫고 다시 하시기 바랍니다.", "확인");
                return;
            }

            if (e.Row >= 0 && e.Column >= 0)
            {
                strDeptCode = ssWard_Sheet1.Cells[e.Row, 0].Text.Trim();
                strDrCode = ssWard_Sheet1.Cells[e.Row, 6].Text.Trim();
                strRtime = ssWard_Sheet1.Cells[e.Row, 2].Text.Trim();

                FstrRowid_OCSResv = ssWard_Sheet1.Cells[e.Row, 7].Text.Trim();

                cboDept.Text = strDeptCode;
                txtDeptName.Text = CF.READ_DEPTNAMEK(clsDB.DbCon,strDeptCode);

                txtResDate.Text = VB.Left(strRtime, 10);
                cboResTime.Text = VB.Right(strRtime, 5);

                cboDrCode.Text = strDrCode;
                txtDrName.Text = CF.READ_DrName(clsDB.DbCon, strDrCode);

                cboDrCode.Enabled = true;
                cboChojae.Select();
            }
        }

        private void btnSelNO_Click(object sender, EventArgs e)
        {
            pnlCount.Visible = false;
            ssPat.Enabled = true;
            ssBohum.Enabled = true;

            FstrFlagRV = "NO";
            pnlLeft.Enabled = true;
            btnSave.Enabled = true;
            chkCard.Enabled = true;
            btnCancel.Enabled = true;

            DISPLAY_SCREEN(clsDB.DbCon);

            cboDept.Focus();
        }

        private void btnSelOK_Click(object sender, EventArgs e)
        {
            string strSendDept = string.Empty;
            string strSendDate3 = string.Empty;
            string strGetFlag = string.Empty;

            FstrRowid_OCSResv = "";
            FstrTchk = "Y";

            btnPrint.Enabled = true;

            if (FnRow > ssList_Sheet1.RowCount - 1)
            {
                ComFunc.MsgBox("환자가 선택되지 않았습니다.");
                return;
            }

            if (ssList_Sheet1.Cells[FnRow, 0].Text == "")
            {
                ComFunc.MsgBox("환자가 선택되지 않았습니다.");
                return;
            }



            strSendDept = ssList_Sheet1.Cells[FnRow, 1].Text.Trim();
            strSendDate3 = ssList_Sheet1.Cells[FnRow, 3].Text.Trim();
            FstrRowid_OPDResvNew = ssList_Sheet1.Cells[FnRow, 5].Text.Trim();

            strGetFlag = READ_OPD_RESV(FstrRowid_OPDResvNew);
            txtSname.Text = TRV.sName;

            if (strGetFlag.Equals("OK"))
            {
                pnlCount.Visible = false;
                ssPat.Enabled = true;
                ssBohum.Enabled = true;

                sBarMsg1.Text = "해당환자 이미 예약되어 있습니다.";
                pnlLeft.Enabled = true;
                pnlUp.Enabled = true;
                btnDelete.Enabled = true;
                btnSelOK.Enabled = true;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                chkCard.Enabled = true;
                clsPmpaPb.GstrLostFocus = "**";
                DISPLAY_SCREEN(clsDB.DbCon);
                txtResUpdate.Focus();

                if (TRV.CardSeqNo > 0)
                    sBarMsg1.Text = "카드 수납하셨습니다.";
                else
                    sBarMsg1.Text = "";

                chkNhic.Checked = false;
                CHECK_NHIC("2");
            }
        }

        /// <summary>
        /// 자격점검
        /// </summary>
        /// <param name="ArgGubun">
        /// 1.자격포함 2.지격미포함
        /// </param>
        /// <seealso cref="READ_CGK_자격"/>
        private void CHECK_NHIC(string ArgGubun)
        {
            txtCanName.Text = "";
            txtMcodeName.Text = "";
            txtJinName.Text = "";

            if (ArgGubun.Equals("1")) //자격포함
            {
                btnKiho.Visible = true;

                lblCan.Visible = true;
                cboCan.Visible = true;
                txtCanName.Visible = true;

                lblMcode.Visible = true;
                cboMcode.Visible = true;
                txtMcodeName.Visible = true;
            }
            else //자격미포함
            {
                btnKiho.Visible = false;

                lblCan.Visible = false;
                cboCan.Visible = false;
                txtCanName.Visible = false;

                lblMcode.Visible = false;
                cboMcode.Visible = false;
                txtMcodeName.Visible = false;
            }
        }

        private string READ_OPD_RESV(string ArgRowid)
        {
            string rtnVal = string.Empty;

            rtnVal = "NO";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Pano, DeptCode, Bi, ";
            SQL += ComNum.VBLF + "        Sname, DrCode,";
            SQL += ComNum.VBLF + "        TO_CHAR(Date1, 'YYYY-MM-DD') DateJupsu,           --접수일자";
            SQL += ComNum.VBLF + "        TO_CHAR(Date2, 'YYYY-MM-DD HH24:MI') DateYeyak,   --예약일자+예약시간";
            SQL += ComNum.VBLF + "        TO_CHAR(Date3, 'YYYY-MM-DD HH24:MI') DateRetry,   --변경일자+변경시간";
            SQL += ComNum.VBLF + "        TO_CHAR(Date3, 'YYYY-MM-DD') DateUpdate,          --변경일자";
            SQL += ComNum.VBLF + "        Chojae, GbGamek, GbGamekC,                        --초재구분,감액(자격),감액(CASE)";
            SQL += ComNum.VBLF + "        GbSpc, Jin, AMT1,                                 --선택진료,접수구분,진찰료발생금액";
            SQL += ComNum.VBLF + "        AMT2, AMT3, AMT4,                                 --진찰료특진료,진찰료총액,진찰료조합부담";
            SQL += ComNum.VBLF + "        AMT5, AMT6, AMT7,                                 --진찰료감액,진찰료미수,진찰료영수금액";
            SQL += ComNum.VBLF + "        Part, Bohun, GelCode,                             --수납조,보훈여부,";
            SQL += ComNum.VBLF + "        CARDSEQNO, VCODE                                  --신용카드일련번호";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW            --외래예약자테이블";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND ROWID = '" + ArgRowid + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;

            }

            if (Dt.Rows.Count > 0)
            {
                rtnVal = "OK";

                TRV.Ptno = Dt.Rows[0]["Pano"].ToString().Trim();
                TRV.DeptCode = Dt.Rows[0]["DeptCode"].ToString().Trim();
                TRV.Bi = Dt.Rows[0]["Bi"].ToString().Trim();
                TRV.sName = Dt.Rows[0]["sName"].ToString().Trim();
                TRV.DrCode = Dt.Rows[0]["DrCode"].ToString().Trim();
                TRV.Date1 = Dt.Rows[0]["DateJupsu"].ToString().Trim();
                TRV.Date2 = Dt.Rows[0]["DateYeyak"].ToString().Trim();
                TRV.Date3 = Dt.Rows[0]["DateRetry"].ToString().Trim();
                TRV.Chojae = Dt.Rows[0]["Chojae"].ToString().Trim();
                TRV.GbGameK = Dt.Rows[0]["GbGameK"].ToString().Trim();
                TRV.GbGameKC = Dt.Rows[0]["GbGameKC"].ToString().Trim();
                TRV.GbSpc = Dt.Rows[0]["GbSpc"].ToString().Trim();
                TRV.Jin = Dt.Rows[0]["Jin"].ToString().Trim();
                TRV.Amt1 = Convert.ToInt64(Dt.Rows[0]["Amt1"].ToString().Trim());
                TRV.Amt2 = Convert.ToInt64(Dt.Rows[0]["Amt2"].ToString().Trim());
                TRV.Amt3 = Convert.ToInt64(Dt.Rows[0]["Amt3"].ToString().Trim());
                TRV.Amt4 = Convert.ToInt64(Dt.Rows[0]["Amt4"].ToString().Trim());
                TRV.Amt5 = Convert.ToInt64(Dt.Rows[0]["Amt5"].ToString().Trim());
                TRV.Amt6 = Convert.ToInt64(Dt.Rows[0]["Amt6"].ToString().Trim());
                TRV.Amt7 = Convert.ToInt64(Dt.Rows[0]["Amt7"].ToString().Trim());
                TRV.Part = Dt.Rows[0]["Part"].ToString().Trim();
                TRV.Bohun = Dt.Rows[0]["Bohun"].ToString().Trim();
                TRV.GelCode = Dt.Rows[0]["GelCode"].ToString().Trim();
                TRV.CardSeqNo = Convert.ToInt64(VB.Val(Dt.Rows[0]["CARDSEQNO"].ToString().Trim()));
                TRV.VCode = Dt.Rows[0]["VCode"].ToString().Trim();

                FnOldAmts[0] = TRV.Amt1;     clsPmpaPb.gnJinAMT1 = TRV.Amt1;
                FnOldAmts[1] = TRV.Amt2;     clsPmpaPb.gnJinAMT2 = TRV.Amt2;
                FnOldAmts[2] = TRV.Amt3;     clsPmpaPb.gnJinAMT3 = TRV.Amt3;
                FnOldAmts[3] = TRV.Amt4;     clsPmpaPb.gnJinAMT4 = TRV.Amt4;
                FnOldAmts[4] = TRV.Amt5;     clsPmpaPb.gnJinAMT5 = TRV.Amt5;
                FnOldAmts[5] = TRV.Amt6;     clsPmpaPb.gnJinAMT6 = TRV.Amt6;
                FnOldAmts[6] = TRV.Amt7;     clsPmpaPb.gnJinAMT7 = TRV.Amt7;

                clsPmpaType.TBP.Sname = TRV.sName;
                clsPmpaType.TBP.DrCode = TRV.DrCode;
                clsPmpaType.TBP.GbGameK = TRV.GbGameK;
                clsPmpaType.TBP.GbGameKC = TRV.GbGameKC;
                clsPmpaType.TBP.GbSpc = TRV.GbSpc;
            }
            else
            {
                TRV.Ptno = "";
                TRV.DeptCode = "";
                TRV.Bi = "";
                TRV.sName = "";
                TRV.DrCode = "";
                TRV.Date1 = "";
                TRV.Date2 = "";
                TRV.Date3 = "";
                TRV.Chojae = "";
                TRV.GbGameK = "";
                TRV.GbGameKC = "";
                TRV.GbSpc = "";
                TRV.Jin = "";
                TRV.Amt1 = 0;
                TRV.Amt2 = 0;
                TRV.Amt3 = 0;
                TRV.Amt4 = 0;
                TRV.Amt5 = 0;
                TRV.Amt6 = 0;
                TRV.Amt7 = 0;
                TRV.Part = "";
                TRV.Bohun = "";
                TRV.GelCode = "";
                TRV.CardSeqNo = 0;
                TRV.VCode = "";
            }

            Dt.Dispose();
            Dt = null;

            return rtnVal;
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            FnRow = e.Row;
        }

        private void btnRePrint_Click(object sender, EventArgs e)
        {
            if (FnRow > ssList_Sheet1.RowCount -1)
            {
                ComFunc.MsgBox("재출력 환자가 선택되지 않았습니다.");
                return;
            }

            if (ssList_Sheet1.Cells[FnRow, 0].Text == "")
            {
                ComFunc.MsgBox("재출력 환자가 선택되지 않았습니다.");
                return;
            }

            string strPtno = txtPtno.Text.Trim();
            string strSname = ssList_Sheet1.Cells[FnRow, 0].Text.Trim();
            string strDeptCode = ssList_Sheet1.Cells[FnRow, 1].Text.Trim();
            string strDeptName = CF.READ_DEPTNAMEK(clsDB.DbCon, strDeptCode);
            string strDrCode = ssList_Sheet1.Cells[FnRow, 2].Text.Trim();
            string strRdate = ssList_Sheet1.Cells[FnRow, 3].Text.Trim();
            string strRtime = ssList_Sheet1.Cells[FnRow, 4].Text.Trim();
            string strActdate = ssList_Sheet1.Cells[FnRow, 6].Text.Trim();
            if (DialogResult.No == ComFunc.MsgBoxQ("예약증을 출력하시겠습니까?", "출력", MessageBoxDefaultButton.Button1))
                return;

            //예약접수증
            if (chkRe.Checked == true)
                CPP.ResJupsu_Print(clsDB.DbCon, "1", strPtno, strSname, strDeptCode, strDeptName, strDrCode, strRdate + " " + strRtime, strActdate, ssResJupsuPrint);
            else
                CPP.ResJupsu_Print(clsDB.DbCon, "0", strPtno, strSname, strDeptCode, strDeptName, strDrCode, strRdate + " " + strRtime, strActdate, ssResJupsuPrint);

        }

        private void cboGamek_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strBdate = "";
            string strJumin = "";
            string strBi = "";
            string strGelCode = "";

            txtGamekName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_감액코드명", cboGamek.Text.Trim(), "J");
            strBdate = txtResDate.Text;

            strJumin = ssPat_Sheet1.Cells[1, 2].Text.ToString();
            strBi = cboBi.Text.Trim();
            strGelCode = ssBohum_Sheet1.Cells[7, 2].Text.ToString();

            if (CPF.READ_GAMEK_RATE(clsDB.DbCon, cboGamek.Text, strBi, strBdate, strGelCode, "") == true)
            {
                if (strGelCode == "H911")//소방전문치료센터 협약 (감액구분 23과 동일하게 적용)
                    CPF.READ_GAMEK_RATE_H911(clsDB.DbCon, cboGamek.Text, strBi, strBdate, strGelCode, "", strJumin);
                else
                    CPF.READ_GAMEK_RATE_EVENT(clsDB.DbCon, cboGamek.Text, strBi, strBdate, strGelCode, "", strJumin);
            }

            if (cboJin.Text != "" && cboChojae.Text != "")
                GESAN_JIN_AMT();
        }

        private void cboGamekCase_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strBdate = "";
            string strJumin = "";
            string strBi = "";
            string strGelCode = "";

            txtGamekCaseName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_감액코드명", cboGamekCase.Text.Trim(), "C");
            strBdate = txtResDate.Text;

            strJumin = ssPat_Sheet1.Cells[1, 2].Text.ToString();
            strBi = cboBi.Text.Trim();
            strGelCode = ssBohum_Sheet1.Cells[7, 2].Text.ToString();

            if (CPF.READ_GAMEK_RATE(clsDB.DbCon, cboGamekCase.Text, strBi, strBdate, strGelCode, "") == true)
            {
                if (strGelCode == "H911")//소방전문치료센터 협약 (감액구분 23과 동일하게 적용)
                    CPF.READ_GAMEK_RATE_H911(clsDB.DbCon, cboGamekCase.Text, strBi, strBdate, strGelCode, "", strJumin);
                else
                    CPF.READ_GAMEK_RATE_EVENT(clsDB.DbCon, cboGamekCase.Text, strBi, strBdate, strGelCode, "", strJumin);
            }

            if (cboJin.Text != "" && cboChojae.Text != "")
                GESAN_JIN_AMT();

        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDept.Text != "")
                txtDeptName.Text = clsPmpaPb.GstrSetDepts[cboDept.SelectedIndex];
            else
                txtDeptName.Text = "";
        }

        private void cboDrCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDrName.Text = CF.READ_DrName(clsDB.DbCon, cboDrCode.Text.Trim());
        }

        private void cboCan_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCanName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_중증암환자", cboCan.Text.Trim());
        }

        private void cboMcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMcodeName.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_의료급여본인부담", cboMcode.Text.Trim());
        }

        private void cboChojae_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtChojaeName.Text = clsPmpaPb.GstrSetChoJaes[Convert.ToInt32(cboChojae.Text.Trim())];
        }

        private void cboJin_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtJinName.Text = CF.READ_JIN(clsDB.DbCon,cboJin.Text.Trim());
        }

        private void cboResTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            READ_RESV_CNT_CHK();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboBi_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBiName.Text = clsPmpaPb.GstrSetBis[Convert.ToInt32(cboBi.Text.Trim())];
        }

        private void ssPat_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column.Equals(2))
            {
                FstrOldData = ssPat_Sheet1.Cells[e.Row, e.Column].Text.Trim();
                ssPat_Sheet1.OperationMode = clsSpread.NORMAL;
            }
            else if (e.Row.Equals(1) && e.Column.Equals(3))
            {
                FstrOldData = ssPat_Sheet1.Cells[e.Row, e.Column].Text.Trim();
                ssPat_Sheet1.OperationMode = clsSpread.NORMAL;
            }

        }

        private void ssBohum_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            if (e.Column.Equals(2))
            {
                FstrOldData = ssBohum_Sheet1.Cells[e.Row, e.Column].Text.Trim();
                ssBohum_Sheet1.OperationMode = clsSpread.NORMAL;
            }

            if (e.Row.Equals(0) && e.Column.Equals(2))//피보관계
            {
                ComboBoxCellType cboCell = new ComboBoxCellType();
                cboCell.Items = new string[] { "1", "2", "3", "4", "5" };
                cboCell.ItemData = new string[] { "본인", "부모", "자녀", "배우자", "기타" };
                cboCell.AcceptsArrowKeys = FarPoint.Win.SuperEdit.AcceptsArrowKeys.AllArrows;
                cboCell.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
                cboCell.Editable = true;
                cboCell.ListAlignment = FarPoint.Win.ListAlignment.Left;
                cboCell.ListOffset = 20;
                cboCell.ListWidth = -1; 
                cboCell.MaxDrop = 5;
                cboCell.EditorValue = EditorValue.ItemData;
                ssBohum_Sheet1.Cells[0, 2].CellType = cboCell;
            }
            else if (e.Row.Equals(6) && e.Column.Equals(2))//보훈,장애
            {
                ComboBoxCellType cboCell = new ComboBoxCellType();
                cboCell.Items = new string[] { "0", "1", "2", "3" };
                cboCell.ItemData = new string[] { "해당없음", "보훈청", "시각장애자", "장애인"};
                cboCell.AcceptsArrowKeys = FarPoint.Win.SuperEdit.AcceptsArrowKeys.AllArrows;
                cboCell.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
                cboCell.Editable = true;
                cboCell.ListAlignment = FarPoint.Win.ListAlignment.Left;
                cboCell.ListOffset = 20;
                cboCell.ListWidth = -1;
                cboCell.MaxDrop = 4;
                cboCell.EditorValue = EditorValue.ItemData;
                ssBohum_Sheet1.Cells[6, 2].CellType = cboCell;
            }

        }


        private void ssBohum_ComboSelChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Row == 0 && e.Column ==2)
                ssBohum_Sheet1.Cells[0, 3].Text = ssBohum_Sheet1.Cells[0, 2].Value.ToString();

            if (e.Row == 6 && e.Column == 2)
                ssBohum_Sheet1.Cells[6, 3].Text = ssBohum_Sheet1.Cells[6, 2].Value.ToString();

        }

        private void txtResDate_Validated(object sender, EventArgs e)
        {
            string strGbn = string.Empty;
            string strMessage = string.Empty;

            int nTime = 0;
            


            if (txtResDate.Text.Trim() == "") { return; }
            if (VB.Replace(txtResDate.Text, "-", "").Length != 8)
            {
                ComFunc.MsgBox("예약일자 오류!! 정확한 날짜 입력요망!!", "알림");
                txtResDate.Select();
                return;
            }

            if (txtResDate.Text.Length == 8)
                txtResDate.Text = VB.Left(txtResDate.Text.Trim(), 4) + "-" + VB.Mid(txtResDate.Text.Trim(), 5, 2) + "-" + VB.Right(txtResDate.Text.Trim(), 2);

            if (txtResDate.Text.Length == 8)
                txtResDate.Text = VB.Left(txtResDate.Text.Trim(), 4) + "-" + VB.Mid(txtResDate.Text.Trim(), 5, 2) + "-" + VB.Right(txtResDate.Text.Trim(), 2);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT HOLYDAY ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_JOB ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND JOBDATE   = TO_DATE('" + txtResDate.Text + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                if (Dt.Rows[0]["HOLYDAY"].ToString().Trim() == "*")
                    sBarMsg1.Text = "해당 예약일은 휴일 입니다.";
            }

            Dt.Dispose();
            Dt = null;

            strMessage = CheckSchedule(cboDrCode.Text.Trim(), txtResDate.Text.Trim());
            if (strMessage != "")
            {
                ComFunc.MsgBox(strMessage, "확인!");
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT YTIMEGBN, AMTIME, PMTIME, ";
            SQL += ComNum.VBLF + "        nvl(YINWON,0) YINWON , AMTIME2, PMTIME2, nvl(YINWON2,0) YINWON2 ";  // null 값오류 2018-08-08
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND DRCODE = '" + cboDrCode.Text.Trim() + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                FstrAmTime = Dt.Rows[0]["AMTIME"].ToString().Trim();
                FstrAmTime2 = Dt.Rows[0]["AMTIME2"].ToString().Trim();
                FstrPmTime = Dt.Rows[0]["PMTIME"].ToString().Trim();
                FstrPmTime2 = Dt.Rows[0]["PMTIME2"].ToString().Trim();

                FnInWon = Convert.ToInt32(Dt.Rows[0]["YINWON"].ToString().Trim());
                FnInWon2 = Convert.ToInt32(Dt.Rows[0]["YINWON2"].ToString().Trim());

                strGbn = Dt.Rows[0]["YTIMEGBN"].ToString().Trim();
                if (strGbn == "") { strGbn = "4"; }
                switch (strGbn)
                {
                    case "1":
                        nTime = 10;
                        break;
                    case "2":
                        nTime = 15;
                        break;
                    case "3":
                        nTime = 20;
                        break;
                    case "4":
                        nTime = 30;
                        break;
                }
            }

            Dt.Dispose();
            Dt = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT GBJIN, GBJIN2 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND SCHDATE   = TO_DATE('" + txtResDate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DRCODE    = '" + cboDrCode.Text.Trim() + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                ssRes_Sheet1.Cells[0, 1].Text = CPF.Get_DrSchedule_Gubun(Dt.Rows[0]["GBJIN"].ToString().Trim());
                ssRes_Sheet1.Cells[0, 2].Text = FstrAmTime;
                ssRes_Sheet1.Cells[0, 3].Text = FstrAmTime2;

                ssRes_Sheet1.Cells[1, 1].Text = CPF.Get_DrSchedule_Gubun(Dt.Rows[0]["GBJIN2"].ToString().Trim());
                ssRes_Sheet1.Cells[1, 2].Text = FstrPmTime;
                ssRes_Sheet1.Cells[1, 3].Text = FstrPmTime2;

                if (FstrRowid_OCSResv == "") { cboResTime.Items.Clear(); }

                if (Dt.Rows[0]["GBJIN"].ToString().Trim() == "1")
                {
                    for (int i = CF.TIME_MI(FstrAmTime); i <= CF.TIME_MI(FstrAmTime2); i = i + nTime)
                        cboResTime.Items.Add(CF.TIME_MI_TIME(i));
                }

                if (Dt.Rows[0]["GBJIN2"].ToString().Trim() == "1")
                {
                    for (int i = CF.TIME_MI(FstrPmTime); i <= CF.TIME_MI(FstrPmTime2); i = i + nTime)
                        cboResTime.Items.Add(CF.TIME_MI_TIME(i));
                }

            }


            Dt.Dispose();
            Dt = null;

            string strJumin = ssPat_Sheet1.Cells[1, 2].Text;
            strJumin = strJumin + ssPat_Sheet1.Cells[1, 3].Text;

            clsPmpaPb.GnAge = ComFunc.AgeCalcEx(strJumin, txtResDate.Text);

            YEYAK_CHOJAE_SET();//진찰료셋팅
        }

        private void ssWard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string strDeptCode = string.Empty;
                string strDrCode = string.Empty;
                string strRtime = string.Empty;

                FstrRowid_OCSResv = "";

                ssPat.Enabled = true;
                ssBohum.Enabled = true;

                FstrFlagRV = "NO";
                pnlLeft.Enabled = true;
                btnSave.Enabled = true;
                chkCard.Enabled = true;
                btnCancel.Enabled = true;

                DISPLAY_SCREEN(clsDB.DbCon);

                if (FstrTchk.Equals("Y"))
                {
                    ComFunc.MsgBox("선택한 예약이 이미 존재합니다." + "\r" + "퇴원예약을 하실려면 창을 닫고 다시 하시기 바랍니다.", "확인");
                    return;
                }

                if (ssWard_Sheet1.ActiveRowIndex >= 0 && ssWard_Sheet1.ActiveColumnIndex >= 0)
                {
                    strDeptCode = ssWard_Sheet1.Cells[ssWard_Sheet1.ActiveRowIndex, 0].Text.Trim();
                    strDrCode = ssWard_Sheet1.Cells[ssWard_Sheet1.ActiveRowIndex, 6].Text.Trim();
                    strRtime = ssWard_Sheet1.Cells[ssWard_Sheet1.ActiveRowIndex, 2].Text.Trim();

                    FstrRowid_OCSResv = ssWard_Sheet1.Cells[ssWard_Sheet1.ActiveRowIndex, 7].Text.Trim();

                    cboDept.Text = strDeptCode;
                    txtDeptName.Text = CF.READ_DEPTNAMEK(clsDB.DbCon, strDeptCode);

                    txtResDate.Text = VB.Left(strRtime, 10);
                    cboResTime.Text = VB.Right(strRtime, 5);

                    cboDrCode.Text = strDrCode;
                    txtDrName.Text = CF.READ_DrName(clsDB.DbCon, strDrCode);

                    cboDrCode.Enabled = true;
                    //cboChojae.Select();
                }

                cboChojae.Focus();
            }

        }

        private void cboResTime_Validated(object sender, EventArgs e)
        {
            string strMessage = string.Empty;

            strMessage = CheckSchedule(cboDrCode.Text.Trim(), txtResDate.Text.Trim(), cboResTime.Text.Trim());
            if (strMessage != "")
            {
                ComFunc.MsgBox(strMessage, "확인!");
            }
        }

        private void txtResUpdate_Validated(object sender, EventArgs e)
        {
            string strMessage = "";

            strMessage = CheckSchedule(cboDrCode.Text.Trim(), txtResUpdate.Text.Trim());
            if (strMessage != "")
            {
                ComFunc.MsgBox(strMessage, "확인!");
            }
        }

        private void txtResUptime_Validated(object sender, EventArgs e)
        {

            string strMessage = "";

            strMessage = CheckSchedule(cboDrCode.Text.Trim(), txtResUpdate.Text.Trim(), txtResUptime.Text.Trim());
            if (strMessage != "")
            {
                ComFunc.MsgBox(strMessage, "확인!");
            }
        }
    }
}
