using ComBase.Controls;
using ComBase.Mvc.Utils;
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
    public class ColumnDateTimeCellType : DateTimeCellType, ICustomCellType 
    {
        public FpSpread fpspread { get; set; }
        public DateTimeType dateTimeType { get; set; }
        public override object GetEditorValue()
        {
            object value = base.GetEditorValue();
            if (value is string)
            {
                // DateUtil.stringToDateTime(value.ToString(), Enums.MTSDateTimeType.YYYY_MM_DD);
                //value = "20171010";
            }
            return value;
        }
        public override void SetEditorValue(object value)
        {
            if (value is string)
            {
                //value = DateUtil.stringToDateTime(value.ToString(), DateTimeType.YYYYMMDD);

            }
            base.SetEditorValue((value));
        }

        public override void PaintCell(Graphics g, Rectangle r, Appearance appearance, object value, bool isSelected, bool isLocked, float zoomFactor)
        {
            if (value != null)
            {
                if (value.ToString().Length == 8)
                {
                    value = DateUtil.stringToDateTime(value.ToString(), DateTimeType.YYYYMMDD);
                }
                else if (value.ToString().Length == 12)
                {
                    value = DateUtil.stringToDateTime(value.ToString(), DateTimeType.YYYYMMDDHHMM);
                }                
            }
            
            base.PaintCell(g, r, appearance, value, isSelected, isLocked, zoomFactor);            
        }

    }
}
