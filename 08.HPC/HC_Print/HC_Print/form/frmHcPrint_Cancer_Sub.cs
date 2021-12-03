using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;

using System.Windows.Forms;


/// <summary>
/// Class Name      : HC_Print
/// File Name       : frmHcPrint_Cancer_Sub.cs
/// Description     : 공단암검진 결과지 인쇄
/// Author          : 김경동
/// Create Date     : 2021-02-01
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm검진결과지암_2020.frm(FrmIDateChange)" />


namespace HC_Print
{
    public partial class frmHcPrint_Cancer_Sub : Form
    {

        int FnJulCNT = 0;
        int FnRowCnt = 0;
        int FnChk2 = 0;                     //검진2차 자료표시
        int FnFlag = 0;
        int FnCnt = 0;
        int FnP = 0;                        //스프레드 결절("0"->SS2(0),"1"-> SS2(1))
        int FnChk = 0;
        int FnExamCnt = 0;                  //기타검사건수
        int FnPanCNT = 0;                   //판정건수
        int FnTimeCNT = 0;

        long fnWrtno = 0;
        long FnWRTNO1 = 0;                   //1차 접수번호
        long FnWrtno2 = 0;                   //2차 접수번호
        long FnPano = 0;                    //건진 등록번호

        string FstrUCodes = "";
        string FstrSExams = "";
        string FstrLtdName = "";
        string FstrName = "";
        string FstrJumin = "";
        string FstrWrtno = "";
        string FstrGjjong = "";
        string FstrBasicExam1 = "";         //기본검사항목,줄,칸
        string FstrBasicExam2 = "";         //순음청력검사
        string FstrSQL1 = "";               //기본검사의 검사항목들('A101','A102',...) :   SQL의 IN 항목
        string FstrSQL2 = "";               //순음청력검사 검사항목들('TH11','TH12',...) : SQL의 IN 항목
        string FstrGjYear = "";             //건진년도
        string FstrSex = "";                //성별(검사결과 참고치)
        string FstrDate = "";               //암검진에 사용
        string FstrTongbodate = "";
        string FstrJepdate = "";

        string[] FstrPrtGbn = new string[7];
        long[] FnDrno = new long[7];



        ComFunc cf = new ComFunc();
        clsHaBase hb = new clsHaBase();
        clsHcPrint hp = new clsHcPrint();
        clsHcBill chb = new clsHcBill();
        clsHcFunc hf = new clsHcFunc();

        HicCancerNewJepsuPatientService hicCancerNewJepsuPatientService = null;

        public frmHcPrint_Cancer_Sub()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcPrint_Cancer_Sub(long argWRTNO)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            fnWrtno = argWRTNO;
        }


        private void SetControl()
        {
            hicCancerNewJepsuPatientService = new HicCancerNewJepsuPatientService();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {

            Result_Print_Sub();
            Result_Print_Main();

        }
        private void Result_Print_Sub()
        {

            int nRow = 0;
            int nTemp = 0; //검사항목 별 판정구분 변수
            int nlen = 0;
            int nlen1 = 0;
            int nlen2 = 0;
            int nlen3 = 0;
            int nLungCount = 0;

            string[] strBeomJu = new string[5];
            string strJumin = "";
            string strLtdCode = "";
            string strJepDate = "";
            string strGjYear = "";
            string strData = "";
            string strList = "";
            string strGunDate = "";
            string strPlace = "";

            string strTemp = "";
            string strTemp1 = "";
            string strTemp2 = "";
            string strTemp3 = "";
            string strTemp4 = "";
            string strTemp5 = "";
            string strResult7 = "";
            string strResult8 = "";
            string strResult9 = "";

            string strLungResult = "";
            string strLungResult1 = "";
            string strLungPanJeng = "";
            string strMent = "";
            string strMent1 = "";
            string strMent2 = "";

            string strOK = "";
            string strTEXT = "";
            string strTEXT1 = "";
            string strTEXT2 = "";
            string strTEXT3 = "";
            string strTEXT4 = "";
            string strTEXT5 = "";
            string strTEXT6 = "";
            string strJuso = "";


            HIC_CANCER_NEW_JEPSU_PATIENT item = hicCancerNewJepsuPatientService.GetIetmbyWrtNo(fnWrtno);

            strJumin = item.JUMIN;
            strLtdCode = VB.Format(item.LTDCODE, "#");
            strJepDate = item.JEPDATE.Trim();
            strGjYear = item.GJYEAR.Trim();
            strJuso = item.GBJUSO.Trim();
            strGunDate = item.GUNDATE.Trim();

            //for (int i = 1; i <= 7; i++)
            //{

            //}



            //검진일 읽어오기
            nlen = 0; strTemp1 = ""; strTemp2 = ""; strTemp3 = ""; strTemp4 = "";

            //위암
            if (item.GBSTOMACH.Trim() == "1")
            {
                FstrPrtGbn[1] = "Y";
                //SS1.ActiveSheet.Cells[0, 0].Text = "";

                //위암(병형)
                nlen = VB.Len(item.STOMACH_S);
                strTemp1 = item.STOMACH_S.Trim();
                strTemp2 = item.STOMACH_P.Trim();
                strTemp3 = item.STOMACH_PETC.Trim();

                if (strTemp1.IsNullOrEmpty())
                {

                    SS1.ActiveSheet.Cells[25, 2].Text = "위조영검사" + ComNum.VBLF + "(" + strGunDate + ")";
                    if (item.S_PLACE == "2")
                    {
                        SS1.ActiveSheet.Cells[25, 2].Text += ComNum.VBLF + "■내원 □출장 ";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[25, 2].Text += ComNum.VBLF + "□내원 ■출장 ";
                    }

                    //위암 소견 및 조치사항
                    for (int j = 1; j <= nlen; j++)
                    {
                        //SS1.ActiveSheet.Cells[25, 5].Text = "";
                        if (!VB.Mid(strTemp1, j, 1).IsNullOrEmpty())
                        {
                            if (VB.Mid(strTemp1, j, 1) != "0")
                            {
                                SS1.ActiveSheet.Cells[25, 5].Text += "판독소견: ";
                                switch (VB.Mid(strTemp1, j, 1))
                                {
                                    case "1": SS1.ActiveSheet.Cells[25, 5].Text += "이상소견없음"; break; //정상일때 병변위치 불필요
                                    case "2":
                                        SS1.ActiveSheet.Cells[25, 5].Text += "위염,";
                                        SS1.ActiveSheet.Cells[25, 5].Text += hp.DisPlay_S병변위치(SS1, strTemp2, "2");
                                        break;
                                    case "3":
                                        SS1.ActiveSheet.Cells[25, 5].Text += "위암의심,";
                                        SS1.ActiveSheet.Cells[25, 5].Text += hp.DisPlay_S병변위치(SS1, strTemp2, "3");
                                        break;
                                    case "4":
                                        SS1.ActiveSheet.Cells[25, 5].Text += "조기위암,";
                                        SS1.ActiveSheet.Cells[25, 5].Text += hp.DisPlay_S병변위치(SS1, strTemp2, "4");
                                        break;
                                    case "5":
                                        SS1.ActiveSheet.Cells[25, 5].Text += "진행위암,";
                                        SS1.ActiveSheet.Cells[25, 5].Text += hp.DisPlay_S병변위치(SS1, strTemp2, "5");
                                        break;
                                    case "6":
                                        SS1.ActiveSheet.Cells[25, 5].Text += "양성위궤양,";
                                        SS1.ActiveSheet.Cells[25, 5].Text += hp.DisPlay_S병변위치(SS1, strTemp2, "6");
                                        break;
                                    case "7":
                                        SS1.ActiveSheet.Cells[25, 5].Text += "위용종,";
                                        SS1.ActiveSheet.Cells[25, 5].Text += hp.DisPlay_S병변위치(SS1, strTemp2, "7");
                                        break;
                                    case "8":
                                        SS1.ActiveSheet.Cells[25, 5].Text += "위점막하종양,";
                                        SS1.ActiveSheet.Cells[25, 5].Text += hp.DisPlay_S병변위치(SS1, strTemp2, "8");
                                        break;
                                    case "9":   //위장조영 병형 기타
                                        for (int k = 1; k <= VB.Len(item.STOMACH_B); k++)
                                        {
                                            if (VB.Mid(item.STOMACH_B, k, 1) == "1")
                                            {
                                                switch (VB.Mid(strTemp1, j, 1))
                                                {
                                                    case "1": SS1.ActiveSheet.Cells[25, 5].Text += "식도/위정맥류,"; break;
                                                    case "2": SS1.ActiveSheet.Cells[25, 5].Text += "식도염,"; break;
                                                    case "3": SS1.ActiveSheet.Cells[25, 5].Text += "식도 점막하종양,"; break;
                                                    case "4": SS1.ActiveSheet.Cells[25, 5].Text += "식도암,"; break;
                                                    case "5": SS1.ActiveSheet.Cells[25, 5].Text += "십이지장궤양,"; break;
                                                    case "6": SS1.ActiveSheet.Cells[25, 5].Text += "십이지장악성종양,"; break;
                                                    case "7": SS1.ActiveSheet.Cells[25, 5].Text += "십이지장점막하종양,"; break;
                                                    case "8":
                                                        if (strTemp3.IsNullOrEmpty())
                                                        {
                                                            SS1.ActiveSheet.Cells[25, 5].Text += "기타소견";
                                                        }
                                                        else
                                                        {
                                                            SS1.ActiveSheet.Cells[25, 5].Text += strTemp3;
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                        SS1.ActiveSheet.Cells[25, 5].Text += hp.DisPlay_S병변위치(SS1, strTemp2, "9");
                                        break;

                                    default: break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    SS1.ActiveSheet.Cells[25, 2].Text = "위장조영검사" + ComNum.VBLF;
                    SS1.ActiveSheet.Cells[25, 2].Text += "□내원 □출장";
                    SS1.ActiveSheet.Cells[25, 5].Text = "해당사항 없음";
                }
                nlen = 0; strTemp1 = ""; strTemp2 = ""; strTemp3 = ""; strTemp4 = "";



                //내시경(병형)
                nlen = VB.Len(item.STOMACH_SENDO);
                strTemp1 = item.STOMACH_SENDO.Trim();       //위내시경 병형
                strTemp2 = item.STOMACH_PENDO.Trim();       //위내시경 병변위치
                strTemp3 = item.STOMACH_ENDOETC.Trim();     //위내시경 병변위치기타
                if (strTemp1.IsNullOrEmpty())
                {

                    SS1.ActiveSheet.Cells[26, 2].Text = "위조영검사" + ComNum.VBLF + "(" + strGunDate + ")";
                    if (item.S_PLACE == "2")
                    {
                        SS1.ActiveSheet.Cells[26, 2].Text += ComNum.VBLF + "■내원 □출장 ";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[26, 2].Text += ComNum.VBLF + "□내원 ■출장 ";
                    }

                    for (int j = 1; j <= 3; j++)
                    {
                        if (Convert.ToInt32(VB.Mid(strTemp1, (j * 2) - 1, 2)) >= 01)
                        {
                            SS1.ActiveSheet.Cells[26, 5].Text += "판독소견: ";
                            switch (VB.Mid(strTemp1, (j * 2) - 1, 2))
                            {
                                case "01": SS1.ActiveSheet.Cells[26, 5].Text += "이상소견없음"; break; //정상일때 병변위치 불필요
                                case "21":
                                    SS1.ActiveSheet.Cells[26, 5].Text += "위염,";
                                    SS1.ActiveSheet.Cells[26, 5].Text += hp.DisPlay_S병변위치1(SS1, strTemp2, j);
                                    break;
                                case "22":
                                    SS1.ActiveSheet.Cells[26, 5].Text += "위축성위염,";
                                    SS1.ActiveSheet.Cells[26, 5].Text += hp.DisPlay_S병변위치1(SS1, strTemp2, j);
                                    break;
                                case "23":
                                    SS1.ActiveSheet.Cells[26, 5].Text += "장상피화생,";
                                    SS1.ActiveSheet.Cells[26, 5].Text += hp.DisPlay_S병변위치1(SS1, strTemp2, j);
                                    break;
                                case "03":
                                    SS1.ActiveSheet.Cells[26, 5].Text += "위암의심,";
                                    SS1.ActiveSheet.Cells[26, 5].Text += hp.DisPlay_S병변위치1(SS1, strTemp2, j);
                                    break;
                                case "04":
                                    SS1.ActiveSheet.Cells[26, 5].Text += "조기위암,";
                                    SS1.ActiveSheet.Cells[26, 5].Text += hp.DisPlay_S병변위치1(SS1, strTemp2, j);
                                    break;
                                case "05":
                                    SS1.ActiveSheet.Cells[26, 5].Text += "진행위암,";
                                    SS1.ActiveSheet.Cells[26, 5].Text += hp.DisPlay_S병변위치1(SS1, strTemp2, j);
                                    break;
                                case "06":
                                    SS1.ActiveSheet.Cells[26, 5].Text += "양성 위궤양,";
                                    SS1.ActiveSheet.Cells[26, 5].Text += hp.DisPlay_S병변위치1(SS1, strTemp2, j);
                                    break;
                                case "07":
                                    SS1.ActiveSheet.Cells[26, 5].Text += "위용종 및 선종,";
                                    SS1.ActiveSheet.Cells[26, 5].Text += hp.DisPlay_S병변위치1(SS1, strTemp2, j);
                                    break;
                                case "71":
                                    SS1.ActiveSheet.Cells[26, 5].Text += "위용종,";
                                    SS1.ActiveSheet.Cells[26, 5].Text += hp.DisPlay_S병변위치1(SS1, strTemp2, j);
                                    break;
                                case "72":
                                    SS1.ActiveSheet.Cells[26, 5].Text += "선종,";
                                    SS1.ActiveSheet.Cells[26, 5].Text += hp.DisPlay_S병변위치1(SS1, strTemp2, j);
                                    break;
                                case "08":
                                    SS1.ActiveSheet.Cells[26, 5].Text += "위 점막하종양,";
                                    SS1.ActiveSheet.Cells[26, 5].Text += hp.DisPlay_S병변위치1(SS1, strTemp2, j);
                                    break;

                                case "09":   //내시경 병형 기타
                                    for (int k = 1; k <= VB.Len(item.STOMACH_BENDO); k++)
                                    {
                                        if (VB.Mid(item.STOMACH_BENDO, k, 1) == "1")
                                        {
                                            switch (k)
                                            {
                                                case 1: SS1.ActiveSheet.Cells[26, 5].Text += "식도/위정맥류,"; break;
                                                case 2: SS1.ActiveSheet.Cells[26, 5].Text += "식도염,"; break;
                                                case 3: SS1.ActiveSheet.Cells[26, 5].Text += "식도 점막하종양,"; break;
                                                case 4: SS1.ActiveSheet.Cells[26, 5].Text += "식도암,"; break;
                                                case 5: SS1.ActiveSheet.Cells[26, 5].Text += "십이지장궤양,"; break;
                                                case 6: SS1.ActiveSheet.Cells[26, 5].Text += "십이지장악성종양,"; break;
                                                case 7: SS1.ActiveSheet.Cells[26, 5].Text += "십이지장점막하종양,"; break;
                                                case 8:
                                                    if (strTemp3.IsNullOrEmpty())
                                                    {
                                                        SS1.ActiveSheet.Cells[26, 5].Text += "기타소견";
                                                    }
                                                    else
                                                    {
                                                        SS1.ActiveSheet.Cells[26, 5].Text += strTemp3;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    SS1.ActiveSheet.Cells[26, 5].Text += hp.DisPlay_S병변위치1(SS1, strTemp2, j);
                                    break;
                                default: break;
                            }
                        }
                    }
                }
                else
                {
                    SS1.ActiveSheet.Cells[26, 2].Text = "위내시경검사" + ComNum.VBLF;
                    SS1.ActiveSheet.Cells[26, 2].Text += "□내원 □출장";
                    SS1.ActiveSheet.Cells[26, 5].Text = "해당사항 없음";
                }
                nlen = 0; strTemp1 = ""; strTemp2 = ""; strTemp3 = ""; strTemp4 = "";

                //조직검사실시여부
                SS1.ActiveSheet.Cells[27, 5].Text = "";
                strTemp1 = item.NEW_SICK67.Trim(); //위조직진단기타
                strTemp2 = item.NEW_SICK63.Trim(); //위조직암종류
                strTemp3 = item.NEW_SICK66.Trim(); //위조직암기타
                strTemp4 = item.NEW_SICK68.Trim(); //위조직암기타-기타

                if (item.S_ANATGBN != "2" || !item.RESULT0001.IsNullOrEmpty())
                {
                    //SS1.ActiveSheet.Cells[27, 5].Text =
                    switch (item.RESULT0010)
                    {
                        case "01": SS1.ActiveSheet.Cells[27, 5].Text = "이상소견없음"; break;
                        case "21": SS1.ActiveSheet.Cells[27, 5].Text = "위 염"; break;
                        case "22": SS1.ActiveSheet.Cells[27, 5].Text = "위축성위염"; break;
                        case "23": SS1.ActiveSheet.Cells[27, 5].Text = "장상피화생"; break;
                        case "03": SS1.ActiveSheet.Cells[27, 5].Text = "염증성 또는 증식성 병변"; break;
                        case "04": SS1.ActiveSheet.Cells[27, 5].Text = "저도선종 또는 이형성"; break;
                        case "05": SS1.ActiveSheet.Cells[27, 5].Text = "고도선종 또는 이형성"; break;
                        case "06": SS1.ActiveSheet.Cells[27, 5].Text = "암의심,"; break;
                        case "07": SS1.ActiveSheet.Cells[27, 5].Text = "(" + hp.DisPlay_위조직암(SS1, strTemp2, strTemp3) + ")"; break;
                        case "08": SS1.ActiveSheet.Cells[27, 5].Text = "(" + hp.DisPlay_위조직암기타(SS1, strTemp1, strTemp4) + ")"; break;

                        default: break;
                    }
                }
                else
                {
                    SS1.ActiveSheet.Cells[27, 5].Text = "해당사항 없음";
                }
                nlen = 0; strTemp1 = ""; strTemp2 = ""; strTemp3 = ""; strTemp4 = "";

                //판정일자
                SS1.ActiveSheet.Cells[30, 3].Text = item.S_PANJENGDATE;
                if (item.NEW_WOMAN32 == "0")
                {
                    SS1.ActiveSheet.Cells[30, 7].Text = "";
                }
                else
                {
                    SS1.ActiveSheet.Cells[30, 7].Text = item.NEW_WOMAN32;
                }

                //위암 판정면허,판정의
                FnDrno[1] = item.NEW_WOMAN32.To<long>();
                SS1.ActiveSheet.Cells[30, 11].Text = hb.READ_License_DrName(FnDrno[1]);
                SS1.ActiveSheet.Cells[31, 3].Text = item.TONGBODATE;

                //위암 종합판정
                strList = "";
                switch (item.RESULT0010)
                {
                    case "1": SS1.ActiveSheet.Cells[27, 5].Text = "이상소견없음 또는 위염"; break;
                    case "2": SS1.ActiveSheet.Cells[27, 5].Text = "양성질환"; break;
                    case "3": SS1.ActiveSheet.Cells[27, 5].Text = "위암 의심"; break;
                    case "4": SS1.ActiveSheet.Cells[27, 5].Text = "위암"; break;
                    case "5": SS1.ActiveSheet.Cells[27, 5].Text = "기타(" + item.S_JILETC + ")"; break;
                    default: break;
                }

                //권고사항
                SS1.ActiveSheet.Cells[29, 2].Text = item.S_SOGEN;
                SS1.ActiveSheet.Cells[29, 2].Text += ComNum.VBLF + item.S_SOGEN2;
                if (VB.Left(item.NEW_SICK03, 1) == "2") { SS1.ActiveSheet.Cells[29, 2].Text += ComNum.VBLF + "기존 위암환자"; }

                //위암종료
            }

            if (item.GBRECTUM == "1")
            {
                //대장암시작
                FstrPrtGbn[2] = "Y";
                nlen = 0; strTemp1 = ""; strTemp2 = ""; strTemp3 = ""; strTemp4 = "";

                //분변잠혈방응검사
                SS2.ActiveSheet.Cells[25, 2].Text = "";
                strTemp1 = item.COLON_RESULT.Trim();
                if (!strTemp1.IsNullOrEmpty() && strTemp1 != "000")
                {
                    nTemp = 1;
                    SS2.ActiveSheet.Cells[25, 2].Text = "분변잠혈반응검사" + ComNum.VBLF + "(" + strGunDate + ")";
                    if (item.C_PLACE.Trim() == "2")
                    {
                        SS2.ActiveSheet.Cells[25, 2].Text += ComNum.VBLF + "■내원 □출장 ";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[25, 2].Text += ComNum.VBLF + "□내원 ■출장 ";
                    }

                    SS2.ActiveSheet.Cells[25, 5].Text = "";
                    switch (item.COLON_RESULT)
                    {
                        case "1": SS2.ActiveSheet.Cells[25, 5].Text = "음성"; break;
                        case "2": SS2.ActiveSheet.Cells[25, 5].Text = "양성"; break;
                        default: break;
                    }
                    SS2.ActiveSheet.Cells[25, 5].Text += " 검사결과: (" + VB.Format(chb.READ_MIR_RESULT3(fnWrtno, "TX26"), "##0.00") + " ng/ml)";
                    SS2.ActiveSheet.Cells[25, 5].Text += " [참고치: 100.00 ng / ml 이하]";

                }
                else
                {
                    SS2.ActiveSheet.Cells[25, 2].Text = "분변잠혈반응검사";
                    SS2.ActiveSheet.Cells[25, 5].Text = "해상사항 없음";
                }


                //대장이중조영검사
                nlen = 0; strTemp1 = ""; strTemp2 = ""; strTemp3 = ""; strTemp4 = "";

                SS2.ActiveSheet.Cells[26, 2].Text = "";
                strTemp1 = item.COLON_S.Trim();         //대장이중조영검사
                strTemp2 = item.COLON_P.Trim();         //대장이중-병변위치
                strTemp3 = item.NEW_SICK38.Trim();      //대장이중-용종크기

                if (!strTemp1.IsNullOrEmpty() && strTemp1 != "000")
                {
                    nTemp = 2;
                    SS2.ActiveSheet.Cells[26, 2].Text = "대장이중조영검사" + ComNum.VBLF + "(" + strGunDate + ")";
                    if (item.C_PLACE.Trim() == "2")
                    {
                        SS2.ActiveSheet.Cells[26, 2].Text += ComNum.VBLF + "■내원 □출장 ";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[26, 2].Text += ComNum.VBLF + "□내원 ■출장 ";
                    }

                    SS2.ActiveSheet.Cells[26, 5].Text = "";
                    nlen = VB.Len(strTemp1);
                    for (int j = 1; j <= nlen; j++)
                    {
                        if (!VB.Mid(strTemp1, j, 1).IsNullOrEmpty())
                        {
                            if (VB.Mid(strTemp1, j, 1) != "0")
                            {
                                SS2.ActiveSheet.Cells[26, 5].Text = "판독소견: ";

                                switch (VB.Mid(strTemp1, j, 1))
                                {
                                    case "1": SS2.ActiveSheet.Cells[25, 5].Text += "이상소견없음"; break;
                                    case "2":
                                        if (strTemp3.IsNullOrEmpty())
                                        {
                                            SS2.ActiveSheet.Cells[25, 5].Text += "대장용종,";
                                        }
                                        else
                                        {
                                            SS2.ActiveSheet.Cells[25, 5].Text += "대장용종 크기: " + strTemp3 + " cm";
                                        }
                                        break;
                                    case "3": SS2.ActiveSheet.Cells[25, 5].Text += "대장암의심,"; break;
                                    case "4": SS2.ActiveSheet.Cells[25, 5].Text += "대장암,"; break;
                                    case "5":
                                        for (int k = 1; k <= VB.Len(item.COLON_B); k++)
                                        {
                                            if (VB.Mid(item.COLON_B, k, 1) == "1")
                                                switch (k)
                                                {
                                                    case 1: SS2.ActiveSheet.Cells[25, 5].Text += "치핵,"; break;
                                                    case 2: SS2.ActiveSheet.Cells[25, 5].Text += "비특이성 장염,"; break;
                                                    case 3: SS2.ActiveSheet.Cells[25, 5].Text += "허혈성장염,"; break;
                                                    case 4: SS2.ActiveSheet.Cells[25, 5].Text += "궤양성대장염,"; break;
                                                    case 5: SS2.ActiveSheet.Cells[25, 5].Text += "크론병,"; break;
                                                    case 6: SS2.ActiveSheet.Cells[25, 5].Text += "장결핵,"; break;
                                                    case 7: SS2.ActiveSheet.Cells[25, 5].Text += "대장게실증,"; break;
                                                    case 8: SS2.ActiveSheet.Cells[25, 5].Text += "대장점막하종양,"; break;
                                                    case 9: SS2.ActiveSheet.Cells[25, 5].Text += "림프구증식,"; break;
                                                    case 10: SS2.ActiveSheet.Cells[25, 5].Text += item.C_JILETC.Trim(); break;
                                                }
                                        }
                                        break;
                                    default: break;
                                }
                                SS2.ActiveSheet.Cells[25, 5].Text += ComNum.VBLF;
                            }
                        }
                    }

                }
                else
                {
                    SS2.ActiveSheet.Cells[25, 2].Text = "대장이중조영검사";
                    SS2.ActiveSheet.Cells[25, 2].Text += ComNum.VBLF + "□내원" + ComNum.VBLF + "□출장";
                    SS2.ActiveSheet.Cells[25, 5].Text = "해상사항 없음";
                }

                SS2.ActiveSheet.Cells[27, 5].Text = hp.DisPlay_C병변위치(SS2, strTemp2);

                //대장내시경검사
                nlen = 0; strTemp1 = ""; strTemp2 = ""; strTemp3 = ""; strTemp4 = "";
                SS2.ActiveSheet.Cells[28, 2].Text = "";
                strTemp1 = item.COLON_SENDO.Trim();     //대장내시경 관찰소견
                strTemp2 = item.COLON_PENDO.Trim();     //대장내시경 병변위치
                strTemp3 = item.NEW_SICK33.Trim();      //대장내시경 용종크기
                strTemp4 = item.NEW_SICK34.Trim();      //내시경 용종절제 여부

                if (!strTemp1.IsNullOrEmpty() && strTemp1 != "000")
                {
                    nTemp = 3;
                    SS2.ActiveSheet.Cells[28, 2].Text = "대장내시경검사" + ComNum.VBLF + "(" + strGunDate + ")";
                    if (item.C_PLACE.Trim() == "2")
                    {
                        SS2.ActiveSheet.Cells[28, 2].Text += ComNum.VBLF + "■내원 □출장 ";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[28, 2].Text += ComNum.VBLF + "□내원 ■출장 ";
                    }

                    //대장내시경검사 - 관찰소견
                    SS2.ActiveSheet.Cells[28, 5].Text = "";
                    nlen = VB.Len(strTemp1);

                    for (int i = 1; i <= nlen; i++)
                    {
                        if (!VB.Mid(strTemp1, i, 1).IsNullOrEmpty())
                        {
                            if (VB.Mid(strTemp1, i, 1) != "0")
                            {
                                SS2.ActiveSheet.Cells[28, 5].Text = "판독소견: ";

                                switch (VB.Mid(strTemp1, i, 1))
                                {
                                    case "1": SS2.ActiveSheet.Cells[28, 5].Text += "이상소견없음"; break;
                                    case "2":
                                        if (strTemp3.IsNullOrEmpty())
                                        {
                                            SS2.ActiveSheet.Cells[28, 5].Text += "대장용종,";
                                        }
                                        else
                                        {
                                            SS2.ActiveSheet.Cells[28, 5].Text += "대장용종 크기:" + strTemp3 + "cm";
                                        }
                                        if (strTemp4 == "1")
                                        {
                                            SS2.ActiveSheet.Cells[28, 5].Text += "용종절제함";
                                        }
                                        break;
                                    case "3": SS2.ActiveSheet.Cells[28, 5].Text += "대장암의심,"; break;
                                    case "4": SS2.ActiveSheet.Cells[28, 5].Text += "대장암,"; break;
                                    case "5":
                                        for (int j = 1; j <= VB.Len(item.COLON_BENDO); j++)
                                        {
                                            if (VB.Mid(item.COLON_B, j, 1) == "1")
                                                switch (j)
                                                {
                                                    case 1: SS2.ActiveSheet.Cells[28, 5].Text += "치핵,"; break;
                                                    case 2: SS2.ActiveSheet.Cells[28, 5].Text += "비특이성 장염,"; break;
                                                    case 3: SS2.ActiveSheet.Cells[28, 5].Text += "허혈성장염,"; break;
                                                    case 4: SS2.ActiveSheet.Cells[28, 5].Text += "궤양성대장염,"; break;
                                                    case 5: SS2.ActiveSheet.Cells[28, 5].Text += "크론병,"; break;
                                                    case 6: SS2.ActiveSheet.Cells[28, 5].Text += "장결핵,"; break;
                                                    case 7: SS2.ActiveSheet.Cells[28, 5].Text += "대장게실증,"; break;
                                                    case 8: SS2.ActiveSheet.Cells[28, 5].Text += "대장점막하종양,"; break;
                                                    case 9: SS2.ActiveSheet.Cells[28, 5].Text += "림프구증식,"; break;
                                                    case 10: SS2.ActiveSheet.Cells[28, 5].Text += item.COLON_ENDOETC.Trim(); break;
                                                }
                                        }
                                        break;
                                    default: break;
                                }
                                SS2.ActiveSheet.Cells[28, 5].Text += ComNum.VBLF;

                            }
                            //병병위치(대장)
                            SS2.ActiveSheet.Cells[29, 5].Text = hp.DisPlay_C병변위치(SS2, strTemp2);
                        }
                    }
                }
                else
                {
                    SS2.ActiveSheet.Cells[28, 2].Text = "대장내시경검사";
                    SS2.ActiveSheet.Cells[28, 2].Text += ComNum.VBLF + "□내원 □출장";
                    SS2.ActiveSheet.Cells[28, 5].Text = "해당사항 없음";
                }

                //조직검사(대장)
                SS2.ActiveSheet.Cells[28, 5].Text = "";
                if (!item.NEW_SICK71.Trim().IsNullOrEmpty())
                {

                    switch (item.NEW_SICK71.Trim())
                    {
                        case "1": SS2.ActiveSheet.Cells[30, 5].Text = "이상소견없음"; break;
                        case "2": SS2.ActiveSheet.Cells[30, 5].Text = "염증성 또는 증식성 병변"; break;
                        case "3": SS2.ActiveSheet.Cells[30, 5].Text = "저도선종 또는 이형성,"; break;
                        case "4": SS2.ActiveSheet.Cells[30, 5].Text = "고도선종 또는 이형성,"; break;
                        case "5": SS2.ActiveSheet.Cells[30, 5].Text = "암의심,"; break;
                        case "6": SS2.ActiveSheet.Cells[30, 5].Text = "암,"; break;
                        case "7": SS2.ActiveSheet.Cells[30, 5].Text = "기타,"; break;
                        default: break;
                    }

                }
                else
                {
                    SS2.ActiveSheet.Cells[30, 5].Text = "해당사항 없음";
                }

                //대장조직진단의 암 결과시
                if (item.NEW_SICK71.Trim() == "6")
                {
                    for (int j = 1; j <= VB.Len(item.NEW_SICK69.Trim()); j++)
                    {
                        if (VB.Mid(item.NEW_SICK69, j, 1) == "1")
                        {
                            switch (j)
                            {
                                case 1: SS2.ActiveSheet.Cells[30, 5].Text += " 샘암종(고분화,중분화,저분화),"; break;
                                case 2: SS2.ActiveSheet.Cells[30, 5].Text += " 점액(샘)암종,"; break;
                                case 3: SS2.ActiveSheet.Cells[30, 5].Text += " 반지세포암종,"; break;
                                case 4: SS2.ActiveSheet.Cells[30, 5].Text += " 샘편평상피암종,"; break;
                                case 5: SS2.ActiveSheet.Cells[30, 5].Text += " 편평상피암종,"; break;
                                case 6: SS2.ActiveSheet.Cells[30, 5].Text += " 소세포암종,"; break;
                                case 7: SS2.ActiveSheet.Cells[30, 5].Text += " 수질암종,"; break;
                                case 8: SS2.ActiveSheet.Cells[30, 5].Text += " 미분화 암종,"; break;
                                case 9: SS2.ActiveSheet.Cells[30, 5].Text += " 악성림프종,"; break;
                                case 10: SS2.ActiveSheet.Cells[30, 5].Text += " 신경내분비종양(맹장과 직장의 1cm이하 종양제외),"; break;
                                case 11: SS2.ActiveSheet.Cells[30, 5].Text += " " + item.NEW_SICK72.Trim(); break;
                                default: break;
                            }
                        }
                    }
                }

                //대장조직진단의 기타결과
                if (item.NEW_SICK71.Trim() == "7")
                {
                    for (int j = 1; j <= VB.Len(item.NEW_SICK73.Trim()); j++)
                    {
                        if (VB.Mid(item.NEW_SICK73, j, 1) == "1")
                        {
                            switch (j)
                            {
                                case 1: SS2.ActiveSheet.Cells[30, 5].Text += " 신경내분비종양,"; break;
                                case 2: SS2.ActiveSheet.Cells[30, 5].Text += " 비상피성종양,"; break;
                                case 3: SS2.ActiveSheet.Cells[30, 5].Text += " 항문앙,"; break;
                                case 4: SS2.ActiveSheet.Cells[30, 5].Text += " 말단회장부위 암,"; break;
                                case 5: SS2.ActiveSheet.Cells[30, 5].Text += " " + item.NEW_SICK74.Trim(); break;
                                default: break;
                            }
                        }
                    }
                }

                //대장암 권고사항
                SS2.ActiveSheet.Cells[32, 2].Text = "";
                SS2.ActiveSheet.Cells[32, 2].Text = VB.RTrim(item.C_SOGEN.Trim() + ComNum.VBLF + item.C_SOGEN.Trim() + ComNum.VBLF + item.C_SOGEN.Trim());

                if (VB.Left(SS2.ActiveSheet.Cells[32, 2].Text, 1) == ComNum.VBLF)
                {
                    SS2.ActiveSheet.Cells[32, 2].Text = VB.RTrim(VB.Right(SS2.ActiveSheet.Cells[32, 2].Text, VB.Len(SS2.ActiveSheet.Cells[32, 2].Text) - 1));
                }
                if (VB.Left(SS2.ActiveSheet.Cells[32, 2].Text, 1) == ComNum.VBLF)
                {
                    SS2.ActiveSheet.Cells[32, 2].Text = VB.RTrim(VB.Right(SS2.ActiveSheet.Cells[32, 2].Text, VB.Len(SS2.ActiveSheet.Cells[32, 2].Text) - 1));
                }
                if (VB.Left(SS2.ActiveSheet.Cells[32, 2].Text, 1) == ComNum.VBLF)
                {
                    SS2.ActiveSheet.Cells[32, 2].Text = VB.RTrim(VB.Right(SS2.ActiveSheet.Cells[32, 2].Text, VB.Len(SS2.ActiveSheet.Cells[32, 2].Text) - 1));
                }

                if (VB.Left(item.NEW_SICK13, 1).Trim() == "2")
                {
                    SS2.ActiveSheet.Cells[32, 2].Text += ComNum.VBLF + "기존 대장암환자";
                }

                //대장암 판정면허,판정의
                SS2.ActiveSheet.Cells[33, 3].Text = item.C_PANJENGDATE.Trim();
                SS2.ActiveSheet.Cells[33, 7].Text = item.NEW_WOMAN33.Trim();
                if (SS2.ActiveSheet.Cells[33, 7].Text == "0")
                {
                    SS2.ActiveSheet.Cells[33, 7].Text = "";
                }
                FnDrno[2] = item.NEW_WOMAN33.To<long>();
                SS2.ActiveSheet.Cells[33, 11].Text = hb.READ_License_DrName(FnDrno[2]);
                SS2.ActiveSheet.Cells[34, 3].Text = item.TONGBODATE;

                //대장암 종합판정
                strList = "";

                SS2.ActiveSheet.Cells[25, 10].Text = "";
                if (nTemp == 2 || nTemp == 3)
                {
                    switch (item.C_PANJENG.Trim())
                    {
                        case "1": SS2.ActiveSheet.Cells[25, 10].Text = "이상소견없음"; break;
                        case "2": SS2.ActiveSheet.Cells[25, 10].Text = "대장용종"; break;
                        case "3": SS2.ActiveSheet.Cells[25, 10].Text = "대장암 의심"; break;
                        case "4": SS2.ActiveSheet.Cells[25, 10].Text = "대장암,"; break;
                        case "5": SS2.ActiveSheet.Cells[25, 10].Text = "기타질환(" + item.C_JILETC.Trim() + ")"; break;
                        default: break;
                    }
                }
                else if (nTemp == 1)
                {
                    switch (item.C_PANJENG.Trim())
                    {
                        case "6": SS2.ActiveSheet.Cells[25, 10].Text = "잠혈반응없음"; break;
                        case "7": SS2.ActiveSheet.Cells[25, 10].Text = "잠혈반응있음"; break;
                        default: break;
                    }
                }
            }
            //대장암종료


            //간암시작
            if (item.GBRECTUM == "1")
            {
                nTemp = 0;
                nlen = 0; strTemp1 = ""; strTemp2 = ""; strTemp3 = ""; strTemp4 = ""; strTemp5 = "";

                nlen = VB.Len(item.RESULT0004.Trim());
                nlen1 = VB.Len(item.RESULT0010.Trim());
                nlen2 = VB.Len(item.RESULT0005.Trim());
                nlen3 = VB.Len(item.RESULT0006.Trim());

                if (!item.RESULT0004.IsNullOrEmpty())
                {
                    if (item.RESULT0004.Trim() != "000")
                    {
                        strTemp1 = item.RESULT0004.Trim();
                        strTemp2 = item.RESULT0015.Trim();
                        strTemp3 = item.RESULT0005.Trim();
                        strTemp4 = item.RESULT0006.Trim();
                        strTemp5 = item.RESULT0010.Trim();
                        strResult7 = item.RESULT0007.Trim();
                        strResult8 = item.RESULT0008.Trim();
                        strResult9 = item.RESULT0009.Trim();
                    }
                }

                SS3.ActiveSheet.Cells[25, 2].Text = "";
                if (!strTemp1.IsNullOrEmpty())
                {
                    SS3.ActiveSheet.Cells[25, 2].Text = "간초음파검사" + ComNum.VBLF + "(" + strGunDate + ")";
                    if (item.C_PLACE.Trim() == "2")
                    {
                        SS3.ActiveSheet.Cells[25, 2].Text += ComNum.VBLF + "■내원 □출장 ";
                    }
                    else
                    {
                        SS3.ActiveSheet.Cells[25, 2].Text += ComNum.VBLF + "□내원 ■출장 ";
                    }

                    SS3.ActiveSheet.Cells[25, 5].Text = "";
                    if (Convert.ToInt32(strTemp1) < 1)
                    {
                        SS3.ActiveSheet.Cells[25, 5].Text = "해당사항 없음";
                    }
                    else
                    {
                        for (int i = 1; i <= nlen; i++)
                        {
                            if (VB.Mid(strTemp1, i, 1) == "1")
                            {

                                switch (i)
                                {
                                    case 1: SS3.ActiveSheet.Cells[25, 5].Text = "정상"; break;
                                    case 2:
                                        if (SS3.ActiveSheet.Cells[25, 5].Text.IsNullOrEmpty())
                                        {
                                            SS3.ActiveSheet.Cells[25, 5].Text = "지방간";
                                        }
                                        else
                                        {
                                            SS3.ActiveSheet.Cells[25, 5].Text += ", " + "지방간";
                                        }
                                        break;
                                    case 3:
                                        if (SS3.ActiveSheet.Cells[25, 5].Text.IsNullOrEmpty())
                                        {
                                            SS3.ActiveSheet.Cells[25, 5].Text = "거친 에코";
                                        }
                                        else
                                        {
                                            SS3.ActiveSheet.Cells[25, 5].Text += ", " + "거친 에코";
                                        }
                                        break;
                                    case 4:
                                        if (SS3.ActiveSheet.Cells[25, 5].Text.IsNullOrEmpty())
                                        {
                                            SS3.ActiveSheet.Cells[25, 5].Text = "간경변";
                                        }
                                        else
                                        {
                                            SS3.ActiveSheet.Cells[25, 5].Text += ", " + "간경변";
                                        }
                                        break;
                                    default: break;
                                }
                            }
                        }
                    }

                    //간낭종
                    if (VB.Left(strTemp2, 1) == "10")
                    {
                        SS3.ActiveSheet.Cells[26, 5].Text = "간낭종";
                    }
                    else
                    {
                        SS3.ActiveSheet.Cells[26, 5].Text = "해당사항 없음";
                    }

                    //종괴
                    SS3.ActiveSheet.Cells[27, 5].Text = "";
                    if (strTemp3 == "00000000" && strTemp4 == "00000000")
                    {
                        SS3.ActiveSheet.Cells[27, 5].Text = "해당사항 없음";
                    }
                    else
                    {

                        if (strTemp3 != "00000000")
                        {
                            SS3.ActiveSheet.Cells[27, 5].Text = "1cm미만종괴 병변위치" + ComNum.VBLF;
                            for (int i = 1; i <= nlen2; i++)
                            {
                                if (VB.Mid(strTemp3, i, 1) == "1")
                                {
                                    switch (i)
                                    {
                                        case 1: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅰ,"; break;
                                        case 2: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅱ,"; break;
                                        case 3: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅲ,"; break;
                                        case 4: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅳ,"; break;
                                        case 5: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅴ,"; break;
                                        case 6: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅵ,"; break;
                                        case 7: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅶ,"; break;
                                        case 8: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅷ"; break;
                                        default: break;
                                    }
                                }
                                SS2.ActiveSheet.Cells[27, 5].Text += ComNum.VBLF;
                            }
                        }

                        if (strTemp4 != "00000000")
                        {
                            SS3.ActiveSheet.Cells[27, 5].Text = "1cm이상종괴 병변위치" + ComNum.VBLF;
                            for (int i = 1; i <= nlen3; i++)
                            {
                                if (VB.Mid(strTemp4, i, 1) == "1")
                                {
                                    switch (i)
                                    {
                                        case 1: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅰ,"; break;
                                        case 2: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅱ,"; break;
                                        case 3: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅲ,"; break;
                                        case 4: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅳ,"; break;
                                        case 5: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅴ,"; break;
                                        case 6: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅵ,"; break;
                                        case 7: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅶ,"; break;
                                        case 8: SS2.ActiveSheet.Cells[27, 5].Text += "Ⅷ"; break;
                                        default: break;
                                    }
                                }
                                SS2.ActiveSheet.Cells[27, 5].Text += ComNum.VBLF;
                            }
                        }

                        if (Convert.ToInt32(strTemp4) > 0)
                        {
                            if (!strResult7.IsNullOrEmpty())
                            {
                                SS2.ActiveSheet.Cells[27, 5].Text = "병변크기: " + strResult7 + "cm";
                            }
                            if (!strResult7.IsNullOrEmpty() && !strResult8.IsNullOrEmpty())
                            {
                                SS2.ActiveSheet.Cells[27, 5].Text = ", " + strResult8 + "cm";
                            }
                            if (!strResult7.IsNullOrEmpty() && !strResult8.IsNullOrEmpty() && !strResult9.IsNullOrEmpty())
                            {
                                SS2.ActiveSheet.Cells[27, 5].Text = ",: " + strResult9 + "cm";
                            }
                        }

                        SS2.ActiveSheet.Cells[28, 5].Text = "";
                        if (Convert.ToInt32(strTemp5) < 1)
                        {
                            SS2.ActiveSheet.Cells[28, 5].Text = "해당사항 없음";
                        }
                        else
                        {
                            for (int i = 1; i <= nlen1; i++)
                            {
                                if (VB.Mid(strTemp5, i, 1) == "1")
                                {
                                    switch (i)
                                    {
                                        case 1: SS2.ActiveSheet.Cells[28, 5].Text += " 담관확장" + ComNum.VBLF; break;
                                        case 2: SS2.ActiveSheet.Cells[28, 5].Text += " 간내담관결석" + ComNum.VBLF; break;
                                        case 3: SS2.ActiveSheet.Cells[28, 5].Text += " 복수" + ComNum.VBLF; break;
                                        case 4: SS2.ActiveSheet.Cells[28, 5].Text += " 비장종대" + ComNum.VBLF; break;
                                        case 5: SS2.ActiveSheet.Cells[28, 5].Text += " 간문맥 혹은 간정맥 혈전" + ComNum.VBLF; break;
                                        case 6: SS2.ActiveSheet.Cells[28, 5].Text += " 담낭이상(담석)" + ComNum.VBLF; break;
                                        case 7: SS2.ActiveSheet.Cells[28, 5].Text += " 담낭이상(담낭용종)" + ComNum.VBLF; break;
                                        case 8: SS2.ActiveSheet.Cells[28, 5].Text += " 기타(" + item.RESULT0012 + ")"; break;
                                        default: break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    SS3.ActiveSheet.Cells[25, 2].Text = "간초음파검사";
                    SS3.ActiveSheet.Cells[25, 2].Text += ComNum.VBLF + "□내원□출장";
                    SS3.ActiveSheet.Cells[25, 5].Text = "해당사항 없음";
                }

                //혈청알파태아단백검사
                nlen = 0; strTemp1 = ""; strTemp2 = ""; strTemp3 = ""; strTemp4 = "";
                SS3.ActiveSheet.Cells[29, 2].Text = "";
                if (!item.LIVER_RPHA.IsNullOrEmpty() || !item.LIVER_EIA.IsNullOrEmpty())
                {

                    nTemp = 2;
                    SS3.ActiveSheet.Cells[29, 2].Text = "혈청알파태아단백검사" + ComNum.VBLF + "(" + strGunDate + ")";
                    if (item.C_PLACE.Trim() == "2")
                    {
                        SS3.ActiveSheet.Cells[29, 2].Text += ComNum.VBLF + "■내원 □출장 ";
                    }
                    else
                    {
                        SS3.ActiveSheet.Cells[29, 2].Text += ComNum.VBLF + "□내원 ■출장 ";
                    }


                    if (!item.LIVER_EIA.IsNullOrEmpty())
                    {
                        SS3.ActiveSheet.Cells[30, 5].Text = "검사결과 :" + item.LIVER_EIA.Trim() + " ng/ml";

                    }
                    else
                    {
                        SS3.ActiveSheet.Cells[30, 5].Text = "미실시";
                    }
                }
                else
                {
                    SS3.ActiveSheet.Cells[29, 2].Text = "혈청알파태아단백검사";
                    SS3.ActiveSheet.Cells[29, 2].Text += ComNum.VBLF + "□내원□출장";
                }

                //간암 소견 및 조치사항
                SS3.ActiveSheet.Cells[31, 2].Text = item.L_SOGEN.Trim();
                if (VB.Left(item.NEW_SICK23.Trim(), 1) == "2")
                {
                    SS3.ActiveSheet.Cells[31, 2].Text += ComNum.VBLF + "기존 간암환자";
                }

                //간암 판정면허, 판정의
                SS3.ActiveSheet.Cells[32, 3].Text = item.L_PANJENGDATE.Trim();
                SS3.ActiveSheet.Cells[32, 7].Text = item.NEW_WOMAN34.Trim();
                if (SS3.ActiveSheet.Cells[32, 7].Text == "0")
                {
                    SS3.ActiveSheet.Cells[32, 7].Text = "";
                }
                FnDrno[3] = item.NEW_WOMAN34.To<long>();
                SS3.ActiveSheet.Cells[32, 11].Text = hb.READ_License_DrName(FnDrno[3]);
                SS3.ActiveSheet.Cells[33, 3].Text = item.TONGBODATE;

                //간암종합판정
                strList = "";
                SS3.ActiveSheet.Cells[25, 10].Text = "";

                switch (item.LIVER_PANJENG.Trim())
                {
                    case "1": SS2.ActiveSheet.Cells[25, 10].Text = "간암 의심소견 없음"; break;
                    case "2": SS2.ActiveSheet.Cells[25, 10].Text = "추적검사 요망(3개월 이내)"; break;
                    case "3": SS2.ActiveSheet.Cells[25, 10].Text = "간암 의심(정밀검사 요망)"; break;
                    case "4": SS2.ActiveSheet.Cells[25, 10].Text = "기타질환" + item.LIVER_JILETC.Trim() + ")"; break;
                    default: break;
                }
                //간암종료
            }

            //유방암시작
            if (item.GBBREAST.Trim() == "1")
            {
                FstrPrtGbn[4] = "Y";
                nlen = 0; strTemp1 = ""; strTemp2 = ""; strTemp3 = ""; strTemp4 = "";

                nlen = VB.Len(item.BREAST_S.Trim());
                strTemp1 = item.BREAST_S.Trim();        //유방촬영 판독소견
                strTemp2 = item.BREAST_P.Trim();        //유방촬영 병변위치
                strTemp4 = item.NEW_WOMAN17.Trim();     //유방촬영 판독소견기타 - 기타

                SS4.ActiveSheet.Cells[25, 2].Text = "";
                if (!strTemp1.IsNullOrEmpty())
                {
                    SS4.ActiveSheet.Cells[25, 2].Text = "유방촬영" + ComNum.VBLF + "(" + strGunDate + ")";
                    if (item.C_PLACE.Trim() == "2")
                    {
                        SS4.ActiveSheet.Cells[25, 2].Text += ComNum.VBLF + "■내원 □출장 ";
                    }
                    else
                    {
                        SS4.ActiveSheet.Cells[25, 2].Text += ComNum.VBLF + "□내원 ■출장 ";
                    }

                    //촉진 및 유방단순 촬영(병형)
                    SS4.ActiveSheet.Cells[25, 5].Text = "판독소견: ";
                    for (int i = 1; i <= nlen; i += 2)
                    {
                        switch (VB.Mid(strTemp1, i, 2))
                        {
                            case "01": SS4.ActiveSheet.Cells[25, 5].Text = " 이상소견없음"; break;
                            case "02": SS4.ActiveSheet.Cells[25, 5].Text += "종괴,"; break;
                            case "03": SS4.ActiveSheet.Cells[25, 5].Text += "양성석회화,"; break;
                            case "04": SS4.ActiveSheet.Cells[25, 5].Text += "미세석회화,"; break;
                            case "05": SS4.ActiveSheet.Cells[25, 5].Text += "구조왜곡,"; break;
                            case "06": SS4.ActiveSheet.Cells[25, 5].Text += "비대칭,"; break;
                            case "07": SS4.ActiveSheet.Cells[25, 5].Text += "피부이상,"; break;
                            case "08": SS4.ActiveSheet.Cells[25, 5].Text += "임파선비후,"; break;
                            case "09": SS4.ActiveSheet.Cells[25, 5].Text += "판정곤란,"; break;
                            case "10": SS4.ActiveSheet.Cells[25, 5].Text += strTemp4; break;
                            default: break;
                        }
                    }

                    SS4.ActiveSheet.Cells[26, 5].Text = "판독소견(우)▷ " + ComNum.VBLF;
                    SS4.ActiveSheet.Cells[26, 5].Text = hp.DisPlay_유방병변위치(SS4, strTemp2, strTemp4);

                    //병변위치 기타(우)
                    if (!item.BREAST_ETC.Trim().IsNullOrEmpty())
                    {
                        SS4.ActiveSheet.Cells[26, 5].Text += item.BREAST_ETC.Trim() + ComNum.VBLF;
                    }

                    SS4.ActiveSheet.Cells[26, 5].Text += ComNum.VBLF;


                    SS4.ActiveSheet.Cells[26, 5].Text += "판독소견(좌)▷ " + ComNum.VBLF;
                    SS4.ActiveSheet.Cells[26, 5].Text = hp.DisPlay_유방병변위치(SS4, strTemp2, strTemp4);

                    //병변위치 기타(좌)
                    if (!item.B_ANATETC.Trim().IsNullOrEmpty())
                    {
                        SS4.ActiveSheet.Cells[26, 5].Text += item.B_ANATETC.Trim() + ComNum.VBLF;
                    }
                }
                else
                {
                    SS4.ActiveSheet.Cells[25, 2].Text = "유방촬영";
                    SS4.ActiveSheet.Cells[25, 2].Text += ComNum.VBLF + "□내원 □출장"
; SS4.ActiveSheet.Cells[25, 5].Text = "해당사항 없음";
                }

                //유방암 소견 및 조치사항
                SS4.ActiveSheet.Cells[28, 2].Text = item.B_SOGEN.Trim();
                if (VB.Left(item.NEW_SICK08, 1) == "2")
                {
                    SS4.ActiveSheet.Cells[28, 2].Text += ComNum.VBLF + "기존 유방암환자";
                }

                //유방암 판정면허, 판정의
                SS4.ActiveSheet.Cells[29, 3].Text = item.B_PANJENGDATE.Trim();
                SS4.ActiveSheet.Cells[29, 7].Text = item.NEW_WOMAN35.Trim();
                if (SS4.ActiveSheet.Cells[29, 7].Text == "0")
                {
                    SS4.ActiveSheet.Cells[29, 7].Text = "";
                }
                FnDrno[4] = item.NEW_WOMAN35.To<long>();
                SS4.ActiveSheet.Cells[29, 11].Text = hb.READ_License_DrName(FnDrno[4]);
                SS4.ActiveSheet.Cells[30, 3].Text = item.TONGBODATE;

                //유방암 종합판정
                SS4.ActiveSheet.Cells[25, 10].Text = "";
                switch (item.B_PANJENG.Trim())
                {
                    case "1": SS4.ActiveSheet.Cells[25, 10].Text = "이상소견없음"; break;
                    case "2": SS4.ActiveSheet.Cells[25, 10].Text = "양성질환"; break;
                    case "3": SS4.ActiveSheet.Cells[25, 10].Text = "유방암 의심"; break;
                    case "4": SS4.ActiveSheet.Cells[25, 10].Text = "판정유보"; break;
                    default: break;
                }
                //유방암종료
            }

            if (item.GBWOMB.Trim() == "1")
            {
                //자궁경부암시작

                FstrPrtGbn[5] = "Y";
                strTemp = "0";
                nlen = 0; strTemp1 = ""; strTemp2 = ""; strTemp3 = ""; strTemp4 = "";

                //자궁경부암결과
                SS5.ActiveSheet.Cells[25, 2].Text = "";
                strTemp1 = item.WOMB03.Trim();          //결과소견
                strTemp2 = item.WOMB04.Trim();          //상피세포이상 경우
                strTemp3 = item.WOMB05.Trim();          //선상피세포이상
                strTemp4 = item.WOMB07.Trim();          //추가소견

                if (!strTemp1.IsNullOrEmpty())
                {
                    SS5.ActiveSheet.Cells[25, 2].Text = "자궁경부" + ComNum.VBLF + "세포검사" + ComNum.VBLF + "(" + strGunDate + ")";
                    if (item.C_PLACE.Trim() == "2")
                    {
                        SS5.ActiveSheet.Cells[25, 2].Text += ComNum.VBLF + "■내원 □출장 ";
                    }
                    else
                    {
                        SS5.ActiveSheet.Cells[25, 2].Text += ComNum.VBLF + "□내원 ■출장 ";
                    }

                    //결과소견
                    SS5.ActiveSheet.Cells[25, 5].Text = "판독소견: ";
                    switch (strTemp1)
                    {
                        case "1": SS5.ActiveSheet.Cells[25, 5].Text += "음성"; break;
                        case "2": SS5.ActiveSheet.Cells[25, 5].Text += "상피세포 이상"; break;
                        case "3": SS5.ActiveSheet.Cells[25, 5].Text += item.WOMB11.Trim(); break;
                        default: break;
                    }

                    //상피세포 이상일 경우
                    if (strTemp1 == "2")
                    {
                        if (!strTemp2.IsNullOrEmpty())
                        {
                            SS5.ActiveSheet.Cells[25, 5].Text += " 편평상피세포 이상-";
                            switch (item.WOMB04.Trim())
                            {
                                case "1": SS5.ActiveSheet.Cells[25, 5].Text += "음성비정형편평상피세포"; break;
                                case "2": SS5.ActiveSheet.Cells[25, 5].Text += "저등급 편평상피내 병변"; break;
                                case "3": SS5.ActiveSheet.Cells[25, 5].Text += "고등급 편평상피내 병변"; break;
                                case "4": SS5.ActiveSheet.Cells[25, 5].Text += "침윤성 편평세포암종"; break;
                                default: break;
                            }
                            SS5.ActiveSheet.Cells[25, 5].Text += ComNum.VBLF;
                        }

                        if (!strTemp3.IsNullOrEmpty())
                        {
                            SS5.ActiveSheet.Cells[25, 5].Text += " 선상피세포 이상-";
                            switch (item.WOMB05.Trim())
                            {
                                case "1": SS5.ActiveSheet.Cells[25, 5].Text += "비정형 선상피세포"; break;
                                case "2": SS5.ActiveSheet.Cells[25, 5].Text += "상피내 선암종"; break;
                                case "3": SS5.ActiveSheet.Cells[25, 5].Text += "침윤성 선암종"; break;
                                case "4": SS5.ActiveSheet.Cells[25, 5].Text += item.WOMB06.Trim(); break;
                                default: break;
                            }
                        }
                    }

                    //결과 및 추가소견
                    SS5.ActiveSheet.Cells[26, 5].Text = "";
                    if (strTemp4 == "6")
                    {
                        SS5.ActiveSheet.Cells[26, 5].Text = item.WOMB08.Trim();
                    }
                    else
                    {
                        switch (strTemp4)
                        {
                            case "1": SS5.ActiveSheet.Cells[26, 5].Text = "반응성세포변화"; break;
                            case "2": SS5.ActiveSheet.Cells[26, 5].Text = "트리코모나스"; break;
                            case "3": SS5.ActiveSheet.Cells[26, 5].Text = "캔디다"; break;
                            case "4": SS5.ActiveSheet.Cells[26, 5].Text = "방선균"; break;
                            case "5": SS5.ActiveSheet.Cells[26, 5].Text = "헤르페스바이러스"; break;
                            default: break;
                        }
                    }
                }
                else
                {
                    SS5.ActiveSheet.Cells[25, 2].Text = "자궁경부" + ComNum.VBLF + "세포검사";
                    SS5.ActiveSheet.Cells[25, 2].Text += ComNum.VBLF + "□내원□출장";
                }

                //자궁경부암 소견 및 조치사항
                SS5.ActiveSheet.Cells[28, 2].Text = item.W_SOGEN.Trim();
                if (VB.Left(item.NEW_SICK18.Trim(), 1) == "2")
                {
                    SS5.ActiveSheet.Cells[28, 2].Text += ComNum.VBLF + "기존 자궁경부암환자";
                }

                //자궁경부암 판정면허,판정의
                SS5.ActiveSheet.Cells[29, 3].Text = item.W_PANJENGDATE.Trim();
                SS5.ActiveSheet.Cells[29, 7].Text = item.NEW_WOMAN36.Trim();
                if (SS5.ActiveSheet.Cells[29, 7].Text == "0")
                {
                    SS5.ActiveSheet.Cells[29, 7].Text = "";
                }
                FnDrno[5] = item.NEW_WOMAN36.To<long>();
                SS5.ActiveSheet.Cells[29, 11].Text = hb.READ_License_DrName(FnDrno[5]);
                SS5.ActiveSheet.Cells[30, 3].Text = item.TONGBODATE;

                //자궁경부암 자동판정
                switch (item.WOMB09.Trim())
                {
                    case "1": SS5.ActiveSheet.Cells[25, 10].Text = "이상소견없음"; break;
                    case "2": SS5.ActiveSheet.Cells[25, 10].Text = "반응성 소견 및 감염성질환"; break;
                    case "3": SS5.ActiveSheet.Cells[25, 10].Text = "비정형 상피세포 이상"; break;
                    case "4": SS5.ActiveSheet.Cells[25, 10].Text = "자궁경부암 전구단계 의심"; break;
                    case "5": SS5.ActiveSheet.Cells[25, 10].Text = "자궁경부암의심"; break;
                    case "6": SS5.ActiveSheet.Cells[25, 10].Text = "기타질환(" + item.WOMB11 + ")"; break;
                    default: break;
                }
                //자궁경부암종료
            }


            if (item.GBLUNG == "1")
            {
                //폐암시작
                FstrPrtGbn[6] = "Y";

                strMent = "          ※폐암 검진결과 받으신 15일 이내로 건강증진센터로 방문하셔서";
                strMent1 = "          검진결과 상담 및 금연상담을 받시기 바랍니다.   ☞ 방문전 전화예약 ☎260-8188";

                strBeomJu[1] = item.LUNG_RESULT072.Trim();
                strBeomJu[2] = item.LUNG_RESULT073.Trim();
                strBeomJu[3] = item.LUNG_RESULT074.Trim();
                strBeomJu[4] = item.LUNG_RESULT075.Trim();
                strBeomJu[5] = item.LUNG_RESULT076.Trim();
                strBeomJu[6] = item.LUNG_RESULT077.Trim();

                strOK = "";
                if ((strBeomJu[1].IsNullOrEmpty() || strBeomJu[1] == "1") && (strBeomJu[2].IsNullOrEmpty() || strBeomJu[2] == "1") &&
                   (strBeomJu[3].IsNullOrEmpty() || strBeomJu[3] == "1") && (strBeomJu[4].IsNullOrEmpty() || strBeomJu[4] == "1") &&
                   (strBeomJu[5].IsNullOrEmpty() || strBeomJu[5] == "1") && (strBeomJu[6].IsNullOrEmpty() || strBeomJu[6] == "1"))
                {
                    strOK = "";
                }
                else
                {
                    if (strOK == "")
                    {
                        for (int i = 1; i <= 6; i++)
                        {
                            if (strBeomJu[i] == "4X")
                            {
                                strOK = i.ToString();
                                break;
                            }
                        }
                    }

                    if (strOK == "")
                    {
                        for (int i = 1; i <= 6; i++)
                        {
                            if (strBeomJu[i] == "4B")
                            {
                                strOK = i.ToString();
                                break;
                            }
                        }
                    }

                    if (strOK == "")
                    {
                        for (int i = 1; i <= 6; i++)
                        {
                            if (strBeomJu[i] == "4A")
                            {
                                strOK = i.ToString();
                                break;
                            }
                        }
                    }

                    if (strOK == "")
                    {
                        for (int i = 1; i <= 6; i++)
                        {
                            if (strBeomJu[i] == "3")
                            {
                                strOK = i.ToString();
                                break;
                            }
                        }
                    }

                    if (strOK == "")
                    {
                        for (int i = 1; i <= 6; i++)
                        {
                            if (strBeomJu[i] == "2B")
                            {
                                strOK = i.ToString();
                                break;
                            }
                        }
                    }

                    if (strOK == "")
                    {
                        for (int i = 1; i <= 6; i++)
                        {
                            if (strBeomJu[i] == "2")
                            {
                                strOK = i.ToString();
                                break;
                            }
                        }
                    }

                    if (strOK == "")
                    {
                        for (int i = 1; i <= 6; i++)
                        {
                            if (strBeomJu[i] == "1")
                            {
                                strOK = i.ToString();
                                break;
                            }
                        }
                    }
                }

                //1번째 폐결절 소견
                if (strOK == "0")
                {

                    strTEXT = item.LUNG_RESULT005.Trim();
                    switch (item.LUNG_RESULT007.Trim())
                    {
                        case "1": strTEXT1 = "우상엽"; break;
                        case "2": strTEXT1 = "우중엽"; break;
                        case "3": strTEXT1 = "우하엽"; break;
                        case "4": strTEXT1 = "좌상엽"; break;
                        case "5": strTEXT1 = "좌하엽"; break;
                        default: break;
                    }

                    switch (item.LUNG_RESULT006.Trim())
                    {
                        case "1": strTEXT2 = "*결절 성상 : 고형"; break;
                        case "2": strTEXT2 = "*결절 성상 : 부분고형"; break;
                        case "3": strTEXT2 = "*결절 성상 : 간유리(비고형)"; break;
                        default: break;
                    }

                    if (item.LUNG_RESULT006.Trim() == "1" || item.LUNG_RESULT006.Trim() == "3")
                    {
                        strTEXT3 = "*결절 크기 : " + item.LUNG_RESULT008.Trim() + "mm";
                    }
                    else if (item.LUNG_RESULT006.Trim() == "2")
                    {
                        strTEXT3 = "*결절 크기 : " + item.LUNG_RESULT008.Trim() + "mm" + "(" + item.LUNG_RESULT009.Trim() + "mm)";
                    }

                    switch (item.LUNG_RESULT010.Trim())
                    {
                        case "1": strTEXT4 = "*결절 특징 : 폐암 시사소견"; break;
                        case "2": strTEXT4 = "*결절 특징 : 양성결절 시사소견(2B)"; break;
                        case "3": strTEXT4 = "*결절 특징 : 해당없음"; break;
                        default: break;
                    }
                }
                //2번째 폐결절 소견
                else if (strOK == "1")
                {
                    strTEXT = item.LUNG_RESULT013.Trim();
                    switch (item.LUNG_RESULT015.Trim())
                    {
                        case "1": strTEXT1 = "우상엽"; break;
                        case "2": strTEXT1 = "우중엽"; break;
                        case "3": strTEXT1 = "우하엽"; break;
                        case "4": strTEXT1 = "좌상엽"; break;
                        case "5": strTEXT1 = "좌하엽"; break;
                        default: break;
                    }

                    switch (item.LUNG_RESULT014.Trim())
                    {
                        case "1": strTEXT2 = "*결절 성상 : 고형"; break;
                        case "2": strTEXT2 = "*결절 성상 : 부분고형"; break;
                        case "3": strTEXT2 = "*결절 성상 : 간유리(비고형)"; break;
                        default: break;
                    }

                    if (item.LUNG_RESULT014.Trim() == "1" || item.LUNG_RESULT014.Trim() == "3")
                    {
                        strTEXT3 = "*결절 크기 : " + item.LUNG_RESULT016.Trim() + "mm";
                    }
                    else if (item.LUNG_RESULT014.Trim() == "2")
                    {
                        strTEXT3 = "*결절 크기 : " + item.LUNG_RESULT016.Trim() + "mm" + "(" + item.LUNG_RESULT017.Trim() + "mm)";
                    }

                    switch (item.LUNG_RESULT018.Trim())
                    {
                        case "1": strTEXT4 = "*결절 특징 : 폐암 시사소견"; break;
                        case "2": strTEXT4 = "*결절 특징 : 양성결절 시사소견(2B)"; break;
                        case "3": strTEXT4 = "*결절 특징 : 해당없음"; break;
                        default: break;
                    }
                }
                //3번째 폐결절 소견
                else if (strOK == "2")
                {
                    strTEXT = item.LUNG_RESULT021.Trim();
                    switch (item.LUNG_RESULT023.Trim())
                    {
                        case "1": strTEXT1 = "우상엽"; break;
                        case "2": strTEXT1 = "우중엽"; break;
                        case "3": strTEXT1 = "우하엽"; break;
                        case "4": strTEXT1 = "좌상엽"; break;
                        case "5": strTEXT1 = "좌하엽"; break;
                        default: break;
                    }

                    switch (item.LUNG_RESULT022.Trim())
                    {
                        case "1": strTEXT2 = "*결절 성상 : 고형"; break;
                        case "2": strTEXT2 = "*결절 성상 : 부분고형"; break;
                        case "3": strTEXT2 = "*결절 성상 : 간유리(비고형)"; break;
                        default: break;
                    }

                    if (item.LUNG_RESULT022.Trim() == "1" || item.LUNG_RESULT022.Trim() == "3")
                    {
                        strTEXT3 = "*결절 크기 : " + item.LUNG_RESULT024.Trim() + "mm";
                    }
                    else if (item.LUNG_RESULT022.Trim() == "2")
                    {
                        strTEXT3 = "*결절 크기 : " + item.LUNG_RESULT024.Trim() + "mm" + "(" + item.LUNG_RESULT025.Trim() + "mm)";
                    }

                    switch (item.LUNG_RESULT026.Trim())
                    {
                        case "1": strTEXT4 = "*결절 특징 : 폐암 시사소견"; break;
                        case "2": strTEXT4 = "*결절 특징 : 양성결절 시사소견(2B)"; break;
                        case "3": strTEXT4 = "*결절 특징 : 해당없음"; break;
                        default: break;
                    }
                }
                //4번째 폐결절 소견
                else if (strOK == "3")
                {
                    strTEXT = item.LUNG_RESULT029.Trim();
                    switch (item.LUNG_RESULT031.Trim())
                    {
                        case "1": strTEXT1 = "우상엽"; break;
                        case "2": strTEXT1 = "우중엽"; break;
                        case "3": strTEXT1 = "우하엽"; break;
                        case "4": strTEXT1 = "좌상엽"; break;
                        case "5": strTEXT1 = "좌하엽"; break;
                        default: break;
                    }

                    switch (item.LUNG_RESULT030.Trim())
                    {
                        case "1": strTEXT2 = "*결절 성상 : 고형"; break;
                        case "2": strTEXT2 = "*결절 성상 : 부분고형"; break;
                        case "3": strTEXT2 = "*결절 성상 : 간유리(비고형)"; break;
                        default: break;
                    }

                    if (item.LUNG_RESULT030.Trim() == "1" || item.LUNG_RESULT030.Trim() == "3")
                    {
                        strTEXT3 = "*결절 크기 : " + item.LUNG_RESULT032.Trim() + "mm";
                    }
                    else if (item.LUNG_RESULT030.Trim() == "2")
                    {
                        strTEXT3 = "*결절 크기 : " + item.LUNG_RESULT032.Trim() + "mm" + "(" + item.LUNG_RESULT033.Trim() + "mm)";
                    }

                    switch (item.LUNG_RESULT034.Trim())
                    {
                        case "1": strTEXT4 = "*결절 특징 : 폐암 시사소견"; break;
                        case "2": strTEXT4 = "*결절 특징 : 양성결절 시사소견(2B)"; break;
                        case "3": strTEXT4 = "*결절 특징 : 해당없음"; break;
                        default: break;
                    }
                }
                //5번째 폐결절 소견
                else if (strOK == "4")
                {
                    strTEXT = item.LUNG_RESULT037.Trim();
                    switch (item.LUNG_RESULT039.Trim())
                    {
                        case "1": strTEXT1 = "우상엽"; break;
                        case "2": strTEXT1 = "우중엽"; break;
                        case "3": strTEXT1 = "우하엽"; break;
                        case "4": strTEXT1 = "좌상엽"; break;
                        case "5": strTEXT1 = "좌하엽"; break;
                        default: break;
                    }

                    switch (item.LUNG_RESULT038.Trim())
                    {
                        case "1": strTEXT2 = "*결절 성상 : 고형"; break;
                        case "2": strTEXT2 = "*결절 성상 : 부분고형"; break;
                        case "3": strTEXT2 = "*결절 성상 : 간유리(비고형)"; break;
                        default: break;
                    }

                    if (item.LUNG_RESULT038.Trim() == "1" || item.LUNG_RESULT038.Trim() == "3")
                    {
                        strTEXT3 = "*결절 크기 : " + item.LUNG_RESULT040.Trim() + "mm";
                    }
                    else if (item.LUNG_RESULT038.Trim() == "2")
                    {
                        strTEXT3 = "*결절 크기 : " + item.LUNG_RESULT040.Trim() + "mm" + "(" + item.LUNG_RESULT041.Trim() + "mm)";
                    }

                    switch (item.LUNG_RESULT042.Trim())
                    {
                        case "1": strTEXT4 = "*결절 특징 : 폐암 시사소견"; break;
                        case "2": strTEXT4 = "*결절 특징 : 양성결절 시사소견(2B)"; break;
                        case "3": strTEXT4 = "*결절 특징 : 해당없음"; break;
                        default: break;
                    }
                }
                //6번째 폐결절 소견
                else if (strOK == "5")
                {
                    strTEXT = item.LUNG_RESULT045.Trim();
                    switch (item.LUNG_RESULT047.Trim())
                    {
                        case "1": strTEXT1 = "우상엽"; break;
                        case "2": strTEXT1 = "우중엽"; break;
                        case "3": strTEXT1 = "우하엽"; break;
                        case "4": strTEXT1 = "좌상엽"; break;
                        case "5": strTEXT1 = "좌하엽"; break;
                        default: break;
                    }

                    switch (item.LUNG_RESULT046.Trim())
                    {
                        case "1": strTEXT2 = "*결절 성상 : 고형"; break;
                        case "2": strTEXT2 = "*결절 성상 : 부분고형"; break;
                        case "3": strTEXT2 = "*결절 성상 : 간유리(비고형)"; break;
                        default: break;
                    }

                    if (item.LUNG_RESULT046.Trim() == "1" || item.LUNG_RESULT046.Trim() == "3")
                    {
                        strTEXT3 = "*결절 크기 : " + item.LUNG_RESULT048.Trim() + "mm";
                    }
                    else if (item.LUNG_RESULT046.Trim() == "2")
                    {
                        strTEXT3 = "*결절 크기 : " + item.LUNG_RESULT048.Trim() + "mm" + "(" + item.LUNG_RESULT049.Trim() + "mm)";
                    }

                    switch (item.LUNG_RESULT050.Trim())
                    {
                        case "1": strTEXT4 = "*결절 특징 : 폐암 시사소견"; break;
                        case "2": strTEXT4 = "*결절 특징 : 양성결절 시사소견(2B)"; break;
                        case "3": strTEXT4 = "*결절 특징 : 해당없음"; break;
                        default: break;
                    }
                }

                //결과
                if (item.LUNG_RESULT053.Trim() == "2")
                {
                    strTEXT5 = "*기관지 내 병변 : 없음";
                }
                else if (item.LUNG_RESULT053.Trim() == "1")
                {
                    strTEXT5 = "*기관지 내 병변 : 있음" + " (" + item.LUNG_RESULT054.Trim() + ")";
                }

                if (item.LUNG_RESULT067.Trim() == "1")
                {
                    strTEXT5 = "*비활동성 폐결핵 : 없음";
                }
                else if (item.LUNG_RESULT067.Trim() == "2")
                {
                    strTEXT5 = "*비활동성 폐결핵 : 있음";
                }

                SS6.ActiveSheet.Cells[26, 3].Text = "";
                if (strOK == "")
                {
                    SS6.ActiveSheet.Cells[26, 3].Text = "폐결절 : 없음";
                }
                else if (strOK == "")
                {
                    SS6.ActiveSheet.Cells[26, 3].Text = "폐결절 : 있음" + ComNum.VBLF + "(" + strTEXT1 + ")";
                }
                else if (strOK == "")
                {
                    SS6.ActiveSheet.Cells[26, 3].Text = "폐결절 :" + ComNum.VBLF + " 석회화 또는 지방 포함 결절";
                }

                //결과 입력
                SS6.ActiveSheet.Cells[25, 5].Text = "";
                if (strOK == "")
                {
                    SS6.ActiveSheet.Cells[25, 5].Text = strTEXT5 + ComNum.VBLF + strTEXT6;
                }
                else if (strOK != "" && strTEXT == "2")
                {
                    SS6.ActiveSheet.Cells[25, 5].Text = strTEXT3 + ComNum.VBLF + strTEXT4 + ComNum.VBLF + strTEXT5 + ComNum.VBLF + strTEXT6;
                }
                else if (strOK != "" && strTEXT == "3")
                {
                    SS6.ActiveSheet.Cells[25, 5].Text = strTEXT5 + ComNum.VBLF + strTEXT6;
                }

                //판정
                strLungResult = "";
                SS6.ActiveSheet.Cells[25, 10].Text = "";
                switch (item.LUNG_RESULT068.Trim())
                {
                    case "1": strLungResult = "*종합판정 : 이상소견없음"; break;
                    case "2": strLungResult = "*종합판정 : 양성결절"; break;
                    case "3": strLungResult = "*종합판정 : 경계선 결절"; break;
                    case "4": strLungResult = "*종합판정 : 폐암의심"; break;
                    case "5": strLungResult = "*종합판정 : 폐결절외 의미있는 소견" + "(" + item.LUNG_RESULT078.Trim() + ")"; break;
                    default: break;
                }

                switch (item.LUNG_RESULT055.Trim())
                {
                    case "1": strLungResult += ComNum.VBLF + "*폐결절 외 폐암시사소견 : 해당없음"; break;
                    case "2": strLungResult += ComNum.VBLF + "*폐결절 외 폐암시사소견 : 폐경화"; break;
                    case "3": strLungResult += ComNum.VBLF + "* 폐결절 외 폐암시사소견 : 무기폐"; break;
                    case "4": strLungResult += ComNum.VBLF + "*폐결절 외 폐암시사소견 : 림프절비대"; break;
                    case "5": strLungResult += ComNum.VBLF + "*폐결절 외 폐암시사소견 : 기타" + "(" + item.LUNG_RESULT056.Trim() + ")"; break;
                    default: break;
                }

                if (item.LUNG_RESULT057.Trim() == "1")
                {
                    strLungResult1 = "*폐결절 외 의미있는 소견 : 없음";
                }
                else
                {
                    if (item.LUNG_RESULT058.Trim() == "1") { strLungResult1 = "관상동맥석회화 (중등도이상),"; }
                    if (item.LUNG_RESULT059.Trim() == "1") { strLungResult1 += "폐기종 (중증도 이상),"; }
                    if (item.LUNG_RESULT060.Trim() == "1") { strLungResult1 += "간질성 폐이상,"; }
                    if (item.LUNG_RESULT061.Trim() == "1") { strLungResult1 += "폐렴 및 활동성 폐결핵,"; }
                    if (item.LUNG_RESULT062.Trim() == "1") { strLungResult1 += "폐외악성물,"; }
                    if (item.LUNG_RESULT063.Trim() == "1") { strLungResult1 += "대동맥류 (≥5.5 cm),"; }
                    if (item.LUNG_RESULT064.Trim() == "1") { strLungResult1 += "다량의 흉수 또는 심낭 삼출,"; }
                    if (item.LUNG_RESULT065.Trim() == "1") { strLungResult1 += "기타: (" + item.LUNG_RESULT056.Trim() + ")"; }
                    //마지막 컴마를 짜름

                    strLungResult1 = VB.Left(strLungResult1, VB.Len(strLungResult1) - 1);
                    strLungResult1 = "*폐결절 외 의미있는 소견 : " + strLungResult1;
                }

                //판정입력
                SS6.ActiveSheet.Cells[25, 10].Text = strLungResult + ComNum.VBLF + strLungResult1;

                //권고사항(폐암 판정구분에의한 권고사항, 폐암 판정 외 기타 권고사항)
                SS6.ActiveSheet.Cells[28, 2].Text = "";
                SS6.ActiveSheet.Cells[28, 2].Text = "● " + item.LUNG_RESULT070.Trim() + ComNum.VBLF + "● " + item.LUNG_RESULT071.Trim() + ComNum.VBLF + strMent + ComNum.VBLF + strMent1;

                //폐암 판정면허, 판정의
                SS6.ActiveSheet.Cells[29, 3].Text = item.L_PANJENGDATE1.Trim();
                SS6.ActiveSheet.Cells[29, 7].Text = item.NEW_WOMAN37.Trim();
                if (SS6.ActiveSheet.Cells[29, 7].Text == "0")
                {
                    SS6.ActiveSheet.Cells[29, 7].Text = "";
                }
                FnDrno[6] = item.NEW_WOMAN33.To<long>();
                SS6.ActiveSheet.Cells[29, 11].Text = hb.READ_License_DrName(FnDrno[6]);
                SS6.ActiveSheet.Cells[30, 3].Text = item.TONGBODATE;

                //폐암종료
            }

            if (!item.LUNG_SANGDAM3.Trim().IsNullOrEmpty())
            {
                //폐암사후상담시작
                FstrPrtGbn[7] = "Y";

                SS7.ActiveSheet.Cells[25, 2].Text = "";
                SS7.ActiveSheet.Cells[25, 2].Text = item.LUNG_SANGDAM1.Trim();

                SS7.ActiveSheet.Cells[28, 2].Text = "";
                SS7.ActiveSheet.Cells[28, 2].Text = item.LUNG_SANGDAM2.Trim();



                //폐암사후상
                SS7.ActiveSheet.Cells[29, 3].Text = item.LUNG_SANGDAM3.Trim();
                SS7.ActiveSheet.Cells[29, 7].Text = item.LUNG_SANGDAM4.Trim();
                if (SS7.ActiveSheet.Cells[29, 7].Text == "0")
                {
                    SS7.ActiveSheet.Cells[29, 7].Text = "";
                }
                FnDrno[7] = item.LUNG_SANGDAM3.To<long>();
                SS7.ActiveSheet.Cells[29, 11].Text = hb.READ_License_DrName(FnDrno[7]);
                SS7.ActiveSheet.Cells[30, 3].Text = item.TONGBODATE;
                //폐암사후상담종료
            }
        }

        private void Result_Print_Main()
        {
            int nPanRow = 0;
            string strJepdate = "";
            string strBangi = "";
            string strName = "";
            string strSS = "";

            //출력로직
            for (int i =1; i <= 7; i++)
            {
                if (FstrPrtGbn[i] == "Y")
                {
                    //SS7.ActiveSheet.Cells[9, 12].Text = FstrJepdate;
                    switch (i)
                    {
                        case 1:
                            nPanRow = 31;
                            SS1.ActiveSheet.Cells[9, 12].Text = FstrJepdate;
                            break;
                        case 2:
                            nPanRow = 34;
                            SS2.ActiveSheet.Cells[9, 12].Text = FstrJepdate;
                            break;
                        case 3:
                            nPanRow = 33;
                            SS3.ActiveSheet.Cells[9, 12].Text = FstrJepdate;
                            break;
                        case 4:
                            nPanRow = 30;
                            SS4.ActiveSheet.Cells[9, 12].Text = FstrJepdate;
                            break;
                        case 5:
                            nPanRow = 30;
                            SS5.ActiveSheet.Cells[9, 12].Text = FstrJepdate;
                            break;
                        case 6:
                            nPanRow = 30;
                            SS5.ActiveSheet.Cells[9, 12].Text = FstrJepdate;
                            break;
                        case 7:
                            nPanRow = 30;
                            SS6.ActiveSheet.Cells[9, 12].Text = FstrJepdate;
                            break;
                        default: break;
                    }

                    hf.SignImage_Spread_Set(SS1, nPanRow, 14, FnDrno[i].ToString(), "C","","");

                    //출력


                }
            }

            //폼종료
        }
    }

}
