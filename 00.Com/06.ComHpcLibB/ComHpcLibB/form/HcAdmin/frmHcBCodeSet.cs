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
/// File Name       : frmHcBCodeSet.cs
/// Description     : 검진센터 기초코드 설정
/// Author          : 김민철
/// Create Date     : 2020-02-17
/// Update History  : 
/// </summary>
/// <seealso cref= "FrmHicBCodeSet(FrmHicBCodeSet.frm)" />
/// 
namespace ComHpcLibB
{
    public partial class frmHcBCodeSet : Form
    {
        clsSpread cSpd = null;
        BasBcodeService basBcodeService = null;

        public frmHcBCodeSet()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            basBcodeService = new BasBcodeService();

            SS1.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SS1.AddColumnButton("삭제", 42, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += ss1Delete_ButtonClick;
            SS1.AddColumn("코드",         nameof(BAS_BCODE.CODE),    64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("명칭",         nameof(BAS_BCODE.NAME),   160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("선택검사코드", nameof(BAS_BCODE.GUBUN2),  82, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });            
            SS1.AddColumn("나이",         nameof(BAS_BCODE.SORT),    42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("성별",         nameof(BAS_BCODE.PART),    42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("년주기",       nameof(BAS_BCODE.CNT),     42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("적용일자",     nameof(BAS_BCODE.JDATE),   42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsVisivle = false });
            SS1.AddColumn("삭제일자",     nameof(BAS_BCODE.DELDATE), 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsVisivle = false });
            SS1.AddColumn("ROWID",        nameof(BAS_BCODE.RID),     42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });

            string[] strCodes = { "HIC_EXAM_MST", "HIC_EXAM_MST_SUB" };

            List<BAS_BCODE> list = basBcodeService.GetListByCodeIn("자료사전목록", strCodes);
            cboJong.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);
        }

        private void ss1Delete_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            BAS_BCODE code = SS1.GetRowData(e.Row) as BAS_BCODE;

            SS1.DeleteRow(e.Row);
        }

        private void SetEvent()
        {
            this.Load                += new EventHandler(eFormLoad);
            this.btnExit.Click       += new EventHandler(eBtnClick);
            this.btnSearch.Click     += new EventHandler(eBtnClick);
            this.btnSave.Click       += new EventHandler(eBtnClick);
            this.btnCancel.Click     += new EventHandler(eBtnClick);
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
            else if (sender == btnCancel)
            {
                Screen_Clear();
            }
        }

        private void Screen_Clear()
        {
            cSpd.Spread_Clear_Simple(SS1);
            lblMsg.Text = "";
            cboJong.SelectedIndex = 1;
        }

        private void Data_Save()
        {
            IList<BAS_BCODE> list = SS1.GetEditbleData<BAS_BCODE>();

            if (list.Count > 0)
            {
                if (basBcodeService.Save(list))
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
            string strGbn = VB.Pstr(cboJong.Text, ".", 1);
            SS1.DataSource = basBcodeService.FindAllByGubun(strGbn);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();
        }
    }
}
