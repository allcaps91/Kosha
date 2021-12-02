using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanTeethJudgment.cs
/// Description     : 구강검사 문진표 및 판정
/// Author          : 이상훈
/// Create Date     : 2019-11-13
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm구강문진판정.frm(Frm구강문진판정)" />

namespace HC_Pan
{
    public partial class frmHcPanTeethJudgment : Form
    {
        HicJepsuService hicJepsuService = null;
        HicResDentalService hicResDentalService = null;
        HicExjongService hicExjongService = null;
        HicSunapdtlService hicSunapdtlService = null;

        frmHcLtdHelp frmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FstrROWID;
        long FnWRTNO;
        string FstrSex;
        string FstrJepDate;

        public frmHcPanTeethJudgment(long nWrtNo)
        {
            InitializeComponent();

            FnWRTNO = nWrtNo;

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicResDentalService = new HicResDentalService();
            hicExjongService = new HicExjongService();
            hicSunapdtlService = new HicSunapdtlService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnNoCounselView.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnMenuSave.Click += new EventHandler(eBtnClick);
            this.btnMenuDelete.Click += new EventHandler(eBtnClick);
            this.btnMenuCancel.Click += new EventHandler(eBtnClick);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);

            this.txtDntStatus.GotFocus += new EventHandler(etxtGotFocus);
            this.txtDntStatus.GotFocus += new EventHandler(etxtGotFocus);
            this.txtFunc1.GotFocus += new EventHandler(etxtGotFocus);
            this.txtHabit1.GotFocus += new EventHandler(etxtGotFocus);
            this.txtHabit4.GotFocus += new EventHandler(etxtGotFocus);
            this.txtMunjinEtc.GotFocus += new EventHandler(etxtGotFocus);
            this.txtOpdDnt.GotFocus += new EventHandler(etxtGotFocus);
            this.txtStat1.GotFocus += new EventHandler(etxtGotFocus);
            this.txtStat2.GotFocus += new EventHandler(etxtGotFocus);
            this.txtCardio.GotFocus += new EventHandler(etxtGotFocus);
            this.txtHabit5.GotFocus += new EventHandler(etxtGotFocus);
            this.txtHabit6.GotFocus += new EventHandler(etxtGotFocus);
            this.txtHabit7.GotFocus += new EventHandler(etxtGotFocus);
            this.txtHabit8.GotFocus += new EventHandler(etxtGotFocus);
            this.txtHabit9.GotFocus += new EventHandler(etxtGotFocus);

            this.txtDntStatus.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtOpdDnt.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCardio.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHabit5.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHabit6.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHabit7.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHabit8.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHabit9.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtFunc1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHabit1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHabit2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHabit4.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txtHabit5.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHabit6.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHabit7.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHabit8.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHabit9.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txtJilbyung.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtMunjinEtc.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtPanjengJochi.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtStat1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtStat2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            txtLtdCode.Text = "";
            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-20).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;
            sp.Spread_All_Clear(ssList);
            fn_Screen_Clear();

            List<HIC_EXJONG> list = hicExjongService.Read_ExJong_Add(true);

            if (list.Count > 0)
            {
                cboJong.Items.Clear();
                cboJong.Items.Add("**.전체");
                for (int i = 0; i < list.Count; i++)
                {
                    cboJong.Items.Add(list[i].CODE.Trim() + "." + list[i].NAME.Trim());
                }
                cboJong.SelectedIndex = 0;
            }

            //hb.ComboJong_Set(cboJong);

            eBtnClick(btnSearch, new EventArgs());

            if (FnWRTNO > 0)
            {
                txtWrtNo.Text = FnWRTNO.ToString();
                fn_Screen_Display();
            }

            //판정프로그램에서 접근시 Data 수정불가 조회만가능
            if (clsHcVariable.GstrTempValue == "1")
            {
                btnMenuSave.Enabled = false;
                btnMenuDelete.Enabled = false;
            }
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
                frmHcLtdHelp frm = new frmHcLtdHelp();
                frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                frm.ShowDialog();
                frm.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!txtLtdCode.Text.Trim().IsNullOrEmpty())
                {
                    if (txtLtdCode.Text.IndexOf(".") == -1)
                    {
                        txtLtdCode.Text = txtLtdCode.Text.Trim() + "." + hb.READ_Ltd_Name(txtLtdCode.Text.Trim());
                    }
                    else
                    {
                        txtLtdCode.Text = VB.Pstr(txtLtdCode.Text.Trim(), ".", 1) + "." + hb.READ_Ltd_Name(VB.Pstr(txtLtdCode.Text.Trim(), ".", 1));
                    }
                }
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                string strList = "";
                int result = 0;
                string strFrDate = "";
                string strToDate = "";
                string strJob = "";
                string strJong = "";
                long nLtdCode = 0;

                Cursor.Current = Cursors.WaitCursor;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                strJong = VB.Left(cboJong.Text, 2);

                sp.Spread_All_Clear(ssList);
                ssList.ActiveSheet.RowCount = 50;

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
                    if (txtLtdCode.Text.IndexOf(".") == -1)
                    {
                        txtLtdCode.Text = txtLtdCode.Text.Trim() + "." + hb.READ_Ltd_Name(txtLtdCode.Text.Trim());
                    }
                }

                result = hicJepsuService.UpdateGbDentalbyWrtno(strFrDate, strToDate);

                if (result < 0)
                {
                    MessageBox.Show("구강검사 여부 갱신중 오류발생!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //자료를 SELECT
                List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDate(strFrDate, strToDate, strJob, strJong, nLtdCode);

                nREAD = list.Count;
                ssList.ActiveSheet.RowCount = nREAD;
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    ssList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.ToString();
                    ssList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    ssList.ActiveSheet.Cells[i, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                    if (rdoJob1.Checked == true)
                    {
                        //미판정자는 붉은색으로 표시
                        if (hicResDentalService.GetPanjengDrNobyWrtNo(list[i].WRTNO) == 0)
                        {
                            ssList.ActiveSheet.Cells[i, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HCFD3FC"));
                        }
                    }
                    ssList.ActiveSheet.Cells[i, 2].Text = list[i].JEPDATE;
                    ssList.ActiveSheet.Cells[i, 3].Text = list[i].GJJONG;
                    progressBar1.Value = i + 1;
                }

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnNoCounselView)
            {
                int nREAD = 0;
                int nRow = 0;
                string strList = "";
                string strOK = "";
                string strFrDate = "";
                string strToDate = "";
                string strJob = "";
                string strJong = "";
                long nLtdCode = 0;

                Cursor.Current = Cursors.WaitCursor;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                strJong = VB.Left(cboJong.Text, 2);
                strJob = "2";

                rdoJob1.Checked = true;
                sp.Spread_All_Clear(ssList);
                ssList.ActiveSheet.RowCount = 50;

                if (!txtLtdCode.Text.IsNullOrEmpty())
                {
                    if (txtLtdCode.Text.IndexOf(".") == -1)
                    {
                        txtLtdCode.Text = txtLtdCode.Text.Trim() + "." + hb.READ_Ltd_Name(txtLtdCode.Text.Trim());
                    }
                    else
                    {
                        txtLtdCode.Text = VB.Pstr(txtLtdCode.Text.Trim(), ".", 1) + "." + hb.READ_Ltd_Name(VB.Pstr(txtLtdCode.Text.Trim(), ".", 1));
                    }
                }

                //자료를 SELECT
                List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDate(strFrDate, strToDate, strJob, strJong, nLtdCode);

                nREAD = list.Count;
                ssList.ActiveSheet.RowCount = nREAD;
                if (nREAD == 0)
                {
                    MessageBox.Show("미상담자가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ssList.ActiveSheet.RowCount = nREAD;
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    strOK = "";
                    //미판정자는 붉은색으로 표시
                    HIC_RES_DENTAL list2 = hicResDentalService.GetPanjengDrnoDate(list[i].WRTNO);
                    if (!list2.IsNullOrEmpty())
                    {
                        if (list2.PANJENGDRNO == 0)
                        {
                            strOK = "OK";
                        }

                        if (list2.PANJENGDATE.IsNullOrEmpty())
                        {
                            strOK = "OK";
                        }
                    }

                    if (strOK == "OK")
                    {
                        nRow += 1;
                        if (nRow > ssList.ActiveSheet.RowCount)
                        {
                            ssList.ActiveSheet.RowCount = nRow;
                        }
                        ssList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].WRTNO.ToString();
                        ssList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME;
                        ssList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].JEPDATE.ToString();
                        ssList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].GJJONG;
                    }
                    progressBar1.Value = i + 1;
                }
                ssList.ActiveSheet.RowCount = nRow;

                if (nRow == 0)
                {
                    MessageBox.Show("미상담자가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnMenuDelete)
            {
                string strMsg = "";
                int result = 0;

                strMsg = "정말로 문진표 및 판정내역을" + "\r\n";
                strMsg += "삭제 하시겠습니까?";

                if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                //문진내역,보험청구내역을 삭제
                result = hicResDentalService.DeletebyWrtNo(FnWRTNO);

                if (result < 0)
                {
                    MessageBox.Show("문진내역을 삭제시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //접수의 문진결과 등록 Flag를 미등록으로 변경
                result = hicResDentalService.UpdatebyWrtNo(FnWRTNO);

                if (result < 0)
                {
                    MessageBox.Show("접수마스타에 문진미등록 FLAG변경시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                eBtnClick(btnSearch, new EventArgs());
                fn_Screen_Clear();
                txtWrtNo.Focus();
            }
            else if (sender == btnMenuSave)
            {
                //문진
                string strOpdDNT = "";
                string strJilbyung1 = "";
                string strJilbyung2 = "";
                string strFunc1 = "";
                string strStat1 = "";
                string strStat2 = "";
                string strDntStatus = "";
                string strHabit1 = "";
                string strHabit2 = "";
                string strHabit4 = "";
                string strHabit5 = "";
                string strHabit6 = "";
                string strHabit7 = "";
                string strHabit8 = "";
                string strHabit9 = "";
                string strMunjinEtc = "";
                string strJepDate = "";

                int result = 0;

                if (txtWrtNo.Text.Trim() == "")
                {
                    MessageBox.Show("저장실패", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //변수 clear
                strMunjinEtc = "";
                //1.구강문진항목
                //구강건강인식
                strOpdDNT = txtOpdDnt.Text;         //(1)
                strJilbyung1 = txtJilbyung.Text.Trim(); //(2)
                strJilbyung2 = txtCardio.Text.Trim(); //(3)
                strFunc1 = txtFunc1.Text.Trim(); //(4)
                strStat1 = txtStat1.Text.Trim(); //(5)
                strStat2 = txtStat2.Text.Trim(); //(6)
                strDntStatus = txtDntStatus.Text.Trim(); //(7)
                strHabit5 = txtHabit5.Text.Trim(); //(8)
                strHabit2 = txtHabit2.Text.Trim(); //(9)
                strHabit6 = txtHabit6.Text.Trim(); //(10)
                strHabit4 = txtHabit4.Text.Trim(); //(11)
                strHabit7 = txtHabit7.Text.Trim(); //(12)
                strHabit8 = txtHabit8.Text.Trim(); //(13)
                strHabit9 = txtHabit9.Text.Trim(); //(14)
                strHabit1 = txtHabit1.Text.Trim(); //(15)
                strMunjinEtc = txtMunjinEtc.Text.Trim(); //(16)


                if (lblOpdDnt.Text.IsNullOrEmpty()) { MessageBox.Show("1번 항목 문진 누락 또는 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (lblJilbyung.Text.IsNullOrEmpty()) { MessageBox.Show("2번 항목 문진 누락 또는 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return;}
                if (lblCardioStatus.Text.IsNullOrEmpty()) {MessageBox.Show("3번 항목 문진 누락 또는 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return;}
                if (lblFunc1.Text.IsNullOrEmpty()) {MessageBox.Show("4번 항목 문진 누락 또는 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return;}
                if (lblStat1.Text.IsNullOrEmpty()) {MessageBox.Show("5번 항목 문진 누락 또는 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return;}
                if (lblStat2.Text.IsNullOrEmpty()) { MessageBox.Show("6번 항목 문진 누락 또는 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return;}
                if (lblDntStatus.Text.IsNullOrEmpty()) { MessageBox.Show("7번 항목 문진 누락 또는 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (lblHabit5.Text.IsNullOrEmpty()) { MessageBox.Show("8번 항목 문진 누락 또는 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (lblHabit6.Text.IsNullOrEmpty()) {MessageBox.Show("10번 항목 문진 누락 또는 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (lblHabit4.Text.IsNullOrEmpty()) {MessageBox.Show("11번 항목 문진 누락 또는 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (lblHabit7.Text.IsNullOrEmpty()) {MessageBox.Show("12번 항목 문진 누락 또는 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (lblHabit8.Text.IsNullOrEmpty()) {MessageBox.Show("13번 항목 문진 누락 또는 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (lblHabit9.Text.IsNullOrEmpty()) {MessageBox.Show("14번 항목 문진 누락 또는 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (lblHabit1.Text.IsNullOrEmpty()) {MessageBox.Show("15번 항목 문진 누락 또는 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                HIC_RES_DENTAL item = new HIC_RES_DENTAL();

                item.WRTNO = FnWRTNO;
                item.OPDDNT = strOpdDNT;
                item.DNTSTATUS = strDntStatus;
                item.T_HABIT1 = strHabit1;
                item.T_HABIT2 = strHabit2.To<long>();
                item.T_HABIT4 = strHabit4;
                item.T_HABIT5 = strHabit5;
                item.T_HABIT6 = strHabit6;
                item.T_HABIT7 = strHabit7;
                item.T_HABIT8 = strHabit8;
                item.T_HABIT9 = strHabit9;
                item.T_STAT1 = strStat1;
                item.T_STAT2 = strStat2;
                item.T_FUNCTION1 = strFunc1;
                item.T_JILBYUNG1 = strJilbyung1;
                item.T_JILBYUNG2 = strJilbyung2;
                item.MUNJINETC = strMunjinEtc;

                clsDB.setBeginTran(clsDB.DbCon);

                //자료저장 및 갱신
                if (FstrROWID == "")
                {
                    result = hicResDentalService.Insert(item);
                }
                else
                {
                    result = hicResDentalService.UpdateAll(item);
                }

                if (result < 0)
                {
                    MessageBox.Show("구강검사 저장중 오류발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon); 
                    return;
                }

                //접수마스타에 문진 등록 완료 SET
                result = hicJepsuService.UpdateGbMinjin2byWrtNo(FnWRTNO);

                if (result < 0)
                {
                    MessageBox.Show("접수마스타에 문진완료 등록중 오류가 발생함!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                //구강검진 문진결과로 문진표평가 및 종합판정,결과해석을 업데이트
                hm.Munjin_Result_Update(FnWRTNO, FstrJepDate);

                fn_Screen_Clear();
                eBtnClick(btnSearch, new EventArgs());
                txtWrtNo.Focus();
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssList)
            {
                fn_Screen_Clear();
                txtWrtNo.Text = ssList.ActiveSheet.Cells[e.Row, 0].Text;
                fn_Screen_Display();
                txtOpdDnt.Focus();
            }
        }

        void etxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtDntStatus)
            {
                lblMsg.Text = "1.매우좋음 2.좋음 3.보통 4.나쁨 5.매우나쁨";
            }
            else if (sender == txtFunc1)
            {
                lblMsg.Text = "1.예     2.아니오";
            }
            else if (sender == txtHabit1)
            {
                lblMsg.Text = "1.전혀 피운 적이 없다  2.현재 피우고 있다  3.이전에 피웠으나 끊었다 ";
            }
            else if (sender == txtHabit4)
            {
                lblMsg.Text = "1.항상 했다  2.대부분 했다  3.가끔했다  4.전혀 하지 않았다  5.치실 혹은 치간속이 무엇인지 모른다";
            }
            else if (sender == txtMunjinEtc)
            {
                lblMsg.Text = "";
            }
            else if (sender == txtOpdDnt)
            {
                lblMsg.Text = "1.예 2.아니오";
            }
            else if (sender == txtStat1)
            {
                lblMsg.Text = "1.예     2.아니오";
            }
            else if (sender == txtStat2)
            {
                lblMsg.Text = "1.예     2.아니오";
            }
            else if (sender == txtCardio)
            {
                lblMsg.Text = "1.예   2.아니오   3.모르겠다";
            }
            else if (sender == txtHabit5)
            {
                lblMsg.Text = "1.예   2.아니오";
            }
            else if (sender == txtHabit6)
            {
                lblMsg.Text = "1.항상했다(7회) 2.대부분했다(4~6회) 3.가끔했다(1~3회 4.전혀안함";
            }
            else if (sender == txtHabit7)
            {
                lblMsg.Text = "1.예   2.아니오   3.모르겠다";
            }
            else if (sender == txtHabit8)
            {
                lblMsg.Text = "1.먹지않음 2.1번 3.2~3번 4.4번이상 5.모르겠음";
            }
            else if (sender == txtHabit9)
            {
                lblMsg.Text = "1.먹지않음 2.1번 3.2~3번 4.4번이상 5.모르겠음";
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtWrtNo)
                {
                    if (txtWrtNo.Text.To<long>() == 0) return;
                    fn_Screen_Display();
                    txtOpdDnt.Focus();
                }
                else
                {
                    if (sender == txtDntStatus)
                    {
                        lblDntStatus.Text = hm.READ_Munjin_Name("DENTAL", "DNTSTATUS", txtDntStatus.Text);
                        if (!txtDntStatus.Text.IsNullOrEmpty() && lblDntStatus.Text == "")
                        {
                            MessageBox.Show("입력값 오류 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtDntStatus.Focus();
                            return;
                        }
                        SendKeys.Send("{TAB}");
                    }
                    else if (sender == txtFunc1)
                    {
                        lblFunc1.Text = hm.READ_Munjin_Name("DENTAL", "예아니오", txtFunc1.Text);
                        if (!txtFunc1.Text.IsNullOrEmpty() && lblFunc1.Text == "")
                        {
                            MessageBox.Show("입력값 오류 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtFunc1.Focus();
                            return;
                        }
                        SendKeys.Send("{TAB}");
                    }
                    else if (sender == txtHabit1)
                    {
                        lblHabit1.Text = hm.READ_Munjin_Name("DENTAL", "구강1", txtHabit1.Text);
                        if (txtHabit1.Text.IsNullOrEmpty() && lblHabit1.Text == "")
                        {
                            MessageBox.Show("입력값 오류 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtHabit1.Focus();
                            return;
                        }
                        SendKeys.Send("{TAB}");
                    }
                    else if (sender == txtHabit4)
                    {
                        lblHabit4.Text = hm.READ_Munjin_Name("DENTAL", "구강3", txtHabit4.Text);
                        if (!txtHabit4.Text.IsNullOrEmpty() && lblHabit4.Text == "")
                        {
                            MessageBox.Show("입력값 오류 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtHabit4.Focus();
                            return;
                        }
                        txtHabit7.Focus();
                    }
                    else if (sender == txtHabit5)
                    {
                        lblHabit5.Text = hm.READ_Munjin_Name("DENTAL", "예아니오", txtHabit5.Text);
                        if (txtHabit5.Text.IsNullOrEmpty() && lblHabit5.Text == "")
                        {
                            MessageBox.Show("입력값 오류 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtHabit5.Focus();
                            return;
                        }
                        txtHabit2.Focus();
                    }
                    else if (sender == txtHabit2)
                    {
                        txtHabit6.Focus();
                    }
                    else if (sender == txtHabit6)
                    {
                        lblHabit6.Text = hm.READ_Munjin_Name("DENTAL", "구강6", txtHabit6.Text);
                        if (txtHabit6.Text.IsNullOrEmpty() && lblHabit6.Text == "")
                        {
                            MessageBox.Show("입력값 오류 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtHabit6.Focus();
                            return;
                        }
                        txtHabit4.Focus();
                    }
                    else if (sender == txtHabit7)
                    {
                        lblHabit7.Text = hm.READ_Munjin_Name("DENTAL", "구강5", txtHabit7.Text);
                        if (txtHabit7.Text.IsNullOrEmpty() && lblHabit7.Text == "")
                        {
                            MessageBox.Show("입력값 오류 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtHabit7.Focus();
                            return;
                        }
                        txtHabit8.Focus();
                    }
                    else if (sender == txtHabit8)
                    {
                        lblHabit8.Text = hm.READ_Munjin_Name("DENTAL", "구강7", txtHabit8.Text);
                        if (txtHabit8.Text.IsNullOrEmpty() && lblHabit8.Text == "")
                        {
                            MessageBox.Show("입력값 오류 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtHabit8.Focus();
                            return;
                        }
                        txtHabit9.Focus();
                    }
                    else if (sender == txtHabit9)
                    {
                        lblHabit9.Text = hm.READ_Munjin_Name("DENTAL", "구강7", txtHabit9.Text);
                        if (txtHabit9.Text.IsNullOrEmpty() && lblHabit9.Text == "")
                        {
                            MessageBox.Show("입력값 오류 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtHabit9.Focus(); return;
                        }
                        txtHabit1.Focus();
                    }
                    else if (sender == txtJilbyung)
                    {
                        lblJilbyung.Text = hm.READ_Munjin_Name("DENTAL", "구강5", txtJilbyung.Text);
                        if (!txtJilbyung.Text.IsNullOrEmpty() && lblJilbyung.Text == "")
                        {
                            MessageBox.Show("입력값 오류 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtJilbyung.Focus(); return;
                        }
                        SendKeys.Send("{TAB}");
                    }
                    else if (sender == txtOpdDnt)
                    {
                        lblOpdDnt.Text = hm.READ_Munjin_Name("DENTAL", "OPDDNT", txtOpdDnt.Text);
                        if (!txtOpdDnt.Text.IsNullOrEmpty() && lblOpdDnt.Text == "")
                        {
                            MessageBox.Show("입력값 오류 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtOpdDnt.Focus(); return;
                        }
                        SendKeys.Send("{TAB}");
                    }
                    else if (sender == txtStat1)
                    {
                        lblStat1.Text = hm.READ_Munjin_Name("DENTAL", "예아니오", txtStat1.Text);
                        if (!txtStat1.Text.IsNullOrEmpty() && lblStat1.Text == "")
                        {
                            MessageBox.Show("입력값 오류 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtStat1.Focus(); return;
                        }
                        SendKeys.Send("{TAB}");
                    }
                    else if (sender == txtStat2)
                    {
                        lblStat2.Text = hm.READ_Munjin_Name("DENTAL", "예아니오", txtStat2.Text);
                        if (!txtStat2.Text.IsNullOrEmpty() && lblStat2.Text == "")
                        {
                            MessageBox.Show("입력값 오류 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtStat2.Focus(); return;
                        }
                        SendKeys.Send("{TAB}");
                    }
                    else if (sender == txtCardio)
                    {
                        lblCardioStatus.Text = hm.READ_Munjin_Name("DENTAL", "구강5", txtCardio.Text);
                        if (!txtCardio.Text.IsNullOrEmpty() && lblCardioStatus.Text == "")
                        {
                            MessageBox.Show("입력값 오류 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtCardio.Focus(); return;
                        }
                        SendKeys.Send("{TAB}");
                    }
                    else if (sender == txtMunjinEtc)
                    {
                        btnMenuSave.Focus();
                    }
                }
            }
        }

        void fn_Screen_Clear()
        {
            FstrROWID = "";
            FstrSex = "";
            FnWRTNO = 0;

            txtWrtNo.Text = "";
            sp.Spread_All_Clear(ssPatInfo);
            ssPatInfo.ActiveSheet.RowCount = 1;

            tabControl1.SelectedTab = tab1;

            //구강문진항목-------------------------------- -
            //구강인식
            txtOpdDnt.Text = "";    lblOpdDnt.Text = "";
            txtJilbyung.Text = "";  lblJilbyung.Text = "";
            txtCardio.Text = "";    lblCardioStatus.Text = "";
            txtFunc1.Text = "";     lblFunc1.Text = "";
            txtStat1.Text = "";     lblStat1.Text = "";
            txtStat2.Text = "";     lblStat2.Text = "";
            txtDntStatus.Text = ""; lblDntStatus.Text = "";


            //구강건강습관
            txtHabit1.Text = "";    lblHabit1.Text = "";
            txtHabit2.Text = "";
            txtHabit4.Text = "";    lblHabit4.Text = "";
            txtHabit5.Text = "";    lblHabit5.Text = "";
            txtHabit6.Text = "";    lblHabit6.Text = "";
            txtHabit7.Text = "";    lblHabit7.Text = "";
            txtHabit8.Text = "";    lblHabit8.Text = "";
            txtHabit9.Text = "";    lblHabit9.Text = "";
            txtMunjinEtc.Text = "";


            //문진표 평가
            SS4.ActiveSheet.Cells[1, 2].Text = "";
            SS4.ActiveSheet.Cells[3, 2].Text = "";
            SS4.ActiveSheet.Cells[1, 9].Text = "";
            SS4.ActiveSheet.Cells[2, 9].Text = "";
            SS4.ActiveSheet.Cells[3, 9].Text = "";
            SS4.ActiveSheet.Cells[4, 9].Text = "";

            //구강검사 결과
            for (int i = 7; i < 13; i++)
            {
                SS4.ActiveSheet.Cells[i, 5].Text = "";
            }

            chkSugar.Checked = false;
            chkTeeth.Checked = false;
            chkFluorine.Checked = false;
            chkThoroughExam.Checked = false;
            chkProfessionalDental.Checked = false;
            chktoothwastingtherapy.Checked = false;
            chkDentalTreatment.Checked = false;

            rdoTotJudgment1.Checked = false;
            rdoTotJudgment2.Checked = false;
            rdoTotJudgment3.Checked = false;
            rdoTotJudgment4.Checked = false;

            txtPanjengJochi.Text = "";
            txtEtcPositionExamOpinion.Text = "";
            txtResultInterpretation.Text = "";

            //치균세균막검사-만40세
            txt40_Pan1.Text = "";
            txt40_Pan2.Text = "";
            txt40_Pan3.Text = "";
            txt40_Pan4.Text = "";
            txt40_Pan5.Text = "";
            txt40_Pan6.Text = "";
            txt40_PanTot.Text = "";

            //2019(치균세균막검사)
            rdo40_Pan1_New0.Checked = true;
            rdo40_Pan2_New0.Checked = true;
            rdo40_Pan3_New0.Checked = true;
            rdo40_Pan4_New0.Checked = true;
            rdo40_Pan5_New0.Checked = true;
            rdo40_Pan6_New0.Checked = true;

            lblJudgmentDay.Text = "";
            lblJudgmentDr.Text = "";
            lblMsg.Text = "";

            btnMenuSave.Enabled = false;
            btnMenuDelete.Enabled = false;
            btnMenuCancel.Enabled = false;
        }

        void fn_Screen_Display()
        {
            long nAge = 0;
            int nIndex = 0;

            string strData = "";
            string strJepDate = "";
            string strRES1 = "";
            string strRES2 = "";
            string strRES3 = "";
            string strResult = "";

            FnWRTNO = txtWrtNo.Text.To<long>();
            if (FnWRTNO == 0) return;

            if (hicSunapdtlService.GetCountbyWrtNoCode(FnWRTNO, "1158") == 0)
            {
                tab3.Visible = false;
            }
            else
            {
                tab3.Visible = true;
            }

            //삭제된것 체크
            if (hm.READ_JepsuSTS_GBN(FnWRTNO) == "D")
            {
                MessageBox.Show(FnWRTNO + " 접수번호는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //인적사항을 Display
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWRTNO);

            if (list == null)
            {
                MessageBox.Show("접수번호 " + FnWRTNO + "번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FstrSex = list.SEX;
            FstrJepDate = list.JEPDATE;

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PANO.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE.To<string>() + FstrSex;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.To<string>());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list.JEPDATE;
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);

            nAge = list.AGE;
            strJepDate = list.JEPDATE;

            //문진내역을 READ
            FstrROWID = "";
            HIC_RES_DENTAL list2 = hicResDentalService.GetItemByWrtno(FnWRTNO);

            if (list2.IsNullOrEmpty())
            {
                btnMenuSave.Enabled = true;
                btnMenuDelete.Enabled = false;
                btnMenuCancel.Enabled = true;
                return;
            }

            FstrROWID = list2.RID.Trim();

            //신규 등록이면
            if (list2.OPDDNT.IsNullOrEmpty())
            {
                txtOpdDnt.Text = "";
                txtJilbyung.Text = "";
                txtCardio.Text = "";
                txtFunc1.Text = "";
                txtStat1.Text = "";
                txtStat2.Text = "";
                txtDntStatus.Text = "";
                txtHabit5.Text = "";
                txtHabit2.Text = "";
                txtHabit6.Text = "";
                txtHabit4.Text = "";
                txtHabit7.Text = "";
                txtHabit8.Text = "";
                txtHabit9.Text = "";
                txtHabit1.Text = "";
            }
            else
            {
                //1.구강문진항목
                //병력과 구강건강인식
                txtOpdDnt.Text = list2.OPDDNT;
                lblOpdDnt.Text = hm.READ_Munjin_Name("DENTAL", "OPDDNT", txtOpdDnt.Text.Trim());
                txtJilbyung.Text = list2.T_JILBYUNG1;
                lblJilbyung.Text = hm.READ_Munjin_Name("DENTAL", "구강5", txtJilbyung.Text);
                txtCardio.Text = list2.T_JILBYUNG2;
                lblCardioStatus.Text = hm.READ_Munjin_Name("DENTAL", "구강5", txtCardio.Text.Trim());
                txtFunc1.Text = list2.T_FUNCTION1.Trim();
                lblFunc1.Text = hm.READ_Munjin_Name("DENTAL", "예아니오", txtFunc1.Text.Trim());
                txtStat1.Text = list2.T_STAT1.Trim();
                lblStat1.Text = hm.READ_Munjin_Name("DENTAL", "예아니오", txtStat1.Text.Trim());
                txtStat2.Text = list2.T_STAT2.Trim();
                lblStat2.Text = hm.READ_Munjin_Name("DENTAL", "예아니오", txtStat2.Text.Trim());
                txtDntStatus.Text = list2.DNTSTATUS.Trim();
                lblDntStatus.Text = hm.READ_Munjin_Name("DENTAL", "DNTSTATUS", txtDntStatus.Text.Trim());

                //구강건강습관
                txtHabit5.Text = list2.T_HABIT5.Trim();
                lblHabit5.Text = hm.READ_Munjin_Name("DENTAL", "예아니오", txtHabit5.Text.Trim());
                txtHabit2.Text = list2.T_HABIT2.ToString();
                txtHabit6.Text = list2.T_HABIT6.Trim();
                lblHabit6.Text = hm.READ_Munjin_Name("DENTAL", "구강6", txtHabit6.Text.Trim());
                txtHabit4.Text = list2.T_HABIT4.Trim();
                lblHabit4.Text = hm.READ_Munjin_Name("DENTAL", "구강3", txtHabit4.Text.Trim());
                txtHabit7.Text = list2.T_HABIT7.Trim();
                lblHabit7.Text = hm.READ_Munjin_Name("DENTAL", "구강5", txtHabit7.Text.Trim());
                txtHabit8.Text = list2.T_HABIT8.Trim();
                lblHabit8.Text = hm.READ_Munjin_Name("DENTAL", "구강7", txtHabit8.Text.Trim());
                txtHabit9.Text = list2.T_HABIT9.Trim();
                lblHabit9.Text = hm.READ_Munjin_Name("DENTAL", "구강7", txtHabit9.Text.Trim());
                txtHabit1.Text = list2.T_HABIT1.Trim();
                lblHabit1.Text = hm.READ_Munjin_Name("DENTAL", "구강1", txtHabit1.Text.Trim());
            }
            //특별한 증상
            txtMunjinEtc.Text = list2.MUNJINETC;

            //--------( 구강판정 및 소견)------------
            strRES1 = list2.RES_MUNJIN;
            strRES2 = list2.RES_RESULT;
            strRES3 = list2.RES_JOCHI;

            if (!strRES1.IsNullOrEmpty())
            {
                //1.(치과)병력문제
                if (VB.Mid(strRES1, 1, 1) == "1")
                {
                    strResult = "1. 없음";
                }
                else if (VB.Mid(strRES1, 1, 1) == "2")
                {
                    strResult = "2. 있음";
                }
                else
                {
                    strResult = "";
                }
                SS4.ActiveSheet.Cells[1, 2].Text = strResult;

                //2.구강건강인식도문제
                if (VB.Mid(strRES1, 2, 1) == "1")
                {
                    strResult = "1. 없음";
                }
                else if (VB.Mid(strRES1, 2, 1) == "2")
                {
                    strResult = "2. 있음";
                }
                else
                {
                    strResult = "";
                }
                SS4.ActiveSheet.Cells[3, 2].Text = strResult;

                //3.구강위생
                if (VB.Mid(strRES1, 3, 1) == "1")
                {
                    strResult = "1. 없음";
                }
                else if (VB.Mid(strRES1, 3, 1) == "2")
                {
                    strResult = "2. 있음";
                }
                else
                {
                    strResult = "";
                }
                SS4.ActiveSheet.Cells[1, 9].Text = strResult;

                //4.불소이용
                if (VB.Mid(strRES1, 4, 1) == "1")
                {
                    strResult = "1. 없음";
                }
                else if (VB.Mid(strRES1, 4, 1) == "2")
                {
                    strResult = "2. 있음";
                }
                else
                {
                    strResult = "";
                }
                SS4.ActiveSheet.Cells[2, 9].Text = strResult;

                //5.설탕섭취
                if (VB.Mid(strRES1, 5, 1) == "1")
                {
                    strResult = "1. 없음";
                }
                else if (VB.Mid(strRES1, 5, 1) == "2")
                {
                    strResult = "2. 있음";
                }
                else
                {
                    strResult = "";
                }
                SS4.ActiveSheet.Cells[3, 9].Text = strResult;

                //6.흡연
                if (VB.Mid(strRES1, 6, 1) == "1")
                {
                    strResult = "1. 없음";
                }
                else if (VB.Mid(strRES1, 6, 1) == "2")
                {
                    strResult = "2. 있음";
                }
                else
                {
                    strResult = "";
                }
                SS4.ActiveSheet.Cells[4, 9].Text = strResult;
            }

            if (!strRES2.IsNullOrEmpty())
            {
                //1.우식치아
                if (VB.Mid(strRES2, 1, 1) == "1")
                {
                    strResult = "1. 없음";
                }
                else if (VB.Mid(strRES2, 1, 1) == "2")
                {
                    strResult = "2. 있음";
                }
                else
                {
                    strResult = "";
                }
                SS4.ActiveSheet.Cells[7, 5].Text = strResult;

                //2.인접면 우식 의심치아
                if (VB.Mid(strRES2, 2, 1) == "1")
                {
                    strResult = "1. 없음";
                }
                else if (VB.Mid(strRES2, 2, 1) == "2")
                {
                    strResult = "2. 있음";
                }
                else
                {
                    strResult = "";
                }
                SS4.ActiveSheet.Cells[8, 5].Text = strResult;

                //3.수복치아
                if (VB.Mid(strRES2, 3, 1) == "1")
                {
                    strResult = "1. 없음";
                }
                else if (VB.Mid(strRES2, 3, 1) == "2")
                {
                    strResult = "2. 있음";
                }
                else
                {
                    strResult = "";
                }
                SS4.ActiveSheet.Cells[9, 5].Text = strResult;

                //4.상실치아
                if (VB.Mid(strRES2, 4, 1) == "1")
                {
                    strResult = "1. 없음";
                }
                else if (VB.Mid(strRES2, 4, 1) == "2")
                {
                    strResult = "2. 있음";
                }
                else
                {
                    strResult = "";
                }
                SS4.ActiveSheet.Cells[10, 5].Text = strResult;

                //5.치은염증
                if (VB.Mid(strRES2, 5, 1) == "1")
                {
                    strResult = "1. 없음";
                }
                else if (VB.Mid(strRES2, 5, 1) == "2")
                {
                    strResult = "2. 경증";
                }
                else if (VB.Mid(strRES2, 5, 1) == "3")
                {
                    strResult = "3. 중증";
                }
                else
                {
                    strResult = "";
                }
                SS4.ActiveSheet.Cells[11, 5].Text = strResult;

                //6.치석
                if (VB.Mid(strRES2, 6, 1) == "1")
                {
                    strResult = "1. 없음";
                }
                else if (VB.Mid(strRES2, 6, 1) == "2")
                {
                    strResult = "2. 경증";
                }
                else if (VB.Mid(strRES2, 6, 1) == "3")
                {
                    strResult = "3. 중증";
                }
                else
                {
                    strResult = "";
                }
                SS4.ActiveSheet.Cells[12, 5].Text = strResult;
            }

            if (!list2.PANJENGDATE.IsNullOrEmpty())
            {
                //종합판정
                switch (list2.T_PANJENG1.Trim())
                {
                    case "1":
                        rdoTotJudgment1.Checked = true;
                        break;
                    case "2":
                        rdoTotJudgment2.Checked = true;
                        break;
                    case "3":
                        rdoTotJudgment3.Checked = true;
                        break;
                    case "4":
                        rdoTotJudgment4.Checked = true;
                        break;
                    default:
                        rdoTotJudgment1.Checked = false;
                        rdoTotJudgment2.Checked = false;
                        rdoTotJudgment3.Checked = false;
                        rdoTotJudgment4.Checked = false;
                        break;
                }

                //조치사항
                chkSugar.Checked = false;
                chkTeeth.Checked = false;
                chkFluorine.Checked = false;
                chkThoroughExam.Checked = false;
                chkProfessionalDental.Checked = false;
                chktoothwastingtherapy.Checked = false;
                chkDentalTreatment.Checked = false;

                if (VB.Mid(strRES3, 1, 1) == "1") chkSugar.Checked = true;
                if (VB.Mid(strRES3, 2, 1) == "1") chkTeeth.Checked = true;
                if (VB.Mid(strRES3, 3, 1) == "1") chkFluorine.Checked = true;
                if (VB.Mid(strRES3, 4, 1) == "1") chkThoroughExam.Checked = true; 
                if (VB.Mid(strRES3, 5, 1) == "1") chkProfessionalDental.Checked = true;
                if (VB.Mid(strRES3, 6, 1) == "1") chktoothwastingtherapy.Checked = true;
                if (VB.Mid(strRES3, 7, 1) == "1") chkDentalTreatment.Checked = true;

                //추가조치사항
                txtPanjengJochi.Text = list2.T_PANJENG_SOGEN;
                txtEtcPositionExamOpinion.Text = list2.T_PANJENG_ETC;
                txtResultInterpretation.Text = list2.SANGDAM;

                //판정일 및 의사
                lblJudgmentDay.Text = list2.PANJENGDATE;
                lblJudgmentDr.Text = hb.READ_License_DrName(list2.PANJENGDRNO);
            }

            //치면세균막검사-만40만
            txt40_Pan1.Text = list2.T40_PAN1.To<string>();
            txt40_Pan2.Text = list2.T40_PAN2.To<string>();
            txt40_Pan3.Text = list2.T40_PAN3.To<string>();
            txt40_Pan4.Text = list2.T40_PAN4.To<string>();
            txt40_Pan5.Text = list2.T40_PAN5.To<string>();
            txt40_Pan6.Text = list2.T40_PAN6.To<string>();

            //2019변경사항
            switch (list2.T40_PAN1_NEW)
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

            switch (list2.T40_PAN2_NEW)
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

            switch (list2.T40_PAN3_NEW)
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

            switch (list2.T40_PAN4_NEW)
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

            switch (list2.T40_PAN5_NEW)
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

            switch (list2.T40_PAN6_NEW)
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

            btnMenuSave.Enabled = true;
            btnMenuCancel.Enabled = true;
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
