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
    public partial class InformationForm : CommonForm, ISelectSite
    {
        HcOshaVisitInformationService hcOshaVisitInformationService;
        public InformationForm()
        {
            InitializeComponent();
            hcOshaVisitInformationService = new HcOshaVisitInformationService();
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
                    IList<HC_OSHA_VISIT_INFORMATION> list = SSList.GetEditbleData<HC_OSHA_VISIT_INFORMATION>();
                    if (list.Count > 0)
                    {
                        if (hcOshaVisitInformationService.Save(list, base.SelectedSite.ID))
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

        private void InformationForm_Load(object sender, EventArgs e)
        {
            SpreadComboBoxData INFORMATIONTYPE = codeService.GetSpreadComboBoxData("VISIT_INFORMATION_KIND", "OSHA");
            SpreadComboBoxData users = userService.GetSpreadByOsha();


            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSList.AddColumnDateTime("제공일자", nameof(HC_OSHA_VISIT_INFORMATION.REGDATE), 120, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = false, IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            SSList.AddColumnComboBox("정보구분", nameof(HC_OSHA_VISIT_INFORMATION.INFORMATIONTYPE), 100, IsReadOnly.N, INFORMATIONTYPE, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnComboBox("발급자", nameof(HC_OSHA_VISIT_INFORMATION.REGUSERID), 100, IsReadOnly.N, users, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("내용", nameof(HC_OSHA_VISIT_INFORMATION.REMARK), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnButton(" ", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += (s, ev) => { SSList.DeleteRow(ev.Row); };


            Search();
        }
        private void Search()
        {
            if (base.SelectedSite != null)
            {
                SSList.SetDataSource(hcOshaVisitInformationService.FindAll(base.SelectedSite.ID));
            }
            else
            {
                SSList.SetDataSource(new List<HC_OSHA_VISIT_COMMITTEE>());
            }

        }
    }
}
