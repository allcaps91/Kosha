using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;
using HC.Core.Dto;
using HC.Core.Service;
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

namespace HC_OSHA
{
    public partial class SiteWorkerPopupForm   : CommonForm
    {
        private HcSiteWorkerService hcSiteWorkerService;

        /// <summary>
        /// 선택된 근로자
        /// </summary>
        private List<HC_SITE_WORKER> WorkerList;
        public SiteWorkerPopupForm()
        {
            InitializeComponent();
            hcSiteWorkerService = new HcSiteWorkerService();
            WorkerList = new List<HC_SITE_WORKER>();
        }

        private void SiteWorkerPopupForm_Load(object sender, EventArgs e)
        {

            CboRole.Items.Clear();
            CboRole.SetItems(codeService.FindActiveCodeByGroupCode("WORKER_ROLE", "OSHA"), "codename", "code", "전체","", AddComboBoxPosition.Top,false);

            //     CboRole.SetValue("HEALTH_ROLE");
            CboRole.SetValue("");

            SpreadComboBoxData comboBoxData = codeService.GetSpreadComboBoxData("WORKER_ROLE", "OSHA");
            SSWorkerList.Initialize(new SpreadOption() { IsRowSelectColor = true });

            SSWorkerList.AddColumnCheckBox("", "", 30, new CheckBoxBooleanCellType());
            SSWorkerList.AddColumnText("이름", nameof(HC_SITE_WORKER.NAME), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SSWorkerList.AddColumnText("부서", nameof(HC_SITE_WORKER.DEPT), 140, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnComboBox("직책", nameof(HC_SITE_WORKER.WORKER_ROLE), 150, IsReadOnly.Y, comboBoxData, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("전화", nameof(HC_SITE_WORKER.TEL), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("휴대폰", nameof(HC_SITE_WORKER.HP), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("이메일", nameof(HC_SITE_WORKER.EMAIL), 150, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnCheckBox("퇴사여부", nameof(HC_SITE_WORKER.ISRETIRE), 100, new CheckBoxStringCellType { IsHeaderCheckBox = false, CheckedValue = "Y", UnCheckedValue = "N" }, new SpreadCellTypeOption { IsSort = false, });

            Search();
        }

        private void Search()
        {
            if (base.SelectedSite == null)
            {
                SSWorkerList.SetDataSource(new List<HC_SITE_WORKER>());
            }
            else
            {
                List<HC_SITE_WORKER> list = hcSiteWorkerService.hcSiteWorkerRepository.FindWorkerByRole(base.SelectedSite.ID, CboRole.GetValue()) ;
             
                SSWorkerList.SetDataSource(list);
            }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            SetSelect();
        }

     
        private void SetSelect()
        {
            WorkerList.Clear();
            for (int i =0; i< SSWorkerList.RowCount(); i++)
            {
                if(Convert.ToBoolean(SSWorkerList.ActiveSheet.Cells[i, 0].Value) == true)
                {
                    HC_SITE_WORKER dto = SSWorkerList.GetRowData(i) as HC_SITE_WORKER;
                    WorkerList.Add(dto);
                }
            }
            this.Close();
        }

        /// <summary>
        /// 선택된 근로자 목록 가져오기
        /// </summary>
        /// <returns></returns>
        public List<HC_SITE_WORKER> GetWorker()
        {
            return WorkerList;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void awd_Load(object sender, EventArgs e)
        {

        }
    }
}
