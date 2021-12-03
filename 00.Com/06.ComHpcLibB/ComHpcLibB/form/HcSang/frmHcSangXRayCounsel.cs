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
using System.Diagnostics;

/// <summary>
/// Class Name      : ComHpcLibB\HC_Sang
/// File Name       : frmHcSangXRayCounsel.cs
/// Description     : 방사선종사자 상담 프로그램
/// Author          : 이상훈
/// Create Date     : 2020-02-05
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHcSang_방사선.frm(FrmAct04)" />

namespace ComHpcLibB
{
    public partial class frmHcSangXRayCounsel : Form
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
        HicXMunjinService hicXMunjinService = null;

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
        frmHcSangStudentCounsel FrmHcSangStudentCounsel = null;
        frmHcSangInternetMunjinView FrmHcSangInternetMunjinView = null;

        long FnWRTNO;
        long FnWrtno2;  //2차 검진시 이전 1차 접수번호
        long FnPano;
        long FnAge;
        int FnTab;
        string FstrPtno;
        string FstrSex;
        string FstrJepDate;
        string FstrJumin;
        string FstrMunjin;
        string FstrChasu;
        string FstrUCodes;
        string FstrGjJong;
        long FnRowNo;       //메모리타자기 위치 저장용
        long FnClickRow;    //Help를 Click한 Row
        string FstrYear;
        string FstrExamFlag;
        string FstrROWID;
        long FnHeaWRTNO;    // 종합검진 접수번호

        public frmHcSangXRayCounsel(long nWrtNo)
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
            hicXMunjinService = new HicXMunjinService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnMenuCancel.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnMenuWait.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.Click += new EventHandler(eTxtClick);
            this.rdoJob1.Click += new EventHandler(eRdoClick);
            this.rdoJob2.Click += new EventHandler(eRdoClick);
            this.SSHistory.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSList1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSList2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSList3.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSList4.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSList5.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.tab1.Click += new EventHandler(eTabClick);
            this.tab2.Click += new EventHandler(eTabClick);
            this.tab3.Click += new EventHandler(eTabClick);
            this.tab4.Click += new EventHandler(eTabClick);
            this.tab5.Click += new EventHandler(eTabClick);            
            this.txtPanDrNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtPanDrNo.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtRemark1.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtRemark1.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtEtc.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtEye.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtWrtNo.Click += new EventHandler(eTxtClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strData = "";            

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;

            //판정의사 여부를 읽음
            hb.READ_HIC_Doctor(long.Parse(clsType.User.IdNumber));

            this.Text += "(" + clsHcVariable.GstrHicDrName + ") (면허:" + clsHcVariable.GnHicLicense + ")";

            fn_Screen_Clear();

            for (int i = 1; i <= 4; i++)
            {
                FpSpread SSList = (Controls.Find("SSList" + i.ToString(), true)[0] as FpSpread);
                SSList.ActiveSheet.Columns[4].Visible = false;
            }
            
            SS2_Sheet1.Columns[7].Visible = false;  //결과값코드
            SS2_Sheet1.Columns[8].Visible = false;  //변경
            SS2_Sheet1.Columns[9].Visible = false;  //ROWID
            SS2_Sheet1.Columns[11].Visible = false; //Result Type

            eBtnClick(btnSearch, new EventArgs());

            if (clsHcVariable.GnHicLicense == 0)
            {
                btnSearch.Enabled = false;
            }

        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtRemark1 || sender == txtSName)
            {
                txtRemark1.ImeMode = ImeMode.Hangul;
            }
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txtRemark1 || sender == txtSName)
            {
                txtRemark1.ImeMode = ImeMode.Hangul;
            }
            else if (sender == txtWrtNo)
            {
                string strJong = "";
                string strWrtNo = "";

                if (txtWrtNo.Text.Trim() == "")
                {
                    return;
                }

                strJong = hb.READ_GJJONG_CODE(long.Parse(txtWrtNo.Text));
                if (strJong == "56")
                {
                    strWrtNo = txtWrtNo.Text.Trim();
                    txtWrtNo.Text = "";
                    this.Hide();
                    FrmHcSangStudentCounsel = new frmHcSangStudentCounsel(long.Parse(strWrtNo));
                    FrmHcSangStudentCounsel.Show(this);
                    SendKeys.Send("{Tab}");
                    return;
                }
                else if (strJong != "50" && strJong != "51" && strJong != "56")
                {
                    strWrtNo = txtWrtNo.Text.Trim();
                    txtWrtNo.Text = "";
                    this.Hide();
                    //FrmHcSangGeneralCounsel = new frmHcSangGeneralCounsel(long.Parse(strWrtNo));
                    //FrmHcSangGeneralCounsel.Show(this);
                    SendKeys.Send("{Tab}");
                    return;
                }
                strWrtNo = txtWrtNo.Text.Trim();

                fn_Screen_Clear();

                txtWrtNo.Text = strWrtNo;
                SendKeys.Send("{Tab}");
                fn_Screen_Display();
            }
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == txtSkin || sender == txtEye)
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:   //115
                        txtEtc.Text = "정상";
                        break;
                    default:
                        break;
                }
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
                //frmHcPanInternetMunjin_New frm = new frmHcPanInternetMunjin_New(0, "", "");

                //if (FormIsExist(frm.GetType()))
                //{
                //    frm.Dispose();
                //    frm.Close();
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
                if (hicSangdamWaitService.GetCountbyWrtNoGubun(clsHcVariable.GstrDrRoom, long.Parse(txtWrtNo.Text)) > 0)
                {
                    result = hicSangdamWaitService.UpdateCallTimeDisplaybyOnlyWrtNo(long.Parse(txtWrtNo.Text));

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
            else if (sender == btnSave)
            {
                string strYN = "";
                string strGBn = "";

                //DATA_ERROR_CHECK
                if (long.Parse(txtPanDrNo.Text) == 0)
                {
                    MessageBox.Show("판정의사 면허번호 누락", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (rdoX0.Checked == true)
                {
                    strYN = "Y";
                }
                else
                {
                    strYN = "N";
                }

                if (rdoGubun0.Checked == true)
                {
                    strGBn = "Y";
                }
                else
                {
                    strGBn = "N";
                }

                if (txtRemark1.Text.Trim() == "")
                {
                    txtRemark1.Text = "특이사항 없음";
                }

                clsDB.setBeginTran(clsDB.DbCon);

                HIC_X_MUNJIN item = new HIC_X_MUNJIN();

                item.JINGBN = strGBn;
                item.XP1 = strYN;
                item.XPJONG = txtXJong.Text.Trim();
                item.XPLACE = txtPlace.Text.Trim();
                item.XREMARK = txtRemark.Text.Trim();
                item.XMUCH = txtMuch.Text.Trim();
                item.XTERM = txtTerm.Text.Trim();
                item.XTERM1 = txtXTerm.Text.Trim();
                item.XJUNGSAN = txtJung.Text.Trim();
                item.MUN1 = txtMun1.Text.Trim();
                item.JUNGSAN1 = txtEye.Text.Trim();
                item.JUNGSAN2 = txtSkin.Text.Trim();
                item.JUNGSAN3 = txtEtc.Text.Trim();
                item.SANGDAM = txtRemark1.Text.Trim();
                item.MUNDRNO = long.Parse(txtPanDrNo.Text.Trim());
                item.WRTNO = FnWRTNO;

                result = hicXMunjinService.Update(item);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("자료를 등록중 오류가 발생함!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //상담완료 Flag (Jepsu)
                result = hicJepsuService.UpdateGbJinChalSangdamdrnobyWrtNo(clsType.User.IdNumber, FnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담Flag 저장시 오류 발생(HIC_JEPSU)!!!", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //상담완료 Flag (Acting)
                result = hicResultService.UpdateEntSabunbyWrtNo(long.Parse(clsType.User.IdNumber), FnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담Flag 저장시 오류 발생(HIC_RESULT)!!!", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
                strSName = txtSName.Text.Trim();

                FpSpread SSList = (Controls.Find("SSList" + ii.ToString(), true)[0] as FpSpread);
                sp.Spread_All_Clear(SSList);

                //결과 테이블에 자료 INSERT
                fn_HIC_X_MUNJIN_INSERT("50");
                fn_HIC_X_MUNJIN_INSERT("51");

                List<HIC_JEPSU_SANGDAM_NEW_EXJONG> list = hicJepsuSangdamNewExjongService.GetItembyJepDate(strFrDate, strToDate, nGBn, FnTab, strSName, nLtdCode, strGuBun);

                nRead = list.Count;
                SSList.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    strOK = "OK";
                    strTEMP = list[i].SANGDAMDRNO.ToString();
                    if (rdoJob1.Checked == true)    //대기자
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
                FrmHcSchoolCommonInput = new frmHcSchoolCommonInput("7");
                FrmHcSchoolCommonInput.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSchoolCommonInput.ssPanjengDblClick += new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick);
                FrmHcSchoolCommonInput.ShowDialog(this);
                FrmHcSchoolCommonInput.ssPanjengDblClick -= new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick);
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

        void eRdoClick(object sender, EventArgs e)
        {
            eBtnClick(btnSearch, new EventArgs());
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

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true)
            {
                return;
            }

            fn_Screen_Clear();

            txtWrtNo.Focus();

            if (sender == SSList1)
            {
                txtWrtNo.Text = SSList1.ActiveSheet.Cells[e.Row, 0].Text.Trim();
            }
            else if (sender == SSList2)
            {
                txtWrtNo.Text = SSList2.ActiveSheet.Cells[e.Row, 0].Text.Trim();
            }
            else if (sender == SSList3)
            {
                txtWrtNo.Text = SSList3.ActiveSheet.Cells[e.Row, 0].Text.Trim();
            }
            else if (sender == SSList4)
            {
                txtWrtNo.Text = SSList4.ActiveSheet.Cells[e.Row, 0].Text.Trim();
            }
            else if (sender == SSList5)
            {
                txtWrtNo.Text = SSList5.ActiveSheet.Cells[e.Row, 0].Text.Trim();
            }
            SendKeys.Send("{TAB}");
        }

        void eTabClick(object sender, EventArgs e)
        {
            eBtnClick(btnSearch, new EventArgs());
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtPanDrNo)
            {
                lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
                SendKeys.Send("{TAB}");
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
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
            long nDrNO = 0;
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

            nCnt = 0;   //상담은 있는데 판정테이블이 없는것 체크

            FnWRTNO = long.Parse(txtWrtNo.Text);
            if (FnWRTNO == 0) return;

            //삭제된것 체크
            if (hb.READ_JepsuSTS(FnWRTNO) == "D")
            {
                MessageBox.Show("접수번호 : " + FnWRTNO + " 은(는) 삭제된것 입니다. 확인하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //상담여부 체크
            strGjJong = hb.READ_GJJONG_CODE(long.Parse(txtWrtNo.Text));
            if (hb.READ_SangDam_Gubun(strGjJong) != "Y")
            {
                MessageBox.Show("접수번호 : " + FnWRTNO + " 은(는) 상담이 없는 검진종류 입니다... 확인하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            fn_HIC_X_MUNJIN_INSERT(strGjJong);
            fn_Update_Patient_GbCall();

            //생애검진자 체크
            if (VB.Left(hicJepsuService.GetGjJongbyWrtNo(FnWRTNO), 1) == "4")
            {
                MessageBox.Show("생애검진 대상자입니다. 전산처리불가", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //상담내역이 있는지 점검
            HIC_X_MUNJIN list = hicXMunjinService.GetMunDrNobyWrtNo(FnWRTNO);
            FstrROWID = "";
            nDrNO = 0;
            if (!list.IsNullOrEmpty())
            {
                FstrROWID = list.RID.Trim();
                if (FstrROWID != "")
                {
                    nCnt += 1;
                }
                nDrNO = list.MUNDRNO;
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
            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list2.PTNO.Trim();
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list2.SNAME.Trim();
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list2.AGE + "/" + list2.SEX.Trim();
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list2.LTDCODE.ToString());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list2.JEPDATE.ToString();
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list2.GJJONG.Trim());

            FstrChasu = list2.GJCHASU.Trim();
            FstrPtno = list2.PTNO.Trim();
            FnPano = list2.PANO;
            FnAge = list2.AGE;
            FstrSex = list2.SEX.Trim();
            FstrJepDate = list2.JEPDATE.ToString();
            FstrJumin = clsAES.DeAES(list2.JUMIN2.Trim());
            FstrYear = list2.GJYEAR.Trim();            
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
                strExCode = list3[i].EXCODE.Trim();
                strResult = list3[i].RESULT.Trim();
                strResCode = list3[i].RESCODE.Trim();
                strResultType = list3[i].RESULTTYPE.Trim();
                strGbCodeUse = list3[i].GBCODEUSE.Trim();

                //수가코드는 DISPLAY 에서 제외
                List<HIC_EXCODE> list4 = hicExcodeService.GetCodebyPart("9");

                for (int j = 0; j < list4.Count; j++)
                {
                    if (list4[j].CODE.Trim() == strExCode)
                    {
                        SS2_Sheet1.Rows[i].Visible = false;
                    }
                }

                SS2.ActiveSheet.Cells[i, 0].Text = list3[i].EXCODE.Trim();
                SS2.ActiveSheet.Cells[i, 0].Text = list3[i].HNAME.Trim();
                SS2.ActiveSheet.Cells[i, 0].Text = strResult;
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
                SS2.ActiveSheet.Cells[i, 9].Text = list3[i].RID.Trim();
                SS2.ActiveSheet.Cells[i, 10].Text = list3[i].RESULTTYPE.Trim();
                strExPan = list3[i].PANJENG.Trim();
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

            if (nDrNO > 0)    //수정
            {
                rdoJob1.Checked = true;
                fn_Screen_Munjin_Display(FnWRTNO);
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
                if (long.Parse(txtPanDrNo.Text) != 0)
                {
                    if (long.Parse(txtPanDrNo.Text) != clsHcVariable.GnHicLicense)
                    {
                        btnSave.Enabled = false;
                    }
                }
                else
                {
                    txtPanDrNo.Text = clsHcVariable.GnHicLicense.ToString();
                }
            }
            lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
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
                    if (hf.OpenForm_Check("frmHcSangInternetMunjinView") == true)
                    {
                        return;
                    }
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

        //건강검진 문진표 및 결과를  READ
        void fn_Screen_Munjin_Display(long argWrtNo)
        {
            HIC_X_MUNJIN list = hicXMunjinService.GetItembyWrtNo(argWrtNo);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 " + argWrtNo + "는 결과 및 판정이 등록 않됨", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //문진
            if (list.XP1.Trim() == "Y")
            {
                rdoX0.Checked = true;
            }
            else if (list.XP1.Trim() == "N")
            {
                rdoX2.Checked = true;
            }

            if (list.JINGBN.Trim() == "Y")
            {
                rdoGubun0.Checked = true;
            }
            else if (list.JINGBN.Trim() == "N")
            {
                rdoGubun1.Checked = true;
            }

            txtXJong.Text = list.XPJONG.Trim();
            txtRemark.Text = list.XPLACE.Trim();
            txtPlace.Text = list.XREMARK.Trim();
            txtTerm.Text = list.XTERM.Trim();
            txtXTerm.Text = list.XTERM1.Trim();
            txtMuch.Text = list.XMUCH.Trim();
            txtJung.Text = list.XJUNGSAN.Trim();
            txtMun1.Text = list.MUN1.Trim();
            txtEye.Text = list.JUNGSAN1.Trim();
            txtSkin.Text = list.JUNGSAN2.Trim();
            txtEtc.Text = list.JUNGSAN3.Trim();
            txtRemark1.Text = list.SANGDAM.Trim();
            txtPanDrNo.Text = list.MUNDRNO.ToString();
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
            List<HIC_SANGDAM_WAIT> list = hicSangdamWaitService.GetWrtNobyGubunNotWrtNo(clsHcVariable.GstrDrRoom, long.Parse(txtWrtNo.Text));

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strWRTNO.Add(list[i].WRTNO.ToString());
                    //strWRTNO += list[i].WRTNO.ToString() + " ";
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
                //result = hicSangdamWaitService.UpdateCallTimebyWrtNoGubun(long.Parse(txtWrtNo.Text), clsHcVariable.GstrDrRoom, strSysDateTime);
                result = hicSangdamWaitService.UpdateCallTimebyWrtNoGubun(long.Parse(txtWrtNo.Text), clsHcVariable.GstrDrRoom);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("미상담 갱신 중 오류발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        void fn_HIC_X_MUNJIN_INSERT(string argJong)
        {
            int nRead = 0;
            long nWrtNo = 0;
            string strJepDate = "";
            long nLtdCode = 0;
            string strFrDate = "";
            string strToDate = "";
            string strSName = "";
            int result = 0;

            strFrDate = dtpFrDate.Text;
            strToDate = dtpToDate.Text;
            nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text.Trim(), ".", 1));
            strSName = txtSName.Text.Trim();

            List<HIC_JEPSU> list = hicJepsuService.GetJepDatebyJepDateGjJong(strFrDate, strToDate, nLtdCode, strSName, argJong);

            nRead = list.Count;
            if (nRead == 0) return;

            clsDB.setBeginTran(clsDB.DbCon);
            for (int i = 0; i < nRead; i++)
            {
                nWrtNo = list[i].WRTNO;
                strJepDate = list[i].JEPDATE;

                result = hicXMunjinService.SaveXMunjin(nWrtNo, strJepDate);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("자료를 등록중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        void fn_Screen_Clear()
        {
            txtLtdCode.Text = "";
            txtSName.Text = "";            
            txtWrtNo.Text = "";

            sp.Spread_All_Clear(SSHistory);
            sp.Spread_All_Clear(ssPatInfo);
            sp.Spread_All_Clear(SS2);
            SS2.ActiveSheet.RowCount = 50;

            btnPACS.Enabled = false;
            btnMed.Enabled = false;

            txtXTerm.Text = "";
            txtMun1.Text = "";
            txtXJong.Text = "";
            txtEye.Text = "";
            txtPlace.Text = "";
            txtSkin.Text = "";
            txtRemark.Text = "";
            txtEtc.Text = "";
            txtTerm.Text = "";
            txtMuch.Text = "";
            txtJung.Text = "";
            txtRemark1.Text = "";
            txtPanDrNo.Text = "";
            lblDrName.Text = "";
            btnSave.Enabled = true;
        }

        void fn_Genjin_History_SET()
        {
            int nRead = 0;
            string strData = "";
            string strJong = "";
            long nHeaPano = 0;

            //종검의 등록번호를 찾음
            nHeaPano = 0;
            nHeaPano = long.Parse(hicPatientService.GetPanobyJumin2(clsAES.AES(FstrJumin)));

            //일반건진, 종합검진의 접수내역을 Display
            List<HIC_JEPSU> list = hicJepsuService.GetItembyOnlyPaNo(FnPano, nHeaPano);

            nRead = list.Count;
            SSHistory.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                strJong = list[i].GJJONG.Trim();
                SSHistory.ActiveSheet.Cells[i, 0].Text = list[i].JEPDATE.ToString();
                if (strJong == "XX")
                {
                    SSHistory.ActiveSheet.Cells[i, 1].Text = "종검";
                }
                else
                {
                    SSHistory.ActiveSheet.Cells[i, 1].Text = hb.READ_GjJong_Name(strJong);
                }
                SSHistory.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.ToString();
                SSHistory.ActiveSheet.Cells[i, 3].Text = list[i].GJCHASU.Trim();
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
