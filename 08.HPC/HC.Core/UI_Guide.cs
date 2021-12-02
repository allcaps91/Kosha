using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Spread.CustomCellType;
using ComBase.Mvc.Utils;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;
using HC.Core.Dto;
using HC.Core.Service;
using HC_Core;
using System;
using System.Collections.Generic;

namespace HC.Core
{
    public partial class UI_Guide : CommonForm
    {
        HcCodeService hcCodeService;
        public UI_Guide()
        {
            InitializeComponent();
        }

        private void UI_Guide_Load(object sender, EventArgs e)
        {
            SpreadComboBoxData RoleComboBoxData = codeService.GetSpreadComboBoxData("ROLE", "OSHA");

            SSGroupCodeList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSGroupCodeList.AddColumnText("그룹코드", nameof(HC_CODE.Code), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true });
            SSGroupCodeList.AddColumnText("그룹코드명", nameof(HC_CODE.GroupCodeName), 170, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true });
            SSGroupCodeList.AddColumnText("설명", nameof(HC_CODE.Description), 150, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, IsMulti = false, ICustomCellType = new EmptyTextCellType() });
            SSGroupCodeList.AddColumnNumber("정렬순서", nameof(HC_CODE.SortSeq), 50, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending, ICustomCellType = new SortSeqCellType() });
            SSGroupCodeList.AddColumnCheckBox("사용여부", nameof(HC_CODE.IsActive), 45, new CheckBoxFlagEnumCellType<IsActive>() { IsHeaderCheckBox = false });
            SSGroupCodeList.AddColumnDateTime("수정일시", nameof(HC_CODE.Modified), 120, IsReadOnly.Y, DateTimeType.YYYY_MM_DD_HH_MM, new SpreadCellTypeOption { IsSort = true, IsShowCalendarButton = false });
            SSGroupCodeList.AddColumnComboBox("역할", nameof(HC_USER.Role), 150, IsReadOnly.N, RoleComboBoxData, new SpreadCellTypeOption { IsSort = false });
            
            //문자열날짜
            SSGroupCodeList.AddColumnDateTime("생성일시", nameof(HC_CODE.Modified), 90, IsReadOnly.Y, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Descending, dateTimeEditorValue = DateTimeEditorValue.String });

            SSGroupCodeList.AddColumnText("수정유저", nameof(HC_CODE.ModifiedUser), 80, IsReadOnly.Y);
            SSGroupCodeList.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += UI_Guide_ButtonClick; ;
          //  SearchGroupCodeList();
        }

        //행추가 기본값 세팅하기, 또는 DTO 생성자에서 기본가
        private void BtnAdd_Click(object sender, EventArgs e)
        {

            SSGroupCodeList.AddRows();
            HC_CODE dto = SSGroupCodeList.GetRowData(0) as HC_CODE;
            dto.Code = "Y";
            dto.Description = "xxx";

        }
        private void UI_Guide_ButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            SSGroupCodeList.DeleteRow(e.Row);
        }

        private void BtnCodeSave_Click(object sender, EventArgs e)
        {
            if (SSGroupCodeList.Validate())
            {
                IList<HC_CODE> list = SSGroupCodeList.GetEditbleData<HC_CODE>();
                if (list.Count > 0)
                {

                    //if (hcCodeService.SaveCode("", list))
                    //{
                    //    MessageUtil.Info("제품을 저장하였습니다");
                    //    //SearchProduct();
                    //}
                    //else
                    //{
                    //    MessageUtil.Error("트랜잭션 오류로 저장 할 수 없습니다");
                    //}
                }
                else
                {
                    MessageUtil.Info("저장할 데이타가 없습니다");
                }
            }
        }

    }
}
