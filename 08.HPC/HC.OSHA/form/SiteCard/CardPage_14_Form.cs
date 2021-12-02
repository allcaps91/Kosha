using HC.Core.Common.Interface;
using HC_Core;
using HC.Core.Common.Util;
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
    public partial class CardPage_14_Form : CommonForm, IPrint
    {
        public CardPage_14_Form()
        {
            InitializeComponent();
        }

        public void Print()
        {
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
        }

        public bool NewPrint()
        {
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
            return true;
        }

        private void BtnPrint_Click_1(object sender, EventArgs e)
        {
            Print();
        }
    }
}
