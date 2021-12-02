using ComBase;
using ComDbB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

//namespace ComPmpaLibB.form.PmpaView
namespace ComPmpaLibB
{
    public partial class frmPmpaMisuMast : Form
    {
        #region 클래스 선언 및 etc....
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPublic cpublic = new clsPublic();
        clsComPmpaSQL cSQL = new clsComPmpaSQL();
        clsComPmpaSpd MagamSpd = new clsComPmpaSpd();
        clsPmpaMisu cPM = new clsPmpaMisu();
        clsPmpaFunc cPF = new clsPmpaFunc();
        clsPmpaType cPT = new clsPmpaType();

        clsPmpaPb CPP = null;
        clsPmpaPb.cMisuMst cMisuMst = null;
        clsPmpaPb.GstrBunSu GstrBunSu = null;

        Card CC = new Card();
        clsOrdFunction OF = new clsOrdFunction();
        clsPmpaType.AcctReqData RSD = new clsPmpaType.AcctReqData();
        clsPmpaType.AcctResData RD = new clsPmpaType.AcctResData();
        string strRemark = "";// remark 임시저장용
        private frmPmpaViewGelSearch frmPmpaViewGelSearchX = null;
        #endregion

        #region //MainFormMessage
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion
         
        public frmPmpaMisuMast()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaMisuMast(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        void setCtrlData()
        {
            setCombo();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);            

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnOK.Click += new EventHandler(eBtnClick);
            this.btnNext.Click += new EventHandler(eBtnClick);
            this.btnDel.Click += new EventHandler(eBtnClick);
            this.btnHelp.Click += new EventHandler(eBtnClick);

            this.Menu1_1.Click += new EventHandler(eMenuClick);
            this.Menu1_2.Click += new EventHandler(eMenuClick);
            this.Menu1_3.Click += new EventHandler(eMenuClick);
            this.Menu1_4.Click += new EventHandler(eMenuClick);
            

            this.btnHelp.GotFocus += new EventHandler(eControl_GotFocus);
            this.cboClass.GotFocus += new EventHandler(eControl_GotFocus);
            this.cboClass.LostFocus += new System.EventHandler(eControl_LostFocus);
            this.TxtMisuID.LostFocus += new System.EventHandler(eControl_LostFocus);
            this.TxtGelCode.LostFocus += new System.EventHandler(eControl_LostFocus);

            this.cboClass.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            this.chkCash.CheckedChanged += new EventHandler(eChk_checkedChanged);

            this.cboClass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.TxtMisuID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.TxtGelCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.dtpBDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.cboBun.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.cboIO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.TxtMirYYMM.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.cboGubun.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.dtpFDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.dtpTDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.cboDept.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.cboMgrRank.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);

            this.ssList2.EditModeOn += new EventHandler(eControl_EditModeon);
            this.ssList2.EditModeOff += new EventHandler(eControl_EditModeoff);
            this.ssList2.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);
            this.ssList2.EditChange += new EditorNotifyEventHandler(Spd_EditChange);
            this.ssList2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
        }

        void setCombo()
        {
    
            int i = 0;

            cboClass.Items.Clear();
            cboClass.Items.Add("08.계약처");
            cboClass.Items.Add("09.헌혈미수");
            cboClass.Items.Add("11.보훈청");
            cboClass.Items.Add("12.시각장애");
            cboClass.Items.Add("13.심신장애");
            cboClass.Items.Add("14.장애인보장구");
            cboClass.Items.Add("15.직원대납");
            cboClass.Items.Add("16.노인장기요양소견서");
            cboClass.Items.Add("17.방문간호지시서");
            cboClass.Items.Add("18.치매검사");
            cboClass.SelectedIndex = 0;

            GstrBunSu.GstrClass = VB.Left(cboClass.Text, 2);

            cboBun.Items.Clear();
            cboBun.Items.Add("01.회사부담");
            cboBun.Items.Add("02.본인부담");

            cboIO.Items.Clear();

            cboIO.Items.Clear();
            for (i = 0; i < cMisuMst.GstrMisuIO.Length; i++)
            {
                cboIO.Items.Add(cMisuMst.GstrMisuIO[i]);
            }

            cboDept.Items.Clear();
            for (i = 0; i < cMisuMst.GstrDept.Length; i++)
            {
                if (cMisuMst.GstrDept[i] != "")
                {
                    cboDept.Items.Add(cMisuMst.GstrDept[i]);
                }
            }

            cboMgrRank.Items.Clear();
            cboMgrRank.Items.Add("1.완불가능");
            cboMgrRank.Items.Add("2.일부입금");
            cboMgrRank.Items.Add("3.대손처리");
            cboMgrRank.Items.Add("4.산재신청중");
            cboMgrRank.Items.Add("9.기타");
            cboMgrRank.SelectedIndex = 0;

            cboGubun.Items.Clear();
            cboGubun.Items.Add(" ");

            
        }

        void setTxtTip()
        {

        }

        void setCtrlInit()
        {
            //clsCompuInfo.SetComputerInfo();
            //DataTable dt = ComQuery.Select_BAS_PCCONFIG(clsDB.DbCon, clsCompuInfo.gstrCOMIP, "프로그램PC세팅", "프로그램위치", "접수프로그램위치_내시경실");

            //if (ComFunc.isDataTableNull(dt) == false)
            //{
            //    //설정세팅
            //}
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                MagamSpd.sSpd_PmpaMisuMast_1(ssList2, MagamSpd.senmPmpaMisuMast_1, MagamSpd.nenmPmpaMisuMast_1, 15, 0);
                MagamSpd.sSpd_PmpaMisuMast_2(ssList3, MagamSpd.senmPmpaMisuMast_2, MagamSpd.nenmPmpaMisuMast_2, 15, 0);


                CPP = new clsPmpaPb();
                cMisuMst = CPP.INITIAL_SET(clsDB.DbCon, "TA");

                GstrBunSu = new clsPmpaPb.GstrBunSu();

                //툴팁
                setTxtTip();

                read_sysdate();

                screen_clear();

                setCtrlData();

                //설정정보 체크
                setCtrlInit();

                switch (clsType.User.IdNumber)
                {
                    case "4349":
                    case "12576":
                    case "19684":
                    case "20175":
                    case "23417":
                    case "36550":
                    case "45316":
                        btnDel.Enabled = true;
                        break;
                    default:
                        btnDel.Enabled = false;
                        break;
                }
                btnOK.Enabled = false;
                panel_main(false);
                cboBun.Text = "";
                cboIO.Text = "";
                cboDept.Text = "";
                GbGubun.Visible = false;

                cboClass.Select();

                foreach (Form frm2 in Application.OpenForms) //떠있는지 체크
                {
                    if (frm2.Name == "frmcomLibBMstSearch1")
                    {
                        frm2.Close();
                    }
                    else if (frm2.Name == "frmPmpaViewMisuNumSearch")
                    {

                        frm2.Close();
                    }
                }
            }
        }

        void eFormResize(object sender, EventArgs e)
        {

        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                foreach (Form frm2 in Application.OpenForms)
                {
                    if (frm2.Name == "frmcomLibBMstSearch1")
                    {
                        frm2.Close();
                        break;
                    }
                }
                foreach (Form frm2 in Application.OpenForms)
                {
                    if (frm2.Name == "frmPmpaViewMisuNumSearch")
                    {
                        frm2.Close();
                        break;
                    }
                }
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eSpdChange(object sender, EventArgs e)
        {

        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 1) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right) return;

            //if (sender == this.ssList2)
            //{
            //    if (e.ColumnHeader == true)
            //    {
            //        clsSpread.gSpdSortRow(o, e.Column); //sort 정렬 기능 
            //        return;
            //    }
            //    if (e.RowHeader == true)
            //    {
            //        return;
            //    }
            //}

            if (btnOK.Enabled == true && e.Column == 0)
            {
                clsPmpaType.TMS.Del[e.Row] = ssList2.ActiveSheet.Cells[e.Row, e.Column].Text;
                Amt_Account();
            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {

        }

        void eSpreadButtonClick(object sender, EditorNotifyEventArgs e)
        {
            if (btnOK.Enabled == true && e.Column == 0)
            {
                clsPmpaType.TMS.Del[e.Row] = ssList2.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();

                Amt_Account();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnHelp)
            {
                frmPmpaViewGelSearchX = new frmPmpaViewGelSearch();
                frmPmpaViewGelSearchX.rGetData += new frmPmpaViewGelSearch.GetData(GetText);
                frmPmpaViewGelSearchX.rEventClose += new frmPmpaViewGelSearch.EventClose(frmPmpaViewGelSearchX_rEventExit);
                frmPmpaViewGelSearchX.Show();

            }
            else if (sender == this.btnOK)
            {
                GetData(clsDB.DbCon, ssList1, ssList2);
            }
            else if (sender == this.btnNext)
            {
                btnOK.Enabled = false;
                screen_clear();
                panel_main(false);
                Menu1_1.Enabled = true;
                Menu1_2.Enabled = false;
                Menu1_3.Enabled = true;
                cboClass.Enabled = true;
                cboClass.Focus();
            }
            else if (sender == this.btnDel)
            {
                GetDelete(clsDB.DbCon);
            }
        }

        void eMenuClick(object sender, EventArgs e)
        {
            if (sender == this.Menu1_1)
            {
                btnOK.Enabled = false;
                screen_clear();
                btnOK.Enabled = true;
                panel_main(true);
                TxtMisuID.Focus();
            }
            else if (sender == this.Menu1_2)
            {
                panel_main(true);
                TxtMisuID.Focus();
            }
            else if (sender == this.Menu1_3)
            {
                GstrBunSu.GstrClass = VB.Left(cboClass.Text, 2);

                foreach (Form frm2 in Application.OpenForms) //중복로드 방지
                {
                    if (frm2.Name == "frmcomLibBMstSearch1")
                    {

                        frm2.Visible = true;
                        frm2.Activate();
                        return;
                    }
                }

                frmcomLibBMstSearch1 frm = new frmcomLibBMstSearch1(GstrBunSu,"GITA");
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(0, 50);
                frm.Show();
            }
            else if (sender == this.Menu1_4)
            {
                GstrBunSu.GstrClass = VB.Left(cboClass.Text, 2);

                foreach (Form frm2 in Application.OpenForms) //중복로드 방지
                {
                    if (frm2.Name == "frmPmpaViewMisuNumSearch")
                    {

                        frm2.Visible = true;
                        frm2.Activate();
                        return;
                    }
                }

                frmPmpaViewMisuNumSearch frm = new frmPmpaViewMisuNumSearch(GstrBunSu,"GITA");
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(0, 50);
                frm.Show();
            }
        }

        void eSave(PsmhDb pDbCon, FpSpread spd2, int argRow, int argCol)
        {
            string strData = "";
            string strMDATE = "";  //마감일자
            string strROWID = "";
            strMDATE = cPM.READ_MISU_MAGAM_DATE();

            if (argCol == 0) { return; }

            if (argCol == 1)
            {
                strData = ssList2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Bdate].Text.Trim();
                strROWID = ssList2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast_1.ROWID].Text.Trim();

                if (strData != "")
                {
                    if (string.Compare(strData, dtpBDate.Text) < 0)
                    {
                        ComFunc.MsgBox("변동일자는 청구일자보다 작을 수 없습니다", "확인");
                        ssList2.ActiveSheet.SetActiveCell(argRow,argCol);
                        return;
                    }

                    if (string.Compare(strData, strMDATE) <= 0 && strROWID == "")
                    {
                        ComFunc.MsgBox("입금일자는 청구일자 보다 작을 수 없습니다", "확인");
                        ssList2.ActiveSheet.SetActiveCell(argRow, argCol);
                        return;
                    }

                    if (CF.DATE_ILSU(clsDB.DbCon, cpublic.strSysDate, strData) > 50)
                    {
                        ComFunc.MsgBox("현재부터 50일전까지만 처리가 가능함", "확인");
                        ssList2.ActiveSheet.SetActiveCell(argRow, argCol);
                        return;
                    }
                }
                ssList2.ActiveSheet.SetActiveCell(argRow, argCol + 1);
            }
        
            if (argCol == 2)
            {
                strData = spd2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Gubun].Text;
                if (strData != "")
                {
                    switch (strData)
                    {
                        case "11":
                        case "21":
                        case "25":
                            break;
                        case "31":
                        case "32":
                        case "99":
                            break;
                        default:
                            ComFunc.MsgBox("구분이 오류입니다.");
                            break;
                    }

                    if (strData == "99")
                    {
                        #region SAKGAM_CALC_Rtn 삭감대상 총 진료비, 삭감액을 계산하여 Sheet에 Display
                        int i = 0;
                        double nWorkSak = 0;

                        for (i = 0; i < 200; i++)
                        {
                            if (clsPmpaType.TMS.Del[i] != "True" && VB.Val(clsPmpaType.TMS.Gubun[i]) > 10)
                            {
                                switch (clsPmpaType.TMS.Gubun[i])
                                {
                                    case "11":
                                    case "12":
                                    case "13":
                                    case "14":
                                    case "15":
                                    case "16":
                                    case "17":
                                    case "18":
                                    case "19":
                                        nWorkSak = nWorkSak + clsPmpaType.TMS.Amt[i];
                                        break;//청구액(미수액)
                                    default:
                                        nWorkSak = nWorkSak = clsPmpaType.TMS.Amt[i];
                                        break;
                                }
                            }
                        }

                        spd2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Gubun].Text = "31";
                        spd2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Amt].Text = nWorkSak.ToString("###########0");
                        clsPmpaType.TMS.Gubun[argRow] = "31";
                        strData = "31";
                        clsPmpaType.TMS.Amt[argRow] = nWorkSak;
                        #endregion End SAKGAM_CALC_Rtn
                    }

                    //자보환자미수관리일때
                    if (GstrBunSu.GstrClass == "TA")
                    {
                        strData = strData + "TA";
                    }

                    strData = VB.Val(spd2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Gubun].Text).ToString("00");
                    spd2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast_1.GubunName].Text = cPM.READ_MisuGye_MISU(strData, "TA");
                    
                    if (cPM.READ_MisuGye_MISU(strData, "TA") == "")
                    {
                        ComFunc.MsgBox("구분이 오류 입니다.", "확인");
                        return;
                    }
                }
            }

            if (argCol == 2 || argCol == 4 || argCol == 5)
            {
                switch (argCol)
                {
                    case 2:
                        clsPmpaType.TMS.Gubun[argRow] = spd2.ActiveSheet.Cells[argRow, argCol].Text;
                        spd2.ActiveSheet.SetActiveCell(argRow, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Qty);
                        break;
                    case 4:
                        clsPmpaType.TMS.Qty[argRow] = Convert.ToInt32(VB.Val(spd2.ActiveSheet.Cells[argRow, argCol].Text));
                        spd2.ActiveSheet.SetActiveCell(argRow, argCol + 1);
                        break;
                    case 5:
                        clsPmpaType.TMS.Amt[argRow] = Convert.ToInt32(VB.Val(spd2.ActiveSheet.Cells[argRow, argCol].Text));
                        spd2.ActiveSheet.SetActiveCell(argRow, argCol + 1);
                        break;
                }

                Amt_Account();
            }

            if (argCol == 6)
            {
                int bytecount = System.Text.Encoding.Default.GetByteCount(ssList2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Remark].Text);
                if (bytecount >= 60)
                {
                    ComFunc.MsgBox("REMARK 적요 길이가 너무 큽니다.", "확인");
                    ssList2.ActiveSheet.SetActiveCell(argRow, argCol);
                    ssList2.ActiveSheet.Cells[argRow, argCol].Text = strRemark;
                }
            }
        }

        void eControl_GotFocus(object sender, EventArgs e)
        {
            if (sender == cboClass)
            {
                if (GstrBunSu.GnWRTNO != 0)
                {
                    Screen_Display(clsDB.DbCon, ssList1, ssList2,ssList3);
                    btnOK.Enabled = true;
                    GstrBunSu.GnWRTNO = 0;
                    Menu1_1.Enabled = false;
                    Menu1_2.Enabled = true;
                    Menu1_3.Enabled = false;
                    cboClass.Enabled = false;
                    ssList2.Select();
                    return;
                }
                Menu1_1.Enabled = true;
                Menu1_2.Enabled = false;
                Menu1_3.Enabled = true;
            }
        }

        void eControl_LostFocus(object sender, EventArgs e)
        {
            if (sender == cboClass)
            {
                GstrBunSu.GstrClass = ComFunc.LeftH(cboClass.Text, 2);

                switch (GstrBunSu.GstrClass)
                {
                    case "18":
                        GbGubun.Visible = true;
                        break;
                    default:
                        GbGubun.Visible = false;
                        break;
                }

                ComboSet_Gubun(GstrBunSu.GstrClass);
            }else if(sender == dtpBDate)
            {
                if(CF.DATE_ILSU(clsDB.DbCon,cpublic.strSysDate,dtpBDate.Text) > 50)
                {
                    ComFunc.MsgBox("현재부터 50일전까지만 처리가 가능함", "확인");
                    dtpBDate.Focus();
                    return;
                }
            }else if(sender == TxtGelCode)
            {
                if(TxtGelCode.Text.Trim() == "")
                {
                    return;
                }

                TxtGelCode.Text = TxtGelCode.Text.ToUpper();
                lblMiaName.Text = cPM.READ_BAS_MIA(TxtGelCode.Text.Trim());
                if(lblMiaName.Text.Trim() == "")
                {
                    ComFunc.MsgBox("조합기호가 등록되지 않음", "확인");
                    TxtGelCode.Focus();
                    return;
                }
            }else if(sender == TxtMisuID)
            {
                string strTemp = "";

                if(TxtMisuID.Text.Trim() == "")
                {
                    return;
                }
                TxtMisuID.Text = VB.Val(TxtMisuID.Text).ToString("00000000");
                lblSname.Text = cPM.READ_BAS_PATIENT(TxtMisuID.Text.Trim());

                if(lblSname.Text.Trim() == "")
                {
                    ComFunc.MsgBox("해당번호가 등록되지 않았습니다.", "확인");
                    TxtMisuID.Text = "";
                    TxtMisuID.Focus();
                    return;
                }

                if(GstrBunSu.GnWRTNO == 0)
                {
                    strTemp = cPM.Misu_Gubun_Chk(clsDB.DbCon, GstrBunSu.GstrClass, TxtMisuID.Text);
                    //cboGubun.text ="";
                    cboGubun.SelectedIndex = -1;
                    if(strTemp != "")
                    {
                        string strMsg = "이전 미수등록시 지역구분에 [" + strTemp+ "]등록되어있습니다."+ ComNum.VBLF + ComNum.VBLF + "이전내역을 그대로 사용하시겠습니까?";

                        if (ComFunc.MsgBoxQ(strMsg, "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        {
                            cboGubun.Text = strTemp;
                        }
                    }
                }
            }
        }

        void eControl_EditModeoff(object sender, EventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            int nRowIndex = 0;
            int nCollndex = 0;

            nRowIndex = ssList2.ActiveSheet.ActiveRowIndex;
            nCollndex = ssList2.ActiveSheet.ActiveColumnIndex;

            if (sender == this.ssList2)
            {
               eSave(clsDB.DbCon, ssList2, nRowIndex, nCollndex);
            }
        }

        void eControl_EditModeon(object sender, EventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            int nRowIndex = 0;
            int nCollndex = 0;

            strRemark = "";
            nRowIndex = ssList2.ActiveSheet.ActiveRowIndex;
            nCollndex = ssList2.ActiveSheet.ActiveColumnIndex;

            if (sender == this.ssList2)
            {
                strRemark = ssList2.ActiveSheet.Cells[nRowIndex, nCollndex].Text;
            }
        }


        void Spd_EditChange(object sender, EditorNotifyEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            string s = string.Empty;

            if (o.ActiveSheet.ActiveColumnIndex == 1)
            {
                int argRow = 0, argCol = 0;
                string strMDATE = cPM.READ_MISU_MAGAM_DATE();

                argRow = o.ActiveSheet.ActiveRowIndex;
                argCol = o.ActiveSheet.ActiveColumnIndex;
            }
        }

        void eCboSelChanged(object sender, EventArgs e)
        {
            GstrBunSu.GstrClass = ComFunc.LeftH(cboClass.Text, 2);
        }

        void eChk_checkedChanged(object sender, EventArgs e)
        {
            if(sender == chkCash)
            {
                if(chkCash.Checked == true)
                {
                    CardApprov_Amt(this.chkCash, "CARD");
                }
                else
                {
                    return;
                }
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            {
                if(sender == cboClass)
                {
                    TxtMisuID.Focus();
                }
                else if (sender == TxtMisuID)
                {
                    TxtGelCode.Focus();
                }else if(sender == TxtGelCode)
                {
                    dtpBDate.Focus();
                }else if(sender == dtpBDate)
                {
                    cboBun.Focus();
                }else if(sender == cboBun)
                {
                    cboIO.Focus();
                }else if(sender == cboIO)
                {
                    TxtMirYYMM.Focus();
                }else if(sender == TxtMirYYMM)
                {
                    dtpFDate.Focus();
                }else if(sender == dtpFDate)
                {
                    dtpTDate.Focus();
                }else if(sender == dtpTDate)
                {
                    cboDept.Focus();
                }else if(sender == cboDept)
                {
                    cboMgrRank.Focus();
                }else if(sender == cboMgrRank)
                {
                    ssList2.Focus();
                }
                else if(sender == ssList2)
                {
                    //int argRow = 0;
                    //int argCol = 0;

                    //FpSpread o = (FpSpread)sender;

                    //argRow = o.ActiveSheet.ActiveRowIndex;
                    //argCol = o.ActiveSheet.ActiveColumnIndex;

                    //if (argCol == (int)clsComPmpaSpd.enmPmpaMisuMast_1.Remark)
                    //{
                    //    ssList2.ActiveSheet.SetActiveCell(argRow + 1, 0);
                    //}
                }
            }

        }

        void read_sysdate()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void screen_clear(string Job = "")
        {
            if (Job == "")
            {
                //int i = 0;

                TxtMisuID.Text = ""; lblSname.Text = "";
                TxtGelCode.Text = ""; lblMiaName.Text = "";
                cboIO.SelectedIndex = -1;
                cboBun.SelectedIndex = -1;
                lblWRTNO.Text = "";
                TxtMirYYMM.Text = "";

                cPM.TMM_Clear_Rtn();

                ssList2.ActiveSheet.RowCount = 0;
                ssList2.ActiveSheet.RowCount = 20;
                ssList3.ActiveSheet.RowCount = 0;
                ssList3.ActiveSheet.RowCount = 20;

                CS.Spread_Clear_Range(ssList1, 0, 0, ssList1.ActiveSheet.RowCount, ssList1.ActiveSheet.ColumnCount);

                dtpBDate.Text = cpublic.strSysDate;
                dtpFDate.Text = cpublic.strSysDate;
                dtpFDate.Text = "";
                dtpFDate.Text = cpublic.strSysDate;
                dtpTDate.Text = "";

                cboDept.SelectedIndex = -1;
                cboMgrRank.SelectedIndex = -1;


                cboGubun.Items.Clear();
                ////스프레드 범위 클리어
                //CS.Spread_Clear_Range(ssList1, 0, 0, ssList1.ActiveSheet.RowCount, ssList1.ActiveSheet.ColumnCount);
            }
            else if (Job == "Clear")
            {
                //ssList1.ActiveSheet.RowCount = 0;
                // 화면상의 정렬표시 Clear ( Sort 정렬 아이콘 클리어)
                //ssList1.ActiveSheet.ColumnHeader.Cells[0, 0, 0, ssList1.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }
        }

        void Screen_Display(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd1, FarPoint.Win.Spread.FpSpread Spd2, FarPoint.Win.Spread.FpSpread Spd3)
        {
            int i = 0;
            //int j = 0;
            string strMDATE = "", strCDATE = "";
            int nREAD = 0;
            DataTable dt = null;

            strMDATE = cPM.READ_MISU_MAGAM_DATE();

            cPM.READ_MISU_IDMST(GstrBunSu.GnWRTNO);

            if (clsPmpaType.TMM.ROWID == "")
            {
                ComFunc.MsgBox("미수내역이 없습니다", "확인");
            }

            TxtMisuID.Text = clsPmpaType.TMM.MisuID;
            lblWRTNO.Text = clsPmpaType.TMM.WRTNO.ToString();
            TxtGelCode.Text = clsPmpaType.TMM.GelCode;
            lblMiaName.Text = cPM.READ_BAS_MIA(clsPmpaType.TMM.GelCode.Trim());
            dtpBDate.Text = clsPmpaType.TMM.BDate;
            TxtMirYYMM.Text = clsPmpaType.TMM.MirYYMM;
            cboIO.SelectedIndex = cPM.ListIndex_MisuIO(clsPmpaType.TMM.IpdOpd.Trim());

            if(clsPmpaType.TMM.Class == "18")
            {
                if(string.Compare(clsPmpaType.TMM.Gubun,"00") > 0)
                {
                    cboGubun.Text = clsPmpaType.TMM.Gubun + "." + cPM.Misu_Gubun_NameChk(clsDB.DbCon,clsPmpaType.TMM.Class, clsPmpaType.TMM.Gubun);
                }
            }

            if (string.Compare(clsPmpaType.TMM.MgrRank, "1") < 0)
            {
                cboMgrRank.SelectedIndex = -1;
            }else
            {
                if (clsPmpaType.TMM.MgrRank == "9")
                {
                    cboMgrRank.Text = "9.기타";
                }
                else
                {
                    cboMgrRank.SelectedIndex = Convert.ToInt32(VB.Val(clsPmpaType.TMM.MgrRank)) - 1;
                }
            }

            if(VB.Left(cboClass.Text,2) == "10")
            {
                cboBun.Enabled = true;
                cboBun.Items.Clear();
                cboBun.Items.Add("01.회사부담");
                cboBun.Items.Add("02.본인부담");
                if(clsPmpaType.TMM.Bun == "01") { cboBun.SelectedIndex = 0; }
                else { cboBun.SelectedIndex = 1; }
            }else
            {
                cboBun.Items.Clear();
                cboBun.Enabled = false;
            }

            lblSname.Text = cPM.READ_BAS_PATIENT(clsPmpaType.TMM.MisuID.Trim());
            dtpFDate.Text = clsPmpaType.TMM.FromDate.Trim();
            dtpTDate.Text = clsPmpaType.TMM.ToDate.Trim();
            cboDept.SelectedIndex = cPM.ListIndex_MisuDept(clsPmpaType.TMM.DeptCode.Trim());

            //MISU_SLIP Display

            dt = cSQL.Sel_MISU_SLIP_Display(pDbCon, GstrBunSu.GnWRTNO);


            nREAD = dt.Rows.Count;
            Spd2.ActiveSheet.RowCount = dt.Rows.Count + 20;


            for (i = 0; i < nREAD; i++)
            {
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.A].Text = "";
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Bdate].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                strCDATE = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Bdate].Text;
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Gubun].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.GubunName].Text = cPM.READ_MisuGye_MISU(dt.Rows[i]["Gubun"].ToString().Trim(), "TA");
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Qty].Text = dt.Rows[i]["Qty"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Amt].Text = dt.Rows[i]["Amt"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Remark].Text = dt.Rows[i]["Remark"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.OldBdate].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.OldGubun].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.OldQty].Text = dt.Rows[i]["Qty"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.OldAmt].Text = dt.Rows[i]["Amt"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.OldRemark].Text = VB.Left(dt.Rows[i]["Remark"].ToString().Trim(),30);
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.EntDate].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.EntPart].Text = dt.Rows[i]["EntPart"].ToString().Trim();
                //Row 높이 설정 
                //Spd2.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);

                //Row 높이 설정 2020-08-26 
                FarPoint.Win.Spread.Row row;
                row = Spd2.ActiveSheet.Rows[i];
                float rowSize = row.GetPreferredHeight();
                row.Height = rowSize;

                //TODO
                //'2018-07-05
                //If strCDATE <= strMDATE Then
                //    SS2.Col = -1
                //    SS2.Lock = True
                //End If

                clsPmpaType.TMS.Del[i] = " ";
                clsPmpaType.TMS.Gubun[i] = dt.Rows[i]["Gubun"].ToString().Trim();
                clsPmpaType.TMS.Qty[i] = Convert.ToInt32(VB.Val(dt.Rows[i]["Qty"].ToString().Trim()));
                clsPmpaType.TMS.Amt[i] = Convert.ToInt32(VB.Val(dt.Rows[i]["Amt"].ToString().Trim()));
                clsPmpaType.TMS.TAmt[i] = 0;

            }

            dt.Dispose();
            dt = null;

            Amt_Account();


            //MISU_MONTHLY Display
            dt = cSQL.Sel_MISU_MONTHLY_Display(clsDB.DbCon, GstrBunSu.GnWRTNO);
            Spd3.ActiveSheet.RowCount = dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_2.YYMM].Text = dt.Rows[i]["YYMM"].ToString().Trim();
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_2.IwolAmt].Text = dt.Rows[i]["IwolAmt"].ToString().Trim();
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_2.MisuAmt].Text = dt.Rows[i]["MisuAmt"].ToString().Trim();
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_2.IpgumAmt].Text = dt.Rows[i]["IpgumAmt"].ToString().Trim();
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_2.SakAmt].Text = dt.Rows[i]["SakAmt"].ToString().Trim();
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_2.BanAmt].Text = dt.Rows[i]["BanAmt"].ToString().Trim();
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_2.EtcAmt].Text = dt.Rows[i]["EtcAmt"].ToString().Trim();
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_2.JanAmt].Text = dt.Rows[i]["JanAmt"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd1, FarPoint.Win.Spread.FpSpread Spd2)
        {
            int i = 0;
            string strROWID = "", strIdChange = "", strSlipChange = "";

            string strDel = "", strBdate1 = "", strGubun1 = "";
            double nQty1 = 0, nTAmt1 = 0, nAmt1 = 0;
            string strRemark1 = "";

            string strBdate2 = "", strGubun2 = "";
            double nQty2 = 0, nTAmt2 = 0, nAmt2 = 0;
            string strRemark2 = "", strEntDate2 = "";
            long nEntPart2 = 0;

            string strFromDate = "", strToDate = "";
            string strJiGubun = ""; //치매 지역구분
            string strMDATE = ""; //미수마감일 

            string Time = "";
            Time = VB.Left(cpublic.strSysTime, 5);

            string SqlErr = ""; //에러문 받는 변수
            string SQL = "";
            int intRowAffected = 0;
            DataTable dt = null;

            strMDATE = cPM.READ_MISU_MAGAM_DATE();

            Invalid_Data_Check();

            clsDB.setBeginTran(pDbCon);
            Invalid_Data_Check();
            #region CmdOK_IDMST_RTN
            clsPmpaType.TMN.WRTNO = clsPmpaType.TMM.WRTNO;
            clsPmpaType.TMN.MisuID = TxtMisuID.Text.Trim();
            clsPmpaType.TMN.BDate = dtpBDate.Text.Trim();
            clsPmpaType.TMN.Class = VB.Left(cboClass.Text, 2);
            clsPmpaType.TMN.IpdOpd = VB.Left(cboIO.Text, 1);
            clsPmpaType.TMN.GelCode = TxtGelCode.Text.Trim();
            clsPmpaType.TMN.Bun = "01";
            if (clsPmpaType.TMN.Class == "10")
            {
                clsPmpaType.TMN.Bun = VB.Left(cboBun.Text, 2);
            }
            clsPmpaType.TMN.DeptCode = "";
            clsPmpaType.TMN.MgrRank = VB.Left(cboMgrRank.Text, 1);
            clsPmpaType.TMN.GbEnd = "1";

            if (clsPmpaType.TMN.JAmt < 1)
            {
                clsPmpaType.TMN.GbEnd = "0";
            }

            clsPmpaType.TMN.FromDate = dtpFDate.Text;
            clsPmpaType.TMN.ToDate = dtpTDate.Text;

            if (clsPmpaType.TMN.FromDate.Trim() != "" && clsPmpaType.TMN.ToDate.Trim() != "")
            {
                clsPmpaType.TMN.Ilsu = CF.DATE_ILSU(clsDB.DbCon, clsPmpaType.TMN.ToDate, clsPmpaType.TMN.FromDate) + 1;
            }
            else{
                clsPmpaType.TMN.Ilsu = 0;
            }

            clsPmpaType.TMN.DeptCode = VB.Left(cboDept.Text, 2);
            clsPmpaType.TMN.Qty[0] = 1;
            clsPmpaType.TMN.MirYYMM = TxtMirYYMM.Text;
            clsPmpaType.TMN.Remark = "";

            if (clsPmpaType.TMN.FromDate.Trim() == "")
            {
                strFromDate = "";
            }
            else
            {
                strFromDate = clsPmpaType.TMN.FromDate;
            }

            if (clsPmpaType.TMN.ToDate.Trim() == "")
            {
                strToDate = "";
            }
            else
            {
                strToDate = clsPmpaType.TMN.ToDate;
            }

            strJiGubun = "";

            if (VB.Left(cboClass.Text, 2) == "18")
            {
                if (VB.Left(cboGubun.Text, 2) != "")
                {
                    strJiGubun = VB.Left(cboGubun.Text, 2);
                }
            }

            if (clsPmpaType.TMN.WRTNO == 0)
            {
                #region BtnOk_IDMST_INSERT 신규등록
                strIdChange = "NO";

                dt = cSQL.Sel_MISU_IDMST_CmdOK_IDMST_INSERT(pDbCon);

                if (dt.Rows.Count == 0)
                {
                    clsPmpaType.TMN.WRTNO = 1;
                }
                else
                {
                    clsPmpaType.TMN.WRTNO = Convert.ToInt32(VB.Val(dt.Rows[0]["mWRTNO"].ToString().Trim())) + 1;
                }

                dt.Dispose();
                dt = null;

                try
                {
                    SqlErr = cSQL.Int_MISU_IDMST_CmdOK_IDMST_INSERT(pDbCon, strFromDate, strToDate, cpublic.strSysDate, cpublic.strSysTime, strJiGubun, ref intRowAffected);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        ComFunc.MsgBox("MISU_IDMST 신규등록시 오류가 발생함", "확인");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                        return;
                    }
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(ex.Message);
                }

                #endregion End BtnOk_IDMST_INSERT
            }
            else
            {
                #region BtnOk_IDMST_UPDATE 변경등록
                strIdChange = "NO";

                if (clsPmpaType.TMM.MisuID != clsPmpaType.TMN.MisuID) { strIdChange = "OK"; }
                if (clsPmpaType.TMM.GelCode != clsPmpaType.TMN.GelCode) { strIdChange = "OK"; }
                if (clsPmpaType.TMM.BDate != clsPmpaType.TMN.BDate) { strIdChange = "OK"; }
                if (clsPmpaType.TMM.IpdOpd != clsPmpaType.TMN.IpdOpd) { strIdChange = "OK"; }
                if (clsPmpaType.TMM.MirYYMM != clsPmpaType.TMN.MirYYMM) { strIdChange = "OK"; }
                if (clsPmpaType.TMM.MgrRank != clsPmpaType.TMN.MgrRank) { strIdChange = "OK"; }

                try
                {
                    if (strIdChange == "OK")
                    {
                        SqlErr = cSQL.Upt_MISU_IDMST_CmdOK_IDMST_UPDATE(pDbCon, strFromDate, strToDate, cpublic.strSysDate, cpublic.strSysTime, ref intRowAffected);
                    }
                    else
                    {
                        SqlErr = cSQL.Upt_MISU_IDMST_CmdOK_IDMST_UPDATE1(pDbCon, strFromDate, strToDate, ref intRowAffected);
                    }

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        ComFunc.MsgBox("MISU_IDMST 변경 등록시 오류가 발생함", "확인");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                        return;
                    }

                    //SLIP에 MisuID,GelCode,IpdOpd가 변경되면 Update

                    strIdChange = "NO";

                    if (clsPmpaType.TMM.MisuID != clsPmpaType.TMN.MisuID) { strIdChange = "OK"; }
                    if (clsPmpaType.TMM.GelCode != clsPmpaType.TMN.GelCode) { strIdChange = "OK"; }
                    if (clsPmpaType.TMM.IpdOpd != clsPmpaType.TMN.IpdOpd) { strIdChange = "OK"; }
                    if (clsPmpaType.TMM.MirYYMM != clsPmpaType.TMN.MirYYMM) { strIdChange = "OK"; }

                    if (strIdChange == "OK")
                    {
                        SqlErr = cSQL.Upt_MISU_SLIP_CmdOK_IDMST_UPDATE(pDbCon, ref intRowAffected);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            ComFunc.MsgBox("MISU_SLIP 변경 등록시 오류가 발생함", "확인");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(ex.Message);
                }
                #endregion End BtnOk_IDMST_UPDATE

                #region BtnOk_IDMST_History
                if (strIdChange == "NO") { }
                else
                {
                    try
                    {
                        SqlErr = cSQL.Int_MISU_HISTORY_CmdOK_IDMST_UPDATE(pDbCon, cpublic.strSysDate, Time, ref intRowAffected);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            ComFunc.MsgBox("MISU_HISTORY INSERT 오류가 발생함", "확인");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(ex.Message);
                    }
                }
                #endregion
            }

            #endregion
            #region CmdOK_Slip_RTN Slip 변경분 Update
            //수정, 변경분 Insert
            for (i = 0; i < Spd2.ActiveSheet.RowCount; i++)
            {
                strDel = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.A].Text;
                strBdate1 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Bdate].Text;
                strGubun1 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Gubun].Text;
                nQty1 = Convert.ToInt32(VB.Val(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Qty].Text));
                nAmt1 = Convert.ToInt32(VB.Val(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Amt].Text));
                strRemark1 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.Remark].Text;

                strROWID = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.ROWID].Text;
                strBdate2 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.OldBdate].Text;
                strGubun2 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.OldGubun].Text;
                nQty2 = Convert.ToInt32(VB.Val(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.OldQty].Text));
                nAmt2 = Convert.ToInt32(VB.Val(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.OldAmt].Text));
                strRemark2 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.OldRemark].Text;
                strEntDate2 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.EntDate].Text;
                nEntPart2 = Convert.ToInt32(VB.Val(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast_1.EntPart].Text));

                nTAmt1 = nAmt1;
                nTAmt2 = nAmt2;

                if (strDel == "True")
                {
                    if (strROWID != "")
                    {
                        //2018-07-05
                        if (string.Compare(strBdate1, strMDATE) <= 0 && strBdate1 != "")
                        {
                            cPM.Message_MISU_Magam(strMDATE);
                        }
                        else
                        {
                            #region CmdOK_MisuSlip_Delete 삭제
                            try
                            {
                                SqlErr = cSQL.Del_MISU_SLIP_CmdOK_MisuSlip_Delete(pDbCon, strROWID, ref intRowAffected);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    ComFunc.MsgBox("MISU_SLIP OLD자료 Delete ERROR", "확인");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                    return;
                                }
                                //Slip History에 Insert
                                SqlErr = cSQL.Int_MISU_HISTORY_CmdOK_MisuSlip_Delete(pDbCon, cpublic.strSysDate, cpublic.strSysTime, strBdate2, strGubun2, nQty2, nTAmt2, nAmt2, strRemark2, strEntDate2, nEntPart2, ref intRowAffected);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    ComFunc.MsgBox("MISU_HISTORY INSERT 오류가 발생함", "확인");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(ex.Message);
                            }
                            #endregion End CmdOK_MisuSlip_Delete
                        }
                    }
                }
                else if (strDel != "1")
                {
                    if (strROWID == "")
                    {
                        //2018-07-05
                        if (string.Compare(strBdate1, strMDATE) <= 0 && strBdate1 != "")
                        {
                            cPM.Message_MISU_Magam(strMDATE);
                        }
                        else
                        {
                            if (strBdate1 == "" && strGubun1 == "")
                            {

                            }
                            else
                            {
                                #region CmdOK_MisuSlip_Insert 신규등록
                                try
                                {

                                    SqlErr = cSQL.Int_MISU_SLIP_CmdOK_MisuSlip_Insert(pDbCon, strGubun1, nQty1, nTAmt1, nAmt1, strBdate1, strRemark1,cpublic.strSysDate,Time, ref intRowAffected);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(pDbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        ComFunc.MsgBox("MISU_SLIP INSERT ERROR", "확인");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                        return;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(ex.Message);
                                }
                                #endregion End CmdOK_MisuSlip_Insert
                            }
                        }
                    }
                    else
                    {
                        strSlipChange = "NO";
                        if (strGubun1 != strGubun2) { strSlipChange = "OK"; }
                        if (strBdate1 != strBdate2) { strSlipChange = "OK"; }
                        if (nQty1 != nQty2) { strSlipChange = "OK"; }
                        if (nTAmt1 != nTAmt2) { strSlipChange = "OK"; }
                        if (nAmt1 != nAmt2) { strSlipChange = "OK"; }
                        if (strRemark1 != strRemark2) { strSlipChange = "OK"; }

                        //2018-07-05
                        if (strSlipChange == "OK")
                        {
                            if (VB.Val(strBdate1) <= VB.Val(strBdate2) && strBdate1 != "")
                            {
                                cPM.Message_MISU_Magam(strMDATE);
                            }
                            else
                            {
                                #region CmdOK_MisuSlip_Update
                                try
                                {

                                    SqlErr = cSQL.Upt_MISU_SLIP_CmdOK_MisuSlip_Update(pDbCon, strBdate1, strGubun1, nQty1, nTAmt1, nAmt1, strRemark1, strROWID,cpublic.strSysDate,Time, ref intRowAffected);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(pDbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        ComFunc.MsgBox("MISU_SLIP UPDATE ERROR", "확인");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                        return;
                                    }

                                    //Slip history에 Insert
                                    SqlErr = cSQL.Int_MISU_HISTORY_CmdOK_MISUSlip_Update(pDbCon, cpublic.strSysDate, cpublic.strSysTime, strBdate2, strGubun2, nQty2, nTAmt2, nAmt2, strRemark2, strEntDate2, nEntPart2, ref intRowAffected);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(pDbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        ComFunc.MsgBox("MISU_HISTORY INSERT 오류가 발생함", "확인");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                        return;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(ex.Message);
                                }
                                #endregion End CmdOK_MisuSlip_Update
                            }
                        }
                    }
                }
            }
            #endregion End CmdOK_Slip_RTN
            clsDB.setCommitTran(pDbCon);

            ////화면 Clear & 각종 Enble Set
            btnOK.Enabled = false;
            screen_clear();
            panel_main(false);
            Menu1_1.Enabled = true;
            Menu1_2.Enabled = false;
            Menu1_3.Enabled = true;
            cboClass.Enabled = true;
            cboClass.Focus();
            return;
        }

        void GetDelete(PsmhDb pDbCon)
        {
            string strMsg = "";

            string SqlErr = ""; //에러문 받는 변수
            string SQL = "";
            int intRowAffected = 0;
            string Time = "";
            Time = VB.Left(cpublic.strSysTime, 5);

            strMsg = "미수내역을 삭제 하시겠습니까?";

            if (ComFunc.MsgBoxQ(strMsg, "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            clsDB.setBeginTran(pDbCon);

            try
            {
                    //IDMST를 삭제함 
                    SqlErr = cSQL.Del_MISU_IDMST_CmdDelete(pDbCon, clsPmpaType.TMM.WRTNO, ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("MISU_IDMST DELETE ERROR", "확인");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }

                //HISTORY Insert
                

                    SqlErr = cSQL.Int_MISU_HISTORY_CmdDelete(pDbCon, cpublic.strSysDate, Time, ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("MISU_HISTORY INSERT 오류가 발생함", "확인");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }

                //삭제할 SLIP을 History에 Insert
                SqlErr = cSQL.Int_MISU_HISTORY_CmdDelete1(pDbCon, cpublic.strSysDate, Time, ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("MISU_HISTORY INSERT 오류가 발생함", "확인");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }

                //SLip을 삭제함
                SqlErr = cSQL.Del_MISU_SLIP_CmdDelete(pDbCon, ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("MISU_SLIP DELETE ERROR", "확인");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            clsDB.setCommitTran(pDbCon);

            //화면 Clear & 각종 Enable Set
            btnOK.Enabled = false;
            screen_clear();
            panel_main(false);
            Menu1_1.Enabled = true;
            Menu1_2.Enabled = false;
            Menu1_3.Enabled = true;
            cboClass.Enabled = true;
            cboClass.Focus();
        }

        void Amt_Account()
        {
            int i = 0;

            for (i = 0; i < 8; i++)
            {
                clsPmpaType.TMN.Amt[i] = 0;
            }

            for (i = 0; i < 4; i++)
            {
                clsPmpaType.TMN.Qty[i] = 0;
            }

            for (i = 0; i < 200; i++)
            {
                if (clsPmpaType.TMS.Del[i] != "True" && clsPmpaType.TMS.Amt[i] != 0)
                {
                    switch (clsPmpaType.TMS.Gubun[i])
                    {
                        case "11":
                        case "12":
                        case "13":
                        case "14":
                        case "15":
                        case "16":
                        case "17":
                        case "18":
                        case "19":
                            clsPmpaType.TMN.Qty[0] = clsPmpaType.TMN.Qty[0] + clsPmpaType.TMS.Qty[i];
                            clsPmpaType.TMN.Amt[1] = clsPmpaType.TMN.Amt[1] + clsPmpaType.TMS.Amt[i];
                            break; //청구액(미수액)
                        case "21":
                        case "22":
                        case "23":
                        case "24":
                        case "25":
                        case "26":
                        case "27":
                        case "28":
                        case "29":
                            clsPmpaType.TMN.Qty[1] = clsPmpaType.TMN.Qty[1] + clsPmpaType.TMS.Qty[i];
                            clsPmpaType.TMN.Amt[2] = clsPmpaType.TMN.Amt[2] + clsPmpaType.TMS.Amt[i];
                            break; //입금액
                        case "31":
                            clsPmpaType.TMN.Qty[2] = clsPmpaType.TMN.Qty[2] + clsPmpaType.TMS.Qty[i];
                            clsPmpaType.TMN.Amt[3] = clsPmpaType.TMN.Amt[3] + clsPmpaType.TMS.Amt[i];
                            break; //삭감
                        case "32":
                            clsPmpaType.TMN.Qty[3] = clsPmpaType.TMN.Qty[3] + clsPmpaType.TMS.Qty[i];
                            clsPmpaType.TMN.Amt[4] = clsPmpaType.TMN.Amt[4] + clsPmpaType.TMS.Amt[i];
                            break; //반송
                        case "00":
                        case "01":
                        case "02":
                        case "03":
                        case "04":
                        case "05":
                        case "06":
                        case "07":
                        case "08":
                        case "09":
                        case "10":
                            break; //Remark
                        default:
                            clsPmpaType.TMN.Amt[6] = clsPmpaType.TMN.Amt[6] + clsPmpaType.TMS.Amt[i];
                            break;
                    }
                }
            }

            clsPmpaType.TMN.Amt[0] = clsPmpaType.TMN.Amt[1]; //총 진료비는 청구액
            clsPmpaType.TMN.JAmt = clsPmpaType.TMN.Amt[1];

            for (i = 2; i < 7; i++)
            {
                clsPmpaType.TMN.JAmt = clsPmpaType.TMN.JAmt - clsPmpaType.TMN.Amt[i];
            }

            ssList1.ActiveSheet.Cells[0, 0].Text = VB.Format(clsPmpaType.TMN.Amt[1], "###,###,###,##0"); //미수
            ssList1.ActiveSheet.Cells[0, 1].Text = VB.Format(clsPmpaType.TMN.Amt[2], "###,###,###,##0"); //입금
            ssList1.ActiveSheet.Cells[0, 2].Text = VB.Format(clsPmpaType.TMN.Amt[3], "###,###,###,##0"); //삭감
            ssList1.ActiveSheet.Cells[0, 3].Text = VB.Format(clsPmpaType.TMN.Amt[4] + clsPmpaType.TMN.Amt[5] + clsPmpaType.TMN.Amt[6], "###,###,###,##0"); //기타
            ssList1.ActiveSheet.Cells[0, 4].Text = VB.Format(clsPmpaType.TMN.JAmt, "###,###,###,##0");

        }

        void CardApprov_Amt(CheckBox ch, string strJob)
        {

            clsPublic.GstrMsgTitle = "TEST_TEMP";
            CC.CardVariable_Clear(ref RSD, ref RD);

            CC.gstrCdPtno = TxtMisuID.Text.Trim();
            CC.gstrCdSName = lblSname.Text.Trim();
            CC.gstrCdDeptCode = cboDept.Text.Trim();

            CC.gstrCdPart = clsType.User.IdNumber.Trim();
            CC.gstrCdGbIo = "O.외래";
            CC.gstrCdPCode = "RES+";
            //CC.glngCdAmt = Convert.ToInt64(VB.Replace(clsPmpaPb.gnJinAMT7.ToString(), ",", ""));
            CC.glngCdAmt = 0;
            CC.GstrCardJob = "Menual2";
            this.Tag = "TRUE";

            frmPmpaEntryCardDaou frm = new frmPmpaEntryCardDaou(CC.gstrCdPtno, CC.gstrCdSName, CC.gstrCdDeptCode, CC.gstrCdGbIo, CC.glngCdAmt, "CASH", dtpBDate.Text);
            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = new Point(10, 10);
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);

            CC.CardVariable_Clear(ref RSD, ref RD);
            clsPublic.GstrMsgTitle = "";

            ch.Checked = false;
        }

        void Invalid_Data_Check() //오류 Data Check
        {
            if (TxtMisuID.Text.Trim() == "")
            {
                ComFunc.MsgBox("미수번호가 공란입니다", "확인");
                return;
            }

            if (TxtGelCode.Text.Trim() == "")
            {
                ComFunc.MsgBox("계약처 코드가 공란입니다.", "확인");
                return;
            }

            if (dtpBDate.Text.Trim() == "")
            {
                ComFunc.MsgBox("청구일자가 공란입니다.", "확인");
                return;
            }

            if (VB.Left(cboIO.Text, 1) != "O" && VB.Left(cboIO.Text, 1) != "I")
            {
                ComFunc.MsgBox("외래,입원 선택이 오류입니다.", "확인");
                return;
            }

            if (TxtMirYYMM.Text.Trim() == "")
            {
                ComFunc.MsgBox("청구년월 공란입니다,", "확인");
            }

            return;
        }

        void panel_main(bool argGUBUN)
        {
            TxtMisuID.Enabled = argGUBUN;
            TxtGelCode.Enabled = argGUBUN;
            dtpBDate.Enabled = argGUBUN;
            cboIO.Enabled = argGUBUN;
            cboMgrRank.Enabled = argGUBUN;
            cboGubun.Enabled = argGUBUN;
            cboBun.Enabled = argGUBUN;
            TxtMirYYMM.Enabled = argGUBUN;
            btnHelp.Enabled = argGUBUN;
            dtpFDate.Enabled = argGUBUN;
            dtpTDate.Enabled = argGUBUN;
            cboDept.Enabled = argGUBUN;
        }

        void ComboSet_Gubun(string ArgGubun)
        {
            string SqlErr = ""; //에러문 받는 변수
            string SQL = "";
            DataTable dt = null;
            int i = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT CODE,NAME FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL = SQL + ComNum.VBLF + "WHERE GUBUN ='계약처미수_지역구분'";
            SQL = SQL + ComNum.VBLF + " AND SUBSTR(CODE,1,2) ='" + ArgGubun + "'";
            SQL = SQL + ComNum.VBLF + "ORDER BY CODE";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                cboGubun.Items.Add(VB.Right((dt.Rows[i]["Code"]).ToString().Trim(),2) + "." + dt.Rows[i]["Name"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;
            

            return ;
        }
        
        void GetText(string argCodeName, string argCode)
        {
            TxtGelCode.Text = argCodeName.Trim();
            lblMiaName.Text = argCode.Trim();
        } //델리게이트

        void frmPmpaViewGelSearchX_rEventExit()
        {
            if (frmPmpaViewGelSearchX != null)
            {
                frmPmpaViewGelSearchX.Dispose();
                frmPmpaViewGelSearchX = null;
            }
        } //델리게이트 

        public void setClass(clsPmpaPb.GstrBunSu argBunSu)
        {
            GstrBunSu = argBunSu;
        }
    }
}