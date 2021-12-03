using ComBase;
using ComDbB;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

/// <summary>
/// Description : 환자 구분변경
/// Author : 김민철
/// Create Date : 2018.03.07
/// </summary>
/// <history>
/// </history>
/// <seealso cref=" \\vb60_new\IPD\iument\Frm환자구분변경.frm"/>
namespace ComPmpaLibB
{
    public partial class frmPmpaTransChange : Form
    {
        #region 클래스 선언 및 etc....

        clsSpread cSpd  = new clsSpread();
        clsPmpaFunc cPF = new clsPmpaFunc();
        ComFunc CF      = new ComFunc();
        clsPmpaPb cPB   = new clsPmpaPb();
        clsIument cIuM  = new clsIument();

        long FnIPDNO = 0;
        long FnNewTRSNO = 0;
        string FstrPano      = string.Empty;
        string FstrSName     = string.Empty;
        string FstrInDate = "1900-01-01";
        string FstrOutDate = "1900-01-01";
        string FstrInDate2   = string.Empty;
        string FstrOutDate2  = string.Empty;
        string FstrJumin1    = string.Empty;
        string FstrJumin2    = string.Empty;
        string FstrDrg       = string.Empty;
        string FstrNewilban2 = string.Empty;
        string FstrOldilban2 = string.Empty;
        string FstrAllNo     = string.Empty;
        string FstrBi        = string.Empty;
        string FstrGbIPD     = string.Empty;
        
        const int MAX_OLD_TRANS = 6;

        int FnOldTrsCNT         = 0;
        string FstrOldGbIPD     = string.Empty;
        long[] FnOldTransNo     = new long[MAX_OLD_TRANS];
        string[] FstrOldInDate  = new string[MAX_OLD_TRANS];
        string[] FstrOldOutDate = new string[MAX_OLD_TRANS];
        
        #endregion
        
        public frmPmpaTransChange()
        {
            InitializeComponent();
            setEvent();
            setCombo();
        }
        
        void setEvent()
        {
            this.Load                   += new EventHandler(eFormLoad);

            //버튼 클릭 이벤트
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.btnSearch2.Click       += new EventHandler(eBtnClick);
            this.btnSave.Click          += new EventHandler(eBtnClick);
            this.btnHellp.Click         += new EventHandler(eBtnClick);
            this.btnCancel.Click        += new EventHandler(eBtnClick);
            this.btnCancel_End.Click    += new EventHandler(eBtnClick);

            //KeyPress 이벤트
            this.txtPano.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.txtGel.KeyPress        += new KeyPressEventHandler(eKeyPress);
            this.cboNewBi.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.cboRange.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.dtpDateF.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.dtpDateT.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.rdoJob0.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.rdoJob1.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.txtGelCode.KeyPress    += new KeyPressEventHandler(eKeyPress);

            //Spread 이벤트
            this.ssTrans.ButtonClicked  += new EditorNotifyEventHandler(eSpreadButtonClick);
            
            //DataTimePicker 이벤트
            this.dtpDateF.ValueChanged  += new EventHandler(CF.eDtpFormatSet);
            this.dtpDateT.ValueChanged  += new EventHandler(CF.eDtpFormatSet);
            this.dtpBirth.ValueChanged  += new EventHandler(CF.eDtpFormatSet);
            this.dtpDate1.ValueChanged  += new EventHandler(CF.eDtpFormatSet);
            this.dtpDate2.ValueChanged  += new EventHandler(CF.eDtpFormatSet);
            this.dtpDate3.ValueChanged  += new EventHandler(CF.eDtpFormatSet);

            //ComboBox 이벤트
            this.cboNewBi.SelectedIndexChanged += new EventHandler(eCboChange);
            this.cboRange.SelectedIndexChanged += new EventHandler(eCboChange);
        }

        void setCombo()
        {
            CF.Combo_BCode_SET(clsDB.DbCon, cboNewBi, "BAS_환자종류", true, 2, "N");
            CF.Combo_BCode_SET(clsDB.DbCon, cboGwange, "BAS_피보험자관계", true, 1, "N");

            cboRange.Items.Clear();
            cboRange.Items.Add("1.전체");
            cboRange.Items.Add("2.앞부분");
            cboRange.Items.Add("3.뒷부분");
            cboRange.SelectedIndex = 0;
        }
        
        void eFormLoad(object sender, EventArgs e)
        {
            clsComPmpaSpd cPSpd = new clsComPmpaSpd();

            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //else
            //{

                cPSpd.sSpd_enmIpdTrsChg(ssTrans, cPB.sSpdIpdTrsChg, cPB.nSpdTrsChg, 10, 0);
                
                //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                
                Screen_Clear();
            
            //}
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dt = null;

            if (sender == txtPano && e.KeyChar == (char)13)
            {
                #region txtPano KeyPres
                if (txtPano.Text.Trim() == "")
                {
                    return;
                }

                txtPano.Text = VB.Format(VB.Val(txtPano.Text), "00000000");

                dt = cPF.Get_Ipd_New_Master(clsDB.DbCon, txtPano.Text, "", 0);

                if (dt == null)
                {
                    ComFunc.MsgBox("입원 환자정보가 없습니다.", "확인");
                    return;
                }

                dt.Dispose();
                dt = null;

                if (Screen_Display(clsDB.DbCon, ssTrans, txtPano.Text) == false)     //자격목록 조회
                {
                    return;
                }

                clsLockCheck.GstrLockPtno = txtPano.Text.Trim();
                clsLockCheck.GstrLockRemark = clsType.User.IdNumber + " " + clsType.User.JobName + "님이 환자구분 변경작업 중입니다.";
                if (clsLockCheck.IpdOcs_Lock_Insert_NEW() != "OK")
                {
                    lblSName.Text = "";
                    ssTrans.ActiveSheet.RowCount = 0;
                    return;
                }
                
                #endregion
            }
            else if (sender == txtGel && e.KeyChar == (char)13)
            {
                #region txtGel KeyPress
                if (txtGel.Text.Trim() == "")
                {
                    lblGel.Text = "";
                    lblMsg.Text = "";
                    return;
                }
                else
                {
                    string strMiaName = CF.Read_MiaName(clsDB.DbCon, txtGel.Text.Trim(), false);

                    if (strMiaName == "")
                    {
                        lblMsg.Text = "산재 계약처 코드 ERROR !!";
                        lblGel.Text = "";
                        txtGel.Focus();
                    }
                    else
                    {
                        lblMsg.Text = strMiaName;
                    }
                }
                #endregion
            }
            else if (sender == cboNewBi && e.KeyChar == (char)13)
            {
                cboRange.Focus();
            }
            else if (sender == cboRange && e.KeyChar == (char)13)
            {
                dtpDateF.Focus();
            }
            else if (sender == dtpDateF && e.KeyChar == (char)13)
            {
                dtpDateT.Focus();
            }
            else if (sender == dtpDateT && e.KeyChar == (char)13)
            {
                rdoJob0.Focus();
            }
            else if (sender == rdoJob0 && e.KeyChar == (char)13)
            {
                rdoJob1.Focus();
            }
            else if (sender == rdoJob1 && e.KeyChar == (char)13)
            {
                txtGelCode.Focus();
            }
            else if (sender == txtGelCode && e.KeyChar == (char)13)
            {
                btnSearch.Focus();
            }

        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                if (txtPano.Text.Trim() != "")
                {
                    clsLockCheck.GstrLockPtno = txtPano.Text.Trim();
                    clsLockCheck.IpdOcs_Lock_Delete_NEW(txtPano.Text.Trim());
                    clsLockCheck.GstrLockPtno = "";
                }
                txtPano.Focus();
                this.Close();
                return;
            }
            else if (sender == this.btnSave)
            {
                eSave(clsDB.DbCon);
            }
            else if (sender == this.btnSearch)
            {
                eSearch_Nhic(clsDB.DbCon);          //자격조회
            }
            else if (sender == this.btnSearch2)
            {
                if (txtPano.Text.Trim() == "" || FstrAllNo == "")
                {
                    return;
                }

                cIuM.Read_Ipd_Mst_Trans(clsDB.DbCon, txtPano.Text, Convert.ToInt64(FstrAllNo), "");

                clsPublic.GstrRetValue = "환자구분변경";

                frmSimsaConfirm frm = new frmSimsaConfirm(Convert.ToInt64(FstrAllNo));
                frm.ShowDialog();

            }
            else if (sender == this.btnCancel)
            {
                eBtnCancel();                       //취소                
            }
            else if (sender == this.btnHellp)
            {
                clsPublic.GstrPANO = "";

                frmPmPaJewonFind frm = new frmPmPaJewonFind(txtPano.Text.Trim());
                frm.ShowDialog();

                if (clsPublic.GstrPANO != "")
                {
                    txtPano.Text = clsPublic.GstrPANO;
                }
            }
            else if (sender == this.btnCancel_End)
            {
                if (clsLockCheck.GstrLockPtno != "")
                {
                    clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                    clsLockCheck.GstrLockPtno = "";
                }
                Screen_Clear();
                this.Close();
                return;
            }
        }

        void eCboChange(object sender, EventArgs e)
        {
            if (sender == cboNewBi)
            {
                #region cboNewBi Events
                lblNewBi.Text = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_환자종류", cboNewBi.Text.Trim());

                if (cboNewBi.Text.Trim() == "31")
                {
                    grpBox.Enabled = true;
                }

                if (cboNewBi.Text.Trim() == "52")
                {
                    lblJKiho.Text = "계약처";
                    lblGKiho.Text = "차량번호";
                }
                else
                {
                    lblJKiho.Text = "조합기호";
                    lblGKiho.Text = "증번호";
                }
                #endregion
            }
            else if (sender == cboRange)
            {
               // dtpSDate.Format = DateTimePickerFormat.Short;
                #region cboRange Events
                if (VB.Left(cboRange.Text, 1) == "1")
                {
                    dtpDateF.Value = Convert.ToDateTime(FstrInDate);
                    dtpDateT.Value = Convert.ToDateTime(FstrOutDate); ;
                }
                else if (VB.Left(cboRange.Text, 1) == "2")
                {
                    dtpDateF.Text = FstrInDate;
                    dtpDateT.Text = "";
                    ComFunc.MsgBox("자격은 1.전체, 3.뒷부분으로 처리하십시오!!" + ComNum.VBLF + ComNum.VBLF + "2.앞부분 구분변경 금지!!", "확인");
                }
                else if (VB.Left(cboRange.Text, 1) == "3")
                {
                    dtpDateF.Text = "";
                    dtpDateT.Text = FstrOutDate;
                }
                #endregion
            }
        }

        void eSpreadButtonClick(object sender, EditorNotifyEventArgs e)
        {
            int i = 0, nCNT = 0;
            bool bIlban = false;
            bool bJibyung = false;
            
            FstrAllNo = "";
            FstrInDate = "";
            FstrOutDate = "";
            FstrDrg = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            for (i = 0; i < ssTrans.ActiveSheet.RowCount; i++)
            {
                if (ssTrans.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.chk01].Text == "True")
                {
                    lblGKihoMsg.Text = "증기호: " + ssTrans.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.GKIHO].Text.Trim();

                    FstrOldilban2 = "";
                    if (ssTrans.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.GBILBAN2].Text.Trim() == "Y")
                    {
                        FstrOldilban2 = "Y";
                        ComFunc.MsgBox("이자격은 선택진료 대상자입니다.!!" + ComNum.VBLF + ComNum.VBLF + "자격변경시 참고하십시오. 작업은 한개씩만 하십시오.", "확인");
                    }

                    if (VB.Val(ssTrans.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.GBSTS].Text) > 0)
                    {
                        ComFunc.MsgBox("환자구분변경시 재원상태에서만 변경이 가능합니다.", "확인");
                        ssTrans.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.chk01].Text = "False";
                        return;
                    }

                    FstrBi = ssTrans.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.BI].Text.Trim();
                    //시작일자
                    if (FstrInDate == "")
                    {
                        FstrInDate = ssTrans.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.INDATE].Text.Trim();
                    }
                    //종료일자
                    if (string.Compare(FstrOutDate, ssTrans.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.OUTDATE].Text.Trim()) < 0)
                    {
                        FstrOutDate = ssTrans.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.OUTDATE].Text.Trim();
                    }

                    //변경할 작업기간
                    dtpDateF.Text = FstrInDate;
                    dtpDateT.Text = clsPublic.GstrSysDate;

                    if (ssTrans.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.RDATE].Text.Trim() == "")
                    {
                        rdoJob1.Checked = true; //재원
                    }
                    else
                    {
                        rdoJob0.Checked = true; //강제퇴원
                    }

                    FstrAllNo += ssTrans.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.TRSNO].Text.Trim() + ",";
                    //지병인지 여부
                    if (ssTrans.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.GBIPD].Text.Trim() == "지병")
                    {
                        FstrGbIPD = "9";
                        bJibyung = true;
                    }
                    else
                    {
                        FstrGbIPD = "1";
                        bIlban = true;
                    }

                    nCNT += 1;

                    if (ssTrans.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.GBDRG].Text.Trim() == "D")
                    {
                        FstrDrg = "D";
                    }
                }
            } //End of For

            if (FstrAllNo == "") { return; }

            if (bJibyung == true && bIlban == true)
            {
                ssTrans.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmIpdTrsChg.chk01].Text = "False";
                ComFunc.MsgBox("일반과 지병을 함께 선택하실수는 없습니다.", "오류");

                for (i = 0; i < ssTrans.ActiveSheet.RowCount; i++)
                {
                    ssTrans.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.chk01].Text = "False";
                }
                FstrAllNo = "";
                FstrInDate = "";
                FstrOutDate = "";
                return;
            }
            cboRange.SelectedIndex = -1;
            //마지막 컴마를 제거함
            if (FstrAllNo != "") { FstrAllNo = VB.Left(FstrAllNo, FstrAllNo.Length - 1); }

            if (nCNT > 1)
            {
                cboRange.SelectedIndex = 0;
                cboRange.Enabled = false;
            }
            else
            {
                cboRange.SelectedIndex = 0;
                cboRange.Enabled = true;
            }
            
        }

        void eSave(PsmhDb pDbCon)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowCnt = 0;
            int i = 0;
            string strGbilban2 = string.Empty;
            string strChk = string.Empty;
            string strTrsSQL = string.Empty;
            string strGbIPD = string.Empty;
            string strHisJob = string.Empty;
            string strInDateSt = string.Empty;
            string strOutDateSt = string.Empty;
            
            clsIpdAcct cIAcct = new clsIpdAcct();

            ComFunc.ReadSysDate(pDbCon);
            clsPmpaPb.GstrActDate = clsPublic.GstrSysDate;

            #region Data_Check
            lblMsg.Text = "";
            if (chkilban2.Checked == true)
            {
                strGbilban2 = "Y";
                FstrNewilban2 = "Y";
            }
            
            if (VB.Left(cboRange.Text, 1) == "2")
            {
                ComFunc.MsgBox("2.앞부분 구분변경 할수없습니다!", "확인");
                return;
            }
            
            //2015-01-07 DRG의 경우 연초 기간분리 작업시 확인멘트
            if (FstrDrg == "D")
            { 
                if (VB.Mid(dtpDateF.Text, 6, 5) == "01-01")
                { 
                    if (ComFunc.MsgBoxQ("DRG 대상은 1월1일부 기간분리 대상자가 아닙니다. 작업하시겠습니까?", "작업확인요망!!", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                }
            }

            if (rdoJob0.Checked)
            {
                clsPmpaPb.GstrChk = "1";
            }
            else if (rdoJob1.Checked)
            {
                clsPmpaPb.GstrChk = "2";
            }
            else
            {
                ComFunc.MsgBox("환자 구분 상태에 체크를 하세요.", "확인");
                return;
            }

            //외국new
            if (cboNewBi.Text.Trim() != "51" && chkilban2.Checked == true)
            {
                ComFunc.MsgBox("외국인 일반수가 2배대상환자는 51종 일반만 가능함. 상태에 체크를 하세요", "확인");
                return;
            }
            
            if (cboNewBi.Text.Trim() == "51" && chkilban2.Checked == true)
            { 
                if (ComFunc.MsgBoxQ("외국인 일반수가 2배 대상으로 정말 작업을 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) ==  DialogResult.No)
                {
                    return;
                }
            }

            //확인창 팝업 2015-07-03
            clsPublic.GstrMsgTitle = "★★ 작업내용 확인 ★★";
            clsPublic.GstrMsgList = "";
            clsPublic.GstrMsgList += "기존자격: " + FstrBi + "종  =>  변경자격: " + cboNewBi.Text + " 종" + ComNum.VBLF;
            clsPublic.GstrMsgList += "전체기간: " + FstrInDate + " ~ " + FstrOutDate + ComNum.VBLF;
            clsPublic.GstrMsgList += "변경기간: " + dtpDateF.Text.Trim() + " ~ " + dtpDateT.Text + ComNum.VBLF + ComNum.VBLF;
            clsPublic.GstrMsgList += "구분변경 작업을 하시겠습니까? ";
            if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                return;
            }

            if (OLD_TransNo_Check(ssTrans) == false)
            {
                return;
            }

            if (TA_SAN_ID_Check(pDbCon, cboNewBi, txtPano.Text.Trim()) == false)   //자보,산재 ID Chcek
            {
                return;
            }

            if (Accept_Area_Kiho_Check(cboNewBi) == false)      //Accept Area 입력 여부
            {
                return;
            }

            if (Accept_Area_Entry_Check() == false)
            {
                return;
            }

            if(Accept_Area_Change_Check() == false)
            {
                return;
            }

            lblMsg.Text = "환자구분 변경 작업 중!! (손대지마시요)";

            string strSndBi = cboNewBi.Text.Trim();
            string strLange = VB.Left(cboRange.Text, 1);
            string strSndFday = dtpDateF.Text.Trim();
            string strSndTday = dtpDateT.Text.Trim();

            //퇴원작업 시점 포함 이전 미전송오더 체크
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,ORDERCODE,COUNT(PTNO) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_IORDER  ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PTNO      = '" + txtPano.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND GBSEND    = '*' ";
            SQL += ComNum.VBLF + "    AND (GBIOE = 'I' OR GBIOE IS NULL ) ";
            switch (strLange)
            {
                case "1":
                    SQL += ComNum.VBLF + "   AND BDATE >=TO_DATE('" + FstrInDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND BDATE <=TO_DATE('" + strSndTday + "','YYYY-MM-DD') ";
                    break;
                case "2":
                    SQL += ComNum.VBLF + "   AND BDATE >=TO_DATE('" + strSndFday + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND BDATE <=TO_DATE('" + strSndTday + "','YYYY-MM-DD') ";
                    break;
                case "3":
                    SQL += ComNum.VBLF + "   AND BDATE >=TO_DATE('" + strSndFday + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND BDATE <=TO_DATE('" + FstrOutDate + "','YYYY-MM-DD') ";
                    break;
                default:
                    SQL += ComNum.VBLF + "   AND BDATE >=TO_DATE('" + FstrInDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND BDATE <=TO_DATE('" + FstrOutDate + "','YYYY-MM-DD') ";
                    break;
            }
            SQL += ComNum.VBLF + "  GROUP BY TO_CHAR(BDATE,'YYYY-MM-DD'),ORDERCODE ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                dt.Dispose();
                dt = null;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                clsPublic.GstrMsgList = "";

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    clsPublic.GstrMsgList += dt.Rows[i]["BDATE"].ToString().Trim() + "일 ";
                    clsPublic.GstrMsgList += dt.Rows[i]["ORDERCODE"].ToString().Trim() + " ";
                    clsPublic.GstrMsgList += dt.Rows[i]["CNT"].ToString().Trim() + "건";
                }

                clsPublic.GstrMsgTitle = "확인";
                clsPublic.GstrMsgList += '\r' + "미전송 오더가 있습니다. 꼭 병동에서 오더를 정리해주세요..";
                clsPublic.GMsgButtons = MessageBoxButtons.OK;
                clsPublic.GMsgIcon = MessageBoxIcon.Information;
                MessageBox.Show(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, clsPublic.GMsgButtons, clsPublic.GMsgIcon);

                dt.Dispose();
                dt = null;
                return;
            }

            dt.Dispose();
            dt = null;
            #endregion

            //IPD_TRANS 새로 만듬
            FnNewTRSNO = cPF.GET_NEXT_TRSNO(pDbCon);

            //변경전 IPD_TRANS의 금액을 표시함
            IPD_TRANS_Amt_View(pDbCon, ssAmt, "1");

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(pDbCon);

                clsPmpaPb.GstrAcctJob = "ICUPDT";
                strTrsSQL = "";

                cIuM.Read_Ipd_Mst_Trans(pDbCon, txtPano.Text, FnOldTransNo[1], "");

                if (New_IPD_Trans_INSERT(pDbCon, FnIPDNO, FnNewTRSNO, FstrNewilban2) == false)
                {
                    ComFunc.MsgBox("IPD_TRANS (New 환자구분) INSERT ERROR", "작업실패");
                    clsDB.setRollbackTran(pDbCon);
                    if (clsLockCheck.GstrLockPtno != "") { clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno); }
                    clsLockCheck.GstrLockPtno = "";
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //2013-12-19 DRG 정보 Set을 복사해줌
                if (chkDrg.Checked == true)
                {
                    if (New_DRG_MASTER_INSERT(pDbCon, FnIPDNO, FnOldTransNo[1], FnNewTRSNO) == false)
                    {
                        ComFunc.MsgBox("DRG_MASTER (New 환자구분) INSERT ERROR", "작업실패");
                        clsDB.setRollbackTran(pDbCon);
                        if (clsLockCheck.GstrLockPtno != "") { clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno); }
                        clsLockCheck.GstrLockPtno = "";
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                for (i = 1; i <= FnOldTrsCNT; i++)
                {
                    //변경전 자격을 취소
                    cIuM.Read_Ipd_Mst_Trans(pDbCon, txtPano.Text, FnOldTransNo[i], "");

                    lblMsg.Text = clsPmpaType.TIT.Pano + " " + clsPmpaType.TIT.InDate + " " + clsPmpaType.TIT.Bi + " 구분변경 작업중 !!";
                    lblMsg.Refresh();

                    //환자구분 변경 처방 취소 및 새로운 자격에 INSERT
                    if (Return_Process(pDbCon, FnOldTransNo[i], strLange, strSndFday, strSndTday) == false)
                    {
                        ComFunc.MsgBox("IPD_MASTER (OLD Bi 재원 MASTER) UPDATE ERROR", "작업실패");
                        clsDB.setRollbackTran(pDbCon);
                        if (clsLockCheck.GstrLockPtno != "") { clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno); }
                        clsLockCheck.GstrLockPtno = "";
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    
                    strGbIPD = FstrOldGbIPD;

                    if (VB.Left(cboRange.Text, 1) == "1") { strGbIPD = "D"; }

                    if (cIAcct.IPD_Trans_Amt_Update(pDbCon, FnOldTransNo[i], strGbIPD) == false)
                    {
                        ComFunc.MsgBox("IPD_TRANS AMT UPDATE ERROR", "작업실패");
                        clsDB.setRollbackTran(pDbCon);
                        if (clsLockCheck.GstrLockPtno != "") { clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno); }
                        clsLockCheck.GstrLockPtno = "";
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    
                    //변경후 자격에 INSERT
                    if (FnOldTrsCNT == 1 && cIAcct.Suga_Read_YN_Check(clsPmpaType.TIT.Bi, strSndBi, FstrNewilban2, FstrOldilban2, "", "") == false && chkAmt.Checked == false)
                    { 
                        if (cIAcct.Create_Process_BB(pDbCon, FnOldTransNo[i], FnNewTRSNO, strSndBi, strLange, strSndFday, strSndTday) == false)
                        {
                            ComFunc.MsgBox("IPD_NEW_SLIP (NEW Bi 재원 SLIP) INSERT ERROR", "작업실패");
                            clsDB.setRollbackTran(pDbCon);
                            if (clsLockCheck.GstrLockPtno != "") { clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno); }
                            clsLockCheck.GstrLockPtno = "";
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {
                        strTrsSQL += FnOldTransNo[i].ToString() + ",";
                    }
                }

                //ARC 작업시 부분적으로 처리를 하면 문제가 있어 동시에 처리함
                if (strTrsSQL != "")
                {
                    strTrsSQL = VB.Left(strTrsSQL, strTrsSQL.Length - 1);
                    cIuM.Read_Ipd_Mst_Trans(pDbCon, txtPano.Text, FnNewTRSNO, "");
                    if (cIAcct.Create_Process(pDbCon, FnIPDNO, strTrsSQL, strLange, strSndFday, strSndTday, FstrOldGbIPD, "") == false)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        if (clsLockCheck.GstrLockPtno != "") { clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno); }
                        clsLockCheck.GstrLockPtno = "";
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                //구분변경한 마스타의 금액을 재집계
                if (cIAcct.IPD_Trans_Amt_Update(pDbCon, FnNewTRSNO, clsPmpaType.TIT.GbIpd) == false)
                {
                    ComFunc.MsgBox("IPD_TRANS AMT UPDATE ERROR", "작업실패");
                    clsDB.setRollbackTran(pDbCon);
                    if (clsLockCheck.GstrLockPtno != "")
                    {
                        clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                    }

                    clsLockCheck.GstrLockPtno = "";
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //변경후 IPD_TRANS의 금액을 표시함
                IPD_TRANS_Amt_View(pDbCon, ssAmt, "2");

                //New구분 IPD_TRANS의 금액을 표시함
                IPD_TRANS_Amt_View(pDbCon, ssAmt, "3");

                //구분변경작업 HISTORY
                if (rdoJob0.Checked == true)
                {
                    strHisJob = "3";
                }
                else
                {
                    strHisJob = "1";
                }

                SQL = "";
                SQL += " SELECT TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,    ";
                SQL += "        TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE   ";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_TRANS         ";
                SQL += "  WHERE TRSNO  = " + FnOldTransNo[1] + "        ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return; ;
                }

                if (dt.Rows.Count > 0)
                {
                    strInDateSt = dt.Rows[0]["INDATE"].ToString().Trim(); 
                    strOutDateSt = dt.Rows[0]["OUTDATE"].ToString().Trim(); 
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "IPD_BITRHISTORY                                      \r\n";
                SQL += "        (BITRDATE, PANO, SNAME, INDATE, OUTDATE, DEPTCODE, BITRJOB, BITRGBN,            \r\n";
                SQL += "        FRBI, TOBI, FRDATE, TODATE , SABUN,Gbilban2,GbSPC )                             \r\n";
                SQL += " VALUES                                                                                 \r\n";
                SQL += "        ( SYSDATE, '" + clsPmpaType.TIT.Pano + "', '" + clsPmpaType.TIT.Sname + "',     \r\n";
                SQL += "        TO_DATE('" + strOutDateSt + "','YYYY-MM-DD'),                                   \r\n";
                SQL += "        TO_DATE('" + strInDateSt + "','YYYY-MM-DD'),                                    \r\n";
                SQL += "        '" + clsPmpaType.TIT.DeptCode + "', '" + strHisJob + "',                        \r\n";
                SQL += "        '" + VB.Left(cboRange.Text, 1) + "', '" + FstrBi + "',                          \r\n";
                SQL += "        '" + cboNewBi.Text.Trim() + "', TO_DATE('" + strSndFday + "','YYYY-MM-DD'),     \r\n";
                SQL += "        TO_DATE('" + strSndTday + "','YYYY-MM-DD'), '" + clsType.User.IdNumber + "',    \r\n";
                SQL += "        '" + FstrOldilban2 + "','' )                                                        ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    ComFunc.MsgBox("IPD_BITRHISTROY INSERT Error", "작업실패");

                    if (clsLockCheck.GstrLockPtno != "")
                    {
                        clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                    }

                    clsLockCheck.GstrLockPtno = "";

                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return;
                }

                #region MIH 정보 UPDATE
                clsOumsad CPO = new clsOumsad();

                clsPmpaType.TMI.Ptno = txtPano.Text.Trim();
                clsPmpaType.TMI.Bi = cboNewBi.Text.Trim();
                clsPmpaType.TMI.TransDate = clsPublic.GstrSysDate;
                clsPmpaType.TMI.Kiho = txtKiho.Text.Trim();
                clsPmpaType.TMI.GKiho = txtGKiho.Text.Trim();
                clsPmpaType.TMI.Gwange = VB.Left(cboGwange.Text, 1);
                clsPmpaType.TMI.PName = txtPName.Text.Trim();
                CPO.UPDATE_BAS_MIH(pDbCon);
                #endregion

                clsDB.setCommitTran(pDbCon);

                cIuM.Ipd_Trans_Update(pDbCon, FnIPDNO);
                
                lblMsg.Text = "";
                ssAmt.Enabled = true;
                panEnd.Visible = true;
                exPanSub02.Enabled = false;
                exPanSub03.Visible = false;
                btnCancel_End.Focus();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
            
        }

        bool OLD_TransNo_Check(FpSpread Spd)
        {
            int i = 0;
            bool bIlban = false;
            bool bJibyung = false;
            bool rtnVal = true;

            FstrOldGbIPD = "";
            FnOldTrsCNT = 0;

            for (i = 0; i < MAX_OLD_TRANS; i++)
            {
                FnOldTransNo[i] = 0;
                FstrOldInDate[i] = "";
                FstrOldOutDate[i] = "";
            }

            for (i = 0; i < Spd.ActiveSheet.RowCount; i++)
            {
                if (Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.chk01].Text == "True")
                {
                    FnOldTrsCNT += 1;
                    if (FnOldTrsCNT > MAX_OLD_TRANS)
                    { 
                        ComFunc.MsgBox("구분변경할 명단 선택은 최대 " + MAX_OLD_TRANS + "개까지 가능합니다.", "오류");
                        return false;
                    }

                    FstrOldInDate[FnOldTrsCNT]  = Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.INDATE].Text.Trim();
                    FnOldTransNo[FnOldTrsCNT]   = (long)VB.Val(Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.TRSNO].Text);
                    FstrOldOutDate[FnOldTrsCNT] = Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.OUTDATE].Text.Trim();

                    if (Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmIpdTrsChg.GBIPD].Text.Trim() == "지병")
                    {
                        bJibyung = true;
                        FstrOldGbIPD = "9";
                    }
                    else
                    {
                        bIlban = true;
                        FstrOldGbIPD = "1";
                    }
                }
            }
            
            if (FnOldTrsCNT == 0)
            {
                ComFunc.MsgBox("구분변경할 내역을 1건도 선택을 안함",  "오류");
                return false;
            }

            if (bJibyung == true && bIlban == true)
            {
                ComFunc.MsgBox("일반과 지병을 함께 선택하실수는 없습니다.", "오류");
                return false;
            }

            return rtnVal;
        }

        bool TA_SAN_ID_Check(PsmhDb pDbCon, ComboBox cNewBi, string ArgPano)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            bool rtnVal = true;

            if (cboNewBi.Text.Trim() != "31" && cboNewBi.Text.Trim() != "52") { return rtnVal; }

            //BAS_SANID 자료 여부를 CHECK
            SQL = "";
            SQL += " SELECT Pano vPano FROM " + ComNum.DB_PMPA + "BAS_SANID     ";
            SQL += "  WHERE Pano = '" + ArgPano + "'                            ";
            SQL += "    AND Bi = '" + cNewBi.Text + "'                          ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                return false;
            }

            if (dt.Rows.Count == 0)
            {
                lblMsg.Text = "BAS_SANID 자료가 없음.";
                cNewBi.Focus();
                dt.Dispose();
                dt = null;
                return false;
            }
            
            return rtnVal;
        }

        bool Accept_Area_Kiho_Check(ComboBox cNewBi)
        {
            bool rtnVal = true;

            if (string.Compare(cNewBi.Text.Trim(), "40") > 0 && string.Compare(cNewBi.Text.Trim(), "52") < 0) { return rtnVal; }
            if (cNewBi.Text.Trim() == "55") { return rtnVal; }
            if (cNewBi.Text.Trim() == "33") { return rtnVal; }
            if (cNewBi.Text.Trim() == "32") { return rtnVal; }
            
            if (txtKiho.Text.Trim() == "")
            {
                lblMsg.Text = "조합기호가 정확하지 않습니다.";
                return false;
            }

            lblMsg.Text = "";

            return rtnVal;
        }

        bool Accept_Area_Entry_Check()
        {
            bool rtnVal = true;
            
            if (cboNewBi.Text.Trim() == "")
            {
                lblMsg.Text = "New 환자구분 Error !!";
                cboNewBi.Focus();
                return false;
            }
            else if (string.Compare(dtpDateT.Text, clsPublic.GstrSysDate) > 0)
            {
                lblMsg.Text = "To 취소일자 > 오늘 Error !!";
                cboRange.Focus();
                return false;
            }
            else if (cboRange.Text.Trim() == "")
            {
                lblMsg.Text = "범위 선택 !!";
                cboRange.Focus();
                return false;
            }
            else if (string.Compare(dtpDateF.Text, FstrInDate) < 0)
            {
                lblMsg.Text = "From 취소일자 < 입원일자 Error !!";
                dtpDateF.Focus();
                return false;
            }
            else if (string.Compare(dtpDateT.Text, dtpDateF.Text) < 0)
            {
                lblMsg.Text = "취소일자  From > To Error !!";
                dtpDateF.Focus();
                return false;
            }
            else if (txtJumin1.Text == "" || txtJumin1.Text.Length != 6)
            {
                lblMsg.Text = "주민번호 Check 요망!";
                txtJumin1.Focus();
                return false;
            }
            return rtnVal;
        }

        bool Accept_Area_Change_Check()
        {
            bool rtnVal = true;

            if (txtPName.Text.Trim() == "")
            {
                lblMsg.Text = "피보성명 입력 요망 !!";
                txtPName.Focus();
                return false;
            }
            else if (cboGwange.Text.Trim() == "")
            {
                lblMsg.Text = "피보관계 입력 요망 !!";
                cboGwange.Focus();
                return false;
            }
            else if ((cboNewBi.Text.Trim() == "12" || cboNewBi.Text.Trim() == "13") && txtGKiho.Text.Trim() == "")
            {
                lblMsg.Text = "증번호 입력 요망 !!";
                txtGKiho.Focus();
                return false;
            }

            return rtnVal;
        }

        void eSearch_Nhic(PsmhDb pDbCon)
        {
            string strJagek = string.Empty;
            string strKiho = string.Empty;
            string strGKiho = string.Empty;
            string strPname = string.Empty;

            ComFunc.ReadSysDate(pDbCon);

            if (FstrSName.Contains("A") || FstrSName.Contains("B") || FstrSName.Contains("C") || FstrSName.Contains("D"))
            {
                if (ComFunc.MsgBoxQ("성명에 A,B,C,D가 포함되어 있습니다. 성명수정 후 자격조회 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    FstrSName = VB.InputBox("성명을 다시 확인하십시오.", "이름확인", FstrSName);
                }
            }

            frmPmpaCheckNhic frm = new frmPmpaCheckNhic(FstrPano, "MD", FstrSName, FstrJumin1, FstrJumin2, clsPublic.GstrSysDate, "");
            frm.ShowDialog();

            if (clsPublic.GstrHelpCode != "")
            {
                strJagek = VB.Pstr(clsPublic.GstrHelpCode, ";", 2);
                strKiho  = VB.Pstr(clsPublic.GstrHelpCode, ";", 4);
                strGKiho = VB.Pstr(clsPublic.GstrHelpCode, ";", 5);
                strPname = VB.Pstr(clsPublic.GstrHelpCode, ";", 1);

                if (VB.Left(strJagek, 1) == "7" || VB.Left(strJagek, 1) == "8")
                {
                    txtKiho.Text = strKiho;
                    txtGKiho.Text = strGKiho;
                }

                if (strKiho == "") { strKiho = "00000000000"; }

                txtKiho.Text = strKiho;
                txtGKiho.Text = strGKiho;

                if (FstrSName == strPname)
                {
                    cboGwange.SelectedIndex = 0;
                }
                else
                {
                    cboGwange.SelectedIndex = 4;
                }
                    
            }
        }

        void eBtnCancel()
        {
          

            if (txtPano.Text.Trim() != "")
            {
                clsLockCheck.GstrLockPtno = txtPano.Text.Trim();
                clsLockCheck.IpdOcs_Lock_Delete_NEW(txtPano.Text.Trim());
                clsLockCheck.GstrLockPtno = "";
            }
            Screen_Clear();
            txtPano.Focus();
        }

        void Screen_Clear()
        {
            int i = 0;

            cboGwange.SelectedIndex = 0;
            cboNewBi.SelectedIndex = 0;
            cboRange.SelectedIndex = 0;

            cSpd.Spread_Clear(ssTrans, ssTrans.ActiveSheet.RowCount, ssTrans.ActiveSheet.ColumnCount);
            ssTrans.ActiveSheet.RowCount = 1;
            cSpd.Spread_Clear(ssTransFor, ssTransFor.ActiveSheet.RowCount, ssTransFor.ActiveSheet.ColumnCount);

            FstrDrg = "";

            txtPano.Text = "";      txtPName.Text = "";
            
            CF.dtpClear(dtpDateF);  CF.dtpClear(dtpDateT);
            
            txtKiho.Text = "";      txtGKiho.Text = "";
            txtJumin1.Text = "";    txtJumin2.Text = "";
            txtGel.Text = "";       lblGKihoMsg.Text = "";
            CF.dtpClear(dtpDate1);  CF.dtpClear(dtpDate2);  CF.dtpClear(dtpDate3);
            lblSName.Text = "";     lblNewBi.Text = "";     lblGel.Text = "";
            lblJKiho.Text = "조합기호";
            lblGKiho.Text = "증번호";
            txtGelCode.Text = "";

            CF.dtpClear(dtpBirth);

            chkilban2.Checked = false; //외국new
            chkDrg.Checked = false;    //DRG
            chkTax.Checked = false;    //부가세 2014-02-24
                
            FstrNewilban2 = "";     FstrOldilban2 = "";
            FstrPano = "";          FstrSName = "";
            FstrJumin1 = "";        FstrJumin2 = "";

            for (i = 0; i < ssAmt.ActiveSheet.RowCount; i++)
            {
                cSpd.Spread_Clear_Range(ssAmt, i, 1, 1, 2);
            }

            txtPano.Enabled = true;
            ssAmt.Enabled = false;
            grpBox.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            exPanSub02.Enabled = false;
            exPanSub03.Enabled = false;

        }
        
        bool Screen_Display(PsmhDb pDbCon, FpSpread Spd, string ArgPano)
        {
            DataTable dt = null;
            bool rtnVal = true;
            int i = 0, nRow = 0, nREAD = 0;

            ComFunc.ReadSysDate(pDbCon);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                dt = cPF.sel_IpdMst_BasPat(pDbCon, ArgPano);

                if (dt == null)
                {
                    ComFunc.MsgBox("재원마스타에 해당 환자가 없습니다.", "오류");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    #region 예외처리
                    if (dt.Rows[0]["GBSTS"].ToString().Trim() == "5")
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("심사가 완료된 환자입니다.", "오류");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    else if (dt.Rows[0]["GBSTS"].ToString().Trim() == "6")
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("퇴원계산서가 발부된 환자입니다.", "오류");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    else if (dt.Rows[0]["GBSTS"].ToString().Trim() == "7")
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("퇴원처리가 완료된 환자입니다.", "오류");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    else if (dt.Rows[0]["GBSTS"].ToString().Trim() == "1")
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("가퇴원 환자입니다.", "오류");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    #endregion

                    #region Screen Display
                    lblSName.Text =  dt.Rows[0]["SNAME"].ToString().Trim() + " ";
                    lblSName.Text += dt.Rows[0]["Sex"].ToString().Trim() + " / ";
                    lblSName.Text += dt.Rows[0]["Age"].ToString().Trim() + " (입원:";
                    lblSName.Text += dt.Rows[0]["InDate"].ToString().Trim() + ")";

                    FstrPano        = dt.Rows[0]["Pano"].ToString().Trim();      
                    FstrSName       = dt.Rows[0]["SNAME"].ToString().Trim();  
                    FnIPDNO         = Convert.ToInt64(dt.Rows[0]["IPDNO"].ToString());    
                    FstrInDate      = dt.Rows[0]["InDate"].ToString().Trim();    
                    FstrOutDate     = dt.Rows[0]["OutDate"].ToString().Trim();  
                    FstrInDate2     = dt.Rows[0]["InDate"].ToString().Trim();   
                    FstrOutDate2    = dt.Rows[0]["OutDate"].ToString().Trim();
                    FstrJumin1      = dt.Rows[0]["Jumin1"].ToString().Trim();
                    FstrJumin2      = clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());

                    txtJumin1.Text  = FstrJumin1;
                    txtJumin2.Text  = FstrJumin2;
                    txtPName.Text   = dt.Rows[0]["PName"].ToString().Trim();
                    
                    if (dt.Rows[0]["BIRTH"].ToString().Trim() == "")
                    {
                        CF.dtpClear(dtpBirth);
                    }
                    else
                    {
                        dtpBirth.Text = dt.Rows[0]["BIRTH"].ToString().Trim();
                    }

                    if (dt.Rows[0]["SName"].ToString().Trim() == dt.Rows[0]["PName"].ToString().Trim())
                    {
                        cboGwange.SelectedIndex = 0;
                    }
                    else
                    {
                        cboGwange.SelectedIndex = 4;
                    }

                    clsPmpaType.TIT.GbGameK = dt.Rows[0]["GbGamek"].ToString().Trim(); 
                    txtGelCode.Text         = dt.Rows[0]["GelCode"].ToString().Trim();                     
                    #endregion
                }

                dt.Dispose();
                dt = null;
                

                cSpd.Spread_Clear(Spd, Spd.ActiveSheet.RowCount, Spd.ActiveSheet.ColumnCount);
                Spd.ActiveSheet.RowCount = 0;
                
                //-------------------------------------------------
                //  IPD_TRANS을 읽어 Sheet에 표시함
                //-------------------------------------------------
                dt = cPF.Get_Ipd_Mst_Trs(pDbCon, ArgPano, 0, "");
                if (dt == null)
                {
                    ComFunc.MsgBox("환자자격정보 조회 오류", "오류");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    #region Spread Display Show
                    nREAD = dt.Rows.Count;

                    for (i = 0; i < nREAD; i++)
                    {
                        nRow += 1;
                        if (Spd.ActiveSheet.RowCount < nRow)
                        {
                            Spd.ActiveSheet.RowCount = nRow;
                        }

                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.chk01].Text     = "";
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.INDATE].Text    = dt.Rows[i]["INDATE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.BI].Text        = dt.Rows[i]["BI"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.DEPT].Text      = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.GBIPD].Text     = dt.Rows[i]["GBIPD"].ToString().Trim() == "9" ? "지병" : "";
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.KIHO].Text      = dt.Rows[i]["KIHO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.BONRATE].Text   = dt.Rows[i]["BONRATE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.GISULRATE].Text = dt.Rows[i]["GISULRATE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.TRSNO].Text     = dt.Rows[i]["TRSNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.OUTDATE].Text   = dt.Rows[i]["OUTDATE"].ToString().Trim() == "" ? clsPublic.GstrSysDate : dt.Rows[0]["OUTDATE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.GBSTS].Text     = dt.Rows[i]["TGBSTS"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.RDATE].Text     = dt.Rows[i]["RDATE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.GELCODE].Text   = dt.Rows[i]["GELCODE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.DRCODE].Text    = dt.Rows[i]["DRCODE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.DRNAME].Text    = clsVbfunc.GetBASDoctorName(pDbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.GKIHO].Text     = dt.Rows[i]["GKIHO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.GBILBAN2].Text  = dt.Rows[i]["GBILBAN2"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.GWANGE].Text    = dt.Rows[i]["GWANGE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.PNAME].Text     = dt.Rows[i]["PNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.BOHUN].Text     = dt.Rows[i]["BOHUN"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.GBGAMEK].Text   = dt.Rows[i]["GBGAMEK"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmIpdTrsChg.GBDRG].Text     = dt.Rows[i]["GBDRG"].ToString().Trim() == "D" ? "D" : "";
                    }
                    #endregion
                }

                dt.Dispose();
                dt = null;

                READ_HISTORY(pDbCon, ssTransFor);       //전실전과 내역 Display

                btnSave.Enabled     = true;
                btnCancel.Enabled   = true;
                exPanSub02.Enabled  = true;
                exPanSub03.Enabled  = true;
                txtPano.Enabled     = false;

                Cursor.Current = Cursors.Default;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }
        
        void READ_HISTORY(PsmhDb pDbCon, FpSpread Spd)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int i = 0;
            int nRow = 0;
            string strOldData = string.Empty;
            string strNewData = string.Empty;
            string strViewData = string.Empty;

            cSpd.Spread_Clear(Spd, Spd.ActiveSheet.RowCount, Spd.ActiveSheet.ColumnCount);
            Spd.ActiveSheet.RowCount = 0;
            
            try
            {
                #region READ_BM
                SQL = "";
                SQL += " SELECT TO_CHAR(JobDate,'YYYY-MM-DD') JobDate, Bi   \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_BM                \r\n";
                SQL += "  WHERE IPDNO = " + FnIPDNO + "                     \r\n";
                SQL += "    AND GbBackUp = 'J'                              \r\n";
                SQL += "  ORDER BY JobDate,Bi                                   ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return;
                }

                nRow = 0;

                if (dt.Rows.Count > 0)
                {
                    strOldData = dt.Rows[0]["BI"].ToString().Trim();

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strNewData = dt.Rows[i]["BI"].ToString().Trim();

                        if (strOldData != strNewData)
                        {
                            nRow += 1;
                            if (Spd.ActiveSheet.RowCount < nRow)
                            {
                                Spd.ActiveSheet.RowCount = nRow;
                            }
                            Spd.ActiveSheet.Cells[nRow - 1, 0].Text = nRow.ToString();
                            Spd.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["JobDate"].ToString().Trim();

                            strViewData = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", strOldData) + "->";
                            strViewData += CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", strNewData);

                            Spd.ActiveSheet.Cells[nRow - 1, 2].Text = strViewData;

                            strOldData = strNewData;
                        }
                        
                    }
                }

                dt.Dispose();
                dt = null;
                #endregion
                
                #region READ_TRANSFOR_Dept
                SQL = "";
                SQL += " SELECT FrDept, FRDOCTOR,  FrRoom, ToDept, TODOCTOR, ToRoom,                \r\n";
                SQL += "        TO_CHAR(TrsDate,'YYYY-MM-DD') TrsDate1                              \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_TRANSFOR                                  \r\n";
                SQL += "  WHERE Pano = '" + FstrPano + "'                                           \r\n";
                SQL += "    AND TO_CHAR(TrsDate,'YYYY-MM-DD') >= '" + VB.Left(FstrInDate2, 10) + "' \r\n";
                if (FstrOutDate2 != "")
                {
                    SQL += "    AND TO_CHAR(TrsDate,'YYYY-MM-DD') <= '" + FstrOutDate2 + "'         \r\n";
                }
                SQL += "    AND FrDept <> ToDept                                                    \r\n";
                SQL += "  ORDER BY TrsDate                                                              ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return;
                }

                nRow = 0;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow += 1;
                        if (Spd.ActiveSheet.RowCount < nRow)
                        {
                            Spd.ActiveSheet.RowCount = nRow;
                        }

                        Spd.ActiveSheet.Cells[nRow - 1, 3].Text = nRow.ToString();
                        Spd.ActiveSheet.Cells[nRow - 1, 4].Text = dt.Rows[i]["TrsDate1"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, 5].Text = dt.Rows[i]["FrDept"].ToString().Trim() + "(" + clsVbfunc.GetBASDoctorName(pDbCon, dt.Rows[i]["FRDOCTOR"].ToString().Trim()) + ")->";
                        Spd.ActiveSheet.Cells[nRow - 1, 5].Text += dt.Rows[i]["ToDept"].ToString().Trim() + "(" + clsVbfunc.GetBASDoctorName(pDbCon, dt.Rows[i]["TODOCTOR"].ToString().Trim()) + ")";
                    }
                }
                
                dt.Dispose();
                dt = null;
                #endregion

                #region READ_TRANSFOR_Room
                SQL = "";
                SQL += " SELECT FrDept, FrRoom, ToDept, ToRoom,                                     \r\n";
                SQL += "        TO_CHAR(TrsDate,'YYYY-MM-DD') TrsDate1                              \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_TRANSFOR                                  \r\n";
                SQL += "  WHERE Pano = '" + FstrPano + "'                                           \r\n";
                SQL += "    AND TO_CHAR(TrsDate,'YYYY-MM-DD') >= '" + VB.Left(FstrInDate2, 10) + "' \r\n";
                if (FstrOutDate2 != "")
                {
                    SQL += "    AND TO_CHAR(TrsDate,'YYYY-MM-DD') <= '" + FstrOutDate2 + "'         \r\n";
                }
                SQL += "    AND FrRoom <> ToRoom                                                    \r\n";
                SQL += "  ORDER BY TrsDate                                                              ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return;
                }

                nRow = 0;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow += 1;
                        if (Spd.ActiveSheet.RowCount < nRow)
                        {
                            Spd.ActiveSheet.RowCount = nRow;
                        }

                        Spd.ActiveSheet.Cells[nRow - 1, 6].Text = nRow.ToString();
                        Spd.ActiveSheet.Cells[nRow - 1, 7].Text = dt.Rows[i]["TrsDate1"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, 8].Text = dt.Rows[i]["FrRoom"].ToString().Trim() + " -> " + dt.Rows[i]["ToRoom"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                #endregion

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return;
            }
            
        }
        
        /// <summary>
        /// ssAmt Display
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="Spd"></param>
        /// <param name="argGBN">1.변경전 2.변경후 3.New금액</param>
        void IPD_TRANS_Amt_View(PsmhDb pDbCon, FpSpread Spd, string argGBN)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int i = 0, j = 0, nRead = 0;
            long[] nAmt = new long[61];
            string strIPDNO = string.Empty;
            
            if (argGBN == "1" || argGBN == "2")
            {
                for (i = 1; i <= FnOldTrsCNT; i++)
                {
                    strIPDNO += FnOldTransNo[i] + ",";
                }
                
                strIPDNO = VB.Left(strIPDNO, strIPDNO.Length - 1);
            }
            else
            {
                strIPDNO = FnNewTRSNO.ToString();
            }
            
            //누적별 금액을 합산함
            SQL = "";
            SQL += " SELECT Bi,TO_CHAR(InDate, 'YYYY-MM-DD') InDate,                        \r\n";
            SQL += "        Amt01,Amt02,Amt03,Amt04,Amt05,Amt06,Amt07,Amt08,Amt09,Amt10,    \r\n";
            SQL += "        Amt11,Amt12,Amt13,Amt14,Amt15,Amt16,Amt17,Amt18,Amt19,Amt20,    \r\n";
            SQL += "        Amt21,Amt22,Amt23,Amt24,Amt25,Amt26,Amt27,Amt28,Amt29,Amt30,    \r\n";
            SQL += "        Amt31,Amt32,Amt33,Amt34,Amt35,Amt36,Amt37,Amt38,Amt39,Amt40,    \r\n";
            SQL += "        Amt41,Amt42,Amt43,Amt44,Amt45,Amt46,Amt47,Amt48,Amt49,Amt50,    \r\n";
            SQL += "        Amt51,Amt52,Amt53,Amt54,Amt55,Amt56,Amt57,Amt58,Amt59,Amt60     \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "IPD_TRANS                                 \r\n";
            SQL += "  WHERE TRSNO IN (" + strIPDNO + ")                                     \r\n";
            SQL += "  ORDER BY InDate,Bi                                                        ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    for (j = 1; j < 61; j++)
                    {
                        nAmt[j] += Convert.ToInt32(VB.Val(dt.Rows[i]["AMT" + VB.Format(j, "00")].ToString()));
                    }
                }
            }

            dt.Dispose();
            dt = null;

            //금액을 Display
            for (i = 1; i < 59; i++)
            {
                Spd.ActiveSheet.Cells[i, Convert.ToInt16(VB.Val(argGBN))].Text = nAmt[i].ToString();
            }
            
        }

        bool New_IPD_Trans_INSERT(PsmhDb pDbCon, long ArgIpdNo, long ArgNewTransNo, string ArgGbilban2)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            bool rtnVal = true;

            int i = 0;
            int nIlsu = 0;
            int nPDilsu = 0;

            string strInDate = string.Empty;
            string strOutDate = string.Empty;
            string strOGPDBun = string.Empty;
            string strOGPDBundtl = string.Empty;

            //--------------------------------------------------
            //   새로운 마스타에 시작,종료일,재원일수 설정
            //--------------------------------------------------
            if (VB.Left(cboRange.Text, 1) == "1")      //재원기간 전체
            { 
                strInDate = FstrInDate;
                strOutDate = FstrOutDate;
                nIlsu = clsPmpaType.TIT.M_Ilsu;
            }
            else if (VB.Left(cboRange.Text, 1) == "2")  //앞부분
            { 
                strInDate = FstrInDate;
                strOutDate = dtpDateT.Text.Trim();
                nIlsu = CF.DATE_ILSU(pDbCon, strOutDate, strInDate);
            }
            else //재원기간 뒷부분
            { 
                strInDate = dtpDateF.Text.Trim();
                strOutDate = FstrOutDate;
                if (strOutDate == "")
                { 
                    nIlsu = CF.DATE_ILSU(pDbCon, clsPublic.GstrSysDate, strInDate);
                }
                else
                { 
                    nIlsu = CF.DATE_ILSU(pDbCon, strOutDate, strInDate);
                }
            }

            clsPmpaType.TIT.Ipdno       = ArgIpdNo;
            clsPmpaType.TIT.Trsno       = ArgNewTransNo;
            clsPmpaType.TIT.Pano        = txtPano.Text.Trim();
            clsPmpaType.TIT.Bi          = cboNewBi.Text.Trim();
            clsPmpaType.TIT.InDate      = strInDate;
            clsPmpaType.TIT.OutDate     = strOutDate;
            clsPmpaType.TIT.ActDate     = "";
            clsPmpaType.TIT.Ilsu        = nIlsu;
            clsPmpaType.TIT.Kiho        = txtKiho.Text.Trim();
            clsPmpaType.TIT.GKiho       = txtGKiho.Text.Trim();
            clsPmpaType.TIT.PName       = txtPName.Text.Trim();
            clsPmpaType.TIT.Gwange      = VB.Left(cboGwange.Text.Trim(), 1);
            clsPmpaType.TIT.BonRate     = cPF.READ_IpdBon_Rate_CHK(pDbCon, clsPmpaType.TIT.Bi, clsPmpaType.TIT.InDate);  //본인부담율
            clsPmpaType.TIT.GISULRATE   = cPF.READ_Gisul_Rate(pDbCon, clsPmpaType.TIT.Bi, clsPmpaType.TIT.InDate);  //기술료 가산
            clsPmpaType.TIT.GelCode     = txtGelCode.Text.Trim();
            clsPmpaType.TIT.Amset1      = "0";
            clsPmpaType.TIT.AmSet5      = "0";
            
            if (chkDrg.Checked == true) { clsPmpaType.TIT.GbDRG = "D"; }
            if (chkTax.Checked == true) { clsPmpaType.TIT.GbTax = "1"; }

            for (i = 1; i < 66; i++)
            {
                clsPmpaType.TIT.Amt[i] = 0;
            }

            switch (cboNewBi.Text.Trim())
            {
                case "11":
                case "12":
                case "13":
                    //차상위2종 - 만성질환,장애인 만성질환, 17세미만
                    if (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F" || clsPmpaType.TIT.OgPdBun == "J" || clsPmpaType.TIT.OgPdBun == "K")
                    {
                        strOGPDBun = clsPmpaType.TIT.OgPdBun;
                        if (clsPmpaType.TIT.Age == 0)
                        {
                            nPDilsu = CF.DATE_ILSU(pDbCon, dtpDateF.Text, dtpBirth.Text) + 1;  //계산에서 1을 더해줌 심사과 심경순 2009-03-23
                            if (nPDilsu <= 28)   //2010-05-26 심사과의뢰
                            {
                                strOGPDBundtl = "P";
                                clsPmpaType.TIT.BonRate = 0;
                            }
                            else if (clsPmpaType.TIT.Age >= 6 && clsPmpaType.TIT.Age <= 15 && string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0)  //add  2017-10-01 add F018통일
                            { 
                                strOGPDBundtl = "Y";
                                clsPmpaType.TIT.BonRate = 3;
                            }
                            else
                            {
                                strOGPDBundtl = "S";
                                clsPmpaType.TIT.BonRate = 10;
                            }
                        }
                    }
                    else
                    { 
                        if (clsPmpaType.TIT.Age >= 1 && clsPmpaType.TIT.Age <= 5 && string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0)  //add  2017-10-01 add F018통일
                        {
                            strOGPDBun = "S";
                            clsPmpaType.TIT.BonRate = 5;
                        }
                        else if (clsPmpaType.TIT.Age >= 6 && clsPmpaType.TIT.Age <= 15 && string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0)  //add  2017-10-01 add F018통일
                        {
                            strOGPDBun = "Y";
                            strOGPDBundtl = "Y";
                            clsPmpaType.TIT.BonRate = 5;
                        }
                        else if (clsPmpaType.TIT.Age == 0 && string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0)
                        {
                            nPDilsu = CF.DATE_ILSU(pDbCon, dtpDateF.Text, dtpBirth.Text) + 1;  //계산에서 1을 더해줌 심사과 심경순 2009-03-23
                            if (nPDilsu <= 28)   //2010-05-26 심사과의뢰
                            {
                                strOGPDBun = "P";
                                clsPmpaType.TIT.BonRate = 0;
                            }
                            else
                            {
                                strOGPDBun = "S";
                                clsPmpaType.TIT.BonRate = 5;
                            }
                        }
                    }
                    break;
                case "21":
                case "22":
                    if (clsPmpaType.TIT.Age <= 5) { strOGPDBun = "P"; }
                    
                    if (clsPmpaType.TIT.Age > 5 && clsPmpaType.TIT.Age <= 15 && string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0) //2017-10-01 add F019통일
                    {
                        strOGPDBun = "Y";
                        if (cboNewBi.Text.Trim() == "22") { clsPmpaType.TIT.BonRate = 3; }
                    }
                    break;
                default:
                    strOGPDBun = "";
                    strOGPDBundtl = "";
                    break;
            }

            try
            {

                SQL = "";
                SQL += "INSERT INTO " + ComNum.DB_PMPA + "IPD_TRANS (                                                       \r\n";
                SQL += "       TrsNo,IPDNO,PANO,GBIPD,INDATE,OUTDATE,ActDate,DEPTCODE,DRCODE,ILSU,BI,KIHO,                  \r\n";
                SQL += "       GKIHO,PNAME,GWANGE,BONRATE,GISULRATE,GBGAMEK,BOHUN,AMSET1,AMSET2,AMSET3,AMSET4,              \r\n";
                SQL += "       AMSET5,AMSETB,FROMTRANS,ERAMT,REMARK,JUPBONO,DRGWRTNO,SANGAMT,DTGAMEK,                       \r\n";
                SQL += "       OGPDBUN,OGPDBUNdtl,AMT01,AMT02,AMT03,AMT04,AMT05,AMT06,AMT07,AMT08,AMT09,AMT10,AMT11,        \r\n";
                SQL += "       AMT12,AMT13,AMT14,AMT15,AMT16,AMT17,AMT18,AMT19,AMT20,AMT21,AMT22,AMT23,AMT24,AMT25,         \r\n";
                SQL += "       AMT26,AMT27,AMT28,AMT29,AMT30,AMT31,AMT32,AMT33,AMT34,AMT35,AMT36,AMT37,AMT38,AMT39,         \r\n";
                SQL += "       AMT40,AMT41,AMT42,AMT43,AMT44,AMT45,AMT46,AMT47,AMT48,AMT49,AMT50,AMT51,AMT52,AMT53,         \r\n";
                SQL += "       AMT54,AMT55,AMT56,AMT57,AMT58,AMT59,AMT60,AMT64,                                             \r\n";

                if (VB.Left(cboRange.Text, 1) == "1")
                    SQL += "            JSIM_SABUN, JSIM_LDATE, JSIM_Set,                                                   \r\n";

                SQL += "       ENTDATE,ENTSABUN,GBSTS,GELCODE,Gbilban2,GbSPC,                                               \r\n";
                SQL += "       GBDRG,DRGCODE,DRGADC1,DRGADC2,DRGADC3,DRGADC4,DRGADC5,AMT65,GbTax) VALUES (                  \r\n";
                SQL += "        " + clsPmpaType.TIT.Trsno + "," + clsPmpaType.TIT.Ipdno + ",'" + clsPmpaType.TIT.Pano + "', \r\n";
                SQL += "       '" + clsPmpaType.TIT.GbIpd + "',                                                             \r\n";
                SQL += "       TO_DATE('" + strInDate + "','YYYY-MM-DD'), TO_DATE('" + strOutDate + "','YYYY-MM-DD'),'',    \r\n";
                SQL += "       '" + clsPmpaType.TIT.DeptCode + "','" + clsPmpaType.TIT.DrCode + "',                         \r\n";
                SQL += "        " + clsPmpaType.TIT.Ilsu + ",'" + clsPmpaType.TIT.Bi + "',                                  \r\n";
                SQL += "       '" + clsPmpaType.TIT.Kiho + "','" + clsPmpaType.TIT.GKiho + "',                              \r\n";
                SQL += "       '" + clsPmpaType.TIT.PName + "','" + clsPmpaType.TIT.Gwange + "',                            \r\n";
                SQL += "       '" + clsPmpaType.TIT.BonRate + "'," + clsPmpaType.TIT.GISULRATE + ",                         \r\n";
                SQL += "       '" + clsPmpaType.TIT.GbGameK + "','" + clsPmpaType.TIT.Bohun + "','0','0','0','0','0','0',   \r\n";
                SQL += "       '','','','','','','','" + strOGPDBun + "','" + strOGPDBundtl + "',                           \r\n";
                SQL += "       '0','0','0','0','0','0','0','0','0','0',                                                     \r\n";
                SQL += "       '0','0','0','0','0','0','0','0','0','0',                                                     \r\n";
                SQL += "       '0','0','0','0','0','0','0','0','0','0',                                                     \r\n";
                SQL += "       '0','0','0','0','0','0','0','0','0','0',                                                     \r\n";
                SQL += "       '0','0','0','0','0','0','0','0','0','0',                                                     \r\n";
                SQL += "       '0','0','0','0','0','0','0','0','0','0',0,                                                   \r\n";

                if(VB.Left(cboRange.Text, 1) == "1")
                    SQL += "   '" + clsPmpaType.TIT.JSIM_SABUN + "',TO_DATE('" + clsPmpaType.TIT.JSIM_LDATE + "','YYYY-MM-DD'), '" + clsPmpaType.TIT.JSIM_Set + "',    \r\n";

                SQL += "       SYSDATE," + clsType.User.IdNumber + ",'0', '" + clsPmpaType.TIT.GelCode + "',                \r\n";
                SQL += "       '" + ArgGbilban2 + "','',                                                                    \r\n";
                SQL += "       '" + clsPmpaType.TIT.GbDRG + "','" + clsPmpaType.TIT.DrgCode + "',                           \r\n";
                SQL += "       '" + clsPmpaType.TIT.DRGADC1 + "','" + clsPmpaType.TIT.DRGADC2 + "',                         \r\n";
                SQL += "       '" + clsPmpaType.TIT.DRGADC3 + "','" + clsPmpaType.TIT.DRGADC4 + "',                         \r\n";
                SQL += "       '" + clsPmpaType.TIT.DRGADC5 + "', 0, '" + clsPmpaType.TIT.GbTax + "')                           ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }
                
                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        bool New_DRG_MASTER_INSERT(PsmhDb pDbCon, long ArgIpdNo, long ArgOldTransNo, long ArgNewTransNo)
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            string strRowid = string.Empty;
            bool rtnVal = true;

            try
            {
                SQL = "";
                SQL += " SELECT ROWID FROM " + ComNum.DB_PMPA + "DRG_MASTER_NEW ";
                SQL += "  WHERE IPDNO =" + ArgIpdNo + " ";
                SQL += "    AND TRSNO =" + ArgOldTransNo + " ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    strRowid = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;


                SQL = "";
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "DRG_MASTER_NEW (                                                 \r\n";
                SQL += "        TRSNO,IPDNO,PANO,SNAME,JINDATE,ILSU,ILLCODE1,ILLCODE2,ILLCODE3,ILLCODE4,                    \r\n";
                SQL += "        ILLCODE5,ILLCODE6,ILLCODE7,ILLCODE8,ILLCODE9,ILLCODE10,OPCODE1,OPCODE2,OPCODE3,             \r\n";
                SQL += "        OPCODE4,OPCODE5,OPCODE6,OPCODE7,OPCODE8,OPCODE9,OPCODE10,EXCODE1,EXCODE2,EXCODE3,           \r\n";
                SQL += "        EXCODE4,EXCODE5,XRCODE1,XRCODE2,XRCODE3,XRCODE4,XRCODE5,JCODE1,JCODE2,JCODE3,               \r\n";
                SQL += "        JCODE4,JCODE5,MCODE1,MCODE2,MCODE3,MCODE4,MCODE5,BCODE1,BCODE2,BCODE3,BCODE4,               \r\n";
                SQL += "        BCODE5,ACODE,WEIGHT,CPR,MDCCODE,DRGCODE,MDCCODEOLD,DRGCODEOLD,ENTDATE1,ENTDATE2,            \r\n";
                SQL += "        ENTSABUN1,ENTSABUN2)                                                                        \r\n";
                SQL += " SELECT " + ArgNewTransNo + ",IPDNO,PANO,SNAME,JINDATE,ILSU,ILLCODE1,ILLCODE2,ILLCODE3,ILLCODE4,    \r\n";
                SQL += "        ILLCODE5,ILLCODE6,ILLCODE7,ILLCODE8,ILLCODE9,ILLCODE10,OPCODE1,OPCODE2,OPCODE3,             \r\n";
                SQL += "        OPCODE4,OPCODE5,OPCODE6,OPCODE7,OPCODE8,OPCODE9,OPCODE10,EXCODE1,EXCODE2,EXCODE3,           \r\n";
                SQL += "        EXCODE4,EXCODE5,XRCODE1,XRCODE2,XRCODE3,XRCODE4,XRCODE5,JCODE1,JCODE2,JCODE3,               \r\n";
                SQL += "        JCODE4,JCODE5,MCODE1,MCODE2,MCODE3,MCODE4,MCODE5,BCODE1,BCODE2,BCODE3,BCODE4,               \r\n";
                SQL += "        BCODE5,ACODE,WEIGHT,CPR,MDCCODE,DRGCODE,MDCCODEOLD,DRGCODEOLD,ENTDATE1,ENTDATE2,            \r\n";
                SQL += "        ENTSABUN1 , ENTSABUN2                                                                       \r\n";
                SQL += "   From " + ComNum.DB_PMPA + "DRG_MASTER_NEW                                                        \r\n";
                SQL += "  WHERE ROWID = '" + strRowid + "'                                                                  \r\n";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        bool Return_Process(PsmhDb pDbCon, long ArgOldTRSNO, string ArgLange, string ArgFDay, string ArgTDay)
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            int i = 0, nREAD = 0;
            int nRetNal = 0;
            int nIlsu = 0;
            long nRetAmt1 = 0, nRetAmt2 = 0;
            string strRowid = string.Empty;
            string strRetNu = string.Empty;
            string strInDate = string.Empty;
            string strOutDate = string.Empty;
            bool rtnVal = true;

            //--------------------------------------------------------------------
            //  TRSNO별도 IPD_NEW_SLIP을 읽어 취소처방을    INSERT
            //                            새로운 자격에 INSERT
            //--------------------------------------------------------------------

            try
            {
                SQL = "";
                SQL += " SELECT IPDNO,TrsNo,TO_CHAR(BDATE,'YYYY-MM-DD') BDate,PANO,BI,SuNext,           \r\n";
                SQL += "        BUN,NU,QTY,BASEAMT,GBSPC,GBNGT,GBGISUL,GBSELF,                          \r\n";
                SQL += "        GBCHILD,DEPTCODE,DRCODE,WARDCODE,SUCODE,GBSLIP,                         \r\n";
                SQL += "        GBHOST,SEQNO,YYMM, DRGSELF, ORDERNO,                                    \r\n";
                SQL += "        SUM(NAL) Nal,SUM(Amt1) Amt1,SUM(Amt2) Amt2,                             \r\n";
                SQL += "        ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,                        \r\n";
                SQL += "        EXAM_WRTNO,RoomCode,DIV,GBSELNOT,GBSUGBS,GBER ,GBSGADD , POWDER,BCODE          \r\n"; //2012-11-15 part 추가
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                      \r\n";
                SQL += "  WHERE TrsNo = " + ArgOldTRSNO + "                                             \r\n";
                if (ArgLange == "2")  //앞부분
                {
                    SQL += " AND Bdate <= TO_DATE('" + ArgTDay + "','YYYY-MM-DD')                       \r\n";
                }
                else if (ArgLange == "3")  //뒷부분
                {
                    SQL += "    AND Bdate >= TO_DATE('" + ArgFDay + "','YYYY-MM-DD')                    \r\n";
                    if (ArgTDay != "")
                    {
                        SQL += "    AND Bdate <= TO_DATE('" + ArgTDay + "','YYYY-MM-DD')                \r\n";
                    }
                }
                SQL += "  GROUP BY IPDNO,TrsNo,Pano,Bi,BDate,SuNext,Bun,Nu,Qty,                         \r\n";
                SQL += "           BASEAMT,GBSPC,GBNGT,GBGISUL,GBSELF,GBCHILD,DEPTCODE,                 \r\n";
                SQL += "           DRCODE,WARDCODE,SUCODE,GBSLIP,GBHOST,SEQNO,YYMM,DRGSELF,ORDERNO,     \r\n";
                SQL += "           ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,EXAM_WRTNO,          \r\n";
                SQL += "           RoomCode,DIV,GBSELNOT,GBSUGBS,GBER ,GBSGADD ,POWDER, BCODE                  \r\n";
                SQL += "  ORDER BY Bdate, Bun                                                           ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }

                nREAD = dt.Rows.Count;

                if (nREAD > 0)
                {
                    for (i = 0; i < nREAD; i++)
                    {
                        nRetNal = Convert.ToInt32(VB.Val(dt.Rows[i]["NAL"].ToString())); 
                        nRetAmt1 = Convert.ToInt64(VB.Val(dt.Rows[i]["AMT1"].ToString())); 
                        nRetAmt2 = Convert.ToInt64(VB.Val(dt.Rows[i]["AMT2"].ToString()));
                        strRetNu = dt.Rows[i]["Nu"].ToString().Trim();

                        if (nRetNal != 0 || nRetAmt1 != 0 || nRetAmt2 != 0)
                        {
                            nRetNal = nRetNal * -1;
                            nRetAmt1 = nRetAmt1 * -1;
                            nRetAmt2 = nRetAmt2 * -1;

                            //----------------------------------------
                            //       취소 SLIP을 INSERT
                            //----------------------------------------
                            SQL = "";
                            SQL += " INSERT INTO " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                                         ";
                            SQL += "        (IPDNO,TrsNo,ActDate, Pano, Bi, Bdate, EntDate, Sunext, Bun,                                    ";
                            SQL += "        Nu, Qty, Nal,  BaseAmt, GbSpc, GbNgt, GbGisul, GbSelf,                                          ";
                            SQL += "        GbChild, DeptCode, DrCode, WardCode, Sucode, GbSlip,                                            ";
                            SQL += "        GbHost, Part, Amt1, Amt2, SeqNo, yymm, DRGSELF, ORDERNO,                                        ";
                            SQL += "        ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,EXAM_WRTNO,RoomCode,DIV,GBSELNOT,GBSUGBS,GBSGADD ,POWDER, BCODE,GBER)  ";
                            SQL += " VALUES (                                                                                               ";
                            SQL += "        " + VB.Val(dt.Rows[i]["IPDNO"].ToString()) + ",                                                 ";
                            SQL += "        " + VB.Val(dt.Rows[i]["TrsNo"].ToString()) + ",                                                 ";
                            SQL += "        TO_DATE('" + clsPmpaPb.GstrActDate + "','YYYY-MM-DD'),'" + clsPmpaType.TIT.Pano + "',           ";
                            SQL += "        '" + dt.Rows[i]["Bi"].ToString() + "',                                                          ";
                            SQL += "        TO_DATE('" + dt.Rows[i]["Bdate"].ToString() + "','YYYY-MM-DD'),                                 ";
                            SQL += "        SYSDATE,                                                                                        ";
                            SQL += "        '" + dt.Rows[i]["Sunext"].ToString() + "',  '" + dt.Rows[i]["Bun"].ToString() + "',             ";
                            SQL += "        '" + dt.Rows[i]["Nu"].ToString() + "',                                                          ";
                            SQL += "         " + VB.Val(dt.Rows[i]["Qty"].ToString()) + ",     '" + nRetNal + "',                           ";
                            SQL += "         " + VB.Val(dt.Rows[i]["BaseAmt"].ToString()) + ", '" + dt.Rows[i]["GbSpc"].ToString() + "',    ";
                            SQL += "        '" + dt.Rows[i]["GbNgt"].ToString() + "',   '" + dt.Rows[i]["GbGisul"].ToString() + "',         ";
                            SQL += "        '" + dt.Rows[i]["GbSelf"].ToString() + "',  '" + dt.Rows[i]["GbChild"].ToString() + "',         ";
                            SQL += "        '" + dt.Rows[i]["DeptCode"].ToString() + "','" + dt.Rows[i]["DrCode"].ToString() + "',          ";
                            SQL += "        '" + dt.Rows[i]["WardCode"].ToString() + "','" + dt.Rows[i]["Sucode"].ToString() + "',          ";
                            SQL += "        '" + dt.Rows[i]["GbSlip"].ToString() + "', '" + dt.Rows[i]["GbHost"].ToString() + "', '-',     ";
                            SQL += "         " + nRetAmt1 + ", " + nRetAmt2 + ", '" + VB.Val(dt.Rows[i]["SeqNo"].ToString()) + "',          ";
                            SQL += "        '" + dt.Rows[i]["YYMM"].ToString() + "' ,'" + dt.Rows[i]["DRGSELF"].ToString() + "',            ";
                            SQL += "        '" + VB.Val(dt.Rows[i]["ORDERNO"].ToString()) + "',                                             ";
                            SQL += "        '" + dt.Rows[i]["ABCDATE"].ToString() + "',                                                     ";
                            SQL += "        '" + dt.Rows[i]["OPER_DEPT"].ToString() + "',                                                   ";
                            SQL += "        '" + dt.Rows[i]["OPER_DCT"].ToString() + "',                                                    ";
                            SQL += "        '" + dt.Rows[i]["ORDER_DEPT"].ToString() + "',                                                  ";
                            SQL += "        '" + dt.Rows[i]["ORDER_DCT"].ToString() + "',                                                   ";
                            SQL += "        '" + VB.Val(dt.Rows[i]["EXAM_WRTNO"].ToString()) + "',                                          ";
                            SQL += "        '" + VB.Val(dt.Rows[i]["RoomCode"].ToString()) + "',                                            ";
                            SQL += "         " + VB.Val(dt.Rows[i]["DIV"].ToString()) + ",                                                  ";
                            SQL += "        '" + dt.Rows[i]["GBSELNOT"].ToString().Trim() + "',                                             ";
                            SQL += "        '" + dt.Rows[i]["GBSUGBS"].ToString() + "',                                                     ";
                            SQL += "        '" + dt.Rows[i]["GBSGADD"].ToString() + "',                                                     ";
                            SQL += "        '" + dt.Rows[i]["POWDER"].ToString() + "',                                                     ";
                            SQL += "        '" + dt.Rows[i]["BCODE"].ToString() + "',                                                     ";
                            SQL += "        '" + dt.Rows[i]["GBER"].ToString().Trim() + "'                                                  ";
                            SQL += "          )                                                                                             ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                return false;
                            }

                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //----------------------------------------------------------
                //  IPD_TRANS의 시작일,종료일,일수를 변경
                //----------------------------------------------------------
                if (ArgLange == "2")  //재원기간 앞부분
                {
                    strInDate = CF.DATE_ADD(pDbCon, ArgTDay, 1);
                    strOutDate = clsPmpaType.TIT.OutDate;
                    if (strOutDate == "")
                    {
                        nIlsu = CF.DATE_ILSU(pDbCon, clsPublic.GstrSysDate, strInDate);
                    }
                    else
                    {
                        nIlsu = CF.DATE_ILSU(pDbCon, strOutDate, strInDate);
                    }
                    if (nIlsu > 999) { nIlsu = 900; }
                }
                else if (ArgLange == "3") //재원기간 뒷부분
                {
                    strInDate = clsPmpaType.TIT.InDate;
                    strOutDate = CF.DATE_ADD(pDbCon, ArgFDay, -1);
                    nIlsu = CF.DATE_ILSU(pDbCon, strOutDate, strInDate);
                    if (nIlsu > 999) { nIlsu = 800; }
                }

                //IPD_TRANS에 UPDATE
                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET ";
                if (ArgLange == "1")  //전체
                {
                    SQL += " GbIPD='D', "; //삭제처리
                    SQL += " GbSTS='9', "; //삭제
                    SQL += " OutDate=TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'), ";
                    SQL += " ActDate=TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')  ";
                }
                else
                {
                    SQL += " InDate=TO_DATE('" + strInDate + "','YYYY-MM-DD'), ";
                    SQL += " OutDate=TO_DATE('" + strOutDate + "','YYYY-MM-DD'), ";
                    SQL += " Ilsu=" + nIlsu + " ";
                }
                SQL += "  WHERE TRSNO = " + ArgOldTRSNO + " ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }
                
                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }
    }
}
