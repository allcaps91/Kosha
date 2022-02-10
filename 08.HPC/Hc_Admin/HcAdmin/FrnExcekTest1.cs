using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HcAdmin
{
    public partial class FrmExcelTest1 : Form
    {
        public FrmExcelTest1()
        {
            InitializeComponent();
            Screen_Set();
        }

        private void FrmExcelTest1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SSExcel.ActiveSheet.RowCount = 0;
            string fileName = @"C:\헬스소프트\견적서.xlsx";
            SSExcel.ActiveSheet.OpenExcel(fileName, 0);

        }

        private void Screen_Set()
        {
            int nYear = 0;
            string strTitle = "";
            nYear = Int32.Parse(DateTime.Now.ToString("yyyy"));

            for(int i = 0; i < 5; i++)
            {
                cboYear.Items.Add(nYear.ToString());
                nYear--;
            }

            SS1_Sheet1.RowCount = 0; 
            SSConv_Sheet1.RowCount = 0;
            SSConv_Sheet1.RowCount = SS1_Sheet1.ColumnCount;

            //for (int i=0; i< SS1_Sheet1.ColumnCount-1; i++)
            for (int i = 0; i < SS1_Sheet1.ColumnCount; i++)
            {
                strTitle = SS1_Sheet1.ColumnHeader.Cells[0, i].Value.ToString();
                SSConv_Sheet1.Cells[i, 0].Text = strTitle;
                SSConv_Sheet1.Cells[i, 1].Value = (i + 1);
            }
        }
    }
}
