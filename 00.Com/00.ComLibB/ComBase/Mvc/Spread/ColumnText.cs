using ComBase.Controls;
using ComBase.Mvc.Enums;
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
    public class ColumnText : AbstractColumn
    {

        
        public ColumnText(FpSpread fpSpread, string caption, string dataField, int width, IsReadOnly isReadOnly, SpreadCellTypeOption option = null) : base(fpSpread, dataField, caption, width, option)
        {
            TextCellType textCellType = null;
            if (option.ICustomCellType != null)
            {
               
                textCellType = (TextCellType)option.ICustomCellType;
                (textCellType as ICustomCellType).fpspread = fpSpread;

            }
            else
            {
                textCellType = new TextCellType();
            }

            
            if (isReadOnly == IsReadOnly.Y)
            {
                textCellType.ReadOnly = true;
                column.Locked = true;
            }
            else
            {
               
                textCellType.ReadOnly = false;
            }

            textCellType.Multiline = option.IsMulti;
            textCellType.WordWrap = option.WordWrap;
            
            
            column.CellType = textCellType;
        ///    column.MergePolicy = option.mergePolicy;
        }

    }
}
