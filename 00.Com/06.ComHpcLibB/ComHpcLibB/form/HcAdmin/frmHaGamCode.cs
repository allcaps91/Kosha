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
/// File Name       : frmHaGamCode.cs
/// Description     : 종합검진 감액코드 관리(종검)
/// Author          : 김민철
/// Create Date     : 2019-11-26
/// Update History  : 
/// </summary>
/// <seealso cref= "FrmGamCodeEntry(HaCode11.frm)" />

namespace ComHpcLibB
{
    public partial class frmHaGamCode : Form
    {
        clsSpread cSpd = null;
        HeaGamcodeService heaGamCodeService = null;

        public frmHaGamCode()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            heaGamCodeService = new HeaGamcodeService();

            SS1.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SS1.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += GamCodeDelete_ButtonClick;
            SS1.AddColumn("코드",       nameof(HEA_GAMCODE.CODE),  42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("코드명칭",   nameof(HEA_GAMCODE.NAME), 160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("적용방법",   nameof(HEA_GAMCODE.GUBUN), 82, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });            
            SS1.AddColumn("할인율(%)",  nameof(HEA_GAMCODE.RATE),  42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("할인액(남)", nameof(HEA_GAMCODE.AMT1),  42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("할인액(여)", nameof(HEA_GAMCODE.AMT2),  42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("ROWID",      nameof(HEA_GAMCODE.RID),   42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("삭제일자",   nameof(HEA_GAMCODE.DELDATE), 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });

        }

        private void GamCodeDelete_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            HEA_GAMCODE code = SS1.GetRowData(e.Row) as HEA_GAMCODE;

            SS1.DeleteRow(e.Row);
        }

        private void SetEvent()
        {
            this.Load                += new EventHandler(eFormLoad);
            this.btnExit.Click       += new EventHandler(eBtnClick);
            this.btnSearch.Click     += new EventHandler(eBtnClick);
            this.btnSave.Click       += new EventHandler(eBtnClick);
            this.btnAdd.Click        += new EventHandler(eBtnClick);
            this.SS1.LeaveCell       += new LeaveCellEventHandler(eSpdLeaveCell);
        }

        private void eSpdLeaveCell(object sender, LeaveCellEventArgs e)
        {
            if (e.Row == 3)
            {
                lblMsg.Text = "▶적용방법: 1.할인율 2.할인금액 3.특별감액";
            }
            else
            {
                lblMsg.Text = "";
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
            IList<HEA_GAMCODE> list = SS1.GetEditbleData<HEA_GAMCODE>();

            if (list.Count > 0)
            {
                if (heaGamCodeService.Save(list))
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

        private void eFormLoad(object sender, EventArgs e)
        {   
            Screen_Display();
        }

        private void Screen_Clear()
        {
            cSpd.Spread_Clear_Simple(SS1);
            lblMsg.Text = "";
        }

        private void Screen_Display()
        {
            Screen_Clear();

            SS1.DataSource = heaGamCodeService.GetListItems(chkDel.Checked);
        }

    }
}
