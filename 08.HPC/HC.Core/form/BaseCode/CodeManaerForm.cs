using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using FarPoint.Win.Spread.Model;
using HC.Core.Dto;
using System;
using System.Collections.Generic;

namespace HC_Core
{
    public partial class CodeManaerForm : CommonForm
    {
        
        public CodeManaerForm()
        {
            InitializeComponent();
           
        }

        private void CodeManaerForm_Load(object sender, EventArgs e)
        {
            CboProgram.Items.Add("COM.공통");
            CboProgram.Items.Add("WEM.작업환경측정");
            CboProgram.Items.Add("OSHA.보건관리전문");
            CboProgram.SelectedIndex = 0;
            SSGroupCodeList.Initialize(new SpreadOption() { IsRowSelectColor = true});
            SSGroupCodeList.AddColumnText("그룹코드", nameof(HC_CODE.Code), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSGroupCodeList.AddColumnText("그룹코드명", nameof(HC_CODE.GroupCodeName), 170, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left});
            SSGroupCodeList.AddColumnText("설명", nameof(HC_CODE.Description), 150, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, IsMulti = false, Aligen= FarPoint.Win.Spread.CellHorizontalAlignment.Left });
           
            SearchGroupCodeList();

            SSCodeList.Initialize(new SpreadOption());
            SSCodeList.AddColumnText("코드", nameof(HC_CODE.Code), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true });
            SSCodeList.AddColumnText("코드명", nameof(HC_CODE.CodeName), 170, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true });
            SSCodeList.AddColumnText("설명", nameof(HC_CODE.Description), 150, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, IsMulti = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSCodeList.AddColumnNumber("정렬순서", nameof(HC_CODE.SortSeq), 50, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            SSCodeList.AddColumnCheckBox("사용여부", nameof(HC_CODE.IsActive), 100, new CheckBoxFlagEnumCellType<IsActive>() { IsHeaderCheckBox = true });
            SSCodeList.AddColumnDateTime("수정일시", nameof(HC_CODE.Modified), 120, IsReadOnly.Y, DateTimeType.YYYY_MM_DD_HH_MM, new SpreadCellTypeOption { IsSort = true, IsShowCalendarButton = false });
            SSCodeList.AddColumnText("수정유저", nameof(HC_CODE.ModifiedUser), 80, IsReadOnly.Y);
            SSCodeList.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += CodeDelete_ButtonClick; ;
            // SearchCodeList(string.Empty);
            
            //Initialize 제네릭으로 변경할것..
            SSCodeList.DataSource = new List<HC_CODE>();
        }

        private void CodeDelete_ButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {

            HC_CODE code = SSCodeList.GetRowData(e.Row) as HC_CODE;
            if(code.Code == null)
            {
                SSCodeList.DeleteRow(e.Row);
            }
            else
            {
             
                SSCodeList.DeleteRow(e.Row);
            }
          
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
           
            //SSGroupCodeList.SearchWithDialogAdvanced("dks");
            //    SSGroupCodeList.Search(0, "ABC", false, false, false, false, 0, 0,);
        }

    
     

        private void SSGroupCodeList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            HC_CODE code = SSGroupCodeList.GetRowData(e.Row) as HC_CODE;
            SSGroupCodeList.ActiveSheet.AddSelection(e.Row, 0, 1, 3);
            SearchCodeList(code.Code);

        }

        private void BtnCodeAdd_Click(object sender, EventArgs e)
        {
            SSCodeList.AddRows();
        }

        private void BtnCodeSave_Click(object sender, EventArgs e)
        {
            //MessageUtil.Alert("xx");
            if (SSCodeList.Validate())
            {
                HC_CODE group = SSGroupCodeList.GetCurrentRowData() as HC_CODE;
                IList<HC_CODE> list = SSCodeList.GetEditbleData<HC_CODE>();
                if (list.Count > 0)
                {
                    string program = CboProgram.Text.Split('.')[0];
                    int result = codeService.SaveCode(group.Code, program, list);
                    if(result == 0){
                     //   MessageUtil.Info("코드를 저장하였습니다");
                        SearchCodeList(group.Code);
                    }
                    else if (result == 2)
                    {
                        
                        MessageUtil.Error("트랜잭션 오류로 저장 할 수 없습니다");
                    }
                    else
                    {
                        MessageUtil.Error("코드 또는 코드명이 중복입니다.");
                    }
                }
                else
                {
                    MessageUtil.Info("저장할 코드가 없습니다");
                }
            }
        }

        public void SearchGroupCodeList()
        {
            string program = CboProgram.Text.Split('.')[0];
            SSGroupCodeList.DataSource = codeService.FindGroupCodeAll(program);
        }
        public void SearchCodeList(string groupCode)
        {
            string program = CboProgram.Text.Split('.')[0];

            SSCodeList.ActiveSheet.RowCount = 0;
            SSCodeList.SetDataSource(codeService.FindAllByGroupCode(groupCode, program));
        }
     

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CboProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchGroupCodeList();
        }
    }
}
