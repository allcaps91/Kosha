using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcActPFTMunjin_Print.cs
/// Description     : 폐활량 검사 문진표 출력폼
/// Author          : 이상훈
/// Create Date     : 2020-02-28
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHcActPFT_Prt.frm(FrmHcActPFT_Prt)" />

namespace ComHpcLibB
{
    public partial class frmHcActPFTMunjin_Print : Form
    {
        HicResSpecialService hicResSpecialService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();

        long FnWRTNO;
        string FstrSName;
        string FstrJumin;

        public frmHcActPFTMunjin_Print()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcActPFTMunjin_Print(long nWrtNo, string strSName, string strJumin)
        {
            InitializeComponent();

            FnWRTNO = nWrtNo;
            FstrSName = strSName;
            FstrJumin = strJumin;

            SetEvent();
        }

        void SetEvent()
        {
            hicResSpecialService = new HicResSpecialService();

            this.Load += new EventHandler(eFormLoad);
            this.timer1.Tick += new EventHandler(eTimerTick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            clsCompuInfo.SetComputerInfo();
            ComFunc.ReadSysDate(clsDB.DbCon);

            hf.SetDojangImage(SSPFT, 44, 17, clsType.User.IdNumber);

            timer1.Enabled = true;
        }

        void eTimerTick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            this.Hide();
            Application.DoEvents();

            fn_Print_Report_Main();

            this.Close();
        }

        void fn_Print_Report_Main()
        {
            string strResult = "";
            string strTemp = "";
            string strPftSabun = "";
            int nRow = 0;
            int nCol = 0;
            string sDirPath = "";
            string strFile = "";
            bool blnExist = false;

            fn_Spread_Item_Clear();

            SSPFT.ActiveSheet.Cells[5, 3].Text = FstrSName;
            SSPFT.ActiveSheet.Cells[5, 11].Text = VB.Left(FstrJumin, 6);
            SSPFT.ActiveSheet.Cells[5, 16].Text = VB.Right(FstrJumin, 7);

            HIC_RES_SPECIAL list = hicResSpecialService.GetItembyWrtNo(FnWRTNO);

            if (!list.IsNullOrEmpty())
            {
                if (list.BMI != "")
                {
                    SSPFT.ActiveSheet.Cells[31, 8].Text = "허리둘레:" + list.BMI + "Cm";
                }

                if (list.WEIGHT != 0)
                {
                    SSPFT.ActiveSheet.Cells[26, 17].Text = list.WEIGHT.To<string>();
                }

                if (list.HEIGHT != 0)
                {
                    SSPFT.ActiveSheet.Cells[26, 14].Text = list.HEIGHT.To<string>();
                }

                //폐활량검사 경험
                if (list.GBCAPACITY != "")
                {
                    strResult = list.GBCAPACITY;
                    switch (strResult)
                    {
                        case "0":
                            SSPFT.ActiveSheet.Cells[7, 3].Text = "True";
                            break;
                        case "1":
                            SSPFT.ActiveSheet.Cells[7, 4].Text = "True";
                            break;
                        default:
                            break;
                    }
                }

                if (list.GBGEUMGI != "")
                {
                    strResult = list.GBGEUMGI;
                    for (int i = 0; i <= 6; i++)
                    {
                        if (VB.Pstr(strResult, ",", i) == "1")
                        {
                            if (i < 4)
                            {
                                nRow = 8;
                            }
                            else
                            {
                                nRow = 9;
                            }
                            switch (i)
                            {
                                case 0:
                                    SSPFT.ActiveSheet.Cells[nRow, 3].Text = "True";
                                    break;
                                case 1:
                                case 4:
                                    SSPFT.ActiveSheet.Cells[nRow, 4].Text = "True";
                                    break;
                                case 2:
                                    SSPFT.ActiveSheet.Cells[nRow, 7].Text = "True";
                                    break;
                                case 5:
                                    SSPFT.ActiveSheet.Cells[nRow, 9].Text = "True";
                                    break;
                                case 3:
                                case 6:
                                    SSPFT.ActiveSheet.Cells[nRow, 13].Text = "True";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                //과거또는현재의 질병
                if (list.OLDJILHWAN != "")
                {
                    strResult = list.OLDJILHWAN;
                    for (int i = 1; i <= 11; i++)
                    {
                        if (VB.Pstr(strResult, ",", i) == "1")
                        {
                            if (i < 9)
                            {
                                nRow = 10;
                            }
                            else
                            {
                                nRow = 11;
                            }
                            switch (i)
                            {
                                case 1:
                                    SSPFT.ActiveSheet.Cells[nRow, 3].Text = "True";
                                    break;
                                case 2:
                                case 9:
                                    SSPFT.ActiveSheet.Cells[nRow, 4].Text = "True";
                                    break;
                                case 3:
                                case 10:
                                    SSPFT.ActiveSheet.Cells[nRow, 7].Text = "True";
                                    break;
                                case 4:
                                    SSPFT.ActiveSheet.Cells[nRow, 8].Text = "True";
                                    break;
                                case 5:
                                    SSPFT.ActiveSheet.Cells[nRow, 10].Text = "True";
                                    break;
                                case 6:
                                    SSPFT.ActiveSheet.Cells[nRow, 12].Text = "True";
                                    break;
                                case 7:
                                    SSPFT.ActiveSheet.Cells[nRow, 14].Text = "True";
                                    break;
                                case 8:
                                    SSPFT.ActiveSheet.Cells[nRow, 16].Text = "True";
                                    break;
                                case 11:
                                    SSPFT.ActiveSheet.Cells[nRow, 11].Text = "True";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                //과거또는현재의 질병기타사항
                if (list.OLDJILHWAN_ETC != "")
                {
                    SSPFT.ActiveSheet.Cells[11, 13].Text = list.OLDJILHWAN_ETC;
                }

                //수술경험
                if (list.GBSUSUL != "")
                {
                    strResult = list.GBSUSUL;
                    for (int i = 1; i <= 8; i++)
                    {
                        if (VB.Pstr(strResult, ",", i) == "1")
                        {
                            switch (i)
                            {
                                case 1:
                                    SSPFT.ActiveSheet.Cells[12, 3].Text = "True";
                                    break;
                                case 2:
                                    SSPFT.ActiveSheet.Cells[12, 4].Text = "True";
                                    break;
                                case 3:
                                    SSPFT.ActiveSheet.Cells[12, 5].Text = "True";
                                    break;
                                case 4:
                                    SSPFT.ActiveSheet.Cells[12, 7].Text = "True";
                                    break;
                                case 5:
                                    SSPFT.ActiveSheet.Cells[12, 9].Text = "True";
                                    break;
                                case 6:
                                    SSPFT.ActiveSheet.Cells[12, 11].Text = "True";
                                    break;
                                case 7:
                                    SSPFT.ActiveSheet.Cells[12, 13].Text = "True";
                                    break;
                                case 8:
                                    SSPFT.ActiveSheet.Cells[12, 14].Text = "True";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                //수술경험기타사항
                if (list.GBSUSUL_ETC != "")
                {
                    SSPFT.ActiveSheet.Cells[12, 14].Text = "True";
                    SSPFT.ActiveSheet.Cells[12, 16].Text = list.GBSUSUL_ETC;
                }

                //현재복용약물
                if (list.GBDRUG != "")
                {
                    strResult = list.GBDRUG;                    
                    for (int i = 1; i <= 4; i++)
                    {
                        if (VB.Pstr(strResult, ",", i) != "")
                        {
                            switch (i)
                            {
                                case 1:
                                    SSPFT.ActiveSheet.Cells[13, 3].Text = "True";
                                    break;
                                case 2:
                                    SSPFT.ActiveSheet.Cells[13, 4].Text = "True";
                                    break;
                                case 3:
                                    SSPFT.ActiveSheet.Cells[13, 11].Text = "True";
                                    break;
                                case 4:
                                    SSPFT.ActiveSheet.Cells[13, 14].Text = "True";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                
                //현재복용약물기타사항
                if (list.GBDRUG_ETC != "")
                {
                    SSPFT.ActiveSheet.Cells[13, 3].Text = list.GBDRUG_ETC;
                }

                //흡연력
                if (list.GBSMOKE1 == "1")
                {
                    SSPFT.ActiveSheet.Cells[14, 3].Text = "True";
                }

                if (list.SMOKEYEAR != "")
                {
                    strResult = list.SMOKEYEAR;
                    for (int i = 1; i <= 12; i++)
                    {
                        if (i == 2)
                        {
                            SSPFT.ActiveSheet.Cells[14, 9].Text = VB.Pstr(strResult, ",", i);
                            if (SSPFT.ActiveSheet.Cells[14, 9].Text == "")
                            {
                                SSPFT.ActiveSheet.Cells[14, 9].Text = VB.Pstr(strResult, ",", 3);
                            }
                        }
                        else
                        {
                            SSPFT.ActiveSheet.Cells[14, 4].Text = VB.Pstr(strResult, ",", i);
                        }
                    }
                }

                //금일흡연여부
                if (list.GBSMOKE != "")
                {
                    strResult = list.GBSMOKE;                    
                    switch (strResult)
                    {
                        case "0":
                            SSPFT.ActiveSheet.Cells[14, 15].Text = "True";
                            break;
                        case "1":
                            SSPFT.ActiveSheet.Cells[14, 16].Text = "True";
                            break;
                        case "2":
                            SSPFT.ActiveSheet.Cells[14, 17].Text = "True";
                            break;
                        default:
                            break;
                    }
                }

                //의치착용여부
                if (list.GBDENTY != "")
                {
                    strResult = list.GBDENTY;
                    switch (strResult)
                    {
                        case "0":
                            SSPFT.ActiveSheet.Cells[15, 3].Text = "True";
                            break;
                        case "1":
                            SSPFT.ActiveSheet.Cells[15, 4].Text = "True";
                            break;
                        case "2":
                            SSPFT.ActiveSheet.Cells[15, 5].Text = "True";
                            break;
                        default:
                            break;
                    }
                }

                //호흡곤란정도
                if (list.DYSPNEA != "")
                {
                    strResult = list.DYSPNEA;
                    switch (strResult)
                    {
                        case "0":
                            SSPFT.ActiveSheet.Cells[16, 3].Text = "True";
                            break;
                        case "1":
                            SSPFT.ActiveSheet.Cells[18, 3].Text = "True";
                            break;
                        case "2":
                            SSPFT.ActiveSheet.Cells[19, 3].Text = "True";
                            break;
                        case "3":
                            SSPFT.ActiveSheet.Cells[20, 3].Text = "True";
                            break;
                        case "4":
                            SSPFT.ActiveSheet.Cells[21, 3].Text = "True";
                            break;
                        default:
                            break;
                    }
                }

                //검사자세
                if (list.POSTURE == "1")
                {
                    SSPFT.ActiveSheet.Cells[25, 6].Text = "True";
                }
                else
                {
                    SSPFT.ActiveSheet.Cells[25, 3].Text = "True";
                }

                //장비 S/N
                SSPFT.ActiveSheet.Cells[26, 3].Text = list.SERIALNO;

                //검사협조
                if (list.COOPERATE == "1")
                {
                    SSPFT.ActiveSheet.Cells[25, 16].Text = "True";
                }
                else if (list.COOPERATE == "2")
                {
                    SSPFT.ActiveSheet.Cells[25, 17].Text = "True";
                }
                else
                {
                    SSPFT.ActiveSheet.Cells[25, 14].Text = "True";
                }

                //검사자의견
                for (int i = 1; i < VB.L(list.PSOGEN, "{}") - 1; i++)
                {
                    strTemp = VB.Pstr(list.PSOGEN, "{}", i);

                    if (i < 10)
                    {
                        nCol = 3;
                    }
                    else
                    {
                        nCol = 13;
                    }

                    if (strTemp == "1")
                    {
                        switch (i)
                        {
                            case 0:
                            case 9:
                                SSPFT.ActiveSheet.Cells[27, nCol].Text = "True";
                                break;
                            case 1:
                            case 12:
                                SSPFT.ActiveSheet.Cells[30, nCol].Text = "True";
                                break;
                            case 2:
                            case 13:
                                SSPFT.ActiveSheet.Cells[31, nCol].Text = "True";
                                break;
                            case 3:
                            case 15:
                                SSPFT.ActiveSheet.Cells[33, nCol].Text = "True";
                                break;
                            case 4:
                                SSPFT.ActiveSheet.Cells[37, nCol].Text = "True";
                                break;
                            case 5:
                                SSPFT.ActiveSheet.Cells[39, nCol].Text = "True";
                                break;
                            case 6:
                                SSPFT.ActiveSheet.Cells[40, nCol].Text = "True";
                                break;
                            case 7:
                                SSPFT.ActiveSheet.Cells[41, nCol].Text = "True";
                                break;
                            case 8:
                                SSPFT.ActiveSheet.Cells[43, nCol].Text = "True";
                                break;
                            case 10:
                                SSPFT.ActiveSheet.Cells[28, nCol].Text = "True";
                                break;
                            case 11:
                                SSPFT.ActiveSheet.Cells[29, nCol].Text = "True";
                                break;
                            case 14:
                                SSPFT.ActiveSheet.Cells[32, nCol].Text = "True";
                                break;
                            case 16:
                                SSPFT.ActiveSheet.Cells[34, nCol].Text = "True";
                                break;
                            default:
                                break;
                        }
                    }
                }

                //검사기타의견
                SSPFT.ActiveSheet.Cells[37, 13].Text = list.PETCSOGEN;

                SSPFT.ActiveSheet.Cells[44, 2].Text = VB.Left(list.PFTDATE, 4) + " 년 ";
                SSPFT.ActiveSheet.Cells[44, 2].Text = VB.Mid(list.PFTDATE, 6, 2) + " 월 ";
                SSPFT.ActiveSheet.Cells[44, 2].Text = VB.Mid(list.PFTDATE, 9, 2) + " 일";

                SSPFT.ActiveSheet.Cells[44, 15].Text = hb.READ_JikwonName(list.PFTSABUN.To<string>());
                strPftSabun = list.PFTSABUN.To<string>();

                //간호사 싸인 이미지 로드
                if (strPftSabun != "")
                {
                    sDirPath = "c:\\temp\\temp_image\\";
                    strFile = "dj" + strPftSabun + ".txt";
                    hf.SignImage_Spread_Set(SSPFT, 44, 17, strPftSabun, "C", sDirPath, strFile);
                }
            }

            if (MessageBox.Show("출력하시겠습니까?" , "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SSPFT, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        void fn_Spread_Item_Clear()
        {
            SSPFT.ActiveSheet.Cells[5, 3].Text = "";
            SSPFT.ActiveSheet.Cells[5, 11].Text = "";
            SSPFT.ActiveSheet.Cells[5, 16].Text = "";

            SSPFT.ActiveSheet.Cells[7, 3].Text = "";
            SSPFT.ActiveSheet.Cells[7, 4].Text = "";

            SSPFT.ActiveSheet.Cells[8, 3].Text = "";
            SSPFT.ActiveSheet.Cells[8, 4].Text = "";
            SSPFT.ActiveSheet.Cells[8, 5].Text = "";
            SSPFT.ActiveSheet.Cells[8, 13].Text = "";

            SSPFT.ActiveSheet.Cells[9, 4].Text = "";
            SSPFT.ActiveSheet.Cells[9, 9].Text = "";
            SSPFT.ActiveSheet.Cells[9, 13].Text = "";

            SSPFT.ActiveSheet.Cells[10, 3].Text = "";
            SSPFT.ActiveSheet.Cells[10, 4].Text = "";
            SSPFT.ActiveSheet.Cells[10, 6].Text = "";
            SSPFT.ActiveSheet.Cells[10, 8].Text = "";
            SSPFT.ActiveSheet.Cells[10, 10].Text = "";
            SSPFT.ActiveSheet.Cells[10, 12].Text = "";
            SSPFT.ActiveSheet.Cells[10, 14].Text = "";
            SSPFT.ActiveSheet.Cells[10, 16].Text = "";

            SSPFT.ActiveSheet.Cells[11, 4].Text = "";
            SSPFT.ActiveSheet.Cells[11, 7].Text = "";
            SSPFT.ActiveSheet.Cells[11, 11].Text = "";

            SSPFT.ActiveSheet.Cells[12, 3].Text = "";
            SSPFT.ActiveSheet.Cells[12, 4].Text = "";
            SSPFT.ActiveSheet.Cells[12, 5].Text = "";
            SSPFT.ActiveSheet.Cells[12, 7].Text = "";
            SSPFT.ActiveSheet.Cells[12, 9].Text = "";
            SSPFT.ActiveSheet.Cells[12, 11].Text = "";
            SSPFT.ActiveSheet.Cells[12, 13].Text = "";
            SSPFT.ActiveSheet.Cells[12, 14].Text = "";

            SSPFT.ActiveSheet.Cells[13, 3].Text = "";
            SSPFT.ActiveSheet.Cells[13, 4].Text = "";

            SSPFT.ActiveSheet.Cells[14, 3].Text = "";
            SSPFT.ActiveSheet.Cells[14, 4].Text = "";
            SSPFT.ActiveSheet.Cells[14, 9].Text = "";
            SSPFT.ActiveSheet.Cells[14, 15].Text = "";
            SSPFT.ActiveSheet.Cells[14, 16].Text = "";
            SSPFT.ActiveSheet.Cells[14, 17].Text = "";

            SSPFT.ActiveSheet.Cells[15, 3].Text = "";
            SSPFT.ActiveSheet.Cells[15, 4].Text = "";
            SSPFT.ActiveSheet.Cells[15, 5].Text = "";

            SSPFT.ActiveSheet.Cells[16, 3].Text = "";
            SSPFT.ActiveSheet.Cells[18, 3].Text = "";
            SSPFT.ActiveSheet.Cells[19, 3].Text = "";
            SSPFT.ActiveSheet.Cells[20, 3].Text = "";
            SSPFT.ActiveSheet.Cells[21, 3].Text = "";

            SSPFT.ActiveSheet.Cells[25, 3].Text = "";
            SSPFT.ActiveSheet.Cells[25, 6].Text = "";
            SSPFT.ActiveSheet.Cells[25, 14].Text = "";
            SSPFT.ActiveSheet.Cells[25, 16].Text = "";
            SSPFT.ActiveSheet.Cells[25, 17].Text = "";

            SSPFT.ActiveSheet.Cells[26, 3].Text = "";
            SSPFT.ActiveSheet.Cells[26, 14].Text = "";
            SSPFT.ActiveSheet.Cells[26, 17].Text = "";

            SSPFT.ActiveSheet.Cells[27, 3].Text = "";
            SSPFT.ActiveSheet.Cells[27, 13].Text = "";

            SSPFT.ActiveSheet.Cells[28, 13].Text = "";
            SSPFT.ActiveSheet.Cells[29, 13].Text = "";
            SSPFT.ActiveSheet.Cells[30, 3].Text = "";
            SSPFT.ActiveSheet.Cells[30, 13].Text = "";
            SSPFT.ActiveSheet.Cells[31, 3].Text = "";
            SSPFT.ActiveSheet.Cells[31, 3].Text = "";
            SSPFT.ActiveSheet.Cells[31, 8].Text = "";
            SSPFT.ActiveSheet.Cells[32, 13].Text = "";
            SSPFT.ActiveSheet.Cells[33, 3].Text = "";
            SSPFT.ActiveSheet.Cells[33, 13].Text = "";
            SSPFT.ActiveSheet.Cells[34, 13].Text = "";
            SSPFT.ActiveSheet.Cells[37, 3].Text = "";
            SSPFT.ActiveSheet.Cells[37, 13].Text = "";
            SSPFT.ActiveSheet.Cells[39, 3].Text = "";
            SSPFT.ActiveSheet.Cells[40, 3].Text = "";
            SSPFT.ActiveSheet.Cells[41, 3].Text = "";
            SSPFT.ActiveSheet.Cells[42, 3].Text = "";
            SSPFT.ActiveSheet.Cells[43, 3].Text = "";

            SSPFT.ActiveSheet.Cells[44, 2].Text = "";
            SSPFT.ActiveSheet.Cells[44, 15].Text = "";
            SSPFT.ActiveSheet.Cells[44, 17].Text = "";
        }
    }
}
