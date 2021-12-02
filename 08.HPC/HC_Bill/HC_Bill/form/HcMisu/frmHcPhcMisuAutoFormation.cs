using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComPmpaLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Bill
/// File Name       : frmHcPhcMisuAutoFormation.cs
/// Description     : 보건소 미수 자동형성
/// Author          : 심명섭
/// Create Date     : 2021-06-23
/// Update History  : 
/// </summary>
/// <seealso cref= "hcmisu > Frm보건소미수자동형성 (Frm보건소미수자동형성.frm)" />
/// 

namespace HC_Bill
{
    public partial class frmHcPhcMisuAutoFormation :BaseForm
    {
        // 약한참조
        clsSpread cSpd = null;
        clsPublic cpublic = null;
        ComFunc cF = null;
        clsHaBase cHb = null;
        clsHcMisu cHM = null;
        clsHcMain hM = null;
        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;
        frmHcCodeHelp FrmHcCodeHelp = null;
        HicJepsuSunapService hicJepsuSunapService = null;
        HicMirCancerService hicMirCancerService = null;
        HicCodeService hicCodeService = null;
        HicJepsuService hicJepsuService = null;
        HicSunapService hicSunapService = null;
        HicMisuDetailService hicMisuDetailService = null;
        HicMisuMstSlipService hicMisuMstSlipService = null;
        HicLtdService hicLtdService = null;
        HicComHpcService hicComHpcService = null;

        const string Jong_4 = "'31','35'";      // 암검진

        List<string> Wrtno = new List<string>();

        bool Gubun = true;

        int result;
        int ninwon;

        long nRow = 0;
        long nCnt;
        long nWrtno;
        long nMisuNo;
        long nTempAmt;

        double nTotAmt;
        double nAmt;

        string FstrCode = "";
        string FstrName = "";
        string FstrLtdName;
        string FstrJong;
        string FstrLtdCode;
        string FstrFDate;
        string FstrTDate;
        string FstrKiho;
        string strMinDate;
        string strMaxDate;
        string strOldData;
        string strNewData = "";
        string strDate;
        string strOK;
        string FstrChaDate;
        string strChk;
        string strBDate;
        string strGJong;
        string strRemark;
        string strPumMok;
        string strGiroNo;
        string strROWID;
        string strltdcode;
        string strFDate;
        string strTDate;
        string strYEAR;
        

        public frmHcPhcMisuAutoFormation()
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
            cHb = new clsHaBase();
            cHM = new clsHcMisu();
            hM = new clsHcMain();
            hicJepsuSunapService = new HicJepsuSunapService();
            hicMirCancerService = new HicMirCancerService();
            hicCodeService = new HicCodeService();
            hicJepsuService = new HicJepsuService();
            hicSunapService = new HicSunapService();
            hicMisuDetailService = new HicMisuDetailService();
            hicMisuMstSlipService = new HicMisuMstSlipService();
            hicLtdService = new HicLtdService();
            hicComHpcService = new HicComHpcService();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnKihoHelp.Click += new EventHandler(eBtnClick);
            this.btnBogunHelp.Click += new EventHandler(eBtnClick);
            this.btnHelp2.Click += new EventHandler(eBtnClick);
            this.menuPrint1.Click += new EventHandler(eBtnClick);
            this.menuPrint2.Click += new EventHandler(eBtnClick);
            this.menuEtc.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);

            this.TxtYYMM.LostFocus += new EventHandler(Sub_LostFocus);

            // keyDown
            this.TxtLtdCode1.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.TxtBogun.KeyDown += new KeyEventHandler(eTxtKeyDown);
        }

        private void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == TxtLtdCode1)
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    LtdSearch();
                    TxtYYMM.Focus();
                }
            }
            else if (sender == TxtBogun)
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    BogunSearch(TxtBogun.Text);
                }
            }
        }

        private void BogunSearch(string text)
        {
            PanelBogun.Text = "";
            if(TxtBogun.Text != "")
            {
                List<HIC_CODE> BogunSearch = hicCodeService.BogunSearch(TxtBogun.Text);
                if(BogunSearch.Count > 0)
                {
                    PanelBogun.Text = BogunSearch[0].NAME;
                }
            }
        }

        private void LtdSearch()
        {
            // 상호로 사업장코드 찾기
            if (!IsNumeric(TxtLtdCode1.Text))
            {
                TxtLtdCode1.Text = cHb.LtdName_2_Code(TxtLtdCode1.Text.Trim());
                if(TxtLtdCode.Text == "")
                {
                    return;
                }
                else
                {
                    PanelLtdName.Text = cHb.READ_Ltd_Name(TxtLtdCode.Text);
                }
            }
            else
            {
                PanelLtdName.Text = cHb.READ_Ltd_Name(TxtLtdCode1.Text.Trim());
            }
        }

        private bool IsNumeric(string input)
        {
            int number = 0;
            return int.TryParse(input, out number);
        }

        private void Sub_LostFocus(object sender, EventArgs e)
        {
            if(string.Compare(VB.Left(VB.Replace(clsPublic.GstrSysDate, "-", ""), 6), TxtYYMM.Text.Trim()) < 0)
            {
                MessageBox.Show("검진년월 입력오류");
                TxtYYMM.Focus();
                return;
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            int nRow;
            long nTotAmt;
            long nTempAmt;
            long nMirNo;
            string strBogenso;
            long nAmt1;
            long nAmt2;
            long nTotAmt1;
            long nTotAmt2;
            long nTotHicAmt;

            if (e.Row < 0)
            {
                return;
            }

            FstrJong = VB.Left(cboJong.Text, 1);

            strBogenso = SS1.ActiveSheet.Cells[e.Row, 0].Text;
            FstrLtdName = SS1.ActiveSheet.Cells[e.Row, 1].Text;
            FstrFDate = SS1.ActiveSheet.Cells[e.Row, 2].Text;
            FstrTDate = SS1.ActiveSheet.Cells[e.Row, 3].Text;
            nMirNo = SS1.ActiveSheet.Cells[e.Row, 7].Text.To<long>(0);
            FstrKiho = SS1.ActiveSheet.Cells[e.Row, 8].Text;

            List<HIC_LTD> item = hicLtdService.GetCodeName(strBogenso);

            TxtLtdCode.Text = "";
            TxtLtdName.Text = FstrLtdName;

            if (item.Count > 0)
            {
                TxtLtdCode.Text = item[0].CODE.ToString().Trim();
                TxtLtdName.Text = item[0].NAME.Trim();
                FstrLtdName = item[0].NAME.Trim();
            }

            TxtInwon.Text = "";
            TxtMisuAmt.Text = "";
            TxtGiroNo.Text = "";
            TxtRemark.Text = "";
            TxtPummok.Text = "";
            TxtChaAmt.Text = "";
            TxtChaRemark.Text = "";

            // 해당 회사의 청구명단을 표시함
            SS2.ActiveSheet.RowCount = 50;
            SS_Clear(SS2_Sheet1);

            nTotAmt = 0;
            nAmt1 = 0;
            nAmt2 = 0;
            nTotAmt1 = 0;
            nTotAmt2 = 0;
            nTotHicAmt = 0;

            // 명단을 SELECT

            List<COMHPC> list = hicComHpcService.GetBogunListData(FstrFDate, FstrTDate, strBogenso, TxtKiho.Trim(), nMirNo, ChkW_Am.Checked, ChkGub.Checked, ChkBogen.Checked);
            nRow = 0;
            strOldData = "";

            for(int i = 0; i < list.Count; i++)
            {
                cHb.READ_SUNAPDTL_PublicHealth(list[i].WRTNO);
                nTempAmt = clsHcVariable.GnAmt_Misu_BogenAmt1.To<long>(0);

                if(nTempAmt != 0)
                {
                    nRow += 1;

                    if (nRow > SS2.ActiveSheet.RowCount)
                    {
                        SS2.ActiveSheet.RowCount = nRow;
                    }

                    SS2.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].PANO.To<string>("0");

                    if (list[i].MURYOAM == "Y")
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, 1].BackColor = Color.LightPink;
                    }

                    SS2.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].SNAME.Trim();
                    SS2.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].JEPDATE.Trim();
                    SS2.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].WRTNO.ToString();
                    SS2.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].AGE.ToString() + " / " + list[i].SEX;
                    SS2.ActiveSheet.Cells[nRow - 1, 6].Text = cHb.READ_GjJong_Name(list[i].GJJONG.Trim());
                    SS2.ActiveSheet.Cells[nRow - 1, 7].Text = VB.Format(list[i].TOTAMT, "###,###,##0");
                    SS2.ActiveSheet.Cells[nRow - 1, 8].Text = VB.Format(list[i].LTDAMT, "###,###,##0");
                    SS2.ActiveSheet.Cells[nRow - 1, 9].Text = VB.Format(nTempAmt, "###,###,##0");
                    SS2.ActiveSheet.Cells[nRow - 1, 10].Text = hM.SExam_Names_Display(list[i].SEXAMS);
                    if (list[i].UCODES.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, 11].Text = "";
                    }
                    else
                    {
                        if (list[i].UCODES.Trim() != "ZZZ")
                        {
                            SS2.ActiveSheet.Cells[nRow - 1, 11].Text = cHM.Ucode_Name_Display(list[i].UCODES.Trim());
                        }
                    }
                    //SS2.ActiveSheet.Cells[nRow - 1, 11].Text = (list[i].UCODES.Trim() != "ZZZ") ? cHM.Ucode_Name_Display(list[i].UCODES.Trim()) : "";
                    SS2.ActiveSheet.Cells[nRow - 1, 12].Text = list[i].GJCHASU.Trim();

                    nTotAmt += nTempAmt;
                }
            }

            nRow += 1;
            SS2.ActiveSheet.RowCount = nRow;
            SS2.ActiveSheet.Cells[nRow - 1, 6].Text = "** 합계 **";
            SS2.ActiveSheet.Cells[nRow - 1, 9].Text = VB.Format(nTotAmt, "###,###,##0");

            Inwon_Amt_Gesan();

            if(TxtLtdCode.Text == "")
            {
                MessageBox.Show("의료급여암, 보건소암은 반드시 회사코드를 선택 후 미수를 생성하십시오");
            }

        }

        private void Inwon_Amt_Gesan()
        {
            long nCnt, nMisuAmt;
            long nMisuAmt1;
            string strChk = "";
            string strOldData;
            string strNewData;

            string strFlag;
            string strChasu;
            long nChaCnt1;
            long nChaCnt2;
            long nChaCnt3;
            long nChaSUM1;
            long nChaSUM2;
            long nChaSUM3;

            string strOK;
            string strJong;
            long nWrtno;


            strFlag = "";
            strJong = "";
            nChaCnt1 = 0;
            nChaCnt2 = 0;
            nChaCnt3 = 0;
            nChaSUM1 = 0;
            nChaSUM2 = 0;
            nChaSUM3 = 0;
            strOldData = "";
            nCnt = 0;
            nMisuAmt = 0;

            for(int i = 0; i < SS2.ActiveSheet.RowCount - 1; i++)
            {
                if(SS2.ActiveSheet.Cells[i, 0].Text.Trim() == "")
                {
                    strChk = "False";
                }
                else if(SS2.ActiveSheet.Cells[i, 0].Text.Trim() == "True")
                {
                    strChk = "True";
                }
                else if (SS2.ActiveSheet.Cells[i, 0].Text.Trim() == "False")
                {
                    strChk = "False";
                }

                nWrtno = SS2.ActiveSheet.Cells[i, 1].Text.Trim().To<long>(0);
                nMisuAmt1 = VB.Replace(SS2.ActiveSheet.Cells[i, 9].Text, ",", "").To<long>(0);
                strChasu = SS2.ActiveSheet.Cells[i, 12].Text.Trim();

                strOK = "";
                if(rdoJob1.Checked == true && strChk == "True")
                {
                    strOK = "OK";
                }
                if(rdoJob2.Checked == true && strChk == "False")
                {
                    strOK = "OK";
                }

                if(strOK == "OK" && nMisuAmt1 != 0)
                { 
                    // 인원카운트
                    strNewData = SS2.ActiveSheet.Cells[i, 4].Text.Trim();
                    if(strOldData != strNewData) { nCnt += 1; strOldData = strNewData; }

                    // 차수별 카운트
                    if(strChasu == "1")
                    {
                        nChaCnt1 += 1;
                        nChaSUM1 += nMisuAmt1;
                    }
                    else if(strChasu == "2")
                    {
                        nChaCnt2 += 1;
                        nChaSUM2 += nMisuAmt1;
                    }
                    else
                    {
                        nChaCnt3 += 1;
                        nChaSUM3 += nMisuAmt1;
                    }

                    // 총금액
                    nMisuAmt += nMisuAmt1;
                }
            }

            TxtInwon.Text = VB.Format(nCnt, "###,##0");
            TxtMisuAmt.Text = VB.Format(nMisuAmt, "###,###,###,##0");
            strJong = "암검진";

            if (nChaCnt3 > 0) { strFlag = "3"; }
            if (FstrJong == "6") { strFlag = "3"; }
            if (strFlag != "3")
            {
                // 2010-11-08 윤조연 수정
                if(nChaSUM2 == 0)
                {
                    TxtRemark.Text = strJong + "1차 금액 :" + VB.Format(nChaSUM1, "###,###,##0") + "원";
                }
                else
                {
                    TxtRemark.Text = strJong + "1차 금액 :" + VB.Format(nChaSUM1, "###,###,##0") + "원, 2차 금액 : " + VB.Format(nChaSUM2, "###,###,##0") + "원";
                }

                if(nChaCnt2 == 0)
                {
                    TxtPummok.Text = "1차 인원 : " + VB.Format(nChaCnt1, "###,###,##0") + "명";
                }
                else
                {
                    TxtPummok.Text = "1차 인원 : " + VB.Format(nChaCnt1, "###,###,##0") + "명, 2차 인원 : " + VB.Format(nChaCnt2, "###,###,##0") + "명";
                }
            }
            else
            {
                if(FstrJong != "6")
                {
                    TxtRemark.Text = strJong + "금액 : " + VB.Format(nMisuAmt, "###,###,##0") + "원";
                    TxtPummok.Text = "인원 : " + VB.Format(nCnt, "###,###,##0") + "명";
                }
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            long nYY;
            SS2.ActiveSheet.Columns.Get(12).Visible = false;
            read_sysdate();
            dtpFDate.Text = cF.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, -60);

            if (string.Compare(dtpFDate.Text, "2013-01-01") < 0)
            {
                dtpFDate.Text = "2013-01-01";
            }

            dtpTDate.Text = clsPublic.GstrSysDate;
            TxtDate.Text = clsPublic.GstrSysDate;

            cboJong.Clear();
            cboJong.Items.Add("1.성인병");
            cboJong.Items.Add("2.공무원");
            cboJong.Items.Add("3.사업장");
            cboJong.Items.Add("4.암검진");
            cboJong.Items.Add("5.기타검진");
            cboJong.SelectedIndex = 3;

            TxtGiroNo.Text = "";
            TxtLtdCode.Text = "";
            TxtLtdName.Text = "";
            TxtInwon.Text = "";
            TxtMisuAmt.Text = "";
            TxtGiroNo.Text = "";
            TxtRemark.Text = "";
            TxtPummok.Text = "";
            TxtLtdCode1.Text = "";
            PanelLtdName.Text = "";
            PanelKihoName.Text = "";
            PanelBogun.Text = "";
            TxtChaAmt.Text = "";
            TxtChaRemark.Text = "";
            TxtYYMM.Text = "";

            menuEtc.Enabled = false;
            nYY = VB.Left(clsPublic.GstrSysDate, 4).To<long>(0);
            cboYear.Clear();

            for(int i = 0; i < 6; i++)
            {
                cboYear.Items.Add(VB.Format(nYY, "0000"));
                nYY -= 1;
            }
            cboYear.SelectedIndex = 0;

            TxtLtdCode.Enabled = false;
            TxtInwon.Enabled = false;
            TxtLtdName.Enabled = false;
            cboJong.Enabled = false;
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
            else if (sender == btnSave)
            {
                DataSave();
            }
            else if(sender == btnLtdHelp)
            {
                Gubun = true;
                LtdHelp(Gubun);
            }
            else if(sender == btnKihoHelp)
            {
                KihoHelp();
            }
            else if(sender == btnBogunHelp)
            {
                BogunHelp();
            }
            else if(sender == btnHelp2)
            {
                Gubun = false;
                LtdHelp(Gubun);
            }
            else if(sender == menuPrint1)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }

                Gubun = true;
                menuPrint(Gubun);
            }
            else if (sender == menuPrint2)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }

                Gubun = false;
                menuPrint(Gubun);
            }
            else if(sender == menuEtc)
            {
                SelfChungGu();
            }
        }

        private void SelfChungGu()
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strltdcode = SS1.ActiveSheet.Cells[i, 1].Text;
                    strFDate = SS1.ActiveSheet.Cells[i, 3].Text;
                    strTDate = SS1.ActiveSheet.Cells[i, 4].Text;
                    nAmt = SS1.ActiveSheet.Cells[i, 6].Text.To<double>(0);

                    // 미수번호 찾기
                    List<HIC_MISU_MST_SLIP> MisuNumSearch = hicMisuMstSlipService.getMisuNum(strltdcode, FstrJong, strTDate, nAmt);
                    nMisuNo = 0;
                    strBDate = "";
                    if (MisuNumSearch.Count > 0)
                    {
                        nMisuNo = MisuNumSearch[0].WRTNO;
                        strBDate = MisuNumSearch[0].BDATE.Trim();
                        strROWID = MisuNumSearch[0].ROWID.Trim();
                    }

                    if (nMisuNo > 0)
                    {
                        if (!RowidUpdate())
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        if (FstrJong.To<int>(0) <= 4)
                        {
                            SelfChungGu_Sub();
                        }
                    }
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

        private void SelfChungGu_Sub()
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                if (!CompanyChunguUpdate())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (!CompanySuNapUpdate())
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

        private bool CompanySuNapUpdate()
        {
            result = hicSunapService.CompanySuNapUpdate(nMisuNo, strFDate, strTDate, strltdcode);

            if (result < 0)
            {
                MessageBox.Show("수납마스타에 공단청구 완료처리 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool CompanyChunguUpdate()
        {
            result = hicJepsuService.CompanyChunguUpdate(nMisuNo, strFDate, strTDate, strltdcode, FstrJong);

            if (result < 0)
            {
                MessageBox.Show("접수마스타에 공단청구 완료처리 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool RowidUpdate()
        {
            result = hicMisuMstSlipService.UpdateRowid(strROWID);

            if (result < 0)
            {
                MessageBox.Show("ROWID UPDATA 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void menuPrint(bool Gubun)
        {
            if(Gubun == true)
            {
                string strTitle = "";
                string strSign = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "공단부담액 청구대상 공단명단";
                strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += cSpd.setSpdPrint_String("회사명칭 : " + PanelLtdName.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += cSpd.setSpdPrint_String("인쇄시각 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A") + VB.Space(20) + "PAGE :" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = cSpd.setSpdPrint_String(strSign, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 40, 40);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, false, false, false, true, 1f);
                cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else
            {
                string strTitle = "";
                string strSign = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "건강진단비 청구내역";
                strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += cSpd.setSpdPrint_String("작업기간 : " + dtpFDate.Text + " ~ " + dtpTDate.Text + VB.Space(10) +"회사명칭 : " + PanelLtdName.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = cSpd.setSpdPrint_String(strSign, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 40, 40);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, false, false, false, true, 1f);
                cSpd.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        private void BogunHelp()
        {
            clsPublic.GstrRetValue = "25";      //보건소 기호
            FrmHcCodeHelp = new frmHcCodeHelp(clsPublic.GstrRetValue);
            FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(eCode_value);
            FrmHcCodeHelp.ShowDialog();

            if (!FstrCode.IsNullOrEmpty())
            {
                TxtBogun.Text = FstrCode.Trim();
                PanelBogun.Text = FstrName.Trim();
            }
            else
            {
                TxtBogun.Text = "";
                PanelBogun.Text = "";
            }

            FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(eCode_value);
        }

        private void KihoHelp()
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

        private void eCode_value(string strCode, string strName)
        {
            FstrCode = strCode;
            FstrName = strName;
        }

        private void LtdHelp(bool Gubun)
        {
            if(Gubun == true)
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
                    TxtLtdCode1.Text = LtdHelpItem.CODE.ToString();
                    PanelLtdName.Text = LtdHelpItem.SANGHO;
                }
                else
                {
                    TxtLtdCode1.Text = "";
                    PanelLtdName.Text = "";
                }
            }
            else
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
                    TxtLtdName.Text = LtdHelpItem.SANGHO;
                }
                else
                {
                    TxtLtdCode.Text = "";
                    TxtLtdName.Text = "";
                }
            }

            
        }

        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void DataSave()
        {
            if(ChkBogen.Checked == false && ChkGub.Checked == false)
            {
                MessageBox.Show("1개 이상 (보건소/급여) 종류를 선택하여 주십시오", "작업선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (TxtLtdCode.Text.Trim()  == "") { MessageBox.Show("회사기호가 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (TxtGiroNo.Text.Trim()   == "") { MessageBox.Show("청구(지로)번호를 입력하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (TxtRemark.Text.Trim()   == "") { MessageBox.Show("적요가 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (TxtPummok.Text.Trim()   == "") { MessageBox.Show("품목이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (TxtYYMM.Text.Trim()     == "") { MessageBox.Show("검진년월이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (VB.Len(TxtYYMM.Text)    !=  6) { MessageBox.Show("검진년월형식을 YYYYMM(202111)으로 입력하십시오.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if(ChkGub.Checked == true)
            {
                if(TxtLtdCode.Text.Trim() == "0174" || TxtLtdCode.Text.Trim() == "")
                {
                    MessageBox.Show("의료급여암, 보건소암은 반드시 회사코드를 선택후 미수를 생성하십시오.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            FstrJong = VB.Left(cboJong.Text, 1);    // 미수청구구분
            strGiroNo = TxtGiroNo.Text.Trim();

            FstrChaDate = "";
            if(TxtChaAmt.Text.To<int>(0) != 0)
            {
                FstrChaDate = clsPublic.GstrSysDate;
            }
            nAmt = 0;

            if(ChkBogen.Checked == true)
            {
                clsPublic.GstrMsgList = "보건소암 회사미수내역을 자동형성 하시겠습니까?";
            }
            else
            {
                if(ChkGub.Checked == true)  // 의료급여암
                {
                    clsPublic.GstrMsgList = "의료급여 회사미수내역을 자동형성 하시겠습니까?";
                }
            }

            if(MessageBox.Show(clsPublic.GstrMsgList, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            strBDate = TxtDate.Text.Trim();
            if(strBDate == "") { MessageBox.Show("공단미수일자가 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            
            Wrtno.Clear();

            for (int i = 0; i < SS2.ActiveSheet.RowCount - 1; i++)
            {
                strChk = SS2.ActiveSheet.Cells[i, 0].Text;
                strOK = "";
                strChk = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                nWrtno = SS2.ActiveSheet.Cells[i, 4].Text.To<long>(0);
                strOK = "";
                if (rdoJob1.Checked == true && strChk == "True") { strOK = "OK"; }
                if (rdoJob2.Checked == true && strChk != "True") { strOK = "OK"; }

                if(strOK == "OK")
                {
                    if(cboYear.Text.Trim() != cHM.READ_GJYEAR(nWrtno))
                    {
                        MessageBox.Show("검진년도와 미수검진년도가 서로 다릅니다.", "미수발생불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    Wrtno.Add(VB.Format(nWrtno, "########0"));
                }
            }
            
            
           if (Wrtno.Count > 0)
            {   // 일반건진
                if (!Save_Sub1())
                {
                    return;
                }
            }

            DisPlay_Screen();

            if(ChkBogen.Checked == true)
            {
                MessageBox.Show("보건소암-회사미수형성이 완료되었습니다.", "확인", MessageBoxButtons.OK);
            }
            else
            {
                if(ChkGub.Checked == true)  //의료급여암
                {
                    MessageBox.Show("보건소암-회사미수형성이 완료되었습니다.", "확인", MessageBoxButtons.OK);
                }
            }

            Screen_Clear();
            
        }

        private void Screen_Clear()
        {
            TxtGiroNo.Text = "";
            TxtLtdCode.Text = "";
            TxtLtdName.Text = "";
            TxtInwon.Text = "";
            TxtMisuAmt.Text = "";
            TxtGiroNo.Text = "";
            TxtRemark.Text = "";
            TxtLtdCode1.Text = "";
            TxtPummok.Text = "";
            PanelLtdName.Text = "";
            PanelKihoName.Text = "";
            TxtChaRemark.Text = "";
            PanelBogun.Text = "";
            TxtChaAmt.Text = "";

            SS_Clear(SS2_Sheet1);
        }

        // --------------------( 일반건진 1건 미수 형성 )--------------------------
        private bool Save_Sub1()
        {
            nMisuNo = cHb.READ_New_MisuNo();
            FstrLtdCode = TxtLtdCode.Text.Trim();

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                // 회사미수             
                if (!JepSuMaster())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return false;
                }

                // 회사미수             
                if (!SuNapMaster())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return false;
                }

                // 미청구 인원수, 금액을 집계
                List<HIC_JEPSU_SUNAP> item = hicJepsuSunapService.GetInWonCash(FstrFDate, FstrTDate, Wrtno, nMisuNo);

                if(item.Count <= 0)
                {
                    MessageBox.Show("미청구 인원수, 금액을 집계.", "미수발생불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                ninwon = 0;
                nAmt = 0;
                strGJong = "31";

                if(FstrJong != "6")
                {
                    for(int i = 0; i < item.Count; i++)
                    {
                        nWrtno = item[i].WRTNO;
                        cHb.READ_SUNAPDTL_PublicHealth(nWrtno);

                        nTempAmt = clsHcVariable.GnAmt_Misu_BogenAmt1.To<long>(0);

                        if(nTempAmt != 0)
                        {
                            ninwon += 1;
                            nAmt += nTempAmt;
                        }

                        List<HIC_JEPSU> list = hicJepsuService.GetWRTNO(nWrtno);
                        strltdcode = VB.Format(list[0].LTDCODE, "#");
                        strGJong = list[0].GJJONG;

                        // HIC_MISU_DETAIL에 상세내역을 입력             
                        if (!MisuDetail())
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return false;
                        }
                    }
                }
                // 암검진일경우는 금액을 TxtMisuAmt 사용함
                if (FstrJong == "4") { nAmt = VB.TR(TxtMisuAmt.Text, ",", "").To<double>(0); }
                

                if(nAmt != 0)
                {
                    // 2004-06-11 적요 및 품목을 별도 입력하도록 수정함
                    strRemark = TxtRemark.Text.Trim();
                    strPumMok = TxtPummok.Text.Trim();
                    strYEAR = cboYear.Text.Trim();

                    // 미수마스타에 신규등록
                    if (!MisuMasterNew())
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return false;
                    }

                    // 미수SLIP에 신규등록
                    if (!MisuSlipNew())
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return false;
                    }

                    // 신규입력 내역을 History에 INSERT
                    if (!NewHistoryInsert())
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return false;
                    }

                }

                clsDB.setCommitTran(clsDB.DbCon);
                FstrKiho = "";

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }

        }

        private bool NewHistoryInsert()
        {
            result = hicMisuMstSlipService.NewHistoryInsert(clsPublic.GnJobSabun, nMisuNo);
            if (result < 0)
            {
                MessageBox.Show("신규등록 내역을 HISTORY에 INSERT시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool MisuSlipNew()
        {
            HIC_MISU_MST_SLIP item = new HIC_MISU_MST_SLIP
            {
                WRTNO =            nMisuNo,
                BDATE =            strBDate,
                LTDCODE =          FstrLtdCode.To<long>(0),
                GEACODE =          "11",
                SLIPAMT =          nAmt.To<long>(0),
                REMARK =           strRemark,
                ENTSABUN =         clsType.User.IdNumber.To<long>(0)
            };

            result = hicMisuMstSlipService.MisuSlipNew(item);
            if (result < 0)
            {
                MessageBox.Show("미수SLIP에 등록중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool MisuMasterNew()
        {
            HIC_MISU_MST_SLIP item = new HIC_MISU_MST_SLIP
            {
                WRTNO =            nMisuNo,
                LTDCODE =          TxtLtdCode.Text.To<long>(0),
                MISUJONG =         "1",
                JISA =             "0719",
                KIHO =             FstrKiho,
                BDATE =            strBDate,
                GJONG =            strGJong,
                GBEND =            "N",
                MISUGBN =          "1",
                MISUAMT =          nAmt.To<long>(0),
                IPGUMAMT =         0,
                GAMAMT =           0,
                SAKAMT =           0,
                BANAMT =           0,
                JANAMT =           nAmt.To<long>(0),
                MIRCHAAMT =        TxtChaAmt.Text.To<long>(0),
                MIRCHAREMARK =     TxtChaRemark.Text.Trim(),
                MIRCHADATE =       FstrChaDate,
                DAMNAME =          clsPublic.GstrJobName,
                REMARK =           strRemark,
                PUMMOK =           strPumMok,
                GIRONO =           strGiroNo,
                MIRGBN =           "1",
                GBMISUBUILD4 =     "Y",
                YYMM_JIN =         TxtYYMM.Text.Trim(),
                ENTSABUN =         clsType.User.IdNumber.To<long>(0),
                GJYEAR =           strYEAR
            };

            result = hicMisuMstSlipService.MisuMasterNew(item);
            if (result < 0)
            {
                MessageBox.Show("HIC_MISU_MST에 자료를 신규등록시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool MisuDetail()
        {

            HIC_MISU_DETAIL item = new HIC_MISU_DETAIL
            {
                WRTNO =             nWrtno,
                MISUNO =            nMisuNo,   
                GUBUN =             "H",
                GJJONG =            strGJong,
                LTDCODE =           strltdcode.To<long>(0),
                MISUJONG =          "5",
                BDATE =             strBDate,   
                ENTJOBSABUN =       clsType.User.IdNumber.To<long>(0),
                MISUAMT =           nTempAmt,
                IPGUMAMT =          0,
                GAMAMT =            0,
                BANAMT =            0,
                SAKAMT =            0,
                HALAMT =            0,
                ETCAMT =            0,
                JANAMT =            0,
                DAMNAME =           clsHelpDesk.READ_INSA_NAME(clsDB.DbCon, clsType.User.IdNumber.Trim()),
                REMARK =            ""
            };

            result = hicMisuDetailService.InsertList(item);

            if (result < 0)
            {
                MessageBox.Show("HIC_MISU_DETAIL에 자료를 신규등록시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool SuNapMaster()
        {
            result = hicSunapService.UpdateGongDan(nMisuNo, Wrtno);

            if (result < 0)
            {
                MessageBox.Show("수납마스타에 공단청구 완료처리 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool JepSuMaster()
        {
            result = hicJepsuService.UpdateBogunMisu(nMisuNo, Wrtno);

            if (result < 0)
            {
                MessageBox.Show("접수마스타에 보건소청구 완료처리 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void DisPlay_Screen()
        {
            // 2013년 이전 자료 조회 불가
            if (string.Compare(dtpFDate.Text, "2003-01-01") < 0)
            {
                MessageBox.Show("2013년도 이후 작업만 가능합니다.");
                return;
            }
            
            if (string.Compare(TxtDate.Text, "2003-01-01") < 0)
            {
                MessageBox.Show("2013년도 이후 작업만 가능합니다.");
                return;
            }

            nRow = 0;

            if(ChkBogen.Checked == false && ChkGub.Checked == false && ChkW_Am.Checked == false)
            {
                MessageBox.Show("1개 이상 (보건소/급여) 종류를 선택하여 주십시오", "작업선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FstrJong = VB.Left(cboJong.Text, 1);
            SS1.ActiveSheet.RowCount = 50;

            SS_Clear(SS1_Sheet1);

            // 미청구 공단별 인원수, 금액을 집계
            List<HIC_JEPSU_SUNAP> item = hicJepsuSunapService.GetInWonMoney(ChkBogen.Checked, ChkW_Am.Checked, TxtBogun.Text, TxtKiho.Text.Trim(), TxtLtdCode1.Text.Trim(), ChkGub.Checked, dtpFDate.Text, dtpTDate.Text);

            strOldData = "";
            strMinDate = "";
            strMaxDate = "";
            nCnt = 0;
            nTotAmt = 0;

            for(int i = 0; i < item.Count; i++)
            {
                // 청구를 하였는지 점검함
                result = hicMirCancerService.CheckChungu(item[i].MIRNO4);

                if (result < 0)
                {
                    strOK = "";
                    MessageBox.Show("청구 점검 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    strOK = "OK";
                }

                if(strOK == "OK")
                {
                    strNewData = item[i].BOGUNSO + "{}";
                    strNewData += VB.Format(item[i].MIRNO, "#0");

                    cHb.READ_SUNAPDTL_PublicHealth(item[i].WRTNO);

                    nAmt = clsHcVariable.GnAmt_Misu_BogenAmt1;

                    if (nAmt != 0)
                    {
                        if (strOldData == "") { strOldData = strNewData; }
                        if (strOldData != strNewData)
                        {
                            View_Display();
                        }
                        // 회사별 인원 및 금액을 누적
                        nCnt += 1;
                        nTotAmt += nAmt;
                        // 검진시작일, 종료일
                        strDate = item[i].MINDATE.ToString();
                        if (strMinDate == "") { strMinDate = strDate; }
                        if (string.Compare(strDate, strMinDate) < 0) { strMinDate = strDate; }
                        strDate = item[i].MAXDATE.ToString();
                        if (strMaxDate == "") { strMaxDate = strDate; }
                        if (string.Compare(strDate, strMaxDate) > 0) { strMaxDate = strDate; }
                    }
                }
            }

            View_Display();
            SS1.ActiveSheet.RowCount = nRow.To<int>(0);
        }

        private void View_Display()
        {
            nRow += 1;
            if (nRow > SS1.ActiveSheet.RowCount)
            {
                SS1.ActiveSheet.RowCount = nRow.To<int>(0);
            }

            SS1.ActiveSheet.Cells[nRow.To<int>(0) - 1, 0].Text = VB.Pstr(strOldData, "{}", 1);


            List<HIC_CODE> item = hicCodeService.GetCodeNameGcode1(VB.Pstr(strOldData, "{}", 1));
            result = hicCodeService.GetCodeNameGcode(VB.Pstr(strOldData, "{}", 1));

            if (result < 0)
            {
                MessageBox.Show("View Display 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if(item.Count > 0 && item.Count != 0)
                {
                    SS1.ActiveSheet.Cells[nRow.To<int>(0) - 1, 1].Text = item[0].NAME;
                }
                else
                {
                    SS1.ActiveSheet.Cells[nRow.To<int>(0) - 1, 1].Text = "";
                }
            }

            SS1.ActiveSheet.Cells[nRow.To<int>(0) - 1, 2].Text = strMinDate;
            SS1.ActiveSheet.Cells[nRow.To<int>(0) - 1, 3].Text = strMaxDate;
            SS1.ActiveSheet.Cells[nRow.To<int>(0) - 1, 4].Text = nCnt.ToString();
            SS1.ActiveSheet.Cells[nRow.To<int>(0) - 1, 5].Text = VB.Format(nTotAmt, "###,###,###,##0");
            SS1.ActiveSheet.Cells[nRow.To<int>(0) - 1, 7].Text = VB.Pstr(strOldData, "{}" ,2);
            SS1.ActiveSheet.Cells[nRow.To<int>(0) - 1, 8].Text = clsHcVariable.GstrKiho;

            strOldData = strNewData;
            strMinDate = "";
            strMaxDate = "";
            nCnt = 0;
            nTotAmt = 0;

            return;
        }

        void read_sysdate()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
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

        private void ChkBogen_CheckedChanged(object sender, EventArgs e)
        {
            if(ChkBogen.Checked == true)
            {
                ChkGub.Checked = false;
            }
        }

        private void ChkGub_CheckedChanged(object sender, EventArgs e)
        {
            
            if (ChkGub.Checked == true)
            {
                ChkBogen.Checked = false;
            }
        }

        private void rdoJob2_CheckedChanged(object sender, EventArgs e)
        {
            Inwon_Amt_Gesan();
        }

        private void rdoJob1_CheckedChanged(object sender, EventArgs e)
        {
            Inwon_Amt_Gesan();
        }

        private void SS2_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            Inwon_Amt_Gesan();
        }

        private void cboJong_Click(object sender, EventArgs e)
        {
            TxtChaAmt.Enabled = false;
            TxtChaRemark.Enabled = false;
            TxtMisuAmt.Enabled = false;
        }
    }
}