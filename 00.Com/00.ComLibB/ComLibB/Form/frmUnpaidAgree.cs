using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmUnpaidAgree
    /// File Name : frmUnpaidAgree.cs
    /// Title or Description : 비급여고지 동의서 출력
    /// Author : 안정수
    /// Create Date : 2021-01-19
    /// Update Histroy : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso> 
    /// </seealso>
    /// <vbp>
    /// </vbp>
    public partial class frmUnpaidAgree : Form
    {
        public frmUnpaidAgree()
        {
            InitializeComponent();
        }

        void button1_Click(object sender, EventArgs e)
        {
            string strPrintName1 = "";
            string strPrintName2 = "";

            try
            {
                strPrintName1 = clsPrint.gGetDefaultPrinter();
            }
            catch (Exception ex)
            {
                clsPrint.gSetDefaultPrinter(strPrintName1);
            }

            ss1_Sheet1.PrintInfo.Printer = strPrintName1;
            ss1_Sheet1.PrintInfo.ShowColumnHeader = PrintHeader.Hide;
            ss1_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Hide;
            ss1_Sheet1.PrintInfo.ShowBorder = false;
            ss1_Sheet1.PrintInfo.ShowColor = false;
            ss1_Sheet1.PrintInfo.ShowGrid = false;
            ss1_Sheet1.PrintInfo.ShowShadows = true;
            ss1_Sheet1.PrintInfo.UseMax = false;
            ss1_Sheet1.PrintInfo.Centering = Centering.Both;
            ss1_Sheet1.PrintInfo.PrintType = PrintType.All;

            ss1.PrintSheet(0);
        }

        private void ss1_CellClick(object sender, CellClickEventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }
    }
}
