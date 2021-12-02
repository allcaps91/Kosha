using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_Print
{
    public partial class frmHicXrayPrint_Sub : Form
    {

        int fnDoct1 = 0;
        int fnDoct2 = 0;
        int fnTpage = 0;
        int fnPage = 0;

        string fstrFDate = "";
        string fstrTDate = "";
        string fstrGubun = "";

        clsSpread cSpd = new clsSpread();

        List<string> fstrXrayNo = new List<string>();

        HIC_XRAY_RESULT hxr = null;


        HicXrayResultService hicXrayResultService = null;
        HicLtdService hicLtdService = null;
        HicCodeService hicCodeService = null;

        public frmHicXrayPrint_Sub()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }


        public frmHicXrayPrint_Sub(HIC_XRAY_RESULT HXR, string strFDate, string strTDATE, string strGubun)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            hxr = HXR;
            fstrFDate = strFDate;
            fstrTDate = strTDATE;
            fstrGubun = strGubun;
        }

        private void SetControl()
        {
            hxr = new HIC_XRAY_RESULT();

            hicXrayResultService = new HicXrayResultService();
            hicLtdService = new HicLtdService();
            hicCodeService = new HicCodeService();

        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {
            this.Hide();

            Result_Print_Sub();

            ComFunc.Delay(1500);
            this.Close();

        }
        private void Result_Print_Sub()
        {

            XRAYNO_LIST();
            SS_CLEAR();
            SS_TITLE();
            SSPAN_DATA();
            SSPAN_PRINT();


        }

        private void XRAYNO_LIST()
        {

            fstrXrayNo.Clear();
            List<HIC_XRAY_RESULT> list = hicXrayResultService.GetItemByJepDateLtdCodeDocJongGubun(fstrFDate, fstrTDate, hxr.LTDCODE, hxr.GBREAD, hxr.GJJONG, hxr.READDOCT1, hxr.READDOCT2, fstrGubun, hxr.READDATE.ToString());
            
            if(!list.IsNullOrEmpty())
            {
                for ( int i = 0; i< list.Count; i++)
                {
                    fstrXrayNo.Add(list[i].XRAYNO.Trim());
                }
            }

        }
        private void SS_CLEAR()
        {
            SS_Print.ActiveSheet.Cells[3, 7].Text = "";
            SS_Print.ActiveSheet.Cells[3, 11].Text = "";

            SS_Print.ActiveSheet.Cells[4, 7].Text = "";
            SS_Print.ActiveSheet.Cells[4, 11].Text = "";

            SS_Print.ActiveSheet.Cells[5, 7].Text = "";
            SS_Print.ActiveSheet.Cells[5, 10].Text = "";
            SS_Print.ActiveSheet.Cells[5, 12].Text = "";

            SS_Print.ActiveSheet.Cells[6, 10].Text = "";

            SS_Print.ActiveSheet.Cells[7, 6].Text = "";
            SS_Print.ActiveSheet.Cells[7, 10].Text = "";

            for (int i = 10; i < 36; i++)
            {
                SS_Print.ActiveSheet.Cells[i, 1].Text = "";
                SS_Print.ActiveSheet.Cells[i, 2].Text = "";
                SS_Print.ActiveSheet.Cells[i, 3].Text = "";
                SS_Print.ActiveSheet.Cells[i, 4].Text = "";
                SS_Print.ActiveSheet.Cells[i, 5].Text = "";
                SS_Print.ActiveSheet.Cells[i, 6].Text = "";
                SS_Print.ActiveSheet.Cells[i, 7].Text = "";
                SS_Print.ActiveSheet.Cells[i, 8].Text = "";
            }

            for (int i = 37; i < 39; i++)
            {
                SS_Print.ActiveSheet.Cells[i, 5].Text = "";
                SS_Print.ActiveSheet.Cells[i, 10].Text = "";
            }

            SS_Print.ActiveSheet.Cells[40, 2].Text = "";
            SS_Print.ActiveSheet.Cells[40, 7].Text = "";
        }

        private void SS_CLEAR_SUB()
        {
            for (int i = 10; i < 35; i++)
            {
                SS_Print.ActiveSheet.Cells[i, 1].Text = "";
                SS_Print.ActiveSheet.Cells[i, 2].Text = "";
                SS_Print.ActiveSheet.Cells[i, 3].Text = "";
                SS_Print.ActiveSheet.Cells[i, 4].Text = "";
                SS_Print.ActiveSheet.Cells[i, 5].Text = "";
            }
        }


        private void SS_TITLE()
        {

            string strPanDrSabun = "";
            string strPanInfo = "";



            HIC_XRAY_RESULT item = hicXrayResultService.GetMaxMinXrayNoByXrayNo(fstrXrayNo);
            if (!item.IsNullOrEmpty())
            {

                HIC_LTD item2 = hicLtdService.GetItembyCode(item.LTDCODE.ToString());
                if (!item2.IsNullOrEmpty())
                {
                    SS_Print.ActiveSheet.Cells[3, 7].Text = item2.JUSO + "" + item2.JUSODETAIL;
                    SS_Print.ActiveSheet.Cells[4, 7].Text = item2.SANGHO;
                    SS_Print.ActiveSheet.Cells[5, 7].Text = item2.JONGMOK;

                    SS_Print.ActiveSheet.Cells[3, 11].Text = item.MINXRAY;
                    SS_Print.ActiveSheet.Cells[4, 11].Text = item.MAXXRAY;
                    SS_Print.ActiveSheet.Cells[5, 10].Text = fstrXrayNo.Count + "명";
                }
            }

            SS_Print.ActiveSheet.Cells[7, 6].Text = " " + VB.Format(fstrFDate, "YYYY/MM/DD") + " ~ " + VB.Format(fstrTDate, "MM/DD");

            if (fstrGubun=="Y")
            {
                SS_Print.ActiveSheet.Cells[5, 12].Text = "일반";
            }
            else
            {
                SS_Print.ActiveSheet.Cells[5, 12].Text = "분진";
            }

            SS_Print.ActiveSheet.Cells[6, 10].Text = " " + fstrFDate + " ~ " + fstrTDate;
            SS_Print.ActiveSheet.Cells[7, 10].Text = " " + hxr.READDATE;



            //판독자 세팅
            HIC_XRAY_RESULT item3 = hicXrayResultService.GetReadDoctByXrayNo(fstrXrayNo);
            if(!item3.IsNullOrEmpty())
            {
                if (item3.READDOCT1 > 0)
                {
                    strPanDrSabun = item3.READDOCT1.ToString();
                    fnDoct1 = item3.READDOCT1.To<int>(0);
                    HIC_CODE item4 = hicCodeService.Read_Hic_Code("97", strPanDrSabun);
                    if(!item4.IsNullOrEmpty())
                    {
                        SS_Print.ActiveSheet.Cells[37, 5].Text = " 포항성모병원 ";
                        SS_Print.ActiveSheet.Cells[38, 5].Text = " "+hxr.READDATE.ToString();
                        SS_Print.ActiveSheet.Cells[39, 5].Text = " " + item4.NAME;
                        SS_Print.ActiveSheet.Cells[40, 2].Text = " 판정의사 : " + item4.GCODE;
                    }
                }

                if (item3.READDOCT2 > 0)
                {
                    strPanDrSabun = item3.READDOCT2.ToString();
                    fnDoct2 = item3.READDOCT2.To<int>(0);
                    HIC_CODE item4 = hicCodeService.Read_Hic_Code("97", strPanDrSabun);
                    if (!item4.IsNullOrEmpty())
                    {
                        SS_Print.ActiveSheet.Cells[37, 5].Text = " 포항성모병원 ";
                        SS_Print.ActiveSheet.Cells[38, 5].Text = " " + hxr.READDATE.ToString();
                        SS_Print.ActiveSheet.Cells[39, 5].Text = " " + item4.NAME;
                        SS_Print.ActiveSheet.Cells[40, 2].Text = " 판정의사 : " + item4.GCODE;
                    }

                }
            }
        }

        private void SSPAN_DATA()
        {
            int nRow = 8;
            string strSogen = "";

            List <HIC_XRAY_RESULT> list = hicXrayResultService.GetItemByXrayNo(fstrXrayNo);
            if(!list.IsNullOrEmpty())
            {

                fnTpage = list.Count / 26;
                fnPage = 0;
                if(fnPage >0) { fnTpage += 1; }


                for (int i = 0; i < list.Count; i++)
                {
                    nRow = nRow + 1;
                    SS_Print.ActiveSheet.Cells[nRow + 9, 1].Text = list[i].SNAME.Trim();
                    SS_Print.ActiveSheet.Cells[nRow + 9, 2].Text = list[i].SEX.Trim();
                    SS_Print.ActiveSheet.Cells[nRow + 9, 3].Text = list[i].AGE.Trim();
                    SS_Print.ActiveSheet.Cells[nRow + 9, 4].Text = list[i].PTNO.Trim();
                    SS_Print.ActiveSheet.Cells[nRow + 9, 5].Text = list[i].XRAYNO.Trim();
                    SS_Print.ActiveSheet.Cells[nRow + 9, 6].Text = list[i].RESULT2.Trim();


                    SS_Print.ActiveSheet.Cells[nRow + 9, 7].Text = hicCodeService.Read_Hic_Name2("97", list[i].READDOCT1.ToString());
                    strSogen = list[i].RESULT4.Trim();
              
                    SS_Print.ActiveSheet.Cells[nRow + 9, 8].Text = strSogen;

                    
                    if (nRow >24)
                    {


                    }

                    //분진판독일경우
                    if(list[i].GBREAD == "2")
                    {
                        if(!list[i].RESULT2_1.IsNullOrEmpty())
                        {
                            nRow = nRow + 1;
                            SS_Print.ActiveSheet.Cells[nRow + 9, 5].Text = list[i].XRAYNO.Trim();
                            SS_Print.ActiveSheet.Cells[nRow + 9, 6].Text = list[i].RESULT2_1.Trim();
                            SS_Print.ActiveSheet.Cells[nRow + 9, 7].Text = hicCodeService.Read_Hic_Name2("97", list[i].READDOCT2.ToString());
                            strSogen = list[i].RESULT4_1.Trim();

                            SS_Print.ActiveSheet.Cells[nRow + 9, 8].Text = strSogen;
                        }
                    }

                    if (nRow > 24)
                    {


                    }

                    //신규일때만 인쇄마킹
                    if (fstrGubun.IsNullOrEmpty())
                    {
                        
                        int result = hicXrayResultService.UpdateGbPrintByXrayNo1(fstrXrayNo, "Y");

                        if (result < 0)
                        {
                            MessageBox.Show("자료 업데이트시 에러발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    }
                }
            }
        }

        private void SSPAN_PRINT()
        {
            


            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "";
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, false, false, true, true, false, false, true);
            cSpd.setSpdPrint(SS_Print, PrePrint, setMargin, setOption, strHeader, strFooter);

        }


    }

}
