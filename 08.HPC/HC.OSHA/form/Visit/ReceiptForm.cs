using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using HC.Core.Common.Interface;
using HC.Core.Model;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Service;
using HC_Core;
using System;
using System.Collections.Generic;

namespace HC_OSHA
{
    public partial class ReceiptForm : CommonForm, ISelectSite
    {
        HcOshaVisitReceiptService hcOshaVisitReceiptService;
        public ReceiptForm()
        {
            InitializeComponent();
            hcOshaVisitReceiptService = new HcOshaVisitReceiptService();
        }

   
        void ISelectSite.Select(ISiteModel siteModel)
        {
            Search();
        }

        private void ReceiptForm_Load(object sender, EventArgs e)
        {
            SpreadComboBoxData users = userService.GetSpreadByOsha();


            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSList.AddColumnDateTime("발급일", nameof(HC_OSHA_VISIT_RECEIPT.REGDATE), 120, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = true, IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            SSList.AddColumnDateTime("계약해지일", nameof(HC_OSHA_VISIT_RECEIPT.CANCELDATE), 100, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = false, IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            SSList.AddColumnText("인계물품", nameof(HC_OSHA_VISIT_RECEIPT.REMARK), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("인수자", nameof(HC_OSHA_VISIT_RECEIPT.TAKEOVER), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            
            SSList.AddColumnText("발급자", nameof(HC_OSHA_VISIT_RECEIPT.REGUSERNAME), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnComboBox("발급자", nameof(HC_OSHA_VISIT_RECEIPT.REGUSERID), 100, IsReadOnly.N, users, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnButton(" ", 60, new SpreadCellTypeOption {  ButtonText = "삭제" }).ButtonClick += (s, ev) => { SSList.DeleteRow(ev.Row); };

            Search();
        }
        private void Search()
        {
            if (base.SelectedSite != null)
            {
                SSList.SetDataSource(hcOshaVisitReceiptService.FindAll(base.SelectedSite.ID));
            }
            else
            {
                SSList.SetDataSource(new List<HC_OSHA_VISIT_RECEIPT>());
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
                    IList<HC_OSHA_VISIT_RECEIPT> list = SSList.GetEditbleData<HC_OSHA_VISIT_RECEIPT>();
                    if (list.Count > 0)
                    {
                        if (hcOshaVisitReceiptService.Save(list, base.SelectedSite.ID))
                        {
                           // MessageUtil.Info("저장하였습니다");
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
                int index =  SSList.AddRows();
                HC_OSHA_VISIT_RECEIPT dto = SSList.GetRowData(index) as HC_OSHA_VISIT_RECEIPT;
                dto.REGUSERID = CommonService.Instance.Session.UserId;
                dto.REGDATE = DateUtil.TodayAsYYYY_MM_DD();
                dto.CANCELDATE = DateUtil.TodayAsYYYY_MM_DD();

            }
        }
    }
}

