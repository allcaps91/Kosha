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
    public class CheckBoxFlagEnumCellType<T> : CheckBoxCellType, ICheckBoxCellType
    {
        public bool IsHeaderCheckBox { get; set; }
        public CheckBoxFlagEnumCellType()
        {
            this.IsHeaderCheckBox = false;
        }
        public CheckBoxFlagEnumCellType(bool isHeaderCheckbox)
        {
            this.IsHeaderCheckBox = isHeaderCheckbox;
        }
        public void AllChecked(FpSpread fpSpread, int columnIndex)
        {
            for (int i = 0; i < fpSpread.ActiveSheet.RowCount; i++)
            {
                fpSpread.ActiveSheet.Cells[i, columnIndex].Value = (T)Enum.ToObject(typeof(T), 0); //enum y
            }
        }
        public void AllUnChecked(FpSpread fpSpread, int columnIndex)
        {
            for (int i = 0; i < fpSpread.ActiveSheet.RowCount; i++)
            {
                fpSpread.ActiveSheet.Cells[i, columnIndex].Value = (T)Enum.ToObject(typeof(T), 1); //enum n
            }
        }
        public override object GetEditorValue()
        {
            object value = base.GetEditorValue();
            if (value is bool)
            {
                if ((bool)value == true)
                {
                    value = (T)Enum.ToObject(typeof(T), 0); //enum y
                }
                else
                {
                    value = (T)Enum.ToObject(typeof(T), 1); //enum n
                }

            }
            return value;
        }
        public override void SetEditorValue(object value)
        {
           
            base.SetEditorValue(ConvertToBoolean(value));
            
        }

        //private T ConvertToEnum<T>(object value)
        //{
        //    T enumVal = (T)Enum.ToObject(typeof(T), value);
        //    return enumVal;
        //}
        private object ConvertToBoolean(object value)
        {
            //체크박스의 enum은 0이 y이고  true여야 한다.
            if(value == null)
            {
                return false;
            }
            T t = (T)Enum.Parse(typeof(T), value.ToString());
            if (t.Equals((T)Enum.ToObject(typeof(T), 0))) //Y
            {
                value = true;
            }
            else
            {
                value = false;
            }
            //if (value.GetType().IsEnum)
            //{
            //    if (value.Equals((T)Enum.ToObject(typeof(T), 0)))
            //    {
            //        value = true;
            //    }
            //    else
            //    {
            //        value = false;
            //    }
            //}
            return value;
        }
        public override void PaintCell(Graphics g, Rectangle r, Appearance appearance, object value, bool isSelected, bool isLocked, float zoomFactor)
        {
            base.PaintCell(g, r, appearance, ConvertToBoolean(value), isSelected, isLocked, zoomFactor);
        }
        
    }
}
