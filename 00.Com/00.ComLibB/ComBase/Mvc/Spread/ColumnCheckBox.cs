using ComBase.Controls;
using ComBase.Mvc.Validation;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Spread
{
    public class ColumnCheckBox : AbstractColumn
    {
        private ICheckBoxCellType checkBoxCellType;
        public event EditorNotifyEventHandler ButtonClick;

        public ColumnCheckBox(FpSpread fpSpread, string caption,  string dataField, int width, ICheckBoxCellType checkBoxCellType, SpreadCellTypeOption option = null) : base(fpSpread, dataField, caption, width, option)
        {

            this.checkBoxCellType = checkBoxCellType;
            
            column.CellType = (CheckBoxCellType)checkBoxCellType;
            column.CellType.SetEditorValue("N");
            if (checkBoxCellType.IsHeaderCheckBox)
            {
               
                fpSpread.ActiveSheet.OperationMode = OperationMode.Normal;
                fpSpread.SelectionBlockOptions = SelectionBlockOptions.None;
                CheckBoxCellType headerCheckboxType = new CheckBoxCellType();
            
                headerCheckboxType.Caption = caption;


                fpSpread.ActiveSheet.ColumnHeader.Cells[0, column.Index].CellType = headerCheckboxType;


                 fpSpread.ActiveSheet.ColumnHeader.Cells[0, column.Index].VerticalAlignment = CellVerticalAlignment.Center;
                fpSpread.ActiveSheet.ColumnHeader.Cells[0, column.Index].HorizontalAlignment = CellHorizontalAlignment.Center;

                fpSpread.CellClick += FpSpread_CellClick;
            }
 
            fpSpread.ButtonClicked += FpSpread_ButtonClicked;
        }

        /// <summary>
        /// 헤더 체크박스 이벤트 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FpSpread_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader && e.Column == column.Index)
            {
                ICheckBoxCellType headerCheckBoxCellType = checkBoxCellType;
                bool isChecked = false;
                if(fpSpread.ActiveSheet.ColumnHeader.Cells[0, column.Index].Value == null || fpSpread.ActiveSheet.ColumnHeader.Cells[0, column.Index].Value.Equals(true))
                {
                    isChecked = false;
                    headerCheckBoxCellType.AllUnChecked(fpSpread, e.Column);
                }
                else
                {
                    isChecked = true;
                    headerCheckBoxCellType.AllChecked(fpSpread, e.Column);
                }

                fpSpread.ActiveSheet.ColumnHeader.Cells[0, column.Index].Value = isChecked;

               
            }
        }

        //체크박스 클릭 이벤크 
        private void FpSpread_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            if (column.Index == e.Column)
            {
                ButtonClick?.Invoke(sender, e);
            }

        }

        //public override void SetCellTypeValidation(MTSValidation mtsValitaion)
        //{
        //    column.Tag = mtsValitaion;
        //}
    }
}
