using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using HC_Core.Dto;
using FarPoint.Win.Spread;

namespace HC_Core
{
    public partial class AutoCompleteCodes : UserControl
    {
        public delegate void HideDelegate(object DataSource, Cell ActiveCell);
        public event HideDelegate hideDelegate;

        private object DataSource;
        private Cell ActiveCell;
        public AutoCompleteCodes()
        {
            InitializeComponent();
            DataSource = new List<object>();
        }
        private void AutoCompleteCodes_Load(object sender, EventArgs e)
        {

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = true, ColumnHeaderHeight = 20 });
            SSList.AddColumnText("코드", nameof(AutoCompleteCode.Code), 73, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("코드명", nameof(AutoCompleteCode.Name), 350, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("비고", nameof(AutoCompleteCode.Remark), 350, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.KeyDown += SSList_KeyUp; 
        }

        private void SSList_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (ActiveCell != null)
                {
                    AutoCompleteCode code = SSList.GetRowData(SSList.GetActiveRow().Index) as AutoCompleteCode;
                    if (hideDelegate != null)
                    {
                        this.Hide();
                        this.hideDelegate(code, ActiveCell);
                    }
                }
            }
        }
        public void SetFocus()
        {
            if (DataSource != null)
            {
                SSList.Focus();
            }

        }
        public void SetDatasource(Cell cell, object dataSource, Rectangle r)
        {
            ActiveCell = cell;
            DataSource = dataSource;

            this.Show();

        //    SSList.Focus();

            if (DataSource != null)
            {
                if(DataSource is List<AutoCompleteCode>)
                {
                    List<AutoCompleteCode> list = dataSource as List<AutoCompleteCode>;
                    SSList.SetDataSource(list);
                    if (r.Left > 1200)
                    {
                        this.Location = new Point(r.Left-300, r.Top + 100);
                    }
                    else
                    {
                        this.Location = new Point(r.Left, r.Top + 100);
                    }
                    

                }
                
            }
        }

   
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void SSList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            this.Hide();
            if (hideDelegate != null)
            {
                AutoCompleteCode code = SSList.GetRowData(e.Row) as AutoCompleteCode;
                this.hideDelegate(code, ActiveCell);
            }
        }
    }
}
