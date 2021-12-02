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
    public class ColumnComboBox : AbstractColumn
    {
        public ColumnComboBox(FpSpread fpSpread, string caption, string dataField, int width, IsReadOnly isReadOnly, SpreadComboBoxData comboBoxData, SpreadCellTypeOption option = null) : base(fpSpread, dataField, caption, width, option)
        {
            ComboBoxCellType comboBoxCellType;

            if (option.ICustomCellType != null)
            {
                comboBoxCellType = (ComboBoxCellType)option.ICustomCellType;
                (comboBoxCellType as ICustomCellType).fpspread = fpSpread;
            }
            else
            {
                comboBoxCellType = new ComboBoxCellType();
            }

            if (isReadOnly == IsReadOnly.Y)
            {
                column.Locked = true;
            }
            if (comboBoxData == null)
            {
                if (option.Items != null)
                {
                    List<string> items = new List<string>();
                    List<string> itemData = new List<string>();
                    foreach (var item in option.Items)
                    {
                        items.Add(item.GetPropertieValue(option.DisplayMember).ToString());
                        itemData.Add(item.GetPropertieValue(option.ValueMember).ToString());
                    }

                    comboBoxCellType.Items = items.ToArray();
                    comboBoxCellType.ItemData = itemData.ToArray();

                }

              
            }
            else
            {

                comboBoxCellType.Items = comboBoxData.GetItems();
                comboBoxCellType.ItemData = comboBoxData.GetItemData();
            }

            comboBoxCellType.EditorValue = EditorValue.ItemData;
            column.CellType = comboBoxCellType;
        }


        //public override void SetCellTypeValidation(MTSValidation mtsValitaion)
        //{
        //    column.Tag = mtsValitaion;
        //}
    }
}
