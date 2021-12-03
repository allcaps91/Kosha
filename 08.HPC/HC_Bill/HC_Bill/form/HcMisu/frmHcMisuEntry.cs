using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Bill
/// File Name       : frmHcMisuEntry.cs
/// Description     : 미수 자료 등록(기타)
/// Author          : 심명섭
/// Create Date     : 2021-06-07
/// Update History  : 
/// </summary>
/// <seealso cref= "hcmisu > FrmMisuEntry (HcMisu01_new.frm)" />
/// 

namespace HC_Bill
{
    public partial class frmHcMisuEntry : BaseForm
    {
        int result;
        long FnMisuAmt = 0;
        long FnIpgumAmt = 0;
        long FnGamAmt = 0;
        long FnSakAmt = 0;
        long FnBanAmt = 0;
        long FnJanAmt = 0;
        long FnCardAmt = 0;
        string FstrROWID = "";
        string FstrGbMisuBuild = "";
        string FstrIpOk = "";
        long FnCharAmt = 0 ;
        string FstrCharDate = "";
        string strJong = "";
        string strMirGbn = "";
        string strPumMok = "";
        // 변동내역을 기준으로 총액을 다시 계산
        string strDel = "";
        string strGeaCode = "";
        string strGeaName = "";
        long nAmt = 0;
        string strBDate = "";
        long nWrtno;
        long nSlipAmt;
        string strRemark;
        string strChange;
        string strROWID;
        string strOk;

        string FstrCode = "";
        string FstrName = "";

        // Spread 약한참조
        clsSpread cSpd = null;
        clsHaBase cHB = null;
        clsHcMisu cHM = null;
        ComFunc cF = null;
        HIC_LTD LtdHelpItem = null;
        HIC_MISU_MST_SLIP MisuNoHelp= null;
        HIC_MISU_MST_SLIP JisaNoHelp= null;
        HicMisuMonthlyService hicMisuMonthlyService = null;
        HicMisuMstSlipService hicMisuMstSlipService = null;
        MisuTaxService misuTaxService = null;
        HicLtdService hicLtdService = null;
        HicMisuGiroService hicMisuGiroService = null;
        frmHcMisuNoHelp1 frmHcMisuNoHelp1 = null;
        frmHcMisuNoHelp2 frmHcMisuNoHelp2 = null;
        frmHcLtdHelp FrmHcLtdHelp = null;
        frmHcCodeHelp FrmHcCodeHelp = null;

        public frmHcMisuEntry()
        {
            InitializeComponent();
            // 이벤트
            SetEvent();
            // 컨트롤
            SetControl();
        }

        private void SetEvent()
        {
            // Menu Btn
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnDel.Click += new EventHandler(eBtnClick);
            this.btnModify.Click += new EventHandler(eBtnClick);
            this.btnNew.Click += new EventHandler(eBtnClick);
            this.btnCompanySearch.Click += new EventHandler(eBtnClick);
            this.btnJisaSearch.Click += new EventHandler(eBtnClick);
            this.btnGiroSearch.Click += new EventHandler(eBtnClick);
            this.btnChunguSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnJisaHelp.Click += new EventHandler(eBtnClick);
            this.btnKihoHelp.Click += new EventHandler(eBtnClick);

            // keyDown
            this.txtWrtNo.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.TxtLtdCode.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.cboJONG.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.cboMir.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.TxtJisa.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.TxtKiho.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.TxtDate.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.TxtMisuAmt.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.TxtGiroNo.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.cboGbn.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.TxtRemark.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.TxtPumMok.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.TxtDamName.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.TxtChaAmt.KeyDown += new KeyEventHandler(eTxtKeyDown);
        }
        
        private void SS2_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if(btnSave.Enabled == true && SS2.ActiveSheet.Cells[e.Row, e.Column].Text == "True" || SS2.ActiveSheet.Cells[e.Row, e.Column].Text == "False")
            {
                Total_Amt_Gesan();
            }
        }

        private void SS2_Sheet1_Change(object sender, ChangeEventArgs e)
        {
            int Row = SS2_Sheet1.ActiveRowIndex;

            if (e.Column > 1)
            {
                SS2.ActiveSheet.Cells[Row, 6].Text = "Y";
            }
            
            if(SS2.ActiveSheet.Cells[e.Row, 2].Text == "")
            {
                SS2.ActiveSheet.Cells[e.Row, 3].Text = "";
            }
            else
            {
                SS2.ActiveSheet.Cells[e.Row, 3].Text = cHM.READ_MisuGea_Name(SS2.ActiveSheet.Cells[e.Row, 2].Text);
            }
            
        }

        private void SS2_Sheet1_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            int nRow = 0;
            int nCol = 0;

            nRow = e.NewRow;
            nCol = e.NewColumn;
            
            if (btnSave.Enabled == true)
            {
                if(nCol == 2 || nCol == 4)
                {
                    Total_Amt_Gesan();
                }
            }
            
            switch (nCol)
            {
                case 0:
                    PanelMsg.Text = "버튼을 선택하면 해당줄의 내용이 삭제됨";
                    break;
                case 1:
                    PanelMsg.Text = "입금 또는 삭감일자를 입력하세요";
                    break;
                case 2:
                    PanelMsg.Text = "21.현금 22.지로입금 23.통장입금 24.기타입금 55.카드입금 25.수수료 31.감액 32.삭감 33.반송 99.청구차액";
                    break;
                default:
                    PanelMsg.Text = "";
                    break;
            }
        }

        private void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == txtWrtNo)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DisPlay_Screen(txtWrtNo.Text);
                }
            }
            else if (sender == TxtLtdCode)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    LtdHelp_Click(TxtLtdCode.Text);
                    cboJONG.Focus();
                }
            }
            else if(sender == cboJONG)
            {
                if(e.KeyCode == Keys.Enter)
                {
                    cboMir.Focus();
                }
            }
            else if (sender == cboMir)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    TxtJisa.Focus();
                }
            }
            else if (sender == TxtJisa)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    JisaHelp_Click(TxtJisa.Text);
                    TxtKiho.Focus();
                }
            }
            else if (sender == TxtKiho)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    TxtDate.Focus();
                }
            }
            else if (sender == TxtDate)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    TxtMisuAmt.Focus();
                }
            }
            else if (sender == TxtMisuAmt)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    TxtGiroNo.Focus();
                }
            }
            else if (sender == TxtGiroNo)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cboGbn.Focus();
                }
            }
            else if (sender == cboGbn)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    TxtRemark.Focus();
                }
            }
            else if (sender == TxtRemark)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    TxtPumMok.Focus();
                }
            }
            else if (sender == TxtPumMok)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    TxtDamName.Focus();
                }
            }
            else if (sender == TxtDamName)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    TxtChaAmt.Focus();
                }
            }
            else if (sender == TxtChaAmt)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    TxtYYMM.Focus();
                }
            }

        }

        private void JisaHelp_Click(string Jisa)
        {
            if(Jisa != "")
            {
                TxtJisa.Text = VB.Left(Jisa, 10).Trim();
                PanelJisaName.Text = VB.Right(Jisa, VB.Len(Jisa) - 10).Trim();
            }
        }

        private void LtdHelp_Click(string LtdCode)
        {
            if(LtdCode != "")
            {
                TxtLtdCode.Text = LtdCode.Trim();
                PanelLtdName.Text = cHB.READ_Ltd_Name(LtdCode);
            }
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            cHB = new clsHaBase();
            cHM = new clsHcMisu();
            cF = new ComFunc();
            hicMisuMstSlipService = new HicMisuMstSlipService();
            hicMisuMonthlyService = new HicMisuMonthlyService();
            misuTaxService = new MisuTaxService();
            hicLtdService = new HicLtdService();
            hicMisuGiroService = new HicMisuGiroService();
            frmHcMisuNoHelp1 = new frmHcMisuNoHelp1();
            frmHcMisuNoHelp2 = new frmHcMisuNoHelp2();
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                DisPlay_Screen(txtWrtNo.Text);
                
            }
            else if (sender == btnSave)
            {
                DataSave();
            }
            else if(sender == btnCancel)
            {
                Cancel();
            }
            else if (sender == btnDel)
            {
                DataDel();
            }
            else if (sender == btnNew)
            {
                New_Registration();
            }
            else if (sender == btnModify)
            {
                Modify();
            }
            else if(sender == btnCompanySearch)
            {
                NumberCompany();
            }
            else if (sender == btnJisaSearch)
            {
                JisaSearch();
            }
            else if (sender == btnGiroSearch)
            {
                GiroSearch();
            }
            else if (sender == btnChunguSearch)
            {
                ChunguSearch();
            }
            else if (sender == btnLtdHelp)
            {
                LtdHelp();
            }
            else if (sender == btnJisaHelp)
            {
                clsPublic.GstrRetValue = "21";      //건강보험 지사
                FrmHcCodeHelp = new frmHcCodeHelp(clsPublic.GstrRetValue);
                FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(eCode_value);
                FrmHcCodeHelp.ShowDialog();

                if (!FstrCode.IsNullOrEmpty())
                {
                    TxtJisa.Text = FstrCode.Trim();
                    PanelJisaName.Text = FstrName.Trim();
                }
                else
                {
                    TxtJisa.Text = "";
                    PanelJisaName.Text = "";
                }

                FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(eCode_value);
            }

            else if (sender == btnKihoHelp)
            {
                clsPublic.GstrRetValue = "18";      //사업장 기호
                FrmHcCodeHelp = new frmHcCodeHelp(clsPublic.GstrRetValue);
                FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(eCode_value);
                FrmHcCodeHelp.ShowDialog();

                if (!FstrCode.IsNullOrEmpty())
                {
                    TxtKiho.Text = FstrCode.Trim();
                    PanelKihoName.Text = FstrName.Trim();
                }
                else
                {
                    TxtKiho.Text = "";
                    PanelKihoName.Text = "";
                }

                FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(eCode_value);
            }
        }

        void eCode_value(string strCode, string strName)
        {
            FstrCode = strCode;
            FstrName = strName;
        }
        
        private void LtdHelp()
        {
            string strLtdCode = "";

            if (TxtLtdCode.Text.IndexOf(".") > 0)
            {
                strLtdCode = VB.Pstr(TxtLtdCode.Text, ".", 2);
            }
            else
            {
                strLtdCode = TxtLtdCode.Text;
            }

            FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
            FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
            FrmHcLtdHelp.ShowDialog();
            FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

            if (!LtdHelpItem.IsNullOrEmpty())
            {
                TxtLtdCode.Text = LtdHelpItem.CODE.ToString();
                PanelLtdName.Text = LtdHelpItem.SANGHO;
            }
            else
            {
                TxtLtdCode.Text = "";
            }
        }

        private void NumberCompany()
        {
            frmHcMisuNoHelp1 = new frmHcMisuNoHelp1();
            frmHcMisuNoHelp1.rSetGstrValue += new frmHcMisuNoHelp1.SetGstrValue(NumCompany);
            frmHcMisuNoHelp1.ShowDialog();
            frmHcMisuNoHelp1.rSetGstrValue -= new frmHcMisuNoHelp1.SetGstrValue(NumCompany);

            if (!MisuNoHelp.IsNullOrEmpty())
            {
                txtWrtNo.Text = MisuNoHelp.WRTNO.ToString();
                DisPlay_Screen(txtWrtNo.Text);
            }
            else
            {
                TxtLtdCode.Text = "";
            }
        }

        private void NumCompany(HIC_MISU_MST_SLIP item)
        {
            MisuNoHelp = item;
        }
        
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void ChunguSearch()
        {
            string strInput;
            long nWrtno;

            strInput = "";
            strInput = Microsoft.VisualBasic.Interaction.InputBox("찾으실 청구번호를 입력하세요", "청구번호");
            if (strInput == "")
            {
                return;
            }

            List<HIC_MISU_MST_SLIP> item = hicMisuMstSlipService.GetchunguNum(strInput.Trim());

            if(item.Count == 0)
            {
                MessageBox.Show("청구번호가 동일한 미수내역이 1건도 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if(item.Count > 1)
            {
                MessageBox.Show("청구번호가 동일한 미수내역이 2건 이상 입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            nWrtno = item[0].WRTNO;
            txtWrtNo.Text = nWrtno.To<string>("0");

            DisPlay_Screen(txtWrtNo.Text);
        }

        private void GiroSearch()
        {
            string strInput;
            long nGiroNo;
            long nWrtno;

            strInput = "";
            strInput = Microsoft.VisualBasic.Interaction.InputBox("찾으실 지로의뢰서 일련번호를 입력하세요", "지로번호");
            if(strInput == "")
            {
                return;
            }
            nGiroNo =  VB.Val(VB.Format(strInput)).To<long>(0);

            //지로발행 내역에서 미수 NWRTNO를 READ
            List<HIC_MISU_GIRO> Read = hicMisuGiroService.getGiro(nGiroNo);

            nWrtno = 0;
            if(Read.Count > 0)
            {
                nWrtno = Read[0].MISUNO;
            }
            if(nWrtno == 0)
            {
                MessageBox.Show("지로발행 내역에 해당 번호는 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            txtWrtNo.Text = nWrtno.To<string>("0");
            DisPlay_Screen(txtWrtNo.Text);
        }

        private void JisaSearch()
        {
            frmHcMisuNoHelp2 = new frmHcMisuNoHelp2();
            frmHcMisuNoHelp2.rSetGstrValue += new frmHcMisuNoHelp2.SetGstrValue(JisaNo);
            frmHcMisuNoHelp2.ShowDialog();
            frmHcMisuNoHelp2.rSetGstrValue -= new frmHcMisuNoHelp2.SetGstrValue(JisaNo);

            if (!JisaNoHelp.IsNullOrEmpty())
            {
                txtWrtNo.Text = JisaNoHelp.WRTNO.ToString();
                DisPlay_Screen(txtWrtNo.Text);
            }
            else
            {
                TxtLtdCode.Text = "";
            }
        }

        private void JisaNo(HIC_MISU_MST_SLIP item)
        {
            JisaNoHelp = item;
        }

        private void DataDel()
        {
            if (!delCheck())
            {
                return;
            }
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                Screen_Clear();
                txtWrtNo.Focus();

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private bool delCheck()
        {
            long nWrtno = 0;

            if (FstrGbMisuBuild == "Y")
            {
                MessageBox.Show("자동 Build된 미수내역은 삭제할 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            clsPublic.GstrMsgList = "정말로 삭제를 하시겠습니까?";

            if (MessageBox.Show(clsPublic.GstrMsgList, "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return false;
            }

            nWrtno = VB.Format(txtWrtNo.Text).To<long>(0);

            HIC_MISU_MST_SLIP item = new HIC_MISU_MST_SLIP();
            result = hicMisuMstSlipService.DelListHistory(item, nWrtno, clsType.User.IdNumber.To<long>(0));

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("삭제내역을 HISTORY에 INSERT시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // 미수자료를 삭제
            result = hicMisuMstSlipService.DelMisuMaster(item, nWrtno);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("미수마스타 삭제시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            result = hicMisuMstSlipService.DelMisuSlip(item, nWrtno);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("미수 SLIP 삭제시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void DataSave()
        {
            FstrIpOk = "";
            Total_Amt_Gesan();                  // 미수번호별 누적금액을 계산

            // 자료에 오류가 있는지 Check
            if (!ERROR_Check())
            {
                return;
            }

            //필수항목값 체크
            //if (!FrameMst.RequiredValidate())
            //{
            //    MessageBox.Show("필수 입력항목이 누락되었습니다.");
            //    return;
            //}

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                // 신규등록시 미수번호를 부여함
                if (txtWrtNo.Text == "신규")
                {
                    //nWrtno = cHM.READ_NEW_MisuNo();
                    nWrtno = cHB.READ_New_MisuNo();
                }
                else
                {
                    nWrtno = VB.Val(VB.Format(txtWrtNo.Text)).To<long>(0);
                }

                // 변동내역을 DB에 UPDAATE                   
                if (!SLIP_Update())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                // 변동내역에 회사코드를 UPDATE
                if (!LtdCode_Update())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                // 미수마스타를 변경
                if (!MISUMST_Update())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                // 미수SLIP에 미수발생 UPDATE
                if (!SlipMisu_Update())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
                // 수정했을경우 재발행시 참고사항 업데이트
                List<MISU_TAX> Note = misuTaxService.GetNote(nWrtno);

                if (Note.Count == 1)
                {
                    //사업장정보 Display
                    List<HIC_LTD> Business = hicLtdService.GetBusiness( VB.Val(TxtLtdCode.Text) );

                    string strDeapyo = "";
                    string strUptae = "";
                    string strJongmok ="";
                    string strSaupNo = "";
                    string strMail = "";
                    string strSangHo = "";

                    strDeapyo = Business[0].DAEPYO;
                    strUptae = Business[0].UPTAE;
                    strJongmok = Business[0].JONGMOK;
                    strSaupNo = (VB.PP(Business[0].SAUPNO, "-", 1) + VB.PP(Business[0].SAUPNO, "-", 2) + VB.PP(Business[0].SAUPNO, "-", 3) + VB.PP(Business[0].SAUPNO, "-", 4)).Trim();
                    strMail = cHB.READ_MAIL_Name(Business[0].MAILCODE.Trim()) + VB.Space(1) + Business[0].JUSODETAIL;
                    strSangHo = Business[0].SANGHO;


                    MISU_TAX item = new MISU_TAX
                    {
                        PUMMOK = TxtPumMok.Trim(),
                        LTDCODE =  TxtLtdCode.Text,
                        DAEPYONAME = strDeapyo,
                        UPTAE = strUptae,
                        JONGMOK = strJongmok,
                        LTDNO = strSaupNo,
                        LTDJUSO = strMail,
                        LTDNAME = strSangHo,
                        ENTSABUN = clsType.User.IdNumber.To<long>(0),
                        MISUNO = txtWrtNo.Text.To<long>(0)
                    };
                    result = hicLtdService.GetMisuTaxUpdate(item);

                    if (result < 0)
                    {
                        MessageBox.Show("변동내역을 등록중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                Screen_Clear();
                txtWrtNo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

        }

        private bool SlipMisu_Update()
        {
            // 기존자료의 수정이고 마스타를 변경하지 않았으면
            //if (FstrROWID != "" && FrameMst.Enabled == false)
            //{
            //    return false;
            //}

            //미수SLIP에 미수발생 내역이 있는지 Check
            List<HIC_MISU_MST_SLIP> MisuCheck = hicMisuMstSlipService.GetMisuCheck(nWrtno);
            strROWID = "";
            if(MisuCheck.Count > 0)
            {
                strROWID = MisuCheck[0].ROWID.Trim();
            }

            if(strROWID == "")
            {
                HIC_MISU_MST_SLIP item = new HIC_MISU_MST_SLIP
                {
                    WRTNO = nWrtno,
                    BDATE = TxtDate.Text,
                    LTDCODE = TxtLtdCode.Text.To<long>(0),
                    SLIPAMT = FnMisuAmt,
                    REMARK = TxtRemark.Text.Trim(),
                    ENTSABUN = clsType.User.IdNumber.To<long>(0)
                };

                result = hicMisuMstSlipService.GetMisuSlipUpdate(item); // error

                if (result < 0)
                {
                    MessageBox.Show("미수SLIP에 미수발생 UPDATE 도중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // 신규입력 내역을 History에 INSERT
                HIC_MISU_MST_SLIP hmm = new HIC_MISU_MST_SLIP();
                result = hicMisuMstSlipService.insertNew(hmm, nWrtno, TxtDate.Text.Trim(), FnMisuAmt, clsType.User.IdNumber.To<long>(0));

                if (result < 0)
                {
                    MessageBox.Show("신규등록 내역을 HISTORY에 INSERT시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                // 변경전 자료를 HISTORY에 INSERT
                HIC_MISU_MST_SLIP item = new HIC_MISU_MST_SLIP();
                result = hicMisuMstSlipService.insertOld(item, clsType.User.IdNumber.To<long>(0), strROWID);

                if (result < 0)
                {
                    MessageBox.Show("신규등록변경전 내역을 HISTORY에 INSERT시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // DB에 UPDATE
                HIC_MISU_MST_SLIP item2 = new HIC_MISU_MST_SLIP();
                result = hicMisuMstSlipService.MisuUpdate(item2, clsType.User.IdNumber.To<long>(0), strROWID, TxtDate.Text.Trim(), VB.Val(TxtLtdCode.Text), TxtRemark.Text, FnMisuAmt);

                if (result < 0)
                {
                    MessageBox.Show("미수SLIP에 미수발생 UPDATE 도중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // 변경후 자료를 HISTORY에 INSERT
                HIC_MISU_MST_SLIP item3 = new HIC_MISU_MST_SLIP();
                result = hicMisuMstSlipService.MisuUpdate_New(item3, clsType.User.IdNumber.To<long>(0), strROWID);

                if (result < 0)
                {
                    MessageBox.Show("변경후 내역을 HISTORY에 INSERT시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private bool MISUMST_Update()               // 미수마스타를 변경
        {
            string strGbEnd;

            strGbEnd = "N"; // 미완료
            if(FnJanAmt == 0) { strGbEnd = "Y"; } // 입금완료

            // 신규등록

            if(FstrROWID == "")
            {
                HIC_MISU_MST_SLIP item = new HIC_MISU_MST_SLIP
                {
                    WRTNO           = nWrtno,
                    LTDCODE         = VB.Val(TxtLtdCode.Text).To<long>(0),
                    JISA            = TxtJisa.Text.Trim(),
                    KIHO            = TxtKiho.Text.Trim(),
                    MISUJONG        = strJong,
                    PUMMOK          = strPumMok,
                    BDATE           = TxtDate.Text,
                    GJONG           = VB.Left(cboJONG.Text, 2),
                    GBEND           = strGbEnd,
                    MISUGBN         = VB.Left(cboGbn.Text, 1),
                    MISUAMT         = FnMisuAmt,
                    IPGUMAMT        = FnIpgumAmt,
                    GAMAMT          = FnGamAmt,
                    SAKAMT          = FnSakAmt,
                    BANAMT          = FnBanAmt,
                    JANAMT          = FnJanAmt,
                    GIRONO          = TxtGiroNo.Text.Trim(),
                    DAMNAME         = TxtDamName.Text.Trim(),
                    REMARK          = cF.Quotation_Change(TxtRemark.Text.Trim()),
                    MIRGBN          = strMirGbn,
                    MIRCHAAMT       = VB.Val(VB.Format(TxtChaAmt.Text)).To<long>(0),
                    MIRCHAREMARK    = cF.Quotation_Change(TxtChaRemark.Text.TrimEnd()),
                    MIRCHADATE      = FstrCharDate.Trim(),
                    YYMM_JIN        = TxtYYMM.Text.Trim(),
                    ENTSABUN        = clsType.User.IdNumber.To<long>(0),
                    CNO             = VB.Val(TxtGiroNo.Text.Trim()).To<long>(0)
                };
                result = hicMisuMstSlipService.GetMisuMaster_Update(item);
                if (result < 0)
                {
                    MessageBox.Show("미수마스타에 UPDATE(0) 도중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                if(FrameMst.Enabled == true)
                {
                    HIC_MISU_MST_SLIP item = new HIC_MISU_MST_SLIP
                    {
                        LTDCODE         = VB.Val(TxtLtdCode.Text).To<long>(0),
                        JISA            = TxtJisa.Text.Trim(),
                        KIHO            = TxtKiho.Text.Trim(),
                        MISUJONG        = strJong,
                        BDATE           = TxtDate.Text,
                        GJONG           = VB.Left(cboJONG.Text, 2),
                        GBEND           = strGbEnd,
                        PUMMOK          = strPumMok,
                        MISUGBN         = VB.Left(cboGbn.Text, 1),
                        GIRONO          = TxtGiroNo.Text.Trim(),
                        CNO             = VB.Val(TxtGiroNo.Text.Trim()).To<long>(0),
                        DAMNAME         = TxtDamName.Trim(),
                        REMARK          = cF.Quotation_Change(TxtRemark.Text.Trim()),
                        MIRGBN          = strMirGbn,
                        MIRCHAAMT       = VB.Val(VB.Format(TxtChaAmt.Text)).To<long>(0),
                        MIRCHAREMARK    = cF.Quotation_Change(TxtChaRemark.Text.TrimEnd()),
                        MIRCHADATE      = FstrCharDate.Trim(),
                        YYMM_JIN        = TxtYYMM.Text.Trim(),
                        ENTSABUN        = clsType.User.IdNumber.To<long>(0),
                        MISUAMT         = FnMisuAmt,
                        IPGUMAMT        = FnIpgumAmt,
                        GAMAMT          = FnGamAmt,
                        SAKAMT          = FnSakAmt,
                        BANAMT          = FnBanAmt,
                        JANAMT          = FnJanAmt,
                        ROWID           = FstrROWID
                    };
                    result = hicMisuMstSlipService.GetMisuMaster_Update_1(item);
                    if (result < 0)
                    {
                        MessageBox.Show("미수마스타에 UPDATE(1) 도중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    HIC_MISU_MST_SLIP item = new HIC_MISU_MST_SLIP
                    {
                        MIRGBN          = strMirGbn,
                        GBEND           = strGbEnd,
                        YYMM_JIN        = TxtYYMM.Text.Trim(),
                        MISUAMT         = FnMisuAmt,
                        IPGUMAMT        = FnIpgumAmt,
                        GAMAMT          = FnGamAmt,
                        SAKAMT          = FnSakAmt,
                        BANAMT          = FnBanAmt,
                        JANAMT          = FnJanAmt,
                        ROWID           = FstrROWID
                    };
                    result = hicMisuMstSlipService.GetMisuMaster_Update_2(item);
                    if (result < 0)
                    {
                        MessageBox.Show("미수마스타에 UPDATE(2) 도중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            
            return true;
        }            

        private bool LtdCode_Update()               // 변동내역의 회사코드를 화면의 코드로 UPDATE
        {
            HIC_MISU_MST_SLIP item = new HIC_MISU_MST_SLIP
            {
                WRTNO = nWrtno,
                LTDCODE = VB.Val(TxtLtdCode.Text).To<long>(0)
            };
            result = hicMisuMstSlipService.LtdCodeUpdate(item);

            if (result < 0)
            {
                MessageBox.Show("SLIP에 회사코드 UPDATE중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool SLIP_Update()                  // 변동내역을 DB에 UPDATE
        {
            for(int i = 0; i < SS2.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1; i++)
            {
                strDel = SS2.ActiveSheet.Cells[i, 0].Text.ToString().Trim();
                strBDate = SS2.ActiveSheet.Cells[i, 1].Text;
                strGeaCode = SS2.ActiveSheet.Cells[i, 2].Text;
                strGeaName = SS2.ActiveSheet.Cells[i, 3].Text;
                nSlipAmt = VB.Replace(SS2.ActiveSheet.Cells[i, 4].Text, ",", "").To<long>(0);
                strRemark = SS2.ActiveSheet.Cells[i, 5].Text;
                strChange =  SS2.ActiveSheet.Cells[i, 6].Text;
                strROWID = SS2.ActiveSheet.Cells[i, 7].Text;

                if(strDel == "True")
                {
                    if(strROWID != "") 
                    {
                        // 삭제내역을 History에 INSERT
                        result = hicMisuMstSlipService.GetinsertDelHistory(strROWID, clsType.User.IdNumber.To<long>(0));

                        if (result < 0)
                        {
                            MessageBox.Show("삭제내역을 HISTORY에 INSERT시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        // 자료를 삭제
                        result = hicMisuMstSlipService.GetDelHistory(strROWID);
                        if (result < 0)
                        {
                            MessageBox.Show(i + "번줄 변동내역을 등록 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
                else
                {
                    if(strROWID != "")
                    {
                        if(strChange == "Y")
                        {
                            // 변경전 자료를 HISTORY에 INSERT
                            result = hicMisuMstSlipService.HistoryIn(strROWID, clsType.User.IdNumber.To<long>(0));
                            if (result < 0)
                            {
                                MessageBox.Show("변경전 내역을 HISTORY에 INSERT시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }

                            //자료를 변경

                            HIC_MISU_MST_SLIP item = new HIC_MISU_MST_SLIP
                            {
                                BDATE       = strBDate,
                                GEACODE     = strGeaCode,
                                SLIPAMT     = nSlipAmt,
                                REMARK      = strRemark,
                                ENTSABUN    = clsType.User.IdNumber.To<long>(0),
                                ROWID       = strROWID
                            };
                            result = hicMisuMstSlipService.HistoryUpdate(item);
                            if(result < 0)
                            {
                                MessageBox.Show(i + "번줄 변동내역을 등록 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }

                            // 변경후 자료를 HISTORY에 INSERT
                            result = hicMisuMstSlipService.HistoryAfter(strROWID, clsType.User.IdNumber.To<long>(0));
                            if (result < 0)
                            {
                                MessageBox.Show("변경후 내역을 HISTORY에 INSERT시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);  
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (strBDate != "")
                        {
                            HIC_MISU_MST_SLIP item = new HIC_MISU_MST_SLIP
                            {
                                WRTNO = nWrtno,
                                BDATE = strBDate,
                                LTDCODE = TxtLtdCode.Text.To<long>(0),
                                GEACODE = strGeaCode,
                                SLIPAMT = nSlipAmt,
                                REMARK = strRemark,
                                ENTSABUN = clsType.User.IdNumber.To<long>(0)
                            };
                            result = hicMisuMstSlipService.HistoryInsert(item);
                            if (result < 0)
                            {
                                MessageBox.Show(i + "번줄 변동내역을 등록 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }

                            // 신규입력 내역을 history에 INSERT
                            result = hicMisuMstSlipService.HistoryInsert_New(item);
                            if (result < 0)
                            {
                                MessageBox.Show("신규등록 내역을 HISTORY에 INSERT시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private bool ERROR_Check()
        {
            string strTEMP = "";
            string strYYMM = "";
            string strBuildDate = "";                // 최종빌드월
            string strROWID2 = "";                   // 이미등록한 slip의 rowid

            if (TxtLtdCode.Text.Trim() == "")   { MessageBox.Show("회사코드가 공란입니다.",                         "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
            if (PanelLtdName.Text.Trim() == "") { MessageBox.Show("회사명이 공란입니다.",                           "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
            if (cboJONG.Text.Trim() == "")      { MessageBox.Show("검진종류가 공란입니다.",                         "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
            if (cboGbn.Text.Trim() == "")       { MessageBox.Show("미수구분이 공란입니다.",                         "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
            if (TxtDate.Text.Trim() == "")      { MessageBox.Show("청구일자가 공란입니다.",                         "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
            if (TxtRemark.Text.Trim() == "")    { MessageBox.Show("미수내역이 공란입니다.",                         "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
            if (TxtYYMM.Text.Trim() == "")      { MessageBox.Show("검진년월이 공란입니다.",                         "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
            if (VB.Len(TxtYYMM.Text) != 6)      { MessageBox.Show("검진년월형식을 YYYYMM(200810)으로 입력하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
            if (FnJanAmt < 0)                   { MessageBox.Show("미수잔액이 0보다 적음.",                         "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }

            strJong = "";
            if (rdoJong1.Checked)      { strJong = "1"; }       // 회사미수
            else if (rdoJong2.Checked) { strJong = "2"; }       // 건강보험
            else if (rdoJong3.Checked) { strJong = "3"; }       // 국고
            else if (rdoJong4.Checked) { strJong = "4"; }       // 개인미수

            if(strJong == "") { MessageBox.Show("미수종류가 선택 않됨.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }

            if(strJong == "1")
            {
                //                2016-12-29 건진부장님 요청으로 특정회사코드 제한 풀어놓음 - 민철
                //  If TxtLtdCode.Text >= "0174" And TxtLtdCode <= "0176" Then
                //MsgBox "미수종류와 회사코드가 상이함", vbInformation, "확인"
                //Exit Sub
                //End If 
            }
            else if(strJong == "2")
            {
                if(TxtJisa.Text.Trim() == "")
                {
                    MessageBox.Show("건강보험은 반드시 지사코드가 입력되어야 합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else if(strJong == "3")
            {
                // 2016-12-29 건진부장님 요청으로 특정회사코드 제한 풀어놓음 - 민철
                // If TxtLtdCode.Text <> "0175" Then MsgBox "미수종류와 회사코드가 상이함", vbInformation, "오류": Exit Sub
            }
            else if (strJong == "4")
            {
                // 2016-12-29 건진부장님 요청으로 특정회사코드 제한 풀어놓음 - 민철
                // If TxtLtdCode.Text <> "0176" Then MsgBox "미수종류와 회사코드가 상이함", vbInformation, "오류": Exit Sub
            }

            strMirGbn = VB.Left(cboMir.Text, 1).Trim();
            if(strMirGbn.To<long>(0) < 1 || strMirGbn.To<long>(0) > 5)
            {
                MessageBox.Show("청구구분이 오류입니다. (1~5를 입력하세요)", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(VB.Val(VB.Format(TxtChaAmt.Text))!= 0 && TxtChaRemark.Text.Trim() == "")
            {
                MessageBox.Show("청구차액이 발생하였으나 상세내역이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (VB.Val(VB.Format(TxtChaAmt.Text)) == 0 && TxtChaRemark.Text.Trim() != "")
            {
                MessageBox.Show("청구차액이 없으나 상세내역에 자료가 있습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(TxtChaRemark.Text.TrimEnd().Length > 500)
            {
                MessageBox.Show("청구차액 상세내역은 200자까지 가능함.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // 차액발생일자를 등록
            if(VB.Format(TxtChaAmt.Text).To<long>(0) != FnCharAmt)
            {
                FstrCharDate = clsPublic.GstrSysDate;
            }

            strPumMok = TxtPumMok.Text.Trim();

            List<HIC_MISU_MONTHLY> YYMM = hicMisuMonthlyService.GetYYMM();
            strBuildDate = YYMM[0].YYMM.Trim();                                     // 최종빌드월 읽기

            if(txtWrtNo.Text != "신규")
            {
                // 미수통계조회
                List<HIC_MISU_MONTHLY> misuTong = hicMisuMonthlyService.GetmisuTong(txtWrtNo.Text.To<long>(0));
                strYYMM = misuTong[0].YYMM;

                // MISU_SLIP 최종날짜.
                List<HIC_MISU_MST_SLIP> misuDate = hicMisuMstSlipService.GetMisuDate(txtWrtNo.Text.To<long>(0));
                strTEMP = misuDate[0].BDATE;
            }
            else if (txtWrtNo.Text == "신규")
            {
                // 미수통계조회
                List<HIC_MISU_MONTHLY> misuTong_2 = hicMisuMonthlyService.GetmisuTong_2();
                strYYMM = misuTong_2[0].YYMM.Trim();

                if(  ( VB.Left(TxtDate.Text.Trim(), 4) + VB.Mid(TxtDate.Text.Trim(), 6, 2) ).To<long>(0) <= strYYMM.To<long>(0))
                {
                    MessageBox.Show("청구빌드한 월에 미수를 발생시킬 수 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }  
            }

            // 변동내역의 자료를 점검

            for(int i = 0; i < SS1.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1; i++)
            {
                strROWID2 = "";
                strDel = SS2.ActiveSheet.Cells[i, 0].Text.Trim().ToString().Trim();
                strBDate = SS2.ActiveSheet.Cells[i, 1].Text.Trim();
                strROWID2 = SS2.ActiveSheet.Cells[i, 7].Text.Trim();

                if(strROWID2 == "")
                {
                    if(strBuildDate.To<long>(0) >= ( VB.Left(strBDate, 4) + VB.Mid(strBDate, 6, 2) ).Trim().To<long>(0))
                    {
                        if (Strings.InStr(clsHcVariable.B08_MisuAdminSabun, "," + VB.Format(clsType.User.IdNumber.To<long>(0), "#0") + ",") == 0)
                        {
                            MessageBox.Show("이미 미수통계가 생성되었습니다. 등록할 수 없습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
                // 빌드작업이후 그달에 다시 등록할 경우
                if(strYYMM != "")
                {
                    if(i == SS2.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1)
                    {
                        if(strYYMM == VB.Left(strBDate, 4) + VB.Mid(strBDate, 6, 2))
                        {
                            if (Strings.InStr(clsHcVariable.B08_MisuAdminSabun, "," + VB.Format(clsType.User.IdNumber.To<long>(0), "#0") + ",") == 0)
                            {
                                Cancel();
                                MessageBox.Show("작업할 수 없습니다. 이달엔 작업불가.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                    }
                }

                strGeaCode = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                strGeaName = SS2.ActiveSheet.Cells[i, 3].Text.Trim();
                nSlipAmt = VB.Replace(SS2.ActiveSheet.Cells[i, 4].Text, ",", "").To<long>(0);
                strRemark = SS2.ActiveSheet.Cells[i, 5].Text.Trim();
                strChange =  SS2.ActiveSheet.Cells[i, 6].Text.Trim();
                strROWID =  SS2.ActiveSheet.Cells[i, 7].Text.Trim();

                strOk = "";
                if(strBDate != "") { strOk = "OK"; }
                if(strGeaCode != "")
                {
                    if(VB.Len(strGeaCode) != 2)
                    {
                        strOk = "OK";
                    }
                }
                if (nSlipAmt != 0) { strOk = "OK"; }
                if (strRemark != "") { strOk = "OK"; }
                if (strDel == "True") { strOk = ""; }

                if(strOk == "OK")
                {
                    if(strBDate == "")
                    {
                        MessageBox.Show(i + "번줄 변동내역의 입력일자가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if(strBDate.To<long>(0) < TxtDate.Text.To<long>(0))
                    {
                        MessageBox.Show(i + "번줄 변동일자가 발생일자보다 적음", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (strGeaCode == "")
                    {
                        MessageBox.Show(i + "번줄 변동내역의 계정이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (nSlipAmt == 0)
                    {
                        MessageBox.Show(i + "번줄 변동내역의 금액이 0원입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (strRemark == "")
                    {
                        MessageBox.Show(i + "번줄 변동내역의 적요란이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            if(FstrIpOk == "Y") // 입금완료시에는 작업할수없음
            {
                if (Strings.InStr(clsHcVariable.B08_MisuAdminSabun, "," + VB.Format(clsType.User.IdNumber.To<long>(0), "#0") + ",") == 0)
                {
                    Cancel();
                    MessageBox.Show("작업할 수 없습니다. 입금완료 되었습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private void Cancel()
        {
            Screen_Clear();
            txtWrtNo.Focus();
        }

        private void DisPlay_Screen(string WrtNo)
        {
            long nWrtno = 0;

            if (!WrtNo.IsNullOrEmpty())
            {
                nWrtno = WrtNo.To<long>(0);
            }
            else{
                nWrtno = 0;
            }
            
            string strYYMM_1 = "";

            FnCharAmt = 0;
            FstrCharDate = "";
            txtWrtNo.Text = txtWrtNo.Text.Trim();
            nWrtno = VB.Val(VB.Format(txtWrtNo.Text)).To<long>(0);
            if(VB.Val(txtWrtNo.Text) == 0)
            {
                MessageBox.Show("작업하실 미수번호가 없습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            strYYMM_1 = VB.Left(clsPublic.GstrSysDate, 4) + VB.Format(VB.Mid(clsPublic.GstrSysDate, 6, 2).To<long>(0) - 1, "00");

            if (!MST_Display())
            {
                return;
            }

            if (!SLIP_Display())
            {
                return;
            }

            if (!SUMMARY_Display())
            {
                return;
            }
            if (!Total_Amt_Gesan())
            {
                return;
            }

            FrameWRTNO.Enabled = false;
            FrameJong.Enabled = false;
            SS2.Enabled = true;
            SS3.Enabled = true;
            btnSearch.Enabled = false;
            btnCancel.Enabled = true;
            btnExit.Enabled = true;
            TxtLtdCode.Enabled = false;
            btnLtdHelp.Enabled = false;
            TxtKiho.Enabled = false;
            btnKihoHelp.Enabled = false;
            TxtDamName.Enabled = false;
            TxtJisa.Enabled = false;
            btnJisaHelp.Enabled = false;
            cboJONG.Enabled = false;
            TxtRemark.Enabled = false;
            cboMir.Enabled = false;
            TxtDate.Enabled = false;
            cboGbn.Enabled = false;
            TxtMisuAmt.Enabled = false;
            TxtGiroNo.Enabled = false;
            TxtChaAmt.Enabled = false;
            TxtPumMok.Enabled = false;
            TxtChaRemark.Enabled = false;
            TxtYYMM.Focus();

            // 미수관리자만 삭제 가능함
            btnDel.Enabled = false;
            if(Strings.InStr(clsHcVariable.B08_MisuAdminSabun, "," + VB.Format(clsType.User.IdNumber.To<long>(0), "#0") + ",") > 0)
            {
                btnDel.Enabled = true;
                SS2.Focus();
            }
        }

        private bool Total_Amt_Gesan()
        {
            // 누적 변수를 Clear
            FnMisuAmt = 0;
            FnIpgumAmt = 0;
            FnGamAmt = 0;
            FnSakAmt = 0;
            FnBanAmt = 0;
            FnJanAmt = 0;
            FnCardAmt = 0;
            // 2020-04-17 미수금액 입력란에 금액을 입력해도 금액이 0원으로 입력되어 주석 해제함
            // FnMisuAmt = Val(Format(TxtMisuAmt.Text)) (원 코딩)
            FnMisuAmt = VB.Replace(TxtMisuAmt.Text, ",", "").To<long>(0); //(변경 코딩)

            for (int i = 0; i < SS2.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1; i++)
            {
                strDel = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                strGeaCode = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                nAmt = VB.Replace(SS2.ActiveSheet.Cells[i, 4].Text.Trim(), ",", "").To<long>(0);
                if (strDel == "True")
                {
                    nAmt = 0;
                }
                if (nAmt != 0)
                {
                    switch (strGeaCode)
                    {
                        case "11":
                            FnMisuAmt = FnMisuAmt; // 2020-04-16(미수금액수정관련 추가)
                            break;
                        case "21":
                        case "22":
                        case "23":
                        case "24":
                        case "25":
                        case "26":
                        case "27":
                        case "28":
                        case "29":
                            FnIpgumAmt += nAmt;
                            break;
                        case "31":
                            FnGamAmt += nAmt;
                            break;
                        case "32":
                            FnSakAmt += nAmt;
                            break;
                        case "33":
                            FnBanAmt += nAmt;
                            break;
                        case "55":
                            FnCardAmt += nAmt;
                            break;
                        default:
                            FnBanAmt += nAmt;
                            break;
                    }
                }
            }

            FnIpgumAmt += FnCardAmt; // 입금액 = 현금입금 + 카드입금
            FnJanAmt = FnMisuAmt - FnIpgumAmt - FnGamAmt - FnSakAmt - FnBanAmt;
            
            SS1.ActiveSheet.Cells[0, 0].Text = VB.Format(FnMisuAmt, "###,###,###,##0"); 
            SS1.ActiveSheet.Cells[0, 1].Text = VB.Format(FnIpgumAmt, "###,###,###,##0");
            SS1.ActiveSheet.Cells[0, 2].Text = VB.Format(FnGamAmt, "###,###,###,##0");
            SS1.ActiveSheet.Cells[0, 3].Text = VB.Format(FnSakAmt, "###,###,###,##0");
            SS1.ActiveSheet.Cells[0, 4].Text = VB.Format(FnBanAmt, "###,###,###,##0");
            SS1.ActiveSheet.Cells[0, 5].Text = VB.Format(FnJanAmt, "###,###,###,##0");

            return true;
        }

        private bool SUMMARY_Display()
        {
            // 미수SUMMARY 내용을 Display
            List<HIC_MISU_MONTHLY> misuSummary = hicMisuMonthlyService.GetmisuSummary(txtWrtNo.Text.To<long>(0));
            SS3.ActiveSheet.RowCount = misuSummary.Count;

            for (int i = 0; i < SS3.ActiveSheet.RowCount; i++)
            {
                SS3.ActiveSheet.Cells[i, 0].Text = misuSummary[i].YYMM;
                SS3.ActiveSheet.Cells[i, 1].Text = VB.Format(misuSummary[i].IWOLAMT, "###,###,###,##0");
                SS3.ActiveSheet.Cells[i, 2].Text = VB.Format(misuSummary[i].MISUAMT, "###,###,###,##0");
                SS3.ActiveSheet.Cells[i, 3].Text = VB.Format(misuSummary[i].IPGUMAMT, "###,###,###,##0");
                SS3.ActiveSheet.Cells[i, 4].Text = VB.Format(misuSummary[i].GAMAMT, "###,###,###,##0");
                SS3.ActiveSheet.Cells[i, 5].Text = VB.Format(misuSummary[i].SAKAMT, "###,###,###,##0");
                SS3.ActiveSheet.Cells[i, 6].Text = VB.Format(misuSummary[i].BANAMT, "###,###,###,##0");
                SS3.ActiveSheet.Cells[i, 7].Text = VB.Format(misuSummary[i].JANAMT, "###,###,###,##0");
            }

            return true;
        }

        private bool SLIP_Display()
        {
            // 미수SLIP 내용을 Display
            List<HIC_MISU_MST_SLIP> misuSlip = hicMisuMstSlipService.GetMisuSlip(txtWrtNo.Text.To<long>(0));
            SS2.ActiveSheet.RowCount = misuSlip.Count + 20;

            for(int i = 0; i < misuSlip.Count; i++)
            {
                SS2.ActiveSheet.Cells[i, 1].Text = misuSlip[i].BDATE.Trim();
                SS2.ActiveSheet.Cells[i, 2].Text = misuSlip[i].GEACODE.Trim();
                SS2.ActiveSheet.Cells[i, 3].Text = cHM.READ_MisuGea_Name(misuSlip[i].GEACODE.Trim());
                SS2.ActiveSheet.Cells[i, 4].Text = misuSlip[i].SLIPAMT.To<string>("0");
                SS2.ActiveSheet.Cells[i, 5].Text = misuSlip[i].REMARK.Trim();
                SS2.ActiveSheet.Cells[i, 6].Text = ""; // 변경
                SS2.ActiveSheet.Cells[i, 7].Text = misuSlip[i].ROWID.Trim();
            }

            return true;
        }

        private bool MST_Display()
        {
            List<HIC_MISU_MST_SLIP> misuMaster = hicMisuMstSlipService.GetMisuMaster(txtWrtNo.Text.To<long>(0));

            if(misuMaster.Count == 0)
            {
                MessageBox.Show("해당 미수번호가 등록되지 않았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtWrtNo.Focus();
                return false;
            }

            FstrROWID = misuMaster[0].ROWID.Trim();

            switch (misuMaster[0].MISUJONG)
            {
                case "1":
                    rdoJong1.Checked = true;
                    break;
                case "2":
                    rdoJong2.Checked = true;
                    break;
                case "3":
                    rdoJong3.Checked = true;
                    break;
                case "4":
                    rdoJong4.Checked = true;
                    break;
            }

            TxtLtdCode.Text         =       VB.Format(misuMaster[0].LTDCODE, "#");
            PanelLtdName.Text       =       cHB.READ_Ltd_Name(TxtLtdCode.Text).Trim();
            TxtJisa.Text            =       misuMaster[0].JISA;
            PanelJisaName.Text      =       cHB.READ_HIC_CODE("21", TxtJisa.Text).Trim();
            TxtKiho.Text            =       misuMaster[0].KIHO;
            PanelKihoName.Text      =       cHB.READ_HIC_CODE("18", TxtKiho.Text).Trim();
            TxtYYMM.Text            =       misuMaster[0].YYMM_JIN;

            // 종류를 Display
            cboJONG.SelectedIndex = -1;
            for(int i = 0; i < cboJONG.Items.Count; i++)
            {
                if(VB.Left(cboJONG.Items[i].To<string>("0"), 2) == misuMaster[0].GJONG)
                {
                    cboJONG.SelectedIndex = i;
                    break;
                }
            }

            // 미수구분
            cboGbn.SelectedIndex = -1;
            for (int i = 0; i < cboGbn.Items.Count; i++)
            {
                if (VB.Left(cboGbn.Items[i].To<string>("0"), 1) == misuMaster[0].MISUGBN)
                {
                    cboGbn.SelectedIndex = i;
                    break;
                }
            }

            // 청구구분
            cboMir.SelectedIndex = -1;
            for (int i = 0; i < cboMir.Items.Count; i++)
            {
                if (VB.Left(cboMir.Items[i].To<string>("0"), 1) == misuMaster[0].MIRGBN)
                {
                    cboMir.SelectedIndex = i;
                    break;
                }
            }

            TxtDate.Text        =       misuMaster[0].BDATE.Trim();
            TxtMisuAmt.Text     =       VB.Format(misuMaster[0].MISUAMT, "###,###,###,##0");
            TxtGiroNo.Text      =       misuMaster[0].GIRONO.Trim();
            TxtDamName.Text     =       misuMaster[0].DAMNAME;
            TxtRemark.Text      =       misuMaster[0].REMARK.Trim();
            TxtPumMok.Text      =       misuMaster[0].PUMMOK;
            TxtChaAmt.Text      =       VB.Format(misuMaster[0].MIRCHAAMT, "###,###,###,##0");
            TxtChaRemark.Text   =       misuMaster[0].MIRCHAREMARK; 
            FnCharAmt           =       VB.Format(TxtChaAmt.Text, "###########0").To<long>(0);
            FstrCharDate        =       misuMaster[0].MIRCHADATE.To<string>("");
            FstrGbMisuBuild = "";
            if (misuMaster[0].GBMISUBUILD == "Y") { FstrGbMisuBuild = "Y"; }
            if (misuMaster[0].GBMISUBUILD2 == "Y") { FstrGbMisuBuild = "Y"; }
            if (misuMaster[0].GBMISUBUILD3 == "Y") { FstrGbMisuBuild = "Y"; }
            if (misuMaster[0].GBMISUBUILD4 == "Y") { FstrGbMisuBuild = "Y"; }

            if(FstrGbMisuBuild == "Y")
            {
                TxtLtdCode.Enabled = false;
                btnLtdHelp.Enabled = false;
                cboJONG.Enabled = false;
                // 2004-08-10일 수정 : 강주임과 상의후.
                cboMir.Enabled = false;
                TxtMisuAmt.Enabled = false;
                TxtChaAmt.Enabled = false;
                if(Strings.InStr(clsHcVariable.B08_MisuAdminSabun, "," + VB.Format(clsType.User.IdNumber.To<long>(0), "#0") + ",") > 0)
                {
                    cboMir.Enabled = true;
                    TxtMisuAmt.Enabled = true;
                    TxtChaAmt.Enabled = true;
                }
            }
            else
            {
                TxtLtdCode.Enabled = true;
                btnLtdHelp.Enabled = true;
                cboJONG.Enabled = true;
                cboMir.Enabled = true;
                TxtMisuAmt.Enabled = true;
                TxtChaAmt.Enabled = true;
            }

            // 금액을 Display
            SS1.ActiveSheet.Cells[0, 0].Text = VB.Format(misuMaster[0].MISUAMT, "###,###,###,##0");
            SS1.ActiveSheet.Cells[0, 1].Text = VB.Format(misuMaster[0].IPGUMAMT, "###,###,###,##0");
            SS1.ActiveSheet.Cells[0, 2].Text = VB.Format(misuMaster[0].GAMAMT, "###,###,###,##0");
            SS1.ActiveSheet.Cells[0, 3].Text = VB.Format(misuMaster[0].SAKAMT, "###,###,###,##0");
            SS1.ActiveSheet.Cells[0, 4].Text = VB.Format(misuMaster[0].BANAMT, "###,###,###,##0");
            SS1.ActiveSheet.Cells[0, 5].Text = VB.Format(misuMaster[0].JANAMT, "###,###,###,##0");
            FstrIpOk = misuMaster[0].GBEND.Trim();
            FnJanAmt = SS1.ActiveSheet.Cells[0, 5].Text.To<long>(0);

            return true;
        }
        
        private void Modify()
        {
            if (Strings.InStr(clsHcVariable.B08_MisuAdminSabun, "," + VB.Format(clsType.User.IdNumber.To<long>(0), "#0") + ",") == 0)
            {
                MessageBox.Show("권한이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FrameMst.Enabled = true;
            FrameJong.Enabled = true;
            if(TxtLtdCode.Enabled == true)
            {
                TxtLtdCode.Focus();
            }
        }

        private void New_Registration()
        {
            txtWrtNo.Text = "신규";
            FstrROWID = "";
            FrameWRTNO.Enabled = false;
            FrameMst.Enabled = true;
            FrameJong.Enabled = true;
            SS2.Enabled = true;
            SS3.Enabled = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDel.Enabled = false;
            btnExit.Enabled = false;
            TxtDate.Text = clsPublic.GstrSysDate;
            cboGbn.SelectedIndex = 0;
            TxtLtdCode.Focus();
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            SS2.ActiveSheet.Columns.Get(6).Visible = false; // 변경
            SS2.ActiveSheet.Columns.Get(7).Visible = false; // ROWID

            cboJONG.Clear();
            cHB.ComboJong_AddItem(cboJONG, false);

            cboGbn.Clear();
            cboGbn.Items.Add("1. 정상미수");
            cboGbn.Items.Add("2. 대손예상");
            cboGbn.Items.Add("3. 악성미수");
            cboGbn.Items.Add("4. 회수불능");

            cboMir.Clear();
            cboMir.Items.Add("1. 처음청구");
            cboMir.Items.Add("2. 재청구");
            cboMir.Items.Add("3. 추가청구");
            cboMir.Items.Add("4. 이의신청");
            cboMir.Items.Add("5. 기타청구");

            txtWrtNo.Text = "";
            Screen_Clear();

            btnDel.Enabled = false;
            if(Strings.InStr(clsHcVariable.B08_MisuAdminSabun, "," + VB.Format(clsType.User.IdNumber.To<long>(0), "#0") + ","  ) > 0)
            {
                btnDel.Enabled = true;
            }
            txtWrtNo.Focus();
        }

        private void Screen_Clear()
        {
            // 누적 변수를 Clear
            FnMisuAmt = 0;        
            FnIpgumAmt = 0;
            FnGamAmt = 0;         
            FnSakAmt = 0;
            FnBanAmt = 0;           
            FnJanAmt = 0;
            FnCardAmt = 0;
            FstrROWID = "";
            FstrGbMisuBuild = "";
            FnCharAmt = 0 ;
            FstrCharDate = "";

            // 미수마스타 내역
            txtWrtNo.Text = "";
            if (rdoClear1.Checked)
            {
                TxtLtdCode.Text = "";
                TxtJisa.Text = "";
                TxtKiho.Text = "";
                cboJONG.Text = "";
                TxtRemark.Text = "";
                TxtChaAmt.Text = "";
                cboGbn.Text = "";
                TxtPumMok.Text = "";
                PanelLtdName.Text = "";
                PanelJisaName.Text = "";
                PanelKihoName.Text = "";
                TxtGiroNo.Text = "";
                TxtChaRemark.Text = "";
                cboMir.Text = "";
                rdoJong1.Checked = false;
                rdoJong2.Checked = false;
                rdoJong3.Checked = false;
                rdoJong4.Checked = false;
                TxtLtdCode.Enabled = true;
                btnLtdHelp.Enabled = true;
                cboJONG.Enabled = true;
                cboMir.Enabled = true;
                TxtMisuAmt.Enabled = true;
                TxtChaAmt.Enabled = true;
            }
            TxtDate.Text = "";
            TxtMisuAmt.Text = "";
            TxtYYMM.Text = "";
            TxtDamName.Text = "";
            TxtDate.Text = "";

            SS_Clear(SS1_Sheet1);

            // 변동내역 Clear
            SS2.ActiveSheet.RowCount = 30;
            SS_Clear(SS2_Sheet1);
            PanelMsg.Text = "";

            // 월별 SUMMARY
            SS3.ActiveSheet.RowCount = 20;
            SS_Clear(SS3_Sheet1);

            // 버튼 Enabled Set
            FrameWRTNO.Enabled = true;
            FrameJong.Enabled = false;
            btnSearch.Enabled = true;

            btnCancel.Enabled = false;
            btnDel.Enabled = false;
            btnExit.Enabled = true;
            SS2.Enabled = false;
            SS3.Enabled = false;
        }

        private void SS_Clear(FarPoint.Win.Spread.SheetView Spd)
        {
            for (int i = 0; i < Spd.RowCount; i++)
            {
                for (int j = 0; j < Spd.ColumnCount; j++)
                {
                    Spd.Cells[i, j].Text = "";
                }
            }
        }

        private void TxtMisuAmt_TextChanged(object sender, EventArgs e)
        {
            try 
            {
                int num;
                string temp = TxtMisuAmt.Text.Replace(",", ""); 
                num = temp.To<int>(0);
                string k = string.Format("{0:#,###}", num); // num을 string으로 변환하면서 천단위 콤마 표시

                TxtMisuAmt.Text = k;
                TxtMisuAmt.SelectionStart = TxtMisuAmt.TextLength; //커서를 항상 맨뒤로
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void TxtChaAmt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int num;
                string temp = TxtChaAmt.Text.Replace(",", "");
                num = temp.To<int>(0);                 
                string k = string.Format("{0:#,###}", num); // num을 string으로 변환하면서 천단위 콤마 표시

                TxtChaAmt.Text = k;
                TxtChaAmt.SelectionStart = TxtChaAmt.TextLength; //커서를 항상 맨뒤로
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}