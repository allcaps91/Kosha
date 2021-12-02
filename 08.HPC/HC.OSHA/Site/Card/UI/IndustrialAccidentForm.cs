using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Exceptions;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using HC.Core.BaseCode.Management.Dto;
using HC.Core.BaseCode.Management.Repository;
using HC.Core.BaseCode.Management.Service;
using HC.Core.Common.Extension;
using HC.Core.Common.Interface;
using HC.Core.Common.Service;
using HC.Core.Common.UI;
using HC.Core.Site.Dto;
using HC.Core.Site.Model;
using HC.Core.Site.UI;
using HC.OSHA.Site.Card.Dto;
using HC.OSHA.Site.Card.Service;
using HC.OSHA.Site.Management.Dto;
using HC.OSHA.Site.Management.Model;
using HC.OSHA.Site.Management.Service;
using HC.OSHA.Visit.Management.UI;
using HC.OSHA.Visit.Schedule.Dto;
using HC.OSHA.Visit.Schedule.Model;
using HC.OSHA.Visit.Schedule.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;


namespace HC.OSHA.Site.Card.UI
{
    public partial class IndustrialAccidentForm : CommonForm, ISelectSite, ISelectEstimate
    {
        HcOshaCard6Service hcOshaCard6Service;
        public IndustrialAccidentForm()
        {
            InitializeComponent();
            hcOshaCard6Service = new HcOshaCard6Service();
        }
        private void IndustrialAccidentForm_Load(object sender, EventArgs e)
        {
            SpreadComboBoxData SITE_CARD_ACCIDENT_TYPE = codeService.GetSpreadComboBoxData("SITE_CARD_ACCIDENT_TYPE","OSHA");
            SpreadComboBoxData users = userService.GetSpreadByOsha();


            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSList.AddColumnDateTime("발생일자", nameof(HC_OSHA_CARD6.ACC_DATE), 120, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = false, IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            SSList.AddColumnComboBox("산재구분", nameof(HC_OSHA_CARD6.IND_ACC_TYPE), 100, IsReadOnly.N, SITE_CARD_ACCIDENT_TYPE, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("이름", nameof(HC_OSHA_CARD6.NAME), 100, IsReadOnly.N,  new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("주민번호", nameof(HC_OSHA_CARD6.JUMIN_NO), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("산재요양신청일", nameof(HC_OSHA_CARD6.REQUEST_DATE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("승인일자", nameof(HC_OSHA_CARD6.APPROVE_DATE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("상병", nameof(HC_OSHA_CARD6.ILLNAME), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("재해발생경위", nameof(HC_OSHA_CARD6.REMARK), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            
            SSList.AddColumnButton(" ", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += (s, ev) => { SSList.DeleteRow(ev.Row); };


            Search();
        }

        private void Search()
        {
            if (base.SelectedSite != null && base.SelectedEstimate!=null)
            {
                SSList.SetDataSource(hcOshaCard6Service.FindAll(base.SelectedEstimate.ID));
            }
            else
            {
                SSList.SetDataSource(new List<HC_OSHA_CARD6>());
            }

        }

    
        private void Button1_Click(object sender, EventArgs e)
        {
            MailUtil util = new MailUtil("faye12005@gmail.com", "love!@0223");
            util.SenderMailAddress = "faye12005@gmail.com";
            util.ReciverMailSddress.Add("faye12005@gmail.com");

            util.Subject = "ddddddddd";
            util.Body = "Ssss";
            util.SendMaill();
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
                    IList<HC_OSHA_CARD6> list = SSList.GetEditbleData<HC_OSHA_CARD6>();
                    if (list.Count > 0)
                    {
                        if (hcOshaCard6Service.Save(list, base.SelectedEstimate.ID))
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
