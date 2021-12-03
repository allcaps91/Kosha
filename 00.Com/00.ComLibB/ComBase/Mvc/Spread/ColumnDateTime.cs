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

    public class ColumnDateTime : AbstractColumn
    {
        public ColumnDateTime(FpSpread fpSpread, string caption, string dataField, int width, IsReadOnly isReadOnly, DateTimeType dateTimeType, SpreadCellTypeOption option = null) : base(fpSpread, dataField, caption, width, option)
        {

            ColumnDateTimeCellType dateTimeCellType;
            if (option.ICustomCellType != null)
            {
                dateTimeCellType = (ColumnDateTimeCellType)option.ICustomCellType;
                (dateTimeCellType as ICustomCellType).fpspread = fpSpread;
            }
            else
            {
                dateTimeCellType = new ColumnDateTimeCellType();
            }
            dateTimeCellType.dateTimeType = dateTimeType;
    //       dateTimeCellType.DateTimeFormat = DateTimeFormat.UserDefined;
    //        dateTimeCellType.UserDefinedFormat = dateTimeType.GetEnumDescription();

            //날짜데이타형식
            dateTimeCellType.EditorValue = option.dateTimeEditorValue;
            dateTimeCellType.DropDownButton = option.IsShowCalendarButton;

            if(isReadOnly == IsReadOnly.Y)
            {
                dateTimeCellType.ReadOnly = true;
                column.Locked = true;
            }
            else
            {
                dateTimeCellType.ReadOnly = false;
            }
            
            column.CellType = dateTimeCellType;
            
        }

        
    }
}
