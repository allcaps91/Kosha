using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaJepsuView.cs
/// Description     : 종검접수 List 조회 (예약자조회 포함)
/// Author          : 김민철
/// Create Date     : 2019-09-06
/// Update History  : 
/// </summary>
/// <seealso cref= "Frm접수명단관리(HaMain11_1.frm) / FrmJepsuVIew(HaMain11.frm)" />
namespace HC_Main
{
    public partial class frmHaJepsuView : BaseForm
    {

        clsHaBase       cHB             = null;
        HIC_LTD         LtdHelpItem     = null;

        HicLtdService   hicLtdService   = null;
        HeaJepsuService heaJepsuService = null;
        HeaCodeService  heaCodeService  = null;
        HeaResultService heaResultService = null;
        EndoJupmstService endoJupmstService = null;
        HeaSunapdtlService heaSunapdtlService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;

        public delegate void SetGstrValue(HEA_JEPSU GstrValue);
        public static event SetGstrValue rSetGstrValue;

        public delegate void SetGstrFamValue(HEA_JEPSU GstrFamValue);
        public static event SetGstrFamValue rSetGstrFamValue;

        long nLtdCode = 0;
        string FstrJong = string.Empty;
        string FstrSName = string.Empty;
        string FstrFDate = string.Empty;
        string FstrTDate = string.Empty;
        string FstrGBSTS = string.Empty;
        bool FnPrvcy = false;
        bool bolSort = false;
        bool bShow = false;

        public frmHaJepsuView()
        {
            InitializeComponent();
            SetEvents();
            SetControl();
        }

        public frmHaJepsuView(string argSTS)
        {
            InitializeComponent();
            SetEvents();
            SetControl();
            FstrGBSTS = argSTS;
        }

        private void SetControl()
        {
            cHB             = new clsHaBase();
            LtdHelpItem     = new HIC_LTD();
            hicLtdService   = new HicLtdService();
            heaJepsuService = new HeaJepsuService();
            heaCodeService  = new HeaCodeService();
            heaResultService = new HeaResultService();
            endoJupmstService = new EndoJupmstService();
            heaSunapdtlService = new HeaSunapdtlService();
            hicIeMunjinNewService = new HicIeMunjinNewService();

            cboJong.Items.Clear();
            cboJong.Items.Add("**.전체");
            cHB.ComboJong2_AddItem(cboJong);

            cboExCode.Clear();
            cboExCode.Items.Add("**.전체");

            foreach (HEA_CODE item in heaCodeService.GetGroupNameByGubun("13"))
            {
                cboExCode.Items.Add(item.NAME);
            }

            SSList.Initialize(new SpreadOption { RowHeaderVisible = true, ColumnHeaderHeight = 45 });
            SSList.AddColumnCheckBox("선택", "", 28, new CheckBoxFlagEnumCellType<IsDeleted>() {  });
            SSList.AddColumn("검진일자",         nameof(HEA_JEPSU.SDATE),         84, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("예약시간",         nameof(HEA_JEPSU.STIME),         44, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("접수시간",         nameof(HEA_JEPSU.CDATE),         44, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("VIP",              nameof(HEA_JEPSU.VIPYN),         32, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("구분",             nameof(HEA_JEPSU.AMPM2),         44, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("등록번호",         nameof(HEA_JEPSU.PTNO),          82, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("배우자",           nameof(HEA_JEPSU.FAMILLY),       68, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsVisivle = false });  //Column No 7
            SSList.AddColumn("가족번호",         "",                              44, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("수검자명",         nameof(HEA_JEPSU.SNAME),         78, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("나이",             nameof(HEA_JEPSU.AGESEX),        42, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false }); //Column No 10
            SSList.AddColumn("대장내시경",       nameof(HEA_JEPSU.COLON),         44, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("수검상태",         nameof(HEA_JEPSU.GBSTS_NM),      88, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsVisivle = false });
            SSList.AddColumn("사업장코드",       nameof(HEA_JEPSU.LTDCODE),       94, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("사업장명",         nameof(HEA_JEPSU.LTDNAME),      100, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("검진종류",         nameof(HEA_JEPSU.GJNAME),       140, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("유형",             nameof(HEA_JEPSU.TYPE),          44, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("당일상담",         nameof(HEA_JEPSU.GBDAILY),       34, FpSpreadCellType.CheckBoxCellType,  new SpreadCellTypeOption { TextTrue = "Y", IsVisivle = false });
            SSList.AddColumn("전화상담",         nameof(HEA_JEPSU.GBDAILY2),      34, FpSpreadCellType.CheckBoxCellType,  new SpreadCellTypeOption { TextTrue = "T", IsVisivle = false });
            SSList.AddColumn("내원상담일",       nameof(HEA_JEPSU.IDATE),         84, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsVisivle = false });
            SSList.AddColumn("수납",             nameof(HEA_JEPSU.SUNAP),         44, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });     //  Column No 20
            SSList.AddColumn("전화확인",         nameof(HEA_JEPSU.GUIDETEL),      90, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("문진종류",         nameof(HEA_JEPSU.IEMUNJIN),      84, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("카톡결과",         nameof(HEA_JEPSU.WEBPRINTREQ),   44, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("조직수납",         nameof(HEA_JEPSU.CLOSUNAP),      44, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });   // Column No 24
            SSList.AddColumn("검사변경참고",     nameof(HEA_JEPSU.EXAMCHANGE),   160, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("접수번호",         nameof(HEA_JEPSU.WRTNO),         78, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("검진번호",         nameof(HEA_JEPSU.PANO),          82, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });  //  Column No 27
            SSList.AddColumn("접수일자",         nameof(HEA_JEPSU.JEPDATE),       84, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("도착시간",         nameof(HEA_JEPSU.WAITDATE),      44, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("휴대폰번호",       nameof(HEA_JEPSU.HPHONE),       110, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("주소",             nameof(HEA_JEPSU.JUSO),         180, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("문진번호",         nameof(HEA_JEPSU.IEMUNNO),       44, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("최종작업자",       nameof(HEA_JEPSU.JOBNAME),       82, FpSpreadCellType.TextCellType,      new SpreadCellTypeOption { IsEditble = false });

            UnaryComparisonConditionalFormattingRule unary;

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "Y", false);
            unary.BackColor = Color.Gold;
            SSList.ActiveSheet.SetConditionalFormatting(-1, 4, unary);

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "미납", false);
            unary.BackColor = Color.LightCoral;
            SSList.ActiveSheet.SetConditionalFormatting(-1, 20, unary);

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "○", false);
            unary.BackColor = Color.MistyRose;
            SSList.ActiveSheet.SetConditionalFormatting(-1, 24, unary);

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "(위)조직", false);
            unary.BackColor = Color.LightGreen;
            SSList.ActiveSheet.SetConditionalFormatting(-1, 25, unary);

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "(위)미생물", false);
            unary.BackColor = Color.LightGreen;
            SSList.ActiveSheet.SetConditionalFormatting(-1, 25, unary);

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "(위)조직+미생", false);
            unary.BackColor = Color.LightGreen;
            SSList.ActiveSheet.SetConditionalFormatting(-1, 25, unary);

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "(대장)조직", false);
            unary.BackColor = Color.LightGreen;
            SSList.ActiveSheet.SetConditionalFormatting(-1, 25, unary);

        }

        private void SetEvents()
        {
            this.Load                   += new EventHandler(eFormload);
            this.Activated              += new EventHandler(eActivated);
            //this.FormClosing            += new FormClosingEventHandler(eFormClosing);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click       += new EventHandler(eBtnClick);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.btnSave.Click          += new EventHandler(eBtnClick);
            this.btnPrint.Click         += new EventHandler(eBtnClick);
            this.txtLtdName.KeyPress    += new KeyPressEventHandler(eKeyPress);
            this.SSList.CellClick       += new CellClickEventHandler(eSpdClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
            this.rdoSTS1.CheckedChanged += new EventHandler(eRdoCheckedChange);
            this.rdoSTS2.CheckedChanged += new EventHandler(eRdoCheckedChange);
            this.rdoSTS6.CheckedChanged += new EventHandler(eRdoCheckedChange);
            this.dtpFDate.TextChanged   += new EventHandler(eDtpChanged);
        }

        private void eActivated(object sender, EventArgs e)
        {
            if (bShow) { bShow = false; return; }

            //화면갱신안함
            if (chkResv.Checked)
            {
                cboJong.SelectedIndex = 0;
                cboExCode.SelectedIndex = 0;
                txtLtdName.Text = "";
                txtSName.Text = "";
                chkPrivacy.Checked = false;
                chkDaeSang.Checked = false;
                chkFamilly.Checked = true;

                lblMsg.Text = "";
                rdoSTS2.Checked = true;
                dtpFDate.Text = DateTime.Now.ToShortDateString();
                dtpTDate.Text = DateTime.Now.ToShortDateString();
                Screen_Display();
            }
        }

        private void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(SSList, e.Column, ref bolSort, true);
            }
        }

        private void eDtpChanged(object sender, EventArgs e)
        {
            if (sender == dtpFDate)
            {
                dtpTDate.Value = dtpFDate.Value;
                dtpTDate.Text = dtpFDate.Text;
            }
        }

        private void eRdoCheckedChange(object sender, EventArgs e)
        {
            if (sender == rdoSTS1)
            {
                chkPrivacy.Checked = false;
                chkDaeSang.Checked = false;
                SSList.ActiveSheet.Columns.Get(1).Label = "예약일자";
                SSList.ActiveSheet.Columns.Get(2).Visible = true;
                SSList.ActiveSheet.Columns.Get(3).Visible = false;
                SSList.ActiveSheet.Columns.Get(20).Visible = false;
            }
            else
            {
                SSList.ActiveSheet.Columns.Get(1).Label = "검진일자";
                SSList.ActiveSheet.Columns.Get(2).Visible = false;
                SSList.ActiveSheet.Columns.Get(3).Visible = true;
                SSList.ActiveSheet.Columns.Get(20).Visible = true;
            }

            Screen_Display();
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader) { return; }

            if (clsHcVariable.GbFamilly_PopUp)
            {
                foreach (Form frm2 in Application.OpenForms) //떠있는지 체크
                {
                    if (frm2.Name == "frmHaJepMain")
                    {
                        HEA_JEPSU item = SSList.GetCurrentRowData() as HEA_JEPSU;

                        if (rSetGstrFamValue.IsNullOrEmpty())
                        {
                            this.Hide();
                            return;
                        }
                        else
                        {
                            rSetGstrFamValue(item);
                            this.Hide();
                            return;
                        }
                    }
                }
            }
            else
            {
                foreach (Form frm2 in Application.OpenForms) //떠있는지 체크
                {
                    if (frm2.Name == "frmHaJepMain")
                    {
                        HEA_JEPSU item = SSList.GetCurrentRowData() as HEA_JEPSU;

                        if (rSetGstrValue.IsNullOrEmpty())
                        {
                            this.Hide();
                            return;
                        }
                        else
                        {
                            rSetGstrValue(item);
                            this.Hide();
                            return;
                        }
                    }
                }
            }
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(); }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display();
            }
            else if (sender == btnExit)
            {
                this.Hide();
                return;
            }
            #region 사업장코드찾기
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
            }
            #endregion
            else if (sender == btnSave)
            {
                Data_Save();
            }
            else if (sender == btnPrint)
            {
                //선택된 수검자 접수증 출력
                Print_Jepsu_Reciept();
            }
        }

        private void Print_Jepsu_Reciept()
        {
            
        }

        private void Data_Save()
        {
            IList<HEA_JEPSU> list = SSList.GetEditbleData<HEA_JEPSU>();

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HEA_JEPSU code in list)
                {
                    if (code.RowStatus == RowStatus.Update)
                    {
                        if (code.GBDAILY2 == "T")
                        {
                            code.GBDAILY = "T";
                        }

                        if (!code.IDATE.IsNullOrEmpty())
                        {
                            code.IDATE = VB.Left(code.IDATE, 10);
                        }

                        heaJepsuService.UpdateJepsuInfo(code);
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                return;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        /// <summary>
        /// 사업장 코드 검색창 연동
        /// </summary>
        private void Ltd_Code_Help()
        {
            string strFind = "";

            if (txtLtdName.Text.Contains("."))
            {
                strFind = VB.Pstr(txtLtdName.Text, ".", 2).Trim();
            }
            else
            {
                strFind = txtLtdName.Text.Trim();
            }

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind);
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();
            frm.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);

            if (LtdHelpItem.CODE > 0 && !LtdHelpItem.IsNullOrEmpty())
            {
                txtLtdName.Text = LtdHelpItem.CODE.To<string>();
                txtLtdName.Text += "." + LtdHelpItem.SANGHO;
            }
            else
            {
                if (VB.Pstr(txtLtdName.Text, ",", 1).Trim() == "")
                {
                    txtLtdName.Text = "";
                }
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void Screen_Display()
        {
            SSList.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            FstrFDate = dtpFDate.Value.ToShortDateString();
            FstrTDate = dtpTDate.Value.ToShortDateString();
            FstrJong = VB.Left(cboJong.Text, 2);
            FstrSName = txtSName.Text.Trim();
            nLtdCode = VB.Pstr(txtLtdName.Text, ".", 1).To<long>();
            FnPrvcy = chkPrivacy.Checked;

            string strOK = string.Empty;
            string strTempRowChk = string.Empty;
            string strAnatFlag = string.Empty;
            string strFlag = string.Empty;
            string strRecvForm = string.Empty;
            string strWRTNO = string.Empty;
            string strGbSts = string.Empty;

            if (rdoSTS1.Checked) { strGbSts = "예약"; }
            else if (rdoSTS2.Checked) { strGbSts = "접수"; }
            else if (rdoSTS6.Checked) { strGbSts = "D"; }

            if (strGbSts.IsNullOrEmpty()) { strGbSts = "접수"; }
            if (!FstrGBSTS.IsNullOrEmpty()) { strGbSts = FstrGBSTS; FstrGBSTS = ""; }

            int nCNT = 0;

            List<string> lstExCodes = new List<string>();

            try
            {
                List<HEA_JEPSU> list = heaJepsuService.GetListByItems(FstrFDate, FstrTDate, strGbSts, FstrJong, FstrSName, nLtdCode, FnPrvcy);

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].SEX == "M") { nCNT += 1; }

                        strOK = "OK";

                        if (VB.Left(cboExCode.Text, 2) != "**" && VB.Left(cboExCode.Text, 2) != "")
                        {
                            strOK = ""; lstExCodes.Clear();

                            List<HEA_CODE> lstHaCD = heaCodeService.GetListByGubunName("13", cboExCode.Text.Trim());
                            if (lstHaCD.Count > 0)
                            {
                                for (int j = 0; j < lstHaCD.Count; j++)
                                {
                                    lstExCodes.Add(lstHaCD[j].CODE);
                                }
                            }

                            strTempRowChk = heaResultService.GetRowidByWrtnoExCodeIN(list[i].WRTNO, lstExCodes);
                            if (!strTempRowChk.IsNullOrEmpty()) { strOK = "OK"; }
                        }

                        if (chkDaeSang.Checked)
                        {
                            strOK = "OK";
                            List<ENDO_JUPMST> lstEndo = endoJupmstService.GetItembyPtNoBDate(list[i].PTNO, list[i].SDATE);
                            if (lstEndo.Count > 0)
                            {
                                strFlag = "";

                                for (int j = 0; j < lstEndo.Count; j++)
                                {
                                    strOK = "OK";

                                    if (strFlag == "")
                                    {
                                        if (!lstEndo[j].SUCODE.IsNullOrEmpty())
                                        {
                                            switch (lstEndo[j].SUCODE.To<string>().Trim())
                                            {
                                                case "TX24":     strFlag = "1"; break;
                                                case "TX25":     strFlag = "2"; break;
                                                case "TX24TX25": strFlag = "3"; break;
                                                case "TX21":     strFlag = "4"; break;
                                                default: break;
                                            }
                                        }
                                    }
                                }

                                //위, 대장 내시경유무
                                lstExCodes.Clear();
                                lstExCodes.Add("TX20"); lstExCodes.Add("TX23"); lstExCodes.Add("TX21");
                                lstExCodes.Add("TX32"); lstExCodes.Add("TX41"); lstExCodes.Add("TX64");

                                strTempRowChk = heaResultService.GetRowidByWrtnoExCodeIN(list[i].WRTNO, lstExCodes);
                                if (!strTempRowChk.IsNullOrEmpty())
                                {
                                    strOK = "OK";
                                }
                                else
                                {
                                    strOK = "";
                                }

                                //조직검사유무
                                lstExCodes.Clear();
                                lstExCodes.Add("TX19"); lstExCodes.Add("TX24"); lstExCodes.Add("TX21"); lstExCodes.Add("TX25");

                                strTempRowChk = heaResultService.GetRowidByWrtnoExCodeIN(list[i].WRTNO, lstExCodes);
                                if (!strTempRowChk.IsNullOrEmpty())
                                {
                                    if (strFlag == "") { strAnatFlag = "OK"; }
                                }
                                else
                                {
                                    strAnatFlag = "";
                                }
                            }
                            else
                            {
                                strOK = ""; strFlag = "";
                            }       
                        }

                        //2016-07-20 명품검진,골드검진,VIP검진 표시
                        if (list[i].GJJONG == "11" || list[i].GJJONG == "12")
                        {
                            strTempRowChk = heaSunapdtlService.CheckVipByWRTNOLikeCodeName(list[i].WRTNO);
                            if (!strTempRowChk.IsNullOrEmpty())
                            {
                                list[i].VIPYN = "Y";
                            }
                        }

                        if (heaSunapdtlService.GetSumAmtByWRTNO(list[i].WRTNO) > 1500000)
                        {
                            list[i].VIPYN = "Y";
                        }

                        list[i].TYPE = heaSunapdtlService.GetCodeTypeByWrtno(list[i].WRTNO);

                        if (list[i].TYPE.To<string>("").Trim() != "")
                        {
                            list[i].TYPE += "형";
                        }

                        list[i].GJNAME = heaSunapdtlService.GetMainSunapDtlCodeNameByWrtno(list[i].WRTNO);

                        if (strFlag != "")
                        {
                            switch (strFlag)
                            {
                                case "1": list[i].EXAMCHANGE = "(위)조직";      break;
                                case "2": list[i].EXAMCHANGE = "(위)미생물";    break;
                                case "3": list[i].EXAMCHANGE = "(위)조직+미생"; break;
                                case "4": list[i].EXAMCHANGE = "(대장)조직";    break;
                                default: break;
                            }

                            //조직검사 수납유무
                            lstExCodes.Clear();
                            lstExCodes.Add("Z5261"); lstExCodes.Add("Z5262"); lstExCodes.Add("Z5263");

                            strTempRowChk = heaSunapdtlService.GetRowidByWrtnoCodeIN(list[i].WRTNO, lstExCodes);

                            if (!strTempRowChk.IsNullOrEmpty())
                            {
                                list[i].CLOSUNAP = "○";
                            }
                        }

                        if (strAnatFlag == "OK" && strFlag == "")
                        {
                            list[i].EXAMCHANGE = "조직검사 없는데 조직검사 접수됨";
                        }

                        //대장내시경 유무
                        lstExCodes.Clear();
                        lstExCodes.Add("TX32"); lstExCodes.Add("TX64"); lstExCodes.Add("TX41"); 

                        strTempRowChk = heaResultService.GetRowidByWrtnoExCodeIN(list[i].WRTNO, lstExCodes);

                        if (!strTempRowChk.IsNullOrEmpty())
                        {
                            list[i].COLON = "○";
                        }

                        if (list[i].IEMUNNO > 0)
                        {
                            strRecvForm = hicIeMunjinNewService.GetResvFormByWrtno(list[i].IEMUNNO);
                            list[i].IEMUNJIN = cHB.IEMunjin_Name_Display(strRecvForm);
                        }
                        else
                        {
                            strRecvForm = hicIeMunjinNewService.GetResvFormByPtno(list[i].PTNO, DateTime.Now.AddDays(-60).ToShortDateString());
                            list[i].IEMUNJIN = cHB.IEMunjin_Name_Display(strRecvForm);
                        }

                        if (strOK != "OK")
                        {
                            list[i].RowStatus = RowStatus.Delete;
                        }
                    }
                }

                Delete_List(ref list);

                SSList.SetDataSource(list);

                lblMsg.Text = "총인원 : " + list.Count + " 명 , 남 : " + nCNT + " 명 , 여 : " + (list.Count - nCNT) + " 명 ";

                int nFamNo = 0;

                if (chkFamilly.Checked)
                {
                    for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                    {
                        strOK = "";
                        strWRTNO = SSList.ActiveSheet.Cells[i, 27].Text.Trim();
                        strOK = SSList.ActiveSheet.Cells[i, 8].Text.Trim();

                        if (strWRTNO != "" && strOK == "")
                        {
                            for (int j = 0; j < SSList.ActiveSheet.RowCount; j++)
                            {
                                if (strWRTNO == SSList.ActiveSheet.Cells[j, 7].Text.Trim())
                                {
                                    nFamNo += 1;
                                    SSList.ActiveSheet.Cells[i, 8].Text = nFamNo.ToString();
                                    SSList.ActiveSheet.Cells[j, 8].Text = nFamNo.ToString();
                                    SSList.ActiveSheet.Cells[i, 8].BackColor = Color.Wheat;
                                    SSList.ActiveSheet.Cells[j, 8].BackColor = Color.Wheat;
                                    
                                    break;
                                }
                            }
                        }
                    }

                }

                if (chkDaeSang.Checked) { clsSpread.gSpdSortRow(SSList, 24, ref bolSort, true); }
                if (rdoSTS2.Checked) { clsSpread.gSpdSortRow(SSList, 3, ref bolSort, true); }

                // VIP , 골드검진
                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (SSList.ActiveSheet.Cells[i, 4].Text == "Y")
                    {
                        SSList.ActiveSheet.Cells[i, 15].BackColor = Color.Gold;
                    }
                }

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void Delete_List(ref List<HEA_JEPSU> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].RowStatus == RowStatus.Delete)
                {
                    list.RemoveAt(i);
                    Delete_List(ref list);
                    break;
                }
            }
        }

        private void eFormload(object sender, EventArgs e)
        {
            bShow = true;
            cboJong.SelectedIndex = 0;
            lblMsg.Text = "";
            rdoSTS2.Checked = true;
            //Screen_Display();
        }

        private void IEMunjin_Name_Display(string ArgRecvForm)
        {
            string strResult = string.Empty;

            if (VB.InStr(ArgRecvForm, "12001") > 0) { strResult += "공통,"; }
            if (VB.InStr(ArgRecvForm, "12003") > 0) { strResult += "암검진,"; }
            if (VB.InStr(ArgRecvForm, "12005") > 0) { strResult += "구강,"; }
            if (VB.InStr(ArgRecvForm, "12006") > 0) { strResult += "생애,"; }
            if (VB.InStr(ArgRecvForm, "12010") > 0) { strResult += "초등학생,"; }
            if (VB.InStr(ArgRecvForm, "12014") > 0) { strResult += "학생구강,"; }
            if (VB.InStr(ArgRecvForm, "12020") > 0) { strResult += "중고등학생,"; }
            if (VB.InStr(ArgRecvForm, "20002") > 0) { strResult += "특수,"; }
            if (VB.InStr(ArgRecvForm, "20003") > 0) { strResult += "폐활량,"; }
            if (VB.InStr(ArgRecvForm, "20004") > 0) { strResult += "흡연,"; }
            if (VB.InStr(ArgRecvForm, "20005") > 0) { strResult += "음주,"; }
            if (VB.InStr(ArgRecvForm, "20006") > 0) { strResult += "운동,"; }
            if (VB.InStr(ArgRecvForm, "20007") > 0) { strResult += "영양,"; }
            if (VB.InStr(ArgRecvForm, "20008") > 0) { strResult += "비만,"; }
            if (VB.InStr(ArgRecvForm, "20009") > 0) { strResult += "만40우울,"; }
            if (VB.InStr(ArgRecvForm, "20010") > 0) { strResult += "만66우울,"; }
            if (VB.InStr(ArgRecvForm, "20011") > 0) { strResult += "인지기능,"; }
            if (VB.InStr(ArgRecvForm, "30001") > 0) { strResult += "야간1차,"; }
            if (VB.InStr(ArgRecvForm, "30003") > 0) { strResult += "야간2차,"; }

            if (strResult != "")
            {
                lblMsg.Text = "▶인터넷문진: " + strResult;
            }
        }
    }
}
