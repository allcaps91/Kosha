using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ComBase.Controls;
using ComLibB;
using ComPmpaLibB;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcAmReserve.cs
/// Description     : 암검진 예약 등록 화면
/// Author          : 이상훈
/// Create Date     : 2020-07-14
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmResv_new.frm(HcAm05)" />

namespace ComHpcLibB
{
    public partial class frmHcAmReserve : Form
    {
        HicCancerResv2Service hicCancerResv2Service = null;
        HicPatientService hicPatientService = null;
        BasPatientService basPatientService = null;
        HeaJepsuService heaJepsuService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicMemoService hicMemoService = null;
        ComHpcLibBService comHpcLibBService = null;
        WorkNhicService workNhicService = null;
        HicCancerResv1Service hicCancerResv1Service = null;
        BasJobService basJobService = null;

        frmHcPanPersonResult FrmHcPanPersonResult = null;
        frmHcNhicView FrmHcNhicView = null;     //자격조회 정보 
        frmHcNhicSub FrmHcNhicSub = null;
        frmHcAmRsvSelect FrmHcAmRsvSelect = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        public delegate void SetCancerGstrValue(string sPtNo, string strRemark, List<string> chk);
        public static event SetCancerGstrValue rSetCancerGstrValue;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsAlimTalk Alim = new clsAlimTalk();


        string[] FstrHolyDay = new string[31];
        string[] FstrYoil = new string[31];
        int[,] FnArray = new int[6, 31];    //설정된 예약제한값
        int[] FnJong = new int[6];
        string FstrChk;
        long[,,,] FnSet = new long[2, 31, 2, 11];
        string FstrOLD_RDate;
        long FnPano;
        string FstrRetValue = "";

        List<string> strChks = new List<string>();

        string FstrRTime;   //예약이 2건 이상일때 선택한 예약일자
        string FstrRowId;   //예약이 2건 이상일때 선택한 RowId
        string FSaveFlag;   //예약 신규/수정 여부

        string FPatSelectFlag = "";  //환자 선택 위치(예약 리스트 : SS2 , 주민번호 타이핑 : "")
        bool boolSort = false;

        public frmHcAmReserve()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetControl()
        {
            
        }

        void SetEvent()
        {
            hicCancerResv2Service = new HicCancerResv2Service();
            hicPatientService = new HicPatientService();
            basPatientService = new BasPatientService();
            heaJepsuService = new HeaJepsuService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicMemoService = new HicMemoService();
            comHpcLibBService = new ComHpcLibBService();
            workNhicService = new WorkNhicService();
            hicCancerResv1Service = new HicCancerResv1Service();
            basJobService = new BasJobService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnLtdCode2.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnOK.Click += new EventHandler(eBtnClick);
            this.btnNhic.Click += new EventHandler(eBtnClick);
            this.btnNhic2.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnPrint1.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnNewPtNo.Click += new EventHandler(eBtnClick);
            this.btnHelp.Click += new EventHandler(eBtnClick);
            this.btnHis.Click += new EventHandler(eBtnClick);
            this.btnJepsuPrt.Click += new EventHandler(eBtnClick);
            this.btnMemoSave.Click += new EventHandler(eBtnClick);
            this.btnLtdView.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS5.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS1.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS2.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS5.CellClick += new CellClickEventHandler(eSpdClick);
            this.txtRemark.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtViewLtd.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtRTime.KeyPress += new KeyPressEventHandler(eTxtKeyPress); 
            this.txtJumin1.TextChanged += new EventHandler(eTxtChange);
            this.txtJumin2.TextChanged += new EventHandler(eTxtChange);
            this.txtJumin1.KeyDown += new KeyEventHandler(eTxtKeyDown);
        }

        void fn_ComMon_Set(ComboBox argCombo, int argMonthCnt)
        {
            int ArgYY = 0;
            int ArgYY1 = 0;
            int ArgMM = 0;
            int nLocate = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            ArgYY = VB.Left(clsPublic.GstrSysDate, 4).To<int>();
            ArgYY1 = VB.Left(clsPublic.GstrSysDate, 4).To<int>() - 1;
            nLocate = VB.Mid(clsPublic.GstrSysDate, 6, 2).To<int>() - 1;
            ArgMM = 1;
            if (ArgMM == 12)
            {
                ArgMM = 1;
            }
            argCombo.Items.Clear();

            for (int i = 0; i < argMonthCnt; i++)
            {
                //argCombo.Items.Add(string.Format("{0:0000}", ArgYY) + "년" + string.Format("{0:00}", ArgMM) + "월분") ;
                argCombo.Items.Add(string.Format("{0:0000}", ArgYY) + "-" + string.Format("{0:00}", ArgMM));
                ArgMM += 1;
                if (ArgMM == 0)
                {
                    ArgMM = 12;
                    ArgYY -= 1;
                }
            }

            if (nLocate >= 8)
            {
                //argCombo.Items.Add(string.Format("{0:0000}", ArgYY + 1) + "년01월분");
                //argCombo.Items.Add(string.Format("{0:0000}", ArgYY + 1) + "년02월분");
                //argCombo.Items.Add(string.Format("{0:0000}", ArgYY + 1) + "년03월분");
                //argCombo.Items.Add(string.Format("{0:0000}", ArgYY + 1) + "년04월분");
                //argCombo.Items.Add(string.Format("{0:0000}", ArgYY + 1) + "년05월분");
                argCombo.Items.Add(string.Format("{0:0000}", ArgYY + 1) + "-01");
                argCombo.Items.Add(string.Format("{0:0000}", ArgYY + 1) + "-02");
                argCombo.Items.Add(string.Format("{0:0000}", ArgYY + 1) + "-03");
                argCombo.Items.Add(string.Format("{0:0000}", ArgYY + 1) + "-04");
                argCombo.Items.Add(string.Format("{0:0000}", ArgYY + 1) + "-05");
            }
            argCombo.SelectedIndex = nLocate;
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nREAD = 0;
            int nYY = 0;
            int nMM = 0;

            FrmHcNhicView = new frmHcNhicView();

            themTabForm(FrmHcNhicView, this.pnlNhic);

            SS1_Sheet1.Columns.Get(14).Visible = false;
            SS1_Sheet1.Columns.Get(15).Visible = false;

            tabControl.SelectedTab = tab1;
            txtViewLtd.Text = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            fn_ComMon_Set(cboYYMM, 12);

            sp.Spread_All_Clear(SS2);
            sp.Spread_All_Clear(SS5);
            fn_Screen_Clear();
            fn_Data_Display();

            cboViewYYMM.Items.Clear();
            cboViewYYMM.Items.Add("전체");
            nYY = int.Parse(VB.Left(clsPublic.GstrSysDate, 4));
            nMM = int.Parse(VB.Mid(clsPublic.GstrSysDate, 6, 2));
            for (int i = 0; i <= 12; i++)
            {
                cboViewYYMM.Items.Add(string.Format("{0:0000}", nYY) + "-" + string.Format("{0:00}", nMM));
                nMM += 1;
                if (nMM == 13)
                {
                    nMM = 1;
                    nYY += 1;
                }
            }
            cboViewYYMM.SelectedIndex = 0;
            if (SS1.ActiveSheet.RowCount > 0)
            {
                //eSpdDClick(SS1, new CellClickEventArgs(new SpreadView(), 0, 0, 0, 0, new MouseButtons(), false, false));
            }
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == txtJumin1 || sender == txtJumin2)
            {
                FPatSelectFlag = "";
            }
        }

        void eTxtChange(object sender, EventArgs e)
        {
            if (FPatSelectFlag.IsNullOrEmpty())
            {
                if (sender == txtJumin1)
                {
                    if (txtJumin1.Text.Length == 6)
                    {
                        txtJumin2.Focus();
                    }
                }
                else if (sender == txtJumin2)
                {
                    if (txtJumin2.Text.Length == 7)
                    {
                        btnHelp.Focus();
                        eBtnClick(btnHelp, new EventArgs());
                    }
                }
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {    
            if (sender == txtRemark || sender == txtRTime || sender == txtTel || sender == txtHopeDr || sender == txtHPhone)
            {
                if (e.KeyChar == 13)
                {
                    if (sender == txtRTime)
                    {
                        if (txtRTime.Text.Trim().Length < 5)
                        {
                            txtRTime.Text = string.Format("{0:0#:0#}", txtRTime.Text.To<int>());
                        }
                    }
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtSName)
            {
                if (!txtSName.Text.IsNullOrEmpty() && !txtJumin1.Text.IsNullOrEmpty() && !txtJumin2.Text.IsNullOrEmpty())
                {
                    btnNhic.Enabled = true;
                    btnNhic2.Enabled = false;                    
                }
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode2, new EventArgs());
                    }
                }
            }
            else if (sender == txtViewLtd)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtViewLtd.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                }
            }
        }

        public void themTabForm(Form frm, Control pControl)
        {
            frm.Owner = this;
            frm.TopLevel = false;
            this.Controls.Add(frm);
            frm.Parent = pControl;
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.WindowState = FormWindowState.Normal;
            frm.Height = pControl.Height;
            frm.Width = pControl.Width;
            frm.Dock = DockStyle.Fill;
            frm.Show();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnLtdCode)
            {
                string strLtdCode = "";

                if (txtViewLtd.Text.IndexOf(".") > 0)
                {
                    strLtdCode = VB.Pstr(txtViewLtd.Text, ".", 2);
                }
                else
                {
                    strLtdCode = txtViewLtd.Text;
                }

                FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
                FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                FrmHcLtdHelp.ShowDialog();
                FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtViewLtd.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtViewLtd.Text = "";
                }
            }
            else if (sender == btnLtdCode2)
            {
                string strLtdCode = "";

                if (txtLtdCode.Text.IndexOf(".") > 0)
                {
                    strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 2);
                }
                else
                {
                    strLtdCode = txtLtdCode.Text;
                }

                FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
                FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                FrmHcLtdHelp.ShowDialog();
                FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnSearch)
            {
                //sp.Spread_All_Clear(SS2);
                //sp.Spread_All_Clear(SS5);
                //fn_Screen_Clear();
                fn_Data_Display();
            }
            else if (sender == btnOK)
            {
                int nREAD = 0;
                long nTime = 0;
                string strTel = "";
                string strHPhone = "";
                string[] strAm = new string[4];
                string[] strJong = new string[11];
                int[] nAm = new int[4];
                int[] nJong = new int[11];
                int nDay = 0;
                string strOK = "";
                string strDate = "";
                string strPANO = "";
                long nRtime = 0;
                string strSex = "";
                string strSms = "";
                string strJumin = "";
                string strMsg = "";
                string strTempCD = "";
                string sMsg = "";
                string strFDate = "";
                string strTDate = "";

                long j = 0;
                int result = 0;

                ComFunc.ReadSysDate(clsDB.DbCon);

                if (txtPaNo.Text.IsNullOrEmpty())
                {
                    eBtnClick(btnNewPtNo, new EventArgs());
                }

                DateTime date1 = new DateTime();
                DateTime date2 = new DateTime();
                date1 = Convert.ToDateTime(dtpRDate.Text);
                date2 = Convert.ToDateTime(clsPublic.GstrSysDate);
                if (date1 < date2)
                {
                    MessageBox.Show("예약일이 오늘보다 적음", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dtpRDate.Focus();
                    return;
                }

                if (txtRTime.Text.IsNullOrEmpty() || txtRTime.Text == "00:00")
                {
                    MessageBox.Show("예약시간을 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtRTime.Focus();
                    return;
                }

                if (txtRTime.Text.Length != 5)
                {
                    MessageBox.Show("정확한 예약시간을 입력하세요. (예시 : 09:30)", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtRTime.Focus();
                    return;
                }

                if (txtHopeDr.Text.Length > 10)
                {
                    MessageBox.Show("희망의사가 10자를 초과함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtHopeDr.Focus();
                    return;
                }

                if (chkJong1.Checked == true && chkJong2.Checked == true)
                {
                    MessageBox.Show("위내시경 본관과 종검 동시에 선택됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //GFS(종검) 점검
                if (chkJong2.Checked == true)
                {
                    if (string.Compare(VB.Right(dtpRDate.Text, 5), "03-01") >= 0 && string.Compare(VB.Right(txtRTime.Text, 5), "12:00") <= 0)
                    {
                        if (MessageBox.Show("종검에서 위내시경만 하십니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            return;
                        }
                    }
                }

                //오후에 외래내시경인 경우 확인 메세지 표시
                if (chkJong1.Checked == true)
                {
                    if (string.Compare(txtRTime.Text, "13:00") >= 0)
                    {
                        if (MessageBox.Show("오후 본관에서 위+대장내시경 동시 검사를 하십니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            return;
                        }
                    }
                }


              


                //정원 체크를 위해 반드시 해당월을 조회 해야됨
                //strDate = VB.Left(cboYYMM.Text, 4) + "-" + string.Format("{0:00}", int.Parse(VB.Mid(cboYYMM.Text, 6, 2)));
                strDate = cboYYMM.Text;
                if (VB.Left(dtpRDate.Text, 7) != strDate)
                {
                    MessageBox.Show("왼쪽 상단의 년월을 예약월로 변경 및 조회를 한 후 저장하세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboYYMM.Focus();
                    return;
                }

                fn_Data_Display();

                long nPoscoAmCount1 = 0;
                long nPoscoPmCount1 = 0;
                long nPoscoAmCount2 = 0;
                long nPoscoPmCount2 = 0;
                long nPoscoAmCount3 = 0;
                long nPoscoPmCount3 = 0;

                //포스코위탁 인원체크
                List<COMHPC> list2 = comHpcLibBService.GetPoscoCount(dtpRDate.Text);
                if (list2.Count > 0)
                {
                    for (int i = 0; i < list2.Count; i++)
                    {
                        if (!list2[i].EXAMRES1.IsNullOrEmpty())
                        {
                            if (VB.Pstr(list2[i].HIC, "^^", 1) =="1")
                            {
                                if (list2[i].EXAMRES1.Value.Hour < 12)
                                {
                                    nPoscoAmCount1 = nPoscoAmCount1 + 1;
                                }
                                else
                                {
                                    nPoscoPmCount1 = nPoscoPmCount1 + 1;
                                }
                            }
                        }

                        if (!list2[i].EXAMRES2.IsNullOrEmpty())
                        {
                            if (VB.Pstr(list2[i].HIC, "^^", 3) == "1")
                            {
                                if (list2[i].EXAMRES2.Value.Hour < 12)
                                {
                                    nPoscoAmCount2 = nPoscoAmCount2 + 1;
                                }
                                else
                                {
                                    nPoscoPmCount2 = nPoscoPmCount2 + 1;
                                }
                            }
                        }

                        if (!list2[i].EXAMRES3.IsNullOrEmpty())
                        {
                            if (VB.Pstr(list2[i].HIC, "^^", 4) == "1")
                            {
                                if (list2[i].EXAMRES3.Value.Hour < 12)
                                {
                                    nPoscoAmCount2 = nPoscoAmCount2 + 1;
                                }
                                else
                                {
                                    nPoscoPmCount2 = nPoscoPmCount2 + 1;
                                }
                            }
                        }
                        if (!list2[i].EXAMRES7.IsNullOrEmpty())
                        {
                            if (VB.Pstr(list2[i].HIC, "^^", 6) == "1")
                            {

                                if (list2[i].EXAMRES7.Value.Hour < 12)
                                {
                                    nPoscoAmCount3 = nPoscoAmCount3 + 1;
                                }
                                else
                                {
                                    nPoscoPmCount3 = nPoscoPmCount3 + 1;
                                }
                            }
                        }
                    }
                }


                //MessageBox.Show("포스코위탁검사 포함 인원초과입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //return;



                strSms = "N";
                if (chkSMS.Checked == true)
                {
                    strSms = "Y";
                }

                //당일 예약부도자 추가 접수는 SMS 발송 안함
                if (dtpRDate.Text == clsPublic.GstrSysDate && strSms == "Y")
                {
                    strSms = "N";
                }

                strJumin = txtJumin1.Text + txtJumin2.Text;

                FstrChk = "N";
                fn_Data_Chk();
                if (FstrChk == "Y")
                {
                    return;
                }

                strTel = txtTel.Text;
                strHPhone = txtHPhone.Text;
                for (int i = 0; i <= 3; i++)
                {
                    CheckBox chkAm = (Controls.Find("chkAm" + i.ToString(), true)[0] as CheckBox);
                    if (chkAm.Checked == true)
                    {
                        strAm[i] = "Y";
                        nAm[i] = 1;
                    }
                    else
                    {
                        strAm[i] = "N";
                        nAm[i] = 0;
                    }
                }

                for (int i = 0; i < 11; i++)
                {
                    CheckBox chkJong = (Controls.Find("chkJong" + i.ToString(), true)[0] as CheckBox);
                    if (chkJong.Checked == true)
                    {
                        strJong[i] = "Y";
                        nJong[i] = 1;
                    }
                    else
                    {
                        strJong[i] = "N";
                        nJong[i] = 0;
                    }
                }

                nDay = int.Parse(VB.Right(dtpRDate.Text, 2)) - 1;
                strOK = "Y";

                //============================================================================================
                //2020.07.27 이상훈 암종류별 멀티 예약 가능 하도록 변경
                //strJumin = clsAES.AES(txtJumin1.Text + txtJumin2.Text);
                //strFDate = VB.Left(dtpRDate.Text, 4) + "-01-01";
                //strTDate = VB.Left(dtpRDate.Text, 4) + "-12-31 23:59";

                //신규,수정을 위해 ROWID를 읽음
                //txtRowId.Text = hicCancerResv2Service.GetRowIdbyJumin2RTime(strJumin, strFDate, strTDate);
                if (FSaveFlag == "NEW" || FSaveFlag.IsNullOrEmpty())
                {
                    txtRowId.Text = "";
                }
                //============================================================================================

                string strOK1 = "";
                string strGBUGI = "";
                string strGBGFS = "";
                string strGBMAMMO = "";
                string strGBRECUTM = "";
                string strGBSONO = "";
                string strGBWOMB = "";
                string strGBBOHUM = "";
                string strGBGFSH = "";
                string strGBCOLON = "";
                string strGBCT = "";
                string strGBLUNG_SANGDAM = "";

                //2021-06-01 암검진 이중등록관련 보완
                if (FSaveFlag == "NEW" || FSaveFlag.IsNullOrEmpty())
                {
                    string strSDate = dtpRDate.Text + " 00:00";
                    string strEDate = dtpRDate.Text + " 23:59";
                    string strJumin1 = clsAES.AES(txtJumin1.Text + txtJumin2.Text);
                    List<HIC_CANCER_RESV2> list1 = hicCancerResv2Service.GetRTimebyJumin2RTime(strJumin1, strSDate, strEDate);
                    if (!list1.IsNullOrEmpty())
                    {
                        for (int i = 0; i <= list1.Count - 1; i++)
                        {
                            strGBUGI = list1[i].GBUGI;                              //위장조영(UGI) 검사여부(Y / N)
                            strGBGFS = list1[i].GBGFS;                              //내시경검사 여부(Y / N)
                            strGBMAMMO = list1[i].GBMAMMO;                          //유방검사 여부(Y / N)
                            strGBRECUTM = list1[i].GBRECUTM;                        //분변검사 여부(Y / N)
                            strGBSONO = list1[i].GBSONO;                            //초음파검사 여부(Y / N)
                            strGBWOMB = list1[i].GBWOMB;                            //자궁경부암 여부(Y/ N)
                            strGBBOHUM = list1[i].GBBOHUM;                          //1차검진(Y / N)
                            strGBGFSH = list1[i].GBGFSH;                            //내시경검사(종검) 여부(Y / N)
                            strGBCOLON = list1[i].GBCOLON;                          //본관 대장내시경 여부(Y / N)
                            strGBCT = list1[i].GBCT;                                //CT검사 여부(Y / N)
                            strGBLUNG_SANGDAM = list1[i].GBLUNG_SANGDAM;            //폐암 사후상담여부(Y / N)
                        }

                        if((strJong[0] =="Y" && strGBUGI == "Y") || (strJong[1] == "Y" && strGBGFS == "Y") || (strJong[2] == "Y" && strGBGFSH == "Y") || (strJong[3] == "Y" && strGBMAMMO == "Y") ||
                            (strJong[4] == "Y" && strGBRECUTM == "Y") || (strJong[5] == "Y" && strGBSONO == "Y") ||(strJong[6] == "Y" && strGBWOMB == "Y") || (strJong[7] == "Y" && strGBBOHUM == "Y") ||
                            (strJong[8] == "Y" && strGBCOLON == "Y") ||(strJong[9] == "Y" && strGBCT == "Y") ||  (strJong[10] == "Y" && strGBLUNG_SANGDAM == "Y"))
                        {
                            strOK1 = "OK";
                        }
    
                        if(strOK1== "OK")
                        {
                            MessageBox.Show("해당일자에 이미 같은종류검사가 예약되어있습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    }
                }

               
                //수정인 경우 수정전 자료를 읽어 예약된 인원에 (-) 처리함
                if (!txtRowId.Text.IsNullOrEmpty())
                {
                    HIC_CANCER_RESV2 list = hicCancerResv2Service.GetItembyRowId(txtRowId.Text);

                    if (!list.IsNullOrEmpty())
                    {
                        j = long.Parse(VB.Mid(list.RTIME.ToString(), 9,2))-1;

                        nTime = long.Parse(long.Parse(VB.Pstr(list.AMPM, ":", 1)) * 60 + VB.Pstr(list.AMPM, ":", 2));
                        if (string.Compare(list.AMPM, "13:30") < 0)
                        {
                            if (list.GBUGI == "Y")
                            {
                                FnSet[0, j, 1, 0] -= 1;
                            }
                            if (list.SDOCT.IsNullOrEmpty())
                            {
                                if (list.GBGFS == "Y")
                                {
                                    FnSet[0, j, 1, 1] -= 1;
                                }
                                if (list.GBGFSH == "Y")
                                {
                                    FnSet[0, j, 1, 2] -= 1;
                                }
                            }
                            if (list.GBMAMMO == "Y") { FnSet[0, j, 1, 3] -= 1; }
                            if (list.GBRECUTM == "Y") { FnSet[0, j, 1, 4] -= 1; }
                            if (list.GBCOLON == "Y") { FnSet[0, j, 1, 5] -= 1; }
                            if (list.GBSONO == "Y") { FnSet[0, j, 1, 6] -= 1; }
                            if (list.GBWOMB == "Y") { FnSet[0, j, 1, 7] -= 1; }
                            if (list.GBBOHUM == "Y") { FnSet[0, j, 1, 8] -= 1; }
                            if (list.GBCT == "Y") { FnSet[0, j, 1, 9] -= 1; }
                            if (list.GBLUNG_SANGDAM == "Y") { FnSet[0, j, 1, 10] -= 1; }
                        }
                        else
                        {
                            if (list.GBUGI == "Y") FnSet[1, j, 1, 0] -= 1;
                            if (list.SDOCT.IsNullOrEmpty())
                            {
                                if (list.GBGFS == "Y") FnSet[1, j, 1, 1] -= 1;
                                if (list.GBGFSH == "Y") FnSet[1, j, 1, 2] -= 1;
                            }
                            if (list.GBMAMMO == "Y") FnSet[1, j, 1, 3] -= 1;
                            if (list.GBRECUTM == "Y") FnSet[1, j, 1, 4] -= 1;
                            if (list.GBCOLON == "Y") { FnSet[1, j, 1, 5] -= 1; }
                            if (list.GBSONO == "Y") FnSet[1, j, 1, 6] -= 1;
                            if (list.GBWOMB == "Y") FnSet[1, j, 1, 7] -= 1;
                            if (list.GBBOHUM == "Y") FnSet[1, j, 1, 8] -= 1;
                            if (list.GBCT == "Y") FnSet[1, j, 1, 9] -= 1;
                            if (list.GBLUNG_SANGDAM == "Y") FnSet[1, j, 1, 10] -= 1;
                        }
                    }
                }

                //12시이후에 접수시 오후에 세팅
                nRtime = (long.Parse(VB.Pstr(txtRTime.Text, ":", 1)) * 60) + (long.Parse(VB.Pstr(txtRTime.Text, ":", 2)));
                if (nRtime < 720)
                {
                    //오전
                    //예약된 인원       1개추가    제한인원(예약제한)
                    if (nJong[0] > 0 && FnSet[0, nDay, 1, 0] + nJong[0] > FnSet[0, nDay, 0, 0]) { strOK = "N"; }       //오전 UGI
                    
                    if (txtHopeDr.Text.IsNullOrEmpty())
                    {
                        if (nJong[1] > 0 && FnSet[0, nDay, 1, 1] + nJong[1] + nPoscoAmCount2 > FnSet[0, nDay, 0, 1]) { strOK = "N"; }   //GFS
                        if (nJong[2] > 0 && FnSet[0, nDay, 1, 2] + nJong[2] + nPoscoAmCount2> FnSet[0, nDay, 0, 2]) { strOK = "N"; }   //GFS(종검)
                    }
                    if (nJong[3] > 0 && FnSet[0, nDay, 1, 3] + nJong[3] > FnSet[0, nDay, 0, 3]) { strOK = "N"; }      //유방
                    if (nJong[4] > 0 && FnSet[0, nDay, 1, 4] + nJong[4] > FnSet[0, nDay, 0, 4]) { strOK = "N"; }      //결직
                    if (nJong[8] > 0 && FnSet[0, nDay, 1, 5] + nJong[8] > FnSet[0, nDay, 0, 5]) { strOK = "N"; }      //COLON
                    if (nJong[5] > 0 && FnSet[0, nDay, 1, 6] + nJong[5] + nPoscoAmCount1 > FnSet[0, nDay, 0, 6]) { strOK = "N"; }      //SONO
                    if (nJong[6] > 0 && FnSet[0, nDay, 1, 7] + nJong[6] > FnSet[0, nDay, 0, 7]) { strOK = "N"; }      //자궁경부
                    if (nJong[7] > 0 && FnSet[0, nDay, 1, 8] + nJong[7] > FnSet[0, nDay, 0, 8]) { strOK = "N"; }      //건진1
                    if (nJong[9] > 0 && FnSet[0, nDay, 1, 9] + nJong[9] + nPoscoAmCount3 > FnSet[0, nDay, 0, 9]) { strOK = "N"; }      //CT
                    if (nJong[10] > 0 && FnSet[0, nDay, 1, 10] + nJong[10] > FnSet[0, nDay, 0, 10]) { strOK = "N"; }    //폐암사후상담
                }
                else
                {
                    //오후
                    //예약된 인원       1개추가    제한인원(예약제한)
                    if (nJong[0] > 0 && FnSet[1, nDay, 1, 0] + nJong[0] > FnSet[1, nDay, 0, 0]) { strOK = "N"; }
                    if (txtHopeDr.Text.IsNullOrEmpty())
                    {
                        if (nJong[1] > 0 && FnSet[1, nDay, 1, 1] + nJong[0] + nPoscoPmCount2 > FnSet[1, nDay, 0, 1]) { strOK = "N"; }
                        if (nJong[2] > 0 && FnSet[1, nDay, 1, 2] + nJong[1] + nPoscoPmCount2 > FnSet[1, nDay, 0, 2]) { strOK = "N"; }
                    }
                    if (nJong[3] > 0 && FnSet[1, nDay, 1, 3] + nJong[3] > FnSet[1, nDay, 0, 3]) { strOK = "N"; }
                    if (nJong[4] > 0 && FnSet[1, nDay, 1, 4] + nJong[4] > FnSet[1, nDay, 0, 4]) { strOK = "N"; }
                    if (nJong[8] > 0 && FnSet[1, nDay, 1, 5] + nJong[8] > FnSet[1, nDay, 0, 5]) { strOK = "N"; }
                    if (nJong[5] > 0 && FnSet[1, nDay, 1, 6] + nJong[5] + nPoscoPmCount1 > FnSet[1, nDay, 0, 6]) { strOK = "N"; }
                    if (nJong[6] > 0 && FnSet[1, nDay, 1, 7] + nJong[6] > FnSet[1, nDay, 0, 7]) { strOK = "N"; }
                    if (nJong[7] > 0 && FnSet[1, nDay, 1, 8] + nJong[7] > FnSet[1, nDay, 0, 8]) { strOK = "N"; }
                    if (nJong[9] > 0 && FnSet[1, nDay, 1, 9] + nJong[9] + nPoscoPmCount3 > FnSet[1, nDay, 0, 9]) { strOK = "N"; }
                    if (nJong[10] > 0 && FnSet[1, nDay, 1, 10] + nJong[10] > FnSet[1, nDay, 0, 10]) { strOK = "N"; }
                }

                //당일 예약부도자 추가접수 → 2018-04-20(당일접수도 예약인원 초과시 불가능하도록 설정요청(김동열 부장)
                if (strOK == "N")
                {
                    fn_Data_Display();
                    MessageBox.Show("예약인원이 초과되었습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //건진번호가 없을경우 강제로 생성함
                if (txthicPano.Text.IsNullOrEmpty())
                {
                    FnPano = hb.New_PatientNo_Create();

                    clsDB.setBeginTran(clsDB.DbCon);

                    switch (VB.Left(txtJumin2.Text, 1))
                    {
                        case "1":
                        case "3":
                        case "5":
                        case "7":
                        case "9":
                            strSex = "M";
                            break;
                        case "2":
                        case "4":
                        case "6":
                        case "8":
                            strSex = "F";
                            break;
                        default:
                            strSex = "";
                            break;
                    }

                    //신규번호를 건강진단 환자마스타에 INSERT
                    result = hicPatientService.InsertItem(FnPano, VB.Left(strJumin, 7) + "******", clsAES.AES(strJumin), txtSName.Text, txtTel.Text, txtPaNo.Text, strSex, 0);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                    clsDB.setCommitTran(clsDB.DbCon);
                }

                strPANO = txtPaNo.Text.Trim();
                clsDB.setBeginTran(clsDB.DbCon);
                if (strOK == "Y")
                {
                    HIC_CANCER_RESV2 item = new HIC_CANCER_RESV2();

                    item.RTIME = Convert.ToDateTime(dtpRDate.Text + " " + txtRTime.Text);
                    item.JUMIN = VB.Left(strJumin, 7) + "******";
                    item.SNAME = txtSName.Text;
                    item.TEL = strTel;
                    item.HPHONE = strHPhone;
                    item.GBAM1 = strAm[0];
                    item.GBAM2 = strAm[1];
                    item.GBAM3 = strAm[2];
                    item.GBAM4 = strAm[3];
                    item.GBUGI = strJong[0];
                    item.GBGFS = strJong[1];
                    item.GBGFSH = strJong[2];
                    item.GBMAMMO = strJong[3];
                    item.GBRECUTM = strJong[4];
                    item.GBSONO = strJong[5];
                    item.GBWOMB = strJong[6];
                    item.GBBOHUM = strJong[7];
                    item.GBCOLON = strJong[8];
                    item.GBCT = strJong[9];
                    item.GBLUNG_SANGDAM = strJong[10];
                    item.PANO = strPANO;
                    item.REMARK = txtRemark.Text;
                    item.SMSOK = strSms;
                    item.SDOCT = txtHopeDr.Text;
                    item.ENTSABUN = long.Parse(clsType.User.IdNumber);
                    item.JUMIN2 = clsAES.AES(strJumin);
                    item.ROWID = txtRowId.Text.Trim();

                    if (txtRowId.Text.IsNullOrEmpty())  //신규등록
                    {
                        result = hicCancerResv2Service.Insert(item);
                    }
                    else
                    {
                        result = hicCancerResv2Service.Update(item);
                    }
                }
                else if (chkRemark.Checked == true && !txtRowId.Text.IsNullOrEmpty())
                {
                    result = hicCancerResv2Service.UpdateRemark(txtRemark.Text, txtRowId.Text);
                }
                else
                {
                    MessageBox.Show("예약이 초과되었습니다.", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (result < 0)
                {
                    MessageBox.Show("DB 등록 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                if (!txtHPhone.Text.IsNullOrEmpty() && !txtRowId.Text.IsNullOrEmpty() && FstrOLD_RDate != dtpRDate.Text)
                //if (!txtHPhone.Text.IsNullOrEmpty() && FstrOLD_RDate != dtpRDate.Text)
                {
                    //====================== 알림톡 전환시 사용 ========================'
                    Alim.Clear_ATK_Varient();
                    strTempCD = "C_MJ_001_02_12452";

                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = dtpRDate.Text + " " + txtRTime.Text;
                    clsHcType.ATK.SendUID = txtHPhone.Text + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = txtPaNo.Text;
                    clsHcType.ATK.sName = txtSName.Text;
                    clsHcType.ATK.HPhone = txtHPhone.Text;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "A";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.Dept = "HR";
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = "";
                    clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);

                    clsHcType.ATK.ATMsg = Alim.READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = Alim.READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", txtSName.Text);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(dtpRDate.Text, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(dtpRDate.Text, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(dtpRDate.Text, 9, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{HH}", txtRTime.Text);

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", txtSName.Text);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(dtpRDate.Text, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(dtpRDate.Text, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(dtpRDate.Text, 9, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{HH}", txtRTime.Text);

                    if (Alim.INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        return;
                    }
                }

                //회사코드 업데이트
                if (!txthicPano.Text.IsNullOrEmpty())
                {
                    if (long.Parse(txthicPano.Text) > 0)
                    {
                        if (txtLtdCode.Text.IsNullOrEmpty())
                        {
                            result = hicPatientService.UpdateLtdCodebyPano(0, long.Parse(txthicPano.Text));
                        }
                        else
                        {
                            result = hicPatientService.UpdateLtdCodebyPano(long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1)), long.Parse(txthicPano.Text));
                        }
                    }
                }
               
                strDate = dtpRDate.Text;
                fn_Screen_Clear();
                fn_SS2_Display(strDate);
                fn_Data_Display();
                eBtnClick(btnSearch, new EventArgs());
                txtJumin1.Focus();
            }
            else if (sender == btnNhic)
            {
                string strTemp = "";
                string strTemp2 = "";
                string strGkiho = "";
                string strAmTemp = "";
                string strNhicChk = "";
                string strYear = "";
                string strJumin = "";
                string strJong = "";
                string str생애구분 = "";
                int[] nGbAm = new int[5];
                int nREAD = 0;
                long nAge = 0;
                bool bOK = false;
                int result = 0;

                //txtRowId.Text = "";
                fn_SS3_Clear();

                for (int i = 0; i <= 4; i++)
                {
                    nGbAm[i] = 0;
                }

                nAge = (long)hb.READ_HIC_AGE_GESAN(txtJumin1.Text + txtJumin2.Text);

                strYear = VB.Left(clsPublic.GstrSysDate, 4);
                strJong = (nAge == 40 || nAge == 66) ? "35" : "31";

                str생애구분 = "N";
                if (strJong == "35")
                {
                    str생애구분 = "Y";
                }

                strJumin = txtJumin1.Text + txtJumin2.Text;

                //항상 신규로 조회하게 함
                if (!strJumin.IsNullOrEmpty())
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    result = workNhicService.DeleteDataAllByJuminNo(strJumin);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("자격조회 History 삭제시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                }

                fn_Hic_Chk_Nhic("H", txtSName.Text, strJumin, txtPaNo.Text, strYear, str생애구분);

                grpNhic.Visible = true;

            }
            else if (sender == btnNhic2)
            {
                int result = 0;

                if (!txtJumin1.Text.IsNullOrEmpty() && !txtJumin2.Text.IsNullOrEmpty())
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    result = workNhicService.DeleteDataAllByJuminNo(txtJumin1.Text + txtJumin2.Text);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("자격조회 History 삭제시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);

                    eBtnClick(btnNhic, new EventArgs());
                }
            }
            else if (sender == btnCancel)
            {
                fn_Screen_Clear();
                FstrRetValue = "";
                FPatSelectFlag = "";
            }
            else if (sender == btnPrint)
            {
                if (SS3.ActiveSheet.Cells[4, 2].Text.IsNullOrEmpty())
                {
                    MessageBox.Show("인쇄할 환자를 선택하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string strPrintName = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = false;

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                clsSpread SP = new clsSpread();
                clsPrint CP = new clsPrint();

                //strPrintName = CP.getPrinter_Chk("접수증");

                setMargin = new clsSpread.SpdPrint_Margin(80, 30, 30, 20, 70, 10);
                setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, false, false, true, true, true, false, false);

                SP.setSpdPrint(SS3, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);

                MessageBox.Show("출력 되었습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == btnPrint1)
            {
                int nREAD = 0;
                int nRow = 0;
                string strJobDate = "";
                string strTemp = "";
                string strRemark = "";
                string strAmPm = "";
                string strTempDate = "";
                bool blnHoly = true;
                string strFDate = "";
                string strTDate = "";

                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = false;

                strJobDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(1).ToShortDateString();

                while (blnHoly == true)
                {
                    strTempDate = basJobService.GetHolyDay(strJobDate);
                    if (strTempDate != "*")
                    {
                        blnHoly = false;
                        break;
                    }
                    strJobDate = DateTime.Parse(strJobDate).AddDays(1).ToShortDateString();
                }

                strJobDate = VB.InputBox("인쇄할 일자를 입력하세요(종료=Q): ", "인쇄일자", strJobDate);
                if (strJobDate == "Q" || strJobDate == "q")
                {
                    return;
                }
                if (VB.IsDate(strJobDate) == false)
                {
                    MessageBox.Show("날짜 형식을 YYYY-MM-DD 헝식으로 입력하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                strAmPm = VB.InputBox("오전,오후를 선택하세요(1.오전 2.오후 3.전체 Q.종료): ", "오전/오후 선택", "1");
                if (strAmPm == "Q" || strAmPm == "q")
                {
                    return;
                }
                if (strAmPm != "1" && strAmPm != "2" && strAmPm != "3")
                {
                    MessageBox.Show("오전/오후를 정확하게 선택하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strFDate = strJobDate + " 00:00";
                strTDate = strJobDate + " 23:59";

                List<HIC_CANCER_RESV2> list = hicCancerResv2Service.GetItembyRTime6(strAmPm, strFDate, strTDate);

                nREAD = list.Count;
                SS4.ActiveSheet.RowCount = nREAD + 2;
                for (int i = 0; i < nREAD; i++)
                {
                    SS4.ActiveSheet.Cells[i, 0].Text = list[i].SNAME;
                    strTemp = list[i].GBBOHUM;
                    if (strTemp == "Y")
                    {
                        SS4.ActiveSheet.Cells[i, 2].Text = "◎";
                    }
                    SS4.ActiveSheet.Cells[i, 3].Text = "";
                    strTemp = list[i].GBUGI;
                    if (strTemp == "Y")
                    {
                        SS4.ActiveSheet.Cells[i, 5].Text = "◎";
                    }
                    strTemp = list[i].GBGFS;
                    if (strTemp == "Y")
                    {
                        SS4.ActiveSheet.Cells[i, 6].Text = "◎";
                    }
                    strTemp = list[i].GBGFSH;
                    if (strTemp == "Y")
                    {
                        SS4.ActiveSheet.Cells[i, 7].Text = "◎";
                    }
                    strTemp = list[i].GBMAMMO;
                    if (strTemp == "Y")
                    {
                        SS4.ActiveSheet.Cells[i, 8].Text = "◎";
                    }
                    strTemp = list[i].GBRECUTM;
                    if (strTemp == "Y")
                    {
                        SS4.ActiveSheet.Cells[i, 9].Text = "◎";
                    }
                    strTemp = list[i].GBCOLON;
                    if (strTemp == "Y")
                    {
                        SS4.ActiveSheet.Cells[i, 10].Text = "◎";
                    }
                    strTemp = list[i].GBSONO;
                    if (strTemp == "Y")
                    {
                        SS4.ActiveSheet.Cells[i, 11].Text = "◎";
                    }
                    strTemp = list[i].GBWOMB;
                    if (strTemp == "Y")
                    {
                        SS4.ActiveSheet.Cells[i, 12].Text = "◎";
                    }
                    strTemp = list[i].GBBOHUM;
                    if (strTemp == "Y")
                    {
                        SS4.ActiveSheet.Cells[i, 13].Text = "◎";
                    }
                    strTemp = list[i].GBCT;
                    if (strTemp == "Y")
                    {
                        SS4.ActiveSheet.Cells[i, 14].Text = "◎";
                    }
                    strRemark = "";
                    if (!list[i].SDOCT.IsNullOrEmpty())
                    {
                        strRemark = "★" + list[i].SDOCT + ",";
                    }
                    strRemark += list[i].REMARK;
                    SS4.ActiveSheet.Cells[i, 15].Text = strRemark;
                    //암검진문진표 여부
                    for (int j = 5; j <= 12; j++)
                    {
                        if (SS4.ActiveSheet.Cells[i, j].Text == "◎")
                        {
                            SS4.ActiveSheet.Cells[i, 3].Text = "◎";
                            break;
                        }
                    }
                }

                SS4.ActiveSheet.Cells[nREAD + 2 - 1, 0].Text = "추가인원";

                if (strAmPm == "1")
                {
                    strTitle = "예약일자: " + strJobDate + "일 (오전)";
                }
                else if (strAmPm == "2")
                {
                    strTitle = "예약일자: " + strJobDate + "일 (오후)";
                }
                else
                {
                    strTitle = "예약일자: " + strJobDate + "일 (전체)";
                }

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("출력 일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + VB.Space(82) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 30, 180, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, true, false, false);

                sp.setSpdPrint(SS4, PrePrint, setMargin, setOption, strHeader, strFooter);

                MessageBox.Show("출력 되었습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == btnDelete)
            {
                string strDate = "";
                string strDelDate = "";
                string strMsg = "";
                string strName = "";
                string strTel = "";
                string strRettel = "";
                string strTime = "";
                string strData = "";
                string strTempCD = "";
                int result = 0;

                strDelDate = "1900" + VB.Right(dtpRDate.Text, 6) + " " + txtRTime.Text;

                if (txtRowId.Text.IsNullOrEmpty())
                {
                    return;
                }

                if (MessageBox.Show("정말로 삭제를 하시겠습니까?", "자료삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                result = hicCancerResv2Service.UpdateRTimeEntTimeEntSabunbyRowId(strDelDate, clsType.User.IdNumber, txtRowId.Text.Trim());

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("삭제오류!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                if (!txtHPhone.Text.IsNullOrEmpty())
                {
                    //====================== 알림톡 전환시 사용 ========================'
                    Alim.Clear_ATK_Varient();
                    strTempCD = "C_MJ_001_02_12475";

                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = dtpRDate.Text + " " + txtRTime.Text;
                    clsHcType.ATK.SendUID = txtHPhone.Text + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = txtPaNo.Text;
                    clsHcType.ATK.sName = txtSName.Text;
                    clsHcType.ATK.HPhone = txtHPhone.Text;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "A";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.Dept = "HR";
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = "";
                    clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);

                    clsHcType.ATK.ATMsg = Alim.READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = Alim.READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", txtSName.Text);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(dtpRDate.Text, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(dtpRDate.Text, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(dtpRDate.Text, 9, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{HH}", txtRTime.Text);

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", txtSName.Text);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(dtpRDate.Text, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(dtpRDate.Text, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(dtpRDate.Text, 9, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{HH}", txtRTime.Text);

                    if (Alim.INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        return;
                    }
                }

                strDate = dtpRDate.Text;
                fn_Screen_Clear();
                fn_SS2_Display(strDate);
                fn_Data_Display();
            }
            else if (sender == btnNewPtNo)
            {
                string strPtNo = "";
                string strJumin1 = "";
                string strJumin2 = "";
                string strSname = "";
                string strPano1 = "";
                string strJiCode = "";

                string strSex = "";
                string strBirth = "";
                string strSDate = "";
                string strEdate = "";
                string strGbBirth = "";
                string strZipCode1 = "";
                string strZipCode2 = "";
                string strJuso1 = "";
                string strJuso2 = "";
                string strTel = "";
                string strHPhone = "";
                string strGbSMS = "";
                string strDeptCode = "";
                string strDRCODE = "";
                string strROWID = "";
                string BAS_PATIENT_INSERT = "";
                string strPANO = "";

                int result = 0;

                strPtNo = txtPaNo.Text;
                strPANO = txthicPano.Text;
                strJumin1 = txtJumin1.Text;
                strJumin2 = txtJumin2.Text;

                if (strJumin1.Length != 6 || strJumin2.Length != 7)
                {
                    BAS_PATIENT_INSERT = "오류";
                    return;
                }

                if (txtSName.Text.IsNullOrEmpty())
                {
                    BAS_PATIENT_INSERT = "오류";
                    return;
                }

                if (txtHPhone.Text.IsNullOrEmpty())
                {
                    BAS_PATIENT_INSERT = "오류";
                    return;
                }

                //등록번호가 있으면 환자마스타에 있는지 점검
                strROWID = "";
                if (!strPtNo.IsNullOrEmpty())
                {
                    BAS_PATIENT list = basPatientService.GetPaNoRowIdbyPaNo(strPtNo);

                    if (!list.IsNullOrEmpty())
                    {
                        if (list.PANO == strPtNo)
                        {
                            strROWID = list.RID;
                        }
                    }
                }

                //환자마스타가 있는지 점검함
                if (strROWID.IsNullOrEmpty())
                {
                    BAS_PATIENT list2 = basPatientService.GetPaNoRowIdbyJumin1Jumin3(strJumin1, clsAES.AES(strJumin2));

                    if (!list2.IsNullOrEmpty())
                    {
                        strPtNo = list2.PANO;
                        strROWID = list2.RID;
                        txtPaNo.Text = strPtNo;
                        return;
                    }
                }

                //환자마스타에 업데이트
                if (!txtSName.Text.IsNullOrEmpty())
                {
                    strSname = txtSName.Text.Trim();
                }
                else
                {
                    strSname = "";
                }
                strSex = "";
                if (VB.Left(txtJumin2.Text, 1) == "1" || VB.Left(txtJumin2.Text, 1) == "3" || VB.Left(txtJumin2.Text, 1) == "5" || VB.Left(txtJumin2.Text, 1) == "7" || VB.Left(txtJumin2.Text, 1) == "9")
                {
                    strSex = "M";
                }
                if (VB.Left(txtJumin2.Text, 1) == "2" || VB.Left(txtJumin2.Text, 1) == "4" || VB.Left(txtJumin2.Text, 1) == "6" || VB.Left(txtJumin2.Text, 1) == "8")
                {
                    strSex = "F";
                }

                strBirth = "";
                strGbBirth = "";
                strSDate = dtpRDate.Text;
                strEdate = dtpRDate.Text;
                strTel = txtTel.Text;
                strHPhone = txtHPhone.Text;
                strGbSMS = "";
                strDeptCode = "HR";
                strDRCODE = "7101";

                //원무과 요청으로 지역번호 054는 표시 안함
                if (VB.Left(strTel, 4) == "054-")
                {
                    strTel = VB.Right(strTel, strTel.Length - 4);
                }

                if (!strROWID.IsNullOrEmpty())
                {
                    result = basPatientService.UpdateSNameHphoneTelbyRowId(strSname, strHPhone, strTel, strROWID);

                    if (result < 0)
                    {
                        MessageBox.Show("환자 마스터 갱신중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                //---------( 병원의 신환번호를 부여함 )----------
                strPano1 = comHpcLibBService.GetNewPanobySeq().ToString();
                if (strPano1.IsNullOrEmpty())
                {
                    BAS_PATIENT_INSERT = "오류";
                    return;
                }

                strPtNo = hb.PANO_LAST_CHAR(strPano1);

                //------( 환자마스타에 INSERT )-----------
                BAS_PATIENT item = new BAS_PATIENT();

                item.PANO = strPtNo;
                item.SNAME = strSname;
                item.SEX = strSex;
                item.JUMIN1 = strJumin1;
                item.JUMIN2 = VB.Left(strJumin2, 1) + "******";
                
                item.STARTDATE = strSDate;
                item.LASTDATE = strEdate;
                item.ZIPCODE1 = strZipCode1;
                item.ZIPCODE2 = strZipCode2;
                item.JUSO = strJuso2;
                item.JICODE = strJiCode;
                item.TEL = strTel;
                item.HPHONE = strHPhone;
                item.EMBPRT = " ";
                item.BI = "51";
                item.PNAME = strSname;
                item.GWANGE = "1";
                item.KIHO = "";
                item.GKIHO = "";
                item.DEPTCODE = strDeptCode;
                item.DRCODE = strDRCODE;
                item.GBSPC = "0";
                item.GBGAMEK = "00";
                item.BOHUN = "";
                item.REMARK = "";
                item.SABUN = "";
                item.BUNUP = "";
                item.BIRTH = strBirth;
                item.GBBIRTH = strGbBirth;
                item.EMAIL = "";
                item.GBINFOR = "";
                item.GBJUSO = "";
                item.GBSMS = strGbSMS;
                item.HPHONE2 = "";
                item.JUMIN3 = clsAES.AES(strJumin2);

                result = basPatientService.Insert(item);

                if (result < 0)
                {
                    MessageBox.Show("환자 마스터 생성 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    BAS_PATIENT_INSERT = "오류";
                    return;
                }

                txtPaNo.Text = strPtNo;

                if (hicPatientService.GetCountbyPaNo(strPANO) > 0)
                {
                    result = hicPatientService.UpdatePtNobyPaNo(strPtNo, strPANO);

                    if (result < 0)
                    {
                        MessageBox.Show("검진 환자 마스터 갱신중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            else if (sender == btnHelp)
            {
                string strSName = "";
                string nREAD = "";
                string strSDate = "";
                string strEDate = "";
                string strHeaSDate = "";
                string strPtNo = "";
                string strROWID = "";
                string[] strRsvRowId = new string[1];
                string strRsvName = "";
                string strRsvRTime = "";
                List<string> strRsvRTimeList = new List<string>();
                List<string> strRsvRowIdList = new List<string>();
                List<string> strGBUGI = new List<string>();
                List<string> strGBGFS = new List<string>();
                List<string> strGBGFSH = new List<string>();
                List<string> strGBMAMMO = new List<string>();
                List<string> strGBCT = new List<string>();
                List<string> strGBRECUTM = new List<string>();
                List<string> strGBCOLON = new List<string>();
                List<string> strGBSONO = new List<string>();
                List<string> strGBWOMB = new List<string>();
                List<string> strGBBOHUM = new List<string>();
                List<string> strGBLUNG_SANGDAM = new List<string>();


                ComFunc.ReadSysDate(clsDB.DbCon);

                strSDate = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01 00:00";
                strEDate = VB.Left(clsPublic.GstrSysDate, 4) + "-12-31 23:59";

                if (!txtJumin1.Text.IsNullOrEmpty())
                {
                    txtJumin1.Text = txtJumin1.Text.Trim();
                }
                if (!txtJumin2.Text.IsNullOrEmpty())
                {
                    txtJumin2.Text = txtJumin2.Text.Trim();
                }

                if (txtJumin1.Text.Length != 6)
                {
                    MessageBox.Show("주민번호(1)을 정확하게 입력하세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtJumin1.Focus();
                    return;
                }

                if (txtJumin2.Text.Length != 7)
                {
                    MessageBox.Show("주민번호(2)을 정확하게 입력하세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtJumin2.Focus();
                    return;
                }

                //주민등록번호 체크
                if (ComFunc.JuminNoCheck(clsDB.DbCon, txtJumin1.Text, txtJumin2.Text) == "주민번호 체크 오류!")
                {
                    string sMsg = "";
                    sMsg = "▶주민등록번호 오류◀" + "\r\n" + "\r\n";
                    sMsg += "주민등록번호에 오류가 있습니다." + "\r\n";
                    sMsg += "다시 한번 더 확인해 주십시오?" + "\r\n";
                    if (MessageBox.Show(sMsg, "선택", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                //주민번호를 건진번호 확인
                string strJumin = clsAES.AES(txtJumin1.Text + txtJumin2.Text);
                HIC_PATIENT list = hicPatientService.GetItembyJumin2NotInSName(strJumin, clsHcVariable.B04_NOT_PATIENT);

                if (!list.IsNullOrEmpty())
                {
                    txthicPano.Text = list.PANO.ToString();
                    txtLtdCode.Text = list.LTDCODE.ToString() + "." + hb.READ_Ltd_Name(list.LTDCODE.ToString());
                }
                else
                {
                    txthicPano.Text = "";
                    txtLtdCode.Text = "";
                }

                //BAS_Patient 에 동일한 주민등록번호가 있는지 Check
                BAS_PATIENT list2 = basPatientService.GetItembyJumin1Jumin3NotInSName(txtJumin1.Text, clsAES.AES(txtJumin2.Text), clsHcVariable.B04_NOT_PATIENT);

                if (!list2.IsNullOrEmpty())
                {
                    strSName = list2.SNAME;
                    txtTel.Text = list2.TEL;
                    txtHPhone.Text = list2.HPHONE;
                    txtPaNo.Text = list2.PANO;
                    strPtNo = list2.PANO;
                    txtSName.Text = strSName;
                }

                //종검예약 여부 점검
                strHeaSDate = heaJepsuService.GetSDatebyPtNo(strPtNo);

                if (!strHeaSDate.IsNullOrEmpty())
                {
                    MessageBox.Show(strHeaSDate + "일 종합검진 예약 수검자입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                List<HIC_CANCER_RESV2> list3 = hicCancerResv2Service.GetRTimebyJumin2RTime(strJumin, strSDate, strEDate);

                Array.Resize(ref strRsvRowId, list3.Count);
                strRsvRTimeList.Clear();
                strRsvRowIdList.Clear();
                strROWID = "";

                FSaveFlag = "";

                if (list3.Count > 0)
                {
                    for (int i = 0; i < list3.Count; i++)
                    {
                        strRsvRowId[i] = list3[i].ROWID;
                        strRsvRowIdList.Add(list3[i].ROWID); 
                        strRsvName = list3[0].SNAME;
                        strRsvRTime += VB.Left(list3[i].RTIMESYSDATE.ToString(), 19) + ", ";
                        strRsvRTimeList.Add(list3[i].RTIMESYSDATE.ToString());
                        strGBUGI.Add(list3[i].GBUGI);
                        strGBGFS.Add(list3[i].GBGFS);
                        strGBGFSH.Add(list3[i].GBGFSH);
                        strGBMAMMO.Add(list3[i].GBMAMMO);
                        strGBCT.Add(list3[i].GBCT);
                        strGBRECUTM.Add(list3[i].GBRECUTM);
                        strGBCOLON.Add(list3[i].GBCOLON);
                        strGBSONO.Add(list3[i].GBSONO);
                        strGBWOMB.Add(list3[i].GBWOMB);
                        strGBBOHUM.Add(list3[i].GBBOHUM);
                        strGBLUNG_SANGDAM.Add(list3[i].GBLUNG_SANGDAM);
                    }
                    strRsvRTime = VB.Mid(strRsvRTime, 1, strRsvRTime.Trim().Length - 1);
                }

                if (list3.Count == 1)
                {
                    if (MessageBox.Show(strRsvName + "님은 [" + strRsvRTime + "] 에 예약이 되어있습니다" + "\r\n" + "기 예약을 수정을 하시려면 [예], 신규 예약을 하시려면 [아니오] 를 선택하세요!", "확인요망", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (!strRsvRTime.IsNullOrEmpty())
                        {
                            FSaveFlag = "MODIFY";
                            dtpRDate.Text = VB.Left(strRsvRTime, 10);
                            txtRTime.Text = VB.Right(strRsvRTime, 5);
                            strROWID = strRsvRowId[0];
                        }
                        else
                        {
                            FSaveFlag = "NEW";
                            dtpRDate.Text = clsPublic.GstrSysDate;
                            txtRTime.Text = "";                            
                            strROWID = "";
                            FstrRowId = "";
                        }
                    }
                    else
                    {
                        FSaveFlag = "NEW";
                        dtpRDate.Text = clsPublic.GstrSysDate;
                        txtRTime.Text = "";                        
                        strROWID = "";
                        FstrRowId = "";
                    }
                }
                else if (list3.Count > 1)
                {
                    if (MessageBox.Show(strRsvName + "님은 [" + strRsvRTime + "] 에 예약이 되어있습니다" + "\r\n" + "기 예약을 수정을 하시려면 [예], 신규 예약을 하시려면 [아니오] 를 선택하세요!", "확인요망", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        FrmHcAmRsvSelect = new frmHcAmRsvSelect(strRsvRowIdList, strRsvName, strRsvRTimeList, 
                                                                strGBUGI, strGBGFS, strGBGFSH, strGBMAMMO, 
                                                                strGBCT, strGBRECUTM, strGBCOLON, strGBSONO, 
                                                                strGBWOMB, strGBBOHUM, strGBLUNG_SANGDAM);
                        FrmHcAmRsvSelect.rSetGstrValue += new frmHcAmRsvSelect.SetGstrValue(RsvDate_value);
                        FrmHcAmRsvSelect.StartPosition = FormStartPosition.CenterParent;
                        FrmHcAmRsvSelect.ShowDialog();
                        FrmHcAmRsvSelect.rSetGstrValue -= new frmHcAmRsvSelect.SetGstrValue(RsvDate_value);

                        if (!strRsvRTimeList.IsNullOrEmpty())
                        {
                            FSaveFlag = "MODIFY";
                            dtpRDate.Text = VB.Left(FstrRTime, 10);
                            txtRTime.Text = VB.Right(FstrRTime, 5);
                            strROWID = FstrRowId;
                        }
                        else
                        {
                            FSaveFlag = "NEW";
                            dtpRDate.Text = clsPublic.GstrSysDate;
                            txtRTime.Text = "";
                            strROWID = "";
                            FstrRowId = "";
                        }
                    }
                    else
                    {
                        FSaveFlag = "NEW";
                        dtpRDate.Text = clsPublic.GstrSysDate;
                        txtRTime.Text = "";                        
                        strROWID = "";
                        FstrRowId = "";
                    }
                }

                chkJong0.Checked = false;
                chkJong1.Checked = false;
                chkJong2.Checked = false;
                chkJong3.Checked = false;
                chkJong9.Checked = false;
                chkJong4.Checked = false;
                chkJong8.Checked = false;
                chkJong5.Checked = false;
                chkJong6.Checked = false;
                chkJong7.Checked = false;
                chkSMS.Checked = true;
                txtSabun.Text = "";
                txtRowId.Text = "";

                if (!strROWID.IsNullOrEmpty())
                {
                    HIC_CANCER_RESV2 list4 = hicCancerResv2Service.GetItembyRowId(strROWID);

                    chkJong0.Checked = list4.GBUGI == "Y" ? true : false;
                    chkJong1.Checked = list4.GBGFS == "Y" ? true : false;
                    chkJong2.Checked = list4.GBGFSH == "Y" ? true : false;
                    chkJong3.Checked = list4.GBMAMMO == "Y" ? true : false;
                    chkJong4.Checked = list4.GBRECUTM == "Y" ? true : false;
                    chkJong5.Checked = list4.GBSONO == "Y" ? true : false;
                    chkJong6.Checked = list4.GBWOMB == "Y" ? true : false;
                    chkJong7.Checked = list4.GBBOHUM == "Y" ? true : false;
                    chkJong8.Checked = list4.GBCOLON == "Y" ? true : false;
                    chkJong9.Checked = list4.GBCT == "Y" ? true : false;
                    chkJong10.Checked = list4.GBLUNG_SANGDAM == "Y" ? true : false;

                    if (list4.IsNullOrEmpty())
                    {
                        chkSMS.Checked = true;
                    }
                    else
                    {
                        chkSMS.Checked = list4.SMSOK == "Y" ? true : false;
                    }
                    dtpRDate.Text = VB.Left(list4.RTIME.ToString(), 10);
                    txtRTime.Text = list4.AMPM.ToString();
                    //txtSabun.Text = hb.READ_HIC_InsaName(list4.ENTSABUN.ToString());
                    txtSabun.Text = clsHelpDesk.READ_INSA_NAME(clsDB.DbCon, list4.ENTSABUN.ToString());
                    txtRowId.Text = strROWID;
                }

                if (!txtSName.Text.IsNullOrEmpty() && !txtJumin1.Text.IsNullOrEmpty() && !txtJumin2.Text.IsNullOrEmpty())
                {
                    btnNhic.Enabled = true;
                    btnNhic2.Enabled = true;
                }

                //메모
                fn_Hic_Memo_Screen();

                txtSName.Focus();

                if (txtLtdCode.Text.Trim() == "0.")
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnHis)
            {
                FrmHcPanPersonResult = new frmHcPanPersonResult("frmHcAmReserve", txtPaNo.Text, txtSName.Text);
                FrmHcPanPersonResult.StartPosition = FormStartPosition.CenterScreen;
                FrmHcPanPersonResult.ShowDialog();
                //FrmHcPanPersonResult = null;
                //FrmHcPanPersonResult.Dispose();
            }
            else if (sender == btnJepsuPrt)
            {
                string strRTime = "";
                string strPrintName = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = false;

                strRTime = VB.InputBox("내시경 예약시간을 입력하시기 바랍니다.", "내시경 예약시간 확인", "");

                if (strRTime.IsNullOrEmpty())
                {
                    MessageBox.Show("내시경 예약시간이 공란입니다. 확인하시기 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                clsSpread SP = new clsSpread();
                clsPrint CP = new clsPrint();

                strPrintName = CP.getPrinter_Chk("접수증");

                ssRsvPrt.ActiveSheet.Cells[3, 2].Text = txtSName.Text;
                ssRsvPrt.ActiveSheet.Cells[4, 2].Text = txtJumin1.Text;
                ssRsvPrt.ActiveSheet.Cells[5, 2].Text = txtPaNo.Text;
                ssRsvPrt.ActiveSheet.Cells[6, 2].Text = dtpRDate.Text;

                ssRsvPrt.ActiveSheet.Cells[7, 3].Text = txtRTime.Text;
                ssRsvPrt.ActiveSheet.Cells[8, 3].Text = strRTime;

                ssRsvPrt.ActiveSheet.Cells[10, 1].Text = txtRemark.Text;

                setMargin = new clsSpread.SpdPrint_Margin(2, 10, 5, 10, 5, 10);
                setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, false, false, true, true, true, false, false);

                SP.setSpdPrint(ssRsvPrt, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);

                MessageBox.Show("출력 되었습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == btnLtdView)
            {
                int nREAD = 0;
                int nRead1 = 0;
                string strFDate = "";
                string strTDate = "";
                string strRDate = "";
                string strTemp = "";
                int[] nCNT = new int[11];
                string strJumin = "";

                if (txtViewLtd.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("검색할 회사코드가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (cboViewYYMM.Text == "전체")
                {
                    strFDate = "";
                    strTDate = "";
                }
                else
                {
                    //strFDate = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 6, 2) + "-01";
                    strFDate = cboViewYYMM.Text + "-01";
                    strTDate = cf.READ_LASTDAY(clsDB.DbCon, strFDate);
                }

                for (int i = 0; i <= 10; i++)
                {
                    nCNT[i] = 0;
                }

                //fn_color_White_Set(SS5, "NO", SS5.ActiveSheet.RowCount - 1);
                sp.Spread_All_Clear(SS5);

                string strLtdCode = VB.Pstr(txtViewLtd.Text, ".", 1);

                List<HIC_CANCER_RESV2> list = hicCancerResv2Service.GetItembyLtdCodeRTime(strLtdCode, strFDate, strTDate, cboViewYYMM.Text);
                
                nREAD = list.Count;
                SS5.ActiveSheet.RowCount = 0;
                SS5.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    strJumin = clsAES.DeAES(list[i].JUMIN2);

                    if (hicJepsuPatientService.GetCountbyJumin2(strRDate, clsAES.AES(strJumin), clsHcVariable.B04_NOT_PATIENT) > 0)
                    {
                        fn_color_White_Set(SS5, "", i);
                    }
                    else
                    {
                        fn_color_White_Set(SS5, "NO", i);
                    }

                    SS5.ActiveSheet.Cells[i, 0].Text = list[i].RTIME.ToString();
                    SS5.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    if( list[i].GBUGI == "Y") { SS5.ActiveSheet.Cells[i, 2].Text = "◎"; nCNT[0] += 1;}
                    if (list[i].GBGFS == "Y") { SS5.ActiveSheet.Cells[i, 3].Text = "◎"; nCNT[1] += 1; }
                    if (list[i].GBGFSH == "Y") { SS5.ActiveSheet.Cells[i, 4].Text = "◎"; nCNT[2] += 1; }
                    if (list[i].GBMAMMO == "Y") { SS5.ActiveSheet.Cells[i, 5].Text = "◎"; nCNT[3] += 1; }
                    if (list[i].GBRECUTM == "Y") { SS5.ActiveSheet.Cells[i, 6].Text = "◎"; nCNT[4] += 1; }
                    if (list[i].GBCOLON == "Y") { SS5.ActiveSheet.Cells[i, 7].Text = "◎"; nCNT[5] += 1; }
                    if (list[i].GBSONO == "Y") { SS5.ActiveSheet.Cells[i, 8].Text = "◎"; nCNT[6] += 1; }
                    if (list[i].GBWOMB == "Y") { SS5.ActiveSheet.Cells[i, 9].Text = "◎"; nCNT[7] += 1; }
                    if (list[i].GBBOHUM == "Y") { SS5.ActiveSheet.Cells[i, 10].Text = "◎"; nCNT[8] += 1; }
                    if (list[i].GBCT == "Y") { SS5.ActiveSheet.Cells[i, 11].Text = "◎"; nCNT[9] += 1; }
                    if (list[i].GBLUNG_SANGDAM == "Y") { SS5.ActiveSheet.Cells[i, 12].Text = "◎"; nCNT[10] += 1; }

                    //SS5.ActiveSheet.Cells[i, 2].Text = list[i].GBUGI == "Y" ? "◎" : (nCNT[0] += 0).ToString();
                    //SS5.ActiveSheet.Cells[i, 3].Text = list[i].GBGFS == "Y" ? "◎" : (nCNT[1] += 1).ToString();
                    //SS5.ActiveSheet.Cells[i, 4].Text = list[i].GBGFSH == "Y" ? "◎" : (nCNT[2] += 1).ToString();
                    //SS5.ActiveSheet.Cells[i, 5].Text = list[i].GBMAMMO == "Y" ? "◎" : (nCNT[3] += 1).ToString();
                    //SS5.ActiveSheet.Cells[i, 6].Text = list[i].GBRECUTM == "Y" ? "◎" : (nCNT[4] += 1).ToString();
                    //SS5.ActiveSheet.Cells[i, 7].Text = list[i].GBSONO == "Y" ? "◎" : (nCNT[5] += 1).ToString();
                    //SS5.ActiveSheet.Cells[i, 8].Text = list[i].GBCOLON == "Y" ? "◎" : (nCNT[6] += 1).ToString();
                    //SS5.ActiveSheet.Cells[i, 9].Text = list[i].GBWOMB == "Y" ? "◎" : (nCNT[7] += 1).ToString();
                    //SS5.ActiveSheet.Cells[i, 10].Text = list[i].GBBOHUM == "Y" ? "◎" : (nCNT[8] += 1).ToString();
                    //SS5.ActiveSheet.Cells[i, 11].Text = list[i].GBCT == "Y" ? "◎" : (nCNT[9] += 1).ToString();
                    //SS5.ActiveSheet.Cells[i, 12].Text = list[i].GBLUNG_SANGDAM == "Y" ? "◎" : (nCNT[10] += 1).ToString();
                    SS5.ActiveSheet.Cells[i, 13].Text = list[i].SDOCT;
                    SS5.ActiveSheet.Cells[i, 14].Text = list[i].PANO;
                    SS5.ActiveSheet.Cells[i, 15].Text = list[i].ROWID;

                    fn_color_White_Set(SS5, "GFS", i);
                }

                if (nREAD > 0)
                {
                    SS5.ActiveSheet.RowCount = nREAD + 1;
                    SS5.ActiveSheet.Cells[nREAD, 0].Text = "합계";
                    SS5.ActiveSheet.Cells[nREAD, 2].Text = nCNT[0].ToString();
                    SS5.ActiveSheet.Cells[nREAD, 3].Text = nCNT[1].ToString();
                    SS5.ActiveSheet.Cells[nREAD, 4].Text = nCNT[2].ToString();
                    SS5.ActiveSheet.Cells[nREAD, 5].Text = nCNT[3].ToString();
                    SS5.ActiveSheet.Cells[nREAD, 6].Text = nCNT[4].ToString();
                    SS5.ActiveSheet.Cells[nREAD, 7].Text = nCNT[5].ToString();
                    SS5.ActiveSheet.Cells[nREAD, 8].Text = nCNT[6].ToString();
                    SS5.ActiveSheet.Cells[nREAD, 9].Text = nCNT[7].ToString();
                    SS5.ActiveSheet.Cells[nREAD, 10].Text = nCNT[8].ToString();
                    SS5.ActiveSheet.Cells[nREAD, 11].Text = nCNT[9].ToString();
                    SS5.ActiveSheet.Cells[nREAD, 12].Text = nCNT[10].ToString();

                    fn_color_White_Set(SS5, "GFS", nREAD);
                }
            }
            else if (sender == btnMemoSave)
            {
                fn_Hic_Memo_Save();
                fn_Hic_Memo_Screen();
            }
        }

        void fn_SS2_Display(string argDate)
        {
            int nRead = 0;
            string strTemp = "";
            string strFDate = "";
            string strTDate = "";

            sp.Spread_All_Clear(SS2);

            strFDate = argDate + " 00:00";
            strTDate = argDate + " 23:59";

            tabControl.SelectedTab = tab1;
            tab1.Text = argDate + " 예약자 명단";

            List<HIC_CANCER_RESV2> list = hicCancerResv2Service.GetItembyRTime5(strFDate, strTDate, "");

            nRead = list.Count;
            SS2.ActiveSheet.RowCount = nRead;
            if (nRead > 0)
            {
                for (int i = 0; i < nRead; i++)
                {
                    SS2.ActiveSheet.Cells[i, 0].Text = list[i].RTIME_TIME.ToString();
                    SS2.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    strTemp = list[i].GBUGI;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[i, 2].Text = "◎";
                    }
                    strTemp = list[i].GBGFS;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[i, 3].Text = "◎";
                    }
                    strTemp = list[i].GBGFSH;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[i, 4].Text = "◎";
                    }
                    strTemp = list[i].GBMAMMO;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[i, 5].Text = "◎";
                    }
                    strTemp = list[i].GBRECUTM;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[i, 6].Text = "◎";
                    }                    
                    strTemp = list[i].GBCOLON;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[i, 7].Text = "◎";
                    }
                    strTemp = list[i].GBSONO;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[i, 8].Text = "◎";
                    }
                    strTemp = list[i].GBWOMB;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[i, 9].Text = "◎";
                    }
                    strTemp = list[i].GBBOHUM;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[i, 10].Text = "◎";
                    }
                    strTemp = list[i].GBCT;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[i, 11].Text = "◎";
                    }
                    strTemp = list[i].GBLUNG_SANGDAM;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[i, 12].Text = "◎";
                    }

                    SS2.ActiveSheet.Cells[i, 13].Text = list[i].SDOCT;
                    SS2.ActiveSheet.Cells[i, 14].Text = list[i].PANO;
                    SS2.ActiveSheet.Cells[i, 15].Text = list[i].ROWID;
                }
            }
        }

        void fn_Data_Chk()
        {
             string strTemp = "";

            for (int i = 0; i <= 10; i++)
            {
                CheckBox chkjong = (Controls.Find("chkjong" + i.ToString(), true)[0] as CheckBox);
                strTemp = chkjong.Checked == true ? "Y" : "N";

                if (strTemp == "Y")
                {
                    break;
                }
            }

            if (txtSName.Text.IsNullOrEmpty())
            {
                strTemp = "N";
            }
            if (txtJumin1.Text.IsNullOrEmpty() || txtJumin2.Text.IsNullOrEmpty())
            {
                strTemp = "N";
            }
            if (dtpRDate.Text.IsNullOrEmpty() || txtRTime.Text.IsNullOrEmpty())
            {
                strTemp = "N";
            }
            if (txtTel.Text.IsNullOrEmpty())
            {
                strTemp = "N";
            }

            if (strTemp == "N")
            {
                MessageBox.Show("빠진 데이타가 있습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FstrChk = "Y";
                return;
            }
        }
        
        void fn_SS3_Clear()
        {
            for (int i = 4; i <= 11; i++)
            {
                SS3.ActiveSheet.Cells[i, 2].Text = "";
                SS3.ActiveSheet.Cells[i, 5].Text = "";
            }
            for (int i = 13; i <= 14; i++)
            {
                SS3.ActiveSheet.Cells[i, 1].Text = "";
            }
        }

        void fn_Hic_Memo_Save()
        {
            long nPano = 0;
            string strCode = "";
            string strMemo = "";
            string strROWID = "";
            string strOK = "";
            string strTime = "";
            int result = 0;

            nPano = long.Parse(txthicPano.Text);

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < SS_ETC.ActiveSheet.RowCount; i++)
            {
                strOK = SS_ETC.ActiveSheet.Cells[i, 0].Text;
                strTime = SS_ETC.ActiveSheet.Cells[i, 1].Text;
                strMemo = SS_ETC.ActiveSheet.Cells[i, 2].Text;
                strROWID = SS_ETC.ActiveSheet.Cells[i, 4].Text;

                if (!strROWID.IsNullOrEmpty())
                {
                    if (strOK == "True")
                    {
                        result = hicMemoService.DeleteData(strROWID);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("수검자 메모 저장 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                else if (strTime.IsNullOrEmpty() && !strMemo.IsNullOrEmpty())
                {
                    HIC_MEMO item = new HIC_MEMO();

                    item.PANO = nPano;
                    item.MEMO = strMemo;
                    item.JOBSABUN = long.Parse(clsType.User.IdNumber);
                    item.PTNO = txtPaNo.Text;

                    result = hicMemoService.Insert(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("수검자 메모 저장 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void fn_Hic_Memo_Screen()
        {
            long nPano = 0;
            int nRead = 0;

            sp.Spread_All_Clear(SS_ETC);
            SS_ETC.ActiveSheet.RowCount = 5;

            if (!txthicPano.Text.IsNullOrEmpty())
            {
                nPano = long.Parse(txthicPano.Text);
            }
            

            if (nPano == 0)
            {
                return;
            }

            //참고사항 Display
            List<HIC_MEMO> list = hicMemoService.GetItembyPaNo(nPano);

            nRead = list.Count;
            SS_ETC.ActiveSheet.RowCount = nRead + 5;
            for (int i = 0; i < nRead; i++)
            {
                SS_ETC.ActiveSheet.Cells[i, 1].Text = list[i].ENTTIME.ToString();
                SS_ETC.ActiveSheet.Cells[i, 2].Text = list[i].MEMO;
                //SS_ETC.ActiveSheet.Cells[i, 3].Text = hb.READ_HIC_InsaName(list[i].JOBSABUN.ToString());
                SS_ETC.ActiveSheet.Cells[i, 3].Text = clsHelpDesk.READ_INSA_NAME(clsDB.DbCon, list[i].JOBSABUN.ToString());
                SS_ETC.ActiveSheet.Cells[i, 4].Text = list[i].RID;
            }

            for (int i = 0; i < SS_ETC.ActiveSheet.RowCount; i++)
            {
                Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 2);
                SS_ETC.ActiveSheet.Rows[i].Height = size.Height;
            }
        }

        void fn_color_White_Set(FpSpread spdNm, string strGb, int nRow)
        {
            if (strGb == "")
            {
                spdNm.ActiveSheet.Cells[nRow, 0, nRow, spdNm.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                spdNm.ActiveSheet.Cells[nRow, 0, nRow, spdNm.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(217, 255, 217);
            }
            else if (strGb == "NO")
            {
                spdNm.ActiveSheet.Cells[nRow, 0, nRow, spdNm.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                spdNm.ActiveSheet.Cells[nRow, 0, nRow, spdNm.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
            }
            else if (strGb == "GFS")
            {
                spdNm.ActiveSheet.Cells[nRow, 4, nRow, 4].ForeColor = Color.FromArgb(0, 0, 0);
                spdNm.ActiveSheet.Cells[nRow, 4, nRow, 4].BackColor = Color.FromArgb(255, 255, 202);
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(SS1, e.Column, ref boolSort, true);
                    return;
                }
            }
            else if (sender == SS2)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(SS2, e.Column, ref boolSort, true);                    
                    return;
                }
            }
            else if (sender == SS5)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(SS5, e.Column, ref boolSort, true);
                    return;
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                int[] nCNT = new int[11];

                int nREAD = 0;
                int nRead1 = 0;
                int nPoscoCount1 = 0;
                int nPoscoCount2 = 0;
                int nPoscoCount3 = 0;
                int nCurRow = 0;

                string strRDate = "";
                string strTemp = "";
                string strJumin = "";
                string strTempFDate = "";
                string strTempTDate = "";
                string strSort = "";
                string strLtdCode = "";




                for (int i = 0; i <= 10; i++)
                {
                    nCNT[i] = 0;
                }
                //if (SS2.ActiveSheet.RowCount > 0)
                //{
                //    fn_color_White_Set(SS2, "NO", SS2.ActiveSheet.RowCount - 1);
                //}
                sp.Spread_All_Clear(SS2);

                strRDate = SS1.ActiveSheet.Cells[e.Row, 16].Text;
                if (strRDate.IsNullOrEmpty())
                {
                    strRDate = SS1.ActiveSheet.Cells[e.Row - 1, 16].Text;
                }

                if (!FstrRetValue.IsNullOrEmpty())
                {
                    //날짜불러온것
                    if (FstrRetValue.Length == 10)
                    {
                        strRDate = FstrRetValue;
                    }
                }

                tabControl.SelectedTab = tab1;
                tab1.Text = strRDate + " 예약자 명단";

                strTempFDate = strRDate + " 00:00";
                strTempTDate = strRDate + " 23:59";

                if (rdoSort0.Checked == true)
                {
                    strSort = "1";
                }
                else if (rdoSort1.Checked == true)
                {
                    strSort = "2";
                }

                List<HIC_CANCER_RESV2> list = hicCancerResv2Service.GetItembyRTime5(strTempFDate, strTempTDate, strSort);

                nREAD = list.Count;
                SS2.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    strJumin = clsAES.DeAES(list[i].JUMIN2);

                    nCurRow = i + 1;

                    HIC_JEPSU_PATIENT list2 = hicJepsuPatientService.GetLtdCodebyRDateJumin2SName(strRDate, clsAES.AES(strJumin), clsHcVariable.B04_NOT_PATIENT);

                    if (!list2.IsNullOrEmpty())
                    {
                        fn_color_White_Set(SS2, "", nCurRow - 1);
                        SS2.ActiveSheet.Cells[nCurRow - 1, 15].Text = hb.READ_Ltd_Name(list2.LTDCODE.ToString());
                    }
                    else
                    {
                        fn_color_White_Set(SS2, "NO", nCurRow - 1);
                        SS2.ActiveSheet.Cells[nCurRow - 1, 15].Text = "";
                    }

                    SS2.ActiveSheet.Cells[nCurRow - 1, 0].Text = list[i].RTIME_TIME;
                    SS2.ActiveSheet.Cells[nCurRow - 1, 1].Text = list[i].SNAME;
                    strTemp = list[i].GBUGI;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[nCurRow - 1, 2].Text = "◎";
                        nCNT[0] += 1;
                    }

                    if (!list[i].SDOCT.IsNullOrEmpty())
                    {
                        strTemp = list[i].GBGFS;
                        if (strTemp == "Y")
                        {
                            SS2.ActiveSheet.Cells[nCurRow - 1, 3].Text = "●";
                            nCNT[1] += 1;
                        }
                        strTemp = list[i].GBGFSH;
                        if (strTemp == "Y")
                        {
                            SS2.ActiveSheet.Cells[nCurRow - 1, 4].Text = "●";
                            nCNT[2] += 1;
                        }
                    }
                    else
                    {
                        strTemp = list[i].GBGFS;
                        if (strTemp == "Y")
                        {
                            SS2.ActiveSheet.Cells[nCurRow - 1, 3].Text = "◎";
                            nCNT[1] += 1;
                        }
                        strTemp = list[i].GBGFSH;
                        if (strTemp == "Y")
                        {
                            SS2.ActiveSheet.Cells[nCurRow - 1, 4].Text = "◎";
                            nCNT[2] += 1;
                        }
                    }
                    strTemp = list[i].GBMAMMO;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[nCurRow - 1, 5].Text = "◎";
                        nCNT[3] += 1;
                    }
                    strTemp = list[i].GBRECUTM;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[nCurRow - 1, 6].Text = "◎";
                        nCNT[4] += 1;
                    }
                    strTemp = list[i].GBCOLON;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[nCurRow - 1, 7].Text = "◎";
                        nCNT[5] += 1;
                    }
                    strTemp = list[i].GBSONO;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[nCurRow - 1, 8].Text = "◎";
                        nCNT[6] += 1;
                    }
                    strTemp = list[i].GBWOMB;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[nCurRow - 1, 9].Text = "◎";
                        nCNT[7] += 1;
                    }
                    strTemp = list[i].GBBOHUM;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[nCurRow - 1, 10].Text = "◎";
                        nCNT[8] += 1;
                    }
                    strTemp = list[i].GBCT;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[nCurRow - 1, 11].Text = "◎";
                        nCNT[9] += 1;
                    }
                    strTemp = list[i].GBLUNG_SANGDAM;
                    if (strTemp == "Y")
                    {
                        SS2.ActiveSheet.Cells[nCurRow - 1, 12].Text = "◎";
                        nCNT[10] += 1;
                    }
                    SS2.ActiveSheet.Cells[nCurRow - 1, 13].Text = list[i].SDOCT;
                    SS2.ActiveSheet.Cells[nCurRow - 1, 14].Text = list[i].PANO;
                    SS2.ActiveSheet.Cells[nCurRow - 1, 15].Text = list[i].ROWID;

                    fn_color_White_Set(SS2, "GFS", nCurRow - 1);
                }

                if (nREAD > 0)
                {
                    SS2.ActiveSheet.RowCount += 1;
                    SS2.ActiveSheet.Cells[nREAD, 0].Text = "합계";
                    SS2.ActiveSheet.Cells[nREAD, 2].Text = nCNT[0].ToString();
                    SS2.ActiveSheet.Cells[nREAD, 3].Text = nCNT[1].ToString();
                    SS2.ActiveSheet.Cells[nREAD, 4].Text = nCNT[2].ToString();
                    SS2.ActiveSheet.Cells[nREAD, 5].Text = nCNT[3].ToString();
                    SS2.ActiveSheet.Cells[nREAD, 6].Text = nCNT[4].ToString();
                    SS2.ActiveSheet.Cells[nREAD, 7].Text = nCNT[5].ToString();
                    SS2.ActiveSheet.Cells[nREAD, 8].Text = nCNT[6].ToString();
                    SS2.ActiveSheet.Cells[nREAD, 9].Text = nCNT[7].ToString();
                    SS2.ActiveSheet.Cells[nREAD, 10].Text = nCNT[8].ToString();
                    SS2.ActiveSheet.Cells[nREAD, 11].Text = nCNT[9].ToString();
                    SS2.ActiveSheet.Cells[nREAD, 12].Text = nCNT[10].ToString();
                    fn_color_White_Set(SS2, "GFS", SS2.ActiveSheet.RowCount - 1);
                }



                //포스코위탁인원표시
                lblPosco.Text = "";

                nPoscoCount1 = 0;
                nPoscoCount2 = 0;
                nPoscoCount3 = 0;

                List<COMHPC> list1 = comHpcLibBService.GetPoscoCount(strRDate);
                if ( list1.Count>0 )
                {
                    for (int i = 0; i < list1.Count; i++)
                    {
                        if (!list1[i].EXAMRES1.IsNullOrEmpty())
                        {
                            if (VB.Pstr(list1[i].HIC, "^^", 1) == "1")
                            {
                                nPoscoCount1 = nPoscoCount1 + 1;
                            }
                        }

                        if (!list1[i].EXAMRES2.IsNullOrEmpty())
                        {
                            if (VB.Pstr(list1[i].HIC, "^^", 3) == "1")
                            {

                                nPoscoCount2 = nPoscoCount2 + 1;

                            }
                        }

                        if (!list1[i].EXAMRES3.IsNullOrEmpty())
                        {
                            if (VB.Pstr(list1[i].HIC, "^^", 4) == "1")
                            {
                                nPoscoCount2 = nPoscoCount2 + 1;

                            }
                        }
                        if (!list1[i].EXAMRES7.IsNullOrEmpty())
                        {
                            if (VB.Pstr(list1[i].HIC, "^^", 6) == "1")
                            {

                                nPoscoCount3 = nPoscoCount3 + 1;
                            }
                        }
                    }

                    lblPosco.Text = "포스코위탁 "+"복부초음파: " + nPoscoCount1 + "건, " + "위내시경: " + nPoscoCount2 + "건, " + "흉부CT: " + nPoscoCount3 + "건";
                }
            }
            else if (sender == SS2)
            {
                string strROWID = "";
                string[] strChk = new string[11];
                string strJumin = "";

                if (e.RowHeader == true) return;
                if (e.ColumnHeader == true) return;

                FPatSelectFlag = "SS2";

                for (int i = 0; i <= 10; i++)
                {
                    strChk[i] = "N";
                }

                strROWID = SS2.ActiveSheet.Cells[e.Row, 15].Text;
                txtRowId.Text = strROWID;

                FSaveFlag = "MODIFY";

                HIC_CANCER_RESV2 list = hicCancerResv2Service.GetItembyRowId(strROWID);

                if (list.IsNullOrEmpty())
                {
                    chkSMS.Checked = true;
                    return;
                }

                strJumin = clsAES.DeAES(list.JUMIN2);
                txtSName.Text = list.SNAME;
                txtJumin1.Text = VB.Left(strJumin, 6);
                txtJumin2.Text = VB.Right(strJumin, 7);
                dtpRDate.Text = VB.Left(list.RTIME.ToString(), 10);
                FstrOLD_RDate = dtpRDate.Text;
                txtRTime.Text = list.AMPM;
                txtTel.Text = list.TEL; 
                txtHPhone.Text = list.HPHONE;
                txtPaNo.Text = list.PANO;
                txtHopeDr.Text = list.SDOCT;
                txtRemark.Text = list.REMARK;
                //txtSabun.Text = hb.READ_HIC_InsaName(list.ENTSABUN.ToString());
                txtSabun.Text = clsHelpDesk.READ_INSA_NAME(clsDB.DbCon, list.ENTSABUN.ToString());

                if (list.GBAM1 == "Y") chkAm0.Checked = true;
                if (list.GBAM2 == "Y") chkAm1.Checked = true;
                if (list.GBAM3 == "Y") chkAm2.Checked = true;
                if (list.GBAM4 == "Y") chkAm3.Checked = true;

                chkJong0.Checked = list.GBUGI == "Y" ? true : false;
                chkJong1.Checked = list.GBGFS == "Y" ? true : false;
                chkJong2.Checked = list.GBGFSH == "Y" ? true : false;
                chkJong3.Checked = list.GBMAMMO == "Y" ? true : false;
                chkJong4.Checked = list.GBRECUTM == "Y" ? true : false;
                chkJong5.Checked = list.GBSONO == "Y" ? true : false;
                chkJong6.Checked = list.GBWOMB == "Y" ? true : false;
                chkJong7.Checked = list.GBBOHUM == "Y" ? true : false;
                chkJong8.Checked = list.GBCOLON == "Y" ? true : false;
                chkJong9.Checked = list.GBCT == "Y" ? true : false;
                chkJong10.Checked = list.GBLUNG_SANGDAM == "Y" ? true : false;

                chkSMS.Checked = list.SMSOK == "Y" ? true : false;

                if (chkJong0.Checked == true) { strChk[0] = "Y"; }
                if (chkJong1.Checked == true) { strChk[1] = "Y"; }
                if (chkJong2.Checked == true) { strChk[2] = "Y"; }
                if (chkJong3.Checked == true) { strChk[3] = "Y"; }
                if (chkJong9.Checked == true) { strChk[4] = "Y"; }
                if (chkJong4.Checked == true) { strChk[5] = "Y"; }
                if (chkJong8.Checked == true) { strChk[6] = "Y"; }
                if (chkJong5.Checked == true) { strChk[7] = "Y"; }
                if (chkJong6.Checked == true) { strChk[8] = "Y"; }
                if (chkJong10.Checked == true) { strChk[10] = "Y"; }

                HIC_PATIENT list2 = hicPatientService.GetPaNoLtdCodebytJumin2(clsAES.AES(strJumin));

                if (!list2.IsNullOrEmpty())
                {
                    txthicPano.Text = list2.PANO.ToString();
                    if (list2.LTDCODE.ToString() == "0")
                    {
                        txtLtdCode.Text = "";
                    }
                    else
                    {
                        txtLtdCode.Text = list2.LTDCODE.ToString() + "." + hb.READ_Ltd_Name(list2.LTDCODE.ToString());
                    }
                }
                else
                {
                    txthicPano.Text = "";
                    txtLtdCode.Text = "";
                }

                if (txtLtdCode.Text.Trim() == "0.")
                {
                    txtLtdCode.Text = "";
                }

                //예약증
                fn_SS3_Display();

                //메모
                fn_Hic_Memo_Screen();

                if (e.Column == 0)
                {
                    strChks.Clear();
                    for (int i = 0; i <= 10; i++)
                    {
                        strChks.Add(strChk[i]);
                    }
                    rSetCancerGstrValue(txtPaNo.Text.Trim(), txtRemark.Text.Trim(), strChks);

                    this.Close();
                    return;
                }

                if (!txtSName.Text.IsNullOrEmpty() && !txtJumin1.Text.IsNullOrEmpty() && !txtJumin2.Text.IsNullOrEmpty())
                {
                    btnNhic.Enabled = true;
                    btnNhic2.Enabled = true;
                }
            }
            else if (sender == SS5)
            {
                string strROWID = "";
                string[] strChk = new string[10];
                string strJumin = "";

                FPatSelectFlag = "SS5";

                if (e.RowHeader == true)
                {
                    return;
                }

                for (int i = 0; i <= 9; i++)
                {
                    strChk[i] = "N";
                }

                strROWID = SS5.ActiveSheet.Cells[e.Row, 15].Text;

                HIC_CANCER_RESV2 list = hicCancerResv2Service.GetItembyRowId(strROWID);

                if (list.IsNullOrEmpty())
                {
                    chkSMS.Checked = true;
                    return;
                }

                strJumin = clsAES.DeAES(list.JUMIN2);

                txtSName.Text = list.SNAME;
                txtJumin1.Text = VB.Left(strJumin, 6);
                txtJumin2.Text = VB.Right(strJumin, 7);
                dtpRDate.Text = VB.Left(list.RTIME.ToString(), 10);
                txtRTime.Text = VB.Right(list.RTIME.ToString(), 5);
                txtTel.Text = list.TEL;
                txtHPhone.Text = list.HPHONE;
                txtPaNo.Text = list.PANO;
                txtHopeDr.Text = list.SDOCT;
                txtRemark.Text = list.REMARK;
                //txtSabun.Text = hb.READ_HIC_InsaName(list.ENTSABUN.ToString());
                txtSabun.Text = clsHelpDesk.READ_INSA_NAME(clsDB.DbCon, list.ENTSABUN.ToString());

                if (list.GBAM1 == "Y") { chkAm0.Checked = true; }
                if (list.GBAM2 == "Y") { chkAm1.Checked = true; }
                if (list.GBAM3 == "Y") { chkAm2.Checked = true; }
                if (list.GBAM4 == "Y") { chkAm3.Checked = true; }

                chkJong0.Checked = list.GBUGI == "Y" ? true : false;
                chkJong1.Checked = list.GBGFS == "Y" ? true : false;
                chkJong2.Checked = list.GBGFSH == "Y" ? true : false;
                chkJong3.Checked = list.GBMAMMO == "Y" ? true : false;
                chkJong4.Checked = list.GBRECUTM == "Y" ? true : false;
                chkJong5.Checked = list.GBSONO == "Y" ? true : false;
                chkJong6.Checked = list.GBWOMB == "Y" ? true : false;
                chkJong7.Checked = list.GBBOHUM == "Y" ? true : false;
                chkJong8.Checked = list.GBCOLON == "Y" ? true : false;
                chkJong9.Checked = list.GBCT == "Y" ? true : false;
                chkSMS.Checked = list.SMSOK == "Y" ? true : false;

                if (chkJong0.Checked == true) strChk[0] = "Y";
                if (chkJong1.Checked == true) strChk[1] = "Y";
                if (chkJong2.Checked == true) strChk[2] = "Y";
                if (chkJong3.Checked == true) strChk[3] = "Y";
                if (chkJong4.Checked == true) strChk[4] = "Y";
                if (chkJong5.Checked == true) strChk[5] = "Y";
                if (chkJong6.Checked == true) strChk[6] = "Y";
                if (chkJong7.Checked == true) strChk[7] = "Y";
                if (chkJong8.Checked == true) strChk[8] = "Y";
                if (chkJong9.Checked == true) strChk[9] = "Y";

                HIC_PATIENT list2 = hicPatientService.GetPaNoLtdCodebytJumin2(clsAES.AES(strJumin));

                if (!list2.IsNullOrEmpty())
                {
                    txthicPano.Text = list2.PANO.ToString();
                    txtLtdCode.Text = list2.LTDCODE.ToString() + "." + hb.READ_Ltd_Name(list2.LTDCODE.ToString());
                }
                else
                {
                    txthicPano.Text = "";
                    txtLtdCode.Text = "";
                }

                //예약증
                fn_SS3_Display();

                if (e.Column == 0)
                {
                    strChks.Clear();
                    for (int i = 0; i <= 9; i++)
                    {
                        strChks.Add(strChk[i]);
                    }
                    rSetCancerGstrValue(txtPaNo.Text.Trim(), txtRemark.Text.Trim(), strChks);

                    this.Close();
                    return;
                }

                if (!txtSName.Text.IsNullOrEmpty() && !txtJumin1.Text.IsNullOrEmpty() && !txtJumin2.Text.IsNullOrEmpty())
                {
                    btnNhic.Enabled = true;
                    btnNhic2.Enabled = true;
                }
            }
        }

        void fn_SS3_Display()
        {
            if (!txtRowId.Text.IsNullOrEmpty())
            {
                SS3.ActiveSheet.Cells[4, 2].Text = VB.Space(5) + txtSName.Text;
                SS3.ActiveSheet.Cells[4, 5].Text = VB.Space(5) + txtPaNo.Text;
                SS3.ActiveSheet.Cells[5, 2].Text = VB.Space(5) + txtJumin1.Text + "-" + VB.Left(txtJumin2.Text, 1) + "******";
                SS3.ActiveSheet.Cells[5, 5].Text = VB.Space(5) + txthicPano.Text;
                SS3.ActiveSheet.Cells[6, 2].Text = VB.Space(5) + dtpRDate.Text;
                SS3.ActiveSheet.Cells[6, 5].Text = VB.Space(5) + txtTel.Text;
                SS3.ActiveSheet.Cells[7, 2].Text = VB.Space(5) + txtRTime.Text;
                SS3.ActiveSheet.Cells[7, 5].Text = VB.Space(5) + txtHPhone.Text;
                if (chkJong0.Checked == true)
                {
                    SS3.ActiveSheet.Cells[9, 2].Text = "◎";
                }
                else
                {
                    SS3.ActiveSheet.Cells[9, 2].Text = "";
                }
                if (chkJong1.Checked == true || chkJong2.Checked == true)
                {
                    SS3.ActiveSheet.Cells[9, 5].Text = "◎";
                }
                else
                {
                    SS3.ActiveSheet.Cells[9, 5].Text = "";
                }
                if (chkJong3.Checked == true)
                {
                    SS3.ActiveSheet.Cells[10, 2].Text = "◎";
                }
                else
                {
                    SS3.ActiveSheet.Cells[10, 2].Text = "";
                }
                if (chkJong4.Checked == true)
                {
                    SS3.ActiveSheet.Cells[10, 5].Text = "◎";
                }
                else
                {
                    SS3.ActiveSheet.Cells[10, 5].Text = "";
                }
                if (chkJong5.Checked == true)
                {
                    SS3.ActiveSheet.Cells[11, 2].Text = "◎";
                }
                else
                {
                    SS3.ActiveSheet.Cells[11, 2].Text = "";
                }
                if (chkJong6.Checked == true)
                {
                    SS3.ActiveSheet.Cells[11, 5].Text = "◎";
                }
                else
                {
                    SS3.ActiveSheet.Cells[11, 5].Text = "";
                }

                //폐암
                if (chkJong9.Checked == true)
                {
                    SS3.ActiveSheet.Cells[12, 2].Text = "◎";
                }
                else
                {
                    SS3.ActiveSheet.Cells[12, 2].Text = "";
                }

                if (chkJong7.Checked == true)
                {
                    SS3.ActiveSheet.Cells[12, 5].Text = "◎";
                }
                else
                {
                    SS3.ActiveSheet.Cells[12, 5].Text = "";
                }


                if (txtHopeDr.Text.IsNullOrEmpty())
                {
                    SS3.ActiveSheet.Cells[14, 1].Text = VB.Space(5) + txtRemark.Text;
                }
                else
                {
                    SS3.ActiveSheet.Cells[14, 1].Text = VB.Space(5) + "희망의사:" + txtHopeDr.Text + "," + txtRemark.Text;
                }
            }
        }

        void fn_Screen_Clear()
        {
            txtSName.Text = "";
            txtJumin1.Text = "";
            txtJumin2.Text = "";
            txtRTime.Text = "00:00";
            txtTel.Text = "";
            txtHPhone.Text = "";
            txtRemark.Text = "";
            txtRowId.Text = "";
            txtPaNo.Text = "";
            txthicPano.Text = "";
            txtLtdCode.Text = "";
            txtHopeDr.Text = "";
            chkRemark.Checked = false;
            dtpRDate.Text = clsPublic.GstrSysDate;

            for (int i = 0; i <= 3; i++)
            {
                CheckBox chkAm = (Controls.Find("chkAm" + i.ToString(), true)[0] as CheckBox);
                chkAm.Checked = false;
            }
            for (int i = 0; i <= 10; i++)
            {
                CheckBox chkJong = (Controls.Find("chkJong" + i.ToString(), true)[0] as CheckBox);
                chkJong.Checked = false;
            }
            pnlNhic = null;

            sp.Spread_All_Clear(SS_ETC);

            lblNhic.Text = "예약전 자격조회 확인요망!!";
            chkSMS.Checked = true;
            btnNhic.Enabled = false;
            btnNhic2.Enabled = false;
            grpNhic.Visible = false;

            txtSabun.Text = "";
        }

        void fn_Data_Display()
        {
            int jj = 0;
            int nRow = 0;
            int nCurRow = 0;
            int nREAD = 0;
            int nDay = 0;
            long nLastDay = 0;
            string strGbn = "";
            string strGbName = "";
            string strFDate = "";
            string strTDate = "";
            string strYoil = "";
            string strDate = "";
            long nTime = 0;

            long[,,,] nCNT = new long[2, 31, 2, 11];  //1.오전,2오후, 1~31일,1.예약가능 2.예약인원,1~10:검사종류
            string strWeek = "";    //요일

            //배열 Clear
            for (int L = 0; L <= 1; L++)
            {
                for (int i = 0; i <= 30; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        for (int k = 0; k <= 9; k++)
                        {
                            nCNT[L, i, j, k] = 0;
                            FnSet[L, i, j, k] = 0;
                        }
                    }
                }
            }

            sp.Spread_All_Clear(SS1);

            nRow = 0;

            //strFDate = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 6, 2) + "-01";
            strFDate = cboYYMM.Text + "-01";
            strTDate = cf.READ_LASTDAY(clsDB.DbCon, strFDate);
            nLastDay = long.Parse(VB.Right(strTDate, 2));
            SS1.ActiveSheet.RowCount = (int)nLastDay * 2;

            List<HIC_CANCER_RESV1> list = hicCancerResv1Service.GetItembyJobDate(strFDate, strTDate);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                //해당일자의 일요일/휴일 여부
                strDate = list[i].JOBDATE.ToString();
                strWeek = VB.Left(cf.READ_YOIL(clsDB.DbCon, strDate), 1);

                if (strWeek != "일")
                {
                    if (basJobService.GetHolyDay(strDate) == "*")
                    {
                        strWeek = "휴";
                    }
                }

                nCurRow = (i * 2) + 1;

                SS1.ActiveSheet.Cells[nCurRow - 1, 0].Text = VB.Mid(strDate, 6, 2) + "/" + VB.Right(strDate, 2);
                SS1.ActiveSheet.Cells[nCurRow - 1, 1].Text = strWeek;
                SS1.ActiveSheet.Cells[nCurRow - 1, 2].Text = "AM";
                SS1.ActiveSheet.Cells[nCurRow - 1, 14].Text = list[i].REMARK;
                SS1.ActiveSheet.Cells[nCurRow - 1, 15].Text = list[i].ROWID;
                SS1.ActiveSheet.Cells[nCurRow - 1, 16].Text = list[i].JOBDATE.ToString();

                //휴일은 바탕색 변경
                if (strWeek == "일" || strWeek == "휴")
                {
                    SS1.ActiveSheet.Cells[nCurRow - 1, 0, nCurRow, 10].BackColor = Color.FromArgb(196, 223, 247);
                }

                //일자별 예약가능 인원을 누적함
                if (strWeek != "일" && strWeek != "휴")
                {
                    jj = i + 1;
                    //오전
                    nCNT[0, jj - 1, 0, 0] += list[i].UGI;
                    nCNT[0, jj - 1, 0, 1] += list[i].GFS;
                    nCNT[0, jj - 1, 0, 2] += list[i].GFSH;
                    nCNT[0, jj - 1, 0, 3] += list[i].MAMMO;
                    nCNT[0, jj - 1, 0, 4] += list[i].RECTUM;
                    nCNT[0, jj - 1, 0, 5] += 999;
                    nCNT[0, jj - 1, 0, 6] += list[i].SONO;
                    nCNT[0, jj - 1, 0, 7] += list[i].WOMB;
                    nCNT[0, jj - 1, 0, 8] += list[i].BOHUM;
                    nCNT[0, jj - 1, 0, 9] += list[i].CT;
                    nCNT[0, jj - 1, 0, 10] += list[i].LUNG_SANGDAM;

                    //오후
                    nCNT[1, jj - 1, 0, 0] += list[i].UGI1;
                    nCNT[1, jj - 1, 0, 1] += list[i].GFS1;
                    nCNT[1, jj - 1, 0, 2] += list[i].GFSH1;
                    nCNT[1, jj - 1, 0, 3] += list[i].MAMMO1;
                    nCNT[1, jj - 1, 0, 4] += list[i].RECTUM1;
                    nCNT[1, jj - 1, 0, 5] += 999;
                    nCNT[1, jj - 1, 0, 6] += list[i].SONO1;
                    nCNT[1, jj - 1, 0, 7] += list[i].WOMB1;
                    nCNT[1, jj - 1, 0, 8] += list[i].BOHUM1;
                    nCNT[1, jj - 1, 0, 9] += list[i].CT1;
                    nCNT[1, jj - 1, 0, 10] += list[i].LUNG_SANGDAM1;
                }

                SS1.ActiveSheet.Cells[nCurRow, 2].Text = "PM";
                SS1.ActiveSheet.Cells[nCurRow, 14].Text = list[i].REMARK1;
            }

            //일자별 예약인원을 READ
            strTDate += " 23:59";

            List<HIC_CANCER_RESV2> list2 = hicCancerResv2Service.GetItembyRTime4(strFDate, strTDate);

            if (list2.Count > 0)
            {
                for (int i = 0; i < list2.Count; i++)
                {
                    jj = int.Parse(VB.Right(DateTime.Parse(list2[i].RTIME.ToString()).ToShortDateString(), 2));
                    nTime = long.Parse(VB.Pstr(list2[i].AMPM, ":", 1)) * 60 + long.Parse(VB.Pstr(list2[i].AMPM, ":", 2));
                    if (nTime < 720)
                    {
                        if (list2[i].GBUGI == "Y")
                        {
                            nCNT[0, jj - 1, 1, 0] += 1;
                        }

                        if (list2[i].SDOCT.IsNullOrEmpty())
                        {
                            if (list2[i].GBGFS == "Y") nCNT[0, jj - 1, 1, 1] += 1;
                            if (list2[i].GBGFSH == "Y") nCNT[0, jj - 1, 1, 2] += 1;
                        }

                        if (list2[i].GBMAMMO == "Y") nCNT[0, jj - 1, 1, 3] += 1;
                        if (list2[i].GBRECUTM == "Y") nCNT[0, jj - 1, 1, 4] += 1;
                        if (list2[i].GBCOLON == "Y") nCNT[0, jj - 1, 1, 5] += 1;
                        if (list2[i].GBSONO == "Y") nCNT[0, jj - 1, 1, 6] += 1;
                        if (list2[i].GBWOMB == "Y") nCNT[0, jj - 1, 1, 7] += 1;
                        if (list2[i].GBBOHUM == "Y") nCNT[0, jj - 1, 1, 8] += 1;
                        if (list2[i].GBCT == "Y") nCNT[0, jj - 1, 1, 9] += 1;
                        if (list2[i].GBLUNG_SANGDAM == "Y") nCNT[0, jj - 1, 1, 10] += 1;
                    }
                    else
                    {
                        if (list2[i].GBUGI == "Y") nCNT[1, jj - 1, 1, 0] += 1;
                        if (list2[i].SDOCT.IsNullOrEmpty())
                        { 
                            if (list2[i].GBGFS == "Y") nCNT[1, jj - 1, 1, 1] += 1;
                            if (list2[i].GBGFSH == "Y") nCNT[1, jj - 1, 1, 2] += 1;
                        }
                        if (list2[i].GBMAMMO == "Y") nCNT[1, jj - 1, 1, 3] += 1;
                        if (list2[i].GBRECUTM == "Y") nCNT[1, jj - 1, 1, 4] += 1;
                        if (list2[i].GBCOLON == "Y") nCNT[1, jj - 1, 1, 5] += 1;
                        if (list2[i].GBSONO == "Y") nCNT[1, jj - 1, 1, 6] += 1;
                        if (list2[i].GBWOMB == "Y") nCNT[1, jj - 1, 1, 7] += 1;
                        if (list2[i].GBBOHUM == "Y") nCNT[1, jj - 1, 1, 8] += 1;
                        if (list2[i].GBCT == "Y") nCNT[1, jj - 1, 1, 9] += 1;
                        if (list2[i].GBLUNG_SANGDAM == "Y") nCNT[1, jj - 1, 1, 10] += 1;
                    }                    
                }
            }

            //예약가능인원/예약인원을 Display
            for (int i = 0; i < nLastDay; i++)
            {
                //오전
                nRow += 1;
                nCurRow = nRow;
                for (int j = 0; j <= 10; j++)
                {
                    if (nCNT[0, i, 0, j] == 999)
                    {
                        SS1.ActiveSheet.Cells[nCurRow - 1, j + 3].Text = nCNT[0, i, 1, j].ToString();
                    }
                    else if (nCNT[0, i, 0, j] == 0 && nCNT[0, i, 1, j] == 0)
                    {
                        SS1.ActiveSheet.Cells[nCurRow - 1, j + 3].Text = "-";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[nCurRow - 1, j + 3].Text = nCNT[0, i, 0, j] + "/" + nCNT[0, i, 1, j];
                        if (nCNT[0, i, 0, j] <= nCNT[0, i, 1, j])
                        {
                            SS1.ActiveSheet.Cells[nCurRow - 1, j + 3].BackColor = Color.FromArgb(228, 202, 255);
                        }
                    }
                    FnSet[0, i, 0, j] = nCNT[0, i, 0, j];
                    FnSet[0, i, 1, j] = nCNT[0, i, 1, j]; //예약세팅
                }

                //오후
                nRow += 1;
                nCurRow = nRow;
                for (int j = 0; j <= 10; j++)
                {
                    if (nCNT[1, i, 0, j] == 999)
                    {
                        SS1.ActiveSheet.Cells[nCurRow - 1, j + 3].Text = nCNT[1, i, 1, j].ToString();
                    }
                    else if (nCNT[1, i, 0, j] == 0 && nCNT[1, i, 1, j] == 0)
                    {
                        SS1.ActiveSheet.Cells[nCurRow - 1, j + 3].Text = "-";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[nCurRow - 1, j + 3].Text = nCNT[1, i, 0, j] + "/" + nCNT[1, i, 1, j];
                        if (nCNT[1, i, 0, j] <= nCNT[1, i, 1, j])
                        {
                            SS1.ActiveSheet.Cells[nCurRow - 1, j + 3].BackColor = Color.FromArgb(228, 202, 255);
                        }
                    }
                    FnSet[1, i, 0, j] = nCNT[1, i, 0, j];
                    FnSet[1, i, 1, j] = nCNT[1, i, 1, j]; //예약세팅
                }                
            }
        }

        private void RsvDate_value(string strRTime, string strRowId)
        {
            FstrRTime = strRTime;
            FstrRowId = strRowId;
        }

        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        /// <summary>
        /// 수검자 공단 자격조회 Main Rutine
        /// </summary>
        /// <param name="argGbn"></param>
        /// <param name="argSName"></param>
        /// <param name="argJuminNo"></param>
        /// <param name="argPtno"></param>
        /// <param name="argYear"></param>
        void fn_Hic_Chk_Nhic(string argGbn, string argSName, string argJuminNo, string argPtno, string argYear, string arg생애구분)
        {
            //당일 조회된 자격내역이 있는지 점검
            WORK_NHIC item = workNhicService.GetNhicInfo_Am(argGbn, clsAES.AES(argJuminNo), argSName, argYear, "1", arg생애구분);

            if (item.IsNullOrEmpty())
            {
                //신규자격조회
                FrmHcNhicSub = new frmHcNhicSub(argSName, argJuminNo, argYear, "H", argPtno);
                FrmHcNhicSub.ShowDialog();
            }

            //자격조회 정보 Spread Display
            FrmHcNhicView.SetDisPlay(item);

            //자격조회 정보 변수 대입
            hm.Display_Nhic_Info(item);

            if (clsHcType.THNV.hJaGubun.IsNullOrEmpty())
            {
                MessageBox.Show("자격이 없습니다.", "확인요망");
                return;
            }
        }
    }
}
