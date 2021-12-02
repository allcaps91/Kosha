using ComBase;
using ComHpcLibB.Dto;
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


/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaPrintFoodTicket.cs
/// Description     : 식사이용권 출력/// Author          : 김경동
/// Create Date     : 2020-10-05
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "" />
namespace ComHpcLibB
{
    public partial class frmHaPrintFoodTicket : Form
    {
        HEA_JEPSU nHJ = null;
        FpSpread ssPrint;

        string FstrSikGbn = null;


        public frmHaPrintFoodTicket()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHaPrintFoodTicket(HEA_JEPSU aHJ, string ArgSikGbn)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            nHJ = aHJ;
            FstrSikGbn = ArgSikGbn;

        }

        private void SetControl()
        {
            nHJ = new HEA_JEPSU();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {
            string strPrintName = "";

            this.Hide();

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread SPR = new clsSpread();
            clsPrint CP = new clsPrint();
            ComFunc cf = new ComFunc();

            SSFoodTicket.ActiveSheet.Cells[2, 3].Text =  nHJ.SNAME;
            SSFoodTicket.ActiveSheet.Cells[3, 3].Text = nHJ.SDATE + "  " + FstrSikGbn;
            SSFoodTicket.ActiveSheet.Cells[19, 3].Text = cf.DATE_ADD(clsDB.DbCon, nHJ.SDATE, 29);

            strPrintName = CP.getPmpaBarCodePrinter("신용카드"); //Default :신용카드(접수창구용 설정이름)
            setMargin = new clsSpread.SpdPrint_Margin(0, 0, 0, 40, 0, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, false, false);
            ssPrint = SSFoodTicket;
            SPR.setSpdPrint(ssPrint, false, setMargin, setOption, "", "", strPrintName);

            ComFunc.Delay(1500);

            ssPrint.Dispose();
            ssPrint = null;

            this.Close();
        }
    }
}
