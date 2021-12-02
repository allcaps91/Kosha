using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using FarPoint.Win.Spread;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// 유해요인 코드관리
/// </summary>
namespace HC_Measurement
{
    public partial class frmHcChkUCode :CommonForm
    {
        clsSpread cSpd = null;
        HicChkUcodeService hicChkService = null;

        public frmHcChkUCode()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            hicChkService = new HicChkUcodeService();

            SpreadComboBoxData GBYN = new SpreadComboBoxData();
            GBYN.Put("Y", "Y");
            GBYN.Put("N", "N");

            SSList.Initialize(new SpreadOption() { ColumnHeaderHeight = 34, RowHeight = 30 });
            SSList.AddColumn("순번",         nameof(HIC_CHK_UCODE.SORT),    44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true, TextMaxLength = 2 });
            SSList.AddColumn("유해요인코드", nameof(HIC_CHK_UCODE.CODE),   120, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true, TextMaxLength = 2 });
            SSList.AddColumn("유해요인명",   nameof(HIC_CHK_UCODE.NAME),   210, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsSort = true, TextMaxLength = 40 });
            SSList.AddColumn("상세설명",     nameof(HIC_CHK_UCODE.REMARK), 210, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { TextMaxLength = 100 });
            SSList.AddColumnComboBox("사용", nameof(HIC_CHK_UCODE.GBUSE),   64, IsReadOnly.N, GBYN, new SpreadCellTypeOption { });
            SSList.AddColumn("ROWID",        nameof(HIC_CHK_UCODE.RID),     42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
        }

        private void UCodeDelete_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            HIC_CHK_UCODE code = SSList.GetRowData(e.Row) as HIC_CHK_UCODE;

            SSList.DeleteRow(e.Row);
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnAdd.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
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
            else if (sender == btnAdd)
            {
                SSList.AddRows(1);
            }
            else if (sender == btnSave)
            {
                IList<HIC_CHK_UCODE> list = SSList.GetEditbleData<HIC_CHK_UCODE>();

                if (list.Count > 0)
                {
                    if (hicChkService.Save(list))
                    {
                        MessageBox.Show("저장하였습니다");
                        Screen_Display();
                    }
                    else
                    {
                        MessageBox.Show("오류가 발생하였습니다. ");
                    }
                }
            }
        }

        private void Screen_Display()
        {
            List<HIC_CHK_UCODE> list = hicChkService.GetItemAll();

            SSList.DataSource = list;

            SSList.AddRows(5);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            cSpd.Spread_Clear_Simple(SSList);
            Screen_Display();
        }

    }
}
