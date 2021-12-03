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
using System.ComponentModel;
using System.IO;
using System.Diagnostics;

/// <summary>
/// Class Name      : HC_Sangdam
/// File Name       : frmHcSangTeeth.cs
/// Description     : 상세 내역 리스트
/// Author          : 이상훈
/// Create Date     : 2020-01-07
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHcSang_구강_2019.frm(FrmAct02_2019)" />

namespace HC_Sangdam
{
    public partial class frmHcSangTeeth : Form
    {
        HicResDentalService hicResDentalService = null;
        HicJepsuService hicJepsuService = null;
        HicJepsuDentalSangdamWaitService hicJepsuDentalSangdamWaitService = null;
        HicJepsuResDentalService hicJepsuResDentalService = null;
        HicResDentalJepsuService hicResDentalJepsuService = null;
        HicResSpecialService hicResSpecialService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicPatientService hicPatientService = null;
        HicSangdamWaitService hicSangdamWaitService = null;
        HicWaitRoomService hicWaitRoomService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicExjongService hicExjongService = null;
        HicResultService hicResultService = null;
        HeaJepsuService heaJepsuService = null;
        HeaSangdamWaitService heaSangdamWaitService = null;
        HeaDentalService heaDentalService = null;
        HeaResultService heaResultService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        frmHeaResult FrmHeaResult = null;
        frmHcSangStudentTeeth FrmHcSangStudentTeeth = null;
        frmHcPanSpcGenSecondView FrmHcPanSpcGenSecondView = null;        
        frmViewResult FrmViewResult = null;
        frmHcSangInternetMunjinView FrmHcSangInternetMunjinView = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        long FnWRTNO;
        long FnWrtno2;  //2차 검진시 이전 1차 접수번호
        long FnPano;
        long FnDrno;
        long FnAge;
        string FstrSname;
        string FstrPtno;
        string FstrSex;
        string FstrJepDate;
        string FstrJumin;
        string FstrMunjin;
        string FstrChasu;
        string FstrUCodes;
        string FstrSpcDent;

        string FstrGjJong;
        string FstrGjYear;
        long FnRowNo;       // 메모리타자기 위치 저장용
        long FnClickRow;    // Help를 Click한 Row
        long FnRow;

        string FstrYear;
        string FstrExamFlag;
        bool FbMunjin;

        string FstrROWID;
        long FnHeaWRTNO;    // 종합검진 접수번호
        string FstrWaitRoom;
        string[,] FstrDent = new string[12, 29];    //치아검사결과

        long FnWrtNo;

        public frmHcSangTeeth()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        public frmHcSangTeeth(long nWrtNo)
        {
            InitializeComponent();
            FnWrtNo = nWrtNo;
            SetEvent();
            SetControl();
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void SetEvent()
        {
            hicResDentalService = new HicResDentalService();
            hicJepsuService = new HicJepsuService();
            hicJepsuDentalSangdamWaitService = new HicJepsuDentalSangdamWaitService();
            hicJepsuResDentalService = new HicJepsuResDentalService();
            hicResDentalJepsuService = new HicResDentalJepsuService();
            hicResSpecialService = new HicResSpecialService();
            hicSunapdtlService = new HicSunapdtlService();
            hicPatientService = new HicPatientService();
            hicSangdamWaitService = new HicSangdamWaitService();
            hicWaitRoomService = new HicWaitRoomService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicResultService = new HicResultService();
            hicExjongService = new HicExjongService();
            heaJepsuService = new HeaJepsuService();
            heaSangdamWaitService = new HeaSangdamWaitService();
            heaDentalService = new HeaDentalService();
            heaResultService = new HeaResultService();

            this.Load += new EventHandler(eFormLoad);
            this.FormClosing += new FormClosingEventHandler(eFromClosing);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnDentSave.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnMenuCall.Click += new EventHandler(eBtnClick);
            this.btnMenuNull.Click += new EventHandler(eBtnClick);
            this.btnMenuWard.Click += new EventHandler(eBtnClick); 
            this.btnMenuSchool.Click += new EventHandler(eBtnClick);
            this.btnMenuCancel.Click += new EventHandler(eBtnClick);
            this.btnPACS.Click += new EventHandler(eBtnClick);
            this.btnMed.Click += new EventHandler(eBtnClick);
            this.btnChgPanDate.Click += new EventHandler(eBtnClick);
            this.cboJengsang1.Click += new EventHandler(eComboClick);
            this.cboJengsang2.Click += new EventHandler(eComboClick);
            this.SSHistory.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick); 
            this.rdoJbk1.Click += new EventHandler(eRdoClick);
            this.rdoJbk2.Click += new EventHandler(eRdoClick);
            this.rdoJengsang1.Click += new EventHandler(eRdoClick);
            this.rdoJengsang2.Click += new EventHandler(eRdoClick);
            this.rdoSogen1.Click += new EventHandler(eRdoClick);
            this.rdoSogen2.Click += new EventHandler(eRdoClick);

            this.rdoMunjinByung1.Click += new EventHandler(eRdoClick);
            this.rdoMunjinByung2.Click += new EventHandler(eRdoClick);
            this.rdoMunjinTeeth1.Click += new EventHandler(eRdoClick);
            this.rdoMunjinTeeth2.Click += new EventHandler(eRdoClick);
            this.rdoTeethHygiene1.Click += new EventHandler(eRdoClick);
            this.rdoTeethHygiene2.Click += new EventHandler(eRdoClick);
            this.rdofluoride1.Click += new EventHandler(eRdoClick);
            this.rdofluoride2.Click += new EventHandler(eRdoClick);
            this.rdoSugar1.Click += new EventHandler(eRdoClick);
            this.rdoSugar2.Click += new EventHandler(eRdoClick);
            this.rdoSmoke1.Click += new EventHandler(eRdoClick);
            this.rdoSmoke2.Click += new EventHandler(eRdoClick);
            this.rdoUsikia1.Click += new EventHandler(eRdoClick);
            this.rdoUsikia2.Click += new EventHandler(eRdoClick);
            this.rdoAdjacentSurfaceUsikia1.Click += new EventHandler(eRdoClick);
            this.rdoAdjacentSurfaceUsikia2.Click += new EventHandler(eRdoClick);
            this.rdoSubokchia1.Click += new EventHandler(eRdoClick);
            this.rdoSubokchia2.Click += new EventHandler(eRdoClick);
            this.rdoLosingteeth1.Click += new EventHandler(eRdoClick);
            this.rdoLosingteeth2.Click += new EventHandler(eRdoClick);
            this.rdoGingivitis1.Click += new EventHandler(eRdoClick);
            this.rdoGingivitis2.Click += new EventHandler(eRdoClick);
            this.rdoFloss1.Click += new EventHandler(eRdoClick);
            this.rdoFloss2.Click += new EventHandler(eRdoClick);
            this.rdoTotalPan1.Click += new EventHandler(eRdoClick);
            this.rdoTotalPan2.Click += new EventHandler(eRdoClick);
            this.rdoTotalPan3.Click += new EventHandler(eRdoClick);
            this.rdoTotalPan4.Click += new EventHandler(eRdoClick);

            this.rdoMunjinByung1.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoMunjinByung2.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoMunjinTeeth1.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoMunjinTeeth2.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoTeethHygiene1.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoTeethHygiene2.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdofluoride1.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdofluoride2.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoSugar1.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoSugar2.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoSmoke1.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoSmoke2.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoUsikia1.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoUsikia2.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoAdjacentSurfaceUsikia1.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoAdjacentSurfaceUsikia2.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoSubokchia1.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoSubokchia2.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoLosingteeth1.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoLosingteeth2.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoGingivitis1.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoGingivitis2.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoFloss1.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoFloss2.CheckedChanged += new EventHandler(eRdoChkChanged);

            this.rdoJob1.Click += new EventHandler(eRdoClick);
            this.rdoJob2.Click += new EventHandler(eRdoClick);

            this.txtWrtNo.Click += new EventHandler(eTxtClick);
            this.txtPanDrNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtResultInterpretation.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress); 
            this.txtPanjengJochi.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtSName.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtSName.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtResultInterpretation.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtEtcPartExamOpinion.GotFocus += new EventHandler(eTxtGotFocus); 
            this.txtPanjengJochi.LostFocus += new EventHandler(eTxtLostFocus);
        }

        void eFromClosing(object sender, FormClosingEventArgs e)
        {
            Form frm = hf.OpenForm_Check_Return("frmHcSangInternetMunjinView");

            if (!frm.IsNullOrEmpty())
            {
                frm.Dispose();
            }

            //if (hf.OpenForm_Check("frmHcSangInternetMunjinView") == true)
            //{
            //    FrmHcSangInternetMunjinView.Close();
            //    FrmHcSangInternetMunjinView.Dispose();
            //    FrmHcSangInternetMunjinView = null;
            //}

            ComFunc.KillProc("friendly omr.exe");
            ComFunc.KillProc("hcscript.exe");
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strData = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;

            //판정의사 여부를 읽음
            if (clsHcVariable.GstrProgram == "출장접수")
            {
                rdoChul2.Checked = true;
                chkMunjin.Checked = false;
            }

            hb.READ_HIC_Doctor(clsType.User.IdNumber.To<long>());

            this.Text += "(" + clsHcVariable.GstrHicDrName + ") (면허:" + clsHcVariable.GnHicLicense + ")";

            fn_Screen_Clear();

            List<HIC_EXJONG> list = hicExjongService.Read_ExJong_Add(true);

            if (list.Count > 0)
            {
                cboJong.Items.Clear();
                cboJong.Items.Add("**.전체");
                for (int i = 0; i < list.Count; i++)
                {
                    cboJong.Items.Add(list[i].CODE.Trim() + "." + list[i].NAME);
                }
                cboJong.SelectedIndex = 0;
            }

            //부식증
            cboJengsang1.Items.Clear();
            cboJengsang1.Items.Add(" ");
            cboJengsang1.Items.Add("E0.정상");
            cboJengsang1.Items.Add("E1.법랑질표면부");
            cboJengsang1.Items.Add("E2.법랑질파괴부");
            cboJengsang1.Items.Add("E3.상아질표면부");
            cboJengsang1.Items.Add("E4.2차상아질파괴부");
            cboJengsang1.Items.Add("E5.치주노출부식");
            cboJengsang1.SelectedIndex = 0;

            //교모증
            cboJengsang2.Items.Clear();
            cboJengsang2.Items.Add(" ");
            cboJengsang2.Items.Add("T0.정상");
            cboJengsang2.Items.Add("T1.법랑질파괴부");
            cboJengsang2.Items.Add("T2.상아질파괴부");
            cboJengsang2.Items.Add("T3.교두의완전파괴");
            cboJengsang2.Items.Add("T4.치관치근경계부까지파괴");
            cboJengsang2.SelectedIndex = 0;

            //치주조직검사결과
            cboChijuRes.Items.Clear();
            cboChijuRes.Items.Add(" ");
            cboChijuRes.Items.Add("1.정상");
            cboChijuRes.Items.Add("2.치주염");
            cboChijuRes.Items.Add("3.치은염");
            cboChijuRes.Items.Add("4.치주농루증(풍치)");

            //종합소견
            cboSogen.Items.Clear();
            cboSogen.Items.Add(" ");
            cboSogen.Items.Add("1.정상");
            cboSogen.Items.Add("2.치석제거요함");
            cboSogen.Items.Add("3.치석제거 및 치은 치료가 필요함");

            SSList_Sheet1.Columns.Get(4).Visible = false;

            if (clsHcVariable.GstrProgram == "출장접수")
            {
                btnPACS.Visible = false;
                btnMed.Visible = false;
            }

            //판정의사 여부를 읽음
            hb.READ_HIC_Doctor(clsType.User.IdNumber.To<long>());

            FstrWaitRoom = "";
            chkWait.Checked = false;
            chkWait.Enabled = false;

            if (clsHcVariable.GstrProgram != "출장접수")
            {
                if (clsType.User.IdNumber == "35712")   //김영배
                {
                    chkWait.Enabled = true;
                    chkWait.Checked = true;
                    FstrWaitRoom = "08";
                }
                else if (clsType.User.IdNumber == "37029")  //최영수
                {
                    chkWait.Enabled = true;
                    chkWait.Checked = true;
                    FstrWaitRoom = "09";
                }
                else if (clsType.User.IdNumber == "48766")  //장희숙
                {
                    chkWait.Enabled = true;
                    chkWait.Checked = true;
                    FstrWaitRoom = "08";
                }
            }

            if (!FnWrtNo.IsNullOrEmpty() && FnWrtNo != 0)
            {
                txtWrtNo.Text = FnWrtNo.To<string>();
                eTxtKeyPress(txtWrtNo, new KeyPressEventArgs((char)Keys.Enter));
            }
            else
            {
                eBtnClick(btnSearch, new EventArgs());
            }

            //tabControl1.SelectedTabIndex = 0;
            //tabControl1.SelectedTab = tab1;
        }

        bool FormIsExist(Type tp)
        {
            foreach (Form ff in this.MdiChildren)
            {
                if (ff.GetType() == tp)
                {
                    ff.Focus();
                    ff.BringToFront();
                    return true;
                }
            }
            return false;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                Form frm = hf.OpenForm_Check_Return("frmHcSangInternetMunjinView");

                if (!frm.IsNullOrEmpty())
                {
                    frm.Dispose();
                }

                ComFunc.KillProc("friendly omr.exe");
                ComFunc.KillProc("hcscript.exe");

                this.Close();
                return;
            }
            else if (sender == btnLtdCode)
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
            else if (sender == btnCancel)
            {
                fn_Screen_Clear();
            }
            else if (sender == btnDentSave)
            {
                string strGbn = "";
                string strTemp = "";
                string strBusik = "";
                string strGyomo = "";
                string strResult = "";

                int result = 0;

                //상담내역이 있는지 점검
                FstrROWID = hicResDentalService.GetRowIdbyWrtNo(FnWRTNO);

                if (rdoJengsang1.Checked == true)
                {
                    if (cboJengsang1.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("부식증(증상)이 선택되지 않았습니다.", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else if (rdoJengsang2.Checked == true)
                {
                    if (cboJengsang2.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("교모증(증상)이 선택되지 않았습니다.", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                strResult = ""; strBusik = ""; strGyomo = "";

                for (int i = 1; i <= 28; i++)
                {
                    CheckBox chkTooth = (Controls.Find("chkTooth" + i.ToString(), true)[0] as CheckBox);
                    if (chkTooth.Checked == true)
                    {
                        strResult += "1";
                    }
                    else
                    {
                        strResult += "0";
                    }
                }

                if (rdoJengsang1.Checked == true)
                {
                    strGbn = VB.Left(cboJengsang1.Text, 2);
                }
                else
                {
                    strGbn = VB.Left(cboJengsang2.Text, 2);
                }

                switch (strGbn)
                {
                    case "E1":
                        strBusik = "OK";
                        break;
                    case "E2":
                        strBusik = "OK";
                        break;
                    case "E3":
                        strBusik = "OK";
                        break;
                    case "E4":
                        strBusik = "OK";
                        break;
                    case "E5":
                        strBusik = "OK";
                        break;
                    case "T1":
                        strGyomo = "OK";
                        break;
                    case "T2":
                        strGyomo = "OK";
                        break;
                    case "T3":
                        strGyomo = "OK";
                        break;
                    case "T4":
                        strGyomo = "OK";
                        break;
                    case "":
                        return;
                    default:
                        break;
                }

                if (strGbn == "E0" || strGbn == "T0")
                {
                    if (!FstrROWID.IsNullOrEmpty())
                    {
                        clsDB.setBeginTran(clsDB.DbCon);

                        result = hicResDentalService.UpdateBusikGyomobyWrtNo(strGbn, FnWRTNO);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("구강진찰 내역 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                    }
                }
                else
                {
                    if (!FstrROWID.IsNullOrEmpty())
                    {
                        clsDB.setBeginTran(clsDB.DbCon);

                        result = hicResDentalService.UpdateResultbyWrtNo(strGbn, FnWRTNO, strResult, strBusik, strGyomo);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("구강진찰 내역 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                    }
                }

                fn_SPECDENT_SET(FnWRTNO);
            }
            else if (sender == btnMenuWard)
            {
                frmHcSchoolCommonDistrictRegView f = new frmHcSchoolCommonDistrictRegView();
                f.ShowDialog(this);
            }
            else if (sender == btnMenuCall)
            {
                int result = 0;

                clsDB.setBeginTran(clsDB.DbCon);

                result = hicSangdamWaitService.DeletebyGubun(clsHcVariable.GstrDrRoom);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("삭제중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                result = hicSangdamWaitService.InsertCall(clsHcVariable.GstrDrRoom);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("다음 수검자 호출 실패!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);
            }
            else if (sender == btnMenuNull)
            {
                int result = 0;
                string strMsg = "";

                strMsg = "자리비움 상태를 변경하시겠습니까?" + ComNum.VBLF;
                strMsg = strMsg + "자리비움은 예(Y), 자리비움취소는 아니오(N)를 누르세요." + ComNum.VBLF;


                if (MessageBox.Show(strMsg, "확인사항", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    result = hicSangdamWaitService.DeletebyGubun(clsHcVariable.GstrDrRoom);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("삭제중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    HIC_SANGDAM_WAIT item = new HIC_SANGDAM_WAIT();

                    item.WRTNO = 0;
                    item.SNAME = "{자리비움}";
                    item.GUBUN = clsHcVariable.GstrDrRoom;

                    result = hicSangdamWaitService.InserWrtNoSNameGubunt(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("자리비움 저장 실패", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    clsDB.setCommitTran(clsDB.DbCon);
                }
                else
                {
                    clsDB.setBeginTran(clsDB.DbCon);
                    result = hicSangdamWaitService.DeletebyGubun(clsHcVariable.GstrDrRoom);
                    clsDB.setCommitTran(clsDB.DbCon);
                }

            }
            else if (sender == btnMed)
            {
                FrmHeaResult = new frmHeaResult(FstrJumin);
                FrmHeaResult.StartPosition = FormStartPosition.CenterParent;
                FrmHeaResult.ShowDialog(this);
            }
            else if (sender == btnPACS)
            {
                FrmViewResult = new frmViewResult(FstrPtno);
                FrmViewResult.StartPosition = FormStartPosition.CenterScreen;
                FrmViewResult.ShowDialog(this);

                //frmViewResult f = new frmViewResult(FstrPtno);
                //f.ShowDialog(this);
            }
            else if (sender == btnSave)
            {
                string[] strTPan = new string[12];
                string strTPanEtc = "";
                string[] strTPanjeng = new string[11];
                string strTPanjengEtc = "";
                string strTPanjengJochi = "";
                string strRemark = "";
                string[] strT40Pan = new string[7];
                string strTemp = "";
                string strChijuRes = "";
                string strSpcPanjeng = "";
                string strRES1 = "";
                string strRES2 = "";
                string strJochi = "";
                string strSangdam = "";
                //2019년도 추가사항
                string[] strT40PanNew = new string[7];

                int result = 0;

                ComFunc.ReadSysDate(clsDB.DbCon);

                for (int i = 0; i <= 6; i++)
                {
                    strT40Pan[i] = "";
                    strT40PanNew[i] = "";
                }

                strT40Pan[0] = txt40_Pan1.Text;
                strT40Pan[1] = txt40_Pan2.Text;
                strT40Pan[2] = txt40_Pan3.Text;
                strT40Pan[3] = txt40_Pan4.Text;
                strT40Pan[4] = txt40_Pan5.Text;
                strT40Pan[5] = txt40_Pan6.Text;

                //치면세균막검사-만40세
                for (int i = 0; i < 6; i++)
                {
                    RadioButton rdo40_Pan1_New = (Controls.Find("rdo40_Pan1_New" + i.ToString(), true)[0] as RadioButton);
                    RadioButton rdo40_Pan2_New = (Controls.Find("rdo40_Pan2_New" + i.ToString(), true)[0] as RadioButton);
                    RadioButton rdo40_Pan3_New = (Controls.Find("rdo40_Pan3_New" + i.ToString(), true)[0] as RadioButton);
                    RadioButton rdo40_Pan4_New = (Controls.Find("rdo40_Pan4_New" + i.ToString(), true)[0] as RadioButton);
                    RadioButton rdo40_Pan5_New = (Controls.Find("rdo40_Pan5_New" + i.ToString(), true)[0] as RadioButton);
                    RadioButton rdo40_Pan6_New = (Controls.Find("rdo40_Pan6_New" + i.ToString(), true)[0] as RadioButton);
                    if (rdo40_Pan1_New.Checked == true)
                    {
                        strT40PanNew[0] = i.ToString();
                    }

                    if (rdo40_Pan2_New.Checked == true)
                    {
                        strT40PanNew[1] = i.ToString();
                    }

                    if (rdo40_Pan3_New.Checked == true)
                    {
                        strT40PanNew[2] = i.ToString();
                    }

                    if (rdo40_Pan4_New.Checked == true)
                    {
                        strT40PanNew[3] = i.ToString();
                    }

                    if (rdo40_Pan5_New.Checked == true)
                    {
                        strT40PanNew[4] = i.ToString();
                    }

                    if (rdo40_Pan6_New.Checked == true)
                    {
                        strT40PanNew[5] = i.ToString();
                    }
                }

                strRES1 = "";
                //1.(치과)병력문제
                if (rdoMunjinByung1.Checked == true)
                {
                    strRES1 += "1";
                }
                else if (rdoMunjinByung2.Checked == true)
                {
                    strRES1 += "2";
                }
                else
                {
                    MessageBox.Show("문진평가-(치과)병력 문제 결과 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //2.구강건강인식도문제
                if (rdoMunjinTeeth1.Checked == true)
                {
                    strRES1 += "1";
                }
                else if (rdoMunjinTeeth2.Checked == true)
                {
                    strRES1 += "2";
                }
                else
                {
                    MessageBox.Show("문진평가-구강건강인식도 문제 결과 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //3.구강위생
                if (rdoTeethHygiene1.Checked == true)
                {
                    strRES1 += "1";
                }
                else if (rdoTeethHygiene2.Checked == true)
                {
                    strRES1 += "2";
                }
                else
                {
                    MessageBox.Show("문진평가-구강위생 결과 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //4.불소이용
                if (rdofluoride1.Checked == true)
                {
                    strRES1 += "1";
                }
                else if (rdofluoride2.Checked == true)
                {
                    strRES1 += "2";
                }
                else
                {
                    MessageBox.Show("문진평가-불소이용 결과 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //5.설탕섭취
                if (rdoSugar1.Checked == true)
                {
                    strRES1 += "1";
                }
                else if (rdoSugar2.Checked == true)
                {
                    strRES1 += "2";
                }
                else
                {
                    MessageBox.Show("문진평가-설탕섭취 결과 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //6.흡연
                if (rdoSmoke1.Checked == true)
                {
                    strRES1 += "1";
                }
                else if (rdoSmoke2.Checked == true)
                {
                    strRES1 += "2";
                }
                else
                {
                    MessageBox.Show("문진평가-흡연 결과 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //1.우식치아
                strRES2 = "";
                if (rdoUsikia1.Checked == true)
                {
                    strRES2 += "1";
                }
                else if (rdoUsikia2.Checked == true)
                {
                    strRES2 += "2";
                }
                else
                {
                    MessageBox.Show("치아검사-우식치아 결과 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //2.인접면 우식 의심치아
                if (rdoAdjacentSurfaceUsikia1.Checked == true)
                {
                    strRES2 += "1";
                }
                else if (rdoAdjacentSurfaceUsikia2.Checked == true)
                {
                    strRES2 += "2";
                }
                else
                {
                    MessageBox.Show("치아검사-인접면 우식 의심치아 결과 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //3.수복치아
                if (rdoSubokchia1.Checked == true)
                {
                    strRES2 += "1";
                }
                else if (rdoSubokchia2.Checked == true)
                {
                    strRES2 += "2";
                }
                else
                {
                    MessageBox.Show("치아검사-수복치아 결과 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //4.상실치아
                if (rdoLosingteeth1.Checked == true)
                {
                    strRES2 += "1";
                }
                else if (rdoLosingteeth2.Checked == true)
                {
                    strRES2 += "2";
                }
                else
                {
                    MessageBox.Show("치아검사-상실치아 결과 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //5.치은염증
                if (rdoGingivitis1.Checked == true)
                {
                    strRES2 += "1";
                }
                else if (rdoGingivitis2.Checked == true)
                {
                    strRES2 += "2";
                }
                else if (rdoGingivitis3.Checked == true)
                {
                    strRES2 += "3";
                }
                else
                {
                    MessageBox.Show("치주질환-치은염증 결과 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //6.치석
                if (rdoFloss1.Checked == true)
                {
                    strRES2 += "1";
                }
                else if (rdoFloss2.Checked == true)
                {
                    strRES2 += "2";
                }
                else if (rdoFloss3.Checked == true)
                {
                    strRES2 += "3";
                }
                else
                {
                    MessageBox.Show("치주질환-치석 결과 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //종합판정
                if (rdoTotalPan1.Checked == true)
                {
                    strTemp = "1";
                }
                else if (rdoTotalPan2.Checked == true)
                {
                    strTemp = "2";
                }
                else if (rdoTotalPan3.Checked == true)
                {
                    strTemp = "3";
                }
                else if (rdoTotalPan4.Checked == true)
                {
                    strTemp = "4";
                }
                else
                {
                    MessageBox.Show("종합판정 결과 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                strTPanjeng[0] = strTemp;

                //조치사항
                strJochi = "";
                if (chkAction11.Checked == true)
                {
                    strJochi += "1";
                }
                else
                {
                    strJochi += "0";
                }

                if (chkAction12.Checked == true)
                {
                    strJochi += "1";
                }
                else
                {
                    strJochi += "0";
                }

                if (chkAction13.Checked == true)
                {
                    strJochi += "1";
                }
                else
                {
                    strJochi += "0";
                }

                if (chkAction21.Checked == true)
                {
                    strJochi += "1";
                }
                else
                {
                    strJochi += "0";
                }

                if (chkAction22.Checked == true)
                {
                    strJochi += "1";
                }
                else
                {
                    strJochi += "0";
                }

                if (chkAction23.Checked == true)
                {
                    strJochi += "1";
                }
                else
                {
                    strJochi += "0";
                }

                if (chkAction24.Checked == true)
                {
                    strJochi += "1";
                }
                else
                {
                    strJochi += "0";
                }

                

                //추가조치사항
                strTPanjengJochi = txtPanjengJochi.Text;
                strTPanjengEtc = txtEtcPartExamOpinion.Text;
                strSangdam = txtResultInterpretation.Text;


                //바로조치
                if(strSangdam.IsNullOrEmpty())
                {
                    MessageBox.Show("바로조치 내용이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //적극적관리
                if (strTPanjengJochi.IsNullOrEmpty())
                {
                    MessageBox.Show("적극적관리 내용이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }



                //치주조직검사결과
                if (rdoJbk1.Checked == true)
                {
                    strChijuRes = cboChijuRes.Text.Trim();
                }
                else
                {
                    strChijuRes = txtChijuRes.Text.Trim();
                }

                //종합소견(특수)
                if (rdoSogen1.Checked == true)
                {
                    strSpcPanjeng = cboSogen.Text.Trim();
                }
                else
                {
                    strSpcPanjeng = txtSpcDental.Text.Trim();
                }

                if (txtPanDrNo.Text.To<long>() == 0)
                {
                    MessageBox.Show("상담의사 면허번호 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (tab2.Visible == true)
                {
                    if ( (txtSpcDental.Text.IsNullOrEmpty() || txtSpcDental.Text == "0") && cboSogen.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("구강판정(특수) 누락", "특수판정 종합소견 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                clsDB.setBeginTran(clsDB.DbCon);

                //판정항목

                HIC_RES_DENTAL item = new HIC_RES_DENTAL();

                item.T_PAN_ETC = strTPanEtc;
                item.T40_PAN1 = strT40Pan[0].To<long>();
                item.T40_PAN2 = strT40Pan[1].To<long>();
                item.T40_PAN3 = strT40Pan[2].To<long>();
                item.T40_PAN4 = strT40Pan[3].To<long>();
                item.T40_PAN5 = strT40Pan[4].To<long>();
                item.T40_PAN6 = strT40Pan[5].To<long>();
                item.T40_PAN1_NEW = strT40PanNew[0].To<long>();
                item.T40_PAN2_NEW = strT40PanNew[1].To<long>();
                item.T40_PAN3_NEW = strT40PanNew[2].To<long>();
                item.T40_PAN4_NEW = strT40PanNew[3].To<long>();
                item.T40_PAN5_NEW = strT40PanNew[4].To<long>();
                item.T40_PAN6_NEW = strT40PanNew[5].To<long>();
                item.T_PANJENG1 = strTPanjeng[0];
                item.T_PANJENG_ETC = strTPanjengEtc;
                item.T_PANJENG_SOGEN = strTPanjengJochi;
                item.RES_MUNJIN = strRES1;
                item.RES_RESULT = strRES2;
                item.RES_JOCHI = strJochi;
                item.SANGDAM = strSangdam;
                item.CHIJURESULT = strChijuRes;
                item.CHIJUSTAT1 = txtChiju1.Text.Trim();
                item.CHIJUSTAT2 = txtChiju2.Text.Trim();
                item.CHIJUSTAT3 = txtChiju3.Text.Trim();
                item.CHIJUSTAT4 = txtChiju4.Text.Trim();
                item.CHIJUSTATETC = txtChijuETC.Text.Trim();
                item.PANJENGDRNO = txtPanDrNo.Text.Trim().To<long>(); 
                item.PANJENGDATE = txtPanDate.Text.Trim();
                item.WRTNO = FnWRTNO;

                result = hicResDentalService.UpdateAllbyWrtNo(item);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("판정 저장중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);                    
                    return;
                }

                if (FstrSpcDent == "Y")
                {
                    //result = hicResDentalService.UpdateDentSogenbyWrtNo(strSpcPanjeng, long.Parse(txtPanDrNo.Text), FnWRTNO);
                    result = hicResSpecialService.UpdateDentSogenbyWrtNo(strSpcPanjeng, long.Parse(txtPanDrNo.Text), FnWRTNO);
                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("판정 저장중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);                        
                        return;
                    }
                }

                //상담여부 체크 및 상담의사, 상담일자 저장
                result = hicJepsuService.UpdateGbJinChal2byWrtNo(FnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담여부 저장중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //상담여부 액팅저장
                result = hicResultService.UpdateActivebyWrtNoExCode(clsType.User.IdNumber, FnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담여부 액팅저장중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //구강검사(ZD00) . 찍기
                result = hicResultService.UpdatePointbyWrtNoExCode(clsType.User.IdNumber, FnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담내역 저장시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                //구강상담기록지
                if (chkDenPrt.Checked == true)
                {
                    clsPublic.GstrRetValue = string.Format("{0:######0}", FnWRTNO) + ";2";   //통보방법:2.주소지                    
                    frmHcPrint_DentalPrint f = new frmHcPrint_DentalPrint(clsPublic.GstrRetValue);
                    f.ShowDialog();
                }

                //상담대기순번 완료 및 다음검사실 지정
                fn_WAIT_NextRoom_SET();

                fn_Screen_Clear();

                if (rdoJob1.Checked == true)
                {
                    eBtnClick(btnSearch, new EventArgs());
                }


                //종합검진 구강결과UPDATE 로직 추가(2021-04-26)
                //if (FnHeaWRTNO > 0)
                //{
                //    int nSCnt = 0;
                //    string strRoom = "";
                //    string strGJJONG = "";

                //    HEA_JEPSU item1 = heaJepsuService.GetItemByGWrtno(FnHeaWRTNO);
                //    strGJJONG = item1.GJJONG;

                //    //검사실 지정없으면 상담저장안됨
                //    for (int i = 0; i < SS_SEL.ActiveSheet.NonEmptyRowCount; i++)
                //    {
                //        if (SS_SEL.ActiveSheet.Cells[i, 2].Text == "True")
                //        {
                //            nSCnt += 1;
                //            strRoom = SS_SEL.ActiveSheet.Cells[i, 3].Text;
                //        }
                //    }

                //    if (nSCnt == 0)
                //    {
                //        MessageBox.Show("다음 검사실을 지정해주세요!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        return;
                //    }
                //    else if (nSCnt > 1)
                //    {
                //        MessageBox.Show("검사실을 여러개 지정할수 없습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        return;
                //    }

                    
                //    if (heaSangdamWaitService.GetRowIdbyEndoRoom(FnHeaWRTNO, clsHcVariable.DENT_ROOM) > 0)
                //    {
                //        string[] strJongSQL;

                //        strJongSQL = new string[] { clsHcVariable.DENT_ROOM };

                //        result = heaSangdamWaitService.Update_Sangdam_GbCall(FnHeaWRTNO, strJongSQL);

                //        if (result < 0)
                //        {
                //            MessageBox.Show("구강상담결과 Data저장시 오류 발생(HEA_SANGDAM_WAIT)", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        HEA_SANGDAM_WAIT item3 = new HEA_SANGDAM_WAIT();

                //        item3.WRTNO = FnHeaWRTNO;
                //        item3.SNAME = item1.SNAME;
                //        item3.SEX = item1.SEX;
                //        item3.AGE = item1.AGE;
                //        item3.GJJONG = item1.GJJONG;
                //        item3.GBCALL = "Y";
                //        item3.GUBUN = clsHcVariable.DENT_ROOM;
                //        item3.WAITNO = 0;

                //        result = heaSangdamWaitService.Insert_Sangdam_Wait(item3);

                //        if (result < 0)
                //        {
                //            MessageBox.Show("상담대기 순번등록 중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //            return;
                //        }
                //    }

                //    //다음검사실 배정 설정- 접수제외
                //    if (nSCnt > 0 && strRoom != "")
                //    {
                //        HEA_SANGDAM_SET(FnHeaWRTNO, strRoom);
                //    }
                //    else
                //    {
                //        int result1 = heaDentalService.GbchkUpdate(FnHeaWRTNO, "Y");

                //        if (result1 < 0)
                //        {
                //            MessageBox.Show("HEA_DENTAL 완료UPDATE 오류!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //            return;
                //        }
                //    }






                //}


                txtWrtNo.Focus();
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                string strJong = "";
                string strTemp = "";
                int result = 0;

                string strFrDate = "";
                string strToDate = "";

                string strSName = "";
                long nLtdCode = 0;

                string strJob = "";
                string strChul = "";

                string strJepDate = "";

                Cursor.Current = Cursors.WaitCursor;

                strSName = txtSName.Text.Trim();
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                strJong = VB.Left(cboJong.Text, 2);

                if (rdoChul1.Checked == true)   //내원
                {
                    strChul = "1";
                }
                else if (rdoChul2.Checked == true)  //출장
                {
                    strChul = "2";
                }

                strJong = VB.Left(cboJong.Text, 2);

                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else if (rdoJob2.Checked == true)
                {
                    strJob = "2";
                }

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                sp.Spread_All_Clear(SSList);
                SSList.ActiveSheet.RowCount = 50;

                result = hicJepsuService.UpdateGbDentalbyResult(strFrDate, strToDate);

                if (result < 0)
                {
                    MessageBox.Show("구강검진 여부 Update 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (hicSangdamWaitService.GetCountbyGubunSName(clsHcVariable.GstrDrRoom, "{자리비움}") > 0)
                {
                    MessageBox.Show("자리비움입니다. 자리비움상태를 취소해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //상담대기순번
                if (chkWait.Checked == true && rdoJob1.Checked == true)
                {
                    List<HIC_JEPSU_DENTAL_SANGDAM_WAIT> list = hicJepsuDentalSangdamWaitService.GetItembyJepDate(strFrDate, strToDate, clsType.User.IdNumber, strSName, strJong, nLtdCode);

                    nREAD = list.Count;
                    SSList.ActiveSheet.RowCount = nREAD;
                    for (int i = 0; i < nREAD; i++)
                    {
                        SSList.ActiveSheet.Rows.Get(i).Height = 28;
                        SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                        SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                        SSList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO;
                        SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                        SSList.ActiveSheet.Cells[i, 2].Text = list[i].JEPDATE;
                        strJepDate = list[i].GJYEAR + "-" + VB.Right(list[i].JEPDATE, 5);
                        strJong = list[i].GJJONG;
                        //SSList.ActiveSheet.Cells[i, 3].Text = hb.READ_GjJong_Name(strJong);
                        SSList.ActiveSheet.Cells[i, 3].Text = list[i].GJJONG;
                        SSList.ActiveSheet.Cells[i, 5].Text = list[i].GJYEAR;
                        //일특 체크
                        if (strJong == "11" || strJong == "16")
                        {
                            if (!list[i].UCODES.IsNullOrEmpty())
                            {
                                if (strJong == "11")
                                {
                                    SSList.ActiveSheet.Cells[i, 4].Text = "Y1"; //일특 1차
                                }
                                if (strJong == "16")
                                {
                                    SSList.ActiveSheet.Cells[i, 4].Text = "Y2"; //일특 2차
                                }
                                //SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                                //SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(200, 255, 200);
                            }
                        }

                        if (!list[i].PANJENGDRNO.IsNullOrEmpty())
                        {
                            if (list[i].PANJENGDRNO == clsHcVariable.GnHicLicense.ToString())
                            {
                                SSList.ActiveSheet.Cells[i, 4].Text = "Y";
                                SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                                SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0FF"));
                            }
                        }

                        //2020-06-13
                        //검진 당일에 종검수검자인지 점검
                        if (heaJepsuService.GetCountbyPtNoSDate(list[i].PTNO, strJepDate) > 0)
                        {
                            SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                        }
                    }
                }
                else
                {
                    List<HIC_JEPSU_RES_DENTAL> list = hicJepsuResDentalService.GetItembyJepDate(strFrDate, strToDate, strJob, strChul, strSName, nLtdCode, strJong);

                    nREAD = list.Count;
                    SSList.ActiveSheet.RowCount = nREAD;
                    for (int i = 0; i < nREAD; i++)
                    {
                        SSList.ActiveSheet.Rows.Get(i).Height = 28;
                        SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                        SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                        SSList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.To<string>("");
                        SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                        SSList.ActiveSheet.Cells[i, 2].Text = list[i].JEPDATE;
                        strJepDate = list[i].GJYEAR +"-"+ VB.Right(list[i].JEPDATE, 5);
                        strJong = list[i].GJJONG;
                        //SSList.ActiveSheet.Cells[i, 3].Text = hb.READ_GjJong_Name(strJong);
                        SSList.ActiveSheet.Cells[i, 3].Text = list[i].GJJONG;
                        SSList.ActiveSheet.Cells[i, 5].Text = list[i].GJYEAR;
                        //일특 체크
                        if (strJong == "11" || strJong == "16")
                        {
                            if (!list[i].UCODES.IsNullOrEmpty())
                            {
                                if (strJong == "11")
                                {
                                    SSList.ActiveSheet.Cells[i, 4].Text = "Y1"; //일특 1차
                                }
                                if (strJong == "16")
                                {
                                    SSList.ActiveSheet.Cells[i, 4].Text = "Y2"; //일특 2차
                                }
                                //SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                                //SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(200, 255, 200);
                            }
                        }

                        if (!list[i].PANJENGDRNO.IsNullOrEmpty() && list[i].PANJENGDRNO != 0)
                        {
                            if (list[i].PANJENGDRNO == clsHcVariable.GnHicLicense)
                            {
                                SSList.ActiveSheet.Cells[i, 4].Text = "Y";
                                SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                                SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0FF"));
                            }
                        }

                        //2020-06-13
                        //검진 당일에 종검수검자인지 점검
                        if (heaJepsuService.GetCountbyPtNoSDate(list[i].PTNO, strJepDate) > 0)
                        {
                            SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                        }
                    }
                }

                //상담인원 및 대기인원 DISPLAY
                HIC_RES_DENTAL_JEPSU list2 = hicResDentalJepsuService.GetCountbySysDate(clsHcVariable.GnHicLicense);

                lblCounter.Text = "총 대기인원: ";
                lblCounter.Text += list2.CNT2 + " 명 / ";
                lblCounter.Text += "상담인원: ";
                lblCounter.Text += list2.CNT + " 명";

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnChgPanDate)
            {
                string strJepDate = "";
                int result = 0;

                strJepDate = ssPatInfo.ActiveSheet.Cells[0, 4].Text.Trim();

                if (string.Compare(txtPanDate.Text, strJepDate) < 0)
                {
                    MessageBox.Show("판정일이 접수일보다 적음", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.Compare(txtPanDate.Text, strJepDate) > 0)
                {
                    MessageBox.Show("판정일이 오늘보다 큼", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //판정항목
                result = hicResDentalService.UPdatePanjengDatebyWrtNo(txtPanDate.Text, FnWRTNO);

                fn_Screen_Clear();
                txtWrtNo.Focus();
            }
            else if (sender == btnMenuCancel)
            {
                fn_Screen_Clear();
            }
            else if (sender == btnMenuSchool)
            {
                if (hf.OpenForm_Check("frmHcSangStudentTeeth") == true)
                {
                    this.Close();
                    return;
                }

                FrmHcSangStudentTeeth = new frmHcSangStudentTeeth();
                FrmHcSangStudentTeeth.WindowState = FormWindowState.Maximized;
                FrmHcSangStudentTeeth.StartPosition = FormStartPosition.CenterParent;
                FrmHcSangStudentTeeth.ShowDialog(this);

                this.Close();
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSHistory)
            {
                if (string.Compare(SSHistory.ActiveSheet.Cells[e.Row, 3].Text, "2") <= 0)
                {
                    clsPublic.GstrRetValue = SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                    FrmHcPanSpcGenSecondView = new frmHcPanSpcGenSecondView(SSHistory.ActiveSheet.Cells[e.Row, 2].Text.To<long>());
                    FrmHcPanSpcGenSecondView.StartPosition = FormStartPosition.CenterParent;
                    FrmHcPanSpcGenSecondView.ShowDialog(this);
                }
                else if (SSHistory.ActiveSheet.Cells[e.Row, 3].Text.Trim() == "3")
                {
                    clsPublic.GstrRetValue = SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                }
            }
            if (sender == SSList)
            {
                if (e.RowHeader == true)
                {
                    return;
                }

                fn_Screen_Clear();

                txtWrtNo.Text = SSList.ActiveSheet.Cells[e.Row, 0].Text.Trim();

                if (SSList.ActiveSheet.Cells[e.Row, 4].Text.Trim() == "Y1")
                {
                    FstrGjJong = "91";  //일특 1차인경우
                }
                else if (SSList.ActiveSheet.Cells[e.Row, 4].Text.Trim() == "Y2")
                {
                    FstrGjJong = "92";  //일특 2차인경우
                }
                else
                {
                    FstrGjJong = "";
                }

                FstrGjYear = SSList.ActiveSheet.Cells[e.Row, 5].Text.Trim();
                fn_Screen_Display();
            }
        }

        void fn_Screen_Display()
        {
            int nREAD = 0;
            int nRow = 0;
            long nDrSabun = 0;
            string strRemark = "";
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strResName = "";
            string strSex = "";
            string strNomal = "";
            string strExPan = "";
            string strTemp = "";
            string strFlag = "";
            string strNextRoom = "";
            string strResMunjin = "";
            string strResJochi = "";

            int result = 0;

            btnPACS.Enabled = true;
            btnMed.Enabled = true;
            tabControl1.Enabled = true;

            FnWRTNO = txtWrtNo.Text.Trim().To<long>();
            if (FnWRTNO == 0)
            {
                return;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            //삭제된것 체크
            if (hb.READ_JepsuSTS(FnWRTNO) == "D")
            {
                MessageBox.Show(FnWRTNO + " 접수번호는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //상담내역이 있는지 점검
            FstrROWID = hicResDentalService.GetRowIdbyWrtNo(FnWRTNO);

            //구강대상자만 표시
            HIC_JEPSU listDental = hicJepsuService.GetGbDentalbyWrtNo(FnWRTNO);

            if (!listDental.IsNullOrEmpty())
            {
                if (listDental.GBDENTAL != "Y")
                {
                    MessageBox.Show("구강접수항목이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            //상담대기 화면에서 환자 Call 등록
            fn_Update_Patient_GbCall();

            //GoSub Screen_Injek_display       '인적사항을 Display
            FstrSpcDent = "";

            HIC_JEPSU_PATIENT list = hicJepsuPatientService.GetItembyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 " + FnWRTNO + "번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //검진자 기본정보 표시---------------
            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PTNO;
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE.To<string>() + "/" + list.SEX;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.To<string>());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list.JEPDATE;
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);

            FstrChasu = list.GJCHASU;
            FstrPtno = list.PTNO;
            FstrSname = list.SNAME;
            FnPano = list.PANO;
            FnAge = (long)hb.READ_HIC_AGE_GESAN(clsAES.DeAES(list.JUMIN2));  //치과 만 나이계산
            FstrSex = list.SEX;
            FstrJepDate = list.JEPDATE;
            FstrJumin = clsAES.DeAES(list.JUMIN2);
            FstrYear = list.GJYEAR;
            FstrUCodes = list.UCODES;
            FstrGjJong = list.GJJONG;
            FnHeaWRTNO = 0;
            FnHeaWRTNO = heaJepsuService.GetListWrtnoByPtnoSDate(FstrPtno, FstrJepDate);
            if (FnHeaWRTNO > 0 )
            {
                SS_SEL.Visible = true;
            }
            else
            {
                SS_SEL.Visible = false;
            }


            if (!FstrUCodes.IsNullOrEmpty())
            {
                for (int i = 1; i < FstrUCodes.Length; i++)
                {
                    if (VB.Pstr(FstrUCodes, ",", 1).IsNullOrEmpty() || VB.Pstr(FstrUCodes, ",", i).IsNullOrEmpty())
                    {
                        break;
                    }
                    else
                    {
                        if (hb.READ_SPC_GBDENTAL(VB.Pstr(FstrUCodes, ",", i)) == "Y")
                        {
                            FstrSpcDent = "Y";
                        }
                    }
                }
            }

            //2015년도 구강검진 판정 변경
            if (string.Compare(FstrGjYear, "2014") <= 0 && string.Compare(FstrGjYear, " ") > 0)
            {
                label14.Text = "결과해석";
                label15.Text = "추가조치";
            }
            else
            {
                label14.Text = "바로 조치";
                label15.Text = "적극적관리";
            }

            //상담테이블 없을 시 상담테이블 생성함
            if (FstrROWID.IsNullOrEmpty())
            {
                if (!fn_HIC_NEW_SANGDAM_INSERT(FnWRTNO, FstrGjJong).IsNullOrEmpty())
                {
                    MessageBox.Show("접수번호 " + FnWRTNO + " 신규상담항목 자동발생시 오류 발생. 전산실 연락바람", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //산이 있을경우 특수문진 테이블 점검
            if (FstrSpcDent == "Y")
            {
                if (hicResSpecialService.GetRowIdbyWrtNo(FnWRTNO).IsNullOrEmpty())
                {
                    result = hicResSpecialService.InsertWrtNo(FnWRTNO);

                    if (result < 0)
                    {
                        MessageBox.Show("접수번호 " + FnWRTNO + " 특수문진 Data 생성시 오류 발생!!! 전산실 연락바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            if (FstrSpcDent != "Y")
            {
                tab2.Visible = false;
            }

            if (hicSunapdtlService.GetCountbyWrtNoCode(FnWRTNO, "1158") == 0)
            {
                tab3.Visible = false;
            }

            //상담내역 Display
            fn_Screen_Sangdam_Display(FnWRTNO);

            if (txtPanDrNo.Text.To<long>() != 0)
            {
                if (txtPanDrNo.Text.To<long>() == clsHcVariable.GnHicLicense)
                {
                    btnSave.Enabled = true;
                    btnChgPanDate.Visible = false;
                }
                else
                {
                    btnSave.Enabled = false;
                    btnChgPanDate.Visible = true;
                    btnChgPanDate.Enabled = true;
                }
                lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
            }
            else
            {
                txtPanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
                lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                btnSave.Enabled = true;
                btnChgPanDate.Visible = false;
            }

            if (clsType.User.IdNumber == "28048")
            {
                btnSave.Enabled = true;
                txtPanDrNo.Enabled = true;
            }

            fn_Genjin_Histroy_SET();    //검진 HISTORY

            if (FstrROWID.IsNullOrEmpty())
            {
                fn_Auto_GaPanjeng();
            }

            //문진뷰어
            if (chkMunjin.Checked == false)
            {
                //검진문진뷰어
                DirectoryInfo dir = new DirectoryInfo(@"C:\Program Files\SamOmr\");
                if (dir.Exists == true)
                {
                    //Process.Start(clsHcVariable.Hic_Mun_EXE_PATH, FstrPtno);
                    VB.Shell(clsHcVariable.Hic_Mun_EXE_PATH + " " + FstrPtno, "NormalFocus");
                }
                else
                {
                    DirectoryInfo dir1 = new DirectoryInfo(@"C:\Program Files (x86)\SamOmr\");
                    if (dir1.Exists == true)
                    {
                        //Process.Start(clsHcVariable.Hic_Mun_EXE_PATH_64, FstrPtno);
                        VB.Shell(clsHcVariable.Hic_Mun_EXE_PATH_64 + " " + FstrPtno, "NormalFocus");
                    }
                }

                //인터넷문진표(New)
                Form frm = hf.OpenForm_Check_Return("frmHcSangInternetMunjinView");

                if (!frm.IsNullOrEmpty())
                {
                    frm.Dispose();
                }

                //if (hf.OpenForm_Check("frmHcSangInternetMunjinView") == true)
                //{
                //    //FrmHcSangInternetMunjinView.Close();
                //    FrmHcSangInternetMunjinView.Dispose();
                //    FrmHcSangInternetMunjinView = null;
                //}

                FrmHcSangInternetMunjinView = new frmHcSangInternetMunjinView(FnWRTNO, FstrJepDate, FstrPtno, FstrGjJong, FstrROWID);
                FrmHcSangInternetMunjinView.Show();
                FrmHcSangInternetMunjinView.WindowState = FormWindowState.Minimized;
            }

            //다음 상담.검사실 표시
            this.Text = VB.Pstr(this.Text, "▶다음 검사실", 1);
            lblWait.Text = "";

            List<HIC_SANGDAM_WAIT> list11 = hicSangdamWaitService.GetNextRoombyWrtNo(FnWRTNO);

            if (list11.Count >= 1)
            {
                strNextRoom = list11[0].NEXTROOM;
                if (strNextRoom != "")
                {
                    if (strNextRoom == "30")
                    {
                        this.Text += VB.Space(15) + "▶1번:시력.소변실로 수검자를 보내 주십시오.";
                        lblWait.Text = " ▶수검자를 1번:소변.시력실로 보내 주십시오.";
                    }
                    else if (strNextRoom == "31")
                    {
                        this.Text += VB.Space(15) + "▶3번 혈압으로 수검자를 보내 주십시오.";
                        lblWait.Text = " ▶수검자를 3번:혈압으로 보내 주십시오.";
                    }
                    else if (strNextRoom == "32")
                    {
                        this.Text += VB.Space(15) + "▶4번:채혈실로 수검자를 보내 주십시오.";
                        lblWait.Text = " ▶수검자를 4번:채혈실로 보내 주십시오.";
                    }
                    else if (strNextRoom == "33")
                    {
                        this.Text += VB.Space(15) + "▶검사가 완료되었습니다. 접수창구에 제출하십시오.";
                        lblWait.Text = " ▶검사완료 접수창구에 제출하십시오.";
                    }
                    else
                    {
                        HIC_WAIT_ROOM list2 = hicWaitRoomService.GetRoomRoomNamebyRoom(VB.Pstr(strNextRoom, ",", 1));

                        if (!list2.IsNullOrEmpty())
                        {
                            this.Text += VB.Space(15) + "▶다음 검사실 : " + list2.ROOM + "번방(";
                            this.Text += list2.ROOMNAME + ") 입니다.";
                            lblWait.Text = " ▶다음검사실: " + list2.ROOM + "." + list2.ROOMNAME;
                        }
                    }
                }
            }
        }

        void fn_Genjin_Histroy_SET()
        {
            int nRead = 0;
            string strData = "";
            string strJong = "";
            long nHeaPano = 0;

            //종검의 등록번호를 찾음
            nHeaPano = 0;

            nHeaPano = hicPatientService.GetPanobyJumin(clsAES.AES(FstrJumin));

            //일반건진, 종합검진의 접수내역을 Display
            List<HIC_JEPSU> list = hicJepsuService.GetItembyOnlyPaNo(FnPano, nHeaPano);

            nRead = list.Count;
            SSHistory.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                strJong = list[i].GJJONG;

                SSHistory.ActiveSheet.Cells[i, 0].Text = list[i].JEPDATE;
                if (strJong == "XX")
                {
                    SSHistory.ActiveSheet.Cells[i, 1].Text = "종검";
                }
                else
                {
                    SSHistory.ActiveSheet.Cells[i, 1].Text = hb.READ_GjJong_Name(strJong);
                }
                SSHistory.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.To<string>();                
                SSHistory.ActiveSheet.Cells[i, 3].Text = list[i].GJCHASU;
                SSHistory.ActiveSheet.Cells[i, 4].Text = list[i].GJJONG;
            }
        }

        void fn_Screen_Sangdam_Display(long argWrtNo)
        {
            int nIndex = 0;
            string strRES1 = "";
            string strRES2 = "";
            string strRES3 = "";
            string strTemp = "";
            int nTempIndex = 0;

            HIC_RES_DENTAL list = hicResDentalService.GetItemByWrtno(FnWRTNO);

            if (FstrSpcDent == "Y")
            {
                HIC_RES_SPECIAL list2 = hicResSpecialService.GetDentSogenbyWrtNo(FnWRTNO);
                if (!list2.DENTSOGEN.IsNullOrEmpty())
                {
                    nTempIndex = Convert.ToInt32(VB.Left(list2.DENTSOGEN, 1));
                }
               
            }

            strRES1 = list.RES_MUNJIN;
            strRES2 = list.RES_RESULT;
            strRES3 = list.RES_JOCHI;
            txtPanDate.Text = list.PANJENGDATE;
            if (txtPanDate.Text.IsNullOrEmpty())
            {
                txtPanDate.Text = clsPublic.GstrSysDate;
            }

            FbMunjin = false;
            if (!strRES1.IsNullOrEmpty())
            {
                FbMunjin = true;
            }

            //-------------------------------------------
            //문진표평가 자동표시
            //-------------------------------------------
            if (strRES1.IsNullOrEmpty())
            {
                //(치과)병력문제
                if (list.OPDDNT == "2")
                {
                    strTemp = "2";
                }
                else if (list.T_JILBYUNG1 == "1")
                {
                    strTemp = "2";
                }
                else if (list.T_JILBYUNG2 == "1")
                {
                    strTemp = "2";
                }
                else if (list.T_FUNCTION1 == "1")
                {
                    strTemp = "2";
                }
                else if (list.T_STAT1 == "1")
                {
                    strTemp = "2";
                }
                else if (list.T_STAT2 == "1")
                {
                    strTemp = "2";
                }
                else
                {
                    strTemp = "1";
                }

                //구강건강인식도 문제
                if (string.Compare(list.DNTSTATUS, "3") >= 0)
                {
                    strTemp += "2";
                }
                else if (string.Compare(list.T_HABIT5, "2") >= 0)
                {
                    strTemp += "2";
                }
                else
                {
                    strTemp += "1";
                }

                //구강위생
                if (string.Compare(list.T_HABIT6, "2") >= 0)
                {
                    strTemp += "2";
                }
                else if (string.Compare(list.T_HABIT4, "2") >= 0)
                {
                    strTemp += "1";
                }

                //불소이용
                if (string.Compare(list.T_HABIT7, "2") >= 0)
                {
                    strTemp += "2";
                }
                else
                {
                    strTemp += "1";
                }

                //설탕섭취
                if (string.Compare(list.T_HABIT8, "3") >= 0)
                {
                    strTemp += "2";
                }
                else if (string.Compare(list.T_HABIT9, "3") >= 0)
                {
                    strTemp += "2";
                }
                else
                {
                    strTemp += "1";
                }

                //흡연
                if (string.Compare(list.T_HABIT1, "2") >= 0)
                {
                    strTemp += "2";
                }
                else
                {
                    strTemp += "1";
                }
                strRES1 = strTemp;
            }

            //1.(치과)병력문제
            if (VB.Mid(strRES1, 1, 1) == "1")
            {
                rdoMunjinByung1.Checked = true;
                rdoMunjinByung2.Checked = false;
            }
            else if (VB.Mid(strRES1, 1, 1) == "2")
            {
                rdoMunjinByung1.Checked = false;
                rdoMunjinByung2.Checked = true;
            }
            else
            {
                rdoMunjinByung1.Checked = true;
                rdoMunjinByung2.Checked = false;
            }

            //2.구강건강인식도문제
            if (VB.Mid(strRES1, 2, 1) == "1")
            {
                rdoMunjinTeeth1.Checked = true;
                rdoMunjinTeeth2.Checked = false;
            }
            else if (VB.Mid(strRES1, 2, 1) == "2")
            {
                rdoMunjinTeeth1.Checked = false;
                rdoMunjinTeeth2.Checked = true;
            }
            else
            {
                rdoMunjinTeeth1.Checked = true;
                rdoMunjinTeeth2.Checked = false;
            }

            //3.구강위생
            if (VB.Mid(strRES1, 3, 1) == "1")
            {
                rdoTeethHygiene1.Checked = true;
                rdoTeethHygiene2.Checked = false;
            }
            else if (VB.Mid(strRES1, 3, 1) == "2")
            {
                rdoTeethHygiene1.Checked = false;
                rdoTeethHygiene2.Checked = true;
            }
            else
            {
                rdoTeethHygiene1.Checked = true;
                rdoTeethHygiene2.Checked = false;
            }

            //4.불소이용
            if (VB.Mid(strRES1, 4, 1) == "1")
            {
                rdofluoride1.Checked = true;
                rdofluoride2.Checked = false;
            }
            else if (VB.Mid(strRES1, 4, 1) == "2")
            {
                rdofluoride1.Checked = false;
                rdofluoride2.Checked = true;
            }
            else
            {
                rdofluoride1.Checked = true;
                rdofluoride2.Checked = false;
            }

            //5.설탕섭취
            if (VB.Mid(strRES1, 5, 1) == "1")
            {
                rdoSugar1.Checked = true;
                rdoSugar2.Checked = false;
            }
            else if (VB.Mid(strRES1, 5, 1) == "2")
            {
                rdoSugar1.Checked = false;
                rdoSugar2.Checked = true;
            }
            else
            {
                rdoSugar1.Checked = true;
                rdoSugar2.Checked = false;
            }

            //6.흡연
            if (VB.Mid(strRES1, 6, 1) == "1")
            {
                rdoSmoke1.Checked = true;
                rdoSmoke2.Checked = false;
            }
            else if (VB.Mid(strRES1, 6, 1) == "2")
            {
                rdoSmoke1.Checked = false;
                rdoSmoke2.Checked = true;
            }
            else
            {
                rdoSmoke1.Checked = true;
                rdoSmoke2.Checked = false;
            }

            //1.우식치아
            if (VB.Mid(strRES2, 1, 1) == "1")
            {
                rdoUsikia1.Checked = true;
                rdoUsikia2.Checked = false;
            }
            else if (VB.Mid(strRES2, 1, 1) == "2")
            {
                rdoUsikia1.Checked = false;
                rdoUsikia2.Checked = true;
            }
            else
            {
                rdoUsikia1.Checked = true;
                rdoUsikia2.Checked = false;
            }

            //2.인접면 우식 의심치아
            if (VB.Mid(strRES2, 2, 1) == "1")
            {
                rdoAdjacentSurfaceUsikia1.Checked = true;
                rdoAdjacentSurfaceUsikia2.Checked = false;
            }
            else if (VB.Mid(strRES2, 2, 1) == "2")
            {
                rdoAdjacentSurfaceUsikia1.Checked = false;
                rdoAdjacentSurfaceUsikia2.Checked = true;
            }
            else
            {
                rdoAdjacentSurfaceUsikia1.Checked = true;
                rdoAdjacentSurfaceUsikia2.Checked = false;
            }

            //3.수복치아
            if (VB.Mid(strRES2, 3, 1) == "1")
            {
                rdoSubokchia1.Checked = true;
                rdoSubokchia2.Checked = false;
            }
            else if (VB.Mid(strRES2, 3, 1) == "2")
            {
                rdoSubokchia1.Checked = false;
                rdoSubokchia2.Checked = true;
            }
            else
            {
                rdoSubokchia1.Checked = false;
                rdoSubokchia2.Checked = true;
            }

            //4.상실치아
            if (VB.Mid(strRES2, 4, 1) == "1")
            {
                rdoLosingteeth1.Checked = true;
                rdoLosingteeth2.Checked = false;
            }
            else if (VB.Mid(strRES2, 4, 1) == "2")
            {
                rdoLosingteeth1.Checked = false;
                rdoLosingteeth2.Checked = true;
            }
            else
            {
                rdoLosingteeth1.Checked = true;
                rdoLosingteeth2.Checked = false;
            }

            //5.치은염증
            if (VB.Mid(strRES2, 5, 1) == "1")
            {
                rdoGingivitis1.Checked = true;
            }
            else if (VB.Mid(strRES2, 5, 1) == "2")
            {
                rdoGingivitis2.Checked = true;
            }
            else if (VB.Mid(strRES2, 5, 1) == "3")
            {
                rdoGingivitis3.Checked = true;
            }
            else
            {
                rdoGingivitis1.Checked = true;
                rdoGingivitis2.Checked = false;
                rdoGingivitis3.Checked = false;
            }

            //6.치석
            if (VB.Mid(strRES2, 6, 1) == "1")
            {
                rdoFloss1.Checked = true;
            }
            else if (VB.Mid(strRES2, 6, 1) == "2")
            {
                rdoFloss2.Checked = true;
            }
            else if (VB.Mid(strRES2, 6, 1) == "3")
            {
                rdoFloss3.Checked = true;
            }
            else
            {
                rdoFloss1.Checked = true;
                rdoFloss2.Checked = false;
                rdoFloss3.Checked = false;
            }

            //2019변경사항
            switch (list.T40_PAN1_NEW)
            {
                case 0:
                    rdo40_Pan1_New0.Checked = true;
                    break;
                case 1:
                    rdo40_Pan1_New1.Checked = true;
                    break;
                case 2:
                    rdo40_Pan1_New2.Checked = true;
                    break;
                case 3:
                    rdo40_Pan1_New3.Checked = true;
                    break;
                case 4:
                    rdo40_Pan1_New4.Checked = true;
                    break;
                case 5:
                    rdo40_Pan1_New5.Checked = true;
                    break;
                default:
                    break;
            }

            switch (list.T40_PAN2_NEW)
            {
                case 0:
                    rdo40_Pan2_New0.Checked = true;
                    break;
                case 1:
                    rdo40_Pan2_New1.Checked = true;
                    break;
                case 2:
                    rdo40_Pan2_New2.Checked = true;
                    break;
                case 3:
                    rdo40_Pan2_New3.Checked = true;
                    break;
                case 4:
                    rdo40_Pan2_New4.Checked = true;
                    break;
                case 5:
                    rdo40_Pan2_New5.Checked = true;
                    break;
                default:
                    break;
            }

            switch (list.T40_PAN3_NEW)
            {
                case 0:
                    rdo40_Pan3_New0.Checked = true;
                    break;
                case 1:
                    rdo40_Pan3_New1.Checked = true;
                    break;
                case 2:
                    rdo40_Pan3_New2.Checked = true;
                    break;
                case 3:
                    rdo40_Pan3_New3.Checked = true;
                    break;
                case 4:
                    rdo40_Pan3_New4.Checked = true;
                    break;
                case 5:
                    rdo40_Pan3_New5.Checked = true;
                    break;
                default:
                    break;
            }

            switch (list.T40_PAN4_NEW)
            {
                case 0:
                    rdo40_Pan4_New0.Checked = true;
                    break;
                case 1:
                    rdo40_Pan4_New1.Checked = true;
                    break;
                case 2:
                    rdo40_Pan4_New2.Checked = true;
                    break;
                case 3:
                    rdo40_Pan4_New3.Checked = true;
                    break;
                case 4:
                    rdo40_Pan4_New4.Checked = true;
                    break;
                case 5:
                    rdo40_Pan4_New5.Checked = true;
                    break;
                default:
                    break;
            }

            switch (list.T40_PAN5_NEW)
            {
                case 0:
                    rdo40_Pan5_New0.Checked = true;
                    break;
                case 1:
                    rdo40_Pan5_New1.Checked = true;
                    break;
                case 2:
                    rdo40_Pan5_New2.Checked = true;
                    break;
                case 3:
                    rdo40_Pan5_New3.Checked = true;
                    break;
                case 4:
                    rdo40_Pan5_New4.Checked = true;
                    break;
                case 5:
                    rdo40_Pan5_New5.Checked = true;
                    break;
                default:
                    break;
            }

            switch (list.T40_PAN6_NEW)
            {
                case 0:
                    rdo40_Pan6_New0.Checked = true;
                    break;
                case 1:
                    rdo40_Pan6_New1.Checked = true;
                    break;
                case 2:
                    rdo40_Pan6_New2.Checked = true;
                    break;
                case 3:
                    rdo40_Pan6_New3.Checked = true;
                    break;
                case 4:
                    rdo40_Pan6_New4.Checked = true;
                    break;
                case 5:
                    rdo40_Pan6_New5.Checked = true;
                    break;
                default:
                    break;
            }

            //종합판정
            if (list.PANJENGDRNO > 0)
            {
                switch (list.T_PANJENG1)
                {
                    case "1":
                        rdoTotalPan1.Checked = true;
                        break;
                    case "2":
                        rdoTotalPan2.Checked = true;
                        break;
                    case "3":
                        rdoTotalPan3.Checked = true;
                        break;
                    case "4":
                        rdoTotalPan4.Checked = true;
                        break;
                    default:
                        rdoTotalPan1.Checked = false;
                        rdoTotalPan2.Checked = false;
                        rdoTotalPan3.Checked = false;
                        rdoTotalPan4.Checked = false;
                        break;
                }

                //조치사항
                chkAction11.Checked = false;
                chkAction12.Checked = false;
                chkAction13.Checked = false;
                chkAction21.Checked = false;
                chkAction22.Checked = false;
                chkAction23.Checked = false;
                chkAction24.Checked = false;

                if (VB.Mid(strRES3, 1, 1) == "1") chkAction11.Checked = true;
                if (VB.Mid(strRES3, 2, 1) == "1") chkAction12.Checked = true;
                if (VB.Mid(strRES3, 3, 1) == "1") chkAction13.Checked = true;
                if (VB.Mid(strRES3, 4, 1) == "1") chkAction21.Checked = true;
                if (VB.Mid(strRES3, 5, 1) == "1") chkAction22.Checked = true;
                if (VB.Mid(strRES3, 6, 1) == "1") chkAction23.Checked = true;
                if (VB.Mid(strRES3, 7, 1) == "1") chkAction24.Checked = true;

                //추가조치사항
                txtPanjengJochi.Text = list.T_PANJENG_SOGEN;
                txtEtcPartExamOpinion.Text = list.T_PANJENG_ETC;
                txtResultInterpretation.Text = list.SANGDAM;
            }

            //치과상담(특수)
            if (FstrSpcDent == "Y")
            {
                fn_SPECDENT_SET(FnWRTNO);
                if (!list.CHIJURESULT.IsNullOrEmpty())
                {
                    nIndex = int.Parse(VB.Left(list.CHIJURESULT, 1));
                }
                

                if (nIndex > 0)
                {
                    rdoJbk1.Checked = true;
                    txtChijuRes.Text = "";
                    cboChijuRes.SelectedIndex = nIndex;
                }
                else
                {
                    rdoJbk2.Checked = true;
                    cboChijuRes.SelectedIndex = 0;
                    txtChijuRes.Text = list.CHIJURESULT;
                }

                txtChiju1.Text = list.CHIJUSTAT1;
                txtChiju2.Text = list.CHIJUSTAT2;
                txtChiju3.Text = list.CHIJUSTAT3;
                txtChiju4.Text = list.CHIJUSTAT4;

                nIndex = int.Parse(VB.Left(nTempIndex.ToString(), 1));
                if (nIndex > 0)
                {
                    rdoSogen1.Checked = true;
                    txtSpcDental.Text = "";
                    cboSogen.SelectedIndex = nIndex;
                }
                else
                {
                    rdoSogen2.Checked = true;
                    cboSogen.SelectedIndex = 0;
                    txtSpcDental.Text = nTempIndex.ToString();
                }
                
                //신규구분
                if (list.PANJENGDRNO.To<string>()=="0")
                {
                    rdoSogen1.Checked = true;
                    txtSpcDental.Text = "";
                    cboSogen.SelectedIndex = nIndex;
                }

            }

            //판정의사
            txtPanDrNo.Text = list.PANJENGDRNO.To<string>();

            if (FbMunjin == false)
            {
                fn_Auto_GaPanjeng();
            }
        }

        void eComboClick(object sender, EventArgs e)
        {
            int nCNT = 0;
            string strGbn = "";
            string strResult = "";
            string strJengName = "";

            for (int i = 1; i <= 28; i++)
            {
                CheckBox chkTooth = (Controls.Find("chkTooth" + i.ToString(), true)[0] as CheckBox);
                chkTooth.Checked = false;
            }

            if (sender == cboJengsang1)
            {
                strGbn = VB.Left(cboJengsang1.Text, 2);
            }
            if (sender == cboJengsang2)
            {
                strGbn = VB.Left(cboJengsang2.Text, 2);
            }

            switch (strGbn)
            {
                case "E1":
                    nCNT = 1;
                    break;
                case "E2":
                    nCNT = 2;
                    break;
                case "E3":
                    nCNT = 3;
                    break;
                case "E4":
                    nCNT = 4;
                    break;
                case "E5":
                    nCNT = 5;
                    break;
                case "T1":
                    nCNT = 6;
                    break;
                case "T2":
                    nCNT = 7;
                    break;
                case "T3":
                    nCNT = 8;
                    break;
                case "T4":
                    nCNT = 9;
                    break;
                default:
                    return;
            }

            for (int i = 1; i <= 28; i++)
            {
                CheckBox chkTooth = (Controls.Find("chkTooth" + i.ToString(), true)[0] as CheckBox);

                if (FstrDent[nCNT, i - 1] == "1")
                {
                    chkTooth.Checked = true;
                }
                else
                {
                    chkTooth.Checked = false;
                }
            }
        }

        void eRdoChkChanged(object sender, EventArgs e)
        {
            fn_Auto_GaPanjeng();
        }

        void eRdoClick(object sender, EventArgs e)
        {
            if (sender == rdoJbk1)
            {
                cboChijuRes.SelectedIndex = 0;
                cboChijuRes.Enabled = true;
                txtChijuRes.Text = "";
                txtChijuRes.Enabled = false;
            }
            else if (sender == rdoJbk2)
            {
                cboChijuRes.SelectedIndex = 0;
                cboChijuRes.Enabled = false;
                txtChijuRes.Text = "";
                txtChijuRes.Enabled = true;
            }
            else if (sender == rdoJengsang1)
            {
                for (int i = 1; i <= 28; i++)
                {
                    CheckBox chkTooth = (Controls.Find("chkTooth" + i.ToString(), true)[0] as CheckBox);

                    chkTooth.Checked = false;
                }

                cboJengsang2.Text = "";
                cboJengsang1.Enabled = true;
                cboJengsang2.Enabled = false;
            }
            else if (sender == rdoJengsang2)
            {
                for (int i = 1; i <= 28; i++)
                {
                    CheckBox chkTooth = (Controls.Find("chkTooth" + i.ToString(), true)[0] as CheckBox);

                    chkTooth.Checked = false;
                }

                cboJengsang1.Text = "";
                cboJengsang1.Enabled = false;
                cboJengsang2.Enabled = true;
            }
            else if (sender == rdoJob1 || sender == rdoJob2)
            {
                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == rdoSogen1)
            {
                cboSogen.SelectedIndex = 0;
                cboSogen.Enabled = true;
                txtSpcDental.Text = "";
                txtSpcDental.Enabled = false;
            }
            else if (sender == rdoSogen2)
            {
                txtSpcDental.Text = "";
                txtSpcDental.Enabled = true;
                cboSogen.SelectedIndex = 0;
                cboSogen.Enabled = false;
            }
            else if (sender == rdoMunjinByung1 || sender == rdoMunjinByung2)
            {
                fn_Auto_GaPanjeng();
            }
            else if (sender == rdoMunjinTeeth1 || sender == rdoMunjinTeeth2)
            {
                fn_Auto_GaPanjeng();
            }
            else if (sender == rdoTeethHygiene1 || sender == rdoTeethHygiene2)
            {
                fn_Auto_GaPanjeng();
            }
            else if (sender == rdofluoride1 || sender == rdofluoride2)
            {
                fn_Auto_GaPanjeng();
            }
            else if (sender == rdoSugar1 || sender == rdoSugar2)
            {
                fn_Auto_GaPanjeng();
            }
            else if (sender == rdoSmoke1 || sender == rdoSmoke2)
            {
                fn_Auto_GaPanjeng();
            }
            else if (sender == rdoUsikia1 || sender == rdoUsikia2)
            {
                fn_Auto_GaPanjeng();
            }
            else if (sender == rdoAdjacentSurfaceUsikia1 || sender == rdoAdjacentSurfaceUsikia2)
            {
                fn_Auto_GaPanjeng();
            }
            else if (sender == rdoSubokchia1 || sender == rdoSubokchia2)
            {
                fn_Auto_GaPanjeng();
            }
            else if (sender == rdoGingivitis1 || sender == rdoGingivitis2)
            {
                fn_Auto_GaPanjeng();
            }
            else if (sender == rdoFloss1 || sender == rdoFloss2)
            {
                fn_Auto_GaPanjeng();
            }
        }

        void fn_Auto_GaPanjeng()
        {
            string strPan = "";
            string strResult1 = "";
            string strResult2 = "";
            string strResult3 = "";

            //문진표평가로 필요 보건교육 설정
            chkAction11.Checked = false;
            chkAction12.Checked = false;
            chkAction13.Checked = false;

            if (rdoSugar2.Checked == true)
            {
                chkAction11.Checked = true;
            }

            if (rdofluoride2.Checked == true)
            {
                chkAction13.Checked = true;
            }

            if (rdoTeethHygiene2.Checked == true)
            {
                chkAction12.Checked = true;
            }

            strPan = "1";
            strResult1 = ""; strResult2 = ""; strResult3 = "";
            txtResultInterpretation.Text = "";

            chkAction21.Checked = false;
            chkAction22.Checked = false;
            chkAction23.Checked = false;
            chkAction24.Checked = false;

            //(1)우식치아
            if (rdoUsikia2.Checked == true)
            {
                chkAction23.Checked = true;
                strPan = "4";
                strResult1 += "치과에서 우식증 치료를 받으시길 바랍니다.";
            }
            else
            {
                chkAction23.Checked = false;
            }

            //(2)인접면 우식 의심치아
            if (rdoAdjacentSurfaceUsikia2.Checked == true)
            {
                chkAction21.Checked = true;
                if (string.Compare(strPan, "3") < 0)
                {
                    strPan = "3";
                }
                strResult2 += "육안 관찰 어려운 충치(치아사이)가 있을수 있으므로 치과 방사선 사진 촬영을 권고합니다.";
            }
            //(3)수복치아
            if (rdoSubokchia2.Checked == true)
            {
                chkAction22.Checked = true;
                if (string.Compare(strPan, "2") < 0)
                {
                    strPan = "2";
                }
                strResult2 += "전문가 구강위생관리 및 정기적 구강검진을 받으시길 바랍니다.";
            }

            //(4)상실치아
            if (rdoLosingteeth2.Checked == true)
            {
                chkAction23.Checked = true;
                if (string.Compare(strPan, "4") < 0)
                {
                    strPan = "4";
                }
                strResult1 += "치과에서 보철치료를 받으시길 바랍니다.";
            }

            //(5)치은염증(경증)
            if (rdoGingivitis2.Checked == true)
            {
                chkAction22.Checked = true;
                if (string.Compare(strPan, "3") < 0)
                {
                    strPan = "3";
                }
                strResult1 += "치아에 염증이 있으므로 치주 전문 치료를 받으시길 바랍니다.";
            }
            //치은염증(중증)
            else if (rdoGingivitis3.Checked == true)
            {
                chkAction24.Checked = true;
                if (string.Compare(strPan, "4") < 0)
                {
                    strPan = "4";
                }
                strResult1 += "잇몸에 염증이 심하므로 치주 전문 상담 및 치료를 받으시길 바랍니다.";
            }

            //(6)치석(경증)
            if (rdoFloss2.Checked == true)
            {
                chkAction22.Checked = true;
                if (string.Compare(strPan, "3") < 0)
                {
                    strPan = "3";
                }
                strResult1 += "치과에서 치석제거를 받으시기 바랍니다.";
            }
            //치석(중증)
            else if (rdoFloss3.Checked == true)
            {
                chkAction24.Checked = true;
                if (string.Compare(strPan, "4") < 0)
                {
                    strPan = "4";    
                }
                strResult1 += "치과에서 치석제거 및 치근활택술 치료를 받으시길 바랍니다.";
            }

            if (chkAction11.Checked == true) strResult3 += "설탕섭취(영양),";
            if (chkAction12.Checked == true) strResult3 += "구강위생,";
            if (chkAction13.Checked == true) strResult3 += "불소이용,";
            if (!strResult3.IsNullOrEmpty())
            {
                strResult3 = VB.Left(strResult3, strResult3.Length - 1);
                strResult3 += "필요 보건교육(" + strResult3 + ")";
            }
            strResult2 += strResult3;
            if (strResult1.IsNullOrEmpty()) strResult1 = "특이소견없음";
            if (strResult2.IsNullOrEmpty()) strResult2 = "특이소견없음";
            txtResultInterpretation.Text = strResult1;
            txtPanjengJochi.Text = strResult2;

            //종합판정
            if (string.Compare(strPan, "1") >= 0 && string.Compare(strPan, "4") <= 0)
            {
                if (strPan == "1")
                {
                    rdoTotalPan1.Checked = true;
                }
                else if (strPan == "2")
                {
                    rdoTotalPan2.Checked = true;
                }
                else if (strPan == "3")
                {
                    rdoTotalPan3.Checked = true;
                }
                else if (strPan == "4")
                {
                    rdoTotalPan4.Checked = true;
                }
                Application.DoEvents();
            }
        }

        void fn_Update_Patient_GbCall()
        {
            //string strWrtNo = "";
            List<string> strWrtNo = new List<string>();
            int result = 0;
            string strSysDate = "";

            //출장검진은 대기순번 관리 안함
            if (rdoChul2.Checked == true) return;

            //오늘 대기순번에 없으면 처리를 안함
            if (hicSangdamWaitService.GetCountbyWrtNo(long.Parse(txtWrtNo.Text)) == 0)
            {
                return;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            strSysDate = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + ":00";

            clsDB.setBeginTran(clsDB.DbCon);

            strWrtNo.Clear();



            //호출은 했으나 상담이 완료안된 접수번호 찾음
            List<HIC_SANGDAM_WAIT> list = hicSangdamWaitService.GetWrtNobyWrtNo(long.Parse(txtWrtNo.Text));

            clsDB.setBeginTran(clsDB.DbCon);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strWrtNo.Add(list[i].WRTNO.ToString());
                }

                if (strWrtNo != null)
                {
                    result = hicSangdamWaitService.UpdateCallTimeDisplaybyWrtNo(strWrtNo);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("저장 오류!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            result = hicSangdamWaitService.UpdateCallTimebyWrtNo(strSysDate, long.Parse(txtWrtNo.Text));

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("호출시간 update 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// 다음 검사실 설정
        /// </summary>
        /// <returns></returns>
        bool fn_WAIT_NextRoom_SET()
        {
            bool rtnVal = true;
            int nWait = 0;
            string strNextRoom = "";
            string strRoom = "";
            string strTemp = "";
            long nWRTNO = 0;
            string strGjJong = "";
            string strSname = "";
            string strSex = "";
            long nAge = 0;
            int result = 0;

            List<HIC_SANGDAM_NEW> listNextRoom = hicSangdamWaitService.GetOnlyNextRoomByWrtNo(FnWRTNO);

            if (listNextRoom.Count <= 0)
            {
                strNextRoom = "";
            }
            else
            {
                strNextRoom = listNextRoom[0].NEXTROOM;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            //다음 검사실이 없으면
            if (strNextRoom.IsNullOrEmpty())
            {   
                result = hicSangdamWaitService.UpdateGbCallCallTimeGubunbyWrtNo(clsHcVariable.GstrDrRoom, FnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("호출여부 갱신중 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rtnVal = false;
                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    rtnVal = true;
                }
                return rtnVal;
            }

            strRoom = VB.Pstr(strNextRoom, ",", 1);
            strTemp = VB.Pstr(strNextRoom, strRoom + ",", 2);
            strNextRoom = strTemp;

            //다음 가셔야할곳이 접수창구이면 등록 안함
            if (string.Compare(strRoom, "30") >= 0)
            {   
                result = hicSangdamWaitService.DeletebyPaNo(FnPano);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    rtnVal = false;
                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    rtnVal = true;
                }
                return rtnVal;
            }

            nWait = (int)hicSangdamWaitService.GetMaxWaitNobyGubun(strRoom, clsPublic.GstrSysDate);

            //기존 등록된 대기순번을 삭제함
            result = hicSangdamWaitService.DeletebyPaNo(FnPano);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("기존 등록된 대기순번 삭제중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rtnVal = false;
                return rtnVal;
            }

            List<HIC_JEPSU> list = hicJepsuService.GetItembyPaNoJepDate(FnPano, clsPublic.GstrSysDate);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    nWRTNO = list[i].WRTNO;
                    strGjJong = list[i].GJJONG;
                    strSname = list[i].SNAME;
                    strSex = list[i].SEX;
                    nAge = list[i].AGE;

                    HIC_SANGDAM_WAIT item = new HIC_SANGDAM_WAIT();

                    item.WRTNO = nWRTNO;
                    item.SNAME = strSname;
                    item.SEX = strSex;
                    item.AGE = nAge;
                    item.GJJONG = strGjJong;
                    item.GUBUN = strRoom;
                    //item.GUBUN = clsHcVariable.GstrDrRoom; 
                    item.WAITNO = nWait;
                    item.PANO = FnPano;
                    //item.NEXTROOM = strRoom;
                    item.NEXTROOM = strNextRoom;

                    //상담대기 등록함
                    result = hicSangdamWaitService.Insert(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("상담대기 순번등록 중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        rtnVal = false;
                        return rtnVal;
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
            rtnVal = true;

            return rtnVal;
        }

        void fn_Screen_Clear()
        {
            tab2.Visible = true;
            tab3.Visible = true;

            btnSave.Enabled = true;
            btnChgPanDate.Visible = false;

            btnPACS.Enabled = false;
            btnMed.Enabled = false;
            tabControl1.Enabled = false;
            tabControl1.SelectedTabIndex = 0;
            tabControl1.SelectedTab = tab1;

            txtLtdCode.Text = "";
            txtSName.Text = "";
            txtWrtNo.Text = "";
            lblWait.Text = "";

            sp.Spread_All_Clear(ssPatInfo);
            ssPatInfo.ActiveSheet.RowCount = 1;
            sp.Spread_All_Clear(SSHistory);

            rdoMunjinByung1.Checked = false;
            rdoMunjinByung2.Checked = false;
            rdoMunjinTeeth1.Checked = false;
            rdoMunjinTeeth2.Checked = false;
            rdoTeethHygiene1.Checked = false;
            rdoTeethHygiene2.Checked = false;
            rdofluoride1.Checked = false;
            rdofluoride2.Checked = false;
            rdoSugar1.Checked = false;
            rdoSugar2.Checked = false;
            rdoSmoke1.Checked = false;
            rdoSmoke2.Checked = false;
            rdoUsikia1.Checked = false;
            rdoUsikia2.Checked = false;
            rdoAdjacentSurfaceUsikia1.Checked = false;
            rdoAdjacentSurfaceUsikia2.Checked = false;
            rdoSubokchia1.Checked = false;
            rdoSubokchia2.Checked = false;
            rdoLosingteeth1.Checked = false;
            rdoLosingteeth2.Checked = false;
            rdoGingivitis1.Checked = false;
            rdoGingivitis2.Checked = false;
            rdoFloss1.Checked = false;
            rdoFloss2.Checked = false;
            rdoTotalPan1.Checked = false;
            rdoTotalPan2.Checked = false;
            rdoTotalPan3.Checked = false;
            rdoTotalPan4.Checked = false;

            chkAction11.Checked = false;
            chkAction12.Checked = false;
            chkAction13.Checked = false;
            chkAction21.Checked = false;
            chkAction22.Checked = false;
            chkAction23.Checked = false;
            chkAction24.Checked = false;

            txtPanjengJochi.Text = "";
            txtEtcPartExamOpinion.Text = "";
            txtResultInterpretation.Text = "";

            txtPanDrNo.Text = "";
            lblDrName.Text = "";

            //특수
            lblE0.Visible = false;
            lblE1.Visible = false;
            lblE2.Visible = false;
            lblE3.Visible = false;
            lblE4.Visible = false;
            lblE5.Visible = false;

            lblT0.Visible = false;
            lblT1.Visible = false;
            lblT2.Visible = false;
            lblT3.Visible = false;
            lblT4.Visible = false;

            cboJengsang1.SelectedIndex = -1;
            cboJengsang2.SelectedIndex = -1;

            for (int i = 1; i <= 28; i++)
            {
                CheckBox chkTooth = (Controls.Find("chkTooth" + i.ToString(), true)[0] as CheckBox);

                chkTooth.Checked = false;
            }

            cboChijuRes.SelectedIndex = -1;
            txtChijuRes.Text = "";
            txtChijuETC.Text = "";

            txtChiju1.Text = "";
            txtChiju2.Text = "";
            txtChiju3.Text = "";
            txtChiju4.Text = "";

            cboSogen.SelectedIndex = -1;
            txtSpcDental.Text = "";

            btnSave.Enabled = false;

            rdo40_Pan1_New0.Checked = true;
            rdo40_Pan2_New0.Checked = true;
            rdo40_Pan3_New0.Checked = true;
            rdo40_Pan4_New0.Checked = true;
            rdo40_Pan5_New0.Checked = true;
            rdo40_Pan6_New0.Checked = true;
        }

        void fn_SPECDENT_SET(long argWrtNo)
        {
            string strBusik0 = "";
            string strBusik1 = "";
            string strBusik2 = "";
            string strBusik3 = "";
            string strBusik4 = "";
            string strBusik5 = "";
            string strGyomo0 = "";
            string strGyomo1 = "";
            string strGyomo2 = "";
            string strGyomo3 = "";
            string strGyomo4 = "";
            string[] strFlag = new string[11];


            for (int i = 0; i <= 10; i++)
            {
                for (int j = 0; j <= 27; j++)
                {
                    FstrDent[i, j] = "";
                }
                strFlag[i] = "";
            }

            HIC_RES_DENTAL list = hicResDentalService.GetItemByWrtno(argWrtNo);

            if (!list.IsNullOrEmpty())
            {
                strBusik0 = list.BUSIK0;
                strBusik1 = list.BUSIK1;
                strBusik2 = list.BUSIK2;
                strBusik3 = list.BUSIK3;
                strBusik4 = list.BUSIK4;
                strBusik5 = list.BUSIK5;
                strGyomo0 = list.GYOMO0;
                strGyomo1 = list.GYOMO1;
                strGyomo2 = list.GYOMO2;
                strGyomo3 = list.GYOMO3;
                strGyomo4 = list.GYOMO4;

                if (strBusik0 == "Y") { lblE0.Visible = true; strFlag[10] = "OK"; }
                if (strGyomo0 == "Y") { lblT0.Visible = true; strFlag[11] = "OK"; }
                for (int i = 0; i <= 27; i++)
               {
                    if (VB.Mid(strBusik1, i + 1, 1) == "1") { FstrDent[1, i] = "1"; lblE1.Visible = true; strFlag[0] = "OK"; }
                    if (VB.Mid(strBusik2, i + 1, 1) == "1") { FstrDent[2, i] = "1"; lblE2.Visible = true; strFlag[1] = "OK"; }
                    if (VB.Mid(strBusik3, i + 1, 1) == "1") { FstrDent[3, i] = "1"; lblE3.Visible = true; strFlag[2] = "OK"; }
                    if (VB.Mid(strBusik4, i + 1, 1) == "1") { FstrDent[4, i] = "1"; lblE4.Visible = true; strFlag[3] = "OK"; }
                    if (VB.Mid(strBusik5, i + 1, 1) == "1") { FstrDent[5, i] = "1"; lblE5.Visible = true; strFlag[4] = "OK"; }
                    if (VB.Mid(strGyomo1, i + 1, 1) == "1") { FstrDent[6, i] = "1"; lblT1.Visible = true; strFlag[5] = "OK"; }
                    if (VB.Mid(strGyomo2, i + 1, 1) == "1") { FstrDent[7, i] = "1"; lblT2.Visible = true; strFlag[6] = "OK"; }
                    if (VB.Mid(strGyomo3, i + 1, 1) == "1") { FstrDent[8, i] = "1"; lblT3.Visible = true; strFlag[7] = "OK"; }
                    if (VB.Mid(strGyomo4, i + 1, 1) == "1") { FstrDent[9, i] = "1"; lblT4.Visible = true; strFlag[8] = "OK"; }
               }

                if (strFlag[0].IsNullOrEmpty()) lblE1.Visible = false;
                if (strFlag[1].IsNullOrEmpty()) lblE2.Visible = false;
                if (strFlag[2].IsNullOrEmpty()) lblE3.Visible = false;
                if (strFlag[3].IsNullOrEmpty()) lblE4.Visible = false;
                if (strFlag[4].IsNullOrEmpty()) lblE5.Visible = false;
                if (strFlag[5].IsNullOrEmpty()) lblT1.Visible = false;
                if (strFlag[6].IsNullOrEmpty()) lblT2.Visible = false;
                if (strFlag[7].IsNullOrEmpty()) lblT3.Visible = false;
                if (strFlag[8].IsNullOrEmpty()) lblT4.Visible = false;
                if (strFlag[9].IsNullOrEmpty()) lblE0.Visible = false;
                if (strFlag[10].IsNullOrEmpty()) lblT0.Visible = false;
            }
        }

        /// <summary>
        /// 학생상담테이블 생성함
        /// </summary>
        /// <param name="argWrtNo"></param>
        /// <param name="argJong"></param>
        /// <returns></returns>
        string fn_HIC_NEW_SANGDAM_INSERT(long argWrtNo, string argJong)
        {
            string rtnVal = "";
            string strRowId = "";
            int result = 0;

            if (hicResDentalService.GetRowIdbyWrtNo(FnWRTNO).IsNullOrEmpty())
            {
                if (fn_HIC_ExJong_CHECK2(argJong) == "Y")
                {
                    result = hicResDentalService.InsertWrtNo(argWrtNo);

                    if (result < 0)
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                    rtnVal = "구강상담 Table 생성오류";
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 상담여부 체크
        /// </summary>
        /// <param name="argJong"></param>
        /// <returns></returns>
        string fn_HIC_ExJong_CHECK2(string argJong)
        {
            string rtnVal = "";

            rtnVal = hicExjongService.GetGbPrt5byCode(argJong);

            return rtnVal;
        }

        void eTxtClick(object sender, EventArgs e)

        {
            if (sender == txtWrtNo)
            {
                txtWrtNo.SelectionStart = 0;
                txtWrtNo.SelectionLength = txtWrtNo.Text.Length;
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtPanDate)
            {
                if (e.KeyChar == 13)
                {   
                    SendKeys.Send("{Tab}");
                }
            }
            else if (sender == txtPanDrNo)
            {
                if (e.KeyChar == 13)
                {
                    lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
                    SendKeys.Send("{Tab}");
                }
            }
            else if (sender == txtEtcPartExamOpinion)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{Tab}");
                }
            }
            else if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                }
            }
            else if (sender == txtWrtNo)
            {
                string strWrtNo = "";

                if (e.KeyChar == 13)
                {
                    if (txtWrtNo.Text.IsNullOrEmpty()) return;

                    if (hb.READ_GJJONG_CODE(txtWrtNo.Text.To<long>()) == "56")
                    {
                        strWrtNo = txtWrtNo.Text.Trim();
                        txtWrtNo.Text = "";

                        FrmHcSangStudentTeeth = new frmHcSangStudentTeeth(strWrtNo.To<long>());
                        FrmHcSangStudentTeeth.WindowState = FormWindowState.Maximized;
                        FrmHcSangStudentTeeth.ShowDialog(this);

                        this.Close();
                        return;
                    }

                    strWrtNo = txtWrtNo.Text.Trim();

                    fn_Screen_Clear();
                    txtWrtNo.Text = strWrtNo;
                    fn_Screen_Display();
                    return;
                }
            }
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtPanjengJochi || sender == txtSName)
            {
                txtPanjengJochi.ImeMode = ImeMode.Hangul;
            }
            else if (sender == txtResultInterpretation)
            {
                txtResultInterpretation.ImeMode = ImeMode.Hangul;
            }
            else if (sender == txtEtcPartExamOpinion)
            {
                txtEtcPartExamOpinion.ImeMode = ImeMode.Hangul;
            }
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txtPanjengJochi || sender == txtSName)
            {
                txtPanjengJochi.ImeMode = ImeMode.Hangul;
            }
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        void HEA_SANGDAM_SET(long argWrtno, string argRoom)
        {
            int nCNT = 0;
            int nREAD = 0;
            int nWait = 0;
            int nAge = 0;

            string strSname = "";
            string strSex = "";
            string strGjjong = "";

            //기본사항
            HEA_JEPSU item = heaJepsuService.GetItemByGWrtno(argWrtno);
            strSex = item.SEX.Trim();
            strSname = item.SNAME.Trim();
            strGjjong = item.GJJONG.Trim();
            nAge = Convert.ToInt32(item.AGE);

            //다른검사실에 등록되어 있는지 확인
            HEA_SANGDAM_WAIT item2 = heaSangdamWaitService.Read_Sangdam_EtcRoomReg(argWrtno, argRoom);
            if (!item2.IsNullOrEmpty())
            {
                //기존의 자료가 있으면 삭제함
                heaSangdamWaitService.Delete_Sangdam_Wait(argWrtno, argRoom);
            }

            //상담대기순번 완료
            nREAD = 0; nCNT = 0;
            nCNT = heaResultService.GetCountbyWrtNo(argWrtno, argRoom);






        }

    }
}
