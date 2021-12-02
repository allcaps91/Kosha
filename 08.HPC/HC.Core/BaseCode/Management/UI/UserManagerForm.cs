using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;
using HC.Core.BaseCode.Management.Dto;
using HC.Core.BaseCode.Management.Service;
using HC.Core.Common.Service;
using HC.Core.Common.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC.Core.BaseCode.Management.UI
{
    public partial class UserManagerForm : CommonForm
    {
        
        HcViewUserService hcViewUserService;
        HcUserService hcUserService;
        List<SpreadComboBoxItem> list = new List<SpreadComboBoxItem>();

        public UserManagerForm()
        {
            InitializeComponent();
            hcViewUserService = new HcViewUserService();
            hcUserService = new HcUserService();
        }

   
        private void UserManagerForm_Load(object sender, EventArgs e)
        {
            SpreadComboBoxData RoleComboBoxData = codeService.GetSpreadComboBoxData("ROLE");//CommonService.Instance.CodeService.GetComboBoxData("ROLE");

            SSViewUserList.Initialize(new SpreadOption() { IsRowSelectColor = true});
            
            SSViewUserList.AddColumnCheckBox("선택", "", 30, new CheckBoxBooleanCellType());
            SSViewUserList.AddColumnText("사번", nameof(HC_VIEW_USER.UserId), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSViewUserList.AddColumnText("이름", nameof(HC_VIEW_USER.Name), 150, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending});
            SSViewUserList.AddColumnText("주민번호", nameof(HC_VIEW_USER.Jumin), 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSViewUserList.AddColumnDateTime("입사일", nameof(HC_VIEW_USER.JoinDate), 90, IsReadOnly.Y, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = false, IsShowCalendarButton = false });
            SSViewUserList.AddColumnText("부서", nameof(HC_VIEW_USER.Dept), 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

            SSViewUserList.DataSource = hcViewUserService.FinAll();



            SSUserList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSUserList.AddColumnText("사번", nameof(HC_USER.UserId), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSUserList.AddColumnText("이름", nameof(HC_USER.Name), 150, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Descending });
            SSUserList.AddColumnComboBox("역할", nameof(HC_USER.Role), 150, IsReadOnly.N, RoleComboBoxData, new SpreadCellTypeOption { IsSort = false });
            SSUserList.AddColumnCheckBox("사용여부", nameof(HC_USER.IsActive), 50, new CheckBoxFlagEnumCellType<IsActive>());
            SSUserList.AddColumnDateTime("수정일", nameof(HC_USER.MODIFIED), 90, IsReadOnly.Y, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = false, IsShowCalendarButton = false, dateTimeEditorValue = DateTimeEditorValue.String });
            SSUserList.AddColumnText("수정자", nameof(HC_USER.MODIFIEDUSER), 90, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSUserList.AddColumnButton("삭제", 70, new SpreadCellTypeOption{ButtonText="삭제"}).ButtonClick += SSUserList_DeleteButtonClick;
         
            SearchUserList();

           // SSUserList.EditorNotify += new FarPoint.Win.Spread.EditorNotifyEventHandler(fpSpread1_ButtonClicked);



        }
        private void SearchUserList()
        {
            SSUserList.SetDataSource(hcUserService.FindAll());
        }
        private void SSUserList_DeleteButtonClick(object sender, EditorNotifyEventArgs e)
        {
   
            SSUserList.DeleteRow(e.Row);
        }
      


        private void BtnAdd_Click(object sender, EventArgs e)
        {
            //  SSUserList.AddRows();
            for(int i=0; i<SSViewUserList.RowCount(); i++)
            {
                CheckBoxCellType c = SSViewUserList.ActiveSheet.Cells[i, 0].Column.CellType as CheckBoxCellType;
                object value = SSViewUserList.ActiveSheet.Cells[i, 0].Value;
                if (value != null)
                {
                    if (bool.Parse(value.ToString()) == true)
                    {

                        HC_VIEW_USER viewUser = SSViewUserList.GetRowData(i) as HC_VIEW_USER;
                        hcUserService.Create(viewUser);
                    }
                }
                
            }
            SearchUserList();

            SSViewUserList.DataSource = hcViewUserService.FinAll();

        }



        private void BtnSave_Click_1(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SSUserList.AddRows();
        }

   

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            foreach (HC_USER user in SSUserList.GetEditbleData<HC_USER>())
            {
               
                    hcUserService.Save(user);
                
            }
            SearchUserList();
        }

        private void CboRole_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
