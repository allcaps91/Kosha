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
    /// File Name       : frmPmpaMisuMast2.cs
    /// Description     : 미수원장관리
    /// Author          : 김해수
    /// Create Date     : 2018-12-12
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= "\misu\FrmMUMAUN02.frm(FrmMisuMast2) >> frmPmpaMisuMast2.cs 폼이름 재정의" />
    /// 
    public partial class frmPmpaMisuMast2 : Form
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

        string GstrMisuGbn = "SANJE";  //디버깅용
        //string GstrMisuGbn = "TA";  //디버깅용
        string strRemark = "";// remark 임시저장용
        string GstrClass = "";

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

        public frmPmpaMisuMast2()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaMisuMast2(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        } 

        public frmPmpaMisuMast2(string argMisuGbn)
        {
            GstrMisuGbn = argMisuGbn;
            InitializeComponent();
            setEvent();
        } //미수 구분자를 받아서 가져옴 

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
            this.btnNext.Click += new EventHandler(eBtnClick);
            this.btnDel.Click += new EventHandler(eBtnClick);
            this.btnHelp.Click += new EventHandler(eBtnClick);           

            //this.btnSave.Click += new EventHandler(eBtnSave);
            this.Menu1_1.Click += new EventHandler(eMenuClick);
            this.Menu1_2.Click += new EventHandler(eMenuClick);
            this.Menu1_3.Click += new EventHandler(eMenuClick);
            this.Menu1_4.Click += new EventHandler(eMenuClick);

            this.btnHelp.GotFocus += new EventHandler(eControl_GotFocus);
            this.cboClass.GotFocus += new EventHandler(eControl_GotFocus);
            this.cboClass.LostFocus += new System.EventHandler(eControl_LostFocus);
            this.TxtMisuID.LostFocus += new System.EventHandler(eControl_LostFocus);
            this.CboDept.LostFocus += new System.EventHandler(eControl_LostFocus);
            this.TxtGelCode.LostFocus += new System.EventHandler(eControl_LostFocus);

            this.cboClass.SelectedIndexChanged += new EventHandler(eCboSelChanged);

            this.TxtMisuID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.TxtGelCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.dtpBDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.cboBun.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.cboIO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.cboTongGbn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.cboMgrRank.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.TxtMirYYMM.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.DtpFDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.DtpTDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.DtpSagoDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.DtpGasiDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.TxtCarNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.TxtGaheja.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.cboClass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.CboDept.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);
            this.CboDrCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.eTxtKeyPress);

            this.ssList2.EditModeOn += new EventHandler(eControl_EditModeon);
            this.ssList2.EditModeOff += new EventHandler(eControl_EditModeoff);
            this.ssList2.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);
            this.ssList2.EditChange += new EditorNotifyEventHandler(Spd_EditChange);

        }

        void setCombo()
        {
            int i = 0;

            cboClass.Items.Clear();
            
            if(GstrMisuGbn == "SANJE")
            {
                cboClass.Items.Add("05.산재");
            }
            else if(GstrMisuGbn == "TA")
            {
                cboClass.Items.Add("07.자보");
            }
            else if(GstrMisuGbn == "PO")
            {
                cboClass.Items.Add("08.계약처");
            }

            cboClass.SelectedIndex = 0;


            GstrClass = VB.Left(cboClass.Text, 2);

            cboBun.Items.Clear();
            for(i = 0; i < cMisuMst.GstrMisuBun.Length; i++)
            {
                cboBun.Items.Add(cMisuMst.GstrMisuBun[i]);
            }

            cboIO.Items.Clear();
            for (i = 0; i < cMisuMst.GstrMisuIO.Length; i++)
            {
                cboIO.Items.Add(cMisuMst.GstrMisuIO[i]);
            }

            CboDept.Items.Clear();
            for (i = 0; i < cMisuMst.GstrDept.Length; i++)
            {
                if(cMisuMst.GstrDept[i] != "")
                {
                    CboDept.Items.Add(cMisuMst.GstrDept[i]);
                }
            }

            cboTongGbn.Items.Clear();
            cboTongGbn.Items.Add("1.퇴원청구");
            cboTongGbn.Items.Add("2.중간청구");
            cboTongGbn.Items.Add("3.재청구");
            cboTongGbn.Items.Add("4.추가청구");
            cboTongGbn.Items.Add("5.이의신청");
            cboTongGbn.Items.Add("6.기타청구");

            cboMgrRank.Items.Add("A.분쟁심의회상정(중)");
            cboMgrRank.Items.Add("B.환자수납예정");
            cboMgrRank.Items.Add("C.자문보류중");
            cboMgrRank.Items.Add("D.소송중");
            cboMgrRank.Items.Add("E.이의제기중");
            cboMgrRank.Items.Add("1.완불가능");
            cboMgrRank.Items.Add("2.독려시입금가능");
            cboMgrRank.Items.Add("3.대손처리예상");
            cboMgrRank.Items.Add("4.재판중");
            cboMgrRank.Items.Add("5.재산압류");
            cboMgrRank.Items.Add("6.분쟁심의회상정");
            cboMgrRank.Items.Add("7.일부입금");
            cboMgrRank.Items.Add("8.문제건");
            cboMgrRank.Items.Add("9.기타");

            cboMgrRank.SelectedIndex = 0;

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
                MagamSpd.sSpd_PmpaMisuMast2(ssList2, MagamSpd.senmPmpaMisuMast2, MagamSpd.nenmPmpaMisuMast2, 15, 0);

                CPP = new clsPmpaPb();
                cMisuMst = CPP.INITIAL_SET(clsDB.DbCon, "TA");
                GstrBunSu = new clsPmpaPb.GstrBunSu();

                btnOK.Enabled = false;
                cboClass.Text = "";
                cboBun.Text = "";
                cboIO.Text = "";
                cboTongGbn.Text = "";
                CboDept.Text = "";
                CboDrCode.Text = "";
                panel_main(false);
                cboMgrRank.Text = "";

                //툴팁
                setTxtTip();

                read_sysdate();

                screen_clear();

                setCtrlData();

                //설정정보 체크
                setCtrlInit();

                btnOK.Enabled = false;
                panel_main(false);
                if (GstrMisuGbn == "SANJE")
                {
                    lblCarNo.Text = "근무회사";
                    lblGaheja.Text = "";
                }
                else if (GstrMisuGbn == "PO")
                {
                    lblSagoDate.Visible = false;
                    lblCarNo.Visible = false;
                    lblGasiDate.Visible = false;
                    lblGaheja.Visible = false;
                    DtpSagoDate.Visible = false;
                    DtpGasiDate.Visible = false;
                    TxtCarNo.Visible = false;
                    TxtGaheja.Visible = false;
                }

                switch (clsType.User.IdNumber)
                {
                    case "4349":
                    case "11701":
                    case "23417":
                    case "20175":
                    case "38358":
                    case "36550":
                        //case "45316":
                        btnDel.Enabled = true;
                        break;
                    default:
                        btnDel.Enabled = false;
                        break;
                }

                cboClass.Select();

                foreach (Form frm2 in Application.OpenForms) //떠있는지 체크
                {
                    if (frm2.Name == "frmcomLibBMstSearch1")
                    {
                        frm2.Close();
                    }else if (frm2.Name == "frmPmpaViewMisuNumSearch")
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

            if(btnOK.Enabled ==true && e.Column == 0)
            {
                clsPmpaType.TMS.Del[e.Row]=ssList2.ActiveSheet.Cells[e.Row, e.Column].Text;
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
                foreach (Form frm2 in Application.OpenForms) //중복로드 방지
                {
                    if (frm2.Name == "frmcomLibBMstSearch1")
                    {

                        frm2.Visible = true;
                        frm2.Activate();
                        return;
                    }
                }

                frmcomLibBMstSearch1 frm = new frmcomLibBMstSearch1(GstrBunSu, GstrMisuGbn);
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(0, 50);
                frm.Show();
            }
            else if (sender == this.Menu1_4)
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

                frmPmpaViewMisuNumSearch frm = new frmPmpaViewMisuNumSearch(GstrBunSu, GstrMisuGbn);
                //frm.StartPosition = FormStartPosition.Manual;
                //frm.Location = new Point(0, 50);
                frm.Show();
            }
        }

        void eControl_GotFocus(object sender, EventArgs e)
        {

            if (sender == cboClass)
            {
                if (GstrBunSu.GnWRTNO != 0)
                {
                    Screen_Display(clsDB.DbCon, ssList1, ssList2);
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
            }else if(sender == CboDept)
            {
                int i = 0;

                CboDrCode.Items.Clear();
                string SQL = "";
                string SqlErr = "";
                DataTable dt = null;

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT DrCode, DrName FROM " +ComNum.DB_PMPA + "BAS_DOCTOR";
                    SQL += ComNum.VBLF + "WHERE DrDept1 = '" + VB.Left(CboDept.Text, 2) + "'";
                    SQL += ComNum.VBLF + "AND Tour   != 'Y' ";                

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        CboDrCode.Items.Add(dt.Rows[i]["DrCode"].ToString().Trim() + "." + dt.Rows[i]["DrName"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;

                }
                catch (Exception ex)
                {
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(ex.Message);
                }
            }
            else if (sender == TxtMisuID)
            {
                if(TxtMisuID.Text == "")
                {
                    return;
                }

                TxtMisuID.Text = VB.Val(TxtMisuID.Text).ToString("00000000");
                lblSname.Text = cPM.READ_BAS_PATIENT(TxtMisuID.Text.Trim());
                if(lblSname.Text == "")
                {
                    ComFunc.MsgBox("해당번호가 등록되지 않았습니다.", "확인");
                    TxtMisuID.Text = "";
                    TxtMisuID.Focus();
                    return;
                }

                return;
            }
            else if (sender == TxtGelCode)
            {
                if (TxtGelCode.Text.Trim() == "")
                {
                    return;
                }

                TxtGelCode.Text = TxtGelCode.Text.ToUpper();
                lblMiaName.Text = cPM.READ_BAS_MIA(TxtGelCode.Text.Trim());
                if (lblMiaName.Text.Trim() == "")
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

                argRow = o.ActiveSheet.ActiveRowIndex;
                argCol = o.ActiveSheet.ActiveColumnIndex;
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
                if (sender == TxtMisuID)
                {
                    TxtGelCode.Focus();
                }
                else if (sender == TxtGelCode)
                {
                    dtpBDate.Focus();
                }
                else if (sender == dtpBDate)
                {
                    cboBun.Focus();
                }
                else if (sender == cboBun)
                {
                    cboIO.Focus();
                }
                else if (sender == cboIO)
                {
                    cboTongGbn.Focus();
                }
                else if (sender == cboTongGbn)
                {
                    cboMgrRank.Focus();
                }
                else if (sender == cboMgrRank)
                {
                    TxtMirYYMM.Focus();
                }
                else if (sender == TxtMirYYMM)
                {
                    DtpSagoDate.Focus();
                }
                else if (sender == DtpSagoDate)
                {
                    DtpGasiDate.Focus();
                }
                else if (sender == DtpGasiDate)
                {
                    TxtCarNo.Focus();
                }
                else if (sender == TxtCarNo)
                {
                    TxtGaheja.Focus();
                }
                else if (sender == TxtGaheja)
                {
                    DtpFDate.Focus();
                }
                else if (sender == DtpFDate)
                {
                    DtpTDate.Focus();
                }
                else if (sender == DtpTDate)
                {
                    CboDept.Focus();
                }
                else if (sender == CboDept)
                {
                    CboDrCode.Focus();
                }
                else if (sender == CboDrCode)
                {
                    ssList2.Focus();
                }
                else if (sender == cboClass)
                {
                    TxtMisuID.Focus();
                }
            }
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
          
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
                dtpBDate.Text = cpublic.strSysDate;
                cboIO.SelectedIndex = -1;
                cboBun.SelectedIndex = -1;
                lblWRTNO.Text = "";
                TxtMirYYMM.Text = "";

                cPM.TMM_Clear_Rtn();

                ssList2.ActiveSheet.RowCount = 0;
                ssList2.ActiveSheet.RowCount = 20;

                CS.Spread_Clear_Range(ssList1, 0, 0, ssList1.ActiveSheet.RowCount, ssList1.ActiveSheet.ColumnCount);

                //자보,산재 내역 Clear
                DtpSagoDate.Text = cpublic.strSysDate;
                DtpGasiDate.Text = cpublic.strSysDate;
                TxtCarNo.Text = "";  TxtGaheja.Text = "";
                DtpFDate.Text = cpublic.strSysDate;
                DtpTDate.Text = cpublic.strSysDate;
                CboDept.SelectedIndex = -1;
                CboDrCode.SelectedIndex = -1;
                cboMgrRank.SelectedIndex = -1;
                cboTongGbn.SelectedIndex = -1;
                
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

        void Screen_Display(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd1, FarPoint.Win.Spread.FpSpread Spd2)
        {
            int i = 0;
            //int j = 0;
            //double nJanAmt = 0;
            //string strData = "";
            string strMDATE = "", strCDATE = "";
            int nREAD = 0;
            DataTable dt = null;

            cPM.READ_MISU_IDMST(GstrBunSu.GnWRTNO);

            if(clsPmpaType.TMM.ROWID == "")
            {
                ComFunc.MsgBox("미수내역이 없습니다", "확인");
            }

            strMDATE = cPM.READ_MISU_MAGAM_DATE();

            TxtMisuID.Text = clsPmpaType.TMM.MisuID;
            lblWRTNO.Text =  clsPmpaType.TMM.WRTNO.ToString();
            TxtGelCode.Text = clsPmpaType.TMM.GelCode;
            lblMiaName.Text = cPM.READ_BAS_MIA(clsPmpaType.TMM.GelCode.Trim());
            dtpBDate.Text = clsPmpaType.TMM.BDate;

            if(string.Compare(clsPmpaType.TMM.TongGbn, "1") < 0)
            {
                cboTongGbn.SelectedIndex = -1;
            }else
            {
                cboTongGbn.SelectedIndex = Convert.ToInt32(VB.Val(clsPmpaType.TMM.TongGbn)) - 1;
            }

            switch (clsPmpaType.TMM.MgrRank)
            {
                case "1": cboMgrRank.Text = "1.완불가능";  break;
                case "2": cboMgrRank.Text = "2.독려시입금가능"; break;
                case "3": cboMgrRank.Text = "3.대손처리예상"; break;
                case "4": cboMgrRank.Text = "4.재판중"; break;
                case "5": cboMgrRank.Text = "5.재산압류"; break;
                case "6": cboMgrRank.Text = "6.분쟁심의회상정"; break;
                case "7": cboMgrRank.Text = "7.일부입금"; break;
                case "8": cboMgrRank.Text = "8.문제건"; break;
                case "9": cboMgrRank.Text = "9.기타"; break;
                case "A": cboMgrRank.Text = "A.분쟁심의회상정(중)"; break;
                case "B": cboMgrRank.Text = "B.환자수납예정"; break;
                case "C": cboMgrRank.Text = "C.자문보류중"; break;
                case "D": cboMgrRank.Text = "D.소송중"; break;
                case "E": cboMgrRank.Text = "E.이의제기중"; break;
                default: cboMgrRank.SelectedIndex = -1; break;
            }

            TxtMirYYMM.Text = clsPmpaType.TMM.MirYYMM;
            cboIO.SelectedIndex = cPM.ListIndex_MisuIO(clsPmpaType.TMM.IpdOpd.Trim());
            cboBun.SelectedIndex = cPM.ListIndex_MisuBun(clsPmpaType.TMM.Bun.Trim(),"TA");
            

            if(GstrBunSu.GstrClass == "05" || GstrBunSu.GstrClass == "07")
            {
                lblSname.Text = cPM.READ_BAS_PATIENT(clsPmpaType.TMM.MisuID.Trim());
                DtpSagoDate.Text = clsPmpaType.TMM.TDATE;
                DtpGasiDate.Text = clsPmpaType.TMM.JDATE;
                TxtCarNo.Text = clsPmpaType.TMM.CARNO;
                TxtGaheja.Text = clsPmpaType.TMM.DRIVER;
            }

            DtpFDate.Text = clsPmpaType.TMM.FromDate;
            DtpTDate.Text = clsPmpaType.TMM.ToDate;
            CboDept.SelectedIndex = cPM.ListIndex_MisuDept(clsPmpaType.TMM.DeptCode.Trim());
            
            CboDrCode.Items.Clear();

            dt = cSQL.Sel_MisuMast2_DISPLAY_SELECT1(pDbCon);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                CboDrCode.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;
            

            dt = cSQL.Sel_MisuMast2_DISPLAY_SELECT2(pDbCon);

            if(dt.Rows.Count > 0)
            {
                CboDrCode.Text = clsPmpaType.TMM.DrCode.Trim() + "." + dt.Rows[0]["DRNAME"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            

            dt = cSQL.Sel_MisuMast2_DISPLAY_SELECT3(pDbCon, GstrBunSu.GnWRTNO);

            nREAD = dt.Rows.Count;
            Spd2.ActiveSheet.RowCount = dt.Rows.Count + 20;
            
            for (i = 0; i < nREAD; i++)
            {
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.A].Text = "";
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.Bdate].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                strCDATE = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.Bdate].Text;
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.Gubun].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.GubunName].Text = cPM.READ_MisuGye_MISU(dt.Rows[i]["Gubun"].ToString().Trim(),"TA");
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.Qty].Text = (Convert.ToDouble(VB.Val(dt.Rows[i]["Qty"].ToString().Trim()))).ToString();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.Amt].Text = dt.Rows[i]["Amt"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.Remark].Text = dt.Rows[i]["Remark"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.OldBdate].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.OldGubun].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.OldQty].Text = dt.Rows[i]["Qty"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.OldAmt].Text = dt.Rows[i]["Amt"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.OldRemark].Text = VB.Left(dt.Rows[i]["Remark"].ToString().Trim(),30);
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.EntDate].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.EntPart].Text = dt.Rows[i]["EntPart"].ToString().Trim();
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

        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd1, FarPoint.Win.Spread.FpSpread Spd2)
        {
            int i = 0;
            //int j = 0, k = 0;
            string strRemark = "", strROWID = "", strIdChange = "", strSlipChange = "";

            string strDel = "", strBdate1 = "", strGubun1 = "";
            double nQty1 = 0, nTAmt1 = 0, nAmt1 = 0;
            string strRemark1 = "";

            string strBdate2 = "", strGubun2 = "";
            double nQty2 = 0, nTAmt2 = 0, nAmt2 = 0;
            string strRemark2 = "", strEntDate2 = "";
            long nEntPart2 = 0;

            string strFromDate = "", strToDate = "";
            string strMDATE = ""; //미수마감일 

            //string Time = "";

            string SqlErr = ""; //에러문 받는 변수
            string SQL = "";
            int intRowAffected = 0;
            DataTable dt = null;

            strMDATE = cPM.READ_MISU_MAGAM_DATE();

            Invalid_Data_Check();

            clsDB.setBeginTran(pDbCon);

            #region CmdOK_IDMST_RTN
            clsPmpaType.TMN.WRTNO = clsPmpaType.TMM.WRTNO;
            clsPmpaType.TMN.MisuID = TxtMisuID.Text.Trim();
            clsPmpaType.TMN.BDate = dtpBDate.Text.Trim();
            clsPmpaType.TMN.Class = VB.Left(cboClass.Text, 2);
            clsPmpaType.TMN.IpdOpd = VB.Left(cboIO.Text, 1);
            clsPmpaType.TMN.GelCode = TxtGelCode.Text.Trim();
            clsPmpaType.TMN.Bun = VB.Left(cboBun.Text, 2);
            clsPmpaType.TMN.DeptCode = "";
            clsPmpaType.TMN.MgrRank = VB.Left(cboMgrRank.Text,1);
            clsPmpaType.TMN.GbEnd = "1";
            
            if(clsPmpaType.TMN.JAmt < 1)
            {
                clsPmpaType.TMN.GbEnd = "0";
            }

            if(VB.IsDate(DtpFDate.Text) == false)
            {
                clsPmpaType.TMN.FromDate = "";
            }else
            {
                clsPmpaType.TMN.FromDate = DtpFDate.Text;
            }

            if(VB.IsDate(DtpTDate.Text) == false)
            {
                clsPmpaType.TMN.ToDate = "";
            }
            else
            {
                clsPmpaType.TMN.ToDate = DtpTDate.Text;
            }

            if(clsPmpaType.TMN.FromDate.Trim() != "" && clsPmpaType.TMN.ToDate.Trim() != "")
            {
                clsPmpaType.TMN.Ilsu = CF.DATE_ILSU(clsDB.DbCon, clsPmpaType.TMN.ToDate, clsPmpaType.TMN.FromDate) + 1;
            }else
            {
                clsPmpaType.TMN.Ilsu = 0;
            }

            clsPmpaType.TMN.DeptCode = VB.Left(CboDept.Text, 2);
            clsPmpaType.TMN.DrCode = VB.Left(CboDrCode.Text.Trim(), 4);
            clsPmpaType.TMN.Qty[1 - 1] = 1;
            clsPmpaType.TMN.MirYYMM = TxtMirYYMM.Text;

            clsPmpaType.TMN.TDATE = DtpSagoDate.Text;
            clsPmpaType.TMN.JDATE = DtpGasiDate.Text;
            clsPmpaType.TMN.CARNO = TxtCarNo.Text;
            clsPmpaType.TMN.DRIVER = TxtGaheja.Text;

            //산재, 자보 Remark Setting

            strRemark = "";

            if(GstrBunSu.GstrClass == "05")
            {
                strRemark = strRemark + VB.Left(TxtCarNo.Text + VB.Space(28), 28);
                strRemark = strRemark + VB.Left(VB.Replace(DtpSagoDate.Text, "-", "") + VB.Space(6), 6);
                strRemark = strRemark + VB.Left(VB.Replace(DtpGasiDate.Text, "-", "") + VB.Space(6), 6);
                clsPmpaType.TMN.Remark = strRemark;
            }else if(GstrBunSu.GstrClass == "07")
            {
                strRemark = VB.Left(VB.Mid(DtpSagoDate.Text, 3, 10) + VB.Space(8), 8);
                strRemark = strRemark + VB.Left(VB.Mid(DtpGasiDate.Text, 3, 10) + VB.Space(8), 8);
                strRemark = strRemark + VB.Left(TxtCarNo.Text + VB.Space(14), 14);
                strRemark = strRemark + VB.Left(TxtGaheja.Text + VB.Space(20), 20);
                clsPmpaType.TMN.Remark = strRemark.Trim();
            }

            //환자 종류 SET
            switch (VB.Left(cboClass.Text, 2))
            {
                case "05":
                    clsPmpaType.TMN.Bi = "31";
                    break;
                case "07":
                    clsPmpaType.TMN.Bi = "52";
                    break;
                default:
                    clsPmpaType.TMN.Bi = "";
                    break;
            }

            if(clsPmpaType.TMN.FromDate.Trim() == "")
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
            }else
            {
                strToDate = clsPmpaType.TMN.ToDate;
            }

            if(clsPmpaType.TMN.WRTNO == 0)
            {
                #region BtnOk_IDMST_INSERT 신규등록
                strIdChange = "NO";

                dt = cSQL.Sel_MisuMast2_BtnOk_IDMST_INSERT(pDbCon);

                if(dt.Rows.Count == 0)
                {
                    clsPmpaType.TMN.WRTNO = 1;
                }else
                {
                    clsPmpaType.TMN.WRTNO = Convert.ToInt32(VB.Val(dt.Rows[0]["mWRTNO"].ToString().Trim())) + 1;
                }

                dt.Dispose();
                dt = null;

                try
                {
                    SqlErr = cSQL.Int_MisuMast2_BtnOk_IDMST_INSERT(pDbCon,strFromDate,strToDate,cpublic.strSysDate,cpublic.strSysTime,VB.Left(cboTongGbn.Text,1), ref intRowAffected);

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
                #region BtnOk_IDMST_UPDATE 변경등록
                strIdChange = "NO";

                if(clsPmpaType.TMM.MisuID != clsPmpaType.TMN.MisuID){ strIdChange = "OK"; }
                if(clsPmpaType.TMM.GelCode != clsPmpaType.TMN.GelCode) { strIdChange = "OK"; }
                if(clsPmpaType.TMM.BDate != clsPmpaType.TMN.BDate) { strIdChange = "OK"; }
                if(clsPmpaType.TMM.IpdOpd != clsPmpaType.TMN.IpdOpd) { strIdChange = "OK"; }
                if(clsPmpaType.TMM.MirYYMM != clsPmpaType.TMN.MirYYMM) { strIdChange = "OK"; }
                if(clsPmpaType.TMM.MgrRank != clsPmpaType.TMN.MgrRank) { strIdChange = "OK"; }

                try
                {
                    if (strIdChange == "OK")
                    {
                        SqlErr = cSQL.Upt_MisuMast2_BtnOk_IDMST_UPDATE(pDbCon,strFromDate,strToDate,cpublic.strSysDate,cpublic.strSysTime,VB.Left(cboTongGbn.Text,1), ref intRowAffected);
                    }
                    else
                    {
                        SqlErr = cSQL.Upt_MisuMast2_BtnOk_IDMST_UPDATE2(pDbCon,strFromDate,strToDate,VB.Left(cboTongGbn.Text,1), ref intRowAffected);
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

                    if(clsPmpaType.TMM.MisuID != clsPmpaType.TMN.MisuID) { strIdChange = "OK"; }
                    if(clsPmpaType.TMM.GelCode != clsPmpaType.TMN.GelCode) { strIdChange = "OK"; }
                    if(clsPmpaType.TMM.IpdOpd != clsPmpaType.TMN.IpdOpd) { strIdChange = "OK"; }
                    if(clsPmpaType.TMM.MirYYMM != clsPmpaType.TMN.MirYYMM) { strIdChange = "OK"; }

                    if(strIdChange == "OK")
                    {
                        SqlErr = cSQL.Upt_MisuMast2_BtnOk_SLIP_UPDATE(pDbCon, ref intRowAffected);

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
                #endregion

                #region BtnOk_IDMST_History
                if (strIdChange == "NO") { }
                else
                {
                    try
                    {
                         SqlErr = cSQL.Upt_MisuMast2_BtnOk_HISTORY_UPDATE(pDbCon, cpublic.strSysDate, cpublic.strSysTime, ref intRowAffected);
                        
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

            #endregion END_CmdOK_IDMST_RTN 

            #region CmdOK_Slip_RTN Slip 변경분 Update
            //수정, 변경분 Insert
            for (i = 0; i < Spd2.ActiveSheet.RowCount; i++)
            {
                strDel = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.A].Text;
                strBdate1 = Spd2.ActiveSheet.Cells[i,(int)clsComPmpaSpd.enmPmpaMisuMast2.Bdate].Text;
                strGubun1 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.Gubun].Text;
                nQty1 = Convert.ToInt32(VB.Val(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.Qty].Text));
                nAmt1 = Convert.ToInt32(VB.Val(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.Amt].Text));
                strRemark1 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.Remark].Text;

                strROWID = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.ROWID].Text;
                strBdate2 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.OldBdate].Text;
                strGubun2 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.OldGubun].Text;
                nQty2 = Convert.ToInt32(VB.Val(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.OldQty].Text));
                nAmt2 = Convert.ToInt32(VB.Val(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.OldAmt].Text));
                strRemark2 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.OldRemark].Text;
                strEntDate2 = Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.EntDate].Text;
                nEntPart2 = Convert.ToInt32(VB.Val(Spd2.ActiveSheet.Cells[i, (int)clsComPmpaSpd.enmPmpaMisuMast2.EntPart].Text));

                nTAmt1 = nAmt1;
                nTAmt2 = nAmt2;

                if(strDel =="True")
                {
                    if(strROWID != "")
                    {
                        //2018-07-05
                        if (string.Compare(strBdate1, strMDATE) <= 0 && strBdate1 != "")
                        {
                            cPM.Message_MISU_Magam(strMDATE);
                        }else
                        {
                            #region CmdOK_MisuSlip_Delete 삭제
                            try
                            {
                                SqlErr = cSQL.Del_MisuMast2_BtnOK_Slip(pDbCon, strROWID, ref intRowAffected);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    ComFunc.MsgBox("MISU_SLIP OLD자료 Delete ERROR", "확인");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                    return;
                                }

                                SqlErr = cSQL.Int_MisuMast2_BtnOK_HISTORY_INSERT(pDbCon,cpublic.strSysDate,cpublic.strSysTime,strBdate2,strGubun2,nQty2,nTAmt2,nAmt2,strRemark2,strEntDate2,nEntPart2, ref intRowAffected);

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
                }else if (strDel != "1")
                {
                    if(strROWID == "")
                    {
                        //2018-07-05
                        //if(VB.Val(strBdate1) <= VB.Val(strMDATE) && strBdate1 != "")
                        if(string.Compare(strBdate1,strMDATE)<=0 && strBdate1 != "") 
                        {
                            cPM.Message_MISU_Magam(strMDATE);
                        }else
                        {
                            if (strBdate1 == "" && strGubun1 == "")
                            {

                            }else
                            {
                                #region CmdOK_MisuSlip_Insert
                                try
                                {
                                    
                                    SqlErr = cSQL.Int_MisuMast2_BtnOK_MisuSlip_INSERT(pDbCon,strGubun1,nQty1,nTAmt1,nAmt1,strBdate1,strRemark1, ref intRowAffected);

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
                    }else
                    {
                        strSlipChange = "NO";
                        if (strGubun1 != strGubun2) { strSlipChange = "OK"; }
                        if (strBdate1 != strBdate2) { strSlipChange = "OK"; }
                        if(nQty1 != nQty2) { strSlipChange = "OK"; }
                        if(nTAmt1 != nTAmt2) { strSlipChange = "OK"; }
                        if(nAmt1 != nAmt2) { strSlipChange = "OK"; }
                        if(strRemark1 != strRemark2) { strSlipChange = "OK"; }

                        //2018-07-05
                        if(strSlipChange == "OK")
                        {
                            if(VB.Val(strBdate1) <= VB.Val(strBdate2) && strBdate1 != "")
                            {
                                cPM.Message_MISU_Magam(strMDATE);
                            }else
                            {
                                #region CmdOK_MisuSlip_Update
                                try
                                {

                                    SqlErr = cSQL.Upt_MisuMast2_BtnOk_MisuSlip_UPDATE(pDbCon,strBdate1,strGubun1,nQty1,nTAmt1,nAmt1,strRemark1,strROWID, ref intRowAffected);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(pDbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        ComFunc.MsgBox("MISU_SLIP UPDATE ERROR", "확인");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                        return;
                                    }

                                    //Slip history에 Insert
                                    SqlErr = cSQL.Int_MisuMast2_BtnOK_MisuHISTOTY_INSERT(pDbCon,cpublic.strSysDate, VB.Left(cpublic.strSysTime, 5), strBdate2,strGubun2,nQty2,nTAmt2,nAmt2,strRemark2,strEntDate2,nEntPart2, ref intRowAffected);

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

            //화면 Clear & 각종 Enble Set
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

            strMsg = "미수내역을 삭제 하시겠습니까?";

            if (ComFunc.MsgBoxQ(strMsg, "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            clsDB.setBeginTran(pDbCon);

            try
            {
                //IDMST를 삭제함 
                SqlErr = cSQL.Del_MisuMast2_BtnDel_IDMST(pDbCon, clsPmpaType.TMM.WRTNO , ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("MISU_IDMST DELETE ERROR", "확인");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }

                //HISTORY Insert
                Time = VB.Left(cpublic.strSysTime, 5);

                SqlErr = cSQL.Int_MisuMast2_BtnDel_History(pDbCon, cpublic.strSysDate, Time, ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("MISU_HISTORY INSERT 오류가 발생함", "확인");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }

                //삭제할 SLIP을 History에 Insert
                SqlErr = cSQL.Int_MisuMast_BtnDel_SlipHistory(pDbCon, cpublic.strSysDate, Time, ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("MISU_HISTORY INSERT 오류가 발생함", "확인");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }

                //SLip을 삭제함
                SqlErr = cSQL.Del_MisuMast2_BtnDel_Slip(pDbCon, ref intRowAffected);

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

        void eSave(PsmhDb pDbCon, FpSpread spd2, int argRow, int argCol)
        {
            string strData = "";
            string strMDATE = "";

            strMDATE = cPM.READ_MISU_MAGAM_DATE();

            if (argCol == 1)
            {
                strData = ssList2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast2.Bdate].Text.Trim();

                if (strData != "")
                {
                    if (string.Compare(strData, dtpBDate.Text) < 0)
                    {
                        ComFunc.MsgBox("입금일자는 청구일자보다 작을 수 없습니다.");
                        ssList2.ActiveSheet.SetActiveCell(argRow, argCol);
                        return;
                    }
                    ssList2.ActiveSheet.SetActiveCell(argRow, argCol + 1);
                }
            }else if (argCol == 2)
            {
                strData = spd2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast2.Gubun].Text;
                if(strData != "")
                {
                    if(strData == "99")
                    {
                        #region SAKGAM_CALC_Rtn
                        int i = 0;
                        double nWorkSak = 0;

                        for (i = 0; i < 200; i++)
                        {
                            if(clsPmpaType.TMS.Del[i] != "True" && VB.Val(clsPmpaType.TMS.Gubun[i]) > 10)
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
                                        nWorkSak = nWorkSak - clsPmpaType.TMS.Amt[i];
                                        break;
                                }
                            }
                        }

                        spd2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast2.Gubun].Text = "31";
                        spd2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast2.Amt].Text = nWorkSak.ToString("###########0");
                        clsPmpaType.TMS.Gubun[argRow] = "31";
                        strData = "31";
                        clsPmpaType.TMS.Amt[argRow] = nWorkSak;
                        #endregion End SAKGAM_CALC_Rtn
                    }

                    strData = VB.Val(spd2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast2.Gubun].Text).ToString("00");

                    //자보환자미수관리일때
                    if(GstrBunSu.GstrClass == "TA")
                    {
                        strData = strData + "TA";
                    }

                    spd2.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmPmpaMisuMast2.GubunName].Text = cPM.READ_MisuGye_MISU(strData, "TA");
                    if(cPM.READ_MisuGye_MISU(strData, "TA") == "")
                    {
                        ComFunc.MsgBox("구분이 오류 입니다.", "확인");
                        return;
                    }
                    else
                    {

                    }
                }
            }
            
            if(argCol == 2 || argCol == 4 || argCol == 5)
            {
                switch (argCol)
                {
                    case 2:
                        clsPmpaType.TMS.Gubun[argRow] = spd2.ActiveSheet.Cells[argRow, argCol].Text;
                        ssList2.ActiveSheet.SetActiveCell(argRow,(int)clsComPmpaSpd.enmPmpaMisuMast2.Qty);
                        break;
                    case 4:
                        clsPmpaType.TMS.Qty[argRow] = Convert.ToInt32(VB.Val(spd2.ActiveSheet.Cells[argRow, argCol].Text));
                        ssList2.ActiveSheet.SetActiveCell(argRow, argCol + 1);
                        break;
                    case 5:
                        clsPmpaType.TMS.Amt[argRow] = Convert.ToInt32(VB.Val(spd2.ActiveSheet.Cells[argRow, argCol].Text));
                        ssList2.ActiveSheet.SetActiveCell(argRow, argCol + 1);
                        break;
                }

                Amt_Account();
            }

            if(argCol == 6)
            {
                int bytecount = System.Text.Encoding.Default.GetByteCount(ssList2.ActiveSheet.Cells[argRow,(int)clsComPmpaSpd.enmPmpaMisuMast2.Remark].Text);
                if(bytecount >= 60)
                {
                    ComFunc.MsgBox("REMARK 적요 길이가 너무 큽니다.", "확인"); 
                    ssList2.ActiveSheet.SetActiveCell(argRow, argCol);
                    ssList2.ActiveSheet.Cells[argRow, argCol].Text = strRemark;
                }
            }
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
                            //clsPmpaType.TMN.Qty[3] = clsPmpaType.TMN.Qty[3] + clsPmpaType.TMS.Qty[i];
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
            ssList1.ActiveSheet.Cells[0, 4].Text = VB.Format(clsPmpaType.TMN.JAmt,   "###,###,###,##0"); 

        }

        void Invalid_Data_Check() //오류 Data Check
        {
            if (TxtMisuID.Text.Trim() == "")
            {
                ComFunc.MsgBox("미수번호가 공란입니다", "확인");
                return;
            }
            if(cboClass.Text.Trim() == "")
            {
                ComFunc.MsgBox("미수종류가 공란입니다", "확인");
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

            if(TxtMirYYMM.Text.Trim() == "")
            {
                ComFunc.MsgBox("청구년월 공란입니다,", "확인");
            }

            if (cboTongGbn.Text.Trim() == "")
            {
                ComFunc.MsgBox("통계구분이 공란입니다.", "확인");
                return;
            }

            TxtMirYYMM.Text = TxtMirYYMM.Text.Trim();

            if (VB.Len(TxtMirYYMM.Text) != 6)
            {
                ComFunc.MsgBox("통계월을 YYYYMM형식으로 입력하세요.", "확인");
                return;
            }

            if(CboDept.Text == "")
            {
                ComFunc.MsgBox("진료과가 공란입니다", "확인");
            }

            if(CboDrCode.Text.Trim() == "")
            {
                ComFunc.MsgBox("진료의사가 공란 입니다.", "확인");
            }

            if (Convert.ToInt32(VB.Right(TxtMirYYMM.Text, 2)) >= 1 && Convert.ToInt32(VB.Right(TxtMirYYMM.Text, 2)) <= 12)
            {}
            else
            {
                ComFunc.MsgBox("통계월의 월이 오류 입니다.(1-12월)", "확인");
                return;
            }

            if (Convert.ToInt32(VB.Left(TxtMirYYMM.Text, 4)) >= 1900 && Convert.ToInt32(VB.Left(TxtMirYYMM.Text, 4)) <= 2999)
            {}
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
            cboMgrRank.Enabled = argGUBUN;
            cboBun.Enabled = argGUBUN;
            cboTongGbn.Enabled = argGUBUN;
            TxtMirYYMM.Enabled = argGUBUN;
            btnHelp.Enabled = argGUBUN;
            DtpFDate.Enabled = argGUBUN;
            DtpTDate.Enabled = argGUBUN;
            TxtCarNo.Enabled = argGUBUN;
            TxtGaheja.Enabled = argGUBUN;
            DtpSagoDate.Enabled = argGUBUN;
            DtpGasiDate.Enabled = argGUBUN;
            CboDept.Enabled = argGUBUN;
            CboDrCode.Enabled = argGUBUN;
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
    }
}