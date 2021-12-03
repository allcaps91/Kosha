using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public class clsHcBill
    {
        //전역선언 부분
        HicResBohum1Service hicResBohum1Service = null;
        HicResSpecialService hicResSpecialService = null;
        HicCodeService hicCodeService = null;
        HicJepsuService hicJepsuService = null;
        HicResultService hicResultService = null;
        ExamResultcService examResultcService = null;
        HicMirBohumService hicMirBohumService = null;
        HicMirCancerService hicMirCancerService = null;
        HicMirCancerBoService hicMirCancerBoService = null;
        HicMirDentalService hicMirDentalService = null;
        HicResBohum2Service hicResBohum2Service = null;
        HicCancerNewService hicCancerNewService = null;
        HicResDentalService hicResDentalService = null;
        ExamSpecmstService examSpecmstService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicExjongService hicExjongService = null;
        HicJepsuPatientService hicJepsuPatientService = null;

        clsHcType cHT = null;

        public clsHcBill()
        {
            //전역선언 
            hicResBohum1Service = new HicResBohum1Service();
            hicResSpecialService = new HicResSpecialService();
            hicCodeService = new HicCodeService();
            hicJepsuService = new HicJepsuService();
            hicResultService = new HicResultService();
            examResultcService = new ExamResultcService();
            hicMirBohumService = new HicMirBohumService();
            hicMirCancerService = new HicMirCancerService();
            hicMirCancerBoService = new HicMirCancerBoService();
            hicMirDentalService = new HicMirDentalService();
            hicResBohum2Service = new HicResBohum2Service();
            hicCancerNewService = new HicCancerNewService();
            hicResDentalService = new HicResDentalService();
            examSpecmstService = new ExamSpecmstService();
            comHpcLibBService = new ComHpcLibBService();            
            hicSunapdtlService = new HicSunapdtlService();
            hicExjongService = new HicExjongService();

            cHT = new clsHcType();
        }

        public void Basic_ITems_SET()
        {
            clsHcVariable.GstrBExam1 = "";
        }

        public void READ_HIC_MIR_BOHUM(long ArgMirno)
        {
            clsHcType.TMB.ROWID = "";

            HIC_MIR_BOHUM list = hicMirBohumService.GetItemByMirno(ArgMirno);

            if (!list.IsNullOrEmpty())
            {
                clsHcType.TMB.MIRNO = ArgMirno;
                clsHcType.TMB.Year = list.YEAR;
                clsHcType.TMB.Gubun = list.GUBUN.Trim();
                clsHcType.TMB.Johap = list.JOHAP.Trim();
                clsHcType.TMB.Kiho = list.KIHO.Trim();
                clsHcType.TMB.JepNo = list.JEPNO.Trim();
                clsHcType.TMB.JepQty = list.JEPQTY;
                clsHcType.TMB.TAmt = list.TAMT;
                clsHcType.TMB.JepDate = list.JEPDATE.ToString();
                clsHcType.TMB.ONE_Qty = list.ONE_QTY;
                clsHcType.TMB.ONE_TAmt = list.ONE_TAMT;
                clsHcType.TMB.ONE_Inwon[1] = list.ONE_INWON011;
                clsHcType.TMB.ONE_Inwon[2] = list.ONE_INWON012;
                clsHcType.TMB.ONE_Inwon[3] = list.ONE_INWON021;
                clsHcType.TMB.ONE_Inwon[4] = list.ONE_INWON022;
                clsHcType.TMB.ONE_Inwon[5] = list.ONE_INWON031;
                clsHcType.TMB.ONE_Inwon[6] = list.ONE_INWON032;
                clsHcType.TMB.ONE_Inwon[7] = list.ONE_INWON041;
                clsHcType.TMB.ONE_Inwon[8] = list.ONE_INWON042;
                clsHcType.TMB.ONE_Inwon[9] = list.ONE_INWON051;
                clsHcType.TMB.ONE_Inwon[10] = list.ONE_INWON052;
                clsHcType.TMB.ONE_Inwon[11] = list.ONE_INWON061;
                clsHcType.TMB.ONE_Inwon[12] = list.ONE_INWON062;
                clsHcType.TMB.ONE_Inwon[13] = list.ONE_INWON071;
                clsHcType.TMB.ONE_Inwon[14] = list.ONE_INWON072;
                clsHcType.TMB.ONE_Inwon[15] = list.ONE_INWON081;
                clsHcType.TMB.ONE_Inwon[16] = list.ONE_INWON082;
                clsHcType.TMB.ONE_Inwon[17] = list.ONE_INWON091;
                clsHcType.TMB.ONE_Inwon[18] = list.ONE_INWON092;
                clsHcType.TMB.ONE_Inwon[19] = list.ONE_INWON101;
                clsHcType.TMB.ONE_Inwon[20] = list.ONE_INWON102;
                clsHcType.TMB.ONE_Inwon[21] = list.ONE_INWON111;
                clsHcType.TMB.ONE_Inwon[22] = list.ONE_INWON112;
                clsHcType.TMB.ONE_Inwon[23] = list.ONE_INWON121;
                clsHcType.TMB.ONE_Inwon[24] = list.ONE_INWON122;
                clsHcType.TMB.TWO_Qty = list.TWO_QTY;
                clsHcType.TMB.TWO_TAmt = list.TWO_TAMT;
                clsHcType.TMB.TWO_Inwon[1] = list.TWO_INWON01;
                clsHcType.TMB.TWO_Inwon[2] = list.TWO_INWON02;
                clsHcType.TMB.TWO_Inwon[3] = list.TWO_INWON03;
                clsHcType.TMB.TWO_Inwon[4] = list.TWO_INWON04;
                clsHcType.TMB.TWO_Inwon[5] = list.TWO_INWON05;
                clsHcType.TMB.TWO_Inwon[6] = list.TWO_INWON06;
                clsHcType.TMB.TWO_Inwon[7] = list.TWO_INWON07;
                clsHcType.TMB.TWO_Inwon[8] = list.TWO_INWON08;
                clsHcType.TMB.TWO_Inwon[9] = list.TWO_INWON09;
                clsHcType.TMB.TWO_Inwon[10] = list.TWO_INWON10;
                clsHcType.TMB.TWO_Inwon[11] = list.TWO_INWON11;
                clsHcType.TMB.TWO_Inwon[12] = list.TWO_INWON12;
                clsHcType.TMB.TWO_Inwon[14] = list.TWO_INWON14;
                clsHcType.TMB.TWO_Inwon[15] = list.TWO_INWON15;
                clsHcType.TMB.TWO_Inwon[21] = list.TWO_INWON21;
                clsHcType.TMB.TWO_Inwon[22] = list.TWO_INWON22;
                clsHcType.TMB.TWO_Inwon[23] = list.TWO_INWON23;
                clsHcType.TMB.TWO_Inwon[24] = list.TWO_INWON24;
                clsHcType.TMB.TWO_Inwon[25] = list.TWO_INWON25;
                clsHcType.TMB.TWO_Inwon[26] = list.TWO_INWON26;
                clsHcType.TMB.TWO_Inwon[27] = list.TWO_INWON27;
                clsHcType.TMB.TWO_Inwon[28] = list.TWO_INWON28;
                clsHcType.TMB.TWO_Inwon[29] = list.TWO_INWON29;
                clsHcType.TMB.TWO_Inwon[30] = list.TWO_INWON30;
                clsHcType.TMB.TWO_Inwon[31] = list.TWO_INWON31;
                clsHcType.TMB.TWO_Inwon[32] = list.TWO_INWON32;
                clsHcType.TMB.TWO_Inwon[33] = list.TWO_INWON33;
                clsHcType.TMB.TWO_Inwon[34] = list.TWO_INWON34;
                clsHcType.TMB.TWO_Inwon[35] = list.TWO_INWON35;
                clsHcType.TMB.TWO_Inwon[36] = list.TWO_INWON36;
                clsHcType.TMB.TWO_Inwon[37] = list.TWO_INWON37;
                clsHcType.TMB.TWO_Inwon[38] = list.TWO_INWON38;
                clsHcType.TMB.TWO_Inwon[39] = list.TWO_INWON39;
                clsHcType.TMB.TWO_Inwon[40] = list.TWO_INWON40;
                clsHcType.TMB.TWO_Inwon[41] = list.TWO_INWON41;
                clsHcType.TMB.IpGumDate = list.IPGUMDATE.ToString();
                clsHcType.TMB.IpGumAmt = list.IPGUMAMT;
                clsHcType.TMB.GbJohap = list.GBJOHAP.Trim();
                clsHcType.TMB.FrDate = list.FRDATE.ToString();
                clsHcType.TMB.ToDate = list.TODATE.ToString();
                clsHcType.TMB.BuildDate = list.BUILDDATE.ToString();
                clsHcType.TMB.BuildSabun = list.BUILDSABUN;
                clsHcType.TMB.BuildCnt = list.BUILDCNT;
                clsHcType.TMB.GbErrChk = list.GBERRCHK.Trim();
                clsHcType.TMB.CHASU = list.CHASU.Trim();
                clsHcType.TMB.Life_Gbn = list.LIFE_GBN.Trim();
                clsHcType.TMB.ROWID = list.ROWID;
            }
        }

        public void READ_HIC_MIR_TBOHUM(long ArgMirno)
        {
            clsHcType.TMB.ROWID = "";

            HIC_MIR_BOHUM list = hicMirBohumService.GetItemByMirno(ArgMirno);

            if (!list.IsNullOrEmpty())
            {
                clsHcType.TMB.MIRNO = ArgMirno;
                clsHcType.TMB.Year = list.YEAR.Trim();
                clsHcType.TMB.Gubun = list.GUBUN.Trim();
                clsHcType.TMB.Johap = list.JOHAP.Trim();
                clsHcType.TMB.Kiho = list.KIHO.Trim();
                clsHcType.TMB.JepNo = list.JEPNO.Trim();
                clsHcType.TMB.JepQty = list.JEPQTY;
                clsHcType.TMB.TAmt = list.TAMT;
                clsHcType.TMB.JepDate = list.JEPDATE.ToString();
                clsHcType.TMB.ONE_Qty = list.ONE_QTY;
                clsHcType.TMB.ONE_TAmt = list.ONE_TAMT;
                clsHcType.TMB.ONE_Inwon[1] = list.ONE_INWON011;
                clsHcType.TMB.ONE_Inwon[2] = list.ONE_INWON021;
                clsHcType.TMB.ONE_Inwon[3] = list.ONE_INWON031;
                clsHcType.TMB.ONE_Inwon[4] = list.ONE_INWON041;
                clsHcType.TMB.ONE_Inwon[5] = list.ONE_INWON051;
                clsHcType.TMB.ONE_Inwon[6] = list.ONE_INWON061;
                clsHcType.TMB.ONE_Inwon[7] = list.ONE_INWON071;
                clsHcType.TMB.ONE_Inwon[8] = list.ONE_INWON081;
                clsHcType.TMB.ONE_Inwon[9] = list.ONE_INWON091;
                clsHcType.TMB.ONE_Inwon[10] = list.ONE_INWON101;
                clsHcType.TMB.ONE_Inwon[11] = list.ONE_INWON111;
                clsHcType.TMB.ONE_Inwon[12] = list.ONE_INWON121;
                clsHcType.TMB.TWO_Qty = list.TWO_QTY;
                clsHcType.TMB.TWO_TAmt = list.TWO_TAMT;
                clsHcType.TMB.TWO_Inwon[1] = list.TWO_INWON01;
                clsHcType.TMB.TWO_Inwon[2] = list.TWO_INWON02;
                clsHcType.TMB.TWO_Inwon[3] = list.TWO_INWON03;
                clsHcType.TMB.TWO_Inwon[4] = list.TWO_INWON04;
                clsHcType.TMB.TWO_Inwon[5] = list.TWO_INWON05;
                clsHcType.TMB.TWO_Inwon[6] = list.TWO_INWON06;
                clsHcType.TMB.TWO_Inwon[7] = list.TWO_INWON07;
                clsHcType.TMB.TWO_Inwon[8] = list.TWO_INWON08;
                clsHcType.TMB.TWO_Inwon[9] = list.TWO_INWON09;
                clsHcType.TMB.TWO_Inwon[10] = list.TWO_INWON10;
                clsHcType.TMB.TWO_Inwon[11] = list.TWO_INWON11;
                clsHcType.TMB.TWO_Inwon[12] = list.TWO_INWON12;
                clsHcType.TMB.TWO_Inwon[14] = list.TWO_INWON14;
                clsHcType.TMB.TWO_Inwon[15] = list.TWO_INWON15;
                clsHcType.TMB.TWO_Inwon[21] = list.TWO_INWON21;
                clsHcType.TMB.TWO_Inwon[22] = list.TWO_INWON22;
                clsHcType.TMB.TWO_Inwon[23] = list.TWO_INWON23;
                clsHcType.TMB.TWO_Inwon[24] = list.TWO_INWON24;
                clsHcType.TMB.TWO_Inwon[25] = list.TWO_INWON25;
                clsHcType.TMB.TWO_Inwon[26] = list.TWO_INWON26;
                clsHcType.TMB.TWO_Inwon[27] = list.TWO_INWON27;
                clsHcType.TMB.TWO_Inwon[28] = list.TWO_INWON28;
                clsHcType.TMB.TWO_Inwon[29] = list.TWO_INWON29;
                clsHcType.TMB.TWO_Inwon[30] = list.TWO_INWON30;
                clsHcType.TMB.TWO_Inwon[31] = list.TWO_INWON31;
                clsHcType.TMB.TWO_Inwon[32] = list.TWO_INWON32;
                clsHcType.TMB.TWO_Inwon[33] = list.TWO_INWON33;
                clsHcType.TMB.TWO_Inwon[34] = list.TWO_INWON34;
                clsHcType.TMB.TWO_Inwon[35] = list.TWO_INWON35;
                clsHcType.TMB.TWO_Inwon[36] = list.TWO_INWON36;
                clsHcType.TMB.TWO_Inwon[37] = list.TWO_INWON37;
                clsHcType.TMB.TWO_Inwon[38] = list.TWO_INWON38;
                clsHcType.TMB.TWO_Inwon[39] = list.TWO_INWON39;
                clsHcType.TMB.TWO_Inwon[40] = list.TWO_INWON40;
                clsHcType.TMB.TWO_Inwon[41] = list.TWO_INWON41;
                clsHcType.TMB.IpGumDate = list.IPGUMDATE.ToString();
                clsHcType.TMB.IpGumAmt = list.IPGUMAMT;
                clsHcType.TMB.GbJohap = list.GBJOHAP.Trim();
                clsHcType.TMB.FrDate = list.FRDATE.ToString();
                clsHcType.TMB.ToDate = list.TODATE.ToString();
                clsHcType.TMB.BuildDate = list.BUILDDATE.ToString();
                clsHcType.TMB.BuildSabun = list.BUILDSABUN;
                clsHcType.TMB.BuildCnt = list.BUILDCNT;
                clsHcType.TMB.GbErrChk = list.GBERRCHK.Trim();
                clsHcType.TMB.CHASU = list.CHASU.Trim();
                clsHcType.TMB.Life_Gbn = list.LIFE_GBN.Trim();
                clsHcType.TMB.ROWID = list.ROWID.Trim();
            }
        }

        public void READ_HIC_MIR_CANCER(long ArgMirno)
        {
            clsHcType.TMC.ROWID = "";

            for (int i = 0; i < 50; i++)
            {
                clsHcType.TMC.Inwon[i] = 0;
            }

            HIC_MIR_CANCER list = hicMirCancerService.GetItembyMirno(ArgMirno);

            if (!list.IsNullOrEmpty())
            {
                clsHcType.TMC.MIRNO = ArgMirno;
                clsHcType.TMC.Year = list.YEAR.Trim();
                clsHcType.TMC.Gubun = list.GUBUN.Trim();
                clsHcType.TMC.Johap = list.JOHAP.Trim();
                clsHcType.TMC.Kiho = list.KIHO.Trim();
                clsHcType.TMC.JepNo = list.JEPNO.Trim();
                clsHcType.TMC.JepQty = list.JEPQTY;
                clsHcType.TMC.TAmt = list.TAMT;
                clsHcType.TMC.JepDate = list.JEPDATE.ToString();
                clsHcType.TMC.Inwon[1] = list.INWON01;
                clsHcType.TMC.Inwon[2] = list.INWON02;
                clsHcType.TMC.Inwon[3] = list.INWON03;
                clsHcType.TMC.Inwon[4] = list.INWON04;
                clsHcType.TMC.Inwon[5] = list.INWON05;
                clsHcType.TMC.Inwon[6] = list.INWON06;
                clsHcType.TMC.Inwon[7] = list.INWON07;
                clsHcType.TMC.Inwon[8] = list.INWON08;
                clsHcType.TMC.Inwon[9] = list.INWON09;
                clsHcType.TMC.Inwon[10] = list.INWON10;
                clsHcType.TMC.Inwon[11] = list.INWON11;
                clsHcType.TMC.Inwon[12] = list.INWON12;
                clsHcType.TMC.Inwon[13] = list.INWON13;
                clsHcType.TMC.Inwon[14] = list.INWON14;
                clsHcType.TMC.Inwon[15] = list.INWON15;
                clsHcType.TMC.Inwon[16] = list.INWON16;
                clsHcType.TMC.Inwon[17] = list.INWON17;
                clsHcType.TMC.Inwon[18] = list.INWON18;
                clsHcType.TMC.Inwon[19] = list.INWON19;
                clsHcType.TMC.Inwon[20] = list.INWON20;
                clsHcType.TMC.Inwon[21] = list.INWON21;
                clsHcType.TMC.Inwon[22] = list.INWON22;
                clsHcType.TMC.Inwon[23] = list.INWON23;
                clsHcType.TMC.Inwon[24] = list.INWON24;
                clsHcType.TMC.Inwon[25] = list.INWON25;
                clsHcType.TMC.Inwon[26] = list.INWON26;
                clsHcType.TMC.Inwon[27] = list.INWON27;
                clsHcType.TMC.Inwon[28] = list.INWON28;
                clsHcType.TMC.GbBogun = list.GBBOGUN.Trim();
                clsHcType.TMC.MirGbn = list.MIRGBN.Trim();
                clsHcType.TMC.Inwon[41] = list.SANGDAM1;  //위암상담
                clsHcType.TMC.Inwon[42] = list.SANGDAM2;  //대장암상담
                clsHcType.TMC.Inwon[43] = list.SANGDAM3;  //간암상담
                clsHcType.TMC.Inwon[44] = list.SANGDAM4;  //유방암상담
                clsHcType.TMC.Inwon[45] = list.SANGDAM5;  //자궁경암상담
                clsHcType.TMC.Inwon[46] = list.SANGDAM6;  //폐암상담
                clsHcType.TMC.IpGumDate = list.IPGUMDATE.ToString();
                clsHcType.TMC.IpGumAmt = list.IPGUMAMT;
                clsHcType.TMC.GbJohap = list.GBJOHAP.Trim();
                clsHcType.TMC.FrDate = list.FRDATE.ToString();
                clsHcType.TMC.ToDate = list.TODATE.ToString();
                clsHcType.TMC.BuildDate = list.BUILDDATE.ToString();
                clsHcType.TMC.BuildSabun = list.BUILDSABUN;
                clsHcType.TMC.BuildCnt = list.BUILDCNT;
                clsHcType.TMC.GbErrChk = list.GBERRCHK.Trim();
                clsHcType.TMC.FileName = list.FILENAME.Trim();
                clsHcType.TMC.Life_Gbn = list.LIFE_GBN.Trim();
                clsHcType.TMC.ROWID = list.ROWID.Trim();
            }
        }

        public void READ_HIC_MIR_CANCER_Bo(long ArgMirno)
        {
            clsHcType.TMCB.ROWID = "";
            for (int i = 0; i < 50; i++)
            {
                clsHcType.TMCB.Inwon[i] = 0;
            }

            HIC_MIR_CANCER_BO list = hicMirCancerBoService.GetItembyMirno(ArgMirno);

            clsHcType.TMCB.MIRNO = 0;

            if (!list.IsNullOrEmpty())
            {
                clsHcType.TMCB.MIRNO = ArgMirno;
                clsHcType.TMCB.Year = list.YEAR.Trim();
                clsHcType.TMCB.Gubun = list.GUBUN.Trim();
                clsHcType.TMCB.Johap = list.JOHAP.Trim();
                clsHcType.TMCB.Kiho = list.KIHO.Trim();
                clsHcType.TMCB.JepNo = list.JEPNO.Trim();
                clsHcType.TMCB.JepQty = list.JEPQTY;
                clsHcType.TMCB.TAmt = list.TAMT;
                clsHcType.TMCB.JepDate = list.JEPDATE.ToString();
                clsHcType.TMCB.Inwon[1] = list.INWON01;
                clsHcType.TMCB.Inwon[2] = list.INWON02;
                clsHcType.TMCB.Inwon[3] = list.INWON03;
                clsHcType.TMCB.Inwon[4] = list.INWON04;
                clsHcType.TMCB.Inwon[5] = list.INWON05;
                clsHcType.TMCB.Inwon[6] = list.INWON06;
                clsHcType.TMCB.Inwon[7] = list.INWON07;
                clsHcType.TMCB.Inwon[8] = list.INWON08;
                clsHcType.TMCB.Inwon[9] = list.INWON09;
                clsHcType.TMCB.Inwon[10] = list.INWON10;
                clsHcType.TMCB.Inwon[11] = list.INWON11;
                clsHcType.TMCB.Inwon[12] = list.INWON12;
                clsHcType.TMCB.Inwon[13] = list.INWON13;
                clsHcType.TMCB.Inwon[14] = list.INWON14;
                clsHcType.TMCB.Inwon[15] = list.INWON15;
                clsHcType.TMCB.Inwon[16] = list.INWON16;
                clsHcType.TMCB.Inwon[17] = list.INWON17;
                clsHcType.TMCB.Inwon[18] = list.INWON18;
                clsHcType.TMCB.Inwon[19] = list.INWON19;
                clsHcType.TMCB.Inwon[20] = list.INWON20;
                clsHcType.TMCB.Inwon[21] = list.INWON21;
                clsHcType.TMCB.Inwon[22] = list.INWON22;
                clsHcType.TMCB.Inwon[23] = list.INWON23;
                clsHcType.TMCB.Inwon[24] = list.INWON24;
                clsHcType.TMCB.Inwon[25] = list.INWON25;
                clsHcType.TMCB.Inwon[26] = list.INWON26;
                clsHcType.TMCB.Inwon[27] = list.INWON27;
                clsHcType.TMCB.Inwon[28] = list.INWON28;
                clsHcType.TMCB.MirGbn = list.MIRGBN.Trim();
                clsHcType.TMCB.Inwon[41] = list.SANGDAM1;  //위암상담
                clsHcType.TMCB.Inwon[42] = list.SANGDAM2;  //대장암상담
                clsHcType.TMCB.Inwon[43] = list.SANGDAM3;  //간암상담
                clsHcType.TMCB.Inwon[44] = list.SANGDAM4;  //유방암상담
                clsHcType.TMCB.Inwon[45] = list.SANGDAM5;  //자궁경암상담
                clsHcType.TMCB.Inwon[46] = list.SANGDAM6;  //폐암상담
                clsHcType.TMCB.IpGumDate = list.IPGUMDATE.ToString();
                clsHcType.TMCB.IpGumAmt = list.IPGUMAMT;
                clsHcType.TMCB.GbJohap = list.GBJOHAP.Trim();
                clsHcType.TMCB.FrDate = list.FRDATE.ToString();
                clsHcType.TMCB.ToDate = list.TODATE.ToString();
                clsHcType.TMCB.BuildDate = list.BUILDDATE.ToString();
                clsHcType.TMCB.BuildSabun = list.BUILDSABUN;
                clsHcType.TMCB.BuildCnt = list.BUILDCNT;
                clsHcType.TMCB.GbErrChk = list.GBERRCHK.Trim();
                clsHcType.TMCB.FileName = list.FILENAME.Trim();
                clsHcType.TMCB.Life_Gbn = list.LIFE_GBN.Trim();
                clsHcType.TMCB.ROWID = list.ROWID.Trim();
            }
        }

        public void READ_HIC_MIR_DENTAL(long ArgMirno)
        {
            clsHcType.TMD.ROWID = "";

            HIC_MIR_DENTAL list = hicMirDentalService.GetItembyMirno(ArgMirno);

            if (!list.IsNullOrEmpty())
            {
                clsHcType.TMD.MIRNO = ArgMirno;
                clsHcType.TMD.Year = list.YEAR.Trim();
                clsHcType.TMD.Gubun = list.GUBUN.Trim();
                clsHcType.TMD.Johap = list.JOHAP.Trim();
                clsHcType.TMD.Kiho = list.KIHO.Trim();
                clsHcType.TMD.JepNo = list.JEPNO.Trim();
                clsHcType.TMD.JepQty = list.JEPQTY;
                clsHcType.TMD.TAmt = list.TAMT;
                clsHcType.TMD.JepDate = list.JEPDATE.ToString();
                clsHcType.TMD.IpGumDate = list.IPGUMDATE.ToString();
                clsHcType.TMD.IpGumAmt = list.IPGUMAMT;
                clsHcType.TMD.GbJohap = list.GBJOHAP.Trim();
                clsHcType.TMD.FrDate = list.FRDATE.ToString();
                clsHcType.TMD.ToDate = list.TODATE.ToString();
                clsHcType.TMD.BuildDate = list.BUILDDATE.ToString();
                clsHcType.TMD.BuildSabun = list.BUILDSABUN;
                clsHcType.TMD.BuildCnt = list.BUILDCNT;
                clsHcType.TMD.GbErrChk = list.GBERRCHK.Trim();
                clsHcType.TMD.Life_Gbn = list.LIFE_GBN.Trim();
                clsHcType.TMD.ROWID = list.ROWID.Trim();
            }
        }
        /// <summary>
        /// 건강보험 1차 판정내역 읽음
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <seealso cref="HcBill.bas> READ_HIC_RES_BOHUM1() "/>
        public void READ_HIC_RES_BOHUM1(long ArgWRTNO)
        {
            int result = 0;

            clsHcType.B1_CLEAR();
            clsHcType.B1.OLDBYENG = new string[7];
            clsHcType.B1.HABIT = new string[5];
            clsHcType.B1.JINCHAL = new string[2];
            clsHcType.B1.PanjengB = new string[9];
            clsHcType.B1.PanjengR = new string[12];

            for (int i = 0; i < 7; i++)
            {
                clsHcType.B1.OLDBYENG[i] = "";
            }

            for (int i = 0; i < 5; i++)
            {
                clsHcType.B1.HABIT[i] = "";
            }

            for (int i = 0; i < 2; i++)
            {
                clsHcType.B1.JINCHAL[i] = "";
            }

            for (int i = 0; i < 9; i++)
            {
                clsHcType.B1.PanjengB[i] = "";
            }

            for (int i = 0; i < 12; i++)
            {
                clsHcType.B1.PanjengR[i] = "";
            }

            //cHT.CLEAR_READ_HIC_RES_BOHUM1();

            HIC_RES_BOHUM1 item = hicResBohum1Service.GetItemByWrtno(ArgWRTNO);
            if (!item.IsNullOrEmpty())
            {

                //Array.Resize(ref clsHcType.B1.OLDBYENG, 2);
                clsHcType.B1.WRTNO = item.WRTNO;
                clsHcType.B1.Height = (int)item.HEIGHT;
                clsHcType.B1.Weight = (int)item.WEIGHT;
                clsHcType.B1.Waist = (int)item.WAIST;
                clsHcType.B1.Biman = item.BIMAN;
                clsHcType.B1.EYE_L = item.EYE_L;
                clsHcType.B1.EYE_R = item.EYE_R;
                clsHcType.B1.EAR_L = item.EAR_L;
                clsHcType.B1.EAR_R = item.EAR_R;
                clsHcType.B1.BLOOD_H = (int)item.BLOOD_H;
                clsHcType.B1.BLOOD_L = (int)item.BLOOD_L;
                clsHcType.B1.URINE1 = item.URINE1;
                clsHcType.B1.URINE2 = item.URINE2;
                clsHcType.B1.URINE3 = item.URINE3;
                clsHcType.B1.BLOOD1 = item.BLOOD1;
                clsHcType.B1.BLOOD2 = (int)item.BLOOD2;
                clsHcType.B1.BLOOD3 = (int)item.BLOOD3;
                clsHcType.B1.BLOOD4 = (int)item.BLOOD4;
                clsHcType.B1.BLOOD5 = (int)item.BLOOD5;
                clsHcType.B1.BLOOD6 = (int)item.BLOOD6;
                clsHcType.B1.LIVER1 = item.LIVER1;
                clsHcType.B1.LIVER2 = item.LIVER2;
                clsHcType.B1.LIVER3 = item.LIVER3;
                clsHcType.B1.XRayGbn = item.XRAYGBN;
                clsHcType.B1.XRayRes = item.XRAYRES;
                clsHcType.B1.EKG = item.EKG;
                clsHcType.B1.FOOT1 = Convert.ToInt32(item.FOOT1);
                clsHcType.B1.FOOT2 = Convert.ToInt32(item.FOOT2);
                clsHcType.B1.BALANCE = Convert.ToInt32(item.BALANCE);
                clsHcType.B1.OSTEO = Convert.ToInt32(item.OSTEO);
                clsHcType.B1.WOMB01 = item.WOMB01;
                clsHcType.B1.WOMB02 = item.WOMB02;
                clsHcType.B1.WOMB03 = item.WOMB03;
                clsHcType.B1.WOMB04 = item.WOMB04;
                clsHcType.B1.WOMB05 = item.WOMB05;
                clsHcType.B1.WOMB06 = item.WOMB06;
                clsHcType.B1.WOMB07 = item.WOMB07;
                clsHcType.B1.WOMB08 = item.WOMB08;
                clsHcType.B1.WOMB09 = item.WOMB09;
                clsHcType.B1.WOMB10 = item.WOMB10;
                clsHcType.B1.WOMB11 = item.WOMB11;

                clsHcType.B1.OLDBYENG[0] = item.OLDBYENG1;
                clsHcType.B1.OLDBYENG[1] = item.OLDBYENG2;
                clsHcType.B1.OLDBYENG[2] = item.OLDBYENG3;
                clsHcType.B1.OLDBYENG[3] = item.OLDBYENG4;
                clsHcType.B1.OLDBYENG[4] = item.OLDBYENG5;
                clsHcType.B1.OLDBYENG[5] = item.OLDBYENG6;
                clsHcType.B1.OLDBYENG[6] = item.OLDBYENG7;
                clsHcType.B1.OLDBYENG1 = "";

                for (int i = 0; i < 7; i++)
                {
                    if (clsHcType.B1.OLDBYENG[i] == "0" || clsHcType.B1.OLDBYENG[i] == "")
                    {
                        clsHcType.B1.OLDBYENG1 = "1";
                    }
                    else
                    {
                        clsHcType.B1.OLDBYENG1 = "2";
                    }
                }

                clsHcType.B1.HABIT[0] = item.HABIT1;
                clsHcType.B1.HABIT[1] = item.HABIT2;
                clsHcType.B1.HABIT[2] = item.HABIT3;
                clsHcType.B1.HABIT[3] = item.HABIT4;
                clsHcType.B1.HABIT[4] = item.HABIT5;
                clsHcType.B1.JINCHAL[0] = item.JINCHAL1;
                clsHcType.B1.JINCHAL[1] = item.JINCHAL2;
                clsHcType.B1.Panjeng = item.PANJENG;
                clsHcType.B1.PanjengB[0] = item.PANJENGB1;
                clsHcType.B1.PanjengB[1] = item.PANJENGB2;
                clsHcType.B1.PanjengB[2] = item.PANJENGB3;
                clsHcType.B1.PanjengB[3] = item.PANJENGB4;
                clsHcType.B1.PanjengB[4] = item.PANJENGB5;
                clsHcType.B1.PanjengB[5] = item.PANJENGB6;
                clsHcType.B1.PanjengB[6] = item.PANJENGB7;
                clsHcType.B1.PanjengB[7] = item.PANJENGB8;
                clsHcType.B1.PanjengB[8] = item.PANJENGB9;
                clsHcType.B1.PanjengR[0] = item.PANJENGR1;
                clsHcType.B1.PanjengR[1] = item.PANJENGR2;
                clsHcType.B1.PanjengR[2] = item.PANJENGR3;
                clsHcType.B1.PanjengR[3] = item.PANJENGR4;
                clsHcType.B1.PanjengR[4] = item.PANJENGR5;
                clsHcType.B1.PanjengR[5] = item.PANJENGR6;
                clsHcType.B1.PanjengR[6] = item.PANJENGR7;
                clsHcType.B1.PanjengR[7] = item.PANJENGR8;
                clsHcType.B1.PanjengR[8] = item.PANJENGR9;
                clsHcType.B1.PanjengR[9] = item.PANJENGR10;
                clsHcType.B1.PanjengR[10] = item.PANJENGR11;
                clsHcType.B1.PanjengR[11] = item.PANJENGR12;
                clsHcType.B1.PanjengB_Etc = item.PANJENGB_ETC;
                clsHcType.B1.PanjengR_Etc = item.PANJENGR_ETC;
                clsHcType.B1.PanjengEtc = item.PANJENGETC;
                clsHcType.B1.Sogen = item.SOGEN;

                clsHcType.B1.PanjengDate = item.PANJENGDATE;
                clsHcType.B1.PanDrNo = item.PANJENGDRNO;
                clsHcType.B1.TongboDate = item.TONGBODATE;
                clsHcType.B1.TongboGbn = item.TONGBOGBN;
                clsHcType.B1.IpsaDate = item.IPSADATE;

                //'문진표 항목
                clsHcType.B1.Sik11 = item.SICK11;
                clsHcType.B1.Sik12 = item.SICK12;
                clsHcType.B1.Sik13 = item.SICK13;
                clsHcType.B1.Sik21 = item.SICK21;
                clsHcType.B1.Sik22 = item.SICK22;
                clsHcType.B1.Sik23 = item.SICK23;
                clsHcType.B1.Sik31 = item.SICK31;
                clsHcType.B1.Sik32 = item.SICK32;
                clsHcType.B1.Sik33 = item.SICK33;

                //'가족질환
                clsHcType.B1.Gajok[0] = item.GAJOK1;
                clsHcType.B1.Gajok[1] = item.GAJOK2;
                clsHcType.B1.Gajok[2] = item.GAJOK3;
                clsHcType.B1.Gajok[3] = item.GAJOK4;
                clsHcType.B1.Gajok[4] = item.GAJOK5;
                clsHcType.B1.Gajok[5] = item.GAJOK6;

                clsHcType.B1.T_STAT01 = item.T_STAT01;
                clsHcType.B1.T_STAT02 = item.T_STAT02;
                clsHcType.B1.T_STAT11 = item.T_STAT11;
                clsHcType.B1.T_STAT12 = item.T_STAT12;
                clsHcType.B1.T_STAT21 = item.T_STAT21;
                clsHcType.B1.T_STAT22 = item.T_STAT22;
                clsHcType.B1.T_STAT31 = item.T_STAT31;
                clsHcType.B1.T_STAT32 = item.T_STAT32;
                clsHcType.B1.T_STAT41 = item.T_STAT41;
                clsHcType.B1.T_STAT42 = item.T_STAT42;
                clsHcType.B1.T_STAT51 = item.T_STAT51;
                clsHcType.B1.T_STAT52 = item.T_STAT52;
                clsHcType.B1.T_GAJOK1 = item.T_GAJOK1;
                clsHcType.B1.T_GAJOK2 = item.T_GAJOK2;
                clsHcType.B1.T_GAJOK3 = item.T_GAJOK3;
                clsHcType.B1.T_GAJOK4 = item.T_GAJOK4;
                clsHcType.B1.T_GAJOK5 = item.T_GAJOK5;
                clsHcType.B1.T_BLIVER = item.T_BLIVER;
                clsHcType.B1.T_SMOKE1 = item.T_SMOKE1;
                clsHcType.B1.T_SMOKE2 = item.T_SMOKE2;
                clsHcType.B1.T_SMOKE3 = item.T_SMOKE3;
                clsHcType.B1.T_SMOKE4 = item.T_SMOKE4;
                clsHcType.B1.T_SMOKE5 = item.T_SMOKE5;
                clsHcType.B1.T_DRINK1 = item.T_DRINK1;
                clsHcType.B1.T_DRINK2 = item.T_DRINK2;
                clsHcType.B1.T_ACTIVE1 = item.T_ACTIVE1;
                clsHcType.B1.T_ACTIVE2 = item.T_ACTIVE2;
                clsHcType.B1.T_ACTIVE3 = item.T_ACTIVE3;
                clsHcType.B1.T40_FEEL1 = item.T40_FEEL1;
                clsHcType.B1.T40_FEEL2 = item.T40_FEEL2;
                clsHcType.B1.T40_FEEL3 = item.T40_FEEL3;
                clsHcType.B1.T40_FEEL4 = item.T40_FEEL4;
                clsHcType.B1.T66_INJECT = item.T66_INJECT;
                clsHcType.B1.T66_STAT1 = item.T66_STAT1;
                clsHcType.B1.T66_STAT2 = item.T66_STAT2;
                clsHcType.B1.T66_STAT3 = item.T66_STAT3;
                clsHcType.B1.T66_STAT4 = item.T66_STAT4;
                clsHcType.B1.T66_STAT5 = item.T66_STAT5;
                clsHcType.B1.T66_STAT6 = item.T66_STAT6;
                clsHcType.B1.T66_FEEL1 = item.T66_FEEL1;
                clsHcType.B1.T66_FEEL2 = item.T66_FEEL2;
                clsHcType.B1.T66_FEEL3 = item.T66_FEEL3;
                clsHcType.B1.T66_MEMORY1 = item.T66_MEMORY1;
                clsHcType.B1.T66_MEMORY2 = item.T66_MEMORY2;
                clsHcType.B1.T66_MEMORY3 = item.T66_MEMORY3;
                clsHcType.B1.T66_MEMORY4 = item.T66_MEMORY4;
                clsHcType.B1.T66_MEMORY5 = item.T66_MEMORY5;
                clsHcType.B1.T66_FALL = item.T66_FALL;
                clsHcType.B1.T66_URO = item.T66_URO;
                clsHcType.B1.PANJENGC1 = item.PANJENGC1;
                clsHcType.B1.PANJENGC2 = item.PANJENGC2;
                clsHcType.B1.PANJENGC3 = item.PANJENGC3;
                clsHcType.B1.PANJENGC4 = item.PANJENGC4;
                clsHcType.B1.PANJENGC5 = item.PANJENGC5;
                clsHcType.B1.PANJENGD11 = item.PANJENGD11;
                clsHcType.B1.PANJENGD12 = item.PANJENGD12;
                clsHcType.B1.PANJENGD13 = item.PANJENGD13;
                clsHcType.B1.PANJENGD21 = item.PANJENGD21;
                clsHcType.B1.PANJENGD22 = item.PANJENGD22;
                clsHcType.B1.PANJENGD23 = item.PANJENGD23;
                clsHcType.B1.PANJENGSAHU = item.PANJENGSAHU;
                clsHcType.B1.PANJENGU1 = item.PANJENGU1;
                clsHcType.B1.PANJENGU2 = item.PANJENGU2;
                clsHcType.B1.PANJENGU3 = item.PANJENGU3;
                clsHcType.B1.PANJENGU4 = item.PANJENGU4;

                //Hcfile.exe > HcBill 추가내용
                clsHcType.B1.TMUN0103 = item.TMUN0103;
                clsHcType.B1.SIM_RESULT1 = item.SIM_RESULT1;
                clsHcType.B1.SIM_RESULT2 = item.SIM_RESULT2;
                clsHcType.B1.SIM_RESULT3 = item.SIM_RESULT3;


                clsHcType.B1.SLIP_BIMAN = item.SLIP_BIMAN;
                clsHcType.B1.ROWID = item.RID;
            }
            else
            {
                clsHcType.B1.ROWID = "";
                clsHcType.B1.PanDrNo = 0;
            }

            if (clsHcType.B1.OLDBYENG1 == "1" || clsHcType.B1.OLDBYENG1 == "2")
            {
                clsDB.setBeginTran(clsDB.DbCon);

                result = hicResBohum1Service.UpdateOldByengbyWrtNo(clsHcType.B1.WRTNO, clsHcType.B1.OLDBYENG1);

                if (result < 0)
                {
                    MessageBox.Show("접수번호 검사결과 저장시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        /// <summary>
        /// 건강보험 2차 판정내역 읽음
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <seealso cref="HcBill.bas> READ_HIC_RES_BOHUM2() "/>
        public void READ_HIC_RES_BOHUM2(long ArgWRTNO)
        {
            clsHcType.B2_CLEAR();
            //clsHcType.B2.ROWID = "";
            HIC_RES_BOHUM2 item = hicResBohum2Service.GetItemByWrtno(ArgWRTNO);
            if (!item.IsNullOrEmpty() && ArgWRTNO > 0)
            {
                clsHcType.B2.WRTNO = item.WRTNO;
                clsHcType.B2.GbChest = item.GBCHEST;
                clsHcType.B2.GbCycle = item.GBCYCLE;
                clsHcType.B2.GbGoji = item.GBGOJI;
                clsHcType.B2.GbLiver = item.GBLIVER;
                clsHcType.B2.GbKidney = item.GBKIDNEY;
                clsHcType.B2.GbAnemia = item.GBANEMIA;
                clsHcType.B2.GbDiabetes = item.GBDIABETES;
                clsHcType.B2.GbEtc = item.GBETC;
                clsHcType.B2.Chest1 = item.CHEST1;
                clsHcType.B2.Chest2 = item.CHEST2;
                clsHcType.B2.Chest3 = item.CHEST3;
                clsHcType.B2.Chest_RES = item.CHEST_RES;
                clsHcType.B2.Cycle1 = item.CYCLE1;
                clsHcType.B2.Cycle2 = item.CYCLE2;
                clsHcType.B2.Cycle3 = item.CYCLE3;
                clsHcType.B2.Cycle4 = item.CYCLE4;
                clsHcType.B2.Cycle_RES = item.CYCLE_RES;
                clsHcType.B2.Goji1 = item.GOJI1;
                clsHcType.B2.Goji2 = item.GOJI2;
                clsHcType.B2.Goji_RES = item.GOJI_RES;
                clsHcType.B2.Liver11 = item.LIVER11;
                clsHcType.B2.Liver12 = item.LIVER12;
                clsHcType.B2.Liver13 = item.LIVER13;
                clsHcType.B2.Liver14 = item.LIVER14;
                clsHcType.B2.Liver15 = item.LIVER15;
                clsHcType.B2.Liver16 = item.LIVER16;
                clsHcType.B2.Liver17 = item.LIVER17;
                clsHcType.B2.Liver18 = item.LIVER18;
                clsHcType.B2.Liver19 = item.LIVER19;
                clsHcType.B2.Liver20 = item.LIVER20;
                clsHcType.B2.Liver21 = item.LIVER21;
                clsHcType.B2.Liver22 = item.LIVER22;
                clsHcType.B2.Liver_Res = item.LIVER_RES;
                clsHcType.B2.Kidney1 = item.KIDNEY1;
                clsHcType.B2.Kidney2 = item.KIDNEY2;
                clsHcType.B2.Kidney3 = item.KIDNEY3;
                clsHcType.B2.Kidney4 = item.KIDNEY4;
                clsHcType.B2.Kidney5 = item.KIDNEY5;
                clsHcType.B2.Kidney_Res = item.KIDNEY_RES;
                clsHcType.B2.Anemia1 = item.ANEMIA1;
                clsHcType.B2.Anemia2 = item.AMEMIA2;
                clsHcType.B2.Anemia3 = item.AMEMIA3;
                clsHcType.B2.Anemia_Res = item.AMEMIA_RES;
                clsHcType.B2.Diabetes1 = item.DIABETES1;
                clsHcType.B2.Diabetes2 = item.DIABETES2;
                clsHcType.B2.Diabetes3 = item.DIABETES3;
                clsHcType.B2.Diabetes_Res = item.DIABETES_RES;
                clsHcType.B2.Etc_Res = item.ETC_RES;
                clsHcType.B2.Etc_Exam = item.ETC_EXAM;
                clsHcType.B2.Sogen = item.SOGEN;
                clsHcType.B2.Panjeng = item.PANJENG;
                clsHcType.B2.Panjeng_D1 = item.PANJENG_D1;
                clsHcType.B2.Panjeng_D11 = item.PANJENG_D11;
                clsHcType.B2.Panjeng_D12 = item.PANJENG_D12;
                clsHcType.B2.PANJENG_SO1 = item.PANJENG_SO1;
                clsHcType.B2.PANJENG_SO2 = item.PANJENG_SO2;
                clsHcType.B2.PANJENG_SO3 = item.PANJENG_SO3;
                clsHcType.B2.PanjengDate = item.PANJENGDATE;
                clsHcType.B2.TongboGbn = item.TONGBOGBN;
                clsHcType.B2.TongboDate = item.TONGBODATE;
                clsHcType.B2.PanjengDrNo = item.PANJENGDRNO;
                clsHcType.B2.GunDate = item.GUNDATE;
                clsHcType.B2.GbPrint = item.GBPRINT;
                clsHcType.B2.WorkYN = item.WORKYN;
                clsHcType.B2.DIABETES_RES_CARE = item.DIABETES_RES_CARE;
                clsHcType.B2.CYCLE_RES_CARE = item.CYCLE_RES_CARE;
                clsHcType.B2.T66_MEM = item.T66_MEM;
                clsHcType.B2.T_SangDam_1 = item.T_SANGDAM_1;
                clsHcType.B2.GbGonghu = item.GBGONGHU;
                clsHcType.B2.ROWID = item.RID;
            }
            else
            {
                clsHcType.B2.Cycle_RES = "";
                clsHcType.B2.Diabetes_Res = "";
                clsHcType.B2.PanjengDrNo = 0;
            }
        }

        /// <summary>
        /// 건강보험 암 판정내역 읽음
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <seealso cref="HcBill.bas> READ_HIC_RES_CANCER() "/>
        public void READ_HIC_CANCER_NEW(long ArgWRTNO)
        {
            clsHcType.B3_CLEAR();
            HIC_CANCER_NEW item = hicCancerNewService.GetItemByWRTNO(ArgWRTNO);

            if (!item.IsNullOrEmpty())
            {
                clsHcType.B3.WRTNO = item.WRTNO;
                clsHcType.B3.Stomach_S = item.STOMACH_S;
                clsHcType.B3.Stomach_B = item.STOMACH_B;
                clsHcType.B3.Stomach_P = item.STOMACH_P;
                clsHcType.B3.Stomach_PETC = item.STOMACH_PETC;
                clsHcType.B3.S_ENDOGBN = item.S_ENDOGBN;
                clsHcType.B3.Stomach_SENDO = item.STOMACH_SENDO;
                clsHcType.B3.Stomach_BENDO = item.STOMACH_BENDO;
                clsHcType.B3.Stomach_PENDO = item.STOMACH_PENDO;
                clsHcType.B3.Stomach_ENDOETC = item.STOMACH_ENDOETC;
                clsHcType.B3.S_ANATGBN = "";
                clsHcType.B3.S_ANAT = item.S_ANAT;
                clsHcType.B3.S_ANATETC = item.S_ANATETC;
                clsHcType.B3.S_PANJENG = item.S_PANJENG;
                clsHcType.B3.S_MONTH = item.S_MONTH;
                clsHcType.B3.S_JILETC = item.S_JILETC;
                clsHcType.B3.S_PLACE = item.S_PLACE;
                clsHcType.B3.COLON_RESULT = item.COLON_RESULT;
                clsHcType.B3.COLONGBN = item.COLONGBN;
                clsHcType.B3.COLON_S = item.COLON_S;
                clsHcType.B3.COLON_B = item.COLON_B;
                clsHcType.B3.COLON_P = item.COLON_P;
                clsHcType.B3.COLON_ENDOGBN = item.COLON_ENDOGBN;
                clsHcType.B3.COLON_SENDO = item.COLON_SENDO;
                clsHcType.B3.COLON_BENDO = item.COLON_BENDO;
                clsHcType.B3.COLON_PENDO = item.COLON_PENDO;
                clsHcType.B3.COLON_PETC = item.COLON_PETC;
                clsHcType.B3.COLON_ENDOETC = item.COLON_ENDOETC;
                clsHcType.B3.C_ENDOGBN = item.C_ENDOGBN;
                clsHcType.B3.C_ANATGBN = item.C_ANATGBN;
                clsHcType.B3.C_ANAT = item.C_ANAT;
                clsHcType.B3.C_ANATETC = item.C_ANATETC;
                clsHcType.B3.C_PANJENG = item.C_PANJENG;
                clsHcType.B3.C_MONTH = item.C_MONTH;
                clsHcType.B3.C_JILETC = item.C_JILETC;
                clsHcType.B3.C_PLACE = item.C_PLACE;
                clsHcType.B3.Liver_S = item.LIVER_S;
                clsHcType.B3.Liver_B = item.LIVER_B;
                clsHcType.B3.Liver_P = item.LIVER_P;
                clsHcType.B3.Liver_SIZE = item.LIVER_SIZE;
                clsHcType.B3.Liver_LSTYLE = item.LIVER_LSTYLE;
                clsHcType.B3.Liver_VIOLATE = item.LIVER_VIOLATE;
                clsHcType.B3.Liver_DISEASSE = item.LIVER_DISEASSE;
                clsHcType.B3.Liver_ETC = item.LIVER_ETC;
                //clsHcType.B3.Liver_RPHA_EIA = item.LIVER_RPHA_EIA;
                clsHcType.B3.Liver_RPHA = item.LIVER_RPHA;
                //clsHcType.B3.Liver_EIA_S = item.LIVER_EIA_S;
                clsHcType.B3.Liver_EIA = item.LIVER_EIA;
                clsHcType.B3.Liver_PANJENG = item.LIVER_PANJENG;
                clsHcType.B3.Liver_JILETC = item.LIVER_JILETC;
                clsHcType.B3.Liver_PLACE = item.LIVER_PLACE;
                clsHcType.B3.Liver_New_Alt = item.LIVER_NEW_ALT;
                clsHcType.B3.Liver_New_B = item.LIVER_NEW_B;
                clsHcType.B3.Liver_New_BRes = item.LIVER_NEW_BRESULT;
                clsHcType.B3.Liver_New_C = item.LIVER_NEW_C;
                clsHcType.B3.Liver_New_CRes = item.LIVER_NEW_CRESULT;
                clsHcType.B3.BREAST_S = item.BREAST_S;
                clsHcType.B3.BREAST_P = item.BREAST_P;
                clsHcType.B3.BREAST_ETC = item.BREAST_ETC;
                clsHcType.B3.B_ANATGBN = item.B_ANATGBN;
                clsHcType.B3.B_ANAT = item.B_ANAT;
                clsHcType.B3.B_ANATETC = item.B_ANATETC;
                clsHcType.B3.B_PANJENG = item.B_PANJENG;
                clsHcType.B3.B_MONTH = item.B_MONTH;
                clsHcType.B3.B_JILETC = item.B_JILETC;
                clsHcType.B3.B_PLACE = item.B_PLACE;
                clsHcType.B3.Height = (int)item.HEIGHT;
                clsHcType.B3.Weight = (int)item.WEIGHT;
                clsHcType.B3.GBSTOMACH = item.GBSTOMACH;
                clsHcType.B3.GbLiver = item.GBLIVER;
                clsHcType.B3.GBRECTUM = item.GBRECTUM;
                clsHcType.B3.GBBREAST = item.GBBREAST;
                clsHcType.B3.GbWomb = item.GBWOMB;
                //clsHcType.B3.GBLUNG = item.GBLUNG;
                clsHcType.B3.SICK11 = item.SICK11;
                clsHcType.B3.SICK12 = item.SICK12;
                clsHcType.B3.SICK21 = item.SICK21;
                clsHcType.B3.SICK22 = item.SICK22;
                clsHcType.B3.SICK31 = item.SICK31;
                clsHcType.B3.SICK32 = item.SICK32;
                clsHcType.B3.SICK41 = item.SICK41;
                clsHcType.B3.SICK42 = item.SICK42;
                clsHcType.B3.SICK51 = item.SICK51;
                clsHcType.B3.SICK52 = item.SICK52;
                clsHcType.B3.SICK61 = item.SICK61;
                clsHcType.B3.SICK62 = item.SICK62;
                clsHcType.B3.SICK71 = item.SICK71;
                clsHcType.B3.SICK72 = item.SICK72;
                clsHcType.B3.SICK81 = item.SICK81;
                clsHcType.B3.SICK82 = item.SICK82;
                clsHcType.B3.SICK91 = item.SICK91;
                clsHcType.B3.SICK92 = item.SICK92;
                clsHcType.B3.JUNGSANG01 = item.JUNGSANG01;
                clsHcType.B3.JUNGSANG02 = item.JUNGSANG02;
                clsHcType.B3.JUNGSANG03 = item.JUNGSANG03;
                clsHcType.B3.JUNGSANG04 = item.JUNGSANG04;
                clsHcType.B3.JUNGSANG05 = item.JUNGSANG05;
                clsHcType.B3.JUNGSANG06 = item.JUNGSANG06;
                clsHcType.B3.JUNGSANG07 = item.JUNGSANG07;
                clsHcType.B3.JUNGSANG08 = item.JUNGSANG08;
                clsHcType.B3.JUNGSANG09 = item.JUNGSANG09;
                clsHcType.B3.JUNGSANG10 = item.JUNGSANG10;
                clsHcType.B3.JUNGSANG11 = item.JUNGSANG11;
                clsHcType.B3.JUNGSANG12 = item.JUNGSANG12;
                clsHcType.B3.JUNGSANG13 = item.JUNGSANG13;
                clsHcType.B3.JUNGSANG14 = item.JUNGSANG14;
                clsHcType.B3.JUNGSANG15 = item.JUNGSANG15;
                clsHcType.B3.GAJOK1 = item.GAJOK1;
                clsHcType.B3.GAJOKETC = item.GAJOKETC;
                clsHcType.B3.DRINK1 = item.DRINK1;
                clsHcType.B3.DRINK2 = item.DRINK2;
                clsHcType.B3.SMOKING1 = item.SMOKING1;
                clsHcType.B3.SMOKING2 = item.SMOKING2;
                clsHcType.B3.WOMAN1 = item.WOMAN1;
                clsHcType.B3.WOMAN2 = item.WOMAN2;
                clsHcType.B3.WOMAN3 = item.WOMAN3;
                clsHcType.B3.WOMAN4 = item.WOMAN4;
                clsHcType.B3.WOMAN5 = item.WOMAN5;
                clsHcType.B3.WOMAN6 = item.WOMAN6;
                clsHcType.B3.WOMAN7 = item.WOMAN7;
                clsHcType.B3.WOMAN8 = item.WOMAN8;
                clsHcType.B3.WOMAN9 = item.WOMAN9;
                clsHcType.B3.WOMAN10 = item.WOMAN10;
                clsHcType.B3.WOMAN11 = item.WOMAN11;
                clsHcType.B3.WOMAN12 = item.WOMAN12;
                clsHcType.B3.WOMAN13 = item.WOMAN13;
                clsHcType.B3.WOMB01 = item.WOMB01;
                clsHcType.B3.WOMB02 = item.WOMB02;
                clsHcType.B3.WOMB03 = item.WOMB03;
                clsHcType.B3.WOMB04 = item.WOMB04;
                clsHcType.B3.WOMB05 = item.WOMB05;
                clsHcType.B3.WOMB06 = item.WOMB06;
                clsHcType.B3.WOMB07 = item.WOMB07;
                clsHcType.B3.WOMB08 = item.WOMB08;
                clsHcType.B3.WOMB09 = item.WOMB09;
                clsHcType.B3.WOMB10 = item.WOMB10;
                clsHcType.B3.WOMB11 = item.WOMB11;
                clsHcType.B3.WOMB_PLACE = item.WOMB_PLACE;
                clsHcType.B3.NEW_SICK01 = item.NEW_SICK01;
                clsHcType.B3.NEW_SICK02 = item.NEW_SICK02;
                clsHcType.B3.NEW_SICK03 = item.NEW_SICK03;
                clsHcType.B3.NEW_SICK04 = item.NEW_SICK04;
                clsHcType.B3.NEW_SICK06 = item.NEW_SICK06;
                clsHcType.B3.NEW_SICK07 = item.NEW_SICK07;
                clsHcType.B3.NEW_SICK08 = item.NEW_SICK08;
                clsHcType.B3.NEW_SICK09 = item.NEW_SICK09;
                clsHcType.B3.NEW_SICK11 = item.NEW_SICK11;
                clsHcType.B3.NEW_SICK12 = item.NEW_SICK12;
                clsHcType.B3.NEW_SICK13 = item.NEW_SICK13;
                clsHcType.B3.NEW_SICK14 = item.NEW_SICK14;
                clsHcType.B3.NEW_SICK16 = item.NEW_SICK16;
                clsHcType.B3.NEW_SICK17 = item.NEW_SICK17;
                clsHcType.B3.NEW_SICK18 = item.NEW_SICK18;
                clsHcType.B3.NEW_SICK19 = item.NEW_SICK19;
                clsHcType.B3.NEW_SICK20 = item.NEW_SICK20;
                clsHcType.B3.NEW_SICK21 = item.NEW_SICK21;
                clsHcType.B3.NEW_SICK22 = item.NEW_SICK22;
                clsHcType.B3.NEW_SICK23 = item.NEW_SICK23;
                clsHcType.B3.NEW_SICK24 = item.NEW_SICK24;
                clsHcType.B3.NEW_SICK25 = item.NEW_SICK25;
                clsHcType.B3.NEW_SICK26 = item.NEW_SICK26;
                clsHcType.B3.NEW_SICK27 = item.NEW_SICK27;
                clsHcType.B3.NEW_SICK28 = item.NEW_SICK28;
                clsHcType.B3.NEW_SICK29 = item.NEW_SICK29;
                clsHcType.B3.NEW_SICK30 = item.NEW_SICK30;
                clsHcType.B3.NEW_SICK31 = item.NEW_SICK31;
                clsHcType.B3.NEW_SICK32 = item.NEW_SICK32;
                clsHcType.B3.NEW_SICK33 = item.NEW_SICK33;
                clsHcType.B3.NEW_SICK34 = item.NEW_SICK34;
                clsHcType.B3.NEW_SICK36 = item.NEW_SICK36;
                clsHcType.B3.NEW_SICK37 = item.NEW_SICK37;
                clsHcType.B3.NEW_SICK38 = item.NEW_SICK38;
                clsHcType.B3.NEW_SICK39 = item.NEW_SICK39;
                clsHcType.B3.NEW_SICK41 = item.NEW_SICK41;
                clsHcType.B3.NEW_SICK42 = item.NEW_SICK42;
                clsHcType.B3.NEW_SICK43 = item.NEW_SICK43;
                clsHcType.B3.NEW_SICK44 = item.NEW_SICK44;
                clsHcType.B3.NEW_SICK46 = item.NEW_SICK46;
                clsHcType.B3.NEW_SICK47 = item.NEW_SICK47;
                clsHcType.B3.NEW_SICK48 = item.NEW_SICK48;
                clsHcType.B3.NEW_SICK49 = item.NEW_SICK49;
                clsHcType.B3.NEW_SICK51 = item.NEW_SICK51;
                clsHcType.B3.NEW_SICK52 = item.NEW_SICK52;
                clsHcType.B3.NEW_SICK53 = item.NEW_SICK53;
                clsHcType.B3.NEW_SICK54 = item.NEW_SICK54;
                clsHcType.B3.NEW_SICK56 = item.NEW_SICK56;
                clsHcType.B3.NEW_SICK57 = item.NEW_SICK57;
                clsHcType.B3.NEW_SICK58 = item.NEW_SICK58;
                clsHcType.B3.NEW_SICK59 = item.NEW_SICK59;
                clsHcType.B3.NEW_SICK61 = item.NEW_SICK61;
                clsHcType.B3.NEW_SICK62 = item.NEW_SICK62;
                clsHcType.B3.NEW_SICK63 = item.NEW_SICK63;
                clsHcType.B3.NEW_SICK64 = item.NEW_SICK64;
                clsHcType.B3.NEW_SICK66 = item.NEW_SICK66;
                clsHcType.B3.NEW_SICK67 = item.NEW_SICK67;
                clsHcType.B3.NEW_SICK68 = item.NEW_SICK68;
                clsHcType.B3.NEW_SICK69 = item.NEW_SICK69;
                clsHcType.B3.NEW_SICK71 = item.NEW_SICK71;
                clsHcType.B3.NEW_SICK72 = item.NEW_SICK72;
                clsHcType.B3.NEW_SICK73 = item.NEW_SICK73;
                clsHcType.B3.NEW_SICK74 = item.NEW_SICK74;
                clsHcType.B3.NEW_B_SICK01 = item.NEW_B_SICK01;
                clsHcType.B3.NEW_B_SICK02 = item.NEW_B_SICK02;
                clsHcType.B3.NEW_B_SICK03 = item.NEW_B_SICK03;
                clsHcType.B3.NEW_B_SICK04 = item.NEW_B_SICK04;
                clsHcType.B3.NEW_B_SICK05 = item.NEW_B_SICK05;
                clsHcType.B3.NEW_B_SICK06 = item.NEW_B_SICK06;
                clsHcType.B3.NEW_N_SICK01 = item.NEW_N_SICK01;
                clsHcType.B3.NEW_N_SICK02 = item.NEW_N_SICK02;
                clsHcType.B3.NEW_N_SICK03 = item.NEW_N_SICK03;
                clsHcType.B3.NEW_S_SICK01 = item.NEW_S_SICK01;
                clsHcType.B3.NEW_S_SICK02 = item.NEW_S_SICK02;
                clsHcType.B3.NEW_S_SICK03 = item.NEW_S_SICK03;
                clsHcType.B3.NEW_S_SICK04 = item.NEW_S_SICK04;
                clsHcType.B3.NEW_CAN_01 = item.NEW_CAN_01;
                clsHcType.B3.NEW_CAN_02 = item.NEW_CAN_02;
                clsHcType.B3.NEW_CAN_03 = item.NEW_CAN_03;
                clsHcType.B3.NEW_CAN_04 = item.NEW_CAN_04;
                clsHcType.B3.NEW_CAN_06 = item.NEW_CAN_06;
                clsHcType.B3.NEW_CAN_07 = item.NEW_CAN_07;
                clsHcType.B3.NEW_CAN_08 = item.NEW_CAN_08;
                clsHcType.B3.NEW_CAN_09 = item.NEW_CAN_09;
                clsHcType.B3.NEW_CAN_11 = item.NEW_CAN_11;
                clsHcType.B3.NEW_CAN_12 = item.NEW_CAN_12;
                clsHcType.B3.NEW_CAN_13 = item.NEW_CAN_13;
                clsHcType.B3.NEW_CAN_14 = item.NEW_CAN_14;
                clsHcType.B3.NEW_CAN_16 = item.NEW_CAN_16;
                clsHcType.B3.NEW_CAN_17 = item.NEW_CAN_17;
                clsHcType.B3.NEW_CAN_18 = item.NEW_CAN_18;
                clsHcType.B3.NEW_CAN_19 = item.NEW_CAN_19;
                clsHcType.B3.NEW_CAN_21 = item.NEW_CAN_21;
                clsHcType.B3.NEW_CAN_22 = item.NEW_CAN_22;
                clsHcType.B3.NEW_CAN_23 = item.NEW_CAN_23;
                clsHcType.B3.NEW_CAN_24 = item.NEW_CAN_24;
                clsHcType.B3.NEW_CAN_26 = item.NEW_CAN_26;
                clsHcType.B3.NEW_CAN_27 = item.NEW_CAN_27;
                clsHcType.B3.NEW_CAN_28 = item.NEW_CAN_28;
                clsHcType.B3.NEW_CAN_29 = item.NEW_CAN_29;
                clsHcType.B3.NEW_HARD = item.NEW_HARD;
                clsHcType.B3.NEW_MARRIED = item.NEW_MARRIED;
                clsHcType.B3.NEW_SCHOOL = item.NEW_SCHOOL;
                clsHcType.B3.NEW_WORK01 = item.NEW_WORK01;
                clsHcType.B3.NEW_WORK02 = item.NEW_WORK02;
                clsHcType.B3.NEW_SMOKE01 = item.NEW_SMOKE01;
                clsHcType.B3.NEW_SMOKE02 = item.NEW_SMOKE02;
                clsHcType.B3.NEW_SMOKE03 = item.NEW_SMOKE03;
                clsHcType.B3.NEW_SMOKE04 = item.NEW_SMOKE04;
                clsHcType.B3.NEW_SMOKE05 = item.NEW_SMOKE05;
                clsHcType.B3.NEW_DRINK01 = item.NEW_DRINK01;
                clsHcType.B3.NEW_DRINK02 = item.NEW_DRINK02;
                clsHcType.B3.NEW_DRINK03 = item.NEW_DRINK03;
                clsHcType.B3.NEW_DRINK04 = item.NEW_DRINK04;
                clsHcType.B3.NEW_DRINK05 = item.NEW_DRINK05;
                clsHcType.B3.NEW_DRINK06 = item.NEW_DRINK06;
                clsHcType.B3.NEW_DRINK07 = item.NEW_DRINK07;
                clsHcType.B3.NEW_DRINK08 = item.NEW_DRINK08;
                clsHcType.B3.NEW_DRINK09 = item.NEW_DRINK09;
                clsHcType.B3.NEW_WOMAN01 = item.NEW_WOMAN01;
                clsHcType.B3.NEW_WOMAN02 = item.NEW_WOMAN02;
                clsHcType.B3.NEW_WOMAN03 = item.NEW_WOMAN03;
                clsHcType.B3.NEW_WOMAN11 = item.NEW_WOMAN11;
                clsHcType.B3.NEW_WOMAN12 = item.NEW_WOMAN12;
                clsHcType.B3.NEW_WOMAN13 = item.NEW_WOMAN13;
                clsHcType.B3.NEW_WOMAN14 = item.NEW_WOMAN14;
                clsHcType.B3.NEW_WOMAN15 = item.NEW_WOMAN15;
                clsHcType.B3.NEW_WOMAN16 = item.NEW_WOMAN16;
                clsHcType.B3.NEW_WOMAN17 = item.NEW_WOMAN17;
                clsHcType.B3.NEW_WOMAN18 = item.NEW_WOMAN18;
                clsHcType.B3.NEW_WOMAN19 = item.NEW_WOMAN19;
                clsHcType.B3.NEW_WOMAN20 = item.NEW_WOMAN20;
                clsHcType.B3.NEW_WOMAN21 = item.NEW_WOMAN21;
                clsHcType.B3.NEW_WOMAN22 = item.NEW_WOMAN22;
                clsHcType.B3.NEW_WOMAN23 = item.NEW_WOMAN23;
                clsHcType.B3.NEW_WOMAN24 = item.NEW_WOMAN24;
                clsHcType.B3.NEW_WOMAN25 = item.NEW_WOMAN25;
                clsHcType.B3.NEW_WOMAN26 = item.NEW_WOMAN26;
                clsHcType.B3.NEW_WOMAN27 = item.NEW_WOMAN27;
                clsHcType.B3.NEW_WOMAN31 = item.NEW_WOMAN31;
                clsHcType.B3.NEW_WOMAN41 = item.NEW_WOMAN41;
                clsHcType.B3.NEW_WOMAN42 = item.NEW_WOMAN42;
                clsHcType.B3.NEW_WOMAN43 = item.NEW_WOMAN43;
                clsHcType.B3.NEW_CAN_WOMAN01 = item.NEW_CAN_WOMAN01;
                clsHcType.B3.NEW_CAN_WOMAN02 = item.NEW_CAN_WOMAN02;
                clsHcType.B3.NEW_CAN_WOMAN03 = item.NEW_CAN_WOMAN03;
                clsHcType.B3.NEW_CAN_WOMAN04 = item.NEW_CAN_WOMAN04;
                clsHcType.B3.NEW_CAN_WOMAN06 = item.NEW_CAN_WOMAN06;
                clsHcType.B3.NEW_CAN_WOMAN07 = item.NEW_CAN_WOMAN07;
                clsHcType.B3.NEW_CAN_WOMAN08 = item.NEW_CAN_WOMAN08;
                clsHcType.B3.NEW_CAN_WOMAN09 = item.NEW_CAN_WOMAN09;
                clsHcType.B3.NEW_CAN_WOMAN11 = item.NEW_CAN_WOMAN11;
                clsHcType.B3.NEW_CAN_WOMAN12 = item.NEW_CAN_WOMAN12;
                clsHcType.B3.NEW_CAN_WOMAN13 = item.NEW_CAN_WOMAN13;
                clsHcType.B3.NEW_CAN_WOMAN14 = item.NEW_CAN_WOMAN14;
                clsHcType.B3.NEW_CAN_WOMAN16 = item.NEW_CAN_WOMAN16;
                clsHcType.B3.NEW_CAN_WOMAN17 = item.NEW_CAN_WOMAN17;
                clsHcType.B3.NEW_CAN_WOMAN18 = item.NEW_CAN_WOMAN18;
                clsHcType.B3.NEW_CAN_WOMAN19 = item.NEW_CAN_WOMAN19;
                clsHcType.B3.S_SOGEN = item.S_SOGEN;
                clsHcType.B3.C_SOGEN = item.C_SOGEN;
                clsHcType.B3.L_SOGEN = item.L_SOGEN;
                clsHcType.B3.B_SOGEN = item.B_SOGEN;
                clsHcType.B3.W_SOGEN = item.W_SOGEN;
                clsHcType.B3.S_PANJENGDATE = item.S_PANJENGDATE;
                clsHcType.B3.C_PANJENGDATE = item.C_PANJENGDATE;
                clsHcType.B3.L_PANJENGDATE = item.L_PANJENGDATE;
                clsHcType.B3.B_PANJENGDATE = item.B_PANJENGDATE;
                clsHcType.B3.W_PANJENGDATE = item.W_PANJENGDATE;
                clsHcType.B3.S_SOGEN2 = item.S_SOGEN2;
                clsHcType.B3.C_SOGEN2 = item.C_SOGEN2;
                clsHcType.B3.C_SOGEN3 = item.C_SOGEN3;
                //clsHcType.B3.Jin_New = item.JIN_NEW;
                //clsHcType.B3.PanDrNo_New1 = item.PANDRNO_NEW1;
                //clsHcType.B3.PanDrNo_New2 = item.PANDRNO_NEW2;
                //clsHcType.B3.PanDrNo_New3 = item.PANDRNO_NEW3;
                //clsHcType.B3.PanDrNo_New4 = item.PANDRNO_NEW4;
                //clsHcType.B3.PanDrNo_New5 = item.PANDRNO_NEW5;
                //clsHcType.B3.Panjeng = item.PANJENG;
                //clsHcType.B3.PanjengDate = item.PANJENGDATE;
                clsHcType.B3.TongboGbn = item.TONGBOGBN;
                clsHcType.B3.TongboDate = item.TONGBODATE;
                clsHcType.B3.PanjengDrNo = Convert.ToInt32(item.PANJENGDRNO);
                clsHcType.B3.Sogen = item.SOGEN;
                clsHcType.B3.GunDate = item.GUNDATE;
                clsHcType.B3.JinchalGbn = item.JINCHALGBN;
                clsHcType.B3.Can_MirGbn = item.CAN_MIRGBN;
                clsHcType.B3.RESULT0001 = item.RESULT0001;
                clsHcType.B3.RESULT0002 = item.RESULT0002;
                clsHcType.B3.RESULT0003 = item.RESULT0003;
                clsHcType.B3.RESULT0004 = item.RESULT0004;
                clsHcType.B3.RESULT0005 = item.RESULT0005;
                clsHcType.B3.RESULT0006 = item.RESULT0006;
                clsHcType.B3.RESULT0007 = item.RESULT0007;
                clsHcType.B3.RESULT0008 = item.RESULT0008;
                clsHcType.B3.RESULT0009 = item.RESULT0009;
                clsHcType.B3.RESULT0010 = item.RESULT0010;
                clsHcType.B3.RESULT0011 = item.RESULT0011;
                clsHcType.B3.RESULT0012 = item.RESULT0012;
                clsHcType.B3.RESULT0013 = item.RESULT0013;
                clsHcType.B3.RESULT0014 = item.RESULT0014;
                clsHcType.B3.RESULT0015 = item.RESULT0015;
                clsHcType.B3.RESULT0016 = item.RESULT0016;
                clsHcType.B3.PANJENGDRNO1 = Convert.ToString(item.PANJENGDRNO1);
                clsHcType.B3.PANJENGDRNO2 = Convert.ToString(item.PANJENGDRNO2);
                clsHcType.B3.PANJENGDRNO3 = Convert.ToString(item.PANJENGDRNO3);
                clsHcType.B3.PANJENGDRNO4 = Convert.ToString(item.PANJENGDRNO4);
                clsHcType.B3.PANJENGDRNO5 = Convert.ToString(item.PANJENGDRNO5);
                clsHcType.B3.PANJENGDRNO6 = Convert.ToString(item.PANJENGDRNO6);
                clsHcType.B3.PANJENGDRNO7 = Convert.ToString(item.PANJENGDRNO7);
                clsHcType.B3.PANJENGDRNO8 = Convert.ToString(item.PANJENGDRNO8);
                clsHcType.B3.PANJENGDRNO9 = Convert.ToString(item.PANJENGDRNO9);
                clsHcType.B3.PANJENGDRNO10 = Convert.ToString(item.PANJENGDRNO10);
                clsHcType.B3.PANJENGDRNO11 = Convert.ToString(item.PANJENGDRNO11);
                clsHcType.B3.NEW_SICK75 = item.NEW_SICK75;
                clsHcType.B3.NEW_SICK76 = item.NEW_SICK76;
                clsHcType.B3.NEW_SICK77 = item.NEW_SICK77;
                clsHcType.B3.NEW_SICK78 = item.NEW_SICK78;
                clsHcType.B3.GBLUNG = item.GBLUNG.Trim();
                clsHcType.B3.L_PANJENGDATE1 = item.L_PANJENGDATE1;
                clsHcType.B3.LUNG_RESULT001 = item.LUNG_RESULT001.To<string>();
                clsHcType.B3.LUNG_RESULT002 = item.LUNG_RESULT002;
                clsHcType.B3.LUNG_RESULT003 = item.LUNG_RESULT003;
                clsHcType.B3.LUNG_RESULT004 = item.LUNG_RESULT004;
                clsHcType.B3.LUNG_RESULT005 = item.LUNG_RESULT005;
                clsHcType.B3.LUNG_RESULT006 = item.LUNG_RESULT006;
                clsHcType.B3.LUNG_RESULT007 = item.LUNG_RESULT007;
                clsHcType.B3.LUNG_RESULT008 = item.LUNG_RESULT008;
                clsHcType.B3.LUNG_RESULT009 = item.LUNG_RESULT009;
                clsHcType.B3.LUNG_RESULT010 = item.LUNG_RESULT010;
                clsHcType.B3.LUNG_RESULT011 = item.LUNG_RESULT011;
                clsHcType.B3.LUNG_RESULT012 = item.LUNG_RESULT012;
                clsHcType.B3.LUNG_RESULT013 = item.LUNG_RESULT013;
                clsHcType.B3.LUNG_RESULT014 = item.LUNG_RESULT014;
                clsHcType.B3.LUNG_RESULT015 = item.LUNG_RESULT015;
                clsHcType.B3.LUNG_RESULT016 = item.LUNG_RESULT016;
                clsHcType.B3.LUNG_RESULT017 = item.LUNG_RESULT017;
                clsHcType.B3.LUNG_RESULT018 = item.LUNG_RESULT018;
                clsHcType.B3.LUNG_RESULT019 = item.LUNG_RESULT019;
                clsHcType.B3.LUNG_RESULT020 = item.LUNG_RESULT020;
                clsHcType.B3.LUNG_RESULT021 = item.LUNG_RESULT021;
                clsHcType.B3.LUNG_RESULT022 = item.LUNG_RESULT022;
                clsHcType.B3.LUNG_RESULT023 = item.LUNG_RESULT023;
                clsHcType.B3.LUNG_RESULT024 = item.LUNG_RESULT024;
                clsHcType.B3.LUNG_RESULT025 = item.LUNG_RESULT025;
                clsHcType.B3.LUNG_RESULT026 = item.LUNG_RESULT026;
                clsHcType.B3.LUNG_RESULT027 = item.LUNG_RESULT027;
                clsHcType.B3.LUNG_RESULT028 = item.LUNG_RESULT028;
                clsHcType.B3.LUNG_RESULT029 = item.LUNG_RESULT029;
                clsHcType.B3.LUNG_RESULT030 = item.LUNG_RESULT030;
                clsHcType.B3.LUNG_RESULT031 = item.LUNG_RESULT031;
                clsHcType.B3.LUNG_RESULT032 = item.LUNG_RESULT032;
                clsHcType.B3.LUNG_RESULT033 = item.LUNG_RESULT033;
                clsHcType.B3.LUNG_RESULT034 = item.LUNG_RESULT034;
                clsHcType.B3.LUNG_RESULT035 = item.LUNG_RESULT035;
                clsHcType.B3.LUNG_RESULT036 = item.LUNG_RESULT036;
                clsHcType.B3.LUNG_RESULT037 = item.LUNG_RESULT037;
                clsHcType.B3.LUNG_RESULT038 = item.LUNG_RESULT038;
                clsHcType.B3.LUNG_RESULT039 = item.LUNG_RESULT039;
                clsHcType.B3.LUNG_RESULT040 = item.LUNG_RESULT040;
                clsHcType.B3.LUNG_RESULT041 = item.LUNG_RESULT041;
                clsHcType.B3.LUNG_RESULT042 = item.LUNG_RESULT042;
                clsHcType.B3.LUNG_RESULT043 = item.LUNG_RESULT043;
                clsHcType.B3.LUNG_RESULT044 = item.LUNG_RESULT044;
                clsHcType.B3.LUNG_RESULT045 = item.LUNG_RESULT045;
                clsHcType.B3.LUNG_RESULT046 = item.LUNG_RESULT046;
                clsHcType.B3.LUNG_RESULT047 = item.LUNG_RESULT047;
                clsHcType.B3.LUNG_RESULT048 = item.LUNG_RESULT048;
                clsHcType.B3.LUNG_RESULT049 = item.LUNG_RESULT049;
                clsHcType.B3.LUNG_RESULT050 = item.LUNG_RESULT050;
                clsHcType.B3.LUNG_RESULT051 = item.LUNG_RESULT051;
                clsHcType.B3.LUNG_RESULT052 = item.LUNG_RESULT052;
                clsHcType.B3.LUNG_RESULT053 = item.LUNG_RESULT053;
                clsHcType.B3.LUNG_RESULT054 = item.LUNG_RESULT054;
                clsHcType.B3.LUNG_RESULT055 = item.LUNG_RESULT055;
                clsHcType.B3.LUNG_RESULT056 = item.LUNG_RESULT056;
                clsHcType.B3.LUNG_RESULT057 = item.LUNG_RESULT057;
                clsHcType.B3.LUNG_RESULT058 = item.LUNG_RESULT058;
                clsHcType.B3.LUNG_RESULT059 = item.LUNG_RESULT059;
                clsHcType.B3.LUNG_RESULT060 = item.LUNG_RESULT060;
                clsHcType.B3.LUNG_RESULT061 = item.LUNG_RESULT061;
                clsHcType.B3.LUNG_RESULT062 = item.LUNG_RESULT062;
                clsHcType.B3.LUNG_RESULT063 = item.LUNG_RESULT063;
                clsHcType.B3.LUNG_RESULT064 = item.LUNG_RESULT064;
                clsHcType.B3.LUNG_RESULT065 = item.LUNG_RESULT065;
                clsHcType.B3.LUNG_RESULT066 = item.LUNG_RESULT066;
                clsHcType.B3.LUNG_RESULT067 = item.LUNG_RESULT067;
                clsHcType.B3.LUNG_RESULT068 = item.LUNG_RESULT068;
                clsHcType.B3.LUNG_RESULT069 = item.LUNG_RESULT069;
                clsHcType.B3.LUNG_RESULT070 = item.LUNG_RESULT070;
                clsHcType.B3.LUNG_RESULT071 = item.LUNG_RESULT071;
                clsHcType.B3.LUNG_RESULT072 = item.LUNG_RESULT072;
                clsHcType.B3.LUNG_RESULT073 = item.LUNG_RESULT073;
                clsHcType.B3.LUNG_RESULT074 = item.LUNG_RESULT074;
                clsHcType.B3.LUNG_RESULT075 = item.LUNG_RESULT075;
                clsHcType.B3.LUNG_RESULT076 = item.LUNG_RESULT076;
                clsHcType.B3.LUNG_RESULT077 = item.LUNG_RESULT077;
                clsHcType.B3.LUNG_RESULT078 = item.LUNG_RESULT078;
                clsHcType.B3.LUNG_PLACE = item.LUNG_PLACE;
                clsHcType.B3.NEW_WOMAN37 = item.NEW_WOMAN37;
                clsHcType.B3.LUNG_RESULT079 = item.LUNG_RESULT079;
                clsHcType.B3.LUNG_RESULT080 = item.LUNG_RESULT080;
                clsHcType.B3.LUNG_SANGDAM1 = item.LUNG_SANGDAM1;
                clsHcType.B3.LUNG_SANGDAM2 = item.LUNG_SANGDAM2;
                clsHcType.B3.LUNG_SANGDAM3 = item.LUNG_SANGDAM3;
                clsHcType.B3.LUNG_SANGDAM4 = item.LUNG_SANGDAM4;
                clsHcType.B3.ROWID = item.RID;
            }
        }

        /// <summary>
        /// 건강보험 구강 판정내역 읽음
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <seealso cref="HcBill.bas> READ_HIC_RES_DENTAL() "/>
        public void READ_HIC_RES_DENTAL(long ArgWRTNO)
        {
            clsHcType.B4_CLEAR();

            HIC_RES_DENTAL item = hicResDentalService.GetItemByWrtno(ArgWRTNO);

            if (!item.IsNullOrEmpty())
            {
                clsHcType.B4.WRTNO = ArgWRTNO;
                clsHcType.B4.USIK1 = item.USIK1;
                clsHcType.B4.USIK2 = item.USIK2;
                clsHcType.B4.USIK3 = item.USIK3;
                clsHcType.B4.USIK4 = item.USIK4;
                clsHcType.B4.USIK5 = item.USIK5;
                clsHcType.B4.USIK6 = item.USIK6;
                clsHcType.B4.GYELSON1 = item.GYELSON1;
                clsHcType.B4.GYELSON2 = item.GYELSON2;
                clsHcType.B4.GYELSON3 = item.GYELSON3;
                clsHcType.B4.CHIJU1 = item.CHIJU1;
                clsHcType.B4.CHIJU2 = item.CHIJU2;
                clsHcType.B4.CHIJU3 = item.CHIJU3;
                clsHcType.B4.CHIJU4 = item.CHIJU4;
                clsHcType.B4.CHIJU5 = item.CHIJU5;
                clsHcType.B4.CHIJU6 = item.CHIJU6;
                clsHcType.B4.CHIJU7 = item.CHIJU7;
                clsHcType.B4.CHIJU8 = item.CHIJU8;
                clsHcType.B4.CHIJU9 = item.CHIJU9;
                clsHcType.B4.CHIJU10 = item.CHIJU10;
                clsHcType.B4.BOCHUL1 = item.BOCHUL1;
                clsHcType.B4.BOCHUL2 = item.BOCHUL2;
                clsHcType.B4.BOCHUL3 = item.BOCHUL3;
                clsHcType.B4.BOCHUL4 = item.BOCHUL4;
                clsHcType.B4.BOCHUL5 = item.BOCHUL5;
                clsHcType.B4.BOCHUL6 = item.BOCHUL6;
                clsHcType.B4.BOCHUL7 = item.BOCHUL7;
                clsHcType.B4.BOCHUL8 = item.BOCHUL8;
                clsHcType.B4.BOCHUL9 = item.BOCHUL9;
                clsHcType.B4.BOCHUL10 = item.BOCHUL10;
                clsHcType.B4.BOCHUL11 = item.BOCHUL11;
                clsHcType.B4.BOCHUL12 = item.BOCHUL12;
                clsHcType.B4.OPDDNT = item.OPDDNT;
                clsHcType.B4.SCALING = item.SCALING;
                clsHcType.B4.DNTSTATUS = item.DNTSTATUS;
                clsHcType.B4.FOOD1 = item.FOOD1;
                clsHcType.B4.FOOD2 = item.FOOD2;
                clsHcType.B4.FOOD3 = item.FOOD3;
                clsHcType.B4.BRUSH11 = item.BRUSH11;
                clsHcType.B4.BRUSH12 = item.BRUSH12;
                clsHcType.B4.BRUSH13 = item.BRUSH13;
                clsHcType.B4.BRUSH14 = item.BRUSH14;
                clsHcType.B4.BRUSH15 = item.BRUSH15;
                clsHcType.B4.BRUSH16 = item.BRUSH16;
                clsHcType.B4.BRUSH21 = item.BRUSH21;
                clsHcType.B4.JUNGSANG1 = item.JUNGSANG1;
                clsHcType.B4.JUNGSANG2 = item.JUNGSANG2;
                clsHcType.B4.JUNGSANG3 = item.JUNGSANG3;
                clsHcType.B4.JUNGSANG4 = item.JUNGSANG4;
                clsHcType.B4.JUNGSANG5 = item.JUNGSANG5;
                clsHcType.B4.JUNGSANG6 = item.JUNGSANG6;
                clsHcType.B4.JUNGSANG7 = item.JUNGSANG7;
                clsHcType.B4.MUNJINETC = item.MUNJINETC;
                clsHcType.B4.PANJENG1 = item.PANJENG1;
                clsHcType.B4.PANJENG2 = item.PANJENG2;
                clsHcType.B4.PANJENG3 = item.PANJENG3;
                clsHcType.B4.PANJENG4 = item.PANJENG4;
                clsHcType.B4.PANJENG5 = item.PANJENG5;
                clsHcType.B4.PANJENG6 = item.PANJENG6;
                clsHcType.B4.PANJENG7 = item.PANJENG7;
                clsHcType.B4.PANJENG8 = item.PANJENG8;
                clsHcType.B4.PANJENG9 = item.PANJENG9;
                clsHcType.B4.PANJENG10 = item.PANJENG10;
                clsHcType.B4.PANJENG11 = item.PANJENG11;
                clsHcType.B4.PANJENG12 = item.PANJENG12;
                clsHcType.B4.PanjengDate = item.PANJENGDATE;
                clsHcType.B4.TongboGbn = item.TONGBOGBN;
                clsHcType.B4.TongboDate = item.TONGBODATE;
                clsHcType.B4.PanjengDrNo = item.PANJENGDRNO;
                clsHcType.B4.MIRNO = item.MIRNO;
                clsHcType.B4.PANJENG13 = item.PANJENG13;
                clsHcType.B4.MIRYN = item.MIRYN;
                clsHcType.B4.T_HABIT1 = item.T_HABIT1;
                clsHcType.B4.T_HABIT2 = item.T_HABIT2;
                clsHcType.B4.T_HABIT3 = item.T_HABIT3;
                clsHcType.B4.T_HABIT4 = item.T_HABIT4;
                clsHcType.B4.T_HABIT5 = item.T_HABIT5;
                clsHcType.B4.T_HABIT6 = item.T_HABIT6;
                clsHcType.B4.T_HABIT7 = item.T_HABIT7;
                clsHcType.B4.T_HABIT8 = item.T_HABIT8;
                clsHcType.B4.T_HABIT9 = item.T_HABIT9;
                clsHcType.B4.T_STAT1 = item.T_STAT1;
                clsHcType.B4.T_STAT2 = item.T_STAT2;
                clsHcType.B4.T_STAT3 = item.T_STAT3;
                clsHcType.B4.T_STAT4 = item.T_STAT4;
                clsHcType.B4.T_STAT5 = item.T_STAT5;
                clsHcType.B4.T_STAT6 = item.T_STAT6;
                clsHcType.B4.T_FUNCTION1 = item.T_FUNCTION1;
                clsHcType.B4.T_FUNCTION2 = item.T_FUNCTION2;
                clsHcType.B4.T_FUNCTION3 = item.T_FUNCTION3;
                clsHcType.B4.T_FUNCTION4 = item.T_FUNCTION4;
                clsHcType.B4.T_FUNCTION5 = item.T_FUNCTION5;
                clsHcType.B4.T_JILBYUNG1 = item.T_JILBYUNG1;
                clsHcType.B4.T_JILBYUNG2 = item.T_JILBYUNG2;
                clsHcType.B4.T_PAN1 = item.T_PAN1;
                clsHcType.B4.T_PAN2 = item.T_PAN2;
                clsHcType.B4.T_PAN3 = item.T_PAN3;
                clsHcType.B4.T_PAN4 = item.T_PAN4;
                clsHcType.B4.T_PAN5 = item.T_PAN5;
                clsHcType.B4.T_PAN6 = item.T_PAN6;
                clsHcType.B4.T_PAN7 = item.T_PAN7;
                clsHcType.B4.T_PAN8 = item.T_PAN8;
                clsHcType.B4.T_PAN9 = item.T_PAN9;
                clsHcType.B4.T_PAN10 = item.T_PAN10;
                clsHcType.B4.T_PAN11 = item.T_PAN11;
                clsHcType.B4.T_PAN_ETC = item.T_PAN_ETC;
                clsHcType.B4.T40_PAN1 = item.T40_PAN1;
                clsHcType.B4.T40_PAN2 = item.T40_PAN2;
                clsHcType.B4.T40_PAN3 = item.T40_PAN3;
                clsHcType.B4.T40_PAN4 = item.T40_PAN4;
                clsHcType.B4.T40_PAN5 = item.T40_PAN5;
                clsHcType.B4.T40_PAN6 = item.T40_PAN6;
                clsHcType.B4.T_PANJENG1 = item.T_PANJENG1;
                clsHcType.B4.T_PANJENG2 = item.T_PANJENG2;
                clsHcType.B4.T_PANJENG3 = item.T_PANJENG3;
                clsHcType.B4.T_PANJENG4 = item.T_PANJENG4;
                clsHcType.B4.T_PANJENG5 = item.T_PANJENG5;
                clsHcType.B4.T_PANJENG6 = item.T_PANJENG6;
                clsHcType.B4.T_PANJENG7 = item.T_PANJENG7;
                clsHcType.B4.T_PANJENG8 = item.T_PANJENG8;
                clsHcType.B4.T_PANJENG9 = item.T_PANJENG9;
                clsHcType.B4.T_PANJENG10 = item.T_PANJENG10;
                clsHcType.B4.T_PANJENG_ETC = item.T_PANJENG_ETC;
                clsHcType.B4.T_PANJENG_SOGEN = item.T_PANJENG_SOGEN;
                clsHcType.B4.SANGDAM = item.SANGDAM;
                clsHcType.B4.RES_MUNJIN = item.RES_MUNJIN;
                clsHcType.B4.RES_JOCHI = item.RES_JOCHI;
                clsHcType.B4.RES_RESULT = item.RES_RESULT;
                clsHcType.B4.ROWID = item.RID;
            }
        }

        /// <summary>
        /// 건강보험 특수검진 판정내역 읽음
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <seealso cref="HcBill.bas> READ_HIC_RES_SPECIA() "/>
        public void READ_HIC_RES_SPECIAL(long ArgWRTNO)
        {
            clsHcType.B5_CLEAR();
            HIC_RES_SPECIAL item = hicResSpecialService.GetItemByWrtno(ArgWRTNO);
            if (!item.IsNullOrEmpty())
            {
                clsHcType.B5.WRTNO = item.WRTNO;
                clsHcType.B5.Jikjong = item.JIKJONG;
                clsHcType.B5.BuseName = item.BUSE;
                clsHcType.B5.GONGJENG = item.GONGJENG;
                clsHcType.B5.ROWID = item.RID;
            }
        }

        public string READ_Res_Pan(string ArgPan)
        {
            string rtnVal = "";

            switch (ArgPan)
            {
                case "1":
                    rtnVal = "정상";
                    break;
                case "2":
                    rtnVal = "정상B";
                    break;
                case "3":
                    rtnVal = "건강주의";
                    break;
                case "4":
                    rtnVal = "유질환";
                    break;
                default:
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 사후관리
        /// </summary>
        /// <param name="ArgCODE"></param>
        /// <seealso cref="HcBill.bas> READ_SAHU() "/>
        public string READ_SAHU(string ArgCODE)
        {
            string rtnVal = string.Empty;

            HIC_CODE item = hicCodeService.GetItembyGubunCode2("12", ArgCODE);

            if (!item.IsNullOrEmpty())
            {
                rtnVal = item.NAME;
            }
            return rtnVal;
        }

        /// <summary>
        /// JUMIN확인
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <seealso cref="HcBill.bas> READ_HIC_PATIENT_JUMIN() "/>
        public long READ_HIC_PATIENT_JUMIN(long ArgWRTNO)
        {
            long rtnVal = 0;

            HIC_JEPSU_PATIENT item = hicJepsuPatientService.GetItembyWrtNo(ArgWRTNO);

            if (!item.IsNullOrEmpty())
            {
                rtnVal = Convert.ToInt32(item.JUMIN);
            }
            return rtnVal;
        }

        /// <summary>
        /// 접수마스터의 나이를 읽음
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <seealso cref="HcBill.bas> READ_PATIENT_AGE() "/>
        /// TODO : HIC_JEPSU 의 Age 컬럼 이용하는게 맞음.
        public long READ_PATIENT_AGE(long ArgWrtno)
        {
            long rtnVal = 0;

            long nAge = hicJepsuService.GetAgeByWrtno(ArgWrtno);

            if (nAge > 0)
            {
                rtnVal = nAge;
            }
            else
            {
                rtnVal = 0;
            }

            return rtnVal;
        }

        public string HIC_Cancer_Bill_Jong(long ArgWRTNO)
        {
            string rtnVal = "";

            HIC_JEPSU item = hicJepsuService.GetMuryoAmGubDaeSangbyWrtNo(ArgWRTNO);

            if (!item.IsNullOrEmpty())
            {
                if (item.MURYOAM.Trim() == "Y")
                {
                    rtnVal = "2";   //무료암
                }

                if (item.GUBDAESANG.Trim() == "Y")
                {
                    rtnVal = "3";   //의료급여암
                }

                if (rtnVal == "")
                {
                    rtnVal = "1";   //특정암
                }
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
        }

        /// <summary>
        /// 건진의사코드로 성명 찾기
        /// </summary>
        /// <param name="ArgCODE"></param>
        /// <seealso cref="HcBill.bas> READ_MIR_DRNAME() "/>
        public string READ_MIR_DRNAME(string ArgCODE)
        {
            string rtnVal = string.Empty;

            HIC_CODE item = hicCodeService.GetItembyGubunCode2("30", ArgCODE);

            if (!item.IsNullOrEmpty())
            {
                rtnVal = item.NAME.Trim();
            }
            return rtnVal;
        }

        /// <summary>
        /// 건진의사코드로 주민번호 찾기
        /// </summary>
        /// <param name="ArgCODE"></param>
        /// <seealso cref="HcBill.bas> READ_MIR_DRJUMIN() "/>
        public string READ_MIR_DRJUMIN(string ArgCODE)
        {
            string rtnVal = string.Empty;

            HIC_CODE item = hicCodeService.GetItembyGubunCode2("30", ArgCODE);

            if (!item.IsNullOrEmpty())
            {
                rtnVal = item.GCODE1.Trim();
            }
            return rtnVal;
        }

        /// <summary>
        /// 건진의사코드로 면허번호 찾기
        /// </summary>
        /// <param name="ArgCODE"></param>
        /// <seealso cref="HcBill.bas> READ_MIR_DRNO1() "/>
        public string READ_MIR_DRNO1(string ArgCODE)
        {
            string rtnVal = string.Empty;

            HIC_CODE item = hicCodeService.GetItembyGubunCode2("30", ArgCODE);

            if (!item.IsNullOrEmpty())
            {
                rtnVal = item.GCODE;
            }
            return rtnVal;
        }

        /// <summary>
        /// 건진의사사번으로 성명 찾기
        /// </summary>
        /// <param name="ArgCODE"></param>
        /// <seealso cref="HcBill.bas> READ_MIR_DRNAME1() "/>
        public string READ_MIR_DRNAME1(string ArgCODE)
        {
            string rtnVal = string.Empty;

            HIC_CODE item = hicCodeService.GetItembyGubunCode2("30", ArgCODE);

            if (!item.IsNullOrEmpty())
            {
                rtnVal = item.NAME;
            }
            return rtnVal;
        }

        /// <summary>
        /// 건진의사사번으로 주민번호 찾기
        /// </summary>
        /// <param name="ArgCODE"></param>
        /// <seealso cref="HcBill.bas> READ_MIR_DRJUMIN1() "/>
        public string READ_MIR_DRJUMIN1(string ArgCODE)
        {
            string rtnVal = string.Empty;

            HIC_CODE item = hicCodeService.GetItembyGubunCode2("30", ArgCODE);

            if (!item.IsNullOrEmpty())
            {
                rtnVal = item.GCODE1;
            }
            return rtnVal;
        }

        /// <summary>
        /// 결진결과읽기1
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <param name="ArgExCode"></param>
        /// <param name="ArgPP"></param>
        /// <param name="ArgGBN"></param>
        public string READ_Mir_Result1(long ArgWRTNO, string ArgExCode, long ArgPP, string ArgGBN)
        {
            string rtnVal = string.Empty;

            HIC_RESULT item = hicResultService.GetExCodebyWrtNo_RESULT1(ArgWRTNO, ArgExCode, ArgPP, ArgGBN);

            if (!item.IsNullOrEmpty())
            {
                rtnVal = item.RESULT.Trim();
            }

            if (rtnVal == "")
            {
                for (int i = 1; i <= ArgPP; i++)
                {
                    rtnVal += "0";
                }
                switch (ArgGBN)
                {
                    case "O":
                        rtnVal = rtnVal.Replace("0", "0");
                        break;
                    case "N":
                        rtnVal = rtnVal.Replace("0", "N");
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// 결진결과읽기2
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <seealso cref="HcBill.bas> READ_MIR_RESULT2() "/>
        public string READ_MIR_RESULT2(long ArgWRTNO, string ArgExCode)
        {
            string rtnVal = string.Empty;

            HIC_RESULT item = hicResultService.GetExCodebyWrtNo_RESULT2(ArgWRTNO, ArgExCode);

            if (!item.IsNullOrEmpty())
            {
                rtnVal = item.RESULT;
            }
            return rtnVal;
        }

        /// <summary>
        /// 결진결과읽기
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <seealso cref="Hcfile.exe > HcBill.bas > READ_MIR_RESULT2() "/>
        /// 
        public string READ_MIR_RESULT3(long ArgWRTNO, string ArgExCode)
        {
            string rtnVal = string.Empty;

            HIC_RESULT item = hicResultService.GetExCodebyWrtNo_RESULT3(ArgWRTNO, ArgExCode);

            if (!item.IsNullOrEmpty())
            {
                rtnVal = item.RESULT;
            }
            return rtnVal;
        }

        /// <summary>
        /// 검진결과읽기 B형간염
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <seealso cref="HcBill.bas> READ_MIR_RESULT_B형간염() "/>
        public string READ_MIR_RESULT_BHhepatitis(long ArgWRTNO, string ArgPTNO, string ArgExCode)
        {
            string rtnVal = string.Empty;
            string strResult = "";
            string strMSCode = "";

            switch (ArgExCode)
            {
                case "A258":
                    strMSCode = "SI07A";
                    break;
                case "A259":
                    strMSCode = "SI072A";
                    break;
                default:
                    break;
            }

            strResult = examSpecmstService.GetResultbyPtNoMsCode(ArgPTNO, strMSCode);

            switch (ArgExCode)
            {
                case "A258":
                    strResult = VB.Pstr(VB.Pstr(strResult, "S/CO", 1), "(", 2).Trim();
                    break;
                case "A259":
                    strResult = VB.Pstr(VB.Pstr(strResult, "mIU", 1), "(", 2).Trim();
                    break;
                default:
                    strResult = "0.0";
                    break;
            }

            if (VB.Right(strResult, 1) == ")")
            {
                strResult = VB.Left(strResult, strResult.Length - 1);
            }

            rtnVal = strResult;

            if (strResult == "0")
            {
                rtnVal = "0.0";
            }

            return rtnVal;
        }

        /// <summary>
        /// 검진코드유무체크
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <seealso cref="HcBill.bas> READ_MIR_검사코드유무() "/>
        public string READ_MIR_ExcodeYN(long ArgWRTNO, string ArgExCode, string ArgGUBUN)
        {
            string rtnVal = string.Empty;

            HIC_RESULT item = hicResultService.GetExCodebyWrtNo_ExcodeYN(ArgWRTNO, ArgExCode, "1");

            rtnVal = "N";
            if (!item.IsNullOrEmpty())
            {
                rtnVal = "Y";
            }

            return rtnVal;
        }

        /// <summary>
        /// 검진코드유무체크2
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <seealso cref="HcBill.bas> READ_MIR_검사코드유무2() "/>
        public string GetExCodebyWrtNo_ExcodeYN2(long ArgWRTNO, string ArgExCode, string ArgGUBUN)
        {
            string rtnVal = string.Empty;

            HIC_RESULT item = hicResultService.GetExCodebyWrtNo_ExcodeYN(ArgWRTNO, ArgExCode, "2");

            rtnVal = "N";
            if (!item.IsNullOrEmpty())
            {
                rtnVal = "Y";
            }

            return rtnVal;
        }

        /// <summary>
        /// 자궁경부암-편평상피세포이상
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <seealso cref="HcBill.bas> READ_WOMB_CHECK() "/>
        public string READ_Womb_Check(long ArgWRTNO)
        {
            string rtnVal = string.Empty;
            string strGbCheck = "";

            strGbCheck = comHpcLibBService.GetExamAnataMst(ArgWRTNO);

            if (strGbCheck != "0")
            {
                rtnVal = strGbCheck.Trim();
            }
            return rtnVal;
        }

        /// <summary>
        /// 검진결과읽기
        /// </summary>
        /// <param name="ArgMirno"></param>
        /// <returns></returns>
        /// <seealso cref="HcBill.bas> READ_HIC_보건소읽기() "/>
        public string READ_HIC_BOGUNSO(long ArgMirno)
        {
            string rtnVal = "";

            rtnVal = hicJepsuService.GetNamebyMirNo(ArgMirno);

            return rtnVal;
        }

        /// <summary>
        /// 객담세포병리검사만 2차에 있는지 확인
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <returns></returns>
        /// <seealso cref="HcBill.bas> READ_HIC_객담세포병리검사2차() "/>
        public bool READ_HIC_GuadamcellPathologyTest2th(long ArgWRTNO)
        {
            bool rtnVal = false;

            if (hicSunapdtlService.GetCount(ArgWRTNO) > 0)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        /// <summary>
        /// 검진종류로 생애 구분
        /// </summary>
        /// <param name="ArgGjJong"></param>
        /// <returns></returns>
        /// <seealso cref="HcBill.bas> READ_HIC_검진종류구분() "/>
        public bool READ_HIC_HcKindGubun(string ArgGjJong)
        {
            bool rtnVal = false;

            if (hicExjongService.GetCount(ArgGjJong) > 0)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        /// <summary>
        /// 건진청구에 사용  검진종류  세팅 함수 
        /// ※ 쿼리에서 GjJong IN () 으로 사용하기 위해 함수를 List<string> 으로 변경 (2020.12.31)
        /// </summary>
        /// <param name="ArgJik"></param>
        /// <param name="ArgChasu"></param>
        /// <param name="ArgLife"></param>
        /// <returns></returns>
        /// <seealso cref="HcBill.bas> READ_HIC_검진종류세팅() "/>
        public List<string> READ_HIC_HcKindSetting_List(string ArgJik, string ArgChasu, string ArgLife)
        {
            List<string> rtnVal = new List<string>();

            rtnVal.Clear();

            switch (ArgJik)
            {
                case "사업장":
                    switch (ArgChasu)
                    {
                        case "0":
                            switch (ArgLife)
                            {
                                case "0":
                                    //rtnVal = "'11','16'";
                                    rtnVal.Add("11");
                                    rtnVal.Add("16");
                                    break;
                                case "1":
                                    //rtnVal = "'41','44'";
                                    rtnVal.Add("41");
                                    rtnVal.Add("44");
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "1":
                            switch (ArgLife)
                            {
                                case "0":
                                    //rtnVal = "'11'";
                                    rtnVal.Add("11");
                                    break;
                                case "1":
                                    //rtnVal = "'41'";
                                    rtnVal.Add("41");                                    
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "2":
                            switch (ArgLife)
                            {
                                case "0":
                                    //rtnVal = "'16'";
                                    rtnVal.Add("16");
                                    break;
                                case "1":
                                    //rtnVal = "'44'";
                                    rtnVal.Add("44");
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "공무원":
                    switch (ArgChasu)
                    {
                        case "0":
                            switch (ArgLife)
                            {
                                case "0":
                                    //rtnVal = "'21'";
                                    rtnVal.Add("21");
                                    break;
                                case "1":
                                    //rtnVal = "'42','45'";
                                    rtnVal.Add("42");
                                    rtnVal.Add("45");
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "1":
                            switch (ArgLife)
                            {
                                case "0":
                                    //rtnVal = "'21'";
                                    rtnVal.Add("21");
                                    break;
                                case "1":
                                    //rtnVal = "'42'";
                                    rtnVal.Add("42");
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "2":
                            switch (ArgLife)
                            {
                                case "0":
                                    //rtnVal = "'21'";
                                    rtnVal.Add("21");
                                    break;
                                case "1":
                                    //rtnVal = "'45'";
                                    rtnVal.Add("45");
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "성인병":
                    switch (ArgChasu)
                    {
                        case "0":
                            switch (ArgLife)
                            {
                                case "0":
                                    //rtnVal = "'13'";
                                    rtnVal.Add("13");
                                    break;
                                case "1":
                                    //rtnVal = "'43','46'";
                                    rtnVal.Add("43");
                                    rtnVal.Add("46");
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "1":
                            switch (ArgLife)
                            {
                                case "0":
                                    //rtnVal = "'11'";
                                    rtnVal.Add("11");
                                    break;
                                case "1":
                                    //rtnVal = "'43'";
                                    rtnVal.Add("43");
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "2":
                            switch (ArgLife)
                            {
                                case "0":
                                    //rtnVal = "'11'";
                                    rtnVal.Add("11");
                                    break;
                                case "1":
                                    //rtnVal = "'46'";
                                    rtnVal.Add("46");
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "통합":
                    switch (ArgChasu)
                    {
                        case "0":
                            switch (ArgLife)
                            {
                                case "0":
                                    //rtnVal = "11','12','13','16','17','18'";
                                    rtnVal.Add("11");
                                    rtnVal.Add("12");
                                    rtnVal.Add("13");
                                    rtnVal.Add("16");
                                    rtnVal.Add("17");
                                    rtnVal.Add("18");
                                    break;
                                case "1":
                                    //rtnVal = "'41','42','43','44','45','46'";
                                    rtnVal.Add("41");
                                    rtnVal.Add("42");
                                    rtnVal.Add("43");
                                    rtnVal.Add("44");
                                    rtnVal.Add("45");
                                    rtnVal.Add("46");
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "1":
                            switch (ArgLife)
                            {
                                case "0":
                                    //rtnVal = "'11','12','13'";
                                    rtnVal.Add("11");
                                    rtnVal.Add("12");
                                    rtnVal.Add("13");
                                    break;
                                case "1":
                                    //rtnVal = "'41','42','43'";
                                    rtnVal.Add("41");
                                    rtnVal.Add("42");
                                    rtnVal.Add("43");
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "2":
                            switch (ArgLife)
                            {
                                case "0":
                                    //rtnVal = "'16','17','18'";
                                    rtnVal.Add("16");
                                    rtnVal.Add("17");
                                    rtnVal.Add("18");
                                    break;
                                case "1":
                                    //rtnVal = "'44','45','46'";
                                    rtnVal.Add("44");
                                    rtnVal.Add("45");
                                    rtnVal.Add("46");
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

        /// <summary>
        /// 건진청구에 사용  검진종류  세팅 함수 
        /// </summary>
        /// <param name="ArgJik"></param>
        /// <param name="ArgChasu"></param>
        /// <param name="ArgLife"></param>
        /// <returns></returns>
        /// <seealso cref="HcBill.bas> READ_HIC_검진종류세팅() "/>
        public string READ_HIC_HcKindSetting(string ArgJik, string ArgChasu, string ArgLife)
        {
            string rtnVal = "";

            switch (ArgJik)
            {
                case "사업장":
                    switch (ArgChasu)
                    {
                        case "0":
                            switch (ArgLife)
                            {
                                case "0":
                                    rtnVal = "'11','16'";
                                    break;
                                case "1":
                                    rtnVal = "'41','44'";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "1":
                            switch (ArgLife)
                            {
                                case "0":
                                    rtnVal = "'11'";
                                    break;
                                case "1":
                                    rtnVal = "'41'";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "2":
                            switch (ArgLife)
                            {
                                case "0":
                                    rtnVal = "'16'";
                                    break;
                                case "1":
                                    rtnVal = "'44'";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "공무원":
                    switch (ArgChasu)
                    {
                        case "0":
                            switch (ArgLife)
                            {
                                case "0":
                                    rtnVal = "'21'";
                                    break;
                                case "1":
                                    rtnVal = "'42','45'";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "1":
                            switch (ArgLife)
                            {
                                case "0":
                                    rtnVal = "'21'";
                                    break;
                                case "1":
                                    rtnVal = "'42'";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "2":
                            switch (ArgLife)
                            {
                                case "0":
                                    rtnVal = "'21'";
                                    break;
                                case "1":
                                    rtnVal = "'45'";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "성인병":
                    switch (ArgChasu)
                    {
                        case "0":
                            switch (ArgLife)
                            {
                                case "0":
                                    rtnVal = "'13'";
                                    break;
                                case "1":
                                    rtnVal = "'43','46'";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "1":
                            switch (ArgLife)
                            {
                                case "0":
                                    rtnVal = "'11'";
                                    break;
                                case "1":
                                    rtnVal = "'43'";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "2":
                            switch (ArgLife)
                            {
                                case "0":
                                    rtnVal = "'11'";
                                    break;
                                case "1":
                                    rtnVal = "'46'";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "통합":
                    switch (ArgChasu)
                    {
                        case "0":
                            switch (ArgLife)
                            {
                                case "0":
                                    rtnVal = "11','12','13','16','17','18'";
                                    break;
                                case "1":
                                    rtnVal = "'41','42','43','44','45','46'";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "1":
                            switch (ArgLife)
                            {
                                case "0":
                                    rtnVal = "'11','12','13'";
                                    break;
                                case "1":
                                    rtnVal = "'41','42','43'";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "2":
                            switch (ArgLife)
                            {
                                case "0":
                                    rtnVal = "'16','17','18'";
                                    break;
                                case "1":
                                    rtnVal = "'44','45','46'";
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgChasu"></param>
        /// <param name="ArgWRTNO"></param>
        /// <returns></returns>
        /// <seealso cref="HcBill.bas> READ_HIC_중복판정점검() "/>
        public bool READ_HIC_DuplicatejudgmentCheck(string ArgGubun, string ArgChasu, long ArgWRTNO)
        {
            bool rtnVal = false;
            int nCnt = 0;

            switch (ArgGubun)
            {
                case "구강":
                    nCnt = hicResDentalService.GetCountbyWrtNo(ArgWRTNO);
                    break;
                case "일반":
                    if (ArgChasu == "1")
                    {
                        nCnt = hicResBohum1Service.GetCountbyWrtNo(ArgWRTNO);
                    }
                    else
                    {
                        nCnt = hicResBohum2Service.GetCountDoublebyWrtNo(ArgWRTNO);
                    }
                    break;
                case "암":
                    nCnt = hicCancerNewService.GetCountbyWrtNo(ArgWRTNO);
                    break;
                default:
                    break;
            }

            if (nCnt > 1)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        /// <summary>
        /// 생애1차판정읽기
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <returns></returns>
        /// <seealso cref="HcBill.bas> READ_HIC_생애1차판정읽기() "/>
        public string READ_HIC_Life1thjudgment(long ArgWRTNO)
        {
            string rtnVal = "";
            string strSogen = "";

            strSogen = hicResBohum1Service.GetCountLife1thbyWrtNo(ArgWRTNO);

            if (!strSogen.IsNullOrEmpty())
            {
                rtnVal = strSogen;
            }
            return rtnVal;
        }

        /// <summary>
        /// 공단부담체크
        /// </summary>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgSQL"></param>
        /// <returns></returns>
        /// <seealso cref="HcBill.bas> READ_HIC_MIR_공단부담체크() "/>
        public string READ_HIC_MIR_HiraBurdenCheckstring(string ArgGubun, string ArgSQL)
        {
            string rtnVal = "";
            long nTempi = 0;
            long nTempRead = 0;
            string strTempSQL = "";
            string strTempSql2 = "";

            strTempSQL = "SELECT WRTNO FROM " + VB.Pstr(ArgSQL, "FROM", 2);

            strTempSql2 = "";
            strTempSql2 = " SELECT WRTNO " + "\r\n";
            strTempSql2 += " FROM ADMIN.HIC_SUNAPDTL " + "\r\n";
            strTempSql2 += "   WHERE WRTNO IN (" + strTempSQL + " ) " + "\r\n";
            switch (ArgGubun)
            {
                case "1":   //구강
                    strTempSql2 += "    AND Code IN ( SELECT Code FROM ADMIN.HIC_CODE WHERE GUBUN ='M1' AND GCODE ='001' ) " + "\r\n";
                    break;
                default:
                    break;
            }
            strTempSql2 += "   AND RTRIM(GBSELF) NOT IN ( '1','01') " + "\r\n"; //조합부담100% 아닌것
            strTempSql2 += " GROUP BY WRTNO " + "\r\n";

            rtnVal = strTempSql2;

            return rtnVal;
        }

        /// <summary>
        /// 급여항목체크
        /// </summary>
        /// <param name="ArgWRTNO"></param>
        /// <returns></returns>
        /// <seealso cref="HcBill.bas> READ_HIC_급여항목체크() "/>
        public string READ_HIC_insuranceItemCheck(long ArgWRTNO)
        {
            string rtnVal = "";

            if (hicSunapdtlService.GetCountbyWrtNo(ArgWRTNO) > 0)
            {
                rtnVal = "OK";
            }
            return rtnVal;
        }

        /// <summary>
        /// 결과활용동의여부
        /// </summary>
        /// <param name="argPTNO"></param>
        /// <param name="ArgYEAR"></param>
        /// <returns></returns>
        /// <seealso cref="HcBill.bas> READ_결과활용동의여부() "/>
        public string READ_ResultApplicationAgreeWhether(string argPTNO, string ArgYEAR)
        {
            string rtnVal = "";

            rtnVal = comHpcLibBService.GetEntDatebyHicPrivacyAccept(argPTNO, ArgYEAR);
            
            return rtnVal;
        }

        /// <summary>
        /// 한글이 포함된 문자열을 지정한 글자수 만큼 자르는 작업
        /// 한글 2Byte중 1Byte만 짤리는 오류를 방지하기 위해 사용함
        /// </summary>
        /// <param name="ArgStr"></param>
        /// <param name="ArgLen"></param>
        /// <returns></returns>
        public string HIC_STRCUTL(string ArgStr, long ArgLen)
        {
            string rtnVal = "";
            long nStrLen = 0;
            string strResult = "";
            string strChar = "";

            //자료가 없으면 NULL을 Return
            if (ArgStr == "")
            {
                rtnVal = "";
                return rtnVal;
            }

            strResult = "";
            nStrLen = ArgStr.Length;
            for (int i = 0; i < nStrLen; i++)
            {
                strChar = VB.Mid(ArgStr, i, 1);

                //건진 청구시 줄바꿈을 방지하기 위해 CRLF를 제거함
                if (strChar == "\r\n") strChar = "";
                if (strChar == "\n") strChar = "";
                if (strChar == "\r") strChar = "";
                if (strChar == Environment.NewLine) strChar = "";

                //한글 짤림을 방지하기 위해 마지막 글자가 한글이고 짤리면 마지막 글자를 무시함
                if ((strResult + strChar).Length > ArgLen)
                {
                    rtnVal = strResult;
                    return rtnVal;
                }
                strResult += strChar;
            }

            rtnVal = strResult;

            return rtnVal;
        }
    }
}
