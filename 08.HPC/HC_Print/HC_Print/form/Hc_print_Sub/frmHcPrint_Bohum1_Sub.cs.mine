using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
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
/// Class Name      : HC_Print
/// File Name       : frmHcPrint_Bohum1_Sub.cs
/// Description     : 공단검진 1차 결과지 인쇄
/// Author          : 김경동
/// Create Date     : 2020-12-08
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm공단검진1차결과지_2020.frm(FrmIDateChange)" />


namespace HC_Print
{
    public partial class frmHcPrint_Bohum1_Sub : Form
    {

        long fnWrtno = 0;
        long fnPano = 0;
        long fnAge = 0;
        long FnDrno = 0;

        string fstrSname = "";
        string fstrGjYear = "";
        string fstrChasu = "";
        string fstrJepdate = "";
        string fstrPtno = "";
        string fstrSex = "";
        string fstrGjjong = "";
        string fstrPandate = "";
        string fstrJumin = "";
        string fstrTongbo = "";

        bool FbChol = false;

        ComFunc cf = new ComFunc();
        clsHaBase hb = new clsHaBase();

        HicJepsuPatientService hicJepsuPatientService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicResBohum1JepsuPatientService hicResBohum1JepsuPatientService = null;
        HicResultSunapdtlService hicResultSunapdtlService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicResultService hicResultService = null;
        HicSangdamNewService hicSangdamNewService = null;
        HicSunapdtlGroupexamService hicSunapdtlGroupexamService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicResBohum1JepsuService hicResBohum1JepsuService = null;

        public frmHcPrint_Bohum1_Sub()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcPrint_Bohum1_Sub(long argWRTNO)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            fnWrtno = argWRTNO;
        }

        private void SetControl()
        {
            hicJepsuPatientService = new HicJepsuPatientService();
            hicSunapdtlService = new HicSunapdtlService   ();
            hicResBohum1JepsuPatientService = new HicResBohum1JepsuPatientService();
            hicResultSunapdtlService = new HicResultSunapdtlService();
            hicResBohum1Service = new HicResBohum1Service(); 
            hicResultService = new HicResultService();
            hicSangdamNewService = new HicSangdamNewService();
            hicSunapdtlGroupexamService = new HicSunapdtlGroupexamService();
            hicResultExCodeService = new HicResultExCodeService();
            hicResBohum1JepsuService = new HicResBohum1JepsuService();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {

            Result_Print_Sub();

        }

        private void Result_Print_Sub()
        {

            HIC_JEPSU_PATIENT item = hicJepsuPatientService.GetItembyWrtNo(fnWrtno);

            fstrSname = item.SNAME;
            fstrGjYear = item.GJYEAR;
            fstrChasu = item.GJCHASU;
            fstrJepdate = item.JEPDATE;
            fstrPtno = item.PTNO;
            fstrSex = item.SEX;
            fstrGjjong = item.GJJONG;
            fstrPandate = Convert.ToDateTime(item.PANJENGDATE.ToString()).ToShortDateString();
            fstrJumin = item.JUMIN2;
            fnPano = item.PANO;
            fnAge = item.AGE;
            FnDrno = item.PANJENGDRNO;

            if (hicSunapdtlService.GetCountbyWrtNoCode(fnWrtno, "1160")> 0) { FbChol = true; }

            Result_Print_Sub1();  //1차 공단검진결과지 표지
            Result_Print_Sub2();  //1차 계측검사,혈액검사 등 결과지
            Result_Print_Sub3();  //추가검사 결과지
            Result_Print_Sub4();  //심뇌혈관질환 위험평가
            Result_Print_Sub5();  //생활습관평가 결과지
            //Result_Print_Sub6();  //우울증상과 극복방법(대상자만)
            //Result_Print_Sub7();  //C형간염결과지

        }

        private void Result_Print_Sub1()
        {
            List<string> strExCode = new List<string>();


            string strJumin = "";
            string strLtdCode = "";
            string strData = "";
            string strTemp = "";
            string strTemp1 = "";
            string strList = "";
            string strOK = "";
            string strJuso = "";
            string strDaeSang = "";
            string strMailCode = "";

            bool bHabit = false;

            HIC_RES_BOHUM1_JEPSU_PATIENT item = hicResBohum1JepsuPatientService.GetItembyWrtNo(fnWrtno);


            strLtdCode = VB.Format(item.LTDCODE, "#").ToString();

            //검진일자
            SS1.ActiveSheet.Cells[7,24].Text = fstrJepdate;

            //주소세팅 - 접수기준으로 세팅함
            strJuso = cf.TextBox_2_MultiLine(item.JUSO1.Trim() + " " + item.JUSO2.Trim(), 66);
            strJuso = VB.Replace(strJuso, ComNum.VBLF, "");
            SS1.ActiveSheet.Cells[9, 11].Text = VB.Pstr(strJuso, "{{@}}", 1);
            SS1.ActiveSheet.Cells[10, 11].Text = VB.Pstr(strJuso, "{{@}}", 2);

            //현대제철의 경우 부서/사번입력(일반검진 요청사항)
            if ( item.GBJUSO.IsNullOrEmpty() || item.GBJUSO == "N")
            {
                if (!item.BUSENAME.IsNullOrEmpty() || !item.SABUN.IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Cells[11, 11].Text = "(" + item.BUSENAME + " " + item.SABUN;
                }
                else
                {
                    SS1.ActiveSheet.Cells[11, 11].Text = "";
                }

                if (item.LTDCODE != 0 )
                {
                    //SS1.ActiveSheet.Cells[11, 11].Text = "(" + item.BUSENAME + " " + item.SABUN
                    SS1.ActiveSheet.Cells[12, 15].Text = fstrSname + " 귀하" + "(" + hb.READ_Ltd_Name(item.LTDCODE.ToString()) + ")";
                }
                else
                {
                    SS1.ActiveSheet.Cells[12, 15].Text = fstrSname + " 귀하";
                }
            }

            else if (item.GBJUSO == "Y")
            {
                SS1.ActiveSheet.Cells[11, 11].Text = "";
                SS1.ActiveSheet.Cells[12, 15].Text = fstrSname + " 귀하";
            }


            for (int i = 1; i <= 3; i++)
            {
                strMailCode = strMailCode + VB.Mid(item.MAILCODE, i, 1);
            }
            for (int i = 4; i <= 6; i++)
            {
                strMailCode = strMailCode + VB.Mid(item.MAILCODE, i, 1);
            }
            SS1.ActiveSheet.Cells[13, 23].Text = strMailCode;

            //기본정보
            SS1.ActiveSheet.Cells[8, 1].Text = "수검자성명: " + fstrSname;
            SS1.ActiveSheet.Cells[9, 1].Text = "주민등록번호: " + VB.Left(fstrJumin, 6) + "-" + VB.Mid(fstrJumin, 7, 1) + "******";
            if (item.GBCHUL == "Y")
            {
                SS1.ActiveSheet.Cells[10, 1].Text = "검진장소: □내원, ■출장";
            }
            else
            {
                SS1.ActiveSheet.Cells[10, 1].Text = "검진장소: ■내원, □출장";
            }

            SS1.ActiveSheet.Cells[11, 1].Text = "검진일자: " + fstrJepdate;

            //나의 건강검진 종합소견
            SS1.ActiveSheet.Cells[20, 4].Text = "";
            SS1.ActiveSheet.Cells[20, 9].Text = "";
            SS1.ActiveSheet.Cells[21, 4].Text = "";
            SS1.ActiveSheet.Cells[21, 9].Text = "";
            SS1.ActiveSheet.Cells[21, 20].Text = "";

            switch (item.PANJENG)
            {
                case "1":
                    SS1.ActiveSheet.Cells[20, 4].Text = "1"; break;
                case "2":
                    SS1.ActiveSheet.Cells[20, 9].Text = "1"; break;
                case "3":
                    SS1.ActiveSheet.Cells[21, 4].Text = "1"; break;
                case "4":
                    SS1.ActiveSheet.Cells[21, 9].Text = "1"; strDaeSang = "strOK"; break;
                case "5":
                    SS1.ActiveSheet.Cells[21, 20].Text = "1"; break;
                default: break;
            }


            strExCode.Add("A118"); strExCode.Add("A129"); strExCode.Add("A130"); strExCode.Add("A143"); strExCode.Add("A144");
            strExCode.Add("A145"); strExCode.Add("A146"); strExCode.Add("A147"); strExCode.Add("TX07"); strExCode.Add("A258");
            strExCode.Add("C404");
            List<HIC_RESULT_SUNAPDTL> list = hicResultSunapdtlService.GetItembyWrtnoExcodeIN(fnWrtno, strExCode);

            for (int i = 0; i <= list.Count; i++)
            {
                switch (list[i].EXCODE)
                { 
                case "A143":
                case "A144":
                case "A145":
                case "A146":
                case "A147":
                    bHabit = true; break;
                case "A130":
                    strTemp = strTemp + "우울증,"; break;
                case "A129":
                    strTemp = strTemp + "인지기능,"; break;
                case "A118":
                    strTemp = strTemp + "신체기능검사,"; break;
                case "TX07":
                    strTemp = strTemp + "골다고증검사,"; break;
                case "A258":
                    strTemp = strTemp + "B형간염,"; ;  break;
                case "C404":
                    strTemp = strTemp + "이상지질혈증,"; ; break;
                default: break;
                }
            }

            if(bHabit == true) { strTemp = strTemp + "생활습관,"; }
            if(!strTemp.IsNullOrEmpty()) { VB.Left(strTemp, VB.Len(strTemp) - 1); }

            if (strTemp.IsNullOrEmpty())
            {
                SS1.ActiveSheet.Cells[23, 1].Text = "◆ " + fstrSname + "님은 일반건강검진 검사를 받으셨습니다.";
            }
            else
            {
                strTemp1 = "◆ " + fstrSname + "님은 일반건강검진 그 외 " + strTemp + " 검사를 받으셨습니다.";
                strTemp = VB.Replace(cf.TextBox_2_MultiLine(strTemp1, 96), "{{@}}", ComNum.VBLF);
                SS1.ActiveSheet.Cells[23, 1].Text = strTemp;
                if (VB.InStr(strTemp, ComNum.VBLF)>0) { SS1.ActiveSheet.Rows[23].Height = 20; } 
            }
            
            //생활습관평가 문구를 삭제
            if(bHabit == false) { SS1.ActiveSheet.Cells[24, 2].Text = ""; }

            //판정내역
            SS1.ActiveSheet.Cells[26, 1].Text = "◆ " + fstrSname + "님은 다음 사항에 대한 관리가 필요합니다.";

            if (!item.SOGEN.IsNullOrEmpty())
            {

                strTemp = "▷의심 질환:" + ComNum.VBLF + VB.RTrim(item.SOGEN) + ComNum.VBLF + ComNum.VBLF;
                strTemp = strTemp + "* 확진검사는 검진결과 고혈압.당뇨병 질환의심자가 해당 질환의 진료와 검사를 가까운 병.의원에서 받을 수 있도록" + ComNum.VBLF;
                strTemp = strTemp + "  최초 1회 진료비 지원(검사기간은 다음연도 1.31.까지)" + ComNum.VBLF;
                strTemp = strTemp + "  단, 의료급여수급권자는 의료급여법에 따라 가까운 의원에서 확진검사 가능";
                SS1.ActiveSheet.Cells[27, 1].Text = strTemp;
            }
            else
            {
                SS1.ActiveSheet.Cells[27, 1].Text = "";
            }

            strTemp1 = "";
            if (item.PANJENGU1 == "1") { strTemp1 = strTemp1 + "혈압,"; }
            if (item.PANJENGU2 == "1") { strTemp1 = strTemp1 + "당뇨,"; }
            if (item.PANJENGU3 == "1") { strTemp1 = strTemp1 + "이상지질,"; }
            if (item.PANJENGU4 == "1") { strTemp1 = strTemp1 + "폐결핵,"; }

            //유질환
            if (!strTemp1.IsNullOrEmpty() || !item.SOGENB.IsNullOrEmpty())
            {
                strTemp = "▷유질환: " + ComNum.VBLF + VB.RTrim(item.SOGENB);
                SS1.ActiveSheet.Cells[29, 1].Text = strTemp;
            }
            else
            {
                SS1.ActiveSheet.Cells[29, 1].Text = "▷유질환: 특이소견 없음";
            }

            //생활습관관리
            if (!item.SOGENC.IsNullOrEmpty())
            {
                strTemp = "▷생활습관 관리: " + ComNum.VBLF + VB.RTrim(item.SOGENC);
                SS1.ActiveSheet.Cells[31, 1].Text = strTemp;
            }
            else
            {
                SS1.ActiveSheet.Cells[31, 1].Text = "▷생활습관 관리:특이소견 없음";
            }

            //기타
            if (!item.SOGEND.IsNullOrEmpty())
            {
                strTemp = "▷기타: " + ComNum.VBLF + VB.RTrim(item.SOGEND);
                SS1.ActiveSheet.Cells[33, 1].Text = strTemp;
            }
            else
            {
                SS1.ActiveSheet.Cells[33, 1].Text = "▷기타: 특이소견 없음";
            }


            //출력



        }
        private void Result_Print_Sub2()
        {
            long nTemp = 0;
            double nResult = 0;


            string[] strRes = new string[20];
            string strExcode = "";
            string strRescode = "";
            string strResult = "";
            string strTemp = "";
            string strTemp1 = "";
            string strOldByeng2 = "";
            string strOldByeng5 = "";
            string strPanjengU1 = "";
            string strPanjengU2 = "";
            string strPanjengU3 = "";
            string strPanjengU4 = "";
            string strSI07A = "";
            string strSI072A = "";

            SS2.ActiveSheet.Cells[1, 1].Text = "수검자성명: " + fstrSname;
            SS2.ActiveSheet.Cells[1, 4].Text = "주민등록번호: " + VB.Left(fstrJumin, 6) + "-" + VB.Mid(fstrJumin, 7, 1) + "******";

            HIC_RES_BOHUM1 item = hicResBohum1Service.GetIetmbyWrtNo(fnWrtno);
            strOldByeng2 = item.OLDBYENG2; //혈압
            strOldByeng5 = item.OLDBYENG5; //당뇨
            strPanjengU1 = item.PANJENGU1; //혈압
            strPanjengU2 = item.PANJENGU2; //당뇨
            strPanjengU3 = item.PANJENGU3; //이상지질
            strPanjengU4 = item.PANJENGU4; //폐결핵

            //결과계측
            List<HIC_RESULT> list = hicResultService.GetItembyWrtNo(fnWrtno);
            for (int i = 0; i <= list.Count; i++)
            {
                switch (list[i].EXCODE)
                {
                    case "A101": strRes[1] = list[i].RESULT; break; //키
                    case "A102": strRes[2] = list[i].RESULT; break; //몸무게
                    case "A104": strRes[3] = list[i].RESULT; break; //시력(좌)
                    case "A105": strRes[4] = list[i].RESULT; break; //시력(우)
                    case "A106": strRes[5] = list[i].RESULT; break; //청력(좌)
                    case "A107": strRes[6] = list[i].RESULT; break; //청력(우)
                    case "A117": strRes[7] = list[i].RESULT; break; //체질량지수
                    case "A115": strRes[8] = list[i].RESULT; break; //허리둘레
                    case "A108": strRes[9] = list[i].RESULT; break; //혈압(최고)
                    case "A109": strRes[10] = list[i].RESULT; break; //혈압(최저)
                    default: break;
                }
            }

            //신장,체중
            SS2.ActiveSheet.Cells[4, 5].Text = VB.Format(strRes[1], "#0.0") + "cm / " + VB.Format(strRes[2], "#0.0") + "kg";
            SS2.ActiveSheet.Cells[6, 5].Text = "  (18.5미만)   (18.5~24.9)  (25~29.9)   (30이상)";

            //체질량지수
            if (VB.Val(strRes[7]) < 18.5)
            {
                SS2.ActiveSheet.Cells[6, 5].Text = "  ■ 저체중     □ 정상         □ 과체중     □ 비만";
            }
            else if (VB.Val(strRes[7]) < 25)
            {
                SS2.ActiveSheet.Cells[6, 5].Text = "  □ 저체중     ■ 정상         □ 과체중     □ 비만";
            }
            else if (VB.Val(strRes[7]) < 30)
            {
                SS2.ActiveSheet.Cells[6, 5].Text = "  □ 저체중     □ 정상         ■ 과체중     □ 비만";
            }
            else
            {
                SS2.ActiveSheet.Cells[6, 5].Text = "  □ 저체중     □ 정상         □ 과체중     ■ 비만";
            }

            //허리둘레
            if (fstrSex == "M" && VB.Val(strRes[8]) >= 90)
            {
                SS2.ActiveSheet.Cells[7, 5].Text = strRes[8] + "㎝    " + "□정상   ■복부 비만(남90이상, 여85이상)";
            }
            else if (fstrSex == "F" && VB.Val(strRes[8]) >= 85)
            {
                SS2.ActiveSheet.Cells[7, 5].Text = strRes[8] + "㎝    " + "□정상   ■복부 비만(남90이상, 여85이상)";
            }
            else
            {
                SS2.ActiveSheet.Cells[7, 5].Text = strRes[8] + "㎝    " + "■정상   □복부 비만(남90이상, 여85이상)";
            }

            //시력
            if (VB.InStr(strRes[3], "(") > 0 || VB.InStr(strRes[4], "(") > 0)
            {
                SS2.ActiveSheet.Cells[8, 4].Text = "   시력(좌우)   " + strRes[3] + " / " + strRes[4] + "      ■교정";
            }
            else
            {
                SS2.ActiveSheet.Cells[8, 4].Text = "   시력(좌우)   " + strRes[3] + " / " + strRes[4] + "      □교정";
            }

            //청력
            SS2.ActiveSheet.Cells[9, 4].Text = "   청력(좌우)   " + strRes[5] + " / " + strRes[6];

            //고혈압
            if (strRes[9].IsNullOrEmpty() && strRes[10].IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[10, 4].Text = "비대상";
                SS2.ActiveSheet.Cells[10, 5].Text = " □정상     □유질환자";
                SS2.ActiveSheet.Cells[11, 5].Text = " □고혈압 전단계(수축기 120-139 또는 이완기 80-89)";
                SS2.ActiveSheet.Cells[12, 5].Text = " □고혈압의심(140 이상 또는 90 이상)";
            }
            else
            {
                SS2.ActiveSheet.Cells[10, 4].Text = strRes[9] + " / " + strRes[10] + " mmHg";
                if (strOldByeng2 == "1" || strOldByeng5 == "1")
                {
                    SS2.ActiveSheet.Cells[10, 5].Text = " □정상     ■유질환자";
                    SS2.ActiveSheet.Cells[11, 5].Text = " □고혈압 전단계(수축기 120-139 또는 이완기 80-89)";
                    SS2.ActiveSheet.Cells[12, 5].Text = " □고혈압의심(140 이상 또는 90 이상)";
                }
                else if (VB.Val(strRes[9]) >= 140 || VB.Val(strRes[10]) >= 90)
                {
                    SS2.ActiveSheet.Cells[10, 5].Text = " □정상     □유질환자";
                    SS2.ActiveSheet.Cells[11, 5].Text = " □고혈압 전단계(수축기 120-139 또는 이완기 80-89)";
                    SS2.ActiveSheet.Cells[12, 5].Text = " ■고혈압의심(140 이상 또는 90 이상)";
                }
                else if (VB.Val(strRes[9]) >= 120 || VB.Val(strRes[10]) >= 80)
                {
                    SS2.ActiveSheet.Cells[10, 5].Text = " □정상     □유질환자";
                    SS2.ActiveSheet.Cells[11, 5].Text = " ■고혈압 전단계(수축기 120-139 또는 이완기 80-89)";
                    SS2.ActiveSheet.Cells[12, 5].Text = " □고혈압의심(140 이상 또는 90 이상)";
                }
                else
                {
                    SS2.ActiveSheet.Cells[10, 5].Text = " ■정상     □유질환자";
                    SS2.ActiveSheet.Cells[11, 5].Text = " □고혈압 전단계(수축기 120-139 또는 이완기 80-89)";
                    SS2.ActiveSheet.Cells[12, 5].Text = " □고혈압의심(140 이상 또는 90 이상)";
                }
            }


            for (int i = 0; i <= list.Count; i++)
            {

                strExcode = list[i].EXCODE.Trim();
                strRescode = list[i].RESCODE.Trim();
                strResult = list[i].RESULT.Trim();

                if (!strResult.IsNullOrEmpty())
                {
                    if (strExcode == "C404" || strExcode == "C405")
                    {
                        strResult = VB.Format(cf.FIX_N(VB.Val(strResult)), "#0");
                    }
                }

                switch (strExcode)
                {
                    case "A121": strRes[1] = list[i].RESULT; break;  //혈색소
                    case "A122": strRes[2] = list[i].RESULT; break;  //공복혈당
                    case "A123": strRes[3] = list[i].RESULT; break;  //총콜레스테롤
                    case "A242": strRes[4] = list[i].RESULT; break;  //HDL
                    case "A241": strRes[5] = list[i].RESULT; break;  //중성지방
                    case "C404":
                    case "C405":
                        strRes[6] = list[i].RESULT; break; //LDL
                    case "A274": strRes[7] = list[i].RESULT; break;  //혈청클레아티닌
                    case "A116": strRes[8] = list[i].RESULT; break;  //신사구체여과율
                    case "A124": strRes[9] = list[i].RESULT; break;  //AST
                    case "A125": strRes[10] = list[i].RESULT; break; //ALT
                    case "A126": strRes[11] = list[i].RESULT; break; //감마 지티피(r-GTP)
                    case "A112": strRes[12] = list[i].RESULT; break; //요단백
                    case "A141":
                    case "A142":
                        strRes[13] = list[i].RESULT; break; //방사선
                    case "TX07": strRes[14] = list[i].RESULT; break; //골다공증
                    case "A118": strRes[15] = list[i].RESULT; break; //하지기능
                    case "A119": strRes[16] = list[i].RESULT; break; //보행장애
                    case "A120": strRes[17] = list[i].RESULT; break; //평형성
                    case "A258": strRes[18] = list[i].RESULT; break; //B형간염항원(EIA)
                    case "A259": strRes[19] = list[i].RESULT; break; //B형간염항체(EIA)
                    default: break;
                }
            }

            //총콜레스테롤4종을 접수하지 않았으면 결과를 0으로 변경
            if (FbChol == false)
            {
                strRes[3] = "비대상"; //총콜레스테롤
                strRes[4] = "비대상"; //HDL
                strRes[5] = "비대상"; //중성지방
                strRes[6] = "비대상"; //LDL
            }

            //혈색소
            SS2.ActiveSheet.Cells[14, 5].Text = strRes[1];
            if (strRes[1].IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[14, 5].Text = "비대상";
                SS2.ActiveSheet.Cells[14, 7].Text = " □정상";
                SS2.ActiveSheet.Cells[14, 8].Text = " □기타";
                SS2.ActiveSheet.Cells[15, 7].Text = " □빈혈 의심";
            }
            else
            {
                if( fstrSex =="M" && VB.Val(strRes[1]) >= 13 && VB.Val(strRes[1])<= 16.5)
                {
                    SS2.ActiveSheet.Cells[14, 7].Text = " ■정상";
                    SS2.ActiveSheet.Cells[14, 8].Text = " □기타";
                    SS2.ActiveSheet.Cells[15, 7].Text = " □빈혈 의심";
                }
                else if (fstrSex == "F" && VB.Val(strRes[1]) >= 12 && VB.Val(strRes[1]) <= 15.5)
                {
                    SS2.ActiveSheet.Cells[14, 7].Text = " ■정상";
                    SS2.ActiveSheet.Cells[14, 8].Text = " □기타";
                    SS2.ActiveSheet.Cells[15, 7].Text = " □빈혈 의심";
                }
                else if (fstrSex == "M" && VB.Val(strRes[1]) < 13 )
                {
                    SS2.ActiveSheet.Cells[14, 7].Text = " □정상";
                    SS2.ActiveSheet.Cells[14, 8].Text = " □기타";
                    SS2.ActiveSheet.Cells[15, 7].Text = " ■빈혈 의심";
                }
                else if (fstrSex == "F" && VB.Val(strRes[1]) < 12)
                {
                    SS2.ActiveSheet.Cells[14, 7].Text = " □정상";
                    SS2.ActiveSheet.Cells[14, 8].Text = " □기타";
                    SS2.ActiveSheet.Cells[15, 7].Text = " ■빈혈 의심";
                }
                else if (fstrSex == "M" && VB.Val(strRes[1]) >= 16.5)
                {
                    SS2.ActiveSheet.Cells[14, 7].Text = " □정상";
                    SS2.ActiveSheet.Cells[14, 8].Text = " ■기타";
                    SS2.ActiveSheet.Cells[15, 7].Text = " □빈혈 의심";
                }
                else if (fstrSex == "F" && VB.Val(strRes[1]) >= 15.5)
                {
                    SS2.ActiveSheet.Cells[14, 7].Text = " □정상";
                    SS2.ActiveSheet.Cells[14, 8].Text = " ■기타";
                    SS2.ActiveSheet.Cells[15, 7].Text = " □빈혈 의심";
                }
                else
                {
                    SS2.ActiveSheet.Cells[14, 7].Text = " □정상";
                    SS2.ActiveSheet.Cells[14, 8].Text = " □기타";
                    SS2.ActiveSheet.Cells[15, 7].Text = " □빈혈 의심";
                }
            }


            //공복혈당
            SS2.ActiveSheet.Cells[16, 5].Text = strRes[2];
            if (strRes[2].IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[16, 5].Text = "비대상";
                SS2.ActiveSheet.Cells[16, 7].Text = " □정상             □유질환자";
                SS2.ActiveSheet.Cells[17, 7].Text = " □공복혈당장애의심 □당뇨병의심";
            }
            else
            {
                if(strOldByeng5 == "1" || strPanjengU2 =="1")
                {
                    SS2.ActiveSheet.Cells[16, 7].Text = " □정상             ■유질환자";
                    SS2.ActiveSheet.Cells[17, 7].Text = " □공복혈당장애의심 □당뇨병의심";
                }
                else if (VB.Val(strRes[2])<= 99)
                {
                    SS2.ActiveSheet.Cells[16, 7].Text = " ■정상             □유질환자";
                    SS2.ActiveSheet.Cells[17, 7].Text = " □공복혈당장애의심 □당뇨병의심";
                }
                else if (VB.Val(strRes[2]) <= 125)
                {
                    SS2.ActiveSheet.Cells[16, 7].Text = " □정상             □유질환자";
                    SS2.ActiveSheet.Cells[17, 7].Text = " ■공복혈당장애의심 □당뇨병의심";
                }
                else
                {
                    SS2.ActiveSheet.Cells[16, 7].Text = " □정상             □유질환자";
                    SS2.ActiveSheet.Cells[17, 7].Text = " □공복혈당장애의심 ■당뇨병의심";
                }
            }

            //이상지질혈증
            SS2.ActiveSheet.Cells[18, 5].Text = strRes[3]; //총콜레스테롤
            SS2.ActiveSheet.Cells[19, 5].Text = strRes[4]; //HDL
            SS2.ActiveSheet.Cells[20, 5].Text = strRes[5]; //중성지방
            SS2.ActiveSheet.Cells[21, 5].Text = strRes[6]; //LDL

            if(strRes[3].IsNullOrEmpty() && strRes[4].IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[18, 7].Text = " □정상  □유질환자";
                SS2.ActiveSheet.Cells[19, 7].Text = " □낮은 HDL 콜레스테롤 의심";
                SS2.ActiveSheet.Cells[20, 7].Text = " □고중성지방혈증 의심";
                SS2.ActiveSheet.Cells[21, 7].Text = " □고콜레스테롤혈증 의심";
            }
            else
            {
                if (strRes[4].IsNullOrEmpty() && strRes[5].IsNullOrEmpty() && strRes[6].IsNullOrEmpty())
                {
                    if (VB.Val(strRes[3]) < 200 && VB.Val(strRes[4]) >= 60 && VB.Val(strRes[5]) <150 && VB.Val(strRes[6]) <130)
                    {
                        SS2.ActiveSheet.Cells[18, 7].Text = " ■정상  □유질환자";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[18, 7].Text = " □정상  □유질환자";
                    }

                    if (VB.Val(strRes[4]) <= 59)
                    {
                        SS2.ActiveSheet.Cells[19, 7].Text = " ■낮은 HDL 콜레스테롤 의심";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[19, 7].Text = " □낮은 HDL 콜레스테롤 의심";
                    }

                    if (VB.Val(strRes[5]) >= 150)
                    {
                        SS2.ActiveSheet.Cells[20, 7].Text = " ■고중성지방혈증 의심";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[20, 7].Text = " □고중성지방혈증 의심";
                    }

                    if (VB.Val(strRes[6]) >= 130)
                    {
                        SS2.ActiveSheet.Cells[21, 7].Text = " ■고콜레스테롤혈증 의심";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[21, 7].Text = " □고콜레스테롤혈증 의심";
                    }
                    if (strRes[4] == "비대상" && strRes[5] == "비대상" && strRes[6] == "비대상")
                    {
                        SS2.ActiveSheet.Cells[18, 7].Text = " □정상  □유질환자";
                        SS2.ActiveSheet.Cells[19, 7].Text = " □낮은 HDL 콜레스테롤 의심";
                        SS2.ActiveSheet.Cells[20, 7].Text = " □고중성지방혈증 의심";
                        SS2.ActiveSheet.Cells[21, 7].Text = " □고콜레스테롤혈증 의심";
                    }
                }
            }

            //신장질환
            SS2.ActiveSheet.Cells[22, 5].Text = strRes[7]; //혈청클레아티닌
            SS2.ActiveSheet.Cells[23, 5].Text = strRes[8]; //신사구체여과율

            if (strRes[8].IsNullOrEmpty()) { SS2.ActiveSheet.Cells[22, 5].Text = "비대상"; }
            if (strRes[7].IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[23, 5].Text = "비대상";
                SS2.ActiveSheet.Cells[22, 7].Text = " □정상";
                SS2.ActiveSheet.Cells[23, 7].Text = " □신장기능 이상 의심";
            }
            else
            {
                if(VB.Val(strRes[7]) <=1.2 && VB.Val(strRes[8])>=70)
                {
                    SS2.ActiveSheet.Cells[22, 7].Text = " ■정상";
                    SS2.ActiveSheet.Cells[23, 7].Text = " □신장기능 이상 의심";
                }
                else
                {
                    SS2.ActiveSheet.Cells[22, 7].Text = " □정상";
                    SS2.ActiveSheet.Cells[23, 7].Text = " ■신장기능 이상 의심";
                }
            }

            //간장질환
            SS2.ActiveSheet.Cells[24, 5].Text = strRes[9];  //AST
            SS2.ActiveSheet.Cells[25, 5].Text = strRes[10]; //ALT
            SS2.ActiveSheet.Cells[26, 5].Text = strRes[11]; //감마 지티피(r-GTP)

            if (strRes[9].IsNullOrEmpty()) { SS2.ActiveSheet.Cells[24, 5].Text = "비대상"; }
            if (strRes[10].IsNullOrEmpty()) { SS2.ActiveSheet.Cells[25, 5].Text = "비대상"; }
            if (strRes[11].IsNullOrEmpty()) { SS2.ActiveSheet.Cells[26, 5].Text = "비대상"; }

            if (strRes[9].IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[24, 7].Text = " □정상";
                SS2.ActiveSheet.Cells[25, 7].Text = " □간기능 이상 의심";
            }
            else
            {
                if(fstrSex == "M")
                {
                    if(VB.Val(strRes[9])<=38 && VB.Val(strRes[10]) <= 43 && VB.Val(strRes[11]) <= 54)
                    {
                        SS2.ActiveSheet.Cells[24, 7].Text = " ■정상";
                        SS2.ActiveSheet.Cells[25, 7].Text = " □간기능 이상 의심";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[24, 7].Text = " □정상";
                        SS2.ActiveSheet.Cells[25, 7].Text = " ■간기능 이상 의심";
                    }
                }
                else
                {
                    if (VB.Val(strRes[9]) <= 38 && VB.Val(strRes[10]) <= 43 && VB.Val(strRes[11]) <= 37)
                    {
                        SS2.ActiveSheet.Cells[24, 7].Text = " ■정상";
                        SS2.ActiveSheet.Cells[25, 7].Text = " □간기능 이상 의심";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[24, 7].Text = " □정상";
                        SS2.ActiveSheet.Cells[25, 7].Text = " ■간기능 이상 의심";
                    }
                }
            }


            //요단백
            if (strRes[12].IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[28, 4].Text = "■비대상      □정상     □경계     □단백뇨 의심";
            }
            else
            {
                switch(strRes[12])
                {
                    case "99": SS2.ActiveSheet.Cells[28, 4].Text = " □정상      □경계      □단백뇨 의심"; break;
                    case "01": SS2.ActiveSheet.Cells[28, 4].Text = " ■정상      □경계      □단백뇨 의심"; break;
                    case "02": SS2.ActiveSheet.Cells[28, 4].Text = " □정상      ■경계      □단백뇨 의심"; break;
                    default: SS2.ActiveSheet.Cells[28, 4].Text = " □정상      □경계      ■단백뇨 의심"; break;
                }
            }

            //흉부촬영
            if (strRes[13].IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[30, 4].Text = "■비대상  □정상  □비활동성 폐결핵 □질환의심 □기타";
            }
            else
            {
                switch (strRes[13])
                {
                    case "01": SS2.ActiveSheet.Cells[30, 4].Text = " ■정상  □비활동성 폐결핵 □질환의심 □기타"; break;              //정상
                    case "02": SS2.ActiveSheet.Cells[30, 4].Text = " □정상  □비활동성 폐결핵 □질환의심 ■기타:사진불량"; break;      //사진불량
                    case "03": SS2.ActiveSheet.Cells[30, 4].Text = " □정상  □비활동성 폐결핵 □질환의심 ■기타:비활동성(정상)"; break;//비활동성(정상)
                    case "07": SS2.ActiveSheet.Cells[30, 4].Text = " □정상  □비활동성 폐결핵 ■질환의심:폐결핵의증 □기타"; break;    //폐결핵의증
                    case "08": SS2.ActiveSheet.Cells[30, 4].Text = " □정상  □비활동성 폐결핵 ■질환의심:비결핵성질환 □기타"; break;  //비결핵성질환
                    case "09": SS2.ActiveSheet.Cells[30, 4].Text = " □정상  □비활동성 폐결핵 ■질환의심:순환기계질환 □기타"; break;  //순환기계질환
                    case "10": SS2.ActiveSheet.Cells[30, 4].Text = " □정상  □비활동성 폐결핵 □질환의심 ■기타:진단미정"; break;      //진단미정
                    case "11": SS2.ActiveSheet.Cells[30, 4].Text = " □정상  □비활동성 폐결핵 □질환의심 ■기타:미촬영"; break;        //미촬영
                    case "13": SS2.ActiveSheet.Cells[30, 4].Text = " □정상  ■비활동성 폐결핵 □질환의심 □기타"; break;              //비활동성 폐결핵
                    default: SS2.ActiveSheet.Cells[30, 4].Text = " □정상  □비활동성 폐결핵 □질환의심 ■기타:" + strRes[13]; break;
                }

            }

            //골밀도검사
            if (strRes[14].IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[44, 3].Text = "□해당  ■미해당";
                SS2.ActiveSheet.Cells[44, 5].Text = "";
            }
            else
            {
                SS2.ActiveSheet.Cells[44, 3].Text = "■해당  □미해당";
                SS2.ActiveSheet.Cells[44, 5].Text = "";
                switch (strRes[14])
                {

                    case "01": SS2.ActiveSheet.Cells[44, 5].Text = " -1.0 이상         ■정상  □골감소증  □골다공증"; break;
                    case "02": SS2.ActiveSheet.Cells[44, 5].Text = " -1.0~-2.5 초과    □정상  ■골감소증  □골다공증"; break;
                    case "03": SS2.ActiveSheet.Cells[44, 5].Text = " -2.5 이하         □정상  □골감소증  ■골다공증 "; break;
                    default: SS2.ActiveSheet.Cells[44, 5].Text = strRes[14].Trim() + "   □정상  □골감소증  □골다공증"; break;
                }
            }

            //노인신체기능검사
            if (strRes[15].IsNullOrEmpty() && strRes[16].IsNullOrEmpty() && strRes[17].IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[45, 3].Text = "□해당  ■미해당";
                SS2.ActiveSheet.Cells[45, 5].Text = "";
            }
            else
            {
                SS2.ActiveSheet.Cells[45, 3].Text = "■해당  □미해당";
                SS2.ActiveSheet.Cells[45, 5].Text = "";

                if (VB.Val(strRes[15]) > 10 || VB.Val(strRes[16]) > 01 || VB.Val(strRes[17]) < 20 || VB.Val(strRes[17]) >= 100)
                {
                    SS2.ActiveSheet.Cells[45, 5].Text = " □정상    ■신체기능저하";
                }
                else
                {
                    SS2.ActiveSheet.Cells[45, 5].Text = " □정상    ■신체기능저하";
                }

                if (strRes[16] == "01")
                {
                    strRes[16] = "무";
                }
                else if (strRes[16] == "02")
                {
                    strRes[16] = "유";
                }
                else if (strRes[16] == "03")
                {
                    strRes[16] = "검사불가";
                }
                SS2.ActiveSheet.Cells[46, 5].Text = "하지기능 : " + strRes[15] + "초 / 보행장애 : " + strRes[16] + " / 평행성 : " + strRes[17] + "초";
            }

            //1차 검진결과를 읽음
            switch (strRes[18])
            {
                case "01": strRes[18] = "음성"; break;
                case "02": strRes[18] = "양성"; break;
                default: break;
            }

            //B형간염
            if (strRes[18].IsNullOrEmpty() && strRes[19].IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[37, 3].Text = "□해당  ■미해당";
                SS2.ActiveSheet.Cells[37, 6].Text = "";
                SS2.ActiveSheet.Cells[38, 6].Text = "";
                SS2.ActiveSheet.Cells[39, 5].Text = "";
            }
            else
            {
                SS2.ActiveSheet.Cells[37, 3].Text = "■해당  □미해당";
                SS2.ActiveSheet.Cells[37, 6].Text = " □일반 ■정밀: ( " + strRes[18] + " )";
                SS2.ActiveSheet.Cells[38, 6].Text = " □일반 ■정밀: ( " + strRes[19] + " )";
                SS2.ActiveSheet.Cells[39, 5].Text = "";


                switch (item.LIVER3)
                {
                    case "1": SS2.ActiveSheet.Cells[39, 5].Text = " ■항체있음 □항체없음 □B형간염 보유자 의심 □판정보류"; break;
                    case "2": SS2.ActiveSheet.Cells[39, 5].Text = " □항체있음 ■항체없음 □B형간염 보유자 의심 □판정보류"; break;
                    case "3": SS2.ActiveSheet.Cells[39, 5].Text = " □항체있음 □항체없음 ■B형간염 보유자 의심 □판정보류"; break;
                    case "4": SS2.ActiveSheet.Cells[39, 5].Text = " □항체있음 □항체없음 □B형간염 보유자 의심 ■판정보류"; break;
                    default: break;
                }
            }

            //우울증
            //인지기능장애
            HIC_RESULT item1 = hicResultService.GetResultRowIdbyWrtNoExCode(fnWrtno, "A130");
            if (item1.IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[40, 3].Text = "□해당  ■미해당";
                SS2.ActiveSheet.Cells[40, 5].Text = "";
                SS2.ActiveSheet.Cells[41, 5].Text = "";
            }
            else
            {
                SS2.ActiveSheet.Cells[40, 3].Text = "■해당  □미해당";
                if (VB.Val(item1.RESULT) <= 4)
                {
                    SS2.ActiveSheet.Cells[40, 5].Text = " ■우울증상이 없음(0~4점)        □가벼운 우울증상(5~9점)";
                    SS2.ActiveSheet.Cells[41, 5].Text = " □중간정도 우울증 의심(10~19점) □심한우울증 의심(20~27점)";
                }
                else if (VB.Val(item1.RESULT) <= 9)
                {
                    SS2.ActiveSheet.Cells[40, 5].Text = " □우울증상이 없음(0~4점)        ■가벼운 우울증상(5~9점)";
                    SS2.ActiveSheet.Cells[41, 5].Text = " □중간정도 우울증 의심(10~19점) □심한우울증 의심(20~27점)"; ;
                }
                else if (VB.Val(item1.RESULT) <= 19)
                {
                    SS2.ActiveSheet.Cells[40, 5].Text = " □우울증상이 없음(0~4점)        □가벼운 우울증상(5~9점)";
                    SS2.ActiveSheet.Cells[41, 5].Text = " ■중간정도 우울증 의심(10~19점) □심한우울증 의심(20~27점)";
                }
                else
                {
                    SS2.ActiveSheet.Cells[40, 5].Text = " □우울증상이 없음(0~4점)        □가벼운 우울증상(5~9점)";
                    SS2.ActiveSheet.Cells[41, 5].Text = " □중간정도 우울증 의심(10~19점) ■심한우울증 의심(20~27점)";
                }
            }

            //인지기능장애
            HIC_RESULT item2 = hicResultService.GetResultRowIdbyWrtNoExCode(fnWrtno, "A129");
            if (item2.IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[42, 3].Text = "□해당  ■미해당";
                SS2.ActiveSheet.Cells[42, 5].Text = "";
                SS2.ActiveSheet.Cells[43, 5].Text = "";
            }
            else
            {
                SS2.ActiveSheet.Cells[42, 3].Text = "■해당  □미해당";
                if (VB.Val(item2.RESULT) >= 6)
                {
                    SS2.ActiveSheet.Cells[42, 5].Text = " □특이소견 없음(0-5점)";
                    SS2.ActiveSheet.Cells[43, 5].Text = " ■인지기능 저하 의심(6점 이상)";
                }
                else
                {
                    SS2.ActiveSheet.Cells[42, 5].Text = " ■특이소견 없음(0-5점)";
                    SS2.ActiveSheet.Cells[43, 5].Text = " □인지기능 저하 의심(6점 이상)";
                }
            }

           if (item.T66_INJECT.IsNullOrEmpty() && item.T66_URO.IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[47, 3].Text = "□해당  ■미해당";
                SS2.ActiveSheet.Cells[47, 6].Text = "";
                SS2.ActiveSheet.Cells[48, 6].Text = "";
                SS2.ActiveSheet.Cells[49, 6].Text = "";
                SS2.ActiveSheet.Cells[50, 6].Text = "";
                SS2.ActiveSheet.Cells[51, 6].Text = "";
                SS2.ActiveSheet.Cells[52, 6].Text = "";
            }
           else
            {
                SS2.ActiveSheet.Cells[47, 3].Text = "■해당  □미해당";
                if (item.T66_FALL.Trim() =="1")
                {
                    SS2.ActiveSheet.Cells[47, 6].Text = " □정상  ■낙상 고위험자";
                }
                else
                {
                    SS2.ActiveSheet.Cells[47, 6].Text = " ■정상  □낙상 고위험자";
                }


                if (item.T66_STAT1.Trim() == "2" || item.T66_STAT2.Trim() == "2" || item.T66_STAT3.Trim() == "2" || item.T66_STAT4.Trim() == "2" || item.T66_STAT5.Trim() == "2" || item.T66_STAT6.Trim() == "2")
                {
                    SS2.ActiveSheet.Cells[48, 6].Text = " □정상  ■일상생활 도움 필요";
                }
                else
                {
                    SS2.ActiveSheet.Cells[48, 6].Text = " ■정상  □일상생활 도움 필요";
                }

                if (item.T66_INJECT.Trim() == "1")
                {
                    SS2.ActiveSheet.Cells[49, 6].Text = " □인플루엔자 접종 필요";
                }
                else
                {
                    SS2.ActiveSheet.Cells[49, 6].Text = " ■인플루엔자 접종 필요";
                }

                if (item.TMUN0013.Trim() == "1")
                {
                    SS2.ActiveSheet.Cells[50, 6].Text = " □폐렴구균 접종 필요";
                }
                else
                {
                    SS2.ActiveSheet.Cells[50, 6].Text = " ■폐렴구균 접종 필요";
                }

                if (item.T66_INJECT.Trim() == "1" && item.TMUN0013.Trim() == "1")
                {
                    SS2.ActiveSheet.Cells[51, 6].Text = " ■접종 필요 없음";
                }
                else
                {
                    SS2.ActiveSheet.Cells[51, 6].Text = " □접종 필요 없음";
                }

                if (item.T66_URO.Trim() == "1" )
                {
                    SS2.ActiveSheet.Cells[52, 6].Text = " □정상  ■배뇨장애 의심";
                }
                else
                {
                    SS2.ActiveSheet.Cells[52, 6].Text = " ■정상  □배뇨장애 의심";
                }
            }

            //과거병력
            strTemp = "";
            if (item.OLDBYENG1 == "1") { strTemp = "간장질환,"; }
            if (item.OLDBYENG2 == "1") { strTemp = strTemp + "고혈압,"; }
            if (item.OLDBYENG3 == "1") { strTemp = strTemp + "뇌졸증,"; }
            if (item.OLDBYENG4 == "1") { strTemp = strTemp + "심장병,"; }
            if (item.OLDBYENG5 == "1") { strTemp = strTemp + "당뇨병,"; }
            if (item.OLDBYENG6 == "1") { strTemp = strTemp + "암,"; }
            if (item.OLDBYENG7 == "1") { strTemp = strTemp + "기타,"; }
            if (strTemp.IsNullOrEmpty()) { strTemp = "무,"; }
            strTemp = VB.Left(strTemp, VB.Len(strTemp) - 1);
            SS2.ActiveSheet.Cells[32, 4].Text = " " + strTemp;

            //약물치료여부
            strTemp1 = "";
            if (item.T_STAT02 == "1") { strTemp1 = "OK"; }
            if (item.T_STAT12 == "1") { strTemp1 = "OK"; }
            if (item.T_STAT22 == "1") { strTemp1 = "OK"; }
            if (item.T_STAT32 == "1") { strTemp1 = "OK"; }
            if (item.T_STAT42 == "1") { strTemp1 = "OK"; }
            if (item.T_STAT52 == "1") { strTemp1 = "OK"; }
            if (item.T_STAT62 == "1") { strTemp1 = "OK"; }
            if (strTemp1 == "OK") { SS2.ActiveSheet.Cells[32, 7].Text = "  유"; }
            if (strTemp1 != "OK") { SS2.ActiveSheet.Cells[32, 7].Text = "  무"; }

            //생활습관
            HIC_SANGDAM_NEW item3 = hicSangdamNewService.GetItembyWrtNo(fnWrtno);
            //금연

            if(item3.HABIT1 == "1")
            {
                SS2.ActiveSheet.Cells[33, 4].Text = "  ■금연 필요  ";
            }
            else
            {
                SS2.ActiveSheet.Cells[33, 4].Text = "  □금연 필요  ";
            }
            //절주
            if (item3.HABIT2 == "1")
            {
                SS2.ActiveSheet.Cells[33, 4].Text = SS2.ActiveSheet.Cells[33, 4].Text + "■절주 필요   ";
            }
            else
            {
                SS2.ActiveSheet.Cells[33, 4].Text = SS2.ActiveSheet.Cells[33, 4].Text + "□절주 필요   ";
            }
            //신체활동
            if (item3.HABIT3 == "1")
            {
                SS2.ActiveSheet.Cells[33, 4].Text = SS2.ActiveSheet.Cells[33, 4].Text + "■신체활동 필요 ";
            }
            else
            {
                SS2.ActiveSheet.Cells[33, 4].Text = SS2.ActiveSheet.Cells[33, 4].Text + "□신체활동 필요 ";
            }
            //근력활동
            if (item3.HABIT4 == "1")
            {
                SS2.ActiveSheet.Cells[33, 4].Text = SS2.ActiveSheet.Cells[33, 4].Text + "■근력운동 필요";
            }
            else
            {
                SS2.ActiveSheet.Cells[33, 4].Text = SS2.ActiveSheet.Cells[33, 4].Text + "□근력운동 필요";
            }


            //출력
        }

        //추가검사 결과지
        private void Result_Print_Sub3()
        {

            List<string> strNotCode = new List<string>();

            int nRow = 0;
            long nAscii = 0;
            long nHyelH = 0;
            long nHyelL = 0;
            long nHeight = 0;
            long nWeight = 0;
            long nResult = 0;
            long nLicense = 0;

            string strGjYear = "";
            string strGjChasu = "";
            string strGjBangi = "";
            string strSex = "";
            string strPart = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strResName = "";
            string strGbCodeUse = "";
            string strNomal = "";
            string strIpsadate = "";
            string strGjjong = "";
            string strDrname = "";
            string strRemark = "";
            string strAddSogen = "";
            string strMSogen = "";
            string strREC = "";
            string strABO = "";
            string strRh = "";
            string strExCode = "";
            string strFont1 = "";
            string strFont2 = "";
            string strFont3 = "";
            string strHead = "";



            List<HIC_SUNAPDTL_GROUPEXAM> list = hicSunapdtlGroupexamService.GetExcodeByWrtNo(fnWrtno);
            for (int i = 0; i <= list.Count; i++)
            {
                strNotCode.Add(list[i].EXCODE);
            }

            //김경동메모: strExCodes변수 확인하기


            //추가 검사가 있으면 읽음
            List<HIC_RESULT_EXCODE> list1 = hicResultExCodeService.GetItemByWrtNoNotInExCode(fnWrtno, strNotCode);

            SS3.ActiveSheet.RowCount = list1.Count;
            nRow = -1;
            for (int i = 0; i <= list1.Count; i++)
            {

                strABO = "";
                strRh = "";
                strExCode = list1[i].EXCODE;                                //검사코드
                strResult = list1[i].RESULT;                                //검사실 결과값
                strResCode = list1[i].RESCODE;                              //결과값 코드
                strResultType = list1[i].RESULTTYPE;                        //결과값 TYPE
                strGbCodeUse = list1[i].GBCODEUSE;                          //결과값코드 사용여부



                switch (strExCode)
                {
                    case "H840": strABO = strResult;        break;          //Abo
                    case "H841": strRh = strResult;         break;           //Rh
                    default: break;
                }

                switch (strABO)
                {
                    case "01": strResult = "A"; break;
                    case "02": strResult = "B"; break;
                    case "03": strResult = "O"; break;
                    case "04": strResult = "AB"; break;
                    case "A":
                    case "B":
                    case "C":
                    case "D": strResult = strABO; break;
                    default: break;
                }

                switch (strRh)
                {
                    case "01": strResult = "+"; break;          
                    case "02": strResult = "-"; break;
                    case "-":
                    case "+": strResult = strRh; break;
                    default: break;
                }

                //SS3에 검사실 결과값을 DISPLAY
                nRow = nRow + 1;
                SS3.ActiveSheet.Cells[nRow, 0].Text = " " + list1[i].HNAME.Trim();
                SS3.ActiveSheet.Cells[nRow, 1].Text = " " + strResult;

                if(strGbCodeUse == "Y")
                {
                    if(!strResult.IsNullOrEmpty())
                    {
                        strResName = hb.READ_ResultName(strResCode, strResult);
                        SS3.ActiveSheet.Cells[nRow, 1].Text = " " + strResName;
                    }
                }

                //참고치를 Dispaly
                if(fstrSex == "M")
                {
                    strNomal = list1[i].MIN_M.Trim() + "~" + list1[i].MAX_M.Trim();
                }
                else
                {
                    strNomal = list1[i].MIN_F.Trim() + "~" + list1[i].MAX_F.Trim();
                }
            
                if(strNomal == "~")
                {
                    strNomal = "";
                }
                else if(!strNomal.IsNullOrEmpty() && VB.Right(strNomal,1) == "~")
                {
                    strNomal = VB.Left(strNomal, VB.Len(strNomal) - 1);
                }

                SS3.ActiveSheet.Cells[nRow, 2].Text = " " + strNomal;


                //줄그리기

            }
            
            if(nRow > 0 )
            {
                //추가판정소견이 있는지 읽음
                HIC_RES_BOHUM1 item = hicResBohum1Service.GetItemByWrtno(fnWrtno);
                strAddSogen = item.ADDSOGEN;
            }


            SS3.ActiveSheet.RowCount = nRow;
            if(nRow == 0 && strAddSogen.IsNullOrEmpty()) { return; }

            if (!strAddSogen.IsNullOrEmpty())
            {
                strMSogen = cf.TextBox_2_MultiLine(strAddSogen, 60);
                nRow = nRow + 2;
                SS3.ActiveSheet.RowCount = nRow;
                SS3.ActiveSheet.Cells[nRow, 0].Text = "▶추가검사 판정:";

                for (int i = 1; i <= VB.L(strMSogen,"{{@}}"); i++)
                {
                    nRow = nRow + 1;
                    if(nRow > SS3.ActiveSheet.RowCount) { SS3.ActiveSheet.RowCount = nRow; }
                    SS3.ActiveSheet.Cells[nRow, 0].Text = "  " + VB.Pstr(strMSogen, "{{@}}", i);
                }
                SS3.ActiveSheet.RowCount = nRow;
            }

            //출력




        }
        private void Result_Print_Sub4()
        {

            List<string> strCodeList = new List<string>();

            int nHealth = 0;                                        //n신체활동일수

            long nCNT = 0;
            long nREAD = 0;
            long nBloodMax = 0;                                     //n혈압최고
            long nBloodMin = 0;                                     //n혈압최저
            long nBloodSugar = 0;                                   //n공복혈당

            double nDrink1 = 0;
            double nDrink2 = 0;
            double nHeight = 0;                                     //n키
            double nWeight = 0;                                     //n몸무게
            double nBMI = 0;                                        //n체질량지수
            double nWaist = 0;                                      //n허리둘레
            double nGFR = 0;                                        //nGFR
            double nCholesterol = 0;                                //n총콜레스테롤
            double nLDL = 0;                                        //nLDL
            double nNomalDrink = 0;                                 //str보통음주
            double nNomalDrink1 = 0;                                //str보통음주1
            double nMaxDrink = 0;                                   //str최대음주
            double nMaxDrink1 = 0;                                  //str최대음주1
            //string strCodeList = "";

            string strFormat = "";  

            string strExCode = "";
            string strResult = "";
            string strTemp = "";
            string strTemp1 = "";
            string strTemp2 = "";
            string strSMOKE = "";                                   //str흡연
            string strSMOKE1 = "";                                  //str흡연1
            string strDrink = "";                                   //str음주
            string strCholesterolDrug = "";                         //str콜레스테롤약
            string strHealth1 = "";                                 //str신체활동1
            string strHealth2 = "";                                 //str신체활동2
            string strHealth3 = "";                                 //str신체활동3
            string strDiabetesDrug = "";                            //str당뇨약
            string strBloodDrug = "";                               //str혈압약
            string strProtein = "";                                 //str요단백
            string strDrink1 = "";                                  //str음주1       
            string strDrink2 = "";                                  //str음주2

            
            string strNotDrink = "";                                //str비음주자
            string strNomal1 = "";                                  //str보통1
            string strNomal2 = "";                                  //str보통2
            string strMax1 = "";                                    //str최대1
            string strMax2 = "";                                    //str최대2



            string str절대위험도 = "";
            string str심뇌혈관확률 = "";

            double n위험도_체질량지수  =0;
            double n위험도_허리둘레    =0;
            double n위험도_비만       =0;
            double n위험도_흡연       =0;
            double n위험도_혈압       =0;
            double n위험도_혈압약      =0;
            double n위험도_신체활동    =0;
            double n위험도_공복혈당    =0;
            double n위험도_당뇨약      =0;
            double n위험도_콜레스테롤  =0;
            double n위험도_GFR = 0;
            double n위험도_요단백      =0;
            double n위험도_신장기능 = 0;


            double n복합상대위험도 = 0;
            double n평균복합상대위험도 = 0;
            double n조정복합상대위험도 = 0;
            double n평균절대위험도 = 0;
            double n절대위험도         =0;

            double n목표상대위험도 = 0;
            double n목표조정복합상대위험도 = 0;
            double n목표절대위험도     = 0;
          

            int n심뇌혈관나이 = 0;
            int n목표체중 = 0;
            int n목표심뇌혈관나이 = 0;


            strCodeList.Add("A101"); strCodeList.Add("A102"); strCodeList.Add("A108"); strCodeList.Add("A109"); strCodeList.Add("A112");
            strCodeList.Add("A115"); strCodeList.Add("A116"); strCodeList.Add("A117"); strCodeList.Add("A122"); strCodeList.Add("A123");
            strCodeList.Add("C404"); strCodeList.Add("C405");
            //검사결과를 읽음
            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItemByWrtNoNotInExCode(fnWrtno, strCodeList);

            for (int i = 0; i <= list.Count; i++)
            {
                strExCode = list[i].EXCODE.Trim();                  //검사코드
                strResult = list[i].RESULT.Trim();                  //검사실결과값


                //Case "A101": n키 = strResult ; break;
                //Case "A102": n몸무게 = Val(strResult)
                //Case "A117": n체질량지수 = Val(strResult)
                //Case "A115": n허리둘레 = Val(strResult)
                //Case "A108": n혈압최고 = Val(strResult)
                //Case "A109": n혈압최저 = Val(strResult)
                //Case "A122": n공복혈당 = Val(strResult)
                //Case "A116": nGFR = Val(strResult)
                //Case "A112": str요단백 = strResult
                //Case "A123": n총콜레스테롤 = Val(strResult)
                //Case "C404": nLDL = Val(strResult)
                //Case "C405": nLDL = Val(strResult)

                switch (strExCode)
                {
                    case "A101": strResult = strResult; break;
                    case "A102": strResult = strResult; break;
                    case "A117": strResult = strResult; break;
                    case "A115": strResult = strResult; break;
                    case "A108": strResult = strResult; break;
                    case "A109": strResult = strResult; break;
                    case "A122": strResult = strResult; break;
                    case "A116": strResult = strResult; break;
                    case "A112": strResult = strResult; break;
                    case "A123": strResult = strResult; break;
                    case "C404": strResult = strResult; break;
                    case "C405": strResult = strResult; break;
                    default: break;
                }
            }

            //총콜레스테롤종을 접수하지 않았으면 결과를 0으로 변경
            if( FbChol ==false)
            {
                nCholesterol = 0;       //총콜레스테롤
                nLDL = 0;               //LDL
            }

            HIC_RES_BOHUM1_JEPSU item = hicResBohum1JepsuService.GetItembyWrtNo(fnWrtno);
            if(!item.IsNullOrEmpty())
            {
                strSMOKE = item.T_SMOKE1.Trim();
                strSMOKE1 = item.TMUN0103.Trim();
                strDrink = item.HABIT1.Trim();
                strBloodDrug = item.T_STAT22.Trim();                        //혈압약
                strDiabetesDrug = item.T_STAT22.Trim();                     //당뇨약
                strCholesterolDrug = item.T_STAT42.Trim();                  //콜레스테롤약
                strHealth1 = item.T_ACTIVE1.Trim();                         //신체활동1
                strHealth2 = item.T_ACTIVE2.Trim();                         //신체활동2
                strHealth3 = item.T_ACTIVE3.Trim();                         //신체활동3

                if (item.PANJENGU1 == "1") { strBloodDrug = "1"; }          //혈압약
                if (item.PANJENGU2 == "1") { strDiabetesDrug = "1"; }       //당뇨약
                if (item.PANJENGU3 == "1") { strCholesterolDrug = "1"; }    //콜레스테롤약
            }

            nNomalDrink = 0; nMaxDrink = 0; strNotDrink = "";
            strNotDrink = item.TMUN0003.Trim();
                
            //보통음주
            nDrink1 = 0; nDrink2 = 0;
            if (item.TMUN0003 == "4")
            {
                strDrink = "0";                                                //비음주자
            }
            else
            {
                //음주계산공식
                for (int i = 1; i <= 3; i++)
                {
                    //strTemp = Trim(AdoGetString(AdoRes, "TMUN00" & Format(i + 4, "00"), 0))
                    strFormat = "TMUN00" + VB.Format(i + 4, "00");
                    //strTemp = item.strFormat
                    if (strTemp.IsNullOrEmpty())
                    {
                        //소주
                        if (VB.Pstr(strTemp, ";", 1) == "1")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() * ((double)4 / 7)); break;
                                case "병": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() * 4); break;
                                case "CC": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() / 90); break;
                                default: break;
                            }
                        }

                        //맥주
                        if (VB.Pstr(strTemp, ";", 1) == "2")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() * ((double)200 / 350)); break;
                                case "병": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() * ((double)500 / 350)); break;
                                case "CC": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() / 350); break;
                                case "캔": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>()); break;
                                default: break;
                            }
                        }

                        //양주
                        if (VB.Pstr(strTemp, ";", 1) == "3")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>()); break;
                                case "병": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() * ((double)500 / 45)); break;
                                case "CC": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() / 45); break;
                                default: break;
                            }
                        }

                        //막걸리
                        if (VB.Pstr(strTemp, ";", 1) == "4")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>()); break;
                                case "병": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() * ((double)750 / 300)); break;
                                case "CC": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() / 300); break;
                                default: break;
                            }
                        }

                        //와인
                        if (VB.Pstr(strTemp, ";", 1) == "5")
                        {
                            switch (VB.Pstr(strTemp, ";", 3))
                            {
                                case "잔": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>()); break;
                                case "병": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() * ((double)750 / 150)); break;
                                case "CC": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() / 150); break;
                                default: break;
                            }
                        }
                    }
                }

                nDrink1 = nDrink1 * item.TMUN0004.To<double>();

                switch (item.TMUN0003.Trim())
                {
                    case "1": nNomalDrink = nDrink1 * 1; break;    //일주일
                    case "2": nNomalDrink = nDrink1 / 4; break;    //한달
                    case "3": nNomalDrink = nDrink1 / 48; break;    //1년
                }


                nDrink1 = 0;
                //최대음주
                for (int i = 1; i <= 3; i++)
                {

                    //strTemp = Trim(AdoGetString(AdoRes, "TMUN00" & Format(i + 7, "00"), 0))
                    strFormat = "TMUN00" + VB.Format(i + 7, "00");
                    //strTemp = item.strFormat
                    if (strTemp.IsNullOrEmpty())
                    {
                        if (VB.Pstr(strTemp, ";", 1) == "1")
                        {
                            {
                                //소주
                                if (VB.Pstr(strTemp, ";", 1) == "1")
                                {
                                    switch (VB.Pstr(strTemp, ";", 3))
                                    {
                                        case "잔": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() * ((double)4 / 7)); break;
                                        case "병": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() * 4); break;
                                        case "CC": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() / 90); break;
                                        default: break;
                                    }
                                }

                                //맥주
                                if (VB.Pstr(strTemp, ";", 1) == "2")
                                {
                                    switch (VB.Pstr(strTemp, ";", 3))
                                    {
                                        case "잔": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() * ((double)200 / 350)); break;
                                        case "병": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() * ((double)500 / 350)); break;
                                        case "CC": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() / 350); break;
                                        case "캔": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>()); break;
                                        default: break;
                                    }
                                }

                                //양주
                                if (VB.Pstr(strTemp, ";", 1) == "3")
                                {
                                    switch (VB.Pstr(strTemp, ";", 3))
                                    {
                                        case "잔": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>()); break;
                                        case "병": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() * ((double)500 / 45)); break;
                                        case "CC": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() / 45); break;
                                        default: break;
                                    }
                                }

                                //막걸리
                                if (VB.Pstr(strTemp, ";", 1) == "4")
                                {
                                    switch (VB.Pstr(strTemp, ";", 3))
                                    {
                                        case "잔": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>()); break;
                                        case "병": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() * ((double)750 / 300)); break;
                                        case "CC": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() / 300); break;
                                        default: break;
                                    }
                                }

                                //와인
                                if (VB.Pstr(strTemp, ";", 1) == "5")
                                {
                                    switch (VB.Pstr(strTemp, ";", 3))
                                    {
                                        case "잔": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>()); break;
                                        case "병": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() * ((double)750 / 150)); break;
                                        case "CC": nDrink1 += (VB.Pstr(strTemp, ";", 2).To<double>() / 150); break;
                                        default: break;
                                    }
                                }
                            }
                        }

                        nMaxDrink = nDrink1 * item.TMUN0004.To<double>();
                        nMaxDrink1 = nDrink1;


                        nNomalDrink1 = VB.Format((nNomalDrink / 7), "##############0.00").To<double>();
                        nMaxDrink1 = VB.Format((nMaxDrink / item.TMUN0004.To<double>()), "##############0.00").To<double>();
                    }
                }
            }



            //체질량지수
            if ( fstrSex == "M")
            {
                if (nBMI < 25 )
                {
                    n위험도_체질량지수 = 1;
                }
                else if (nBMI < 26.5)
                {
                    n위험도_체질량지수 = 1.04;
                }
                else if (nBMI < 28)
                {
                    n위험도_체질량지수 = 1.15;
                }
                else if (nBMI < 30)
                {
                    n위험도_체질량지수 = 1.36;
                }
                else
                {
                    n위험도_체질량지수 = 1.58;
                }
            }
            else
            {
                if (nBMI < 25)
                {
                    n위험도_체질량지수 = 1;
                }
                else if (nBMI < 26.5)
                {
                    n위험도_체질량지수 = 1.02;
                }
                else if (nBMI < 28)
                {
                    n위험도_체질량지수 = 1.03;
                }
                else if (nBMI < 30)
                {
                    n위험도_체질량지수 = 1.15;
                }
                else
                {
                    n위험도_체질량지수 = 1.33;
                }
            }

            //허리둘레
            if (fstrSex == "M")
            {
                if (nWaist < 25)
                {
                    n위험도_허리둘레 = 1;
                }
                else
                {
                    n위험도_허리둘레 = 1.44;
                }
            }
            else
            {
                if (nWaist < 25)
                {
                    n위험도_허리둘레 = 1;
                }
                else  
                {
                    n위험도_허리둘레 = 1.52;
                }
            }

            //비만 위험도
            if(n위험도_체질량지수 > n위험도_허리둘레)
            {
                n위험도_비만 = n위험도_체질량지수;
            }
            else
            {
                n위험도_비만 = n위험도_허리둘레;
            }

            //흡연위험도
            if(fstrSex == "M")
            {
                switch (strSMOKE)
                {
                    case "2": n위험도_흡연 = 1.3; break;
                    case "1": n위험도_흡연 = 1.6; break;
                    default: break;
                }
            }
            else
            {
                switch(strSMOKE)
                {
                    case "2": n위험도_흡연 = 1.2; break;
                    case "1": n위험도_흡연 = 1.6; break;
                    default: break;
                }
            }

            if (fstrSex == "M")
            {
                switch (strSMOKE1)
                {
                    case "1": n위험도_흡연 = 1; break;
                    default: break;
                }
            }
            else
            {
                switch (strSMOKE1)
                {
                    case "1": n위험도_흡연 = 1; break;
                    default: break;
                }
            }

            //혈압 위험도
            if(nBloodMax == 0 && nBloodMin == 0)
            {

            }
            else
            {

            }

            //음주목표상태 표시
            if(fstrSex =="M" && fnAge < 65 && nNomalDrink > 14)
            {
                SS4.ActiveSheet.Cells[25, 17].Text = "일주일 14잔 이하";
                strDrink1 = "OK";
                strMax1 = "일주일 14잔 이하";
            }
            else if (fstrSex == "M" && fnAge >= 65 && nNomalDrink > 7)
            {
                SS4.ActiveSheet.Cells[25, 17].Text = "일주일 7잔 이하";
                strDrink1 = "OK";
                strMax1 = "일주일 7잔 이하";
            }
            else if (fstrSex == "F" && fnAge < 65 && nNomalDrink > 7)
            {
                SS4.ActiveSheet.Cells[25, 17].Text = "일주일 7잔 이하";
                strDrink1 = "OK";
                strMax1 = "일주일 7잔 이하";
            }
            else if (fstrSex == "F" && fnAge >= 65 && nNomalDrink > 3)
            {
                SS4.ActiveSheet.Cells[25, 17].Text = "일주일 3잔 이하";
                strDrink1 = "OK";
                strMax1 = "일주일 3잔 이하";
            }

            if (fstrSex == "M")
            {
                if (nMaxDrink1 >4 )
                {
                    SS4.ActiveSheet.Cells[26, 17].Text = "하루 4잔 이하";
                    strDrink2 = "OK";
                    strMax2 = "하루 4잔 이하";
                }
                else
                {
                    SS4.ActiveSheet.Cells[26, 17].Text = "유지";
                    strMax2 = "유지";
                }
            }
            else if (fstrSex == "F")
            {
                if (nMaxDrink1 > 3)
                {
                    SS4.ActiveSheet.Cells[26, 17].Text = "하루 3잔 이하";
                    strDrink2 = "OK";
                    strMax2 = "하루 3잔 이하";
                }
                else
                {
                    SS4.ActiveSheet.Cells[26, 17].Text = "유지";
                    strMax2 = "유지";
                }
            }

            if (!strMax1.IsNullOrEmpty() && !strMax1.IsNullOrEmpty())
            {
                SS4.ActiveSheet.Cells[25, 17].Text = strMax1 + ComNum.VBLF + strMax2;
            }
            else if (!strMax1.IsNullOrEmpty())
            {
                SS4.ActiveSheet.Cells[25, 17].Text = strMax1;
            }
            else if (!strMax2.IsNullOrEmpty())
            {
                SS4.ActiveSheet.Cells[25, 17].Text = strMax2;
            }


            //SS4.Row = 26: SS4.Col = 26
            //If str음주1 = "" And str음주2 = "" Then
            //    SS4.TypePictPicture = Picture3.Picture '안전
            //ElseIf str음주1 = "OK" And str음주2 = "OK" Then
            //    SS4.TypePictPicture = Picture1.Picture '위험
            //ElseIf str음주1 = "OK" Or str음주2 = "OK" Then
            //    SS4.TypePictPicture = Picture2.Picture '주의
            //End If

            if(nBloodMax == 0 && nBloodMin == 0)
            {
                SS4.ActiveSheet.Cells[27, 10].Text = "";
                SS4.ActiveSheet.Cells[27, 17].Text = "";
                SS4.ActiveSheet.Cells[27, 25].Text = "";
            }
            else
            {

            }

            //음주목표상태 표시
            if(fstrSex =="M" && fnAge < 65 && nNomalDrink > 14)
            {
                SS4.ActiveSheet.Cells[25, 17].Text = "일주일 14잔 이하";
                strDrink1 = "OK";
                strMax1 = "일주일 14잔 이하";
            }
            else if (fstrSex == "M" && fnAge >= 65 && nNomalDrink > 7)
            {
                SS4.ActiveSheet.Cells[25, 17].Text = "일주일 7잔 이하";
                strDrink1 = "OK";
                strMax1 = "일주일 7잔 이하";
            }
            else if (fstrSex == "F" && fnAge < 65 && nNomalDrink > 7)
            {
                SS4.ActiveSheet.Cells[25, 17].Text = "일주일 7잔 이하";
                strDrink1 = "OK";
                strMax1 = "일주일 7잔 이하";
            }
            else if (fstrSex == "F" && fnAge >= 65 && nNomalDrink > 3)
            {
                SS4.ActiveSheet.Cells[25, 17].Text = "일주일 3잔 이하";
                strDrink1 = "OK";
                strMax1 = "일주일 3잔 이하";
            }

            if (fstrSex == "M")
            {
                if (nMaxDrink1 >4 )
                {
                    SS4.ActiveSheet.Cells[26, 17].Text = "하루 4잔 이하";
                    strDrink2 = "OK";
                    strMax2 = "하루 4잔 이하";
                }
                else
                {
                    SS4.ActiveSheet.Cells[26, 17].Text = "유지";
                    strMax2 = "유지";
                }
            }
            else if (fstrSex == "F")
            {
                if (nMaxDrink1 > 3)
                {
                    SS4.ActiveSheet.Cells[26, 17].Text = "하루 3잔 이하";
                    strDrink2 = "OK";
                    strMax2 = "하루 3잔 이하";
                }
                else
                {
                    SS4.ActiveSheet.Cells[26, 17].Text = "유지";
                    strMax2 = "유지";
                }
            }

            if (!strMax1.IsNullOrEmpty() && !strMax1.IsNullOrEmpty())
            {
                SS4.ActiveSheet.Cells[25, 17].Text = strMax1 + ComNum.VBLF + strMax2;
            }
            else if (!strMax1.IsNullOrEmpty())
            {
                SS4.ActiveSheet.Cells[25, 17].Text = strMax1;
            }
            else if (!strMax2.IsNullOrEmpty())
            {
                SS4.ActiveSheet.Cells[25, 17].Text = strMax2;
            }


            //SS4.Row = 26: SS4.Col = 26
            //If str음주1 = "" And str음주2 = "" Then
            //    SS4.TypePictPicture = Picture3.Picture '안전
            //ElseIf str음주1 = "OK" And str음주2 = "OK" Then
            //    SS4.TypePictPicture = Picture1.Picture '위험
            //ElseIf str음주1 = "OK" Or str음주2 = "OK" Then
            //    SS4.TypePictPicture = Picture2.Picture '주의
            //End If

            if(nBloodMax == 0 && nBloodMin == 0)
            {
                SS4.ActiveSheet.Cells[27, 10].Text = "";
                SS4.ActiveSheet.Cells[27, 17].Text = "";
                SS4.ActiveSheet.Cells[27, 25].Text = "";
            }
            else
            {

                SS4.ActiveSheet.Cells[27, 10].Text = nBloodMax + "/" + nBloodMin;
                SS4.ActiveSheet.Cells[27, 17].Text = "120/80 미만";
                //SS4.Row = 28: SS4.Col = 26
                if (strBloodDrug == "1")
                {
                    if(nBloodMax <140 && nBloodMin < 90)
                    {
                        //SS4.TypePictPicture = Picture2.Picture '주의
                    }
                    else
                    {
                        //SS4.TypePictPicture = Picture1.Picture '위험
                    }
                }
                else
                {
                    if (nBloodMax < 120 && nBloodMin < 80)
                    {
                        //SS4.TypePictPicture = Picture3.Picture '안전
                    }
                    else if (nBloodMax <= 139 && nBloodMin <= 89)
                    {
                        //SS4.TypePictPicture = Picture2.Picture '주의
                    }
                    else
                    {
                        //SS4.TypePictPicture = Picture1.Picture '위험
                    }
                }
            }

            SS4.ActiveSheet.Cells[29, 10].Text = "";
            switch (strSMOKE1)
            {
                case "1": SS4.ActiveSheet.Cells[29, 10].Text ="비흡연"; break;
                default:  break;
            }

            switch (strSMOKE)
            {
                case "2": SS4.ActiveSheet.Cells[29, 25].Text = "비흡연"; break;
                case "1": SS4.ActiveSheet.Cells[29, 25].Text = "비흡연"; break;
                default: SS4.ActiveSheet.Cells[29, 25].Text = "";  break;

                //Case "2":  SS4.TypePictPicture = Picture2.Picture '주의  '과거흡연
                //Case "1":  SS4.TypePictPicture = Picture1.Picture '위험  '현재흡연
                //Case Else: SS4.TypePictPicture = Picture3.Picture '안전
            }

            if (nBloodSugar == 0)
            {
                SS4.ActiveSheet.Cells[31, 10].Text = "";
                SS4.ActiveSheet.Cells[31, 17].Text = "";
                SS4.ActiveSheet.Cells[31, 25].Text = "";
                strDiabetesDrug = "";
            }
            else
            {
                SS4.ActiveSheet.Cells[31, 10].Text = nBloodSugar.ToString();
                SS4.ActiveSheet.Cells[31, 17].Text = "100 미만";
                //SS4.ActiveSheet.Cells[31, 25].Text = "";
                if (strDiabetesDrug== "1")
                {
                    if (nBloodSugar < 126)
                    {
                        //SS4.TypePictPicture = Picture2.Picture '주의
                    }
                    else
                    {
                        //SS4.TypePictPicture = Picture1.Picture '위험
                    }
                }
                else
                {
                    if (nBloodSugar < 100)
                    {
                        //SS4.TypePictPicture = Picture3.Picture '안전
                    }
                    else if (nBloodSugar < 125)
                    {
                        //SS4.TypePictPicture = Picture2.Picture '주의
                    }
                    else
                    {
                        //SS4.TypePictPicture = Picture1.Picture '위험
                    }
                }
            }

            //총콜레스테롤
            if (nCholesterol != 0 && nLDL != 0)
            {
                SS4.ActiveSheet.Cells[33, 10].Text = nCholesterol.ToString();
                SS4.ActiveSheet.Cells[33, 17].Text = "200 미만";
                SS4.ActiveSheet.Cells[34, 10].Text = nLDL.ToString();

                if (strDiabetesDrug == "1")
                {
                    SS4.ActiveSheet.Cells[34, 17].Text = "100 미만";
                }
                else
                {
                    SS4.ActiveSheet.Cells[34, 17].Text = "130 미만";
                }

                //SS4.Row = 34: SS4.Col = 26
                if (nCholesterol < 200)
                {
                    if(strCholesterolDrug=="1")
                    {
                        if (strDiabetesDrug == "1")
                        {
                            if ( nLDL < 100)
                            {
                                //주의이미지
                            }
                            else
                            {
                                //위험이미지
                            }
                        }
                        else
                        {
                            if (nLDL < 130)
                            {
                                //주의이미지
                            }
                            else
                            {
                                //위험이미지
                            }

                        }
                    }
                    else
                    {
                        if (strDiabetesDrug == "1")
                        {
                            if (nLDL < 100)
                            {
                                if(nCholesterol >200)
                                {
                                    //주의이미지
                                }
                                else
                                {
                                    //안전이미지
                                }
                            }
                            else
                            {
                                //위험이미지
                            }
                        }
                        else
                        {
                            if (nLDL < 130)
                            {
                                if (nCholesterol > 200)
                                {
                                    //주의이미지
                                }
                                else
                                {
                                    //안전이미지
                                }
                            }
                            else if (nLDL < 160)
                            {
                                //주의이미지
                            }
                            else
                            {
                                //위험이미지
                            }
                        }
                    }
                }

                else if (nCholesterol >= 200)
                {
                    if (strCholesterolDrug == "1")
                    {
                        if (strDiabetesDrug == "1")
                        {
                            if (nLDL < 100)
                            {
                                //주의이미지
                            }
                            else
                            {
                                //위험이미지
                            }
                        }
                        else
                        {
                            if (nLDL < 130)
                            {
                                //주의이미지
                            }
                            else
                            {
                                //위험이미지
                            }

                        }
                    }
                    else
                    {
                        if (strDiabetesDrug == "1")
                        {
                            if (nLDL < 100)
                            {
                                //주의이미지
                            }
                            else
                            {
                                //위험이미지
                            }
                        }
                        else
                        {
                            if (nLDL < 130)
                            {
                                //주의이미지
                            }
                            else if(nLDL < 160)
                            {
                                //주의이미지
                            }
                            else
                            {
                                //위험이미지
                            }
                        }
                    }
                }
            }

            //건강위험요인 조절하면
            if (n절대위험도 == 0)
            {
                SS4.ActiveSheet.Cells[46, 1].Text = "0% 감소( 0% -> 0%)";

            }
            else
            {
                SS4.ActiveSheet.Cells[46, 1].Text = VB.Format((n절대위험도 - n목표절대위험도) / n절대위험도 * 100, "#0") + "% 감소 (" + VB.Format(n절대위험도 / 1000, "#0.0") + "% ->" + VB.Format(n목표절대위험도 / 1000, "#0.0") + "%)";
            }

            SS4.ActiveSheet.Cells[46, 19].Text = VB.Format(n목표절대위험도 / 1000, "#0.0") + "%)";

            //판정일자 등
            SS4.ActiveSheet.Cells[50, 5].Text = fstrPandate;
            SS4.ActiveSheet.Cells[50, 14].Text = fstrTongbo;
            SS4.ActiveSheet.Cells[49, 20].Text = FnDrno.ToString();
            SS4.ActiveSheet.Cells[50, 20].Text = hb.READ_License_DrName(FnDrno);


            //출력로직 



        }
        private void Result_Print_Sub5()
        {
            int[] nJumsu = new int[10];

            string[] strGbn = new string[6];
            string[] strSlipPrt = new string[6];
            string strTemp1 = "";
            string strTemp2 = "";
            string strOK = "";
            string strSLIP1 = "";
            string strSLIP2 = "";
            string strSLIP3 = "";
            string strSLIP4 = "";
            string strSLIP5 = "";







        }

    }
}
