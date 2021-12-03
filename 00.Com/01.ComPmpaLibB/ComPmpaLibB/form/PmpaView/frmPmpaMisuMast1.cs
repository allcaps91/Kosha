using ComBase;
using ComDbB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaMisuMast1.cs
    /// Description     : 미수원장관리(보험)
    /// Author          : 김해수
    /// Create Date     : 2018-11-22
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= "\misu\FrmMUMAUN01.frm(FrmMisuMast1) >> frmPmpaMisuMast1.cs 폼이름 재정의" />
    /// 

    public partial class frmPmpaMisuMast1 : Form
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

        private frmPmpaViewGelSearch frmPmpaViewGelSearchX = null;

        #endregion
        string strRemark = "";// remark 임시저장용
        string GstrMisuGbn = "BOHUM";
        string[] strDeptNamek = new string[300];
        string[] strDrName = new string[99];
        string[] strSname = new string[99];

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
        public frmPmpaMisuMast1()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaMisuMast1(MainFormMessage pform)
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

            //this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnOK.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnDel.Click += new EventHandler(eBtnClick);
            this.btnHelp.Click += new EventHandler(eBtnClick);


            //this.btnSave.Click += new EventHandler(eBtnSave);
            this.Menu1_1.Click += new EventHandler(eMenuClick);
            this.Menu1_2.Click += new EventHandler(eMenuClick);
            this.Menu1_3.Click += new EventHandler(eMenuClick);
            this.Menu1_4.Click += new EventHandler(eMenuClick);
            this.Menu2_1.Click += new EventHandler(eMenuClick);

            this.btnHelp.GotFocus += new EventHandler(eControl_GotFocus);
            this.cboClass.GotFocus += new EventHandler(eControl_GotFocus);
            this.cboClass.LostFocus += new System.EventHandler(eControl_LostFocus);
            this.dtpBDate.LostFocus += new System.EventHandler(eControl_LostFocus);
            this.TxtGelCode.LostFocus += new System.EventHandler(eControl_LostFocus);

            this.cboClass.SelectedIndexChanged += new EventHandler(eCboSelChanged);

            this.cboClass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.TxtMisuID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.TxtGelCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.dtpBDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.cboIO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.cboTongGbn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.cboBun.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.TxtJepsuNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.TxtMirYYMM.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);

            this.ssList2.EditModeOn += new EventHandler(eControl_EditModeon);
            this.ssList2.EditModeOff += new EventHandler(eControl_EditModeoff);
            this.ssList2.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);
            this.ssList2.EditChange += new EditorNotifyEventHandler(Spd_EditChange);
        }

        void setCombo()
        {
            cboClass.Items.Clear();
            cboBun.Items.Clear();
            cboIO.Text = "";
            cboTongGbn.Text = "";
            cboBun.Text = "";

            if (GstrMisuGbn == "BOHUM")
            {
                cboClass.Items.Add("01.공단");
                cboClass.Items.Add("02.직장");
                cboClass.Items.Add("03.지역");
                cboClass.Items.Add("04.보호");
            }
            else if (GstrMisuGbn == "SANJE")
            {
                cboClass.Items.Add("05.산재");
            } else if (GstrMisuGbn == "TA")
            {
                cboClass.Items.Add("07.자보");
            } else if (GstrMisuGbn == "ETC")
            {
                cboClass.Items.Add("08.개인미수");
                cboClass.Items.Add("09.혈액원");
                cboClass.Items.Add("10.신검");
                cboClass.Items.Add("11.건진");
                cboClass.Items.Add("12.기타");
            }

            cboClass.SelectedIndex = 0;
            GstrBunSu.GstrClass = VB.Left(cboClass.Text, 2);

            if (GstrMisuGbn == "ETC")
            {
                for (int i = 0; i < cMisuMst.GstrMisuSayu.Length; i++)
                {
                    cboBun.Items.Add(cMisuMst.GstrMisuSayu[i]);
                }
            }
            else
            {
                for (int i = 0; i < cMisuMst.GstrMisuBun.Length; i++)
                {
                    cboBun.Items.Add(cMisuMst.GstrMisuBun[i]);
                }
            }

            cboIO.Items.Clear();

            for (int i = 0; i < cMisuMst.GstrMisuIO.Length; i++)
            {
                cboIO.Items.Add(cMisuMst.GstrMisuIO[i]);
            }


            cboTongGbn.Items.Clear();
            cboTongGbn.Items.Add("1.퇴원청구");
            cboTongGbn.Items.Add("2.중간청구");
            cboTongGbn.Items.Add("3.재청구");
            cboTongGbn.Items.Add("4.추가청구");
            cboTongGbn.Items.Add("5.이의신청");
            cboTongGbn.Items.Add("6.기타청구");

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
                MagamSpd.sSpd_PmpaMisuMast1_2(ssList2, MagamSpd.senmPmpaMisuMast1_2, MagamSpd.nenmPmpaMisuMast1_2, 9, 0);
                MagamSpd.sSpd_PmpaMisuMast1_3(ssList3, MagamSpd.senmPmpaMisuMast1_3, MagamSpd.nenmPmpaMisuMast1_3, 7, 0);

                ssList2.PreviewKeyDown += ssList2_IpgoEntryForm_PreviewKeyDown;

                CPP = new clsPmpaPb(); 
                cMisuMst = CPP.INITIAL_SET(clsDB.DbCon, "");

                GstrBunSu = new clsPmpaPb.GstrBunSu();
                //툴팁
                setTxtTip();

                screen_clear();

                setCtrlData();

                //설정정보 체크
                setCtrlInit();

                read_sysdate();

                switch (clsType.User.IdNumber)
                {
                    case "4349":
                        btnDel.Enabled = true;
                        break;
                    case "13850":
                        btnDel.Enabled = true;
                        break;
                    case "45316":
                        btnDel.Enabled = true;
                        break;
                    default:
                        btnDel.Enabled = false;
                        break;
                }

                GstrBunSu.GstrMiaCode = "";
                btnOK.Enabled = false;
                //PanelMain.Enabled = false;
                cboClass.Focus();
                cboClass.Select();
                panel_main(false);

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

            if (sender == this.ssList1)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column); //sort 정렬 기능 
                    return;
                }
                if (e.RowHeader == true)
                {
                    return;
                }
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
                //frmPmpaViewGelSearch f = new frmPmpaViewGelSearch();
                //f.Show();

                frmPmpaViewGelSearchX = new frmPmpaViewGelSearch();
                frmPmpaViewGelSearchX.rGetData += new frmPmpaViewGelSearch.GetData(GetText);
                frmPmpaViewGelSearchX.rEventClose += new frmPmpaViewGelSearch.EventClose(frmPmpaViewGelSearchX_rEventExit);
                frmPmpaViewGelSearchX.Show();

                //if(frmPmpaViewGelSearchX != null)
                //{
                //    frmPmpaViewGelSearchX.Dispose();
                //    frmPmpaViewGelSearchX = null;
                //}
            }
            else if (sender == this.btnOK)
            {
                GetData(clsDB.DbCon, ssList1, ssList2, ssList3);
            }
            else if (sender == this.btnCancel)
            {
                btnOK.Enabled = false;
                screen_clear();
                //PanelMain.Enabled = false;
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
                GstrBunSu.GnWRTNO = 0;
                btnOK.Enabled = false;
                screen_clear("dtpdate");
                btnOK.Enabled = true;
                //PanelMain.Enabled = true;
                panel_main(true);
                TxtMisuID.Focus();

            }
            else if (sender == this.Menu1_2)
            {
                //PanelMain.Enabled = true;
                panel_main(true);
                TxtMisuID.Focus();
            }
            else if (sender == this.Menu1_3)
            {
                foreach (Form frm2 in Application.OpenForms) //중복로드 방지
                {
                    if (frm2.Name == "frmcomLibBMstSearch1")
                    {

                        frm2.Visible = true;
                        frm2.Activate();
                        return;
                    }
                }

                frmcomLibBMstSearch1 frm = new frmcomLibBMstSearch1(GstrBunSu,"BOHUM"); 
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(0, 50);
                frm.Show();
            }
            else if (sender == this.Menu1_4)
            {
                if (VB.Len(VB.Pstr(cboClass.Text, ".", 1).Trim()) == 2)
                {
                    foreach (Form frm2 in Application.OpenForms) //중복로드 방지
                    {
                        if (frm2.Name == "frmPmpaViewMisuNumSearch")
                        {

                            frm2.Visible = true;
                            frm2.Activate();
                            return;
                        }
                    }

                    frmPmpaViewMisuNumSearch frm = new frmPmpaViewMisuNumSearch(GstrBunSu, "BOHUM");
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = new Point(0, 50);
                    frm.Show();
                }
                else
                {
                    ComFunc.MsgBox("미수 종류 구분 오류 입니다...");
                    return;
                }
            }
            else if (sender == this.Menu2_1)
            {

            }
        }

        void eControl_GotFocus(object sender, EventArgs e)
        {
            if(sender == cboClass)
            {
                if(GstrBunSu.GnWRTNO != 0)
                {
                    Screen_Display(clsDB.DbCon,ssList1,ssList2,ssList3);
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
            if(sender == cboClass)
            {
                GstrBunSu.GstrClass = ComFunc.LeftH(cboClass.Text, 2);
            }else if(sender == dtpBDate)
            {
                if (cPM.DATA_CHECK(dtpBDate.Text, cpublic.strSysDate) == "NO")
                {
                    //dtpBDate.Focus();
                    return;
                }
            }else if(sender == TxtGelCode)
            {
               if(TxtGelCode.Text.Trim() == "")
                {
                    return;
                }
                lblMiaName.Text = cPM.READ_BAS_MIA(TxtGelCode.Text.Trim());
                if (lblMiaName.Text.CompareTo("") == 0)
                {
                    ComFunc.MsgBox("조합기호가 등록되지 않음", "확인");
                    TxtGelCode.Focus();
                    return;
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
                eSave(clsDB.DbCon,ssList2, nRowIndex, nCollndex);
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

        void eCboSelChanged(object sender, EventArgs e)
        {
            GstrBunSu.GstrClass = ComFunc.LeftH(cboClass.Text, 2);
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            {
                if (sender == cboClass)
                {
                    TxtMisuID.Focus();
                }
                else if (sender == TxtMisuID)
                {
                    TxtGelCode.Focus();
                }
                else if (sender == TxtGelCode)
                {
                    dtpBDate.Focus();
                }
                else if (sender == dtpBDate)
                {
                    cboIO.Focus();
                }
                else if (sender == cboIO)
                {
                    cboTongGbn.Focus();
                }
                else if(sender == cboTongGbn)
                {
                    cboBun.Focus();
                }
                else if (sender == cboBun)
                {
                    TxtJepsuNo.Focus();
                }
                else if (sender == TxtJepsuNo)
                {
                    TxtMirYYMM.Select();
                }
                else if(sender == TxtMirYYMM)
                {
                    ssList2.Focus();
                }
                                
            }
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.ssList2)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }

            }
        }

        void Spd_EditChange(object sender, EditorNotifyEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            string s = string.Empty;

            if (o.ActiveSheet.ActiveColumnIndex == 1)
            {
                int argRow = 0, argCol = 0;
                //string strData = "";

                argRow = o.ActiveSheet.ActiveRowIndex;
                argCol = o.ActiveSheet.ActiveColumnIndex;

                s += o.ActiveSheet.Cells[argRow,argCol].Text;  

                //if (VB.Len(s) == 4 || VB.Len(s) == 7)
                //{
                //    o.ActiveSheet.Cells[argRow, argCol].Text += "-";
                //    o.Select();
                //    o.EditMode = true;
                //}

                //if (VB.Len(s) >= 10)
                //{
                //    o.EditMode = false;
                //    o.Select();

                //}
            }
        }

        public void setClass(clsPmpaPb.GstrBunSu argBunSu)
        {
            GstrBunSu = argBunSu;
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
                TxtMisuID.Text = ""; lblSname.Text = "";
                TxtGelCode.Text = ""; lblMiaName.Text = "";
                TxtJepsuNo.Text = ""; lblWRTNO.Text = "";
                TxtMirYYMM.Text = ""; lblChasu.Text = "";
                lblMirNo.Text = ""; lblMukNo.Text = "";
                cboIO.SelectedIndex = -1;
                cboTongGbn.SelectedIndex = -1;
                cboBun.SelectedIndex = -1;
                dtpBDate.Text = "";

                cPM.TMM_Clear_Rtn();

                ssList2.ActiveSheet.RowCount = 0;
                ssList2.ActiveSheet.RowCount = 20;

                CS.Spread_Clear_Range(ssList1, 0, 0, ssList1.ActiveSheet.RowCount, ssList1.ActiveSheet.ColumnCount);

                ssList3.ActiveSheet.RowCount = 20;
                CS.Spread_Clear_Range(ssList3, 0, 0, ssList3.ActiveSheet.RowCount, ssList3.ActiveSheet.ColumnCount);

           
            }
            else if (Job == "dtpdate")
            {
                TxtMisuID.Text = ""; lblSname.Text = "";
                TxtGelCode.Text = ""; lblMiaName.Text = "";
                TxtJepsuNo.Text = ""; lblWRTNO.Text = "";
                TxtMirYYMM.Text = ""; lblChasu.Text = "";
                lblMirNo.Text = ""; lblMukNo.Text = "";
                cboIO.SelectedIndex = -1;
                cboTongGbn.SelectedIndex = -1;
                cboBun.SelectedIndex = -1;

                cPM.TMM_Clear_Rtn();

                ssList2.ActiveSheet.RowCount = 0;
                ssList2.ActiveSheet.RowCount = 20;

                CS.Spread_Clear_Range(ssList1, 0, 0, ssList1.ActiveSheet.RowCount, ssList1.ActiveSheet.ColumnCount);

                ssList3.ActiveSheet.RowCount = 20;
                CS.Spread_Clear_Range(ssList3, 0, 0, ssList3.ActiveSheet.RowCount, ssList3.ActiveSheet.ColumnCount);

            }
        }

        void Screen_Display(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd1, FarPoint.Win.Spread.FpSpread Spd2, FarPoint.Win.Spread.FpSpread Spd3)
        {
            int i = 0, j = 0;
            int nREAD = 0;
            int nLine = 0;
            string strRemark = "";
            string[] strRemarkL = null;

            DataTable dt = null;

            cPM.READ_MISU_IDMST(GstrBunSu.GnWRTNO);

            if (clsPmpaType.TMM.WRTNO.ToString().Trim() == "")
            {
                ComFunc.MsgBox("미수내역이 없습니다.", "확인");
            }

            TxtMisuID.Text = clsPmpaType.TMM.MisuID;
            lblWRTNO.Text = clsPmpaType.TMM.WRTNO.ToString();
            TxtGelCode.Text = clsPmpaType.TMM.GelCode.Trim();
            lblMiaName.Text = cPM.READ_BAS_MIA(clsPmpaType.TMM.GelCode);
            dtpBDate.Text = clsPmpaType.TMM.BDate;
            TxtJepsuNo.Text = clsPmpaType.TMM.JepsuNo.Trim();

            TxtMirYYMM.Text = clsPmpaType.TMM.MirYYMM.Trim();

            if (string.Compare(clsPmpaType.TMM.TongGbn, "1") < 0)
            {
                cboTongGbn.SelectedIndex = 0;
            }
            else
            {
                cboTongGbn.SelectedIndex = Convert.ToInt32(VB.Val(clsPmpaType.TMM.TongGbn)) - 1;
            }

            lblMirNo.Text = clsPmpaType.TMM.EdiMirNo;
            lblChasu.Text = clsPmpaType.TMM.ChaSu;
            lblMukNo.Text = clsPmpaType.TMM.MukNo;
            

            cboIO.SelectedIndex = cPM.ListIndex_MisuIO(clsPmpaType.TMM.IpdOpd);

            if (string.Compare(GstrBunSu.GstrClass, "05") < 0)
            {
                cboBun.SelectedIndex = cPM.ListIndex_MisuBun(clsPmpaType.TMM.Bun,"");
            }
            else if (string.Compare(GstrBunSu.GstrClass, "08") == 0)
            {
                cboBun.SelectedIndex = cPM.ListIndex_MisuSayu(clsPmpaType.TMM.Bun);
            }
            else
            {
                cboBun.SelectedIndex = -1;
            }

            lblSname.Text = "";

            //MISU_SLIP_Display
            dt = cSQL.sel_MisuMast1_1(pDbCon, GstrBunSu.GnWRTNO);

            nREAD = dt.Rows.Count;

            Spd2.ActiveSheet.RowCount = nREAD + 20;

            for (i = 0; i < nREAD; i++)
            {
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Bdate].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Gubun].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.GubunName].Text = cPM.READ_MisuGye_MISU(dt.Rows[i]["gubun"].ToString().Trim(),"");
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Qty].Text = dt.Rows[i]["Qty"].ToString().Trim();
                //Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.TAmt].Text = String.Format("{0:###,###,###,##0}",VB.Val(dt.Rows[i]["TAmt"].ToString()));
                //Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Amt].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["Amt"].ToString()));
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.TAmt].Text = dt.Rows[i]["TAmt"].ToString();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Amt].Text = dt.Rows[i]["Amt"].ToString();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Remark].Text = dt.Rows[i]["Remark"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Chasu].Text = dt.Rows[i]["CHASU"].ToString().Trim();

                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.OldBdate].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.OldGubun].Text = dt.Rows[i]["gubun"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.OldQty].Text = dt.Rows[i]["Qty"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.OldTAmt].Text = dt.Rows[i]["TAmt"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.OldAmt].Text = dt.Rows[i]["Amt"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.OldRemark].Text = VB.Left(dt.Rows[i]["Remark"].ToString().Trim(),30);
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.OldChasu].Text = dt.Rows[i]["CHASU"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.EntDate].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.EntPart].Text = dt.Rows[i]["EntPart"].ToString().Trim();
                
                //Row 높이 설정 2020-08-26 
                FarPoint.Win.Spread.Row row;
                row = Spd2.ActiveSheet.Rows[i];
                float rowSize = row.GetPreferredHeight();
                row.Height = rowSize;

                //Spd2.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);

                clsPmpaType.TMS.Del[i] = " ";
                clsPmpaType.TMS.Gubun[i] = dt.Rows[i]["Gubun"].ToString().Trim();
                clsPmpaType.TMS.Qty[i] = Convert.ToInt32(VB.Val(dt.Rows[i]["Qty"].ToString().Trim()));
                clsPmpaType.TMS.TAmt[i] = Convert.ToInt32(VB.Val(dt.Rows[i]["TAmt"].ToString().Trim()));
                clsPmpaType.TMS.Amt[i] = Convert.ToInt32(VB.Val(dt.Rows[i]["Amt"].ToString().Trim()));

                nLine = 1;

                strRemark = dt.Rows[i]["Remark"].ToString().Trim();
                strRemarkL = new string[strRemark.Length + 1];
                strRemarkL = VB.Split(strRemark, ComNum.VBLF);

                if (VB.UBound(strRemarkL) > 0)
                {
                    nLine = VB.UBound(strRemarkL) + 1;

                    for (j = 0; j < VB.UBound(strRemarkL); j++)
                    {
                        if (ComFunc.LenH(strRemarkL[j]) > 46)
                        {
                            nLine = nLine + 1;
                        }
                    }
                }
                else
                {
                    if (VB.Len(strRemark) > 46)
                    {
                        nLine = nLine + 1;
                    }
                }

                if (nLine == 0)
                {
                    nLine = 1;
                }

                Spd2.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT -3);

            }

            dt.Dispose();
            dt = null;

            Amt_Account();

            //MISU_MONTHLY Display
            dt = cSQL.sel_MisuMast1_2(pDbCon,GstrBunSu.GnWRTNO);

            nREAD = dt.Rows.Count;

            Spd3.ActiveSheet.RowCount = nREAD;

            for (i = 0; i < nREAD; i++)
            {
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_3.YYMM].Text = dt.Rows[i]["YYMM"].ToString().Trim();
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_3.IwolAmt].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["IwolAmt"].ToString()));
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_3.MisuAmt].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["MisuAmt"].ToString())); 
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_3.IpgumAmt].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["IpgumAmt"].ToString())); 
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_3.SakAmt].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["SakAmt"].ToString())); 
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_3.SakAmt2].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["SakAmt2"].ToString()));
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_3.BanAmt].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["BanAmt"].ToString()));
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_3.EtcAmt].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["EtcAmt"].ToString()));
                Spd3.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_3.JanAmt].Text = String.Format("{0:###,###,###,##0}", VB.Val(dt.Rows[i]["JanAmt"].ToString()));
                Spd3.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
            }

            dt.Dispose();
            dt = null;
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd1, FarPoint.Win.Spread.FpSpread Spd2, FarPoint.Win.Spread.FpSpread Spd3)
        {
            int i = 0;
            string strROWID = "", strIdChange = "", strSlipChange = "", strDel = "", strBdate1 = "";
            string strGubun1 = "", strRemark1 = "", strChasu1 = "";
            double nQty1 = 0, nTAmt1 = 0, nAmt1 = 0;
            string strBdate2 = "", strGubun2 = "", strRemark2 = "", strChasu2="", strEntDate2 = "";
            double nQty2 = 0, nTAmt2 = 0, nAmt2 = 0;
            long nEntPart2 = 0;
            string strFromDate = "", strToDate = "";
            string Time = "";

            string SqlErr = ""; //에러문 받는 변수
            string SQL = "";
            int intRowAffected = 0;
            DataTable dt = null;

            //if(cPM.CHECK_MISU_MAGAM_SPREAD(ssList2,1) == false)
            //{
            //    return;
            //}

            Invalid_Data_Check();

            clsDB.setBeginTran(pDbCon);

            #region CmdOK_IDMST_RTN
            clsPmpaType.TMN.WRTNO = clsPmpaType.TMM.WRTNO;
            clsPmpaType.TMN.MisuID = TxtMisuID.Text.Trim();
            clsPmpaType.TMN.BDate = dtpBDate.Text.Trim();
            //clsPmpaType.TMN.BDate = VB.Val(dtpBDate.Text);
            clsPmpaType.TMN.Class = VB.Left(cboClass.Text, 2);
            clsPmpaType.TMN.IpdOpd = VB.Left(cboIO.Text, 1);
            clsPmpaType.TMN.GelCode = TxtGelCode.Text.Trim();
            clsPmpaType.TMN.Bun = VB.Left(cboBun.Text, 2);
            clsPmpaType.TMN.JepsuNo = TxtJepsuNo.Text.Trim();
            clsPmpaType.TMN.FromDate = "";
            clsPmpaType.TMN.ToDate = "";
            clsPmpaType.TMN.Ilsu = 0;
            clsPmpaType.TMN.DeptCode = "";
            clsPmpaType.TMN.MgrRank = "1";
            clsPmpaType.TMN.Remark = "";
            clsPmpaType.TMN.GbEnd = "1";
            clsPmpaType.TMN.TongGbn = VB.Left(cboTongGbn.Text, 1);
            clsPmpaType.TMN.MirYYMM = TxtMirYYMM.Text.Trim();

            if ( clsPmpaType.TMN.JAmt < 1)
            {
                clsPmpaType.TMN.GbEnd = "0";
            }

            if (clsPmpaType.TMN.FromDate.Trim() == "")
            {
                strFromDate = "";
            }
            else
            {
                strFromDate = clsPmpaType.TMN.FromDate;
            }

            if(clsPmpaType.TMN.ToDate.Trim() == "")
            {
                strToDate = "";
            }
            else
            {
                strToDate = clsPmpaType.TMN.ToDate;
            }

            read_sysdate();

            if(clsPmpaType.TMN.WRTNO == 0)
            {
                #region CmdOK_IDMST_INSET 신규등록
                strIdChange = "NO";

                dt = cSQL.sel_MisuMast1_CmdOK_IDMST_INSERT(pDbCon);

                if(dt.Rows.Count == 0)
                {
                    clsPmpaType.TMN.WRTNO = 1;
                }
                else
                {
                    clsPmpaType.TMN.WRTNO = Convert.ToInt32(VB.Val(dt.Rows[i]["mWRTNO"].ToString().Trim())) + 1;
                }
                 
                dt.Dispose();
                dt = null;

                try
                {
                    SqlErr = cSQL.Int_MisuMast1_CmdOK_IDMST_INSERT(pDbCon, strFromDate, strToDate, ref intRowAffected);

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
                    #endregion
            }
            else
            {
                #region CmdOK_IDMST_UPDATE
                strIdChange = "NO";

                if(clsPmpaType.TMM.MisuID != clsPmpaType.TMN.MisuID){ strIdChange = "OK"; }
                if(clsPmpaType.TMM.GelCode != clsPmpaType.TMN.GelCode) { strIdChange = "OK"; }
                if(clsPmpaType.TMM.BDate != clsPmpaType.TMN.BDate) { strIdChange = "OK"; }
                if(clsPmpaType.TMM.IpdOpd != clsPmpaType.TMN.IpdOpd) { strIdChange = "OK"; }
                if(clsPmpaType.TMM.Bun != clsPmpaType.TMN.Bun) { strIdChange = "OK"; }
                if(clsPmpaType.TMM.JepsuNo != clsPmpaType.TMN.JepsuNo) { strIdChange = "OK"; }
                if(clsPmpaType.TMM.TongGbn != clsPmpaType.TMN.TongGbn) { strIdChange = "OK"; }
                if(clsPmpaType.TMM.MirYYMM != clsPmpaType.TMN.MirYYMM) { strIdChange = "OK"; }

                try
                {

                    SqlErr = cSQL.Upt_MisuMast1_CmdOK_IDMST_UPDATE(pDbCon,strIdChange,strFromDate,strToDate, ref intRowAffected);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        ComFunc.MsgBox("MISU_IDMST 변경 등록시 오류가 발생함", "확인");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                        return;
                    }

                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(ex.Message);
                }

                if(strIdChange == "OK")
                {
                    try
                    {

                        SqlErr = cSQL.Upt_MisuMast1_CmdOK_IDMST_UPDATE2(pDbCon, ref intRowAffected);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            ComFunc.MsgBox("MISU_SLIP 변경 등록시 오류가 발생함", "확인");
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
                #endregion END_CmdOK_IDMST_UPDATE

                #region CmdOK_IDMST_History 변경내역을 등록함

                if (strIdChange == "NO")
                {
                   
                }
                else
                {
                    try
                    {
                        SqlErr = cSQL.Int_MisuMast1_CmdOK_IDMST_History(pDbCon, ref intRowAffected);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            ComFunc.MsgBox("MISU_HISTORY INSERT  오류가 발생함", "확인");
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
                #endregion END_CmdOK_IDMST_History 변경내역을 등록함
            }
            #endregion END_CmdOK_IDMST_RTN 

            #region CmdOK_Slip_RTN SLIP 변경분 UPDATE
            //수정,변경분 INSERT
            for (i = 0; i < Spd2.ActiveSheet.RowCount; i++)
            {
                strDel = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.A].Text.Trim();
                strBdate1 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Bdate].Text.Trim();
                strGubun1 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Gubun].Text.Trim();
                nQty1 = Convert.ToInt32(VB.Val(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Qty].Text.Trim()));
                nTAmt1 = Convert.ToInt32(VB.Replace(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.TAmt].Text.Trim(),",",""));
                nAmt1 = Convert.ToInt32(VB.Replace(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Amt].Text.Trim(),",",""));
                strRemark1 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Remark].Text;
                strChasu1 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Chasu].Text.Trim();

                strROWID = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.ROWID].Text.Trim();
                strBdate2 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.OldBdate].Text.Trim();
                strGubun2 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.OldGubun].Text.Trim();
                nQty2 = Convert.ToInt32(VB.Val(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.OldQty].Text.Trim()));
                nTAmt2 = Convert.ToInt32(VB.Replace(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.OldTAmt].Text.Trim(),",",""));
                nAmt2 = Convert.ToInt32(VB.Replace(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.OldAmt].Text.Trim(),",",""));
                strRemark2 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.OldRemark].Text;
                strChasu2 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.OldChasu].Text.Trim();
                strEntDate2 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.EntDate].Text.Trim();
                nEntPart2 = Convert.ToInt32(VB.Val(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.EntPart].Text.Trim()));

                if(strDel == "True")  //삭제인경우
                {
                    if(strROWID != "")
                    {
                        #region CmdOK_MisuSlip_Delete
                        try
                        {
                            SqlErr = cSQL.Del_MisuMast1_CmdOK_MisuSlip_Delete(pDbCon, strROWID, ref intRowAffected);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox("MISU_SLIP OLD자료 Delete ERROR", "확인");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(ex.Message);
                        }
                        //Slip History에 Insert
                        try
                        {
                            Time = VB.Left(cpublic.strSysTime, 5);
                            SqlErr = cSQL.Int_MisuMast1_CmdOK_MisuSlip_History(pDbCon,cpublic.strSysDate,Time,strBdate2,strGubun2,nQty2,nTAmt2,nAmt2,strRemark2,strEntDate2,nEntPart2, ref intRowAffected);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
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
                        #endregion End_CmdOK_MisuSlip_Delete
                    }
                }
                else if(strDel != "True")
                {
                    if(strROWID == "")
                    {
                        #region CmdOK_MisuSlip_Insert
                        if(strBdate1 == "" && strGubun1 == "")
                        {
                            
                        }
                        else
                        {
                            try
                            {
                                Time = VB.Left(cpublic.strSysTime, 5);
                                SqlErr = cSQL.Int_MisuMast1_CmdOK_MisuSlip_Insert(pDbCon, strBdate1, strGubun1, nQty1, nTAmt1, nAmt1, strRemark1, strChasu1, cpublic.strSysDate, Time, ref intRowAffected);

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
                        }
                        #endregion END_CmdOK_MisuSlip_Insert
                    }
                    else
                    {
                        strSlipChange = "NO";
                        if(strGubun1 != strGubun2) { strSlipChange = "OK"; }
                        if(strBdate1 != strBdate2) { strSlipChange = "OK"; }
                        if(nQty1 != nQty2) { strSlipChange = "OK"; }
                        if(nTAmt1 != nTAmt2) { strSlipChange = "OK"; }
                        if(nAmt1 != nAmt2) { strSlipChange = "OK"; }
                        if(strRemark1 != strRemark2) { strSlipChange = "OK"; }
                        if(strChasu1 != strChasu2) { strSlipChange = "OK"; }
                        if(strSlipChange == "OK")
                        {
                            #region CmdOK_MisuSlip_Update
                            try
                            {
                                Time = VB.Left(cpublic.strSysTime, 5);
                                SqlErr = cSQL.Upt_MisuMast1_CmdOK_MisuSlip_Update(pDbCon,strBdate1,strGubun1,nQty1,nTAmt1,nAmt1,strChasu1,strRemark1,cpublic.strSysDate,Time,strROWID, ref intRowAffected);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox("MISU_SLIP INSERT ERROR", "확인");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                    return;
                                }

                                SqlErr = cSQL.Int_MisuMast1_CmdOK_MisuHISTORY_insert(pDbCon, cpublic.strSysDate, Time, strBdate2, nQty2, nTAmt2, nAmt2, strGubun2, strRemark2, nEntPart2, strEntDate2, ref intRowAffected);

                                if(SqlErr != "")
                                {
                                    clsDB.setRollbackTran(pDbCon);
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
                            #endregion END_CmdOK_MisuSlip_Update
                        }
                    }
                }
            }
            #endregion

            clsDB.setCommitTran(pDbCon);

            //화면 Clear & 각종 Enable Set
            btnOK.Enabled = false;
            screen_clear("dtpdate");
            //PanelMain.Enabled = false;
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

            strMsg = "미수내역을 삭제 하시겠습니까?";

            if (ComFunc.MsgBoxQ(strMsg, "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            clsDB.setBeginTran(pDbCon);

            try
            {
                //IDMST
                SqlErr = cSQL.Del_MisuMast1_1(pDbCon, lblWRTNO.Text, ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("MISU_IDMST DELETE ERROR", "확인");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }

                //HISTORY Insert
                Time = VB.Left(cpublic.strSysTime, 5);
                SqlErr = cSQL.Int_MisuMast1_1(pDbCon, cpublic.strSysDate, Time, ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("MISU_HISTORY INSERT 오류가 발생함", "확인");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }

                //삭제할 SLIP을 History에 Insert
                
                SqlErr = cSQL.Int_MisuMast1_3(pDbCon, cpublic.strSysDate, Time, ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("MISU_HISTORY INSERT 오류가 발생함", "확인");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }

                //SLip을 삭제함
                SqlErr = cSQL.Del_MisuMast1_4(pDbCon, ref intRowAffected);

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

            btnOK.Enabled = false;
            screen_clear();
            //PanelMain.Enabled = false;
            panel_main(false);
            Menu1_1.Enabled = true;
            Menu1_2.Enabled = false;
            Menu1_3.Enabled = true;
            cboClass.Enabled = true;
            cboClass.Focus();
        }

        void eSave(PsmhDb pDbCon, FpSpread spd, int argRow, int argCol)
        {
            string strData = "";

            if(argCol == 7)
            {
                SendKeys.Send("{ENTER}");
            }

            if(argCol == 0 || argCol == 3)
            {
                return;
            }

            if (argCol == 1)
            {
                strData = ssList2.ActiveSheet.Cells[argRow,(int)clsComPmpaSpd.enmPmpaMisuMast1_2.Bdate].Text.Trim();

                if (strData != "")
                {
                    if (cPM.DATA_CHECK(strData, cpublic.strSysDate) == "NO")
                    {
                        return;
                    }

                    if (string.Compare(strData, dtpBDate.Text) < 0)
                    {
                        ComFunc.MsgBox("입금일자는 청구일자 보다 작을 수 없습니다", "확인");
                        ssList2.ActiveSheet.SetActiveCell(argRow, argCol);
                        return;
                    }
                }
                ssList2.ActiveSheet.SetActiveCell(argRow, argCol + 1);
            }

            if (argCol == 2)
            {
                strData = spd.ActiveSheet.Cells[argRow, argCol].Text.Trim();

                if(strData != "")
                {
                    if(strData == "99")
                    {
                        strData = SAKGAM_CALC_Rtn(argRow);
                    }
                    strData = spd.ActiveSheet.Cells[argRow, 2].Text = VB.Val(spd.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Gubun].Text.Trim()).ToString("00");
                    spd.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.GubunName].Text = cPM.READ_MisuGye_MISU(strData,"");

                    if (cPM.READ_MisuGye_MISU(strData, "") == "")
                    {
                        ComFunc.MsgBox("구분이 오류입니다.", "확인");
                        //TODO
                        //SS2.Col = 3: SS2.Action = SS_ACTION_ACTIVE_CELL
                        return;
                    }else
                    {
                        
                    }
                }
            }
            
            if(argCol == 2 || argCol == 4 || argCol == 5 || argCol == 6)
            {
                switch (argCol)
                {
                    case 2: clsPmpaType.TMS.Gubun[argRow] = spd.ActiveSheet.Cells[argRow, argCol].Text;
                        ssList2.ActiveSheet.SetActiveCell(argRow, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Qty);
                        break;
                    case 4: clsPmpaType.TMS.Qty[argRow] = Convert.ToInt32(VB.Val(spd.ActiveSheet.Cells[argRow, argCol].Text.Trim()));
                        ssList2.ActiveSheet.SetActiveCell(argRow, argCol + 1);
                        break;
                    case 5: clsPmpaType.TMS.TAmt[argRow] = Convert.ToInt32(VB.Val(spd.ActiveSheet.Cells[argRow, argCol].Text.Trim()));
                        ssList2.ActiveSheet.SetActiveCell(argRow, argCol + 1);
                        break;
                    case 6: clsPmpaType.TMS.Amt[argRow] = Convert.ToInt32(VB.Val(spd.ActiveSheet.Cells[argRow, argCol].Text.Trim()));
                        ssList2.ActiveSheet.SetActiveCell(argRow, argCol + 1);
                        break;
                }

                Amt_Account();
            }

            //2021-08-02 직요 길이 999로 btye 확장으로 인한 주석 처리 
            //if (argCol == 6)
            //{
            //    int bytecount = System.Text.Encoding.Default.GetByteCount(ssList2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Remark].Text);
            //    if (bytecount >= 60)
            //    {
            //        ComFunc.MsgBox("REMARK 적요 길이가 너무 큽니다.", "확인");
            //        ssList2.ActiveSheet.SetActiveCell(argRow, argCol);
            //        ssList2.ActiveSheet.Cells[argRow, argCol].Text = strRemark;
            //    }                
            //}  
        }

        void Amt_Account() 
        {
            int i = 0;

            for (i = 0; i < 8; i++)
            {
                clsPmpaType.TMN.Amt[i] = 0;
            }

            for (i = 0; i < 5; i++)
            {
                clsPmpaType.TMN.Qty[i] = 0;
            }

            for (i = 0; i < 200; i++)
            {
                if(clsPmpaType.TMS.Del[i] != "True" && clsPmpaType.TMS.Amt[i] != 0)
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
                            clsPmpaType.TMN.Amt[0] = clsPmpaType.TMN.Amt[0] + clsPmpaType.TMS.TAmt[i];
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
                            clsPmpaType.TMN.Qty[4] = clsPmpaType.TMN.Qty[4] + clsPmpaType.TMS.Qty[i];
                            clsPmpaType.TMN.Amt[4] = clsPmpaType.TMN.Amt[4] + clsPmpaType.TMS.Amt[i];
                            break; //반송
                        case "33":
                            clsPmpaType.TMN.Amt[5] = clsPmpaType.TMN.Amt[5] + clsPmpaType.TMS.Amt[i];
                            break; //과지급금
                        case "35":
                            clsPmpaType.TMN.Qty[3] = clsPmpaType.TMN.Qty[3] + clsPmpaType.TMS.Qty[i];
                            clsPmpaType.TMN.Amt[7] = clsPmpaType.TMN.Amt[7] + clsPmpaType.TMS.Amt[i];
                            break; //절사삭감
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

            clsPmpaType.TMN.JAmt = clsPmpaType.TMN.Amt[1];
            
            for(i = 2 ; i < 8 ; i++)
            {
                clsPmpaType.TMN.JAmt = clsPmpaType.TMN.JAmt -  clsPmpaType.TMN.Amt[i];
            }

            ssList1.ActiveSheet.Cells[0, 1].Text = VB.Format(clsPmpaType.TMN.Qty[0], "###,##0"); //청구건수
            ssList1.ActiveSheet.Cells[0, 2].Text = VB.Format(clsPmpaType.TMN.Qty[1], "###,##0"); //입금건수
            ssList1.ActiveSheet.Cells[0, 3].Text = VB.Format(clsPmpaType.TMN.Qty[2], "###,##0"); //삭감건수
            ssList1.ActiveSheet.Cells[0, 4].Text = VB.Format(clsPmpaType.TMN.Qty[3], "###,##0"); //절사삭감건수
            ssList1.ActiveSheet.Cells[0, 5].Text = VB.Format(clsPmpaType.TMN.Qty[4], "###,##0"); //반송건수

            ssList1.ActiveSheet.Cells[1, 0].Text = VB.Format(clsPmpaType.TMN.Amt[0], "###,###,###,##0"); //총진료비
            ssList1.ActiveSheet.Cells[1, 1].Text = VB.Format(clsPmpaType.TMN.Amt[1], "###,###,###,##0"); //미수
            ssList1.ActiveSheet.Cells[1, 2].Text = VB.Format(clsPmpaType.TMN.Amt[2], "###,###,###,##0"); //입금
            ssList1.ActiveSheet.Cells[1, 3].Text = VB.Format(clsPmpaType.TMN.Amt[3], "###,###,###,##0"); //삭감
            ssList1.ActiveSheet.Cells[1, 4].Text = VB.Format(clsPmpaType.TMN.Amt[7], "###,###,###,##0"); //절사삭감
            ssList1.ActiveSheet.Cells[1, 5].Text = VB.Format(clsPmpaType.TMN.Amt[4], "###,###,###,##0"); //반송
            ssList1.ActiveSheet.Cells[1, 6].Text = VB.Format(clsPmpaType.TMN.Amt[5], "###,###,###,##0"); //과지급
            ssList1.ActiveSheet.Cells[1, 7].Text = VB.Format(clsPmpaType.TMN.Amt[6], "###,###,###,##0"); //계산착오
            ssList1.ActiveSheet.Cells[1, 8].Text = VB.Format(clsPmpaType.TMN.JAmt, "###,###,###,##0");

        }

        void Invalid_Data_Check() //오류 Data Check
        {
            if(ComFunc.LeftH(cboClass.Text,2).Trim() == "")
            {
                ComFunc.MsgBox("미수종류가 공란입니다.", "확인");
                return;
            }

            if(TxtMisuID.Text.Trim() == "")
            {
                ComFunc.MsgBox("미수번호가 공란입니다", "확인");
                return;
            }

            if(TxtGelCode.Text.Trim() == "")
            {
                ComFunc.MsgBox("계약처 코드가 공란입니다.", "확인");
                return;
            }
            
            if(dtpBDate.Text.Trim() == "")
            {
                ComFunc.MsgBox("청구일자가 공란입니다.", "확인");
                return;
            }

            if(VB.Left(cboIO.Text,1) != "O" && VB.Left(cboIO.Text,1) != "I")
            {
                ComFunc.MsgBox("외래,입원 선택이 오류입니다.", "확인");
                return;
            }

            if(cboTongGbn.Text.Trim() == "")
            {
                ComFunc.MsgBox("통계구분이 공란입니다.", "확인");
                return;
            }

            TxtMirYYMM.Text = TxtMirYYMM.Text.Trim();

            if(VB.Len(TxtMirYYMM.Text) != 6)
            {
                ComFunc.MsgBox("통계월을 YYYYMM형식으로 입력하세요.", "확인");
                return;
            }

            if(Convert.ToInt32(VB.Right(TxtMirYYMM.Text, 2)) >= 1 && Convert.ToInt32(VB.Right(TxtMirYYMM.Text, 2)) <= 12)
            {

            }
            else
            {
                ComFunc.MsgBox("통계월의 월이 오류 입니다.(1-12월)", "확인");
                return;
            }

            if (Convert.ToInt32(VB.Left(TxtMirYYMM.Text, 4)) >= 1900 && Convert.ToInt32(VB.Left(TxtMirYYMM.Text, 4)) <= 2999)
            {

            }
            else
            {
                ComFunc.MsgBox("통계월의 년도가 오류 입니다.(1900-2999년)", "확인");
                return;
            }

            return;
        }

        void panel_main(bool argGUBUN)
        {
            TxtMisuID.Enabled = argGUBUN;
            TxtGelCode.Enabled = argGUBUN;
            dtpBDate.Enabled = argGUBUN;
            cboIO.Enabled = argGUBUN;
            cboTongGbn.Enabled = argGUBUN;
            cboBun.Enabled = argGUBUN;
            TxtJepsuNo.Enabled = argGUBUN;
            TxtMirYYMM.Enabled = argGUBUN;
            btnHelp.Enabled = argGUBUN;

            TxtMisuID.BackColor = Color.White;
            TxtGelCode.BackColor = Color.White;
            cboIO.BackColor = Color.White;
            cboTongGbn.BackColor = Color.White;
            cboBun.BackColor = Color.White;
            TxtJepsuNo.BackColor = Color.White;
            TxtMirYYMM.BackColor = Color.White;
            btnHelp.BackColor = Color.White;
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

        public string SAKGAM_CALC_Rtn(int argRow)
        {
            int i = 0;
            double nWorkSak1 = 0, nWorkSak2 = 0;
            bool nSimFLAG;
            string argVal = "";

            nSimFLAG = false;

            for (i = 0; i < 200; i++)
            {
                if (clsPmpaType.TMS.Del[i] != "True" && string.Compare(clsPmpaType.TMS.Gubun[i], "09") > 0)
                {
                    if (clsPmpaType.TMS.Gubun[i] == "26")
                    {
                        nSimFLAG = true;
                    }
                }
            }

            for (i = 0; i < 200; i++)
            {
                if (clsPmpaType.TMS.Del[i] != "True" && string.Compare(clsPmpaType.TMS.Gubun[i], "09") > 0)
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
                            nWorkSak1 = nWorkSak1 + clsPmpaType.TMS.TAmt[i];
                            nWorkSak2 = nWorkSak2 + clsPmpaType.TMS.Amt[i];
                            break; //청구 미수액
                        case "26":
                            nWorkSak1 = nWorkSak1 - clsPmpaType.TMS.TAmt[i];
                            nWorkSak2 = nWorkSak2 - clsPmpaType.TMS.Amt[i];
                            break; //심사중입금
                        case "21":
                        case "22":
                        case "23":
                        case "24":
                        case "25":
                        case "27":
                        case "28":
                        case "29":
                            nWorkSak2 = nWorkSak2 - clsPmpaType.TMS.Amt[i];
                            break; //입금액
                        case "31":
                        case "35":
                            nWorkSak1 = nWorkSak1 - clsPmpaType.TMS.TAmt[i];
                            nWorkSak2 = nWorkSak2 - clsPmpaType.TMS.Amt[i];
                            break; //삭감
                        case "32":
                            nWorkSak1 = nWorkSak1 - clsPmpaType.TMS.TAmt[i];
                            nWorkSak2 = nWorkSak2 - clsPmpaType.TMS.Amt[i];
                            break; //반송
                        case "33":
                        case "34":
                            nWorkSak2 = nWorkSak2 - clsPmpaType.TMS.Amt[i];
                            break; //과지급금, 계산착오
                        case "10":
                            if (nSimFLAG == false)
                            {
                                nWorkSak1 = nWorkSak1 - clsPmpaType.TMS.TAmt[i];
                                nWorkSak2 = nWorkSak2 - clsPmpaType.TMS.Amt[i];
                            }
                            break; //심사중
                    }
                }
            }

            ssList2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Gubun].Text = "35";
            ssList2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.TAmt].Text = VB.Format(nWorkSak1, "###########0");
            ssList2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Amt].Text = VB.Format(nWorkSak2, "###########0");
            clsPmpaType.TMS.Gubun[argRow] = "35";
            clsPmpaType.TMS.TAmt[argRow] = nWorkSak1;
            clsPmpaType.TMS.Amt[argRow] = nWorkSak2;

            return argVal;
        }

        private void ssList2_IpgoEntryForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int nRow = 0;
                int nCol = 0;

                nRow = ssList2_Sheet1.ActiveRowIndex;
                nCol = ssList2_Sheet1.ActiveColumnIndex;

                if (nCol == (int)clsComPmpaSpd.enmPmpaMisuMast1_2.Remark)
                {
                    ssList2_Sheet1.SetActiveCell(nRow + 1, 0);
                }
                else
                {
                    ssList2_Sheet1.SetActiveCell(nRow, nCol + 1);
                }
            }
        }

    }
}
