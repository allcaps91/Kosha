using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using ComHpcLibB;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using HC.Core.Common.Util;
using HC.Core.Dto;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Model;
using HC_Measurement.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HC_Measurement
{
    public partial class frmHcChkEstimate :CommonForm
    {
        HicChukMstNewService hicChukMstNewService = null;
        HicChkMcodeService hicChkMcodeService = null;
        HicChkSugaService hicChkSugaService = null;
        HicChukEstimateService hicChukEstimateService = null;
        HicChukDtlResultService hicChukDtlResultService = null;
        HicChukDtlPlanService hicChukDtlPlanService = null;
        HC_CODE sendMailAddress;

        clsHcFunc cHF = null;
        clsSpread cSpd = null;

        long FnWRTNO = 0;

        public frmHcChkEstimate()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            long nYEAR = DateTime.Now.Year;

            hicChukMstNewService = new HicChukMstNewService();
            hicChkMcodeService = new HicChkMcodeService();
            hicChkSugaService = new HicChkSugaService();
            hicChukEstimateService = new HicChukEstimateService();
            hicChukDtlResultService = new HicChukDtlResultService();
            hicChukDtlPlanService = new HicChukDtlPlanService();

            cHF = new clsHcFunc();
            cSpd = new clsSpread();

            SSList.Initialize(new SpreadOption() { ColumnHeaderHeight = 34, RowHeight = 30, RowHeaderVisible = true });

            SSList.AddColumn("번호",           nameof(HIC_CHUKMST_NEW.WRTNO),       46, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SSList.AddColumn("년도",           nameof(HIC_CHUKMST_NEW.CHKYEAR),     46, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SSList.AddColumn("반기",           nameof(HIC_CHUKMST_NEW.BANGI),       52, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SSList.AddColumn("사업장코드",     nameof(HIC_CHUKMST_NEW.LTDCODE),     42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("사업장명",       nameof(HIC_CHUKMST_NEW.LTDNAME),    180, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("측정일자 Fr",    nameof(HIC_CHUKMST_NEW.SDATE),       92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("측정일자 To",    nameof(HIC_CHUKMST_NEW.EDATE),       92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("ROWID",          nameof(HIC_CHUKMST_NEW.RID),         42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });

            #region 측정사업장 견적서 내역
            //SpreadComboBoxData CHK_SUCODE = hicChkSugaService.GetSpreadComboBoxData(nYEAR);

            ssEST.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 38 });
            ssEST.AddColumnCheckBox("삭제",        nameof(HIC_CHUK_ESTIMATE.IsDelete),    42, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false }).ButtonClick += eSSEST_ChkButtonClick;   //0
            ssEST.AddColumn("일련번호",            nameof(HIC_CHUK_ESTIMATE.WRTNO),       74, new SpreadCellTypeOption { IsVisivle = false });
            ssEST.AddColumnNumber("순번",          nameof(HIC_CHUK_ESTIMATE.SEQNO),       42, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            ssEST.AddColumn("항목", nameof(HIC_CHUK_ESTIMATE.SUNAME), 130, new SpreadCellTypeOption { IsVisivle = false });
            ssEST.AddColumn("물질코드", nameof(HIC_CHUK_ESTIMATE.MCODE), 130, new SpreadCellTypeOption { IsVisivle = false });
            ssEST.AddColumn("항목명", nameof(HIC_CHUK_ESTIMATE.MCODE_NM), 130, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });                                        //5
            ssEST.AddColumn("분석방법", nameof(HIC_CHUK_ESTIMATE.ANALWAY_NM), 130, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            ssEST.AddColumnNumber("단가",          nameof(HIC_CHUK_ESTIMATE.PRICE),       68, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right, Mask = "###,###,##0" }); //7
            ssEST.AddColumnNumber("건수",          nameof(HIC_CHUK_ESTIMATE.QTY),         42, new SpreadCellTypeOption { });                                                                                  
            ssEST.AddColumnNumber("공급가액",      nameof(HIC_CHUK_ESTIMATE.AMT),         70, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Right, Mask = "###,###,##0" });
            ssEST.AddColumn("비고",                nameof(HIC_CHUK_ESTIMATE.REMARK),      92, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });                                            //10
            ssEST.AddColumn("수가코드",            nameof(HIC_CHUK_ESTIMATE.SUCODE),      92, new SpreadCellTypeOption { IsVisivle = false });                                                               
            ssEST.AddColumn("ROWID",               nameof(HIC_CHUK_ESTIMATE.RID),         74, new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            sendMailAddress = codeService.FindActiveCodeByGroupAndCode("WEM_EMAIL", "mail", "WEM");
        }

        private void eSSEST_ChkButtonClick(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column == 0)
            {
                HIC_CHUK_ESTIMATE code = ssEST.GetRowData(e.Row) as HIC_CHUK_ESTIMATE;

                ssEST.DeleteRow(e.Row);
            }
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnAdd_Est.Click += new EventHandler(eBtnClick);
            this.btnAmt.Click += new EventHandler(eBtnClick);
            this.btnEstSendMail.Click += new EventHandler(eBtnClick);
            this.btnEstSendMail_Imsi.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);

            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
            this.ssEST.EditModeOff += new EventHandler(eSpdEditModeOff);

            this.txtChaPercent.ValueChanged += new EventHandler(eTxtValChanged);
            this.txtChaAmt1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        private void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtChaAmt1)
            {
                if (e.KeyChar == 13)
                {
                    long nBillAmt = VB.Replace(txtBillAmt.Text, ",", "").To<long>(0);
                    long nHalinAmt = txtChaAmt1.Value.To<long>(0);
                    long nHalinAmt2 = VB.Replace(txtChaAmt2.Text, ",", "").To<long>(0);

                    txtChaAmt.Text = VB.Format(nHalinAmt + nHalinAmt2, "###,###,##0");

                    txtAmt.Text = VB.Format(nBillAmt - nHalinAmt - nHalinAmt2, "###,###,##0");
                }
            }
        }

        private void eTxtValChanged(object sender, EventArgs e)
        {
            if (sender == txtChaPercent)
            {
                long nPer = txtChaPercent.Value.To<long>(0);
                long nBillAmt = VB.Replace(txtBillAmt.Text, ",", "").To<long>(0);
                long nHalinAmt = txtChaAmt1.Value.To<long>(0);
                long nHalinAmt2 = ((long)Math.Truncate(nBillAmt * (nPer / 100.0) / 10) * 10);

                txtChaAmt.Text = VB.Format(nHalinAmt + nHalinAmt2, "###,###,##0"); 
                txtChaAmt2.Text = VB.Format(nHalinAmt2, "###,###,##0");

                txtAmt.Text = VB.Format(nBillAmt - nHalinAmt - nHalinAmt2, "###,###,##0");
            }
        }

        private void eSpdEditModeOff(object sender, EventArgs e)
        {
            if (sender == ssEST)
            {
                int nRow = ssEST.ActiveSheet.ActiveRow.Index;
                int nCol = ssEST.ActiveSheet.ActiveColumn.Index;

                if (nCol == 7 || nCol == 8)
                {
                    long nPRICE = ssEST.ActiveSheet.Cells[nRow, 7].Value.To<long>(0);
                    long nQTY = ssEST.ActiveSheet.Cells[nRow, 8].Value.To<long>(0);
                    long nAMT = nPRICE * nQTY;

                    ssEST.ActiveSheet.Cells[nRow, 9].Value = nAMT;
                }
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            int nRow = 0;
            
            if (sender == SSList)
            {
                Screen_Clear();

                long nWRTNO = SSList.ActiveSheet.Cells[e.Row, 0].Text.To<long>(0);
                long nYear = SSList.ActiveSheet.Cells[e.Row, 1].Text.To<long>(0);

                FnWRTNO = nWRTNO;

                //최종작업내역
                HIC_CHUKMST_NEW hCMN = hicChukMstNewService.GetItemByWrtno(nWRTNO);

                if (!hCMN.IsNullOrEmpty())
                {
                    lblLastJobTime.Text = hCMN.EST_DATE.To<string>("");
                    lblLastJobName.Text = hCMN.EST_JOBNAME;
                    txtLtdManager.Text = hCMN.LTD_MANAGER;
                    txtReceiveEmail.Text = hCMN.LTD_EMAIL;
                }
                
                //견적서 내역
                List<HIC_CHUK_ESTIMATE> lstHCEST = hicChukEstimateService.GetItemByWrtno(nWRTNO);

                if (lstHCEST.Count > 0)
                {
                    ssEST.DataSource = lstHCEST;
                }
                else
                {
                    //작성된 견적서가 없으면 측정계획 내용 자동 Display 및 기본계산
                    List<HIC_CHUKDTL_PLAN_SUGA> lstSUGA = hicChukDtlPlanService.GetAccountByPlan(nWRTNO, nYear);

                    if (lstSUGA.Count > 0)
                    {
                        List<HIC_CHUK_ESTIMATE> lstHCE = new List<HIC_CHUK_ESTIMATE>();
                        HIC_CHUK_ESTIMATE hCE = null;
                        
                        //기본관리료 계산 및 추가
                        ADD_BASIC_SUGA(lstHCE, hCE, ref nRow, nWRTNO, hCMN);

                        for (int i = 0; i < lstSUGA.Count; i++)
                        {
                            nRow += 1;

                            hCE = new HIC_CHUK_ESTIMATE();

                            hCE.WRTNO = nWRTNO;
                            hCE.SEQNO = nRow;
                            hCE.SUCODE = lstSUGA[i].SUCODE;
                            hCE.SUNAME = lstSUGA[i].SUNAME;
                            hCE.MCODE = lstSUGA[i].MCODE;
                            hCE.MCODE_NM = lstSUGA[i].MCODE_NM;
                            hCE.ANALWAY_NM = lstSUGA[i].ANALWAY_NM;

                            if (hCMN.GBSUPPORT == "1")
                            {
                                hCE.PRICE = lstSUGA[i].GAMT;
                            }
                            else
                            {
                                hCE.PRICE = lstSUGA[i].AMT;
                            }
                            
                            hCE.QTY = lstSUGA[i].CHKCOUNT;
                            hCE.AMT = hCE.PRICE * lstSUGA[i].CHKCOUNT;

                            lstHCE.Add(hCE);
                        }

                        ssEST.DataSource = lstHCE;

                    }
                }

                for (int i = 0; i < ssEST.ActiveSheet.RowCount; i++)
                {
                    if (ssEST.ActiveSheet.Cells[i, 0].Text == "Y")
                    {
                        ssEST.DeleteRow(i);
                    }
                }

                //계산내역
                Display_Amt_Gesan(nWRTNO);

                long nTotAmt = 0;

                for (int i = 0; i < ssEST.ActiveSheet.RowCount; i++)
                {
                    nTotAmt += ssEST.ActiveSheet.Cells[i, 9].Value.To<long>();
                }

                lblLineTotAmt.Text = "합계 : " + VB.Format(nTotAmt, "###,###,##0");
            }
        }

        private void Display_Amt_Gesan(long nWRTNO)
        {
            txtBaseAmt.Text = "0";
            txtChargeAmt.Text = "0";
            txtBillAmt.Text = "0";
            txtChaPercent.Value = 0;
            txtChaAmt1.Value = 0;
            txtChaAmt2.Text = "0";
            txtChaAmt.Text = "0";
            txtAmt.Text = "0";
            txtPrtTime.Text = "";
            txtSendTime.Text = "";

            HIC_CHUK_ESTIMATE hCEST = hicChukEstimateService.GetSumEstAmtByWrtno(nWRTNO);

            if (!hCEST.IsNullOrEmpty())
            {
                txtBaseAmt.Text = VB.Format(hCEST.BASEAMT, "###,###,##0");
                txtChargeAmt.Text = VB.Format(hCEST.CHARGEAMT, "###,###,##0");
                txtBillAmt.Text = VB.Format(hCEST.TOTAMT, "###,###,##0");
                txtChaPercent.Value = hCEST.PER;
                txtChaAmt1.Value = hCEST.HALINAMT1;
                txtChaAmt2.Text = VB.Format(hCEST.HALINAMT2, "###,###,##0");
                txtChaAmt.Text = VB.Format(hCEST.HALINAMT, "###,###,##0");
                txtAmt.Text = VB.Format(hCEST.AMT, "###,###,##0");
            }

            HIC_CHUK_ESTIMATE hCEST2 = hicChukEstimateService.GetSendInfoByWrtno(nWRTNO);

            if (!hCEST2.IsNullOrEmpty())
            {
                txtPrtTime.Text = hCEST2.PRINTDATE.To<string>("");
                txtSendTime.Text = hCEST2.SENDTIME.To<string>("");
            }
        }

        private void ADD_BASIC_SUGA(List<HIC_CHUK_ESTIMATE> lstHCE, HIC_CHUK_ESTIMATE hCE, ref int nRow, long nWRTNO, HIC_CHUKMST_NEW hCMN)
        {
            string strSuCode= "";

            //기본관리비 추가 로직
            if (hCMN.GBSUPPORT == "1") { strSuCode = "A010"; }
            else if (hCMN.INWON < 5) { strSuCode = "A001"; }
            else if (hCMN.INWON >= 5 && hCMN.INWON <= 49) { strSuCode = "A002"; }
            else if (hCMN.INWON >= 50 && hCMN.INWON <= 99) { strSuCode = "A003"; }
            else if (hCMN.INWON >= 100 && hCMN.INWON <= 299) { strSuCode = "A004"; }
            else if (hCMN.INWON >= 300 && hCMN.INWON <= 499) { strSuCode = "A005"; }
            else if (hCMN.INWON >= 500 && hCMN.INWON <= 999) { strSuCode = "A006"; }
            else if (hCMN.INWON >= 1000 && hCMN.INWON <= 1999) { strSuCode = "A007"; }
            else if (hCMN.INWON >= 2000 && hCMN.INWON <= 2999) { strSuCode = "A008"; }
            else if (hCMN.INWON >= 3000) { strSuCode = "A009"; }

            HIC_CHK_SUGA hCS = hicChkSugaService.GetItemByCode(strSuCode);

            if (!hCS.IsNullOrEmpty())
            {
                nRow += 1;
                hCE = new HIC_CHUK_ESTIMATE();

                hCE.WRTNO = nWRTNO;
                hCE.SEQNO = nRow;
                hCE.SUCODE = strSuCode;
                hCE.SUNAME = hCS.SUNAME;
                hCE.MCODE_NM = hCS.SUNAME;

                if (hCMN.GBSUPPORT == "1") { hCE.PRICE = hCS.GAMT; }
                else { hCE.PRICE = hCS.AMT; }

                hCE.QTY = hCMN.ILSU;
                hCE.AMT = hCE.PRICE * hCMN.ILSU;

                lstHCE.Add(hCE);
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnPrint)
            {
                DisPlay_Estimate();
                Print_Page(ssPrt);
            }
            else if (sender == btnSave)
            {
                Save_Data();
            }
            else if (sender == btnSearch)
            {
                Display_List();
            }
            else if (sender == btnAdd_Est)
            {
                ssEST.AddRows(1);
            }
            else if (sender == btnAmt)
            {
                Process_Amt_Gesan();
            }
            else if (sender == btnEstSendMail)
            {
                DisPlay_Estimate();
                Print_Page(ssPrt);
                Estimate_Send_EMail();
            }
            else if (sender == btnEstSendMail_Imsi)
            {
                DisPlay_Estimate("임시");
                Print_Page(ssPrt);
                Estimate_Send_EMail();
            }
            else if (sender == btnDelete)
            {
                if (FnWRTNO == 0) { return; }

                if (MessageBox.Show("견적서 내용을 삭제하시겠습니까?", "작업확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Delete_Estimate(FnWRTNO);
                }
            }
        }

        private void Estimate_Send_EMail()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //HIC_CHUK_ESTIMATE dto = PanEstimate.GetData<HIC_CHUK_ESTIMATE>();
                HIC_CHUKMST_NEW dto = hicChukMstNewService.GetItemByWrtno(FnWRTNO);

                if (FnWRTNO > 0)
                {
                    HC_CODE pdfPath = codeService.FindActiveCodeByGroupAndCode("PDF_PATH", "OSHA_ESTIMATE", "OSHA");
                    SpreadPrint print = new SpreadPrint(ssPrt, PrintStyle.STANDARD_APPROVAL, false);

                    string pdfFileName = pdfPath.CodeName + "\\" + dto.LTDNAME +"_" + DateTime.Now.ToShortDateString().Replace("-", "") + FnWRTNO + ".pdf";
                    print.ExportPDFNoWait(pdfFileName, ssPrt.ActiveSheet);

                    frmHcChkEstimateMailForm form = new frmHcChkEstimateMailForm();
                    form.SelectedSite = base.SelectedSite;
                    form.GetMailForm().SenderMailAddress = sendMailAddress.CodeName;

                    string[] receiver = txtReceiveEmail.Text.Split(',');
                    foreach (string email in receiver)
                    {
                        form.GetMailForm().ReciverMailSddress.Add(email);
                    }

                    form.GetMailForm().AttachmentsList.Add(pdfFileName);
                    DialogResult result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        txtSendTime.Text = DateTime.Now.ToString();

                        string strLtdMgr = txtLtdManager.Text;
                        string strLtdAddr = txtReceiveEmail.Text;
                        long nSeqno = hicChukEstimateService.GetMaxSeqNoByWrtno(FnWRTNO);
                        string strRowid = hicChukEstimateService.GetRowidEstAmtByWrtno(FnWRTNO, nSeqno);

                        hicChukEstimateService.UpdateSendMail(strLtdMgr, strLtdAddr, strRowid);
                    }

                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageUtil.Alert(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void Delete_Estimate(long fnWRTNO)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                //견적서 내역 삭제
                if (!hicChukEstimateService.DeleteAll(FnWRTNO))
                {
                    MessageBox.Show("견적서자료 삭제 중 오류가 발생하였습니다. ");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //견적서 금액 삭제
                HIC_CHUK_ESTIMATE hCE = hicChukEstimateService.GetSumEstAmtByWrtno(FnWRTNO);
                long nSeqno = hicChukEstimateService.GetMaxSeqNoByWrtno(FnWRTNO);
                hCE.SEQNO = nSeqno;
                if (!hicChukEstimateService.DeleteAmt(hCE, FnWRTNO))
                {
                    MessageBox.Show("견적서금액 삭제 중 오류가 발생하였습니다. ");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //측정사업장 마스터에 견적정보 Clear
                if (!hicChukMstNewService.UpDateEstInfoDel(FnWRTNO))
                {
                    MessageBox.Show("HIC_CHUKMST_NEW 견적서정보 삭제 중 오류가 발생하였습니다. ");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("삭제완료. ");

                Screen_Clear();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void DisPlay_Estimate(string argMark = "")
        {
            string strYear = "";
            string strMonth = "";
            string strDay = "";
            StringBuilder sbTxt = new StringBuilder();

            long nBAmt = 0;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                ssPrt_Clear();

                HIC_CHUKMST_NEW dto = hicChukMstNewService.GetItemByWrtno(FnWRTNO);

                if (dto.GBEST.Equals("Y"))
                {
                    strYear = VB.Left(Convert.ToDateTime(dto.EST_DATE).ToShortDateString(), 4);
                    strMonth = VB.Mid(Convert.ToDateTime(dto.EST_DATE).ToShortDateString(), 6, 2);
                    strDay = VB.Mid(Convert.ToDateTime(dto.EST_DATE).ToShortDateString(), 9, 2);

                    sbTxt.Append("서기 ");
                    sbTxt.Append(strYear);
                    sbTxt.Append("년 ");
                    sbTxt.Append(strMonth);
                    sbTxt.Append("월 ");
                    sbTxt.Append(strDay);
                    sbTxt.Append("일 ");

                    ssPrt.SetCellText(2, 1, sbTxt.ToString());   //견적서 발행일자
                    ssPrt.SetCellText(3, 1, dto.LTDNAME);   //사업장명

                    sbTxt.Clear();
                    sbTxt.Append("작업환경측정 수수료를 아래와 같이 견적 합니다. (");
                    sbTxt.Append(dto.CHKYEAR);
                    sbTxt.Append("년 ");
                    sbTxt.Append(dto.BANGI == "1" ? "상반기)" : "하반기)");

                    ssPrt.SetCellText(4, 1, sbTxt.ToString());

                    nBAmt = hicChukEstimateService.GetEstAmtSumByWrtno(FnWRTNO);

                    ssPrt.SetCellText(6, 2, cHF.Number2Hangle(nBAmt) + " 정");

                    int nRow = ssEST.ActiveSheet.RowCount;
                    string strTmp = string.Empty;

                    //견적건수가 견적서 칸보다 많을시 칸 추가
                    if (nRow > 19)
                    {
                        int nCNT = nRow - 19;
                        ssPrt.ActiveSheet.ActiveRowIndex = 8;

                        //할인금액, 기본관리비, 분석수수료, 합계 ... 4칸 더 줌
                        ssPrt.AddRows(nCNT + 4);
                    }

                    int nPrtRow = 8;
                    
                    //견적건 견적서에 내용 복사
                    for (int i = 0; i < nRow; i++)
                    {
                        if (ssEST.GetCellText(i, 0) != "Y")
                        {
                            //취급물질명
                            strTmp = ssEST.GetCellText(i, 5);
                            if (strTmp.IsNullOrEmpty())
                            {
                                strTmp = ssEST.GetCellText(i, 3);
                            }
                            ssPrt.SetCellText(nPrtRow, 1, strTmp);

                            //분석방법
                            strTmp = ssEST.GetCellText(i, 6);
                            ssPrt.SetCellText(nPrtRow, 2, strTmp);

                            //수량
                            strTmp = ssEST.GetCellText(i, 8);
                            ssPrt.SetCellText(nPrtRow, 3, strTmp);

                            //단가
                            strTmp = ssEST.GetCellText(i, 7);
                            ssPrt.SetCellText(nPrtRow, 4, strTmp);

                            //금액
                            strTmp = ssEST.GetCellText(i, 9);
                            ssPrt.SetCellText(nPrtRow, 6, strTmp);

                            //비고
                            strTmp = ssEST.GetCellText(i, 10);
                            ssPrt.SetCellText(nPrtRow, 9, strTmp);

                            nPrtRow += 1;
                        }
                    }

                    string strHalRemark = txtChaPercent.Value.To<string>("") + "%감액";
                    string strHailAmt = VB.Format((VB.Replace(txtChaAmt.Text, ",", "").To<long>(0) * -1), "###,###,##0");

                    //할인금액 표시
                    if (strHailAmt.To<long>(0) > 0)
                    {
                        ssPrt.SetCellText(nPrtRow, 1, "감액");         //항목
                        ssPrt.SetCellText(nPrtRow, 3, "1");            //건수    
                        ssPrt.SetCellText(nPrtRow, 4, strHailAmt);     //단가    
                        ssPrt.SetCellText(nPrtRow, 6, strHailAmt);     //공급가액
                        ssPrt.SetCellText(nPrtRow, 9, strHalRemark);   //비고    
                        nPrtRow += 1;
                    }
                    
                    //기본관리비 표시
                    ssPrt.SetCellText(nPrtRow, 1, "기본관리비");   //항목
                    ssPrt.SetCellText(nPrtRow, 3, "1");            //건수     
                    ssPrt.SetCellText(nPrtRow, 6, txtBaseAmt.Text);     //공급가액

                    nPrtRow += 1;
                    //분석수수료 표시
                    ssPrt.SetCellText(nPrtRow, 1, "분석수수료");         //항목  
                    ssPrt.SetCellText(nPrtRow, 6, txtChargeAmt.Text);    //공급가액    

                    nPrtRow += 1;
                    //합계 표시
                    ssPrt.SetCellText(nPrtRow, 1, "합계");         //항목
                    ssPrt.SetCellText(nPrtRow, 6, txtAmt.Text);    //공급가액

                    if (!argMark.IsNullOrEmpty())
                    {
                        int nRow2 = ssPrt.ActiveSheet.RowCount - 1;
                        ssPrt.ActiveSheet.Rows.Get(nRow2).Visible = false;
                    }
                    else
                    {
                        int nRow2 = ssPrt.ActiveSheet.RowCount - 1;
                        ssPrt.ActiveSheet.Rows.Get(nRow2).Visible = true;
                    }
                }
                else
                {
                    MessageBox.Show("견적서 내용이 저장되지 않았습니다.", "메일발송 불가");
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void Process_Amt_Gesan()
        {
            long nAMT = 0;
            long nBaseAmt = 0;
            long nHalinAmt = 0;

            for (int i = 0; i < ssEST.ActiveSheet.RowCount; i++)
            {
                if (VB.Left(ssEST.ActiveSheet.Cells[i, 11].Text, 1) == "A")
                {
                    nBaseAmt += ssEST.ActiveSheet.Cells[i, 9].Value.To<long>(0);
                }
                else
                {
                    nAMT += ssEST.ActiveSheet.Cells[i, 9].Value.To<long>(0);
                }
            }

            nHalinAmt = VB.Replace(txtChaAmt.Text, ",", "").To<long>(0);

            txtBaseAmt.Text = VB.Format(nBaseAmt, "###,###,##0");
            txtChargeAmt.Text = VB.Format(nAMT, "###,###,##0");
            txtBillAmt.Text = VB.Format(nBaseAmt + nAMT, "###,###,##0");
            txtAmt.Text = VB.Format(nBaseAmt + nAMT - nHalinAmt, "###,###,##0");
            
            long nTotAmt = 0;

            for (int i = 0; i < ssEST.ActiveSheet.RowCount; i++)
            {
                nTotAmt += ssEST.ActiveSheet.Cells[i, 9].Value.To<long>();
            }

            lblLineTotAmt.Text = "합계 : " + VB.Format(nTotAmt, "###,###,##0");
        }

        private void Save_Data()
        {
            try
            {
                //체크로직
                if (Check_Estimate_Amt() == false)
                {
                    if (MessageBox.Show("견적서 금액과 계산금액이 서로 다릅니다. 그래도 저장하시겠습니까?", "금액확인", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }

                clsDB.setBeginTran(clsDB.DbCon);

                //견적서내역 저장
                List<HIC_CHUK_ESTIMATE> lstEST = new List<HIC_CHUK_ESTIMATE>();

                for (int i = 0; i < ssEST.ActiveSheet.RowCount; i++)
                {
                    HIC_CHUK_ESTIMATE sss = ssEST.GetRowData(i) as HIC_CHUK_ESTIMATE;

                    if (sss.WRTNO > 0)
                    {
                        lstEST.Add(sss);
                    }
                }

                if (lstEST.Count > 0)
                {
                    if (!hicChukEstimateService.Save(lstEST, FnWRTNO))
                    {
                        MessageBox.Show("견적서자료 등록 중 오류가 발생하였습니다. ");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                //계산금액 저장
                long nSeqno = hicChukEstimateService.GetMaxSeqNoByWrtno(FnWRTNO);
                string strRowid = hicChukEstimateService.GetRowidEstAmtByWrtno(FnWRTNO, nSeqno);

                HIC_CHUK_ESTIMATE hCEST = new HIC_CHUK_ESTIMATE
                {
                    TOTAMT = VB.Replace(txtBillAmt.Text, ",", "").To<long>(0),
                    SEQNO = nSeqno,
                    BASEAMT = VB.Replace(txtBaseAmt.Text, ",", "").To<long>(0),
                    CHARGEAMT = VB.Replace(txtChargeAmt.Text, ",", "").To<long>(0),
                    HALINAMT = VB.Replace(txtChaAmt.Text, ",", "").To<long>(0),
                    PER = txtChaPercent.Value.To<long>(0),
                    HALINAMT1 = txtChaAmt1.Value.To<long>(0),
                    HALINAMT2 = VB.Replace(txtChaAmt2.Text, ",", "").To<long>(0),
                    AMT = VB.Replace(txtAmt.Text, ",", "").To<long>(0)
                };

                if (strRowid.IsNullOrEmpty())
                {
                    if (!hicChukEstimateService.InsertAmt(hCEST, FnWRTNO))
                    {
                        MessageBox.Show("견적서금액 등록 중 오류가 발생하였습니다. ");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
                else
                {
                    hCEST.RID = strRowid;

                    if (!hicChukEstimateService.UpDateAmt(hCEST, FnWRTNO))
                    {
                        MessageBox.Show("견적서금액 등록 중 오류가 발생하였습니다. ");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                //HIC_CHUKMST_NEW 견적정보 등록
                if (!hicChukMstNewService.UpDateEstInfo(FnWRTNO, clsType.User.IdNumber.To<long>(0)))
                {
                    MessageBox.Show("HIC_CHUKMST_NEW 견적서정보 등록 중 오류가 발생하였습니다. ");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("저장완료. ");

                Screen_Clear();

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private bool Check_Estimate_Amt()
        {
            bool rtnVal = true;
            long nAMT = 0;
            long nBaseAmt = 0;
            long nHalinAmt = 0;

            for (int i = 0; i < ssEST.ActiveSheet.RowCount; i++)
            {
                if (VB.Left(ssEST.ActiveSheet.Cells[i, 11].Text, 1) == "A")
                {
                    nBaseAmt += ssEST.ActiveSheet.Cells[i, 9].Value.To<long>(0);
                }
                else
                {
                    nAMT += ssEST.ActiveSheet.Cells[i, 9].Value.To<long>(0);
                }
            }

            nHalinAmt = VB.Replace(txtChaAmt.Text, ",", "").To<long>(0);

            if (txtBaseAmt.Text != VB.Format(nBaseAmt, "###,###,##0")) { rtnVal = false; }
            if (txtChargeAmt.Text != VB.Format(nAMT, "###,###,##0")) { rtnVal = false; }
            if (txtBillAmt.Text != VB.Format(nBaseAmt + nAMT, "###,###,##0")) { rtnVal = false; }
            if (txtAmt.Text != VB.Format(nBaseAmt + nAMT - nHalinAmt, "###,###,##0")) { rtnVal = false; }

            long nTotAmt = 0;

            for (int i = 0; i < ssEST.ActiveSheet.RowCount; i++)
            {
                nTotAmt += ssEST.ActiveSheet.Cells[i, 9].Value.To<long>();
            }

            if (txtBillAmt.Text != VB.Format(nTotAmt, "###,###,##0")) { rtnVal = false; }

            return rtnVal;
        }

        private void Display_List()
        {
            string strGbn = rdoYN1.Checked ? "N" : "Y";
            string strFDate = dtpFDate.Text;
            string strTDate = dtpTDate.Text;
            string strKeyward = txtKeyWord.Text.Trim();

            List<HIC_CHUKMST_NEW> lstHCMN = hicChukMstNewService.GetListEstimate(strGbn, strFDate, strTDate, strKeyward);

            SSList.DataSource = lstHCMN;
        }

        private void eFormload(object sender, EventArgs e)
        {
            dtpFDate.Text = DateTime.Now.AddDays(-15).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            Screen_Clear();

            Display_List();

            ssEST.AddRows(1);
        }

        private void Screen_Clear()
        {
            lblLastJobName.Text = "";
            lblLastJobTime.Text = "";

            txtBaseAmt.Text = "0";
            txtChargeAmt.Text = "0";
            txtBillAmt.Text = "0";
            txtChaPercent.Value = 0;
            txtChaAmt.Text = "0";
            txtChaAmt1.Value = 0;
            txtChaAmt2.Text = "0";
            txtAmt.Text = "0";
            cSpd.Spread_Clear_Simple(ssEST);

            ssPrt_Clear();

            FnWRTNO = 0;
        }

        private void ssPrt_Clear()
        {
            ssPrt.ActiveSheet.Cells[2, 1].Text = "";
            ssPrt.ActiveSheet.Cells[3, 1].Text = "";
            ssPrt.ActiveSheet.Cells[4, 1].Text = "";

            for (int i = 8; i < 27; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    ssPrt.ActiveSheet.Cells[i, j].Text = "";
                }
            }

            if (ssPrt.ActiveSheet.RowCount > 28)
            {
                int nRow = ssPrt.ActiveSheet.RowCount - 28;

                ssPrt.ActiveSheet.ActiveRowIndex = 8;

                for (int i = 1; i <= nRow; i++)
                {
                    ssPrt.DeleteRow();
                }
            }
        }

        private void Print_Page(FpSpread ssPrt, string strTitle = "", bool ColVis = false)
        {
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            try
            {
                if (strTitle != "")
                {
                    strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 12), clsSpread.enmSpdHAlign.Left, false, true);
                }
                else
                {
                    strHeader = cSpd.setSpdPrint_String("", null, clsSpread.enmSpdHAlign.Center, false, true);
                }

                strFooter = cSpd.setSpdPrint_String("", null, clsSpread.enmSpdHAlign.Center, false, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 40, 10, 10, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, ColVis, false, true, false, false, false, true);
                cSpd.setSpdPrint(ssPrt, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void ActionPrintAndSendEmail(string EMP_ID)
        {
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread sp = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            //string PassWord = Biz.FindJumin(EMP_ID);
            string sDirPath = @"c:\pay\pay\";
            DirectoryInfo di = new DirectoryInfo(sDirPath);
            if (di.Exists == false)
            {
                di.Create();
            }

            //setMargin = new clsSpread.SpdPrint_Margin(0, 0, 40, 40, 10, 10);
            //setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, false, false, false, false, 1.2f);
            //sp.setSpdPrintToPdf(UcPaySS1.SS1, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Both, @"c:\pay\pay\" + EMP_ID + "_근로소득원천징수영수증1(암호).pdf", PassWord);
            ////sp.setSpdPrintToPdf(SS1, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Both, @"c:\pay\pay\"+ EMP_ID + "_근로소득원천징수영수증1(암호).pdf", PassWord);
            ////sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Both);


            //setMargin = new clsSpread.SpdPrint_Margin(0, 0, 40, 40, 10, 10);
            //setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, false, false, false, false, 1.2f);
            //sp.setSpdPrintToPdf(UcPaySS2.SS2, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Both, @"c:\pay\pay\" + EMP_ID + "_근로소득원천징수영수증2(암호).pdf", PassWord);
            ////sp.setSpdPrintToPdf(SS2, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Both, @"c:\pay\pay\" + EMP_ID + "_근로소득원천징수영수증2(암호).pdf", PassWord);
            ////sp.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Both);


            //setMargin = new clsSpread.SpdPrint_Margin(10, 0, 40, 40, 10, 10);
            //setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, false, false, false, false, 1.1f);
            //sp.setSpdPrintToPdf(UcPaySS3.SS3, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal, @"c:\pay\pay\" + EMP_ID + "_근로소득원천징수영수증3(암호).pdf", PassWord);
            ////sp.setSpdPrintToPdf(SS3, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal, @"c:\pay\pay\" + EMP_ID + "_근로소득원천징수영수증3(암호).pdf", PassWord);
            ////sp.setSpdPrint(SS3, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);

            for (int i = 0; i < 1000; i++)
            {
                Application.DoEvents();
                Application.DoEvents();
                Application.DoEvents();
                Application.DoEvents();
            }
        }

        private void DoSendEmail(string EMP_ID)
        {
            clsMail clsMail = new clsMail();
            // 서버주소
            clsMail.smtpServer = "smtp.mailplug.co.kr";
            // 서버포트
            clsMail.smtpPort = 465;
            // 사용자 아이디
            clsMail.smtpUser = "psmh@pohangsmh.co.kr";
            // 사용자 패스워드
            clsMail.smtpPW = "vhgkdtjdahquddnjs*";
            // 보낸사람 주소
            clsMail.FromEmailAddr = "psmh@pohangsmh.co.kr";
            // 받는사람 주소
            //clsMail.ToEmailAddr = Biz.FindEmail(EMP_ID);
            //if (!TxtEmailAddr.Text.Trim().IsNullOrEmpty())
            //{
            //    clsMail.ToEmailAddr = TxtEmailAddr.Text.Trim();
            //}
            // 제목
            clsMail.Subject = "근로소득원청징수 영수증";
            // 내용
            clsMail.Body = "근로소득원청징수 영수증 입니다 \r\n\r\n암호는 생년월일입니다.";
            // 첨부파일
            string strAttach1 = @"c:\pay\pay\" + EMP_ID + "_근로소득원천징수영수증1(암호).pdf";
            clsMail.Attachment.Add(strAttach1);
            string strAttach2 = @"c:\pay\pay\" + EMP_ID + "_근로소득원천징수영수증2(암호).pdf";
            clsMail.Attachment.Add(strAttach2);
            string strAttach3 = @"c:\pay\pay\" + EMP_ID + "_근로소득원천징수영수증3(암호).pdf";
            clsMail.Attachment.Add(strAttach3);

            if (clsMail.ToEmailAddr == "")
            {
                MessageUtil.Alert("받는사람 메일주소가 존재하지 않습니다.");
                return;
            }

            clsMail.SendMail();
        }
    }
}
