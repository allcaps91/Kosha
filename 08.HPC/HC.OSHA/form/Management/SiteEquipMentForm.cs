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
    public partial class SiteEquipMentForm : CommonForm, ISelectSite, ISelectEstimate
    {

        private HcOshaEquipmentService hcOshaEquipmentService;
        private StatusReportByEngineer statusReportByEngineer;
        public SiteEquipMentForm()
        {
            InitializeComponent();
            hcOshaEquipmentService = new HcOshaEquipmentService();
        }
        private void SiteEquipMentForm_Load(object sender, EventArgs e)
        {
            SpreadComboBoxData EQUIP_NAME = codeService.GetSpreadComboBoxData("OSHA_EQUIPMENT_NAME", "OSHA");
            //SpreadComboBoxData EQUIP_MODELNAME = codeService.GetSpreadComboBoxData("OSHA_EQUIPMENT_MODELNAME", "OSHA");
            SpreadComboBoxData users = userService.GetSpreadByOsha();
            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSList.AddColumnDateTime("사용일자", nameof(HC_OSHA_EQUIPMENT.REGDATE), 120, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            SSList.AddColumnComboBox("장비명", nameof(HC_OSHA_EQUIPMENT.NAME), 400, IsReadOnly.N, EQUIP_NAME, new SpreadCellTypeOption { });
            //SSList.AddColumnComboBox("모델명", nameof(HC_OSHA_EQUIPMENT.MODELNAME), 100, IsReadOnly.N, EQUIP_MODELNAME, new SpreadCellTypeOption { });
            //SSList.AddColumnText("시리얼번호", nameof(HC_OSHA_EQUIPMENT.SERIALNUMBER), 60, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("사용내역", nameof(HC_OSHA_EQUIPMENT.REMARK), 180, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

            SSList.AddColumnButton(" ", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += (s, ev) => { SSList.DeleteRow(ev.Row); };

            LblSite.Text = "";
            if (base.SelectedSite != null) LblSite.Text = base.SelectedSite.NAME;
            Search();
        }
        private void Search()
        {
            if (base.SelectedSite != null)
            {
                SSList.SetDataSource(hcOshaEquipmentService.FindAll(base.SelectedSite.ID));
            }
            else
            {
                SSList.SetDataSource(new List<HC_OSHA_EQUIPMENT>());
            }

        }



        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;
            LblSite.Text = base.SelectedSite.NAME;
            Search();
        }

        public void Select(IEstimateModel estimateModel)
        {
           // throw new NotImplementedException();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null || base.SelectedSite.ID <= 0)
            {
                MessageUtil.Info("사업장을 선택하세요");
            }
            else
            {
                SSList.AddRows();
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
                    IList<HC_OSHA_EQUIPMENT> list = SSList.GetEditbleData<HC_OSHA_EQUIPMENT>();
                    if (list.Count > 0)
                    {
                        if (hcOshaEquipmentService.Save(list, base.SelectedSite.ID))
                        {
                            //MessageUtil.Info("저장하였습니다");
                            Search();

                            if (statusReportByEngineer != null)
                            {
                                foreach (HC_OSHA_EQUIPMENT dto in list)
                                {
                                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert) 
                                    {
                                        statusReportByEngineer.InsertOption(dto);
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageUtil.Alert("오류가 발생하였습니다. ");

                        }
                    }
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void SetStatusReportByEngineer(StatusReportByEngineer statusReportByEngineer)
        {
            this.statusReportByEngineer = statusReportByEngineer;
        }
    }
}
