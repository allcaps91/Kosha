using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Bill
/// File Name       : frmHcBillExpenseFileCreate.cs
/// Description     : 건강검진 청구대상자 점검 및 파일형성 [2020/생애포함] (폐암추가)
/// Author          : 이상훈
/// Create Date     : 2021-01-06
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm청구파일생성_2020.frm(Frm청구파일생성_2020)" />

namespace HC_Bill
{
    public partial class frmHcBillExpenseFileCreate : Form
    {
        HicJepsuService hicJepsuService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicJepsuSunapService hicJepsuSunapService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicResultSunapdtlService hicResultSunapdtlService = null;
        HicResultService hicResultService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicResBohum2Service hicResBohum2Service = null;
        HicMirBohumService hicMirBohumService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicMirDentalService hicMirDentalService = null;
        HicMirCancerBoService hicMirCancerBoService = null;
        HicMirCancerService hicMirCancerService = null;
        HicSunapService hicSunapService = null;
        HicJepsuCancerNewService hicJepsuCancerNewService = null;
        HicResDentalJepsuPatientService hicResDentalJepsuPatientService = null;
        HicTitemService hicTitemService = null;
        HicResBohum1JepsuPatientRepository hicResBohum1JepsuPatientRepository = null;
        HicResBohum1JepsuPatientService hicResBohum1JepsuPatientService = null;
        HicAmtBohumService hicAmtBohumService = null;
        HicMirErrorTongboService hicMirErrorTongboService = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;
        HicJepsuResBohum2Service hicJepsuResBohum2Service = null;
        HicJepsuResDentalService hicJepsuResDentalService = null;
        HicAmtCancerService hicAmtCancerService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicCancerNewService hicCancerNewService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        frmHcBillExpenses FrmHcBillExpenses = null;
        frmHcBillOralExamExpenses FrmHcBillOralExamExpenses = null;
        frmHcBillCancerExpenses FrmHcBillCancerExpenses = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();
        clsPrint CP = new clsPrint();

        string FstrLtdName;
        long FnMirNo;
        string FstrSname;
        string FstrSex;
        long FnAge;
        int FnRow;

        long FnErrCNT1;
        long FnErrCNT2;
        string FstrCOMMIT;
        string FstrOK;
        string FstrJik;

        StringBuilder FstrREC = new StringBuilder();

        string FstrSetYear;
        long FnAmt1;
        long[] FnOneCnt = new long[51];
        long[] FnTwoCnt = new long[51];
        long[] FnOneAmt = new long[51];
        long[] FnTwoAmt = new long[51];
        long[] FnAmAmt1 = new long[51];
        long[] FnAmAmt2 = new long[51];
        long[] FnAmAmt3 = new long[51];
        string FstrChasu;
        string FstrGunDate;
        string FstrJong;    // 검진종류
        string FstrFrDate;  // 시작일자

        long Fn절사금액합계1; //단 절사금액 합계
        long Fn절사금액합계2; //보건소 절사금액 합계

        string Fstr생활습관상담유무;    // 임시적으로 사용함 - 강제로 생활습관상담 금액포함여부

        bool FbGonghyu = false;
        int FnGonghyuCnt = 0;

        public frmHcBillExpenseFileCreate()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicSunapdtlService = new HicSunapdtlService();
            hicJepsuSunapService = new HicJepsuSunapService();
            comHpcLibBService = new ComHpcLibBService();
            hicResultSunapdtlService = new HicResultSunapdtlService();
            hicResultService = new HicResultService();
            hicResBohum1Service = new HicResBohum1Service();
            hicResBohum2Service = new HicResBohum2Service();
            hicMirBohumService = new HicMirBohumService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicMirDentalService = new HicMirDentalService();
            hicMirCancerBoService = new HicMirCancerBoService();
            hicMirCancerService = new HicMirCancerService();
            hicSunapService = new HicSunapService();
            hicJepsuCancerNewService = new HicJepsuCancerNewService();
            hicResDentalJepsuPatientService = new HicResDentalJepsuPatientService();
            hicTitemService = new HicTitemService();
            hicResBohum1JepsuPatientRepository = new HicResBohum1JepsuPatientRepository();
            hicResBohum1JepsuPatientService = new HicResBohum1JepsuPatientService();
            hicAmtBohumService = new HicAmtBohumService();
            hicMirErrorTongboService = new HicMirErrorTongboService();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            hicJepsuResBohum2Service = new HicJepsuResBohum2Service();
            hicJepsuResDentalService = new HicJepsuResDentalService();
            hicAmtCancerService = new HicAmtCancerService();
            hicResultExCodeService = new HicResultExCodeService();
            hicCancerNewService = new HicCancerNewService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnMenuPrint1.Click += new EventHandler(eBtnClick);
            this.btnMenuPrint2.Click += new EventHandler(eBtnClick);
            this.btnMenuPrint3.Click += new EventHandler(eBtnClick);
            this.btnMenuSangdam.Click += new EventHandler(eBtnClick);
            this.btnMenuErrSave.Click += new EventHandler(eBtnClick);
            this.btnErrCheck.Click += new EventHandler(eBtnClick);
            this.btnSearch2.Click += new EventHandler(eBtnClick);
            this.btn_DiskMake.Click += new EventHandler(eBtnClick);
            this.btnDel.Click += new EventHandler(eBtnClick);
            this.btnCDel.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS1.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.cboYear.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.cboJong.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.cboJong.Click += new EventHandler(eCboClick);
            
        }


        void SetControl()
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            SS1_Sheet1.Columns[11].Visible = false;
            SS1_Sheet1.Columns[14].Visible = false;

            SS1_Sheet1.Rows[-1].Height = 20;
            SS2_Sheet1.Rows[-1].Height = 20;
            SS3_Sheet1.Rows[-1].Height = 20;

            txtGongWon.Text = "";
            txtBoWon.Text = "";
            txtLtdCode.Text = "";

            //cboYear.Text = VB.Left(clsPublic.GstrSysDate, 4);
            cboYear.Text = "2020";


            cboYear.Enabled = false;

            cboJong.Items.Clear();
            cboJong.Items.Add("1.건강검진");
            cboJong.Items.Add("3.구강검진");
            cboJong.Items.Add("4.공단암");
            cboJong.Items.Add("E.의료급여");
            cboJong.SelectedIndex = 0;

            btnErrCheck.Enabled = false;
            btn_DiskMake.Enabled = false;

            dtpFDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-3).ToShortDateString();
            dtpTDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(2).ToShortDateString();

            txtSabun.Text = "";
            txtMirNo.Text = "";

            Fstr생활습관상담유무 = "OK";
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
                string strLtdCode = "";

                if (txtLtdCode.Text.IndexOf(".") > 0)
                {
                    strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 2);
                }
                else
                {
                    strLtdCode = txtLtdCode.Text;
                }

                FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
                FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                FrmHcLtdHelp.ShowDialog();
                FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                string strJong = "";
                string strMirGbn = "";

                tabBldControl.SelectedTab = tabBld1;
                strJong = VB.Left(cboJong.Text, 1);
                btn_DiskMake.Enabled = false;
                btnErrCheck.Enabled = false;

                sp.Spread_All_Clear(SS1);
                SS1_Sheet1.Rows[-1].Height = 24;
                strMirGbn = "N";
                if (rdoMirGbn1.Checked == true)
                {
                    strMirGbn = "Y";    //추가청구
                }

                //해당 명단을 SELECT
                List<COMHPC> list = comHpcLibBService.GetMirItembyYearMirGbn(cboYear.Text, strMirGbn, strJong);

                nREAD = list.Count;
                nRow = 0;
                for (int i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    if (nRow > SS1.ActiveSheet.RowCount)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                    }

                    SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                    SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].MIRNO.To<string>();
                    switch (list[i].JOHAP)
                    {
                        case "J":
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "성인병";
                            break;
                        case "K":
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "사업장";
                            break;
                        case "G":
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "공무원";
                            break;
                        case "X":
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "의료급여";
                            break;
                        case "T":
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "통합";
                            break;
                        default:
                            break;
                    }

                    if (list[i].LIFE_GBN == "Y")
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.FromArgb(128, 255, 128);
                    }

                    SS1.ActiveSheet.Cells[nRow - 1, 3].Text = "";
                    SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].BUILDCNT.To<string>();
                    SS1.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].GBERRCHK;
                    SS1.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].JEPNO;
                    SS1.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].JEPDATE;
                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = list[i].FRDATE;
                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = list[i].TODATE;
                    SS1.ActiveSheet.Cells[nRow - 1, 10].Text = list[i].LTDCODE.To<string>();
                    if (list[i].GBJOHAP == "4")
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 10].BackColor = Color.FromArgb(255, 255, 0); //급여
                    }
                    SS1.ActiveSheet.Cells[nRow - 1, 11].Text = list[i].KIHO;
                    SS1.ActiveSheet.Cells[nRow - 1, 13].Text = list[i].BUILDSABUN.To<string>();
                    SS1.ActiveSheet.Cells[nRow - 1, 14].Text = list[i].MIRGBN;
                    SS1.ActiveSheet.Cells[nRow - 1, 15].Text = list[i].CHASU;
                    SS1.ActiveSheet.Cells[nRow - 1, 16].Text = string.Format("{0:###,###,##0}", list[i].TAMT);
                    if (list[i].TAMT == 0)
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 16].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    if (strJong == "4" || strJong == "E")
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 17].Text = list[i].BOTAMT.To<string>();
                    }
                }

                SS1.ActiveSheet.RowCount = nRow;

                btnErrCheck.Enabled = true;
                btnMenuErrSave.Enabled = false;
                if (rdoJob0.Checked == true)
                {
                    btnMenuErrSave.Enabled = true;
                }
            }
            else if (sender == btnMenuPrint1)   //명단인쇄
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "건강보험 청구대상자 명단";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("출력일시: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + VB.Space(25) + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

            }
            else if (sender == btnMenuPrint2)   //오류내역인쇄
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "건강보험 청구대상자 오류내역";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("회 사 명: " + FstrLtdName + VB.Space(20) + "출력일시: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + VB.Space(25) + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnMenuPrint3)   //청구명단인쇄
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "건강보험 청구대상자 상세내역";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("회 사 명: " + FstrLtdName + VB.Space(20) + "출력일시: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + VB.Space(25) + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS3, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnMenuSangdam)
            {
                if (btnMenuSangdam.Text == "생활습관(상담)포함")
                {
                    Fstr생활습관상담유무 = "NO";
                    btnMenuSangdam.Text = "생활습관(상담)미포함";
                }
                else
                {
                    Fstr생활습관상담유무 = "OK";
                    btnMenuSangdam.Text = "생활습관(상담)포함";
                }
            }
            else if (sender == btnMenuErrSave)
            {
                long nWRTNO = 0;
                int nReSimsaCnt = 0;
                string strList = "";
                long nMirno = 0;
                string strSName = "";
                string strSex = "";
                long nAge = 0;
                string strRemark = "";
                string strGubun = "";
                string strMirnoList = "";
                long nCnt = 0;
                long nCnt1 = 0;
                long nCnt2 = 0;

                int result = 0;

                if (MessageBox.Show("선택한 명단을 별도 보관 및 청구제외를 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                strGubun = VB.Left(cboJong.Text, 1);

                strList = ";";
                strMirnoList = ";";
                for (int i = 0; i < SS2.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (SS2.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        //청구번호
                        nMirno = SS2.ActiveSheet.Cells[i, 1].Text.To<long>();
                        if (strList.IndexOf(";" + nMirno.To<string>() + ";") == 0)
                        {

                        }
                        //접수번호
                        nWRTNO = SS2.ActiveSheet.Cells[i, 2].Text.To<long>();
                        if (strList.IndexOf(";" + nWRTNO.To<string>() + ";") == 0)
                        {
                            strList += nWRTNO.To<string>() + ";";
                            clsDB.setBeginTran(clsDB.DbCon);
                            for (int j = 0; j < SS2.ActiveSheet.NonEmptyRowCount; j++)
                            {
                                if (SS2.ActiveSheet.Cells[j, 2].Text.To<long>() == nWRTNO)
                                {
                                    nMirno = SS2.ActiveSheet.Cells[j, 1].Text.To<long>();
                                    strSName = SS2.ActiveSheet.Cells[j, 5].Text;
                                    strSex = VB.Pstr(SS2.ActiveSheet.Cells[j, 6].Text, "/", 2);
                                    nAge = VB.Pstr(SS2.ActiveSheet.Cells[j, 6].Text, "/", 1).To<long>();
                                    strRemark = SS2.ActiveSheet.Cells[j, 7].Text;

                                    if (hicMirErrorTongboService.GetCountbyWrtNoGubunRemark(nWRTNO, strGubun, strRemark) == 0)
                                    {
                                        result = hicMirErrorTongboService.Insert(nWRTNO, nMirno, strSName, strSex, nAge, strGubun, strRemark);

                                        if (result < 0)
                                        {
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            MessageBox.Show("HIC_MIR_ERROR_TONGBO Insert 중 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                    }
                                }
                            }

                            //접수마스타 청구형성 완료 Clear
                            result = hicJepsuService.UpdateMirNo0byWrtNo(nWRTNO, strGubun);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("HIC_JEPSU Update 중 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            //수납마스타에 청구형성 완료 Clear
                            result = hicSunapService.UpdateMirNo0byWrtNo(nWRTNO, strGubun);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("HIC_SUNAP Update 중 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            clsDB.setCommitTran(clsDB.DbCon);
                        }
                    }
                }

                for (int i = 2; i < VB.L(strMirnoList, ";") - 1; i++)
                {
                    nMirno = VB.Pstr(strMirnoList, ";", i).To<long>();

                    if (nMirno > 0)
                    {
                        clsDB.setBeginTran(clsDB.DbCon);

                        HIC_JEPSU list2 = hicJepsuService.GetCountChcasubyMirNo(nMirno, strGubun);

                        if (!list2.IsNullOrEmpty())
                        {
                            nCnt = list2.CNT;
                            nCnt1 = list2.CNT1;
                            nCnt2 = list2.CNT2;
                        }

                        result = comHpcLibBService.UpdateMirBuildCntbyMirNo(nMirno, nCnt, nCnt1, strGubun);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            if (strGubun == "1")
                            {
                                MessageBox.Show("HIC_MIR_BOHUM에 UPDATE시 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else if (strGubun == "3")
                            {
                                MessageBox.Show("HIC_MIR_DENTAL에 UPDATE시 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else if (strGubun == "4")
                            {
                                MessageBox.Show("HIC_MIR_CANCER에 UPDATE시 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                MessageBox.Show("HIC_MIR_CANCER에 UPDATE시 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            return;
                        }
                        clsDB.setCommitTran(clsDB.DbCon);
                    }
                }

                //------------( 재판정 의뢰 )-------------------
                nReSimsaCnt = 0;
                for (int i = 0; i < SS3.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strRemark = SS3.ActiveSheet.Cells[i, 14].Text;
                    if (!strRemark.IsNullOrEmpty())
                    {
                        nReSimsaCnt += 1;
                        //청구번호
                        nMirno = SS3.ActiveSheet.Cells[i, 0].Text.To<long>();
                        nWRTNO = SS3.ActiveSheet.Cells[i, 20].Text.To<long>();

                        HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(nWRTNO);

                        if (!list.IsNullOrEmpty())
                        {
                            strSName = list.SNAME;
                            strSex = list.SEX;
                            nAge = list.AGE;
                        }

                        strRemark = "재판정사유: " + strRemark;

                        clsDB.setBeginTran(clsDB.DbCon);

                        result = hicMirErrorTongboService.Insert(nWRTNO, nMirno, strSName, strSex, nAge, strGubun, strRemark);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_MIR_ERROR_TONGBO INSERT시 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        //접수마스타 청구형성 완료 Clear
                        result = hicJepsuService.UpdateMirNo0byWrtNo(nWRTNO, strGubun);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_JEPSU 청구번호 UPDATE시 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        //수납마스타에 청구형성 완료 Clear
                        result = hicSunapService.UpdateMirNo0byWrtNo(nWRTNO, strGubun);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_SUNAP 청구번호 UPDATE시 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        HIC_JEPSU list2 = hicJepsuService.GetCountChcasubyMirNo(nMirno, strGubun);

                        if (!list2.IsNullOrEmpty())
                        {
                            nCnt = list.CNT;
                            nCnt1 = list.CNT1;
                            nCnt2 = list.CNT2;

                            result = comHpcLibBService.UpdateMirBuildCntbyMirNo(nMirno, nCnt, nCnt1, strGubun);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                if (strGubun == "1")
                                {
                                    MessageBox.Show("HIC_MIR_BOHUM에 UPDATE시 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else if (strGubun == "3")
                                {
                                    MessageBox.Show("HIC_MIR_DENTAL에 UPDATE시 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else if (strGubun == "4")
                                {
                                    MessageBox.Show("HIC_MIR_CANCER에 UPDATE시 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    MessageBox.Show("HIC_MIR_CANCER에 UPDATE시 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                return;
                            }
                            clsDB.setCommitTran(clsDB.DbCon);
                        }
                    }
                }

                if (nReSimsaCnt > 0)
                {
                    MessageBox.Show("재판정 의뢰한 청구번호는 다시 사전점검을 해야 됩니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("저장 완료!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                sp.Spread_All_Clear(SS2);
                sp.Spread_All_Clear(SS3);

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnErrCheck)
            {
                long nMirno = 0;
                string strChk = "";
                string strFrDate = "";
                string strToDate = "";
                string strMirGbn = "";
                int result = 0;

                FnRow = 0;
                sp.Spread_All_Clear(SS2);
                SS2_Sheet1.Rows[-1].Height = 24;
                txtGongWon.Text = "";
                txtBoWon.Text = "";

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    nMirno = SS1.ActiveSheet.Cells[i, 1].Text.To<long>();
                    if (SS1.ActiveSheet.Cells[i, 2].Text.Trim() == "사업장")
                    {
                        FstrJik = "K";
                    }
                    else if (SS1.ActiveSheet.Cells[i, 2].Text.Trim() == "공무원")
                    {
                        FstrJik = "G";
                    }
                    else if (SS1.ActiveSheet.Cells[i, 2].Text.Trim() == "성인병")
                    {
                        FstrJik = "J";
                    }
                    else if (SS1.ActiveSheet.Cells[i, 2].Text.Trim() == "의료급여")
                    {
                        FstrJik = "X";
                    }

                    strFrDate = SS1.ActiveSheet.Cells[i, 8].Text;
                    strToDate = SS1.ActiveSheet.Cells[i, 9].Text;
                    strMirGbn = SS1.ActiveSheet.Cells[i, 14].Text;
                    FstrFrDate = strFrDate;

                    if (strChk == "True")
                    {
                        //=======================================================================================================
                        //GoSub CmdErrCheck_SUB
                        //=======================================================================================================
                        FnErrCNT1 = 0;
                        FnErrCNT2 = 0;
                        FstrCOMMIT = "OK";

                        //기존에 등록된 오류 내역을 삭제
                        clsDB.setBeginTran(clsDB.DbCon);

                        result = comHpcLibBService.DeleteHicMirErrorbyMirNo(nMirno);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_MIR_ERROR 삭제중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);

                        if (VB.Left(cboJong.Text, 1) == "1")    //건강검진
                        {
                            fn_SET_BillAmt_Bohum(cboYear.Text, strFrDate);
                            //건강검진 1차 오류 점검
                            chb.READ_HIC_MIR_TBOHUM(nMirno);
                            fn_Mir_BohumBill_Clear();
                            fn_Check_Bohum1(nMirno, strFrDate, strToDate);
                            FstrCOMMIT = "OK";
                            fn_Check_Bohum2(nMirno, strFrDate, strToDate);
                            //건강건진비 청구서 저장
                            fn_HIC_MIR_TBOHUM_Save();
                            //건진일, 판정일 , 통보일 체크
                            fn_Check_Date(nMirno, strFrDate, strToDate);
                        }
                        else if (VB.Left(cboJong.Text, 1) == "3") //구강검진
                        {
                            fn_SET_BillAmt_Bohum(cboYear.Text, strFrDate);
                            chb.READ_HIC_MIR_DENTAL(nMirno);
                            fn_Mir_DentalBill_Clear();
                            fn_Check_Dental(nMirno, strFrDate, strToDate);
                            //구강 청구서 저장
                            fn_HIC_MIR_DENTAL_Save();
                        }
                        else if (VB.Left(cboJong.Text, 1) == "4")  //암검진,보건소암
                        {
                            fn_SET_BillAmt_Cancer(cboYear.Text, strFrDate);
                            chb.READ_HIC_MIR_CANCER(nMirno);
                            fn_Check_Cancer(nMirno, strFrDate, strToDate);
                            //암검진 점검 및 저장
                            fn_HIC_MIR_Cancer_Save();
                            //보건소암 점검 및 저장
                            chb.READ_HIC_MIR_CANCER_Bo(nMirno);
                            if (clsHcType.TMCB.MIRNO > 0)
                            {
                                fn_HIC_MIR_Cancer_Bo_Save();
                            }
                            //건진일, 판정일 , 통보일 체크
                            fn_Check_Date2(nMirno, strFrDate, strToDate);
                        }
                        else if (VB.Left(cboJong.Text, 1) == "E")  //의료급여암
                        {
                            fn_SET_BillAmt_Cancer(cboYear.Text, strFrDate);
                            chb.READ_HIC_MIR_CANCER(nMirno);
                            fn_Check_Cancer(nMirno, strFrDate, strToDate);
                            //암 청구서 저장
                            fn_HIC_MIR_Cancer_Save();
                            //건진일, 판정일 , 통보일 체크
                            fn_Check_Date2(nMirno, strFrDate, strToDate);
                        }
                        //=======================================================================================================
                        SS1.ActiveSheet.Cells[i, 0].Text = "True";
                    }
                }
                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnSearch2)
            {
                int nREAD = 0;
                int nRow = 0;
                string strJong = "";
                string strOK = "";
                string strMirGbn = "";
                string strYear = "";
                long nLtdCode = 0;
                string strSabun = "";
                long nMirNo = 0;
                string strJob = "";
                string strFDate = "";
                string strTDate = "";
                string strJepDate = "";

                sp.Spread_All_Clear(SS1);
                sp.Spread_All_Clear(SS3);

                tabBldControl.SelectedTab = tabBld1;
                strJong = VB.Left(cboJong.Text, 1);
                strYear = cboYear.Text;
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                strSabun = txtSabun.Text.Trim();
                nMirNo = txtMirNo.Text.Trim().To<long>();
                if (rdoJob0.Checked == true)
                {
                    strJob = "0";
                }
                else
                {
                    strJob = "1";
                }

                strFDate = dtpFDate.Text;
                strTDate = dtpTDate.Text;

                strJepDate = cboYear.Text + "-01-01";

                btn_DiskMake.Enabled = false;
                if (clsType.User.IdNumber == "25420")
                {
                    btnErrCheck.Enabled = true;
                }

                strMirGbn = "N";
                if (rdoMirGbn1.Checked == true)
                {
                    strMirGbn = "Y";
                }

                List<COMHPC> list = comHpcLibBService.GetItembyYearLtdCodeSabunMirNoJepDate(strJong, strYear, nLtdCode, strSabun, nMirNo, strJob, strFDate, strTDate, strJepDate, strMirGbn);

                nREAD = list.Count;
                nRow = 0;
                for (int i = 0; i < nREAD; i++)
                {
                    strOK = "OK";
                    nRow += 1;
                    if (nRow > SS1.ActiveSheet.RowCount)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                    }

                    SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                    SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].MIRNO.To<string>();
                    switch (list[i].JOHAP)
                    {
                        case "J":
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "성인병";
                            break;
                        case "K":
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "사업장";
                            break;
                        case "G":
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "공무원";
                            break;
                        case "X":
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "의료급여";
                            break;
                        case "T":
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "통합";
                            break;
                        default:
                            break;
                    }

                    if (list[i].LIFE_GBN == "Y")
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.FromArgb(128, 255, 128);
                    }
                    SS1.ActiveSheet.Cells[nRow - 1, 3].Text = "";
                    SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].JEPQTY.To<string>();
                    SS1.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].GBERRCHK;
                    SS1.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].JEPNO;
                    SS1.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].JEPDATE;
                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = list[i].FRDATE;
                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = list[i].TODATE;
                    if (list[i].GBJOHAP == "4")
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 10].BackColor = Color.FromArgb(255, 0, 0);  //급여
                    }
                    SS1.ActiveSheet.Cells[nRow - 1, 11].Text = list[i].KIHO;
                    SS1.ActiveSheet.Cells[nRow - 1, 12].Text = list[i].FILENAME;
                    SS1.ActiveSheet.Cells[nRow - 1, 13].Text = list[i].BUILDSABUN.To<string>();
                    SS1.ActiveSheet.Cells[nRow - 1, 14].Text = list[i].MIRGBN;
                    SS1.ActiveSheet.Cells[nRow - 1, 16].Text = string.Format("{0:###,###,##0}", list[i].TAMT);
                    if (list[i].TAMT == 0)
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 16].BackColor = Color.FromArgb(255, 0, 0);
                    }

                    if (SS1.ActiveSheet.Cells[nRow - 1, 16].Text.To<long>() == 0)
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 16].Text = "";
                        SS1.ActiveSheet.Cells[nRow - 1, 16].Locked = true;
                    }

                    //보건소암 금액 체크
                    //2015-11-24 보건소암 건수 및 금액이 없는 경우도 있어서 점검 안함
                    if (strJong == "4")
                    {
                        chb.READ_HIC_MIR_CANCER_Bo(list[i].MIRNO);
                    }

                    if (strJong == "4" || strJong == "E")
                    {
                        if (hicMirCancerBoService.GetTamtbyMirNo(list[i].MIRNO) > 0)
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 17].Text = string.Format("{0:###,###,##0}", list[i].TAMT);
                        }
                    }
                    SS1.ActiveSheet.Cells[nRow - 1, 18].Text = list[i].NHICNO;
                }

                SS1.ActiveSheet.RowCount = nRow;

                if (rdoJob0.Checked == true)
                {
                    btn_DiskMake.Enabled = true;
                }
                btnMenuErrSave.Enabled = false;
            }
            else if (sender == btn_DiskMake)
            {
                long nMirno = 0;
                string strChk = "";
                string strFrDate = "";
                string strToDate = "";
                string strkiho = "";
                string strJohap = "";
                string strJong = "";
                string strLtd = "";
                string strChungDate = "";
                string strChungDate1 = "";
                int nCnt = 0;
                int nREAD = 0;
                int nRead1 = 0;
                int nRead2 = 0;

                string strDir = "";
                string strMyPath1 = "";
                string strMyName = "";
                string strPathName = "";

                string strChasu = "";   //2차 단독 청구시 Y 마크
                string strGubun = "";
                string strMirGbn = "";  //추가청구
                string strJepNo = "";
                string strJepNo1 = "";

                COMHPC list = new COMHPC();

                ComFunc.ReadSysDate(clsDB.DbCon);

                strJepNo = VB.Mid(clsPublic.GstrSysDate, 3, 2) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Right(clsPublic.GstrSysDate, 2);
                strJepNo1 = VB.Mid(clsPublic.GstrSysDate, 1, 4) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Right(clsPublic.GstrSysDate, 2);

                //GnDentAmt에 해당년도 구강검진 금액을 설정
                hb.SET_Dental_Amt(cboYear.Text);

                nCnt = 0;
                strJepNo = strJepNo + "%";
                if (VB.Left(cboJong.Text, 1) == "1")
                {
                    list = comHpcLibBService.GetJepNobyYear("1", cboYear.Text, strJepNo);
                }
                else if (VB.Left(cboJong.Text, 1) == "3")
                {
                    list = comHpcLibBService.GetJepNobyYear("3", cboYear.Text, strJepNo);
                }
                else if (VB.Left(cboJong.Text, 1) == "4" | VB.Left(cboJong.Text, 1) == "E")
                {
                    list = comHpcLibBService.GetJepNobyYear("4", cboYear.Text, strJepNo);
                }

                if (!list.IsNullOrEmpty())
                {
                    nCnt = VB.Right(list.JEPNO, 2).To<int>();

                    if (nCnt >= 99)
                    {
                        MessageBox.Show("해당일에 청구번호가 99번 이상입니다.." + "\r\n" + "대단히 수고하셨습니다..청구작업은 다음날 하세요~", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("자료가 없습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                strJong = VB.Left(cboJong.Text, 1);
                strkiho = "37100068";

                for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    nMirno = SS1.ActiveSheet.Cells[i, 1].Text.To<long>();
                    strJohap = SS1.ActiveSheet.Cells[i, 2].Text;
                    strChungDate = strJepNo;
                    strChungDate1 = strJepNo1;
                    strFrDate = SS1.ActiveSheet.Cells[i, 8].Text;
                    strToDate = SS1.ActiveSheet.Cells[i, 9].Text;
                    strLtd = SS1.ActiveSheet.Cells[i, 10].Text;
                    strMirGbn = SS1.ActiveSheet.Cells[i, 14].Text;
                    strChasu = SS1.ActiveSheet.Cells[i, 15].Text;

                    switch (strJohap)
                    {
                        case "성인병":
                            if (strLtd == "급여")
                            {
                                strJohap = "X";
                            }
                            else
                            {
                                strJohap = "T";
                            }
                            break;
                        case "사업장":
                            strJohap = "T";
                            break;
                        case "공무원":
                            strJohap = "T";
                            break;
                        case "의료급여":
                            strJohap = "X";
                            break;
                        case "통합":
                            strJohap = "T";
                            break;
                        default:
                            break;
                    }

                    if (strChk == "True")
                    {
                        //폴더생성
                        strMyPath1 = @"C:\청구작업\";

                        DirectoryInfo Dir = new DirectoryInfo(strMyPath1);
                        if (Dir.Exists == false)
                        {
                            Dir.Create();

                            if (VB.Left(cboJong.Text, 1) == "1")
                            {
                                strDir = @"C:\청구작업\건강보험\";
                            }
                            else if (VB.Left(cboJong.Text, 1) == "3")
                            {
                                strDir = @"C:\청구작업\구강검진\";
                            }
                            else if (VB.Left(cboJong.Text, 1) == "4")
                            {
                                strDir = @"C:\청구작업\암검진\";
                            }
                            else if (VB.Left(cboJong.Text, 1) == "E")
                            {
                                strDir = @"C:\청구작업\의료급여\";
                            }

                            DirectoryInfo Dir1 = new DirectoryInfo(strDir);
                            if (Dir1.Exists == false)
                            {
                                Dir1.Create();
                            }
                        }
                        else
                        {
                            if (VB.Left(cboJong.Text, 1) == "1")
                            {
                                strDir = @"C:\청구작업\건강보험\";
                            }
                            else if (VB.Left(cboJong.Text, 1) == "3")
                            {
                                strDir = @"C:\청구작업\구강검진\";
                            }
                            else if (VB.Left(cboJong.Text, 1) == "4")
                            {
                                strDir = @"C:\청구작업\암검진\";
                            }
                            else if (VB.Left(cboJong.Text, 1) == "E")
                            {
                                strDir = @"C:\청구작업\의료급여\";
                            }

                            DirectoryInfo Dir1 = new DirectoryInfo(strDir);
                            if (Dir1.Exists == false)
                            {
                                Dir1.Create();
                            }
                        }

                        //1.건강보험청구
                        if (VB.Left(cboJong.Text, 1) == "1")
                        {
                            if (chkLiverC.Checked == true)
                            {
                                fn_Make_File5(nMirno, strJohap, strPathName, strChungDate, nCnt, strLtd, strMirGbn);
                            }
                            else
                            {
                                fn_Make_File1(nMirno, strJohap, strPathName, strChungDate, nCnt, strLtd, strMirGbn);
                            }

                            if (strChasu == "Y")
                            {
                                strChasu = "12";
                            }
                            else
                            {
                                strChasu = "11";
                            }
                            strGubun = "01";
                            if (strMirGbn == "Y")
                            {
                                strGubun = "B1";
                            }
                        }

                        //2.구강청구
                        if (VB.Left(cboJong.Text, 1) == "3")
                        {
                            fn_Make_File2(nMirno, strJohap, strPathName, strChungDate, nCnt, strLtd, strMirGbn);
                            if (strChasu == "Y")
                            {
                                strChasu = "20";
                            }
                            else
                            {
                                strChasu = "20";
                            }
                            strGubun = "06";
                            if (strMirGbn == "Y")
                            {
                                strGubun = "B6";
                            }
                        }
                        //4.암검진 , E.의료급여암 청구
                        if (VB.Left(cboJong.Text, 1) == "4" || VB.Left(cboJong.Text, 1) == "E")
                        {
                            fn_Make_File3(nMirno, strJohap, strPathName, strChungDate, nCnt, strMirGbn);

                            strChasu = "30";
                            strGubun = "04";

                            if (strJohap == "X")
                            {
                                strGubun = "05";
                            }

                            if (strMirGbn == "Y")
                            {
                                strGubun = "B4";
                                if (strJohap == "X")
                                {
                                    strGubun = "B5";
                                }
                            }
                        }

                        MessageBox.Show(strPathName + strkiho + "_" + VB.Right(cboYear.Text, 2) + "01" + strJohap + "_" + strGubun + strChasu + "_" +
                                       string.Format("{0:000000}", strChungDate) + "_" + string.Format("{0:00}", nCnt) + " 파일생성완료", "작업완료", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        SS1.ActiveSheet.Cells[i, 0].Text = "";
                    }

                    eBtnClick(btnSearch2, new EventArgs());
                }
            }
            else if (sender == btnDel)
            {
                string strChk = "";
                long nMirNo = 0;
                string sMsg = "";
                int result = 0;

                sMsg = "청구 작업을 정말로 취소하시겠습니까?";
                if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);
                for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    nMirNo = SS1.ActiveSheet.Cells[i, 1].Text.To<long>();
                    if (strChk == "True")
                    {
                        result = comHpcLibBService.UpdateMirJepDatebyMirNo(nMirNo, VB.Left(cboJong.Text, 1));

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    SS1.ActiveSheet.Cells[i, 0].Text = "";
                }
                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnSearch2, new EventArgs());
            }
            else if (sender == btnCDel)
            {
                string strChk = "";
                long nMirno = 0;
                long nMisuNo = 0;

                string sMsg = "";
                int result = 0;

                sMsg = "해당 청구 작업이 삭제됩니다.. 청구 작업을 정말로 삭제하시겠습니까?";
                if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);
                for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    nMirno = SS1.ActiveSheet.Cells[i, 1].Text.To<long>();
                    if (strChk == "True")
                    {
                        switch (VB.Left(cboJong.Text, 1))
                        {
                            case "1":   //사업장
                                List<HIC_JEPSU> list = hicJepsuService.GetMisuno2byMirNo(nMirno, "2");
                                if (list.Count > 0)
                                {
                                    for (int j = 0; j < list.Count; j++)
                                    {
                                        if (list[j].MISUNO2 > 0)
                                        {
                                            sMsg = "미수번호 : " + list[i].MISUNO2 + "\r\n" + "\r\n";
                                            sMsg += "이미 미수번호가 존재합니다. 미수번호 삭제후 청구삭제를 작업을 하십시오.";
                                            MessageBox.Show(sMsg, "청구삭제 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            return;
                                        }
                                    }
                                }

                                result = hicMirBohumService.UpdatebyMirNo(nMirno);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("HIC_MIR_BOHUM UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                //접수및 수납 테이블 업데이트
                                result = hicJepsuService.UpdatebyMirNo1(nMirno);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("HIC_MIR_BOHUM UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                result = hicSunapService.UpdatebyMirNo1(nMirno);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("HIC_MIR_BOHUM UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                break;
                            case "3": //구강
                                List<HIC_JEPSU> list2 = hicJepsuService.GetMisuno2byMirNo(nMirno, "3");
                                if (list2.Count > 0)
                                {
                                    for (int j = 0; j < list2.Count; j++)
                                    {
                                        if (list2[j].MISUNO3 > 0)
                                        {
                                            sMsg = "미수번호 : " + list2[j].MISUNO3 + "\r\n" + "\r\n";
                                            sMsg += "이미 미수번호가 존재합니다. 미수번호 삭제후 청구삭제를 작업을 하십시오.";
                                            MessageBox.Show(sMsg, "청구삭제 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            return;
                                        }
                                    }
                                }

                                result = hicMirDentalService.UpdateMirNoOldMirNobyMirNo(nMirno);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("HIC_MIR_DENTAL UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                //접수및 수납 테이블 업데이트
                                result = hicJepsuService.UpdateMirno2byMirNo2(nMirno);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("HIC_JEPSU UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                result = hicSunapService.UpdateMirno2byMirNo2(nMirno);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("HIC_JEPSU UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                break;
                            case "4":
                            case "E":
                                if (VB.Left(cboJong.Text, 1) == "4")
                                {
                                    List<HIC_JEPSU> list3 = hicJepsuService.GetMisuNo2MisuNo4byMirNo3(nMirno);
                                    if (list3.Count > 0)
                                    {
                                        for (int j = 0; j < list3.Count; j++)
                                        {
                                            if (list3[j].MISUNO2 > 0 || list3[j].MISUNO4 > 0)
                                            {
                                                if (list3[j].MISUNO2 > 0)
                                                {
                                                    nMisuNo = list3[j].MISUNO2;
                                                }
                                                else
                                                {
                                                    nMisuNo = list3[j].MISUNO4;
                                                }
                                                sMsg = "이미 미수번호가 존재합니다. 미수번호 삭제후 청구삭제를 작업을 하십시오.";
                                                sMsg += "미수번호 : " + nMisuNo;
                                                MessageBox.Show(sMsg, "청구삭제 불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                clsDB.setRollbackTran(clsDB.DbCon);
                                                return;
                                            }
                                        }
                                    }
                                }
                                else if (VB.Left(cboJong.Text, 1) == "E")
                                {
                                    List<HIC_JEPSU> list4 = hicJepsuService.GetMisuNo4byMirNo5(nMirno);
                                    if (list4.Count > 0)
                                    {
                                        for (int j = 0; j < list4.Count; j++)
                                        {
                                            if (list4[j].MISUNO4 > 0)
                                            {
                                                nMisuNo = list4[j].MISUNO4;
                                                sMsg += "미수번호 : " + nMisuNo + "\r\n" + "\r\n";
                                                sMsg = "이미 미수번호가 존재합니다. 미수번호 삭제후 청구삭제를 작업을 하십시오.";
                                                MessageBox.Show(sMsg, "청구삭제 불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                clsDB.setRollbackTran(clsDB.DbCon);
                                                return;
                                            }
                                        }
                                    }
                                }

                                result = hicMirCancerService.UpdateMirNoOldMirNobyMirNo(nMirno);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("HIC_MIR_CANCER UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                if (VB.Left(cboJong.Text, 1) == "4")
                                {
                                    //보건소 청구 초기화
                                    result = hicMirCancerBoService.UpdateMirNoOldMirNobyMirNo(nMirno);

                                    if (result < 0)
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        MessageBox.Show("HIC_MIR_CANCER_BO UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    result = hicJepsuService.UpdateMirNo3byMirNo3(nMirno);

                                    if (result < 0)
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        MessageBox.Show("HIC_JEPSU UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    result = hicSunapService.UpdateMirNo3byMirNo3(nMirno);

                                    if (result < 0)
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        MessageBox.Show("HIC_SUNAP UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                                else if (VB.Left(cboJong.Text, 1) == "E")
                                {
                                    //접수및 수납 테이블 업데이트
                                    result = hicJepsuService.UpdateMirNo5byMirNo5(nMirno);

                                    if (result < 0)
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        MessageBox.Show("HIC_JEPSU UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    result = hicSunapService.UpdateMirNo5byMirNo5(nMirno);

                                    if (result < 0)
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        MessageBox.Show("HIC_SUNAP UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }

                                break;
                            default:
                                break;
                        }
                    }
                    SS1.ActiveSheet.Cells[i, 0].Text = "";
                }

                clsDB.setCommitTran(clsDB.DbCon);
                eBtnClick(btnSearch2, new EventArgs());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argFrDate"></param>
        /// <param name="argToDate"></param>
        void fn_Check_Cancer(long argMirno, string argFrDate, string argToDate)
        {
            int nREAD = 0;
            long nWRTNO = 0;
            long nPano = 0;
            string strGbAm = "";
            string strJepDate = "";
            string strGbChul = "";
            bool bGonghyu = false;
            int nGonghyuCnt = 0;
            string strJong = "";

            FnMirNo = argMirno; //청구번호
            strJong = VB.Left(cboJong.Text, 1);

            clsDB.setBeginTran(clsDB.DbCon);

            //암검진 청구오류 점검
            List<HIC_JEPSU_CANCER_NEW> list = hicJepsuCancerNewService.GetItembyJepDateMirNo2(argFrDate, argToDate, argMirno, strJong);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                nWRTNO = list[i].WRTNO;
                FstrSname = list[i].SNAME;
                FstrSex = list[i].SEX;
                FnAge = list[i].AGE;
                strGbAm = list[i].GBAM;
                strJepDate = list[i].JEPDATE;
                strGbChul = list[i].GBCHUL;

                //암검진
                //전체 결과등록 체크
                if (chkResult.Checked == false)
                {
                    fn_Cancer_Result_chk(nWRTNO);
                }
                //검진결과,문진표를 읽음
                chb.READ_HIC_CANCER_NEW(nWRTNO);

                if (clsHcType.B3.ROWID.IsNullOrEmpty())
                {
                    FstrCOMMIT = "NO";
                    break;
                }

                //GoSub Check_Cancer_ERROR_Check //오류점검
                //=================================================================================================
                int nCntB = 0;
                int nCntR = 0;
                string strSangdamCnt = "";

                strSangdamCnt = "";

                if (clsHcType.B3.TongboDate.IsNullOrEmpty())
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "통보일자가 공란입니다.");
                }

                if (string.Compare(clsHcType.B3.TongboGbn, "1") < 0 || string.Compare(clsHcType.B3.TongboGbn, "3") > 0)
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "통보방법이 오류입니다.");
                }

                //위암
                if (VB.Mid(clsHcType.B3.Can_MirGbn, 1, 1) != "1")   //판정에서 위암제외아닐경우
                {
                    if (string.Compare(clsHcType.B3.GBSTOMACH, "0") < 0 || string.Compare(clsHcType.B3.GBSTOMACH, "3") > 0)
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "청구종목 위암 오류입니다.");
                    }
                    if (string.Compare(clsHcType.B3.GBSTOMACH, "0") > 0)
                    {
                        if (clsHcType.B3.GBSTOMACH == "1")
                        {
                            if (VB.Pstr(strGbAm, ",", 1) != "1" && VB.Pstr(strGbAm, ",", 2) != "1")
                            {
                                fn_ERROR_INSERT(nWRTNO, "2", "접수 위암을 선택 안함.");
                            }
                            if (clsHcType.B3.S_ANATGBN == "1" && clsHcType.B3.Stomach_SENDO.IsNullOrEmpty())
                            {
                                fn_ERROR_INSERT(nWRTNO, "2", "위암병형 오류입니다.");
                            }
                            else if (clsHcType.B3.Stomach_S == "1" && clsHcType.B3.Stomach_S == "")
                            {
                                fn_ERROR_INSERT(nWRTNO, "2", "위암병형 오류입니다.");
                            }
                        }

                        if (string.Compare(clsHcType.B3.S_ANATGBN, "1") < 0 || string.Compare(clsHcType.B3.S_ANATGBN, "2") > 0 && !clsHcType.B3.S_ANATGBN.IsNullOrEmpty())
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "위암 조직검사실시여부 오류입니다.");
                        }
                        if (string.Compare(clsHcType.B3.S_ANAT, "1") < 0 || string.Compare(clsHcType.B3.S_ANAT, "7") > 0 && !clsHcType.B3.S_ANAT.IsNullOrEmpty())
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "위암 조직검사결과 오류입니다.");
                        }
                        if (string.Compare(clsHcType.B3.S_PANJENG, "1") < 0 || string.Compare(clsHcType.B3.S_PANJENG, "6") > 0 && clsHcType.B3.S_PANJENG.IsNullOrEmpty())
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "위암 종합판정  오류입니다.");
                        }
                        if (clsHcType.B3.S_PLACE != "0" && clsHcType.B3.S_PLACE != "1" && clsHcType.B3.S_PLACE != "2")
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "위암 검진장소구분 오류입니다.");
                        }
                        if (clsHcType.B3.RESULT0001.IsNullOrEmpty() && string.Compare(clsHcType.B3.NEW_SICK54, "0") > 0)
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "위암 조직검사를 하였으나 판정에 누락됨");
                        }
                    }
                }

                //대장암
                if (VB.Mid(clsHcType.B3.Can_MirGbn, 2, 1) != "1")   //판정에서 대장암제외아닐경우
                {
                    if (string.Compare(clsHcType.B3.GBRECTUM, "0") < 0 || string.Compare(clsHcType.B3.GBRECTUM, "3") > 0)
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "청구종목 대장암 오류입니다.");
                    }
                    if (string.Compare(clsHcType.B3.GBRECTUM, "0") > 0)
                    {
                        if (VB.Pstr(strGbAm, ",", 3) != "1")
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "접수에 대장암 선택 안함.");
                        }
                        if (VB.Left(cboJong.Text, 1) == "4")
                        {
                            if (clsHcType.B3.COLON_RESULT.IsNullOrEmpty())
                            {
                                fn_ERROR_INSERT(nWRTNO, "2", "대장암 분변잠혈결과 공란입니다.");
                            }
                        }
                        if ((clsHcType.B3.COLON_RESULT != "1" && clsHcType.B3.COLON_RESULT != "2") && !clsHcType.B3.COLON_RESULT.IsNullOrEmpty())
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "대장암 RPHA,EIA 오류입니다.");
                        }
                        if ((string.Compare(clsHcType.B3.C_ANATGBN, "0") < 0 || string.Compare(clsHcType.B3.C_ANATGBN, "2") > 0) && !clsHcType.B3.C_ANATGBN.IsNullOrEmpty())
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "대장암 조직검사실시여부 병형 오류입니다.");
                        }
                        if ((string.Compare(clsHcType.B3.C_ANAT, "1") < 0 || string.Compare(clsHcType.B3.C_ANAT, "6") > 0) && !clsHcType.B3.C_ANAT.IsNullOrEmpty())
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "대장암 조직검사결과  오류입니다.");
                        }
                        if ((string.Compare(clsHcType.B3.C_PANJENG, "1") < 0 || string.Compare(clsHcType.B3.C_PANJENG, "7") > 0) && !clsHcType.B3.C_PANJENG.IsNullOrEmpty())
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "대장암 종합판정  오류입니다.");
                        }
                        if (clsHcType.B3.C_PLACE != "0" && clsHcType.B3.C_PLACE != "1" && clsHcType.B3.C_PLACE != "2")
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "대장암 검진장소구분 오류입니다.");
                        }
                    }
                }

                //간암
                if (VB.Mid(clsHcType.B3.Can_MirGbn, 3, 1) != "1") //판정에서 간암제외아닐경우
                {
                    if (string.Compare(clsHcType.B3.GbLiver, "0") < 0 || string.Compare(clsHcType.B3.GbLiver, "3") > 0)
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "청구종목 간암 오류입니다.");
                    }
                    if (string.Compare(clsHcType.B3.GbLiver, "0") > 0)
                    {
                        if (VB.Pstr(strGbAm, ",", 4) != "1")
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "접수에 간암 선택 안함.");
                        }
                        if ((string.Compare(clsHcType.B3.Liver_RPHA, "1") < 0 || string.Compare(clsHcType.B3.Liver_RPHA, "6") > 0) && !clsHcType.B3.Liver_RPHA.IsNullOrEmpty())
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "간암 RPHA 오류입니다.");
                        }
                        if (string.Compare(clsHcType.B3.Liver_EIA, "99") > 0 && !clsHcType.B3.Liver_RPHA.IsNullOrEmpty())
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "간암 EIA 오류입니다.");
                        }
                        if ((string.Compare(clsHcType.B3.Liver_PANJENG, "1") < 0 || string.Compare(clsHcType.B3.Liver_PANJENG, "8") > 0) && !clsHcType.B3.Liver_PANJENG.IsNullOrEmpty())
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "간암 종합판정  오류입니다.");
                        }
                        if (clsHcType.B3.Liver_PLACE != "0" && clsHcType.B3.Liver_PLACE != "1" && clsHcType.B3.Liver_PLACE != "2")
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "간암 검진장소구분 오류입니다.");
                        }
                    }
                }

                //유방암
                //판정에서 유방암제외아닐경우
                if (string.Compare(clsHcType.B3.GBBREAST, "0") < 0 || string.Compare(clsHcType.B3.GBBREAST, "3") > 0)
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "청구종목 유방암 오류입니다.");
                }
                if (string.Compare(clsHcType.B3.GBBREAST, "0") > 0)
                {
                    if (VB.Pstr(strGbAm, ",", 5) != "1")
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "접수에 유방암 선택 안함.");
                    }
                    if (clsHcType.B3.GBBREAST == "1" && clsHcType.B3.BREAST_S.IsNullOrEmpty())
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "유방암 병형 오류입니다.");
                    }
                    if ((string.Compare(clsHcType.B3.B_ANAT, "1") < 0 || string.Compare(clsHcType.B3.B_ANAT, "5") > 0) && !clsHcType.B3.B_ANAT.IsNullOrEmpty())
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "유방암 조직검사 결과  오류입니다.");
                    }
                    if ((string.Compare(clsHcType.B3.B_PANJENG, "1") < 0 || string.Compare(clsHcType.B3.B_PANJENG, "5") > 0) && !clsHcType.B3.B_PANJENG.IsNullOrEmpty())
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "유방암 종합판정  오류입니다.");
                    }
                    if (clsHcType.B3.B_PLACE != "0" && clsHcType.B3.B_PLACE != "1" && clsHcType.B3.B_PLACE != "2")
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "유방암 검진장소구분 오류입니다.");
                    }
                    if (clsHcType.B3.B_ANATGBN.IsNullOrEmpty())
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "유방암 실질분포량 공란 오류입니다.");
                    }
                }

                //자궁경부암
                if (VB.Mid(clsHcType.B3.Can_MirGbn, 5, 1) != "1")   //판정에서 자궁경부암제외아닐경우
                {
                    if (string.Compare(clsHcType.B3.GbWomb, "0") > 0)
                    {
                        if (VB.Pstr(strGbAm, ",", 6) != "1")
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "접수에 자궁경부암 선택 안함.");
                        }
                        if ((string.Compare(clsHcType.B3.WOMB09, "1") < 0 || string.Compare(clsHcType.B3.WOMB09, "6") > 0) && !clsHcType.B3.WOMB09.IsNullOrEmpty())
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "자궁경부암 종합판정  오류입니다.");
                        }
                        //자궁경부
                        if (VB.Left(cboJong.Text, 1) == "4")
                        {
                            if (clsHcType.B3.WOMB01.IsNullOrEmpty()) { fn_ERROR_INSERT(nWRTNO, "2", "자궁경부암 자궁질도말검사 검체상태  공란입니다."); }
                            if (clsHcType.B3.WOMB02.IsNullOrEmpty()) { fn_ERROR_INSERT(nWRTNO, "2", "자궁경부암 자궁질도말검사 선상피세포  공란입니다."); }
                            if (clsHcType.B3.WOMB03.IsNullOrEmpty()) { fn_ERROR_INSERT(nWRTNO, "2", "자궁경부암 자궁질도말검사 유형별진단  공란입니다."); }
                        }
                    }
                }

                if (VB.Mid(clsHcType.B3.Can_MirGbn, 6, 1) != "1")   //판정에서 자궁경부암제외아닐경우
                {
                    if (string.Compare(clsHcType.B3.GBLUNG, "0") > 0)
                    {
                        if (clsHcType.B3.LUNG_RESULT001.IsNullOrEmpty())
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "폐암 선량값이 공란입니다.");
                        }
                    }
                }

                //중복판정점검
                if (chb.READ_HIC_DuplicatejudgmentCheck("암", "1", nWRTNO) == true)
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "판정이 2건 발생함 .. 확인바람");
                }

                //공휴가산 점검
                bGonghyu = false;
                if (hb.HIC_Huil_GasanDay(strJepDate) == true) bGonghyu = true;
                if (strGbChul == "Y") bGonghyu = false;

                //(분변상담제외 접수시에는 공휴가산 제외)
                nGonghyuCnt = hicSunapdtlService.GetCountbyWrtNoCode(nWRTNO, "1119");

                //(상담제외 확인)
                if (hicSunapdtlService.GetCountbyWrtNoCode(nWRTNO, "3168") == 1)
                {
                    strSangdamCnt = "1";
                }

                if (strSangdamCnt != "1")
                {
                    if (bGonghyu == true && nGonghyuCnt == 0)
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "암검진 공휴 가산 코드 누락");
                    }
                    else if (bGonghyu == false && nGonghyuCnt > 0)
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "암검진 공휴 가산 코드 오류 산정");
                    }
                }
                //=================================================================================================

                if (FstrCOMMIT == "NO")
                {
                    break;
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void fn_Cancer_Result_chk(long argWRTNO)
        {
            string strTemp = "";
            List<string> strExCodes = new List<string>();

            strExCodes.Clear();
            strExCodes.Add("A161");
            strExCodes.Add("A162");
            strExCodes.Add("A166");
            strExCodes.Add("A167");
            strExCodes.Add("A168");
            strExCodes.Add("TY05");
            strExCodes.Add("A170");
            strExCodes.Add("A172");
            strExCodes.Add("A173");

            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetCodeNamebyWrtNoNotInExCode(argWRTNO, strExCodes);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strTemp = list[i].CODE + " " + list[i].HNAME;
                    fn_ERROR_INSERT(argWRTNO, "2", strTemp + " 결과 미등록 오류");
                }
            }
        }

        void fn_HIC_MIR_TBOHUM_Save()
        {
            string strErrChk = "";
            int result = 0;

            strErrChk = "";

            if (hicMirErrorTongboService.GetCountbyMirNo(FnMirNo) > 0)
            {
                strErrChk = "N";
            }
            else
            {
                strErrChk = "Y";
            }

            clsDB.setBeginTran(clsDB.DbCon);

            if (!clsHcType.TMB.ROWID.IsNullOrEmpty())
            {
                HIC_MIR_BOHUM item = new HIC_MIR_BOHUM();

                item.ONE_INWON011 = clsHcType.TMB.ONE_Inwon[1]; //상담
                item.ONE_INWON012 = clsHcType.TMB.ONE_Inwon[2];
                item.ONE_INWON021 = clsHcType.TMB.ONE_Inwon[3];
                item.ONE_INWON022 = clsHcType.TMB.ONE_Inwon[4];
                item.ONE_INWON031 = clsHcType.TMB.ONE_Inwon[5];
                item.ONE_INWON032 = clsHcType.TMB.ONE_Inwon[6];
                item.ONE_INWON041 = clsHcType.TMB.ONE_Inwon[7];
                item.ONE_INWON042 = clsHcType.TMB.ONE_Inwon[8];
                item.ONE_INWON051 = clsHcType.TMB.ONE_Inwon[9];
                item.ONE_INWON052 = clsHcType.TMB.ONE_Inwon[10];
                item.ONE_INWON061 = clsHcType.TMB.ONE_Inwon[11];
                item.ONE_INWON062 = clsHcType.TMB.ONE_Inwon[12];
                item.ONE_INWON071 = clsHcType.TMB.ONE_Inwon[13];
                item.ONE_INWON072 = clsHcType.TMB.ONE_Inwon[14];
                item.ONE_INWON081 = clsHcType.TMB.ONE_Inwon[15];
                item.ONE_INWON082 = clsHcType.TMB.ONE_Inwon[16];
                item.ONE_INWON091 = clsHcType.TMB.ONE_Inwon[17];
                item.ONE_INWON092 = clsHcType.TMB.ONE_Inwon[18];
                item.ONE_INWON101 = clsHcType.TMB.ONE_Inwon[19];
                item.ONE_INWON102 = clsHcType.TMB.ONE_Inwon[20];
                item.ONE_INWON111 = clsHcType.TMB.ONE_Inwon[21];
                item.ONE_INWON112 = clsHcType.TMB.ONE_Inwon[22];
                item.ONE_INWON013 = clsHcType.TMB.ONE_Inwon[23];
                if (!strErrChk.IsNullOrEmpty())
                {
                    item.GBERRCHK = strErrChk;
                }
                item.TWO_INWON01 = clsHcType.TMB.TWO_Inwon[1]; //상담료
                item.TWO_INWON02 = clsHcType.TMB.TWO_Inwon[2];
                item.TWO_INWON03 = clsHcType.TMB.TWO_Inwon[3];
                item.TWO_INWON04 = clsHcType.TMB.TWO_Inwon[4];
                item.TWO_INWON05 = clsHcType.TMB.TWO_Inwon[5];
                item.TWO_INWON06 = clsHcType.TMB.TWO_Inwon[6];
                item.TWO_INWON07 = clsHcType.TMB.TWO_Inwon[7];
                item.TWO_INWON08 = clsHcType.TMB.TWO_Inwon[8];
                item.TWO_INWON09 = clsHcType.TMB.TWO_Inwon[9];
                item.TWO_INWON10 = clsHcType.TMB.TWO_Inwon[10];
                item.TWO_INWON11 = clsHcType.TMB.TWO_Inwon[11];
                item.TWO_INWON12 = clsHcType.TMB.TWO_Inwon[12];
                item.TWO_INWON16 = clsHcType.TMB.TWO_Inwon[13];
                item.ROWID = clsHcType.TMB.ROWID;

                result = hicMirBohumService.UpdatebyRowId(item);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("HIC_MIR_BOHUM UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        /// <summary>
        /// 건강보험
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argFrDate"></param>
        /// <param name="argToDate"></param>
        void fn_Check_Date(long argMirno, string argFrDate, string argToDate)
        {
            int nREAD = 0;
            int nRead2 = 0;
            long nWRTNO = 0;
            long nPano = 0;
            long nPanDrno = 0;
            string GunDate = "";
            string PanDate = "";
            string TongDate = "";
            string GunDate2 = "";
            string PanDate2 = "";
            string TongDate2 = "";

            FnMirNo = argMirno; //청구번호
            clsDB.setBeginTran(clsDB.DbCon);
            chb.READ_HIC_MIR_BOHUM(argMirno);

            //1차 대상자 검색
            List<HIC_JEPSU_LTD_RES_BOHUM1> list = hicJepsuLtdResBohum1Service.GetItembyJepDateMirNo(argFrDate, argToDate, argMirno, clsHcType.TMB.CHASU);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                if (list[i].GJCHASU == "1")
                {
                    nWRTNO = list[i].WRTNO;
                    FstrSname = list[i].SNAME;
                    FstrSex = list[i].SEX;
                    FnAge = list[i].AGE;
                    nPano = list[i].PANO;
                    nPanDrno = list[i].PANJENGDRNO;
                    GunDate = list[i].JEPDATE;
                    PanDate = list[i].PANJENGDATE;
                    TongDate = list[i].TONGBODATE;

                    //판정일이 공휴일
                    if (hb.HIC_DATE_HUIL_Check(PanDate) == true)
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "1차 판정일확인-공.휴일");
                    }
                }

                nRead2 = 0;
                if (clsHcType.TMB.CHASU != "Y")
                {
                    //2차 대상자 검색
                    HIC_JEPSU_RES_BOHUM2 list2 = hicJepsuResBohum2Service.GetItembyJepDateMirNo(argFrDate, argToDate, argMirno);

                    if (!list2.IsNullOrEmpty())
                    {
                        if (list[i].GJCHASU == "1")
                        {
                            DateTime DATE1 = Convert.ToDateTime(GunDate);
                            DateTime DATE2 = Convert.ToDateTime(TongDate);
                            DateTime DATE3 = Convert.ToDateTime(PanDate);

                            if (DATE1 > DATE2)
                            {
                                fn_ERROR_INSERT(nWRTNO, "2", "1차 건진일이 통보일 보다 큽니다.");
                            }
                            if (DATE1 > DATE3)
                            {
                                fn_ERROR_INSERT(nWRTNO, "2", "1차 건진일이 판정일 보다 큽니다.");
                            }
                            if (DATE3 > DATE2)
                            {
                                fn_ERROR_INSERT(nWRTNO, "2", "1차 건진일이 판정일 보다 큽니다");
                            }
                        }

                        nWRTNO = list2.WRTNO;
                        FstrSname = list2.SNAME;
                        FstrSex = list2.SEX;
                        FnAge = list2.AGE;
                        nPano = list2.PANO;
                        nPanDrno = list2.PANJENGDRNO;
                        GunDate2 = list2.JEPDATE;
                        PanDate2 = list2.PANJENGDATE;
                        TongDate2 = list2.TONGBODATE;

                        //판정일이 공휴일
                        if (hb.HIC_DATE_HUIL_Check(PanDate2) == true)
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "2차 판정일확인-공.휴일.");
                        }

                        DateTime DATE21 = Convert.ToDateTime(GunDate2);
                        DateTime DATE22 = Convert.ToDateTime(TongDate2);
                        DateTime DATE23 = Convert.ToDateTime(PanDate2);

                        if (DATE21 > DATE22)
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "2차 건진일이 통보일 보다 큽니다.");
                        }
                        if (DATE21 > DATE23)
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "2차 건진일이 판정일 보다 큽니다.");
                        }
                        if (DATE23 > DATE22)
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "2차 판정일이 통보일 보다 큽니다.");
                        }

                        if (DATE21 > DATE23)
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "2차 건진일이 1차판정일 보다 작습니다.");
                        }
                        if (DATE23 > DATE21)
                        {
                            fn_ERROR_INSERT(nWRTNO, "2", "2차 판정일이 1차건진일 보다 작습니다.");
                        }
                    }
                }
                else
                {
                    GunDate2 = list[0].JEPDATE;
                    PanDate2 = list[0].PANJENGDATE;
                    PanDate2 = list[0].TONGBODATE;

                    DateTime DATE31 = Convert.ToDateTime(GunDate2);
                    DateTime DATE32 = Convert.ToDateTime(TongDate2);
                    DateTime DATE33 = Convert.ToDateTime(PanDate2);

                    if (DATE31 > DATE32)
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "2차 건진일이 통보일 보다 큽니다.");
                    }
                    if (DATE31 > DATE33)
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "2차 건진일이 판정일 보다 큽니다.");
                    }
                    if (DATE33 > DATE32)
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "2차 판정일이 통보일 보다 큽니다.");
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 암검진
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argFrDate"></param>
        /// <param name="argToDate"></param>
        void fn_Check_Date2(long argMirno, string argFrDate, string argToDate)
        {
            int nREAD = 0;
            int nRead2 = 0;
            long nWRTNO = 0;
            long nPano = 0;
            long nPanDrno = 0;
            string GunDate = "";
            string PanDate = "";
            string TongDate = "";
            string GunDate2 = "";
            string PanDate2 = "";
            string TongDate2 = "";
            string strJong = "";

            FnMirNo = argMirno; //청구번호
            clsDB.setBeginTran(clsDB.DbCon);
            chb.READ_HIC_MIR_BOHUM(argMirno);

            strJong = VB.Left(cboJong.Text, 1);

            //암 대상자 검색
            List<HIC_JEPSU_CANCER_NEW> list = hicJepsuCancerNewService.GetItembyJepDateMirNo(argFrDate, argToDate, argMirno, strJong);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                nWRTNO = list[i].WRTNO;
                FstrSname = list[i].SNAME;
                FstrSex = list[i].SEX;
                FnAge = list[i].AGE;
                nPano = list[i].PANO;
                nPanDrno = list[i].PANJENGDRNO;
                GunDate = list[i].JEPDATE;
                PanDate = list[i].PANJENGDATE;
                TongDate = list[i].TONGBODATE;

                //판정일이 공휴일
                if (hb.HIC_DATE_HUIL_Check(PanDate) == true)
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "1차 판정일확인-공.휴일");
                }

                if (list[i].GJCHASU == "3")
                {
                    DateTime DATE1 = Convert.ToDateTime(GunDate);
                    DateTime DATE2 = Convert.ToDateTime(TongDate);
                    DateTime DATE3 = Convert.ToDateTime(PanDate);

                    if (DATE1 > DATE2)
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", " 건진일이 통보일 보다 큽니다.");
                    }
                    if (DATE1 > DATE3)
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", " 건진일이 판정일 보다 큽니다.");
                    }
                    if (DATE3 > DATE2)
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", " 판정일이 통보일 보다 큽니다.");
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 건강보험 청구비용 단가 SET
        /// </summary>
        /// <param name="argYear"></param>
        /// <param name="argFrDate"></param>
        void fn_SET_BillAmt_Bohum(string argYear, string argFrDate)
        {
            int READ = 0;
            string strREC = "";
            string strCode = "";
            string strName = "";
            int inx = 0;
            long nAmt = 0;
            long[] nPrice1 = new long[51];
            long[] nPrice2 = new long[51];
            long[] nPrice3 = new long[51];
            string strSDate = "";
            string strFDate = "";
            string strTDate = "";

            //항목을 Clear
            for (int i = 0; i <= 50; i++)
            {
                FnOneAmt[i] = 0;
                FnTwoAmt[i] = 0;
                nPrice1[i] = 0;
                nPrice2[i] = 0;
                nPrice3[i] = 0;
            }

            //1=검진상담료 2=흉부검사 3=요검사 4=혈색소 5=공복혈당 6=총콜레스테롤
            //7=HDL콜레스테롤 8=트리글리세라이드 9=AST 10=ALT 11=감마지피티 12=혈청크레아티닌
            //16=항체(정밀) 17=양방사선골밀도 21=노인신체기능검사 22=LDL콜레스테롤 23=토.공휴일가산료

            //기준일자 설정
            strFDate = argYear + "-01-01";
            strTDate = argFrDate;
            if (string.Compare(argYear, VB.Left(argFrDate, 4)) < 0)
            {
                strTDate = argYear + "-12-31";
            }

            //기초코드에서 해당년도 청구비용을 읽음
            List<HIC_AMT_BOHUM> list = hicAmtBohumService.GetItembySDate(strFDate, strTDate);

            if (list.Count > 0)
            {
                //1차                
                nPrice1[1] = list[0].ONE_AMT01;
                nPrice1[2] = list[0].ONE_AMT02;
                nPrice1[3] = list[0].ONE_AMT03;
                nPrice1[4] = list[0].ONE_AMT04;
                nPrice1[5] = list[0].ONE_AMT05;
                nPrice1[6] = list[0].ONE_AMT06;
                nPrice1[7] = list[0].ONE_AMT07;
                nPrice1[8] = list[0].ONE_AMT08;
                nPrice1[9] = list[0].ONE_AMT09;
                nPrice1[10] = list[0].ONE_AMT10;
                nPrice1[11] = list[0].ONE_AMT11;
                nPrice1[12] = list[0].ONE_AMT12;
                nPrice1[13] = list[0].ONE_AMT13;
                nPrice1[14] = list[0].ONE_AMT14;
                nPrice1[15] = list[0].ONE_AMT15;
                nPrice1[16] = list[0].ONE_AMT16;
                nPrice1[17] = list[0].ONE_AMT17;
                nPrice1[18] = list[0].ONE_AMT18;
                nPrice1[19] = list[0].ONE_AMT19;
                nPrice1[20] = list[0].ONE_AMT20;
                nPrice1[21] = list[0].ONE_AMT21;
                nPrice1[22] = list[0].ONE_AMT22;
                nPrice1[23] = list[0].ONE_AMT23;
                nPrice1[24] = list[0].ONE_AMT24;
                nPrice1[25] = list[0].ONE_AMT25;

                //2차
                nPrice2[1] = list[0].TWO_AMT01;
                nPrice2[2] = list[0].TWO_AMT02;
                nPrice2[3] = list[0].TWO_AMT03;
                nPrice2[4] = list[0].TWO_AMT04;
                nPrice2[5] = list[0].TWO_AMT05;
                nPrice2[6] = list[0].TWO_AMT06;
                nPrice2[7] = list[0].TWO_AMT07;
                nPrice2[8] = list[0].TWO_AMT08;
                nPrice2[9] = list[0].TWO_AMT09;
                nPrice2[10] = list[0].TWO_AMT10;
                nPrice2[11] = list[0].TWO_AMT11;
                nPrice2[12] = list[0].TWO_AMT12;
                nPrice2[13] = list[0].TWO_AMT13;
                nPrice2[14] = list[0].TWO_AMT14;
                nPrice2[15] = list[0].TWO_AMT15;

                //3차
                nPrice3[1] = list[0].ONE_DENT1;
                nPrice3[2] = list[0].ONE_DENT2;

                FnOneAmt[1] = nPrice1[1];       //검진상담료
                FnOneAmt[2] = nPrice1[2];       //흉부방사선검사
                FnOneAmt[3] = nPrice1[3];       //요검사
                FnOneAmt[4] = nPrice1[4];       //혈색소
                FnOneAmt[5] = nPrice1[5];       //식전혈당
                FnOneAmt[6] = nPrice1[6];       //총콜레스테롤
                FnOneAmt[7] = nPrice1[7];       //HDL콜레스테롤
                FnOneAmt[8] = nPrice1[8];       //트리글리세라이드
                FnOneAmt[9] = nPrice1[9];       //AST(SGOT)
                FnOneAmt[10] = nPrice1[10];     //LT(SGPT)
                FnOneAmt[11] = nPrice1[11];     //마지피티
                FnOneAmt[12] = nPrice1[12];     //청클레아티닌
                FnOneAmt[13] = nPrice1[13];     //염항원일반
                FnOneAmt[14] = nPrice1[14];     //염항원정밀
                FnOneAmt[15] = nPrice1[15];     //염항체일반
                FnOneAmt[16] = nPrice1[16];     //염항체정밀
                FnOneAmt[17] = nPrice1[17];     //방사선골밀도
                FnOneAmt[18] = nPrice1[18];     //방사선말단골밀도
                FnOneAmt[19] = nPrice1[19];     //량적전산화단층
                FnOneAmt[20] = nPrice1[20];     //음파골밀도
                FnOneAmt[21] = nPrice1[21];     //인신체기능검사
                FnOneAmt[22] = nPrice1[22];     //DL콜레스테롤(추가분)
                FnOneAmt[23] = nPrice1[23];     //진찰료공휴가산

                FnTwoAmt[1] = nPrice2[1];       //차 상담료
                FnTwoAmt[2] = nPrice2[2];       //차 식전혈당(혈액화학분석기)
                FnTwoAmt[3] = nPrice2[3];       //차 식전혈당(자가혈당측정기)
                FnTwoAmt[4] = nPrice2[4];       //차 기본상담
                FnTwoAmt[5] = nPrice2[5];       //차 흡연
                FnTwoAmt[6] = nPrice2[6];       //차 음주
                FnTwoAmt[7] = nPrice2[7];       //차 운동
                FnTwoAmt[8] = nPrice2[8];       //차 영양
                FnTwoAmt[9] = nPrice2[9];       //2차 비만
                FnTwoAmt[10] = nPrice2[10];     //2차 우울증(CES-D)
                FnTwoAmt[11] = nPrice2[10];     //차 우울증(CES-D)
                FnTwoAmt[12] = nPrice2[11];     //차 인지기능검사(치매)
                FnTwoAmt[13] = nPrice2[12];     //차 상담료(공휴가산)
                FnTwoAmt[14] = nPrice2[13];     //신건강검사(PHQ-9)

                clsHcVariable.GnDentAmt = nPrice3[1];      //구강
                clsHcVariable.GnDentAddAmt = nPrice3[2];   //구강(휴일가산)
                clsHcVariable.GnDentAddAmt1 = nPrice1[24]; //치면세균막검사
            }
        }

        /// <summary>
        /// 건강검진비 파일생성 (청구서 + 1차검진 결과 + 2차검진 결과 + 문진표)
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argJohap"></param>
        /// <param name="argPath"></param>
        /// <param name="argDATE"></param>
        /// <param name="nCnt"></param>
        /// <param name="argLtd"></param>
        /// <param name="argMirGbn"></param>
        void fn_Make_File1(long argMirno, string argJohap, string argPath, string argDATE, int nCnt, string argLtd, string argMirGbn)
        {
            //StringBuilder FstrREC = new StringBuilder();

            int nREAD = 0;
            int nRead1 = 0;
            int nRead2 = 0;
            long nWRTNO = 0;
            long nAge = 0;
            string strGkiho = "";
            string strTongDate = "";
            string strGunDate = "";
            string strPandate = "";
            string strSogen = "";
            string strSogenB = "";
            string strSogenL = "";
            string strPanEtc = "";
            int nDentalCnt = 0;
            int nDentalHuCnt = 0;
            int nRowCnt = 0;
            string str교정시력 = "";
            string str교정청력 = "";
            string strLtdCode = "";
            string strGubun = "";
            string strGubun1 = "";
            string strJong = "";
            string strkiho = "";
            string strChasu = "";
            string strChungDate = "";
            string strChungDate1 = "";
            string strSex = "";
            string strCESD = ""; //우울증
            string strGDS = ""; //노인성우울증
            string strBogunso = ""; //보건소 -생애-성인병-급여만
            string strPtno = "";
            long nWRTNO1 = 0;

            string strSLIP1 = "";
            string strSLIP2 = "";
            string strSLIP3 = "";
            string strSLIP4 = "";
            string strSLIP5 = "";
            string strDAT = "";
            long[] nJemsu = new long[8];
            string strTSmoke1 = "";
            string strDrink1 = "";
            string strHealth1 = "";
            string strDiet1 = "";
            string strBiman1 = "";

            string strLife1 = "";
            string strLife2 = "";
            string strTSmoke = "";
            string strDrink = "";
            string strHealth = "";
            string strDiet = "";
            string strBiman = "";
            string strPHQScr = "";
            string strKDSQScr = "";
            double nBiman1 = 0;
            long nBiman2 = 0;
            long nIlsu1 = 0;
            long nTime11 = 0;
            long nTime12 = 0;
            long nIlsu2 = 0;
            long nTime21 = 0;
            long nTime22 = 0;
            long nTime1 = 0;
            long nTime2 = 0;
            string strOK = "";
            string strData = "";
            int nJumSu = 0;

            bool b자살생각 = false;
            string[] strGbSlip = new string[3];
            string Fstr인지능력 = "";

            int nITRVW1 = 0;
            int nITRVW2 = 0;
            int nITRVW3 = 0;
            int nITRVW4 = 0;
            int nITRVW5 = 0;
            int nITRVW6 = 0;
            int nITRVW7 = 0;
            int nITRVW8 = 0;
            int nITRVW9 = 0;
            int nITRVW10 = 0;
            int nITRVW11 = 0;
            int nITRVW12 = 0;
            int nITRVW13 = 0;
            int nITRVW14 = 0;
            int nITRVW15 = 0;
            int nITRVW16 = 0;
            int nITRVW17 = 0;
            int nITRVW18 = 0;
            int nITRVW19 = 0;
            int nITRVW20 = 0;
            int nITRVW21 = 0;
            int nITRVW22 = 0;
            int nITRVW23 = 0;
            int nITRVW24 = 0;
            int nITRVW25 = 0;
            int nITRVW26 = 0;
            int nITRVW27 = 0;
            int nITRVW28 = 0;
            int nITRVW29 = 0;
            int nITRVW30 = 0;
            int nITRVW31 = 0;
            int nITRVW32 = 0;
            int nITRVW33 = 0;
            int nITRVW34 = 0;
            int nITRVW35 = 0;
            int nITRVW36 = 0;
            int nITRVW37 = 0;
            int nITRVW38 = 0;
            int nITRVW39 = 0;
            int nITRVW40 = 0;

            string strSogenC = "";
            string strSogenD = "";

            string strGBCHK1 = "";
            string strGBCHK2 = "";

            string strCHK2 = "";    //의료급여대상(OK = 대상)
            string strCHK3 = "";    //콜레스테롤 검사확인

            string sJepDate = "";

            strJong = VB.Left(cboJong.Text, 1);
            strkiho = "37100068";
            strChungDate = argDATE;
            strChungDate1 = VB.Left(cboYear.Text, 2) + argDATE;

            //청구서 생성
            //청구내역읽음
            chb.READ_HIC_MIR_TBOHUM(argMirno);

            HIC_MIR_BOHUM list = hicMirBohumService.GetItemByMirno(argMirno);

            strGubun = "01";    //청구구분 건강검진 01
            if (argMirGbn == "Y")
            {
                strGubun = "B1";    //추가청구구분 건강검진 01
            }

            //생애-성인병-급여만
            strBogunso = "";
            if (clsHcType.TMB.GbJohap == "4" && clsHcType.TMB.Johap == "J")
            {
                strBogunso = hicJepsuService.GetBogunsobyJepDateMirNo(clsHcType.TMB.FrDate, clsHcType.TMB.ToDate, argMirno, "1");
            }

            nCnt += 1;
            if (list.CHASU == "Y")
            {
                strChasu = "12";
            }
            else
            {
                strChasu = "11";
            }

            string strFileName = argPath + strkiho + "_" + VB.Right(cboYear.Text, 2) + "01" + argJohap + "_" + strGubun + strChasu + "_" + string.Format("{0:000000}", argDATE) + "_" + string.Format("{0:00}", nCnt) + ".txt";

            StreamWriter sw = new StreamWriter(new FileStream(strFileName, FileMode.Create));

            //청구금액 계산
            //구강건수읽음
            List<HIC_JEPSU> list2 = hicJepsuService.GetWrtNobyJepDateGjYearMirno1(list.FRDATE, list.TODATE, VB.Left(list.FRDATE, 4));

            nREAD = list2.Count;
            nDentalCnt = 0;
            nDentalHuCnt = 0;

            for (int i = 0; i < nREAD; i++)
            {
                nDentalCnt += 1;    //구강건수

                if (hicSunapdtlService.GetCountbyWrtNoCode(list2[i].WRTNO, "1118") > 0)
                {
                    nDentalHuCnt += 1; //구강 공휴건수
                }
            }

            //실제 금액 조회
            List<HIC_JEPSU_SUNAP> list3 = hicJepsuSunapService.GetJohapAmtbyJepDAteMirNo1(list.FRDATE, argMirno);

            clsHcType.TMB.TAmt = list3[0].JOHAPAMT + list3[1].JOHAPAMT; //1,2차 금액
            clsHcType.TMB.TAmt -= (clsHcVariable.GnDentAmt * nDentalCnt) - (clsHcVariable.GnDentAddAmt * nDentalHuCnt);      //구강금액뺀금액

            sJepDate = cboYear.Text + "-01-01";

            FstrREC.Clear();
            FstrREC.Append("[");

            //1차 건강검진결과(SAM)
            List<HIC_JEPSU_PATIENT> list4 = hicJepsuPatientService.GetItembyJepDateMirNo1(sJepDate, argMirno);

            nREAD = list4.Count;

            for (int i = 0; i < nREAD; i++)
            {
                nWRTNO = list4[i].WRTNO;
                nWRTNO1 = list4[i].WRTNO;
                strGBCHK1 = list4[i].GBCHK1;
                nAge = list4[i].AGE;
                strSex = list4[i].SEX;
                strLtdCode = list4[i].LTDCODE.To<string>();
                strGkiho = list4[i].GKIHO.Replace("-", "");
                strPtno = list4[i].PTNO;
                strGunDate = VB.Left(list4[i].JEPDATE, 4) + VB.Mid(list4[i].JEPDATE, 6, 2) + VB.Right(list4[i].JEPDATE, 2);
                strPandate = list4[i].PANJENGDATE.To<string>().Replace("-", "");
                strTongDate = list4[i].TONGBODATE.Replace("-", "");

                FstrREC.Append("{" + "" + (char)34 + "ITEM0" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM1" + (char)34 + ":" + (char)34 + VB.Left(cboYear.Text + VB.Space(4), 4) + (char)34 + ",");                         //사업년도
                FstrREC.Append("" + (char)34 + "ITEM2" + (char)34 + ":" + (char)34 + VB.Left(clsAES.DeAES(list4[i].JUMIN2) + VB.Space(13), 13) + (char)34 + ",");      //주민번호
                FstrREC.Append("" + (char)34 + "ITEM3" + (char)34 + ":" + (char)34 + VB.Left(strGkiho + VB.Space(11), 11) + (char)34 + ",");                           //증번호

                //2020년 청구
                //Call 체위검사(ArgMirno, nWRTNO)
                fn_PhysicalExam(argMirno, nWRTNO);

                List<string> strGbSelf = new List<string>();
                strGbSelf.Clear();
                strGbSelf.Add("01");

                strCHK2 = "";
                //의료급여대상 조회
                if (hicSunapdtlService.GetCountbyWrtNoCodeGbSelf(nWRTNO, "1155", strGbSelf) > 0)
                {
                    strCHK2 = "OK";
                }

                strGbSelf.Clear();
                strGbSelf.Add("1");
                strGbSelf.Add("01");
                strCHK3 = "";
                //콜레스테롤 확인
                if (hicSunapdtlService.GetCountbyWrtNoCodeGbSelf(nWRTNO, "1160", strGbSelf) > 0)
                {
                    strCHK3 = "OK";
                }

                //fn_혈액검사(ArgMirno, nWRTNO, strCHK2, strCHK3);
                fn_BloodExam(argMirno, nWRTNO, strCHK2, strCHK3);
                //B형간염검사(ArgMirno, nWRTNO)
                fn_BHepatitistExam(argMirno, nWRTNO);
                //노인신체기능검사(ArgMirno, nWRTNO, strCHK2)
                fn_ElderlyPhysicalFunctionExam(argMirno, nWRTNO, strCHK2);
                //진찰및상담(ArgMirno, nWRTNO)
                fn_ClinicCounsel(argMirno, nWRTNO);

                List<string> strCodeList = new List<string>();

                strCodeList.Clear();
                strCodeList.Add("A130");
                strCodeList.Add("A129");

                List<HIC_RESULT> list5 = hicResultService.GetExCodeResultbyWrtNoExCode(nWRTNO1, strCodeList);

                if (list5.Count > 0)
                {
                    nREAD = list5.Count;
                    for (int j = 0; j < nREAD; j++)
                    {
                        switch (list5[j].EXCODE)
                        {
                            case "A130":
                                nJemsu[6] = list5[j].RESULT.To<long>(); //우울증
                                break;
                            case "A129":
                                nJemsu[7] = list5[j].RESULT.To<long>(); //인지기능
                                break;
                            default:
                                break;
                        }
                    }
                }

                for (int j = 0; j < 2; j++)
                {
                    strGbSlip[j] = "";
                }
                b자살생각 = false;

                //우울증,인지기능 대상인지 확인

                List<string> strCodeList1 = new List<string>();

                strCodeList1.Clear();
                strCodeList1.Add("1163");
                strCodeList1.Add("1167");
                strCodeList1.Add("1168");

                List<HIC_SUNAPDTL> list6 = hicSunapdtlService.GetCodebyWrtNoCode(nWRTNO1, strCodeList1);

                if (list6.Count > 0)
                {
                    for (int j = 0; j < list6.Count; j++)
                    {
                        switch (list6[j].CODE)
                        {
                            case "1167":
                                strGbSlip[1] = "Y"; //우울증
                                break;
                            case "1163":
                                strGbSlip[2] = "Y"; //인지기능
                                break;
                            default:
                                break;
                        }
                    }
                }

                //정신건강검사(ArgMirno, nWRTNO, strGbSlip(1), nJemsu(6))
                fn_MentalHealthExam(argMirno, nWRTNO, strGbSlip[1], nJemsu[6]);
                //인지기능장애(ArgMirno, nWRTNO, strGbSlip(2), nJemsu(7))
                fn_IsolatedCognitiveImpairment(argMirno, nWRTNO, strGbSlip[2], nJemsu[7]);
                //종합소견및판정(ArgMirno, ArgWRTNO)
                fn_TotalOpinionJudgment(argMirno, nWRTNO);
                //청구자료(ArgMirno, nWRTNO, strGunDate, strPandate, strTongDate)
                fn_ExpenseData(argMirno, nWRTNO, strGunDate, strPandate, strTongDate);

                //교정시력 여부를 찾음
                List<string> strCodeList2 = new List<string>();
                strCodeList2.Clear();
                strCodeList2.Add("A104");
                strCodeList2.Add("A105");

                List<HIC_RESULT> list7 = hicResultService.GetCorrectedEyeSightbyWrtNoExCode(nWRTNO, strCodeList2);

                str교정시력 = "0";
                for (int j = 0; j < list7.Count; j++)
                {
                    if (list7[j].RESULT.IndexOf("(") > 0)
                    {
                        str교정시력 = "1";
                    }
                }

                //교정청력 여부를 찾음
                List<string> strCodeList3 = new List<string>();
                strCodeList2.Clear();
                strCodeList2.Add("A106");
                strCodeList2.Add("A107");

                List<HIC_RESULT> list8 = hicResultService.GetCorrectedEyeSightbyWrtNoExCode(nWRTNO, strCodeList3);

                str교정청력 = "0";
                for (int j = 0; j < list8.Count; j++)
                {
                    if (list8[j].RESULT.IndexOf("(") > 0)
                    {
                        str교정청력 = "1";
                    }
                }


                FstrREC.Append("" + (char)34 + "ITEM136" + (char)34 + ":" + (char)34 + str교정시력 + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM137" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM138" + (char)34 + ":" + (char)34 + str교정청력 + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM139" + (char)34 + ":" + (char)34 + VB.Space(95) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM140" + (char)34 + ":" + (char)34 + "E" + (char)34 + "},");

                sw.WriteLine(FstrREC);
                FstrREC.Clear();
            }

            //============================================================================================================
            //-----------------------------
            //1,2차 문진내용(SAM)
            //-----------------------------
            List<HIC_JEPSU_PATIENT> list9 = hicJepsuPatientService.GetFirstMunbyJepDateMirNo1(sJepDate, argMirno);

            nRead2 = list9.Count;

            for (int i = 0; i < nRead2; i++)
            {
                nWRTNO = list9[i].WRTNO;
                if (VB.Mid(list9[i].GKIHO, 2, 1) == "-")
                {
                    strGkiho = VB.Mid(list9[i].GKIHO, 1, 1) + VB.Mid(list9[i].GKIHO, 3, 9) + VB.Mid(list9[i].GKIHO, 1, 1) + VB.Right(list9[i].GKIHO, 1);
                }
                else
                {
                    strGkiho = "";
                }
                strLtdCode = list9[i].LTDCODE.To<string>();

                FstrREC.Append("{" + "" + (char)34 + "ITEM0" + (char)34 + ":" + (char)34 + "4" + (char)34 + ",");                                                   //청구파일 구분)
                FstrREC.Append("" + (char)34 + "ITEM1" + (char)34 + ":" + (char)34 + VB.Left(cboYear.Text + VB.Space(4), 4) + (char)34 + ",");                      //사업년도
                FstrREC.Append("" + (char)34 + "ITEM2" + (char)34 + ":" + (char)34 + VB.Left(clsAES.DeAES(list9[i].JUMIN2) + VB.Space(13), 13) + (char)34 + ",");   //주민번호
                FstrREC.Append("" + (char)34 + "ITEM3" + (char)34 + ":" + (char)34 + VB.Left(strGkiho + VB.Space(11), 11) + (char)34 + ",");                        //증번호

                if (VB.L(list9[i].TEL, "-") - 1 == 2)
                {
                    FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list9[i].TEL, "-", 1) + VB.Space(4), 4) + (char)34 + ","); //전화 지역
                    FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list9[i].TEL, "-", 2) + VB.Space(4), 4) + (char)34 + ","); //전화 국번
                    FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list9[i].TEL, "-", 3) + VB.Space(4), 4) + (char)34 + ","); //전화 번호
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                }

                if (VB.L(list9[i].HPHONE, "-") - 1 == 2)
                {
                    FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list9[i].HPHONE, "-", 1) + VB.Space(4), 4) + (char)34 + ",");   //핸드폰 지역
                    FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list9[i].HPHONE, "-", 2) + VB.Space(4), 4) + (char)34 + ",");   //핸드폰 국번
                    FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list9[i].HPHONE, "-", 3) + VB.Space(4), 4) + (char)34 + ",");   //핸드폰 번호
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                }

                if (!list9[i].EMAIL.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(list9[i].EMAIL, 40) + VB.Space(40), 40) + (char)34 + ",");        //전자우편주소
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + VB.Space(40) + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM11" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");          //통보서 수신방법(1:우편, 2:이메일)

                //과거병력(ArgMirno, nWRTNO)
                fn_PastTroops(argMirno, nWRTNO);
                //흡연(ArgMirno, nWRTNO)
                fn_Smoking(argMirno, nWRTNO);
                //음주(ArgMirno, nWRTNO)
                fn_Drinking(argMirno, nWRTNO);
                //운동(ArgMirno, nWRTNO)
                fn_Exercise(argMirno, nWRTNO);

                //20 신규자료
                FstrREC.Append("" + (char)34 + "ITEM103" + (char)34 + ":" + (char)34 + VB.Left(list9[i].MAILCODE + VB.Space(5), 5) + (char)34 + ",");           //우편번호
                FstrREC.Append("" + (char)34 + "ITEM104" + (char)34 + ":" + (char)34 + VB.Left(list9[i].JUSO1 + VB.Space(200), 200) + (char)34 + ",");          //주소
                FstrREC.Append("" + (char)34 + "ITEM105" + (char)34 + ":" + (char)34 + VB.Left(list9[i].JUSO2 + VB.Space(200), 200) + (char)34 + ",");          //상세주소
                FstrREC.Append("" + (char)34 + "ITEM106" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM107" + (char)34 + ":" + (char)34 + "E" + (char)34 + "},");

                sw.WriteLine(FstrREC);
                FstrREC.Clear();
            }

            //생활습관 청구파일 생성

            List<string> sExCodeList = new List<string>();

            sExCodeList.Clear();
            sExCodeList.Add("A143");
            sExCodeList.Add("A144");
            sExCodeList.Add("A145");
            sExCodeList.Add("A146");
            sExCodeList.Add("A147");
            sExCodeList.Add("A130");
            sExCodeList.Add("A129");

            List<HIC_JEPSU_PATIENT> list10 = hicJepsuPatientService.GetLifeItembyJepDateMirno1(sJepDate, argMirno);

            nREAD = list10.Count;

            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list10[i].WRTNO;
                    nAge = list10[i].AGE;
                    strGkiho = list10[i].GKIHO.Replace("-", "");
                    //생활습관 검진일자,판정일자,통보일자
                    strGunDate = VB.Left(list10[i].JEPDATE, 4) + VB.Mid(list10[i].JEPDATE, 6, 2) + VB.Right(list10[i].JEPDATE, 2);
                    strPandate = list10[i].PANJENGDATE.To<string>().Replace("-", "");
                    strTongDate = list10[i].TONGBODATE.Replace("-", "");

                    if (nAge == 40 || nAge == 50 || nAge == 60 || nAge == 70)
                    {
                        FstrREC.Append("{" + "" + (char)34 + "ITEM0" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM1" + (char)34 + ":" + (char)34 + VB.Left(cboYear.Text + VB.Space(4), 4) + (char)34 + ","); //사업년도
                        FstrREC.Append("" + (char)34 + "ITEM2" + (char)34 + ":" + (char)34 + VB.Left(clsAES.DeAES(list10[i].JUMIN2) + VB.Space(13), 13) + (char)34 + ","); //주민번호
                        FstrREC.Append("" + (char)34 + "ITEM3" + (char)34 + ":" + (char)34 + VB.Left(strGkiho + VB.Space(11), 11) + (char)34 + ","); //증번호

                        strSLIP1 = list10[i].SLIP_SMOKE;
                        strSLIP2 = list10[i].SLIP_DRINK;
                        strSLIP3 = list10[i].SLIP_ACTIVE;
                        strSLIP4 = list10[i].SLIP_FOOD;
                        strSLIP5 = list10[i].SLIP_BIMAN;
                        strTSmoke1 = list10[i].T_SMOKE1;
                        strDrink1 = list10[i].TMUN0003;

                        List<HIC_RESULT> hIC_RESULTs = hicResultService.GetCorrectedEyeSightbyWrtNoExCode(nWRTNO, sExCodeList);

                        if (hIC_RESULTs.Count > 0)
                        {
                            for (int j = 0; j < hIC_RESULTs.Count; j++)
                            {
                                switch (hIC_RESULTs[j].EXCODE)
                                {
                                    case "A143":
                                        nJemsu[1] = hIC_RESULTs[j].RESULT.To<long>(); //흡연
                                        break;
                                    case "A144":
                                        nJemsu[2] = hIC_RESULTs[j].RESULT.To<long>(); //음주
                                        break;
                                    case "A145":
                                        nJemsu[3] = hIC_RESULTs[j].RESULT.To<long>(); //운동
                                        break;
                                    case "A146":
                                        nJemsu[4] = hIC_RESULTs[j].RESULT.To<long>(); //영양
                                        break;
                                    case "A147":
                                        nJemsu[5] = hIC_RESULTs[j].RESULT.To<long>(); //비만
                                        break;
                                    case "A130":
                                        nJemsu[6] = hIC_RESULTs[j].RESULT.To<long>(); //우울증
                                        break;
                                    case "A129":
                                        nJemsu[7] = hIC_RESULTs[j].RESULT.To<long>(); //인지기능
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        strTSmoke = nJemsu[1].To<string>();
                        strDrink = nJemsu[2].To<string>();
                        strDiet = nJemsu[4].To<string>();
                        strBiman = nJemsu[5].To<string>();

                        strLife1 = "";
                        strLife2 = "";

                        List<string> sCodeList = new List<string>();
                        sCodeList.Clear();
                        sCodeList.Add("1165");
                        sCodeList.Add("1166");

                        List<HIC_SUNAPDTL> hIC_SUNAPDTLs = hicSunapdtlService.GetCodebyWrtNoCode(nWRTNO, sCodeList);

                        nRead1 = hIC_SUNAPDTLs.Count;
                        if (nRead1 > 0)
                        {
                            for (int j = 0; j < nRead1; j++)
                            {
                                switch (hIC_SUNAPDTLs[j].CODE)
                                {
                                    case "1165":
                                        strLife1 = "OK";    //흡연
                                        break;
                                    case "1166":
                                        strLife2 = "OK";    //음주
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        //생활습관흡연(ArgMirno, nWRTNO, strSLIP1, strLife1)
                        fn_LifeStype_Smoking(argMirno, nWRTNO, strSLIP1, strLife1);
                        //생활습관음주(ArgMirno, nWRTNO, strSLIP2, strDrink1, strLife2, strDrink)
                        fn_LifeStype_Drinking(argMirno, nWRTNO, strSLIP2, strDrink1, strLife2, strDrink);
                        //생활습관운동(ArgMirno, nWRTNO, strSLIP3)
                        fn_LifeStype_Exercise(argMirno, nWRTNO, strSLIP3);
                        //생활습관영양(ArgMirno, nWRTNO, strSLIP4, strDiet)
                        fn_LifeStype_Diet(argMirno, nWRTNO, strSLIP4, strDiet);
                        //생활습관비만(ArgMirno, nWRTNO, strSLIP5)
                        fn_LifeStype_Biman(argMirno, nWRTNO, strSLIP5);

                        FstrREC.Append("" + (char)34 + "ITEM98" + (char)34 + ":" + (char)34 + VB.Left(strGunDate + VB.Space(8), 8) + (char)34 + ",");     //검진일자
                        FstrREC.Append("" + (char)34 + "ITEM99" + (char)34 + ":" + (char)34 + VB.Left(strPandate + VB.Space(8), 8) + (char)34 + ",");     //판정일자

                        //검진장소구분(1-출장,2내원)
                        if (list10[i].GBCHUL == "Y" && list10[i].GBCHUL2.IsNullOrEmpty())
                        {
                            FstrREC.Append("" + (char)34 + "ITEM100" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                        }
                        else
                        {
                            FstrREC.Append("" + (char)34 + "ITEM100" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                        }

                        if (list10[i].LTDCODE != 0 && !list10[i].LTDCODE.IsNullOrEmpty())
                        {
                            hb.READ_Ltd_Name(list10[i].LTDCODE.To<string>());
                            if (clsHcVariable.GstrLtdJuso != list10[i].JUSOAA)
                            {
                                FstrREC.Append("" + (char)34 + "ITEM101" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                            }
                            else
                            {
                                FstrREC.Append("" + (char)34 + "ITEM101" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");       //회사코드와 접수시 주소가 다르면 개인으로
                            }
                        }
                        else
                        {
                            FstrREC.Append("" + (char)34 + "ITEM101" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");           //회사코드 없으면 개인으로
                        }

                        FstrREC.Append("" + (char)34 + "ITEM102" + (char)34 + ":" + (char)34 + VB.Left(strTongDate + VB.Space(8), 8) + (char)34 + ",");        //통보일자

                        if (!list10[i].PANJENGDRNO.IsNullOrEmpty())
                        {
                            FstrREC.Append("" + (char)34 + "ITEM103" + (char)34 + ":" + (char)34 + VB.Left(list10[i].PANJENGDRNO + VB.Space(10), 10) + (char)34 + ",");     //의사면허
                            FstrREC.Append("" + (char)34 + "ITEM104" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(list10[i].PANJENGDRNO.To<string>()) + VB.Space(12), 12) + (char)34 + ",");     //의사성명
                            FstrREC.Append("" + (char)34 + "ITEM105" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN(list10[i].PANJENGDRNO.To<string>()) + VB.Space(13), 13) + (char)34 + ",");    //의사주민번호
                        }
                        else
                        {
                            FstrREC.Append("" + (char)34 + "ITEM103" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                            FstrREC.Append("" + (char)34 + "ITEM104" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                            FstrREC.Append("" + (char)34 + "ITEM105" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                        }

                        FstrREC.Append("" + (char)34 + "ITEM106" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(list10[i].PANJENGDRNO.To<string>()) + VB.Space(12), 12) + (char)34 + ",");    //주치의이름(의사성명)

                        //생활습관처방
                        //생활습관금연처방(strSLIP1, strLife1)
                        fn_LifeStype_Smoking_Order(strSLIP1, strLife1);
                        //생활습관음주처방(strSLIP2, strLife2, strDrink)
                        fn_LifeStype_Drinking_Order(strSLIP2, strLife2, strDrink);
                        //생활습관운동처방(strSLIP3)
                        fn_LifeStype_Exercise_Order(strSLIP3);
                        //생활습관영양처방(strSLIP4)
                        fn_LifeStype_Diet_Order(strSLIP4);
                        //생활습관비만처방(strSLIP5)
                        fn_LifeStype_Biman_Order(strSLIP5);
                    }
                    else
                    {
                        FstrREC.Append(VB.Space(2750)); //대상아닐경우
                    }

                    FstrREC.Append("" + (char)34 + "ITEM234" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");

                    if (i == nREAD)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM235" + (char)34 + ":" + (char)34 + "E" + (char)34 + "}");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM235" + (char)34 + ":" + (char)34 + "E" + (char)34 + "},");
                    }

                    FstrREC.Append("]");
                    sw.WriteLine(FstrREC);
                    FstrREC.Clear();
                    sw.Close();
                }
            }

            strFileName = "";
            strFileName = strkiho + "_" + VB.Right(cboYear.Text, 2) + "01" + argJohap + "_" + strGubun + strChasu + "_" + string.Format("{0:000000}", argDATE) + "_" + string.Format("{0:00}", nCnt);

            string strJepNo = VB.Mid(clsPublic.GstrSysDate, 2, 2) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Right(clsPublic.GstrSysDate, 2) + string.Format("{0:00}", nCnt);

            clsDB.setBeginTran(clsDB.DbCon);
            //청구완료 DB에 UPDATE
            int result = hicMirBohumService.UpdateJepNoFileNameJepDatebyMirNo(strJepNo, strFileName, argMirno);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_MIR_HOHUM에 청구번호 UPDATE시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argJohap"></param>
        /// <param name="argPath"></param>
        /// <param name="argDATE"></param>
        /// <param name="nCnt"></param>
        /// <param name="argLtd"></param>
        /// <param name="argMirGbn"></param>
        void fn_Make_File2(long argMirno, string argJohap, string argPath, string argDATE, int nCnt, string argLtd, string argMirGbn)
        {
            int nREAD = 0;
            int nRead1 = 0;
            string strkiho = "";
            string strJong = "";
            long nMirno = 0;
            string strLtdCode = "";
            string strGubun = "";
            string strJohap = "";
            string strChungDate = "";
            string strChungDate1 = "";
            string strGkiho = "";
            string strGunDate = "";
            string strTongDate = "";
            string strFileName = "";
            string strPathName = "";
            string strCDate = "";
            string strChasu = "";
            long nDentalCnt = 0;
            long nDentalHuCnt = 0;
            string strBogunso = "";  //생애-성인병-급여
            string strPtno = "";
            string strGjJong = "";  //검진종류 2010
            string strRES_Munjin = "";
            string strRES_Jochi = "";
            string strRES_Result = "";
            string strSangdam = "";
            string strSogen = "";
            string strDANTAL1 = "";
            long nWRTNO = 0;

            string sJepDate = "";
            string sFDate = "";
            string sTDate = "";

            strJong = VB.Left(cboJong.Text, 1);
            strkiho = "37100068";
            strCDate = VB.Left(cboYear.Text, 2) + argDATE;

            //구강 검사 (청구서 + 구강검사결과 + 문진표)
            //청구서 생성 => 
            ///TODO : 이상훈(2021.01.12) 이걸 왜 해놨을까요???????
            HIC_MIR_DENTAL list = hicMirDentalService.GetChasubyMirNo(argMirno);

            if (!list.IsNullOrEmpty())
            {
                if (list.CHASU == "Y")
                {
                    strChasu = "20";
                }
                else
                {
                    strChasu = "20";
                }

                sFDate = list.FRDATE;
                sTDate = list.TODATE;
            }

            //청구내역읽음
            chb.READ_HIC_MIR_DENTAL(argMirno);

            //청구금액 계산
            //구강건수읽음
            List<HIC_JEPSU> list2 = hicJepsuService.GetWrtNobyJepDateGjYear(sFDate, sTDate, VB.Left(sFDate, 4), argMirno);

            nREAD = list2.Count;
            nDentalCnt = 0;
            nDentalHuCnt = 0;

            for (int i = 0; i < nREAD; i++)
            {
                nDentalCnt += 1; //구강건수

                if (hicSunapdtlService.GetCountbyWrtNoCode(list2[i].WRTNO, "1118") > 0)
                {
                    nDentalHuCnt += 1; //구강 공휴건수
                }
            }

            clsHcType.TMB.TAmt = (clsHcVariable.GnDentAmt * nDentalCnt) + (clsHcVariable.GnDentAddAmt * nDentalHuCnt);   //구강금액
            nCnt += 1;

            //생애-성인병-급여만
            strBogunso = "";
            if (clsHcType.TMD.GbJohap == "4" && clsHcType.TMD.GbJohap == "J")   //급여
            {
                strBogunso = hicJepsuService.GetBogunsobyJepDateMirNo(clsHcType.TMD.FrDate, clsHcType.TMD.ToDate, argMirno, "1");
            }

            strGubun = "06"; //청구구분 구강일경우 06 세팅
            if (argMirGbn == "Y")
            {
                strGubun = "B6";    //구강추가청구 M
            }

            //파일생성
            strFileName = argPath + strkiho + "_" + VB.Right(cboYear.Text, 2) + "01" + argJohap + "_" + strGubun + strChasu + "_" + string.Format("{0:000000}", argDATE) + "_" + string.Format("{0:00}", nCnt) + ".txt";

            StreamWriter sw = new StreamWriter(new FileStream(strFileName, FileMode.Create));

            FstrREC.Clear();
            FstrREC.Append("[");

            //구강 검진결과(SAM)
            List<HIC_JEPSU_PATIENT> list3 = hicJepsuPatientService.GetDentalItembyJepDateMirNo2(sJepDate, argMirno);

            nREAD = list3.Count;
            for (int i = 0; i < nREAD; i++)
            {
                nWRTNO = list3[i].WRTNO;
                strLtdCode = list3[i].LTDCODE.To<string>();
                strGjJong = list3[i].GJJONG;
                strPtno = list3[i].PTNO;
                if (VB.Mid(list3[i].GKIHO, 2, 1) == "-")
                {
                    strGkiho = VB.Mid(list3[i].GKIHO, 1, 1) + VB.Mid(list3[i].GKIHO, 3, 9) + VB.Right(list3[i].GKIHO, 1);
                }
                else
                {
                    strGkiho = list3[i].GKIHO;
                }

                strGunDate = VB.Left(list3[i].JEPDATE, 4) + VB.Mid(list3[i].JEPDATE, 6, 2) + VB.Mid(list3[i].JEPDATE, 9, 2);
                strTongDate = VB.Left(list3[i].TONGBODATE, 4) + VB.Mid(list3[i].TONGBODATE, 6, 2) + VB.Mid(list3[i].TONGBODATE, 9, 2);
                strRES_Munjin = list3[i].RES_MUNJIN;
                strRES_Jochi = list3[i].RES_JOCHI;
                strRES_Result = list3[i].RES_RESULT;

                strSogen = list3[i].T_PANJENG_SOGEN;    // 바로조치
                strSangdam = list3[i].SANGDAM;          //적극적인 관리

                if (strSogen.IsNullOrEmpty())
                {
                    strSogen = "특이소견없음";
                }

                if (strSangdam.IsNullOrEmpty())
                {
                    strSangdam = "특이소견없음";
                }

                FstrREC.Append("{" + "" + (char)34 + "ITEM0" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");   //청구파일 구분
                FstrREC.Append("" + (char)34 + "ITEM1" + (char)34 + ":" + (char)34 + VB.Left(cboYear.Text + VB.Space(4), 4) + (char)34 + ",");  //사업년도
                FstrREC.Append("" + (char)34 + "ITEM2" + (char)34 + ":" + (char)34 + VB.Left(clsAES.DeAES(list3[i].JUMIN2) + VB.Space(13), 13) + (char)34 + ","); //주민번호
                FstrREC.Append("" + (char)34 + "ITEM3" + (char)34 + ":" + (char)34 + VB.Left(strGkiho + VB.Space(11), 11) + (char)34 + ",");    //증번호

                //치면세균막검사
                strDANTAL1 = "";

                if (hicSunapdtlService.GetCountbyWrtNoCode(nWRTNO, "1158") > 0)
                {
                    strDANTAL1 = "Y";
                }

                //구강결과(ArgMirno, nWRTNO, strDANTAL1)
                fn_DentalResult(argMirno, nWRTNO, strDANTAL1);

                //바로조치사항
                FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(strSogen, 500) + VB.Space(500), 500) + (char)34 + ",");
                //적극적인관리
                FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(strSangdam, 500) + VB.Space(500), 500) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + VB.Left(strGunDate + VB.Space(8), 8) + (char)34 + ",");                             //검사일자
                //검진장소구분(출장:1,내원:2)
                if (list3[i].GBCHUL == "Y" && list3[i].GBCHUL2.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + VB.Left(list3[i].PANJENGDRNO + VB.Space(10), 10) + (char)34 + ",");                       //의사면허번호
                FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(list3[i].PANJENGDRNO.To<string>()) + VB.Space(12), 12) + (char)34 + ",");      //의사명 12
                FstrREC.Append("" + (char)34 + "ITEM30" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN(list3[i].PANJENGDRNO.To<string>()) + VB.Space(13), 13) + (char)34 + ",");     //의사주민번호 13

                //2012년 추가됨
                if (!chb.READ_ResultApplicationAgreeWhether(strPtno, cboYear.Text).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                                 //결과활용동의여부 -동의1, 동의안함0
                    FstrREC.Append("" + (char)34 + "ITEM32" + (char)34 + ":" + (char)34 + chb.READ_ResultApplicationAgreeWhether(strPtno, cboYear.Text) + (char)34 + ",");      //결과활용동의 일자
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");             //결과활용동의여부 -동의1, 동의안함0
                    FstrREC.Append("" + (char)34 + "ITEM32" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");        //결과활용동의 일자
                }

                FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");    //SPACE
                FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + "E" + (char)34 + "},");

                sw.WriteLine(FstrREC);
                FstrREC.Clear();
            }

            //구강 문진(SAM)
            List<HIC_JEPSU_PATIENT> list4 = hicJepsuPatientService.GetDentalItembyJepDateMirNo2(sJepDate, argMirno);

            nRead1 = list4.Count;
            for (int i = 0; i < nRead1; i++)
            {
                if (VB.Mid(list4[i].GKIHO, 2, 1) == "-")
                {
                    strGkiho = VB.Mid(list4[i].GKIHO, 1, 1) + VB.Mid(list4[i].GKIHO, 3, 9) + VB.Right(list4[i].GKIHO, 1);
                }
                else
                {
                    strGkiho = list4[i].GKIHO;
                }

                FstrREC.Append("{" + "" + (char)34 + "ITEM0" + (char)34 + ":" + (char)34 + "4" + (char)34 + ",");                                                            //청구파일 구분
                FstrREC.Append("" + (char)34 + "ITEM1" + (char)34 + ":" + (char)34 + VB.Left(cboYear.Text + VB.Space(4), 4) + (char)34 + ",");                                //사업년도
                FstrREC.Append("" + (char)34 + "ITEM2" + (char)34 + ":" + (char)34 + VB.Left(clsAES.DeAES(list4[i].JUMIN2) + VB.Space(13), 13) + (char)34 + ",");      //주민번호
                FstrREC.Append("" + (char)34 + "ITEM3" + (char)34 + ":" + (char)34 + VB.Left(strGkiho + VB.Space(11), 11) + (char)34 + ",");                                      //증번호

                if (VB.L(list4[i].TEL, "-") - 1 == 2)
                {
                    FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list4[i].TEL, "-", 1) + VB.Space(4), 4) + (char)34 + ",");  //전화 지역
                    FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list4[i].TEL, "-", 2) + VB.Space(4), 4) + (char)34 + ",");  //전화 국번
                    FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list4[i].TEL, "-", 3) + VB.Space(4), 4) + (char)34 + ",");  //전화 번호
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                }

                if (VB.L(list4[i].HPHONE, "-") - 1 == 2)
                {
                    FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list4[i].HPHONE, "-", 1) + VB.Space(4), 4) + (char)34 + ",");  //핸드폰 지역
                    FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list4[i].HPHONE, "-", 2) + VB.Space(4), 4) + (char)34 + ",");  //핸드폰 국번
                    FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list4[i].HPHONE, "-", 3) + VB.Space(4), 4) + (char)34 + ",");  //핸드폰 번호
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                }

                if (!list4[i].EMAIL.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(list4[i].EMAIL, 40) + VB.Space(40), 40) + (char)34 + ",");        //전자우편주소
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + VB.Space(40) + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM11" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");          //통보서 수신방법(1:우편, 2:이메일)

                //구강문진(ArgMirno, nWRTNO)
                fn_DentalMunjin(argMirno, nWRTNO);

                //2020 신규자료
                FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + VB.Left(list4[i].MAILCODE + VB.Space(5), 5) + (char)34 + ",");           //우편번호
                FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + VB.Left(list4[i].JUSO1 + VB.Space(200), 200) + (char)34 + ",");          //주소
                FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + VB.Left(list4[i].JUSO2 + VB.Space(200), 200) + (char)34 + ",");          //상세주소


                FstrREC.Append("" + (char)34 + "ITEM30" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");                    //SPACE


                if (i == nRead1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + "E" + (char)34 + "}");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + "E" + (char)34 + "},");
                }

                FstrREC.Append("]");
                sw.WriteLine(FstrREC);
                FstrREC.Clear();
            }

            sw.Close();

            strFileName = "";
            strFileName = strkiho + "_" + VB.Right(cboYear.Text, 2) + "01" + argJohap + "_" + strGubun + strChasu + "_" + string.Format("{0:000000}", argDATE) + "_" + string.Format("{0:00}", nCnt);

            string strJepNo = VB.Mid(clsPublic.GstrSysDate, 2, 2) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Right(clsPublic.GstrSysDate, 2) + string.Format("{0:00}", nCnt);

            clsDB.setBeginTran(clsDB.DbCon);
            //청구완료 DB에 UPDATE
            int result = hicMirDentalService.UpdateJepNoFileNameJepDatebyMirNo(strJepNo, strFileName, argMirno);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_MIR_DENTAL 청구번호 UPDATE시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argJohap"></param>
        /// <param name="argPath"></param>
        /// <param name="argDATE"></param>
        /// <param name="nCnt"></param>
        /// <param name="argLtd"></param>
        /// <param name="argMirGbn"></param>
        void fn_Make_File3(long argMirno, string argJohap, string argPath, string argDate, int nCnt, string argMirGbn)
        {
            int nREAD = 0;
            int nRead1 = 0;
            long nMirno = 0;
            string strkiho = "";
            string strJong = "";
            string strLtdCode = "";
            string strGubun = "";
            string strJohap = "";
            string strChungDate = "";
            string strChungDate1 = "";
            string strGkiho = "";
            string strGunDate = "";
            string strTongDate = "";
            string strFileName = "";
            string strPathName = "";
            string strCDate = "";
            string strChasu = "";
            string strRectum = "";
            string strLiver = "";
            string strBogunso = "";
            string strJepNo = "";
            int result = 0;

            strJong = VB.Left(cboJong.Text, 1);
            strkiho = "37100068";
            strCDate = VB.Left(cboYear.Text, 2) + argDate;
            //암검진 검사 (청구서 + 무료암청구서(보건소)+ 암검사결과 + 문진표)

            //청구서 생성
            chb.READ_HIC_MIR_CANCER(argMirno);
            chb.READ_HIC_MIR_CANCER_Bo(argMirno);

            strLtdCode = "";
            if (VB.Left(cboJong.Text, 1) == "4")
            {
                strGubun = "04";    //특정암,보건소암

                if (argMirGbn == "Y")
                {
                    strGubun = "B4";
                }

                strBogunso = hicJepsuService.GetBogunsobyJepDateMirNo(clsHcType.TMC.FrDate, clsHcType.TMC.ToDate, argMirno, "3");
            }
            else if (VB.Left(cboJong.Text, 1) == "E")
            {
                strGubun = "05";    //의료급여암

                if (argMirGbn == "Y")
                {
                    strGubun = "B5";
                }

                strBogunso = hicJepsuService.GetBogunsobyJepDateMirNo(clsHcType.TMC.FrDate, clsHcType.TMC.ToDate, argMirno, "5");
            }

            nCnt += 1;
            strChasu = "30";

            //파일생성
            strFileName = argPath + strkiho + "_" + VB.Right(cboYear.Text, 2) + "01" + argJohap + "_" + strGubun + strChasu + "_" + string.Format("{0:000000}", argDate) + "_" + string.Format("{0:00}", nCnt) + ".txt";

            StreamWriter sw = new StreamWriter(new FileStream(strFileName, FileMode.Create));

            FstrREC.Clear();
            FstrREC.Append("[");

            //암검진 결과(위,대장,간,유방,자궁,폐암)
            fn_Cancer_Mir_Result1_File(argMirno, sw);
            //암검진 문진
            fn_Cancer_Mir_Munjin_File(argMirno, sw);

            strJepNo = VB.Mid(clsPublic.GstrSysDate, 2, 2) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Right(clsPublic.GstrSysDate, 2) + string.Format("{0:00}", nCnt);
            strFileName = strkiho + "_" + VB.Right(cboYear.Text, 2) + "01" + argJohap + "_" + strGubun + strChasu + "_" + string.Format("{0:000000}", argDate) + "_" + string.Format("{0:00}", nCnt);
            //청구완료 DB에 UPDATE

            clsDB.setBeginTran(clsDB.DbCon);
            result = hicMirCancerBoService.UpdateJepNoFileNameJepDatebyMirNo(strJepNo, strFileName, argMirno);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_MIR_CANCER_bo 에 청구번호 UPDATE시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argJohap"></param>
        /// <param name="argPath"></param>
        /// <param name="argDATE"></param>
        /// <param name="nCnt"></param>
        /// <param name="argLtd"></param>
        /// <param name="argMirGbn"></param>
        void fn_Make_File5(long argMirno, string argJohap, string argPath, string argDATE, int nCnt, string argLtd, string argMirGbn)
        {
            int nREAD = 0;
            int nRead1 = 0;
            int nRead2 = 0;
            long nWRTNO = 0;
            long nAge = 0;
            string strGkiho = "";
            string strTongDate = "";
            string strGunDate = "";
            string strPandate = "";
            string strSogen = "";
            string strSogenB = "";
            string strSogenL = "";
            string strPanEtc = "";
            int nDentalCnt = 0;
            int nDentalHuCnt = 0;
            int nRowCnt = 0;
            string str교정시력 = "";
            string strLtdCode = "";
            string strGubun = "";
            string strGubun1 = "";
            string strJong = "";
            string strkiho = "";
            string strChasu = "";
            string strChungDate = "";
            string strChungDate1 = "";
            string strSex = "";
            string strCESD = "";    //우울증
            string strGDS = "";     //노인성우울증
            string strBogunso = ""; //보건소-생애-성인병-급여만
            string strPtno = "";

            string strSLIP1 = "";
            string strSLIP2 = "";
            string strSLIP3 = "";
            string strSLIP4 = "";
            string strSLIP5 = "";
            string strDAT = "";
            long[] nJemsu = new long[7];
            string strTSmoke1 = "";
            string strDrink1 = "";
            string strHealth1 = "";
            string strDiet1 = "";
            string strBiman1 = "";

            string strLife1 = "";
            string strLife2 = "";

            string strTSmoke = "";
            string strDrink = "";
            string strHealth = "";
            string strDiet = "";
            string strBiman = "";

            string strPHQScr = "";
            string strKDSQScr = "";

            double nBiman1 = 0;
            long nBiman2 = 0;
            int nIlsu1 = 0;
            int nTime11 = 0;
            int nTime12 = 0;
            int nIlsu2 = 0;
            int nTime21 = 0;
            int nTime22 = 0;
            int nTime1 = 0;
            int nTime2 = 0;
            string strOK = "";
            string strData = "";
            int nJumSu = 0;

            bool b자살생각 = false;
            string[] strGbSlip = new string[3];
            string Fstr인지능력 = "";
            string strFileName = "";

            string strE512 = "";
            string strE513 = "";
            //2019년도 생활습관평가

            string sJepDate = "";

            strE512 = "";
            strE513 = "";
            strJong = VB.Left(cboJong.Text, 1);
            strkiho = "37100068";
            strChungDate = argDATE;
            strChungDate1 = VB.Left(cboYear.Text, 2) + argDATE;

            //청구서 생성
            //청구내역읽음
            chb.READ_HIC_MIR_TBOHUM(argMirno);

            HIC_MIR_BOHUM list = hicMirBohumService.GetItemByMirno(argMirno);

            strGubun = "OX";    //C형간염 OX
            if (argMirGbn == "Y")
            {
                strGubun = "B1";    //추가청구구분 건강검진 01
            }

            //생애-성인병-급여만
            strBogunso = "";
            if (clsHcType.TMB.GbJohap == "4" && clsHcType.TMB.Johap == "J")
            {
                strBogunso = hicJepsuService.GetBogunsobyJepDateMirNo(clsHcType.TMB.FrDate, clsHcType.TMB.ToDate, argMirno, "1");
            }

            //순번확인
            nCnt += 1;
            if (list.CHASU == "Y")
            {
                strChasu = "12";
            }
            else
            {
                strChasu = "11";
            }

            sJepDate = cboYear.Text + "-01-01";
            List<string> strExCodeList = new List<string>();
            strExCodeList.Clear();
            strExCodeList.Add("E512");
            strExCodeList.Add("E513");

            //파일생성
            strFileName = argPath + strkiho + "_" + VB.Right(cboYear.Text, 2) + "01" + argJohap + "_" + strGubun + strChasu + "_" + string.Format("{0:000000}", argDATE) + "_" + string.Format("{0:00}", nCnt) + ".txt";

            StreamWriter sw = new StreamWriter(new FileStream(strFileName, FileMode.Create));

            FstrREC.Clear();
            FstrREC.Append("[");

            //1차 건강검진결과(SAM)
            List<HIC_JEPSU_PATIENT> list2 = hicJepsuPatientService.GetItembyJepDateMirNo1(sJepDate, argMirno);

            nREAD = list2.Count;
            for (int i = 0; i < nREAD; i++)
            {
                nWRTNO = list2[i].WRTNO;

                nAge = list2[i].AGE;
                strSex = list2[i].SEX;
                strLtdCode = list2[i].LTDCODE.To<string>();
                strGkiho = list2[i].GKIHO.Replace("-", "");
                strPtno = list2[i].PTNO;
                strGunDate = VB.Left(list2[i].JEPDATE, 4) + VB.Mid(list2[i].JEPDATE, 6, 2) + VB.Right(list2[i].JEPDATE, 2);
                strPandate = list2[i].PANJENGDATE.To<string>().Replace("-", "");
                strTongDate = list2[i].TONGBODATE.Replace("-", "");

                List<HIC_RESULT> list3 = hicResultService.GetCorrectedEyeSightbyWrtNoExCode(nWRTNO, strExCodeList);

                if (list3.Count > 0)
                {
                    for (int j = 0; j < list3.Count; j++)
                    {
                        if (list3[j].EXCODE == "E512")
                        {
                            strE512 = list3[j].RESULT;
                        }
                        else if (list3[i].EXCODE == "E513")
                        {
                            strE513 = list3[j].RESULT;
                        }
                    }
                }

                FstrREC.Append("{" + "" + (char)34 + "ITEM0" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");   //청구파일 구분
                FstrREC.Append("" + (char)34 + "ITEM1" + (char)34 + ":" + (char)34 + VB.Left(cboYear.Text + VB.Space(4), 4) + (char)34 + ",");                                  //사업년도
                FstrREC.Append("" + (char)34 + "ITEM2" + (char)34 + ":" + (char)34 + VB.Left(clsAES.DeAES(list2[i].JUMIN2) + VB.Space(13), 13) + (char)34 + ",");      //주민번호
                FstrREC.Append("" + (char)34 + "ITEM3" + (char)34 + ":" + (char)34 + VB.Left(strGkiho + VB.Space(11), 11) + (char)34 + ",");                                      //증번호
                FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");

                //C형선별검사
                FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + "2" + (char)34 + ","); //1:C형간염항체(일반), 2:C형간염항체(정밀), 3:핵의학적방법(정밀)
                if (string.Compare(strE512, "1") < 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + "1" + (char)34 + ","); //1:음성 2:양성
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + "2" + (char)34 + ","); //1:음성 2:양성
                }

                strE512 = (strE512.To<int>() * 100).To<string>();
                FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + string.Format("{0:00000}", strE512) + (char)34 + ","); //수치입력(검사방법이 2,3일 경우만 입력, 소수점 제외, 00000~99999)

                if (strE513.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + "3" + (char)34 + ","); //1:정성그룹 2, 2:정성그룹 3, 3:정량그룹 2
                    if (string.Compare(strE513, "15") < 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + "1" + (char)34 + ","); //1:음성 2:양성
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");//1:음성 2:양성
                    }

                    strE513 = (strE513.To<int>() * 1).To<string>();
                    FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + string.Format("{0:000000000}", strE513) + (char)34 + ",");    //수치입력(검사방법이 2,3일 경우만 입력, 소수점 제외, 00000~99999)
                }

                strE512 = "";
                strE513 = "";

                //종합판정
                FstrREC.Append("" + (char)34 + "ITEM11" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM12" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");

                FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + VB.Left(strGunDate + VB.Space(8), 8) + (char)34 + ",");    //검진일자
                FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + VB.Left(strPandate + VB.Space(8), 8) + (char)34 + ",");    //판정일자

                if (list2[i].GBCHUL == "Y" && list2[i].GBCHUL2.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + "1" + (char)34 + ","); //검진장소구분(1-출장,2내원)
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                if (list2[i].LTDCODE != 0 && !list2[i].LTDCODE.IsNullOrEmpty())
                {
                    hb.READ_Ltd_Name(string.Format("{0:#}", list2[i].LTDCODE));
                    if (clsHcVariable.GstrLtdJuso != list2[i].JUSOAA)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");    //회사코드와 접수시 주소가 다르면 개인으로
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");    //회사코드 없으면 개인으로
                }

                FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + VB.Left(strTongDate + VB.Space(8), 8) + (char)34 + ",");  //통보일자

                if (!list2[i].PANJENGDRNO.IsNullOrEmpty() && list2[i].PANJENGDRNO != 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + VB.Left(list2[i].PANJENGDRNO + VB.Space(10), 10) + (char)34 + ",");    //의사면허
                    FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(list2[i].PANJENGDRNO.To<string>()) + VB.Space(12), 12) + (char)34 + ",");  //의사성명
                    FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN(list2[i].PANJENGDRNO.To<string>()) + VB.Space(13), 13) + (char)34 + ","); //의사주민번호
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + VB.Space(95) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + "E" + (char)34 + "},");

                sw.WriteLine(FstrREC);
                FstrREC.Clear();
            }

            //==================================================================================================================================================================================
            //-----------------------------
            //1,2차 문진내용(SAM)
            //-----------------------------
            List<HIC_JEPSU_PATIENT> list4 = hicJepsuPatientService.GetFirstMunbyJepDateMirNo1(sJepDate, argMirno);

            nRead2 = list4.Count;

            for (int i = 0; i < nRead2; i++)
            {
                nWRTNO = list4[i].WRTNO;
                if (VB.Mid(list4[i].GKIHO, 2, 1) == "-")
                {
                    strGkiho = VB.Mid(list4[i].GKIHO, 1, 1) + VB.Mid(list4[i].GKIHO, 3, 9) + VB.Mid(list4[i].GKIHO, 1, 1) + VB.Right(list4[i].GKIHO, 1);
                }
                else
                {
                    strGkiho = "";
                }
                strLtdCode = list4[i].LTDCODE.To<string>();

                FstrREC.Append("{" + "" + (char)34 + "ITEM0" + (char)34 + ":" + (char)34 + "4" + (char)34 + ",");                                                   //청구파일 구분)
                FstrREC.Append("" + (char)34 + "ITEM1" + (char)34 + ":" + (char)34 + VB.Left(cboYear.Text + VB.Space(4), 4) + (char)34 + ",");                      //사업년도
                FstrREC.Append("" + (char)34 + "ITEM2" + (char)34 + ":" + (char)34 + VB.Left(clsAES.DeAES(list4[i].JUMIN2) + VB.Space(13), 13) + (char)34 + ",");   //주민번호
                FstrREC.Append("" + (char)34 + "ITEM3" + (char)34 + ":" + (char)34 + VB.Left(strGkiho + VB.Space(11), 11) + (char)34 + ",");                        //증번호

                if (VB.L(list4[i].TEL, "-") - 1 == 2)
                {
                    FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list4[i].TEL, "-", 1) + VB.Space(4), 4) + (char)34 + ","); //전화 지역
                    FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list4[i].TEL, "-", 2) + VB.Space(4), 4) + (char)34 + ","); //전화 국번
                    FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list4[i].TEL, "-", 3) + VB.Space(4), 4) + (char)34 + ","); //전화 번호
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                }

                if (VB.L(list4[i].HPHONE, "-") - 1 == 2)
                {
                    FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list4[i].HPHONE, "-", 1) + VB.Space(4), 4) + (char)34 + ",");   //핸드폰 지역
                    FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list4[i].HPHONE, "-", 2) + VB.Space(4), 4) + (char)34 + ",");   //핸드폰 국번
                    FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list4[i].HPHONE, "-", 3) + VB.Space(4), 4) + (char)34 + ",");   //핸드폰 번호
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                }

                if (!list4[i].EMAIL.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(list4[i].EMAIL, 40) + VB.Space(40), 40) + (char)34 + ",");        //전자우편주소
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + VB.Space(40) + (char)34 + ",");
                }
                FstrREC.Append("" + (char)34 + "ITEM11" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");          //통보서 수신방법(1:우편, 2:이메일)

                //C형문진표내용
                FstrREC.Append("" + (char)34 + "ITEM12" + (char)34 + ":" + (char)34 + list4[i].MUN0105 + (char)34 + ",");

                FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + list4[i].TMUN0106 + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + list4[i].TMUN0107 + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + VB.Left(list4[i].TMUN0108 + VB.Space(4), 4) + (char)34 + ",");
                if (list4[i].TMUN0107 == "2")
                {
                    FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + list4[i].TMUN0109 + (char)34 + ",");
                    if (list4[i].TMUN0110 == "2")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }

                    if (list4[i].TMUN0111 == "2")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    if (list4[i].TMUN0112 == "2")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    if (list4[i].TMUN0113 == "2")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                }

                if (list4[i].TMUN0107 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + "" + (char)34 + ",");
                }
                else
                {
                    if (list4[i].TMUN0114 == "2")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    if (list4[i].TMUN0115 == "2")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    if (list4[i].TMUN0116 == "2")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    if (list4[i].TMUN0117 == "2")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    if (list4[i].TMUN0118 == "2")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + VB.Left(list4[i].TMUN0119 + VB.Space(4), 4) + (char)34 + ",");


                    if (list4[i].TMUN0120 == "2")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    if (list4[i].TMUN0121 == "2")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    if (list4[i].TMUN0122 == "2")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                }

                FstrREC.Append("" + (char)34 + "ITEM30" + (char)34 + ":" + (char)34 + VB.Left(list4[i].TMUN0123 + VB.Space(4), 4) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + list4[i].TMUN0124 + (char)34 + ",");

                //2020 신규자료
                FstrREC.Append("" + (char)34 + "ITEM32" + (char)34 + ":" + (char)34 + VB.Left(list4[i].MAILCODE + VB.Space(5), 5) + (char)34 + ",");          //우편번호
                FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + VB.Left(list4[i].JUSO1 + VB.Space(200), 200) + (char)34 + ",");         //주소
                FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + VB.Left(list4[i].JUSO2 + VB.Space(200), 200) + (char)34 + ",");         //상세주소


                FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");

                if (i == nREAD)
                {
                    FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + "E" + (char)34 + "}");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + "E" + (char)34 + "},");
                }

                FstrREC.Append("]");
                sw.WriteLine(FstrREC);
                FstrREC.Clear();
            }

            sw.Close();

            strFileName = "";
            strFileName = strkiho + "_" + VB.Right(cboYear.Text, 2) + "01" + argJohap + "_" + strGubun + strChasu + "_" + string.Format("{0:000000}", argDATE) + "_" + string.Format("{0:00}", nCnt);

            string strJepNo = VB.Mid(clsPublic.GstrSysDate, 2, 2) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Right(clsPublic.GstrSysDate, 2) + string.Format("{0:00}", nCnt);

            clsDB.setBeginTran(clsDB.DbCon);
            //청구완료 DB에 UPDATE
            int result = hicMirBohumService.UpdateJepNoFileNameJepDatebyMirNo(strJepNo, strFileName, argMirno);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_MIR_HOHUM에 청구번호 UPDATE시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void fn_HIC_MIR_Cancer_Save()
        {
            string strErrChk = "";
            long nCnt = 0;
            long nTotAmt = 0;
            int nREAD = 0;
            int nRead1 = 0;
            string strResult = "";
            long nWRTNO = 0;
            long nBogenAmt = 0;

            int Inwon01 = 0; //위암-위장조영촬영
            int Inwon02 = 0;    //위암-상부소화관
            int Inwon03 = 0;    //위암-조직검사 1-3
            int Inwon04 = 0;    //위암-조직검사
            int Inwon05 = 0;    //위암-조직검사
            int Inwon06 = 0;    //위암-조직검사
            int Inwon07 = 0;    //위암-조직검사
            int Inwon27 = 0;    //위암-포셉사용

            int Inwon08 = 0;    //간암-의료급여-ALT
            int Inwon09 = 0;    //간암-의료급여-B형간염항원-일반
            int Inwon10 = 0;    //간암-의료급여-C형간염항체-일반
            int Inwon11 = 0;    //간암-간초음파검사
            int Inwon12 = 0;    //간암-혈청알파태아단백-RPHA
            int Inwon13 = 0;    //간암-혈청알파태아단백-EIA

            int Inwon14 = 0;    //대장암-분변잠혈반응검사-RPHA
            int Inwon15 = 0;    //대장암-분변잠혈반응검사-분변혈색소정량법
            int Inwon16 = 0;    //대장암-대장이중조영검사
            int Inwon17 = 0;    //대장암-대장내시경검사
            int Inwon18 = 0;    //대장암-조직검사 1-3
            int Inwon19 = 0;    //대장암-조직검사
            int Inwon20 = 0;    //대장암-조직검사
            int Inwon21 = 0;    //대장암-조직검사
            int Inwon22 = 0;    //대장암-조직검사
            int Inwon28 = 0;    //대장암-포셉사용

            int Inwon23 = 0;    //유방암-유방촬영
            int Inwon24 = 0;    //자궁경부암-자궁경부세포검사
            int Inwon26 = 0;    //토요일,공휴일가산
            int Inwon29 = 0;    //폐암- 저선량흉부CT
            int Inwon30 = 0;    //폐암- 사후결과상담
            long nTotAmt1 = 0;
            long nJohapAmt = 0;
            long nChaAmt = 0;
            long[] nAmt = new long[51];
            long nCnt1 = 0;         //상담료
            long nAmtTotal = 0;
            string strSelf = "";
            string strChk = "";
            int[] nJin_New = new int[7]; //진촬상담여부 - 위.대장.간.유방.자궁.폐
            int[] nTot_Jin = new int[7];
            string strLife_Gbn = "";       //생애구분
            string strDate = "";
            string strGbSelf = "";
            string str3168 = "";
            string sJepDate = "";

            List<string> sExCodes = new List<string>();

            List<HIC_JEPSU> lstJepsu = new List<HIC_JEPSU>();

            nTotAmt = 0;
            strErrChk = "";
            Inwon01 = 0; Inwon02 = 0; Inwon03 = 0; Inwon04 = 0; Inwon05 = 0; Inwon06 = 0; Inwon07 = 0;
            Inwon08 = 0; Inwon09 = 0; Inwon10 = 0; Inwon11 = 0; Inwon12 = 0; Inwon13 = 0; Inwon14 = 0;
            Inwon15 = 0; Inwon16 = 0; Inwon17 = 0; Inwon18 = 0; Inwon19 = 0; Inwon20 = 0; Inwon21 = 0;
            Inwon22 = 0; Inwon23 = 0; Inwon24 = 0; Inwon26 = 0; Inwon27 = 0; Inwon28 = 0; Inwon29 = 0; Inwon30 = 0;
            nAmtTotal = 0; nCnt = 0; nCnt1 = 0; strChk = "J";
            str3168 = "";
            Fn절사금액합계1 = 0;

            for (int i = 0; i <= 50; i++)
            {
                nAmt[i] = 0;
            }

            HIC_MIR_CANCER list = hicMirCancerService.GetItembyMirno(FnMirNo);

            if (list.GBBOGUN == "E" || list.JOHAP == "X")
            {
                lstJepsu = hicJepsuService.GetWrtNobyMirno(FnMirNo, "5");
            }
            else if (list.GBBOGUN != "E")
            {
                lstJepsu = hicJepsuService.GetWrtNobyMirno(FnMirNo, "3");
            }

            strLife_Gbn = "";
            if (list.LIFE_GBN == "Y")
            {
                strLife_Gbn = "Y";
            }

            nCnt = lstJepsu.Count;

            //보건소암체크
            chb.READ_HIC_MIR_CANCER_Bo(FnMirNo);

            for (int j = 0; j <= 5; j++)
            {
                nJin_New[j] = 0;
            }

            for (int j = 0; j <= 50; j++)
            {
                nAmt[j] = 0;
            }

            for (int i = 0; i < nCnt; i++)
            {
                nWRTNO = lstJepsu[i].WRTNO;

                List<HIC_RESULT> list2 = hicResultService.GetItembyOnlyWrtNo(nWRTNO);

                nRead1 = list2.Count;
                strChk = "J";
                for (int j = 0; j <= 6; j++)
                {
                    nTot_Jin[j] = 0;
                }

                chb.READ_HIC_CANCER_NEW(nWRTNO); //접수번호에 대한 암검사 종류 읽음

                sExCodes.Clear();
                sExCodes.Add("TX22");

                //위조영
                if (hicResultService.GetCountbyWrtNoInExCode(nWRTNO, sExCodes) > 0 && (clsHcType.B3.PANJENGDRNO1 == "0" || clsHcType.B3.PANJENGDRNO1.IsNullOrEmpty()))
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "위조영 판독의사 입력을 확인하세요.");
                }

                //위내시경
                sExCodes.Clear();
                sExCodes.Add("TX20");
                sExCodes.Add("TX23");
                sExCodes.Add("TX41");

                if (hicResultService.GetCountbyWrtNoInExCode(nWRTNO, sExCodes) > 0 &&
                    (clsHcType.B3.PANJENGDRNO1 == "0" || clsHcType.B3.PANJENGDRNO1.IsNullOrEmpty()))
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "위내시경 판독의사 입력을 확인하세요.");
                }
                if (hicResultService.GetCountbyWrtNoInExCode(nWRTNO, sExCodes) > 0 && !clsHcType.B3.RESULT0001.IsNullOrEmpty() &&
                    (clsHcType.B3.PANJENGDRNO3 == "0" || clsHcType.B3.PANJENGDRNO3.IsNullOrEmpty()))
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "위내시경 병리진단의사 입력을 확인하세요.");
                }

                //대장내시경
                sExCodes.Clear();
                sExCodes.Add("TX32");
                sExCodes.Add("TX64");
                sExCodes.Add("TX41");

                if (hicResultService.GetCountbyWrtNoInExCode(nWRTNO, sExCodes) > 0 &&
                    (clsHcType.B3.PANJENGDRNO4 == "0" || clsHcType.B3.PANJENGDRNO4.IsNullOrEmpty()) &&
                    (clsHcType.B3.PANJENGDRNO5 == "0" || clsHcType.B3.PANJENGDRNO5.IsNullOrEmpty()) &&
                    (clsHcType.B3.PANJENGDRNO6 == "0" || clsHcType.B3.PANJENGDRNO6.IsNullOrEmpty()))
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "위내시경 판독의사 입력을 확인하세요.");
                }

                if (clsHcType.B3.GbLiver == "1" && (clsHcType.B3.PANJENGDRNO7 == "0" || clsHcType.B3.PANJENGDRNO7.IsNullOrEmpty()))
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "간암 검사의사 입력을 확인하세요.");
                }

                if (clsHcType.B3.GBBREAST == "1" && (clsHcType.B3.PANJENGDRNO8 == "0" || clsHcType.B3.PANJENGDRNO8.IsNullOrEmpty()))
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "유방암 판독의사 입력을 확인하세요.");
                }

                if (clsHcType.B3.GbWomb == "1" && (clsHcType.B3.PANJENGDRNO9 == "0" || clsHcType.B3.PANJENGDRNO9.IsNullOrEmpty()) && (clsHcType.B3.PANJENGDRNO10 == "0" || clsHcType.B3.PANJENGDRNO10.IsNullOrEmpty()))
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "자궁경부암 검체채취,병리진단의사 입력을 확인하세요.");
                }

                //2018-08-22 (상담비 90,100% 구분조건)
                if (!clsHcType.B3.PanDrNo_New1.IsNullOrEmpty() && VB.Mid(clsHcType.B3.Can_MirGbn, 1, 1) != "1")
                {
                    nTot_Jin[1] = 1;
                    nJin_New[1] += 1; //위
                }

                if (!clsHcType.B3.PanDrNo_New2.IsNullOrEmpty() && VB.Mid(clsHcType.B3.Can_MirGbn, 2, 1) != "1")
                {
                    nTot_Jin[2] = 1;
                    nJin_New[2] += 1; //대장
                }

                if (!clsHcType.B3.PanDrNo_New3.IsNullOrEmpty() && VB.Mid(clsHcType.B3.Can_MirGbn, 3, 1) != "1")
                {
                    nTot_Jin[3] = 1;
                    nJin_New[3] += 1; //간
                }

                if (!clsHcType.B3.PanDrNo_New4.IsNullOrEmpty() && VB.Mid(clsHcType.B3.Can_MirGbn, 4, 1) != "1")
                {
                    nTot_Jin[4] = 1;
                    nJin_New[4] += 1; //유방
                }

                if (!clsHcType.B3.PanDrNo_New5.IsNullOrEmpty() && VB.Mid(clsHcType.B3.Can_MirGbn, 5, 1) != "1")
                {
                    nTot_Jin[5] = 1;
                    nJin_New[5] += 1; //자궁
                }

                if (!clsHcType.B3.NEW_WOMAN37.IsNullOrEmpty() && VB.Mid(clsHcType.B3.Can_MirGbn, 6, 1) != "1")
                {
                    nTot_Jin[6] = 1;
                    nJin_New[6] += 1; //폐
                }

                str3168 = "";
                //분변검사만 제출시 상담료0원
                if (nTot_Jin[2] == 1)
                {
                    if (hicSunapdtlService.GetCountbyWrtNoCode(nWRTNO, "3168") > 0)
                    {
                        nJin_New[2] = 0;
                        str3168 = "Y";
                    }
                }

                if (nRead1 > 0)
                {
                    for (int j = 0; j < nRead1; j++)
                    {
                        strResult = list2[j].RESULT;

                        if (clsHcType.B3.GbLiver != "1")
                        {
                            switch (list2[j].EXCODE)
                            {
                                case "TX09":
                                case "TX10":
                                case "TX27":
                                    strResult = "";
                                    break;
                                default:
                                    break;
                            }
                        }

                        if (VB.Left(cboJong.Text, 1) == "4")    //공단암
                        {
                            //접수시 수동 회사100% 일경우 항목 제외
                            List<HIC_RESULT_SUNAPDTL> list3 = hicResultSunapdtlService.GetItembyWrtNoExCode(nWRTNO, list2[j].EXCODE);

                            if (list3.Count > 0)
                            {
                                for (int k = 0; k < list3.Count; k++)
                                {
                                    if (!list3[k].CODE.IsNullOrEmpty())
                                    {
                                        strResult = "";
                                    }
                                }
                            }

                            //대장암일경우(나이로 체크 만50세 이상만)
                            if (strResult != "미실시")
                            {
                                if (list2[j].EXCODE == "TX26" && chb.READ_PATIENT_AGE(nWRTNO) < 50)
                                {
                                    strResult = "";
                                }
                            }
                        }

                        //판정화면에서 청구제외는 포함안함    2012-01-30  KMC
                        for (int k = 0; k <= 6; k++)
                        {
                            if (VB.Mid(clsHcType.B3.Can_MirGbn, k + 1, 1) == "1")
                            {
                                if (k == 0) //위암
                                {
                                    switch (list2[j].EXCODE)
                                    {
                                        case "TX22":        //위암-위장조영검사
                                            strResult = "";
                                            break;
                                        case "TX23":        //위암-위내시경검사
                                            strResult = "";
                                            break;
                                        case "TX45":        //위암-조직검사 1-3
                                            strResult = "";
                                            break;
                                        case "TX46":        //위암-조직검사
                                            strResult = "";
                                            break;
                                        case "TX47":        //위암-조직검사
                                            strResult = "";
                                            break;
                                        case "TX48":        //위암-조직검사
                                            strResult = "";
                                            break;
                                        case "TX49":        //위암-조직검사
                                            strResult = "";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else if (k == 1)    //대장
                                {
                                    switch (list2[j].EXCODE)
                                    {
                                        case "TX26":            //대장암-분변잠혈-RPHA
                                            strResult = "";
                                            break;
                                        case "TX31":            //대장암-대장이중조영검사
                                            strResult = "";
                                            break;
                                        case "TX32":            //대장암-대장내시경검사
                                            strResult = "";
                                            break;
                                        case "TX21":            //대장암-조직검사 1-3
                                            strResult = "";
                                            break;
                                        case "TX71":            //대장암-조직검사
                                            strResult = "";
                                            break;
                                        case "TX72":            //대장암-조직검사
                                            strResult = "";
                                            break;
                                        case "TX73":            //대장암-조직검사
                                            strResult = "";
                                            break;
                                        case "TX74":            //대장암-조직검사
                                            strResult = "";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else if (k == 2)    //간암
                                {
                                    switch (list2[j].EXCODE)
                                    {
                                        case "A125":            //간암-의료급여-ALT
                                            strResult = "";
                                            break;
                                        case "A258":            //간암-의료급여-B형간염항원-정밀 2010
                                            strResult = "";
                                            break;
                                        case "E508":            //간암-의료급여-C형간염항체-정밀 2010
                                            strResult = "";
                                            break;
                                        case "TX09":            //간암-간초음파
                                            strResult = "";
                                            break;
                                        case "TX10":            //간암-간초음파
                                            strResult = "";
                                            break;
                                        case "TX27":            //간암-간초음파
                                            strResult = "";
                                            break;
                                        case "A264":            //간암-혈청알파태아단백-EIA
                                            strResult = "";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else if (k == 3)    //유방암
                                {
                                    switch (list2[j].EXCODE)
                                    {
                                        case "TX29":            //유방암-유방촬영
                                            strResult = "";
                                            break;
                                        case "TY16":            //유방암-편측촬영
                                            strResult = "";
                                            break;
                                        case "TY17":            //유방암-편측촬영
                                            strResult = "";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else if (k == 4)    //자궁암
                                {
                                    switch (list2[j].EXCODE)
                                    {
                                        case "A171":            //자궁경부암-자궁경부세포검사
                                            strResult = "";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else if (k == 5)    //폐암
                                {
                                    switch (list2[j].EXCODE)
                                    {
                                        case "TY10":            //폐암-자궁경부세포검사
                                            strResult = "";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }

                        List<HIC_RESULT_SUNAPDTL> list4 = hicResultSunapdtlService.GetCodebyWrtNo(nWRTNO);

                        if (list4.Count > 0)
                        {
                            for (int k = 0; k < list4.Count; k++)
                            {
                                if (!list4[k].CODE.IsNullOrEmpty())
                                {
                                    strResult = "";
                                }
                            }
                        }

                        if (!strResult.IsNullOrEmpty() && strResult != "." && strResult != "결과없음" && strResult != "미실시")
                        {
                            switch (list2[j].EXCODE)
                            {
                                case "TX22":            //위암-위장조영검사
                                    Inwon01 += 1;
                                    nAmt[1] += 1;
                                    strChk = "Y";
                                    break;
                                case "TX23":            //위암-위내시경검사
                                    Inwon02 += 1;
                                    nAmt[2] += 1;
                                    strChk = "Y";
                                    break;
                                case "TX45":            //위암-조직검사 1-3
                                    Inwon03 += 1;
                                    nAmt[3] += 1;
                                    strChk = "Y";
                                    break;
                                case "TX46":            //위암-조직검사
                                    Inwon04 += 1;
                                    nAmt[4] += 1;
                                    strChk = "Y";
                                    break;
                                case "TX47":            //위암-조직검사
                                    Inwon05 += 1;
                                    nAmt[5] += 1;
                                    strChk = "Y";
                                    break;
                                case "TX48":            //위암-조직검사
                                    Inwon06 += 1;
                                    nAmt[6] += 1;
                                    strChk = "Y";
                                    break;
                                case "TX49":            //위암-조직검사
                                    Inwon07 += 1;
                                    nAmt[7] += 1;
                                    strChk = "Y";
                                    break;
                                case "TX26":            //대장암-분변잠혈-정량법
                                    Inwon15 += 1;
                                    nAmt[15] += 1;
                                    break;
                                case "TX31":            //대장암-분변잠혈-정량법
                                    Inwon16 += 1;
                                    nAmt[16] += 1;
                                    break;
                                case "TX32":            //대장암-대장내시경검사
                                    Inwon17 += 1;
                                    nAmt[17] += 1;
                                    break;
                                case "TX21":            //대장암-조직검사 1-3
                                    Inwon18 += 1;
                                    nAmt[18] += 1;
                                    break;
                                case "TX71":            //대장암-조직검사
                                    Inwon19 += 1;
                                    nAmt[19] += 1;
                                    break;
                                case "TX72":            //대장암-조직검사
                                    Inwon20 += 1;
                                    nAmt[20] += 1;
                                    break;
                                case "TX73":            //대장암-조직검사
                                    Inwon21 += 1;
                                    nAmt[21] += 1;
                                    strChk = "Y";
                                    break;
                                case "TX74":            //대장암-조직검사
                                    Inwon22 += 1;
                                    nAmt[22] += 1;
                                    strChk = "Y";
                                    break;
                                case "TX29":            //유방암-유방촬영
                                    Inwon23 += 1;
                                    nAmt[23] += 1;
                                    strChk = "Y";
                                    break;
                                case "TY16":            //유방암-편측촬영
                                    Inwon28 += 1;
                                    nAmt[28] += 1;
                                    strChk = "Y";
                                    break;
                                case "TY17":            //유방암-편측촬영
                                    Inwon28 += 1;
                                    nAmt[28] += 1;
                                    strChk = "Y";
                                    break;
                                case "A171":            //자궁경부암-자궁경부세포검사
                                    Inwon01 += 1;
                                    nAmt[24] += 1;
                                    break;
                                case "TY10":            //폐암-저선량륭부CT
                                    Inwon29 += 1;
                                    nAmt[29] += 1;
                                    break;
                                case "A312":            //폐암-사후결과상담
                                    Inwon30 += 1;
                                    nAmt[30] += 1;
                                    break;
                                default:
                                    break;
                            }

                            if (VB.Mid(clsHcType.B3.Can_MirGbn, 3, 1) != "1")
                            {
                                switch (list2[j].EXCODE)
                                {
                                    case "A125":        //간암-의료급여-ALT
                                        Inwon08 += 1;
                                        nAmt[8] += 1;
                                        strChk = "Y";
                                        break;
                                    case "A258":        //간암-의료급여-B형간염항원-정밀 2010
                                        Inwon09 += 1;
                                        nAmt[9] += 1;
                                        strChk = "Y";
                                        break;
                                    case "E508":        //간암-의료급여-C형간염항체-정밀 2010
                                        Inwon10 += 1;
                                        nAmt[10] += 1;
                                        strChk = "Y";
                                        break;
                                    case "TX09":        //간암-간초음파
                                        Inwon11 += 1;
                                        nAmt[11] += 1;
                                        strChk = "Y";
                                        break;
                                    case "TX10":        //간암-간초음파
                                        Inwon11 += 1;
                                        nAmt[11] += 1;
                                        strChk = "Y";
                                        break;
                                    case "TX27":        //간암-간초음파
                                        Inwon11 += 1;
                                        nAmt[11] += 1;
                                        strChk = "Y";
                                        break;
                                    case "A264":        //간암-혈청알파태아단백-EIA
                                        Inwon13 += 1;
                                        nAmt[13] += 1;
                                        strChk = "Y";
                                        //접수에는 항목이 있으나 판정에 없을경우 RPHA
                                        if (clsHcType.B3.Liver_EIA.IsNullOrEmpty() && clsHcType.B3.Liver_RPHA.IsNullOrEmpty())
                                        {
                                            Inwon13 -= 1;
                                            nAmt[13] -= 1;
                                            strChk = "";          //간암-EIA
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }

                //암검진 토.공휴일가산
                if (hicSunapdtlService.GetCountbyWrtNoCode(nWRTNO, "1119") > 0)
                {
                    Inwon26 += 1;
                    nAmt[26] += 1;
                }

                //보건소부담액을 읽음
                nBogenAmt = hicSunapService.GetBogenAmtbyWrtNo(nWRTNO);

                HIC_JEPSU_SUNAP list5 = hicJepsuSunapService.GetItembyWrtNoCode(nWRTNO, "3101");

                strDate = list5.JEPDATE;
                strGbSelf = list5.GBSELF;

                if (VB.Left(cboJong.Text, 1) != "E")
                {
                    if (strLife_Gbn.IsNullOrEmpty() || nBogenAmt > 0)
                    {
                        //공단암 + 보건소암 (90%수가)
                        nAmt[1] *= FnAmAmt2[1];      //위암-위장조영검사
                        nAmt[2] *= FnAmAmt2[2];      //위암-위내시경검사
                        //2015-09-01 포셉수가 22,000원 추가  //
                        nAmt[3] *= FnAmAmt2[3];      //위암-조직검사 1-3
                        nAmt[4] *= FnAmAmt2[4];      //위암-조직검사
                        nAmt[5] *= FnAmAmt2[5];      //위암-조직검사
                        nAmt[6] *= FnAmAmt2[6];      //위암-조직검사
                        nAmt[7] *= FnAmAmt2[7];      //위암-조직검사
                        nAmt[8] *= FnAmAmt2[8];      //간암-ALT
                        nAmt[11] *= FnAmAmt2[11];    //간암-간초음파
                        nAmt[13] *= FnAmAmt2[13];    //간암-EIA
                        nAmt[14] *= FnAmAmt2[14];    //대장암-RPHA
                        nAmt[15] *= FnAmAmt2[15];    //대장암-분변잠혈
                        nAmt[16] *= FnAmAmt2[16];    //대장암-대장이중조영검사
                        nAmt[17] *= FnAmAmt2[17];    //대장암-대장내시경
                        //2015-09-01 포셉수가 22,000원 추가  
                        nAmt[18] *= FnAmAmt2[18];    //대장암-조직검사 1-3
                        nAmt[19] *= FnAmAmt2[19];    //대장암-조직검사
                        nAmt[20] *= FnAmAmt2[20];    //대장암-조직검사
                        nAmt[21] *= FnAmAmt2[21];    //대장암-조직검사
                        nAmt[22] *= FnAmAmt2[22];    //대장암-조직검사
                        nAmt[23] *= FnAmAmt2[23];    //유방암-유방촬영
                        nAmt[24] *= FnAmAmt2[24];    //자궁경부암-자궁경부세포검사
                        //nAmt[26] *= FnAmAmt2[27]   //토요일,공휴일30 % 가산
                        nAmt[27] *= FnAmAmt2[28];    //조직검사용 포셉사용
                        nAmt[28] *= FnAmAmt2[29];    //유방암:편측촬영
                        nAmt[29] *= FnAmAmt2[30];    //폐암:저선량흉부CT
                        nAmt[30] *= FnAmAmt2[31];    //폐암:사후결과상담

                        if ((nTot_Jin[1] + nTot_Jin[2] + nTot_Jin[3] + nTot_Jin[4] + nTot_Jin[5] + nTot_Jin[6]) >= 1)
                        {
                            if (strChk == "Y" && (nTot_Jin[2] == 0 && nTot_Jin[5] == 0))
                            {
                                nAmt[25] = FnAmAmt2[25];              //상담료[90%]
                                nAmt[26] = nAmt[26] * FnAmAmt2[27];   //토요일,공휴일30%가산 [90%]
                            }
                            else
                            {
                                nAmt[25] = FnAmAmt1[25];              //상담료[대장암, 자궁경부암일경우만 100%]
                                nAmt[26] = nAmt[26] * FnAmAmt1[27];   //토요일,공휴일30%가산 [100%]
                            }
                            nCnt1 += 1;
                        }
                        else
                        {
                            nAmt[25] = 0;              //상담료
                        }
                    }
                    else
                    {
                        //생애면 100%
                        nAmt[1] *= FnAmAmt1[1];       //위암-위장조영검사
                        nAmt[2] *= FnAmAmt1[2];       //위암-위내시경검사
                        //2015-09-01 포셉수가 22,000원 추가   //
                        nAmt[3] *= FnAmAmt1[3];       //위암-조직검사 1-3
                        nAmt[4] *= FnAmAmt1[4];       //위암-조직검사
                        nAmt[5] *= FnAmAmt1[5];       //위암-조직검사
                        nAmt[6] *= FnAmAmt1[6];       //위암-조직검사
                        nAmt[7] *= FnAmAmt1[7];       //위암-조직검사
                        nAmt[8] *= FnAmAmt1[8];       //간암-ALT
                        nAmt[11] *= FnAmAmt1[11];     //감암-간초음파
                        nAmt[13] *= FnAmAmt1[13];     //간암-EIA
                        nAmt[14] *= FnAmAmt1[14];     //대장암-RPHA
                        nAmt[15] *= FnAmAmt1[15];     //대장암-분변잠혈
                        nAmt[16] *= FnAmAmt1[16];     //대장암-대장이중조영검사
                        nAmt[17] *= FnAmAmt1[17];     //대장암-대장내시경검사
                        //2015-09-01 포셉수가 22,000원 추가   //
                        nAmt[18] *= FnAmAmt1[18];     //대장암-조직검사 1-3
                        nAmt[19] *= FnAmAmt1[19];     //대장암-조직검사
                        nAmt[20] *= FnAmAmt1[20];     //대장암-조직검사
                        nAmt[21] *= FnAmAmt1[21];     //대장암-조직검사
                        nAmt[22] *= FnAmAmt1[22];     //대장암-조직검사
                        nAmt[23] *= FnAmAmt1[23];     //유방암-유방촬영
                        nAmt[24] *= FnAmAmt1[24];     //자궁경부암-자궁경부세포검사
                        nAmt[26] *= FnAmAmt1[27];     //토요일,공휴일30%가산
                        nAmt[27] *= FnAmAmt1[28];     //조직검사용 포셉사용
                        nAmt[28] *= FnAmAmt1[29];     //유방암:편측촬영
                        nAmt[29] *= FnAmAmt2[30];     //폐암:저선량흉부CT
                        nAmt[30] *= FnAmAmt2[31];     //폐암:사후결과상담

                        if ((nTot_Jin[1] + nTot_Jin[2] + nTot_Jin[3] + nTot_Jin[4] + nTot_Jin[5] + nTot_Jin[6]) >= 1)
                        {
                            nAmt[25] = FnAmAmt1[25];           //상담료(자궁경부암일경우만 100%)
                            nCnt1 += 1;
                        }
                        else
                        {
                            nAmt[25] = 0;                      //상담료
                        }
                    }
                }
                else
                {
                    //의료급여암 = 공단 100%
                    nAmt[1] *= FnAmAmt1[1];       //위암-위장조영검사
                    nAmt[2] *= FnAmAmt1[2];       //위암-위내시경검사
                    //2015-09-01 포셉수가 22,000원 추가
                    nAmt[3] *= FnAmAmt1[3];       //위암-조직검사 1-3
                    nAmt[4] *= FnAmAmt1[4];       //위암-조직검사
                    nAmt[5] *= FnAmAmt1[5];       //위암-조직검사
                    nAmt[6] *= FnAmAmt1[6];       //위암-조직검사
                    nAmt[7] *= FnAmAmt1[7];       //위암-조직검사
                    nAmt[8] *= FnAmAmt1[8];       //간암-ALT
                    nAmt[9] *= FnAmAmt1[9];       //간암-의료급여-B형간염항원-일반
                    nAmt[11] *= FnAmAmt1[11];     //감암-간초음파
                    nAmt[13] *= FnAmAmt1[13];     //간암-EIA
                    nAmt[14] *= FnAmAmt1[14];     //대장암-RPHA
                    nAmt[15] *= FnAmAmt1[15];     //대장암-분변잠혈
                    nAmt[16] *= FnAmAmt1[16];     //대장암-대장이중조영검사
                    nAmt[17] *= FnAmAmt1[17];     //대장암-대장내시경검사
                    //2015-09-01 포셉수가 22,000원 추가   
                    nAmt[18] *= FnAmAmt1[18];     //대장암-조직검사 1-3
                    nAmt[19] *= FnAmAmt1[19];     //대장암-조직검사
                    nAmt[20] *= FnAmAmt1[20];     //대장암-조직검사
                    nAmt[21] *= FnAmAmt1[21];     //대장암-조직검사
                    nAmt[22] *= FnAmAmt1[22];     //대장암-조직검사
                    nAmt[23] *= FnAmAmt1[23];     //유방암-유방촬영
                    nAmt[24] *= FnAmAmt1[24];     //자궁경부암-자궁경부세포검사
                    nAmt[26] *= FnAmAmt1[27];     //토요일,공휴일30%가산
                    nAmt[27] *= FnAmAmt1[28];     //조직검사용 포셉사용
                    nAmt[28] *= FnAmAmt1[29];     //유방암:편측촬영
                    nAmt[29] *= FnAmAmt2[30];     //폐암:저선량흉부CT
                    nAmt[30] *= FnAmAmt2[31];     //폐암:사후결과상담

                    //2018-09-05 위의 코드 수정
                    if ((nTot_Jin[1] + nTot_Jin[2] + nTot_Jin[3] + nTot_Jin[4] + nTot_Jin[5]) >= 1)
                    {
                        nCnt1 += 1;

                        if (str3168 == "Y" && (nTot_Jin[1] + nTot_Jin[3] + nTot_Jin[4] + nTot_Jin[5] + nTot_Jin[6]) == 0)
                        {
                            nAmt[25] = 0;              //상담료
                        }
                        else
                        {
                            nAmt[25] = FnAmAmt1[25];           //상담료(자궁경부암일경우만 100%)
                            nCnt1 = nCnt1 + 1;
                        }
                    }
                    else
                    {
                        nAmt[25] = 0;              //상담료
                    }
                }

                nTotAmt1 = nAmt[1] + nAmt[2] + nAmt[3] + nAmt[4] + nAmt[5] + nAmt[6] + nAmt[7] + nAmt[8] + nAmt[9] + nAmt[10] + nAmt[11] + nAmt[12] + nAmt[13] + nAmt[14] + nAmt[15] + nAmt[16] + nAmt[17] + nAmt[18] + nAmt[19] + nAmt[20] + nAmt[21] + nAmt[22] + nAmt[23] + nAmt[24] + nAmt[25] + nAmt[26] + nAmt[27] + nAmt[28] + nAmt[29] + nAmt[30];
                nAmtTotal += nTotAmt1;

                sJepDate = cboYear.Text + "-01-01";

                if (chkchk.Checked == false)
                {
                    HIC_JEPSU_SUNAP list6 = hicJepsuSunapService.GetItembyJepDateWrtNo(nWRTNO, sJepDate);

                    if (VB.Left(cboJong.Text, 1) == "4")
                    {
                        nJohapAmt = list6.JOHAPAMT;
                    }
                    else if (VB.Left(cboJong.Text, 1) == "E")
                    {
                        nJohapAmt = list6.BOGENAMT;
                    }

                    nChaAmt = nTotAmt1 - nJohapAmt;
                    Fn절사금액합계1 = Fn절사금액합계1 + nChaAmt;
                    FstrSname = list6.SNAME;

                    if (nChaAmt != 0)
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", nChaAmt + "￦ 암검진 청구비용 차액발생");
                    }

                    if (!list6.BOGUNSO.IsNullOrEmpty() && list6.MURYOAM == "N" || list6.MURYOAM.IsNullOrEmpty())
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "보건소번호가있고, 무료암대상이 비대상자 입니다.");
                    }
                    else if (list6.BOGUNSO.IsNullOrEmpty() && list6.MURYOAM == "Y")
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", "보건소번호가없고, 무료암대상이 대상자 입니다.");
                    }

                    FnAge = 0;
                    FstrSex = "";
                }
                nChaAmt = 0;
                nTotAmt1 = 0;
                nJohapAmt = 0;
                for (int j = 0; j <= 50; j++)
                {
                    nAmt[j] = 0;
                }
            }

            txtGongWon.Text = Fn절사금액합계1.To<string>();

            //암검진 공단암 금액 조회
            if (VB.Left(cboJong.Text, 1) == "4" || VB.Left(cboJong.Text, 1) == "E")
            {
                nTotAmt = hicSunapService.GetJohapAmtbyMirNo(FnMirNo, VB.Left(cboJong.Text, 1));
            }

            List<COMHPC> list7 = comHpcLibBService.GetCountHicMirErrorbyMirNo(FnMirNo);

            if (list7.Count > 0)
            {
                strErrChk = "N";
            }
            else
            {
                strErrChk = "Y";
            }
            nREAD = list7.Count;

            clsDB.setBeginTran(clsDB.DbCon);

            if (chkchk.Checked == false)
            {
                nAmtTotal = Math.Truncate(nAmtTotal.To<decimal>()).To<long>(); //금액을 원단위 절사함.

                if (!clsHcType.B3.ROWID.IsNullOrEmpty())
                {
                    if (!strErrChk.IsNullOrEmpty())
                    {
                        HIC_MIR_CANCER item = new HIC_MIR_CANCER();

                        item.GBERRCHK = strErrChk;
                        item.INWON01 = Inwon01;
                        item.INWON02 = Inwon02;
                        item.INWON03 = Inwon03;
                        item.INWON04 = Inwon04;
                        item.INWON05 = Inwon05;
                        item.INWON06 = Inwon06;
                        item.INWON07 = Inwon07;
                        item.INWON08 = Inwon08;
                        item.INWON09 = Inwon09;
                        item.INWON10 = Inwon10;
                        item.INWON11 = Inwon11;
                        item.INWON12 = Inwon12;
                        item.INWON13 = Inwon13;
                        item.INWON14 = Inwon14;
                        item.INWON15 = Inwon15;
                        item.INWON16 = Inwon16;
                        item.INWON17 = Inwon17;
                        item.INWON18 = Inwon18;
                        item.INWON19 = Inwon19;
                        item.INWON20 = Inwon20;
                        item.INWON21 = Inwon21;
                        item.INWON22 = Inwon22;
                        item.INWON23 = Inwon23;
                        item.INWON24 = Inwon24;
                        item.INWON25 = nCnt1;
                        item.INWON26 = Inwon26;
                        item.INWON27 = Inwon29;
                        item.INWON28 = Inwon30;
                        item.JEPQTY = nCnt;
                        item.BUILDCNT = nCnt;
                        if (strChk == "J")
                        {
                            item.GBBOGUN = "J"; // 자궁경부암만 할경우
                        }
                        item.SANGDAM1 = nJin_New[1];
                        item.SANGDAM2 = nJin_New[2];
                        item.SANGDAM3 = nJin_New[3];
                        item.SANGDAM4 = nJin_New[4];
                        item.SANGDAM5 = nJin_New[5];
                        item.SANGDAM6 = nJin_New[6];
                        item.MIRNO = FnMirNo;

                        int result = hicMirCancerService.UpdatebyMirNo(item);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_MIR_CANCER UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
            else
            {
                if (!clsHcType.B3.ROWID.IsNullOrEmpty())
                {
                    if (!strErrChk.IsNullOrEmpty())
                    {
                        int result2 = hicMirCancerService.UpdateJepQtyBuildCntGbErrChkbyMirNo(nCnt, strErrChk, FnMirNo);

                        if (result2 < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_MIR_CANCER UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void fn_HIC_MIR_Cancer_Bo_Save()
        {
            string strErrChk = "";
            long nCnt = 0;
            long nTotAmt = 0;
            int nREAD = 0;
            int nRead1 = 0;
            string strResult = "";
            long nWRTNO = 0;

            int Inwon01 = 0; //위암-위장조영촬영
            int Inwon02 = 0;    //위암-상부소화관
            int Inwon03 = 0;    //위암-조직검사 1-3
            int Inwon04 = 0;    //위암-조직검사
            int Inwon05 = 0;    //위암-조직검사
            int Inwon06 = 0;    //위암-조직검사
            int Inwon07 = 0;    //위암-조직검사

            int Inwon08 = 0;    //간암-의료급여-ALT
            int Inwon09 = 0;    //간암-의료급여-B형간염항원-일반
            int Inwon10 = 0;    //간암-의료급여-C형간염항체-일반
            int Inwon11 = 0;    //간암-간초음파검사
            int Inwon12 = 0;    //간암-혈청알파태아단백-RPHA
            int Inwon13 = 0;    //간암-혈청알파태아단백-EIA

            int Inwon14 = 0;    //대장암-분변잠혈반응검사-RPHA
            int Inwon15 = 0;    //대장암-분변잠혈반응검사-분변혈색소정량법
            int Inwon16 = 0;    //대장암-대장이중조영검사
            int Inwon17 = 0;    //대장암-대장내시경검사
            int Inwon18 = 0;    //대장암-조직검사 1-3
            int Inwon19 = 0;    //대장암-조직검사
            int Inwon20 = 0;    //대장암-조직검사
            int Inwon21 = 0;    //대장암-조직검사
            int Inwon22 = 0;    //대장암-조직검사

            int Inwon23 = 0;    //유방암-유방촬영
            int Inwon24 = 0;    //자궁경부암-자궁경부세포검사
            int Inwon26 = 0;    //토요일,공휴일가산

            int Inwon27 = 0;    //폐암- 저선량흉부CT
            int Inwon28 = 0;    //폐암- 사후결과상담

            long nTotAmt1 = 0;
            long nJohapAmt = 0;
            long nChaAmt = 0;
            long[] nAmt = new long[51];
            long nCnt1 = 0;         //상담료
            long nAmtTotal = 0;

            string strSelf = "";
            string strWrtNo = "";
            string strWrtNo2 = "";
            int[] nJin_New = new int[7]; //진촬상담여부 - 위.대장.간.유방.자궁.폐
            int[] nTot_Jin = new int[7];
            string strLife_Gbn = "";       //생애구분
            string strDate = "";
            string strGbSelf = "";
            string sJepDate = "";

            List<string> sExCodes = new List<string>();

            List<HIC_JEPSU> lstJepsu = new List<HIC_JEPSU>();

            strWrtNo = "";
            strWrtNo2 = "";
            nTotAmt = 0;
            strErrChk = "";
            Inwon01 = 0; Inwon02 = 0; Inwon03 = 0; Inwon04 = 0; Inwon05 = 0; Inwon06 = 0; Inwon07 = 0;
            Inwon08 = 0; Inwon09 = 0; Inwon10 = 0; Inwon11 = 0; Inwon12 = 0; Inwon13 = 0; Inwon14 = 0;
            Inwon15 = 0; Inwon16 = 0; Inwon17 = 0; Inwon18 = 0; Inwon19 = 0; Inwon20 = 0; Inwon21 = 0;
            Inwon22 = 0; Inwon23 = 0; Inwon24 = 0; Inwon26 = 0; Inwon27 = 0; Inwon28 = 0;
            nAmtTotal = 0; nCnt = 0; nCnt1 = 0;
            Fn절사금액합계1 = 0;

            for (int i = 0; i <= 50; i++)
            {
                nAmt[i] = 0;
            }

            List<HIC_JEPSU> list = hicJepsuService.GetWrtNoJepDatebyMirNo(FnMirNo);

            nCnt = list.Count;

            for (int i = 0; i <= 6; i++)
            {
                nJin_New[i] = 0;
            }

            for (int i = 0; i < nCnt; i++)
            {
                nWRTNO = lstJepsu[i].WRTNO;

                List<HIC_RESULT> list2 = hicResultService.GetItembyOnlyWrtNo(nWRTNO);

                nRead1 = list2.Count;

                for (int j = 0; j <= 6; j++)
                {
                    nTot_Jin[j] = 0;
                }

                for (int j = 0; j <= 50; j++)
                {
                    nAmt[j] = 0;
                }

                chb.READ_HIC_CANCER_NEW(nWRTNO); //접수번호에 대한 암검사 종류 읽음

                //(상담비 90,100% 구분조건)
                if (!clsHcType.B3.PanDrNo_New1.IsNullOrEmpty() && VB.Mid(clsHcType.B3.Can_MirGbn, 1, 1) != "1")
                {
                    nTot_Jin[1] = 1;
                    nJin_New[1] += 1;   //위
                }

                if (!clsHcType.B3.PanDrNo_New2.IsNullOrEmpty() && VB.Mid(clsHcType.B3.Can_MirGbn, 2, 1) != "1")
                {
                    nTot_Jin[2] = 1;
                    nJin_New[2] += 1;   //대장
                }

                if (!clsHcType.B3.PanDrNo_New3.IsNullOrEmpty() && VB.Mid(clsHcType.B3.Can_MirGbn, 3, 1) != "1")
                {
                    nTot_Jin[3] = 1;
                    nJin_New[3] += 1;   //간
                }

                if (!clsHcType.B3.PanDrNo_New4.IsNullOrEmpty() && VB.Mid(clsHcType.B3.Can_MirGbn, 4, 1) != "1")
                {
                    nTot_Jin[4] = 1;
                    nJin_New[4] += 1;   //유방
                }

                if (!clsHcType.B3.PanDrNo_New5.IsNullOrEmpty() && VB.Mid(clsHcType.B3.Can_MirGbn, 5, 1) != "1")
                {
                    nTot_Jin[5] = 1;
                    nJin_New[5] += 1;   //자궁
                }

                if (!clsHcType.B3.NEW_SICK77.IsNullOrEmpty() && VB.Mid(clsHcType.B3.Can_MirGbn, 6, 1) != "1")
                {
                    nTot_Jin[6] = 1;
                    nJin_New[6] += 1;   //폐
                }

                if (nRead1 != 0)
                {
                    for (int j = 0; j < nRead1; j++)
                    {
                        strResult = list2[j].RESULT;

                        if (clsHcType.B3.GbLiver != "1")
                        {
                            switch (list2[j].EXCODE)
                            {
                                case "TX10":
                                case "TX09":
                                case "TX27":
                                    strResult = "";
                                    break;
                                default:
                                    break;
                            }

                            if (VB.Left(cboJong.Text, 1) == "4")    //공단암
                            {
                                //접수시 수동 회사100% 일경우 항목 제외
                                List<HIC_RESULT_SUNAPDTL> list3 = hicResultSunapdtlService.GetCodebyWrtNoExCode(nWRTNO, list2[j].EXCODE);

                                for (int k = 0; k < list3.Count; k++)
                                {
                                    if (!list3[k].CODE.IsNullOrEmpty())
                                    {
                                        strResult = "";
                                    }
                                }

                                //대장암일경우
                                if (strResult != "미실시")
                                {
                                    if (list2[j].EXCODE == "TX26" && chb.READ_PATIENT_AGE(nWRTNO) < 50)
                                    {
                                        strResult = "";
                                    }
                                }

                                //조합부담이 없으면  제외
                                if (hicResultSunapdtlService.GetCountbyWrtNoExCode(nWRTNO, list2[j].EXCODE) > 0)
                                {
                                    strResult = "";
                                }
                            }

                            //판정화면에서 청구제외는 포함안함    2012-01-30  KMC
                            for (int k = 1; k <= 5; k++)
                            {
                                if (k == 1) //위암
                                {
                                    switch (list2[j].EXCODE)
                                    {
                                        case "TX22":        //위암-위장조영검사
                                        case "TX23":        //위암-위내시경검사
                                        case "TX45":        //위암-조직검사 1-3
                                        case "TX46":        //위암-조직검사
                                        case "TX47":        //위암-조직검사
                                        case "TX48":        //위암-조직검사
                                        case "TX49":        //위암-조직검사
                                            strResult = "";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else if (k == 2)    //대장
                                {
                                    switch (list2[j].EXCODE)
                                    {
                                        case "TX26":        //대장암-분변잠혈-RPHA
                                        case "TX31":        //대장암-대장이중조영검사
                                        case "TX32":        //대장암-대장내시경검사
                                        case "TX21":        //대장암-조직검사 1-3
                                        case "TX71":        //대장암-조직검사
                                        case "TX72":        //대장암-조직검사
                                        case "TX73":        //대장암-조직검사
                                        case "TX74":        //대장암-조직검사
                                            strResult = "";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else if (k == 3)    //간암
                                {
                                    switch (list2[j].EXCODE)
                                    {
                                        case "A125":        //간암-의료급여-ALT
                                        case "A258":        //간암-의료급여-B형간염항원-정밀 2010
                                        case "E508":        //간암-의료급여-C형간염항체-정밀 2010
                                        case "TX09":        //간암-간초음파
                                        case "TX10":        //간암-간초음파
                                        case "TX27":        //간암-간초음파
                                        case "A264":        //간암-혈청알파태아단백-EIA
                                            strResult = "";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else if (k == 4)    //유방암
                                {
                                    switch (list2[j].EXCODE)
                                    {
                                        case "TX29":        //유방암-유방촬영
                                            strResult = "";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else if (k == 5)    //자궁암
                                {
                                    switch (list2[j].EXCODE)
                                    {
                                        case "A171":        //자궁경부암-자궁경부세포검사
                                            strResult = "";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else if (k == 6)    //폐암
                                {
                                    switch (list2[j].EXCODE)
                                    {
                                        case "TY10":        //폐암-저선량흉부CT
                                        case "A312":        //폐암-사후결과상담
                                            strResult = "";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }

                            if (!strResult.IsNullOrEmpty() && strResult != "." && strResult != "결과없음" && strResult != "미실시")
                            {
                                switch (list2[j].EXCODE)
                                {
                                    case "TX22":
                                        Inwon01 += 1;
                                        nAmt[1] += 1;      //위암-위장조영검사
                                        break;
                                    case "TX23":
                                        Inwon02 += 1;
                                        nAmt[2] += 1;      //위암-위내시경검사
                                        break;
                                    case "TX45":
                                        Inwon03 += 1;
                                        nAmt[3] += 1;      //위암-조직검사 1-3
                                        break;
                                    case "TX46":
                                        Inwon04 += 1;
                                        nAmt[4] += 1;      //위암-조직검사
                                        break;
                                    case "TX47":
                                        Inwon05 += 1;
                                        nAmt[5] += 1;      //위암-조직검사
                                        break;
                                    case "TX48":
                                        Inwon06 += 1;
                                        nAmt[6] += 1;      //위암-조직검사
                                        break;
                                    case "TX49":
                                        Inwon07 += 1;
                                        nAmt[7] += 1;      //위암-조직검사
                                        break;

                                    case "TX26":
                                        Inwon15 += 1;
                                        nAmt[15] += 1;      //대장암-분변잠혈-RPHA
                                        break;
                                    case "TX31":
                                        Inwon16 += 1;
                                        nAmt[16] += 1;      //대장암-대장이중조영검사
                                        break;
                                    case "TX32":
                                        Inwon17 += 1;
                                        nAmt[17] += 1;      //대장암-대장내시경검사
                                        break;
                                    case "TX21":
                                        Inwon18 += 1;
                                        nAmt[18] += 1;      //대장암-조직검사 1-3
                                        break;
                                    case "TX71":
                                        Inwon19 += 1;
                                        nAmt[19] += 1;      //대장암-조직검사
                                        break;
                                    case "TX72":
                                        Inwon20 += 1;
                                        nAmt[20] += 1;      //대장암-조직검사
                                        break;
                                    case "TX73":
                                        Inwon21 += 1;
                                        nAmt[21] += 1;      //대장암-조직검사
                                        break;
                                    case "TX74":
                                        Inwon22 += 1;
                                        nAmt[22] += 1;      //대장암-조직검사
                                        break;
                                    case "TX29":
                                        Inwon23 += 1;
                                        nAmt[23] += 1;      //유방암-유방촬영
                                        break;
                                    case "A171":
                                        Inwon24 += 1;
                                        nAmt[24] += 1;      //자궁경부암-자궁경부세포검사
                                        break;
                                    case "TY10":
                                        Inwon27 += 1;
                                        nAmt[27] += 1;      //폐암-저선량륭부CT
                                        break;
                                    case "A312":
                                        Inwon28 += 1;
                                        nAmt[28] += 1;      //폐암-사후결과상담
                                        break;
                                    default:
                                        break;
                                }

                                if (VB.Mid(clsHcType.B3.Can_MirGbn, 3, 1) != "1")
                                {
                                    switch (list2[j].EXCODE)
                                    {
                                        case "TX09":
                                        case "TX10":
                                        case "TX27":            //간암-간초음파
                                            Inwon11 += 1;
                                            nAmt[11] += 1;
                                            break;
                                        case "A264":            //간암-혈청알파태아단백-EIA
                                            Inwon13 += 1;
                                            nAmt[13] += 1;
                                            //접수에는 항목이 있으나 판정에 없을경우 RPHA
                                            if (clsHcType.B3.Liver_EIA.IsNullOrEmpty() && clsHcType.B3.Liver_RPHA.IsNullOrEmpty())
                                            {
                                                Inwon13 -= 1;
                                                nAmt[13] -= 1;          //간암-EIA
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }

                //암검진 토.공휴일가산
                if (hicSunapdtlService.GetCountbyWrtNoCode(nWRTNO, "1119") > 0)
                {
                    Inwon26 += 1;
                    nAmt[26] += 1;
                }

                nAmt[1] *= FnAmAmt3[1];      //위암-위장조영검사
                nAmt[2] *= FnAmAmt3[2];      //위암-위내시경검사

                nAmt[3] *= FnAmAmt3[3];      //위암-조직검사 1-3
                nAmt[4] *= FnAmAmt3[4];      //위암-조직검사
                nAmt[5] *= FnAmAmt3[5];      //위암-조직검사
                nAmt[6] *= FnAmAmt3[6];      //위암-조직검사
                nAmt[7] *= FnAmAmt3[7];      //위암-조직검사
                nAmt[8] *= FnAmAmt3[8];      //간암-의료급여-ALT
                nAmt[9] *= FnAmAmt3[9];      //간암-의료급여-B형간염항원-정밀-EIA
                nAmt[10] *= FnAmAmt3[10];    //간암-의료급여-C형간염항체-정밀-EIA

                nAmt[11] *= FnAmAmt3[11];    //간암-간초음파
                nAmt[13] *= FnAmAmt3[13];    //간암-EIA

                nAmt[14] *= FnAmAmt3[14];    //대장암-RPHA
                nAmt[15] *= FnAmAmt3[15];    //대장암-분변잠혈
                nAmt[16] *= FnAmAmt3[16];    //대장암-대장이중조영검사

                nAmt[17] *= FnAmAmt3[17];    //대장암-대장내시경

                nAmt[18] *= FnAmAmt3[18];    //대장암-조직검사 1-3
                nAmt[19] *= FnAmAmt3[19];    //대장암-조직검사
                nAmt[20] *= FnAmAmt3[20];    //대장암-조직검사
                nAmt[21] *= FnAmAmt3[21];    //대장암-조직검사
                nAmt[22] *= FnAmAmt3[22];    //대장암-조직검사

                nAmt[23] *= FnAmAmt3[23];    //유방암-유방촬영

                nAmt[24] *= FnAmAmt3[24];    //자궁경부암-자궁경부세포검사
                nAmt[26] *= FnAmAmt3[27];    //토요일,공휴일30 % 가산

                nAmt[27] *= FnAmAmt3[30];    //폐암-저선량CT
                nAmt[28] *= FnAmAmt3[31];    //폐암:사후상담

                if ((nTot_Jin[1] + nTot_Jin[2] + nTot_Jin[3] + nTot_Jin[4] + nTot_Jin[5]) >= 1)
                {
                    if (nTot_Jin[1] + nTot_Jin[2] + nTot_Jin[3] + nTot_Jin[4] + nTot_Jin[5] == 1 && nTot_Jin[5] == 1)
                    {
                        nAmt[25] = 0;              //상담료
                    }
                    else if (nTot_Jin[2] == 0)
                    {
                        nAmt[25] = 0;              //상담료
                    }
                    else
                    {
                        nAmt[25] = FnAmAmt3[25];              //상담료
                        nCnt1 += 1;
                        strWrtNo += nWRTNO.To<string>() + ",";
                    }
                    nCnt1 += 1;
                }
                else
                {
                    nAmt[25] = 0;              //상담료
                }

                nTotAmt1 = nAmt[1] + nAmt[2] + nAmt[3] + nAmt[4] + nAmt[5] + nAmt[6] + nAmt[7] + nAmt[8] + nAmt[9] + nAmt[10] + nAmt[11] + nAmt[12] + nAmt[13] + nAmt[14] + nAmt[15] + nAmt[16] + nAmt[17] + nAmt[18] + nAmt[19] + nAmt[20] + nAmt[21] + nAmt[22] + nAmt[23] + nAmt[24] + nAmt[25] + nAmt[26] + nAmt[27] + nAmt[28];
                nAmtTotal += nTotAmt1;

                sJepDate = cboYear.Text + "-01-01";

                if (chkchk.Checked == false)
                {
                    HIC_JEPSU_SUNAP list6 = hicJepsuSunapService.GetItembyJepDateWrtNo(nWRTNO, sJepDate);

                    nJohapAmt = list6.BOGENAMT;

                    if (nJohapAmt > 0)
                    {
                        nChaAmt = nTotAmt1 - nJohapAmt;
                        if (clsHcType.B3.JinchalGbn == "1")
                        {
                            nCnt += 1;
                        }
                    }
                    else
                    {
                        nChaAmt = 0;
                        nAmtTotal -= nAmt[25];
                        strWrtNo2 += nWRTNO.To<string>() + ",";
                    }

                    FstrSname = list6.SNAME;
                    Fn절사금액합계2 += nChaAmt;

                    if (nChaAmt != 0)   //암검진 청구비용 차액발생
                    {
                        fn_ERROR_INSERT(nWRTNO, "2", nChaAmt + "￦ 보건소 청구비용 차액발생");
                        if ((nTotAmt1 - clsHcVariable.GnAmt_Misu_BogenAmt2) != 0)
                        {
                            SS2.ActiveSheet.Cells[FnRow, 7].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                        }
                    }

                    FnAge = 0;
                    FstrSex = "";
                }

                nChaAmt = 0;
                nTotAmt1 = 0;
                nJohapAmt = 0;
                for (int j = 0; j <= 50; j++)
                {
                    nAmt[j] = 0;
                }
            }

            txtGongWon.Text = Fn절사금액합계2.To<string>();

            nTotAmt = hicSunapService.GetLtdAmtbyMirNo(FnMirNo);

            //암검진 공단암 금액 조회
            nTotAmt = hicSunapService.GetJohapAmtbyMirNo(FnMirNo, VB.Left(cboJong.Text, 1));

            List<COMHPC> list7 = comHpcLibBService.GetCountHicMirErrorbyMirNo(FnMirNo);

            if (list7.Count > 0)
            {
                strErrChk = "N";
            }
            else
            {
                strErrChk = "Y";
            }
            nREAD = list7.Count;

            clsDB.setBeginTran(clsDB.DbCon);

            if (chkchk.Checked == false)
            {
                nAmtTotal = Math.Truncate(nAmtTotal.To<decimal>()).To<long>(); //금액을 원단위 절사함.

                if (!clsHcType.B3.ROWID.IsNullOrEmpty())
                {
                    if (!strErrChk.IsNullOrEmpty())
                    {
                        HIC_MIR_CANCER_BO item = new HIC_MIR_CANCER_BO();

                        item.GBERRCHK = strErrChk;
                        item.INWON01 = Inwon01;
                        item.INWON02 = Inwon02;
                        item.INWON03 = Inwon03;
                        item.INWON04 = Inwon04;
                        item.INWON05 = Inwon05;
                        item.INWON06 = Inwon06;
                        item.INWON07 = Inwon07;
                        item.INWON08 = Inwon08;
                        item.INWON09 = Inwon09;
                        item.INWON10 = Inwon10;
                        item.INWON11 = Inwon11;
                        item.INWON12 = Inwon12;
                        item.INWON13 = Inwon13;
                        item.INWON14 = Inwon14;
                        item.INWON15 = Inwon15;
                        item.INWON16 = Inwon16;
                        item.INWON17 = Inwon17;
                        item.INWON18 = Inwon18;
                        item.INWON19 = Inwon19;
                        item.INWON20 = Inwon20;
                        item.INWON21 = Inwon21;
                        item.INWON22 = Inwon22;
                        item.INWON23 = Inwon23;
                        item.INWON24 = Inwon24;
                        item.INWON25 = nCnt1;
                        item.INWON26 = Inwon26;
                        item.INWON27 = Inwon27;
                        item.INWON28 = Inwon28;
                        item.JEPQTY = nCnt;
                        item.BUILDCNT = nCnt;
                        item.SANGDAM1 = nJin_New[1];
                        item.SANGDAM2 = nJin_New[2];
                        item.SANGDAM3 = nJin_New[3];
                        item.SANGDAM4 = nJin_New[4];
                        item.SANGDAM5 = nJin_New[5];
                        item.MIRNO = FnMirNo;

                        int result = hicMirCancerBoService.UpdatebyMirNo(item);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_MIR_CANCER_BO UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
            else
            {
                if (!clsHcType.B3.ROWID.IsNullOrEmpty())
                {
                    if (!strErrChk.IsNullOrEmpty())
                    {
                        int result2 = hicMirCancerBoService.UpdateJepQtyBuildCntGbErrChkbyMirNo(nCnt, strErrChk, FnMirNo);

                        if (result2 < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_MIR_CANCER_BO UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }


        /// <summary>
        /// 암검진 결과(위,대장,간,유방,자궁,폐암)
        /// </summary>
        /// <param name="argMirno"></param>
        void fn_Cancer_Mir_Result1_File(long argMirno, StreamWriter sw)
        {
            int q = 0;
            int ss = 0;
            int TT = 0;
            long nWRTNO = 0;
            int nREAD = 0;
            string strLtdCode = "";
            string strGkiho = "";
            string strGunDate = "";
            string strTongDate = "";
            string strJepDate = "";
            string strTemp = "";
            string strOK = "";
            string strSogen = "";
            string strPtno = "";
            string strGbJin = "";
            string strRESULT0005 = "";
            string strRESULT0006 = "";

            string strDate = "";
            string strGbSelf = "";

            string strOK1 = "";
            string strCHK1 = "";

            string sJepDate = "";
            string sJong = "";
            List<string> strExCodes = new List<string>();

            string strFileName = "";

            sJepDate = cboYear.Text + "-01-01";
            sJong = VB.Left(cboJong.Text, 1);

            //암검사 결과
            List<HIC_JEPSU_CANCER_NEW> list = hicJepsuCancerNewService.GetItembyJepDateJongGMirNo(sJepDate, sJong, argMirno);

            nREAD = list.Count;

            FstrREC.Clear();
            FstrREC.Append("[");

            for (int i = 0; i < nREAD; i++)
            {
                nWRTNO = list[i].WRTNO;
                chb.READ_HIC_CANCER_NEW(nWRTNO); //접수번호에 대한 암검사 종류 읽음

                strExCodes.Clear();
                strExCodes.Add("TX20");
                strExCodes.Add("TX23");
                strExCodes.Add("TX41");

                strCHK1 = "";
                if (hicResultService.GetCountbyWrtNoInExCode(nWRTNO, strExCodes) > 0)
                {
                    strCHK1 = "OK";
                }

                HIC_JEPSU_SUNAP list2 = hicJepsuSunapService.GetItembyWrtNoCode(nWRTNO, "3101");

                if (!list2.IsNullOrEmpty())
                {
                    strDate = list2.JEPDATE;
                    strGbSelf = list2.GBSELF;
                }

                //상담료선택기준
                if (clsHcType.B3.GBRECTUM == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 2, 1) != "1")
                {
                    strGbJin = "2";
                }
                else if (clsHcType.B3.GbWomb == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 5, 1) != "1")
                {
                    strGbJin = "5";
                }
                else if (clsHcType.B3.GBSTOMACH == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 1, 1) != "1")
                {
                    strGbJin = "1";
                }
                else if (clsHcType.B3.GbLiver == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 3, 1) != "1")
                {
                    strGbJin = "3";
                }
                else if (clsHcType.B3.GBBREAST == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 4, 1) != "1")
                {
                    strGbJin = "4";
                }
                else if (clsHcType.B3.GBLUNG == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 6, 1) != "1")
                {
                    strGbJin = "6";
                }

                if (hicJepsuSunapService.GetCountbyWrtNoCode(nWRTNO, "3168") > 0)
                {
                    strGbJin = "";
                }

                strOK1 = "";
                if (hicResultService.GetCountbyWrtNoCode(nWRTNO, "TX31") > 0)
                {
                    strOK1 = "OK";
                }

                strLtdCode = list[i].LTDCODE.To<string>();
                strGkiho = list[i].GKIHO.Replace("-", "");
                strPtno = list[i].PTNO;
                strGunDate = list[i].GUNDATE.Replace("-", "");
                strJepDate = list[i].JEPDATE.Replace("-", "");
                strTongDate = list[i].TONGBODATE.Replace("-", "");

                FstrREC.Append("{" + "" + (char)34 + "ITEM0" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");       //청구파일 구분
                FstrREC.Append("" + (char)34 + "ITEM1" + (char)34 + ":" + (char)34 + VB.Left(cboYear.Text + VB.Space(4), 4) + (char)34 + ","); //사업년도
                FstrREC.Append("" + (char)34 + "ITEM2" + (char)34 + ":" + (char)34 + VB.Left(clsAES.DeAES(list[i].JUMIN2) + VB.Space(13), 13) + (char)34 + ","); // 주민번호
                FstrREC.Append("" + (char)34 + "ITEM3" + (char)34 + ":" + (char)34 + VB.Left(strGkiho + VB.Space(11), 11) + (char)34 + ","); // 증번호

                //암청구종목(nWRTNO)
                fn_CancerChargeKind(nWRTNO);

                if (list[i].MURYOAM == "Y")
                {
                    FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + VB.Left(list[i].BOGUNSO + VB.Space(8), 8) + (char)34 + ",");         //소속보건소기호
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
                }

                //------------------------------------------------------------------------------------------------------------------------------
                //★ 1.위암
                if (clsHcType.B3.GBSTOMACH == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 1, 1) != "")   //판정에서 위암제외아닐경우
                {
                    //위장조영검사(strGunDate)
                    fn_GastroIntestinalIllumination(strGunDate);
                    //위내시경검사(strCHK1, strJepDate, strGunDate)
                    fn_GastroScope(strCHK1, strJepDate, strGunDate);
                    //위조직진단(strCHK1, strGunDate)
                    fn_GastricTissue(strCHK1, strGunDate);
                    //위종합판정(nWRTNO, strCHK1, strGbJin)
                    fn_StomachTotalPan(nWRTNO, strCHK1, strGbJin);
                }

                //★ 2.대장암
                if (clsHcType.B3.GBRECTUM == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 2, 1) != "1")   //판정에서 대장암제외아닐경우
                {
                    //Call 분변잠혈반응(nWRTNO, strGunDate)
                    fn_VariationsLatentBloodTest(nWRTNO, strGunDate);
                    //Call 대장이중조영검사(strGunDate, strOK, strOK1)
                    fn_ColonizationTest(strGunDate, strOK, strOK1);
                    //Call 대장내시경검사
                    fn_ColonoScopy();
                    //Call 대장조직진단(strGunDate)
                    fn_ColonoTissue(strGunDate);
                    //Call 대장종합판정(nWRTNO, strGbJin)
                    fn_ColonoTotalPan(nWRTNO, strGbJin);
                }

                //★ 3.간암
                if (clsHcType.B3.GbLiver == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 3, 1) != "1")    //판정에서 간암제외아닐경우
                {
                    //Call 간암상반기(strGbJin, strGunDate)
                    fn_LiverFirstHalf(strGbJin, strGunDate);
                    FstrREC.Append(VB.Space(792));  //하반기 간암검진 공란으로
                }
                //하반기 간암건진 청구
                else
                {
                    //Call 간암하반기(strGbJin, strGunDate)
                    fn_LiverLastHalf(strGbJin, strGunDate);
                    FstrREC.Append(VB.Space(792));  //상반기 간암검진 공란으로
                }

                //★ 4.유방암
                if (clsHcType.B3.GBBREAST == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 4, 1) != "1")   //판정에서 유방암제외아닐경우
                {
                    //Call 유방암결과
                    fn_BreastCancerResult();
                    //Call 유방암결과1
                    fn_BreastCancerResult1();
                    //Call 유방암결과2
                    fn_BreastCancerResult2();
                    //Call 유방암판정(strGunDate, strGbJin)
                    fn_BreastCancerPan(strGunDate, strGbJin);
                }

                //★ 5.자궁암
                if (clsHcType.B3.GbWomb == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 5, 1) != "1") //판정에서 자궁암제외아닐경우
                {
                    //Call 자궁암결과
                    fn_WombResult();
                    //Call 자궁암판정(strGbJin, strGunDate)
                    fn_WombPan(strGbJin, strGunDate);
                }

                //★ 폐암
                if (clsHcType.B3.GBLUNG == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 6, 1) != "1")
                {
                    //Call 폐암결과(strGbJin)
                    fn_LungCancerResult(strGbJin);
                    //Call 폐암판정(strGunDate)
                    fn_lungLungCancerPan(strGunDate);
                }

                //=======================================================================================================================
                if (list[i].LTDCODE != 0)      //통보방법
                {
                    hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());

                    //회사코드와 접수시 주소가 다르면 개인으로
                    if (clsHcVariable.GstrLtdJuso != list[i].JUSOAA)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM605" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM605" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                    }
                }
                else
                {
                    //회사코드 없으면 개인으로
                    FstrREC.Append("" + (char)34 + "ITEM605" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                if (chb.READ_ResultApplicationAgreeWhether(strPtno, cboYear.Text) != "")
                {
                    FstrREC.Append("" + (char)34 + "ITEM607" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                                 //결과활용동의여부 -동의1, 동의안함0
                    FstrREC.Append("" + (char)34 + "ITEM608" + (char)34 + ":" + (char)34 + chb.READ_ResultApplicationAgreeWhether(strPtno, cboYear.Text) + (char)34 + ",");         //결과활용동의 일자
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM607" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");                                                    //결과활용동의여부 -동의1, 동의안함0
                    FstrREC.Append("" + (char)34 + "ITEM608" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");                                               //결과활용동의 일자
                }

                FstrREC.Append("" + (char)34 + "ITEM611" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM612" + (char)34 + ":" + (char)34 + "E" + (char)34 + "},");

                sw.WriteLine(FstrREC);
                FstrREC.Clear();
            }
        }

        /// <summary>
        /// 폐암결과()
        /// </summary>
        /// <param name="argGbJin"></param>
        void fn_LungCancerResult(string argGbJin)
        {
            string strGbJin = "";

            strGbJin = argGbJin;

            FstrREC.Append("" + (char)34 + "ITEM513" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");        //FULL PACS
            FstrREC.Append("" + (char)34 + "ITEM514" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:00.00}", clsHcType.B3.LUNG_RESULT001) + VB.Space(5), 5) + (char)34 + ",");


            if (clsHcType.B3.LUNG_RESULT002 == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM515" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM516" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM517" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM515" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM516" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT003 + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM517" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT004 + (char)34 + ",");
            }

            //폐결절소견1
            if (clsHcType.B3.LUNG_RESULT005.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM518" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM519" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM520" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM521" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM522" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM523" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM524" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM525" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            else
            {
                if (clsHcType.B3.LUNG_RESULT005.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM518" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");

                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM518" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT005 + (char)34 + ",");
                }

                if (clsHcType.B3.LUNG_RESULT006.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM519" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");

                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM519" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT006 + (char)34 + ",");
                }
                if (clsHcType.B3.LUNG_RESULT007.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM520" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM520" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT007 + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM521" + (char)34 + ":" + (char)34 + VB.Left(string.Format(clsHcType.B3.LUNG_RESULT008, "000") + VB.Space(3), 3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM522" + (char)34 + ":" + (char)34 + VB.Left(string.Format(clsHcType.B3.LUNG_RESULT009, "000") + VB.Space(3), 3) + (char)34 + ",");

                if (clsHcType.B3.LUNG_RESULT010.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM523" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM523" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT010 + (char)34 + ",");
                }
                if (clsHcType.B3.LUNG_RESULT011.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM524" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM524" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT011 + (char)34 + ",");
                }
                if (clsHcType.B3.LUNG_RESULT012.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM525" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM525" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT012 + (char)34 + ",");
                }
            }

            //폐결절소견2
            if (clsHcType.B3.LUNG_RESULT013.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM526" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM527" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM528" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM529" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM530" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM531" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM532" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM533" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            else
            {
                if (clsHcType.B3.LUNG_RESULT013.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM526" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM526" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT013 + (char)34 + ",");
                }

                if (clsHcType.B3.LUNG_RESULT014.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM527" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM527" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT014 + (char)34 + ",");
                }
                if (clsHcType.B3.LUNG_RESULT015.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM528" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM528" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT015 + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM529" + (char)34 + ":" + (char)34 + VB.Left(string.Format(clsHcType.B3.LUNG_RESULT016, "000") + VB.Space(3), 3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM530" + (char)34 + ":" + (char)34 + VB.Left(string.Format(clsHcType.B3.LUNG_RESULT017, "000") + VB.Space(3), 3) + (char)34 + ",");

                if (clsHcType.B3.LUNG_RESULT018.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM531" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM531" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT018 + (char)34 + ",");
                }
                if (clsHcType.B3.LUNG_RESULT019.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM532" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM532" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT019 + (char)34 + ",");
                }
                if (clsHcType.B3.LUNG_RESULT020.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM533" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM533" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT030 + (char)34 + ",");
                }
            }

            //폐결절소견3
            if (clsHcType.B3.LUNG_RESULT021.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM534" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM535" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM536" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM537" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM538" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM539" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM540" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM541" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            else
            {
                if (clsHcType.B3.LUNG_RESULT021.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM534" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM534" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT021 + (char)34 + ",");
                }
                if (clsHcType.B3.LUNG_RESULT022.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM535" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM535" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT022 + (char)34 + ",");
                }
                if (clsHcType.B3.LUNG_RESULT023.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM536" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM536" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT023 + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM537" + (char)34 + ":" + (char)34 + VB.Left(string.Format(clsHcType.B3.LUNG_RESULT024, "000") + VB.Space(3), 3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM538" + (char)34 + ":" + (char)34 + VB.Left(string.Format(clsHcType.B3.LUNG_RESULT025, "000") + VB.Space(3), 3) + (char)34 + ",");

                if (clsHcType.B3.LUNG_RESULT026.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM539" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM539" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT026 + (char)34 + ",");
                }
                if (clsHcType.B3.LUNG_RESULT027.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM540" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM540" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT027 + (char)34 + ",");
                }
                if (clsHcType.B3.LUNG_RESULT028.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM541" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM541" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT028 + (char)34 + ",");
                }
            }

            //폐결절소견4
            if (clsHcType.B3.LUNG_RESULT029.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM542" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM543" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM544" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM545" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM546" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM547" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM548" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM549" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            else
            {
                if (clsHcType.B3.LUNG_RESULT029.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM542" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM542" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT029 + (char)34 + ",");
                }

                if (clsHcType.B3.LUNG_RESULT030.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM543" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM543" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT030 + (char)34 + ",");
                }
                if (clsHcType.B3.LUNG_RESULT031.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM544" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM544" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT031 + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM545" + (char)34 + ":" + (char)34 + VB.Left(string.Format(clsHcType.B3.LUNG_RESULT032, "000") + VB.Space(3), 3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM546" + (char)34 + ":" + (char)34 + VB.Left(string.Format(clsHcType.B3.LUNG_RESULT033, "000") + VB.Space(3), 3) + (char)34 + ",");

                if (clsHcType.B3.LUNG_RESULT034.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM547" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM547" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT034 + (char)34 + ",");
                }

                if (clsHcType.B3.LUNG_RESULT035.IsNullOrEmpty())
                    FstrREC.Append("" + (char)34 + "ITEM548" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM548" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT035 + (char)34 + ",");
                }

                if (clsHcType.B3.LUNG_RESULT036.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM549" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM549" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT036 + (char)34 + ",");
                }
            }
            //폐결절소견5
            if (clsHcType.B3.LUNG_RESULT037.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM550" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM551" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM552" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM553" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM554" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM555" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM556" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM557" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            else
            {
                if (clsHcType.B3.LUNG_RESULT037.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM550" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM550" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT037 + (char)34 + ",");
                }

                if (clsHcType.B3.LUNG_RESULT038.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM551" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM551" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT038 + (char)34 + ",");
                }

                if (clsHcType.B3.LUNG_RESULT039.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM552" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM552" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT039 + (char)34 + ",");
                }
                FstrREC.Append("" + (char)34 + "ITEM553" + (char)34 + ":" + (char)34 + VB.Left(string.Format(clsHcType.B3.LUNG_RESULT040, "000") + VB.Space(3), 3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM554" + (char)34 + ":" + (char)34 + VB.Left(string.Format(clsHcType.B3.LUNG_RESULT041, "000") + VB.Space(3), 3) + (char)34 + ",");

                if (clsHcType.B3.LUNG_RESULT042.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM555" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM555" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT042 + (char)34 + ",");
                }

                if (clsHcType.B3.LUNG_RESULT043.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM556" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM556" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT043 + (char)34 + ",");
                }

                if (clsHcType.B3.LUNG_RESULT044.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM557" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM557" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT044 + (char)34 + ",");
                }
            }

            //폐결절소견6
            if (clsHcType.B3.LUNG_RESULT045.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM558" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM559" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM560" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM561" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM562" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM563" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM564" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM565" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            else
            {
                if (clsHcType.B3.LUNG_RESULT045.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM558" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM558" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT045 + (char)34 + ",");
                }

                if (clsHcType.B3.LUNG_RESULT046.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM559" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM559" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT046 + (char)34 + ",");
                }
                if (clsHcType.B3.LUNG_RESULT047.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM560" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM560" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT047 + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM561" + (char)34 + ":" + (char)34 + VB.Left(string.Format(clsHcType.B3.LUNG_RESULT048, "000") + VB.Space(3), 3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM562" + (char)34 + ":" + (char)34 + VB.Left(string.Format(clsHcType.B3.LUNG_RESULT049, "000") + VB.Space(3), 3) + (char)34 + ",");

                if (clsHcType.B3.LUNG_RESULT050.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM563" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM563" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT050 + (char)34 + ",");
                }
                if (clsHcType.B3.LUNG_RESULT051.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM564" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM564" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT051 + (char)34 + ",");
                }

                if (clsHcType.B3.LUNG_RESULT052.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM565" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM565" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT052 + (char)34 + ",");
                }
            }

            //폐결절 외
            if (clsHcType.B3.LUNG_RESULT053 == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM566" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
            }
            else if (clsHcType.B3.LUNG_RESULT053 == "2")
            {
                FstrREC.Append("" + (char)34 + "ITEM566" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }

            FstrREC.Append("" + (char)34 + "ITEM567" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.LUNG_RESULT054 + VB.Space(20), 20) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM568" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT055 + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM569" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.LUNG_RESULT056 + VB.Space(20), 20) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM570" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT057 + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM571" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT058 + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM572" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT059 + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM573" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT060 + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM574" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT061 + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM575" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT062 + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM576" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT063 + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM577" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT064 + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM578" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT065 + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM579" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.LUNG_RESULT066 + VB.Space(20), 20) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM580" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT067 + (char)34 + ",");

            //판정구분
            FstrREC.Append("" + (char)34 + "ITEM581" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT068 + (char)34 + ",");

            if (clsHcType.B3.LUNG_RESULT068 == "4")
            {
                FstrREC.Append("" + (char)34 + "ITEM582" + (char)34 + ":" + (char)34 + clsHcType.B3.LUNG_RESULT069 + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM582" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //기타폐결절외의미있는소견
            if (!clsHcType.B3.LUNG_RESULT078.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM610" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM583" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.LUNG_RESULT078 + VB.Space(20), 20) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM610" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM583" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }

            FstrREC.Append("" + (char)34 + "ITEM584" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.LUNG_RESULT070 + VB.Space(600), 600) + (char)34 + ",");         //폐암 판정구분에의한 권고사항

            //폐암 판정 외 기타 권고사항
            if (clsHcType.B3.LUNG_RESULT057 == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM585" + (char)34 + ":" + (char)34 + VB.Space(600) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM585" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.LUNG_RESULT071 + VB.Space(600), 600) + (char)34 + ",");
            }

            //폐암 상담료포함 여부
            if (strGbJin == "6")
            {
                FstrREC.Append("" + (char)34 + "ITEM586" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM586" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }
        }

        /// <summary>
        /// 폐암판정()
        /// </summary>
        /// <param name="argGunDate"></param>
        void fn_lungLungCancerPan(string argGunDate)
        {
            string strGunDate = "";
            strGunDate = argGunDate;

            FstrREC.Append("" + (char)34 + "ITEM587" + (char)34 + ":" + (char)34 + VB.Left(strGunDate + VB.Space(8), 8) + (char)34 + ",");       //폐암 검진일자
            FstrREC.Append("" + (char)34 + "ITEM588" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");                                        //검진장소

            //판독의사 면허번호, 성명, 주민번호
            if (clsHcType.B3.GBLUNG == "1")
            {
                if (!clsHcType.B3.PANJENGDRNO11.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM589" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PANJENGDRNO11 + VB.Space(10), 10) + (char)34 + ",");                      //폐암-판독면허번호 10
                    FstrREC.Append("" + (char)34 + "ITEM590" + (char)34 + ":" + (char)34 + VB.Left(hb.READ_BILL_DRCODE(clsHcType.B3.PANJENGDRNO11) + VB.Space(12), 12) + (char)34 + ",");    //폐암의사명 12
                    FstrREC.Append("" + (char)34 + "ITEM591" + (char)34 + ":" + (char)34 + VB.Left(hb.READ_BILL_JUMIN(clsHcType.B3.PANJENGDRNO11) + VB.Space(13), 13) + (char)34 + ",");     //폐암의사주민번호 13
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM589" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM590" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM591" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM589" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM590" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM591" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
            }

            //'판정일자, 판정의사 면허번호, 성명, 주민번호
            FstrREC.Append("" + (char)34 + "ITEM592" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.L_PANJENGDATE1.Replace("-", "") + VB.Space(8), 8) + (char)34 + ",");  //폐암-판정일자


            if (clsHcType.B3.GBLUNG == "1")
            {
                if (!clsHcType.B3.NEW_WOMAN37.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM593" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_WOMAN37 + VB.Space(10), 10) + (char)34 + ",");                              //폐-판정면허번호 10
                    FstrREC.Append("" + (char)34 + "ITEM594" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(clsHcType.B3.NEW_WOMAN37) + VB.Space(12), 12) + (char)34 + ",");             //폐암의사명 12
                    FstrREC.Append("" + (char)34 + "ITEM595" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN(clsHcType.B3.NEW_WOMAN37) + VB.Space(13), 13) + (char)34 + ",");
                }
                //폐암의사주민번호 13
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM593" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM594" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM595" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM593" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM594" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM595" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
            }

            //폐암 기존 폐암환자
            if (VB.Left(clsHcType.B3.NEW_SICK76, 1) == "2")
            {
                FstrREC.Append("" + (char)34 + "ITEM596" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM596" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //폐암 장애인 안전 편의관리비
            FstrREC.Append("" + (char)34 + "ITEM597" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");

            //'사후결과상담
            FstrREC.Append("" + (char)34 + "ITEM598" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.LUNG_SANGDAM1 + VB.Space(600), 600) + (char)34 + ",");    //폐암검진결과 관련 상담내용
            FstrREC.Append("" + (char)34 + "ITEM599" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.LUNG_SANGDAM2 + VB.Space(600), 600) + (char)34 + ",");    //금연상담 관련 상담 내용
            FstrREC.Append("" + (char)34 + "ITEM600" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.LUNG_SANGDAM3 + VB.Space(8), 8) + (char)34 + ",");        //폐암 검진일자
            FstrREC.Append("" + (char)34 + "ITEM601" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");                                               //상담장소(2:내원)

            if (clsHcType.B3.GBLUNG == "1")
            {
                if (!clsHcType.B3.NEW_WOMAN37.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM602" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.LUNG_SANGDAM4 + VB.Space(10), 10) + (char)34 + ",");                  //자궁암-판정면허번호 10
                    FstrREC.Append("" + (char)34 + "ITEM603" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(clsHcType.B3.LUNG_SANGDAM4) + VB.Space(12), 12) + (char)34 + ","); //자궁암의사명 12
                    FstrREC.Append("" + (char)34 + "ITEM604" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN(clsHcType.B3.LUNG_SANGDAM4) + VB.Space(13), 13) + (char)34 + ","); //자궁암의사주민번호 13
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM602" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM603" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM604" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM602" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM603" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM604" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 자궁암결과
        /// </summary>
        void fn_WombResult()
        {
            //자궁암-검체상태
            if (!clsHcType.B3.WOMB01.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM477" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.WOMB01 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM477" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //자궁암-선상피세포유무
            if (!clsHcType.B3.WOMB02.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM478" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.WOMB02 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM478" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //자궁암-유형별진단
            if (!clsHcType.B3.WOMB03.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM479" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.WOMB03 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM479" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            if (clsHcType.B3.WOMB04.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM480" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.WOMB04 + VB.Space(1), 1) + (char)34 + ",");                //자궁암-유형별진단-편상피세포이상
                FstrREC.Append("" + (char)34 + "ITEM481" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.WOMAN12 + VB.Space(1), 1) + (char)34 + ",");               //자궁암-유형별진단-편상피세포이상-위험구분-병리과자료 읽어옴
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM480" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM481" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //자궁암-유형별진단-선상피세포이상
            if (!clsHcType.B3.WOMB05.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM482" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.WOMB05 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM482" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //자궁암-유형별진단-선상피세포이상-비정형 선상피세포
            if (!clsHcType.B3.RESULT0014.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM483" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.WOMB05 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM483" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //자궁암-유형별진단-기타
            if (!clsHcType.B3.WOMB11.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM484" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.WOMB11, 20) + VB.Space(20), 20) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM484" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }

            //자궁암-추가소견
            if (!clsHcType.B3.WOMB07.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM485" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.WOMB07 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM485" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //자궁암-추가소견-기타
            if (!clsHcType.B3.WOMB08.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM486" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.WOMB08, 20) + VB.Space(20), 20) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM486" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }

            //(중복자궁)
            FstrREC.Append("" + (char)34 + "ITEM487" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM488" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM489" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM490" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM491" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM492" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM493" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM494" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM495" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM496" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
        }

        /// <summary>
        /// 자궁암판정
        /// </summary>
        /// <param name="argGbJin"></param>
        /// <param name="argGunDate"></param>
        void fn_WombPan(string argGbJin, string argGunDate)
        {
            string strTemp = "";
            string strGbJin = "";
            string strGunDate = "";

            strGbJin = argGbJin;
            strGunDate = argGunDate;

            if (clsHcType.B3.GbWomb == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM497" + (char)34 + ":" + (char)34 + "2" + (char)34 + ","); //자궁암-검진장소
                FstrREC.Append("" + (char)34 + "ITEM498" + (char)34 + ":" + (char)34 + VB.Left(strGunDate + VB.Space(8), 8) + (char)34 + ","); //자궁암 검진일자
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM497" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM498" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
            }

            //자궁암-종합판정
            if (!clsHcType.B3.WOMB09.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM499" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.WOMB09 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM499" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //자궁암-기존자궁암
            if (VB.Left(clsHcType.B3.NEW_SICK18, 1) == "2")
            {
                FstrREC.Append("" + (char)34 + "ITEM500" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM500" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //자궁암-자궁암종합기타
            if (!clsHcType.B3.WOMB10.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM501" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.WOMB10, 20) + VB.Space(20), 20) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM501" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }

            strTemp = clsHcType.B3.W_SOGEN.Replace("\r\n", " ");
            FstrREC.Append("" + (char)34 + "ITEM502" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(strTemp, 600) + VB.Space(600), 600) + (char)34 + ","); //자궁암-자궁암권고

            //자궁암-판정일자
            if (!clsHcType.B3.W_PANJENGDATE.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM503" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.W_PANJENGDATE.Replace("-", "") + VB.Space(8), 8) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM503" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
            }

            if (clsHcType.B3.GbWomb == "1")
            {
                if (!clsHcType.B3.PanDrNo_New5.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM504" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PanDrNo_New5 + VB.Space(10), 10) + (char)34 + ","); //자궁암-판정면허번호 10
                    FstrREC.Append("" + (char)34 + "ITEM505" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(clsHcType.B3.PanDrNo_New5) + VB.Space(12), 12) + (char)34 + ",");  //자궁암의사명 12
                    FstrREC.Append("" + (char)34 + "ITEM506" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN(clsHcType.B3.PanDrNo_New5) + VB.Space(13), 13) + (char)34 + ","); //자궁암의사주민번호 13
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM504" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM505" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM506" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM504" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM505" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM506" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
            }

            //자궁암-진찰료여부
            if (strGbJin == "5")
            {
                FstrREC.Append("" + (char)34 + "ITEM507" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM507" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //유방암-판독의사
            if (clsHcType.B3.GbWomb == "1")
            {
                if (!clsHcType.B3.PANJENGDRNO9.IsNullOrEmpty() && string.Compare(clsHcType.B3.PANJENGDRNO9, "0") > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM508" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PANJENGDRNO9 + VB.Space(10), 10) + (char)34 + ",");                        //면허번호
                    FstrREC.Append("" + (char)34 + "ITEM509" + (char)34 + ":" + (char)34 + VB.Left(hb.READ_BILL_DRCODE(clsHcType.B3.PANJENGDRNO9) + VB.Space(12), 12) + (char)34 + ",");      //의사명
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM508" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM509" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM508" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM509" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
            }

            //유방암-판독의사
            if (clsHcType.B3.GbWomb == "1")
            {
                if (!clsHcType.B3.PANJENGDRNO10.IsNullOrEmpty() && string.Compare(clsHcType.B3.PANJENGDRNO10, "0") > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM510" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PANJENGDRNO10 + VB.Space(10), 10) + (char)34 + ",");                                                   //면허번호
                    FstrREC.Append("" + (char)34 + "ITEM511" + (char)34 + ":" + (char)34 + VB.Left(cf.Read_Bcode_Name(clsDB.DbCon, "HIC_암판정_의사매칭", clsHcType.B3.PANJENGDRNO10) + VB.Space(12), 12) + (char)34 + ",");           //의사명
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM510" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM511" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM510" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM511" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
            }

            FstrREC.Append("" + (char)34 + "ITEM512" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");                  //장애인안전편의관리비
        }

        /// <summary>
        /// 유방암결과
        /// </summary>
        void fn_BreastCancerResult()
        {
            string strOK = "";

            //유방암 실질분포도
            if (!clsHcType.B3.B_ANATGBN.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM409" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.B_ANATGBN + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM409" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            if (!clsHcType.B3.RESULT0013.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM410" + (char)34 + ":" + (char)34 + clsHcType.B3.RESULT0013 + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM410" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //유방촬영 검사방법 3:Full Pacs
            FstrREC.Append("" + (char)34 + "ITEM411" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");

            if (!VB.Mid(clsHcType.B3.BREAST_S, 1, 2).IsNullOrEmpty() && VB.Mid(clsHcType.B3.BREAST_S, 1, 2) != "00")
            {
                FstrREC.Append("" + (char)34 + "ITEM412" + (char)34 + ":" + (char)34 + VB.Mid(clsHcType.B3.BREAST_S, 1, 2) + (char)34 + ",");

                //기타병형은 제외
                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 1, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 1, 6), VB.Mid(clsHcType.B3.BREAST_S, 1, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM413" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM413" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                strOK = "";

                if (VB.Mid(clsHcType.B3.BREAST_S, 1, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 7, 6), VB.Mid(clsHcType.B3.BREAST_S, 1, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM414" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM414" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";

                if (VB.Mid(clsHcType.B3.BREAST_S, 1, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 13, 6), VB.Mid(clsHcType.B3.BREAST_S, 1, 2)) > 1)
                {
                    strOK = "OK";
                }

                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM415" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM415" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";

                if (VB.Mid(clsHcType.B3.BREAST_S, 1, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 19, 6), VB.Mid(clsHcType.B3.BREAST_S, 1, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM416" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM416" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 1, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 25, 6), VB.Mid(clsHcType.B3.BREAST_S, 1, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM417" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM417" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 1, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 31, 6), VB.Mid(clsHcType.B3.BREAST_S, 1, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM418" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM418" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                //7직접입력
                if (!clsHcType.B3.BREAST_ETC.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM419" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM420" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.BREAST_ETC + VB.Space(20), (20)) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM419" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM420" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 1, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 37, 6), VB.Mid(clsHcType.B3.BREAST_S, 1, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM421" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM421" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 1, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 43, 6), VB.Mid(clsHcType.B3.BREAST_S, 1, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM422" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");

                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM422" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 1, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 49, 6), VB.Mid(clsHcType.B3.BREAST_S, 1, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM423" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM423" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }


                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 1, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 55, 6), VB.Mid(clsHcType.B3.BREAST_S, 1, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM424" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM424" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 1, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 61, 6), VB.Mid(clsHcType.B3.BREAST_S, 1, 2)) > 1)
                {
                    strOK = "OK";
                }

                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM425" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM425" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 1, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 67, 6), VB.Mid(clsHcType.B3.BREAST_S, 1, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM426" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM426" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!clsHcType.B3.BREAST_ETC.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM427" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM428" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.B_ANATETC + VB.Space(20), (20)) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM427" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM428" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM412" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM413" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM414" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM415" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM416" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM417" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM418" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM419" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM420" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM421" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM422" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM423" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM424" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM425" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM426" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM427" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM428" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 유방암결과1
        /// </summary>
        void fn_BreastCancerResult1()
        {
            string strOK = "";

            if (!VB.Mid(clsHcType.B3.BREAST_S, 3, 2).IsNullOrEmpty() && VB.Mid(clsHcType.B3.BREAST_S, 3, 2) != "00")
            {
                FstrREC.Append("" + (char)34 + "ITEM429" + (char)34 + ":" + (char)34 + VB.Mid(clsHcType.B3.BREAST_S, 3, 2) + (char)34 + ",");

                //기타병형은 제외
                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 3, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 1, 6), VB.Mid(clsHcType.B3.BREAST_S, 3, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM430" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM430" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 3, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 7, 6), VB.Mid(clsHcType.B3.BREAST_S, 3, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM431" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM431" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 3, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 13, 6), VB.Mid(clsHcType.B3.BREAST_S, 3, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM432" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM432" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 3, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 19, 6), VB.Mid(clsHcType.B3.BREAST_S, 3, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM433" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM433" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 3, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 25, 6), VB.Mid(clsHcType.B3.BREAST_S, 3, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM434" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM434" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 3, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 31, 6), VB.Mid(clsHcType.B3.BREAST_S, 3, 2)) > 1)
                {
                    strOK = "OK";
                }

                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM435" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM435" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                //7직접입력
                if (!clsHcType.B3.BREAST_ETC.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM436" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM437" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.BREAST_ETC + VB.Space(20), 20) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM436" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM437" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 3, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 37, 6), VB.Mid(clsHcType.B3.BREAST_S, 3, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM438" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM438" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 3, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 43, 6), VB.Mid(clsHcType.B3.BREAST_S, 3, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM439" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM439" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 3, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 49, 6), VB.Mid(clsHcType.B3.BREAST_S, 3, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM440" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM440" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 3, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 55, 6), VB.Mid(clsHcType.B3.BREAST_S, 3, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM441" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM441" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 3, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 61, 6), VB.Mid(clsHcType.B3.BREAST_S, 3, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM442" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM442" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 3, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 67, 6), VB.Mid(clsHcType.B3.BREAST_S, 3, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM443" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM443" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                //7직접입력
                if (!clsHcType.B3.BREAST_ETC.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM444" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM445" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.B_ANATETC + VB.Space(20), 20) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM444" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM445" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM429" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM430" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM431" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM432" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM433" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM434" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM435" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM436" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM437" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM438" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM439" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM440" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM441" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM442" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM443" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM444" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM445" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 유방암결과2
        /// </summary>
        void fn_BreastCancerResult2()
        {
            string strOK = "";

            if (!VB.Mid(clsHcType.B3.BREAST_S, 5, 2).IsNullOrEmpty() && VB.Mid(clsHcType.B3.BREAST_S, 5, 2) != "00")
            {
                FstrREC.Append("" + (char)34 + "ITEM446" + (char)34 + ":" + (char)34 + VB.Mid(clsHcType.B3.BREAST_S, 5, 2) + (char)34 + ",");

                //기타병형은 제외
                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 5, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 1, 6), VB.Mid(clsHcType.B3.BREAST_S, 5, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM447" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM447" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 5, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 7, 6), VB.Mid(clsHcType.B3.BREAST_S, 5, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM448" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM448" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 5, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 7, 6), VB.Mid(clsHcType.B3.BREAST_S, 5, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM449" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM449" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 5, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 19, 6), VB.Mid(clsHcType.B3.BREAST_S, 5, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM450" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM450" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 5, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 25, 6), VB.Mid(clsHcType.B3.BREAST_S, 5, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM451" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM451" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 5, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 31, 6), VB.Mid(clsHcType.B3.BREAST_S, 5, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM452" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM452" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                //7직접입력
                if (!clsHcType.B3.BREAST_ETC.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM453" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM454" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.BREAST_ETC + VB.Space(20), (20)) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM453" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM454" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 5, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 37, 6), VB.Mid(clsHcType.B3.BREAST_S, 5, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM455" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM455" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 5, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 43, 6), VB.Mid(clsHcType.B3.BREAST_S, 5, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM456" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM456" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 5, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 49, 6), VB.Mid(clsHcType.B3.BREAST_S, 5, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM457" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM457" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 5, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 55, 6), VB.Mid(clsHcType.B3.BREAST_S, 5, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM458" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM458" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 5, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 61, 6), VB.Mid(clsHcType.B3.BREAST_S, 5, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM459" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM459" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                strOK = "";
                if (VB.Mid(clsHcType.B3.BREAST_S, 5, 2) == "10")
                {
                    strOK = "";
                }
                else if (VB.L(VB.Mid(clsHcType.B3.BREAST_P, 67, 6), VB.Mid(clsHcType.B3.BREAST_S, 5, 2)) > 1)
                {
                    strOK = "OK";
                }
                if (strOK == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM460" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM460" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                //7직접입력
                if (!clsHcType.B3.BREAST_ETC.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM461" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM462" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.B_ANATETC + VB.Space(20), (20)) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM461" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM462" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM446" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM447" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM448" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM449" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM450" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM451" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM452" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM453" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM454" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM455" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM456" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM457" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM458" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM459" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM460" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM461" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM462" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }

            //유방암-소견기타
            if (!clsHcType.B3.NEW_WOMAN17.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM463" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.NEW_WOMAN17, 20) + VB.Space(20), (20)) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM463" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 유방암판정
        /// </summary>
        /// <param name="argGunDate"></param>
        /// <param name="argGbJin"></param>
        void fn_BreastCancerPan(string argGunDate, string argGbJin)
        {
            string strGunDate = "";
            string strTemp = "";
            string strGbJin = "";


            strGunDate = argGunDate;
            strGbJin = argGbJin;

            if (clsHcType.B3.GBBREAST == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM464" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");                                   //유방암-검진장소
                FstrREC.Append("" + (char)34 + "ITEM465" + (char)34 + ":" + (char)34 + VB.Left(strGunDate + VB.Space(8), 8) + (char)34 + ",");       //유방암 검진일자
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM464" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM465" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
            }
            //유방암-종합판정
            if (!clsHcType.B3.B_PANJENG.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM466" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.B_PANJENG + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM466" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //유방암-기존유방암
            if (VB.Left(clsHcType.B3.NEW_SICK08, 1) == "2")
            {
                FstrREC.Append("" + (char)34 + "ITEM467" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM467" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            strTemp = clsHcType.B3.B_SOGEN.Replace("\r\n", " ");
            FstrREC.Append("" + (char)34 + "ITEM468" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(strTemp, 600) + VB.Space(600), 600) + (char)34 + ",");    //유방암-유방암권고

            //유방암-판정일자
            if (!clsHcType.B3.B_PANJENGDATE.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM469" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.B_PANJENGDATE.Replace("-", "") + VB.Space(8), 8) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM469" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
            }

            if (clsHcType.B3.GBBREAST == "1")
            {
                if (!clsHcType.B3.PanDrNo_New4.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM470" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PanDrNo_New4 + VB.Space(10), 10) + (char)34 + ",");                        //유방암-판정면허번호 10
                    FstrREC.Append("" + (char)34 + "ITEM471" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(clsHcType.B3.PanDrNo_New4) + VB.Space(12), 12) + (char)34 + ",");       //유방암의사명 12
                    FstrREC.Append("" + (char)34 + "ITEM472" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN(clsHcType.B3.PanDrNo_New4) + VB.Space(13), 13) + (char)34 + ",");      //유방암의사주민번호 13
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM470" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM471" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM472" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM470" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM471" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM472" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
            }

            //유방암-진찰료여부
            if (strGbJin == "4")
            {
                FstrREC.Append("" + (char)34 + "ITEM473" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM473" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //유방암-판독의사
            if (clsHcType.B3.GBBREAST == "1")
            {
                if (!clsHcType.B3.PANJENGDRNO8.IsNullOrEmpty() && string.Compare(clsHcType.B3.PANJENGDRNO8, "0") > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM474" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PANJENGDRNO8 + VB.Space(10), 10) + (char)34 + ",");                            //면허번호
                    FstrREC.Append("" + (char)34 + "ITEM475" + (char)34 + ":" + (char)34 + VB.Left(hb.READ_BILL_DRCODE(clsHcType.B3.PANJENGDRNO8) + VB.Space(12), 12) + (char)34 + ",");          //의사명
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM474" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM475" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM474" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM475" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
            }

            FstrREC.Append("" + (char)34 + "ITEM476" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");                   //장애인
        }

        /// <summary>
        /// 간암상반기()
        /// </summary>
        /// <param name="argGbJin"></param>
        /// <param name="argGunDate"></param>
        void fn_LiverFirstHalf(string argGbJin, string argGunDate)
        {
            string strRESULT0005 = "";
            string strRESULT0006 = "";
            string strTemp = "";
            string strGbJin = "";
            string strGunDate = "";

            strGbJin = argGbJin;
            strGunDate = argGunDate;

            if (VB.Left(clsHcType.B3.RESULT0004, 1) != "1")
            {
                if (VB.Mid(clsHcType.B3.RESULT0004, 2, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM301" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM301" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.RESULT0004, 3, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM302" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM302" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.RESULT0004, 4, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM303" + (char)34 + ":" + (char)34 + "4" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM303" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                if (VB.Left(clsHcType.B3.RESULT0004, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM304" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    if (clsHcType.B3.RESULT0015 != "10")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM304" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM304" + (char)34 + ":" + (char)34 + "5" + (char)34 + ",");
                    }
                }
                strRESULT0005 = "";
                strRESULT0006 = "";

                for (int j = 1; j <= 8; j++)
                {
                    if (VB.Mid(clsHcType.B3.RESULT0005, j, 1) == "1")
                    {
                        strRESULT0005 = "OK";
                    }
                }

                for (int j = 1; j <= 8; j++)
                {
                    if (VB.Mid(clsHcType.B3.RESULT0006, j, 1) == "1")
                    {
                        strRESULT0006 = "OK";
                    }
                }

                if (strRESULT0005 == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM305" + (char)34 + ":" + (char)34 + "6" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM305" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                if (strRESULT0006 == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM306" + (char)34 + ":" + (char)34 + "7" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM306" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                if (clsHcType.B3.RESULT0010 != "00000000")
                {
                    FstrREC.Append("" + (char)34 + "ITEM307" + (char)34 + ":" + (char)34 + "8" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM307" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM301" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM302" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM303" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM304" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM305" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM306" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM307" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0005, 1, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM308" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM308" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0005, 2, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM309" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM309" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0005, 3, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM310" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM310" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0005, 4, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM311" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM311" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0005, 5, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM312" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM312" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0005, 6, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM313" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM313" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0005, 7, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM314" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM314" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0005, 8, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM315" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM315" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 1, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM316" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM316" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 2, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM317" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM317" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 3, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM318" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM318" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 4, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM319" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM319" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 5, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM320" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM320" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 6, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM321" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM321" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 7, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM322" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM322" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 8, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM323" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM323" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            FstrREC.Append("" + (char)34 + "ITEM324" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.RESULT0007 + VB.Space(3), 3) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM325" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.RESULT0008 + VB.Space(3), 3) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM326" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.RESULT0009 + VB.Space(3), 3) + (char)34 + ",");

            if (VB.Mid(clsHcType.B3.RESULT0010, 1, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM327" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM327" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0010, 2, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM328" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM328" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0010, 3, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM329" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM329" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0010, 4, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM330" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM330" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0010, 5, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM331" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM331" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0010, 6, 1) == "1" || VB.Mid(clsHcType.B3.RESULT0010, 7, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM332" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM332" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0010, 8, 1) == "1" || !clsHcType.B3.RESULT0012.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM333" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM333" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0010, 6, 1) == "1" && VB.Mid(clsHcType.B3.RESULT0010, 7, 1) == "0")
            {
                FstrREC.Append("" + (char)34 + "ITEM334" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL("담석", 20) + VB.Space(20), 20) + (char)34 + ",");
            }
            else if (VB.Mid(clsHcType.B3.RESULT0010, 6, 1) == "0" && VB.Mid(clsHcType.B3.RESULT0010, 7, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM334" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL("담낭용종", 20) + VB.Space(20), 20) + (char)34 + ",");
            }
            else if (VB.Mid(clsHcType.B3.RESULT0010, 6, 1) == "1" && VB.Mid(clsHcType.B3.RESULT0010, 7, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM334" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL("담석,담낭용종", 20) + VB.Space(20), 20) + (char)34 + ",");
            }
            else if (VB.Mid(clsHcType.B3.RESULT0010, 6, 1) == "0" && VB.Mid(clsHcType.B3.RESULT0010, 7, 1) == "0")
            {
                FstrREC.Append("" + (char)34 + "ITEM334" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }

            FstrREC.Append("" + (char)34 + "ITEM335" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.RESULT0012, 20) + VB.Space(20), 20) + (char)34 + ",");

            //정량법
            if (!clsHcType.B3.Liver_EIA.IsNullOrEmpty())
            {
                //2015-12-30 <부호 공단청구시 오류로 제거함
                clsHcType.B3.Liver_EIA = clsHcType.B3.Liver_EIA.Replace("<", "");
                FstrREC.Append("" + (char)34 + "ITEM336" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");                                                           //간암-혈청알파태아단백-검사방법
                FstrREC.Append("" + (char)34 + "ITEM337" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");                                                   //간암-혈청알파태아단백-정성법-결과
                FstrREC.Append("" + (char)34 + "ITEM338" + (char)34 + ":" + (char)34 + VB.Left(string.Format(clsHcType.B3.Liver_EIA, "0000.0") + VB.Space(6), 6) + (char)34 + ",");           //간암-혈청알파태아단백-검사수치
                FstrREC.Append("" + (char)34 + "ITEM339" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                                           //간암-혈청알파태아단백-단위
                FstrREC.Append("" + (char)34 + "ITEM340" + (char)34 + ":" + (char)34 + "  10.5" + (char)34 + ",");                                                      //간암-혈청알파태아단백-기준치 0~10.5
            }
            else
            {
                FstrREC.Append(VB.Space(15));
                FstrREC.Append("" + (char)34 + "ITEM336" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM337" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM338" + (char)34 + ":" + (char)34 + VB.Space(6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM339" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM340" + (char)34 + ":" + (char)34 + VB.Space(6) + (char)34 + ",");
            }

            if (clsHcType.B3.GbLiver == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM341" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");                               //간암-검진장소
                FstrREC.Append("" + (char)34 + "ITEM342" + (char)34 + ":" + (char)34 + VB.Left(strGunDate + VB.Space(8), 8) + (char)34 + ",");   //간암 검진일자
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM341" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM342" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
            }

            //간암-종합판정
            if (!clsHcType.B3.Liver_PANJENG.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM343" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.Liver_PANJENG + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM343" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //간암-기존간암
            if (VB.Left(clsHcType.B3.NEW_SICK23, 1) == "2")
            {
                FstrREC.Append("" + (char)34 + "ITEM344" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM344" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //간암-간암종합기타
            if (!clsHcType.B3.Liver_JILETC.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM345" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.Liver_JILETC, 20) + VB.Space(20), 20) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM345" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }

            strTemp = clsHcType.B3.L_SOGEN.Replace("\r\n", " ");
            FstrREC.Append("" + (char)34 + "ITEM346" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(strTemp, 600) + VB.Space(600), 600) + (char)34 + ",");    //간암-간암권고


            //간암-판정일자
            if (!clsHcType.B3.L_PANJENGDATE.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM347" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.L_PANJENGDATE.Replace("-", "") + VB.Space(8), 8) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM347" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
            }

            if (clsHcType.B3.GbLiver == "1")
            {
                if (!clsHcType.B3.PanDrNo_New3.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM348" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PanDrNo_New3 + VB.Space(10), 10) + (char)34 + ",");                        //간암-판정면허번호 10
                    FstrREC.Append("" + (char)34 + "ITEM349" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(clsHcType.B3.PanDrNo_New3) + VB.Space(12), 12) + (char)34 + ",");       //간장암의사명 12
                    FstrREC.Append("" + (char)34 + "ITEM350" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN(clsHcType.B3.PanDrNo_New3) + VB.Space(13), 13) + (char)34 + ",");      //간장암의사주민번호 13
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM348" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM349" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM350" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM348" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM349" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM350" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
            }

            //간암-진찰료여부
            if (strGbJin == "3")
            {
                FstrREC.Append("" + (char)34 + "ITEM351" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM351" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //간암-검사의사
            if (clsHcType.B3.GbLiver == "1")
            {
                if (!clsHcType.B3.PANJENGDRNO7.IsNullOrEmpty() && string.Compare(clsHcType.B3.PANJENGDRNO7, "0") > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM352" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PANJENGDRNO7 + VB.Space(10), 10) + (char)34 + ","); //면허번호
                    FstrREC.Append("" + (char)34 + "ITEM353" + (char)34 + ":" + (char)34 + VB.Left(hb.READ_BILL_DRCODE(clsHcType.B3.PANJENGDRNO7) + VB.Space(12), 12) + (char)34 + ","); //의사명
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM352" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM353" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM352" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM353" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
            }

            //간암-장애인안전편의관리비
            FstrREC.Append("" + (char)34 + "ITEM354" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
        }

        /// <summary>
        /// 간암하반기()
        /// </summary>
        /// <param name="argGbJin"></param>
        /// <param name="argGunDate"></param>
        void fn_LiverLastHalf(string argGbJin, string argGunDate)
        {
            string strRESULT0005 = "";
            string strRESULT0006 = "";

            string strTemp = "";
            string strGbJin = "";
            string strGunDate = "";

            strGbJin = argGbJin;
            strGunDate = argGunDate;
            if (VB.Left(clsHcType.B3.RESULT0004, 1) != "1")
            {
                if (VB.Mid(clsHcType.B3.RESULT0004, 2, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM355" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM355" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.RESULT0004, 3, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM356" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM356" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.RESULT0004, 4, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM357" + (char)34 + ":" + (char)34 + "4" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM357" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                if (VB.Left(clsHcType.B3.RESULT0004, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM358" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    if (clsHcType.B3.RESULT0015 != "10")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM358" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM358" + (char)34 + ":" + (char)34 + "5" + (char)34 + ",");
                    }
                }

                strRESULT0005 = "";
                strRESULT0006 = "";

                for (int j = 1; j <= 8; j++)
                {
                    if (VB.Mid(clsHcType.B3.RESULT0005, j, 1) == "1")
                    {
                        strRESULT0005 = "OK";
                    }
                }

                for (int j = 1; j <= 8; j++)
                {
                    if (VB.Mid(clsHcType.B3.RESULT0006, j, 1) == "1")
                    {
                        strRESULT0006 = "OK";
                    }
                }

                if (strRESULT0005 == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM359" + (char)34 + ":" + (char)34 + "6" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM359" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                if (strRESULT0006 == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM360" + (char)34 + ":" + (char)34 + "7" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM360" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                if (clsHcType.B3.RESULT0010 != "00000000")
                {
                    FstrREC.Append("" + (char)34 + "ITEM361" + (char)34 + ":" + (char)34 + "8" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM361" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM355" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM356" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM357" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM358" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM359" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM360" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM361" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0005, 1, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM362" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM362" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }


            if (VB.Mid(clsHcType.B3.RESULT0005, 2, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM363" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM363" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0005, 3, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM364" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM364" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0005, 4, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM365" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM365" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0005, 5, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM366" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM366" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0005, 6, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM367" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM367" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0005, 7, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM368" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM368" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0005, 8, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM369" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM369" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 1, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM370" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM370" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 2, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM371" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM371" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 3, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM372" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM372" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 4, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM373" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM373" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 5, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM374" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM374" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 6, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM375" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM375" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 7, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM376" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM376" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0006, 8, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM377" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM377" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            FstrREC.Append("" + (char)34 + "ITEM378" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.RESULT0007 + VB.Space(3), 3) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM379" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.RESULT0008 + VB.Space(3), 3) + (char)34 + ",");
            FstrREC.Append("" + (char)34 + "ITEM380" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.RESULT0009 + VB.Space(3), 3) + (char)34 + ",");

            if (VB.Mid(clsHcType.B3.RESULT0010, 1, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM381" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM381" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0010, 2, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM382" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM382" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0010, 3, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM383" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM383" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0010, 4, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM384" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM384" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0010, 5, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM385" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM385" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0010, 6, 1) == "1" || VB.Mid(clsHcType.B3.RESULT0010, 7, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM386" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM386" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0010, 8, 1) == "1" || !clsHcType.B3.RESULT0012.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM387" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM387" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.RESULT0010, 6, 1) == "1" && VB.Mid(clsHcType.B3.RESULT0010, 7, 1) == "0")
            {
                FstrREC.Append("" + (char)34 + "ITEM388" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL("담석", 20) + VB.Space(20), 20) + (char)34 + ",");
            }
            else if (VB.Mid(clsHcType.B3.RESULT0010, 6, 1) == "0" && VB.Mid(clsHcType.B3.RESULT0010, 7, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM388" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL("담낭용종", 20) + VB.Space(20), 20) + (char)34 + ",");
            }
            else if (VB.Mid(clsHcType.B3.RESULT0010, 6, 1) == "1" && VB.Mid(clsHcType.B3.RESULT0010, 7, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM388" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL("담석,담낭용종", 20) + VB.Space(20), 20) + (char)34 + ",");
            }
            else if (VB.Mid(clsHcType.B3.RESULT0010, 6, 1) == "0" && VB.Mid(clsHcType.B3.RESULT0010, 7, 1) == "0")
            {
                FstrREC.Append("" + (char)34 + "ITEM388" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }

            FstrREC.Append("" + (char)34 + "ITEM389" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.RESULT0012, 20) + VB.Space(20), 20) + (char)34 + ",");

            //정량법
            if (!clsHcType.B3.Liver_EIA.IsNullOrEmpty())
            {
                //2015-12-30 <부호 공단청구시 오류로 제거함
                clsHcType.B3.Liver_EIA = clsHcType.B3.Liver_EIA.Replace("<", "");
                FstrREC.Append("" + (char)34 + "ITEM390" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");                                                           //간암-혈청알파태아단백-검사방법
                FstrREC.Append("" + (char)34 + "ITEM391" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");                                                      //간암-혈청알파태아단백-정성법-결과
                FstrREC.Append("" + (char)34 + "ITEM392" + (char)34 + ":" + (char)34 + VB.Left(string.Format(clsHcType.B3.Liver_EIA, "0000.0") + VB.Space(6), 6) + (char)34 + ",");           //간암-혈청알파태아단백-검사수치
                FstrREC.Append("" + (char)34 + "ITEM393" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                                           //간암-혈청알파태아단백-단위
                FstrREC.Append("" + (char)34 + "ITEM394" + (char)34 + ":" + (char)34 + "  10.5" + (char)34 + ",");                                                      //간암-혈청알파태아단백-기준치 0~10.5
            }
            else
            {
                FstrREC.Append(VB.Space(15));
                FstrREC.Append("" + (char)34 + "ITEM390" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM391" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM392" + (char)34 + ":" + (char)34 + VB.Space(6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM393" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM394" + (char)34 + ":" + (char)34 + VB.Space(6) + (char)34 + ",");
            }

            if (clsHcType.B3.GbLiver == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM395" + (char)34 + ":" + (char)34 + "2" + (char)34 + ","); //간암-검진장소
                FstrREC.Append("" + (char)34 + "ITEM396" + (char)34 + ":" + (char)34 + VB.Left(strGunDate + VB.Space(8), 8) + (char)34 + ","); //간암 검진일자
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM395" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM396" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
            }

            //간암-종합판정
            if (!clsHcType.B3.Liver_PANJENG.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM397" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.Liver_PANJENG + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM397" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //간암-기존간암
            if (VB.Left(clsHcType.B3.NEW_SICK23, 1) == "2")
            {
                FstrREC.Append("" + (char)34 + "ITEM398" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM398" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //간암-간암종합기타
            if (!clsHcType.B3.Liver_JILETC.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM399" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.Liver_JILETC, 20) + VB.Space(20), 20) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM399" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }

            strTemp = clsHcType.B3.L_SOGEN.Replace("\r\n", " ");
            FstrREC.Append("" + (char)34 + "ITEM400" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(strTemp, 600) + VB.Space(600), 600) + (char)34 + ","); //간암-간암권고

            //간암-판정일자
            if (!clsHcType.B3.L_PANJENGDATE.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM401" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.L_PANJENGDATE.Replace("-", "") + VB.Space(8), 8) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM401" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
            }

            if (clsHcType.B3.GbLiver == "1")
            {
                if (!clsHcType.B3.PanDrNo_New3.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM402" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PanDrNo_New3 + VB.Space(10), 10) + (char)34 + ","); //간암-판정면허번호 10
                    FstrREC.Append("" + (char)34 + "ITEM403" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(clsHcType.B3.PanDrNo_New3) + VB.Space(12), 12) + (char)34 + ",");  //간장암의사명 12
                    FstrREC.Append("" + (char)34 + "ITEM404" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN(clsHcType.B3.PanDrNo_New3) + VB.Space(13), 13) + (char)34 + ","); //간장암의사주민번호 13
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM402" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM403" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM404" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM402" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM403" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM404" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
            }

            //간암-진찰료여부
            if (strGbJin == "3")
            {
                FstrREC.Append("" + (char)34 + "ITEM405" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM405" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //간암-검사의사
            if (clsHcType.B3.GbLiver == "1")
            {
                if (!clsHcType.B3.PANJENGDRNO7.IsNullOrEmpty() && string.Compare(clsHcType.B3.PANJENGDRNO7, "0") > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM406" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PANJENGDRNO7 + VB.Space(10), 10) + (char)34 + ","); //면허번호
                    FstrREC.Append("" + (char)34 + "ITEM407" + (char)34 + ":" + (char)34 + VB.Left(hb.READ_BILL_DRCODE(clsHcType.B3.PANJENGDRNO7) + VB.Space(12), 12) + (char)34 + ","); //의사명
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM406" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM407" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM406" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM407" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
            }

            //간암-장애인안전편의관리비
            FstrREC.Append("" + (char)34 + "ITEM408" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
        }

        /// <summary>
        /// 분변잠혈반응()
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argGunDate"></param>
        void fn_VariationsLatentBloodTest(long argWRTNO, string argGunDate)
        {
            string strGunDate = "";
            long nWRTNO = 0;

            nWRTNO = argWRTNO;
            strGunDate = argGunDate;

            if (!VB.Left(clsHcType.B3.COLON_RESULT, 1).IsNullOrEmpty())
            {
                //2012-07-06부터 분변검사 정량법으로 바뀜                
                FstrREC.Append("" + (char)34 + "ITEM140" + (char)34 + ":" + (char)34 + "2" + (char)34 + ","); //대장암-분변잠혈-검사방법(정량)               

                //대장암-분변잠혈-검사결과 1음성,2양성
                if (VB.Left(clsHcType.B3.COLON_RESULT, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM141" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM141" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM142" + (char)34 + ":" + (char)34 + "100.00" + (char)34 + ",");                                                                                  //대장암-분변잠혈 정량-기준치
                FstrREC.Append("" + (char)34 + "ITEM143" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000.00}", chb.READ_MIR_RESULT2(nWRTNO, "TX26").Replace(">", "")) + VB.Space(6), 6) + (char)34 + ",");      //대장암-분변잠혈 정량-결과수치

                //2019-08-06
                //대장암-검진장소
                if (!VB.Left(clsHcType.B3.COLON_RESULT, 1).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM144" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM144" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                FstrREC.Append("" + (char)34 + "ITEM145" + (char)34 + ":" + (char)34 + VB.Left(strGunDate + VB.Space(8), 8) + (char)34 + ",");           //대장암-분변잠혈-검사일
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM140" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM141" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM142" + (char)34 + ":" + (char)34 + VB.Space(6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM143" + (char)34 + ":" + (char)34 + VB.Space(6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM144" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM145" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 대장이중조영검사
        /// </summary>
        /// <param name="argGunDate"></param>
        /// <param name="argOK"></param>
        /// <param name="argOK1"></param>
        void fn_ColonizationTest(string argGunDate, string argOK, string argOK1)
        {
            string strOK = "";
            string strOK1 = "";
            string strGunDate = "";

            strGunDate = argGunDate;
            strOK = argOK;
            strOK1 = argOK1;

            if (!VB.Mid(clsHcType.B3.COLON_S, 1, 1).IsNullOrEmpty() && VB.Mid(clsHcType.B3.COLON_S, 1, 1) != "0")
            {
                FstrREC.Append("" + (char)34 + "ITEM146" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");              //대장이중조영검사 검사방법 3:Full Pacs
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM146" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");      //대장이중조영검사 검사방법 3:Full Pacs
            }

            if (!VB.Mid(clsHcType.B3.COLON_S, 1, 1).IsNullOrEmpty() && VB.Mid(clsHcType.B3.COLON_S, 1, 1) != "0")
            {
                FstrREC.Append("" + (char)34 + "ITEM147" + (char)34 + ":" + (char)34 + VB.Mid(clsHcType.B3.COLON_S, 1, 1) + (char)34 + ",");

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 1, 3), VB.Mid(clsHcType.B3.COLON_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM148" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM148" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 4, 3), VB.Mid(clsHcType.B3.COLON_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM149" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM149" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 7, 3), VB.Mid(clsHcType.B3.COLON_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM150" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM150" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 10, 3), VB.Mid(clsHcType.B3.COLON_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM151" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM151" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 13, 3), VB.Mid(clsHcType.B3.COLON_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM152" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM152" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 16, 3), VB.Mid(clsHcType.B3.COLON_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM153" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM153" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 19, 3), VB.Mid(clsHcType.B3.COLON_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM154" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM154" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 22, 3), VB.Mid(clsHcType.B3.COLON_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM155" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM155" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 25, 3), VB.Mid(clsHcType.B3.COLON_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM156" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM156" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 28, 3), VB.Mid(clsHcType.B3.COLON_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM157" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM157" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM147" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM148" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM149" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM150" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM151" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM152" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM153" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM154" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM155" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM156" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM157" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (!VB.Mid(clsHcType.B3.COLON_S, 2, 1).IsNullOrEmpty() && VB.Mid(clsHcType.B3.COLON_S, 2, 1) != "0")
            {
                FstrREC.Append("" + (char)34 + "ITEM158" + (char)34 + ":" + (char)34 + VB.Mid(clsHcType.B3.COLON_S, 2, 1) + (char)34 + ",");

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 1, 3), VB.Mid(clsHcType.B3.COLON_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM159" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM159" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 4, 3), VB.Mid(clsHcType.B3.COLON_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM160" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM160" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 7, 3), VB.Mid(clsHcType.B3.COLON_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM161" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM161" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 10, 3), VB.Mid(clsHcType.B3.COLON_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM162" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM162" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 13, 3), VB.Mid(clsHcType.B3.COLON_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM163" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM163" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 16, 3), VB.Mid(clsHcType.B3.COLON_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM164" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM164" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 19, 3), VB.Mid(clsHcType.B3.COLON_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM165" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM165" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 22, 3), VB.Mid(clsHcType.B3.COLON_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM166" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM166" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 25, 3), VB.Mid(clsHcType.B3.COLON_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM167" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM167" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 28, 3), VB.Mid(clsHcType.B3.COLON_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM168" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM168" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM158" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM159" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM160" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM161" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM162" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM163" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM164" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM165" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM166" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM167" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM168" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (!VB.Mid(clsHcType.B3.COLON_S, 3, 1).IsNullOrEmpty() && VB.Mid(clsHcType.B3.COLON_S, 3, 1) != "0")
            {
                FstrREC.Append("" + (char)34 + "ITEM169" + (char)34 + ":" + (char)34 + VB.Mid(clsHcType.B3.COLON_S, 3, 1) + (char)34 + ",");

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 1, 3), VB.Mid(clsHcType.B3.COLON_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM170" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM170" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 4, 3), VB.Mid(clsHcType.B3.COLON_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM171" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM171" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 7, 3), VB.Mid(clsHcType.B3.COLON_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM172" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM172" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 10, 3), VB.Mid(clsHcType.B3.COLON_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM173" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM173" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 13, 3), VB.Mid(clsHcType.B3.COLON_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM174" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM174" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 16, 3), VB.Mid(clsHcType.B3.COLON_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM175" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM175" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 19, 3), VB.Mid(clsHcType.B3.COLON_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM176" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM176" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 22, 3), VB.Mid(clsHcType.B3.COLON_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM177" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM177" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 25, 3), VB.Mid(clsHcType.B3.COLON_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM178" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM178" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 28, 3), VB.Mid(clsHcType.B3.COLON_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM179" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM179" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM169" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM170" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM171" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM172" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM173" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM174" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM175" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM176" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM177" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM178" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM179" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //대장암-대장이중-용종크기
            if (!clsHcType.B3.NEW_SICK33.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM180" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_SICK33 + VB.Space(3), 3) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM180" + (char)34 + ":" + (char)34 + "000" + (char)34 + ",");
            }

            //대장이중검사판독소견-기타
            if (VB.Mid(clsHcType.B3.COLON_B, 1, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM181" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM181" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_B, 2, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM182" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM182" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_B, 3, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM183" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM183" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_B, 4, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM184" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM184" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_B, 5, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM185" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM185" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_B, 6, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM186" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM186" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_B, 7, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM187" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM187" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_B, 8, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM188" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM188" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_B, 9, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM189" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM189" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_B, 10, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM190" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM190" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (!clsHcType.B3.COLON_PETC.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM191" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.COLON_PETC, 20) + VB.Space(20), 20) + (char)34 + ","); //대장암-대장이중-소견기타
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM191" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }

            if (!clsHcType.B3.COLON_S.IsNullOrEmpty() && clsHcType.B3.COLON_S != "000")
            {
                FstrREC.Append("" + (char)34 + "ITEM192" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");                                       //대장이중 검진장소
                FstrREC.Append("" + (char)34 + "ITEM193" + (char)34 + ":" + (char)34 + VB.Left(strGunDate + VB.Space(8), 8) + (char)34 + ",");      //대장이중 검진일자     
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM192" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM193" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
            }

            FstrREC.Append("" + (char)34 + "ITEM194" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");

            if (strOK1 == "OK")
            {
                if (!clsHcType.B3.RESULT0002.IsNullOrEmpty())
                {
                    //맹장삽입여부
                    FstrREC.Append("" + (char)34 + "ITEM195" + (char)34 + ":" + (char)34 + clsHcType.B3.RESULT0002 + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM195" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                if (!clsHcType.B3.RESULT0003.IsNullOrEmpty())
                {
                    //장정결도
                    FstrREC.Append("" + (char)34 + "ITEM196" + (char)34 + ":" + (char)34 + clsHcType.B3.RESULT0003 + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM196" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM195" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM196" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 대장내시경검사
        /// </summary>
        void fn_ColonoScopy()
        {
            if (!VB.Mid(clsHcType.B3.COLON_SENDO, 1, 1).IsNullOrEmpty() && VB.Mid(clsHcType.B3.COLON_SENDO, 1, 1) != "0")
            {
                FstrREC.Append("" + (char)34 + "ITEM197" + (char)34 + ":" + (char)34 + VB.Mid(clsHcType.B3.COLON_SENDO, 1, 1) + (char)34 + ",");

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 1, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM198" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM198" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 4, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM199" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM199" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 7, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM200" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM200" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 10, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM201" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM201" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 13, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM202" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM202" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 16, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM203" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM203" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 19, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM204" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM204" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 22, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM205" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM205" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 25, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM206" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM206" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 28, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM207" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM207" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM197" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM198" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM199" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM200" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM201" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM202" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM203" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM204" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM205" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM206" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM207" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (!VB.Mid(clsHcType.B3.COLON_SENDO, 2, 1).IsNullOrEmpty() && VB.Mid(clsHcType.B3.COLON_SENDO, 2, 1) != "0")
            {
                FstrREC.Append("" + (char)34 + "ITEM208" + (char)34 + ":" + (char)34 + VB.Mid(clsHcType.B3.COLON_SENDO, 2, 1) + (char)34 + ",");

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 1, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM209" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM209" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 4, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM210" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM210" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 7, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM211" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM211" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 10, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM212" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM212" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 13, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM213" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM213" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 16, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM214" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM214" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 19, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM215" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM215" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 22, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM216" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM216" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 25, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM217" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM217" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 28, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM218" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM218" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM208" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM209" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM210" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM211" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM212" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM213" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM214" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM215" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM216" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM217" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM218" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (!VB.Mid(clsHcType.B3.COLON_SENDO, 3, 1).IsNullOrEmpty() && VB.Mid(clsHcType.B3.COLON_SENDO, 3, 1) != "0")
            {
                FstrREC.Append("" + (char)34 + "ITEM219" + (char)34 + ":" + (char)34 + VB.Mid(clsHcType.B3.COLON_SENDO, 3, 1) + (char)34 + ",");

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 1, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM220" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM220" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 4, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM221" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM221" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 7, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM222" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM222" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 10, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM223" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM223" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 13, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM224" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM224" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 16, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM225" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM225" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 19, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM226" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM226" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 22, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM227" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM227" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 25, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM228" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM228" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.COLON_P, 28, 3), VB.Mid(clsHcType.B3.COLON_SENDO, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM229" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM229" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM219" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM220" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM221" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM222" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM223" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM224" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM225" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM226" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM227" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM228" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM229" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //대장암-대장내시경-용종크기
            if (!clsHcType.B3.NEW_SICK38.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM230" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_SICK38 + VB.Space(3), 3) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM230" + (char)34 + ":" + (char)34 + "999" + (char)34 + ",");
            }

            //대장암-대장내시경-용종절제실시
            if (clsHcType.B3.NEW_SICK34 != "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM231" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_SICK34 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM231" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
            }

            //대장내시경 전처치 재료(1.4L 2.2L 3.354ml)
            if (VB.Mid(clsHcType.B3.COLON_SENDO, 1, 1).IsNullOrEmpty() && VB.Mid(clsHcType.B3.COLON_SENDO, 1, 1) != "0")
            {
                FstrREC.Append("" + (char)34 + "ITEM232" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM232" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //대장내시경소견-기타
            if (VB.Mid(clsHcType.B3.COLON_BENDO, 1, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM233" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM233" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_BENDO, 2, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM234" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM234" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_BENDO, 3, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM235" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM235" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_BENDO, 4, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM236" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM236" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_BENDO, 5, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM237" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM237" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_BENDO, 6, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM238" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM238" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_BENDO, 7, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM239" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM239" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_BENDO, 8, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM240" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM240" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_BENDO, 9, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM241" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM241" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.COLON_BENDO, 10, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM242" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM242" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //대장암-대장내시경-소견기타
            if (!clsHcType.B3.COLON_ENDOETC.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM243" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.COLON_ENDOETC, 20) + VB.Space(20), 20) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM243" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }

            if (!clsHcType.B3.C_ANATGBN.IsNullOrEmpty())
            {
                //대장암-대장조직진단-필요
                FstrREC.Append("" + (char)34 + "ITEM244" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.C_ANATGBN + VB.Space(1), 1) + (char)34 + ",");
                //2015-09-01 포셉
                FstrREC.Append("" + (char)34 + "ITEM609" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                if (clsHcType.B3.C_ANATGBN == "1")
                {
                    //포셉(일회용)
                    FstrREC.Append("" + (char)34 + "ITEM245" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    //포셉
                    FstrREC.Append("" + (char)34 + "ITEM245" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM244" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM609" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM245" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 대장조직진단
        /// </summary>
        /// <param name="argGunDate"></param>
        void fn_ColonoTissue(string argGunDate)
        {
            string strGunDate = "";
            string strTemp = "";
            strGunDate = argGunDate;

            if (!clsHcType.B3.NEW_SICK71.IsNullOrEmpty())
            {
                //대장암-대장조직진단
                FstrREC.Append("" + (char)34 + "ITEM246" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_SICK71 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                //대장암-조직검사 개수
                FstrREC.Append("" + (char)34 + "ITEM246" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //대장암-조직진단시암
            if (VB.Mid(clsHcType.B3.NEW_SICK69, 1, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM247" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM247" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK69, 2, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM248" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM248" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK69, 3, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM249" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM249" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK69, 4, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM250" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM250" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK69, 5, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM251" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM251" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK69, 6, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM252" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM252" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK69, 7, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM253" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM253" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK69, 8, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM254" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM254" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK69, 9, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM255" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM255" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK69, 10, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM256" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM256" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK69, 11, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM257" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM257" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK69, 12, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM258" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM258" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK69, 13, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM259" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM259" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //대장암-조직진단시암-기타
            if (!clsHcType.B3.NEW_SICK72.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM260" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.NEW_SICK72, 20) + VB.Space(20), 20) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM260" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }

            //대장암-조직진단시기타
            if (VB.Mid(clsHcType.B3.NEW_SICK73, 1, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM261" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM261" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK73, 2, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM262" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM262" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK73, 3, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM263" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM263" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK73, 4, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM264" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM264" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.NEW_SICK73, 5, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM265" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM265" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //대장암-조직진단시기타-기타
            if (!clsHcType.B3.NEW_SICK74.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM266" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.NEW_SICK74, 20) + VB.Space(20), 20) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM266" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }

            if (!clsHcType.B3.NEW_SICK71.IsNullOrEmpty() || (!clsHcType.B3.COLON_SENDO.IsNullOrEmpty() && clsHcType.B3.COLON_SENDO != "000"))
            {
                FstrREC.Append("" + (char)34 + "ITEM267" + (char)34 + ":" + (char)34 + "2" + (char)34 + ","); //대장암-대장내시경,조직진단 장소
                FstrREC.Append("" + (char)34 + "ITEM268" + (char)34 + ":" + (char)34 + VB.Left(strGunDate + VB.Space(8), 8) + (char)34 + ","); //대장내시경,조직진단 검진일자
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM267" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM268" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
            }

            //대장암-기존대장암
            if (VB.Left(clsHcType.B3.NEW_SICK13, 1) == "2")
            {
                FstrREC.Append("" + (char)34 + "ITEM269" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM269" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //2020년 변경사항
            strTemp = "";
            if (!clsHcType.B3.C_SOGEN.IsNullOrEmpty() && clsHcType.B3.C_SOGEN2.IsNullOrEmpty() && clsHcType.B3.C_SOGEN3.IsNullOrEmpty())
            {
                strTemp = clsHcType.B3.C_SOGEN.Replace("\r\n", " ");

                FstrREC.Append("" + (char)34 + "ITEM270" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.C_PANJENG + VB.Space(1), 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM271" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM272" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(strTemp, 600) + VB.Space(600), 600) + (char)34 + ",");

                FstrREC.Append("" + (char)34 + "ITEM273" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM274" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM275" + (char)34 + ":" + (char)34 + VB.Space(600) + (char)34 + ",");

                FstrREC.Append("" + (char)34 + "ITEM276" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM277" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM278" + (char)34 + ":" + (char)34 + VB.Space(600) + (char)34 + ",");
            }
            else if (clsHcType.B3.C_SOGEN.IsNullOrEmpty() && !clsHcType.B3.C_SOGEN2.IsNullOrEmpty() && clsHcType.B3.C_SOGEN3.IsNullOrEmpty())
            {
                strTemp = clsHcType.B3.C_SOGEN2.Replace("\r\n", " ");

                FstrREC.Append("" + (char)34 + "ITEM270" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM271" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM272" + (char)34 + ":" + (char)34 + VB.Space(600) + (char)34 + ",");

                FstrREC.Append("" + (char)34 + "ITEM273" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.C_PANJENG + VB.Space(1), 1) + (char)34 + ",");
                if (!clsHcType.B3.C_JILETC.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM274" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.C_JILETC, 20) + VB.Space(20), 20) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM274" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM275" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(strTemp, 600) + VB.Space(600), 600) + (char)34 + ",");

                FstrREC.Append("" + (char)34 + "ITEM276" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM277" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM278" + (char)34 + ":" + (char)34 + VB.Space(600) + (char)34 + ",");

            }
            else if (clsHcType.B3.C_SOGEN.IsNullOrEmpty() && clsHcType.B3.C_SOGEN2.IsNullOrEmpty() && !clsHcType.B3.C_SOGEN3.IsNullOrEmpty())
            {
                strTemp = clsHcType.B3.C_SOGEN3.Replace("\r\n", " ");

                FstrREC.Append("" + (char)34 + "ITEM270" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM271" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM272" + (char)34 + ":" + (char)34 + VB.Space(600) + (char)34 + ",");

                FstrREC.Append("" + (char)34 + "ITEM273" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM274" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM275" + (char)34 + ":" + (char)34 + VB.Space(600) + (char)34 + ",");

                FstrREC.Append("" + (char)34 + "ITEM276" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                if (!clsHcType.B3.C_JILETC.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM277" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.C_JILETC, 20) + VB.Space(20), 20) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM277" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                }
                FstrREC.Append("" + (char)34 + "ITEM278" + (char)34 + ":" + (char)34 + VB.Space(600) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM270" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM271" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM272" + (char)34 + ":" + (char)34 + VB.Space(600) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM273" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM274" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM275" + (char)34 + ":" + (char)34 + VB.Space(600) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM276" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM277" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM278" + (char)34 + ":" + (char)34 + VB.Space(600) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 대장종합판정
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argGbJin"></param>
        void fn_ColonoTotalPan(long argWRTNO, string argGbJin)
        {
            long nWRTNO = 0;
            string strGbJin = "";
            string[] strExCode = { "TX26" };

            nWRTNO = argWRTNO;
            strGbJin = argGbJin;

            //분변잠혈
            if (hicResultService.GetCountbyWrtNo(nWRTNO, strExCode) > 0)
            {
                //분변잠혈
                if (!clsHcType.B3.C_PANJENGDATE.IsNullOrEmpty())
                {
                    //대장암-판정일자
                    FstrREC.Append("" + (char)34 + "ITEM279" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.C_PANJENGDATE.Replace("-", "") + VB.Space(8), 8) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM279" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
                }

                if (clsHcType.B3.GBRECTUM == "1")
                {
                    if (!clsHcType.B3.PanDrNo_New2.IsNullOrEmpty())
                    {
                        FstrREC.Append("" + (char)34 + "ITEM280" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PanDrNo_New2 + VB.Space(10), 10) + (char)34 + ","); //대장암-판정면허번호 10
                        FstrREC.Append("" + (char)34 + "ITEM281" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(clsHcType.B3.PanDrNo_New2) + VB.Space(12), 12) + (char)34 + ","); //대장암의사명 12
                        FstrREC.Append("" + (char)34 + "ITEM282" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN(clsHcType.B3.PanDrNo_New2) + VB.Space(13), 13) + (char)34 + ","); //대장암의사주민번호 13
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM280" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM281" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM282" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM280" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM281" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM282" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM279" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM280" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM281" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM282" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
            }

            //'대장조영
            string[] strExCode1 = { "TX31" };

            if (hicResultService.GetCountbyWrtNo(nWRTNO, strExCode1) > 0)
            {
                //대장조영
                if (!clsHcType.B3.C_PANJENGDATE.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM283" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.C_PANJENGDATE.Replace("-", "") + VB.Space(8), 8) + (char)34 + ","); //대장암-판정일자
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM283" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
                }
                if (clsHcType.B3.GBRECTUM == "1")
                {
                    if (!clsHcType.B3.PanDrNo_New2.IsNullOrEmpty())
                    {
                        FstrREC.Append("" + (char)34 + "ITEM284" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PanDrNo_New2 + VB.Space(10), 10) + (char)34 + ","); //대장암-판정면허번호 10
                        FstrREC.Append("" + (char)34 + "ITEM285" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(clsHcType.B3.PanDrNo_New2) + VB.Space(12), 12) + (char)34 + ",");  //대장암의사명 12
                        FstrREC.Append("" + (char)34 + "ITEM286" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN(clsHcType.B3.PanDrNo_New2) + VB.Space(13), 13) + (char)34 + ","); //대장암의사주민번호 13
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM284" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM285" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM286" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM284" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM285" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM286" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM283" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM284" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM285" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM286" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
            }

            //'대장내시경
            string[] strExCode2 = { "TX32", "TX64", "TX41" };

            if (hicResultService.GetCountbyWrtNo(nWRTNO, strExCode2) > 0)
            {
                //대장내시경
                if (!clsHcType.B3.C_PANJENGDATE.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM287" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.C_PANJENGDATE.Replace("-", "") + VB.Space(8), 8) + (char)34 + ","); //대장암-판정일자
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM287" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
                }
                if (clsHcType.B3.GBRECTUM == "1")
                {
                    if (!clsHcType.B3.PanDrNo_New2.IsNullOrEmpty())
                    {
                        FstrREC.Append("" + (char)34 + "ITEM288" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PanDrNo_New2 + VB.Space(10), 10) + (char)34 + ","); //대장암-판정면허번호 10
                        FstrREC.Append("" + (char)34 + "ITEM289" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(clsHcType.B3.PanDrNo_New2) + VB.Space(12), 12) + (char)34 + ","); //대장암의사명 12
                        FstrREC.Append("" + (char)34 + "ITEM290" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN(clsHcType.B3.PanDrNo_New2) + VB.Space(13), 13) + (char)34 + ","); //대장암의사주민번호 13
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM288" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM289" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM290" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM288" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM289" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM290" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM287" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM288" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM289" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM290" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
            }


            //대장암 - 진찰료여부
            if (strGbJin == "2")
            {
                FstrREC.Append("" + (char)34 + "ITEM291" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM291" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //판독의사
            if (clsHcType.B3.GBRECTUM == "1")
            {
                if (!clsHcType.B3.PANJENGDRNO4.IsNullOrEmpty() && string.Compare(clsHcType.B3.PANJENGDRNO4, "0") > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM292" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PANJENGDRNO4 + VB.Space(10), 10) + (char)34 + ","); //면허번호
                    FstrREC.Append("" + (char)34 + "ITEM293" + (char)34 + ":" + (char)34 + VB.Left(hb.READ_BILL_DRCODE(clsHcType.B3.PANJENGDRNO4) + VB.Space(12), 12) + (char)34 + ","); //의사명
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM292" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM293" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                }
            }
            else
            {                
                FstrREC.Append("" + (char)34 + "ITEM292" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM293" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
            }

            //검사의사
            if (clsHcType.B3.GBRECTUM == "1")
            {
                if (!clsHcType.B3.PANJENGDRNO5.IsNullOrEmpty() && string.Compare(clsHcType.B3.PANJENGDRNO5, "0") > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM294" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PANJENGDRNO5 + VB.Space(10), 10) + (char)34 + ",");                            //면허번호
                    FstrREC.Append("" + (char)34 + "ITEM295" + (char)34 + ":" + (char)34 + VB.Left(hb.READ_BILL_DRCODE(clsHcType.B3.PANJENGDRNO5) + VB.Space(12), 12) + (char)34 + ",");       //의사명
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM294" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM295" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM294" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM295" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
            }

            //병리진단의사
            if (clsHcType.B3.GBRECTUM == "1")
            {
                if (!clsHcType.B3.PANJENGDRNO6.IsNullOrEmpty() && string.Compare(clsHcType.B3.PANJENGDRNO6, "0") > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM296" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PANJENGDRNO6 + VB.Space(10), 10) + (char)34 + ",");                                            //면허번호
                    FstrREC.Append("" + (char)34 + "ITEM297" + (char)34 + ":" + (char)34 + VB.Left(cf.Read_Bcode_Name(clsDB.DbCon, "HIC_암판정_의사매칭", clsHcType.B3.PANJENGDRNO6) + VB.Space(12), 12) + (char)34 + ","); //의사명
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM296" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM297" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM296" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM297" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
            }

            FstrREC.Append("" + (char)34 + "ITEM298" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");           //장애인 안전편의 관리비(분변잠혈)
            FstrREC.Append("" + (char)34 + "ITEM299" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");           //장애인 안전편의 관리비(대장조영)
            FstrREC.Append("" + (char)34 + "ITEM300" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");           //장애인 안전편의 관리비(대장내시경)
        }

        /// <summary>
        /// 위장조영검사()
        /// </summary>
        /// <param name="argGunDate"></param>
        void fn_GastroIntestinalIllumination(string argGunDate)
        {
            string strGunDate = "";

            strGunDate = argGunDate;

            if (!VB.Mid(clsHcType.B3.Stomach_S, 1, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM11" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");                     //위장조영검사방법 3:Full Pacs
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM11" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            if (!VB.Mid(clsHcType.B3.Stomach_S, 1, 1).IsNullOrEmpty() && VB.Mid(clsHcType.B3.Stomach_S, 1, 1) != "0")
            {
                FstrREC.Append("" + (char)34 + "ITEM12" + (char)34 + ":" + (char)34 + VB.Mid(clsHcType.B3.Stomach_S, 1, 1) + (char)34 + ",");

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 1, 3), VB.Mid(clsHcType.B3.Stomach_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 4, 3), VB.Mid(clsHcType.B3.Stomach_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 7, 3), VB.Mid(clsHcType.B3.Stomach_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 10, 3), VB.Mid(clsHcType.B3.Stomach_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 13, 3), VB.Mid(clsHcType.B3.Stomach_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 16, 3), VB.Mid(clsHcType.B3.Stomach_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 19, 3), VB.Mid(clsHcType.B3.Stomach_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 22, 3), VB.Mid(clsHcType.B3.Stomach_S, 1, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM12" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (!VB.Mid(clsHcType.B3.Stomach_S, 2, 1).IsNullOrEmpty() && VB.Mid(clsHcType.B3.Stomach_S, 2, 1) != "0")
            {
                FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + VB.Mid(clsHcType.B3.Stomach_S, 2, 1) + (char)34 + ",");
                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 1, 3), VB.Mid(clsHcType.B3.Stomach_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 4, 3), VB.Mid(clsHcType.B3.Stomach_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 7, 3), VB.Mid(clsHcType.B3.Stomach_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 10, 3), VB.Mid(clsHcType.B3.Stomach_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 13, 3), VB.Mid(clsHcType.B3.Stomach_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 16, 3), VB.Mid(clsHcType.B3.Stomach_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 19, 3), VB.Mid(clsHcType.B3.Stomach_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 22, 3), VB.Mid(clsHcType.B3.Stomach_S, 2, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (!VB.Mid(clsHcType.B3.Stomach_S, 3, 1).IsNullOrEmpty() && VB.Mid(clsHcType.B3.Stomach_S, 3, 1) != "0")
            {
                FstrREC.Append("" + (char)34 + "ITEM30" + (char)34 + ":" + (char)34 + VB.Mid(clsHcType.B3.Stomach_S, 3, 1) + (char)34 + ",");
                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 1, 3), VB.Mid(clsHcType.B3.Stomach_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 4, 3), VB.Mid(clsHcType.B3.Stomach_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM32" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM32" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 7, 3), VB.Mid(clsHcType.B3.Stomach_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 10, 3), VB.Mid(clsHcType.B3.Stomach_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 13, 3), VB.Mid(clsHcType.B3.Stomach_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 16, 3), VB.Mid(clsHcType.B3.Stomach_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 19, 3), VB.Mid(clsHcType.B3.Stomach_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM37" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM37" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.L(VB.Mid(clsHcType.B3.Stomach_P, 22, 3), VB.Mid(clsHcType.B3.Stomach_S, 3, 1)) > 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM30" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM32" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM37" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //위장조영판독소견-기타
            if (VB.Mid(clsHcType.B3.Stomach_B, 1, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM39" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM39" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.Stomach_B, 2, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM40" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM40" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.Stomach_B, 3, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM41" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM41" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.Stomach_B, 4, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM42" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM42" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.Stomach_B, 5, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM43" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM43" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.Stomach_B, 6, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM44" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM44" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.Stomach_B, 7, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM45" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM45" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (VB.Mid(clsHcType.B3.Stomach_B, 8, 1) == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM46" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM46" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (!clsHcType.B3.Stomach_PETC.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM47" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.Stomach_PETC, 20) + VB.Space(20), 20) + (char)34 + ","); //위암-위장조영-소견기타
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM47" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
            }

            if ((clsHcType.B3.Stomach_SENDO.IsNullOrEmpty() || clsHcType.B3.Stomach_SENDO == "000") && !clsHcType.B3.Stomach_S.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM48" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");                                     //위조영-검진장소
                FstrREC.Append("" + (char)34 + "ITEM49" + (char)34 + ":" + (char)34 + VB.Left(strGunDate + VB.Space(8), 8) + (char)34 + ",");    //위조영-검진일자
                FstrREC.Append("" + (char)34 + "ITEM50" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                     //위조영-조영제(1.300g)
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM48" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM49" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM50" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 위내시경검사()
        /// </summary>
        /// <param name="argCHK1"></param>
        /// <param name="argJepDate"></param>
        /// <param name="argGunDate"></param>
        void fn_GastroScope(string argCHK1, string argJepDate, string argGunDate)
        {
            string strCHK1 = "";
            string strTemp = "";
            //'삭제하기
            int K = 0;
            int ss = 0;
            string strJepDate = "";
            string strGunDate = "";

            strCHK1 = argCHK1;
            strJepDate = argJepDate;
            strGunDate = argGunDate;

            //'2016-02-25 1Byte->2Byte로 변경됨
            if (strCHK1 == "OK")
            {
                if (string.Compare(VB.Mid(clsHcType.B3.Stomach_SENDO, 1, 2), "01") >= 0)
                {
                    strTemp = VB.Mid(clsHcType.B3.Stomach_SENDO, 1, 2).Replace("0", "");
                    FstrREC.Append("" + (char)34 + "ITEM51" + (char)34 + ":" + (char)34 + VB.Left(strTemp + VB.Space(2), 2) + (char)34 + ",");

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 1, 3);
                    if (string.Compare(VB.Mid(strTemp, 1, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM52" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM52" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 4, 3);
                    if (string.Compare(VB.Mid(strTemp, 1, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM53" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM53" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 7, 3);
                    if (string.Compare(VB.Mid(strTemp, 1, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM54" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM54" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 10, 3);
                    if (string.Compare(VB.Mid(strTemp, 1, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM55" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM55" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 13, 3);
                    if (string.Compare(VB.Mid(strTemp, 1, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM56" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM56" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 16, 3);
                    if (string.Compare(VB.Mid(strTemp, 1, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM57" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM57" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 19, 3);
                    if (string.Compare(VB.Mid(strTemp, 1, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM58" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM58" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 22, 3);
                    if (string.Compare(VB.Mid(strTemp, 1, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM59" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM59" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM51" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM52" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM53" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM54" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM55" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM56" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM57" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM58" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM59" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (string.Compare(VB.Mid(clsHcType.B3.Stomach_SENDO, 3, 2), "01") >= 0)
                {
                    strTemp = VB.Mid(clsHcType.B3.Stomach_SENDO, 3, 2).Replace("0", "");
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + VB.Left(strTemp + VB.Space(2), 2) + (char)34 + ",");

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 1, 3);
                    if (string.Compare(VB.Mid(strTemp, 2, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 4, 3);
                    if (string.Compare(VB.Mid(strTemp, 2, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM62" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM62" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 7, 3);
                    if (string.Compare(VB.Mid(strTemp, 2, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM63" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM63" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 10, 3);
                    if (string.Compare(VB.Mid(strTemp, 2, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM64" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM64" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 13, 3);
                    if (string.Compare(VB.Mid(strTemp, 2, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM65" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM65" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 16, 3);
                    if (string.Compare(VB.Mid(strTemp, 2, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM66" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM66" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 19, 3);
                    if (string.Compare(VB.Mid(strTemp, 2, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 22, 3);
                    if (string.Compare(VB.Mid(strTemp, 2, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM68" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM68" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                }
                else
                { 
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM62" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM63" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM64" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM65" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM66" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM68" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (string.Compare(VB.Mid(clsHcType.B3.Stomach_SENDO, 5, 2), "01") >= 0)
                {
                    strTemp = VB.Mid(clsHcType.B3.Stomach_SENDO, 5, 2).Replace("0", "");
                    FstrREC.Append("" + (char)34 + "ITEM69" + (char)34 + ":" + (char)34 + VB.Left(strTemp + VB.Space(2), 2) + (char)34 + ",");

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 1, 3);
                    if (string.Compare(VB.Mid(strTemp, 3, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM70" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM70" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 4, 3);
                    if (string.Compare(VB.Mid(strTemp, 3, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM71" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM71" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 7, 3);
                    if (string.Compare(VB.Mid(strTemp, 3, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM72" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM72" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 10, 3);
                    if (string.Compare(VB.Mid(strTemp, 3, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM73" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM73" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 13, 3);
                    if (string.Compare(VB.Mid(strTemp, 3, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM74" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM74" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 16, 3);
                    if (string.Compare(VB.Mid(strTemp, 3, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM75" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM75" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 19, 3);
                    if (string.Compare(VB.Mid(strTemp, 3, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM76" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM76" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    strTemp = VB.Mid(clsHcType.B3.Stomach_PENDO, 22, 3);
                    if (string.Compare(VB.Mid(strTemp, 3, 1), "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM77" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM77" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                }
                else
                { 
                    FstrREC.Append("" + (char)34 + "ITEM69" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM70" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM71" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM72" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM73" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM74" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM75" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM76" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM77" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                //위내시경관찰소견-기타
                if (VB.Mid(clsHcType.B3.Stomach_BENDO, 1, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM78" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM78" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.Stomach_BENDO, 2, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM79" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM79" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.Stomach_BENDO, 3, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM80" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM80" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.Stomach_BENDO, 4, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM81" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM81" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.Stomach_BENDO, 5, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM82" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM82" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.Stomach_BENDO, 6, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM83" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM83" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.Stomach_BENDO, 7, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM84" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM84" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.Stomach_BENDO, 8, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM85" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM85" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!clsHcType.B3.Stomach_ENDOETC.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM86" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.Stomach_ENDOETC, 20) + VB.Space(20), 20) + (char)34 + ",");            //위암-위내시경-소견기타
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM86" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                }

                if (!clsHcType.B3.S_ANATGBN.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM87" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.S_ANATGBN + VB.Space(1), 1) + (char)34 + ",");          //위암-위조직진단필요
                    //2015-09-01 포셉(센타는 재사용을 사용함)
                    if (clsHcType.B3.S_ANATGBN == "1")
                    {
                        //2015-10-19일부터 1회용 포셉을 사용함                        
                        FstrREC.Append("" + (char)34 + "ITEM88" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");            //포셉(일회용)
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM88" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");           //포셉
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM87" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM88" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");               //포셉
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM51" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM52" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM53" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM54" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM55" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM56" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM57" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM58" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM59" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM62" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM63" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM64" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM65" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM66" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM68" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM69" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM70" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM71" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM72" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM73" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM74" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM75" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM76" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM77" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM78" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM79" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM80" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM81" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM82" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM83" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM84" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM85" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM86" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM87" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM88" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 위조직진단
        /// </summary>
        /// <param name="argCHK1"></param>
        /// <param name="argGunDate"></param>
        void fn_GastricTissue(string argCHK1, string argGunDate)
        {
            string strCHK1 = "";
            string strGunDate = "";

            strCHK1 = argCHK1;
            strGunDate = argGunDate;

            if (strCHK1 == "OK")
            {
                if (!clsHcType.B3.RESULT0001.IsNullOrEmpty())
                {
                    if (VB.Left(clsHcType.B3.RESULT0001, 1) == "0")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM89" + (char)34 + ":" + (char)34 + VB.Right(clsHcType.B3.RESULT0001, 1) + (char)34 + ","); //위암-위조직진단
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM89" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.RESULT0001 + VB.Space(2), 2) + (char)34 + ",");
                    }
                    //2019년도 변경사항
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM89" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                }

                //위암-조직진단시 암- 암종
                if (VB.Mid(clsHcType.B3.NEW_SICK63, 1, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM90" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM90" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK63, 2, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM91" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM91" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }


                if (VB.Mid(clsHcType.B3.NEW_SICK63, 3, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM92" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM92" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK63, 4, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM93" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM93" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK63, 5, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM94" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM94" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK63, 6, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM95" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM95" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK63, 7, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM96" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM96" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK63, 8, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM97" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM97" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK63, 9, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM98" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                { 
                    FstrREC.Append("" + (char)34 + "ITEM98" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK63, 10, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM99" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                { 
                    FstrREC.Append("" + (char)34 + "ITEM99" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK63, 11, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM100" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                { 
                    FstrREC.Append("" + (char)34 + "ITEM100" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK63, 12, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM101" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                { 
                    FstrREC.Append("" + (char)34 + "ITEM101" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK63, 13, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM102" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM102" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK63, 14, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM103" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM103" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (clsHcType.B3.NEW_SICK66.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM104" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.NEW_SICK66, 20) + VB.Space(20), 20) + (char)34 + ",");     //위암-조직진단시암-기타
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM104" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK67, 1, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM105" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM105" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK67, 2, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM106" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM106" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK67, 3, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM107" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM107" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK67, 4, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM108" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM108" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK67, 5, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM109" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM109" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK67, 6, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM110" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM110" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK67, 7, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM111" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM111" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Mid(clsHcType.B3.NEW_SICK67, 8, 1) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM112" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM112" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!clsHcType.B3.NEW_SICK68.IsNullOrEmpty())
                {
                    if (VB.Left(clsHcType.B3.NEW_SICK68 + VB.Space(20), 20).Length % 2 > 0)
                    {
                        clsHcType.B3.NEW_SICK68 = VB.Left(VB.Left(clsHcType.B3.NEW_SICK68 + VB.Space(20), 20), VB.Left(clsHcType.B3.NEW_SICK68 + VB.Space(20), 20).Length - 1);
                    }
                    FstrREC.Append("" + (char)34 + "ITEM113" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_SICK68 + VB.Space(20), 20) + (char)34 + ","); //위암-조직진단시기타-기타
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM113" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                }

                if (!clsHcType.B3.Stomach_SENDO.IsNullOrEmpty() && (clsHcType.B3.Stomach_S.IsNullOrEmpty() || clsHcType.B3.Stomach_S == "000"))
                {
                    FstrREC.Append("" + (char)34 + "ITEM114" + (char)34 + ":" + (char)34 + "2" + (char)34 + ","); //위내시경 및 조직진단 검진장소
                    FstrREC.Append("" + (char)34 + "ITEM115" + (char)34 + ":" + (char)34 + VB.Left(strGunDate + VB.Space(8), 8) + (char)34 + ","); //위내시경 및 조직진단 검진일자
                }
                else
                { 
                    FstrREC.Append("" + (char)34 + "ITEM114" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM115" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM89" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM90" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM91" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM92" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM93" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM94" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM95" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM96" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM97" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM98" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM99" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM100" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM101" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM102" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM103" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM104" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM105" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM106" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM107" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM108" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM109" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM110" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM111" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM112" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM113" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM114" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM115" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 위종합판정
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argCHK1"></param>
        /// <param name="argGbJin"></param>
        void fn_StomachTotalPan(long argWRTNO, string argCHK1, string argGbJin)
        {
            long nWRTNO = 0;
            string strTemp = "";
            string strCHK1 = "";
            string strGbJin = "";

            nWRTNO = argWRTNO;
            strCHK1 = argCHK1;
            strGbJin = argGbJin;

            if (VB.Left(clsHcType.B3.NEW_SICK03, 1) == "2")
            {
                FstrREC.Append("" + (char)34 + "ITEM116" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM116" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            strTemp = "";
            //위암-위종합기타
            if (!VB.Mid(clsHcType.B3.Stomach_S, 1, 1).IsNullOrEmpty())
            {
                //위조영종합판정
                if (!clsHcType.B3.S_PANJENG.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM117" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.S_PANJENG + VB.Space(1), 1) + (char)34 + ","); //위암-위암종합판정
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM117" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                if (!clsHcType.B3.S_JILETC.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM118" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.S_JILETC, 20) + VB.Space(20), 20) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM118" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                }

                strTemp = clsHcType.B3.S_SOGEN.Replace("\r\n", " ") + " " + clsHcType.B3.S_SOGEN2.Replace("\r\n", " ");
                //위암-위암권고
                FstrREC.Append("" + (char)34 + "ITEM119" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(strTemp, 600) + VB.Space(600), 600) + (char)34 + ",");

                //위내시경
                FstrREC.Append("" + (char)34 + "ITEM120" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM121" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM122" + (char)34 + ":" + (char)34 + VB.Space(60) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM117" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM118" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM119" + (char)34 + ":" + (char)34 + VB.Space(60) + (char)34 + ",");

                //위조영종합판정
                if (!clsHcType.B3.S_PANJENG.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM120" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.S_PANJENG + VB.Space(1), 1) + (char)34 + ",");             //위암-위암종합판정
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM120" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                if (!clsHcType.B3.S_JILETC.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM121" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.S_JILETC, 20) + VB.Space(20), 20) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM121" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                }

                strTemp = clsHcType.B3.S_SOGEN.Replace("\r\n", " ") + " " + clsHcType.B3.S_SOGEN2.Replace("\r\n", " ");
                //위암-위암권고
                FstrREC.Append("" + (char)34 + "ITEM122" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(strTemp, 600) + VB.Space(600), 600) + (char)34 + ",");
            }

            string[] strExCode = { "TX22" };

            if (hicResultService.GetCountbyWrtNo(nWRTNO, strExCode) > 0)
            {
                //위조영
                if (!clsHcType.B3.S_PANJENGDATE.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM123" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.S_PANJENGDATE.Replace("-", "") + VB.Space(8), 8) + (char)34 + ",");                    //위암-판정일자
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM123" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
                }
                if (clsHcType.B3.GBSTOMACH == "1")
                {
                    if (!clsHcType.B3.PanDrNo_New1.IsNullOrEmpty())
                    {
                        FstrREC.Append("" + (char)34 + "ITEM124" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PanDrNo_New1 + VB.Space(10), 10) + (char)34 + ",");                            //위암-판정면허번호 10
                        FstrREC.Append("" + (char)34 + "ITEM125" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(clsHcType.B3.PanDrNo_New1) + VB.Space(12), 12) + (char)34 + ",");       //위암의사명 12
                        FstrREC.Append("" + (char)34 + "ITEM126" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN(clsHcType.B3.PanDrNo_New1) + VB.Space(13), 13) + (char)34 + ",");      //위암의사주민번호 13
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM124" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM125" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM126" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM124" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM125" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM126" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM123" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM124" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM125" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM126" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
            }

            //'위내시경
            if (strCHK1 == "OK")
            {
                if (!clsHcType.B3.S_PANJENGDATE.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM127" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.S_PANJENGDATE.Replace("-", "") + VB.Space(8), 8) + (char)34 + ",");                     //위암-판정일자
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM127" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
                }
                if (clsHcType.B3.GBSTOMACH == "1")
                {
                    if (!clsHcType.B3.PanDrNo_New1.IsNullOrEmpty())
                    {
                        FstrREC.Append("" + (char)34 + "ITEM128" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PanDrNo_New1 + VB.Space(10), 10) + (char)34 + ",");                  //위암-판정면허번호 10
                        FstrREC.Append("" + (char)34 + "ITEM129" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(clsHcType.B3.PanDrNo_New1) + VB.Space(12), 12) + (char)34 + ",");           //위암의사명 12
                        FstrREC.Append("" + (char)34 + "ITEM130" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN(clsHcType.B3.PanDrNo_New1) + VB.Space(13), 13) + (char)34 + ",");          //위암의사주민번호 13
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM128" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM129" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM130" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM128" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM129" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM130" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM127" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM128" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM129" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM130" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
            }

            //위암-진찰료여부
            if (strGbJin == "1")
            {
                FstrREC.Append("" + (char)34 + "ITEM131" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM131" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            if (clsHcType.B3.GBSTOMACH == "1")
            {
                if (!clsHcType.B3.PANJENGDRNO1.IsNullOrEmpty() && string.Compare(clsHcType.B3.PANJENGDRNO1, "0") > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM132" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PANJENGDRNO1 + VB.Space(10), 10) + (char)34 + ",");                            //위암-판독의면허번호 10
                    FstrREC.Append("" + (char)34 + "ITEM133" + (char)34 + ":" + (char)34 + VB.Left(hb.READ_BILL_DRCODE(clsHcType.B3.PANJENGDRNO1) + VB.Space(12), 12) + (char)34 + ",");          //위암판독 의사명 12
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM132" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM133" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM132" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM133" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
            }

            if (clsHcType.B3.GBSTOMACH == "1")
            {
                if (!clsHcType.B3.PANJENGDRNO2.IsNullOrEmpty() && string.Compare(clsHcType.B3.PANJENGDRNO2, "0") > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM134" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PANJENGDRNO2 + VB.Space(10), 10) + (char)34 + ","); //위암-검사의면허번호 10
                    FstrREC.Append("" + (char)34 + "ITEM135" + (char)34 + ":" + (char)34 + VB.Left(hb.READ_BILL_DRCODE(clsHcType.B3.PANJENGDRNO2) + VB.Space(12), 12) + (char)34 + ","); //위암검사 의사명 12
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM134" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM135" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM134" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM135" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
            }

            if (clsHcType.B3.GBSTOMACH == "1")
            {
                if (clsHcType.B3.PANJENGDRNO3.IsNullOrEmpty() && string.Compare(clsHcType.B3.PANJENGDRNO3, "0") > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM136" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.PANJENGDRNO3 + VB.Space(10), 10) + (char)34 + ","); //위암-병리진단의면허번호 10
                    FstrREC.Append("" + (char)34 + "ITEM137" + (char)34 + ":" + (char)34 + VB.Left(cf.Read_Bcode_Name(clsDB.DbCon, "HIC_암판정_의사매칭", clsHcType.B3.PANJENGDRNO3) + VB.Space(12), 12) + (char)34 + ","); //위암병리진단 의사명 12
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM136" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM137" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM136" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM137" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
            }

            FstrREC.Append("" + (char)34 + "ITEM138" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");             //장애인 안전편의 관리비(위조영)
            FstrREC.Append("" + (char)34 + "ITEM139" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");             //장애인 안전편의 관리비(위내시경)
        }

        /// <summary>
        /// 암검진 문진
        /// </summary>
        /// <param name="argMirno"></param>
        void fn_Cancer_Mir_Munjin_File(long argMirno, StreamWriter sw)
        {
            int nRead1 = 0;
            string strGkiho = "";
            string strSex = "";
            string strChk = "";
            string sJepDate = "";
            string strJong = "";

            sJepDate = cboYear.Text + "-01-01";
            strJong = VB.Left(cboJong.Text, 1);


            //암검사 문진
            List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetItembyJepDateMirNoJong(sJepDate, strJong, argMirno);

            nRead1 = list.Count;
            for (int i = 0; i < nRead1; i++)
            {
                chb.READ_HIC_CANCER_NEW(list[i].WRTNO); //접수번호에 대한 암검사 종류 읽음

                strSex = list[i].SEX;
                strGkiho = list[i].GKIHO.Replace("-", "");

                FstrREC.Append("{" + "" + (char)34 + "ITEM0" + (char)34 + ":" + (char)34 + "4" + (char)34 + ",");                                                               //청구파일 구분
                FstrREC.Append("" + (char)34 + "ITEM1" + (char)34 + ":" + (char)34 + VB.Left(cboYear.Text + VB.Space(4), 4) + (char)34 + ",");                                  //사업년도
                FstrREC.Append("" + (char)34 + "ITEM2" + (char)34 + ":" + (char)34 + VB.Left(clsAES.DeAES(list[i].JUMIN2) + VB.Space(13), 13) + (char)34 + ",");//주민번호
                FstrREC.Append("" + (char)34 + "ITEM3" + (char)34 + ":" + (char)34 + VB.Left(strGkiho + VB.Space(11), 11) + (char)34 + ",");                                      //증번호

                if (VB.L(list[i].TEL, "-") - 1 == 2)
                {
                    FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list[i].TEL, "-", 1) + VB.Space(4), 4) + (char)34 + ","); //전화 지역


                    FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list[i].TEL, "-", 2) + VB.Space(4), 4) + (char)34 + ","); //전화 국번


                    FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list[i].TEL, "-", 3) + VB.Space(4), 4) + (char)34 + ","); //전화 번호
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                }


                if (VB.L(list[i].HPHONE, "-") - 1 == 2)
                {
                    FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list[i].HPHONE, "-", 1) + VB.Space(4), 4) + (char)34 + ","); //핸드폰 지역
                    FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list[i].HPHONE, "-", 2) + VB.Space(4), 4) + (char)34 + ","); //핸드폰 국번
                    FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list[i].HPHONE, "-", 3) + VB.Space(4), 4) + (char)34 + ","); //핸드폰 번호
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                }

                if (!list[i].EMAIL.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(list[i].EMAIL, 40) + VB.Space(40), 40) + (char)34 + ",");        //전자우편주소
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + VB.Space(40) + (char)34 + ",");
                }
                FstrREC.Append("" + (char)34 + "ITEM11" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");          //통보서 수신방법(1:우편, 2:이메일)

                //Call 암기본문진
                fn_CancerBasicMun();
                //Call 암과거병력
                fn_CancerMedicalHis();
                //Call 암검사경험
                fn_CancerExamExperience();
                //Call 암질환유무
                fn_CancerDieaseYN();
                //Call 암여성문항
                fn_CancerWomenMun();

                //2020 신규자료
                FstrREC.Append("" + (char)34 + "ITEM102" + (char)34 + ":" + (char)34 + VB.Left(list[i].MAILCODE + VB.Space(5), 5) + (char)34 + ",");            //우편번호
                FstrREC.Append("" + (char)34 + "ITEM103" + (char)34 + ":" + (char)34 + VB.Left(list[i].JUSO1 + VB.Space(200), 200) + (char)34 + ",");           //주소
                FstrREC.Append("" + (char)34 + "ITEM104" + (char)34 + ":" + (char)34 + VB.Left(list[i].JUSO2 + VB.Space(200), 200) + (char)34 + ",");           //상세주소


                FstrREC.Append("" + (char)34 + "ITEM105" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");                                           //SPACE
                if (i == nRead1 - 1)
                {
                    FstrREC.Append("" + (char)34 + "ITEM106" + (char)34 + ":" + (char)34 + "E" + (char)34 + "}");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM106" + (char)34 + ":" + (char)34 + "E" + (char)34 + "},");
                }

                sw.WriteLine(FstrREC);
                FstrREC.Clear();
            }
            FstrREC.Append("]");
            sw.Close();
        }

        /// <summary>
        /// 암기본문진()
        /// </summary>
        void fn_CancerBasicMun()
        {
            //현재불편증상 유무
            if (!clsHcType.B3.NEW_SICK19.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM12" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_SICK19 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM12" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //현재불편증상내역
            if (!clsHcType.B3.NEW_SICK20.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.NEW_SICK20, 40) + VB.Space(40), 40) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + VB.Space(40) + (char)34 + ",");
            }


            if (clsHcType.B3.Weight > 0)
            {
                FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + "2" + (char)34 + ","); //체중감소 2 있다.
                FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:###}", clsHcType.B3.Weight) + VB.Space(3), 3) + (char)34 + ","); //체중
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 암과거병력()
        /// </summary>
        void fn_CancerMedicalHis()
        {
            if (!clsHcType.B3.NEW_SICK01.IsNullOrEmpty())
            {
                //과거병력-위암
                FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_SICK01 + VB.Space(1), 1) + (char)34 + ",");
                if (VB.Left(clsHcType.B3.NEW_SICK01 + VB.Space(1), 1) == "2")
                {
                    FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK03, 1, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK03, 2, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK03, 3, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK03, 4, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK03, 5, 1) + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            if (!clsHcType.B3.NEW_SICK06.IsNullOrEmpty())
            {
                //과거병력-유방암
                FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_SICK06 + VB.Space(1), 1) + (char)34 + ",");

                if (VB.Left(clsHcType.B3.NEW_SICK06 + VB.Space(1), 1) == "2")
                {
                    FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK08, 1, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK08, 2, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK08, 3, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK08, 4, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK08, 5, 1) + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {

                    FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            if (!clsHcType.B3.NEW_SICK11.IsNullOrEmpty())
            {
                //과거병력-대장암
                FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_SICK11 + VB.Space(1), 1) + (char)34 + ",");
                if (VB.Left(clsHcType.B3.NEW_SICK11 + VB.Space(1), 1) == "2")
                {
                    FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK13, 1, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM30" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK13, 2, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK13, 3, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM32" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK13, 4, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK13, 5, 1) + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM30" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM32" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM30" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM32" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            if (!clsHcType.B3.NEW_SICK16.IsNullOrEmpty())
            {
                //과거병력-자궁암
                FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_SICK16 + VB.Space(1), 1) + (char)34 + ",");

                if (VB.Left(clsHcType.B3.NEW_SICK16 + VB.Space(1), 1) == "2")
                {
                    FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK18, 1, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK18, 2, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM37" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK18, 3, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK18, 4, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM39" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK18, 5, 1) + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM37" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM39" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM37" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM39" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            if (!clsHcType.B3.NEW_SICK21.IsNullOrEmpty())
            {
                //과거병력-간암
                FstrREC.Append("" + (char)34 + "ITEM40" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_SICK21 + VB.Space(1), 1) + (char)34 + ",");
                if (VB.Left(clsHcType.B3.NEW_SICK21 + VB.Space(1), 1) == "2")
                {
                    FstrREC.Append("" + (char)34 + "ITEM41" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK23, 1, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM42" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK23, 2, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM43" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK23, 3, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM44" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK23, 4, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM45" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK23, 5, 1) + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM41" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM42" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM43" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM44" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM45" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM40" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM41" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM42" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM43" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM44" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM45" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!clsHcType.B3.NEW_SICK75.IsNullOrEmpty())
            {
                if (!clsHcType.B3.NEW_SICK75.IsNullOrEmpty())
                {
                    //과거병력-폐암
                    FstrREC.Append("" + (char)34 + "ITEM46" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_SICK75 + VB.Space(1), 1) + (char)34 + ",");
                    if (VB.Left(clsHcType.B3.NEW_SICK75 + VB.Space(1), 1) == "2")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM47" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK76, 1, 1) + VB.Space(1), 1) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM48" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK76, 2, 1) + VB.Space(1), 1) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM49" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK76, 3, 1) + VB.Space(1), 1) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM50" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK76, 4, 1) + VB.Space(1), 1) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM51" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK76, 5, 1) + VB.Space(1), 1) + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM47" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM48" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM49" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM50" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM51" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM46" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM47" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM48" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM49" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM50" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM51" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
            }

            if (!clsHcType.B3.NEW_SICK26.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM52" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(clsHcType.B3.NEW_SICK30, 20) + VB.Space(20), 20) + (char)34 + ",");   //과거병력-기타내역
                FstrREC.Append("" + (char)34 + "ITEM53" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_SICK26 + VB.Space(1), 1) + (char)34 + ",");                          //과거병력-기타

                if (VB.Left(clsHcType.B3.NEW_SICK26 + VB.Space(1), 1) == "2")
                {
                    FstrREC.Append("" + (char)34 + "ITEM54" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK28, 1, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM55" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK28, 2, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM56" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK28, 3, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM57" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK28, 4, 1) + VB.Space(1), 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM58" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK28, 5, 1) + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM54" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM55" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM56" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM57" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM58" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM52" + (char)34 + ":" + (char)34 + VB.Space(20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM53" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM54" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM55" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM56" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM57" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM58" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 암검사경험
        /// </summary>
        void fn_CancerExamExperience()
        {
            //검사경험-위암-위장조영검사
            if (!clsHcType.B3.NEW_CAN_WOMAN01.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM59" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_CAN_WOMAN01 + VB.Space(1), 1) + (char)34 + ",");
            }
            else {
                FstrREC.Append("" + (char)34 + "ITEM59" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //검사경험-위암-위내시경
            if (!clsHcType.B3.NEW_CAN_WOMAN04.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_CAN_WOMAN04 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //검사경험-유방암-유방촬영
            if (!clsHcType.B3.NEW_CAN_WOMAN16.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_CAN_WOMAN16 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //검사경험-대장암-분변잠혈
            if (clsHcType.B3.NEW_CAN_WOMAN06.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM62" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_CAN_WOMAN06 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM62" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }


            //검사경험-대장암-대장이중조영
            if (!clsHcType.B3.NEW_CAN_WOMAN09.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM63" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_CAN_WOMAN09 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM63" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }


            //검사경험-대장암-대장내시경
            if (clsHcType.B3.NEW_CAN_WOMAN11.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM64" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_CAN_WOMAN11 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM64" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }


            //검사경험-자궁암-자궁경부
            if (clsHcType.B3.NEW_CAN_WOMAN19.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM65" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_CAN_WOMAN19 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM65" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }


            //검사경험-폐암-흉뷰CT
            if (clsHcType.B3.NEW_SICK78.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM66" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_SICK78 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM66" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }


            //검사경험-간암-간초음파
            if (clsHcType.B3.NEW_CAN_WOMAN14.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_CAN_WOMAN14 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 암질환유무()
        /// </summary>
        void fn_CancerDieaseYN()
        {
            string strChk = "";

            strChk = "1";   //기본은 없음
            for (int k = 1; k <= 5; k++)
            {
                if (!VB.Mid(clsHcType.B3.NEW_SICK61, k, 1).IsNullOrEmpty())
                {
                    strChk = "2"; //2는 있음;                
                }
            }

            FstrREC.Append("" + (char)34 + "ITEM68" + (char)34 + ":" + (char)34 + strChk + (char)34 + ",");

            //위장질환 유무
            if (!VB.Mid(clsHcType.B3.NEW_SICK61, 1, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM69" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK61, 1, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM69" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK61, 2, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM70" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK61, 2, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM70" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK61, 3, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM71" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK61, 3, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM71" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK61, 4, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM72" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK61, 4, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM72" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK61, 5, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM73" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK61, 5, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM73" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //'현재,과거 항문질환 진단 2010
            strChk = "1"; //기본은 없음
            for (int k = 1; k <= 5; k++)
            {
                if (!VB.Mid(clsHcType.B3.NEW_SICK62, k, 1).IsNullOrEmpty())
                {
                    if (VB.Mid(clsHcType.B3.NEW_SICK62, k, 1) == "2")
                    {
                        strChk = "2"; //2는 있음
                    }
                }
            }
            FstrREC.Append("" + (char)34 + "ITEM74" + (char)34 + ":" + (char)34 + strChk + (char)34 + ",");

            //'대장항문질환 유무
            if (!VB.Mid(clsHcType.B3.NEW_SICK62, 1, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM75" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK62, 1, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM75" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK62, 2, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM76" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK62, 2, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM76" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK62, 3, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM77" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK62, 3, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM77" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK62, 4, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM78" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK62, 4, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM78" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK62, 5, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM79" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK62, 5, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM79" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //'현재,과거 간질환 진단 2010
            strChk = "1"; //기본은 없음
            for (int k = 0; k <= 5; k++)
            {
                if (!VB.Mid(clsHcType.B3.NEW_SICK25, k, 1).IsNullOrEmpty())
                {
                    if (VB.Mid(clsHcType.B3.NEW_SICK25, k, 1) == "2")
                    {
                        strChk = "2"; //2는 있음
                    }
                }
            }
            FstrREC.Append("" + (char)34 + "ITEM80" + (char)34 + ":" + (char)34 + strChk + (char)34 + ",");


            //'간장질환 유무
            if (!VB.Mid(clsHcType.B3.NEW_SICK25, 1, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM81" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK25, 1, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM81" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK25, 2, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM82" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK25, 2, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM82" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK25, 3, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM83" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK25, 3, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM83" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK25, 4, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM84" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK25, 4, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM84" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK25, 5, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM85" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK25, 5, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM85" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //'현재,과거 폐질환 진단 2010
            strChk = "1"; //기본은 없음
            for (int k = 1; k <= 6; k++)
            {
                if (!VB.Mid(clsHcType.B3.NEW_SICK77, k, 1).IsNullOrEmpty())
                {
                    if (VB.Mid(clsHcType.B3.NEW_SICK77, k, 1) == "2")
                    {
                        strChk = "2"; //2는 있음
                    }
                }
            }

            FstrREC.Append("" + (char)34 + "ITEM86" + (char)34 + ":" + (char)34 + strChk + (char)34 + ",");
            //폐질환 유무
            if (!VB.Mid(clsHcType.B3.NEW_SICK77, 1, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM87" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK77, 1, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM87" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK77, 2, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM88" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK77, 2, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM88" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK77, 3, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM89" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK77, 3, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM89" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK77, 4, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM90" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK77, 4, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM90" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK77, 5, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM91" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK77, 5, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM91" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
            if (!VB.Mid(clsHcType.B3.NEW_SICK77, 6, 1).IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM92" + (char)34 + ":" + (char)34 + VB.Left(VB.Mid(clsHcType.B3.NEW_SICK77, 6, 1) + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM92" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 암여성문항
        /// </summary>
        void fn_CancerWomenMun()
        {
            if (!clsHcType.B3.NEW_WOMAN02.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM93" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                                    //월경시작 만?세
                FstrREC.Append("" + (char)34 + "ITEM94" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", clsHcType.B3.NEW_WOMAN02) + VB.Space(3), 3) + (char)34 + ",");     //월경시작 나이
            }
            else
            {
                //월경시작 초경없음
                FstrREC.Append("" + (char)34 + "ITEM93" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM94" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
            }

            //월경상태
            if (!clsHcType.B3.NEW_WOMAN11.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM95" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_WOMAN11 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM95" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }


            //폐경연령
            if (!clsHcType.B3.NEW_WOMAN14.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM96" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", clsHcType.B3.NEW_WOMAN14) + VB.Space(3), 3) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM96" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
            }


            //호르몬여부
            if (!clsHcType.B3.NEW_WOMAN18.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM97" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_WOMAN18 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM97" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }


            //자녀출산
            if (!clsHcType.B3.NEW_WOMAN21.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM98" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_WOMAN21 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM98" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //모유기간
            if (!clsHcType.B3.NEW_WOMAN41.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM99" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_WOMAN41 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM99" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //유방양성종양
            if (!clsHcType.B3.NEW_WOMAN27.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM100" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_WOMAN27 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM100" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //피임여부
            if (!clsHcType.B3.NEW_WOMAN31.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM101" + (char)34 + ":" + (char)34 + VB.Left(clsHcType.B3.NEW_WOMAN31 + VB.Space(1), 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM101" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 구강문진()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="nWRTNO"></param>
        void fn_DentalMunjin(long argMirno, long argWRTNO)
        {
            string strRES_Munjin = "";
            string strRES_Result = "";
            string sJepDate = "";

            sJepDate = cboYear.Text + "-01-01";

            HIC_RES_DENTAL_JEPSU_PATEINT list = hicResDentalJepsuPatientService.GetItembyMirNoMirNo(argMirno, argWRTNO, sJepDate);

            if (!list.IsNullOrEmpty())
            {
                //1.년간 치과병원
                if (!list.OPDDNT.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM12" + (char)34 + ":" + (char)34 + VB.Left(list.OPDDNT + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM12" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                //2.현재 당뇨병
                if (!list.T_JILBYUNG1.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + VB.Left(list.T_JILBYUNG1 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");
                }

                //3.현재 심혈관
                if (!list.T_JILBYUNG2.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + VB.Left(list.T_JILBYUNG2 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");
                }

                //4.최근 3개월 음식을...
                if (!list.T_FUNCTION1.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + VB.Left(list.T_FUNCTION1 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                //5.치아가 쑤시거나..
                if (!list.T_STAT1.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + VB.Left(list.T_STAT1 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                //6.잇몸이 아프거나 피가..
                if (!list.T_STAT2.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + VB.Left(list.T_STAT2 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                //7.스스로 생각할때 구강관리..
                if (!list.DNTSTATUS.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + VB.Left(list.DNTSTATUS + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");
                }

                //8.치아 닦는법 배운적..
                if (!list.T_HABIT5.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + VB.Left(list.T_HABIT5 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                //9.양치횟수
                if (list.T_HABIT2 > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list.T_HABIT2, "00") + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + "77" + (char)34 + ",");
                }

                //10.잠자기전
                if (!list.T_HABIT6.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + VB.Left(list.T_HABIT6 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + "4" + (char)34 + ",");
                }

                //11.치실,치간솔?
                if (!list.T_HABIT4.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + VB.Left(list.T_HABIT4 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + "4" + (char)34 + ",");
                }

                //12.치약에 불소
                if (!list.T_HABIT7.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + VB.Left(list.T_HABIT7 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");
                }

                //13.달라붙는 간식
                if (list.T_HABIT8.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + VB.Left(list.T_HABIT8 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + "5" + (char)34 + ",");
                }

                //14.탄산음료
                if (!list.T_HABIT9.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + VB.Left(list.T_HABIT9 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + "5" + (char)34 + ",");
                }

                //15.담배
                if (!list.T_HABIT1.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + VB.Left(list.T_HABIT1 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");
                }
            }
        }

        /// <summary>
        /// 구강결과()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="nWRTNO"></param>
        /// <param name="strDANTAL1"></param>
        void fn_DentalResult(long argMirno, long argWRTNO, string argGubun)
        {
            string strRES_Munjin = "";
            string strRES_Result = "";
            string sJepDate = "";

            sJepDate = cboYear.Text + "-01-01";

            HIC_RES_DENTAL_JEPSU_PATEINT list = hicResDentalJepsuPatientService.GetItembyMirNoMirNo(argMirno, argWRTNO, sJepDate);

            if (!list.IsNullOrEmpty())
            {
                strRES_Munjin = list.RES_MUNJIN;
                strRES_Result = list.RES_RESULT;

                //문진표평가(2014)
                FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + VB.Mid(strRES_Munjin + "111111", 1, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + VB.Mid(strRES_Munjin + "111111", 2, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + VB.Mid(strRES_Munjin + "111111", 3, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + VB.Mid(strRES_Munjin + "111111", 4, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + VB.Mid(strRES_Munjin + "111111", 5, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + VB.Mid(strRES_Munjin + "111111", 6, 1) + (char)34 + ",");

                //치아검사
                FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + VB.Mid(strRES_Result + "111111", 1, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM11" + (char)34 + ":" + (char)34 + VB.Mid(strRES_Result + "111111", 2, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM12" + (char)34 + ":" + (char)34 + VB.Mid(strRES_Result + "111111", 3, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + VB.Mid(strRES_Result + "111111", 4, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + VB.Mid(strRES_Result + "111111", 5, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + VB.Mid(strRES_Result + "111111", 6, 1) + (char)34 + ",");

                //기타소견
                FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(list.T_PANJENG_ETC, 150) + VB.Space(150), 150) + (char)34 + ",");

                //치균세막검사 만40세
                //치균세막검사 대상확인;

                if (argGubun == "Y")
                {
                    if (string.Format("{0:00}", list.T40_PAN1_NEW) == "00")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + "77" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list.T40_PAN1_NEW) + (char)34 + ",");
                    }

                    if (string.Format("{0:00}", list.T40_PAN2_NEW) == "00")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + "77" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list.T40_PAN2_NEW) + (char)34 + ",");
                    }

                    if (string.Format("{0:00}", list.T40_PAN3_NEW) == "00")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + "77" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list.T40_PAN3_NEW) + (char)34 + ",");
                    }

                    if (string.Format("{0:00}", list.T40_PAN4_NEW) == "00")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + "77" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list.T40_PAN4_NEW) + (char)34 + ",");
                    }

                    if (string.Format("{0:00}", list.T40_PAN5_NEW) == "00")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + "77" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list.T40_PAN5_NEW) + (char)34 + ",");
                    }

                    if (string.Format("{0:00}", list.T40_PAN6_NEW) == "00")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + "77" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list.T40_PAN6_NEW) + (char)34 + ",");
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + "00" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + "00" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + "00" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + "00" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + "00" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + "00" + (char)34 + ",");
                }

                //종합판정
                FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + VB.Left(list.T_PANJENG1 + VB.Space(1), 1) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 생활습관금연처방()
        /// </summary>
        /// <param name="argSLIP1"></param>
        /// <param name="argLife1"></param>
        void fn_LifeStype_Smoking_Order(string argSLIP1, string argLife1)
        {
            string strSLIP1 = "";
            string strLife1 = "";
            strSLIP1 = argSLIP1;
            strLife1 = argLife1;

            //'금연처방여부(0:미선택, 1:선택)
            if (argSLIP1.IsNullOrEmpty() || VB.Pstr(argSLIP1, ";", 1) == "0" || argLife1.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM107" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM108" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM109" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM110" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM111" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM112" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM113" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM114" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM115" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM116" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM117" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM118" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM119" + (char)34 + ":" + (char)34 + VB.Space(200) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM120" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM121" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM122" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM123" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM124" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM125" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM126" + (char)34 + ":" + (char)34 + VB.Space(200) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM127" + (char)34 + ":" + (char)34 + VB.Space(200) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM107" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                //금연-흡연상태,니코틴의존도,금연계획단계,금연처방

                if (VB.Pstr(strSLIP1, ";", 1).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM108" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM108" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 1) + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP1, ";", 2).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM109" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM109" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 2) + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP1, ";", 3).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM110" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM110" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 3) + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP1, ";", 4).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM111" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM111" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 4) + (char)34 + ",");
                }

                //금연-약물처방체크
                if (VB.Pstr(argSLIP1, ";", 5) == "1" || VB.Pstr(strSLIP1, ";", 6) == "1" || VB.Pstr(argSLIP1, ";", 7) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM112" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM112" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                //금연-금연처방 및 약물처방
                FstrREC.Append("" + (char)34 + "ITEM113" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 5) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM114" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM115" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 7) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM116" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 8) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM117" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 9) + (char)34 + ",");

                //2019-09-23
                //금연-금연처방 기타 체크
                if (VB.Pstr(strSLIP1, ";", 10).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM118" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM118" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }

                //금연-금연처방 기타내용
                FstrREC.Append("" + (char)34 + "ITEM119" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP1, ";", 10) + VB.Space(200), 200) + (char)34 + ",");

                //금연-금단증상 및 흡연충동 극복하기
                if (VB.Pstr(strSLIP1, ";", 11).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM120" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM120" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 11) + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP1, ";", 12).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM121" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM121" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 12) + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP1, ";", 13).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM122" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM122" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 13) + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP1, ";", 14).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM123" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM123" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 14) + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP1, ";", 15).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM124" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM124" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 15) + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP1, ";", 16).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM125" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM125" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 16) + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM126" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP1, ";", 18) + VB.Space(200), 200) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM127" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP1, ";", 17) + VB.Space(200), 200) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 생활습관음주처방()
        /// </summary>
        /// <param name="argSLIP2"></param>
        /// <param name="argLife2"></param>
        /// <param name="argDrink"></param>
        void fn_LifeStype_Drinking_Order(string argSLIP2, string argLife2, string argDrink)
        {
            string strSLIP2 = "";
            string strLife2 = "";
            string strDrink = "";
            string strTemp = "";


            strSLIP2 = argSLIP2;
            strLife2 = argLife2;
            strDrink = argDrink;

            //'음주/절주 처방여부(0:미선택, 1:선택)
            if (strSLIP2.IsNullOrEmpty() || strLife2.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM128" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM129" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM130" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM131" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM132" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM133" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM134" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM135" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM136" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM137" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM138" + (char)34 + ":" + (char)34 + VB.Space(200) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM139" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM140" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM141" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM142" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM143" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM144" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM145" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM146" + (char)34 + ":" + (char)34 + VB.Space(200) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM128" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                //생활습관음주점수
                FstrREC.Append("" + (char)34 + "ITEM129" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", strDrink) + VB.Space(3), 3) + (char)34 + ",");


                FstrREC.Append("" + (char)34 + "ITEM130" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP2, ";", 2) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM131" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP2, ";", 3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM132" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP2, ";", 4) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM133" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP2, ";", 5) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM134" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP2, ";", 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM135" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP2, ";", 7) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM136" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP2, ";", 8) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM137" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP2, ";", 9) + (char)34 + ",");

                //음주- 질환기타내용
                FstrREC.Append("" + (char)34 + "ITEM138" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP2, ";", 18) + VB.Space(200), 200) + (char)34 + ",");
                strTemp = (VB.Pstr(strSLIP2, ";", 10).To<int>() - 1).To<string>();
                FstrREC.Append("" + (char)34 + "ITEM139" + (char)34 + ":" + (char)34 + strTemp + (char)34 + ",");

                FstrREC.Append("" + (char)34 + "ITEM140" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP2, ";", 11) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM141" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP2, ";", 12) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM142" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP2, ";", 13) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM143" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP2, ";", 14) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM144" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP2, ";", 15) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM145" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP2, ";", 16) + (char)34 + ",");

                //음주-기타의견
                FstrREC.Append("" + (char)34 + "ITEM146" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP2, ";", 17) + VB.Space(200), 200) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 생활습관운동처방()
        /// </summary>
        /// <param name="argSLIP3"></param>
        void fn_LifeStype_Exercise_Order(string argSLIP3)
        {
            string strSLIP3 = "";

            strSLIP3 = argSLIP3;

            //'운동처방여부(0:미선택, 1:선택)
            if (strSLIP3.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM147" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM148" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM149" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM150" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM151" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM152" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM153" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM154" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM155" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM156" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM157" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");

                FstrREC.Append("" + (char)34 + "ITEM158" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM159" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM160" + (char)34 + ":" + (char)34 + VB.Space(200) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM161" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM162" + (char)34 + ":" + (char)34 + VB.Space(40) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM163" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM164" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM165" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM166" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM167" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");

                FstrREC.Append("" + (char)34 + "ITEM168" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM169" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM170" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM171" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM172" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM173" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM174" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM175" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM176" + (char)34 + ":" + (char)34 + VB.Space(200) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM177" + (char)34 + ":" + (char)34 + VB.Space(200) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM147" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM148" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 1) + (char)34 + ",");

                FstrREC.Append("" + (char)34 + "ITEM149" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 2) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM150" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM151" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 4) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM152" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 5) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM153" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM154" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 7) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM155" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 8) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM156" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 9) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM157" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM158" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 11) + (char)34 + ",");

                if (VB.Pstr(strSLIP3, ";", 12).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM159" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM159" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                FstrREC.Append("" + (char)34 + "ITEM160" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP3, ";", 12) + VB.Space(200), 200) + (char)34 + ",");

                //LINE161
                if (VB.Pstr(strSLIP3, ";", 13).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM161" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM161" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 13) + (char)34 + ",");
                }
                FstrREC.Append("" + (char)34 + "ITEM162" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP3, ";", 14) + VB.Space(40), 40) + (char)34 + ",");

                if (VB.Pstr(strSLIP3, ";", 15).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM163" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM163" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 15) + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM164" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 16) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM165" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 17) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM166" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 18) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM167" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 19) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM168" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM169" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 21) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM170" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 22) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM171" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 23) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM172" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 24) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM173" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 25) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM174" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP3, ";", 26) + (char)34 + ",");

                if (VB.Pstr(strSLIP3, ";", 27).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM175" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM175" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                FstrREC.Append("" + (char)34 + "ITEM176" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP3, ";", 28) + VB.Space(200), 200) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM177" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP3, ";", 29) + VB.Space(200), 200) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 생활습관영양처방()
        /// </summary>
        /// <param name="argSLIP4"></param>
        void fn_LifeStype_Diet_Order(string argSLIP4)
        {
            string strSLIP4 = "";
            strSLIP4 = argSLIP4;

            //'영양처방여부(0:미선택, 1:선택)
            if (strSLIP4.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM178" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM179" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM180" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM181" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM182" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM183" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM184" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM185" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM186" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM187" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM188" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM189" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM190" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM191" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM192" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM193" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM194" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM195" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM196" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM197" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM198" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM199" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM200" + (char)34 + ":" + (char)34 + VB.Space(200) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM201" + (char)34 + ":" + (char)34 + VB.Space(200) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM178" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM179" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM180" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 2) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM181" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM182" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 4) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM183" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 5) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM184" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM185" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 7) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM186" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 8) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM187" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 9) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM188" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM189" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 11) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM190" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 12) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM191" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 13) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM192" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 14) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM193" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 15) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM194" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 16) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM195" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 17) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM196" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 18) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM197" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 19) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM198" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP4, ";", 20) + (char)34 + ",");

                if (VB.Pstr(strSLIP4, ";", 21).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM199" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM199" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM200" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP4, ";", 21) + VB.Space(200), 200) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM201" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP4, ";", 22) + VB.Space(200), 200) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 생활습관비만처방()
        /// </summary>
        /// <param name="argSLIP5"></param>
        void fn_LifeStype_Biman_Order(string argSLIP5)
        {
            string strSLIP5 = "";

            strSLIP5 = argSLIP5;

            //'비만처방여부(0:미선택, 1:선택)
            if (strSLIP5.IsNullOrEmpty())
            {
                FstrREC.Append("" + (char)34 + "ITEM202" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");

                FstrREC.Append("" + (char)34 + "ITEM203" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM204" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM205" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM206" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM207" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM208" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM209" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM210" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM211" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM212" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM213" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM214" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM215" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM216" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM217" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM218" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM219" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM220" + (char)34 + ":" + (char)34 + VB.Space(200) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM221" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM222" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM223" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM224" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM225" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM226" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM227" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM228" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM229" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM230" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM231" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM232" + (char)34 + ":" + (char)34 + VB.Space(200) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM233" + (char)34 + ":" + (char)34 + VB.Space(200) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM202" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");

                if (VB.Pstr(strSLIP5, ";", 1).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM203" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM203" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 1) + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP5, ";", 2).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM204" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM204" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 2) + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP5, ";", 3).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM205" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM205" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 3) + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP5, ";", 4).IsNullOrEmpty() && VB.Pstr(strSLIP5, ";", 5).IsNullOrEmpty() || VB.Pstr(strSLIP5, ";", 6).IsNullOrEmpty() || VB.Pstr(strSLIP5, ";", 7).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM206" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM206" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM207" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP5, ";", 4) + VB.Space(3), 3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM208" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP5, ";", 5) + VB.Space(3), 3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM209" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP5, ";", 6) + VB.Space(2), 2) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM210" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP5, ";", 7) + VB.Space(3), 3) + (char)34 + ",");

                FstrREC.Append("" + (char)34 + "ITEM211" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 8) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM212" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 9) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM213" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 10) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM214" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 11) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM215" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 12) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM216" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 13) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM217" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 14) + (char)34 + ",");

                if (VB.Pstr(strSLIP5, ";", 15).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM218" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM218" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP5, ";", 16).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM219" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM219" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM220" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP5, ";", 16) + VB.Space(200), 200) + (char)34 + ",");


                FstrREC.Append("" + (char)34 + "ITEM221" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 17) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM222" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 18) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM223" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 19) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM224" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 20) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM225" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 21) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM226" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 22) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM227" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 23) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM228" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 24) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM229" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 25) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM230" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP5, ";", 26) + (char)34 + ",");

                if (VB.Pstr(strSLIP5, ";", 27).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM231" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM231" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM232" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP5, ";", 27) + VB.Space(200), 200) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM233" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(strSLIP5, ";", 28) + VB.Space(200), 200) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 생활습관흡연()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        /// <param name="argSLIP1"></param>
        /// <param name="argLife1"></param>
        void fn_LifeStype_Smoking(long argMirno, long argWRTNO, string argSLIP1, string argLife1)
        {
            string strSLIP1 = "";
            string strLife1 = "";
            string strTemp = "";

            strSLIP1 = argSLIP1;
            strLife1 = argLife1;

            //'흡연
            List<HIC_TITEM> list = hicTitemService.GetItembyWrtNoGubun(argWRTNO, "11");

            if (list.Count == 8)
            {
                FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + VB.Right(list[0].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + VB.Right(list[1].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + VB.Right(list[2].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + VB.Right(list[3].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + VB.Right(list[4].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + VB.Right(list[5].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + VB.Right(list[6].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM11" + (char)34 + ":" + (char)34 + VB.Right(list[7].CODE, 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM11" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //흡연
            if (!strSLIP1.IsNullOrEmpty() && strLife1 == "OK")
            {
                strTemp = VB.Pstr(strSLIP1, ";", 1) + 1;
                FstrREC.Append("" + (char)34 + "ITEM12" + (char)34 + ":" + (char)34 + strTemp + (char)34 + ",");
                if (VB.Pstr(strSLIP1, ";", 2).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP1, ";", 2) + (char)34 + ",");
                }

                //금연 처방
                if (VB.Pstr(strSLIP1, ";", 8) == "1" || VB.Pstr(strSLIP1, ";", 9) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");
                }
                else if (VB.Pstr(strSLIP1, ";", 5) == "1" || VB.Pstr(strSLIP1, ";", 6) == "1" || VB.Pstr(strSLIP1, ";", 7) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
                else if (VB.Pstr(strSLIP1, ";", 4) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM12" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 생활습관음주()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        /// <param name="argSLIP1"></param>
        /// <param name="argLife1"></param>
        void fn_LifeStype_Drinking(long argMirno, long argWRTNO, string argSLIP2, string argDrink1, string argLife2, string argDrink)
        {
            string strSLIP2 = "";
            string strDrink1 = "";
            string strLife2 = "";
            string strDrink = "";
            string strTemp = "";

            strSLIP2 = argSLIP2;
            strDrink1 = argDrink1;
            strLife2 = argLife2;
            strDrink = argDrink;

            //'음주
            List<HIC_TITEM> list = hicTitemService.GetItembyWrtNoGubun(argWRTNO, "12");

            if (list.Count == 10)
            {
                FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + VB.Right(list[0].CODE, 0) + (char)34 + ",");

                if (VB.Right(list[1].CODE, 1).To<int>() < 6)
                {
                    FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + VB.Right(list[1].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                else if (VB.Right(list[1].CODE, 1).To<int>() > 5)
                {
                    FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    strTemp = (VB.Right(list[1].CODE, 1).To<int>() - 5).To<string>();
                    FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + strTemp + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + VB.Right(list[2].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + VB.Right(list[3].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + VB.Right(list[4].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + VB.Right(list[5].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + VB.Right(list[6].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + VB.Right(list[7].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + VB.Right(list[8].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + VB.Right(list[9].CODE, 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            if ((strDrink1 == "1" || strDrink1 == "2" || strDrink1 == "3") && strLife2 == "OK")
            {
                if (!strSLIP2.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + VB.Pstr(strSLIP2, ";", 10) + (char)34 + ",");
                    if (VB.Pstr(strSLIP2, ";", 16) == "1")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");
                    }
                    else if (VB.Pstr(strSLIP2, ";", 15) == "1")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                    }
                    else if (VB.Pstr(strSLIP2, ";", 11) == "1" || VB.Pstr(strSLIP2, ";", 12) == "1" || VB.Pstr(strSLIP2, ";", 13) == "1" || VB.Pstr(strSLIP2, ";", 14) == "1")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    }
                    FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", strDrink) + VB.Space(3), 3) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 생활습관운동()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        /// <param name="argSLIP3"></param>
        void fn_LifeStype_Exercise(long argMirno, long argWRTNO, string argSLIP3)
        {
            int nTime1 = 0;
            int nTime2 = 0;
            string strHealth = "";
            string strSLIP3 = "";
            string strDAT = "";
            string strOK = "";

            strSLIP3 = argSLIP3;

            //'3.운동
            List<HIC_TITEM> list = hicTitemService.GetItembyWrtNoGubunJumsu(argWRTNO);

            if (list.Count == 17)
            {
                if (list[0].CODE.To<int>() > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM30" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM30" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                if (list[0].CODE == "-" || list[0].CODE.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + list[0].CODE + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM32" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[1].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[2].CODE) + (char)34 + ",");

                if (list[3].CODE.To<int>() > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                if (list[3].CODE == "-" || list[3].CODE.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + list[3].CODE + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[4].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM37" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[5].CODE) + (char)34 + ",");

                if (list[6].CODE.To<int>() > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                if (list[6].CODE == "-" || list[6].CODE.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM39" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM39" + (char)34 + ":" + (char)34 + list[6].CODE + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM40" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[7].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM41" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[8].CODE) + (char)34 + ",");

                if (list[9].CODE.To<int>() > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM42" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM42" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                if (list[9].CODE == "-" || list[9].CODE.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM43" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM43" + (char)34 + ":" + (char)34 + list[9].CODE + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM44" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[10].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM45" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[11].CODE) + (char)34 + ",");

                if (list[12].CODE.To<int>() > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM46" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM46" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                if (list[12].CODE == "-" || list[12].CODE.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM47" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM47" + (char)34 + ":" + (char)34 + list[12].CODE + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM48" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[13].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM49" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[14].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM50" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[15].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM51" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM30" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM32" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM37" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM39" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM40" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM41" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM42" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM43" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM44" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM45" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM46" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM47" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM48" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM49" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM50" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM51" + (char)34 + ":" + (char)34 + string.Format("{0:00}", list[16].CODE) + (char)34 + ",");

            }

            List<HIC_TITEM> list2 = hicTitemService.GetItembyWrtNoGubunJumsu2(argWRTNO, "13");

            if (list2.Count == 8)
            {
                FstrREC.Append("" + (char)34 + "ITEM52" + (char)34 + ":" + (char)34 + VB.Right(list[0].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM53" + (char)34 + ":" + (char)34 + VB.Right(list[1].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM54" + (char)34 + ":" + (char)34 + VB.Right(list[2].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM55" + (char)34 + ":" + (char)34 + VB.Right(list[3].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM56" + (char)34 + ":" + (char)34 + VB.Right(list[4].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM57" + (char)34 + ":" + (char)34 + VB.Right(list[5].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM58" + (char)34 + ":" + (char)34 + VB.Right(list[6].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM59" + (char)34 + ":" + (char)34 + VB.Right(list[7].CODE, 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM52" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM53" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM54" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM55" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM56" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM57" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM58" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM59" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            HIC_RES_BOHUM1 list3 = hicResBohum1Service.GetItemByWrtno(argWRTNO);

            //신체활동
            nTime1 = 0;
            nTime2 = 0;

            if (!list3.TMUN0011.IsNullOrEmpty())
            {
                nTime1 = VB.Pstr(list3.TMUN0011, ":", 1).To<int>() * 60;
                nTime1 += VB.Pstr(list3.TMUN0011, ":", 2).To<int>();
                nTime1 *= list3.T_ACTIVE1.To<int>() * 2;
            }

            if (list3.TMUN0012.IsNullOrEmpty())
            {
                nTime2 = VB.Pstr(list3.TMUN0012, ":", 1).To<int>() * 60;
                nTime2 += VB.Pstr(list3.TMUN0012, ":", 2).To<int>();
                nTime2 *= list3.T_ACTIVE2.To<int>();
            }

            strHealth = string.Format("{0:0}", nTime1 + nTime2);

            if (!strSLIP3.IsNullOrEmpty())
            {
                if (strHealth.To<int>() < 150)
                {
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + "01" + (char)34 + ",");
                }
                else if (strHealth.To<int>() >= 150 && strHealth.To<int>() < 300)
                {
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + "02" + (char)34 + ",");
                }
                else if (strHealth.To<int>() >= 300)
                {
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + "03" + (char)34 + ",");
                }
                else if (list3.T_ACTIVE3.To<int>() < 2)
                {
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + "04" + (char)34 + ",");
                }
                else if (list3.T_ACTIVE3.To<int>() >= 3)
                {
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + "05" + (char)34 + ",");
                }
                else if (strHealth.To<int>() < 150 && list3.T_ACTIVE3.To<int>() < 2)
                {
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + "06" + (char)34 + ",");
                }
                else if (strHealth.To<int>() < 150 && list3.T_ACTIVE3.To<int>() >= 3)
                {
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + "07" + (char)34 + ",");
                }
                else if (strHealth.To<int>() >= 150 && strHealth.To<int>() < 300 && list3.T_ACTIVE3.To<int>() < 2)
                {
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + "08" + (char)34 + ",");
                }
                else if (strHealth.To<int>() >= 150 && strHealth.To<int>() < 300 && list3.T_ACTIVE3.To<int>() >= 3)
                {
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + "09" + (char)34 + ",");
                }
                else if (strHealth.To<int>() >= 300 && list3.T_ACTIVE3.To<int>() < 2)
                {
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + "10" + (char)34 + ",");
                }
                else if (strHealth.To<int>() >= 300 && list3.T_ACTIVE3.To<int>() >= 3)
                {
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + "11" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                }

                //운동-처방(종류)
                strOK = "";
                for (int i = 2; i <= 11; i++)
                {
                    if (VB.Pstr(strSLIP3, ";", i) == "1" && i == 2)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                        strOK = "OK";
                        break;
                    }
                    if (VB.Pstr(strSLIP3, ";", i) == "1" && i == 5) { FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + "1" + (char)34 + ","); strOK = "OK"; break; }
                    if (VB.Pstr(strSLIP3, ";", i) == "1" && i == 4) { FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + "1" + (char)34 + ","); strOK = "OK"; break; }
                    if (VB.Pstr(strSLIP3, ";", i) == "1" && i == 8) { FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + "1" + (char)34 + ","); strOK = "OK"; break; }
                    if (VB.Pstr(strSLIP3, ";", i) == "1" && i == 10) { FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + "1" + (char)34 + ","); strOK = "OK"; break; }
                    if (VB.Pstr(strSLIP3, ";", i) == "1" && i == 11) { FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + "1" + (char)34 + ","); strOK = "OK"; break; }
                }

                //운동처-처방(시간)
                if (strOK == "")
                {
                    if (VB.Pstr(strSLIP3, ";", 12).IsNullOrEmpty())
                    {
                        FstrREC.Append("" + (char)34 + "ITEM62" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                }
                else
                {
                    strDAT = VB.Pstr(strSLIP3, ";", 13);
                    if (string.Compare(strDAT, "1") >= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM62" + (char)34 + ":" + (char)34 + strDAT + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM62" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    }
                }

                //운동-처방(빈도,횟수)
                strDAT = VB.Pstr(strSLIP3, ";", 15);
                if (string.Compare(strDAT, "1") >= 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM63" + (char)34 + ":" + (char)34 + strDAT + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM63" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                //운동-평가점수
                FstrREC.Append("" + (char)34 + "ITEM64" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", strHealth) + VB.Space(3), 3) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM62" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM63" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM64" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 생활습관영양()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        /// <param name="argSLIP4"></param>
        /// <param name="argDiet"></param>
        void fn_LifeStype_Diet(long argMirno, long argWRTNO, string argSLIP4, string argDiet)
        {
            string strSLIP4 = "";
            string strDiet = "";

            strSLIP4 = argSLIP4;
            strDiet = argDiet;

            List<HIC_TITEM> list = hicTitemService.GetItembyWrtNoGubun(argWRTNO, "14");

            if (list.Count == 11)
            {
                FstrREC.Append("" + (char)34 + "ITEM65" + (char)34 + ":" + (char)34 + VB.Right(list[0].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM66" + (char)34 + ":" + (char)34 + VB.Right(list[1].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + VB.Right(list[2].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM68" + (char)34 + ":" + (char)34 + VB.Right(list[3].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM69" + (char)34 + ":" + (char)34 + VB.Right(list[4].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM70" + (char)34 + ":" + (char)34 + VB.Right(list[5].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM71" + (char)34 + ":" + (char)34 + VB.Right(list[6].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM72" + (char)34 + ":" + (char)34 + VB.Right(list[7].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM73" + (char)34 + ":" + (char)34 + VB.Right(list[8].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM74" + (char)34 + ":" + (char)34 + VB.Right(list[9].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM75" + (char)34 + ":" + (char)34 + VB.Right(list[10].CODE, 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM65" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM66" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM68" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM69" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM70" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM71" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM72" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM73" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM74" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM75" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //4.영양
            //영양-평가
            if (!strSLIP4.IsNullOrEmpty())
            {
                if (strDiet.To<int>() <= 28)
                {
                    FstrREC.Append("" + (char)34 + "ITEM76" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else if (strDiet.To<int>() >= 28 && strDiet.To<int>() <= 38)
                {
                    FstrREC.Append("" + (char)34 + "ITEM76" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
                else if (strDiet.To<int>() >= 39)
                {
                    FstrREC.Append("" + (char)34 + "ITEM76" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");
                }

                //영양처방(권장음식)
                if (VB.Pstr(strSLIP4, ";", 2) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM77" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM77" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP4, ";", 3) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM78" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM78" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP4, ";", 4) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM79" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM79" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                //영양처방(제한음식)
                if (VB.Pstr(strSLIP4, ";", 5) == "1" || VB.Pstr(strSLIP4, ";", 6) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM80" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM80" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP4, ";", 7) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM81" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM81" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP4, ";", 8) == "1" || VB.Pstr(strSLIP4, ";", 11) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM82" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM82" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                //영양처방(식습관)
                if (VB.Pstr(strSLIP4, ";", 9) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM83" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM83" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (VB.Pstr(strSLIP4, ";", 10) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM84" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM84" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                //영양처방(연계)
                FstrREC.Append("" + (char)34 + "ITEM85" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");

                //영양-평가점수
                FstrREC.Append("" + (char)34 + "ITEM86" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", strDiet) + VB.Space(3), 3) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM76" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM77" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM78" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM79" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM80" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM81" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM82" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM83" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM84" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM85" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM86" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 생활습관비만()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        /// <param name="argSLIP5"></param>
        void fn_LifeStype_Biman(long argMirno, long argWRTNO, string argSLIP5)
        {
            string strSLIP5 = "";
            string strDAT = "";

            strSLIP5 = argSLIP5;

            List<HIC_TITEM> list = hicTitemService.GetItembyWrtNoGubunJumsu2(argWRTNO, "15");

            if (list.Count == 3)
            {
                FstrREC.Append("" + (char)34 + "ITEM87" + (char)34 + ":" + (char)34 + VB.Right(list[0].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM88" + (char)34 + ":" + (char)34 + VB.Right(list[1].CODE, 1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM89" + (char)34 + ":" + (char)34 + VB.Right(list[2].CODE, 1) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM87" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM88" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM89" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }

            //비만
            if (!strSLIP5.IsNullOrEmpty())
            {
                //비만(평가)
                if (!VB.Pstr(strSLIP5, ";", 1).IsNullOrEmpty())
                {
                    strDAT = VB.Pstr(strSLIP5, ";", 1);
                    FstrREC.Append("" + (char)34 + "ITEM90" + (char)34 + ":" + (char)34 + strDAT + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM90" + (char)34 + ":" + (char)34 + VB.Space(0) + (char)34 + ",");
                }

                //비만(처방)
                //7가지
                //식사량조절
                if (VB.Pstr(strSLIP5, ";", 8) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM91" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM91" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                //간식량조절
                if (VB.Pstr(strSLIP5, ";", 9) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM92" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM92" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                //음주량조절
                if (VB.Pstr(strSLIP5, ";", 12) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM93" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM93" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                //외식조절
                if (VB.Pstr(strSLIP5, ";", 10) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM94" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM94" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                //운동처방참고
                if (VB.Pstr(strSLIP5, ";", 13) == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM95" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM95" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                //연계클리닉
                if (!VB.Pstr(strSLIP5, ";", 15).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM96" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM96" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                //기타
                if (VB.Pstr(strSLIP5, ";", 16).IsNullOrEmpty() && VB.Pstr(strSLIP5, ";", 8) == "0" && VB.Pstr(strSLIP5, ";", 9) == "0" && VB.Pstr(strSLIP5, ";", 12) == "0" &&
                    VB.Pstr(strSLIP5, ";", 10) == "0" && VB.Pstr(strSLIP5, ";", 13) == "0" && VB.Pstr(strSLIP5, ";", 15) == "0")
                {
                    FstrREC.Append("" + (char)34 + "ITEM97" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else if (!VB.Pstr(strSLIP5, ";", 16).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM97" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM97" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM90" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM91" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM92" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM93" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM94" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM95" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM96" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM97" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 과거병력() - 과거병력,가족력
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        void fn_PastTroops(long argMirno, long argWRTNO)
        {
            string sJepDate = "";

            sJepDate = cboYear.Text + "-01-01";

            HIC_RES_BOHUM1_JEPSU_PATIENT list = hicResBohum1JepsuPatientRepository.GetItembyJepDateMirNoWrtNo(sJepDate, argMirno, argWRTNO);

            if (!list.IsNullOrEmpty())
            {
                if (list.T_STAT01 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM12" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                               //과거병력-진단여부-뇌졸중 1해당,2미해당
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM12" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_STAT11 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                               //과거병력-진단여부-심장병 1해당,2미해당
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_STAT21 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                                //과거병력-진단여부-고혈압
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_STAT31 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                             //과거병력-진단여부-당뇨병
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_STAT41 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                             //과거병력-진단여부-이상지질혈증
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_STAT61 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                                //과거병력-진단여부-폐결핵 2010
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_STAT51 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                                //과거병력-진단여부-기타
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_STAT02 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                                //과거병력-약물치료여부-뇌졸중 1해당,2미해당
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_STAT12 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                                //과거병력-약물치료여부-심장병
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_STAT22 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                                //과거병력-약물치료여부-고혈압
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_STAT32 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                               //과거병력-약물치료여부-당뇨병
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_STAT42 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                               //과거병력-약물치료여부-이상지질혈증
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_STAT62 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                                //과거병력-약물치료여부-폐결핵 2010
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_STAT52 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                               //과거병력-약물치료여부-기타
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_GAJOK1 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                               //가족력-뇌졸증
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_GAJOK2 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                               //가족력-심장병
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_GAJOK3 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                               //가족력-고혈압
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_GAJOK4 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                              //가족력-당뇨병
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.T_GAJOK5 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM30" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                               //가족력-고지혈증
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM30" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.T_BLIVER.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + VB.Left(list.T_BLIVER + VB.Space(1), 1) + (char)34 + ","); //B형간염(정밀) 항원보유자 (1.예, 2.아니오. 3 모름)
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
            }
        }

        /// <summary>
        /// 흡연() - 흡연,권련형전자담배,액상형전자담배
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        void fn_Smoking(long argMirno, long argWRTNO)
        {
            string sJepDate = "";

            sJepDate = cboYear.Text + "-01-01";

            HIC_RES_BOHUM1_JEPSU_PATIENT list = hicResBohum1JepsuPatientService.GetItembyJepDateMirNoWrtNo(sJepDate, argMirno, argWRTNO);

            if (!list.IsNullOrEmpty())
            {
                //2019변경사항
                if (list.TMUN0103 == "2")
                {
                    FstrREC.Append("" + (char)34 + "ITEM32" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");        //흡연4
                    if (list.T_SMOKE1 == "2")                                                     //흡연 4-1
                    {
                        FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:00}", list.T_SMOKE2) + VB.Space(2), 2) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", list.T_SMOKE3) + VB.Space(3), 3) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:00}", list.TMUN0096) + VB.Space(2), 2) + (char)34 + ",");

                    }
                    else if (list.T_SMOKE1 == "1")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:00}", list.T_SMOKE4) + VB.Space(2), 2) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", list.T_SMOKE5) + VB.Space(3), 3) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                    }
                    else
                    {
                        //공백1칸을 공단오류로 0값으로 설정(2020-03-02)
                        FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM32" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    //공백1칸을 공단오류로 0값으로 설정(2020-03-02)
                    FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                }

                if (list.TMUN0104 == "2")
                {
                    FstrREC.Append("" + (char)34 + "ITEM37" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                    if (list.TMUN0097 == "2")                                                   //흡연 5-1
                    {
                        FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM39" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:00}", list.TMUN0098) + VB.Space(2), 2) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM40" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", list.TMUN0099) + VB.Space(3), 3) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM41" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:00}", list.TMUN0102) + VB.Space(2), 2) + (char)34 + ",");
                    }
                    else if (list.TMUN0097 == "1")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM39" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:00}", list.TMUN0100) + VB.Space(2), 2) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM40" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", list.TMUN0101) + VB.Space(3), 3) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM41" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                    }
                    else
                    {
                        //공백1칸을 공단오류로 0값으로 설정(2020-03-02)
                        FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM39" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM40" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                        FstrREC.Append("" + (char)34 + "ITEM41" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM37" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    //공백1칸을 공단오류로 0값으로 설정(2020-03-02)
                    FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM39" + (char)34 + ":" + (char)34 + VB.Space(0) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM40" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM41" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                }

                //2020변경 (1.아니오, 2 예)
                if (!list.TMUN0001.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM42" + (char)34 + ":" + (char)34 + VB.Left(list.TMUN0001 + VB.Space(1), 1) + (char)34 + ",");  //전자담배 6-1
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM42" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                if (!list.TMUN0002.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM43" + (char)34 + ":" + (char)34 + VB.Left(list.TMUN0002 + VB.Space(1), 1) + (char)34 + ","); //전자담배 6-2
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM43" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
            }
        }

        /// <summary>
        /// 음주()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        void fn_Drinking(long argMirno, long argWRTNO)
        {
            string strData = "";
            int nITRVW1 = 0;
            int nITRVW2 = 0;
            int nITRVW3 = 0;
            int nITRVW4 = 0;
            int nITRVW5 = 0;
            int nITRVW6 = 0;
            int nITRVW7 = 0;
            int nITRVW8 = 0;
            int nITRVW9 = 0;
            int nITRVW10 = 0;
            int nITRVW11 = 0;
            int nITRVW12 = 0;
            int nITRVW13 = 0;
            int nITRVW14 = 0;
            int nITRVW15 = 0;
            int nITRVW16 = 0;
            int nITRVW17 = 0;
            int nITRVW18 = 0;
            int nITRVW19 = 0;
            int nITRVW20 = 0;
            int nITRVW21 = 0;
            int nITRVW22 = 0;
            int nITRVW23 = 0;
            int nITRVW24 = 0;
            int nITRVW25 = 0;
            int nITRVW26 = 0;
            int nITRVW27 = 0;
            int nITRVW28 = 0;
            int nITRVW29 = 0;
            int nITRVW30 = 0;
            int nITRVW31 = 0;
            int nITRVW32 = 0;
            int nITRVW33 = 0;
            int nITRVW34 = 0;
            int nITRVW35 = 0;
            int nITRVW36 = 0;
            int nITRVW37 = 0;
            int nITRVW38 = 0;
            int nITRVW39 = 0;
            int nITRVW40 = 0;

            string strGubun = "";
            string strGubun1 = "";

            string sJepDate = "";

            sJepDate = cboYear.Text + "-01-01";

            HIC_RES_BOHUM1_JEPSU_PATIENT list = hicResBohum1JepsuPatientRepository.GetItembyJepDateMirNoWrtNo(sJepDate, argMirno, argWRTNO);

            if (!list.IsNullOrEmpty())
            {
                //음주
                if (!list.TMUN0003.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM44" + (char)34 + ":" + (char)34 + VB.Left(list.TMUN0003 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM44" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
                if (!list.TMUN0004.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM45" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", list.TMUN0004) + VB.Space(3), 3) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM45" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                }

                nITRVW1 = 0; nITRVW2 = 0; nITRVW3 = 0; nITRVW4 = 0; nITRVW5 = 0; nITRVW6 = 0; nITRVW7 = 0; nITRVW8 = 0; nITRVW9 = 0; nITRVW10 = 0;
                nITRVW11 = 0; nITRVW12 = 0; nITRVW13 = 0; nITRVW14 = 0; nITRVW15 = 0; nITRVW16 = 0; nITRVW17 = 0; nITRVW18 = 0; nITRVW19 = 0; nITRVW20 = 0;
                nITRVW21 = 0; nITRVW22 = 0; nITRVW23 = 0; nITRVW24 = 0; nITRVW25 = 0; nITRVW26 = 0; nITRVW27 = 0; nITRVW28 = 0; nITRVW29 = 0; nITRVW30 = 0;
                nITRVW31 = 0; nITRVW32 = 0; nITRVW33 = 0; nITRVW34 = 0; nITRVW35 = 0; nITRVW36 = 0; nITRVW37 = 0; nITRVW38 = 0; nITRVW39 = 0; nITRVW40 = 0;

                //음주(strGubun : 1.소주, 2.맥주, 3.양주, 4.막걸리, 4.와인)
                for (int i = 1; i <= 3; i++)
                {
                    if (i == 1) { strData = list.TMUN0005; }
                    if (i == 2) { strData = list.TMUN0006; }
                    if (i == 3) { strData = list.TMUN0007; }

                    if (!strData.IsNullOrEmpty())
                    {
                        strGubun = VB.Left(strData, 1);
                        strGubun1 = VB.Right(strData, 1);
                        if (strGubun == "1")
                        {
                            if (strGubun1 == "잔") { nITRVW1 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "병") { nITRVW2 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "캔") { nITRVW3 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "C")  { nITRVW4 = VB.Pstr(strData, ";", 2).To<int>(); }
                        }

                        strGubun = VB.Left(strData, 1);
                        strGubun1 = VB.Right(strData, 1);
                        if (strGubun == "2")
                        {
                            if (strGubun1 == "잔") { nITRVW5 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "병") { nITRVW6 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "캔") { nITRVW7 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "C")  { nITRVW8 = VB.Pstr(strData, ";", 2).To<int>(); }
                        }

                        strGubun = VB.Left(strData, 1);
                        strGubun1 = VB.Right(strData, 1);
                        if (strGubun == "3")
                        {
                            if (strGubun1 == "잔") { nITRVW9 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "병") { nITRVW10 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "캔") { nITRVW11 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "C")  { nITRVW12 = VB.Pstr(strData, ";", 2).To<int>(); }
                        }

                        strGubun = VB.Left(strData, 1);
                        strGubun1 = VB.Right(strData, 1);
                        if (strGubun == "4")
                        {
                            if (strGubun1 == "잔") { nITRVW13 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "병") { nITRVW14 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "캔") { nITRVW15 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "C")  { nITRVW16 = VB.Pstr(strData, ";", 2).To<int>(); }
                        }

                        strGubun = VB.Left(strData, 1);
                        strGubun1 = VB.Right(strData, 1);
                        if (strGubun == "5")
                        {
                            if (strGubun1 == "잔") { nITRVW17 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "병") { nITRVW18 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "캔") { nITRVW19 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "C")  { nITRVW20 = VB.Pstr(strData, ";", 2).To<int>(); }
                        }
                    }
                }

                for (int i = 1; i <= 3; i++)
                {
                    if (i == 1) { strData = list.TMUN0008; }
                    if (i == 2) { strData = list.TMUN0009; }
                    if (i == 3) { strData = list.TMUN0010; }

                    if (!strData.IsNullOrEmpty())
                    {
                        strGubun = VB.Left(strData, 1);
                        strGubun1 = VB.Right(strData, 1);
                        if (strGubun == "1")
                        {
                            if (strGubun1 == "잔") { nITRVW21 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "병") { nITRVW22 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "캔") { nITRVW23 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "C")  { nITRVW24 = VB.Pstr(strData, ";", 2).To<int>(); }
                        }

                        strGubun = VB.Left(strData, 1);
                        strGubun1 = VB.Right(strData, 1);
                        if (strGubun == "2")
                        {
                            if (strGubun1 == "잔") { nITRVW25 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "병") { nITRVW26 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "캔") { nITRVW27 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "C")  { nITRVW28 = VB.Pstr(strData, ";", 2).To<int>(); }
                        }

                        strGubun = VB.Left(strData, 1);
                        strGubun1 = VB.Right(strData, 1);
                        if (strGubun == "3")
                        {
                            if (strGubun1 == "잔") { nITRVW29 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "병") { nITRVW30 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "캔") { nITRVW31 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "C")  { nITRVW32 = VB.Pstr(strData, ";", 2).To<int>(); }
                        }

                        strGubun = VB.Left(strData, 1);
                        strGubun1 = VB.Right(strData, 1);
                        if (strGubun == "4")
                        {
                            if (strGubun1 == "잔") { nITRVW33 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "병") { nITRVW34 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "캔") { nITRVW35 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "C")  { nITRVW36 = VB.Pstr(strData, ";", 2).To<int>(); }
                        }

                        strGubun = VB.Left(strData, 1);
                        strGubun1 = VB.Right(strData, 1);
                        if (strGubun == "5")
                        {
                            if (strGubun1 == "잔") { nITRVW37 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "병") { nITRVW38 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "캔") { nITRVW39 = VB.Pstr(strData, ";", 2).To<int>(); }
                            if (strGubun1 == "C")  { nITRVW40 = VB.Pstr(strData, ";", 2).To<int>(); }
                        }
                    }
                }

                FstrREC.Append("" + (char)34 + "ITEM46" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW1) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM47" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW2) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM48" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW3) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM49" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW4) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM50" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW5) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM51" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW6) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM52" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW7) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM53" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW8) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM54" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW9) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM55" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW10) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM56" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW11) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM57" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW12) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM58" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW13) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM59" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW14) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW15) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW16) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM62" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW17) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM63" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW18) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM64" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW19) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM65" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW20) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM66" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW21) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW22) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM68" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW23) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM69" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW24) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM70" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW25) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM71" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW26) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM72" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW27) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM73" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW28) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM74" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW29) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM75" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW30) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM76" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW31) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM77" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW32) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM78" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW33) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM79" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW34) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM80" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW35) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM81" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW36) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM82" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW37) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM83" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW38) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM84" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW39) + VB.Space(6), 6) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM85" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", nITRVW40) + VB.Space(6), 6) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 운동() - 운동,신체활동,노인기능평가
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        void fn_Exercise(long argMirno, long argWRTNO)
        {
            string sJepDate = "";

            sJepDate = cboYear.Text + "-01-01";

            HIC_RES_BOHUM1_JEPSU_PATIENT list = hicResBohum1JepsuPatientService.GetItembyJepDateMirNoWrtNo(sJepDate, argMirno, argWRTNO);

            if (!list.IsNullOrEmpty())
            {
                //신체활동(운동-고강도)
                if (list.T_ACTIVE1.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM86" + (char)34 + ":" + (char)34 + VB.Left(list.T_ACTIVE1 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM86" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!VB.Pstr(list.TMUN0011, ":", 1).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM87" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list.TMUN0011, ":", 1) + VB.Space(2), 2) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM87" + (char)34 + ":" + (char)34 + "00" + (char)34 + ",");
                }
                if (!VB.Pstr(list.TMUN0011, ":", 2).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM88" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list.TMUN0011, ":", 2) + VB.Space(2), 2) + (char)34 + ", ");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM88" + (char)34 + ":" + (char)34 + "00" + (char)34 + ",");
                }

                //신체활동(운동-중강도)
                if (!list.T_ACTIVE2.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM89" + (char)34 + ":" + (char)34 + VB.Left(list.T_ACTIVE2 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM89" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                if (!VB.Pstr(list.TMUN0012, ":", 1).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM90" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list.TMUN0012, ":", 1) + VB.Space(2), 2) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM90" + (char)34 + ":" + (char)34 + "00" + (char)34 + ",");
                }
                if (!VB.Pstr(list.TMUN0012, ":", 2).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM91" + (char)34 + ":" + (char)34 + VB.Left(VB.Pstr(list.TMUN0012, ":", 2) + VB.Space(2), 2) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM91" + (char)34 + ":" + (char)34 + "00" + (char)34 + ",");
                }

                //신체활동(근력)
                if (list.T_ACTIVE3.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM92" + (char)34 + ":" + (char)34 + VB.Left(list.T_ACTIVE3 + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM92" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                //노인기능평가(만66세, 70세, 80세 대상자)
                if (chb.READ_PATIENT_AGE(argWRTNO) == 66 || chb.READ_PATIENT_AGE(argWRTNO) == 70 || chb.READ_PATIENT_AGE(argWRTNO) == 80)
                {
                    if (list.T66_INJECT == "1")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM93" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM93" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                if (list.TMUN0013 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM94" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM94" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
                if (list.T66_STAT1 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM95" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM95" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                if (list.T66_STAT2 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM96" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM96" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
                if (list.T66_STAT3 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM97" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM97" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
                if (list.T66_STAT4 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM98" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM98" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                if (list.T66_STAT5 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM99" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM99" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
                if (list.T66_STAT6 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM100" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM100" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                if (list.T66_FALL == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM101" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM101" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
                if (list.T66_URO == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM102" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM102" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM93" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM94" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM95" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM96" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM97" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM98" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM99" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM100" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM101" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM102" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 혈액검사()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        /// <param name="argCHK2"></param>
        /// <param name="argCHK3"></param>
        void fn_BloodExam(long argMirno, long argWRTNO, string argCHK2, string argCHK3)
        {
            string strSex = "";
            long nAge = 0;
            string strCHK2 = "";
            string strCHK3 = "";

            string sJepDate = "";

            sJepDate = cboYear.Text + "-01-01";

            HIC_RES_BOHUM1_JEPSU_PATIENT list = hicResBohum1JepsuPatientService.GetItembyJepDateMirNoWrtNo(sJepDate, argMirno, argWRTNO);

            if (!list.IsNullOrEmpty())
            {
                nAge = list.AGE;
                strSex = list.SEX;
                strCHK2 = argCHK2;
                strCHK3 = argCHK3;

                if (nAge >= 66 && nAge % 2 == 0 && strCHK2 == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                }
                else
                {
                    if (list.URINE2 == "9")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");    //요단백
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM14" + (char)34 + ":" + (char)34 + VB.Left(list.URINE2 + VB.Space(1), 1) + (char)34 + ",");  //요단백
                    }

                    ///TODO : 이상훈 (2021.01.19) 아래 값 신경 써서 확인 해 볼것
                    FstrREC.Append("" + (char)34 + "ITEM15" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", list.BLOOD1).Replace(".", "") + VB.Space(3), 3) + (char)34 + ",");//혈색소
                    FstrREC.Append("" + (char)34 + "ITEM16" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", list.BLOOD2) + VB.Space(3), 3) + (char)34 + ","); //혈당
                }

                if ((strSex == "M" && nAge >= 24 && nAge % 4 == 0 && strCHK2 == "" && strCHK3 == "OK") || (strSex == "F" && nAge >= 40 && nAge % 4 == 0 && strCHK2 == "" && strCHK3 == "OK"))
                {
                    if (string.Compare(chb.READ_Mir_Result1(argWRTNO, "A123", 4, "0"), "0") > 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", chb.READ_Mir_Result1(argWRTNO, "A123", 4, "0")) + VB.Space(4), 4) + (char)34 + ","); ///총콜레스트롤
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    }

                    if (string.Compare(chb.READ_Mir_Result1(argWRTNO, "A241", 4, "0"), "0") > 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + VB.Left(string.Format(chb.READ_Mir_Result1(argWRTNO, "A241", 4, "0"), "0000") + VB.Space(4), 4) + (char)34 + ",");    //트리글리세라이드
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    }

                    if (string.Compare(chb.READ_Mir_Result1(argWRTNO, "A242", 4, "0"), "0") > 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", chb.READ_Mir_Result1(argWRTNO, "A242", 4, "0")) + VB.Space(4), 4) + (char)34 + ",");   //HDL콜레스테롤
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    }

                    //2017-01-10 C404 LDL 결과값이 1이면 공란으로 처리

                    if (string.Format("{0:0000}", chb.READ_Mir_Result1(argWRTNO, "C404", 4, "0")) == "0001" || string.Format("{0:0000}", chb.READ_Mir_Result1(argWRTNO, "C404", 4, "0")) == "0000")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", chb.READ_Mir_Result1(argWRTNO, "C404", 4, "0"), "0000") + VB.Space(4), 4) + (char)34 + ",");    //LDL콜레스테롤
                    }

                    if (!chb.READ_MIR_RESULT2(argWRTNO, "C405").IsNullOrEmpty())
                    {
                        FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", chb.READ_Mir_Result1(argWRTNO, "C405", 4, "0")) + VB.Space(4), 4) + (char)34 + ",");    //LDL콜레스테롤 실측정값
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM17" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM18" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM19" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM20" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM21" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                }

                if (strCHK2 == "OK" && nAge >= 66)
                {
                    FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM22" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", chb.READ_Mir_Result1(argWRTNO, "A274", 4, "0")) + VB.Space(4), 4) + (char)34 + ",");   //크레아티닌
                    FstrREC.Append("" + (char)34 + "ITEM23" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", list.BLOOD4) + VB.Space(4), 4) + (char)34 + ",");   //'AST
                    FstrREC.Append("" + (char)34 + "ITEM24" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", list.BLOOD5) + VB.Space(4), 4) + (char)34 + ",");   //'ALT
                    FstrREC.Append("" + (char)34 + "ITEM25" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", list.BLOOD6) + VB.Space(4), 4) + (char)34 + ",");   //'감마지피티
                    FstrREC.Append("" + (char)34 + "ITEM26" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                                                //GFR 산출방법(MDRD)
                    FstrREC.Append("" + (char)34 + "ITEM27" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", chb.READ_Mir_Result1(argWRTNO, "A116", 3, "0"), "000") + VB.Space(3), 3) + (char)34 + ",");    //GFR
                }
            }
        }

        /// <summary>
        /// B형간염검사()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        void fn_BHepatitistExam(long argMirno, long argWRTNO)
        {
            long nREAD = 0;
            string strPtno = "";

            string sJepDate = "";

            sJepDate = cboYear.Text + "-01-01";

            HIC_RES_BOHUM1_JEPSU_PATIENT list = hicResBohum1JepsuPatientService.GetItembyJepDateMirNoWrtNo(sJepDate, argMirno, argWRTNO);

            if (!list.IsNullOrEmpty())
            {
                //생애대상자만 간염검사여부 확인   2011-12-27
                strPtno = list.PTNO;
                if (chb.READ_PATIENT_AGE(argWRTNO) == 40 && !chb.READ_MIR_RESULT2(argWRTNO, "A258").IsNullOrEmpty())
                {
                    //항원
                    FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + "2" + (char)34 + ","); //1.일반, 2.정밀, 3.핵의학적방법
                    if (chb.READ_MIR_RESULT2(argWRTNO, "A258") == "01")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + "1" + (char)34 + ","); //음성일경우
                    }
                    else if (chb.READ_MIR_RESULT2(argWRTNO, "A258") == "02")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + "2" + (char)34 + ","); //양성일경우
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    }

                    //수치입력(검사실 결과에서 직접읽어옴) 6자리
                    FstrREC.Append("" + (char)34 + "ITEM30" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_RESULT_BHhepatitis(argWRTNO, strPtno, "A258") + VB.Space(6), 6) + (char)34 + ",");   //항원
                    FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + "000001" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM32" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");

                    //항체
                    if (string.Compare(chb.READ_MIR_RESULT2(argWRTNO, "A259"), "10") < 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else if (string.Compare(chb.READ_MIR_RESULT2(argWRTNO, "A259"), "10") >= 0 || chb.READ_MIR_RESULT2(argWRTNO, "A259") == ">785.0")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    }

                    if (chb.READ_Mir_Result1(argWRTNO, "A259", 6, "0") == "999")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + VB.Space(6) + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_Mir_Result1(argWRTNO, "A259", 6, "0") + VB.Space(6), 6) + (char)34 + ",");
                    }

                    FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + "000010" + (char)34 + ",");
                    if (!list.LIVER3.IsNullOrEmpty())
                    {
                        FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + list.LIVER3 + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM28" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM29" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM30" + (char)34 + ":" + (char)34 + VB.Space(6) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM31" + (char)34 + ":" + (char)34 + VB.Space(6) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM32" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM33" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM34" + (char)34 + ":" + (char)34 + VB.Space(6) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM35" + (char)34 + ":" + (char)34 + VB.Space(6) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM36" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }
            }
        }

        /// <summary>
        /// 노인신체기능검사()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        /// <param name="argCHK2"></param>
        void fn_ElderlyPhysicalFunctionExam(long argMirno, long argWRTNO, string argCHK2)
        {
            string strCHK2 = "";
            long nAge = 0;

            string sJepDate = "";

            sJepDate = cboYear.Text + "-01-01";

            HIC_RES_BOHUM1_JEPSU_PATIENT list = hicResBohum1JepsuPatientService.GetItembyJepDateMirNoWrtNo(sJepDate, argMirno, argWRTNO);

            if (!list.IsNullOrEmpty())
            {
                strCHK2 = argCHK2;
                nAge = list.AGE;

                //66세이상 의료급여검진(만66,70,80) 노인신체기능검사
                //★★★
                if (!chb.READ_MIR_RESULT2(argWRTNO, "A118").IsNullOrEmpty()) //하지기능 -읽어나3m
                {
                    FstrREC.Append("" + (char)34 + "ITEM37" + (char)34 + ":" + (char)34 + VB.Left(string.Format(chb.READ_Mir_Result1(argWRTNO, "A118", 2, "0"), "00") + VB.Space(2), 2) + (char)34 + ","); //하지기능 -읽어나3m
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM37" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");                                                                                 //하지기능 -읽어나3m
                }

                if (chb.READ_MIR_RESULT2(argWRTNO, "A119").IsNullOrEmpty())
                {
                    if (chb.READ_Mir_Result1(argWRTNO, "A119", 1, " ") == "01")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                    }
                    else if (chb.READ_Mir_Result1(argWRTNO, "A119", 1, " ") == "02")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else if (chb.READ_Mir_Result1(argWRTNO, "A119", 1, " ") == "03")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM38" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                if (!chb.READ_MIR_RESULT2(argWRTNO, "A118").IsNullOrEmpty())
                {
                    //평행성 눈뜬상태-몇초
                    FstrREC.Append("" + (char)34 + "ITEM39" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM40" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:00}", chb.READ_Mir_Result1(argWRTNO, "A120", 2, " ")) + VB.Space(2), 2) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM39" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM40" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                if (nAge >= 66 && nAge % 2 == 0 && strCHK2 == "OK")
                {
                    FstrREC.Append("" + (char)34 + "ITEM41" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM42" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM41" + (char)34 + ":" + (char)34 + "4" + (char)34 + ","); //흉부방사선검사 촬영 구분 1:직촬(14X14)  2:직촬(14X17) 3:CR/DR 4:Full Pacs 5:미촬영 강제4번
                    if (!list.XRAYRES.IsNullOrEmpty())
                    {
                        FstrREC.Append("" + (char)34 + "ITEM42" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:00}", list.XRAYRES) + VB.Space(2), 2) + (char)34 + ","); //흉부방사선검사
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM42" + (char)34 + ":" + (char)34 + VB.Space(2) + (char)34 + ",");
                    }
                }

                //66세이상 의료급여검진(만66세 여성)
                if (!chb.READ_MIR_RESULT2(argWRTNO, "TX07").IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM43" + (char)34 + ":" + (char)34 + "1" + (char)34 + ","); //수탁의뢰여부(1.자체검사, 2.의뢰검사)
                    FstrREC.Append("" + (char)34 + "ITEM44" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ","); //수탁의뢰검진기과기호
                    FstrREC.Append("" + (char)34 + "ITEM45" + (char)34 + ":" + (char)34 + "1" + (char)34 + ","); //검사방법

                    if (chb.READ_MIR_RESULT2(argWRTNO, "TX07") == "01")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM46" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else if (chb.READ_MIR_RESULT2(argWRTNO, "TX07") == "02")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM46" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                    }
                    else if (chb.READ_MIR_RESULT2(argWRTNO, "TX07") == "03")
                    {
                        FstrREC.Append("" + (char)34 + "ITEM46" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM46" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    }

                    FstrREC.Append("" + (char)34 + "ITEM47" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM43" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM44" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM45" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM46" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM47" + (char)34 + ":" + (char)34 + VB.Space(4) + (char)34 + ",");
                }

            }
        }

        /// <summary>
        /// 진찰및상담()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        void fn_ClinicCounsel(long argMirno, long argWRTNO)
        {
            string strCHK2 = "";
            long nAge = 0;

            string sJepDate = "";

            sJepDate = cboYear.Text + "-01-01";

            HIC_RES_BOHUM1_JEPSU_PATIENT list = hicResBohum1JepsuPatientService.GetItembyJepDateMirNoWrtNo(sJepDate, argMirno, argWRTNO);

            if (!list.IsNullOrEmpty())
            {
                if (list.T_STAT01 == "1" || list.T_STAT11 == "1" || list.T_STAT21 == "1" || list.T_STAT31 == "1" || list.T_STAT41 == "1" || list.T_STAT51 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM48" + (char)34 + ":" + (char)34 + "2" + (char)34 + ","); //과거병력 1무,2유
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM48" + (char)34 + ":" + (char)34 + "1" + (char)34 + ","); //과거병력 1무,2유
                }

                if (list.T_STAT02 == "1" || list.T_STAT12 == "1" || list.T_STAT22 == "1" || list.T_STAT32 == "1" || list.T_STAT42 == "1" || list.T_STAT52 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM49" + (char)34 + ":" + (char)34 + "2" + (char)34 + ","); //약물치료 1무,2유
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM49" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");    //약물치료 1무,2유
                }

                if (list.HABIT2 == "1") //흡연  0.양호 1.개선 ->실제양식은 1양호 2.개선임 치환함
                {
                    FstrREC.Append("" + (char)34 + "ITEM50" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM50" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }

                if (list.HABIT1 == "1") //음주  0.양호 1.개선
                {
                    FstrREC.Append("" + (char)34 + "ITEM51" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM51" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }

                if (list.HABIT3 == "1") //운동  0.양호 1.개선
                {
                    FstrREC.Append("" + (char)34 + "ITEM52" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM52" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                if (list.HABIT4 == "1") //체중  0.양호 1.개선
                {
                    FstrREC.Append("" + (char)34 + "ITEM53" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM53" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                //진단 및 상담(공백2)
                FstrREC.Append("" + (char)34 + "ITEM54" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM55" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 체위검사()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWrtNo"></param>
        void fn_PhysicalExam(long argMirno, long argWrtNo)
        {
            long nREAD = 0;

            string sJepDate = "";

            sJepDate = cboYear.Text + "-01-01";

            HIC_RES_BOHUM1_JEPSU_PATIENT list = hicResBohum1JepsuPatientService.GetItembyJepDateMirNoWrtNo(sJepDate, argMirno, argWrtNo);

            if (!list.IsNullOrEmpty())
            {
                //체위검사
                FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", list.HEIGHT * 10) + VB.Space(4), 4) + (char)34 + ",");                                  //신장
                FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", list.WEIGHT * 10) + VB.Space(4), 4) + (char)34 + ",");                                  //체중
                FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:00}", list.EYE_L).Replace(".", "") + VB.Space(2), 2) + (char)34 + ",");               //시력(좌)
                FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:00}", list.EYE_R).Replace(".", "") + VB.Space(2), 2) + (char)34 + ",");               //시력(우)
                FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + VB.Left(list.EAR_L + VB.Space(1), 1) + (char)34 + ",");                                                        //청력(좌)
                FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + VB.Left(list.EAR_R + VB.Space(1), 1) + (char)34 + ",");                                                        //청력(우)
                FstrREC.Append("" + (char)34 + "ITEM10" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:0000}", list.WAIST * 10) + VB.Space(4), 4) + (char)34 + ",");                       //허리둘레
                FstrREC.Append("" + (char)34 + "ITEM11" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", chb.READ_Mir_Result1(argWrtNo, "A117", 3, "0").Replace(".", "")) + VB.Space(3), 3) + (char)34 + ","); //체질량지수
                if (list.BLOOD_H != 0 || list.BLOOD_L != 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM12" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", list.BLOOD_H) + VB.Space(3), 3) + (char)34 + ",");                                      //혈압최고
                    FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", list.BLOOD_L) + VB.Space(3), 3) + (char)34 + ",");                                      //
                }
                else
                {
                    //2020-07-02 검사없을시 공백
                    FstrREC.Append("" + (char)34 + "ITEM12" + (char)34 + ":" + (char)34 + "" + (char)34 + ","); //혈압최고
                    FstrREC.Append("" + (char)34 + "ITEM13" + (char)34 + ":" + (char)34 + "" + (char)34 + ","); //혈압최저
                }
            }
        }

        /// <summary>
        /// 정신건강검사() - (우울증)
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        /// <param name="argGbSlip"></param>
        /// <param name="argJemsu"></param>
        void fn_MentalHealthExam(long argMirno, long argWRTNO, string argGbSlip, long argJemsu)
        {
            long nREAD = 0;
            string strGbSlip = "";
            string strPHQScr = "";
            long nJumSu = 0;
            long nJumSu1 = 0;
            bool b자살생각 = false;
            string strJumSu = "";
            string[] sCodes = { "80902", "80903", "80904" };

            strGbSlip = argGbSlip;
            strPHQScr = argJemsu.To<string>();

            b자살생각 = false;
            strJumSu = "";

            if (strGbSlip == "Y")
            {
                List<HIC_TITEM> list = hicTitemService.GetItembyWrtNoGubun(argWRTNO, "18");

                if (list.Count > 0)
                {
                    nREAD = list.Count;

                    FstrREC.Append("" + (char)34 + "ITEM56" + (char)34 + ":" + (char)34 + VB.Right(list[0].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM57" + (char)34 + ":" + (char)34 + VB.Right(list[1].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM58" + (char)34 + ":" + (char)34 + VB.Right(list[2].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM59" + (char)34 + ":" + (char)34 + VB.Right(list[3].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + VB.Right(list[4].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + VB.Right(list[5].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM62" + (char)34 + ":" + (char)34 + VB.Right(list[6].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM63" + (char)34 + ":" + (char)34 + VB.Right(list[7].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM64" + (char)34 + ":" + (char)34 + VB.Right(list[8].CODE, 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM56" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM57" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM58" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM59" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM62" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM63" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM64" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                nJumSu = hicTitemService.GetCountbyWrtNoGubunCode(argWRTNO, "18", sCodes);

                b자살생각 = false;
                if (nJumSu > 0)
                {
                    b자살생각 = true;
                }

                strJumSu = nJumSu.To<string>();

                if (b자살생각 == true)
                {
                    FstrREC.Append("" + (char)34 + "ITEM65" + (char)34 + ":" + (char)34 + "4" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM66" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", strPHQScr) + VB.Space(3), 3) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + strJumSu + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM68" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    if (string.Compare(strPHQScr, "4") <= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM65" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else if (string.Compare(strPHQScr, "9") <= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM65" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                    }
                    else if (string.Compare(strPHQScr, "19") <= 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM65" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM65" + (char)34 + ":" + (char)34 + "4" + (char)34 + ",");
                    }

                    nJumSu += 1;

                    FstrREC.Append("" + (char)34 + "ITEM66" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", strPHQScr) + VB.Space(3), 3) + (char)34 + ",");
                    if (nJumSu == 0)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else if (nJumSu == 1)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                    }
                    else if (nJumSu == 2)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + "3" + (char)34 + ",");
                    }
                    else if (nJumSu == 3)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + "4" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }

                    if (nJumSu >= 1)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM68" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM68" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    }
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM56" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM57" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM58" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM59" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM60" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM61" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM62" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM63" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM64" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM65" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM66" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM67" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM68" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 인지기능장애()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        /// <param name="argGbSlip"></param>
        /// <param name="argJemsu"></param>
        void fn_IsolatedCognitiveImpairment(long argMirno, long argWRTNO, string argGbSlip, long argJemsu)
        {
            long nREAD = 0;
            string strGbSlip = "";
            string strKDSQScr = "";
            long nJumSu = 0;

            strGbSlip = argGbSlip;
            strKDSQScr = argJemsu.To<string>();

            if (strGbSlip == "Y")
            {
                List<HIC_TITEM> list = hicTitemService.GetItembyWrtNoGubun(argWRTNO, "19");

                if (list.Count > 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM69" + (char)34 + ":" + (char)34 + VB.Right(list[0].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM70" + (char)34 + ":" + (char)34 + VB.Right(list[1].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM71" + (char)34 + ":" + (char)34 + VB.Right(list[2].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM72" + (char)34 + ":" + (char)34 + VB.Right(list[3].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM73" + (char)34 + ":" + (char)34 + VB.Right(list[4].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM74" + (char)34 + ":" + (char)34 + VB.Right(list[5].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM75" + (char)34 + ":" + (char)34 + VB.Right(list[6].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM76" + (char)34 + ":" + (char)34 + VB.Right(list[7].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM77" + (char)34 + ":" + (char)34 + VB.Right(list[8].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM78" + (char)34 + ":" + (char)34 + VB.Right(list[9].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM79" + (char)34 + ":" + (char)34 + VB.Right(list[10].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM80" + (char)34 + ":" + (char)34 + VB.Right(list[11].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM81" + (char)34 + ":" + (char)34 + VB.Right(list[12].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM82" + (char)34 + ":" + (char)34 + VB.Right(list[13].CODE, 1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM83" + (char)34 + ":" + (char)34 + VB.Right(list[14].CODE, 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM69" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM70" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM71" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM72" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM73" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM74" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM75" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM76" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM77" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM78" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM79" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM80" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM81" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM82" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM83" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                }

                //인지기능장애
                //인지기능장애 KDSQ-C
                if (string.Compare(strKDSQScr, "5") <= 0)
                {
                    FstrREC.Append("" + (char)34 + "ITEM84" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM84" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }
                //인지기능장애 점수
                FstrREC.Append("" + (char)34 + "ITEM85" + (char)34 + ":" + (char)34 + VB.Left(string.Format("{0:000}", strKDSQScr) + VB.Space(3), 3) + (char)34 + ",");
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM69" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM70" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM71" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM72" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM73" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM74" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM75" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM76" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM77" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM78" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM79" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM80" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM81" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM82" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM83" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM84" + (char)34 + ":" + (char)34 + VB.Space(1) + (char)34 + ",");
                FstrREC.Append("" + (char)34 + "ITEM85" + (char)34 + ":" + (char)34 + VB.Space(3) + (char)34 + ",");
            }
        }

        /// <summary>
        /// 종합소견및판정()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        void fn_TotalOpinionJudgment(long argMirno, long argWrtNo)
        {
            string strPanEtc = "";
            string sJepDate = "";

            sJepDate = cboYear.Text + "-01-01";

            HIC_RES_BOHUM1_JEPSU_PATIENT list = hicResBohum1JepsuPatientService.GetItembyJepDateMirNoWrtNo(sJepDate, argMirno, argWrtNo);

            if (!list.IsNullOrEmpty())
            {
                if (list.PANJENG == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM86" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");     //종합판정-정상A
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM86" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                if (list.PANJENGB1 == "1" || list.PANJENGB2 == "1" || list.PANJENGB3 == "1" || list.PANJENGB4 == "1" || list.PANJENGB5 == "1" || list.PANJENGB6 == "1" || list.PANJENGB7 == "1" || list.PANJENGB8 == "1" || list.PANJENGB9 == "1" || list.PANJENGB10 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM87" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");     //종합판정-정상B
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM87" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (list.PANJENGR1 == "1" || list.PANJENGR2 == "1" || list.PANJENGR4 == "1" || list.PANJENGR5 == "1" || list.PANJENGR7 == "1" || list.PANJENGR8 == "1" || list.PANJENGR9 == "1" || list.PANJENGR10 == "1" || list.PANJENGR11 == "1" || list.PANJENGR12 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM88" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");     //종합판정-R1
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM88" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                if (list.PANJENGR3 == "1" || list.PANJENGR6 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM89" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");     //종합판정-R2
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM89" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                if (list.PANJENGU1 == "1" || list.PANJENGU2 == "1" || list.PANJENGU3 == "1" || list.PANJENGU4 == "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM90" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");     //종합판정-D -유질환 2010
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM90" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGB1.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM91" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGB1 + VB.Space(1), 1) + (char)34 + ",");     //정상B-비만관리
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM91" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }


                if (!list.PANJENGB2.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM92" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGB2 + VB.Space(1), 1) + (char)34 + ",");     //혈압관리
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM92" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGB3.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM93" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGB3 + VB.Space(1), 1) + (char)34 + ",");     //콜레스트롤관리
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM93" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGB4.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM94" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGB4 + VB.Space(1), 1) + (char)34 + ",");     //간기능관리
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM94" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGB5.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM95" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGB5 + VB.Space(1), 1) + (char)34 + ",");      //당뇨관리
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM95" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGB6.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM96" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGB6 + VB.Space(1), 1) + (char)34 + ",");     //신장기능관리
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM96" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGB7.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM97" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGB7 + VB.Space(1), 1) + (char)34 + ",");     //빈혈관리
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM97" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGB8.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM98" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGB8 + VB.Space(1), 1) + (char)34 + ",");      //골다공증
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM98" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                //2019추가
                if (!list.PANJENGB10.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM99" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGB10 + VB.Space(1), 1) + (char)34 + ",");     //비활동성폐결핵
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM99" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGB9.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM100" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGB9 + VB.Space(1), 1) + (char)34 + ",");     //기타질환
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM100" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM101" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGB_ETC_DTL + VB.Space(1), 1) + (char)34 + ",");         //기타질환관리 세부

                //종합판정 일반질환의심(R1)
                if (!list.PANJENGR1.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM102" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGR1 + VB.Space(1), 1) + (char)34 + ",");     //R1-폐결핵의심
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM102" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGR2.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM103" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGR2 + VB.Space(1), 1) + (char)34 + ",");     //기타흉부질환의심
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM103" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGR4.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM104" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGR4 + VB.Space(1), 1) + (char)34 + ",");     //이상지질혈증
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM104" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGR5.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM105" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGR5 + VB.Space(1), 1) + (char)34 + ",");     //간장질환의심
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM105" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGR7.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM106" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGR7 + VB.Space(1), 1) + (char)34 + ",");    //신장질환의심
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM106" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGR8.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM107" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGR8 + VB.Space(1), 1) + (char)34 + ",");     //빈혈증의심
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM107" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGR9.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM108" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGR9 + VB.Space(1), 1) + (char)34 + ",");     //골다골증
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM108" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGR12.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM109" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGR12 + VB.Space(1), 1) + (char)34 + ",");      //난청
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM109" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGR11.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM110" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGR11 + VB.Space(1), 1) + (char)34 + ",");     //비만
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM110" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                //---------------------------------------------------------------------------------
                if (!list.PANJENGR10.IsNullOrEmpty() || !list.PANJENGR11.IsNullOrEmpty())
                {
                    strPanEtc = chb.HIC_STRCUTL(list.PANJENGR_ETC, 40);
                    if (!list.PANJENGR10.IsNullOrEmpty())
                    {
                        FstrREC.Append("" + (char)34 + "ITEM111" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGR10 + VB.Space(1), 1) + (char)34 + ",");        //기타질환의심
                        FstrREC.Append("" + (char)34 + "ITEM112" + (char)34 + ":" + (char)34 + VB.Left(strPanEtc + VB.Space(40), 40) + (char)34 + ",");      //기타질환의심 세부
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM111" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGR11 + VB.Space(1), 1) + (char)34 + ",");        //비만의심-자체처리
                        FstrREC.Append("" + (char)34 + "ITEM112" + (char)34 + ":" + (char)34 + VB.Left(strPanEtc + VB.Space(40), 40) + (char)34 + ",");         //기타질환의심 세부
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM111" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM112" + (char)34 + ":" + (char)34 + VB.Space(40) + (char)34 + ",");
                }

                if (!list.PANJENGR3.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM113" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGR3 + VB.Space(1), 1) + (char)34 + ",");    //R2-고혈압의심
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM113" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
                if (!list.PANJENGR6.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM114" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGR6 + VB.Space(1), 1) + (char)34 + ",");      //당뇨질환의심
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM114" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGU1.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM115" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGU1 + VB.Space(1), 1) + (char)34 + ",");     //유질환D -고혈압
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM115" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGU2.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM116" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGU2 + VB.Space(1), 1) + (char)34 + ",");      //유질환D -당뇨
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM116" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGU3.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM117" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGU3 + VB.Space(1), 1) + (char)34 + ",");       //유질환D -이상지질혈증
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM117" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                if (!list.PANJENGU4.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM118" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGU4 + VB.Space(1), 1) + (char)34 + ",");      //유질환D -폐결핵
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM118" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }

                FstrREC.Append("" + (char)34 + "ITEM119" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(list.SOGEN, 500) + VB.Space(500), 500) + (char)34 + ",");  //의심질환소견
                FstrREC.Append("" + (char)34 + "ITEM120" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(list.SOGENB, 500) + VB.Space(500), 500) + (char)34 + ","); //유질환 소견
                FstrREC.Append("" + (char)34 + "ITEM121" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(list.SOGENC, 500) + VB.Space(500), 500) + (char)34 + ","); //생활습관관리 소견
                FstrREC.Append("" + (char)34 + "ITEM122" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_STRCUTL(list.SOGEND, 500) + VB.Space(500), 500) + (char)34 + ","); //기타 소견
            }
        }

        /// <summary>
        /// 청구자료()
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argWRTNO"></param>
        /// <param name="argGunDate"></param>
        /// <param name="argPanDate"></param>
        /// <param name="argTongDate"></param>
        void fn_ExpenseData(long argMirno, long argWrtNo, string argGunDate, string argPanDate, string argTongDate)
        {
            long nREAD = 0;
            string strGunDate = "";
            string strPandate = "";
            string strTongDate = "";
            string strPtno = "";
            string sJepDate = "";

            strGunDate = argGunDate;
            strPandate = argPanDate;
            strTongDate = argTongDate;

            sJepDate = cboYear.Text + "-01-01";

            HIC_RES_BOHUM1_JEPSU_PATIENT list = hicResBohum1JepsuPatientService.GetItembyJepDateMirNoWrtNo(sJepDate, argMirno, argWrtNo);

            if (!list.IsNullOrEmpty())
            {
                strPtno = list.PTNO;
                FstrREC.Append("" + (char)34 + "ITEM123" + (char)34 + ":" + (char)34 + VB.Left(strGunDate + VB.Space(8), 8) + (char)34 + ",");       //검진일자
                FstrREC.Append("" + (char)34 + "ITEM124" + (char)34 + ":" + (char)34 + VB.Left(strPandate + VB.Space(8), 8) + (char)34 + ",");       //판정일자

                if (list.GBCHUL == "Y" && list.GBCHUL2 == "")
                {
                    FstrREC.Append("" + (char)34 + "ITEM125" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");        //검진장소구분(1-출장,2내원)
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM125" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");
                }

                if (list.LTDCODE != 0)                                                      //통보방법
                {
                    hb.READ_Ltd_Name(list.LTDCODE.To<string>());
                    if (clsHcVariable.GstrLtdJuso != list.JUSOAA)
                    {
                        FstrREC.Append("" + (char)34 + "ITEM126" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");
                    }
                    else
                    {
                        FstrREC.Append("" + (char)34 + "ITEM126" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");    //회사코드와 접수시 주소가 다르면 개인으로
                    }
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM126" + (char)34 + ":" + (char)34 + "2" + (char)34 + ",");        //회사코드 없으면 개인으로
                }

                FstrREC.Append("" + (char)34 + "ITEM127" + (char)34 + ":" + (char)34 + VB.Left(strTongDate + VB.Space(8), 8) + (char)34 + ",");       //통보일자
                if (!list.PANJENGDRNO.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM128" + (char)34 + ":" + (char)34 + VB.Left(list.PANJENGDRNO + VB.Space(10), 10) + (char)34 + ",");                          //의사면허
                    FstrREC.Append("" + (char)34 + "ITEM129" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME(list.PANJENGDRNO.To<string>()) + VB.Space(12), 12) + (char)34 + ",");         //의사성명
                    FstrREC.Append("" + (char)34 + "ITEM130" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN(list.PANJENGDRNO.To<string>()) + VB.Space(13), 13) + (char)34 + ",");        //의사주민번호
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM128" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM129" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM130" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                }

                if (!list.SANGDAMDRNO.IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM131" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNO1(list.SANGDAMDRNO.To<string>()) + VB.Space(10), 10) + (char)34 + ",");          //의사면허
                    FstrREC.Append("" + (char)34 + "ITEM132" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRNAME1(list.SANGDAMDRNO.To<string>()) + VB.Space(12), 12) + (char)34 + ",");        //의사성명
                    FstrREC.Append("" + (char)34 + "ITEM133" + (char)34 + ":" + (char)34 + VB.Left(chb.READ_MIR_DRJUMIN1(list.SANGDAMDRNO.To<string>()) + VB.Space(13), 13) + (char)34 + ",");       //의사주민번호
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM131" + (char)34 + ":" + (char)34 + VB.Space(10) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM132" + (char)34 + ":" + (char)34 + VB.Space(12) + (char)34 + ",");
                    FstrREC.Append("" + (char)34 + "ITEM133" + (char)34 + ":" + (char)34 + VB.Space(13) + (char)34 + ",");
                }

                if (!chb.READ_ResultApplicationAgreeWhether(strPtno, cboYear.Text).IsNullOrEmpty())
                {
                    FstrREC.Append("" + (char)34 + "ITEM134" + (char)34 + ":" + (char)34 + "1" + (char)34 + ",");                                                //결과활용동의여부 -동의1, 동의안함0
                    FstrREC.Append("" + (char)34 + "ITEM135" + (char)34 + ":" + (char)34 + chb.READ_ResultApplicationAgreeWhether(strPtno, cboYear.Text) + (char)34 + ",");     //결과활용동의 일자
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM134" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");                                                //결과활용동의여부 -동의1, 동의안함0
                    FstrREC.Append("" + (char)34 + "ITEM135" + (char)34 + ":" + (char)34 + VB.Space(8) + (char)34 + ",");                                        //결과활용동의 일자
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                int nRow = 0;
                int nREAD = 0;
                string strLtdCode = "";
                string strMirno = "";
                string strFrDate  = "";
                string strToDate = "";
                string strJumin = "";
                string strJong = "";
                List<string> strCodeList = new List<string>();

                sp.Spread_All_Clear(SS3);
                sp.Spread_All_Clear(SS5);

                SS3_Sheet1.Rows[-1].Height = 24;
                SS5_Sheet1.Rows[-1].Height = 24;

                if (e.RowHeader == true)
                {
                    return;
                }

                if (e.Column == 17)
                {
                    return;
                }

                nRow = 0;

                strJong = VB.Left(cboJong.Text, 1);

                if (e.Column == 1)
                {
                    //청구서 보여줌
                    switch (VB.Left(cboJong.Text, 1))
                    {
                        case "1":
                            FrmHcBillExpenses = new frmHcBillExpenses(SS1.ActiveSheet.Cells[e.Row, 1].Text);
                            FrmHcBillExpenses.ShowDialog(this);
                            break;
                        case "3":
                            FrmHcBillOralExamExpenses = new frmHcBillOralExamExpenses(SS1.ActiveSheet.Cells[e.Row, 1].Text.To<long>());
                            FrmHcBillOralExamExpenses.ShowDialog(this);
                            break;
                        case "4":
                        case "E":
                            FrmHcBillCancerExpenses = new frmHcBillCancerExpenses(SS1.ActiveSheet.Cells[e.Row, 1].Text);
                            FrmHcBillCancerExpenses.ShowDialog(this);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    tabBldControl.SelectedTabIndex = 1;
                    tabBldControl.SelectedTab = tabBld2;

                    strMirno = SS1.ActiveSheet.Cells[e.Row, 1].Text;
                    strFrDate = SS1.ActiveSheet.Cells[e.Row, 8].Text;
                    strToDate = SS1.ActiveSheet.Cells[e.Row, 9].Text;
                    strLtdCode = SS1.ActiveSheet.Cells[e.Row, 11].Text;

                    List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetItembyJepDateMirNo(strFrDate, strToDate, strMirno, strJong);

                    nREAD = list.Count;
                    SS3.ActiveSheet.RowCount = nREAD;
                    for (int i = 0; i < nREAD; i++)
                    {
                        strJumin = clsAES.DeAES(list[i].JUMIN2);
                        if (list[i].MURYOAM == "Y")
                        {
                            SS3.ActiveSheet.Cells[i, 0].BackColor = Color.FromArgb(255, 202, 255);
                        }
                        if (VB.Left(cboJong.Text, 1) == "1")
                        {
                            SS3.ActiveSheet.Cells[i, 0].Text = list[i].MIRNO1.To<string>(); //청구번호
                        }
                        if (VB.Left(cboJong.Text, 1) == "3")
                        {
                            SS3.ActiveSheet.Cells[i, 0].Text = list[i].MIRNO2.To<string>(); //청구번호
                        }
                        if (VB.Left(cboJong.Text, 1) == "4")
                        {
                            SS3.ActiveSheet.Cells[i, 0].Text = list[i].MIRNO3.To<string>(); //청구번호
                        }
                        else if (VB.Left(cboJong.Text, 1) == "E")
                        {
                            SS3.ActiveSheet.Cells[i, 0].Text = list[i].MIRNO3.To<string>(); //청구번호
                        }

                        SS3.ActiveSheet.Cells[i, 1].Text = list[i].JEPDATE;
                        SS3.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.To<string>();
                        SS3.ActiveSheet.Cells[i, 3].Text = list[i].SNAME;
                        SS3.ActiveSheet.Cells[i, 4].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";  //주민번호
                        SS3.ActiveSheet.Cells[i, 5].Text = hb.READ_GjJong_Name(list[i].GJJONG); //검진종류
                        SS3.ActiveSheet.Cells[i, 6].Text = list[i].GJCHASU;
                        //SS3.ActiveSheet.Cells[i, 7].Text = list[i].MIRNO3;
                        SS3.ActiveSheet.Cells[i, 8].Text = hm.SExam_Names_Display(list[i].SEXAMS);  //선택검사
                        SS3.ActiveSheet.Cells[i, 9].Text = hm.UCode_Names_Display(list[i].UCODES);  //유해인자
                        SS3.ActiveSheet.Cells[i, 10].Text = list[i].LTDCODE.To<string>();
                        SS3.ActiveSheet.Cells[i, 11].Text = list[i].KIHO;           //기호
                        SS3.ActiveSheet.Cells[i, 12].Text = list[i].BOGUNSO;        //보건소
                        SS3.ActiveSheet.Cells[i, 13].Text = list[i].GKIHO;          //증기호

                        //GoSub Display_Mir_Cancer_Detail
                        //----------------------------------------------------------------------------------------------------------------------
                        nRow += 1;
                        if (SS5.ActiveSheet.RowCount < nRow)
                        {
                            SS5.ActiveSheet.RowCount = nRow;
                        }

                        if (VB.Left(cboJong.Text, 1) == "4")
                        {
                            SS5.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].MIRNO3.To<string>();  //청구번호
                        }
                        else if (VB.Left(cboJong.Text, 1) == "E")
                        {
                            SS5.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].MIRNO5.To<string>();  //청구번호
                        }

                        SS5.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].JEPDATE;
                        SS5.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].WRTNO.To<string>();
                        SS5.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].SNAME;
                        SS5.ActiveSheet.Cells[nRow - 1, 4].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";   //주민번호
                        SS5.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_GjJong_Name(list[i].GJJONG);          //검진종류

                        //암검사종류를 읽음
                        HIC_CANCER_NEW list3 = hicCancerNewService.GetItembyWrtNo(list[i].WRTNO);

                        if (!list3.IsNullOrEmpty())
                        {
                            if (list3.GBSTOMACH == "1")
                            {
                                if (SS5.ActiveSheet.Cells[nRow - 1, 6].Text.Trim() != "")
                                {
                                    nRow += 1;
                                }

                                if (SS5.ActiveSheet.RowCount < nRow)
                                {
                                    SS5.ActiveSheet.RowCount += 1;
                                }
                                SS5.ActiveSheet.Cells[nRow - 1, 6].Text = "위암";
                            }

                            if (list3.GBRECTUM == "1")
                            {
                                if (SS5.ActiveSheet.Cells[nRow - 1, 6].Text.Trim() != "")
                                {
                                    nRow += 1;
                                }

                                if (SS5.ActiveSheet.RowCount < nRow)
                                {
                                    SS5.ActiveSheet.RowCount += 1;
                                }
                                SS5.ActiveSheet.Cells[nRow - 1, 6].Text = "대장암";
                            }

                            if (list3.GBLIVER == "1")
                            {
                                if (SS5.ActiveSheet.Cells[nRow - 1, 6].Text.Trim() != "")
                                {
                                    nRow += 1;
                                }

                                if (SS5.ActiveSheet.RowCount < nRow)
                                {
                                    SS5.ActiveSheet.RowCount += 1;
                                }
                                SS5.ActiveSheet.Cells[nRow - 1, 6].Text = "간암";
                            }

                            if (list3.GBBREAST == "1")
                            {
                                if (SS5.ActiveSheet.Cells[nRow - 1, 6].Text.Trim() != "")
                                {
                                    nRow += 1;
                                }

                                if (SS5.ActiveSheet.RowCount < nRow)
                                {
                                    SS5.ActiveSheet.RowCount += 1;
                                }
                                SS5.ActiveSheet.Cells[nRow - 1, 6].Text = "유방암";
                            }

                            if (list3.GBWOMB == "1")
                            {
                                if (SS5.ActiveSheet.Cells[nRow - 1, 6].Text.Trim() != "")
                                {
                                    nRow += 1;
                                }

                                if (SS5.ActiveSheet.RowCount < nRow)
                                {
                                    SS5.ActiveSheet.RowCount += 1;
                                }
                                SS5.ActiveSheet.Cells[nRow - 1, 6].Text = "자궁경부암";
                            }
                        }

                        SS5.ActiveSheet.RowCount = SS5.ActiveSheet.NonEmptyRowCount;

                        //----------------------------------------------------------------------------------------------------------------------

                        //휴일,공휴 30%가산 여부
                        strCodeList.Clear();

                        switch (VB.Left(cboJong.Text, 1))
                        {
                            case "4":
                            case "E":
                                strCodeList.Add("1119");
                                break;
                            case "3":
                                strCodeList.Add("1118");
                                break;
                            default:
                                strCodeList.Add("1116");
                                strCodeList.Add("1117");
                                break;
                        }

                        List<HIC_SUNAPDTL> list2 = hicSunapdtlService.GetCodebyWrtNoCode(list[i].WRTNO, strCodeList);

                        if (list2.Count >= 1)
                        {
                            SS3.ActiveSheet.Cells[i, 15].Text = "Y";
                        }
                    }
                }
            }
        }

        void eCboClick(object sender, EventArgs e)
        {
            if (VB.Left(cboJong.Text, 1) != "1")
            {
                chkLtd.Visible = false;
                chkLtd.Checked = false;
            }
            else
            {
                chkLtd.Visible = true;
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                }
            }
            else if (sender == cboYear)
            {
                cboJong.Focus();
            }
            else if (sender == cboJong)
            {
                SendKeys.Send("{Tab}");
            }
        }


        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (sender == SS1)
            {
                long nMirno = 0;
                string strnHicNo = "";
                string strJong = "";
                string strTable = "";
                int result = 0;

                if (e.Column != 19)
                {
                    return;
                }

                nMirno = SS1.ActiveSheet.Cells[e.Row, e.Column].Text.To<long>();
                strnHicNo = SS1.ActiveSheet.Cells[e.Row, e.Column].Text;

                strJong = VB.Left(cboJong.Text, 1);

                clsDB.setBeginTran(clsDB.DbCon);

                result = comHpcLibBService.UpdateNHicNobyMirNo(strnHicNo, nMirno, strJong);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                if (hicMirCancerBoService.GetCountbyMirNo(nMirno) > 0)
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    result = hicMirCancerBoService.UpdateNhicNobyMirNo(strnHicNo, nMirno);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    clsDB.setCommitTran(clsDB.DbCon);
                }

                MessageBox.Show("저장 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }  
        }

        /// <summary>
        /// 암청구종목()
        /// </summary>
        /// <param name="argWRTNO"></param>
        void fn_CancerChargeKind(long argWRTNO)
        {
            //'위암
            if (clsHcType.B3.GBSTOMACH == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 1, 1) != "1")
            { 
                if (VB.Mid(clsHcType.B3.Can_MirGbn, 1, 1) != "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_Cancer_Bill_Jong(argWRTNO) + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM4" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //'대장암
            if (clsHcType.B3.GBRECTUM == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 2, 1) != "1")
            {
                if (VB.Mid(clsHcType.B3.Can_MirGbn, 2, 1) != "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_Cancer_Bill_Jong(argWRTNO) + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM5" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //'간암
            if (clsHcType.B3.GbLiver == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 3, 1) != "1")
            {
                if (VB.Mid(clsHcType.B3.Can_MirGbn, 3, 1) != "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_Cancer_Bill_Jong(argWRTNO) + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM6" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //'유방암
            if (clsHcType.B3.GBBREAST == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 4, 1) != "1")
            {
                if (VB.Mid(clsHcType.B3.Can_MirGbn, 4, 1) != "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_Cancer_Bill_Jong(argWRTNO) + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM7" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //'자궁암
            if (clsHcType.B3.GbWomb == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 5, 1) != "1")
            {
                if (VB.Mid(clsHcType.B3.Can_MirGbn, 5, 1) != "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_Cancer_Bill_Jong(argWRTNO) + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM8" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
            }

            //'2019-08-06
            //'폐암
            if (clsHcType.B3.GBLUNG == "1" && VB.Mid(clsHcType.B3.Can_MirGbn, 6, 1) != "1")
            {
                if (VB.Mid(clsHcType.B3.Can_MirGbn, 6, 1) != "1")
                {
                    FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + VB.Left(chb.HIC_Cancer_Bill_Jong(argWRTNO) + VB.Space(1), 1) + (char)34 + ",");
                }
                else
                {
                    FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
                }
            }
            else
            {
                FstrREC.Append("" + (char)34 + "ITEM9" + (char)34 + ":" + (char)34 + "0" + (char)34 + ",");
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

        /// <summary>
        /// 건진1차
        /// </summary>
        /// <param name="argMirno"></param>
        /// <param name="argFDate"></param>
        /// <param name="argToDate"></param>
        void fn_Check_Bohum1(long argMirno, string argFDate, string argToDate)
        {
            int nREAD = 0;
            long nWRTNO = 0;
            long nPano = 0;
            string strJepDate = "";
            string strGbChul = "";

            FnMirNo = argMirno; //청구번호
            Fn절사금액합계1 = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            //건진1차 청구오류 점검
            List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDateMirNoCjChasu(argFDate, argToDate, argMirno, "1");

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                nWRTNO = list[i].WRTNO;
                FstrSname = list[i].SNAME;
                FstrSetYear = list[i].SEX;
                FnAge = list[i].AGE;
                FstrJong = list[i].GJJONG;
                strJepDate = list[i].JEPDATE;
                strGbChul = list[i].GBCHUL;
                //검진1차 검사결과를 HIC_RES_BOHUM1에 UPDATE

                chb.READ_HIC_RES_BOHUM1(nWRTNO);
                fn_One_Mir_Sunap(nWRTNO);
                if (FstrCOMMIT != "OK")
                {
                    break;
                }

                //검진결과,문진표를 읽음
                chb.READ_HIC_RES_BOHUM1(nWRTNO);
                if (clsHcType.B1.ROWID.IsNullOrEmpty())
                {
                    break;
                }

                //GoSub Check_Bohum1_ERROR_Check (오류점검)
                fn_Check_Bohum1_ERROR_Check(nWRTNO, strJepDate, strGbChul);
            }

            txtGongWon.Text = Fn절사금액합계1.To<string>();

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 오류점검
        /// </summary>
        void fn_Check_Bohum1_ERROR_Check(long argWRTNO, string argJepDate, string argGbChul)
        {
            int nCntB = 0;
            int nCntR = 0;
            string strResult = "";

            if (clsHcType.B1.Panjeng.IsNullOrEmpty())
            {
                fn_ERROR_INSERT(argWRTNO, "2", "통보일자가 공란입니다.");
            }

            if (string.Compare(clsHcType.B1.TongboGbn, "1") < 0 || string.Compare(clsHcType.B1.TongboGbn, "3") > 0)
            {
                fn_ERROR_INSERT(argWRTNO, "2", "통보방법이 오류입니다." + clsHcType.B1.TongboGbn);
            }

            if (clsHcType.B1.PanjengR[11] == "1" && clsHcType.B1.PanjengR_Etc.IsNullOrEmpty())
            {
                //fn_ERROR_INSERT(argWRTNO, "2", "기타질환 누락.");
            }

            //진찰결과
            if (string.Compare(clsHcType.B1.JINCHAL[1], "1") < 0 || string.Compare(clsHcType.B1.JINCHAL[1], "2") > 0)
            {
                fn_ERROR_INSERT(argWRTNO, "2", "외상,휴유증 진찰소견 오류");
            }
            if (string.Compare(clsHcType.B1.JINCHAL[2], "1") < 0 || string.Compare(clsHcType.B1.JINCHAL[2], "3") > 0)
            {
                fn_ERROR_INSERT(argWRTNO, "2", "일반상태 진찰소견 오류");
            }

            if (!chb.READ_MIR_RESULT2(argWRTNO, "A259").IsNullOrEmpty())
            {
                if (VB.L(chb.READ_MIR_RESULT2(argWRTNO, "A259").Trim(), "<") > 1 || chb.READ_MIR_RESULT2(argWRTNO, "A259").Trim() == ">9999" ||
                    string.Compare(chb.READ_MIR_RESULT2(argWRTNO, "A259"), "999") >= 0)
                {
                    fn_ERROR_INSERT(argWRTNO, "2", "간염항체(A259) 값 9999이상임 확인바람");
                }
            }

            if (VB.Pstr(clsHcType.B1.SLIP_BIMAN, ";", 60).To<long>() >= 13)
            {
                fn_ERROR_INSERT(argWRTNO, "2", "비만처방전 개월수 12개월초과");
            }

            if (VB.L(chb.READ_MIR_RESULT2(argWRTNO, "A121"), "확인") > 1)
            {
                fn_ERROR_INSERT(argWRTNO, "2", "(A121) 값 이상임 확인바람");
            }

            strResult = chb.READ_MIR_RESULT2(argWRTNO, "A241");
            if (strResult.IndexOf("확인") > 0)
            {
                fn_ERROR_INSERT(argWRTNO, "2", "(A241) 값 이상임 확인바람");
            }
            else if (VB.Left(strResult, 1) == ">" || VB.Left(strResult, 1) == "<")
            {
                fn_ERROR_INSERT(argWRTNO, "2", "(A241) 값 이상임 확인바람");
            }

            strResult = chb.READ_MIR_RESULT2(argWRTNO, "A259");
            if (strResult.IndexOf("음성") > 0)
            {
                fn_ERROR_INSERT(argWRTNO, "2", "(A259) 값 이상임 확인바람");
            }
            else if (strResult.IndexOf("양성") > 0)
            {
                fn_ERROR_INSERT(argWRTNO, "2", "(A259) 값 이상임 확인바람");
            }

            if (VB.L(chb.READ_MIR_RESULT2(argWRTNO, "A241"), "확인") > 1)
            {
                fn_ERROR_INSERT(argWRTNO, "2", "(A241) 값 이상임 확인바람");
            }

            if ((chb.READ_MIR_ExcodeYN(argWRTNO, "A258", "1") == "Y" || chb.READ_MIR_ExcodeYN(argWRTNO, "A259", "1") == "Y") &&
                    (FnAge < 39 || FnAge > 41))
            {
                //일반검진시 추가검사로 간염인경우 체크
                if (chb.READ_HIC_HcKindGubun(FstrJong) == true)
                {
                    fn_ERROR_INSERT(argWRTNO, "2", "간염검사는 40세만 해당됨..검사코드확인요함");
                }
            }

            //중복판정점검
            if (chb.READ_HIC_DuplicatejudgmentCheck("일반", "1", argWRTNO) == true)
            {
                fn_ERROR_INSERT(argWRTNO, "2", "판정이 2건 발생함 .. 확인바람");
            }

            //공휴가산 점검
            FbGonghyu = false;
            if (hb.HIC_Huil_GasanDay(argJepDate) == true)
            {
                FbGonghyu = true;
            }
            if (argGbChul == "Y")
            {
                FbGonghyu = false;
            }

            FnGonghyuCnt = hicSunapdtlService.GetCountbyWrtNoCode(argWRTNO, "1116");

            if (FbGonghyu == true && FnGonghyuCnt == 0)
            {
                fn_ERROR_INSERT(argWRTNO, "2", "공휴 가산 코드 누락");
            }
            else if (FbGonghyu == false && FnGonghyuCnt > 0)
            {
                fn_ERROR_INSERT(argWRTNO, "2", "공휴 가산 코드 오류 산");
            }
        }

        void fn_Mir_BohumBill_Clear()
        {
            clsHcType.TMB.ONE_Qty = 0;
            clsHcType.TMB.ONE_TAmt = 0;
            for (int i = 0; i <= 50; i++)
            {
                clsHcType.TMB.ONE_Inwon[i] = 0;
            }
            clsHcType.TMB.TWO_Qty = 0;
            clsHcType.TMB.TWO_TAmt = 0;
            for (int i = 0; i <= 50; i++)
            {
                clsHcType.TMB.TWO_Inwon[i] = 0;
            }
        }

        void fn_Mir_DentalBill_Clear()
        {
            clsHcType.TMD.JepQty = 0;
            clsHcType.TMD.TAmt = 0;
            clsHcType.TMD.HuQty = 0;
        }
        
        void fn_SET_BillAmt_Cancer(string argYear, string argFrDate)
        {
            int nREAD = 0;
            string strSDate = "";
            string strREC = "";
            string strCode = "";
            string strTemp = "";
            int inx = 0;
            long nAmt = 0;
            string strPrice = "";
            long[] nPrice1 = new long[51];
            long[] nPrice2 = new long[51];
            long[] nPrice3 = new long[51];

            string strFDate = "";

            //기준일자 설정
            strSDate = argFrDate;

            if (string.Compare(argYear, VB.Left(argFrDate, 4)) < 0)
            {
                strSDate = argYear + "-12-31";
            }

            //항목을 Clear
            for (int i = 0; i <= 50; i++)
            {
                FnAmAmt1[i] = 0;
                FnAmAmt2[i] = 0;
                FnAmAmt3[i] = 0;
                nPrice1[i] = 0;
                nPrice2[i] = 0;
                nPrice3[i] = 0;
            }

            strFDate = VB.Left(strSDate, 4) + "-01-01";

            //기초코드에서 해당년도 청구비용을 읽음
            List<HIC_AMT_CANCER> list = hicAmtCancerService.GetItembySDate(strFDate, strSDate);

            if (list.Count > 0)
            {
                strPrice = list[0].AMT01;
                nPrice1[1] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[1] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[1] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT02;
                nPrice1[2] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[2] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[2] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT03;
                nPrice1[3] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[3] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[3] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT04;
                nPrice1[4] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[4] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[4] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT05;
                nPrice1[5] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[5] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[5] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT06;
                nPrice1[6] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[6] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[6] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT07;
                nPrice1[7] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[7] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[7] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT08;
                nPrice1[8] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[8] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[8] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT09;
                nPrice1[9] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[9] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[9] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT10;
                nPrice1[10] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[10] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[10] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT11;
                nPrice1[11] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[11] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[11] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT12;
                nPrice1[12] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[12] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[12] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT13;
                nPrice1[13] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[13] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[13] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT14;
                nPrice1[14] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[14] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[14] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT15;
                nPrice1[15] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[15] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[15] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT16;
                nPrice1[16] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[16] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[16] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT17;
                nPrice1[17] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[17] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[17] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT18;
                nPrice1[18] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[18] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[18] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT19;
                nPrice1[19] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[19] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[19] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT20;
                nPrice1[20] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[20] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[20] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT21;
                nPrice1[21] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[21] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[21] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT22;
                nPrice1[22] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[22] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[22] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT23;
                nPrice1[23] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[23] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[23] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT24;
                nPrice1[24] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[24] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[24] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT25;
                nPrice1[25] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[25] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[25] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT26;
                nPrice1[26] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[26] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[26] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT27;
                nPrice1[27] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[27] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[27] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT28;
                nPrice1[28] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[28] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[28] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                strPrice = list[0].AMT29;
                nPrice1[29] = VB.Pstr(strPrice, ";", 1).To<long>();  //공단100%
                nPrice2[29] = VB.Pstr(strPrice, ";", 2).To<long>();  //공단90%
                nPrice3[29] = VB.Pstr(strPrice, ";", 3).To<long>();  //보건소

                //포셉비용을 조직검사비에 합산함
                for (int i = 5; i <= 9; i++)
                {
                    nPrice1[i] += nPrice1[26];
                    nPrice2[i] += nPrice2[26];
                    nPrice3[i] += nPrice3[26];
                }

                for (int i = 19; i <= 23; i++)
                {
                    nPrice1[i] += nPrice1[26];
                    nPrice2[i] += nPrice2[26];
                    nPrice3[i] += nPrice3[26];
                }
            }

            for (int i = 1; i <= 29; i++)
            {
                switch (i)
                {
                    case 1:  inx = 25; break; //암검진 상담료
                    case 2:  inx = 27; break;  //암검진 상담료(공휴가산)
                    case 3:  inx = 1 ; break;  //위장조영검사
                    case 4:  inx = 2 ; break;  //위내시경
                    case 5:  inx = 3 ; break;  //위조직검사(1-3개)
                    case 6:  inx = 4 ; break;  //위조직검사(4-6개)
                    case 7:  inx = 5 ; break;  //위조직검사(7-9개)
                    case 8:  inx = 6 ; break;  //위조직검사(10-12개)
                    case 9:  inx = 7 ; break;  //위조직검사(13개이상)
                    case 10: inx = 8 ; break;  //간암:ALT
                    case 11: inx = 9 ; break;  //간암:B형간염항원정밀
                    case 12: inx = 10; break;  //간암:C형간염항체정밀
                    case 13: inx = 11; break;  //간초음파검사
                    case 14: inx = 13; break;  //혈청알파태아단백
                    case 15: inx = 14; break;  //대장암:잠혈반응(RPHA)
                    case 16: inx = 15; break;  //대장암:잠혈반응(분변혈색소)
                    case 17: inx = 16; break;  //대장이중조영검사
                    case 18: inx = 17; break;  //대장내시경검사
                    case 19: inx = 18; break;  //대장조직검사(1-3개)
                    case 20: inx = 19; break;  //대장조직검사(4-6개)
                    case 21: inx = 20; break;  //대장조직검사(7-9개)
                    case 22: inx = 21; break;  //대장조직검사(10-12개)
                    case 23: inx = 22; break;  //대장조직검사(13개이상)
                    case 24: inx = 23; break;  //유방암:유방촬영
                    case 25: inx = 24; break;  //자궁경부세포검사
                    case 26: inx = 28; break;  //포셉
                    case 27: inx = 29; break;  //유방암:편측촬영
                    case 28: inx = 30; break;  //폐암: 저선량흉부CT
                    case 29: inx = 31; break;  //폐암: 사후결과상담
                    default:
                        break;
                }
                FnAmAmt1[inx] = nPrice1[i];
                FnAmAmt2[inx] = nPrice2[i];
                FnAmAmt3[inx] = nPrice3[i];
            }
        }

        /// <summary>
        /// 구강 청구서 저장
        /// </summary>
        void fn_HIC_MIR_DENTAL_Save()
        {
            string strErrChk = "";
            long nCnt = 0;
            long nTotAmt = 0;
            int nREAD = 0;
            int result = 0;

            strErrChk = "";
            List<COMHPC> list = comHpcLibBService.GetHicMirErrorCountbyMirNo(FnMirNo);

            if (list.Count > 0)
            {
                strErrChk = "N";
            }
            else
            {
                strErrChk = "Y";
            }

            HIC_MIR_DENTAL list2 = hicMirDentalService.GetItembyMirno(FnMirNo);

            if (!list2.IsNullOrEmpty())
            {
                nCnt = list2.JEPQTY;
                nTotAmt = (clsHcVariable.GnDentAmt * nCnt) + (clsHcVariable.GnDentAddAmt * clsHcType.TMD.HuQty);
            }

            clsDB.setBeginTran(clsDB.DbCon);

            if (!list2.IsNullOrEmpty())
            {
                if (!strErrChk.IsNullOrEmpty())
                {
                    result = hicMirDentalService.UpdatebyMirNo(strErrChk, nCnt, clsHcType.TMD.HuQty, nTotAmt, FnMirNo);

                    if (result  < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("HIC_MIR_DENTAL UPDATE시 오류가 발생함", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 구강검진
        /// </summary>
        /// <param name="nMirno"></param>
        /// <param name="argFrDate"></param>
        /// <param name="argToDate"></param>
        void fn_Check_Dental(long argMirno, string argFrDate, string argToDate)
        {
            int nREAD = 0;
            long nWRTNO = 0;
            long nPano = 0;
            string strJepDate = "";
            bool bGonghyu = false;
            int nGonghyuCnt = 0;
            string strGbChul = "";

            int nCntB = 0;
            int nCntR = 0;

            FnMirNo = argMirno; //청구번호

            clsDB.setBeginTran(clsDB.DbCon);
            //구강검진 청구오류 점검
            List<HIC_JEPSU_RES_DENTAL> list = hicJepsuResDentalService.GetItembyJepDateMirNo(argFrDate, argToDate, argMirno);

            nREAD = list.Count;
            clsHcType.TMD.JepQty = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                nWRTNO = list[i].WRTNO;
                FstrSname = list[i].SNAME;
                FstrSex = list[i].SEX;
                FnAge = list[i].AGE;
                strJepDate = list[i].JEPDATE;
                strGbChul = list[i].GBCHUL;

                if (FstrCOMMIT != "OK")
                {
                    break;
                }

                //구강 토.공휴일가산
                if (hicSunapdtlService.GetCountbyWrtNoCode(nWRTNO, "1118") > 0)
                {
                    clsHcType.TMD.HuQty += 1;   //토.공휴일 가산
                }

                //검진결과,문진표를 읽음
                chb.READ_HIC_RES_DENTAL(nWRTNO);
                if (clsHcType.B4.ROWID.IsNullOrEmpty())
                {
                    FstrCOMMIT = "NO";
                    break;
                }

                //오류점검
                //GoSub Check_Dental_ERROR_Check
                if (clsHcType.B4.RES_MUNJIN.IsNullOrEmpty())
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "문진표평가 항목누락.. 오류입니다.");
                }
                if (clsHcType.B4.RES_JOCHI.IsNullOrEmpty())
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "조치사항 항목누락.. 오류입니다.");
                }
                if (clsHcType.B4.RES_RESULT.IsNullOrEmpty())
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "구강검사결과 항목누락.. 오류입니다.");
                }

                if (FstrCOMMIT == "NO")
                {
                    break;
                }

                //종합소견
                if (string.Compare(clsHcType.B4.T_PANJENG1, "1") < 0 || string.Compare(clsHcType.B4.T_PANJENG1, "4") > 0)
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "종합판정 1~4 이외의 값 오류입니다.");
                }

                if (clsHcType.B4.PanjengDrNo == 0)
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "판정의사가 공란입니다.. 오류입니다.");
                }

                //문진표 평가
                if (clsHcType.B4.OPDDNT == "2" && VB.Mid(clsHcType.B4.RES_MUNJIN, 1, 1) == "1")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "문진표 1번 항목의 답이 [아니오]인 경우, 문진표 평가의 병력문제가 [있음]으로 표시되어야 함");
                }
                if (clsHcType.B4.T_JILBYUNG1 == "1" && VB.Mid(clsHcType.B4.RES_MUNJIN, 1, 1) == "1")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "문진표 2번 항목의 답이 [예]인 경우, 문진표 평가의 병력문제가 [있음]으로 표시되어야 함");
                }
                if (clsHcType.B4.T_JILBYUNG2 == "1" && VB.Mid(clsHcType.B4.RES_MUNJIN, 1, 1) == "1")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "문진표 3번 항목의 답이 [예]인 경우, 문진표 평가의 병력문제가 [있음]으로 표시되어야 함");
                }
                if (clsHcType.B4.T_FUNCTION1 == "1" && VB.Mid(clsHcType.B4.RES_MUNJIN, 1, 1) == "1")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "문진표 4번 항목의 답이 [예]인 경우, 문진표 평가의 병력문제가 [있음]으로 표시되어야 함");
                }
                if (clsHcType.B4.T_STAT1 == "1" && VB.Mid(clsHcType.B4.RES_MUNJIN, 1, 1) == "1")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "문진표 5번 항목의 답이 [예]인 경우, 문진표 평가의 병력문제가 [있음]으로 표시되어야 함");
                }
                if (clsHcType.B4.T_STAT2 == "1" && VB.Mid(clsHcType.B4.RES_MUNJIN, 1, 1) == "1")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "문진표 6번 항목의 답이 [예]인 경우, 문진표 평가의 병력문제가 [있음]으로 표시되어야 함");
                }
                if (string.Compare(clsHcType.B4.DNTSTATUS, "3") >= 0 && VB.Mid(clsHcType.B4.RES_MUNJIN, 2, 1) == "1")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "문진표 7번 항목의 답이 [3,4,5]인 경우, 문진표 평가의 구강건강인식도 문제가 [있음]으로 표시되어야 함");
                }
                if (clsHcType.B4.T_HABIT5 == "2" && VB.Mid(clsHcType.B4.RES_MUNJIN, 3, 1) == "1")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "문진표 8번 항목의 답이 [아니오]인 경우, 문진표 평가의 구강위생 문제가 [있음]으로 표시되어야 함");
                }
                if (string.Compare(clsHcType.B4.T_HABIT6, "2") >= 0 && VB.Mid(clsHcType.B4.RES_MUNJIN, 3, 1) == "1")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "문진표 10번 항목의 답이 [2,3,4]인 경우, 문진표 평가의 구강위생 문제가 [있음]으로 표시되어야 함");
                }
                if (string.Compare(clsHcType.B4.T_HABIT4, "3") >= 0 && VB.Mid(clsHcType.B4.RES_MUNJIN, 3, 1) == "1")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "문진표 11번 항목의 답이 [3,4,5]인 경우, 문진표 평가의 구강위생 문제가 [있음]으로 표시되어야 함");
                }
                if (string.Compare(clsHcType.B4.T_HABIT7, "2") >= 0 && VB.Mid(clsHcType.B4.RES_MUNJIN, 4, 1) == "1")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "문진표 12번 항목의 답이 [2,3]인 경우, 문진표 평가의 불소이용 문제가 [있음]으로 표시되어야 함");
                }
                if (string.Compare(clsHcType.B4.T_HABIT8, "3") >= 0 && VB.Mid(clsHcType.B4.RES_MUNJIN, 5, 1) == "1")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "문진표 13번 항목의 답이 [3,4,5]인 경우, 문진표 평가의 설탕섭취 문제가 [있음]으로 표시되어야 함");
                }
                if (string.Compare(clsHcType.B4.T_HABIT9, "3") >= 0 && VB.Mid(clsHcType.B4.RES_MUNJIN, 5, 1) == "1")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "문진표 14번 항목의 답이 [3,4,5]인 경우, 문진표 평가의 설탕섭취 문제가 [있음]으로 표시되어야 함");
                }
                if (clsHcType.B4.T_HABIT1 == "2" && VB.Mid(clsHcType.B4.RES_MUNJIN, 6, 1) == "1")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "문진표 15번 항목의 답이 [2]인 경우, 문진표 평가의 흡연 문제가 [있음]으로 표시되어야 함");
                }

                //조치사항
                if (VB.Mid(clsHcType.B4.RES_RESULT, 4, 1) == "2" && VB.Mid(clsHcType.B4.RES_JOCHI, 6, 1) == "0")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "상실치아가 있을 경우, 사후관리 권고사항에 [치아우식치료필요] 항목에 체크되어야 함");
                }
                if (VB.Mid(clsHcType.B4.RES_RESULT, 3, 1) == "2" && clsHcType.B4.T_PANJENG1 == "1")
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "수복치아가  있을 경우, 종합판정B로  체크되어야 함");
                }

                //중복판정점검
                if (chb.READ_HIC_DuplicatejudgmentCheck("구강", "1", nWRTNO) == true)
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "판정이 2건 발생함 .. 확인바람");
                }

                //공휴가산 점검
                bGonghyu = false; 
                if (hb.HIC_Huil_GasanDay(strJepDate) == false)
                {
                    bGonghyu = true;
                }
                if (strGbChul == "Y")
                {
                    bGonghyu = false;
                }

                nGonghyuCnt = hicSunapdtlService.GetCountbyWrtNoCode(nWRTNO, "1118");

                if (bGonghyu == true && nGonghyuCnt == 0)
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "구강검진 공휴 가산 코드 누락");
                }
                else if (bGonghyu == false && nGonghyuCnt > 0)
                {
                    fn_ERROR_INSERT(nWRTNO, "2", "구강검진 공휴 가산 코드 오류 산정");
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void fn_Mir_BohumBill_Count(string argChasu, string argCode, string argResult, string argWRTNO)
        {
            int nRead = 0;

            //1차 건강검진비 청구서 건수 누적
            if (argChasu == "1")
            {
                //검사결과가 없으면 건수 누적을 않함
                if (argResult.IsNullOrEmpty() || argResult == "." || argResult == "결과없음" || argResult == "미실시")
                {
                    return;
                }
                //요단백 99는 미실시
                if (argCode == "A112" && argResult == "99")
                {
                    return;
                }

                //1차 통보건수
                clsHcType.TMB.ONE_Qty += 1;

                switch (argCode)
                {
                    case "A101":
                        clsHcType.TMB.ONE_Inwon[1] += 1;
                        FnOneCnt[1] = 1;       //건강검진상담(신장)
                        break;
                    case "A141":
                    case "A142":
                        if (argResult != "02" && argResult != "11")
                        {
                            clsHcType.TMB.ONE_Inwon[2] += 1;
                            FnOneCnt[2] = 1;            //흉부방사선검사 직접 14*17
                        }
                        break;
                    case "A112":
                        clsHcType.TMB.ONE_Inwon[3] += 1;       //요검사(요단백)
                        FnOneCnt[3] = 0;
                        break;
                    //혈액검사
                    case "A121":
                        clsHcType.TMB.ONE_Inwon[4] += 1;
                        FnOneCnt[4] = 1;      //혈색소
                        break;
                    case "A122":
                        clsHcType.TMB.ONE_Inwon[5] += 1;
                        FnOneCnt[5] = 1;      //혈당
                        break;
                    case "A123":
                        clsHcType.TMB.ONE_Inwon[6] += 1;
                        FnOneCnt[6] = 1;       //총콜레스테롤
                        break;
                    case "A242":
                        clsHcType.TMB.ONE_Inwon[7] += 1;
                        FnOneCnt[7] = 1;       //HDL총콜레스테롤
                        break;
                    case "A241":
                        clsHcType.TMB.ONE_Inwon[8] += 1;
                        FnOneCnt[8] = 1;      //트리그리세라이드-중성지방
                        break;
                    case "A124":
                        clsHcType.TMB.ONE_Inwon[9] += 1;
                        FnOneCnt[9] = 1;       //혈청지오티 AST
                        break;
                    case "A125":
                        clsHcType.TMB.ONE_Inwon[10] += 1;
                        FnOneCnt[10] = 1;    //혈청지피티 ALT
                        break;
                    case "A126":
                        clsHcType.TMB.ONE_Inwon[11] += 1;
                        FnOneCnt[11] = 1;    //감마지피티
                        break;
                    case "A274":
                        clsHcType.TMB.ONE_Inwon[12] += 1;
                        FnOneCnt[12] = 1;   //혈청크레아티닌
                        break;

                    case "C405":
                        clsHcType.TMB.ONE_Inwon[22] += 1;
                        FnOneCnt[22] = 1;  //LDL콜레스테롤[TG 400 이상만]
                        break;
                    case "A258":
                        clsHcType.TMB.ONE_Inwon[14] += 1;
                        FnOneCnt[14] = 1;    //항원-정밀  2009 - 11 - 06 손대영샘 요청 생애만
                        break;
                    case "A259":
                        clsHcType.TMB.ONE_Inwon[16] += 1;
                        FnOneCnt[16] = 1;    //항체-정밀  '2009 - 11 - 06 손대영샘 요청 생애만
                        break;
                    //골밀도-양방사선
                    case "TX07":
                        clsHcType.TMB.ONE_Inwon[17] += 1;
                        FnOneCnt[17] = 1;    //양방사선골밀도
                        break;
                    case "A118":
                        clsHcType.TMB.ONE_Inwon[21] += 1;
                        FnOneCnt[21] = 1;   //노인신체
                        break;
                    default:
                        break;
                }

                //검사결과가 없으면 건수 누적을 않함
                if (argResult.IsNullOrEmpty() || argResult == "." || argResult == "결과없음" || argResult == "미실시")
                {
                    return;
                }

                switch (argCode)
                {
                    //혈당검사
                    case "A148":
                        clsHcType.TMB.TWO_Inwon[3] += 1;
                        FnTwoCnt[3] = 1;             //혈당-자가혈당측정기
                        break;
                    //혈당검사
                    case "A143":
                        clsHcType.TMB.TWO_Inwon[5] += 1;
                        FnTwoCnt[5] = 1;            //흡연
                        break;
                    //생활습관
                    case "A144":
                        clsHcType.TMB.TWO_Inwon[6] += 1;
                        FnTwoCnt[6] = 1;             //음주
                        break;
                    case "A145":
                        clsHcType.TMB.TWO_Inwon[7] += 1;
                        FnTwoCnt[7] = 1;
                        FnTwoCnt[4] = 1;            //운동
                        break;
                    case "A146":
                        clsHcType.TMB.TWO_Inwon[8] += 1;
                        FnTwoCnt[8] = 1;            //영양
                        break;
                    case "A147":
                        clsHcType.TMB.TWO_Inwon[9] += 1;
                        FnTwoCnt[9] = 1;             //비만
                        break;
                    //우울증
                    case "A127":
                        clsHcType.TMB.TWO_Inwon[10] += 1;
                        FnTwoCnt[10] = 1;          //우울증 만40
                        break;
                    case "A128":
                        clsHcType.TMB.TWO_Inwon[11] += 1;
                        FnTwoCnt[11] = 1;          //우울증 만66
                        break;
                    case "A129":
                        clsHcType.TMB.TWO_Inwon[12] += 1;
                        FnTwoCnt[12] = 1;         //인지기능[치매]
                        break;
                    case "A130":
                        clsHcType.TMB.TWO_Inwon[12] += 1;
                        FnTwoCnt[14] = 1;
                        break;
                    default:
                        break;
                }
            }
        }

        void fn_RESULT_Bohum1_Update(long argWRTNO)
        {
            int nREAD = 0;
            int nHuCnt = 0;
            string strCode = "";
            string strResult = "";
            string strSelf = "";
            double nResult = 0;

            string strCHK1 = "";
            string strLife1 = "";
            string strLife2 = "";
            string strGbSlip1 = "";
            string strGbSlip2 = "";
            long nAge = 0;

            string strOK1 = "";
            string strOK2 = "";
            string strOK3 = "";
            string strSex = "";

            int nGan1 = 0;
            int nGan2 = 0;
            string strA131 = "";
            string strA132 = "";

            string strLtd = "";

            int result = 0;

            //검사결과가 저장될 변수를 Clear
            clsHcType.B1.Height = 0; clsHcType.B1.Weight = 0; clsHcType.B1.Biman = ""; clsHcType.B1.Waist = 0;
            clsHcType.B1.BimanRate = 0; clsHcType.B1.EYE_L = 0; clsHcType.B1.EYE_R = 0;
            clsHcType.B1.EAR_L = "0"; clsHcType.B1.EAR_R = "0"; clsHcType.B1.BLOOD_H = 0;
            clsHcType.B1.BLOOD_L = 0; clsHcType.B1.URINE1 = ""; clsHcType.B1.URINE2 = "";
            clsHcType.B1.URINE3 = ""; clsHcType.B1.URINE4 = 0; clsHcType.B1.BLOOD1 = 0;
            clsHcType.B1.BLOOD2 = 0; clsHcType.B1.BLOOD3 = 0; clsHcType.B1.BLOOD4 = 0;
            clsHcType.B1.BLOOD5 = 0; clsHcType.B1.BLOOD6 = 0;
            clsHcType.B1.LIVER1 = ""; clsHcType.B1.LIVER2 = ""; clsHcType.B1.LIVER3 = "0";
            clsHcType.B1.XRayGbn = ""; clsHcType.B1.XRayRes = ""; clsHcType.B1.EKG = "";
            clsHcType.B1.BALANCE = 0; clsHcType.B1.FOOT1 = 0; clsHcType.B1.FOOT2 = 0; clsHcType.B1.FOOT3 = "";
            clsHcType.B1.OSTEO = 0; clsHcType.B1.GbGonghu = "";
            strCHK1 = ""; strLife1 = ""; strLife2 = "";
            strGbSlip1 = ""; strGbSlip2 = ""; nAge = 0; strOK1 = ""; strOK2 = ""; strOK3 = ""; strSex = "";
            strA131 = "";
            strA132 = "";
            nGan1 = 0;
            nGan2 = 0;

            //의료급여 체크
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(argWRTNO);

            if (!list.IsNullOrEmpty())
            {
                if (list.GBCHK1 == "04")
                {
                    strCHK1 = "OK";
                }
                nAge = list.AGE;
                strSex = list.SEX;
            }

            //의료급여 체크
            if (hicSunapdtlService.GetCountbyWrtNo(argWRTNO) > 0)
            {
                strCHK1 = "OK";
            }

            List<HIC_SUNAPDTL> list2 = hicSunapdtlService.GetWrtNoCodeGbSelfbyWrtNo(argWRTNO);

            if (list2.Count > 0)
            {
                nREAD = list2.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    switch (list2[i].CODE)
                    {
                        case "1165":
                            strLife1 = "OK";      //흡연
                            break;
                        case "1166":
                            strLife2 = "OK";      //음주
                            break;
                        case "1167":
                            strGbSlip1 = "OK";    //우울증
                            break;
                        case "1163":
                            strGbSlip2 = "OK";    //인지기능
                            break;
                        default:
                            break;
                    }

                    if (list2[i].CODE == "1160" && (list2[i].GBSELF == "1" || list2[i].GBSELF == "01"))
                    {
                        strOK1 = "OK";  //콜레스트롤(4종)
                    }

                    if (list2[i].CODE == "J193" && (list2[i].GBSELF == "1" || list2[i].GBSELF == "01"))
                    {
                        strOK2 = "OK";  //중성지방400mg
                    }
                }
            }

            List<string> strGbSelf = new List<string>();
            strGbSelf.Clear();
            strGbSelf.Add("1");
            strGbSelf.Add("01");

            if (hicSunapdtlService.GetCountbyWrtNoCodeGbSelf(argWRTNO, "1160", strGbSelf) > 0)
            {
                strOK3 = "OK";
            }

            if ((chb.READ_PATIENT_AGE(argWRTNO) >= 24 && chb.READ_PATIENT_AGE(argWRTNO) % 4 != 0 && strSex == "M") ||
                (chb.READ_PATIENT_AGE(argWRTNO) >= 40 && chb.READ_PATIENT_AGE(argWRTNO) % 4 != 0 && strSex == "W"))
            {
                if (strOK1 == "OK")
                {
                    fn_ERROR_INSERT(argWRTNO, "1", "콜레스트롤(4종) 접수 오류");
                }
            }

            if ((chb.READ_PATIENT_AGE(argWRTNO) >= 66 && chb.READ_PATIENT_AGE(argWRTNO) % 2 != 0) && chb.READ_PATIENT_AGE(argWRTNO) < 66)
            {
                if (strGbSlip2 == "OK")
                {
                    fn_ERROR_INSERT(argWRTNO, "1", "인지기능장애 접수 오류");
                }
            }

            if (chkLtd.Checked == true)
            {
                strLtd = "1";
            }
            else
            {
                strLtd = "";
            }

            //접수번호별 검사결과를 읽어 변수에 저장함
            List<HIC_RESULT_SUNAPDTL> list4 = hicResultSunapdtlService.GetExCodeResultGbSelfbyWrtNo(argWRTNO, strLtd, strOK3);

            nREAD = list2.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strCode = list4[i].EXCODE;
                strResult = list4[i].RESULT;
                strSelf = list4[i].GBSELF;
                nResult = strResult.To<double>();

                //미실시, .은 제외함
                if (strResult == "미실시" || strResult == ".")
                {
                    strResult = "";
                    nResult = 0;
                }

                //시력 좌,우의 (1.0)을 숫자로 변환
                if ((strCode == "A104" || strCode == "A105") && !strResult.IsNullOrEmpty()) //시력좌우
                {
                    if (VB.Left(strResult, 1) == "(" && VB.Right(strResult, 1) == ")")
                    {
                        nResult = VB.Mid(strResult, 2, strResult.Length - 2).To<double>();
                    }
                }

                if (!strResult.IsNullOrEmpty())
                {
                    switch (strCode)
                    {
                        //청력 좌,우 정상,비정상 결과값 변환
                        case "A106":
                        case "A107":
                            switch (strResult)
                            {
                                case "01":
                                case "정상":
                                case "정상(교정)":
                                    strResult = "1";
                                    break;
                                case "02":
                                case "비정상":
                                case "비정상(교정)":
                                    strResult = "2";
                                    break;
                                default:
                                    strResult = "X";
                                    break;
                            }
                            break;
                        //소변검사
                        case "A111":
                        case "A112":
                        case "A113":
                            strResult = VB.Right(strResult, 1);
                            break;
                        //허리둘레
                        case "A115":
                            if (nResult > 999)
                            {
                                nResult = 999;
                            }
                            break;
                        //EKG
                        case "A151":
                            strResult = VB.Right(strResult, 1);
                            break;
                        //부인과 검사결과
                        case "A163":
                        case "A164":
                        case "A165":
                        case "A166":
                        case "A167":
                        case "A169":
                        case "A171":
                            strResult = VB.Right(strResult, 1);
                            break;
                        default:
                            break;
                    }
                }

                //검사코드로 항목별 결과를 저장
                switch (strCode)
                {
                    case "A101":
                        clsHcType.B1.Height = nResult.To<int>();    //신장
                        break;
                    case "A102":
                        clsHcType.B1.Weight = nResult.To<int>();    //체중
                        break;
                    case "A115":
                        clsHcType.B1.Waist = nResult.To<int>();     //허리둘레
                        break;
                    case "A117":
                        //체질량지수
                        break;
                    case "A104":
                        clsHcType.B1.EYE_L = nResult;               //시력(좌)
                        break;
                    case "A105":
                        clsHcType.B1.EYE_R = nResult;               //시력(우)
                        break;
                    case "A106":
                        clsHcType.B1.EAR_L = strResult;             //청력(좌)
                        break;
                    case "A107":
                        clsHcType.B1.EAR_R = strResult;             //청력(우)
                        break;
                    case "A108":
                        clsHcType.B1.BLOOD_H = nResult.To<int>();   //혈압 고
                        break;
                    case "A109":
                        clsHcType.B1.BLOOD_L = nResult.To<int>();   //혈압 저
                        break;
                    //요검사
                    case "A112":
                        clsHcType.B1.URINE2 = strResult;            //요단백
                        break;
                    //혈액검사
                    case "A121":
                        clsHcType.B1.BLOOD1 = nResult;              //혈색소
                        break;
                    case "A122":
                        clsHcType.B1.BLOOD2 = nResult.To<int>();    //혈당
                        break;
                    case "A123":
                        clsHcType.B1.BLOOD3 = nResult.To<int>();    //총콜레스테롤
                        break;
                    case "A124":
                        clsHcType.B1.BLOOD4 = nResult.To<int>();    //혈청지오티
                        break;
                    case "A125":
                        clsHcType.B1.BLOOD5 = nResult.To<int>();    //혈청지피티
                        break;
                    case "A126":
                        clsHcType.B1.BLOOD6 = nResult.To<int>();    //감마지피티
                        break;
                    //간염검사
                    case "A131":
                        clsHcType.B1.LIVER1 = strResult;            //간염항체
                        break;
                    case "A132":
                        clsHcType.B1.LIVER2 = strResult;            //간염항원
                        break;
                    case "A258":
                        clsHcType.B1.LIVER1 = strResult;            //간염항원 '2009 - 11 - 06 손대영샘 요청 생애만
                        break;
                    case "A259":
                        clsHcType.B1.LIVER2 = strResult;            //간염항체 '2009 - 11 - 06 손대영샘 요청 생애만
                        break;
                    //방사선
                    case "A142":
                        clsHcType.B1.XRayRes = strResult;           //흉부방사선 직접
                        clsHcType.B1.XRayGbn = "4";
                        break;
                    case "A141":
                        clsHcType.B1.XRayRes = strResult;           //흉부방사선 간접
                        clsHcType.B1.XRayGbn = "2";
                        break;
                    //심전도
                    case "A151":
                        clsHcType.B1.EKG = strResult;               //심전도검사
                        break;
                    //생애검진 관련 추가 내역
                    case "A118":
                        clsHcType.B1.FOOT1 = nResult.To<int>();
                        clsHcType.B1.FOOT3 = strResult;             //하지기능장애
                        break;
                    case "A119":
                        clsHcType.B1.FOOT2 = nResult.To<int>();     //보행장애
                        break;
                    case "A120":
                        clsHcType.B1.BALANCE = nResult.To<int>();   //평형성
                        break;
                    case "TX07":
                        clsHcType.B1.OSTEO = nResult.To<int>();     //골밀도 검사
                        break;
                    default:
                        break;
                }

                if (strSelf == "01" || strSelf == "1" || strOK3 == "OK")
                {
                    //1차 검강검진비 청구서 건수 누적
                    fn_Mir_BohumBill_Count("1", strCode, strResult, argWRTNO.To<string>());
                }
            }

            //1차 토.공휴일가산
            if (hicSunapdtlService.GetCountbyWrtNoCode(argWRTNO, "1116") > 0)
            {
                clsHcType.B1.GbGonghu = "Y";
                clsHcType.TMB.ONE_Inwon[23] += 1;
                FnOneCnt[23] = 1;   //토.공휴일 가산
            }

            //간염판정
            if (clsHcType.B1.LIVER1.Length > 1)
            {
                clsHcType.B1.LIVER1 = clsHcType.B1.LIVER1.To<long>().To<string>();
            }

            if (clsHcType.B1.LIVER2.Length > 1)
            {
                clsHcType.B1.LIVER2 = clsHcType.B1.LIVER2.To<long>().To<string>();
            }

            List<HIC_RESULT> list5 = hicResultService.GetWrtNoExCodeResultbyWrtNo(argWRTNO);

            if (list5.Count > 0)
            {
                for (int i = 0; i < list5.Count; i++)
                {
                    if (list5[i].EXCODE == "A131") { strA131 = list5[i].RESULT; }
                    if (list5[i].EXCODE == "A132") { strA132 = list5[i].RESULT; }
                    if (list5[i].EXCODE == "A258") { nGan1 = Regex.Replace(list5[i].RESULT, @"\D", "").To<int>(); }
                    if (list5[i].EXCODE == "A259") { nGan2 = Regex.Replace(list5[i].RESULT, @"\D", "").To<int>(); }
                }
            }

            if (strA132 == "02")
            {
                if (clsHcType.B1.LIVER3 == "02")
                {
                    clsHcType.B1.LIVER3 = "2";  //면역자
                }
                else if (strA132 == "01")
                {
                    if (strA131 == "01")
                    {
                        clsHcType.B1.LIVER3 = "3";
                    }
                    if (strA131 == "02")
                    {
                        clsHcType.B1.LIVER3 = "1";
                    }
                }
                else if (nGan1 > 0 && nGan2 >= 0)
                {
                    if (nGan1 == 1 && nGan2 >= 10)
                    {
                        clsHcType.B1.LIVER3 = "1";
                    }
                    else if (nGan1 == 1 && nGan2 < 10)
                    {
                        clsHcType.B1.LIVER3 = "2";
                    }
                    else if (nGan1 == 2 && nGan2 < 10)
                    {
                        clsHcType.B1.LIVER3 = "3";
                    }
                    else if (nGan1 == 2 && nGan2 >= 10)
                    {
                        clsHcType.B1.LIVER3 = "4";
                    }
                }

                //자료의 오류를 점검함
                if (clsHcType.B1.Height < 100 || clsHcType.B1.Height > 220) { fn_ERROR_INSERT(argWRTNO, "1", "키(신장) 검사결과 오류:" + clsHcType.B1.Height); }
                if (clsHcType.B1.Weight < 20 || clsHcType.B1.Weight > 200) { fn_ERROR_INSERT(argWRTNO, "1", "몸무게 검사결과 오류:" + clsHcType.B1.Weight); }
                if (clsHcType.B1.Waist < 0 || clsHcType.B1.Waist > 300) { fn_ERROR_INSERT(argWRTNO, "1", "허리둘레 검사결과 오류:" + clsHcType.B1.Waist); }

                if ((clsHcType.B1.EYE_L > 2.5 || clsHcType.B1.EYE_L != 9.9) || clsHcType.B1.EYE_L == 0) { fn_ERROR_INSERT(argWRTNO, "1", "시력(좌) 검사결과 오류:" + clsHcType.B1.EYE_L); }
                if ((clsHcType.B1.EYE_R > 2.5 || clsHcType.B1.EYE_R != 9.9) || clsHcType.B1.EYE_R == 0) { fn_ERROR_INSERT(argWRTNO, "1", "시력(우) 검사결과 오류:" + clsHcType.B1.EYE_R); }

                //청력
                if (clsHcType.B1.EAR_L != "1" && clsHcType.B1.EAR_L != "2") { fn_ERROR_INSERT(argWRTNO, "1", "청력(좌) 검사결과 오류:" + clsHcType.B1.EAR_L); }
                if (clsHcType.B1.EAR_R != "1" && clsHcType.B1.EAR_R != "2") { fn_ERROR_INSERT(argWRTNO, "1", "청력(우) 검사결과 오류:" + clsHcType.B1.EAR_R); }

                //혈압
                if (strCHK1.IsNullOrEmpty())
                {
                    if (clsHcType.B1.BLOOD_H < 40 || clsHcType.B1.BLOOD_H > 300) { fn_ERROR_INSERT(argWRTNO, "1", "혈압(최고) 검사결과 오류:" + clsHcType.B1.BLOOD_H); }
                    if (clsHcType.B1.BLOOD_L < 40 || clsHcType.B1.BLOOD_L > 300) { fn_ERROR_INSERT(argWRTNO, "1", "혈압(최저) 검사결과 오류:" + clsHcType.B1.BLOOD_L); }
                }

                //하지기능장애
                if (clsHcType.B1.FOOT1 < 0 && clsHcType.B1.FOOT1 > 99) { fn_ERROR_INSERT(argWRTNO, "1", "하지기능장애(A118) 검사결과 오류:" + clsHcType.B1.FOOT1); }
                if (clsHcType.B1.FOOT3.Length > 2) { fn_ERROR_INSERT(argWRTNO, "1", "하지기능장애(A118) 검사결과 오류:" + clsHcType.B1.FOOT3); }
                if (clsHcType.B1.BALANCE < 0 || clsHcType.B1.BALANCE > 99) { fn_ERROR_INSERT(argWRTNO, "1", "하지기능장애(A120) 검사결과 오류:" + clsHcType.B1.FOOT3); }
                if (clsHcType.B1.FOOT1 > 19 && clsHcType.B1.FOOT2 == "01".To<int>()) { fn_ERROR_INSERT(argWRTNO, "1", "하지기능장애 결과값 19이상인데 보행장애 이상 무:" + clsHcType.B1.FOOT1); }

                //2018 - 07 - 16(임시설정 주석처리)
                //If b1.LIVER1 <> "" And b1.LIVER2 <> "") Call ERROR_INSERT(ArgWRTNO, "1", "B형간염대상자 임시제외")
                //If strLife2 = "OK") Call ERROR_INSERT(ArgWRTNO, "1", "생활습관음주 임시제외")

                //결과를 DB에 저장

                HIC_RES_BOHUM1 item = new HIC_RES_BOHUM1();

                item.HEIGHT = clsHcType.B1.Height;
                item.WEIGHT = clsHcType.B1.Weight;
                item.WAIST = clsHcType.B1.Waist;
                item.BIMAN = clsHcType.B1.Biman;
                item.EYE_L = clsHcType.B1.EYE_L;
                item.EYE_R = clsHcType.B1.EYE_R;
                item.EAR_L = clsHcType.B1.EAR_L;
                item.EAR_R = clsHcType.B1.EAR_R;
                item.BLOOD_H = clsHcType.B1.BLOOD_H;
                item.BLOOD_L = clsHcType.B1.BLOOD_L;
                item.URINE1 = clsHcType.B1.URINE1;
                item.URINE2 = clsHcType.B1.URINE2;
                item.URINE3 = clsHcType.B1.URINE3;
                item.URINE4 = clsHcType.B1.URINE4;
                item.BLOOD1 = clsHcType.B1.BLOOD1;
                item.BLOOD2 = clsHcType.B1.BLOOD2;
                item.BLOOD3 = clsHcType.B1.BLOOD3;
                item.BLOOD4 = clsHcType.B1.BLOOD4;
                item.BLOOD5 = clsHcType.B1.BLOOD5;
                item.BLOOD6 = clsHcType.B1.BLOOD6;
                item.LIVER1 = clsHcType.B1.LIVER1;
                item.LIVER2 = clsHcType.B1.LIVER2;
                item.LIVER3 = clsHcType.B1.LIVER3;
                item.XRAYGBN = clsHcType.B1.XRayGbn;
                item.XRAYRES = clsHcType.B1.XRayRes;
                item.FOOT1 = clsHcType.B1.FOOT1.To<string>();
                item.FOOT2 = clsHcType.B1.FOOT2.To<string>();
                item.BALANCE = clsHcType.B1.BALANCE.To<string>();
                item.OSTEO = clsHcType.B1.OSTEO.To<string>();
                item.EKG = clsHcType.B1.EKG;
                item.GBGONGHU = clsHcType.B1.GbGonghu;
                item.WRTNO = clsHcType.B1.WRTNO;

                result = hicResBohum1Service.UpdateAll(item);

                if (result < 0)
                {
                    FstrCOMMIT = "NO";
                    //clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show(" 접수번호 검사결과 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        void fn_Check_Bohum2(long argMirno, string argFrDate, string argToDate)
        {
            int nREAD = 0;
            long nWRTNO = 0;
            long nPano = 0;
            string strJepDate = "";
            string strGbChul = "";
            bool bGonghyu = false;
            int nGonghyuCnt = 0;

            FnMirNo = argMirno; //청구번호

            clsDB.setBeginTran(clsDB.DbCon);

            //건진2차 청구오류 점검
            List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDateMriNo(argFrDate, argToDate, argMirno);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                nWRTNO = list[i].WRTNO;
                FstrSname = list[i].SNAME;
                FstrSex = list[i].SEX;
                FnAge = list[i].AGE;
                strJepDate = list[i].JEPDATE;
                strGbChul = list[i].GBCHUL;

                //검진2차 검사결과를 HIC_RES_BOHUM2에 UPDATE
                fn_RESULT_Bohum2_Update(nWRTNO);

                FnTwoCnt[1] = 1;    //2차 (TMB.TWO_Inwon(1))건수는 2010-11-08

                FstrChasu = "2";
                fn_One_Mir_Sunap(nWRTNO);
                FstrChasu = "";

                //검진결과,문진표를 읽음
                chb.READ_HIC_RES_BOHUM2(nWRTNO);
                if (!clsHcType.B2.ROWID.IsNullOrEmpty())
                {
                    //GoSub Check_Bohum2_ERROR_Check
                    fn_Check_Bohum2_ERROR_Check(nWRTNO, strJepDate, strGbChul);
                }

                if (FstrCOMMIT == "NO")
                {
                    break;
                }
            }

            txtGongWon.Text = Fn절사금액합계1.To<string>();

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void fn_RESULT_Bohum2_Update(long argWrtNo)
        {
            int nREAD = 0;
            string strCode = "";
            string strResult = "";
            double nResult = 0;
            string strOK = "";
            string str생애소견1차 = "";
            int result = 0;

            //검사결과가 저장될 변수를 Clear
            clsHcType.B2.Chest1 = "";       clsHcType.B2.Chest2 = "";    clsHcType.B2.Chest3 = "";       clsHcType.B2.Chest_RES = "";
            clsHcType.B2.Cycle1 = "";       clsHcType.B2.Cycle2 = "";    clsHcType.B2.Cycle3 = "";       clsHcType.B2.Cycle4 = "";  clsHcType.B2.Cycle_RES = "";  clsHcType.B2.Goji1 = "";
            clsHcType.B2.Goji2 = "";        clsHcType.B2.Goji_RES = "";  clsHcType.B2.Liver11 = "";      clsHcType.B2.Liver12 = ""; clsHcType.B2.Liver13 = "";    clsHcType.B2.Liver14 = "";
            clsHcType.B2.Liver15 = "";      clsHcType.B2.Liver16 = "";   clsHcType.B2.Liver17 = "";      clsHcType.B2.Liver18 = ""; clsHcType.B2.Liver19 = "";    clsHcType.B2.Liver20 = "";
            clsHcType.B2.Liver21 = "";      clsHcType.B2.Liver22 = "";
            clsHcType.B2.Liver_Res = "";    clsHcType.B2.Kidney1 = "";   clsHcType.B2.Kidney2 = "";      clsHcType.B2.Kidney3 = ""; clsHcType.B2.Kidney4 = "";    clsHcType.B2.Kidney5 = "";
            clsHcType.B2.Kidney_Res = "";   clsHcType.B2.Anemia1 = "";   clsHcType.B2.Anemia2 = "";      clsHcType.B2.Anemia3 = ""; clsHcType.B2.Anemia_Res = ""; clsHcType.B2.Diabetes1 = "";
            clsHcType.B2.Diabetes2 = "";    clsHcType.B2.Diabetes3 = ""; clsHcType.B2.Diabetes_Res = ""; clsHcType.B2.GbGonghu = "";

            strOK = "";

            //접수번호별 검사결과를 읽어 변수에 저장함
            List<HIC_RESULT> list = hicResultService.GetExCodeREsultbyWrtNoExCodeNotLike(argWrtNo);

            nREAD = list.Count;
            clsHcType.TMB.TWO_Inwon[1] += 1;  //2차 상담 인원 result 없어도 카운트함

            for (int i = 0; i < nREAD; i++)
            {
                strCode = list[i].EXCODE;
                strResult = list[i].RESULT;
                nResult = strResult.To<double>();

                switch (strCode)
                {
                    case "A142":    //흉부방사선검사(직접)
                        if (strResult == "미실시" || strResult == "." || strResult.IsNullOrEmpty())
                        {
                            clsHcType.B2.Chest1 = " ";
                        }
                        else
                        {
                            clsHcType.B2.Chest1 = strResult;
                        }
                        break;
                    case "A231":    //순환기계(혈압최고)
                    case "A108":
                        clsHcType.B2.Cycle1 = strResult;
                        break;
                    case "A232":    //순환기계(혈압최저)
                    case "A109":
                        clsHcType.B2.Cycle1 = strResult;
                        break;
                    case "A233":
                        if (strResult == "미실시" || strResult == "." || strResult.IsNullOrEmpty())
                        {
                            clsHcType.B2.Cycle3 = "";
                        }
                        else
                        {
                            clsHcType.B2.Cycle3 = VB.Right(strResult, 1);   //혈압정밀안저검사
                        }
                        break;
                    case "A151":
                        clsHcType.B2.Cycle4 = VB.Right(strResult, 1);      //심전도
                        if (strResult == "미실시" || strResult == "." || strResult.IsNullOrEmpty())
                        {
                            clsHcType.B2.Cycle4 = "";
                        }
                        else
                        {
                            clsHcType.B2.Cycle4 = VB.Right(strResult, 1);   //혈압정밀안저검사
                        }
                        break;
                    case "A262":
                        if (strResult == "미실시")
                        {
                            clsHcType.B2.Diabetes1 = " ";
                        }
                        else
                        {
                            clsHcType.B2.Diabetes1 = strResult;             //식후(혈당최고)
                        }
                        break;
                    case "A261":
                    case "A148":
                        if (strResult == "미실시")
                        {
                            clsHcType.B2.Diabetes2 = " ";
                        }
                        else
                        {
                            clsHcType.B2.Diabetes2 = strResult;             //식전(혈당최저)
                        }
                        break;
                    case "A263":
                        if (strResult == "미실시")
                        {
                            clsHcType.B2.Diabetes3 = " ";
                        }
                        else
                        {
                            clsHcType.B2.Diabetes3 = VB.Right(strResult, 1);  //당뇨정밀안저검사
                        }
                        break;
                    default:
                        break;
                }

                //2차 검강검진비 청구서 건수 누적
                fn_Mir_BohumBill_Count("2", strCode, strResult, argWrtNo.To<string>());
            }

            //2차 토.공휴일가산
            if (hicSunapdtlService.GetCountbyWrtNoCode(argWrtNo, "1117") > 0)
            {
                clsHcType.B2.GbGonghu = "Y";
                //토.공휴일 가산료
                clsHcType.TMB.TWO_Inwon[13] += 1;
                FnTwoCnt[13] = 1;
            }

            //2차 생활습관검사

            string[] sCode = { "4402", "4502", "4602" };
            if (clsHcType.TMB.Life_Gbn == "Y")
            {
                if (hicSunapdtlService.GetCountbyWrtNoInCode(argWrtNo, sCode) > 0)
                {
                    clsHcType.TMB.TWO_Inwon[4] += 1;
                    FnTwoCnt[4] = 1;
                }
            }

            //생애2차만 - 1차소견 2차에 넣어줌
            str생애소견1차 = "";
            if (clsHcType.TMB.Life_Gbn == "Y")
            {
                //2차번호로 1차 읽어 판정읽음
                str생애소견1차 = chb.READ_HIC_Life1thjudgment(argWrtNo);
                if (!str생애소견1차.IsNullOrEmpty())
                {
                    str생애소견1차 = VB.Left(str생애소견1차, 300);
                    str생애소견1차 = str생애소견1차.Replace("\r\n", " "); //생애1차소견
                }
            }

            //결과를 DB에 저장
            HIC_RES_BOHUM2 item = new HIC_RES_BOHUM2();

            item.CYCLE1 = clsHcType.B2.Cycle1;
            item.CYCLE2 = clsHcType.B2.Cycle2;
            item.CYCLE3 = clsHcType.B2.Cycle3;
            item.CYCLE4 = clsHcType.B2.Cycle4;

            if (!str생애소견1차.IsNullOrEmpty() && clsHcType.TMB.Life_Gbn == "Y")
            {
                item.T_SANGDAM_1 = str생애소견1차;
            }

            item.DIABETES1 = clsHcType.B2.Diabetes1;
            item.DIABETES2 = clsHcType.B2.Diabetes2;
            item.DIABETES3 = clsHcType.B2.Diabetes3;

            item.GBGONGHU = clsHcType.B2.GbGonghu;
            item.WRTNO = argWrtNo;

            result = hicResBohum2Service.UpdateCycleDiabetesbyWrtNo(item);

            if (result < 0)
            {
                FstrCOMMIT = "NO";
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(argWrtNo + " 접수번호 검사결과 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsHcType.B2.Liver18 = "";
            clsHcType.B2.Liver19 = "";
            clsHcType.B2.Liver20 = "";
        }

        /// <summary>
        /// 오류점검
        /// </summary>
        /// <param name="argWrtNo"></param>
        /// <param name="argJepDate"></param>
        /// <param name="argGbChul"></param>
        void fn_Check_Bohum2_ERROR_Check(long argWrtNo, string argJepDate, string argGbChul)
        {
            int nCntB = 0;
            int nCntR = 0;

            if (clsHcType.B2.TongboDate.IsNullOrEmpty())
            {
                fn_ERROR_INSERT(argWrtNo, "2", "통보일자가 공란입니다.");
            }

            if (string.Compare(clsHcType.B2.TongboGbn, "1") < 0 || string.Compare(clsHcType.B2.TongboGbn, "3") > 0)
            {
                fn_ERROR_INSERT(argWrtNo, "2", "통보방법이 오류입니다." + clsHcType.B2.TongboGbn);
            }

            //고혈압
            if (clsHcType.B2.GbCycle == "Y")
            {
                if (clsHcType.B2.Cycle_RES.IsNullOrEmpty())
                {
                    fn_ERROR_INSERT(argWrtNo, "2", "고혈압 판정값 공란..");
                }
                if (string.Compare(clsHcType.B2.Cycle_RES, "1") < 0 || string.Compare(clsHcType.B2.Cycle_RES, "3") > 0)
                {
                    fn_ERROR_INSERT(argWrtNo, "2", "고혈압(소견 1~3까지만) 오류입니다.");
                }
                if (string.Compare(clsHcType.B2.Cycle1, "200") > 0 && !clsHcType.B2.Cycle1.IsNullOrEmpty())
                {
                    fn_ERROR_INSERT(argWrtNo, "2", "혈압 오류입니다.");
                }
                if (string.Compare(clsHcType.B2.Cycle2, "40") < 0 && !clsHcType.B2.Cycle2.IsNullOrEmpty())
                {
                    fn_ERROR_INSERT(argWrtNo, "2", "혈압 오류입니다.");
                }

                if (string.Compare(clsHcType.B2.CYCLE_RES_CARE, "1") < 0 || string.Compare(clsHcType.B2.CYCLE_RES_CARE, "3") > 0)
                {
                    fn_ERROR_INSERT(argWrtNo, "2", "고혈압(치료계획 1~3까지만) 오류입니다.");
                }
            }

            //당뇨질환
            if (clsHcType.B2.GbDiabetes == "Y")
            {
                if (clsHcType.B2.Diabetes_Res.IsNullOrEmpty())
                {
                    fn_ERROR_INSERT(argWrtNo, "2", "당뇨질환판정 공란..");
                }
                if (string.Compare(clsHcType.B2.Diabetes_Res, "1") < 0 || string.Compare(clsHcType.B2.Diabetes_Res, "3") > 0)
                {
                    fn_ERROR_INSERT(argWrtNo, "2", "당뇨질환(소견 1~3까지만) 오류입니다.");
                }
                if (string.Compare(clsHcType.B2.DIABETES_RES_CARE, "1") < 0 || string.Compare(clsHcType.B2.DIABETES_RES_CARE, "3") > 0)
                {
                    fn_ERROR_INSERT(argWrtNo, "2", "당뇨질환(치료계획 1~3까지만) 오류입니다.");
                }
                if (clsHcType.B2.Diabetes2 != "")
                {
                    if (string.Compare(clsHcType.B2.Diabetes2, "0") < 0 || string.Compare(clsHcType.B2.Diabetes2, "999") > 0)
                    {
                        fn_ERROR_INSERT(argWrtNo, "2", "당뇨질환(혈당) 오류입니다.");
                    }
                }
                else if (clsHcType.B2.Diabetes2.IsNullOrEmpty())
                {
                    fn_ERROR_INSERT(argWrtNo, "2", "당뇨질환(혈당)값 공란 오류입니다.");
                }
            }

            //권고사항
            if (clsHcType.B2.Sogen.IsNullOrEmpty())
            {
                fn_ERROR_INSERT(argWrtNo, "2", "2차판정 권고사항공란 오류입니다.");
            }
            else
            {
                if (VB.L(clsHcType.B2.Sogen, "\r\n") > 1)
                {
                    clsHcType.B2.Sogen = clsHcType.B2.Sogen.Replace("\r\n", "");
                }
                if (VB.L(clsHcType.B2.Sogen, "\n") > 1)
                {
                    clsHcType.B2.Sogen = clsHcType.B2.Sogen.Replace("\n", "");
                }
                if (VB.L(clsHcType.B2.Sogen, "\r") > 1)
                {
                    clsHcType.B2.Sogen = clsHcType.B2.Sogen.Replace("\r", "");
                }

                if (clsHcType.B2.Sogen.IsNullOrEmpty())
                {
                    fn_ERROR_INSERT(argWrtNo, "2", "2차판정 권고사항공란 오류입니다.");
                }
            }

            if (clsHcType.TMB.Life_Gbn == "Y" && clsHcType.B2.T_SangDam_1.IsNullOrEmpty())
            {
                fn_ERROR_INSERT(argWrtNo, "2", "2차판정 권고사항공란 오류입니다.");
            }

            //중복판정점검
            if (chb.READ_HIC_DuplicatejudgmentCheck("일반", "2", argWrtNo) == true)
            {
                fn_ERROR_INSERT(argWrtNo, "2", "판정이 2건 발생함 .. 확인바람");
            }

            //공휴가산 점검
            FbGonghyu = false;
            if (hb.HIC_Huil_GasanDay(argJepDate) == true)
            {
                FbGonghyu = true;
            }
            if (argGbChul == "Y")
            {
                FbGonghyu = false;
            }

            FnGonghyuCnt = hicSunapdtlService.GetCountbyWrtNoCode(argWrtNo, "1117");

            if (FbGonghyu == true && FnGonghyuCnt == 0)
            {
                fn_ERROR_INSERT(argWrtNo, "2", "2차 공휴 가산 코드 누락");
            }
            else if (FbGonghyu == false && FnGonghyuCnt > 0)
            {
                fn_ERROR_INSERT(argWrtNo, "2", "2차 공휴 가산 코드 오류 산정");
            }
        }

        void fn_ERROR_INSERT(long argWrtNo, string argGubun, string argRemark)
        {
            int result = 0;

            if (FstrCOMMIT == "NO")
            {
                return;
            }

            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(argWrtNo);

            FnRow += 1;
            SS2.ActiveSheet.RowCount = FnRow;
            SS2.ActiveSheet.Cells[FnRow - 1, 0].Text = "";
            SS2.ActiveSheet.Cells[FnRow - 1, 1].Text = FnMirNo.To<string>();
            SS2.ActiveSheet.Cells[FnRow - 1, 2].Text = argWrtNo.To<string>();
            SS2.ActiveSheet.Cells[FnRow - 1, 3].Text = list.PANO.To<string>();
            SS2.ActiveSheet.Cells[FnRow - 1, 4].Text = list.JEPDATE;
            SS2.ActiveSheet.Cells[FnRow - 1, 5].Text = list.SNAME;
            SS2.ActiveSheet.Cells[FnRow - 1, 6].Text = list.AGE + "/" + list.SEX;
            SS2.ActiveSheet.Cells[FnRow - 1, 7].Text = VB.Left(argRemark, 100);

            //오류내역을 DB에 등록
            result = comHpcLibBService.InsertHicMirError(FnMirNo, argWrtNo, argGubun, FstrSname, FstrSex, FnAge, VB.Left(argRemark.Replace("'", "`"), 50));

            if (result < 0)
            {
                FstrCOMMIT = "NO";
                MessageBox.Show(argWrtNo + " 오류내역 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        void fn_One_Mir_Sunap(long argWRTNO)
        {
            long nJohapAmt = 0;
            long nChaAmt = 0;
            string strDent = "";
            string strDent1 = "";
            string strHuDent = "";
            long nTemp = 0;
            string strJepDate = "";

            if (chkchk.Checked == true)
            {
                return;
            }

            if (FstrChasu != "2")
            {
                for (int j = 1; j <= 23; j++)
                {
                    FnOneCnt[j] += FnOneAmt[j];
                }

                FnAmt1 = FnOneCnt[1] + FnOneCnt[2] + FnOneCnt[3] + FnOneCnt[4] + FnOneCnt[5] + FnOneCnt[6] + FnOneCnt[7] + FnOneCnt[8] + FnOneCnt[9] + FnOneCnt[10];
                FnAmt1 = FnAmt1 + FnOneCnt[11] + FnOneCnt[12] + FnOneCnt[13] + FnOneCnt[14] + FnOneCnt[15] + FnOneCnt[16] + FnOneCnt[17] + FnOneCnt[21] + FnOneCnt[22] + FnOneCnt[23];

                //2차한명 금액 계산
                for (int j = 1; j < 14; j++)
                {
                    FnTwoCnt[j] += FnTwoAmt[j];
                }

                FnAmt1 = FnAmt1 + FnTwoCnt[1] + FnTwoCnt[2] + FnTwoCnt[3] + FnTwoCnt[4] + FnTwoCnt[5] + FnTwoCnt[6];
                FnAmt1 = FnAmt1 + FnTwoCnt[7] + FnTwoCnt[8] + FnTwoCnt[9] + FnTwoCnt[10] + FnTwoCnt[11] + FnTwoCnt[12] + FnTwoCnt[13] + FnTwoCnt[14];
            }

            //구강이 있을경우
            strHuDent = "";
            if (strDent == "Y")
            {
                if (hicSunapdtlService.GetCountbyWrtNoCode(argWRTNO, "1118") >= 1)
                {
                    strHuDent = "Y";
                }
            }

            strDent1 = "";
            if (hicSunapdtlService.GetCountbyWrtNoCode(argWRTNO, "1158") >= 1)
            {
                strDent1 = "Y";
            }

            strJepDate = cboYear.Text + "-01-01";

            HIC_JEPSU_SUNAP list = hicJepsuSunapService.GetItembyJepDateWrtNo(argWRTNO, strJepDate);

            if (list.IsNullOrEmpty())
            {
                if (list.GJCHASU == "1")
                {
                    if (strDent == "Y")
                    {
                        nJohapAmt = list.JOHAPAMT - clsHcVariable.GnDentAmt;
                        if (strHuDent == "Y")
                        {
                            nJohapAmt -= clsHcVariable.GnDentAddAmt;
                        }
                    }
                    else
                    {
                        nJohapAmt = list.JOHAPAMT;
                    }

                    if (strDent1 == "Y")
                    {
                        nJohapAmt -= 3000;
                    }

                    nChaAmt = FnAmt1 - nJohapAmt;

                    Fn절사금액합계1 += nChaAmt;

                    if (FnAmt1 != nJohapAmt)
                    {
                        if (list.GJCHASU == "1")
                        {
                            fn_ERROR_INSERT(argWRTNO, "2", nChaAmt + "￦ 1차 청구비용 차액발생");
                        }
                        else
                        {
                            fn_ERROR_INSERT(argWRTNO, "2", nChaAmt + "￦ 2차 청구비용 차액발생");
                        }
                    }

                    //변수 Clear
                    FnAmt1 = 0;
                    nJohapAmt = 0;
                    nChaAmt = 0;

                    for (int i = 0; i <= 50; i++)
                    {
                        FnOneCnt[i] = 0;
                        FnTwoCnt[i] = 0;
                    }
                }
            }
        }
    }
}
