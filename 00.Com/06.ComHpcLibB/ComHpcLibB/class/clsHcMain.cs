using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Repository;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public class clsHcMain
    {
        #region !-- MVC Service 선언 --!
        clsHaBase cHaB = new clsHaBase();
        HicJepsuService hicJepsuService = new HicJepsuService();
        HicExjongService hicExjongService = new HicExjongService();
        HicJepsuExjongPatientService hicJepsuExjongPatientService = new HicJepsuExjongPatientService();
        ExamDisplayNewService examDisplayNewService = new ExamDisplayNewService();
        HicPatientService hicPatientService = new HicPatientService();
        ComHpcLibBService comHpcLibBService = new ComHpcLibBService();
        HicSangdamNewService hicSangdamNewService = new HicSangdamNewService();
        HicMcodeService hicMcodeService = new HicMcodeService();
        HicCodeService hicCodeservice = new HicCodeService();
        HicGroupcodeService hicGroupcodeService = new HicGroupcodeService();
        HicExcodeService hicExCodeservice = new HicExcodeService();
        HicResultService hicResultService = new HicResultService();
        HicResSpecialService hicResSpecialService = new HicResSpecialService();
        HicSpcPanjengService hicSpcPanjengService = new HicSpcPanjengService();
        HicResBohum1Service hicResBohum1Service = new HicResBohum1Service();
        HicResBohum2Service hicResBohum2Service = new HicResBohum2Service();
        HicCancerNewService hicCancerNewService = new HicCancerNewService();
        HicXrayResultService hicXrayResultService = new HicXrayResultService();
        HicResDentalService hicResDentalService = new HicResDentalService();
        HeaResultwardService heaResultwardService = new HeaResultwardService();
        HeaJepsuService heaJepsuService = new HeaJepsuService();
        HeaResvLtdService heaResvLtdService = new HeaResvLtdService();
        HicResultHisService hicResultHisService = new HicResultHisService();
        ComHpcLibBRepository comHpcLibBRepository = new ComHpcLibBRepository();
        HeaAutopanService heaAutopanService = new HeaAutopanService();
        HeaAutopanLogicService heaAutopanLogicService = new HeaAutopanLogicService();
        HeaAutopanLogicResultService heaAutopanLogicResultService = new HeaAutopanLogicResultService();
        HeaAutoPanMatchResultService heaAutoPanMatchResultService = new HeaAutoPanMatchResultService();
        HicSpcScodeService hicSpcScodeService = new HicSpcScodeService();
        HicSchoolNewService hicSchoolNewService = new HicSchoolNewService();
        HicBcodeService hicBcodeService = new HicBcodeService();
        HeaJepsuPatientService heaJepsuPatientService = new HeaJepsuPatientService();
        HicTitemService hicTitemService = new HicTitemService();
        HeaResultService heaResultService = new HeaResultService();
        HeaResvExamService heaResvExamService = new HeaResvExamService();
        HicJepsuPatientService hicJepsuPatientService = new HicJepsuPatientService();
        #endregion

        /// <summary>
        /// 자동판정 세팅
        /// </summary>
        /// <param name="ArgWRTNO">접수번호</param>
        /// <param name="nPano">검진번호</param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> Auto_Panjen_BEtc"/>
        /// <comment>진입 전 clsHcType.HFA_Clear(); 클리어 해야함</comment> 
        public void Auto_Panjen_BEtc(long ArgWRTNO, long nPano)
        {
            string[] strExamList = new string[] { "A117", "A121", "A103", "A106", "A107", "A104", "A105" };

            clsHcVariable.Gstr_PanB_Etc = "";
            clsHcVariable.GnPanB_Etc = 0;

            //기존 logic에서 접수여부 확인 및 성별확인 부분 생략함

            // TODO: Auto_Panjen_BEtc 로직 진입전 해당 환자정보 읽어오는지 확인
            // 
            //if (!hicPatientService.Set_HCPAT_Info(nPano))
            //{
            //    MessageBox.Show("환자정보가 없습니다.","확인");
            //    return;
            //}

            IList<EXAM_DISPLAY_NEW> list = examDisplayNewService.GetItemsInResultExCode(ArgWRTNO, strExamList);

            if (list.Count == 0)
            {
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].EXCODE == "A121")       //혈색소
                {
                    if (clsHcType.HPI.SEX == "M")
                    {
                        if (VB.Val(list[i].RESULT) > 16.5)
                        {
                            clsHcType.TFA.PanB[8] = true;
                            clsHcVariable.GnPanB_Etc = 1;
                            clsHcVariable.Gstr_PanB_Etc = "혈색소과다:변화관찰/ ";
                        }
                    }
                    else
                    {
                        if (VB.Val(list[i].RESULT) > 15.5)
                        {
                            clsHcType.TFA.PanB[8] = true;
                            clsHcVariable.GnPanB_Etc = 1;
                            clsHcVariable.Gstr_PanB_Etc = "혈색소과다:변화관찰/ ";
                        }
                    }
                }
                else if (list[i].EXCODE == "A117")  //체질량지수
                {
                    if (VB.Val(list[i].RESULT) < 18.5)  //저체중은 제외
                    {
                        clsHcType.TFA.PanB[8] = true;
                        clsHcVariable.GnPanB_Etc = 2;
                        clsHcVariable.Gstr_PanB_Etc = "저체중관리:균형식이조절요/";
                    }
                }
                else if (list[i].EXCODE == "A103")  //비만도
                {
                    if (list[i].RESULT == "01")
                    {
                        clsHcType.TFA.PanB[8] = true;
                        clsHcVariable.GnPanB_Etc = 2;
                    }
                }
                else if (list[i].EXCODE == "A104" || list[i].EXCODE == "A105")  //시력
                {
                    if (VB.Left(list[i].RESULT, 1) == "(" && VB.Right(list[i].RESULT, 1) == ")") { list[i].RESULT = VB.STRCUT(list[i].RESULT, "(", ")"); }
                    if (VB.Val(list[i].RESULT) <= 0.4)
                    {
                        clsHcType.TFA.PanB[8] = true;
                        clsHcVariable.GnPanB_Etc = 3;
                        clsHcVariable.Gstr_PanB_Etc = "시력저하:추적검사/ ";
                    }
                }
            }

            return;
        }

        /// <summary>
        /// 일반검진 자동판정
        /// </summary>
        /// <param name="ArgWRTNO">접수번호</param>
        /// <param name="ArgGbHea">종검접수여부 Y or Null </param>
        /// <seealso cref="HcMain.bas> Auto_NewPanjeng_First_2019"/>
        /// <comment>진입 전 clsHcType.HFA_Clear(); 클리어 해야함
        ///                  clsHcType.HFA 배열선언 해야함</comment>
        public void Auto_NewPanjeng_First(long ArgWRTNO, string ArgJepDate, string ArgYear, string ArgSex, string ArgGbHea = "")
        {
            string strExPan = string.Empty;
            string strExCode = string.Empty;
            string strResult = string.Empty;
            string strBiman = string.Empty;
            int nHyelH = 0;
            int nHyelL = 0;
            int nGoJi = 0;
            int nLiver = 0;
            int nSinJang = 0;
            int nBinHyel = 0;
            int nDangNyo = 0;
            double nHeight = 0;
            double nWeight = 0;
            double nGan1 = 0;
            double nGan2 = 0.0;

            string strA154Result = string.Empty;
            string strA131 = string.Empty;
            string strA132 = string.Empty;
            string[] strT_STAT = new string[9];
            string strSogen = string.Empty;
            string strSogenB = string.Empty;
            string strSogenC = string.Empty;
            string strSogenD = string.Empty;
            string strPanjengB = string.Empty;
            string strPanjengC = string.Empty;
            string strPanjengR = string.Empty;
            string strPanjengU = string.Empty;
            string strGojichk = string.Empty;

            clsHcVariable.Gstr_PanR_Etc = "";

            if (comHpcLibBService.SunapDtlChk(ArgWRTNO, "1160").To<string>("").Trim() != "")
            {
                strGojichk = "OK";
            }

            IList<EXAM_DISPLAY_NEW> list = examDisplayNewService.GetItemsInResultExCode(ArgWRTNO);

            if (list.Count == 0)
            {
                return;
            }

            clsHcType.HcMain_First_AutoPanjeng_Clear();


            for (int i = 0; i < list.Count; i++)
            {
                strExCode = list[i].EXCODE;
                strResult = list[i].RESULT;

                strExPan = ExCode_Result_Panjeng(strExCode, strResult, ArgSex, ArgJepDate, ArgYear);

                if (strResult != null && strResult != "")
                {
                    if (strExCode.Equals("A103"))
                    {
                        strBiman = strResult;
                    }
                    //폐결핵 및 기타 흉부질환 판정
                    else if (strExCode.Equals("A142") || strExCode.Equals("A141"))
                    {
                        switch (strResult.Trim())
                        {
                            case "01": clsHcType.TFA.PanR[0] = false; break;
                            case "02": clsHcType.TFA.PanR[0] = false; break;
                            case "03": clsHcType.TFA.PanR[0] = false; break;
                            case "04": clsHcType.TFA.PanR[0] = true; break;
                            case "05": clsHcType.TFA.PanR[0] = true; break;
                            case "06": clsHcType.TFA.PanR[0] = true; break;
                            case "07": clsHcType.TFA.PanR[0] = true; break;
                            case "08": clsHcType.TFA.PanR[0] = false; clsHcType.TFA.PanR[1] = true; break;
                            case "09": clsHcType.TFA.PanR[0] = false; clsHcType.TFA.PanR[9] = true; clsHcVariable.Gstr_PanR_Etc = "순환기계질환/"; break;
                            case "10": clsHcType.TFA.PanR[0] = false; clsHcType.TFA.PanR[9] = true; break;
                            case "11": clsHcType.TFA.PanR[0] = false; break;
                            case "13": clsHcType.TFA.PanR[0] = false; clsHcType.TFA.PanB[9] = true; break;
                            default:
                                clsHcType.TFA.PanR[0] = false;
                                break;
                        }
                    }
                    //흉부XRAY판정
                    else if (strExCode.Equals("A154"))
                    {
                        strA154Result = strResult;
                    }
                    //고혈압(최고)
                    else if (strExCode.Equals("A108"))
                    {
                        nHyelH = Convert.ToInt16(strResult);
                    }
                    //고혈압(최저)
                    else if (strExCode.Equals("A109"))
                    {
                        nHyelL = Convert.ToInt16(strResult);
                    }
                    //총콜레스테롤
                    else if (strExCode.Equals("A123") || strExCode.Equals("A242") || strExCode.Equals("A241") || strExCode.Equals("C404"))
                    {
                        if (strExPan.Equals("B") && nGoJi == 0 && !strGojichk.Equals(""))
                        {
                            nGoJi = 1;
                        }
                        else if (strExPan.Equals("R") && !strGojichk.Equals(""))
                        {
                            nGoJi = 2;
                        }
                    }
                    //간장질환(GOT,GPT,GGT)
                    else if (strExCode.Equals("A124") || strExCode.Equals("A125") || strExCode.Equals("A126"))
                    {
                        if (strExPan.Equals("B") && nLiver == 0)
                        {
                            nLiver = 1;
                        }
                        else if (strExPan.Equals("R"))
                        {
                            nLiver = 2;
                        }
                    }
                    //신장질환(요단백, GFR, 크레아티닌)
                    else if (strExCode.Equals("A112") || strExCode.Equals("A116") || strExCode.Equals("A274") || strExCode.Equals("LU48"))
                    {
                        if (strExPan.Equals("B") && nSinJang == 0)
                        {
                            nSinJang = 1;
                        }
                        else if (strExPan.Equals("R"))
                        {
                            nSinJang = 2;
                        }
                    }
                    //빈혈증질환(2004.6.1혈구용적치는 제외)
                    else if (strExCode.Equals("A121"))      //혈색소
                    {
                        if (strResult.To<double>() < 16.5)
                        {
                            if (strExPan.Equals("B") && nBinHyel == 0)
                            {
                                nBinHyel = 1;
                            }
                            else if (strExPan.Equals("R"))
                            {
                                nBinHyel = 2;
                            }
                        }
                    }
                    //청력
                    else if (strExCode.Equals("A106") || strExCode.Equals("A107"))
                    {
                        if (strExPan.Equals("R"))
                        {
                            clsHcType.TFA.PanR[11] = true;
                        }
                    }
                    //당뇨질환
                    else if (strExCode.Equals("A111") || strExCode.Equals("A122") || strExCode.Equals("LU48"))      //요당
                    {
                        if (strExPan.Equals("B") && nDangNyo == 0)
                        {
                            if (Convert.ToInt16(strResult) >= 100 && Convert.ToInt16(strResult) <= 109)
                            {
                                nDangNyo = 1;
                            }
                            else if (Convert.ToInt16(strResult) >= 110 && Convert.ToInt16(strResult) <= 125)
                            {
                                nDangNyo = 2;
                            }

                        }
                        else if (strExPan.Equals("R"))
                        {
                            nDangNyo = 3;
                        }
                    }
                    //키
                    else if (strExCode.Equals("A101"))
                    {
                        nHeight = strResult.To<double>();
                    }
                    //몸무게
                    else if (strExCode.Equals("A102"))
                    {
                        nWeight = strResult.To<double>();
                    }
                    //체질량지수
                    else if (strExCode.Equals("A117"))
                    {
                        if (strResult.To<double>() >= 18.5)
                        {
                            if (strExPan.Equals("B") && clsHcType.TFA.PanR[10] == false)
                            {
                                clsHcType.TFA.PanB[0] = true;
                            }
                            else if (strExPan.Equals("R"))
                            {
                                clsHcType.TFA.PanB[0] = false;
                                clsHcType.TFA.PanR[10] = true;

                            }
                        }
                    }
                    //허리둘레
                    else if (strExCode.Equals("A115"))
                    {
                        if (strExPan.Equals("B") && clsHcType.TFA.PanR[10] == false)
                        {
                            clsHcType.TFA.PanB[0] = true;
                        }
                        else if (strExPan.Equals("R"))
                        {
                            clsHcType.TFA.PanB[0] = false;
                            clsHcType.TFA.PanR[10] = true;
                        }
                    }
                    //간염검사
                    else if (strExCode.Equals("A258"))      //간염항원
                    {
                        nGan1 = strResult.To<double>();
                    }
                    else if (strExCode.Equals("A259"))      //간염항체
                    {
                        nGan2 = VB.Replace(strResult, ">", "").To<double>();
                    }
                    else if (strExCode.Equals("A131"))
                    {
                        strA131 = strResult;
                    }
                    else if (strExCode.Equals("A132"))
                    {
                        strA132 = strResult;
                    }
                    else if (strExCode.Equals("TX07"))
                    {
                        if (strExPan.Equals("B"))
                        {
                            clsHcType.TFA.PanB[7] = true;
                        }
                        else if (strExPan.Equals("R"))
                        {
                            clsHcType.TFA.PanR[8] = true;
                        }
                    }
                    //시력이 9.9인경우 시력장애 표시
                    else if (strExCode.Equals("A104") || strExCode.Equals("A105"))
                    {
                        if (VB.Left(strResult, 1) == "(")
                        {
                            strResult = VB.STRCUT(strResult, "(", ")");
                        }

                        if (Convert.ToDouble(strResult) == 9.9)
                        {
                            clsHcType.TFA.PanR[9] = true;

                            if (clsHcVariable.Gstr_PanR_Etc.IsNullOrEmpty() || clsHcVariable.Gstr_PanR_Etc.Equals(""))
                            {
                                clsHcVariable.Gstr_PanR_Etc = "시력장애";
                            }
                            else
                            {
                                clsHcVariable.Gstr_PanR_Etc = clsHcVariable.Gstr_PanR_Etc + ",시력장애";
                            }
                        }
                    }
                }

            }

            //D 판정 Check
            if (ArgGbHea != "Y")
            {
                HIC_SANGDAM_NEW item = hicSangdamNewService.SelOneData(ArgWRTNO);

                if (item != null)
                {
                    //고혈압
                    strT_STAT[1] = item.T_STAT21;
                    strT_STAT[2] = item.T_STAT22;
                    //당뇨
                    strT_STAT[3] = item.T_STAT31;
                    strT_STAT[4] = item.T_STAT32;
                    //이상지질혈증
                    strT_STAT[5] = item.T_STAT41;
                    strT_STAT[6] = item.T_STAT42;
                    //폐결핵
                    strT_STAT[7] = item.T_STAT71;
                    strT_STAT[8] = item.T_STAT72;

                    Dictionary<string, object> RESB = comHpcLibBService.SelMunjin(ArgWRTNO);

                    //고혈압
                    if(!strT_STAT[1].IsNullOrEmpty())
                    {
                        if (strT_STAT[1].Equals("2")) { strT_STAT[1] = RESB["T_STAT21"].ToString().Trim(); }
                    }
                    if(!strT_STAT[2].IsNullOrEmpty())
                    {
                        if (strT_STAT[2].Equals("2")) { strT_STAT[2] = RESB["T_STAT22"].ToString().Trim(); }
                    }

                    //당뇨
                    if (!strT_STAT[3].IsNullOrEmpty())
                    {
                        if (strT_STAT[3].Equals("2")) { strT_STAT[3] = RESB["T_STAT31"].ToString().Trim(); }
                    }
                    if (!strT_STAT[4].IsNullOrEmpty())
                    {
                        if (strT_STAT[4].Equals("2")) { strT_STAT[4] = RESB["T_STAT32"].ToString().Trim(); }
                    }
                    //이상지질혈증
                    if (!strT_STAT[5].IsNullOrEmpty())
                    {
                        if (strT_STAT[5].Equals("2")) { strT_STAT[5] = RESB["T_STAT41"].ToString().Trim(); }
                    }
                    if (!strT_STAT[6].IsNullOrEmpty())
                    {
                        if (strT_STAT[6].Equals("2")) { strT_STAT[6] = RESB["T_STAT42"].ToString().Trim(); }
                    }
                    //폐결핵
                    if (!strT_STAT[7].IsNullOrEmpty())
                    {
                        if (strT_STAT[7].Equals("2")) { strT_STAT[7] = RESB["T_STAT61"].ToString().Trim(); }
                    }
                    if (!strT_STAT[8].IsNullOrEmpty())
                    {
                        if (strT_STAT[8].Equals("2")) { strT_STAT[8] = RESB["T_STAT62"].ToString().Trim(); }
                    }


                    if (!strT_STAT[1].IsNullOrEmpty() &&  !strT_STAT[2].IsNullOrEmpty())
                    {
                        if (strT_STAT[1].Equals("1") && strT_STAT[2].Equals("1"))
                        {
                            //고혈압
                            clsHcType.TFA.PanB[1] = false; clsHcType.TFA.PanR[2] = false; clsHcType.TFA.PanU[0] = true;
                        }
                    }

                    if (!strT_STAT[3].IsNullOrEmpty() && !strT_STAT[4].IsNullOrEmpty())
                    {
                        if (strT_STAT[3].Equals("1") && strT_STAT[4].Equals("1"))
                        {
                            //당뇨
                            clsHcType.TFA.PanB[4] = false; clsHcType.TFA.PanR[5] = false; clsHcType.TFA.PanU[1] = true;
                        }
                    }
                    if (!strT_STAT[5].IsNullOrEmpty() && !strT_STAT[6].IsNullOrEmpty())
                    {
                        if (strT_STAT[5].Equals("1") && strT_STAT[6].Equals("1"))
                        {
                            //이상지질혈증
                            clsHcType.TFA.PanB[2] = false; clsHcType.TFA.PanR[3] = false; clsHcType.TFA.PanU[2] = true;
                        }
                    }
                    if (!strT_STAT[7].IsNullOrEmpty() && !strT_STAT[8].IsNullOrEmpty())
                    {
                        if (strT_STAT[7].Equals("1") && strT_STAT[8].Equals("1"))
                        {
                            //폐결핵
                            clsHcType.TFA.PanR[0] = false; clsHcType.TFA.PanU[3] = true;
                        }
                    }
                }
            }

            if (clsHcType.TFA.PanU[0] == true) { clsHcType.TFA.PanB[1] = false; clsHcType.TFA.PanR[2] = false; }
            if (clsHcType.TFA.PanU[1] == true) { clsHcType.TFA.PanB[4] = false; clsHcType.TFA.PanR[5] = false; }
            if (clsHcType.TFA.PanU[2] == true) { clsHcType.TFA.PanB[2] = false; clsHcType.TFA.PanR[3] = false; }
            if (clsHcType.TFA.PanU[3] == true) { clsHcType.TFA.PanR[0] = false; }

            //고혈압판정
            if (nHyelH >= 140 || nHyelL >= 90)
            {
                if (clsHcType.TFA.PanU[0] == false)
                {
                    clsHcType.TFA.PanB[1] = false; clsHcType.TFA.PanR[2] = true;
                }
            }
            else if ((nHyelH >= 130 && nHyelH <= 139) || (nHyelL >= 85 && nHyelL <= 89))
            {
                if (clsHcType.TFA.PanU[0] == false)
                {
                    clsHcType.TFA.PanB[1] = true; clsHcType.TFA.PanR[2] = false;
                    strSogenD = strSogenD + "혈압관리:저염식이,운동/ ";
                }
            }
            else if ((nHyelH >= 120 && nHyelH <= 129) || (nHyelL >= 80 && nHyelL <= 84))
            {
                if (clsHcType.TFA.PanU[0] == false)
                {
                    clsHcType.TFA.PanB[1] = true; clsHcType.TFA.PanR[2] = false;
                    strSogenD = strSogenD + "생활습관개선(혈압)/ ";
                }
            }
            else
            {
                clsHcType.TFA.PanB[1] = false;
                clsHcType.TFA.PanR[2] = false;
            }

            //당뇨질환 판정
            if (nDangNyo == 1)
            {
                if (clsHcType.TFA.PanU[1] == false)
                {
                    clsHcType.TFA.PanB[4] = true; clsHcType.TFA.PanR[5] = false;
                    strSogenD = strSogenD + "식이및운동요법(당뇨)/ ";
                }
            }
            else if (nDangNyo == 2)
            {
                if (clsHcType.TFA.PanU[1] == false)
                {
                    clsHcType.TFA.PanB[4] = true; clsHcType.TFA.PanR[5] = false;
                    strSogenD = strSogenD + "당뇨관리:식이요법,운동,혈당 추적검사요/ ";
                }
            }
            else if (nDangNyo == 3)
            {
                if (clsHcType.TFA.PanU[1] == false)
                {
                    clsHcType.TFA.PanB[4] = false; clsHcType.TFA.PanR[5] = true;
                }
            }
            else
            {
                clsHcType.TFA.PanB[4] = false; clsHcType.TFA.PanR[5] = false;
            }

            //이상지혈증 판정
            if (nGoJi == 1)
            {
                if (clsHcType.TFA.PanU[2] == false)
                {
                    clsHcType.TFA.PanB[2] = true; clsHcType.TFA.PanR[3] = false;
                }
            }
            else if (nGoJi == 2)
            {
                if (clsHcType.TFA.PanU[2] == false)
                {
                    clsHcType.TFA.PanB[2] = false; clsHcType.TFA.PanR[3] = true;
                }
            }
            else
            {
                clsHcType.TFA.PanB[2] = false; clsHcType.TFA.PanR[3] = false;
            }

            //간장질환 판정
            if (nLiver == 1)
            {
                clsHcType.TFA.PanB[3] = true; clsHcType.TFA.PanR[4] = false;
            }
            else if (nLiver == 2)
            {
                clsHcType.TFA.PanB[3] = false; clsHcType.TFA.PanR[4] = true;
            }
            else
            {
                clsHcType.TFA.PanB[3] = false; clsHcType.TFA.PanR[4] = false;
            }

            //신장질환 판정
            if (nSinJang == 1)
            {
                clsHcType.TFA.PanB[5] = true; clsHcType.TFA.PanR[6] = false;
            }
            else if (nSinJang == 2)
            {
                clsHcType.TFA.PanB[5] = false; clsHcType.TFA.PanR[6] = true;
            }
            else
            {
                clsHcType.TFA.PanB[5] = false; clsHcType.TFA.PanR[6] = false;
            }

            //빈혈증 판정
            if (nBinHyel == 1)
            {
                clsHcType.TFA.PanB[6] = true; clsHcType.TFA.PanR[7] = false;
            }
            else if (nBinHyel == 2)
            {
                clsHcType.TFA.PanB[6] = false; clsHcType.TFA.PanR[7] = true;
            }
            else
            {
                clsHcType.TFA.PanB[6] = false; clsHcType.TFA.PanR[7] = false;
            }

            //간염판정
            clsHcType.TFA.Liver = 0;
            if (strA132.Equals("02"))
            {
                clsHcType.TFA.Liver = 1;    //면역자
            }
            else if (strA132.Equals("01"))
            {
                if (strA131.Equals("01")) { clsHcType.TFA.Liver = 3; }
                if (strA131.Equals("02")) { clsHcType.TFA.Liver = 1; }
            }
            else if (nGan1 > 0 && nGan2 >= 0.0)
            {
                if (nGan1 == 1 && nGan2 >= 10.0)
                {
                    clsHcType.TFA.Liver = 1;
                }
                else if (nGan1 == 1 && nGan2 < 10.0)
                {
                    clsHcType.TFA.Liver = 2;
                }
                else if (nGan1 == 2 && nGan2 < 10.0)
                {
                    clsHcType.TFA.Liver = 3;
                }
                else if (nGan1 == 2 && nGan2 >= 10.0)
                {
                    clsHcType.TFA.Liver = 4;
                    //2019년 변경사항
                    if (clsHcVariable.Gstr_PanR_Etc.Equals(""))
                    {
                        clsHcVariable.Gstr_PanR_Etc = "간염판정보류";
                    }
                    else
                    {
                        clsHcVariable.Gstr_PanR_Etc = clsHcVariable.Gstr_PanR_Etc + ",간염판정보류";
                    }
                }
            }

            //간염자동판정 제외 대상자 (고도화에서는 11종에서 다른 검사를 추가로 넣지 않기로 하여 해당로직은 적용안함)

            #region 일반검진이 아닌 경우 간염자동판정 대상에서 제외하는 로직
            //SQL = " SELECT WRTNO, GROUPCODE, EXCODE FROM HIC_RESULT WHERE WRTNO = " & ArgWRTNO & " "
            //SQL = SQL & " AND EXCODE IN ('A131','A132')"
            //Call AdoOpenSet(rs1, SQL)
            //    If RowIndicator >= 1 Then
            //        strGroupCode = Trim(AdoGetString(rs1, "GROUPCODE", 0))


            //        SQL = " SELECT WRTNO, CODE FROM HIC_SUNAPDTL WHERE WRTNO = " & ArgWRTNO & " "
            //        SQL = SQL & " AND CODE IN ('9026','" & strGroupCode & "') "
            //        SQL = SQL & " AND GBSELF NOT IN ('1','01') "
            //        Call AdoOpenSet(rs2, SQL)
            //            If RowIndicator = 1 Then
            //                TFA.Liver = 0
            //            End If
            //        Call AdoCloseSet(rs2)
            //    End If
            //Call AdoCloseSet(rs1) 
            #endregion

            //Dictionary<string, object> RESB1 = comHpcLibBService.SelMunjin(ArgWRTNO);
            //if ((string)RESB1["T66_INJECT"] == "2") { strSogenD = strSogenD + "인플루엔자 접종필요/"; }
            //if ((string)RESB1["TMUN0013"] == "2") { strSogenD = strSogenD + "폐렴 예방접종필요/"; }
            //if ((string)RESB1["T66_FALL"] == "1") { strSogenD = strSogenD + "낙상주의"; }

            HIC_RES_BOHUM1 RESB1 = hicResBohum1Service.GetItemByWrtno(ArgWRTNO);

            if (!RESB1.IsNullOrEmpty())
            {
                if (RESB1.T66_INJECT == "2") { strSogenD += "인플루엔자 접종필요/"; }
                if (RESB1.TMUN0013 == "2") { strSogenD += "폐렴 예방접종필요/"; }
                if (RESB1.T66_FALL == "1") { strSogenD += "낙상주의"; }
            }

            clsHcType.TFA.Panjeng = "";

            for (int i = 0; i < 10; i++) { if (strPanjengB.Equals("") && clsHcType.TFA.PanB[i] == true) { strPanjengB = "B"; } }    //정상 B
            for (int i = 0; i < 12; i++) { if (strPanjengR.Equals("") && clsHcType.TFA.PanR[i] == true) { strPanjengR = "R"; } }    //의심R
            for (int i = 0; i < 4; i++) { if (strPanjengU.Equals("") && clsHcType.TFA.PanU[i] == true) { strPanjengU = "U"; } }    //유질환U
            if (clsHcType.TFA.PanR[2] == true || clsHcType.TFA.PanR[5] == true) { strPanjengR = "R2"; }                             //재검R2

            if (strPanjengB.Equals("") && strPanjengR.Equals("") && strPanjengU.Equals(""))
            {
                clsHcType.TFA.Panjeng = "1";
            }
            else if (strPanjengB.Equals("B") && strPanjengR.Equals("") && strPanjengU.Equals(""))
            {
                clsHcType.TFA.Panjeng = "2";
            }
            else if (strPanjengR.Equals("R") && strPanjengU.Equals(""))
            {
                clsHcType.TFA.Panjeng = "4";
            }
            else if (strPanjengU.Equals("U") && strPanjengR != "R2")
            {
                clsHcType.TFA.Panjeng = "8";
            }
            else if (strPanjengR == "R2")
            {
                clsHcType.TFA.Panjeng = "5";
            }

            clsHcType.TFA.Sogen = strSogen;
            clsHcType.TFA.SogenB = strSogenB;
            clsHcType.TFA.SogenC = strSogenC;
            clsHcType.TFA.SogenD = strSogenD;
        }

        public long Read_Auto_WORK(long nWRTNO)
        {
            long rtnVal = 0;

            int nREAD = 0;
            long nResult = 0;
            long nResult1 = 0;
            long nResult2 = 0;
            long nResult3 = 0;
            long nResult4 = 0;
            long nResult5 = 0;
            long nResult6 = 0;

            List<HIC_TITEM> list = hicTitemService.GetItembyWrtNoGubunJumsu(nWRTNO);

            nREAD = list.Count;

            if (nREAD > 0)
            {
                for (int i = 0; i <= 5; i++)
                {
                    if (!list[i].CODE.IsNullOrEmpty())
                    {
                        if (list[i].JUMSU == 901) { nResult1 = list[i].CODE.To<long>(); }    //고강도 일수
                        if (list[i].JUMSU == 902) { nResult2 = list[i].CODE.To<long>(); }    //고강도 시간(분) 1시간(60)
                        if (list[i].JUMSU == 903) { nResult3 = list[i].CODE.To<long>(); }    //고강도 분
                        if (list[i].JUMSU == 904) { nResult4 = list[i].CODE.To<long>(); }
                        if (list[i].JUMSU == 905) { nResult5 = list[i].CODE.To<long>(); }
                        if (list[i].JUMSU == 906) { nResult6 = list[i].CODE.To<long>(); }
                    }
                }
            }

            //(고강도 신체활동 주당 횟수×시간(분)×2) + (중강도 신체활동 주당 횟수×시간(분))
            nResult = (nResult1 * ((nResult2 * 60) + nResult3) * 2) + (nResult4 * ((nResult5 * 60) + nResult6));

            rtnVal = nResult;

            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="argBDate"></param>
        /// <param name="strDrug"></param>
        /// <param name="v"></param>
        /// <param name="p"></param>
        /// <param name="strDRSABUN"></param>
        public void Insert_OCS_Mayak(long argWrtNo, string argPtNo, string argBDate, string argDrug, string argDC, double argHyangQty, string argDRSABUN, string strGubun, string strDeptCode)
        {
            double nQty = 0;
            double nSQty = 0;
            double nTotQty = 0;
            double nRealQty = 0;
            string strOK = "";
            string strROWID = "";

            string strSname = "";
            string strSex = "";
            string strPtNo = "";
            string strJuso = "";
            string strJumin = "";
            string strJong = "";
            string strDRSABUN = "";
            long nAge = 0;
            int result = 0;

            strDRSABUN = argDRSABUN.Trim();
            if (strDRSABUN.IsNullOrEmpty()) { strDRSABUN = "32158"; }

            //향정/마약처방 조회
            strROWID = comHpcLibBService.GetRowIdOcsMayakbyPtNoSuCodeBDate(argPtNo, argDrug, argBDate);

            if (argDC != "D")
            {
                //해당 접수번호의 수면 내시경오더가 있는지 점검
                HEA_JEPSU_PATIENT list = heaJepsuPatientService.GetItembyWrtNo(argWrtNo);
                if (!list.IsNullOrEmpty())
                {
                    //MessageBox.Show("종검 접수내역이 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //return;
                    strPtNo = list.PTNO;
                    strSname = list.SNAME;
                    nAge = list.AGE;
                    strSex = list.SEX;
                    strJumin = clsAES.DeAES(list.JUMIN2);
                    strJuso = list.JUSO1 + list.JUSO2;


                    nTotQty = 0; nRealQty = 0;
                    nQty = argHyangQty;
                    nSQty = 0;
                    nRealQty = 1;

                    if (!strROWID.IsNullOrEmpty())
                    {
                        result = comHpcLibBService.UpdateOcsMayak(strDeptCode, strDRSABUN, nQty, nSQty, nRealQty, strROWID);

                        if (result < 0)
                        {
                            MessageBox.Show("향정처방 Data UPDATE 도중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        result = comHpcLibBService.InsertOcsMayak(strPtNo, strSname, "51", "TO", "TO",
                                                                  argDRSABUN, "O", argDrug, nRealQty.ToString(), nRealQty, 1, "920103",
                                                                  "검사용", "Pain", strSex, nAge, VB.Left(strJumin, 7) + "******", clsAES.AES(strJumin), strJuso, nQty, argBDate);
                        if (result < 0)
                        {
                            MessageBox.Show("향정약품 처방전 전송시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                else
                {
                    if (!strROWID.IsNullOrEmpty())
                    {
                        result = comHpcLibBService.InsertOcsMayakSelect(strROWID);

                        if (result < 0)
                        {
                            MessageBox.Show("향정처방 Data 삭제 도중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                if(strPtNo.IsNullOrEmpty())
                {
                    HIC_JEPSU_PATIENT list1 = hicJepsuPatientService.GetEndoItembyWrtNo(argWrtNo);
                    if (!list1.IsNullOrEmpty())
                    {
                        //MessageBox.Show("종검 접수내역이 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //return;
                        strPtNo = list1.PTNO;
                        strSname = list1.SNAME;
                        nAge = list1.AGE;
                        strSex = list1.SEX;
                        strJumin = clsAES.DeAES(list1.JUMIN2);
                        strJuso = list1.JUSO1 + list1.JUSO2;

                        nTotQty = 0; nRealQty = 0;
                        nQty = argHyangQty;
                        nSQty = 0;
                        nRealQty = 1;

                        if (!strROWID.IsNullOrEmpty())
                        {
                            result = comHpcLibBService.UpdateOcsMayak("HR", strDRSABUN, nQty, nSQty, nRealQty, strROWID);

                            if (result < 0)
                            {
                                MessageBox.Show("향정처방 Data UPDATE 도중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            result = comHpcLibBService.InsertOcsMayak(strPtNo, strSname, "51", "HR", "HR",
                                                                      argDRSABUN, "O", argDrug, nRealQty.ToString(), nRealQty, 1, "920103",
                                                                      "검사용", "Pain", strSex, nAge, VB.Left(strJumin, 7) + "******", clsAES.AES(strJumin), strJuso, nQty, argBDate);
                            if (result < 0)
                            {
                                MessageBox.Show("향정약품 처방전 전송시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (strROWID != "")
                        {
                            result = comHpcLibBService.InsertOcsMayakSelect(strROWID);

                            if (result < 0)
                            {
                                MessageBox.Show("향정처방 Data 삭제 도중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }



            }
        }

        public bool fn_PanjengYN(long argWrtNo)
        {
            bool rtnVal = false;

            long nDrNo = hicJepsuService.GetPanjengDrNobyWrtNo(argWrtNo);

            if (nDrNo > 0)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        public bool fn_PanjengYN(List<long> argWrtNo)
        {
            bool rtnVal = false;

            string strRid = hicJepsuService.GetPanjengDrNobyWrtNo(argWrtNo);

            if (strRid.IsNullOrEmpty())
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        public void Insert_Ocs_Hyang(long argWrtNo, string argPtNo, string argBDate, string argDrug, string argDC, double argHyangQty, string argDRSABUN, string strGubun, string strDeptCode)
        {
            double nQty = 0;
            double nSQty = 0;
            double nTotQty = 0;
            double nRealQty = 0;
            string strOK = "";
            string strROWID = "";
            string strROWID1 = "";

            string strSname = "";
            string strSex = "";
            string strPtNo = "";
            string strJuso = "";
            string strJumin = "";
            string strJong = "";
            string strDRSABUN = "";
            long nAge = 0;
            int result = 0;

            strDRSABUN = argDRSABUN.Trim();

            strROWID = "";
            strROWID1 = "";

            //향정/마약처방 조회
            strROWID = comHpcLibBService.GetRowIdOcsHyangbyPtNoSuCodeBDate(argPtNo, argDrug, argBDate);

            if (argDC != "D")
            {
                //해당 접수번호의 수면 내시경오더가 있는지 점검


                HEA_JEPSU_PATIENT list = heaJepsuPatientService.GetItembyWrtNo(argWrtNo);
                if (!list.IsNullOrEmpty())
                {
                    strPtNo = list.PTNO;
                    strSname = list.SNAME;
                    nAge = list.AGE;
                    strSex = list.SEX;
                    strJumin = clsAES.DeAES(list.JUMIN2);
                    strJuso = list.JUSO1 + list.JUSO2;
                    strJong = "1";

                    nTotQty = 0;
                    nQty = argHyangQty;
                    nRealQty = 1;

                    if (!strROWID.IsNullOrEmpty())
                    {
                        //result = comHpcLibBService.UpdateOcsHyang("TO", strDRSABUN, nQty, nSQty, nRealQty, strROWID);
                        result = comHpcLibBService.UpdateOcsHyang(strDeptCode, strDRSABUN, nQty, nSQty, nRealQty, strROWID);
                        

                        if (result < 0)
                        {
                            MessageBox.Show("향정처방 Data UPDATE 도중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        //result = comHpcLibBService.InsertOcsHyang(strPtNo, strSname, "51", "TO", argBDate, "TO",
                        //                                          argDRSABUN, "O", argDrug, nQty.To<string>(), nRealQty, 1, "920103",
                        //                                          "검사용", "Pain", strSex, nAge, VB.Left(strJumin, 7) + "******", clsAES.AES(strJumin), strJuso, nQty);
                        result = comHpcLibBService.InsertOcsHyang(strPtNo, strSname, "51", strDeptCode, argBDate, strDeptCode,
                                                                  argDRSABUN, "O", argDrug, nQty.To<string>(), nRealQty, 1, "920103",
                                                                  "검사용", "Pain", strSex, nAge, VB.Left(strJumin, 7) + "******", clsAES.AES(strJumin), strJuso, nQty);

                        if (result < 0)
                        {
                            MessageBox.Show("향정약품 처방전 전송시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                else
                {
                    if (strROWID != "")
                    {
                        result = comHpcLibBService.InsertOcsHyangSelect(strROWID);

                        if (result < 0)
                        {
                            MessageBox.Show("향정처방 Data 삭제 도중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                if (strPtNo.IsNullOrEmpty())
                {
                    HIC_JEPSU_PATIENT list1 = hicJepsuPatientService.GetEndoItembyWrtNo(argWrtNo);

                    if (!list1.IsNullOrEmpty())
                    {
                        strPtNo = list1.PTNO;
                        strSname = list1.SNAME;
                        nAge = list1.AGE;
                        strSex = list1.SEX;
                        strJumin = clsAES.DeAES(list1.JUMIN2);
                        strJuso = list1.JUSO1 + list1.JUSO2;
                        strJong = "1";

                        nTotQty = 0;
                        nQty = argHyangQty;
                        nRealQty = 1;

                        if (!strROWID.IsNullOrEmpty())
                        {
                            result = comHpcLibBService.UpdateOcsHyang("HR", strDRSABUN, nQty, nSQty, nRealQty, strROWID);

                            if (result < 0)
                            {
                                MessageBox.Show("향정처방 Data UPDATE 도중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            result = comHpcLibBService.InsertOcsHyang(strPtNo, strSname, "51", "HR", argBDate, "HR",
                                                                      argDRSABUN, "O", argDrug, nQty.To<string>(), nRealQty, 1, "920103",
                                                                      "검사용", "Pain", strSex, nAge, VB.Left(strJumin, 7) + "******", clsAES.AES(strJumin), strJuso, nQty);

                            if (result < 0)
                            {
                                MessageBox.Show("향정약품 처방전 전송시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (strROWID != "")
                        {
                            result = comHpcLibBService.InsertOcsHyangSelect(strROWID);

                            if (result < 0)
                            {
                                MessageBox.Show("향정처방 Data 삭제 도중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 검사항목별 참고치 판정 NEW
        /// </summary>
        /// <param name="argExCode">검사항목코드</param>
        /// <param name="argResult">검사결과</param>
        /// <param name="argSex">성별</param>
        /// <param name="argJepDate">검진일자</param>
        /// <param name="argYear">검진년도</param>
        /// <returns></returns>
        /// <comment>하드코딩으로 세팅하던 부분을 Table에 년도별로 관리함 HIC_EXCODE_REFER 테이블 이용
        ///             참고치 관련 테이블 생성후 적용시킬것
        /// </comment>
        public string ExCode_Result_Panjeng_New(string argExCode, string argResult, string argSex, string argJepDate, string argYear)
        {
            string rtnVal = string.Empty;
            double nMin = 0.0;
            double nMin_B = 0.0;
            double nMin_R = 0.0;
            double nMax = 0.0;
            double nMax_B = 0.0;
            double nMax_R = 0.0;

            //결과가 공란이면 판정을 안함
            if (string.IsNullOrEmpty(argResult)) { return ""; }
            //2014-07-16 [H827.혈중카르복시헤모글로빈] 미실시이면 정상B로 판정함(판정창에 상단에 표시하기 위함)
            if (argExCode.Equals("H827") && argResult.Equals("미실시")) { return "B"; }
            //2014-08-25 TZ46 니코틴소변검사 양성이면 R로 표시
            if (argExCode.Equals("TZ46") && argResult.ToLower().Equals("positive")) { return "R"; }
            if (argResult.Equals(".")) { return ""; }
            //2014-09-04 검사미실시는 "R"로 처리
            if (argResult.Equals("미실시")) { return "R"; }

            #region 기존 하드코딩되어 있던 부분은 적용시키지 않고 코드화 작업함
            ////2014-09-12 마약 양성일 경우 "R"로 표시
            //if (argResult.ToLower().Equals("positive"))
            //{
            //    if (argExCode.Equals("E922")) { return "R"; } //필로폰
            //    if (argExCode.Equals("E924")) { return "R"; } //코카인
            //    if (argExCode.Equals("E925")) { return "R"; } //아편
            //    if (argExCode.Equals("E926")) { return "R"; } //대마
            //}
            ////2019-03-13(QuantiFERON-TB)
            //if (argExCode.Equals("E935") && argResult.Equals("양성")) { return "R"; }

            ////2014-09-18 흉부촬영결과가 심비대이면 "R"로 표시
            //if (argExCode.Equals("A154") && VB.InStr(argResult, "심비대") > 0) { return "R"; }

            ////2015-02-06 장티푸스검사
            //if (argExCode.Equals("A285"))
            //{
            //    if (string.Compare(argResult, "1:40") >= 0) { return "B"; }
            //}

            ////2015-04-06 객담세포학적검사
            //if (argExCode.Equals("LM10"))
            //{
            //    if (string.Compare(argResult, "02") >= 0) { return "B"; }
            //}

            ////정상은 정상으로 처리
            //if (argResult.Equals("정상")) { return ""; }

            ////이경검사 정상
            //if (argExCode.Equals("TI01") && VB.Left(argResult, 2).Equals("01")) { return ""; }
            //if (argExCode.Equals("TI02") && VB.Left(argResult, 2).Equals("01")) { return ""; }

            ////2016-02-10 백혈구 4,000이하 또는 11,000이상 R
            //if (argExCode.Equals("A282"))
            //{
            //    if (Convert.ToInt32(argResult) > 0 && Convert.ToInt32(argResult) <= 4000) { return "R"; }
            //    if (Convert.ToInt32(argResult) >= 11000) { return "R"; }
            //}

            ////2016-02-10 당화혈색소 6.5이상 R
            //if (argExCode.Equals("ZE22"))
            //{
            //    if (Convert.ToDouble(argResult) >= 6.5) { return "R"; }
            //}

            ////2016-03-02 요단백 미실시는 정상으로 표시
            //if (argExCode.Equals("A112") && argResult.Equals("99")) { return ""; }

            ////2016-03-07 증상문진 비정상은  R로 표시
            //if (argResult.Equals("비정상"))
            //{
            //    if (argExCode.Equals("TZ72") || argExCode.Equals("TZ85")) { return "R"; }
            //    if (argExCode.Equals("TZ86") || argExCode.Equals("TZ87")) { return "R"; }
            //}

            ////2016-05-10 B형간염항체 음성 또는 10미만은 R로(남복동과장님)
            //if (argExCode.Equals("A259"))
            //{
            //    if (argResult.Equals("음성")) { return "R"; }
            //    //>821.0
            //    if (VB.Left(argResult, 1).Equals(">")) { return ""; }
            //    if (argResult != "")
            //    {
            //        if (Convert.ToInt16(argResult) >= 10) { return ""; }
            //        if (Convert.ToInt16(argResult) < 10) { return "R"; }
            //    }
            //}

            ////판정여부(최초판정 상태는 정상으로 세팅)
            //bool BoolPanjeng = true;
            //switch (argExCode)
            //{
            //    case "A101":
            //    case "A102": BoolPanjeng = false; break;    //신장/체중
            //    case "ZD00":
            //    case "A214": BoolPanjeng = false; break;    //치아검사,방사선관촬번호
            //    case "ZD01": BoolPanjeng = false; break;    //생애구강검사
            //    case "H840":
            //    case "H841": BoolPanjeng = false; break;    //혈액형(ABO/RH)
            //    case "A215": BoolPanjeng = false; break;    //방사선직촬번호
            //    default: break;
            //}
            //if (BoolPanjeng == false) { return ""; } 
            #endregion

            double nResult = VB.Val(argResult);

            //2016-07-29 시력검사 괄호 제거
            if (argExCode.Equals("A104") || argExCode.Equals("A105"))
            {
                if (VB.Left(argResult, 1).Equals("(") && VB.Right(argResult, 1).Equals(")"))
                {
                    nResult = VB.Val(VB.STRCUT(argResult, "(", ")"));
                }
            }

            //검사항목의 정상치,정상B,질환의심 값을 READ
            HIC_EXCODE item = hicExCodeservice.FindOne(argExCode);

            if (argSex.Equals("M")) //남자
            {
                nMin = VB.Val(item.MIN_M);
                nMin_B = VB.Val(item.MIN_MB);
                nMin_R = VB.Val(item.MIN_MR);
                nMax = VB.Val(item.MAX_M);
                nMax_B = VB.Val(item.MAX_MB);
                nMax_R = VB.Val(item.MAX_MR);
            }
            else          //여자
            {
                nMin = VB.Val(item.MIN_F);
                nMin_B = VB.Val(item.MIN_FB);
                nMin_R = VB.Val(item.MIN_FR);
                nMax = VB.Val(item.MAX_F);
                nMax_B = VB.Val(item.MAX_FB);
                nMax_R = VB.Val(item.MAX_FR);
            }

            //코드마스타에 등록된 값으로 검사항목별 판정(B.정상B R.질환의심)
            if (nResult != 0.0)
            {
                if (nResult >= nMin && nResult <= nMax)
                {
                    rtnVal = "";
                }
                else if (nResult >= nMin_B && nResult <= nMax_B)
                {
                    rtnVal = "B";
                }
                else if (nResult >= nMin_R && nResult <= nMax_R)
                {
                    rtnVal = "R";
                }
                else
                {
                    rtnVal = "B";
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 검사항목별 참고치 판정
        /// </summary>
        /// <param name="argExCode">검사항목코드</param>
        /// <param name="argResult">검사결과</param>
        /// <param name="argSex">성별</param>
        /// <param name="argJepDate">검진일자</param>
        /// <param name="argYear">검진년도</param>
        /// <seealso cref="HcMain.bas> ExCode_Result_Panjeng"/>
        /// <returns></returns>
        public string ExCode_Result_Panjeng(string argExCode, string argResult, string argSex, string argJepDate, string argYear)
        {
            string rtnVal = string.Empty;
            double nMin = 0.0;
            double nMin_B = 0.0;
            double nMin_R = 0.0;
            double nMax = 0.0;
            double nMax_B = 0.0;
            double nMax_R = 0.0;

            //결과가 공란이면 판정을 안함
            if (argResult.IsNullOrEmpty()) { return ""; }
            //2014-07-16 [H827.혈중카르복시헤모글로빈] 미실시이면 정상B로 판정함(판정창에 상단에 표시하기 위함)
            if (argExCode.Equals("H827") && argResult.Equals("미실시")) { return "B"; }
            //2014-08-25 TZ46 니코틴소변검사 양성이면 R로 표시
            if (argExCode.Equals("TZ46") && argResult.ToLower().Equals("positive")) { return "R"; }
            if (argResult.Equals(".")) { return ""; }
            //2014-09-04 검사미실시는 "R"로 처리
            if (argResult.Equals("미실시")) { return "R"; }

            //2014-09-12 마약 양성일 경우 "R"로 표시
            if (argResult.ToLower().Equals("positive"))
            {
                if (argExCode.Equals("E922")) { return "R"; } //필로폰
                if (argExCode.Equals("E924")) { return "R"; } //코카인
                if (argExCode.Equals("E925")) { return "R"; } //아편
                if (argExCode.Equals("E926")) { return "R"; } //대마
            }

            //2019-03-13(QuantiFERON-TB)
            if (argExCode.Equals("E935") && argResult.Equals("양성")) { return "R"; }

            //2019-08-28(전현준과장님 요청건으로 인해 추가)
            if (argExCode.Equals("E940") && argResult.Equals("양성")) { return "R"; }

            //2014-09-18 흉부촬영결과가 심비대이면 "R"로 표시
            if (argExCode.Equals("A154") && VB.InStr(argResult, "심비대") > 0) { return "R"; }

            //2015-02-06 장티푸스검사
            if (argExCode.Equals("A285"))
            {
                if (string.Compare(argResult, "1:40") >= 0) { return "B"; }
            }

            //2015-04-06 객담세포학적검사
            if (argExCode.Equals("LM10"))
            {
                if (string.Compare(argResult, "02") >= 0) { return "B"; }
            }

            //정상은 정상으로 처리
            if (argResult.Equals("정상")) { return ""; }

            //이경검사 정상
            if (argExCode.Equals("TI01") && VB.Left(argResult, 2).Equals("01")) { return ""; }
            if (argExCode.Equals("TI02") && VB.Left(argResult, 2).Equals("01")) { return ""; }

            //진동검사 정상
            if (argExCode.Equals("TZ12") && VB.Left(argResult, 2).Equals("01")) { return ""; }
            if (argExCode.Equals("ZE76") && VB.Left(argResult, 2).Equals("01")) { return ""; }
            if (argExCode.Equals("ZE77") && VB.Left(argResult, 2).Equals("01")) { return ""; }

            //2016-02-10 백혈구 4,000이하 또는 11,000이상 R
            if (argExCode.Equals("A282"))
            {
                if (Convert.ToInt32(argResult) > 0 && Convert.ToInt32(argResult) <= 4000) { return "R"; }
                if (Convert.ToInt32(argResult) >= 11000) { return "R"; }
            }

            //2016-02-10 당화혈색소 6.5이상 R
            if (argExCode.Equals("ZE22"))
            {
                if (Convert.ToDouble(argResult) >= 6.5) { return "R"; }
            }

            //2016-03-02 요단백 미실시는 정상으로 표시
            if (argExCode.Equals("A112") && argResult.Equals("99")) { return ""; }

            //2016-03-07 증상문진 비정상은  R로 표시
            if (argResult.Equals("비정상"))
            {
                if (argExCode.Equals("TZ72") || argExCode.Equals("TZ85")) { return "R"; }
                if (argExCode.Equals("TZ86") || argExCode.Equals("TZ87")) { return "R"; }
            }

            //2016-05-10 B형간염항체 음성 또는 10미만은 R로(남복동과장님)
            if (argExCode.Equals("A259"))
            {
                if (argResult.Equals("음성")) { return "R"; }
                //>821.0
                if (VB.Left(argResult, 1).Equals(">")) { return ""; }
                if (argResult != "")
                {
                    if (argResult.To<double>() >= 10) { return ""; }
                    if (argResult.To<double>() < 10) { return "R"; }
                }
            }

            //판정여부(최초판정 상태는 정상으로 세팅)
            bool BoolPanjeng = true;
            switch (argExCode)
            {
                case "A101":
                case "A102": BoolPanjeng = false; break;    //신장/체중
                case "ZD00":
                case "A214": BoolPanjeng = false; break;    //치아검사,방사선관촬번호
                case "ZD01": BoolPanjeng = false; break;    //생애구강검사
                case "H840":
                case "H841": BoolPanjeng = false; break;    //혈액형(ABO/RH)
                case "A215": BoolPanjeng = false; break;    //방사선직촬번호
                default: break;
            }
            if (BoolPanjeng == false) { return ""; }

            //검사항목의 정상치,정상B,질환의심 값을 READ
            HIC_EXCODE item = hicExCodeservice.FindOne(argExCode);

            if (argSex == "M") //남자
            {
                nMin = VB.Val(item.MIN_M);
                nMin_B = VB.Val(item.MIN_MB);
                nMin_R = VB.Val(item.MIN_MR);
                nMax = VB.Val(item.MAX_M);
                nMax_B = VB.Val(item.MAX_MB);
                nMax_R = VB.Val(item.MAX_MR);
            }
            if (argSex == "F")//여자
            {
                nMin = VB.Val(item.MIN_F);
                nMin_B = VB.Val(item.MIN_FB);
                nMin_R = VB.Val(item.MIN_FR);
                nMax = VB.Val(item.MAX_F);
                nMax_B = VB.Val(item.MAX_FB);
                nMax_R = VB.Val(item.MAX_FR);
            }

            double nResult = VB.Val(argResult);

            //크레아티닌 설정은 구현안함(과거기준임)

            //각 검사항목별 판정(B.정상B R.질환의심)
            rtnVal = "";

            //2016-07-29 시력검사 괄호 제거
            if (argExCode.Equals("A104") || argExCode.Equals("A105"))
            {
                if (VB.Left(argResult, 1).Equals("(") && VB.Right(argResult, 1).Equals(")"))
                {
                    nResult = VB.Val(VB.STRCUT(argResult, "(", ")"));
                }
            }

            if (argExCode.Equals("A104") || argExCode.Equals("A105"))   //시력
            {
                if (nResult <= 0.4) { rtnVal = "B"; }
                else if (nResult == 9.9) { rtnVal = "R"; }
                else { rtnVal = " "; }
            }
            else if (argExCode.Equals("A123"))  //총콜레스테롤
            {
                //예전기준 구현안함
            }
            else if (argExCode.Equals("A116"))  //GFR 수치
            {
                if (nResult < 60) { rtnVal = "R"; }
                else { rtnVal = " "; }
            }
            else if (argExCode.Equals("A117"))  //체질량지수
            {
                //기준 없음
            }
            else if (argExCode.Equals("A124"))  //G.O.T
            {
                if (nResult >= 3 && nResult <= 38) { rtnVal = " "; }
                else if (nResult >= 39 && nResult <= 50) { rtnVal = "B"; }
                else if (nResult >= 51) { rtnVal = "R"; }
            }
            else if (argExCode.Equals("A125"))  //G.P.T
            {
                if (nResult >= 3 && nResult <= 43) { rtnVal = " "; }
                else if (nResult >= 44 && nResult <= 50) { rtnVal = "B"; }
                else if (nResult >= 51) { rtnVal = "R"; }
            }
            else if (argExCode.Equals("A126"))  //r-G.T.P
            {
                if (argSex == "M")  //남자
                {
                    if (nResult >= 5 && nResult <= 54) { rtnVal = " "; }
                    else if (nResult >= 55 && nResult <= 77) { rtnVal = "B"; }
                    else if (nResult >= 78) { rtnVal = "R"; }
                }
                else
                {
                    if (nResult >= 5 && nResult <= 37) { rtnVal = " "; }
                    else if (nResult >= 38 && nResult <= 47) { rtnVal = "B"; }
                    else if (nResult >= 48) { rtnVal = "R"; }
                }
            }
            else if (argExCode.Equals("A142") || argExCode.Equals("A141") || argExCode.Equals("A211") || argExCode.Equals("A213"))
            {
                //폐결핵 및 기타 흉부질환 판정
                if (argResult.Trim().Equals("01") || argResult.Trim().Equals("03")) { rtnVal = " "; }
                else if (argResult.Trim().Equals("11") || argResult.Trim().Equals("12")) { rtnVal = "B"; }
                else { rtnVal = "R"; }
            }
            else if (argExCode.Equals("A106") || argExCode.Equals("A107"))
            {
                //청력 좌,우
                if (VB.Left(argResult, 2) == "정상") { rtnVal = " "; }
                else { rtnVal = "R"; }
            }
            else if (argExCode.Equals("A111") || argExCode.Equals("A112") || argExCode.Equals("A113"))
            {
                //요당,요단백,요잠혈
                if (nResult == 2) { rtnVal = "B"; }
                else if (nResult >= 3) { rtnVal = "R"; }
                else { rtnVal = " "; }
            }
            else if (argExCode.Equals("A120"))
            {
                //평형성
                if (nResult <= 9) { rtnVal = "R"; }
                else if (nResult >= 10 && nResult <= 19) { rtnVal = "B"; }
                else { rtnVal = " "; }
            }
            else if (argExCode.Equals("A121"))
            {
                //혈색소
                if (argSex == "M")  //남자
                {
                    if (nResult >= 12 && nResult <= 12.9)
                    {
                        rtnVal = "B";
                    }
                    else if (nResult > 16.5)
                    {
                        rtnVal = "B";
                    }
                    else if (nResult < 12.0)
                    {
                        rtnVal = "R";
                    }
                    else
                    {
                        rtnVal = " ";
                    }
                }
                else
                {
                    if (nResult >= 10 && nResult <= 11.9)
                    {
                        rtnVal = "B";
                    }
                    else if (nResult >= 0 && nResult <= 9.9)
                    {
                        rtnVal = "R";
                    }
                    else if (nResult >= 15.6)
                    {
                        rtnVal = "B";
                    }
                    else
                    {
                        rtnVal = " ";
                    }
                }
            }
            else if (argExCode.Equals("A132"))
            {
                //HBs-Ab
                rtnVal = " ";
            }
            else if (argExCode.Equals("A131") || argExCode.Equals("A258") || argExCode.Equals("A289") || argExCode.Equals("E504") || argExCode.Equals("E505")
                  || argExCode.Equals("E508") || argExCode.Equals("E921") || argExCode.Equals("H841") || argExCode.Equals("TE15") || argExCode.Equals("TE16")
                  || argExCode.Equals("TE17") || argExCode.Equals("TE19") || argExCode.Equals("TE20") || argExCode.Equals("TE21"))
            {
                //HBs-Ag, RA반응, B형간염e항원,B형간염e항체 A259제외 (이미옥 2007-11-02 ),HIV-Ab(AIDS), 혈액인자 Rh,HCV-Ab
                //안저검사좌측(동맥경화성변화), 안저검사좌측(당뇨병성화), 안저검사좌측(고혈압성),안저검사우측(동맥경화성변화), 안저검사우측(당뇨병성화), 안저검사우측(고혈압성)
                if (nResult == 1) { rtnVal = " "; }
                else if (nResult == 2) { rtnVal = "R"; }
            }
            else if (argExCode.Equals("A161"))
            {
                //부인과세포검사(유형별)
            }
            else if (argExCode.Equals("A162"))
            {
                //세포검사(기타소견)
            }
            else if (argExCode.Equals("A212"))
            {
                //결핵균 도말검사
            }
            else if (argExCode.Equals("A233"))
            {
                //정밀안저검사(혈압)
                if (nResult == 1) { rtnVal = " "; }
                else { rtnVal = "R"; }
            }
            else if (argExCode.Equals("A263"))
            {
                //정밀안저검사(당뇨)
                if (nResult == 1) { rtnVal = " "; }
                else { rtnVal = "R"; }
            }
            else if (argExCode.Equals("TH01") || argExCode.Equals("TH02"))
            {
                //팀파노메트리검사(좌)
                if (nResult == 1) { rtnVal = " "; }
                else { rtnVal = "R"; }
            }
            else if (argExCode.Equals("A163") || argExCode.Equals("A164") || argExCode.Equals("A165"))
            {
                //부인과 검체상태, 부인과 자궁경부선상피 , 부인과 유형별진단
                if (nResult == 1) { rtnVal = " "; }
                else { rtnVal = "R"; }
            }
            else if (argExCode.Equals("A166") || argExCode.Equals("A167") || argExCode.Equals("A168"))
            {
                //부인과 편평상피세포이상
                if (nResult != 0) { rtnVal = "R"; }
                else { rtnVal = " "; }
            }
            else if (argExCode.Equals("A171"))
            {
                //부인과 종합판정
                if (nResult == 1) { rtnVal = " "; }
                else { rtnVal = "R"; }
            }
            //----------( 비만 )---------------------
            else if (argExCode.Equals("A103"))
            {
                //비만
                rtnVal = " ";
                if (argResult == "비만") rtnVal = "B";
                if (argResult == "과체중") rtnVal = "C";
                if (string.Compare(argYear, "2008") >= 0)
                {
                    if (argResult.Trim().Equals("03") || argResult.Trim().Equals("04") || argResult.Trim().Equals("05")) { rtnVal = "B"; }
                }
            }
            else if (argExCode.Equals("A151") || argExCode.Equals("A153") || argExCode.Equals("A234"))
            {
                //심전도
                if (argResult.Trim().Equals("01") || argResult.Trim() == "")
                    rtnVal = " ";
                else
                {
                    rtnVal = "R";
                }
            }
            else if (argExCode.Equals("A271") || argExCode.Equals("A272"))
            {
                if (argResult.ToUpper() == "MANY")
                {
                    rtnVal = "R";
                }
            }
            else if (argExCode.Equals("LM10"))   //객담세포검사
            {
                if (argResult.Trim().Equals("01") || argResult.Trim().Equals("02")) { rtnVal = " "; }
                else { rtnVal = "R"; }
            }
            else if (argExCode.Equals("LM11"))   //요세포검사
            {
                if (argResult.Trim().Equals("01") || argResult.Trim().Equals("02")) { rtnVal = " "; }
                else { rtnVal = "R"; }
            }
            else if (argExCode.Equals("TR11"))
            {
                if (argResult.Trim().Equals("01")) { rtnVal = " "; }
                else { rtnVal = "R"; }
            }
            else if (argExCode.Equals("A115"))   //허리둘레
            {
                if (argSex == "M")
                {
                    if (nResult >= 90) { rtnVal = "B"; }
                }
                else if (argSex == "F")
                {
                    if (nResult >= 85 && nResult != 999.9) { rtnVal = "B"; }
                }
            }
            else if (argExCode.Equals("TX07"))  //골다공증
            {
                if (argSex == "F")
                {
                    if (nResult == 1) { rtnVal = " "; }
                    else if (nResult == 2) { rtnVal = "B"; }
                    else if (nResult == 3) { rtnVal = "R"; }
                }
            }
            else if (argExCode.Equals("A119"))  //보행장애
            {
                if (nResult == 1) { rtnVal = " "; }
                else if (nResult == 2) { rtnVal = "B"; }
            }
            else if (argExCode.Equals("LU44") || argExCode.Equals("LU45") ||argExCode.Equals("LU46") || argExCode.Equals("LU47") || argExCode.Equals("LU49") || argExCode.Equals("LU50") || argExCode.Equals("LU53"))    //뇨잠혈
            {
                if (VB.L(argResult, "-") > 1) { rtnVal = " "; }
                else { rtnVal = "B"; }
            }
            else if (argExCode.Equals("LU39"))    //매독(VDRL)
            {
                if (argResult == "양성") { rtnVal = "R"; }
                else if (argResult == "Reactive") { rtnVal = "R"; }
                else if (argResult == "측정불가") { rtnVal = "B"; }
            }
            else if (argExCode.Equals("TZ12") || argExCode.Equals("ZE76") || argExCode.Equals("ZE77")) 
            {
                if (argResult == "01") { rtnVal = " "; }
            }

            //2021년부터 허리둘레 R판정으로 정리(ArgReturn 초기화로 코드마스터 검사항목별 체크로직)
            if (string.Compare(VB.Left(argJepDate, 4), "2021") >= 0 && argExCode.Equals("A115"))
            {
                rtnVal = "";
            }

            //위에서 검사를 하였으면 아래의 점검을 다시 안함
            if (rtnVal != "")
            {
                return rtnVal;
            }

            //코드마스타에 등록된 값으로 검사항목별 판정(B.정상B R.질환의심)
            if (nResult != 0.0 && nResult != 0 && argExCode != "A999" && argExCode != "ZD99")
            {
                if (nResult >= nMin && nResult <= nMax)
                {
                    rtnVal = "";
                }
                else if (nResult >= nMin_B && nResult <= nMax_B)
                {
                    rtnVal = "B";
                }
                else if (nResult >= nMin_R && nResult <= nMax_R)
                {
                    rtnVal = "R";
                }
                else
                {
                    rtnVal = "B";
                }
            }

            if (argExCode.Equals("A115") && nResult == 999.9) { rtnVal = ""; }
            if (argExCode.Equals("A241") && argResult == ">1000") { rtnVal = "R"; }

            return rtnVal;
        }

        /// <summary>
        /// 의사사번으로 의사이름 읽어오기
        /// </summary>
        /// <param name="argSABUN"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> READ_DrName_Sabun"/>
        public string READ_DrName_Sabun(long argSABUN)
        {
            string rtnVal = comHpcLibBService.ReadHicDoctor(argSABUN);

            return rtnVal;
        }

        /// <summary>
        /// 개인정보동의 처리일자
        /// </summary>
        /// <param name="argYear"></param>
        /// <param name="argPtno"></param>
        /// <returns> TEST OK</returns>
        public string Read_Privacy_Accept(string argYear, string argPtno)
        {
            string rtnVal = string.Empty;

            if (string.IsNullOrEmpty(argYear) || string.IsNullOrEmpty(argPtno))
            {
                return rtnVal;
            }

            rtnVal = comHpcLibBService.SelHicPrivacy_Accept(argYear, argPtno);

            return rtnVal;
        }

        /// <summary>
        /// 유해물질명 Display
        /// </summary>
        /// <param name="">유해물질코드</param>
        /// <returns>TEST OK</returns>
        /// <seealso cref="HcMain.bas> UCode_Names_Display"/>
        public string UCode_Names_Display(string ArgUCodes)
        {
            string rtnVal = string.Empty;
            string ArgSql = string.Empty;
            StringBuilder strSQL = new StringBuilder();

            if (string.IsNullOrEmpty(ArgUCodes))
            {
                return rtnVal;
            }

            for (int i = 0; i < VB.L(ArgUCodes, ","); i++)
            {
                if (VB.Pstr(ArgUCodes, ",", i + 1).Trim() != "")
                {
                    strSQL.Append("'");
                    strSQL.Append(VB.Pstr(ArgUCodes, ",", i + 1).Trim());
                    strSQL.Append("',");
                }
            }

            if (Convert.ToString(strSQL) == "") { return rtnVal; }

            ArgSql = strSQL.ToString();
            ArgSql = VB.Left(ArgSql, ArgSql.Length - 1);

            strSQL = new StringBuilder();

            IList<HIC_MCODE> list = hicMcodeService.SelMCode_Many(ArgSql);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].YNAME != null)
                {
                    strSQL.Append(list[i].YNAME);
                    strSQL.Append(",");
                }
                else
                {
                    strSQL.Append(list[i].NAME);
                    strSQL.Append(",");
                }
            }

            rtnVal = strSQL.ToString();
            rtnVal = VB.Left(rtnVal, rtnVal.Length - 1);

            return rtnVal;
        }

        /// <summary>
        /// 결과지 본인수령 체크
        /// </summary>
        /// <param name="">결과지 본인수령 체크</param>
        /// <returns>TEST OK</returns>
        /// <seealso cref="HcMain.bas> READ_JEPSU_GBCHK3"/>
        public string READ_JEPSU_GBCHK3(long argWrtno)
        {

            HIC_JEPSU item = hicJepsuService.GetItembyWrtNo(argWrtno);

            if (item.GBCHK3 == "Y")
            {
                return "OK";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 접수 카운트 확인
        /// </summary>
        /// <param name="">결과지 접수카운트 확인</param>
        /// <returns>TEST OK</returns>
        /// <seealso cref="HcMain.bas> READ_JEPSU_COUNT"/>
        public string READ_JEPSU_COUNT(string argPtno, string argJepDate)
        {
            string strGJJONG = "";

            List<HIC_JEPSU> list = hicJepsuService.GetGjJongbyPtnoJepDate2(argPtno, argJepDate);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (strGJJONG == "")
                    {
                        strGJJONG = list[i].GJJONG;
                    }
                    else
                    {
                        strGJJONG = strGJJONG + "," + list[i].GJJONG;
                    }

                }
                return strGJJONG;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 사후관리 코드 사후관리명 보여주기
        /// </summary>
        /// <param name="ArgCode"></param>
        /// <seealso cref="HcMain.bas> Sahu_Names_Display"/>
        /// <returns>TEST OK</returns>
        public string Sahu_Names_Display(string ArgCode)
        {
            string rtnVal = string.Empty;
            string ArgSql = string.Empty;
            StringBuilder strSQL = new StringBuilder();

            if (string.IsNullOrEmpty(ArgCode))
            {
                return rtnVal;
            }

            for (int i = 0; i < VB.L(ArgCode, ","); i++)
            {
                if (VB.Pstr(ArgCode, ",", i + 1).Trim() != "")
                {
                    strSQL.Append("'");
                    strSQL.Append(VB.Pstr(ArgCode, ",", i + 1).Trim());
                    strSQL.Append("',");
                }
            }

            if (Convert.ToString(strSQL) == "") { return rtnVal; }

            ArgSql = strSQL.ToString();
            ArgSql = VB.Left(ArgSql, ArgSql.Length - 1);

            strSQL = new StringBuilder();

            IList<HIC_CODE> list = hicCodeservice.FindCodeIn("32", ArgSql);

            for (int i = 0; i < list.Count; i++)
            {
                strSQL.Append(list[i].NAME);
                strSQL.Append(",");
            }

            rtnVal = strSQL.ToString();
            rtnVal = VB.Left(rtnVal, rtnVal.Length - 1);

            return rtnVal;
        }

        /// <summary>
        /// 그룹코드명 보여주기
        /// </summary>
        /// <param name="ArgCode"></param>
        /// <returns>TEST OK</returns>
        /// <seealso cref="HcMain.bas> SExam_Names_Display"/>
        public string SExam_Names_Display(string ArgCode)
        {
            string rtnVal = string.Empty;
            string ArgSql = string.Empty;
            StringBuilder strSQL = new StringBuilder();

            if (string.IsNullOrEmpty(ArgCode))
            {
                return rtnVal;
            }

            for (int i = 1; i <= VB.L(ArgCode, ","); i++)
            {
                if (VB.Pstr(ArgCode, ",", i).Trim() != "")
                {
                    strSQL.Append("'");
                    strSQL.Append(VB.Pstr(ArgCode, ",", i).Trim());
                    strSQL.Append("',");
                }
            }

            if (Convert.ToString(strSQL) == "") { return rtnVal; }

            ArgSql = strSQL.ToString();
            ArgSql = VB.Left(ArgSql, ArgSql.Length - 1);

            strSQL = new StringBuilder();

            IList<HIC_GROUPCODE> list = hicGroupcodeService.FindCodeIn(ArgSql);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].YNAME != null)
                {
                    strSQL.Append(list[i].YNAME);
                    strSQL.Append(",");
                }
                else
                {
                    strSQL.Append(list[i].NAME);
                    strSQL.Append(",");
                }
            }

            rtnVal = strSQL.ToString();
            rtnVal = VB.Left(rtnVal, rtnVal.Length - 1);

            return rtnVal;
        }

        /// <summary>
        /// 검진 문진표 응답항목 값 매칭
        /// </summary>
        /// <param name="ArgTable"></param>
        /// <param name="ArgCol"></param>
        /// <param name="ArgData"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> READ_Munjin_Name"/>
        public string READ_Munjin_Name(string ArgTable, string ArgCol, string ArgData)
        {
            string rtnVal = string.Empty;

            switch (ArgTable)
            {
                case "DENTAL":
                    switch (ArgCol)
                    {
                        case "OPDDNT":
                        case "SCALING":
                        case "JUNGSANG1":
                        case "JUNGSANG7":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.예"; break;
                                case "2": rtnVal = "2.아니오"; break;
                                case "3": rtnVal = "3.모르겠다"; break;
                                default:
                                    break;
                            }
                            break;
                        case "DNTSTATUS":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.매우좋음"; break;
                                case "2": rtnVal = "2.좋음"; break;
                                case "3": rtnVal = "3.보통"; break;
                                case "4": rtnVal = "4.나쁨"; break;
                                case "5": rtnVal = "5.매우나쁨"; break;
                                default:
                                    break;
                            }
                            break;
                        case "FOOD1":
                        case "FOOD2":
                        case "FOOD3":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.그렇다"; break;
                                case "2": rtnVal = "2.아니다"; break;
                                case "3": rtnVal = "3.보통이다"; break;
                                default:
                                    break;
                            }
                            break;
                        case "BRUSH21":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.옆으로"; break;
                                case "2": rtnVal = "2.위아래로"; break;
                                default:
                                    break;
                            }
                            break;
                        case "예아니오":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.예"; break;
                                case "2": rtnVal = "2.아니오"; break;
                                default:
                                    break;
                            }
                            break;
                        case "구강1":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.전혀피운적이없다"; break;
                                case "2": rtnVal = "2.현재피우고있다"; break;
                                case "3": rtnVal = "3.이전에피웠으나끊었다"; break;
                                default:
                                    break;
                            }
                            break;
                        case "구강2":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.항상했다"; break;
                                case "2": rtnVal = "2.대부분했다"; break;
                                case "3": rtnVal = "3.가끔했다"; break;
                                case "4": rtnVal = "4.전혀하지않았다"; break;
                                default:
                                    break;
                            }
                            break;
                        case "구강3":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.항상했다"; break;
                                case "2": rtnVal = "2.대부분했다"; break;
                                case "3": rtnVal = "3.가끔했다"; break;
                                case "4": rtnVal = "4.전혀하지않았다"; break;
                                case "5": rtnVal = "5.무엇인지모른다"; break;
                                default:
                                    break;
                            }
                            break;
                        case "구강4":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.매우불편하다"; break;
                                case "2": rtnVal = "2.불편하다"; break;
                                case "3": rtnVal = "3.그저그렇다"; break;
                                case "4": rtnVal = "4.별로불편하지않다"; break;
                                case "5": rtnVal = "5.전혀불편하지않다"; break;
                                default:
                                    break;
                            }
                            break;
                        case "구강5":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.예"; break;
                                case "2": rtnVal = "2.아니오"; break;
                                case "3": rtnVal = "3.모르겠다"; break;
                                default:
                                    break;
                            }
                            break;
                        case "구강6":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.항상 했다(7회)"; break;
                                case "2": rtnVal = "2.대부분 했다(4~6회)"; break;
                                case "3": rtnVal = "3.가끔 했다(1~3회)"; break;
                                case "4": rtnVal = "3.전혀 하지 않았다(0회)"; break;
                                default:
                                    break;
                            }
                            break;
                        case "구강7":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.먹지않음"; break;
                                case "2": rtnVal = "2.1번"; break;
                                case "3": rtnVal = "3.2~3번"; break;
                                case "4": rtnVal = "4.4번이상"; break;
                                case "5": rtnVal = "5.모르겠음"; break;
                                default:
                                    break;
                            }
                            break;
                        case "구강판정1":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.없음"; break;
                                case "2": rtnVal = "2.있음"; break;
                                default:
                                    break;
                            }
                            break;
                        case "구강판정2":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.정상"; break;
                                case "2": rtnVal = "2.이상"; break;
                                default:
                                    break;
                            }
                            break;
                        case "구강판정3":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.없음"; break;
                                case "2": rtnVal = "2.경증"; break;
                                case "3": rtnVal = "3.중증"; break;
                                default:
                                    break;
                            }
                            break;
                        case "구강판정4":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.양호"; break;
                                case "2": rtnVal = "2.수리/재제작요망"; break;
                                case "3": rtnVal = "3.보철물없음"; break;
                                default:
                                    break;
                            }
                            break;
                        case "구강판정5":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.양호"; break;
                                case "2": rtnVal = "2.수리/재제작요망"; break;
                                case "3": rtnVal = "3.틀니없음"; break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "BOHUM": break;
                case "CANCER":
                    switch (ArgCol.ToUpper())
                    {
                        case "GAJOK1":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.간염 혹은 간경변"; break;
                                case "2": rtnVal = "2.대장용종(폴립,혹)"; break;
                                case "3": rtnVal = "3.암"; break;
                                case "4": rtnVal = "4.기타"; break;
                            }
                            break;
                        case "DRINK1":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.(거의)마시지않는다"; break;
                                case "2": rtnVal = "2.월2~3회정도"; break;
                                case "3": rtnVal = "3.일주일1~2회"; break;
                                case "4": rtnVal = "4.일주일3~4회"; break;
                                case "5": rtnVal = "5.거의매일"; break;
                            }
                            break;
                        case "DRINK2":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.소주반병이하"; break;
                                case "2": rtnVal = "2.소주한병"; break;
                                case "3": rtnVal = "3.소주1병반"; break;
                                case "4": rtnVal = "4.소주2병이상"; break;
                            }
                            break;
                        case "SMOKING1":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.(거의)피우지않는다"; break;
                                case "2": rtnVal = "2.피웠으나 현재 끊음"; break;
                                case "3": rtnVal = "3.현재도 피운다"; break;
                            }
                            break;
                        case "SMOKING2":
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.반갑미만"; break;
                                case "2": rtnVal = "2.한갑미만"; break;
                                case "3": rtnVal = "3.두갑미만"; break;
                                case "4": rtnVal = "4.두갑이상"; break;
                            }
                            break;
                        case "WOMAN1":  //초경연령
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.10세미만"; break;
                                case "2": rtnVal = "2.10~13세"; break;
                                case "3": rtnVal = "3.14-16세"; break;
                                case "4": rtnVal = "4.17세이후"; break;
                                case "5": rtnVal = "5.모름"; break;
                            }
                            break;
                        case "WOMAN2":  //폐경여부
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.아니오"; break;
                                case "2": rtnVal = "2.예"; break;
                                case "3": rtnVal = "3.모름"; break;
                            }
                            break;
                        case "WOMAN3":  //폐경나이
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.39세이전"; break;
                                case "2": rtnVal = "2.40~44세"; break;
                                case "3": rtnVal = "3.45~49세"; break;
                                case "4": rtnVal = "4.50~54세"; break;
                                case "5": rtnVal = "5.55세이후"; break;
                            }
                            break;
                        case "WOMAN4":  //여성호로몬
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.39세이전"; break;
                                case "2": rtnVal = "2.40~50세"; break;
                                case "3": rtnVal = "3.51~60세"; break;
                                case "4": rtnVal = "4.60세이후"; break;
                            }
                            break;
                        case "WOMAN5":  //여성호로몬 연령
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.39세이전"; break;
                                case "2": rtnVal = "2.40~50세"; break;
                                case "3": rtnVal = "3.51~60세"; break;
                                case "4": rtnVal = "4.60세이후"; break;
                            }
                            break;
                        case "WOMAN6":  //여성호로몬 사용기간
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.1년미만"; break;
                                case "2": rtnVal = "2.1~5년"; break;
                                case "3": rtnVal = "3.6~10년"; break;
                                case "4": rtnVal = "4.11~20년"; break;
                                case "5": rtnVal = "5.20년이상"; break;
                            }
                            break;
                        case "WOMAN7":  //출산여부
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.아니오"; break;
                                case "2": rtnVal = "2.예"; break;
                            }
                            break;
                        case "WOMAN8":  //출산횟수
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.1회"; break;
                                case "2": rtnVal = "2.2회"; break;
                                case "3": rtnVal = "3.3회"; break;
                                case "4": rtnVal = "4.4회이상"; break;
                            }
                            break;
                        case "WOMAN9":  //초산시 나이
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.19세이전"; break;
                                case "2": rtnVal = "2.20~25세"; break;
                                case "3": rtnVal = "3.26~30세"; break;
                                case "4": rtnVal = "4.31~35세"; break;
                                case "5": rtnVal = "5.36세이후"; break;
                            }
                            break;
                        case "WOMAN10":  //모유 수유 기간
                            switch (ArgData)
                            {
                                case "1": rtnVal = "1.없다"; break;
                                case "2": rtnVal = "2.3개월이하"; break;
                                case "3": rtnVal = "3.4~12개월"; break;
                                case "4": rtnVal = "4.1년이상"; break;
                            }
                            break;
                    }
                    break;
                default:
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 검진 문진표 응답항목 값 매칭 READ_Munjin2009_Name => READ_Munjin_Name_New 로 변경
        /// </summary>
        /// <param name="ArgTable"></param>
        /// <param name="ArgCol"></param>
        /// <param name="ArgData"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> READ_Munjin_Name"/>
        public string READ_Munjin_Name_New(string ArgTable, string ArgCol, string ArgData)
        {
            string rtnVal = string.Empty;

            switch (ArgTable)
            {
                case "건강검진":
                    switch (ArgCol)
                    {
                        case "예아니오":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.예";
                                    break;
                                case "2":
                                    rtnVal = "2.아니오";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "일주일":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "0";
                                    break;
                                case "2":
                                    rtnVal = "1";
                                    break;
                                case "3":
                                    rtnVal = "2";
                                    break;
                                case "4":
                                    rtnVal = "3";
                                    break;
                                case "5":
                                    rtnVal = "4";
                                    break;
                                case "6":
                                    rtnVal = "5";
                                    break;
                                case "7":
                                    rtnVal = "6";
                                    break;
                                case "8":
                                    rtnVal = "7";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "만40세1":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.극히드물었다";
                                    break;
                                case "2":
                                    rtnVal = "2.가끔있었다";
                                    break;
                                case "3":
                                    rtnVal = "3.종종있었다";
                                    break;
                                case "4":
                                    rtnVal = "4.대부분그랬다";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "만66세1":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.아니다";
                                    break;
                                case "2":
                                    rtnVal = "2.가끔(조금)그렇다";
                                    break;
                                case "3":
                                    rtnVal = "3.자주(많이)그렇다";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "전자담배사용1":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.아니다";
                                    break;
                                case "2":
                                    rtnVal = "2.월1-2일";
                                    break;
                                case "3":
                                    rtnVal = "3.월3-9일";
                                    break;
                                case "4":
                                    rtnVal = "4.월10-29일";
                                    break;
                                case "5":
                                    rtnVal = "5.매일";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "구강검진":
                    switch (ArgCol.ToUpper())
                    {
                        case "예아니오":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.예";
                                    break;
                                case "2":
                                    rtnVal = "2.아니오";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "구강1":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.전혀피운적이없다";
                                    break;
                                case "2":
                                    rtnVal = "2.현재피우고있다";
                                    break;
                                case "3":
                                    rtnVal = "3.이전에피웠으나끊었다";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "구강2":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.항상했다";
                                    break;
                                case "2":
                                    rtnVal = "2.대부분했다";
                                    break;
                                case "3":
                                    rtnVal = "3.가끔했다";
                                    break;
                                case "4":
                                    rtnVal = "4.전혀하지않았다";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "구강3":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.항상했다";
                                    break;
                                case "2":
                                    rtnVal = "2.대부분했다";
                                    break;
                                case "3":
                                    rtnVal = "3.가끔했다";
                                    break;
                                case "4":
                                    rtnVal = "4.전혀하지않았다";
                                    break;
                                case "5":
                                    rtnVal = "5.무엇인지모른다";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "구강4":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.매우불편하다";
                                    break;
                                case "2":
                                    rtnVal = "2.불편하다";
                                    break;
                                case "3":
                                    rtnVal = "3.그저그렇다";
                                    break;
                                case "4":
                                    rtnVal = "4.별로불편하지않다";
                                    break;
                                case "5":
                                    rtnVal = "5.전혀불편하지않다";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "구강5":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.예";
                                    break;
                                case "2":
                                    rtnVal = "2.아니오";
                                    break;
                                case "3":
                                    rtnVal = "3.모르겠다";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "구강6":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.항상 했다(7회)";
                                    break;
                                case "2":
                                    rtnVal = "2.대부분 했다(4~6회)";
                                    break;
                                case "3":
                                    rtnVal = "3.가끔 했다(1~3회)";
                                    break;
                                case "4":
                                    rtnVal = "4.전혀 하지 않았다(0회)";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "구강7":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.먹지않음";
                                    break;
                                case "2":
                                    rtnVal = "2.1번";
                                    break;
                                case "3":
                                    rtnVal = "3.2~3번";
                                    break;
                                case "4":
                                    rtnVal = "4.4번이상";
                                    break;
                                case "5":
                                    rtnVal = "4.모르겠음";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "구강판정1":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.없음";
                                    break;
                                case "2":
                                    rtnVal = "2.있음";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "구강판정2":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.정상";
                                    break;
                                case "2":
                                    rtnVal = "2.이상";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "구강판정3":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.없음";
                                    break;
                                case "2":
                                    rtnVal = "2.경증";
                                    break;
                                case "3":
                                    rtnVal = "3.중증";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "구강판정4":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.양호";
                                    break;
                                case "2":
                                    rtnVal = "2.수리/재제작요망";
                                    break;
                                case "3":
                                    rtnVal = "3.보철물없음";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "구강판정5":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.양호";
                                    break;
                                case "2":
                                    rtnVal = "2.수리/재제작요망";
                                    break;
                                case "3":
                                    rtnVal = "3.틀니없음";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "암검진":
                    rtnVal = "";
                    break;
                case "특수검진":
                    switch (ArgCol.ToUpper())
                    {
                        case "예아니오":
                            switch (ArgData)
                            {
                                case "1":
                                    rtnVal = "1.예";
                                    break;
                                case "2":
                                    rtnVal = "2.아니오";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            return rtnVal;
        }

        public string GET_HEA_JepsuDate(long fnHeaWRTNO)
        {
            string rtnVal = "";

            //종전 검사일자
            rtnVal = hicJepsuService.GET_HEA_JepsuDate(fnHeaWRTNO);

            return rtnVal;
        }

        /// <summary>
        /// 검사결과에 판정 구분 UpDate
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argSex"></param>
        /// <param name="argDate"></param>
        /// <param name="argYear"></param>
        /// <seealso cref="HcMain.bas> ExamResult_RePanjeng"/>
        public void ExamResult_RePanjeng(long argWRTNO, string argSex, string argDate, string argYear)
        {
            string strExCode = string.Empty;
            string strOldPan = string.Empty;
            string strNewPan = string.Empty;
            string strResult = string.Empty;

            int result = 0;

            IList<HIC_RESULT> item = hicResultService.Get_Results(argWRTNO);

            for (int i = 0; i < item.Count; i++)
            {
                strExCode = item[i].EXCODE.To<string>("").Trim();
                strResult = item[i].RESULT.To<string>("").Trim();
                strOldPan = item[i].PANJENG.To<string>("").Trim();
                strNewPan = ExCode_Result_Panjeng(strExCode, strResult, argSex, argDate, argYear);

                if (string.Compare(strOldPan, strNewPan) != 0)
                {
                    result = hicResultService.PanUpDate(strNewPan, item[i].RID);
                }
            }
        }

        public void ExamResult_RePanjeng(List<long> argWRTNO, string argSex, string argDate, string argYear)
        {
            string strExCode = string.Empty;
            string strOldPan = string.Empty;
            string strNewPan = string.Empty;
            string strResult = string.Empty;

            int result = 0;

            IList<HIC_RESULT> item = hicResultService.Get_Results(argWRTNO);

            for (int i = 0; i < item.Count; i++)
            {
                strExCode = item[i].EXCODE.To<string>("").Trim();
                strResult = item[i].RESULT.To<string>("").Trim();
                strOldPan = item[i].PANJENG.To<string>("").Trim();
                strNewPan = ExCode_Result_Panjeng(strExCode, strResult, argSex, argDate, argYear);

                if (string.Compare(strOldPan, strNewPan) != 0)
                {
                    result = hicResultService.PanUpDate(strNewPan, item[i].RID);
                }
            }
        }

        /// <summary>
        /// 종검 검사결과에 판정 구분 UpDate
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argSex"></param>
        /// <param name="argDate"></param>
        /// <seealso cref="HaMain.bas> ExamResult_RePanjeng"/>
        public void ExamResult_RePanjeng_Hea(long argWRTNO, string argSex, string argDate)
        {
            int result = 0;
            string strNomal = "";
            string strExCode = "", strResult = "", strOldPan = "", strNewPan = "";
            double nMinData = 0.0;
            double nMaxData = 0.0;
            bool BoolPanjeng = true;

            //해당 접수번호의 결과를 READ
            List<HEA_RESULT> list = heaResultService.GetListByWrtno(argWRTNO);

            if (!list.IsNullOrEmpty() && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strExCode = list[i].EXCODE.To<string>("").Trim();
                    strResult = list[i].RESULT.To<string>("").Trim();
                    strOldPan = list[i].PANJENG.To<string>("").Trim();

                    //if (strExCode == "TX26")
                    //{
                    //    MessageBox.Show("A115", "");
                    //}

                    if (argSex == "M")
                    {
                        strNomal = list[i].MIN_M.To<string>("") + "~" + list[i].MAX_M.To<string>("");
                        nMinData = list[i].MIN_M.To<double>();
                        nMaxData = list[i].MAX_M.To<double>();
                    }
                    else
                    {
                        strNomal = list[i].MIN_F.To<string>("") + "~" + list[i].MAX_F.To<string>("");
                        nMinData = list[i].MIN_F.To<double>();
                        nMaxData = list[i].MAX_F.To<double>();
                    }

                    if (nMinData != 0 || nMaxData != 0)
                    {
                        switch (strExCode)
                        {
                            case "A101": BoolPanjeng = false; break;    //신장
                            case "A214": BoolPanjeng = false; break;    //치아검사,방사선관촬번호
                            case "H840":
                            case "H841": BoolPanjeng = false; break;    //혈액형(ABO/RH)
                            case "A215": BoolPanjeng = false; break;    //방사선직촬번호
                            default: break;
                        }

                        if (strResult.Trim() == "." || strResult.Trim() == "") { BoolPanjeng = false; }
                        //체크판정
                        if (BoolPanjeng == true)
                        {
                            strNewPan = cHaB.Result_Panjeng_New(strExCode, strResult, strNomal);
                        }
                    }
                    else if ((VB.Left(strResult.Trim(), 2) == "양성" || VB.Left(strResult.Trim(), 1) == "+") && strExCode != "H841" && strExCode != "A132")
                    {
                        //소변 및 대변 검사
                        if (strResult.Trim() == "양성" || strResult.Trim() == "+" || strResult.Trim() == "++" || strResult.Trim() == "+++" || strResult.Trim() == "++++" || strResult.Trim() == "+++++")
                        {
                            strNewPan = "B";
                        }
                        else if (strResult.Trim() == "+-")
                        {
                            if (strExCode != "LU46" && strExCode != "A259") { strNewPan = "B"; }
                        }
                    }

                    if (strNomal == "~") { strNomal = ""; }
                    if (strNomal == "음성(-)~") { strNomal = "음성(-)"; }
                    if (strNomal == "음성~") { strNomal = "음성"; }
                    if (strNomal == "정상~") { strNomal = "정상"; }
                    //정상,이상으로 결과값이 나올때
                    if (strNomal.Trim() == "정상" && strResult.Trim() != "" && strResult.Trim() != ".")
                    {
                        if (strNomal.Trim() != strResult.Trim()) { strNewPan = "B"; }
                    }
                    //강제색상변경
                    if (strExCode == "A289" && strResult.ToUpper() == "TRACE") { strNewPan = "B"; }

                    if (strExCode == "LU39" && strResult != "-" && strResult != "음성") { strNewPan = "B"; }
                    if (strExCode == "LU42" && strResult != "yellow" && strResult != ".") { strNewPan = "B"; }
                    if (strExCode == "LU44" && strResult != "-" && strResult != "음성") { strNewPan = "B"; }
                    if (strExCode == "LU45" && strResult != "-" && strResult != "음성") { strNewPan = "B"; }
                    if (strExCode == "LU46" && strResult != "-" && strResult != "+-" && strResult!= "음성") { strNewPan = "B"; }
                    if (strExCode == "LU47" && strResult != "-" && strResult != "음성") { strNewPan = "B"; }
                    if (strExCode == "LU48" && strResult != "-" && strResult != "음성") { strNewPan = "B"; }
                    if (strExCode == "LU49" && strResult != "-" && strResult != "음성") { strNewPan = "B"; }
                    if (strExCode == "LU50" && strResult != "-" && strResult != "음성") { strNewPan = "B"; }

                    if (strExCode == "TE05" && strResult!= "정상") { strNewPan = "B"; }
                    if (strExCode == "H841" && strResult == "-") { strNewPan = "B"; }
                    if (strExCode == "TH28" && VB.Left(strResult, 8) == "정밀검사") { strNewPan = "B"; }
                    if (strExCode == "ZE13" && VB.Left(strResult, 4) != "정상" && strResult!= ".") { strNewPan = "B"; }
                    if (strExCode == "A212" || strExCode == "TZ46") { strNewPan = "B"; }

                    //2020-10-06
                    if (strExCode == "A241" && strResult == ">1000") { strNewPan = "B"; }

                    if (strExCode == "TE05" || strExCode == "TE13" || strExCode == "TE14" || strExCode == "TE15"
                        || strExCode == "TE16" || strExCode == "TE17" || strExCode == "TE18" || strExCode == "TE19"
                        || strExCode == "TE20" || strExCode == "TE21" || strExCode == "TE22" || strExCode == "TE24"
                        || strExCode == "TE31" || strExCode == "TE32" || strExCode == "TE42")
                    {
                        if (strResult != "정상" && strResult != ".") { strNewPan = "B"; }
                    }
                  
                    if (strExCode == "A151" && strResult != "정상" && strResult != ".") { strNewPan = "B"; }
                    if (strExCode == "ZD00" && strResult != "정상" && strResult != ".") { strNewPan = "B"; }
                    if (strExCode == "A258" && strResult != "-") { strNewPan = "B"; }
                    //if (strExCode == "A259" && strResult != "-") { strNewPan = "B"; }
                    if (strExCode == "A259") { strNewPan = "B"; }
                    if (strExCode == "A424") { strNewPan = "B"; }
                    if (strExCode == "A456") { strNewPan = "B"; }
                    if (strExCode == "ZD06" && strResult != "음성") { strNewPan = "B"; }
                    if (strExCode == "ZD08" &&(strResult != "-" && strResult != "음성")) { strNewPan = "B"; }
                    if (strExCode == "H837") { strNewPan = "B"; }
                    if (strExCode == "E917" || strExCode == "E918") { strNewPan = "R"; }
                    if (strExCode == "E504" && strResult != "" && strResult != ".") { strNewPan = "B"; }
                    if (strExCode == "E910" && strResult != "" && strResult != ".") { strNewPan = "B"; }
                    
                    if (strExCode == "TH15" && strResult == "측정불가") { strNewPan = "B"; }
                    if (strExCode == "TH25" && strResult == "측정불가") { strNewPan = "B"; }
                    
                    if (strExCode == "A103")
                    {
                        if (string.Compare(VB.Left(argDate, 4), "2005") < 0)
                        {
                            strResult = Biman_Gesan(argWRTNO, "HEA");
                        }
                        else
                        {
                            strResult = Biman_Gesan(argWRTNO, "HEA");
                        }

                        switch (VB.Pstr(strResult, ".", 1))
                        {
                            case "01": strNewPan = "B"; break;
                            case "02": strNewPan = ""; break;
                            case "03":
                            case "04":
                            case "05":
                            case "06":
                            case "07": strNewPan = "B"; break;
                            default: break;
                        }
                    }
                    else
                    {
                        if (strExCode == "A259")  { strNewPan = "B"; }
                    }
                    if (strExCode == "A271" &&(strNewPan == "H" || strNewPan == "L")) { strNewPan = "B"; }
                    if (strExCode == "A272" &&(strNewPan == "H" || strNewPan == "L")) { strNewPan = "B"; }

                    //2021-10-26
                    if(strExCode == "E911" & strResult != "-") { strNewPan = "B"; }

                    if (strResult == "측정불가") { strNewPan = "B"; }
                    if (strResult == "본인제외") { strNewPan = "B"; }

                    if (strOldPan != strNewPan)
                    {
                        result = heaResultService.UpDateResultPanjengByRid(strNewPan, list[i].RID);
                    }

                    BoolPanjeng = true;
                    strNewPan = "";
                }
            }
        }

        /// <summary>
        /// Hic_Res_Special 정리
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> INSERT_HicResSpecial "/>
        public bool INSERT_HicResSpecial(HIC_JEPSU nHJ)
        {
            bool rtnVal = true;
            int result = 0;
            string strRowid = string.Empty;

            List<string> lstmCode = new List<string>();

            ComFunc.ReadSysDate(clsDB.DbCon);

            HIC_JEPSU item = hicJepsuService.Read_Jepsu_Wrtno(nHJ.WRTNO);

            if (item == null)
            {
                return false;
            }

            //암검진
            if (item.GJJONG.Equals("31") || item.GJJONG.Equals("35"))
            {
                return rtnVal;
            }

            //학생,운전면허 등
            if (string.Compare(item.GJJONG, "50") >= 0)
            {
                return rtnVal;
            }


            //취급물질이 없고 채용검진이 아니면 처리를 안함
            if ((item.UCODES == null || item.UCODES == "") && !item.GJJONG.Equals("21") && !item.GJJONG.Equals("22"))
            {
                result = hicResSpecialService.Delete(nHJ.WRTNO);
                if (result <= 0)
                {
                    return false;
                }

                //물질별 판정이 있으면 삭제함(판정된것은 제외)
                result = hicSpcPanjengService.All_Del_UpDate(nHJ.WRTNO);
                if (result <= 0)
                {
                    return false;
                }
            }

            //자료가 있는지 점검
            strRowid = hicResSpecialService.Read_Res_Special(nHJ.WRTNO);

            if (!strRowid.IsNullOrEmpty())
            {
                result = hicResSpecialService.UCodeName_UpDate(strRowid, item.UCODES);
                if (result <= 0)
                {
                    return false;
                }
            }

            //HIC_RES_SPECIAL
            if (strRowid.IsNullOrEmpty())
            {
                HIC_RES_SPECIAL nHRS = new HIC_RES_SPECIAL
                {
                    WRTNO = nHJ.WRTNO,
                    JEPDATE = nHJ.JEPDATE,
                    UCODENAME = nHJ.UCODES,
                    SEXAMNAME = nHJ.SEXAMS,
                    SABUN = nHJ.SABUN,
                    BUSE = nHJ.BUSENAME,
                    IPSADATE = nHJ.IPSADATE,
                    SUCHUPYN = nHJ.GBSUCHEP,
                    JIKJONG = nHJ.JIKJONG,
                    JENIPDATE = nHJ.BUSEIPSA
                };

                hicResSpecialService.Insert(nHRS);
            }


            //유해물질  SET

            int nUCodeCNT = (int)VB.L(item.UCODES, ",") - 1;

            if (nUCodeCNT <= 0)
            {
                //특수2차는 일반판정을 저장 안함
                switch (item.GJJONG)
                {
                    case "16":
                    case "19":
                    case "27":
                    case "28":
                    case "29": break;
                    default: lstmCode.Add("ZZZ"); break;
                }
            }
            else
            {
                for (int i = 1; i <= nUCodeCNT; i++)
                {
                    lstmCode.Add(VB.Pstr(item.UCODES, ",", i));
                }
            }

            //유해물질 건수를 UPDATE
            result = hicResSpecialService.UCodeCount_UpDate(nHJ.WRTNO, nUCodeCNT);
            if (result <= 0)
            {
                return false;
            }

            //21종, 22종 채용검진은 일반판정 항목 추가
            if (item.GJJONG.Equals("21") || item.GJJONG.Equals("22"))
            {
                if (VB.InStr(item.UCODES, "ZZZ") == 0)
                {
                    lstmCode.Add("ZZZ");
                }
            }

            //제외된 물질은 자동삭제
            string strOK = "";
            IList<HIC_SPC_PANJENG> lst = hicSpcPanjengService.Read_Spc_Panjeng(nHJ.WRTNO);

            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    strOK = "";
                    foreach (string strMCode in lstmCode)
                    {
                        if (strMCode.Equals(lst[i].MCODE))
                        {
                            strOK = "OK";
                            break;
                        }
                    }

                    if (strOK != "OK")
                    {
                        result = hicSpcPanjengService.OneDelUpDate(clsPublic.GstrSysDate, lst[i].RID);
                        if (result <= 0)
                        {
                            return false;
                        }
                    }
                }
            }

            //물질별 판정 레코드 형성
            int nCNT = lstmCode.Count;
            if (nCNT <= 0) { nCNT = 1; }

            string strCODE = string.Empty;
            string strUCODE = string.Empty;

            foreach (string strMCode in lstmCode)
            {
                strCODE = strMCode;
                if (strCODE.Equals("999"))
                {
                    strCODE = "ZZZ";
                }

                //자료가 있는지 점검
                strRowid = hicSpcPanjengService.FindRid(nHJ.WRTNO, strCODE);

                if (strRowid.IsNullOrEmpty())
                {
                    strUCODE = hicMcodeService.Read_UCode(strCODE);

                    if (strUCODE != "")
                    {
                        HIC_SPC_PANJENG item2 = new HIC_SPC_PANJENG();

                        item2.WRTNO = nHJ.WRTNO;
                        item2.JEPDATE = item.JEPDATE;
                        item2.LTDCODE = item.LTDCODE;
                        item2.MCODE = strCODE;
                        item2.UCODE = strUCODE;

                        result = hicSpcPanjengService.Insert(item2);

                        if (result <= 0)
                        {
                            return false;
                        }
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 접수마스터 입력상태 구분 변경
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> JEPSU_STS_Update"/>
        public bool JEPSU_STS_Update(long argWRTNO)
        {
            bool rtnVal = true;
            string[] strMunOK = new string[4];
            string strSTS = string.Empty;

            //접수번호별 문진 입력상태를 읽음
            HIC_JEPSU_EXJONG_PATIENT item = hicJepsuExjongPatientService.ReadJepMunjinSatus(argWRTNO);
            if (item.IsNullOrEmpty()) { return false; }

            switch (item.GBMUNJIN.ToString())
            {
                case "1":
                case "2":
                case "4": strMunOK[1] = "N"; break;     //건강보험1차,암문진,건강보험+특수
                case "3": strMunOK[3] = "N"; break;     //특수검진
                default: break;
            }

            if (item.UCODES.ToString() != "") { strMunOK[3] = "N"; }
            if (strMunOK[1] == "N" && item.GBDENTAL.ToString() == "Y") { strMunOK[2] = "N"; }

            //문진 입력여부 점검
            if (item.GBMUNJIN1.ToString() == "Y") { strMunOK[1] = "Y"; }
            if (item.GBMUNJIN2.ToString() == "Y") { strMunOK[2] = "Y"; }
            if (item.GBMUNJIN3.ToString() == "Y") { strMunOK[3] = "Y"; }

            //HIC_EXCODE GBRESEMPTY  컬럼 N 인 검사는 제외하고 미입력된 검사가 있는지 점검
            int nNotCNt = hicResultService.Chk_NonExcute_Result_Count(argWRTNO);
            strSTS = "1"; //결과 미완료

            if (nNotCNt == 0) { strSTS = "2"; } //결과입력 완료

            //결과입력 상태를 저장
            int result = hicJepsuService.UpDate_GbSTS(strSTS, argWRTNO, strMunOK);
            if (result <= 0)
            {
                return false;
            }

            return rtnVal;
        }

        /// <summary>
        /// 접수상태 GBSTS UPDATE
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> Result_EntryEnd_Check"/>
        public string Result_EntryEnd_Check(long argWRTNO)
        {
            string rtnVal = string.Empty;
            string strSTS = "2";

            //HIC_EXCODE GBRESEMPTY  컬럼 N 인 검사는 제외하고 미입력된 검사가 있는지 점검
            int nNotCNt = hicResultService.Chk_NonExcute_Result_Count(argWRTNO);

            if (nNotCNt > 0) { strSTS = "1"; }

            //결과입력 상태를 저장
            int result = hicJepsuService.UpDate_GbSTS(strSTS, argWRTNO);

            rtnVal = strSTS;

            return rtnVal;
        }

        /// <summary>
        /// 비만도 계산 Biman_Gesan_2004, Biman_Gesan_2005, Biman_Gesan_2008 => Biman_Gesan 하나로 취합
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="strGubun : "HIC : 일반검진" / "HEA : "종검"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> Biman_Gesan_2008 "/>
        public string Biman_Gesan(long argWRTNO, string argGubun = "", string argSex = "")
        {
            string rtnVal = string.Empty;
            string strBMI = string.Empty;
            string[] strCodes = new string[] { "A101", "A102", "A117" };
            double nHEIGHT = 0.0;
            double nWEIGHT = 0.0;
            double nBiman = 0.0;
            int nSex = 0;

            string strResultEtc = "";


            IList<HIC_RESULT> item = hicResultService.Read_Result_All(argWRTNO, argGubun, strCodes);

            if (item.Count > 0)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    if (item[i].EXCODE.Equals("A101") && item[i].RESULT != null)
                    {
                        nHEIGHT = item[i].RESULT.To<double>();
                        if (item[i].RESULT == "본인제외") { strResultEtc = "본인제외"; }
                        if (item[i].RESULT == "측정불가") { strResultEtc = "측정불가"; }
                    }
                    else if (item[i].EXCODE.Equals("A102") && item[i].RESULT != null)
                    {
                        nWEIGHT = item[i].RESULT.To<double>(); 
                        if (item[i].RESULT == "본인제외") { strResultEtc = "본인제외"; }
                        if (item[i].RESULT == "측정불가") { strResultEtc = "측정불가"; }
                    }
                    else if (item[i].EXCODE.Equals("A117"))
                    {
                        strBMI = "OK";
                    }
                }
            }

            if ( strResultEtc == "본인제외" || strResultEtc == "측정불가") { return strResultEtc; }



            if (nHEIGHT == 0 || nWEIGHT == 0)
            {
                return rtnVal;
            }

            //비만도= 체중(kg) / 키(m) / 키(m)
            //nHEIGHT = Math.Round(nHEIGHT / 100.0,               3, MidpointRounding.AwayFromZero);
            //nBiman  = Math.Round(nWEIGHT / nHEIGHT / nHEIGHT,   3, MidpointRounding.AwayFromZero);
            nHEIGHT = (nHEIGHT / 100.0);
            nBiman = (nWEIGHT / nHEIGHT / nHEIGHT);
            //소수점 2자리에서 1자리로 수정 2020-12-17 KMC
            //nBiman = Math.Round(nBiman, 2, MidpointRounding.AwayFromZero);
            nBiman = Math.Round(nBiman, 1, MidpointRounding.AwayFromZero);

            string strPanjeng = string.Empty;

            if (argGubun == "HIC")
            {

                if (nBiman < 18.5)
                {
                    rtnVal = "01.저체중";      //저체중
                    strPanjeng = "B";
                }
                else if (nBiman >= 18.5 && nBiman < 23.0)
                {
                    rtnVal = "02.정상체중";      //정상체중
                    strPanjeng = "";
                }
                else if (nBiman >= 23.0 && nBiman < 25.0)
                {
                    rtnVal = "03.비만전단계";      //비만전단계
                    strPanjeng = "B";
                }
                else if (nBiman >= 25.0 && nBiman < 30.0)
                {
                    rtnVal = "04.비만1단계";      //비만1단계
                    strPanjeng = "B";
                }
                else if (nBiman >= 30.0 && nBiman < 40.0)
                {
                    rtnVal = "05.비만2단계";      //비만2단계
                    strPanjeng = "B";
                }
                else
                {
                    rtnVal = "06.비만3단계";      //비만3단계
                    strPanjeng = "B";
                }

                //비만도 검사코드 정보 읽기
                HIC_EXCODE item2 = hicExCodeservice.FindOne("A103");

                //HIC_RESULT Table에 비만도 계산값 UpDate
                HIC_RESULT item3 = new HIC_RESULT
                {
                    WRTNO = argWRTNO,
                    EXCODE = "A103",
                    RESULT = rtnVal,
                    RESCODE = item2.RESCODE,
                    PANJENG = strPanjeng,
                    //ENTSABUN = Convert.ToInt32(clsType.User.IdNumber)
                    ENTSABUN = 111
                };

                int result = hicResultService.UpDate(item3);

                if (strBMI.Equals("OK"))
                {
                    //체질량지수 검사코드 정보 읽기
                    HIC_EXCODE item_A117 = hicExCodeservice.FindOne("A117");

                    //체질량을 저장
                    HIC_RESULT item4 = new HIC_RESULT
                    {
                        WRTNO = argWRTNO,
                        EXCODE = "A117",
                        //RESULT = nBiman.To<string>(),
                        RESULT = (Math.Truncate(nBiman * 10) / 10).ToString(),
                        RESCODE = item_A117.RESCODE,
                        PANJENG = strPanjeng,
                        //ENTSABUN = Convert.ToInt32(clsType.User.IdNumber)
                        ENTSABUN = 111
                    };

                    int result2 = hicResultService.UpDate(item4);
                }
            }
            else if (argGubun == "HEA")
            {

                //종합검진경우 비만도계산시 반올림 제외
                nBiman = (nWEIGHT / nHEIGHT / nHEIGHT);
                if (nBiman < 18.6)
                {
                    rtnVal = "01.저체중";      //저체중
                    strPanjeng = "B";
                }
                else if (nBiman >= 18.6 && nBiman < 23.0)
                {
                    rtnVal = "02.정상체중";      //정상체중
                    strPanjeng = "";
                }
                else if (nBiman >= 23.0 && nBiman < 25.0)
                {
                    rtnVal = "03.비만전단계";      //비만전단계
                    strPanjeng = "B";
                }
                else if (nBiman >= 25.0 && nBiman < 30.0)
                {
                    rtnVal = "04.비만1단계";      //비만1단계
                    strPanjeng = "B";
                }
                else if (nBiman >= 30.0 && nBiman < 40.0)
                {
                    rtnVal = "05.비만2단계";      //비만2단계
                    strPanjeng = "B";
                }
                else
                {
                    rtnVal = "06.비만3단계";      //비만3단계
                    strPanjeng = "B";
                }
            }
            else if (argGubun == "HAPAN")
            {
                if (argSex == "M" || argSex == "남자")
                {
                    nSex = 22;
                }
                else
                {
                    nSex = 21;
                }

                rtnVal = nBiman.To<string>("0");
            }

            return rtnVal;
        }

        /// <summary>
        /// AIR 3분법 자동계산
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argGubun (HIC : 일반검진 / HEA : 일반검진)"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> AIR3_AUTO_2004"/>
        public bool AIR3_AUTO(long argWRTNO, string argGubun = "")
        {
            int result = 0;
            bool rtnVal = true;
            string strResult_Lt = "";
            string strResult_Rt = "";
            string[] strCodeList = new string[] { "TH21", "TH22", "TH23", "TH11", "TH12", "TH13", "TH67", "TH68", "TH69", "TH73", "TH74", "TH75" };
            double nLt_500 = 0;
            double nLt_1000 = 0;
            double nLt_2000 = 0;
            double nRt_500 = 0;
            double nRt_1000 = 0;
            double nRt_2000 = 0;
            double nTH90 = 0;
            double nTH91 = 0;
            string strTH90 = "";
            string strTH91 = "";

            string strR500 = "";
            string strR1000 = "";
            string strR2000 = "";
            string strL500 = "";
            string strL1000 = "";
            string strL2000 = "";

            IList<HIC_RESULT> list = hicResultService.Read_Result_All(argWRTNO, argGubun, strCodeList);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].RESULT == "X" || list[i].RESULT == "x")
                    {
                        if (list[i].EXCODE.Equals("TH67") || list[i].EXCODE.Equals("TH68") || list[i].EXCODE.Equals("TH69") ||
                            list[i].EXCODE.Equals("TH11") || list[i].EXCODE.Equals("TH12") || list[i].EXCODE.Equals("TH13"))
                        {
                            strResult_Lt = "X";
                        }
                        else if (list[i].EXCODE.Equals("TH73") || list[i].EXCODE.Equals("TH74") || list[i].EXCODE.Equals("TH75") ||
                                 list[i].EXCODE.Equals("TH21") || list[i].EXCODE.Equals("TH22") || list[i].EXCODE.Equals("TH23"))
                        {
                            strResult_Rt = "X";
                        }
                    }
                    //음차폐가 있다면 음차폐먼저 적용
                    else
                    {
                        switch (list[i].EXCODE)
                        {
                            case "TH67": nLt_500 = VB.Val(list[i].RESULT); strL500 = "OK"; break;
                            case "TH68": nLt_1000 = VB.Val(list[i].RESULT); strL1000 = "OK"; break;
                            case "TH69": nLt_2000 = VB.Val(list[i].RESULT); strL2000 = "OK"; break;

                            case "TH73": nRt_500 = VB.Val(list[i].RESULT); strR500 = "OK"; break;
                            case "TH74": nRt_1000 = VB.Val(list[i].RESULT); strR1000 = "OK"; break;
                            case "TH75": nRt_2000 = VB.Val(list[i].RESULT); strR2000 = "OK"; break;

                            case "TH11": if (nLt_500 == 0) { nLt_500 = VB.Val(list[i].RESULT); strL500 = "OK"; } break;
                            case "TH12": if (nLt_1000 == 0) { nLt_1000 = VB.Val(list[i].RESULT); strL1000 = "OK"; } break;
                            case "TH13": if (nLt_2000 == 0) { nLt_2000 = VB.Val(list[i].RESULT); strL2000 = "OK"; } break;

                            case "TH21": if (nRt_500 == 0) { nRt_500 = VB.Val(list[i].RESULT); strR500 = "OK"; } break;
                            case "TH22": if (nRt_1000 == 0) { nRt_1000 = VB.Val(list[i].RESULT); strR1000 = "OK"; } break;
                            case "TH23": if (nRt_2000 == 0) { nRt_2000 = VB.Val(list[i].RESULT); strR2000 = "OK"; } break;
                            default:
                                break;
                        }
                    }
                }

                nTH90 = nRt_500 + nRt_1000 + nRt_2000;
                nTH91 = nLt_500 + nLt_1000 + nLt_2000;

                //소수 두째자리 절사 = 6.19 > 6.1
                if (nTH90 == 0)
                {
                    strTH90 = "0";
                }
                else
                {
                    strTH90 = string.Format("{0:##0.0}", (Math.Truncate(nTH90 / 3.0 * 10) / 10).To<double>());
                }

                if (nTH91 == 0)
                {
                    strTH91 = "0";
                }
                else
                {
                    strTH91 = string.Format("{0:##0.0}", (Math.Truncate(nTH91 / 3.0 * 10) / 10));
                }

                if (strResult_Rt.Equals("X")) { strTH90 = "X"; }
                if (strResult_Lt.Equals("X")) { strTH91 = "X"; }

                HIC_RESULT item1 = new HIC_RESULT();

                item1.EXCODE = "TH90";
                item1.RESULT = strTH90;
                item1.ENTSABUN = 111;
                item1.WRTNO = argWRTNO;

                result = hicResultService.UpDate(item1);
                if (result <= 0)
                {
                    rtnVal = false;
                }

                item1.EXCODE = "TH91";
                item1.RESULT = strTH91;
                item1.ENTSABUN = 111;
                item1.WRTNO = argWRTNO;

                result = hicResultService.UpDate(item1);
                if (result <= 0)
                {
                    rtnVal = false;
                }

                if (strR500 == "" || strR1000 == "" || strR2000 == "" || strL500 == "" || strL1000 == "" || strL2000 == "" )
                {
                    rtnVal = false;
                }

            }
            return rtnVal;
        }

        /// <summary>
        /// AIR 6분법 자동계산
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argGubun (HIC : 일반검진 / HEA : 일반검진)"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> AIR6_AUTO_2010"/>
        public bool AIR6_AUTO(long argWRTNO, string argGubun = "")
        {
            int result = 0;
            bool rtnVal = true;
            string strResult_Lt = string.Empty;
            string strResult_Rt = string.Empty;
            string strTH96 = string.Empty;
            string strTH97 = string.Empty;
            string[] strCodeList1 = new string[] { "TH67", "TH68", "TH69", "TH71", "TH73", "TH74", "TH75", "TH77" };
            string[] strCodeList2 = new string[] { "TH21", "TH22", "TH23", "TH25", "TH11", "TH12", "TH13", "TH15" };
            double nTH21 = 0.0;
            double nTH22 = 0.0;
            double nTH23 = 0.0;
            double nTH25 = 0.0;
            double nTH11 = 0.0;
            double nTH12 = 0.0;
            double nTH13 = 0.0;
            double nTH15 = 0.0;
            double nTH96 = 0.0;
            double nTH97 = 0.0;

            //음차폐가 있으면 음차폐값으로 계산
            IList<HIC_RESULT> list1 = hicResultService.Read_Result_All(argWRTNO, argGubun, strCodeList1);
            if (list1.Count > 0)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    if (list1[i].RESULT != null)
                    {
                        if (list1[i].RESULT.Equals("X"))
                        {
                            if (list1[i].EXCODE.Equals("TH67") || list1[i].EXCODE.Equals("TH68") || list1[i].EXCODE.Equals("TH69") || list1[i].EXCODE.Equals("TH71"))
                            {
                                strResult_Lt = "X";
                            }
                            else if (list1[i].EXCODE.Equals("TH73") || list1[i].EXCODE.Equals("TH74") || list1[i].EXCODE.Equals("TH75") || list1[i].EXCODE.Equals("TH77"))
                            {
                                strResult_Rt = "X";
                            }
                        }
                        else
                        {
                            if (VB.Val(list1[i].RESULT) > 0)
                            {
                                switch (list1[i].EXCODE)
                                {
                                    case "TH73": nTH21 = VB.Val(list1[i].RESULT); break;
                                    case "TH74": nTH22 = VB.Val(list1[i].RESULT); break;
                                    case "TH75": nTH23 = VB.Val(list1[i].RESULT); break;
                                    case "TH77": nTH25 = VB.Val(list1[i].RESULT); break;
                                    case "TH67": nTH11 = VB.Val(list1[i].RESULT); break;
                                    case "TH68": nTH12 = VB.Val(list1[i].RESULT); break;
                                    case "TH69": nTH13 = VB.Val(list1[i].RESULT); break;
                                    case "TH71": nTH15 = VB.Val(list1[i].RESULT); break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            IList<HIC_RESULT> list2 = hicResultService.Read_Result_All(argWRTNO, argGubun, strCodeList2);
            if (list2.Count > 0)
            {
                for (int i = 0; i < list2.Count; i++)
                {
                    if (list2[i].RESULT != null)
                    {
                        if (list2[i].RESULT.Equals("X"))
                        {
                            if (list2[i].EXCODE.Equals("TH11") || list2[i].EXCODE.Equals("TH12") || list2[i].EXCODE.Equals("TH13") || list2[i].EXCODE.Equals("TH15"))
                            {
                                strResult_Lt = "X";
                            }
                            else if (list2[i].EXCODE.Equals("TH21") || list2[i].EXCODE.Equals("TH22") || list2[i].EXCODE.Equals("TH23") || list2[i].EXCODE.Equals("TH25"))
                            {
                                strResult_Rt = "X";
                            }
                        }
                        else
                        {
                            if (VB.Val(list2[i].RESULT) > 0)
                            {
                                switch (list2[i].EXCODE)
                                {
                                    case "TH21": nTH21 = VB.Val(list2[i].RESULT); break;
                                    case "TH22": nTH22 = VB.Val(list2[i].RESULT); break;
                                    case "TH23": nTH23 = VB.Val(list2[i].RESULT); break;
                                    case "TH25": nTH25 = VB.Val(list2[i].RESULT); break;
                                    case "TH11": nTH11 = VB.Val(list2[i].RESULT); break;
                                    case "TH12": nTH12 = VB.Val(list2[i].RESULT); break;
                                    case "TH13": nTH13 = VB.Val(list2[i].RESULT); break;
                                    case "TH15": nTH15 = VB.Val(list2[i].RESULT); break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            if (list1 != null && list2 != null)
            {
                //소수 두째자리 절사 = 6.19 > 6.1
                nTH96 = (nTH21 + (2 * nTH22) + (2 * nTH23) + nTH25) / 6.0;
                nTH97 = (nTH11 + (2 * nTH12) + (2 * nTH13) + nTH15) / 6.0;

                if (nTH96 > 0)
                {
                    strTH96 = (Math.Truncate(nTH96 * 100) / 100).ToString();
                }
                else
                {
                    strTH96 = "0";
                }

                if (nTH97 > 0)
                {
                    strTH97 = (Math.Truncate(nTH97 * 100) / 100).ToString();
                }
                else
                {
                    strTH97 = "0";
                }

                if (strResult_Lt.Equals("X")) { strTH97 = "X"; }
                if (strResult_Rt.Equals("X")) { strTH96 = "X"; }

                HIC_RESULT item1 = new HIC_RESULT();

                item1.EXCODE = "TH96";
                item1.RESULT = strTH96;
                item1.ENTSABUN = 111;
                item1.WRTNO = argWRTNO;

                result = hicResultService.UpDate(item1);
                if (result <= 0)
                {
                    rtnVal = false;
                }

                item1.EXCODE = "TH97";
                item1.RESULT = strTH97;
                item1.ENTSABUN = 111;
                item1.WRTNO = argWRTNO;

                result = hicResultService.UpDate(item1);
                if (result <= 0)
                {
                    rtnVal = false;
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 조치대상 이름 읽어오기
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> Jochi2_Name"/>
        public string Jochi2_Name(string argCode)
        {
            string rtnVal = string.Empty;

            HIC_CODE item = hicCodeservice.Read_Hic_Code(argCode, "14");

            rtnVal = item.NAME;

            return rtnVal;
        }

        /// <summary>
        /// 1차 판정의사 면허번호  UPDATE
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <seealso cref="HcMain.bas> UPDATE_FirstPanjeng_DrNo"/>
        public bool UPDATE_FirstPanjeng_DrNo(long argWRTNO)
        {
            long nPaDrno = 0;
            string[] strJongSQL = null;

            try
            {
                HIC_JEPSU item = hicJepsuService.Read_Jepsu_Wrtno(argWRTNO);

                if (item == null) { return true; }
                if (item.GJCHASU != "2") { return true; }

                switch (item.GJJONG)
                {
                    case "16": strJongSQL = new string[] { "11", "14" }; break;
                    case "17": strJongSQL = new string[] { "12" }; break;
                    case "18": strJongSQL = new string[] { "13" }; break;
                    case "19": strJongSQL = new string[] { "14" }; break;
                    case "27": strJongSQL = new string[] { "21" }; break;
                    case "28": strJongSQL = new string[] { "23" }; break;
                    case "29": strJongSQL = new string[] { "22" }; break;
                    case "33": strJongSQL = new string[] { "24" }; break;
                    case "44": strJongSQL = new string[] { "41" }; break;
                    case "45": strJongSQL = new string[] { "42" }; break;
                    case "46": strJongSQL = new string[] { "43" }; break;
                    case "50": strJongSQL = new string[] { "51" }; break;
                    default: strJongSQL = new string[] { "11", "12", "13", "14" }; break;
                }

                COMHPC cItem = comHpcLibBService.ReadFirstPanDrno(item.PANO, item.GJYEAR, item.GJJONG, item.JEPDATE.ToString(), strJongSQL);
                if (!cItem.IsNullOrEmpty())
                {
                    if (cItem.DRNO1 > 0)
                    {
                        nPaDrno = cItem.DRNO1;
                    }
                    else
                    {
                        if (cItem.DRNO2 > 0)
                        {
                            nPaDrno = cItem.DRNO2;
                        }
                    }
                }

                if (nPaDrno > 0)
                {
                    HIC_JEPSU item1 = new HIC_JEPSU();

                    item1.WRTNO = argWRTNO;
                    item1.FIRSTPANDRNO = nPaDrno;

                    if (hicJepsuService.UpDate_FirstPanDrno(item1) <= 0)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// HDL 콜레스테롤 결과값 계산
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argDate"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> HDL_Cholesterol_CHK"/>
        public string HDL_Cholesterol_CHK(long argWRTNO, DateTime argDate)
        {
            string rtnVal = string.Empty;
            double[] nTemp = new double[4];

            long nPano = hicJepsuService.GetPanoByWrtno(argWRTNO);

            long nWrtno = hicJepsuService.GetWrtnoByPano_Cholesterol(nPano, argDate, argDate.ToString("yyyy"));

            string[] strWrtno = new string[] { argWRTNO.ToString(), nWrtno.ToString() };
            string[] strCodes = new string[] { "A123", "A242", "A241" };

            IList<HIC_RESULT> list = hicResultService.GetResultByWrtnosExCodes(strWrtno, strCodes);

            //3개가 있어야 보여줌.
            if (list.Count >= 3)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    switch (list[i].EXCODE)
                    {
                        case "A123": nTemp[1] = VB.Val(list[i].RESULT); break;
                        case "A241": nTemp[1] = VB.Val(list[i].RESULT); break;
                        case "A242": nTemp[1] = VB.Val(list[i].RESULT); break;
                        default:
                            break;
                    }
                }

                if (nTemp[2] >= 400)
                {
                    rtnVal = "";
                }
                else
                {
                    rtnVal = Math.Truncate(nTemp[1] - (nTemp[3] + (nTemp[2] / 5))).ToString();
                }
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
        }

        /// <summary>
        /// LDL 결과 계산
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <seealso cref="HcMain.bas> LDL_Gesan"/>
        public void LDL_Gesan(long argWRTNO)
        {
            double nA123 = 0;
            double nA242 = 0;
            double nA241 = 0;
            double nLDL = 0;

            string[] strCodes = new string[] { "A123", "A242", "A241", "C404" };

            IList<HIC_RESULT> list = hicResultService.GetResultByWrtnoExCodes(argWRTNO, strCodes);

            if (list.Count != 4) { return; }

            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i].EXCODE)
                {
                    case "A123": nA123 = VB.Val(list[i].RESULT); break;
                    case "A242": nA242 = VB.Val(list[i].RESULT); break;
                    case "A241": nA241 = VB.Val(list[i].RESULT); break;
                    default:
                        break;
                }
            }

            //2012년도 변경사항 400 이상 수치도 계산한다(수가발생함)
            if (nA241 >= 400)
            {
                nLDL = 1;
            }
            else
            {
                nLDL = Math.Truncate(nA123 - nA242 - (nA241 / 5));
            }

            //LDL 계산값이 음수이면 '1' 로 치환함 (09.05.19 손대영요청)
            if (nLDL < 0) { nLDL = 1; }

            HIC_RESULT item1 = new HIC_RESULT();

            item1.EXCODE = "C404";
            item1.RESULT = nLDL.ToString().Trim();
            item1.ENTSABUN = 111;
            item1.WRTNO = argWRTNO;

            int result = hicResultService.UpDate(item1);

        }

        /// <summary>
        /// TIBC 결과 계산
        /// </summary>
        /// <param name="argWRTNO"></param>
        public void TIBC_Gesan(long argWRTNO)
        {
            double nC901 = 0.0;
            double nLH43 = 0.0;
            double nC902 = 0.0;

            string[] strCodes = new string[] { "LH43", "C902", "C901" };

            IList<HIC_RESULT> list = hicResultService.GetResultByWrtnoExCodes(argWRTNO, strCodes);

            if (list.Count != 4) { return; }

            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i].EXCODE)
                {
                    case "C901": nC901 = VB.Val(list[i].RESULT); break;
                    case "LH43": nLH43 = VB.Val(list[i].RESULT); break;
                    case "C902": nC902 = VB.Val(list[i].RESULT); break;
                    default:
                        break;
                }
            }

            nC901 = nLH43 + nC902;

            HIC_RESULT item1 = new HIC_RESULT();

            item1.EXCODE = "C901";
            item1.RESULT = nC901.ToString().Trim();
            item1.ENTSABUN = 111;
            item1.WRTNO = argWRTNO;

            int result = hicResultService.UpDate(item1);

        }

        /// <summary>
        /// 청력(좌), 청력(우) 판정결과 UpDate
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <seealso cref="HcMain.bas> Update_Audiometry"/>
        public void Update_Audiometry(long argWRTNO)
        {
            string strPanjengL = string.Empty;
            string strPanjengR = string.Empty;
            string[] strCodes = new string[] { "TH12", "TH22" };

            IList<HIC_RESULT> list = hicResultService.GetResultByWrtnoExCodes(argWRTNO, strCodes);

            if (list.Count == 0) { return; }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].EXCODE.Equals("TH12"))
                {
                    if (!string.IsNullOrEmpty(list[i].RESULT))
                    {
                        if (list[i].RESULT.Equals(".") || VB.Val(list[i].RESULT) >= -10 && VB.Val(list[i].RESULT) <= 39 && list[i].RESULT != "X")
                        {
                            strPanjengL = "정상";
                        }
                        else
                        {
                            strPanjengL = "비정상";
                        }
                    }
                }
                else if (list[i].EXCODE.Equals("TH22"))
                {
                    if (!string.IsNullOrEmpty(list[i].RESULT))
                    {
                        if (list[i].RESULT.Equals(".") || VB.Val(list[i].RESULT) >= -10 && VB.Val(list[i].RESULT) <= 39 && list[i].RESULT != "X")
                        {
                            strPanjengR = "정상";
                        }
                        else
                        {
                            strPanjengR = "비정상";
                        }
                    }
                }
            }

            int result = 0;

            if (!strPanjengL.IsNullOrEmpty() && !strPanjengR.IsNullOrEmpty())
            {
                HIC_RESULT item1 = new HIC_RESULT();

                item1.EXCODE = "A106";
                item1.RESULT = strPanjengL;
                item1.ENTSABUN = 111;
                item1.WRTNO = argWRTNO;
                item1.ACTIVE = "Y";

                //result = hicResultService.UpDate(item1);
                result = hicResultService.UpDate_Audio_Auto(item1);

                item1.EXCODE = "A107";
                item1.RESULT = strPanjengR;
                item1.ENTSABUN = 111;
                item1.WRTNO = argWRTNO;
                item1.ACTIVE = "Y";

                //result = hicResultService.UpDate(item1);
                result = hicResultService.UpDate_Audio_Auto(item1);
            }
        }

        public bool Munjin_ITEM_SET(long argWRTNO)
        {
            bool rtnVal = true;
            string[] strMunOK = new string[4];
            string strSTS = string.Empty;

            try
            {
                //접수번호별 문진 입력상태를 읽음
                //IDictionary<string, object> item = comHpcLibBService.ReadJepMunjinSatus(argWRTNO);
                HIC_JEPSU_EXJONG_PATIENT item = hicJepsuExjongPatientService.ReadJepMunjinSatus(argWRTNO);
                if (item.IsNullOrEmpty()) { return true; }

                switch (item.GBMUNJIN)
                {
                    case "1":
                    case "2":
                    case "4": strMunOK[1] = "N"; break;     //건강보험1차,암문진,건강보험+특수
                    case "3": strMunOK[3] = "N"; break;     //특수검진
                    default: break;
                }

                if (!item.UCODES.IsNullOrEmpty()) { strMunOK[3] = "N"; }
                if (strMunOK[1] == "N" && item.GBDENTAL == "Y") { strMunOK[2] = "N"; }

                //문진 입력여부 점검
                if (item.GBMUNJIN1 == "Y") { strMunOK[1] = "Y"; }
                if (item.GBMUNJIN2 == "Y") { strMunOK[2] = "Y"; }
                if (item.GBMUNJIN3 == "Y") { strMunOK[3] = "Y"; }

                //결과입력 상태를 저장
                if (!hicJepsuService.UpDateGbMunjin2ByWrtno(argWRTNO, strMunOK[1], strMunOK[2], strMunOK[3]))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 체혈 자동 액팅 UpDate
        /// </summary>
        /// <param name="argPano"></param>
        /// <param name="argWrtno"></param>
        /// <param name="argDate"></param>
        /// <see cref="HcMain.bas> Update_Blood_Acting"/>
        public void Update_Blood_Acting(long argPano, long argWrtno, string argDate)
        {
            string strResult = string.Empty;
            string strAct = string.Empty;

            EXAM_DISPLAY_NEW item = examDisplayNewService.GetItemForBloodActing(argWrtno);

            if (!item.IsNullOrEmpty())
            {
                strResult = item.RESULT;
                strAct = item.ACTIVE;

                //62종 혈종은 당일 다른검진 채혈액팅 상태를 따라감
                long nWRTNO = examDisplayNewService.GetWrtnoForBloodActing(argPano, argDate);

                if (nWRTNO > 0)
                {
                    HIC_RESULT item2 = new HIC_RESULT
                    {
                        EXCODE = "A135",
                        WRTNO = nWRTNO,
                        ACTIVE = strAct,
                        RESULT = strResult,
                        ENTSABUN = clsType.User.IdNumber.To<long>()
                    };

                    int result = hicResultService.UpDate(item2);
                }
            }
        }

        /// <summary>
        /// 문진표 Table 자동생성
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argJONG"></param>
        /// <param name="argDent"></param>
        /// <param name="argUCodes"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> HIC_NEW_MUNITEM_INSERT"/>
        public string HIC_NEW_MUNITEM_INSERT(long argWRTNO, string argJONG, string argDent, string argUCodes)
        {
            int result = 0;
            string rtnVal = string.Empty;

            string strJong = hicExjongService.GetGbMunjin(argJONG);

            switch (strJong)
            {
                case "1":
                case "4":
                    if (HIC_WRTNO_CHECK(argWRTNO, "검진").Equals("Y"))
                    {
                        result = comHpcLibBService.InsertByWrtno(argWRTNO, "HIC_RES_BOHUM1");
                        if (result <= 0) { rtnVal = "일반문진생성시 오류"; }

                        if (!argUCodes.IsNullOrEmpty())
                        {
                            result = comHpcLibBService.InsertByWrtno(argWRTNO, "HIC_RES_SPECIAL");
                            if (result <= 0) { rtnVal = "특수문진생성시 오류"; }
                        }
                    }
                    break;
                case "2":
                    if (HIC_WRTNO_CHECK(argWRTNO, "암").Equals("Y"))
                    {
                        result = comHpcLibBService.InsertByWrtno(argWRTNO, "HIC_CANCER_NEW");
                        if (result <= 0) { rtnVal = "암문진생성시 오류"; }
                    }
                    break;
                case "3":
                    if (HIC_WRTNO_CHECK(argWRTNO, "특수").Equals("Y"))
                    {
                        result = comHpcLibBService.InsertByWrtno(argWRTNO, "HIC_RES_SPECIAL");
                        if (result <= 0) { rtnVal = "특수문진생성시 오류"; }
                    }
                    break;
                default:
                    break;
            }

            //일반2차
            if ((argJONG.Equals("16") || argJONG.Equals("17") || argJONG.Equals("18") ||
                 argJONG.Equals("19") || argJONG.Equals("44") || argJONG.Equals("45") || argJONG.Equals("46")) &&
                HIC_WRTNO_CHECK(argWRTNO, "일반2차").Equals("Y"))
            {
                result = comHpcLibBService.InsertByWrtno(argWRTNO, "HIC_RES_BOHUM2");
                if (result <= 0) { rtnVal = "일반2차문진생성시 오류"; }
            }

            //학생
            if (argJONG.Equals("16") && HIC_WRTNO_CHECK(argWRTNO, "일반2차").Equals("Y"))
            {
                result = comHpcLibBService.InsertByWrtno(argWRTNO, "HIC_SCHOOL_NEW");
                if (result <= 0) { rtnVal = "학생문진생성시 오류"; }
            }

            //구강
            if (argDent.Equals("Y") && HIC_WRTNO_CHECK(argWRTNO, "구강").Equals("Y"))
            {
                result = comHpcLibBService.InsertByWrtno(argWRTNO, "HIC_RES_DENTAL");
                if (result <= 0) { rtnVal = "구강문진생성시 오류"; }
            }

            return rtnVal;
        }

        /// <summary>
        /// 각 Table Rowid 가져오기
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argGBN"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> HIC_WRTNO_CHECK"/>
        public string HIC_WRTNO_CHECK(long argWRTNO, string argGBN)
        {
            string rtnVal = "N";
            string strROWID = string.Empty;

            switch (argGBN)
            {
                case "구강": strROWID = comHpcLibBService.GetTableRowidByWrtno(argWRTNO, "HIC_RES_DENTAL"); break;
                case "검진": strROWID = comHpcLibBService.GetTableRowidByWrtno(argWRTNO, "HIC_RES_BOHUM1"); break;
                case "일반2차": strROWID = comHpcLibBService.GetTableRowidByWrtno(argWRTNO, "HIC_RES_BOHUM2"); break;
                case "특수": strROWID = comHpcLibBService.GetTableRowidByWrtno(argWRTNO, "HIC_RES_SPECIAL"); break;
                case "학생": strROWID = comHpcLibBService.GetTableRowidByWrtno(argWRTNO, "HIC_SCHOOL_NEW"); break;
                case "암": strROWID = comHpcLibBService.GetTableRowidByWrtno(argWRTNO, "HIC_CANCER_NEW"); break;
                default:
                    break;
            }

            if (strROWID.IsNullOrEmpty())
            {
                rtnVal = "Y";
            }

            return rtnVal;
        }

        /// <summary>
        ///  내시경 챠트 테이블 생성
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argDate"></param>
        /// <param name="argPTNO"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> HIC_ENDOCHART_INSERT"/>
        public bool HIC_ENDOCHART_INSERT(long argWRTNO, string argDate, string argPTNO, string argDept, long argPano)
        {
            bool rtnVal = true;
            int result = 0;

            if (argDept == "TO")
            {
                IList<EXAM_DISPLAY_NEW> items = examDisplayNewService.GetItemsInResultHaExCode(argWRTNO);
                if (items == null) { return rtnVal; }

                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].ENDOGUBUN2.To<string>("") == "Y" || items[i].ENDOGUBUN3.To<string>("") == "Y" || items[i].ENDOGUBUN4.To<string>("") == "Y" || items[i].ENDOGUBUN5.To<string>("") == "Y" || items[i].EXCODE.To<string>("") == "TX98")
                    {
                        //외래접수가 없으면 ..
                        if (comHpcLibBService.ChkOpdMaster(argPTNO, argDate, argDept).IsNullOrEmpty())
                        {
                            COMHPC eOS = new COMHPC();

                            if (argDept == "HR")
                            {
                                HIC_JEPSU dHic = hicJepsuService.Read_Jepsu_Wrtno(argWRTNO);

                                eOS.AGE = dHic.AGE.To<long>(0);
                                eOS.PANO = dHic.PANO.To<long>(0);
                                eOS.SDATE = dHic.JEPDATE;
                                eOS.JEPDATE = dHic.JEPDATE;
                                eOS.SNAME = dHic.SNAME;
                                eOS.SEX = dHic.SEX;
                                eOS.LTDCODE = dHic.LTDCODE;
                                eOS.GJJONG = dHic.GJJONG;
                                eOS.PTNO = dHic.PTNO;
                                eOS.DEPTCODE = "HR";
                                eOS.DRCODE = "7101";
                                eOS.JOBSABUN = 111;
                            }
                            else
                            {
                                HEA_JEPSU dHea = heaJepsuService.GetItembyWrtNo(argWRTNO);

                                eOS.AGE = dHea.AGE.To<long>(0);
                                eOS.PANO = dHea.PANO.To<long>(0);
                                eOS.SDATE = dHea.SDATE;
                                eOS.JEPDATE = dHea.JEPDATE;
                                eOS.SNAME = dHea.SNAME;
                                eOS.SEX = dHea.SEX;
                                eOS.LTDCODE = dHea.LTDCODE;
                                eOS.GJJONG = dHea.GJJONG;
                                eOS.PTNO = dHea.PTNO;
                                eOS.DEPTCODE = "TO";
                                eOS.DRCODE = "7102";
                                eOS.JOBSABUN = 222;
                            }

                            if (!eOS.PTNO.IsNullOrEmpty())
                            {
                                result = comHpcLibBService.InsertOpdMasterByHicJepsu(eOS);
                                if (result <= 0) { return false; }
                            }
                        }

                        if (comHpcLibBService.GetRowidEndoChartByPtno(argPTNO, argDate).IsNullOrEmpty())
                        {
                            result = comHpcLibBService.InsertEndoChart(argPTNO, argDate);
                            if (result <= 0) { return false; }
                        }
                    }
                }

                //보류자 검사조회 후 OPD_MASTER 추가(2021-04-16)
                if (heaResvExamService.GetCountbyPaNoGbExam(argPano) > 0)
                {
                    if (comHpcLibBService.ChkOpdMaster(argPTNO, DateTime.Now.ToShortDateString(), argDept).IsNullOrEmpty())
                    {

                        COMHPC eOS = new COMHPC();
                        HEA_JEPSU dHea = heaJepsuService.GetItembyWrtNo(argWRTNO);

                        eOS.AGE = dHea.AGE.To<long>(0);
                        eOS.PANO = dHea.PANO.To<long>(0);
                        eOS.SDATE = dHea.SDATE;
                        //보류자는 JEPDATE 당일로
                        eOS.JEPDATE = DateTime.Now.ToShortDateString();
                        eOS.SNAME = dHea.SNAME;
                        eOS.SEX = dHea.SEX;
                        eOS.LTDCODE = dHea.LTDCODE;
                        eOS.GJJONG = dHea.GJJONG;
                        eOS.PTNO = dHea.PTNO;
                        eOS.DEPTCODE = "TO";
                        eOS.DRCODE = "7102";
                        eOS.JOBSABUN = 222;
                        if (!eOS.PTNO.IsNullOrEmpty())
                        {
                            result = comHpcLibBService.InsertOpdMasterByHicJepsu(eOS);
                            if (result <= 0) { return false; }
                        }
                    }

                    if (comHpcLibBService.GetRowidEndoChartByPtno(argPTNO, DateTime.Now.ToShortDateString()).IsNullOrEmpty())
                    {
                        result = comHpcLibBService.InsertEndoChart(argPTNO, DateTime.Now.ToShortDateString());
                        if (result <= 0) { return false; }
                    }
                }
            }

            else if (argDept == "HR")
            {
                IList<EXAM_DISPLAY_NEW> items = examDisplayNewService.GetItemsInResultExCode(argWRTNO);
                if (items == null) { return rtnVal; }

                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].ENDOGUBUN2.To<string>("") == "Y" || items[i].ENDOGUBUN3.To<string>("") == "Y" || items[i].ENDOGUBUN4.To<string>("") == "Y" || items[i].ENDOGUBUN5.To<string>("") == "Y" || items[i].EXCODE.To<string>("") == "TX98")
                    {
                        //외래접수가 없으면 ..
                        if (comHpcLibBService.ChkOpdMaster(argPTNO, argDate, argDept).IsNullOrEmpty())
                        {
                            COMHPC eOS = new COMHPC();

                            if (argDept == "HR")
                            {
                                HIC_JEPSU dHic = hicJepsuService.Read_Jepsu_Wrtno(argWRTNO);

                                eOS.AGE = dHic.AGE.To<long>(0);
                                eOS.PANO = dHic.PANO.To<long>(0);
                                eOS.SDATE = dHic.JEPDATE;
                                eOS.JEPDATE = dHic.JEPDATE;
                                eOS.SNAME = dHic.SNAME;
                                eOS.SEX = dHic.SEX;
                                eOS.LTDCODE = dHic.LTDCODE;
                                eOS.GJJONG = dHic.GJJONG;
                                eOS.PTNO = dHic.PTNO;
                                eOS.DEPTCODE = "HR";
                                eOS.DRCODE = "7101";
                                eOS.JOBSABUN = 111;
                            }
                            else
                            {
                                HEA_JEPSU dHea = heaJepsuService.GetItembyWrtNo(argWRTNO);

                                eOS.AGE = dHea.AGE.To<long>(0);
                                eOS.PANO = dHea.PANO.To<long>(0);
                                eOS.SDATE = dHea.SDATE;
                                eOS.JEPDATE = dHea.JEPDATE;
                                eOS.SNAME = dHea.SNAME;
                                eOS.SEX = dHea.SEX;
                                eOS.LTDCODE = dHea.LTDCODE;
                                eOS.GJJONG = dHea.GJJONG;
                                eOS.PTNO = dHea.PTNO;
                                eOS.DEPTCODE = "TO";
                                eOS.DRCODE = "7102";
                                eOS.JOBSABUN = 222;
                            }

                            if (!eOS.PTNO.IsNullOrEmpty())
                            {
                                result = comHpcLibBService.InsertOpdMasterByHicJepsu(eOS);
                                if (result <= 0) { return false; }
                            }
                        }

                        if (comHpcLibBService.GetRowidEndoChartByPtno(argPTNO, argDate).IsNullOrEmpty())
                        {
                            result = comHpcLibBService.InsertEndoChart(argPTNO, argDate);
                            if (result <= 0) { return false; }
                        }
                    }
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// 판독여부 점검 HIC_XRAY_RESULT
        /// </summary>
        /// <param name="argPTNO"></param>
        /// <param name="argJepDate"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> Xray_Read_Check"/>
        public bool Xray_Read_Check(string argPTNO, string argJepDate)
        {
            bool rtnVal = false;
            string[] strCodes = new string[] { "A142" };

            //접수에 방사선 제외를 한경우 점검
            IList<EXAM_DISPLAY_NEW> item = examDisplayNewService.GetItemsInJepsuResult(argPTNO, argJepDate, strCodes);
            if (item.Count == 0) { rtnVal = true; return rtnVal; }

            HIC_XRAY_RESULT item2 = hicXrayResultService.GetItemByPtnoJepDate(argPTNO, DateTime.Parse(argJepDate).AddDays(-30).ToShortDateString(), DateTime.Parse(argJepDate).AddDays(30).ToShortDateString());
            if (item2 == null) { rtnVal = true; return rtnVal; }

            rtnVal = false;

            if (!item2.READTIME1.IsNullOrEmpty()) { rtnVal = true; }

            return rtnVal;
        }

        /// <summary>
        /// 구강검진 현재 상태를 표시함
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> Dental_Status_Check"/>
        public string Dental_Status_Check(long argWRTNO)
        {
            string rtnVal = string.Empty;

            HIC_RES_DENTAL item = hicResDentalService.GetItemByWrtno(argWRTNO);

            if (item == null)
            {
                rtnVal = "X";
            }
            else if (item.OPDDNT.IsNullOrEmpty())
            {
                rtnVal = "문진X";
            }
            //else if (item.GBPRINT.Equals("Y"))
            //{
            //    rtnVal = "인쇄";
            //}
            else if (!item.GBPRINT.IsNullOrEmpty())
            {
                if (item.GBPRINT == "Y")
                {
                    rtnVal = "인쇄";
                }
            }
            else if (item.RES_RESULT.IsNullOrEmpty())
            {
                rtnVal = "판정X";
            }
            else if (item.PANJENGDATE.IsNullOrEmpty())
            {
                rtnVal = "판정X";
            }
            else if (item.PANJENGDRNO == 0)
            {
                rtnVal = "판정X";
            }
            else
            {
                rtnVal = "판정";
            }

            return rtnVal;
        }

        /// <summary>
        /// 구강검진 문진결과로 문진표평가 및 종합판정,결과해석을 업데이트
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="ArgPanDate"></param>
        /// <seealso cref="HcMain.bas> Munjin_Result_Update"/>
        public void Munjin_Result_Update(long argWRTNO, string ArgPanDate)
        {
            string strPan = string.Empty;
            string strRES2 = string.Empty;
            string[] strMUN = new string[7];
            string[] strJochi = new string[8];
            StringBuilder strResult = new StringBuilder();

            HIC_RES_DENTAL item = hicResDentalService.GetItemByWrtno(argWRTNO);
            if (item.IsNullOrEmpty()) { return; }

            strRES2 = item.RES_RESULT;

            //---------( 1. 치과 병력 문제 )------------
            //(1) 치과병원에 가신적이 있습니까?
            if (item.OPDDNT.Equals("2")) { strMUN[1] = "2"; }
            //(2) 현재 당뇨병을 앓고 계십니까
            else if (item.T_JILBYUNG1.Equals("1")) { strMUN[1] = "2"; }
            //(3) 현재 심혈관질환을 앓고 계십니까
            else if (item.T_JILBYUNG2.Equals("1")) { strMUN[1] = "2"; }
            //(4) 최근 3개월 동안 치아나 잇몸 문제..
            else if (item.T_FUNCTION1.Equals("1")) { strMUN[1] = "2"; }
            //(5) 최근 3개월 동안 치아가 쑤시거나..
            else if (item.T_STAT1.Equals("1")) { strMUN[1] = "2"; }
            //(6) 최근 3개월 동안 잇몸이 아프거나..
            else if (item.T_STAT2.Equals("1")) { strMUN[1] = "2"; }
            else { strMUN[1] = "1"; }

            //---------( 2. 구강건강인식도 )------------
            //(7) 스스로 생각하실 때 치아와 잇몸...
            if (string.Compare(item.DNTSTATUS, "3") >= 0) { strMUN[2] = "2"; }
            else { strMUN[2] = "1"; }

            //---------( 3. 구강위생 )------------
            //(8) 치아 닦는 방법을 치과나 보건소...
            if (item.T_HABIT5.Equals("2")) { strMUN[3] = "2"; }
            //(9) 어제 하루 동안 치아를 몇 번...
            else if (item.T_HABIT2 <= 1) { strMUN[3] = "2"; }
            //(10) 최근 일주일 동안 잠자기 직전..
            else if (string.Compare(item.T_HABIT6, "2") >= 0) { strMUN[3] = "2"; }
            //(11) 최근 일주일 동안 치실/치간솔..
            else if (string.Compare(item.T_HABIT4, "3") >= 0) { strMUN[3] = "2"; }
            else { strMUN[3] = "1"; }

            //---------( 4. 불소이용 )------------
            //(12) 현재 사용중인 치약에 불소가..
            if (string.Compare(item.T_HABIT7, "2") >= 0) { strMUN[4] = "2"; }
            else { strMUN[4] = "1"; }

            //---------( 5. 설탕섭취 )------------
            //(13) 평소 간식 섭취..
            if (string.Compare(item.T_HABIT8, "3") >= 0) { strMUN[5] = "2"; }
            //(14) 평소 탄산/청량음료..
            else if (string.Compare(item.T_HABIT9, "3") >= 0) { strMUN[5] = "2"; }
            else { strMUN[5] = "1"; }

            //---------( 6. 흡연 )------------
            //(15) 담배를 피우십니까
            if (item.T_HABIT1.Equals("2")) { strMUN[6] = "2"; }
            else { strMUN[6] = "1"; }

            //문진 결과로 필요 구강보건교육 설정
            if (strMUN[3].Equals("2")) { strJochi[2] = "1"; } //조치(구강위생)
            if (strMUN[4].Equals("2")) { strJochi[3] = "1"; } //조치(불소이용)
            if (strMUN[5].Equals("2")) { strJochi[1] = "1"; } //조치(설탕섭취)

            //----------------------------------------------------
            //  구강검사 결과로 사후관리 및 결과해석, 판정을 함
            //----------------------------------------------------
            strPan = "1";
            strResult.Clear();

            //(1)우식치아
            if (VB.Mid(strRES2, 1, 1).Equals("2"))
            {
                strJochi[6] = "1"; //치아우식치료
                strPan = "4";
                strResult.Append("치과에서 우식증 치료를 받으시길 바랍니다.");
            }
            //(2)인접면 우식 의심치아
            if (VB.Mid(strRES2, 2, 1).Equals("2"))
            {
                strJochi[4] = "1"; //정밀구강검진
                if (string.Compare(strPan, "3") < 0) { strPan = "3"; }
                strResult.Append("육안 관찰 어려운 충치(치아사이)가 있을수 있으므로 치과 방사선 사진 촬영을 권고합니다.");
            }
            //(3)수복치아
            if (VB.Mid(strRES2, 3, 1).Equals("2"))
            {
                strJochi[5] = "1"; //전문가 구강위생..
                if (string.Compare(strPan, "2") < 0) { strPan = "2"; }
                strResult.Append("전문가 구강위생관리 및 정기적 구강검진을 받으시길 바랍니다.");
            }
            //(4)상실치아
            if (VB.Mid(strRES2, 4, 1).Equals("2"))
            {
                strJochi[6] = "1"; //치아우식치료
                if (string.Compare(strPan, "4") < 0) { strPan = "4"; }
                strResult.Append("치과에서 보철치료를 받으시길 바랍니다.");
            }
            //(5)치은염증(경증)
            if (VB.Mid(strRES2, 5, 1).Equals("2"))
            {
                strJochi[5] = "1"; //전문가 구강위생..
                if (string.Compare(strPan, "3") < 0) { strPan = "3"; }
                strResult.Append("치아에 염증이 있으므로 치주 전문 치료를 받으시길 바랍니다.");
                //치은염증(중증)
            }
            else if (VB.Mid(strRES2, 5, 1).Equals("3"))
            {
                strJochi[7] = "1"; //치주치료필요
                if (string.Compare(strPan, "4") < 0) { strPan = "4"; }
                strResult.Append("잇몸에 염증이 심하므로 치주 전문 상담 및 치료를 받으시길 바랍니다.");
            }
            //(6)치석(경증)
            if (VB.Mid(strRES2, 6, 1).Equals("2"))
            {
                strJochi[5] = "1"; //전문가 구강위생..
                if (string.Compare(strPan, "3") < 0) { strPan = "3"; }
                strResult.Append("치과에서 치석제거를 받으시기 바랍니다.");
                //치석(중증)
            }
            else if (VB.Mid(strRES2, 6, 1).Equals("3"))
            {
                strJochi[7] = "1"; //치주치료필요
                if (string.Compare(strPan, "4") < 0) { strPan = "4"; }
                strResult.Append("치과에서 치석제거 및 치근활택술 치료를 받으시길 바랍니다.");
            }

            StringBuilder strRES1 = new StringBuilder();
            for (int i = 1; i <= 6; i++)
            {
                strRES1.Append(strMUN[i]);
            }

            StringBuilder strRES3 = new StringBuilder();
            for (int i = 1; i <= 7; i++)
            {
                strRES3.Append(strJochi[i]);
            }

            HIC_RES_DENTAL item2 = new HIC_RES_DENTAL();

            item2.PANJENGDATE = ArgPanDate;
            item2.T_PANJENG1 = strPan;
            item2.RES_MUNJIN = strRES1.ToString();
            item2.RES_RESULT = strRES2;
            item2.RES_JOCHI = strRES3.ToString();
            item2.SANGDAM = strResult.ToString();

            clsDB.setBeginTran(clsDB.DbCon);

            int result = hicResDentalService.MunjinResultUpDate(item2);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        public bool UPDATE_SPC_MUNJIN(HIC_JEPSU nHJ, FpSpread ssJik, List<READ_SUNAP_ITEM> lstRSI, HIC_RES_SPECIAL nHRS)
        {
            List<string> lstUCodes = new List<string>();
            List<string> lstUCodes_Del = new List<string>();

            string strOldGong1 = "";
            string strOldMCode1 = "";
            string strOldYear1 = "";
            string strOldDayTime1 = "";

            //string strOldGong2 = "";
            //string strOldMCode2 = "";
            //string strOldYear2 = "";
            //string strOldDayTime2 = "";

            //string strOldGong3 = "";
            //string strOldMCode3 = "";
            //string strOldYear3 = "";
            //string strOldDayTime3 = "";

            try
            {
                //string strRowid = comHpcLibBService.GetRowidByTableWRTNO(nHJ.WRTNO, "KOSMOS_PMPA.HIC_RES_SPECIAL");

                for (int i = 0; i < lstRSI.Count; i++)
                {
                    if (lstRSI[i].RowStatus != ComBase.Mvc.RowStatus.Delete)
                    {
                        if (!lstRSI[i].UCODE.IsNullOrEmpty())
                        {
                            lstUCodes.Add(lstRSI[i].UCODE);
                        }
                    }
                    else
                    {
                        if (!lstRSI[i].UCODE.IsNullOrEmpty())
                        {
                            lstUCodes_Del.Add(lstRSI[i].UCODE);
                        }
                    }
                }

                //물질별 판정레코드 형성
                if (lstUCodes.Count > 0)
                {
                    for (int i = 0; i < lstUCodes.Count; i++)
                    {
                        if (lstUCodes[i] == "999") { lstUCodes[i] = "ZZZ"; }

                        if (hicSpcPanjengService.GetRowidByWrtnoMCode(nHJ.WRTNO, lstUCodes[i]).IsNullOrEmpty())
                        {
                            string strUCode = hicMcodeService.GetUCodebyCode(lstUCodes[i]);

                            HIC_SPC_PANJENG nHSP = new HIC_SPC_PANJENG
                            {
                                WRTNO = nHJ.WRTNO,
                                JEPDATE = nHJ.JEPDATE,
                                LTDCODE = nHJ.LTDCODE,
                                MCODE = lstUCodes[i],
                                UCODE = strUCode
                            };

                            if (hicSpcPanjengService.Insert(nHSP) <= 0)
                            {
                                return false;
                            }
                        }
                    }
                }

                //제외된 물질 판정에서 삭제
                if (lstUCodes_Del.Count > 0)
                {
                    if (!hicSpcPanjengService.UpDateDelDateByWrtnoMCodeIN(nHJ.WRTNO, lstUCodes_Del))
                    {
                        return false;
                    }
                }

                nHRS.WRTNO = nHJ.WRTNO;
                nHRS.UCODECNT = lstUCodes.Count;
                nHRS.UCODENAME = lstUCodes.ToString();
                nHRS.OLDGONG1 = ssJik.ActiveSheet.Cells[0, 0].Text;
                nHRS.OLDMCODE1 = ssJik.ActiveSheet.Cells[0, 1].Text;
                nHRS.OLDYEAR1 = ssJik.ActiveSheet.Cells[0, 4].Text.To<long>();
                nHRS.OLDDAYTIME1 = ssJik.ActiveSheet.Cells[0, 5].Text.To<long>();

                if (nHJ.GJJONG != "21" && nHJ.GJJONG != "32")
                {
                    if (nHRS.GONGJENG.IsNullOrEmpty()) { MessageBox.Show("현작업공정이 공란입니다.", "문진저장불가"); return false; }
                    if (nHRS.IPSADATE.IsNullOrEmpty()) { MessageBox.Show("입사입자가 공란입니다.", "문진저장불가"); return false; }
                    if (nHRS.JENIPDATE.IsNullOrEmpty()) { MessageBox.Show("현직전입입자가 공란입니다.", "문진저장불가"); return false; }
                    if (nHRS.GBSPC.IsNullOrEmpty()) { MessageBox.Show("특검종류가 공란입니다.", "문진저장불가"); return false; }
                }

                //자료에 오류가 있는지 점검
                if (nHRS.GBOHMS == "Y" && !nHRS.OLDMCODE1.IsNullOrEmpty() && (nHRS.OLDYEAR1 == 0 || nHRS.OLDDAYTIME1 == 0))
                {
                    MessageBox.Show("과거직력① 오류.", "문진저장불가");
                    return false;
                }

                //변경한 자료를 DB에 UPDATE
                if (!hicResSpecialService.UpDateMunjinbyItem(nHRS)) { return false; }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nHJ"></param>
        /// <param name="argHeight"></param>
        /// <param name="argWeight"></param>
        /// <param name="argGubun (HIC : 일반검진 / HEA : 종검)"></param>
        /// <returns></returns>
        public bool UPDATE_SCHOOL_MUNJIN(HIC_JEPSU nHJ, string argHeight, string argWeight, string argGubun = "")
        {
            try
            {

                if (hicSchoolNewService.GetRowIdbyWrtNo(nHJ.WRTNO).IsNullOrEmpty())
                {
                    hicSchoolNewService.InsertWrtNo(nHJ.WRTNO);
                    return true;

                }

                HIC_SCHOOL_NEW nHSN = new HIC_SCHOOL_NEW
                {
                    SEX = nHJ.SEX,
                    SNAME = nHJ.SNAME,
                    JUMIN = nHJ.JUMINNO,
                    JUMIN2 = nHJ.JUMINNO2,
                    SDATE = nHJ.JEPDATE,
                    LTDCODE = nHJ.LTDCODE,
                    CLASS = nHJ.CLASS,
                    BAN = nHJ.BAN,
                    BUN = nHJ.BUN,
                    GBN = nHJ.GBN,
                    WRTNO = nHJ.WRTNO
                };

                //변경한 자료를 DB에 UPDATE
                if (!hicSchoolNewService.UpDateItembyItem(nHSN)) { return false; }

                //결과입력에 입력된 키/몸무게 Update
                hicResultService.UpdateResultbyWrtNoExCode(argHeight, nHJ.WRTNO, clsType.User.IdNumber.To<long>(), "A101"); //키
                hicResultService.UpdateResultbyWrtNoExCode(argWeight, nHJ.WRTNO, clsType.User.IdNumber.To<long>(), "A102"); //몸무게

                Biman_Gesan(nHJ.WRTNO, argGubun);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 청구를 하였어도 재판정이 가능한지 점검
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> RePanjeng_WRTNO_Check"/>
        public bool RePanjeng_WRTNO_Check(long argWRTNO)
        {
            bool rtnVal = false;

            HIC_BCODE list = hicBcodeService.Read_Hic_BCode("HIC_재판정접수번호", argWRTNO.To<string>());

            if (!list.IsNullOrEmpty())
            {
                if (list.CODE.To<long>() == argWRTNO)
                {
                    rtnVal = true;
                }
            }
            else
            {
                rtnVal = false;
            }

            //기존 혈중카르복시헤모글로빈 결과 점검로직은 HIC_RESULT_H827 테이블을 사용하지 않아 컨버전 안함.
            #region 기존로직
            //'혈중카르복시헤모글로빈
            //SQL = "SELECT a.WRTNO,b.Result "
            //SQL = SQL & " FROM HIC_RESULT_H827 a,HIC_RESULT b "
            //SQL = SQL & "WHERE a.WRTNO=" & ArgWRTNO & " "
            //SQL = SQL & "  AND a.WRTNO=b.WRTNO(+) "
            //SQL = SQL & "  AND b.ExCode='H827' "
            //Call AdoOpenSet(RsRepan, SQL, , , False)
            //If RowIndicator > 0 Then
            //    Select Case Trim(AdoGetString(RsRepan, "Result", 0))
            //        Case "", ".", "미실시":
            //        Case Else: RePanjeng_WRTNO_Check = True
            //    End Select
            //End If
            //Call AdoCloseSet(RsRepan) 
            #endregion

            return rtnVal;
        }

        /// <summary>
        /// 2차 검진자중 당뇨,고혈압 재검만 있는지 점검(True=당뇨,고혈압 재검만 있음,False=아님)
        /// argDMExam은 기초코드의 자료사전에 있는 자료를 참조함
        /// </summary>
        /// <param name="argDMExam"></param>
        /// <param name="argExam"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> RePanjeng_WRTNO_Check"/>
        public bool GET_DangnyoExam_Check(string argDMExam, string argExam)
        {
            string strTemp = string.Empty;

            if (argExam.IsNullOrEmpty())
            {
                return false;
            }

            int nCNT = (int)VB.L(argExam, ",");

            for (int i = 1; i <= nCNT; i++)
            {
                strTemp = VB.Pstr(argExam, ",", i).Trim();

                if (!strTemp.IsNullOrEmpty())
                {
                    if (VB.InStr(argDMExam, strTemp + ",") == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 2015-03-27 특수검진 결과지를 1차,2차 분리하기 위해 추가
        /// 1차 판정완료 후 2차 판정대상을 별도 보관하는 작업
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> UPDATE_SPC_PanHis"/>
        public void UPDATE_SPC_PanHis(long argWRTNO)
        {
            //1차검진이 아니면 처리를 안함
            HIC_JEPSU item = hicJepsuService.Read_Jepsu_Wrtno(argWRTNO);

            //취급물질이 없으면 작업 제외
            if (item.UCODES.IsNullOrEmpty()) { return; }
            //2차는 제외
            if (item.GJCHASU.Equals("2")) { return; }
            //암검진,방사선 등은 제외
            if (item.GJJONG.Equals("31") || item.GJJONG.Equals("35")) { return; }
            if (string.Compare(item.GJJONG, "49") >= 0) { return; }

            //1,2차 구분이 NULL이면 1차로 처리
            int result = hicSpcPanjengService.ChasuUpDate(argWRTNO);
            if (result < 0)
            {
                MessageBox.Show("특수검진 2차대상 별도 보관 오류(1)", "확인");
                return;
            }

            //기존 자료가 있으면 삭제함
            result = comHpcLibBService.DeleteSpcPanHis(argWRTNO);
            if (result < 0)
            {
                MessageBox.Show("특수검진 2차대상 별도 보관 오류(2)", "확인");
                return;
            }

            //1차 판정이 R,U인것을 HIC_SPC_PanHis에 보관
            result = comHpcLibBService.InsertSpcPanhisBySelSpcPan(argWRTNO);
            if (result < 0)
            {
                MessageBox.Show("특수검진 2차대상 별도 보관 오류(3)", "확인");
                return;
            }
        }

        /// <summary>
        /// 2015-06-27 PFT결과 자동 판정(01.정상)
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> PFT_Auto_Panjeng"/>
        public string PFT_Auto_Panjeng(long argWRTNO)
        {
            string rtnVal = string.Empty;

            //PFT 검사결과 표시
            IList<Dictionary<string, object>> D = comHpcLibBService.GetItemHicResPft(argWRTNO);

            for (int i = 0; i < D.Count; i++)
            {
                if (D[i]["FVC_PRED"].To<long>() >= 80 && D[i]["FEV1_FVC_MEAS"].To<long>() >= 70)
                {
                    rtnVal = "01";
                    break;
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 학생검진 비만도 계산 및 구분
        /// </summary>
        /// <param name="argWeight"></param>
        /// <param name="argHeight"></param>
        /// <param name="argSex"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> CALC_Biman_Rate"/>
        public string CALC_Biman_Rate(double argWeight, double argHeight, string argSex)
        {
            string rtnVal = string.Empty;
            double nValue = 0;

            //기존 하드코딩 된 소스 비만도계산 공식으로 변경함
            //표준체중 구하기 = (키 - 100) * 0.9 
            //상대체중 구하기 = 몸무게 / 표준체중 * 100
            // 120 이하 : 정상   
            // 121 ~ 130 이하 : 경도비만 
            // 131 ~ 149 이하 : 중등도비만 
            // 150 이상 : 고도비만

            nValue = Math.Truncate(argWeight / ((argHeight - 100) * 0.9) * 100);

            if (nValue <= 120)
            {
                rtnVal = "1.정상";
            }
            else if (nValue >= 121 && nValue <= 130)
            {
                rtnVal = "2.경도";
            }
            else if (nValue >= 131 && nValue <= 149)
            {
                rtnVal = "3.중등도";
            }
            else if (nValue >= 150)
            {
                rtnVal = "4.고도";
            }

            return rtnVal;
        }

        /// <summary>
        /// 야간작업 소견코드로 표적장기분류 설정값
        /// </summary>
        /// <param name="argSogenCD"></param>
        /// <param name="argSogenRemark"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain.bas> GET_Night_Pyojanggi"/>
        public string GET_Night_OrganTarget(string argSogenCD, string argSogenRemark)
        {
            string rtnVal = string.Empty;

            if (argSogenCD.Equals("N01B") || argSogenCD.Equals("N02C") || argSogenCD.Equals("N02D"))
            {
                rtnVal = "1"; //심혈관계-고혈압
            }
            else if (argSogenCD.Equals("N01E") || argSogenCD.Equals("N04C") || argSogenCD.Equals("N04D"))
            {
                rtnVal = "2"; //심혈관계-이상지질혈증
            }
            else if (argSogenCD.Equals("N01F") || argSogenCD.Equals("N03C") || argSogenCD.Equals("N03D") || argSogenCD.Equals("NO4E"))
            {
                rtnVal = "3"; //심혈관계-당뇨병
            }
            else if (argSogenCD.Equals("N01G") || argSogenCD.Equals("N05C") || argSogenCD.Equals("N05D"))
            {
                rtnVal = "4"; //신경계-수면장애
            }
            else if (argSogenCD.Equals("N01H") || argSogenCD.Equals("N05E") || argSogenCD.Equals("N06D"))
            {
                rtnVal = "6"; //위장관계
            }
            else if (argSogenCD.Equals("N01I") || argSogenCD.Equals("NO4F"))
            {
                rtnVal = "5"; //내분비계-유방암
            }
            else
            {
                rtnVal = "9"; //기타
            }

            //야간작업 정상
            if (argSogenCD.Equals("001"))
            {
                //고혈압
                if (VB.InStr(argSogenRemark, "혈압") > 0) { rtnVal = "1"; }
                //이상지질
                if (VB.InStr(argSogenRemark, "이상지질") > 0) { rtnVal = "2"; }
                if (VB.InStr(argSogenRemark, "콜레") > 0) { rtnVal = "2"; }
                //당뇨병
                if (VB.InStr(argSogenRemark, "당뇨") > 0) { rtnVal = "3"; }
                if (VB.InStr(argSogenRemark, "혈당") > 0) { rtnVal = "3"; }
                //수면장애
                if (VB.InStr(argSogenRemark, "수면") > 0) { rtnVal = "4"; }
            }

            return rtnVal;
        }

        /// <summary>
        /// 종검 자동판정 문구
        /// </summary>
        /// <param name="argGUBUN"></param>
        /// <param name="argCHK"></param>
        /// <returns></returns>
        /// <seealso cref="HaMain.bas> Auto_PanRemark"/>
        public string Auto_PanRemark(long argGUBUN, string argCHK)
        {
            string rtnVal = string.Empty;
            string strTitle = string.Empty;

            string strGubun = VB.Pstr(argGUBUN.To<string>(), "@", 1);

            long nSabun = clsType.User.IdNumber.To<long>();

            HEA_RESULTWARD item = heaResultwardService.GetWardNameByCode("01", nSabun, strGubun);
            if (!item.IsNullOrEmpty())
            {
                strTitle = " < " + item.WARDNAME + " > " + ComNum.VBLF;
            }

            HEA_RESULTWARD item2 = heaResultwardService.GetWardNameByCode("02", nSabun, argGUBUN.To<string>().Trim());
            if (!item.IsNullOrEmpty())
            {
                if (!argCHK.IsNullOrEmpty())
                {
                    rtnVal = strTitle;
                }

                rtnVal = rtnVal + item2.WARDNAME;
            }

            return rtnVal;
        }

        /// <summary>
        /// 종검접수 진행상황
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <returns></returns>
        /// <seealso cref="HaMain.bas> READ_JepsuSTS_GBN"/>
        public string READ_JepsuSTS_GBN(long argWRTNO)
        {
            string rtnVal = string.Empty;

            string strSTS = heaJepsuService.GetGbStsByWrtno(argWRTNO);

            if (!strSTS.IsNullOrEmpty())
            {
                switch (strSTS)
                {
                    case "0": rtnVal = "예약"; break;
                    case "1": rtnVal = "수진등록"; break;
                    case "2": rtnVal = "결과입력중"; break;
                    case "3": rtnVal = "결과입력완료"; break;
                    case "5": rtnVal = "가판정완료"; break;
                    case "9": rtnVal = "판정완료"; break;
                    default:
                        rtnVal = "";
                        break;
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 내시경 입력정보 체크
        /// </summary>
        /// <param name="argPtno"></param>
        /// <param name="argBDate"></param>
        /// <param name="argRDate"></param>
        /// <returns></returns>
        /// <seealso cref="HaMain.bas> READ_ENDO_CHART_CHK"/>
        public string READ_ENDO_CHART_CHK(string argPtno, DateTime argBDate, DateTime argRDate)
        {
            StringBuilder rtnVal = null;

            if (argBDate.IsNullOrEmpty() || argRDate.IsNullOrEmpty())
            {
                rtnVal.Append("처방일자 혹은 예약일자 누락.");
            }
            else
            {
                Dictionary<string, object> EndoJupMst = comHpcLibBService.GetOneEndoMstByPtno(argPtno, argBDate.ToShortDateString(), argRDate.ToShortDateString());

                if (!EndoJupMst.IsNullOrEmpty())
                {
                    if (EndoJupMst["GBCON_2"].ToString().Equals("Y") ||
                        EndoJupMst["GBCON_3"].ToString().Equals("Y") ||
                        EndoJupMst["GBCON_4"].ToString().Equals("Y"))
                    {
                        rtnVal.Append("내시경 입력정보!! \r\n");
                    }

                    //midazolam
                    if (EndoJupMst["GBCON_2"].ToString().Equals("Y"))
                    {
                        rtnVal.Append("Midazolam >> ");
                        rtnVal.Append(EndoJupMst["GBCON_21"]);
                        rtnVal.Append("mg ");
                        rtnVal.Append(EndoJupMst["GBCON_22"]);
                        rtnVal.Append("\r\n\r\n");
                    }

                    //propofol
                    if (EndoJupMst["GBCON_3"].ToString().Equals("Y"))
                    {
                        rtnVal.Append("Propofol >> ");
                        rtnVal.Append(EndoJupMst["GBCON_31"]);
                        rtnVal.Append("mg ");
                        rtnVal.Append(EndoJupMst["GBCON_32"]);
                        rtnVal.Append("\r\n\r\n");
                    }

                    //pethidine
                    if (EndoJupMst["GBCON_4"].ToString().Equals("Y"))
                    {
                        rtnVal.Append("Pethidine >> ");
                        rtnVal.Append(EndoJupMst["GBCON_41"]);
                        rtnVal.Append("mg ");
                        rtnVal.Append(EndoJupMst["GBCON_42"]);
                        rtnVal.Append("\r\n\r\n");
                    }
                }
            }

            if (!rtnVal.IsNullOrEmpty())
            {
                return rtnVal.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 내시경 사용약물 기록지에 Set
        /// </summary>
        /// <param name="argPtno"></param>
        /// <param name="argBDate"></param>
        /// <param name="argRDate"></param>
        /// <returns></returns>
        /// <seealso cref="HaMain.bas> READ_ENDO_CHART_SET"/>
        public string READ_ENDO_CHART_SET(string argPtno, DateTime argBDate, DateTime argRDate)
        {
            string rtnVal1 = string.Empty;
            string rtnVal2 = string.Empty;
            string rtnVal3 = string.Empty;

            if (argBDate.IsNullOrEmpty() || argRDate.IsNullOrEmpty())
            {
                return "";
            }
            else
            {
                IList<Dictionary<string, object>> EndoJupMst = comHpcLibBService.GetListEndoMstByPtno(argPtno, argBDate.ToShortDateString(), argRDate.ToShortDateString());

                if (EndoJupMst.Count > 0)
                {
                    for (int i = 0; i < EndoJupMst.Count; i++)
                    {
                        //midazolam
                        if (EndoJupMst[i]["GBCON_2"].ToString().Equals("Y"))
                        {
                            rtnVal1 = EndoJupMst[i]["GBCON_21"].ToString() + ";";
                        }
                        else
                        {
                            if (rtnVal1.IsNullOrEmpty()) { rtnVal1 = ";"; }
                        }

                        //propofol
                        if (EndoJupMst[i]["GBCON_3"].ToString().Equals("Y"))
                        {
                            rtnVal2 = EndoJupMst[i]["GBCON_31"].ToString() + ";";
                        }
                        else
                        {
                            if (rtnVal2.IsNullOrEmpty()) { rtnVal2 = ";"; }
                        }

                        //pethidine
                        if (EndoJupMst[i]["GBCON_4"].ToString().Equals("Y"))
                        {
                            rtnVal3 = EndoJupMst[i]["GBCON_41"].ToString() + ";";
                        }
                        else
                        {
                            if (rtnVal3.IsNullOrEmpty()) { rtnVal3 = ";"; }
                        }
                    }
                }
            }

            return rtnVal1 + rtnVal2 + rtnVal3;
        }

        /// <summary>
        /// 회사별 특정일자 가예약 Update
        /// </summary>
        /// <param name="ArgLtdCode"></param>
        /// <param name="argDate"></param>
        /// <seealso cref="HaMain.bas> Update_GaResvLtd"/>
        public void Update_GaResvLtd(long ArgLtdCode, string argDate)
        {
            int result = 0;
            int nCNT1 = 0;
            int nCNT2 = 0;
            int nCNT3 = 0;
            int nCNT4 = 0;
            string strExcode = "00";        //종검
            string strYoil = "1";

            ComFunc cF = new ComFunc();

            if (ArgLtdCode.IsNullOrEmpty()) { return; }

            try
            {
                //(1) 접수인원 및 잔여인원을 Clear
                result = heaResvLtdService.UpDateInwonClear(ArgLtdCode, argDate);

                //(4) 종합건진 접수/예약인원을 업데이트
                HEA_JEPSU item = heaJepsuService.GetSumAmPmCount(argDate, ArgLtdCode);

                if (item.IsNullOrEmpty())
                {
                    return;
                }
                else
                {
                    nCNT1 = item.AMCNT.To<int>();
                    nCNT2 = item.PMCNT.To<int>();

                    strExcode = "00";
                    switch (cF.READ_YOIL(clsDB.DbCon, argDate))
                    {
                        case "월요일": strYoil = "1"; break;
                        case "화요일": strYoil = "2"; break;
                        case "수요일": strYoil = "3"; break;
                        case "목요일": strYoil = "4"; break;
                        case "금요일": strYoil = "5"; break;
                        case "토요일": strYoil = "6"; break;
                        case "일요일": strYoil = "7"; break;
                        default: break;
                    }

                    HEA_RESV_LTD list = heaResvLtdService.GetInwonAmPm(argDate, ArgLtdCode, strExcode);

                    HEA_RESV_LTD data = new HEA_RESV_LTD
                    {
                        SDATE = argDate,
                        GUBUN = strExcode,
                        LTDCODE = ArgLtdCode,
                        ENTSABUN = clsType.User.IdNumber.To<long>(),
                        AMINWON = 0,
                        PMINWON = 0,
                        AMJEPSU = nCNT1,
                        PMJEPSU = nCNT2,
                        AMJAN = nCNT3,
                        PMJAN = nCNT4,
                        YOIL = strYoil
                    };

                    if (list.IsNullOrEmpty())
                    {
                        result = heaResvLtdService.InsertData(data);
                    }
                    else
                    {
                        data.RID = list.RID.To<string>("");
                        nCNT3 = (int)list.AMINWON - nCNT1;
                        nCNT4 = (int)list.PMINWON - nCNT2;
                        if (nCNT3 < 0) { nCNT3 = 0; }
                        if (nCNT4 < 0) { nCNT4 = 0; }
                        data.AMJAN = nCNT3;
                        data.PMJAN = nCNT4;
                        result = heaResvLtdService.UpDateInwon1(data);
                    }
                }

                //(5) 선택검사 예약인원을 업데이트
                IList<HEA_RESV_EXAM> lists = heaResvLtdService.GetItemsToGroupby(argDate, ArgLtdCode);

                if (lists.Count > 0)
                {
                    for (int i = 0; i < lists.Count; i++)
                    {
                        strExcode = lists[i].GBEXAM.To<string>("");
                        nCNT1 = lists[i].AMCNT.To<int>();
                        nCNT2 = lists[i].PMCNT.To<int>();

                        switch (cF.READ_YOIL(clsDB.DbCon, argDate))
                        {
                            case "월": strYoil = "1"; break;
                            case "화": strYoil = "2"; break;
                            case "수": strYoil = "3"; break;
                            case "목": strYoil = "4"; break;
                            case "금": strYoil = "5"; break;
                            case "토": strYoil = "6"; break;
                            case "일": strYoil = "7"; break;
                            default: break;
                        }

                        HEA_RESV_LTD list2 = heaResvLtdService.GetInwonAmPm(argDate, ArgLtdCode, strExcode);

                        HEA_RESV_LTD data2 = new HEA_RESV_LTD
                        {
                            SDATE = argDate,
                            GUBUN = strExcode,
                            LTDCODE = ArgLtdCode,
                            ENTSABUN = clsType.User.IdNumber.To<long>(),
                            AMINWON = 0,
                            PMINWON = 0,
                            AMJEPSU = nCNT1,
                            PMJEPSU = nCNT2,
                            //AMJAN = (nCNT1 * -1),
                            //PMJAN = (nCNT2 * -1),
                            AMJAN = nCNT1,
                            PMJAN = nCNT2,
                            YOIL = strYoil
                        };

                        if (list2.IsNullOrEmpty())
                        {
                            //data2.AMJAN = (nCNT1 * -1);
                            //data2.PMJAN = (nCNT2 * -1);
                            result = heaResvLtdService.InsertData(data2);
                        }
                        else
                        {
                            data2.RID = list2.RID.To<string>("");

                            nCNT3 = (int)list2.AMINWON - nCNT1;
                            nCNT4 = (int)list2.PMINWON - nCNT2;
                            if (nCNT3 < 0) { nCNT3 = 0; }
                            if (nCNT4 < 0) { nCNT4 = 0; }
                            data2.AMJAN = nCNT3;
                            data2.PMJAN = nCNT4;
                            result = heaResvLtdService.UpDateInwon1(data2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public void MDRD_GFR_Gesan(long nWrtNo, string strSex, long nAge, string strGubun)
        {
            double nA274 = 0;
            double nCFR = 0;
            string strPanjeng = "";
            double nTemp = 0;
            string strExCode = "";

            strExCode = "A274";

            
            if (strGubun == "HIC")
            {
                HIC_RESULT RsltList = hicResultService.Read_Result_YN(nWrtNo, strExCode);
                if (RsltList == null) return;
                nA274 = Convert.ToDouble(RsltList.RESULT);
            }
            else if (strGubun == "HEA")
            {
                HEA_RESULT RsltList = heaResultService.Read_Result_YN(nWrtNo, strExCode);
                if (RsltList == null) return;
                nA274 = Convert.ToDouble(RsltList.RESULT);

            }
            
            if (nA274 == 0) return;

            nTemp = 0;
            nTemp = 186 * Math.Pow(nA274, -1.154) * Math.Pow(nAge, -0.203);
            //여성인경우 0.742를 곱해준다
            if (strSex == "F")
            {
                nTemp = (int)(nTemp * 0.742);
            }
            else
            {
                nTemp = (int)nTemp;
            }

            //GFR계산
            nCFR = Convert.ToDouble(string.Format("{0:####0}", nTemp));

            strExCode = "A116";

            if (strGubun == "HIC")
            {
                int result = hicResultHisService.Result_History_Update2(nCFR, nWrtNo, strExCode);
                if (result < 0)
                {
                    MessageBox.Show("검사결과 History 저장중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //혈청크레아티닌 재계산
                nA274 = nA274 * 100;
                nA274 = Convert.ToDouble(string.Format("{0:#.00}", (nA274 / 10) / 10));

                strExCode = "A274";
                int result2 = hicResultHisService.Result_History_Update2(nA274, nWrtNo, strExCode);

                if (result2 < 0)
                {
                    MessageBox.Show("검사결과 저장중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                return;

            }
            else if (strGubun == "HEA")
            {
                int result = heaResultService.Result_Update2(nCFR, nWrtNo, strExCode);
                if (result < 0)
                {
                    MessageBox.Show("검사결과 History 저장중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //혈청크레아티닌 재계산
                nA274 = nA274 * 100;
                nA274 = Convert.ToDouble(string.Format("{0:#.00}", (nA274 / 10) / 10));

                strExCode = "A274";
                int result2 = heaResultService.Result_Update2(nA274, nWrtNo, strExCode);

                if (result2 < 0)
                {
                    MessageBox.Show("검사결과 저장중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                return;

            } 
        }

        public string READ_HIC_Doctor_Name(string argLicence)
        {
            string rtnVal = "";

            rtnVal = comHpcLibBRepository.Read_Hic_Doctor_Name_byLicence(argLicence);

            return rtnVal;
        }

        public string GET_HIC_JepsuDate(long ArgWRTNO)
        {
            string rtnVal = "";

            //종전 검사일자
            rtnVal = hicJepsuService.GetJepDatebyWrtNo(ArgWRTNO);

            return rtnVal;
        }

        public string ReadAutoPan(string argJepNo)
        {
            int j = 0;
            int kk = 0;
            string rtnVal = "";
            int nREAD = 0;
            string strTemp = "";
            string[] strWRTNOTemp = new string[1];
            string[] strWRTNO = new string[1];
            string[] strCOUNT = new string[1];
            string[] strGRPNO = new string[1];
            string[] strAutoPanWrtno = new string[1];
            string[] strAutoPanGrpno = new string[1];
            string strOK = "";
            string strSex = "";
            string strCompare1 = "";
            string strCompare1Cnt = "";
            string strCompare2 = "";
            string strCompare2Cnt = "";
            string strCompare3 = "";
            string strCompare3Cnt = "";

            //코드 없이 여부 확인
            //1. 혈압약 복용 여부
            //2. 흡연 여부
            //3. 당뇨병 치료 중
            //4. 고혈압 치료중
            //5. 대장 내시경 실시 여부
            //6. 전립선 초음파
            //7. 요침검사
            Array.Resize(ref strWRTNO, 0);
            Array.Resize(ref strAutoPanWrtno, 0);

            strSex = hicJepsuService.GetSexbyWrtNo(argJepNo) == "F" ? "여자" : "남자";

            List<COMHPC> list = comHpcLibBService.GetWrtNOCntGrpNo(argJepNo);

            if (list.Count > 0)
            {
                Array.Resize(ref strWRTNO, list.Count);
                Array.Resize(ref strCOUNT, list.Count);
                Array.Resize(ref strGRPNO, list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    strWRTNO[i] = list[i].WRTNO.ToString();
                    strCOUNT[i] = list[i].CNT.ToString();
                    strGRPNO[i] = list[i].GRPNO;
                }
            }

            if (list.Count < 1)
            {
                rtnVal = "";
                return rtnVal;
            }

            Array.Resize(ref strAutoPanGrpno, 0);
            for (int i = 0; i < VB.UBound(strWRTNO); i++)
            {
                List<COMHPC> list2 = comHpcLibBService.GetWrtNoSeqNobyWrtNo(argJepNo);

                nREAD = list2.Count;
                if (nREAD > 0)
                {
                    for (int k = 0; k < nREAD; k++)
                    {
                        strOK = "OK";
                        if (VB.UBound(strAutoPanGrpno) > 0)
                        {
                            for (int l = 0; l < VB.UBound(strAutoPanGrpno); l++)
                            {
                                if (strGRPNO[i] == strAutoPanGrpno[l])
                                {
                                    strOK = "NO";
                                }
                            }
                        }

                        if (ReadAutoPanEXCODE(argJepNo, strWRTNO[i], strSex, list2[k].SEQNO.ToString()) && strOK == "OK")
                        {
                            j += 1;
                            Array.Resize(ref strAutoPanWrtno, j);
                            Array.Resize(ref strAutoPanGrpno, j);
                            strAutoPanWrtno[j - 1] = strWRTNO[i];
                            strAutoPanGrpno[j - 1] = strGRPNO[i];
                        }
                    }
                }
            }

            Array.Resize(ref strWRTNOTemp, VB.UBound(strAutoPanWrtno));

            List<HEA_AUTOPAN> listAutoPan = heaAutopanService.GetWrtNo();

            if (listAutoPan.Count > 0)
            {
                for (int i = 0; i < listAutoPan.Count; i++)
                {
                    for (int jj = 0; jj < VB.UBound(strAutoPanWrtno); jj++)
                    {
                        if (listAutoPan[i].WRTNO.ToString() == strAutoPanWrtno[jj])
                        {
                            strWRTNOTemp[kk] = strAutoPanWrtno[jj];
                            kk += 1;
                        }
                    }
                }
            }

            strTemp = "";
            if (strWRTNOTemp[0] != "")
            {
                for (int i = 0; i < VB.UBound(strAutoPanWrtno); i++)
                {
                    strTemp += ViewResultSyntex(strWRTNOTemp[i], argJepNo) + "\r\n" + "\r\n";
                }
            }

            rtnVal = strTemp;

            return rtnVal;
        }

        public bool ReadAutoPanEXCODE(string argJepNo, string ArgWrtNo, string ArgSex, string ArgSeqno)
        {
            bool rtnVal = false;
            int nREAD = 0;
            string strOldEXCODE = "";
            string strSex = "";
            string strValue1 = "";
            string strValue2 = "";
            string strLogic = "";
            string nCntR1 = "";
            string nCntR2 = "";
            string strCalc = "";
            string strRetVal = "";
            string strLeftValue = "";
            string strLeftLogic = "";
            string strMidValue = "";
            string strRightValue = "";
            string strRightLogic = "";
            bool bLogic1 = false;
            bool bLogic2 = false;
            bool bLogic3 = false;
            bool bLogic4 = false;
            bool bLogic5 = false;

            nCntR1 = heaAutopanLogicService.GetExCodebyWrtNo(ArgWrtNo);

            nCntR2 = hicResultService.GetAllByWrtNo(ArgWrtNo, argJepNo);

            if (nCntR1 != nCntR2)
            {
                rtnVal = false;
                return rtnVal;
            }

            bLogic1 = false;
            List<HEA_AUTOPAN_LOGIC_RESULT> list = heaAutopanLogicResultService.GetItembyWrtNoSeqNo(ArgWrtNo, ArgSeqno, argJepNo);

            nREAD = list.Count;
            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {
                    strSex = list[i].SEX;
                    strValue1 = list[i].RESULT.Trim();
                    strValue2 = list[i].RESULTVALUE.Trim();
                    strLogic = list[i].LOGIC.Trim();
                    if (strSex == "")
                    {
                        bLogic1 = CompareLogic(strValue1, strValue2, strLogic);
                    }
                    else if (strSex == ArgSex)
                    {
                        bLogic1 = CompareLogic(strValue1, strValue2, strLogic);
                    }
                    else
                    {
                        bLogic1 = false;
                    }

                    if (bLogic1 == false)
                    {
                        rtnVal = false;
                        return rtnVal;
                    }
                }
            }

            if (bLogic1 == false)
            {
                rtnVal = false;
                return rtnVal;
            }

            List<HEA_AUTOPAN_LOGIC_RESULT> list2 = heaAutopanLogicResultService.GetItembyWrtNoSeqNo_Second(ArgWrtNo, ArgSeqno, argJepNo);

            nREAD = list2.Count;
            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {
                    strSex = list2[i].SEX;
                    strValue1 = list2[i].RESULT.Trim();
                    strValue2 = list2[i].RESULTVALUE.Trim();
                    strLogic = list2[i].LOGIC.Trim();

                    if (strOldEXCODE != list2[i].EXCODE.Trim())
                    {
                        strRightValue = list2[i].RESULTVALUE.Trim();
                        strRightLogic = list2[i].LOGIC.Trim();
                        strMidValue = list2[i].RESULT.Trim();
                    }
                    else
                    {
                        strLeftValue = list2[i].RESULTVALUE.Trim();
                        strLeftLogic = list2[i].LOGIC.Trim();

                        if (strSex == "")
                        {
                            bLogic1 = CompareLogic(strValue1, strValue2, strLogic);
                        }
                        else if (strSex == ArgSex)
                        {
                            bLogic1 = CompareLogic(strValue1, strValue2, strLogic);
                        }
                        else
                        {
                            bLogic1 = false;
                        }
                    }
                    strOldEXCODE = list2[i].EXCODE.Trim();

                    if (bLogic1 == false)
                    {
                        rtnVal = false;
                        return rtnVal;
                    }
                }
            }

            bLogic2 = false;
            List<HEA_AUTOPAN_LOGIC_RESULT> list3 = heaAutopanLogicResultService.GetItembyWrtNoSeqNo_Third(ArgWrtNo, ArgSeqno, argJepNo);

            nREAD = list3.Count;
            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {
                    strSex = list3[i].SEX;
                    strValue1 = list3[i].RESULT.Trim();
                    strValue2 = list3[i].RESULTVALUE.Trim();
                    strLogic = list3[i].LOGIC.Trim();

                    if (strSex == "")
                    {
                        if (CompareLogic(strValue1, strValue2, strLogic) == true)
                        {
                            bLogic2 = true;
                            break;
                        }
                    }
                    else
                    {
                        if (strSex == ArgSex)
                        {
                            if (CompareLogic(strValue1, strValue2, strLogic) == true)
                            {
                                bLogic2 = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (nREAD < 1)
            {
                bLogic2 = true;
            }

            bLogic3 = false;
            List<HEA_AUTOPAN_LOGIC_RESULT> list4 = heaAutopanLogicResultService.GetItembyWrtNoSeqNo_Forth(ArgWrtNo, ArgSeqno, argJepNo);

            nREAD = list4.Count;
            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {
                    strSex = list4[i].SEX;
                    strValue1 = list4[i].RETVAL1.Trim();
                    strValue2 = list4[i].RETVAL2.Trim();
                    strCalc = list4[i].CALC.Trim();
                    strLogic = list4[i].LOGIC.Trim();
                    strRetVal = list4[i].RESULTVALUE.Trim();

                    if (strSex == "")
                    {
                        if (CompareCalc(strValue1, strValue2, strCalc, strRetVal, strLogic) == true)
                        {
                            bLogic3 = true;
                            break;
                        }
                    }
                    else
                    {
                        if (strSex == ArgSex)
                        {
                            if (CompareCalc(strValue1, strValue2, strCalc, strRetVal, strLogic) == true)
                            {
                                bLogic3 = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (nREAD < 1)
            {
                bLogic3 = true;
            }

            bLogic4 = true;
            //기타검사 조건이 있으면 무조건 FALSE
            //문진표 전산화 이후 적용 예정

            if (heaAutopanLogicService.GetCountbyWrtNoSeqNo(ArgWrtNo, ArgSeqno) > 0)
            {
                bLogic4 = false;
            }

            if (bLogic4 == true)
            {
                if (heaAutopanLogicService.GetCountbyWrtNoSeqNoJepNo(ArgWrtNo, ArgSeqno, argJepNo) < 1)
                {
                    bLogic4 = true;
                }
                else
                {
                    if (heaAutopanLogicService.GetCountbyWrtNoSeqNoJepNo_Implement(ArgWrtNo, ArgSeqno, argJepNo, "실시(포함)") < 1)
                    {
                        bLogic4 = false;
                    }

                    if (heaAutopanLogicService.GetCountbyWrtNoSeqNoJepNo_Implement(ArgWrtNo, ArgSeqno, argJepNo, "미실시(미포함)") > 0)
                    {
                        bLogic4 = false;
                    }
                }
            }

            if (bLogic1 == true && bLogic2 == true && bLogic3 == true && bLogic4 == true)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        public bool CompareLogic(string Arg1, string Arg2, string argLogic)
        {
            bool rtnVal = false;

            switch (argLogic)
            {
                case "=":
                    if (VB.IsNumeric(Arg1) && VB.IsNumeric(Arg2))
                    {
                        rtnVal = long.Parse(Arg1) == long.Parse(Arg2);
                    }
                    else
                    {
                        rtnVal = (Arg1 == Arg2);
                    }
                    break;
                case "<>":
                    if (VB.IsNumeric(Arg1) && VB.IsNumeric(Arg2))
                    {
                        rtnVal = long.Parse(Arg1) == long.Parse(Arg2);
                    }
                    else
                    {
                        rtnVal = Arg1 == Arg2;
                    }
                    break;
                case ">":
                    rtnVal = long.Parse(Arg1) > long.Parse(Arg2);
                    break;
                case ">=":
                    rtnVal = long.Parse(Arg1) >= long.Parse(Arg2);
                    break;
                case "<":
                    rtnVal = long.Parse(Arg1) < long.Parse(Arg2);
                    break;
                case "<=":
                    rtnVal = long.Parse(Arg1) <= long.Parse(Arg2);
                    break;
                default:
                    rtnVal = false;
                    break;
            }

            return rtnVal;
        }

        public bool CompareLogic2(string Arg1, string arg1Logic, string Arg2, string arg2Logic, string argRet)
        {
            bool rtnVal = false;
            bool bLeft = false;
            bool bRight = false;

            switch (arg1Logic)
            {
                case ">":
                    bLeft = long.Parse(Arg1) > long.Parse(argRet);
                    break;
                case ">=":
                    bLeft = long.Parse(Arg1) >= long.Parse(argRet);
                    break;
                case "<":
                    bLeft = long.Parse(Arg1) < long.Parse(argRet);
                    break;
                case "<=":
                    bLeft = long.Parse(Arg1) <= long.Parse(argRet);
                    break;
                default:
                    break;
            }

            switch (arg2Logic)
            {
                case ">":
                    bRight = long.Parse(Arg2) > long.Parse(argRet);
                    break;
                case ">=":
                    bRight = long.Parse(Arg2) >= long.Parse(argRet);
                    break;
                case "<":
                    bRight = long.Parse(Arg2) < long.Parse(argRet);
                    break;
                case "<=":
                    bRight = long.Parse(Arg2) <= long.Parse(argRet);
                    break;
                default:
                    break;
            }

            if (bLeft == true && bRight == true)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        public bool CompareCalc(string Arg1, string Arg2, string argCalc, string argRetVal, string argLogic)
        {
            bool rtnVal = false;
            bool bCompare = false;
            long nCalcRet = 0;

            switch (argCalc)
            {
                case "=":
                    if (VB.IsNumeric(Arg1) && VB.IsNumeric(Arg2))
                    {
                        rtnVal = long.Parse(Arg1) > long.Parse(Arg2);
                    }
                    else
                    {
                        rtnVal = Arg1 == Arg2;
                    }
                    break;
                case "<>":
                    if (VB.IsNumeric(Arg1) && VB.IsNumeric(Arg2))
                    {
                        rtnVal = long.Parse(Arg1) != long.Parse(Arg2);
                    }
                    else
                    {
                        rtnVal = Arg1 != Arg2;
                    }
                    break;
                case ">":
                    rtnVal = long.Parse(Arg1) > long.Parse(Arg2);
                    break;
                case ">=":
                    rtnVal = long.Parse(Arg1) >= long.Parse(Arg2);
                    break;
                case "<":
                    rtnVal = long.Parse(Arg1) < long.Parse(Arg2);
                    break;
                case "<=":
                    rtnVal = long.Parse(Arg1) <= long.Parse(Arg2);
                    break;
                default:
                    break;
            }

            if (rtnVal == true)
            {
                return rtnVal;
            }

            switch (argCalc)
            {
                case "-":
                    nCalcRet = long.Parse(Arg1) - long.Parse(Arg2);
                    break;
                case "+":
                    nCalcRet = long.Parse(Arg1) + long.Parse(Arg2);
                    break;
                case "/":
                    nCalcRet = long.Parse(Arg1) / long.Parse(Arg2);
                    break;
                case "*":
                    nCalcRet = long.Parse(Arg1) * long.Parse(Arg2);
                    break;
                default:

                    break;
            }

            if (nCalcRet.IsNullOrEmpty())
            {
                return rtnVal;
            }

            switch (argLogic)
            {
                case "=":
                    rtnVal = nCalcRet == long.Parse(argRetVal);
                    break;
                case "<>":
                    rtnVal = nCalcRet != long.Parse(argRetVal);
                    break;
                case ">":
                    rtnVal = nCalcRet > long.Parse(argRetVal);
                    break;
                case ">=":
                    rtnVal = nCalcRet >= long.Parse(argRetVal);
                    break;
                case "<":
                    rtnVal = nCalcRet < long.Parse(argRetVal);
                    break;
                case "<=":
                    rtnVal = nCalcRet <= long.Parse(argRetVal);
                    break;
                default:
                    rtnVal = false;
                    break;
            }

            return rtnVal;
        }

        public string ViewResultSyntex(string ArgWRTNO, string argJepNo)
        {
            string rtnVal = "";
            string strSyntex = "";

            string[] strMCode = new string[1];
            string[] strExCode = new string[1];
            string[] strResult = new string[1];

            if (ArgWRTNO == "")
            {
                return rtnVal;
            }

            strSyntex = heaAutopanService.GetTextbyWrtNo(long.Parse(ArgWRTNO));

            if (strSyntex == "") return rtnVal;

            List<HEA_AUTOPAN_MATCH_RESULT> list = heaAutoPanMatchResultService.GetItembyWrtno(ArgWRTNO, argJepNo);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strSyntex = strSyntex.Replace(list[i].MCODE.Trim(), list[i].RESULT.Trim());
                }
            }

            rtnVal = strSyntex;

            return rtnVal;
        }

        /// <summary>
        /// 특수 소견내역,조치내역,업무적합성,사후관리,추가검사 자동설정
        /// 결과값:(1)소견내역 (2)조치내역 (3)업무적합성 (4)사후관리 (5)추가검사
        /// </summary>
        /// <param name="ArgPanjeng"></param>
        /// <param name="ArgSogen"></param>
        /// <param name="ArgRemark"></param>
        /// <returns></returns>
        public string Pajeng_Auto_JochiRemark(string ArgPanjeng, string ArgSogen, string ArgRemark)
        {
            string rtnVal = "";
            string strResult = "";

            if (string.Compare(ArgSogen, "J007") >= 0 && string.Compare(ArgSogen, "J018") <= 0)
            {
                strResult += ArgRemark + "{$}";
                if (string.Compare(ArgSogen, "J007") >= 0 && string.Compare(ArgSogen, "J009") <= 0)
                {
                    strResult += "보호구 착용 철저 및 추적검사(증상시는 진료){$}";
                }
                else
                {
                    strResult += "보호구 착용 철저 및 호흡기내과 진료{$}";
                }
                strResult += "002{$}2,3{$}{$}";
                rtnVal = strResult;
                return rtnVal;
            }

            //코드에 설정된 자동판정값을 Return
            HIC_SPC_SCODE list2 = hicSpcScodeService.GetItembyCode(ArgSogen);

            if (!list2.IsNullOrEmpty())
            {
                if (list2.GBAUTOPAN == "Y")
                {
                    strResult = list2.SOGENREMARK.To<string>("").Trim() + "{$}";   //(1)
                    strResult += list2.JOCHIREMARK.To<string>("").Trim() + "{$}";  //(2)
                    strResult += list2.WORKYN.To<string>("").Trim() + "{$}";       //(3)
                    strResult += list2.SAHUCODE.To<string>("").Trim() + "{$}";     //(4)
                    strResult += list2.REEXAM.To<string>("").Trim() + "{$}";       //(5)
                }
                else
                {
                    strResult = "";
                }
            }

            rtnVal = strResult;

            return rtnVal;
        }

        public string READ_JepsuSTS_Name(string argCode)
        {
            string rtnVal = "";

            switch (argCode)
            {
                case "":
                case "1":
                    rtnVal = "X";
                    break;
                case "2":
                    rtnVal = "◎";
                    break;
                case "3":
                    rtnVal = "판정";
                    break;
                case "D":
                    rtnVal = "삭제";
                    break;
                default:
                    rtnVal = "";
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// HIC_JEPSU , 가접수등 사용 (HIC_회사코드설정(ArgGJJONG As String, ArgLtdCode As String))
        /// </summary>
        /// <param name="argGjJong"></param>
        /// <param name="argLtdCode"></param>
        /// <returns></returns>
        public string Hic_LtdCode_Set(string argGjJong, string argLtdCode)
        {
            string rtnVal = "";

            switch (argGjJong)
            {
                case "13":
                case "18":
                case "43":
                case "46":
                    rtnVal = "";
                    break;
                default:
                    rtnVal = argLtdCode;

                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// HIC_PATIENT (HIC_회사코드설정2(ArgPano As Long, ArgGJJONG As String, ArgLtdCode As String))
        /// </summary>
        /// <param name="argGjJong"></param>
        /// <param name="argLtdCode"></param>
        /// <returns></returns>
        public string Hic_LtdCode_Set2(string argPaNo, string argGjJong, string argLtdCode)
        {
            string rtnVal = "";
            string strLtdCode2 = "";

            //if (argLtdCode.IsNullOrEmpty())
            //{
            //    HIC_PATIENT list = hicPatientService.GetLtdCodebyPaNo(argPaNo);

            //    if (!list.IsNullOrEmpty())
            //    {
            //        strLtdCode2 = string.Format("{0:#}", list.LTDCODE);
            //    }
            //    else
            //    {
            //        strLtdCode2 = "";
            //    }
            //}
            //else
            //{
            //    strLtdCode2 = argLtdCode;
            //}

            rtnVal = strLtdCode2;

            return rtnVal;
        }

        /// <summary>
        /// HIC_JEPSU , 가접수등 사용 (HIC_회사코드설정3(ArgGJJONG As String, ArgLtdCode As String, ArgKiho As String) As String)
        /// </summary>
        /// <param name="argGjJong"></param>
        /// <param name="argLtdCode"></param>
        /// <returns></returns>
        public string Hic_LtdCode_Set3(string argGjJong, string argLtdCode, string argKiho)
        {
            string rtnVal = "";

            switch (argGjJong)
            {
                case "31":
                case "35":
                    rtnVal = argLtdCode;
                    break;
                default:
                    rtnVal = argKiho;
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 증번호로 자격구분 체크 HIC_JEPSU GbChk1 
        /// 자격정보1 - (01:사업장 02:공무원 03:성인병, 04:의료급여)
        /// </summary>
        /// <param name="ArgGkiho"></param>
        /// <param name="ArgWRTNO"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain_Exam : Read_Gkiho2_Gbn"/>
        public string Read_Gkiho2_Gbn(string ArgGkiho, long ArgWRTNO = 0)
        {
            string rtnVal = string.Empty;

            if (ArgGkiho != "")
            {
                switch (VB.Left(ArgGkiho, 1))
                {
                    case "1":
                    case "2":
                    case "3": rtnVal = "01"; break;
                    case "5":
                    case "6": rtnVal = "02"; break;
                    case "7":
                    case "8": rtnVal = "01"; break;
                    case "9": rtnVal = "04"; break;
                    default: MessageBox.Show("증번호 오류입니다.", "오류"); break;
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 선택검사+자격으로 의료급여생애 체크 HIC_JEPSU GbChk2
        /// 자격정보2 - (01:생애의료급여)
        /// </summary>
        /// <param name="argSEaxmCodes"></param>
        /// <param name="ArgGkiho"></param>
        /// <param name="argNHic"></param>
        /// <param name="argAge"></param>
        /// <param name="ArgWRTNO"></param>
        /// <returns></returns>
        /// <seealso cref="HcMain_Exam : Read_66Gub2_Gbn"/>
        public string Read_66Gub2_Gbn(string argSEaxmCodes, string ArgGkiho, string argNHic, int argAge, long ArgWRTNO = 0)
        {
            string rtnVal = string.Empty;
            string strCode = string.Empty;

            if (ArgWRTNO > 0) { return rtnVal; }

            if (argSEaxmCodes != "")
            {
                for (int i = 1; i <= VB.L(argSEaxmCodes, ","); i++)
                {
                    switch (VB.Pstr(argSEaxmCodes, ",", i))
                    {
                        case "1162":
                        case "1163":
                        case "1164":
                        case "1165":
                        case "1166":
                        case "1167": strCode = "OK"; break;
                        default:
                            break;
                    }

                    if (strCode == "OK") { break; }
                }
            }

            if (argNHic == "Y")
            {
                if (VB.Left(ArgGkiho, 1) == "9" && strCode == "OK")
                {
                    rtnVal = "01";
                }
            }
            else
            {
                if (argAge == 66 && strCode == "OK")
                {
                    rtnVal = "01";
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 특정검사코드 참고치 변경으로 공용모듈 만듬
        /// </summary>
        public string EXAM_NomalValue_SET(string argExCode, string argJepDate, string argSex, string argMinM, string argMaxM, string argMinF, string argMaxF)
        {
            string rtnVal = "";
            string strNomal = "";

            if (argSex == "M")
            {
                strNomal = argMinM + "~" + argMaxM;
            }
            else
            {
                strNomal = argMinF + "~" + argMaxF;
            }

            if (strNomal == "~")
            {
                strNomal = "";
            }

            if (!strNomal.IsNullOrEmpty())
            {
                if (VB.Right(strNomal, 1) == "~")
                {
                    strNomal = VB.Left(strNomal, strNomal.Length - 1);
                }
            }

            rtnVal = strNomal;

            return rtnVal;
        }

        /// <summary>
        /// 공단 자격조회 내역 Display 및 변수 저장
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="argGbn"></param>
        /// <param name="argSName"></param>
        /// <param name="argJuminNo"></param>
        /// <param name="argPtno"></param>
        /// <param name="argYear"></param>
        /// <returns></returns>
        public bool Display_Nhic_Info(WORK_NHIC item)
        {
            bool rtnVal = false;

            clsHcType.TEC_CLEAR("ALL");
            clsHcType.THNV_CLEAR();

            if (!item.IsNullOrEmpty())
            {
                clsHcType.TEC.NHICOK = "Y";

                //접수시 참조하는 구조체변수에 저장
                clsHcType.TEC.REL = item.REL;
                clsHcType.TEC.GKIHO = item.GKIHO;
                clsHcType.TEC.EXAMA = "1.N"; if (!item.EXAMA.IsNullOrEmpty() && item.EXAMA.Equals("대상")) { clsHcType.TEC.EXAMA = "1.Y"; }
                clsHcType.TEC.EXAMB = "2.N"; if (!item.LIVER.IsNullOrEmpty() && item.LIVER.Equals("대상")) { clsHcType.TEC.EXAMB = "2.Y"; }
                clsHcType.TEC.EXAMC = "3.N"; if (!item.LIVERC.IsNullOrEmpty() && item.LIVERC.Equals("대상")) { clsHcType.TEC.EXAMC = "3.Y"; }
                clsHcType.TEC.EXAMD = "4.N"; if (!item.EXAMD.IsNullOrEmpty() && item.EXAMD.Equals("대상")) { clsHcType.TEC.EXAMD = "4.Y"; }
                clsHcType.TEC.EXAME = "5.N"; if (!item.EXAME.IsNullOrEmpty() && item.EXAME.Equals("대상")) { clsHcType.TEC.EXAME = "5.Y"; }
                clsHcType.TEC.EXAMF = "6.N"; if (!item.EXAMF.IsNullOrEmpty() && item.EXAMF.Equals("대상")) { clsHcType.TEC.EXAMF = "6.Y"; }
                clsHcType.TEC.EXAMG = "7.N"; if (!item.EXAMG.IsNullOrEmpty() && item.EXAMG.Equals("대상")) { clsHcType.TEC.EXAMG = "7.Y"; }
                clsHcType.TEC.EXAMH = "8.N"; if (!item.EXAMH.IsNullOrEmpty() && item.EXAMH.Equals("대상")) { clsHcType.TEC.EXAMH = "8.Y"; }
                clsHcType.TEC.EXAMI = "9.N"; if (!item.EXAMI.IsNullOrEmpty() && item.EXAMI.Equals("대상")) { clsHcType.TEC.EXAMI = "9.Y"; }

                //자격조회 정보 저장
                clsHcType.THNV.hSName = item.SNAME;
                clsHcType.THNV.hJumin = clsAES.DeAES(item.JUMIN2);    //주민번호
                clsHcType.THNV.hJaGubun = item.REL;                     //사업구분
                clsHcType.THNV.Year = item.YEAR;                    //사업년도
                clsHcType.THNV.hJaSTS = item.TRANS;                   //자격상태
                clsHcType.THNV.hGKiho = item.GKIHO;                   //증기호
                clsHcType.THNV.hJisa = item.JISA;                    //소속지사
                clsHcType.THNV.hGetDate = item.BDATE;                   //취득일자
                clsHcType.THNV.hBogen = item.CANCER53;                //관할보건소
                clsHcType.THNV.hKiho = item.KIHO;                    //회사기호
                clsHcType.THNV.h1Cha = item.FIRST;                   //1차검진
                clsHcType.THNV.hDental = item.DENTAL;                  //구강검진
                clsHcType.THNV.hLiver = item.LIVER;                   //B형간염
                clsHcType.THNV.hLiverC = item.LIVERC;                  //C형간염
                clsHcType.THNV.h2Cha = item.SECOND;                  //2차검진
                clsHcType.THNV.hCan1 = item.CANCER11;                //암-위
                clsHcType.THNV.hCan12 = item.CANCER12;                //암-위 치료비지원
                clsHcType.THNV.hCan2 = item.CANCER21;                //암-유방
                clsHcType.THNV.hCan22 = item.CANCER22;                //암-유방 치료비지원
                clsHcType.THNV.hCan3 = item.CANCER31;                //암-대장
                clsHcType.THNV.hCan32 = item.CANCER32;                //암-대장 치료비지원
                if (string.Compare(VB.Mid(DateTime.Now.ToShortDateString(), 6, 5), "06-30") <= 0)
                {
                    clsHcType.THNV.hCan4 = item.CANCER41;               //간암(전반기)
                    clsHcType.THNV.hCan42 = item.CANCER42;              //간암(전반기)
                }
                else
                {
                    clsHcType.THNV.hCan6 = item.CANCER61;               //간암(후반기)
                    clsHcType.THNV.hCan62 = item.CANCER62;              //간암(후반기)
                }

                clsHcType.THNV.hCan5 = item.CANCER51;                //암-자궁
                clsHcType.THNV.hCan52 = item.CANCER52;                  //암-자궁
                clsHcType.THNV.hCan7 = item.CANCER71;                //암-폐
                clsHcType.THNV.hCan72 = item.CANCER72;                  //암-폐
                //수검정보
                clsHcType.THNV.h1ChaDate = item.GBCHK01;               //1차검진 일자
                clsHcType.THNV.h1ChaHName = item.GBCHK01_NAME;          //1차검진 병원명
                clsHcType.THNV.h2ChaDate = item.GBCHK02;               //2차검진 일자
                clsHcType.THNV.h2ChaHName = item.GBCHK02_NAME;          //2차검진 병원명
                clsHcType.THNV.hDentDate = item.GBCHK03;               //구강검진 일자
                clsHcType.THNV.hDentHName = item.GBCHK03_NAME;          //구강검진 병원명
                clsHcType.THNV.h위Date = item.GBCHK04;               //위암검진 일자
                clsHcType.THNV.h위HName = item.GBCHK04_NAME;          //위암검진 병원명
                clsHcType.THNV.h대장Date = item.GBCHK05;               //대장암검진 일자
                clsHcType.THNV.h대장HName = item.GBCHK05_NAME;          //대장암검진 병원명
                clsHcType.THNV.h유방Date = item.GBCHK06;               //유방암검진 일자
                clsHcType.THNV.h유방HName = item.GBCHK06_NAME;          //유방암검진 병원명
                clsHcType.THNV.h자궁Date = item.GBCHK07;               //자궁경부암검진 일자
                clsHcType.THNV.h자궁HName = item.GBCHK07_NAME;          //자궁경부암검진 병원명
                clsHcType.THNV.h간Date = item.GBCHK09;               //간암검진 일자
                clsHcType.THNV.h간HName = item.GBCHK09_NAME;          //간암검진 병원명
                clsHcType.THNV.h폐Date = item.GBCHK10;               //폐암검진 일자
                clsHcType.THNV.h폐HName = item.GBCHK10_NAME;          //폐암검진 병원명

                if (item.GBCHK04.IsNullOrEmpty())
                {
                    clsHcType.THNV.h위Date = item.GBCHK15;
                    clsHcType.THNV.h위HName = item.GBCHK15_NAME;
                }

                if (item.GBCHK05.IsNullOrEmpty())
                {
                    clsHcType.THNV.h대장Date = item.GBCHK16;
                    clsHcType.THNV.h대장HName = item.GBCHK16_NAME;
                }

                if (item.GBCHK05.IsNullOrEmpty() && item.GBCHK16.IsNullOrEmpty())
                {
                    clsHcType.THNV.h대장Date = item.GBCHK17;
                    clsHcType.THNV.h대장HName = item.GBCHK17_NAME;
                }

                rtnVal = true;
            }

            return rtnVal;
        }

        /// <summary>
        /// 유해물질_30년보관대상
        /// </summary>
        /// <param name="">결과지 유해물질 30년보관대상 조회</param>
        /// <returns>TEST OK</returns>
        /// <seealso cref="HcMain.bas> 유해물질_30년보관대상"/>
        public string READ_UCODES_30Y(string argUcodes)
        {
            string UcodseOK = "";

            for (int i = 1; i <= VB.L(argUcodes, ",") - 1; i++)
            {
                switch (VB.Pstr(argUcodes, ",", i))
                {
                    case "F08":
                    case "F38": 
                    case "F80": 
                    case "G38": 
                    case "G39": 
                    case "F47": 
                    case "E80":
                    case "E55": 
                    case "F69": 
                    case "E45": 
                    case "E59": 
                    case "E90": 
                    case "F35": 
                    case "G33":
                    case "G01": 
                    case "G41": 
                    case "G42": 
                    case "G43": 
                    case "G44": 
                    case "G06": 
                    case "G24":
                    case "G34": 
                    case "F77": 
                    case "F78": 
                    case "F06": 
                    case "H02": 
                    case "H03":
                    case "F04":
                    case "F17": 
                    case "F25": 
                    case "F02": 
                    case "G05": 
                    case "F64": 
                    case "Q01": 
                    case "Q02":
                    case "F22": 
                    case "F09": 
                    case "H04":

                    UcodseOK = "OK";
                    break;
                    default: break;
                }
            }
            return UcodseOK;

        }
    }
}
