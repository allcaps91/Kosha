using ComBase.Controls;
using ComBase.Mvc.Validation;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ComBase.Mvc.Spread
{
    public class ColumnImage : AbstractColumn
    {
        public ColumnImage(FpSpread fpSpread, string caption, string dataField, int width, SpreadCellTypeOption option = null) : base(fpSpread, dataField, caption, width, option)
        {
            ImageCellType cellType = new ImageCellType();        

            column.CellType = cellType;            
            
        }

    }
}
