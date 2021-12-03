using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA
{
    public class customChild : FarPoint.Win.Spread.SheetView
    {
        public override int ParentRowIndex
        {
            get
            {
                return Convert.ToInt32(SheetName);
            }

        }

    }
    public class CustomSheet : SheetView
    {
        public override Int32 ChildRelationCount
        {
            get { return 1; }
        }

        public override FarPoint.Win.Spread.SheetView GetChildView(int row, int relationIndex)
        {

            customChild child = (customChild)FindChildView(row, 0);

            if (child == null)

            {
                
                child = new customChild();

                child.RowCount = 3;

                child.ColumnCount = 3;

                child.Parent = this;

                child.SheetName = row.ToString();


                child.Cells[0, 0].Text = "get data for parent row " + row.ToString();

                child.Columns[0].Width = 200;

                ChildViews.Add(child);

            }

            return child;

        }



        public override SheetView FindChildView(int row, int relationIndex)
        {

            foreach (SheetView v in ChildViews)
            {

                if (v.SheetName == row.ToString())

                    return v;

            }

            return null;

        }
    }
}
