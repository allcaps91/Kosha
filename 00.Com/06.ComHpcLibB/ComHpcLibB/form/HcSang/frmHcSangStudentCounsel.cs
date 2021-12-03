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
/// File Name       : frmHcSangStudentCounsel.cs
/// Description     : 학생검진 상담 프로그램
/// Author          : 이상훈
/// Create Date     : 2020-02-05
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHcSang_학생.frm(FrmAct03)" />

namespace ComHpcLibB
{
    public partial class frmHcSangStudentCounsel : Form
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
        //frmHcSangGeneralCounsel FrmHcSangGeneralCounsel = null;
        frmHcSangXRayCounsel FrmHcSangXRayCounsel = null;
        frmHcSchoolCommonDistrictRegView FrmHcSchoolCommonDistrictRegView = null;
        frmHcPanSpcGenSecondView FrmHcPanSpcGenSecondView = null;
        frmHcPanInternetMunjin_New FrmHcPanInternetMunjin_New = null;
        frmHcSangInternetMunjinView FrmHcSangInternetMunjinView = null;

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

        string FstrGjJong;
        long FnRowNo;           //메모리타자기 위치 저장용
        long FnRow;

        string FstrYear;
        string FstrROWID;       //상담테이블
        string FstrROWID2;      //판정테이블
        long FnHeaWRTNO;        //종합검진 접수번호

        public frmHcSangStudentCounsel(long nWrtNo)
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

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnMenuCancel.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnMenuWait.Click += new EventHandler(eBtnClick);
            this.btnMenuWard.Click += new EventHandler(eBtnClick);
            this.btnPACS.Click += new EventHandler(eBtnClick);
            this.btnMed.Click += new EventHandler(eBtnClick); 
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
            this.txtEJEtc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHJEtc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtJEtc0.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtJEtc1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtJEtc2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtJEtc3.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtMEtc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtPanDrNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtPResEtc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtPanDrNo.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtRemark.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtRemark.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtSEtc.GotFocus += new EventHandler(eTxtGotFocus);
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
            hb.READ_HIC_Doctor(clsType.User.IdNumber.To<long>());

            this.Text += "(" + clsHcVariable.GstrHicDrName + ") (면허:" + clsHcVariable.GnHicLicense + ")";

            SSList1_Sheet1.Columns[4].Visible = false;
            SSList2_Sheet1.Columns[4].Visible = false;
            SSList3_Sheet1.Columns[4].Visible = false;
            SSList4_Sheet1.Columns[4].Visible = false;

            fn_ComboBox_Set();
            fn_Screen_Clear();
            eBtnClick(btnSearch, new EventArgs());

            if (FnWRTNO > 0)
            {
                txtWrtNo.Text = FnWRTNO.To<string>();                
                eTxtKeyPress(txtWrtNo, new KeyPressEventArgs((char)Keys.Enter));
            }
        }

        void fn_ComboBox_Set()
        {
            //근골격 및 척추
            cboPRes.Items.Clear();
            cboPRes.Items.Add(" ");
            cboPRes.Items.Add("01.정상");
            cboPRes.Items.Add("02.흉부측만");
            cboPRes.Items.Add("03.흉부후만");
            cboPRes.Items.Add("04.흉부전만");
            cboPRes.Items.Add("05.오목가슴");
            cboPRes.Items.Add("06.새가슴");
            cboPRes.Items.Add("07.요통");
            cboPRes.Items.Add("08.어깨결림");
            cboPRes.Items.Add("09.발달장애");
            cboPRes.Items.Add("10.기타");
            cboPRes.SelectedIndex = 0;

            //안질환(좌/우)
            for (int i = 0; i <= 1; i++)
            {
                ComboBox cboEJ = (Controls.Find("cboEJ" + i.To<string>(), true)[0] as ComboBox);
                cboEJ.Items.Clear();
                cboEJ.Items.Add(" ");
                cboEJ.Items.Add("1.없음");
                cboEJ.Items.Add("2.결막염");
                cboEJ.Items.Add("3.눈썹찔림증");
                cboEJ.Items.Add("4.사시");
                cboEJ.Items.Add("5.기타");
                cboEJ.SelectedIndex = 0;
            }

            //콧병
            cboM.Items.Clear();
            cboM.Items.Add(" ");
            cboM.Items.Add("1.없음");
            cboM.Items.Add("2.부비동염");
            cboM.Items.Add("3.비염");
            cboM.Items.Add("4.기타");
            cboM.SelectedIndex = 1;

            //목병
            cboN.Items.Clear();
            cboN.Items.Add(" ");
            cboN.Items.Add("1.없음");
            cboN.Items.Add("2.편도비대");
            cboN.Items.Add("3.임파절증대");
            cboN.Items.Add("4.갑상선비대");
            cboN.Items.Add("5.기타");
            cboN.SelectedIndex = 1;

            //피부병
            cboS.Items.Clear();
            cboS.Items.Add(" ");
            cboS.Items.Add("1.없음");
            cboS.Items.Add("2.아토피성피부염");
            cboS.Items.Add("3.전염성피부염");
            cboS.Items.Add("4.기타");
            cboS.SelectedIndex = 1;

            //귓병
            cboHJ.Items.Clear();
            cboHJ.Items.Add(" ");
            cboHJ.Items.Add("1.없음");
            cboHJ.Items.Add("2.중이염");
            cboHJ.Items.Add("3.외이도염");
            cboHJ.Items.Add("4.기타");
            cboHJ.SelectedIndex = 1;

            //진찰 및 상담
            cboJ0.Items.Clear();
            cboJ0.Items.Add(" ");
            cboJ0.Items.Add("1.무");
            cboJ0.Items.Add("2.유");
            cboJ0.SelectedIndex = 1;

            cboJ1.Items.Clear();
            cboJ1.Items.Add(" ");
            cboJ1.Items.Add("1.양호");
            cboJ1.Items.Add("2.개선필요");
            cboJ1.SelectedIndex = 1;

            cboJ2.Items.Clear();
            cboJ2.Items.Add(" ");
            cboJ2.Items.Add("1.무");
            cboJ2.Items.Add("2.유");
            cboJ2.SelectedIndex = 1;

            cboJ3.Items.Clear();
            cboJ3.Items.Add(" ");
            cboJ3.Items.Add("1.양호");
            cboJ3.Items.Add("2.보통");
            cboJ3.Items.Add("3.불량");
            cboJ3.SelectedIndex = 1;

            //기관능력
            for (int i = 0; i <= 5; i++)
            {
                ComboBox cboOrgan = (Controls.Find("cboOrgan" + i.To<string>(), true)[0] as ComboBox);
                cboOrgan.Items.Clear();
                cboOrgan.Items.Add(" ");
                cboOrgan.Items.Add("1.정상");
                cboOrgan.Items.Add("2.예방필요");
                cboOrgan.Items.Add("3.정밀검사");
                cboOrgan.SelectedIndex = 1;
            }
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
                    clsPublic.GstrRetValue = SSHistory.ActiveSheet.Cells[e.Row, 2].Text;
                    FrmHcPanSpcGenSecondView = new frmHcPanSpcGenSecondView(SSHistory.ActiveSheet.Cells[e.Row, 2].Text.To<long>());
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
            if (sender == txtPanDrNo)
            {
                lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                SendKeys.Send("{TAB}");
            }
            else if (sender == txtWrtNo)
            {
                if (e.KeyChar == 13)
                {
                    eTxtLostFocus(txtWrtNo, new EventArgs());
                    SendKeys.Send("{TAB}");
                }
            }
            else  
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txtRemark)
            {
                txtRemark.ImeMode = ImeMode.Hangul;
            }
            else if (sender == txtPanDrNo)
            {
                if (txtPanDrNo.Text.Length != 0)
                {
                    btnSave.Enabled = true;
                }
            }
            else if (sender == txtSName)
            {
                txtSName.ImeMode = ImeMode.Hangul;
            }
            else if (sender == txtWrtNo)
            {
                string strJong = "";
                string strWrtNo = "";

                if (txtWrtNo.Text.Trim() == "")
                {
                    return;
                }

                strJong = hb.READ_GJJONG_CODE(txtWrtNo.Text.To<long>());

                if (strJong != "56" && strJong != "50" && strJong != "51")
                {
                    strWrtNo = txtWrtNo.Text.Trim();
                    txtWrtNo.Text = "";

                    this.Hide();
                    //FrmHcSangGeneralCounsel = new frmHcSangGeneralCounsel(strWrtNo.To<long>());
                    //FrmHcSangGeneralCounsel.ShowDialog(this);
                    SendKeys.Send("{TAB}");
                    return;
                }
                else if (strJong == "50" || strJong == "51")
                {
                    strWrtNo = txtWrtNo.Text.Trim();
                    txtWrtNo.Text = "";
                    this.Hide();
                    FrmHcSangXRayCounsel = new frmHcSangXRayCounsel(strWrtNo.To<long>());
                    FrmHcSangXRayCounsel.ShowDialog(this);
                    SendKeys.Send("{TAB}");
                    return;
                }

                strWrtNo = txtWrtNo.Text.Trim();

                fn_Screen_Clear();
                txtWrtNo.Text = strWrtNo;
                SendKeys.Send("{TAB}");
                fn_Screen_Display();
            }
        }

        /// <summary>
        /// 상담여부 체크
        /// </summary>
        /// <param name="argJong"></param>
        /// <returns></returns>
        string fn_HIC_ExJong_CHECK2(string argJong)
        {
            string rtnVal = "";

            rtnVal = hicExjongService.GetGbSangdambyCode(argJong).Trim();

            return rtnVal;
        }

        string fn_HIC_NEW_SANGDAM_INSERT(long argWrtNo, string argJong)
        {
            string rtnVal = "";
            string strRowId = "";
            int result = 0;

            strRowId = hicSangdamNewService.GetRowIdbyWrtNo(FnWRTNO);

            if (strRowId == "")
            {
                HIC_SANGDAM_NEW item = new HIC_SANGDAM_NEW();

                item.WRTNO = argWrtNo;
                item.GJJONG = argJong;
                item.GJCHASU = FstrChasu;
                item.JEPDATE = FstrJepDate;
                item.PANO = FnPano;
                item.PTNO = FstrPtno;
                item.GBSTS = "";

                clsDB.setBeginTran(clsDB.DbCon);
                if (fn_HIC_ExJong_CHECK2(argJong) == "Y")
                {
                    result = hicSangdamNewService.Insert(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        rtnVal = "암문진생성시 오류";
                        return rtnVal;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
            }

            return rtnVal;
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtRemark)
            {
                txtRemark.ImeMode = ImeMode.Hangul;
            }
            else if (sender == txtSEtc)
            {
                txtSEtc.ImeMode = ImeMode.Hangul;
            }
            else if (sender == txtSName)
            {
                txtSName.ImeMode = ImeMode.Hangul;
            }

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
            int result = 0;

            if (sender == btnExit)
            {
                //if (FormIsExist(FrmHcPanInternetMunjin_New.GetType()))
                //{
                //    FrmHcPanInternetMunjin_New.Dispose();
                //    FrmHcPanInternetMunjin_New.Close();
                //}

                ComFunc.KillProc("friendly omr.exe");
                ComFunc.KillProc("hcscript.exe");

                this.Close();
                return;
            }
            else if (sender == btnCancel)
            {
                clsDB.setBeginTran(clsDB.DbCon);

                //호출은 했으나 상담이 완료안된 접수번호 찾음
                if (hicSangdamWaitService.GetCountbyWrtNoGubun(clsHcVariable.GstrDrRoom, txtWrtNo.Text.To<long>()) > 0)
                {
                    result = hicSangdamWaitService.UpdateCallTimeDisplaybyOnlyWrtNo(txtWrtNo.Text.To<long>());

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("취소처리 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);

                fn_Screen_Clear();
                txtWrtNo.Focus();
            }
            else if (sender == btnLtdCode)
            {
                frmHcLtdHelp frm = new frmHcLtdHelp();
                frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                frm.ShowDialog();
                frm.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.To<string>() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
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
                FrmViewResult.StartPosition = FormStartPosition.CenterParent;
                FrmViewResult.ShowDialog(this);
            }
            else if (sender == btnSave)
            {
                string strFlag="";
                string strPRes = "";
                string strRemark = "";
                string[] strPPanC = new string[5];
                string[] strPPanK = new string[4];
                string[] strPPanD = new string[6];

                strFlag = "";

                //DATA_ERROR_CHECK
                if (txtPanDrNo.Text.To<long>() == 0)
                {
                    MessageBox.Show("판정의사 면허번호 누락", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (cboPRes.Text.Trim() == "") strFlag = "OK";
                if (cboEJ0.Text.Trim() == "") strFlag = "OK";
                if (cboEJ1.Text.Trim() == "") strFlag = "OK";
                if (cboM.Text.Trim() == "") strFlag = "OK";
                if (cboN.Text.Trim() == "") strFlag = "OK";
                if (cboS.Text.Trim() == "") strFlag = "OK";
                if (cboHJ.Text.Trim() == "") strFlag = "OK";
                for (int i = 0; i <= 3; i++)
                {
                    ComboBox cboJ = (Controls.Find("cboJ" + i.To<string>(), true)[0] as ComboBox);
                    if (cboJ.Text == "") strFlag = "OK";
                }

                if (strFlag == "OK")
                {
                    MessageBox.Show("판정항목 누락.", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (VB.Left(cboPRes.Text, 2).To<long>() > 9)
                {
                    strPRes = VB.Left(cboPRes.Text, 2);
                }
                else
                {
                    //strPRes = VB.Left(cboPRes.Text, 2).To<long>();
                    strPRes = VB.Left(cboPRes.Text, 2);
                }

                strPPanC[0] = VB.Left(cboEJ1.Text.Trim(), 1) +"^^" + VB.Left(cboEJ0.Text.Trim(), 1) + "^^" + txtEJEtc.Text.Trim() + "^^";
                strPPanC[1] = VB.Left(cboM.Text.Trim(), 1) + "^^" + txtMEtc.Text.Trim() + "^^";
                strPPanC[2] = VB.Left(cboN.Text.Trim(), 1) +"^^" + txtNEtc.Text.Trim() + "^^";
                strPPanC[3] = VB.Left(cboS.Text.Trim(), 1) +"^^" + txtSEtc.Text.Trim() + "^^";
                strPPanC[4] = VB.Left(cboHJ.Text.Trim(), 1) +"^^" + txtHJEtc.Text.Trim() + "^^";
                //진찰상담
                strPPanK[0] = VB.Left(cboJ0.Text.Trim(), 1) +"^^" + txtJEtc0.Text.Trim() + "^^";
                strPPanK[1] = VB.Left(cboJ1.Text.Trim(), 1) +"^^" + txtJEtc1.Text.Trim() + "^^";
                strPPanK[2] = VB.Left(cboJ2.Text.Trim(), 1) +"^^" + txtJEtc2.Text.Trim() + "^^";
                strPPanK[3] = VB.Left(groupBox4.Text.Trim(), 1) +"^^" + txtJEtc3.Text.Trim() + "^^";
                //기관능력
                strPPanD[0] = VB.Left(cboOrgan0.Text.Trim(), 1) + "^^" + txtOrgan0Etc.Text.Trim() + "^^";
                strPPanD[1] = VB.Left(cboOrgan1.Text.Trim(), 1) + "^^" + txtOrgan1Etc.Text.Trim() + "^^";
                strPPanD[2] = VB.Left(cboOrgan2.Text.Trim(), 1) + "^^" + txtOrgan2Etc.Text.Trim() + "^^";
                strPPanD[3] = VB.Left(cboOrgan3.Text.Trim(), 1) +"^^" + txtOrgan3Etc.Text.Trim() + "^^";
                strPPanD[4] = VB.Left(cboOrgan4.Text.Trim(), 1) +"^^" + txtOrgan4Etc.Text.Trim() + "^^";
                strPPanD[5] = VB.Left(cboOrgan5.Text.Trim(), 1) + "^^" + txtOrgan5Etc.Text.Trim() + "^^";

                if (txtRemark.Text.Trim() == "")
                {
                    strRemark = "특이사항 없음";
                }
                else
                {
                    strRemark = txtRemark.Text.Trim();
                }

                clsDB.setBeginTran(clsDB.DbCon);

                //UPDATE_SCHOOL_NEW
                HIC_SCHOOL_NEW item = new HIC_SCHOOL_NEW();

                item.PPANB1 = strPRes;
                item.PPANB2 = txtPResEtc.Text.Trim();
                item.PPANC4 = strPPanC[0];
                item.PPANC6 = strPPanC[4];
                item.PPANC7 = strPPanC[1];
                item.PPANC8 = strPPanC[2];
                item.PPANC9 = strPPanC[3];
                item.PPAND1 = strPPanD[0];
                item.PPAND2 = strPPanD[1];
                item.PPAND3 = strPPanD[2];
                item.PPAND4 = strPPanD[3];
                item.PPAND5 = strPPanD[4];
                item.PPAND6 = strPPanD[5];
                item.PPANK1 = strPPanK[0];
                item.PPANK2 = strPPanK[1];
                item.PPANK3 = strPPanK[2];
                item.PPANK4 = strPPanK[3];
                item.SANGDAM = strRemark;
                item.WRTNO = FnWRTNO;

                result = hicSchoolNewService.UpdatebyPpanBWrtNo(item);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //UPDATE_SANGDAM
                HIC_SCHOOL_NEW item1 = new HIC_SCHOOL_NEW();

                item1.SCHPAN1 = strPRes;
                item1.SCHPAN2 = txtPResEtc.Text.Trim();
                item1.SCHPAN3 = strPPanC[0];
                item1.SCHPAN4 = strPPanC[1];
                item1.SCHPAN5 = strPPanC[2];
                item1.SCHPAN6 = strPPanC[3];
                item1.SCHPAN7 = strPPanC[4];
                item1.SCHPAN8 = strPPanK[0];
                item1.SCHPAN9 = strPPanK[1];
                item1.SCHPAN10 = strPPanK[2];
                item1.SCHPAN11 = strPPanK[3];
                item1.REMARK = strRemark;
                item1.GBSTS = "Y";
                item1.SANGDAMDRNO = txtPanDrNo.Text.To<long>();
                item1.ENTSABUN = clsType.User.IdNumber.To<long>();
                item1.WRTNO = FnWRTNO;

                result = hicSchoolNewService.UpdatebySchPanWrtNo(item1);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //상담여부 체크 및 상담의사, 상담일자 저장
                result = hicJepsuService.UpdateGbNunjinbyWrtNo(clsType.User.IdNumber.To<long>(), FnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담여부 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //상담여부 액팅저장
                result = hicResultService.UpdateEntSabunbyWrtNo(clsType.User.IdNumber.To<long>(), FnWRTNO);

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

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

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

                for (int i = 1; i <= 4; i++)
                {
                    RadioButton rdoGbn = (Controls.Find("rdoGbn" + i.To<string>(), true)[0] as RadioButton);
                    if (rdoGbn.Checked == true)
                    {
                        nGBn = i - 1;
                    }
                }

                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                strSName = txtSName.Text.Trim();

                FpSpread SSList = (Controls.Find("SSList" + (ii + 1).To<string>(), true)[0] as FpSpread);
                sp.Spread_All_Clear(SSList);

                List<HIC_JEPSU_SANGDAM_NEW_EXJONG> list = hicJepsuSangdamNewExjongService.GetItembyJepDate(strFrDate, strToDate, nGBn, FnTab, strSName, nLtdCode, strGuBun);

                nRead = list.Count;
                //SSList.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    strOK = "OK";
                    strTEMP = list[i].SANGDAMDRNO.To<string>();
                    if (rdoJob1.Checked == true)
                    {
                        if (strTEMP != "")
                        {
                            strOK = "";
                        }
                    }
                    else
                    {
                        if (strTEMP.IsNullOrEmpty())
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

                        SSList.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                        SSList.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].WRTNO.To<string>();
                        SSList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME;
                        SSList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].JEPDATE.To<string>();
                        SSList.ActiveSheet.Cells[nRow - 1, 3].Text = hb.READ_GjJong_Name(list[i].GJJONG); 
                        SSList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].GJJONG;

                        //일특 체크
                        if (strJong == "11" || strJong == "16" || strJong == "41" || strJong == "44")
                        {
                            if (list[i].UCODES != "")
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

                        if (strTEMP == clsType.User.IdNumber)
                        {
                            SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                            SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0FF")); 
                        }
                    }
                }

                if (rdoJob1.Checked == true)
                {
                    if (SSList.ActiveSheet.RowCount > 0)
                    {
                        SSList.ActiveSheet.Cells[0, 0, 0, SSList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                    }
                }

                //상담인원 및 대기인원 DISPLAY
                HIC_SANGDAM_NEW_JEPSU_EXJONG list2 = hicSangdamNewJepsuExjongService.GetCntCnt2(clsHcVariable.GnHicLicense);

                lblCounter.Text = "총 대기인원: ";
                lblCounter.Text += list2.CNT2.To<string>() + " 명 / ";
                lblCounter.Text += "상담인원 : ";
                lblCounter.Text += list2.CNT.To<string>() + " 명 / ";
            }
            else if (sender == btnWards)
            {
                FrmHcSchoolCommonInput = new frmHcSchoolCommonInput("7");
                FrmHcSchoolCommonInput.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSchoolCommonInput.ssPanjengDblClick += new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick);
                FrmHcSchoolCommonInput.ShowDialog(this);
                FrmHcSchoolCommonInput.ssPanjengDblClick -= new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick);
            }
            else if (sender == btnMenuCancel)
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

        void frmHcSchoolCommonInput_ssPanjengDblClick(string argGubun, string strCommon)
        {
            txtRemark.Text += strCommon;
        }

        void fn_Screen_Display()
        {
            int nRead = 0;
            int nRow = 0;
            int nCnt = 0;

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
            string strTEMP = "";
            string strFlag = "";
            string strGbSTS = "";
            string strGjJong = "";

            btnPACS.Enabled = true;
            btnMed.Enabled = true;
            pnlDetail.Enabled = true;

            nCnt = 0;   //상담은 있는데 판정테이블이 없는것 체크

            FnWRTNO = txtWrtNo.Text.To<long>();
            if (FnWRTNO == 0) return;

            //삭제된것 체크
            if (hb.READ_JepsuSTS(FnWRTNO) == "D")
            {
                MessageBox.Show("접수번호 : " + FnWRTNO + " 은(는) 삭제된것 입니다. 확인하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //상담여부 체크
            strGjJong = hb.READ_GJJONG_CODE(txtWrtNo.Text.To<long>());
            if (hb.READ_SangDam_Gubun(strGjJong) == "D")
            {
                MessageBox.Show("접수번호 : " + FnWRTNO + " 은(는) 상담이 없는 검진종류 입니다... 확인하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            fn_Update_Patient_GbCall();

            //생애검진자 체크
            if (VB.Left(hicJepsuService.GetGjJongbyWrtNo(FnWRTNO), 1) == "4")
            {
                MessageBox.Show("생애검진 대상자입니다. 전산처리불가", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //상담내역이 있는지 점검(상담)
            HIC_SANGDAM_NEW list = hicSangdamNewService.GetGbStsRowIdbyWrtNo(FnWRTNO);
            FstrROWID = "";
            strGbSTS = "";
            if (!list.IsNullOrEmpty())
            {
                FstrROWID = list.RID;
                if (FstrROWID != "")
                {
                    nCnt += 1;
                }
                strGbSTS = list.GBSTS;
            }

            //상담내역이 있는지 점검(판정)
            FstrROWID2 = hicSchoolNewService.GetRowIdbyWrtNo(FnWRTNO);
            if (FstrROWID != "")
            {
                nCnt += 1;
            }

            #region Screen_Injek_display
            //Screen_Injek_display  //인적사항을 Display
            HIC_JEPSU_PATIENT list2 = hicJepsuPatientService.GetItembyWrtNo(FnWRTNO);

            if (list2.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 : " + FnWRTNO + " 번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //검진자 기본정보 표시---------------
            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list2.PTNO;
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list2.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list2.AGE + "/" + list2.SEX;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list2.LTDCODE.To<string>());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list2.JEPDATE.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list2.GJJONG);

            FstrChasu = list2.GJCHASU;
            FstrPtno = list2.PTNO;
            FnPano = list2.PANO;
            FnAge = list2.AGE;
            FstrSex = list2.SEX;
            FstrJepDate = list2.JEPDATE.To<string>();
            FstrJumin = clsAES.DeAES(list2.JUMIN2);
            FstrYear = list2.GJYEAR; 

            //상담테이블 없을 시 상담테이블 생성함
            if (FstrROWID == "")
            {
                if (fn_HIC_NEW_SANGDAM_INSERT(FnWRTNO, "56") != "")
                {
                    MessageBox.Show("접수번호 : " + FnWRTNO + " 은(는) 신규상담항목 자동발생시 오류가 발생함..전산실에 연락바람..", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            //판정테이블 없을 시 판정테이블 생성함
            if (FstrROWID2 == "")
            {
                if (fn_HIC_NEW_SCHOOL_INSERT(FnWRTNO, "56") != "")
                {
                    MessageBox.Show("접수번호 : " + FnWRTNO + " 은(는) 신규판정테이블 자동발생시 오류가 발생함..전산실에 연락바람..", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            #endregion

            #region Screen_Exam_Items_display
            //Screen_Exam_Items_display  '검사항목을 Display
            sp.Spread_All_Clear(SS2);

            List<HIC_RESULT_EXCODE> list3 = hicResultExCodeService.GetItemCounselbyWrtNo(FnWRTNO);

            nRead = list3.Count;
            SS2.ActiveSheet.RowCount = nRead;
            nRow = 0;
            for (int i = 0; i < nRead; i++)
            {
                strExCode = list3[i].EXCODE;
                strResult = list3[i].RESULT;
                strResCode = list3[i].RESCODE;
                strResultType = list3[i].RESULTTYPE;
                strGbCodeUse = list3[i].GBCODEUSE;

                //수가코드는 DISPLAY 에서 제외
                List<HIC_EXCODE> list4 = hicExcodeService.GetCodebyPart("9");

                for (int j = 0; j < list4.Count; j++)
                {
                    if (list4[j].CODE == strExCode)
                    {
                        SS2_Sheet1.Rows[i].Visible = false;
                    }
                }

                SS2.ActiveSheet.Cells[i, 0].Text = list3[i].EXCODE;
                SS2.ActiveSheet.Cells[i, 1].Text = list3[i].HNAME;
                SS2.ActiveSheet.Cells[i, 2].Text = strResult;
                //A103(비만도)는 자동계산(입력금지)
                FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();
                SS2.ActiveSheet.Cells[i, 2].CellType = txt;
                SS2.ActiveSheet.Cells[i, 2].HorizontalAlignment = CellHorizontalAlignment.Center;
                SS2.ActiveSheet.Cells[i, 2].VerticalAlignment = CellVerticalAlignment.Center;

                SS2.ActiveSheet.Cells[i, 3].CellType = txt;
                SS2.ActiveSheet.Cells[i, 3].HorizontalAlignment = CellHorizontalAlignment.Right;
                SS2.ActiveSheet.Cells[i, 3].VerticalAlignment = CellVerticalAlignment.Center;
                SS2.ActiveSheet.Cells[i, 3].Text = "";

                //비만도는 자동 계산함.
                if (strExCode == "A103")
                {
                    //if (string.Compare(FstrYear, "2005") >= 0 && string.Compare(FstrYear, "2007") <= 0)
                    //{
                    //    strResCode = "061";
                    //}
                    //else if (string.Compare(FstrYear, "2008") >= 0)
                    //{
                    //    strResCode = "065";
                    //}
                    strResCode = "065";
                }

                //자동계산은 선택못함.
                switch (strExCode)
                {
                    case "A103":
                    case "TH91":
                    case "TH90":
                    case "A117":
                    case "A116":
                        SS2.ActiveSheet.Cells[i, 2].Locked = true;
                        break;
                    default:
                        break;
                }

                if (strGbCodeUse == "Y")
                {
                    if (strResult != "")
                    {
                        SS2.ActiveSheet.Cells[i, 4].Text = hb.READ_ResultName(strResCode, strResult);
                    }
                }

                //참고치를 Dispaly
                strNomal = hm.EXAM_NomalValue_SET(strExCode, FstrJepDate, FstrSex, list3[i].MIN_M, list3[i].MAX_M, list3[i].MIN_F, list3[i].MAX_F);
                if (FstrSex == "M")
                {
                    strNomal = list3[i].MIN_M + "~" + list3[i].MAX_M;
                }
                else
                {
                    strNomal = list3[i].MIN_F + "~" + list3[i].MAX_F;
                }
                if (strNomal == "~")
                {
                    strNomal = "";
                }
                SS2.ActiveSheet.Cells[i, 6].Text = strNomal;
                SS2.ActiveSheet.Cells[i, 7].Text = strResCode;
                if (list3[i].EXCODE.Trim() == "A151")
                {
                    SS2.ActiveSheet.Cells[i, 7].Text = "007";
                }

                if (list3[i].EXCODE.Trim() == "TH01" || list3[i].EXCODE.Trim() == "TH02")
                {
                    SS2.ActiveSheet.Cells[i, 7].Text = "022";
                }
                SS2.ActiveSheet.Cells[i, 8].Text = "";
                SS2.ActiveSheet.Cells[i, 9].Text = list3[i].RID;
                SS2.ActiveSheet.Cells[i, 10].Text = list3[i].RESULTTYPE;
                strExPan = list3[i].PANJENG;
                SS2.ActiveSheet.Cells[i, 5].Text = strExPan;
                //판정결과별 바탕색상을 다르게 표시함
                if (strResult != "")
                {
                    switch (strExPan)
                    {
                        case "B":
                            SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);  //정상B
                            break;
                        case "C":
                            SS2.ActiveSheet.Cells[i, 2].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));   //주의C
                            break;
                        case "R":
                            SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 170, 170);  //질환의심(R)
                            break;
                        default:
                            SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(190, 250, 220);  //정상A 또는 기타
                            break;
                    }
                }
            }
            #endregion

            if (strGbSTS == "Y")    //수정
            {
                rdoJob1.Checked = true;
                fn_Screen_SangDam_display(FnWRTNO);
            }

            nDrSabun = hicJepsuService.GetSangdamDrNobyWrtNo(FnWRTNO);

            if (nDrSabun != 0)
            {
                txtPanDrNo.Text = hicDoctorService.GetLicensebySabun(nDrSabun);
            }
            else
            {
                txtPanDrNo.Text = "";
            }

            if (clsHcVariable.GnHicLicense == 0)
            {
                btnSave.Enabled = false;
            }
            else
            {
                if (txtPanDrNo.Text.To<long>() != 0)
                {
                    if (txtPanDrNo.Text.To<long>() != clsHcVariable.GnHicLicense)
                    {
                        btnSave.Enabled = false;
                    }
                }
                else
                {
                    txtPanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
                }
            }
            lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());

            fn_Genjin_History_SET();    //검진 HISTORY

            //문진뷰어
            if (chkMunjin.Checked == false)
            {
                //====인터넷 문진Data 조회====
                if (hicIeMunjinNewService.GetwebHtmlbyWrtNo(FnWRTNO) > 0)
                {
                    clsHcVariable.GstrJepDate = DateTime.Parse(FstrJepDate).AddDays(-60).ToShortDateString();
                    clsHcVariable.GnWRTNO = FnWRTNO;

                    FrmHcPanInternetMunjin_New = new frmHcPanInternetMunjin_New(0, "", FstrPtno);
                    FrmHcPanInternetMunjin_New.ShowDialog(this);
                }
                else
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
                    FrmHcSangInternetMunjinView = new frmHcSangInternetMunjinView(FnWRTNO, FstrJepDate, FstrPtno, FstrGjJong, FstrROWID);
                    FrmHcSangInternetMunjinView.Show();
                }
            }

            if (nCnt == 1)
            {
                eBtnClick(btnSave, new EventArgs());
                MessageBox.Show("자동으로 작업을 처리하였습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void fn_Screen_SangDam_display(long argWrtNo)
        {

            List<HIC_SCHOOL_NEW> list = hicSchoolNewService.GetItembyWrtNo(FnWRTNO);

            if (!list.IsNullOrEmpty())
            {
                cboPRes.SelectedIndex = list[0].PPANB1.To<int>();
                txtPResEtc.Text = list[0].PPANB2;
                cboEJ0.SelectedIndex = VB.Pstr(list[0].PPANC4, "^^", 2).To<int>();
                cboEJ1.SelectedIndex = VB.Pstr(list[0].PPANC4, "^^", 1).To<int>();
                txtEJEtc.Text = VB.Pstr(list[0].PPANC4, "^^", 3);
                cboM.SelectedIndex = VB.Pstr(list[0].PPANC7, "^^", 1).To<int>();
                txtMEtc.Text = VB.Pstr(list[0].PPANC7, "^^", 2);
                cboN.SelectedIndex = VB.Pstr(list[0].PPANC8, "^^", 1).To<int>();
                txtNEtc.Text = VB.Pstr(list[0].PPANC8, "^^", 2);
                cboS.SelectedIndex = VB.Pstr(list[0].PPANC9, "^^", 1).To<int>();
                txtSEtc.Text = VB.Pstr(list[0].PPANC9, "^^", 2);
                cboHJ.SelectedIndex = VB.Pstr(list[0].PPANC6, "^^", 1).To<int>();
                txtHJEtc.Text = VB.Pstr(list[0].PPANC6, "^^", 2);

                //기관능력
                cboOrgan0.SelectedIndex = VB.Pstr(list[0].PPAND1, "^^", 1).To<int>();
                txtOrgan0Etc.Text = VB.Pstr(list[0].PPAND1, "^^", 2);
                cboOrgan1.SelectedIndex = VB.Pstr(list[0].PPAND2, "^^", 1).To<int>();
                txtOrgan1Etc.Text = VB.Pstr(list[0].PPAND2, "^^", 2);
                cboOrgan2.SelectedIndex = VB.Pstr(list[0].PPAND3, "^^", 1).To<int>();
                txtOrgan2Etc.Text = VB.Pstr(list[0].PPAND3, "^^", 2);
                cboOrgan3.SelectedIndex = VB.Pstr(list[0].PPAND4, "^^", 1).To<int>();
                txtOrgan3Etc.Text = VB.Pstr(list[0].PPAND4, "^^", 2);
                cboOrgan4.SelectedIndex = VB.Pstr(list[0].PPAND5, "^^", 1).To<int>();
                txtOrgan4Etc.Text = VB.Pstr(list[0].PPAND5, "^^", 2);
                cboOrgan5.SelectedIndex = VB.Pstr(list[0].PPAND6, "^^", 1).To<int>();
                txtOrgan5Etc.Text = VB.Pstr(list[0].PPAND6, "^^", 2);

                //진촬및상담
                cboJ0.SelectedIndex = VB.Pstr(list[0].PPANK1, "^^", 1).To<int>();
                txtJEtc0.Text = VB.Pstr(list[0].PPANK1, "^^", 2);
                cboJ1.SelectedIndex = VB.Pstr(list[0].PPANK2, "^^", 1).To<int>();
                txtJEtc1.Text = VB.Pstr(list[0].PPANK2, "^^", 2);
                cboJ2.SelectedIndex = VB.Pstr(list[0].PPANK3, "^^", 1).To<int>();
                txtJEtc2.Text = VB.Pstr(list[0].PPANK3, "^^", 2);
                cboJ3.SelectedIndex = VB.Pstr(list[0].PPANK4, "^^", 1).To<int>();
                txtJEtc3.Text = VB.Pstr(list[0].PPANK4, "^^", 2);
                txtRemark.Text = list[0].SANGDAM;
            }
        }

        //void Genjin_Histroy_SET()
        //{
        //    int nRead = 0;
        //    string strData = "";
        //    string strJong = "";
        //    long nHeaPano = 0;

        //    //종검의 등록번호를 찾음
        //    nHeaPano = 0;
        //    nHeaPano = hicPatientService.GetPanobyJumin2(clsAES.AES(FstrJumin)).To<long>();

        //    //일반건진, 종합검진의 접수내역을 Display
        //    List<HIC_JEPSU> list = hicJepsuService.GetItembyOnlyPaNo(FnPano, nHeaPano);

        //    nRead = list.Count;
        //    SSHistory.ActiveSheet.RowCount = nRead;
        //    for (int i = 0; i < nRead; i++)
        //    {
        //        strJong = list[i].GJJONG;
        //        SSHistory.ActiveSheet.Cells[i, 0].Text = list[i].JEPDATE.To<string>();
        //        if (strJong == "XX")
        //        {
        //            SSHistory.ActiveSheet.Cells[i, 1].Text = "종검";
        //        }
        //        else
        //        {
        //            SSHistory.ActiveSheet.Cells[i, 1].Text = hb.READ_GjJong_Name(strJong);
        //        }
        //        SSHistory.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.To<string>();
        //        SSHistory.ActiveSheet.Cells[i, 3].Text = list[i].GJCHASU;
        //    }
        //}

        void fn_Screen_Clear()
        {
            btnPACS.Enabled = false;
            btnMed.Enabled = false;
            pnlDetail.Enabled = false;

            txtLtdCode.Text = "";
            txtWrtNo.Text = "";
            txtSName.Text = "";
            sp.Spread_All_Clear(SSHistory);
            sp.Spread_All_Clear(ssPatInfo);
            ssPatInfo.ActiveSheet.RowCount = 1;

            cboPRes.SelectedIndex = 1;
            txtPResEtc.Text = "";
            cboEJ0.SelectedIndex = 1;
            cboEJ1.SelectedIndex = 1;
            txtEJEtc.Text = "";
            cboM.SelectedIndex = 1;
            txtMEtc.Text = "";
            cboN.SelectedIndex = 1;
            txtNEtc.Text = "";
            cboS.SelectedIndex = 1;
            txtSEtc.Text = "";
            cboHJ.SelectedIndex = 1;
            txtHJEtc.Text = "";

            for (int i = 0; i <= 3; i++)
            {
                ComboBox cboJ = (Controls.Find("cboJ" + i.To<string>(), true)[0] as ComboBox);
                TextBox txtJEtc = (Controls.Find("txtJEtc" + i.To<string>(), true)[0] as TextBox);
                cboJ.SelectedIndex = 1;
                txtJEtc.Text = "";
            }

            txtRemark.Text = "";
            txtPanDrNo.Text = "";
            lblDrName.Text = "";
            btnSave.Enabled = true;

            for (int i = 0; i <= 5; i++)
            {
                ComboBox cboOrgan = (Controls.Find("cboOrgan" + i.To<string>(), true)[0] as ComboBox);
                cboOrgan.SelectedIndex = 1;
            }

            txtOrgan0Etc.Text = "";
            txtOrgan1Etc.Text = "";
            txtOrgan2Etc.Text = "";
            txtOrgan3Etc.Text = "";
            txtOrgan4Etc.Text = "";
            txtOrgan5Etc.Text = "";
        }

        void fn_Update_Patient_GbCall()
        {
            List<string> strWRTNO = new List<string>();
            string strSysDateTime = "";
            int result = 0;
            
            ComFunc.ReadSysDate(clsDB.DbCon);

            strSysDateTime = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;

            clsDB.setBeginTran(clsDB.DbCon);

            strWRTNO.Clear();

            //호출은 했으나 상담이 완료안된 접수번호 찾음
            List<HIC_SANGDAM_WAIT> list = hicSangdamWaitService.GetWrtNobyGubunNotWrtNo(clsHcVariable.GstrDrRoom, txtWrtNo.Text.To<long>());

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strWRTNO.Add(list[i].WRTNO.To<string>());
                    //strWRTNO += list[i].WRTNO.To<string>() + " ";
                }               

                if (strWRTNO != null)
                {
                    result = hicSangdamWaitService.UpdateCallTimeDisplaybyWrtNo(strWRTNO, "NOT");

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("미상담 갱신 중 오류발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //2020-09-09
                //result = hicSangdamWaitService.UpdateCallTimebyWrtNoGubun(txtWrtNo.Text.To<long>(), clsHcVariable.GstrDrRoom, strSysDateTime);
                result = hicSangdamWaitService.UpdateCallTimebyWrtNoGubun(txtWrtNo.Text.To<long>(), clsHcVariable.GstrDrRoom);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("미상담 갱신 중 오류발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        string fn_HIC_NEW_SCHOOL_INSERT(long argWrtNo, string argJong)
        {
            string rtnVal = "";
            string strROWID = "";
            int result = 0;

            strROWID = hicSchoolNewService.GetRowIdbyWrtNo(FnWRTNO);
            if (strROWID == "")
            {
                clsDB.setBeginTran(clsDB.DbCon);

                result = hicSchoolNewService.InsertWrtNo(argWrtNo);

                if (result < 0)
                {
                    rtnVal = "암문진생성시 오류";
                    return rtnVal;
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);

            return rtnVal;
        }

        void fn_Genjin_History_SET()
        {
            int nRead = 0;
            string strData = "";
            string strJong = "";
            long nHeaPano = 0;

            //종검의 등록번호를 찾음
            nHeaPano = 0;
            nHeaPano = hicPatientService.GetPanobyJumin2(clsAES.AES(FstrJumin)).To<long>();

            //일반건진, 종합검진의 접수내역을 Display
            List<HIC_JEPSU> list = hicJepsuService.GetItembyUnionPaNo(FnPano);

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
    }
}
