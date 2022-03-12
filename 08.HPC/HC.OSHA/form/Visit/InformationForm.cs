using ComBase;
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
    public partial class InformationForm : CommonForm, ISelectSite
    {
        HcOshaVisitInformationService hcOshaVisitInformationService;

        public delegate void OnInformationSelectedHandle(HC_OSHA_VISIT_INFORMATION item);
        public event OnInformationSelectedHandle OnSelected;
        public string strRETURN="";  //폼으로 전달할 값
        bool IsPopup = false;

         //HC_OSHA_VISIT_INFORMATION
        public InformationForm()
        {
            InitializeComponent();
            hcOshaVisitInformationService = new HcOshaVisitInformationService();
        }

        public InformationForm(bool ispopup)
        {
            InitializeComponent();
            hcOshaVisitInformationService = new HcOshaVisitInformationService();

            IsPopup = ispopup;
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
              int row = SSList.AddRows();
              SSList.ActiveSheet.Cells[row, 4].Value = CommonService.Instance.Session.UserId;
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
            SSList.AddColumnDateTime("제공일자", nameof(HC_OSHA_VISIT_INFORMATION.REGDATE), 120, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = true, IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            SSList.AddColumnComboBox("정보구분", nameof(HC_OSHA_VISIT_INFORMATION.INFORMATIONTYPE), 100, IsReadOnly.N, INFORMATIONTYPE, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("내용", nameof(HC_OSHA_VISIT_INFORMATION.REMARK), 310, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });            
            SSList.AddColumnText("발급자", nameof(HC_OSHA_VISIT_INFORMATION.REGUSERNAME), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnCheckBox("", "", 30, new CheckBoxBooleanCellType());
            SSList.AddColumnComboBox("발급자", nameof(HC_OSHA_VISIT_INFORMATION.REGUSERID), 100, IsReadOnly.N, users, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnButton(" ", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += (s, ev) => { SSList.DeleteRow(ev.Row); };
            if (IsPopup)
            {
                SSList.AddColumnButton(" ", 60, new SpreadCellTypeOption { ButtonText = "적용" }).ButtonClick += (s, ev) =>
                {
                    if (OnSelected != null)
                    {
                        OnSelected(SSList.GetCurrentRowData() as HC_OSHA_VISIT_INFORMATION);
                    }
                };
            }

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

        private void SSList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if(OnSelected != null)
            {
                OnSelected(SSList.GetCurrentRowData() as HC_OSHA_VISIT_INFORMATION);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!IsPopup) return;

            strRETURN = "";
            for (int i=0; i< SSList.ActiveSheet.RowCount; i++)
            {
                if (Convert.ToBoolean(SSList.ActiveSheet.Cells[i, 4].Value) == true)
                {
                    strRETURN += SSList.ActiveSheet.Cells[i, 2].Text.Trim() + "/";
                }
            }
        }

        private void BtnAdd1_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null)
            {
                MessageUtil.Info("사업장을 선택하세요");
                return;
            }

            int row = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strData = "";
            string strNow = DateTime.Now.ToString("yyyy-MM-dd");

            SQL = "SELECT CODE,CODENAME FROM HIC_CODES ";
            SQL = SQL + ComNum.VBLF + "WHERE GROUPCODE='VISIT_INFORMATION_DETAIL' ";
            SQL = SQL + ComNum.VBLF + "  AND ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND ISACTIVE='Y' ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY CODE ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strData = dt.Rows[i]["CODENAME"].ToString().Trim();
                    if (strData != "")
                    {
                        row = SSList.AddRows();
                        SSList.ActiveSheet.Cells[row, 0].Value = strNow;
                        SSList.ActiveSheet.Cells[row, 1].Text = VB.Pstr(strData, "{}", 1);
                        SSList.ActiveSheet.Cells[row, 2].Value = VB.Pstr(strData, "{}", 2);
                        SSList.ActiveSheet.Cells[row, 3].Value = clsType.User.JobName;
                        SSList.ActiveSheet.Cells[row, 4].Value = true;
                        SSList.ActiveSheet.Cells[row, 5].Value = CommonService.Instance.Session.UserId;
                    }
                }
            }
            dt.Dispose();
            dt = null;
        }
    }
}
