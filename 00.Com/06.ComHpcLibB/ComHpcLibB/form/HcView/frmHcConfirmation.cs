using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using FarPoint.Win.Spread;
using System;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmHcConfirmation :Form
    {
        HIC_LTD nHL = null;
        HIC_JEPSU nHJ = null;
        FpSpread ssPrint;
        string FstrUCodesName = string.Empty;


        public frmHcConfirmation()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }
        
        public frmHcConfirmation(HIC_JEPSU aHJ, HIC_LTD aHL, string argUCodeName)
        {
            InitializeComponent();
            SetControl();
            SetEvent();
            nHJ = aHJ;
            nHL = aHL;
            FstrUCodesName = argUCodeName;
        }

        private void SetControl()
        {
            nHL = new HIC_LTD();
            nHJ = new HIC_JEPSU();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {
            this.Hide();

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread SPR = new clsSpread();

            //Spread Set

            if (nHL == null)
            {
                SS7.ActiveSheet.Cells[3, 2].Text = "회 사 명: 개 인";
            }
            else
            {
                SS7.ActiveSheet.Cells[3, 2].Text = "회 사 명: " + nHL.SANGHO;
                SS7.ActiveSheet.Cells[4, 2].Text = "주      소: " + nHL.JUSO + " " + nHL.JUSODETAIL;
                SS7.ActiveSheet.Cells[5, 2].Text = "회사전화: " + nHL.TEL;
            }

            SS7.ActiveSheet.Cells[6, 2].Text = "수검자명: " + nHJ.SNAME;
            SS7.ActiveSheet.Cells[7, 2].Text = "주민번호: " + VB.Left(nHJ.JUMINNO, 6) + "-" + VB.Mid(nHJ.JUMINNO, 7, 1) + "******";
            SS7.ActiveSheet.Cells[8, 2].Text = "검진일자: " + nHJ.JEPDATE;
            SS7.ActiveSheet.Cells[9, 4].Text = FstrUCodesName;
            SS7.ActiveSheet.Cells[14, 1].Text = "발행일: " + DateTime.Now.Year.To<string>() + "년 " + DateTime.Now.Month.To<string>() + "월 " + DateTime.Now.Day.To<string>() + "일";

            ssPrint = SS7;

            setMargin = new clsSpread.SpdPrint_Margin(0, 0, 20, 10, 40, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, false, false);

            SPR.setSpdPrint(ssPrint, false, setMargin, setOption, "", "");

            ComFunc.Delay(1500);

            ssPrint.Dispose();
            ssPrint = null;

            this.Close();
            
        }

        
    }
}
