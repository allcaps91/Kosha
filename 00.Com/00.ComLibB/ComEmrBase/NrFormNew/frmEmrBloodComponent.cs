using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrBloodComponent : Form
    {
        public delegate void EventBloodComponent(Control control, string Code, Form frm);
        public event EventBloodComponent rEventBloodComponent;

        Control mcontrol = null;

        public frmEmrBloodComponent(Control control)
        {
            mcontrol = control;
            InitializeComponent();
        }

        private void frmEmrBloodComponent_Load(object sender, EventArgs e)
        {

        }

        private void ssBlood_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            rEventBloodComponent?.Invoke(mcontrol, ssBlood_Sheet1.Cells[e.Row, 1].Text.Trim(), this);
            Close();
        }
    }
}
