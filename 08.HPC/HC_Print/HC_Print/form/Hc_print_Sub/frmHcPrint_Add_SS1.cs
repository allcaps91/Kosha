using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Windows.Forms;
using ComHpcLibB.Model;
using ComHpcLibB;

namespace HC_Print
{
    public partial class frmHcPrint_Add_SS1 : Form
    {

        long fnWrtno = 0;

        FpSpread ssPrint;
        HicJepsuResEtcPatientService hicJepsuResEtcPatientService = null;


        public frmHcPrint_Add_SS1()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcPrint_Add_SS1(long argWRTNO)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            fnWrtno = argWRTNO;
        }


        private void SetControl()
        {
            hicJepsuResEtcPatientService = new HicJepsuResEtcPatientService();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {

            string strPrintName = "";

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread SPR = new clsSpread();
            clsPrint CP = new clsPrint();

            long nDrNO = 0;
            string strUCodes = "";
            string strJumin = "";
            string strLtdCode = "";
            string strJepDate = "";
            string strData = "";
            string strTemp = "";
            string strTemp1 = "";
            string strTemp2 = "";
            string strList = "";
            string strSex = "";

            HIC_JEPSU_RES_ETC_PATIENT item = hicJepsuResEtcPatientService.GetItemByWrtnoGubun(fnWrtno, "");
            strJumin = item.JUMIN;
            strLtdCode = VB.Format(item.LTDCODE.ToString(),"#");
            strJepDate = item.JEPDATE;
            nDrNO = item.PANJENGDRNO;

            if( item.SEX == "M")
            {
                strSex = "남";
            }
            else if ( item.SEX == "F")
            {
                strSex = "여";
            }

            SS1.ActiveSheet.Cells[5, 2].Text = item.SNAME;
            SS1.ActiveSheet.Cells[5, 3].Text = "주민등록번호 : "+VB.Left(strJumin,6) + "-" + VB.Mid(strJumin,7,1)+"******";
            SS1.ActiveSheet.Cells[5, 6].Text = strSex;

            SS1.ActiveSheet.Cells[6, 2].Text = item.PTNO;
            SS1.ActiveSheet.Cells[6, 3].Text = item.LTDCODE.ToString();

            SS1.ActiveSheet.Cells[7, 2].Text = item.JUSO1 + " "+item.JUSO2;

            SS1.ActiveSheet.Cells[8, 2].Text = item.JEPDATE;
            SS1.ActiveSheet.Cells[8, 3].Text = "전 화 번 호 : " + item.TEL;

            SS1.ActiveSheet.Cells[31, 3].Text = "면 허 번 호 : " + item.PANJENGDRNO;
            SS1.ActiveSheet.Cells[31, 6].Text = item.PANJENGDRNO.ToString();

            //SignImage_Spread_SET

            //종합판정소견
            strTemp1 = item.SOGEN;
            SS1.ActiveSheet.Cells[12, 1].Text = item.SOGEN;

            strPrintName = CP.getPmpaBarCodePrinter("신용카드"); //Default :신용카드(접수창구용 설정이름)
            setMargin = new clsSpread.SpdPrint_Margin(0, 0, 20, 40, 0, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, false, false);
            ssPrint = SS1;
            SPR.setSpdPrint(ssPrint, false, setMargin, setOption, "", "", strPrintName);

            ComFunc.Delay(1500);

            ssPrint.Dispose();
            ssPrint = null;

            this.Close();
        }







    }
}
