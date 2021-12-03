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
/// Class Name      : HC_Sangdam
/// File Name       : frmHcSangStudentTeeth.cs
/// Description     : 학생 구강상담 프로그램
/// Author          : 이상훈
/// Create Date     : 2020-01-01
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHcSang_학생구강.frm(FrmAct01)" />

namespace HC_Sangdam
{
    public partial class frmHcSangStudentTeeth : Form
    {
        HicSchoolNewService hicSchoolNewService = null;
        HicJepsuService hicJepsuService = null;
        HicResultService hicResultService = null;
        HicJepsuSchoolNewSangdamWaitService hicJepsuSchoolNewSangdamWaitService = null;
        HicJepsuSchoolNewService hicJepsuSchoolNewService = null;
        HicSchoolNewJepsuService hicSchoolNewJepsuService = null;
        HicSangdamNewService hicSangdamNewService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicSangdamWaitService hicSangdamWaitService = null;
        HicWaitRoomService hicWaitRoomService = null;
        HicPatientService hicPatientService = null;
        HicExjongService hicExjongService = null;

        frmViewResult FrmViewResult = null;


        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        frmHcSchoolCommonInput FrmHcSchoolCommonInput = null;
        frmHeaResult FrmHeaResult = null;
        frmHcSangInternetMunjinView FrmHcSangInternetMunjinView = null;
        frmHcSangTeeth FrmHcSangTeeth = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        long FnWRTNO;
        long FnWrtno2;  //2차 검진시 이전 1차 접수번호
        long FnPano;
        long FnAge;

        string FstrPtno;
        string FstrSex;
        string FstrJepDate;
        string FstrJumin;
        string FstrMunjin;
        string FstrChasu;

        string FstrGjJong;
        long FnRowNo;   // 메모리타자기 위치 저장용
        long FnRow;

        string FstrYear;

        string FstrROWID;
        string FstrWaitRoom;

        long FnHeaWRTNO;    //종합검진 접수번호
        bool FbDentalOnly;  //초등학생 구강검진만 있는지 여부(True:구강검진만 있음)
        long FnWrtNo;

        public frmHcSangStudentTeeth()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcSangStudentTeeth(long nWrtNo)
        {
            InitializeComponent();
            FnWrtNo = nWrtNo;
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicSchoolNewService = new HicSchoolNewService();
            hicJepsuService = new HicJepsuService();
            hicResultService = new HicResultService();
            hicJepsuSchoolNewSangdamWaitService = new HicJepsuSchoolNewSangdamWaitService();
            hicJepsuSchoolNewService = new HicJepsuSchoolNewService();
            hicSchoolNewJepsuService = new HicSchoolNewJepsuService();
            hicSangdamNewService = new HicSangdamNewService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicSangdamWaitService = new HicSangdamWaitService();
            hicWaitRoomService = new HicWaitRoomService();
            hicPatientService = new HicPatientService();
            hicExjongService = new HicExjongService();

            this.Load += new EventHandler(eFormLoad);
            this.FormClosing += new FormClosingEventHandler(eFromClosing);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnMenuGen.Click += new EventHandler(eBtnClick);
            this.btnPACS.Click += new EventHandler(eBtnClick);
            this.btnMed.Click += new EventHandler(eBtnClick);
            this.btnSet3.Click += new EventHandler(eBtnClick);
            this.btnDntOk.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnMenuCancel.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            //this.SS1.Change += new ChangeEventHandler(eSpdChange);
            this.rdoJob1.Click += new EventHandler(eRdoClick);
            this.rdoJob2.Click += new EventHandler(eRdoClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtChijuETC.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDntPanCnt1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDntPanCnt2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDntPanDown1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDntPanDown2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDntPanDown3.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDntPanSogen.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtDntPanSogen.LostFocus += new EventHandler(eTxtLostFocus);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
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
            //    //FrmHcSangInternetMunjinView.Close();
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
            hb.READ_HIC_Doctor(clsType.User.IdNumber.To<long>());

            this.Text += "(" + clsHcVariable.GstrHicDrName + ") (면허:" + clsHcVariable.GnHicLicense + ")";

            fn_ComboPan_Set();
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

            SSList_Sheet1.Columns.Get(4).Visible = false;

            if (clsType.User.IdNumber == "35712")   //김영배
            {
                chkWait.Enabled = true;
                chkWait.Checked = true;
                FstrWaitRoom = "08";
            }
            else if (clsType.User.IdNumber == "37029")   //최영수
            {
                chkWait.Enabled = true;
                chkWait.Checked = true;
                FstrWaitRoom = "09";
            }
            else
            {
                chkWait.Enabled = false;
                chkWait.Checked = false;
                FstrWaitRoom = "";
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
        }

        void fn_ComboPan_Set()
        {
            //구내염 및 연조직질환
            cboDntPan1.Items.Clear();
            cboDntPan1.Items.Add(" ");
            cboDntPan1.Items.Add("1.없음"); 
            cboDntPan1.Items.Add("2.있음"); 
            cboDntPan1.SelectedIndex = 1;

            //부정교합
            cboDntPan2.Items.Clear();
            cboDntPan2.Items.Add(" ");
            cboDntPan2.Items.Add("1.없음");
            cboDntPan2.Items.Add("2.요교정");
            cboDntPan2.Items.Add("3.교정중");
            cboDntPan2.SelectedIndex = 1;

            //위생상태
            cboDntPan3.Items.Clear();
            cboDntPan3.Items.Add(" ");
            cboDntPan3.Items.Add("1.우수");
            cboDntPan3.Items.Add("2.보통");
            cboDntPan3.Items.Add("3.개선요망");
            cboDntPan3.SelectedIndex = 1;

            //그밖에상태
            cboDntPan4.Items.Clear();
            cboDntPan4.Items.Add(" ");
            cboDntPan4.Items.Add("1.과잉치");
            cboDntPan4.Items.Add("2.유치잔존");
            cboDntPan4.Items.Add("3.그밖의치아상태");
            cboDntPan4.SelectedIndex = 0;

            //악관절이상
            cboDntPan5.Items.Clear();
            cboDntPan5.Items.Add(" ");
            cboDntPan5.Items.Add("1.없음");
            cboDntPan5.Items.Add("2.있음");
            cboDntPan5.SelectedIndex = 1;

            //치아마모증
            cboDntPan6.Items.Clear();
            cboDntPan6.Items.Add(" ");
            cboDntPan6.Items.Add("1.없음");
            cboDntPan6.Items.Add("2.있음");
            cboDntPan6.SelectedIndex = 1;

            //제3대구치
            cboDntPan7.Items.Clear();
            cboDntPan7.Items.Add(" ");
            cboDntPan7.Items.Add("1.정상");
            cboDntPan7.Items.Add("2.이상");
            cboDntPan7.SelectedIndex = 0;

            //치아상태
            cboPanjeng1.Items.Clear();
            cboPanjeng1.Items.Add(" ");
            cboPanjeng1.Items.Add("1.정상");
            cboPanjeng1.Items.Add("2.정상경계");
            cboPanjeng1.Items.Add("3.정밀검사요함");
            cboPanjeng1.SelectedIndex = 0;

            //구강상태
            cboPanjeng2.Items.Clear();
            cboPanjeng2.Items.Add(" ");
            cboPanjeng2.Items.Add("1.정상");
            cboPanjeng2.Items.Add("2.정상경계");
            cboPanjeng2.Items.Add("3.정밀검사요함");
            cboPanjeng2.SelectedIndex = 0;
        }

        /// <summary>
        /// Main 폼에서 폼이 로드된 경우
        /// </summary>
        /// <param name="frm"></param>
        /// <returns></returns>
        private bool FormIsExist(Form frm)
        {
            foreach (Control ctl in this.Controls)
            {
                if (ctl is Form)
                {
                    if (ctl.Name == frm.Name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                Form frm = hf.OpenForm_Check_Return("frmHcSangInternetMunjinView");

                if (!frm.IsNullOrEmpty()) { frm.Dispose(); }
                
                ComFunc.KillProc("friendly omr.exe");
                ComFunc.KillProc("hcscript.exe");

                this.Close();
                return;
            }
            else if (sender == btnCancel)
            {
                fn_Screen_Clear();
                txtWrtNo.Focus();
            }
            else if (sender == btnDntOk)
            {
                txtDntPanJochi.Text = "식후3분이내 3분동안 하루에 3-4번은 꼼꼼히 잇솔질을 하시기 바랍니다.";
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
                FrmHeaResult.ShowDialog(this);
            }
            else if (sender == btnPACS)
            {
                FrmViewResult = new frmViewResult(FstrPtno);
                FrmViewResult.ShowDialog();
            }
            else if (sender == btnSave)
            {
                string[] strDPan = new string[15];
                int result = 0;

                if (txtPanDrNo.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("상담의사 면허번호 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (cboPanjeng1.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("치아상태 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (cboPanjeng2.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("구강상태 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtDntPanSogen.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("종합소견 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtDntPanJochi.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("가정에서 조치할 사항 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strDPan[0] = txtDntPanUp1.Text + "^^" + txtDntPanDown1.Text + "^^";
                strDPan[1] = txtDntPanUp2.Text + "^^" + txtDntPanDown2.Text + "^^";
                strDPan[2] = txtDntPanUp3.Text + "^^" + txtDntPanDown3.Text + "^^";

                strDPan[3] = VB.Left(cboDntPan1.Text, 1) + "^^" + txtDntPanCnt1.Text.Trim() + "^^";
                strDPan[4] = VB.Left(cboDntPan2.Text, 1);

                strDPan[5] = VB.Left(cboDntPan3.Text, 1);
                strDPan[6] = VB.Left(cboDntPan4.Text, 1) + "^^" + txtDntPanStsEtc.Text.Trim() + "^^";

                if (chkChiJu1.Checked == true)
                {
                    strDPan[7] += "1^^";
                }
                else
                {
                    strDPan[7] += "0^^";
                }

                if (chkChiJu2.Checked == true)
                {
                    strDPan[7] += "1^^";
                }
                else
                {
                    strDPan[7] += "0^^";
                }

                if (chkChiJu3.Checked == true)
                {
                    strDPan[7] += "1^^";
                }
                else
                {
                    strDPan[7] += "0^^";
                }
                strDPan[7] += txtChijuETC.Text + "^^";
                strDPan[8] = VB.Left(cboDntPan5.Text, 1);
                strDPan[9] = VB.Left(cboDntPan6.Text, 1);
                strDPan[10] = VB.Left(cboDntPan7.Text, 1) + "^^" + txtDntPanCnt2.Text + "^^";
                strDPan[11] = txtDntPanSogen.Text.Replace("'", "`");
                strDPan[12] = txtDntPanJochi.Text.Replace("'", "`");
                strDPan[13] = VB.Left(cboPanjeng1.Text, 1);
                strDPan[14] = VB.Left(cboPanjeng2.Text, 1);
                //=================================================================================

                clsDB.setBeginTran(clsDB.DbCon);

                HIC_SCHOOL_NEW item = new HIC_SCHOOL_NEW();

                item.DPAN1 = strDPan[0];
                item.DPAN2 = strDPan[1];
                item.DPAN3 = strDPan[2];
                item.DPAN4 = strDPan[3];
                item.DPAN5 = strDPan[4];
                item.DPAN6 = strDPan[5];
                item.DPAN7 = strDPan[6];
                item.DPAN8 = strDPan[7];
                item.DPAN9 = strDPan[8];
                item.DPAN10 = strDPan[9];
                item.DPAN11 = strDPan[10];
                item.DPANSOGEN = strDPan[11];
                item.DPANJOCHI = strDPan[12];
                item.DPAN12 = strDPan[13];
                item.DPAN13 = strDPan[14];
                item.DPANDRNO = txtPanDrNo.Text.To<long>();
                item.WRTNO = FnWRTNO;

                result = hicSchoolNewService.UpdatebyWrtNo(item);
                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("학생신체검사 저장 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //상담여부 체크 및 상담의사, 상담일자 저장
                result = hicJepsuService.UpdateGbMunjin2GbJinChal2byWrtNo(FnWRTNO);
                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담여부 체크 및 상담의사, 상담일자 저장 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //상담여부 액팅저장
                result = hicResultService.UpdateActiveResultbyWrtNoExCode(clsType.User.IdNumber, FnWRTNO);
                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담여부 액팅저장 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //상담여부 액팅저장
                result = hicResultService.UpdateResultbyWrtNo(clsType.User.IdNumber, FnWRTNO);
                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담여부 액팅저장 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //초등학생 구강만 있는 경우 접수마스타에 판정완료 SET
                if (FbDentalOnly == true)
                {
                    result = hicJepsuService.UpdatePanjengDrNobyWrtNo(FnWRTNO, txtPanDrNo.Text.To<long>());
                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("초등학생 구강만 있는 경우 접수마스타에 판정완료 SET 저장 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
            }
            else if (sender == btnSet3)
            {
                clsPublic.GstrRetValue = "3";


                FrmHcSchoolCommonInput = new frmHcSchoolCommonInput("3");
                FrmHcSchoolCommonInput.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSchoolCommonInput.ssPanjengDblClick += new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick);
                FrmHcSchoolCommonInput.ShowDialog(this);
                FrmHcSchoolCommonInput.ssPanjengDblClick -= new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick);

                if (!clsPublic.GstrRetValue.IsNullOrEmpty())
                {
                    txtDntPanSogen.Text += clsPublic.GstrRetValue;
                }
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                string strJong = "";
                string strFDate = "";
                string strTDate = "";
                string strSName = "";
                long nLtdCode = 0;
                string strJob = "";

                sp.Spread_All_Clear(SSList);
                SSList.ActiveSheet.RowCount = 50;

                strFDate = dtpFrDate.Text;
                strTDate = dtpToDate.Text;
                strSName = txtSName.Text.Trim();
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();

                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else if (rdoJob2.Checked == true)
                {
                    strJob = "2";
                }

                //상담대기순번
                if (chkWait.Checked == true && strJob == "1")
                {
                    List<HIC_JEPSU_SCHOOL_NEW_SANGDAM_WAIT> list = hicJepsuSchoolNewSangdamWaitService.GetItembyJepDate(strFDate, strTDate, strSName, nLtdCode);

                    nREAD = list.Count;
                    SSList.ActiveSheet.RowCount = nREAD;
                    for (int i = 0; i < nREAD; i++)
                    {
                        SSList.ActiveSheet.Cells[0, 0, SSList.ActiveSheet.RowCount - 1, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                        SSList.ActiveSheet.Cells[0, 0, SSList.ActiveSheet.RowCount - 1, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                        if (!list[i].DPANDRNO.IsNullOrEmpty())
                        {
                            if (list[i].DPANDRNO == clsHcVariable.GnHicLicense.To<string>())
                            {
                                SSList.ActiveSheet.Cells[i, 4].Text = "Y";
                                SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                                SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0FF"));
                            }
                        }
                        SSList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.To<string>();
                        SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                        SSList.ActiveSheet.Cells[i, 2].Text = list[i].JEPDATE.To<string>();
                        SSList.ActiveSheet.Cells[i, 3].Text = list[i].GJJONG;
                        SSList.ActiveSheet.Cells[i, 3].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                    }
                }
                else
                {
                    //clsHcVariable.GnHicLicense = 1565;

                    List<HIC_JEPSU_SCHOOL_NEW> list = hicJepsuSchoolNewService.GetItembyJepDate(strFDate, strTDate, strJob, strSName, nLtdCode, clsHcVariable.GnHicLicense);

                    nREAD = list.Count;
                    SSList.ActiveSheet.RowCount = nREAD;
                    for (int i = 0; i < nREAD; i++)
                    {
                        SSList.ActiveSheet.Cells[0, 0, SSList.ActiveSheet.RowCount - 1, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                        SSList.ActiveSheet.Cells[0, 0, SSList.ActiveSheet.RowCount - 1, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                        if (!list[i].DPANDRNO.IsNullOrEmpty())
                        {
                            if (list[i].DPANDRNO == clsHcVariable.GnHicLicense.To<string>())
                            {
                                SSList.ActiveSheet.Cells[i, 4].Text = "Y";
                                SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                                SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0FF"));
                            }
                        }
                        SSList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.To<string>();
                        SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                        SSList.ActiveSheet.Cells[i, 2].Text = list[i].JEPDATE.To<string>();
                        SSList.ActiveSheet.Cells[i, 3].Text = list[i].GJJONG;
                        SSList.ActiveSheet.Cells[i, 3].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                    }
                }

                //상담인원 및 대기인원 DISPLAY
                HIC_SCHOOL_NEW_JEPSU list2 = hicSchoolNewJepsuService.GetCntbyGjJong("56", clsHcVariable.GnHicLicense);

                lblCounter.Text = "총 대기인원 : ";
                lblCounter.Text += list2.CNT2.To<string>() + " 명 / ";
                lblCounter.Text += "상담인원";
                lblCounter.Text += list2.CNT.To<string>() + " 명";
            }
            else if (sender == btnMenuGen)
            {
                if (hf.OpenForm_Check("frmHcSangTeeth") == true)
                {
                    this.Close();
                    return;
                }

                FrmHcSangTeeth = new frmHcSangTeeth();
                FrmHcSangTeeth.WindowState = FormWindowState.Maximized;
                FrmHcSangTeeth.StartPosition = FormStartPosition.CenterParent;
                FrmHcSangTeeth.ShowDialog(this);

                this.Close();
            }
        }

        void frmHcSchoolCommonInput_ssPanjengDblClick(string argGubun, string strCommon)
        {
            if (argGubun == "1")
            {
                txtDntPanSogen.Text = strCommon;
            }
            else if (argGubun == "2")
            {
                txtDntPanJochi.Text = strCommon;
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
            string strFlag="";
            string strNextRoom = "";

            btnPACS.Enabled = true;
            btnMed.Enabled = true;
            pnl6.Enabled = true;

            FnWRTNO = txtWrtNo.Text.To<long>();

            if (FnWRTNO == 0)
            {
                return;
            }

            //삭제된것 체크
            if (hb.READ_JepsuSTS(FnWRTNO) == "D")
            {
                MessageBox.Show(FnWRTNO + "접수번호는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //생애검진자 체크
            if (VB.Left(hicJepsuService.GetGjJongbyWrtNo(FnWRTNO), 1) == "4")
            {
                MessageBox.Show("생애검진 대상자입니다. 전산처리불가", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //상담내역이 있는지 점검
            FstrROWID = hicSangdamNewService.GetRowIdbyWrtNo(FnWRTNO);

            //GoSub Screen_Injek_display       '인적사항을 Display
            HIC_JEPSU_PATIENT list = hicJepsuPatientService.GetItembyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 " + FnWRTNO + "번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //검진자 기본정보 표시---------------
            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PTNO;
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE + "/" + list.SEX;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.To<string>());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list.JEPDATE.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);

            FstrChasu = list.GJCHASU;
            FstrPtno = list.PTNO;
            FnPano = list.PANO;
            FnAge = list.AGE;
            FstrSex = list.SEX;
            FstrJepDate = list.JEPDATE.To<string>();
            FstrGjJong = list.GJJONG;
            FstrJumin = clsAES.DeAES(list.JUMIN2);
            FstrYear = list.GJYEAR;

            FbDentalOnly = false;
            //초등학교 2,3,5,6학년은 구강검사만 있음
            if (list.GBN == "1")
            {
                switch (list.CLASS)
                {
                    case 2:
                    case 3:
                    case 5:
                    case 6:
                        FbDentalOnly = true;
                        break;
                    default:
                        break;
                }
            }

            if (VB.I(ssPatInfo.ActiveSheet.Cells[0, 3].Text.Trim(), "초등") > 1)
            {
                cboDntPan5.Text = "";
                cboDntPan6.Text = "";
                cboDntPan7.Text = "";
            }
            else if (VB.I(ssPatInfo.ActiveSheet.Cells[0, 3].Text.Trim(), "중학") > 1)
            {
                pnlStep2.Enabled = true;
                cboDntPan6.Text = "";
                cboDntPan7.Text = "";
            }
            else if (VB.I(ssPatInfo.ActiveSheet.Cells[0, 3].Text.Trim(), "고등") > 1)
            {
                pnlStep2.Enabled = true;
                pnlStep3.Enabled = true;
            }

            if (rdoJob1.Checked == true)    //신규
            {
                if (SSList.ActiveSheet.Cells[(int)FnRow, 4].Text == "Y")    //이전에 상담한 수검자는 READ 함
                {
                    fn_Screen_SangDam_display(FnWRTNO);
                }
            }
            else
            {
                fn_Screen_SangDam_display(FnWRTNO);
            }

            if (txtPanDrNo.Text.To<long>() != 0)
            {
                if (txtPanDrNo.Text != hb.READ_License_DrName(txtPanDrNo.Text.To<long>()))
                {
                    lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                }
            }
            else
            {
                txtPanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
                lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
            }

            fn_Genjin_Histroy_SET();          //검진 HISTORY

            //다음 상담.검사실 표시
            this.Text += VB.Pstr(this.Text, "▶다음 검사실", 1).Trim();
            lblWait.Text = "";

            strNextRoom = hicSangdamWaitService.GetINextRoombyWrtNo(FnWRTNO);

            if (!strNextRoom.IsNullOrEmpty())
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
        }

        void fn_Genjin_Histroy_SET()
        {
            int nREAD = 0;
            string strData = "";
            string strJong = "";
            long nHeaPano = 0;

            //종검의 등록번호를 찾음
            nHeaPano = 0;

            nHeaPano = hicPatientService.GetPanobyJumin2(clsAES.AES(FstrJumin)).To<long>();

            //일반건진, 종합검진의 접수내역을 Display
            List<HIC_JEPSU> list = hicJepsuService.GetItembyUnionPaNo(FnPano);

            nREAD = list.Count;
            SSHistory.ActiveSheet.RowCount = nREAD;
            for (int i = 0; i < nREAD; i++)
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

        void fn_Screen_SangDam_display(long nWrtNo)
        {
            List<HIC_SCHOOL_NEW> list = hicSchoolNewService.GetItembyWrtNo(FnWRTNO);

            if (!list.IsNullOrEmpty())
            {
                txtDntPanUp1.Text = VB.Pstr(list[0].DPAN1, "^^", 1);
                txtDntPanDown1.Text = VB.Pstr(list[0].DPAN1, "^^", 2);
                txtDntPanUp2.Text = VB.Pstr(list[0].DPAN2, "^^", 1);
                txtDntPanDown2.Text = VB.Pstr(list[0].DPAN2, "^^", 2);
                txtDntPanUp3.Text = VB.Pstr(list[0].DPAN3, "^^", 1);
                txtDntPanDown3.Text = VB.Pstr(list[0].DPAN3, "^^", 2);
                cboDntPan1.SelectedIndex = VB.Pstr(list[0].DPAN4, "^^", 1).To<int>();
                txtDntPanCnt1.Text = VB.Pstr(list[0].DPAN4, "^^", 2);
                cboDntPan2.SelectedIndex = list[0].DPAN5.To<int>();
                cboDntPan3.SelectedIndex = list[0].DPAN6.To<int>();
                cboDntPan4.SelectedIndex = VB.Pstr(list[0].DPAN7, "^^", 1).To<int>();
                txtDntPanStsEtc.Text = VB.Pstr(list[0].DPAN7, "^^", 2);

                if (VB.Pstr(list[0].DPAN8, "^^", 1) == "1")
                {
                    chkChiJu1.Checked = true;
                }
                else
                {
                    chkChiJu1.Checked = false;
                }

                if (VB.Pstr(list[0].DPAN8, "^^", 2) == "1")
                {
                    chkChiJu2.Checked = true;
                }
                else
                {
                    chkChiJu2.Checked = false;
                }

                if (VB.Pstr(list[0].DPAN8, "^^", 3) == "1")
                {
                    chkChiJu3.Checked = true;
                }
                else
                {
                    chkChiJu3.Checked = false;
                }

                txtChijuETC.Text = VB.Pstr(list[0].DPAN8, "^^", 4);
                cboDntPan5.SelectedIndex = list[0].DPAN9.To<int>();
                cboDntPan6.SelectedIndex = list[0].DPAN10.To<int>();
                cboDntPan7.SelectedIndex = VB.Pstr(list[0].DPAN11, "^^", 1).To<int>();

                cboPanjeng1.SelectedIndex = list[0].DPAN12.To<int>();
                cboPanjeng2.SelectedIndex = list[0].DPAN13.To<int>();

                txtDntPanCnt2.Text = VB.Pstr(list[0].DPAN11, "^^", 2);
                txtDntPanSogen.Text = list[0].DPANSOGEN;
                txtDntPanJochi.Text = list[0].DPANJOCHI;
                txtPanDrNo.Text = list[0].DPANDRNO.To<string>();
            }
        }

        void fn_Screen_Clear()
        {
            btnPACS.Enabled = false;
            btnMed.Enabled = false;
            pnl6.Enabled = false;
            btnSave.Enabled = true;
            txtLtdCode.Text = "";
            txtSName.Text = "";
            txtWrtNo.Text = "";

            sp.Spread_All_Clear(ssPatInfo);
            ssPatInfo.ActiveSheet.RowCount = 1;
            lblWait.Text = "";

            txtDntPanCnt1.Text = "";
            txtDntPanCnt2.Text = "";
            
            txtDntPanUp1.Text = "";
            txtDntPanUp2.Text = "";
            txtDntPanUp3.Text = "";

            txtDntPanDown1.Text = "";
            txtDntPanDown2.Text = "";
            txtDntPanDown3.Text = "";

            chkChiJu1.Checked = false;
            chkChiJu2.Checked = false;
            chkChiJu3.Checked = false;

            txtChijuETC.Text = "";

            cboDntPan1.SelectedIndex = 1;
            cboDntPan2.SelectedIndex = 1;
            cboDntPan3.SelectedIndex = 1;
            cboDntPan4.SelectedIndex = 0;
            cboDntPan5.SelectedIndex = 1;
            cboDntPan6.SelectedIndex = 1;
            cboDntPan7.SelectedIndex = 0;

            txtDntPanStsEtc.Text = "";
            txtDntPanSogen.Text = "";
            txtDntPanJochi.Text = "";
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSList)
            {
                if (e.ColumnHeader == true)
                {
                    return;
                }

                FnRow = e.Row;
                fn_Screen_Clear();

                txtWrtNo.Text = SSList.ActiveSheet.Cells[e.Row, 0].Text.Trim();

                fn_Screen_Display();
            }
        }

        void eRdoClick(object sender, EventArgs e)
        {
            if (sender == rdoJob1 || sender == rdoJob2)
            {
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            string strJong = "";
            string strWrtNo = "";

            if (sender == txtWrtNo) 
            {
                if (e.KeyChar == 13)
                {
                    if (txtWrtNo.Text.IsNullOrEmpty())
                    {
                        return;
                    }

                    strJong = hb.READ_GJJONG_CODE(txtWrtNo.Text.To<long>());

                    if (strJong != "56")
                    {
                        strWrtNo = txtWrtNo.Text;
                        txtWrtNo.Text = "";

                        frmHcSangTeeth f = new frmHcSangTeeth(strWrtNo.To<long>());
                        f.WindowState = FormWindowState.Maximized;
                        f.ShowDialog(this);

                        this.Close();
                        return;
                    }
                    strWrtNo = txtWrtNo.Text.Trim();

                    fn_Screen_Clear();
                    txtWrtNo.Text = strWrtNo;
                    fn_Screen_Display();
                }
                //SendKeys.Send("{TAB}");
            }
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtDntPanSogen)
            {
                txtDntPanSogen.ImeMode = ImeMode.Hangul;
            }
            else if (sender == txtSName)
            {
                txtSName.ImeMode = ImeMode.Hangul;
            }
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            string strJong = "";
            string strWrtNo = "";

            if (sender == txtDntPanSogen)
            {
                txtDntPanSogen.ImeMode = ImeMode.Hangul;
            }
        }

        bool WAIT_NextRoom_SET()
        {
            bool rtnVal = false;

            long nWait = 0;
            string strNextRoom = "";
            string strRoom = "";
            string strTemp = "";
            long nWRTNO = 0;
            string strGjJong = "";
            string strSname = "";
            string strSex = "";
            long nAge = 0;
            int result = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            strNextRoom = hicSangdamWaitService.GetINextRoombyWrtNo(FnWRTNO);

            //다음 검사실이 없으면
            if (strNextRoom.IsNullOrEmpty())
            {
                result = hicSangdamWaitService.UpdateGbCallbyWrtNo(clsHcVariable.GstrDrRoom, FnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("저장 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            strRoom = VB.Pstr(strNextRoom, ",", 1);
            strTemp = VB.Pstr(strNextRoom, strRoom + ",", 2);
            strNextRoom = strTemp;

            //다음 가셔야할곳이 접수창구이면 등록 안함
            if (string.Compare(strRoom, "30") >= 0)
            {
                result = hicSangdamWaitService.Delete_Hic_Sangdam_Wait(FnPano, "");

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("저장 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            nWait = hicSangdamWaitService.GetWaitbyGubunEntTime(strRoom);

            clsDB.setCommitTran(clsDB.DbCon);

            //기존 등록된 대기순번을 삭제함
            result = hicSangdamWaitService.Delete_Hic_Sangdam_Wait(FnPano, "");

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("저장 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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

                    //상담대기 등록함

                    HIC_SANGDAM_WAIT item = new HIC_SANGDAM_WAIT();

                    item.WRTNO = nWRTNO;
                    item.SNAME = strSname;
                    item.SEX = strSex;
                    item.AGE = nAge;
                    item.GJJONG = strGjJong;
                    item.GUBUN = strRoom;
                    item.WAITNO = nWait;
                    item.PANO = FnPano;
                    item.NEXTROOM = strNextRoom;

                    result = hicSangdamWaitService.Insert(item);

                    if (result != 0)
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
    }
}
