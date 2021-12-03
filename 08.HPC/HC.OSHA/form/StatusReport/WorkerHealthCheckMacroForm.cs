using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Utils;
using HC.Core.Dto;
using HC_Core;
using HC.Core.Service;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Service.StatusReport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_OSHA.StatusReport
{
    public partial class WorkerHealthCheckMacroForm : CommonForm
    {
        private WorkerHealthCheckMacroService workerHealthCheckMacroService;


        public WorkerHealthCheckMacroForm()
        {
            InitializeComponent();
            workerHealthCheckMacroService = new WorkerHealthCheckMacroService();
        }
    
        private void MacrowordForm_Load(object sender, EventArgs e)
        {
           
            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSList.AddColumnText("상용구", nameof(WorkerHealthCheckMacrowordDto.TITLE), 150, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false});
            SSList.AddColumnText("표시순서", nameof(WorkerHealthCheckMacrowordDto.DISPSEQ), 61, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //   SSList.AddColumnButton("", 50, new SpreadCellTypeOption { IsSort = false, ButtonText = "수정" }).ButtonClick += MacrowordForm_ButtonClick;

            SSList2.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSList2.AddColumnText("상용구", nameof(WorkerHealthCheckMacrowordDto.TITLE), 150, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList2.AddColumnText("표시순서", nameof(WorkerHealthCheckMacrowordDto.DISPSEQ), 61, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

            SearchMacroword();
            SearchMacroword2();
        }

        private void MacrowordForm_ButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            WorkerHealthCheckMacrowordDto dto = SSList.GetRowData(e.Row) as WorkerHealthCheckMacrowordDto;
            PanMacroword.SetData(dto);

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            WorkerHealthCheckMacrowordDto dto = PanMacroword.GetData<WorkerHealthCheckMacrowordDto>();
            if (PanMacroword.Validate<WorkerHealthCheckMacrowordDto>())
            {
                dto.GUBUN = "1";

                string macroType = "0";
                if (RdoMacroType0.Checked)
                {
                    macroType = "0";
                }
                else if (RdoMacroType1.Checked)
                {
                    macroType = "1";

                }
                else
                {
                    macroType = "2";

                }
                dto.MACROTYPE = macroType;
                WorkerHealthCheckMacrowordDto saved = workerHealthCheckMacroService.Save(dto);
                // panMacroword.SetData(saved);
                PanMacroword.Initialize();
                SearchMacroword();
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            PanMacroword.Initialize();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            WorkerHealthCheckMacrowordDto dto = PanMacroword.GetData<WorkerHealthCheckMacrowordDto>();
            if (dto.ID == 0)
            {
                MessageUtil.Alert(" 상용구를 선택하세요 ");
            }
            else
            {
                workerHealthCheckMacroService.healthChecMacroRepository.Delete(dto.ID);
                PanMacroword.Initialize();
                SearchMacroword();

            }
        }

        public void SearchMacroword()
        {
            string macroType = "0";
            if (RdoMacroType0.Checked)
            {
                macroType = "0";
            }
            else if (RdoMacroType1.Checked)
            {
                macroType = "1";

            }
            else
            {
                macroType = "2";

            }
                List<WorkerHealthCheckMacrowordDto> list = workerHealthCheckMacroService.healthChecMacroRepository.FindAll(CommonService.Instance.Session.UserId, "1", macroType);
            SSList.SetDataSource(list);

        }
        public void SearchMacroword2()
        {
            string macroType = "0";
            if (RdoMacro2Type0.Checked)
            {
                macroType = "0";
            }
            else if (RdoMacro2Type1.Checked)
            {
                macroType = "1";

            }
            else
            {
                macroType = "2";

            }
            List<WorkerHealthCheckMacrowordDto> list = workerHealthCheckMacroService.healthChecMacroRepository.FindAll(CommonService.Instance.Session.UserId, "2", macroType);
            SSList2.SetDataSource(list);

        }
        private void SSList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            WorkerHealthCheckMacrowordDto dto = SSList.GetRowData(e.Row) as WorkerHealthCheckMacrowordDto;
           
       
        }

        private void SSList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            WorkerHealthCheckMacrowordDto dto = SSList.GetRowData(e.Row) as WorkerHealthCheckMacrowordDto;
            PanMacroword.SetData(dto);
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PanMacroword_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtnSave2_Click(object sender, EventArgs e)
        {
            WorkerHealthCheckMacrowordDto dto = PanMacroword2.GetData<WorkerHealthCheckMacrowordDto>();
            if (PanMacroword2.Validate<WorkerHealthCheckMacrowordDto>())
            {
                dto.GUBUN = "2";

                string macroType = "0";
                if (RdoMacro2Type0.Checked)
                {
                    macroType = "0";
                }
                else if (RdoMacro2Type1.Checked)
                {
                    macroType = "1";

                }
                else
                {
                    macroType = "2";

                }
                dto.MACROTYPE = macroType;

                WorkerHealthCheckMacrowordDto saved = workerHealthCheckMacroService.Save(dto);
                // panMacroword.SetData(saved);
                PanMacroword2.Initialize();
                SearchMacroword2();
            }
        }

        private void BtnNew2_Click(object sender, EventArgs e)
        {
            PanMacroword2.Initialize();
        }

        private void SSList2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            WorkerHealthCheckMacrowordDto dto = SSList2.GetRowData(e.Row) as WorkerHealthCheckMacrowordDto;
            PanMacroword2.SetData(dto);
        }

        private void BtnDelete2_Click(object sender, EventArgs e)
        {
            WorkerHealthCheckMacrowordDto dto = PanMacroword2.GetData<WorkerHealthCheckMacrowordDto>();
            if (dto.ID == 0)
            {
                MessageUtil.Alert(" 상용구를 선택하세요 ");
            }
            else
            {
                workerHealthCheckMacroService.healthChecMacroRepository.Delete(dto.ID);
                PanMacroword2.Initialize();
                SearchMacroword2();

            }
        }

        private void RdoMacroType0_CheckedChanged(object sender, EventArgs e)
        {
            SearchMacroword();
        }

        private void RdoMacro2Type0_CheckedChanged(object sender, EventArgs e)
        {
            SearchMacroword2();
        }
    }
}
