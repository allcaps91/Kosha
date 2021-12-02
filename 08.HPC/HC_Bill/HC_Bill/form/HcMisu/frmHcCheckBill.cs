using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Bill
/// File Name       : frmHcCheckBill.cs
/// Description     : 계산서조회
/// Author          : 심명섭
/// Create Date     : 2021-06-30
/// Update History  : 
/// </summary>
/// <seealso cref= "hcmisu > Frm게산서조회 (Frm계산서조회.frm)" />
/// 

namespace HC_Bill
{
    public partial class frmHcCheckBill :BaseForm
    {
        clsSpread cSpd = null;
        clsPublic cpublic = null;
        ComFunc cF = null;
        MisuTaxService misuTaxService = null;
        AccTaxService accTaxService = null;
        AccCloMgtService accCloMgtService = null;
        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;
        HicLtdService hicLtdService = null;

        int result;

        string strGbn;
        string strJong;
        string strOldName;
        string strNewName;
        string strTaxNo;
        string strLTDNAME;
        string strLTD;
        string strLTDCODE2;

        double nGAmt;
        double nVat;
        double nToGamt;
        double nToVat;
        double nChaAmt;

        string strBDate;
        string strLTDNAME2;
        string strLTDNO2;
        string strUPTAE2;
        string strJONGMOK2;
        string strWRTNO;
        string strBuDate;
        string strCLOYN;
        string strCLOYMD;
        string strYEAR;

        public frmHcCheckBill()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            cpublic = new clsPublic();
            cF = new ComFunc();
            misuTaxService = new MisuTaxService();
            accTaxService = new AccTaxService();
            accCloMgtService = new AccCloMgtService();
            FrmHcLtdHelp = new frmHcLtdHelp();
            hicLtdService = new HicLtdService();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            int Col = e.Column;
            int Row = e.Row;
            if(Col != 7) { return; }

            clsPublic.GstrRetValue = "";

            for(int i = 0; i < 1; i++)
            {
                LtdHelp(Row);
            }
        }

        private void LtdHelp(int Row)
        {
            string strLtdCode = "";
            
            FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
            FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
            FrmHcLtdHelp.ShowDialog();
            FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

            if (!LtdHelpItem.IsNullOrEmpty())
            {
                SS1.ActiveSheet.Cells[Row, 7].Text = LtdHelpItem.SAUPNO.Replace("-", "");
                SS1.ActiveSheet.Cells[Row, 8].Text = LtdHelpItem.NAME;
                SS1.ActiveSheet.Cells[Row, 9].Text = VB.Format(LtdHelpItem.CODE, "00000");
                SS1.ActiveSheet.Cells[Row, 10].Text = LtdHelpItem.UPTAE;
                SS1.ActiveSheet.Cells[Row, 11].Text = LtdHelpItem.JONGMOK;
            }
            else
            {
                SS1.ActiveSheet.Cells[Row, 7].Text =  "";
                SS1.ActiveSheet.Cells[Row, 8].Text =  "";
                SS1.ActiveSheet.Cells[Row, 9].Text =  "";
                SS1.ActiveSheet.Cells[Row, 10].Text = "";
                SS1.ActiveSheet.Cells[Row, 11].Text = "";
            }
        }

        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
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
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return;
                }
                Spread_Print();
            }
            else if (sender == btnSearch)
            {
                DisPlay_Screen();
            }
            else if (sender == btnSave)
            {
                DataSave();
            }
            else if (sender == btnCancel)
            {
                Cancel();
            }
        }

        private void DataSave()
        {
            // 계산서 마감일자 select함.
            List<ACC_TAX> list = accTaxService.GetTaxDate();
            strBuDate = list[0].TAXDATE;

            for(int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                strBDate = SS1.ActiveSheet.Cells[i, 0].Text;
                strLTDNO2 = SS1.ActiveSheet.Cells[i, 7].Text;
                strLTDNAME2 = SS1.ActiveSheet.Cells[i, 8].Text;
                strLTDCODE2 = SS1.ActiveSheet.Cells[i, 9].Text;
                strUPTAE2 = SS1.ActiveSheet.Cells[i, 10].Text;
                strJONGMOK2 = SS1.ActiveSheet.Cells[i, 11].Text;
                strWRTNO = SS1.ActiveSheet.Cells[i, 12].Text;

                strCLOYN = "";
                strCLOYMD = strBDate.Replace("-", "");
                // 계산서 마감일자 select함. (c#)
                List<ACC_CLO_MGT> item = accCloMgtService.GetMagamDay(strCLOYMD);

                strCLOYN = item[0].MM_CLO_YN;

                if(strCLOYN == "Y")
                {
                    MessageBox.Show(strBDate + "일자는 재무회계팀 마감된 날짜입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                strYEAR = VB.Mid(strBDate, 3, 2);

                if(strBDate != "" && strWRTNO != "")
                {
                    try
                    {
                        clsDB.setBeginTran(clsDB.DbCon);

                        if (!UpdateMisuTax())
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        MessageBox.Show("저장완료.", "확인", MessageBoxButtons.OK, MessageBoxIcon.None);

                        clsDB.setCommitTran(clsDB.DbCon);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
            }
        }

        private bool UpdateMisuTax()
        {
            MISU_TAX item = new MISU_TAX
            {
                LTDNO2 =           strLTDNO2,
                LTDNAME2 =         strLTDNAME2,
                LTDCODE2 =         strLTDCODE2,
                UPTAE2 =           strUPTAE2,
                JONGMOK2 =         strJONGMOK2,
                WRTNO =            strWRTNO,
                GJYEAR =           strYEAR
            };

            result = misuTaxService.UpdateMisuTax(item);

            if (result < 0)
            {
                MessageBox.Show("계산서 업데이트 쿼리오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void DisPlay_Screen()
        {
            // 세금계산서 매출/매입(매입 1, 매출 2)
            strGbn = "2";
            strTaxNo = TxtTaxNo.Text.Trim();
            strJong = VB.Left(cboMJong.Text, 1);

            SS1.ActiveSheet.RowCount = 0;
            nToGamt = 0;
            nToVat = 0;
            nGAmt = 0;
            nVat = 0;

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                
                if (!CreateViewTax())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                List<MISU_TAX> item = misuTaxService.GetViewItem(rdoSort1.Checked, rdoSort2.Checked);

                strOldName = "";
                SS1.ActiveSheet.RowCount = item.Count();
                for(int i = 0; i < item.Count; i++)
                {
                    nGAmt = item[i].GAMT;
                    SS1.ActiveSheet.Cells[i, 0].Text = item[i].BDATE.Trim();
                    
                    if(READ_TRB_SALEEBILL(item[i].TRBNO.Trim()) == "OK")
                    {
                        SS1.ActiveSheet.Cells[i, 0].BackColor = Color.LightPink;
                    }

                    strLTDCODE2 = item[i].LTDCODE2;
                    
                    if (!item[i].LTD.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 1].Text = item[i].LTD.Trim();          // 사업자등록번호
                    }

                    if (!item[i].NAME.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 2].Text = item[i].NAME.Trim();         // 상호
                    }

                    if (!item[i].UPTAE.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 3].Text = item[i].UPTAE.Trim();        // 업태
                    }

                    if (!item[i].JONGMOK.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 4].Text = item[i].JONGMOK.Trim();      // 종목
                    }

                    SS1.ActiveSheet.Cells[i, 5].Text = nGAmt.To<string>("");            // 공급가액
                    
                    if (!item[i].GBTRB.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 6].Text = item[i].GBTRB.Trim();        // 전자계산서
                    }

                    if (!item[i].LTD2.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 7].Text = item[i].LTD2.Trim();         // 전자계산서
                    }

                    if (!item[i].NAME2.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 8].Text = item[i].NAME2.Trim();        // 전자계산서
                    }

                    if (!item[i].LTDCODE2.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 9].Text = item[i].LTDCODE2.Trim();     // 전자계산서
                    }

                    if (!item[i].UPTAE2.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 10].Text = item[i].UPTAE2.Trim();      // 전자계산서
                    }

                    if (!item[i].JONGMOK2.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 11].Text = item[i].JONGMOK2.Trim();    // 전자계산서
                    }

                    if (!item[i].WRTNO.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 12].Text = item[i].WRTNO.Trim();       // 전자계산서
                    }
                    

                    nToGamt += nGAmt;
                }
                SS1.ActiveSheet.RowCount += 1;

                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 4].Text = "합   계";
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 5].Text = nToGamt.ToString();


                if (!DropView())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

        }

        private bool DropView()
        {
            misuTaxService.DropView();
            
            return true;
        }

        private string READ_TRB_SALEEBILL(string ArgCode)
        {
            string Val = "";
            //TrusBill 전송여부 확인

            if(ArgCode.To<int>(0) == 0)
            {
                return Val;
            }

            result = misuTaxService.GetSALEEBILL(ArgCode.To<int>(0));

            if(result < 0)
            {
                return Val;
            }
            else
            {
                Val = "OK";
                return Val;
            }
        }

        private bool CreateViewTax()
        {
            result = misuTaxService.CreateViewBill(dtpFDate.Text, dtpTDate.Text, strTaxNo, strJong);

            return true;
        }

        private void Cancel()
        {
            SS1.ActiveSheet.RowCount = 0;
            SS1.ActiveSheet.RowCount = 20;
        }

        private void Spread_Print()
        {
            string strTitle = "";
            string strSign = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "건강증진센타 계산서 내역";
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = cSpd.setSpdPrint_String(strSign, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("미수종류:" + cboMJong.Text + VB.Space(10) + "인쇄일자 : " + clsPublic.GstrSysDate + VB.Space(10) +"PAGE :" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);


            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 40, 40);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, false, false, false, true, 1f);
            cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            SS1.ActiveSheet.Columns.Get(12).Visible = false;

            read_sysdate();
            dtpFDate.Text = cF.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, -20);
            dtpTDate.Text = clsPublic.GstrSysDate;
            TxtTaxNo.Text = "";

            // 미수종류 SET
            cboMJong.Clear();
            cboMJong.Items.Add("*.전체미수");
            cboMJong.Items.Add("1.건강검진");
            cboMJong.Items.Add("2.보건대행");
            cboMJong.Items.Add("3.작업측정");
            cboMJong.Items.Add("4.종합검진");
            cboMJong.Items.Add("5.개인미수");
            cboMJong.Items.Add("6.국고미수");
            cboMJong.SelectedIndex = 0;

            btnSave.Visible = false;

            if(clsPublic.GnJobSabun == 13850 || clsPublic.GnJobSabun == 16341 || clsPublic.GnJobSabun == 36540)
            {
                btnSave.Visible = true;
            }
        }

        void read_sysdate()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }
    }
}
