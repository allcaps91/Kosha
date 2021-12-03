namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;

    public class HicResultExCodeService
    {
        private HicResultExCodeRepository hicResultExCodeRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicResultExCodeService()
        {
            this.hicResultExCodeRepository = new HicResultExCodeRepository();
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoOrderbyPanjengPartExCode(long fnWRTNO)
        {
            return hicResultExCodeRepository.GetItembyWrtNoOrderbyPanjengPartExCode(fnWRTNO);
        }

        public int GetCountbyWrtNo(long nWRTNO)
        {
            return hicResultExCodeRepository.GetCountbyWrtNo(nWRTNO);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoNoActing(long fnWRTNO, string strHeaSORT)
        {
            return hicResultExCodeRepository.GetItembyWrtNoNoActing(fnWRTNO, strHeaSORT);
        }

        public List<HIC_RESULT_EXCODE> GetResultbyWrtNo(long nWrtNo)
        {
            return hicResultExCodeRepository.GetResultbyWrtNo(nWrtNo);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoResult(long fnWRTNO, string strGubun = "")
        {
            return hicResultExCodeRepository.GetItembyWrtNoResult(fnWRTNO, strGubun);
        }

        public List<HIC_RESULT_EXCODE> GetItemNoActingbyWrtNo(long fnWRTNO, string strResult)
        {
            return hicResultExCodeRepository.GetItemNoActingbyWrtNo(fnWRTNO, strResult);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoGbn(long fnWRTNO, string argGbn)
        {
            return hicResultExCodeRepository.GetItembyWrtNoGbn(fnWRTNO, argGbn);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoNewExCode(long fnWRTNO, List<string> strNewExCode, string strPanjeng = "")
        {
            return hicResultExCodeRepository.GetItembyWrtNoNewExCode(fnWRTNO, strNewExCode, strPanjeng);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoSogenCode(long fnWRTNO, string argGbn, List<string> strNewExCode, string argJob)
        {
            return hicResultExCodeRepository.GetItembyWrtNoSogenCode(fnWRTNO, argGbn, strNewExCode, argJob);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNo_First(long fnWrtno, string argGbn)
        {
            return hicResultExCodeRepository.GetItembyWrtNo_First(fnWrtno, argGbn);
        }

        public List<HIC_RESULT_EXCODE> GetItembyPaNoGjYear(long nPano, string strGjYear)
        {
            return hicResultExCodeRepository.GetItembyPaNoGjYear(nPano, strGjYear);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoExCode(long nWRTNO)
        {
            return hicResultExCodeRepository.GetItembyWrtNoExCode(nWRTNO);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoExCodeSpc(long fnWRTNO)
        {
            return hicResultExCodeRepository.GetItembyWrtNoExCodeSpc(fnWRTNO);
        }

        public List<HIC_RESULT_EXCODE> GetXrayItembyWrtNoExCode(long fnWRTNO)
        {
            return hicResultExCodeRepository.GetXrayItembyWrtNoExCode(fnWRTNO);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoExCodeNotIn(long nWRTNO)
        {
            return hicResultExCodeRepository.GetItembyWrtNoExCodeNotIn(nWRTNO);
        }

        public List<HIC_RESULT_EXCODE> GetHeaEndoExListByWrtno(long nHeaWrtno)
        {
            return hicResultExCodeRepository.GetHeaEndoExListByWrtno(nHeaWrtno);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNo(long nWrtNo)
        {
            return hicResultExCodeRepository.GetItembyWrtNo(nWrtNo);
        }

        public List<HIC_RESULT_EXCODE> GetItembyOnlyWrtNo(long nWrtNo)
        {
            return hicResultExCodeRepository.GetItembyOnlyWrtNo(nWrtNo);
        }

        public List<HIC_RESULT_EXCODE> GetItembyOnlyWrtNoSort(long nWrtNo)
        {
            return hicResultExCodeRepository.GetItembyOnlyWrtNoSort(nWrtNo);
        }

        public List<HIC_RESULT_EXCODE> GetItemHicHeabyWrtNoOrderbyPanjengPartExCode(string fstrGubun, long fnWrtNo)
        {
            return hicResultExCodeRepository.GetItemHicHeabyWrtNoOrderbyPanjengPartExCode(fstrGubun, fnWrtNo);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoPaNo(long fnWrtno1, long fnWrtno2, long fnWRTNO, long fnPano, string fstrGjChasu)
        {
            return hicResultExCodeRepository.GetItembyWrtNoPaNo(fnWrtno1, fnWrtno2, fnWRTNO, fnPano, fstrGjChasu);
        }

        public HIC_RESULT_EXCODE GetExCodeGroupCodebyWrtNo(long nWRTNO)
        {
            return hicResultExCodeRepository.GetExCodeGroupCodebyWrtNo(nWRTNO);
        }

        public List<HIC_RESULT_EXCODE> GetHicEndoExListByWrtnoIN(List<long> lstHicWrtno)
        {
            return hicResultExCodeRepository.GetHicEndoExListByWrtnoIN(lstHicWrtno);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoPart(long fnWrtno)
        {
            return hicResultExCodeRepository.GetItembyWrtNoPart(fnWrtno);
        }

        public HIC_RESULT_EXCODE GetHicExCodeGroupCodebyWrtNo(long nWRTNO)
        {
            return hicResultExCodeRepository.GetHicExCodeGroupCodebyWrtNo(nWRTNO);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoExCodeCheckNew(long fnWRTNO, List<string> fstrPartExam, string strChkNew)
        {
            return hicResultExCodeRepository.GetItembyWrtNoExCodeCheckNew(fnWRTNO, fstrPartExam, strChkNew);
        }

        public List<HIC_RESULT_EXCODE> GetItemByWrtNoNotInExCode(long fnWRTNO, List<string> fstrExcode)
        {
            return hicResultExCodeRepository.GetItemByWrtNoNotInExCode(fnWRTNO, fstrExcode);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoInExCode(long fnWRTNO, List<string> g37_DOCT_ENTCODE)
        {
            return hicResultExCodeRepository.GetItembyWrtNoInExCode(fnWRTNO, g37_DOCT_ENTCODE);
        }

        public List<HIC_RESULT_EXCODE> GetListByWrtno(long argWrtno, string argBuse)
        {
            return hicResultExCodeRepository.GetListByWrtno(argWrtno, argBuse);
        }

        public List<HIC_RESULT_EXCODE> GetItemCounselbyWrtNo(long fnWRTNO)
        {
            return hicResultExCodeRepository.GetItemCounselbyWrtNo(fnWRTNO);
        }

        public List<HIC_RESULT_EXCODE> GetItemCounselbyWrtNo(List<long> fnWRTNO)
        {
            return hicResultExCodeRepository.GetItemCounselbyWrtNo(fnWRTNO);
        }

        public List<HIC_RESULT_EXCODE> GetItemHeaNoActingbyWrtNo(long fnWRTNO)
        {
            return hicResultExCodeRepository.GetItemHeaNoActingbyWrtNo(fnWRTNO);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoInExCodes(long fnWRTNO, List<string> strNewExCode, string strGubun)
        {
            return hicResultExCodeRepository.GetItembyWrtNoInExCodes(fnWRTNO, strNewExCode, strGubun);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoInExCodePart(long argWrtNo, List<string> strExCode)
        {
            return hicResultExCodeRepository.GetItembyWrtNoInExCodePart(argWrtNo, strExCode);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNo1WrtNo2PaNo(long fnWrtno1, long fnWrtno2, long nPaNo)
        {
            return hicResultExCodeRepository.GetItembyWrtNo1WrtNo2PaNo(fnWrtno1, fnWrtno2, nPaNo);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoPartNot9(long fnWrtNo)
        {
            return hicResultExCodeRepository.GetItembyWrtNoPartNot9(fnWrtNo);
        }

        public int GetCountbyWrtNoNotPart9(long fnWrtNo)
        {
            return hicResultExCodeRepository.GetCountbyWrtNoNotPart9(fnWrtNo);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoNotInPart(long fnWrtNo, string strPart)
        {
            return hicResultExCodeRepository.GetItembyWrtNoNotInPart(fnWrtNo, strPart);
        }

        public List<HIC_RESULT_EXCODE> GetUrineItembyWrtNo(long nWrtNo)
        {
            return hicResultExCodeRepository.GetUrineItembyWrtNo(nWrtNo);
        }

        public List<HIC_RESULT_EXCODE> GetEntPartByWRTNO(long fnWRTNO)
        {
            return hicResultExCodeRepository.GetEntPartByWRTNO(fnWRTNO);
        }

        public List<HIC_RESULT_EXCODE> GetHicEndoExListByWrtno(long argWrtno)
        {
            return hicResultExCodeRepository.GetHicEndoExListByWrtno(argWrtno);
        }

        public List<HIC_RESULT_EXCODE> GetCountbyPaNoJepDateWrtNoEntPart(long pano, string sysdate, long nWrtNo, string eNTPART, string strJong)
        {
            return hicResultExCodeRepository.GetCountbyPaNoJepDateWrtNoEntPart(pano, sysdate, nWrtNo, eNTPART, strJong); 
        }

        public List<HIC_RESULT_EXCODE> GetItemHeabyWrtNo(long nWRTNO)
        {
            return hicResultExCodeRepository.GetItemHeabyWrtNo(nWRTNO);
        }

        public List<HIC_RESULT_EXCODE> GetHNamebyWrtNo(string strWRTNO)
        {
            return hicResultExCodeRepository.GetHNamebyWrtNo(strWRTNO);
        }

        public List<HIC_RESULT_EXCODE> GetHeaEndoExListByWrtnoIN(List<long> lstHeaWrtno)
        {
            return hicResultExCodeRepository.GetHeaEndoExListByWrtnoIN(lstHeaWrtno);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoResultHea(long nWRTNO)
        {
            return hicResultExCodeRepository.GetItembyWrtNoResultHea(nWRTNO);
        }

        public List<HIC_RESULT_EXCODE> GetItemNotEmptybyWrtNo(string strWrtNo, string strGubun = "")
        {
            return hicResultExCodeRepository.GetItemNotEmptybyWrtNo(strWrtNo, strGubun);
        }

        public List<HIC_RESULT_EXCODE> GetItemHeabyWrtNoSort(long argWrtNo)
        {
            return hicResultExCodeRepository.GetItemHeabyWrtNoSort(argWrtNo);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoNotInPartNoResult(long fnWrtNo, string strPart)
        {
            return hicResultExCodeRepository.GetItemHeabyWrtNoSort(fnWrtNo, strPart);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoNotExCode(long fnWrtNo)
        {
            return hicResultExCodeRepository.GetItembyWrtNoNotExCode(fnWrtNo);
        }

        public List<HIC_RESULT_EXCODE> GetItemCounselCanbyWrtNo(long argWrtNo)
        {
            return hicResultExCodeRepository.GetItemCounselCanbyWrtNo(argWrtNo);
        }

        public List<HIC_RESULT_EXCODE> GetCodeNamebyWrtNoNotInExCode(long argWRTNO, List<string> strExCodes)
        {
            return hicResultExCodeRepository.GetCodeNamebyWrtNoNotInExCode(argWRTNO, strExCodes);
        }

        public List<HIC_RESULT_EXCODE> GetExcodeResultListByWrtno(long argWrtno, string argSex)
        {
            return hicResultExCodeRepository.GetExcodeResultListByWrtno(argWrtno, argSex);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNo_Stomach(long nWrtNo)
        {
            return hicResultExCodeRepository.GetItembyWrtNo_Stomach(nWrtNo);
        }

    }
}
