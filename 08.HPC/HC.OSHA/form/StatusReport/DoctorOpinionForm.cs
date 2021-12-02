using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using HC.Core.Dto;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Service.StatusReport;
using HC_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_OSHA.form.StatusReport
{
    public partial class DoctorOpinionForm : CommonForm
    {
        private StatusReportDoctorService statusReportDoctorService;
        private MacrowordService macrowordService;
        public StatusReportDoctorDto StatusReportDoctorDto { get; set; }
        public DoctorOpinionForm()
        {
            InitializeComponent();
            macrowordService = new MacrowordService();
            statusReportDoctorService = new StatusReportDoctorService();
        }

        private void DoctorOpinionForm_Load(object sender, EventArgs e)
        {

            TxtOPINION.SetOptions(new TextBoxOption { DataField = nameof(StatusReportDoctorDto.OPINION) });

            //    NumIsEduCount.SetOptions(new NumericUpDownOption { DataField = nameof(NursePerformContentJson.IsEduCount) });

            ssMacroList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            ssMacroList.AddColumnText("상용구", nameof(MacrowordDto.TITLE), 300, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SearchMacroword();
        }
        private void SearchMacroword()
        {
            List<MacrowordDto> list = macrowordService.FindAll("StatusReportByDoctor", TxtOPINION.Name);
            ssMacroList.SetDataSource(list);
        }
        public void SetDto(StatusReportDoctorDto dto, ISiteModel iSiteModel, IEstimateModel iEstimateModel)
        {
            this.StatusReportDoctorDto = dto;
            TxtOPINION.Text = dto.OPINION;
            this.SelectedSite = iSiteModel;
            this.SelectedEstimate = iEstimateModel;
        }

        private void BtnMacro_Click(object sender, EventArgs e)
        {
            MacrowordForm form = new MacrowordForm("StatusReportByDoctor", TxtOPINION);
            form.Show();
            SearchMacroword();
        }

        private void ssMacroList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            MacrowordDto dto = ssMacroList.GetRowData(e.Row) as MacrowordDto;
            //  TxtOPINION.Text = dto.CONTENT;
            if (ChkOverWrite.Checked)
            {
                int selectionIndex = TxtOPINION.SelectionStart;
                TxtOPINION.Text = TxtOPINION.Text.Insert(selectionIndex, dto.CONTENT);
                TxtOPINION.SelectionStart = selectionIndex + dto.CONTENT.Length;
            }
            else
            {
                TxtOPINION.Text = dto.CONTENT;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (StatusReportDoctorDto != null)
            {
                if (MessageUtil.Confirm("저장 하시겠습니까?", this, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    this.StatusReportDoctorDto.OPINION = TxtOPINION.Text;
                    statusReportDoctorService.UpdateOpinion(StatusReportDoctorDto);
                    MessageUtil.Info("종합의견을 저장하였습니다.");
                }
            }
            else
            {
                MessageUtil.Info("상태보고서가 없습니다");
            }
        }

        private void BtnCard19_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null || base.SelectedEstimate == null)
            {
                return;
            }

            CardPage_15_Form form = new CardPage_15_Form();
            form.Show();

            if (base.SelectedSite.ID > 0)
            {
                (form as ISelectSite).Select(base.SelectedSite);
            }

            if (base.SelectedEstimate.ID > 0)
            {
                (form as ISelectEstimate).Select(base.SelectedEstimate);

            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (MessageUtil.Confirm("전체 내용을 지우시겠습니까?", this, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                TxtOPINION.Text = string.Empty;
            }
        }
    }
}
