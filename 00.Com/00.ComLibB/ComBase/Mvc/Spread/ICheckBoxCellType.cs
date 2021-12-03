using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Spread
{
    public interface ICheckBoxCellType
    {
        bool IsHeaderCheckBox { get; set; } 
        void AllChecked(FpSpread fpSpread, int columnIndex);
        void AllUnChecked(FpSpread fpSpread, int columnIndex);
       
    }
}
