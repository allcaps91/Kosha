using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using HC.OSHA.Visit.Management.Dto;
using HC.OSHA.Visit.Management.Service;
using FarPoint.Win.Spread;
using HC.Core.BaseCode.Management.Dto;
using HC.Core.BaseCode.Management.Service;
using HC.Core.Common.Interface;
using HC.Core.Common.UI;
using HC.Core.Site.Model;

using System;
using System.Collections.Generic;

namespace HC.OSHA.Visit.Management.UI
{
    public partial class CommitteeForm : CommonForm, ISelectSite
    {
        HcOshaVisitCommitteeService hcOshaVisitCommitteeService;
        public CommitteeForm()
        {
            InitializeComponent();
            hcOshaVisitCommitteeService = new HcOshaVisitCommitteeService();
        }

        private void CommitteeForm_Load(object sender, EventArgs e)
        {

            SpreadComboBoxData VISIT_COMMITTEE_TYPE = codeService.GetSpreadComboBoxData("VISIT_COMMITTEE_TYPE", "OSHA");
            SpreadComboBoxData VISIT_COMMITTEE_KIND = codeService.GetSpreadComboBoxData("VISIT_COMMITTEE_KIND", "OSHA");

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSList.AddColumnDateTime("개최일자", nameof(HC_OSHA_VISIT_COMMITTEE.REGDATE), 120, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = false, IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            SSList.AddColumnComboBox("참석구분", nameof(HC_OSHA_VISIT_COMMITTEE.METTINGTYPE), 100, IsReadOnly.N, VISIT_COMMITTEE_TYPE, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnComboBox("회의종료", nameof(HC_OSHA_VISIT_COMMITTEE.METTINGTYPE), 100, IsReadOnly.N, VISIT_COMMITTEE_KIND, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("참석자(위임자)", nameof(HC_OSHA_VISIT_COMMITTEE.MEETINGUSER), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnButton(" ", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += (s, ev) => { SSList.DeleteRow(ev.Row); };
          

            Search();
        }
        private void Search()
        {
            if (base.SelectedSite != null)
            {
                SSList.SetDataSource(hcOshaVisitCommitteeService.FindAll(base.SelectedSite.ID));
            }
            else
            {
                SSList.SetDataSource(new List<HC_OSHA_VISIT_COMMITTEE>());
            }

        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null)
            {
                MessageUtil.Info("사업장을 선택하세요");
            }
            else
            {
                if (SSList.Validate())
                {
                    IList<HC_OSHA_VISIT_COMMITTEE> list = SSList.GetEditbleData<HC_OSHA_VISIT_COMMITTEE>();
                    if (list.Count > 0)
                    {
                        if (hcOshaVisitCommitteeService.Save(list, base.SelectedSite.ID))
                        {
                            MessageUtil.Info("저장하였습니다");
                            Search();
                        }
                        else
                        {
                            MessageUtil.Info("오류가 발생하였습니다. ");

                        }
                    }
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null)
            {
                MessageUtil.Info("사업장을 선택하세요");
            }
            else
            {
                SSList.AddRows();
            }
            
        }
        void ISelectSite.Select(ISiteModel siteModel)
        {
            Search();
        }
    }
}
