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
    public class HeaSunapdtlService
    {        
        private HeaSunapdtlRepository heaSunapdtlRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaSunapdtlService()
        {
			this.heaSunapdtlRepository = new HeaSunapdtlRepository();
        }

        public List<HEA_SUNAPDTL> Read_YName(long wrtno)
        {
            return heaSunapdtlRepository.Read_YName(wrtno);
        }

        public List<HEA_SUNAPDTL> GetNamebyWrtNo(long nWRTNO)
        {
            return heaSunapdtlRepository.GetNamebyWrtNo(nWRTNO);
        }

        public string CheckVipByWRTNOLikeCodeName(long nWRTNO)
        {
            return heaSunapdtlRepository.CheckVipByWRTNOLikeCodeName(nWRTNO);
        }

        public long GetSumAmtByWRTNO(long nWRTNO)
        {
            return heaSunapdtlRepository.GetSumAmtByWRTNO(nWRTNO);
        }

        public List<HEA_SUNAPDTL> GetListByWRTNO(long nWRTNO)
        {
            return heaSunapdtlRepository.GetListByWRTNO(nWRTNO);
        }

        public List<HEA_SUNAPDTL> GetSunapListByWrtno(long nWRTNO)
        {
            return heaSunapdtlRepository.GetSunapListByWrtno(nWRTNO);
        }

        public string GetCodeTypeByWrtno(long nWRTNO)
        {
            return heaSunapdtlRepository.GetCodeTypeByWrtno(nWRTNO);
        }

        public string GetRowidByWrtnoCodeIN(long nWRTNO, List<string> lstExCodes)
        {
            return heaSunapdtlRepository.GetRowidByWrtnoCodeIN(nWRTNO, lstExCodes);
        }

        public int GetCountbyWrtNo(long argWrtno)
        {
            return heaSunapdtlRepository.GetCountbyWrtNo(argWrtno);
        }

        public void DeleteAllByWrtno(long argWrtno)
        {
            heaSunapdtlRepository.DeleteAllByWrtno(argWrtno);
        }

        public void InsertData(long argWrtno, READ_SUNAP_ITEM rSuInfo)
        {
            heaSunapdtlRepository.InsertData(argWrtno, rSuInfo);
        }

        public void InsertDataHis(long argWrtno, READ_SUNAP_ITEM rSuInfo)
        {
            heaSunapdtlRepository.InsertDataHis(argWrtno, rSuInfo);
        }

        public string GetMainSunapDtlCodeNameByWrtno(long wRTNO)
        {
            return heaSunapdtlRepository.GetMainSunapDtlCodeNameByWrtno(wRTNO);
        }
    }
}
