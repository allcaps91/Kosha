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
    public partial class GroupCodeForm : CommonForm
    {
        public GroupCodeForm()
        {
            InitializeComponent();
        }

        private void GroupCodeForm_Load(object sender, EventArgs e)
        {
            SSGroupCodeList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSGroupCodeList.AddColumnText("그룹코드", nameof(HC_CODE.Code), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true });
            SSGroupCodeList.AddColumnText("그룹코드명", nameof(HC_CODE.GroupCodeName), 170, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true });
            SSGroupCodeList.AddColumnText("설명", nameof(HC_CODE.Description), 150, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, IsMulti = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left, ICustomCellType = new EmptyTextCellType() });
            SSGroupCodeList.AddColumnNumber("정렬순서", nameof(HC_CODE.SortSeq), 50, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending, ICustomCellType = new SortSeqCellType() });
            SSGroupCodeList.AddColumnCheckBox("사용여부", nameof(HC_CODE.IsActive), 45, new CheckBoxFlagEnumCellType<IsActive>() { IsHeaderCheckBox = false });
            SSGroupCodeList.AddColumnDateTime("수정일시", nameof(HC_CODE.Modified), 120, IsReadOnly.Y, DateTimeType.YYYY_MM_DD_HH_MM, new SpreadCellTypeOption { IsSort = true, IsShowCalendarButton = false });
            SSGroupCodeList.AddColumnText("수정유저", nameof(HC_CODE.ModifiedUser), 80, IsReadOnly.Y);
            SSGroupCodeList.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += GroupCodeDelete_ButtonClick;
            SearchGroupCodeList();
        }
        private void GroupCodeDelete_ButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {

            HC_CODE code = SSGroupCodeList.GetRowData(e.Row) as HC_CODE;
           
            SSGroupCodeList.DeleteRow(e.Row);

        }

        private void BtnGroupCodeSave_Click(object sender, EventArgs e)
        {
            if (SSGroupCodeList.Validate())
            {
                IList<HC_CODE> list = SSGroupCodeList.GetEditbleData<HC_CODE>();
                if (list.Count > 0)
                {
                    if (codeService.SaveGroupCode(list))
                    {
                        MessageUtil.Info("그룹코드를 저장하였습니다");
                        SearchGroupCodeList();
                    }
                    else
                    {
                        MessageUtil.Error("트랜잭션 오류로 저장 할 수 없습니다");
                    }
                }
                else
                {
                    MessageUtil.Alert("저장할 그룹코드가 없습니다");
                }

            }
        }

        private void BtnGroupCodeAdd_Click(object sender, EventArgs e)
        {
            SSGroupCodeList.AddRows();
        }
        public void SearchGroupCodeList()
        {
            SSGroupCodeList.SetDataSource(codeService.FindGroupCodeAll());
        }
    }
}
