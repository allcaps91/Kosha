using System;
using ComBase;
using ComHpcLibB;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComDbB;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using FarPoint.Win.Spread;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;

namespace HC_Pan
{
    public partial class frmHcPanPatList : Form
    {
        HcPanjengPatlistService hcPanjengPatlistService = null;
        HicResSahusangdamService hicResSahusangdamService = null;
        HicSpcPanjengService hicSpcPanjengService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicSunapdtlService hicSunapdtlService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread cSpd = null;
        clsHcMain cHM = null;
        clsHaBase cHB = null;
        clsHcFunc cHF = null;
        ComFunc CF = null;

        string FstrJob = string.Empty;
        string FstrFDate = string.Empty;
        string FstrTDate = string.Empty;
        string FstrGubun = string.Empty;    //좌측 탭 조회구분 (공단, 특수, 암 ... )
        string FstrPano = string.Empty;
        string FstrJepDate = string.Empty;

        PAN_PATLIST_SEARCH sItem = null;

        Thread thread = null;
        FpSpread lstSpd = null;

        public delegate void SetPatItem(HC_PANJENG_PATLIST GPatItem);
        public event SetPatItem rSetPatItem;

        public frmHcPanPatList()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            this.Load               += new EventHandler(eFormLoad);
            this.btnSearch.Click    += new EventHandler(eBtnClick);
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnLtdCode.Click   += new EventHandler(eBtnClick);

            this.tab1.Click += new EventHandler(eTabClick);
            this.tab2.Click += new EventHandler(eTabClick);
            this.tab3.Click += new EventHandler(eTabClick);
            this.tab4.Click += new EventHandler(eTabClick);
            
            this.rdoJob0.CheckedChanged += new EventHandler(eRdoCheckedChanged);
            this.rdoJob1.CheckedChanged += new EventHandler(eRdoCheckedChanged);
            this.rdoJob2.CheckedChanged += new EventHandler(eRdoCheckedChanged);

            this.txtSName.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.SSList.CellClick   += new CellClickEventHandler(eSpdCellClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdCellDblClick);

            this.dtpFDate.TextChanged += new EventHandler(eDtpTxtChanged);
        }

        private void eDtpTxtChanged(object sender, EventArgs e)
        {
            dtpTDate.Text = dtpFDate.Text;
        }

        void SetControl()
        {
            cSpd = new clsSpread();
            cHM = new clsHcMain();
            cHB = new clsHaBase();
            cHF = new clsHcFunc();
            CF = new ComFunc();

            sItem = new PAN_PATLIST_SEARCH();

            hcPanjengPatlistService = new HcPanjengPatlistService();
            hicResSahusangdamService = new HicResSahusangdamService();
            hicSpcPanjengService = new HicSpcPanjengService();
            hicResultExCodeService = new HicResultExCodeService();
            hicSunapdtlService = new HicSunapdtlService();

            SSList.Initialize(new SpreadOption { RowHeaderVisible = true, ColumnHeaderHeight = 50 });
            SSList.AddColumn("접수번호",        nameof(HC_PANJENG_PATLIST.WRTNO),         64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("성명",            nameof(HC_PANJENG_PATLIST.SNAME),         74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = true, IsEditble = false });
            SSList.AddColumn("검진종류",        nameof(HC_PANJENG_PATLIST.EXNAME),        80, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("회사명",          nameof(HC_PANJENG_PATLIST.LTDNAME),      160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("검진일자",        nameof(HC_PANJENG_PATLIST.JEPDATE),       82, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("검진차수",        nameof(HC_PANJENG_PATLIST.GJCHASU),       42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });                      // index 5
            SSList.AddColumn("의사",            nameof(HC_PANJENG_PATLIST.DOCTOR),        74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("미판독",          nameof(HC_PANJENG_PATLIST.XREAD),         42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("종검수검",        nameof(HC_PANJENG_PATLIST.JONGGUMYN),     42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("성별",            nameof(HC_PANJENG_PATLIST.SEX),           30, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("나이",            nameof(HC_PANJENG_PATLIST.AGE),           34, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });                     // index 10
            SSList.AddColumn("외래번호",        nameof(HC_PANJENG_PATLIST.PTNO),          80, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("출장",            nameof(HC_PANJENG_PATLIST.GBCHUL),        32, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("입력완료",        nameof(HC_PANJENG_PATLIST.GBSTSNM),       38, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("입력완료여부",    nameof(HC_PANJENG_PATLIST.GBSTS),         30, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, IsEditble = false });
            SSList.AddColumn("종류",            nameof(HC_PANJENG_PATLIST.GJJONG),        32, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, IsEditble = false });                     // index 15
            SSList.AddColumn("주민번호",        nameof(HC_PANJENG_PATLIST.JUMIN),        150, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("주민번호",        nameof(HC_PANJENG_PATLIST.JUMIN2),       150, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, IsEditble = false });
            SSList.AddColumn("학년",            nameof(HC_PANJENG_PATLIST.CLASS),         44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("반",              nameof(HC_PANJENG_PATLIST.BAN),           44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("번호",            nameof(HC_PANJENG_PATLIST.BUN),           44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });                     // index 20
            SSList.AddColumn("판정의사번호",    nameof(HC_PANJENG_PATLIST.PANJENGDRNO),   44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SSList.AddColumn("가판정",          nameof(HC_PANJENG_PATLIST.GAPANJENGNAME), 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SSList.AddColumn("PANO",            nameof(HC_PANJENG_PATLIST.PANO),          50, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SSList.AddColumn("인터넷문진번호",  nameof(HC_PANJENG_PATLIST.IEMUNNO),       50, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SSList.AddColumn("GWRTNO",          nameof(HC_PANJENG_PATLIST.GWRTNO),        80, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SSList.AddColumn("사업장코드",      nameof(HC_PANJENG_PATLIST.LTDCODE),       44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SSList.AddColumn("69종판정",        nameof(HC_PANJENG_PATLIST.GBADDPAN),      38, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });  // index 27

            UnaryComparisonConditionalFormattingRule unary;

            //미판독
            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.PaleVioletRed;
            SSList.ActiveSheet.SetConditionalFormatting(-1, (int)clsHcSpd.enmHcPanPatList.XREAD, unary);

            //종검수검
            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "Y", false);
            unary.BackColor = Color.PaleGoldenrod;
            SSList.ActiveSheet.SetConditionalFormatting(-1, (int)clsHcSpd.enmHcPanPatList.JONGGUMYN, unary);

            //출장여부
            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "Y", false);
            unary.BackColor = Color.PaleGoldenrod;
            SSList.ActiveSheet.SetConditionalFormatting(-1, (int)clsHcSpd.enmHcPanPatList.GBCHUL, unary);

            //입력완료
            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "N", false);
            unary.BackColor = Color.PaleVioletRed;
            SSList.ActiveSheet.SetConditionalFormatting(-1, (int)clsHcSpd.enmHcPanPatList.GBSTS, unary);

            cboPan.Items.Clear();
            cHB.READ_Combo_HicDoctor2(cboPan);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            cSpd.Spread_All_Clear(SSList);

            ComFunc.ReadSysDate(clsDB.DbCon);

            cHF.SET_자료사전_VALUE();

            if (clsHcVariable.GstrPanFrDate.IsNullOrEmpty())
            {
                dtpFDate.Text = CF.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, -20);
                dtpTDate.Text = clsPublic.GstrSysDate;
            }
            else
            {
                dtpFDate.Text = clsHcVariable.GstrPanFrDate;
                dtpTDate.Text = clsHcVariable.GstrPanToDate;
            }

            //판정구분
            if (!clsHcVariable.GStrPanJob.IsNullOrEmpty())
            {
                if (clsHcVariable.GStrPanJob == "0")
                {
                    rdoJob0.Checked = true;
                }
                else if (clsHcVariable.GStrPanJob == "1")
                {
                    rdoJob1.Checked = true;
                }
                else if (clsHcVariable.GStrPanJob == "2")
                {
                    rdoJob2.Checked = true;
                }
            }

            //출장구분
            if (!clsHcVariable.GstrGbChul.IsNullOrEmpty())
            {
                if (clsHcVariable.GstrGbChul == "0")
                {
                    rdoChul0.Checked = true;
                }
                else if (clsHcVariable.GstrGbChul == "1")
                {
                    rdoChul1.Checked = true;
                }
                else if (clsHcVariable.GstrGbChul == "2")
                {
                    rdoChul2.Checked = true;
                }
            }

            //정렬기준
            if (!clsHcVariable.GstrGbSort.IsNullOrEmpty())
            {
                if (clsHcVariable.GstrGbSort == "0")
                {
                    rdoHicSort0.Checked = true;
                }
                else if (clsHcVariable.GstrGbSort == "1")
                {
                    rdoHicSort1.Checked = true;
                }
                else if (clsHcVariable.GstrGbSort == "2")
                {
                    rdoHicSort2.Checked = true;
                }
                else if (clsHcVariable.GstrGbSort == "3")
                {
                    rdoHicSort3.Checked = true;
                }
            }

            if (clsHcVariable.GstrPanDrNo != "")
            {
                cboPan.SelectedIndex = cboPan.FindString(clsHcVariable.GstrPanDrNo);
            }
            else
            {
                if (clsHcVariable.GnHicLicense == 0 || clsHcVariable.GnHicLicense.IsNullOrEmpty())
                {
                    cboPan.SelectedIndex = 0;
                }
                else
                {
                    cboPan.SelectedIndex = cboPan.FindString(clsHcVariable.GnHicLicense.To<string>("0"));
                }
            }

            if (!clsHcVariable.GstrLtdName.IsNullOrEmpty())
            {
                txtLtdCode.Text = clsHcVariable.GstrLtdName;
            }

            chkHicMunjin.Checked = clsHcVariable.GbMunjinView;

            if (clsHcVariable.GStrPanGjJong.IsNullOrEmpty())
            {
                eTabClick(tab1, new EventArgs());
            }
            else
            {
                switch (clsHcVariable.GStrPanGjJong)
                {
                    case "HIC":     //공단      
                        superTabControl1.SelectedTab = tab1;
                        eTabClick(tab1, new EventArgs());
                        break;
                    case "SPC":     //특수검진
                        superTabControl1.SelectedTab = tab2;
                        eTabClick(tab2, new EventArgs());
                        break;
                    case "REC":     //채용
                        superTabControl1.SelectedTab = tab3;
                        eTabClick(tab3, new EventArgs());
                        break;
                    case "ETC":     //기타
                        superTabControl1.SelectedTab = tab4;
                        eTabClick(tab4, new EventArgs());
                        break;
                    default:
                        break;
                }
            }

            btnSearch.PerformClick();
        }

        void eRdoCheckedChanged(object sender, EventArgs e)
        {
            if (sender == rdoJob0)
            {
                chkAutoPan.Enabled = true;
            }
            else if (sender == rdoJob1)
            {
                chkAutoPan.Checked = false;
                chkAutoPan.Enabled = false;
            }
            else if (sender == rdoJob2)
            {
                chkAutoPan.Enabled = true;
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtSName)
            {
                if (e.KeyChar == 13)
                {
                    if (txtSName.Text.Trim() != "")
                    {
                        eBtnClick(btnSearch, new EventArgs());

                        Application.DoEvents();
                        FstrGubun = "";
                    }
                }
            }
            else if (sender == txtLtdCode)
            {
                if (e.KeyChar == 13)
                {
                    if (e.KeyChar == (char)13)
                    {
                        if (txtLtdCode.Text.Length >= 2)
                        {
                            eBtnClick(btnLtdCode, new EventArgs());
                        }
                    }
                }
            }
            else if (sender == txtWrtNo)
            {
                if (e.KeyChar == 13)
                {
                    if (txtWrtNo.Text != "")
                    {
                        eBtnClick(btnSearch, new EventArgs());
                        Application.DoEvents();
                        FstrGubun = "";

                        if (SSList.ActiveSheet.RowCount > 0)
                        {
                            Sel_Pat_Info(0);
                        }
                    }
                }
            }
        }

        void eSpdCellDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader) { return; }

            //접수번호 바코드 인식으로 입력한 경우 정보갱신이 안됨으로 보완함
            Sel_Pat_Info(e.Row);
        }

        void Sel_Pat_Info(int nRow)
        {
            HC_PANJENG_PATLIST item = new HC_PANJENG_PATLIST();

            item.SNAME       = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.SNAME].Text;
            item.EXNAME      = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.EXNAME].Text;
            item.LTDCODE     = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.LTDCODE].Text;
            item.LTDNAME     = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.LTDNAME].Text;
            item.JEPDATE     = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.JEPDATE].Text;
            item.GJCHASU     = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.GJCHASU].Text;
            item.DOCTOR      = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.DOCTOR].Text;
            item.WRTNO       = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.WRTNO].Text.To<long>();
            item.JONGGUMYN   = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.JONGGUMYN].Text;
            item.SEX         = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.SEX].Text;
            item.AGE         = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.AGE].Text.To<long>();
            item.PTNO        = string.Format("{0:00000000}", SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.PTNO].Text.To<long>());
            item.GBCHUL      = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.GBCHUL].Text;
            item.GBSTS       = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.GBSTS].Text;
            item.GBSTSNM     = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.GBSTSNM].Text;
            item.GJJONG      = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.GJJONG].Text;
            item.PANJENGDRNO = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.PANJENGDRNO].Text.To<long>();
            item.PANO        = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.PANO].Text.To<long>();
            item.IEMUNNO     = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.IEMUNNO].Text.To<long>();
            item.GWRTNO      = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.GWRTNO].Text.To<long>();
            item.JUMIN       = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.JUMIN].Text;
            item.JUMIN2      = SSList.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPanPatList.JUMIN2].Text;
            item.FRDATE      = dtpFDate.Text;
            item.TODATE      = dtpTDate.Text;
            item.SELECTTAB   = FstrGubun;
            item.JOB         = FstrJob;
            item.UCODES      = cHB.Read_UCODES(item.WRTNO);
            item.MUN         = chkHicMunjin.Checked ? "Y" : "N";

            clsHcVariable.GbMunjinView = chkHicMunjin.Checked;
            clsHcVariable.GStrPanGjJong = FstrGubun;
            clsHcVariable.GstrPanFrDate = dtpFDate.Text;
            clsHcVariable.GstrPanToDate = dtpTDate.Text;
            //clsHcVariable.GstrPanDrNo = VB.Pstr(cboPan.Text, ".", 1);
            clsHcVariable.GstrPanDrNo = cboPan.Text;
            clsHcVariable.GstrLtdName = txtLtdCode.Text.Trim();
            clsHcVariable.GstrGbChul = rdoChul0.Checked ? "0" : rdoChul1.Checked ? "1" : rdoChul2.Checked ? "2" : "";
            clsHcVariable.GstrGbSort = rdoHicSort0.Checked ? "0" : rdoHicSort1.Checked ? "1" : rdoHicSort2.Checked ? "2" : rdoHicSort3.Checked ? "3" : "";

            if (rdoJob0.Checked == true)
            {
                clsHcVariable.GStrPanJob = "0";
            }
            else if (rdoJob1.Checked == true)
            {
                clsHcVariable.GStrPanJob = "1";
            }
            else if (rdoJob2.Checked == true)
            {
                clsHcVariable.GStrPanJob = "2";
            }

            //수검자 판정화면 구분 누락으로 오류 보완 2021-05-28 (from. 김귀남)
            if (FstrGubun == "")
            {
                switch (item.GJJONG)
                {
                    case "11":
                    case "14":
                        item.SELECTTAB = "HIC";
                        break;
                    case "16":
                    case "23":
                    case "28":
                        item.SELECTTAB = "SPC";
                        break;
                    case "21":
                    case "22":
                    case "24":
                    case "25":
                    case "26":
                    case "27":

                    case "29":
                    case "30":
                    case "33":
                    case "49":
                        item.SELECTTAB = "REC";
                        break;
                    case "62":  //혈액종합검진
                    case "69":  //회사추가검진
                    case "51":  //방사선종사자1차
                    case "50":  //방사선종사자2차
                    case "54":  //위생
                        item.SELECTTAB = "ETC";
                        break;
                    default:
                        break;
                }
            }

            rSetPatItem(item);

            this.Close();

            return;
        }

        void eSpdCellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread cSpd = new clsSpread();
                cSpd.setSpdSort(SSList, e.Column, true);
                cSpd = null;
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display(SSList);
            }
            else if (sender == btnExit)
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
        }

        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        void eTabClick(object sender, EventArgs e)
        {
            fn_OptionViewReset();
            lblLtdTitle.Text = "사업장";
            panJob.Enabled = true;

            fn_Set_Spread();

            if (sender == tab4)
            {
                if (clsHcVariable.GnHicLicense == 0)
                {
                    SSList.ActiveSheet.Columns.Get(27).Visible = true;
                }
            }
            else
            {
                SSList.ActiveSheet.Columns.Get(27).Visible = false;
            }
            

            fn_PanelSet(pnlHic, true);
            panWrtno.Visible = true;

            if (clsHcVariable.GStrPanJob == "0")
            {
                rdoJob0.Checked = true;
            }
            else if (clsHcVariable.GStrPanJob == "1")
            {
                rdoJob1.Checked = true;
            }
            else if (clsHcVariable.GStrPanJob == "2")
            {
                rdoJob2.Checked = true;
            }
            else
            {
                rdoJob1.Checked = true;
            }

            if (clsHcVariable.GstrPanFrDate.IsNullOrEmpty())
            {
                dtpFDate.Text = CF.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, -20);
            }

            btnSearch.PerformClick();
        }

        private void fn_Set_Spread()
        {
            SSList.ActiveSheet.Columns.Get(18).Visible = false;
            SSList.ActiveSheet.Columns.Get(19).Visible = false;
            SSList.ActiveSheet.Columns.Get(20).Visible = false;
        }

        void fn_OptionViewReset()
        {
            fn_PanelSet(pnlHic, false);
        }

        void fn_PanelSet(Panel pnl, bool bTF)
        {
            pnl.Visible = bTF;
            pnl.Location = new System.Drawing.Point(5, 1);
        }

        void Screen_Display(FpSpread Spd)
        {
            string strSort = "";
            string strChul = "";
            long nLtdCode = 0;

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            lstSpd = Spd;

            FstrJob = "";

            string strPan = VB.Pstr(cboPan.Text, ".", 1);

            if (rdoJob1.Checked) { FstrJob = "1"; }         //미판정
            else if (rdoJob2.Checked) { FstrJob = "2"; }    //판정 

            FstrFDate = dtpFDate.Text;
            FstrTDate = dtpTDate.Text;

            if (superTabControl1.SelectedTab == tab1)   //공단, 생활습관, 특수, 암, 기타(방사선종사자/회사추가검진/혈액종양)
            {
                FstrGubun = "HIC";
            }
            else if (superTabControl1.SelectedTab == tab2)   //특수검진
            {
                FstrGubun = "SPC";
            }
            else if (superTabControl1.SelectedTab == tab3)   //채용
            {
                FstrGubun = "REC";
            }
            else if (superTabControl1.SelectedTab == tab4)   //기타
            {
                FstrGubun = "ETC";
            }
            
            //조회 조건 세팅
            switch (FstrGubun)
            {
                case "HIC":     //공단      
                case "SPC":     //특수검진
                case "REC":     //채용              
                case "ETC":     //기타
                    if (rdoJob1.Checked == true)      { FstrJob = "1"; }     //미판정 
                    else if (rdoJob2.Checked == true) { FstrJob = "2"; }     //판정
                    else if (rdoJob0.Checked == true) { FstrJob = "";  }     //전체

                    if (rdoChul0.Checked == true)      { strChul = "N"; }
                    else if (rdoChul1.Checked == true) { strChul = "Y"; }

                    if (rdoHicSort0.Checked == true)      { strSort = "0"; }
                    else if (rdoHicSort1.Checked == true) { strSort = "1"; }
                    else if (rdoHicSort2.Checked == true) { strSort = "2"; }
                    else if (rdoHicSort3.Checked == true) { strSort = "3"; }

                    break;
                default: break;
            }

            if (!txtLtdCode.Text.IsNullOrEmpty()) { nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0); }

            //명단조회 조건
            sItem = new PAN_PATLIST_SEARCH
            {
                JOBGUBUN    = FstrGubun,
                FRDATE      = FstrFDate,
                TODATE      = FstrTDate,
                MUNJINVIEW  = chkHicMunjin.Checked ? true : false,
                JOB         = FstrJob,
                IDNUMBER    = clsType.User.IdNumber.To<long>(),
                LICENSE     = clsHcVariable.GnHicLicense,           //로그인 판정의사 LICENSE
                LICENSE_VIEW= strPan,                               //조회할 판정의사 LICENSE
                SNAME       = txtSName.Text.Replace("\r\n", ""),
                SORT        = strSort,
                LTDCODE     = nLtdCode,
                CHUL        = strChul,
                WRTNO       = txtWrtNo.Text.To<long>(),
                B02_PANJENG_DRNO = clsHcVariable.B02_PANJENG_DRNO,
                GBAUTOPAN   = chkAutoPan.Checked ? "Y" : "N",
            };

            try
            {
                //SSList.DataSource =  hcPanjengPatlistService.GetPanjengPatListbyJepDate(sItem);

                //스레드 시작
                thread = new Thread(tProcess);
                thread.Start();

                Cursor.Current = Cursors.Default;
               
                this.btnExit.Enabled = true;
                this.SSList.Enabled = true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        delegate void threadSpdTypeDelegate(FpSpread spd, List<HC_PANJENG_PATLIST> lists);
        delegate void threadProcessDelegate(bool b);

        void trunCircular(bool b)
        {
            this.Progress.Visible = b;
            this.Progress.IsRunning = b;
        }

        void tProcess()
        {
            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));
            this.lstSpd.BeginInvoke(new System.Action(() => this.lstSpd.Enabled = false));

            List<HC_PANJENG_PATLIST> lstHPP = hcPanjengPatlistService.GetPanjengPatListbyJepDate(sItem);
            this.Invoke(new threadSpdTypeDelegate(tShowSpread), lstSpd, lstHPP);

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });
            this.lstSpd.BeginInvoke(new System.Action(() => this.lstSpd.Enabled = true));
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));
        }

        void tShowSpread(FpSpread spd, List<HC_PANJENG_PATLIST> lists)
        {
            spd.DataSource = lists;
        }
    }
}
