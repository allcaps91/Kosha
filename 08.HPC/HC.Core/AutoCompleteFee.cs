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
using HC.Core.Model;

namespace HC_Core
{
    public partial class AutoCompleteFee : UserControl
    {
        public delegate void HideDelegate(AutoCompleteFeeModel model, Cell ActiveCell);
        public event HideDelegate hideDelegate;

        private List<AutoCompleteFeeModel> DataSource;
        private Cell ActiveCell;
        public AutoCompleteFee()
        {
            InitializeComponent();

            DataSource = new List<AutoCompleteFeeModel>();
        }
        private void AutoCompleteCodes_Load(object sender, EventArgs e)
        {

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = true, ColumnHeaderHeight = 20 });
            SSList.AddColumnText("년도", nameof(AutoCompleteFeeModel.M_YEAR), 37, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("ID", nameof(AutoCompleteFeeModel.M_ID), 37, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("측정방법", nameof(AutoCompleteFeeModel.M_NAME), 120, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("산정내역", nameof(AutoCompleteFeeModel.M_SANNAME), 120, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("단가", nameof(AutoCompleteFeeModel.M_DANGA), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.SetDataSource(new List<AutoCompleteFeeModel>());
            SSList.KeyDown += SSList_KeyUp;
            
        }

        private void SSList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ActiveCell != null)
                {
                    AutoCompleteFeeModel code = SSList.GetRowData(SSList.GetActiveRow().Index) as AutoCompleteFeeModel;
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
        public void SetDatasource(Cell cell, List<AutoCompleteFeeModel> dataSource, Rectangle r)
        {
            ActiveCell = cell;
            DataSource = dataSource;
            this.Show();
            //    SSList.Focus();

            if (DataSource != null)
            {
                if (DataSource is List<AutoCompleteFeeModel>)
                {
                    List<AutoCompleteFeeModel> list = dataSource as List<AutoCompleteFeeModel>;
                    SSList.SetDataSource(list);
                    if (r.Left > 1200)
                    {
                        this.Location = new Point(r.Left - 200, r.Top + 100);
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
                AutoCompleteFeeModel model = SSList.GetRowData(e.Row) as AutoCompleteFeeModel;
                this.hideDelegate(model, ActiveCell);
            }
        }
    }
}
