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
    public class ColumnListBox : AbstractColumn
    {
        public ColumnListBox(FpSpread fpSpread, string caption, string dataField, int width, ImageList imageList, SpreadCellTypeOption option = null) : base(fpSpread, dataField, caption, width, option)
        {
            ListBoxCellType listBoxCellType;
            if (option.ICustomCellType != null)
            {
                listBoxCellType = (ListBoxCellType)option.ICustomCellType;
                (listBoxCellType as ICustomCellType).fpspread = fpSpread;
            }
            else
            {
                listBoxCellType = new ListBoxCellType();
            }

            listBoxCellType.ImageList = imageList;


            listBoxCellType.ItemData = new string[] { "One", "Two", "Three" };

            listBoxCellType.Items = new string[] { "One", "Two", "Three" };

            listBoxCellType.ItemHeight = 150;
            
            column.CellType = listBoxCellType;

           

        }


        //public override void SetCellTypeValidation(MTSValidation mtsValitaion)
        //{
        //    column.Tag = mtsValitaion;
        //}
    }
}
