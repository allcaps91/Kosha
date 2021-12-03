namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuCancerNewService
    {
        
        private HicJepsuCancerNewRepository hicJepsuCancerNewRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuCancerNewService()
        {
			this.hicJepsuCancerNewRepository = new HicJepsuCancerNewRepository();
        }

        public List<HIC_JEPSU_CANCER_NEW> GetItembyJepDate(string strFrDate, string strToDate, string strJob)
        {
            return hicJepsuCancerNewRepository.GetItembyJepDate(strFrDate, strToDate, strJob);
        }

        public HIC_JEPSU_CANCER_NEW GetItembyPaNoGjYear(long argPano, string strYear)
        {
            return hicJepsuCancerNewRepository.GetItembyPaNoGjYear(argPano, strYear);
        }

        public List<HIC_JEPSU_CANCER_NEW> GetItembyJepDateMirNo(string argFrDate, string argToDate, long argMirno, string strJong)
        {
            return hicJepsuCancerNewRepository.GetItembyJepDateMirNo(argFrDate, argToDate, argMirno, strJong);
        }

        public List<HIC_JEPSU_CANCER_NEW> GetItembyJepDateJongGMirNo(string sJepDate, string sJong, long argMirno)
        {
            return hicJepsuCancerNewRepository.GetItembyJepDateJongGMirNo(sJepDate, sJong, argMirno);
        }

        public List<HIC_JEPSU_CANCER_NEW> GetItembyJepDateMirNo2(string argFrDate, string argToDate, long argMirno, string strJong)
        {
            return hicJepsuCancerNewRepository.GetItembyJepDateMirNo2(argFrDate, argToDate, argMirno, strJong);
        }

        public List<HIC_JEPSU_CANCER_NEW> GetItemWrtNoInbyJepDateMirNo(string strFrDate, string strToDate, long nMirNo)
        {
            return hicJepsuCancerNewRepository.GetItemWrtNoInbyJepDateMirNo(strFrDate, strToDate, nMirNo);
        }
    }
}
