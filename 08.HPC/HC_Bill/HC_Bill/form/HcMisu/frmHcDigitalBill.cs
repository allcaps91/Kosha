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
/// File Name       : frmHcDigitalBill.cs
/// Description     : 전자계산서 발급
/// Author          : 심명섭
/// Create Date     : 2021-07-01
/// Update History  : 
/// </summary>
/// <seealso cref= "hcmisu > Frm전자계산서발급 (Frm전자계산서발급.frm)" />
/// 

namespace HC_Bill
{
    public partial class frmHcDigitalBill :BaseForm
    {
        clsSpread cSpd = null;
        clsPublic cpublic = null;
        clsHaBase cHB = null;
        ComFunc CF = null;
        HicBogenltdService hicBogenltdService = null;
        HicMisuMstLtdService hicMisuMstLtdService = null;
        HicLtdTaxService hicLtdTaxService = null;
        AccTaxService accTaxService = null;
        AccCloMgtService accCloMgtService = null;
        HicBcodeService hicBcodeService = null;
        HicMisuMstSlipService hicMisuMstSlipService = null;
        HicLtdService hicLtdService = null;
        ComHpcLibBService comHpcLibBService = null;
        MisuTaxService misuTaxService = null;
        clsHcMisu cHM = null;
        clsUser cU = null;

        int result;
        int nRow;
        long nYY;
        long nMM;
        long FnMisuAmt;
        long FnIpgumAmt;
        long FnGamAmt;
        long FnSakAmt;
        long FnBanAmt;
        long FnJanAmt;
        string FstrSangHo;

        // search
        string strMJong;
        string strFDate;
        string strTDate;

        // Save
        long nCnt;
        long nWrtno;
        long nTaxWRTNO;     // 신규 세금계산서번호
        double nAmt;
        string strData;
        string strCheck;
        string strMirNo;
        string strGjyear;
        string strLtdNo;
        string strPumMok;
        string strBuDate;
        string strJong;
        string strltdcode;
        string strLTDCODE2;
        long nTRBNO;
        string strDLtd;
        string strSangHo2;
        string strSaupNo;
        string strSaupNo2;
        string strCLOYN;
        string strCLOYMD;
        string strBDate;

        public frmHcDigitalBill()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            cpublic = new clsPublic();
            cHB = new clsHaBase();
            CF = new ComFunc();
            cHM = new clsHcMisu();
            hicMisuMstLtdService = new HicMisuMstLtdService();
            hicLtdTaxService = new HicLtdTaxService();
            accTaxService = new AccTaxService();
            accCloMgtService = new AccCloMgtService();
            hicBcodeService = new HicBcodeService();
            hicMisuMstSlipService = new HicMisuMstSlipService();
            hicLtdService = new HicLtdService();
            hicBogenltdService = new HicBogenltdService();
            comHpcLibBService = new ComHpcLibBService();
            misuTaxService = new MisuTaxService();
            cU = new clsUser();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLtd.Click += new EventHandler(eBtnClick);
            this.btnAll.Click += new EventHandler(eBtnClick);
            this.btnNo.Click += new EventHandler(eBtnClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (SSList.ActiveSheet.RowCount < 0)
            {
                return;
            }
            nWrtno = SSList.ActiveSheet.Cells[e.Row, 2].Text.To<long>(0);
            strJong = SSList.ActiveSheet.Cells[e.Row, 3].Text;
            strltdcode = SSList.ActiveSheet.Cells[e.Row, 5].Text;

            Screen_Display(nWrtno, strJong, strltdcode);
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
                DisPlay_Screen();
            }
            else if (sender == btnAll)
            {
                AllChoice();
            }
            else if (sender == btnNo)
            {
                AllCancel();
            }
            else if (sender == btnPrint)
            {
                Issuance();
            }

        }

        private void Issuance()
        {
            try
            {
                if (TxtBalDate.Text == "")
                {
                    MessageBox.Show("발급일이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                strGjyear = VB.Mid(TxtBalDate.Text, 3, 2).Trim();

                // 계산서 마감일지 select함

                List<ACC_TAX> list = accTaxService.GetTaxDate();
                strBuDate = list[0].TAXDATE;

                if (string.Compare(TxtBalDate.Text, strBuDate) < 0)
                {
                    MessageBox.Show(strBuDate + "이전일자는 수정불가 경리과 전화 요망.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strBDate = TxtBalDate.Text;

                //2020-12-30 추가
                strCLOYN = "";
                strCLOYMD = strBDate.Replace("-", "");
                // 계산서 마감일자 select함. (c#)
                List<ACC_CLO_MGT> item = accCloMgtService.GetMagamDay(strCLOYMD);
                strCLOYN = item[0].MM_CLO_YN;

                if (strCLOYN == "Y")
                {
                    MessageBox.Show(strBDate + "일자는 재무회계팀 마감된 날짜입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //GnJobSabun = 16341
                //2017-05-17 전자계산서 발급책임자
                string Sabun = clsType.User.IdNumber;
                List<HIC_BCODE> Rowid = hicBcodeService.GetRowid(Sabun);

                if (Rowid.Count == 0)
                {
                    MessageBox.Show("전자계산서 발급책임자만 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                switch (VB.Left(cboChungu.Text, 1))
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                        break;
                    default:
                        MessageBox.Show("영구/청구 구분이 오류입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }

                // 자료에 오류가 있는지 점검
                nCnt = 0;
                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (SSList.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nAmt = SSList.ActiveSheet.Cells[i, 4].Text.To<double>(0);
                        if (nAmt == 0) { MessageBox.Show(i + "번줄 금액이 0원입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                        strData = SSList.ActiveSheet.Cells[i, 6].Text;
                        if (strData == "") { MessageBox.Show(i + "번줄 담당자명이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                        strData = SSList.ActiveSheet.Cells[i, 7].Text;
                        if (strData == "") { MessageBox.Show(i + "번줄 회사전화가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                        strData = SSList.ActiveSheet.Cells[i, 8].Text;
                        if (strData == "") { MessageBox.Show(i + "번줄 이메일이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                        strData = SSList.ActiveSheet.Cells[i, 9].Text;
                        if (strData == "") { MessageBox.Show(i + "번줄 휴대폰이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                        strData = SSList.ActiveSheet.Cells[i, 10].Text;
                        if (strData == "") { MessageBox.Show(i + "번줄 사업자등록번호가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                        strData = SSList.ActiveSheet.Cells[i, 11].Text;
                        if (strData == "") { MessageBox.Show(i + "번줄 상호가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                        strData = SSList.ActiveSheet.Cells[i, 12].Text;
                        if (strData == "") { MessageBox.Show(i + "번줄 대표자명이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                        strData = SSList.ActiveSheet.Cells[i, 13].Text;
                        if (strData == "") { MessageBox.Show(i + "번줄 업태가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                        strData = SSList.ActiveSheet.Cells[i, 14].Text;
                        if (strData == "") { MessageBox.Show(i + "번줄 종목이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                        strData = SSList.ActiveSheet.Cells[i, 15].Text;
                        if (strData == "") { MessageBox.Show(i + "번줄 주소가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                        nCnt += 1;
                    }
                }

                if (nCnt == 0)
                {
                    MessageBox.Show("발급할자료를 1건도 선택을 안함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 발급할 자료를 선택하여 1건씩 발급함
                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    nRow = i;
                    if (SSList.ActiveSheet.Cells[i, 0].Text == "" || SSList.ActiveSheet.Cells[i, 0].Text == "False")
                    {
                        strCheck = "False";
                    }
                    else if (SSList.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        strCheck = "True";
                    }
                    nWrtno = SSList.ActiveSheet.Cells[i, 2].Text.To<long>(0);
                    strJong = SSList.ActiveSheet.Cells[i, 3].Text;
                    strltdcode = SSList.ActiveSheet.Cells[i, 5].Text;
                    strLTDCODE2 = SSList.ActiveSheet.Cells[i, 18].Text;
                    if (strCheck == "True")
                    {
                        Print_TAX_OK();
                    }
                }

                DisPlay_Screen();
                SSList.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private void Print_TAX_OK()
        {
            Screen_Display(nWrtno, strJong, strltdcode);

            if (TxtLtdCode.Text.Trim() == "0176")
            {
                strMJong = "5";
            }
            else
            {
                switch (VB.Left(cboJONG.Text, 2))
                {
                    case "81":
                        strMJong = "3";
                        break;
                    case "82":
                        if (VB.Left(cboMJong.Text.Trim(), 1) == "6")
                        {
                            strMJong = "6";
                        }
                        else
                        {
                            strMJong = "2";
                        }
                        break;
                    case "83":
                        strMJong = "4";
                        break;
                    default:
                        strMJong = "1";
                        break;
                }
            }

            strMirNo = TxtMirNo.Text;
            strLtdNo = VB.Left(txtLtdNo.Text, 3) + VB.Mid(txtLtdNo.Text, 5, 2) + VB.Right(txtLtdNo.Text, 5);
            if (strLtdNo == "")
            {
                MessageBox.Show("세금계산서 발급시 사업자등록번호가 없습니다!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 신규 세금계산서 번호를 발생함
            List<MISU_TAX> TaxWrtno = misuTaxService.GetWrtno(strGjyear);
            if (TaxWrtno.Count > 0)
            {
                nTaxWRTNO = VB.Format(TaxWrtno[0].TAXWRTNO, "0000").To<long>(0) + 1;
            }
            if (nTaxWRTNO == 0)
            {
                MessageBox.Show("세금계산서 신규번호를 부여시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<COMHPC> Bill = comHpcLibBService.GetBillNo();

            strPumMok = TxtPumMok.Text;
            nAmt = TxtMisuAmt.Text.Replace(",", "").To<long>(0);
            nTRBNO = Bill[0].SEQNO;

            // 미수는 원래 거래처(회사)로 미수 발생함
            nWrtno = SSList.ActiveSheet.Cells[nRow, 2].Text.To<long>(0);
            strJong = SSList.ActiveSheet.Cells[nRow, 3].Text;
            strltdcode = SSList.ActiveSheet.Cells[nRow, 18].Text;
            Screen_Display(nWrtno, strJong, strltdcode);

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                // 세금계산서 발급 내역을 INSERT
                if (!initBill())
                {
                    return;
                }

                // 미수내역에 세금계산서 발급 완료 UPDATE
                if (!UpdateBill())
                {
                    return;
                }

                nWrtno = SSList.ActiveSheet.Cells[nRow, 2].Text.To<long>(0);
                strJong = SSList.ActiveSheet.Cells[nRow, 3].Text;
                strltdcode = SSList.ActiveSheet.Cells[nRow, 5].Text;

                Screen_Display(nWrtno, strJong, strltdcode);

                //---------------------------------------------
                // 전자세금계산서 전송 요청 테이블에INSERT
                //---------------------------------------------
                if (!SendBill())
                {
                    return;
                }

                if (!InsertBill())
                {
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

        private bool InsertBill()
        {
            COMHPC item = new COMHPC
            {
                SEQID = VB.Format(nTRBNO, "#0"),
                DETAILSEQID = VB.Format(nTRBNO, "#0"),
                ITEMDATE = TxtBalDate.Text.Replace("-",""),
                ITEMNAME = TxtPumMok.Text,
                ITEMTYPE = txtStandard.Text,
                ITEMQTY = 1,
                ITEMPRICE = VB.Format(nAmt, "#0").To<long>(0),
                ITEMSUPPRICE = VB.Format(nAmt, "#0").To<long>(0),
                ITEMTAX = 0,
                ITEMREMARKS = ""
            };

            result = comHpcLibBService.InsertBill(item);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("TRB_SALEEBill에 신규등록 시 오류가 발생함!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool SendBill()
        {
            COMHPC item = new COMHPC
            {
                RESSEQ = VB.Format(nTRBNO, "#0"),
                DOCTYPE = "",
                DOCCODE = "02",
                EBILLKIND = 1,
                CUSTOMS = "N",
                TAXSNUM = "",
                DOCATTR = "N",
                PUBDATE = TxtBalDate.Text.Replace("-",""),
                SYSTEMCODE = "KF",
                PUBTYPE = "S",
                PUBFORM = "R",
                PUBFORM2 = "D",
                MEMID = "kimhh123",
                MEMDEPTNAME = "일반건진",
                MEMDEPTNAME2 = "보건대행",
                MEMDEPTNAME3 = "작업환경측정",
                MEMDEPTNAME4 = "종합건진",
                //MEMNAME = clsPublic.GstrJobName,
                MEMNAME = cU.GstrJobName,
                EMAIL = "health@pohangsmh.co.kr",
                TEL = "054-260-8185",
                TEL2 = "054-260-8193",
                TEL3 = "054-260-8194",
                COREGNO = "5068200896",
                COTAXREGNO = "",
                CONAME = "(재)포항성모병원",
                COCEO = "이종녀",
                COADDR = "경북 포항시 남구 대잠동길 17",
                COBIZTYPE = "의료.보건업",
                COBIZSUB = "병원",
                RECMEMID = "",
                RECMEMDEPTNAME = "",
                RECMEMNAME = TxtDamName2.Text,
                RECEMAIL = TxtEMail.Text,
                RECTEL = TxtTel.Text,
                RECCOREGNOTYPE = "01",
                RECCOREGNO = txtLtdNo.Text.Replace("-",""),
                RECCOTAXREGNO = TxtJSaupNo.Text,
                RECCONAME = PanelLtdName.Text,
                RECCOCEO = txtDname.Text,
                RECCOADDR = txtJuso.Text,
                RECCOBIZTYPE = txtUptae.Text,
                RECCOBIZSUB = txtJongMok.Text,
                SUPPRICE = VB.Format(nAmt, "#0").To<long>(0),
                TAX = 0,
                CASH = VB.Format(nAmt, "#0").To<long>(0),
                CHEQUE = VB.Format(nAmt, "#0").To<long>(0),
                BILL = VB.Format(nAmt, "#0").To<long>(0),
                OUTSTAND = VB.Format(nAmt, "#0").To<long>(0),
                PUBKIND = "N",
                SMS = TxtHphone.Text.Replace("-",""),
                REMARKS = ComFunc.QuotConv(SSList.ActiveSheet.Cells[nRow, 16].Text)
            };

            result = comHpcLibBService.SendBill(item, VB.Left(cboChungu.Text, 1), VB.Left(cboMJong.Text, 1));

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("TRB_SALEEBill에 신규등록 시 오류가 발생함!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool UpdateBill()
        {
            result = hicMisuMstLtdService.UpdateBill(nWrtno);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("미수마스타에 지로발행 완료 UPDATE시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool initBill()
        {
            MISU_TAX initItem = new MISU_TAX
            {
                GJYEAR = strGjyear,
                WRTNO = VB.Format(nTaxWRTNO, "0000"),
                BDATE = TxtBalDate.Text,
                GBBUSE = "1",
                MJONG = strMJong,
                LTDCODE = strLTDCODE2,
                LTDNAME = PanelLtdName.Text,
                LTDNO = txtLtdNo.Text.Replace("-","").Trim(),
                DAEPYONAME = txtDname.Text,
                LTDJUSO = txtJuso.Text,
                UPTAE = txtUptae.Text,
                JONGMOK = txtJongMok.Text,
                MISUNO = nWrtno,
                MIRNO = strMirNo,
                AMT = nAmt.To<long>(0),
                ENTSABUN = clsType.User.IdNumber.To<long>(0),
                REMARK = TxtRemark.Text,
                PUMMOK = strPumMok,
                GBTRB = "Y",
                TRBNO = nTRBNO,
                TRBNO2 = 0,
            };

            result = misuTaxService.initBill(initItem);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("세금계산서 발행내역을 등록시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void Screen_Display(long nWrtno, string strJong, string strltdcode)
        {
            if (!View_MST_Display())
            {
                return;
            }
            if (!View_SLIP_Display())
            {
                return;
            }
            if (!View_BOGENLTD_Display())
            {
                return;
            }

        }

        private bool View_BOGENLTD_Display()
        {
            try
            {
                List<HIC_BOGENLTD> list = hicBogenltdService.GetDaeHang(strltdcode.To<long>(0));

                if (list.Count > 0)
                {
                    if (list[0].BILL == "1" && strJong == "82") // 계산서 인원&단가 표시여부가 선택되어 있고 미수종류가 보건대행(82) 인거
                    {
                        txtNum.Text = list[0].INWON.ToString();
                        txtPrice.Text = list[0].PRICE.ToString();
                    }
                    else
                    {
                        txtNum.Text = "";
                        txtPrice.Text = "";
                    }
                }
                else
                {
                    txtNum.Text = "";
                    txtPrice.Text = "";
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }
        }

        private bool View_SLIP_Display()
        {
            try
            {
                List<HIC_MISU_MST_SLIP> list = hicMisuMstSlipService.GetMisuSlipDisplay(nWrtno);

                SS2.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    SS2.ActiveSheet.Cells[i, 0].Text = list[i].BDATE;
                    SS2.ActiveSheet.Cells[i, 1].Text = list[i].GEACODE;
                    SS2.ActiveSheet.Cells[i, 2].Text = cHM.READ_MisuGea_Name(list[i].GEACODE.Trim());
                    SS2.ActiveSheet.Cells[i, 3].Text = list[i].SLIPAMT.ToString();
                    SS2.ActiveSheet.Cells[i, 4].Text = list[i].REMARK;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }
        }

        private bool View_MST_Display()
        {
            try
            {
                List<HIC_MISU_MST_SLIP> list = hicMisuMstSlipService.GetMisuMaster2(nWrtno);

                if (list.Count == 0)
                {
                    return false;
                }

                TxtLtdCode.Text = strltdcode;

                // 사업장정보 Display
                List<HIC_LTD> item = hicLtdService.GetBusinessItem(TxtLtdCode.Text);

                txtDname.Text = item[0].DAEPYO;
                txtUptae.Text = item[0].UPTAE;
                txtJongMok.Text = item[0].JONGMOK;
                txtLtdNo.Text = item[0].SAUPNO;
                txtJuso.Text = item[0].JUSO + " " + item[0].JUSODETAIL;
                TxtJSaupNo.Text = item[0].JSAUPNO;
                // 2016-08-24 계산서용 주소 별도 관리
                if (item[0].TAX_JUSO == null)
                {
                    item[0].TAX_JUSO = "";
                }
                if (item[0].TAX_JUSO.Trim() != "")
                {
                    txtJuso.Text = item[0].TAX_JUSO + " " + item[0].TAX_JUSODETAIL;
                }
                FstrSangHo = item[0].SANGHO.Trim();
                PanelLtdName.Text = FstrSangHo;

                // 종류를 Display
                for (int i = 0; i < cboJONG.Items.Count; i++)
                {
                    cboJONG.SelectedIndex = i;
                    if (VB.Left(cboJONG.Text, 2) == list[0].GJONG)
                    {
                        cboJONG.SelectedIndex = i;
                        break;
                    }
                }

                // 미수구분
                for (int i = 0; i < cboGbn.Items.Count; i++)
                {
                    cboGbn.SelectedIndex = i;
                    if (VB.Left(cboGbn.Text, 1) == list[0].MISUGBN)
                    {
                        cboGbn.SelectedIndex = i;
                        break;
                    }
                }

                TxtDate.Text = list[0].BDATE;
                TxtMisuAmt.Text = VB.Format(list[0].MISUAMT, "###,###,###,##0");
                TxtMirNo.Text = list[0].GIRONO;
                TxtDamName.Text = list[0].DAMNAME;
                TxtRemark.Text = list[0].REMARK;
                TxtPumMok.Text = list[0].PUMMOK;

                // 미수종류
                switch (list[0].MISUJONG.Trim())
                {
                    case "1":
                        PanelMisuJong.Text = "회사미수";
                        break;
                    case "2":
                        PanelMisuJong.Text = "건강보험";
                        break;
                    case "3":
                        PanelMisuJong.Text = "국고미수";
                        break;
                    case "4":
                        PanelMisuJong.Text = "개인미수";
                        break;
                    default:
                        PanelMisuJong.Text = "";
                        break;
                }

                // 세금계산서 담당자 표시
                List<HIC_LTD_TAX> Category= hicLtdTaxService.GetDamDangJa(TxtLtdCode.Text, list[0].GJONG);

                if (Category.Count > 0)
                {
                    TxtDamName2.Text = Category[0].DAMNAME;
                    TxtTel.Text = Category[0].TEL;
                    TxtEMail.Text = Category[0].EMAIL;
                    TxtHphone.Text = Category[0].HPHONE;
                }

                // 금액을 Display
                SS1.ActiveSheet.Cells[0, 0].Text = VB.Format(list[0].MISUAMT, "###,###,###,##0");
                SS1.ActiveSheet.Cells[0, 1].Text = VB.Format(list[0].IPGUMAMT, "###,###,###,##0");
                SS1.ActiveSheet.Cells[0, 2].Text = VB.Format(list[0].GAMAMT, "###,###,###,##0");
                SS1.ActiveSheet.Cells[0, 3].Text = VB.Format(list[0].SAKAMT, "###,###,###,##0");
                SS1.ActiveSheet.Cells[0, 4].Text = VB.Format(list[0].BANAMT, "###,###,###,##0");
                SS1.ActiveSheet.Cells[0, 5].Text = VB.Format(list[0].JANAMT, "###,###,###,##0");

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void AllCancel()
        {
            for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
            {
                SSList.ActiveSheet.Cells[i, 0].Text = "False";
            }
        }

        private void AllChoice()
        {
            for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
            {
                SSList.ActiveSheet.Cells[i, 0].Text = "True";
            }
        }

        private void DisPlay_Screen()
        {
            strFDate = VB.Left(cboYYMM.Text, 4) + "-" + VB.Right(cboYYMM.Text, 2) + "-01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            strMJong = VB.Left(cboMJong.Text, 1);

            // Sheet를 Clear
            SSList.ActiveSheet.RowCount = 30;
            SS_Clear(SSList_Sheet1);

            Screen_Clear();

            // 자료를 SELECT
            List<HIC_MISU_MST_LTD> list = hicMisuMstLtdService.GetBillList(strFDate, strTDate, strMJong);

            SSList.ActiveSheet.RowCount = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].SANGHO.IsNullOrEmpty())
                {
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].SANGHO.Trim();
                }

                if (!list[i].WRTNO.IsNullOrEmpty())
                {
                    SSList.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.To<string>("");
                }

                if (!list[i].GJONG.IsNullOrEmpty())
                {
                    SSList.ActiveSheet.Cells[i, 3].Text = list[i].GJONG.Trim();
                }

                if (!list[i].MISUAMT.IsNullOrEmpty())
                {
                    SSList.ActiveSheet.Cells[i, 4].Text = VB.Format(list[i].MISUAMT, "###,###,###,##0");
                }

                if (!list[i].LTDCODE.IsNullOrEmpty())
                {
                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].LTDCODE.To<string>("");
                }

                if (!list[i].LTDCODE.IsNullOrEmpty())
                {
                    SSList.ActiveSheet.Cells[i, 18].Text = list[i].LTDCODE.To<string>("").Trim();
                }

                SSList.ActiveSheet.Cells[i, 6].Text = "";
                SSList.ActiveSheet.Cells[i, 7].Text = "";
                SSList.ActiveSheet.Cells[i, 8].Text = "";
                SSList.ActiveSheet.Cells[i, 9].Text = "";

                // 아래주석을 풀지말것
                // 대행회사 사업장이면
                if (list[i].DLTD > 0)
                {
                    List<HIC_MISU_MST_LTD> DLTDList = hicMisuMstLtdService.GetActingItem(list[i].DLTD);

                    if (!DLTDList[0].SAUPNO.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 10].Text = DLTDList[0].SAUPNO.Trim();
                    }
                    if (!DLTDList[0].SANGHO.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 11].Text = DLTDList[0].SANGHO.Trim();
                    }
                    if (!DLTDList[0].DAEPYO.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 12].Text = DLTDList[0].DAEPYO.Trim();
                    }
                    if (!DLTDList[0].UPTAE.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 13].Text = DLTDList[0].UPTAE.Trim();
                    }
                    if (!DLTDList[0].JONGMOK.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 14].Text = DLTDList[0].JONGMOK.Trim();
                    }
                    if (!DLTDList[0].JUSO.IsNullOrEmpty() || !DLTDList[0].JUSODETAIL.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 15].Text = (DLTDList[0].JUSO + " " + DLTDList[0].JUSODETAIL).Trim();
                    }

                    SSList.ActiveSheet.Cells[i, 16].Text = "";  //참고사항 입력칸

                    if (!list[i].JSAUPNO.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 17].Text = list[i].JSAUPNO.Trim();
                    }

                    if (!list[i].DLTD.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 17].Text = VB.Format(list[i].DLTD, "#0");
                    }
                }
                else
                {
                    if (!list[i].SAUPNO.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 10].Text = list[i].SAUPNO.Trim();
                    }
                    if (!list[i].SANGHO.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 11].Text = list[i].SANGHO.Trim();
                    }
                    if (!list[i].DAEPYO.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 12].Text = list[i].DAEPYO.Trim();
                    }
                    if (!list[i].UPTAE.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 13].Text = list[i].UPTAE.Trim();
                    }
                    if (!list[i].JONGMOK.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 14].Text = list[i].JONGMOK.Trim();
                    }
                    if (!list[i].JUSO.IsNullOrEmpty() || !list[i].JUSODETAIL.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 15].Text = (list[i].JUSO + " " + list[i].JUSODETAIL).Trim();
                    }

                    SSList.ActiveSheet.Cells[i, 16].Text = "";  //참고사항 입력칸

                    if (!list[i].JSAUPNO.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 17].Text = list[i].JSAUPNO.Trim();
                    }

                }

                //세금계산서 담당자 표시
                List<HIC_LTD_TAX> item = hicLtdTaxService.GetTaxDamDang(list[i].DLTD, list[i].LTDCODE, list[i].GJONG);

                if (item.Count > 0)
                {
                    if (!item[0].DAMNAME.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 6].Text = item[0].DAMNAME.Trim();
                    }

                    if (!item[0].TEL.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 7].Text = item[0].TEL.Trim();
                    }

                    if (!item[0].EMAIL.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 8].Text = item[0].EMAIL.Trim();
                    }

                    if (!item[0].HPHONE.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 9].Text = item[0].HPHONE.Trim();
                    }
                }
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            btnLtd.Visible = false;
            FrameMst.Visible = false;
            SS2.Visible = false;

            read_sysdate();
            nYY = VB.Left(clsPublic.GstrSysDate, 4).To<long>(0);
            nMM = VB.Format(VB.Mid(clsPublic.GstrSysDate, 6, 2)).To<long>(0);

            // 작업월 COMBO SET
            cboYYMM.Clear();
            for (int i = 0; i < 6; i++)
            {
                cboYYMM.Items.Add(VB.Format(nYY, "0000") + "-" + VB.Format(nMM, "00"));
                nMM -= 1;
                if (nMM == 0) { nMM = 12; nYY -= 1; }
            }
            cboYYMM.SelectedIndex = 0;

            // 미수종류 SET
            cboMJong.Clear();
            cboMJong.Items.Add("1.건강검진");
            cboMJong.Items.Add("2.보건대행");
            cboMJong.Items.Add("3.작업측정");
            cboMJong.Items.Add("4.종합검진");
            cboMJong.SelectedIndex = 0;

            cboJONG.Clear();
            cHB.ComboJong_AddItem(cboJONG, false);

            cboChungu.Clear();
            cboChungu.Items.Add("1.현금");
            cboChungu.Items.Add("2.수표");
            cboChungu.Items.Add("3.어음");
            cboChungu.Items.Add("4.외상미수금");
            cboChungu.SelectedIndex = 3;

            TxtBalDate.Text = clsPublic.GstrSysDate;

            Screen_Clear();

            // 전산실, 건진과장 통계빌드 및 자료삭제 작업가능, 강성민 = 통계 빌드 작업가능
            if (Strings.InStr(clsHcVariable.B08_MisuAdminSabun, "," + VB.Format(clsType.User.IdNumber.To<long>(0), "#0") + ",") > 0)
            {
                btnLtd.Visible = true;
                FrameMst.Visible = true;
                SS2.Visible = true;
                FrameMst.Enabled = true;
            }
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
            FstrSangHo = "";

            // 미수마스타 내역
            TxtLtdCode.Text = "";
            PanelLtdName.Text = "";
            cboJONG.Text = "";
            TxtMirNo.Text = "";
            TxtDate.Text = "";
            TxtMisuAmt.Text = "";
            TxtRemark.Text = "";
            PanelMisuJong.Text = "";
            cboGbn.Text = "";
            TxtDamName.Text = "";
            txtDname.Text = "";
            txtUptae.Text = "";
            txtJongMok.Text = "";
            txtLtdNo.Text = "";
            txtJuso.Text = "";
            TxtPumMok.Text = "";
            txtStandard.Text = "";
            txtNum.Text = "";
            txtPrice.Text = "";

            TxtJSaupNo.Text = "";
            TxtDamName2.Text = "";
            TxtTel.Text = "";
            TxtEMail.Text = "";
            TxtHphone.Text = "";

            SS_Clear(SS1_Sheet1);

            // 변동내역 Clear
            SSList.ActiveSheet.RowCount = 30;
            SS_Clear(SS2_Sheet1);
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

        void read_sysdate()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }
    }
}