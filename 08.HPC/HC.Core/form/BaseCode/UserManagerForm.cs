using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;
using HC.Core.Dto;
using HC.Core.Repository;
using HC.Core.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HC_Core
{
    public partial class UserManagerForm : CommonForm
    {
        private HC_USER selectSignUser = null;
        private HcUserSignRepository hcUserSignRepository;
        HcViewUserService hcViewUserService;
        HcUserService hcUserService;
        List<SpreadComboBoxItem> list = new List<SpreadComboBoxItem>();


        public UserManagerForm()
        {
            InitializeComponent();
            hcViewUserService = new HcViewUserService();
            hcUserService = new HcUserService();
            hcUserSignRepository = new HcUserSignRepository();
        }

   
        private void UserManagerForm_Load(object sender, EventArgs e)
        {
            BtnAdd.Size = new Size(75, 30);
            BtnSave.Size = new Size(75, 30);

            SpreadComboBoxData RoleComboBoxData = codeService.GetSpreadComboBoxData("ROLE", "OSHA");//CommonService.Instance.CodeService.GetComboBoxData("ROLE");
            HcCodeRepository hcCodeRepository = new HcCodeRepository();

            List<HC_CODE> roles = hcCodeRepository.FindActiveCodeByGroupCode("ROLE", "WEM");

            foreach (HC_CODE code in roles)
            {
                RoleComboBoxData.Put(code.Code, code.CodeName);
            }
          
            SSViewUserList.Initialize(new SpreadOption() { IsRowSelectColor = true});
            SSViewUserList.AddColumnCheckBox("", "", 30, new CheckBoxBooleanCellType());
            SSViewUserList.AddColumnText("사번", nameof(HC_VIEW_USER.UserId), 70, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSViewUserList.AddColumnText("이름", nameof(HC_VIEW_USER.Name), 70, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending});
            SSViewUserList.AddColumnText("주민번호", nameof(HC_VIEW_USER.Jumin), 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSViewUserList.AddColumnDateTime("입사일", nameof(HC_VIEW_USER.JoinDate), 90, IsReadOnly.Y, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = false, IsShowCalendarButton = false });
            SSViewUserList.AddColumnText("부서", nameof(HC_VIEW_USER.Dept), 150, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

            List<HC_VIEW_USER> list = hcViewUserService.FinAll();
            SSViewUserList.DataSource = list;

            SSUserList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSUserList.AddColumnText("사번", nameof(HC_USER.UserId), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSUserList.AddColumnText("이름", nameof(HC_USER.Name), 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Descending });
            SSUserList.AddColumnComboBox("역할", nameof(HC_USER.Role), 150, IsReadOnly.N, RoleComboBoxData, new SpreadCellTypeOption { IsSort = false });
            SSUserList.AddColumnText("자격종목번호", nameof(HC_USER.CERTNO), 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Descending });
            SSUserList.AddColumnText("고유키", nameof(HC_USER.SEQ_WORD), 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Descending });
            SSUserList.AddColumnCheckBox("사용여부", nameof(HC_USER.IsActive), 30, new CheckBoxFlagEnumCellType<IsActive>());
            SSUserList.AddColumnDateTime("수정일", nameof(HC_USER.MODIFIED), 90, IsReadOnly.Y, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = false, IsShowCalendarButton = false, dateTimeEditorValue = DateTimeEditorValue.String });
            SSUserList.AddColumnText("수정자", nameof(HC_USER.MODIFIEDUSER), 90, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSUserList.AddColumnButton("삭제", 70, new SpreadCellTypeOption{ButtonText="삭제"}).ButtonClick += SSUserList_DeleteButtonClick;
            SSUserList.AddColumnButton("서명", 70, new SpreadCellTypeOption { ButtonText = "서명" }).ButtonClick += UserManagerForm_ButtonClick;

            SearchUserList();
        }

        private void UserManagerForm_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            selectSignUser = SSUserList.GetRowData(e.Row) as HC_USER;

            SignPadForm form = new SignPadForm(true);
            form.Location = new Point(0, 0);

            if (form.ShowDialog() == DialogResult.OK)
            {
                Save(form.Base64Image);
            }
     

        }
        public void Save(string base64)
        {
            HIC_USERSIGN dto = new HIC_USERSIGN();
            dto.USERID = selectSignUser.UserId;
            dto.IMAGEBASE64 = base64;
            HIC_USERSIGN saved = hcUserSignRepository.FindOne(dto.USERID);
            if (saved == null)
            {
                hcUserSignRepository.Insert(dto);
            }
            else
            {
                hcUserSignRepository.Update(dto);
            }
            MessageUtil.Info("서명이 저장되었습니다");
       
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
            //SSUserList.AddRows();
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

        private void FpSpread1_CellClick(object sender, CellClickEventArgs e)
        {

        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void contentTitle2_Load(object sender, EventArgs e)
        {

        }
    }
}
