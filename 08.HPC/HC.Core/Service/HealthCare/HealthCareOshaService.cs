using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Spread;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Repository;
using ComHpcLibB.Service;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Repository;
using HC.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC.Core.Service
{
    /// <summary>
    /// 질병유소견자사후관리 대행
    /// </summary>
    public class HealthCareOshaService
    {
        string FstrLtdCode;
        string FstrFDate;
        string FstrTDate;

        long FnWRTNO;   //1차 접수번호
        long FnWrtno2;  //2차 접수번호

        string FstrSex;

        int FnCNT1_1;   //특수1차 건수
        int FnCNT1_2;   //특수2차 건수
        int FnCNT2_1;   //일반1차 건수
        int FnCNT2_2;   //일반2차 건수

        int FnCNT3_1;   //생애1차만건수 2010
        int FnCNT3_2;   // 생애2차했는건수 2010

        long FnPano;
        string FstrSName = "";
        string FstrUCodes = "";
        string FstrJepDate = "";
        string FstrGjYear = "";
        string FstrGjBangi = "";
        string FstrGjJong = "";

        int FnRow;
        int FnDoctCnt;
        long[] FnPanDrNo = new long[10];
        int FnStartRow;

        string FstrDate1;
        string FstrDate2;

        string Fstr생애구분;

        string FstrGubun;
        string FstrSogen;
        string FstrExams;
        string FstrJochi;
        string FstrChk;
        string FstrNameFlag;
        string Fstr생애Flag;

        int nREAD = 0;
        long nSpcRow = 0;
        int nSpcRowStart = 0;
        int nRow = 0;
        long nResult2CNT = 0;
        long nPanjengDrno = 0;
        int nSpcResCnt = 0; //특검 유질환 물질 건수

        string strSname = "";
        string strSex = "";
        long nAge = 0;
        string strIpsadate = "";
        string strJepDate = "";
        long nGunsok = 0;
        long nGunsokYY = 0;
        long nGunsokMM = 0;
        int nRowCnt2 = 0;
        string strPanName = "";
        string strPanjeng = "";
        string strGbPanBun2 = "";
        string strSogen = "";
        string strResult = "";
        string strOK = "";
        string strGong = "";
        string strUNames = "";
        string strUNames_EDIT = "";
        string strOldData = "";
        string strNewData = "";
        string strMCode = "";
        string strResult2 = "";
        List<string> strTemp = new List<string>();
        string strTemp2 = "";

        string strGongjeng;

        string[] strPanjengC = new string[5];
        string[] strPanjengD1 = new string[3];
        string[] strPanjengD2 = new string[3];
        string[] strPanjengU = new string[4];
        string strFlag = ""; //생애한줄 작업
        string strD2Flag = "";  //D2 대상자


        HicCodeService hicCodeService = null;
        HicResultExCodeService hicResultExCodeService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicJepsuService hicJepsuService = null;
        HicSpcPanjengService hicSpcPanjengService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicSpcSahuService hicSpcSahuService = null;
        HicJepsuGundateService hicJepsuGundateService = null;
        HicResultService hicResultService = null;
        HicJepsuResSpecialService hicJepsuResSpecialService = null;
        HicJepsuResSpecialLtdService hicJepsuResSpecialLtdService = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        ComFunc cf = new ComFunc();
        clsHcFunc hf = new clsHcFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType ht = new clsHcType();

        public HealthCareOshaService()
        {
            chb = new clsHcBill();
            hicCodeService = new HicCodeService();
            hicResultExCodeService = new HicResultExCodeService();
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuService = new HicJepsuService();
            hicSpcPanjengService = new HicSpcPanjengService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicSpcSahuService = new HicSpcSahuService();
            hicJepsuGundateService = new HicJepsuGundateService();
            hicResultService = new HicResultService();
            hicJepsuResSpecialService = new HicJepsuResSpecialService();
            hicJepsuResSpecialLtdService = new HicJepsuResSpecialLtdService();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
        }
        public HealthCareResultModel GetFirstExaminationQuestionnaireByOsha(long wrtNo)
        {
         
            chb.READ_HIC_RES_BOHUM1(wrtNo);

            fn_Screen_First_Result(wrtNo);
        }

        void fn_Screen_First_Result(long nWrtNo)
        {
            //int nRow = 0;
            //string strSogen = "";
            //string strPanName = "";
            //string strGbPanBun2 = "";
            //string strResult = "";
            //string strOK = "";
            ////string strPanjeng = "";
            //string[] strPanjengC = new string[5];
            //string[] strPanjengD1 = new string[3];
            //string[] strPanjengD2 = new string[3];
            //string[] strPanjengU = new string[4];
            //string strFlag = ""; //생애한줄 작업
            //string strD2Flag = "";  //D2 대상자

            for (int i = 0; i < 5; i++)
            {
                strPanjengC[i] = "";
            }

            for (int i = 0; i < 3; i++)
            {
                strPanjengD1[i] = "";
                strPanjengD2[i] = "";
            }

            for (int i = 0; i < 4; i++)
            {
                strPanjengU[i] = "";
            }

            chb.READ_HIC_RES_BOHUM1(nWrtNo);
            if (clsHcType.B1.ROWID.IsNullOrEmpty() || clsHcType.B1.PanDrNo == 0) return;

            //판정의사 명단 등록
            if (FnDoctCnt == 0)
            {
                FnDoctCnt = 1;
                FnPanDrNo[FnDoctCnt - 1] = clsHcType.B1.PanDrNo;
            }
            else
            {
                strOK = "";
                for (int i = 0; i < FnDoctCnt; i++)
                {
                    if (FnPanDrNo[i] == clsHcType.B1.PanDrNo)
                    {
                        strOK = "OK";
                        break;
                    }
                }
                if (strOK != "OK")
                {
                    FnDoctCnt += 1;
                    FnPanDrNo[FnDoctCnt - 1] = clsHcType.B1.PanDrNo;
                }
            }

            strOK = "";
            strD2Flag = "";

            //정상인경우 표시함
            if (clsHcType.B1.Panjeng == "1" || clsHcType.B1.Panjeng == "2")
            {
                strOK = "OK";
            }

            //질환의심(R)이 있으면 표시함
            for (int i = 0; i < 12; i++)
            {
                if (clsHcType.B1.PanjengR[i] == "1")
                {
                    strOK = "OK";
                    break;
                }
            }

            //직업병(D1,D2)이 있으면 표시함
            if (!clsHcType.B1.PANJENGD11.IsNullOrEmpty()) { strOK = "OK"; strPanjengD1[0] = clsHcType.B1.PANJENGD11; }
            if (!clsHcType.B1.PANJENGD12.IsNullOrEmpty()) { strOK = "OK"; strPanjengD1[1] = clsHcType.B1.PANJENGD12; }
            if (!clsHcType.B1.PANJENGD13.IsNullOrEmpty()) { strOK = "OK"; strPanjengD1[2] = clsHcType.B1.PANJENGD13; }
            if (!clsHcType.B1.PANJENGD21.IsNullOrEmpty()) { strOK = "OK"; strPanjengD2[0] = clsHcType.B1.PANJENGD21; strD2Flag = "OK"; }
            if (!clsHcType.B1.PANJENGD22.IsNullOrEmpty()) { strOK = "OK"; strPanjengD2[1] = clsHcType.B1.PANJENGD22; strD2Flag = "OK"; }
            if (!clsHcType.B1.PANJENGD23.IsNullOrEmpty()) { strOK = "OK"; strPanjengD2[2] = clsHcType.B1.PANJENGD23; strD2Flag = "OK"; }

            //유질환(D)이 있으면 표시함
            if (clsHcType.B1.PANJENGU1 == "1") { strOK = "OK"; strPanjengU[0] = clsHcType.B1.PANJENGU1; } //고혈압
            if (clsHcType.B1.PANJENGU2 == "1") { strOK = "OK"; strPanjengU[1] = clsHcType.B1.PANJENGU2; } //당뇨
            if (clsHcType.B1.PANJENGU3 == "1") { strOK = "OK"; strPanjengU[2] = clsHcType.B1.PANJENGU3; } //이상지질
            if (clsHcType.B1.PANJENGU4 == "1") { strOK = "OK"; strPanjengU[3] = clsHcType.B1.PANJENGU4; } //폐결핵

            //2010 생애일경우 2차 안했으면 strok ="OK" 달아줌
            if (Fstr생애구분 == "생애" && clsHcType.B2.ROWID.IsNullOrEmpty() && strOK == "")
            {
                strOK = "OK";
            }

            if (strOK != "OK")
            {
                return;
            }

            //인적사항을 읽음
            //HIC_JEPSU_PATIENT list = hicJepsuPatientService.GetItembyWrtNo(FnWRTNO);

            //if (!list.IsNullOrEmpty())
            //{
            //    strSname = list.SNAME.Trim();
            //    if (list.SEX == "M")
            //    {
            //        strSex = "남";
            //    }
            //    else
            //    {
            //        strSex = "여";
            //    }
            //    strGongjeng = list.BUSENAME;
            //    strIpsadate = list.IPSADATE;
            //    strJepDate = list.JEPDATE;
            //    nGunsokYY = long.Parse(VB.Left(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 3));
            //    nGunsokMM = long.Parse(VB.Mid(clsVbfunc.GunsokYearMonthDayGesan(strIpsadate, strJepDate), 4, 2));
            //    nAge = list.AGE;
            //}

            //2010 생애부분추가
            //생애1차만 했을경우
            strFlag = "";
            if (!clsHcType.B1.ROWID.IsNullOrEmpty() && clsHcType.B2.ROWID.IsNullOrEmpty() && Fstr생애구분 == "생애")
            {
                strPanName = "2차생애미수검";
                strResult = "2차생애상담필요";
                strFlag = "OK";
                fn_Screen_First_Sub(strPanName, strResult);
                strFlag = "";
            }

            //정상소견
            if (clsHcType.B1.Panjeng == "1" || clsHcType.B1.Panjeng == "2")
            {
                strPanName = "정상";
                strResult = "";
                strPanjeng = "1";
                fn_Screen_First_Sub(strPanName, strResult);
            }

            //2011-11-01 건진센터 한흥렬계장 요청사항(의뢰서)
            //D2 일반질병 판정이 있을경우 동일한 질환으로 질환의심R1 판정이 있어도 D2 질환만 표시함
            //질환의심(R1,R2)
            for (int i = 0; i <= 11; i++)
            {
                if (clsHcType.B1.PanjengR[i] == "1")
                {
                    if (!strD2Flag.IsNullOrEmpty())
                    {
                        if (fn_Read_GenPanjengD2(i + 1) == "OK")
                        {
                            clsHcType.B1.PanjengR[i] = "";
                        }
                    }

                    if (clsHcType.B1.PanjengR[i] == "1")
                    {
                        switch (i)
                        {
                            case 0:
                                strPanName = "폐결핵의심";
                                strPanjeng = "4";
                                break;
                            case 1:
                                strPanName = "기타흉부질환의심";
                                strPanjeng = "4";
                                break;
                            case 2:
                                strPanName = "고혈압";
                                strPanjeng = "5";
                                break;
                            case 3:
                                strPanName = "이상지질혈증의심";
                                strPanjeng = "4";
                                break;
                            case 4:
                                strPanName = "간장질환의심";
                                strPanjeng = "4";
                                break;
                            case 5:
                                strPanName = "당뇨병";
                                strPanjeng = "5";
                                break;
                            case 6:
                                strPanName = "신장질환의심";
                                strPanjeng = "4";
                                break;
                            case 7:
                                strPanName = "빈혈증의심";
                                strPanjeng = "4";
                                break;
                            case 8:
                                strPanName = "골다공증의심";
                                strPanjeng = "4";
                                break;
                            case 9:
                                strPanName = "기타질환의심";
                                strPanjeng = "4";
                                break;
                            case 10:
                                strPanName = "비만";
                                strPanjeng = "4";
                                break;
                            case 11:
                                strPanName = "난청";
                                strPanjeng = "4";
                                break;
                            default:
                                break;
                        }
                        strGbPanBun2 = string.Format("{0:00}", i + 1);
                        strResult += Read_Exam_Result("일반", strGbPanBun2, "");
                        fn_Screen_First_Sub(strPanName, strResult);
                    }
                }
            }

            //직업병(D1)
            for (int i = 0; i < 3; i++)
            {
                if (!strPanjengD1[i].IsNullOrEmpty())
                {
                    strPanName = fn_Read_GenPanjengD1D2("31", strPanjengD1[i]);
                    strResult = "";
                    strResult += Read_Exam_Result("D1D2", "31", strPanjengD1[i]);
                    if (!strPanName.IsNullOrEmpty())
                    {
                        strPanjeng = "6";
                    }
                    fn_Screen_First_Sub(strPanName, strResult);
                }
            }

            //일반질환(D2)
            strResult = "";
            for (int i = 0; i < 3; i++)
            {
                if (!strPanjengD2[i].IsNullOrEmpty())
                {
                    strPanName = fn_Read_GenPanjengD1D2("33", strPanjengD2[i]);
                    strResult = "";
                    strResult += Read_Exam_Result("D1D2", "33", strPanjengD2[i]);
                    if (!strPanName.IsNullOrEmpty())
                    {
                        strPanjeng = "7";
                    }
                    fn_Screen_First_Sub(strPanName, strResult);
                }
            }

            //유질환(D)
            for (int i = 0; i < 4; i++)
            {
                if (strPanjengU[i] == "1")
                {
                    switch (i)
                    {
                        case 0:
                            strPanName = "고혈압";
                            strPanjeng = "8";
                            strGbPanBun2 = "03";
                            break;
                        case 1:
                            strPanName = "유질환(당뇨병)";
                            strPanjeng = "8";
                            strGbPanBun2 = "06";
                            break;
                        case 2:
                            strPanName = "유질환(이상지질혈증)";
                            strPanjeng = "8";
                            strGbPanBun2 = "04";
                            break;
                        case 3:
                            strPanName = "유질환(폐결핵)";
                            strPanjeng = "8";
                            strGbPanBun2 = "01";
                            break;
                        default:
                            break;
                    }
                    strResult = "";
                    strResult += Read_Exam_Result("일반", strGbPanBun2, "");
                    fn_Screen_First_Sub(strPanName, strResult);
                }
            }
        }

        void fn_Screen_First_Sub(string argPanName, string argResult)
        {
            long nSogenCnt = 0;
            long nResultCNT = 0;
            long jj = 0;

            FstrChk = "Y";
            FnRow += 1;
            if (FnRow > SS1.ActiveSheet.RowCount)
            {
                SS1.ActiveSheet.RowCount = FnRow;
            }

            if (!strSname.IsNullOrEmpty())
            {
                SS1.ActiveSheet.Cells[FnRow - 1, 0].Text = strGongjeng.IsNullOrEmpty() ? "기타" : strGongjeng;
                SS1.ActiveSheet.Cells[FnRow - 1, 1].Text = strSname;
                SS1.ActiveSheet.Cells[FnRow - 1, 2].Text = strSex;
                SS1.ActiveSheet.Cells[FnRow - 1, 3].Text = nAge.ToString();
                SS1.ActiveSheet.Cells[FnRow - 1, 4].Text = nGunsokYY + "년" + nGunsokMM + "개월";
                SS1.ActiveSheet.Cells[FnRow - 1, 5].Text = "▶일반건진";
                FstrNameFlag = "OK";

                strSogen = "";
                if (Fstr생애구분 == "일반")
                {
                    if (strPanjeng == "1" || strPanjeng == "2")
                    {
                        strSogen = "";
                        strSogen = "필요없음";
                    }
                    else
                    {
                        strSogen += clsHcType.B1.Sogen;
                    }
                }

                SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = strSogen;
                SS1_Sheet1.Rows.Get(FnRow - 1).Border = new LineBorder(Color.Black, 1, false, true, false, false);
            }

            if (!clsHcType.B1.ROWID.IsNullOrEmpty() && clsHcType.B2.ROWID.IsNullOrEmpty() && Fstr생애구분 == "생애" && strFlag == "OK")
            {
                strSogen = "";
                strPanjeng = "X";
                strSogen = "★2차생애상담필요합니다. ";
            }

            switch (strPanjeng)
            {
                case "1":
                case "2":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "A";
                    break;
                case "3":
                case "4":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "C";
                    break;
                case "5":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "확진검사대상";
                    break;
                case "6":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "D1";
                    break;
                case "7":
                case "8":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "D2";
                    break;
                case "X":
                    SS1.ActiveSheet.Cells[FnRow - 1, 7].Text = "생애2차상담";
                    break;
                default:
                    break;
            }

            //[기타질환의심]은 상세내용을 표시함(소장님)
            if (strPanName == "기타질환의심" && !clsHcType.B1.PanjengR_Etc.IsNullOrEmpty())
            {
                strPanName += ":" + clsHcType.B1.PanjengR_Etc.Trim();
            }

            if (strResult == "")
            {
                strResult = strPanName;
            }
            else
            {
                strResult = strPanName + "/" + strResult;
            }

            SS1.ActiveSheet.Cells[FnRow - 1, 8].Text = strResult;
            SS1.ActiveSheet.Cells[FnRow - 1, 9].Text = strSogen;

            SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "가";
            if (string.Compare(strPanjeng, "2") > 0)
            {
                SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "나";
            }
            if (strPanjeng == "X")
            {
                SS1.ActiveSheet.Cells[FnRow - 1, 10].Text = "";
            }

            strSname = "";
            nSogenCnt = 0;
            strSogen = "";
            strResult = "";
            strPanName = "";
            strPanjeng = "";
        }

        /// <summary>
        /// READ_일반판정D2()
        /// </summary>
        /// <param name="argGbn"></param>
        /// <returns></returns>
        string fn_Read_GenPanjengD2(int argGbn)
        {
            string strDPan = "";
            string rtnVal = "NO";

            strDPan = clsHcType.B1.PANJENGD21 + "@" + clsHcType.B1.PANJENGD22 + "@" + clsHcType.B1.PANJENGD23;

            switch (argGbn)
            {
                case 1:
                case 2:
                    rtnVal = VB.L(strDPan, "J") > 1 ? "OK" : "NO";
                    break;
                case 4:
                    rtnVal = VB.L(strDPan, "E") > 1 ? "OK" : "NO";
                    break;
                case 5:
                    rtnVal = VB.L(strDPan, "K") > 1 ? "OK" : "NO";
                    break;
                case 8:
                    rtnVal = VB.L(strDPan, "D") > 1 ? "OK" : "NO";
                    break;
                default:
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argJob : 일반,특수,D1D2"></param>
        /// <param name="argGbn"></param>
        /// <param name="argCode"></param>
        /// <returns></returns>
        string Read_Exam_Result(string argJob, string argGbn, string argCode)
        {
            string rtnVal = "";

            string strRETURN = "";
            string strRet1 = "";
            string strRet2 = "";
            int nREAD = 0;
            int nFVC = 0;
            int nFvcMeas = 0;
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strExName = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strExCodes = "";
            //string strNewExCode = "";
            List<string> strNewExCode = new List<string>();
            string strJepDate = "";
            string strPtNo = "";

            if (argJob == "D1D2")
            {
                HIC_CODE list = hicCodeService.GetGCode2byGubunCode(argGbn, argCode);
                if (list != null)
                {
                    strExCodes = list.GCODE2;
                }
                else
                {
                    return rtnVal;
                }

                if (strExCodes == "")
                {
                    return rtnVal;
                }

                for (int i = 0; i < VB.L(strExCodes, ","); i++)
                {
                    if (VB.L(strExCodes, ",") == 1)
                    {
                        strNewExCode.Add(strExCodes);
                    }
                    else
                    {
                        strNewExCode.Add(VB.Pstr(strExCodes, ",", i).Trim());
                    }
                }
            }

            //1차검사 결과를 READ
            List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoSogenCode(FnWRTNO, argGbn, strNewExCode, argJob);

            nREAD = list2.Count;
            strRet1 = "";
            strRet2 = "";
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list2[i].EXCODE;                 //검사코드
                strResult = list2[i].RESULT;                 //검사실 결과값
                strResCode = list2[i].RESCODE;               //결과값 코드
                strResultType = list2[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list2[i].GBCODEUSE;           //결과값코드 사용여부
                strExName = list2[i].YNAME;
                if (strExName == "") strExName = list2[i].HNAME;
                if (VB.Left(strExName, 3) == "HDL") strExName = "HDL";
                if (VB.Left(strExName, 3) == "LDL") strExName = "LDL";

                if (strGbCodeUse == "Y")
                {
                    if (strResult != "")
                    {
                        strResult = hb.READ_ResultName(strResCode, strResult);
                    }
                }
                if (VB.InStr(strExName, "LDL") > 0 && strResult == "1")
                {
                    strResult = "";
                }

                //혈압,당뇨 2차 표시
                switch (strExCode)
                {
                    case "A108":
                    case "A122":
                        strExName = "1차:" + strExName;
                        break;
                    default:
                        break;
                }

                if (strResult == ".") strResult = "";
                if (strResult != "") strRet1 += strExName + ":" + strResult + ",";
            }

            //2차검사 결과를 READ
            if (FnWrtno2 > 0)
            {
                List<HIC_RESULT_EXCODE> list3 = hicResultExCodeService.GetItembyWrtNoSogenCode(FnWrtno2, argGbn, strNewExCode, argJob);

                nREAD = list3.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strExCode = list3[i].EXCODE;                 //검사코드
                    strResult = list3[i].RESULT;                 //검사실 결과값
                    strResCode = list3[i].RESCODE;               //결과값 코드
                    strResultType = list3[i].RESULTTYPE;         //결과값 TYPE
                    strGbCodeUse = list3[i].GBCODEUSE;           //결과값코드 사용여부
                    strExName = list3[i].YNAME;
                    if (strExName == "")
                    {
                        strExName = list3[i].HNAME.Trim();
                    }
                    if (VB.Left(strExName, 3) == "HDL")
                    {
                        strExName = "HDL";
                    }

                    if (VB.Left(strExName, 3) == "LDL")
                    {
                        strExName = "LDL";
                    }

                    if (strGbCodeUse == "Y")
                    {
                        if (strResult != "")
                        {
                            strResult = hb.READ_ResultName(strResCode, strResult);
                        }
                    }

                    if (VB.InStr(strExName, "LDL") > 0 && strResult == "1")
                    {
                        strResult = "";
                    }
                    //혈압,당뇨 2차 표시
                    switch (strExCode)
                    {
                        case "A148":
                        case "A231":
                        case "ZE22":
                            strExName = "2차:" + strExName;
                            break;
                        default:
                            break;
                    }

                    if (strResult != "")
                    {
                        strRETURN += strExName + ":" + strResult + ",";
                    }
                }
            }

            if (VB.InStr(strRet1, "청력") > 0) strRet1 = "1차:" + strRet1;
            if (VB.InStr(strRet2, "청력") > 0) strRet2 = "2차:" + strRet2;
            strRETURN = strRet1 + strRet2;

            //PFT 검사결과 표시
            nFVC = 0;
            nFvcMeas = 0;

            if (argJob == "특수" && VB.Left(argGbn, 1) == "J")
            {
                List<COMHPC> list4 = comHpcLibBService.GetHicResPftbyWrtNo(FnWRTNO, FnWrtno2);

                nREAD = list4.Count;
                if (nREAD > 0)
                {
                    for (int i = 0; i < nREAD; i++)
                    {
                        strRETURN += "[" + i + 1 + "] FVC(%):" + list4[i].FVC_PRED + ",";
                        strRETURN += "FVC1.0(%):" + list4[i].FEV10_PRED + ",";
                        strRETURN += "FEV1/FVC(%):" + list4[i].FEV1_FVC_MEAS + " ";
                    }
                }

                //종검의 PFT 결과를 읽음
                if (nREAD == 0)
                {
                    //등록번호 및 접수일자를 읽음
                    HIC_JEPSU list5 = hicJepsuService.GetItembyWrtNo(FnWRTNO);

                    strJepDate = list5.JEPDATE.ToString();
                    strPtNo = list5.PTNO;
                    if (strPtNo != "" && strJepDate != "")
                    {
                        List<COMHPC> list6 = comHpcLibBService.GetHeaResPftbyPtNoExDate(strPtNo, strJepDate);

                        nREAD = list6.Count;
                        if (nREAD > 0)
                        {
                            for (int i = 0; i < nREAD; i++)
                            {
                                strRETURN += "[" + i + 1 + "] FVC(%):" + list6[i].FVC_PRED + ",";
                                strRETURN += "FVC1.0(%):" + list6[i].FEV10_PRED + ",";
                                strRETURN += "FEV1/FVC(%):" + list6[i].FEV1_FVC_MEAS + " ";
                            }
                        }
                    }
                }
            }

            rtnVal = strRETURN;

            return rtnVal;
        }

        /// <summary>
        /// READ_일반판정D1D2
        /// </summary>
        /// <param name="argGbn"></param>
        /// <param name="argCode"></param>
        /// <returns></returns>
        string fn_Read_GenPanjengD1D2(string argGbn, string argCode)
        {
            string rtnVal = "";

            HIC_CODE list = hicCodeService.GetItembyGubunCode2(argGbn, argCode);

            if (!list.IsNullOrEmpty())
            {
                if (list.NAME.Trim() == list.NEWNAME.Trim())
                {
                    rtnVal = list.NEWNAME.Trim();
                }
                else
                {
                    rtnVal = list.NEWNAME.Trim() + "(" + list.NAME.Trim() + ")";
                }
            }

            return rtnVal;
        }
    }
}
