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
/// File Name       : frmHaGroupExam.cs
/// Description     : 종검 묶음코드 검사항목 관리
/// Author          : 김민철
/// Create Date     : 2020-03-16
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmGroupExam(HaCode04.frm)" />
namespace ComHpcLibB
{
    public partial class frmHaGroupExam : Form
    {
        HeaGroupcodeService heaGroupcodeService = null;
        HeaExjongService heaExjongService = null;
        HicExcodeService hicExcodeService = null;
        HeaCodeService heaCodeService = null;
        HeaGroupexamExcodeService heaGroupexamExcodeService = null;
        HeaGroupexamService heaGroupexamService = null;
        HIC_LTD LtdHelpItem = null;

        string FstrGbSuga = string.Empty;
        string FstrCode = string.Empty;

        public frmHaGroupExam()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            heaGroupcodeService = new HeaGroupcodeService();
            heaExjongService = new HeaExjongService();
            heaCodeService = new HeaCodeService();
            hicExcodeService = new HicExcodeService();
            heaGroupexamExcodeService = new HeaGroupexamExcodeService();
            heaGroupexamService = new HeaGroupexamService();
            LtdHelpItem = new HIC_LTD();

            //검진종류
            cboJong.SetOptions(new ComboBoxOption { DataField = nameof(HEA_GROUPCODE.JONG) });
            cboJong.Items.Add("**.전체");
            cboJong.SetItems(heaExjongService.FindAll(), "NAME", "CODE", "", "", AddComboBoxPosition.Top);
            cboJong1.Items.Add("**.전체");
            cboJong1.SetItems(heaExjongService.FindAll(), "NAME", "CODE");

            cboPart.Items.Clear();
            cboPart.Items.Add("0.전체");
            cboPart.SetItems(heaCodeService.GetNameByGubun("07"), "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            cboBuRate.Items.Clear();
            cboBuRate.Items.Add("1.본인100%");
            cboBuRate.Items.Add("2.회사100%");
            cboBuRate.Items.Add("3.회사,본인50%");

            SS3.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            SS3.AddColumn("묶음코드",   nameof(HEA_GROUPCODE.CODE), 68, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsSort = true });
            SS3.AddColumn("묶음코드명", nameof(HEA_GROUPCODE.NAME), 210, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });

            SS1.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            SS1.AddColumn("검사코드",       nameof(HIC_EXCODE.CODE), 68, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsSort = true });
            SS1.AddColumn("검사명칭(한글)", nameof(HIC_EXCODE.HNAME), 210, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });

            SS2.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            SS2.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += eSpdDelete;            
            SS2.AddColumn("순위",         nameof(HEA_GROUPEXAM_EXCODE.HEASORT), 54, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS2.AddColumn("검사코드",     nameof(HEA_GROUPEXAM_EXCODE.EXCODE),  64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("보험수가",     nameof(HEA_GROUPEXAM_EXCODE.AMT2),    42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS2.AddColumn("검사명(한글)", nameof(HEA_GROUPEXAM_EXCODE.HNAME),  210, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("ROWID",        nameof(HEA_GROUPEXAM_EXCODE.RID),     42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsVisivle = false });

            chkSelect.SetOptions(new CheckBoxOption { DataField = nameof(HEA_GROUPCODE.GBSELECT), CheckValue = "Y", UnCheckValue = "" });
        }

        private void SetEvent()
        {
            this.Load                   += new EventHandler(eFormLoad);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnCancel.Click        += new EventHandler(eBtnClick);
            this.btnSave.Click          += new EventHandler(eBtnClick);
            this.btnDelete.Click        += new EventHandler(eBtnClick);
            this.btnSearch1.Click       += new EventHandler(eBtnClick);
            this.btnSearch2.Click       += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click       += new EventHandler(eBtnClick);
            this.txtLtdName.KeyPress    += new KeyPressEventHandler(eKeyPress);
            this.SS1.CellDoubleClick    += new CellClickEventHandler(eSpdDbClick);
            this.SS2.ButtonClicked      += new EditorNotifyEventHandler(eSpdDelete);
            this.SS3.CellDoubleClick    += new CellClickEventHandler(eSpdDbClick);
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(); }
            }
        }

        private void eSpdDelete(object sender, EditorNotifyEventArgs e)
        {
            HEA_GROUPEXAM code = SS2.GetRowData(e.Row) as HEA_GROUPEXAM;

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

                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    if (strCode == SS2.ActiveSheet.Cells[i, 2].Text.Trim())
                    {
                        MessageBox.Show(i + "번줄에 이미 " + strCode + "코드가 있습니다.", "확인");
                        return;
                    }
                }

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
                        default: SS2.ActiveSheet.Cells[nRow, 4].Text = ""; break;

                    }

                    SS2.ActiveSheet.SetActiveCell(nRow, 1);
                }
            }
            else if (sender == SS3)
            {
                string strCode = SS3.ActiveSheet.Cells[e.Row, 0].Text.Trim();

                HEA_GROUPCODE item = heaGroupcodeService.GetItemByCode(strCode);

                if (!item.IsNullOrEmpty())
                {
                    FstrCode = item.CODE.Trim();
                    FstrGbSuga = item.GBSUGA.Trim();
                    panSub03.SetData(item);
                    if (!item.IsNullOrEmpty())
                    {
                        txtLtdName.Text = item.LTDCODE + "." + item.LTDNAME;
                    }
                }

                //검사항목 Display
                SS2.DataSource = heaGroupexamExcodeService.GetItemsByCode(strCode);
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
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
            }
        }

        private void Data_Search_Part(string argCode)
        {
            if (argCode.IsNullOrEmpty()) { return; }

            SS1.DataSource = hicExcodeService.GetCodebyHeaSort(argCode);
        }

        private void Data_Search_Jong(string argCode)
        {
            if (argCode.IsNullOrEmpty()) { return; }

            SS3.DataSource = heaGroupcodeService.GetListByAll(argCode);
        }

        private void Data_Save(string argJob)
        {
            if (argJob.Equals("Save"))
            {
                IList<HEA_GROUPEXAM> list = SS2.GetEditbleData<HEA_GROUPEXAM>();

                if (list.Count > 0)
                {
                    if (heaGroupexamService.Save(list))
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
                if (heaGroupexamService.DeleteAll(FstrCode))
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
                txtLtdName.Text = txtLtdName.Text + "." + LtdHelpItem.SANGHO;
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
    }
}
