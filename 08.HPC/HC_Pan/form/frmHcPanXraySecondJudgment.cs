using ComBase;
using ComLibB;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanXraySecondJudgment.cs
/// Description     : 방사선 작업종사자 2차판정
/// Author          : 이상훈
/// Create Date     : 2019-12-11
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm방사선종사자1차판정.frm(Frm방사선종사자1차판정)" />

namespace HC_Pan.form
{
    public partial class frmHcPanXraySecondJudgment : Form
    {
        ComHpcLibBService comHpcLibBService = null;
        HicJepsuService hicJepsuService = null;
        HicPatientService hicPatientService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;
        BasIllsService basIllsService = null;
        HicJepsuXMunjinExjongLtdService hicJepsuXMunjinExjongLtdService = null;
        HicResBohum1Service hicResBohum1Service = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcCombo Combo = new clsHcCombo();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        frmHcCodeHelp FrmHcCodeHelp = null;
        HIC_CODE CodeHelpItem = null;

        long FnWRTNO;
        long FnPano;
        string FstrJumin;
        string FstrSex;
        string FstrPano;    //원무행정의 등록번호
        string FstrJong;    //건진종류

        long FnWrtno1;  //1차
        long FnWrtno2;  //2차
        string FstrCOMMIT;
        string FstrUCodes;
        string FstrSaveGbn;

        string FstrGbOHMS;
        string FstrMCode;
        string FstrYuhe;

        string FstrROWID;
        string FstrYROWID;  // 약속판정 ROWID
        string FstrPROWID;
        string FstrPanOk1;
        string FstrPanOk2;

        long FnPan2Row;

        int nOldCNT = 0;
        //string strExamCode = "";
        List<string> strExamCode = new List<string>();
        string strAllWRTNO = "";
        string strJepDate = "";
        string strExPan = "";

        int nHyelH = 0;
        int nHyelL = 0;
        int nHeight = 0;
        int nWeight = 0;
        int nResult = 0;

        int nREAD = 0;

        string strExCode = "";
        string strResult = "";
        string strResCode = "";
        string strResultType = "";
        string strGbCodeUse = "";
        string strResName = "";
        string strRemark = "";
        string strOldJepsuDate = "";
        string strOldJepDate = "";

        string FstrCode;
        string FstrName;

        public frmHcPanXraySecondJudgment()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuService = new HicJepsuService();
            hicPatientService = new HicPatientService();
            hicResultExCodeService = new HicResultExCodeService();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            basIllsService = new BasIllsService();
            hicJepsuXMunjinExjongLtdService = new HicJepsuXMunjinExjongLtdService();
            hicResBohum1Service = new HicResBohum1Service();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.cboPanjeng.KeyPress += new KeyPressEventHandler(eComboBoxKeyPress);
            this.cboPanjeng.Click += new EventHandler(eComboBoxClick);
            this.chkAllDoctor.Click += new EventHandler(eChkBoxClick);
            this.chkX1_11.Click += new EventHandler(eChkBoxClick);
            this.chkX1_23.Click += new EventHandler(eChkBoxClick);
            this.chkX1_42.Click += new EventHandler(eChkBoxClick);
            this.chkX43.Click += new EventHandler(eChkBoxClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSHistory.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtEtc.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtJobYN.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtJochiRemark.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtJochiRemark.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtPanDate.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtPanDrNo1.KeyUp += new KeyEventHandler(eTxtKeyUp);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }


        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            this.Text = "방사선 작업종사자 1차판정(" + clsHcVariable.GstrHicDrName + ") (면허:" + clsHcVariable.GnHicLicense + ")";

            SSList_Sheet1.Columns.Get(4).Visible = false;   //1차

            SS2_Sheet1.Columns.Get(7).Visible = false;      //검사코드
            SS2_Sheet1.Columns.Get(8).Visible = false;      //결과1
            SS2_Sheet1.Columns.Get(9).Visible = false;      //결과2
            SS2_Sheet1.Columns.Get(10).Visible = false;     //결과3
            SS2_Sheet1.Columns.Get(11).Visible = false;     //결과4

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-30).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtLtdCode.Text = "";

            fn_Screen_Clear();

            Combo.ComboPanjeng2_SET(cboPanjeng);  //특수종합판정

            eBtnClick(btnSearch, new EventArgs());

            if (clsHcVariable.GnWRTNO > 0)
            {
                FnWRTNO = clsHcVariable.GnWRTNO;
                fn_Screen_Display();
                fn_Genjin_Histroy_SET();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnDefault)
            {
                txtSogenRemark.Text = "정상";
            }
            else if (sender == btnJobYN)
            {
                FrmHcCodeHelp = new frmHcCodeHelp("13");    //업무수행적합여부
                FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(Code_value);
                FrmHcCodeHelp.ShowDialog();
                FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(Code_value);

                if (!FstrCode.IsNullOrEmpty())
                {
                    txtJobYN.Text = FstrCode.Trim();
                    lblJobYN.Text = FstrName.Trim();
                }
                else
                {
                    txtJobYN.Text = "";
                    lblJobYN.Text = "";
                }
            }
            else if (sender == btnJochi1)
            {
                clsPublic.GstrRetValue = "70";
                frmHcPanXHelp f = new frmHcPanXHelp("70");
                f.ShowDialog(this);
            }
            else if (sender == btnLtdCode)
            {
                clsPublic.GstrRetName = "";

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
            else if (sender == btnMCancel)
            {
                txtPlace.Text = "";
                txtRemark.Text = "";
                txtMuch.Text = "";
                txtJung.Text = "";
                txtEye.Text = "";
                txtSkin.Text = "";
                txtEtc.Text = "";
                txtTerm.Text = "";
                txtXTerm.Text = "";
                txtMun1.Text = "";
                rdo1.Checked = false;
                rdo2.Checked = false;
                txtXJong.Text = "";
                txtPanDrNo1.Text = "";
                lblDrName1.Text = "";
                rdoGubun1.Checked = false;
                rdoGubun2.Checked = false;
            }
            else if (sender == btnMed)
            {
                frmHeaResult f = new frmHeaResult(FstrJumin);
                f.ShowDialog(this);
            }
            else if (sender == btnMSave)
            {
                string strYN = "";
                string strGbn = "";
                int result = 0;

                if (txtPanDrNo1.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("진찰의사면호번호 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clsDB.setBeginTran(clsDB.DbCon);

                    if (rdo1.Checked == true)
                    {
                        strYN = "Y";
                    }
                    else
                    {
                        strYN = "N";
                    }

                    if (rdoGubun1.Checked == true)
                    {
                        strGbn = "Y";
                    }
                    else
                    {
                        strGbn = "N";
                    }

                    COMHPC item = new COMHPC();

                    item.JINGBN = strGbn;
                    item.XP1 = strYN;
                    item.XPJONG = txtXJong.Text.Trim();
                    item.XPLACE = txtPlace.Text.Trim();
                    item.XREMARK = txtRemark.Text.Trim();
                    item.XMUCH = txtMuch.Text.Trim();
                    item.XTERM = txtTerm.Text.Trim();
                    item.XTERM1 = txtTerm.Text.Trim();
                    item.XJUNGSAN = txtJung.Text.Trim();
                    item.MUN1 = txtMun1.Text.Trim();
                    item.JUNGSAN1 = txtEye.Text.Trim();
                    item.JUNGSAN2 = txtSkin.Text.Trim();
                    item.JUNGSAN3 = txtEtc.Text.Trim();
                    item.MUNDRNO = txtPanDrNo1.Text;
                    item.WRTNO = FnWRTNO;

                    result = comHpcLibBService.UpdateHic_X_MunjinbyWrtNo(item);

                    if (result < 0)
                    {
                        MessageBox.Show("자료를 등록중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnMCancel, new EventArgs());
            }
            else if (sender == btnPACS)
            {
                clsPublic.GstrHelpCode = FstrPano;

                frmViewResult f = new frmViewResult(FstrPano);
                f.ShowDialog(this);
            }           
            else if (sender == btnSahu)
            {
                clsPublic.GstrRetValue = "70";
                frmHcPanSpcSahuCode f = new frmHcPanSpcSahuCode("70");
                f.ShowDialog(this);
            }
            else if (sender == btnSogen)
            {
                clsPublic.GstrRetValue = "70";
                frmHcPanXHelp f = new frmHcPanXHelp("70");
                f.ShowDialog(this);
                if (!clsPublic.GstrRetValue.IsNullOrEmpty())
                {
                    txtSogenRemark.Text = clsPublic.GstrRetValue;
                }
            }
            else if (sender == btnSearch)
            {
                int nRead = 0;
                string strOK = "";
                string strFrDate = "";
                string strToDate = "";
                long nLtdCode = 0;
                string strSName = "";
                string strJob = "";
                string strAll = "";
                string strAllDoctor = "";
                string strSort = "";
                string[] strGjJong = { "50" };

                //결과 테이블에 자료 INSERT
                fn_HIC_X_MUNJIN_INSERT(strGjJong);
                sp.Spread_All_Clear(SSList);
                txtSName.Text = txtSName.Text.Trim();
                strSName = txtSName.Text.Trim();
                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));

                if (chkAll.Checked == true)
                {
                    strAll = "Y";
                }
                else
                {
                    strAll = "";
                }

                if (chkAllDoctor.Checked == true)
                {
                    strAllDoctor = "Y";
                }
                else
                {
                    strAllDoctor = "";
                }

                if (chkSort.Checked == true)
                {
                    strSort = "Y";
                }
                else
                {
                    strSort = "";
                }

                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else if (rdoJob2.Checked == true)
                {
                    strJob = "2";
                }

                if (!txtLtdCode.Text.IsNullOrEmpty())
                {
                    if (txtLtdCode.Text.Trim().IndexOf(".") != -1)
                    {
                        txtLtdCode.Text = VB.Pstr(txtLtdCode.Text.Trim(), ".", 1) + "." + hb.READ_Ltd_Name(VB.Pstr(txtLtdCode.Text.Trim(), ".", 1));
                    }
                    else
                    {
                        txtLtdCode.Text = txtLtdCode.Text.Trim() + "." + hb.READ_Ltd_Name(VB.Pstr(txtLtdCode.Text.Trim(), ".", 1));
                    }
                }

                //신규접수 및 접수수정 자료를 SELECT
                List<HIC_JEPSU_X_MUNJIN_EXJONG_LTD> list = hicJepsuXMunjinExjongLtdService.GetItembyJepDateLtdCodeSName(strFrDate, strToDate, nLtdCode, strSName, clsHcVariable.GnHicLicense, strJob, strAll, strAllDoctor, strSort);

                nRead = list.Count;
                SSList.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    strOK = "OK";

                    if (strOK == "OK")
                    {
                        SSList.ActiveSheet.Cells[i, 0].Text = list[i].SNAME.Trim();
                        SSList.ActiveSheet.Cells[i, 1].Text = hb.READ_GjJong_Name(list[i].GJJONG.Trim());
                        SSList.ActiveSheet.Cells[i, 2].Text = list[i].LTDNAME.Trim();
                        SSList.ActiveSheet.Cells[i, 3].Text = list[i].JEPDATE.ToString();
                        if (!list[i].PANJENG.IsNullOrEmpty())
                        {
                            if (list[i].PANJENGDRNO == 0)
                            {
                                SSList.ActiveSheet.Cells[i, 4].Text += "*";
                                SSList.ActiveSheet.Cells[i, 4].BackColor = Color.FromArgb(255, 255, 0);
                            }
                        }
                        SSList.ActiveSheet.Cells[i, 5].Text = list[i].WRTNO;
                        SSList.ActiveSheet.Cells[i, 6].Text = list[i].SEX.Trim();
                        SSList.ActiveSheet.Cells[i, 7].Text = list[i].AGE.ToString();
                    }
                }
                
                txtSName.Text = "";
                SSList.Enabled = true;
                btnSearch.Enabled = true;
            }
            else if (sender == btnCancel)
            {
                string sMsg = "";

                sMsg = "변경한 내용을 저장하지 않습니다." + "\r\n";
                sMsg += "정말 취소하시겠습니까?";
                if (MessageBox.Show("", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fn_Screen_Clear();
                    SSList.Focus();
                    return;
                }
            }
            else if (sender == btnOK)
            {
                fn_DB_Update_Panjeng("판정");
                Panjeng_End_Check();
            }
        }

        private void Code_value(string strCode, string strName)
        {
            FstrCode = strCode;
            FstrName = strCode;
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSHistory)
            {
                frmHcPanXrayPracticianView f = new frmHcPanXrayPracticianView(SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim().To<long>());
                f.ShowDialog(this);
            }
            else if (sender == SSList)
            {
                string strOK = "";

                fn_Screen_Clear();

                FnWRTNO = long.Parse(SSList.ActiveSheet.Cells[e.Row, 5].Text.Trim());
                FstrJong = SSList.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                txtWrtNo.Text = FnWRTNO.ToString();

                //삭제된것 체크
                if (hb.READ_JepsuSTS(FnWRTNO) == "D")
                {
                    MessageBox.Show("접수번호는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                fn_Screen_Display();
                fn_Genjin_Histroy_SET();

                if (clsHcVariable.GnHicLicense == 0)
                {
                    btnOK.Enabled = false;
                }
            }
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == txtEtc)
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:
                        txtEtc.Text = "정상";
                        break;
                    default:
                        break;
                }
            }
            else if (sender == txtEye)
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:
                        txtEye.Text = "정상";
                        break;
                    default:
                        break;
                }
            }
            else if (sender == txtSkin)
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:
                        txtSkin.Text = "정상";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                SendKeys.Send("{TAB}");
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtJobYN)
                {
                    lblJobYN.Text = hb.READ_HIC_CODE("13", txtJobYN.Text.Trim());   //업무적합성
                    SendKeys.Send("{TAB}");
                }
                else if (sender == txtPanDate)
                {
                    SendKeys.Send("{TAB}");
                }
                else if (sender == txtPanDrNo)
                {
                    lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
                    SendKeys.Send("{TAB}");
                }
                else if (sender == txtPanDrNo1)
                {
                    lblDrName1.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo1.Text));
                    SendKeys.Send("{TAB}");
                }
                else if (sender == txtSahu)
                {
                    lblSahuName.Text = hm.Sahu_Names_Display(txtSahu.Text.Trim());
                    SendKeys.Send("{TAB}");
                }
                else if (sender == txtSName)
                {
                    btnSearch.Focus();
                }
                else if (sender == txtSogenRemark)
                {
                    SendKeys.Send("{TAB}");
                }
                else if (sender == txtWrtNo)
                {
                    long nWrtNo = 0;

                    if (txtWrtNo.Text.IsNullOrEmpty()) return;

                    nWrtNo = long.Parse(txtWrtNo.Text);
                    if (nWrtNo == 0)
                    {
                        return;
                    }

                    fn_Screen_Clear();
                    txtWrtNo.Text = nWrtNo.ToString();
                    FnWRTNO = nWrtNo;

                    //삭제된것 체크
                    if (hb.READ_JepsuSTS(FnWRTNO) == "D")
                    {
                        MessageBox.Show("접수번호 " + FnWRTNO + " 는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    fn_Screen_Display();
                    fn_Genjin_Histroy_SET();

                    if (clsHcVariable.GnHicLicense == 0)
                    {
                        btnOK.Enabled = false;
                    }
                }
            }
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtJochiRemark)
            {
                txtJochiRemark.ImeMode = ImeMode.Hangul;
            }
            else if (sender == txtPanDrNo1)
            {
                lblMsg.Text = "F5.전혜리 F6.김중구 F7.황성일 F8.김주호 F9.공백";
            }
            else if (sender == txtSName)
            {
                txtSName.ImeMode = ImeMode.Hangul;
            }
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txtJochiRemark)
            {
                txtJochiRemark.ImeMode = ImeMode.Hangul;
            }
            else if (sender == txtSName)
            {
                txtSName.ImeMode = ImeMode.Alpha;
            }
        }

        void eTxtDblClick(object sender, EventArgs e)
        {
            if (sender == txtPanDate)
            {
                frmCalendar f = new frmCalendar();
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();

                txtPanDate.Text = clsPublic.GstrCalDate;
                clsPublic.GstrCalDate = "";
            }
        }

        void eTxtKeyUp(object sender, KeyEventArgs e)
        {
            long nDrNO = 0;
            string strResCode = "";
            string strResType = "";

            switch (e.KeyCode)
            {
                case Keys.F5:
                    nDrNO = 48054;  //F5(전혜리)
                    break;
                case Keys.F6:
                    nDrNO = 19516;  //F6(김중구)
                    break;
                case Keys.F7:
                    nDrNO = 18685;  //F7(황성일)
                    break;
                case Keys.F8:
                    nDrNO = 22977;  //F8(김주호)
                    break;
                case Keys.F9:
                    nDrNO = 0;
                    break;
                default:
                    break;
            }

            txtPanDrNo1.Text = nDrNO.ToString();
            lblDrName1.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo1.Text));
            SendKeys.Send("{TAB}");
        }

        void fn_HIC_X_MUNJIN_INSERT(string[] argJong)
        {
            int nREAD = 0;
            long nWrtNo = 0;
            string strJepDate = "";
            string strFrDate = "";
            string strToDate = "";

            long nLtdCode = 0;
            string strSName = "";

            int result = 0;

            strFrDate = dtpFrDate.Text;
            strToDate = dtpToDate.Text;
            nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
            strSName = txtSName.Text.Trim();

            List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDateLtdCodeSName(strFrDate, strToDate, nLtdCode, strSName, argJong);

            nREAD = list.Count;
            if (nREAD == 0) return;

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < nREAD; i++)
            {
                nWrtNo = list[i].WRTNO;
                strJepDate = list[i].JEPDATE;

                result = comHpcLibBService.SaveHicXMunjinbyWrtNo(nWrtNo, strJepDate);

                if (result < 0)
                {
                    MessageBox.Show("자료 등록중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
            }
            clsDB.setCommitTran(clsDB.DbCon);

        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        void eChkBoxClick(object sender, EventArgs e)
        {
            if (sender == chkAllDoctor)
            {
                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == chkX1_13)
            {
                if (chkX1_11.Checked == true)
                {
                    txtX1_1.Enabled = true;
                }
                else
                {
                    txtX1_1.Enabled = false;
                }
            }
            else if (sender == chkX1_23)
            {
                if (chkX1_23.Checked == true)
                {
                    txtX1_2.Enabled = true;
                }
                else
                {
                    txtX1_2.Enabled = false;
                }
            }
            else if (sender == chkX1_42)
            {
                if (chkX1_42.Checked == true)
                {
                    txtX1_4.Enabled = true;
                }
                else
                {
                    txtX1_4.Enabled = false;
                }
            }
            else if (sender == chkX43)
            {
                if (sender == chkX1_41)
                {
                    txtJikjong.Enabled = true;
                }
                else
                {
                    txtJikjong.Enabled = false;
                }
            }

        }

        void fn_Screen_Clear()
        {
            FnPano = 0; FstrJumin = ""; FnWRTNO = 0;
            FstrSex = ""; FstrROWID = "";
            FstrUCodes = ""; FstrCOMMIT = ""; FnPano = 0;
            FnWrtno1 = 0; FnWrtno2 = 0;
            FstrSaveGbn = ""; FstrPano = ""; FstrJumin = "";
            FstrGbOHMS = "";
            FstrPanOk1 = ""; FstrPanOk2 = "";

            txtPanDate.Text = "";
            txtWrtNo.Text = "";
            lblMsg.Text = "";

            txtSName.Text = "";
            txtResult1.Text = "";
            txtResult21.Text = "";
            txtResult22.Text = "";

            txtJobYN.Text = "";
            txtSahu.Text = "";
            lblJobYN.Text = "";
            lblSahuName.Text = "";

            //ssPatInfo.ActiveSheet.Cells[0, 0].Text = "";
            sp.Spread_All_Clear(ssPatInfo);
            ssPatInfo.ActiveSheet.RowCount = 1;
            sp.Spread_All_Clear(SS2);
            SS2.ActiveSheet.RowCount = 40;

            //문진
            txtPlace.Text = "";
            txtRemark.Text = "";
            txtMuch.Text = "";
            txtJung.Text = "";
            txtEye.Text = "";
            txtSkin.Text = "";
            txtEtc.Text = "";
            txtTerm.Text = "";
            rdo1.Checked = false;
            rdo2.Checked = false;
            txtXJong.Text = "";
            txtXTerm.Text = "";
            txtMun1.Text = "";
            txtXJong.Text = "";
            rdoGubun1.Checked = false;
            rdoGubun2.Checked = false;
            txtSogenRemark.Text = "";
            txtJochiRemark.Text = "";

            sp.Spread_All_Clear(SS2);

            txtPanDrNo.Text = "";
            lblDrName.Text = "";

            //방사선종사자 상담부분 추가항목
            tabControl2.SelectedTab = tab21;
            tabControl3.SelectedTab = tab31;

            rdoX11.Checked = true;

            chkX1_11.Checked = false;
            chkX1_12.Checked = false;
            chkX1_13.Checked = false;

            chkX1_21.Checked = false;
            chkX1_22.Checked = false;
            chkX1_23.Checked = false;

            chkX1_41.Checked = false;
            chkX1_42.Checked = false;

            txtX1_1.Text = "";
            txtX1_2.Text = "";
            txtX1_3.Text = "";
            txtX1_4.Text = "";
            txtX1_5.Text = "";

            txtX1_1.Enabled = false;
            txtX1_2.Enabled = false;
            txtX1_4.Enabled = false;
            txtJikjong.Enabled = false;

            txtX2_1.Text = "";
            txtX2_2.Text = "";
            txtX2_3.Text = "";

            rdoX21.Checked = true;
            txtXSympton.Text = "";

            chkX41.Checked = false;
            chkX42.Checked = false;
            chkX43.Checked = false;
            txtJikjong.Text = "";
            txtRemark2.Text = "";
        }

        void Panjeng_End_Check()
        {
            string strOK = "";
            long nPanDrno = 0;
            string strPanDate = "";
            int result = 0;
            string sMsg = "";

            strOK = "OK";

            clsDB.setBeginTran(clsDB.DbCon);
            //방사선종사자 1차 판정완료 Check
            COMHPC list = comHpcLibBService.GetHic_X_MunjinbyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                strOK = "NO";
            }
            else
            {
                if (list.PANJENGDRNO == 0)
                {
                    strOK = "NO";
                }
                nPanDrno = list.PANJENGDRNO;
                strPanDate = list.PANJENGDATE.ToString();
            }

            //판정완료/미완료 SET
            result = comHpcLibBService.UpdateHic_X_Munjin(FnWRTNO, strOK);

            if (result < 0)
            {
                MessageBox.Show("방사선 작업종사자 판정 여부 Setting 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            //접수마스타에 판정일자 Update
            result = hicJepsuService.UpdatePanjengDatebyWrtNo(FnWRTNO, strOK, strPanDate, nPanDrno);

            if (result < 0)
            {
                MessageBox.Show("방사선 작업종사자 판정 여부 Setting 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            if (strOK == "OK")
            {
                sMsg = "판정이 완료되었습니다." + "\r\n";
                sMsg += "화면을 Clear후 다음 환자를 판정하시겠습니까?";
                if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fn_Screen_Clear();
                    eBtnClick(btnSearch, new EventArgs());
                    SSList.Focus();
                }
            }
        }

        void fn_Screen_Display()
        {
            int nREAD = 0;
            int nRow = 0;

            string strSex = "";
            string strPart = "";
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strResName = "";
            string strGbCodeUse = "";
            string strNomal = "";
            string strIpsadate = "";
            string strJepDate = "";
            string strGjJong = "";
            long nLicense = 0;
            string strDrname = "";

            int nAscii = 0;
            int nHyelH = 0;
            int nHyelL = 0;
            int nHeight = 0;
            int nWeight = 0;
            int nResult = 0;
            string strRemark = "";
            string strExPan = "";    //검사1건의 판정결과
            string strGbSPC = "";

            int nGan1 = 0;
            int nGan2 = 0;
            int nGanResult = 0;

            txtResult1.Text = "";
            txtResult21.Text = "";
            txtResult22.Text = "";

            tabControl1.SelectedTab = tab11;
            tabControl1.Text = "";
            tabControl2.SelectedTab = tab22;
            tabControl2.Text = "";

            //Screen_Injek_display  
            //인적사항을 Display
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWRTNO);

            if (list == null)
            {
                MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            strIpsadate = list.IPSADATE.ToString();
            FnPano = list.PANO;
            strSex = list.SEX;
            strJepDate = list.JEPDATE;
            clsHcVariable.GstrGjYear = list.GJYEAR;
            FstrSex = strSex;

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PANO.ToString();
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE + "/" + list.SEX;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.ToString());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = strJepDate;
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);
            strGjJong = list.GJJONG;
            strIpsadate = list.IPSADATE.ToString();

            btnPACS.Enabled = false;
            btnMed.Enabled = false;

            //주민등록번호로 원무행정의 등록번호를 찾음
            HIC_PATIENT list2 = hicPatientService.GetItembyPaNo(FnPano);

            FstrJumin = "";
            FstrPano = "";

            if (!list2.IsNullOrEmpty())
            {
                FstrJumin = clsAES.DeAES(list2.JUMIN2.Trim());
                FstrPano = list2.PTNO;
            }

            //주민등록번호로 환자 등록번호 찾기
            if (!FstrPano.IsNullOrEmpty())
            {
                btnPACS.Enabled = true;
            }

            //종합검진을 하였는지 점검함
            if (hicPatientService.GetCountbyJumin2(clsAES.AES(FstrJumin)) > 0)
            {
                btnMed.Enabled = true;
            }

            hm.ExamResult_RePanjeng(FnWRTNO, FstrSex, strJepDate, ""); //검사결과를 재판정

            //Screen_Exam_Items_display  
            //검사항목을 Display
            List<HIC_RESULT_EXCODE> list3 = hicResultExCodeService.GetItembyWrtNoOrderbyPanjengPartExCode(FnWRTNO);

            nREAD = list3.Count;
            nRow = 0;
            strRemark = "";
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list3[i].EXCODE;                        //검사코드
                strResult = list3[i].RESULT.Trim();                 //검사실 결과값
                strResCode = list3[i].RESCODE.Trim();               //결과값 코드
                strResultType = list3[i].RESULTTYPE.Trim();         //결과값 TYPE
                strGbCodeUse = list3[i].GBCODEUSE.Trim();            //결과값코드 사용여부

                //SS2에 검사실 결과값을 DISPLAY
                SS2.ActiveSheet.Cells[i, 0].Text = " " + list3[i].HNAME.Trim();
                SS2.ActiveSheet.Cells[i, 1].Text = strResult;
                //비만도
                if (strExCode == "A103")
                {
                    strResCode = "061";
                }

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        strResName = hb.READ_ResultName(strResCode, strResult);
                        SS2.ActiveSheet.Cells[i, 1].Text = hb.READ_ResultName(strResCode, strResult);
                        if (strResName.Length > 7)
                        {
                            strRemark += "▷" + list3[i].HNAME.Trim() + ":";
                            strRemark += strResName + "\r\n";
                        }
                    }
                }
                else if (strResult.Length > 7)
                {
                    strRemark += "▷" + list3[i].HNAME.Trim() + ":";
                    strRemark += strResName + "\r\n";
                }

                if (list3[i].PANJENG.Trim() == "2")
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = "*";
                }

                //참고치를 Dispaly
                strNomal = hm.EXAM_NomalValue_SET(strExCode, strJepDate, FstrSex, list3[i].MIN_M, list3[i].MAX_M, list3[i].MIN_F, list3[i].MAX_F);

                SS2.ActiveSheet.Cells[i, 3].Text = strNomal;
                SS2.ActiveSheet.Cells[i, 7].Text = strExCode;
                SS2.ActiveSheet.Cells[i, 8].Text = strResult; //정상값 점검용
                strExPan = list3[i].PANJENG.Trim();
                //판정결과별 바탕색상을 다르게 표시함
                switch (strExPan)
                {
                    case "B":
                        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 210, 222);  //정상B
                        break;
                    case "R":
                        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 170, 170);  //질환의심(R)
                        break;
                    default:
                        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(190, 250, 220);  //정상A 또는 기타
                        break;
                }

                //간염검사
                if (strExCode == "A131")     //간염항원
                {
                    nGan1 = int.Parse(strResult);
                }
                else if (strExCode == "A132")     //간염항체
                {
                    nGan2 = int.Parse(strResult);
                }
            }

            if (!strRemark.IsNullOrEmpty())
            {
                txtResult1.Text = strRemark;
            }

            //Screen_Munjin_Display
            //문진표를 Display
            int nJil = 0;
            int nGajok = 0;

            //건강검진 문진표 및 결과를  READ
            COMHPC list4 = comHpcLibBService.GetHic_X_MunjinbyWrtNo(FnWRTNO);

            if (list4 == null)
            {
                MessageBox.Show("접수번호 " + FnWRTNO + " 는 문진 및 판정이 등록 안됨", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //문진
            if (list4.XP1.Trim() == "Y")
            {
                rdo1.Checked = true;
            }
            else if (list4.XP1.Trim() == "N")
            {
                rdo2.Checked = true;
            }

            if (list4.JINGBN.Trim() == "Y")
            {
                rdoGubun1.Checked = true;
            }
            else if (list4.JINGBN.Trim() == "N")
            {
                rdoGubun2.Checked = true;
            }

            if (!list4.PAN.IsNullOrEmpty())
            {
                cboPanjeng.SelectedIndex = int.Parse(VB.Left(list4.PAN.Trim(), 1));
            }
            else
            {
                cboPanjeng.SelectedIndex = 0;
            }
            txtSogenRemark.Text = list4.SOGEN.Trim();

            txtXJong.Text = list4.XPJONG.Trim();
            txtRemark.Text = list4.XREMARK.Trim();
            txtPlace.Text = list4.XPLACE.Trim();
            txtTerm.Text = list4.XTERM.Trim();
            txtXTerm.Text = list4.XTERM1.Trim();
            txtMuch.Text = list4.XMUCH.Trim();
            txtJung.Text = list4.XJUNGSAN.Trim();
            txtMun1.Text = list4.MUN1.Trim();
            txtEye.Text = list4.JUNGSAN1.Trim();
            txtSkin.Text = list4.JUNGSAN2.Trim();
            txtEtc.Text = list4.JUNGSAN3.Trim();
            txtPanDrNo1.Text = list4.MUNDRNO.Trim();

            txtJobYN.Text = list4.WORKYN.Trim();
            txtSahu.Text = list4.SAHUCODE.Trim();
            lblJobYN.Text = hb.READ_HIC_CODE("13", txtJobYN.Text.Trim());     //업무접합성
            lblSahuName.Text = hm.Sahu_Names_Display(txtSahu.Text.Trim());    //사후관리

            //판정
            txtJochiRemark.Text = list4.PANJENG.Trim();
            if (!list4.PANJENGDATE.ToString().IsNullOrEmpty())
            {
                txtPanDate.Text = list4.PANJENGDATE.ToString();
            }
            else
            {
                txtPanDate.Text = clsPublic.GstrSysDate;
            }
            nLicense = list4.PANJENGDRNO;               //의사면허번호
            txtPanDrNo.Text = "";
            lblDrName.Text = "";
            if (nLicense > 0)
            {
                txtPanDrNo.Text = nLicense.ToString();
                lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
            }
            else
            {
                txtPanDrNo.Text = clsHcVariable.GnHicLicense.ToString();
                lblDrName.Text = hb.READ_License_DrName(clsHcVariable.GnHicLicense);
            }

            //2013-11-25 방사선종사자 컬럼 추가
            nJil = int.Parse(list4.JILBYUNG.Trim());             //과거 질병력
            rdoX11.Checked = true;

            chkX1_11.Checked = list4.BLOOD1 == "1" ? true : false; //혈액관련질환(빈혈)
            chkX1_12.Checked = list4.BLOOD2 == "1" ? true : false; //혈액관련질환(백혈병)
            if (!list4.BLOOD3.IsNullOrEmpty())       //혈액관련질환(기타)
            {
                chkX1_13.Checked = true;
                txtX1_1.Text = list4.BLOOD3.Trim();
            }
            chkX1_21.Checked = list4.SKIN1 == "1" ? true : false;  //피부질환(아토피)
            chkX1_22.Checked = list4.SKIN2 == "1" ? true : false;  //피부질환(습진)
            if (!list4.SKIN3.IsNullOrEmpty())        //피부질환(기타)
            {
                chkX1_23.Checked = true;
                txtX1_2.Text = list4.SKIN3.Trim();
            }
            txtX1_3.Text = list4.NERVOUS1.Trim();                   //신경계질환명
            chkX1_41.Checked = list4.EYE1 == "1" ? true : false;    //눈 질환(백내장)
            if (!list4.EYE2.IsNullOrEmpty())         //눈 질환(기타)
            {
                chkX1_42.Checked = true;
                txtX1_4.Text = list4.EYE2.Trim();
            }
            txtX1_5.Text = list4.CANCER1.Trim();     //암 질환명

            nGajok = int.Parse(list4.GAJOK.Trim());              //가족력
            rdoX21.Checked = true;

            txtX2_1.Text = list4.BLOOD.Trim();          //혈액관련질환명
            txtX2_2.Text = list4.NERVOUS2.Trim();       //신경계질환명
            txtX2_3.Text = list4.CANCER2.Trim();        //암 질환명
            txtXSympton.Text = list4.SYMPTON.Trim();    //최근 특이증상

            chkX41.Checked = list4.JIKJONG1 == "1" ? true : false;  //현재 직종(비파괴검사)
            chkX42.Checked = list4.JIKJONG2 == "1" ? true : false;  //현재 직종(방사선사)
            if (!list4.JIKJONG3.IsNullOrEmpty())         //눈 질환(기타)
            {
                chkX43.Checked = true;
                txtJikjong.Text = list4.JIKJONG3.Trim();
            }

            txtRemark2.Text = list4.SANGDAM.Trim();

            //종전결과 3개를 Display
            fn_OLD_Result_Display(long.Parse(FstrPano), strJepDate, strSex);

            tabControl1.SelectedTab = tab11;
            tabControl2.SelectedTab = tab21;
            tabControl3.SelectedTab = tab31;
        }

        void fn_OLD_Result_Display(long argPano, string argJepDate, string argSex)
        {
            // 검사항목을 Setting
            strExamCode.Clear();

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                if (!SS2.ActiveSheet.Cells[i, 7].Text.IsNullOrEmpty())
                {
                    strExamCode.Add(SS2.ActiveSheet.Cells[i, 7].Text.Trim());
                }
            }

            //1차검사 종전 접수번호를 읽음
            List<HIC_JEPSU_LTD_RES_BOHUM1> list = hicJepsuLtdResBohum1Service.GetItembyPaNoJepDateGjJong(argPano, argJepDate);

            nOldCNT = list.Count;
            strAllWRTNO = "";
            if (nOldCNT > 2) nOldCNT = 2;
            for (int i = 0; i < nOldCNT; i++)
            {
                strAllWRTNO += list[i].WRTNO.ToString() + ",";
                strJepDate = list[i].JEPDATE.ToString();
                SS2_Sheet1.ColumnHeader.Cells.Get(0, i + 5).Value = VB.Left(strJepDate, 4) + VB.Mid(strJepDate, 6, 2) + VB.Right(strJepDate, 2);
                fn_OLD_Result_Display_SUB(list[i].WRTNO, strExamCode, argSex, i);
                if (i >= 2) break;
            }
        }

        /// <summary>
        /// 종전 1회 검사의 결과를 Display
        /// </summary>
        void fn_OLD_Result_Display_SUB(long nWrtNo, List<string> argExamCode, string argSex, int index)
        {
            int nRow = 0;

            ///TODO : 이상훈(2019.10.31) clsHcMain.cs GET_HIC_JepsuDate Method 확인 필요
            //판정결과를 strRemark에 보관
            //strOldJepsuDate = hm.GET_HIC_JepsuDate(nWrtNo);

            //검사항목 및 결과를 READ
            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyWrtNoNewExCode(nWrtNo, argExamCode, "N");

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list[i].EXCODE;                 //검사코드
                strResult = list[i].RESULT;                 //검사실 결과값
                strResCode = list[i].RESCODE;               //결과값 코드
                strResultType = list[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list[i].GBCODEUSE;           //결과값코드 사용여부

                //해당검사코드가 있는 Row를 찾음
                nRow = 0;
                for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                {
                    if (SS2.ActiveSheet.Cells[i, 7].Text.Trim() == strExCode)
                    {
                        nRow = j;
                        break;
                    }
                }

                //해당검사가 시트에 있으면 결과를 표시함
                if (nRow > 0)
                {
                    SS2.ActiveSheet.Cells[nRow, i + 5].Text = strResult;
                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResName = hb.READ_ResultName(strResCode, strResult);
                            SS2.ActiveSheet.Cells[nRow, i + 5].Text = strResName;
                            if (strResName.Length > 7)
                            {
                                strRemark += "▷" + list[i].HNAME.Trim() + ": ";
                                strRemark += strResName + "\r\n";
                            }
                        }
                        else if (strResult.Length > 7)
                        {
                            strResult += "▷" + list[i].HNAME.Trim() + ": ";
                            strRemark += strResult + "\r\n";
                        }
                        SS2.ActiveSheet.Cells[nRow, i + 10].Text = strResult;   //정상값 점검용
                        strExPan = hm.ExCode_Result_Panjeng(strExCode, strResult, argSex, strOldJepDate, "");
                        //판정결과별 바탕색상을 다르게 표시함
                        switch (strExPan)
                        {
                            case "B":
                                SS2.ActiveSheet.Cells[nRow, i + 5].BackColor = Color.FromArgb(250, 210, 222);   //정상B
                                break;
                            case "R":
                                SS2.ActiveSheet.Cells[nRow, i + 5].BackColor = Color.FromArgb(250, 170, 170);   //질환의심(R)
                                break;
                            default:
                                SS2.ActiveSheet.Cells[nRow, i + 5].BackColor = Color.FromArgb(190, 250, 220);   //정상A 또는 기타
                                break;
                        }
                    }
                }
            }

            if (index == 0)
            {
                txtResult21.Text = strRemark;
            }
            else
            {
                txtResult22.Text = strRemark;
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

            nHeaPano = hicPatientService.GetPanobyJumin(clsAES.AES(FstrJumin));

            //일반건진, 종합검진의 접수내역을 Display
            List<HIC_JEPSU> list = hicJepsuService.GetItembyOnlyPaNo(FnPano, nHeaPano);

            nREAD = list.Count;
            SSHistory.ActiveSheet.RowCount = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                strJong = list[i].GJJONG.Trim();

                SSHistory.ActiveSheet.Cells[i, 0].Text = list[i].JEPDATE;
                if (strJong == "XX")
                {
                    SSHistory.ActiveSheet.Cells[i, 1].Text = "종검";
                }
                else
                {
                    SSHistory.ActiveSheet.Cells[i, 1].Text = hb.READ_GjJong_Name(strJong);
                }
                SSHistory.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.ToString();
                SSHistory.ActiveSheet.Cells[i, 3].Text = list[i].GJJONG;
                SSHistory.ActiveSheet.Cells[i, 4].Text = list[i].GJCHASU;
            }
        }

        string READ_BAS_ILLS(string argCode)
        {
            string rtnVal = "";

            if (argCode.IsNullOrEmpty())
            {
                return rtnVal;
            }

            rtnVal = basIllsService.GetIllNameKbyIllCode(argCode);

            return rtnVal;
        }

        void fn_DB_Update_Panjeng(string argGbn)
        {
            int nBCnt = 0;
            int nRCNT = 0;
            string strPanjeng = "";
            string strGbErFlag = "";
            int result = 0;

            if (txtPanDrNo.Text.IsNullOrEmpty())
            {
                MessageBox.Show("판정의사공란", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                clsHcVariable.GnHicLicense = long.Parse(txtPanDrNo.Text);
            }

            txtJochiRemark.Text = txtJochiRemark.Text.Trim();
            if (txtPanDate.Text.IsNullOrEmpty())
            {
                txtPanDate.Text = clsPublic.GstrSysDate;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            //판정결과를 DB에 UPDATE
            COMHPC item = new COMHPC();

            item.PAN = VB.Left(cboPanjeng.Text, 1);
            item.STS = "Y";
            item.SOGEN = txtSogenRemark.Text.Trim().Replace("'", "`");
            item.WORKYN = txtJobYN.Text.Trim();
            item.SAHUCODE = txtSahu.Text.Trim();
            item.PANJENG = txtJochiRemark.Text.Trim().Replace("'", "`");
            item.PANJENGDATE = txtPanDate.Text.Trim();
            item.PANJENGDRNO = clsHcVariable.GnHicLicense;
            item.WRTNO = FnWRTNO;

            result = comHpcLibBService.UpdateHic_X_MunjinResult(item);

            if (result < 0)
            {
                MessageBox.Show("판정결과 DB에 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void eComboBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void eComboBoxClick(object sender, EventArgs e)
        {
            if (VB.Left(cboPanjeng.Text, 1) == "1")
            {
                txtSogenRemark.Text = "정상";
                SendKeys.Send("{TAB}");
            }
            else
            {
                txtSogenRemark.Text = "";
                txtPanDate.Text = clsPublic.GstrSysDate;
            }
        }

        void fn_OLD_Panjeng_Display(int argNo, long argWrtNo)
        {
            string strPan = "";
            string strPAN1 = "";

            if (argNo == 0)
            {
                txtResult21.Text = "";
            }
            else
            {
                txtResult22.Text = "";
            }

            //건강검진 문진표 및 결과를  READ
            HIC_RES_BOHUM1 list = hicResBohum1Service.GetItemByWrtno(argWrtNo);

            if (list == null)
            {
                return;
            }

            //판정결과,판정일자,판정의사
            strPan = "▶판정결과:";
            switch (list.PANJENG)
            {
                case "1":
                    strPan += "정상A";
                    break;
                case "2":
                    strPan += "정상B";
                    break;
                case "3":
                    strPan += "질환의심(R)";
                    break;
                case "5":
                    strPan += "정상B+질환의심";
                    break;
                default:
                    strPan += "<오류>";
                    break;
            }
            strPan += " ○판정일자:" + list.PANJENGDATE;
            strPan += " ○판정의사:" + hb.READ_License_DrName(list.PANJENGDRNO) + "\r\n";

            //판정(B)
            strPAN1 = "";

            if (list.PANJENGB1 == "1")
            {
                strPAN1 += "◎비만관리 ";
            }
            if (list.PANJENGB2 == "1")
            {
                strPAN1 += "◎혈압관리 ";
            }
            if (list.PANJENGB3 == "1")
            {
                strPAN1 += "◎콜레스테롤관리 ";
            }
            if (list.PANJENGB4 == "1")
            {
                strPAN1 += "◎간기능관리 ";
            }
            if (list.PANJENGB5 == "1")
            {
                strPAN1 += "◎당뇨관리 ";
            }
            if (list.PANJENGB6 == "1")
            {
                strPAN1 += "◎신장기능관리 ";
            }
            if (list.PANJENGB7 == "1")
            {
                strPAN1 += "◎빈혈관리 ";
            }
            if (list.PANJENGB8 == "1")
            {
                strPAN1 += "◎부인과질환관리 ";
            }

            if (!strPAN1.IsNullOrEmpty())
            {
                strPan += "▶판정(정상B): " + strPAN1 + "\r\n";
            }

            //판정(R)
            strPAN1 = "";
            if (list.PANJENGR1 == "1")
            {
                strPAN1 += "◎폐결핵의심 ";
            }
            if (list.PANJENGR2 == "1")
            {
                strPAN1 += "◎기타흉부질환의심 ";
            }
            if (list.PANJENGR3 == "1")
            {
                strPAN1 += "◎고혈압의심 ";
            }
            if (list.PANJENGR4 == "1")
            {
                strPAN1 += "◎고지혈증의심 ";
            }
            if (list.PANJENGR5 == "1")
            {
                strPAN1 += "◎간장질환의심 ";
            }
            if (list.PANJENGR6 == "1")
            {
                strPAN1 += "◎당뇨질환의심 ";
            }
            if (list.PANJENGR7 == "1")
            {
                strPAN1 += "◎신장질환의심 ";
            }
            if (list.PANJENGR8 == "1")
            {
                strPAN1 += "◎빈혈증의심 ";
            }
            if (list.PANJENGR9 == "1")
            {
                strPAN1 += "◎부인과질환의심 ";
            }
            if (list.PANJENGR10 == "1")
            {
                strPAN1 += "◎자궁경부암의심 ";
            }
            if (list.PANJENGR11 == "1")
            {
                strPAN1 += "◎기타질환의심 ";
            }

            if (!strPAN1.IsNullOrEmpty())
            {
                strPan += "▶판정(질환의심): " + strPAN1 + "\r\n";
            }

            //소견 및 조치사항
            if (!list.SOGEN.IsNullOrEmpty())
            {
                strPan += "▶소견및조치사항: " + list.SOGEN.Trim() + "\r\n";
            }
            //간염검사
            if (!list.LIVER3.IsNullOrEmpty())
            {
                switch (list.LIVER3.Trim())
                {
                    case "1":
                        strPan += "▶간염검사: 보균자" + "\r\n";
                        break;
                    case "2":
                        strPan += "▶간염검사: 면역자" + "\r\n";
                        break;
                    case "3":
                        strPan += "▶간염검사: 접종대상자" + "\r\n";
                        break;
                    default:
                        break;
                }
            }
            //자궁경부 선상피세포 유/무
            if (list.WOMB02.Trim() == "1")
            {
                strPan += "▶자궁경부 선상피세표 있음" + "\r\n";
            }

            if (argNo == 0)
            {
                txtResult21.Text = strPan;
            }
            else
            {
                txtResult22.Text = strPan;
            }
        }
    }
}
