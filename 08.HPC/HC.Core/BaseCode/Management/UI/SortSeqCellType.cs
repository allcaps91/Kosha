using ComBase.Mvc.Spread;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Core.BaseCode.Management.UI
{
    public class SortSeqCellType : NumberCellType, ICustomCellType
    {
        public FpSpread fpspread { get; set; }
        public override void PaintCell(Graphics g, Rectangle r, Appearance appearance, object value, bool isSelected, bool isLocked, float zoomFactor)
        {
            if (value.ToString().Equals("0"))
            {
                appearance.BackColor = Color.Red;
            }
            else
            {
                appearance.BackColor = Color.ForestGreen;
            }

            base.PaintCell(g, r, appearance, value, isSelected, isLocked, zoomFactor);
        }
    }
}
