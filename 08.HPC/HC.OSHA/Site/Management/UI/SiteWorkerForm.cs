using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using HC.OSHA.Site.Management.Dto;
using HC.OSHA.Site.Management.Service;
using HC.Core.Common.UI;
using HC.Core.Site.Model;
using HC.Core.Common.Interface;
using System;
using System.Collections.Generic;

namespace HC.OSHA.Site.Management.UI
{

    public partial class SiteWorkerForm : CommonForm, ISelectSite
    {

        private HcSiteWorkerService hcSiteWorkerService;
        
        public SiteWorkerForm()
        {
            InitializeComponent();

            hcSiteWorkerService = new HcSiteWorkerService();
        
        }
       
        private void BtnSearch_Click(object sender, EventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            SSWorkerList.AddRows();
         
        }

        private void SiteWorkerForm_Load(object sender, EventArgs e)
        {
            SpreadComboBoxData comboBoxData = codeService.GetSpreadComboBoxData("WORKER_ROLE", "OSHA");

            SSWorkerList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSWorkerList.AddColumnText("이름", nameof(HC_SITE_WORKER.NAME), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SSWorkerList.AddColumnText("소속", nameof(HC_SITE_WORKER.DEPT), 140, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnComboBox("직책", nameof(HC_SITE_WORKER.WORKER_ROLE), 150, IsReadOnly.N, comboBoxData, new SpreadCellTypeOption { IsSort = false });

            SSWorkerList.AddColumnText("전화", nameof(HC_SITE_WORKER.TEL), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });            
            SSWorkerList.AddColumnText("휴대폰", nameof(HC_SITE_WORKER.HP), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("이메일", nameof(HC_SITE_WORKER.EMAIL), 150, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnCheckBox("퇴사여부", nameof(HC_SITE_WORKER.ISRETIRE), 100, new CheckBoxStringCellType { IsHeaderCheckBox = false, CheckedValue="Y", UnCheckedValue="N"}, new SpreadCellTypeOption { IsSort = false,  });
            SSWorkerList.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += SSWorkerListDelete_ButtonClick; ;
            SSWorkerList.SetDataSource(new List<HC_SITE_WORKER>());

            Search();
        }

        private void SSWorkerListDelete_ButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            HC_SITE_WORKER dto =  SSWorkerList.GetRowData(e.Row) as HC_SITE_WORKER;
         
            SSWorkerList.DeleteRow(e.Row);
           
        }

        private void Search()
        {
            if(base.SelectedSite == null)
            {
                SSWorkerList.SetDataSource(new List<HC_SITE_WORKER> ());
            }
            else
            {
                List<HC_SITE_WORKER> list = hcSiteWorkerService.FindAll(base.SelectedSite.ID);
                SSWorkerList.SetDataSource(list);
            }
            
        }

    

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if(base.SelectedSite == null)
            {
                MessageUtil.Alert("사업장을 선택하세요");
            }
            else
            {
                if (SSWorkerList.Validate())
                {
                    IList<HC_SITE_WORKER> list = SSWorkerList.GetEditbleData<HC_SITE_WORKER>();
                    if (list.Count > 0)
                    {

                        if (hcSiteWorkerService.Save(base.SelectedSite.ID, list))
                        {
                            MessageUtil.Info("제품을 저장하였습니다");
                            Search();
                        }
                        else
                        {
                            MessageUtil.Error("트랜잭션 오류로 저장 할 수 없습니다");
                        }
                    }
                    else
                    {
                        MessageUtil.Info("저장할 데이타가 없습니다");
                    }
                }
            }
        }


        void ISelectSite.Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;
            Search();
        }
    }
}

