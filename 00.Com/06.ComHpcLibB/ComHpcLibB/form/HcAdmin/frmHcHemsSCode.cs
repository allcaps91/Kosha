using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcHemsSCode.cs
/// Description     : 특수판정 공단전송용 소견코드 등록
/// Author          : 김민철
/// Create Date     : 2020-02-26
/// Update History  : 
/// </summary>
/// <seealso cref= "FrmSogenEntry(HcCode20.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcHemsSCode : Form
    {
        clsSpread cSpd = null;
        HicScodeService hicScodeService = null;

        public frmHcHemsSCode()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            hicScodeService = new HicScodeService();

            SS1.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SS1.AddColumnButton("삭제", 42, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += ss1Delete_ButtonClick;
            SS1.AddColumn("소견코드", nameof(HIC_SCODE.CODE),       64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("판정",     nameof(HIC_SCODE.PANJENG),   160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("차수",     nameof(HIC_SCODE.CHASU),      82, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });            
            SS1.AddColumn("소견명",   nameof(HIC_SCODE.NAME),       42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("병원코드", nameof(HIC_SCODE.SCODE),      42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("공단코드", nameof(HIC_SCODE.JCODE),      42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("",         "",                           42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsVisivle = false });
            SS1.AddColumn("ROWID",    nameof(HIC_SCODE.RID),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });

        }

        private void ss1Delete_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            HIC_SCODE code = SS1.GetRowData(e.Row) as HIC_SCODE;

            SS1.DeleteRow(e.Row);
        }

        private void SetEvent()
        {
            this.Load                += new EventHandler(eFormLoad);
            this.btnExit.Click       += new EventHandler(eBtnClick);
            this.btnSave.Click       += new EventHandler(eBtnClick);
            this.btnCancel.Click     += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                Data_Save();
            }
            else if (sender == btnCancel)
            {
                Screen_Clear();
            }
        }

        private void Screen_Clear()
        {
            cSpd.Spread_Clear_Simple(SS1);
            Screen_Display();
        }

        private void Data_Save()
        {
            IList<HIC_SCODE> list = SS1.GetEditbleData<HIC_SCODE>();

            if (list.Count > 0)
            {
                if (hicScodeService.Save(list))
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

        private void Screen_Display()
        {
            SS1.DataSource = hicScodeService.FindAll();
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        private void panTitle_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
