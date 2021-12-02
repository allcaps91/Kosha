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
    public class ColumnButton : AbstractColumn
    {
      
        public event EditorNotifyEventHandler ButtonClick;

        public ColumnButton(FpSpread fpSpread, string caption, string dataField, int width, SpreadCellTypeOption option = null) : base(fpSpread, dataField, caption, width, option)
        {
            ButtonCellType buttonCellType; 
            if (option.ICustomCellType != null)
            {
                buttonCellType = (ButtonCellType)option.ICustomCellType;
            }
            else
            {
                buttonCellType = new ButtonCellType();
            }
           
            buttonCellType.Text = option.ButtonText;

            column.CellType = buttonCellType;
            
            fpSpread.ButtonClicked += FpSpread_ButtonClicked;
        }

        private void FpSpread_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
           if(column.Index == e.Column)
           {
                ButtonClick?.Invoke(sender, e);
           }
          
        }


    }
}
