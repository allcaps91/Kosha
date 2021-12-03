namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSunapWorkService
    {
        
        private HicSunapWorkRepository hicSunapWorkRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSunapWorkService()
        {
			this.hicSunapWorkRepository = new HicSunapWorkRepository();
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            return hicSunapWorkRepository.UpdatePaNobyPaNo(argPaNo, argJumin2);
        }

        public int DeletebyPaNoGjJong(long argPaNo, string argGjjong)
        {
            return hicSunapWorkRepository.DeletebyPaNoGjJong(argPaNo, argGjjong);
        }

        public int Insert(HIC_SUNAP_WORK item)
        {
            return hicSunapWorkRepository.Insert(item);
        }

        public int GetCountbyPaNoGjJong(long argPano, string argGjjong)
        {
            return hicSunapWorkRepository.GetCountbyPaNoGjJong(argPano, argGjjong);
        }

        public HIC_SUNAP_WORK Read_Hic_Sunap_Work(long argPANO, string argJONG, string argSUDATE1, string argSUDATE2)
        {
            return hicSunapWorkRepository.Read_Hic_Sunap_Work(argPANO, argJONG, argSUDATE1, argSUDATE2);
        }

        public bool DeleteByPanoSuDate(string jEPDATE, long pANO, string argJong = "")
        {
            try
            {
                hicSunapWorkRepository.DeleteByPanoSuDate(jEPDATE, pANO, argJong);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
