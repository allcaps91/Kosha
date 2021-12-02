using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcMatterEntry.cs
/// Description     : 유해물질 코드 등록관리
/// Author          : 김민철
/// Create Date     : 2019-12-30
/// Update History  : 
/// </summary>
/// <seealso cref= "FrmMatterEntry(HcCode13.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcMatterEntry : Form
    {
        HicMcodeService hicMcodeService = null;
        HicCodeService hicCodeService = null;

        public frmHcMatterEntry()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            hicMcodeService = new HicMcodeService();
            hicCodeService = new HicCodeService();

            SS1.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SS1.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += eSpdDelete;
            SS1.AddColumn("코드",         nameof(HIC_MCODE.CODE),     42, FpSpreadCellType.TextCellType);
            SS1.AddColumn("취급물질명",   nameof(HIC_MCODE.NAME),    160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS1.AddColumn("약어",         nameof(HIC_MCODE.YNAME),   120, FpSpreadCellType.TextCellType);            
            SS1.AddColumn("유해인자",     nameof(HIC_MCODE.UCODE),    42, FpSpreadCellType.TextCellType);
            SS1.AddColumn("유해인자명",   nameof(HIC_MCODE.UNAME),   160, FpSpreadCellType.TextCellType);
            SS1.AddColumn("검사주기(월)", nameof(HIC_MCODE.JUGI),     42, FpSpreadCellType.TextCellType);
            SS1.AddColumn("선택검사",     nameof(HIC_MCODE.GBSELECT), 42, FpSpreadCellType.CheckBoxCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("구강여부",     nameof(HIC_MCODE.GBDENT),   42, FpSpreadCellType.TextCellType);
            SS1.AddColumn("ROWID",        nameof(HIC_MCODE.RID),      42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            
        }

        private void eSpdDelete(object sender, EditorNotifyEventArgs e)
        {
            HIC_MCODE code = SS1.GetRowData(e.Row) as HIC_MCODE;

            SS1.DeleteRow(e.Row);
        }

        private void SetEvent()
        {
            this.Load                += new EventHandler(eFormLoad);
            this.btnExit.Click       += new EventHandler(eBtnClick);            
            this.btnSave.Click       += new EventHandler(eBtnClick);
            this.btnAdd.Click        += new EventHandler(eBtnClick);
            this.SS1.EditModeOff     += new EventHandler(eSpdEditOff);
        }

        private void eSpdEditOff(object sender, EventArgs e)
        {
            int nRow, nCol;

            if (sender == SS1)
            {
                nRow = SS1.ActiveSheet.ActiveRowIndex;
                nCol = SS1.ActiveSheet.ActiveColumnIndex;

                if (nCol != 4) { return; }

                string strCode = SS1.ActiveSheet.Cells[nRow, 4].Text;

                if (strCode.IsNullOrEmpty()) { return; }

                SS1.ActiveSheet.Cells[nRow, 5].Text = hicCodeService.GetNameByGubunCode("09", strCode);
            }
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
            else if (sender == btnAdd)
            {
                SS1.AddRows();
            }
        }

        private void Data_Save()
        {
            IList<HIC_MCODE> list = SS1.GetEditbleData<HIC_MCODE>();

            if (list.Count > 0)
            {
                if (hicMcodeService.Save(list))
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
            SS1.DataSource = hicMcodeService.FindAll();
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Display();
        }
    }
}
