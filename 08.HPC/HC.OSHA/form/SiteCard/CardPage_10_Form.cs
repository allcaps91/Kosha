using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using FarPoint.Win.Spread;
using HC.Core.Common.Interface;
using HC_Core;
using HC.Core.Common.Util;
using HC.Core.Model;
using HC.OSHA.Dto;
using HC.OSHA.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComHpcLibB.Model;

namespace HC_OSHA
{
    /// <summary>
    /// 안전검사
    /// </summary>
    public partial class CardPage_10_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {
        public CardPage_10_Form()
        {
            InitializeComponent();
        }

        public void Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;
        }

        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;
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

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }
    }
}
