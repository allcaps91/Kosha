using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcSunapList.cs
/// Description     : 검진비 수납내역 조회
/// Author          : 김민철
/// Create Date     : 2019-09-06
/// Update History  : 
/// </summary>
/// <seealso cref= "FrmSunapReport(HaMain64.frm) / Frm검진비수납자명단(Frm검진비수납자명단.frm)" /> 
namespace HC_Main
{
    /// <summary>
    /// TODO : 종합검진비 수납집계표 예약금 수납내역도 집계가능하도록 설계
    /// </summary>
    public partial class frmHcSunapList : Form
    {
        clsHaBase cHB = null;
        clsSpread cSpd = null;
        clsSpread sp = new clsSpread();

        HIC_LTD LtdHelpItem = null;

        HicJepsuSunapService hicJepsuSunapService = null;
        HeaJepsuSunapService heaJepsuSunapService = null;
        HeaGamcodeService heaGamcodeService = null;

        string FstrFDate = string.Empty;
        string FstrTDate = string.Empty;
        string FstrJong = string.Empty;
        string FstrJob = string.Empty;
        string FstrSName = string.Empty;

        long FnLtdCode = 0;

        bool FbZero = false;

        public frmHcSunapList()
        {
            InitializeComponent();
            SetEvents();
            SetControl();
        }

        private void SetEvents()
        {
            this.Load += new EventHandler(eFormload);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.txtLtdName.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.rdoAll1.CheckedChanged += new EventHandler(eRdoChanged);
            this.rdoAll2.CheckedChanged += new EventHandler(eRdoChanged);
        }

        private void eRdoChanged(object sender, EventArgs e)
        {
            if (rdoAll1.Checked)
            {
                cboJong.Items.Clear();
                cboJong.Items.Add("**.전체");
                cHB.ComboJong_AddItem(cboJong, true);

                cboJong.SelectedIndex = 0;
            }
            else
            {
                cboJong.Items.Clear();
                cboJong.Items.Add("**.전체");
                cHB.ComboJong2_AddItem(cboJong);

                cboJong.SelectedIndex = 0;
            }
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(); }
            }
        }

        private void SetControl()
        {
            cHB = new clsHaBase();
            cSpd = new clsSpread();
            LtdHelpItem = new HIC_LTD();
            hicJepsuSunapService = new HicJepsuSunapService();
            heaJepsuSunapService = new HeaJepsuSunapService();
            heaGamcodeService = new HeaGamcodeService();

            cboJong.Items.Clear();
            cboJong.Items.Add("**.전체");
            cHB.ComboJong_AddItem(cboJong, true);

            //감액계정
            List<HEA_GAMCODE> heaGamCode = heaGamcodeService.GetListItems(false);
            cboHalinGye.SetItems(heaGamCode, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            SSList.Initialize();
            SSList.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            SSList.AddColumn("검진종류", "", 78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("등록번호", "", 72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("수검자명", "", 64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("주민번호", "", 102, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("순번", "", 34, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            SSList.AddColumn("접수번호", "", 54, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            SSList.AddColumn("접수일자", "", 44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsSort = true, });
            SSList.AddColumn("종검유무", "", 34, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            SSList.AddColumn("사업장명", "", 120, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("수납자명", "", 62, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("카드", "", 34, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            SSList.AddColumn("수납액", "", 72, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Right });
            SSList.AddColumn("총진료비", "", 72, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Right });
            SSList.AddColumn("공단부담", "", 72, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Right });
            SSList.AddColumn("회사부담", "", 72, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Right });
            SSList.AddColumn("보건소부담", "", 74, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Right });
            SSList.AddColumn("본인부담", "", 72, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Right });
            SSList.AddColumn("할인금액", "", 72, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Right });
            SSList.AddColumn("미수금액", "", 72, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Right });
            SSList.AddColumn("카드금액", "", 72, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Right });
            SSList.AddColumn("수납일자", "", 44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            SSList.AddColumn("비고", "", 120, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                if (rdoAll1.Checked)
                {
                    Spread_Set("HIC");
                    Screen_Display_Hic(SSList);
                }
                else
                {
                    Spread_Set("HEA");
                    Screen_Display_Hea(SSList);
                }

            }

            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                if (rdoAll1.Checked)
                {
                    strTitle = "일반건진 수납집계표";

                    strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                    //strHeader += sp.setSpdPrint_String("┌─┬────┬────┬────┬────┐", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    //strHeader += sp.setSpdPrint_String("│결│ 담  당 │ 계  장 │ 팀  장 │ 부  장 │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    //strHeader += sp.setSpdPrint_String("│  ├────┼────┼────┼────┤", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    //strHeader += sp.setSpdPrint_String("│  │        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    //strHeader += sp.setSpdPrint_String("│  │        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    //strHeader += sp.setSpdPrint_String("│재│        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    //strHeader += sp.setSpdPrint_String("└─┴────┴────┴────┴────┘", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    //strHeader += sp.setSpdPrint_String("      수납일자 : " + dtpFDate.Text + "     " + "인쇄시각 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                    strHeader += sp.setSpdPrint_String("┌─┬────┬────┬────┬────┬────┐", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    strHeader += sp.setSpdPrint_String("│결│ 담  당 │ 계  장 │ 팀  장 │ 부  장 │ 병원장 │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    strHeader += sp.setSpdPrint_String("│  ├────┼────┼────┼────┼────┤", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    strHeader += sp.setSpdPrint_String("│  │        │        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    strHeader += sp.setSpdPrint_String("│  │        │        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    strHeader += sp.setSpdPrint_String("│재│        │        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    strHeader += sp.setSpdPrint_String("└─┴────┴────┴────┴────┴────┘", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    strHeader += sp.setSpdPrint_String("      수납일자 : " + dtpFDate.Text + "     " + "인쇄시각 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                }
                else
                {
                    strTitle = "종합건진 수납집계표";

                    strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                    //strHeader += sp.setSpdPrint_String("┌─┬────┬────┬────┐", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    //strHeader += sp.setSpdPrint_String("│결│ 담  당 │ 부  장 │ 병원장 │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    //strHeader += sp.setSpdPrint_String("│  ├────┼────┼────┤", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    //strHeader += sp.setSpdPrint_String("│  │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    //strHeader += sp.setSpdPrint_String("│  │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    //strHeader += sp.setSpdPrint_String("│재│        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    //strHeader += sp.setSpdPrint_String("└─┴────┴────┴────┘", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    //strHeader += sp.setSpdPrint_String("      수납일자 : " + dtpFDate.Text + "     " + "인쇄시각 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                    strHeader += sp.setSpdPrint_String("┌─┬────┬────┬────┬────┬────┐", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    strHeader += sp.setSpdPrint_String("│결│ 담  당 │ 계  장 │ 팀  장 │ 부  장 │ 병원장 │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    strHeader += sp.setSpdPrint_String("│  ├────┼────┼────┼────┼────┤", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    strHeader += sp.setSpdPrint_String("│  │        │        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    strHeader += sp.setSpdPrint_String("│  │        │        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    strHeader += sp.setSpdPrint_String("│재│        │        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    strHeader += sp.setSpdPrint_String("└─┴────┴────┴────┴────┴────┘", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                    strHeader += sp.setSpdPrint_String("      수납일자 : " + dtpFDate.Text + "     " + "인쇄시각 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                }

                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.7f);
                sp.setSpdPrint(SSList, PrePrint, setMargin, setOption, strHeader, strFooter);

            }

            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            #region 사업장코드찾기
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
            }
            #endregion
        }

        private void Spread_Set(string argGubun)
        {
            if (argGubun.Equals("HIC"))
            {
                SSList.ActiveSheet.Columns.Get(7).Label = "종검유무";
                SSList.ActiveSheet.Columns.Get(7).Width = 34;
            }
            else if (argGubun.Equals("HEA"))
            {
                SSList.ActiveSheet.Columns.Get(7).Label = "할인구분";
                SSList.ActiveSheet.Columns.Get(7).Width = 120;
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

            if (LtdHelpItem.CODE > 0 && !LtdHelpItem.IsNullOrEmpty())
            {
                txtLtdName.Text = LtdHelpItem.CODE.To<string>();
                txtLtdName.Text += "." + LtdHelpItem.SANGHO;
            }
            else
            {
                txtLtdName.Text = "";
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void Screen_Display_Hic(FpSpread Spd)
        {
            int nRow = 0;
            string strNewData = string.Empty;
            string strOldData = string.Empty;

            long nTotalAmt = 0;
            long nJohapAmt = 0;
            long nLtdAmt = 0;
            long nBoninAmt = 0;
            long nHalinAmt = 0;

            long[,] nTotAmt = new long[3, 10];

            string strSunap = "";

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            FstrFDate = dtpFDate.Value.ToShortDateString();
            FstrTDate = dtpTDate.Value.ToShortDateString();
            FstrJong = VB.Left(cboJong.Text, 2);
            FnLtdCode = VB.Pstr(txtLtdName.Text, ".", 1).To<long>();
            FstrJob = rdoJob1.Checked == true ? "1" : rdoJob2.Checked == true ? "2" : "3";
            FstrSName = txtSname.Text.Trim();
            FbZero = chkZeroAmt.Checked;

            if (rdoSunap0.Checked)
            {
                strSunap = "0";
            }
            else if (rdoSunap1.Checked)
            {
                strSunap = "1";
            }
            else if (rdoSunap2.Checked)
            {
                strSunap = "2";
            }

            try
            {
                List<HIC_JEPSU_SUNAP> list = null;

                if (rdoHis1.Checked)
                {
                    list = hicJepsuSunapService.GetListAll(FstrFDate, FstrTDate, FstrJong, FstrJob, FnLtdCode, FstrSName, FbZero, strSunap);
                }
                else
                {
                    list = hicJepsuSunapService.GetListSum(FstrFDate, FstrTDate, FstrJong, FstrJob, FnLtdCode, FstrSName, FbZero, strSunap);
                }


                if (list.Count <= 0)
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (int i = 0; i < list.Count; i++)
                {
                    nTotalAmt = 0; nJohapAmt = 0; nLtdAmt = 0; nBoninAmt = 0; nHalinAmt = 0;

                    strNewData = list[i].GJJONG;
                    strNewData = strNewData + list[i].PTNO;

                    nRow = nRow + 1;
                    if (SSList.ActiveSheet.RowCount < nRow) { SSList.ActiveSheet.RowCount = nRow; }

                    if (strOldData.IsNullOrEmpty())
                    {
                        strOldData = strNewData;
                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].GJNAME;
                    }
                    else if (VB.Left(strOldData, 2) != VB.Left(strNewData, 2))
                    {
                        SSList.ActiveSheet.Cells[nRow - 1, 6].Text = "소계";
                        cSpd.setSpdCellColor(SSList, nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1, System.Drawing.Color.FromArgb(255, 224, 192));

                        for (int j = 1; j < 10; j++)
                        {
                            SSList.ActiveSheet.Cells[nRow - 1, j + 10].Text = nTotAmt[1, j].ToString();
                            nTotAmt[1, j] = 0;
                        }

                        strOldData = strNewData;

                        nRow = nRow + 1;
                        if (SSList.ActiveSheet.RowCount < nRow) { SSList.ActiveSheet.RowCount = nRow; }

                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].GJNAME;

                    }
                    else if (strOldData != strNewData)
                    {
                        strOldData = strNewData;
                    }

                    if (list[i].SNAME == "최연정")
                    {
                        nRow = nRow;
                    }

                    SSList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].PTNO;
                    SSList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].JUMINNO;
                    SSList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].SEQNO.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].WRTNO.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].JEPDATE_MMDD;
                    SSList.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].JONGGUMYN;
                    SSList.ActiveSheet.Cells[nRow - 1, 8].Text = list[i].LTDNAME;
                    SSList.ActiveSheet.Cells[nRow - 1, 9].Text = list[i].JOBNAME;
                    SSList.ActiveSheet.Cells[nRow - 1, 10].Text = list[i].GBCARD;
                    SSList.ActiveSheet.Cells[nRow - 1, 11].Text = list[i].SUNAPAMT1.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 12].Text = list[i].TOTAMT.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 13].Text = list[i].JOHAPAMT.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 14].Text = list[i].LTDAMT.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 15].Text = list[i].BOGENAMT.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 16].Text = list[i].BONINAMT.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 17].Text = list[i].HALINAMT.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 18].Text = list[i].MISUAMT.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 19].Text = list[i].SUNAPAMT2.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 20].Text = list[i].SUDATE_MMDD;
                    SSList.ActiveSheet.Cells[nRow - 1, 21].Text = list[i].REMARK;

                    //소계에 금액을 Add
                    nTotAmt[1, 1] = nTotAmt[1, 1] + list[i].SUNAPAMT1;
                    nTotAmt[1, 2] = nTotAmt[1, 2] + list[i].TOTAMT;
                    nTotAmt[1, 3] = nTotAmt[1, 3] + list[i].JOHAPAMT;
                    nTotAmt[1, 4] = nTotAmt[1, 4] + list[i].LTDAMT;
                    nTotAmt[1, 5] = nTotAmt[1, 5] + list[i].BOGENAMT;
                    nTotAmt[1, 6] = nTotAmt[1, 6] + list[i].BONINAMT;
                    nTotAmt[1, 7] = nTotAmt[1, 7] + list[i].HALINAMT;
                    nTotAmt[1, 8] = nTotAmt[1, 8] + list[i].MISUAMT;
                    nTotAmt[1, 9] = nTotAmt[1, 9] + list[i].SUNAPAMT2;

                    //합계에 금액을 Add
                    nTotAmt[2, 1] = nTotAmt[2, 1] + list[i].SUNAPAMT1;
                    nTotAmt[2, 2] = nTotAmt[2, 2] + list[i].TOTAMT;
                    nTotAmt[2, 3] = nTotAmt[2, 3] + list[i].JOHAPAMT;
                    nTotAmt[2, 4] = nTotAmt[2, 4] + list[i].LTDAMT;
                    nTotAmt[2, 5] = nTotAmt[2, 5] + list[i].BOGENAMT;
                    nTotAmt[2, 6] = nTotAmt[2, 6] + list[i].BONINAMT;
                    nTotAmt[2, 7] = nTotAmt[2, 7] + list[i].HALINAMT;
                    nTotAmt[2, 8] = nTotAmt[2, 8] + list[i].MISUAMT;
                    nTotAmt[2, 9] = nTotAmt[2, 9] + list[i].SUNAPAMT2;

                    //차액발생시 색깔 표시
                    if (list[i].TOTAMT != (list[i].JOHAPAMT + list[i].LTDAMT + list[i].BONINAMT + list[i].HALINAMT + list[i].BOGENAMT))
                    {
                        cSpd.setSpdCellColor(SSList, nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1, System.Drawing.Color.FromArgb(255, 192, 192));
                    }
                    else if (list[i].SUNAPAMT1 != list[i].BONINAMT)
                    {
                        cSpd.setSpdCellColor(SSList, nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1, System.Drawing.Color.FromArgb(255, 192, 192));
                    }
                    else
                    {
                        cSpd.setSpdCellColor(SSList, nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1, System.Drawing.Color.White);
                    }
                }

                nRow = nRow + 1;
                if (SSList.ActiveSheet.RowCount < nRow) { SSList.ActiveSheet.RowCount = nRow; }

                SSList.ActiveSheet.Cells[nRow - 1, 6].Text = "소계";
                cSpd.setSpdCellColor(SSList, nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1, System.Drawing.Color.FromArgb(255, 224, 192));

                for (int j = 1; j < 10; j++)
                {
                    SSList.ActiveSheet.Cells[nRow - 1, j + 10].Text = nTotAmt[1, j].ToString();
                }

                nRow = nRow + 1;
                if (SSList.ActiveSheet.RowCount < nRow) { SSList.ActiveSheet.RowCount = nRow; }

                SSList.ActiveSheet.Cells[nRow - 1, 6].Text = "합계";
                cSpd.setSpdCellColor(SSList, nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1, System.Drawing.Color.FromArgb(192, 255, 192));

                for (int j = 1; j < 10; j++)
                {
                    SSList.ActiveSheet.Cells[nRow - 1, j + 10].Text = nTotAmt[2, j].To<string>();
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void Screen_Display_Hea(FpSpread Spd)
        {
            int nRow = 0;
            string strNewData = string.Empty;
            string strOldData = string.Empty;
            string strGubun = "";

            long nTotalAmt = 0;
            long nJohapAmt = 0;
            long nLtdAmt = 0;
            long nBoninAmt = 0;
            long nHalinAmt = 0;

            long[,] nTotAmt = new long[3, 10];
            List<long> strAllWrtno = new List<long>();


            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            FstrFDate = dtpFDate.Value.ToShortDateString();
            FstrTDate = dtpTDate.Value.ToShortDateString();
            FstrJong = VB.Left(cboJong.Text, 2);
            FnLtdCode = txtLtdName.Text.To<int>();
            FstrJob = rdoJob1.Checked == true ? "1" : rdoJob2.Checked == true ? "2" : "3";
            FstrSName = txtSname.Text.Trim();
            FbZero = chkZeroAmt.Checked;

            try
            {
                List<HEA_JEPSU_SUNAP> list = null;
                List<HEA_JEPSU_SUNAP> list2 = null;



                strAllWrtno.Clear();
                //수납변경자명단
                list2 = heaJepsuSunapService.GetListSum2(FstrFDate, FstrTDate, FstrJong, FstrJob, FnLtdCode, FstrSName, FbZero);
                if (!list2.IsNullOrEmpty())
                {
                    for (int i = 0; i< list2.Count; i++)
                    {
                        strAllWrtno.Add(list2[i].WRTNO);
                    }
                }
                if (rdoHis1.Checked)
                {
                    list = heaJepsuSunapService.GetListAll(FstrFDate, FstrTDate, FstrJong, FstrJob, FnLtdCode, FstrSName, FbZero);
                }
                else
                {
                    strGubun = "합계";
                    list = heaJepsuSunapService.GetListSum(FstrFDate, FstrTDate, FstrJong, FstrJob, FnLtdCode, FstrSName, FbZero, strAllWrtno);
                    //수납변경자명단
                    //list2 = heaJepsuSunapService.GetListSum2(FstrFDate, FstrTDate, FstrJong, FstrJob, FnLtdCode, FstrSName, FbZero);
                }


                if (list.Count <= 0)
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (int i = 0; i < list.Count; i++)
                {
                    nTotalAmt = 0; nJohapAmt = 0; nLtdAmt = 0; nBoninAmt = 0; nHalinAmt = 0;

                    strNewData = list[i].GJJONG;
                    strNewData = strNewData + list[i].PTNO;

                    nRow = nRow + 1;
                    if (SSList.ActiveSheet.RowCount < nRow) { SSList.ActiveSheet.RowCount = nRow; }

                    if (strOldData.IsNullOrEmpty())
                    {
                        strOldData = strNewData;
                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].GJNAME;
                    }
                    else if (strOldData.Substring(0, 2) != strNewData.Substring(0, 2))
                    {
                        SSList.ActiveSheet.Cells[nRow - 1, 6].Text = "소계";
                        cSpd.setSpdCellColor(SSList, nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1, System.Drawing.Color.FromArgb(255, 224, 192));

                        for (int j = 1; j < 10; j++)
                        {
                            SSList.ActiveSheet.Cells[nRow - 1, j + 10].Text = nTotAmt[1, j].ToString();
                            nTotAmt[1, j] = 0;
                        }

                        strOldData = strNewData;

                        nRow = nRow + 1;
                        if (SSList.ActiveSheet.RowCount < nRow) { SSList.ActiveSheet.RowCount = nRow; }

                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].GJNAME;

                    }
                    else if (strOldData != strNewData)
                    {
                        strOldData = strNewData;
                    }

                    SSList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].PTNO;
                    SSList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].JUMINNO;
                    SSList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].SEQNO.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].WRTNO.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].SDATE_MMDD;
                    SSList.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].HALINGYE_NM;
                    SSList.ActiveSheet.Cells[nRow - 1, 8].Text = list[i].LTDNAME;
                    SSList.ActiveSheet.Cells[nRow - 1, 9].Text = list[i].JOBNAME;
                    SSList.ActiveSheet.Cells[nRow - 1, 10].Text = list[i].GBCARD;
                    SSList.ActiveSheet.Cells[nRow - 1, 11].Text = list[i].SUNAPAMT1.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 12].Text = (list[i].TOTAMT - list[i].HALINAMT).ToString();
                    SSList.ActiveSheet.Cells[nRow - 1, 13].Text = list[i].JOHAPAMT.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 14].Text = list[i].LTDAMT.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 15].Text = "0";
                    SSList.ActiveSheet.Cells[nRow - 1, 16].Text = list[i].BONINAMT.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 17].Text = list[i].HALINAMT.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 18].Text = list[i].MISUAMT.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 19].Text = list[i].SUNAPAMT2.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 20].Text = list[i].SUDATE_MMDD;
                    SSList.ActiveSheet.Cells[nRow - 1, 21].Text = list[i].REMARK;

                    //소계에 금액을 Add
                    nTotAmt[1, 1] = nTotAmt[1, 1] + list[i].SUNAPAMT1;
                    nTotAmt[1, 2] = nTotAmt[1, 2] + list[i].TOTAMT - list[i].HALINAMT;
                    nTotAmt[1, 3] = nTotAmt[1, 3] + list[i].JOHAPAMT;
                    nTotAmt[1, 4] = nTotAmt[1, 4] + list[i].LTDAMT;
                    //nTotAmt[1, 5] = nTotAmt[1, 5] + list[i].BOGENAMT;
                    nTotAmt[1, 6] = nTotAmt[1, 6] + list[i].BONINAMT;
                    nTotAmt[1, 7] = nTotAmt[1, 7] + list[i].HALINAMT;
                    nTotAmt[1, 8] = nTotAmt[1, 8] + list[i].MISUAMT;
                    nTotAmt[1, 9] = nTotAmt[1, 9] + list[i].SUNAPAMT2;

                    //합계에 금액을 Add
                    nTotAmt[2, 1] = nTotAmt[2, 1] + list[i].SUNAPAMT1;
                    nTotAmt[2, 2] = nTotAmt[2, 2] + list[i].TOTAMT - list[i].HALINAMT;
                    nTotAmt[2, 3] = nTotAmt[2, 3] + list[i].JOHAPAMT;
                    nTotAmt[2, 4] = nTotAmt[2, 4] + list[i].LTDAMT;
                    //nTotAmt[2, 5] = nTotAmt[2, 5] + list[i].BOGENAMT;
                    nTotAmt[2, 6] = nTotAmt[2, 6] + list[i].BONINAMT;
                    nTotAmt[2, 7] = nTotAmt[2, 7] + list[i].HALINAMT;
                    nTotAmt[2, 8] = nTotAmt[2, 8] + list[i].MISUAMT;
                    nTotAmt[2, 9] = nTotAmt[2, 9] + list[i].SUNAPAMT2;

                    ////차액발생시 색깔 표시
                    //if (list[i].TOTAMT != (list[i].JOHAPAMT + list[i].LTDAMT + list[i].BONINAMT + list[i].HALINAMT))
                    //{
                    //    cSpd.setSpdCellColor(SSList, nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1, System.Drawing.Color.FromArgb(255, 192, 192));
                    //}
                    //else if (list[i].SUNAPAMT1 != list[i].BONINAMT)
                    //{
                    //    cSpd.setSpdCellColor(SSList, nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1, System.Drawing.Color.FromArgb(255, 192, 192));
                    //}
                    //else
                    //{
                    //    cSpd.setSpdCellColor(SSList, nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1, System.Drawing.Color.White);
                    //}
                }

                nRow = nRow + 1;
                if (SSList.ActiveSheet.RowCount < nRow) { SSList.ActiveSheet.RowCount = nRow; }

                SSList.ActiveSheet.Cells[nRow - 1, 6].Text = "소계";
                cSpd.setSpdCellColor(SSList, nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1, System.Drawing.Color.FromArgb(255, 224, 192));
                for (int j = 1; j < 10; j++)
                {
                    SSList.ActiveSheet.Cells[nRow - 1, j + 10].Text = nTotAmt[1, j].ToString();
                }

                //

                for (int j = 1; j < 10; j++)
                {
                    SSList.ActiveSheet.Cells[nRow - 1, j + 10].Text = nTotAmt[1, j].ToString();
                    nTotAmt[1, j] = 0;
                }


                if (!list2.IsNullOrEmpty() && strGubun == "합계")
                {
                    for (int i = 0; i < list2.Count; i++)
                    {
                        long nGAmt = 0;
                        long nMAmt = 0;
                        long nHAmt = 0;
                        long nCAmt = 0;

                        nGAmt = list2[i].TOTAMT - list2[i].HALINAMT;        //총검진비
                        nMAmt = list2[i].LTDAMT;                            //회사미수
                        nHAmt = list2[i].SUNAPAMT1 + list2[i].SUNAPAMT2;
                        nCAmt = list2[i].SUNAPAMT2;
                        if (nGAmt != 0 || nMAmt != 0 || nHAmt != 0 || nCAmt != 0)
                        {
                            if (SSList.ActiveSheet.RowCount == nRow) { SSList.ActiveSheet.RowCount = nRow + 1; }
                            nRow = nRow + 1;
                            SSList.ActiveSheet.Cells[nRow - 1, 0].Text = "수납변경";
                            SSList.ActiveSheet.Cells[nRow - 1, 1].Text = list2[i].PTNO;
                            SSList.ActiveSheet.Cells[nRow - 1, 2].Text = list2[i].SNAME;
                            SSList.ActiveSheet.Cells[nRow - 1, 3].Text = list2[i].JUMINNO;
                            SSList.ActiveSheet.Cells[nRow - 1, 4].Text = list2[i].SEQNO.To<string>();
                            SSList.ActiveSheet.Cells[nRow - 1, 5].Text = list2[i].WRTNO.To<string>();
                            SSList.ActiveSheet.Cells[nRow - 1, 6].Text = list2[i].SDATE_MMDD;
                            SSList.ActiveSheet.Cells[nRow - 1, 7].Text = list2[i].HALINGYE_NM;
                            SSList.ActiveSheet.Cells[nRow - 1, 8].Text = list2[i].LTDNAME;
                            SSList.ActiveSheet.Cells[nRow - 1, 9].Text = list2[i].JOBNAME;
                            SSList.ActiveSheet.Cells[nRow - 1, 10].Text = list2[i].GBCARD;
                            SSList.ActiveSheet.Cells[nRow - 1, 11].Text = list2[i].SUNAPAMT1.To<string>();
                            SSList.ActiveSheet.Cells[nRow - 1, 12].Text = (list2[i].TOTAMT - list2[i].HALINAMT).ToString();
                            SSList.ActiveSheet.Cells[nRow - 1, 13].Text = list2[i].JOHAPAMT.To<string>();
                            SSList.ActiveSheet.Cells[nRow - 1, 14].Text = list2[i].LTDAMT.To<string>();
                            SSList.ActiveSheet.Cells[nRow - 1, 15].Text = "0";
                            SSList.ActiveSheet.Cells[nRow - 1, 16].Text = list2[i].BONINAMT.To<string>();
                            SSList.ActiveSheet.Cells[nRow - 1, 17].Text = list2[i].HALINAMT.To<string>();
                            SSList.ActiveSheet.Cells[nRow - 1, 18].Text = list2[i].MISUAMT.To<string>();
                            SSList.ActiveSheet.Cells[nRow - 1, 19].Text = list2[i].SUNAPAMT2.To<string>();
                            SSList.ActiveSheet.Cells[nRow - 1, 20].Text = list2[i].SUDATE_MMDD;
                            SSList.ActiveSheet.Cells[nRow - 1, 21].Text = list2[i].REMARK;

                            //소계에 금액을 Add
                            nTotAmt[1, 1] = nTotAmt[1, 1] + list2[i].SUNAPAMT1;
                            nTotAmt[1, 2] = nTotAmt[1, 2] + list2[i].TOTAMT - list2[i].HALINAMT;
                            nTotAmt[1, 3] = nTotAmt[1, 3] + list2[i].JOHAPAMT;
                            nTotAmt[1, 4] = nTotAmt[1, 4] + list2[i].LTDAMT;
                            nTotAmt[1, 6] = nTotAmt[1, 6] + list2[i].BONINAMT;
                            nTotAmt[1, 7] = nTotAmt[1, 7] + list2[i].HALINAMT;
                            nTotAmt[1, 8] = nTotAmt[1, 8] + list2[i].MISUAMT;
                            nTotAmt[1, 9] = nTotAmt[1, 9] + list2[i].SUNAPAMT2;

                            //합계에 금액을 Add
                            nTotAmt[2, 1] = nTotAmt[2, 1] + list2[i].SUNAPAMT1;
                            nTotAmt[2, 2] = nTotAmt[2, 2] + list2[i].TOTAMT - list2[i].HALINAMT;
                            nTotAmt[2, 3] = nTotAmt[2, 3] + list2[i].JOHAPAMT;
                            nTotAmt[2, 4] = nTotAmt[2, 4] + list2[i].LTDAMT;

                            nTotAmt[2, 6] = nTotAmt[2, 6] + list2[i].BONINAMT;
                            nTotAmt[2, 7] = nTotAmt[2, 7] + list2[i].HALINAMT;
                            nTotAmt[2, 8] = nTotAmt[2, 8] + list2[i].MISUAMT;
                            nTotAmt[2, 9] = nTotAmt[2, 9] + list2[i].SUNAPAMT2;

                        }
                    }

                    nRow = nRow + 1;
                    if (SSList.ActiveSheet.RowCount < nRow) { SSList.ActiveSheet.RowCount = nRow; }
                    SSList.ActiveSheet.Cells[nRow - 1, 6].Text = "소계";
                    cSpd.setSpdCellColor(SSList, nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1, System.Drawing.Color.FromArgb(255, 224, 192));
                    for (int j = 1; j < 10; j++)
                    {
                        SSList.ActiveSheet.Cells[nRow - 1, j + 10].Text = nTotAmt[1, j].ToString();
                    }
                }


                nRow = nRow + 1;
                if (SSList.ActiveSheet.RowCount < nRow) { SSList.ActiveSheet.RowCount = nRow; }
                SSList.ActiveSheet.Cells[nRow - 1, 6].Text = "합계";
                cSpd.setSpdCellColor(SSList, nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1, System.Drawing.Color.FromArgb(192, 255, 192));

                for (int j = 1; j < 10; j++)
                {
                    SSList.ActiveSheet.Cells[nRow - 1, j + 10].Text = nTotAmt[2, j].To<string>();
                }

                Cursor.Current = Cursors.Default;



            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void eFormload(object sender, EventArgs e)
        {
            dtpFDate.Text = DateTime.Now.ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            cboJong.SelectedIndex = 0;
        }

    }
}
