using ComBase;
using ComBase.Controls;
using ComLibB.Dto;
using ComLibB.Service;
using System;

namespace ComLibB
{
    class clsComHpc
    {
        ComHpcService comHpcService = null;

        public clsComHpc()
        {

            comHpcService = new ComHpcService();
        }

        /// <summary>
        /// 회사명칭을 READ
        /// READ_Ltd_Name() => READ_Ltd_One_Name()
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_Ltd_Name(string strCode)
        {
            string rtnVal = "";

            if (VB.Val(strCode) == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            COMHPC list = comHpcService.FindOne(strCode.Trim());

            if (list != null)
            {
                rtnVal = list.NAME;
                clsComVariable.GstrLtdJuso = list.JUSO + " " + list.JUSODETAIL;
                clsComVariable.GstrLtdJuso1 = list.SANGHO + " ";
                clsComVariable.GstrLtdJuso2 = list.SANGHO + " " + list.NAME;
                clsComVariable.GstrLtdMailcode = VB.Mid(list.MAILCODE, 1, 1) + " " + VB.Mid(list.MAILCODE, 2, 1) + " " +
                                                VB.Mid(list.MAILCODE, 3, 1) + "-" + VB.Mid(list.MAILCODE, 4, 1) + " " +
                                                VB.Mid(list.MAILCODE, 5, 1) + "-" + VB.Mid(list.MAILCODE, 6, 1) + " ";
                clsComVariable.GstrKiho = list.KIHO;
                clsComVariable.GnInwon = Convert.ToInt32(list.INWON);
                clsComVariable.GstrTel = list.TEL;
            }
            else
            {
                rtnVal = "";
                clsComVariable.GstrLtdJuso = "";
                clsComVariable.GstrLtdJuso1 = "";
                clsComVariable.GstrLtdJuso2 = "";
                clsComVariable.GstrLtdMailcode = "";
                clsComVariable.GstrKiho = "";
                clsComVariable.GnInwon = 0;
                clsComVariable.GstrTel = "";
            }
            return rtnVal;
        }


        /// <summary>
        /// 종검종류명을 READ
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_GjJong_HeaName(string strCode)
        {
            string rtnVal = "";

            if (strCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = comHpcService.Read_Hea_ExJong_Name(strCode.Trim());

            return rtnVal;
        }

        /// <summary>
        /// 종합검진 검사결과 판정(L=Low,H=High,"":Nomal)
        /// </summary>
        /// <param name="strExCode"></param>
        /// <param name="strResult"></param>
        /// <param name="strNormal"></param>
        /// <returns></returns>
        public string Result_Panjeng(string strExCode, string strResult, string strNormal)
        {
            string rtnVal = "";
            double nMinValue = 0;
            double nMaxValue = 0;
            double nResult = 0;
            double nLowRes = 0;
            double nHighRes = 0;

            if (strResult.IsNullOrEmpty() || strNormal.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            if (VB.L(strNormal, "~") < 2)
            {
                rtnVal = "";
                return rtnVal;
            }

            nMinValue = string.Format(VB.Pstr(strNormal, "~", 1)).To<double>();
            nMaxValue = string.Format(VB.Pstr(strNormal, "~", 2)).To<double>();

            if (nMinValue == 0 && nMaxValue == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            nResult = VB.Val(string.Format(strResult));
            nResult = VB.Val(VB.Replace(VB.Replace(strResult, ">", ""), "<", ""));



            switch (strExCode.Trim())
            {
                case "A271":
                case "A272":
                    nLowRes = string.Format(VB.Pstr(strResult, "-", 1)).To<double>();
                    nHighRes = string.Format(VB.Pstr(strResult, "-", 2)).To<double>();

                    if (nMinValue > nLowRes)
                    {
                        rtnVal = "L";
                        return rtnVal;
                    }
                    else if (nMaxValue < nHighRes)
                    {
                        rtnVal = "H";
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                case "A241":
                    if (nResult > nMaxValue)
                    {
                        rtnVal = "H";
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                case "TU42":
                    if (nMinValue > nResult)
                    {
                        rtnVal = "L";
                        return rtnVal;
                    }
                    else if (nResult > nMaxValue)
                    {
                        rtnVal = "H";
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                default:
                    rtnVal = "";
                    break;
            }

            //소변 및 대변 검사
            switch (strResult)
            {
                case "음성":
                case "-":
                    rtnVal = "";
                    return rtnVal;
                case "양성":
                    rtnVal = "L";
                    return rtnVal;
                case "+-":
                    if (strExCode.Trim() == "LU46" && strExCode.Trim() == "A259")
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = "L";
                        return rtnVal;
                    }
                default:
                    rtnVal = "";
                    break;
            }

            rtnVal = "";
            if (nResult < nMinValue)
            {
                rtnVal = "L";
            }
            else if (nResult > nMaxValue)
            {
                rtnVal = "H";
            }
            else
            {
                rtnVal = "";    //Nomal 또는 점검불능
            }

            return rtnVal;
        }


        /// <summary>
        /// 결과코드명 READ  READ_HeaResultName() Merge
        /// </summary>
        /// <param name="strGBN"></param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_ResultName(string strGBN, string strCode)
        {
            string rtnVal = "";

            if (strCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = comHpcService.Read_Hic_ResCodeName(strGBN, strCode.Trim());

            return rtnVal;
        }

        public bool Check_ReferValue_ChangeCode(string argCode)
        {
            bool rtnVal = false;

            if (VB.InStr(clsComVariable.REFER_CHANGE_CODELIST, argCode) > 0)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        public string GET_Refer_Value(string argCode, string argSex, string argDate, string argGbUnit)
        {
            string rtnVal = "";
            int nCnt = 0;
            string strRec = "";
            string strRefer = "";
            string strCode = "";
            string strFromDate = "";
            string strToDate = "";

            //종전의 참고값을 찾음
            for (int i = 0; i < clsComVariable.REFER_OLD_CNT; i++)
            {
                if (argSex == "M")
                {
                    strRec = VB.Pstr(clsComVariable.REFER_OLD_남자_VALUE, "{@}", i);
                }
                else
                {
                    strRec = VB.Pstr(clsComVariable.REFER_OLD_여자_VALUE, "{@}", i);
                }
                strCode = VB.Pstr(strRec, "{}", 1);

                if (strCode == argCode)
                {
                    strFromDate = VB.Pstr(strRec, "{}", 2);
                    strToDate = VB.Pstr(strRec, "{}", 3);
                    if (string.Compare(argDate, strFromDate) >= 0 && string.Compare(argDate, strToDate) <= 0)
                    {
                        strRefer = VB.STRCUT(strRec, "{}" + strToDate + "{}", "");
                    }
                }
            }

            //종전의 참고치 대상이 아니면 HIC_EXCODE의 참고치를 가져옴
            if (strRefer == "")
            {
                if (argSex == "M")
                {
                    strRefer = VB.STRCUT(clsComVariable.REFER_OLD_남자_VALUE, "{}", "{@}");
                }
                else
                {
                    strRefer = VB.STRCUT(clsComVariable.REFER_OLD_여자_VALUE, "{}", "{@}");
                }
            }

            if (argGbUnit == "Y")
            {
                if (VB.Pstr(strRefer, "{}", 1) == "~")
                {
                    strRefer = "";
                }
                else
                {
                    strRefer = strRefer.Replace("{}", "");
                }
            }
            else
            {
                strRefer = VB.Pstr(strRefer, "{}", 1);
            }

            rtnVal = strRefer;

            return rtnVal;
        }

        /// <summary>
        /// 건진종류명을 READ
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_GjJong_Name(string strCode)
        {
            string rtnVal = string.Empty;

            if (strCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }
            rtnVal = comHpcService.Read_Hic_ExJong_Name(strCode.Trim());
            //rtnVal = heaExjongService.Read_ExJong_Name(strCode.Trim());

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

    }
}
