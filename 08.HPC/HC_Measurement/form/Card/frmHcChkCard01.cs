using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using HC.Core.Dto;
using HC.Core.Service;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace HC_Measurement
{
    public partial class frmHcChkCard01 :CommonForm
    {
        long FnWRTNO = 0;
        bool FbLtdSeq_New = false;
        bool FbDelYN = false;
        bool FbShowEx = false;
        string FstrRowid = "";

        HIC_LTD     LtdHelpItem     = null;
        clsSpread cSpd = null;

        HcUserService hcUserService = null;
        HicLtdService hicLtdService = null;
        HicChukMstNewService hicChukMstNewService = null;
        HicChukWorkerService hicChukWorkerService = null;
        HicChukDtlPlanService hicChukDtlPlanService = null;
        HicChukDtlChemicalService hicChukDtlChemicalService = null;
        HicChukDtlSubltdService hicChukDtlSubltdService = null;
        HcCodeService hcCodesService = null;
        ComHpcLibBService comHpcLibBService = null;
        HcCodeService hcCodeService = null;

        public frmHcChkCard01()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        /// <summary>
        /// 측정사업장 관리 기본 마스터
        /// </summary>
        /// <param name="argWRTNO">일련번호</param>
        /// <param name="bDelYN">삭제버튼 표시여부</param>
        /// <param name="bShowEx">닫기버튼 표시여부</param>
        public frmHcChkCard01(long argWRTNO, bool bDelYN = true, bool bShowEx = false)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FnWRTNO = argWRTNO;
            FbDelYN = bDelYN;
            FbShowEx = bShowEx;
        }

        private void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
            cSpd = new clsSpread();

            hcUserService = new HcUserService();
            hicLtdService = new HicLtdService();
            hicChukMstNewService = new HicChukMstNewService();
            hicChukWorkerService = new HicChukWorkerService();
            hicChukDtlPlanService = new HicChukDtlPlanService();
            hicChukDtlChemicalService = new HicChukDtlChemicalService();
            hicChukDtlSubltdService = new HicChukDtlSubltdService();
            hcCodesService = new HcCodeService();
            comHpcLibBService = new ComHpcLibBService();
            hcCodeService = new HcCodeService();

            txtLtdCode.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.LTDCODE), Min = 0 });
            nmrGjYear.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.CHKYEAR), Min = 0 });
            nmrLtdSeqNo.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.LTDSEQNO), Min = 0, Max = 999 });
            nmrTLimit.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.T_LIMIT), Min = 0, Max = 999 });
            nmrToAccum.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.TO_ACCUM), Min = 0, Max = 999 });
            nmrT5Accum.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.T5_ACCUM), Min = 0, Max = 999 });
            nmrT5Limit.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.T5_LIMIT), Min = 0, Max = 999 });

            chkGukgo.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBSUPPORT), CheckValue = "1", UnCheckValue = "0" });
            chkGbTemp.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBTEMP), CheckValue = "Y", UnCheckValue = "N" });
            chkEstimate.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBEST), CheckValue = "Y", UnCheckValue = "N" });

            //신규구분
            rdoNew1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBNEW), CheckValue = "2" });
            rdoNew2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBNEW), CheckValue = "1" });
            //반기
            rdoBangi1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.BANGI), CheckValue = "1" });
            rdoBangi2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.BANGI), CheckValue = "2" });
            //조사방법
            rdoWay1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBWAY), CheckValue = "1" });
            rdoWay2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBWAY), CheckValue = "2" });
            //예비조사일
            dtpBDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_CHUKMST_NEW.BDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.YYYY_MM_DD });
            //측정예상일자 Fr - To
            dtpChkSDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_CHUKMST_NEW.SDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.YYYY_MM_DD });
            dtpChkEDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_CHUKMST_NEW.EDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.YYYY_MM_DD });

            //진행상태
            List<HC_CODE> lstGBSTS = hcCodesService.FindActiveCodeByGroupCode("GBSTS", "WEM");
            cboGbSTS.SetOptions(new ComboBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBSTS) });
            cboGbSTS.SetItems(lstGBSTS, "CODENAME", "CODE");

            //지정한계 및 측정실적
            rdoProcChg1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_PROCS_NEW_CHANGE_YN), CheckValue = "0" });
            rdoProcChg2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_PROCS_NEW_CHANGE_YN), CheckValue = "1" });
            dtpProcChgDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_PROCS_NEW_CHANGE_DATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.YYYY_MM_DD });
            rdoWemRes1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_PROCS_WEM_RESULT), CheckValue = "0" });
            rdoWemRes2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_PROCS_WEM_RESULT), CheckValue = "1" });
            rdoWemRes3.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_PROCS_WEM_RESULT), CheckValue = "2" });
            rdoWemRes4.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_PROCS_WEM_RESULT), CheckValue = "3" });
            rdoCrngnYN1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_CRNGN_RDMTR_OVER_YN), CheckValue = "0" });
            rdoCrngnYN2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_CRNGN_RDMTR_OVER_YN), CheckValue = "1" });
            rdoChmclsYN1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_CHMCLS_RDMTR_OVER_YN), CheckValue = "0" });
            rdoChmclsYN2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_CHMCLS_RDMTR_OVER_YN), CheckValue = "1" });
            rdoFutrWemCycle1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_FUTR_WEM_CYCLE), CheckValue = "0" });
            rdoFutrWemCycle2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_FUTR_WEM_CYCLE), CheckValue = "1" });
            rdoFutrWemCycle3.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_FUTR_WEM_CYCLE), CheckValue = "2" });
            dtpWemPlanDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_FUTR_WEM_PLAN_DATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.YYYY_MM_DD });

            #region 측정자, 분석자 등록내역
            SS1.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 30 });
            SS1.AddColumnCheckBox("삭제", nameof(HIC_CHUK_WORKER.IsDelete), 44, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false }).ButtonClick += eSS1_ChkButtonClick;
            SS1.AddColumn("일련번호", nameof(HIC_CHUK_WORKER.WRTNO), 74, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("작업자사번", nameof(HIC_CHUK_WORKER.WORKER_SABUN), 74, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("작업자명", nameof(HIC_CHUK_WORKER.WORKER_NAME), 84, new SpreadCellTypeOption { });
            SS1.AddColumn("역할", nameof(HIC_CHUK_WORKER.ROLE), 120, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("자격번호", nameof(HIC_CHUK_WORKER.CERTNO), 220, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("비고", nameof(HIC_CHUK_WORKER.BIGO), 180, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("ROWID", nameof(HIC_CHUK_WORKER.RID), 74, new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            #region Control SetOption
            nmrILSU.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.ILSU), Min = 0 });
            nmrInwon.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.INWON), Min = 0 });
            nmrInwonS.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.INWON_S), Min = 0 });
            nmrInwonH.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.INWON_H), Min = 0 });
            nmrDaytime.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.DAYTIME), Min = 0 });
            nmrShiftGrpCnt.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.SHIFTGRPCNT), Min = 0 });
            nmrShiftQtr.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.SHIFTQUARTER), Min = 0 });
            nmrShiftTime.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.SHIFTTIME), Min = 0 });
            chkGbDay.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBDAY), CheckValue = "1", UnCheckValue = "0" });
            chkGbShift.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBSHIFT), CheckValue = "1", UnCheckValue = "0" });
            rdoOverTime1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBOVERTIME), CheckValue = "Y" });
            rdoOverTime2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBOVERTIME), CheckValue = "N" });
            rdoEstimate1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBESTIMATE), CheckValue = "Y" });
            rdoEstimate2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBESTIMATE), CheckValue = "N" });
            rdoCorrect1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBCORRECT), CheckValue = "Y" });
            rdoCorrect2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBCORRECT), CheckValue = "N" });
            rdoSample1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBSAMPLE), CheckValue = "Y" });
            rdoSample2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBSAMPLE), CheckValue = "N" });
            rdoChromium1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBCHROMIUM), CheckValue = "Y" });
            rdoChromium2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBCHROMIUM), CheckValue = "N" });

            chkUcode1.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBUCODE1), CheckValue = "1", UnCheckValue = "0" });
            chkUcode2.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBUCODE2), CheckValue = "1", UnCheckValue = "0" });
            chkUcode3.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBUCODE3), CheckValue = "1", UnCheckValue = "0" });
            chkUcode4.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBUCODE4), CheckValue = "1", UnCheckValue = "0" });
            chkUcode5.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBUCODE5), CheckValue = "1", UnCheckValue = "0" });
            chkUcode6.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBUCODE6), CheckValue = "1", UnCheckValue = "0" });
            #endregion

            panMain.SetEnterKey();

        }

        private void eSS1_ChkButtonClick(object sender, EditorNotifyEventArgs e)
        {
            HIC_CHUK_WORKER code = SS1.GetRowData(e.Row) as HIC_CHUK_WORKER;

            SS1.DeleteRow(e.Row);
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.SS1.EditModeOff += new EventHandler(eSpdEditModeOff);

            this.txtLtdCode.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtWorkTime11.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtWorkTime12.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtWorkTime21.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtWorkTime22.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtWorkTime31.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtWorkTime32.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtWorkTime41.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtWorkTime42.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMealTime11.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMealTime12.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMealTime21.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMealTime22.GotFocus += new EventHandler(eTxtGotFocus); 

            this.txtWorkTime11.Leave += new EventHandler(eTxtLeaveFocus);
            this.txtWorkTime12.Leave += new EventHandler(eTxtLeaveFocus);
            this.txtWorkTime21.Leave += new EventHandler(eTxtLeaveFocus);
            this.txtWorkTime22.Leave += new EventHandler(eTxtLeaveFocus);
            this.txtWorkTime31.Leave += new EventHandler(eTxtLeaveFocus);
            this.txtWorkTime32.Leave += new EventHandler(eTxtLeaveFocus);
            this.txtWorkTime41.Leave += new EventHandler(eTxtLeaveFocus);
            this.txtWorkTime42.Leave += new EventHandler(eTxtLeaveFocus);
            this.txtMealTime11.Leave += new EventHandler(eTxtLeaveFocus);
            this.txtMealTime12.Leave += new EventHandler(eTxtLeaveFocus);
            this.txtMealTime21.Leave += new EventHandler(eTxtLeaveFocus);
            this.txtMealTime22.Leave += new EventHandler(eTxtLeaveFocus);

            this.chkNewSeq.CheckedChanged += new EventHandler(eChkChanged);

            this.rdoFutrWemCycle1.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoFutrWemCycle2.CheckedChanged += new EventHandler(eRdoChkChanged);
            this.rdoFutrWemCycle3.CheckedChanged += new EventHandler(eRdoChkChanged);
        }

        private void eRdoChkChanged(object sender, EventArgs e)
        {
            if (sender == rdoFutrWemCycle1)
            {
                dtpWemPlanDate.Checked = true;
                dtpWemPlanDate.Value = dtpChkSDate.Value.AddMonths(3);
            }
            else if (sender == rdoFutrWemCycle2)
            {
                dtpWemPlanDate.Checked = true;
                dtpWemPlanDate.Value = dtpChkSDate.Value.AddMonths(6);
            }
            else if (sender == rdoFutrWemCycle3)
            {
                dtpWemPlanDate.Checked = true;
                dtpWemPlanDate.Value = dtpChkSDate.Value.AddYears(1);
            }
        }

        private void eTxtLeaveFocus(object sender, EventArgs e)
        {
            string strChk = ((MaskedTextBox)sender).Text.Trim();

            if (VB.Pstr(strChk, ":", 1).To<int>(0) > 23)
            {
                MessageBox.Show("시간 입력범위 초과", "입력오류");
                ((MaskedTextBox)sender).Focus();
                return;
            }
            else if (VB.Pstr(strChk, ":", 2).To<int>(0) > 59)
            {
                MessageBox.Show("분 입력범위 초과", "입력오류");
                ((MaskedTextBox)sender).Focus();
                return;
            }
        }

        private void eTxtGotFocus(object sender, EventArgs e)
        {
            ((MaskedTextBox)sender).SelectAll();
        }

        private void eChkChanged(object sender, EventArgs e)
        {
            if (sender == chkNewSeq)
            {
                if (chkNewSeq.Checked)
                {
                    FbLtdSeq_New = true;
                    MessageBox.Show("저장시 해당 사업장내 신규 순번을 부여합니다.", "신규순번 부여");
                }
                else
                {
                    FbLtdSeq_New = false;
                }
            }
        }

        private void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtLtdCode.Text.Trim() != "")
                {
                    //기본사업장 정보 출력
                    HIC_LTD dHLTD = hicLtdService.GetMailCodebyCode(txtLtdCode.Text);

                    if (!dHLTD.IsNullOrEmpty())
                    {
                        txtSangho.Text = dHLTD.NAME;
                        txtMail.Text = dHLTD.MAILCODE;
                        txtJuso.Text = dHLTD.JUSO + " " + dHLTD.JUSODETAIL;
                        txtDaepyo.Text = dHLTD.DAEPYO;
                        txtTel.Text = dHLTD.TEL;
                    }
                }
            }
        }

        private void eSpdEditModeOff(object sender, EventArgs e)
        {
            if (sender == SS1)
            {
                int nRow = SS1.ActiveSheet.ActiveRowIndex;
                string strSName = SS1.ActiveSheet.Cells[nRow, 3].Text;

                if (!strSName.IsNullOrEmpty())
                {
                    HC_USER item = hcUserService.FindByName(strSName);

                    if (!item.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[nRow, 1].Text = FnWRTNO.To<string>("0");
                        SS1.ActiveSheet.Cells[nRow, 2].Text = item.UserId;
                        SS1.ActiveSheet.Cells[nRow, 3].Text = item.Name;
                        SS1.ActiveSheet.Cells[nRow, 4].Text = item.Role;
                        SS1.ActiveSheet.Cells[nRow, 5].Text = item.CERTNO;

                        SS1.AddRows(1);
                    }
                }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSave)
            {
                Data_Save();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnDelete)
            {
                if (MessageBox.Show("측정사업장 내역을 전부 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                Data_Save("DEL");
            }
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
                return;
            }
        }

        private void Data_Save(string argMode = "")
        {
            if (!panMain.RequiredValidate())
            {
                MessageBox.Show("필수 입력항목이 누락되었습니다.");
                return;
            }

            if (txtLtdCode.Value <= 0)
            {
                MessageBox.Show("사업장 코드 입력안됨.");
                return;
            }

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                //측정사업장 등록
                HIC_CHUKMST_NEW item = panMain.GetData<HIC_CHUKMST_NEW>();
                item.RID = FstrRowid;
                item.WRTNO = FnWRTNO;
                item.ENTSABUN = clsType.User.IdNumber.To<long>(0);
                if (FnWRTNO == 0) { FnWRTNO = item.WRTNO; }

                if (argMode == "DEL")
                {
                    item.RowStatus = ComBase.Mvc.RowStatus.Delete;
                    item.DELSABUN = clsType.User.IdNumber.To<long>(0);

                    //측정자, 분석자 내역 등록
                    if (!hicChukWorkerService.DeleteAll(FnWRTNO))
                    {
                        MessageBox.Show("측정자 삭제중 오류가 발생하였습니다. ");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    //측정대상 공정 및 유해인자별 측정계획
                    if (!hicChukDtlPlanService.DeleteAll(FnWRTNO))
                    {
                        MessageBox.Show("측정대상 공정별 유해인자 삭제중 오류가 발생하였습니다. ");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    //화학물질 사용실태
                    if (!hicChukDtlChemicalService.DeleteAll(FnWRTNO))
                    {
                        MessageBox.Show("유해화학물질 삭제중 오류가 발생하였습니다. ");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    //협렵업체 내역
                    if (!hicChukDtlSubltdService.DeleteAll(FnWRTNO))
                    {
                        MessageBox.Show("협력업체 내역 삭제중 오류가 발생하였습니다. ");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
                else
                {
                    //사업장관리순번 신규
                    if (FbLtdSeq_New)
                    {
                        item.LTDSEQNO = hicChukMstNewService.GetMaxLtdSeqNoByLtdCode(item.LTDCODE);

                        int nCNT = hicChukMstNewService.GetChkCountByLtdCode(item.LTDCODE);
                        //측정횟수가 1회이상 있다면 
                        if (nCNT > 0)
                        {
                             item.LTDSEQNO += 1; 
                        }
                        
                    }

                    //누적치 계산 (신규등록시 계산)
                    if (FnWRTNO == 0)
                    {
                        nmrToAccum.Value = hicChukMstNewService.GetTotAccumByBangiYear(item.BANGI, item.CHKYEAR);       //총누적
                        nmrT5Accum.Value = hicChukMstNewService.GetT5AccumByBangiYear(item.BANGI, item.CHKYEAR);       //5인이상 누적
                        nmrT5Limit.Value = hicChukMstNewService.GetT5LimitByBangiYear(item.BANGI, item.CHKYEAR);       //국고누적
                    }
                }

                hicChukMstNewService.Save(item);

                //측정자, 분석자 내역 등록
                IList<HIC_CHUK_WORKER> list = SS1.GetEditbleData<HIC_CHUK_WORKER>();

                if (list.Count > 0)
                {
                    if (argMode == "DEL")
                    {
                        foreach (HIC_CHUK_WORKER code in list)
                        {
                            code.RowStatus = ComBase.Mvc.RowStatus.Delete;
                        }
                    }

                    if (!hicChukWorkerService.Save(list, FnWRTNO))
                    {
                        MessageBox.Show("측정자 등록중 오류가 발생하였습니다. ");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("저장완료. ");

                Screen_Display(FnWRTNO);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private void Ltd_Code_Help()
        {
            string strFind = "";

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind, "측정");
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();
            frm.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);

            if (!LtdHelpItem.IsNullOrEmpty() && LtdHelpItem.CODE > 0)
            {
                txtLtdCode.Text = LtdHelpItem.CODE.To<string>();
                txtSangho.Text = LtdHelpItem.NAME;
                txtMail.Text = LtdHelpItem.MAILCODE;
                txtJuso.Text = LtdHelpItem.JUSO + " " + LtdHelpItem.JUSODETAIL;
                txtDaepyo.Text = LtdHelpItem.DAEPYO;
                txtTel.Text = LtdHelpItem.TEL;
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            this.panMain.AddRequiredControl(nmrGjYear);
            this.panMain.AddRequiredControl(txtLtdCode);

            panMain.Initialize();

            SS1.DataSource = new List<HIC_CHUK_WORKER>();
            SS1.AddRows(1);
            
            nmrGjYear.Value = DateTime.Now.Year;
            cboGbSTS.SelectedIndex = 0;

            //포항성모병원 지정한계치
            nmrTLimit.Value = 480;
            btnDelete.Visible = FbDelYN;
            btnSave.Visible = FbDelYN;
            btnExit.Visible = FbShowEx;

            if (FnWRTNO > 0)
            {
                Screen_Display(FnWRTNO);
            }
        }

        private void Screen_Display(long argWRTNO)
        {
            if (argWRTNO == 0) { return; }

            //계약내용이 있다면 출력
            HIC_CHUKMST_NEW item = hicChukMstNewService.GetItemByWrtno(argWRTNO);

            if (!item.IsNullOrEmpty())
            {
                panMain.SetData(item);

                if (!item.CYCLE_FUTR_WEM_PLAN_DATE.IsNullOrEmpty())
                {
                    dtpWemPlanDate.Value = (DateTime)item.CYCLE_FUTR_WEM_PLAN_DATE;
                }
                
                //기본사업장 정보 출력
                HIC_LTD dHLTD = hicLtdService.GetMailCodebyCode(item.LTDCODE.To<string>("0"));

                if (!dHLTD.IsNullOrEmpty())
                {
                    txtSangho.Text = dHLTD.NAME;
                    txtMail.Text = dHLTD.MAILCODE;
                    txtJuso.Text = dHLTD.JUSO + " " + dHLTD.JUSODETAIL;
                    txtDaepyo.Text = dHLTD.DAEPYO;
                    txtTel.Text = dHLTD.TEL;
                }

                FstrRowid = item.RID;

                //사업장 측정자, 분석자 출력
                List<HIC_CHUK_WORKER> lsthCW = hicChukWorkerService.GetListByWrtno(argWRTNO);
                SS1.DataSource = lsthCW;
                SS1.AddRows(1);
            }
        }
    }
}
