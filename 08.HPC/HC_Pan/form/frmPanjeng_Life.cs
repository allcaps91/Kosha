using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ComEmrBase;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmPanjeng_Life.cs
/// Description     : 생활습관처방전
/// Author          : 이상훈
/// Create Date     : 2020-05-21
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm1차일반특수판정_2019.frm(Frm1차일반특수판정_2019) " />

namespace HC_Pan
{
    public partial class frmPanjeng_Life : Form
    {
        HicSpcPanjengService hicSpcPanjengService = null;
        HicResSpecialService hicResSpecialService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicJepsuService hicJepsuService = null;
        HicSpcPanhisService hicSpcPanhisService = null;
        HicCodeService hicCodeService = null;
        HicOrgancodeService hicOrgancodeService = null;
        HicBcodeService hicBcodeService = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;
        ComHpcLibBService comHpcLibBService = null;
        HicMcodeService hicMcodeService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HeaJepsuService heaJepsuService = null;
        HicJepsuMunjinNightService hicJepsuMunjinNightService = null;
        HicMunjinNightService hicMunjinNightService = null;
        HicResBohum1JepsuService hicResBohum1JepsuService = null;
        HicResultService hicResultService = null;
        BasDoctorService basDoctorService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcType ht = new clsHcType();
        clsHcCombo hc = new clsHcCombo();

        long FnWrtNo;
        long FnPano;
        string FstrJumin;
        string FstrSex;
        string FstrJepDate2;
        string FstrPano;    // 원무행정의 등록번호
        string FstrJong;    // 건진종류
        string FstrPtno;
        long FnAge;
        string FstrPtNo;

        string FstrJob;

        long FnWrtno1;      //1차
        long FnWrtno2;      //2차
        string FstrCOMMIT;
        string FstrUCodes;
        string FstrSaveGbn;

        string FstrTFlag;

        string FstrGbOHMS;
        string FstrMCode;
        string FstrYuhe;

        long FnRow;

        string FstrROWID;
        string FstrYROWID;              //약속판정 ROWID
        string FstrPROWID;
        string FstrGbSPC;               //일반+특수여부(Y/N)
        string FstrPanOk1;
        string FstrPanOk2;
        long FnPan2Row;
        string FstrOK;
        string FstrXrayRead;
        string FstrWrtnoDisplay;
        string FstrOldSpcPan;           //특수판정코드 변경 여부

        string FstrJepDate;             //2차 접수일자
        string FstrSpcTable;            //취급물질테이블(P.HIC_SPC_PANJENG H:HIC_SPC_PANHIS)

        string FnA118;                  //'하지기능(0.정상 1.정상B 2,의심R)
        string FnA119;                  //'보행장애(1.무 2.유 3.검사불가)
        string FnA120;                  //'평형성의심(0.정상 1.정상B 2.의심R)

        List<string> FstrNotAddPanList = new List<string>();       //2015-09-01일부 추가판정 제외 그룹코드 목록
        bool FbAutoPanjeng;             //자동판정 저장 여부(True/False)
        string[] FstrHabit = new string[18];

        List<string> FstrKind = new List<string>();

        List<HC_PANJENG_PATLIST> FPatListItem;

        string FstrGbHea;   //검진당일 종검수검 여부

        public frmPanjeng_Life()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicSpcPanjengService = new HicSpcPanjengService();
            hicResSpecialService = new HicResSpecialService();
            hicJepsuService = new HicJepsuService();
            hicSpcPanhisService = new HicSpcPanhisService();
            hicCodeService = new HicCodeService();
            hicOrgancodeService = new HicOrgancodeService();
            hicBcodeService = new HicBcodeService();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            comHpcLibBService = new ComHpcLibBService();
            hicMcodeService = new HicMcodeService();
            hicResultExCodeService = new HicResultExCodeService();
            heaJepsuService = new HeaJepsuService();
            hicJepsuMunjinNightService = new HicJepsuMunjinNightService();
            hicMunjinNightService = new HicMunjinNightService();
            hicResBohum1JepsuService = new HicResBohum1JepsuService();
            hicResultService = new HicResultService();
            basDoctorService = new BasDoctorService();

            this.Load += new EventHandler(eFormLoad);
            this.btnSave.Click += new EventHandler(eBtnClick);
        }

        void SetControl()
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            fn_Combo_Add();
            fn_Screen_Clear();

            fn_Screen_Display();
        }

        void fn_Screen_Display()
        {
            int nREAD = 0;
            string strDAT = "";
            string strTSmoke1 = "";
            string strTSmoke2 = "";
            string strDrink = "";
            long nTOT = 0;
            string strSLIP1 = "";
            string strSLIP2 = "";
            string strSLIP3 = "";
            string strSLIP4 = "";
            string strSLIP5 = "";
            long[] nJemsu = new long[5];
            double nBiman1 = 0;
            long nBiman2 = 0;
            long nIlsu1 = 0;
            long nTime11 = 0;
            long nTime12 = 0;
            long nIlsu2 = 0;
            long nTime21 = 0;
            long nTime22 = 0;
            long nTime1 = 0;
            long nTime2 = 0;

            string strTemp1 = "";
            string strTemp2 = "";
            string strTemp = "";
            double nDrink1 = 0;
            double nDrink2 = 0;
            string str음주 = "";
            string str음주1 = "";
            string str음주2 = "";

            //2019년기준
            string str보통음주 = "";
            string str보통음주1 = "";
            string str최대음주 = "";
            string str최대음주1 = "";

            strSLIP1 = "";
            strSLIP2 = "";
            strSLIP3 = "";
            strSLIP4 = "";
            strSLIP5 = "";
            strTSmoke1 = "";

            List<string> strCodeList = new List<string>();

            for (int i = 0; i < 5; i++)
            {
                nJemsu[i] = 0;
            }

            txtLifeSogen1.Text = "";
            txtLifeSogen2.Text = "";

            //생활습관,신체활동 등
            HIC_RES_BOHUM1_JEPSU list = hicResBohum1JepsuService.GetItembyWrtNo(FnWrtNo);

            str음주 = "";
            str보통음주 = ""; str최대음주 = "";
            str보통음주1 = ""; str최대음주1 = "";

            //보통음주
            nDrink1 = 0;
            nDrink2 = 0;

            if (list.TMUN0003 == "4")
            {
                str음주 = "0";    //비음주자
            }
            else
            {
                //보통음주
                strTemp = list.TMUN0005;
                if (!strTemp.IsNullOrEmpty())
                {
                    //소주
                    if (VB.Pstr(strTemp, ";", 1) == "1")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (4 / 7);
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 4;
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 90;
                                break;
                            default:
                                break;
                        }
                    }
                    //맥주
                    else if (VB.Pstr(strTemp, ";", 1) == "2")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (200 / 350);
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (500 / 350);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "캔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 350;
                                break;
                            default:
                                break;
                        }
                    }
                    //양주
                    else if (VB.Pstr(strTemp, ";", 1) == "3")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (500 / 45);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 45;
                                break;
                            default:
                                break;
                        }
                    }
                    //막걸리
                    else if (VB.Pstr(strTemp, ";", 1) == "4")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (750 / 300);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 300;
                                break;
                            default:
                                break;
                        }
                    }
                    //와인
                    else if (VB.Pstr(strTemp, ";", 1) == "5")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (750 / 150);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 150;
                                break;
                            default:
                                break;
                        }
                    }
                }

                strTemp = list.TMUN0006;
                if (!strTemp.IsNullOrEmpty())
                {
                    //소주
                    if (VB.Pstr(strTemp, ";", 1) == "1")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (4 / 7);
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 4;
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 90;
                                break;
                            default:
                                break;
                        }
                    }
                    //맥주
                    else if (VB.Pstr(strTemp, ";", 1) == "2")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (200 / 350);
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (500 / 350);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "캔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 350;
                                break;
                            default:
                                break;
                        }
                    }
                    //양주
                    else if (VB.Pstr(strTemp, ";", 1) == "3")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (500 / 45);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 45;
                                break;
                            default:
                                break;
                        }
                    }
                    //막걸리
                    else if (VB.Pstr(strTemp, ";", 1) == "4")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (750 / 300);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 300;
                                break;
                            default:
                                break;
                        }
                    }
                    //와인
                    else if (VB.Pstr(strTemp, ";", 1) == "5")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (750 / 150);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 150;
                                break;
                            default:
                                break;
                        }
                    }
                }

                strTemp = list.TMUN0007;
                if (!strTemp.IsNullOrEmpty())
                {
                    //소주
                    if (VB.Pstr(strTemp, ";", 1) == "1")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (4 / 7);
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 4;
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 90;
                                break;
                            default:
                                break;
                        }
                    }
                    //맥주
                    else if (VB.Pstr(strTemp, ";", 1) == "2")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (200 / 350);
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (500 / 350);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "캔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 350;
                                break;
                            default:
                                break;
                        }
                    }
                    //양주
                    else if (VB.Pstr(strTemp, ";", 1) == "3")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (500 / 45);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 45;
                                break;
                            default:
                                break;
                        }
                    }
                    //막걸리
                    else if (VB.Pstr(strTemp, ";", 1) == "4")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (750 / 300);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 300;
                                break;
                            default:
                                break;
                        }
                    }
                    //와인
                    else if (VB.Pstr(strTemp, ";", 1) == "5")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (750 / 150);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 150;
                                break;
                            default:
                                break;
                        }
                    }
                }
                nDrink1 += double.Parse(list.TMUN0004);

                switch (list.TMUN0003)
                {
                    case "1":
                        str보통음주 = (nDrink1 * 1).ToString();  //일주일
                        break;
                    case "2":
                        str보통음주 = (nDrink1 / 4).ToString();  //한달
                        break;
                    case "3":
                        str보통음주 = (nDrink1 / 48).ToString();  //1년
                        break;
                    default:
                        break;
                }

                nDrink1 = 0;
                //최대음주
                strTemp = list.TMUN0008;
                if (!strTemp.IsNullOrEmpty())
                {
                    //소주
                    if (VB.Pstr(strTemp, ";", 1) == "1")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (4 / 7);
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 4;
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 90;
                                break;
                            default:
                                break;
                        }
                    }
                    //맥주
                    else if (VB.Pstr(strTemp, ";", 1) == "2")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (200 / 350);
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (500 / 350);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "캔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 350;
                                break;
                            default:
                                break;
                        }
                    }
                    //양주
                    else if (VB.Pstr(strTemp, ";", 1) == "3")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (500 / 45);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 45;
                                break;
                            default:
                                break;
                        }
                    }
                    //막걸리
                    else if (VB.Pstr(strTemp, ";", 1) == "4")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (750 / 300);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 300;
                                break;
                            default:
                                break;
                        }
                    }
                    //와인
                    else if (VB.Pstr(strTemp, ";", 1) == "5")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (750 / 150);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 150;
                                break;
                            default:
                                break;
                        }
                    }
                }

                strTemp = list.TMUN0009;
                if (!strTemp.IsNullOrEmpty())
                {
                    //소주
                    if (VB.Pstr(strTemp, ";", 1) == "1")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (4 / 7);
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 4;
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 90;
                                break;
                            default:
                                break;
                        }
                    }
                    //맥주
                    else if (VB.Pstr(strTemp, ";", 1) == "2")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (200 / 350);
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (500 / 350);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "캔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 350;
                                break;
                            default:
                                break;
                        }
                    }
                    //양주
                    else if (VB.Pstr(strTemp, ";", 1) == "3")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (500 / 45);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 45;
                                break;
                            default:
                                break;
                        }
                    }
                    //막걸리
                    else if (VB.Pstr(strTemp, ";", 1) == "4")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (750 / 300);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 300;
                                break;
                            default:
                                break;
                        }
                    }
                    //와인
                    else if (VB.Pstr(strTemp, ";", 1) == "5")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (750 / 150);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 150;
                                break;
                            default:
                                break;
                        }
                    }
                }

                strTemp = list.TMUN0010;
                if (!strTemp.IsNullOrEmpty())
                {
                    //소주
                    if (VB.Pstr(strTemp, ";", 1) == "1")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (4 / 7);
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 4;
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 90;
                                break;
                            default:
                                break;
                        }
                    }
                    //맥주
                    else if (VB.Pstr(strTemp, ";", 1) == "2")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (200 / 350);
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (500 / 350);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "캔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 350;
                                break;
                            default:
                                break;
                        }
                    }
                    //양주
                    else if (VB.Pstr(strTemp, ";", 1) == "3")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (500 / 45);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 45;
                                break;
                            default:
                                break;
                        }
                    }
                    //막걸리
                    else if (VB.Pstr(strTemp, ";", 1) == "4")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (750 / 300);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 300;
                                break;
                            default:
                                break;
                        }
                    }
                    //와인
                    else if (VB.Pstr(strTemp, ";", 1) == "5")
                    {
                        switch (VB.Pstr(strTemp, ";", 3))
                        {
                            case "잔":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * 1;
                                break;
                            case "병":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) * (750 / 150);
                                break;
                            case "CC":
                                nDrink1 += long.Parse(VB.Pstr(strTemp, ";", 2)) / 150;
                                break;
                            default:
                                break;
                        }
                    }
                }

                nDrink1 = nDrink1 * double.Parse(list.TMUN0004);
                switch (list.TMUN0003)
                {
                    case "1":
                        str최대음주 = (nDrink1 * 1).ToString();  //일주일
                        break;
                    case "2":
                        str최대음주 = (nDrink1 / 4).ToString();  //한달
                        break;
                    case "3":
                        str최대음주 = (nDrink1 / 48).ToString();  //1년
                        break;
                    default:
                        break;
                }

                str최대음주1 = nDrink1.ToString();
                str보통음주1 = (VB.Fix(int.Parse(str보통음주) / 7)).ToString();

                if (FstrSex == "M" && FnAge < 65)
                {
                    if (string.Compare(str보통음주, "14") > 0)
                    {
                        str음주1 = "OK";                      //절주
                    }
                }
                else if (FstrSex == "M" && FnAge >= 65)
                {
                    if (string.Compare(str보통음주, "7") > 0)
                    {
                        str음주1 = "OK";                      //절주
                    }
                }
                else if (FstrSex == "F" && FnAge < 65)
                {
                    if (string.Compare(str보통음주, "7") > 0)
                    {
                        str음주1 = "OK";                      //절주
                    }
                }
                else if (FstrSex == "F" && FnAge >= 65)
                {
                    if (string.Compare(str보통음주, "3") > 0)
                    {
                        str음주1 = "OK";                      //절주
                    }
                }

                if (FstrSex == "M")
                {
                    if (string.Compare(str최대음주1, "4") > 4)
                    {
                        str음주2 = "OK"; //절주
                    }
                }
                else if (FstrSex == "F")
                {
                    if (string.Compare(str최대음주1, "3") > 4)
                    {
                        str음주2 = "OK"; //절주
                    }
                }
            }

            //생활습관처방전
            HIC_RES_BOHUM1 list2 = hicResBohum1Service.GetItemByWrtno(FnWrtNo);

            if (!list2.IsNullOrEmpty())
            {
                strSLIP1 = list2.SLIP_SMOKE;
                strSLIP2 = list2.SLIP_DRINK;
                strSLIP3 = list2.SLIP_ACTIVE;
                strSLIP4 = list2.SLIP_FOOD;
                strSLIP5 = list2.SLIP_BIMAN;
                strTSmoke1 = list2.T_SMOKE1;
                strTSmoke2 = list2.TMUN0097;
                strDrink = list2.TMUN0003;
                txtLifeSogen1.Text = list2.SLIP_LIFESOGEN1;
                txtLifeSogen2.Text = list2.SLIP_LIFESOGEN2;
            }

            //생활습관평가도구표 결과값을 읽음

            strCodeList.Clear();
            strCodeList.Add("A143");
            strCodeList.Add("A144");
            strCodeList.Add("A145");
            strCodeList.Add("A146");
            strCodeList.Add("A147");
            strCodeList.Add("A130");
            strCodeList.Add("A129");

            List<HIC_RESULT> list3 = hicResultService.GetExCodeResultbyWrtNoExCode(FnWrtNo, strCodeList);

            nREAD = list3.Count;
            for (int i = 0; i < nREAD; i++)
            {
                switch (list3[i].EXCODE)
                {
                    case "A143":
                        nJemsu[0] = long.Parse(list3[i].RESULT);    //흡연
                        break;
                    case "A144":
                        nJemsu[0] = long.Parse(list3[i].RESULT);    //음주
                        break;
                    case "A145":
                        nJemsu[0] = long.Parse(list3[i].RESULT);    //운동
                        break;
                    case "A146":
                        nJemsu[0] = long.Parse(list3[i].RESULT);    //영양
                        break;
                    case "A147":
                        nJemsu[0] = long.Parse(list3[i].RESULT);    //비만
                        break;
                    default:
                        break;
                }
            }

            //금연처방전
            if (tabSmoke.Visible == true)
            {
                txtSmoke0.Text = nJemsu[0].ToString();
                if (strSLIP1.IsNullOrEmpty())
                {
                    //흡연상태
                    cboSmoke1.SelectedIndex = 0;

                    if (strTSmoke1.IsNullOrEmpty() && strTSmoke2.IsNullOrEmpty())
                    {
                        cboSmoke1.SelectedIndex = 1;
                        cboSmoke2.SelectedIndex = 0;
                    }
                    else if (strTSmoke1 == "1")
                    {
                        cboSmoke1.SelectedIndex = 3;
                        txtLifeSogen1.Text += "금연필요/";
                    }
                    else if (strTSmoke1 == "2")
                    {
                        cboSmoke1.SelectedIndex = 2;
                        cboSmoke2.SelectedIndex = 1;
                    }
                    else if (strTSmoke1 == "1")
                    {
                        cboSmoke1.SelectedIndex = 4;
                    }

                    //니코틴의존도
                    if (int.Parse(txtSmoke0.Text) <= 3)
                    {
                        cboSmoke2.SelectedIndex = 1;
                    }
                    else if (int.Parse(txtSmoke0.Text) >= 4 && int.Parse(txtSmoke0.Text) <= 6)
                    {
                        cboSmoke2.SelectedIndex = 2;
                    }
                    else if (int.Parse(txtSmoke0.Text) >= 7)
                    {
                        cboSmoke2.SelectedIndex = 3;
                    }
                }
                else
                {
                    //흡연상태
                    strDAT = VB.Pstr(strSLIP1, ";", 1);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboSmoke1.Items.Count; i++)
                        {
                            cboSmoke1.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboSmoke1.Text, ".", 1))
                            {
                                cboSmoke1.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    //니코틴의존도
                    strDAT = VB.Pstr(strSLIP1, ";", 2);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboSmoke2.Items.Count; i++)
                        {
                            cboSmoke2.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboSmoke2.Text, ".", 1))
                            {
                                cboSmoke2.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    //금연계획단계
                    strDAT = VB.Pstr(strSLIP1, ";", 3);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboSmoke3.Items.Count; i++)
                        {
                            cboSmoke3.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboSmoke3.Text, ".", 1))
                            {
                                cboSmoke3.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    //금연처방
                    for (int i = 4; i <= 9; i++)
                    {
                        CheckBox chkSmoking4 = (Controls.Find("chkSmoking4" + (i - 4).ToString(), true)[0] as CheckBox);
                        chkSmoking4.Checked = VB.Pstr(strSLIP1, ";", i) == "1" ? true : false;
                    }

                    //금연처방-기타
                    txtSmoke4.Text = VB.Pstr(strSLIP1, ";", 10);
                    //금단증상
                    for (int i = 11; i <= 16; i++)
                    {
                        CheckBox chkSmoking5 = (Controls.Find("chkSmoking5" + (i - 11).ToString(), true)[0] as CheckBox);
                        chkSmoking5.Checked = VB.Pstr(strSLIP1, ";", i) == "1" ? true : false;
                    }
                    txtSmoke6.Text = VB.Pstr(strSLIP1, ";", 17);
                    txtSmoke7.Text = VB.Pstr(strSLIP1, ";", 18);
                }
            }

            //음주처방전
            if (tabDrink.Visible == true)
            {
                txtDrink1.Text = nJemsu[1].ToString();
                if (strSLIP2.IsNullOrEmpty())
                {
                    //음주
                    if (int.Parse(txtDrink1.Text) >= 16)
                    {
                        cboDrink1.SelectedIndex = 4;
                        txtLifeSogen1.Text += "절주필요/";
                    }
                    else
                    {
                        cboDrink1.SelectedIndex = 3;
                        txtLifeSogen1.Text += "절주필요/";
                    }
                }
                else
                {
                    //음주관련 질환
                    for (int i = 2; i <= 9; i++)
                    {
                        CheckBox chkDrink0 = (Controls.Find("chkDrink0" + (i - 2).ToString(), true)[0] as CheckBox);
                        chkDrink0.Checked = VB.Pstr(strSLIP2, ";", i) == "1" ? true : false;
                    }

                    //음주상태
                    strDAT = VB.Pstr(strSLIP2, ";", 10);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboDrink1.Items.Count; i++)
                        {
                            cboDrink1.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboDrink1.Text, ".", 1))
                            {
                                cboDrink1.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    //금주처방
                    for (int i = 11; i <= 16; i++)
                    {
                        CheckBox chkDrink2 = (Controls.Find("chkDrink2" + (i - 11).ToString(), true)[0] as CheckBox);
                        chkDrink2.Checked = VB.Pstr(strSLIP2, ";", i) == "1" ? true : false;
                    }
                    txtDrink3.Text = VB.Pstr(strSLIP2, ";", 17);
                    txtDrink4.Text = VB.Pstr(strSLIP2, ";", 18);
                }
            }

            //운동처방전
            if (tabExercise.Visible == true)
            {
                HIC_RES_BOHUM1 list4 = hicResBohum1Service.GetItemByWrtno(FnWrtNo);

                //신체활동
                nTime1 = 0;
                nTime2 = 0;
                if (!list4.TMUN0011.IsNullOrEmpty())
                {
                    nTime1 = long.Parse(VB.Pstr(list4.TMUN0011, ":", 1)) * 60;
                    nTime1 += long.Parse(VB.Pstr(list4.TMUN0011, ":", 2));
                    nTime1 *= long.Parse(list4.T_ACTIVE1) * 2;
                }
                if (!list4.TMUN0012.IsNullOrEmpty())
                {
                    nTime2 = long.Parse(VB.Pstr(list4.TMUN0011, ":", 1)) * 60;
                    nTime2 += long.Parse(VB.Pstr(list4.TMUN0011, ":", 2));
                    nTime2 *= long.Parse(list4.T_ACTIVE2);
                }
                txtHealth0.Text = (nTime1 + nTime2).ToString();

                //운동상태
                if (int.Parse(txtHealth0.Text) < 150)
                {
                    cboHealth1.SelectedIndex = 1;
                    if (strSLIP3.IsNormalized())
                    {
                        txtLifeSogen2.Text += "신체활동 필요/";
                    }
                }
                else if (int.Parse(txtHealth0.Text) >= 150 && int.Parse(txtHealth0.Text) < 300)
                {
                    cboHealth1.SelectedIndex = 2;
                }
                else if (int.Parse(txtHealth0.Text) >= 300)
                {
                    cboHealth1.SelectedIndex = 3;
                }

                //근력운동
                cboHealth4.SelectedIndex = 2;
                if (int.Parse(list4.T_ACTIVE3) < 2)
                {
                    cboHealth4.SelectedIndex = 1;
                    if (strSLIP3.IsNormalized())
                    {
                        txtLifeSogen2.Text += "근력운동 필요/";
                    }
                    chkHealth9.Checked = true;
                }

                if (!strSLIP3.IsNullOrEmpty())
                {
                    //운동종류
                    for (int i = 2; i <= 11; i++)
                    {
                        CheckBox chkHealth = (Controls.Find("chkHealth" + (i - 2).ToString(), true)[0] as CheckBox);
                        chkHealth.Checked = VB.Pstr(strSLIP3, ";", i) == "1" ? true : false;
                    }
                    if (cboHealth4.SelectedIndex == 1)
                    {
                        chkHealth9.Checked = true;
                    }
                    txtHealth_Etc1.Text = VB.Pstr(strSLIP3, ";", 12);
                    //운동시간
                    strDAT = VB.Pstr(strSLIP3, ";", 13);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboHealth2.Items.Count; i++)
                        {
                            cboHealth2.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboHealth2.Text, ".", 1))
                            {
                                cboHealth2.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    txtHealth_Etc2.Text = VB.Pstr(strSLIP3, ";", 14);
                    //운동횟수
                    strDAT = VB.Pstr(strSLIP3, ";", 15);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboHealth3.Items.Count; i++)
                        {
                            cboHealth3.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboHealth3.Text, ".", 1))
                            {
                                cboHealth3.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    //호전가능
                    for (int i = 16; i <= 26; i++)
                    {
                        CheckBox chkHealth2 = (Controls.Find("chkHealth2" + (i - 16).ToString(), true)[0] as CheckBox);
                        chkHealth2.Checked = VB.Pstr(strSLIP3, ";", i) == "1" ? true : false;
                    }
                    txtHealth_Etc3.Text = VB.Pstr(strSLIP3, ";", 27);
                    txtHealth_Etc4.Text = VB.Pstr(strSLIP3, ";", 28);
                }
            }

            //영양처방전
            if (tabDiet.Visible == true)
            {
                txtDiet0.Text = nJemsu[4].ToString();
                if (strSLIP4.IsNullOrEmpty())
                {
                    cboDiet.SelectedIndex = 3;
                    txtLifeSogen2.Text += "식생활습관 개선/";
                }
                else if (int.Parse(txtDiet0.Text) >= 28 && int.Parse(txtDiet0.Text) <= 38)
                {
                    cboDiet.SelectedIndex = 2;
                }
                else if (int.Parse(txtDiet0.Text) >= 39)
                {
                    cboDiet.SelectedIndex = 1;
                }
            }
            else
            {
                //식생활습관
                strDAT = VB.Pstr(strSLIP4, ";", 1);
                if (strDAT.IsNullOrEmpty())
                {

                    for (int i = 0; i < cboDiet.Items.Count; i++)
                    {
                        cboDiet.SelectedIndex = i;
                        if (strDAT == VB.Pstr(cboDiet.Text, ".", 1))
                        {
                            cboDiet.SelectedIndex = i;
                            break;
                        }
                    }
                }

                //개선처방
                for (int i = 2; i <= 11; i++)
                {
                    CheckBox chkDiet = (Controls.Find("chkDiet" + (i - 2).ToString(), true)[0] as CheckBox);
                    chkDiet.Checked = VB.Pstr(strSLIP4, ";", i) == "1" ? true : false;
                }
                //호전가능
                for (int i = 12; i <= 20; i++)
                {
                    CheckBox chkDiet2 = (Controls.Find("chkDiet2" + (i - 12).ToString(), true)[0] as CheckBox);
                    chkDiet2.Checked = VB.Pstr(strSLIP4, ";", i) == "1" ? true : false;
                }
                txtDiet_ETC1.Text = VB.Pstr(strSLIP4, ";", 21);
                txtDiet_ETC2.Text = VB.Pstr(strSLIP4, ";", 22);
            }

            //비만처방전
            if (tabBiman.Visible == true)
            {
                txtBiman0.Text = nJemsu[5].ToString();
                if (strSLIP5.IsNullOrEmpty())
                {
                    nBiman1 = 0;
                    nBiman2 = 0;

                    List<string> strExCode = new List<string>();

                    strExCode.Clear();
                    strExCode.Add("A115");
                    strExCode.Add("A117");

                    List<HIC_RESULT> list5 = hicResultService.GetExCodeResultbyWrtNoExCode(FnWrtNo, strExCode);

                    if (list5.Count > 0)
                    {
                        for (int i = 0; i < list5.Count; i++)
                        {
                            if (list5[i].EXCODE == "A117")
                            {
                                nBiman1 = long.Parse(list5[i].RESCODE);
                            }
                            if (list5[i].EXCODE == "A115")
                            {
                                nBiman2 = long.Parse(list5[i].RESCODE);
                            }
                        }
                    }

                    //비만도체크
                    if (nBiman1 >= 18.5 && nBiman1 <= 24.9)
                    {
                        cboBiman0.SelectedIndex = 1;
                    }
                    else if (nBiman1 >= 25 && nBiman1 <= 29.9)
                    {
                        cboBiman0.SelectedIndex = 2;
                        txtLifeSogen2.Text += "체중관리/";
                    }
                    else if (nBiman1 >= 30)
                    {
                        cboBiman0.SelectedIndex = 3;
                        txtLifeSogen2.Text += "체중관리/";
                    }

                    //복부비만
                    switch (FstrSex)
                    {
                        case "M":
                            if (nBiman2 >= 90)
                            {
                                cboBiman1.SelectedIndex = 1;
                                txtLifeSogen2.Text += "복부비만관리/";
                            }
                            else if (nBiman2 < 90)
                            {
                                cboBiman1.SelectedIndex = 2;
                            }
                            break;
                        case "F":
                            if (nBiman2 >= 85)
                            {
                                cboBiman1.SelectedIndex = 1;
                                txtLifeSogen2.Text += "복부비만관리/";
                            }
                            else if (nBiman2 < 85)
                            {
                                cboBiman1.SelectedIndex = 2;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    //체중
                    strDAT = VB.Pstr(strSLIP5, ";", 1);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboBiman0.Items.Count; i++)
                        {
                            cboBiman0.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboBiman0.Text, ".", 1))
                            {
                                cboBiman0.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    //비만
                    strDAT = VB.Pstr(strSLIP5, ";", 2);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboBiman1.Items.Count; i++)
                        {
                            cboBiman1.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboBiman2.Text, ".", 1))
                            {
                                cboBiman1.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    //질환발생위험
                    strDAT = VB.Pstr(strSLIP5, ";", 3);
                    if (!strDAT.IsNullOrEmpty())
                    {
                        for (int i = 0; i < cboBiman2.Items.Count; i++)
                        {
                            cboBiman2.SelectedIndex = i;
                            if (strDAT == VB.Pstr(cboBiman2.Text, ".", 1))
                            {
                                cboBiman2.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    txtBiman0.Text = VB.Pstr(strSLIP5, ";", 4);
                    txtBiman01.Text = VB.Pstr(strSLIP5, ";", 5);
                    txtBiman02.Text = VB.Pstr(strSLIP5, ";", 6);
                    txtBiman03.Text = VB.Pstr(strSLIP5, ";", 7);
                    //비만처방
                    for (int i = 8; i <= 14; i++)
                    {
                        CheckBox chkBiman = (Controls.Find("chkBiman" + (i - 8).ToString(), true)[0] as CheckBox);
                        chkBiman.Checked = VB.Pstr(strSLIP5, ";", i) == "1" ? true : false;
                    }

                    txtBiman20.Text = VB.Pstr(strSLIP5, ";", 15);
                    txtBiman21.Text = VB.Pstr(strSLIP5, ";", 16);
                    //호잔가능
                    for (int i = 17; i < 26; i++)
                    {
                        CheckBox chkBiman2 = (Controls.Find("chkBiman2" + (i - 17).ToString(), true)[0] as CheckBox);
                        chkBiman2.Checked = VB.Pstr(strSLIP5, ";", i) == "1" ? true : false;
                    }
                    txtBiman3.Text = VB.Pstr(strSLIP5, ";", 27);
                    txtBiman4.Text = VB.Pstr(strSLIP5, ";", 28);
                }
            }

            if (txtLifeSogen1.Text == "")
            {
                txtLifeSogen1.Text = "특이소견 없음/";
            }
            if (txtLifeSogen2.Text == "")
            {
                txtLifeSogen2.Text = "특이소견 없음/";
            }

            //생활습관도구 판넬을 처음것을 열기
            for (int i = 0; i <= 4; i++)
            {
                //If SSTab_Life.TabVisible(i) = True Then
                //    SSTab_Life.Tab = i
                //    DoEvents
                //    Exit For
                //End If                
            }
        }

        void fn_Combo_Add()
        {
            //금연처방전
            cboSmoke1.Items.Clear();
            cboSmoke1.Items.Add(" ");
            cboSmoke1.Items.Add("0.비흡연자");
            cboSmoke1.Items.Add("1.과거 흡연자");
            cboSmoke1.Items.Add("2.현재 흡연자");
            cboSmoke1.Items.Add("3.전자담배 단독 사용자");

            cboSmoke2.Items.Clear();
            cboSmoke2.Items.Add(" ");
            cboSmoke2.Items.Add("1.낮음(0~3점)");
            cboSmoke2.Items.Add("2.중간정도(4~6점)");
            cboSmoke2.Items.Add("3.높은정도(7~10점)");

            cboSmoke3.Items.Clear();
            cboSmoke3.Items.Add(" ");
            cboSmoke3.Items.Add("1.금연 계획 이전단계");
            cboSmoke3.Items.Add("2.금연 계획 단계");
            cboSmoke3.Items.Add("3.금연 준비 단계");
            cboSmoke3.Items.Add("4.금연 시도");
            cboSmoke3.Items.Add("5.금연 유지");

            cboDrink1.Items.Clear();
            cboDrink1.Items.Add(" ");
            cboDrink1.Items.Add("1.비음주자");
            cboDrink1.Items.Add("2.적정 음주자");
            cboDrink1.Items.Add("3.위험 음주자");
            cboDrink1.Items.Add("4.알코올 사용장애 의심");

            cboHealth1.Items.Clear();
            cboHealth1.Items.Add(" ");
            cboHealth1.Items.Add("1.신체활동 부족");
            cboHealth1.Items.Add("2.기본 신체활동");
            cboHealth1.Items.Add("3.건강증진 신체활동");

            cboHealth2.Items.Clear();
            cboHealth2.Items.Add(" ");
            cboHealth2.Items.Add("1.10분");
            cboHealth2.Items.Add("2.15~30분");
            cboHealth2.Items.Add("3.30분 이상");
            cboHealth2.Items.Add("4.기타");

            cboHealth3.Items.Clear();
            cboHealth3.Items.Add(" ");
            cboHealth3.Items.Add("1주일에 1~2회");
            cboHealth3.Items.Add("1주일에 3~4회");
            cboHealth3.Items.Add("1주일에 5회이상");

            cboHealth4.Items.Clear();
            cboHealth4.Items.Add(" ");
            cboHealth4.Items.Add("1.근력운동 부족");
            cboHealth4.Items.Add("2.근력운동 적절");

            //식생활습관
            cboDiet.Items.Clear();
            cboDiet.Items.Add(" ");
            cboDiet.Items.Add("1.양호");
            cboDiet.Items.Add("2.보통");
            cboDiet.Items.Add("3.불량");
            cboDiet.SelectedIndex = 0;

            //체중
            cboBiman0.Items.Clear();
            cboBiman0.Items.Add(" ");
            cboBiman0.Items.Add("1.정상체중");
            cboBiman0.Items.Add("2.과체중");
            cboBiman0.Items.Add("3.비만");
            cboBiman0.SelectedIndex = 0;

            //비만
            cboBiman1.Items.Clear();
            cboBiman1.Items.Add(" ");
            cboBiman1.Items.Add("1.복부비만");
            cboBiman1.Items.Add("2.정상");
            cboBiman1.SelectedIndex = 0;

            //질환발생위험
            cboBiman2.Items.Clear();
            cboBiman2.Items.Add(" ");
            cboBiman2.Items.Add("1.낮음");
            cboBiman2.Items.Add("2.보통");
            cboBiman2.Items.Add("3.다소증가");
            cboBiman2.Items.Add("4.어느정도증가");
            cboBiman2.Items.Add("5.상당히증가");
            cboBiman2.Items.Add("6.매우증가");
            cboBiman2.SelectedIndex = 0;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSave)
            {
                int nBCnt = 0;
                int nRCNT = 0;
                int nUCNT = 0;
                string strPanjeng = "";
                string strGbErFlag = "";
                string strTemp = "";
                string strChk = "";
                string strGbHabit = "";
                string[] strHabit = new string[5];
                string strJinchal1 = "";
                string strJinchal2 = "";
                string strOK = "";
                string strT40Feel = "";
                string strT66Stat = "";
                string strTSogen = "";
                string strMsg = "";
                string strSave = "";
                string strFood = "";
                string strDrink = "";
                long nLicense = 0;
                int result = 0;

                if (tabSmoke.Visible == true)
                {
                    if (VB.Left(cboSmoke1.Text, 1) == "2")  //현재 흡연자
                    {
                        if (cboSmoke3.Text == "")
                        {
                            MessageBox.Show("생활습관 금연처방전 결과가 공란입니다.", "저장 불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            FstrOK = "N";
                            return;
                        }
                        //금연처방
                        strChk = "";
                        for (int i = 0; i <= 5; i++)
                        {
                            CheckBox chkSmoking4 = (Controls.Find("chkSmoking4" + (i).ToString(), true)[0] as CheckBox);
                            if (chkSmoking4.Checked == true)
                            {
                                strChk = "Y";
                                break;
                            }
                        }
                        if (strChk == "")
                        {
                            MessageBox.Show("금연처방이 공란입니다.", "저장 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FstrOK = "N";
                            return;
                        }
                    }
                }
                if (tabDrink.Visible == true)
                {
                    if (cboDrink1.Text == "")
                    {
                        MessageBox.Show("생활습관 음주상태가 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FstrOK = "N";
                        return;
                    }

                    if (VB.Left(cboDrink1.Text, 1) == "3" || VB.Left(cboDrink1.Text, 1) == "4")
                    {
                        //금주/절주 처방
                        strChk = "";
                        for (int i = 0; i <= 5; i++)
                        {
                            CheckBox chkDrink2 = (Controls.Find("chkDrink2" + (i).ToString(), true)[0] as CheckBox);
                            if (chkDrink2.Checked == true)
                            {
                                strChk = "Y";
                                break;
                            }
                        }
                        if (strChk == "")
                        {
                            MessageBox.Show("금주/절주 처방이 공란입니다.", "저장 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FstrOK = "N";
                            return;
                        }
                    }
                }

                if (tabExercise.Visible == true)
                {
                    if (cboHealth1.Text == "")
                    {
                        MessageBox.Show("운동상태가 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FstrOK = "N";
                        return;
                    }

                    if (cboHealth4.Text == "")
                    {
                        MessageBox.Show("근력운동이 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FstrOK = "N";
                        return;
                    }

                    if (VB.Left(cboHealth1.Text, 1) == "1" || VB.Left(cboHealth4.Text, 1) == "1")
                    {
                        //운동종류
                        for (int i = 0; i <= 9; i++)
                        {
                            strChk = "";
                            CheckBox chkHealth = (Controls.Find("chkHealth" + (i).ToString(), true)[0] as CheckBox);
                            if (chkHealth.Checked == true)
                            {
                                strChk = "Y";
                                break;
                            }
                        }
                        if (strChk == "")
                        {
                            MessageBox.Show("운동종류 처방이 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FstrOK = "N";
                            return;
                        }

                        if (cboHealth2.Text == "")
                        {
                            MessageBox.Show("운동시간이 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FstrOK = "N";
                            return;
                        }

                        if (cboHealth3.Text == "")
                        {
                            MessageBox.Show("운동횟수가 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FstrOK = "N";
                            return;
                        }
                    }

                    if (tabDiet.Visible == true)
                    {
                        if (cboDiet.Text == "")
                        {
                            MessageBox.Show("식생활습관 결과가 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FstrOK = "N";
                            return;
                        }
                        if (VB.Left(cboDiet.Text, 1) == "3")
                        {
                            //식생활 개선처방
                            strChk = "";
                            for (int i = 0; i <= 9; i++)
                            {
                                CheckBox chkDiet = (Controls.Find("chkDiet" + (i).ToString(), true)[0] as CheckBox);
                                if (chkDiet.Checked == true)
                                {
                                    strChk = "Y";
                                    break;
                                }
                            }
                            if (strChk == "")
                            {
                                MessageBox.Show("식생활개선 처방이 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                FstrOK = "N";
                                return;
                            }
                        }
                    }

                    if (tabBiman.Visible == true)
                    {   
                        if (VB.Left(cboBiman1.Text, 1) == "3" || VB.Left(cboBiman1.Text, 1) == "1")
                        {
                            strChk = "";
                            for (int i = 0; i <= 6; i++)
                            {
                                CheckBox chkBiman = (Controls.Find("chkBiman" + (i).ToString(), true)[0] as CheckBox);
                                if (chkBiman.Checked == true)
                                {
                                    strChk = "Y";
                                    break;
                                }
                            }

                            if (txtBiman20.Text != "")
                            {
                                strChk = "Y";
                            }
                            if (txtBiman21.Text != "")
                            {
                                strChk = "Y";
                            }
                            if (strChk == "")
                            {
                                MessageBox.Show("비만 처방이 공란입니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                FstrOK = "N";
                                return;
                            }
                        }

                        if (int.Parse(txtBiman02.Text) >= 13)
                        {
                            MessageBox.Show("비만처방 개월수는 12개월 이하여야합니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FstrOK = "N";
                            return;
                        }

                        for (int i = 1; i <= txtBiman01.Text.Length; i++)
                        {
                            if (VB.Mid(txtBiman01.Text, i, 1) == ".")
                            {
                                MessageBox.Show("비만처방전 목표KG은 정수만 입력해주세요.(소수점이하 불가능)", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                FstrOK = "N";
                                return;
                            }
                        }

                        for (int i = 1; i <= txtBiman03.Text.Length; i++)
                        {
                            if (VB.Mid(txtBiman03.Text, i, 1) == ".")
                            {
                                MessageBox.Show("비만처방전 목표KG은 정수만 입력해주세요.(소수점이하 불가능)", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                FstrOK = "N";
                                return;
                            }
                        }
                    }
                }
                //의사면허번호 조회
                nLicense = clsHcVariable.GnHicLicense;
                if (clsHcVariable.GbHicAdminSabun == true)
                {
                    nLicense = long.Parse(txtPanDrNo.Text);
                }
                else
                {
                    txtPanDrNo.Text = clsHcVariable.GnHicLicense.ToString();
                    lblDrName.Text = clsType.User.UserName;
                }

                //퇴사일 이후 판정금지
                if (clsHcVariable.GstrReDay != "")
                {
                    DateTime date1 = new DateTime();
                    DateTime date2 = new DateTime();
                    date1 = Convert.ToDateTime(dtpPanDate.Text);
                    date2 = Convert.ToDateTime(clsHcVariable.GstrReDay);
                    if (date1 > date2)
                    {
                        MessageBox.Show("판정일자가 " + clsHcVariable.GstrReDay + "일보다 큼" + "\r\n" + "판정일자를 다른날짜로 옮겨주시길 바랍니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FstrOK = "N";
                        return;
                    }
                }

                hb.READ_HIC_DrSabun(txtPanDrNo.Text);
                //의사당직 휴무 여부확인
                if (hb.READ_DOCTOR_SCH2(clsType.User.DrCode, dtpPanDate.Text) == "NO")
                {
                    MessageBox.Show("해당 판정일자는 과장님의 진료가 없거나 휴일입니다." + "\r\n" + "판정일자를 다른날짜로 옮겨주시길 바랍니다.", "판정 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FstrOK = "N";
                    return;
                }

                strFood = "";
                for (int i = 0; i <= 9; i++)
                {
                    CheckBox chkDiet = (Controls.Find("chkDiet" + (i).ToString(), true)[0] as CheckBox);
                    if (chkDiet.Checked == true)
                    {
                        strFood = "OK";
                    }
                }

                if (strFood == "")
                {
                    chkDiet8.Checked = true;
                }

                strDrink = "";
                for (int i = 0; i <= 5; i++)
                {
                    CheckBox chkDrink2 = (Controls.Find("chkDrink2" + (i).ToString(), true)[0] as CheckBox);
                    if (chkDrink2.Checked == true)
                    {
                        strDrink = "OK";
                    }
                }

                if (strDrink == "")
                {
                    chkDrink20.Checked = true;
                }

                //저장
                fn_DB_Update_LifeSlip();
            }
        }

        void fn_DB_Update_LifeSlip()
        {
            int result = 0;
            string strSLIP1 = "";
            string strSLIP2 = "";
            string strSLIP3 = "";
            string strSLIP4 = "";
            string strSLIP5 = "";
            string strSLIP6 = "";
            string strSLIP7 = "";

            //금연처방전
            if (tabSmoke.Visible == true)
            {
                txtSmoke4.Text = txtSmoke4.Text.Replace(";", ",");
                txtSmoke6.Text = txtSmoke6.Text.Replace(";", ",");
                txtSmoke7.Text = txtSmoke7.Text.Replace(";", ",");
                strSLIP1 = VB.Pstr(cboSmoke1.Text, ".", 1) + ";";      //흡연상태(1)
                strSLIP1 += VB.Pstr(cboSmoke2.Text, ".", 1) + ";";     //니코틴의존도(2)
                strSLIP1 += VB.Pstr(cboSmoke3.Text, ".", 1) + ";";     //금연계획단계(3)
                //금연처방(4-9)
                for (int i = 0; i <= 5; i++)
                {
                    CheckBox chkSmoking4 = (Controls.Find("chkSmoking4" + (i).ToString(), true)[0] as CheckBox);
                    strSLIP1 += chkSmoking4.Checked == true ? "1" : "0" + ";";
                }

                strSLIP1 += txtSmoke4.Text + ";"; //금연계획단계(10)
                //금단증상극복(11-16)
                for (int i = 0; i <= 5; i++)
                {
                    CheckBox chkSmoking5 = (Controls.Find("chkSmoking5" + (i).ToString(), true)[0] as CheckBox);
                    strSLIP1 += chkSmoking5.Checked == true ? "1" : "0" + ";";
                }
                strSLIP1 += txtSmoke6.Text + ";"; //추가조치사항(17)
                strSLIP1 += txtSmoke7.Text + ";"; 
            }

            //음주처방전
            if (tabDrink.Visible == true)
            {
                txtDrink3.Text = txtDrink3.Text.Replace(";", ",");
                txtDrink4.Text = txtDrink4.Text.Replace(";", ",");
                strSLIP2 = txtDrink1.Text + ";";                  //평가점수(1)
                //영향질환(2-9)
                for (int i = 0; i <= 7; i++)
                {
                    CheckBox chkDrink0 = (Controls.Find("chkDrink0" + (i).ToString(), true)[0] as CheckBox);
                    strSLIP2 += chkDrink0.Checked == true ? "1" : "0" + ";";
                }
                strSLIP2 += VB.Pstr(cboDrink1.Text, ".", 1) + ";"; //음주상태(10)
                //금주/절주처방(11-16)
                for (int i = 0; i <= 5; i++)
                {
                    CheckBox chkDrink2 = (Controls.Find("chkDrink2" + (i).ToString(), true)[0] as CheckBox);
                    strSLIP2 += chkDrink2.Checked == true ? "1" : "0" + ";";
                }
                strSLIP2 += txtDrink3.Text + ";"; //추가조치사항(17)
                strSLIP2 += txtDrink4.Text + ";"; //기타질환의심
            }

            //운동처방전
            if (tabExercise.Visible == true)
            {
                txtHealth_Etc1.Text = txtHealth_Etc1.Text.Replace(";", ",");
                txtHealth_Etc2.Text = txtHealth_Etc2.Text.Replace(";", ",");
                txtHealth_Etc3.Text = txtHealth_Etc3.Text.Replace(";", ",");
                txtHealth_Etc4.Text = txtHealth_Etc4.Text.Replace(";", ",");

                strSLIP3 = VB.Pstr(cboHealth1.Text, ".", 1) + ";";            //운동상태(1)
                //운동종류(2-11)
                for (int i = 0; i <= 9; i++)
                {
                    CheckBox chkHealth = (Controls.Find("chkHealth" + (i).ToString(), true)[0] as CheckBox);
                    strSLIP3 += chkHealth.Checked == true ? "1" : "0" + ";";
                }
                strSLIP3 += txtHealth_Etc1.Text + ";";              //기타(12)
                strSLIP3 += VB.Pstr(cboHealth2.Text, ".", 1) + ";"; //운동시간(13)
                strSLIP3 += txtHealth_Etc2.Text + ";";              //기타(14)
                strSLIP3 += VB.Pstr(cboHealth3.Text, ".", 1) + ";"; //운동횟수(15)
                //호전가능(16-26)
                for (int i = 0; i <= 10; i++)
                {
                    CheckBox chkHealth2 = (Controls.Find("chkHealth2" + (i).ToString(), true)[0] as CheckBox);
                    strSLIP3 += chkHealth2.Checked == true ? "1" : "0" + ";";
                }
                strSLIP3 += txtHealth_Etc3.Text + ";";              //기타(27)
                strSLIP3 += txtHealth_Etc4.Text + ";";              //추가조치사항(28)
                strSLIP3 += VB.Pstr(cboHealth4.Text, ".", 1) + ";"; //근력운동(29)
            }

            //영양처방전
            if (tabDiet.Visible == true)
            {
                txtDiet_ETC1.Text = txtDiet_ETC1.Text.Replace(";", ",");
                txtDiet_ETC2.Text = txtDiet_ETC2.Text.Replace(";", ",");
                strSLIP4 = VB.Pstr(cboDiet.Text, ".", 1) + ";";            //식생활습관(1)

                //식생활개선(2-11)
                for (int i = 0; i <= 9; i++)
                {
                    CheckBox chkDiet = (Controls.Find("chkDiet" + (i).ToString(), true)[0] as CheckBox);
                    strSLIP4 += chkDiet.Checked == true ? "1" : "0" + ";";
                }
                //호전가능(12-20)
                for (int i = 0; i <= 8; i++)
                {
                    CheckBox chkDiet2 = (Controls.Find("chkDiet2" + (i).ToString(), true)[0] as CheckBox);
                    strSLIP4 += chkDiet2.Checked == true ? "1" : "0" + ";";
                }
                strSLIP4 += txtDiet_ETC1.Text + ";";  //기타(21)
                strSLIP4 += txtDiet_ETC2.Text + ";";  //추가조치사항(22)
            }

            //비만처방전
            if(tabBiman.Visible == true)
            {
                txtBiman20.Text = txtBiman20.Text.Replace(";", ",");
                txtBiman21.Text = txtBiman21.Text.Replace(";", ",");
                txtBiman3.Text = txtBiman3.Text.Replace(";", ",");
                txtBiman4.Text = txtBiman4.Text.Replace(";", ",");

                strSLIP5 = VB.Pstr(cboBiman0.Text, ".", 1) + ";";  //체중(1)
                strSLIP5 += VB.Pstr(cboBiman1.Text, ".", 1) + ";"; //비만(2)
                strSLIP5 += VB.Pstr(cboBiman2.Text, ".", 1) + ";"; //질환발생위험(3)
                strSLIP5 += txtBiman00.Text + ";";        //목표체중(4)
                strSLIP5 += txtBiman01.Text + ";";        //목표체중(5)
                strSLIP5 += txtBiman02.Text + ";";        //목표체중(6)
                strSLIP5 += txtBiman03.Text + ";";        //목표체중(7)
                //비만처방(8-14)
                for (int i = 0; i <= 6; i++)
                {
                    CheckBox chkBiman = (Controls.Find("chkBiman" + (i).ToString(), true)[0] as CheckBox);
                    strSLIP5 += chkBiman.Checked == true ? "1" : "0" + ";";
                }
                strSLIP5 += txtBiman20.Text + ";";       //약물치료(15)
                strSLIP5 += txtBiman21.Text + ";";       //기타(16)
                //호전가능(17-26)
                for (int i = 0; i <= 9; i++)
                {
                    CheckBox chkBiman2 = (Controls.Find("chkBiman2" + (i).ToString(), true)[0] as CheckBox);
                    strSLIP5 += chkBiman2.Checked == true ? "1" : "0" + ";";
                }
                strSLIP5 += txtBiman3.Text + ";";      //기타(27)
                strSLIP5 += txtBiman4.Text + ";";       //기타의견(28)
            }

            strSLIP6 = txtLifeSogen1.Text.Replace("'", "`");
            strSLIP7 = txtLifeSogen2.Text.Replace("'", "`");

            clsDB.setBeginTran(clsDB.DbCon);

            //판정결과를 DB에 UPDATE
            HIC_RES_BOHUM1 item = new HIC_RES_BOHUM1();

            item.SLIP_SMOKE = strSLIP1;
            item.SLIP_DRINK = strSLIP2;
            item.SLIP_ACTIVE = strSLIP3;
            item.SLIP_FOOD = strSLIP4;
            item.SLIP_BIMAN = strSLIP5;
            item.SLIP_LIFESOGEN1 = strSLIP6;
            item.SLIP_LIFESOGEN2 = strSLIP7;
            item.WRTNO = FnWrtNo;

            result = hicResBohum1Service.UpdateLifebyWrtNo(item);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("생활습관처방전 DB에 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void fn_Screen_Clear()
        {
            clsHcVariable.GnPanB_Etc = 0;
            clsHcVariable.Gstr_PanB_Etc = "";
            clsHcType.HFA.Sogen = "";
            FbAutoPanjeng = false;

            //FnPano = 0;             FstrJumin = "";             FnWrtNo = 0;
            FstrSex = ""; FstrROWID = "";
            FstrUCodes = ""; FstrCOMMIT = ""; FnPano = 0;
            FnWrtno1 = 0; FnWrtno2 = 0;
            FstrSaveGbn = ""; FstrPano = ""; FstrJumin = "";
            FstrGbOHMS = ""; FstrGbSPC = "";
            FstrPanOk1 = ""; FstrPanOk2 = "";
            FstrTFlag = ""; FstrJepDate2 = ""; FstrSpcTable = "";

            //금연처방전
            txtSmoke0.Text = "";
            cboSmoke1.SelectedIndex = -1;
            cboSmoke2.SelectedIndex = -1;
            cboSmoke3.SelectedIndex = -1;

            for (int i = 0; i <= 5; i++)
            {
                CheckBox chkSmoking4 = (Controls.Find("chkSmoking4" + (i).ToString(), true)[0] as CheckBox);
                CheckBox chkSmoking5 = (Controls.Find("chkSmoking5" + (i).ToString(), true)[0] as CheckBox);
                chkSmoking4.Checked = false;
                chkSmoking5.Checked = false;
            }

            txtSmoke4.Text = "";
            txtSmoke6.Text = "";
            txtSmoke7.Text = "";

            //음주/절주 처방전
            txtDrink1.Text = "";
            for (int i = 0; i <= 7; i++)
            {
                CheckBox chkDrink0 = (Controls.Find("chkDrink0" + (i).ToString(), true)[0] as CheckBox);
                chkDrink0.Checked = false;
            }
            cboDrink1.SelectedIndex = -1;

            for (int i = 0; i <= 7; i++)
            {
                CheckBox chkDrink2 = (Controls.Find("chkDrink2" + (i).ToString(), true)[0] as CheckBox);
                chkDrink2.Checked = false;
            }
            txtDrink3.Text = "";
            txtDrink4.Text = "";

            //운동처방전
            txtHealth0.Text = "";
            cboHealth1.SelectedIndex = -1; cboHealth2.SelectedIndex = -1; cboHealth3.SelectedIndex = -1;
            cboHealth1.SelectedIndex = -1; cboHealth2.SelectedIndex = -1; cboHealth3.SelectedIndex = -1;
            txtHealth_Etc1.Text = "";
            txtHealth_Etc2.Text = "";
            txtHealth_Etc3.Text = "";
            txtHealth_Etc4.Text = "";

            for (int i = 0; i <= 10; i++)
            {
                if (i < 10)
                {
                    CheckBox chkHealth = (Controls.Find("chkHealth" + (i).ToString(), true)[0] as CheckBox);
                    chkHealth.Checked = false;
                }
            }

            //영양
            txtDiet0.Text = "";
            cboDiet.Text = "";
            for (int i = 0; i <= 9; i++)
            {
                CheckBox chkDiet = (Controls.Find("chkDiet" + (i).ToString(), true)[0] as CheckBox);
                chkDiet.Checked = false;
                if (i < 9)
                {
                    CheckBox chkDiet2 = (Controls.Find("chkDiet2" + (i).ToString(), true)[0] as CheckBox);
                    chkDiet2.Checked = false;
                }
            }
            txtDiet_ETC1.Text = "";
            txtDiet_ETC2.Text = "";

            //비만
            txtBiman0.Text = "";
            for (int i = 0; i <= 3; i++)
            {
                if (i < 3)
                {
                    ComboBox cboBiman = (Controls.Find("cboBiman" + (i).ToString(), true)[0] as ComboBox);
                    cboBiman.SelectedIndex = -1;
                }
                if (i < 2)
                {
                    TextBox txtBiman2 = (Controls.Find("txtBiman2" + (i).ToString(), true)[0] as TextBox);
                    txtBiman2.Text = "";
                }
                TextBox txtBiman1 = (Controls.Find("txtBiman1" + (i).ToString(), true)[0] as TextBox);
                txtBiman1.Text = "";
            }

            for (int i = 0; i <= 9; i++)
            {
                if (i < 7)
                {
                    CheckBox chkBiman = (Controls.Find("chkBiman" + (i).ToString(), true)[0] as CheckBox);
                    chkBiman.Checked = false;
                }
                CheckBox chkBiman2 = (Controls.Find("chkBiman2" + (i).ToString(), true)[0] as CheckBox);
                chkBiman2.Checked = false;
            }
            txtBiman3.Text = "";
            txtBiman4.Text = "";
        }
    }
}
