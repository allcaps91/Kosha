using ComBase.Controls;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Spread.CustomCellType
{
    public class EmptyTextCellType : TextCellType, ICustomCellType
    {
        public FpSpread fpspread { get; set;}
        
        public override void PaintCell(Graphics g, Rectangle r, Appearance appearance, object value, bool isSelected, bool isLocked, float zoomFactor)
        {
            if (value.IsNullOrEmpty())
            {
                appearance.BackColor = Color.Red;
            }
            Log.Debug("row:" + fpspread.ActiveSheet.ActiveRowIndex);
            base.PaintCell(g, r, appearance, value, isSelected, isLocked, zoomFactor);
        }
    }
}
