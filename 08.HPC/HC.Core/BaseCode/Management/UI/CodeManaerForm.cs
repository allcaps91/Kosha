using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Spread.CustomCellType;
using ComBase.Mvc.Utils;
using ComBase.Mvc.Validation;
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
    public partial class CodeManaerForm : CommonForm
    {
        
        public CodeManaerForm()
        {
            InitializeComponent();
           
        }

        private void CodeManaerForm_Load(object sender, EventArgs e)
        {
           
            SSGroupCodeList.Initialize(new SpreadOption() { IsRowSelectColor = true});
            SSGroupCodeList.AddColumnText("그룹코드", nameof(HC_CODE.Code), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSGroupCodeList.AddColumnText("그룹코드명", nameof(HC_CODE.GroupCodeName), 170, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left});
            SSGroupCodeList.AddColumnText("설명", nameof(HC_CODE.Description), 150, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, IsMulti = false, ICustomCellType = new EmptyTextCellType(), Aligen= FarPoint.Win.Spread.CellHorizontalAlignment.Left });
           
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
            if (SSCodeList.Validate())
            {
                HC_CODE group = SSGroupCodeList.GetCurrentRowData() as HC_CODE;
                IList<HC_CODE> list = SSCodeList.GetEditbleData<HC_CODE>();
                if (list.Count > 0)
                {
                    if (codeService.SaveCode(group.Code, list))
                    {
                        MessageUtil.Info("코드를 저장하였습니다");
                        SearchCodeList(group.Code);
                    }
                    else
                    {
                        MessageUtil.Error("트랜잭션 오류로 저장 할 수 없습니다");
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
            SSGroupCodeList.DataSource = codeService.FindGroupCodeAll();
        }
        public void SearchCodeList(string groupCode)
        {
            SSCodeList.ActiveSheet.RowCount = 0;
            SSCodeList.SetDataSource(codeService.FindAllByGroupCode(groupCode));
        }

    }
}
