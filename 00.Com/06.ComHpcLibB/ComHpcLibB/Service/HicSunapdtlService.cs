namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB.Model;
    
    /// <summary>
    /// 
    /// </summary>
    public class HicSunapdtlService
    {        
        private HicSunapdtlRepository hicSunapdtlRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSunapdtlService()
        {
			this.hicSunapdtlRepository = new HicSunapdtlRepository();
        }

        
        public List<HIC_SUNAPDTL> Read_UCode(string strPtno, string strJepDate)
        {
            return hicSunapdtlRepository.Read_UCode(strPtno, strJepDate);
        }

        public List<HIC_SUNAPDTL> Read_GbSelf(long argWrtNo)
        {
            return hicSunapdtlRepository.Read_GbSelf(argWrtNo);
        }

        public HIC_SUNAPDTL GetSunapDtlbyWrtNo(long nWrtNo, List<string> strCode)
        {
            return hicSunapdtlRepository.GetSunapDtlbyWrtNo(nWrtNo, strCode);
        }

        public List<HIC_SUNAPDTL> GetNamebyWrtNo(long nWRTNO)
        {
            return hicSunapdtlRepository.GetNamebyWrtNo(nWRTNO);
        }

        public int GetCount(long argWRTNO)
        {
            return hicSunapdtlRepository.GetCount(argWRTNO);
        }

        public int GetCountbyWrtNo(long argWRTNO)
        {
            return hicSunapdtlRepository.GetCountbyWrtNo(argWRTNO);
        }

        public int GetRowIdbyWrtNo(long nWRTNO, string sCode)
        {
            return hicSunapdtlRepository.GetRowIdbyWrtNo(nWRTNO, sCode);
        }

        public int GetCountbyWrtNoCode(long fnWRTNO, string strCode)
        {
            return hicSunapdtlRepository.GetCountbyWrtNoCode(fnWRTNO, strCode);
        }

        public List<HIC_SUNAPDTL> GetAllbyWrtNo(long fnWrtNo)
        {
            return hicSunapdtlRepository.GetAllbyWrtNo(fnWrtNo);
        }

        public HIC_SUNAPDTL GetCodebyWrtNo(long fnWrtNo, string strGbn)
        {
            return hicSunapdtlRepository.GetCodebyWrtNo(fnWrtNo, strGbn);
        }

        public int GetCountbyWrtNOInCode(long argWrtNo, List<string> g36_NIGHT_CODE)
        {
            return hicSunapdtlRepository.GetCountbyWrtNOInCode(argWrtNo, g36_NIGHT_CODE);
        }

        public string GetGbSelfByWrtno(long nWRTNO)
        {
            return hicSunapdtlRepository.GetGbSelfByWrtno(nWRTNO);
        }

        public List<HIC_SUNAPDTL> GetItembyWrtNo(long fnWrtNo)
        {
            return hicSunapdtlRepository.GetItembyWrtNo(fnWrtNo);
        }

        public int UpdateCodebyWrtNoCode(string strNew_Group, long nWrtNo, string strOLD_Group)
        {
            return hicSunapdtlRepository.UpdateCodebyWrtNoCode(strNew_Group, nWrtNo, strOLD_Group);
        }

        public HIC_SUNAPDTL GetRowIdbyWrtNoCode(long wRTNO, List<string> g36_NIGHT_CODE)
        {
            return hicSunapdtlRepository.GetRowIdbyWrtNoCode(wRTNO, g36_NIGHT_CODE);
        }

        public int GetCountbyWrtNoNotInCode(long nWrtNo, List<string> g36_NIGHT_CODE)
        {
            return hicSunapdtlRepository.GetCountbyWrtNoInCode(nWrtNo, g36_NIGHT_CODE);
        }

        public void DeleteAllByWrtno(long argWrtno)
        {
            hicSunapdtlRepository.DeleteAllByWrtno(argWrtno);
        }

        public void InsertData(long argWrtno, READ_SUNAP_ITEM rSuInfo)
        {
            hicSunapdtlRepository.InsertData(argWrtno, rSuInfo);
        }

        public void InsertDataHis(long argWrtno, READ_SUNAP_ITEM rSuInfo, int nSeq)
        {
            hicSunapdtlRepository.InsertDataHis(argWrtno, rSuInfo, nSeq);
        }

        public int GetCountbyWrtnoInHcCode(long argWrtno)
        {
            return hicSunapdtlRepository.GetCountbyWrtnoInHcCode(argWrtno);
        }

        public string GetRowidbyWrtNoCodeIN(long argWrtno, List<string> argCode)
        {
            return hicSunapdtlRepository.GetRowidbyWrtNoCodeIN(argWrtno, argCode);
        }

        public List<HIC_SUNAPDTL> GetGbSangdambyWrtNo(long nWrtNo)
        {
            return hicSunapdtlRepository.GetGbSangdambyWrtNo(nWrtNo);
        }

        public int GetMaxHistorySeqNoByWrtno(long argWrtno)
        {
            return hicSunapdtlRepository.GetMaxHistorySeqNoByWrtno(argWrtno);
        }

        public bool InsertSelectBySunapDtlWork(long wRTNO, HIC_JEPSU_WORK iHJW)
        {
            try
            {
                hicSunapdtlRepository.InsertSelectBySunapDtlWork(wRTNO, iHJW);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool InsertData(HIC_SUNAPDTL item)
        {
            try
            {
                hicSunapdtlRepository.InsertData(item);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool InsertSelectBySunapDtlWork2(long nWRTNO, HIC_JEPSU_WORK hJW)
        {
            try
            {
                hicSunapdtlRepository.InsertSelectBySunapDtlWork2(nWRTNO, hJW);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetCountbyWrtNoInCode(long nWRTNO, string[] strCode)
        {
            return hicSunapdtlRepository.GetCountbyWrtNoInCode(nWRTNO, strCode);
        }

        public HIC_SUNAPDTL GetCount(string strJong, long nMirNo)
        {
            return hicSunapdtlRepository.GetCount(strJong, nMirNo);
        }

        public HIC_SUNAPDTL GetCountbyCodeMirNo(string strCode, string strJong, long nMirNo)
        {
            return hicSunapdtlRepository.GetCountbyCodeMirNo(strCode, strJong, nMirNo);
        }

        public List<HIC_SUNAPDTL> GetCodebyWrtNo(long nLifeWrtNo)
        {
            return hicSunapdtlRepository.GetCodebyWrtNo(nLifeWrtNo);
        }

        public List<HIC_SUNAPDTL> GetCodebyWrtNo(List<long> nLifeWrtNo)
        {
            return hicSunapdtlRepository.GetCodebyWrtNo(nLifeWrtNo);
        }

        public int GetCountbyWrtNoGbSelf(long wRTNO)
        {
            return hicSunapdtlRepository.GetCountbyWrtNoGbSelf(wRTNO);
        }

        public int GetCountbyWrtno(long wRTNO)
        {
            return hicSunapdtlRepository.GetCountbyWrtno(wRTNO);
        }

        public List<HIC_SUNAPDTL> GetWrtNoCodeGbSelfbyWrtNo(long argWRTNO)
        {
            return hicSunapdtlRepository.GetWrtNoCodeGbSelfbyWrtNo(argWRTNO);
        }

        public int GetCountbyWrtNoCodeGbSelf(long argWRTNO, string strCode, List<string> strGbSelf)
        {
            return hicSunapdtlRepository.GetCountbyWrtNoCodeGbSelf(argWRTNO, strCode, strGbSelf);
        }

        public List<HIC_SUNAPDTL> GetCodebyWrtNoCode(long nWRTNO1, List<string> strCodeList1)
        {
            return hicSunapdtlRepository.GetCodebyWrtNoCode(nWRTNO1, strCodeList1);
        }

        public long GetCountbyInWrtNoJepDate(string strGubun, string strFDate, string strTDate, long nMirNo)
        {
            return hicSunapdtlRepository.GetCountbyInWrtNoJepDate(strGubun, strFDate, strTDate, nMirNo);
        }

        public List<HIC_SUNAPDTL> GetDentalList(long nWrtno)
        {
            return hicSunapdtlRepository.GetDentalList(nWrtno);
        }

        public List<HIC_SUNAPDTL> GetAll(long nWrtno)
        {
            return hicSunapdtlRepository.GetAll(nWrtno);
        }
    }
}
