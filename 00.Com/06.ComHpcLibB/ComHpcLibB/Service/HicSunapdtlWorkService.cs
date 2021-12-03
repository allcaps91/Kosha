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
    public class HicSunapdtlWorkService
    {
        
        private HicSunapdtlWorkRepository hicSunapdtlWorkRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSunapdtlWorkService()
        {
			this.hicSunapdtlWorkRepository = new HicSunapdtlWorkRepository();
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            return hicSunapdtlWorkRepository.UpdatePaNobyPaNo(argPaNo, argJumin2);
        }

        public int DeletebyPaNoGjJong(long argPaNo, string strJong)
        {
            return hicSunapdtlWorkRepository.DeletebyPaNoGjJong(argPaNo, strJong);
        }

        public int Insert(HIC_SUNAPDTL_WORK item)
        {
            return hicSunapdtlWorkRepository.Insert(item);
        }

        public List<HIC_SUNAPDTL_WORK> GetItembyPaNoSuDateGjJong(string fstrPANO, string argGJepdate, string argGjJong)
        {
            return hicSunapdtlWorkRepository.GetItembyPaNoSuDateGjJong(fstrPANO, argGJepdate, argGjJong);
        }

        public void DeleteAllByPano(long argPano, string argJong)
        {
            hicSunapdtlWorkRepository.DeleteAllByPano(argPano, argJong);
        }

        public void InsertData(long argPano, string argJong, READ_SUNAP_ITEM rSuInfo, string argDate)
        {
            hicSunapdtlWorkRepository.InsertData(argPano, argJong, rSuInfo, argDate);
        }

        public int GetCountSunapDtlWorkbyPanoJong(long argPano, string argJong)
        {
            return hicSunapdtlWorkRepository.GetCountSunapDtlWorkbyPanoJong(argPano, argJong);
        }

        public int GetCountbyPaNo(string argPaNo)
        {
            return hicSunapdtlWorkRepository.GetCountbyPaNo(argPaNo);
        }

        public HIC_SUNAPDTL_WORK GetExCodeHNamebyPaNoSudate(string strPANO, string strJepDate)
        {
            return hicSunapdtlWorkRepository.GetExCodeHNamebyPaNoSudate(strPANO, strJepDate);
        }

        public bool DeletebyPaNoSuDate(HIC_JEPSU_WORK iHJW)
        {
            try
            {
                hicSunapdtlWorkRepository.DeletebyPaNoSuDate(iHJW);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeletebyPaNoSuDate2(HIC_JEPSU_WORK hJW)
        {
            try
            {
                hicSunapdtlWorkRepository.DeletebyPaNoSuDate2(hJW);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
