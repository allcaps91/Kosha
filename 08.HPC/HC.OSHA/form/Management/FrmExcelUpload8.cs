using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_OSHA
{
    public partial class FrmExcelUpload8 : Form
    {
        public FrmExcelUpload8()
        {
            InitializeComponent();
        }

        private void Screen_Set()
        {
            int i = 0;
            string strTitle = "";

            TxtLtdcode.Text = "";

            for (i = 0; i < 1000; i++)
            {
                FnCol[i] = 0;
            }

            SS1_Sheet1.RowCount = 0;
            SSConv_Sheet1.RowCount = 0;
            SSConv_Sheet1.RowCount = SS1_Sheet1.ColumnCount;

            for (i = 0; i < SS1_Sheet1.ColumnCount - 2; i++)
            {
                strTitle = SS1_Sheet1.ColumnHeader.Cells[0, i].Value.ToString();
                SSConv_Sheet1.Cells[i, 0].Text = strTitle;
            }

            btnJob1.Enabled = true;
            btnJob2.Enabled = false;
            btnJob3.Enabled = false;
            btnJob4.Enabled = false;
            btnJob5.Enabled = false;
        }

    }
}
