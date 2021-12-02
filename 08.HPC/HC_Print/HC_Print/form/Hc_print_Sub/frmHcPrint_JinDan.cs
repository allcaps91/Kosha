using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_Print
{
    public partial class frmHcPrint_JinDan : Form
    {

        long fnWrtno = 0;
        string fstrJinGbn = "";
        string fstrDate = "";
        string fstrJepdate = "";
        //string fstrTemp = "";

        FpSpread ssPrint;

        clsHaBase hb = new clsHaBase();

        HicJepsuResEtcPatientService hicJepsuResEtcPatientService = null;
        HicResultService hicResultService = null;
        HicJinWordService hicJinWordService = null;
        HicJepsuPatientService hicJepsuPatientService = null;


        public frmHcPrint_JinDan()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcPrint_JinDan(long argWRTNO, string argJepdate)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            fnWrtno = argWRTNO;
            fstrJepdate = argJepdate;
        }

        private void SetControl()
        {
            hicJepsuResEtcPatientService = new HicJepsuResEtcPatientService();
            hicResultService = new HicResultService();
            hicJinWordService = new HicJinWordService();
            hicJepsuPatientService = new HicJepsuPatientService();
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


            if (fstrJinGbn.IsNullOrEmpty())
            {
                MessageBox.Show("진단서 구분을 먼저 저장후 인쇄하세요.", "오류");
                return;
            }
            JinDan_Set();
            Result_Print_Sub();

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

        private void JinDan_Set()
        {

            string strTemp = "";

            //진단을 위해 시행한 검사
            string strXRAY = "";
            string strTBPE = "";
            string strAIDS = "";
            string strETC = "";
            string strETCR = "";

            switch (fstrJinGbn)
            {
                case "00":
                    SS1.ActiveSheet.Cells[8, 2].Text = "";
                    SS1.ActiveSheet.Cells[12, 2].Text = "";
                    SS1.ActiveSheet.Cells[13, 2].Text = "";
                    break;
                case "01":
                case "02":
                    strTemp = "  위 사람은 정신질환자,정신지체인,마약/대마" + ComNum.VBLF;
                    strTemp = strTemp + "또는 향정신성 의약품 중독자가 아님을 증명함.";
                    SS1.ActiveSheet.Cells[8, 2].Text = strTemp;
                    break;
                case "03":
                    strTemp = "  위 사람은 정신질환자,마약/대마" + ComNum.VBLF;
                    strTemp = strTemp + "또는 향정신성 의약품 중독자가 아님을 증명함.";
                    SS1.ActiveSheet.Cells[8, 2].Text = strTemp;
                    break;
                case "04":
                case "05":
                case "17":
                    strTemp = "  위 사람은 정신질환자,전염병환자," + ComNum.VBLF;
                    strTemp = strTemp + "마약ㆍ기타약물 중독자가 아님을 증명함.";
                    SS1.ActiveSheet.Cells[8, 2].Text = strTemp;
                    break;
            }

            switch (fstrJinGbn)
            {
                case "01":
                    strTemp = "면허 발급용";
                    break;
                case "36":
                    strTemp = "연구기관 제출용";
                    break;
                case "02":
                case "03":
                    strTemp = hb.READ_HIC_CODE("J1", fstrJinGbn);
                    strTemp = strTemp + " 면허 제출용";
                    break;
                default:
                    strTemp = hb.READ_HIC_CODE("J1", fstrJinGbn);
                    strTemp = strTemp + " 면허 발급용";
                    break;
            }
            SS1.ActiveSheet.Cells[13, 2].Text = strTemp;


            //건강진단서 문구 변경(JINDAN_WORD)
            HIC_JIN_WORD item = hicJinWordService.GetItemByGubunJdate(fstrDate, fstrJinGbn);
            if (!item.IsNullOrEmpty())
            {
                SS1.ActiveSheet.Cells[8, 2].Text = item.WORD1.Trim();
                SS1.ActiveSheet.Cells[9, 1].Text = item.WORD2.Trim();
            }

            //진단을 위해 시행한 검사
            List<HIC_RESULT> list = hicResultService.GetItembyWrtNo(fnWrtno);

            strXRAY = "□흉부X-선";
            strTBPE = "□TBPE 검사";
            strAIDS = "□AIDS";
            strETC = "□기타( 정신과적 문진 ) ";

            for (int i = 0; i < list.Count; i++)
            {

                switch (list[i].EXCODE)
                {
                    case "A142":
                        strXRAY = "■흉부X-선";
                        break;
                    case "E923":
                        strTBPE = "■TBPE 검사";
                        break;
                    case "E921":
                        strAIDS = "■AIDS";
                        break;
                    case "TZ70":
                        strETC = "■기타( 정신과적 문진 ) ";
                        strETCR = list[i].RESULT.Trim();
                        break;
                }

                switch (fstrJinGbn)
                {
                    case "00":
                        SS1.ActiveSheet.Cells[13, 2].Text = "";
                        break;

                    //영양사, 조리사
                    case "04":
                    case "05":
                        strTemp = strXRAY + "    " + strTBPE + "    " + strAIDS + "    " + strETC;
                        SS1.ActiveSheet.Cells[13, 2].Text = strTemp;
                        break;
                    default:
                        strTemp = strXRAY + "    " + strTBPE + "    " + "    " + strETC;
                        SS1.ActiveSheet.Cells[13, 2].Text = strTemp;
                        break;
                }

                if (strETCR.IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Cells[13, 3].Text = "정상";
                }

                if (!strETCR.IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Cells[13, 3].Text = " " + strETCR;
                }
            }

        }
        private void Result_Print_Sub()
        {

            long nREAD = 0;
            string strTemp = "";
            string strJumin = "";

            //인적사항
            HIC_JEPSU_PATIENT item = hicJepsuPatientService.GetItemByWrtnoJepdate(fnWrtno, fstrJepdate);
            if (item.IsNullOrEmpty())
            {
                MessageBox.Show("자료가 없습니다.", "오류");
                return;
            }

            strJumin = clsAES.DeAES(item.JUMIN2);

            SS1.ActiveSheet.Cells[3, 2].Text = ": " + item.PTNO.Trim();
            SS1.ActiveSheet.Cells[4, 2].Text = ": " + VB.Left(item.JEPDATE.Trim(), 4) + "-";
            SS1.ActiveSheet.Cells[4, 5].Text = ": " + VB.Left(item.JUMIN.Trim(), 6) + "-" + VB.Right(item.JUMIN2.Trim(), 7);

            SS1.ActiveSheet.Cells[6, 2].Text = item.SNAME;
            SS1.ActiveSheet.Cells[6, 4].Text = item.SEX;



            if( VB.Mid(strJumin,7,1) == "1" || VB.Mid(strJumin, 7, 1) == "2" || VB.Mid(strJumin, 7, 1) == "5" || VB.Mid(strJumin, 7, 1) == "6" )
            {
                SS1.ActiveSheet.Cells[6, 6].Text = "19" + VB.Left(strJumin, 2) + "-" + VB.Mid(item.JUMIN.Trim(), 3, 2) + "-" + VB.Mid(item.JUMIN.Trim(), 5, 2);
            }
            else if (VB.Mid(strJumin, 7, 1) == "3" || VB.Mid(strJumin, 7, 1) == "4" || VB.Mid(strJumin, 7, 1) == "7" || VB.Mid(strJumin, 7, 1) == "8")
            {
                SS1.ActiveSheet.Cells[6, 6].Text = "20" + VB.Left(strJumin, 2) + "-" + VB.Mid(item.JUMIN.Trim(), 3, 2) + "-" + VB.Mid(item.JUMIN.Trim(), 5, 2);
            }
            else
            {
                SS1.ActiveSheet.Cells[6, 6].Text = "";
            }

            SS1.ActiveSheet.Cells[6, 8].Text = "만 " + item.AGE + "세";

            SS1.ActiveSheet.Cells[7, 2].Text = " " + item.JUSO.Trim();
            SS1.ActiveSheet.Cells[12, 7].Text = VB.Left(item.JEPDATE.Trim(), 4) + "년" + VB.Mid(item.JEPDATE.Trim(), 6, 2) + "월" + VB.Mid(item.JEPDATE.Trim(), 9, 2) + "일";

            //발행일
            SS1.ActiveSheet.Cells[15, 2].Text = VB.Left(clsPublic.GstrSysDate, 4) + "년" + VB.Mid(clsPublic.GstrSysDate, 6, 2) + "월" + VB.Mid(clsPublic.GstrSysDate, 9, 2) + "일";
            SS1.ActiveSheet.Cells[16, 2].Text = ": 포항성모병원";
            SS1.ActiveSheet.Cells[17, 2].Text = ": 경북 포항시 남구 대잠동길 17(대잠동) 건강증진센터";
            SS1.ActiveSheet.Cells[18, 2].Text = ": 054-260-8188  /  (Fax) 054-260-8190";

            if (item.PANJENGDRNO.IsNullOrEmpty())
            {
                SS1.ActiveSheet.Cells[19, 2].Text = "면 허 번 호 : ";
            }
            else
            {
                SS1.ActiveSheet.Cells[19, 2].Text = "면 허 번 호 : " + item.PANJENGDRNO + "  호";
                SS1.ActiveSheet.Cells[19, 5].Text = hb.READ_License_DrName(item.PANJENGDRNO) + "  (인)";
            }
            

        }
    }
}
