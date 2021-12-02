using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
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
/// File Name       : frmHaPrintPano.cs
/// Description     : 접수증(가셔야할곳) 출력 로직/// Author          : 김경동
/// Create Date     : 2020-10-07
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "" />
namespace ComHpcLibB
{
    public partial class frmHaPrintPano : Form
    {
        HEA_JEPSU nHJ = null;
        HIC_PATIENT nHP = null;
        FpSpread ssPrint;
        HicCancerResv2Service hicCancerResv2Service = null;
        HicLtdService hicLtdService = null;


        string[] FstrExamName = new string[12];
        string[] FstrName = new string[12];
        string[] FstrContrast = new string[12];
        string[] FstrMsg = new string[12];

        public frmHaPrintPano()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHaPrintPano(HEA_JEPSU aHJ, HIC_PATIENT aHP,string[] argExamName, string[] argName, string[] argContrast, string[] argMsg)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            nHJ = aHJ;
            nHP = aHP;
            FstrExamName = argExamName;
            FstrName = argName;
            FstrContrast = argContrast;
            FstrMsg = argMsg;
        }

        private void SetControl()
        {
            nHJ = new HEA_JEPSU();
            hicCancerResv2Service = new HicCancerResv2Service();
            hicLtdService = new HicLtdService();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {

            int nRow = 0;
            string strPrintName = "";
            string strADDPRINT = "";
            string strLtdNamd = "";

            this.Hide();
            FstrExamName[10] = ""; //스트레스검사
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread SPR = new clsSpread();
            clsPrint CP = new clsPrint();

            if (!nHJ.LTDCODE.IsNullOrEmpty() && nHJ.LTDCODE != 0)
            {
                HIC_LTD item = hicLtdService.GetItembyCode(nHJ.LTDCODE.ToString());
                strLtdNamd = item.NAME;
            }


            for (int i = 0; i < 12; i++)
            {
                if (!FstrExamName[i].IsNullOrEmpty())
                {
                    nRow = 0;
                    SSPano.ActiveSheet.Cells[2, 1].Text = "등록번호: "+ nHJ.PTNO + " (생년월일: " + VB.Left(nHP.JUMIN,6) + ")";
                    SSPano.ActiveSheet.Cells[3, 1].Text = "성    명: "+nHJ.SNAME + " ( " + nHJ.SEX + "/" + nHJ.AGE + ")";

                    if (!FstrExamName[i].ToString().IsNullOrEmpty())
                    {
                        for (int j = 1; j <= VB.L(FstrExamName[i], ","); j++)
                        {
                            nRow = nRow + 1;
                            //SSPano.ActiveSheet.Cells[4+ nRow, 1].Text = "검 사 항 목 : " + FstrExamName[i].ToString();
                            SSPano.ActiveSheet.Cells[3 + nRow, 1].Text = "검 사 항 목 : " + VB.Pstr(FstrExamName[i], ",", j);
                        }

                        if (!FstrName[i].IsNullOrEmpty())
                        {
                            SSPano.ActiveSheet.Cells[4 + nRow, 1].Text = "장      소 : " + FstrName[i].ToString();
                        }

                        SSPano.ActiveSheet.Cells[5 + nRow, 1].Text = "회      사 : "+ strLtdNamd;
                        if (i == 5 && nHP.GBJIKWON == "Y" && nHJ.LTDCODE == 107)
                        {
                            SSPano.ActiveSheet.Cells[6 + nRow, 1].Text = "★대장용종절제술 회사부담 대상자";
                        }

                        if (FstrContrast[i] == "Y")
                        {
                            strADDPRINT = "OK";
                            SSPano.ActiveSheet.Cells[7 + nRow, 1].Text = "3-way extension tube #1,";
                            SSPano.ActiveSheet.Cells[8 + nRow, 1].Text = "Jellco Needle #1,";
                            SSPano.ActiveSheet.Cells[9 + nRow, 1].Text = "Control Line Filter #1";
                        }

                        if (i == 8)
                        {
                            if (!FstrMsg[1].IsNullOrEmpty() || !FstrMsg[9].IsNullOrEmpty())
                            {
                                nRow = nRow + 1;
                                SSPano.ActiveSheet.Cells[10 + nRow, 1].Text = FstrMsg[1];
                            }
                            if (!FstrMsg[2].IsNullOrEmpty() || !FstrMsg[8].IsNullOrEmpty())
                            {
                                nRow = nRow + 1;
                                SSPano.ActiveSheet.Cells[10 + nRow, 1].Text = FstrMsg[2];
                            }
                            if (!FstrMsg[3].IsNullOrEmpty())
                            {
                                nRow = nRow + 1;
                                SSPano.ActiveSheet.Cells[10 + nRow, 1].Text = FstrMsg[3];
                            }
                            if (!FstrMsg[4].IsNullOrEmpty())
                            {
                                nRow = nRow + 1;
                                SSPano.ActiveSheet.Cells[10 + nRow, 1].Text = FstrMsg[4];
                            }
                            if (!FstrMsg[5].IsNullOrEmpty() || !FstrMsg[6].IsNullOrEmpty())
                            {
                                nRow = nRow + 1;
                                SSPano.ActiveSheet.Cells[10 + nRow, 1].Text = FstrMsg[5];
                            }
                            if (!FstrMsg[7].IsNullOrEmpty())
                            {
                                nRow = nRow + 1;
                                SSPano.ActiveSheet.Cells[10 + nRow, 1].Text = FstrMsg[7];
                            }
                        }
                    }

                    SSPano.ActiveSheet.Cells[18, 1].Text = "포 항 성 모 병 원";
                    SSPano.ActiveSheet.Cells[19, 1].Text = "-건 강 증 진 센 터 -";
                    SSPano.ActiveSheet.Cells[20, 1].Text = "인쇄일자: " + DateTime.Now.Year.To<string>() + "-" + DateTime.Now.Month.To<string>() + "-" + DateTime.Now.Day.To<string>();

                    strPrintName = CP.getPmpaBarCodePrinter("신용카드"); //Default :신용카드(접수창구용 설정이름)
                    setMargin = new clsSpread.SpdPrint_Margin(0, 0, 0, 40, 0, 0);
                    setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, false, false);
                    ssPrint = SSPano;
                    SPR.setSpdPrint(ssPrint, false, setMargin, setOption, "", "", strPrintName);

                    ComFunc.Delay(1500);

                    SSPano_Clear();
                }
            }

            if (strADDPRINT == "OK")
            {
                for (int i = 0; i <= 11; i++)
                {
                    if (!FstrExamName[i].IsNullOrEmpty() && FstrContrast[i] == "Y")
                    {
                        nRow = 0;
                        SSPano.ActiveSheet.Cells[2, 1].Text = "등록번호: " + nHJ.PTNO + " (생년월일: " + nHP.JUMIN + ")";
                        SSPano.ActiveSheet.Cells[3, 1].Text = "성    명: " + nHJ.SNAME + " ( " + nHJ.SEX + "/" + nHJ.AGE + ")";

                        if (!FstrExamName[i].ToString().IsNullOrEmpty())
                        {
                            for (int j = 1; j <= VB.L(FstrExamName[i], ","); j++)
                            {
                                nRow = nRow + 1;
                                //SSPano.ActiveSheet.Cells[4+ nRow, 1].Text = "검 사 항 목 : " + FstrExamName[i].ToString();
                                SSPano.ActiveSheet.Cells[3 + nRow, 1].Text = "검 사 항 목 : " + VB.Pstr(FstrExamName[i], ",", j);
                            }

                            if (!FstrName[i].IsNullOrEmpty())
                            {
                                SSPano.ActiveSheet.Cells[4 + nRow, 1].Text = "장      소 : " + FstrName[i].ToString();
                            }

                            SSPano.ActiveSheet.Cells[5 + nRow, 1].Text = "회      사 : ";
                            if (i == 5 && nHP.GBJIKWON == "Y" && nHJ.LTDCODE == 107)
                            {
                                SSPano.ActiveSheet.Cells[6 + nRow, 1].Text = "★대장용종절제술 회사부담 대상자";
                            }

                            if (FstrContrast[i] == "Y")
                            {
                                SSPano.ActiveSheet.Cells[7 + nRow, 1].Text = "3-way extension tube #1,";
                            }

                            if (i == 8)
                            {
                                if (!FstrMsg[1].IsNullOrEmpty() || !FstrMsg[9].IsNullOrEmpty())
                                {
                                    nRow = nRow + 1;
                                    SSPano.ActiveSheet.Cells[10 + nRow, 1].Text = FstrMsg[1];
                                }
                                if (!FstrMsg[2].IsNullOrEmpty() || !FstrMsg[8].IsNullOrEmpty())
                                {
                                    nRow = nRow + 1;
                                    SSPano.ActiveSheet.Cells[10 + nRow, 1].Text = FstrMsg[1];
                                }
                                if (!FstrMsg[3].IsNullOrEmpty())
                                {
                                    nRow = nRow + 1;
                                    SSPano.ActiveSheet.Cells[10 + nRow, 1].Text = FstrMsg[1];
                                }
                                if (!FstrMsg[4].IsNullOrEmpty())
                                {
                                    nRow = nRow + 1;
                                    SSPano.ActiveSheet.Cells[10 + nRow, 1].Text = FstrMsg[1];
                                }
                                if (!FstrMsg[5].IsNullOrEmpty() || !FstrMsg[6].IsNullOrEmpty())
                                {
                                    nRow = nRow + 1;
                                    SSPano.ActiveSheet.Cells[10 + nRow, 1].Text = FstrMsg[1];
                                }
                                if (!FstrMsg[7].IsNullOrEmpty())
                                {
                                    nRow = nRow + 1;
                                    SSPano.ActiveSheet.Cells[10 + nRow, 1].Text = FstrMsg[1];
                                }
                            }
                        }
                        SSPano.ActiveSheet.Cells[18, 1].Text = "포 항 성 모 병 원";
                        SSPano.ActiveSheet.Cells[19, 1].Text = "-건 강 증 진 센 터 -";
                        SSPano.ActiveSheet.Cells[20, 1].Text = "인쇄일자: " + DateTime.Now.Year.To<string>() + "-" + DateTime.Now.Month.To<string>() + "-" + DateTime.Now.Day.To<string>();

                        strPrintName = CP.getPmpaBarCodePrinter("신용카드"); //Default :신용카드(접수창구용 설정이름)
                        setMargin = new clsSpread.SpdPrint_Margin(0, 0, 0, 40, 0, 0);
                        setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, false, false);
                        ssPrint = SSPano;
                        SPR.setSpdPrint(ssPrint, false, setMargin, setOption, "", "", strPrintName);

                        ComFunc.Delay(1500);

                        SSPano_Clear();
                    }
                }
            }

            this.Close();

            void SSPano_Clear()
            {
                for (int i = 1; i <= 18; i++)
                {
                    SSPano.ActiveSheet.Cells[i, 1].Text = "";
                }
            }
        }
    }
}
