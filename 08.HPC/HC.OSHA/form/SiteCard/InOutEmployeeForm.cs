using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Exceptions;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using HC.Core.Dto;
using HC.Core.Repository;
using HC.Core.Service;
using HC.Core.Common.Extension;
using HC.Core.Common.Interface;
using HC_Core;
using HC.Core.Model;
using HC.OSHA.Dto;
using HC.OSHA.Service;
using HC.OSHA.Model;
using HC_OSHA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using ComHpcLibB.Model;

namespace HC_OSHA
{
    /// <summary>
    /// 입퇴사자관리
    /// </summary>
    public partial class InOutEmployeeForm : CommonForm, ISelectSite, ISelectEstimate
    {
        HcOshaCard5Service hcOshaCard5Service;
        public InOutEmployeeForm()
        {
            InitializeComponent();
            hcOshaCard5Service = new HcOshaCard5Service();
        }
        private void InOutEmployeeForm_Load(object sender, EventArgs e)
        {

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSList.AddColumnDateTime("등록일자", nameof(HC_OSHA_CARD5.REGISTERDATE), 120, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            SSList.AddColumnText("입사자", nameof(HC_OSHA_CARD5.JOINCOUNT), 100, IsReadOnly.N, new SpreadCellTypeOption { });
            SSList.AddColumnText("퇴사자", nameof(HC_OSHA_CARD5.QUITCOUNT), 100, IsReadOnly.N, new SpreadCellTypeOption { });
      //      SSList.AddColumnDateTime("수정일자", nameof(HC_OSHA_CARD5.MODIFIED), 100, IsReadOnly.Y, DateTimeType.YYYY_MM_DD_HH_MM, new SpreadCellTypeOption { IsShowCalendarButton = false});
     //       SSList.AddColumnText("수정자", nameof(HC_OSHA_CARD5.MODIFIEDUSER), 100, IsReadOnly.Y, new SpreadCellTypeOption {});
     //       SSList.AddColumnDateTime("생성일자", nameof(HC_OSHA_CARD5.CREATED), 100, IsReadOnly.Y, DateTimeType.YYYY_MM_DD_HH_MM, new SpreadCellTypeOption { IsShowCalendarButton = false });
     //       SSList.AddColumnText("생성자", nameof(HC_OSHA_CARD5.CREATEDUSER), 100, IsReadOnly.Y, new SpreadCellTypeOption {});
            SSList.AddColumnButton(" ", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += (s, ev) => { SSList.DeleteRow(ev.Row); };
            Search();
        }

        private void Search()
        {
            if (base.SelectedSite != null && base.SelectedEstimate != null)
            {
                List<HC_OSHA_CARD5> list = hcOshaCard5Service.FindAll(base.SelectedEstimate.ID);
                SSList.SetDataSource(list);
            }
            else
            {
                SSList.SetDataSource(new List<HC_OSHA_CARD5>());
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
                if (SSList.RowCount() > 0)
                {
                    IList<HC_OSHA_CARD5> list = SSList.GetEditbleData<HC_OSHA_CARD5>();
                    if (list.Count > 0)
                    {
                        if (hcOshaCard5Service.Save(list, base.SelectedEstimate.ID, base.SelectedSite.ID))
                        {
                            MessageUtil.Info("저장하였습니다");
                            Search();
                        }
                        else
                        {
                            MessageUtil.Alert("오류가 발생하였습니다. ");

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

        public void Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;
            Search();
        }

        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;

            SSList.SetDataSource(new List<HC_OSHA_CARD6>());
        }
    }
}
