using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Utils;
using HC.Core.Dto;
using HC.Core.Service;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_Core
{
    public partial class MacrowordForm : CommonForm
    {
        private MacrowordService macrowordService;

        public string FormName { get; set; }
        public TextBox TargetTextBox { get; set; }
        public TextBox TargetTextBox2 { get; set; }
        public string controlId { get; set; }


        public MacrowordForm(string formName, TextBox textBox)
        {
            InitializeComponent();
            this.FormName = formName;
            this.TargetTextBox = textBox;
            this.controlId = textBox.Name;
            macrowordService = new MacrowordService();
        }
        public MacrowordForm(string formName, TextBox textBox, TextBox textBox2)
        {
            InitializeComponent();
            this.FormName = formName;
            this.TargetTextBox = textBox;
            this.TargetTextBox2 = textBox2;
            this.controlId = textBox.Name;
            macrowordService = new MacrowordService();
        }
        public MacrowordForm(string formName, string ckeditorId)
        {
            InitializeComponent();
            this.FormName = formName;
            this.TargetTextBox = null;
            this.controlId = ckeditorId;
            macrowordService = new MacrowordService();
        }
        private void MacrowordForm_Load(object sender, EventArgs e)
        {
            TxtTitle.SetOptions(new TextBoxOption { DataField = nameof(MacrowordDto.TITLE) });
            TxtContent.SetOptions(new TextBoxOption { DataField = nameof(MacrowordDto.CONTENT) });
            TxtContent2.SetOptions(new TextBoxOption { DataField = nameof(MacrowordDto.CONTENT2) });

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSList.AddColumnText("상용구", nameof(MacrowordDto.TITLE), 210, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false});
            SSList.AddColumnText("순서", nameof(MacrowordDto.DISPSEQ), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //   SSList.AddColumnButton("", 50, new SpreadCellTypeOption { IsSort = false, ButtonText = "수정" }).ButtonClick += MacrowordForm_ButtonClick;
            SearchMacroword();
        }

        private void MacrowordForm_ButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            MacrowordDto dto = SSList.GetRowData(e.Row) as MacrowordDto;
            panMacroword.SetData(dto);

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            MacrowordDto dto = panMacroword.GetData<MacrowordDto>();
            if (dto.ID == 0)
            {
                dto.FORMNAME = this.FormName;
                dto.CONTROL = controlId;
                dto.DISPSEQ = Decimal.ToDouble(numDispSeq.Value);
            }

            if (panMacroword.Validate<MacrowordDto>())
            {
                MacrowordDto saved = macrowordService.Save(dto);
                //panMacroword.SetData(saved);
                panMacroword.Initialize();

                SearchMacroword();
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            panMacroword.Initialize();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            MacrowordDto dto = panMacroword.GetData<MacrowordDto>();
            if (dto.ID == 0)
            {
                MessageUtil.Alert(" 상용구를 선택하세요 ");
            }
            else
            {
                if(MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    macrowordService.MacrowordRepository.Delete(dto.ID);
                    panMacroword.Initialize();
                    SearchMacroword();
                }
            }
        }

        public void SearchMacroword()
        {
           
         
            List<MacrowordDto> list = macrowordService.FindAll(this.FormName, controlId);
            SSList.SetDataSource(list);
        }

        private void SSList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            MacrowordDto dto = SSList.GetRowData(e.Row) as MacrowordDto;
            if (TargetTextBox != null)
            {
                if (ChkOverWrite.Checked)
                {

                    int selectionIndex = TargetTextBox.SelectionStart;
                    TargetTextBox.Text = TargetTextBox.Text.Insert(selectionIndex, dto.CONTENT);
                    TargetTextBox.SelectionStart = selectionIndex + dto.CONTENT.Length;

                    if (TargetTextBox2 != null && dto.CONTENT2!=null)
                    {
                        int selectionInde2 = TargetTextBox2.SelectionStart;
                        TargetTextBox2.Text = TargetTextBox2.Text.Insert(selectionInde2, dto.CONTENT2);
                        TargetTextBox2.SelectionStart = selectionInde2 + dto.CONTENT2.Length;
                    }
                 
                }
                else
                {
                    TargetTextBox.Text = dto.CONTENT;
                    if (TargetTextBox2 != null)
                    {

                        TargetTextBox2.Text = dto.CONTENT2;
                    }
                    
                }
            }
            else
            {

            }
       
        }

        private void SSList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            MacrowordDto dto = SSList.GetRowData(e.Row) as MacrowordDto;
            panMacroword.SetData(dto);
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PanMacroword_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
