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
    public class ColumnNumber : AbstractColumn
    {

        public ColumnNumber(FpSpread fpSpread, string caption, string dataField, int width, SpreadCellTypeOption option = null) : base(fpSpread, dataField, caption, width, option)
        {
            NumberCellType numberCellType;
            if (option.ICustomCellType != null)
            {
                numberCellType = (NumberCellType)option.ICustomCellType;
                (numberCellType as ICustomCellType).fpspread = fpSpread;
            }
            else
            {
                numberCellType = new NumberCellType();
            }

            if (option.ICustomCellType != null)
            {
                numberCellType = (NumberCellType)option.ICustomCellType;
            }
            else
            {
                numberCellType = new NumberCellType();
            }
            numberCellType.Separator = ",";
            numberCellType.ShowSeparator = true;
            numberCellType.DecimalPlaces = option.DecimalPlaces;
            column.CellType = numberCellType;
            
        }

        //public override void SetCellTypeValidation(MTSValidation mtsValitaion)
        //{
        //    //throw new NotImplementedException();
        //}
    }
}
