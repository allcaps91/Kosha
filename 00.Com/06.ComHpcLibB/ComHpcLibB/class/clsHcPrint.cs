using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System.Windows.Forms;
using ComBase;
using System;
using FarPoint.Win.Spread;
using System.Drawing.Printing;
using ComPmpaLibB;
using System.IO;
using ComBase.Mvc;

namespace ComHpcLibB
{
    public class clsHcPrint
    {

        HicJepsuService hicJepsuService = null;
        HicEmrResultService hicEmrResultService = null;
        HicEmrResultWorkService hicEmrResultWorkService = null; 

        public clsHcPrint()
        {
            hicJepsuService = new HicJepsuService();
            hicEmrResultService = new HicEmrResultService();
            hicEmrResultWorkService = new HicEmrResultWorkService();
        }



        public const string CARD_PRINTER_NAME = "신용카드";
        public const string JUP_PRINTER_NAME = "접수증";
        public const string RES_PRINTER_NAME = "예약증";
        public const string RECEIPT_PRINTER_NAME = "영수증";
        public const string DRUG_PRINTER_NAME = "원외처방전";
        public const string ARTICLE_PRINTER_NAME = "혈액환자정보";
        public const string MCRT_PRINTER_NAME = "증명서";

        /// <summary>
        /// S병변
        /// </summary>
        /// <param name="ArgSS, ArgP, ArgNum"></param>
        /// <seealso cref="HcPrint.bas> DisPlay_S병변위치() "/>
        public string DisPlay_S병변위치(Control ArgSS, string ArgP, string ArgNum)
        {
            string rtnVal = "";

            string strOK = "";

            for (int i = 1; i <= VB.Len(ArgP); i++)
            {
                if (VB.Mid(ArgP, i, 1) == ArgNum) { return rtnVal; }
                if (i == VB.Len(ArgP)) { return rtnVal; }
            }

            for (int i = 1; i <= VB.Len(ArgP); i++)
            {
                if (VB.Mid(ArgP, i, 1) == ArgNum)
                {
                    switch (i)
                    {
                        case 1: ArgSS.Text = rtnVal + "위저부,"; break;
                        case 2: ArgSS.Text = ArgSS.Text + "위저부,"; break;
                        case 3: ArgSS.Text = ArgSS.Text + "위저부,"; break;

                        case 4: ArgSS.Text = ArgSS.Text + "위체부,"; break;
                        case 5: ArgSS.Text = ArgSS.Text + "위체부,"; break;
                        case 6: ArgSS.Text = ArgSS.Text + "위체부,"; break;

                        case 7: ArgSS.Text = ArgSS.Text + "위전정부,"; break;
                        case 8: ArgSS.Text = ArgSS.Text + "위전정부,"; break;
                        case 9: ArgSS.Text = ArgSS.Text + "위전정부,"; break;

                        case 10: ArgSS.Text = ArgSS.Text + "위분문부,"; break;
                        case 11: ArgSS.Text = ArgSS.Text + "위분문부,"; break;
                        case 12: ArgSS.Text = ArgSS.Text + "위분문부,"; break;

                        case 13: ArgSS.Text = ArgSS.Text + "소만,"; break;
                        case 14: ArgSS.Text = ArgSS.Text + "소만,"; break;
                        case 15: ArgSS.Text = ArgSS.Text + "소만,"; break;

                        case 16: ArgSS.Text = ArgSS.Text + "대만,"; break;
                        case 17: ArgSS.Text = ArgSS.Text + "대만,"; break;
                        case 18: ArgSS.Text = ArgSS.Text + "대만,"; break;

                        case 19: ArgSS.Text = ArgSS.Text + "전벽,"; break;
                        case 20: ArgSS.Text = ArgSS.Text + "전벽,"; break;
                        case 21: ArgSS.Text = ArgSS.Text + "전벽,"; break;

                        case 22: ArgSS.Text = ArgSS.Text + "후벽,"; break;
                        case 23: ArgSS.Text = ArgSS.Text + "후벽,"; break;
                        case 24: ArgSS.Text = ArgSS.Text + "후벽,"; break;

                            //default: rtnVal = ""; break;
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// S병변1
        /// </summary>
        /// <param name="ArgSS, ArgP, ArgNum"></param>
        /// <seealso cref="HcPrint.bas> DisPlay_S병변위치() "/>
        public string DisPlay_S병변위치1(Control ArgSS, string ArgP, int ArgNum)
        {
            int No = 0;
            string rtnVal = "";
            string strOK = "";
            string strTemp = "";

            for (int i = 0; i < VB.Len(ArgP); i += 3)
            {
                strTemp = VB.Mid(ArgP, i, 3);
                if (VB.Val(VB.Mid(strTemp, ArgNum, 1)) >= 1) { return rtnVal; }
                if (i == VB.Len(ArgP)) { return rtnVal; }
            }

            for (int i = 1; i <= VB.Len(ArgP); i += 3)
            {
                No = No + 1;
                strTemp = VB.Mid(ArgP, i, 3);
                if (VB.Val(VB.Mid(strTemp, ArgNum, 1)) >= 1)
                {
                    switch (No)
                    {
                        case 1: ArgSS.Text = ArgSS.Text + "위저부,"; break;
                        case 2: ArgSS.Text = ArgSS.Text + "위체부,"; break;
                        case 3: ArgSS.Text = ArgSS.Text + "위전정부,"; break;
                        case 4: ArgSS.Text = ArgSS.Text + "위분문부,"; break;
                        case 5: ArgSS.Text = ArgSS.Text + "소만,"; break;
                        case 6: ArgSS.Text = ArgSS.Text + "대만,"; break;
                        case 7: ArgSS.Text = ArgSS.Text + "전벽,"; break;
                        case 8: ArgSS.Text = ArgSS.Text + "후벽,"; break;
                            //default: rtnVal = ""; break;
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 위조직암
        /// </summary>
        /// <param name="ArgSS, ArgVar, ArgEtc"></param>
        /// <seealso cref="HcPrint.bas> DisPlay_위조직암() "/>
        public string DisPlay_위조직암(Control ArgSS, string ArgVar, string ArgEtc)
        {

            string rtnVal = "";

            for (int i = 1; i <= VB.Len(ArgVar); i++)
            {
                if (VB.Mid(ArgVar, i, 1) == "1")
                {
                    switch (i)
                    {
                        case 1: ArgSS.Text = ArgSS.Text + "관상샘암종(고분화,중분화,저분화),"; break;
                        case 2: ArgSS.Text = ArgSS.Text + "유두상샘암종,"; break;
                        case 3: ArgSS.Text = ArgSS.Text + "반지세포암종,"; break;
                        case 4: ArgSS.Text = ArgSS.Text + "위림프종(저도,고도),"; break;
                        case 5: ArgSS.Text = ArgSS.Text + "점액(샘)암종,"; break;
                        case 6: ArgSS.Text = ArgSS.Text + "샘편평상피암종,"; break;
                        case 7: ArgSS.Text = ArgSS.Text + "편평상피암종,"; break;
                        case 8: ArgSS.Text = ArgSS.Text + "소세포암종,"; break;
                        case 9: ArgSS.Text = ArgSS.Text + "미분화암종,"; break;
                        case 10: ArgSS.Text = ArgSS.Text + "미분화암종,"; break;
                        case 11: ArgSS.Text = ArgSS.Text + ArgEtc; break;
                            //default: rtnVal = ""; break;
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 위조직암_2016
        /// </summary>
        /// <param name="ArgSS, ArgVar, ArgEtc"></param>
        /// <seealso cref="HcPrint.bas> DisPlay_위조직암() "/>
        public string DisPlay_위조직암_2016(Control ArgSS, string ArgVar, string ArgEtc)
        {

            string rtnVal = "";

            for (int i = 1; i <= VB.Len(ArgVar); i++)
            {
                if (VB.Mid(ArgVar, i, 1) == "1")
                {
                    switch (i)
                    {
                        case 1: ArgSS.Text = ArgSS.Text + "관상샘암종(고분화),"; break;
                        case 2: ArgSS.Text = ArgSS.Text + "관상샘암종(중분화),"; break;
                        case 3: ArgSS.Text = ArgSS.Text + "관상샘암종(저분화),"; break;
                        case 4: ArgSS.Text = ArgSS.Text + "유두상샘암종,"; break;
                        case 5: ArgSS.Text = ArgSS.Text + "반지세포암종,"; break;
                        case 6: ArgSS.Text = ArgSS.Text + "위림프종(저도),"; break;
                        case 7: ArgSS.Text = ArgSS.Text + "위림프종(고도),"; break;
                        case 8: ArgSS.Text = ArgSS.Text + "점액(샘)암종,"; break;
                        case 9: ArgSS.Text = ArgSS.Text + "샘편평상피암종,"; break;
                        case 10: ArgSS.Text = ArgSS.Text + "편평상피암종,"; break;
                        case 11: ArgSS.Text = ArgSS.Text + "소세포암종,"; break;
                        case 12: ArgSS.Text = ArgSS.Text + "미분화암종,"; break;
                        case 13: ArgSS.Text = ArgSS.Text + "신경내분비종양,"; break;
                        case 14: ArgSS.Text = ArgSS.Text + ArgEtc; break;
                            //default: rtnVal = ""; break;
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 위조직암_2016
        /// </summary>
        /// <param name="ArgSS, ArgP, ArgNum"></param>
        /// <seealso cref="HcPrint.bas> DisPlay_위조직암기타() "/>
        public string DisPlay_위조직암기타(Control ArgSS, string ArgVar, string ArgEtc)
        {

            string rtnVal = "";

            for (int i = 1; i <= VB.Len(ArgVar); i++)
            {
                if (VB.Mid(ArgVar, i, 1) == "1")
                {
                    switch (i)
                    {
                        case 1: ArgSS.Text = ArgSS.Text + "위의비상피성종양,"; break;
                        case 2: ArgSS.Text = ArgSS.Text + "식도염,"; break;
                        case 3: ArgSS.Text = ArgSS.Text + "식도암종,"; break;
                        case 4: ArgSS.Text = ArgSS.Text + "식도 점막하종양,"; break;
                        case 5: ArgSS.Text = ArgSS.Text + "십이지장궤양,"; break;
                        case 6: ArgSS.Text = ArgSS.Text + "십이지장암종,"; break;
                        case 7: ArgSS.Text = ArgSS.Text + "십이지장점막하종양,"; break;
                        case 8: ArgSS.Text = ArgSS.Text + ArgEtc; break;
                            //default: rtnVal = ""; break;
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 위조직암_C병변위치
        /// </summary>
        /// <param name="ArgSS, ArgP"></param>
        /// <seealso cref="HcPrint.bas> DisPlay_C병변위치() "/>
        public string DisPlay_C병변위치(Control ArgSS, string ArgP)
        {

            string rtnVal = "";
            string strOK = "";

            for (int i = 1; i <= 5; i++)
            {
                for (int j = 1; i <= VB.Len(ArgP); j++)
                {
                    if (VB.Val(VB.Mid(ArgP, j, 1)) == i)
                    {
                        switch (i)
                        {
                            case 1:
                            case 2: ArgSS.Text = ArgSS.Text + "용종: "; strOK = "OK"; break;
                            case 3: ArgSS.Text = ArgSS.Text + "대장암의심: "; strOK = "OK"; break;
                            case 4: ArgSS.Text = ArgSS.Text + "대장암: "; strOK = "OK"; break;
                            case 5: ArgSS.Text = ArgSS.Text + "기타: "; strOK = "OK"; break;
                                //default: rtnVal = ""; break;
                        }
                        if (rtnVal != "") { break; }
                    }
                }

                for (int j = 1; i <= VB.Len(ArgP); j++)
                {

                    if (VB.Val(VB.Mid(ArgP, j, 1)) == i)
                    {
                        switch (j)
                        {
                            case 1: ArgSS.Text = ArgSS.Text + "회장단말부,"; break;
                            case 2: ArgSS.Text = ArgSS.Text + "회장단말부,"; break;
                            case 3: ArgSS.Text = ArgSS.Text + "회장단말부,"; break;

                            case 4: ArgSS.Text = ArgSS.Text + "맹장,"; break;
                            case 5: ArgSS.Text = ArgSS.Text + "맹장,"; break;
                            case 6: ArgSS.Text = ArgSS.Text + "맹장,"; break;

                            case 7: ArgSS.Text = ArgSS.Text + "상행결장,"; break;
                            case 8: ArgSS.Text = ArgSS.Text + "상행결장,"; break;
                            case 9: ArgSS.Text = ArgSS.Text + "상행결장,"; break;

                            case 10: ArgSS.Text = ArgSS.Text + "간만곡,"; break;
                            case 11: ArgSS.Text = ArgSS.Text + "간만곡,"; break;
                            case 12: ArgSS.Text = ArgSS.Text + "간만곡,"; break;

                            case 13: ArgSS.Text = ArgSS.Text + "횡행결장,"; break;
                            case 14: ArgSS.Text = ArgSS.Text + "횡행결장,"; break;
                            case 15: ArgSS.Text = ArgSS.Text + "횡행결장,"; break;

                            case 16: ArgSS.Text = ArgSS.Text + "비만곡,"; break;
                            case 17: ArgSS.Text = ArgSS.Text + "비만곡,"; break;
                            case 18: ArgSS.Text = ArgSS.Text + "비만곡,"; break;

                            case 19: ArgSS.Text = ArgSS.Text + "하행결장,"; break;
                            case 20: ArgSS.Text = ArgSS.Text + "하행결장,"; break;
                            case 21: ArgSS.Text = ArgSS.Text + "하행결장,"; break;

                            case 22: ArgSS.Text = ArgSS.Text + "에스결장,"; break;
                            case 23: ArgSS.Text = ArgSS.Text + "에스결장,"; break;
                            case 24: ArgSS.Text = ArgSS.Text + "에스결장,"; break;

                            case 25: ArgSS.Text = ArgSS.Text + "직장,"; break;
                            case 26: ArgSS.Text = ArgSS.Text + "직장,"; break;
                            case 27: ArgSS.Text = ArgSS.Text + "직장,"; break;

                            case 28: ArgSS.Text = ArgSS.Text + "항문,"; break;
                            case 29: ArgSS.Text = ArgSS.Text + "항문,"; break;
                            case 30: ArgSS.Text = ArgSS.Text + "항문,"; break;
                                //default: rtnVal = ""; break;
                        }
                        if (strOK == "OK")
                        {
                            ArgSS.Text = ArgSS.Text + ComNum.VBLF;
                        }
                        strOK = "";
                    }
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// 위조직암_C병변위치
        /// </summary>
        /// <param name="ArgSS, ArgP"></param>
        /// <seealso cref="HcPrint.bas> DisPlay_병변위치() "/>
        public string DisPlay_병변위치(Control ArgSS, string ArgP)
        {

            string rtnVal = "";
            string strOK = "";

            for (int i = 1; i <= 5; i++)
            {
                for (int j = 1; i <= VB.Len(ArgP); j++)
                {
                    if (VB.Val(VB.Mid(ArgP, j, 1)) == i)
                    {
                        switch (i)
                        {
                            case 1:
                            case 2: ArgSS.Text = ArgSS.Text + "용종: "; strOK = "OK"; break;
                            case 3: ArgSS.Text = ArgSS.Text + "대장암의심: "; strOK = "OK"; break;
                            case 4: ArgSS.Text = ArgSS.Text + "대장암: "; strOK = "OK"; break;
                            case 5: ArgSS.Text = ArgSS.Text + "기타: "; strOK = "OK"; break;
                                //default: rtnVal = ""; break;
                        }
                        if (rtnVal != "") { break; }
                    }
                }

                for (int j = 1; i <= VB.Len(ArgP); j++)
                {

                    if (VB.Val(VB.Mid(ArgP, j, 1)) == i)
                    {
                        switch (j)
                        {
                            case 1: ArgSS.Text = ArgSS.Text + "회장단말부,"; break;
                            case 2: ArgSS.Text = ArgSS.Text + "회장단말부,"; break;
                            case 3: ArgSS.Text = ArgSS.Text + "회장단말부,"; break;

                            case 4: ArgSS.Text = ArgSS.Text + "맹장,"; break;
                            case 5: ArgSS.Text = ArgSS.Text + "맹장,"; break;
                            case 6: ArgSS.Text = ArgSS.Text + "맹장,"; break;

                            case 7: ArgSS.Text = ArgSS.Text + "상행결장,"; break;
                            case 8: ArgSS.Text = ArgSS.Text + "상행결장,"; break;
                            case 9: ArgSS.Text = ArgSS.Text + "상행결장,"; break;

                            case 10: ArgSS.Text = ArgSS.Text + "간만곡,"; break;
                            case 11: ArgSS.Text = ArgSS.Text + "간만곡,"; break;
                            case 12: ArgSS.Text = ArgSS.Text + "간만곡,"; break;

                            case 13: ArgSS.Text = ArgSS.Text + "횡행결장,"; break;
                            case 14: ArgSS.Text = ArgSS.Text + "횡행결장,"; break;
                            case 15: ArgSS.Text = ArgSS.Text + "횡행결장,"; break;

                            case 16: ArgSS.Text = ArgSS.Text + "비만곡,"; break;
                            case 17: ArgSS.Text = ArgSS.Text + "비만곡,"; break;
                            case 18: ArgSS.Text = ArgSS.Text + "비만곡,"; break;

                            case 19: ArgSS.Text = ArgSS.Text + "하행결장,"; break;
                            case 20: ArgSS.Text = ArgSS.Text + "하행결장,"; break;
                            case 21: ArgSS.Text = ArgSS.Text + "하행결장,"; break;

                            case 22: ArgSS.Text = ArgSS.Text + "에스결장,"; break;
                            case 23: ArgSS.Text = ArgSS.Text + "에스결장,"; break;
                            case 24: ArgSS.Text = ArgSS.Text + "에스결장,"; break;

                            case 25: ArgSS.Text = ArgSS.Text + "직장,"; break;
                            case 26: ArgSS.Text = ArgSS.Text + "직장,"; break;
                            case 27: ArgSS.Text = ArgSS.Text + "직장,"; break;

                            case 28: ArgSS.Text = ArgSS.Text + "항문,"; break;
                            case 29: ArgSS.Text = ArgSS.Text + "항문,"; break;
                            case 30: ArgSS.Text = ArgSS.Text + "항문,"; break;
                                //default: rtnVal = ""; break;
                        }
                        if (strOK == "OK")
                        {
                            ArgSS.Text = ArgSS.Text + ComNum.VBLF;
                        }
                        strOK = "";
                    }
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// 유방병변위치
        /// </summary>
        /// <param name="ArgSS, ArgP, ArgPEtc"></param>
        /// <seealso cref="HcPrint.bas> DisPlay_유방병변위치() "/>
        public string DisPlay_유방병변위치(Control ArgSS, string ArgP, string ArgPEtc)
        {

            string rtnVal = "";
            string strOK = "";

            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; i <= 36; j += 2)
                {
                    if (VB.Val(VB.Mid(ArgP, j, 2)) == i)
                    {
                        switch (i)
                        {
                            case 1:
                            case 2: ArgSS.Text = ArgSS.Text + "종괴: "; strOK = "OK"; break;
                            case 3: ArgSS.Text = ArgSS.Text + "양성석회화: "; strOK = "OK"; break;
                            case 4: ArgSS.Text = ArgSS.Text + "미세석회화: "; strOK = "OK"; break;
                            case 5: ArgSS.Text = ArgSS.Text + "구조왜곡: "; strOK = "OK"; break;
                            case 6: ArgSS.Text = ArgSS.Text + "비대칭: "; strOK = "OK"; break;
                            case 7: ArgSS.Text = ArgSS.Text + "피부이상: "; strOK = "OK"; break;
                            case 8: ArgSS.Text = ArgSS.Text + "임파선비후: "; strOK = "OK"; break;
                            case 9: ArgSS.Text = ArgSS.Text + "판정곤란: "; strOK = "OK"; break;
                            case 10: ArgSS.Text = ArgSS.Text + " " + ArgPEtc + ": "; strOK = "OK"; break;
                                //default: rtnVal = ""; break;
                        }
                        if (rtnVal != "") { break; }
                    }
                }

                for (int j = 1; i <= 36; j += 2)
                {

                    if (VB.Val(VB.Mid(ArgP, j, 2)) == i)
                    {
                        switch (j)
                        {
                            case 1: ArgSS.Text = ArgSS.Text + "상외측,"; break;
                            case 2: ArgSS.Text = ArgSS.Text + "상외측,"; break;
                            case 3: ArgSS.Text = ArgSS.Text + "상외측,"; break;
                            case 4: ArgSS.Text = ArgSS.Text + "상외측,"; break;
                            case 5: ArgSS.Text = ArgSS.Text + "상외측,"; break;
                            case 6: ArgSS.Text = ArgSS.Text + "상외측,"; break;

                            case 7: ArgSS.Text = ArgSS.Text + "상내측,"; break;
                            case 8: ArgSS.Text = ArgSS.Text + "상내측,"; break;
                            case 9: ArgSS.Text = ArgSS.Text + "상내측,"; break;
                            case 10: ArgSS.Text = ArgSS.Text + "상내측,"; break;
                            case 11: ArgSS.Text = ArgSS.Text + "상내측,"; break;
                            case 12: ArgSS.Text = ArgSS.Text + "상내측,"; break;

                            case 13: ArgSS.Text = ArgSS.Text + "하외측,"; break;
                            case 14: ArgSS.Text = ArgSS.Text + "하외측,"; break;
                            case 15: ArgSS.Text = ArgSS.Text + "하외측,"; break;
                            case 16: ArgSS.Text = ArgSS.Text + "하외측,"; break;
                            case 17: ArgSS.Text = ArgSS.Text + "하외측,"; break;
                            case 18: ArgSS.Text = ArgSS.Text + "하외측,"; break;

                            case 19: ArgSS.Text = ArgSS.Text + "하내측,"; break;
                            case 20: ArgSS.Text = ArgSS.Text + "하내측,"; break;
                            case 21: ArgSS.Text = ArgSS.Text + "하내측,"; break;
                            case 22: ArgSS.Text = ArgSS.Text + "하내측,"; break;
                            case 23: ArgSS.Text = ArgSS.Text + "하내측,"; break;
                            case 24: ArgSS.Text = ArgSS.Text + "하내측,"; break;

                            case 25: ArgSS.Text = ArgSS.Text + "유두하부,"; break;
                            case 26: ArgSS.Text = ArgSS.Text + "유두하부,"; break;
                            case 27: ArgSS.Text = ArgSS.Text + "유두하부,"; break;
                            case 28: ArgSS.Text = ArgSS.Text + "유두하부,"; break;
                            case 29: ArgSS.Text = ArgSS.Text + "유두하부,"; break;
                            case 30: ArgSS.Text = ArgSS.Text + "유두하부,"; break;

                            case 31: ArgSS.Text = ArgSS.Text + "액와부,"; break;
                            case 32: ArgSS.Text = ArgSS.Text + "액와부,"; break;
                            case 33: ArgSS.Text = ArgSS.Text + "액와부,"; break;
                            case 34: ArgSS.Text = ArgSS.Text + "액와부,"; break;
                            case 35: ArgSS.Text = ArgSS.Text + "액와부,"; break;
                            case 36: ArgSS.Text = ArgSS.Text + "액와부,"; break;
                                //default: rtnVal = ""; break;
                        }
                        if (strOK == "OK")
                        {
                            ArgSS.Text = ArgSS.Text + ComNum.VBLF;
                        }
                        strOK = "";
                    }
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// 유방병변위치L
        /// </summary>
        /// <param name="ArgSS, ArgP, ArgPEtc"></param>
        /// <seealso cref="HcPrint.bas> DisPlay_유방병변위치L() "/>
        public string DisPlay_유방병변위치L(Control ArgSS, string ArgP, string ArgPEtc)
        {

            string rtnVal = "";
            string strOK = "";

            for (int i = 1; i <= 10; i++)
            {
                for (int j = 37; i <= 72; j += 2)
                {
                    if (VB.Val(VB.Mid(ArgP, j, 2)) == i)
                    {
                        switch (i)
                        {
                            case 1:
                            case 2: ArgSS.Text = ArgSS.Text + "종괴: "; strOK = "OK"; break;
                            case 3: ArgSS.Text = ArgSS.Text + "양성석회화: "; strOK = "OK"; break;
                            case 4: ArgSS.Text = ArgSS.Text + "미세석회화: "; strOK = "OK"; break;
                            case 5: ArgSS.Text = ArgSS.Text + "구조왜곡: "; strOK = "OK"; break;
                            case 6: ArgSS.Text = ArgSS.Text + "비대칭: "; strOK = "OK"; break;
                            case 7: ArgSS.Text = ArgSS.Text + "피부이상: "; strOK = "OK"; break;
                            case 8: ArgSS.Text = ArgSS.Text + "임파선비후: "; strOK = "OK"; break;
                            case 9: ArgSS.Text = ArgSS.Text + "판정곤란: "; strOK = "OK"; break;
                            case 10: ArgSS.Text = ArgSS.Text + " " + ArgPEtc + ": "; strOK = "OK"; break;
                                //default: rtnVal = ""; break;
                        }
                        if (rtnVal != "") { break; }
                    }
                }

                for (int j = 37; i <= 72; j += 2)
                {
                    if (VB.Val(VB.Mid(ArgP, j, 2)) == i)
                    {
                        switch (j)
                        {
                            case 1: ArgSS.Text = ArgSS.Text + "상외측,"; break;
                            case 2: ArgSS.Text = ArgSS.Text + "상외측,"; break;
                            case 3: ArgSS.Text = ArgSS.Text + "상외측,"; break;
                            case 4: ArgSS.Text = ArgSS.Text + "상외측,"; break;
                            case 5: ArgSS.Text = ArgSS.Text + "상외측,"; break;
                            case 6: ArgSS.Text = ArgSS.Text + "상외측,"; break;

                            case 7: ArgSS.Text = ArgSS.Text + "상내측,"; break;
                            case 8: ArgSS.Text = ArgSS.Text + "상내측,"; break;
                            case 9: ArgSS.Text = ArgSS.Text + "상내측,"; break;
                            case 10: ArgSS.Text = ArgSS.Text + "상내측,"; break;
                            case 11: ArgSS.Text = ArgSS.Text + "상내측,"; break;
                            case 12: ArgSS.Text = ArgSS.Text + "상내측,"; break;

                            case 13: ArgSS.Text = ArgSS.Text + "하외측,"; break;
                            case 14: ArgSS.Text = ArgSS.Text + "하외측,"; break;
                            case 15: ArgSS.Text = ArgSS.Text + "하외측,"; break;
                            case 16: ArgSS.Text = ArgSS.Text + "하외측,"; break;
                            case 17: ArgSS.Text = ArgSS.Text + "하외측,"; break;
                            case 18: ArgSS.Text = ArgSS.Text + "하외측,"; break;

                            case 19: ArgSS.Text = ArgSS.Text + "하내측,"; break;
                            case 20: ArgSS.Text = ArgSS.Text + "하내측,"; break;
                            case 21: ArgSS.Text = ArgSS.Text + "하내측,"; break;
                            case 22: ArgSS.Text = ArgSS.Text + "하내측,"; break;
                            case 23: ArgSS.Text = ArgSS.Text + "하내측,"; break;
                            case 24: ArgSS.Text = ArgSS.Text + "하내측,"; break;

                            case 25: ArgSS.Text = ArgSS.Text + "유두하부,"; break;
                            case 26: ArgSS.Text = ArgSS.Text + "유두하부,"; break;
                            case 27: ArgSS.Text = ArgSS.Text + "유두하부,"; break;
                            case 28: ArgSS.Text = ArgSS.Text + "유두하부,"; break;
                            case 29: ArgSS.Text = ArgSS.Text + "유두하부,"; break;
                            case 30: ArgSS.Text = ArgSS.Text + "유두하부,"; break;

                            case 31: ArgSS.Text = ArgSS.Text + "액와부,"; break;
                            case 32: ArgSS.Text = ArgSS.Text + "액와부,"; break;
                            case 33: ArgSS.Text = ArgSS.Text + "액와부,"; break;
                            case 34: ArgSS.Text = ArgSS.Text + "액와부,"; break;
                            case 35: ArgSS.Text = ArgSS.Text + "액와부,"; break;
                            case 36: ArgSS.Text = ArgSS.Text + "액와부,"; break;
                                //default: rtnVal = ""; break;
                        }
                        if (strOK == "OK")
                        {
                            ArgSS.Text = ArgSS.Text + ComNum.VBLF;
                        }
                        strOK = "";
                    }
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// 간암유형
        /// </summary>
        /// <param name="ArgSS, ArgVar, ArgByeng, ArgSize"></param>
        /// <seealso cref="HcPrint.bas> DisPlay_간암유형() "/>
        public string DisPlay_간암유형(Control ArgSS, string ArgVar, string ArgByeng, string ArgSize)
        {

            string rtnVal = "";

            ArgSS.Text = ArgSS.Text + ComNum.VBLF;
            ArgSS.Text = ArgSS.Text + " 간암형: ";
            for (int i = 1; i <= VB.Len(ArgVar); i++)
            {
                if (VB.Mid(ArgVar, i, 1) == "1")
                {
                    switch (i)
                    {
                        case 1: ArgSS.Text = ArgSS.Text + "단발성결절형,"; break;
                        case 2: ArgSS.Text = ArgSS.Text + "다발성결절형,"; break;
                        case 3: ArgSS.Text = ArgSS.Text + "대종괴형,"; break;
                        case 4: ArgSS.Text = ArgSS.Text + "식도 미만형,"; break;
                            //default: rtnVal = ""; break;
                    }
                }
            }

            ArgSS.Text = ArgSS.Text + ComNum.VBLF;
            ArgSS.Text = ArgSS.Text + " 병변위치: ";
            for (int i = 1; i <= VB.Len(ArgByeng); i++)
            {
                if (VB.Mid(ArgByeng, i, 1) == "1")
                {
                    switch (i)
                    {
                        case 1: ArgSS.Text = ArgSS.Text + "Ⅰ,"; break;
                        case 2: ArgSS.Text = ArgSS.Text + "Ⅱ,"; break;
                        case 3: ArgSS.Text = ArgSS.Text + "Ⅲ,"; break;
                        case 4: ArgSS.Text = ArgSS.Text + "Ⅳ,"; break;
                        case 5: ArgSS.Text = ArgSS.Text + "Ⅴ,"; break;
                        case 6: ArgSS.Text = ArgSS.Text + "Ⅵ,"; break;
                        case 7: ArgSS.Text = ArgSS.Text + "Ⅶ,"; break;
                        case 8: ArgSS.Text = ArgSS.Text + "식도 Ⅷ,"; break;
                            //default: rtnVal = ""; break;
                    }
                }
            }

            ArgSS.Text = ArgSS.Text + ComNum.VBLF;
            ArgSS.Text = ArgSS.Text + " 병변크기: ";
            switch (ArgSize)
            {
                case "1": ArgSS.Text = ArgSS.Text + "2cm 미만"; break;
                case "2": ArgSS.Text = ArgSS.Text + "2~5cm 미만"; break;
                case "3": ArgSS.Text = ArgSS.Text + "5cm 이상"; break;
                    //default: rtnVal = ""; break;
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 예약접수증 출력
        /// Author : 박병규
        /// Create Date : 2018.03.14
        /// <param name="pDbCon"></param>
        /// <param name="ArgRe">사본여부(1.사본)</param>
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgName">환자명</param>
        /// <param name="ArgDept">진료과목코드</param>
        /// <param name="ArgDeptName">진료과목명</param>
        /// <param name="ArgDr">예약의사코드</param>
        /// <param name="ArgRDate">예약일자</param>
        /// <param name="ArgPicture_Sign"></param>
        /// <param name="SPREAD"></param>
        public void ResJupsu_Print(string ArgRe, string ArgPtno, string ArgName, string ArgDept, string ArgDeptName, string ArgDr, string ArgRDate, string ArgActDate, FpSpread o)
        {
            ComFunc CF = new ComBase.ComFunc();
            clsPrint CP = new clsPrint();
            clsSpread CS = new clsSpread();
            clsPmpaFunc CPF = new ComPmpaLibB.clsPmpaFunc();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strPrintName = "";

            strPrintName = CP.getPmpaBarCodePrinter(CARD_PRINTER_NAME); //Default :신용카드(접수창구용 설정이름)

            //프린트이름이 없을시 다른 이름으로 설정(프린트명 : 접수증, 예약증(원무팀 사무실 설정이름))
            if (strPrintName == "")
                strPrintName = CP.getPmpaBarCodePrinter(JUP_PRINTER_NAME);

            if (strPrintName == "")
                strPrintName = CP.getPmpaBarCodePrinter(RES_PRINTER_NAME);


            //접수증 프린터 설정
            if (strPrintName != "")
            {
                if (ArgRe == "1")
                    o.ActiveSheet.Cells[0, 0].Text = "예약증 (사본)";
                else
                    o.ActiveSheet.Cells[0, 0].Text = "예약증 (후불)";

                o.ActiveSheet.Cells[0, 1].Text = ArgPtno + VB.Asc(VB.Left(ArgDept, 1)) + VB.Asc(VB.Right(ArgDept, 1)); //바코드
                o.ActiveSheet.Cells[1, 1].Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Bold);
                o.ActiveSheet.Cells[1, 1].Text = ArgDeptName; //진료과/의사명
                o.ActiveSheet.Cells[2, 1].Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Bold);
                o.ActiveSheet.Cells[2, 1].Text = ArgRDate; //예약일자
                o.ActiveSheet.Cells[3, 1].Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Bold);
                o.ActiveSheet.Cells[3, 1].Text = ArgPtno + " / " + ArgName; //등록번호/성명

                o.ActiveSheet.Cells[4, 1].ColumnSpan = 2;
                o.ActiveSheet.Cells[4, 1].HorizontalAlignment = CellHorizontalAlignment.Left;
                if (ArgActDate != "")
                {
                    o.ActiveSheet.Cells[4, 1].Text = ArgActDate + " " + "(" + clsType.User.JobName + ")"; //수납일자/수납자
                }
                else
                {
                    o.ActiveSheet.Cells[4, 1].Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "(" + clsType.User.JobName + ")"; //수납일자/수납자
                }


                o.ActiveSheet.Cells[5, 1].ColumnSpan = 2;
                o.ActiveSheet.Cells[5, 1].HorizontalAlignment = CellHorizontalAlignment.Left;
                o.ActiveSheet.Cells[5, 1].Text = CPF.Get_Dept_TelNo(clsDB.DbCon, ArgDept, ArgDr).Trim() + "(진료과)";

                string strHeader = "";
                string strFooter = "";
                bool PrePrint = false;

                strHeader = "";
                strFooter = "";

                setMargin = new clsSpread.SpdPrint_Margin(2, 10, 5, 10, 5, 10);
                setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

                CS.setSpdPrint(o, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);

                ComFunc.Delay(200);
            }
        }

        /// <summary>
        /// 영수증 서식
        /// </summary>
        /// <param name="hSP"></param>
        public void Receipt_Report_Print(HIC_JEPSU nHJ, HIC_SUNAP hSP)
        {
            FpSpread ssSpread = null;
            SheetView ssSpread_Sheet = null;

            ComFunc CF = new ComFunc();
            clsOumsadChk COC = new ComPmpaLibB.clsOumsadChk();
            clsDrugPrint clsDP = new clsDrugPrint();
            PrintDocument pd = new PrintDocument();
            PageSettings ps = new PageSettings();
            PrintController pc = new StandardPrintController();
            clsPrint CP = new clsPrint();
            Card CC = new Card();

            string strDept = "";
            string strBi = "82";
            long nX = 0;
            long nY = 0;
            string strPrintName = "";

            clsDP.SetReciptPrint_New(ref ssSpread, ref ssSpread_Sheet);
            clsDP.ClearReciptPrint_New(ssSpread, ssSpread_Sheet);

            nX = 0 + clsPmpaPb.GnPrintXSet;
            nY = 620 + clsPmpaPb.GnPrintYSet;

            //영수증출력
            CP.getPrinter_Chk(RECEIPT_PRINTER_NAME);
            strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);

            if (strPrintName.Trim() == "") { return; }

            if (nHJ.JEPBUN == "5")
            {
                strDept = "TO";
                ssSpread_Sheet.Cells[0, 4].Text = "종합검진(TO)";
            }
            else
            {
                strDept = "HR";
                ssSpread_Sheet.Cells[0, 4].Text = "일반검진(HR)";
            }

            ssSpread_Sheet.Cells[0, 12].Text = nHJ.PTNO + VB.Asc(VB.Left(strDept, 1)) + VB.Asc(VB.Right(strDept, 1)); //바코드

            if (clsPmpaPb.GstrErJobFlag == "OK")
            {
                if (clsPmpaPb.GnNight1 == "2")
                    ssSpread_Sheet.Cells[3, 13].Text = "√";
                else if (clsPmpaPb.GnNight1 == "1")
                    ssSpread_Sheet.Cells[3, 13].Text = VB.Space(11) + "√";
            }

            if (clsPmpaPb.GstrCopyPrint == "OK")
            {
                ssSpread_Sheet.Cells[0, 4].Text += " [사본]";
            }

            //공급받는자보관용
            ssSpread_Sheet.Cells[3, 0].Text = nHJ.PTNO;
            ssSpread_Sheet.Cells[3, 4].Text = nHJ.SNAME;
            //ssSpread_Sheet.Cells[3, 8].Text = DateTime.Now.ToShortDateString();
            ssSpread_Sheet.Cells[3, 8].Text = nHJ.JEPDATE;
            if (strDept == "HR")
            {
                ssSpread_Sheet.Cells[5, 0].Text = "일반건진(HR)";
            }
            else if (strDept == "TO")
            {
                ssSpread_Sheet.Cells[5, 0].Text = "종합검진(TO)";
            }

            ssSpread_Sheet.Cells[5, 11].Text = strDept;
            ssSpread_Sheet.Cells[5, 12].Text = clsType.User.JobName + "(" + clsType.User.IdNumber + ") " + hSP.SEQNO;

            //if (ArgSunap != "예약비")
            //{
            //진찰료 급여본인부담금
            ////if (clsPmpaPb.gnJinAMT5 != 0)
            ////    ssSpread_Sheet.Cells[9, 3].Text = string.Format("{0:#,###}", ((clsPmpaPb.gnJinAMT7 + clsPmpaPb.gnJinAMT5) - clsPmpaPb.gnJinAMT2));
            ////else
            ////    ssSpread_Sheet.Cells[9, 3].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaPb.gnJinAMT2));

            //////진찰료 급여공단부담금
            ////ssSpread_Sheet.Cells[9, 5].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT4);
            //////진찰료 선택진료료
            ////ssSpread_Sheet.Cells[9, 8].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT2);

            //}
            //else if (ArgSunap == "예약비")
            //{
            //    //진찰료 급여본인부담금
            //    ssSpread_Sheet.Cells[31, 3].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaPb.gnJinAMT2));
            //    //진찰료 급여공단부담금
            //    ssSpread_Sheet.Cells[31, 5].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT4);
            //    //진찰료 선택진료료
            //    ssSpread_Sheet.Cells[31, 8].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT2);
            //}

            //진찰료 총액에 선택진료비 포함되어었음
            ////clsPmpaPb.gnJinAMT3 = clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT2;

            //합계 급여본인부담금
            ////if (clsPmpaPb.gnJinAMT5 != 0)
            ////    ssSpread_Sheet.Cells[37, 3].Text = string.Format("{0:#,###}", ((clsPmpaPb.gnJinAMT7 + clsPmpaPb.gnJinAMT5) - clsPmpaPb.gnJinAMT2));
            ////else
            ////    ssSpread_Sheet.Cells[37, 3].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaPb.gnJinAMT2));

            //////합계 급여공단부담금
            ////ssSpread_Sheet.Cells[37, 5].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT4);
            //////합계 선택진료료
            ////ssSpread_Sheet.Cells[37, 8].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT2);

            //<금액산정내용>***************************************************************************************************************
            //진료비총액
            ssSpread_Sheet.Cells[7, 13].Text = string.Format("{0:#,###}", hSP.TOTAMT);
            //ssSpread_Sheet.Cells[7, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT1 + clsPmpaPb.gnJinAMT2));


            //환자부담총액
            ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", hSP.BONINAMT);
            //if (clsPmpaPb.gnJinAMT5 != 0)
            //    ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 + clsPmpaPb.gnJinAMT5));
            //else
            //    ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);

            //납부할금액
            ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", hSP.BONINAMT);
            //ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);

            //납부한금액
            if (clsPmpaType.RSD.Gubun == "1")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//카드
                    //ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - (clsPmpaType.RSD.TotAmt * -1)));//현금
                }
                else
                {
                    //2020-09-07 DB데이터 
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (hSP.SUNAPAMT2));//카드
                    //ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//카드

                    //ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaType.RSD.TotAmt));//현금
                }
            }
            else if (clsPmpaType.RSD.Gubun == "2")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {

                    //ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//현금영수증
                    //ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - (clsPmpaType.RSD.TotAmt * -1)));//현금
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaType.RSD.TotAmt * -1));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[30, 12].Text = "현금승인취소";
                        ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
                else
                {
                    //2020-09-07 DB데이터 
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (hSP.SUNAPAMT - hSP.SUNAPAMT2));//현금
                    //ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaType.RSD.TotAmt));//현금

                    //ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//현금영수증
                    //ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaType.RSD.TotAmt));//현금


                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[30, 12].Text = "현금승인";
                        //ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
            }
            else
            {
                ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (hSP.SUNAPAMT2));//카드
                ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (hSP.SUNAPAMT - hSP.SUNAPAMT2));//현금
            }

            //if (hSP.SUNAPAMT == 0)
            //{
            //    ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", (hSP.SUNAPAMT2)); //합계
            //}
            //else if(hSP.SUNAPAMT2 == 0)
            //{
            //    ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", (hSP.SUNAPAMT)); //합계
            //}
            //else if (hSP.SUNAPAMT != hSP.SUNAPAMT2)
            //{
            //    ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", (hSP.SUNAPAMT+ hSP.SUNAPAMT2)); //합계
            //}
            //else if (hSP.SUNAPAMT == hSP.SUNAPAMT2)
            //{
            //    ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", (hSP.SUNAPAMT2)); //합계
            //}


            ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", (hSP.SUNAPAMT)); //합계
            //ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);//합계
            //<금액산정내용>***************************************************************************************************************

            //감액
            if (clsPmpaPb.gnJinAMT5 > 0)
            {
                ssSpread_Sheet.Cells[13, 11].ColumnSpan = 2;
                ssSpread_Sheet.Cells[13, 11].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssSpread_Sheet.Cells[13, 11].Text = "감액";
                ssSpread_Sheet.Cells[13, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT5);//감액

                if (clsPmpaType.TOM.GbGameK != "00" && clsPmpaType.TOM.GbGameK != "")
                {
                    if (clsPmpaType.TOM.GbGameK == "11")
                    {
                        ssSpread_Sheet.Cells[14, 11].ColumnSpan = 4;
                        ssSpread_Sheet.Cells[14, 11].HorizontalAlignment = CellHorizontalAlignment.Left;

                        ssSpread_Sheet.Cells[14, 11].Text = "감액사유 : 직원본인할인";
                    }
                    else if (clsPmpaType.TOM.GbGameK == "13" || clsPmpaType.TOM.GbGameK == "14" || clsPmpaType.TOM.GbGameK == "22" || clsPmpaType.TOM.GbGameK == "23" || clsPmpaType.TOM.GbGameK == "24" || clsPmpaType.TOM.GbGameK == "27" || clsPmpaType.TOM.GbGameK == "32" || clsPmpaType.TOM.GbGameK == "33" || clsPmpaType.TOM.GbGameK == "34")
                    {
                        ssSpread_Sheet.Cells[14, 11].ColumnSpan = 4;
                        ssSpread_Sheet.Cells[14, 11].HorizontalAlignment = CellHorizontalAlignment.Left;

                        ssSpread_Sheet.Cells[14, 11].Text = "감액사유 : 직원가족할인";
                    }
                }
            }

            //미수
            if (clsPmpaPb.gnJinAMT6 > 0)
            {
                ssSpread_Sheet.Cells[14, 11].ColumnSpan = 2;
                ssSpread_Sheet.Cells[14, 11].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssSpread_Sheet.Cells[14, 11].Text = "미수금";
                ssSpread_Sheet.Cells[14, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT6);//미수
            }

            ssSpread_Sheet.Cells[38, 7].Text = "";
            ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime;


            //clsPmpaType.RSD.CardSeqNo = 6556955;

            //카드영수증 통합
            if (clsPmpaType.RSD.CardSeqNo > 0)
            {
                Card.GstrCardApprov_Info = CC.Card_Sign_Info_Set(clsDB.DbCon, clsPmpaType.RSD.CardSeqNo, nHJ.PTNO, "CENTER");

                if (Card.GstrCardApprov_Info != "")
                {
                    ssSpread_Sheet.Cells[31, 13].ColumnSpan = 3;
                    ssSpread_Sheet.Cells[31, 13].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[31, 13].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 24); //거래구분
                    ssSpread_Sheet.Cells[32, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[32, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    //ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4) + " " + VB.Pstr(Card.GstrCardApprov_Info, "{}", 2); //카드종류/카드번호
                    ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4);  //카드종류
                    ssSpread_Sheet.Cells[33, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[33, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[33, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 20); //승인금액
                    ssSpread_Sheet.Cells[34, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[34, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[34, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 12); //승인번호
                    ssSpread_Sheet.Cells[35, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[35, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[35, 12].Text = "72117503"; //가맹점번호
                    ssSpread_Sheet.Cells[36, 12].ColumnSpan = 2;
                    ssSpread_Sheet.Cells[36, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[36, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 14); //거래일시
                    ssSpread_Sheet.Cells[36, 15].Text = VB.Pstr(VB.Pstr(Card.GstrCardApprov_Info, "{}", 8), "개월", 1);
                }

                Card.GstrCardApprov_Info = "";
            }


            strPrintName = VB.Replace(strPrintName, "(리디렉션)", "");
            strPrintName = VB.Replace(strPrintName, "(리디렉션 1)", "");
            strPrintName = VB.Replace(strPrintName, "(리디렉션 2)", "");
            clsDP.PrintReciptPrint_New(ssSpread, ssSpread_Sheet, strPrintName);
        }

        /// <summary>
        /// 영수증 서식
        /// </summary>
        /// <param name="hSP"></param>
        public void Receipt_Report_Print(HEA_JEPSU nHJ, HIC_SUNAP hSP)
        {
            FpSpread ssSpread = null;
            SheetView ssSpread_Sheet = null;

            ComFunc CF = new ComFunc();
            clsOumsadChk COC = new ComPmpaLibB.clsOumsadChk();
            clsDrugPrint clsDP = new clsDrugPrint();
            PrintDocument pd = new PrintDocument();
            PageSettings ps = new PageSettings();
            PrintController pc = new StandardPrintController();
            clsPrint CP = new clsPrint();
            Card CC = new Card();

            string strDept = "";
            string strBi = "82";
            long nX = 0;
            long nY = 0;
            string strPrintName = "";

            clsDP.SetReciptPrint_New(ref ssSpread, ref ssSpread_Sheet);
            clsDP.ClearReciptPrint_New(ssSpread, ssSpread_Sheet);

            nX = 0 + clsPmpaPb.GnPrintXSet;
            nY = 620 + clsPmpaPb.GnPrintYSet;

            //영수증출력
            CP.getPrinter_Chk(RECEIPT_PRINTER_NAME);
            strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);

            if (strPrintName.Trim() == "") { return; }

            if (nHJ.JEPBUN == "5")
            {
                strDept = "TO";
                ssSpread_Sheet.Cells[0, 4].Text = "종합검진(TO)";
            }
            else
            {
                strDept = "HR";
                ssSpread_Sheet.Cells[0, 4].Text = "일반검진(HR)";
            }

            ssSpread_Sheet.Cells[0, 12].Text = nHJ.PTNO + VB.Asc(VB.Left(strDept, 1)) + VB.Asc(VB.Right(strDept, 1)); //바코드

            if (clsPmpaPb.GstrErJobFlag == "OK")
            {
                if (clsPmpaPb.GnNight1 == "2")
                    ssSpread_Sheet.Cells[3, 13].Text = "√";
                else if (clsPmpaPb.GnNight1 == "1")
                    ssSpread_Sheet.Cells[3, 13].Text = VB.Space(11) + "√";
            }

            if (clsPmpaPb.GstrCopyPrint == "OK")
            {
                ssSpread_Sheet.Cells[0, 4].Text += " [사본]";
            }

            //공급받는자보관용
            ssSpread_Sheet.Cells[3, 0].Text = nHJ.PTNO;
            ssSpread_Sheet.Cells[3, 4].Text = nHJ.SNAME;
            //ssSpread_Sheet.Cells[3, 8].Text = DateTime.Now.ToShortDateString
            ssSpread_Sheet.Cells[3, 8].Text = nHJ.SDATE;
            if (strDept == "HR")
            {
                ssSpread_Sheet.Cells[5, 0].Text = "일반건진(HR)";
            }
            else if (strDept == "TO")
            {
                ssSpread_Sheet.Cells[5, 0].Text = "종합검진(TO)";
            }

            ssSpread_Sheet.Cells[5, 11].Text = strDept;
            ssSpread_Sheet.Cells[5, 12].Text = clsType.User.JobName + "(" + clsType.User.IdNumber + ") " + hSP.SEQNO;

            //if (ArgSunap != "예약비")
            //{
            //진찰료 급여본인부담금
            if (clsPmpaPb.gnJinAMT5 != 0)
                ssSpread_Sheet.Cells[9, 3].Text = string.Format("{0:#,###}", ((clsPmpaPb.gnJinAMT7 + clsPmpaPb.gnJinAMT5) - clsPmpaPb.gnJinAMT2));
            else
                ssSpread_Sheet.Cells[9, 3].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaPb.gnJinAMT2));

            //진찰료 급여공단부담금
            ssSpread_Sheet.Cells[9, 5].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT4);
            //진찰료 선택진료료
            ssSpread_Sheet.Cells[9, 8].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT2);

            //}
            //else if (ArgSunap == "예약비")
            //{
            //    //진찰료 급여본인부담금
            //    ssSpread_Sheet.Cells[31, 3].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaPb.gnJinAMT2));
            //    //진찰료 급여공단부담금
            //    ssSpread_Sheet.Cells[31, 5].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT4);
            //    //진찰료 선택진료료
            //    ssSpread_Sheet.Cells[31, 8].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT2);
            //}

            //진찰료 총액에 선택진료비 포함되어었음
            clsPmpaPb.gnJinAMT3 = clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT2;

            //합계 급여본인부담금
            if (clsPmpaPb.gnJinAMT5 != 0)
                ssSpread_Sheet.Cells[37, 3].Text = string.Format("{0:#,###}", ((clsPmpaPb.gnJinAMT7 + clsPmpaPb.gnJinAMT5) - clsPmpaPb.gnJinAMT2));
            else
                ssSpread_Sheet.Cells[37, 3].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaPb.gnJinAMT2));

            //합계 급여공단부담금
            ssSpread_Sheet.Cells[37, 5].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT4);
            //합계 선택진료료
            ssSpread_Sheet.Cells[37, 8].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT2);

            //<금액산정내용>***************************************************************************************************************
            //진료비총액
            ssSpread_Sheet.Cells[7, 13].Text = string.Format("{0:#,###}", hSP.TOTAMT);
            //ssSpread_Sheet.Cells[7, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT1 + clsPmpaPb.gnJinAMT2));


            //환자부담총액
            ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", hSP.BONINAMT);
            //if (clsPmpaPb.gnJinAMT5 != 0)
            //    ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 + clsPmpaPb.gnJinAMT5));
            //else
            //    ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);


            //2020-11-06
            if (strDept == "TO")
            {
                ssSpread_Sheet.Cells[13, 11].Text = "할인금액";
                ssSpread_Sheet.Cells[13, 13].Text = string.Format("{0:#,###}", hSP.HALINAMT);
            }

            //납부할금액
            ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", hSP.BONINAMT);
            //ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);

            //납부한금액
            if (clsPmpaType.RSD.Gubun == "1")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//카드
                    //ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - (clsPmpaType.RSD.TotAmt * -1)));//현금
                }
                else
                {
                    //2020-09-07 DB데이터 
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (hSP.SUNAPAMT2));//카드
                    //ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//카드

                    //ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaType.RSD.TotAmt));//현금
                }
            }
            else if (clsPmpaType.RSD.Gubun == "2")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {

                    //ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//현금영수증
                    //ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - (clsPmpaType.RSD.TotAmt * -1)));//현금
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaType.RSD.TotAmt * -1));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[30, 12].Text = "현금승인취소";
                        ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
                else
                {
                    //2020-09-07 DB데이터 
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (hSP.BONINAMT - hSP.SUNAPAMT2));//현금
                    //ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaType.RSD.TotAmt));//현금

                    //ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//현금영수증
                    //ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaType.RSD.TotAmt));//현금


                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[30, 12].Text = "현금승인";
                        //ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
            }
            else
            {
                ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (hSP.SUNAPAMT2));//카드
                ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (hSP.BONINAMT - hSP.SUNAPAMT2));//현금
            }

            //if (hSP.SUNAPAMT == 0)
            //{
            //    ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", (hSP.SUNAPAMT2)); //합계
            //}
            //else if(hSP.SUNAPAMT2 == 0)
            //{
            //    ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", (hSP.SUNAPAMT)); //합계
            //}
            //else if (hSP.SUNAPAMT != hSP.SUNAPAMT2)
            //{
            //    ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", (hSP.SUNAPAMT+ hSP.SUNAPAMT2)); //합계
            //}
            //else if (hSP.SUNAPAMT == hSP.SUNAPAMT2)
            //{
            //    ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", (hSP.SUNAPAMT2)); //합계
            //}


            ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", (hSP.BONINAMT)); //합계
            //ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);//합계
            //<금액산정내용>***************************************************************************************************************

            //감액
            if (clsPmpaPb.gnJinAMT5 > 0)
            {
                ssSpread_Sheet.Cells[13, 11].ColumnSpan = 2;
                ssSpread_Sheet.Cells[13, 11].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssSpread_Sheet.Cells[13, 11].Text = "감액";
                ssSpread_Sheet.Cells[13, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT5);//감액

                if (clsPmpaType.TOM.GbGameK != "00" && clsPmpaType.TOM.GbGameK != "")
                {
                    if (clsPmpaType.TOM.GbGameK == "11")
                    {
                        ssSpread_Sheet.Cells[14, 11].ColumnSpan = 4;
                        ssSpread_Sheet.Cells[14, 11].HorizontalAlignment = CellHorizontalAlignment.Left;

                        ssSpread_Sheet.Cells[14, 11].Text = "감액사유 : 직원본인할인";
                    }
                    else if (clsPmpaType.TOM.GbGameK == "13" || clsPmpaType.TOM.GbGameK == "14" || clsPmpaType.TOM.GbGameK == "22" || clsPmpaType.TOM.GbGameK == "23" || clsPmpaType.TOM.GbGameK == "24" || clsPmpaType.TOM.GbGameK == "27" || clsPmpaType.TOM.GbGameK == "32" || clsPmpaType.TOM.GbGameK == "33" || clsPmpaType.TOM.GbGameK == "34")
                    {
                        ssSpread_Sheet.Cells[14, 11].ColumnSpan = 4;
                        ssSpread_Sheet.Cells[14, 11].HorizontalAlignment = CellHorizontalAlignment.Left;

                        ssSpread_Sheet.Cells[14, 11].Text = "감액사유 : 직원가족할인";
                    }
                }
            }

            //미수
            if (clsPmpaPb.gnJinAMT6 > 0)
            {
                ssSpread_Sheet.Cells[14, 11].ColumnSpan = 2;
                ssSpread_Sheet.Cells[14, 11].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssSpread_Sheet.Cells[14, 11].Text = "미수금";
                ssSpread_Sheet.Cells[14, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT6);//미수
            }

            ssSpread_Sheet.Cells[38, 7].Text = "";
            ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime;


            //clsPmpaType.RSD.CardSeqNo = 6556955;

            //카드영수증 통합
            if (clsPmpaType.RSD.CardSeqNo > 0)
            {
                Card.GstrCardApprov_Info = CC.Card_Sign_Info_Set(clsDB.DbCon, clsPmpaType.RSD.CardSeqNo, nHJ.PTNO, "CENTER");

                if (Card.GstrCardApprov_Info != "")
                {
                    ssSpread_Sheet.Cells[31, 13].ColumnSpan = 3;
                    ssSpread_Sheet.Cells[31, 13].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[31, 13].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 24); //거래구분
                    ssSpread_Sheet.Cells[32, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[32, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    //ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4) + " " + VB.Pstr(Card.GstrCardApprov_Info, "{}", 2); //카드종류/카드번호
                    ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4);  //카드종류
                    ssSpread_Sheet.Cells[33, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[33, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[33, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 20); //승인금액
                    ssSpread_Sheet.Cells[34, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[34, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[34, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 12); //승인번호
                    ssSpread_Sheet.Cells[35, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[35, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[35, 12].Text = "72117503"; //가맹점번호
                    ssSpread_Sheet.Cells[36, 12].ColumnSpan = 2;
                    ssSpread_Sheet.Cells[36, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[36, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 14); //거래일시
                    ssSpread_Sheet.Cells[36, 15].Text = VB.Pstr(VB.Pstr(Card.GstrCardApprov_Info, "{}", 8), "개월", 1);
                }

                Card.GstrCardApprov_Info = "";
            }
            strPrintName = VB.Replace(strPrintName, "(리디렉션)", "");
            strPrintName = VB.Replace(strPrintName, "(리디렉션 1)", "");
            strPrintName = VB.Replace(strPrintName, "(리디렉션 2)", "");
            clsDP.PrintReciptPrint_New(ssSpread, ssSpread_Sheet, strPrintName);
        }

        /// <summary>
        /// Description : 
        /// Author : 김경동
        /// Create Date : 2020.10.14

        /// <param name="argWRTNO">접수번호</param>

        public void EMR_FileToDB(long argWRTNO, string argGubun, string argSeqno, string argFndrno, string argTableName)
        {
            string strFileName = "";
            string strFileNameFTP = "";
            string strJobDate = "";
            string strPTNO = "";

            strFileName = @"C:\" + argWRTNO + argGubun + argSeqno + ".ss7";

            HIC_JEPSU item = hicJepsuService.GetItembyWrtNo(argWRTNO);
            strPTNO = item.PTNO;

            strJobDate = VB.Replace(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), "-", "");
            strFileNameFTP = strPTNO + "_" + strJobDate + "_" + argGubun + "_" + argWRTNO + ".ss7";

            //디렉토리 만듬
            //Call FTP_CREATE_DIRECTORY_Process("/data/hic_result/" & strJobDate)

            //파일 업로드
            //Call FTP_Send_Process(strFileName, "/data/hic_result/" & strJobDate & "/" & strFileNameFTP)

            HIC_EMR_RESULT item1 = hicEmrResultService.GetItemByWrtnoGubunSeqno(argWRTNO, argGubun, argSeqno);

            if (item1.IsNullOrEmpty())
            {
                //INSERT
                strJobDate = VB.Replace(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), "-", "");

                HIC_EMR_RESULT item3 = new HIC_EMR_RESULT();

                item3.JOBDATE = clsPublic.GstrSysDate;
                item3.TABLE_NAME = argTableName;
                item3.GUBUN = argGubun;
                item3.WRTNO = argWRTNO;
                item3.SEQNO = long.Parse(argSeqno);
                item3.DRNO = long.Parse(argFndrno);
                item3.ENTSABUN = long.Parse(clsType.User.IdNumber);
                item3.FILENAME = strFileNameFTP;

                int result = hicEmrResultService.Insert(item3);

                if (result < 0)
                {
                    MessageBox.Show("자료를 등록중 오류가 발생함!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            else
            {
                //UPDATE
                strJobDate = VB.Replace(item1.JOBDATE, "-", "");
                hicEmrResultService.UpdateFileNameFTPByRID(strFileNameFTP, item1.RID);
            }

        }

        public void HIC_CERT_INSERT(Control argSS, long argWrtno, string argGubun, long argDrno)
        {
            DirectoryInfo Dir = null;
            //Dir = new FileInfo(strFile);


            string strFileName = "";
            string strFileNameFTP = "";
            string strJobDate = "";
            string strPtno = "";
            string strFile = "";

            //파일이름 = 접수번호(8)+오늘날짜+결과지구분
            strJobDate = VB.Replace(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), "-", "");
            strFileName = VB.Format(argWrtno, "00000000") + "-" + strJobDate + "-" + VB.Trim(argGubun) + ".ss7";

            //해당파일 삭제
            strFile = "C:\\temp\\" + strFileName;
            Dir = new DirectoryInfo(strFile);

            if(Dir.Exists == true)
            {
                Dir.Delete();
            }

            //건강검진 결과지를 파일로 저장

            if (Dir.Exists == true)
            {
                ((FpSpread)(argSS)).SaveExcel("", FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat | FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
                HIC_EMR_RESULT_WORK item = hicEmrResultWorkService.GetItemByWrtnoGubun(argWrtno, argGubun);
                if (item.IsNullOrEmpty())
                {
                    //INSERT
                    strJobDate = VB.Replace(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), "-", "");

                    HIC_EMR_RESULT_WORK item1 = new HIC_EMR_RESULT_WORK();

                    item1.JOBDATE = strJobDate;
                    item1.GUBUN = argGubun;
                    item1.WRTNO = argWrtno;
                    item1.DRNO = argDrno;
                    item1.ENTSABUN = long.Parse(clsType.User.IdNumber);
                    item1.ENTDATE = clsPublic.GstrSysDate;
                    item1.FILENAME = strFileNameFTP;

                    int result = hicEmrResultWorkService.Insert(item1);

                    if (result < 0)
                    {
                        MessageBox.Show("자료를 등록중 오류가 발생함!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    //UPDATE
                    hicEmrResultWorkService.UpdateFileNameByRID(strFileNameFTP, item.RID);
                }

                //인쇄한 결과지를 DB에 업데이트
                //HIC_EMR_RESULT_WORK item2 = hicEmrResultWorkService.GetPrtResultByWrtnoGubun(argWrtno, argGubun);


                MTSResult result1 = new MTSResult(true);
                try
                {
                    byte[] bytes = File.ReadAllBytes(@"c:\temp\" + strFileName);

                    int RowAffected = 0;
                    string SQL = "UPDATE KOSMOS_PMPA.HIC_EMR_RESULT_WORK ";
                    SQL += ComNum.VBLF + "SET ";
                    SQL += ComNum.VBLF + "PRTRESULT = :PRTRESULT";
                    SQL += ComNum.VBLF + "WHERE WRTNO = " + argWrtno;
                    SQL += ComNum.VBLF + "  AND GUBUN = '" + argGubun + "' ";
                    SQL += ComNum.VBLF + "  AND JobDate=TRUNC(SYSDATE) ";

                    string SqlErr = clsDB.ExecuteLongRawQueryEx(SQL, bytes, ref RowAffected, clsDB.DbCon);
                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        return;
                    }

                    result1.SetSuccessMessage("성공");
                }
                catch (Exception ex)
                {
                    result1.SetErrMessage(ex.Message);

                }
            }

            if (Dir.Exists == true)
            {
                Dir.Delete();
            }
        }
    }
}
