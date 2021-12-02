using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using HC.Core.Service;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// 유해인자(물질) 코드관리
/// </summary>
namespace HC_Measurement
{
    public partial class frmHcChkMCode :CommonForm
    {
        clsSpread cSpd = null;
        HicChkMcodeService hicChkMcodeService = null;
        HicChkUcodeService hicChkUcodeService = null;
        HicCodeService hicCodeService = null;
        HcCodeService hcCodeService = null;

        public frmHcChkMCode()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            hicChkMcodeService = new HicChkMcodeService();
            hicChkUcodeService = new HicChkUcodeService();
            hicCodeService = new HicCodeService();
            hcCodeService = new HcCodeService();

            SpreadComboBoxData cboChkWay = hicCodeService.GetListSpdComboBoxData("15");
            SpreadComboBoxData cboBun = hicChkUcodeService.GetListAllComboBoxData();
            SpreadComboBoxData cboUnit = hcCodeService.GetSpreadComboBoxData("CRITERIA_UNIT", "WEM");

            SSList.Initialize(new SpreadOption() { ColumnHeaderHeight = 34, RowHeight = 30, RowHeaderVisible = true });
            //SSList.AddColumn("순번",             nameof(HIC_CHK_MCODE.SORT),         44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SSList.AddColumn("코드",             nameof(HIC_CHK_MCODE.CODE),         74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SSList.AddColumn("물질명",           nameof(HIC_CHK_MCODE.FULLNAME),    120, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("영문명",           nameof(HIC_CHK_MCODE.ENAME),       140, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("상용명",           nameof(HIC_CHK_MCODE.NAME),         92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("CAS 번호",         nameof(HIC_CHK_MCODE.CAS_NO),      100, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("EC 번호",          nameof(HIC_CHK_MCODE.EC_NO),        92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("주기",             nameof(HIC_CHK_MCODE.CYCLE),        44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("TWA",              nameof(HIC_CHK_MCODE.TWA_PPM),      92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("TWA",              nameof(HIC_CHK_MCODE.TWA_MG),       92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("STEL",             nameof(HIC_CHK_MCODE.STEL_PPM),     92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("STEL",             nameof(HIC_CHK_MCODE.STEL_MG),      92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("Ceiling",          nameof(HIC_CHK_MCODE.CEILING),      92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("회수율",           nameof(HIC_CHK_MCODE.RET_RATE),     44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("FACTOR값",         nameof(HIC_CHK_MCODE.FACTOR_VAL),   44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumnComboBox("측정방법", nameof(HIC_CHK_MCODE.CHK_WAY),     120, IsReadOnly.N,    cboChkWay,    new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = false });
            SSList.AddColumnComboBox("측정단위", nameof(HIC_CHK_MCODE.CHK_UNIT),     44, IsReadOnly.N,    cboUnit,      new SpreadCellTypeOption { IsEditble = false, IsSort = false });
            SSList.AddColumnComboBox("분류",     nameof(HIC_CHK_MCODE.BUN),         110, IsReadOnly.N,    cboBun,       new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = false });
            SSList.AddColumn("측정",             nameof(HIC_CHK_MCODE.GBCHK),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("특검",             nameof(HIC_CHK_MCODE.GBSPC),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("혼합물",           nameof(HIC_CHK_MCODE.GBMIX),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("발암성",           nameof(HIC_CHK_MCODE.GBCAN),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("사용",             nameof(HIC_CHK_MCODE.GBUSE),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("분자량",           nameof(HIC_CHK_MCODE.MOLECULE),     64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("계산상수",         nameof(HIC_CHK_MCODE.CAL_CONST),    48, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("적용값1",          nameof(HIC_CHK_MCODE.APPLY_VALUE1), 48, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("적용값2",          nameof(HIC_CHK_MCODE.APPLY_VALUE2), 48, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("적용값3",          nameof(HIC_CHK_MCODE.APPLY_VALUE3), 48, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("ROWID",            nameof(HIC_CHK_MCODE.RID),          42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader)
            {
                return;
            }

            string strRid = SSList.ActiveSheet.Cells[e.Row, 27].Text.Trim();

            frmHcChkMCode_Entry frm = new frmHcChkMCode_Entry(strRid);
            frm.ShowDialog();

            Screen_Display();
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
                Screen_Display();
            }
            else if (sender == btnSave)
            {
                frmHcChkMCode_Entry frm = new frmHcChkMCode_Entry();
                frm.ShowDialog();

                Screen_Display();
            }
        }

        private void Screen_Display()
        {
            string strKeyWard = txtKeyWord.Text.Trim();

            List<HIC_CHK_MCODE> list = hicChkMcodeService.GetItemAll(strKeyWard);

            SSList.DataSource = list;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            cSpd.Spread_Clear_Simple(SSList);
            Screen_Display();
        }

    }
}
