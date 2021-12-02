using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Windows.Forms;
using ComHpcLibB.Model;
using ComHpcLibB;
using System.Collections.Generic;
using ComHpcLibB.Service;
using ComBase.Controls;

namespace HC_Print
{
    public partial class frmHcPrint_Add_SS2 : Form
    {

        long fnWrtno = 0;

        clsHaBase hb = new clsHaBase();
        ComFunc cf = new ComFunc();

        FpSpread ssPrint;
        HicJepsuResEtcPatientService hicJepsuResEtcPatientService = null;
        HicResultExcodeJepsuService hicResultExcodeJepsuService = null;

        public frmHcPrint_Add_SS2()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }


        public frmHcPrint_Add_SS2(long argWRTNO)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            fnWrtno = argWRTNO;
        }


        private void SetControl()
        {
            hicJepsuResEtcPatientService = new HicJepsuResEtcPatientService();
            hicResultExcodeJepsuService = new HicResultExcodeJepsuService();
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

            int nRow = 0;
            long nDrNO = 0;
            string strJumin = "";
            string strLtdCode = "";
            string strJepDate = "";
            string strSex = "";

            string strNomal = "";
            string strGjYear = "";
            string strData = "";
            string strTemp1 = "";
            string strList = "";

            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";

           
            

            HIC_JEPSU_RES_ETC_PATIENT item = hicJepsuResEtcPatientService.GetItemByWrtnoGubun(fnWrtno, "");
            strJumin = item.JUMIN;
            strLtdCode = VB.Format(item.LTDCODE.ToString(), "#");
            strJepDate = item.JEPDATE;
            nDrNO = item.PANJENGDRNO;

            if (item.SEX == "M")
            {
                strSex = "남";
            }
            else if (item.SEX == "F")
            {
                strSex = "여";
            }
            nRow = 5;

            SS1.ActiveSheet.Cells[1, 1].Text = "  " + item.PTNO + "  " + item.SNAME + "  나이:  " + item.AGE + "세  성별:  " + strSex + "  검진일  " + strJepDate;

            List<HIC_RESULT_EXCODE_JEPSU> list = hicResultExcodeJepsuService.GetItemByWrtNo(fnWrtno);
            for (int i = 0; i <= list.Count; i++)
            {

                strExCode = list[i].EXCODE;
                strResult = list[i].RESULT;
                strResCode = list[i].RESCODE;
                strResultType = list[i].RESULTTYPE;
                strGbCodeUse = list[i].GBCODEUSE;

                //간염항원
                if (strExCode == "E504")
                {
                    if (strResult == "01")
                    {
                        strResult = "음성(-)";
                    }
                    else if (strResult == "02")
                    {
                        strResult = "양성(+)";
                    }
                }


                SS1.ActiveSheet.Cells[nRow, 1].Text = "  " + list[i].HNAME;
                SS1.ActiveSheet.Cells[nRow, 3].Text = "  " + strResult;

                //비만도는 자동 계산함
                if (strExCode == "A103")
                {
                    strResCode = "061";
                }

                if (strGbCodeUse =="Y")
                {
                    if(strResult.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[nRow, 3].Text = "  " + hb.READ_ResultName(strResCode, strResult);
                    }
                }

                //참고치 Display
                if (strSex == "남")
                {
                    strNomal = list[i].MIN_M.Trim() + " ~ " + list[i].MAX_M.Trim();
                }
                else if (strSex == "여")
                {
                    strNomal = list[i].MIN_F.Trim() + " ~ " + list[i].MAX_F.Trim();
                }

                
                if( list[i].UNIT.IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Cells[nRow, 2].Text = "  " + strNomal + "(" + list[i].UNIT + ")";
                }
                else
                {
                    SS1.ActiveSheet.Cells[nRow, 2].Text = "  " + strNomal;
                }

                strTemp1 = SS1.ActiveSheet.Cells[nRow, 3].Text;

                if (!strTemp1.IsNullOrEmpty())
                {
                    strList =  cf.TextBox_2_MultiLine(" " + strTemp1, 34);
                    for (int j = 1; j <= VB.L(strList,"{{@}}"); j++)
                    {
                        if(j > 1)
                        {
                            nRow = nRow + 1;
                        }

                        if( j < 5)
                        {
                            SS1.ActiveSheet.Cells[nRow, 2].Text = "  " + VB.Pstr(strList,"{{@}}", j);
                        }
                    }
                    strTemp1 = "";
                }







            }



                //SignImage_Spread_SET

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
