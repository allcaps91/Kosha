using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcGroupCode.cs
/// Description     : 건진센터 묶음코드 검사항목 관리
/// Author          : 김민철
/// Create Date     : 2019-12-27
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmGroupExam(HcCode05.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcGroupExam : Form
    {
        HicGroupexamExcodeService hicGroupexamExcodeService = null;
        HicGroupcodeService hicGroupcodeService = null;
        HicExjongService hicExjongService = null;
        HicExcodeService hicExcodeService = null;
        HicMcodeService hicMcodeService = null;
        HicGroupexamService hicGroupexamService = null;

        string FstrCode = string.Empty;
        string FstrGbSuga = string.Empty;

        public frmHcGroupExam()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            hicGroupexamExcodeService = new HicGroupexamExcodeService();
            hicGroupcodeService = new HicGroupcodeService();
            hicExjongService = new HicExjongService();
            hicExcodeService = new HicExcodeService();
            hicMcodeService = new HicMcodeService();
            hicGroupexamService = new HicGroupexamService();

            //검진종류
            cboJong.SetOptions(new ComboBoxOption { DataField = nameof(HIC_GROUPCODE.JONG) });
            cboJong.SetItems(hicExjongService.FindAll(), "NAME", "CODE", "", "", AddComboBoxPosition.Top);
            cboJong1.Items.Add("**.전체");
            cboJong1.SetItems(hicExjongService.FindAll(), "NAME", "CODE");

            cboPart.Items.Clear();
            cboPart.Items.Add("*.전체항목");
            cboPart.Items.Add("1.전체항목");
            cboPart.Items.Add("2.전체항목");
            cboPart.Items.Add("3.전체항목");
            cboPart.Items.Add("4.전체항목");
            cboPart.Items.Add("9.전체항목");

            //취급물질
            cboUCode.SetOptions(new ComboBoxOption { DataField = nameof(HIC_GROUPCODE.UCODE) });
            cboUCode.SetItems(hicMcodeService.FindAll(), "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            SS3.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            SS3.AddColumn("묶음코드",     nameof(HIC_GROUPCODE.CODE),   68, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsSort = true });
            SS3.AddColumn("묶음코드명",   nameof(HIC_GROUPCODE.NAME),  210, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });

            SS1.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            SS1.AddColumn("검사코드",       nameof(HIC_EXCODE.CODE),    68, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsSort = true });
            SS1.AddColumn("검사명칭(한글)", nameof(HIC_EXCODE.HNAME),  210, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });

            SS2.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            SS2.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += eSpdDelete;
            //SS2.AddColumn("S",            "",                                    44, FpSpreadCellType.CheckBoxCellType, new SpreadCellTypeOption { IsEditble = false });
            SS2.AddColumn("순위",         nameof(HIC_GROUPEXAM_EXCODE.SEQNO),    54, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS2.AddColumn("검사코드",     nameof(HIC_GROUPEXAM_EXCODE.EXCODE),   64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("S",            nameof(HIC_GROUPEXAM_EXCODE.SUGAGBN),  42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });            
            SS2.AddColumn("검진금액",     nameof(HIC_GROUPEXAM_EXCODE.GAMT1),    64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Right });            
            SS2.AddColumn("검사명(한글)", nameof(HIC_GROUPEXAM_EXCODE.HNAME),   210, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("ROWID",        nameof(HIC_GROUPEXAM_EXCODE.RID),      42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsVisivle = false });

            chkSelect.SetOptions( new CheckBoxOption { DataField = nameof(HIC_GROUPCODE.GBSELECT), CheckValue = "Y", UnCheckValue = "" });
        }

        private void SetEvent()
        {
            this.Load                += new EventHandler(eFormLoad);
            this.btnExit.Click       += new EventHandler(eBtnClick);
            this.btnCancel.Click     += new EventHandler(eBtnClick);
            this.btnSave.Click       += new EventHandler(eBtnClick);
            this.btnDelete.Click     += new EventHandler(eBtnClick);
            this.btnSearch1.Click    += new EventHandler(eBtnClick);
            this.btnSearch2.Click    += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDbClick);
            this.SS2.ButtonClicked += new EditorNotifyEventHandler(eSpdDelete);
            this.SS3.CellDoubleClick += new CellClickEventHandler(eSpdDbClick);
        }

        private void eSpdDelete(object sender, EditorNotifyEventArgs e)
        {
            HIC_GROUPEXAM code = SS2.GetRowData(e.Row) as HIC_GROUPEXAM;

            SS2.DeleteRow(e.Row);
        }

        private void eSpdDbClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader || e.RowHeader)
            {
                return;
            }

            if (sender == SS1)
            {
                string strCode = SS1.ActiveSheet.Cells[e.Row, 0].Text.Trim();

                HIC_EXCODE item = hicExcodeService.FindOne(strCode);

                if (!item.IsNullOrEmpty())
                {
                    int nRow = SS2.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Style);
                    //nRow = SS2.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1;
                    if (SS2.ActiveSheet.Cells[nRow, 2].Text != "")
                    {
                        SS2.ActiveSheet.RowCount += 1;
                        nRow += 1;
                    }

                    SS2.ActiveSheet.Cells[nRow, 1].Text = "0";
                    SS2.ActiveSheet.Cells[nRow, 2].Text = item.CODE;
                    SS2.ActiveSheet.Cells[nRow, 5].Text = item.HNAME;

                    switch (FstrGbSuga)
                    {
                        case "1": SS2.ActiveSheet.Cells[nRow, 4].Text = item.AMT1.To<string>(); break;
                        case "2": SS2.ActiveSheet.Cells[nRow, 4].Text = item.AMT2.To<string>(); break;
                        case "3": SS2.ActiveSheet.Cells[nRow, 4].Text = item.AMT3.To<string>(); break;
                        case "4": SS2.ActiveSheet.Cells[nRow, 4].Text = item.AMT4.To<string>(); break;
                        case "5": SS2.ActiveSheet.Cells[nRow, 4].Text = item.AMT5.To<string>(); break;
                        default:  SS2.ActiveSheet.Cells[nRow, 4].Text = ""; break;
                            
                    }

                    SS2.ActiveSheet.SetActiveCell(nRow, 1);
                }
            }
            else if (sender == SS3)
            {
                string strCode = SS3.ActiveSheet.Cells[e.Row, 0].Text.Trim();

                HIC_GROUPCODE item = hicGroupcodeService.GetItemByCode(strCode);

                if (!item.IsNullOrEmpty())
                {
                    FstrCode = item.CODE.Trim();
                    FstrGbSuga = item.GBSUGA.Trim();
                    panSub03.SetData(item);
                }

                //검사항목 Display
                SS2.DataSource = hicGroupexamExcodeService.GetItemsByCode(strCode);
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnCancel)
            {
                Screen_Clear();
            }
            else if (sender == btnSearch1)
            {
                Data_Search_Jong(VB.Left(cboJong1.Text, 2));
            }
            else if (sender == btnSearch2)
            {
                Data_Search_Part(VB.Left(cboPart.Text, 1));
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            //저장
            else if (sender == btnSave)
            {
                Data_Save("Save");
            }
            //삭제
            else if (sender == btnDelete)
            {
                Data_Save("Delete");
            }
        }

        private void Data_Search_Part(string argCode)
        {
            if (argCode.IsNullOrEmpty()) { return; }

            SS1.DataSource = hicExcodeService.GetCodebyPart(argCode);
        }

        private void Data_Search_Jong(string argCode)
        {
            if (argCode.IsNullOrEmpty()) { return; }

            SS3.DataSource = hicGroupcodeService.GetListByAll(argCode);
        }

        private void Data_Save(string argJob)
        {
            if (argJob.Equals("Save"))
            {
                IList<HIC_GROUPEXAM> list = SS2.GetEditbleData<HIC_GROUPEXAM>();

                if (list.Count > 0)
                {
                    if (hicGroupexamService.Save(list))
                    {
                        MessageBox.Show("저장하였습니다");
                    }
                    else
                    {
                        MessageBox.Show("오류가 발생하였습니다. ");
                    }
                }
            }
            else if (argJob.Equals("Delete"))
            {
                if (hicGroupexamService.DeleteAll(FstrCode))
                {
                    MessageBox.Show("삭제하였습니다");
                }
                else
                {
                    MessageBox.Show("오류가 발생하였습니다.");
                }
            }
            

            Screen_Clear();
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();

            cboJong1.SelectedIndex = 0;
            cboPart.SelectedIndex = 0;
        }

        private void Screen_Clear()
        {
            FstrCode = "";
            FstrGbSuga = "";

            ComFunc.SetAllControlClearEx(panSub03);
            ComFunc.SetAllControlClearEx(panSub04);
        }
    }
}
