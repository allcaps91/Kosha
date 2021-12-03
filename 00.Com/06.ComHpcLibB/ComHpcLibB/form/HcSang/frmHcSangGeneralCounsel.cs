using ComBase;
using ComLibB;
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
using System.ComponentModel;
using ComBase.Controls;
using System.IO;

/// <summary>
/// Class Name      : ComHpcLibB\HC_Sang
/// File Name       : frmHcSangGeneralCounsel.cs
/// Description     : 일반검진 상담 프로그램 (사용무 : 2020.09.02 김경동 확인)
/// Author          : 이상훈
/// Create Date     : 2020-02-12
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHcSang_일반.frm(FrmHcAct01)" />

namespace ComHpcLibB
{
    public partial class frmHcSangGeneralCounsel : Form
    {
        HicSangdamWaitService hicSangdamWaitService = null;
        HicSchoolNewService hicSchoolNewService = null;
        HicJepsuService hicJepsuService = null;
        HicResultService hicResultService = null;
        HicJepsuSangdamNewExjongService hicJepsuSangdamNewExjongService = null;
        HicSangdamNewJepsuExjongService hicSangdamNewJepsuExjongService = null;
        HicSangdamNewService hicSangdamNewService = null;
        HicDoctorService hicDoctorService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicExcodeService hicExcodeService = null;
        HicPatientService hicPatientService = null;
        HicExjongService hicExjongService = null;
        EtcJupmstService etcJupmstService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        frmHcSchoolCommonInput FrmHcSchoolCommonInput = null;
        frmHeaResult FrmHeaResult = null;
        frmViewResult FrmViewResult = null;
        frmHcSangGeneralCounsel FrmHcSangGeneralCounsel = null;
        frmHcSangXRayCounsel FrmHcSangXRayCounsel = null;
        frmHcSchoolCommonDistrictRegView FrmHcSchoolCommonDistrictRegView = null;
        frmHcPanSpcGenSecondView FrmHcPanSpcGenSecondView = null;
        frmHcPanInternetMunjin_New FrmHcPanInternetMunjin_New = null;

        long FnWRTNO;
        long FnWrtno2;      //2차 검진시 이전 1차 접수번호
        long FnPano;
        long FnAge;
        long FnTab;

        string FstrPtno;
        string FstrSex;
        string FstrJepDate;
        string FstrJumin;
        string FstrMunjin;
        string FstrChasu;

        string FstrUCodes;
        string[] FstrHabit = new string[6];
        string FstrJong;
        string FstrChul;

        string FstrGjJong;
        long FnRowNo;           //메모리타자기 위치 저장용
        long FnRow;
        long FnClickRow;        //Help를 Click한 Row

        string FstrYear;
        string FstrExamFlag;
        string FstrTFlag;       //생애검진 플래그
        string FstrROWID;       //상담테이블
        string FstrEndoRowID;
        string FstrEndoGbn;
        long FnCancerGbn;
        long FnHeaWRTNO;        //종합검진 접수번호

        public frmHcSangGeneralCounsel(long nWrtNo)
        {
            InitializeComponent();
            FnWRTNO = nWrtNo;
            SetEvent();
            SetControl();
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void SetEvent()
        {
            hicSangdamWaitService = new HicSangdamWaitService();
            hicSchoolNewService = new HicSchoolNewService();
            hicJepsuService = new HicJepsuService();
            hicResultService = new HicResultService();
            hicJepsuSangdamNewExjongService = new HicJepsuSangdamNewExjongService();
            hicSangdamNewJepsuExjongService = new HicSangdamNewJepsuExjongService();
            hicSangdamNewService = new HicSangdamNewService();
            hicDoctorService = new HicDoctorService();
            hicIeMunjinNewService = new HicIeMunjinNewService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicResultExCodeService = new HicResultExCodeService();
            hicExcodeService = new HicExcodeService();
            hicPatientService = new HicPatientService();
            hicExjongService = new HicExjongService();
            etcJupmstService = new EtcJupmstService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.Click += new EventHandler(eTxtClick);
            this.rdoJob1.Click += new EventHandler(eRdoClick);
            this.rdoJob2.Click += new EventHandler(eRdoClick);
            this.SSHistory.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSList1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSList2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.tab1.Click += new EventHandler(eTabClick);
            this.tab2.Click += new EventHandler(eTabClick);
            this.tab3.Click += new EventHandler(eTabClick);
            this.tab4.Click += new EventHandler(eTabClick);
            this.tab5.Click += new EventHandler(eTabClick);
            
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            if (clsPublic.GstrRetValue == "1")
            {
                //READ_금액표시기_SETTING(FrmHcSang_일반, lstMonitors)
                //hf.Read_Amountindicator(FrmHcSangGeneralCounsel, lstMonitors);
            }

            //처음부터 단일모니터가 아닐경우와
            if (clsHcVariable.singmon != 1)
            {
                //단일이면 1 아니면 (듀얼이면)...(메뉴에서 조정했을경우)
                if (clsHcVariable.selmon == 1)
                {
                    this.Left = 0;
                    this.Top = 0;
                }
                else
                {
                    this.Left = (int)clsHcVariable.slavecoodinate * 15;
                    this.Top = 0;
                }
            }

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;

            //판정의사 여부를 읽음
            hb.READ_HIC_Doctor(long.Parse(clsType.User.IdNumber));

            this.Text += "(" + clsHcVariable.GstrHicDrName + ") (면허:" + clsHcVariable.GnHicLicense + ")";

            fn_ComboBox_Set();
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

            SSList1_Sheet1.Columns[4].Visible = false;
            SSList2_Sheet1.Columns[4].Visible = false;
            SSList3_Sheet1.Columns[4].Visible = false;
            SSList4_Sheet1.Columns[4].Visible = false;

            SSList1_Sheet1.Columns[5].Visible = false;
            SSList2_Sheet1.Columns[5].Visible = false;
            SSList3_Sheet1.Columns[5].Visible = false;
            SSList4_Sheet1.Columns[5].Visible = false;

            SS2_Sheet1.Columns[7].Visible = false;  //결과값코드
            SS2_Sheet1.Columns[8].Visible = false;  //변경
            SS2_Sheet1.Columns[9].Visible = false;  //ROWID
            SS2_Sheet1.Columns[10].Visible = false; //Result Type

            switch (clsHcVariable.GstrDrRoom)
            {
                case "7":
                    tabControl1.SelectedTab = tab1;
                    break;
                case "8":
                    tabControl1.SelectedTab = tab2;
                    break;
                case "9":
                    tabControl1.SelectedTab = tab3;
                    break;
                case "10":
                    tabControl1.SelectedTab = tab4;
                    break;
                default:
                    tabControl1.SelectedTab = tab1;
                    break;
            }

            
            
            eBtnClick(btnSearch, new EventArgs());
        }

        void fn_ComboBox_Set()
        {
            
        }

        void eTxtClick(object sender, EventArgs e)
        {
            if (sender == txtLtdCode)
            {
                eBtnClick(btnLtdCode, new EventArgs());
            }
            else if (sender == txtWrtNo)
            {
                txtWrtNo.SelectionStart = 0;
                txtWrtNo.SelectionLength = txtWrtNo.Text.Length;
            }
        }

        void eRdoClick(object sender, EventArgs e)
        {
            eBtnClick(btnSearch, new EventArgs());
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSHistory)
            {
                if (string.Compare(SSHistory.ActiveSheet.Cells[e.Row, 3].Text, "2") <= 0)
                {
                    clsPublic.GstrRetValue = SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                    FrmHcPanSpcGenSecondView = new frmHcPanSpcGenSecondView(long.Parse(SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim()));
                    FrmHcPanSpcGenSecondView.ShowDialog(this);
                }
                else if (SSHistory.ActiveSheet.Cells[e.Row, 3].Text.Trim() == "3")
                {
                    clsPublic.GstrRetValue = SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                }
            }
            else if (sender == SSList1)
            {
                if (e.RowHeader == true) return;

                FnRow = e.Row;
                fn_Screen_Clear();
                txtWrtNo.Focus();
                txtWrtNo.Text = SSList1.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                SendKeys.Send("{TAB}");
            }
            else if (sender == SSList2)
            {
                if (e.RowHeader == true) return;

                FnRow = e.Row;
                fn_Screen_Clear();
                txtWrtNo.Focus();
                txtWrtNo.Text = SSList2.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                SendKeys.Send("{TAB}");
            }
        }

        void eTabClick(object sender, EventArgs e)
        {
            eBtnClick(btnSearch, new EventArgs());
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            //if (sender == txtPanDrNo)
            //{
            //    lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
            //    SendKeys.Send("{TAB}");
            //}
            //else
            //{
            //    if (e.KeyChar == 13)
            //    {
            //        SendKeys.Send("{TAB}");
            //    }
            //}
        }

        void eBtnClick(object sender, EventArgs e)
        {
            int result = 0;

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            //else if (sender == btnCancel)
            //{
            //    clsDB.setBeginTran(clsDB.DbCon);

            //    //호출은 했으나 상담이 완료안된 접수번호 찾음
            //    if (hicSangdamWaitService.GetCountbyWrtNoGubun(clsHcVariable.GstrDrRoom, long.Parse(txtWrtNo.Text)) > 0)
            //    {
            //        result = hicSangdamWaitService.UpdateCallTimeDisplaybyOnlyWrtNo(long.Parse(txtWrtNo.Text));

            //        if (result < 0)
            //        {
            //            clsDB.setRollbackTran(clsDB.DbCon);
            //            MessageBox.Show("취소처리 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            return;
            //        }
            //    }
            //    clsDB.setCommitTran(clsDB.DbCon);

            //    fn_Screen_Clear();
            //    txtWrtNo.Focus();
            //}
            else if (sender == btnLtdCode)
            {
                frmHcLtdHelp frm = new frmHcLtdHelp();
                frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                frm.ShowDialog();
                frm.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }            
            else if (sender == btnMed)
            {
                FrmHeaResult = new frmHeaResult(FstrJumin);
                FrmHeaResult.ShowDialog(this);
            }
            else if (sender == btnPACS)
            {
                FrmViewResult = new frmViewResult(FstrPtno);
                FrmViewResult.ShowDialog(this);
            }
            else if (sender == btnAudio)
            {
                string strHicPano = "";
                string strHeaPano = "";
                string StrJumin = "";
                string strRowId = "";

                strRowId = etcJupmstService.GetRowIdbyPtNoBDateDeptCode(FstrPtno, FstrJepDate, "6");

                hf.AudioFILE_DBToFile1(strRowId, "1", 1);
            }
            else if (sender == btnPFT)
            {
                //폐기능은 여러건을 볼수 있도록 처리 합니다.
                string fs = "";
                string f = "";
                string S = "";
                string strFileName = "";

                string strHicPano = "";
                string strHeaPano = "";
                string StrJumin = "";

                //기존화일 삭제
                strFileName = @"c:\cmc\*.jpg";

                DirectoryInfo dir = new DirectoryInfo(@"c:\cmc\");
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo F in files)
                {
                    if (F.Extension == ".jpg")
                    {
                        F.Delete();
                    }
                }

                List<ETC_JUPMST> list = etcJupmstService.GetRowIdbyPtNoBDate(FstrPtno, FstrJepDate, "N", "4");

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        hf.PFTFILE_DBToFile(list[i].ROWID, "1", i);
                    }
                }
            }
            else if (sender == btnSave)
            {
                string strJinchal1 = "";
                string strJinchal2 = "";
                string strSiksa = "";
                string strRemark = "";
                string strGbHabit = "";
                string strGbOldByeng = "";
                string stroldByengName = "";
                string[] strSick1 = new string[3];
                string[] strSick2 = new string[3];
                string[] strSick3 = new string[3];
                string[] strSick4 = new string[3];
                string[] strSick5 = new string[3];
                string[] strSick6 = new string[3];
                string[] strSick7 = new string[3];
                string[] strSick8 = new string[3];
                string[] strHabit = new string[3];
                string[] strOldByeng = new string[8];
                string[] strOLD = new string[15];
                string[] strDrug = new string[8];
                string strMSG = "";
                long nMunDrno = 0;
                string strDiet = "";
                string strSTS = "";
                string strOld13 = "";
                string strOld_Etc = "";
                string strDrug_Etc = "";
                string strDrug_Stop1 = "";
                string strDrug_Stop2 = "";
                string str_B_Drug = "";
                string str_B_Drug1 = "";
                string str_B_Drug11 = "";
                string strBigo = "";

                //DATA_ERROR_CHECK
                if (txtPanDrNo.Text.To<long>() == 0)
                {
                    MessageBox.Show("판정의사 면허번호 누락", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strGbHabit = "1";
                strGbOldByeng = "1";
                nMunDrno = 0;

                for (int i = 0; i <= 2; i++)
                {
                    strSick1[i] = "2";
                    strSick2[i] = "2";
                    strSick3[i] = "2";
                    strSick4[i] = "2";
                    strSick5[i] = "2";
                    strSick6[i] = "2";
                    strSick7[i] = "2";
                    strSick8[i] = "2";
                }

                if (hm.HIC_NEW_MUNITEM_INSERT(FnWRTNO, FstrJong, "", FstrUCodes) != "")
                {
                    MessageBox.Show("문진 Table 생성 중 ERROR 발생!!", "상담불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //INPUT_DATA_BUILD
                //진단여부 및 약물복용 여부
                if (chkSick110.Checked == true) { strSick1[1] = "1"; }
                if (chkSick111.Checked == true) { strSick1[2] = "1"; }

                if (chkSick210.Checked == true) { strSick2[1] = "1"; }
                if (chkSick211.Checked == true) { strSick2[2] = "1"; }

                if (chkSick310.Checked == true) { strSick3[1] = "1"; }
                if (chkSick311.Checked == true) { strSick3[2] = "1"; }

                if (chkSick410.Checked == true) { strSick4[1] = "1"; }
                if (chkSick411.Checked == true) { strSick4[2] = "1"; }

                if (chkSick510.Checked == true) { strSick5[1] = "1"; }
                if (chkSick511.Checked == true) { strSick5[2] = "1"; }

                if (chkSick610.Checked == true) { strSick6[1] = "1"; }
                if (chkSick611.Checked == true) { strSick6[2] = "1"; }

                if (chkSick710.Checked == true) { strSick7[1] = "1"; }
                if (chkSick711.Checked == true) { strSick7[2] = "1"; }

                if (chkSick810.Checked == true) { strSick8[1] = "1"; }
                if (chkSick811.Checked == true) { strSick8[2] = "1"; }

                //일반상태
                if (rdoJinchal20.Checked == true) { strJinchal2 = "1"; }
                if (rdoJinchal21.Checked == true) { strJinchal2 = "2"; }
                if (rdoJinchal22.Checked == true) { strJinchal2 = "3"; }

                //외상 및 후유증
                if (rdoJinchal10.Checked == true) { strJinchal1 = "1"; }
                if (rdoJinchal11.Checked == true) { strJinchal1 = "2"; }

                //식사여부
                strSiksa = "";
                if (rdoJinchal10.Checked == true) { strJinchal1 = "1"; }
                if (rdoJinchal11.Checked == true) { strJinchal1 = "2"; }

                //생활습관 개선필요
                for (int i = 0; i <= 4; i++)
                {
                    CheckBox chkHabit = (Controls.Find("chkHabit" + i.ToString(), true)[0] as CheckBox);
                    if (chkHabit.Checked == false)
                    {
                        strHabit[i + 1] = "0";
                    }
                    if (chkHabit.Checked == true)
                    {
                        strHabit[i + 1] = "1";
                        strGbHabit = "2";
                    }
                }

                //과거병력
                for (int i = 0; i <= 6; i++)
                {
                    
                }

                //if (cboPRes.Text.Trim() == "") strFlag = "OK";
                //if (cboEJ0.Text.Trim() == "") strFlag = "OK";
                //if (cboEJ1.Text.Trim() == "") strFlag = "OK";
                //if (cboM.Text.Trim() == "") strFlag = "OK";
                //if (cboN.Text.Trim() == "") strFlag = "OK";
                //if (cboS.Text.Trim() == "") strFlag = "OK";
                //if (cboHJ.Text.Trim() == "") strFlag = "OK";
                //for (int i = 0; i <= 3; i++)
                //{
                //    ComboBox cboJ = (Controls.Find("cboJ" + i.ToString(), true)[0] as ComboBox);
                //    if (cboJ.Text == "") strFlag = "OK";
                //}

                //if (strFlag == "OK")
                //{
                //    MessageBox.Show("판정항목 누락.", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                //if (long.Parse(VB.Left(cboPRes.Text, 2)) > 9)
                //{
                //    strPRes = VB.Left(cboPRes.Text, 2);
                //}
                //else
                //{
                //    //strPRes = long.Parse(VB.Left(cboPRes.Text, 2));
                //    strPRes = VB.Left(cboPRes.Text, 2);
                //}

                //strPPanC[0] = VB.Left(cboEJ1.Text.Trim(), 1) + "^^" + VB.Left(cboEJ0.Text.Trim(), 1) + "^^" + txtEJEtc.Text.Trim() + "^^";
                //strPPanC[1] = VB.Left(cboM.Text.Trim(), 1) + "^^" + txtMEtc.Text.Trim() + "^^";
                //strPPanC[2] = VB.Left(cboN.Text.Trim(), 1) + "^^" + txtNEtc.Text.Trim() + "^^";
                //strPPanC[3] = VB.Left(cboS.Text.Trim(), 1) + "^^" + txtSEtc.Text.Trim() + "^^";
                //strPPanC[4] = VB.Left(cboHJ.Text.Trim(), 1) + "^^" + txtHJEtc.Text.Trim() + "^^";
                ////진찰상담
                //strPPanK[0] = VB.Left(cboJ0.Text.Trim(), 1) + "^^" + txtJEtc0.Text.Trim() + "^^";
                //strPPanK[1] = VB.Left(cboJ1.Text.Trim(), 1) + "^^" + txtJEtc1.Text.Trim() + "^^";
                //strPPanK[2] = VB.Left(cboJ2.Text.Trim(), 1) + "^^" + txtJEtc2.Text.Trim() + "^^";
                //strPPanK[3] = VB.Left(groupBox4.Text.Trim(), 1) + "^^" + txtJEtc3.Text.Trim() + "^^";
                ////기관능력
                //strPPanD[0] = VB.Left(cboOrgan0.Text.Trim(), 1) + "^^" + txtOrgan0Etc.Text.Trim() + "^^";
                //strPPanD[1] = VB.Left(cboOrgan1.Text.Trim(), 1) + "^^" + txtOrgan1Etc.Text.Trim() + "^^";
                //strPPanD[2] = VB.Left(cboOrgan2.Text.Trim(), 1) + "^^" + txtOrgan2Etc.Text.Trim() + "^^";
                //strPPanD[3] = VB.Left(cboOrgan3.Text.Trim(), 1) + "^^" + txtOrgan3Etc.Text.Trim() + "^^";
                //strPPanD[4] = VB.Left(cboOrgan4.Text.Trim(), 1) + "^^" + txtOrgan4Etc.Text.Trim() + "^^";
                //strPPanD[5] = VB.Left(cboOrgan5.Text.Trim(), 1) + "^^" + txtOrgan5Etc.Text.Trim() + "^^";

                //if (txtRemark.Text.Trim() == "")
                //{
                //    strRemark = "특이사항 없음";
                //}
                //else
                //{
                //    strRemark = txtRemark.Text.Trim();
                //}

                //clsDB.setBeginTran(clsDB.DbCon);

                ////UPDATE_SCHOOL_NEW
                //HIC_SCHOOL_NEW item = new HIC_SCHOOL_NEW();

                //item.PPANB1 = strPRes;
                ////item.PPANB2 = txtPResEtc.Text.Trim();
                //item.PPANC4 = strPPanC[0];
                //item.PPANC6 = strPPanC[4];
                //item.PPANC7 = strPPanC[1];
                //item.PPANC8 = strPPanC[2];
                //item.PPANC9 = strPPanC[3];
                //item.PPAND1 = strPPanD[0];
                //item.PPAND2 = strPPanD[1];
                //item.PPAND3 = strPPanD[2];
                //item.PPAND4 = strPPanD[3];
                //item.PPAND5 = strPPanD[4];
                //item.PPAND6 = strPPanD[5];
                //item.PPANK1 = strPPanK[0];
                //item.PPANK2 = strPPanK[1];
                //item.PPANK3 = strPPanK[2];
                //item.PPANK4 = strPPanK[3];
                //item.SANGDAM = strRemark;
                //item.WRTNO = FnWRTNO;

                //result = hicSchoolNewService.UpdatebyPpanBWrtNo(item);

                //if (result < 0)
                //{
                //    clsDB.setRollbackTran(clsDB.DbCon);
                //    MessageBox.Show("저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                ////UPDATE_SANGDAM
                //HIC_SCHOOL_NEW item1 = new HIC_SCHOOL_NEW();

                //item1.SCHPAN1 = strPRes;
                ////item1.SCHPAN2 = txtPResEtc.Text.Trim();
                //item1.SCHPAN3 = strPPanC[0];
                //item1.SCHPAN4 = strPPanC[1];
                //item1.SCHPAN5 = strPPanC[2];
                //item1.SCHPAN6 = strPPanC[3];
                //item1.SCHPAN7 = strPPanC[4];
                //item1.SCHPAN8 = strPPanK[0];
                //item1.SCHPAN9 = strPPanK[1];
                //item1.SCHPAN10 = strPPanK[2];
                //item1.SCHPAN11 = strPPanK[3];
                //item1.REMARK = strRemark;
                //item1.GBSTS = "Y";
                ////item1.SANGDAMDRNO = long.Parse(txtPanDrNo.Text);
                //item1.ENTSABUN = long.Parse(clsType.User.IdNumber);
                //item1.WRTNO = FnWRTNO;

                //result = hicSchoolNewService.UpdatebySchPanWrtNo(item1);

                //if (result < 0)
                //{
                //    clsDB.setRollbackTran(clsDB.DbCon);
                //    MessageBox.Show("저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                //상담여부 체크 및 상담의사, 상담일자 저장
                result = hicJepsuService.UpdateGbNunjinbyWrtNo(long.Parse(clsType.User.IdNumber), FnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담여부 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //상담여부 액팅저장
                result = hicResultService.UpdateEntSabunbyWrtNo(long.Parse(clsType.User.IdNumber), FnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담여부 액팅 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //상담대기순번 완료
                result = hicSangdamWaitService.UpdateGbCallGubunbyWrtNo(clsHcVariable.GstrDrRoom, FnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담내역 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                fn_Screen_Clear();
                eBtnClick(btnSearch, new EventArgs());
                txtWrtNo.Focus();
            }
            else if (sender == btnSearch)
            {
                int ii = 0;
                int nRead = 0;
                int nRow = 0;
                string strJong = "";
                string strTEMP = "";
                string strOK = "";
                string strGuBun = "";
                int nGBn = 0;

                string strFrDate = "";
                string strToDate = "";
                string strSName = "";
                long nLtdCode = 0;

                FnTab = tabControl1.SelectedTabIndex;
                ii = tabControl1.SelectedTabIndex;

                switch (ii)
                {
                    case 0:
                        strGuBun = "7";
                        break;
                    case 1:
                        strGuBun = "8";
                        break;
                    case 2:
                        strGuBun = "9";
                        break;
                    case 3:
                        strGuBun = "10";
                        break;
                    default:
                        strGuBun = "";
                        break;
                }

                for (int i = 0; i <= 3; i++)
                {
                    RadioButton rdoGbn = (Controls.Find("rdoGbn" + i.ToString(), true)[0] as RadioButton);
                    if (rdoGbn.Checked == true)
                    {
                        nGBn = i;
                    }
                }

                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                strSName = txtSName.Text.Trim();

                FpSpread SSList = (Controls.Find("SSList" + (ii + 1).ToString(), true)[0] as FpSpread);
                sp.Spread_All_Clear(SSList);

                List<HIC_JEPSU_SANGDAM_NEW_EXJONG> list = hicJepsuSangdamNewExjongService.GetItembyJepDate(strFrDate, strToDate, nGBn, FnTab, strSName, nLtdCode, strGuBun);

                nRead = list.Count;
                SSList.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    strOK = "OK";
                    strTEMP = list[i].SANGDAMDRNO.ToString();
                    if (rdoJob1.Checked == true)
                    {
                        if (strTEMP != "")
                        {
                            strOK = "";
                        }
                    }
                    else
                    {
                        if (strTEMP != "")
                        {
                            strOK = "";
                        }
                    }

                    if (FnTab == 4)
                    {
                        if (hicSangdamWaitService.GetCountbyOnlyWrtNo(list[i].WRTNO) > 0)
                        {
                            strOK = "";
                        }
                    }

                    if (strOK == "OK")
                    {
                        nRow += 1;
                        if (SSList.ActiveSheet.RowCount < nRow)
                        {
                            SSList.ActiveSheet.RowCount = nRow;
                        }

                        SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                        SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                        SSList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.ToString();
                        SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME.Trim();
                        SSList.ActiveSheet.Cells[i, 2].Text = list[i].JEPDATE.ToString();
                        SSList.ActiveSheet.Cells[i, 3].Text = list[i].GJJONG.Trim();
                        SSList.ActiveSheet.Cells[i, 3].Text = hb.READ_GjJong_HeaName(strJong);

                        //일특 체크
                        if (strJong == "11" || strJong == "16" || strJong == "41" || strJong == "44")
                        {
                            if (list[i].UCODES.Trim() != "")
                            {
                                if (strJong == "11")
                                {
                                    SSList.ActiveSheet.Cells[i, 4].Text = "Y1"; //일특 1차
                                }
                                if (strJong == "16")
                                {
                                    SSList.ActiveSheet.Cells[i, 4].Text = "Y2"; //일특 2차
                                }
                                if (strJong == "41")
                                {
                                    SSList.ActiveSheet.Cells[i, 4].Text = "Y3"; //일특 1차(생애)
                                }
                                if (strJong == "44")
                                {
                                    SSList.ActiveSheet.Cells[i, 4].Text = "Y4"; //일특 1차(생애)
                                }

                                SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                                SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(200, 255, 200);
                            }
                        }

                        if (strTEMP == clsType.User.IdNumber.Trim())
                        {
                            SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                            SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0FF"));
                        }
                    }
                }

                if (rdoJob1.Checked == true)
                {
                    SSList.ActiveSheet.Cells[0, 0, SSList.ActiveSheet.RowCount - 1, SSList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                }

                //상담인원 및 대기인원 DISPLAY
                HIC_SANGDAM_NEW_JEPSU_EXJONG list2 = hicSangdamNewJepsuExjongService.GetCntCnt2(clsHcVariable.GnHicLicense);

                lblCounter.Text = "총 대기인원: ";
                lblCounter.Text += list2.CNT2.ToString() + " 명 / ";
                lblCounter.Text += "상담인원 : ";
                lblCounter.Text += list2.CNT.ToString() + " 명 / ";
            }
            else if (sender == btnWards)
            {
                FrmHcSchoolCommonInput = new frmHcSchoolCommonInput("4");
                FrmHcSchoolCommonInput.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSchoolCommonInput.ssPanjengDblClick += new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick);
                FrmHcSchoolCommonInput.ShowDialog(this);
                FrmHcSchoolCommonInput.ssPanjengDblClick -= new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick);
            }
            else if (sender == btnWards2)
            {
                FrmHcSchoolCommonInput = new frmHcSchoolCommonInput("4");
                FrmHcSchoolCommonInput.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSchoolCommonInput.ssPanjengDblClick += new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick2);
                FrmHcSchoolCommonInput.ShowDialog(this);
                FrmHcSchoolCommonInput.ssPanjengDblClick -= new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick2);
            }
            else if (sender == btnWards3)
            {
                FrmHcSchoolCommonInput = new frmHcSchoolCommonInput("4");
                FrmHcSchoolCommonInput.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSchoolCommonInput.ssPanjengDblClick += new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick3);
                FrmHcSchoolCommonInput.ShowDialog(this);
                FrmHcSchoolCommonInput.ssPanjengDblClick -= new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick3);
            }
            else if (sender == btnMenuWait)
            {
                int nRead = 0;
                int nWaitNo = 0;
                string strFrDate = "";
                string strToDate = "";
                string strFrDate1 = "";
                string strToDate1 = "";

                strFrDate = FstrJepDate;
                strToDate = DateTime.Parse(FstrJepDate).AddDays(1).ToShortDateString();

                strFrDate1 = clsPublic.GstrSysDate;
                strToDate1 = DateTime.Parse(clsPublic.GstrSysDate).AddDays(1).ToShortDateString();

                if (FnWRTNO == 0)
                {
                    return;
                }

                if (clsHcVariable.GstrDrRoom == "")
                {
                    return;
                }

                if (MessageBox.Show("상담실 방번호를 변경하시겠습니까?", "상담실 구분변경", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsHcVariable.GnWRTNO = FnWRTNO;

                nWaitNo = 2;

                if (clsHcVariable.GstrDrRoom == hicSangdamWaitService.GetGubunbyPaNoWrtNo(clsHcVariable.GnWRTNO))
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                HIC_SANGDAM_WAIT list = hicSangdamWaitService.GetMaxWaitNobyGubunWaitNo(clsHcVariable.GstrDrRoom);

                if (!list.IsNullOrEmpty())
                {
                    List<HIC_SANGDAM_WAIT> list2 = hicSangdamWaitService.GetItembyGubunEntTime(clsHcVariable.GstrDrRoom, strFrDate, strToDate);

                    nRead = list2.Count;
                    for (int i = 0; i < nRead; i++)
                    {
                        nWaitNo += 1;
                        result = hicSangdamWaitService.UpdateWaitNobyWrtNo(nWaitNo, list2[i].WRTNO, clsHcVariable.GstrDrRoom, strFrDate1, strToDate1);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("상담순번 변경 시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                result = hicSangdamWaitService.UpdateWaitNoGubunbyWrtNo(clsHcVariable.GstrDrRoom, clsHcVariable.GnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담순번 변경 시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnMenuWard)
            {
                FrmHcSchoolCommonDistrictRegView = new frmHcSchoolCommonDistrictRegView();
                FrmHcSchoolCommonDistrictRegView.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSchoolCommonDistrictRegView.ShowDialog(this);
            }
            else if (sender == btnMenuCancel)
            {
                fn_Screen_Clear();
            }
            else if (sender == btnMenuWait)
            {
                int nRead = 0;
                int nWaitNo = 0;
                string strFrDate = "";
                string strToDate = "";
                string strFrDate1 = "";
                string strToDate1 = "";

                strFrDate = FstrJepDate;
                strToDate = DateTime.Parse(FstrJepDate).AddDays(1).ToShortDateString();

                strFrDate1 = clsPublic.GstrSysDate;
                strToDate1 = DateTime.Parse(clsPublic.GstrSysDate).AddDays(1).ToShortDateString();


                if (FnWRTNO == 0)
                {
                    return;
                }

                if (clsHcVariable.GstrDrRoom == "")
                {
                    return;
                }

                if (MessageBox.Show("상담실 방번호를 변경하시겠습니까?", "상담실 구분변경", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsHcVariable.GnWRTNO = FnWRTNO;

                nWaitNo = 2;

                if (clsHcVariable.GstrDrRoom == hicSangdamWaitService.GetGubunbyWrtNo(clsHcVariable.GnWRTNO))
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                HIC_SANGDAM_WAIT list = hicSangdamWaitService.GetMaxWaitNobyDrRoom(clsHcVariable.GstrDrRoom);

                if (!list.IsNullOrEmpty())
                {
                    List<HIC_SANGDAM_WAIT> list2 = hicSangdamWaitService.GetWrtNoWaitNobyGubunEntTime(clsHcVariable.GstrDrRoom, strFrDate, strToDate);

                    for (int i = 0; i < list2.Count; i++)
                    {
                        nWaitNo += 1;
                        result = hicSangdamWaitService.UpdateWaitNobyWrtnoGubunEntTime(nWaitNo, list2[i].WRTNO, clsHcVariable.GstrDrRoom, strFrDate1, strToDate1);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("상담순번 변경 시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                result = hicSangdamWaitService.UpdateWaitNoGubunbyWrtNo(clsHcVariable.GstrDrRoom, clsHcVariable.GnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담순번 변경 시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void frmHcSchoolCommonInput_ssPanjengDblClick(string strGubun, string strWardName)
        {
            txtRemark1.Text += strGubun + strWardName;
        }

        void frmHcSchoolCommonInput_ssPanjengDblClick2(string strGubun, string strWardName)
        {
            txtRemark2.Text += strGubun + strWardName;
        }

        void frmHcSchoolCommonInput_ssPanjengDblClick3(string strGubun, string strWardName)
        {
            txtNotes.Text += strGubun + strWardName;
        }

        void fn_Screen_Clear()
        {
            FnTab = 0;
            //공통항목 Clear-----------------------------
            txtLtdCode.Text = "";
            txtSName.Text = "";
            txtWrtNo.Text = "";
            sp.Spread_All_Clear(SSHistory);
            sp.Spread_All_Clear(ssPatInfo);
            sp.Spread_All_Clear(SS2);
            SS2.ActiveSheet.RowCount = 50;
            lblGjJong.Text = "";
            tab21.Visible = true;
            tab22.Visible = true;
            tabControl2.SelectedTabIndex = 0;
            tabControl2.Enabled = false;
            tabControl4.SelectedTabIndex = 0;
            tabControl4.SelectedTab = tab41;
            tab42.Visible = true;
            pnlList.Visible = false;

            btnResOK.Enabled = false;
            btnResOK.Visible = false;
            btnSpcRes.Enabled = false;

            pnlBottom1.Enabled = true;
            lblSpc.Visible = false;
            lblSpc.Text = "";

            btnAm.Enabled = false;

            FstrTFlag = "";

            //==[ 1차 상담항목 ]=======================================================================
            //----------------------------
            //         일반검진
            //----------------------------
            for (int i = 0; i <= 1; i++)
            {
                CheckBox chkSick11 = (Controls.Find("chkSick11" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkSick21 = (Controls.Find("chkSick21" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkSick31 = (Controls.Find("chkSick31" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkSick41 = (Controls.Find("chkSick41" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkSick51 = (Controls.Find("chkSick51" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkSick61 = (Controls.Find("chkSick61" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkSick71 = (Controls.Find("chkSick71" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkSick81 = (Controls.Find("chkSick81" + i.ToString(), true)[0] as CheckBox);

                chkSick11.Checked = false;  //뇌졸증
                chkSick21.Checked = false;  //심장병
                chkSick31.Checked = false;  //고혈압
                chkSick41.Checked = false;  //당뇨병
                chkSick51.Checked = false;  //이상지질
                chkSick61.Checked = false;  //기타(암포함)
                chkSick71.Checked = false;  //간장질환
                chkSick81.Checked = false;  //폐결핵
            }
            txtOldByengName.Text = "";
            grpPresentCondition4.Enabled = false;

            //----------------------------
            //         특수검진
            //----------------------------
            txtGajok.Text = "";
            txtGiinsung.Text = "";
            txtJengSang.Text = "";

            //임상진찰
            for (int i = 0; i <= 3; i++)
            {
                TextBox txtJinChal = (Controls.Find("txtJinChal" + i.ToString(), true)[0] as TextBox);
                txtJinChal.Text = "";
            }

            //============================================
            for (int i = 0; i <= 2; i++)
            {
                RadioButton rdoDang = (Controls.Find("rdoDang" + i.ToString(), true)[0] as RadioButton);
                RadioButton rdoDangJob = (Controls.Find("rdoDangJob" + i.ToString(), true)[0] as RadioButton);
                RadioButton rdoGohyul = (Controls.Find("rdoGohyul" + i.ToString(), true)[0] as RadioButton);
                RadioButton rdoGohyulJob = (Controls.Find("rdoGohyulJob" + i.ToString(), true)[0] as RadioButton);

                rdoDang.Checked = false;
                rdoDangJob.Checked = false;
                rdoGohyul.Checked = false;
                rdoGohyulJob.Checked = false;
            }
            //============================================

            //공통항목
            rdoJinchal20.Checked = true;
            rdoJinchal10.Checked = true;
            rdoDiet11.Checked = true;
            rdoDiet21.Checked = true;
            for (int i = 0; i <= 4; i++)
            {
                CheckBox chkHabit = (Controls.Find("chkHabit" + i.ToString(), true)[0] as CheckBox);
            }

            txtRemark1.Text = "";
            txtRemark2.Text = "";
            txtPanDrNo.Text = "";
            txtPanDrNo2.Text = "";
            lblDrName.Text = "";
            lblDrName2.Text = "";
            btnSave.Enabled = true;
            btnSave2.Enabled = true;

            //생애관련 항목
            txtSmkScr.Text = "";
            txtDrkScr.Text = "";
            txtHelthScr.Text = "";
            txtDietScr.Text = "";

            chkDepress.Checked = false;
            chkDementia.Checked = false;
            chkWHabit0.Checked = false;
            chkWHabit1.Checked = false;
            chkWHabit2.Checked = false;

            for (int i = 0; i <= 8; i++)
            {
                if (i < 7)
                {
                    CheckBox chkBiman = (Controls.Find("chkBiman" + i.ToString(), true)[0] as CheckBox);
                    chkBiman.Checked = false;
                }

                if (i < 3)
                {
                    CheckBox chkDiet = (Controls.Find("chkDiet" + i.ToString(), true)[0] as CheckBox);
                    CheckBox chkDiet3 = (Controls.Find("chkDiet3" + i.ToString(), true)[0] as CheckBox);
                    RadioButton rdoSmk = (Controls.Find("rdoSmk" + i.ToString(), true)[0] as RadioButton);
                    RadioButton rdoDrk = (Controls.Find("rdoDrk" + i.ToString(), true)[0] as RadioButton);
                    RadioButton rdoHelth4 = (Controls.Find("rdoHelth4" + i.ToString(), true)[0] as RadioButton);

                    chkDiet.Checked = false;
                    chkDiet3.Checked = false;
                    rdoSmk.Checked = false;
                    rdoDrk.Checked = false;
                    rdoHelth4.Checked = false;
                }

                if (i < 8)
                {
                    RadioButton rdoHelth2 = (Controls.Find("rdoHelth2" + i.ToString(), true)[0] as RadioButton);
                    rdoHelth2.Checked = false;
                }

                if (i < 4)
                {
                    RadioButton rdoHelth3 = (Controls.Find("rdoHelth3" + i.ToString(), true)[0] as RadioButton);
                    rdoHelth3.Checked = false;
                }
            }

            chkDiet20.Checked = false;
            chkDiet21.Checked = false;
            chkDiet4.Checked = false;

            cboSmk1.SelectedIndex = 0;
            cboDrink1.SelectedIndex = 0;
            cboHelth1.SelectedIndex = 0;
            cboDiet.SelectedIndex = 0;
            cboBiman1.SelectedIndex = 0;
            cboBiman2.SelectedIndex = 0;

            for (int i = 0; i <= 5; i++)
            {
                FstrHabit[i] = "";
            }

            tabLife1.Visible = true;
            tabLife2.Visible = true;
            tabLife3.Visible = true;
            tabLife4.Visible = true;
            tabLife5.Visible = true;
            tabLife6.Visible = true;

            //내시경 기록지 ---------------------------------------------------------------------
            for (int i = 0; i <= 5; i++)
            {
                CheckBox chkJong = (Controls.Find("chkJong" + i.ToString(), true)[0] as CheckBox);
                chkJong.Checked = false;
            }

            pnlAM.Visible = false;

            chkEGD0.Checked = false;
            chkEGD1.Checked = false;
            chkCFS0.Checked = false;
            chkCFS1.Checked = false;

            rdoFood1.Checked = true;
            rdoWholeBody0.Checked = true;
            for (int i = 0; i <= 14; i++)
            {
                CheckBox chkMedHistory = (Controls.Find("chkMedHistory" + i.ToString(), true)[0] as CheckBox);
                chkMedHistory.Checked = false;
            }

            txtMedHistory13.Text = "";
            txtMedHistoryEtc.Text = "";

            for (int i = 0; i <= 7; i++)
            {
                CheckBox chkMedicine = (Controls.Find("chkMedicine" + i.ToString(), true)[0] as CheckBox);
                chkMedicine.Checked = false;
            }
            txtMedHistoryEtc.Text = "";

            txtMedAspirin.Text = "";
            txtAntiCoagulantDrug.Text = "";

            for (int i = 0; i <= 1; i++)
            {
                CheckBox chkPreTreatment = (Controls.Find("chkPreTreatment" + i.ToString(), true)[0] as CheckBox);
                chkPreTreatment.Checked = false;
            }
            chkPreTreatment0.Checked = true;
            txtTranquilizerUse.Text = "";
            txtNotes.Text = "";
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }
    }
}
