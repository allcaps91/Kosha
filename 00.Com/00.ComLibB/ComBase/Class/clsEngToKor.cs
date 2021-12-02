using System;

namespace ComBase
{
    public class clsEngToKor
    {

        public const double HAN_FIRST = 44032.0;
        //-----------------------------------------------------------------------------------
        // 한글 초, 중, 종성 테이블
        //-----------------------------------------------------------------------------------
        public static string[] strArrChoseong = {"ㄱ",       "ㄲ",        "ㄴ",        "ㄷ",        "ㄸ",        "ㄹ",        "ㅁ",        "ㅂ",
                                                        "ㅃ",        "ㅅ",        "ㅆ",        "ㅇ",        "ㅈ",        "ㅉ",
                                                        "ㅊ",        "ㅋ",        "ㅌ",        "ㅍ",        "ㅎ" };
        public static string[] strArrJungseong = {"ㅏ",      "ㅐ",        "ㅑ",        "ㅒ",        "ㅓ",        "ㅔ",        "ㅕ",        "ㅖ",
                                                            "ㅗ",        "ㅘ",        "ㅙ",        "ㅚ",        "ㅛ",        "ㅜ",
                                                            "ㅝ",        "ㅞ",        "ㅟ",        "ㅠ",        "ㅡ",        "ㅢ",        "ㅣ" };
        public static string[] strArrJongseong = {  "",     "ㄱ",        "ㄲ",        "ㄳ",        "ㄴ",        "ㄵ",        "ㄶ",        "ㄷ",        "ㄹ",        "ㄺ",
                                                            "ㄻ",        "ㄼ",        "ㄽ",        "ㄾ",        "ㄿ",
                                                            "ㅀ",        "ㅁ",        "ㅂ",        "ㅄ",        "ㅅ",
                                                            "ㅆ",        "ㅇ",        "ㅈ",        "ㅊ",        "ㅋ",        "ㅌ",        "ㅍ",        "ㅎ" };
        //-----------------------------------------------------------------------------------
        // 한글 초, 중, 종성 분리
        //-----------------------------------------------------------------------------------
        public static int UDF_HanPas(string strHangul, ref string strChoseong, ref string strJungseong, ref string strJongseong)
        {
            int functionReturnValue = 0;
            double dCode = 0;
            int iChoseong = 0;
            int iJungseong = 0;
            int iJongseong = 0;

            //// 입력된 문자의 유니코드 값을 가져옵니다.
            dCode = Convert.ToDouble("&h" + VB.Hex(VB.AscW(strHangul)));

            //// 입력문자가 '가'(한글 조합문자의 처음 코드)보다 작으면 실행을 취소합니다.
            if ((dCode < HAN_FIRST))
            {
                return functionReturnValue;
            }
            dCode = dCode - HAN_FIRST;

            //// 초,중,종성의 한글 테이블 인덱스 값을 얻습니다.
            //// 21 = 중성 테이블의 원소 수
            //// 28 = 종성 테이블의 원소 수

            iChoseong = Convert.ToInt32(dCode / (21 * 28));
            dCode = dCode % (21 * 28);
            iJungseong = Convert.ToInt32(dCode / 28);
            iJongseong = Convert.ToInt32(dCode % 28);

            //// 테이블의 상한 인덱스 값을 넘는지 검사합니다.

            if ((iChoseong > 18 | iJungseong > 20 | iJongseong > 27))
            {
                return functionReturnValue;
            }

            //// 인덱스 값을 갖는 문자를 얻습니다.
            strChoseong = strArrChoseong[iChoseong];
            strJungseong = strArrJungseong[iJungseong];
            strJongseong = strArrJongseong[iJongseong];

            //// 종성이 없는 경우를 위해 반환되는 수를 리턴합니다.
            //// 2 = 종성 없음, 3 = 종성 있음
            functionReturnValue = (!string.IsNullOrEmpty(strJongseong) ? 3 : 2);
            return functionReturnValue;
        }

        //-----------------------------------------------------------------------------------
        // 한글 초, 중, 종성 조합
        //-----------------------------------------------------------------------------------
        public static string UDF_HanPas_Rev(string strChoseong, string strJungseong, string strJongseong = "")
        {
            string functionReturnValue = null;
            int iChoseong = 0;
            int iJungseong = 0;
            int iJongseong = 0;
            int i = 0;

            //// 입력된 초성과 같은 문자를 갖는 인덱스를 구합니다.
            iChoseong = -1;
            for (i = 0; i <= 18; i++)
            {
                //If (strChoseong = strArrChoseong(i)) Then
                if ((strChoseong == strArrChoseong[i]))
                {
                    iChoseong = i;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            if (iChoseong < 0)
            {
                return functionReturnValue;
            }

            //// 입력된 중성과 같은 문자를 갖는 인덱스를 구합니다.
            iJungseong = -1;
            for (i = 0; i <= 20; i++)
            {
                if ((strJungseong == strArrJungseong[i]))
                {
                    iJungseong = i;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            if ((iJungseong < 0))
            {
                return functionReturnValue;
            }

            //// 입력된 종성과 같은 문자를 갖는 인덱스를 구합니다.
            iJongseong = -1;
            for (i = 0; i <= 27; i++)
            {
                if ((strJongseong == strArrJongseong[i]))
                {
                    iJongseong = i;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            if ((iJongseong < 0))
            {
                return functionReturnValue;
            }

            //// 각 인덱스 값을 수식에 대입하여 유니코드 값을 얻어 반환합니니다.
            //// 21 = 중성 테이블의 원소 수
            //// 28 = 종성 테이블의 원소 수
            //// HAN_FIRST = 유니코드 '가'의 코드 값(한글 조합문자의 처음 코드)
            //functionReturnValue = VB.ChrW(((iChoseong * 21 + iJungseong) * 28 + iJongseong + HAN_FIRST);
            functionReturnValue = Convert.ToString(VB.ChrW(Convert.ToInt32(((iChoseong * 21 + iJungseong) * 28 + iJongseong + HAN_FIRST))));
            return functionReturnValue;
        }

        //-----------------------------------------------------------------------------------
        // 영어로 입력된 단어를 한글로 자동변환
        //-----------------------------------------------------------------------------------
        public static string UDF_Eng2Han(string engStr)
        {
            string functionReturnValue = null;

            string hanStr = null;
            int i = 0;
            string retStr = null;
            string oneChar = null;
            string oneHan = null;
            string twoHan = null;

            engStr = VB.Trim(engStr);
            for (i = 1; i <= VB.Len(engStr); i++)
            {
                switch (VB.Mid(engStr, i, 2))
                {
                    case "rt":
                    case "sw":
                    case "sg":
                    case "fr":
                    case "fa":
                    case "fq":
                    case "ft":
                    case "fx":
                    case "fv":
                    case "fg":
                    case "qt":
                        //자음두개짜리
                        //자음두개로 끝나는 경우
                        if (VB.Len(engStr) == i + 1)
                        {
                            oneChar = UDF_Eng2Han_OneChar(VB.Mid(engStr, i, 2));
                            i = i + 1;
                        }
                        else
                        {
                            switch (VB.Mid(engStr, i + 2, 1))
                            {
                                case "q":
                                case "w":
                                case "e":
                                case "r":
                                case "t":
                                case "a":
                                case "s":
                                case "d":
                                case "f":
                                case "g":
                                case "z":
                                case "x":
                                case "c":
                                case "v":
                                case "Q":
                                case "W":
                                case "E":
                                case "R":
                                case "T":
                                    //자음 두개 다음에 자음이 오는 경우
                                    oneChar = UDF_Eng2Han_OneChar(VB.Mid(engStr, i, 2));
                                    i = i + 1;
                                    break;
                                default:
                                    oneChar = UDF_Eng2Han_OneChar(VB.Mid(engStr, i, 1));
                                    //자음 두개 다음에 모음이 오는 경우는 자음한개로 봐야 함
                                    break;
                            }
                        }
                        break;
                    case "hk":
                    case "ho":
                    case "hl":
                    case "nj":
                    case "np":
                    case "nl":
                    case "ml":
                        //모음두개짜리
                        oneChar = UDF_Eng2Han_OneChar(VB.Mid(engStr, i, 2));
                        i = i + 1;
                        break;
                    default:
                        oneChar = UDF_Eng2Han_OneChar(VB.Mid(engStr, i, 1));
                        break;
                }
                if (string.IsNullOrEmpty(oneChar))
                {
                    return functionReturnValue;
                }
                else
                {
                    hanStr = hanStr + oneChar;
                }
            }

            i = 1;

            int i_ = 0;
            int i_1 = 0;
            int i_2 = 0;

            while (i <= VB.Len(hanStr))
            {
                if (VB.Len(hanStr) == i)
                {
                    //마지막에 한 개만 남은 경우 에러
                    retStr = retStr + VB.Mid(hanStr, i, 1);
                    functionReturnValue = retStr;

                    return functionReturnValue;
                }
                else if (VB.Len(hanStr) == i + 1)
                {
                    oneHan = UDF_HanPas_Rev(VB.Mid(hanStr, i, 1), VB.Mid(hanStr, i + 1, 1));
                    if (string.IsNullOrEmpty(oneHan))
                    {
                        retStr = retStr + VB.Mid(hanStr, i, 2);
                        i = i + 2;
                        // Exit Function '두 개 남았는데 글자가 아닌 경우 에러
                    }
                    else
                    {
                        retStr = retStr + oneHan;
                        i = i + 2;
                    }
                }
                else if (VB.Len(hanStr) == i + 2)
                {
                    oneHan = UDF_HanPas_Rev(VB.Mid(hanStr, i, 1), VB.Mid(hanStr, i + 1, 1), VB.Mid(hanStr, i + 2, 1));
                    if (string.IsNullOrEmpty(oneHan))
                    {
                        oneHan = UDF_HanPas_Rev(VB.Mid(hanStr, i, 1), VB.Mid(hanStr, i + 1, 1));
                        if (string.IsNullOrEmpty(oneHan))
                        {
                            retStr = retStr + VB.Mid(hanStr, i, 1);
                            i = i + 1;
                        }
                        else
                        {
                            retStr = retStr + oneHan;
                            i = i + 2;
                        }
                        // Exit Function '세 개 남았는데 글자가 아닌 경우 에러
                    }
                    else
                    {
                        retStr = retStr + oneHan;
                        i = i + 3;
                    }
                }
                else
                {
                    oneHan = UDF_HanPas_Rev(VB.Mid(hanStr, i, 1), VB.Mid(hanStr, i + 1, 1));
                    if (string.IsNullOrEmpty(oneHan))
                    {
                        retStr = retStr + VB.Mid(hanStr, i, 1);
                        i = i + 1;
                        // Exit Function '앞에 두 개가 글자가 아닌 경우 에러
                    }
                    else
                    {
                        twoHan = UDF_HanPas_Rev(VB.Mid(hanStr, i + 2, 1), VB.Mid(hanStr, i + 3, 1));
                        if (string.IsNullOrEmpty(twoHan))
                        {
                            oneHan = UDF_HanPas_Rev(VB.Mid(hanStr, i, 1), VB.Mid(hanStr, i + 1, 1), VB.Mid(hanStr, i + 2, 1));
                            if (string.IsNullOrEmpty(oneHan))
                            {
                                oneHan = UDF_HanPas_Rev(VB.Mid(hanStr, i, 1), VB.Mid(hanStr, i + 1, 1));
                                retStr = retStr + oneHan;
                                i = i + 2;
                                // Exit Function '세,네번째가 글자가 아닌데 앞에 세 개가 글자가 아닌 경우 에러'
                            }
                            else
                            {
                                retStr = retStr + oneHan;
                                i = i + 3;
                            }
                        }
                        else
                        {
                            retStr = retStr + oneHan;
                            i = i + 2;
                        }
                    }
                }
            }
            functionReturnValue = retStr;
            return functionReturnValue;
        }

        //단어를 자동변환해주는 부분
        public static string UDF_Eng2Han_OneChar(string engStr)
        {
            string functionReturnValue = null;
            if (VB.Len(engStr) > 2)
                return functionReturnValue;
            switch (engStr)
            {
                case "a":
                    functionReturnValue = "ㅁ";
                    break;
                case "b":
                    functionReturnValue = "ㅠ";
                    break;
                case "c":
                    functionReturnValue = "ㅊ";
                    break;
                case "d":
                    functionReturnValue = "ㅇ";
                    break;
                case "e":
                    functionReturnValue = "ㄷ";
                    break;
                case "f":
                    functionReturnValue = "ㄹ";
                    break;
                case "g":
                    functionReturnValue = "ㅎ";
                    break;
                case "h":
                    functionReturnValue = "ㅗ";
                    break;
                case "i":
                    functionReturnValue = "ㅑ";
                    break;
                case "j":
                    functionReturnValue = "ㅓ";
                    break;
                case "k":
                    functionReturnValue = "ㅏ";
                    break;
                case "l":
                    functionReturnValue = "ㅣ";
                    break;
                case "m":
                    functionReturnValue = "ㅡ";
                    break;
                case "n":
                    functionReturnValue = "ㅜ";
                    break;
                case "o":
                    functionReturnValue = "ㅐ";
                    break;
                case "p":
                    functionReturnValue = "ㅔ";
                    break;
                case "q":
                    functionReturnValue = "ㅂ";
                    break;
                case "r":
                    functionReturnValue = "ㄱ";
                    break;
                case "s":
                    functionReturnValue = "ㄴ";
                    break;
                case "t":
                    functionReturnValue = "ㅅ";
                    break;
                case "u":
                    functionReturnValue = "ㅕ";
                    break;
                case "v":
                    functionReturnValue = "ㅍ";
                    break;
                case "w":
                    functionReturnValue = "ㅈ";
                    break;
                case "x":
                    functionReturnValue = "ㅌ";
                    break;
                case "y":
                    functionReturnValue = "ㅛ";
                    break;
                case "z":
                    functionReturnValue = "ㅋ";
                    break;
                case "E":
                    functionReturnValue = "ㄸ";
                    break;
                case "O":
                    functionReturnValue = "ㅒ";
                    break;
                case "P":
                    functionReturnValue = "ㅖ";
                    break;
                case "Q":
                    functionReturnValue = "ㅃ";
                    break;
                case "R":
                    functionReturnValue = "ㄲ";
                    break;
                case "T":
                    functionReturnValue = "ㅆ";
                    break;
                case "W":
                    functionReturnValue = "ㅉ";
                    break;
                case "rt":
                    functionReturnValue = "ㄳ";
                    break;
                case "sw":
                    functionReturnValue = "ㄵ";
                    break;
                case "sg":
                    functionReturnValue = "ㄶ";
                    break;
                case "fr":
                    functionReturnValue = "ㄺ";
                    break;
                case "fa":
                    functionReturnValue = "ㄻ";
                    break;
                case "fq":
                    functionReturnValue = "ㄽ";
                    break;
                case "ft":
                    functionReturnValue = "ㄾ";
                    break;
                case "fx":
                    functionReturnValue = "ㄾ";
                    break;
                case "fv":
                    functionReturnValue = "ㄿ";
                    break;
                case "fg":
                    functionReturnValue = "ㅀ";
                    break;
                case "qt":
                    functionReturnValue = "ㅄ";
                    break;
                case "hk":
                    functionReturnValue = "ㅘ";
                    break;
                case "ho":
                    functionReturnValue = "ㅙ";
                    break;
                case "hl":
                    functionReturnValue = "ㅚ";
                    break;
                case "nj":
                    functionReturnValue = "ㅝ";
                    break;
                case "np":
                    functionReturnValue = "ㅞ";
                    break;
                case "nl":
                    functionReturnValue = "ㅟ";
                    break;
                case "ml":
                    functionReturnValue = "ㅢ";
                    break;
                default:
                    functionReturnValue = engStr;
                    break;
            }
            return functionReturnValue;
        }

        /// <summary>
        /// 전각 => 반각 
        /// </summary>
        /// <param name="sFull"></param>
        /// <returns></returns>
        private string Full2Half(string sFull)
        {
            char[] ch = sFull.ToCharArray(0, sFull.Length);
            for (int i = 0; i < sFull.Length; ++i)
            {
                if (ch[i] > 0xff00 && ch[i] <= 0xff5e)
                    ch[i] -= (char)0xfee0;
                else if (ch[i] == 0x3000)
                    ch[i] = (char)0x20;
            }
            return (new string(ch));
        }

        /// <summary>
        /// 반각 => 전각
        /// </summary>
        /// <param name="sHalf"></param>
        /// <returns></returns>
        private string Half2Full(string sHalf)
        {
            char[] ch = sHalf.ToCharArray(0, sHalf.Length);
            for (int i = 0; i < sHalf.Length; ++i)
            {
                if (ch[i] > 0x21 && ch[i] <= 0x7e)
                    ch[i] += (char)0xfee0;
                else if (ch[i] == 0x20)
                    ch[i] = (char)0x3000;
            }
            return (new string(ch));
        }

    }
}
