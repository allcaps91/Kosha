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
    
    public class CheckBoxBooleanCellType : CheckBoxCellType, ICheckBoxCellType
    {
        public bool IsHeaderCheckBox { get; set; }
        public CheckBoxBooleanCellType()
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
     

    }
}
