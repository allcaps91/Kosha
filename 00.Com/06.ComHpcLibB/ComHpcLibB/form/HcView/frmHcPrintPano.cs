using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPrintPano.cs
/// Description     : 접수증(가셔야할곳) 출력 로직/// Author          : 김경동
/// Create Date     : 2020-07-01
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "" />
namespace ComHpcLibB
{
    public partial class frmHcPrintPano : Form
    {

        HIC_JEPSU nHJ = null;
        FpSpread ssPrint;
        HicCancerResv2Service hicCancerResv2Service = null;

        string FstrEndo = "";
        string[] FstrExamName = new string[12];
        string[] FstrName = new string[12];

        public frmHcPrintPano()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcPrintPano(HIC_JEPSU aHJ, string argEndo, string[] argExamName, string[] argName)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            nHJ = aHJ;
            FstrEndo = argEndo;
            FstrExamName = argExamName;
            FstrName = argName;
        }

        private void SetControl()
        {
            nHJ = new HIC_JEPSU();
            hicCancerResv2Service = new HicCancerResv2Service();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {
            string[] strPrt = new string[12];
            string strPrintName = "";

            this.Hide();

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread SPR = new clsSpread();
            clsPrint CP = new clsPrint();

            for (int i =0; i <=11; i++)
            {
                //TEST
                //strPrt[5] = "Y";

                //if (strPrt[i] == "Y")
                if ( VB.L(FstrExamName[i].ToString(), "^^") > 1)
                {
                    SSPano.ActiveSheet.Cells[2, 3].Text = nHJ.PTNO + "(접수번호: " + nHJ.WRTNO + ")";
                    SSPano.ActiveSheet.Cells[3, 3].Text = nHJ.SNAME + "  ( " + nHJ.SEX + "/" + nHJ.AGE + ")";

                    //테스트
                    //FstrExamName[2] = "위내시경^^"; FstrExamName[5] = "유방단순촬영^^";

                    for (int j = 1; j <= VB.L(FstrExamName[i].ToString(),"^^")-1; j++)
                    {
                        //내시경실 경우 수면/일반/부담율 표시
                        if(i == 2)
                        {
                            SSPano.ActiveSheet.Cells[4, 1].Text = "검 사 항 목 : " + "(" + FstrEndo + ")" + VB.Pstr(FstrExamName[i].ToString(), "^^", j);
                            if (VB.InStr(VB.Pstr(FstrExamName[i].ToString(),"^^",j),"대장")>0)
                            {
                                switch (VB.Left(nHJ.BURATE, 2))
                                {
                                    case "09": SSPano.ActiveSheet.Cells[5, 1].Text = "자 격 사 항 : [대장]: 대상[10%]"; break;
                                    case "11": SSPano.ActiveSheet.Cells[5, 1].Text = "자 격 사 항 : [대장]: 대상[무료]"; break;
                                    default: SSPano.ActiveSheet.Cells[5, 1].Text = "자 격 사 항: [대장]: "+ clsHcType.THNV.hCan1; break;
                                }
                            }
                            else
                            {
                                switch (VB.Left(nHJ.BURATE, 2))
                                {
                                    case "09": SSPano.ActiveSheet.Cells[5, 1].Text = "자 격 사 항 : [위]: 대상[10%]"; break;
                                    case "11": SSPano.ActiveSheet.Cells[5, 1].Text = "자 격 사 항 : [위]: 대상[무료]"; break;
                                    default: SSPano.ActiveSheet.Cells[5, 1].Text = "자 격 사 항: [위]: " + clsHcType.THNV.hCan1; break;
                                }
                            }
                        
                            //암검진예약시 선택한 의사를 인쇄함.
                            HIC_CANCER_RESV2 item = hicCancerResv2Service.GetItemByPtnoRTime(nHJ.PTNO, nHJ.JEPDATE);
                            if (!item.IsNullOrEmpty())
                            {
                                SSPano.ActiveSheet.Cells[6, 1].Text = "희 망 의 사 : " + item.SDOCT;
                            }
                            else
                            {
                                SSPano.ActiveSheet.Cells[6, 1].Text = "";
                            }
                        }
                        else
                        {
                            if (VB.Pstr(FstrExamName[i].ToString(), "^^", j) != "")
                            {
                                SSPano.ActiveSheet.Cells[4, 1].Text = "검 사 항 목 : " + VB.Pstr(FstrExamName[i].ToString(), "^^", j);
                                SSPano.ActiveSheet.Cells[5, 1].Text = "";
                            }
                            SSPano.ActiveSheet.Cells[6, 1].Text = "";
                        }

                        if (i == 2 && nHJ.GBHEAENDO =="Y")
                        {
                            SSPano.ActiveSheet.Cells[7, 1].Text = "장        소 : 내시경(종검 2층) / 챠트";
                        }
                        else
                        {
                            SSPano.ActiveSheet.Cells[7, 1].Text = "장        소 : " + VB.Pstr(FstrName[i].ToString(), "^^", j);
                        }

                        if (i == 2 )
                        {
                            SSPano.ActiveSheet.Cells[10, 1].Text = "              "+"[혈압 :        /         ]";
                        }
                        else if (i == 10 &&  VB.InStr(FstrExamName[i].ToString(), "뇌혈류초음파") >0)
                        {
                            SSPano.ActiveSheet.Cells[8, 1].Text = "    " + "[혈압 :        /         ]";
                            SSPano.ActiveSheet.Cells[9, 1].Text = "    " + "[맥박 :        ]";
                        }
                        else
                        {
                            SSPano.ActiveSheet.Cells[8, 1].Text = "";
                            SSPano.ActiveSheet.Cells[9, 1].Text = "";
                            SSPano.ActiveSheet.Cells[10, 1].Text = "";
                        }

                        SSPano.ActiveSheet.Cells[12, 1].Text = "포 항 성 모 병 원";
                        SSPano.ActiveSheet.Cells[13, 1].Text = "-건 강 증 진 센 터 -";
                        SSPano.ActiveSheet.Cells[14, 1].Text = "인쇄일자: " + DateTime.Now.Year.To<string>() + "-" + DateTime.Now.Month.To<string>() + "-" + DateTime.Now.Day.To<string>();

                        strPrintName = CP.getPmpaBarCodePrinter("신용카드"); //Default :신용카드(접수창구용 설정이름)
                        setMargin = new clsSpread.SpdPrint_Margin(0, 0, 20, 40, 0, 0);
                        setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, false, false);
                        ssPrint = SSPano;
                        SPR.setSpdPrint(ssPrint, false, setMargin, setOption, "", "", strPrintName);

                        ComFunc.Delay(1500);
                    }

                    ssPrint.Dispose();
                    ssPrint = null;

                }
            }

            this.Close();
        }
    }
}
