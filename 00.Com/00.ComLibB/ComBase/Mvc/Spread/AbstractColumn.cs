using ComBase.Controls;
using ComBase.Mvc.Validation;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc.Spread
{
    public abstract class AbstractColumn
    {
        /// <summary>
        /// 유효성 검사
        /// </summary>
       

        protected FpSpread fpSpread;
        protected Column column;
        public AbstractColumn(FpSpread fpSpread, string dataField, string caption, int width, SpreadCellTypeOption option = null)
        {
            this.fpSpread = fpSpread;

            if (dataField == "ac")
            {
                fpSpread.ActiveSheet.Columns.Add(1, 1);
            }
            else
            {
                fpSpread.ActiveSheet.Columns.Add(fpSpread.ActiveSheet.Columns.Count, 1);
            }
            
            this.column = fpSpread.ActiveSheet.Columns[fpSpread.ActiveSheet.Columns.Count - 1];


            column.MergePolicy = option.mergePolicy;
            column.DataField = dataField;
            column.Label = caption;
            column.Width = width;
            column.Visible = option.IsVisivle;
         //   column.Locked = option.IsEditble;
            column.VerticalAlignment = CellVerticalAlignment.Center;
            column.HorizontalAlignment = option.Aligen;
            column.AllowAutoSort = option.IsSort;
            column.SortIndicator = option.sortIndicator;
            column.BackColor = option.BackColor;
            column.ForeColor = option.ForceColor;
            
        }

        public Column GetColumn()
        {
            return this.column;
        }
    }
}
