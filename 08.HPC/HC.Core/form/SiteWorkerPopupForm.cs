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
            SSWorkerList.Initialize(new SpreadOption() { IsRowSelectColor = true });

            SSWorkerList.AddColumnCheckBox("", "", 30, new CheckBoxBooleanCellType());
            SSWorkerList.AddColumnText("이름", nameof(HC_SITE_WORKER.NAME), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SSWorkerList.AddColumnText("부서", nameof(HC_SITE_WORKER.DEPT), 140, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("직책", nameof(HC_SITE_WORKER.WORKER_ROLE), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("사번", nameof(HC_SITE_WORKER.SABUN), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("퇴사일", nameof(HC_SITE_WORKER.END_DATE), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

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
                string strName = TxtNAME.Text.Trim();
                List<HC_SITE_WORKER> list = hcSiteWorkerService.hcSiteWorkerRepository.FindAll(base.SelectedSite.ID, strName,"") ;
             
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
