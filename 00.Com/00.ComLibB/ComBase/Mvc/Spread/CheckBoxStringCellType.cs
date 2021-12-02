
using ComBase.Controls;
using ComBase.Mvc.Enums;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ComBase.Mvc.Spread
{

    public class CheckBoxStringCellType : CheckBoxCellType, ICheckBoxCellType
    {
        public bool IsHeaderCheckBox { get; set; }
        public string CheckedValue { get; set; }
        public string UnCheckedValue { get; set; }

        public CheckBoxStringCellType()
        {
            this.IsHeaderCheckBox = true;
        }
        public void AllChecked(FpSpread fpSpread, int columnIndex)
        {
            for (int i = 0; i < fpSpread.ActiveSheet.RowCount; i++)
            {
                fpSpread.ActiveSheet.Cells[i, columnIndex].Value = true;
            }
        }
        public void AllUnChecked(FpSpread fpSpread, int columnIndex)
        {
            for (int i = 0; i < fpSpread.ActiveSheet.RowCount; i++)
            {
                fpSpread.ActiveSheet.Cells[i, columnIndex].Value = false;
            }
        }

        public override object GetEditorValue()
        {
            object value = base.GetEditorValue();
            if(value.IsNullOrEmpty())
            {
                value = UnCheckedValue;
            }
            else
            {
                if (value is bool)
                {
                    if ((bool)value == true)
                    {
                        value = CheckedValue;
                    }
                    else
                    {
                        value = UnCheckedValue;
                    }

                }
            }
            
            return value;
        }
        public override void SetEditorValue(object value)
        {

            base.SetEditorValue(ConvertToBoolean(value));

        }
        private object ConvertToBoolean(object value)
        {
            if (value == null)
            {
                return false;
            }
            if (value.Equals(CheckedValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void PaintCell(Graphics g, Rectangle r, Appearance appearance, object value, bool isSelected, bool isLocked, float zoomFactor)
        {
            base.PaintCell(g, r, appearance, ConvertToBoolean(value), isSelected, isLocked, zoomFactor);
        }


    }
}
