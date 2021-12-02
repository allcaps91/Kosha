using ComEmrBase;
using System;
using System.Windows.Forms;

namespace ComEmrLibB
{
    public partial class frmChartViewReq : Form
    {
        public EmrPatient p = null;

        public frmChartViewReq()
        {
            InitializeComponent();
        }

        public frmChartViewReq(EmrPatient po)
        {
            InitializeComponent();
            p = po;
        }

        private void frmChartViewReq_Load(object sender, EventArgs e)
        {

        }
    }
}
