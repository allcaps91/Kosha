using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Spread
{
    public interface ICustomCellType
    {
        FpSpread fpspread { get; set; }
        void PaintCell(Graphics g, Rectangle r, Appearance appearance, object value, bool isSelected, bool isLocked, float zoomFactor);
        
    }
}
