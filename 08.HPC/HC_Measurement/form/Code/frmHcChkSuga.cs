using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
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
    public partial class frmHcChkSuga :CommonForm
    {
        clsSpread cSpd = null;
        HicChkSugaService hicChkSugaService = null;
        HicCodeService hicCodeService = null;

        public frmHcChkSuga()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            hicChkSugaService = new HicChkSugaService();
            hicCodeService = new HicCodeService();

            SpreadComboBoxData cboChkCD = hicCodeService.GetListSpdComboBoxData("15");

            SSList.Initialize(new SpreadOption() { ColumnHeaderHeight = 34, RowHeight = 30 });
            SSList.AddColumn("년도",             nameof(HIC_CHK_SUGA.GJYEAR),      52, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SSList.AddColumn("수가코드",         nameof(HIC_CHK_SUGA.SUCODE),      84, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SSList.AddColumn("항목명",           nameof(HIC_CHK_SUGA.SUNAME),     240, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("물질코드",         nameof(HIC_CHK_SUGA.MCODE),      240, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SSList.AddColumn("물질명",           nameof(HIC_CHK_SUGA.MCODE_NM),   240, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true, IsVisivle = false });
            SSList.AddColumn("측정방법명",       nameof(HIC_CHK_SUGA.CHKNAME),    240, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("산정내역명",       nameof(HIC_CHK_SUGA.CALNAME),    240, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("계산서용",         nameof(HIC_CHK_SUGA.ACCNAME),    240, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumnNumber("수수료",     nameof(HIC_CHK_SUGA.AMT),         92, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Right });
            SSList.AddColumnNumber("공단수수료", nameof(HIC_CHK_SUGA.GAMT),        92, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Right });
            SSList.AddColumn("분류항목",         nameof(HIC_CHK_SUGA.HANG1),      120, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("측정방법",         nameof(HIC_CHK_SUGA.HANG2),       88, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("분석방법",         nameof(HIC_CHK_SUGA.HANG3),       88, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("공단코드",         nameof(HIC_CHK_SUGA.GCODE),       92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("사용여부",         nameof(HIC_CHK_SUGA.GBUSE),       84, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("ROWID",            nameof(HIC_CHK_SUGA.RID),         42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
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

            string strRid = SSList.ActiveSheet.Cells[e.Row, 15].Text.Trim();

            frmHcChkSuga_Entry frm = new frmHcChkSuga_Entry(strRid);
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
                frmHcChkSuga_Entry frm = new frmHcChkSuga_Entry();
                frm.ShowDialog();

                Screen_Display();
            }
        }

        private void Screen_Display()
        {
            string strKeyWard = txtKeyWord.Text.Trim();
            long nYEAR = nmrGjYear.Value.To<long>(0);

            List<HIC_CHK_SUGA> list = hicChkSugaService.GetItemAll(nYEAR, strKeyWard);

            SSList.DataSource = list;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            cSpd.Spread_Clear_Simple(SSList);

            int nYY = DateTime.Now.ToShortDateString().Substring(0, 4).To<int>();
            nmrGjYear.Value = nYY;

            Screen_Display();
        }

    }
}
