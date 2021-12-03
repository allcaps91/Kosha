using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using FarPoint.Win.Spread;
using HC.Core.Dto;
using HC.Core.Service;
using HC.Core.Common.Interface;
using HC_Core;
using HC.Core.Model;
using HC.OSHA.Dto;
using HC.OSHA.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComHpcLibB.Model;

namespace HC_OSHA
{
    public partial class EducationForm : CommonForm, ISelectSite
    {
        HcOshaVisitEduService hcOshaVisitEduService;
        public EducationForm()
        {
            InitializeComponent();
            hcOshaVisitEduService = new HcOshaVisitEduService();
        }

        private void EducationReportForm_Load(object sender, EventArgs e)
        {
         
            SpreadComboBoxData EduTypeComboBoxData = codeService.GetSpreadComboBoxData("VISIT_EDU_TYPE", "OSHA");
            SpreadComboBoxData EduUserIdComboBoxData = userService.GetSpreadByOsha();

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSList.AddColumnDateTime("교육일자", nameof(HC_OSHA_VISIT_EDU.EDUDATE), 120, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = true, IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String  });
            SSList.AddColumnComboBox("종류", nameof(HC_OSHA_VISIT_EDU.EDUTYPE), 100, IsReadOnly.N, EduTypeComboBoxData, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("교육주제", nameof(HC_OSHA_VISIT_EDU.TITLE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("참석자", nameof(HC_OSHA_VISIT_EDU.TARGET), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("교육장소", nameof(HC_OSHA_VISIT_EDU.LOCATION), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("실시자", nameof(HC_OSHA_VISIT_EDU.EDUUSERNAME), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnComboBox("실시자", nameof(HC_OSHA_VISIT_EDU.EDUUSERID), 100, IsReadOnly.N, EduUserIdComboBoxData, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnButton(" ", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += (s, ev) => { SSList.DeleteRow(ev.Row); };
            SSList.AddColumnButton(" ", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += EducationReportForm_ButtonClick;

            Search();
        }

        private void EducationReportForm_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            SSList.DeleteRow(e.Row);
        }

        private void Search()
        {
            if (base.SelectedSite != null)
            {
                SSList.SetDataSource(hcOshaVisitEduService.FindAll(base.SelectedSite.ID));
            }
            else
            {
                SSList.SetDataSource(new List<HC_OSHA_VISIT_EDU>());
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if(base.SelectedSite == null)
            {
                MessageUtil.Info("사업장을 선택하세요");
            }
            else
            {
                if (SSList.Validate())
                {
                    IList<HC_OSHA_VISIT_EDU> list = SSList.GetEditbleData<HC_OSHA_VISIT_EDU>();
                    if (list.Count > 0)
                    {
                        if (hcOshaVisitEduService.Save(list, base.SelectedSite.ID))
                        {
                          //  MessageUtil.Info("저장하였습니다");
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

        void ISelectSite.Select(ISiteModel siteModel)
        {
            Search();
        }
    }
}

